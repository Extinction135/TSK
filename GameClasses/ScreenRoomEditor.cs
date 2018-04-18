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
    public class ScreenRoomEditor : ScreenLevel
    {
        public ScreenRoomEditor() { this.name = "Room Editor Screen"; }

        public override void LoadContent()
        {
            Level.ID = LevelID.DEV_Room;
            base.LoadContent();
            //setup room editor state
            Functions_TopMenu.DisplayWidgets(WidgetDisplaySet.Dungeon);
            Widgets.RoomTools.SetState(WidgetRoomToolsState.Room);
            
            //teleport hero to outside of room at top left corner
            Functions_Movement.Teleport(Pool.hero.compMove,
                Functions_Level.buildPosition.X - 32,
                Functions_Level.buildPosition.Y + 32);

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