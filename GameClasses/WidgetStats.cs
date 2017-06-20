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
    public class WidgetStats : Widget
    {
        public List<MenuItem> menuItems;
        public ComponentAmountDisplay healthLevel;
        public ComponentAmountDisplay magicLevel;
        public ComponentAmountDisplay agilityLevel;
        public ComponentAmountDisplay luckLevel;


        public WidgetStats()
        {
            window = new MenuWindow(new Point(-100, -100),
                new Point(100, 100), "Stats");

            //create menuitems
            menuItems = new List<MenuItem>();
            for (i = 0; i < 4; i++) { menuItems.Add(new MenuItem()); }

            healthLevel = new ComponentAmountDisplay(0, 0, 0);
            magicLevel = new ComponentAmountDisplay(0, 0, 0);
            agilityLevel = new ComponentAmountDisplay(0, 0, 0);
            luckLevel = new ComponentAmountDisplay(0, 0, 0);
        }

        public override void Reset(int X, int Y)
        {   //align this widgets component to Position + Size
            Functions_MenuWindow.ResetAndMove(window, X, Y, 
                new Point(16 * 6 + 8, 16 * 4), "Stats");


            #region Place the menuItems

            menuItems[0].compSprite.position.X = window.background.position.X + 16 * 1;
            menuItems[1].compSprite.position.X = menuItems[0].compSprite.position.X + 24;
            menuItems[2].compSprite.position.X = menuItems[1].compSprite.position.X + 24;
            menuItems[3].compSprite.position.X = menuItems[2].compSprite.position.X + 24;

            menuItems[0].compSprite.position.Y = window.background.position.Y + 16 * 2;
            menuItems[1].compSprite.position.Y = menuItems[0].compSprite.position.Y;
            menuItems[2].compSprite.position.Y = menuItems[0].compSprite.position.Y;
            menuItems[3].compSprite.position.Y = menuItems[0].compSprite.position.Y;

            #endregion

            //set the menuItem's neighbors
            Functions_MenuItem.SetNeighbors(menuItems, 4);
            //set the menuItem's data
            //MenuItemFunctions.SetMenuItemData(MenuItemType.StatsHealth, menuItems[0]);
            //MenuItemFunctions.SetMenuItemData(MenuItemType.StatsMagic, menuItems[1]);
            //MenuItemFunctions.SetMenuItemData(MenuItemType.StatsAgility, menuItems[2]);
            //MenuItemFunctions.SetMenuItemData(MenuItemType.StatsLuck, menuItems[3]);
            //place amount displays
            Functions_Component.Move(healthLevel, menuItems[0]);
            Functions_Component.Move(magicLevel, menuItems[1]);
            Functions_Component.Move(agilityLevel, menuItems[2]);
            Functions_Component.Move(luckLevel, menuItems[3]);
            //set amount displays value
            Functions_Component.UpdateAmount(healthLevel, 0);
            Functions_Component.UpdateAmount(magicLevel, 0);
            Functions_Component.UpdateAmount(agilityLevel, 0);
            Functions_Component.UpdateAmount(luckLevel, 0);
        }

        public override void Update()
        {
            Functions_MenuWindow.Update(window);
        }

        public override void Draw()
        {
            Functions_Draw.Draw(window);
            if (window.interior.displayState == DisplayState.Opened)
            {
                for (i = 0; i < menuItems.Count; i++)
                { Functions_Draw.Draw(menuItems[i].compSprite); }
                Functions_Draw.Draw(healthLevel);
                Functions_Draw.Draw(magicLevel);
                Functions_Draw.Draw(agilityLevel);
                Functions_Draw.Draw(luckLevel);
            }
        }

    }
}