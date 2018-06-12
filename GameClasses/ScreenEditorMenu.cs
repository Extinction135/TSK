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
        public ScreenRec bkgRec = new ScreenRec();
        public ComponentButton currentSheet;



        public ScreenEditorMenu() { this.name = "Editor Menu Screen"; }

        public override void LoadContent()
        {
            bkgRec.alpha = 0.0f;
            bkgRec.fadeInSpeed = 0.12f;
            bkgRec.maxAlpha = 0.5f;
            bkgRec.fadeState = FadeState.FadeIn;

            currentSheet = new ComponentButton("---", new Point(16, 16 + 2));
            currentSheet.rec.Width = 16 * 5;

            //initialize to forest/standard state
            currentSheet.compText.text = "forest";
            Level.ID = LevelID.DEV_Field;
            Widgets.WO_Enemy_Forest.visible = true;
            Widgets.WO_Shared_Forest.visible = true;
            Widgets.WO_Building_Forest.visible = true;

            Widgets.WO_Environment.visible = true;
            Widgets.WO_Dungeon.visible = true;
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
                CheckObjList(Widgets.WO_Environment);
                CheckObjList(Widgets.WO_Dungeon);

                //check forest widget input
                if (Widgets.WO_Enemy_Forest.visible)
                { CheckObjList(Widgets.WO_Enemy_Forest); }
                if (Widgets.WO_Shared_Forest.visible)
                { CheckObjList(Widgets.WO_Shared_Forest); }
                if (Widgets.WO_Building_Forest.visible)
                { CheckObjList(Widgets.WO_Building_Forest); }

                //check colliseum widget input
                if (Widgets.WO_Building_Colliseum.visible)
                { CheckObjList(Widgets.WO_Building_Colliseum); }
            }

            //pass tool input
            Widgets.RoomTools.HandleInput();
            Widgets.ObjectTools.HandleInput_Widget();
            //dont pass input to ObjectTools.HandleInput_World();




            if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
            {
                if (currentSheet.rec.Contains(Input.cursorPos))
                {

                    #region Reset custom obj widgets

                    Widgets.WO_Enemy_Forest.visible = false;
                    Widgets.WO_Shared_Forest.visible = false;
                    Widgets.WO_Building_Forest.visible = false;

                    Widgets.WO_Building_Colliseum.visible = false;

                    #endregion


                    #region Setup custom widgets and level.id

                    if (Level.ID == LevelID.DEV_Field) //initial case
                    {
                        Level.ID = LevelID.Colliseum;
                        currentSheet.compText.text = "colliseum";
                        Widgets.WO_Building_Colliseum.visible = true;
                    }
                    else if(Level.ID == LevelID.Colliseum)
                    {
                        Level.ID = LevelID.DEV_Field;
                        currentSheet.compText.text = "forest";
                        Widgets.WO_Enemy_Forest.visible = true;
                        Widgets.WO_Shared_Forest.visible = true;
                        Widgets.WO_Building_Forest.visible = true;
                    }
                    else
                    {   //default case
                        Level.ID = LevelID.DEV_Field;
                        currentSheet.compText.text = "forest";
                        Widgets.WO_Enemy_Forest.visible = true;
                        Widgets.WO_Shared_Forest.visible = true;
                        Widgets.WO_Building_Forest.visible = true;
                    }

                    #endregion


                    //update floors and objects
                    Functions_Texture.SetFloorTextures();
                    for(int i = 0; i < Pool.roomObjCount; i++)
                    {
                        Functions_GameObject.SetType(Pool.roomObjPool[i], Pool.roomObjPool[i].type);
                    }
                    //update all shared widget objects
                    Functions_Texture.SetWOTexture(Widgets.WO_Dungeon);
                    Functions_Texture.SetWOTexture(Widgets.WO_Environment);
                }
            }
        }

        public override void Update(GameTime GameTime)
        {
            //fade overlay in
            Functions_ScreenRec.Fade(bkgRec);
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            Functions_Draw.Draw(bkgRec);

            //update and draw obj widgets that are visible
            if (Widgets.WO_Environment.visible)
            { Widgets.WO_Environment.Update(); Widgets.WO_Environment.Draw(); }

            if (Widgets.WO_Dungeon.visible)
            { Widgets.WO_Dungeon.Update(); Widgets.WO_Dungeon.Draw(); }

            if (Widgets.WO_Enemy_Forest.visible)
            { Widgets.WO_Enemy_Forest.Update(); Widgets.WO_Enemy_Forest.Draw(); }

            if (Widgets.WO_Building_Forest.visible)
            { Widgets.WO_Building_Forest.Update(); Widgets.WO_Building_Forest.Draw(); }

            if (Widgets.WO_Shared_Forest.visible)
            { Widgets.WO_Shared_Forest.Update(); Widgets.WO_Shared_Forest.Draw(); }

            if (Widgets.WO_Building_Colliseum.visible)
            { Widgets.WO_Building_Colliseum.Update(); Widgets.WO_Building_Colliseum.Draw(); }


            //draw tool widgets
            Widgets.RoomTools.Update();
            Widgets.RoomTools.Draw();
            Widgets.ObjectTools.Update();
            Widgets.ObjectTools.Draw();

            //draw additional tools
            Functions_Draw.Draw(currentSheet);

            ScreenManager.spriteBatch.End();
        }



        public void CheckObjList(WidgetObject WO)
        {
            if (WO.visible & WO.window.interior.rec.Contains(Input.cursorPos))
            { Widgets.ObjectTools.CheckObjList(WO.objList); }
        }




    }
}