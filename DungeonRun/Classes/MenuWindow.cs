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

        public MenuWindow(Point Position, Point Size, String Title)
        {
            size = Size;

            background = new MenuRectangle(
                new Point(Position.X + 0, Position.Y + 0), 
                new Point(size.X - 0, size.Y - 0), 
                Assets.colorScheme.windowBkg);
            border = new MenuRectangle(
                new Point(Position.X + 1, Position.Y + 1), 
                new Point(size.X - 2, size.Y - 2),
                Assets.colorScheme.windowBorder);
            inset = new MenuRectangle(
                new Point(Position.X + 2, Position.Y + 2), 
                new Point(size.X - 4, size.Y - 4),
                Assets.colorScheme.windowInset);
            interior = new MenuRectangle(
                new Point(Position.X + 3, Position.Y + 3), 
                new Point(size.X - 6, size.Y - 6),
                Assets.colorScheme.windowInterior);

            background.openDelay = 0;
            border.openDelay = 2;
            inset.openDelay = 2;
            interior.openDelay = 8;

            title = new ComponentText(
                Assets.font, Title, 
                new Vector2(Position.X + 8, Position.Y + 2), 
                Assets.colorScheme.textSmall);

            headerLine = new MenuRectangle(
                new Point(Position.X + 8, Position.Y + 16),
                new Point(size.X - 16, 1),
                Assets.colorScheme.windowInset);
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
            }
        }

    }
}