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

    public enum RoomType
    {
        Normal,
        Key,
        Boss,
        Hub,
        Exit,
        Secret
    }

    public struct Room
    {
        public Point position;
        public Byte2 size;
        public Point center;
        public RoomType type;
        public byte enemyCount;
    }

    public struct Dungeon
    {
        //a list of rooms
        //a timespan
        //a string name
    }
}
