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
    public class WidgetInventory : Widget
    {
        public List<ComponentText> labels; //weapons, armor, equipment texts
        public List<MenuItem> menuItems;
        public ComponentAmountDisplay bombsDisplay;
        public ComponentAmountDisplay arrowsDisplay;



        public WidgetInventory()
        {
            window = new MenuWindow(new Point(-100, -100),
                new Point(100, 100), "Info Window");
            //create dividers
            for (i = 0; i < 6; i++)
            {
                window.lines.Add(new MenuRectangle(new Point(0, 0), 
                    new Point(0, 0), Assets.colorScheme.windowInset));
            }
            //create labels
            labels = new List<ComponentText>();
            labels.Add(new ComponentText(Assets.font, "weapons", 
                new Vector2(0,0), Assets.colorScheme.textDark));
            labels.Add(new ComponentText(Assets.font, "armor",
                new Vector2(0, 0), Assets.colorScheme.textDark));
            labels.Add(new ComponentText(Assets.font, "equipment",
                new Vector2(0, 0), Assets.colorScheme.textDark));
            //create menuitems
            menuItems = new List<MenuItem>();
            for (i = 0; i < 25; i++) { menuItems.Add(new MenuItem()); }
            //create bomb amount display
            bombsDisplay = new ComponentAmountDisplay(0, 0, 0);
            arrowsDisplay = new ComponentAmountDisplay(0, 0, 0);
        }

        public override void Reset(int X, int Y)
        {   //reset additional divider lines
            window.lines[2].position.Y = Y + 16 * 4 + 8;
            window.lines[3].position.Y = Y + 16 * 5 + 8;
            window.lines[4].position.Y = Y + 16 * 7 + 8;
            window.lines[5].position.Y = Y + 16 * 8 + 8;
            window.lines[6].position.Y = Y + 16 * 10 + 8;
            window.lines[7].position.Y = Y + 16 * 11 + 8;
            //align this widgets component to Position + Size
            Functions_MenuWindow.ResetAndMove(window, X, Y, 
                new Point(16 * 8, 16 * 14 + 8), "Items");
            //place labels (X)
            labels[0].position.X = X + 8;
            labels[1].position.X = labels[0].position.X;
            labels[2].position.X = labels[0].position.X;
            //place labels (Y)
            labels[0].position.Y = window.lines[2].position.Y + 1;
            labels[1].position.Y = window.lines[4].position.Y + 1;
            labels[2].position.Y = window.lines[6].position.Y + 1;
            //place the menuItems
            PlaceRow(00, X + 16 * 1, Y + 16 * 02 + 0);
            PlaceRow(05, X + 16 * 1, Y + 16 * 03 + 8);
            PlaceRow(10, X + 16 * 1, Y + 16 * 06 + 8);
            PlaceRow(15, X + 16 * 1, Y + 16 * 09 + 8);
            PlaceRow(20, X + 16 * 1, Y + 16 * 12 + 8);
            //reset the menuItems
            for (i = 0; i < menuItems.Count; i++)
            { Functions_MenuItem.SetMenuItemData(MenuItemType.Unknown, menuItems[i]); }
            //set the menuItem's neighbors
            Functions_MenuItem.SetNeighbors(menuItems, 5);
            //set the amount displays to be hidden, initially
            bombsDisplay.visible = false;
            arrowsDisplay.visible = false;
            //set the inventory widget's menuItems based on saveData booleans
            SetInventoryMenuItems();
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
                for (i = 0; i < labels.Count; i++)
                { Functions_Draw.Draw(labels[i]); }
                for (i = 0; i < menuItems.Count; i++)
                { Functions_Draw.Draw(menuItems[i].compSprite); }
                if (bombsDisplay.visible) { Functions_Draw.Draw(bombsDisplay); }
                if (arrowsDisplay.visible) { Functions_Draw.Draw(arrowsDisplay); }
            }
        }



        public void SetInventoryMenuItems()
        {

            #region Items

            //MenuItemFunctions.SetMenuItemData(MenuItemType.ItemBoomerang, menuItems[0]);
            
            if (PlayerData.current.bombsCurrent > 0)
            {
                Functions_MenuItem.SetMenuItemData(MenuItemType.ItemBomb, menuItems[1]);
                Functions_Component.Align(bombsDisplay, menuItems[1].compSprite);
                //if hero has bombs, display the number of bombs + draw display amount
                Functions_Component.UpdateAmount(bombsDisplay, PlayerData.current.bombsCurrent);
                bombsDisplay.visible = true;
            }

            #endregion


            #region Bottles

            //set the empty bottles based on booleans
            if (PlayerData.current.bottle1)
            { Functions_MenuItem.SetMenuItemData(MenuItemType.BottleEmpty, menuItems[2]); }
            if (PlayerData.current.bottle2)
            { Functions_MenuItem.SetMenuItemData(MenuItemType.BottleEmpty, menuItems[3]); }
            if (PlayerData.current.bottle3)
            { Functions_MenuItem.SetMenuItemData(MenuItemType.BottleEmpty, menuItems[4]); }

            //set the filled bottles based on booleans
            if (PlayerData.current.bottleHealth)
            { Functions_MenuItem.SetMenuItemData(MenuItemType.BottleHealth, menuItems[2]); }
            if (PlayerData.current.bottleMagic)
            { Functions_MenuItem.SetMenuItemData(MenuItemType.BottleMagic, menuItems[3]); }
            if (PlayerData.current.bottleFairy)
            { Functions_MenuItem.SetMenuItemData(MenuItemType.BottleFairy, menuItems[4]); }

            #endregion


            #region Set the magic medallion items

            if (PlayerData.current.magicFireball)
            { Functions_MenuItem.SetMenuItemData(MenuItemType.MagicFireball, menuItems[5]); }

            //MenuItemFunctions.SetMenuItemData(MenuItemType.MagicFireball, menuItems[6]);
            //MenuItemFunctions.SetMenuItemData(MenuItemType.MagicFireball, menuItems[7]);
            //MenuItemFunctions.SetMenuItemData(MenuItemType.MagicFireball, menuItems[8]);
            //MenuItemFunctions.SetMenuItemData(MenuItemType.MagicFireball, menuItems[9]);

            #endregion


            #region Weapons

            Functions_MenuItem.SetMenuItemData(MenuItemType.WeaponSword, menuItems[10]);

            if (PlayerData.current.weaponBow)
            {
                Functions_MenuItem.SetMenuItemData(MenuItemType.WeaponBow, menuItems[11]);
                Functions_Component.Align(arrowsDisplay, menuItems[11].compSprite);
                //if hero has arrows, display the number of arrows + draw display amount
                Functions_Component.UpdateAmount(arrowsDisplay, PlayerData.current.arrowsCurrent);
                arrowsDisplay.visible = true;
            }
            
            //MenuItemFunctions.SetMenuItemData(MenuItemType.WeaponStaff, menuItems[12]);
            //MenuItemFunctions.SetMenuItemData(MenuItemType.WeaponSword, menuItems[13]);
            //MenuItemFunctions.SetMenuItemData(MenuItemType.WeaponSword, menuItems[14]);

            #endregion


            #region Armor

            Functions_MenuItem.SetMenuItemData(MenuItemType.ArmorCloth, menuItems[15]);

            if (PlayerData.current.armorChest)
            { Functions_MenuItem.SetMenuItemData(MenuItemType.ArmorChest, menuItems[16]); }

            if (PlayerData.current.armorCape)
            { Functions_MenuItem.SetMenuItemData(MenuItemType.ArmorCape, menuItems[17]); }

            if (PlayerData.current.armorRobe)
            { Functions_MenuItem.SetMenuItemData(MenuItemType.ArmorRobe, menuItems[18]); }

            #endregion


            #region Equipment

            if (PlayerData.current.equipmentRing)
            { Functions_MenuItem.SetMenuItemData(MenuItemType.EquipmentRing, menuItems[20]); }
            //MenuItemFunctions.SetMenuItemData(MenuItemType.EquipmentPearl, menuItems[21]);
            //MenuItemFunctions.SetMenuItemData(MenuItemType.EquipmentNecklace, menuItems[22]);
            //MenuItemFunctions.SetMenuItemData(MenuItemType.EquipmentGlove, menuItems[23]);
            //MenuItemFunctions.SetMenuItemData(MenuItemType.EquipmentPin, menuItems[24]);

            #endregion

        }

        public void PlaceRow(int index, int X, int Y)
        {   //place the menuItem index at X, Y
            menuItems[index].compSprite.position.X = X;
            menuItems[index].compSprite.position.Y = Y;
            //align the rest of the row
            Functions_MenuItem.PlaceMenuItem(menuItems[index+1], menuItems[index], 24);
            Functions_MenuItem.PlaceMenuItem(menuItems[index+2], menuItems[index+1], 24);
            Functions_MenuItem.PlaceMenuItem(menuItems[index+3], menuItems[index+2], 24);
            Functions_MenuItem.PlaceMenuItem(menuItems[index+4], menuItems[index+3], 24);
        }

    }
}