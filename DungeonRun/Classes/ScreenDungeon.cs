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
    public class DungeonScreen : Screen
    {
        public DungeonScreen() { this.name = "DungeonScreen"; }





        //screen state
        //the state of the dungeon screen
        public enum ScreenState { FadeOut, Playing, FadeIn, Waiting }
        public ScreenState screenState = ScreenState.FadeOut;

        Rectangle overlay; //foreground black rectangle
        float overlayAlpha = 1.0f;
        float overlayFadeSpeed = 0.05f;

        //game state
        public enum GameState { Playing, Won, Lost }
        public GameState gameState = GameState.Playing;



        public override void LoadContent()
        {
            overlay = new Rectangle(0, 0, 640, 360);
            Pool.Initialize();
            DungeonGenerator.Initialize(this);
            DungeonGenerator.BuildRoom();
            //ActorFunctions.SetType(Pool.hero, Actor.Type.Blob);
        }

        public override void HandleInput(GameTime GameTime)
        {
            //reset the input for hero and the active actor
            Input.ResetInputData(Pool.hero.compInput);
            Input.ResetInputData(Pool.actorPool[Pool.activeActor].compInput);
            //if screen is playing, allow input for player + active actor
            if (screenState == ScreenState.Playing) 
            {
                Input.MapPlayerInput(Pool.hero.compInput);
                AiFunctions.SetActorInput(); //process one actor per frame
            }

            if (Flags.Debug)
            {   //if the game is in debug mode, dump info on clicked actor/obj
                if (Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
                { DebugFunctions.Inspect(this); }
                //toggle the paused boolean
                if (Input.IsNewKeyPress(Keys.Space))
                {
                    if (Flags.Paused) { Flags.Paused = false; }
                    else { Flags.Paused = true; }
                }
                DebugMenu.HandleInput();
            }

            /*
            //dump the states for every active actor
            if (Input.IsNewKeyPress(Keys.Enter))
            {
                for (Pool.counter = 0; Pool.counter < Pool.actorCount; Pool.counter++)
                {
                    if (Pool.actorPool[Pool.counter].active)
                    { DebugFunctions.Inspect(Pool.actorPool[Pool.counter]); }
                    
                }
            }
            */
        }

        public override void Update(GameTime GameTime)
        {
            Timing.Reset();

            if(!Flags.Paused)
            {   
                if(screenState == ScreenState.FadeOut) //fade overlay to 0
                {
                    overlayAlpha -= overlayFadeSpeed;
                    if (overlayAlpha <= 0.0f)
                    { overlayAlpha = 0.0f; screenState = ScreenState.Playing;  }
                }
                else if (screenState == ScreenState.Playing) //update 
                {
                    WinLoseFunctions.Check(this);
                }
                else if (screenState == ScreenState.FadeIn) //fade overlay to 1.0
                {
                    overlayAlpha += overlayFadeSpeed;
                    if (overlayAlpha >= 1.0f)
                    { overlayAlpha = 1.0f; screenState = ScreenState.Waiting; }
                }
                else if (screenState == ScreenState.Waiting)
                {
                    if (gameState == GameState.Lost)
                    {
                        ScreenManager.AddScreen(new SummaryScreen(false));
                        gameState = GameState.Playing;
                    }
                    else if (gameState == GameState.Won)
                    {
                        ScreenManager.AddScreen(new SummaryScreen(true));
                        gameState = GameState.Playing;
                    }
                    else if (gameState == GameState.Playing)
                    {
                        //wait for fadeOut call from DungeonGenerator
                        //this happens when a new dungeon is built.
                    }
                }

                //update and move actors, objects, and projectiles
                PoolFunctions.Update(this);
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
            if (Flags.DrawCollisions) { DrawFunctions.Draw(Input.cursorColl); }
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