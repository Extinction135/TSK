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

        public static ScreenDungeon dungeonScreen;
        public static Dungeon dungeon; //the last dungeon created
        public static Room currentRoom; //points to a room on dungeon's roomList



        public static void Initialize(ScreenDungeon DungeonScreen) { dungeonScreen = DungeonScreen; }

        public static void BuildDungeon(DungeonType Type)
        {
            //create a new dungeon
            dungeon = new Dungeon();
            dungeon.type = Type;

            if (Type == DungeonType.Shop)
            {   //set the objPool texture
                Functions_Pool.SetDungeonTexture(Assets.shopSheet);
                //create the shop room
                dungeon.rooms.Add(new Room(new Point(16 * 10, 16 * 21), RoomType.Shop, 0));

                //keep the title music playing
                Functions_Music.PlayMusic(Music.Title);
            }
            else
            {   //set the objPool texture
                Functions_Pool.SetDungeonTexture(Assets.cursedCastleSheet);


                //create the dungeon's rooms
                Room exitRoom = new Room(new Point(0, 0), RoomType.Exit, 0);
                Room hubRoom = new Room(new Point(0, 0), RoomType.Hub, 1);
                Room bossRoom = new Room(new Point(0, 0), RoomType.Boss, 2);

                //place/move the rooms (relative to each other)
                exitRoom.Move(16 * 10, 16 * 100);
                hubRoom.Move(16 * 10, exitRoom.collision.rec.Y - (16 * hubRoom.size.Y) - 16);
                bossRoom.Move(16 * 10, hubRoom.collision.rec.Y - (16 * bossRoom.size.Y) - 16);

                //add rooms to the rooms list
                dungeon.rooms.Add(exitRoom);
                dungeon.rooms.Add(hubRoom);
                dungeon.rooms.Add(bossRoom);

                



                //cycle thru dungeon tracks
                if (Functions_Music.currentMusic == Assets.musicDungeonA)
                { Functions_Music.PlayMusic(Music.DungeonB); }
                else if (Functions_Music.currentMusic == Assets.musicDungeonB)
                { Functions_Music.PlayMusic(Music.DungeonC); }
                else if (Functions_Music.currentMusic == Assets.musicDungeonC)
                { Functions_Music.PlayMusic(Music.DungeonA); }
            }

            //build the first room in the dungeon (room with exit)
            Functions_Room.BuildRoom(dungeon.rooms[0]);
            currentRoom = dungeon.rooms[0];

            //reset the dungeon screen's dungeon record, passing dungeonID
            DungeonRecord.Reset();
            DungeonRecord.dungeonID = 0; //ID = 0 for now
            DungeonRecord.timer.Start(); //start the record timer

            //fade the dungeon screen out from black, revealing the new level
            dungeonScreen.overlayAlpha = 1.0f;
            dungeonScreen.displayState = DisplayState.Opening;
        }



        public static void ConnectRooms(Room Parent, Room Child)
        {   //connect parent to child from any direction
            Point poke = new Point(0, 0); //used to see if child.collision.contains() poke value
            List<Point> doorPositions = new List<Point>(); //a list of possible door positions
            List<Direction> directions = new List<Direction>();
            int counter;


            #region Poke to see how the Parent and Child rooms can connect

            //check up - iterate horizontally above parent from top left corner
            for (counter = 0; counter < Parent.size.X; counter++)
            {
                poke.X = Parent.collision.rec.X + counter * 16;
                poke.Y = Parent.collision.rec.Y - 32;
                if (Child.collision.rec.Contains(poke)) { doorPositions.Add(poke); directions.Add(Direction.Up); }
            }
            //check down - iterate horizontally below parent from bottom left corner
            for (counter = 0; counter < Parent.size.X; counter++)
            {
                poke.X = Parent.collision.rec.X + counter * 16;
                poke.Y = Parent.collision.rec.Y + Parent.collision.rec.Height + 32;
                if (Child.collision.rec.Contains(poke)) { doorPositions.Add(poke); directions.Add(Direction.Down); }
            }
            //check left - iterate vertically left of parent from top left corner
            for (counter = 0; counter < Parent.size.Y; counter++)
            {
                poke.X = Parent.collision.rec.X - 32;
                poke.Y = Parent.collision.rec.Y + counter * 16;
                if (Child.collision.rec.Contains(poke)) { doorPositions.Add(poke); directions.Add(Direction.Left); }
            }
            //check right - iterate vertically right of parent from top left corner
            for (counter = 0; counter < Parent.size.Y; counter++)
            {
                poke.X = Parent.collision.rec.X + Parent.collision.rec.Width + 32;
                poke.Y = Parent.collision.rec.Y + counter * 16;
                if (Child.collision.rec.Contains(poke)) { doorPositions.Add(poke); directions.Add(Direction.Right); }
            }

            #endregion


            Vector2 doorPosition = new Vector2(); //create the door gameobject at this doorPosition
            int doorChoice;
            //if we only have 1 possible connection use it, otherwise choose the middlemost position to place a door at
            if (doorPositions.Count == 1) { doorChoice = 0; }
            else if (doorPositions.Count > 1) { doorChoice = (int)doorPositions.Count / 2; }
            else { return; } //we did not find any possible connection between Parent and Child, bail from this method
            //set the door position based on the choice we made above
            doorPosition.X = doorPositions[doorChoice].X;
            doorPosition.Y = doorPositions[doorChoice].Y;

            //create the parent and child doors
            GameObject parentDoor = Functions_Pool.GetRoomObj();
            GameObject childDoor = Functions_Pool.GetRoomObj();


            #region Based on direction selected, set the direction of both the parent and child doors, place them correctly

            if (directions[doorChoice] == Direction.Up)
            {
                parentDoor.direction = Direction.Down;
                childDoor.direction = Direction.Up;
                Functions_Movement.Teleport(parentDoor.compMove, doorPosition.X + 8, doorPosition.Y + 16 + 8);
                Functions_Movement.Teleport(childDoor.compMove, doorPosition.X + 8, doorPosition.Y + 16 + 8);
                //parentDoor.sprite.position.X += 8;
                //parentDoor.sprite.position.Y += 16 + 8;
                //childDoor.sprite.position.X += 8;
                //childDoor.sprite.position.Y += 16 + 8;
            }
            else if (directions[doorChoice] == Direction.Down)
            {
                parentDoor.direction = Direction.Up;
                childDoor.direction = Direction.Down;
                Functions_Movement.Teleport(parentDoor.compMove, doorPosition.X + 8, doorPosition.Y + 16 + 8 - 32 - 16);
                Functions_Movement.Teleport(childDoor.compMove, doorPosition.X + 8, doorPosition.Y + 16 + 8 - 32 - 16);
                //parentDoor.sprite.position.X += 8;
                //parentDoor.sprite.position.Y += 16 + 8 - 32 - 16;
                //childDoor.sprite.position.X += 8;
                //childDoor.sprite.position.Y += 16 + 8 - 32 - 16;
            }
            else if (directions[doorChoice] == Direction.Left)
            {
                parentDoor.direction = Direction.Left;
                childDoor.direction = Direction.Right;
                Functions_Movement.Teleport(parentDoor.compMove, doorPosition.X + 8 + 16, doorPosition.Y + 0 + 8);
                Functions_Movement.Teleport(childDoor.compMove, doorPosition.X + 8 + 16, doorPosition.Y + 0 + 8);
                //parentDoor.sprite.position.X += 8 + 16;
                //parentDoor.sprite.position.Y += 8 + 0;
                //childDoor.sprite.position.X += 8 + 16;
                //childDoor.sprite.position.Y += 8 + 0;
            }
            else if (directions[doorChoice] == Direction.Right)
            {
                parentDoor.direction = Direction.Right;
                childDoor.direction = Direction.Left;
                Functions_Movement.Teleport(parentDoor.compMove, doorPosition.X + 8 - 16 * 2, doorPosition.Y + 0 + 8);
                Functions_Movement.Teleport(childDoor.compMove, doorPosition.X + 8 - 16 * 2, doorPosition.Y + 0 + 8);
                //parentDoor.sprite.position.X += 8 - 16 * 2;
                //parentDoor.sprite.position.Y += 8 + 0;
                //childDoor.sprite.position.X += 8 - 16 * 2;
                //childDoor.sprite.position.Y += 8 + 0;
            }

            #endregion


            #region Change the door type based on the child room type

            if (Child.type == RoomType.Boss)
            {
                //parentDoor.type = GameObject.Type.DungeonDoorBoss;
                //childDoor.type = GameObject.Type.DungeonDoorTrap;
                Functions_GameObject.SetType(parentDoor, ObjType.DoorBoss);
                Functions_GameObject.SetType(childDoor, ObjType.DoorTrap);
            }
            else
            {
                Functions_GameObject.SetType(parentDoor, ObjType.DoorOpen);
                Functions_GameObject.SetType(childDoor, ObjType.DoorOpen);
            }
            //else if (Child.type == RoomType.Secret)
            //{ parentDoor.type = GameObject.Type.DungeonDoorBombable; childDoor.type = GameObject.Type.DungeonDoorBombed; }

            #endregion
            
        }






    }
}