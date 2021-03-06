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
    public class Game1 : Game
    {
        public GraphicsDeviceManager graphics;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            //settings unique to DirectX
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Window.Title = "Legend Of Zelda: The Shadow King v" + Flags.Version;
            Window.AllowUserResizing = false;
        }

        protected override void Initialize() { base.Initialize(); }

        protected override void LoadContent()
        { base.LoadContent(); Assets.Load(GraphicsDevice, Content); ScreenManager.Initialize(this); }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        { ScreenManager.Update(gameTime); Functions_Music.Update(); base.Update(gameTime); }

        protected override void Draw(GameTime gameTime)
        { ScreenManager.Draw(gameTime); base.Draw(gameTime); }
    }
}