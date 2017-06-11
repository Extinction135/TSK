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
    public class ScreenRoomBuilder : Screen
    {

        int i;
        public Room room;


        public ScreenRoomBuilder() { this.name = "RoomBuilder Screen"; }

        public override void LoadContent()
        {
            Widgets.RoomBuilder.Reset(0, 0);
            room = new Room(new Point(16 * 5, 16 * 5), RoomType.Dev, 0);

            //clear any previous dungeon data
            Functions_Dungeon.dungeon = new Dungeon();
            //set the objPool texture & build the room instance
            Functions_Pool.SetDungeonTexture(Assets.cursedCastleSheet);
            Functions_Room.BuildRoom(room);
            Functions_Dungeon.currentRoom = room;
            //update the pool once
            Functions_Pool.Update();

            displayState = DisplayState.Opened;
        }

        public override void HandleInput(GameTime GameTime)
        {
            Functions_Debug.HandleDebugMenuInput();


            #region Check Widget Objects for User Interaction

            if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
            {   //on left button click - does builder widget contain the mouse cursor's position?
                if (Widgets.RoomBuilder.window.interior.rec.Contains(Input.cursorPos))
                {   //does any obj on the widget's objList contain the mouse position?
                    for (i = 0; i < Widgets.RoomBuilder.total; i++)
                    {   //if there is a collision, set the active object to the object clicked on
                        if (Widgets.RoomBuilder.objList[i].compCollision.rec.Contains(Input.cursorPos))
                        {
                            if (i < 40) //handle collision with room obj
                            { Widgets.RoomBuilder.SetActiveObj(i); }
                            else //handle collision with toolbar obj
                            { Widgets.RoomBuilder.SetActiveTool(i); }
                        }
                    }
                }
            }

            #endregion


            #region Check Widget Buttons for User Interaction

            for (i = 0; i < 3; i++)
            {
                if (Widgets.RoomBuilder.buttons[i].rec.Contains(Input.cursorPos))
                {   //any button containing the cursor draws with 'over' color
                    Widgets.RoomBuilder.buttons[i].currentColor = Assets.colorScheme.buttonOver;
                    if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
                    {   //any button clicked on becomes selected
                        Widgets.RoomBuilder.buttons[i].currentColor = Assets.colorScheme.buttonDown;

                        if (i == 0) //save btn
                        { Functions_SaveLoad.Save(room); }
                        else if (i == 1) //new btn
                        { }
                        else if (i == 2) //load btn
                        { }
                    }
                } //buttons not touching cursor return to button up color
                else { Widgets.RoomBuilder.buttons[i].currentColor = Assets.colorScheme.buttonUp; }
            }

            #endregion


        }

        public override void Update(GameTime GameTime)
        {
            Timing.Reset();

            Widgets.RoomBuilder.Update();
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
            Widgets.RoomBuilder.Draw();
            Functions_Draw.DrawDebugMenu();
            Functions_Draw.DrawDebugInfo();
            ScreenManager.spriteBatch.End();

            Timing.stopWatch.Stop();
            Timing.drawTime = Timing.stopWatch.Elapsed;
            Timing.totalTime = Timing.updateTime + Timing.drawTime;
        }

    }
}