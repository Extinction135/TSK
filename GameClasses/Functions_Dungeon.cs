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
        public static int i;
        public static int j;
        
        public static ComponentSprite floorRef;
        public static GameObject objRef;
        public static Actor actorRef;
        public static Point pos;



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

            Functions_GameObject.AlignRoomObjs();
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
                    Room.roomID == RoomID.Key ||
                    Room.roomID == RoomID.ForestIsland_HubRoom ||
                    Room.roomID == RoomID.DeathMountain_HubRoom ||
                    Room.roomID == RoomID.SwampIsland_HubRoom
                    )
                { Pool.floorPool[i].currentFrame = AnimationFrames.Dungeon_FloorSpecial[0]; }
                else if (
                    Room.roomID == RoomID.ForestIsland_BossRoom ||
                    Room.roomID == RoomID.DeathMountain_BossRoom ||
                    Room.roomID == RoomID.SwampIsland_BossRoom
                    )
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
                        for (j = 0; j < LevelSet.currentLevel.doors.Count; j++)
                        {
                            if (Pool.roomObjPool[i].compCollision.rec.Contains(LevelSet.currentLevel.doors[j].rec.Location))
                            {
                                //set the room's doors based on the dungeon.door.type
                                if (LevelSet.currentLevel.doors[j].type == DoorType.Bombable)
                                { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.Dungeon_DoorBombable); }

                                else if (LevelSet.currentLevel.doors[j].type == DoorType.Boss)
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
                                if (Room.roomID == RoomID.ForestIsland_BossRoom ||
                                    Room.roomID == RoomID.DeathMountain_BossRoom ||
                                    Room.roomID == RoomID.SwampIsland_BossRoom
                                    )
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
            LevelSet.currentLevel.doors.Add(new Door(new Point(posX + middleX, posY - 16))); //top
            LevelSet.currentLevel.doors.Add(new Door(new Point(posX - 16, posY + middleY))); //left
            LevelSet.currentLevel.doors.Add(new Door(new Point(posX + width, posY + middleY))); //right

            if (Room.roomID != RoomID.DEV_Exit) //exit rooms have exit objs on south wall, no door
            { LevelSet.currentLevel.doors.Add(new Door(new Point(posX + middleX, posY + height))); } //bottom
        }

        public static void ProcedurallyFinish(Room Room)
        {   //Pass the room to the appropriate method for completion


            #region Special Rooms

            if (Room.roomID == RoomID.Exit)
            {
                PlaceExit(Room);
                AddCrackedWalls(Room);
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
                //this room's focus is the puzzle
                //AddWallStatues(Room);
                AddCrackedWalls(Room);
                ScatterDebris(Room);
            }
            else if (
                Room.roomID == RoomID.ForestIsland_HubRoom ||
                Room.roomID == RoomID.DeathMountain_HubRoom ||
                Room.roomID == RoomID.SwampIsland_HubRoom
                )
            {
                //we dont want the miniboss to spawn in the editor
                //we can manually spawn him - he destroys the room
                if (Flags.bootRoutine == BootRoutine.Game) { SpawnMiniBoss(Room); }
                
                //dont add wall statues, cause they damage miniboss
                //AddWallStatues(Room);
                AddCrackedWalls(Room);
                ScatterDebris(Room);
            }
            else if (
                Room.roomID == RoomID.ForestIsland_BossRoom ||
                Room.roomID == RoomID.DeathMountain_BossRoom ||
                Room.roomID == RoomID.SwampIsland_BossRoom
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

            else if (Room.roomID == RoomID.Column)
            {
                AddCrackedWalls(Room);
                ScatterDebris(Room);
            }
            else if (Room.roomID == RoomID.Row)
            {
                AddCrackedWalls(Room);
                ScatterDebris(Room);
            }
            else if (Room.roomID == RoomID.Square)
            {
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
                ShutDoors(Room); //we do want to shut the doors tho
            }
            else if(Room.roomID == RoomID.DEV_Exit)
            {
                PlaceExit(Room);
            }
            else if(Room.roomID == RoomID.DEV_Hub)
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
            Functions_GameObject.AlignRoomObjs();
        }



        static int torchCount;
        public static void SetupPuzzle(Room Room)
        {
            //this is called at the end of a room build
            torchCount = 0;
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active)
                {   //if there is an active switch in the room - this is a switch puzzle
                    if (Pool.roomObjPool[i].type == ObjType.Dungeon_Switch)
                    {
                        //if autosolve cheat is enabled, convert switch to perm down version
                        if (Flags.AutoSolvePuzzle) 
                        { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.Dungeon_SwitchDownPerm); }
                        //if game isn't in hard mode, and we already visited this level, convert it too
                        else if (Flags.HardMode == false & Room.visited == true)
                        { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.Dungeon_SwitchDownPerm); }

                        //if this room hasn't been visited, setup any puzzle it contains
                        //if (LevelSet.dungeon.currentRoom.visited == false) { }

                        //we haven't visited this room, or the game is in hard mode
                        else
                        {   //setup the switch puzzle
                            Room.puzzleType = PuzzleType.Switch;
                            Functions_GameObject_Dungeon.CloseDoors(); //convert all openDoors to trapDoors
                            i = Pool.roomObjCount; //end loop
                        }
                    }
                    //count all the unlit torches
                    else if (Pool.roomObjPool[i].type == ObjType.Dungeon_TorchUnlit)
                    { torchCount++; } 
                }
            }

            //autosolve torch puzzles too
            if (Flags.AutoSolvePuzzle) { torchCount = 0; }
            //check for more than 3 torches, if 4 or more, then 4 need to be lit
            if (torchCount > 3)
            {   //convert all openDoors to trapDoors
                Room.puzzleType = PuzzleType.Torches;
                Functions_GameObject_Dungeon.CloseDoors();
            }   //torches > switches
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
                        Functions_GameObject.Spawn(ObjType.Dungeon_FloorStain,
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


        public static void Check_BossDoor()
        {
            //can be ANY room, dungeon recipes can attach boss to any room
            for (i = 0; i < Pool.roomObjCount; i++)
            {   //loop thru all active roomObjects
                if (Pool.roomObjPool[i].active)
                {
                    //create the boss welcome mat (zelda staple, grabs players attention)
                    if (Pool.roomObjPool[i].type == ObjType.Dungeon_DoorBoss)
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







        //room specific procedural objects 

        public static void FinishSecretRoom(Room Room)
        {
            //what goes in a secret room? not chests
            //perhaps a secret vendor? or vendors?
        }

        //setup KEY chest
        public static void FinishKeyRoom(Room Room)
        {
            for (i = 0; i < Pool.roomObjCount; i++)
            {   //loop thru all active roomObjects
                if (Pool.roomObjPool[i].active)
                {   //find any chest objects in the key room
                    if (Pool.roomObjPool[i].group == ObjGroup.Chest)
                    {   //check the dungeon.bigKey boolean to see if this chest should be filled
                        if (LevelSet.dungeon.bigKey)
                        { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.Dungeon_ChestEmpty); }
                        else //if hero has found the map, this chest is empty, else it has a key
                        { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.Dungeon_ChestKey); }
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
        }




        











        


        public static void BuildDungeon_Forest()
        {
            //recipe for size 1 dungeon - very easy

            //base dungeon
            BuildDungeon_ExitToHub(1); //short
            BuildDungeon_AddBossRoom();
            //small size
            BuildDungeon_KeyPath(1); //sm size
            BuildDungeon_ImproveExit();
            BuildDungeon_Expand(1); //small, secrets++
            BuildDungeon_Finalize();
        }

        public static void BuildDungeon_Mountain()
        {
            //recipe for size 2 dungeon - normal challenge

            //base dungeon
            BuildDungeon_ExitToHub(1); //short
            BuildDungeon_AddBossRoom();
            //medium size
            BuildDungeon_KeyPath(2); //med size
            BuildDungeon_ImproveExit();
            BuildDungeon_Expand(2); //med size/secrets
            BuildDungeon_Finalize();
        }

        public static void BuildDungeon_Swamp()
        {
            //recipe for size 3 dungeon: warning - very big
            
            //base dungeon
            BuildDungeon_ExitToHub(2); //med
            BuildDungeon_AddBossRoom();

            //huge size
            BuildDungeon_KeyPath(3); //med size
            BuildDungeon_ImproveExit();
            BuildDungeon_Expand(2);
            BuildDungeon_Expand(2);
            //lots of secrets
            Functions_Level.AddSecretRooms();
            Functions_Level.AddSecretRooms();
            Functions_Level.AddSecretRooms();
            //ok stop
            BuildDungeon_Finalize();
            


            /*
            //test idea: can we hide the key path behind a secret room?
            //YES WE CAN!
            //base dungeon
            BuildDungeon_ExitToHub(3); //long
            BuildDungeon_AddBossRoom();

            //lots of secret rooms
            Functions_Level.AddSecretRooms();
            Functions_Level.AddSecretRooms();
            Functions_Level.AddSecretRooms();
            Functions_Level.AddSecretRooms();
            Functions_Level.AddSecretRooms();
            //huge size
            BuildDungeon_KeyPath(1); //stay short so it cant touch hub
            BuildDungeon_ImproveExit();

            //lots of secret rooms
            Functions_Level.AddSecretRooms();
            Functions_Level.AddSecretRooms();
            Functions_Level.AddSecretRooms();
            Functions_Level.AddSecretRooms();
            Functions_Level.AddSecretRooms();

            //some connecting rooms
            Functions_Level.AddMoreRooms();

            //lots of secret rooms
            Functions_Level.AddSecretRooms();
            Functions_Level.AddSecretRooms();
            Functions_Level.AddSecretRooms();
            Functions_Level.AddSecretRooms();
            Functions_Level.AddSecretRooms();

            //ok stop
            BuildDungeon_Finalize();
            */



        }






        //fields used in dungeon generation
        static int lastRoom;
        static int b;
        static int hubIndex = 0; //0 = hub doesn't exist
        static int bossIndex = 0; //0 = boss doesn't exist

        static void BuildDungeon_ExitToHub(byte numOfRoomsBetween)
        {
            //create the exit room at the build position
            Room exitRoom = new Room(new Point(0, 0), RoomID.Exit);
            Functions_Room.MoveRoom(exitRoom, 
                Functions_Level.buildPosition.X, 
                Functions_Level.buildPosition.Y);

            LevelSet.dungeon.rooms.Add(exitRoom); //exit room must be at index0
            lastRoom = LevelSet.dungeon.rooms.Count() - 1;

            //create a north path to the hub room (keeps hub centered to map)
            //we *could* randomly choose which direction these rooms are built,
            //but that leads to the possibility that the hub ends up next to exit
            //which seems buggy and cheap and too easy - so we build away from exit room (north)
            for (b = 0; b < numOfRoomsBetween; b++)
            {
                lastRoom = LevelSet.dungeon.rooms.Count() - 1;
                Room room = new Room(new Point(0, 0), Functions_Level.GetRandomRoomType());
                Functions_Room.MoveRoom(room,
                    LevelSet.dungeon.rooms[lastRoom].rec.X,
                    LevelSet.dungeon.rooms[lastRoom].rec.Y - (16 * room.size.Y) - 16);
                LevelSet.dungeon.rooms.Add(room);
            }

            //create hub north of last room
            lastRoom = LevelSet.dungeon.rooms.Count() - 1;
            RoomID hubType = RoomID.ForestIsland_HubRoom;
            //set hubType based on dungeon type
            if (LevelSet.dungeon.ID == LevelID.Forest_Dungeon)
            { hubType = RoomID.ForestIsland_HubRoom; } //ocd
            else if (LevelSet.dungeon.ID == LevelID.Mountain_Dungeon)
            { hubType = RoomID.DeathMountain_HubRoom; }
            else if (LevelSet.dungeon.ID == LevelID.Swamp_Dungeon)
            { hubType = RoomID.SwampIsland_HubRoom; }

            //create and move hub room
            Room hubRoom = new Room(new Point(0, 0), hubType);
            Functions_Room.MoveRoom(hubRoom,
                LevelSet.dungeon.rooms[lastRoom].rec.X,
                LevelSet.dungeon.rooms[lastRoom].rec.Y - (16 * hubRoom.size.Y) - 16);
            LevelSet.dungeon.rooms.Add(hubRoom);
            //track the hub's index
            hubIndex = LevelSet.dungeon.rooms.Count() - 1;
        }

        static void BuildDungeon_AddBossRoom()
        {
            //place boss north of hub room added - hub must be last room
            lastRoom = LevelSet.currentLevel.rooms.Count() - 1;
            //AddBossRoom() MUST immediately follow call to ExitToHub()

            RoomID bossID = RoomID.ForestIsland_BossRoom;
            //determine what boss room to load based on current level id
            if (LevelSet.dungeon.ID == LevelID.Forest_Dungeon)
            { bossID = RoomID.ForestIsland_BossRoom; }
            else if (LevelSet.dungeon.ID == LevelID.Mountain_Dungeon)
            { bossID = RoomID.DeathMountain_BossRoom; }
            else if (LevelSet.dungeon.ID == LevelID.Swamp_Dungeon)
            { bossID = RoomID.SwampIsland_BossRoom; }

            //create and move boss room
            Room bossRoom = new Room(new Point(0, 0), bossID);
            Functions_Room.MoveRoom(bossRoom,
                LevelSet.dungeon.rooms[lastRoom].rec.X,
                LevelSet.dungeon.rooms[lastRoom].rec.Y - (16 * bossRoom.size.Y) - 16);
            LevelSet.dungeon.rooms.Add(bossRoom);
            //track the boss's index
            bossIndex = LevelSet.dungeon.rooms.Count() - 1;
        }

        static void BuildDungeon_KeyPath(byte numOfRoomsBetween)
        {
            //create the start of key path from a room that isn't exit / boss / hub

            //create a starting room for this path
            Room keyPathStart = new Room(new Point(0, 0), Functions_Level.GetRandomRoomType());
            
            //randomly select a room that isn't exit(0), boss(count-1), or hub(count-2)
            //*this only works if it follows exit to hub, then add boss path...
            int random = Functions_Random.Int(1, LevelSet.dungeon.rooms.Count() - 2);
            //try to attach the starting key path room to the target room 20 times
            Functions_Level.AddRoom(LevelSet.dungeon.rooms[random], keyPathStart, 20, true);

            //we could improve this by randomizing which room we try
            //to attach key path start to, each time. we'd need a boolean
            //to model success or failure, which determines if we repeat or exit

            //create a random path to the key room from key path start room 
            //the key path start room is the last room added to Level.rooms
            for (b = 0; b < numOfRoomsBetween; b++)
            {
                lastRoom = LevelSet.dungeon.rooms.Count() - 1;
                Room room = new Room(new Point(0, 0), Functions_Level.GetRandomRoomType());
                Functions_Level.AddRoom(LevelSet.dungeon.rooms[lastRoom], room, 20, false);
            }

            //create key randomly around last room
            lastRoom = LevelSet.dungeon.rooms.Count() - 1;
            Room keyRoom = new Room(new Point(0, 0), RoomID.Key);
            Functions_Level.AddRoom(LevelSet.dungeon.rooms[lastRoom], keyRoom, 20, true);
        }

        static void BuildDungeon_ImproveExit()
        {
            //Add rooms around Exit Room, makes Dungeon non-linear
            //this diversifies the initial paths presented to player
            for (b = 0; b < 2; b++)
            {
                lastRoom = LevelSet.dungeon.rooms.Count() - 1;
                Room room = new Room(new Point(0, 0), Functions_Level.GetRandomRoomType());
                Functions_Level.AddRoom(LevelSet.dungeon.rooms[0], room, 20, true);
            }
        }

        static void BuildDungeon_Expand(int Recipe)
        {

            #region Finalize Dungeon - Recipe 1

            if (Recipe == 1)
            {
                //compact, with lots of secret paths
                Functions_Level.AddMoreRooms();
                Functions_Level.AddSecretRooms();
                Functions_Level.AddSecretRooms();
                Functions_Level.AddSecretRooms();
            }

            #endregion


            #region Finalize Dungeon - Recipe 2

            else if (Recipe == 2)
            {
                //add rooms, then connect them - twice

                //expand + connect dungeon
                Functions_Level.AddMoreRooms();
                Functions_Level.AddSecretRooms();
                Functions_Level.AddSecretRooms();
                //expand + connect dungeon
                Functions_Level.AddMoreRooms();
                Functions_Level.AddSecretRooms();
                Functions_Level.AddSecretRooms();
            }

            #endregion


        }

        static void BuildDungeon_Finalize()
        {
            //create a temporary build list
            //(we will destructively edit this list)
            List<Room> buildList = new List<Room>();
            for (b = 0; b < LevelSet.dungeon.rooms.Count; b++)
            { buildList.Add(LevelSet.dungeon.rooms[b]); }


            #region Connect Rooms with Doors

            if (Flags.PrintOutput) { Debug.WriteLine("connecting rooms..."); }
            Boolean connectRooms;
            while (buildList.Count() > 0)
            {   //check first room against remaining rooms
                for (b = 1; b < buildList.Count(); b++)
                {
                    connectRooms = true;


                    #region Boss Rooms can only connect to Hub Rooms, for now

                    //forest boss room can only connect to forest hub room
                    if(buildList[0].roomID == RoomID.ForestIsland_BossRoom & 
                        buildList[b].roomID != RoomID.ForestIsland_HubRoom)
                    { connectRooms = false; }

                    //cave boss room can only connect to cave hub room
                    else if (buildList[0].roomID == RoomID.DeathMountain_BossRoom &
                        buildList[b].roomID != RoomID.DeathMountain_HubRoom)
                    { connectRooms = false; }

                    //swamp boss room can only connect to swamp hub room
                    if (buildList[0].roomID == RoomID.SwampIsland_BossRoom &
                        buildList[b].roomID != RoomID.SwampIsland_HubRoom)
                    { connectRooms = false; }

                    #endregion



                    if (connectRooms)
                    {   //if the two rooms are nearby, create door between them
                        if (Functions_Level.RoomsNearby(buildList[0], buildList[b]))
                        { Functions_Level.GetDoorLocations(buildList[0], buildList[b]); }
                    }
                    //Debug.WriteLine("rooms nearby: " + RoomsNearby(buildList[0], buildList[b]));
                    //Debug.WriteLine("parent: " + buildList[0].type);
                    //Debug.WriteLine("child: " + buildList[b].type);
                }
                buildList.RemoveAt(0); //remove first room
            }
            if (Flags.PrintOutput)
            {
                Debug.WriteLine("connected " + LevelSet.dungeon.rooms.Count + " rooms");
                Debug.WriteLine("created  " + LevelSet.dungeon.doors.Count + " doors");
            }

            #endregion


            #region Choose Dungeon Music

            //select the dungeon music
            if (LevelSet.dungeon.dungeonTrack == 0) { Functions_Music.PlayMusic(Music.DungeonA); }
            else if (LevelSet.dungeon.dungeonTrack == 1) { Functions_Music.PlayMusic(Music.DungeonB); }
            else if (LevelSet.dungeon.dungeonTrack == 2) { Functions_Music.PlayMusic(Music.DungeonC); }
            //cycle thru dungeon music tracks
            LevelSet.dungeon.dungeonTrack++;
            if (LevelSet.dungeon.dungeonTrack > 2) { LevelSet.dungeon.dungeonTrack = 0; }

            #endregion


            #region Setup room's roomData index

            for (b = 0; b < LevelSet.dungeon.rooms.Count; b++)
            {
                //based on type, set room's data index to random roomData ref
                //this allows the room to build with the same roomData each time hero enters it
                //but this can also build the same room multiple times in a dungeon

                //if (LevelSet.currentLevel.rooms[b].roomID == RoomID.Boss)
                //{ LevelSet.currentLevel.rooms[b].dataIndex = Functions_Random.Int(0, RoomData.bossRooms.Count); }
                //else if (LevelSet.currentLevel.rooms[b].roomID == RoomID.Hub)
                //{ LevelSet.currentLevel.rooms[b].dataIndex = Functions_Random.Int(0, RoomData.hubRooms.Count); }


                if (LevelSet.dungeon.rooms[b].roomID == RoomID.Column)
                { LevelSet.dungeon.rooms[b].dataIndex = Functions_Random.Int(0, RoomData.columnRooms.Count); }
                else if (LevelSet.dungeon.rooms[b].roomID == RoomID.Key)
                { LevelSet.dungeon.rooms[b].dataIndex = Functions_Random.Int(0, RoomData.keyRooms.Count); }
                else if (LevelSet.dungeon.rooms[b].roomID == RoomID.Row)
                { LevelSet.dungeon.rooms[b].dataIndex = Functions_Random.Int(0, RoomData.rowRooms.Count); }
                else if (LevelSet.dungeon.rooms[b].roomID == RoomID.Square)
                { LevelSet.dungeon.rooms[b].dataIndex = Functions_Random.Int(0, RoomData.squareRooms.Count); }
                else if (LevelSet.dungeon.rooms[b].roomID == RoomID.Exit)
                { LevelSet.dungeon.rooms[b].dataIndex = Functions_Random.Int(0, RoomData.exitRooms.Count); }
            }

            #endregion


            //reset the dungeon screen's dungeon record, passing dungeonID
            DungeonRecord.Clear();
            DungeonRecord.dungeonID = 0; //ID = 0 for now
            DungeonRecord.timer.Restart(); //restart timer
        }





        //blob dungeons are created differently
        /*
        //example of a mini-dungeon
        else if (Level.ID == LevelID.MiniBossDungeon)
        {
            //create exit room
            Level.rooms.Add(new Room(new Point(buildPosition.X, buildPosition.Y), RoomID.Exit));
                
            //add 2nd room north of exit room
            Level.rooms.Add(new Room(new Point(
                Level.rooms[0].rec.X, 
                Level.rooms[0].rec.Y - (16 * Level.rooms[0].size.Y) - 16), 
                RoomID.Hub));

            //connect rooms with a door
            Door door = new Door(new Point(Level.rooms[0].rec.X + 16 * 2, Level.rooms[0].rec.Y - 16));
            door.type = DoorType.Open;
            Level.doors.Add(door);
                
            Functions_Music.PlayMusic(Music.LightWorld);
        }
        */















        //old methods - for reference

        /*
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
        */






    }
}