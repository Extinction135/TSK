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
    public static class MovementFunctions
    {

        public static void Move(Actor Actor, DungeonScreen DungeonScreen)
        {
            ProjectMovement(Actor.compMove);
            CollisionFunctions.CheckCollisions(Actor.compMove, Actor.compCollision, DungeonScreen);
            CollisionFunctions.AlignComponents(Actor.compMove, Actor.compSprite, Actor.compCollision);
        }

        public static void Move(GameObject Obj, DungeonScreen DungeonScreen)
        {
            ProjectMovement(Obj.compMove);
            CollisionFunctions.CheckCollisions(Obj.compMove, Obj.compCollision, DungeonScreen);
            CollisionFunctions.AlignComponents(Obj.compMove, Obj.compSprite, Obj.compCollision);
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

        public static void Teleport(ComponentMovement Move, float X, float Y)
        {
            Move.position.X = X;
            Move.position.Y = Y;
        }

    }
}