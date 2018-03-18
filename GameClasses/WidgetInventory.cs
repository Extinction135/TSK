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
        public ComponentAmountDisplay bombsDisplay;
        public ComponentAmountDisplay arrowsDisplay;



        public WidgetInventory()
        {
            window = new MenuWindow(new Point(-100, -100),
                new Point(100, 100), "Info Window");
            //create dividers
            for (i = 0; i < 4; i++)
            {
                window.lines.Add(new MenuRectangle(new Point(0, 0), 
                    new Point(0, 0), Assets.colorScheme.windowInset));
            }
            //create labels
            labels = new List<ComponentText>();
            labels.Add(new ComponentText(Assets.font, "weapons", 
                new Vector2(0,0), Assets.colorScheme.textDark));
            labels.Add(new ComponentText(Assets.font, "equipment",
                new Vector2(0, 0), Assets.colorScheme.textDark));
            //create menuitems
            menuItems = new List<MenuItem>();
            for (i = 0; i < (7 * 6); i++) { menuItems.Add(new MenuItem()); }
            //create amount displays
            bombsDisplay = new ComponentAmountDisplay(0, 0, 0);
            arrowsDisplay = new ComponentAmountDisplay(0, 0, 0);
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
                new Point(16 * 11, 16 * 14 + 8), "Items");
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

            //reset the menuItems
            for (i = 0; i < menuItems.Count; i++)
            { Functions_MenuItem.SetType(MenuItemType.Unknown, menuItems[i]); }
            bombsDisplay.visible = true; //bomb amount is always displayed
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

            #region Items - 0 thru 3, 7 thru 10, 14 thru 20

            //0 = boomerang
            Functions_MenuItem.SetType(MenuItemType.ItemBoomerang, menuItems[0]);
            //1 = bombs
            Functions_MenuItem.SetType(MenuItemType.ItemBomb, menuItems[1]);
            Functions_Component.Align(bombsDisplay, menuItems[1].compSprite);
            Functions_Component.UpdateAmount(bombsDisplay, PlayerData.current.bombsCurrent);
            //2 - magic mirror
            //3 - ???

            //7 - hookshot
            //8 - net
            if (PlayerData.current.weaponNet)
            { Functions_MenuItem.SetType(MenuItemType.WeaponNet, menuItems[8]); }
            //9 - ?
            //10 - ???

            //14 - mirror shield
            //15 - cape
            //16 - firerod
            //17 - icerod
            //18 - shovel
            //19 - cane1
            //20 - cane2

            #endregion


            #region Bottles - 4, 5, 6

            Functions_Bottle.SetMenuItemType(menuItems[4], PlayerData.current.bottleA);
            Functions_Bottle.SetMenuItemType(menuItems[5], PlayerData.current.bottleB);
            Functions_Bottle.SetMenuItemType(menuItems[6], PlayerData.current.bottleC);

            #endregion


            #region Set the magic medallion items - 11, 12, 13

            if (PlayerData.current.magicFireball)
            { Functions_MenuItem.SetType(MenuItemType.MagicFireball, menuItems[11]); }

            #endregion


            #region Weapons - 21 thru 27

            //21 - sword
            Functions_MenuItem.SetType(MenuItemType.WeaponSword, menuItems[21]);
            //22 - bow and arrow
            if (PlayerData.current.weaponBow)
            {   //if hero has arrows, display the number of arrows + draw display amount
                Functions_MenuItem.SetType(MenuItemType.WeaponBow, menuItems[22]);
                Functions_Component.Align(arrowsDisplay, menuItems[22].compSprite);
                Functions_Component.UpdateAmount(arrowsDisplay, PlayerData.current.arrowsCurrent);
                arrowsDisplay.visible = PlayerData.current.weaponBow; //hero has a bow, show arrow count
            }
            //23 - hammer
            //24 - axe
            //25 - push wand
            //26 - deku stick

            #endregion


            #region Equipment - 28 thru 35 (maybe 42?)

            Functions_MenuItem.SetType(MenuItemType.ArmorCloth, menuItems[28]);
            //29 - ring
            if (PlayerData.current.equipmentRing)
            { Functions_MenuItem.SetType(MenuItemType.EquipmentRing, menuItems[29]); }
            //30 - pearl
            //31 - necklace
            //32 - power glove
            //33 - pin
            //34 - pendant

            //35 - tattered shawl
            if (PlayerData.current.armorCape)
            { Functions_MenuItem.SetType(MenuItemType.ArmorCape, menuItems[35]); }
            //36 - ?
            //37 - ?
            //38 - ?
            //39 - ?
            //40 - ?
            //41 - ?

            #endregion

        }

    }
}