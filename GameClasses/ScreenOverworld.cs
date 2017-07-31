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
        ScreenRec background = new ScreenRec();
        ScreenRec overlay = new ScreenRec();
        public Scroll scroll;
        public static ComponentSprite map;

        //pointers to the locations
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



        public ScreenOverworld() { this.name = "OverworldScreen"; }

        public override void LoadContent()
        {
            background.alpha = 1.0f;
            overlay.alpha = 1.0f;
            overlay.fadeInSpeed = 0.03f; //fade in slow

            scroll = new Scroll(new Vector2(16 * 10, 16 * 2), 18, 19);
            //open the scroll
            scroll.displayState = DisplayState.Opening;
            Assets.Play(Assets.sfxMapOpen);

            //determine the top left position of the map texture/sprite
            Vector2 mapTopLeft = scroll.startPos + new Vector2(16+4, 24);
            //Vector2 mapTopLeft = new Vector2(window.border.position.X + 7, window.border.position.Y + 24);
            map = new ComponentSprite(Assets.overworldSheet,
                mapTopLeft, new Byte4(0, 0, 0, 0), new Point(256, 256));
            map.position.X += map.cellSize.X / 2;
            map.position.Y += map.cellSize.Y / 2;

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
            //play the title music
            Functions_Music.PlayMusic(Music.Title);
            //fill hero's health up to max - prevents drum track from playing
            Pool.hero.health = Pool.hero.maxHealth;
        }

        public override void HandleInput(GameTime GameTime)
        {
            if (scroll.displayState == DisplayState.Opened)
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
                else if (Functions_Input.IsNewButtonPress(Buttons.Start) || 
                    Functions_Input.IsNewButtonPress(Buttons.A))
                {
                    Assets.Play(Assets.sfxMenuItem); //play selection sfx
                    Assets.Play(Assets.sfxInventoryClose); //play closing sfx
                    scroll.displayState = DisplayState.Closing; //close scroll
                }
            }
        }

        public override void Update(GameTime GameTime)
        {
            if (scroll.displayState == DisplayState.Opening)
            {   //fade overlay out
                overlay.fadeState = FadeState.FadeOut;
                Functions_ScreenRec.Fade(overlay);
                Functions_Scroll.AnimateOpen(scroll);
            }
            else if(scroll.displayState == DisplayState.Opened)
            {   //pulse the alpha of the selection box
                if (selectionBox.alpha >= 1.0f) { selectionBox.alpha = 0.1f; }
                else { selectionBox.alpha += 0.025f; }
                //match the position of the selectionBox to the currently selected menuItem
                selectionBox.position = currentlySelected.compSprite.position;
                //scale the selectionBox down to 1.0
                if (selectionBox.scale > 1.0f) { selectionBox.scale -= 0.07f; }
                else { selectionBox.scale = 1.0f; }
            }
            else if (scroll.displayState == DisplayState.Closing)
            {   //fade overlay in
                overlay.fadeState = FadeState.FadeIn;
                Functions_ScreenRec.Fade(overlay);
                Functions_Scroll.AnimateClosed(scroll);
            }
            else if (scroll.displayState == DisplayState.Closed)
            {   //set the type of dungeon we are about to build/load
                if (currentlySelected == Shop)
                { Functions_Dungeon.dungeonType = DungeonType.Shop; }
                else { Functions_Dungeon.dungeonType = DungeonType.CursedCastle; }
                //dungeon is built by dungeon screen
                ScreenManager.ExitAndLoad(new ScreenDungeon());
            }
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            Functions_Draw.Draw(background);
            Functions_Scroll.Draw(scroll);
            if (scroll.displayState == DisplayState.Opened)
            {
                Functions_Draw.Draw(map);
                Functions_Draw.Draw(selectionBox);
            }
            Functions_Draw.Draw(overlay);
            ScreenManager.spriteBatch.End();
        }



        public void GetCurrentLocationName(MenuItem Location)
        { scroll.title.text = "Overworld Map - " + Location.name; }

    }
}
