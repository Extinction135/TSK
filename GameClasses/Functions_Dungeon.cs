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

        public static Stopwatch stopWatch = new Stopwatch();
        public static TimeSpan time;
        public static int i;
        public static int j;
        
        public static ComponentSprite floorRef;
        public static GameObject objRef;
        public static Actor actorRef;
        public static Point pos;



        public static void BuildEmptyRoom(Room Room)
        {
            stopWatch.Reset(); stopWatch.Start();
            Functions_Pool.Reset(); //reset the pools + counter
            pos = Room.rec.Location; //shorten room's position reference


            //build floors, top bottom left and right walls
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
                        Functions_GameObject.Spawn(
                            ObjType.Dungeon_WallStraight,
                            i * 16 + pos.X + 8,
                            0 * 16 - 16 + pos.Y + 8,
                            Direction.Down);

                        if (i == 0)
                        {   //topleft corner
                            Functions_GameObject.Spawn(
                                ObjType.Dungeon_WallInteriorCorner,
                                -16 + pos.X + 8,
                                -16 + pos.Y + 8,
                                Direction.Down);
                        }
                        else if (i == Room.size.X - 1)
                        {   //topright corner
                            Functions_GameObject.Spawn(
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
                        Functions_GameObject.Spawn(
                            ObjType.Dungeon_WallStraight,
                            i * 16 + pos.X + 8,
                            Room.size.Y * 16 + pos.Y + 8,
                            Direction.Up);

                        if (i == 0)
                        {   //bottom left corner
                            Functions_GameObject.Spawn(
                                ObjType.Dungeon_WallInteriorCorner,
                                -16 + pos.X + 8,
                                Room.size.Y * 16 + pos.Y + 8,
                                Direction.Right);
                        }
                        else if (i == Room.size.X - 1)
                        {   //bottom right corner
                            Functions_GameObject.Spawn(
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
                        Functions_GameObject.Spawn(
                            ObjType.Dungeon_WallStraight,
                            i * 16 - 16 + pos.X + 8,
                            j * 16 + pos.Y + 8,
                            Direction.Right);
                    }
                    else if (i == Room.size.X - 1)
                    {   //right side
                        Functions_GameObject.Spawn(
                            ObjType.Dungeon_WallStraight,
                            i * 16 + 16 + pos.X + 8,
                            j * 16 + pos.Y + 8,
                            Direction.Left);
                    }

                    #endregion

                }
            }


            //update all roomObjs, then remove overlapping objs
            Functions_GameObject.AlignRoomObjs();
            Functions_Room.Cleanup(Room); //remove overlapping objs

            stopWatch.Stop(); time = stopWatch.Elapsed;
            DebugInfo.roomTime = time.Ticks;
            if (Flags.PrintOutput)
            { Debug.WriteLine("built " + Room.roomID + " room in " + time.Ticks + " ticks"); }
        }


        public static void BuildRoomFrom(RoomXmlData RoomXmlData)
        {
            //reset pool, get blank room, fill with floors + walls
            BuildEmptyRoom(Functions_Level.currentRoom);
            //set the floortile frames properly based on room.type
            SetFloors(Functions_Level.currentRoom);
            //change certain walls to doors based on collisions with Level.doors
            SetDoors(Functions_Level.currentRoom);
            //build the xml objects over the empty dungeon room
            Functions_Room.BuildRoomXmlData(RoomXmlData);
            //add decorative objs and check for torches/switches/etc..
            ProcedurallyFinish(Functions_Level.currentRoom);
            CheckForPuzzles(Functions_Level.currentRoom);
        }


        public static void SetFloors(Room Room)
        {
            for (i = 0; i < Pool.floorCount; i++)
            {   //set all the floor sprite's current frame based on the room.type
                //default dungeon floors to normal sprite
                Pool.floorPool[i].currentFrame = AnimationFrames.Dungeon_FloorNormal[0];
                //based on type, change the default floor sprite to special or boss
                if (Room.roomID == RoomID.Hub || Room.roomID == RoomID.Key)
                { Pool.floorPool[i].currentFrame = AnimationFrames.Dungeon_FloorSpecial[0]; }
                else if (Room.roomID == RoomID.Boss)
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
                                {
                                    Functions_GameObject_Dungeon.DecorateDoor(
                                        Pool.roomObjPool[i], ObjType.Dungeon_WallPillar);
                                }
                                else if (Pool.roomObjPool[i].type == ObjType.Dungeon_DoorOpen)
                                {
                                    Functions_GameObject_Dungeon.DecorateDoor(
                                        Pool.roomObjPool[i], ObjType.Dungeon_WallTorch);
                                }

                                //finally, override door types based on specific room.type
                                if (Room.roomID == RoomID.Boss)
                                {   
                                    //all doors inside boss room are trap doors (push hero + close)
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




        public static int g;
        public static void ShutDoors(Room Room)
        {   //convert ANY kind of door to a 1 way trap door in room
            for (g = 0; g < Pool.roomObjCount; g++)
            {
                if (Pool.roomObjPool[g].active)
                {   //note this is a GROUP check, so even bombable doors convert
                    if (Pool.roomObjPool[g].group == ObjGroup.Door)
                    {   
                        Functions_GameObject.SetType(
                            Pool.roomObjPool[g], 
                            ObjType.Dungeon_DoorTrap);
                    }
                }
            }
        }
        




        public static void AddDevDoors(Room Room)
        {
            //add temporary doors to this room, so hero can enter/exit it
            int posX = Room.rec.X;
            int posY = Room.rec.Y;
            int middleX = (Room.size.X / 2) * 16;
            int middleY = (Room.size.Y / 2) * 16;
            int width = Room.size.X * 16;
            int height = Room.size.Y * 16;

            //add and set NSEW door positions
            Level.doors.Add(new Door(new Point(posX + middleX, posY - 16))); //top
            Level.doors.Add(new Door(new Point(posX - 16, posY + middleY))); //left
            Level.doors.Add(new Door(new Point(posX + width, posY + middleY))); //right

            if (Room.roomID != RoomID.DEV_Exit) //exit rooms have exit objs on south wall, no door
            { Level.doors.Add(new Door(new Point(posX + middleX, posY + height))); } //bottom
        }

        public static void ProcedurallyFinish(Room Room)
        {   //Pass the room to the appropriate method for completion
            stopWatch.Reset(); stopWatch.Start();


            #region Special Rooms

            if (Room.roomID == RoomID.Exit)
            {
                PlaceExit(Room);
                AddCrackedWalls(Room);
                ScatterDebris(Room);
            }
            else if (Room.roomID == RoomID.Secret)
            {
                FinishSecretRoom(Room);
            }

            #endregion


            #region Critical Rooms (key, hub, boss)

            else if (Room.roomID == RoomID.Key)
            {
                FinishKeyRoom(Room);
                AddWallStatues(Room);
                AddCrackedWalls(Room);
                ScatterDebris(Room);
            }
            else if (Room.roomID == RoomID.Hub)
            {
                FinishHubRoom(Room);
                AddWallStatues(Room);
                AddCrackedWalls(Room);
                ScatterDebris(Room);
            }
            else if (Room.roomID == RoomID.Boss)
            {
                ShutDoors(Room);
                FinishBossRoom(Room);
                AddCrackedWalls(Room);
                ScatterDebris(Room);
            }

            #endregion


            #region Standard Rooms (column, row, square)

            else if (Room.roomID == RoomID.Column)
            {
                AddWallStatues(Room);
                AddCrackedWalls(Room);
                ScatterDebris(Room);
            }
            else if (Room.roomID == RoomID.Row)
            {
                AddWallStatues(Room);
                AddCrackedWalls(Room);
                ScatterDebris(Room);
            }
            else if (Room.roomID == RoomID.Square)
            {
                AddWallStatues(Room);
                AddCrackedWalls(Room);
                ScatterDebris(Room);
            }

            #endregion


            #region Dev Rooms

            else if(Room.roomID == RoomID.DEV_Column
                || Room.roomID == RoomID.DEV_Row
                || Room.roomID == RoomID.DEV_Square)
            {
                //nothing
            }
            else if(Room.roomID == RoomID.DEV_Boss)
            {
                //FinishBossRoom(Room); //this just spawns the boss
                ShutDoors(Room); //we do want to shut the doors tho
            }
            else if(Room.roomID == RoomID.DEV_Exit)
            {
                PlaceExit(Room);
            }
            else if(Room.roomID == RoomID.DEV_Hub)
            {
                FinishHubRoom(Room);
            }
            else if (Room.roomID == RoomID.DEV_Key)
            {
                FinishKeyRoom(Room);
            }

            #endregion


            //always check any built room for puzzles
            CheckForPuzzles(Room);

            //align + remove overlapping objs
            Functions_GameObject.AlignRoomObjs();
            Functions_Room.Cleanup(Room);
            Assets.Play(Assets.sfxDoorOpen); //play door sfx

            stopWatch.Stop(); time = stopWatch.Elapsed;
            DebugInfo.roomTime += time.Ticks; //add finish time to roomTime
            if (Flags.PrintOutput)
            {
                Debug.WriteLine("finished " + Room.roomID +
                    " room (id:" + Room.XMLid +
                    ") in " + time.Ticks + " ticks");
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
                        Functions_GameObject_Dungeon.CloseDoors(); //convert all openDoors to trapDoors
                        return;
                    }
                    else if (Pool.roomObjPool[i].type == ObjType.Dungeon_TorchUnlit)
                    { torchCount++; } //count all the unlit torches
                }
            }
            //check for more than 3 torches
            if (torchCount > 3)
            {   //convert all openDoors to trapDoors
                Room.puzzleType = PuzzleType.Torches;
                Functions_GameObject_Dungeon.CloseDoors();
            }
        }



        public static void PlaceExit(Room Room)
        {
            //create exit pillars
            Functions_GameObject.Spawn(ObjType.Dungeon_ExitPillarLeft,
                (Room.size.X / 2) * 16 + pos.X + 8 - 16,
                Room.size.Y * 16 + pos.Y + 8 - 16 * 2,
                Direction.Down);
            Functions_GameObject.Spawn(ObjType.Dungeon_ExitPillarRight,
                (Room.size.X / 2) * 16 + pos.X + 8 + 16,
                Room.size.Y * 16 + pos.Y + 8 - 16 * 2,
                Direction.Down);
            //create exit light fx
            Functions_GameObject.Spawn(ObjType.Dungeon_ExitLight,
                (Room.size.X / 2) * 16 + pos.X + 8,
                Room.size.Y * 16 + pos.Y - 16 * 1,
                Direction.Down);

            //if we're developing an exit room, don't place real exit obj
            if (Room.roomID == RoomID.DEV_Exit) { return; }
            //create the actual dungeon exit
            Functions_GameObject.Spawn(ObjType.Dungeon_Exit,
                (Room.size.X / 2) * 16 + pos.X + 8,
                Room.size.Y * 16 + pos.Y + 8 - 16 * 2,
                Direction.Down);
            //set the room.spawnPos to pos above exit door obj
            Room.spawnPos.X = (Room.size.X / 2) * 16 + Room.rec.X + 8;
            Room.spawnPos.Y = Room.rec.Y + (Room.size.Y - 1) * 16;
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
                        Functions_GameObject.Spawn(ObjType.Dungeon_FloorBlood,
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

        //room specific procedural objects 

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
                        Functions_GameObject.Spawn(ObjType.Dungeon_FloorDecal,
                            Pool.roomObjPool[i].compSprite.position.X - 8,
                            Pool.roomObjPool[i].compSprite.position.Y + 16,
                            Direction.Down);
                        //build the boss welcome mat (right)
                        objRef = Functions_GameObject.Spawn(ObjType.Dungeon_FloorDecal,
                            Pool.roomObjPool[i].compSprite.position.X + 8,
                            Pool.roomObjPool[i].compSprite.position.Y + 16,
                            Direction.Down);
                        objRef.compSprite.flipHorizontally = true;
                    }
                }
            }
        }

    }
}