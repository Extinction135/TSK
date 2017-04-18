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
        public Room(Point Pos, Byte2 Size, RoomType Type, byte EnemyCount)
        {
            position = Pos;
            size = Size;
            center = new Point(Pos.X + (Size.x / 2) * 16, Pos.Y + (Size.y / 2) * 16);
            type = Type;
            enemyCount = EnemyCount;
        }
    }

    public struct Dungeon
    {
        public List<Room> rooms;
        public String name;
        public Dungeon(String Name)
        {
            rooms = new List<Room>();
            name = Name;
        }
    }
}
