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



        public ScreenDialog(Dialog Dialog)
        {
            this.name = "Dialog Screen";
            dialogType = Dialog;
        }

        public override void LoadContent()
        {

            #region Based on DialogType, set the speaker and dialog text

            //assume guide is speaker w/ default dialog
            speakerType = ObjType.VendorStory; 
            dialogString = "this is the default guide text...";

            //get specific dialog
            if (dialogType == Dialog.GameSaved)
            { dialogString = "your current game has been successfully saved."; }
            else if (dialogType == Dialog.GameCreated)
            { dialogString = "you have created a new game save file."; }
            else if (dialogType == Dialog.GameLoaded)
            { dialogString = "your selected game file has been loaded."; }

            #endregion


            displayState = DisplayState.Opening;
            Widgets.Dialog.Reset(16 * 9, 16 * 12);
            //display the dialog
            Widgets.Dialog.DisplayDialog(speakerType, dialogString);
            //play the opening soundFX
            Assets.Play(Assets.sfxInventoryOpen);
        }

        public override void HandleInput(GameTime GameTime)
        {   //exit this screen upon start or b button press
            if (Functions_Input.IsNewButtonPress(Buttons.Start) ||
                Functions_Input.IsNewButtonPress(Buttons.B) ||
                Functions_Input.IsNewButtonPress(Buttons.A))
            {
                Assets.Play(Assets.sfxInventoryClose);
                ScreenManager.RemoveScreen(this);
            }
        }

        public override void Update(GameTime GameTime)
        {
            Widgets.Dialog.Update();
            //if we can update the dungeon screen, do so
            if (Functions_Dungeon.dungeonScreen != null)
            { Functions_Dungeon.dungeonScreen.Update(GameTime); }
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            Widgets.Dialog.Draw();
            ScreenManager.spriteBatch.End();
        }

    }
}