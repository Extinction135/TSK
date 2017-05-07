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
    public static class MenuWidgetForSale
    {
        static int i;
        public static MenuWindow window;
        public static List<MenuItem> menuItems;



        static MenuWidgetForSale()
        {
            window = new MenuWindow(new Point(-100, -100),
                new Point(100, 100), "For Sale - 000");

            //create menuitems
            menuItems = new List<MenuItem>();
            for (i = 0; i < 10; i++) { menuItems.Add(new MenuItem()); }
        }

        public static void Reset(Point Position)
        {   //align this widgets component to Position
            window.ResetAndMoveWindow(Position, 
                new Point(16 * 8, 16 * 5 + 8), 
                "For Sale - 000");


            #region Place first row of menuItems

            menuItems[0].compSprite.position.X = window.background.position.X + 16 * 1;
            menuItems[0].compSprite.position.Y = window.background.position.Y + 16 * 2;

            menuItems[1].compSprite.position.X = menuItems[0].compSprite.position.X + 24;
            menuItems[1].compSprite.position.Y = menuItems[0].compSprite.position.Y;

            menuItems[2].compSprite.position.X = menuItems[1].compSprite.position.X + 24;
            menuItems[2].compSprite.position.Y = menuItems[0].compSprite.position.Y;

            menuItems[3].compSprite.position.X = menuItems[2].compSprite.position.X + 24;
            menuItems[3].compSprite.position.Y = menuItems[0].compSprite.position.Y;

            menuItems[4].compSprite.position.X = menuItems[3].compSprite.position.X + 24;
            menuItems[4].compSprite.position.Y = menuItems[0].compSprite.position.Y;

            #endregion


            #region Place second row of menuItems

            menuItems[5].compSprite.position.X = window.background.position.X + 16 * 1;
            menuItems[5].compSprite.position.Y = window.background.position.Y + 16 * 3 + 8;

            menuItems[6].compSprite.position.X = menuItems[5].compSprite.position.X + 24;
            menuItems[6].compSprite.position.Y = menuItems[5].compSprite.position.Y;

            menuItems[7].compSprite.position.X = menuItems[6].compSprite.position.X + 24;
            menuItems[7].compSprite.position.Y = menuItems[5].compSprite.position.Y;

            menuItems[8].compSprite.position.X = menuItems[7].compSprite.position.X + 24;
            menuItems[8].compSprite.position.Y = menuItems[5].compSprite.position.Y;

            menuItems[9].compSprite.position.X = menuItems[8].compSprite.position.X + 24;
            menuItems[9].compSprite.position.Y = menuItems[5].compSprite.position.Y;

            #endregion


            //set the menuItem's neighbors
            MenuItemFunctions.SetNeighbors(menuItems, 5);
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
            }
        }

    }
}