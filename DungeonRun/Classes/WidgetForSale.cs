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
    public class WidgetForSale : Widget
    {
        public List<MenuItem> menuItems;
        public List<ComponentAmountDisplay> amounts;



        public WidgetForSale()
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

        public override void Reset(int X, int Y)
        {   //align this widgets component to Position
            window.ResetAndMove(X, Y, new Point(16 * 8, 16 * 5 + 8), "For Sale");


            #region Place first row of menuItems

            menuItems[0].compSprite.position.X = window.background.position.X + 16 * 1;
            menuItems[0].compSprite.position.Y = window.background.position.Y + 16 * 2;
            Functions_MenuItem.SetMenuItemData(MenuItemType.Unknown, menuItems[0]);

            menuItems[1].compSprite.position.X = menuItems[0].compSprite.position.X + 24;
            menuItems[1].compSprite.position.Y = menuItems[0].compSprite.position.Y;
            Functions_MenuItem.SetMenuItemData(MenuItemType.Unknown, menuItems[1]);

            menuItems[2].compSprite.position.X = menuItems[1].compSprite.position.X + 24;
            menuItems[2].compSprite.position.Y = menuItems[0].compSprite.position.Y;
            Functions_MenuItem.SetMenuItemData(MenuItemType.Unknown, menuItems[2]);

            menuItems[3].compSprite.position.X = menuItems[2].compSprite.position.X + 24;
            menuItems[3].compSprite.position.Y = menuItems[0].compSprite.position.Y;
            Functions_MenuItem.SetMenuItemData(MenuItemType.Unknown, menuItems[3]);

            menuItems[4].compSprite.position.X = menuItems[3].compSprite.position.X + 24;
            menuItems[4].compSprite.position.Y = menuItems[0].compSprite.position.Y;
            Functions_MenuItem.SetMenuItemData(MenuItemType.Unknown, menuItems[4]);

            #endregion


            #region Place second row of menuItems

            menuItems[5].compSprite.position.X = window.background.position.X + 16 * 1;
            menuItems[5].compSprite.position.Y = window.background.position.Y + 16 * 3 + 8;
            Functions_MenuItem.SetMenuItemData(MenuItemType.Unknown, menuItems[5]);

            menuItems[6].compSprite.position.X = menuItems[5].compSprite.position.X + 24;
            menuItems[6].compSprite.position.Y = menuItems[5].compSprite.position.Y;
            Functions_MenuItem.SetMenuItemData(MenuItemType.Unknown, menuItems[6]);

            menuItems[7].compSprite.position.X = menuItems[6].compSprite.position.X + 24;
            menuItems[7].compSprite.position.Y = menuItems[5].compSprite.position.Y;
            Functions_MenuItem.SetMenuItemData(MenuItemType.Unknown, menuItems[7]);

            menuItems[8].compSprite.position.X = menuItems[7].compSprite.position.X + 24;
            menuItems[8].compSprite.position.Y = menuItems[5].compSprite.position.Y;
            Functions_MenuItem.SetMenuItemData(MenuItemType.Unknown, menuItems[8]);

            menuItems[9].compSprite.position.X = menuItems[8].compSprite.position.X + 24;
            menuItems[9].compSprite.position.Y = menuItems[5].compSprite.position.Y;
            Functions_MenuItem.SetMenuItemData(MenuItemType.Unknown, menuItems[9]);

            #endregion


            //set the menuItem's neighbors
            Functions_MenuItem.SetNeighbors(menuItems, 5);
            //place the amounts relative to the menuItems
            for (i = 0; i < 10; i++)
            { Functions_Component.Move(amounts[i], menuItems[i]); }
        }

        public override void Update()
        {
            window.Update();
        }

        public override void Draw()
        {
            Functions_Draw.Draw(window);
            if (window.interior.displayState == DisplayState.Opened)
            {
                for (i = 0; i < 10; i++)
                {
                    Functions_Draw.Draw(menuItems[i].compSprite);
                    if (amounts[i].visible) { Functions_Draw.Draw(amounts[i]); }
                }
            }
        }



        public void ResetItemsForSale()
        { 
            for (i = 0; i < 10; i++)
            {
                Functions_MenuItem.SetMenuItemData(MenuItemType.Unknown, menuItems[i]);
                amounts[i].amount.text = "";
                amounts[i].visible = false;
            }
        }

        public void DisplayItemCost(MenuItem Item, ComponentAmountDisplay Amount)
        {
            Amount.visible = true;
            Functions_Component.UpdateAmount(Amount, Item.price);
        }

        public void SetItemsForSale(ObjType VendorType)
        {
            //reset all the menuItems to unknown
            ResetItemsForSale();

            //check if hero has any vendor items, via PlayerData.saveData.(itemBoolean)
            if (VendorType == ObjType.VendorItems)
            {
                //MenuItemFunctions.SetMenuItemData(MenuItemType.ItemBoomerang, menuItems[0]);
                if (PlayerData.saveData.bombsCurrent < PlayerData.saveData.bombsMax)
                {   //if the hero has room to purchase bombs, display them
                    Functions_MenuItem.SetMenuItemData(MenuItemType.ItemBomb, menuItems[1]);
                    Functions_MenuItem.SetMenuItemData(MenuItemType.ItemBombs, menuItems[2]);
                }
            }
            else if (VendorType == ObjType.VendorPotions)
            {
                if (!PlayerData.saveData.bottleHealth)
                { Functions_MenuItem.SetMenuItemData(MenuItemType.BottleHealth, menuItems[0]); }
                if (!PlayerData.saveData.bottleMagic)
                { Functions_MenuItem.SetMenuItemData(MenuItemType.BottleMagic, menuItems[1]); }
                if (!PlayerData.saveData.bottleFairy)
                { Functions_MenuItem.SetMenuItemData(MenuItemType.BottleFairy, menuItems[2]); }
            }
            else if (VendorType == ObjType.VendorMagic)
            {
                if (!PlayerData.saveData.magicFireball)
                { Functions_MenuItem.SetMenuItemData(MenuItemType.MagicFireball, menuItems[0]); }
            }
            else if (VendorType == ObjType.VendorWeapons)
            {
                if (!PlayerData.saveData.weaponBow)
                { Functions_MenuItem.SetMenuItemData(MenuItemType.WeaponBow, menuItems[0]); }
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
                { Functions_MenuItem.SetMenuItemData(MenuItemType.EquipmentRing, menuItems[0]); }
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

    }
}