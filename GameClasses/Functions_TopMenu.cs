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
                ResetEditorWidgets();
                //set buttons correctly - dungeon = down, world = up
                TopDebugMenu.buttons[5].currentColor = Assets.colorScheme.buttonDown;
                TopDebugMenu.buttons[6].currentColor = Assets.colorScheme.buttonUp;
            }
            else if (Set == WidgetDisplaySet.World)
            {
                TopDebugMenu.display = WidgetDisplaySet.World;
                ResetEditorWidgets();
                //set buttons correctly - dungeon = up, world = down
                TopDebugMenu.buttons[6].currentColor = Assets.colorScheme.buttonDown;
                TopDebugMenu.buttons[5].currentColor = Assets.colorScheme.buttonUp;
            }
        }


        public static void UpdateEditorWidgets()
        {
            Widgets.ObjectTools.Update();
            Widgets.RoomTools.Update();

            Widgets.WidgetObjects_Dungeon.Update();
            Widgets.WidgetObjects_Enemy.Update();

            Widgets.WidgetObjects_Environment.Update();
            Widgets.WidgetObjects_Building.Update();

            Widgets.WidgetObjects_Shared.Update();
        }


        public static void ResetEditorWidgets()
        {
            //editor set
            Widgets.ObjectTools.Reset(16 * 1, 16 * 17 + 8); //bot left
            Widgets.RoomTools.Reset(16 * 33, 16 * 17 + 8); //bot right

            //dungeon set
            Widgets.WidgetObjects_Dungeon.Reset(16 * 1, 16 * 2); //left
            Widgets.WidgetObjects_Enemy.Reset(16 * 34, 16 * 2); //right

            //world set
            Widgets.WidgetObjects_Environment.Reset(16 * 1, 16 * 2); //left
            Widgets.WidgetObjects_Building.Reset(16 * 34, 16 * 2); //right

            //shared
            Widgets.WidgetObjects_Shared.Reset(16 * 7, 16 * 2); //right of left widget
        }


        public static void HandleInput()
        {
            if (Flags.Release) { return; } //kill any dev input in release mode


            #region Set Cursor Sprite's AnimationFrame and Position


            if (TopDebugMenu.objToolState == ObjToolState.MoveObj) //check/set move state
            {   //if moving, show open hand cursor
                TopDebugMenu.cursor.currentFrame = AnimationFrames.Ui_Hand_Open[0];
                //if moving, and dragging, show grab cursor
                if (Functions_Input.IsMouseButtonDown(MouseButtons.LeftButton))
                { TopDebugMenu.cursor.currentFrame = AnimationFrames.Ui_Hand_Grab[0]; }
            }
            else
            {   //default to pointer
                TopDebugMenu.cursor.currentFrame = AnimationFrames.Ui_Hand_Point[0];
                //if clicking/dragging, show pointer press cursor
                if (Functions_Input.IsMouseButtonDown(MouseButtons.LeftButton))
                { TopDebugMenu.cursor.currentFrame = AnimationFrames.Ui_Hand_Press[0]; }
            }

            TopDebugMenu.cursor.position.X = Input.cursorPos.X;
            TopDebugMenu.cursor.position.Y = Input.cursorPos.Y;

            if (TopDebugMenu.objToolState != ObjToolState.MoveObj)
            {   //apply offset for pointer sprite
                TopDebugMenu.cursor.position.X += 3;
                TopDebugMenu.cursor.position.Y += 6;
            }

            #endregion


            UpdateEditorWidgets(); //this animates them


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
            

            #region F4 - Convert XML to CS

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


            #region F6 - Set Dungeon Widgets Display

            if (Functions_Input.IsNewKeyPress(Keys.F6))
            {
                TopDebugMenu.display = WidgetDisplaySet.Dungeon;
                ResetEditorWidgets();
                //set buttons correctly - dungeon = down, world = up
                TopDebugMenu.buttons[5].currentColor = Assets.colorScheme.buttonDown;
                TopDebugMenu.buttons[6].currentColor = Assets.colorScheme.buttonUp;
            }

            #endregion


            #region F7 - Set World Widgets Display

            if (Functions_Input.IsNewKeyPress(Keys.F7))
            {
                TopDebugMenu.display = WidgetDisplaySet.World;
                ResetEditorWidgets();
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


            #region Handle User Clicking TopMenu Buttons

            if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
            {
                //button - level editor
                if (TopDebugMenu.buttons[TopDebugMenu.buttons.Count-1].rec.Contains(Input.cursorPos))
                {   //switch to level/world mode
                    ScreenManager.ExitAndLoad(new ScreenLevelEditor());
                    TopDebugMenu.display = WidgetDisplaySet.World;
                    ResetEditorWidgets();
                    //display the editor state
                    TopDebugMenu.buttons[TopDebugMenu.buttons.Count - 1].currentColor = Assets.colorScheme.buttonDown;
                    TopDebugMenu.buttons[TopDebugMenu.buttons.Count - 2].currentColor = Assets.colorScheme.buttonUp;
                }
                //button - room editor
                else if (TopDebugMenu.buttons[TopDebugMenu.buttons.Count-2].rec.Contains(Input.cursorPos))
                {   //switch to room/dungeon mode
                    ScreenManager.ExitAndLoad(new ScreenRoomEditor());
                    TopDebugMenu.display = WidgetDisplaySet.Dungeon;
                    ResetEditorWidgets();
                    //display the editor state
                    TopDebugMenu.buttons[TopDebugMenu.buttons.Count - 1].currentColor = Assets.colorScheme.buttonUp;
                    TopDebugMenu.buttons[TopDebugMenu.buttons.Count - 2].currentColor = Assets.colorScheme.buttonDown;
                }

                //hold down left ctrl button to call Inspect() on anything touching cursor
                if (Functions_Input.IsKeyDown(Keys.LeftControl)) { Functions_Debug.Inspect(); }
            }

            #endregion






            #region Handle User Clicking Editor Widget Objects

            if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
            {
                //here is the thought process behind why we return; in the code below:

                //if we dont return, then objTools path gets checked, which then may
                //add or delete or rotate an object BEHIND the object widget we just
                //selected an object from. that's unintentional, so we bail from the
                //method before it reaches objTools, if we click on a widget's window

                if (TopDebugMenu.display == WidgetDisplaySet.Dungeon)
                {
                    //Handle Dungeon Objs Widget 
                    if (Widgets.WidgetObjects_Dungeon.window.interior.rec.Contains(Input.cursorPos))
                    {
                        Widgets.ObjectTools.CheckObjList(Widgets.WidgetObjects_Dungeon.objList);
                        return;
                    }

                    //Handle Enemy Spawns Widget
                    if (Widgets.WidgetObjects_Enemy.window.interior.rec.Contains(Input.cursorPos))
                    {
                        Widgets.ObjectTools.CheckObjList(Widgets.WidgetObjects_Enemy.objList);
                        return;
                    }
                }
                else if (TopDebugMenu.display == WidgetDisplaySet.World)
                {
                    //Handle Environment Objs Widget 
                    if (Widgets.WidgetObjects_Environment.window.interior.rec.Contains(Input.cursorPos))
                    {
                        Widgets.ObjectTools.CheckObjList(Widgets.WidgetObjects_Environment.objList);
                        return;
                    }

                    //Handle Building Objs Widget 
                    if (Widgets.WidgetObjects_Building.window.interior.rec.Contains(Input.cursorPos))
                    {
                        Widgets.ObjectTools.CheckObjList(Widgets.WidgetObjects_Building.objList);
                        return;
                    }
                }
                if (TopDebugMenu.displaySharedObjsWidget)
                {
                    //Handle Shared Objs Widget 
                    if (Widgets.WidgetObjects_Shared.window.interior.rec.Contains(Input.cursorPos))
                    {
                        Widgets.ObjectTools.CheckObjList(Widgets.WidgetObjects_Shared.objList);
                        return;
                    }
                }
            }

            #endregion


            #region Pass Input to RoomTools, if it's being displayed

            if (TopDebugMenu.display != WidgetDisplaySet.None)
            {
                Widgets.RoomTools.HandleInput();
            }

            #endregion


            //editor should be able to use cursor in expected manner
            Widgets.ObjectTools.HandleInput();
        }


        public static void Draw()
        {
            if (TopDebugMenu.display != WidgetDisplaySet.None)
            {
                //draw the top bkgRec rec
                ScreenManager.spriteBatch.Draw(
                    Assets.dummyTexture,
                    TopDebugMenu.rec,
                    Assets.colorScheme.debugBkg);
                //loop draw all the buttons
                for (TopDebugMenu.counter = 0;
                    TopDebugMenu.counter < TopDebugMenu.buttons.Count;
                    TopDebugMenu.counter++)
                { Functions_Draw.Draw(TopDebugMenu.buttons[TopDebugMenu.counter]); }
                //if dungeon mode, draw dungeon widgets
                if (TopDebugMenu.display == WidgetDisplaySet.Dungeon)
                {
                    Widgets.WidgetObjects_Dungeon.Draw();
                    Widgets.WidgetObjects_Enemy.Draw();
                }
                //if level mode, draw level widgets
                else if (TopDebugMenu.display == WidgetDisplaySet.World)
                {
                    Widgets.WidgetObjects_Environment.Draw();
                    Widgets.WidgetObjects_Building.Draw();
                }
                //shared objs widget too
                if (TopDebugMenu.displaySharedObjsWidget)
                {
                    Widgets.WidgetObjects_Shared.Draw();
                }

                //draw needed editor widgets
                Widgets.RoomTools.Draw();
                Widgets.ObjectTools.Draw();
            }

            //ALWAYS draw the cursor, and draw it last
            Functions_Draw.Draw(TopDebugMenu.cursor);
            //ALWAYS draw the toolTip too
            if (TopDebugMenu.objToolState != ObjToolState.MoveObj)
            { Functions_Draw.Draw(Widgets.ObjectTools.toolTipSprite); }
        }

    }
}