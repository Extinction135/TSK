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
        public static List<ComponentAmountDisplay> amounts;



        static MenuWidgetForSale()
        {
            window = new MenuWindow(new Point(-100, -100),
                new Point(100, 100), "");

            //create menuitems
            menuItems = new List<MenuItem>();
            for (i = 0; i < 10; i++) { menuItems.Add(new MenuItem()); }
            //create amounts
            amounts = new List<ComponentAmountDisplay>();
            for (i = 0; i < 10; i++) { amounts.Add(new ComponentAmountDisplay(0,0,0)); }
        }

        public static void Reset(Point Position)
        {   //align this widgets component to Position
            window.ResetAndMoveWindow(Position, 
                new Point(16 * 8, 16 * 5 + 8), "For Sale");


            #region Place first row of menuItems

            menuItems[0].compSprite.position.X = window.background.position.X + 16 * 1;
            menuItems[0].compSprite.position.Y = window.background.position.Y + 16 * 2;
            MenuItemFunctions.SetMenuItemData(MenuItemType.Unknown, menuItems[0]);

            menuItems[1].compSprite.position.X = menuItems[0].compSprite.position.X + 24;
            menuItems[1].compSprite.position.Y = menuItems[0].compSprite.position.Y;
            MenuItemFunctions.SetMenuItemData(MenuItemType.Unknown, menuItems[1]);

            menuItems[2].compSprite.position.X = menuItems[1].compSprite.position.X + 24;
            menuItems[2].compSprite.position.Y = menuItems[0].compSprite.position.Y;
            MenuItemFunctions.SetMenuItemData(MenuItemType.Unknown, menuItems[2]);

            menuItems[3].compSprite.position.X = menuItems[2].compSprite.position.X + 24;
            menuItems[3].compSprite.position.Y = menuItems[0].compSprite.position.Y;
            MenuItemFunctions.SetMenuItemData(MenuItemType.Unknown, menuItems[3]);

            menuItems[4].compSprite.position.X = menuItems[3].compSprite.position.X + 24;
            menuItems[4].compSprite.position.Y = menuItems[0].compSprite.position.Y;
            MenuItemFunctions.SetMenuItemData(MenuItemType.Unknown, menuItems[4]);

            #endregion


            #region Place second row of menuItems

            menuItems[5].compSprite.position.X = window.background.position.X + 16 * 1;
            menuItems[5].compSprite.position.Y = window.background.position.Y + 16 * 3 + 8;
            MenuItemFunctions.SetMenuItemData(MenuItemType.Unknown, menuItems[5]);

            menuItems[6].compSprite.position.X = menuItems[5].compSprite.position.X + 24;
            menuItems[6].compSprite.position.Y = menuItems[5].compSprite.position.Y;
            MenuItemFunctions.SetMenuItemData(MenuItemType.Unknown, menuItems[6]);

            menuItems[7].compSprite.position.X = menuItems[6].compSprite.position.X + 24;
            menuItems[7].compSprite.position.Y = menuItems[5].compSprite.position.Y;
            MenuItemFunctions.SetMenuItemData(MenuItemType.Unknown, menuItems[7]);

            menuItems[8].compSprite.position.X = menuItems[7].compSprite.position.X + 24;
            menuItems[8].compSprite.position.Y = menuItems[5].compSprite.position.Y;
            MenuItemFunctions.SetMenuItemData(MenuItemType.Unknown, menuItems[8]);

            menuItems[9].compSprite.position.X = menuItems[8].compSprite.position.X + 24;
            menuItems[9].compSprite.position.Y = menuItems[5].compSprite.position.Y;
            MenuItemFunctions.SetMenuItemData(MenuItemType.Unknown, menuItems[9]);

            #endregion


            //set the menuItem's neighbors
            MenuItemFunctions.SetNeighbors(menuItems, 5);
            //place the amounts relative to the menuItems
            for (i = 0; i < 10; i++) { amounts[i].Move(menuItems[i]); }
        }



        public static void ResetItemsForSale()
        { 
            for (i = 0; i < 10; i++)
            {
                MenuItemFunctions.SetMenuItemData(MenuItemType.Unknown, menuItems[i]);
                amounts[i].amount.text = "";
                amounts[i].visible = false;
            }
        }

        public static void DisplayItemCost(MenuItem Item, ComponentAmountDisplay Amount)
        {
            Amount.visible = true;
            Amount.UpdateAmount(Item.price);
        }

        public static void SetItemsForSale(ObjType VendorType)
        {
            //reset all the menuItems to unknown
            ResetItemsForSale();

            //check if hero has any vendor items, via PlayerData.saveData.(itemBoolean)
            if (VendorType == ObjType.VendorItems)
            {
                //MenuItemFunctions.SetMenuItemData(MenuItemType.ItemBoomerang, menuItems[0]);
                if (PlayerData.saveData.bombsCurrent < PlayerData.saveData.bombsMax)
                {   //if the hero has room to purchase bombs, display them
                    MenuItemFunctions.SetMenuItemData(MenuItemType.ItemBomb, menuItems[1]);
                    MenuItemFunctions.SetMenuItemData(MenuItemType.ItemBombs, menuItems[2]);
                }
            }
            else if (VendorType == ObjType.VendorPotions)
            {
                if (!PlayerData.saveData.bottleHealth)
                { MenuItemFunctions.SetMenuItemData(MenuItemType.BottleHealth, menuItems[0]); }
                if (!PlayerData.saveData.bottleMagic)
                { MenuItemFunctions.SetMenuItemData(MenuItemType.BottleMagic, menuItems[1]); }
                if (!PlayerData.saveData.bottleFairy)
                { MenuItemFunctions.SetMenuItemData(MenuItemType.BottleFairy, menuItems[2]); }
            }
            else if (VendorType == ObjType.VendorMagic)
            {
                if (!PlayerData.saveData.magicFireball)
                { MenuItemFunctions.SetMenuItemData(MenuItemType.MagicFireball, menuItems[0]); }
            }
            else if (VendorType == ObjType.VendorWeapons)
            {
                //MenuItemFunctions.SetMenuItemData(MenuItemType.WeaponBow, menuItems[0]);
                //MenuItemFunctions.SetMenuItemData(MenuItemType.WeaponStaff, menuItems[1]);
            }
            else if (VendorType == ObjType.VendorArmor)
            {
                //MenuItemFunctions.SetMenuItemData(MenuItemType.ArmorChest, menuItems[0]);
                //MenuItemFunctions.SetMenuItemData(MenuItemType.ArmorCape, menuItems[1]);
                //MenuItemFunctions.SetMenuItemData(MenuItemType.ArmorRobe, menuItems[2]);
            }
            else if (VendorType == ObjType.VendorEquipment)
            {
                if (!PlayerData.saveData.equipmentRing)
                { MenuItemFunctions.SetMenuItemData(MenuItemType.EquipmentRing, menuItems[0]); }
                //MenuItemFunctions.SetMenuItemData(MenuItemType.EquipmentPearl, menuItems[1]);
                //MenuItemFunctions.SetMenuItemData(MenuItemType.EquipmentNecklace, menuItems[2]);
                //MenuItemFunctions.SetMenuItemData(MenuItemType.EquipmentGlove, menuItems[3]);
                //MenuItemFunctions.SetMenuItemData(MenuItemType.EquipmentPin, menuItems[4]);
            }


            for (i = 0; i < 10; i++)
            {   //if any menuItem is known, display it's cost
                if (menuItems[i].type != MenuItemType.Unknown)
                { DisplayItemCost(menuItems[i], amounts[i]); }
            }
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
                for (i = 0; i < 10; i++)
                {
                    DrawFunctions.Draw(menuItems[i].compSprite);
                    if (amounts[i].visible) { DrawFunctions.Draw(amounts[i]); }
                }
            }
        }

    }
}