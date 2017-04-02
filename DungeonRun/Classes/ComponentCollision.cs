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
    public class ComponentCollision
    {
        public Rectangle rec; //the rectangle used for checking collisions (using rec.Contains() and rec.Intersects())
        public int offsetX; //offsets rec from sprite.position
        public int offsetY; //offsets rec from sprite.position
        public Boolean blocking; //is this actor/obj impassable

        public ComponentCollision(int X, int Y, int Width, int Height, Boolean Blocking)
        {
            rec = new Rectangle(X, Y, Width, Height);
            offsetX = 0; offsetY = 0;
            blocking = Blocking;
        }
    }
}