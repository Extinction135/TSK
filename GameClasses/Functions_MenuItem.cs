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
        static int itemCounter;


        public static void PlaceMenuItems(List<MenuItem> MenuItems, int X, int Y, byte rowLength)
        {
            //for the 1st, place it at X, Y
            MenuItems[0].compSprite.position.X = X;
            MenuItems[0].compSprite.position.Y = Y;

            rowCounter = 0;
            itemCounter = 0;

            //for the rest, place with 24 px offset, dividing list into rows based on divider value
            for (i = 0; i < MenuItems.Count; i++) //i=1 skips 0, the first menuItem we set above
            {   //place menuItems 24px apart horizontally
                MenuItems[i].compSprite.position.X = MenuItems[0].compSprite.position.X + (24 * itemCounter);
                //each new vertical row gets the same 24 px offset
                MenuItems[i].compSprite.position.Y = MenuItems[0].compSprite.position.Y + (24 * rowCounter);
                itemCounter++;

                if (itemCounter == rowLength) { rowCounter++; itemCounter = 0; }
            }

            SetNeighbors(MenuItems, rowLength);
        }

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
                if (rowCounter == 0) //if this menuItem is the first on it's row
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

        public static void PlaceMenuItem(MenuItem Child, MenuItem Parent, int Offset)
        {
            Child.compSprite.position.X = Parent.compSprite.position.X + Offset;
            Child.compSprite.position.Y = Parent.compSprite.position.Y;
        }

        public static void PlaceRow(List<MenuItem> MenuItems, int index, int X, int Y, int rowLength)
        {   //place the menuItem index at X, Y
            MenuItems[index].compSprite.position.X = X;
            MenuItems[index].compSprite.position.Y = Y;
            //place the rest of the row relative to index
            for(i = 0; i < rowLength; i++)
            {
                if ((index + i + 1) < MenuItems.Count)
                { PlaceMenuItem(MenuItems[index + i + 1], MenuItems[index + i], 24); }
            }
        }



        public static void SetType(MenuItemType Type, MenuItem MenuItem)
        {
            //MOST menuItem objects exist on the UiItems Sheet
            MenuItem.compSprite.texture = Assets.uiItemsSheet;
            //some exceptions are: map, key, pet_dog

            //set the MenuItem data based on the passed Type
            MenuItem.type = Type;
            MenuItem.price = 0; //defaults to 0


            #region Item menuItems

            if (Type == MenuItemType.ItemHeart)
            {
                MenuItem.name = "Heart Piece";
                MenuItem.description = "Increases maximum \nhealth by 1.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_ItemHeart;
                MenuItem.price = 75;
            }
            else if (Type == MenuItemType.ItemBomb)
            {
                MenuItem.name = "Explosive Bomb";
                MenuItem.description = "Explodes, dealing 2 \nphysical damage.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_ItemBomb;
                MenuItem.price = 10;
            }
            else if (Type == MenuItemType.ItemBomb3Pack)
            {
                MenuItem.name = "Explosive Bombs";
                MenuItem.description = "A set of 3 explosive \nbombs, for 30 gold.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_ItemBomb3Pack;
                MenuItem.price = 30;
            }
            else if (Type == MenuItemType.ItemArrowPack)
            {
                MenuItem.name = "Arrow Set";
                MenuItem.description = "A set of 20 arrows, \nfor 20 gold.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_ItemArrowPack;
                MenuItem.price = 20;
            }
            else if (Type == MenuItemType.ItemBoomerang)
            {
                MenuItem.name = "Magic Boomerang";
                MenuItem.description = "A magical boomerang.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_ItemBoomerang;
                MenuItem.price = 50;
            }

            #endregion


            #region Bottle menuItems

            else if (Type == MenuItemType.BottleEmpty)
            {
                MenuItem.name = "Empty Bottle";
                MenuItem.description = "An empty bottle.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Bottle_Empty;
                MenuItem.price = 5;
            }
            else if (Type == MenuItemType.BottleHealth)
            {
                MenuItem.name = "Health Potion";
                MenuItem.description = "Fully restores hearts, \neven from death.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Bottle_Health;
                MenuItem.price = 20;
            }
            else if (Type == MenuItemType.BottleMagic)
            {
                MenuItem.name = "Magic Potion";
                MenuItem.description = "Fully restores all\navailable magic.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Bottle_Magic;
                MenuItem.price = 20;
            }
            else if (Type == MenuItemType.BottleCombo)
            {
                MenuItem.name = "Combo Potion";
                MenuItem.description = "Fully restores hearts \nand magic.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Bottle_Combo;
                MenuItem.price = 50;
            }
            else if (Type == MenuItemType.BottleFairy)
            {
                MenuItem.name = "Fairy in a Bottle";
                MenuItem.description = "Fully restores hearts, \neven from death.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Bottle_Fairy;
                MenuItem.price = 50;
            }
            else if (Type == MenuItemType.BottleBlob)
            {
                MenuItem.name = "Blob in a Bottle";
                MenuItem.description = "Transforms hero in a \nloveable blob.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Bottle_Blob;
                MenuItem.price = 50;
            }

            #endregion


            #region Magic medallion menuItems

            else if (Type == MenuItemType.MagicFireball)
            {
                MenuItem.name = "Fireball Magic";
                MenuItem.description = "deals 2 magical damage, \ncosts 1 magic.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Magic_Fireball;
                MenuItem.price = 99;
            }

            #endregion


            #region Weapon menuItems

            else if (Type == MenuItemType.WeaponSword)
            {
                MenuItem.name = "Hero's Sword";
                MenuItem.description = "sharp and strong, \ndeals 1 physical damage.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Weapon_Sword;
                MenuItem.price = 50;
            }
            else if (Type == MenuItemType.WeaponBow)
            {
                MenuItem.name = "Sturdy Bow";
                MenuItem.description = "shoots arrows, which \ndeal 1 physical damage.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Weapon_Bow;
                MenuItem.price = 50;
            }
            else if (Type == MenuItemType.WeaponNet)
            {
                MenuItem.name = "Old Net";
                MenuItem.description = "can capture small \ncreatures in a bottle.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Weapon_Net;
                MenuItem.price = 50;
            }

            #endregion


            #region Armor menuItems

            else if (Type == MenuItemType.ArmorCloth)
            {
                MenuItem.name = "Old Tunic";
                MenuItem.description = "A familiar tunic \nworn from years of wear.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Armor_Cloth;
                MenuItem.price = 0;
            }
            else if (Type == MenuItemType.ArmorCape)
            {
                MenuItem.name = "Tattered Shawl";
                MenuItem.description = "Dirty and Pitiful.\nIncreases speed.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Armor_Cape;
                MenuItem.price = 80;
            }

            #endregion


            #region Equipment menuItems

            else if (Type == MenuItemType.EquipmentRing)
            {
                MenuItem.name = "Lucky Ring";
                MenuItem.description = "Increases the drop rate \nof loot from enemies.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Eq_Ring;
                MenuItem.price = 50;
            }
            else if (Type == MenuItemType.EquipmentPearl)
            {
                MenuItem.name = "Magic Pearl";
                MenuItem.description = "A magic pearl.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Eq_Pearl;
                MenuItem.price = 50;
            }
            else if (Type == MenuItemType.EquipmentNecklace)
            {
                MenuItem.name = "Magic Necklace";
                MenuItem.description = "A magic necklace.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Eq_Necklace;
                MenuItem.price = 50;
            }
            else if (Type == MenuItemType.EquipmentGlove)
            {
                MenuItem.name = "Magic Glove";
                MenuItem.description = "A magic glove.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Eq_Glove;
                MenuItem.price = 50;
            }
            else if (Type == MenuItemType.EquipmentPin)
            {
                MenuItem.name = "Magic Pin";
                MenuItem.description = "A magic pin.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Eq_Pin;
                MenuItem.price = 50;
            }

            #endregion




            #region Pets

            else if (Type == MenuItemType.PetStinkyDog)
            {
                MenuItem.name = "Stinky Dog";
                MenuItem.description = "A stinky, but loveable \ndog companion.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Pet_Dog_Idle;
                MenuItem.compSprite.texture = Assets.petsSheet;
                MenuItem.price = 0;
            }

            #endregion



            //vendor menuItems?



            #region Player inventory/loadout menuItems

            else if (Type == MenuItemType.InventoryGold)
            {
                MenuItem.name = "Total Gold";
                MenuItem.description = "The total amount\nof gold collected.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_InventoryGold;
                MenuItem.compSprite.cellSize.X = 8;
            }
            else if (Type == MenuItemType.InventoryMap)
            {
                MenuItem.name = "Dungeon Map";
                MenuItem.description = "This map displays\nthe dungeon's rooms.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Dungeon_Map;
                MenuItem.compSprite.texture = Assets.forestLevelSheet;
            }
            else if (Type == MenuItemType.InventoryKey)
            {
                MenuItem.name = "Dungeon Key";
                MenuItem.description = "This key unlocks\nthe boss door.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Dungeon_BossKey;
                MenuItem.compSprite.texture = Assets.forestLevelSheet;
            }
            

            #endregion


            #region Option menuItems

            else if (Type == MenuItemType.OptionsContinue)
            {
                MenuItem.name = "Continue Game";
                MenuItem.description = "Continues the\nlast game.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_ArrowUp;
                MenuItem.compSprite.rotation = Rotation.Clockwise90;
            }



            else if (Type == MenuItemType.OptionsNewGame)
            {
                MenuItem.name = "New Game";
                MenuItem.description = "Starts a new\ngame.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_ArrowRight;
            }
            else if (Type == MenuItemType.OptionsLoadGame)
            {
                MenuItem.name = "Load Game";
                MenuItem.description = "Loads a saved game.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_ArrowUp;
            }
            else if (Type == MenuItemType.OptionsQuitGame)
            {
                MenuItem.name = "Quit Game";
                MenuItem.description = "Quit the current game.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Cross;
            }
            else if (Type == MenuItemType.OptionsSaveGame)
            {
                MenuItem.name = "Save Game";
                MenuItem.description = "Saves the \ncurrent game.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_ArrowToDisk;
            }



            else if (Type == MenuItemType.OptionsAudioCtrls)
            {
                MenuItem.name = "Audio Controls";
                MenuItem.description = "Changes the volume \nof soundfx and music.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_SpeakerVolume;
            }
            else if (Type == MenuItemType.OptionsInputCtrls)
            {
                MenuItem.name = "Input Controls";
                MenuItem.description = "Changes the mapping \nof input buttons.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Controller;
            }
            else if (Type == MenuItemType.OptionsVideoCtrls)
            {
                MenuItem.name = "Video Controls";
                MenuItem.description = "Changes the size \nof the game window.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Monitor;
            }
            else if (Type == MenuItemType.OptionsGameCtrls)
            {
                MenuItem.name = "Game Controls";
                MenuItem.description = "Changes game \ncontrols + settings.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Joystick;
            }


            else if (Type == MenuItemType.OptionsCheatMenu)
            {
                MenuItem.name = "Cheat Menu";
                MenuItem.description = "Flip cheats \non or off.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_QuestionMark;
            }









            #endregion


            #region Cheat menuItems

            else if (Type == MenuItemType.CheatsInfiniteArrows)
            {
                MenuItem.name = "Infinite Arrows";
                MenuItem.description = "An endless supply of \nsharp pointy things.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_CheatOn;
            }
            else if (Type == MenuItemType.CheatsInfiniteBombs)
            {
                MenuItem.name = "Infinite Bombs";
                MenuItem.description = "Just don't get caught \nin the explosions.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_CheatOn;
            }
            else if (Type == MenuItemType.CheatsInfiniteGold)
            {
                MenuItem.name = "Infinite Gold";
                MenuItem.description = "You literally can't \nspend it all.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_CheatOn;
            }
            else if (Type == MenuItemType.CheatsInfiniteHP)
            {
                MenuItem.name = "Infinite Hearts";
                MenuItem.description = "You won't take any \ndamage, ever.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_CheatOn;
            }
            else if (Type == MenuItemType.CheatsInfiniteMagic)
            {
                MenuItem.name = "Infinite Magic";
                MenuItem.description = "Redefining OP.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_CheatOn;
            }



            else if (Type == MenuItemType.CheatsKey)
            {
                MenuItem.name = "Has Key";
                MenuItem.description = "Link permanently has \nthe dungeon key.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_CheatOn;
            }
            else if (Type == MenuItemType.CheatsMap)
            {
                MenuItem.name = "Has Map";
                MenuItem.description = "Link permanently has \nthe dungeon map.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_CheatOn;
            }



            else if (Type == MenuItemType.CheatsUnlockAll)
            {
                MenuItem.name = "Unlock All";
                MenuItem.description = "Unlocks all items,\nweapons, etc..";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_CheatOn;
            }

            #endregion



            else
            {   //if the type was unhandled, default to unknown
                MenuItem.name = "Unknown";
                MenuItem.description = "No description available\nfor this item.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Unknown;
            }

            //check above may of changed cellsize, just update it once
            Functions_Component.UpdateCellSize(MenuItem.compSprite);
            //update the sprite's current frame to the animation list set above
            Functions_Animation.Animate(MenuItem.compAnim, MenuItem.compSprite);
        }

    }
}