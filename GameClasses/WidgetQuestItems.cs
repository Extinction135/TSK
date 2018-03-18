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
    public class WidgetQuestItems : Widget
    {
        public List<MenuItem> menuItems;


        public WidgetQuestItems()
        {
            window = new MenuWindow(new Point(-100, -100),
                new Point(100, 100), "Quest Items");

            //create menuitems
            menuItems = new List<MenuItem>();
            for (i = 0; i < 4 * 4; i++)
            { menuItems.Add(new MenuItem()); }
        }

        public override void Reset(int X, int Y)
        {   //align this widgets component to Position + Size
            Functions_MenuWindow.ResetAndMove(window, X, Y, 
                new Point(16 * 6 + 8, 16 * 8 + 8), "Quest Items");
            Functions_MenuItem.PlaceMenuItems(menuItems, 
                window.background.position.X + 16, 
                window.background.position.Y + 16 * 2, 
                4);
        }

        public override void Update()
        {
            Functions_MenuWindow.Update(window);
        }

        public override void Draw()
        {
            Functions_Draw.Draw(window);
            if (window.interior.displayState == DisplayState.Opened)
            {
                for (i = 0; i < menuItems.Count; i++)
                { Functions_Draw.Draw(menuItems[i].compSprite); }
            }
        }

    }
}