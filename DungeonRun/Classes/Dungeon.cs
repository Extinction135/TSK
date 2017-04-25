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

    public struct Room
    {
        public ComponentCollision collision;
        public Byte2 size;
        public Point center;
        public RoomType type;
        public byte enemyCount;
        public int id;
        public Room(Point Pos, Byte2 Size, RoomType Type, byte EnemyCount, int ID)
        {
            collision = new ComponentCollision();
            collision.rec.X = Pos.X;
            collision.rec.Y = Pos.Y;
            collision.rec.Width = Size.x * 16;
            collision.rec.Height = Size.y * 16;
            size = Size;
            center = new Point(Pos.X + (Size.x / 2) * 16, Pos.Y + (Size.y / 2) * 16);
            type = Type;
            enemyCount = EnemyCount;
            id = ID;
        }
    }

    public struct Dungeon
    {
        public List<Room> rooms;
        public String name;
        public Boolean bigKey;
        public Dungeon(String Name)
        {
            rooms = new List<Room>();
            name = Name;
            bigKey = false; //hero has not found the big key yet
        }
    }
}
