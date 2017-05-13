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
    public static class WorldUI
    {

        public static int i;

        public static List<ComponentSprite> hearts;
        public static int lastHeartCount = 3;
        public static int currentHeartCount = 3;
        public static byte maxHearts = 0;
        public static int pieceCounter = 0;

        public static List<ComponentSprite> meterPieces;

        public static List<ComponentSprite> weaponBkg;
        public static List<ComponentSprite> itemBkg;
        public static MenuItem currentWeapon;
        public static MenuItem currentItem;
        public static MenuItemType heroWeapon;
        public static MenuItemType heroItem;

        public static ComponentText frametime;



        public static void Move(int X, int Y)
        {
            //move the weapon bkg & sprite
            MoveBkg(weaponBkg, X + 8, Y + 8);
            currentWeapon.compSprite.position.X = X + 16;
            currentWeapon.compSprite.position.Y = Y + 16;

            //move the item bkg & sprite
            MoveBkg(itemBkg, X + 16 * 8 + 8, Y + 8);
            currentItem.compSprite.position.X = X + 16 * 8 + 16;
            currentItem.compSprite.position.Y = Y + 16;

            //move the hearts
            for (i = 0; i < 9; i++)
            {
                hearts[i].position.X = X + (10 * i) + (16 * 2) + 8;
                hearts[i].position.Y = Y + 8;
            }

            //move the magic meter sprites
            for (i = 0; i < 11; i++)
            {
                meterPieces[i].position.X = X + (8 * i) + (16 * 2) + 8;
                meterPieces[i].position.Y = Y + 8 + 16;
            }
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



        static WorldUI()
        {
            //create the heart sprites
            hearts = new List<ComponentSprite>();
            for (i = 0; i < 9; i++)
            {
                hearts.Add(new ComponentSprite(Assets.mainSheet, 
                    new Vector2(0, 0), new Byte4(15, 2, 0, 0), 
                    new Point(16, 16)));
            }

            //create the meter sprites
            meterPieces = new List<ComponentSprite>();
            for (i = 0; i < 11; i++)
            {
                meterPieces.Add(new ComponentSprite(Assets.mainSheet,
                    new Vector2(0, 0), new Byte4(31, 3, 0, 0),
                    new Point(8, 16)));
            }

            //set the head and tail meter frames
            meterPieces[0].currentFrame.X = 28;
            meterPieces[10].currentFrame.X = 28;
            meterPieces[10].flipHorizontally = true;

            //create the weapon and item background sprites
            weaponBkg = new List<ComponentSprite>();
            itemBkg = new List<ComponentSprite>();
            for (i = 0; i < 4; i++)
            {
                weaponBkg.Add(new ComponentSprite(Assets.mainSheet,
                    new Vector2(0, 0), new Byte4(15, 4, 0, 0),
                    new Point(16, 16)));
                itemBkg.Add(new ComponentSprite(Assets.mainSheet,
                    new Vector2(0, 0), new Byte4(15, 4, 0, 0),
                    new Point(16, 16)));
            }

            //create the current weapon & item menuItems
            currentWeapon = new MenuItem();
            currentItem = new MenuItem();

            //get the hero's current weapon and item
            heroWeapon = Pool.hero.weapon;
            heroItem = Pool.hero.item;
            MenuItemFunctions.SetMenuItemData(heroWeapon, currentWeapon);
            MenuItemFunctions.SetMenuItemData(heroItem, currentItem);

            //create the frametime text component
            frametime = new ComponentText(Assets.font, "test", 
                new Vector2(640 - 55, 41), Assets.colorScheme.textLight);

            //move the entire worldUI
            Move(50, 50);
        }

        public static void Update()
        {

            #region Update & Limit Hero's Hearts

            //reset maxHearts and pieceCounter, we will calculate them for this frame
            maxHearts = 0; pieceCounter = 0;
            //determine the max hearts that hero has, based on heart pieces
            for (i = 0; i < PlayerData.saveData.heartPieces; i++)
            {
                pieceCounter++; //hearts are groups of 4 pieces
                if(pieceCounter == 4) { maxHearts++; pieceCounter = 0; }
            }
            //clip maxHearts to 14, match hero's health
            if(maxHearts > 9) { maxHearts = 9; }
            Pool.hero.maxHealth = maxHearts; //match maxHearts to maxHealth
            if (Pool.hero.health > maxHearts) { Pool.hero.health = maxHearts; }
            //animate (scale) the hero's hearts
            currentHeartCount = Pool.hero.health; //get the current heart count
            if (currentHeartCount != lastHeartCount) //if current does not equal last
            {   //scale up the current hearts, hero's health changed from last frame
                for (i = 0; i < currentHeartCount; i++)
                { hearts[i].scale = 1.5f; }
            }
            lastHeartCount = Pool.hero.health; //set the last heart count to current
            //set heart sprites to outline or empty, based on maxHearts
            for (i = 0; i < hearts.Count; i++)
            {
                if (i < maxHearts) 
                //set the empty (unlocked) hearts
                { hearts[i].currentFrame.Y = 1; }
                //set the outline (locked) hearts
                else { hearts[i].currentFrame.Y = 2; }
                //set the full hearts
                if (i <= Pool.hero.health - 1)
                { hearts[i].currentFrame.Y = 0; }
                //scale each heart back down to 1.0
                if (hearts[i].scale > 1.0f)
                { hearts[i].scale -= 0.05f; }
            }

            #endregion


            #region Update & Limit Hero's Magic

            //limit the max magic amount to 9
            if (PlayerData.saveData.magicMax > 9) { PlayerData.saveData.magicMax = 9; }
            //limit the current magic amount to the max magic amount
            if (PlayerData.saveData.magicCurrent > PlayerData.saveData.magicMax)
            { PlayerData.saveData.magicCurrent = PlayerData.saveData.magicMax; }
            //loop thru the magic meter sprites, setting their frame
            for (i = 0; i < 9; i++)
            {   //reset sprite to locked
                meterPieces[i + 1].currentFrame.X = 31;
                //set available bars
                if (i < PlayerData.saveData.magicMax)
                { meterPieces[i + 1].currentFrame.X = 30; }
                //set filled bars
                if (i < PlayerData.saveData.magicCurrent)
                { meterPieces[i + 1].currentFrame.X = 29; }
            }

            #endregion


            #region Update & Animate Weapon and Item

            //weapon and item routines
            if (heroWeapon != Pool.hero.weapon)
            {   //check to see if hero's weapon has changed, update worldUI to new weapon
                heroWeapon = Pool.hero.weapon;
                MenuItemFunctions.SetMenuItemData(heroWeapon, currentWeapon);
            }
            if (heroItem != Pool.hero.item)
            {   //check to see if hero's item has changed, update worldUI to new item
                heroItem = Pool.hero.item;
                MenuItemFunctions.SetMenuItemData(heroItem, currentItem);
            }

            //if the hero has an item equipped, then draw it
            if (heroItem == MenuItemType.Unknown)
            { currentItem.compSprite.visible = false; }
            else { currentItem.compSprite.visible = true; }

            //animate (and scale) the current weapon + item
            Functions_Animation.Animate(currentWeapon.compAnim, currentWeapon.compSprite);
            Functions_Animation.Animate(currentItem.compAnim, currentItem.compSprite);

            #endregion


            //limit the hero's gold to a max of 99
            if (PlayerData.saveData.gold > 99) { PlayerData.saveData.gold = 99; }
        }

        public static void Draw()
        {
            for (i = 0; i < hearts.Count; i++) { DrawFunctions.Draw(hearts[i]); }
            for (i = 0; i < meterPieces.Count; i++) { DrawFunctions.Draw(meterPieces[i]); }
            for (i = 0; i < 4; i++)
            {
                DrawFunctions.Draw(weaponBkg[i]);
                DrawFunctions.Draw(itemBkg[i]);
            }
            DrawFunctions.Draw(currentWeapon.compSprite);
            DrawFunctions.Draw(currentItem.compSprite);
            if (!Flags.Release)
            {
                frametime.text = "U:" + Timing.updateTime.Milliseconds;
                frametime.text += "\nD:" + Timing.drawTime.Milliseconds;
                frametime.text += "\nT:" + Timing.totalTime.Milliseconds;
                DrawFunctions.Draw(frametime);
            }
        }

    }
}