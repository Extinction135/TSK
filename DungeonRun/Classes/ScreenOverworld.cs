﻿using System;
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

        //the foreground black rectangle, overlays and hides screen content
        Rectangle overlay;
        public float overlayAlpha = 0.0f;
        float fadeInSpeed = 0.05f;

        public static MenuWindow window;
        public static ComponentSprite map;
        public static ComponentText selectedLocation;

        //pointers to the locations
        //public MenuItem ShopArmor;
        //public MenuItem ShopWeapon;
        //public MenuItem ShopEquipment;
        //public MenuItem ShopMagic;
        public MenuItem Shop;
        public MenuItem DungeonCursedCastle;
        public MenuItem DungeonDesertPalace;
        public MenuItem DungeonEasternTemple;
        public MenuItem DungeonMountainFortress;

        //these point to a menuItem/location above
        public MenuItem currentlySelected;
        public MenuItem previouslySelected;
        //simply visually tracks which menuItem is selected
        public ComponentSprite selectionBox;
        public List<MenuItem> locations;
        public int i;



        public static void GetCurrentLocationName(MenuItem Location)
        {   //get the location name, center it to the window/screen
            selectedLocation.text = Location.name;
            ComponentFunctions.CenterText(selectedLocation, selectedLocation.font, 320);
        }



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
                new Vector2(0, 0), new Byte4(15, 7, 0, 0),
                new Point(16, 16));

            //create the locations list
            locations = new List<MenuItem>();
            //populate locations with shops
            for (i = 0; i < 5; i++) { locations.Add(new MenuItem()); }
            //populate locations with dungeons
            for (i = 0; i < 4; i++) { locations.Add(new MenuItem()); }
            //we never draw the locations (menuItems), we use them for their neighbors

            //set the named pointers
            //ShopArmor = locations[0];
            //ShopWeapon = locations[1];
            //ShopEquipment = locations[2];
            //ShopMagic = locations[3];
            Shop = locations[4];
            DungeonCursedCastle = locations[5];
            DungeonDesertPalace = locations[6];
            DungeonEasternTemple = locations[7];
            DungeonMountainFortress = locations[8];


            #region Name and Place the Locations

            //name & place the shops
            //ShopArmor.name = "Armor Shop";
            //ShopArmor.compSprite.position = new Vector2(mapTopLeft.X + 24, mapTopLeft.Y + 119); //armor
            //ShopWeapon.name = "Weapon Shop";
            //ShopWeapon.compSprite.position = new Vector2(mapTopLeft.X + 73, mapTopLeft.Y + 135); //sword
            //ShopEquipment.name = "Equipment Shop";
            //ShopEquipment.compSprite.position = new Vector2(mapTopLeft.X + 74, mapTopLeft.Y + 173); //ring
            //ShopMagic.name = "Magic Shop";
            //ShopMagic.compSprite.position = new Vector2(mapTopLeft.X + 130, mapTopLeft.Y + 174); //magic
            Shop.name = "Potion Shop";
            Shop.compSprite.position = new Vector2(mapTopLeft.X + 196, mapTopLeft.Y + 78); //potion

            //name & place the dungeons
            DungeonCursedCastle.name = "Cursed Castle";
            DungeonCursedCastle.compSprite.position = new Vector2(mapTopLeft.X + 121, mapTopLeft.Y + 107); //center
            DungeonDesertPalace.name = "Desert Palace";
            DungeonDesertPalace.compSprite.position = new Vector2(mapTopLeft.X + 13, mapTopLeft.Y + 198); //bottom left
            DungeonEasternTemple.name = "Eastern Temple";
            DungeonEasternTemple.compSprite.position = new Vector2(mapTopLeft.X + 233, mapTopLeft.Y + 105); //middle right
            DungeonMountainFortress.name = "Mountain Fortress";
            DungeonMountainFortress.compSprite.position = new Vector2(mapTopLeft.X + 137, mapTopLeft.Y + 7); //top center

            //add an 8 pixel offset to the menuItems to align them with the locations
            for (i = 0; i < locations.Count; i++) { locations[i].compSprite.position += new Vector2(8,8); }

            #endregion


            #region Setup the Location/MenuItem Neighbors

            //shop - armor
            //ShopArmor.neighborDown = DungeonDesertPalace;
            //ShopArmor.neighborLeft = ShopArmor;
            //ShopArmor.neighborUp = ShopArmor;
            //ShopArmor.neighborRight = ShopWeapon;
            //shop - weapon
            //ShopWeapon.neighborDown = ShopEquipment;
            //ShopWeapon.neighborLeft = ShopArmor;
            //ShopWeapon.neighborUp = ShopWeapon;
            //ShopWeapon.neighborRight = DungeonCursedCastle;
            //shop - equipment
            //ShopEquipment.neighborDown = ShopEquipment;
            //ShopEquipment.neighborLeft = DungeonDesertPalace;
            //ShopEquipment.neighborUp = ShopWeapon;
            //ShopEquipment.neighborRight = ShopMagic;
            //shop - magic
            //ShopMagic.neighborDown = ShopMagic;
            //ShopMagic.neighborLeft = ShopEquipment;
            //ShopMagic.neighborUp = DungeonCursedCastle;
            //ShopMagic.neighborRight = DungeonEasternTemple;
            //shop - potion
            Shop.neighborDown = DungeonEasternTemple;
            Shop.neighborLeft = DungeonCursedCastle;
            Shop.neighborUp = DungeonMountainFortress;
            Shop.neighborRight = DungeonEasternTemple;

            //dungeon - cursed castle
            DungeonCursedCastle.neighborDown = DungeonDesertPalace;
            DungeonCursedCastle.neighborLeft = DungeonDesertPalace;
            DungeonCursedCastle.neighborUp = DungeonMountainFortress;
            DungeonCursedCastle.neighborRight = Shop;
            //dungeon - desert palace
            DungeonDesertPalace.neighborDown = DungeonDesertPalace;
            DungeonDesertPalace.neighborLeft = DungeonDesertPalace;
            DungeonDesertPalace.neighborUp = DungeonCursedCastle;
            DungeonDesertPalace.neighborRight = DungeonCursedCastle;
            //dungeon - eastern temple
            DungeonEasternTemple.neighborDown = DungeonEasternTemple;
            DungeonEasternTemple.neighborLeft = Shop;
            DungeonEasternTemple.neighborUp = Shop;
            DungeonEasternTemple.neighborRight = DungeonEasternTemple;
            //dungeon - mountain fortress
            DungeonMountainFortress.neighborDown = DungeonCursedCastle;
            DungeonMountainFortress.neighborLeft = DungeonMountainFortress;
            DungeonMountainFortress.neighborUp = DungeonMountainFortress;
            DungeonMountainFortress.neighborRight = Shop;

            #endregion


            //set the selected menuItem pointers
            currentlySelected = DungeonCursedCastle;
            previouslySelected = DungeonCursedCastle;
            //get the current location's name
            GetCurrentLocationName(currentlySelected);
            //reset the index
            i = 0;
            //open the screen
            displayState = DisplayState.Opening;
            //play the title music
            MusicFunctions.PlayMusic(Music.Title);
            //fill hero's health up to max - prevents drum track from playing
            Pool.hero.health = Pool.hero.maxHealth;
        }

        public override void HandleInput(GameTime GameTime)
        {
            if (displayState == DisplayState.Opened)
            {   //only allow input if the screen has opened completely
                //get the previouslySelected menuItem
                previouslySelected = currentlySelected;
                //check to see if the gamePad direction is a new direction - prevents rapid scrolling
                if (Input.gamePadDirection != Input.lastGamePadDirection)
                {
                    //this is a new direction, allow movement between menuItems
                    if (Input.gamePadDirection == Direction.Right)
                    { currentlySelected = currentlySelected.neighborRight; }
                    else if (Input.gamePadDirection == Direction.Left)
                    { currentlySelected = currentlySelected.neighborLeft; }
                    else if (Input.gamePadDirection == Direction.Down)
                    { currentlySelected = currentlySelected.neighborDown; }
                    else if (Input.gamePadDirection == Direction.Up)
                    { currentlySelected = currentlySelected.neighborUp; }

                    //check to see if we changed menuItems
                    if (previouslySelected != currentlySelected)
                    {
                        GetCurrentLocationName(currentlySelected);
                        Assets.Play(Assets.sfxTextLetter);
                        selectionBox.scale = 2.0f;
                    }
                }
                else if (Input.IsNewButtonPress(Buttons.Start) || Input.IsNewButtonPress(Buttons.A))
                {
                    Assets.Play(Assets.sfxMenuItem); //play soundfx
                    displayState = DisplayState.Closing; //begin closing the screen
                    //later we'll build the dungeon or shop based on the currentlySelected location
                }
            }
        }

        public override void Update(GameTime GameTime)
        {
            window.Update();


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

                if (currentlySelected == Shop) { DungeonFunctions.BuildDungeon(DungeonType.Shop); }
                else { DungeonFunctions.BuildDungeon(DungeonType.CursedCastle); }
            }

            #endregion


            if (displayState != DisplayState.Opening)
            {   //if screen is opened, closing, or closed, pulse the selectionBox alpha
                if (selectionBox.alpha >= 1.0f) { selectionBox.alpha = 0.1f; }
                else { selectionBox.alpha += 0.025f; }
                //match the position of the selectionBox to the currently selected menuItem
                selectionBox.position = currentlySelected.compSprite.position;
                //scale the selectionBox down to 1.0
                if (selectionBox.scale > 1.0f) { selectionBox.scale -= 0.07f; }
                else { selectionBox.scale = 1.0f; }
            }
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
