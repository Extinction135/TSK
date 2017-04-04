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

        
        public DebugInfo debugInfo;

        public DungeonScreen() { this.name = "DungeonScreen"; }



        public Stopwatch stopWatch = new Stopwatch();  
        public Stopwatch total = new Stopwatch(); 
        public TimeSpan updateTime;
        public TimeSpan drawTime;
        public TimeSpan totalTime;

        public Camera2D camera;
        public GameTime gameTime;




        public Actor hero;
        public ActorPool actorPool;








        public override void LoadContent()
        {
            debugInfo = new DebugInfo(this);
            camera = new Camera2D(screenManager);



            hero = new Actor(this);
            actorPool = new ActorPool(this);

            hero.SetType(Actor.Type.Hero, 100, 100);
        }



        public override void HandleInput(InputHelper Input, GameTime GameTime)
        {
            gameTime = GameTime;
            hero.compInput.HandlePlayerInput(Input);
        }




        public override void Update(GameTime GameTime)
        {
            stopWatch.Reset(); stopWatch.Start();

            //update actors
            hero.Update();
            actorPool.Update();

            //track camera to hero
            camera.targetZoom = 1.0f;
            camera.targetPosition = hero.compSprite.position;
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

            hero.Draw();
            actorPool.Draw();

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