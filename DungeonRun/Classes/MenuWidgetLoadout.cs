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

        //pointers to the loadout menuItems
        public static MenuItem item;
        public static MenuItem weapon;
        public static MenuItem armor;
        public static MenuItem equipment;

        public static int goldTracker;
        public static ComponentAmountDisplay goldDisplay;



        static MenuWidgetLoadout()
        {
            window = new MenuWindow(new Point(-100, -100),
                new Point(100, 100), "Loadout");

            //create menuitems
            menuItems = new List<MenuItem>();
            for (i = 0; i < 8; i++) { menuItems.Add(new MenuItem()); }
            //set loadout pointers
            item = menuItems[0];
            weapon = menuItems[1];
            armor = menuItems[2];
            equipment = menuItems[3];

            //create gold amount display
            goldDisplay = new ComponentAmountDisplay(0, 0, 0);
        }

        public static void Reset(Point Position)
        {   //align this widgets component to Position + Size
            window.ResetAndMoveWindow(Position,
                new Point(16 * 6 + 8, 16 * 5 + 8), 
                "Loadout");


            #region Place first row of menuItems

            menuItems[0].compSprite.position.X = window.background.position.X + 16 * 1;
            menuItems[0].compSprite.position.Y = window.background.position.Y + 16 * 2;

            menuItems[1].compSprite.position.X = menuItems[0].compSprite.position.X + 24;
            menuItems[1].compSprite.position.Y = menuItems[0].compSprite.position.Y;

            menuItems[2].compSprite.position.X = menuItems[1].compSprite.position.X + 24;
            menuItems[2].compSprite.position.Y = menuItems[0].compSprite.position.Y;

            menuItems[3].compSprite.position.X = menuItems[2].compSprite.position.X + 24;
            menuItems[3].compSprite.position.Y = menuItems[0].compSprite.position.Y;

            #endregion


            #region Place second row of menuItems

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

            UpdateLoadout();
            //set the inventory heart pieces frame, based on the number of heart pieces hero has
            //add the piece counter to the current X frame, pieceCounter will always be less than 5
            menuItems[5].compAnim.currentAnimation = new List<Byte4>
            { new Byte4((byte)(11 + WorldUI.pieceCounter), 6, 0, 0) };
            AnimationFunctions.Animate(menuItems[5].compAnim, menuItems[5].compSprite);

            //place the goldAmount text component & bkg to the gold menuItem
            goldDisplay.Move(menuItems[4]);
            //initially display the player's gold
            goldTracker = PlayerData.saveData.gold;
            UpdateGoldAmount();
        }



        public static void SetLoadoutItem(MenuItem Item, MenuItemType Type)
        {
            if (Item.type != Type)
            {
                MenuItemFunctions.SetMenuItemData(Type, Item);
                Item.compSprite.scale = 2.0f;
            }
        }

        public static void UpdateGoldAmount()
        {   //display the gold amount with a prefix of 0, if needed
            if (goldTracker < 10) { goldDisplay.amount.text = "0" + goldTracker; }
            else { goldDisplay.amount.text = "" + goldTracker; }
        }

        public static void UpdateLoadout()
        {
            //set the loadout display based on hero's loadout
            SetLoadoutItem(item, Pool.hero.item);
            SetLoadoutItem(weapon, Pool.hero.weapon);
            SetLoadoutItem(armor, Pool.hero.armor);
            SetLoadoutItem(equipment, Pool.hero.equipment);

            //set the gold, hearts, and dungeon items
            MenuItemFunctions.SetMenuItemData(MenuItemType.InventoryGold, menuItems[4]);
            MenuItemFunctions.SetMenuItemData(MenuItemType.InventoryHeartPieces, menuItems[5]);
            if (DungeonFunctions.dungeon.map) //if player found the map, display it
            { MenuItemFunctions.SetMenuItemData(MenuItemType.InventoryMap, menuItems[6]); }
            else { MenuItemFunctions.SetMenuItemData(MenuItemType.Unknown, menuItems[6]); }
            if (DungeonFunctions.dungeon.bigKey) //if player found the key, display it
            { MenuItemFunctions.SetMenuItemData(MenuItemType.InventoryKey, menuItems[7]); }
            else { MenuItemFunctions.SetMenuItemData(MenuItemType.Unknown, menuItems[7]); }
        }



        public static void Update()
        {
            window.Update();
            //scale the loadout sprites back down to 1.0
            AnimationFunctions.Animate(item.compAnim, item.compSprite);
            AnimationFunctions.Animate(weapon.compAnim, weapon.compSprite);
            AnimationFunctions.Animate(armor.compAnim, armor.compSprite);
            AnimationFunctions.Animate(equipment.compAnim, equipment.compSprite);
            //animate the gold menuItem to grab the player's attention
            AnimationFunctions.Animate(menuItems[4].compAnim, menuItems[4].compSprite);
            if(goldTracker != PlayerData.saveData.gold)
            {   //count the gold amount up or down
                if (goldTracker < PlayerData.saveData.gold) { goldTracker++; }
                else if (goldTracker > PlayerData.saveData.gold) { goldTracker--; }
                UpdateGoldAmount();
                //randomly play the gold sound effect
                if (GetRandom.Int(0, 100) > 60) { Assets.Play(Assets.sfxGoldPickup); }
            }
        }

        public static void Draw()
        {
            DrawFunctions.Draw(window);
            if (window.interior.displayState == DisplayState.Opened)
            {
                for (i = 0; i < menuItems.Count; i++)
                { DrawFunctions.Draw(menuItems[i].compSprite); }
                DrawFunctions.Draw(goldDisplay);
            }
        }

    }
}