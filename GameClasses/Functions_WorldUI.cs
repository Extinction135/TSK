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
    public static class Functions_WorldUI
    {
        public static int i;
        public static int count;



        public static void Update()
        {

            #region Update & Limit Hero's Hearts

            //clip max/total Hearts to 9
            if (PlayerData.current.heartsTotal > 9)
            { PlayerData.current.heartsTotal = 9; }
            //assign maxHearts
            WorldUI.maxHearts = PlayerData.current.heartsTotal;
            //match maxHearts to maxHealth, clip current health to max value
            Pool.hero.maxHealth = WorldUI.maxHearts; 
            if (Pool.hero.health > WorldUI.maxHearts)
            { Pool.hero.health = WorldUI.maxHearts; }
            //animate (scale) the hero's hearts
            WorldUI.currentHeartCount = Pool.hero.health; //get the current heart count
            if (WorldUI.currentHeartCount != WorldUI.lastHeartCount) //if current does not equal last
            {   //scale up the current hearts, hero's health changed from last frame
                for (i = 0; i < WorldUI.currentHeartCount; i++)
                { WorldUI.hearts[i].scale = 1.5f; }
            }
            WorldUI.lastHeartCount = Pool.hero.health; //set the last heart count to current
            //set heart sprites to outline or empty, based on maxHearts
            for (i = 0; i < WorldUI.hearts.Count; i++)
            {
                if (i < WorldUI.maxHearts)
                //set the empty (unlocked) hearts
                { WorldUI.hearts[i].currentFrame.Y = 1; }
                //set the outline (locked) hearts
                else { WorldUI.hearts[i].currentFrame.Y = 2; }
                //set the full hearts
                if (i <= Pool.hero.health - 1)
                { WorldUI.hearts[i].currentFrame.Y = 0; }
                //scale each heart back down to 1.0
                if (WorldUI.hearts[i].scale > 1.0f)
                { WorldUI.hearts[i].scale -= 0.05f; }
            }

            #endregion


            #region Update & Limit Hero's Magic

            //get the number of unlocked magic meter pieces
            PlayerData.current.magicTotal = PlayerData.current.magicUnlocked;

            //add the robe effect to the magicTotal value
            if (Pool.hero.armor == MenuItemType.ArmorRobe)
            { PlayerData.current.magicTotal += 4; }

            //limit the magicTotal amount to 9
            if (PlayerData.current.magicTotal > 9)
            { PlayerData.current.magicTotal = 9; }

            //limit the current magic amount to the max magic amount
            if (PlayerData.current.magicCurrent > PlayerData.current.magicTotal)
            { PlayerData.current.magicCurrent = PlayerData.current.magicTotal; }

            //loop thru the magic meter sprites, setting their frame
            for (i = 0; i < 9; i++)
            {   //reset sprite to locked
                WorldUI.meterPieces[i + 1].currentFrame.X = 31;
                //set available bars
                if (i < PlayerData.current.magicTotal)
                { WorldUI.meterPieces[i + 1].currentFrame.X = 30; }
                //set filled bars
                if (i < PlayerData.current.magicCurrent)
                { WorldUI.meterPieces[i + 1].currentFrame.X = 29; }
            }

            #endregion


            #region Update & Animate Weapon and Item

            //weapon and item routines
            if (WorldUI.heroWeapon != Pool.hero.weapon)
            {   //check to see if hero's weapon has changed, update worldUI to new weapon
                WorldUI.heroWeapon = Pool.hero.weapon;
                Functions_MenuItem.SetMenuItemData(WorldUI.heroWeapon, WorldUI.currentWeapon);
            }
            if (WorldUI.heroItem != Pool.hero.item)
            {   //check to see if hero's item has changed, update worldUI to new item
                WorldUI.heroItem = Pool.hero.item;
                Functions_MenuItem.SetMenuItemData(WorldUI.heroItem, WorldUI.currentItem);
            }

            //if the hero has an item equipped, then draw it
            if (WorldUI.heroItem == MenuItemType.Unknown)
            { WorldUI.currentItem.compSprite.visible = false; }
            else { WorldUI.currentItem.compSprite.visible = true; }

            //animate (and scale) the current weapon + item
            Functions_Animation.Animate(WorldUI.currentWeapon.compAnim, WorldUI.currentWeapon.compSprite);
            Functions_Animation.Animate(WorldUI.currentItem.compAnim, WorldUI.currentItem.compSprite);

            #endregion


            #region Limit Hero's arrows, bombs, & gold

            if (PlayerData.current.arrowsCurrent > PlayerData.current.arrowsMax)
            { PlayerData.current.arrowsCurrent = PlayerData.current.arrowsMax; }

            if (PlayerData.current.bombsCurrent > PlayerData.current.bombsMax)
            { PlayerData.current.bombsCurrent = PlayerData.current.bombsMax; }

            if (PlayerData.current.gold > 99) { PlayerData.current.gold = 99; }

            #endregion


            #region Handle Autosave Display & animation

            if (WorldUI.autosaveCounter < 13) //copy autosaving string character by character
            { WorldUI.autosaveText.text = WorldUI.autosaving.Substring(0, WorldUI.autosaveCounter); }

            if (WorldUI.autosaveCounter < 20) { WorldUI.autosaveCounter++; }
            else { WorldUI.autosaveText.text = ""; } //stop counting, clear text

            #endregion

        }

        public static void Draw()
        {
            count = WorldUI.hearts.Count();
            for (i = 0; i < count; i++)
            {
                Functions_Draw.Draw(WorldUI.hearts[i]);
            }
            count = WorldUI.meterPieces.Count();
            for (i = 0; i < count; i++)
            {
                Functions_Draw.Draw(WorldUI.meterPieces[i]);
            }
            for (i = 0; i < 4; i++)
            {
                Functions_Draw.Draw(WorldUI.weaponBkg[i]);
                Functions_Draw.Draw(WorldUI.itemBkg[i]);
            }
            Functions_Draw.Draw(WorldUI.currentWeapon.compSprite);
            Functions_Draw.Draw(WorldUI.weaponAmount);
            Functions_Draw.Draw(WorldUI.currentItem.compSprite);
            Functions_Draw.Draw(WorldUI.itemAmount);
            Functions_Draw.Draw(WorldUI.autosaveText);
            if (Flags.DrawUDT)
            {
                WorldUI.frametime.text = "U:" + Timing.updateTime.Milliseconds;
                WorldUI.frametime.text += "\nD:" + Timing.drawTime.Milliseconds;
                WorldUI.frametime.text += "\nT:" + Timing.totalTime.Milliseconds;
                Functions_Draw.Draw(WorldUI.frametime);
            }
        }



        public static void Move(int X, int Y)
        {
            //move the weapon bkg & sprite & amount display
            MoveBkg(WorldUI.weaponBkg, X + 8, Y + 8);
            WorldUI.currentWeapon.compSprite.position.X = X + 16;
            WorldUI.currentWeapon.compSprite.position.Y = Y + 16;
            Functions_Component.Align(WorldUI.weaponAmount, WorldUI.currentWeapon.compSprite);

            //move the item bkg & sprite & amount display
            MoveBkg(WorldUI.itemBkg, X + 16 * 8 + 8, Y + 8);
            WorldUI.currentItem.compSprite.position.X = X + 16 * 8 + 16;
            WorldUI.currentItem.compSprite.position.Y = Y + 16;
            Functions_Component.Align(WorldUI.itemAmount, WorldUI.currentItem.compSprite);

            //move the hearts
            for (i = 0; i < 9; i++)
            {
                WorldUI.hearts[i].position.X = X + (10 * i) + (16 * 2) + 8;
                WorldUI.hearts[i].position.Y = Y + 8;
            }

            //move the magic meter sprites
            for (i = 0; i < 11; i++)
            {
                WorldUI.meterPieces[i].position.X = X + (8 * i) + (16 * 2) + 8;
                WorldUI.meterPieces[i].position.Y = Y + 8 + 16;
            }

            //place frametime & autosave texts
            WorldUI.frametime.position.X = 32;
            WorldUI.frametime.position.Y = 41+8+1;
            WorldUI.autosaveText.position.X = 54;
            WorldUI.autosaveText.position.Y = 81;
        }

        public static void MoveBkg(List<ComponentSprite> bkgList, int Xpos, int Ypos)
        {
            bkgList[0].position.X = Xpos;
            bkgList[0].position.Y = Ypos;

            bkgList[1].position.X = Xpos + 16;
            bkgList[1].position.Y = Ypos;
            bkgList[1].flipHorizontally = true;

            bkgList[2].position.X = Xpos;
            bkgList[2].position.Y = Ypos + 16;
            bkgList[2].flipHorizontally = true;

            bkgList[3].position.X = Xpos + 16;
            bkgList[3].position.Y = Ypos + 16;

            bkgList[2].rotation = Rotation.Clockwise180;
            bkgList[3].rotation = Rotation.Clockwise180;
        }

        public static void DisplayAutosave()
        {
            WorldUI.autosaveCounter = 0;
        }

    }
}