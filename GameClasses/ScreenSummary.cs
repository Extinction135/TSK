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
        TitleAnimated leftTitle;
        TitleAnimated rightTitle;
        ComponentText summaryText;
        ComponentText summaryData;
        ComponentText continueText;
        float textFadeSpeed = 0.05f;

        int enemyCount = 0;
        int totalDamage = 0;
        float runSkillRating = 0.0f;
        Boolean countingComplete = false;



        public ScreenSummary() { this.name = "SummaryScreen"; }

        public override void LoadContent()
        {
            background.alpha = 1.0f;


            #region Create animated Titles

            float yPos = 80;
            if (DungeonRecord.beatDungeon)
            {   //'dungeon complete' state
                leftTitle = new TitleAnimated(
                    new Vector2(-200, yPos),
                    new Vector2(115 + 5, yPos),
                    TitleText.Dungeon, 8);
                rightTitle = new TitleAnimated(
                    new Vector2(640, yPos),
                    new Vector2(320 + 5, yPos),
                    TitleText.Complete, 8);
                Assets.Play(Assets.sfxBeatDungeon);
            }
            else
            {   //'you died' state
                leftTitle = new TitleAnimated(
                    new Vector2(-100, yPos),
                    new Vector2(190 + 4, yPos),
                    TitleText.You, 8);
                rightTitle = new TitleAnimated(
                    new Vector2(640, yPos),
                    new Vector2(305 + 4, yPos),
                    TitleText.Died, 8);
            }

            #endregion


            #region Create the summary + data + continue text fields

            summaryText = new ComponentText(Assets.medFont, 
                "time \nenemies \ndamage \nrating",
                new Vector2(220, 150),
                Assets.colorScheme.textLight);
            summaryText.alpha = 0.0f;
            summaryData = new ComponentText(Assets.medFont, 
                "00:00:00 \n0 \n0 \n0",
                new Vector2(330, 150),
                Assets.colorScheme.textLight);
            summaryData.alpha = 0.0f;
            continueText = new ComponentText(Assets.medFont,
                "press any button\n    to continue",
                new Vector2(220, 260),
                Assets.colorScheme.textLight);
            continueText.alpha = 0.0f;

            #endregion

            
            Functions_Music.PlayMusic(Music.Title); //play title music
            //fill hero's health up to max - prevents drum track from playing
            Pool.hero.health = PlayerData.current.heartsTotal;


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
            //autosave PlayerData.current
            Functions_Backend.SaveGame(GameFile.AutoSave);

            //these two values combined allow us to track how many times each dungeon & boss has been defeated, or killed hero
            //DungeonRecord.dungeonID
            //DungeonRecord.beatDungeon

            #endregion


            runSkillRating = (float)DungeonRecord.enemyCount / (float)DungeonRecord.totalDamage;
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
            {   //animate titles into place
                Functions_TitleAnimated.AnimateMovement(leftTitle);
                Functions_TitleAnimated.AnimateMovement(rightTitle);
                //fade in components
                leftTitle.compSprite.alpha += textFadeSpeed;
                rightTitle.compSprite.alpha += textFadeSpeed;
                continueText.alpha += textFadeSpeed;
                summaryText.alpha += textFadeSpeed;
                summaryData.alpha += textFadeSpeed;
                //check components position + opacity, transition state
                if (rightTitle.displayState == DisplayState.Opened && 
                    leftTitle.displayState == DisplayState.Opened &&
                    continueText.alpha >= 1.0f)
                { continueText.alpha = 1.0f; displayState = DisplayState.Opened; }
            }
            else if (displayState == DisplayState.Closing)
            {
                //fade out components
                leftTitle.compSprite.alpha -= textFadeSpeed * 1.5f;
                rightTitle.compSprite.alpha -= textFadeSpeed * 1.5f;
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
            {
                //pulse the alpha of the left and right title sprites
                if (leftTitle.compSprite.alpha >= 1.0f) { leftTitle.compSprite.alpha = 0.85f; }
                if (rightTitle.compSprite.alpha >= 1.0f) { rightTitle.compSprite.alpha = 0.85f; }
                if (continueText.alpha >= 1.0f) { continueText.alpha = 0.85f; }
                leftTitle.compSprite.alpha += 0.004f;
                rightTitle.compSprite.alpha += 0.004f;
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
                summaryData.text += "\n" + enemyCount; //enemies
                summaryData.text += "\n" + totalDamage; //damage
                summaryData.text += "\n" + runSkillRating; //skill rating
            }

            #endregion


            #region Handle Exit State

            else if (displayState == DisplayState.Closed)
            {   //exit all screens, return to overworld
                ScreenManager.ExitAndLoad(new ScreenOverworld());
            }

            #endregion

        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            Functions_Draw.Draw(background);
            Functions_Draw.Draw(leftTitle.compSprite);
            Functions_Draw.Draw(rightTitle.compSprite);
            Functions_Draw.Draw(summaryData);
            Functions_Draw.Draw(summaryText);
            Functions_Draw.Draw(continueText);
            ScreenManager.spriteBatch.End();
        }

    }
}