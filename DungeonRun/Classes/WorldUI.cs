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

        public static List<ComponentSprite> hearts;
        public static int lastHeartCount;
        public static int currentHeartCount;
        public static byte maxHearts = 0;
        public static int pieceCounter = 0;

        public static List<ComponentSprite> weaponBkg;
        public static List<ComponentSprite> itemBkg;
        public static ComponentSprite currentWeapon;
        public static ComponentSprite currentItem;
        public static ComponentText frametime;
        public static int counter;



        public static void CreateRow(int Xpos, int Ypos)
        {
            for (counter = 0; counter < 7; counter++)
            {
                hearts.Add(new ComponentSprite(Assets.mainSheet,
                    new Vector2(Xpos + 11 * counter, Ypos),
                    new Byte4(15, 2, 0, 0), new Point(16, 16)));
            }
        }

        public static List<ComponentSprite> CreateBkg(int Xpos, int Ypos)
        {
            List<ComponentSprite> background = new List<ComponentSprite>();
            background.Add(new ComponentSprite(Assets.mainSheet, new Vector2(Xpos, Ypos),
                new Byte4(15, 4, 0, 0), new Point(16, 16)));
            background.Add(new ComponentSprite(Assets.mainSheet, new Vector2(Xpos + 16, Ypos),
                new Byte4(15, 4, 1, 0), new Point(16, 16)));
            background.Add(new ComponentSprite(Assets.mainSheet, new Vector2(Xpos, Ypos + 16),
                new Byte4(15, 4, 1, 0), new Point(16, 16)));
            background.Add(new ComponentSprite(Assets.mainSheet, new Vector2(Xpos + 16, Ypos + 16),
                new Byte4(15, 4, 0, 0), new Point(16, 16)));
            background[2].rotation = Rotation.Clockwise180;
            background[3].rotation = Rotation.Clockwise180;
            return background;
        }

        static WorldUI()
        {
            Point UIpos = new Point(255, 32); //center aligned
            UIpos.X = 50; UIpos.Y = 50; //left top aligned (traditional)

            hearts = new List<ComponentSprite>();
            weaponBkg = CreateBkg(UIpos.X, UIpos.Y);
            CreateRow(UIpos.X + 30, UIpos.Y + 01);
            CreateRow(UIpos.X + 30, UIpos.Y + 14);
            itemBkg = CreateBkg(UIpos.X + 110, UIpos.Y);

            //create the current weapon sprite
            currentWeapon = new ComponentSprite(Assets.mainSheet,
                new Vector2(UIpos.X + 8, UIpos.Y + 8),
                new Byte4(8, 5, 0, 0), new Point(16, 16));

            //create the current item sprite
            currentItem = new ComponentSprite(Assets.mainSheet, 
                new Vector2(UIpos.X + 8 + 110, UIpos.Y + 8),
                new Byte4(7, 5, 0, 0), new Point(16, 16));

            //create the frametime text component
            frametime = new ComponentText(Assets.font, "test", 
                new Vector2(640 - 55, UIpos.Y - 9), Assets.colorScheme.textLight);

            currentHeartCount = 3;
            lastHeartCount = 3;
        }



        public static void Update()
        {   //reset maxHearts and pieceCounter, we will calculate them for this frame
            maxHearts = 0; pieceCounter = 0;
            //determine the max hearts that hero has, based on heart pieces
            for (counter = 0; counter < PlayerData.saveData.heartPieces; counter++)
            {
                pieceCounter++; //hearts are groups of 4 pieces
                if(pieceCounter == 4) { maxHearts++; pieceCounter = 0; }
            }

            //clip maxHearts to 14, match hero's health
            if(maxHearts > 14) { maxHearts = 14; }
            if (Pool.hero.health > maxHearts) { Pool.hero.health = maxHearts; }

            //animate (scale) the hero's hearts
            currentHeartCount = Pool.hero.health; //get the current heart count
            if (currentHeartCount != lastHeartCount) //if current does not equal last
            {   //scale up the current hearts, hero's health changed from last frame
                for (counter = 0; counter < currentHeartCount; counter++)
                { hearts[counter].scale = 1.5f; }
            }
            lastHeartCount = Pool.hero.health; //set the last heart count to current

            //set heart sprites to outline or empty, based on maxHearts
            for (counter = 0; counter < hearts.Count; counter++)
            {
                if (counter < maxHearts) 
                //set the empty (unlocked) hearts
                { hearts[counter].currentFrame.Y = 1; }
                //set the outline (locked) hearts
                else { hearts[counter].currentFrame.Y = 2; }

                //set the full hearts
                if (counter <= Pool.hero.health - 1)
                { hearts[counter].currentFrame.Y = 0; }
                //scale each heart back down to 1.0
                if (hearts[counter].scale > 1.0f)
                { hearts[counter].scale -= 0.05f; }
            }

            //scale current weapon and item back down to 1.0
            if (currentWeapon.scale > 1.0f) { currentWeapon.scale -= 0.05f; }
            if (currentItem.scale > 1.0f) { currentItem.scale -= 0.05f; }

            //limit the hero's gold to a max of 99
            if (PlayerData.saveData.gold > 99) { PlayerData.saveData.gold = 99; }
        }

        public static void Draw()
        {
            for (counter = 0; counter < hearts.Count; counter++)
            {
                //check to see if sprite is visible
                DrawFunctions.Draw(hearts[counter]);
            }
            for (counter = 0; counter < itemBkg.Count; counter++)
            {
                DrawFunctions.Draw(weaponBkg[counter]);
                DrawFunctions.Draw(itemBkg[counter]);
            }
            DrawFunctions.Draw(currentWeapon);
            DrawFunctions.Draw(currentItem);
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