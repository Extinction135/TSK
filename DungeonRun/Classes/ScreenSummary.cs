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
    public class SummaryScreen : Screen
    {

        int animSpeed; //how fast title sprites move, lower is faster

        ComponentSprite leftTitle;
        Vector2 leftTitleStartPos;
        Vector2 leftTitleEndPos;

        ComponentSprite rightTitle;
        Vector2 rightTitleStartPos;
        Vector2 rightTitleEndPos;

        ComponentText summaryText;
        ComponentText summaryData;
        ComponentText continueText;
        float fadeSpeed = 0.05f;

        int enemyCount = 0;
        int totalDamage = 0;
        int reward = 0;
        int rewardTotal = 0;
        Boolean countingComplete = false;



        public SummaryScreen() { this.name = "SummaryScreen"; }

        public override void LoadContent()
        {


            #region Create the various sprites and text components

            float yPos = 60;

            //create the left and right title sprites
            leftTitle = new ComponentSprite(Assets.bigTextSheet, 
                new Vector2(0, 0), new Byte4(0, 0, 0, 0), new Byte2(1, 1));
            leftTitle.alpha = 0.0f;
            rightTitle = new ComponentSprite(Assets.bigTextSheet, 
                new Vector2(0, 0), new Byte4(0, 0, 0, 0), new Byte2(1, 1));
            rightTitle.alpha = 0.0f;

            //create the summary + data + continue text fields
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


            #region Setup the Title Text Sprite Components

            if (DungeonRecord.beatDungeon)
            {   //"dungoen complete"
                leftTitle.currentFrame = new Byte4(0, 0, 0, 0);
                leftTitle.cellSize = new Byte2(16 * 13, 16 * 4);
                leftTitleStartPos = new Vector2(-200, yPos);
                
                rightTitle.currentFrame = new Byte4(0, 1, 0, 0);
                rightTitle.cellSize = new Byte2(16 * 13, 16 * 4);
                rightTitleStartPos = new Vector2(640, yPos);

                leftTitleEndPos = new Vector2(130-15, yPos);
                rightTitleEndPos = new Vector2(305+15, yPos);
                animSpeed = 8; //lower is faster
                Assets.Play(Assets.sfxBeatDungeon);
            }
            else
            {   //"you died"
                leftTitle.currentFrame = new Byte4(0, 2, 0, 0);
                leftTitle.cellSize = new Byte2(16 * 8, 16 * 4);
                leftTitleStartPos = new Vector2(-100, yPos);
                
                rightTitle.currentFrame = new Byte4(1, 2, 0, 0);
                rightTitle.cellSize = new Byte2(16 * 8, 16 * 4);
                rightTitleStartPos = new Vector2(640, yPos);

                leftTitleEndPos = new Vector2(200-10, yPos);
                rightTitleEndPos = new Vector2(285+20, yPos);
                animSpeed = 8; //lower is faster
            }

            #endregion


            //set the title sprites into their starting positions
            leftTitle.position.X = leftTitleStartPos.X;
            leftTitle.position.Y = leftTitleStartPos.Y;
            rightTitle.position.X = rightTitleStartPos.X;
            rightTitle.position.Y = rightTitleStartPos.Y;
            //update the cellsize for the title components, since we changed them
            ComponentFunctions.UpdateCellSize(leftTitle);
            ComponentFunctions.UpdateCellSize(rightTitle);

            //fade out the dungeon music
            MusicFunctions.trackToLoad = Music.None;
            MusicFunctions.fadeState = FadeState.FadeOut;

            //if the player beat the boss, the base reward is 100
            if (DungeonRecord.beatDungeon) { rewardTotal = 100; }
            //calculate additional rewards
            rewardTotal += (DungeonRecord.enemyCount - DungeonRecord.totalDamage);
            //clip the rewardTotal to 0, if it goes negative
            if (rewardTotal < 0) { rewardTotal = 0; }
        }

        public override void HandleInput(GameTime GameTime)
        {
            if (displayState == DisplayState.Opened)
            {
                if (
                    Input.IsNewButtonPress(Buttons.Start) ||
                    Input.IsNewButtonPress(Buttons.A) ||
                    Input.IsNewButtonPress(Buttons.B) ||
                    Input.IsNewButtonPress(Buttons.X) ||
                    Input.IsNewButtonPress(Buttons.Y))
                {
                    displayState = DisplayState.Closing;
                    continueText.alpha = 1.0f;
                    //play the summary exit sound effect immediately
                    Assets.Play(Assets.sfxExitSummary);
                }
            }
        }

        public override void Update(GameTime GameTime)
        {


            #region Animate Title Sprites in / out

            if (displayState == DisplayState.Opening)
            {
                if (leftTitle.position.X < leftTitleEndPos.X)
                {   //move left title to endPos
                    leftTitle.position.X += (leftTitleEndPos.X - leftTitle.position.X) / animSpeed;
                    leftTitle.position.X += 1; //fixes delayed movement
                }
                if (leftTitle.position.X > leftTitleEndPos.X)
                { leftTitle.position.X = leftTitleEndPos.X; }

                if (rightTitle.position.X > rightTitleEndPos.X)
                {   //move right title to endPos
                    rightTitle.position.X -= (rightTitle.position.X - rightTitleEndPos.X) / animSpeed;
                    rightTitle.position.X -= 1; //fixes delayed movement
                }
                if (rightTitle.position.X < rightTitleEndPos.X)
                { rightTitle.position.X = rightTitleEndPos.X; }
                //fade in components
                leftTitle.alpha += fadeSpeed;
                rightTitle.alpha += fadeSpeed;
                continueText.alpha += fadeSpeed;
                summaryText.alpha += fadeSpeed;
                summaryData.alpha += fadeSpeed;
                //check components position + opacity, transition state
                if (rightTitle.position.X == rightTitleEndPos.X && 
                    leftTitle.position.X == leftTitleEndPos.X &&
                    continueText.alpha >= 1.0f)
                { continueText.alpha = 1.0f; displayState = DisplayState.Opened; }
            }
            else if (displayState == DisplayState.Closing)
            {
                if (leftTitle.position.X > leftTitleStartPos.X)
                {   //move left title to startPos
                    leftTitle.position.X -= (leftTitle.position.X - leftTitleStartPos.X) / animSpeed;
                    leftTitle.position.X -= 1; //fixes delayed movement
                }
                if (rightTitle.position.X < rightTitleStartPos.X)
                {   //move right title to startPos
                    rightTitle.position.X += (rightTitleStartPos.X - rightTitle.position.X) / animSpeed;
                    rightTitle.position.X += 1; //fixes delayed movement
                }
                //fade out components
                leftTitle.alpha -= fadeSpeed * 1.5f;
                rightTitle.alpha -= fadeSpeed * 1.5f;
                summaryText.alpha -= fadeSpeed * 1.5f;
                summaryData.alpha -= fadeSpeed * 1.5f;
                continueText.alpha -= fadeSpeed * 0.9f;
                //check components opacity, transition state
                if (continueText.alpha <= 0.0f)
                { continueText.alpha = 0.0f; displayState = DisplayState.Closed; }
            }

            #endregion


            #region Handle Display State

            else if (displayState == DisplayState.Opened)
            {
                //pulse the alpha of the left and right title sprites
                if (leftTitle.alpha >= 1.0f) { leftTitle.alpha = 0.85f; }
                if (rightTitle.alpha >= 1.0f) { rightTitle.alpha = 0.85f; }
                if (continueText.alpha >= 1.0f) { continueText.alpha = 0.85f; }
                leftTitle.alpha += 0.004f;
                rightTitle.alpha += 0.004f;
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
            {
                //award the gold bonus to player data
                PlayerData.saveData.gold += rewardTotal;
                //rebuild the dungeon and exit this screen
                DungeonFunctions.BuildDungeon();
                ScreenManager.RemoveScreen(this);
            }

            #endregion


        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            DrawFunctions.Draw(leftTitle);
            DrawFunctions.Draw(rightTitle);
            DrawFunctions.Draw(summaryData);
            DrawFunctions.Draw(summaryText);
            DrawFunctions.Draw(continueText);
            ScreenManager.spriteBatch.End();
        }

    }
}