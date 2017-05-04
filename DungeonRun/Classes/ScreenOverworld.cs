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
    public class ScreenOverworld : Screen
    {

        public static MenuWindow window;
        public static ComponentSprite map;
        public static ComponentText selectedLocation;

        //the foreground black rectangle, overlays and hides screen content
        Rectangle overlay;
        public float overlayAlpha = 0.0f;
        float fadeInSpeed = 0.05f;

        //simply visually tracks which menuItem is selected
        public ComponentSprite selectionBox;
        public List<MenuItem> locations;
        public int i;


        public ScreenOverworld() { this.name = "OverworldScreen"; }

        public override void LoadContent()
        {
            overlay = new Rectangle(0, 0, 640, 360);

            window = new MenuWindow(new Point(16 * 11 + 8, 16 * 1 + 8),
                new Point(16 * 17, 16 * 19), "Overworld Map");
            //determine the top left position of the map texture/sprite
            Vector2 mapTopLeft = new Vector2(window.border.position.X + 7, window.border.position.Y + 24);
            map = new ComponentSprite(Assets.overworldSheet,
                mapTopLeft, new Byte4(0, 0, 0, 0), new Point(256, 256));
            map.position.X += map.cellSize.X / 2;
            map.position.Y += map.cellSize.Y / 2;
            selectedLocation = new ComponentText(Assets.font, "Dungeon 1", 
                new Vector2(window.border.position.X + 16 * 7 + 8, window.footerLine.position.Y - 1), 
                Assets.colorScheme.textDark);
            Assets.Play(Assets.sfxMapOpen);

            //create the selectionBox
            selectionBox = new ComponentSprite(Assets.mainSheet,
                new Vector2(0, 0), new Byte4(15, 6, 0, 0),
                new Point(16, 16));

            //create the locations list
            locations = new List<MenuItem>();
            //populate locations with shops
            for (i = 0; i < 5; i++) { locations.Add(new MenuItem()); }
            //populate locations with dungeons
            for (i = 0; i < 4; i++) { locations.Add(new MenuItem()); }
            //we never draw the locations (menuItems), we use them for their neighbors

            //place the shops
            locations[0].compSprite.position = new Vector2(mapTopLeft.X + 24, mapTopLeft.Y + 119); //armor
            locations[1].compSprite.position = new Vector2(mapTopLeft.X + 73, mapTopLeft.Y + 135); //sword
            locations[2].compSprite.position = new Vector2(mapTopLeft.X + 74, mapTopLeft.Y + 173); //ring
            locations[3].compSprite.position = new Vector2(mapTopLeft.X + 130, mapTopLeft.Y + 174); //magic
            locations[4].compSprite.position = new Vector2(mapTopLeft.X + 196, mapTopLeft.Y + 77); //potion

            //place the dungeons
            locations[5].compSprite.position = new Vector2(mapTopLeft.X + 120, mapTopLeft.Y + 107); //center
            locations[6].compSprite.position = new Vector2(mapTopLeft.X + 13, mapTopLeft.Y + 198); //bottom left
            locations[7].compSprite.position = new Vector2(mapTopLeft.X + 233, mapTopLeft.Y + 105); //middle right
            locations[8].compSprite.position = new Vector2(mapTopLeft.X + 137, mapTopLeft.Y + 7); //top center

            //add an 8 pixel offset to the menuItems to align them with the locations
            for (i = 0; i < locations.Count; i++) { locations[i].compSprite.position += new Vector2(8,8); }

            //place the selection box at a location
            selectionBox.position = locations[0].compSprite.position;
            i = 0;




            //open the screen
            displayState = DisplayState.Opening;
        }

        public override void HandleInput(GameTime GameTime)
        {
            if (displayState == DisplayState.Opened)
            {   //only allow input if the screen has opened completely
                if (Input.IsNewButtonPress(Buttons.Start) ||
                    Input.IsNewButtonPress(Buttons.A) ||
                    Input.IsNewButtonPress(Buttons.B))
                {
                    //displayState = DisplayState.Closing;
                    //play the summary exit sound effect immediately
                    //Assets.Play(Assets.sfxExitSummary);

                    //begin closing the screen
                    displayState = DisplayState.Closing;
                }

                //iterate thru the locations
                if (Input.IsNewButtonPress(Buttons.Y))
                {
                    i++;
                    if(i >= locations.Count) { i = 0; }
                    selectionBox.position = locations[i].compSprite.position;
                }
            }
        }

        public override void Update(GameTime GameTime)
        {
            window.Update();

            //center the location text


            #region Handle Screen State

            if (displayState == DisplayState.Opening)
            {
                if (window.interior.displayState == DisplayState.Opened)
                { displayState = DisplayState.Opened; }
            }
            else if (displayState == DisplayState.Closing)
            {
                overlayAlpha += fadeInSpeed;
                if (overlayAlpha >= 1.0f)
                {
                    overlayAlpha = 1.0f;
                    displayState = DisplayState.Closed;
                }
            }
            else if (displayState == DisplayState.Closed)
            {
                ScreenManager.RemoveScreen(this);
                DungeonFunctions.BuildDungeon();
            }

            #endregion

            //pulse the selectionBox alpha
            if (selectionBox.alpha >= 1.0f) { selectionBox.alpha = 0.1f; }
            else { selectionBox.alpha += 0.025f; }
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            DrawFunctions.Draw(window);

            if (window.interior.displayState == DisplayState.Opened || window.interior.displayState == DisplayState.Closing)
            {
                DrawFunctions.Draw(map);
                DrawFunctions.Draw(selectedLocation);
                DrawFunctions.Draw(selectionBox);
            }

            //draw the overlay rec last
            ScreenManager.spriteBatch.Draw(Assets.dummyTexture,
                overlay, Assets.colorScheme.overlay * overlayAlpha);

            ScreenManager.spriteBatch.End();
        }

    }
}
