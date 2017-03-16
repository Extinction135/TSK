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
        }


        public void Draw()
        {
            timingText.text = "u: " + screen.updateTime.Ticks;
            timingText.text += "\nd: " + screen.drawTime.Ticks;
            timingText.text += "\nt: " + screen.totalTime.Milliseconds + " ms";
            timingText.text += "\n" + screen.gameTime.TotalGameTime.ToString(@"hh\:mm\:ss");
            timingText.text += "\n" + MemoryManager.AppMemoryUsage / 1024 / 1024 + " mb";

            actorText.text = "actor: hero";
            actorText.text += "\ninp: " + screen.hero.inputState;
            actorText.text += "\ncur: " + screen.hero.state;
            actorText.text += "\nlck: " + screen.hero.stateLocked;
            actorText.text += "\ndir: " + screen.hero.direction;

            moveText.text = "pos x:" + screen.hero.spriteComp.position.X + ", y:" + screen.hero.spriteComp.position.Y;
            moveText.text += "\nspd:" + screen.hero.moveComp.speed + "  fric:" + screen.hero.moveComp.friction;
            moveText.text += "\nmag x:" + screen.hero.moveComp.magnitude.X;
            moveText.text += "\nmag y:" + screen.hero.moveComp.magnitude.Y;
            moveText.text += "\ndir: " + screen.hero.moveComp.direction;

            //moveText.text += "\nfric:" 

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