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
        float fadeSpeed = 0.05f;

        int enemyCount = 0;
        int totalDamage = 0;
        int reward = 0;
        int rewardTotal = 0;
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
                "time \nenemies \ndamage \nreward",
                new Vector2(220, 150),
                Assets.colorScheme.textLight);
            summaryText.alpha = 0.0f;
            summaryData = new ComponentText(Assets.medFont, 
                "00:00:00 \n0 \n0 \n0",
                new Vector2(330, 150),
                Assets.colorScheme.textLight);
            summaryData.alpha = 0.0f;
            continueText = new ComponentText(Assets.medFont,
                "press any button\n    to play again",
                new Vector2(220, 260),
                Assets.colorScheme.textLight);
            continueText.alpha = 0.0f;

            #endregion

            
            Functions_Music.PlayMusic(Music.Title); //play title music
            //fill hero's health up to max - prevents drum track from playing
            Pool.hero.health = Pool.hero.maxHealth;
            //reward player gold, if dungeon was completed
            if (DungeonRecord.beatDungeon)
            { rewardTotal = 99; } else { rewardTotal = 0; }
            PlayerData.saveData.gold += rewardTotal;
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
                    displayState = DisplayState.Closing;
                    //close the animated titles
                    if (leftTitle.displayState == DisplayState.Opened)
                    { leftTitle.displayState = DisplayState.Closing; }
                    if (rightTitle.displayState == DisplayState.Opened)
                    { rightTitle.displayState = DisplayState.Closing; }
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
                leftTitle.compSprite.alpha += fadeSpeed;
                rightTitle.compSprite.alpha += fadeSpeed;
                continueText.alpha += fadeSpeed;
                summaryText.alpha += fadeSpeed;
                summaryData.alpha += fadeSpeed;
                //check components position + opacity, transition state
                if (rightTitle.displayState == DisplayState.Opened && 
                    leftTitle.displayState == DisplayState.Opened &&
                    continueText.alpha >= 1.0f)
                { continueText.alpha = 1.0f; displayState = DisplayState.Opened; }
            }
            else if (displayState == DisplayState.Closing)
            {
                //fade out components
                leftTitle.compSprite.alpha -= fadeSpeed * 1.5f;
                rightTitle.compSprite.alpha -= fadeSpeed * 1.5f;
                summaryText.alpha -= fadeSpeed * 1.5f;
                summaryData.alpha -= fadeSpeed * 1.5f;
                continueText.alpha -= fadeSpeed * 0.9f;
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
                {   //if the screen should count the summary values
                    //animate the summary data up to it's proper amount
                    if (enemyCount < DungeonRecord.enemyCount)
                    { enemyCount++; Assets.Play(Assets.sfxTextLetter); }
                    else
                    {
                        if (totalDamage < DungeonRecord.totalDamage)
                        { totalDamage++; Assets.Play(Assets.sfxTextLetter); }
                        else
                        {
                            if (reward < rewardTotal)
                            { reward++; Assets.Play(Assets.sfxTextLetter); }
                            else //we've counted everything, exit the count routine
                            { Assets.Play(Assets.sfxTextDone); countingComplete = true; }
                        }
                    }
                }

                //set the summary data text component
                summaryData.text = "" + DungeonRecord.timer.Elapsed.ToString(@"hh\:mm\:ss");
                summaryData.text += "\n" + enemyCount; //enemies
                summaryData.text += "\n" + totalDamage; //damage
                summaryData.text += "\n" + reward; //reward
            }

            #endregion


            #region Handle Exit State

            else if (displayState == DisplayState.Closed)
            {   //exit this screen, return to overworld
                //ScreenManager.RemoveScreen(this);
                //ScreenManager.AddScreen(new ScreenOverworld());
                ScreenManager.ExitAndLoad(new ScreenOverworld());
            }

            #endregion

        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            //draw background first
            ScreenManager.spriteBatch.Draw(Assets.dummyTexture,
                background.rec, Assets.colorScheme.overlay * background.alpha);
            Functions_Draw.Draw(leftTitle.compSprite);
            Functions_Draw.Draw(rightTitle.compSprite);
            Functions_Draw.Draw(summaryData);
            Functions_Draw.Draw(summaryText);
            Functions_Draw.Draw(continueText);
            ScreenManager.spriteBatch.End();
        }

    }
}