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
        static WidgetDisplaySet prevSet; //allows us to 'unhide' widgets with f3



        public static void DisplayWidgets(WidgetDisplaySet Set)
        {
            if (Set == WidgetDisplaySet.Dungeon)
            {
                TopDebugMenu.display = WidgetDisplaySet.Dungeon;
                //set buttons correctly - dungeon = down, world = up
                TopDebugMenu.buttons[5].currentColor = Assets.colorScheme.buttonDown;
                TopDebugMenu.buttons[6].currentColor = Assets.colorScheme.buttonUp;
            }
            else if (Set == WidgetDisplaySet.World)
            {
                TopDebugMenu.display = WidgetDisplaySet.World;
                //set buttons correctly - dungeon = up, world = down
                TopDebugMenu.buttons[6].currentColor = Assets.colorScheme.buttonDown;
                TopDebugMenu.buttons[5].currentColor = Assets.colorScheme.buttonUp;
            }
        }

        



        




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
                if (TopDebugMenu.display != WidgetDisplaySet.None)
                {   
                    //grab the 'previous display set' reference
                    prevSet = TopDebugMenu.display; 
                    //set the 'current display set' to none
                    TopDebugMenu.display = WidgetDisplaySet.None;
                    //set down state
                    TopDebugMenu.buttons[2].currentColor = Assets.colorScheme.buttonDown;
                }
                else
                {
                    //set 'current display set' to previous value
                    TopDebugMenu.display = prevSet;
                    //set up state
                    TopDebugMenu.buttons[2].currentColor = Assets.colorScheme.buttonUp;
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



            //"playtest" mode
            if (TopDebugMenu.display == WidgetDisplaySet.None)
            {   //allow editor to use cursor tool
                Widgets.ObjectTools.HandleInput();
            }

            //"edit" mode
            else
            {

                #region F6 - Set Dungeon Widgets Display

                if (Functions_Input.IsNewKeyPress(Keys.F6))
                {
                    TopDebugMenu.display = WidgetDisplaySet.Dungeon;
                    //ResetEditorWidgets();
                    //set buttons correctly - dungeon = down, world = up
                    TopDebugMenu.buttons[5].currentColor = Assets.colorScheme.buttonDown;
                    TopDebugMenu.buttons[6].currentColor = Assets.colorScheme.buttonUp;
                }

                #endregion


                #region F7 - Set World Widgets Display

                if (Functions_Input.IsNewKeyPress(Keys.F7))
                {
                    TopDebugMenu.display = WidgetDisplaySet.World;
                    //ResetEditorWidgets();
                    //set buttons correctly - dungeon = up, world = down
                    TopDebugMenu.buttons[6].currentColor = Assets.colorScheme.buttonDown;
                    TopDebugMenu.buttons[5].currentColor = Assets.colorScheme.buttonUp;
                }

                #endregion


                #region F8 - Set SharedObjs Widget Display

                if (Functions_Input.IsNewKeyPress(Keys.F8))
                {
                    if (TopDebugMenu.displaySharedObjsWidget)
                    {   //set released state
                        TopDebugMenu.displaySharedObjsWidget = false;
                        TopDebugMenu.buttons[7].currentColor = Assets.colorScheme.buttonUp;
                    }
                    else
                    {   //set down state
                        TopDebugMenu.displaySharedObjsWidget = true;
                        TopDebugMenu.buttons[7].currentColor = Assets.colorScheme.buttonDown;
                    }
                }

                #endregion


                if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
                {
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

                    //hold down left ctrl button to call Inspect() on anything touching cursor
                    if (Functions_Input.IsKeyDown(Keys.LeftControl)) { Functions_Debug.Inspect(); }
                }





                if(TopDebugMenu.display != WidgetDisplaySet.None)
                {   //allow editor to use cursor tool
                    Widgets.ObjectTools.HandleInput();
                    Widgets.ObjectTools.Update();
                }
                




            }
        }


        public static void Draw()
        {
            if (TopDebugMenu.display != WidgetDisplaySet.None)
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