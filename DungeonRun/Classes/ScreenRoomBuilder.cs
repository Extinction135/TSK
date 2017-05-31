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


        public ScreenRoomBuilder() { this.name = "Room Builder Screen"; }

        public override void LoadContent()
        {
            Widgets.RoomBuilder.Reset(0, 0);
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
                        { }
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
            Widgets.RoomBuilder.Update();
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            Widgets.RoomBuilder.Draw();
            Functions_Draw.DrawDebugMenu();

            if (Flags.DrawCollisions)
            {
                Functions_Draw.Draw(Input.cursorColl);
                //draw the room object's collision recs + interaction recs
            }

            ScreenManager.spriteBatch.End();
        }

    }
}