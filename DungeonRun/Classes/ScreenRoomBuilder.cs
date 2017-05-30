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



        public ScreenRoomBuilder() { this.name = "Room Builder Screen"; }

        public override void LoadContent()
        {
            //reset the room builder widget's window
            Widgets.RoomBuilder.window.background.Reset();
            Widgets.RoomBuilder.window.border.Reset();
            Widgets.RoomBuilder.window.inset.Reset();
            Widgets.RoomBuilder.window.interior.Reset();
            Widgets.RoomBuilder.window.headerLine.Reset();
            Widgets.RoomBuilder.window.footerLine.Reset();


        }

        public override void HandleInput(GameTime GameTime)
        {
            Functions_Debug.HandleDebugMenuInput();
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