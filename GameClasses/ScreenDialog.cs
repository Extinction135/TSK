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
        ScreenRec foreground = new ScreenRec();



        public ScreenDialog(Dialog Dialog)
        {
            this.name = "Dialog Screen";
            dialogType = Dialog;
        }

        public override void LoadContent()
        {
            background.fade = false;
            foreground.fade = false;


            #region Based on DialogType, set the speaker and dialog text

            //assume guide is speaker w/ default dialog
            speakerType = ObjType.VendorStory; 
            dialogString = "this is the default guide text...";

            //get specific dialog
            if (dialogType == Dialog.GameSaved)
            {
                dialogString = "I have successfully saved the current game.";
                background.fade = true; foreground.fade = false;
            }
            else if (dialogType == Dialog.GameCreated)
            {
                dialogString = "I have created a new game for you.";
                background.fade = true; foreground.fade = true;
            }
            else if (dialogType == Dialog.GameLoaded)
            {
                dialogString = "I have loaded the selected game file.";
                background.fade = true; foreground.fade = true;
            }
            else if (dialogType == Dialog.GameNotFound)
            {
                dialogString = "the selected game file was not found. I have saved your current game to the\n";
                dialogString += "selected game slot instead.";
                background.fade = true; foreground.fade = true;
            }
            else if (dialogType == Dialog.GameLoadFailed)
            {
                dialogString = "Oh no! I'm terribly sorry, but there was a problem loading this game file...\n";
                dialogString += "The data may be corrupted.";
                background.fade = true; foreground.fade = false;
            }
            else if (dialogType == Dialog.GameAutoSaved)
            {
                dialogString = "I've successfully loaded your last autosaved game.";
                background.fade = true; foreground.fade = true;
            }

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
                if (background.fade)
                {   //fade background in
                    background.fadeState = FadeState.FadeIn;
                    Functions_ScreenRec.Fade(background);
                    if (background.fadeState == FadeState.FadeComplete)
                    { displayState = DisplayState.Opened; }
                }   //else skip any fading
                else { displayState = DisplayState.Opened; }
            }
            else if (displayState == DisplayState.Closing)
            {
                if (foreground.fade)
                {   //fade foreground in
                    foreground.fadeState = FadeState.FadeIn;
                    Functions_ScreenRec.Fade(foreground);
                    if (foreground.fadeState == FadeState.FadeComplete)
                    { displayState = DisplayState.Closed; }
                }
                else
                {   //if we don't fade foreground in, then fade background out
                    background.fadeState = FadeState.FadeOut;
                    Functions_ScreenRec.Fade(background);
                    if (background.fadeState == FadeState.FadeComplete)
                    { displayState = DisplayState.Closed; }
                    //also, close the dialog widget
                }
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
            //draw foreground last
            ScreenManager.spriteBatch.Draw(Assets.dummyTexture,
                foreground.rec, Assets.colorScheme.overlay * foreground.alpha);
            ScreenManager.spriteBatch.End();
        }

    }
}