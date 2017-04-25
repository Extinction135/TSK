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
    public static class MenuWidgetInfo
    {

        public static MenuWindow window;
        public static ComponentSprite compSprite;
        public static ComponentText description;
        public static MenuRectangle divider1;



        static MenuWidgetInfo()
        {
            window = new MenuWindow(new Point(-100, -100), 
                new Point(100, 100), "Info Window");
            compSprite = new ComponentSprite(Assets.mainSheet, 
                new Vector2(-100, 1000), new Byte4(15, 5, 0, 0), 
                new Byte2(16, 16));
            description = new ComponentText(Assets.font, 
                "default description \ntext here...", new Vector2(-100, -100), 
                Assets.colorScheme.textDark);
            divider1 = new MenuRectangle(new Point(-100, -100), 
                new Point(0, 0), Assets.colorScheme.windowInset);
        }

        public static void Reset(Point Position, Point Size)
        {   //align this widgets component to Position + Size
            window.ResetAndMoveWindow(Position, Size, "Info Window");
            compSprite.position.X = Position.X + 16 * 3 + 4;
            compSprite.position.Y = Position.Y + 16 * 2;
            description.position.X = Position.X + 8;
            description.position.Y = Position.Y + 16 * 3;
            //reset and align the divider line
            divider1.openDelay = window.headerLine.openDelay;
            divider1.position.X = Position.X + 8;
            divider1.position.Y = Position.Y + 16 * 3;
            divider1.size.X = Size.X - 16;
            divider1.size.Y = 1;
            divider1.Reset();
        }

        public static void Display(MenuItem MenuItem)
        {   //set the widget's components based on the MenuItem's fields
            window.title.text = MenuItem.name;
            compSprite.currentFrame = MenuItem.compSprite.currentFrame;
            description.text = MenuItem.description;
        }

        public static void Update()
        {
            window.Update();
            divider1.Update();
        }

        public static void Draw()
        {
            DrawFunctions.Draw(window);
            DrawFunctions.Draw(divider1);
            if(window.interior.displayState == DisplayState.Opened)
            {
                DrawFunctions.Draw(compSprite);
                DrawFunctions.Draw(description);
            }
        }

    }
}