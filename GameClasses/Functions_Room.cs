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



        public static void BuildRoom(Room Room)
        {
            stopWatch.Reset(); stopWatch.Start();

            //reset the pools + counter
            Functions_Pool.Reset();
            //shorten the room's position reference
            pos = Room.rec.Location;


            #region Build the room

            for (i = 0; i < Room.size.X; i++)
            {
                for (j = 0; j < Room.size.Y; j++)
                {
                    //place the floors
                    floorRef = Functions_Pool.GetFloor();
                    floorRef.position.X = i * 16 + pos.X + 8;
                    floorRef.position.Y = j * 16 + pos.Y + 8;


                    #region Top Row Walls

                    if (j == 0)
                    {
                        //top row
                        Functions_RoomObject.SpawnRoomObj(
                            ObjType.WallStraight,
                            i * 16 + pos.X + 8, 
                            0 * 16 - 16 + pos.Y + 8, 
                            Direction.Down);

                        if (i == 0)
                        {   //topleft corner
                            Functions_RoomObject.SpawnRoomObj(
                                ObjType.WallInteriorCorner,
                                -16 + pos.X + 8, 
                                -16 + pos.Y + 8, 
                                Direction.Down);
                        }
                        else if (i == Room.size.X - 1)
                        {   //topright corner
                            Functions_RoomObject.SpawnRoomObj(
                                ObjType.WallInteriorCorner,
                                Room.size.X * 16 + pos.X + 8,
                                -16 + pos.Y + 8,
                                Direction.Left);
                        }
                    }

                    #endregion


                    #region Bottom Row Walls

                    else if (j == Room.size.Y - 1)
                    {
                        //bottom row
                        Functions_RoomObject.SpawnRoomObj(
                            ObjType.WallStraight,
                            i * 16 + pos.X + 8,
                            Room.size.Y * 16 + pos.Y + 8,
                            Direction.Up);

                        if (i == 0)
                        {   //bottom left corner
                            Functions_RoomObject.SpawnRoomObj(
                                ObjType.WallInteriorCorner,
                                -16 + pos.X + 8,
                                Room.size.Y * 16 + pos.Y + 8,
                                Direction.Right);
                        }
                        else if (i == Room.size.X - 1)
                        {   //bottom right corner
                            Functions_RoomObject.SpawnRoomObj(
                                ObjType.WallInteriorCorner,
                                Room.size.X * 16 + pos.X + 8,
                                Room.size.Y * 16 + pos.Y + 8,
                                Direction.Up);
                        }
                    }

                    #endregion


                    #region Left & Right Column Walls

                    if (i == 0)
                    {   //left side
                        Functions_RoomObject.SpawnRoomObj(
                            ObjType.WallStraight,
                            i * 16 - 16 + pos.X + 8,
                            j * 16 + pos.Y + 8,
                            Direction.Right);
                    }
                    else if (i == Room.size.X - 1)
                    {   //right side
                        Functions_RoomObject.SpawnRoomObj(
                            ObjType.WallStraight,
                            i * 16 + 16 + pos.X + 8,
                            j * 16 + pos.Y + 8,
                            Direction.Left);
                    }

                    #endregion

                }
            }

            #endregion


            SetDoors(Room); //set the room's doors
            SetFloors(Room); //set the floortile frames based on room.type
            //update all roomObjs, then remove overlapping objs
            Functions_Pool.AlignRoomObjs();
            CleanupRoom(Room); //remove overlapping objs

            stopWatch.Stop(); time = stopWatch.Elapsed;
            DebugInfo.roomTime = time.Ticks;
            if (Flags.PrintOutput)
            { Debug.WriteLine("built " + Room.type + " room in " + time.Ticks + " ticks"); }
        }

        public static void FinishRoom(Room Room)
        {   //Pass the room to the appropriate method for completion
            stopWatch.Reset(); stopWatch.Start();

            //procedurally finished rooms
            if (Room.type == RoomType.Shop) { FinishShopRoom(Room); }
            else if (Room.type == RoomType.Exit) { FinishExitRoom(Room); }
            else if (Room.type == RoomType.Secret) { FinishSecretRoom(Room); }
            //special rooms (key, hub, boss)
            else if (Room.type == RoomType.Key)
            {
                BuildRoomXmlData(Assets.roomDataKey[Room.XMLid]);
                FinishKeyRoom(Room);
                AddWallStatues(Room);
            }
            else if (Room.type == RoomType.Hub)
            {
                BuildRoomXmlData(Assets.roomDataHub[Room.XMLid]);
                FinishHubRoom(Room);
                AddWallStatues(Room);
            }
            else if (Room.type == RoomType.Boss)
            {
                BuildRoomXmlData(Assets.roomDataBoss[Room.XMLid]);
                FinishBossRoom(Room);
            }
            //standard/generic rooms (column, row, square)
            else if (Room.type == RoomType.Column)
            {
                BuildRoomXmlData(Assets.roomDataColumn[Room.XMLid]);
                AddWallStatues(Room);
            }
            else if (Room.type == RoomType.Row)
            {
                BuildRoomXmlData(Assets.roomDataRow[Room.XMLid]);
                AddWallStatues(Room);
            }
            else if (Room.type == RoomType.Square)
            {
                BuildRoomXmlData(Assets.roomDataSquare[Room.XMLid]);
                AddWallStatues(Room);
            }
            //dungeon rooms get debris + cracked walls
            if (Room.type != RoomType.Shop)
            {
                ScatterDebris(Room);
                AddCrackedWalls(Room);
            }
            //align + remove overlapping objs
            Functions_Pool.AlignRoomObjs();
            CleanupRoom(Room);
            Assets.Play(Assets.sfxDoorOpen); //play door sfx
            TeleportDoggo(); //teleport doggo to hero's position

            stopWatch.Stop(); time = stopWatch.Elapsed;
            DebugInfo.roomTime += time.Ticks; //add finish time to roomTime
            if (Flags.PrintOutput)
            { Debug.WriteLine("finished " + Room.type + " room (id:" + Room.XMLid + ") in " + time.Ticks + " ticks"); }
        }

        public static void SetRoomXMLid(Room Room)
        {   //based on the room type, set the xml value between 0 and relative xmlRoomData list count
            int count = 1;
            if (Room.type == RoomType.Boss) { count = Assets.roomDataBoss.Count; }
            else if (Room.type == RoomType.Column) { count = Assets.roomDataColumn.Count; }
            else if (Room.type == RoomType.Hub) { count = Assets.roomDataHub.Count; }
            else if (Room.type == RoomType.Key) { count = Assets.roomDataKey.Count; }
            else if (Room.type == RoomType.Row) { count = Assets.roomDataRow.Count; }
            else if (Room.type == RoomType.Square) { count = Assets.roomDataSquare.Count; }
            Room.XMLid = Functions_Random.Int(0, count);
        }

        public static void SetType(Room Room, RoomType Type)
        {
            Room.type = Type;
            //set room size based on type - sizes should be odd, so doors can be centered
            if (Type == RoomType.Exit) { Room.size.X = 11; Room.size.Y = 11; }
            else if (Type == RoomType.Hub) { Room.size.X = 19; Room.size.Y = 19; }
            else if (Type == RoomType.Boss) { Room.size.X = 19; Room.size.Y = 11; }
            else if (Type == RoomType.Key) { Room.size.X = 19; Room.size.Y = 11; }
            else if (Type == RoomType.Shop) { Room.size.X = 19; Room.size.Y = 11; }
            //comon dungeon rooms
            else if (Type == RoomType.Column) { Room.size.X = 11; Room.size.Y = 19; }
            else if (Type == RoomType.Row) { Room.size.X = 19; Room.size.Y = 11; }
            else if (Type == RoomType.Square) { Room.size.X = 11; Room.size.Y = 11; }
            else if (Type == RoomType.Secret) { Room.size.X = 3; Room.size.Y = 3; }
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

        public static void SetFloors(Room Room)
        {
            for (i = 0; i < Pool.floorCount; i++)
            {   //set all the floor sprite's current frame based on the room.type
                Pool.floorPool[i].currentFrame.X = 6;
                Pool.floorPool[i].currentFrame.Y = 0; //default to normal floor
                //determine what floor sprite to display for special rooms
                if (Room.type == RoomType.Hub) { Pool.floorPool[i].currentFrame.Y = 1; }
                else if (Room.type == RoomType.Key) { Pool.floorPool[i].currentFrame.Y = 1; }
                else if (Room.type == RoomType.Boss) { Pool.floorPool[i].currentFrame.Y = 2; }
            }
        }

        public static void SetDoors(Room Room)
        {
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active)
                {
                    if (Pool.roomObjPool[i].group == ObjGroup.Wall)
                    {   //check to see if wall collides with any door from dungeon
                        for (j = 0; j < Level.doors.Count; j++)
                        {
                            if (Pool.roomObjPool[i].compCollision.rec.Contains(Level.doors[j].rec.Location))
                            {
                                //set the room's doors based on the dungeon.door.type
                                if (Level.doors[j].type == DoorType.Bombable)
                                { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.DoorBombable); }

                                else if (Level.doors[j].type == DoorType.Boss)
                                { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.DoorBoss); }

                                else //all other doorTypes are Open
                                { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.DoorOpen); }

                                //set the door decorations (bombed/bombable doors dont get decorations)
                                if (Pool.roomObjPool[i].type == ObjType.DoorBoss)
                                { Functions_RoomObject.DecorateDoor(Pool.roomObjPool[i], ObjType.WallPillar); }
                                else if (Pool.roomObjPool[i].type == ObjType.DoorOpen)
                                { Functions_RoomObject.DecorateDoor(Pool.roomObjPool[i], ObjType.WallTorch); }

                                //finally, override door types based on specific room.type
                                if (Room.type == RoomType.Boss)
                                {   //all doors inside boss room are trap doors (push hero + close)
                                    Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.DoorTrap);
                                }

                                //sort door object
                                Functions_Component.SetZdepth(Pool.roomObjPool[i].compSprite);
                                //place a floor tile underneath door
                                floorRef = Functions_Pool.GetFloor();
                                floorRef.position = Pool.roomObjPool[i].compSprite.position;
                            }
                        }
                    }
                }
            }
        }

        public static void SpawnHeroInCurrentRoom()
        {   //teleport hero to currentRoom's spawn position
            Functions_Movement.Teleport(Pool.hero.compMove,
                Functions_Level.currentRoom.spawnPos.X,
                Functions_Level.currentRoom.spawnPos.Y);
            Functions_Movement.StopMovement(Pool.hero.compMove);
            Pool.hero.compSprite.scale = 1.0f; //rescale hero to 100%
            Pool.hero.state = ActorState.Idle;
            Pool.hero.stateLocked = false;
            //set camera's target to hero or room based on flag boolean
            if (Flags.CameraTracksHero) //center camera to hero
            { Camera2D.targetPosition = Pool.hero.compMove.newPosition; }
            else
            {   //center camera to current room
                Camera2D.targetPosition.X = Functions_Level.currentRoom.center.X;
                Camera2D.targetPosition.Y = Functions_Level.currentRoom.center.Y;
            }
            //teleport camera to targetPos, update camera view
            Camera2D.currentPosition.X = Camera2D.targetPosition.X;
            Camera2D.currentPosition.Y = Camera2D.targetPosition.Y;
            Functions_Camera2D.Update();
            TeleportDoggo(); //teleport doggo to hero's position
        }

        public static void TeleportDoggo()
        {   //teleport doggo to hero's position
            Functions_Movement.Teleport(Pool.doggo.compMove,
                Pool.hero.compMove.newPosition.X,
                Pool.hero.compMove.newPosition.Y);
            Functions_Movement.StopMovement(Pool.doggo.compMove);
            Pool.doggo.compSprite.scale = 1.0f; //rescale hero to 100%
        }



        //adds/changes objs in room 

        public static void PlaceExit(Room Room)
        {
            //create the exit
            Functions_RoomObject.SpawnRoomObj(ObjType.Exit,
                (Room.size.X / 2) * 16 + pos.X + 8,
                Room.size.Y * 16 + pos.Y + 8 - 16 * 2, 
                Direction.Down);

            //set the room.spawnPos to pos above exit door obj
            Room.spawnPos.X = (Room.size.X / 2) * 16 + Room.rec.X + 8;
            Room.spawnPos.Y = Room.rec.Y + (Room.size.Y - 1) * 16;

            //place the exit light fx over exit obj
            Functions_RoomObject.SpawnRoomObj(ObjType.ExitLightFX,
                (Room.size.X / 2) * 16 + pos.X + 8,
                Room.size.Y * 16 + pos.Y + 8 - 16 * 1,
                Direction.Down);

            //create exit pillars
            Functions_RoomObject.SpawnRoomObj(ObjType.ExitPillarLeft,
                (Room.size.X / 2) * 16 + pos.X + 8 - 16,
                Room.size.Y * 16 + pos.Y + 8 - 16 * 2,
                Direction.Down);
            Functions_RoomObject.SpawnRoomObj(ObjType.ExitPillarRight,
                (Room.size.X / 2) * 16 + pos.X + 8 + 16,
                Room.size.Y * 16 + pos.Y + 8 - 16 * 2,
                Direction.Down);
        }

        public static void AddWallStatues(Room Room)
        {   //add wall statues along 1/3rd and 2/3rds of all walls
            int RoomThirdX = Room.rec.X + (Room.size.X / 3) * 16 + 8;
            int RoomTwoThirdsX = 16 + RoomThirdX + (Room.size.X / 3) * 16;
            int RoomThirdY = Room.rec.Y + (Room.size.Y / 3) * 16 + 8;
            int RoomTwoThirdsY = 16 + RoomThirdY + (Room.size.Y / 3) * 16;
            //we could also check against room centers
            //if (obj.compSprite.position.X == Room.collision.rec.X + room.center.X)
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active)
                {
                    if (Pool.roomObjPool[i].type == ObjType.WallStraight)
                    {
                        if (Pool.roomObjPool[i].compSprite.position.X == RoomThirdX ||
                            Pool.roomObjPool[i].compSprite.position.X == RoomTwoThirdsX)
                        { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.WallStatue); }

                        else if (Pool.roomObjPool[i].compSprite.position.Y == RoomThirdY ||
                            Pool.roomObjPool[i].compSprite.position.Y == RoomTwoThirdsY)
                        { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.WallStatue); }

                        //the wall statue inherits the proper direction from the original wall obj
                    }
                }
            }
        }

        public static void ScatterDebris(Room Room)
        {   
            for (i = 0; i < Pool.floorCount; i++)
            {
                if (Pool.floorPool[i].visible)
                {   //randomly scatter rock piles on floor tiles
                    //if (Functions_Random.Int(0, 100) > 95)
                    //{ Functions_Interaction.ScatterRockDebris(Pool.floorPool[i].position, false); }
                    if (Functions_Random.Int(0, 100) > 80)
                    {
                        Functions_RoomObject.SpawnRoomObj(ObjType.FloorDebrisBlood,
                            Pool.floorPool[i].position.X + Functions_Random.Int(-4, 4),
                            Pool.floorPool[i].position.Y + Functions_Random.Int(-4, 4),
                            Direction.Down);
                    }
                }
            }
        }

        public static void AddCrackedWalls(Room Room)
        {   //randomly change straight walls into cracked walls
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active && Pool.roomObjPool[i].type == ObjType.WallStraight)
                {
                    if (Functions_Random.Int(0, 100) > 85)
                    { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.WallStraightCracked); }
                }
            }
        }



        //removes overlapping objs from room

        static GameObject objA; //object we want to keep
        static GameObject objB; //object we want to remove
        static Boolean removeObjB;

        public static void CleanupRoom(Room Room)
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
                                    if (objB.type == ObjType.WallPillar) { removeObjB = false; }
                                }
                                else if (objB.type == ObjType.FloorDebrisBlood)
                                {
                                    removeObjB = true;
                                    //allow overlap with these objects
                                    if (objA.type == ObjType.ConveyorBeltOn) { removeObjB = false; }
                                    else if (objA.type == ObjType.ConveyorBeltOff) { removeObjB = false; }
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



        //rooms that are procedurally 'finished' - exit, shop, secret

        public static void FinishShopRoom(Room Room)
        {
            PlaceExit(Room);


            #region Place some test shop objects

            //bookcase
            Functions_RoomObject.SpawnRoomObj(ObjType.Bookcase1,
                5 * 16 + pos.X + 8,
                0 * 16 + pos.Y + 0, Direction.Down);
            //drawers
            Functions_RoomObject.SpawnRoomObj(ObjType.Bookcase2,
                7 * 16 + pos.X + 8,
                0 * 16 + pos.Y + 0, Direction.Down);

            #endregion


            //create all the vendors
            Functions_RoomObject.CreateVendor(ObjType.VendorItems, new Vector2(1 * 16 + pos.X + 8, 4 * 16 + pos.Y + 8));
            Functions_RoomObject.CreateVendor(ObjType.VendorPotions, new Vector2(4 * 16 + pos.X + 8, 4 * 16 + pos.Y + 8));
            Functions_RoomObject.CreateVendor(ObjType.VendorMagic, new Vector2(7 * 16 + pos.X + 8, 4 * 16 + pos.Y + 8));
            Functions_RoomObject.CreateVendor(ObjType.VendorWeapons, new Vector2(10 * 16 + pos.X + 8, 4 * 16 + pos.Y + 8));
            Functions_RoomObject.CreateVendor(ObjType.VendorArmor, new Vector2(13 * 16 + pos.X + 8, 4 * 16 + pos.Y + 8));
            Functions_RoomObject.CreateVendor(ObjType.VendorEquipment, new Vector2(16 * 16 + pos.X + 8, 4 * 16 + pos.Y + 8));

            //create story vendor
            Functions_RoomObject.SpawnRoomObj(ObjType.VendorStory,
                7 * 16 + pos.X + 8,
                8 * 16 + pos.Y + 0, Direction.Down);
        }

        public static void FinishExitRoom(Room Room)
        {
            PlaceExit(Room);

            //place decorative statues
            Functions_RoomObject.SpawnRoomObj(ObjType.BossStatue,
                3 * 16 + pos.X + 8, 2 * 16 + pos.Y + 8, Direction.Down);
            Functions_RoomObject.SpawnRoomObj(ObjType.BossStatue,
                7 * 16 + pos.X + 8, 2 * 16 + pos.Y + 8, Direction.Down);
            Functions_RoomObject.SpawnRoomObj(ObjType.BossStatue,
                3 * 16 + pos.X + 8, 4 * 16 + pos.Y + 8, Direction.Down);
            Functions_RoomObject.SpawnRoomObj(ObjType.BossStatue,
                7 * 16 + pos.X + 8, 4 * 16 + pos.Y + 8, Direction.Down);
            Functions_RoomObject.SpawnRoomObj(ObjType.BossStatue,
                3 * 16 + pos.X + 8, 6 * 16 + pos.Y + 8, Direction.Down);
            Functions_RoomObject.SpawnRoomObj(ObjType.BossStatue,
                7 * 16 + pos.X + 8, 6 * 16 + pos.Y + 8, Direction.Down);
        }

        public static void FinishSecretRoom(Room Room)
        {
            //what goes in a secret room? not chests
            //perhaps a secret vendor? or vendors?
        }

        public static void FinishBossRoom(Room Room)
        {   //spawn boss in center of room
            Functions_Actor.SpawnActor(ActorType.Boss, Room.center.X + 8, Room.center.Y + 8);
        }

        public static void FinishKeyRoom(Room Room)
        {
            for (i = 0; i < Pool.roomObjCount; i++)
            {   //loop thru all active roomObjects
                if (Pool.roomObjPool[i].active)
                {   //find any chest objects in the key room
                    if (Pool.roomObjPool[i].group == ObjGroup.Chest)
                    {   //check the dungeon.bigKey boolean to see if this chest should be filled
                        if (Level.bigKey)
                        { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.ChestEmpty); }
                        else //if hero has found the map, this chest is empty, else it has a key
                        { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.ChestKey); }
                    }
                }
            }
        }

        public static void FinishHubRoom(Room Room)
        {
            for (i = 0; i < Pool.roomObjCount; i++)
            {   //loop thru all active roomObjects
                if (Pool.roomObjPool[i].active)
                {   //find any chest objects in the hub room
                    if (Pool.roomObjPool[i].group == ObjGroup.Chest)
                    {   //check the dungeon.map boolean to see if this chest should be filled
                        if (Level.map)
                        { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.ChestEmpty); }
                        else //if hero has found the map, this chest is empty, else it has a map
                        { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.ChestMap); }
                    }
                    else if (Pool.roomObjPool[i].type == ObjType.DoorBoss)
                    {   //build the boss welcome mat (left)
                        Functions_RoomObject.SpawnRoomObj(ObjType.BossDecal,
                            Pool.roomObjPool[i].compSprite.position.X - 8,
                            Pool.roomObjPool[i].compSprite.position.Y + 16,
                            Direction.Down);
                        //build the boss welcome mat (right)
                        objRef = Functions_RoomObject.SpawnRoomObj(ObjType.BossDecal,
                            Pool.roomObjPool[i].compSprite.position.X + 8,
                            Pool.roomObjPool[i].compSprite.position.Y + 16,
                            Direction.Down);
                        objRef.compSprite.flipHorizontally = true;
                    }
                }
            }
        }



        //spawns RoomObjs + Entities in the CurrentRoom, based on the passed RoomXmlData
        //this happens for row, column, square, boss, key, hub roomTypes ONLY

        public static void BuildRoomXmlData(RoomXmlData RoomXmlData)
        {

            #region Create room objs & enemies

            if (RoomXmlData != null && RoomXmlData.objs.Count > 0)
            {
                for (i = 0; i < RoomXmlData.objs.Count; i++)
                {   
                    //we store roomObjs & projectiles in roomXmlData
                    //based on the roomXmlData.obj.type, get an entity or a roomObj
                    if(RoomXmlData.objs[i].type == ObjType.ProjectileSpikeBlock)
                    { objRef = Functions_Pool.GetEntity(); }
                    else { objRef = Functions_Pool.GetRoomObj(); }
                    
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
                        if (objRef.type == ObjType.SpawnEnemy1)
                        { Functions_Actor.SpawnActor(ActorType.Blob, objRef.compSprite.position); }
                        else if (objRef.type == ObjType.SpawnEnemy2)
                        { Functions_Actor.SpawnActor(ActorType.Blob, objRef.compSprite.position); }
                        if (objRef.type == ObjType.SpawnEnemy3)
                        { Functions_Actor.SpawnActor(ActorType.Blob, objRef.compSprite.position); }
                        if (objRef.type == ObjType.SpawnFairy)
                        { Functions_Actor.SpawnActor(ActorType.Fairy, objRef.compSprite.position); }
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