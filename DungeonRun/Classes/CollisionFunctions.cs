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
    public static class CollisionFunctions
    {
        public static void Move(ComponentMovement Move, ComponentCollision Coll, ComponentSprite Sprite)
        {
            ProjectMovement(Move);
            CheckCollisions(Move, Coll);
            PlaceSprite(Coll, Sprite);
        }



        public static void ProjectMovement(ComponentMovement Move)
        {
            //calculate magnitude
            if (Move.direction == Direction.Down)
            { Move.magnitude.Y += Move.speed; }
            else if (Move.direction == Direction.Left)
            { Move.magnitude.X -= Move.speed; }
            else if (Move.direction == Direction.Right)
            { Move.magnitude.X += Move.speed; }
            else if (Move.direction == Direction.Up)
            { Move.magnitude.Y -= Move.speed; }
            else if (Move.direction == Direction.DownLeft)
            { Move.magnitude.Y += Move.speed * 0.75f; Move.magnitude.X -= Move.speed * 0.75f; }
            else if (Move.direction == Direction.DownRight)
            { Move.magnitude.Y += Move.speed * 0.75f; Move.magnitude.X += Move.speed * 0.75f; }
            else if (Move.direction == Direction.UpLeft)
            { Move.magnitude.Y -= Move.speed * 0.75f; Move.magnitude.X -= Move.speed * 0.75f; }
            else if (Move.direction == Direction.UpRight)
            { Move.magnitude.Y -= Move.speed * 0.75f; Move.magnitude.X += Move.speed * 0.75f; }
            //apply friction to magnitude, clip magnitude to 0 when it gets very small
            Move.magnitude = Move.magnitude * Move.friction;
            if (Math.Abs(Move.magnitude.X) < 0.01f) { Move.magnitude.X = 0; }
            if (Math.Abs(Move.magnitude.Y) < 0.01f) { Move.magnitude.Y = 0; }
            //project newPosition based on current position + magnitude
            Move.newPosition.X = Move.position.X;
            Move.newPosition.Y = Move.position.Y;
            Move.newPosition += Move.magnitude;
        }

        public static void CheckCollisions(ComponentMovement Move, ComponentCollision Coll)
        {
            //align collisionRec to Move.pos, respecting offset

            //project the collRec along X axis to Move.newPos
            //check collisions against objects + actors
            //handle collisions, calculate collRec movement

            //project the collRec along Y axis to Move.newPos
            //check collisions against objects + actors
            //handle collisions, calculate collRec movement




            //match collisionRec to projected position
            Coll.rec.X = (int)Move.newPosition.X;
            Coll.rec.Y = (int)Move.newPosition.Y;

            //assume no collisions
            Move.position = Move.newPosition;
        }

        public static void PlaceSprite(ComponentCollision Coll, ComponentSprite Sprite)
        {
            //set sprite.pos (pair of floats) to collisionRec.pos (pair of integers)
            //this ensures the sprite position will always be pixel aligned
            Sprite.position.X = Coll.rec.X;
            Sprite.position.Y = Coll.rec.Y;
            Sprite.SetZdepth();
        }
    }
}