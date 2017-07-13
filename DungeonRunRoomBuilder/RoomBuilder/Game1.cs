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
        public GraphicsDeviceManager graphics;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //settings unique to RoomBuilder
            IsMouseVisible = false;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Window.Title = "RoomBuilder v0.1";
            Window.AllowUserResizing = false;
        }

        protected override void Initialize() { base.Initialize(); }

        protected override void LoadContent()
        {
            base.LoadContent();
            Assets.Load(GraphicsDevice, Content);
            ScreenManager.Initialize(this);

            //editor
            //ScreenManager.ExitAndLoad(new ScreenRoomBuilder());
            //game
            ScreenManager.ExitAndLoad(new ScreenTitle());
            //Functions_Backend.SaveGame(GameFile.AutoSave);
            //Flags.InfiniteGold = true;
            Flags.CameraTracksHero = true;
            //Camera2D.speed = 3f; //3 is slow/medium speed
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            ScreenManager.Update(gameTime);
            Functions_Music.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) { ScreenManager.Draw(gameTime); base.Draw(gameTime); }
    }
}
