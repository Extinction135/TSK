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
    public class ScreenManager
    {
        public Game1 game;
        public List<Screen> screens;
        public List<Screen> screensToUpdate;
        public SpriteBatch spriteBatch;
        public bool coveredByOtherScreen;
        public int transitionCount;
        public RenderTarget2D renderSurface;



        public Screen[] GetScreens() { return screens.ToArray(); }

        public void AddScreen(Screen screen)
        {   //set all of screen's references
            screen.game = game;
            screen.screenManager = this;
            screen.assets = game.assets;
            screen.LoadContent();
            screens.Add(screen);
        }

        public void RemoveScreen(Screen screen)
        {
            screen.UnloadContent();
            screens.Remove(screen);
            screensToUpdate.Remove(screen);
        }

        public void ExitAndLoad(Screen screenToLoad)
        {   //remove every screen on screens list
            while (screens.Count > 0)
            {
                try
                {
                    screens[0].UnloadContent();
                    screensToUpdate.Remove(screens[0]);
                    screens.Remove(screens[0]);
                }
                catch { }
            }
            this.AddScreen(screenToLoad);
        }

        public void UnloadContent() { foreach (Screen screen in screens) { screen.UnloadContent(); } }



        public ScreenManager(Game1 Game1)
        {
            game = Game1;
            screens = new List<Screen>();
            screensToUpdate = new List<Screen>();
        }

        public void Initialize()
        {
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            renderSurface = new RenderTarget2D(game.GraphicsDevice, 640, 360);
            //gridRef = new Sprite(this, game.gridSheet, new Vector2(320, 320), new Point(640, 360), new Vector3(0, 0, 0));
            Input.Initialize();
            Camera2D.Initialize(this);
        }

        public void Update(GameTime GameTime)
        {
            Input.Update(GameTime); //read the keyboard and gamepad

            //make a copy of the master screen list, to avoid confusion if
            //the process of updating one screen adds or removes others
            screensToUpdate.Clear();
            foreach (Screen screen in screens) { screensToUpdate.Add(screen); }
            coveredByOtherScreen = false;

            //loop as long as there are screens waiting to be updated.
            while (screensToUpdate.Count > 0)
            {   //remove the topmost screen from the waiting list
                Screen screen = screensToUpdate[screensToUpdate.Count - 1];
                screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

                if (coveredByOtherScreen == false) //targeting the top most screen
                {   //update & send input only to the top screen
                    screen.HandleInput(GameTime);
                    screen.Update(GameTime);
                    coveredByOtherScreen = true; //no update/input to screens below top
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            //target the render surface + draw the sprites in the 640x360 texture
            game.GraphicsDevice.SetRenderTarget(renderSurface);
            //or target the viewport like normal
            //game.GraphicsDevice.SetRenderTarget(null);
            //clear and set the background
            game.GraphicsDevice.Clear(game.colorScheme.background);

            //each screen handles opening and closing the spriteBatch for drawing
            //this allows screens to use camera matrices to draw world views
            foreach (Screen screen in screens) { screen.Draw(gameTime); }

            /*
            //draw grid reference here
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
            if (game.variables.drawGrid) { gridRef.Draw(); }
            cursor.Draw(); //cursor is always last thing drawn to viewport
            spriteBatch.End();
            */

            
            #region Draw the renderSurface to the window frame

            game.GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            spriteBatch.Draw(
                renderSurface, new Rectangle(0, 0,
                    game.graphics.GraphicsDevice.Viewport.Width,      //match width of window frame
                    game.graphics.GraphicsDevice.Viewport.Height),    //match height of window frame
                Color.White);
            if (game.drawCollisionRecs) { DrawFunctions.Draw(Input.cursorColl, this); }

            spriteBatch.End();

            #endregion
            
        }

    }
}