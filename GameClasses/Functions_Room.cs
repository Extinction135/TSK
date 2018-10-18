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
    public static class Functions_Room
    {
        public static Stopwatch stopWatch = new Stopwatch();
        public static TimeSpan time;
        public static int i;
        public static int j;
        public static ComponentSprite floorRef;
        public static GameObject objRef;
        public static Actor actorRef;
        public static Point pos;



        public static void SetType(Room Room, RoomID ID)
        {
            Room.roomID = ID;
            
            //set room size based on type - sizes should be odd, so doors/exits can be centered
            if (ID == RoomID.Exit || ID == RoomID.DEV_Exit)
            {
                Room.size.X = 11; Room.size.Y = 11;
            }
            else if (ID == RoomID.Hub || ID == RoomID.DEV_Hub)
            {
                Room.size.X = 19; Room.size.Y = 19;
            }
            else if (ID == RoomID.Boss || ID == RoomID.DEV_Boss)
            {
                Room.size.X = 19; Room.size.Y = 11;
            }
            else if (ID == RoomID.Key || ID == RoomID.DEV_Key)
            {
                Room.size.X = 19; Room.size.Y = 11;
            }
            
            //dungeon rooms
            else if (ID == RoomID.Column || ID == RoomID.DEV_Column)
            {
                Room.size.X = 11; Room.size.Y = 19;
            }
            else if (ID == RoomID.Row || ID == RoomID.DEV_Row)
            {
                Room.size.X = 19; Room.size.Y = 11;
            }
            else if (ID == RoomID.Square || ID == RoomID.DEV_Square)
            {
                Room.size.X = 11; Room.size.Y = 11;
            }
            else if (ID == RoomID.Secret)
            {
                Room.size.X = 3; Room.size.Y = 3;
            }

            //field rooms fill the screen - maintain 16:9 ratio
            else
            {
                //Room.size.X = 40; Room.size.Y = 23; //640x360 / 16
                Room.size.X = 40*2; Room.size.Y = 23*2; //double size
            } 

            //set collision rec size
            Room.rec.Width = Room.size.X * 16;
            Room.rec.Height = Room.size.Y * 16;
        }

        public static void MoveRoom(Room Room, int X, int Y)
        {
            Room.rec.X = X;
            Room.rec.Y = Y;
            Room.center.X = X + (Room.size.X / 2) * 16;
            Room.center.Y = Y + (Room.size.Y / 2) * 16;
        }

        
        //removes overlapping objs from room
        static GameObject objA; //object we want to keep
        static GameObject objB; //object we want to remove
        static Boolean removeObjB;

        public static void Cleanup(Room Room)
        {   //skip the hero's pet (roomObj[0])
            for (i = 1; i < Pool.roomObjCount; i++)
            {
                objA = Pool.roomObjPool[i];
                if (objA.active)
                {   //loop thru roomObjs checking A against B 
                    for (j = 1; j < Pool.roomObjCount; j++) //skip hero's pet here too
                    {
                        objB = Pool.roomObjPool[j];
                        if (objB.active)
                        {   //make sure we aren't checking an object against itself
                            if (objA != objB)
                            {
                                removeObjB = false;

                                //these are roomObjects we remove, if they overlap ANY other roomObject
                                if (objB.group == ObjGroup.Wall)
                                {
                                    removeObjB = true;
                                    //keep these walls
                                    if (objB.type == ObjType.Dungeon_WallPillar) { removeObjB = false; }
                                    //keep walls if these objs overlap them
                                    if (objA.type == ObjType.Dungeon_PitTrap) { removeObjB = false; }
                                }
                                else if (objB.type == ObjType.Dungeon_FloorBlood)
                                {
                                    removeObjB = true;
                                    //allow overlap with these objects
                                    if (objA.type == ObjType.Dungeon_ConveyorBeltOn) { removeObjB = false; }
                                    else if (objA.type == ObjType.Dungeon_ConveyorBeltOff) { removeObjB = false; }
                                }

                                if (removeObjB)
                                {   //check that objA and objB actually overlap
                                    if (objA.compCollision.rec.Intersects(objB.compCollision.rec))
                                    { Functions_Pool.Release(objB); }
                                }
                            }
                        }
                    }

                }
            }
        }
        

        public static void BuildRoom(Room Room, RoomXmlData RoomXmlData = null)
        {
            if (Flags.PrintOutput)
            { Debug.WriteLine("-- building room: " + Room.roomID + " --"); }
            stopWatch.Reset(); stopWatch.Start();



            //Setup RoomData OR DevRooms

            if (RoomXmlData == null)
            {



                #region Setup SkullIsland Rooms (Levels)

                if (Room.roomID == RoomID.SkullIsland_Colliseum)
                {
                    RoomXmlData = LevelData_SkullIsland.SkullIsland_Colliseum;
                    LevelSet.currentLevel.isField = true;
                }
                else if (Room.roomID == RoomID.SkullIsland_ColliseumPit)
                {
                    RoomXmlData = LevelData_SkullIsland.SkullIsland_ColliseumPit;
                    LevelSet.currentLevel.isField = true;
                }
                else if (Room.roomID == RoomID.SkullIsland_Town)
                {
                    RoomXmlData = LevelData_SkullIsland.SkullIsland_Town;
                    LevelSet.currentLevel.isField = true;
                }
                else if (Room.roomID == RoomID.SkullIsland_ShadowKing)
                {
                    RoomXmlData = LevelData_SkullIsland.SkullIsland_ShadowKing;
                    LevelSet.currentLevel.isField = true;
                }

                #endregion









                #region Setup dungeon room data

                //here is where we need to tie specific roomData to dungeon types

                else if (Room.roomID == RoomID.Boss)
                {
                    //find a matching bossRoom to use
                    for (i = 0; i < RoomData.bossRooms.Count; i++)
                    {   //this works for now, >> put on list based on levelID, then randomly choose from
                        if (RoomData.bossRooms[i].levelID == LevelSet.currentLevel.ID)
                        { RoomXmlData = RoomData.bossRooms[i]; } //grabs last valid one (consistently)
                    }
                    //RoomXmlData = RoomData.bossRooms[Room.dataIndex];
                    LevelSet.currentLevel.isField = false;
                }
                else if (Room.roomID == RoomID.Column)
                {
                    RoomXmlData = RoomData.columnRooms[Room.dataIndex];
                    LevelSet.currentLevel.isField = false;
                }
                else if (Room.roomID == RoomID.Exit)
                {
                    RoomXmlData = RoomData.exitRooms[Room.dataIndex];
                    LevelSet.currentLevel.isField = false;
                }
                else if (Room.roomID == RoomID.Hub)
                {
                    //find a matching hubRoom to use
                    for (i = 0; i < RoomData.hubRooms.Count; i++)
                    {   //this works for now, >> put on list based on levelID, then randomly choose from
                        if (RoomData.hubRooms[i].levelID == LevelSet.currentLevel.ID)
                        { RoomXmlData = RoomData.hubRooms[i]; } //grabs last valid one (consistently)
                    }
                    //RoomXmlData = RoomData.hubRooms[Room.dataIndex];
                    LevelSet.currentLevel.isField = false;
                }
                else if (Room.roomID == RoomID.Key)
                {
                    RoomXmlData = RoomData.keyRooms[Room.dataIndex];
                    LevelSet.currentLevel.isField = false;
                }
                else if (Room.roomID == RoomID.Row)
                {
                    RoomXmlData = RoomData.rowRooms[Room.dataIndex];
                    LevelSet.currentLevel.isField = false;
                }
                else if (Room.roomID == RoomID.Square)
                {
                    RoomXmlData = RoomData.squareRooms[Room.dataIndex];
                    LevelSet.currentLevel.isField = false;
                }

                #endregion

            }


            #region Setup DEV ROOMS

            if (Room.roomID == RoomID.DEV_Boss || Room.roomID == RoomID.DEV_Column ||
                Room.roomID == RoomID.DEV_Exit || Room.roomID == RoomID.DEV_Hub ||
                Room.roomID == RoomID.DEV_Key || Room.roomID == RoomID.DEV_Row ||
                Room.roomID == RoomID.DEV_Square)
            {
                RoomXmlData = new RoomXmlData();
                LevelSet.currentLevel.isField = false;
                RoomXmlData.type = Room.roomID;
                SetType(LevelSet.currentLevel.currentRoom, Room.roomID);
                //add nsew doors so hero can enter/exit for testing
                Functions_Dungeon.AddDevDoors(Room); 
            }
            else if (Room.roomID == RoomID.DEV_Field)
            {
                RoomXmlData = new RoomXmlData();
                LevelSet.currentLevel.isField = true;
            }

            #endregion
            

            #region Build the room + roomData

            if (LevelSet.currentLevel.isField)
            {   //we are building an outdoor overworld field room
                Functions_Pool.Reset();
                BuildRoomXmlData(RoomXmlData);
            }
            else
            {   //else, we are building an interior dungeon room
                Functions_Dungeon.BuildRoomFrom(RoomXmlData);
            }

            #endregion
            

            #region Handle room specific initial events (like setting music)

            if (Room.roomID == RoomID.Boss)
            {
                Assets.Play(Assets.sfxBossIntro);
                Functions_Music.PlayMusic(Music.Boss);
            }

            #endregion



            stopWatch.Stop(); time = stopWatch.Elapsed;
            if (Flags.PrintOutput)
            { Debug.WriteLine("room " + Room.roomID + " built in " + time.Ticks + " ticks"); }
        }


        public static void BuildRoomXmlData(RoomXmlData RoomXmlData = null)
        {
            //note that RoomXmlData is an optional parameter

            #region Create room objs & enemies

            if (RoomXmlData != null && RoomXmlData.objs.Count > 0)
            {
                for (i = 0; i < RoomXmlData.objs.Count; i++)
                {   
                    //we store roomObjs in roomXmlData
                    objRef = Functions_Pool.GetRoomObj();

                    //move roomObj to xmlObj's position (with room offset)
                    Functions_Movement.Teleport(objRef.compMove,
                        LevelSet.currentLevel.currentRoom.rec.X + RoomXmlData.objs[i].posX,
                        LevelSet.currentLevel.currentRoom.rec.Y + RoomXmlData.objs[i].posY);
                    //get obj direction
                    objRef.direction = RoomXmlData.objs[i].direction;
                    //finally, set roomObj.type to xmlObj.type
                    Functions_GameObject.SetType(objRef, RoomXmlData.objs[i].type);

                    //create enemies at enemySpawn obj locations
                    if (objRef.group == ObjGroup.EnemySpawn)
                    {
                        //here we check level.id to determine what
                        //type of STANDARD enemy to spawn
                        if(LevelSet.currentLevel.ID == LevelID.Forest_Dungeon)
                        {
                            Functions_Actor.SpawnActor(ActorType.Standard_AngryEye, objRef.compSprite.position);
                        }
                        else if(LevelSet.currentLevel.ID == LevelID.Mountain_Dungeon)
                        {   
                            Functions_Actor.SpawnActor(ActorType.Standard_BeefyBat, objRef.compSprite.position);
                        }
                        else if (LevelSet.currentLevel.ID == LevelID.Swamp_Dungeon)
                        {
                            //we spawn blobs for now, but we need swamp standards SOON
                            Functions_Actor.SpawnActor(ActorType.Blob, objRef.compSprite.position);
                        }


                        else
                        {   //any other dungeon spawns blobs
                            Functions_Actor.SpawnActor(ActorType.Blob, objRef.compSprite.position);
                        }
                    }
                }
            }

            #endregion


            #region Check enemySpawn obj visibility

            if (Flags.ShowEnemySpawns == false)
            {   //find any spawnObj, set obj.active = false
                for (i = 0; i < Pool.roomObjCount; i++)
                {
                    if (Pool.roomObjPool[i].group == ObjGroup.EnemySpawn)
                    { Pool.roomObjPool[i].active = false; }
                }
            }

            #endregion

        }

    }
}