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
    public class GameObject
    {
        public enum Type
        {
            Wall,
        }
        public Type type;



        public ComponentSprite compSprite;
        public ComponentCollision compCollision;



        public void Update()
        {
            if (type == Type.Wall) { }
            //etc...
        }
    }
}