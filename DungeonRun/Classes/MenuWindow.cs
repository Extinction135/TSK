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
    public class MenuWindow
    {

        public Point size;
        public int animationCounter = 0;        //counts up to delay value
        public int openDelay = 0;               //how many updates are ignored before open occurs

        public MenuRectangle background; 
        public MenuRectangle border; 
        public MenuRectangle inset;
        public MenuRectangle interior; 

        public ComponentText title;
        public MenuRectangle headerLine;
        public MenuRectangle footerLine;

        public MenuWindow(Point Position, Point Size, String Title)
        {
            size = Size;
            //create the window components
            background = new MenuRectangle(new Point(0,0), new Point(0,0), Assets.colorScheme.windowBkg);
            border = new MenuRectangle(new Point(0,0), new Point(0,0), Assets.colorScheme.windowBorder);
            inset = new MenuRectangle(new Point(0,0), new Point(0,0), Assets.colorScheme.windowInset);
            interior = new MenuRectangle(new Point(0,0), new Point(0,0), Assets.colorScheme.windowInterior);
            headerLine = new MenuRectangle(new Point(0,0), new Point(0,0), Assets.colorScheme.windowInset);
            footerLine = new MenuRectangle(new Point(0,0), new Point(0,0), Assets.colorScheme.windowInset);
            title = new ComponentText(Assets.font, "", new Vector2(0,0), Assets.colorScheme.textDark);
            //align all the window components
            ResetAndMoveWindow(Position, Size, Title);
            //set the openDelay to cascade in all the components
            background.openDelay = 0;
            border.openDelay = 2;
            inset.openDelay = 2;
            interior.openDelay = 8;
            headerLine.openDelay = 12;
            footerLine.openDelay = 12;
        }

        public void Update()
        {   //count up to the openDelay value, then begin updating the menu rectangles
            if (animationCounter < openDelay) { animationCounter += 1; }
            if (animationCounter >= openDelay)
            {
                background.Update();
                border.Update();
                inset.Update();
                interior.Update();
                headerLine.Update();
                footerLine.Update();
            }
        }

        public void ResetAndMoveWindow(Point Position, Point Size, String Title)
        {
            size = Size;
            //set the new title, move into position
            title.text = Title;
            title.position.X = Position.X + 8;
            title.position.Y = Position.Y + 2;


            #region Reset all the MenuRectangles, and update them to the passed Position + Size

            background.position.X = Position.X + 0;
            background.position.Y = Position.Y + 0;
            background.size.X = Size.X + 0;
            background.size.Y = Size.Y + 0;
            background.Reset();

            border.position.X = Position.X + 1;
            border.position.Y = Position.Y + 1;
            border.size.X = Size.X - 2;
            border.size.Y = Size.Y - 2;
            border.Reset();

            inset.position.X = Position.X + 2;
            inset.position.Y = Position.Y + 2;
            inset.size.X = Size.X - 4;
            inset.size.Y = Size.Y - 4;
            inset.Reset();

            interior.position.X = Position.X + 3;
            interior.position.Y = Position.Y + 3;
            interior.size.X = Size.X - 6;
            interior.size.Y = Size.Y - 6;
            interior.Reset();

            #endregion


            #region Reset the header and footer lines, update with Position + Size

            headerLine.position.X = Position.X + 8;
            headerLine.position.Y = Position.Y + 16;
            headerLine.size.X = Size.X - 16;
            headerLine.size.Y = 1;
            headerLine.Reset();

            footerLine.position.X = Position.X + 8;
            footerLine.position.Y = Position.Y + Size.Y - 16;
            footerLine.size.X = Size.X - 16;
            footerLine.size.Y = 1;
            footerLine.Reset();

            #endregion
            

        }

    }
}