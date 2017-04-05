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
        public Rectangle rec = new Rectangle(100, 100, 16, 16); //the rectangle used for checking collisions (using rec.Contains() and rec.Intersects())
        public int offsetX = 0; //offsets rec from sprite.position
        public int offsetY = 0; //offsets rec from sprite.position
        public Boolean blocking = true; //is this actor/obj impassable

        public ComponentCollision(int X, int Y, int Width, int Height, Boolean Blocking)
        {
            rec = new Rectangle(X, Y, Width, Height);
            offsetX = 0; offsetY = 0;
            blocking = Blocking;
        }
    }
}