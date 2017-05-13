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
    public static class MenuWidgetStats
    {

        static int i;
        public static MenuWindow window;
        public static List<MenuItem> menuItems;



        static MenuWidgetStats()
        {
            window = new MenuWindow(new Point(-100, -100),
                new Point(100, 100), "Stats");

            //create menuitems
            menuItems = new List<MenuItem>();
            for (i = 0; i < 4; i++) { menuItems.Add(new MenuItem()); }
        }

        public static void Reset(Point Position)
        {   //align this widgets component to Position + Size
            window.ResetAndMoveWindow(Position,
                new Point(16 * 6 + 8, 16 * 8 + 8), 
                "Stats");


            #region Place the menuItems

            menuItems[0].compSprite.position.X = window.background.position.X + 16 * 1;
            menuItems[0].compSprite.position.Y = window.background.position.Y + 16 * 2;

            menuItems[1].compSprite.position.X = menuItems[0].compSprite.position.X;
            menuItems[1].compSprite.position.Y = menuItems[0].compSprite.position.Y + 24;

            menuItems[2].compSprite.position.X = menuItems[1].compSprite.position.X;
            menuItems[2].compSprite.position.Y = menuItems[1].compSprite.position.Y + 24;

            menuItems[3].compSprite.position.X = menuItems[2].compSprite.position.X;
            menuItems[3].compSprite.position.Y = menuItems[2].compSprite.position.Y + 24;

            #endregion


            //set the menuItem's neighbors
            Functions_MenuItem.SetNeighbors(menuItems, 1);
            //set the menuItem's data
            //MenuItemFunctions.SetMenuItemData(MenuItemType.StatsHealth, menuItems[0]);
            //MenuItemFunctions.SetMenuItemData(MenuItemType.StatsMagic, menuItems[1]);
            //MenuItemFunctions.SetMenuItemData(MenuItemType.StatsAgility, menuItems[2]);
            //MenuItemFunctions.SetMenuItemData(MenuItemType.Stats4, menuItems[3]);
        }

        public static void Update()
        {
            window.Update();
        }

        public static void Draw()
        {
            Functions_Draw.Draw(window);
            if (window.interior.displayState == DisplayState.Opened)
            {
                for (i = 0; i < menuItems.Count; i++)
                { Functions_Draw.Draw(menuItems[i].compSprite); }
            }
        }

    }
}