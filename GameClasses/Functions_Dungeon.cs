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
        //Dungeon Generation Recipes

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

            PlayerData.ForestRecord.Clear();
            PlayerData.ForestRecord.timer.Restart(); //start timer
        }

        public static void BuildDungeon_Mountain()
        {
            //for migration
            BuildDungeon_Forest();
            return;

            /*
            //recipe for size 2 dungeon - normal challenge

            //base dungeon
            BuildDungeon_ExitToHub(1); //short
            BuildDungeon_AddBossRoom();
            //medium size
            BuildDungeon_KeyPath(2); //med size
            BuildDungeon_ImproveExit();
            BuildDungeon_Expand(2); //med size/secrets
            BuildDungeon_Finalize();

            PlayerData.MountainRecord.Clear();
            PlayerData.MountainRecord.timer.Restart(); //start timer
            */
        }

        public static void BuildDungeon_Swamp()
        {
            //for migration
            BuildDungeon_Forest();
            return;

            /*
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

            PlayerData.SwampRecord.Clear();
            PlayerData.SwampRecord.timer.Restart(); //start timer
            */


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









        //Dungeon Generation Functions and Data

        static int lastRoom;
        static int b;
        static int hubIndex = 0; //0 = hub doesn't exist
        static int bossIndex = 0; //0 = boss doesn't exist

        static void BuildDungeon_ExitToHub(byte numOfRoomsBetween)
        {
            //create the exit room at the build position
            Room exitRoom = new Room(new Point(0, 0), RoomID.ForestIsland_ExitRoom);
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
                Room room = new Room(new Point(0, 0), Functions_Level.GetRandomRoomType_Forest());
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
            { hubType = RoomID.ForestIsland_HubRoom; }
            //else if (LevelSet.dungeon.ID == LevelID.Mountain_Dungeon)
            //{ hubType = RoomID.DeathMountain_HubRoom; }
            //else if (LevelSet.dungeon.ID == LevelID.Swamp_Dungeon)
            //{ hubType = RoomID.SwampIsland_HubRoom; }

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
            //else if (LevelSet.dungeon.ID == LevelID.Mountain_Dungeon)
            //{ bossID = RoomID.DeathMountain_BossRoom; }
            //else if (LevelSet.dungeon.ID == LevelID.Swamp_Dungeon)
            //{ bossID = RoomID.SwampIsland_BossRoom; }

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
            Room keyPathStart = new Room(new Point(0, 0), Functions_Level.GetRandomRoomType_Forest());
            
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
                Room room = new Room(new Point(0, 0), Functions_Level.GetRandomRoomType_Forest());
                Functions_Level.AddRoom(LevelSet.dungeon.rooms[lastRoom], room, 20, false);
            }

            //create key randomly around last room
            lastRoom = LevelSet.dungeon.rooms.Count() - 1;
            Room keyRoom = new Room(new Point(0, 0), RoomID.ForestIsland_KeyRoom);
            Functions_Level.AddRoom(LevelSet.dungeon.rooms[lastRoom], keyRoom, 20, true);
        }

        static void BuildDungeon_ImproveExit()
        {
            //Add rooms around Exit Room, makes Dungeon non-linear
            //this diversifies the initial paths presented to player
            for (b = 0; b < 2; b++)
            {
                lastRoom = LevelSet.dungeon.rooms.Count() - 1;
                Room room = new Room(new Point(0, 0), Functions_Level.GetRandomRoomType_Forest());
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

                    /*
                    //cave boss room can only connect to cave hub room
                    else if (buildList[0].roomID == RoomID.DeathMountain_BossRoom &
                        buildList[b].roomID != RoomID.DeathMountain_HubRoom)
                    { connectRooms = false; }

                    //swamp boss room can only connect to swamp hub room
                    if (buildList[0].roomID == RoomID.SwampIsland_BossRoom &
                        buildList[b].roomID != RoomID.SwampIsland_HubRoom)
                    { connectRooms = false; }
                    */

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


            #region Setup some room's roomData index

            for (b = 0; b < LevelSet.dungeon.rooms.Count; b++)
            {
                //based on type, set room's data index to random roomData ref
                //this allows the room to build with the same roomData each time hero enters it
                //but this can also build the same room multiple times in a dungeon
                
                //we id these rooms so they build from the same data upon re-entry
                if (LevelSet.dungeon.rooms[b].roomID == RoomID.ForestIsland_ColumnRoom)
                { LevelSet.dungeon.rooms[b].dataIndex = Functions_Random.Int(0, RoomData_SkullIsland_Columns.Data.Count); }
                else if (LevelSet.dungeon.rooms[b].roomID == RoomID.ForestIsland_KeyRoom)
                { LevelSet.dungeon.rooms[b].dataIndex = Functions_Random.Int(0, RoomData_SkullIsland_Key.Data.Count); }
                else if (LevelSet.dungeon.rooms[b].roomID == RoomID.ForestIsland_RowRoom)
                { LevelSet.dungeon.rooms[b].dataIndex = Functions_Random.Int(0, RoomData_SkullIsland_Row.Data.Count); }
                else if (LevelSet.dungeon.rooms[b].roomID == RoomID.ForestIsland_SquareRoom)
                { LevelSet.dungeon.rooms[b].dataIndex = Functions_Random.Int(0, RoomData_SkullIsland_Square.Data.Count); }
            }

            #endregion


            
        }




        
        


        //MiniDungeon Example
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