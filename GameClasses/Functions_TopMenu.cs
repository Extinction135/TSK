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


            //handle objTools widget input
            if (Widgets.ObjectTools.HandleInput_Widget())
            {
                //if editor clicked inside of objTools widget,
                //do not pass input thru to game world
            }
            else
            {   //if editor did not click inside of objTools widget,
                //then we can pass input to game world
                Widgets.ObjectTools.HandleInput_World();
            }


            #region Editor Keyboard Shortcuts

            if (Functions_Input.IsNewKeyPress(Keys.D0)) //0 right of 9
            {
                Widgets.ObjectTools.SetActiveTool(Widgets.ObjectTools.moveObj);
                TopDebugMenu.objToolState = ObjToolState.MoveObj;
            }
            else if (Functions_Input.IsNewKeyPress(Keys.OemMinus)) //right of 0
            {
                Widgets.ObjectTools.SetActiveTool(Widgets.ObjectTools.deleteObj);
                TopDebugMenu.objToolState = ObjToolState.DeleteObj;
            }
            else if (Functions_Input.IsNewKeyPress(Keys.OemPlus)) //right of -
            {
                Widgets.ObjectTools.SetActiveTool(Widgets.ObjectTools.addObj);
                TopDebugMenu.objToolState = ObjToolState.AddObj;
            }
            else if (Functions_Input.IsNewKeyPress(Keys.D9)) //left of 0
            {
                Widgets.ObjectTools.SetActiveTool(Widgets.ObjectTools.rotateObj);
                TopDebugMenu.objToolState = ObjToolState.RotateObj;
            }
            else if (Functions_Input.IsNewKeyPress(Keys.Back)) //backspace!
            {
                ScreenManager.AddScreen(new ScreenEditorMenu());
            }

            #endregion

            
            
            //dev function buttons

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

            else if (Functions_Input.IsNewKeyPress(Keys.F2))
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

            else if (Functions_Input.IsNewKeyPress(Keys.F3))
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


            #region F4 - Compile Xml to Cs

            else if (Functions_Input.IsNewKeyPress(Keys.F4))
            {
                Functions_Backend.ConvertXMLtoCS();
            }

            #endregion
            

            #region F5 - Toggle Paused flag

            else if (Functions_Input.IsNewKeyPress(Keys.F5))
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
            }
            else
            {
                //if objtools is drawing, update it
                Widgets.ObjectTools.Update();

                //hold down left ctrl button to call Inspect() on anything touching cursor
                if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
                {
                    if (Functions_Input.IsKeyDown(Keys.LeftControl))
                    { Functions_Debug.Inspect(); }
                }
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