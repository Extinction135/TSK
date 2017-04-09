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
        public static void Draw(ComponentSprite Sprite, ScreenManager ScreenManager)
        {
            if (Sprite.visible)
            {
                //set draw rec
                Sprite.drawRec.X = (int)(Sprite.cellSize.x * Sprite.currentFrame.x);
                Sprite.drawRec.Y = (int)(Sprite.cellSize.y * Sprite.currentFrame.y);
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

        
        public static void Draw(ComponentCollision Coll, ScreenManager ScreenManager)
        {   //draw the collision rec of the collision component
            ScreenManager.spriteBatch.Draw(
                ScreenManager.game.assets.dummyTexture,
                Coll.rec,
                ScreenManager.game.colorScheme.collisionActor);
        }
    }
}