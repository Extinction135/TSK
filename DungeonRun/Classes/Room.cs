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
    public class Room
    {
        public enum Type
        {
            Standard,
            Boss,
        }
        public Type type;

        public Rectangle rectangle; //represents the position and bounds of the room
        //rectangles are always multiples of 16 pixels, because floor tiles are 16 pixels



        public Room (int X, int Y, int Width, int Height, Room.Type Type)
        {
            rectangle = new Rectangle(X, Y, Width, Height);
            type = Type;
        }
    }
}