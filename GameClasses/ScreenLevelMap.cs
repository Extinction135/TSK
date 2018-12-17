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
    public class ScreenLevelMap : Screen
    {
        ScreenRec background = new ScreenRec();
        Scroll scroll;
        int i;

        public List<Room> rooms;
        public List<Door> doors;
        public Rectangle marker = new Rectangle(-100, -100, 1, 1); //marks current room
        public float markerAlpha = 0.0f;
        public ComponentText bossIcon = new ComponentText(
            Assets.font, "B", new Vector2(), ColorScheme.textDark);
        public ComponentText keyIcon = new ComponentText(
            Assets.font, "K", new Vector2(), ColorScheme.textDark);
        public ComponentText hubIcon = new ComponentText(
            Assets.font, "H", new Vector2(), ColorScheme.textDark);
        public ComponentText exitIcon = new ComponentText(
            Assets.font, "E", new Vector2(), ColorScheme.textDark);

        int verticalOffset = 16 * 2; //how far map is pushed down from screen center



        
        public ScreenLevelMap()
        {
            this.name = "DungeonMap";
            //create scroll instance
            scroll = new Scroll(
                new Vector2(16 * 12, 16 * 4 - 8),
                15, 16);
            scroll.title.text = "Dungeon Map";
            scroll.displayState = DisplayState.Opening;
        }

        public override void Open()
        {
            background.alpha = 0.0f; //fade bkg in
            background.maxAlpha = 0.6f;
            Assets.Play(Assets.sfxMapOpen);

            //create rooms and doors lists
            rooms = new List<Room>();
            doors = new List<Door>();


            #region Collect all the dungeon rooms

            rooms = new List<Room>();
            Vector2 iconPos;
            Room dungeonRoom;

            //hide icons + marker
            bossIcon.position.X = -100;
            keyIcon.position.X = -100;
            hubIcon.position.X = -100;
            exitIcon.position.X = -100;
            marker.X = -100;

            for (i = 0; i < LevelSet.currentLevel.rooms.Count; i++)
            {
                dungeonRoom = LevelSet.currentLevel.rooms[i];
                Room mapRoom = new Room(new Point(0, 0), dungeonRoom.roomID);
                //get the room size
                mapRoom.rec.Width = dungeonRoom.size.X;
                mapRoom.rec.Height = dungeonRoom.size.Y;
                //get the room position
                mapRoom.rec.X = dungeonRoom.rec.X;
                mapRoom.rec.Y = dungeonRoom.rec.Y;
                //subtract the build position
                mapRoom.rec.X -= Functions_Level.buildPosition.X;
                mapRoom.rec.Y -= Functions_Level.buildPosition.Y;
                //reduce position 16:1
                mapRoom.rec.X = mapRoom.rec.X / 16;
                mapRoom.rec.Y = mapRoom.rec.Y / 16;
                //center to screen
                mapRoom.rec.X += (640 / 2) - 8; //horizontal offset
                mapRoom.rec.Y += (360 / 2) + verticalOffset; //vertical offset
                //get visibility
                mapRoom.visited = dungeonRoom.visited;
                //set marker to currentRoom position 
                if (dungeonRoom == LevelSet.currentLevel.currentRoom)
                {   //match currentRoom properties to rooms[i] (current room)
                    marker.X = mapRoom.rec.X + (mapRoom.rec.Width / 2);
                    marker.Y = mapRoom.rec.Y + (mapRoom.rec.Height / 2);
                }
                //add the mapRoom to rooms list
                rooms.Add(mapRoom);

                //'visit' allrooms if map cheating
                if (Flags.MapCheat) { mapRoom.visited = true; }
                //display map icons if room visited (keep track while exploring)
                if (mapRoom.visited)
                {   
                    iconPos = new Vector2( //the center of the current mapRoom
                        mapRoom.rec.X + (mapRoom.rec.Width / 2) - 1,
                        mapRoom.rec.Y + (mapRoom.rec.Height / 2) - 7);
                    if (
                        dungeonRoom.roomID == RoomID.DEV_Boss ||
                        dungeonRoom.roomID == RoomID.ForestIsland_BossRoom ||
                        dungeonRoom.roomID == RoomID.DeathMountain_BossRoom ||
                        dungeonRoom.roomID == RoomID.HauntedSwamps_BossRoom ||
                        dungeonRoom.roomID == RoomID.ThievesHideout_BossRoom ||
                        dungeonRoom.roomID == RoomID.LavaIsland_BossRoom ||
                        dungeonRoom.roomID == RoomID.CloudIsland_BossRoom ||
                        dungeonRoom.roomID == RoomID.SkullIsland_BossRoom
                        )
                    { bossIcon.position = iconPos; }
                    else if (
                        dungeonRoom.roomID == RoomID.DEV_Hub ||
                        dungeonRoom.roomID == RoomID.ForestIsland_HubRoom ||
                        dungeonRoom.roomID == RoomID.DeathMountain_HubRoom ||
                        dungeonRoom.roomID == RoomID.HauntedSwamps_HubRoom ||
                        dungeonRoom.roomID == RoomID.ThievesHideout_HubRoom ||
                        dungeonRoom.roomID == RoomID.LavaIsland_HubRoom ||
                        dungeonRoom.roomID == RoomID.CloudIsland_HubRoom ||
                        dungeonRoom.roomID == RoomID.SkullIsland_HubRoom
                        )
                    { hubIcon.position = iconPos; }
                    else if (
                        dungeonRoom.roomID == RoomID.DEV_Key ||
                        dungeonRoom.roomID == RoomID.ForestIsland_KeyRoom ||
                        dungeonRoom.roomID == RoomID.DeathMountain_KeyRoom ||
                        dungeonRoom.roomID == RoomID.HauntedSwamps_KeyRoom ||
                        dungeonRoom.roomID == RoomID.ThievesHideout_KeyRoom ||
                        dungeonRoom.roomID == RoomID.LavaIsland_KeyRoom ||
                        dungeonRoom.roomID == RoomID.CloudIsland_KeyRoom ||
                        dungeonRoom.roomID == RoomID.SkullIsland_KeyRoom
                        )
                    { keyIcon.position = iconPos; }
                    else if (
                        dungeonRoom.roomID == RoomID.DEV_Exit ||
                        dungeonRoom.roomID == RoomID.ForestIsland_ExitRoom ||
                        dungeonRoom.roomID == RoomID.DeathMountain_ExitRoom ||
                        dungeonRoom.roomID == RoomID.HauntedSwamps_ExitRoom ||
                        dungeonRoom.roomID == RoomID.ThievesHideout_ExitRoom ||
                        dungeonRoom.roomID == RoomID.LavaIsland_ExitRoom ||
                        dungeonRoom.roomID == RoomID.CloudIsland_ExitRoom ||
                        dungeonRoom.roomID == RoomID.SkullIsland_ExitRoom
                        )
                    { exitIcon.position = iconPos; }
                }
            }

            #endregion


            #region Collect all the dungeon doors

            doors = new List<Door>();
            for (i = 0; i < LevelSet.currentLevel.doors.Count; i++)
            {
                Door dungeonDoor = new Door(new Point());
                //map doors are always 1x1 pixels
                dungeonDoor.rec.Width = 1;
                dungeonDoor.rec.Height = 1;
                //get the door position
                dungeonDoor.rec.X = LevelSet.currentLevel.doors[i].rec.X;
                dungeonDoor.rec.Y = LevelSet.currentLevel.doors[i].rec.Y;
                //subtract the build position
                dungeonDoor.rec.X -= Functions_Level.buildPosition.X;
                dungeonDoor.rec.Y -= Functions_Level.buildPosition.Y;
                //reduce position 16:1
                dungeonDoor.rec.X = dungeonDoor.rec.X / 16;
                dungeonDoor.rec.Y = dungeonDoor.rec.Y / 16;
                //center to screen
                dungeonDoor.rec.X += (640 / 2) - 8;
                dungeonDoor.rec.Y += (360 / 2) + verticalOffset;
                //get visibility
                dungeonDoor.visited = LevelSet.currentLevel.doors[i].visited;
                //add door to doors list
                doors.Add(dungeonDoor);
            }

            #endregion

            //reset the scroll to an open state, then open the screen
            Functions_Scroll.ResetScroll(scroll);
            scroll.displayState = DisplayState.Opening;
        }

        public override void HandleInput(GameTime GameTime)
        {
            //Exit Screen Input
            if (scroll.displayState == DisplayState.Opened)
            {   //exit this screen upon start / b button
                if (
                    (Input.Player1.Start & Input.Player1.Start_Prev == false)
                    ||
                    (Input.Player1.B & Input.Player1.B_Prev == false)
                    )
                {
                    Assets.Play(Assets.sfxWindowClose);
                    scroll.displayState = DisplayState.Closing;
                }
            }
        }

        public override void Update(GameTime GameTime)
        {
            if (scroll.displayState == DisplayState.Opening)
            {   //fade background in
                background.fadeState = FadeState.FadeIn;
                Functions_ScreenRec.Fade(background);
                Functions_Scroll.AnimateOpen(scroll);
            }
            else if (scroll.displayState == DisplayState.Closing)
            {   //fade background out
                background.fadeState = FadeState.FadeOut;
                Functions_ScreenRec.Fade(background);
                Functions_Scroll.AnimateClosed(scroll);
            }
            else if (scroll.displayState == DisplayState.Closed)
            { ScreenManager.RemoveScreen(this); }
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(
                SpriteSortMode.Deferred, 
                BlendState.AlphaBlend, 
                SamplerState.PointClamp);
            Functions_Draw.Draw(background);
            Functions_Scroll.Draw(scroll);
            if (scroll.displayState == DisplayState.Opened)
            {   //draw map if scroll is open

                #region Draw dungeon rooms

                for (i = 0; i < rooms.Count; i++)
                {   //do we need to draw this room?
                    if (rooms[i].visited || LevelSet.currentLevel.map)
                    {
                        if (rooms[i].visited)
                        {   //draw visited rooms with Visited color
                            ScreenManager.spriteBatch.Draw(
                                Assets.dummyTexture, rooms[i].rec,
                                ColorScheme.mapVisited);
                        }
                        else//hero has map
                        {   //draw unvisited rooms with NotVisited color
                            ScreenManager.spriteBatch.Draw(
                                Assets.dummyTexture, rooms[i].rec,
                                ColorScheme.mapNotVisited);
                        }
                    }
                }

                #endregion


                #region Draw dungeon doors

                for (i = 0; i < doors.Count; i++)
                {   //do we need to draw this door?
                    if (doors[i].visited || LevelSet.currentLevel.map)
                    {
                        if (doors[i].visited)
                        {   //draw visited doors with Visited color
                            ScreenManager.spriteBatch.Draw(
                                Assets.dummyTexture, doors[i].rec,
                                ColorScheme.mapVisited);
                        }
                        else//hero has map
                        {   //draw unvisited doors with NotVisited color
                            ScreenManager.spriteBatch.Draw(
                                Assets.dummyTexture, doors[i].rec,
                                ColorScheme.mapNotVisited);
                        }
                    }
                }

                #endregion


                #region Draw Room Icons + Marker

                //draw the debugging room icons
                Functions_Draw.Draw(bossIcon);
                Functions_Draw.Draw(keyIcon);
                Functions_Draw.Draw(hubIcon);
                Functions_Draw.Draw(exitIcon);

                //draw marker
                ScreenManager.spriteBatch.Draw(
                    Assets.dummyTexture, marker,
                    ColorScheme.mapBlinker * markerAlpha);
                //pulse marker alpha
                if (markerAlpha < 1.0) { markerAlpha += 0.05f; }
                else { markerAlpha = 0.0f; }

                #endregion

            }
            ScreenManager.spriteBatch.End();
        }

    }
}