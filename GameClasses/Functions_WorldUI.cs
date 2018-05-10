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
            //clip hero.health to maxHearts value
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

            //limit the magicTotal amount to 9
            if (PlayerData.current.magicTotal > 9)
            { PlayerData.current.magicTotal = 9; }

            //limit the current magic amount to the max magic amount
            if (PlayerData.current.magicCurrent > PlayerData.current.magicTotal)
            { PlayerData.current.magicCurrent = PlayerData.current.magicTotal; }

            //loop thru the magic meter sprites, setting their frame
            for (i = 0; i < 9; i++)
            {   //reset sprite to locked
                WorldUI.meterPieces[i + 1].currentFrame.X = 5*2+1;
                //set available bars
                if (i < PlayerData.current.magicTotal)
                { WorldUI.meterPieces[i + 1].currentFrame.X = 5*2; }
                //set filled bars
                if (i < PlayerData.current.magicCurrent)
                { WorldUI.meterPieces[i + 1].currentFrame.X = 4*2+1; }
            }

            #endregion


            #region Limit Hero's arrows, bombs, & gold

            if (PlayerData.current.arrowsCurrent > PlayerData.current.arrowsMax)
            { PlayerData.current.arrowsCurrent = PlayerData.current.arrowsMax; }

            if (PlayerData.current.bombsCurrent > PlayerData.current.bombsMax)
            { PlayerData.current.bombsCurrent = PlayerData.current.bombsMax; }

            if (PlayerData.current.gold > 99) { PlayerData.current.gold = 99; }

            #endregion


            #region Update & Animate Weapon and Item

            //weapon and item routines
            if (WorldUI.heroWeapon != Pool.hero.weapon)
            {   //check to see if hero's weapon has changed, update worldUI to new weapon
                WorldUI.heroWeapon = Pool.hero.weapon;
                Functions_MenuItem.SetType(WorldUI.heroWeapon, WorldUI.currentWeapon);
            }
            if (WorldUI.heroItem != Pool.hero.item)
            {   //check to see if hero's item has changed, update worldUI to new item
                WorldUI.heroItem = Pool.hero.item;
                Functions_MenuItem.SetType(WorldUI.heroItem, WorldUI.currentItem);
            }


            WorldUI.itemAmount.visible = false; //assume false
            if (WorldUI.heroItem == MenuItemType.ItemBow)
            {   //check to see if we should display arrow's remaining ammo
                Functions_Component.UpdateAmount(WorldUI.itemAmount, PlayerData.current.arrowsCurrent);
                WorldUI.itemAmount.visible = true;
            }
            if (WorldUI.heroItem == MenuItemType.ItemBomb)
            {   //check to see if we should display bombs's remaining ammo
                Functions_Component.UpdateAmount(WorldUI.itemAmount, PlayerData.current.bombsCurrent);
                WorldUI.itemAmount.visible = true;
            }



            //if the hero has an item equipped, then draw it
            if (WorldUI.heroItem != MenuItemType.Unknown)
            { WorldUI.currentItem.compSprite.visible = true; }
            else { WorldUI.currentItem.compSprite.visible = false; }

            //animate (and scale) the current weapon + item
            Functions_Animation.Animate(WorldUI.currentWeapon.compAnim, WorldUI.currentWeapon.compSprite);
            Functions_Animation.ScaleSpriteDown(WorldUI.currentWeapon.compSprite);
            Functions_Animation.Animate(WorldUI.currentItem.compAnim, WorldUI.currentItem.compSprite);
            Functions_Animation.ScaleSpriteDown(WorldUI.currentItem.compSprite);

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
            Functions_Draw.Draw(WorldUI.currentItem.compSprite);
            if (WorldUI.itemAmount.visible) { Functions_Draw.Draw(WorldUI.itemAmount); }
            Functions_Draw.Draw(WorldUI.autosaveText);
            if (Flags.DrawUDT)
            {
                WorldUI.frametime.text = "C:" + Pool.collisionsCount;
                WorldUI.frametime.text += "\nI:" + Pool.interactionsCount;
                WorldUI.frametime.text += "\n";
                WorldUI.frametime.text += "\nU:" + DebugInfo.updateAvg;
                WorldUI.frametime.text += "\nD:" + DebugInfo.drawAvg;
                WorldUI.frametime.text += "\nT:" + Timing.totalTime.Milliseconds + " ms";
                Functions_Draw.Draw(WorldUI.frametime);
            }
        }

        public static void DisplayAutosave()
        {
            WorldUI.autosaveCounter = 0;
        }

    }
}