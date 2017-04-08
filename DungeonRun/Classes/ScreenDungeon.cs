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
        public Camera2D camera;
        public GameTime gameTime;
        
        public FloorPool floorPool;
        public Pool pool;

        //public GameObjectPool objPool;
        //public GameObjectPool projectilePool;
        //public ActorPool actorPool;

        public override void LoadContent()
        {
            debugInfo = new DebugInfo(this);
            camera = new Camera2D(screenManager);

            floorPool = new FloorPool(this);
            pool = new Pool(this);
            //objPool = new GameObjectPool(assets.dungeonSheet);
            //projectilePool = new GameObjectPool(assets.particleSheet);
            //actorPool = new ActorPool(this);

            DungeonGenerator.CreateRoom(this);
            //ActorFunctions.SetType(actorPool.hero, Actor.Type.Blob);
        }

        public override void HandleInput(InputHelper Input, GameTime GameTime)
        {
            gameTime = GameTime;
            InputFunctions.MapPlayerInput(Input, pool.hero.compInput);
        }

        public override void Update(GameTime GameTime)
        {
            stopWatch.Reset(); stopWatch.Start();

            //update actors + objects
            pool.Update();
            //actorPool.Update();
            //objPool.Update();
            //projectilePool.Update();

            //move actors + objects
            pool.Move(this);
            //actorPool.Move(this);
            //objPool.Move(this);
            //projectilePool.Move(this);

            //track camera to hero
            camera.targetZoom = 1.0f;
            camera.targetPosition = pool.hero.compSprite.position;
            camera.Update(GameTime);

            stopWatch.Stop(); updateTime = stopWatch.Elapsed;
        }

        public override void Draw(GameTime GameTime)
        {
            stopWatch.Reset(); stopWatch.Start();
            
            //draw gameworld
            screenManager.spriteBatch.Begin(
                        SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend,
                        SamplerState.PointClamp,
                        null,
                        null,
                        null,
                        camera.view
                        );

            floorPool.Draw(screenManager);
            pool.Draw(screenManager);
            //objPool.Draw(screenManager);
            //projectilePool.Draw(screenManager);
            //actorPool.Draw(screenManager);
            screenManager.spriteBatch.End();
            
            //draw UI
            screenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            debugInfo.Draw();
            screenManager.spriteBatch.End();
            
            stopWatch.Stop(); drawTime = stopWatch.Elapsed;
            totalTime = updateTime + drawTime;
        }
    }
}