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
            //create amount displays
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
            { Functions_MenuItem.SetType(MenuItemType.Unknown, menuItems[i]); }
            //set the menuItem's neighbors
            Functions_MenuItem.SetNeighbors(menuItems, 5);
            //align the amount displays
            Functions_Component.Align(bombsDisplay, menuItems[0].compSprite);
            Functions_Component.Align(arrowsDisplay, menuItems[11].compSprite);
            bombsDisplay.visible = true; //bomb amount is always displayed
            arrowsDisplay.visible = false; //arrow amount is displayed if hero has bow
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



        public void SetBottleSprite(MenuItem menuItem, Byte value)
        {
            if (value == 0) { Functions_MenuItem.SetType(MenuItemType.Unknown, menuItem); }
            else if (value == 1) { Functions_MenuItem.SetType(MenuItemType.BottleEmpty, menuItem); }
            else if (value == 2) { Functions_MenuItem.SetType(MenuItemType.BottleHealth, menuItem); }
            else if (value == 3) { Functions_MenuItem.SetType(MenuItemType.BottleMagic, menuItem); }
            else if (value == 4) { Functions_MenuItem.SetType(MenuItemType.BottleCombo, menuItem); }
            else if (value == 5) { Functions_MenuItem.SetType(MenuItemType.BottleFairy, menuItem); }
        }

        public void SetInventoryMenuItems()
        {

            #region Items

            //display the hero's bombs + amount
            Functions_MenuItem.SetType(MenuItemType.ItemBomb, menuItems[0]);
            Functions_Component.UpdateAmount(bombsDisplay, PlayerData.current.bombsCurrent);

            //MenuItemFunctions.SetType(MenuItemType.ItemBoomerang, menuItems[1]);

            #endregion


            #region Bottles

            SetBottleSprite(menuItems[2], PlayerData.current.bottleA);
            SetBottleSprite(menuItems[3], PlayerData.current.bottleB);
            SetBottleSprite(menuItems[4], PlayerData.current.bottleC);

            #endregion


            #region Set the magic medallion items

            if (PlayerData.current.magicFireball)
            { Functions_MenuItem.SetType(MenuItemType.MagicFireball, menuItems[5]); }

            //MenuItemFunctions.SetType(MenuItemType.MagicFireball, menuItems[6]);
            //MenuItemFunctions.SetType(MenuItemType.MagicFireball, menuItems[7]);
            //MenuItemFunctions.SetType(MenuItemType.MagicFireball, menuItems[8]);
            //MenuItemFunctions.SetType(MenuItemType.MagicFireball, menuItems[9]);

            #endregion


            #region Weapons

            Functions_MenuItem.SetType(MenuItemType.WeaponSword, menuItems[10]);

            if (PlayerData.current.weaponBow)
            {   //if hero has arrows, display the number of arrows + draw display amount
                Functions_MenuItem.SetType(MenuItemType.WeaponBow, menuItems[11]);
                Functions_Component.UpdateAmount(arrowsDisplay, PlayerData.current.arrowsCurrent);
                arrowsDisplay.visible = true;
            }

            if(PlayerData.current.weaponNet)
            { Functions_MenuItem.SetType(MenuItemType.WeaponNet, menuItems[12]); }

            //MenuItemFunctions.SetType(MenuItemType.WeaponSword, menuItems[13]);
            //MenuItemFunctions.SetType(MenuItemType.WeaponSword, menuItems[14]);

            #endregion


            #region Armor

            Functions_MenuItem.SetType(MenuItemType.ArmorCloth, menuItems[15]);

            if (PlayerData.current.armorChest)
            { Functions_MenuItem.SetType(MenuItemType.ArmorChest, menuItems[16]); }

            if (PlayerData.current.armorCape)
            { Functions_MenuItem.SetType(MenuItemType.ArmorCape, menuItems[17]); }

            if (PlayerData.current.armorRobe)
            { Functions_MenuItem.SetType(MenuItemType.ArmorRobe, menuItems[18]); }

            #endregion


            #region Equipment

            if (PlayerData.current.equipmentRing)
            { Functions_MenuItem.SetType(MenuItemType.EquipmentRing, menuItems[20]); }
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