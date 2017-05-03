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

        public ScreenOverworld() { this.name = "OverworldScreen"; }

        public override void LoadContent()
        {
            window = new MenuWindow(new Point(16 * 11 + 8, 16 * 1 + 8),
                new Point(16 * 17, 16 * 19), "Overworld Map");
            map = new ComponentSprite(Assets.overworldSheet, 
                new Vector2(window.border.position.X + 8, window.border.position.Y + 24), 
                new Byte4(0, 0, 0, 0), new Byte2(255, 255));
            map.position.X += map.cellSize.x / 2;
            map.position.Y += map.cellSize.y / 2;
            selectedLocation = new ComponentText(Assets.font, "Dungeon 1", 
                new Vector2(window.border.position.X + 16 * 7 + 8, window.footerLine.position.Y - 1), 
                Assets.colorScheme.textDark);
        }

        public override void HandleInput(GameTime GameTime)
        {
            if (Input.IsNewButtonPress(Buttons.Start) ||
                Input.IsNewButtonPress(Buttons.A) ||
                Input.IsNewButtonPress(Buttons.B))
            {
                //displayState = DisplayState.Closing;
                //play the summary exit sound effect immediately
                //Assets.Play(Assets.sfxExitSummary);
                ScreenManager.RemoveScreen(this);
                DungeonFunctions.BuildDungeon();
            }
        }

        public override void Update(GameTime GameTime)
        {
            window.Update();

            //center the location text
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

            ScreenManager.spriteBatch.End();
        }

    }
}
