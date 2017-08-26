﻿using System;
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


        public static Direction GetCardinalDirection(Direction Direction)
        {   //converts a diagonal direction to a cardinal direction
            if (Direction == Direction.UpRight) { return Direction.Right; }
            else if (Direction == Direction.DownRight) { return Direction.Right; }
            else if (Direction == Direction.UpLeft) { return Direction.Left; }
            else if (Direction == Direction.DownLeft) { return Direction.Left; }
            else { return Direction; } //Direction parameter is already Cardinal
        }

        public static Direction GetOppositeDirection(Direction Direction)
        {
            if (Direction == Direction.Down) { return Direction.Up; }
            else if (Direction == Direction.Up) { return Direction.Down; }

            else if (Direction == Direction.Left) { return Direction.Right; }
            else if (Direction == Direction.UpLeft) { return Direction.Right; }
            else if (Direction == Direction.DownLeft) { return Direction.Right; }

            else if (Direction == Direction.Right) { return Direction.Left; }
            else if (Direction == Direction.DownRight) { return Direction.Left; }
            else if (Direction == Direction.UpRight) { return Direction.Left; }

            return Direction.None;
        }

        public static Direction GetRelativeDirection(Vector2 PosA, Vector2 PosB)
        {   //return the direction from PosB to PosA
            if (PosB.Y > PosA.Y)
            {   //actor below obj, return down right or down left
                if (PosB.X > PosA.X)
                { return Direction.DownRight; }
                else { return Direction.DownLeft; }
            }
            else
            {   //actor above obj, return up right or up left
                if (PosB.X > PosA.X)
                { return Direction.UpRight; }
                else { return Direction.UpLeft; }
            }
        }

        public static Direction GetDirectionToHero(Vector2 Pos)
        {   //get the x and y distances between starting position and hero
            xDistance = (int)Math.Abs(Pool.hero.compSprite.position.X - Pos.X);
            yDistance = (int)Math.Abs(Pool.hero.compSprite.position.Y - Pos.Y);
            //determine the axis hero is closest on
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

        public static Direction GetRandomCardinal()
        {
            if (Functions_Random.Int(0, 100) > 50)
            {
                if (Functions_Random.Int(0, 100) > 50)
                { return Direction.Up; } else { return Direction.Down; }
            }
            else
            {
                if (Functions_Random.Int(0, 100) > 50)
                { return Direction.Left; } else { return Direction.Right; }
            }
        }

    }
}