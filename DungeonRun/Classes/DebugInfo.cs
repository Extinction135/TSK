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
using Windows.System;

namespace DungeonRun
{
    public class DebugInfo
    {
        public DungeonScreen screen;
        public Rectangle background;
        public List<ComponentText> textFields;
        public int counter = 0;
        public int size = 0;

        public ComponentText timingText;
        public ComponentText actorText;
        public ComponentText moveText;
        public ComponentText poolText;
        
        public byte framesTotal = 30; //how many frames to average over
        public byte frameCounter = 0; //increments thru frames 0-framesTotal
        public long updateTicks; //update tick times are added to this
        public long drawTicks; //draw tick times are added to this
        public long updateAvg; //stores the average update ticks
        public long drawAvg; //stores the average draw ticks


        public DebugInfo(DungeonScreen DungeonScreen)
        {
            screen = DungeonScreen;
            textFields = new List<ComponentText>();

            background = new Rectangle(0, 322 - 8, 640, 50);
            int yPos = background.Y - 2;

            timingText = new ComponentText(Assets.font, "", 
                new Vector2(2, yPos + 00), Assets.colorScheme.textSmall);
            textFields.Add(timingText);

            actorText = new ComponentText(Assets.font, "", 
                new Vector2(16 * 3, yPos + 00), Assets.colorScheme.textSmall);
            textFields.Add(actorText);

            moveText = new ComponentText(Assets.font, "", 
                new Vector2(16 * 7, yPos + 00), Assets.colorScheme.textSmall);
            textFields.Add(moveText);

            poolText = new ComponentText(Assets.font, "", 
                new Vector2(16 * 12, yPos + 00), Assets.colorScheme.textSmall);
            textFields.Add(poolText);
        }

        public void Draw()
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
            updateTicks += screen.updateTime.Ticks;
            drawTicks += screen.drawTime.Ticks;

            //per frame
            //timingText.text = "u: " + screen.updateTime.Ticks;
            //timingText.text += "\nd: " + screen.drawTime.Ticks;
            //timingText.text += "\nt: " + screen.totalTime.Milliseconds + " ms";
            //average over framesTotal
            timingText.text = "u: " + updateAvg;
            timingText.text += "\nd: " + drawAvg;
            timingText.text += "\nt: " + screen.totalTime.Milliseconds + " ms";
            timingText.text += "\n" + ScreenManager.gameTime.TotalGameTime.ToString(@"hh\:mm\:ss");
            timingText.text += "\n" + MemoryManager.AppMemoryUsage / 1024 / 1024 + " mb";

            #endregion


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

            poolText.text = "floors: " + Pool.floorIndex + "/" + Pool.floorCount;

            poolText.text += "\nobjs: " + Pool.objsUsed + "/" + Pool.objCount;
            poolText.text += "\nactors: " + Pool.actorsUsed + "/" + Pool.actorCount;
            poolText.text += "\nprojectiles: " + Pool.projectilesUsed + " / " + Pool.projectileCount;
            poolText.text += "\nparticles: 0/0";


            ScreenManager.spriteBatch.Draw(Assets.dummyTexture, background, Assets.colorScheme.windowBkg);
            size = textFields.Count();
            for (counter = 0; counter < size; counter++) { DrawFunctions.Draw(textFields[counter]); }
        }

    }
}