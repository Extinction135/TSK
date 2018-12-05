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
        public InteractiveType speakerType;
        public String dialogString;

        ScreenRec background = new ScreenRec();
        ScreenRec foreground = new ScreenRec();

        ExitAction exitAction = ExitAction.ExitScreen;
        






        public ScreenDialog()
        {
            this.name = "Dialog Screen";
            dialogs = AssetsDialog.Guide; //default value
        }

        public override void Open()
        {
            foreground.fadeInSpeed = 0.05f;
            background.fadeOutSpeed = 0.05f;
            foreground.alpha = 0.0f;
            background.alpha = 0.0f;

            //pass dialog[0] to dialog widget for display
            DisplayDialog(dialogs[0]);
            dialogIndex = 1; //target next dialog

            //display hero's current animation 
            //he may of been set into an animation just as dialog screen was created
            Functions_Actor.SetAnimationGroup(Pool.hero);
            Functions_Actor.SetAnimationDirection(Pool.hero);
            Functions_Animation.Animate(Pool.hero.compAnim, Pool.hero.compSprite);


            //reset exit action to just exit this dialog screen
            exitAction = ExitAction.ExitScreen;

            if (dialogs == AssetsDialog.GameCreated)
            { exitAction = ExitAction.GameCreated; }

            else if (
                dialogs == AssetsDialog.Enter_ForestDungeon
                || dialogs == AssetsDialog.Enter_MountainDungeon
                || dialogs == AssetsDialog.Enter_SwampDungeon
                )
            { exitAction = ExitAction.Dungeon; }

            else if(dialogs == AssetsDialog.Enter_Colliseum)
            { exitAction = ExitAction.Field; }
        }

        public override void HandleInput(GameTime GameTime)
        {   //force player to wait for the dialog to complete
            if (Widgets.Dialog.dialogDisplayed)
            {

                #region Dialogs with A/B Choices + Diff Outcomes

                if (//game exit dialogs
                    dialogs == AssetsDialog.AreYouSure
                    //dungeon entrnce dialogs
                    || dialogs == AssetsDialog.Enter_ForestDungeon
                    || dialogs == AssetsDialog.Enter_MountainDungeon
                    || dialogs == AssetsDialog.Enter_SwampDungeon
                    //coliseum entrance dialogs
                    || dialogs == AssetsDialog.Enter_Colliseum
                    )
                {

                    //process dialog forward 
                    if (Input.Player1.A & Input.Player1.A_Prev == false)
                    {   //exiting to title from inventory + dialog screens
                        if (dialogs == AssetsDialog.AreYouSure)
                        {
                            exitAction = ExitAction.Title; //goto title
                            Assets.Play(Assets.sfxQuit); //play quit sfx
                            foreground.fade = true; //fade foreground in
                            //ExitDialog(); //this plays window close sfx, so bypass
                            displayState = DisplayState.Closing;
                            Functions_MenuWindow.Close(Widgets.Dialog.window);
                        }


                        #region Dungeon Entrance Dialogs

                        else if (dialogs == AssetsDialog.Enter_ForestDungeon)
                        {
                            LevelSet.dungeon.ID = LevelID.Forest_Dungeon;
                            ExitDialog(); Assets.Play(Assets.sfxEnterDungeon);
                        }

                        else if (dialogs == AssetsDialog.Enter_MountainDungeon)
                        {
                            LevelSet.dungeon.ID = LevelID.Mountain_Dungeon;
                            ExitDialog(); Assets.Play(Assets.sfxEnterDungeon);
                        }

                        else if (dialogs == AssetsDialog.Enter_SwampDungeon)
                        {
                            LevelSet.dungeon.ID = LevelID.Swamp_Dungeon;
                            ExitDialog(); Assets.Play(Assets.sfxEnterDungeon);
                        }

                        #endregion


                        #region Coliseum Entrances

                        //enter colliseum from exterior colliseum field level
                        else if (dialogs == AssetsDialog.Enter_Colliseum)
                        {
                            LevelSet.field.ID = LevelID.SkullIsland_ColliseumPit;
                            ExitDialog(); Assets.Play(Assets.sfxEnterDungeon);
                        }

                        #endregion

                    }


                    if(Input.Player1.B & Input.Player1.B_Prev == false)
                    {
                        exitAction = ExitAction.ExitScreen; //reset exit action
                        ExitDialog(); //exit dialog
                    }
                }

                #endregion


                #region Iterative Dialogs (sequential) - loading, reading, etc...
                
                else
                {
                    if (Input.Player1.A & Input.Player1.A_Prev == false)
                    {
                        if (dialogIndex >= dialogs.Count)
                        {   //no more dialogs, close dialog screen
                            ExitDialog();
                        }
                        else
                        {   //display the next dialog
                            DisplayDialog(dialogs[dialogIndex]);
                        }
                    }
                }

                #endregion

            }
        }

        public override void Update(GameTime GameTime)
        {

            #region Opening

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

            #endregion


            #region Closing

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

            #endregion


            #region Closed

            else if (displayState == DisplayState.Closed)
            {   
                //from lsn screen to overworld
                if(exitAction == ExitAction.GameCreated)
                {   //exit all screens, start new game
                    ScreenManager.ExitAndLoad(Screens.Overworld_ShadowKing);
                }
                //from inventory screen to title
                else if(exitAction == ExitAction.Title)
                {   //exit all screens, load overworld level
                    ScreenManager.ExitAndLoad(Screens.Title);
                }

                //from field to field (ex: colliseum)
                else if (exitAction == ExitAction.Field)
                {   //close the level screen, exiting to field level
                    Functions_Level.CloseLevel(ExitAction.Field);
                    ScreenManager.RemoveScreen(this);
                }
                //from field to dungeon
                else if(exitAction == ExitAction.Dungeon)
                {   //close the level screen, exiting to dungeon level
                    Functions_Level.CloseLevel(ExitAction.Dungeon);
                    ScreenManager.RemoveScreen(this);
                }
                else
                {   //or simply exit this screen
                    ScreenManager.RemoveScreen(this);
                }
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
            displayState = DisplayState.Opening;
            dialogIndex++; //track dialog count
        }

        public void ExitDialog()
        {
            Assets.Play(Assets.sfxWindowClose);
            displayState = DisplayState.Closing;
            Functions_MenuWindow.Close(Widgets.Dialog.window);
        }

        public void SetDialog(List<Dialog> Dialogs)
        {
            dialogs = Dialogs;
        }


    }
}