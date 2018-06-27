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
                    for (j = 0; j < Pool.roomObjCount; j++)
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
            {   //assume level is dungeon level
                Level.isField = false;



                #region Setup Colliseum levels


                if (Room.roomID == RoomID.Colliseum)
                {
                    RoomXmlData = LevelData.Colliseum;
                    Level.isField = true;
                }
                else if (Room.roomID == RoomID.ColliseumPit)
                {
                    RoomXmlData = LevelData.ColliseumPit;
                    Level.isField = true;
                }

                #endregion


                #region Setup overworld level data

                else if (Room.roomID == RoomID.ForestEntrance)
                {
                    RoomXmlData = LevelData.ForestEntrance;
                    Level.isField = true;
                }
                else if (Room.roomID == RoomID.MountainEntrance)
                {
                    RoomXmlData = LevelData.MountainEntrance;
                    Level.isField = true;
                }

                else if (Room.roomID == RoomID.TheFarm)
                {
                    RoomXmlData = LevelData.TheFarm;
                    Level.isField = true;
                }
                else if (Room.roomID == RoomID.LeftTown2)
                {
                    RoomXmlData = LevelData.LeftTown2;
                    Level.isField = true;
                }

                #endregion


                #region Setup dungeon room data

                else if (Room.roomID == RoomID.Boss)
                {
                    RoomXmlData = RoomData.bossRooms[Room.dataIndex];
                }
                else if (Room.roomID == RoomID.Column)
                {
                    RoomXmlData = RoomData.columnRooms[Room.dataIndex];
                }
                else if (Room.roomID == RoomID.Exit)
                {
                    RoomXmlData = RoomData.exitRooms[Room.dataIndex];
                }
                else if (Room.roomID == RoomID.Hub)
                {
                    RoomXmlData = RoomData.hubRooms[Room.dataIndex];
                }
                else if (Room.roomID == RoomID.Key)
                {
                    RoomXmlData = RoomData.keyRooms[Room.dataIndex];
                }
                else if (Room.roomID == RoomID.Row)
                {
                    RoomXmlData = RoomData.rowRooms[Room.dataIndex];
                }
                else if (Room.roomID == RoomID.Square)
                {
                    RoomXmlData = RoomData.squareRooms[Room.dataIndex];
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
                Level.isField = false;
                RoomXmlData.type = Room.roomID;
                SetType(Level.currentRoom, Room.roomID);
                //add nsew doors so hero can enter/exit for testing
                Functions_Dungeon.AddDevDoors(Room); 
            }
            else if (Room.roomID == RoomID.DEV_Field)
            {
                RoomXmlData = new RoomXmlData();
                Level.isField = true;
            }

            #endregion
            

            #region Build the room + roomData

            if (Level.isField)
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
                        Level.currentRoom.rec.X + RoomXmlData.objs[i].posX,
                        Level.currentRoom.rec.Y + RoomXmlData.objs[i].posY);
                    //get obj direction
                    objRef.direction = RoomXmlData.objs[i].direction;
                    //finally, set roomObj.type to xmlObj.type
                    Functions_GameObject.SetType(objRef, RoomXmlData.objs[i].type);

                    //create enemies at enemySpawn obj locations
                    if (objRef.group == ObjGroup.EnemySpawn)
                    {
                        if (objRef.type == ObjType.Dungeon_SpawnMob)
                        {
                            //spawn a mob type enemy
                            Functions_Actor.SpawnActor(ActorType.Blob, objRef.compSprite.position);
                        }
                        else if (objRef.type == ObjType.Dungeon_SpawnMiniBoss)
                        {
                            //spawn a mini-boss type enemy
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