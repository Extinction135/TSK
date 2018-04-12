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
            //set the widgets to display
            Functions_TopMenu.DisplayWidgets(WidgetDisplaySet.World);
            
            //register this level screen with Functions_Level
            Functions_Level.levelScreen = this;

            //build default empty row room
            Widgets.RoomTools.roomData = new RoomXmlData();
            Widgets.RoomTools.roomData.type = RoomType.Row;
            Widgets.RoomTools.BuildRoomData(Widgets.RoomTools.roomData);
            
            //set spawnPos to outside of room at top left corner
            Functions_Level.currentRoom.spawnPos.X = Functions_Level.buildPosition.X - 32;
            Functions_Level.currentRoom.spawnPos.Y = Functions_Level.buildPosition.Y + 32;
            Functions_Hero.SpawnInCurrentRoom(); //centered
            Pool.hero.health = 3; //give hero health
            Functions_Hero.UnlockAll(); //unlock all items

            //set to black background
            Assets.colorScheme.background = new Color(0, 0, 0, 0);
            //setup the screen
            overlay.alpha = 0.0f;
            displayState = DisplayState.Opened; //open the screen
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
        }
    }
}