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
    public static class DebugMenu
    {

        public static Rectangle rec; //background rec
        public static List<ComponentButton> buttons;
        public static int counter;

        public static void Initialize()
        {
            rec = new Rectangle(0, 0, 640, 13);
            buttons = new List<ComponentButton>();
            buttons.Add(new ComponentButton("draw collisions", new Point(2, 2)));
        }

        public static void HandleInput()
        {
            //check input for buttons
            //set button state
        }

        public static void Update()
        {

        }

        public static void Draw()
        {
            //draw the background rec with correct color
            ScreenManager.spriteBatch.Draw(
                Assets.dummyTexture, rec,
                Assets.colorScheme.windowBkg);
            //loop draw all the buttons
            for (counter = 0; counter < buttons.Count; counter++)
            { DrawFunctions.Draw(buttons[counter]); }
        }

    }
}
