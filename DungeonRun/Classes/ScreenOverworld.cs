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
    public class ScreenOverworld : Screen
    {


        public static MenuWindow window;


        public ScreenOverworld() { this.name = "OverworldScreen"; }

        public override void LoadContent()
        {
            window = new MenuWindow(new Point(16 * 5, 16 * 1),
                new Point(16 * 31, 16 * 20 + 8), "Overworld Map");
        }

        public override void HandleInput(GameTime GameTime)
        {
            //dont do anything right now
            if (Input.IsNewButtonPress(Buttons.Start) ||
                Input.IsNewButtonPress(Buttons.A) ||
                Input.IsNewButtonPress(Buttons.B))
            {
                //displayState = DisplayState.Closing;
                //play the summary exit sound effect immediately
                //Assets.Play(Assets.sfxExitSummary);
                ScreenManager.RemoveScreen(this);
            }

        }

        public override void Update(GameTime GameTime)
        {
            window.Update();
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            DrawFunctions.Draw(window);

            ScreenManager.spriteBatch.End();
        }

    }
}
