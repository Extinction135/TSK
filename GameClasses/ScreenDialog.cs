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
        public List<Dialog> dialogs;
        public int dialogIndex = 0;
        public ObjType speakerType;
        public String dialogString;

        ScreenRec background = new ScreenRec();
        ScreenRec foreground = new ScreenRec();
        Boolean exitToOverworld = false;



        public ScreenDialog(List<Dialog> Dialogs)
        {
            dialogs = Dialogs;
        }

        public override void LoadContent()
        {
            this.name = "Dialog Screen";
            foreground.fadeInSpeed = 0.05f;
            background.fadeOutSpeed = 0.05f;

            //pass dialog[0] to dialog widget for display
            DisplayDialog(dialogs[0]);
            dialogIndex = 1; //target next dialog

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
                    if (dialogIndex >= dialogs.Count)
                    {   //no more dialogs, close dialog screen
                        Assets.Play(Assets.sfxWindowClose);
                        displayState = DisplayState.Closing;
                        Functions_MenuWindow.Close(Widgets.Dialog.window);
                    }
                    else
                    {   //display the next dialog
                        DisplayDialog(dialogs[dialogIndex]);
                    }
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
            {   
                if(exitToOverworld)
                {   
                    //load overworld map, starting at shop
                    Level.ID = LevelID.Colliseum;
                    ScreenManager.ExitAndLoad(new ScreenOverworld());
                }
                else //or simply exit this screen
                { ScreenManager.RemoveScreen(this); }
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



        public void DisplayDialog(Dialog Dialog)
        {   //reset & open the dialog widget
            Widgets.Dialog.Reset(16 * 9, 16 * 12);
            Widgets.Dialog.DisplayDialog(Dialog.speaker, Dialog.title, Dialog.text);
            Assets.Play(Dialog.sfx);
            background.fade = Dialog.fadeBackgroundIn;
            foreground.fade = Dialog.fadeForegroundIn;
            exitToOverworld = Dialog.exitToOverworld;
            displayState = DisplayState.Opening;
            dialogIndex++; //track dialog count
        }

    }
}