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
    public class ScreenRoomBuilder : Screen
    {


        int i;


        public ScreenRoomBuilder() { this.name = "Room Builder Screen"; }

        public override void LoadContent()
        {
            Widgets.RoomBuilder.Reset(0, 0);
        }

        public override void HandleInput(GameTime GameTime)
        {
            Functions_Debug.HandleDebugMenuInput();

            if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
            {   //on left button click - does builder widget contain the mouse cursor's position?
                if (Widgets.RoomBuilder.window.interior.rec.Contains(Input.cursorPos))
                {   //does any obj on the widget's objList contain the mouse position?
                    for (i = 0; i < 5 * 7; i++)
                    {   //if there is a collision, set the active object to the object clicked on
                        if (Widgets.RoomBuilder.objList[i].compCollision.rec.Contains(Input.cursorPos))
                        {  Widgets.RoomBuilder.SetActiveObj(i); }
                    }

                    //does any tool icon contain the mouse position?
                    if(Widgets.RoomBuilder.moveIcon.drawRec.Contains(Input.cursorPos))
                    { Widgets.RoomBuilder.SetActiveTool(Widgets.RoomBuilder.moveIcon); }
                    else if (Widgets.RoomBuilder.addIcon.drawRec.Contains(Input.cursorPos))
                    { Widgets.RoomBuilder.SetActiveTool(Widgets.RoomBuilder.addIcon); }
                    else if (Widgets.RoomBuilder.minusIcon.drawRec.Contains(Input.cursorPos))
                    { Widgets.RoomBuilder.SetActiveTool(Widgets.RoomBuilder.minusIcon); }


                }
            }
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