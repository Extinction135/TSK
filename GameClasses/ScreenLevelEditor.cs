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
            //set the widgets to display
            Functions_TopMenu.DisplayWidgets(WidgetDisplaySet.World);
            
            //register this level screen with Functions_Level
            Functions_Level.levelScreen = this;

            //clear level and pool data
            Functions_Level.ResetLevel();
            Functions_Pool.Reset();

            //create a field for the hero to run around in
            Functions_Level.currentRoom = new Room(Functions_Level.buildPosition, RoomType.Field);
            Functions_Room.SetType(Functions_Level.currentRoom, RoomType.Field);

            //set spawnPos to center of room
            Functions_Level.currentRoom.spawnPos.X = 
                (Functions_Level.currentRoom.rec.X + Functions_Level.currentRoom.rec.Width / 2);
            Functions_Level.currentRoom.spawnPos.Y = 
                (Functions_Level.currentRoom.rec.Y + Functions_Level.currentRoom.rec.Height / 2);
            Functions_Hero.SpawnInCurrentRoom(); //centered
            Pool.hero.health = 3; //give hero health
            Functions_Hero.UnlockAll(); //unlock all items

            //set to gray background
            Assets.colorScheme.background = new Color(150, 150, 150, 255);
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