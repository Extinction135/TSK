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
            { Room.size.X = 11; Room.size.Y = 11; }
            else if (ID == RoomID.Hub || ID == RoomID.DEV_Hub)
            { Room.size.X = 19; Room.size.Y = 19; }
            else if (ID == RoomID.Boss || ID == RoomID.DEV_Boss)
            { Room.size.X = 19; Room.size.Y = 11; }
            else if (ID == RoomID.Key || ID == RoomID.DEV_Key)
            { Room.size.X = 19; Room.size.Y = 11; }
            
            //dungeon rooms
            else if (ID == RoomID.Column || ID == RoomID.DEV_Column)
            { Room.size.X = 11; Room.size.Y = 19; }
            else if (ID == RoomID.Row || ID == RoomID.DEV_Row)
            { Room.size.X = 19; Room.size.Y = 11; }
            else if (ID == RoomID.Square || ID == RoomID.DEV_Square)
            { Room.size.X = 11; Room.size.Y = 11; }
            else if (ID == RoomID.Secret)
            { Room.size.X = 3; Room.size.Y = 3; }

            //field rooms fill the screen
            else { Room.size.X = 40; Room.size.Y = 23; } 

            //set collision rec size
            Room.rec.Width = Room.size.X * 16;
            Room.rec.Height = Room.size.Y * 16;
        }
        
        public static void SetRoomXMLid(Room Room)
        {   //based on the room type, set the xml value between 0 and relative xmlRoomData list count
            int count = 1;
            if (Room.roomID == RoomID.Boss) { count = Assets.roomDataBoss.Count; }
            else if (Room.roomID == RoomID.Column) { count = Assets.roomDataColumn.Count; }
            else if (Room.roomID == RoomID.Hub) { count = Assets.roomDataHub.Count; }
            else if (Room.roomID == RoomID.Key) { count = Assets.roomDataKey.Count; }
            else if (Room.roomID == RoomID.Row) { count = Assets.roomDataRow.Count; }
            else if (Room.roomID == RoomID.Square) { count = Assets.roomDataSquare.Count; }
            Room.XMLid = Functions_Random.Int(0, count);
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
        {
            for (i = 0; i < Pool.roomObjCount; i++)
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

            if (Flags.DrawDebugInfo)
            {   //get the count of active roomObjs, if debug info is visible
                Pool.roomObjIndex = 0;
                for (i = 0; i < Pool.roomObjCount; i++)
                { if (Pool.roomObjPool[i].active) { Pool.roomObjIndex++; } }
            }
        }
        






        public static void AddDevDoors(Room Room)
        {   //add temporary doors to this room, so hero can enter/exit it
            int posX = Room.rec.X;
            int posY = Room.rec.Y;
            int middleX = (Room.size.X / 2) * 16;
            int middleY = (Room.size.Y / 2) * 16;
            int width = Room.size.X * 16;
            int height = Room.size.Y * 16;
            //set NSEW door positions
            Level.doors.Add(new Door(new Point(posX + middleX, posY - 16))); //top
            Level.doors.Add(new Door(new Point(posX + middleX, posY + height))); //bottom
            Level.doors.Add(new Door(new Point(posX - 16, posY + middleY))); //left
            Level.doors.Add(new Door(new Point(posX + width, posY + middleY))); //right
        }






        public static void BuildRoom(Room Room, RoomXmlData RoomXmlData = null)
        {

            //if we are developing, clear roomXmlData
            //if (Room.roomID == RoomID.DEV_Room || Room.roomID == RoomID.DEV_Field)
            //{ RoomXmlData = new RoomXmlData(); }

            if (RoomXmlData == null)
            {
                //if the roomData is null, then the method must set the roomData
                //this is done based on Room.RoomID, and sets the roomData
                //to an instance already loaded into Assets, but by default
                //to be safe, we set the roomXmlData to just be a new instance

                //assume level is dungeon room
                Level.isField = false;

                //dungeon rooms
                if (Room.roomID == RoomID.Key) { RoomXmlData = Assets.roomDataKey[Room.XMLid]; }
                else if (Room.roomID == RoomID.Hub) { RoomXmlData = Assets.roomDataHub[Room.XMLid]; }
                else if (Room.roomID == RoomID.Boss) { RoomXmlData = Assets.roomDataBoss[Room.XMLid]; }
                else if (Room.roomID == RoomID.Column) { RoomXmlData = Assets.roomDataColumn[Room.XMLid]; }
                else if (Room.roomID == RoomID.Row) { RoomXmlData = Assets.roomDataRow[Room.XMLid]; }
                else if (Room.roomID == RoomID.Square) { RoomXmlData = Assets.roomDataSquare[Room.XMLid]; }

                //in game rooms
                //else if (Room.roomID == RoomID.Colliseum) { RoomXmlData = Assets.overworldLevels[0]; }
                else
                {   //locate any in game rooms by their roomID / roomXmlData.type
                    for (int i = 0; i < Assets.overworldLevels.Count; i++)
                    {
                        if (Room.roomID == Assets.overworldLevels[i].type)
                        { RoomXmlData = Assets.overworldLevels[i]; }
                    }
                }
                

                //we should check for RoomID.DEV_Row, etc.. here
                //its the easiest solution right now

                //this should be handled better, because we don't actually
                //know that colliseum = overworldLevels[0], we just know
                //that there is only ONE overworld level rn, so it must be index0

                //loop over Assets.overworldLevels
                //find the Room with the same roomID and set RoomXmlData to that index
            }



            //setup dev rooms
            if (Room.roomID == RoomID.DEV_Boss || Room.roomID == RoomID.DEV_Column ||
                Room.roomID == RoomID.DEV_Exit || Room.roomID == RoomID.DEV_Hub ||
                Room.roomID == RoomID.DEV_Key || Room.roomID == RoomID.DEV_Row ||
                Room.roomID == RoomID.DEV_Square)
            {
                RoomXmlData = new RoomXmlData();
                Level.isField = false;
                
                RoomXmlData.type = Room.roomID;
                SetType(Functions_Level.currentRoom, Room.roomID);
                AddDevDoors(Room); //so we can test the room
            }
            else if (Room.roomID == RoomID.DEV_Field)
            {
                RoomXmlData = new RoomXmlData();
                Level.isField = true;
            }




            #region Build the referenced XmlData

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

            Debug.WriteLine("room built = " + Room.roomID);
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
                        Functions_Level.currentRoom.rec.X + RoomXmlData.objs[i].posX,
                        Functions_Level.currentRoom.rec.Y + RoomXmlData.objs[i].posY);
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