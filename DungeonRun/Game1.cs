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

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            if (DEBUG) { IsMouseVisible = true; }
            else { IsMouseVisible = false; }
        }
        protected override void Initialize() { base.Initialize(); }
        protected override void LoadContent()
        {
            base.LoadContent();
            Assets.Load(GraphicsDevice, Content);
            Assets.SetDefaultColorScheme();
            ScreenManager.Initialize(this);
            ScreenManager.AddScreen(new DungeonScreen());
        }
        protected override void UnloadContent() { }
        protected override void Update(GameTime gameTime) { ScreenManager.Update(gameTime); base.Update(gameTime); }
        protected override void Draw(GameTime gameTime) { ScreenManager.Draw(gameTime); base.Draw(gameTime); }
    }
}