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
    public static class MenuItemFunctions
    {

        static int i;
        static int rowCounter;

        public static void SetNeighbors(List<MenuItem> MenuItems, int rowLength)
        {
            rowCounter = 0;

            //takes a list of menuItems, and sets their neighbors based on a row width value
            for (i = 0; i < MenuItems.Count; i++)
            {
                //set right neighbor
                if (i + 1 < MenuItems.Count)
                { MenuItems[i].neighborRight = MenuItems[i + 1]; }
                //set down neighbor
                if (i + rowLength < MenuItems.Count)
                { MenuItems[i].neighborDown = MenuItems[i + rowLength]; }
                //set left neighbor
                if (i - 1 >= 0)
                { MenuItems[i].neighborLeft = MenuItems[i - 1]; }
                //set up neighbor
                if (i - rowLength >= 0)
                { MenuItems[i].neighborUp = MenuItems[i - rowLength]; }

                //disable neighbor wrapping
                if(rowCounter == 0) //if this menuItem is the first on it's row
                {
                    MenuItems[i].neighborLeft = MenuItems[i]; //discard left neighbor (no wrapping)
                }
                rowCounter++; //track where we are in the row
                if (rowCounter == rowLength) //if this menuItem is the last on it's row
                {
                    MenuItems[i].neighborRight = MenuItems[i]; //discard right neighbor (no wrapping)
                    rowCounter = 0; //reset the rowCounter
                }
            }
        }

        public static void SetMenuItemData(MenuItemType Type, MenuItem MenuItem)
        {
            //set the MenuItem data based on the passed Type
            MenuItem.type = Type;


            #region Item menuItems

            if (Type == MenuItemType.ItemBoomerang)
            {
                MenuItem.name = "Magic Boomerang";
                MenuItem.description = "A magical boomerang.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(5, 5, 0, 0) };
            }
            else if (Type == MenuItemType.ItemBomb)
            {
                MenuItem.name = "Explosive Bomb";
                MenuItem.description = "An explosive bomb.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(5, 6, 0, 0) };
            }

            #endregion


            #region Bottle menuItems

            else if (Type == MenuItemType.BottleEmpty)
            {
                MenuItem.name = "Empty Bottle";
                MenuItem.description = "An empty bottle.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(6, 5, 0, 0) };
            }
            else if (Type == MenuItemType.BottleHealth)
            {
                MenuItem.name = "Health Potion";
                MenuItem.description = "Fully restores all\navailable hearts.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(6, 6, 0, 0) };
            }
            else if (Type == MenuItemType.BottleFairy)
            {
                MenuItem.name = "Fairy in a Bottle";
                MenuItem.description = "Revives hero upon\ndeath.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(6, 9, 0, 0) };
            }

            #endregion


            #region Magic medallion menuItems

            else if (Type == MenuItemType.MagicFireball)
            {
                MenuItem.name = "Fireball Magic";
                MenuItem.description = "Shoots a fireball\nin the facing direction.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(7, 5, 0, 0) };
            }

            #endregion


            #region Weapon menuItems

            else if (Type == MenuItemType.WeaponSword)
            {
                MenuItem.name = "Magic Sword";
                MenuItem.description = "A magic sword.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(8, 5, 0, 0) };
            }
            else if (Type == MenuItemType.WeaponBow)
            {
                MenuItem.name = "Magic Bow";
                MenuItem.description = "A magic bow.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(8, 6, 0, 0) };
            }
            else if (Type == MenuItemType.WeaponStaff)
            {
                MenuItem.name = "Magic Staff";
                MenuItem.description = "A magic staff.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(8, 7, 0, 0) };
            }

            #endregion


            #region Armor menuItems

            else if (Type == MenuItemType.ArmorCloth)
            {
                MenuItem.name = "Basic Cloth";
                MenuItem.description = "A basic tshirt.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(9, 5, 0, 0) };
            }
            else if (Type == MenuItemType.ArmorChest)
            {
                MenuItem.name = "Plate Armor";
                MenuItem.description = "Heavy plate armor.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(9, 6, 0, 0) };
            }
            else if (Type == MenuItemType.ArmorCape)
            {
                MenuItem.name = "Magic Cape";
                MenuItem.description = "A magical cape.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(9, 7, 0, 0) };
            }
            else if (Type == MenuItemType.ArmorRobe)
            {
                MenuItem.name = "Magic Robe";
                MenuItem.description = "A magical robe.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(9, 8, 0, 0) };
            }

            #endregion


            #region Equipment menuItems

            else if (Type == MenuItemType.EquipmentRing)
            {
                MenuItem.name = "Magic Ring";
                MenuItem.description = "A magic ring.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(10, 5, 0, 0) };
            }
            else if (Type == MenuItemType.EquipmentPearl)
            {
                MenuItem.name = "Magic Pearl";
                MenuItem.description = "A magic pearl.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(10, 6, 0, 0) };
            }
            else if (Type == MenuItemType.EquipmentNecklace)
            {
                MenuItem.name = "Magic Necklace";
                MenuItem.description = "A magic necklace.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(10, 7, 0, 0) };
            }
            else if (Type == MenuItemType.EquipmentGlove)
            {
                MenuItem.name = "Magic Glove";
                MenuItem.description = "A magic glove.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(10, 8, 0, 0) };
            }
            else if (Type == MenuItemType.EquipmentPin)
            {
                MenuItem.name = "Magic Pin";
                MenuItem.description = "A magic pin.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(10, 9, 0, 0) };
            }

            #endregion


            #region Player inventory/loadout menuItems

            else if (Type == MenuItemType.InventoryGold)
            {
                MenuItem.name = "Total Gold";
                MenuItem.description = "The total amount\nof gold collected.";
                MenuItem.compAnim.currentAnimation = new List<Byte4>
                { new Byte4(10, 3, 0, 0), new Byte4(11, 3, 0, 0), new Byte4(12, 3, 0, 0), new Byte4(13, 3, 0, 0) };
            }
            else if (Type == MenuItemType.InventoryHeartPieces)
            {
                MenuItem.name = "Heart Pieces";
                MenuItem.description = "The number of heart\npieces you've collected.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(10, 1, 0, 0) };
            }
            else if (Type == MenuItemType.InventoryKey)
            {
                MenuItem.name = "Dungeon Key";
                MenuItem.description = "This key unlocks\nthe boss door.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(11, 2, 0, 0) };
            }
            else if (Type == MenuItemType.InventoryMap)
            {
                MenuItem.name = "Dungeon Map";
                MenuItem.description = "This map displays\nthe dungeon's rooms.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(10, 2, 0, 0) };
            }

            #endregion


            #region Option menuItems

            else if (Type == MenuItemType.OptionsSaveGame)
            {
                MenuItem.name = "Save Game";
                MenuItem.description = "Saves the current\ngame.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(15, 7, 0, 0) };
            }
            else if (Type == MenuItemType.OptionsLoadGame)
            {
                MenuItem.name = "Load Game";
                MenuItem.description = "Loads a saved\ngame.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(15, 7, 0, 0) };
            }
            else if (Type == MenuItemType.OptionsAudioCtrls)
            {
                MenuItem.name = "Audio Controls";
                MenuItem.description = "Changes the volume\nof soundfx and music.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(15, 10, 0, 0) };
            }
            else if (Type == MenuItemType.OptionsInputCtrls)
            {
                MenuItem.name = "Input Controls";
                MenuItem.description = "Changes the mapping\nof input buttons.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(15, 9, 0, 0) };
            }
            else if (Type == MenuItemType.OptionsVideoCtrls)
            {
                MenuItem.name = "Video Controls";
                MenuItem.description = "Changes the size\nof the game window.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(15, 8, 0, 0) };
            }

            #endregion


            #region Stat menuItems

            else if (Type == MenuItemType.StatsHealth)
            {
                MenuItem.name = "Health Stat";
                MenuItem.description = "How many hearts\nyou have.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(15, 5, 0, 0) };
            }
            else if (Type == MenuItemType.StatsMagic)
            {
                MenuItem.name = "Magic Stat";
                MenuItem.description = "How powerful\nyour magic is.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(15, 5, 0, 0) };
            }
            else if (Type == MenuItemType.StatsAgility)
            {
                MenuItem.name = "Agility Stat";
                MenuItem.description = "How quickly you\nmove and dash.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(15, 5, 0, 0) };
            }
            else if (Type == MenuItemType.Stats4)
            {
                MenuItem.name = "??? Stat";
                MenuItem.description = "TBD later...";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(15, 5, 0, 0) };
            }

            #endregion


            else
            {   //if the type was unhandled, default to unknown
                MenuItem.name = "Uknown";
                MenuItem.description = "No description available\nfor this item.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(15, 5, 0, 0) };
            }

            //update the sprite's current frame to the animation list set above
            AnimationFunctions.Animate(MenuItem.compAnim, MenuItem.compSprite);
        }

    }
}