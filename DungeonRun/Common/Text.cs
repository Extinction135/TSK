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
    public class Text
    {
        ScreenManager screenManager;
        public SpriteFont font;
        public String text;             //the string of text to draw
        public Vector2 position;        //the position of the text to draw
        public Color color;             //the color of the text to draw
        public float alpha = 1.0f;      //the opacity of the text
        public float rotation = 0.0f;
        public float scale = 1.0f;
        public float zDepth = 0.001f;   //the layer to draw the text to

        public Text(ScreenManager ScreenManager, SpriteFont Font, String Text, Vector2 Position)
        {
            screenManager = ScreenManager;
            position = Position;
            text = Text;
            color = screenManager.game.colorScheme.textSmall;
            font = Font;
        }

        public void Draw()
        {
            screenManager.spriteBatch.DrawString(
                font, text, position, color * alpha, rotation, 
                Vector2.Zero, scale, SpriteEffects.None, zDepth);
        }
    }
}