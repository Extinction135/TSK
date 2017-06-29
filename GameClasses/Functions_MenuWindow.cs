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
    public static class Functions_MenuWindow
    {
        static int i;

        public static void Update(MenuWindow Window)
        {
            Functions_MenuRectangle.Update(Window.background);
            Functions_MenuRectangle.Update(Window.border);
            Functions_MenuRectangle.Update(Window.inset);
            Functions_MenuRectangle.Update(Window.interior);
            //update all window lines
            for (i = 0; i < Window.lines.Count; i++)
            { Functions_MenuRectangle.Update(Window.lines[i]); }
        }

        public static void Close(MenuWindow Window)
        {
            Window.background.displayState = DisplayState.Closing;
            Window.border.displayState = DisplayState.Closing;
            Window.inset.displayState = DisplayState.Closing;
            Window.interior.displayState = DisplayState.Closing;
            //close all window lines
            for (i = 0; i < Window.lines.Count; i++)
            { Window.lines[i].displayState = DisplayState.Closing; }
        }

        public static void ResetAndMove(MenuWindow Window, int X, int Y, Point Size, String Title)
        {
            Window.size = Size;
            //set the new title, move into position
            Window.title.text = Title;
            Window.title.position.X = X + 8;
            Window.title.position.Y = Y + 2;


            #region Reset all the MenuRectangles, and update them to the passed Position + Size

            Window.background.position.X = X + 0;
            Window.background.position.Y = Y + 0;
            Window.background.size.X = Size.X + 0;
            Window.background.size.Y = Size.Y + 0;
            Functions_MenuRectangle.Reset(Window.background);

            Window.border.position.X = X + 1;
            Window.border.position.Y = Y + 1;
            Window.border.size.X = Size.X - 2;
            Window.border.size.Y = Size.Y - 2;
            Functions_MenuRectangle.Reset(Window.border);

            Window.inset.position.X = X + 2;
            Window.inset.position.Y = Y + 2;
            Window.inset.size.X = Size.X - 4;
            Window.inset.size.Y = Size.Y - 4;
            Functions_MenuRectangle.Reset(Window.inset);

            Window.interior.position.X = X + 3;
            Window.interior.position.Y = Y + 3;
            Window.interior.size.X = Size.X - 6;
            Window.interior.size.Y = Size.Y - 6;
            Functions_MenuRectangle.Reset(Window.interior);

            #endregion


            #region Reset the header and footer lines, update with Position + Size

            //set header and footer Y positions
            Window.lines[0].position.Y = Y + 16;
            Window.lines[1].position.Y = Y + Size.Y - 16;
            //reset all line's horizontal position & width
            for (i = 0; i < Window.lines.Count; i++)
            {
                Window.lines[i].openDelay = Window.lines[0].openDelay;
                Window.lines[i].position.X = X + 8;
                Window.lines[i].size.X = Size.X - 16;
                Window.lines[i].size.Y = 1;
                Functions_MenuRectangle.Reset(Window.lines[i]);
            }

            #endregion

        }
    }
}