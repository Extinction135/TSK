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
    public static class Functions_TopMenu
    {
        //static int i;

        public static void HandleInput()
        {

            #region F1 - Toggle Collision Rec Drawing

            if (Functions_Input.IsNewKeyPress(Keys.F1))
            {   //toggle draw collision boolean
                if (Flags.DrawCollisions)
                {
                    Flags.DrawCollisions = false;
                    TopDebugMenu.buttons[0].currentColor = Assets.colorScheme.buttonUp;
                }
                else
                {
                    Flags.DrawCollisions = true;
                    TopDebugMenu.buttons[0].currentColor = Assets.colorScheme.buttonDown;
                }
            }

            #endregion


            #region F2 - Toggle Drawing of InfoPanel

            if (Functions_Input.IsNewKeyPress(Keys.F2))
            {
                if (Flags.DrawDebugInfo)
                {
                    Flags.DrawDebugInfo = false;
                    TopDebugMenu.buttons[1].currentColor = Assets.colorScheme.buttonUp;
                }
                else
                {
                    Flags.DrawDebugInfo = true;
                    TopDebugMenu.buttons[1].currentColor = Assets.colorScheme.buttonDown;
                }
            }

            #endregion


            #region F3 - Hide/Unhide Widgets

            if (Functions_Input.IsNewKeyPress(Keys.F3))
            {   //set the player's gold to 99
                //PlayerData.current.gold = 99;
                //Assets.Play(Assets.sfxGoldPickup);

                //ScreenManager.ExitAndLoad(new ScreenRoomEditor());

                if (Flags.HideEditorWidgets)
                {
                    Flags.HideEditorWidgets = false;
                    TopDebugMenu.buttons[2].currentColor = Assets.colorScheme.buttonUp;
                }
                else
                {
                    Flags.HideEditorWidgets = true;
                    TopDebugMenu.buttons[2].currentColor = Assets.colorScheme.buttonDown;
                }
            }

            #endregion
            

            #region F4 - ???

            if (Functions_Input.IsNewKeyPress(Keys.F4))
            {   //dump savedata
                //Inspect(PlayerData.current);

                //ScreenManager.ExitAndLoad(new ScreenLevelEditor());
            }

            #endregion
            

            #region F5 - Toggle Paused flag

            if (Functions_Input.IsNewKeyPress(Keys.F5))
            {
                if (Flags.Paused)
                {
                    Flags.Paused = false;
                    TopDebugMenu.buttons[4].currentColor = Assets.colorScheme.buttonUp;
                }
                else
                {
                    Flags.Paused = true;
                    TopDebugMenu.buttons[4].currentColor = Assets.colorScheme.buttonDown;
                }
            }

            #endregion





            #region F6 - Set Dungeon Widgets Display

            if (Functions_Input.IsNewKeyPress(Keys.F6))
            {
                

                //this is an enum
            }

            #endregion


            #region F7 - Set World Widgets Display

            if (Functions_Input.IsNewKeyPress(Keys.F6))
            {
                //this is an enum
            }

            #endregion


            #region F8 - Set SharedObjs Widget Display

            if (Functions_Input.IsNewKeyPress(Keys.F6))
            {
                //this is a boolean
            }

            #endregion






            #region Handle User Clicking TopMenu Buttons

            if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
            {
                //button - level editor
                if (TopDebugMenu.buttons[TopDebugMenu.buttons.Count-1].rec.Contains(Input.cursorPos))
                {
                    //switch to level mode
                    ScreenManager.ExitAndLoad(new ScreenLevelEditor());
                }
                //button - room editor
                else if (TopDebugMenu.buttons[TopDebugMenu.buttons.Count-2].rec.Contains(Input.cursorPos))
                {
                    //switch to room mode
                    ScreenManager.ExitAndLoad(new ScreenRoomEditor());
                }

                //hold down left ctrl button to call Inspect() on anything touching cursor
                if (Functions_Input.IsKeyDown(Keys.LeftControl)) { Functions_Debug.Inspect(); }
            }

            #endregion


            /*
            //dump the states for every active actor if Enter key is pressed
            if (Functions_Input.IsNewKeyPress(Keys.Enter))
            {
                for (Pool.actorCounter = 0; Pool.actorCounter < Pool.actorCount; Pool.actorCounter++)
                {
                    if (Pool.actorPool[Pool.actorCounter].active)
                    { Inspect(Pool.actorPool[Pool.actorCounter]); }
                }
            }
            */
        }

        public static void Draw()
        {
            // draw the background rec with correct color
            ScreenManager.spriteBatch.Draw(Assets.dummyTexture, TopDebugMenu.rec, Assets.colorScheme.debugBkg);
            //loop draw all the buttons
            for (TopDebugMenu.counter = 0; TopDebugMenu.counter < TopDebugMenu.buttons.Count; TopDebugMenu.counter++)
            { Functions_Draw.Draw(TopDebugMenu.buttons[TopDebugMenu.counter]); }
        }

    }
}