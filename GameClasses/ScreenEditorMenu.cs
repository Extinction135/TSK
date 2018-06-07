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
    public class ScreenEditorMenu : Screen
    {
        public ScreenEditorMenu() { this.name = "Editor Menu Screen"; }

        public override void LoadContent()
        {
            //position obj widgets
            Widgets.WidgetObjects_Dungeon.Reset(16 * 1, 16 * 2);
            Widgets.WidgetObjects_Enemy.Reset(16 * 6 + 8, 16 * 2);
            Widgets.WidgetObjects_Shared.Reset(16 * 12, 16 * 2);
            Widgets.WidgetObjects_Environment.Reset(16 * 17 + 8, 16 * 2); //left
            Widgets.WidgetObjects_Building.Reset(16 * 23, 16 * 2); //right

            //position tools widgets
            Widgets.ObjectTools.Reset(16 * 1, 16 * 17 + 8); //bot left
            Widgets.RoomTools.Reset(16 * 33, 16 * 17 + 8); //bot right
        }

        public override void HandleInput(GameTime GameTime)
        {

            if (Functions_Input.IsNewKeyPress(Keys.Back))
            {
                ScreenManager.RemoveScreen(this);
            }


            //handle obj selection for obj widgets
            if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
            {
                if (Widgets.WidgetObjects_Dungeon.window.interior.rec.Contains(Input.cursorPos))
                {
                    Widgets.ObjectTools.CheckObjList(Widgets.WidgetObjects_Dungeon.objList);
                }
                if (Widgets.WidgetObjects_Enemy.window.interior.rec.Contains(Input.cursorPos))
                {
                    Widgets.ObjectTools.CheckObjList(Widgets.WidgetObjects_Enemy.objList);
                } 
                if (Widgets.WidgetObjects_Environment.window.interior.rec.Contains(Input.cursorPos))
                {
                    Widgets.ObjectTools.CheckObjList(Widgets.WidgetObjects_Environment.objList);
                }
                if (Widgets.WidgetObjects_Building.window.interior.rec.Contains(Input.cursorPos))
                {
                    Widgets.ObjectTools.CheckObjList(Widgets.WidgetObjects_Building.objList);
                }
                if (Widgets.WidgetObjects_Shared.window.interior.rec.Contains(Input.cursorPos))
                {
                    Widgets.ObjectTools.CheckObjList(Widgets.WidgetObjects_Shared.objList);
                }
            }
            //pass tool input
            Widgets.RoomTools.HandleInput();
            Widgets.ObjectTools.HandleInput();
        }

        public override void Update(GameTime GameTime)
        {
            Widgets.ObjectTools.Update();
            Widgets.RoomTools.Update();

            Widgets.WidgetObjects_Dungeon.Update();
            Widgets.WidgetObjects_Enemy.Update();
            Widgets.WidgetObjects_Environment.Update();
            Widgets.WidgetObjects_Building.Update();
            Widgets.WidgetObjects_Shared.Update();
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            //draw obj widgets
            Widgets.WidgetObjects_Dungeon.Draw();
            Widgets.WidgetObjects_Enemy.Draw();
            Widgets.WidgetObjects_Environment.Draw();
            Widgets.WidgetObjects_Building.Draw();
            Widgets.WidgetObjects_Shared.Draw();

            //draw tool widgets
            Widgets.RoomTools.Draw();
            Widgets.ObjectTools.Draw();

            ScreenManager.spriteBatch.End();
        }
    }
}