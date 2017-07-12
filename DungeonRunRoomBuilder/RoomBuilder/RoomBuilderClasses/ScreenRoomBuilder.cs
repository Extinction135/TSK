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


    //the various states the room builder screen can be in
    public enum EditorState { MoveObj, AddObj, DeleteObj }



    


    public class ScreenRoomBuilder : Screen
    {
        int i;
        public Room room;
        public WidgetRoomBuilder RoomBuilder;
        public EditorState editorState;


        public ComponentSprite cursorSprite;
        public ComponentSprite addDeleteSprite;
        public Point worldPos;
        public GameObject grabbedObj;

        public Boolean updateRoom = false;


        public ScreenRoomBuilder() { this.name = "RoomBuilder Screen"; }

        public override void LoadContent()
        {
            RoomBuilder = new WidgetRoomBuilder();
            RoomBuilder.Reset(8, 16 * 3);
            room = new Room(new Point(16 * 5, 16 * 5), RoomType.Dev, 0);

            //clear any previous dungeon data
            Functions_Dungeon.dungeon = new Dungeon();
            //set the objPool texture & build the room instance
            Functions_Pool.SetDungeonTexture(Assets.cursedCastleSheet);
            Functions_Room.BuildRoom(room);
            Functions_Dungeon.currentRoom = room;
            //hide hero offscreen
            Functions_Movement.Teleport(Pool.hero.compMove, -100, -100);
            Functions_Pool.Update(); //update the pool once

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

            displayState = DisplayState.Opened; //open the screen
        }

        public override void HandleInput(GameTime GameTime)
        {
            Functions_Debug.HandleDebugMenuInput();
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

                    
                    #region Handle Button Selection

                    for (i = 0; i < RoomBuilder.buttons.Count; i++)
                    {   //check to see if the user has clicked on a button
                        if (RoomBuilder.buttons[i].rec.Contains(Input.cursorPos))
                        {
                            if (RoomBuilder.buttons[i] == RoomBuilder.saveBtn)
                            {
                                Debug.WriteLine("saving");
                            }
                            else if (RoomBuilder.buttons[i] == RoomBuilder.newBtn)
                            {
                                Debug.WriteLine("new room created");
                            }
                            else if (RoomBuilder.buttons[i] == RoomBuilder.loadBtn)
                            {
                                Debug.WriteLine("loading");
                            }
                            else if (RoomBuilder.buttons[i] == RoomBuilder.updateBtn)
                            {
                                if (updateRoom)
                                {
                                    updateRoom = false;
                                    RoomBuilder.updateBtn.currentColor = Assets.colorScheme.buttonUp;
                                }
                                else
                                {
                                    updateRoom = true;
                                    RoomBuilder.updateBtn.currentColor = Assets.colorScheme.buttonDown;
                                }
                            }
                        }
                    }

                    #endregion

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


            #region Handle Release Grabbed Obj

            if (Functions_Input.IsNewMouseButtonRelease(MouseButtons.LeftButton))
            { grabbedObj = null; }

            #endregion


            #region Handle Button Over/Up States

            for (i = 0; i < 3; i++)
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
            Timing.Reset();
            RoomBuilder.Update();

            if (updateRoom) { Functions_Pool.Update(); } //animate the roomObjs

            //track camera to left-center of room instance
            Camera2D.targetPosition.X = room.center.X - 16 * 3;
            Camera2D.targetPosition.Y = room.center.Y;
            Functions_Camera2D.Update(GameTime);
            Timing.stopWatch.Stop();
            Timing.updateTime = Timing.stopWatch.Elapsed;


            #region Handle Dragging of Grabbed Obj

            if(Functions_Input.IsMouseButtonDown(MouseButtons.LeftButton))
            {   //if we have a grabbedObj, match it to cursorPos if LMB is down
                if (editorState == EditorState.MoveObj)
                {   //match grabbed Obj pos to worldPos, aligned to 16px grid
                    if(grabbedObj != null)
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

        }

        public override void Draw(GameTime GameTime)
        {
            Timing.Reset();


            #region Draw gameworld from camera's view

            ScreenManager.spriteBatch.Begin(
                        SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend,
                        SamplerState.PointClamp,
                        null,
                        null,
                        null,
                        Camera2D.view
                        );
            Functions_Pool.Draw();
            if (Flags.DrawCollisions) { Functions_Draw.Draw(Input.cursorColl); }
            ScreenManager.spriteBatch.End();

            #endregion


            //Draw UI, debug info + debug menu
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            RoomBuilder.Draw();
            Functions_Draw.DrawDebugMenu();
            Functions_Draw.DrawDebugInfo();
            Functions_Draw.Draw(cursorSprite);
            if (editorState != EditorState.MoveObj) { Functions_Draw.Draw(addDeleteSprite); }
            ScreenManager.spriteBatch.End();

            Timing.stopWatch.Stop();
            Timing.drawTime = Timing.stopWatch.Elapsed;
            Timing.totalTime = Timing.updateTime + Timing.drawTime;
        }



        public Vector2 AlignToGrid(int X, int Y)
        {
            return new Vector2(16 * (X / 16) + 8, 16 * (Y / 16) + 8);
        }
        
    }
}