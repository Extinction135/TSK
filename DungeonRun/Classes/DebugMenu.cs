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
    public static class DebugMenu
    {

        public static Rectangle rec; //background rec
        public static List<ComponentButton> buttons;
        public static int counter;

        static DebugMenu()
        {
            rec = new Rectangle(0, 0, 640, 13);
            buttons = new List<ComponentButton>();
            buttons.Add(new ComponentButton("draw collisions", new Point(2, 2)));
            buttons.Add(new ComponentButton("build dungeon", new Point(68, 2)));
        }

        public static void HandleInput()
        {
            //dump the states for every active actor if Enter key is pressed
            if (Input.IsNewKeyPress(Keys.Enter))
            {
                for (Pool.counter = 0; Pool.counter < Pool.actorCount; Pool.counter++)
                {
                    if (Pool.actorPool[Pool.counter].active)
                    { DebugFunctions.Inspect(Pool.actorPool[Pool.counter]); }
                }
            }


            //check each button for user mouse button input (click + hover states)
            for (counter = 0; counter < buttons.Count; counter++)
            {   //check each button to see if it contains the cursor
                if (buttons[counter].rec.Contains(Input.cursorPos))
                {   //any button containing the cursor draws with 'over' color
                    buttons[counter].currentColor = Assets.colorScheme.buttonOver;
                    if(Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
                    {   //any button clicked on becomes selected


                        #region Button Events

                        if (counter == 0) //toggle draw collisions boolean
                        {
                            if (Flags.DrawCollisions) { Flags.DrawCollisions = false; }
                            else { Flags.DrawCollisions = true; }
                            //match the draw collisions boolean for the selected state
                            buttons[counter].selected = Flags.DrawCollisions;
                        }
                        else if (counter == 1) //build the dungeon room again
                        {
                            DungeonGenerator.BuildRoom();
                        }

                        #endregion


                    }
                } //buttons not touching cursor return to button up color
                else { buttons[counter].currentColor = Assets.colorScheme.buttonUp; }
                //selected buttons get the button down color
                if (buttons[counter].selected)
                { buttons[counter].currentColor = Assets.colorScheme.buttonDown; }
            }
        }

        public static void Draw()
        {
            //draw the background rec with correct color
            ScreenManager.spriteBatch.Draw(
                Assets.dummyTexture, rec,
                Assets.colorScheme.windowBkg);
            //loop draw all the buttons
            for (counter = 0; counter < buttons.Count; counter++)
            { DrawFunctions.Draw(buttons[counter]); }
        }

    }
}