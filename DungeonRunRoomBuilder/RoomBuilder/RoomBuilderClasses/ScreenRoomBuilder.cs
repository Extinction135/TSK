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


    //the various states the room builder screen can be in
    public enum EditorState { MoveObj, AddObj, DeleteObj }





    public class ScreenRoomBuilder : Screen
    {
        int i;
        public Room room;
        public WidgetRoomBuilder RoomBuilder;
        public EditorState editorState;


        public ComponentSprite cursorSprite;
        Byte4 handFrame = new Byte4(14, 13, 0, 0);




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

            cursorSprite = new ComponentSprite(Assets.mainSheet,
                new Vector2(0, 0), handFrame, new Point(16, 16));

            RoomBuilder.SetActiveObj(0); //set active obj to first widget obj
            RoomBuilder.SetActiveTool(RoomBuilder.moveObj); //set widet to move tool
            editorState = EditorState.MoveObj; //set screen to move state

            displayState = DisplayState.Opened; //open the screen
        }

        public override void HandleInput(GameTime GameTime)
        {
            Functions_Debug.HandleDebugMenuInput();

            cursorSprite.position.X = Input.cursorPos.X;
            cursorSprite.position.Y = Input.cursorPos.Y;


            #region Check Widget Objects for User Interaction

            if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
            {   //on left button click - does builder widget contain the mouse cursor's position?
                if (RoomBuilder.window.interior.rec.Contains(Input.cursorPos))
                {   //does any obj on the widget's objList contain the mouse position?
                    for (i = 0; i < RoomBuilder.total; i++)
                    {   //if there is a collision, set the active object to the object clicked on
                        if (RoomBuilder.objList[i].compCollision.rec.Contains(Input.cursorPos))
                        {

                            if (i < 40) //handle collision with room obj
                            { RoomBuilder.SetActiveObj(i); }

                            //handle collision with tool obj
                            else if (RoomBuilder.objList[i] == RoomBuilder.moveObj)
                            {
                                RoomBuilder.SetActiveTool(RoomBuilder.moveObj);
                                editorState = EditorState.MoveObj;
                                //
                            }

                            else if (RoomBuilder.objList[i] == RoomBuilder.addObj)
                            {
                                RoomBuilder.SetActiveTool(RoomBuilder.addObj);
                                editorState = EditorState.AddObj;
                                //
                            }

                            else if (RoomBuilder.objList[i] == RoomBuilder.deleteObj)
                            {
                                RoomBuilder.SetActiveTool(RoomBuilder.deleteObj);
                                editorState = EditorState.DeleteObj;
                                //
                            }

                        }
                    }
                }
            }

            #endregion


            #region Check Widget Buttons for User Interaction

            for (i = 0; i < 3; i++)
            {
                if (RoomBuilder.buttons[i].rec.Contains(Input.cursorPos))
                {   //any button containing the cursor draws with 'over' color
                    RoomBuilder.buttons[i].currentColor = Assets.colorScheme.buttonOver;
                    if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
                    {   //any button clicked on becomes selected
                        RoomBuilder.buttons[i].currentColor = Assets.colorScheme.buttonDown;

                        if (RoomBuilder.buttons[i] == RoomBuilder.saveBtn) //save btn
                        {
                            //
                            Debug.WriteLine("saving");
                        }
                        else if (RoomBuilder.buttons[i] == RoomBuilder.newBtn) //new btn
                        {
                            //
                            Debug.WriteLine("new room created");
                        }
                        else if (RoomBuilder.buttons[i] == RoomBuilder.loadBtn) //load btn
                        {
                            //
                            Debug.WriteLine("loading");
                        }
                    }
                } //buttons not touching cursor return to button up color
                else { RoomBuilder.buttons[i].currentColor = Assets.colorScheme.buttonUp; }
            }

            #endregion


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

    }
}