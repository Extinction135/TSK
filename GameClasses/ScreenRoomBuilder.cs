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
    public class ScreenRoomBuilder : ScreenDungeon
    {
        int i;
        public WidgetRoomBuilder RoomBuilder;
        public EditorState editorState;

        public ComponentSprite cursorSprite;
        public ComponentSprite addDeleteSprite;
        public Point worldPos;
        public GameObject grabbedObj;
        public RoomXmlData roomData;
        GameObject objRef;
        Actor actorRef;


        public ScreenRoomBuilder() { this.name = "RoomBuilder Screen"; }

        public override void LoadContent()
        {
            RoomBuilder = new WidgetRoomBuilder();
            RoomBuilder.Reset(16 * 33, 16 * 2);
            Functions_Dungeon.Initialize(this);

            //place hero onscreen & build default empty room
            Functions_Movement.Teleport(Pool.hero.compMove, 200, 150);
            BuildRoomData();

            //create the cursor sprite
            cursorSprite = new ComponentSprite(Assets.mainSheet,
                new Vector2(0, 0), new Byte4(14, 13, 0, 0), new Point(16, 16));
            addDeleteSprite = new ComponentSprite(Assets.mainSheet,
                new Vector2(0, 0), new Byte4(15, 15, 0, 0), new Point(16, 16));
            //initialize the RB widget
            RoomBuilder.SetActiveObj(0); //set active obj to first widget obj
            RoomBuilder.SetActiveTool(RoomBuilder.moveObj); //set widet to move tool
            editorState = EditorState.MoveObj; //set screen to move state
            grabbedObj = null;
            //setup the screen
            overlay.alpha = 0.0f;
            displayState = DisplayState.Opened; //open the screen

            //editor specific flags
            Flags.Debug = true; //necessary for editor operation
            Flags.Invincibility = true; //hero cannot die in editor
            Flags.InfiniteMagic = true; //hero has infinite magic
            Flags.InfiniteGold = true; //hero has infinite gold
            Flags.InfiniteArrows = true; //hero has infinite arrows
            Flags.InfiniteBombs = true; //hero has infinite bombs
            Flags.CameraTracksHero = false; //center camera to dev room

            //set testing saveData
            PlayerData.current.magicFireball = true;
            PlayerData.current.weaponBow = true;
        }

        public override void HandleInput(GameTime GameTime)
        {
            base.HandleInput(GameTime);
            //Functions_Debug.HandleDebugMenuInput();
            //convert cursor Pos to world pos
            worldPos = Functions_Camera2D.ConvertScreenToWorld(Input.cursorPos.X, Input.cursorPos.Y);


            # region Set Mouse Cursor Sprite

            cursorSprite.currentFrame.Y = 14; ; //default to pointer
            if (editorState == EditorState.MoveObj) //check/set move state
            { cursorSprite.currentFrame.Y = 13; }
            if (Functions_Input.IsMouseButtonDown(MouseButtons.LeftButton))
            { cursorSprite.currentFrame.X = 15; } //set clicked frame
            else { cursorSprite.currentFrame.X = 14; }

            #endregion


            #region Match position of cursor sprite to cursor

            cursorSprite.position.X = Input.cursorPos.X;
            cursorSprite.position.Y = Input.cursorPos.Y;
            if (editorState != EditorState.MoveObj)
            {   //apply offset for pointer sprite
                cursorSprite.position.X += 3;
                cursorSprite.position.Y += 6;
            }

            addDeleteSprite.position.X = Input.cursorPos.X + 12;
            addDeleteSprite.position.Y = Input.cursorPos.Y - 0;
            
            #endregion


            if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
            {   //if mouse is contained within RB widget
                if (RoomBuilder.window.interior.rec.Contains(Input.cursorPos))
                {

                    #region Handle Obj / Tool Selection

                    //does any obj on the widget's objList contain the mouse position?
                    for (i = 0; i < RoomBuilder.total; i++)
                    {   //if there is a collision, set the active object to the object clicked on
                        if (RoomBuilder.objList[i].compCollision.rec.Contains(Input.cursorPos))
                        {
                            //handle collision with room obj
                            if (i < 40) { RoomBuilder.SetActiveObj(i); }
                            //handle collision with tool obj
                            else if (RoomBuilder.objList[i] == RoomBuilder.moveObj)
                            {
                                RoomBuilder.SetActiveTool(RoomBuilder.moveObj);
                                editorState = EditorState.MoveObj;
                            }
                            else if (RoomBuilder.objList[i] == RoomBuilder.addObj)
                            {
                                RoomBuilder.SetActiveTool(RoomBuilder.addObj);
                                editorState = EditorState.AddObj;
                                addDeleteSprite.currentFrame.X = 14;
                            }
                            else if (RoomBuilder.objList[i] == RoomBuilder.deleteObj)
                            {
                                RoomBuilder.SetActiveTool(RoomBuilder.deleteObj);
                                editorState = EditorState.DeleteObj;
                                addDeleteSprite.currentFrame.X = 15;
                            }
                        }
                    }

                    #endregion

                    
                    //Handle Button Selection
                    for (i = 0; i < RoomBuilder.buttons.Count; i++)
                    {   //check to see if the user has clicked on a button
                        if (RoomBuilder.buttons[i].rec.Contains(Input.cursorPos))
                        {

                            #region Save Button

                            if (RoomBuilder.buttons[i] == RoomBuilder.saveBtn)
                            {
                                //create RoomXmlData instance
                                roomData = new RoomXmlData();
                                //populate this instance with the room's objs
                                for (Pool.counter = 0; Pool.counter < Pool.roomObjCount; Pool.counter++)
                                {
                                    objRef = Pool.roomObjPool[Pool.counter];
                                    //if this object is active, save it
                                    if (objRef.active)
                                    {   
                                        if(objRef.canBeSaved)
                                        {   //make this obj relative to room top left corner
                                            ObjXmlData objData = new ObjXmlData();
                                            objData.type = objRef.type;
                                            objData.posX = objRef.compSprite.position.X - Functions_Dungeon.currentRoom.collision.rec.X;
                                            objData.posY = objRef.compSprite.position.Y - Functions_Dungeon.currentRoom.collision.rec.Y;
                                            roomData.objs.Add(objData);
                                        }
                                    }
                                }
                                Functions_Backend.SaveRoomData(roomData);
                            }

                            #endregion


                            #region Play Button

                            else if (RoomBuilder.buttons[i] == RoomBuilder.playBtn)
                            {
                                if (Flags.Paused) { Flags.Paused = false; }
                                else { Flags.Paused = true; }
                            }

                            #endregion


                            //Load Button
                            else if (RoomBuilder.buttons[i] == RoomBuilder.loadBtn)
                            { Functions_Backend.SelectRoomFile(this); }
                            
                            //New Room Button
                            else if (RoomBuilder.buttons[i] == RoomBuilder.newRoomBtn)
                            {   //create a new room based on the type buttons state
                                roomData = new RoomXmlData();
                                roomData.type = RoomBuilder.roomType;
                                BuildRoomData();
                            }


                            #region Room Type Button

                            else if (RoomBuilder.buttons[i] == RoomBuilder.roomTypeBtn)
                            {   //iterate thru a limited set of roomTypes
                                if (RoomBuilder.roomType == RoomType.Column)
                                { RoomBuilder.roomType = RoomType.Row; }

                                else if (RoomBuilder.roomType == RoomType.Row)
                                { RoomBuilder.roomType = RoomType.Square; }

                                else if (RoomBuilder.roomType == RoomType.Square)
                                { RoomBuilder.roomType = RoomType.Hub; }

                                else if (RoomBuilder.roomType == RoomType.Hub)
                                { RoomBuilder.roomType = RoomType.Boss; }

                                else if (RoomBuilder.roomType == RoomType.Boss)
                                { RoomBuilder.roomType = RoomType.Key; }

                                else if (RoomBuilder.roomType == RoomType.Key)
                                { RoomBuilder.roomType = RoomType.Column; }

                                //set the text in button to roomType enum
                                RoomBuilder.roomTypeBtn.compText.text = "" + RoomBuilder.roomType;
                            }

                            #endregion

                            else if(RoomBuilder.buttons[i] == RoomBuilder.reloadRoomBtn)
                            {
                                //assumes roomData hasn't changed since last build
                                BuildRoomData();
                            }
                        }
                    }
                }
                //if mouse worldPos is contained in room, allow add/delete selected object
                else if (Functions_Dungeon.currentRoom.collision.rec.Contains(worldPos))
                {

                    #region Handle Add Object State

                    if (editorState == EditorState.AddObj)
                    {   //place currently selected obj in room, aligned to 16px grid
                        GameObject objRef = Functions_Pool.GetRoomObj();

                        objRef.compMove.newPosition = AlignToGrid(worldPos.X, worldPos.Y);
                        Functions_Movement.Teleport(objRef.compMove,
                            objRef.compMove.newPosition.X, objRef.compMove.newPosition.Y);

                        Functions_GameObject.SetType(objRef, RoomBuilder.activeObj.type);
                        Functions_Component.Align(objRef.compMove, objRef.compSprite, objRef.compCollision);
                        Functions_Animation.Animate(objRef.compAnim, objRef.compSprite);
                    }

                    #endregion


                    #region Handle Delete Object State

                    else if (editorState == EditorState.DeleteObj)
                    {   //check collisions between worldPos and roomObjs, release any colliding obj
                        for (Pool.counter = 0; Pool.counter < Pool.roomObjCount; Pool.counter++)
                        {
                            if (Pool.roomObjPool[Pool.counter].active)
                            {
                                if (Pool.roomObjPool[Pool.counter].compCollision.rec.Contains(worldPos))
                                { Functions_Pool.Release(Pool.roomObjPool[Pool.counter]); }
                            }
                        }
                    }

                    #endregion

                }

                //objects CAN be moved outside of room

                #region Handle Grab (Move) Object State

                if (editorState == EditorState.MoveObj)
                {   //check collisions between worldPos and roomObjs, grab any colliding obj
                    for (Pool.counter = 0; Pool.counter < Pool.roomObjCount; Pool.counter++)
                    {
                        if (Pool.roomObjPool[Pool.counter].active)
                        {
                            if (Pool.roomObjPool[Pool.counter].compCollision.rec.Contains(worldPos))
                            { grabbedObj = Pool.roomObjPool[Pool.counter]; }
                        }
                    }
                }

                #endregion

            }


            #region Handle Dragging of Grabbed Obj

            if (Functions_Input.IsMouseButtonDown(MouseButtons.LeftButton))
            {   //if we have a grabbedObj, match it to cursorPos if LMB is down
                if (editorState == EditorState.MoveObj)
                {   //match grabbed Obj pos to worldPos, aligned to 16px grid
                    if (grabbedObj != null)
                    {
                        grabbedObj.compMove.newPosition = AlignToGrid(worldPos.X, worldPos.Y);
                        Functions_Movement.Teleport(grabbedObj.compMove,
                            grabbedObj.compMove.newPosition.X, grabbedObj.compMove.newPosition.Y);
                        Functions_Component.Align(grabbedObj.compMove,
                            grabbedObj.compSprite, grabbedObj.compCollision);
                    }
                }
            }

            #endregion


            #region Handle Release Grabbed Obj

            if (Functions_Input.IsNewMouseButtonRelease(MouseButtons.LeftButton))
            { grabbedObj = null; }

            #endregion


            #region Handle Button Over/Up States

            for (i = 0; i < RoomBuilder.buttons.Count; i++)
            {   //by default, set buttons to up color
                RoomBuilder.buttons[i].currentColor = Assets.colorScheme.buttonUp;
                //if user hovers over a button, set button to down color
                if (RoomBuilder.buttons[i].rec.Contains(Input.cursorPos))
                { RoomBuilder.buttons[i].currentColor = Assets.colorScheme.buttonDown; }
            }

            #endregion

        }

        public override void Update(GameTime GameTime)
        {
            base.Update(GameTime);
            RoomBuilder.Update();
            //set the update room button color
            if (Flags.Paused)
            { RoomBuilder.playBtn.currentColor = Assets.colorScheme.buttonUp; }
            else { RoomBuilder.playBtn.currentColor = Assets.colorScheme.buttonDown; }
        }

        public override void Draw(GameTime GameTime)
        {
            base.Draw(GameTime);
            //draw roomBuilder, cursor sprites
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            RoomBuilder.Draw();
            Functions_Draw.Draw(cursorSprite);
            if (editorState != EditorState.MoveObj) { Functions_Draw.Draw(addDeleteSprite); }
            ScreenManager.spriteBatch.End();
        }



        public Vector2 AlignToGrid(int X, int Y)
        {
            return new Vector2(16 * (X / 16) + 8, 16 * (Y / 16) + 8);
        }
        
        public void BuildRoomData()
        {
            Functions_Dungeon.dungeon = new Dungeon();
            Functions_Pool.SetDungeonTexture(Assets.cursedCastleSheet);

            //build the room
            if (roomData != null)
            { Functions_Dungeon.currentRoom = new Room(new Point(16 * 5, 16 * 5), roomData.type, 0); }
            else
            { Functions_Dungeon.currentRoom = new Room(new Point(16 * 5, 16 * 5), RoomType.Row, 0); }

            //simplify / collect room values
            int posX = Functions_Dungeon.currentRoom.collision.rec.X;
            int posY = Functions_Dungeon.currentRoom.collision.rec.Y;
            int middleX = (Functions_Dungeon.currentRoom.size.X / 2) * 16;
            int middleY = (Functions_Dungeon.currentRoom.size.Y / 2) * 16;
            int width = Functions_Dungeon.currentRoom.size.X * 16;
            int height = Functions_Dungeon.currentRoom.size.Y * 16;
            //set NSEW door locations
            Functions_Dungeon.dungeon.doorLocations.Add(new Point(posX + middleX, posY - 16)); //Top Door
            Functions_Dungeon.dungeon.doorLocations.Add(new Point(posX + middleX, posY + height)); //Bottom Door
            Functions_Dungeon.dungeon.doorLocations.Add(new Point(posX - 16, posY + middleY)); //Left Door
            Functions_Dungeon.dungeon.doorLocations.Add(new Point(posX + width, posY + middleY)); //Right Door
            //releases all roomObjs, builds walls + floors + doors
            Functions_Room.BuildRoom(Functions_Dungeon.currentRoom);
            
            //create the room objs
            if (roomData != null && roomData.objs.Count > 0)
            {   
                for (i = 0; i < roomData.objs.Count; i++)
                {
                    objRef = Functions_Pool.GetRoomObj();
                    Functions_Movement.Teleport(objRef.compMove,
                        Functions_Dungeon.currentRoom.collision.rec.X + roomData.objs[i].posX,
                        Functions_Dungeon.currentRoom.collision.rec.Y + roomData.objs[i].posY);
                    objRef.direction = Direction.Down; //we'll need to save this later
                    Functions_GameObject.SetType(objRef, roomData.objs[i].type); //get type

                    if (objRef.group == ObjGroup.EnemySpawn)
                    {
                        //create blob enemy here
                        actorRef = Functions_Pool.GetActor();
                        if(actorRef != null)
                        {
                            Functions_Actor.SetType(actorRef, ActorType.Blob);
                            Functions_Movement.Teleport(actorRef.compMove,
                                objRef.compSprite.position.X,
                                objRef.compSprite.position.Y);
                        }

                    }
                }
            }

            Functions_Pool.Update(); //update roomObjs once
            Flags.Paused = true; //initially freeze the loaded room
        }

    }
}