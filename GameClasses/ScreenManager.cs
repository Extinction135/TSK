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
    public static class ScreenManager
    {

        public static Game1 game;

        public static List<Screen> screens; //list of active screens
        public static Screen activeScreen; //targets the active screen

        public static SpriteBatch spriteBatch;
        public static RenderTarget2D renderSurface;
        public static GameTime gameTime;



        public static void Initialize(Game1 Game1)
        {
            game = Game1;
            screens = new List<Screen>();
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            renderSurface = new RenderTarget2D(game.GraphicsDevice, 640, 360);

            Pool.Initialize();


            //GAME BOOT ROUTINE
            if (Flags.bootRoutine == BootRoutine.Game)
            {
                ExitAndLoad(new ScreenTitle());
            }
            else if (Flags.bootRoutine == BootRoutine.Editor_Room)
            {
                ExitAndLoad(new ScreenEditor());
            }
            else if (Flags.bootRoutine == BootRoutine.Editor_Level)
            {
                ExitAndLoad(new ScreenEditor());
            }
            else if (Flags.bootRoutine == BootRoutine.Editor_Map)
            {
                ExitAndLoad(new Overworld_ShadowKing());
            }
        }

        public static void Update(GameTime GameTime)
        {
            gameTime = GameTime;

            //set global inputs prior to screens processing
            Functions_Input.Update();
            //fuzz hero input if testing
            if (Flags.FuzzyInput) { Functions_Input.FuzzInput(Pool.hero.compInput); }

            if (screens.Count > 0)
            {
                //the only 'active screen' is the last one (top one)
                activeScreen = screens[screens.Count - 1];
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
            game.GraphicsDevice.Clear(ColorScheme.background);

            //each screen handles opening and closing the spriteBatch for drawing
            //this allows screens to use camera matrices to draw world views
            foreach (Screen screen in screens) { screen.Draw(gameTime); }

            //Draw input, watermark, and cursor + tooltip
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);


            #region Draw Input Display

            if (Flags.DrawInput)
            {
                Functions_Draw.Draw(InputDisplay.directionalBkg);
                Functions_Draw.Draw(InputDisplay.buttonBkg);
                InputDisplay.ReadInput(); //set directions+buttons b4 draw
                Functions_Draw.Draw(InputDisplay.directions);
                Functions_Draw.Draw(InputDisplay.buttons);
            }

            #endregion


            #region Draw Watermark

            if (Flags.DrawWatermark)
            {
                Functions_Draw.Draw(WaterMark.display);
            }

            #endregion


            if (Flags.Release == false) //ignore cursor in release mode
            {   

                #region Position & Draw Cursor

                if (TopDebugMenu.objToolState == ObjToolState.MoveObj) //check move state
                {   //if moving, show open hand cursor
                    TopDebugMenu.cursor.currentFrame = AnimationFrames.Ui_Hand_Open[0];
                    //if dragging, show grab cursor
                    if (Functions_Input.IsMouseButtonDown(MouseButtons.LeftButton))
                    { TopDebugMenu.cursor.currentFrame = AnimationFrames.Ui_Hand_Grab[0]; }
                }
                else
                {   //default to pointer
                    TopDebugMenu.cursor.currentFrame = AnimationFrames.Ui_Hand_Point[0];
                    //if clicking/dragging, show pointer press cursor
                    if (Functions_Input.IsMouseButtonDown(MouseButtons.LeftButton))
                    { TopDebugMenu.cursor.currentFrame = AnimationFrames.Ui_Hand_Press[0]; }
                }
                //match cursor to mouse pos + toolTip
                TopDebugMenu.cursor.position.X = Input.cursorPos.X;
                TopDebugMenu.cursor.position.Y = Input.cursorPos.Y;
                if (TopDebugMenu.objToolState != ObjToolState.MoveObj)
                {   //apply offset for pointer sprite
                    TopDebugMenu.cursor.position.X += 3;
                    TopDebugMenu.cursor.position.Y += 6;
                }

                Functions_Draw.Draw(TopDebugMenu.cursor);

                #endregion


                #region Position & Draw ToolTip

                //match position of cursor sprite to cursor
                TopDebugMenu.toolTipSprite.position.X = Input.cursorPos.X + 12;
                TopDebugMenu.toolTipSprite.position.Y = Input.cursorPos.Y - 0;

                //set toolTip's animation frame based on objToolState
                if (TopDebugMenu.objToolState == ObjToolState.AddObj)
                { TopDebugMenu.toolTipSprite.currentFrame = AnimationFrames.Ui_Add[0]; }
                else if (TopDebugMenu.objToolState == ObjToolState.DeleteObj)
                { TopDebugMenu.toolTipSprite.currentFrame = AnimationFrames.Ui_Delete[0]; }
                else { TopDebugMenu.toolTipSprite.currentFrame = AnimationFrames.Ui_Rotate[0]; }

                //draw
                if (TopDebugMenu.objToolState != ObjToolState.MoveObj)
                { Functions_Draw.Draw(TopDebugMenu.toolTipSprite); }

                #endregion

            }


            spriteBatch.End();
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
        {   
            screen.Open(); //this resets the screen to opening state
            screens.Add(screen); //add the global screen instance to the active list
        }

        public static void RemoveScreen(Screen screen)
        {
            screen.Close();
            screens.Remove(screen);
        }

        public static void ExitAndLoad(Screen screenToLoad)
        {   //remove every screen on screens list
            while (screens.Count > 0)
            {
                screens[0].Close();
                screens.Remove(screens[0]);
            }
            AddScreen(screenToLoad);
        }

    }
}