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

        public MenuWindow inventoryWindow;
        public MenuWindow statsWindow;
        public MenuWindow selectionWindow;
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

            //play the menu opening soundFX

            inventoryWindow = new MenuWindow(
                new Point(16 * 9, 16 * 4), 
                new Point(16 * 6 + 8, 16 * 5 + 8), 
                "Current Inventory");
            statsWindow = new MenuWindow(
                new Point(16 * 9, 16 * 10),
                new Point(16 * 6 + 8, 16 * 8 + 8),
                "Stats");
            selectionWindow = new MenuWindow(
                new Point(16 * 16, 16 * 4),
                new Point(16 * 8, 16 * 14 + 8),
                "Item");
            optionsWindow = new MenuWindow(
                new Point(16 * 24 + 8, 16 * 10),
                new Point(16 * 6 + 8, 16 * 8 + 8),
                "Game Options");



            MenuWidgetInfo.Reset(
                new Point(16 * 24 + 8, 16 * 4),
                new Point(16 * 6 + 8, 16 * 5 + 8));
        }

        public override void HandleInput(GameTime GameTime)
        {
            //exit this screen upon start or b button press
            
            if (Input.IsNewButtonPress(Buttons.Start) ||
                Input.IsNewButtonPress(Buttons.B))
            {
                //screenState = ScreenState.Closing;
                //play the menu close soundFX
                //Assets.sfxExitSummary.Play();
                ScreenManager.RemoveScreen(this);
            }
            


            /*
            //move between menu items one at a time
            if (Input.SNEScontroller.IsNewDpadDirection()) //this prevents fast menuItem scrolling
            {
                //if this is new directional input, update the currentlySelected menuItem based on it's neighbors
                if (Input.SNEScontroller.currentDpadDirection == Direction.Right) { currentlySelected = currentlySelected.neighborRight; }
                else if (Input.SNEScontroller.currentDpadDirection == Direction.Left) { currentlySelected = currentlySelected.neighborLeft; }
                else if (Input.SNEScontroller.currentDpadDirection == Direction.Up) { currentlySelected = currentlySelected.neighborUp; }
                else if (Input.SNEScontroller.currentDpadDirection == Direction.Down) { currentlySelected = currentlySelected.neighborDown; }
            }
            */


        }

        public override void Update(GameTime GameTime)
        {
            inventoryWindow.Update();
            statsWindow.Update();
            selectionWindow.Update();
            optionsWindow.Update();

            MenuWidgetInfo.Update();
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);


            DrawFunctions.Draw(inventoryWindow);
            DrawFunctions.Draw(statsWindow);
            DrawFunctions.Draw(selectionWindow);
            DrawFunctions.Draw(optionsWindow);



            MenuWidgetInfo.Draw();

            
            ScreenManager.spriteBatch.End();
        }

    }
}