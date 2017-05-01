﻿using System;
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
    public class DungeonScreen : Screen
    {

        //the foreground black rectangle, overlays and hides game content
        Rectangle overlay; 
        public float overlayAlpha = 1.0f;
        float fadeInSpeed = 0.015f;
        float fadeOutSpeed = 0.025f;
        //the various states the game can be in
        public enum GameState { Playing, Summary }
        public GameState gameState = GameState.Playing;



        public DungeonScreen() { this.name = "DungeonScreen"; }

        public override void LoadContent()
        {
            overlay = new Rectangle(0, 0, 640, 360);

            Pool.Initialize();
            DungeonFunctions.Initialize(this);
            DungeonFunctions.BuildDungeon();
            //ActorFunctions.SetType(Pool.hero, Actor.Type.Blob);

            //open the screen
            displayState = DisplayState.Opening;
        }

        public override void HandleInput(GameTime GameTime)
        {
            //if screen is playing, allow input for player + active actor
            if (displayState == DisplayState.Opened) 
            {
                //reset the input for hero, map player input to hero
                Input.ResetInputData(Pool.hero.compInput);
                Input.MapPlayerInput(Pool.hero.compInput);
                AiFunctions.SetActorInput(); //set AI for actor
                AiFunctions.SetActorInput(); //set AI for actor
                AiFunctions.SetActorInput(); //set AI for actor

                //open the inventory screen if player presses start button
                if (Input.IsNewButtonPress(Buttons.Start))
                { ScreenManager.AddScreen(new ScreenInventory()); }
            }
            else//dungeonScreen is not in playing state..
            {   //reset all input for hero + actors
                Input.ResetInputData(Pool.hero.compInput);
                PoolFunctions.ResetActorPoolInput();
            }

            if (Flags.Debug)
            {   //if the game is in debug mode, dump info on clicked actor/obj
                if (Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
                { DebugFunctions.Inspect(this); }
                //toggle the paused boolean
                if (Input.IsNewKeyPress(Keys.Space))
                { if (Flags.Paused) { Flags.Paused = false; } else { Flags.Paused = true; } }
                DebugMenu.HandleInput();
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
                    overlayAlpha -= fadeOutSpeed;
                    if (overlayAlpha <= 0.0f)
                    {
                        overlayAlpha = 0.0f;
                        displayState = DisplayState.Opened;
                    }
                }
                else if (displayState == DisplayState.Opened)
                {   //set overlay alpha to a negative value
                    overlayAlpha = -2.0f; //delays closing state a bit
                }   //delay gives player time to understand what's happening
                else if (displayState == DisplayState.Closing) //fade overlay to 1.0
                {
                    overlayAlpha += fadeInSpeed;
                    if (overlayAlpha >= 1.0f)
                    {
                        overlayAlpha = 1.0f;
                        displayState = DisplayState.Closed;
                        gameState = GameState.Summary;
                    }
                }
                else if (displayState == DisplayState.Closed)
                {
                    if (gameState == GameState.Summary)
                    {
                        DungeonRecord.timer.Stop();
                        ScreenManager.AddScreen(new SummaryScreen());
                        gameState = GameState.Playing;
                    }
                    else
                    {
                        //wait for ScreenState.Opening call from DungeonGenerator
                        //this happens when a new dungeon is built
                    }
                }

                #endregion


                //update and move actors, objects, and projectiles
                PoolFunctions.Update();
                CollisionFunctions.CheckDungeonRoomCollisions();
                WorldUI.Update();
                //track camera to hero
                Camera2D.targetPosition = Pool.hero.compSprite.position;
                Camera2D.Update(GameTime);
            }

            Timing.stopWatch.Stop();
            Timing.updateTime = Timing.stopWatch.Elapsed;
        }

        public override void Draw(GameTime GameTime)
        {
            Timing.Reset();

            //draw gameworld from camera's view
            ScreenManager.spriteBatch.Begin(
                        SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend,
                        SamplerState.PointClamp,
                        null,
                        null,
                        null,
                        Camera2D.view
                        );
            PoolFunctions.Draw();
            if (Flags.DrawCollisions)
            {
                DrawFunctions.Draw(Input.cursorColl);
                DrawFunctions.Draw(DungeonFunctions.dungeon);
                DrawFunctions.Draw(InteractionFunctions.interactionRec);
            }
            ScreenManager.spriteBatch.End();

            //draw UI, debug info + debug menu
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            WorldUI.Draw();
            if (Flags.Debug) { DebugInfo.Draw(); DebugMenu.Draw(); }
            //draw the overlay rec last
            ScreenManager.spriteBatch.Draw( Assets.dummyTexture, 
                overlay, Assets.colorScheme.overlay * overlayAlpha);
            ScreenManager.spriteBatch.End();

            Timing.stopWatch.Stop();
            Timing.drawTime = Timing.stopWatch.Elapsed;
            Timing.totalTime = Timing.updateTime + Timing.drawTime;
        }

    }
}