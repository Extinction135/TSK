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
    

    

    public enum MouseButtons { LeftButton, RightButton }

    public struct ColorScheme
    {
        public Color background;
        public Color textSmall;
        public Color windowBkg;
    }












    public class Game1 : Game
    {
        public Boolean DEPLOYED = false; //true = playing on xbox1
        public Boolean DEBUG = true; //toggles debug codepaths



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


            if(DEPLOYED) //playing on XBOX1
            {
                //3:1 resolution
                graphics.PreferredBackBufferWidth = 1920;
                graphics.PreferredBackBufferHeight = 1080;
                this.IsMouseVisible = false;
            }
            else //debugging on PC
            {
                //2:1 resolution
                graphics.PreferredBackBufferWidth = 1280;
                graphics.PreferredBackBufferHeight = 720;
                this.IsMouseVisible = true;
            }

            //setup default color scheme
            colorScheme.background = new Color(100, 100, 100, 255);
            colorScheme.textSmall = new Color(255, 255, 255, 255);
            colorScheme.windowBkg = new Color(0, 0, 0, 200);
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