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
    public static class Functions_Dungeon
    {

        public static ScreenDungeon dungeonScreen;
        public static Dungeon dungeon;
        public static Room currentRoom; //points to a room on the dungeon's room list
        public static Stopwatch stopWatch = new Stopwatch();
        public static TimeSpan time;



        public static void Initialize(ScreenDungeon DungeonScreen) { dungeonScreen = DungeonScreen; }

        public static void BuildDungeon(DungeonType Type)
        {
            //create a new dungeon
            dungeon = new Dungeon();
            dungeon.type = Type;

            if (Type == DungeonType.Shop)
            {   //set the objPool texture
                Functions_Pool.SetDungeonTexture(Assets.shopSheet);
                //create the shop room
                dungeon.rooms.Add(new Room(new Point(16 * 10, 16 * 21), new Byte2(20, 10), RoomType.Shop, 10, 0));

                //keep the title music playing
                Functions_Music.PlayMusic(Music.Title);
            }
            else
            {   //set the objPool texture
                Functions_Pool.SetDungeonTexture(Assets.cursedCastleSheet);
                //populate the dungeon with rooms
                dungeon.rooms.Add(new Room(new Point(16 * 10, 16 * 21), new Byte2(20, 10), RoomType.Exit, 10, 0));
                dungeon.rooms.Add(new Room(new Point(16 * 10, 16 * 10), new Byte2(20, 10), RoomType.Boss, 10, 1));

                //cycle thru dungeon tracks
                if (Functions_Music.currentMusic == Assets.musicDungeonA)
                { Functions_Music.PlayMusic(Music.DungeonB); }
                else if (Functions_Music.currentMusic == Assets.musicDungeonB)
                { Functions_Music.PlayMusic(Music.DungeonC); }
                else if (Functions_Music.currentMusic == Assets.musicDungeonC)
                { Functions_Music.PlayMusic(Music.DungeonA); }
            }

            //build the first room in the dungeon (the spawn room)
            Functions_Room.BuildRoom(dungeon.rooms[0]);
            currentRoom = dungeon.rooms[0];

            //reset the dungeon screen's dungeon record, passing dungeonID
            DungeonRecord.Reset();
            DungeonRecord.dungeonID = 0; //ID = 0 for now
            DungeonRecord.timer.Start(); //start the record timer

            //fade the dungeon screen out from black, revealing the new level
            dungeonScreen.overlayAlpha = 1.0f;
            dungeonScreen.displayState = DisplayState.Opening;
        }
        
    }
}