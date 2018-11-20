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
    public class ScreenEditor : ScreenLevel
    {
        public ScreenEditor() { this.name = "Editor Screen"; }

        public override void Open()
        {
            //based on the boot routine, setup the editor differently
            if(Flags.bootRoutine == BootRoutine.Editor_Level)
            {
                LevelSet.currentLevel = LevelSet.field;
                LevelSet.field.ID = LevelID.DEV_Field;
                
                Widgets.RoomTools.roomData = new RoomXmlData();
                Widgets.RoomTools.roomData.type = RoomID.DEV_Field;
                base.Open();

                Widgets.RoomTools.SetState(WidgetRoomToolsState.Level);
                //fields auto-teleport hero to field spawn point
            }
            else
            {
                LevelSet.currentLevel = LevelSet.dungeon;
                LevelSet.dungeon.ID = LevelID.Forest_Dungeon;

                Widgets.RoomTools.roomData = new RoomXmlData();
                Widgets.RoomTools.roomData.type = RoomID.DEV_Row;
                base.Open();

                Widgets.RoomTools.SetState(WidgetRoomToolsState.Room);
                //teleport hero to outside of room at top left corner
                Functions_Movement.Teleport(Pool.hero.compMove,
                    Functions_Level.buildPosition.X - 32,
                    Functions_Level.buildPosition.Y + 32);
            }

            //position the roomTools widget
            Widgets.RoomTools.Reset(16 * 33, 16 * 17 + 8); //bottom right

            //setup the screen
            overlay.alpha = 0.0f;
            displayState = DisplayState.Opened; //open the screen
            Flags.Paused = false; //unpause editor initially
        }

        public override void HandleInput(GameTime GameTime)
        {
            base.HandleInput(GameTime);
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
                ScreenManager.AddScreen(Screens.EditorMenu);
            }

            #endregion



            //dev function buttons

            #region F1 - Toggle Collision Rec Drawing

            if (Functions_Input.IsNewKeyPress(Keys.F1))
            {   //toggle draw collision boolean
                if (Flags.DrawCollisions)
                {
                    Flags.DrawCollisions = false;
                    TopDebugMenu.buttons[1].currentColor = ColorScheme.buttonUp;
                }
                else
                {
                    Flags.DrawCollisions = true;
                    TopDebugMenu.buttons[1].currentColor = ColorScheme.buttonDown;
                }
            }

            #endregion


            #region F2 - Toggle Drawing of InfoPanel

            else if (Functions_Input.IsNewKeyPress(Keys.F2))
            {
                if (Flags.EnableDebugInfo)
                {
                    Flags.EnableDebugInfo = false;
                    TopDebugMenu.buttons[2].currentColor = ColorScheme.buttonUp;
                }
                else
                {
                    Flags.EnableDebugInfo = true;
                    TopDebugMenu.buttons[2].currentColor = ColorScheme.buttonDown;
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
                TopDebugMenu.buttons[4].currentColor = ColorScheme.buttonDown;
                Functions_Backend.ConvertXMLtoCS();
            }

            #endregion


            #region F5 - Toggle Paused flag

            else if (Functions_Input.IsNewKeyPress(Keys.F5))
            {
                if (Flags.Paused)
                {
                    Flags.Paused = false;
                    TopDebugMenu.buttons[5].currentColor = ColorScheme.buttonUp;
                }
                else
                {
                    Flags.Paused = true;
                    TopDebugMenu.buttons[5].currentColor = ColorScheme.buttonDown;
                }
            }

            #endregion



            #region F6 - Increment Hero Type

            else if (Functions_Input.IsNewKeyPress(Keys.F6))
            {
                //step through all actor types defined in actorType enum
                if (Pool.hero.type == ActorType.Hero)
                { Functions_Actor.Transform(Pool.hero, ActorType.Blob); }

                //standards
                else if (Pool.hero.type == ActorType.Blob)
                { Functions_Actor.Transform(Pool.hero, ActorType.Standard_AngryEye); }
                else if (Pool.hero.type == ActorType.Standard_AngryEye)
                { Functions_Actor.Transform(Pool.hero, ActorType.Standard_BeefyBat); }
                else if (Pool.hero.type == ActorType.Standard_BeefyBat)
                { Functions_Actor.Transform(Pool.hero, ActorType.MiniBoss_BlackEye); }

                //minibosses
                else if (Pool.hero.type == ActorType.MiniBoss_BlackEye)
                { Functions_Actor.Transform(Pool.hero, ActorType.MiniBoss_Spider_Armored); }
                else if (Pool.hero.type == ActorType.MiniBoss_Spider_Armored)
                { Functions_Actor.Transform(Pool.hero, ActorType.MiniBoss_Spider_Unarmored); }
                else if (Pool.hero.type == ActorType.MiniBoss_Spider_Unarmored)
                { Functions_Actor.Transform(Pool.hero, ActorType.MiniBoss_OctoMouth); }
                else if (Pool.hero.type == ActorType.MiniBoss_OctoMouth)
                { Functions_Actor.Transform(Pool.hero, ActorType.Boss_BigEye); }

                //bosses
                else if (Pool.hero.type == ActorType.Boss_BigEye)
                { Functions_Actor.Transform(Pool.hero, ActorType.Boss_BigBat); }
                else if (Pool.hero.type == ActorType.Boss_BigBat)
                { Functions_Actor.Transform(Pool.hero, ActorType.Boss_OctoHead); }
                else if (Pool.hero.type == ActorType.Boss_OctoHead)
                { Functions_Actor.Transform(Pool.hero, ActorType.Special_Tentacle); }

                //specials
                else if (Pool.hero.type == ActorType.Special_Tentacle)
                { Functions_Actor.Transform(Pool.hero, ActorType.Hero); }
            }            

            #endregion






            #region Inspect Ctrl-Clicked Obj

            //hold down left ctrl button to call Inspect() on anything touching cursor
            if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
            {
                if (Functions_Input.IsKeyDown(Keys.LeftControl))
                { Functions_Debug.Inspect(); }
            }

            #endregion

        }

        public override void Update(GameTime GameTime)
        {
            base.Update(GameTime);
            Widgets.ObjectTools.Update();
        }

        public override void Draw(GameTime GameTime)
        {
            base.Draw(GameTime);




            /*
            #region Draw from Camera View
            ScreenManager.spriteBatch.Begin(
                        SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend,
                        SamplerState.PointClamp,
                        null, null, null, Camera2D.view);

            //things you need to draw from camera view: ?

            ScreenManager.spriteBatch.End();

            #endregion
            */






            #region Draw the TOP MENU + Buttons

            if (TopDebugMenu.hideAll == false)
            {   //open the spritebatch
                ScreenManager.spriteBatch.Begin(
                    SpriteSortMode.Deferred, 
                    BlendState.AlphaBlend, 
                    SamplerState.PointClamp);
                //draw the top bkgRec rec
                ScreenManager.spriteBatch.Draw(
                    Assets.dummyTexture,
                    TopDebugMenu.rec,
                    ColorScheme.debugBkg);
                //loop draw all the buttons
                for (TopDebugMenu.counter = 0;
                    TopDebugMenu.counter < TopDebugMenu.buttons.Count;
                    TopDebugMenu.counter++)
                { Functions_Draw.Draw(TopDebugMenu.buttons[TopDebugMenu.counter]); }
                
                //draw the object tools
                Widgets.ObjectTools.Draw();

                //close up the spritebatch
                ScreenManager.spriteBatch.End();
            }

            #endregion


        }

    }
}