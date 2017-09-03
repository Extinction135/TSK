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
    public class WidgetRoomTools : Widget
    {
        public RoomXmlData roomData; //the roomData loaded & built
        public RoomType roomType;

        public List<ComponentButton> buttons; //save, new, load buttons
        public ComponentButton saveBtn;
        public ComponentButton loadBtn;
        public ComponentButton newRoomBtn;
        public ComponentButton roomTypeBtn;
        public ComponentButton reloadRoomBtn;

        public ComponentText roomNameText;



        public WidgetRoomTools()
        {   //create Window
            window = new MenuWindow( new Point(0, 0), 
                new Point(16 * 6, 16 * 5 + 8), "Room Tools");
            //create Save/Play/Load New/Type/Reload Buttons
            buttons = new List<ComponentButton>();
            saveBtn = new ComponentButton("save", new Point(0, 0));
            loadBtn = new ComponentButton("load", new Point(0, 0));
            newRoomBtn = new ComponentButton("new room", new Point(0, 0));
            roomTypeBtn = new ComponentButton("column", new Point(0, 0));
            reloadRoomBtn = new ComponentButton("reload", new Point(0, 0));

            buttons.Add(saveBtn);
            buttons.Add(loadBtn);
            buttons.Add(newRoomBtn);
            buttons.Add(roomTypeBtn);
            buttons.Add(reloadRoomBtn);

            roomType = RoomType.Column;
            roomNameText = new ComponentText(Assets.font, "filename", new Vector2(0, 0), Assets.colorScheme.textDark);
        }

        public override void Reset(int X, int Y)
        {
            Functions_MenuWindow.ResetAndMove(window, X, Y, window.size, window.title.text);
            
            //Move Buttons
            saveBtn.rec.X = X + 16 * 1 - 8;
            saveBtn.rec.Y = Y + 16 * 1 + 8;
            Functions_Component.CenterText(saveBtn);

            reloadRoomBtn.rec.X = X + 16 * 1 + 17;
            reloadRoomBtn.rec.Y = Y + 16 * 1 + 8;
            Functions_Component.CenterText(reloadRoomBtn);

            loadBtn.rec.X = X + 16 * 4 + 10 - 8;
            loadBtn.rec.Y = Y + 16 * 1 + 8;
            Functions_Component.CenterText(loadBtn);

            newRoomBtn.rec.X = X + 16 * 1 - 8;
            newRoomBtn.rec.Y = Y + 16 * 2 + 8;
            Functions_Component.CenterText(newRoomBtn);

            roomTypeBtn.rec.X = X + 16 * 4 - 8 - 1;
            roomTypeBtn.rec.Y = Y + 16 * 2 + 8;
            Functions_Component.CenterText(roomTypeBtn);

            roomNameText.position.X = X + 16 * 1 - 8;
            roomNameText.position.Y = Y + 16 * 3 + 5;
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

                            #region Save Button

                            if (buttons[i] == saveBtn)
                            {
                                //create RoomXmlData instance
                                roomData = new RoomXmlData();
                                roomData.type = Functions_Level.currentRoom.type; //save the room type

                                //populate roomData with roomObjs
                                for (Pool.roomObjCounter = 0; Pool.roomObjCounter < Pool.roomObjCount; Pool.roomObjCounter++)
                                { SaveObject(Pool.roomObjPool[Pool.roomObjCounter], roomData); }

                                //populate roomData with entities
                                for (Pool.entityCounter = 0; Pool.entityCounter < Pool.entityCount; Pool.entityCounter++)
                                { SaveObject(Pool.entityPool[Pool.entityCounter], roomData); }

                                Functions_Backend.SaveRoomData(roomData);
                            }

                            #endregion


                            #region Load Button

                            else if (buttons[i] == loadBtn)
                            {
                                Functions_Backend.SelectRoomFile();
                            }

                            #endregion


                            #region New Room Button

                            else if (buttons[i] == newRoomBtn)
                            {   //create a new room based on the type buttons state
                                roomData = new RoomXmlData();
                                roomData.type = roomType;
                                BuildRoomData(roomData);
                            }

                            #endregion


                            #region Room Type Button

                            else if (buttons[i] == roomTypeBtn)
                            {   //iterate thru a limited set of roomTypes
                                if (roomType == RoomType.Column) { roomType = RoomType.Row; }
                                else if (roomType == RoomType.Row) { roomType = RoomType.Square; }
                                else if (roomType == RoomType.Square) { roomType = RoomType.Hub; }
                                else if (roomType == RoomType.Hub) { roomType = RoomType.Boss; }
                                else if (roomType == RoomType.Boss) { roomType = RoomType.Key; }
                                else if (roomType == RoomType.Key) { roomType = RoomType.Column; }
                                //set the text in button to roomType enum
                                roomTypeBtn.compText.text = "" + roomType;
                            }

                            #endregion


                            #region Reload Button

                            else if (buttons[i] == reloadRoomBtn)
                            {   //assumes roomData hasn't changed since last build
                                BuildRoomData(roomData);
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
                buttons[i].currentColor = Assets.colorScheme.buttonUp;
                //if user hovers over a button, set button to down color
                if (buttons[i].rec.Contains(Input.cursorPos))
                { buttons[i].currentColor = Assets.colorScheme.buttonDown; }
            }

            #endregion

        }

        public override void Update()
        {
            Functions_MenuWindow.Update(window);
            if (window.interior.displayState == DisplayState.Opened)
            {

            }
        }

        public override void Draw()
        {
            Functions_Draw.Draw(window);
            if (window.interior.displayState == DisplayState.Opened)
            {
                for (i = 0; i < buttons.Count; i++) //draw all the buttons
                { Functions_Draw.Draw(buttons[i]); }
                Functions_Draw.Draw(roomNameText);
            }
        }




        public void SaveObject(GameObject Obj, RoomXmlData RoomData)
        {
            if (Obj.active)
            {   //if this object is active & can be saved
                if (Obj.canBeSaved)
                {   //translate Obj to ObjXmlData, add it to the roomData.objs list
                    ObjXmlData objData = new ObjXmlData();
                    objData.type = Obj.type;
                    objData.direction = Obj.direction;
                    //set saved obj's position relative to room's top left corner
                    objData.posX = Obj.compSprite.position.X - Functions_Level.currentRoom.rec.X;
                    objData.posY = Obj.compSprite.position.Y - Functions_Level.currentRoom.rec.Y;
                    RoomData.objs.Add(objData);
                }
            }
        }

        public void BuildRoomData(RoomXmlData RoomXmlData)
        {
            Functions_Level.ResetLevel();
            //this can be whatever texture we want later
            Level.type = LevelType.Castle;
            Functions_Pool.SetFloorTexture(LevelType.Castle);

            //build the room, if room data exists
            if (RoomXmlData != null)
            { Functions_Level.currentRoom = new Room(Functions_Level.buildPosition, RoomXmlData.type); }
            else //if room data doesn't exist, create a default row room
            { Functions_Level.currentRoom = new Room(Functions_Level.buildPosition, RoomType.Row); }

            //add this room to the dungeon.rooms list
            Level.rooms.Add(Functions_Level.currentRoom);
            Functions_Level.currentRoom.visited = true;
            if (Flags.MapCheat) { Level.map = true; }
            else { Level.map = false; }

            //simplify / collect room values
            int posX = Functions_Level.currentRoom.rec.X;
            int posY = Functions_Level.currentRoom.rec.Y;
            int middleX = (Functions_Level.currentRoom.size.X / 2) * 16;
            int middleY = (Functions_Level.currentRoom.size.Y / 2) * 16;
            int width = Functions_Level.currentRoom.size.X * 16;
            int height = Functions_Level.currentRoom.size.Y * 16;

            //set NSEW doors
            Level.doors.Add(new Door(new Point(posX + middleX, posY - 16))); //top
            Level.doors.Add(new Door(new Point(posX + middleX, posY + height))); //bottom
            Level.doors.Add(new Door(new Point(posX - 16, posY + middleY))); //left
            Level.doors.Add(new Door(new Point(posX + width, posY + middleY))); //right

            //releases all roomObjs, builds walls + floors + doors
            Functions_Room.BuildRoom(Functions_Level.currentRoom);
            //build the xml data
            Functions_Room.BuildRoomXmlData(RoomXmlData);

            //add debris + cracked walls
            Functions_Room.ScatterDebris(Functions_Level.currentRoom);
            Functions_Room.AddCrackedWalls(Functions_Level.currentRoom);
            
            //align + remove overlapping objs
            Functions_Pool.AlignRoomObjs();
            Functions_Room.CleanupRoom(Functions_Level.currentRoom);
            //update roomObjs once
            Functions_Pool.Update();

            //set spawnPos outside TopLeft of newly built room
            Functions_Level.currentRoom.spawnPos.X = Functions_Level.currentRoom.rec.X - 32;
            Functions_Level.currentRoom.spawnPos.Y = Functions_Level.currentRoom.rec.Y;
            Functions_Room.SpawnHeroInCurrentRoom(); //spawn hero in room
            Pool.hero.direction = Direction.Down; //face hero down
            Flags.Paused = true;
        }


    }
}