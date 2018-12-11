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

        public long roomTime = 0;
        public long dungeonTime = 0;
        byte framesTotal = 30; //how many frames to average over
        byte frameCounter = 0; //increments thru frames 0-framesTotal
        long updateTicks; //update tick times are added to this
        long drawTicks; //draw tick times are added to this
        long updateAvg; //stores the average update ticks
        long drawAvg; //stores the average draw ticks
        int counter; //counts roomObjs, pros, parts
        int i;




        public ScreenLevel() { this.name = "LevelScreen"; }

        public override void Open()
        {   
            overlay.alpha = 1.0f;
            overlay.fadeOutSpeed = 0.025f;
            //level id is set by overworld screen
            Functions_Level.BuildLevel(LevelSet.currentLevel.ID); 
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
                
                //map input.player1 to Poolhero.compInput
                //Functions_Input.MapPlayerInput(Pool.hero.compInput);
                Functions_Input.MapGameInputToInputComponent(Input.Player1, Pool.hero.compInput);

                //allow player access to inventory screen from overworld map
                if (Input.Player1.Start & Input.Player1.Start_Prev == false)
                { ScreenManager.AddScreen(Screens.Inventory); }
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
                //handle screen state


                #region Opening

                if(displayState == DisplayState.Opening) //fade overlay to 0
                {   //fade overlay out
                    overlay.fadeState = FadeState.FadeOut;
                    Functions_ScreenRec.Fade(overlay);
                    if (overlay.fadeState == FadeState.FadeComplete)
                    { displayState = DisplayState.Opened; }
                }

                #endregion


                #region Opened

                else if (displayState == DisplayState.Opened)
                {
                    //set overlay alpha to a negative value
                    overlay.alpha = -1.5f; //delays closing state a bit
                }   //delay gives player time to understand what's happening

                #endregion


                #region Closing

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

                #endregion


                #region Closed

                else if (displayState == DisplayState.Closed)
                {
                    //prevent magic from continuing between levels
                    Functions_MagicSpells.StopAll();

                    //handle exit action
                    if (exitAction == ExitAction.Summary)
                    {   //set the 'end time' for this playthrough
                        PlayerData.endTime = DateTime.Now;
                        ScreenManager.ExitAndLoad(Screens.Summary);
                    }
                    else if (exitAction == ExitAction.Overworld)
                    {
                        //exit level, loading shadowking overworld map
                        ScreenManager.ExitAndLoad(Screens.Overworld_ShadowKing);
                    }
                    else if (exitAction == ExitAction.Title)
                    {
                        ScreenManager.ExitAndLoad(Screens.Title);
                    }
                    else if (exitAction == ExitAction.Field)
                    {   //point to field level
                        LevelSet.currentLevel = LevelSet.field;
                        ScreenManager.ExitAndLoad(Screens.Level);
                    }
                    else if (exitAction == ExitAction.Dungeon)
                    {   //point to dungeon level
                        LevelSet.currentLevel = LevelSet.dungeon;
                        ScreenManager.ExitAndLoad(Screens.Level);
                    }
                    
                    if (Flags.PrintOutput)
                    { Debug.WriteLine("exit action for level screen: " + exitAction); }
                }

                #endregion




                #region World Update Routine - any state except closing

                if(displayState != DisplayState.Closed)
                {   
                    //track Hero in Fields and Dungeons
                    if (LevelSet.currentLevel.isField)
                    {
                        //center camera to field
                        Camera2D.targetPosition.X = LevelSet.field.currentRoom.center.X;
                        Camera2D.targetPosition.Y = LevelSet.field.currentRoom.center.Y;
                        //move 50% toward hero
                        Camera2D.targetPosition.X -=
                                (LevelSet.field.currentRoom.center.X - Pool.hero.compSprite.position.X) * 0.5f;
                        Camera2D.targetPosition.Y -=
                            (LevelSet.field.currentRoom.center.Y - Pool.hero.compSprite.position.Y) * 0.5f;
                    }
                    else
                    {   //hero is in a dungeon
                        if (Flags.CameraTracksHero)
                        {
                            Camera2D.tracks = false; //teleport to/follow hero
                            Camera2D.targetPosition.X = Pool.hero.compSprite.position.X;
                            Camera2D.targetPosition.Y = Pool.hero.compSprite.position.Y;
                        }
                        else
                        {
                            //center camera to current room
                            Camera2D.targetPosition.X = LevelSet.dungeon.currentRoom.center.X;
                            Camera2D.targetPosition.Y = LevelSet.dungeon.currentRoom.center.Y + 8;
                            Camera2D.tracks = true; //wait until room change, then move
                        }
                    }
                    Functions_Camera2D.Update();
                    Functions_MagicSpells.Update();
                    Functions_Pool.Update();
                    Functions_WorldUI.Update();
                }

                #endregion

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
                        null, null, null, Camera2D.view);

            Functions_Pool.Draw();

            if (Flags.DrawCollisions)
            {
                Functions_Draw.Draw(Input.cursorColl);
                Functions_Draw.DrawLevel();
                //Functions_Draw.Draw(Functions_Hero.interactionPoint);
            }

            ScreenManager.spriteBatch.End();

            #endregion


            //Draw UI, top menu, debug, & screen fading in/out overlay
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            Functions_WorldUI.Draw();


            #region Debug Info

            if(Flags.EnableDebugInfo) //setup debug display data
            {

                #region Setup update, draw, total times, gametime and total ram

                frameCounter++;
                if (frameCounter > framesTotal)
                {   //reset the counter + total ticks
                    frameCounter = 0;
                    updateTicks = 0;
                    drawTicks = 0;
                }
                else if (frameCounter == framesTotal)
                {   //calculate the average ticks
                    updateAvg = updateTicks / framesTotal;
                    drawAvg = drawTicks / framesTotal;
                }
                //collect tick times
                updateTicks += Timing.updateTime.Ticks;
                drawTicks += Timing.drawTime.Ticks;

                TopDebugMenu.DebugDisplay_Ram.stringBuilder.Append("U: " + updateAvg);
                TopDebugMenu.DebugDisplay_Ram.stringBuilder.Append("\nD: " + drawAvg);
                TopDebugMenu.DebugDisplay_Ram.stringBuilder.Append("\nT: " + Timing.totalTime.Milliseconds + " ms");
                TopDebugMenu.DebugDisplay_Ram.stringBuilder.Append("\n" + ScreenManager.gameTime.TotalGameTime.ToString(@"hh\:mm\:ss"));
                TopDebugMenu.DebugDisplay_Ram.stringBuilder.Append("\n" + Functions_Backend.GetRam() + " mb");
                TopDebugMenu.DebugDisplay_Ram.textComp.text = TopDebugMenu.DebugDisplay_Ram.stringBuilder.ToString();
                TopDebugMenu.DebugDisplay_Ram.stringBuilder.Clear();

                #endregion


                #region Main Pool Counters

                counter = 0; //indestructible objs
                for (i = 0; i < Pool.indObjCount; i++) { if (Pool.indObjPool[i].active) { counter++; } }
                TopDebugMenu.DebugDisplay_PoolCounter.stringBuilder.Append("ind " + counter + "/" + Pool.indObjCount);

                counter = 0; //interactive objs
                for (i = 0; i < Pool.intObjCount; i++) { if (Pool.intObjPool[i].active) { counter++; } }
                TopDebugMenu.DebugDisplay_PoolCounter.stringBuilder.Append("\nint " + counter + "/" + Pool.intObjCount);

                counter = 0;
                for (i = 0; i < Pool.projectileCount; i++) { if (Pool.projectilePool[i].active) { counter++; } }
                TopDebugMenu.DebugDisplay_PoolCounter.stringBuilder.Append("\npro " + counter + "/" + Pool.projectileCount);

                counter = 0; //particles
                for (i = 0; i < Pool.particleCount; i++) { if (Pool.particlePool[i].active) { counter++; } }
                TopDebugMenu.DebugDisplay_PoolCounter.stringBuilder.Append("\npart " + counter + "/" + Pool.particleCount);

                counter = 0; //floors
                for (i = 0; i < Pool.floorCount; i++) { if (Pool.floorPool[i].visible) { counter++; } }
                TopDebugMenu.DebugDisplay_PoolCounter.stringBuilder.Append("\nflrs " + counter + "/" + Pool.floorCount);

                //write it
                TopDebugMenu.DebugDisplay_PoolCounter.textComp.text = TopDebugMenu.DebugDisplay_PoolCounter.stringBuilder.ToString();
                TopDebugMenu.DebugDisplay_PoolCounter.stringBuilder.Clear();

                #endregion


                #region Collisions and Interactions and Hero Interactions Counter

                //display total possible checks vs actual processed - to determine system weaknesses
                TopDebugMenu.DebugDisplay_SysCounter.stringBuilder.Append("collisions");
                TopDebugMenu.DebugDisplay_SysCounter.stringBuilder.Append("\n" + Pool.collisions_ThisFrame + "/" + Pool.collisions_Possible);
                TopDebugMenu.DebugDisplay_SysCounter.stringBuilder.Append("\ninteraxtns");
                TopDebugMenu.DebugDisplay_SysCounter.stringBuilder.Append("\n" + Pool.interactions_ThisFrame + "/" + Pool.interactions_Possible);

                TopDebugMenu.DebugDisplay_SysCounter.stringBuilder.Append("\nhro " + Pool.heroInts_ThisFrame + "/" + Pool.heroInts_Possible);

                TopDebugMenu.DebugDisplay_SysCounter.textComp.text = TopDebugMenu.DebugDisplay_SysCounter.stringBuilder.ToString();
                TopDebugMenu.DebugDisplay_SysCounter.stringBuilder.Clear();
                

                #endregion


                #region Build Times Debug Display Setup

                TopDebugMenu.DebugDisplay_BuildTimes.stringBuilder.Append("lvlbld " + Functions_Level.time.Ticks);
                TopDebugMenu.DebugDisplay_BuildTimes.stringBuilder.Append("\nrm bld " + Functions_Room.time.Ticks);

                counter = 0; //display active wind objs
                for (i = 0; i < Pool.windObjCount; i++) { if (Pool.windObjPool[i].active) { counter++; } }
                TopDebugMenu.DebugDisplay_BuildTimes.stringBuilder.Append("\nwind " + counter + "/" + Pool.windObjCount);
                TopDebugMenu.DebugDisplay_BuildTimes.stringBuilder.Append("\n" + Pool.windInts_ThisFrame + "/" + Pool.windInts_Possible);
                
                TopDebugMenu.DebugDisplay_BuildTimes.stringBuilder.Append("\n...");
                TopDebugMenu.DebugDisplay_BuildTimes.textComp.text = TopDebugMenu.DebugDisplay_BuildTimes.stringBuilder.ToString();
                TopDebugMenu.DebugDisplay_BuildTimes.stringBuilder.Clear();

                #endregion


                #region Setup Hero State/Move Displays

                TopDebugMenu.DebugDisplay_HeroState.stringBuilder.Append("IN: " + Pool.hero.inputState);
                TopDebugMenu.DebugDisplay_HeroState.stringBuilder.Append("\nST: " + Pool.hero.state);
                TopDebugMenu.DebugDisplay_HeroState.stringBuilder.Append("\nLCK: " + Pool.hero.stateLocked);
                TopDebugMenu.DebugDisplay_HeroState.stringBuilder.Append("\nMDIR: " + Pool.hero.compMove.direction);
                TopDebugMenu.DebugDisplay_HeroState.stringBuilder.Append("\nFDIR: " + Pool.hero.direction);
                TopDebugMenu.DebugDisplay_HeroState.textComp.text = TopDebugMenu.DebugDisplay_HeroState.stringBuilder.ToString();
                TopDebugMenu.DebugDisplay_HeroState.stringBuilder.Clear();

                TopDebugMenu.DebugDisplay_Movement.stringBuilder.Append("PX: " + Pool.hero.compSprite.position.X);
                TopDebugMenu.DebugDisplay_Movement.stringBuilder.Append("\nPY: " + Pool.hero.compSprite.position.Y);
                TopDebugMenu.DebugDisplay_Movement.stringBuilder.Append("\nFRIC: " + Pool.hero.compMove.friction);
                TopDebugMenu.DebugDisplay_Movement.stringBuilder.Append("\nMX:" + Pool.hero.compMove.magnitude.X.ToString("0.####"));
                TopDebugMenu.DebugDisplay_Movement.stringBuilder.Append("\nMY:" + Pool.hero.compMove.magnitude.Y.ToString("0.####"));
                TopDebugMenu.DebugDisplay_Movement.textComp.text = TopDebugMenu.DebugDisplay_Movement.stringBuilder.ToString();
                TopDebugMenu.DebugDisplay_Movement.stringBuilder.Clear();

                #endregion





                #region Call Draw()

                Functions_Draw.Draw(TopDebugMenu.DebugDisplay_Ram);
                Functions_Draw.Draw(TopDebugMenu.DebugDisplay_PoolCounter);
                Functions_Draw.Draw(TopDebugMenu.DebugDisplay_SysCounter);
                Functions_Draw.Draw(TopDebugMenu.DebugDisplay_BuildTimes);
                Functions_Draw.Draw(TopDebugMenu.DebugDisplay_HeroState);
                Functions_Draw.Draw(TopDebugMenu.DebugDisplay_Movement);

                #endregion

            }

            #endregion


            Functions_Draw.Draw(overlay); //draw black fade in/out overlay rec
            ScreenManager.spriteBatch.End();

            Timing.stopWatch.Stop();
            Timing.drawTime = Timing.stopWatch.Elapsed;
            Timing.totalTime = Timing.updateTime + Timing.drawTime;
        }

    }
}