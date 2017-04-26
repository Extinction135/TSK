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
    public static class MenuWidgetInventory
    {

        static int i;
        public static MenuWindow window;
        public static List<MenuRectangle> dividers;
        public static List<ComponentText> labels; //weapons, armor, equipment texts
        public static List<MenuItem> menuItems;



        static MenuWidgetInventory()
        {
            window = new MenuWindow(new Point(-100, -100),
                new Point(100, 100), "Info Window");

            //create dividers
            dividers = new List<MenuRectangle>();
            for (i = 0; i < 6; i++)
            {
                dividers.Add(new MenuRectangle(new Point(0, 0), 
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

            //throw some junk data into the items for checking
            menuItems[0].name = "Boomerang";
            menuItems[0].description = "a magical boomerang\nthat always returns";
            menuItems[0].compSprite.currentFrame = new Byte4(5, 5, 0, 0);
        }

        public static void ResetDivider(MenuRectangle Divider, int X, int Y, int Width)
        {
            Divider.openDelay = window.headerLine.openDelay;
            Divider.position.X = X;
            Divider.position.Y = Y;
            Divider.size.X = Width;
            Divider.size.Y = 1;
            Divider.Reset();
        }

        public static void PlaceRow(int index, int X, int Y)
        {   //place the menuItem index at X, Y
            menuItems[index].compSprite.position.X = X;
            menuItems[index].compSprite.position.Y = Y;
            //align the rest of the row
            PlaceMenuItem(menuItems[index+1], menuItems[index]);
            PlaceMenuItem(menuItems[index+2], menuItems[index+1]);
            PlaceMenuItem(menuItems[index+3], menuItems[index+2]);
            PlaceMenuItem(menuItems[index+4], menuItems[index+3]);
        }

        public static void PlaceMenuItem(MenuItem Child, MenuItem Parent)
        {
            Child.compSprite.position.X = Parent.compSprite.position.X + 24;
            Child.compSprite.position.Y = Parent.compSprite.position.Y;
        }

        public static void Reset(Point Position, Point Size)
        {   //align this widgets component to Position + Size
            window.ResetAndMoveWindow(Position, Size, "Items");


            #region Reset and align dividers

            //set1 - weapons
            ResetDivider(dividers[0], Position.X + 8, Position.Y + 16 * 4 + 8, Size.X - 16);
            ResetDivider(dividers[1], Position.X + 8, Position.Y + 16 * 5 + 8, Size.X - 16);
            //set2 - armor
            ResetDivider(dividers[2], Position.X + 8, Position.Y + 16 * 7 + 8, Size.X - 16);
            ResetDivider(dividers[3], Position.X + 8, Position.Y + 16 * 8 + 8, Size.X - 16);
            //set3 - equipment
            ResetDivider(dividers[4], Position.X + 8, Position.Y + 16 * 10 + 8, Size.X - 16);
            ResetDivider(dividers[5], Position.X + 8, Position.Y + 16 * 11 + 8, Size.X - 16);

            #endregion


            #region Place the Labels

            //place labels (X)
            labels[0].position.X = Position.X + 8;
            labels[1].position.X = labels[0].position.X;
            labels[2].position.X = labels[0].position.X;
            //place labels (Y)
            labels[0].position.Y = dividers[0].position.Y + 1;
            labels[1].position.Y = dividers[2].position.Y + 1;
            labels[2].position.Y = dividers[4].position.Y + 1;

            #endregion


            //place the menuItems
            PlaceRow(00, Position.X + 16 * 1, Position.Y + 16 * 02 + 0);
            PlaceRow(05, Position.X + 16 * 1, Position.Y + 16 * 03 + 8);
            PlaceRow(10, Position.X + 16 * 1, Position.Y + 16 * 06 + 8);
            PlaceRow(15, Position.X + 16 * 1, Position.Y + 16 * 09 + 8);
            PlaceRow(20, Position.X + 16 * 1, Position.Y + 16 * 12 + 8);
            
            //set the menuItem's neighbors
            MenuItemFunctions.SetNeighbors(menuItems, 5);

            #region Set the menuItem data (based on the hero's inventory - later)

            MenuItemFunctions.SetMenuItemData(MenuItemType.ItemBoomerang, menuItems[0]);
            MenuItemFunctions.SetMenuItemData(MenuItemType.ItemBomb, menuItems[1]);
            MenuItemFunctions.SetMenuItemData(MenuItemType.BottleEmpty, menuItems[2]);
            MenuItemFunctions.SetMenuItemData(MenuItemType.BottleHealth, menuItems[3]);
            //MenuItemFunctions.SetMenuItemData(MenuItemType.BottleEmpty, menuItems[4]);

            MenuItemFunctions.SetMenuItemData(MenuItemType.MagicFireball, menuItems[5]);
            //MenuItemFunctions.SetMenuItemData(MenuItemType.MagicFireball, menuItems[6]);
            //MenuItemFunctions.SetMenuItemData(MenuItemType.MagicFireball, menuItems[7]);
            //MenuItemFunctions.SetMenuItemData(MenuItemType.MagicFireball, menuItems[8]);
            //MenuItemFunctions.SetMenuItemData(MenuItemType.MagicFireball, menuItems[9]);

            MenuItemFunctions.SetMenuItemData(MenuItemType.WeaponSword, menuItems[10]);
            MenuItemFunctions.SetMenuItemData(MenuItemType.WeaponBow, menuItems[11]);
            //MenuItemFunctions.SetMenuItemData(MenuItemType.WeaponSword, menuItems[12]);
            //MenuItemFunctions.SetMenuItemData(MenuItemType.WeaponSword, menuItems[13]);
            //MenuItemFunctions.SetMenuItemData(MenuItemType.WeaponSword, menuItems[14]);

            MenuItemFunctions.SetMenuItemData(MenuItemType.ArmorCloth, menuItems[15]);
            //MenuItemFunctions.SetMenuItemData(MenuItemType.ArmorCloth, menuItems[16]);
            //MenuItemFunctions.SetMenuItemData(MenuItemType.ArmorCloth, menuItems[17]);
            //MenuItemFunctions.SetMenuItemData(MenuItemType.ArmorCloth, menuItems[18]);
            //MenuItemFunctions.SetMenuItemData(MenuItemType.ArmorCloth, menuItems[19]);

            MenuItemFunctions.SetMenuItemData(MenuItemType.EquipmentRing, menuItems[20]);
            //MenuItemFunctions.SetMenuItemData(MenuItemType.EquipmentRing, menuItems[21]);
            //MenuItemFunctions.SetMenuItemData(MenuItemType.EquipmentRing, menuItems[22]);
            //MenuItemFunctions.SetMenuItemData(MenuItemType.EquipmentRing, menuItems[23]);
            //MenuItemFunctions.SetMenuItemData(MenuItemType.EquipmentRing, menuItems[24]);

            #endregion

        }

        public static void Update()
        {
            window.Update();
            for (i = 0; i < dividers.Count; i++) { dividers[i].Update(); }
        }

        public static void Draw()
        {
            DrawFunctions.Draw(window);
            for (i = 0; i < dividers.Count; i++) { DrawFunctions.Draw(dividers[i]); }
            if (window.interior.displayState == DisplayState.Opened)
            {
                for (i = 0; i < labels.Count; i++)
                { DrawFunctions.Draw(labels[i]); }
                for (i = 0; i < menuItems.Count; i++)
                { DrawFunctions.Draw(menuItems[i].compSprite); }
            }
        }

    }
}