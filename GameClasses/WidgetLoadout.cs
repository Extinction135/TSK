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
            //place ALL the menuItems
            Functions_MenuItem.PlaceMenuItems(menuItems,
                window.background.position.X + 16 * 1,
                window.background.position.Y + 16 * 2,
                4); ;

            UpdateLoadout();
            //display the player's gold, place gold display with gold menuItem
            goldTracker = PlayerData.current.gold;
            Functions_Component.UpdateAmount(goldDisplay, goldTracker);
            Functions_Component.Align(goldDisplay, menuItems[4].compSprite);
        }

        public override void Update()
        {
            Functions_MenuWindow.Update(window);
            //scale the loadout sprites back down to 1.0
            Functions_Animation.ScaleSpriteDown(item.compSprite);
            Functions_Animation.ScaleSpriteDown(weapon.compSprite);
            Functions_Animation.ScaleSpriteDown(armor.compSprite);
            Functions_Animation.ScaleSpriteDown(equipment.compSprite);
            //animate the gold menuItem to grab the player's attention
            Functions_Animation.Animate(menuItems[4].compAnim, menuItems[4].compSprite);
            Functions_Animation.ScaleSpriteDown(menuItems[4].compSprite);

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
                Functions_MenuItem.SetType(Type, Item);
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

            //set the gold menuItem
            Functions_MenuItem.SetType(MenuItemType.InventoryGold, menuItems[4]);
            
            //set pet menuItem - unknown, chicken, dog
            Functions_MenuItem.SetType(PlayerData.current.petType, menuItems[5]);

            //set dungeon map menuItem
            if (Level.map) //if player found the map, display it
            { Functions_MenuItem.SetType(MenuItemType.InventoryMap, menuItems[6]); }
            else { Functions_MenuItem.SetType(MenuItemType.Unknown, menuItems[6]); }
            //set dungeon key menuItem
            if (Level.bigKey) //if player found the key, display it
            { Functions_MenuItem.SetType(MenuItemType.InventoryKey, menuItems[7]); }
            else { Functions_MenuItem.SetType(MenuItemType.Unknown, menuItems[7]); }
        }

    }
}