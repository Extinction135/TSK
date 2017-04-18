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
    public static class DungeonGenerator
    {

        public static DungeonScreen dungeonScreen;
        public static Dungeon dungeon;



        public static Stopwatch stopWatch = new Stopwatch();
        public static TimeSpan time;
        public static int i;
        public static int j;
        public static int g;
        public static ComponentSprite floorRef;
        public static GameObject objRef;
        public static Actor actorRef;



        public static void Initialize(DungeonScreen DungeonScreen)
        {
            dungeonScreen = DungeonScreen;
            dungeon = new Dungeon("test");
            dungeon.rooms.Add(new Room(new Point(16 * 10, 16 * 21), new Byte2(20, 10), RoomType.Normal, 10));
            dungeon.rooms.Add(new Room(new Point(16 * 10, 16 * 10), new Byte2(20, 10), RoomType.Normal, 10));
        }

        /*
        public static void RandomizeRoom()
        {
            room.position.X = 16 * 10;
            room.position.Y = 16 * 10;
            room.size.x = (byte)GetRandom.Int(15, 30);
            room.size.y = (byte)GetRandom.Int(7, 12);
            room.center.X = room.size.x / 2 * 16 + room.position.X;
            room.center.Y = room.size.y / 2 * 16 + room.position.Y;
            room.enemyCount = (byte)GetRandom.Int(7, 12);
            room.type = RoomType.Normal;
        }
        */



        public static void BuildDungeon()
        {
            BuildRoom(dungeon.rooms[0]);
        }
  
        public static void BuildRoom(Room Room)
        {
            stopWatch.Reset(); stopWatch.Start();

            //RandomizeRoom();
            //reset the pools + counter
            PoolFunctions.Reset();
            Pool.counter = 0;


            #region Build the room

            for (i = 0; i < Room.size.x; i++)
            {
                for (j = 0; j < Room.size.y; j++)
                {
                    //place the floors
                    floorRef = PoolFunctions.GetFloor();
                    floorRef.position.X = i * 16 + Room.position.X;
                    floorRef.position.Y = j * 16 + Room.position.Y;
                    

                    #region Top Row Walls

                    if (j == 0)
                    {

                        

                        //build a test door
                        if (i == 5)
                        {
                            objRef = PoolFunctions.GetObj();
                            MovementFunctions.Teleport(objRef.compMove,
                                i * 16 + Room.position.X,
                                0 * 16 - 16 + Room.position.Y);
                            objRef.direction = Direction.Down;
                            GameObjectFunctions.SetType(objRef, GameObject.Type.DoorOpen);

                            //build left wall torch
                            objRef = PoolFunctions.GetObj();
                            MovementFunctions.Teleport(objRef.compMove,
                                (i - 1) * 16 + Room.position.X,
                                0 * 16 - 16 + Room.position.Y);
                            objRef.direction = Direction.Down;
                            GameObjectFunctions.SetType(objRef, GameObject.Type.WallTorch);

                            //build right wall torch
                            objRef = PoolFunctions.GetObj();
                            MovementFunctions.Teleport(objRef.compMove,
                                (i + 1) * 16 + Room.position.X,
                                0 * 16 - 16 + Room.position.Y);
                            objRef.direction = Direction.Down;
                            GameObjectFunctions.SetType(objRef, GameObject.Type.WallTorch);
                        }
                        else
                        {
                            //top row
                            objRef = PoolFunctions.GetObj();
                            MovementFunctions.Teleport(objRef.compMove,
                                i * 16 + Room.position.X,
                                0 * 16 - 16 + Room.position.Y);
                            objRef.direction = Direction.Down;
                            GameObjectFunctions.SetType(objRef, GameObject.Type.WallStraight);

                            
                        }



                        if (i == 0)
                        {   //topleft corner
                            objRef = PoolFunctions.GetObj();
                            MovementFunctions.Teleport(objRef.compMove, 
                                -16 + Room.position.X, 
                                -16 + Room.position.Y);
                            objRef.direction = Direction.Down;
                            GameObjectFunctions.SetType(objRef, GameObject.Type.WallInteriorCorner);
                        }
                        else if (i == Room.size.x - 1)
                        {   //topright corner
                            objRef = PoolFunctions.GetObj();
                            MovementFunctions.Teleport(objRef.compMove,
                                Room.size.x * 16 + Room.position.X, 
                                -16 + Room.position.Y);
                            objRef.direction = Direction.Left;
                            GameObjectFunctions.SetType(objRef, GameObject.Type.WallInteriorCorner);
                        }
                    }

                    #endregion


                    #region Bottom Row Walls

                    else if (j == Room.size.y - 1)
                    {   //bottom row
                        objRef = PoolFunctions.GetObj();
                        MovementFunctions.Teleport(objRef.compMove, 
                            i * 16 + Room.position.X,
                            Room.size.y * 16 + Room.position.Y);
                        objRef.direction = Direction.Up;
                        GameObjectFunctions.SetType(objRef, GameObject.Type.WallStraight);
                        if (i == 0)
                        {   //bottom left corner
                            objRef = PoolFunctions.GetObj();
                            MovementFunctions.Teleport(objRef.compMove, 
                                -16 + Room.position.X,
                                Room.size.y * 16 + Room.position.Y);
                            objRef.direction = Direction.Right;
                            GameObjectFunctions.SetType(objRef, GameObject.Type.WallInteriorCorner);
                        }
                        else if (i == Room.size.x - 1)
                        {   //bottom right corner
                            objRef = PoolFunctions.GetObj();
                            MovementFunctions.Teleport(objRef.compMove,
                                Room.size.x * 16 + Room.position.X,
                                Room.size.y * 16 + Room.position.Y);
                            objRef.direction = Direction.Up;
                            GameObjectFunctions.SetType(objRef, GameObject.Type.WallInteriorCorner);
                        }
                    }

                    #endregion


                    #region Left & Right Column Walls

                    if (i == 0)
                    {   //left side
                        objRef = PoolFunctions.GetObj();
                        MovementFunctions.Teleport(objRef.compMove, 
                            i * 16 - 16 + Room.position.X, 
                            j * 16 + Room.position.Y);
                        objRef.direction = Direction.Right;
                        GameObjectFunctions.SetType(objRef, GameObject.Type.WallStraight);
                    }
                    else if (i == Room.size.x - 1)
                    {   //right side
                        objRef = PoolFunctions.GetObj();
                        MovementFunctions.Teleport(objRef.compMove, 
                            i * 16 + 16 + Room.position.X, 
                            j * 16 + Room.position.Y);
                        objRef.direction = Direction.Left;
                        GameObjectFunctions.SetType(objRef, GameObject.Type.WallStraight);
                    }

                    #endregion
                    
                }
            }

            //update the object pool, since we teleported objects around
            PoolFunctions.UpdateObjectPool();

            #endregion


            #region Set the Room Actors

            //place enemies within the room
            for (i = 0; i < Room.enemyCount; i++)
            {
                actorRef = PoolFunctions.GetActor();
                ActorFunctions.SetType(actorRef, Actor.Type.Blob);
                //get a random value between the min/max size of room
                int randomX = GetRandom.Int(-Room.size.x, Room.size.x);
                int randomY = GetRandom.Int(-Room.size.y, Room.size.y);
                //divide random value in half
                randomX = randomX / 2;
                randomY = randomY / 2;
                //ensure this value isn't 0
                if (randomX == 0) { randomX = 1; }
                if (randomY == 0) { randomY = 1; }
                //randomX = 0; randomY = 0; //debugging
                //actor.compCollision.blocking = false; //debugging
                //teleport actor to center of room, apply random offset
                MovementFunctions.Teleport(actorRef.compMove,
                    Room.center.X + 16 * randomX,
                    Room.center.Y + 16 * randomY);
            }
            
            #endregion


            //center hero to room
            ActorFunctions.SetType(Pool.hero, Actor.Type.Hero);
            MovementFunctions.Teleport(Pool.hero.compMove, Room.center.X, Room.center.Y);

            //reset the dungeon screen's dungeon record, passing dungeonID
            DungeonRecord.Reset();
            DungeonRecord.dungeonID = 0; //ID = 0 for now
            DungeonRecord.timer.Start(); //start the record timer

            //load the dungeon's music track
            i = GetRandom.Int(0, 100);
            if (i > 60) { MusicFunctions.trackToLoad = Music.DungeonA; }
            else if (i > 30) { MusicFunctions.trackToLoad = Music.Overworld; }
            else { MusicFunctions.trackToLoad = Music.Shop; }

            //tell music functions to play the loaded music track
            MusicFunctions.fadeState = MusicFunctions.FadeState.Silent;

            //fade the dungeon screen out from black, revealing the new level
            dungeonScreen.overlayAlpha = 1.0f;
            dungeonScreen.screenState = DungeonScreen.ScreenState.FadeOut;

            stopWatch.Stop(); time = stopWatch.Elapsed;
            DebugInfo.roomTime = time.Ticks;
        }

    }
}