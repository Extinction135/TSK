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

        //these point to a menuItem that is part of a widget
        public MenuItem currentlySelected;
        public MenuItem previouslySelected;

        //simply visually tracks which menuItem is selected
        public ComponentSprite selectionBox;



        public ScreenInventory() { this.name = "InventoryScreen"; }

        public override void LoadContent()
        {
            displayState = DisplayState.Opening;

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
            MenuWidgetOptions.Reset(
                new Point(16 * 24 + 8, 16 * 10),
                new Point(16 * 6 + 8, 16 * 8 + 8));


            #region Connect loadout widget's menuItems to stats widget's menuItems

            MenuWidgetLoadout.menuItems[4].neighborDown = MenuWidgetStats.menuItems[0];
            MenuWidgetLoadout.menuItems[5].neighborDown = MenuWidgetStats.menuItems[0];
            MenuWidgetLoadout.menuItems[6].neighborDown = MenuWidgetStats.menuItems[0];
            MenuWidgetLoadout.menuItems[7].neighborDown = MenuWidgetStats.menuItems[0];
            MenuWidgetStats.menuItems[0].neighborUp = MenuWidgetLoadout.menuItems[4];

            #endregion


            #region Connect loadout widget's menuItems to inventory widget's menuItems

            MenuWidgetLoadout.menuItems[3].neighborRight = MenuWidgetInventory.menuItems[0];
            MenuWidgetInventory.menuItems[0].neighborLeft = MenuWidgetLoadout.menuItems[3];

            MenuWidgetLoadout.menuItems[7].neighborRight = MenuWidgetInventory.menuItems[5];
            MenuWidgetInventory.menuItems[5].neighborLeft = MenuWidgetLoadout.menuItems[7];

            #endregion


            #region Connect stat widget's menuItems to inventory widget's menuItems

            MenuWidgetStats.menuItems[0].neighborRight = MenuWidgetInventory.menuItems[10];
            MenuWidgetInventory.menuItems[10].neighborLeft = MenuWidgetStats.menuItems[0];

            MenuWidgetStats.menuItems[1].neighborRight = MenuWidgetInventory.menuItems[15];
            MenuWidgetInventory.menuItems[15].neighborLeft = MenuWidgetStats.menuItems[1];

            MenuWidgetStats.menuItems[2].neighborRight = MenuWidgetInventory.menuItems[15];

            MenuWidgetStats.menuItems[3].neighborRight = MenuWidgetInventory.menuItems[20];
            MenuWidgetInventory.menuItems[20].neighborLeft = MenuWidgetStats.menuItems[3];

            #endregion


            #region Connect options widget's menuItems to inventory widget's menuItems

            MenuWidgetOptions.menuItems[0].neighborLeft = MenuWidgetInventory.menuItems[14];
            MenuWidgetInventory.menuItems[14].neighborRight = MenuWidgetOptions.menuItems[0];

            MenuWidgetOptions.menuItems[2].neighborLeft = MenuWidgetInventory.menuItems[19];
            MenuWidgetInventory.menuItems[19].neighborRight = MenuWidgetOptions.menuItems[2];

            MenuWidgetOptions.menuItems[4].neighborLeft = MenuWidgetInventory.menuItems[19];

            MenuWidgetOptions.menuItems[6].neighborLeft = MenuWidgetInventory.menuItems[24];
            MenuWidgetInventory.menuItems[24].neighborRight = MenuWidgetOptions.menuItems[6];

            #endregion


            //set the currently selected menuItem to the first inventory menuItem
            currentlySelected = MenuWidgetInventory.menuItems[0];
            previouslySelected = MenuWidgetInventory.menuItems[0];
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

            else if(Input.IsNewButtonPress(Buttons.A))
            {
                currentlySelected.compSprite.scale = 2.0f;
                Assets.Play(Assets.sfxMenuItem);
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
            MenuWidgetStats.Update();
            MenuWidgetInfo.Update();
            MenuWidgetInventory.Update();
            MenuWidgetOptions.Update();

            //pulse the selectionBox alpha
            if (selectionBox.alpha >= 1.0f) { selectionBox.alpha = 0.1f; }
            else { selectionBox.alpha += 0.025f; }
            //match the position of the selectionBox to the currently selected menuItem
            selectionBox.position = currentlySelected.compSprite.position;
            AnimationFunctions.Animate(currentlySelected.compAnim, currentlySelected.compSprite);
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            ScreenManager.spriteBatch.Draw(Assets.dummyTexture, background, Assets.colorScheme.overlay * bkgAlpha);

            MenuWidgetLoadout.Draw();
            MenuWidgetStats.Draw();
            MenuWidgetInfo.Draw();
            MenuWidgetInventory.Draw();
            MenuWidgetOptions.Draw();

            DrawFunctions.Draw(selectionBox);
            ScreenManager.spriteBatch.End();
        }

    }
}