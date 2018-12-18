using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace DungeonRun
{
    public class ScreenTitle : Screen
    {
        ScreenRec overlay = new ScreenRec();
        ComponentSprite background;
        static MenuWindow window;
        ComponentSprite title;
        int i;

        public List<ComponentText> labels;
        List<MenuItem> menuItems;
        //MenuItem contGame = new MenuItem();
        MenuItem newGame = new MenuItem();
        MenuItem loadGame = new MenuItem();
        MenuItem quitGame = new MenuItem();


        //these point to a menuItem
        public MenuItem currentlySelected;
        public MenuItem previouslySelected;
        //simply visually tracks which menuItem is selected
        public ComponentSprite selectionBox;



        public ScreenTitle()
        {
            this.name = "TitleScreen";

            background = new ComponentSprite(Assets.titleBkgSheet,
                new Vector2(640 / 2, 360 / 2), new Byte4(0, 0, 0, 0), new Point(640, 360));

            title = new ComponentSprite(Assets.bigTextSheet,
                new Vector2(583 - 256, 200-32-15), //center
                new Byte4(0, 0, 0, 0),
                new Point(16 * 16, 16 * 4));
            window = new MenuWindow(
                new Point(16 * 15 + 8, 16 * 12 + 8),
                new Point(16 * 9 + 8, 16 * 4),
                "Main Menu");
            


            #region Create the menuItems

            menuItems = new List<MenuItem>();
            //set the menuItem data
            Functions_MenuItem.SetType(MenuItemType.OptionsNewGame, newGame);
            Functions_MenuItem.SetType(MenuItemType.OptionsSandBox, loadGame);
            Functions_MenuItem.SetType(MenuItemType.OptionsQuitGame, quitGame);

            //add the menuItems to the menuItems list
            menuItems.Add(newGame);
            menuItems.Add(loadGame);
            menuItems.Add(quitGame);

            //set the menuItem's neighbors
            Functions_MenuItem.SetNeighbors(menuItems, 3);

            #endregion


            #region Place the menuItems

            //row 1
            newGame.compSprite.position.X = window.background.rec.X + 16;
            newGame.compSprite.position.Y = window.background.rec.Y + 24 + 8;
            Functions_MenuItem.PlaceMenuItem(loadGame, newGame, 48);
            Functions_MenuItem.PlaceMenuItem(quitGame, loadGame, 48);

            #endregion


            #region Create the labels

            labels = new List<ComponentText>();
            //row 1
            labels.Add(new ComponentText(Assets.font, "new\ngame",
                newGame.compSprite.position + new Vector2(11, -12),
                ColorScheme.textDark));
            labels.Add(new ComponentText(Assets.font, "sand\nbox",
                loadGame.compSprite.position + new Vector2(11, -12),
                ColorScheme.textDark));
            labels.Add(new ComponentText(Assets.font, "quit\ngame",
                quitGame.compSprite.position + new Vector2(11, -12),
                ColorScheme.textDark));

            #endregion


            //create the selectionBox
            selectionBox = new ComponentSprite(Assets.uiItemsSheet,
                new Vector2(0, 0),
                AnimationFrames.Ui_SelectionBox[0],
                new Point(16, 16));
        }

        public override void Open()
        {
            overlay.alpha = 6.0f;
            overlay.fadeInSpeed = 0.03f; //slower closing fade
            title.alpha = 0.0f;


            //reset window instance
            Functions_MenuWindow.ResetAndMove(window,
                16 * 15 + 8, 16 * 12 + 8, //XY position
                new Point(16 * 9 + 8, 16 * 4), "Main Menu");
            window.interior.displayState = DisplayState.Opening;
            displayState = DisplayState.Opening;

            //set the currently selected menuItem to the first inventory menuItem
            currentlySelected = menuItems[0];
            previouslySelected = menuItems[0];
            //play the title music
            Functions_Music.PlayMusic(Music.Title);
            //reset cheats + player data
            Flags.Reset();
            PlayerData.Reset();


            //pre-build an instance of a dungeon when game boots 
            //done to bypass JIT compilation on other platforms
            LevelSet.currentLevel = LevelSet.dungeon;
            Functions_Level.BuildLevel(LevelID.Forest_Dungeon);
            Functions_Level.ResetLevel(LevelSet.currentLevel);
            //pre-build a standard field level too
            LevelSet.currentLevel = LevelSet.field;
            Functions_Level.BuildLevel(LevelID.SkullIsland_Town);
            Functions_Level.ResetLevel(LevelSet.currentLevel);
            //the level hero loads into will be decided by overworld later on
        }

        public override void HandleInput(GameTime GameTime)
        {
            if (window.interior.displayState == DisplayState.Opened)
            {   //only allow input if the window has opened completely

                
                #region Handle MenuItem Selection via A button press

                if (Input.Player1.A & Input.Player1.A_Prev == false) 
                {
                    currentlySelected.compSprite.scale = 2.0f;

                    if (currentlySelected.type == MenuItemType.OptionsNewGame)
                    {   //create a new game
                        Screens.Dialog.SetDialog(AssetsDialog.GameCreated);
                        ScreenManager.AddScreen(Screens.Dialog);
                        PlayerData.timer.Restart(); //start timing game
                    }
                    else if (currentlySelected.type == MenuItemType.OptionsSandBox)
                    {   //create a new game, with cheats on
                        PlayerData.SetSandboxMode();
                        Screens.Dialog.SetDialog(AssetsDialog.GameCreated);
                        ScreenManager.AddScreen(Screens.Dialog);
                        PlayerData.timer.Restart(); //start timing game
                    }
                    else if (currentlySelected.type == MenuItemType.OptionsQuitGame)
                    {
                        displayState = DisplayState.Closing; //fadeout, remove screen
                    }

                    //handle exit soundEffect
                    if (currentlySelected.type == MenuItemType.OptionsQuitGame)
                    { Assets.Play(Assets.sfxQuit); }
                    else { Assets.Play(Assets.sfxMenuItem); }
                }

                #endregion


                #region Handle Movement between MenuItems

                //get the previouslySelected menuItem
                previouslySelected = currentlySelected;
                //check to see if the gamePad direction is a new direction - prevents rapid scrolling
                if (Input.Player1.direction != Input.Player1.direction_Prev)
                {
                    //this is a new direction, allow movement between menuItems
                    if (Input.Player1.direction == Direction.Right)
                    { currentlySelected = currentlySelected.neighborRight; }
                    else if (Input.Player1.direction == Direction.Left)
                    { currentlySelected = currentlySelected.neighborLeft; }
                    else if (Input.Player1.direction == Direction.Down)
                    { currentlySelected = currentlySelected.neighborDown; }
                    else if (Input.Player1.direction == Direction.Up)
                    { currentlySelected = currentlySelected.neighborUp; }

                    //check to see if we changed menuItems
                    if (previouslySelected != currentlySelected)
                    {
                        Assets.Play(Assets.sfxTextLetter);
                        previouslySelected.compSprite.scale = 1.0f;
                        selectionBox.scale = 2.0f;
                    }
                }

                #endregion


            }
        }






        public override void Update(GameTime GameTime)
        {


            #region Handle Screen State

            if (displayState == DisplayState.Opening)
            {   //fade overlay out
                overlay.fadeState = FadeState.FadeOut;
                Functions_ScreenRec.Fade(overlay);
                if (overlay.fadeState == FadeState.FadeComplete)
                {
                    displayState = DisplayState.Opened;
                    Assets.Play(Assets.sfxWindowOpen);
                }
            }
            else if (displayState == DisplayState.Opened)
            {   //open the window
                Functions_MenuWindow.Update(window);
                //fade title in after bkg fade in
                if (title.alpha < 1.0f) { title.alpha += 0.03f; }
                else { title.alpha = 1.0f; }
            }
            else if (displayState == DisplayState.Closing)
            {   //fade overlay in
                overlay.fadeState = FadeState.FadeIn;
                Functions_ScreenRec.Fade(overlay);
                if (overlay.fadeState == FadeState.FadeComplete)
                { displayState = DisplayState.Closed; }
            }
            else if (displayState == DisplayState.Closed)
            { ScreenManager.game.Exit(); }

            #endregion


            if (window.interior.displayState == DisplayState.Opened)
            {   //pulse the selectionBox alpha
                if (selectionBox.alpha >= 1.0f) { selectionBox.alpha = 0.1f; }
                else { selectionBox.alpha += 0.025f; }
                //match the position of the selectionBox to the currently selected menuItem
                selectionBox.position = currentlySelected.compSprite.position;
                //scale the selectionBox down to 1.0
                if (selectionBox.scale > 1.0f) { selectionBox.scale -= 0.07f; }
                else { selectionBox.scale = 1.0f; }
                //scale currently selected menuItem back down to 1.0
                Functions_Animation.ScaleSpriteDown(currentlySelected.compSprite);
            }
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            Functions_Draw.Draw(background);
            Functions_Draw.Draw(title);
            Functions_Draw.Draw(window);
            if (window.interior.displayState == DisplayState.Opened)
            {
                for (i = 0; i < menuItems.Count; i++)
                { Functions_Draw.Draw(menuItems[i].compSprite); }
                for (i = 0; i < labels.Count; i++)
                { Functions_Draw.Draw(labels[i]); }
                Functions_Draw.Draw(selectionBox);
            }
            Functions_Draw.Draw(overlay);
            ScreenManager.spriteBatch.End();
        }

    }
}