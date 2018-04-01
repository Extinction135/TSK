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
                            ObjType.Dungeon_WallStraight,
                            i * 16 + pos.X + 8, 
                            0 * 16 - 16 + pos.Y + 8, 
                            Direction.Down);

                        if (i == 0)
                        {   //topleft corner
                            Functions_RoomObject.SpawnRoomObj(
                                ObjType.Dungeon_WallInteriorCorner,
                                -16 + pos.X + 8, 
                                -16 + pos.Y + 8, 
                                Direction.Down);
                        }
                        else if (i == Room.size.X - 1)
                        {   //topright corner
                            Functions_RoomObject.SpawnRoomObj(
                                ObjType.Dungeon_WallInteriorCorner,
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
                            ObjType.Dungeon_WallStraight,
                            i * 16 + pos.X + 8,
                            Room.size.Y * 16 + pos.Y + 8,
                            Direction.Up);

                        if (i == 0)
                        {   //bottom left corner
                            Functions_RoomObject.SpawnRoomObj(
                                ObjType.Dungeon_WallInteriorCorner,
                                -16 + pos.X + 8,
                                Room.size.Y * 16 + pos.Y + 8,
                                Direction.Right);
                        }
                        else if (i == Room.size.X - 1)
                        {   //bottom right corner
                            Functions_RoomObject.SpawnRoomObj(
                                ObjType.Dungeon_WallInteriorCorner,
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
                            ObjType.Dungeon_WallStraight,
                            i * 16 - 16 + pos.X + 8,
                            j * 16 + pos.Y + 8,
                            Direction.Right);
                    }
                    else if (i == Room.size.X - 1)
                    {   //right side
                        Functions_RoomObject.SpawnRoomObj(
                            ObjType.Dungeon_WallStraight,
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
            Functions_RoomObject.AlignRoomObjs();
            CleanupRoom(Room); //remove overlapping objs
            Functions_Hero.ResetUponRoomBuild(); //reset hero

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
                CheckForPuzzles(Room);
            }
            else if (Room.type == RoomType.Hub)
            {
                BuildRoomXmlData(Assets.roomDataHub[Room.XMLid]);
                FinishHubRoom(Room);
                AddWallStatues(Room);
                CheckForPuzzles(Room);
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
                CheckForPuzzles(Room);
            }
            else if (Room.type == RoomType.Row)
            {
                BuildRoomXmlData(Assets.roomDataRow[Room.XMLid]);
                AddWallStatues(Room);
                CheckForPuzzles(Room);
            }
            else if (Room.type == RoomType.Square)
            {
                BuildRoomXmlData(Assets.roomDataSquare[Room.XMLid]);
                AddWallStatues(Room);
                CheckForPuzzles(Room);
            }
            //dungeon rooms get debris + cracked walls
            if (Room.type != RoomType.Shop)
            {
                ScatterDebris(Room);
                AddCrackedWalls(Room);
            }
            //align + remove overlapping objs
            Functions_RoomObject.AlignRoomObjs();
            CleanupRoom(Room);
            Assets.Play(Assets.sfxDoorOpen); //play door sfx

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
                //default dungeon floors to normal sprite
                Pool.floorPool[i].currentFrame = AnimationFrames.Dungeon_FloorNormal[0];
                //based on type, change the default floor sprite to special or boss
                if (Room.type == RoomType.Hub || Room.type == RoomType.Key)
                { Pool.floorPool[i].currentFrame = AnimationFrames.Dungeon_FloorSpecial[0]; }
                else if (Room.type == RoomType.Boss)
                { Pool.floorPool[i].currentFrame = AnimationFrames.Dungeon_FloorBoss[0]; }
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
                                { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.Dungeon_DoorBombable); }

                                else if (Level.doors[j].type == DoorType.Boss)
                                { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.Dungeon_DoorBoss); }

                                else //all other doorTypes are Open
                                { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.Dungeon_DoorOpen); }

                                //set the door decorations (bombed/bombable doors dont get decorations)
                                if (Pool.roomObjPool[i].type == ObjType.Dungeon_DoorBoss)
                                { Functions_RoomObject.DecorateDoor(Pool.roomObjPool[i], ObjType.Dungeon_WallPillar); }
                                else if (Pool.roomObjPool[i].type == ObjType.Dungeon_DoorOpen)
                                { Functions_RoomObject.DecorateDoor(Pool.roomObjPool[i], ObjType.Dungeon_WallTorch); }

                                //finally, override door types based on specific room.type
                                if (Room.type == RoomType.Boss)
                                {   //all doors inside boss room are trap doors (push hero + close)
                                    Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.Dungeon_DoorTrap);
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

        



        //adds/changes objs in room 

        public static void PlaceExit(Room Room)
        {
            //create the exit
            Functions_RoomObject.SpawnRoomObj(ObjType.Dungeon_Exit,
                (Room.size.X / 2) * 16 + pos.X + 8,
                Room.size.Y * 16 + pos.Y + 8 - 16 * 2, 
                Direction.Down);

            //set the room.spawnPos to pos above exit door obj
            Room.spawnPos.X = (Room.size.X / 2) * 16 + Room.rec.X + 8;
            Room.spawnPos.Y = Room.rec.Y + (Room.size.Y - 1) * 16;

            //place the exit light fx over exit obj
            Functions_RoomObject.SpawnRoomObj(ObjType.Dungeon_ExitLight,
                (Room.size.X / 2) * 16 + pos.X + 8,
                Room.size.Y * 16 + pos.Y + 8 - 16 * 1,
                Direction.Down);

            //create exit pillars
            Functions_RoomObject.SpawnRoomObj(ObjType.Dungeon_ExitPillarLeft,
                (Room.size.X / 2) * 16 + pos.X + 8 - 16,
                Room.size.Y * 16 + pos.Y + 8 - 16 * 2,
                Direction.Down);
            Functions_RoomObject.SpawnRoomObj(ObjType.Dungeon_ExitPillarRight,
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
                    if (Pool.roomObjPool[i].type == ObjType.Dungeon_WallStraight)
                    {
                        if (Pool.roomObjPool[i].compSprite.position.X == RoomThirdX ||
                            Pool.roomObjPool[i].compSprite.position.X == RoomTwoThirdsX)
                        { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.Dungeon_WallStatue); }

                        else if (Pool.roomObjPool[i].compSprite.position.Y == RoomThirdY ||
                            Pool.roomObjPool[i].compSprite.position.Y == RoomTwoThirdsY)
                        { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.Dungeon_WallStatue); }

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
                        Functions_RoomObject.SpawnRoomObj(ObjType.Dungeon_FloorBlood,
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
                if (Pool.roomObjPool[i].active && Pool.roomObjPool[i].type == ObjType.Dungeon_WallStraight)
                {
                    if (Functions_Random.Int(0, 100) > 85)
                    { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.Dungeon_WallStraightCracked); }
                }
            }
        }

        public static void CheckForPuzzles(Room Room)
        {   //this is called at the end of a room build
            int torchCount = 0;

            for (i = 0; i < Pool.roomObjCount; i++)
            {   
                if (Pool.roomObjPool[i].active)
                {   //if there is an active switch in the room
                    if (Pool.roomObjPool[i].type == ObjType.Dungeon_Switch)
                    {   //set the room puzzleType and bail from method
                        Room.puzzleType = PuzzleType.Switch;
                        Functions_RoomObject.CloseDoors(); //convert all openDoors to trapDoors
                        return;
                    }
                    else if(Pool.roomObjPool[i].type == ObjType.Dungeon_TorchUnlit)
                    { torchCount++; } //count all the unlit torches
                }
            }
            //check for more than 3 torches
            if (torchCount > 3)
            {   //convert all openDoors to trapDoors
                Room.puzzleType = PuzzleType.Torches;
                Functions_RoomObject.CloseDoors(); 
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



        //rooms that are procedurally 'finished' - exit, shop, secret

        public static void FinishShopRoom(Room Room)
        {
            PlaceExit(Room);


            #region Place some test shop objects

            //bookcase
            Functions_RoomObject.SpawnRoomObj(ObjType.World_Bookcase,
                5 * 16 + pos.X + 8,
                0 * 16 + pos.Y + 0, Direction.Down);
            //drawers
            Functions_RoomObject.SpawnRoomObj(ObjType.World_Shelf,
                7 * 16 + pos.X + 8,
                0 * 16 + pos.Y + 0, Direction.Down);

            #endregion


            //create all the vendors
            Functions_RoomObject.CreateVendor(ObjType.Vendor_NPC_Items, 
                new Vector2(1 * 16 + pos.X + 8, 4 * 16 + pos.Y + 8));

            Functions_RoomObject.CreateVendor(ObjType.Vendor_NPC_Potions, 
                new Vector2(4 * 16 + pos.X + 8, 4 * 16 + pos.Y + 8));

            Functions_RoomObject.CreateVendor(ObjType.Vendor_NPC_Magic, 
                new Vector2(7 * 16 + pos.X + 8, 4 * 16 + pos.Y + 8));

            Functions_RoomObject.CreateVendor(ObjType.Vendor_NPC_Weapons, 
                new Vector2(10 * 16 + pos.X + 8, 4 * 16 + pos.Y + 8));

            Functions_RoomObject.CreateVendor(ObjType.Vendor_NPC_Armor, 
                new Vector2(13 * 16 + pos.X + 8, 4 * 16 + pos.Y + 8));

            Functions_RoomObject.CreateVendor(ObjType.Vendor_NPC_Equipment, 
                new Vector2(16 * 16 + pos.X + 8, 4 * 16 + pos.Y + 8));

            //add pet vendor
            Functions_RoomObject.CreateVendor(ObjType.Vendor_NPC_Pets, 
                new Vector2(16 * 16 + pos.X + 8, 7 * 16 + pos.Y + 8));

            //create story vendor
            Functions_RoomObject.SpawnRoomObj(ObjType.Vendor_NPC_Story,
                7 * 16 + pos.X + 8,
                8 * 16 + pos.Y + 0, Direction.Down);
        }

        public static void FinishExitRoom(Room Room)
        {
            PlaceExit(Room);

            //place decorative statues
            Functions_RoomObject.SpawnRoomObj(ObjType.Dungeon_Statue,
                3 * 16 + pos.X + 8, 2 * 16 + pos.Y + 8, Direction.Down);
            Functions_RoomObject.SpawnRoomObj(ObjType.Dungeon_Statue,
                7 * 16 + pos.X + 8, 2 * 16 + pos.Y + 8, Direction.Down);
            Functions_RoomObject.SpawnRoomObj(ObjType.Dungeon_Statue,
                3 * 16 + pos.X + 8, 4 * 16 + pos.Y + 8, Direction.Down);
            Functions_RoomObject.SpawnRoomObj(ObjType.Dungeon_Statue,
                7 * 16 + pos.X + 8, 4 * 16 + pos.Y + 8, Direction.Down);
            Functions_RoomObject.SpawnRoomObj(ObjType.Dungeon_Statue,
                3 * 16 + pos.X + 8, 6 * 16 + pos.Y + 8, Direction.Down);
            Functions_RoomObject.SpawnRoomObj(ObjType.Dungeon_Statue,
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
                        { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.Dungeon_ChestEmpty); }
                        else //if hero has found the map, this chest is empty, else it has a key
                        { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.Dungeon_ChestKey); }
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
                        { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.Dungeon_ChestEmpty); }
                        else //if hero has found the map, this chest is empty, else it has a map
                        { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.Dungeon_ChestMap); }
                    }
                    else if (Pool.roomObjPool[i].type == ObjType.Dungeon_DoorBoss)
                    {   //build the boss welcome mat (left)
                        Functions_RoomObject.SpawnRoomObj(ObjType.Dungeon_FloorDecal,
                            Pool.roomObjPool[i].compSprite.position.X - 8,
                            Pool.roomObjPool[i].compSprite.position.Y + 16,
                            Direction.Down);
                        //build the boss welcome mat (right)
                        objRef = Functions_RoomObject.SpawnRoomObj(ObjType.Dungeon_FloorDecal,
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