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
    public static class ScreenManager
    {

        public static Game1 game;
        public static List<Screen> screens;
        public static List<Screen> screensToUpdate;
        public static SpriteBatch spriteBatch;
        public static bool coveredByOtherScreen;
        public static RenderTarget2D renderSurface;
        public static GameTime gameTime;



        public static void Initialize(Game1 Game1)
        {
            game = Game1;
            screens = new List<Screen>();
            screensToUpdate = new List<Screen>();
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            renderSurface = new RenderTarget2D(game.GraphicsDevice, 640, 360);
            Pool.Initialize();

            //GAME BOOT ROUTINE

            //editor
            //ScreenManager.ExitAndLoad(new ScreenRoomBuilder());

            //game
            ScreenManager.ExitAndLoad(new ScreenTitle());
            //Functions_Backend.SaveGame(GameFile.AutoSave);

            //dev testing
            //Flags.InfiniteGold = true;
            //Flags.CameraTracksHero = true;
            //Camera2D.speed = 3f; //3 is slow/medium speed
        }

        public static void Update(GameTime GameTime)
        {
            gameTime = GameTime;
            Functions_Input.Update(GameTime);

            screensToUpdate.Clear();
            screensToUpdate.AddRange(screens);
            coveredByOtherScreen = false;

            if (screens.Count > 0)
            {
                Screen activeScreen = screens[screens.Count - 1];
                activeScreen.HandleInput(GameTime);
                activeScreen.Update(GameTime);
            }
        }

        public static void Draw(GameTime gameTime)
        {
            //target the render surface + draw the sprites in the 640x360 texture
            game.GraphicsDevice.SetRenderTarget(renderSurface);
            //or target the viewport like normal
            //game.GraphicsDevice.SetRenderTarget(null);
            //clear and set the background
            game.GraphicsDevice.Clear(Assets.colorScheme.background);

            //each screen handles opening and closing the spriteBatch for drawing
            //this allows screens to use camera matrices to draw world views
            foreach (Screen screen in screens) { screen.Draw(gameTime); }

            //Draw the renderSurface to the window frame
            game.GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            spriteBatch.Draw(
                renderSurface, new Rectangle(0, 0,
                    game.graphics.GraphicsDevice.Viewport.Width,      //match width of window frame
                    game.graphics.GraphicsDevice.Viewport.Height),    //match height of window frame
                Color.White);
            spriteBatch.End();
        }



        public static void AddScreen(Screen screen)
        {   //set all of screen's references
            screen.game = game;
            screen.LoadContent();
            screens.Add(screen);
        }

        public static void RemoveScreen(Screen screen)
        {
            screen.UnloadContent();
            screens.Remove(screen);
            screensToUpdate.Remove(screen);
        }

        public static void ExitAndLoad(Screen screenToLoad)
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
            AddScreen(screenToLoad);
        }

        public static void UnloadContent() { foreach (Screen screen in screens) { screen.UnloadContent(); } }

    }
}