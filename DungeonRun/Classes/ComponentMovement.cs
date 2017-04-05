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
    public class ComponentMovement
    {
        public Vector2 position = new Vector2(100,100); //current position of actor/object
        public Vector2 newPosition = new Vector2(100, 100); //projected position
        
        public Direction direction = Direction.Down; //the direction actor/obj is moving
        public Vector2 magnitude = new Vector2(0,0); //how much actor/obj moves each frames
        public float speed = 0.25f; //controls magnitude
        public float friction = 0.75f; //reduces magnitude each frame


        public void ProjectMovement()
        {
            //calculate magnitude
            if (direction == Direction.Down) { magnitude.Y += speed; }
            else if (direction == Direction.Left) { magnitude.X -= speed; }
            else if (direction == Direction.Right) { magnitude.X += speed; }
            else if (direction == Direction.Up) { magnitude.Y -= speed; }
            else if (direction == Direction.DownLeft) { magnitude.Y += speed * 0.75f; magnitude.X -= speed * 0.75f; }
            else if (direction == Direction.DownRight) { magnitude.Y += speed * 0.75f; magnitude.X += speed * 0.75f; }
            else if (direction == Direction.UpLeft) { magnitude.Y -= speed * 0.75f; magnitude.X -= speed * 0.75f; }
            else if (direction == Direction.UpRight) { magnitude.Y -= speed * 0.75f; magnitude.X += speed * 0.75f; }

            //apply friction to magnitude, clip magnitude to 0 when it gets very small
            magnitude = magnitude * friction;
            if (Math.Abs(magnitude.X) < 0.01f) { magnitude.X = 0; }
            if (Math.Abs(magnitude.Y) < 0.01f) { magnitude.Y = 0; }
            //project newPosition based on current position + magnitude
            newPosition.X = position.X;
            newPosition.Y = position.Y;
            newPosition += magnitude;
        }
    }
}