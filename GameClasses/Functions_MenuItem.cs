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
    public static class Functions_MenuItem
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
            MenuItem.price = 0; //defaults to 0


            #region Item menuItems

            if (Type == MenuItemType.ItemBoomerang)
            {
                MenuItem.name = "Magic Boomerang";
                MenuItem.description = "A magical boomerang.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(5, 5, 0, 0) };
                MenuItem.price = 50;
            }
            else if (Type == MenuItemType.ItemBomb)
            {
                MenuItem.name = "Explosive Bomb";
                MenuItem.description = "Explodes, dealing 2 \nphysical damage.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(5, 6, 0, 0) };
                MenuItem.price = 10;
            }
            else if (Type == MenuItemType.ItemBomb3Pack)
            {
                MenuItem.name = "Explosive Bombs";
                MenuItem.description = "A set of 3 explosive \nbombs, for 30 gold.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(5, 7, 0, 0) };
                MenuItem.price = 30;
            }
            else if (Type == MenuItemType.ItemArrowPack)
            {
                MenuItem.name = "Arrow Set";
                MenuItem.description = "A set of 20 arrows, \nfor 20 gold.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(5, 8, 0, 0) };
                MenuItem.price = 20;
            }

            #endregion


            #region Bottle menuItems

            else if (Type == MenuItemType.BottleEmpty)
            {
                MenuItem.name = "Empty Bottle";
                MenuItem.description = "An empty bottle.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(6, 5, 0, 0) };
                MenuItem.price = 5;
            }
            else if (Type == MenuItemType.BottleHealth)
            {
                MenuItem.name = "Health Potion";
                MenuItem.description = "Fully restores all\navailable hearts.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(6, 6, 0, 0) };
                MenuItem.price = 20;
            }
            else if (Type == MenuItemType.BottleMagic)
            {
                MenuItem.name = "Magic Potion";
                MenuItem.description = "Fully restores all\navailable magic.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(6, 7, 0, 0) };
                MenuItem.price = 20;
            }
            else if (Type == MenuItemType.BottleFairy)
            {
                MenuItem.name = "Fairy in a Bottle";
                MenuItem.description = "Restores health and \nmagic, even from death.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(6, 9, 0, 0) };
                MenuItem.price = 50;
            }

            #endregion


            #region Magic medallion menuItems

            else if (Type == MenuItemType.MagicFireball)
            {
                MenuItem.name = "Fireball Magic";
                MenuItem.description = "deals 2 magical damage, \ncosts 1 magic.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(7, 6, 0, 0) };
                MenuItem.price = 99;
            }

            #endregion


            #region Weapon menuItems

            else if (Type == MenuItemType.WeaponSword)
            {
                MenuItem.name = "Hero's Sword";
                MenuItem.description = "sharp and strong, \ndeals 1 physical damage.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(8, 5, 0, 0) };
                MenuItem.price = 50;
            }
            else if (Type == MenuItemType.WeaponBow)
            {
                MenuItem.name = "Sturdy Bow";
                MenuItem.description = "shoots arrows, which \ndeal 1 physical damage.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(8, 6, 0, 0) };
                MenuItem.price = 50;
            }
            else if (Type == MenuItemType.WeaponStaff)
            {
                MenuItem.name = "Magic Staff";
                MenuItem.description = "A magic staff.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(8, 7, 0, 0) };
                MenuItem.price = 50;
            }

            #endregion


            #region Armor menuItems

            else if (Type == MenuItemType.ArmorCloth)
            {
                MenuItem.name = "Hero's Tunic";
                MenuItem.description = "Provides protection \nagainst magical damage.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(9, 5, 0, 0) };
                MenuItem.price = 0;
            }
            else if (Type == MenuItemType.ArmorChest)
            {
                MenuItem.name = "Knight's Armor";
                MenuItem.description = "Prevents some damage, \nbut slows movement.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(9, 6, 0, 0) };
                MenuItem.price = 80;
            }
            else if (Type == MenuItemType.ArmorCape)
            {
                MenuItem.name = "Rogue's Cape";
                MenuItem.description = "Increases movement \nand dash speed.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(9, 7, 0, 0) };
                MenuItem.price = 80;
            }
            else if (Type == MenuItemType.ArmorRobe)
            {
                MenuItem.name = "Mage's Robe";
                MenuItem.description = "Increases maximum \namount of magic by 4.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(9, 8, 0, 0) };
                MenuItem.price = 60;
            }

            #endregion


            #region Equipment menuItems

            else if (Type == MenuItemType.EquipmentRing)
            {
                MenuItem.name = "Lucky Ring";
                MenuItem.description = "Increases the drop rate \nof loot from enemies.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(10, 5, 0, 0) };
                MenuItem.price = 50;
            }
            else if (Type == MenuItemType.EquipmentPearl)
            {
                MenuItem.name = "Magic Pearl";
                MenuItem.description = "A magic pearl.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(10, 6, 0, 0) };
                MenuItem.price = 50;
            }
            else if (Type == MenuItemType.EquipmentNecklace)
            {
                MenuItem.name = "Magic Necklace";
                MenuItem.description = "A magic necklace.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(10, 7, 0, 0) };
                MenuItem.price = 50;
            }
            else if (Type == MenuItemType.EquipmentGlove)
            {
                MenuItem.name = "Magic Glove";
                MenuItem.description = "A magic glove.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(10, 8, 0, 0) };
                MenuItem.price = 50;
            }
            else if (Type == MenuItemType.EquipmentPin)
            {
                MenuItem.name = "Magic Pin";
                MenuItem.description = "A magic pin.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(10, 9, 0, 0) };
                MenuItem.price = 50;
            }

            #endregion


            #region Player inventory/loadout menuItems

            else if (Type == MenuItemType.InventoryGold)
            {
                MenuItem.name = "Total Gold";
                MenuItem.description = "The total amount\nof gold collected.";
                MenuItem.compAnim.currentAnimation = new List<Byte4>
                { new Byte4(11, 7, 0, 0), new Byte4(12, 7, 0, 0), new Byte4(13, 7, 0, 0), new Byte4(14, 7, 0, 0) };
            }
            else if (Type == MenuItemType.InventoryHeartPieces)
            {
                MenuItem.name = "Heart Pieces";
                MenuItem.description = "The number of heart\npieces you've collected.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(11, 6, 0, 0) };
            }
            else if (Type == MenuItemType.InventoryKey)
            {
                MenuItem.name = "Dungeon Key";
                MenuItem.description = "This key unlocks\nthe boss door.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(12, 5, 0, 0) };
            }
            else if (Type == MenuItemType.InventoryMap)
            {
                MenuItem.name = "Dungeon Map";
                MenuItem.description = "This map displays\nthe dungeon's rooms.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(11, 5, 0, 0) };
            }

            #endregion


            #region Option menuItems

            else if (Type == MenuItemType.OptionsContinue)
            {
                MenuItem.name = "Continue Game";
                MenuItem.description = "Continues the\nlast game.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(15, 8, 0, 0) };
            }
            else if (Type == MenuItemType.OptionsNewGame)
            {
                MenuItem.name = "New Game";
                MenuItem.description = "Starts a new\ngame.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(15, 12, 0, 0) };
            }
            else if (Type == MenuItemType.OptionsLoadGame)
            {
                MenuItem.name = "Load Game";
                MenuItem.description = "Loads a saved\ngame.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(15, 8, 0, 0) };
            }
            else if (Type == MenuItemType.OptionsQuitGame)
            {
                MenuItem.name = "Quit Game";
                MenuItem.description = "Quit the current\ngame.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(14, 12, 0, 0) };
            }
            //
            else if (Type == MenuItemType.OptionsAudioCtrls)
            {
                MenuItem.name = "Audio Controls";
                MenuItem.description = "Changes the volume\nof soundfx and music.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(15, 11, 0, 0) };
            }
            else if (Type == MenuItemType.OptionsInputCtrls)
            {
                MenuItem.name = "Input Controls";
                MenuItem.description = "Changes the mapping\nof input buttons.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(15, 10, 0, 0) };
            }
            else if (Type == MenuItemType.OptionsVideoCtrls)
            {
                MenuItem.name = "Video Controls";
                MenuItem.description = "Changes the size\nof the game window.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(15, 9, 0, 0) };
            }
            else if (Type == MenuItemType.OptionsGameCtrls)
            {
                MenuItem.name = "Game Controls";
                MenuItem.description = "Changes game\ncontrols + settings.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(14, 11, 0, 0) };
            }
            //
            else if (Type == MenuItemType.OptionsSaveGame)
            {
                MenuItem.name = "Save Game";
                MenuItem.description = "Saves the current\ngame.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(15, 8, 0, 0) };
            }

            #endregion


            #region Stat menuItems

            /*
            else if (Type == MenuItemType.StatsHealth)
            {
                MenuItem.name = "Health Stat";
                MenuItem.description = "How many hearts\nyou have.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(15, 6, 0, 0) };
            }
            else if (Type == MenuItemType.StatsMagic)
            {
                MenuItem.name = "Magic Stat";
                MenuItem.description = "How powerful\nyour magic is.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(15, 6, 0, 0) };
            }
            else if (Type == MenuItemType.StatsAgility)
            {
                MenuItem.name = "Agility Stat";
                MenuItem.description = "How quickly you\nmove and dash.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(15, 6, 0, 0) };
            }
            else if (Type == MenuItemType.Stats4)
            {
                MenuItem.name = "Luck Stat";
                MenuItem.description = "TBD later...";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(15, 6, 0, 0) };
            }
            */

            #endregion


            else
            {   //if the type was unhandled, default to unknown
                MenuItem.name = "Unknown";
                MenuItem.description = "No description available\nfor this item.";
                MenuItem.compAnim.currentAnimation = new List<Byte4> { new Byte4(15, 6, 0, 0) };
            }

            //update the sprite's current frame to the animation list set above
            Functions_Animation.Animate(MenuItem.compAnim, MenuItem.compSprite);
        }

        public static void PlaceMenuItem(MenuItem Child, MenuItem Parent, int Offset)
        {
            Child.compSprite.position.X = Parent.compSprite.position.X + Offset;
            Child.compSprite.position.Y = Parent.compSprite.position.Y;
        }

    }
}