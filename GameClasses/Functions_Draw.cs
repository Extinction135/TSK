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
    public static class Functions_Draw
    {
        static int i;

        public static void Draw(Dungeon Dungeon)
        {   //draw the dungeon's room collision recs
            for (int i = 0; i < Dungeon.rooms.Count; i++)
            {
                ScreenManager.spriteBatch.Draw(
                    Assets.dummyTexture, 
                    Dungeon.rooms[i].collision.rec,
                    Assets.colorScheme.roomRec);
            }

            //draw the dungeon door locations too
            for (int i = 0; i < Dungeon.doorLocations.Count; i++)
            {
                Rectangle doorPosRec = new Rectangle(0, 0, 16, 16);

                doorPosRec.X = Dungeon.doorLocations[i].X;
                doorPosRec.Y = Dungeon.doorLocations[i].Y;

                ScreenManager.spriteBatch.Draw(
                    Assets.dummyTexture,
                    doorPosRec,
                    Assets.colorScheme.interaction);
            }
        }

        public static void Draw(GameObject Obj)
        {
            if (Obj.active)
            {
                Draw(Obj.compSprite);
                if (Flags.DrawCollisions)
                { Draw(Obj.compCollision); }
            }
        }

        public static void Draw(Actor Actor)
        {
            if(Actor.active)
            {
                Draw(Actor.compSprite);
                if (Flags.DrawCollisions)
                { Draw(Actor.compCollision); }
            }
        }

        public static void Draw(ComponentSprite Sprite)
        {
            if (Sprite.visible)
            {
                //set draw rec
                Sprite.drawRec.X = (int)(Sprite.cellSize.X * Sprite.currentFrame.X);
                Sprite.drawRec.Y = (int)(Sprite.cellSize.Y * Sprite.currentFrame.Y);
                //update rotationValue based on rotation enum
                if (Sprite.rotation == Rotation.Clockwise90)
                { Sprite.rotationValue = 1.575f; }
                else if (Sprite.rotation == Rotation.Clockwise180)
                { Sprite.rotationValue = 3.15f; }
                else if (Sprite.rotation == Rotation.Clockwise270)
                { Sprite.rotationValue = -1.575f; }
                else { Sprite.rotationValue = 0.0f; }
                //set sprite effect
                if (Sprite.currentFrame.flipHori > 0)
                { Sprite.spriteEffect = SpriteEffects.FlipHorizontally; }
                else { Sprite.spriteEffect = SpriteEffects.None; }
                if (Sprite.flipHorizontally)
                { Sprite.spriteEffect = SpriteEffects.FlipHorizontally; }
                //draw the sprite
                ScreenManager.spriteBatch.Draw(
                    Sprite.texture, Sprite.position, Sprite.drawRec,
                    Sprite.drawColor * Sprite.alpha, Sprite.rotationValue,
                    Sprite.origin, Sprite.scale, Sprite.spriteEffect,
                    Sprite.zDepth);
            }
        }

        public static void Draw(ComponentCollision Coll)
        {   //draw the collision rec of the collision component
            if(Coll.blocking)
            {
                ScreenManager.spriteBatch.Draw(
                    Assets.dummyTexture, Coll.rec,
                    Assets.colorScheme.collision);
            }
            else
            {
                ScreenManager.spriteBatch.Draw(
                    Assets.dummyTexture, Coll.rec,
                    Assets.colorScheme.interaction);
            }
        }

        public static void Draw(ComponentText Text)
        {   
            ScreenManager.spriteBatch.DrawString(
                Text.font, Text.text, Text.position,
                Text.color * Text.alpha, Text.rotation, Vector2.Zero,
                Text.scale, SpriteEffects.None, Text.zDepth);
        }

        public static void Draw(ComponentButton Button)
        {   //draw buttons background rec, foreground text
            ScreenManager.spriteBatch.Draw(
                Assets.dummyTexture, Button.rec,
                Button.currentColor);
            Draw(Button.compText);
        }

        public static void Draw(MenuRectangle MenuRec)
        {
            ScreenManager.spriteBatch.Draw(
                Assets.dummyTexture, 
                MenuRec.rec, 
                MenuRec.color);
        }

        public static void Draw(MenuWindow MenuWindow)
        {
            Draw(MenuWindow.background);
            Draw(MenuWindow.border);
            Draw(MenuWindow.inset);
            Draw(MenuWindow.interior);
            for (i = 0; i < MenuWindow.lines.Count; i++)
            { Draw(MenuWindow.lines[i]); }

            //only draw the title if the window is completely open
            if (MenuWindow.interior.displayState == DisplayState.Opened)
            { Draw(MenuWindow.title); }
        }

        public static void Draw(ComponentAmountDisplay Amnt)
        {  //draw the amnt.bkg & amnt.text component
            ScreenManager.spriteBatch.Draw(
                Assets.dummyTexture, Amnt.bkg,
                Assets.colorScheme.windowBkg);
            Draw(Amnt.amount);
        }

        public static void Draw(ScreenRec Rec)
        {  
            ScreenManager.spriteBatch.Draw(
                Assets.dummyTexture, Rec.rec, 
                Assets.colorScheme.overlay * Rec.alpha);
        }

        public static void Draw(GameDisplayData Display)
        {
            Draw(Display.menuItem.compSprite);
            Draw(Display.hero);
            Draw(Display.timeDateText);
            for (i = 0; i < 6; i++) { Draw(Display.crystals[i].compSprite); }
        }



        public static void DrawDebugInfo()
        {

            #region Calculate Update + Draw Times, Frame Times, and Ram useage

            DebugInfo.frameCounter++;
            if (DebugInfo.frameCounter > DebugInfo.framesTotal)
            {   //reset the counter + total ticks
                DebugInfo.frameCounter = 0;
                DebugInfo.updateTicks = 0;
                DebugInfo.drawTicks = 0;
            }
            else if (DebugInfo.frameCounter == DebugInfo.framesTotal)
            {   //calculate the average ticks
                DebugInfo.updateAvg = DebugInfo.updateTicks / DebugInfo.framesTotal;
                DebugInfo.drawAvg = DebugInfo.drawTicks / DebugInfo.framesTotal;
            }
            //collect tick times
            DebugInfo.updateTicks += Timing.updateTime.Ticks;
            DebugInfo.drawTicks += Timing.drawTime.Ticks;

            //average over framesTotal
            DebugInfo.timingText.text = "u: " + DebugInfo.updateAvg;
            DebugInfo.timingText.text += "\nd: " + DebugInfo.drawAvg;
            DebugInfo.timingText.text += "\nt: " + Timing.totalTime.Milliseconds + " ms";
            DebugInfo.timingText.text += "\n" + ScreenManager.gameTime.TotalGameTime.ToString(@"hh\:mm\:ss");
            DebugInfo.timingText.text += "\n" + Functions_Backend.GetRam() + " mb";

            #endregion


            #region Actor + Movement Components

            DebugInfo.actorText.text = "actor: hero";
            DebugInfo.actorText.text += "\ninp: " + Pool.hero.inputState;
            DebugInfo.actorText.text += "\ncur: " + Pool.hero.state;
            DebugInfo.actorText.text += "\nlck: " + Pool.hero.stateLocked;
            DebugInfo.actorText.text += "\ndir: " + Pool.hero.direction;

            DebugInfo.moveText.text = "pos x:" + Pool.hero.compSprite.position.X + ", y:" + Pool.hero.compSprite.position.Y;
            DebugInfo.moveText.text += "\nspd:" + Pool.hero.compMove.speed + "  fric:" + Pool.hero.compMove.friction;
            DebugInfo.moveText.text += "\nmag x:" + Pool.hero.compMove.magnitude.X;
            DebugInfo.moveText.text += "\nmag y:" + Pool.hero.compMove.magnitude.Y;
            DebugInfo.moveText.text += "\ndir: " + Pool.hero.compMove.direction;

            #endregion


            #region Pool, Creation Time, and Record Components

            DebugInfo.poolText.text = "floors: " + Pool.floorIndex + "/" + Pool.floorCount;

            DebugInfo.poolText.text += "\nobjs: " + Pool.roomObjIndex + "/" + Pool.roomObjCount;
            DebugInfo.poolText.text += "\nactrs: " + Pool.actorIndex + "/" + Pool.actorCount;
            DebugInfo.poolText.text += "\npros: " + Pool.entityIndex + "/" + Pool.entityCount;

            DebugInfo.creationText.text = "timers";
            DebugInfo.creationText.text += "\nroom: " + DebugInfo.roomTime;
            DebugInfo.creationText.text += "\ndung: " + DebugInfo.dungeonTime;

            DebugInfo.recordText.text = "record";
            DebugInfo.recordText.text += "\ntime: " + DungeonRecord.timer.Elapsed.ToString(@"hh\:mm\:ss");
            DebugInfo.recordText.text += "\nenemies: " + DungeonRecord.enemyCount;
            DebugInfo.recordText.text += "\ndamage: " + DungeonRecord.totalDamage;

            #endregion


            #region Music + SaveData Components

            DebugInfo.musicText.text = "music: " + Functions_Music.trackToLoad;
            DebugInfo.musicText.text += "\n" + Functions_Music.currentMusic.State + ": " + Functions_Music.currentMusic.Volume;
            DebugInfo.musicText.text += "\n" + Assets.musicDrums.State + ": " + Assets.musicDrums.Volume;

            //saveDataText.text = "save data";
            //saveDataText.text += "\ngold: " + PlayerData.saveData.gold;

            #endregion


            ScreenManager.spriteBatch.Draw(Assets.dummyTexture, DebugInfo.background, Assets.colorScheme.debugBkg);
            DebugInfo.size = DebugInfo.textFields.Count();
            for (DebugInfo.counter = 0; DebugInfo.counter < DebugInfo.size; DebugInfo.counter++)
            { Draw(DebugInfo.textFields[DebugInfo.counter]); }
        }

        public static void DrawDebugMenu()
        {   //draw the background rec with correct color
            ScreenManager.spriteBatch.Draw( Assets.dummyTexture, DebugMenu.rec, Assets.colorScheme.debugBkg);
            //loop draw all the buttons
            for (DebugMenu.counter = 0; DebugMenu.counter < DebugMenu.buttons.Count; DebugMenu.counter++)
            { Draw(DebugMenu.buttons[DebugMenu.counter]); }
        }

    }
}