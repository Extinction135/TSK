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

namespace DungeonRun
{
    public static class Functions_Draw
    {
        static int i;

        public static void DrawLevel()
        {   //draw the dungeon's room collision recs
            for (i = 0; i < Level.rooms.Count; i++)
            {
                ScreenManager.spriteBatch.Draw(
                    Assets.dummyTexture,
                    Level.rooms[i].rec,
                    Assets.colorScheme.roomRec);
            }
            //draw the dungeon doors
            for (i = 0; i < Level.doors.Count; i++)
            {
                ScreenManager.spriteBatch.Draw(
                    Assets.dummyTexture,
                    Level.doors[i].rec,
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




        public static void Draw(List<ComponentSprite> Sprites)
        {
            for(i = 0; i < Sprites.Count; i++) { Draw(Sprites[i]); }
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
            Draw(Display.lastStoryItem.compSprite);
        }

    }
}