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
    public class WidgetLoadout : Widget
    {
        public List<MenuItem> menuItems;
        //pointers to the loadout menuItems
        public MenuItem item;
        public MenuItem weapon;
        public MenuItem armor;
        public MenuItem equipment;

        public int goldTracker;
        public ComponentAmountDisplay goldDisplay;



        public WidgetLoadout()
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

        public override void Reset(int X, int Y)
        {   //align this widgets component to Position + Size
            Functions_MenuWindow.ResetAndMove(window, X, Y, 
                new Point(16 * 6 + 8, 16 * 5 + 8), "Loadout");


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
            Functions_MenuItem.SetNeighbors(menuItems, 4);

            UpdateLoadout();

            //display the player's gold, place gold display with gold menuItem
            goldTracker = PlayerData.current.gold;
            Functions_Component.UpdateAmount(goldDisplay, goldTracker);
            Functions_Component.Move(goldDisplay, menuItems[4]);
        }

        public override void Update()
        {
            Functions_MenuWindow.Update(window);
            //scale the loadout sprites back down to 1.0
            Functions_Animation.Animate(item.compAnim, item.compSprite);
            Functions_Animation.Animate(weapon.compAnim, weapon.compSprite);
            Functions_Animation.Animate(armor.compAnim, armor.compSprite);
            Functions_Animation.Animate(equipment.compAnim, equipment.compSprite);
            //animate the gold menuItem to grab the player's attention
            Functions_Animation.Animate(menuItems[4].compAnim, menuItems[4].compSprite);
            if (goldTracker != PlayerData.current.gold)
            {   //count the gold amount up or down
                if (goldTracker < PlayerData.current.gold) { goldTracker++; }
                else if (goldTracker > PlayerData.current.gold) { goldTracker--; }
                Functions_Component.UpdateAmount(goldDisplay, goldTracker);
                //randomly play the gold sound effect
                if (Functions_Random.Int(0, 100) > 60) { Assets.Play(Assets.sfxGoldPickup); }
            }
        }

        public override void Draw()
        {
            Functions_Draw.Draw(window);
            if (window.interior.displayState == DisplayState.Opened)
            {
                for (i = 0; i < menuItems.Count; i++)
                { Functions_Draw.Draw(menuItems[i].compSprite); }
                Functions_Draw.Draw(goldDisplay);
            }
        }



        public void SetLoadoutItem(MenuItem Item, MenuItemType Type)
        {
            if (Item.type != Type)
            {
                Functions_MenuItem.SetMenuItemData(Type, Item);
                Item.compSprite.scale = 2.0f;
            }
        }

        public void UpdateLoadout()
        {
            //set the loadout display based on hero's loadout
            SetLoadoutItem(item, Pool.hero.item);
            SetLoadoutItem(weapon, Pool.hero.weapon);
            SetLoadoutItem(armor, Pool.hero.armor);
            SetLoadoutItem(equipment, Pool.hero.equipment);

            //set the gold, hearts, and dungeon items
            Functions_MenuItem.SetMenuItemData(MenuItemType.InventoryGold, menuItems[4]);
            Functions_MenuItem.SetMenuItemData(MenuItemType.Unknown, menuItems[5]); //unused for now
            if (Functions_Dungeon.dungeon.map) //if player found the map, display it
            { Functions_MenuItem.SetMenuItemData(MenuItemType.InventoryMap, menuItems[6]); }
            else { Functions_MenuItem.SetMenuItemData(MenuItemType.Unknown, menuItems[6]); }
            if (Functions_Dungeon.dungeon.bigKey) //if player found the key, display it
            { Functions_MenuItem.SetMenuItemData(MenuItemType.InventoryKey, menuItems[7]); }
            else { Functions_MenuItem.SetMenuItemData(MenuItemType.Unknown, menuItems[7]); }
        }

    }
}