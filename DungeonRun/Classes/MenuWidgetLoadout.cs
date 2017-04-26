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
    public static class MenuWidgetLoadout
    {

        static int i;
        public static MenuWindow window;
        public static List<MenuItem> menuItems;
        public static ComponentText goldAmount;
        public static Rectangle goldBkg;


        static MenuWidgetLoadout()
        {
            window = new MenuWindow(new Point(-100, -100),
                new Point(100, 100), "Loadout");

            //create menuitems
            menuItems = new List<MenuItem>();
            for (i = 0; i < 8; i++) { menuItems.Add(new MenuItem()); }

            //throw some junk data into the items for checking
            menuItems[0].name = "Boomerang";
            menuItems[0].description = "a magical boomerang\nthat always returns";
            menuItems[0].compSprite.currentFrame = new Byte4(5, 5, 0, 0);

            //create the gold amount text and background
            goldAmount = new ComponentText(Assets.font, "999", 
                new Vector2(0, 0), Assets.colorScheme.textLight);
            goldBkg = new Rectangle(0, 0, 13, 7);
        }

        public static void Reset(Point Position, Point Size)
        {   //align this widgets component to Position + Size
            window.ResetAndMoveWindow(Position, Size, "Loadout");


            #region Place first row of loadout menuItems

            menuItems[0].compSprite.position.X = window.background.position.X + 16 * 1;
            menuItems[0].compSprite.position.Y = window.background.position.Y + 16 * 2;

            menuItems[1].compSprite.position.X = menuItems[0].compSprite.position.X + 24;
            menuItems[1].compSprite.position.Y = menuItems[0].compSprite.position.Y;

            menuItems[2].compSprite.position.X = menuItems[1].compSprite.position.X + 24;
            menuItems[2].compSprite.position.Y = menuItems[0].compSprite.position.Y;

            menuItems[3].compSprite.position.X = menuItems[2].compSprite.position.X + 24;
            menuItems[3].compSprite.position.Y = menuItems[0].compSprite.position.Y;

            #endregion


            #region Place second row of loadout menuItems

            menuItems[4].compSprite.position.X = window.background.position.X + 16 * 1;
            menuItems[4].compSprite.position.Y = window.background.position.Y + 16 * 3 + 8;

            menuItems[5].compSprite.position.X = menuItems[4].compSprite.position.X + 24;
            menuItems[5].compSprite.position.Y = menuItems[4].compSprite.position.Y;

            menuItems[6].compSprite.position.X = menuItems[5].compSprite.position.X + 24;
            menuItems[6].compSprite.position.Y = menuItems[4].compSprite.position.Y;

            menuItems[7].compSprite.position.X = menuItems[6].compSprite.position.X + 24;
            menuItems[7].compSprite.position.Y = menuItems[4].compSprite.position.Y;

            #endregion


            //set the menuItem's neighbors
            MenuItemFunctions.SetNeighbors(menuItems, 4);
            //set the menuItem's data
            MenuItemFunctions.SetMenuItemData(MenuItemType.ItemBoomerang, menuItems[0]);
            MenuItemFunctions.SetMenuItemData(MenuItemType.WeaponSword, menuItems[1]);
            MenuItemFunctions.SetMenuItemData(MenuItemType.ArmorCloth, menuItems[2]);
            MenuItemFunctions.SetMenuItemData(MenuItemType.EquipmentRing, menuItems[3]);
            MenuItemFunctions.SetMenuItemData(MenuItemType.InventoryGold, menuItems[4]);
            MenuItemFunctions.SetMenuItemData(MenuItemType.InventoryHeartPieces, menuItems[5]);
            if (DungeonFunctions.dungeon.map) //if player found the map, display it
            { MenuItemFunctions.SetMenuItemData(MenuItemType.InventoryMap, menuItems[6]); }
            if (DungeonFunctions.dungeon.bigKey) //if player found the key, display it
            { MenuItemFunctions.SetMenuItemData(MenuItemType.InventoryKey, menuItems[7]); }

            //place the goldAmount text component & bkg to the gold menuItem
            goldAmount.position.X = menuItems[4].compSprite.position.X - 5;
            goldAmount.position.Y = menuItems[4].compSprite.position.Y - 3;
            goldBkg.X = (int)goldAmount.position.X - 1;
            goldBkg.Y = (int)goldAmount.position.Y + 4;
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
                //draw gold amount with background
                ScreenManager.spriteBatch.Draw(
                    Assets.dummyTexture, goldBkg,
                    Assets.colorScheme.textDark);
                DrawFunctions.Draw(goldAmount);
            }
        }

    }
}