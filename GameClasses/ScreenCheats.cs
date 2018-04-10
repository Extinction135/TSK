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
    public class ScreenCheats : Screen
    {
        int i;
        MenuWindow window;
        ScreenRec background = new ScreenRec();

        //the list of menuItems and texts for cheats
        public List<ComponentText> labels;
        public List<MenuItem> menuItems;

        //these point to a menuItem
        public MenuItem currentlySelected;
        public MenuItem previouslySelected;
        //simply visually tracks which menuItem is selected
        public ComponentSprite selectionBox;

        

        


        public ScreenCheats()
        {
            this.name = "Cheats Screen";

            /*
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
            */
        }

        public override void LoadContent()
        {
            //setup the screen
            background.alpha = 0.0f;
            background.fadeInSpeed = 0.03f;
            background.fadeOutSpeed = 0.07f;
            background.maxAlpha = 0.7f;
            displayState = DisplayState.Opening;


            #region Create Window, Labels, MenuItems

            //create the bkg window
            window = new MenuWindow(
                new Point(16 * 8 - 8, 16 * 4), 
                new Point(16 * 18, 16 * 10), 
                "Cheats");

            int columns = 5;
            int rows = 2;

            //create menuitem labels
            labels = new List<ComponentText>();
            for (i = 0; i < columns * rows; i++)
            {
                labels.Add(new ComponentText(Assets.font,
                  "test\ntest", new Vector2(-100, -100),
                  Assets.colorScheme.textDark));
            }

            //create menuitems
            menuItems = new List<MenuItem>();
            for (i = 0; i < columns * rows; i++) { menuItems.Add(new MenuItem()); }

            #endregion



            #region Set MenuItems & Labels

            //CheatsInfiniteHP
            labels[0].text = "inf.\nhp"; 
            Functions_MenuItem.SetType(MenuItemType.CheatsInfiniteHP, menuItems[0]);
            menuItems[0].compSprite.position.X = window.interior.rec.X + 8 + 5;
            menuItems[0].compSprite.position.Y = window.interior.rec.Y + 16 + 8 + 5;

            //CheatsInfiniteGold
            labels[1].text = "inf.\ngold"; 
            Functions_MenuItem.SetType(MenuItemType.CheatsInfiniteGold, menuItems[1]);
            menuItems[1].compSprite.position.X = menuItems[0].compSprite.position.X;
            menuItems[1].compSprite.position.Y = menuItems[0].compSprite.position.Y + 24;

            //CheatsInfiniteMagic
            labels[2].text = "inf.\nmagic";
            Functions_MenuItem.SetType(MenuItemType.CheatsInfiniteMagic, menuItems[2]);
            menuItems[2].compSprite.position.X = menuItems[1].compSprite.position.X;
            menuItems[2].compSprite.position.Y = menuItems[1].compSprite.position.Y + 24;

            //CheatsInfiniteArrows
            labels[3].text = "inf.\narrws";
            Functions_MenuItem.SetType(MenuItemType.CheatsInfiniteArrows, menuItems[3]);
            menuItems[3].compSprite.position.X = menuItems[2].compSprite.position.X;
            menuItems[3].compSprite.position.Y = menuItems[2].compSprite.position.Y + 24;

            //CheatsInfiniteBombs
            labels[4].text = "inf.\nbombs";
            Functions_MenuItem.SetType(MenuItemType.CheatsInfiniteBombs, menuItems[4]);
            menuItems[4].compSprite.position.X = menuItems[3].compSprite.position.X;
            menuItems[4].compSprite.position.Y = menuItems[3].compSprite.position.Y + 24;

            //CheatsMap (column2)
            labels[5].text = "got\nmap";
            Functions_MenuItem.SetType(MenuItemType.CheatsMap, menuItems[5]);
            menuItems[5].compSprite.position.X = menuItems[0].compSprite.position.X + 24 + 16;
            menuItems[5].compSprite.position.Y = menuItems[0].compSprite.position.Y;

            //CheatsKey
            labels[6].text = "got\nkey";
            Functions_MenuItem.SetType(MenuItemType.CheatsKey, menuItems[6]);
            menuItems[6].compSprite.position.X = menuItems[5].compSprite.position.X;
            menuItems[6].compSprite.position.Y = menuItems[5].compSprite.position.Y + 24;

            //CheatsUnlockAll
            labels[7].text = "unlck\nall";
            Functions_MenuItem.SetType(MenuItemType.CheatsUnlockAll, menuItems[7]);
            menuItems[7].compSprite.position.X = menuItems[6].compSprite.position.X;
            menuItems[7].compSprite.position.Y = menuItems[6].compSprite.position.Y + 24;

            #endregion



            #region Set Vertical Neighbors

            //column1
            menuItems[0].neighborDown = menuItems[1];
            menuItems[1].neighborUp = menuItems[0];

            menuItems[1].neighborDown = menuItems[2];
            menuItems[2].neighborUp = menuItems[1];

            menuItems[2].neighborDown = menuItems[3];
            menuItems[3].neighborUp = menuItems[2];

            menuItems[3].neighborDown = menuItems[4];
            menuItems[4].neighborUp = menuItems[3];



            //column2
            menuItems[5].neighborDown = menuItems[6];
            menuItems[6].neighborUp = menuItems[5];

            menuItems[6].neighborDown = menuItems[7];
            menuItems[7].neighborUp = menuItems[6];


            #endregion


            #region Set Horizontal Neighbors

            menuItems[0].neighborRight = menuItems[5];
            menuItems[5].neighborLeft = menuItems[0];

            menuItems[1].neighborRight = menuItems[6];
            menuItems[6].neighborLeft = menuItems[1];

            menuItems[2].neighborRight = menuItems[7];
            menuItems[7].neighborLeft = menuItems[2];

            #endregion


            #region Finish Up, prep for screen opening

            //set labels relative to menuItems
            for (i = 0; i < labels.Count; i++)
            {
                labels[i].position.X = menuItems[i].compSprite.position.X + 12;
                labels[i].position.Y = menuItems[i].compSprite.position.Y - 12;
            }

            //set the cheat menuItem sprites based on their relative booleans
            SetCheatMenuItems();

            //set the currently selected menuItem to the first inventory menuItem
            currentlySelected = menuItems[0];
            previouslySelected = menuItems[0];
            Widgets.Info.Display(currentlySelected);
            //create the selectionBox
            selectionBox = new ComponentSprite(Assets.uiItemsSheet,
                new Vector2(0, 0),
                AnimationFrames.Ui_SelectionBox[0],
                new Point(16, 16));
            //play the opening soundFX
            Assets.Play(Assets.sfxWindowOpen);

            #endregion

        }

        public override void HandleInput(GameTime GameTime)
        {   //prevent all input until the screen has opened
            if (displayState != DisplayState.Opened) { return; }
            base.HandleInput(GameTime);


            //Handle A Button Input
            if (Functions_Input.IsNewButtonPress(Buttons.A))
            {

                #region Handle Cheat Effects

                if (currentlySelected.type == MenuItemType.CheatsInfiniteHP)
                {
                    if (Flags.Invincibility) { Flags.Invincibility = false; }
                    else { Flags.Invincibility = true; }
                }
                else if (currentlySelected.type == MenuItemType.CheatsInfiniteGold)
                {
                    if (Flags.InfiniteGold) { Flags.InfiniteGold = false; }
                    else { Flags.InfiniteGold = true; }
                }
                else if (currentlySelected.type == MenuItemType.CheatsInfiniteMagic)
                {
                    if (Flags.InfiniteMagic) { Flags.InfiniteMagic = false; }
                    else { Flags.InfiniteMagic = true; }
                }
                else if (currentlySelected.type == MenuItemType.CheatsInfiniteArrows)
                {
                    if (Flags.InfiniteArrows) { Flags.InfiniteArrows = false; }
                    else { Flags.InfiniteArrows = true; }
                }
                else if (currentlySelected.type == MenuItemType.CheatsInfiniteBombs)
                {
                    if (Flags.InfiniteBombs) { Flags.InfiniteBombs = false; }
                    else { Flags.InfiniteBombs = true; }
                }
                
                else if (currentlySelected.type == MenuItemType.CheatsKey)
                {
                    if (Flags.KeyCheat) { Flags.KeyCheat = false; }
                    else { Flags.KeyCheat = true; }
                    Level.bigKey = Flags.KeyCheat;
                }
                else if (currentlySelected.type == MenuItemType.CheatsMap)
                {
                    if (Flags.MapCheat) { Flags.MapCheat = false; }
                    else { Flags.MapCheat = true; }
                    Level.map = Flags.MapCheat;
                }
                else if (currentlySelected.type == MenuItemType.CheatsUnlockAll)
                {
                    Flags.UnlockAll = true;
                    Functions_Hero.UnlockAll();
                }




                #endregion


                //we will always have a menuItem selected
                Assets.Play(Assets.sfxMenuItem);
                SetCheatMenuItems();
            }


            #region Exit Screen Input

            //exit this screen upon start / b button press
            else if (Functions_Input.IsNewButtonPress(Buttons.Start)
                || Functions_Input.IsNewButtonPress(Buttons.B))
            {
                Assets.Play(Assets.sfxWindowClose);
                displayState = DisplayState.Closing;
                Functions_MenuWindow.Close(window);
            }

            #endregion


            #region Move Between MenuItems Input

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
                    Widgets.Info.Display(currentlySelected);
                    Assets.Play(Assets.sfxTextLetter);
                    previouslySelected.compSprite.scale = 1.0f;
                    selectionBox.scale = 2.0f;
                }
            }

            #endregion


        }

        public override void Update(GameTime GameTime)
        {
            base.Update(GameTime);

            //update the window, and set screen's display state relative
            Functions_MenuWindow.Update(window);


            #region Handle Display State

            if (displayState == DisplayState.Opening)
            {   //fade background in
                background.fadeState = FadeState.FadeIn;
                Functions_ScreenRec.Fade(background);
                if (background.fadeState == FadeState.FadeComplete)
                {
                    displayState = DisplayState.Opened;
                    Assets.Play(Assets.sfxTextLetter);
                }
                selectionBox.scale = 2.0f;
            }
            else if (displayState == DisplayState.Closing)
            {   //fade background out
                background.fadeState = FadeState.FadeOut;
                Functions_ScreenRec.Fade(background);
                if (background.fadeState == FadeState.FadeComplete)
                { displayState = DisplayState.Closed; }
            }
            else if (displayState == DisplayState.Closed)
            {   //overlay has faded in 100%
                ScreenManager.RemoveScreen(this);
            }

            #endregion


            #region Handle SelectionBox

            //pulse the selectionBox alpha
            if (selectionBox.alpha >= 1.0f) { selectionBox.alpha = 0.1f; }
            else { selectionBox.alpha += 0.025f; }
            //match the position of the selectionBox to the currently selected menuItem
            selectionBox.position = currentlySelected.compSprite.position;
            //scale the selectionBox down to 1.0
            if (selectionBox.scale > 1.0f) { selectionBox.scale -= 0.07f; }
            else { selectionBox.scale = 1.0f; }

            #endregion

        }

        public override void Draw(GameTime GameTime)
        {
            base.Draw(GameTime);
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            //draw stuff
            Functions_Draw.Draw(background);

            Functions_Draw.Draw(window);
            //only draw the selection box if the screen has opened completely
            if (displayState == DisplayState.Opened)
            {
                for (i = 0; i < menuItems.Count; i++)
                { Functions_Draw.Draw(menuItems[i].compSprite); }
                for (i = 0; i < labels.Count; i++)
                { Functions_Draw.Draw(labels[i]); }

                Functions_Draw.Draw(selectionBox);
            }
            Widgets.Info.Draw();
            ScreenManager.spriteBatch.End();
        }



        void SetCheatMenuItems()
        {   //reset all menuItems to unknown state
            for (i = 0; i < menuItems.Count; i++)
            { menuItems[i].compSprite.currentFrame = AnimationFrames.Ui_MenuItem_CheatOff[0]; }

            //set menuItem based on boolean
            if (Flags.Invincibility) { menuItems[0].compSprite.currentFrame = AnimationFrames.Ui_MenuItem_CheatOn[0]; }
            if (Flags.InfiniteGold) { menuItems[1].compSprite.currentFrame = AnimationFrames.Ui_MenuItem_CheatOn[0]; }
            if (Flags.InfiniteMagic) { menuItems[2].compSprite.currentFrame = AnimationFrames.Ui_MenuItem_CheatOn[0]; }
            if (Flags.InfiniteArrows) { menuItems[3].compSprite.currentFrame = AnimationFrames.Ui_MenuItem_CheatOn[0]; }
            if (Flags.InfiniteBombs) { menuItems[4].compSprite.currentFrame = AnimationFrames.Ui_MenuItem_CheatOn[0]; }
            //column2
            if (Flags.MapCheat) { menuItems[5].compSprite.currentFrame = AnimationFrames.Ui_MenuItem_CheatOn[0]; }
            if (Flags.KeyCheat) { menuItems[6].compSprite.currentFrame = AnimationFrames.Ui_MenuItem_CheatOn[0]; }
            if (Flags.UnlockAll) { menuItems[7].compSprite.currentFrame = AnimationFrames.Ui_MenuItem_CheatOn[0]; }
            //expand this to include additional cheats
        }

    }
}