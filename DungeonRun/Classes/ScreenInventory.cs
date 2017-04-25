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
    public class ScreenInventory : Screen
    {

        //the foreground black rectangle, overlays and hides game content
        Rectangle background;
        public float bkgAlpha = 0.0f;
        public float maxAlpha = 0.7f;
        float fadeInSpeed = 0.03f;
        float fadeOutSpeed = 0.1f;

        public MenuWindow optionsWindow;

        //these point to a menuItem that is part of a widget
        public MenuItem currentlySelected;
        public MenuItem previouslySelected;

        //simply visually tracks which menuItem is selected
        public ComponentSprite selectionBox;



        public ScreenInventory() { this.name = "InventoryScreen"; }

        public override void LoadContent()
        {
            screenState = ScreenState.Opening;

            optionsWindow = new MenuWindow(
                new Point(16 * 24 + 8, 16 * 10),
                new Point(16 * 6 + 8, 16 * 8 + 8),
                "Game Options");

            MenuWidgetLoadout.Reset(
                new Point(16 * 9, 16 * 4),
                new Point(16 * 6 + 8, 16 * 5 + 8));
            MenuWidgetStats.Reset(
                new Point(16 * 9, 16 * 10),
                new Point(16 * 6 + 8, 16 * 8 + 8));
            MenuWidgetInventory.Reset(
                new Point(16 * 16, 16 * 4),
                new Point(16 * 8, 16 * 14 + 8));
            MenuWidgetInfo.Reset(
                new Point(16 * 24 + 8, 16 * 4),
                new Point(16 * 6 + 8, 16 * 5 + 8));

            //set the currently selected menuItem to the first inventory menuItem
            currentlySelected = MenuWidgetInventory.menuItems[0];
            previouslySelected = MenuWidgetInventory.menuItems[0];
            //create the selectionBox
            selectionBox = new ComponentSprite(Assets.mainSheet, 
                new Vector2(0, 0), new Byte4(15, 6, 0, 0), 
                new Byte2(16, 16));

            //create the background rec
            background = new Rectangle(0, 0, 640, 360);
            //play the opening soundFX
            Assets.sfxInventoryOpen.Play();
        }

        public override void HandleInput(GameTime GameTime)
        {
            //exit this screen upon start or b button press
            
            if (Input.IsNewButtonPress(Buttons.Start) ||
                Input.IsNewButtonPress(Buttons.B))
            {
                Assets.sfxInventoryClose.Play();
                //ScreenManager.RemoveScreen(this);
                screenState = ScreenState.Closing;
            }

            else if(Input.IsNewButtonPress(Buttons.A))
            {
                currentlySelected.compSprite.scale = 2.0f;
                Assets.sfxSelectMenuItem.Play();
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
                    Assets.sfxSelectMenuItem.Play();
                    //don't leave any menuItems scaled up beyond 1.0f
                    previouslySelected.compSprite.scale = 1.0f;
                }
            }

        }

        public override void Update(GameTime GameTime)
        {
            //fade background in
            if (screenState == ScreenState.Opening)
            {   
                bkgAlpha += fadeInSpeed;
                if (bkgAlpha >= maxAlpha)
                {
                    bkgAlpha = maxAlpha;
                    screenState = ScreenState.Opened;
                }
            }
            //fade background out
            else if (screenState == ScreenState.Closing)
            {
                bkgAlpha -= fadeOutSpeed;
                if (bkgAlpha <= 0.0f)
                {
                    bkgAlpha = 0.0f;
                    ScreenManager.RemoveScreen(this);
                }
            }

            optionsWindow.Update();
            MenuWidgetLoadout.Update();
            MenuWidgetStats.Update();
            MenuWidgetInfo.Update();
            MenuWidgetInventory.Update();

            //pulse the selectionBox alpha
            if (selectionBox.alpha >= 1.0f)
            { selectionBox.alpha = 0.1f; }
            else { selectionBox.alpha += 0.025f; }
            //match the position of the selectionBox to the currently selected menuItem
            selectionBox.position = currentlySelected.compSprite.position;

            //scale the currently selected item down to 1.0f
            if (currentlySelected.compSprite.scale > 1.0f)
            { currentlySelected.compSprite.scale -= 0.07f; }
            else { currentlySelected.compSprite.scale = 1.0f; }
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            ScreenManager.spriteBatch.Draw(Assets.dummyTexture, background, Assets.colorScheme.overlay * bkgAlpha);

            DrawFunctions.Draw(optionsWindow);

            MenuWidgetLoadout.Draw();
            MenuWidgetStats.Draw();
            MenuWidgetInfo.Draw();
            MenuWidgetInventory.Draw();

            DrawFunctions.Draw(selectionBox);
            ScreenManager.spriteBatch.End();
        }

    }
}