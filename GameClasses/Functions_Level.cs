﻿using System;
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




        //where exit/field room is placed - should be part of build functions
        public static Point buildPosition = new Point(16 * 10, 16 * 200);




        public static void ResetLevel(Level Level)
        {   
            Level.rooms = new List<Room>();
            Level.doors = new List<Door>();
            Level.bigKey = false;
            Level.map = false;
            //enable map and key cheats
            Level.map = Flags.MapCheat;
            Level.bigKey = Flags.KeyCheat;
        }








        








        public static void BuildLevel(LevelID levelID)
        {
            //reset these booleans in case we changed them via levelSet.currentLevel
            LevelSet.dungeon.isField = false;
            LevelSet.field.isField = true;




            #region Dungeon Build

            if (
                levelID == LevelID.Forest_Dungeon
                || levelID == LevelID.DeathMountain_Dungeon
                || levelID == LevelID.HauntedSwamp_Dungeon
                || levelID == LevelID.ThievesHideout_Dungeon
                || levelID == LevelID.Lava_Dungeon
                || levelID == LevelID.Cloud_Dungeon
                || levelID == LevelID.Skull_Dungeon
                )
            {
                //reset dungeon data
                ResetLevel(LevelSet.dungeon);
                //point current level to dungeon
                LevelSet.dungeon.ID = levelID;
                LevelSet.currentLevel = LevelSet.dungeon;
                //set background color
                if (levelID == LevelID.Forest_Dungeon)
                { ColorScheme.background = ColorScheme.bkg_dungeon_forest; }
                else if (levelID == LevelID.DeathMountain_Dungeon)
                { ColorScheme.background = ColorScheme.bkg_dungeon_mountain; }
                else if (levelID == LevelID.HauntedSwamp_Dungeon)
                { ColorScheme.background = ColorScheme.bkg_dungeon_swamp; }
                else if (levelID == LevelID.ThievesHideout_Dungeon)
                { ColorScheme.background = ColorScheme.bkg_dungeon_forest; }
                else if (levelID == LevelID.Lava_Dungeon)
                { ColorScheme.background = ColorScheme.bkg_dungeon_forest; }
                else if (levelID == LevelID.Cloud_Dungeon)
                { ColorScheme.background = ColorScheme.bkg_dungeon_forest; }
                else if (levelID == LevelID.Skull_Dungeon)
                { ColorScheme.background = ColorScheme.bkg_dungeon_forest; }
            }


            #endregion


            //or


            #region Field Build

            else
            {   //reset field data
                ResetLevel(LevelSet.field);
                //point current level to field
                LevelSet.field.ID = levelID;
                LevelSet.currentLevel = LevelSet.field;
                //set bkg color: green, as most levels occur on land
                ColorScheme.background = ColorScheme.bkg_level_room;
            }

            #endregion








            if (Flags.PrintOutput)
            {   //lets debug some stuff
                string output = "building level....";
                output += "currLevel: " + LevelSet.currentLevel.ID + "\n";
                output += "field: " + LevelSet.field.ID + "\n";
                output += "dungeon: " + LevelSet.dungeon.ID + "\n";
                Debug.WriteLine("" + output);
            }
            //start building levels
            stopWatch.Reset(); stopWatch.Start();
            //setup the room (level), or series of rooms (dungeon)


            #region Build DEV Levels

            if (Flags.bootRoutine != BootRoutine.Game) //we may be building dev levels
            {
                if (Widgets.RoomTools.roomData == null)
                {

                }
                else
                {



                    if (
                    //check to see if editor just built a new dungeon room to work on
                    Widgets.RoomTools.roomData.type == RoomID.DEV_Boss ||
                    Widgets.RoomTools.roomData.type == RoomID.DEV_Column ||
                    Widgets.RoomTools.roomData.type == RoomID.DEV_Exit ||
                    Widgets.RoomTools.roomData.type == RoomID.DEV_Hub ||
                    Widgets.RoomTools.roomData.type == RoomID.DEV_Key ||
                    Widgets.RoomTools.roomData.type == RoomID.DEV_Row ||
                    Widgets.RoomTools.roomData.type == RoomID.DEV_Square
                    )
                    {
                        //make a new room, which inherits the RoomTool's roomData type
                        RoomID roomType = Widgets.RoomTools.roomData.type;

                        //build the dev room based on roomType
                        Room room = new Room(new Point(buildPosition.X, buildPosition.Y), roomType);
                        LevelSet.currentLevel.rooms.Add(room); //spawn/exit room must be at index 0
                        LevelSet.currentLevel.currentRoom = LevelSet.currentLevel.rooms[0];

                        //set spawnPos (outside TopLeft of new dev room)
                        LevelSet.spawnPos_Dungeon.X = room.rec.X - 32;
                        LevelSet.spawnPos_Dungeon.Y = room.rec.Y;
                    }
                    //or if editor just built a new field to work on 
                    else if (Widgets.RoomTools.roomData.type == RoomID.DEV_Field)
                    {
                        Room field = new Room(new Point(buildPosition.X, buildPosition.Y), RoomID.DEV_Field);
                        LevelSet.currentLevel.rooms.Add(field);
                        LevelSet.currentLevel.currentRoom = LevelSet.currentLevel.rooms[0];
                        Functions_Hero.ResetFieldSpawnPos();
                    }



                }
            }

            //that's all the editor builds, if we load a level in the editor thats
            //not a dev level, it will build and interact normally, as in game.

            #endregion





            //Build Overworld Field Levels


            #region Skull Island

            if (LevelSet.currentLevel.ID == LevelID.SkullIsland_Colliseum)
            {
                Functions_Music.PlayMusic(Music.LightWorld);
                Room field = new Room(new Point(buildPosition.X, buildPosition.Y), RoomID.SkullIsland_Colliseum);
                LevelSet.currentLevel.rooms.Add(field);
            }
            else if (LevelSet.currentLevel.ID == LevelID.SkullIsland_ColliseumPit)
            {
                Functions_Music.PlayMusic(Music.CrowdWaiting);
                Room field = new Room(new Point(buildPosition.X, buildPosition.Y), RoomID.SkullIsland_ColliseumPit);
                LevelSet.currentLevel.rooms.Add(field);
            }
            else if (LevelSet.currentLevel.ID == LevelID.SkullIsland_Town)
            {
                Functions_Music.PlayMusic(Music.LightWorld);
                Room field = new Room(new Point(buildPosition.X, buildPosition.Y), RoomID.SkullIsland_Town);
                LevelSet.currentLevel.rooms.Add(field);
            }
            else if (LevelSet.currentLevel.ID == LevelID.SkullIsland_ShadowKing)
            {
                Functions_Music.PlayMusic(Music.LightWorld);
                Room field = new Room(new Point(buildPosition.X, buildPosition.Y), RoomID.SkullIsland_ShadowKing);
                LevelSet.currentLevel.rooms.Add(field);
            }

            #endregion


            #region Death Mountain

            else if (LevelSet.currentLevel.ID == LevelID.DeathMountain_MainEntrance)
            {
                Functions_Music.PlayMusic(Music.LightWorld);
                Room field = new Room(new Point(buildPosition.X, buildPosition.Y), RoomID.DeathMountain_MainEntrance);
                LevelSet.currentLevel.rooms.Add(field);
            }

            #endregion


            #region Forest Island

            else if (LevelSet.currentLevel.ID == LevelID.ForestIsland_MainEntrance)
            {
                Functions_Music.PlayMusic(Music.LightWorld);
                Room field = new Room(new Point(buildPosition.X, buildPosition.Y), RoomID.ForestIsland_MainEntrance);
                LevelSet.currentLevel.rooms.Add(field);
            }

            #endregion


            #region Lava Island

            else if (LevelSet.currentLevel.ID == LevelID.LavaIsland_MainEntrance)
            {
                Functions_Music.PlayMusic(Music.LightWorld);
                Room field = new Room(new Point(buildPosition.X, buildPosition.Y), RoomID.LavaIsland_MainEntrance);
                LevelSet.currentLevel.rooms.Add(field);
            }

            #endregion


            #region Cloud Island

            else if (LevelSet.currentLevel.ID == LevelID.CloudIsland_MainEntrance)
            {
                Functions_Music.PlayMusic(Music.LightWorld);
                Room field = new Room(new Point(buildPosition.X, buildPosition.Y), RoomID.CloudIsland_MainEntrance);
                LevelSet.currentLevel.rooms.Add(field);
            }

            #endregion


            #region Swamp Island

            else if (LevelSet.currentLevel.ID == LevelID.SwampIsland_MainEntrance)
            {
                Functions_Music.PlayMusic(Music.LightWorld);
                Room field = new Room(new Point(buildPosition.X, buildPosition.Y), RoomID.SwampIsland_MainEntrance);
                LevelSet.currentLevel.rooms.Add(field);
            }

            #endregion


            #region Thieves Den

            else if (LevelSet.currentLevel.ID == LevelID.ThievesDen_GateEntrance)
            {
                Functions_Music.PlayMusic(Music.LightWorld);
                Room field = new Room(new Point(buildPosition.X, buildPosition.Y), RoomID.ThievesDen_GateEntrance);
                LevelSet.currentLevel.rooms.Add(field);
            }

            #endregion


            //or

            #region Build Dungeons

            else if (LevelSet.currentLevel.ID == LevelID.Forest_Dungeon)
            { Functions_Dungeon.BuildDungeon_Forest(); }
            else if (LevelSet.currentLevel.ID == LevelID.DeathMountain_Dungeon)
            { Functions_Dungeon.BuildDungeon_Mountain(); }
            else if (LevelSet.currentLevel.ID == LevelID.HauntedSwamp_Dungeon)
            { Functions_Dungeon.BuildDungeon_Swamp(); }
            else if (LevelSet.currentLevel.ID == LevelID.ThievesHideout_Dungeon)
            { Functions_Dungeon.BuildDungeon_Thieves(); }
            else if (LevelSet.currentLevel.ID == LevelID.Lava_Dungeon)
            { Functions_Dungeon.BuildDungeon_Lava(); }
            else if (LevelSet.currentLevel.ID == LevelID.Cloud_Dungeon)
            { Functions_Dungeon.BuildDungeon_Cloud(); }
            else if (LevelSet.currentLevel.ID == LevelID.Skull_Dungeon)
            { Functions_Dungeon.BuildDungeon_Skull(); }

            #endregion






            //update the dungeon texture based on levelID (sets themed sheet)
            Functions_Texture.UpdateDungeonTexture();
            Functions_Texture.SetFloorTextures();

            //setup and build the first room
            LevelSet.currentLevel.rooms[0].visited = true;
            LevelSet.currentLevel.currentRoom = LevelSet.currentLevel.rooms[0];
            Functions_Room.BuildRoom(LevelSet.currentLevel.currentRoom);

            if (Flags.PrintOutput)
            {
                Debug.WriteLine("current room level ID = " + LevelSet.currentLevel.currentRoom.levelID);
                Debug.WriteLine("current room room ID = " + LevelSet.currentLevel.currentRoom.roomID);
            }
            





            //spawn hero in the starting room
            Functions_Hero.SpawnInCurrentRoom();

            //give hero a minimum amount of health
            if (Pool.hero.health < 3) { Pool.hero.health = 3; }

            //teleport camera to center of room
            Camera2D.targetPosition.X = LevelSet.currentLevel.currentRoom.center.X;
            Camera2D.targetPosition.Y = LevelSet.currentLevel.currentRoom.center.Y;
            Camera2D.currentPosition = Camera2D.targetPosition;
            Functions_Camera2D.SetView();
            //level screen will then decide where the camera should be per frame

            stopWatch.Stop(); time = stopWatch.Elapsed;
            if (Flags.PrintOutput)
            {
                Debug.WriteLine("level " + LevelSet.currentLevel.ID + " built in " + time.Ticks + " ticks");
            }

            //fade the dungeon screen out from black, revealing the new level
            //this needs to be implemented when level screen is global
            //levelScreen.overlay.alpha = 1.0f;
            //levelScreen.displayState = DisplayState.Opening;
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
            if (
                (
                    Parent.roomID == RoomID.DEV_Exit ||
                    Parent.roomID == RoomID.ForestIsland_ExitRoom ||
                    Parent.roomID == RoomID.DeathMountain_ExitRoom ||
                    Parent.roomID == RoomID.HauntedSwamps_ExitRoom ||
                    Parent.roomID == RoomID.ThievesHideout_ExitRoom ||
                    Parent.roomID == RoomID.LavaIsland_ExitRoom ||
                    Parent.roomID == RoomID.CloudIsland_ExitRoom ||
                    Parent.roomID == RoomID.SkullIsland_ExitRoom
                )
                & 
                Dir == Direction.Down
                ) { return; }
            if (
                (
                    Child.roomID == RoomID.DEV_Exit ||
                    Child.roomID == RoomID.ForestIsland_ExitRoom ||
                    Child.roomID == RoomID.DeathMountain_ExitRoom ||
                    Child.roomID == RoomID.HauntedSwamps_ExitRoom ||
                    Child.roomID == RoomID.ThievesHideout_ExitRoom ||
                    Child.roomID == RoomID.LavaIsland_ExitRoom ||
                    Child.roomID == RoomID.CloudIsland_ExitRoom ||
                    Child.roomID == RoomID.SkullIsland_ExitRoom
                )
                & 
                Dir == Direction.Up
                ) { return; }

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
            if (
                Parent.roomID == RoomID.DEV_Boss ||
                Parent.roomID == RoomID.ForestIsland_BossRoom ||
                Parent.roomID == RoomID.DeathMountain_BossRoom ||
                Parent.roomID == RoomID.HauntedSwamps_BossRoom ||
                Parent.roomID == RoomID.ThievesHideout_BossRoom ||
                Parent.roomID == RoomID.LavaIsland_BossRoom ||
                Parent.roomID == RoomID.CloudIsland_BossRoom ||
                Parent.roomID == RoomID.SkullIsland_BossRoom
                )
            { door.type = DoorType.Boss; }
            else if (
                Child.roomID == RoomID.DEV_Boss ||
                Child.roomID == RoomID.ForestIsland_BossRoom ||
                Child.roomID == RoomID.DeathMountain_BossRoom ||
                Child.roomID == RoomID.HauntedSwamps_BossRoom ||
                Child.roomID == RoomID.ThievesHideout_BossRoom ||
                Child.roomID == RoomID.LavaIsland_BossRoom ||
                Child.roomID == RoomID.CloudIsland_BossRoom ||
                Child.roomID == RoomID.SkullIsland_BossRoom
                )
            { door.type = DoorType.Boss; }

            if (Parent.roomID == RoomID.Secret) { door.type = DoorType.Bombable; }
            else if (Child.roomID == RoomID.Secret) { door.type = DoorType.Bombable; }

            //add the door to Level.doors list
            LevelSet.currentLevel.doors.Add(door);
        }





        public static RoomID GetRandomRoomType()
        {   //return a column, row, or square RoomType
            int random = Functions_Random.Int(0, 3);

            if (LevelSet.dungeon.ID == LevelID.Forest_Dungeon)
            {
                if (random == 0) { return RoomID.ForestIsland_ColumnRoom; }
                else if (random == 1) { return RoomID.ForestIsland_RowRoom; }
                else { return RoomID.ForestIsland_SquareRoom; }
            }
            else if (LevelSet.dungeon.ID == LevelID.DeathMountain_Dungeon)
            {
                if (random == 0) { return RoomID.DeathMountain_ColumnRoom; }
                else if (random == 1) { return RoomID.DeathMountain_RowRoom; }
                else { return RoomID.DeathMountain_SquareRoom; }
            }
            else if (LevelSet.dungeon.ID == LevelID.HauntedSwamp_Dungeon)
            {
                if (random == 0) { return RoomID.HauntedSwamps_ColumnRoom; }
                else if (random == 1) { return RoomID.HauntedSwamps_RowRoom; }
                else { return RoomID.HauntedSwamps_SquareRoom; }
            }
            else if (LevelSet.dungeon.ID == LevelID.ThievesHideout_Dungeon)
            {
                if (random == 0) { return RoomID.ThievesHideout_ColumnRoom; }
                else if (random == 1) { return RoomID.ThievesHideout_RowRoom; }
                else { return RoomID.ThievesHideout_SquareRoom; }
            }
            else if (LevelSet.dungeon.ID == LevelID.Lava_Dungeon)
            {
                if (random == 0) { return RoomID.LavaIsland_ColumnRoom; }
                else if (random == 1) { return RoomID.LavaIsland_RowRoom; }
                else { return RoomID.LavaIsland_SquareRoom; }
            }
            else if (LevelSet.dungeon.ID == LevelID.Cloud_Dungeon)
            {
                if (random == 0) { return RoomID.CloudIsland_ColumnRoom; }
                else if (random == 1) { return RoomID.CloudIsland_RowRoom; }
                else { return RoomID.CloudIsland_SquareRoom; }
            }
            else if (LevelSet.dungeon.ID == LevelID.Skull_Dungeon)
            {
                if (random == 0) { return RoomID.SkullIsland_ColumnRoom; }
                else if (random == 1) { return RoomID.SkullIsland_RowRoom; }
                else { return RoomID.SkullIsland_SquareRoom; }
            }

            //code should never get here
            return RoomID.ForestIsland_ColumnRoom;
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
                for (int i = 0; i < LevelSet.currentLevel.rooms.Count; i++)
                { if (Child.rec.Intersects(LevelSet.currentLevel.rooms[i].rec)) { collision = true; } }
                //deflate
                Child.rec.Width -= 32; Child.rec.Height -= 32;
                Child.rec.X += 16; Child.rec.Y += 16;

                //set the child room's center
                Functions_Room.MoveRoom(Child, Child.rec.X, Child.rec.Y);
                if (!collision) //if there wasn't a room collision, add room to rooms list
                { LevelSet.currentLevel.rooms.Add(Child); return true; }
            }
            return false; //we didn't successfully place the child room
        }

        public static void AddMoreRooms()
        {   //randomly add additional rooms to all rooms except exit/boss/key
            int coreRoomCount = LevelSet.currentLevel.rooms.Count;
            for (int i = 0; i < coreRoomCount; i++)
            {
                if (
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.DEV_Exit ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.ForestIsland_ExitRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.DeathMountain_ExitRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.HauntedSwamps_ExitRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.ThievesHideout_ExitRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.LavaIsland_ExitRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.CloudIsland_ExitRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.SkullIsland_ExitRoom
                    )
                { } //do nothing
                else if (
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.DEV_Boss ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.ForestIsland_BossRoom||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.DeathMountain_BossRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.HauntedSwamps_BossRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.ThievesHideout_BossRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.LavaIsland_BossRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.CloudIsland_BossRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.SkullIsland_BossRoom 
                    )
                { } //only one entrance to boss rooms
                else if (
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.DEV_Key ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.ForestIsland_KeyRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.DeathMountain_KeyRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.HauntedSwamps_KeyRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.ThievesHideout_KeyRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.LavaIsland_KeyRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.CloudIsland_KeyRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.SkullIsland_KeyRoom
                    )
                { }
                else
                {
                    Room room = new Room(new Point(0, 0), GetRandomRoomType());
                    AddRoom(LevelSet.currentLevel.rooms[i], room, 10, false);
                }
            }
        }

        public static void AddSecretRooms()
        {   //randomly add secret rooms to all rooms except exit/boss/secret/hub
            int coreRoomCount = LevelSet.currentLevel.rooms.Count;
            for (int i = 0; i < coreRoomCount; i++)
            {
                if (
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.DEV_Exit ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.ForestIsland_ExitRoom || 
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.DeathMountain_ExitRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.HauntedSwamps_ExitRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.ThievesHideout_ExitRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.LavaIsland_ExitRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.CloudIsland_ExitRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.SkullIsland_ExitRoom
                    )
                { }
                else if (
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.DEV_Boss ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.ForestIsland_BossRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.DeathMountain_BossRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.HauntedSwamps_BossRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.ThievesHideout_BossRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.LavaIsland_BossRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.CloudIsland_BossRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.SkullIsland_BossRoom
                    )
                { }
                
                else if (
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.DEV_Hub ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.ForestIsland_HubRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.DeathMountain_HubRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.HauntedSwamps_HubRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.ThievesHideout_HubRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.LavaIsland_HubRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.CloudIsland_HubRoom ||
                    LevelSet.currentLevel.rooms[i].roomID == RoomID.SkullIsland_HubRoom
                    )
                { }
                else if (LevelSet.currentLevel.rooms[i].roomID == RoomID.Secret)
                { } //why? because secret rooms would build to nowhere, and we want them to connect existing rooms
                else
                {
                    Room room = new Room(new Point(0, 0), RoomID.Secret);
                    AddRoom(LevelSet.currentLevel.rooms[i], room, 10, false);
                }
            }
        }

        public static void CloseLevel(ExitAction ExitAct)
        {
            Screens.Level.exitAction = ExitAct;
            Screens.Level.displayState = DisplayState.Closing;
        }

    }
}