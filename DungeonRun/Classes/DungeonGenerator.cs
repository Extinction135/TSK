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
    public static class DungeonGenerator
    {

        public static DungeonScreen dungeonScreen;

        public static Room room;
        public static Stopwatch stopWatch = new Stopwatch();
        public static TimeSpan time;
        public static int i;
        public static int j;

        public static ComponentSprite floorRef;
        public static GameObject objRef;
        public static Actor actorRef;

        public static void Initialize(DungeonScreen DungeonScreen)
        { dungeonScreen = DungeonScreen; }

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

        public static void BuildRoom()
        {
            stopWatch.Reset(); stopWatch.Start();

            RandomizeRoom();
            //reset the pools + counter
            PoolFunctions.Reset();
            Pool.counter = 0;


            #region Build the room

            for (i = 0; i < room.size.x; i++)
            {
                for (j = 0; j < room.size.y; j++)
                {
                    //place the floors
                    floorRef = PoolFunctions.GetFloor();
                    floorRef.position.X = i * 16 + room.position.X;
                    floorRef.position.Y = j * 16 + room.position.Y;
                    

                    #region Top Row Walls

                    if (j == 0)
                    {
                        //top row
                        objRef = PoolFunctions.GetObj();
                        MovementFunctions.Teleport(objRef.compMove, 
                            i * 16 + room.position.X, 
                            0 * 16 - 16 + room.position.Y);
                        objRef.direction = Direction.Down;
                        GameObjectFunctions.SetType(objRef, GameObject.Type.WallStraight);
                        if (i == 0)
                        {   //topleft corner
                            objRef = PoolFunctions.GetObj();
                            MovementFunctions.Teleport(objRef.compMove, 
                                -16 + room.position.X, 
                                -16 + room.position.Y);
                            objRef.direction = Direction.Down;
                            GameObjectFunctions.SetType(objRef, GameObject.Type.WallInteriorCorner);
                        }
                        else if (i == room.size.x - 1)
                        {   //topright corner
                            objRef = PoolFunctions.GetObj();
                            MovementFunctions.Teleport(objRef.compMove,
                                room.size.x * 16 + room.position.X, 
                                -16 + room.position.Y);
                            objRef.direction = Direction.Left;
                            GameObjectFunctions.SetType(objRef, GameObject.Type.WallInteriorCorner);
                        }
                    }

                    #endregion


                    #region Bottom Row Walls

                    else if (j == room.size.y - 1)
                    {   //bottom row
                        objRef = PoolFunctions.GetObj();
                        MovementFunctions.Teleport(objRef.compMove, 
                            i * 16 + room.position.X,
                            room.size.y * 16 + room.position.Y);
                        objRef.direction = Direction.Up;
                        GameObjectFunctions.SetType(objRef, GameObject.Type.WallStraight);
                        if (i == 0)
                        {   //bottom left corner
                            objRef = PoolFunctions.GetObj();
                            MovementFunctions.Teleport(objRef.compMove, 
                                -16 + room.position.X,
                                room.size.y * 16 + room.position.Y);
                            objRef.direction = Direction.Right;
                            GameObjectFunctions.SetType(objRef, GameObject.Type.WallInteriorCorner);
                        }
                        else if (i == room.size.x - 1)
                        {   //bottom right corner
                            objRef = PoolFunctions.GetObj();
                            MovementFunctions.Teleport(objRef.compMove,
                                room.size.x * 16 + room.position.X,
                                room.size.y * 16 + room.position.Y);
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
                            i * 16 - 16 + room.position.X, 
                            j * 16 + room.position.Y);
                        objRef.direction = Direction.Right;
                        GameObjectFunctions.SetType(objRef, GameObject.Type.WallStraight);
                    }
                    else if (i == room.size.x - 1)
                    {   //right side
                        objRef = PoolFunctions.GetObj();
                        MovementFunctions.Teleport(objRef.compMove, 
                            i * 16 + 16 + room.position.X, 
                            j * 16 + room.position.Y);
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
            for (i = 0; i < room.enemyCount; i++)
            {
                actorRef = PoolFunctions.GetActor();
                ActorFunctions.SetType(actorRef, Actor.Type.Blob);
                //get a random value between the min/max size of room
                int randomX = GetRandom.Int(-room.size.x, room.size.x);
                int randomY = GetRandom.Int(-room.size.y, room.size.y);
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
                    room.center.X + 16 * randomX, 
                    room.center.Y + 16 * randomY);
            }
            
            #endregion


            //center hero to room
            ActorFunctions.SetType(Pool.hero, Actor.Type.Hero);
            MovementFunctions.Teleport(Pool.hero.compMove, room.center.X, room.center.Y);

            //reset the dungeon screen's dungeon record, passing dungeonID
            DungeonRecord.Reset();
            DungeonRecord.dungeonID = 0; //ID = 0 for now
            DungeonRecord.timer.Start(); //start the record timer

            //fade the dungeon screen out from black, revealing the new level
            dungeonScreen.overlayAlpha = 1.0f;
            dungeonScreen.screenState = DungeonScreen.ScreenState.FadeOut;

            stopWatch.Stop(); time = stopWatch.Elapsed;
            DebugInfo.roomTime = time.Ticks;
        }

    }
}