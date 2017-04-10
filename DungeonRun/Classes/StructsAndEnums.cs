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

    public static class Global
    {
        public static Random Random = new Random();
    }
    


    public struct Byte2
    {
        public byte x;
        public byte y;
        public Byte2(byte X, byte Y)
        {
            x = X; y = Y;
        }
    }

    public struct Byte4 //used for animation
    {
        public byte x; //x frame
        public byte y; //y frame
        public byte flipHori; //>0 = flip horizontally
        public byte flags; //represents various states (unused)
        public Byte4(byte X, byte Y, byte Flip, byte Flags)
        {
            x = X; y = Y;
            flipHori = Flip;
            flags = Flags;
        }
    }

    public struct ColorScheme
    {
        public Color background;
        public Color textSmall;
        public Color windowBkg;

        public Color collisionActor;
        public Color collisionObj;
    }


    

    public enum Direction { Up, UpRight, Right, DownRight, Down, DownLeft, Left, UpLeft, None }

    public enum Rotation { None, Clockwise90, Clockwise180, Clockwise270 }

    public enum MouseButtons { LeftButton, RightButton }

}