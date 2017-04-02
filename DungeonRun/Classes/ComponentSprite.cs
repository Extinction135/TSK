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
    public class ComponentSprite
    {
        SpriteBatch spriteBatch;

        public Texture2D texture;
        public Vector2 position;
        public Byte4 currentFrame;
        public Byte2 cellSize;

        public SpriteEffects spriteEffect; //flip vertically, flip horizontally, none
        public Boolean visible;
        public Vector2 origin;
        public Rectangle drawRec;

        public Color drawColor;
        public float alpha;
        public float scale;
        public int zOffset;

        public float zDepth;
        public Rotation rotation;
        public float rotationValue;





        public ComponentSprite(SpriteBatch SpriteBatch, Texture2D Texture, Vector2 Position, Byte4 CurrentFrame, Byte2 CellSize)
        {
            spriteBatch = SpriteBatch;

            texture = Texture;
            position = Position;
            currentFrame = CurrentFrame;
            cellSize = CellSize;

            spriteEffect = SpriteEffects.None;
            visible = true;
            CenterOrigin();
            drawRec = new Rectangle((int)Position.X, (int)Position.Y, (int)CellSize.x, (int)CellSize.y);

            drawColor = new Color(255, 255, 255);
            alpha = 1.0f;
            scale = 1.0f;
            zOffset = 0;

            SetZdepth();
            rotation = Rotation.None;
            rotationValue = 0.0f;
        }



        public void CenterOrigin() { origin.X = cellSize.x * 0.5f; origin.Y = cellSize.y * 0.5f; }

        public void SetZdepth() { zDepth = 0.999990f - ((position.Y + zOffset) * 0.000001f); }

        public void UpdateCellSize() { drawRec.Width = cellSize.x; drawRec.Height = cellSize.y; }


        public void Draw()
        {
            if (visible)
            {
                //align position to nearest pixel
                position.X = (int)position.X;
                position.Y = (int)position.Y;

                //set draw rec
                drawRec.X = (int)(cellSize.x * currentFrame.x);
                drawRec.Y = (int)(cellSize.y * currentFrame.y);

                //update rotationValue based on rotation enum
                if (rotation == Rotation.Clockwise90) { rotationValue = 1.575f; }
                else if (rotation == Rotation.Clockwise180) { rotationValue = 3.15f; }
                else if (rotation == Rotation.Clockwise270) { rotationValue = -1.575f; }
                else { rotationValue = 0.0f; }

                //set sprite effect
                if (currentFrame.flipHori > 0)
                { spriteEffect = SpriteEffects.FlipHorizontally; }
                else { spriteEffect = SpriteEffects.None; }

                //draw the sprite
                spriteBatch.Draw(texture, position, drawRec,
                    drawColor * alpha, rotationValue,
                    origin, scale, spriteEffect, zDepth);
            }
        }
    }
}