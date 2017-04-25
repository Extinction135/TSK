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
    public static class MenuWidgetOptions
    {

        static int i;
        public static MenuWindow window;
        public static List<ComponentText> labels;
        public static List<MenuItem> menuItems;



        static MenuWidgetOptions()
        {
            window = new MenuWindow(new Point(-100, -100),
                new Point(100, 100), "Options");

            //create menuitems
            labels = new List<ComponentText>();
            for (i = 0; i < 8; i++)
            { labels.Add(new ComponentText(Assets.font, 
                "test\ntest", new Vector2(-100, -100), 
                Assets.colorScheme.textDark));
            }

            //create menuitems
            menuItems = new List<MenuItem>();
            for (i = 0; i < 8; i++) { menuItems.Add(new MenuItem()); }
        }

        public static void Reset(Point Position, Point Size)
        {   //align this widgets component to Position + Size
            window.ResetAndMoveWindow(Position, Size, "Options");


            //these should be arranged by ROW, not column, due to how neighbors are set

            menuItems[0].compSprite.position.X = window.background.position.X + 16 * 1;
            menuItems[0].compSprite.position.Y = window.background.position.Y + 16 * 2;
            menuItems[1].compSprite.position.X = menuItems[0].compSprite.position.X + 16 * 3;
            menuItems[1].compSprite.position.Y = menuItems[0].compSprite.position.Y;

            menuItems[2].compSprite.position.X = menuItems[0].compSprite.position.X;
            menuItems[2].compSprite.position.Y = menuItems[0].compSprite.position.Y + 24;
            menuItems[3].compSprite.position.X = menuItems[2].compSprite.position.X + 16 * 3;
            menuItems[3].compSprite.position.Y = menuItems[2].compSprite.position.Y;

            menuItems[4].compSprite.position.X = menuItems[2].compSprite.position.X;
            menuItems[4].compSprite.position.Y = menuItems[2].compSprite.position.Y + 24;
            menuItems[5].compSprite.position.X = menuItems[4].compSprite.position.X + 16 * 3;
            menuItems[5].compSprite.position.Y = menuItems[4].compSprite.position.Y;

            menuItems[6].compSprite.position.X = menuItems[4].compSprite.position.X;
            menuItems[6].compSprite.position.Y = menuItems[4].compSprite.position.Y + 24;
            menuItems[7].compSprite.position.X = menuItems[6].compSprite.position.X + 16 * 3;
            menuItems[7].compSprite.position.Y = menuItems[6].compSprite.position.Y;


            #region Place Labels and set their text strings

            for (i = 0; i < 8; i++)
            {
                labels[i].position.X = menuItems[i].compSprite.position.X + 12;
                labels[i].position.Y = menuItems[i].compSprite.position.Y - 12;
            }

            labels[0].text = "save\ngame";
            labels[1].text = "load\ngame";

            labels[2].text = "video\nctrls";
            labels[3].text = "input\nctrls";

            labels[4].text = "audio\nctrls";
            labels[5].text = "";

            labels[6].text = "";
            labels[7].text = "";

            #endregion


            //set the menuItem's neighbors
            MenuItemFunctions.SetNeighbors(menuItems, 2);
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
                for (i = 0; i < menuItems.Count; i++)
                { DrawFunctions.Draw(menuItems[i].compSprite); }
                for (i = 0; i < labels.Count; i++)
                { DrawFunctions.Draw(labels[i]); }
            }
        }

    }
}