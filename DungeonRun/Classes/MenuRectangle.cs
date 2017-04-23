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
    public class MenuRectangle
    {

        public enum DisplayState { Opening, Open }
        public DisplayState displayState = DisplayState.Opening;

        public int animationSpeed = 5;      //how quickly the UI element animates in/out
        public int animationCounter = 0;    //counts up to delay value
        public int openDelay = 0;           //how many updates are ignored before open animation occurs
        public Rectangle rec;
        public Point position;
        public Point size;
        public Color color;

        public MenuRectangle(Point Position, Point Size, Color Color)
        {
            position = Position;
            size = Size;
            rec = new Rectangle(0, 0, 0, 0);
            color = Color;
            rec.Location = position;
        }

        public void Update()
        {
            if (displayState == DisplayState.Opening)
            {
                if (animationCounter < openDelay) { animationCounter += 1; }
                if (animationCounter >= openDelay)
                {   //grow right
                    rec.Height = size.Y;
                    if (rec.Width < size.X) { rec.Width += ((size.X - rec.Width) / animationSpeed) + 1; } //easeIn 
                    if (rec.Width > size.X) { rec.Width = size.X; }
                    if (rec.Width == size.X) { displayState = DisplayState.Open; animationCounter = 0; } //open complete
                }
            }
        }

    }
}