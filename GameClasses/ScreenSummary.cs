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
    public class ScreenSummary : Screen
    {
        ScreenRec background = new ScreenRec();
        ComponentSprite title;
        ComponentText summaryText;
        ComponentText summaryData;
        ComponentText continueText;
        float textFadeSpeed = 0.05f;
        Boolean countingComplete = false;

        int enemyCount = 0;
        int totalDamage = 0;
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
            Pool.hero.health = PlayerData.current.heartsTotal;


            #region Update title

            if (DungeonRecord.beatDungeon) //won
            { title.currentFrame.Y = 2; }
            else
            { title.currentFrame.Y = 1; } //loss

            #endregion


            #region Calculate skill ratings + ratings change

            int enemiesKilledNew = PlayerData.current.enemiesKilled + DungeonRecord.enemyCount;
            int damageNew = PlayerData.current.damageTaken + DungeonRecord.totalDamage;

            double newSR = 0.0; //prevent divide by 0
            if (enemiesKilledNew <= 0) { enemiesKilledNew = 1; }
            if (damageNew <= 0) { damageNew = 1; }

            double oldSR = 0.0; // prevent divide by 0
            if (PlayerData.current.enemiesKilled <= 0) { PlayerData.current.enemiesKilled = 1; }
            if (PlayerData.current.damageTaken <= 0) { PlayerData.current.damageTaken = 1; }
            
            //calculate SR and ratings change to 2 digits
            newSR = enemiesKilledNew / damageNew;
            oldSR = PlayerData.current.enemiesKilled / PlayerData.current.damageTaken;
            ratingChange = (float)Math.Round(newSR - oldSR, 2);

            #endregion


            #region Append & Save Player's Summary Data

            //append damage and kills
            PlayerData.current.damageTaken += DungeonRecord.totalDamage;
            PlayerData.current.enemiesKilled += DungeonRecord.enemyCount;
            //convert stopwatch dungeon timer to timespan
            TimeSpan toAdd = DungeonRecord.timer.Elapsed;
            //append hours, mins, seconds from timespan to saveData
            PlayerData.current.hours += toAdd.Hours;
            PlayerData.current.mins += toAdd.Minutes;
            PlayerData.current.secs += toAdd.Seconds;
            //cleanup saveData
            while (PlayerData.current.mins >= 60)
            { PlayerData.current.hours++; PlayerData.current.mins -= 60; }
            while (PlayerData.current.secs >= 60)
            { PlayerData.current.mins++; PlayerData.current.secs -= 60; }

            //these two values combined allow us to track how many times each dungeon & boss has been defeated, or killed hero
            //DungeonRecord.dungeonID
            //DungeonRecord.beatDungeon

            #endregion


            //reset stuff
            title.alpha = 0.0f;
            continueText.alpha = 0.0f;
            summaryText.alpha = 0.0f;
            summaryData.alpha = 0.0f;
            enemyCount = 0;
            totalDamage = 0;
            ratingChange = 0.0f;

            displayState = DisplayState.Opening;
        }

        public override void HandleInput(GameTime GameTime)
        {
            if (displayState == DisplayState.Opened)
            {
                if (Functions_Input.IsNewButtonPress(Buttons.Start) ||
                    Functions_Input.IsNewButtonPress(Buttons.A) ||
                    Functions_Input.IsNewButtonPress(Buttons.B) ||
                    Functions_Input.IsNewButtonPress(Buttons.X) ||
                    Functions_Input.IsNewButtonPress(Buttons.Y))
                {
                    displayState = DisplayState.Closing; //only happens once
                    DungeonRecord.timer.Reset();
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
                {   //animate summary data up to it's proper amount
                    if (enemyCount < DungeonRecord.enemyCount)
                    { enemyCount++; Assets.Play(Assets.sfxTextLetter); }
                    else if(totalDamage < DungeonRecord.totalDamage)
                    { totalDamage++; Assets.Play(Assets.sfxTextLetter); }
                    else  //we've counted everything, exit count routine
                    { Assets.Play(Assets.sfxTextDone); countingComplete = true; }
                }

                //set the summary data text component
                summaryData.text = "" + DungeonRecord.timer.Elapsed.ToString(@"hh\:mm\:ss");
                summaryData.text += "\n" + enemyCount; 
                summaryData.text += "\n" + totalDamage;

                //handle displaying the +, if ratingChange is positive
                if (ratingChange >= 0.0f)
                { summaryData.text += "\n+" + ratingChange; }
                else { summaryData.text += "\n" + ratingChange; }
            }

            #endregion


            #region Handle Exit State

            else if (displayState == DisplayState.Closed)
            {   //exit all screens, return to proper overworld map
                Functions_Overworld.OpenMap();
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