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
        public List<Room> rooms;
        public List<Door> doors;
        public Color mapColor;
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


            #region Collect all the dungeon rooms

            rooms = new List<Room>();
            for (i = 0; i < Functions_Dungeon.dungeon.rooms.Count; i++)
            {
                Room dungeonRoom = new Room(new Point(0, 0), Functions_Dungeon.dungeon.rooms[i].type);
                //get the room size
                dungeonRoom.rec.Width = Functions_Dungeon.dungeon.rooms[i].size.X;
                dungeonRoom.rec.Height = Functions_Dungeon.dungeon.rooms[i].size.Y;
                //get the room position
                dungeonRoom.rec.X = Functions_Dungeon.dungeon.rooms[i].rec.X;
                dungeonRoom.rec.Y = Functions_Dungeon.dungeon.rooms[i].rec.Y;
                //subtract the build position
                dungeonRoom.rec.X -= Functions_Dungeon.buildPosition.X;
                dungeonRoom.rec.Y -= Functions_Dungeon.buildPosition.Y;
                //reduce position 16:1
                dungeonRoom.rec.X = dungeonRoom.rec.X / 16;
                dungeonRoom.rec.Y = dungeonRoom.rec.Y / 16;
                //add the map offset
                dungeonRoom.rec.X += dungeonBkg.X + (dungeonBkg.Width / 2) - 8;
                dungeonRoom.rec.Y += dungeonBkg.Y + (dungeonBkg.Height / 2) + 32;
                //grab the visited boolean
                dungeonRoom.visited = Functions_Dungeon.dungeon.rooms[i].visited;
                rooms.Add(dungeonRoom);
            }

            #endregion


            #region Collect all the dungeon doors

            doors = new List<Door>();
            for (i = 0; i < Functions_Dungeon.dungeon.doors.Count; i++)
            {
                Door dungeonDoor = new Door(new Point());
                //map doors are always 1x1 pixels
                dungeonDoor.rec.Width = 1;
                dungeonDoor.rec.Height = 1;
                //get the door position
                dungeonDoor.rec.X = Functions_Dungeon.dungeon.doors[i].rec.X;
                dungeonDoor.rec.Y = Functions_Dungeon.dungeon.doors[i].rec.Y;
                //subtract the build position
                dungeonDoor.rec.X -= Functions_Dungeon.buildPosition.X;
                dungeonDoor.rec.Y -= Functions_Dungeon.buildPosition.Y;
                //reduce position 16:1
                dungeonDoor.rec.X = dungeonDoor.rec.X / 16;
                dungeonDoor.rec.Y = dungeonDoor.rec.Y / 16;
                //add the map offset
                dungeonDoor.rec.X += dungeonBkg.X + (dungeonBkg.Width / 2) - 8;
                dungeonDoor.rec.Y += dungeonBkg.Y + (dungeonBkg.Height / 2) + 32;
                //grab the visited boolean
                dungeonDoor.visited = Functions_Dungeon.dungeon.doors[i].visited;
                doors.Add(dungeonDoor);
            }

            #endregion


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
                    Assets.colorScheme.mapBkg);

                #region Draw dungeon rooms

                for (i = 0; i < rooms.Count; i++)
                {   //do we need to draw this room?
                    if (rooms[i].visited || Functions_Dungeon.dungeon.map)
                    {
                        if (rooms[i].visited)
                        {   //draw visited rooms with Visited color
                            ScreenManager.spriteBatch.Draw(
                                Assets.dummyTexture, rooms[i].rec, 
                                Assets.colorScheme.mapVisited);
                        }
                        else//hero has map
                        {   //draw unvisited rooms with NotVisited color
                            ScreenManager.spriteBatch.Draw(
                                Assets.dummyTexture, rooms[i].rec, 
                                Assets.colorScheme.mapNotVisited);
                        }
                    }
                }

                #endregion


                #region Draw dungeon doors

                for (i = 0; i < doors.Count; i++)
                {   //do we need to draw this door?
                    if (doors[i].visited || Functions_Dungeon.dungeon.map)
                    {
                        if (doors[i].visited)
                        {   //draw visited doors with Visited color
                            ScreenManager.spriteBatch.Draw(
                                Assets.dummyTexture, doors[i].rec,
                                Assets.colorScheme.mapVisited);
                        }
                        else//hero has map
                        {   //draw unvisited doors with NotVisited color
                            ScreenManager.spriteBatch.Draw(
                                Assets.dummyTexture, doors[i].rec,
                                Assets.colorScheme.mapNotVisited);
                        }
                    }
                }

                #endregion


                //draw hero rec
            }
            ScreenManager.spriteBatch.End();
        }
    }
}