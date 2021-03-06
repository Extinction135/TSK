﻿using System;
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
        public int itemCount = 7 * 6;
        public ComponentAmountDisplay bombsDisplay;
        public ComponentAmountDisplay arrowsDisplay;



        public WidgetInventory()
        {
            window = new MenuWindow(new Point(-100, -100),
                new Point(100, 100), "default");
            //create dividers
            for (i = 0; i < 4; i++)
            {
                window.lines.Add(new MenuRectangle(new Point(0, 0), 
                    new Point(0, 0), ColorScheme.windowInset));
            }
            //create labels
            labels = new List<ComponentText>();
            labels.Add(new ComponentText(Assets.font, "ITEMS (X)", 
                new Vector2(0, 0), ColorScheme.textDark));
            labels.Add(new ComponentText(Assets.font, "equipment",
                new Vector2(0, 0), ColorScheme.textDark));
            //create menuitems
            menuItems = new List<MenuItem>();
            for (i = 0; i < itemCount; i++)
            {
                MenuItem mi = new MenuItem();
                mi.id = i; //set id
                menuItems.Add(mi);
            }
            //create amount displays
            bombsDisplay = new ComponentAmountDisplay(0, 4096, 4096);
            arrowsDisplay = new ComponentAmountDisplay(0, 4096, 4096);
        }

        public override void Reset(int X, int Y)
        {   
            //place 'weapons' section title
            window.lines[2].position.Y = Y + 16 * 6 + 0;
            labels[0].position.Y = window.lines[2].position.Y + 1;
            window.lines[3].position.Y = Y + 16 * 7 + 0;
            //place 'equipment' section title
            window.lines[4].position.Y = Y + 16 * 9 + 0;
            labels[1].position.Y = window.lines[4].position.Y + 1;
            window.lines[5].position.Y = Y + 16 * 10 + 0;

            //align this widgets component to Position + Size
            Functions_MenuWindow.ResetAndMove(window, X, Y, 
                new Point(16 * 11, 16 * 14 + 8), "ITEMS (Y)");
            //place labels (X)
            labels[0].position.X = X + 8;
            labels[1].position.X = labels[0].position.X;

            //we place rows like dis' now
            Functions_MenuItem.PlaceRow(menuItems, 7 * 0, X + 16 * 1, Y + 16 * 02 + 0, 7);
            Functions_MenuItem.PlaceRow(menuItems, 7 * 1, X + 16 * 1, Y + 16 * 03 + 8, 7);
            Functions_MenuItem.PlaceRow(menuItems, 7 * 2, X + 16 * 1, Y + 16 * 05 + 0, 7);
            //weapons
            Functions_MenuItem.PlaceRow(menuItems, 7 * 3, X + 16 * 1, Y + 16 * 08 + 0, 7);
            //equipment
            Functions_MenuItem.PlaceRow(menuItems, 7 * 4, X + 16 * 1, Y + 16 * 11 + 0, 7);
            Functions_MenuItem.PlaceRow(menuItems, 7 * 5, X + 16 * 1, Y + 16 * 12 + 8, 7);
            Functions_MenuItem.SetNeighbors(menuItems, 7); //set menuItems neighbors

            //set the inventory widget's menuItems based on saveData booleans
            SetInventoryMenuItems();
        }

        public override void Update()
        {
            Functions_MenuWindow.Update(window);
            for (i = 0; i < itemCount; i++) //scale all the menuItems down each frame
            { Functions_Animation.ScaleSpriteDown(menuItems[i].compSprite); }
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

            #region Reset Inventory MenuItems and Amount Displays

            //reset display amounts to false
            arrowsDisplay.visible = false;
            bombsDisplay.visible = true;
            //reset the menuItems
            for (i = 0; i < menuItems.Count; i++)
            { Functions_MenuItem.SetType(MenuItemType.Unknown, menuItems[i]); }

            #endregion


            //setup menuItems


            #region Items - 0 thru 20

            //0 = boomerang
            Functions_MenuItem.SetType(MenuItemType.ItemBoomerang, menuItems[0]);

            //1 = bombs
            Functions_MenuItem.SetType(MenuItemType.ItemBomb, menuItems[1]);
            Functions_Component.Align(bombsDisplay, menuItems[1].compSprite);
            Functions_Component.UpdateAmount(bombsDisplay, PlayerData.bombsCurrent);

            //2 - arrows
            if (PlayerData.itemBow)
            {   //if hero has arrows, display the number of arrows + draw display amount
                Functions_MenuItem.SetType(MenuItemType.ItemBow, menuItems[2]);
                Functions_Component.Align(arrowsDisplay, menuItems[2].compSprite);
                Functions_Component.UpdateAmount(arrowsDisplay, PlayerData.arrowsCurrent);
                arrowsDisplay.visible = true; //hero has a bow, show arrow count
            }

            //3 - ?

            //Bottles - 4, 5, 6
            //set the hero's inventory bottles to the contents of the saveData bottles
            Functions_MenuItem.SetType(PlayerData.bottleA, menuItems[4]);
            Functions_MenuItem.SetType(PlayerData.bottleB, menuItems[5]);
            Functions_MenuItem.SetType(PlayerData.bottleC, menuItems[6]);

            //7 - firerod
            if (PlayerData.itemFirerod)
            { Functions_MenuItem.SetType(MenuItemType.ItemFirerod, menuItems[7]); }
            //8 - icerod
            if (PlayerData.itemIcerod)
            { Functions_MenuItem.SetType(MenuItemType.ItemIcerod, menuItems[8]); }
            //9 - ?
            //10 - ?

            //Set the magic medallion items - 11, 12, 13
            if (PlayerData.magicBombos)
            { Functions_MenuItem.SetType(MenuItemType.MagicBombos, menuItems[12]); }
            if (PlayerData.magicEther)
            { Functions_MenuItem.SetType(MenuItemType.MagicEther, menuItems[13]); }
            //13


            //14 - magic mirror
            if (PlayerData.itemMagicMirror)
            { Functions_MenuItem.SetType(MenuItemType.ItemMagicMirror, menuItems[14]); }
            //15 - ?
            //16 - magic seed bag
            //17 - 
            //18 - 
            //19 - 
            //20 - reserved for spellbook
            if (PlayerData.itemSpellbook)
            { Functions_MenuItem.SetType(MenuItemType.ItemSpellbook, menuItems[20]); }

            #endregion


            #region Weapons - 21 thru 27

            //21 - sword
            Functions_MenuItem.SetType(MenuItemType.WeaponSword, menuItems[21]);
            //22 - net
            if (PlayerData.weaponNet)
            { Functions_MenuItem.SetType(MenuItemType.WeaponNet, menuItems[22]); }
            //23 - shovel
            if (PlayerData.weaponShovel)
            { Functions_MenuItem.SetType(MenuItemType.WeaponShovel, menuItems[23]); }
            //24 - hammer
            if (PlayerData.weaponHammer)
            { Functions_MenuItem.SetType(MenuItemType.WeaponHammer, menuItems[24]); }
            //25 - 
            //26 -

            
            //27 - WAND
            if (PlayerData.weaponWand)
            { Functions_MenuItem.SetType(MenuItemType.WeaponWand, menuItems[27]); }
            //testing enemy bite/fang weapon
            //Functions_MenuItem.SetType(PlayerData.enemyWeapon, menuItems[27]);

            #endregion


            #region Equipment - 28 thru 35

            //armor/wearables row

            //28 standard cloth
            Functions_MenuItem.SetType(MenuItemType.ArmorCloth, menuItems[28]);
            //29 magical cape
            if (PlayerData.armorCape)
            { Functions_MenuItem.SetType(MenuItemType.ArmorCape, menuItems[29]); }
            //30 - ?
            //31 - ?
            //32 - ?
            //33 - ?
            //34 - ?


            //bottom row - items
            if (PlayerData.equipmentRing)
            { Functions_MenuItem.SetType(MenuItemType.EquipmentRing, menuItems[35]); }
            //36 ?
            //37 - ?
            //38 - ?
            //39 - ?
            //40 - ?
            //41 - ?

            #endregion


        }

    }
}