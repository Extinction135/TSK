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
    public class ScreenLoadSaveNew : Screen
    {
        ScreenRec background = new ScreenRec();
        static MenuWindow window;
        LoadSaveNewState screenState;
        ExitAction exitAction;

        /*
        //these point to a menuItem
        public MenuItem currentlySelected;
        public MenuItem previouslySelected;
        //simply visually tracks which menuItem is selected
        public ComponentSprite selectionBox;
        */

        public ScreenLoadSaveNew(LoadSaveNewState State)
        {
            screenState = State;
            this.name = "ScreenLoadSaveNew";
        }

        public override void LoadContent()
        {
            background.alpha = 0.0f;
            background.fadeInSpeed = 0.03f;

            window = new MenuWindow(new Point(16 * 13 + 8 + 4, 16 * 6),
                new Point(16 * 12 + 8, 16 * 11), "Default ");
            //setup window title based on screenState
            if (screenState == LoadSaveNewState.Load) { window.title.text = "Load "; }
            else if (screenState == LoadSaveNewState.New) { window.title.text = "New "; }
            else if (screenState == LoadSaveNewState.Save) { window.title.text = "Save "; }
            window.title.text += "Game";



            /*
            //set the currently selected menuItem to the first inventory menuItem
            currentlySelected = menuItems[0];
            previouslySelected = menuItems[0];
            //create the selectionBox
            selectionBox = new ComponentSprite(Assets.mainSheet,
                new Vector2(0, 0), new Byte4(15, 7, 0, 0),
                new Point(16, 16));
            */

            //open the screen
            displayState = DisplayState.Opening;
            //play the title music
            //Functions_Music.PlayMusic(Music.Title);
        }

        public override void HandleInput(GameTime GameTime)
        {

            //exitAction

            if (displayState == DisplayState.Opened)
            {   //exit this screen upon start or b button press
                if (Functions_Input.IsNewButtonPress(Buttons.B))
                {
                    Assets.Play(Assets.sfxInventoryClose);
                    displayState = DisplayState.Closing;
                    exitAction = ExitAction.ExitScreen;
                }


                /*
                //only allow input if the screen has opened completely
                if (Functions_Input.IsNewButtonPress(Buttons.Start) ||
                    Functions_Input.IsNewButtonPress(Buttons.A))
                {
                    currentlySelected.compSprite.scale = 2.0f;

                    //handle load/save/new

                    //handle soundEffect
                    if (currentlySelected.type == MenuItemType.OptionsQuitGame)
                    { Assets.Play(Assets.sfxInventoryClose); }
                    else { Assets.Play(Assets.sfxMenuItem); }
                }
                */



                /*
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
                */

            }
            

        }

        public override void Update(GameTime GameTime)
        {

            #region Handle Screen State

            if (displayState == DisplayState.Opening)
            {   //fade bkg in
                background.alpha += background.fadeInSpeed;
                if (background.alpha >= 0.8f)
                {
                    background.alpha = 0.8f;
                    displayState = DisplayState.Opened;
                }
            }
            else if (displayState == DisplayState.Closing)
            {   //fade bkg out
                background.alpha -= background.fadeOutSpeed;
                if (background.alpha <= 0.0f)
                {
                    background.alpha = 0.0f;
                    displayState = DisplayState.Closed;
                }
            }
            else if (displayState == DisplayState.Closed)
            {
                if (exitAction == ExitAction.ExitScreen)
                {
                    //Functions_Backend.LoadPlayerData(); //load autoSave data
                    //ScreenManager.ExitAndLoad(new ScreenOverworld());
                    ScreenManager.RemoveScreen(this);
                }
            }

            #endregion

            //open the window
            Functions_MenuWindow.Update(window);

            /*
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
            */
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            ScreenManager.spriteBatch.Draw(Assets.dummyTexture,
                background.rec, Assets.colorScheme.overlay * background.alpha);
            Functions_Draw.Draw(window);

            if (window.interior.displayState == DisplayState.Opened)
            {
                /*
                for (i = 0; i < menuItems.Count; i++)
                { Functions_Draw.Draw(menuItems[i].compSprite); }
                for (i = 0; i < labels.Count; i++)
                { Functions_Draw.Draw(labels[i]); }
                Functions_Draw.Draw(selectionBox);
                */
            }

            ScreenManager.spriteBatch.End();
        }

    }
}