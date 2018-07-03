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

            //initialize to forest state
            ResetWidgets();
            currentSheet.compText.text = "forest";
            Widgets.WO_Forest.visible = true;
            Widgets.WE_Forest.visible = true;
            Level.ID = LevelID.Forest_Entrance;
        }

        public override void HandleInput(GameTime GameTime)
        {

            #region Exit This Screen

            if (Functions_Input.IsNewKeyPress(Keys.Back))
            {
                ScreenManager.RemoveScreen(this);
            }

            #endregion


            #region Handle obj/actor selection for widgets

            if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
            {
                if (Widgets.WO_Environment.visible) { CheckObjList(Widgets.WO_Environment); }
                if (Widgets.WO_Dungeon.visible) { CheckObjList(Widgets.WO_Dungeon); }

                //unique WOs
                if (Widgets.WO_Town.visible) { CheckObjList(Widgets.WO_Town); }
                if (Widgets.WO_Forest.visible) { CheckObjList(Widgets.WO_Forest); }
                if (Widgets.WO_Mountain.visible) { CheckObjList(Widgets.WO_Mountain); }
                if (Widgets.WO_Colliseum.visible) { CheckObjList(Widgets.WO_Colliseum); }

                //dev WO
                if (Widgets.WO_DEV.visible) { CheckObjList(Widgets.WO_DEV); }


                //actor widgets
                if (Widgets.WE_Forest.visible) { CheckActList(Widgets.WE_Forest); }
                if (Widgets.WE_Mountain.visible) { CheckActList(Widgets.WE_Mountain); }
                if (Widgets.WE_Town.visible) { CheckActList(Widgets.WE_Town); }

            }

            #endregion


            #region Handle Level Button (currentSheet) Presses

            if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
            {
                if (currentSheet.rec.Contains(Input.cursorPos))
                {
                    ResetWidgets();



                    //Iterate thru forest, town, colliseum, mountain, etc...
                    if (Level.ID == LevelID.LeftTown2)
                    {   //leads to colliseum
                        currentSheet.compText.text = "colliseum";
                        Widgets.WO_Colliseum.visible = true;
                        Level.ID = LevelID.Colliseum;
                    }
                    else if(Level.ID == LevelID.Colliseum)
                    {   //leads to forest
                        currentSheet.compText.text = "forest";
                        Widgets.WO_Forest.visible = true;
                        Widgets.WE_Forest.visible = true;
                        Level.ID = LevelID.Forest_Entrance;
                    }
                    else if (Level.ID == LevelID.Forest_Entrance)
                    {   //leads to mountain
                        currentSheet.compText.text = "mountain";
                        Widgets.WO_Mountain.visible = true;
                        Widgets.WE_Mountain.visible = true;
                        Level.ID = LevelID.Mountain_Entrance;
                    }
                    else if (Level.ID == LevelID.Mountain_Entrance)
                    {   //leads to town
                        currentSheet.compText.text = "town";
                        Widgets.WO_Town.visible = true;
                        Widgets.WE_Town.visible = true;
                        Level.ID = LevelID.LeftTown2;
                    }

                    else
                    {   //any other case resets button's sequence, leads to forest
                        currentSheet.compText.text = "forest";
                        Widgets.WO_Forest.visible = true;
                        Level.ID = LevelID.Forest_Entrance;
                    }
                    


                    //update level floors and room objects
                    Functions_Texture.SetFloorTextures();
                    for(int i = 0; i < Pool.roomObjCount; i++)
                    { Functions_GameObject.SetType(Pool.roomObjPool[i], Pool.roomObjPool[i].type); }
                    //update all shared widget objects
                    Functions_Texture.SetWOTexture(Widgets.WO_Dungeon);
                    Functions_Texture.SetWOTexture(Widgets.WO_Environment);

                    //hide obj tools selection box offscreen
                    //because it's likely positioned over something different now
                    Widgets.ObjectTools.selectionBoxObj.position.X = 2048;
                }
            }

            #endregion


            //pass tool input
            Widgets.RoomTools.HandleInput();
            Widgets.ObjectTools.HandleInput_Widget();
            //dont pass input to ObjectTools.HandleInput_World();
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
            if (Widgets.WO_Environment.visible) { Widgets.WO_Environment.Update(); Widgets.WO_Environment.Draw(); }
            if (Widgets.WO_Dungeon.visible) { Widgets.WO_Dungeon.Update(); Widgets.WO_Dungeon.Draw(); }

            if (Widgets.WO_Forest.visible) { Widgets.WO_Forest.Update(); Widgets.WO_Forest.Draw(); }
            if (Widgets.WO_Mountain.visible) { Widgets.WO_Mountain.Update(); Widgets.WO_Mountain.Draw(); }

            if (Widgets.WO_Town.visible) { Widgets.WO_Town.Update(); Widgets.WO_Town.Draw(); }
            if (Widgets.WO_Colliseum.visible) { Widgets.WO_Colliseum.Update(); Widgets.WO_Colliseum.Draw(); }

            if (Widgets.WO_DEV.visible) { Widgets.WO_DEV.Update(); Widgets.WO_DEV.Draw(); }


            //update and draw actor widgets that are visible
            if (Widgets.WE_Forest.visible) { Widgets.WE_Forest.Update(); Widgets.WE_Forest.Draw(); }
            if (Widgets.WE_Mountain.visible) { Widgets.WE_Mountain.Update(); Widgets.WE_Mountain.Draw(); }
            if (Widgets.WE_Town.visible) { Widgets.WE_Town.Update(); Widgets.WE_Town.Draw(); }


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

        public void CheckActList(WidgetActor WE)
        {
            if (WE.visible & WE.window.interior.rec.Contains(Input.cursorPos))
            { Widgets.ObjectTools.CheckActorsList(WE.actors); }
        }


        public void ResetWidgets()
        {
            currentSheet.compText.text = "nothing";

            //set shared widgets to be visible
            Widgets.WO_Environment.visible = true;
            Widgets.WO_Dungeon.visible = true;

            //set all other widgets not visible
            Widgets.WO_Forest.visible = false;
            Widgets.WO_Mountain.visible = false;
            Widgets.WO_Town.visible = false;
            Widgets.WO_Colliseum.visible = false;
            
            //set actor widgets not visible
            Widgets.WE_Forest.visible = false;
            Widgets.WE_Mountain.visible = false;
            Widgets.WE_Town.visible = false;

            //set dev widget to always be visible
            Widgets.WO_DEV.visible = true;
        }


    }
}