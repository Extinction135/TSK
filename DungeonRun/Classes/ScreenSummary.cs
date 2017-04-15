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
        Boolean won = true; //false means player died
        public SummaryScreen(Boolean Won) { this.name = "SummaryScreen"; won = Won; }

        enum State { AnimateIn, Display, AnimateOut, Exit }
        State state = State.AnimateIn;

        ComponentSprite leftTitle;
        Vector2 leftTitleEndPos;
        ComponentSprite rightTitle;
        Vector2 rightTitleEndPos;

        ComponentText summaryText;
        ComponentText summaryData;
        ComponentText continueText;
        float fadeSpeed = 0.05f;

        public override void LoadContent()
        {


            #region Create the various sprites and text components

            float yPos = 60;
            //create the left and right title sprites offscreen
            leftTitle = new ComponentSprite(Assets.bigTextSheet, 
                new Vector2(-200, yPos), new Byte4(0, 0, 0, 0), new Byte2(1, 1));
            rightTitle = new ComponentSprite(Assets.bigTextSheet, 
                new Vector2(640 + 10, yPos), new Byte4(0, 1, 0, 0), new Byte2(1, 1));

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


            #region Determine text to display for title sprites, and ending positions

            if (won)
            {
                leftTitle.currentFrame = new Byte4(0, 0, 0, 0);
                leftTitle.cellSize = new Byte2(16 * 13, 16 * 4);
                leftTitleEndPos = new Vector2(125, yPos);

                rightTitle.currentFrame = new Byte4(0, 1, 0, 0);
                rightTitle.cellSize = new Byte2(16 * 13, 16 * 4);
                rightTitleEndPos = new Vector2(325, yPos);
            }
            else
            {
                leftTitle.currentFrame = new Byte4(0, 2, 0, 0);
                leftTitle.cellSize = new Byte2(16 * 8, 16 * 4);
                leftTitleEndPos = new Vector2(200 - 10, yPos);

                rightTitle.currentFrame = new Byte4(1, 2, 0, 0);
                rightTitle.cellSize = new Byte2(16 * 8, 16 * 4);
                rightTitleEndPos = new Vector2(300 - 0, yPos);
            }

            #endregion


            ComponentFunctions.UpdateCellSize(leftTitle);
            ComponentFunctions.UpdateCellSize(rightTitle);
        }

        public override void HandleInput(GameTime GameTime)
        {
            if (state == State.Display)
            {
                if(
                    Input.IsNewButtonPress(Buttons.Start) ||
                    Input.IsNewButtonPress(Buttons.A) ||
                    Input.IsNewButtonPress(Buttons.B) ||
                    Input.IsNewButtonPress(Buttons.X) ||
                    Input.IsNewButtonPress(Buttons.Y))
                { state = State.AnimateOut; }
            }
        }

        public override void Update(GameTime GameTime)
        {


            #region Animate Title Sprites in / out

            if (state == State.AnimateIn)
            {
                if (leftTitle.position.X < leftTitleEndPos.X)
                {   //move left title to the right
                    leftTitle.position.X += (leftTitleEndPos.X - leftTitle.position.X) / 10;
                    leftTitle.position.X += 1; //fixes delayed movement
                }
                if (rightTitle.position.X > rightTitleEndPos.X)
                {   //move right title to the left
                    rightTitle.position.X -= (rightTitle.position.X - rightTitleEndPos.X) / 10;
                    rightTitle.position.X -= 1; //fixes delayed movement
                }
                //fade in other text components
                continueText.alpha += fadeSpeed;
                summaryText.alpha += fadeSpeed;
                summaryData.alpha += fadeSpeed;
                //once continue text hits 100% opacity, transition to display state
                if (continueText.alpha >= 1.0f)
                { continueText.alpha = 1.0f; state = State.Display; }
            }
            else if (state == State.AnimateOut)
            {
                if (leftTitle.position.X > -200)
                {   //move left title to the left offscreen
                    leftTitle.position.X -= (leftTitle.position.X - 200) / 10;
                    leftTitle.position.X -= 1; //fixes delayed movement
                }
                if (rightTitle.position.X < 650)
                {   //move right title to the left
                    rightTitle.position.X += (650 - rightTitle.position.X) / 10;
                    rightTitle.position.X += 1; //fixes delayed movement
                }
                //fade out other text components
                continueText.alpha -= fadeSpeed;
                summaryText.alpha -= fadeSpeed;
                summaryData.alpha -= fadeSpeed;
                //once continue text hits 100% opacity, transition to display state
                if (continueText.alpha <= 0.0f)
                { continueText.alpha = 0.0f; state = State.Exit; }
            }

            #endregion


            #region Handle Display State

            else if (state == State.Display)
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

            else if (state == State.Exit)
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