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
    public static class Functions_MenuRectangle
    {

        public static void Update(MenuRectangle MenuRec)
        {
            if (MenuRec.displayState == DisplayState.Opening)
            {
                if (MenuRec.animationCounter < MenuRec.openDelay) { MenuRec.animationCounter += 1; }
                if (MenuRec.animationCounter >= MenuRec.openDelay)
                {   //grow right
                    MenuRec.rec.Height = MenuRec.size.Y;
                    if (MenuRec.rec.Width < MenuRec.size.X)
                    { MenuRec.rec.Width += ((MenuRec.size.X - MenuRec.rec.Width) / MenuRec.animationSpeed) + 1; } //easeIn 
                    if (MenuRec.rec.Width > MenuRec.size.X)
                    { MenuRec.rec.Width = MenuRec.size.X; }
                    if (MenuRec.rec.Width == MenuRec.size.X)
                    { MenuRec.displayState = DisplayState.Opened; MenuRec.animationCounter = 0; } //open complete
                }
            }
        }

        public static void Reset(MenuRectangle MenuRec)
        {
            MenuRec.rec.Width = 0;
            MenuRec.rec.Height = 0;
            MenuRec.rec.Location = MenuRec.position;
            MenuRec.animationCounter = 0;
            MenuRec.displayState = DisplayState.Opening;
        }

    }
}