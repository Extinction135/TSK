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
    public class Game1 : Game
    {
        //control booleans for game codepaths
        public Boolean DEBUG = true; //false = release mode
        public Boolean drawCollisionRecs = true;




        public GraphicsDeviceManager graphics;
        public ScreenManager screenManager;
        public Assets assets;
        public ColorScheme colorScheme;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            screenManager = new ScreenManager(this);
            assets = new Assets(Content);

            if (DEBUG) { IsMouseVisible = true; }
            else { IsMouseVisible = false; }

            /*
            if(DEPLOYED) //playing on XBOX1
            { this.IsMouseVisible = false; }
            else //debugging on PC
            { this.IsMouseVisible = true; }
            */

            //setup default color scheme
            colorScheme.background = new Color(100, 100, 100, 255);
            colorScheme.textSmall = new Color(255, 255, 255, 255);
            colorScheme.windowBkg = new Color(0, 0, 0, 200);
            colorScheme.collisionActor = new Color(100, 0, 0, 0);
            colorScheme.collisionObj = new Color(100, 0, 0, 0);
        }
        protected override void Initialize() { base.Initialize(); }
        protected override void LoadContent()
        {
            base.LoadContent();
            assets.Load(GraphicsDevice);
            screenManager.Initialize();
            screenManager.AddScreen(new DungeonScreen());
        }
        protected override void UnloadContent() { }
        protected override void Update(GameTime gameTime) { screenManager.Update(gameTime); base.Update(gameTime); }
        protected override void Draw(GameTime gameTime) { screenManager.Draw(gameTime); base.Draw(gameTime); }
    }
}