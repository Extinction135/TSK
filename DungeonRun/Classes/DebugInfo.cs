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
using Windows.System;

namespace DungeonRun
{
    public static class DebugInfo
    {

        public static Rectangle background;
        public static List<ComponentText> textFields;
        public static int counter = 0;
        public static int size = 0;

        public static ComponentText timingText;
        public static ComponentText actorText;
        public static ComponentText moveText;
        public static ComponentText poolText;
        public static ComponentText creationText;
        public static ComponentText recordText;
        public static ComponentText musicText;
        public static ComponentText saveDataText;

        public static long roomTime = 0;
        public static long dungeonTime = 0;

        public static byte framesTotal = 30; //how many frames to average over
        public static byte frameCounter = 0; //increments thru frames 0-framesTotal
        public static long updateTicks; //update tick times are added to this
        public static long drawTicks; //draw tick times are added to this
        public static long updateAvg; //stores the average update ticks
        public static long drawAvg; //stores the average draw ticks



        static DebugInfo()
        {
            textFields = new List<ComponentText>();

            background = new Rectangle(0, 322 - 8, 640, 50);
            int yPos = background.Y - 2;

            timingText = new ComponentText(Assets.font, "", 
                new Vector2(2, yPos + 00), Assets.colorScheme.textLight);
            textFields.Add(timingText);

            actorText = new ComponentText(Assets.font, "", 
                new Vector2(16 * 3, yPos + 00), Assets.colorScheme.textLight);
            textFields.Add(actorText);

            moveText = new ComponentText(Assets.font, "", 
                new Vector2(16 * 7, yPos + 00), Assets.colorScheme.textLight);
            textFields.Add(moveText);

            poolText = new ComponentText(Assets.font, "", 
                new Vector2(16 * 12, yPos + 00), Assets.colorScheme.textLight);
            textFields.Add(poolText);

            creationText = new ComponentText(Assets.font, "",
                new Vector2(16 * 17, yPos + 00), Assets.colorScheme.textLight);
            textFields.Add(creationText);

            recordText = new ComponentText(Assets.font, "",
                new Vector2(16 * 21, yPos + 00), Assets.colorScheme.textLight);
            textFields.Add(recordText);

            musicText = new ComponentText(Assets.font, "",
                new Vector2(16 * 26 - 8, yPos + 00), Assets.colorScheme.textLight);
            textFields.Add(musicText);

            saveDataText = new ComponentText(Assets.font, "",
                new Vector2(16 * 30, yPos + 00), Assets.colorScheme.textLight);
            textFields.Add(saveDataText);

        }

        public static void Draw()
        {

            #region Calculate Update + Draw Times, Frame Times, and Ram useage

            frameCounter++;
            if (frameCounter > framesTotal)
            {   //reset the counter + total ticks
                frameCounter = 0;
                updateTicks = 0;
                drawTicks = 0;
            }
            else if (frameCounter == framesTotal)
            {   //calculate the average ticks
                updateAvg = updateTicks / framesTotal;
                drawAvg = drawTicks / framesTotal;
            }
            //collect tick times
            updateTicks += Timing.updateTime.Ticks;
            drawTicks += Timing.drawTime.Ticks;

            //per frame
            //timingText.text = "u: " + screen.updateTime.Ticks;
            //timingText.text += "\nd: " + screen.drawTime.Ticks;
            //timingText.text += "\nt: " + screen.totalTime.Milliseconds + " ms";
            //average over framesTotal
            timingText.text = "u: " + updateAvg;
            timingText.text += "\nd: " + drawAvg;
            timingText.text += "\nt: " + Timing.totalTime.Milliseconds + " ms";
            timingText.text += "\n" + ScreenManager.gameTime.TotalGameTime.ToString(@"hh\:mm\:ss");
            timingText.text += "\n" + MemoryManager.AppMemoryUsage / 1024 / 1024 + " mb";

            #endregion


            #region Actor + Movement Components

            actorText.text = "actor: hero";
            actorText.text += "\ninp: " + Pool.hero.inputState;
            actorText.text += "\ncur: " + Pool.hero.state;
            actorText.text += "\nlck: " + Pool.hero.stateLocked;
            actorText.text += "\ndir: " + Pool.hero.direction;

            moveText.text = "pos x:" + Pool.hero.compSprite.position.X + ", y:" + Pool.hero.compSprite.position.Y;
            moveText.text += "\nspd:" + Pool.hero.compMove.speed + "  fric:" + Pool.hero.compMove.friction;
            moveText.text += "\nmag x:" + Pool.hero.compMove.magnitude.X;
            moveText.text += "\nmag y:" + Pool.hero.compMove.magnitude.Y;
            moveText.text += "\ndir: " + Pool.hero.compMove.direction;

            #endregion


            #region Pool, Creation Time, and Record Components

            poolText.text = "floors: " + Pool.floorIndex + "/" + Pool.floorCount;

            poolText.text += "\nobjs: " + Pool.objIndex + "/" + Pool.objCount;
            poolText.text += "\nactrs: " + Pool.actorIndex + "/" + Pool.actorCount;
            poolText.text += "\npros: " + Pool.projectileIndex + "/" + Pool.projectileCount;

            creationText.text = "timers";
            creationText.text += "\nroom: " + roomTime;
            creationText.text += "\ndung: " + dungeonTime;

            recordText.text = "record";
            recordText.text += "\ntime: " + DungeonRecord.timer.Elapsed.ToString(@"hh\:mm\:ss");
            recordText.text += "\nenemies: " + DungeonRecord.enemyCount;
            recordText.text += "\ndamage: " + DungeonRecord.totalDamage;

            #endregion


            #region Music + SaveData Components

            musicText.text = "music: " + MusicFunctions.trackToLoad;
            musicText.text += "\n" + MusicFunctions.currentMusic.State + ": " + MusicFunctions.currentMusic.Volume;
            musicText.text += "\n" + Assets.musicDrums.State + ": " + Assets.musicDrums.Volume;

            saveDataText.text = "save data";
            saveDataText.text += "\ngold: " + PlayerData.saveData.gold;

            #endregion


            ScreenManager.spriteBatch.Draw(Assets.dummyTexture, background, Assets.colorScheme.debugBkg);
            size = textFields.Count();
            for (counter = 0; counter < size; counter++) { DrawFunctions.Draw(textFields[counter]); }
        }

    }
}