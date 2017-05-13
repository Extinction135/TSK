﻿using System;
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
        public byte X;
        public byte Y;
        public Byte2(byte x, byte y)
        {
            X = x; Y = y;
        }
    }

    public struct Byte4
    {   //used for animation
        public byte X; //x frame
        public byte Y; //y frame
        public byte flipHori; //>0 = flip horizontally
        public byte flags; //represents various states (unused)
        public Byte4(byte x, byte y, byte Flip, byte Flags)
        {
            X = x; Y = y;
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
        public AnimationGroup death;
        public AnimationGroup heroDeath;

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
            collision.rec.Width = Size.X * 16;
            collision.rec.Height = Size.Y * 16;
            size = Size;
            center = new Point(Pos.X + (Size.X / 2) * 16, Pos.Y + (Size.Y / 2) * 16);
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
        public DungeonType type;
        public Dungeon(String Name)
        {   //initially, the map and key have not been found
            rooms = new List<Room>();
            name = Name;
            bigKey = false;
            map = false;
            type = DungeonType.CursedCastle;
        }
    }

    public struct SaveData
    {   //data that will be saved/loaded from game session to session
        public String name;
        public int gold;
        public byte heartPieces; //sets max health

        public byte magicCurrent; //current magic amount
        public byte magicMax; //max magic amount

        public byte bombsCurrent;
        public byte bombsMax;

        public Boolean itemBoomerang;
        //itemBomb

        public Boolean bottle1;
        public Boolean bottle2;
        public Boolean bottle3;
        public Boolean bottleHealth;
        public Boolean bottleMagic;
        public Boolean bottleFairy;

        public Boolean magicFireball;
        //portal

        public Boolean weaponBow;
        //bow
        //axe

        public Boolean armorPlatemail;
        //platemail
        //cape
        //robe

        public Boolean equipmentRing;
        //pearl
        //necklace
        //glove
        //pin

        public SaveData(String Name)
        {
            name = Name;
            gold = 99;
            heartPieces = 4 * 3; //player starts with 3 hearts

            magicCurrent = 3;
            magicMax = 3;

            bombsCurrent = 3;
            bombsMax = 99;

            //all items default to false
            itemBoomerang = false;

            bottle1 = false;
            bottle2 = false;
            bottle3 = false;
            bottleHealth = false;
            bottleMagic = false;
            bottleFairy = false;

            magicFireball = false;
            weaponBow = false;
            armorPlatemail = false;
            equipmentRing = false;
        }

    }

    public struct ColorScheme
    {
        public String name;
        public Color background;
        public Color overlay;
        public Color debugBkg;

        public Color collision;
        public Color interaction;

        public Color buttonUp;
        public Color buttonOver;
        public Color buttonDown;

        public Color windowBkg;
        public Color windowBorder;
        public Color windowInset;
        public Color windowInterior;

        public Color textLight;
        public Color textDark;

        public ColorScheme(String Name)
        {
            name = Name;
            background = new Color(100, 100, 100, 255);
            overlay = new Color(0, 0, 0, 255);
            debugBkg = new Color(0, 0, 0, 200);

            collision = new Color(100, 0, 0, 50);
            interaction = new Color(0, 100, 0, 50);

            buttonUp = new Color(44, 44, 44);
            buttonOver = new Color(66, 66, 66);
            buttonDown = new Color(100, 100, 100);

            windowBkg = new Color(0, 0, 0);
            windowBorder = new Color(210, 210, 210);
            windowInset = new Color(130, 130, 130);
            windowInterior = new Color(156, 156, 156);

            textLight = new Color(255, 255, 255);
            textDark = new Color(0, 0, 0);
        }
    }





    public class ComponentAmountDisplay
    {   //displays a 2 digit amount against a black background
        public ComponentText amount;
        public Rectangle bkg;
        public Boolean visible;
        public ComponentAmountDisplay(int Amount, int X, int Y)
        {
            amount = new ComponentText(Assets.font, "" + Amount,
                new Vector2(X, Y), Assets.colorScheme.textLight);
            bkg = new Rectangle(new Point(X, Y), new Point(9, 7));
            visible = true;
        }
    }





}