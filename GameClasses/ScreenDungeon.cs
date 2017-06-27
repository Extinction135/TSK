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
    public class ScreenDungeon : Screen
    {
        public ScreenRec overlay = new ScreenRec();
        public ExitAction exitAction;



        public ScreenDungeon() { this.name = "DungeonScreen"; }

        public override void LoadContent()
        {
            overlay.alpha = 1.0f;
            overlay.fadeOutSpeed = 0.025f;

            Functions_Dungeon.Initialize(this);
            Functions_Dungeon.BuildDungeon();
            //ActorFunctions.SetType(Pool.hero, Actor.Type.Blob);
            //open the screen
            displayState = DisplayState.Opening;
        }

        public override void HandleInput(GameTime GameTime)
        {
            if (displayState == DisplayState.Opened)
            {   //input maps only when screen is open

                #region Handle AI Input

                if (Flags.ProcessAI)
                {   //process AI for up to 3 actors per frame
                    Functions_Ai.SetActorInput();
                    Functions_Ai.SetActorInput();
                    Functions_Ai.SetActorInput();
                }

                #endregion


                #region Handle Player Input

                //map player input to hero
                Functions_Input.MapPlayerInput(Pool.hero.compInput);
                //open the inventory screen if player presses start button
                if (Functions_Input.IsNewButtonPress(Buttons.Start))
                { ScreenManager.AddScreen(new ScreenInventory()); }

                #endregion


                #region Handle Debug Keyboard / Mouse Input

                if (Flags.Debug)
                {   //if the game is in debug mode, dump info on clicked actor/obj
                    if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
                    { Functions_Debug.Inspect(); }
                    //toggle the paused boolean
                    if (Functions_Input.IsNewKeyPress(Keys.Space))
                    { if (Flags.Paused) { Flags.Paused = false; } else { Flags.Paused = true; } }
                    Functions_Debug.HandleDebugMenuInput();
                }

                #endregion

            }
            else
            {   //screen is not opened, prevent all input mapping
                Functions_Pool.ResetActorPoolInput(); //clear enemy input
                Functions_Input.ResetInputData(Pool.hero.compInput); //clear hero input
            }
        }

        public override void Update(GameTime GameTime)
        {
            Timing.Reset();

            if(!Flags.Paused)
            {

                #region Handle Screen State

                if(displayState == DisplayState.Opening) //fade overlay to 0
                {
                    overlay.alpha -= overlay.fadeOutSpeed;
                    if (overlay.alpha <= 0.0f)
                    {
                        overlay.alpha = 0.0f;
                        displayState = DisplayState.Opened;
                    }
                }
                else if (displayState == DisplayState.Opened)
                {   //set overlay alpha to a negative value
                    overlay.alpha = -1.5f; //delays closing state a bit
                }   //delay gives player time to understand what's happening
                else if (displayState == DisplayState.Closing) //fade overlay to 1.0
                {   //set the fadeInSpeed & overlay alpha based on the exitAction
                    if (exitAction == ExitAction.Overworld)
                    {   //exits fade in immediately, and much faster
                        overlay.fadeInSpeed = 0.05f;
                        if (overlay.alpha < 0.0f) { overlay.alpha = 0.0f; }
                    }
                    else { overlay.fadeInSpeed = 0.015f; } //victory/defeat fades in much slower

                    overlay.alpha += overlay.fadeInSpeed;
                    if (overlay.alpha >= 1.0f)
                    {
                        overlay.alpha = 1.0f;
                        displayState = DisplayState.Closed;
                    }
                }
                else if (displayState == DisplayState.Closed)
                {
                    DungeonRecord.timer.Stop();
                    //save the player's current game progress
                    Functions_Backend.SaveGame(GameFile.AutoSave);
                    //handle exit action
                    if (exitAction == ExitAction.Summary)
                    { ScreenManager.ExitAndLoad(new ScreenSummary()); }
                    else if(exitAction == ExitAction.Overworld)
                    { ScreenManager.ExitAndLoad(new ScreenOverworld()); }
                    else if(exitAction == ExitAction.Title)
                    { ScreenManager.ExitAndLoad(new ScreenTitle()); }
                }

                #endregion


                //if the screen is closed, don't play any sounds or do any work
                if (displayState != DisplayState.Closed)
                {   //update and move actors, objects, projectiles, and camera
                    Functions_Pool.Update();
                    Functions_Collision.CheckDungeonRoomCollisions();
                    Functions_WorldUI.Update();
                    //track camera to hero
                    Camera2D.targetPosition = Pool.hero.compSprite.position;
                    Functions_Camera2D.Update(GameTime);
                }
            }

            Timing.stopWatch.Stop();
            Timing.updateTime = Timing.stopWatch.Elapsed;
        }

        public override void Draw(GameTime GameTime)
        {
            Timing.Reset();


            #region Draw gameworld from camera's view

            ScreenManager.spriteBatch.Begin(
                        SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend,
                        SamplerState.PointClamp,
                        null,
                        null,
                        null,
                        Camera2D.view
                        );
            Functions_Pool.Draw();
            if (Flags.DrawCollisions)
            {
                Functions_Draw.Draw(Input.cursorColl);
                Functions_Draw.Draw(Functions_Dungeon.dungeon);
                Functions_Draw.Draw(Functions_Interaction.interactionRec);
            }
            ScreenManager.spriteBatch.End();

            #endregion


            #region Draw UI, debug info + debug menu, & overlay

            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            Functions_WorldUI.Draw();
            if (Flags.Debug)
            {
                Functions_Draw.DrawDebugInfo();
                Functions_Draw.DrawDebugMenu();
            }
            ScreenManager.spriteBatch.Draw( Assets.dummyTexture, 
                overlay.rec, Assets.colorScheme.overlay * overlay.alpha);
            ScreenManager.spriteBatch.End();

            #endregion


            Timing.stopWatch.Stop();
            Timing.drawTime = Timing.stopWatch.Elapsed;
            Timing.totalTime = Timing.updateTime + Timing.drawTime;
        }

    }
}