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
        public static GameObject speaker;


        static MenuWidgetDialog()
        {
            window = new MenuWindow(new Point(-100, -100), new Point(100, 100), "");
            speaker = new GameObject(Assets.mainSheet);
            GameObjectFunctions.SetType(speaker, ObjType.VendorArmor);
        }

        public static void Reset(Point Position)
        {   //align this widgets component to Position + Size
            window.ResetAndMoveWindow(Position,
                new Point(16 * 22, 16 * 4),
                "Dialog Window");
            speaker.compSprite.position.X = Position.X + 16 * 1;
            speaker.compSprite.position.Y = Position.Y + 16 * 2;
        }


        public static void DisplayDialog(ObjType Type, String Dialog)
        {   //set the menuItem's type and sprite frame
            GameObjectFunctions.SetType(speaker, Type);
        }



        public static void Update()
        {
            window.Update();
            AnimationFunctions.Animate(speaker.compAnim, speaker.compSprite);
        }

        public static void Draw()
        {
            DrawFunctions.Draw(window);
            if (window.interior.displayState == DisplayState.Opened)
            {
                //
                DrawFunctions.Draw(speaker.compSprite);
            }
        }

    }
}