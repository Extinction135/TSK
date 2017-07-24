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
    public static class Functions_Dungeon
    {

        public static ScreenDungeon dungeonScreen;
        public static Dungeon dungeon = new Dungeon(); //the last dungeon created
        public static DungeonType dungeonType = DungeonType.CursedCastle; //changed later
        public static Room currentRoom; //points to a room on dungeon's roomList
        public static int dungeonTrack = 0;


        public static void Initialize(ScreenDungeon DungeonScreen) { dungeonScreen = DungeonScreen; }

        public static void BuildDungeon()
        {
            dungeon = new Dungeon(); //DungeonScreen calls BuildDungeon()
            dungeon.type = dungeonType; //dungeonType is set by OverworldScreen

            //set all floor sprites to the appropriate dungeon texture
            if (dungeon.type == DungeonType.CursedCastle)
            { Functions_Pool.SetFloorTexture(Assets.cursedCastleSheet); }
            //expand this to include all dungeon textures...
            else if (dungeon.type == DungeonType.Shop)
            { Functions_Pool.SetFloorTexture(Assets.shopSheet); }


            #region Create Shop

            if (dungeonType == DungeonType.Shop)
            {   //create the shop room
                dungeon.rooms.Add(new Room(new Point(16 * 10, 16 * 21), RoomType.Shop, 0));
                //keep the title music playing
                Functions_Music.PlayMusic(Music.Title);
            }

            #endregion


            #region Create Basic Testing Dungeon - no randomness YET

            else
            {
                if (Flags.PrintOutput) { Debug.WriteLine("-- creating dungeon --"); }

                //create the dungeon's rooms
                Room exitRoom = new Room(new Point(0, 0), RoomType.Exit, 0);
                Room hubRoom = new Room(new Point(0, 0), RoomType.Hub, 1);
                Room bossRoom = new Room(new Point(0, 0), RoomType.Boss, 2);
                Room keyRoom = new Room(new Point(0, 0), RoomType.Key, 3);
                Room columnRoom = new Room(new Point(0, 0), RoomType.Column, 4);
                Room rowRoom = new Room(new Point(0, 0), RoomType.Row, 5);
                Room squareRoom = new Room(new Point(0, 0), RoomType.Square, 6);

                //place/move the rooms (relative to each other)
                Functions_Room.MoveRoom(exitRoom, 16 * 10, 16 * 200);
                Functions_Room.MoveRoom(columnRoom, 16 * 10, exitRoom.collision.rec.Y - (16 * columnRoom.size.Y) - 16);
                Functions_Room.MoveRoom(rowRoom, 16 * 10, columnRoom.collision.rec.Y - (16 * rowRoom.size.Y) - 16);
                Functions_Room.MoveRoom(squareRoom, 16 * 10, rowRoom.collision.rec.Y - (16 * squareRoom.size.Y) - 16);
                Functions_Room.MoveRoom(hubRoom, 16 * 10, squareRoom.collision.rec.Y - (16 * hubRoom.size.Y) - 16);
                Functions_Room.MoveRoom(bossRoom, 16 * 10, hubRoom.collision.rec.Y - (16 * bossRoom.size.Y) - 16);
                Functions_Room.MoveRoom(keyRoom, hubRoom.collision.rec.X - (16 * keyRoom.size.X) - 16, hubRoom.collision.rec.Y);
                
                //add rooms to the rooms list
                dungeon.rooms.Add(exitRoom); //exit room must be at index0
                dungeon.rooms.Add(hubRoom);
                dungeon.rooms.Add(bossRoom);
                dungeon.rooms.Add(keyRoom);
                dungeon.rooms.Add(columnRoom);
                dungeon.rooms.Add(rowRoom);
                dungeon.rooms.Add(squareRoom);

                //create the door location points
                List<Room> buildList = new List<Room>();
                //add boss room first, followed by hub room
                buildList.Add(bossRoom); //the boss door pos should be
                buildList.Add(hubRoom); //at index 0 of doorLocations
                //then add whatever other rooms exist in dungeon
                buildList.Add(exitRoom);
                buildList.Add(keyRoom);
                buildList.Add(columnRoom);
                buildList.Add(rowRoom);
                buildList.Add(squareRoom);


                if (Flags.PrintOutput) { Debug.WriteLine("connecting rooms..."); }
                while (buildList.Count() > 0)
                {   //check first room against remaining rooms
                    for (int i = 1; i < buildList.Count(); i++)
                    {   //if the two rooms are nearby

                        //Debug.WriteLine("rooms nearby: " + RoomsNearby(buildList[0], buildList[i]));
                        //Debug.WriteLine("parent: " + buildList[0].type);
                        //Debug.WriteLine("child: " + buildList[i].type);

                        if (RoomsNearby(buildList[0], buildList[i]))
                        {   //get door locations between these two rooms
                            GetDoorLocations(buildList[0], buildList[i]);
                        }
                    }
                    buildList.RemoveAt(0); //remove first room
                }
                if (Flags.PrintOutput)
                {
                    Debug.WriteLine("connected " + dungeon.rooms.Count + " rooms");
                    Debug.WriteLine("created  " + dungeon.doorLocations.Count + " doors");
                }

                //dump door locations
                //Debug.WriteLine("doorlocations");
                //for (int i = 0; i < dungeon.doorLocations.Count; i++)
                //{ Debug.WriteLine("" + dungeon.doorLocations[i]); }

                //choose the dungeon track
                if (dungeonTrack == 0) { Functions_Music.PlayMusic(Music.DungeonA); }
                else if (dungeonTrack == 1) { Functions_Music.PlayMusic(Music.DungeonB); }
                else if (dungeonTrack == 2) { Functions_Music.PlayMusic(Music.DungeonC); }
                //cycle thru dungeon tracks
                dungeonTrack++;
                if (dungeonTrack > 2) { dungeonTrack = 0; }
            }

            #endregion


            //build the first room in the dungeon (room with exit)
            Functions_Room.BuildRoom(dungeon.rooms[0]);
            Functions_Room.FinishRoom(dungeon.rooms[0]);
            currentRoom = dungeon.rooms[0];

            //place hero in the current room (exit room, rooms[0]) in front of exit door
            Functions_Actor.SetType(Pool.hero, ActorType.Hero);
            Functions_Movement.Teleport(Pool.hero.compMove,
                (currentRoom.size.X / 2) * 16 + currentRoom.collision.rec.X + 8,
                currentRoom.collision.rec.Y + (currentRoom.size.Y - 1) * 16);
            Functions_Movement.StopMovement(Pool.hero.compMove);
            Pool.hero.direction = Direction.Up; //face hero up

            //place cameras starting position in dungeon
            if (Flags.CameraTracksHero) //center camera to hero
            { Camera2D.targetPosition = Pool.hero.compMove.newPosition; }
            else
            {   //center hero to current room
                Camera2D.targetPosition.X = currentRoom.center.X;
                Camera2D.targetPosition.Y = currentRoom.center.Y;
            }
            //teleport camera to targetPos
            Camera2D.currentPosition = Camera2D.targetPosition;

            //reset the dungeon screen's dungeon record, passing dungeonID
            DungeonRecord.Reset();
            DungeonRecord.dungeonID = 0; //ID = 0 for now
            DungeonRecord.timer.Start(); //start the record timer

            //fade the dungeon screen out from black, revealing the new level
            dungeonScreen.overlay.alpha = 1.0f;
            dungeonScreen.displayState = DisplayState.Opening;
        }

        static Rectangle compRec = new Rectangle(0, 0, 0, 0);
        static Boolean RoomsNearby(Room Parent, Room Child)
        {   //place & size comparisonRec to be 2 cells larger than parent
            compRec.X = Parent.collision.rec.X - 32;
            compRec.Y = Parent.collision.rec.Y - 32;
            compRec.Width = Parent.collision.rec.Width + 64;
            compRec.Height = Parent.collision.rec.Height + 64;
            //ensure that compRec intersets child room
            if (compRec.Intersects(Child.collision.rec))
            { return true; } else { return false; }
        }

        public static void GetDoorLocations(Room Parent, Room Child)
        {   //determine the direction child is located relative to parent
            compRec.Width = Parent.collision.rec.Width;
            compRec.Height = Parent.collision.rec.Height;

            //check left
            compRec.X = Parent.collision.rec.X - 32;
            compRec.Y = Parent.collision.rec.Y;
            if (compRec.Intersects(Child.collision.rec))
            { Poke(Direction.Left, Parent, Child); return; }

            //check right
            compRec.X = Parent.collision.rec.X + 32;
            compRec.Y = Parent.collision.rec.Y;
            if (compRec.Intersects(Child.collision.rec))
            { Poke(Direction.Right, Parent, Child); return; }

            //check up
            compRec.X = Parent.collision.rec.X;
            compRec.Y = Parent.collision.rec.Y - 32;
            if (compRec.Intersects(Child.collision.rec))
            { Poke(Direction.Up, Parent, Child); return; }

            //check down
            compRec.X = Parent.collision.rec.X;
            compRec.Y = Parent.collision.rec.Y + 32;
            if (compRec.Intersects(Child.collision.rec))
            { Poke(Direction.Down, Parent, Child); return; }
        }

        static Point poke = new Point(0, 0); //used to see if child.collision.contains() poke value
        static void Poke(Direction Dir, Room Parent, Room Child)
        {
            List<Point> doorPos = new List<Point>(); //a list of possible door positions


            #region Check Left

            if (Dir == Direction.Left)
            {   //iterate vertically left of parent from top left corner
                for (int i = 0; i < Parent.size.Y; i++)
                {
                    poke.X = Parent.collision.rec.X - 24;
                    poke.Y = Parent.collision.rec.Y + i * 16;
                    if (Child.collision.rec.Contains(poke))
                    { poke.X += 8;  doorPos.Add(poke); }
                }
            }

            #endregion


            #region Check Right

            else if(Dir == Direction.Right)
            {   //iterate vertically left of parent from top right corner
                for (int i = 0; i < Parent.size.Y; i++)
                {
                    poke.X = Parent.collision.rec.X + Parent.collision.rec.Width + 24;
                    poke.Y = Parent.collision.rec.Y + i * 16;
                    if (Child.collision.rec.Contains(poke))
                    { poke.X -= 8; doorPos.Add(poke); }
                }
            }

            #endregion


            #region Check Up

            else if (Dir == Direction.Up)
            {   //iterate horizontally above parent from top left corner
                for (int i = 0; i < Parent.size.X; i++)
                {
                    poke.X = Parent.collision.rec.X + i * 16;
                    poke.Y = Parent.collision.rec.Y - 24;
                    if (Child.collision.rec.Contains(poke))
                    { poke.Y += 8; doorPos.Add(poke); }
                }
            }

            #endregion


            #region Check Down

            else if (Dir == Direction.Down)
            {   //iterate horizontally below parent from bottom left corner
                for (int i = 0; i < Parent.size.X; i++)
                {
                    poke.X = Parent.collision.rec.X + i * 16;
                    poke.Y = Parent.collision.rec.Y + Parent.collision.rec.Height + 24;
                    if (Child.collision.rec.Contains(poke))
                    { poke.Y -= 24; doorPos.Add(poke); }
                }
            }

            #endregion


            if (doorPos.Count > 2) //choose middle door position
            { dungeon.doorLocations.Add(doorPos[(int)doorPos.Count / 2]); }
            else { dungeon.doorLocations.Add(doorPos[0]); } //choose 1st door
        }

        public static void LoadShop()
        {
            dungeonType = DungeonType.Shop;
            ScreenManager.ExitAndLoad(new ScreenDungeon());
        }

    }
}