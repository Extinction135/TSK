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
    public static class Functions_Movement
    {
        static float maxMagnitude = 10.0f;

        public static void Push(ComponentMovement Move, Direction Direction, float Amount)
        {   //modify move component's magnitude based on direction by amount
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

        public static void ProjectMovement(ComponentMovement Move)
        {
            Push(Move, Move.direction, Move.speed);
            Move.moving = true; //assume moving to be true
            //apply friction to magnitude (this always makes magnitude smaller overtime)
            Move.magnitude = Move.magnitude * Move.friction;


            #region Clip Magnitude (min and max)

            //clip magnitude's minimum values
            if (Math.Abs(Move.magnitude.X) < 0.01f) { Move.magnitude.X = 0; }
            if (Math.Abs(Move.magnitude.Y) < 0.01f) { Move.magnitude.Y = 0; }
            //clip magnitude's maximum values
            if (Move.magnitude.X > maxMagnitude) { Move.magnitude.X = maxMagnitude; }
            else if (Move.magnitude.X < -maxMagnitude) { Move.magnitude.X = -maxMagnitude; }
            if (Move.magnitude.Y > maxMagnitude) { Move.magnitude.Y = maxMagnitude; }
            else if (Move.magnitude.Y < -maxMagnitude) { Move.magnitude.Y = -maxMagnitude; }

            #endregion


            //set moving boolean based on magnitude values
            if (Move.magnitude.X == 0 && Move.magnitude.Y == 0) { Move.moving = false; }
            //set newPosition based on current position + magnitude
            Move.newPosition.X = Move.position.X + Move.magnitude.X;
            Move.newPosition.Y = Move.position.Y + Move.magnitude.Y;
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

        public static Vector2 AlignToGrid(int X, int Y)
        {   //align to 16 pixel grid
            return new Vector2(16 * (X / 16) + 8, 16 * (Y / 16) + 8);
        }

        public static void RevertPosition(ComponentMovement compMove)
        {   //revert to previous position
            compMove.newPosition.X = compMove.position.X;
            compMove.newPosition.Y = compMove.position.Y;
        }

        public static Direction GetMovingDirection(ComponentMovement Move)
        {   //based on the Move component's magnitude, return the dominant cardinal direction
            if(Math.Abs(Move.magnitude.X) > Math.Abs(Move.magnitude.Y))
            {
                //horizontal is dominant axis
                if (Move.magnitude.X > 0) { return Direction.Right; }
                else if (Move.magnitude.X < 0) { return Direction.Left; }
                else { return Direction.None; }
            }
            else
            {
                //vertical is dominant axis
                if (Move.magnitude.Y > 0) { return Direction.Down; }
                else if (Move.magnitude.Y < 0) { return Direction.Up; }
                else { return Direction.None; } //default case
            }
        }
    }
}