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

        public Stopwatch stopWatch = new Stopwatch();  
        public Stopwatch total = new Stopwatch(); 
        public TimeSpan updateTime;
        public TimeSpan drawTime;
        public TimeSpan totalTime;
        public DebugInfo debugInfo;
        public Boolean paused = false;

        public override void LoadContent()
        {
            debugInfo = new DebugInfo(this);
            Pool.Initialize(this);
            DungeonGenerator.CreateRoom(this);
            //ActorFunctions.SetType(pool.hero, Actor.Type.Blob);
            //DebugFunctions.Inspect(pool.hero);
        }

        public override void HandleInput(GameTime GameTime)
        {
            Input.MapPlayerInput(Pool.hero.compInput);

            if (game.DEBUG)
            {   //if the game is in debug mode, dump info on clicked actor/obj
                if (Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
                { DebugFunctions.Inspect(this); }
                //toggle the paused boolean
                if (Input.IsNewKeyPress(Keys.Space))
                { if (paused) { paused = false; } else { paused = true; } }
            }
        }

        public override void Update(GameTime GameTime)
        {
            stopWatch.Reset(); stopWatch.Start();

            if(!paused)
            {   //update and move actors, objects, and projectiles
                PoolFunctions.Update();
                PoolFunctions.Move(this);
            }

            //track camera to hero
            Camera2D.targetPosition = Pool.hero.compSprite.position;
            Camera2D.Update(GameTime);

            stopWatch.Stop(); updateTime = stopWatch.Elapsed;
        }

        public override void Draw(GameTime GameTime)
        {
            stopWatch.Reset(); stopWatch.Start();

            //draw gameworld
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
            if (game.drawCollisionRecs) { DrawFunctions.Draw(Input.cursorColl); }

            ScreenManager.spriteBatch.End();

            //draw UI
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            debugInfo.Draw();
            ScreenManager.spriteBatch.End();
            
            stopWatch.Stop(); drawTime = stopWatch.Elapsed;
            totalTime = updateTime + drawTime;
        }
    }
}