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
    public class ScreenVendor : Screen
    {

        //the foreground black rectangle, overlays and hides game content
        Rectangle background;
        public float bkgAlpha = 0.0f;
        public float maxAlpha = 0.7f;
        float fadeInSpeed = 0.03f;
        float fadeOutSpeed = 0.1f;

        //these point to a menuItem that is part of a widget
        public MenuItem currentlySelected;
        public MenuItem previouslySelected;

        //simply visually tracks which menuItem is selected
        public ComponentSprite selectionBox;
        public GameObject vendorType;


        public ScreenVendor(GameObject Obj)
        {
            this.name = "Vendor Screen";
            vendorType = Obj;
        }

        public override void LoadContent()
        {
            displayState = DisplayState.Opening;

            MenuWidgetLoadout.Reset(
                new Point(16 * 9, 16 * 8),
                new Point(16 * 6 + 8, 16 * 5 + 8));
            MenuWidgetForSale.Reset(
                new Point(16 * 16, 16 * 8),
                new Point(16 * 8, 16 * 5 + 8));
            MenuWidgetInfo.Reset(
                new Point(16 * 24 + 8, 16 * 8),
                new Point(16 * 6 + 8, 16 * 5 + 8));
            
            //Connect Loadout menuItems to ForSale menuItems
            MenuWidgetLoadout.menuItems[3].neighborRight = MenuWidgetForSale.menuItems[0];
            MenuWidgetLoadout.menuItems[7].neighborRight = MenuWidgetForSale.menuItems[5];
            MenuWidgetForSale.menuItems[0].neighborLeft = MenuWidgetLoadout.menuItems[3];
            MenuWidgetForSale.menuItems[5].neighborLeft = MenuWidgetLoadout.menuItems[7];


            //set the menuItems based on the vendorType
            if(vendorType.type == ObjType.VendorItems)
            {
                MenuItemFunctions.SetMenuItemData(MenuItemType.ItemBoomerang, MenuWidgetForSale.menuItems[0]);
                MenuItemFunctions.SetMenuItemData(MenuItemType.ItemBomb, MenuWidgetForSale.menuItems[1]);
            }
            else
            {
                //other vendor for sale items here...
            }


            //set the currently selected menuItem to the first inventory menuItem
            currentlySelected = MenuWidgetForSale.menuItems[0];
            previouslySelected = MenuWidgetForSale.menuItems[0];
            MenuWidgetInfo.Display(currentlySelected);

            //create the selectionBox
            selectionBox = new ComponentSprite(Assets.mainSheet,
                new Vector2(0, 0), new Byte4(15, 6, 0, 0),
                new Point(16, 16));

            //create the background rec
            background = new Rectangle(0, 0, 640, 360);
            //play the opening soundFX
            Assets.Play(Assets.sfxInventoryOpen);
        }

        public override void HandleInput(GameTime GameTime)
        {
            //exit this screen upon start or b button press

            if (Input.IsNewButtonPress(Buttons.Start) ||
                Input.IsNewButtonPress(Buttons.B))
            {
                Assets.Play(Assets.sfxInventoryClose);
                //ScreenManager.RemoveScreen(this);
                displayState = DisplayState.Closing;
            }

            else if (Input.IsNewButtonPress(Buttons.A))
            {
                currentlySelected.compSprite.scale = 2.0f;
                Assets.Play(Assets.sfxMenuItem);

                //check if player already has currently selected in inventory
                //calculate if player has enough gold to purchase currently selected
                //either purchase or reject
            }

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
                    MenuWidgetInfo.Display(currentlySelected);
                    Assets.Play(Assets.sfxTextLetter);
                    previouslySelected.compSprite.scale = 1.0f;
                    selectionBox.scale = 2.0f;
                    //display the currently selected item's price in the for sale window title
                    MenuWidgetForSale.window.title.text = "For Sale - " + currentlySelected.price;
                }
            }

        }

        public override void Update(GameTime GameTime)
        {
            //fade background in
            if (displayState == DisplayState.Opening)
            {
                bkgAlpha += fadeInSpeed;
                if (bkgAlpha >= maxAlpha)
                {
                    bkgAlpha = maxAlpha;
                    displayState = DisplayState.Opened;
                }
            }
            //fade background out
            else if (displayState == DisplayState.Closing)
            {
                bkgAlpha -= fadeOutSpeed;
                if (bkgAlpha <= 0.0f)
                {
                    bkgAlpha = 0.0f;
                    ScreenManager.RemoveScreen(this);
                }
            }

            MenuWidgetLoadout.Update();
            MenuWidgetForSale.Update();
            MenuWidgetInfo.Update();

            //pulse the selectionBox alpha
            if (selectionBox.alpha >= 1.0f) { selectionBox.alpha = 0.1f; }
            else { selectionBox.alpha += 0.025f; }
            //match the position of the selectionBox to the currently selected menuItem
            selectionBox.position = currentlySelected.compSprite.position;
            AnimationFunctions.Animate(currentlySelected.compAnim, currentlySelected.compSprite);
            //scale the selectionBox down to 1.0
            if (selectionBox.scale > 1.0f) { selectionBox.scale -= 0.07f; }
            else { selectionBox.scale = 1.0f; }
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            ScreenManager.spriteBatch.Draw(Assets.dummyTexture, background, Assets.colorScheme.overlay * bkgAlpha);

            MenuWidgetLoadout.Draw();
            MenuWidgetForSale.Draw();
            MenuWidgetInfo.Draw();

            DrawFunctions.Draw(selectionBox);
            ScreenManager.spriteBatch.End();
        }

    }
}