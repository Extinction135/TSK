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



        public ScreenRoomBuilder() { this.name = "RoomBuilder Screen"; }

        public override void LoadContent()
        {
            RoomBuilder = new WidgetRoomBuilder();
            RoomBuilder.Reset(8, 16 * 4);
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

            //initialize the RB widget
            RoomBuilder.SetActiveObj(0); //set active obj to first widget obj
            RoomBuilder.SetActiveTool(RoomBuilder.moveObj); //set widet to move tool
            editorState = EditorState.MoveObj; //set screen to move state

            displayState = DisplayState.Opened; //open the screen
        }

        public override void HandleInput(GameTime GameTime)
        {
            Functions_Debug.HandleDebugMenuInput();


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

            #endregion



            

            
            if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
            {
                //if mouse is contained within RB widget
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
                            }
                            else if (RoomBuilder.objList[i] == RoomBuilder.deleteObj)
                            {
                                RoomBuilder.SetActiveTool(RoomBuilder.deleteObj);
                                editorState = EditorState.DeleteObj;
                            }
                        }
                    }

                    #endregion

                    
                    #region Handle Button Selection

                    for (i = 0; i < 3; i++)
                    {
                        if (RoomBuilder.buttons[i].rec.Contains(Input.cursorPos))
                        {   //buttons clicked on become button down color
                            RoomBuilder.buttons[i].currentColor = Assets.colorScheme.buttonDown;
                            if (RoomBuilder.buttons[i] == RoomBuilder.saveBtn) //save btn
                            {
                                Debug.WriteLine("saving");
                            }
                            else if (RoomBuilder.buttons[i] == RoomBuilder.newBtn) //new btn
                            {
                                Debug.WriteLine("new room created");
                            }
                            else if (RoomBuilder.buttons[i] == RoomBuilder.loadBtn) //load btn
                            {
                                Debug.WriteLine("loading");
                            }
                        } //buttons not clicked on return to button up color
                        else { RoomBuilder.buttons[i].currentColor = Assets.colorScheme.buttonUp; }
                    }

                    #endregion

                }
                //else check world interaction
                else
                {

                    if(editorState == EditorState.AddObj)
                    {
                        //convert cursor Pos to world pos
                        Point worldPos = Functions_Camera2D.ConvertScreenToWorld(Input.cursorPos.X, Input.cursorPos.Y);
                        Point objPos = AlignToGrid(worldPos.X, worldPos.Y);
                        GameObject objRef = Functions_Pool.GetRoomObj();
                        Functions_Movement.Teleport(objRef.compMove, objPos.X, objPos.Y);
                        Functions_GameObject.SetType(objRef, RoomBuilder.activeObj.type);
                        Functions_Component.Align(objRef.compMove, objRef.compSprite, objRef.compCollision);
                        Functions_Animation.Animate(objRef.compAnim, objRef.compSprite);
                    }
                    
                    else if(editorState == EditorState.DeleteObj)
                    {


                        //convert cursor to world pos
                        Point worldPos = Functions_Camera2D.ConvertScreenToWorld(Input.cursorPos.X, Input.cursorPos.Y);
                        //check collisions between worldPos and all roomObjs
                        //if collision happens, release obj

                        for (Pool.counter = 0; Pool.counter < Pool.roomObjCount; Pool.counter++)
                        {
                            if (Pool.roomObjPool[Pool.counter].active)
                            {
                                if (Pool.roomObjPool[Pool.counter].compCollision.rec.Contains(worldPos))
                                { Functions_Pool.Release(Pool.roomObjPool[Pool.counter]); }
                            }
                        }


                    }


                }
            }



        }

        public override void Update(GameTime GameTime)
        {
            Timing.Reset();
            RoomBuilder.Update();
            //track camera to left-center of room instance
            Camera2D.targetPosition.X = room.center.X - 16 * 3;
            Camera2D.targetPosition.Y = room.center.Y;
            Functions_Camera2D.Update(GameTime);
            Timing.stopWatch.Stop();
            Timing.updateTime = Timing.stopWatch.Elapsed;
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
            ScreenManager.spriteBatch.End();

            Timing.stopWatch.Stop();
            Timing.drawTime = Timing.stopWatch.Elapsed;
            Timing.totalTime = Timing.updateTime + Timing.drawTime;
        }



        public Point AlignToGrid(int X, int Y)
        {
            return new Point(16 * (X / 16) + 8, 16 * (Y / 16) + 8);
        }

    }
}