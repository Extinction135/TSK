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
    public class ScreenLevelEditor : ScreenLevel
    {

        public ScreenLevelEditor() { this.name = "Level Editor Screen"; }

        public override void LoadContent()
        {
            //register this level screen with Functions_Level
            Functions_Level.levelScreen = this;

            //build default empty row room
            Widgets.RoomTools.roomData = new RoomXmlData();
            Widgets.RoomTools.roomData.type = RoomType.Row;
            Widgets.RoomTools.BuildRoomData(Widgets.RoomTools.roomData);

            //place hero outside of room at top left corner
            Functions_Movement.Teleport(Pool.hero.compMove,
                Functions_Level.buildPosition.X - 32,
                Functions_Level.buildPosition.Y + 32);

            //setup the screen
            overlay.alpha = 0.0f;
            displayState = DisplayState.Opened; //open the screen
            Functions_Editor.SetEditorFlags();
            Functions_Editor.SetEditorLoadout();
            Flags.Paused = false; //unpause editor initially
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
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            ScreenManager.spriteBatch.End();
        }
    }
}