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

        //this is a problem, will be solved with structural change soon
        public static ScreenLevel levelScreen; 

        
        

        public static void ResetLevel()
        {   //level type is set by overworld & dialog screens
            Level.rooms = new List<Room>();
            Level.doors = new List<Door>();
            Level.bigKey = false;
            Level.map = false;
            //enable map and key cheats
            Level.map = Flags.MapCheat;
            Level.bigKey = Flags.KeyCheat;
            //reset to overworld lightworld
            Level.isField = true;
            Level.lightWorld = true;
        }


        public static void BuildLevel(LevelID levelID)
        {
            ResetLevel();
            Level.ID = levelID;

            //set dungeon booleans
            if (levelID == LevelID.Forest_Dungeon || levelID == LevelID.DEV_Room)
            {
                Level.isField = false;
                Level.lightWorld = false;
            }


            #region Set The Background Color

            //assume light world
            Assets.colorScheme.background = Assets.colorScheme.bkg_lightWorld;

            //check for dark world
            if (Level.lightWorld == false) 
            { Assets.colorScheme.background = Assets.colorScheme.bkg_darkWorld; }

            //check for dungeons
            if (levelID == LevelID.Forest_Dungeon
                || levelID == LevelID.Mountain_Dungeon) 
            { Assets.colorScheme.background = Assets.colorScheme.bkg_dungeon; }

            #endregion




            //setup the room (level), or series of rooms (dungeon)
            if (Flags.PrintOutput)
            { Debug.WriteLine("-- building level: " + Level.ID + " --"); }
            stopWatch.Reset(); stopWatch.Start();










            #region Build DEV Levels

            if (Level.ID == LevelID.DEV_Room)
            {
                //make a new room, which inherits the RoomTool's roomData type
                RoomID roomType = RoomID.DEV_Row;
                if (Widgets.RoomTools.roomData != null)
                { roomType = Widgets.RoomTools.roomData.type; }

                //build the dev room based on roomType
                Room room = new Room(new Point(Level.buildPosition.X, Level.buildPosition.Y), roomType); 
                Level.rooms.Add(room); //spawn room must be index0

                //set spawnPos outside TopLeft of new dev room
                room.spawnPos.X = room.rec.X - 32;
                room.spawnPos.Y = room.rec.Y;
            }
            else if (Level.ID == LevelID.DEV_Field)
            {
                Room field = new Room(new Point(Level.buildPosition.X, Level.buildPosition.Y), RoomID.DEV_Field);
                Level.rooms.Add(field);
            }

            #endregion

            //or

            #region Build Overworld Field Levels

            //special levels
            else if (Level.ID == LevelID.Colliseum)
            {
                Functions_Music.PlayMusic(Music.LightWorld);
                Room field = new Room(new Point(Level.buildPosition.X, Level.buildPosition.Y), RoomID.Colliseum);
                Level.rooms.Add(field);
            }
            else if (Level.ID == LevelID.ColliseumPit)
            {
                Functions_Music.PlayMusic(Music.CrowdWaiting);
                Room field = new Room(new Point(Level.buildPosition.X, Level.buildPosition.Y), RoomID.ColliseumPit);
                Level.rooms.Add(field);
            }

            //entrances
            else if (Level.ID == LevelID.Forest_Entrance)
            {
                Functions_Music.PlayMusic(Music.LightWorld);
                Room field = new Room(new Point(Level.buildPosition.X, Level.buildPosition.Y), RoomID.ForestEntrance);
                Level.rooms.Add(field);
            }
            else if (Level.ID == LevelID.Mountain_Entrance)
            {
                Functions_Music.PlayMusic(Music.LightWorld);
                Room field = new Room(new Point(Level.buildPosition.X, Level.buildPosition.Y), RoomID.MountainEntrance);
                Level.rooms.Add(field);
            }

            //standard levels
            else if (Level.ID == LevelID.TheFarm)
            {
                Functions_Music.PlayMusic(Music.LightWorld);
                Room field = new Room(new Point(Level.buildPosition.X, Level.buildPosition.Y), RoomID.TheFarm);
                Level.rooms.Add(field);
            }
            else if (Level.ID == LevelID.LeftTown2)
            {
                Functions_Music.PlayMusic(Music.LightWorld);
                Room field = new Room(new Point(Level.buildPosition.X, Level.buildPosition.Y), RoomID.LeftTown2);
                Level.rooms.Add(field);
            }

            #endregion

            //or

            #region Build Dungeons

            else if (Level.ID == LevelID.Forest_Dungeon)
            {
                Functions_Dungeon.BuildDungeon_Forest();
            }
            else if (Level.ID == LevelID.Mountain_Dungeon)
            {
                Functions_Dungeon.BuildDungeon_Mountain();
            }

            #endregion












            //build the 1st room on Level.rooms list (index0) - exit/spawn room
            Level.rooms[0].visited = true;
            Level.currentRoom = Level.rooms[0];

            //this actually builds everything in the first room
            Functions_Room.BuildRoom(Level.currentRoom);
            Functions_Texture.SetFloorTextures();
            Functions_Hero.SpawnInCurrentRoom();

            //give hero a minimum amount of health
            if (Pool.hero.health < 3) { Pool.hero.health = 3; }

            //teleport camera to center of room
            Camera2D.targetPosition.X = Level.currentRoom.center.X;
            Camera2D.targetPosition.Y = Level.currentRoom.center.Y;
            Camera2D.currentPosition = Camera2D.targetPosition;
            Functions_Camera2D.SetView();
            //level screen will then decide where the camera should be per frame

            stopWatch.Stop(); time = stopWatch.Elapsed;
            if (Flags.PrintOutput)
            {
                Debug.WriteLine("level " + Level.ID + " built in " + time.Ticks + " ticks");
            }

            //fade the dungeon screen out from black, revealing the new level
            levelScreen.overlay.alpha = 1.0f;
            levelScreen.displayState = DisplayState.Opening;
        }


        static Rectangle compRec = new Rectangle(0, 0, 0, 0);
        public static Boolean RoomsNearby(Room Parent, Room Child)
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
            if (Parent.roomID == RoomID.Exit && Dir == Direction.Down) { return; }
            if (Child.roomID == RoomID.Exit && Dir == Direction.Up) { return; }

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


            //if doorPos.count == 0, we can't add a door between these two rooms
            if (doorPos.Count == 0) { return; }

            //select the door
            int chosenIndex = 0; //default choose first door position
            if (doorPos.Count > 2) //but if we have 3 or more door positions,
            { chosenIndex = doorPos.Count / 2; } //choose the middle one

            //set the type of the door
            Door door = new Door(doorPos[chosenIndex]);
            door.type = DoorType.Open; //defaults to open
            //randomly convert some open doors to bombable doors
            if (Functions_Random.Int(0, 101) > 70) { door.type = DoorType.Bombable; }
            //convert doors based on parent and child room types
            if (Parent.roomID == RoomID.Boss) { door.type = DoorType.Boss; }
            else if (Child.roomID == RoomID.Boss) { door.type = DoorType.Boss; }
            if (Parent.roomID == RoomID.Secret) { door.type = DoorType.Bombable; }
            else if (Child.roomID == RoomID.Secret) { door.type = DoorType.Bombable; }

            //add the door to Level.doors list
            Level.doors.Add(door);
        }

        public static RoomID GetRandomRoomType()
        {   //return a column, row, or square RoomType
            int random = Functions_Random.Int(0, 3);
            if (random == 0) { return RoomID.Column; }
            else if (random == 1) { return RoomID.Row; }
            else { return RoomID.Square; }
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
                if (Child.roomID == RoomID.Secret) 
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
                if (Level.rooms[i].roomID == RoomID.Exit) { }
                else if (Level.rooms[i].roomID == RoomID.Boss) { }
                else if (Level.rooms[i].roomID == RoomID.Key) { }
                else
                {
                    Room room = new Room(new Point(0, 0), GetRandomRoomType());
                    AddRoom(Level.rooms[i], room, 10, false);
                }
            }
        }

        public static void AddSecretRooms()
        {   //randomly add secret rooms to all rooms except exit/boss/secret/hub
            int coreRoomCount = Level.rooms.Count;
            for (int i = 0; i < coreRoomCount; i++)
            {
                if (Level.rooms[i].roomID == RoomID.Exit) { }
                else if (Level.rooms[i].roomID == RoomID.Boss) { }
                else if (Level.rooms[i].roomID == RoomID.Secret) { }
                else if (Level.rooms[i].roomID == RoomID.Hub) { }
                else
                {
                    Room room = new Room(new Point(0, 0), RoomID.Secret);
                    AddRoom(Level.rooms[i], room, 10, false);
                }
            }
        }

        public static void CloseLevel(ExitAction ExitAct)
        {
            levelScreen.exitAction = ExitAct;
            levelScreen.displayState = DisplayState.Closing;
        }

    }
}