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
    public static class DungeonFunctions
    {

        public static DungeonScreen dungeonScreen;
        public static Dungeon dungeon;
        public static Room currentRoom; //points to a room on the dungeon's room list

        public static Stopwatch stopWatch = new Stopwatch();
        public static TimeSpan time;
        public static int i;
        public static int j;
        public static int g;
        public static ComponentSprite floorRef;
        public static GameObject objRef;
        public static Actor actorRef;
        public static Point pos;



        public static void Initialize(DungeonScreen DungeonScreen) { dungeonScreen = DungeonScreen; }

        public static void BuildDungeon()
        {
            //create a new dungeon
            dungeon = new Dungeon("test");
            dungeon.rooms.Add(new Room(new Point(16 * 10, 16 * 21), new Byte2(20, 10), RoomType.Normal, 10, 0));
            dungeon.rooms.Add(new Room(new Point(16 * 10, 16 * 10), new Byte2(20, 10), RoomType.Boss, 10, 1));

            //build the first room in the dungeon (the spawn room)
            BuildRoom(dungeon.rooms[0]);
            currentRoom = dungeon.rooms[0];

            //center hero to spawn room
            ActorFunctions.SetType(Pool.hero, Actor.Type.Hero);
            MovementFunctions.Teleport(Pool.hero.compMove, 
                dungeon.rooms[0].center.X, 
                dungeon.rooms[0].center.Y);

            //reset the dungeon screen's dungeon record, passing dungeonID
            DungeonRecord.Reset();
            DungeonRecord.dungeonID = 0; //ID = 0 for now
            DungeonRecord.timer.Start(); //start the record timer

            //load the dungeon's music track
            i = GetRandom.Int(0, 100);
            if (i > 50) { MusicFunctions.trackToLoad = Music.DungeonA; }
            else { MusicFunctions.trackToLoad = Music.Overworld; }
            //else { MusicFunctions.trackToLoad = Music.Shop; }

            //tell music functions to play the loaded music track
            MusicFunctions.fadeState = MusicFunctions.FadeState.Silent;

            //fade the dungeon screen out from black, revealing the new level
            dungeonScreen.overlayAlpha = 1.0f;
            dungeonScreen.screenState = DungeonScreen.ScreenState.FadeOut;
        }

        public static void BuildRoom(Room Room)
        {
            stopWatch.Reset(); stopWatch.Start();

            //reset the pools + counter
            PoolFunctions.Reset();
            Pool.counter = 0;
            //shorten the room's position reference
            pos = Room.collision.rec.Location;


            #region Build the room

            for (i = 0; i < Room.size.x; i++)
            {
                for (j = 0; j < Room.size.y; j++)
                {
                    //place the floors
                    floorRef = PoolFunctions.GetFloor();
                    floorRef.position.X = i * 16 + pos.X + 8;
                    floorRef.position.Y = j * 16 + pos.Y + 8;

                    if (Room.type == RoomType.Normal) { floorRef.currentFrame.y = 0; }
                    else if (Room.type == RoomType.Boss) { floorRef.currentFrame.y = 1; }


                    #region Top Row Walls

                    if (j == 0)
                    {
                        //top row
                        objRef = PoolFunctions.GetObj();
                        MovementFunctions.Teleport(objRef.compMove,
                            i * 16 + pos.X + 8,
                            0 * 16 - 16 + pos.Y + 8);
                        objRef.direction = Direction.Down;
                        GameObjectFunctions.SetType(objRef, GameObject.Type.WallStraight);

                        if (i == 0)
                        {   //topleft corner
                            objRef = PoolFunctions.GetObj();
                            MovementFunctions.Teleport(objRef.compMove, 
                                -16 + pos.X + 8, 
                                -16 + pos.Y + 8);
                            objRef.direction = Direction.Down;
                            GameObjectFunctions.SetType(objRef, GameObject.Type.WallInteriorCorner);
                        }
                        else if (i == Room.size.x - 1)
                        {   //topright corner
                            objRef = PoolFunctions.GetObj();
                            MovementFunctions.Teleport(objRef.compMove,
                                Room.size.x * 16 + pos.X + 8, 
                                -16 + pos.Y + 8);
                            objRef.direction = Direction.Left;
                            GameObjectFunctions.SetType(objRef, GameObject.Type.WallInteriorCorner);
                        }
                    }

                    #endregion


                    #region Bottom Row Walls

                    else if (j == Room.size.y - 1)
                    {
                        //bottom row
                        objRef = PoolFunctions.GetObj();
                        MovementFunctions.Teleport(objRef.compMove,
                            i * 16 + pos.X + 8,
                            Room.size.y * 16 + pos.Y + 8);
                        objRef.direction = Direction.Up;
                        GameObjectFunctions.SetType(objRef, GameObject.Type.WallStraight);

                        if (i == 0)
                        {   //bottom left corner
                            objRef = PoolFunctions.GetObj();
                            MovementFunctions.Teleport(objRef.compMove, 
                                -16 + pos.X + 8,
                                Room.size.y * 16 + pos.Y + 8);
                            objRef.direction = Direction.Right;
                            GameObjectFunctions.SetType(objRef, GameObject.Type.WallInteriorCorner);
                        }
                        else if (i == Room.size.x - 1)
                        {   //bottom right corner
                            objRef = PoolFunctions.GetObj();
                            MovementFunctions.Teleport(objRef.compMove,
                                Room.size.x * 16 + pos.X + 8,
                                Room.size.y * 16 + pos.Y + 8);
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
                            i * 16 - 16 + pos.X + 8, 
                            j * 16 + pos.Y + 8);
                        objRef.direction = Direction.Right;
                        GameObjectFunctions.SetType(objRef, GameObject.Type.WallStraight);
                    }
                    else if (i == Room.size.x - 1)
                    {   //right side
                        objRef = PoolFunctions.GetObj();
                        MovementFunctions.Teleport(objRef.compMove, 
                            i * 16 + 16 + pos.X + 8, 
                            j * 16 + pos.Y + 8);
                        objRef.direction = Direction.Left;
                        GameObjectFunctions.SetType(objRef, GameObject.Type.WallStraight);
                    }

                    #endregion
                    
                }
            }

            #endregion


            FinishRoom(Room); //add type specific room objs
            CleanupRoom(Room); //remove overlapping objs
            //update the object pool, since we teleported objects around
            PoolFunctions.UpdateObjectPool();

            stopWatch.Stop(); time = stopWatch.Elapsed;
            DebugInfo.roomTime = time.Ticks;
        }

        public static void FinishRoom(Room Room)
        {

            #region Normal Room (spawn room)

            if (Room.type == RoomType.Normal)
            {
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    5 * 16 + pos.X + 8,
                    0 * 16 - 16 + pos.Y + 8);
                objRef.direction = Direction.Down;
                GameObjectFunctions.SetType(objRef, GameObject.Type.DoorBoss);
                //build left wall torch
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    (5 - 1) * 16 + pos.X + 8,
                    0 * 16 - 16 + pos.Y + 8);
                objRef.direction = Direction.Down;
                GameObjectFunctions.SetType(objRef, GameObject.Type.WallTorch);
                //build right wall torch
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    (5 + 1) * 16 + pos.X + 8,
                    0 * 16 - 16 + pos.Y + 8);
                objRef.direction = Direction.Down;
                GameObjectFunctions.SetType(objRef, GameObject.Type.WallTorch);


                //build the boss welcome mat (left)
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    5 * 16 + pos.X + 0,
                    1 * 16 - 16 + pos.Y + 8);
                objRef.direction = Direction.Down;
                GameObjectFunctions.SetType(objRef, GameObject.Type.BossDecal);
                //build the right welcome mat
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    6 * 16 + pos.X + 0,
                    1 * 16 - 16 + pos.Y + 8);
                objRef.direction = Direction.Down;
                GameObjectFunctions.SetType(objRef, GameObject.Type.BossDecal);
                objRef.compSprite.flipHorizontally = true;

                //place bigKey gameObj in bottom right corner
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    (Room.size.x - 1) * 16 + pos.X + 8,
                    (Room.size.y - 1) * 16 + pos.Y + 8);
                objRef.direction = Direction.Down;
                GameObjectFunctions.SetType(objRef, GameObject.Type.ItemBigKey);

                //spawn enemies inside of this room
                SpawnEnemies(Room);
            }

            #endregion


            #region Boss Room

            else if (Room.type == RoomType.Boss)
            {
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    5 * 16 + pos.X + 8,
                    Room.size.y * 16 + pos.Y + 8);
                objRef.direction = Direction.Up;
                GameObjectFunctions.SetType(objRef, GameObject.Type.DoorTrap);
                //build left wall torch
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    (5 - 1) * 16 + pos.X + 8,
                    Room.size.y * 16 + pos.Y + 8);
                objRef.direction = Direction.Up;
                GameObjectFunctions.SetType(objRef, GameObject.Type.WallTorch);
                //build right wall torch
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    (5 + 1) * 16 + pos.X + 8,
                    Room.size.y * 16 + pos.Y + 8);
                objRef.direction = Direction.Up;
                GameObjectFunctions.SetType(objRef, GameObject.Type.WallTorch);

                //spawn a boss actor
                actorRef = PoolFunctions.GetActor();
                ActorFunctions.SetType(actorRef, Actor.Type.Boss);
                //teleport boss to center of room
                MovementFunctions.Teleport(actorRef.compMove,
                    Room.center.X + 8,
                    Room.center.Y + 8);
                //dont spawn any mobs in this room
            }

            #endregion

        }

        public static void SpawnEnemies(Room Room)
        {
            if (Flags.SpawnMobs)
            {
                //place enemies within the room
                for (i = 0; i < Room.enemyCount; i++)
                {
                    actorRef = PoolFunctions.GetActor();
                    //we SHOULD be checking to see if actorRef is null..
                    //but because we reset the pool earlier in this function,
                    //and the room's enemy count will never be larger than the total actors
                    //we'll never get a null result from GetActor() right here
                    ActorFunctions.SetType(actorRef, Actor.Type.Blob);
                    //get a random value between the min/max size of room
                    int randomX = GetRandom.Int(-Room.size.x + 2, Room.size.x - 2);
                    int randomY = GetRandom.Int(-Room.size.y + 2, Room.size.y - 2);
                    //divide random value in half
                    randomX = randomX / 2;
                    randomY = randomY / 2;
                    //ensure this value isn't 0
                    if (randomX == 0) { randomX = 1; }
                    if (randomY == 0) { randomY = 1; }
                    //teleport actor to center of room, apply random offset
                    MovementFunctions.Teleport(actorRef.compMove,
                        Room.center.X + 16 * randomX + 8,
                        Room.center.Y + 16 * randomY + 8);
                }
            }
        }

        public static void CleanupRoom(Room Room)
        {
            //remove any walls that overlap doors
            for (i = 0; i < Pool.objCount; i++)
            {
                if (Pool.objPool[i].objGroup == GameObject.ObjGroup.Door)
                {
                    for (j = 0; j < Pool.objCount; j++)
                    {
                        if (Pool.objPool[j].objGroup == GameObject.ObjGroup.Wall)
                        {
                            if (Pool.objPool[i].compCollision.rec.Intersects(Pool.objPool[j].compCollision.rec))
                            {
                                PoolFunctions.Release(Pool.objPool[j]);
                            }
                        }
                    }
                }
            }
        }

    }
}