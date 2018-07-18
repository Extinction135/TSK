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
            //register this dungeon screen with Functions_Level
            Functions_Level.levelScreen = this;
            //level id is set by overworld screen
            Functions_Level.BuildLevel(LevelSet.currentLevel.ID); 
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
                    if (exitAction == ExitAction.Summary)
                    { ScreenManager.ExitAndLoad(Screens.Summary); }
                    else if (exitAction == ExitAction.ExitDungeon)
                    {
                        //hero doesn't return to overworld
                        //ScreenManager.ExitAndLoad(new ScreenOverworld());

                        //instead hero loads into the last field level
                        LevelSet.currentLevel = LevelSet.field;
                        ScreenManager.ExitAndLoad(Screens.Level);
                    }
                    else if (exitAction == ExitAction.Overworld)
                    {
                        ScreenManager.ExitAndLoad(Screens.Overworld);
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


                //if the screen is closed, don't play any sounds or do any work
                if (displayState != DisplayState.Closed)
                {   //update and move actors, objects, projectiles, and camera
                    Functions_Pool.Update();
                    Functions_WorldUI.Update();


                    #region Track Hero in Fields and Dungeons

                    if (LevelSet.currentLevel.isField)
                    {
                        //center camera to field
                        Camera2D.targetPosition.X = LevelSet.currentLevel.currentRoom.center.X;
                        Camera2D.targetPosition.Y = LevelSet.currentLevel.currentRoom.center.Y;
                        //move 50% toward hero
                        Camera2D.targetPosition.X -=
                                (LevelSet.currentLevel.currentRoom.center.X - Pool.hero.compSprite.position.X) * 0.5f;
                        Camera2D.targetPosition.Y -=
                            (LevelSet.currentLevel.currentRoom.center.Y - Pool.hero.compSprite.position.Y) * 0.5f;
                    }
                    else
                    {   //hero is in a dungeon
                        if(Flags.CameraTracksHero)
                        {
                            Camera2D.tracks = false; //teleport to/follow hero
                            Camera2D.targetPosition.X = Pool.hero.compSprite.position.X;
                            Camera2D.targetPosition.Y = Pool.hero.compSprite.position.Y;
                        }
                        else
                        {
                            //center camera to current room
                            Camera2D.targetPosition.X = LevelSet.currentLevel.currentRoom.center.X;
                            Camera2D.targetPosition.Y = LevelSet.currentLevel.currentRoom.center.Y + 16 * 1;
                            Camera2D.tracks = true; //wait until room change, then move
                        }
                    }

                    #endregion
                    

                    Functions_Camera2D.Update();
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

            if(Flags.EnableDebugInfo)
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

                TopDebugMenu.DebugDisplay_Ram.textComp.text = "U: " + updateAvg;
                TopDebugMenu.DebugDisplay_Ram.textComp.text += "\nD: " + drawAvg;
                TopDebugMenu.DebugDisplay_Ram.textComp.text += "\nT: " + Timing.totalTime.Milliseconds + " ms";
                TopDebugMenu.DebugDisplay_Ram.textComp.text += "\n" + ScreenManager.gameTime.TotalGameTime.ToString(@"hh\:mm\:ss");
                TopDebugMenu.DebugDisplay_Ram.textComp.text += "\n" + Functions_Backend.GetRam() + " mb";

                #endregion


                TopDebugMenu.DebugDisplay_HeroState.textComp.text = "IN: " + Pool.hero.inputState;
                TopDebugMenu.DebugDisplay_HeroState.textComp.text += "\nST: " + Pool.hero.state;
                TopDebugMenu.DebugDisplay_HeroState.textComp.text += "\nLCK: " + Pool.hero.stateLocked;
                TopDebugMenu.DebugDisplay_HeroState.textComp.text += "\nMDIR: " + Pool.hero.compMove.direction;
                TopDebugMenu.DebugDisplay_HeroState.textComp.text += "\nFDIR: " + Pool.hero.direction;

                TopDebugMenu.DebugDisplay_Movement.textComp.text = "PX: " + Pool.hero.compSprite.position.X;
                TopDebugMenu.DebugDisplay_Movement.textComp.text += "\nPY: " + Pool.hero.compSprite.position.Y;
                TopDebugMenu.DebugDisplay_Movement.textComp.text += "\nFRIC: " + Pool.hero.compMove.friction;
                TopDebugMenu.DebugDisplay_Movement.textComp.text += "\nMX:" + Pool.hero.compMove.magnitude.X.ToString("0.####");
                TopDebugMenu.DebugDisplay_Movement.textComp.text += "\nMY:" + Pool.hero.compMove.magnitude.Y.ToString("0.####");



                #region Setup counts for roomObjs, projectils, particles, actors, and floors

                counter = 0;
                for (i = 0; i < Pool.roomObjCount; i++) { if (Pool.roomObjPool[i].active) { counter++; } }
                TopDebugMenu.DebugDisplay_PoolCounter.textComp.text = "OBJ: " + counter + "/" + Pool.roomObjCount;

                counter = 0;
                for (i = 0; i < Pool.projectileCount; i++) { if (Pool.projectilePool[i].active) { counter++; } }
                TopDebugMenu.DebugDisplay_PoolCounter.textComp.text += "\nPRO: " + counter + "/" + Pool.projectileCount;

                counter = 0;
                for (i = 0; i < Pool.particleCount; i++) { if (Pool.particlePool[i].active) { counter++; } }
                TopDebugMenu.DebugDisplay_PoolCounter.textComp.text += "\nPAR: " + counter + "/" + Pool.particleCount;

                counter = 0;
                for (i = 0; i < Pool.actorCount; i++) { if (Pool.actorPool[i].active) { counter++; } }
                TopDebugMenu.DebugDisplay_PoolCounter.textComp.text += "\nACT: " + counter + "/" + Pool.actorCount;

                counter = 0;
                for (i = 0; i < Pool.floorCount; i++) { if (Pool.floorPool[i].visible) { counter++; } }
                TopDebugMenu.DebugDisplay_PoolCounter.textComp.text += "\nFLR: " + counter + "/" + Pool.floorCount;

                #endregion


                TopDebugMenu.DebugDisplay_BuildTimes.textComp.text = "LVL: " + Functions_Level.time.Ticks;
                TopDebugMenu.DebugDisplay_BuildTimes.textComp.text += "\nRM: " + Functions_Room.time.Ticks;
                TopDebugMenu.DebugDisplay_BuildTimes.textComp.text += "\nT: " + DungeonRecord.timer.Elapsed.ToString(@"hh\:mm\:ss");
                TopDebugMenu.DebugDisplay_BuildTimes.textComp.text += "\nENMY: " + DungeonRecord.enemyCount;
                TopDebugMenu.DebugDisplay_BuildTimes.textComp.text += "\nDMG: " + DungeonRecord.totalDamage;

                TopDebugMenu.DebugDisplay_Collisions.textComp.text = "COL: " + Pool.collisionsCount;
                TopDebugMenu.DebugDisplay_Collisions.textComp.text += "\nINT: " + Pool.interactionsCount;
                TopDebugMenu.DebugDisplay_Collisions.textComp.text += "\n-";
                TopDebugMenu.DebugDisplay_Collisions.textComp.text += "\n-";
                TopDebugMenu.DebugDisplay_Collisions.textComp.text += "\n-";

                //draw all our debug info like this from now on:
                Functions_Draw.Draw(TopDebugMenu.DebugDisplay_Ram);
                Functions_Draw.Draw(TopDebugMenu.DebugDisplay_HeroState);
                Functions_Draw.Draw(TopDebugMenu.DebugDisplay_Movement);
                Functions_Draw.Draw(TopDebugMenu.DebugDisplay_PoolCounter);
                Functions_Draw.Draw(TopDebugMenu.DebugDisplay_BuildTimes);
                Functions_Draw.Draw(TopDebugMenu.DebugDisplay_Collisions);
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