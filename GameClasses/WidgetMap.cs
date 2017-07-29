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
    public class WidgetMap : Widget
    {
        public Rectangle dungeonBkg;
        public List<Room> rooms;
        public List<Door> doors;
        public Rectangle currentRoom;
        public float currentRoomAlpha = 0.0f;

        public ComponentText bossIcon;
        public ComponentText keyIcon;
        public ComponentText hubIcon;
        public ComponentText exitIcon;

        public Color mapColor;



        public WidgetMap()
        {
            window = new MenuWindow(new Point(-100, -100), new Point(0, 0), "init");
            //create dungeon bkg, current room instance
            dungeonBkg = new Rectangle(0, 0, 0, 0);
            currentRoom = new Rectangle(0, 0, 1, 1);
            //create room icons (for debugging)
            bossIcon = new ComponentText(Assets.font, "B", new Vector2(-100, -100), Assets.colorScheme.textDark);
            keyIcon = new ComponentText(Assets.font, "K", new Vector2(-100, -100), Assets.colorScheme.textDark);
            hubIcon = new ComponentText(Assets.font, "H", new Vector2(-100, -100), Assets.colorScheme.textDark);
            exitIcon = new ComponentText(Assets.font, "E", new Vector2(-100, -100), Assets.colorScheme.textDark);
            //initially this widget starts out closed
            window.interior.displayState = DisplayState.Closed;
        }

        public override void Reset(int X, int Y)
        {   //parameters X and Y are ignored for this function
            Functions_MenuWindow.ResetAndMove(window,
                (int)WorldUI.currentWeapon.compSprite.position.X,
                (int)WorldUI.currentWeapon.compSprite.position.Y + 22, 
                new Point(16 * 8, 16 * 9 + 8), "Dungeon Map");
            //if hero is in shop, rename the window title
            if (Functions_Dungeon.dungeon.type == DungeonType.Shop)
            { window.title.text = "Shop Map"; }
            //set dungeonBkg rectangle
            dungeonBkg.Width = 16 * 7;
            dungeonBkg.Height = 16 * 7;
            dungeonBkg.X = window.background.position.X + 8;
            dungeonBkg.Y = window.background.position.Y + 20;
            //reset icon texts
            bossIcon.position.X = -100;
            keyIcon.position.X = -100;
            hubIcon.position.X = -100;
            exitIcon.position.X = -100;
            //we dont change the position of rooms or doors
            //thats done in the SyncToDungeon() method below
        }

        public override void Update()
        {
            Functions_MenuWindow.Update(window);
        }

        public override void Draw()
        {
            Functions_Draw.Draw(window);
            if (window.interior.displayState == DisplayState.Opened)
            {
                //draw dungeon background
                ScreenManager.spriteBatch.Draw(
                    Assets.dummyTexture, dungeonBkg,
                    Assets.colorScheme.mapBkg);


                #region Draw dungeon rooms

                for (i = 0; i < rooms.Count; i++)
                {   //do we need to draw this room?
                    if (rooms[i].visited || Functions_Dungeon.dungeon.map)
                    {
                        if (rooms[i].visited)
                        {   //draw visited rooms with Visited color
                            ScreenManager.spriteBatch.Draw(
                                Assets.dummyTexture, rooms[i].rec,
                                Assets.colorScheme.mapVisited);
                        }
                        else//hero has map
                        {   //draw unvisited rooms with NotVisited color
                            ScreenManager.spriteBatch.Draw(
                                Assets.dummyTexture, rooms[i].rec,
                                Assets.colorScheme.mapNotVisited);
                        }
                    }
                }

                #endregion


                #region Draw dungeon doors

                for (i = 0; i < doors.Count; i++)
                {   //do we need to draw this door?
                    if (doors[i].visited || Functions_Dungeon.dungeon.map)
                    {
                        if (doors[i].visited)
                        {   //draw visited doors with Visited color
                            ScreenManager.spriteBatch.Draw(
                                Assets.dummyTexture, doors[i].rec,
                                Assets.colorScheme.mapVisited);
                        }
                        else//hero has map
                        {   //draw unvisited doors with NotVisited color
                            ScreenManager.spriteBatch.Draw(
                                Assets.dummyTexture, doors[i].rec,
                                Assets.colorScheme.mapNotVisited);
                        }
                    }
                }

                #endregion


                //draw the debugging room icons
                Functions_Draw.Draw(bossIcon);
                Functions_Draw.Draw(keyIcon);
                Functions_Draw.Draw(hubIcon);
                Functions_Draw.Draw(exitIcon);

                //draw currentRoom rec
                ScreenManager.spriteBatch.Draw(
                    Assets.dummyTexture, currentRoom,
                    Assets.colorScheme.mapCurrentRoom * currentRoomAlpha);
                //pulse the alpha value
                if (currentRoomAlpha < 1.0) { currentRoomAlpha += 0.05f; }
                else { currentRoomAlpha = 0.0f; }
            }
        }





        public void SyncToDungeon()
        {

            #region Collect all the dungeon rooms

            rooms = new List<Room>();
            Vector2 iconPos;
            Room dungeonRoom;

            for (i = 0; i < Functions_Dungeon.dungeon.rooms.Count; i++)
            {
                dungeonRoom = Functions_Dungeon.dungeon.rooms[i];
                Room mapRoom = new Room(new Point(0, 0), dungeonRoom.type);
                //get the room size
                mapRoom.rec.Width = dungeonRoom.size.X;
                mapRoom.rec.Height = dungeonRoom.size.Y;
                //get the room position
                mapRoom.rec.X = dungeonRoom.rec.X;
                mapRoom.rec.Y = dungeonRoom.rec.Y;
                //subtract the build position
                mapRoom.rec.X -= Functions_Dungeon.buildPosition.X;
                mapRoom.rec.Y -= Functions_Dungeon.buildPosition.Y;
                //reduce position 16:1
                mapRoom.rec.X = mapRoom.rec.X / 16;
                mapRoom.rec.Y = mapRoom.rec.Y / 16;
                //add the map offset
                mapRoom.rec.X += dungeonBkg.X + (dungeonBkg.Width / 2) - 8;
                mapRoom.rec.Y += dungeonBkg.Y + (dungeonBkg.Height / 2) + 32;
                //grab the visited boolean
                mapRoom.visited = dungeonRoom.visited;
                rooms.Add(mapRoom);
                //display map icons, if we are map cheating (or debugging)
                if (Flags.MapCheat)
                {   //check to see if this room gets an icon
                    iconPos = new Vector2( //the center of the current mapRoom
                        mapRoom.rec.X + (mapRoom.rec.Width / 2) - 1,
                        mapRoom.rec.Y + (mapRoom.rec.Height / 2) - 7);
                    if (dungeonRoom.type == RoomType.Boss) { bossIcon.position = iconPos; }
                    else if (dungeonRoom.type == RoomType.Key) { keyIcon.position = iconPos; }
                    else if (dungeonRoom.type == RoomType.Hub) { hubIcon.position = iconPos; }
                    else if (dungeonRoom.type == RoomType.Exit) { exitIcon.position = iconPos; }
                }
            }

            #endregion


            #region Collect all the dungeon doors

            doors = new List<Door>();
            for (i = 0; i < Functions_Dungeon.dungeon.doors.Count; i++)
            {
                Door dungeonDoor = new Door(new Point());
                //map doors are always 1x1 pixels
                dungeonDoor.rec.Width = 1;
                dungeonDoor.rec.Height = 1;
                //get the door position
                dungeonDoor.rec.X = Functions_Dungeon.dungeon.doors[i].rec.X;
                dungeonDoor.rec.Y = Functions_Dungeon.dungeon.doors[i].rec.Y;
                //subtract the build position
                dungeonDoor.rec.X -= Functions_Dungeon.buildPosition.X;
                dungeonDoor.rec.Y -= Functions_Dungeon.buildPosition.Y;
                //reduce position 16:1
                dungeonDoor.rec.X = dungeonDoor.rec.X / 16;
                dungeonDoor.rec.Y = dungeonDoor.rec.Y / 16;
                //add the map offset
                dungeonDoor.rec.X += dungeonBkg.X + (dungeonBkg.Width / 2) - 8;
                dungeonDoor.rec.Y += dungeonBkg.Y + (dungeonBkg.Height / 2) + 32;
                //grab the visited boolean
                dungeonDoor.visited = Functions_Dungeon.dungeon.doors[i].visited;
                doors.Add(dungeonDoor);
            }

            #endregion

        }

        public void GetCurrentRoom()
        {   //rooms list is identical copy of dungeon.rooms
            for (i = 0; i < Functions_Dungeon.dungeon.rooms.Count; i++)
            {
                if (Functions_Dungeon.dungeon.rooms[i] == Functions_Dungeon.currentRoom)
                {   //match currentRoom properties to rooms[i] (current room)
                    currentRoom.X = rooms[i].rec.X + (rooms[i].rec.Width / 2);
                    currentRoom.Y = rooms[i].rec.Y + (rooms[i].rec.Height / 2);
                }
            }
        }

        //sync to dungeon should be called when a dungeon is built
        //get current room should be called when a room is built

    }
}