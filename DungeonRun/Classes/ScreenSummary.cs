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
        public SummaryScreen() { this.name = "SummaryScreen"; }

        enum ScreenState { AnimateIn, Display, AnimateOut, Exit }
        ScreenState screenState = ScreenState.AnimateIn;

        //how fast title sprites move, lower is faster
        int animSpeed; 

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
                "enemies\ntime\ndamage\nreward",
                new Vector2(220, 150),
                Assets.colorScheme.textSmall);
            summaryText.alpha = 0.0f;
            summaryData = new ComponentText(Assets.medFont,
                "13\n00:10:31\n14\n100",
                new Vector2(330, 150),
                Assets.colorScheme.textSmall);
            summaryData.alpha = 0.0f;
            continueText = new ComponentText(Assets.medFont,
                "press any button\n    to play again",
                new Vector2(220, 260),
                Assets.colorScheme.textSmall);
            continueText.alpha = 0.0f;

            #endregion


            //populate the summary data text with record data
            summaryData.text = "" + DungeonRecord.enemyCount; //enemies
            summaryData.text += "\n" + DungeonRecord.totalTime;
            summaryData.text += "\n" + DungeonRecord.totalDamage; //damage
            summaryData.text += "\n" + 0; //reward


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
        }

        public override void HandleInput(GameTime GameTime)
        {
            if (screenState == ScreenState.Display)
            {
                if(
                    Input.IsNewButtonPress(Buttons.Start) ||
                    Input.IsNewButtonPress(Buttons.A) ||
                    Input.IsNewButtonPress(Buttons.B) ||
                    Input.IsNewButtonPress(Buttons.X) ||
                    Input.IsNewButtonPress(Buttons.Y))
                { screenState = ScreenState.AnimateOut; }
            }
        }

        public override void Update(GameTime GameTime)
        {


            #region Animate Title Sprites in / out

            if (screenState == ScreenState.AnimateIn)
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
                { continueText.alpha = 1.0f; screenState = ScreenState.Display; }
            }
            else if (screenState == ScreenState.AnimateOut)
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
                continueText.alpha -= fadeSpeed;
                summaryText.alpha -= fadeSpeed;
                summaryData.alpha -= fadeSpeed;
                //check components opacity, transition state
                if (summaryText.alpha <= 0.0f)
                { summaryText.alpha = 0.0f; screenState = ScreenState.Exit; }
            }

            #endregion


            #region Handle Display State

            else if (screenState == ScreenState.Display)
            {
                //pulse the alpha of the left and right title sprites
                if (leftTitle.alpha >= 1.0f) { leftTitle.alpha = 0.85f; }
                if (rightTitle.alpha >= 1.0f) { rightTitle.alpha = 0.85f; }
                if (continueText.alpha >= 1.0f) { continueText.alpha = 0.85f; }
                leftTitle.alpha += 0.004f;
                rightTitle.alpha += 0.004f;
                continueText.alpha += 0.01f;
            }

            #endregion


            #region Handle Exit State

            else if (screenState == ScreenState.Exit)
            {
                DungeonGenerator.BuildRoom();
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