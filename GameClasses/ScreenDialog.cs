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
    public class ScreenDialog : Screen
    {
        public Dialog dialogType = Dialog.Default;
        public ObjType speakerType;
        public String dialogString;
        ScreenRec background = new ScreenRec();
        float overlayAlpha = 0.0f;
        Boolean fadeBkg = false;


        public ScreenDialog(Dialog Dialog)
        {
            this.name = "Dialog Screen";
            dialogType = Dialog;
        }

        public override void LoadContent()
        {
            background.alpha = 0.0f;
            background.fadeInSpeed = 0.05f; //slower closing fade


            #region Based on DialogType, set the speaker and dialog text

            //assume guide is speaker w/ default dialog
            speakerType = ObjType.VendorStory; 
            dialogString = "this is the default guide text...";

            //get specific dialog
            if (dialogType == Dialog.GameSaved)
            { dialogString = "I have successfully saved the current game."; fadeBkg = true; }
            else if (dialogType == Dialog.GameCreated)
            { dialogString = "I have created a new game for you."; fadeBkg = true; }
            else if (dialogType == Dialog.GameLoaded)
            { dialogString = "I have loaded the selected game file."; fadeBkg = true; }
            else if (dialogType == Dialog.GameNotFound)
            {
                dialogString = "the selected game file was not found. I have saved your current game to the\n";
                dialogString += "selected game slot instead."; fadeBkg = true;
            }
            else if (dialogType == Dialog.GameLoadFailed)
            {
                dialogString = "Oh no! I'm terribly sorry, but there was a problem loading this game file...\n";
                dialogString += "The data may be corrupted."; fadeBkg = true;
            }
            else if (dialogType == Dialog.GameAutoSaved)
            { dialogString = "I've successfully loaded your last autosaved game."; fadeBkg = true; }

            #endregion


            Widgets.Dialog.Reset(16 * 9, 16 * 12);
            //display the dialog
            Widgets.Dialog.DisplayDialog(speakerType, dialogString);
            //play the opening soundFX
            Assets.Play(Assets.sfxInventoryOpen);
            displayState = DisplayState.Opening;
        }

        public override void HandleInput(GameTime GameTime)
        {   //exit this screen upon start or b button press
            if (Functions_Input.IsNewButtonPress(Buttons.Start) ||
                Functions_Input.IsNewButtonPress(Buttons.B) ||
                Functions_Input.IsNewButtonPress(Buttons.A))
            {
                Assets.Play(Assets.sfxInventoryClose);
                displayState = DisplayState.Closing;
            }
        }

        public override void Update(GameTime GameTime)
        {

            #region Handle Screen State

            if (displayState == DisplayState.Opening)
            {
                if (fadeBkg)
                {
                    //fade background in
                    background.alpha += background.fadeInSpeed;
                    if (background.alpha >= 1.0f)
                    {
                        background.alpha = 1.0f;
                        displayState = DisplayState.Opened;
                    }
                }
                else { displayState = DisplayState.Opened; }
            }
            else if (displayState == DisplayState.Closing)
            {
                if (fadeBkg)
                {
                    //fade overlay in
                    overlayAlpha += background.fadeInSpeed;
                    if (overlayAlpha >= 1.0f)
                    {
                        overlayAlpha = 1.0f;
                        displayState = DisplayState.Closed;
                    }
                }
                else { displayState = DisplayState.Closed; }
            }
            else if (displayState == DisplayState.Closed)
            {   //exit all screens, restart game
                if (dialogType == Dialog.GameCreated ||
                    dialogType == Dialog.GameLoaded ||
                    dialogType == Dialog.GameNotFound ||
                    dialogType == Dialog.GameAutoSaved)
                { ScreenManager.StartGame(); }
                //or simply exit this screen
                else { ScreenManager.RemoveScreen(this); }
            }

            #endregion


            Widgets.Dialog.Update();
            //if we can update the dungeon screen, do so
            if (Functions_Dungeon.dungeonScreen != null)
            { Functions_Dungeon.dungeonScreen.Update(GameTime); }
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            //draw background first
            ScreenManager.spriteBatch.Draw(Assets.dummyTexture,
                background.rec, Assets.colorScheme.overlay * background.alpha);
            Widgets.Dialog.Draw();
            //draw overlay last
            ScreenManager.spriteBatch.Draw(Assets.dummyTexture,
                background.rec, Assets.colorScheme.overlay * overlayAlpha);
            ScreenManager.spriteBatch.End();
        }

    }
}