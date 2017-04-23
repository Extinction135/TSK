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

        //fields

        //hero window
        //inventory window
        //description window
        //options window



        public ScreenInventory() { this.name = "InventoryScreen"; }

        public override void LoadContent()
        {
            screenState = ScreenState.Opening;
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
                    //play the summary exit sound effect immediately
                    //Assets.sfxExitSummary.Play();
                    ScreenManager.RemoveScreen(this);
                }
            }
        }

        public override void Update(GameTime GameTime)
        {
            
        }

        public override void Draw(GameTime GameTime)
        {

        }

    }
}