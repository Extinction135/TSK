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
    public enum WidgetRoomToolsState { Room, Level }

    public class WidgetRoomTools : Widget
    {
        //based on state, we load roomData as a room or level
        WidgetRoomToolsState state = WidgetRoomToolsState.Level;
        //this is the only way to change the state rn
        public void SetState(WidgetRoomToolsState State)
        {   //capture the state change, set the window title
            state = State;
            if (state == WidgetRoomToolsState.Room)
            { window.title.text = "room tools"; }
            else { window.title.text = "level tools"; }
        }




        
        public RoomXmlData roomData; //the roomData loaded & built
        public RoomID roomID = RoomID.DEV_Field; //default to devfield

        public List<ComponentButton> buttons; //save, new, load buttons
        public ComponentButton saveBtn;
        public ComponentButton loadBtn;
        public ComponentButton newBtn;
        public ComponentButton roomTypeBtn;
        



        public WidgetRoomTools()
        {   //create Window
            window = new MenuWindow( 
                new Point(0, 0), 
                new Point(16 * 6, 16 * 4), 
                "Room Tools");
            //create Save/Play/Load New/Type/Reload Buttons
            buttons = new List<ComponentButton>();
            saveBtn = new ComponentButton("save", new Point(0, 0));
            loadBtn = new ComponentButton("load", new Point(0, 0));
            newBtn = new ComponentButton("new", new Point(0, 0));
            roomTypeBtn = new ComponentButton("-------", new Point(0, 0));
            
            buttons.Add(saveBtn);
            buttons.Add(loadBtn);
            buttons.Add(newBtn);
            buttons.Add(roomTypeBtn);

            roomTypeBtn.compText.text = "" + roomID;
            //center text to button, prevent half pixel offsets
            roomTypeBtn.compText.position.X =
                (int)(roomTypeBtn.rec.Location.X + roomTypeBtn.rec.Width / 2) 
                - (roomTypeBtn.textWidth / 2);
            //center text vertically
            roomTypeBtn.compText.position.Y = roomTypeBtn.rec.Location.Y - 3;
        }

        public override void Reset(int X, int Y)
        {
            Functions_MenuWindow.ResetAndMove(window, X, Y, window.size, window.title.text);
            
            //buttons
            saveBtn.rec.X = X + 16 * 1 - 8;
            saveBtn.rec.Y = Y + 16 * 1 + 8 - 2;
            Functions_Component.CenterText(saveBtn);
            saveBtn.rec.Width = 39; //

            loadBtn.rec.X = saveBtn.rec.X + saveBtn.rec.Width + 2;
            loadBtn.rec.Y = saveBtn.rec.Y;
            Functions_Component.CenterText(loadBtn);
            loadBtn.rec.Width = 39; //

            newBtn.rec.X = X + 16 * 1 - 8;
            newBtn.rec.Y = Y + 16 * 2 - 0 + 2;
            Functions_Component.CenterText(newBtn);
            newBtn.rec.Width = 20; //

            //determine what default room to display
            if (state == WidgetRoomToolsState.Room)
            { roomID = RoomID.DEV_Row; }
            else { roomID = RoomID.DEV_Field; }
            roomTypeBtn.compText.text = "" + roomID;

            roomTypeBtn.rec.X = newBtn.rec.X + newBtn.rec.Width + 2;
            roomTypeBtn.rec.Y = newBtn.rec.Y;
            Functions_Component.CenterText(roomTypeBtn);
            roomTypeBtn.rec.Width = 16 * 3 +10; //
        }

        public void HandleInput()
        {

            #region Handle Button Selection

            if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
            {   //if mouse is contained within widget
                if (window.interior.rec.Contains(Input.cursorPos))
                {  
                    for (i = 0; i < buttons.Count; i++)
                    {   //check to see if the user has clicked on a button
                        if (buttons[i].rec.Contains(Input.cursorPos))
                        {

                            #region Save

                            if (buttons[i] == saveBtn)
                            {
                                SaveCurrentRoom();
                            }

                            #endregion


                            #region Load

                            else if (buttons[i] == loadBtn)
                            {
                                Functions_Backend.SelectRoomFile();
                            }

                            #endregion


                            #region New Room 

                            else if (buttons[i] == newBtn)
                            {
                                //based on state, set level id
                                if (state == WidgetRoomToolsState.Room)
                                {   //default level to forest row room
                                    LevelSet.currentLevel.ID = LevelID.Forest_Dungeon;
                                }
                                else
                                {   //a blank, disconnected (from map) field
                                    LevelSet.currentLevel.ID = LevelID.DEV_Field;
                                    roomID = RoomID.DEV_Field;
                                }

                                //store the roomID in roomTool's roomData type
                                Widgets.RoomTools.roomData = new RoomXmlData();
                                Widgets.RoomTools.roomData.type = roomID;
                                //reference that roomData in buildLevel() to get roomID & levelID
                                Functions_Level.BuildLevel(LevelSet.currentLevel.ID);

                                Debug.WriteLine("level id: " + LevelSet.currentLevel.ID);
                                Debug.WriteLine("room id: " + roomID);
                            }

                            #endregion


                            #region Set Room Type

                            else if (buttons[i] == roomTypeBtn)
                            {
                                if (state == WidgetRoomToolsState.Room)
                                {
                                    //iterate thru a limited set of dungeon dev roomTypes
                                    if (roomID == RoomID.DEV_Boss) { roomID = RoomID.DEV_Column; }
                                    else if (roomID == RoomID.DEV_Column) { roomID = RoomID.DEV_Exit; }
                                    else if (roomID == RoomID.DEV_Exit) { roomID = RoomID.DEV_Hub; }
                                    else if (roomID == RoomID.DEV_Hub) { roomID = RoomID.DEV_Key; }
                                    else if (roomID == RoomID.DEV_Key) { roomID = RoomID.DEV_Row; }
                                    else if (roomID == RoomID.DEV_Row) { roomID = RoomID.DEV_Square; }
                                    else if (roomID == RoomID.DEV_Square) { roomID = RoomID.DEV_Boss; }
                                }
                                else
                                {   //editors only edit fields in overworld mode
                                    roomID = RoomID.DEV_Field;
                                }

                                //set the text in button to roomType enum
                                roomTypeBtn.compText.text = "" + roomID;
                            }

                            #endregion


                        }
                    }
                }
            }

            #endregion


            #region Handle Button Over/Up States

            for (i = 0; i < buttons.Count; i++)
            {   //by default, set buttons to up color
                buttons[i].currentColor = ColorScheme.buttonUp;
                //if user hovers over a button, set button to down color
                if (buttons[i].rec.Contains(Input.cursorPos))
                { buttons[i].currentColor = ColorScheme.buttonDown; }
            }

            #endregion

        }

        public override void Update()
        {
            Functions_MenuWindow.Update(window);
            if (window.interior.displayState == DisplayState.Opened)
            {
                //nothzing
            }
        }

        public override void Draw()
        {
            Functions_Draw.Draw(window);
            if (window.interior.displayState == DisplayState.Opened)
            {
                for (i = 0; i < buttons.Count; i++) //draw all the buttons
                { Functions_Draw.Draw(buttons[i]); }
            }
        }




        int p;
        public void SaveCurrentRoom()
        {   //create RoomXmlData instance
            roomData = new RoomXmlData();
            RoomID id = LevelSet.currentLevel.currentRoom.roomID; //shorten roomID
            
            //convert DEV room types into proper GAME room types
            if (id == RoomID.DEV_Boss) { roomData.type = RoomID.ForestIsland_BossRoom; }
            else if (id == RoomID.DEV_Column) { roomData.type = RoomID.ForestIsland_ColumnRoom; }
            else if (id == RoomID.DEV_Exit) { roomData.type = RoomID.ForestIsland_ExitRoom; }
            else if (id == RoomID.DEV_Hub) { roomData.type = RoomID.ForestIsland_HubRoom; }
            else if (id == RoomID.DEV_Key) { roomData.type = RoomID.ForestIsland_KeyRoom; }
            else if (id == RoomID.DEV_Row) { roomData.type = RoomID.ForestIsland_RowRoom; }
            else if (id == RoomID.DEV_Square) { roomData.type = RoomID.ForestIsland_SquareRoom; }
            else
            {   //we may be saving a proper GAME room/level or field
                roomData.type = id;
            }

            //transfer the levelID - we code this by hand now
            //roomData.levelID = LevelSet.currentLevel.currentRoom.levelID;

            //populate roomData with indestructible objects
            for (p = 0; p < Pool.indObjCount; p++)
            {
                if (Pool.indObjPool[p].active)
                {   
                    //translate Obj to IndObjXmlData (always saved)
                    IndObjXmlData objData = new IndObjXmlData();
                    objData.type = Pool.indObjPool[p].type;
                    objData.direction = Pool.indObjPool[p].direction;
                    //set saved obj's position relative to room's top left corner
                    objData.posX = Pool.indObjPool[p].compSprite.position.X - LevelSet.currentLevel.currentRoom.rec.X;
                    objData.posY = Pool.indObjPool[p].compSprite.position.Y - LevelSet.currentLevel.currentRoom.rec.Y;
                    roomData.inds.Add(objData);
                }
            }

            //populate roomData with interactive objects
            for (p = 0; p < Pool.intObjCount; p++)
            {
                if (Pool.intObjPool[p].active)
                {   //if this object is active & can be saved
                    if (Pool.intObjPool[p].canBeSaved)
                    {   //translate Obj to IntObjXmlData
                        IntObjXmlData objData = new IntObjXmlData();
                        objData.type = Pool.intObjPool[p].type;
                        objData.direction = Pool.intObjPool[p].direction;
                        //set saved obj's position relative to room's top left corner
                        objData.posX = Pool.intObjPool[p].compSprite.position.X - LevelSet.currentLevel.currentRoom.rec.X;
                        objData.posY = Pool.intObjPool[p].compSprite.position.Y - LevelSet.currentLevel.currentRoom.rec.Y;
                        roomData.ints.Add(objData);
                    }
                }
            }
            
            //send the new roomdata to be saved
            Functions_Backend.SaveRoomData(roomData);
        }

       

        public void BuildFromFile(RoomXmlData RoomXmlData)
        {   //called at end of functions_backend (directx or uwp) 
            //after it's deserialized xml into Widgets.RoomTools.roomData







            if (state == WidgetRoomToolsState.Room)
            {
                //set level to dungeon
                LevelSet.currentLevel = LevelSet.dungeon;
                LevelSet.currentLevel.ID = LevelID.Forest_Dungeon; //default to forest dungeon
                LevelSet.currentLevel.rooms = new List<Room>();
                LevelSet.currentLevel.doors = new List<Door>();
                LevelSet.currentLevel.isField = false;

                //reset the level flags
                LevelSet.currentLevel.bigKey = false;
                LevelSet.currentLevel.map = false;

                //create and set the room
                Room room = new Room(Functions_Level.buildPosition, RoomXmlData.type);
                LevelSet.currentLevel.rooms.Add(room);
                LevelSet.currentLevel.currentRoom = room;

                Functions_Room.AddDevDoors(room);
                //build walled empty room with floors, add xml objs, etc...
                Functions_Room.Build_DUNGEON_RoomFrom(RoomXmlData);

                //set spawnPos outside TopLeft of new dev room
                LevelSet.spawnPos_Dungeon.X = LevelSet.currentLevel.currentRoom.rec.X - 32;
                LevelSet.spawnPos_Dungeon.Y = LevelSet.currentLevel.currentRoom.rec.Y;
                Functions_Hero.SpawnInCurrentRoom(); //spawn hero in room

                //set camera to center of room (so we can see it upon load)
                Camera2D.targetPosition.X = LevelSet.currentLevel.currentRoom.center.X;
                Camera2D.targetPosition.Y = LevelSet.currentLevel.currentRoom.center.Y;
            }
            else
            {
                //set level to field
                LevelSet.currentLevel = LevelSet.field;
                //LevelSet.currentLevel.ID = LevelID.SkullIsland_Colliseum; //default to skull coliseum
                LevelSet.currentLevel.rooms = new List<Room>();
                LevelSet.currentLevel.isField = true;

                //create and set the room
                Room room = new Room(Functions_Level.buildPosition, RoomXmlData.type);
                LevelSet.currentLevel.rooms.Add(room); //index0
                LevelSet.currentLevel.rooms[0].visited = true;
                LevelSet.currentLevel.currentRoom = LevelSet.currentLevel.rooms[0];
                //LevelSet.currentLevel.currentRoom = LevelSet.field.rooms[0]; //= room;
                //Functions_Room.BuildRoom(LevelSet.currentLevel.currentRoom);
                //build the room with the xml data
                Functions_Room.BuildRoom(LevelSet.currentLevel.currentRoom, RoomXmlData);


                //set spawnPos
                Functions_Hero.ResetFieldSpawnPos();
                Functions_Hero.SpawnInCurrentRoom(); //spawn hero in room
                //set camera to room spawnpos (where hero just spawned)
                Camera2D.targetPosition.X = LevelSet.spawnPos_Dungeon.X;
                Camera2D.targetPosition.Y = LevelSet.spawnPos_Dungeon.Y;
            }

            //teleport camera to it's starting position, set above
            Camera2D.currentPosition = Camera2D.targetPosition;
            Functions_Camera2D.SetView();

            //update editor world once, but pause game
            Screens.Editor.Update(ScreenManager.gameTime);
            Screens.Editor.Draw(ScreenManager.gameTime);
            Flags.Paused = true;
        }





    }
}