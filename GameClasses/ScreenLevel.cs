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
    public class ScreenLevel : Screen
    {
        public ScreenRec overlay = new ScreenRec();
        public ExitAction exitAction;



        public ScreenLevel() { this.name = "LevelScreen"; }

        public override void LoadContent()
        {
            overlay.alpha = 1.0f;
            overlay.fadeOutSpeed = 0.025f;
            //register this dungeon screen with Functions_Level
            Functions_Level.levelScreen = this;
            Functions_Level.BuildLevel();

            //load hero's actorType from SaveData
            Functions_Actor.SetType(Pool.hero, PlayerData.current.actorType);
            Functions_Hero.SetLoadout();

            //open the screen
            displayState = DisplayState.Opening;
        }

        public override void HandleInput(GameTime GameTime)
        {
            if (displayState == DisplayState.Opened)
            {   //input maps only when screen is open
                //Handle AI Input
                if (Flags.ProcessAI)
                {   //process AI for up to 3 actors per frame
                    Functions_Ai.SetActorInput();
                    Functions_Ai.SetActorInput();
                    Functions_Ai.SetActorInput();
                }
                //map player input to hero
                Functions_Input.MapPlayerInput(Pool.hero.compInput);
                if (Flags.EnableTopMenu) { Functions_TopMenu.HandleInput(); }
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
                {   //fade overlay out
                    overlay.fadeState = FadeState.FadeOut;
                    Functions_ScreenRec.Fade(overlay);
                    if (overlay.fadeState == FadeState.FadeComplete)
                    { displayState = DisplayState.Opened; }
                }
                else if (displayState == DisplayState.Opened)
                {   //set overlay alpha to a negative value
                    overlay.alpha = -1.5f; //delays closing state a bit
                }   //delay gives player time to understand what's happening
                else if (displayState == DisplayState.Closing) //fade overlay to 1.0
                {
                    if (overlay.alpha == -1.5f) //just began fading in overlay
                    {   //events that happen when hero exits dungeon/game
                        Functions_WorldUI.DisplayAutosave();
                        //handle how quickly the overlay fade in based on exitAction value
                        if (exitAction == ExitAction.Summary) //win/loss in dungeon
                        { overlay.fadeInSpeed = 0.015f; } //victory/defeat fades in slow
                        else
                        {   //quickly fade in if hero is exiting any level
                            overlay.fadeInSpeed = 0.05f;
                            overlay.alpha = 0.0f;
                        }
                    }
                    
                    //fade overlay in
                    overlay.fadeState = FadeState.FadeIn;
                    Functions_ScreenRec.Fade(overlay);
                    if (overlay.fadeState == FadeState.FadeComplete)
                    { displayState = DisplayState.Closed; }
                }
                else if (displayState == DisplayState.Closed)
                {
                    DungeonRecord.timer.Stop();
                    //handle exit action
                    if (exitAction == ExitAction.Summary || exitAction == ExitAction.ExitDungeon)
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
                    Functions_WorldUI.Update();
                    if (Flags.CameraTracksHero) //track camera to hero
                    {
                        Camera2D.tracks = false; //teleport follow hero
                        Camera2D.targetPosition = Pool.hero.compSprite.position;
                    }
                    else
                    {   //center camera to current room
                        Camera2D.targetPosition.X = Functions_Level.currentRoom.center.X;
                        Camera2D.targetPosition.Y = Functions_Level.currentRoom.center.Y + 8;
                        Camera2D.tracks = true; //move follow room to room
                    }
                    Functions_Camera2D.Update();
                }
            }

            Timing.stopWatch.Stop();
            Timing.updateTime = Timing.stopWatch.Elapsed;


            #region Calculate Average Update and Draw Times

            if(Flags.DrawUDT)
            {
                DebugInfo.frameCounter++;
                if (DebugInfo.frameCounter > DebugInfo.framesTotal)
                {   //reset the counter + total ticks
                    DebugInfo.frameCounter = 0;
                    DebugInfo.updateTicks = 0;
                    DebugInfo.drawTicks = 0;
                }
                else if (DebugInfo.frameCounter == DebugInfo.framesTotal)
                {   //calculate the average ticks
                    DebugInfo.updateAvg = DebugInfo.updateTicks / DebugInfo.framesTotal;
                    DebugInfo.drawAvg = DebugInfo.drawTicks / DebugInfo.framesTotal;
                }
                //collect tick times
                DebugInfo.updateTicks += Timing.updateTime.Ticks;
                DebugInfo.drawTicks += Timing.drawTime.Ticks;
            }

            #endregion


        }

        public override void Draw(GameTime GameTime)
        {
            Timing.Reset();


            #region Draw gameworld from camera's view

            ScreenManager.spriteBatch.Begin(
                        SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend,
                        SamplerState.PointClamp,
                        null, null, null, Camera2D.view);
            Functions_Pool.Draw();
            if (Flags.DrawCollisions)
            {
                Functions_Draw.Draw(Input.cursorColl);
                Functions_Draw.DrawLevel();
                Functions_Draw.Draw(Functions_Hero.interactionRec);
            }
            ScreenManager.spriteBatch.End();

            #endregion


            #region Draw UI, debug info + debug menu, & overlay

            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            Functions_WorldUI.Draw();

            //draw top menu + debug info
            if (Flags.EnableTopMenu) { Functions_TopMenu.Draw(); }
            if (Flags.DrawDebugInfo) { Functions_Debug.Draw(); }

            //draw the overlay rec last
            Functions_Draw.Draw(overlay);
            ScreenManager.spriteBatch.End();

            #endregion


            Timing.stopWatch.Stop();
            Timing.drawTime = Timing.stopWatch.Elapsed;
            Timing.totalTime = Timing.updateTime + Timing.drawTime;
        }

    }
}