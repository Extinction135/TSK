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
    public class MenuItem
    {

        public ComponentSprite compSprite;
        public String name = "unknown";
        public String description = "no description available for this item.";
        public Boolean selected = false;

        //the cardinal neighbors this menuItem links with
        public MenuItem neighborUp;
        public MenuItem neighborDown;
        public MenuItem neighborLeft;
        public MenuItem neighborRight;

        public MenuItem()
        {   //default to ? sprite, hidden offscreen
            compSprite = new ComponentSprite(Assets.mainSheet, 
                new Vector2(-100, 1000), 
                new Byte4(15, 5, 0, 0), 
                new Byte2(16, 16));
            neighborUp = this;
            neighborDown = this;
            neighborLeft = this;
            neighborRight = this;
        }

    }
}