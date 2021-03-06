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
    public static class Functions_MenuItem
    {
        static int i;
        static int rowCounter;
        static int itemCounter;


        public static void PlaceMenuItems(List<MenuItem> MenuItems, int X, int Y, 
            byte rowLength, int xOffset = 24, int yOffset = 24)
        {
            //for the 1st, place it at X, Y
            MenuItems[0].compSprite.position.X = X;
            MenuItems[0].compSprite.position.Y = Y;

            rowCounter = 0;
            itemCounter = 0;

            //for the rest, place with 24 px offset, dividing list into rows based on divider value
            for (i = 0; i < MenuItems.Count; i++) //i=1 skips 0, the first menuItem we set above
            {   //place menuItems 24px apart horizontally
                MenuItems[i].compSprite.position.X = MenuItems[0].compSprite.position.X + (xOffset * itemCounter);
                //each new vertical row gets the same 24 px offset
                MenuItems[i].compSprite.position.Y = MenuItems[0].compSprite.position.Y + (yOffset * rowCounter);
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
            else if (Type == MenuItemType.ItemBow)
            {
                MenuItem.name = "Sturdy Bow";
                MenuItem.description = "shoots arrows, which \ndeal 1 physical damage.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Item_Bow;
                MenuItem.price = 50;
            }
            else if (Type == MenuItemType.ItemFirerod)
            {
                MenuItem.name = "Firerod";
                MenuItem.description = "shoots a fireball. \ncosts 1 magic.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Item_Firerod;
                MenuItem.price = 99;
            }
            else if (Type == MenuItemType.ItemIcerod)
            {
                MenuItem.name = "Icerod";
                MenuItem.description = "shoots an iceball. \ncosts 1 magic.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Item_Icerod;
                MenuItem.price = 99;
            }
            else if (Type == MenuItemType.ItemMagicMirror)
            {
                MenuItem.name = "Magic Mirror";
                MenuItem.description = "returns hero to \ndungeon exit or map.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Item_MagicMirror;
                MenuItem.price = 99;
            }

            else if (Type == MenuItemType.ItemSpellbook)
            {
                MenuItem.name = "Spell Book";
                MenuItem.description = "contains all spells \nlearned so far.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Item_SpellBook;
                MenuItem.price = 99;
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

            else if (Type == MenuItemType.MagicBombos)
            {
                MenuItem.name = "Bombos Magic";
                MenuItem.description = "creates many bombs.\ncosts 5 magic.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Magic_Bombos;
                MenuItem.price = 99;
            }
            else if (Type == MenuItemType.MagicEther)
            {
                MenuItem.name = "Ether Magic";
                MenuItem.description = "lightning bolts\nfrom the sky, 5 magic.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Magic_Ether;
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
            else if (Type == MenuItemType.WeaponNet)
            {
                MenuItem.name = "Old Net";
                MenuItem.description = "can capture small \ncreatures in a bottle.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Weapon_Net;
                MenuItem.price = 50;
            }
            else if (Type == MenuItemType.WeaponShovel)
            {
                MenuItem.name = "Shovel";
                MenuItem.description = "useful for digging \nand breaking armor.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Weapon_Shovel;
                MenuItem.price = 50;
            }
            else if (Type == MenuItemType.WeaponHammer)
            {
                MenuItem.name = "Hammer";
                MenuItem.description = "sturdy and \npowerful.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Weapon_Hammer;
                MenuItem.price = 50;
            }
            else if (Type == MenuItemType.WeaponWand)
            {
                MenuItem.name = "Wand";
                MenuItem.description = "thin and brittle.\nCasts spells.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Weapon_Wand;
                MenuItem.price = 99;
            }
            else if (Type == MenuItemType.WeaponFang)
            {
                MenuItem.name = "Creature's Fang";
                MenuItem.description = "used by some enemies\nto bite for 1 damage.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Weapon_Fang;
                MenuItem.price = 0;
            }


            #endregion


            #region Armor menuItems

            else if (Type == MenuItemType.ArmorCloth)
            {
                MenuItem.name = "Old Tunic";
                MenuItem.description = "A familiar old tunic.";
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


            #region Pet adoption menuItems

            else if (Type == MenuItemType.PetDog_Gray)
            {
                MenuItem.name = "Stinky Dog";
                MenuItem.description = "A stinky, but loveable \ndog companion.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Pet_Dog_Idle;
                MenuItem.compSprite.texture = Assets.petsSheet;
                MenuItem.price = 0;
            }

            #endregion


            #region Colliseum Challenge menuItems

            //standards
            else if (Type == MenuItemType.Challenge_Blobs)
            {
                MenuItem.name = "Mobs - Blobs";
                MenuItem.description = "Defeat 25 enemies \nfor 25 gold.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Challenge;
                MenuItem.compSprite.texture = Assets.CommonObjsSheet;
                MenuItem.price = 10;
            }

            //minibosses
            else if (Type == MenuItemType.Challenge_Mini_BlackEyes)
            {
                MenuItem.name = "Miniboss - Blackeyes";
                MenuItem.description = "Defeat 2 blackeyes \nfor 99 gold.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Challenge;
                MenuItem.compSprite.texture = Assets.CommonObjsSheet;
                MenuItem.price = 10;
            }
            else if (Type == MenuItemType.Challenge_Mini_Spiders)
            {
                MenuItem.name = "Miniboss - Spiders";
                MenuItem.description = "Defeat 2 spiders \nfor 99 gold.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Challenge;
                MenuItem.compSprite.texture = Assets.CommonObjsSheet;
                MenuItem.price = 10;
            }

            //bosses
            else if (Type == MenuItemType.Challenge_Bosses_BigEye)
            {
                MenuItem.name = "Boss - BigEye";
                MenuItem.description = "Defeat BigEye \nfor 99 gold.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Challenge;
                MenuItem.compSprite.texture = Assets.CommonObjsSheet;
                MenuItem.price = 10;
            }
            else if (Type == MenuItemType.Challenge_Bosses_BigBat)
            {
                MenuItem.name = "Boss - BigBat";
                MenuItem.description = "Defeat BigBat \nfor 99 gold.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Challenge;
                MenuItem.compSprite.texture = Assets.CommonObjsSheet;
                MenuItem.price = 10;
            }

            #endregion



            




            #region Player inventory/loadout menuItems

            else if (Type == MenuItemType.InventoryGold)
            {
                MenuItem.name = "Total Gold";
                MenuItem.description = "The total amount\nof gold collected.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_InventoryGold;
                MenuItem.compSprite.drawRec.Width = 8;
            }
            else if (Type == MenuItemType.InventoryMap)
            {
                MenuItem.name = "Dungeon Map";
                MenuItem.description = "This map displays\nthe dungeon's rooms.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Dungeon_Map;
                MenuItem.compSprite.texture = Assets.CommonObjsSheet;
            }
            else if (Type == MenuItemType.InventoryKey)
            {
                MenuItem.name = "Dungeon Key";
                MenuItem.description = "This key unlocks\nthe boss door.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Dungeon_BossKey;
                MenuItem.compSprite.texture = Assets.CommonObjsSheet;
            }
            

            #endregion






            #region Option Widget menuItems

            else if (Type == MenuItemType.OptionsNewGame)
            {
                MenuItem.name = "New Game";
                MenuItem.description = "Starts a new\ngame.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_ArrowRight;
            }
            else if (Type == MenuItemType.OptionsSandBox)
            {
                MenuItem.name = "Sandbox";
                MenuItem.description = "New game with\ncheats enabled.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_ArrowRight;
            }
            else if (Type == MenuItemType.OptionsQuitGame)
            {
                MenuItem.name = "Quit Game";
                MenuItem.description = "Quits the \ncurrent game.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Cross;
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
            else if (Type == MenuItemType.OptionsOptionsMenu)
            {
                MenuItem.name = "Options Menu";
                MenuItem.description = "setup game\noptions.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Joystick;
            }
            else if (Type == MenuItemType.OptionsCheatMenu)
            {
                MenuItem.name = "Cheat Menu";
                MenuItem.description = "Flip cheats \non or off.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_QuestionMark;
            }

            #endregion


            #region Cheat Screen menuItems

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
                MenuItem.description = "It's ok, you can't \nbe good at everything.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_CheatOn;
            }
            else if (Type == MenuItemType.CheatsInfiniteMagic)
            {
                MenuItem.name = "Infinite Magic";
                MenuItem.description = "spam y to win. \nyou're welcome.";
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
            else if (Type == MenuItemType.CheatsClipping)
            {
                MenuItem.name = "Clipping";
                MenuItem.description = "Toggles interactions\nand collisions on/off.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_CheatOn;
            }
            else if (Type == MenuItemType.CheatsInfiniteFairies)
            {
                MenuItem.name = "Infinite Fairies";
                MenuItem.description = "A bottled fairy will\n always revive you.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_CheatOn;
            }



            else if (Type == MenuItemType.CheatsAutoSolvePuzzles)
            {
                MenuItem.name = "Auto-solve Room Puzzles";
                MenuItem.description = "Handles switches,\n aand torches.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_CheatOn;
            }

            #endregion


            #region Option Screen menuItems

            else if (Type == MenuItemType.Options_DrawInput)
            {
                MenuItem.name = "Draw Input Display";
                MenuItem.description = "draws controller \ninput on screen.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_CheatOff;
            }
            else if (Type == MenuItemType.Options_TrackCamera)
            {
                MenuItem.name = "Track Camera to Hero";
                MenuItem.description = "centers camera to \ndungeon room or hero.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_CheatOff;
            }
            else if (Type == MenuItemType.Options_Watermark)
            {
                MenuItem.name = "Draw Watermark";
                MenuItem.description = "draws game's repo \nlink in top right.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_CheatOff;
            }
            else if (Type == MenuItemType.Options_HardMode)
            {
                MenuItem.name = "Hard Mode";
                MenuItem.description = "rooms reset, etc.. \nalmost unfair.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_CheatOff;
            }
            else if (Type == MenuItemType.Options_DrawBuildTimes)
            {
                MenuItem.name = "Draw Build Times";
                MenuItem.description = "displays ms time for \nframe, room, and level.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_CheatOff;
            }
            else if (Type == MenuItemType.Options_PlayMusic)
            {
                MenuItem.name = "Play or Mute Music";
                MenuItem.description = "play or mute the game \nmusic.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_CheatOff;
            }
            else if (Type == MenuItemType.Options_DrawDebug)
            {
                MenuItem.name = "Draw Debug Info";
                MenuItem.description = "displays debug info. \nmay slow game down.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_CheatOff;
            }
            else if (Type == MenuItemType.Options_DrawHitBoxes)
            {
                MenuItem.name = "Draw HitBoxes";
                MenuItem.description = "red = blocking. \nblue = room.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_CheatOff;
            }
            else if (Type == MenuItemType.Options_Gore)
            {
                MenuItem.name = "Gore Settings";
                MenuItem.description = "Enables blood, \nguts, skeletons.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_CheatOff;
            }

            #endregion



            //SpellBook Screen menuItems


            #region Wind Spells

            else if (Type == MenuItemType.Wind_Gust)
            {
                MenuItem.name = "Gust";
                MenuItem.description = "Casts gust of wind \nin facing direction.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Wind_Gust;
                MenuItem.compSprite.texture = Assets.entitiesSheet;
                MenuItem.compAnim.loop = true;
                MenuItem.compAnim.speed = 6; //speed up for power
            }
            else if (Type == MenuItemType.Wind_Calm)
            {
                MenuItem.name = "Calm";
                MenuItem.description = "Calms nearby winds, \nmakes climbing fun.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Wind_Gust;
                MenuItem.compSprite.texture = Assets.entitiesSheet;
                MenuItem.compAnim.loop = true;
                MenuItem.compAnim.speed = 12; //slow down for calming fx
            }
            else if (Type == MenuItemType.Wind_Fury)
            {
                MenuItem.name = "Fury";
                MenuItem.description = "Enrages nearby winds.\n";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Wind_Gust;
                MenuItem.compSprite.texture = Assets.entitiesSheet;
                MenuItem.compAnim.loop = true;
                MenuItem.compAnim.speed = 3; //speed up for power
            }
            else if (Type == MenuItemType.Wind_Dir)
            {
                MenuItem.name = "Wind Direction";
                MenuItem.description = "Changes wind direction\nbased on caster.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Wind_Gust;
                MenuItem.compSprite.texture = Assets.entitiesSheet;
                MenuItem.compAnim.loop = true;
                MenuItem.compAnim.speed = 10; //speed up for power
            }
            else if (Type == MenuItemType.Wind_Stop)
            {
                MenuItem.name = "Stop Wind";
                MenuItem.description = "Ceases nearby winds.\n";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Wind_Gust;
                MenuItem.compSprite.texture = Assets.entitiesSheet;
                MenuItem.compAnim.loop = true;
                MenuItem.compAnim.speed = 24; //very slow
            }

            #endregion


            #region Explosive/Fire spells

            else if (Type == MenuItemType.Fire)
            {
                MenuItem.name = "Fire";
                MenuItem.description = "Casts a ground fire \nfacing direction.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Projectile_FireGround;
                MenuItem.compSprite.texture = Assets.entitiesSheet;
                MenuItem.compAnim.loop = true;
                MenuItem.compAnim.speed = 8;
            }
            else if (Type == MenuItemType.Fire_Walk)
            {
                MenuItem.name = "Fire Walk";
                MenuItem.description = "Creates fire where \ncaster walks.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Projectile_FireGround;
                MenuItem.compSprite.texture = Assets.entitiesSheet;
                MenuItem.compAnim.loop = true;
                MenuItem.compAnim.speed = 8;
            }
            else if (Type == MenuItemType.Explosive_Single)
            {
                MenuItem.name = "Explode";
                MenuItem.description = "Casts an explosion \nin front of the caster.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Projectile_Explosion;
                MenuItem.compSprite.texture = Assets.entitiesSheet;
                MenuItem.compAnim.loop = true;
                MenuItem.compAnim.speed = 10;
            }
            else if (Type == MenuItemType.Explosive_Line)
            {
                MenuItem.name = "Chain Explosions";
                MenuItem.description = "Casts a series of \nexplosions..be careful.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Projectile_Explosion;
                MenuItem.compSprite.texture = Assets.entitiesSheet;
                MenuItem.compAnim.loop = true;
                MenuItem.compAnim.speed = 10;
            }

            #endregion


            #region Ice Spells

            else if (Type == MenuItemType.Spells_Ice_FreezeWalk)
            {
                MenuItem.name = "Ice Walk";
                MenuItem.description = "Ice is created where \ncaster walks.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Projectile_Iceball;
                MenuItem.compSprite.texture = Assets.entitiesSheet;
                MenuItem.compAnim.loop = true;
                MenuItem.compAnim.speed = 10;
            }
            else if (Type == MenuItemType.Spells_Ice_FreezeGround)
            {
                MenuItem.name = "Freeze Ground";
                MenuItem.description = "Freezes ground around \ncaster.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Projectile_Iceball;
                MenuItem.compSprite.texture = Assets.entitiesSheet;
                MenuItem.compAnim.loop = true;
                MenuItem.compAnim.speed = 10;
            }

            #endregion


            #region Electrical spells

            else if (Type == MenuItemType.Spells_Lightning_Ether)
            {
                MenuItem.name = "Ether";
                MenuItem.description = "Bolts strike all \nenemies.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Particle_LightningBolt;
                MenuItem.compSprite.texture = Assets.entitiesSheet;
                MenuItem.compAnim.loop = true;
                MenuItem.compAnim.speed = 3;
            }

            #endregion


            #region Summon Spells

            else if (Type == MenuItemType.Spells_Summon_Bat_Explosion)
            {
                MenuItem.name = "Summon Bats";
                MenuItem.description = "Summons an army of \nbats around caster.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Projectile_Bat;
                MenuItem.compSprite.texture = Assets.EnemySheet;
                MenuItem.compAnim.loop = true;
                MenuItem.compAnim.speed = 10;
            }

            #endregion





            else
            {   //if the type was unhandled, default to unknown
                MenuItem.name = "Unknown";
                MenuItem.description = "No description available\nfor this item.";
                MenuItem.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Unknown;
            }


            //update the sprite's current frame to the animation list set above
            Functions_Animation.Animate(MenuItem.compAnim, MenuItem.compSprite);
        }

    }
}