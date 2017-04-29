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

    public struct Byte2
    {
        public byte x;
        public byte y;
        public Byte2(byte X, byte Y)
        {
            x = X; y = Y;
        }
    }

    public struct Byte4
    {   //used for animation
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

    public struct AnimationGroup
    {   //represents an animation with Down, Up, Left, Right states
        public List<Byte4> down;
        public List<Byte4> up;
        public List<Byte4> right;
        public List<Byte4> left;
    }

    public struct ActorAnimationList
    {
        public AnimationGroup idle;
        public AnimationGroup move;
        public AnimationGroup dash;
        public AnimationGroup interact;

        public AnimationGroup attack;
        public AnimationGroup hit;
        public AnimationGroup dead;
        public AnimationGroup reward;

        //pickup, hold, carry, drag, etc...
    }

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
        public Boolean map;
        public Dungeon(String Name)
        {   //initially, the map and key have not been found
            rooms = new List<Room>();
            name = Name;
            bigKey = false;
            map = false;
        }
    }

    public struct SaveData
    {   //data that will be saved/loaded from game session to session
        public int gold;
        public byte heartPieces; //sets max health
    }

}