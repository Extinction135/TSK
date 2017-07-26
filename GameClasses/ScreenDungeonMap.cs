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
    public class ScreenDungeonMap : Screen
    {
        ScreenRec background = new ScreenRec();
        public static MenuWindow window;
        public Rectangle dungeonBkg;
        public List<Rectangle> rooms;
        public List<Rectangle> doors;
        public int i;



        public ScreenDungeonMap() { this.name = "DungeonMapScreen"; }

        public override void LoadContent()
        {
            background.alpha = 0.0f;
            background.maxAlpha = 0.5f;
            
            window = new MenuWindow(
                new Point( //place map underneath the worldUI hud
                    (int)WorldUI.currentWeapon.compSprite.position.X,
                    (int)WorldUI.currentWeapon.compSprite.position.Y + 22),
                new Point(16 * 8, 16 * 9 + 8), "Dungeon Map");
            dungeonBkg = new Rectangle(
                window.background.position.X + 8,
                window.background.position.Y + 20,
                16 * 7, 16 * 7);

            //place every room from dungeon.rooms list into rooms list
            rooms = new List<Rectangle>();
            for (i = 0; i < Functions_Dungeon.dungeon.rooms.Count; i++)
            {
                Rectangle dungeonRoom = new Rectangle(0, 0, 0, 0);
                //get the room size
                dungeonRoom.Width = Functions_Dungeon.dungeon.rooms[i].size.X;
                dungeonRoom.Height = Functions_Dungeon.dungeon.rooms[i].size.Y;
                //get the room position
                dungeonRoom.X = Functions_Dungeon.dungeon.rooms[i].rec.X;
                dungeonRoom.Y = Functions_Dungeon.dungeon.rooms[i].rec.Y;
                //subtract the build position
                dungeonRoom.X -= Functions_Dungeon.buildPosition.X;
                dungeonRoom.Y -= Functions_Dungeon.buildPosition.Y;
                //reduce position 16:1
                dungeonRoom.X = dungeonRoom.X / 16;
                dungeonRoom.Y = dungeonRoom.Y / 16;
                //add the map offset
                dungeonRoom.X += dungeonBkg.X + (dungeonBkg.Width / 2) - 8;
                dungeonRoom.Y += dungeonBkg.Y + (dungeonBkg.Height / 2) + 32;
                //add to the map rooms
                if (Functions_Dungeon.dungeon.rooms[i].visited)
                { rooms.Add(dungeonRoom); }
            }

            //display all the dungeon doors
            doors = new List<Rectangle>();
            for (i = 0; i < Functions_Dungeon.dungeon.doors.Count; i++)
            {
                Rectangle dungeonDoor = new Rectangle(0, 0, 1, 1);
                //get the door position
                dungeonDoor.X = Functions_Dungeon.dungeon.doors[i].rec.X;
                dungeonDoor.Y = Functions_Dungeon.dungeon.doors[i].rec.Y;
                //subtract the build position
                dungeonDoor.X -= Functions_Dungeon.buildPosition.X;
                dungeonDoor.Y -= Functions_Dungeon.buildPosition.Y;
                //reduce position 16:1
                dungeonDoor.X = dungeonDoor.X / 16;
                dungeonDoor.Y = dungeonDoor.Y / 16;
                //add the map offset
                dungeonDoor.X += dungeonBkg.X + (dungeonBkg.Width / 2) - 8;
                dungeonDoor.Y += dungeonBkg.Y + (dungeonBkg.Height / 2) + 32;
                //add to the map doors
                if (Functions_Dungeon.dungeon.doors[i].visited)
                { doors.Add(dungeonDoor); }
            }

            //display hero as a small white square
            //place this square centered to the current room

            //open the screen
            displayState = DisplayState.Opening;
            Assets.Play(Assets.sfxMapOpen);
        }

        public override void HandleInput(GameTime GameTime)
        {
            if (displayState == DisplayState.Opened)
            {   //only allow input if the screen has opened completely
                if (Functions_Input.IsNewButtonPress(Buttons.Start) ||
                    Functions_Input.IsNewButtonPress(Buttons.Back) ||
                    Functions_Input.IsNewButtonPress(Buttons.B))
                {
                    Assets.Play(Assets.sfxInventoryClose); //play closing sfx
                    Functions_MenuWindow.Close(window);
                    displayState = DisplayState.Closing; //begin closing the screen
                }
            }
        }

        public override void Update(GameTime GameTime)
        {

            #region Handle Screen State

            if (displayState == DisplayState.Opening)
            {
                //fade background in
                background.fadeState = FadeState.FadeIn;
                Functions_ScreenRec.Fade(background);
                if (background.fadeState == FadeState.FadeComplete)
                { displayState = DisplayState.Opened; }
            }
            else if (displayState == DisplayState.Closing)
            {
                //fade background out
                background.fadeState = FadeState.FadeOut;
                Functions_ScreenRec.Fade(background);
                if (background.fadeState == FadeState.FadeComplete)
                { displayState = DisplayState.Closed; }
            }
            else if (displayState == DisplayState.Closed)
            { ScreenManager.RemoveScreen(this); }

            #endregion


            Functions_MenuWindow.Update(window);
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            Functions_Draw.Draw(background);
            Functions_Draw.Draw(window);
            if (window.interior.displayState == DisplayState.Opened)
            {   //draw dungeon background
                ScreenManager.spriteBatch.Draw(
                    Assets.dummyTexture, dungeonBkg,
                    Assets.colorScheme.windowInset);
                //draw room recs
                for (i = 0; i < rooms.Count; i++)
                {
                    ScreenManager.spriteBatch.Draw(
                        Assets.dummyTexture, rooms[i],
                        Assets.colorScheme.buttonDown);
                }
                //draw dungeon doors
                for (i = 0; i < doors.Count; i++)
                {
                    ScreenManager.spriteBatch.Draw(
                        Assets.dummyTexture, doors[i],
                        Assets.colorScheme.buttonDown);
                }
                //draw hero rec
            }
            ScreenManager.spriteBatch.End();
        }
    }
}
