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

        //inventory window
        //stats window
        //selection window
        //info window
        //options window
        public MenuWindow inventoryWindow;


        public ScreenInventory() { this.name = "InventoryScreen"; }

        public override void LoadContent()
        {
            screenState = ScreenState.Opening;

            //play the menu opening soundFX

            inventoryWindow = new MenuWindow(
                new Point(16 * 9, 16 * 4), 
                new Point(16 * 6 + 8, 16 * 5 + 8), 
                "Current Inventory");
        }

        public override void HandleInput(GameTime GameTime)
        {
            //exit this screen upon start or b button press
            //if (screenState == ScreenState.Opened)
            {
                if (
                    Input.IsNewButtonPress(Buttons.Start) ||
                    Input.IsNewButtonPress(Buttons.B))
                {
                    //screenState = ScreenState.Closing;
                    //play the menu close soundFX
                    //Assets.sfxExitSummary.Play();
                    ScreenManager.RemoveScreen(this);
                }
            }
        }

        public override void Update(GameTime GameTime)
        {
            inventoryWindow.Update();
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            DrawFunctions.Draw(inventoryWindow);
            ScreenManager.spriteBatch.End();
        }

    }
}