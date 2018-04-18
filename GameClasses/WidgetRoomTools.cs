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
        public ComponentButton newRoomBtn;
        public ComponentButton roomTypeBtn;
        public ComponentButton reloadRoomBtn;



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
            newRoomBtn = new ComponentButton("new room", new Point(0, 0));
            roomTypeBtn = new ComponentButton("-------", new Point(0, 0));
            reloadRoomBtn = new ComponentButton("reload", new Point(0, 0));

            buttons.Add(saveBtn);
            buttons.Add(loadBtn);
            buttons.Add(newRoomBtn);
            buttons.Add(roomTypeBtn);
            buttons.Add(reloadRoomBtn);

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
            
            //Move Buttons
            saveBtn.rec.X = X + 16 * 1 - 8;
            saveBtn.rec.Y = Y + 16 * 1 + 8 - 2;
            Functions_Component.CenterText(saveBtn);

            reloadRoomBtn.rec.X = X + 16 * 1 + 17;
            reloadRoomBtn.rec.Y = Y + 16 * 1 + 8 - 2;
            Functions_Component.CenterText(reloadRoomBtn);

            loadBtn.rec.X = X + 16 * 4 + 10 - 8;
            loadBtn.rec.Y = Y + 16 * 1 + 8 - 2;
            Functions_Component.CenterText(loadBtn);

            newRoomBtn.rec.X = X + 16 * 1 - 8;
            newRoomBtn.rec.Y = Y + 16 * 2 - 0 + 2;
            Functions_Component.CenterText(newRoomBtn);


            #region RoomType Button

            roomTypeBtn.rec.X = X + 16 * 4 - 8 - 1;
            roomTypeBtn.rec.Y = Y + 16 * 2 - 0 + 2;

            //determine what default room to display
            if (state == WidgetRoomToolsState.Room)
            { roomID = RoomID.DEV_Room; }
            else
            { roomID = RoomID.DEV_Field; }

            //set the room types button correctly
            roomTypeBtn.compText.text = "" + roomID;

            //center text to button, prevent half pixel offsets
            roomTypeBtn.compText.position.X =
                (int)(roomTypeBtn.rec.Location.X + roomTypeBtn.rec.Width / 2) 
                - (roomTypeBtn.textWidth / 2);
            //center text vertically
            roomTypeBtn.compText.position.Y = roomTypeBtn.rec.Location.Y - 3;

            #endregion

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
                            
                            if (buttons[i] == saveBtn)
                            {
                                SaveCurrentRoom();
                            }
                            else if (buttons[i] == loadBtn)
                            {
                                Functions_Backend.SelectRoomFile();
                            }
                            else if (buttons[i] == newRoomBtn)
                            {   //create a new room based on the type buttons state
                                roomData = new RoomXmlData();
                                roomData.type = roomID;
                                LoadRoomData(roomData);
                            }
                            else if (buttons[i] == roomTypeBtn)
                            {
                                if (state == WidgetRoomToolsState.Room)
                                {
                                    //iterate thru a limited set of dungeon roomTypes
                                    if (roomID == RoomID.Column) { roomID = RoomID.Row; }
                                    else if (roomID == RoomID.Row) { roomID = RoomID.Square; }
                                    else if (roomID == RoomID.Square) { roomID = RoomID.Hub; }
                                    else if (roomID == RoomID.Hub) { roomID = RoomID.Boss; }
                                    else if (roomID == RoomID.Boss) { roomID = RoomID.Key; }
                                    else if (roomID == RoomID.Key) { roomID = RoomID.Column; }
                                }
                                else
                                {
                                    //editors only edit fields in overworld mode
                                    roomID = RoomID.DEV_Field;
                                }

                                //set the text in button to roomType enum
                                roomTypeBtn.compText.text = "" + roomID;
                            }
                            else if (buttons[i] == reloadRoomBtn)
                            {   //assumes roomData hasn't changed since last build
                                LoadRoomData(roomData);
                            } 
                            
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





        public void SaveCurrentRoom()
        {   //create RoomXmlData instance
            roomData = new RoomXmlData();
            roomData.type = Functions_Level.currentRoom.roomID; //save the room type/id
            //populate roomData with roomObjs
            for (Pool.roomObjCounter = 0; Pool.roomObjCounter < Pool.roomObjCount; Pool.roomObjCounter++)
            { SaveObject(Pool.roomObjPool[Pool.roomObjCounter], roomData); }
            //send the new roomdata to be saved
            Functions_Backend.SaveRoomData(roomData);
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









        

        public void LoadRoomData(RoomXmlData RoomXmlData)
        {
            Functions_Level.ResetLevel();
            Functions_Level.SetFloorTexture(LevelID.Castle_Dungeon);
            Functions_Pool.Reset();
            

            #region Setup Level or Room based on EditorState

            if (state == WidgetRoomToolsState.Level)
            {
                Level.ID = LevelID.DEV_Field;
                Functions_Level.currentRoom = new Room(Functions_Level.buildPosition, RoomID.DEV_Field);
                Level.map = false; //there is no level map
            }
            else
            {
                Level.ID = LevelID.DEV_Room;
                //build the room, if room data exists
                if (RoomXmlData != null)
                { Functions_Level.currentRoom = new Room(Functions_Level.buildPosition, RoomXmlData.type); }
                else //if room data doesn't exist, create a default row room
                { Functions_Level.currentRoom = new Room(Functions_Level.buildPosition, RoomID.DEV_Room); }
                //set dungeon map
                if (Flags.MapCheat) { Level.map = true; } else { Level.map = false; }
            }

            #endregion


            //setup the current room
            Level.rooms.Add(Functions_Level.currentRoom);
            Functions_Level.currentRoom.visited = true;

            
            #region Add Example Doors to Dungeon Rooms

            if (state == WidgetRoomToolsState.Room)
            {
                //simplify / collect room values
                int posX = Functions_Level.currentRoom.rec.X;
                int posY = Functions_Level.currentRoom.rec.Y;
                int middleX = (Functions_Level.currentRoom.size.X / 2) * 16;
                int middleY = (Functions_Level.currentRoom.size.Y / 2) * 16;
                int width = Functions_Level.currentRoom.size.X * 16;
                int height = Functions_Level.currentRoom.size.Y * 16;

                //set NSEW doors positions
                Level.doors.Add(new Door(new Point(posX + middleX, posY - 16))); //top
                Level.doors.Add(new Door(new Point(posX + middleX, posY + height))); //bottom
                Level.doors.Add(new Door(new Point(posX - 16, posY + middleY))); //left
                Level.doors.Add(new Door(new Point(posX + width, posY + middleY))); //right
            }

            #endregion


            Functions_Room.BuildRoom(Functions_Level.currentRoom, RoomXmlData);
            
            if (state == WidgetRoomToolsState.Room)
            {   //set spawnPos outside TopLeft of newly built room
                Functions_Level.currentRoom.spawnPos.X = Functions_Level.currentRoom.rec.X - 32;
                Functions_Level.currentRoom.spawnPos.Y = Functions_Level.currentRoom.rec.Y;
            }

            Functions_Pool.Update(); //update roomObjs once
            Functions_Hero.SpawnInCurrentRoom(); //spawn hero in room
            Pool.hero.direction = Direction.Down; //face hero down
            Flags.Paused = true;
        }

    }
}