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
    public class ScreenEditor : ScreenLevel
    {
        public ScreenEditor() { this.name = "Editor Screen"; }

        public override void LoadContent()
        {
            //based on the boot routine, setup the editor differently
            if(Flags.bootRoutine == BootRoutine.Editor_Level)
            {
                Level.ID = LevelID.DEV_Field;
                Widgets.RoomTools.roomData = new RoomXmlData();
                Widgets.RoomTools.roomData.type = RoomID.DEV_Field;
                base.LoadContent();

                Widgets.RoomTools.SetState(WidgetRoomToolsState.Level);
                //fields auto-teleport hero to field spawn point
            }
            else
            {
                Level.ID = LevelID.DEV_Room;
                Widgets.RoomTools.roomData = new RoomXmlData();
                Widgets.RoomTools.roomData.type = RoomID.DEV_Row;
                base.LoadContent();

                Widgets.RoomTools.SetState(WidgetRoomToolsState.Room);
                //teleport hero to outside of room at top left corner
                Functions_Movement.Teleport(Pool.hero.compMove,
                    Functions_Level.buildPosition.X - 32,
                    Functions_Level.buildPosition.Y + 32);
            }

            //position the roomTools widget
            Widgets.RoomTools.Reset(16 * 33, 16 * 17 + 8); //bottom right

            //setup the screen
            overlay.alpha = 0.0f;
            displayState = DisplayState.Opened; //open the screen
            Flags.Paused = false; //unpause editor initially
            Functions_Hero.UnlockAll(); //unlock all items
        }

        public override void HandleInput(GameTime GameTime)
        {
            base.HandleInput(GameTime);
        }

        public override void Update(GameTime GameTime)
        {   
            base.Update(GameTime);
        }

        public override void Draw(GameTime GameTime)
        {
            base.Draw(GameTime);
        }
    }
}