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
    public static class Flags
    {   //the master control booleans for various codepaths
        //puts the game in release mode, overwriting other control booleans
        public static Boolean Release = false; //the MASTER control boolean
        public static Boolean Debug = true; //draw/enable debugging info/menu/dev input
        public static Boolean DrawCollisions = false; //draw/hide collision rec components
        public static Boolean Paused = false;
    }

    public class Game1 : Game
    {

        public GraphicsDeviceManager graphics;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //if game is in release mode, all other control flags get set to false
            if (Flags.Release) { Flags.Debug = Flags.DrawCollisions = Flags.Paused = false; }
            //hide/show the cursor based on Debug flag
            if (Flags.Debug) { IsMouseVisible = true; } else { IsMouseVisible = false; }
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