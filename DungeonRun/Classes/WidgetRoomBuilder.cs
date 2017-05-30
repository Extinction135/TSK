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
    public class WidgetRoomBuilder : Widget
    {

        public WidgetRoomBuilder()
        {
            window = new MenuWindow(
                new Point(-100, -100), 
                new Point(100, 100), "");

        }

        public override void Reset(int X, int Y)
        {   //align this widgets component to Position + Size
            window.ResetAndMove(X, Y,
                new Point(16 * 6, 16 * 14 + 8), 
                "Room Builder");
        }

        public override void Update()
        {
            window.Update();

        }

        public override void Draw()
        {
            Functions_Draw.Draw(window);
        }

    }
}