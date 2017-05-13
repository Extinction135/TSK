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
    public static class MovementFunctions
    {

        static float maxMagnitude = 7.0f;



        public static void Push(ComponentMovement Move, Direction Direction, float Amount)
        {
            //modify move components magnitude based on direction by amount
            if (Direction == Direction.Down) { Move.magnitude.Y += Amount; }
            else if (Direction == Direction.Left) { Move.magnitude.X -= Amount; }
            else if (Direction == Direction.Right) { Move.magnitude.X += Amount; }
            else if (Direction == Direction.Up) { Move.magnitude.Y -= Amount; }
            else if (Direction == Direction.DownLeft)
            { Move.magnitude.Y += Amount * 0.75f; Move.magnitude.X -= Amount * 0.75f; }
            else if (Direction == Direction.DownRight)
            { Move.magnitude.Y += Amount * 0.75f; Move.magnitude.X += Amount * 0.75f; }
            else if (Direction == Direction.UpLeft)
            { Move.magnitude.Y -= Amount * 0.75f; Move.magnitude.X -= Amount * 0.75f; }
            else if (Direction == Direction.UpRight)
            { Move.magnitude.Y -= Amount * 0.75f; Move.magnitude.X += Amount * 0.75f; }
        }

        public static void Move(Actor Actor)
        {
            ProjectMovement(Actor.compMove);
            Functions_Collision.CheckCollisions(Actor);
            Functions_Component.Align(Actor.compMove, Actor.compSprite, Actor.compCollision);
        }

        public static void Move(GameObject Obj)
        {   //particles never move or check collisions
            if (Obj.group != ObjGroup.Particle)
            {
                ProjectMovement(Obj.compMove);
                Functions_Collision.CheckCollisions(Obj);
            }
            Functions_Component.Align(Obj.compMove, Obj.compSprite, Obj.compCollision);
        }

        public static void ProjectMovement(ComponentMovement Move)
        {
            Push(Move, Move.direction, Move.speed);
            //apply friction to magnitude
            Move.magnitude = Move.magnitude * Move.friction;
            //clip magnitude to 0 when it gets very small
            if (Math.Abs(Move.magnitude.X) < 0.01f) { Move.magnitude.X = 0; }
            if (Math.Abs(Move.magnitude.Y) < 0.01f) { Move.magnitude.Y = 0; }
            //clip magnitude's maximum values
            if (Move.magnitude.X > maxMagnitude) { Move.magnitude.X = maxMagnitude; }
            else if (Move.magnitude.X < -maxMagnitude) { Move.magnitude.X = -maxMagnitude; }
            if (Move.magnitude.Y > maxMagnitude) { Move.magnitude.Y = maxMagnitude; }
            else if (Move.magnitude.Y < -maxMagnitude) { Move.magnitude.Y = -maxMagnitude; }
            //project newPosition based on current position + magnitude
            Move.newPosition.X = Move.position.X;
            Move.newPosition.Y = Move.position.Y;
            Move.newPosition += Move.magnitude;
        }

        public static void Teleport(ComponentMovement Move, float X, float Y)
        {
            Move.position.X = X;
            Move.position.Y = Y;
            Move.newPosition.X = X;
            Move.newPosition.Y = Y;
        }

        public static void StopMovement(ComponentMovement Move)
        {
            Move.magnitude.X = 0;
            Move.magnitude.Y = 0;
            Move.speed = 0;
        }

    }
}