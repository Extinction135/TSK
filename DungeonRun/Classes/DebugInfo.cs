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
        public ScreenManager screenManager;

        public Rectangle background;

        public List<Text> textFields;
        public int counter = 0;
        public int size = 0;

        public Text timingText;
        public Text actorText;
        public Text moveText;
        public Text poolText;
        

        public byte framesTotal = 30; //how many frames to average over
        public byte frameCounter = 0; //increments thru frames 0-framesTotal
        public long updateTicks; //update tick times are added to this
        public long drawTicks; //draw tick times are added to this
        public long updateAvg; //stores the average update ticks
        public long drawAvg; //stores the average draw ticks


        public DebugInfo(DungeonScreen DungeonScreen)
        {
            screen = DungeonScreen;
            screenManager = DungeonScreen.screenManager;
            textFields = new List<Text>();

            background = new Rectangle(0, 322 - 8, 640, 50);
            int yPos = background.Y - 2;

            timingText = new Text(screenManager, screen.assets.font, "", new Vector2(2, yPos + 00));
            textFields.Add(timingText);

            actorText = new Text(screenManager, screen.assets.font, "", new Vector2(16 * 3, yPos + 00));
            textFields.Add(actorText);

            moveText = new Text(screenManager, screen.assets.font, "", new Vector2(16 * 7, yPos + 00));
            textFields.Add(moveText);

            poolText = new Text(screenManager, screen.assets.font, "", new Vector2(16 * 12, yPos + 00));
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
            timingText.text += "\n" + screen.gameTime.TotalGameTime.ToString(@"hh\:mm\:ss");
            timingText.text += "\n" + MemoryManager.AppMemoryUsage / 1024 / 1024 + " mb";

            #endregion


            actorText.text = "actor: hero";
            actorText.text += "\ninp: " + screen.pool.hero.inputState;
            actorText.text += "\ncur: " + screen.pool.hero.state;
            actorText.text += "\nlck: " + screen.pool.hero.stateLocked;
            actorText.text += "\ndir: " + screen.pool.hero.direction;

            moveText.text = "pos x:" + screen.pool.hero.compSprite.position.X + ", y:" + screen.pool.hero.compSprite.position.Y;
            moveText.text += "\nspd:" + screen.pool.hero.compMove.speed + "  fric:" + screen.pool.hero.compMove.friction;
            moveText.text += "\nmag x:" + screen.pool.hero.compMove.magnitude.X;
            moveText.text += "\nmag y:" + screen.pool.hero.compMove.magnitude.Y;
            moveText.text += "\ndir: " + screen.pool.hero.compMove.direction;

            poolText.text = "floors: " + screen.pool.floorIndex + "/" + screen.pool.floorCount;
            poolText.text += "\nobjs: " + screen.pool.objIndex + "/" + screen.pool.objCount;
            poolText.text += "\nactors: " + screen.pool.actorIndex + "/" + screen.pool.actorCount;
            poolText.text += "\nprojectiles: 0/0";
            poolText.text += "\nparticles: 0/0";
            

            //stateText.text += "\nstate: " + dungeonScreen.hero.state;
            //stateText.text += "\nlocked: " + dungeonScreen.hero.stateLocked;
            //stateText.text += "\naTime: " + dungeonScreen.hero.animationFunctions.timer;
            //stateText.text += "\nframe: " + dungeonScreen.hero.spriteData.currentFrame[0] + ", " + dungeonScreen.hero.spriteData.currentFrame[1];

            //spriteText.text = "pos: " + dungeonScreen.hero.sprite.position.X + "," + dungeonScreen.hero.sprite.position.Y;
            //spriteText.text += "\ncam: " + dungeonScreen.camera.currentPosition.X + "," + dungeonScreen.camera.currentPosition.Y;
            //spriteText.text += "\nz: " + dungeonScreen.hero.sprite.zDepth;

            screenManager.spriteBatch.Draw(screen.assets.dummyTexture, background, screen.game.colorScheme.windowBkg);
            size = textFields.Count();
            for (counter = 0; counter < size; counter++) { textFields[counter].Draw(); }
        }
    }
}