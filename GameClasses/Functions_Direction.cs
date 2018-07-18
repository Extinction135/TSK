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
    public static class Functions_Direction
    {
        static int xDistance;
        static int yDistance;
        static int randomDir = 0;
        static Direction dir = Direction.None;

        public static Direction GetCardinalDirection(Direction Direction)
        {   //converts a diagonal direction to a cardinal direction
            if (Direction == Direction.UpRight) { return Direction.Right; }
            else if (Direction == Direction.DownRight) { return Direction.Right; }
            else if (Direction == Direction.UpLeft) { return Direction.Left; }
            else if (Direction == Direction.DownLeft) { return Direction.Left; }
            else { return Direction; } //Direction parameter is already Cardinal
        }

        public static Direction GetOppositeDirection(Direction Direction)
        {   //returns opposite direction of direction
            if (Direction == Direction.Down) { return Direction.Up; }
            else if (Direction == Direction.Up) { return Direction.Down; }
            else if (Direction == Direction.Left) { return Direction.Right; }
            else if (Direction == Direction.UpLeft) { return Direction.DownRight; }
            else if (Direction == Direction.DownLeft) { return Direction.UpRight; }
            else if (Direction == Direction.Right) { return Direction.Left; }
            else if (Direction == Direction.DownRight) { return Direction.UpLeft; }
            else if (Direction == Direction.UpRight) { return Direction.DownLeft; }
            return Direction.None;
        }

        public static Direction GetOppositeCardinal(Vector2 PosA, Vector2 PosB)
        {   //figure out which axis is the dominant axis, return opposite direction along dominant axis
            if (Math.Abs(PosB.Y - PosA.Y) > Math.Abs(PosB.X - PosA.X))
            {   //Y axis dominant
                if (PosB.Y > PosA.Y) { return Direction.Up; } else { return Direction.Down; }
            }
            else
            {   //X axis dominant
                if (PosB.X > PosA.X) { return Direction.Left; } else { return Direction.Right; }
            }
        }

        public static Direction GetOppositeDiagonal(Vector2 PosA, Vector2 PosB)
        {   //return the direction from PosB to PosA
            if (PosB.Y > PosA.Y)
            {   //upleft or upright
                if (PosB.X > PosA.X)
                { return Direction.UpLeft; }
                else { return Direction.UpRight; }
            }
            else
            {   //downleft or downright
                if (PosB.X > PosA.X)
                { return Direction.DownLeft; }
                else { return Direction.DownRight; }
            }
        }

        public static Direction GetCardinalDirectionToHero(Vector2 Pos)
        {   //get the x and y distances between starting position and hero
            xDistance = (int)Math.Abs(Pool.hero.compSprite.position.X - Pos.X);
            yDistance = (int)Math.Abs(Pool.hero.compSprite.position.Y - Pos.Y);
            //return the cardinal direction to hero
            if (xDistance < yDistance)
            {
                if (Pool.hero.compSprite.position.Y > Pos.Y)
                { return Direction.Down; }
                else { return Direction.Up; }
            }
            else
            {
                if (Pool.hero.compSprite.position.X > Pos.X)
                { return Direction.Right; }
                else { return Direction.Left; }
            }
        }

        public static Direction GetDiagonalToHero(Vector2 Pos)
        {   //get the x and y distances between starting position and hero
            xDistance = (int)Math.Abs(Pool.hero.compSprite.position.X - Pos.X);
            yDistance = (int)Math.Abs(Pool.hero.compSprite.position.Y - Pos.Y);
            //return a NSEW or DIAG direction towards hero
            if (Pool.hero.compSprite.position.Y > Pos.Y)
            {   //down left or down right
                if (Pool.hero.compSprite.position.X > Pos.X)
                { return Direction.DownRight; }
                else { return Direction.DownLeft; }
            }
            else
            {   //up left or up right
                if (Pool.hero.compSprite.position.X > Pos.X)
                { return Direction.UpRight; }
                else { return Direction.UpLeft; }
            }
        }

        public static Direction GetRandomCardinal()
        {
            randomDir = Functions_Random.Int(0, 4);
            if (randomDir == 0) { dir = Direction.Up; }
            else if (randomDir == 1) { dir = Direction.Right; }
            else if (randomDir == 2) { dir = Direction.Down; }
            else if (randomDir == 3) { dir = Direction.Left; }
            return dir;
        }

        public static Direction GetRandomDirection()
        {
            randomDir = Functions_Random.Int(0, 8);
            if (randomDir == 0) { dir = Direction.Up; }
            else if (randomDir == 1) { dir = Direction.UpRight; }
            else if (randomDir == 2) { dir = Direction.Right; }
            else if (randomDir == 3) { dir = Direction.DownRight; }
            else if (randomDir == 4) { dir = Direction.Down; }
            else if (randomDir == 5) { dir = Direction.DownLeft; }
            else if (randomDir == 6) { dir = Direction.Left; }
            else if (randomDir == 7) { dir = Direction.UpLeft; }
            return dir;
        }

        public static Direction GetDiagonalToCenterOfRoom(Vector2 Pos)
        {   //return a NSEW or DIAG direction towards room center
            if (LevelSet.currentLevel.currentRoom.center.Y > Pos.Y)
            {   //down left or down right
                if (LevelSet.currentLevel.currentRoom.center.X > Pos.X)
                { return Direction.DownRight; }
                else { return Direction.DownLeft; }
            }
            else
            {   //up left or up right
                if (LevelSet.currentLevel.currentRoom.center.X > Pos.X)
                { return Direction.UpRight; }
                else { return Direction.UpLeft; }
            }
        }


    }
}