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
    public static class DrawFunctions
    {

        public static void Draw(Dungeon Dungeon)
        {   //draw the dungeon's room collision recs
            for (int i = 0; i < Dungeon.rooms.Count; i++)
            { Draw(Dungeon.rooms[i].collision); }
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
            Draw(MenuWindow.headerLine);
            Draw(MenuWindow.footerLine);
            //only draw the title if the window is completely open
            if (MenuWindow.interior.displayState == DisplayState.Opened)
            { Draw(MenuWindow.title); }
        }

    }
}