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

using System.IO;




namespace DungeonRun
{
    public static class Functions_TopMenu
    {


        public static void HandleInput()
        {
            if (Flags.Release) { return; } //kill any dev input in release mode
            
            //Backspace - Show Editor Menu
            if (Functions_Input.IsNewKeyPress(Keys.Back))
            {
                ScreenManager.AddScreen(new ScreenEditorMenu());
            }



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
                if (Flags.EnableDebugInfo)
                {
                    Flags.EnableDebugInfo = false;
                    TopDebugMenu.buttons[1].currentColor = Assets.colorScheme.buttonUp;
                }
                else
                {
                    Flags.EnableDebugInfo = true;
                    TopDebugMenu.buttons[1].currentColor = Assets.colorScheme.buttonDown;
                }
            }

            #endregion


            #region F3 - Hide/Unhide Widgets

            if (Functions_Input.IsNewKeyPress(Keys.F3))
            {
                if (TopDebugMenu.hideAll == false)
                {
                    TopDebugMenu.hideAll = true; //enable hide
                }
                else
                {
                    TopDebugMenu.hideAll = false;
                }
            }

            #endregion


            #region F4 - xml to cs

            if (Functions_Input.IsNewKeyPress(Keys.F4))
            {
                Functions_Backend.ConvertXMLtoCS();
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






            if (TopDebugMenu.hideAll)
            {   
                //all widgets are hidden

                //allow editor to use objTools keyboard shortcuts
                //pull these out of roomTools input section
            }
            else
            {
                //widgets are displayed, allow all cursor input

                //allow editor to use cursor tool
                Widgets.ObjectTools.HandleInput();
                Widgets.ObjectTools.Update();



                #region Move Between Level and Rooom Modes

                if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
                {

                    /*
                    //button - level editor
                    if (TopDebugMenu.buttons[TopDebugMenu.buttons.Count - 1].rec.Contains(Input.cursorPos))
                    {   //switch to level/world mode
                        ScreenManager.ExitAndLoad(new ScreenLevelEditor());
                        TopDebugMenu.display = WidgetDisplaySet.World;
                        //ResetEditorWidgets();
                        //display the editor state
                        TopDebugMenu.buttons[TopDebugMenu.buttons.Count - 1].currentColor = Assets.colorScheme.buttonDown;
                        TopDebugMenu.buttons[TopDebugMenu.buttons.Count - 2].currentColor = Assets.colorScheme.buttonUp;
                        return;
                    }
                    //button - room editor
                    else if (TopDebugMenu.buttons[TopDebugMenu.buttons.Count - 2].rec.Contains(Input.cursorPos))
                    {   //switch to room/dungeon mode
                        ScreenManager.ExitAndLoad(new ScreenRoomEditor());
                        TopDebugMenu.display = WidgetDisplaySet.Dungeon;
                        //ResetEditorWidgets();
                        //display the editor state
                        TopDebugMenu.buttons[TopDebugMenu.buttons.Count - 1].currentColor = Assets.colorScheme.buttonUp;
                        TopDebugMenu.buttons[TopDebugMenu.buttons.Count - 2].currentColor = Assets.colorScheme.buttonDown;
                        return;
                    }
                    */




                    //hold down left ctrl button to call Inspect() on anything touching cursor
                    if (Functions_Input.IsKeyDown(Keys.LeftControl)) { Functions_Debug.Inspect(); }
                }

                #endregion


            }

            
        }


        public static void Draw()
        {
            if (TopDebugMenu.hideAll == false)
            {   //draw the top bkgRec rec
                ScreenManager.spriteBatch.Draw(
                    Assets.dummyTexture,
                    TopDebugMenu.rec,
                    Assets.colorScheme.debugBkg);
                //loop draw all the buttons
                for (TopDebugMenu.counter = 0;
                    TopDebugMenu.counter < TopDebugMenu.buttons.Count;
                    TopDebugMenu.counter++)
                { Functions_Draw.Draw(TopDebugMenu.buttons[TopDebugMenu.counter]); }
                //draw the object tools
                Widgets.ObjectTools.Draw();
            }
        }

    }
}