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
    public class CollisionComponent
    {
        public Rectangle rec; //the rectangle used for checking collisions (using rec.Contains() and rec.Intersects())
        public Byte2 offset; //offsets rec from sprite.position
        public Boolean blocking; //is this actor/obj impassable
    }
}