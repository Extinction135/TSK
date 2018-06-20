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
            Functions_MenuWindow.ResetAndMove(window, X, Y, 
                new Point(16 * 8, 16 * 5 + 8), "For Sale");


            #region Place first row of menuItems

            menuItems[0].compSprite.position.X = window.background.position.X + 16 * 1;
            menuItems[0].compSprite.position.Y = window.background.position.Y + 16 * 2;
            Functions_MenuItem.SetType(MenuItemType.Unknown, menuItems[0]);

            menuItems[1].compSprite.position.X = menuItems[0].compSprite.position.X + 24;
            menuItems[1].compSprite.position.Y = menuItems[0].compSprite.position.Y;
            Functions_MenuItem.SetType(MenuItemType.Unknown, menuItems[1]);

            menuItems[2].compSprite.position.X = menuItems[1].compSprite.position.X + 24;
            menuItems[2].compSprite.position.Y = menuItems[0].compSprite.position.Y;
            Functions_MenuItem.SetType(MenuItemType.Unknown, menuItems[2]);

            menuItems[3].compSprite.position.X = menuItems[2].compSprite.position.X + 24;
            menuItems[3].compSprite.position.Y = menuItems[0].compSprite.position.Y;
            Functions_MenuItem.SetType(MenuItemType.Unknown, menuItems[3]);

            menuItems[4].compSprite.position.X = menuItems[3].compSprite.position.X + 24;
            menuItems[4].compSprite.position.Y = menuItems[0].compSprite.position.Y;
            Functions_MenuItem.SetType(MenuItemType.Unknown, menuItems[4]);

            #endregion


            #region Place second row of menuItems

            menuItems[5].compSprite.position.X = window.background.position.X + 16 * 1;
            menuItems[5].compSprite.position.Y = window.background.position.Y + 16 * 3 + 8;
            Functions_MenuItem.SetType(MenuItemType.Unknown, menuItems[5]);

            menuItems[6].compSprite.position.X = menuItems[5].compSprite.position.X + 24;
            menuItems[6].compSprite.position.Y = menuItems[5].compSprite.position.Y;
            Functions_MenuItem.SetType(MenuItemType.Unknown, menuItems[6]);

            menuItems[7].compSprite.position.X = menuItems[6].compSprite.position.X + 24;
            menuItems[7].compSprite.position.Y = menuItems[5].compSprite.position.Y;
            Functions_MenuItem.SetType(MenuItemType.Unknown, menuItems[7]);

            menuItems[8].compSprite.position.X = menuItems[7].compSprite.position.X + 24;
            menuItems[8].compSprite.position.Y = menuItems[5].compSprite.position.Y;
            Functions_MenuItem.SetType(MenuItemType.Unknown, menuItems[8]);

            menuItems[9].compSprite.position.X = menuItems[8].compSprite.position.X + 24;
            menuItems[9].compSprite.position.Y = menuItems[5].compSprite.position.Y;
            Functions_MenuItem.SetType(MenuItemType.Unknown, menuItems[9]);

            #endregion


            //set the menuItem's neighbors
            Functions_MenuItem.SetNeighbors(menuItems, 5);
            //place the amounts relative to the menuItems
            for (i = 0; i < 10; i++)
            { Functions_Component.Align(amounts[i], menuItems[i].compSprite); }
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
                Functions_MenuItem.SetType(MenuItemType.Unknown, menuItems[i]);
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
        {   //reset all the menuItems to unknown
            ResetItemsForSale();


            #region Set MenuItems based on VendorType

            if (VendorType == ObjType.Vendor_NPC_Items)
            {
                Functions_MenuItem.SetType(MenuItemType.ItemHeart, menuItems[0]);
                //skip menuItems[1]
                //skip menuItems[2]
                Functions_MenuItem.SetType(MenuItemType.ItemBomb, menuItems[3]);
                Functions_MenuItem.SetType(MenuItemType.ItemBomb3Pack, menuItems[4]);
                //row 2
                Functions_MenuItem.SetType(MenuItemType.ItemBow, menuItems[5]);
                Functions_MenuItem.SetType(MenuItemType.ItemArrowPack, menuItems[6]);
            }
            else if (VendorType == ObjType.Vendor_NPC_Potions)
            {
                Functions_MenuItem.SetType(MenuItemType.BottleHealth, menuItems[0]);
                Functions_MenuItem.SetType(MenuItemType.BottleMagic, menuItems[1]);
                Functions_MenuItem.SetType(MenuItemType.BottleCombo, menuItems[2]);
            }
            else if (VendorType == ObjType.Vendor_NPC_Magic)
            {
                Functions_MenuItem.SetType(MenuItemType.MagicFireball, menuItems[0]);
                Functions_MenuItem.SetType(MenuItemType.MagicBombos, menuItems[1]);
                Functions_MenuItem.SetType(MenuItemType.MagicBolt, menuItems[2]);
            }
            else if (VendorType == ObjType.Vendor_NPC_Weapons)
            {
                Functions_MenuItem.SetType(MenuItemType.WeaponNet, menuItems[0]);
                Functions_MenuItem.SetType(MenuItemType.WeaponShovel, menuItems[1]);
            }
            else if (VendorType == ObjType.Vendor_NPC_Armor)
            {
                Functions_MenuItem.SetType(MenuItemType.ArmorCape, menuItems[0]);
            }
            else if (VendorType == ObjType.Vendor_NPC_Equipment)
            {
                Functions_MenuItem.SetType(MenuItemType.EquipmentRing, menuItems[0]);
                //MenuItemFunctions.SetType(MenuItemType.EquipmentPearl, menuItems[1]);
                //MenuItemFunctions.SetType(MenuItemType.EquipmentNecklace, menuItems[2]);
                //MenuItemFunctions.SetType(MenuItemType.EquipmentGlove, menuItems[3]);
                //MenuItemFunctions.SetType(MenuItemType.EquipmentPin, menuItems[4]);
            }
            else if (VendorType == ObjType.Vendor_NPC_Pets)
            {
                Functions_MenuItem.SetType(MenuItemType.PetDog_Gray, menuItems[0]);
                Functions_MenuItem.SetType(MenuItemType.PetDog_Gray, menuItems[1]);
                return; //prevents pet's cost from being displayed
            }


            else if (VendorType == ObjType.Vendor_Colliseum_Mob)
            {
                Functions_MenuItem.SetType(MenuItemType.Challenge_Blobs, menuItems[0]);
                Functions_MenuItem.SetType(MenuItemType.Challenge_Minibosses, menuItems[1]);
                Functions_MenuItem.SetType(MenuItemType.Challenge_Bosses, menuItems[5]);
            }



            #endregion


            for (i = 0; i < 10; i++)
            {   //if any menuItem is known, display it's cost
                if (menuItems[i].type != MenuItemType.Unknown)
                { DisplayItemCost(menuItems[i], amounts[i]); }
            }
        }

    }
}