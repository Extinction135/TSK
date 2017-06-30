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
        TitleAnimated leftTitle;
        TitleAnimated rightTitle;
        int i;

        public List<ComponentText> labels;
        List<MenuItem> menuItems;
        MenuItem contGame = new MenuItem();
        MenuItem newGame = new MenuItem();
        MenuItem loadGame = new MenuItem();
        MenuItem quitGame = new MenuItem();
        MenuItem audioCtrls = new MenuItem();
        MenuItem inputCtrls = new MenuItem();
        MenuItem videoCtrls = new MenuItem();
        MenuItem gameCtrls = new MenuItem();

        //these point to a menuItem
        public MenuItem currentlySelected;
        public MenuItem previouslySelected;
        //simply visually tracks which menuItem is selected
        public ComponentSprite selectionBox;



        public ScreenTitle() { this.name = "TitleScreen"; }

        public override void LoadContent()
        {
            overlay.alpha = 6.0f;
            overlay.fadeInSpeed = 0.03f; //slower closing fade

            background = new ComponentSprite(Assets.titleBkgSheet,
                new Vector2(640/2, 360/2), new Byte4(0, 0, 0, 0), new Point(640, 360));
            window = new MenuWindow(new Point(16 * 13 + 8 + 4, 16 * 15),
                new Point(16 * 12 + 8, 16 * 5 + 8), "Main Menu");
            float yPos = 200;
            leftTitle = new TitleAnimated(
                new Vector2(-150, yPos),
                new Vector2(200-35, yPos),
                TitleText.Dungeon, 8);
            rightTitle = new TitleAnimated(
                new Vector2(640+25, yPos),
                new Vector2(320+35, yPos),
                TitleText.Run, 8);


            #region Create the menuItems

            menuItems = new List<MenuItem>();
            //set the menuItem data
            Functions_MenuItem.SetMenuItemData(MenuItemType.OptionsContinue, contGame);
            Functions_MenuItem.SetMenuItemData(MenuItemType.OptionsNewGame, newGame);
            Functions_MenuItem.SetMenuItemData(MenuItemType.OptionsLoadGame, loadGame);
            Functions_MenuItem.SetMenuItemData(MenuItemType.OptionsQuitGame, quitGame);
            Functions_MenuItem.SetMenuItemData(MenuItemType.OptionsAudioCtrls, audioCtrls);
            Functions_MenuItem.SetMenuItemData(MenuItemType.OptionsInputCtrls, inputCtrls);
            Functions_MenuItem.SetMenuItemData(MenuItemType.OptionsVideoCtrls, videoCtrls);
            Functions_MenuItem.SetMenuItemData(MenuItemType.OptionsGameCtrls, gameCtrls);
            //customize the continue game menuItem sprite
            contGame.compSprite.rotation = Rotation.Clockwise90;
            //add the menuItems to the menuItems list
            menuItems.Add(contGame);
            menuItems.Add(newGame);
            menuItems.Add(loadGame);
            menuItems.Add(quitGame);
            menuItems.Add(audioCtrls);
            menuItems.Add(inputCtrls);
            menuItems.Add(videoCtrls);
            menuItems.Add(gameCtrls);
            //set the menuItem's neighbors
            Functions_MenuItem.SetNeighbors(menuItems, 4);

            #endregion
            

            #region Place the menuItems

            //row 1
            contGame.compSprite.position.X = window.background.rec.X + 16;
            contGame.compSprite.position.Y = window.background.rec.Y + 24 + 8;
            Functions_MenuItem.PlaceMenuItem(newGame, contGame, 48);
            Functions_MenuItem.PlaceMenuItem(loadGame, newGame, 48);
            Functions_MenuItem.PlaceMenuItem(quitGame, loadGame, 48);
            //row 2
            audioCtrls.compSprite.position.X = contGame.compSprite.position.X;
            audioCtrls.compSprite.position.Y = contGame.compSprite.position.Y + 24;
            Functions_MenuItem.PlaceMenuItem(inputCtrls, audioCtrls, 48);
            Functions_MenuItem.PlaceMenuItem(videoCtrls, inputCtrls, 48);
            Functions_MenuItem.PlaceMenuItem(gameCtrls, videoCtrls, 48);

            #endregion


            #region Create the labels

            labels = new List<ComponentText>();
            //row 1
            labels.Add(new ComponentText(Assets.font, "con-\ntinue",
                contGame.compSprite.position + new Vector2(11, -12), 
                Assets.colorScheme.textDark));
            labels.Add(new ComponentText(Assets.font, "new\ngame",
                newGame.compSprite.position + new Vector2(11, -12),
                Assets.colorScheme.textDark));
            labels.Add(new ComponentText(Assets.font, "load\ngame",
                loadGame.compSprite.position + new Vector2(11, -12),
                Assets.colorScheme.textDark));
            labels.Add(new ComponentText(Assets.font, "quit\ngame",
                quitGame.compSprite.position + new Vector2(11, -12),
                Assets.colorScheme.textDark));
            //row 2
            labels.Add(new ComponentText(Assets.font, "audio\nctrls",
                audioCtrls.compSprite.position + new Vector2(11, -12),
                Assets.colorScheme.textDark));
            labels.Add(new ComponentText(Assets.font, "input\nctrls",
                inputCtrls.compSprite.position + new Vector2(11, -12),
                Assets.colorScheme.textDark));
            labels.Add(new ComponentText(Assets.font, "video\nctrls",
                videoCtrls.compSprite.position + new Vector2(11, -12),
                Assets.colorScheme.textDark));
            labels.Add(new ComponentText(Assets.font, "game\nctrls",
                gameCtrls.compSprite.position + new Vector2(11, -12),
                Assets.colorScheme.textDark));

            #endregion


            //set the currently selected menuItem to the first inventory menuItem
            currentlySelected = menuItems[0];
            previouslySelected = menuItems[0];
            //create the selectionBox
            selectionBox = new ComponentSprite(Assets.mainSheet,
                new Vector2(0, 0), new Byte4(15, 7, 0, 0),
                new Point(16, 16));
            //open the screen
            displayState = DisplayState.Opening;
            //play the title music
            Functions_Music.PlayMusic(Music.Title);
        }

        public override void HandleInput(GameTime GameTime)
        {
            if (displayState == DisplayState.Opened)
            {   //only allow input if the screen has opened completely
                if (Functions_Input.IsNewButtonPress(Buttons.Start) ||
                    Functions_Input.IsNewButtonPress(Buttons.A))
                {
                    currentlySelected.compSprite.scale = 2.0f;


                    #region Handle MenuItem Selection

                    if (currentlySelected.type == MenuItemType.OptionsContinue)
                    {
                        Functions_Backend.LoadGame(GameFile.AutoSave);
                    }
                    else if (currentlySelected.type == MenuItemType.OptionsNewGame)
                    {
                        ScreenManager.AddScreen(new ScreenLoadSaveNew(LoadSaveNewState.New));
                    }
                    else if (currentlySelected.type == MenuItemType.OptionsLoadGame)
                    {
                        ScreenManager.AddScreen(new ScreenLoadSaveNew(LoadSaveNewState.Load));
                    }
                    else if (currentlySelected.type == MenuItemType.OptionsQuitGame)
                    {
                        displayState = DisplayState.Closing; //fadeout, remove screen
                    }
                    else if (currentlySelected.type == MenuItemType.OptionsAudioCtrls)
                    {
                        //create audio ctrls screen
                    }
                    else if (currentlySelected.type == MenuItemType.OptionsInputCtrls)
                    {
                        //create input ctrls screen
                    }
                    else if (currentlySelected.type == MenuItemType.OptionsVideoCtrls)
                    {
                        //create video ctrls screen
                    }
                    else if (currentlySelected.type == MenuItemType.OptionsGameCtrls)
                    {
                        //create game ctrls screen
                    }

                    #endregion


                    //handle soundEffect
                    if (currentlySelected.type == MenuItemType.OptionsQuitGame)
                    { Assets.Play(Assets.sfxInventoryClose); }
                    else { Assets.Play(Assets.sfxMenuItem); }
                }
                //get the previouslySelected menuItem
                previouslySelected = currentlySelected;
                //check to see if the gamePad direction is a new direction - prevents rapid scrolling
                if (Input.gamePadDirection != Input.lastGamePadDirection)
                {
                    //this is a new direction, allow movement between menuItems
                    if (Input.gamePadDirection == Direction.Right)
                    { currentlySelected = currentlySelected.neighborRight; }
                    else if (Input.gamePadDirection == Direction.Left)
                    { currentlySelected = currentlySelected.neighborLeft; }
                    else if (Input.gamePadDirection == Direction.Down)
                    { currentlySelected = currentlySelected.neighborDown; }
                    else if (Input.gamePadDirection == Direction.Up)
                    { currentlySelected = currentlySelected.neighborUp; }

                    //check to see if we changed menuItems
                    if (previouslySelected != currentlySelected)
                    {
                        Assets.Play(Assets.sfxTextLetter);
                        previouslySelected.compSprite.scale = 1.0f;
                        selectionBox.scale = 2.0f;
                    }
                }
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
                { displayState = DisplayState.Opened; }
            }
            else if (displayState == DisplayState.Opened)
            {   //animate titles in
                Functions_TitleAnimated.AnimateMovement(leftTitle);
                Functions_TitleAnimated.AnimateMovement(rightTitle);
                //fade titles in
                if (leftTitle.compSprite.alpha < 1.0f)
                { leftTitle.compSprite.alpha += 0.05f; }
                if (rightTitle.compSprite.alpha < 1.0f)
                { rightTitle.compSprite.alpha += 0.05f; }
                //open the window
                Functions_MenuWindow.Update(window);
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


            if (displayState != DisplayState.Opening)
            {   //pulse the selectionBox alpha
                if (selectionBox.alpha >= 1.0f) { selectionBox.alpha = 0.1f; }
                else { selectionBox.alpha += 0.025f; }
                //match the position of the selectionBox to the currently selected menuItem
                selectionBox.position = currentlySelected.compSprite.position;
                //scale the selectionBox down to 1.0
                if (selectionBox.scale > 1.0f) { selectionBox.scale -= 0.07f; }
                else { selectionBox.scale = 1.0f; }
                //animate the currently selected menuItem - this scales it back down to 1.0
                Functions_Animation.Animate(currentlySelected.compAnim, currentlySelected.compSprite);
            }
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            Functions_Draw.Draw(background);
            Functions_Draw.Draw(leftTitle.compSprite);
            Functions_Draw.Draw(rightTitle.compSprite);
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