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

        public static InteractiveObject intRef;
        public static IndestructibleObject indRef;

        public static Actor actorRef;
        public static Point pos;
        static int torchCount;




        //Room Management Routines

        public static void SetType(Room Room, RoomID ID)
        {
            Room.roomID = ID;
            
            //set room size based on type - sizes should be odd, so doors/exits can be centered
            if (ID == RoomID.ForestIsland_ExitRoom || ID == RoomID.DEV_Exit)
            {
                Room.size.X = 11; Room.size.Y = 11;
            }
            else if (
                ID == RoomID.ForestIsland_HubRoom ||
                ID == RoomID.DEV_Hub)
            {
                Room.size.X = 19; Room.size.Y = 19;
            }
            else if (
                ID == RoomID.ForestIsland_BossRoom ||
                ID == RoomID.DEV_Boss)
            {
                Room.size.X = 19; Room.size.Y = 11;
            }
            else if (ID == RoomID.ForestIsland_KeyRoom || ID == RoomID.DEV_Key)
            {
                Room.size.X = 19; Room.size.Y = 11;
            }
            
            //dungeon rooms
            else if (ID == RoomID.ForestIsland_ColumnRoom || ID == RoomID.DEV_Column)
            {
                Room.size.X = 11; Room.size.Y = 19;
            }
            else if (ID == RoomID.ForestIsland_RowRoom || ID == RoomID.DEV_Row)
            {
                Room.size.X = 19; Room.size.Y = 11;
            }
            else if (ID == RoomID.ForestIsland_SquareRoom || ID == RoomID.DEV_Square)
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
                Room.size.X = 40*2; Room.size.Y = 23*2; //double sized
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








        //Dungeon Room Building Method

        public static void BuildRoom(Room Room, RoomXmlData RoomXmlData = null)
        {
            if (Flags.PrintOutput)
            { Debug.WriteLine("-- building room: " + Room.roomID + " --"); }
            stopWatch.Reset(); stopWatch.Start();



            //Setup RoomData OR DevRooms

            if (RoomXmlData == null)
            {


                //overworld levels

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


                #region Setup DeathMountain Rooms (Levels)

                else if (Room.roomID == RoomID.DeathMountain_MainEntrance)
                {
                    RoomXmlData = LevelData_DeathMountain.DeathMountain_MainEntrance;
                    LevelSet.currentLevel.isField = true;
                }

                #endregion


                #region Setup ForestIsland Rooms (Levels)

                else if (Room.roomID == RoomID.ForestIsland_MainEntrance)
                {
                    RoomXmlData = LevelData_ForestIsland.ForestIsland_MainEntrance;
                    LevelSet.currentLevel.isField = true;
                }

                #endregion


                #region Setup LavaIsland Rooms (Levels)

                else if (Room.roomID == RoomID.LavaIsland_MainEntrance)
                {
                    RoomXmlData = LevelData_LavaIsland.LavaIsland_MainEntrance;
                    LevelSet.currentLevel.isField = true;
                }

                #endregion


                #region Setup CloudIsland Rooms (Levels)

                else if (Room.roomID == RoomID.CloudIsland_MainEntrance)
                {
                    RoomXmlData = LevelData_CloudIsland.CloudIsland_MainEntrance;
                    LevelSet.currentLevel.isField = true;
                }

                #endregion


                #region Setup SwampIsland Rooms (Levels)

                else if (Room.roomID == RoomID.SwampIsland_MainEntrance)
                {
                    RoomXmlData = LevelData_HauntedSwamps.SwampIsland_MainEntrance;
                    LevelSet.currentLevel.isField = true;
                }

                #endregion


                #region Setup Thieves Den Rooms (Levels)

                else if (Room.roomID == RoomID.ThievesDen_GateEntrance)
                {
                    RoomXmlData = LevelData_ThievesHideout.ThievesDen_GateEntrance;
                    LevelSet.currentLevel.isField = true;
                }

                #endregion



                //dungeon rooms

                #region Forest Roomdata

                //load the xml room data from it's dataIndex, or basd on it's ID for exit/hub/boss
                else if (Room.roomID == RoomID.ForestIsland_ColumnRoom)
                {
                    RoomXmlData = RoomData_SkullIsland_Columns.Data[Room.dataIndex];
                    LevelSet.currentLevel.isField = false;
                }
                else if (Room.roomID == RoomID.ForestIsland_KeyRoom)
                {
                    RoomXmlData = RoomData_SkullIsland_Key.Data[Room.dataIndex];
                    LevelSet.currentLevel.isField = false;
                }
                else if (Room.roomID == RoomID.ForestIsland_RowRoom)
                {
                    RoomXmlData = RoomData_SkullIsland_Row.Data[Room.dataIndex];
                    LevelSet.currentLevel.isField = false;
                }
                else if (Room.roomID == RoomID.ForestIsland_SquareRoom)
                {
                    RoomXmlData = RoomData_SkullIsland_Square.Data[Room.dataIndex];
                    LevelSet.currentLevel.isField = false;
                }

                else if (
                    Room.roomID == RoomID.ForestIsland_ExitRoom
                    || Room.roomID == RoomID.ForestIsland_BossRoom
                    || Room.roomID == RoomID.ForestIsland_HubRoom
                    )
                {
                    LevelSet.currentLevel.isField = false;
                    for (i = 0; i < RoomData_SkullIsland_ExitBossHub.Data.Count; i++)
                    {   //match room to it's ID
                        if (RoomData_SkullIsland_ExitBossHub.Data[i].type == Room.roomID)
                        { RoomXmlData = RoomData_SkullIsland_ExitBossHub.Data[i]; }
                    }
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
                AddDevDoors(Room);
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
                LevelSet.currentLevel.currentRoom = LevelSet.field.rooms[0];
            }
            else
            {   //else, we are building an interior dungeon room
                BuildRoomFrom(RoomXmlData);
                //current room is set by hero in this case
            }

            #endregion


            #region Handle room specific initial events (like setting music)

            if (
                Room.roomID == RoomID.ForestIsland_BossRoom
                )
            {
                Assets.Play(Assets.sfxBossIntro);
                Functions_Music.PlayMusic(Music.Boss);
            }

            #endregion



            stopWatch.Stop(); time = stopWatch.Elapsed;
            if (Flags.PrintOutput)
            { Debug.WriteLine("room " + Room.roomID + " built in " + time.Ticks + " ticks"); }
        }



        //Field Room Building Method

        public static void BuildRoomXmlData(RoomXmlData RoomXmlData = null)
        {
            //note that RoomXmlData is an optional parameter
            //we have two lists now, ints and inds
            //which means we need to getObjs() diff, and put them on diff lists too

            //create ints and ind objects based on room data xml
            if(RoomXmlData != null)
            {
                //create indestructibles
                if(RoomXmlData.inds.Count > 0)
                {
                    for (i = 0; i < RoomXmlData.inds.Count; i++)
                    {
                        indRef = Functions_Pool.GetIndObj();
                        //move roomObj to xmlObj's position (with room offset)
                        indRef.compSprite.position.X = LevelSet.currentLevel.currentRoom.rec.X + RoomXmlData.inds[i].posX;
                        indRef.compSprite.position.Y = LevelSet.currentLevel.currentRoom.rec.Y + RoomXmlData.inds[i].posY;
                        Functions_Component.Align(indRef);
                        indRef.direction = RoomXmlData.inds[i].direction;
                        Functions_IndestructibleObjs.SetType(indRef, RoomXmlData.inds[i].type);
                    }
                }

                //create interactives
                if (RoomXmlData.ints.Count > 0)
                {
                    for (i = 0; i < RoomXmlData.ints.Count; i++)
                    {
                        intRef = Functions_Pool.GetIntObj();
                        //move roomObj to xmlObj's position (with room offset)
                        Functions_Movement.Teleport(intRef.compMove,
                            LevelSet.currentLevel.currentRoom.rec.X + RoomXmlData.ints[i].posX,
                            LevelSet.currentLevel.currentRoom.rec.Y + RoomXmlData.ints[i].posY);
                        Functions_Component.Align(intRef);
                        intRef.direction = RoomXmlData.ints[i].direction;
                        Functions_InteractiveObjs.SetType(intRef, RoomXmlData.ints[i].type);


                        //create enemies at enemySpawn obj locations
                        if (intRef.group == InteractiveGroup.EnemySpawn)
                        {   //here we check level.id to determine what type of STANDARD enemy to spawn
                            if (LevelSet.currentLevel.ID == LevelID.Forest_Dungeon)
                            { Functions_Actor.SpawnActor(ActorType.Standard_AngryEye, intRef.compSprite.position); }

                            /*
                            else if (LevelSet.currentLevel.ID == LevelID.Mountain_Dungeon)
                            { Functions_Actor.SpawnActor(ActorType.Standard_BeefyBat, intRef.compSprite.position); }
                            else if (LevelSet.currentLevel.ID == LevelID.Swamp_Dungeon)
                            { Functions_Actor.SpawnActor(ActorType.Blob, intRef.compSprite.position); }
                            */

                            else //any other dungeon spawns blobs
                            { Functions_Actor.SpawnActor(ActorType.Blob, intRef.compSprite.position); }
                        }
                    }
                }
            }



            #region Check enemySpawn obj visibility

            if (Flags.ShowEnemySpawns == false)
            {   //find any spawnObj, set obj.active = false
                for (i = 0; i < Pool.intObjCount; i++)
                {
                    if (Pool.intObjPool[i].group == InteractiveGroup.EnemySpawn)
                    { Pool.intObjPool[i].active = false; }
                }
            }

            #endregion

        }


























        //procedural room methods

        public static void BuildEmptyRoom(Room Room)
        {
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
                        Functions_InteractiveObjs.Spawn(
                            InteractiveType.Dungeon_WallStraight,
                            i * 16 + pos.X + 8,
                            0 * 16 - 16 + pos.Y + 8,
                            Direction.Down);

                        if (i == 0)
                        {   //topleft corner
                            Functions_InteractiveObjs.Spawn(
                                InteractiveType.Dungeon_WallInteriorCorner,
                                -16 + pos.X + 8,
                                -16 + pos.Y + 8,
                                Direction.Down);
                        }
                        else if (i == Room.size.X - 1)
                        {   //topright corner
                            Functions_InteractiveObjs.Spawn(
                                InteractiveType.Dungeon_WallInteriorCorner,
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
                        Functions_InteractiveObjs.Spawn(
                            InteractiveType.Dungeon_WallStraight,
                            i * 16 + pos.X + 8,
                            Room.size.Y * 16 + pos.Y + 8,
                            Direction.Up);

                        if (i == 0)
                        {   //bottom left corner
                            Functions_InteractiveObjs.Spawn(
                                InteractiveType.Dungeon_WallInteriorCorner,
                                -16 + pos.X + 8,
                                Room.size.Y * 16 + pos.Y + 8,
                                Direction.Right);
                        }
                        else if (i == Room.size.X - 1)
                        {   //bottom right corner
                            Functions_InteractiveObjs.Spawn(
                                InteractiveType.Dungeon_WallInteriorCorner,
                                Room.size.X * 16 + pos.X + 8,
                                Room.size.Y * 16 + pos.Y + 8,
                                Direction.Up);
                        }
                    }

                    #endregion


                    #region Left & Right Column Walls

                    if (i == 0)
                    {   //left side
                        Functions_InteractiveObjs.Spawn(
                            InteractiveType.Dungeon_WallStraight,
                            i * 16 - 16 + pos.X + 8,
                            j * 16 + pos.Y + 8,
                            Direction.Right);
                    }
                    else if (i == Room.size.X - 1)
                    {   //right side
                        Functions_InteractiveObjs.Spawn(
                            InteractiveType.Dungeon_WallStraight,
                            i * 16 + 16 + pos.X + 8,
                            j * 16 + pos.Y + 8,
                            Direction.Left);
                    }

                    #endregion

                }
            }
        }

        public static void BuildRoomFrom(RoomXmlData RoomXmlData)
        {
            //reset pool, get blank room, fill with floors + walls
            BuildEmptyRoom(LevelSet.dungeon.currentRoom);
            //set the floortile frames properly based on room.type
            SetFloors(LevelSet.dungeon.currentRoom);
            //change certain walls to doors based on collisions with Level.doors
            SetDoors(LevelSet.dungeon.currentRoom);
            //build the xml objects over the empty dungeon room
            Functions_Room.BuildRoomXmlData(RoomXmlData);
            //add decorative objs and check for torches/switches/etc..
            ProcedurallyFinish(LevelSet.dungeon.currentRoom);
            //always call the puzzle setup routine
            SetupPuzzle(LevelSet.dungeon.currentRoom);
            Assets.Play(Assets.sfxDoorOpen); //play door sfx
        }



        public static void SetFloors(Room Room)
        {
            for (i = 0; i < Pool.floorCount; i++)
            {   //set all the floor sprite's current frame based on the room.type
                //default dungeon floors to normal sprite
                Pool.floorPool[i].currentFrame = AnimationFrames.Dungeon_FloorNormal[0];
                //based on type, change the default floor sprite to special or boss
                if (
                    Room.roomID == RoomID.DEV_Key ||
                    Room.roomID == RoomID.ForestIsland_KeyRoom ||
                    Room.roomID == RoomID.ForestIsland_HubRoom
                    )
                { Pool.floorPool[i].currentFrame = AnimationFrames.Dungeon_FloorSpecial[0]; }
                else if (
                    Room.roomID == RoomID.DEV_Boss ||
                    Room.roomID == RoomID.ForestIsland_BossRoom
                    )
                { Pool.floorPool[i].currentFrame = AnimationFrames.Dungeon_FloorBoss[0]; }
            }
        }

        public static void SetDoors(Room Room)
        {
            for (i = 0; i < Pool.intObjCount; i++)
            {
                if (Pool.intObjPool[i].active)
                {
                    if (Pool.intObjPool[i].group == InteractiveGroup.Wall_Dungeon)
                    {   //check to see if wall collides with any door from dungeon
                        for (j = 0; j < LevelSet.currentLevel.doors.Count; j++)
                        {
                            if (Pool.intObjPool[i].compCollision.rec.Contains(LevelSet.currentLevel.doors[j].rec.Location))
                            {
                                //set the room's doors based on the dungeon.door.type
                                if (LevelSet.currentLevel.doors[j].type == DoorType.Bombable)
                                { Functions_InteractiveObjs.SetType(Pool.intObjPool[i], InteractiveType.Dungeon_DoorBombable); }

                                else if (LevelSet.currentLevel.doors[j].type == DoorType.Boss)
                                { Functions_InteractiveObjs.SetType(Pool.intObjPool[i], InteractiveType.Dungeon_DoorBoss); }

                                else //all other doorTypes are Open
                                { Functions_InteractiveObjs.SetType(Pool.intObjPool[i], InteractiveType.Dungeon_DoorOpen); }

                                //set the door decorations (bombed/bombable doors dont get decorations)
                                if (Pool.intObjPool[i].type == InteractiveType.Dungeon_DoorBoss)
                                {
                                    DecorateDoor(Pool.intObjPool[i], InteractiveType.Dungeon_WallPillar);
                                }
                                else if (Pool.intObjPool[i].type == InteractiveType.Dungeon_DoorOpen)
                                {
                                    DecorateDoor(Pool.intObjPool[i], InteractiveType.Dungeon_WallTorch);
                                }

                                //finally, override door types based on specific room.type
                                if (
                                    Room.roomID == RoomID.ForestIsland_BossRoom
                                    )
                                {
                                    //all doors inside boss room are trap doors (push hero + close)
                                    Functions_InteractiveObjs.SetType(Pool.intObjPool[i], InteractiveType.Dungeon_DoorTrap);
                                }

                                //sort door object
                                Functions_Component.SetZdepth(Pool.intObjPool[i].compSprite);
                                //place a floor tile underneath door
                                floorRef = Functions_Pool.GetFloor();
                                floorRef.position = Pool.intObjPool[i].compSprite.position;
                            }
                        }
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
            LevelSet.currentLevel.doors.Add(new Door(new Point(posX + middleX, posY - 16))); //top
            LevelSet.currentLevel.doors.Add(new Door(new Point(posX - 16, posY + middleY))); //left
            LevelSet.currentLevel.doors.Add(new Door(new Point(posX + width, posY + middleY))); //right

            if (Room.roomID != RoomID.DEV_Exit) //exit rooms have exit objs on south wall, no door
            { LevelSet.currentLevel.doors.Add(new Door(new Point(posX + middleX, posY + height))); } //bottom
        }





        //misc

        public static void ProcedurallyFinish(Room Room)
        {   //Pass the room to the appropriate method for completion
            
            if (Room.roomID == RoomID.Secret)
            {
                FinishSecretRoom(Room);
            }


            #region Critical Rooms (exit, key, hub, boss)

            else if (
                Room.roomID == RoomID.DEV_Exit
                || Room.roomID == RoomID.ForestIsland_ExitRoom
                )
            {
                PlaceExit(Room);
            }

            else if (
                Room.roomID == RoomID.DEV_Key
                || Room.roomID == RoomID.ForestIsland_KeyRoom
                )
            {
                FinishKeyRoom(Room);
                AddCrackedWalls(Room);
                ScatterDebris(Room);
            }
            else if (
                Room.roomID == RoomID.DEV_Hub
                || Room.roomID == RoomID.ForestIsland_HubRoom
                )
            {
                //we dont want the miniboss to spawn in the editor
                //we can manually spawn him - he destroys the room
                if (Flags.bootRoutine == BootRoutine.Game) { SpawnMiniBoss(Room); }

                AddCrackedWalls(Room);
                ScatterDebris(Room);
            }
            else if (
                Room.roomID == RoomID.DEV_Boss
                || Room.roomID == RoomID.ForestIsland_BossRoom 
                )
            {
                ShutDoors(Room);
                //we dont want the boss to spawn in the editor
                //we can manually spawn him - he destroys the room
                if (Flags.bootRoutine == BootRoutine.Game) { SpawnBoss(Room); }
                //dont add wall statues, cause they damage boss
                //AddWallStatues(Room);
                AddCrackedWalls(Room);
                ScatterDebris(Room);
            }

            #endregion


            #region Standard Rooms (column, row, square)

            else if (
                Room.roomID == RoomID.DEV_Column
                || Room.roomID == RoomID.ForestIsland_ColumnRoom
                )
            {
                AddCrackedWalls(Room);
                ScatterDebris(Room);
            }
            else if (
                Room.roomID == RoomID.DEV_Row
                || Room.roomID == RoomID.ForestIsland_RowRoom
                )
            {
                AddCrackedWalls(Room);
                ScatterDebris(Room);
            }
            else if (
                Room.roomID == RoomID.DEV_Square
                || Room.roomID == RoomID.ForestIsland_SquareRoom
                )
            {
                AddCrackedWalls(Room);
                ScatterDebris(Room);
            }

            #endregion


            #region Dev Rooms

            else if (Room.roomID == RoomID.DEV_Column
                || Room.roomID == RoomID.DEV_Row
                || Room.roomID == RoomID.DEV_Square)
            {
                //nothing
            }
            else if (Room.roomID == RoomID.DEV_Boss)
            {
                ShutDoors(Room); //we do want to shut the doors tho
            }
            else if (Room.roomID == RoomID.DEV_Exit)
            {
                PlaceExit(Room);
            }
            else if (Room.roomID == RoomID.DEV_Hub)
            {
                //nothing at all
            }
            else if (Room.roomID == RoomID.DEV_Key)
            {
                FinishKeyRoom(Room);
            }

            #endregion


            //check to see if boss door exist in room, decorate
            Check_BossDoor();
            Functions_Pool.AlignIndObjs();
            Functions_Pool.AlignIntObjs();
        }

        public static void SetupPuzzle(Room Room)
        {
            //this is called at the end of a room build
            torchCount = 0;
            for (i = 0; i < Pool.intObjCount; i++)
            {
                if (Pool.intObjPool[i].active)
                {   //if there is an active switch in the room - this is a switch puzzle
                    if (Pool.intObjPool[i].type == InteractiveType.Dungeon_Switch)
                    {
                        //if autosolve cheat is enabled, convert switch to perm down version
                        if (Flags.AutoSolvePuzzle)
                        { Functions_InteractiveObjs.SetType(Pool.intObjPool[i], InteractiveType.Dungeon_SwitchDownPerm); }
                        //if game isn't in hard mode, and we already visited this level, convert it too
                        else if (Flags.HardMode == false & Room.visited == true)
                        { Functions_InteractiveObjs.SetType(Pool.intObjPool[i], InteractiveType.Dungeon_SwitchDownPerm); }

                        //if this room hasn't been visited, setup any puzzle it contains
                        //if (LevelSet.dungeon.currentRoom.visited == false) { }

                        //we haven't visited this room, or the game is in hard mode
                        else
                        {   //setup the switch puzzle
                            Room.puzzleType = PuzzleType.Switch;
                            Functions_InteractiveObjs.CloseDoors(); //convert all openDoors to trapDoors
                            i = Pool.intObjCount; //end loop
                        }
                    }
                    //count all the unlit torches
                    else if (Pool.intObjPool[i].type == InteractiveType.TorchUnlit)
                    { torchCount++; }
                }
            }

            //autosolve torch puzzles too
            if (Flags.AutoSolvePuzzle) { torchCount = 0; }
            //check for more than 3 torches, if 4 or more, then 4 need to be lit
            if (torchCount > 3)
            {   //convert all openDoors to trapDoors
                Room.puzzleType = PuzzleType.Torches;
                Functions_InteractiveObjs.CloseDoors();
            }   //torches > switches
        }

        public static void PlaceExit(Room Room)
        {
            //create exit pillars
            Functions_IndestructibleObjs.Spawn(IndestructibleType.Dungeon_ExitPillarLeft,
                (Room.size.X / 2) * 16 + pos.X + 8 - 16,
                Room.size.Y * 16 + pos.Y + 8 - 16 * 2,
                Direction.Down);
            Functions_IndestructibleObjs.Spawn(IndestructibleType.Dungeon_ExitPillarRight,
                (Room.size.X / 2) * 16 + pos.X + 8 + 16,
                Room.size.Y * 16 + pos.Y + 8 - 16 * 2,
                Direction.Down);
            //create exit light fx
            Functions_IndestructibleObjs.Spawn(IndestructibleType.Dungeon_ExitLight,
                (Room.size.X / 2) * 16 + pos.X + 8,
                Room.size.Y * 16 + pos.Y - 16 * 1,
                Direction.Down);

            //if we're developing an exit room, don't place real exit obj
            if (Room.roomID == RoomID.DEV_Exit) { return; }
            //create the actual dungeon exit
            Functions_IndestructibleObjs.Spawn(IndestructibleType.Dungeon_Exit,
                (Room.size.X / 2) * 16 + pos.X + 8,
                Room.size.Y * 16 + pos.Y + 8 - 16 * 2,
                Direction.Down);

            //set the hero's spawnPos to above exit door obj
            LevelSet.spawnPos_Dungeon.X = (Room.size.X / 2) * 16 + Room.rec.X + 8;
            LevelSet.spawnPos_Dungeon.Y = Room.rec.Y + (Room.size.Y - 1) * 16;
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
                        Functions_InteractiveObjs.Spawn(InteractiveType.FloorStain,
                            Pool.floorPool[i].position.X + Functions_Random.Int(-4, 4),
                            Pool.floorPool[i].position.Y + Functions_Random.Int(-4, 4),
                            Direction.Down);
                    }
                }
            }
        }

        public static void AddCrackedWalls(Room Room)
        {   //randomly change straight walls into cracked walls
            for (i = 0; i < Pool.intObjCount; i++)
            {
                if (Pool.intObjPool[i].active && Pool.intObjPool[i].type == InteractiveType.Dungeon_WallStraight)
                {
                    if (Functions_Random.Int(0, 100) > 85)
                    { Functions_InteractiveObjs.SetType(Pool.intObjPool[i], InteractiveType.Dungeon_WallStraightCracked); }
                }
            }
        }

        public static void Check_BossDoor()
        {
            //can be ANY room, dungeon recipes can attach boss to any room
            for (i = 0; i < Pool.intObjCount; i++)
            {   //loop thru all active roomObjects
                if (Pool.intObjPool[i].active)
                {
                    //create the boss welcome mat (zelda staple, grabs players attention)
                    if (Pool.intObjPool[i].type == InteractiveType.Dungeon_DoorBoss)
                    {   //build the boss welcome mat (left)
                        Functions_InteractiveObjs.Spawn(InteractiveType.Dungeon_FloorDecal,
                            Pool.intObjPool[i].compSprite.position.X - 8,
                            Pool.intObjPool[i].compSprite.position.Y + 16,
                            Direction.Down);
                        //build the boss welcome mat (right)
                        intRef = Functions_InteractiveObjs.Spawn(InteractiveType.Dungeon_FloorDecal,
                            Pool.intObjPool[i].compSprite.position.X + 8,
                            Pool.intObjPool[i].compSprite.position.Y + 16,
                            Direction.Down);
                        intRef.compSprite.flipHorizontally = true;
                    }
                }
            }
        }

        public static void FinishSecretRoom(Room Room)
        {
            //what goes in a secret room? not chests
            //perhaps a secret vendor? or vendors?
        }

        public static void FinishKeyRoom(Room Room)
        {
            for (i = 0; i < Pool.intObjCount; i++)
            {   //loop thru all active roomObjects
                if (Pool.intObjPool[i].active)
                {   //find any chest objects in the key room
                    if (
                        Pool.intObjPool[i].type == InteractiveType.Chest
                        || Pool.intObjPool[i].type == InteractiveType.ChestKey
                        )
                    {   //check the dungeon.bigKey boolean to see if this chest should be filled
                        if (LevelSet.dungeon.bigKey)
                        { Functions_InteractiveObjs.SetType(Pool.intObjPool[i], InteractiveType.ChestEmpty); }
                        else //if hero has found the map, this chest is empty, else it has a key
                        { Functions_InteractiveObjs.SetType(Pool.intObjPool[i], InteractiveType.ChestKey); }
                    }
                }
            }
        }

        public static void SpawnBoss(Room Room)
        {   //spawn boss in center of room
            if (LevelSet.dungeon.ID == LevelID.Forest_Dungeon)
            {
                Functions_Actor.SpawnActor(
                    ActorType.Boss_BigEye,
                    Room.center.X + 8,
                    Room.center.Y + 8);
            }

            /*
            else if (LevelSet.dungeon.ID == LevelID.Mountain_Dungeon)
            {
                Functions_Actor.SpawnActor(
                    ActorType.Boss_BigBat,
                    Room.center.X + 8,
                    Room.center.Y + 8);
            }
            else if (LevelSet.dungeon.ID == LevelID.Swamp_Dungeon)
            {
                Functions_Actor.SpawnActor(
                    ActorType.Boss_OctoHead,
                    Room.center.X + 8,
                    Room.center.Y + 8);
            }
            */

        }

        public static void SpawnMiniBoss(Room Room)
        {   //spawn miniboss in center of room
            if (LevelSet.dungeon.ID == LevelID.Forest_Dungeon)
            {
                Functions_Actor.SpawnActor(
                    ActorType.MiniBoss_BlackEye,
                    Room.center.X + 8,
                    Room.center.Y + 8);
            }

            /*
            else if (LevelSet.dungeon.ID == LevelID.Mountain_Dungeon)
            {
                Functions_Actor.SpawnActor(
                        ActorType.MiniBoss_Spider_Armored,
                        Room.center.X + 8,
                        Room.center.Y + 8);
                Functions_Actor.SpawnActor(
                        ActorType.MiniBoss_Spider_Armored,
                        Room.center.X + 8,
                        Room.center.Y + 8);
            }
            else if (LevelSet.dungeon.ID == LevelID.Swamp_Dungeon)
            {
                Functions_Actor.SpawnActor(
                        ActorType.MiniBoss_OctoMouth,
                        Room.center.X + 8,
                        Room.center.Y + 8);
            }
            */


        }


        static Vector2 posA = new Vector2();
        static Vector2 posB = new Vector2();
        public static void DecorateDoor(InteractiveObject Door, InteractiveType Type)
        {   //decorates a door on left/right or top/bottom
            if (Door.direction == Direction.Up || Door.direction == Direction.Down)
            {   //build left/right decorations if Door.direction is Up or Down
                posA.X = Door.compSprite.position.X - 16;
                posA.Y = Door.compSprite.position.Y;
                posB.X = Door.compSprite.position.X + 16;
                posB.Y = Door.compSprite.position.Y;
            }
            else
            {   //build top/bottom decorations if Door.direction is Left or Right
                posA.X = Door.compSprite.position.X;
                posA.Y = Door.compSprite.position.Y - 16;
                posB.X = Door.compSprite.position.X;
                posB.Y = Door.compSprite.position.Y + 16;
            }
            //build wall decorationA torch/pillar/decoration
            Functions_InteractiveObjs.Spawn(Type, posA.X, posA.Y, Door.direction);
            //build wall decorationB torch/pillar/decoration
            Functions_InteractiveObjs.Spawn(Type, posB.X, posB.Y, Door.direction);
        }


        

        //Methods called during Room Interactions

        public static int g;
        public static void ShutDoors(Room Room)
        {   //convert ANY kind of door to a 1 way trap door in room
            for (g = 0; g < Pool.intObjCount; g++)
            {
                if (Pool.intObjPool[g].active)
                {   //note this is a GROUP check, so even bombable doors convert
                    if (Pool.intObjPool[g].group == InteractiveGroup.Door_Dungeon)
                    {
                        Functions_InteractiveObjs.SetType(
                            Pool.intObjPool[g],
                            InteractiveType.Dungeon_DoorTrap);
                    }
                }
            }
        }

        public static void CheckForPuzzles(Boolean solved)
        {   //check to see if hero has solved room
            if (LevelSet.currentLevel.currentRoom.puzzleType == PuzzleType.Torches)
            {   //if the current room's puzzle type is Torches, check to see how many have been lit
                if (CountTorches())
                {   //enough torches have been lit to unlock this room / solve puzzle

                    //right now, this can be spammed, if hero lights/unlights torch to get 4
                    //we need to track if room has been 'solved' - store this in dungeon room list
                    Assets.Play(Assets.sfxReward); //should be secret sfx!!!
                    OpenTrapDoors(); //open all the trap doors
                }
            }
        }

        public static Boolean CountTorches()
        {   //count to see if there are more than 3 lit torches in the current room
            int torchCount = 0;
            for (i = 0; i < Pool.intObjCount; i++)
            {
                if (Pool.intObjPool[i].active)
                {   //if there is an active switch in the room
                    if (Pool.intObjPool[i].type == InteractiveType.TorchLit)
                    { torchCount++; } //count all the lit torches
                }
            }
            //check for exactly 4 lit torches
            if (torchCount == 4) { return true; } else { return false; }
        }

        public static void OpenTrapDoors()
        {
            for (i = 0; i < Pool.intObjCount; i++)
            {   //loop thru all active roomObjects
                if (Pool.intObjPool[i].active)
                {   //convert trap doors to open doors
                    if (Pool.intObjPool[i].type == InteractiveType.Dungeon_DoorTrap)
                    {   //display an attention particle where the conversion happened
                        Functions_InteractiveObjs.SetType(Pool.intObjPool[i], InteractiveType.Dungeon_DoorOpen);
                        Functions_Particle.Spawn(
                            ParticleType.Attention,
                            Pool.intObjPool[i].compSprite.position.X,
                            Pool.intObjPool[i].compSprite.position.Y);
                    }
                }
            }
        }

        public static void CloseTrapDoors()
        {
            for (i = 0; i < Pool.intObjCount; i++)
            {   //loop thru all active roomObjects
                if (Pool.intObjPool[i].active)
                {   //convert open doors to trap doors
                    if (Pool.intObjPool[i].type == InteractiveType.Dungeon_DoorOpen)
                    {   //display an attention particle where the conversion happened
                        Functions_InteractiveObjs.SetType(Pool.intObjPool[i], InteractiveType.Dungeon_DoorTrap);
                        Functions_Particle.Spawn(
                            ParticleType.Attention,
                            Pool.intObjPool[i].compSprite.position.X,
                            Pool.intObjPool[i].compSprite.position.Y);
                    }
                }
            }
        }






    }
}