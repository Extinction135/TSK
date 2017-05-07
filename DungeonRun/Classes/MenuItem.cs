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
        public ComponentAnimation compAnim;
        public MenuItemType type;
        public String name = "";
        public String description = "";
        public Byte price = 0;
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
                new Point(16, 16));
            compAnim = new ComponentAnimation();
            MenuItemFunctions.SetMenuItemData(MenuItemType.Unknown, this);
            neighborUp = this;
            neighborDown = this;
            neighborLeft = this;
            neighborRight = this;
        }

    }
}