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
    public class ScreenRoomBuilder : ScreenLevel
    {

        public ScreenRoomBuilder() { this.name = "RoomBuilder Screen"; }

        public override void LoadContent()
        {
            //place the object tools + room tools widgets
            Widgets.ObjectTools.Reset(16 * 33, 16 * 1);
            Widgets.RoomTools.Reset(16 * 33, 16 * 16);

            //register this level screen with Functions_Level
            Functions_Level.levelScreen = this;

            //build default empty row room (pass null)
            Widgets.RoomTools.BuildRoomData(null);
            //place hero outside of room at top left corner
            Functions_Movement.Teleport(Pool.hero.compMove,
                Functions_Level.buildPosition.X - 32,
                Functions_Level.buildPosition.Y + 32);

            //setup the screen
            overlay.alpha = 0.0f;
            displayState = DisplayState.Opened; //open the screen
            SetEditorFlags();
            SetEditorLoadout();
            Flags.Paused = false; //unpause editor initially
        }

        public override void HandleInput(GameTime GameTime)
        {
            base.HandleInput(GameTime);
            Widgets.ObjectTools.HandleInput();
            Widgets.RoomTools.HandleInput();
        }

        public override void Update(GameTime GameTime)
        {
            base.Update(GameTime);
            Widgets.ObjectTools.Update();
            Widgets.RoomTools.Update();
        }

        public override void Draw(GameTime GameTime)
        {
            base.Draw(GameTime);
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            Widgets.RoomTools.Draw();
            Widgets.ObjectTools.Draw();
            ScreenManager.spriteBatch.End();
        }



        public void SetEditorFlags()
        {   //editor specific flags
            Flags.EnableTopMenu = true; //necessary
            Flags.DrawDebugInfo = false; //initial display, can be changed
            Flags.Invincibility = true; //hero cannot die in editor
            Flags.InfiniteMagic = true; //hero has infinite magic
            Flags.InfiniteGold = true; //hero has infinite gold
            Flags.InfiniteArrows = true; //hero has infinite arrows
            Flags.InfiniteBombs = true; //hero has infinite bombs
            Flags.CameraTracksHero = false; //center to room
            Flags.ShowEnemySpawns = true; //necessary for editing
        }

        public void SetEditorLoadout()
        {   //unlock most/all items
            PlayerData.current = new SaveData();

            PlayerData.current.heartsTotal = 9;
            Pool.hero.health = 3;
            PlayerData.current.bombsCurrent = 99;
            PlayerData.current.arrowsCurrent = 99;

            //set items
            PlayerData.current.bottleA = 2;
            PlayerData.current.bottleB = 3;
            PlayerData.current.bottleC = 4;
            PlayerData.current.magicFireball = true;

            //set weapons
            PlayerData.current.weaponBow = true;
            PlayerData.current.weaponNet = true;

            //set armor
            PlayerData.current.armorChest = true;
            PlayerData.current.armorCape = true;
            PlayerData.current.armorRobe = true;

            //set equipment
            PlayerData.current.equipmentRing = true;
        }
    }
}