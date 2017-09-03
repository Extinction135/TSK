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
            foreground.fadeInSpeed = 0.05f;
            background.fadeOutSpeed = 0.05f;

            //assume speaker is guide, assume no screen fading
            speakerType = ObjType.VendorStory; 
            background.fade = false;
            foreground.fade = false;
            

            #region Game Saved/Loaded/Created/etc Dialogs

            if (dialogType == Dialog.GameSaved)
            {   //returns to inventory screen upon close
                dialogString = "I have successfully saved the current game.";
                background.fade = true; foreground.fade = false;
            }
            else if (dialogType == Dialog.GameLoadFailed)
            {   //returns to previous screen (inventory or title) upon close
                dialogString = "Oh no! I'm terribly sorry, but there was a problem loading this game file...\n";
                dialogString += "The data is corrupted... I've overwritten the file with your current game.";
                background.fade = true; foreground.fade = false;
            }
            else if (dialogType == Dialog.GameCreated)
            {   //goes to overworld screen upon close
                dialogString = "I have created a new game for you.";
                background.fade = true; foreground.fade = true;
            }
            else if (dialogType == Dialog.GameLoaded)
            {   //goes to overworld screen upon close
                dialogString = "I have loaded the selected game file.";
                background.fade = true; foreground.fade = true;
            }
            else if (dialogType == Dialog.GameNotFound)
            {   //goes to overworld screen upon close
                dialogString = "the selected game file was not found. I have saved your current game to the\n";
                dialogString += "selected game slot instead.";
                background.fade = true; foreground.fade = true;
            }
            else if (dialogType == Dialog.GameAutoSaved)
            {   //goes to overworld screen upon close
                dialogString = "I've successfully loaded your last autosaved game.";
                background.fade = true; foreground.fade = true;
            }

            #endregion


            #region Dungeon Dialogs

            else if (dialogType == Dialog.DoesNotHaveKey)
            { dialogString = "This door is locked. You'll need a key to open it."; }
            else if (dialogType == Dialog.HeroGotKey)
            { dialogString = "You found the dungeon key. It can open any door."; }
            else if (dialogType == Dialog.HeroGotMap)
            {
                dialogString = "You found the dungeon map! This map reveals the location of all rooms.";
                dialogString += "\nPress the Left Shoulder button to view this dungeon map.";
            }
            //bottle capturing dialogs
            else if(dialogType == Dialog.BottleCant)
            {
                dialogString = "You caught a creature in your net!";
                dialogString += "\nUnfortunately, you can't fit this creature in a bottle.";
            }
            else if (dialogType == Dialog.BottleFull)
            {
                dialogString = "You caught a creature in your net!";
                dialogString += "\nUnfortunately, You don't have any empty bottles to put this creature in.";
            }
            else if (dialogType == Dialog.BottleSuccess)
            {
                dialogString = "You caught a creature in your net!";
                dialogString += "\nYou have successfully captured this creature in an empty bottle.";
            }

            #endregion


            #region Editor Dialogs

            else if (dialogType == Dialog.CantAddKeyChest)
            {
                dialogString = "You cannot add a key chest to a non-key room!";
                background.fade = true;
            }
            else if (dialogType == Dialog.CantAddMapChest)
            {
                dialogString = "You cannot add a map chest to a non-hub room!";
                background.fade = true;
            }
            else if (dialogType == Dialog.CantAddMoreChests)
            {
                dialogString = "You cannot add more than 1 chest to a room!";
                background.fade = true;
            }

            #endregion


            else //default dialog
            {
                dialogString = "i'm the guide. in a future update i'll explain the story.";
                dialogString += "\nfor now, just have fun playtesting the game.";
            }

            //display the dialog
            Widgets.Dialog.Reset(16 * 9, 16 * 12);
            Widgets.Dialog.DisplayDialog(speakerType, dialogString);
            //play the opening soundFX
            Assets.Play(Assets.sfxWindowOpen);
            displayState = DisplayState.Opening;

            //display hero's current animation 
            //he may of been set into an animation just as dialog screen was created
            Functions_ActorAnimationList.SetAnimationGroup(Pool.hero);
            Functions_ActorAnimationList.SetAnimationDirection(Pool.hero);
            Functions_Animation.Animate(Pool.hero.compAnim, Pool.hero.compSprite);
        }

        public override void HandleInput(GameTime GameTime)
        {   //force player to wait for the dialog to complete
            if (Widgets.Dialog.dialogDisplayed)
            {   //exit this screen upon start or a/b button press
                if (Functions_Input.IsNewButtonPress(Buttons.Start) ||
                    Functions_Input.IsNewButtonPress(Buttons.B) ||
                    Functions_Input.IsNewButtonPress(Buttons.A))
                {
                    Assets.Play(Assets.sfxWindowClose);
                    displayState = DisplayState.Closing;
                    Functions_MenuWindow.Close(Widgets.Dialog.window);
                }
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
                else
                {   //make sure dialog widget has enough time to open
                    if (Widgets.Dialog.window.background.displayState == DisplayState.Opened)
                    { displayState = DisplayState.Opened; }
                }
            }
            else if (displayState == DisplayState.Closing)
            {
                if (foreground.fade)
                {   //fade foreground in
                    foreground.fadeState = FadeState.FadeIn;
                    Functions_ScreenRec.Fade(foreground);
                }
                else
                {   //if we don't fade foreground in, then fade background out
                    background.fadeState = FadeState.FadeOut;
                    Functions_ScreenRec.Fade(background);
                }
                //make sure dialog widget has enough time to close
                if (Widgets.Dialog.window.background.displayState == DisplayState.Closed)
                { displayState = DisplayState.Closed; }
            }
            else if (displayState == DisplayState.Closed)
            {   //exit all screens, restart game
                if (dialogType == Dialog.GameCreated ||
                    dialogType == Dialog.GameLoaded ||
                    dialogType == Dialog.GameNotFound ||
                    dialogType == Dialog.GameAutoSaved)
                {   //load overworld map, starting at shop
                    Level.type = LevelType.Shop;
                    ScreenManager.ExitAndLoad(new ScreenOverworld());
                }
                //or simply exit this screen
                else { ScreenManager.RemoveScreen(this); }
            }

            #endregion


            Widgets.Dialog.Update();
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            Functions_Draw.Draw(background);
            Widgets.Dialog.Draw();
            Functions_Draw.Draw(foreground);
            ScreenManager.spriteBatch.End();
        }

    }
}