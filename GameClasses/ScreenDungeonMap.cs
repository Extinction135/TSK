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
    public class ScreenDungeonMap : Screen
    {
        ScreenRec background = new ScreenRec();
        int i;
        int j;

        ComponentText title;
        MenuRectangle headerLine;

        public List<Room> rooms;
        public List<Door> doors;
        public Rectangle marker = new Rectangle(-100, -100, 1, 1); //marks current room
        public float markerAlpha = 0.0f;
        public ComponentText bossIcon = new ComponentText(Assets.font, "B", new Vector2(), Assets.colorScheme.textDark);
        public ComponentText keyIcon = new ComponentText(Assets.font, "K", new Vector2(), Assets.colorScheme.textDark);
        public ComponentText hubIcon = new ComponentText(Assets.font, "H", new Vector2(), Assets.colorScheme.textDark);
        public ComponentText exitIcon = new ComponentText(Assets.font, "E", new Vector2(), Assets.colorScheme.textDark);

        public List<ComponentSprite> leftScroll = new List<ComponentSprite>();
        public List<ComponentSprite> rightScroll = new List<ComponentSprite>();
        public List<ComponentSprite> scrollBkg = new List<ComponentSprite>();
        Vector2 startPos = new Vector2(16 * 14, 16 * 6);
        Vector2 endPos;
        Vector2 columnPos = new Vector2(16 * 14, 16 * 6); //scroll top left pos
        int scrollHeight = 10;
        int scrollWidth = 11;
        int animSpeed = 8;


        public ScreenDungeonMap() { this.name = "DungeonMap"; }

        public override void LoadContent()
        {
            background.alpha = 0.0f; //fade bkg in
            background.maxAlpha = 0.6f;
            displayState = DisplayState.Opening;
            Assets.Play(Assets.sfxMapOpen);

            title = new ComponentText(Assets.font,
                "Dungeon Map", columnPos + new Vector2(20, 6),
                Assets.colorScheme.textDark);
            headerLine = new MenuRectangle(
                new Point((int)title.position.X + 0, (int)title.position.Y + 14),
                new Point(16 * scrollWidth - 23, 1),
                Assets.colorScheme.windowInset);

            //create left scroll
            Functions_Component.CreateScrollColumn(false, false, scrollHeight, columnPos, leftScroll);
            //create scroll's background columns
            for (j = 0; j < scrollWidth; j++)
            {   
                columnPos += new Vector2(16, 0);
                Functions_Component.CreateScrollColumn(true, false, scrollHeight, columnPos, scrollBkg);
            }
            //create right scroll
            Functions_Component.CreateScrollColumn(false, true, scrollHeight, startPos + new Vector2(32, 0), rightScroll);
            //set right scroll's end position
            endPos = new Vector2(startPos.X + ((scrollWidth + 1) * 16), startPos.Y);

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
                //center to screen
                mapRoom.rec.X += (640 / 2) - 8;
                mapRoom.rec.Y += (360 / 2) + 32;
                //get visibility
                mapRoom.visited = dungeonRoom.visited;
                //set marker to currentRoom position 
                if (dungeonRoom == Functions_Dungeon.currentRoom)
                {   //match currentRoom properties to rooms[i] (current room)
                    marker.X = mapRoom.rec.X + (mapRoom.rec.Width / 2);
                    marker.Y = mapRoom.rec.Y + (mapRoom.rec.Height / 2);
                }
                //add the mapRoom to rooms list
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
                //center to screen
                dungeonDoor.rec.X += (640 / 2) - 8;
                dungeonDoor.rec.Y += (360 / 2) + 32;
                //get visibility
                dungeonDoor.visited = Functions_Dungeon.dungeon.doors[i].visited;
                //add door to doors list
                doors.Add(dungeonDoor);
            }

            #endregion

        }

        public override void HandleInput(GameTime GameTime)
        {
            if (displayState == DisplayState.Opened)
            {
                if (Functions_Input.IsNewButtonPress(Buttons.Back) ||
                    Functions_Input.IsNewButtonPress(Buttons.B))
                {
                    Assets.Play(Assets.sfxInventoryClose);
                    displayState = DisplayState.Closing;
                }
            }
        }

        public override void Update(GameTime GameTime)
        {

            #region Handle Screen State

            if (displayState == DisplayState.Opening)
            {   //fade background in
                background.fadeState = FadeState.FadeIn;
                Functions_ScreenRec.Fade(background);

                //animate the right pillar to the right
                if (rightScroll[0].position.X < endPos.X)
                {   //animate the rightScroll right
                    for (i = 0; i < rightScroll.Count; i++)
                    {
                        rightScroll[i].position.X += (endPos.X - rightScroll[i].position.X) / animSpeed;
                        rightScroll[i].position.X += 1;
                        if (rightScroll[i].position.X > endPos.X)
                        { rightScroll[i].position.X = endPos.X; }
                    }
                }
                else { displayState = DisplayState.Opened; }
            }
            else if (displayState == DisplayState.Closing)
            {   //fade background out
                background.fadeState = FadeState.FadeOut;
                Functions_ScreenRec.Fade(background);

                //animate the left pillar to the right
                if (leftScroll[0].position.X < endPos.X - 32)
                {   //animate the rightScroll right
                    for (i = 0; i < leftScroll.Count; i++)
                    {
                        leftScroll[i].position.X += (endPos.X - leftScroll[i].position.X) / animSpeed;
                        leftScroll[i].position.X += 1;
                        if (leftScroll[i].position.X > endPos.X - 32)
                        { leftScroll[i].position.X = endPos.X - 32; }
                    }
                }
                else { displayState = DisplayState.Closed; }
            }
            else if (displayState == DisplayState.Closed)
            { ScreenManager.RemoveScreen(this); }

            #endregion


            if (displayState == DisplayState.Opened)
            { Functions_MenuRectangle.Update(headerLine); }
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            Functions_Draw.Draw(background);

            if(displayState == DisplayState.Opening)
            {
                for (i = 0; i < scrollBkg.Count; i++)
                {   //if bkg sprite is left of animating right scroll, draw
                    if (scrollBkg[i].position.X < rightScroll[0].position.X)
                    { Functions_Draw.Draw(scrollBkg[i]); }
                }
            }
            else if(displayState == DisplayState.Closing)
            {
                for (i = 0; i < scrollBkg.Count; i++)
                {   //if bkg sprite is right of animating left scroll, draw
                    if (scrollBkg[i].position.X > leftScroll[0].position.X)
                    { Functions_Draw.Draw(scrollBkg[i]); }
                }
            }
            else if (displayState == DisplayState.Opened)
            {   //draw everything if screen is open
                for (i = 0; i < scrollBkg.Count; i++)
                { Functions_Draw.Draw(scrollBkg[i]); }

                Functions_Draw.Draw(title);
                Functions_Draw.Draw(headerLine);


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


                #region Draw Room Icons + Marker

                //draw the debugging room icons
                Functions_Draw.Draw(bossIcon);
                Functions_Draw.Draw(keyIcon);
                Functions_Draw.Draw(hubIcon);
                Functions_Draw.Draw(exitIcon);

                //draw marker
                ScreenManager.spriteBatch.Draw(
                    Assets.dummyTexture, marker,
                    Assets.colorScheme.mapBlinker * markerAlpha);
                //pulse marker alpha
                if (markerAlpha < 1.0) { markerAlpha += 0.05f; }
                else { markerAlpha = 0.0f; }

                #endregion

            }

            //always draw scroll pillars / ends
            for (i = 0; i < leftScroll.Count; i++)
            {   //these lists will always have same count
                Functions_Draw.Draw(leftScroll[i]);
                Functions_Draw.Draw(rightScroll[i]);
            }

            ScreenManager.spriteBatch.End();
        }

    }
}