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
    public static class MenuWidgetDialog
    {

        public static MenuWindow window;



        static MenuWidgetDialog()
        {
            window = new MenuWindow(new Point(-100, -100),
                new Point(100, 100), "Info Window");
        }

        public static void Reset(Point Position)
        {   //align this widgets component to Position + Size
            window.ResetAndMoveWindow(Position,
                new Point(16 * 22, 16 * 4),
                "Dialog Window");
        }

        public static void Update()
        {
            window.Update();
        }

        public static void Draw()
        {
            DrawFunctions.Draw(window);
            if (window.interior.displayState == DisplayState.Opened)
            {
                //
            }
        }

    }
}