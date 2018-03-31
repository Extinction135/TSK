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
            title = new ComponentSprite(Assets.bigTextSheet, 
                new Vector2(583 - 256, 200), //center
                new Byte4(0, 0, 0, 0), 
                new Point(16 * 16, 16 * 4));
            title.alpha = 0.0f;


            #region Create the menuItems

            menuItems = new List<MenuItem>();
            //set the menuItem data
            Functions_MenuItem.SetType(MenuItemType.OptionsContinue, contGame);
            Functions_MenuItem.SetType(MenuItemType.OptionsNewGame, newGame);
            Functions_MenuItem.SetType(MenuItemType.OptionsLoadGame, loadGame);
            Functions_MenuItem.SetType(MenuItemType.OptionsQuitGame, quitGame);
            Functions_MenuItem.SetType(MenuItemType.OptionsAudioCtrls, audioCtrls);
            Functions_MenuItem.SetType(MenuItemType.OptionsInputCtrls, inputCtrls);
            Functions_MenuItem.SetType(MenuItemType.OptionsVideoCtrls, videoCtrls);
            Functions_MenuItem.SetType(MenuItemType.OptionsGameCtrls, gameCtrls);
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
            selectionBox = new ComponentSprite(Assets.uiItemsSheet,
                new Vector2(0, 0), 
                AnimationFrames.Ui_SelectionBox[0],
                new Point(16, 16));
            //open the screen
            displayState = DisplayState.Opening;
            
            //play the title music
            Functions_Music.PlayMusic(Music.Title);
            //silently load autosave file
            Functions_Backend.LoadGame(GameFile.Game1, false);
            Functions_Backend.LoadGame(GameFile.Game2, false);
            Functions_Backend.LoadGame(GameFile.Game3, false);
            Functions_Backend.LoadGame(GameFile.AutoSave, false);
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
                        Functions_Backend.LoadGame(GameFile.AutoSave, true);
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
                    if (currentlySelected.type == MenuItemType.OptionsContinue)
                    { Assets.Play(Assets.sfxSelectFile); }
                    if (currentlySelected.type == MenuItemType.OptionsQuitGame)
                    { Assets.Play(Assets.sfxQuit); }
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
                {
                    displayState = DisplayState.Opened;
                    Assets.Play(Assets.sfxWindowOpen);
                }
            }
            else if (displayState == DisplayState.Opened)
            {   //open the window
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
                //scale currently selected menuItem back down to 1.0
                Functions_Animation.ScaleSpriteDown(currentlySelected.compSprite);

                //flicker title
                if (title.alpha >= 1.0f) { title.alpha = 0.85f; }
                else if (title.alpha < 0.85f) { title.alpha += 0.03f; }
                title.alpha += 0.005f;
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