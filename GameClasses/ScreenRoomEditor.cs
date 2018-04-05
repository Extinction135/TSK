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

        public ScreenRoomEditor()
        {
            this.name = "Room Editor Screen";

            //unlock most/all items
            PlayerData.current = new SaveData();
            PlayerData.current.heartsTotal = 9;
            Pool.hero.health = 3;
            PlayerData.current.bombsCurrent = 99;
            PlayerData.current.arrowsCurrent = 99;
            //set items
            PlayerData.current.bottleA = MenuItemType.BottleHealth;
            PlayerData.current.bottleB = MenuItemType.BottleMagic;
            PlayerData.current.bottleC = MenuItemType.BottleFairy;
            PlayerData.current.magicFireball = true;
            //set weapons
            PlayerData.current.weaponBow = true;
            PlayerData.current.weaponNet = true;
            //set armor
            PlayerData.current.armorCape = true;
            //set equipment
            PlayerData.current.equipmentRing = true;
        }

        public override void LoadContent()
        {
            //place the object tools + room tools widgets
            Widgets.ObjectTools.Reset(16 * 1, 16 * 17);
            Widgets.RoomTools.Reset(16 * 33, 16 * 16);
            Widgets.WidgetObjects_Dungeon.Reset(16 * 1, 16 * 2);

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
            Flags.Paused = false; //unpause editor initially
        }

        public override void HandleInput(GameTime GameTime)
        {
            base.HandleInput(GameTime);

            if (!Flags.HideEditorWidgets)
            {
                Widgets.ObjectTools.HandleInput();
                Widgets.RoomTools.HandleInput();
            }
        }

        public override void Update(GameTime GameTime)
        {
            base.Update(GameTime);

            if (!Flags.HideEditorWidgets)
            {
                Widgets.ObjectTools.Update();
                Widgets.WidgetObjects_Dungeon.Update();
                Widgets.RoomTools.Update();
            }
        }

        public override void Draw(GameTime GameTime)
        {
            base.Draw(GameTime);
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            if (!Flags.HideEditorWidgets)
            {
                Widgets.RoomTools.Draw();
                Widgets.WidgetObjects_Dungeon.Draw();
                Widgets.ObjectTools.Draw(); //drawn last cause cursor
            }
            ScreenManager.spriteBatch.End();
        }
    }
}