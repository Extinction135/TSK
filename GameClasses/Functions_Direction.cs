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

        public static Direction GetRelativeDirection(GameObject Obj, Actor Actor)
        {   //return the direction from obj to actor
            if (Actor.compSprite.position.Y > Obj.compSprite.position.Y)
            {   //actor below obj, return down right or down left
                if (Actor.compSprite.position.X > Obj.compSprite.position.X)
                { return Direction.DownRight; } else { return Direction.DownLeft; }
            }
            else
            {   //actor above obj, return up right or up left
                if (Actor.compSprite.position.X > Obj.compSprite.position.X)
                { return Direction.UpRight; } else { return Direction.UpLeft; }
            }
        }

    }
}