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

        public static void ResetEditorWidgets()
        {
            //editor set
            Widgets.ObjectTools.Reset(16 * 1, 16 * 17 + 8); //bot left
            Widgets.RoomTools.Reset(16 * 33, 16 * 17 + 8); //bot right

            //dungeon set
            Widgets.WidgetObjects_Dungeon.Reset(16 * 1, 16 * 2); //left

            //world set
            Widgets.WidgetObjects_Environment.Reset(16 * 1, 16 * 2); //left
            Widgets.WidgetObjects_Building.Reset(16 * 34, 16 * 2); //right
        }


        public static void HandleInput()
        {

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
                TopDebugMenu.display = WidgetDisplaySet.Dungeon;
            }

            #endregion


            #region F7 - Set World Widgets Display

            if (Functions_Input.IsNewKeyPress(Keys.F6))
            {
                TopDebugMenu.display = WidgetDisplaySet.World;
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
                {   //switch to level/world mode
                    ScreenManager.ExitAndLoad(new ScreenLevelEditor());
                    TopDebugMenu.display = WidgetDisplaySet.World;
                    ResetEditorWidgets();
                }
                //button - room editor
                else if (TopDebugMenu.buttons[TopDebugMenu.buttons.Count-2].rec.Contains(Input.cursorPos))
                {   //switch to room/dungeon mode
                    ScreenManager.ExitAndLoad(new ScreenRoomEditor());
                    TopDebugMenu.display = WidgetDisplaySet.Dungeon;
                    ResetEditorWidgets();
                }

                //hold down left ctrl button to call Inspect() on anything touching cursor
                if (Functions_Input.IsKeyDown(Keys.LeftControl)) { Functions_Debug.Inspect(); }
            }

            #endregion


            #region Handle User Clicking Editor Widget Objects

            //handle selecting obj from editor's widgets
            if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
            {
                if (TopDebugMenu.display == WidgetDisplaySet.Dungeon)
                {
                    //Handle Dungeon Objs Widget 
                    if (Widgets.WidgetObjects_Dungeon.window.interior.rec.Contains(Input.cursorPos))
                    { Widgets.ObjectTools.CheckObjList(Widgets.WidgetObjects_Dungeon.objList); }
                    //Handle Enemy Spawns Widget
                }
                else if (TopDebugMenu.display == WidgetDisplaySet.World)
                {
                    //Handle Environment Objs Widget 
                    if (Widgets.WidgetObjects_Environment.window.interior.rec.Contains(Input.cursorPos))
                    { Widgets.ObjectTools.CheckObjList(Widgets.WidgetObjects_Environment.objList); }

                    //Handle Building Objs Widget 
                    if (Widgets.WidgetObjects_Building.window.interior.rec.Contains(Input.cursorPos))
                    { Widgets.ObjectTools.CheckObjList(Widgets.WidgetObjects_Building.objList); }
                }
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
            //draw the background rec with correct color
            ScreenManager.spriteBatch.Draw(Assets.dummyTexture, TopDebugMenu.rec, Assets.colorScheme.debugBkg);
            
            //loop draw all the buttons
            for (TopDebugMenu.counter = 0; TopDebugMenu.counter < TopDebugMenu.buttons.Count; TopDebugMenu.counter++)
            { Functions_Draw.Draw(TopDebugMenu.buttons[TopDebugMenu.counter]); }

            //draw all editor widgets that TopMenu is responsible for, if we are drawing widets
            if (!Flags.HideEditorWidgets)
            {
                //if dungeon mode, draw dungeon widgets
                if (TopDebugMenu.display == WidgetDisplaySet.Dungeon)
                {
                    Widgets.WidgetObjects_Dungeon.Draw();
                    //enemy spawn objs widget here later
                }
                //if level mode, draw level widgets
                else if (TopDebugMenu.display == WidgetDisplaySet.World)
                {
                    Widgets.WidgetObjects_Environment.Draw();
                    Widgets.WidgetObjects_Building.Draw();
                }
                //shared objs widget here
            }

            //ALWAYS draw the cursor, and draw it last
            Functions_Draw.Draw(TopDebugMenu.cursor);
        }


    }
}