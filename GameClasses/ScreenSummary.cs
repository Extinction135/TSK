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
    public class ScreenSummary : Screen
    {
        ScreenRec background = new ScreenRec();
        ComponentSprite title;
        ComponentText summaryText;
        ComponentText summaryData;
        ComponentText continueText;
        float textFadeSpeed = 0.05f;
        Boolean countingComplete = false;
        float ratingChange = 0.0f;


        public ScreenSummary()
        {
            this.name = "SummaryScreen";


            #region Create Title

            title = new ComponentSprite(Assets.bigTextSheet,
                    new Vector2(583 - 256, 80),
                    new Byte4(0, 1, 0, 0),
                    new Point(16 * 16, 16 * 4));
            title.alpha = 0.0f;

            #endregion


            #region Create the summary + data + continue text fields

            summaryText = new ComponentText(Assets.medFont,
                "time \nenemies \ndamage \nrating",
                new Vector2(220, 150),
                ColorScheme.textLight);
            summaryText.alpha = 0.0f;
            summaryData = new ComponentText(Assets.medFont,
                "00:00:00 \n0 \n0 \n0",
                new Vector2(330, 150),
                ColorScheme.textLight);
            summaryData.alpha = 0.0f;
            continueText = new ComponentText(Assets.medFont,
                "press any button\n      to continue",
                new Vector2(220, 260),
                ColorScheme.textLight);
            continueText.alpha = 0.0f;

            #endregion

        }

        public override void Open()
        {
            background.alpha = 1.0f;
            Functions_Music.PlayMusic(Music.Title); //play title music
            //fill hero's health up to max - prevents drum track from playing
            Pool.hero.health = PlayerData.heartsTotal;
            


            #region Calculate skill ratings + ratings change

            //int damageNew = PlayerData.damageTaken + DungeonRecord.totalDamage;

            double newSR = 0.0; //prevent divide by 0
            //if (enemiesKilledNew <= 0) { enemiesKilledNew = 1; }
            //if (damageNew <= 0) { damageNew = 1; }

            double oldSR = 0.0; // prevent divide by 0
            if (PlayerData.enemiesKilled <= 0) { PlayerData.enemiesKilled = 1; }
            if (PlayerData.damageTaken <= 0) { PlayerData.damageTaken = 1; }
            
            //calculate SR and ratings change to 2 digits
            //newSR = enemiesKilledNew / damageNew;
            oldSR = PlayerData.enemiesKilled / PlayerData.damageTaken;
            ratingChange = (float)Math.Round(newSR - oldSR, 2);

            #endregion


            #region Append & Save Player's Summary Data

            //append damage and kills
            //PlayerData.damageTaken += DungeonRecord.totalDamage;
            //PlayerData.enemiesKilled += DungeonRecord.enemyCount;
            //convert stopwatch dungeon timer to timespan
            //TimeSpan toAdd = DungeonRecord.timer.Elapsed;
            //append hours, mins, seconds from timespan to saveData
            //PlayerData.hours += toAdd.Hours;
            //PlayerData.mins += toAdd.Minutes;
            //PlayerData.secs += toAdd.Seconds;
            //cleanup saveData
            while (PlayerData.mins >= 60)
            { PlayerData.hours++; PlayerData.mins -= 60; }
            while (PlayerData.secs >= 60)
            { PlayerData.mins++; PlayerData.secs -= 60; }

            //these two values combined allow us to track how many times each dungeon & boss has been defeated, or killed hero
            //DungeonRecord.dungeonID
            //DungeonRecord.beatDungeon

            #endregion


            //reset stuff
            title.alpha = 0.0f;
            continueText.alpha = 0.0f;
            summaryText.alpha = 0.0f;
            summaryData.alpha = 0.0f;
            ratingChange = 0.0f;

            displayState = DisplayState.Opening;
        }

        public override void HandleInput(GameTime GameTime)
        {
            if (displayState == DisplayState.Opened)
            {
                if (
                    (Input.Player1.A & Input.Player1.A_Prev == false)
                    ||
                    (Input.Player1.B & Input.Player1.B_Prev == false)
                    ||
                    (Input.Player1.Start & Input.Player1.Start_Prev == false)
                    )
                {
                    displayState = DisplayState.Closing; //only happens once
                    //play the summary exit sound effect immediately
                    Assets.Play(Assets.sfxExitSummary);
                    continueText.alpha = 1.0f;
                }
            }
        }

        public override void Update(GameTime GameTime)
        {

            #region Handle Opening / Closing screen state

            if (displayState == DisplayState.Opening)
            {   //fade in components
                title.alpha += textFadeSpeed;
                continueText.alpha += textFadeSpeed;
                summaryText.alpha += textFadeSpeed;
                summaryData.alpha += textFadeSpeed;
                //check components position + opacity, transition state
                if (continueText.alpha >= 1.0f)
                { continueText.alpha = 1.0f; displayState = DisplayState.Opened; }
            }
            else if (displayState == DisplayState.Closing)
            {
                //fade out components
                title.alpha -= textFadeSpeed * 1.5f;
                summaryText.alpha -= textFadeSpeed * 1.5f;
                summaryData.alpha -= textFadeSpeed * 1.5f;
                continueText.alpha -= textFadeSpeed * 0.9f;
                //check components opacity, transition state
                if (continueText.alpha <= 0.0f)
                { continueText.alpha = 0.0f; displayState = DisplayState.Closed; }
            }

            #endregion


            #region Handle Opened Display State

            else if (displayState == DisplayState.Opened)
            {   //flicker title
                if (title.alpha >= 1.0f) { title.alpha = 0.85f; }
                else if (title.alpha < 0.85f) { title.alpha += 0.03f; }
                title.alpha += 0.005f;
                //flicker continue text
                if (continueText.alpha >= 1.0f) { continueText.alpha = 0.85f; }
                continueText.alpha += 0.01f;

                if(!countingComplete)
                {   
                    /*
                    //animate summary data up to it's proper amount
                    if (enemyCount < DungeonRecord.enemyCount)
                    { enemyCount++; Assets.Play(Assets.sfxTextLetter); }
                    else if(totalDamage < DungeonRecord.totalDamage)
                    { totalDamage++; Assets.Play(Assets.sfxTextLetter); }
                    else  //we've counted everything, exit count routine
                    { Assets.Play(Assets.sfxTextDone); countingComplete = true; }
                    */
                    Assets.Play(Assets.sfxTextDone); countingComplete = true;
                }

                //set the summary data text component
                //summaryData.text = "" + DungeonRecord.timer.Elapsed.ToString(@"hh\:mm\:ss");
                //summaryData.text += "\n" + enemyCount; 
                //summaryData.text += "\n" + totalDamage;

                //handle displaying the +, if ratingChange is positive
                if (ratingChange >= 0.0f)
                { summaryData.text += "\n+" + ratingChange; }
                else { summaryData.text += "\n" + ratingChange; }
            }

            #endregion


            #region Handle Exit State

            else if (displayState == DisplayState.Closed)
            {   //exit all screens, goto title (resets playerdata)
                ScreenManager.ExitAndLoad(Screens.Title);
            }

            #endregion

        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            Functions_Draw.Draw(background);
            Functions_Draw.Draw(title);
            Functions_Draw.Draw(summaryData);
            Functions_Draw.Draw(summaryText);
            Functions_Draw.Draw(continueText);
            ScreenManager.spriteBatch.End();
        }

    }
}