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
    public class ScreenOverworld : Screen
    {

        public static MenuWindow window;
        public static ComponentSprite map;
        public static ComponentText selectedLocation;

        //the foreground black rectangle, overlays and hides screen content
        Rectangle overlay;
        public float overlayAlpha = 0.0f;
        float fadeInSpeed = 0.05f;



        public ScreenOverworld() { this.name = "OverworldScreen"; }

        public override void LoadContent()
        {
            overlay = new Rectangle(0, 0, 640, 360);

            window = new MenuWindow(new Point(16 * 11 + 8, 16 * 1 + 8),
                new Point(16 * 17, 16 * 19), "Overworld Map");
            map = new ComponentSprite(Assets.overworldSheet, 
                new Vector2(window.border.position.X + 7, window.border.position.Y + 24), 
                new Byte4(0, 0, 0, 0), new Point(256, 256));
            map.position.X += map.cellSize.X / 2;
            map.position.Y += map.cellSize.Y / 2;
            selectedLocation = new ComponentText(Assets.font, "Dungeon 1", 
                new Vector2(window.border.position.X + 16 * 7 + 8, window.footerLine.position.Y - 1), 
                Assets.colorScheme.textDark);
            Assets.Play(Assets.sfxMapOpen);

            //open the screen
            displayState = DisplayState.Opening;
        }

        public override void HandleInput(GameTime GameTime)
        {
            if (displayState == DisplayState.Opened)
            {   //only allow input if the screen has opened completely
                if (Input.IsNewButtonPress(Buttons.Start) ||
                Input.IsNewButtonPress(Buttons.A) ||
                Input.IsNewButtonPress(Buttons.B))
                {
                    //displayState = DisplayState.Closing;
                    //play the summary exit sound effect immediately
                    //Assets.Play(Assets.sfxExitSummary);

                    //begin closing the screen
                    displayState = DisplayState.Closing;
                }
            }
        }

        public override void Update(GameTime GameTime)
        {
            window.Update();

            //center the location text


            #region Handle Screen State

            if (displayState == DisplayState.Opening)
            {
                if (window.interior.displayState == DisplayState.Opened)
                { displayState = DisplayState.Opened; }
            }
            else if (displayState == DisplayState.Closing)
            {
                overlayAlpha += fadeInSpeed;
                if (overlayAlpha >= 1.0f)
                {
                    overlayAlpha = 1.0f;
                    displayState = DisplayState.Closed;
                }
            }
            else if (displayState == DisplayState.Closed)
            {
                ScreenManager.RemoveScreen(this);
                DungeonFunctions.BuildDungeon();
            }

            #endregion


        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            DrawFunctions.Draw(window);

            if (window.interior.displayState == DisplayState.Opened)
            {
                DrawFunctions.Draw(map);
                DrawFunctions.Draw(selectedLocation);
            }

            //draw the overlay rec last
            ScreenManager.spriteBatch.Draw(Assets.dummyTexture,
                overlay, Assets.colorScheme.overlay * overlayAlpha);

            ScreenManager.spriteBatch.End();
        }

    }
}
