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
    public class WidgetCrystals : Widget
    {
        public List<MenuItem> menuItems;



        public WidgetCrystals()
        {
            window = new MenuWindow(new Point(-100, -100),
                new Point(100, 100), "Crystals");

            //create menuitems
            menuItems = new List<MenuItem>();
            for (i = 0; i < 6; i++) { menuItems.Add(new MenuItem()); }
        }

        public override void Reset(int X, int Y)
        {   //align this widgets component to Position + Size
            Functions_MenuWindow.ResetAndMove(window, X, Y, 
                new Point(16 * 6 + 8, 16 * 4), "Crystals");


            #region Place the crystal menuItems

            menuItems[0].compSprite.position.X = window.background.position.X + 13;
            menuItems[1].compSprite.position.X = menuItems[0].compSprite.position.X + 15;
            menuItems[2].compSprite.position.X = menuItems[1].compSprite.position.X + 16;
            menuItems[3].compSprite.position.X = menuItems[2].compSprite.position.X + 16;
            menuItems[4].compSprite.position.X = menuItems[3].compSprite.position.X + 16;
            menuItems[5].compSprite.position.X = menuItems[4].compSprite.position.X + 15;

            menuItems[0].compSprite.position.Y = window.background.position.Y + 16 * 2;
            menuItems[1].compSprite.position.Y = menuItems[0].compSprite.position.Y;
            menuItems[2].compSprite.position.Y = menuItems[0].compSprite.position.Y;
            menuItems[3].compSprite.position.Y = menuItems[0].compSprite.position.Y;
            menuItems[4].compSprite.position.Y = menuItems[0].compSprite.position.Y;
            menuItems[5].compSprite.position.Y = menuItems[0].compSprite.position.Y;

            #endregion


            //set the menuItem's neighbors
            Functions_MenuItem.SetNeighbors(menuItems, 6);
            //set the menuItem's data
            Functions_MenuItem.SetMenuItemData(MenuItemType.CrystalFilled, menuItems[0]);
            Functions_MenuItem.SetMenuItemData(MenuItemType.CrystalEmpty, menuItems[1]);
            Functions_MenuItem.SetMenuItemData(MenuItemType.CrystalEmpty, menuItems[2]);
            Functions_MenuItem.SetMenuItemData(MenuItemType.CrystalEmpty, menuItems[3]);
            Functions_MenuItem.SetMenuItemData(MenuItemType.CrystalEmpty, menuItems[4]);
            Functions_MenuItem.SetMenuItemData(MenuItemType.CrystalEmpty, menuItems[5]);
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
            }
        }

    }
}