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
    public static class Functions_Level
    {
        public static Stopwatch stopWatch = new Stopwatch();
        public static TimeSpan time;

        public static ScreenLevel levelScreen;

        public static Room currentRoom; //points to a room on dungeon's roomList
        public static int dungeonTrack = 0;

        //where the exit room is placed in dungeon.rooms list
        public static Point buildPosition = new Point(16 * 10, 16 * 200); 



        public static void ResetLevel()
        {   //level type is set by overworld & dialog screens
            Level.rooms = new List<Room>();
            Level.doors = new List<Door>();
            Level.bigKey = false;
            Level.map = false;
        }

        public static void BuildLevel()
        {
            ResetLevel();
            //set all floor sprites to the appropriate dungeon texture
            Functions_Pool.SetFloorTexture(Level.type);
            if (Flags.PrintOutput) { Debug.WriteLine("-- creating dungeon --"); }


            #region Create Shop

            if (Level.type == LevelType.Shop)
            {   //create the dungeon's rooms
                Room shopRoom = new Room(new Point(0, 0), RoomType.Shop);
                Functions_Room.MoveRoom(shopRoom, buildPosition.X, buildPosition.Y);
                Level.rooms.Add(shopRoom); //exit room must be at index0

                //keep the title music playing
                Functions_Music.PlayMusic(Music.Title);
            }

            #endregion


            #region  Create Dungeon

            else
            {   //start timing dungeon generation
                stopWatch.Reset(); stopWatch.Start();


                #region Generate the Dungeon

                //create the exit room
                Room exitRoom = new Room(new Point(0, 0), RoomType.Exit);
                Functions_Room.MoveRoom(exitRoom, buildPosition.X, buildPosition.Y);
                Level.rooms.Add(exitRoom); //exit room must be at index0
                int last = Level.rooms.Count() - 1;
                int i;

                //create a north path to the hub room (keeps hub centered to map)
                for (i = 0; i < 2; i++)
                {
                    last = Level.rooms.Count() - 1;
                    Room room = new Room(new Point(0, 0), GetRandomRoomType());
                    Functions_Room.MoveRoom(room, Level.rooms[last].rec.X, Level.rooms[last].rec.Y - (16 * room.size.Y) - 16);
                    Level.rooms.Add(room);
                }
                //create hub north of last room
                last = Level.rooms.Count() - 1;
                Room hubRoom = new Room(new Point(0, 0), RoomType.Hub);
                Functions_Room.MoveRoom(hubRoom, Level.rooms[last].rec.X, Level.rooms[last].rec.Y - (16 * hubRoom.size.Y) - 16);
                Level.rooms.Add(hubRoom);
                //place boss north of hub room
                Room bossRoom = new Room(new Point(0, 0), RoomType.Boss);
                Functions_Room.MoveRoom(bossRoom, hubRoom.rec.X, hubRoom.rec.Y - (16 * bossRoom.size.Y) - 16);
                Level.rooms.Add(bossRoom);

                //create the start of key path from a room that isn't exit / boss / hub
                Room keyPathStart = new Room(new Point(0, 0), GetRandomRoomType());
                int random = Functions_Random.Int(1, Level.rooms.Count() - 2);
                AddRoom(Level.rooms[random], keyPathStart, 20, true);
                //create a random path to the key room from key path start room (last room)
                for (i = 0; i < 2; i++)
                {
                    last = Level.rooms.Count() - 1;
                    Room room = new Room(new Point(0, 0), GetRandomRoomType());
                    AddRoom(Level.rooms[last], room, 20, false);
                }
                //create key randomly around last room
                last = Level.rooms.Count() - 1;
                Room keyRoom = new Room(new Point(0, 0), RoomType.Key);
                AddRoom(Level.rooms[last], keyRoom, 20, true);

                //randomly add rooms around exit room
                for (i = 0; i < 2; i++)
                {
                    last = Level.rooms.Count() - 1;
                    Room room = new Room(new Point(0, 0), GetRandomRoomType());
                    AddRoom(Level.rooms[0], room, 20, true);
                }

                AddMoreRooms();
                AddMoreRooms();
                AddSecretRooms();

                #endregion


                //create the build list
                List<Room> buildList = new List<Room>();
                for (i = 0; i < Level.rooms.Count; i++)
                { buildList.Add(Level.rooms[i]); }


                #region Connect Rooms with Doors

                if (Flags.PrintOutput) { Debug.WriteLine("connecting rooms..."); }
                Boolean connectRooms;
                while (buildList.Count() > 0)
                {   //check first room against remaining rooms
                    for (i = 1; i < buildList.Count(); i++)
                    {
                        connectRooms = true;
                        //only the boss room can connect to the hub room
                        if (buildList[0].type == RoomType.Boss && buildList[i].type != RoomType.Hub)
                        { connectRooms = false; }
                        
                        if(connectRooms)
                        {   //if the two rooms are nearby, create door between them
                            if (RoomsNearby(buildList[0], buildList[i]))
                            { GetDoorLocations(buildList[0], buildList[i]); }
                        }
                        //Debug.WriteLine("rooms nearby: " + RoomsNearby(buildList[0], buildList[i]));
                        //Debug.WriteLine("parent: " + buildList[0].type);
                        //Debug.WriteLine("child: " + buildList[i].type);
                    }
                    buildList.RemoveAt(0); //remove first room
                }
                if (Flags.PrintOutput)
                {
                    Debug.WriteLine("connected " + Level.rooms.Count + " rooms");
                    Debug.WriteLine("created  " + Level.doors.Count + " doors");
                }

                #endregion


                //choose the dungeon music
                if (dungeonTrack == 0) { Functions_Music.PlayMusic(Music.DungeonA); }
                else if (dungeonTrack == 1) { Functions_Music.PlayMusic(Music.DungeonB); }
                else if (dungeonTrack == 2) { Functions_Music.PlayMusic(Music.DungeonC); }
                //cycle thru dungeon music tracks
                dungeonTrack++;
                if (dungeonTrack > 2) { dungeonTrack = 0; }

                //collect the elapsed ticks for dungeon generation
                stopWatch.Stop(); time = stopWatch.Elapsed;
                DebugInfo.dungeonTime = time.Ticks;
                if (Flags.PrintOutput)
                { Debug.WriteLine("dungeon generated in " + time.Ticks + " ticks"); }
            }

            #endregion


            #region Finish Level

            //build the first room in the dungeon (room with exit)
            Level.rooms[0].visited = true; //hero spawns in this room
            currentRoom = Level.rooms[0];
            Functions_Room.BuildRoom(Level.rooms[0]);
            Functions_Room.FinishRoom(Level.rooms[0]);
            //check to see if dungeon map should be given to hero upon spawn
            if (Flags.MapCheat) { Level.map = true; } else { Level.map = false; }
            //reset the dungeon screen's dungeon record, passing dungeonID
            DungeonRecord.Reset();
            DungeonRecord.dungeonID = 0; //ID = 0 for now
            DungeonRecord.timer.Start(); //start the record timer
            //fade the dungeon screen out from black, revealing the new level
            levelScreen.overlay.alpha = 1.0f;
            levelScreen.displayState = DisplayState.Opening;

            #endregion


            //ensure hero is ActorType.Hero (in case his type was changed elsewhere)
            Functions_Actor.SetType(Pool.hero, ActorType.Hero);
            Functions_Room.SpawnHeroInCurrentRoom(); //spawn hero in exit room
            Pool.hero.direction = Direction.Up; //face hero up
        }



        static Rectangle compRec = new Rectangle(0, 0, 0, 0);
        static Boolean RoomsNearby(Room Parent, Room Child)
        {   //place & size comparisonRec to be 2 cells larger than parent
            compRec.X = Parent.rec.X - 32;
            compRec.Y = Parent.rec.Y - 32;
            compRec.Width = Parent.rec.Width + 64;
            compRec.Height = Parent.rec.Height + 64;
            //ensure that compRec intersets child room
            if (compRec.Intersects(Child.rec))
            { return true; } else { return false; }
        }

        public static void GetDoorLocations(Room Parent, Room Child)
        {   //determine the direction child is located relative to parent
            compRec.Width = Parent.rec.Width;
            compRec.Height = Parent.rec.Height;

            //check left
            compRec.X = Parent.rec.X - 32;
            compRec.Y = Parent.rec.Y;
            if (compRec.Intersects(Child.rec))
            { Poke(Direction.Left, Parent, Child); return; }

            //check right
            compRec.X = Parent.rec.X + 32;
            compRec.Y = Parent.rec.Y;
            if (compRec.Intersects(Child.rec))
            { Poke(Direction.Right, Parent, Child); return; }

            //check up
            compRec.X = Parent.rec.X;
            compRec.Y = Parent.rec.Y - 32;
            if (compRec.Intersects(Child.rec))
            { Poke(Direction.Up, Parent, Child); return; }

            //check down
            compRec.X = Parent.rec.X;
            compRec.Y = Parent.rec.Y + 32;
            if (compRec.Intersects(Child.rec))
            { Poke(Direction.Down, Parent, Child); return; }
        }

        static Point poke = new Point(0, 0); //used to see if child.collision.contains() poke value
        static void Poke(Direction Dir, Room Parent, Room Child)
        {   
            //no room can connect/build a south door to exit
            if (Parent.type == RoomType.Exit && Dir == Direction.Down) { return; }
            if (Child.type == RoomType.Exit && Dir == Direction.Up) { return; }

            List<Point> doorPos = new List<Point>(); //a list of possible door positions


            #region Check Left

            if (Dir == Direction.Left)
            {   //iterate vertically down parent from top left corner
                for (int i = 0; i < Parent.size.Y; i++)
                {
                    poke.X = Parent.rec.X - 24;
                    poke.Y = Parent.rec.Y + i * 16;
                    if (Child.rec.Contains(poke))
                    { poke.X += 8; doorPos.Add(poke); }
                }
            }

            #endregion


            #region Check Right

            else if(Dir == Direction.Right)
            {   //iterate vertically down parent from top right corner
                for (int i = 0; i < Parent.size.Y; i++)
                {
                    poke.X = Parent.rec.X + Parent.rec.Width + 24;
                    poke.Y = Parent.rec.Y + i * 16;
                    if (Child.rec.Contains(poke))
                    { poke.X -= 8; poke.X -= 16; doorPos.Add(poke); }
                }
            }

            #endregion


            #region Check Up

            else if (Dir == Direction.Up)
            {   //iterate horizontally above parent from top left corner
                for (int i = 0; i < Parent.size.X; i++)
                {
                    poke.X = Parent.rec.X + i * 16;
                    poke.Y = Parent.rec.Y - 24;
                    if (Child.rec.Contains(poke))
                    { poke.Y += 8; doorPos.Add(poke); }
                }
            }

            #endregion


            #region Check Down

            else if (Dir == Direction.Down)
            {   //iterate horizontally below parent from bottom left corner
                for (int i = 0; i < Parent.size.X; i++)
                {
                    poke.X = Parent.rec.X + i * 16;
                    poke.Y = Parent.rec.Y + Parent.rec.Height + 24;
                    if (Child.rec.Contains(poke))
                    { poke.Y -= 24; doorPos.Add(poke); }
                }
            }

            #endregion


            //select the door
            int chosenIndex = 0; //default choose first door position
            if (doorPos.Count > 2) //but if we have 3 or more door positions,
            { chosenIndex = doorPos.Count / 2; } //choose the middle one

            //set the type of the door, based on parent & child type
            Door door = new Door(doorPos[chosenIndex]);
            if (Parent.type == RoomType.Boss) { door.type = DoorType.Boss; }
            else if (Child.type == RoomType.Boss) { door.type = DoorType.Boss; }
            if (Parent.type == RoomType.Secret) { door.type = DoorType.Bombable; }
            else if (Child.type == RoomType.Secret) { door.type = DoorType.Bombable; }

            //add the door to Level.doors list
            Level.doors.Add(door);
        }

        public static RoomType GetRandomRoomType()
        {   //return a column, row, or square RoomType
            int random = Functions_Random.Int(0, 3);
            if (random == 0) { return RoomType.Column; }
            else if (random == 1) { return RoomType.Row; }
            else { return RoomType.Square; }
        }

        public static Boolean AddRoom(Room Parent, Room Child, int Attempts, Boolean ignoreSouth)
        {
            int attempt = 0;
            Boolean collision;
            for (attempt = 0; attempt < Attempts; attempt++)
            {   //inherit parent's position
                Child.rec.X = Parent.rec.X;
                Child.rec.Y = Parent.rec.Y;
                int direction; //get the offset direction
                if (ignoreSouth) //should we ignore south direction?
                { direction = Functions_Random.Int(1, 4); } //ignore south
                else { direction = Functions_Random.Int(1, 5); } //include south
                //randomize placement of child in relation to parent
                if (direction == 1) { Child.rec.X += Parent.rec.Width + 16; } //place right
                else if (direction == 2) { Child.rec.Y -= Child.rec.Height + 16; } //place up
                else if (direction == 3) { Child.rec.X -= Child.rec.Width + 16; } //place left
                else if (direction == 4) { Child.rec.Y += Parent.rec.Height + 16; } //place down
                //randomize placement of secret rooms more
                if (Child.type == RoomType.Secret) 
                {
                    if (direction == 2 || direction == 4)
                    {   //if direction is up or down, move child room right random amount less than parent's width
                        Child.rec.X += Functions_Random.Int(0, Parent.size.X) * 16;
                    }
                    else
                    {   //if direction is L/R, move child room down random amount less than parent's height
                        Child.rec.Y += Functions_Random.Int(0, Parent.size.Y) * 16;
                    }
                }

                collision = false;
                //below we check to make sure the room isn't overlapping another room
                //we do this by inflating the rec by two tiles, then checking collisions, then deflating
                //this ensures that the room has a 16px border on all edges and never touches another room

                //inflate
                Child.rec.Width += 32; Child.rec.Height += 32;
                Child.rec.X -= 16; Child.rec.Y -= 16;
                //check to see if child collides with any room
                for (int i = 0; i < Level.rooms.Count; i++)
                { if (Child.rec.Intersects(Level.rooms[i].rec)) { collision = true; } }
                //deflate
                Child.rec.Width -= 32; Child.rec.Height -= 32;
                Child.rec.X += 16; Child.rec.Y += 16;

                //set the child room's center
                Functions_Room.MoveRoom(Child, Child.rec.X, Child.rec.Y);
                if (!collision) //if there wasn't a room collision, add room to rooms list
                { Level.rooms.Add(Child); return true; }
            }
            return false; //we didn't successfully place the child room
        }

        public static void AddMoreRooms()
        {   //randomly add additional rooms to all rooms except exit/boss/key
            int coreRoomCount = Level.rooms.Count;
            for (int i = 0; i < coreRoomCount; i++)
            {
                if (Level.rooms[i].type == RoomType.Exit) { }
                else if (Level.rooms[i].type == RoomType.Boss) { }
                else if (Level.rooms[i].type == RoomType.Key) { }
                else
                {
                    Room room = new Room(new Point(0, 0), GetRandomRoomType());
                    AddRoom(Level.rooms[i], room, 10, false);
                }
            }
        }

        public static void AddSecretRooms()
        {   //randomly add secret rooms to all rooms except exit/boss/secret
            int coreRoomCount = Level.rooms.Count;
            for (int i = 0; i < coreRoomCount; i++)
            {
                if (Level.rooms[i].type == RoomType.Exit) { }
                else if (Level.rooms[i].type == RoomType.Boss) { }
                else if (Level.rooms[i].type == RoomType.Secret) { }
                else
                {
                    Room room = new Room(new Point(0, 0), RoomType.Secret);
                    AddRoom(Level.rooms[i], room, 10, false);
                }
            }
        }

    }
}