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

        // ******************************************************************
        public static Boolean Release = false; //puts the game in release mode, overwrites other flags
        // ******************************************************************

        public static Boolean Debug = false; //draw/enable debugging info/menu/dev input
        public static Boolean DrawCollisions = false; //draw/hide collision rec components
        public static Boolean Paused = false; //this shouldn't be changed here, it's controlled by user in debug mode
        public static Boolean PlayMusic = false; //turns music on/off (but not soundFX)
        public static Boolean SpawnMobs = true; //toggles the spawning of lesser enemies (not bosses)
    }

    public class Game1 : Game
    {

        public GraphicsDeviceManager graphics;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            if (Flags.Release)
            {   //set the game's flags for release mode
                Flags.Debug = false;
                Flags.DrawCollisions = false;
                Flags.Paused = false;
                Flags.PlayMusic = true;
                Flags.SpawnMobs = true;
            }

            //hide/show the cursor based on Debug flag
            if (Flags.Debug) { IsMouseVisible = true; } else { IsMouseVisible = false; }
        }

        protected override void Initialize() { base.Initialize(); }

        protected override void LoadContent()
        {
            base.LoadContent();
            Assets.Load(GraphicsDevice, Content);
            ScreenManager.Initialize(this);
            ScreenManager.AddScreen(new ScreenDungeon());
            ScreenManager.AddScreen(new ScreenOverworld());
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            ScreenManager.Update(gameTime);
            if (Flags.PlayMusic) { MusicFunctions.Update(); }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) { ScreenManager.Draw(gameTime); base.Draw(gameTime); }

    }
}