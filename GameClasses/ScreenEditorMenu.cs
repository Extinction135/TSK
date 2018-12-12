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

        //ui buttons
        public ComponentButton widgetDisplaySet_Btn;
        public ComponentButton ignoreWaterTiles;
        public ComponentButton ignoreRoofTiles;
        public ComponentButton clearRoofTiles;

        public ComponentButton ignoreBoatTiles;










        public ScreenEditorMenu()
        {
            this.name = "Editor Menu Screen";

            //setup current sheet button
            widgetDisplaySet_Btn = new ComponentButton("---", new Point(16 * 1, 16 + 2));
            widgetDisplaySet_Btn.rec.Width = 16 * 5;
            //initialize to forest state
            ResetWidgets();
            widgetDisplaySet_Btn.compText.text = "interactive objs";

            //setup ignore water tiles button
            ignoreWaterTiles = new ComponentButton("---", new Point(16 * 6 + 8, 16 + 2));
            ignoreWaterTiles.rec.Width = 16 * 5;
            ignoreWaterTiles.compText.text = "ignore water objs";

            //setup ignoreRoofTiles button
            ignoreRoofTiles = new ComponentButton("---", new Point(16 * 12, 16 + 2));
            ignoreRoofTiles.rec.Width = 16 * 5;
            ignoreRoofTiles.compText.text = "ignore roof objs";

            //setup ignoreRoofTiles button
            clearRoofTiles = new ComponentButton("---", new Point(16 * 17 + 8, 16 + 2));
            clearRoofTiles.rec.Width = 16 * 5;
            clearRoofTiles.compText.text = "clear roof objs";
            
            //setup ignoreBoatTiles button
            ignoreBoatTiles = new ComponentButton("---", new Point(16 * 23, 16 + 2));
            ignoreBoatTiles.rec.Width = 16 * 5;
            ignoreBoatTiles.compText.text = "ignore boat objs";
        }

        public override void Open()
        {
            bkgRec.alpha = 0.0f;
            bkgRec.fadeInSpeed = 0.12f;
            bkgRec.maxAlpha = 0.5f;
            bkgRec.fadeState = FadeState.FadeIn;
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
            {   //int widgets
                if (Widgets.WO_Forest.visible)
                {
                    CheckIntObjList(Widgets.WO_Forest);
                    CheckIntObjList(Widgets.WO_Mountain);
                    CheckIntObjList(Widgets.WO_Swamp);
                    CheckIntObjList(Widgets.WO_Lava);
                    CheckIntObjList(Widgets.WO_Cloud);
                    CheckIntObjList(Widgets.WO_Den);
                    CheckIntObjList(Widgets.WO_Shadow);
                }
                else if (Widgets.WO_Environment.visible)
                {
                    CheckIntObjList(Widgets.WO_Environment);
                    CheckIntObjList(Widgets.WO_Water);
                    CheckIntObjList(Widgets.WO_House);
                    CheckIntObjList(Widgets.WO_NPCS);
                    CheckIntObjList(Widgets.WO_Dev1);
                    CheckIntObjList(Widgets.WO_Dev2);
                    CheckIntObjList(Widgets.WO_Dungeon);
                }
                //ind widgets
                else if (Widgets.WD_Forest.visible)
                {
                    CheckIndObjList(Widgets.WD_Forest);
                    CheckIndObjList(Widgets.WD_Mountain);
                    CheckIndObjList(Widgets.WD_Swamp);
                    CheckIndObjList(Widgets.WD_Lava);
                    CheckIndObjList(Widgets.WD_Cloud);
                    CheckIndObjList(Widgets.WD_Den);
                    CheckIndObjList(Widgets.WD_Shadow);
                }
                else if (Widgets.WD_BoatA.visible)
                {
                    CheckIndObjList(Widgets.WD_BoatA);
                    CheckIndObjList(Widgets.WD_BoatB);
                    CheckIndObjList(Widgets.WD_Coliseum);
                    CheckIndObjList(Widgets.WD_Dev1);
                    CheckIndObjList(Widgets.WD_Dev2);
                    CheckIndObjList(Widgets.WD_Dev3);
                    CheckIndObjList(Widgets.WD_Dev4);
                }
                //actor widget
                else if (Widgets.WA_Forest.visible)
                {
                    CheckActList(Widgets.WA_Forest);
                    CheckActList(Widgets.WA_Mountain);
                    CheckActList(Widgets.WA_Swamp);
                    CheckActList(Widgets.WA_Lava);
                    CheckActList(Widgets.WA_Cloud);
                    CheckActList(Widgets.WA_Thievs);
                    CheckActList(Widgets.WA_Shadow);
                }
            }

            #endregion


            #region Button Input Clicks (+keyboard shortcut)

            if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
            {
                //Button - Iterate Widget Display Set 
                if (widgetDisplaySet_Btn.rec.Contains(Input.cursorPos))
                { IterateWidgetSet(); }


                #region Button - Ignore Water Tiles

                else if(ignoreWaterTiles.rec.Contains(Input.cursorPos))
                {
                    if (Flags.IgnoreWaterTiles)
                    {
                        Flags.IgnoreWaterTiles = false;
                        ignoreWaterTiles.currentColor = ColorScheme.buttonUp;
                    }
                    else
                    {
                        Flags.IgnoreWaterTiles = true;
                        ignoreWaterTiles.currentColor = ColorScheme.buttonDown;
                    }
                }

                #endregion


                #region Button - Ignore Roof Tiles

                else if (ignoreRoofTiles.rec.Contains(Input.cursorPos))
                {
                    if (Flags.IgnoreRoofTiles)
                    {
                        Flags.IgnoreRoofTiles = false;
                        ignoreRoofTiles.currentColor = ColorScheme.buttonUp;
                    }
                    else
                    {
                        Flags.IgnoreRoofTiles = true;
                        ignoreRoofTiles.currentColor = ColorScheme.buttonDown;
                        
                    }
                }

                #endregion


                #region Button - Clear Roof Tiles

                else if (clearRoofTiles.rec.Contains(Input.cursorPos))
                {
                    clearRoofTiles.currentColor = ColorScheme.buttonDown;

                    //loop through all roomObjs, releasing any roof type
                    for (int i = 0; i < Pool.intObjCount; i++)
                    {
                        if(
                            Pool.intObjPool[i].type == InteractiveType.House_Roof_Bottom ||
                            Pool.intObjPool[i].type == InteractiveType.House_Roof_Chimney ||
                            Pool.intObjPool[i].type == InteractiveType.House_Roof_Top
                            )
                        {
                            Functions_Pool.Release(Pool.intObjPool[i]);
                        }
                    }

                    //update the gameworld / level roomObjects (so roof disappear in bkg)
                    Functions_Pool.Update(); //fire the main game loop once
                }

                #endregion


                #region Button - Ignore Boat Tiles

                else if (ignoreBoatTiles.rec.Contains(Input.cursorPos))
                {
                    if (Flags.IgnoreBoatTiles)
                    {
                        Flags.IgnoreBoatTiles = false;
                        ignoreBoatTiles.currentColor = ColorScheme.buttonUp;
                    }
                    else
                    {
                        Flags.IgnoreBoatTiles = true;
                        ignoreBoatTiles.currentColor = ColorScheme.buttonDown;

                    }
                }

                #endregion

            }

            //keyboard input - shortcut to iterate thru level sheets
            if(Functions_Input.IsNewKeyPress(Keys.F12))
            { IterateWidgetSet(); }

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

            //update/draw interactive obj widgets
            if (Widgets.WO_Forest.visible)
            {
                Widgets.WO_Forest.Update(); Widgets.WO_Forest.Draw();
                Widgets.WO_Mountain.Update(); Widgets.WO_Mountain.Draw();
                Widgets.WO_Swamp.Update(); Widgets.WO_Swamp.Draw();
                Widgets.WO_Lava.Update(); Widgets.WO_Lava.Draw();
                Widgets.WO_Cloud.Update(); Widgets.WO_Cloud.Draw();
                Widgets.WO_Den.Update(); Widgets.WO_Den.Draw();
                Widgets.WO_Shadow.Update(); Widgets.WO_Shadow.Draw();
            }
            else if(Widgets.WO_Environment.visible)
            {
                Widgets.WO_Environment.Update(); Widgets.WO_Environment.Draw();
                Widgets.WO_Water.Update(); Widgets.WO_Water.Draw();
                Widgets.WO_House.Update(); Widgets.WO_House.Draw();
                Widgets.WO_NPCS.Update(); Widgets.WO_NPCS.Draw();
                Widgets.WO_Dev1.Update(); Widgets.WO_Dev1.Draw();
                Widgets.WO_Dev2.Update(); Widgets.WO_Dev2.Draw();
                Widgets.WO_Dungeon.Update(); Widgets.WO_Dungeon.Draw();
            }



            //update/draw indestructible obj widgets
            else if (Widgets.WD_Forest.visible)
            {
                Widgets.WD_Forest.Update(); Widgets.WD_Forest.Draw();
                Widgets.WD_Mountain.Update(); Widgets.WD_Mountain.Draw();
                Widgets.WD_Swamp.Update(); Widgets.WD_Swamp.Draw();
                Widgets.WD_Lava.Update(); Widgets.WD_Lava.Draw();
                Widgets.WD_Cloud.Update(); Widgets.WD_Cloud.Draw();
                Widgets.WD_Den.Update(); Widgets.WD_Den.Draw();
                Widgets.WD_Shadow.Update(); Widgets.WD_Shadow.Draw();
            }
            else if (Widgets.WD_BoatA.visible)
            {
                Widgets.WD_BoatA.Update(); Widgets.WD_BoatA.Draw();
                Widgets.WD_BoatB.Update(); Widgets.WD_BoatB.Draw();
                Widgets.WD_Coliseum.Update(); Widgets.WD_Coliseum.Draw();
                Widgets.WD_Dev1.Update(); Widgets.WD_Dev1.Draw();
                Widgets.WD_Dev2.Update(); Widgets.WD_Dev2.Draw();
                Widgets.WD_Dev3.Update(); Widgets.WD_Dev3.Draw();
                Widgets.WD_Dev4.Update(); Widgets.WD_Dev4.Draw();
            }

            
            //update/draw actor widgets
            else if (Widgets.WA_Forest.visible)
            {
                Widgets.WA_Forest.Update(); Widgets.WA_Forest.Draw();
                Widgets.WA_Mountain.Update(); Widgets.WA_Mountain.Draw();
                Widgets.WA_Swamp.Update(); Widgets.WA_Swamp.Draw();
                Widgets.WA_Lava.Update(); Widgets.WA_Lava.Draw();
                Widgets.WA_Cloud.Update(); Widgets.WA_Cloud.Draw();
                Widgets.WA_Thievs.Update(); Widgets.WA_Thievs.Draw();
                Widgets.WA_Shadow.Update(); Widgets.WA_Shadow.Draw();
            }

            

            
            //draw tool widgets
            Widgets.RoomTools.Update();
            Widgets.RoomTools.Draw();
            Widgets.ObjectTools.Update();
            Widgets.ObjectTools.Draw();

            //draw additional tools
            Functions_Draw.Draw(widgetDisplaySet_Btn);
            Functions_Draw.Draw(ignoreWaterTiles);
            Functions_Draw.Draw(ignoreRoofTiles);
            Functions_Draw.Draw(clearRoofTiles);
            Functions_Draw.Draw(ignoreBoatTiles);



            ScreenManager.spriteBatch.End();
        }
















        public void ResetWidgets()
        {
            widgetDisplaySet_Btn.compText.text = "interactive objs";
            HideWidgets();
            //set to dungeon int objs state
            Widgets.WO_Forest.visible = true;
            Widgets.WO_Mountain.visible = true;
            Widgets.WO_Swamp.visible = true;
            Widgets.WO_Lava.visible = true;
            Widgets.WO_Cloud.visible = true;
            Widgets.WO_Den.visible = true;
            Widgets.WO_Shadow.visible = true;
            //set obj tools editor state to interactive objs
            Widgets.ObjectTools.editorState = WidgetObjectTools.EditorState.InteractiveObj;
        }

        public void HideWidgets()
        {   //set all widgets visible to false
            Widgets.WO_Forest.visible = false;
            Widgets.WO_Mountain.visible = false;
            Widgets.WO_Swamp.visible = false;
            Widgets.WO_Lava.visible = false;
            Widgets.WO_Cloud.visible = false;
            Widgets.WO_Den.visible = false;
            Widgets.WO_Shadow.visible = false;
            //int set 2
            Widgets.WO_Environment.visible = false;
            Widgets.WO_Water.visible = false;
            Widgets.WO_House.visible = false;
            Widgets.WO_NPCS.visible = false;
            Widgets.WO_Dev1.visible = false;
            Widgets.WO_Dev2.visible = false;
            Widgets.WO_Dungeon.visible = false;

            //set all indestructible obj widgets to false
            Widgets.WD_Forest.visible = false;
            Widgets.WD_Mountain.visible = false;
            Widgets.WD_Swamp.visible = false;
            Widgets.WD_Lava.visible = false;
            Widgets.WD_Cloud.visible = false;
            Widgets.WD_Den.visible = false;
            Widgets.WD_Shadow.visible = false;
            //ind set 2
            Widgets.WD_BoatA.visible = false;
            Widgets.WD_BoatB.visible = false;
            Widgets.WD_Coliseum.visible = false;
            Widgets.WD_Dev1.visible = false;
            Widgets.WD_Dev2.visible = false;
            Widgets.WD_Dev3.visible = false;
            Widgets.WD_Dev4.visible = false;

            //set all actor widgets to false
            Widgets.WA_Forest.visible = false;
            Widgets.WA_Mountain.visible = false;
            Widgets.WA_Swamp.visible = false;
            Widgets.WA_Lava.visible = false;
            Widgets.WA_Cloud.visible = false;
            Widgets.WA_Thievs.visible = false;
            Widgets.WA_Shadow.visible = false;
        }




        public void IterateWidgetSet()
        {

            if(Widgets.WO_Forest.visible)
            {   //goto int set 2
                HideWidgets();
                Widgets.WO_Environment.visible = true;
                Widgets.WO_Water.visible = true;
                Widgets.WO_House.visible = true;
                Widgets.WO_NPCS.visible = true;
                Widgets.WO_Dev1.visible = true;
                Widgets.WO_Dev2.visible = true;
                Widgets.WO_Dungeon.visible = true;
                widgetDisplaySet_Btn.compText.text = "int objs 2";
                Widgets.ObjectTools.editorState = WidgetObjectTools.EditorState.InteractiveObj;
            }
            else if(Widgets.WO_Environment.visible)
            {   //from int set 2 to ind set 1
                HideWidgets();
                Widgets.WD_Forest.visible = true;
                Widgets.WD_Mountain.visible = true;
                Widgets.WD_Swamp.visible = true;
                Widgets.WD_Lava.visible = true;
                Widgets.WD_Cloud.visible = true;
                Widgets.WD_Den.visible = true;
                Widgets.WD_Shadow.visible = true;
                widgetDisplaySet_Btn.compText.text = "ind objs 1";
                Widgets.ObjectTools.editorState = WidgetObjectTools.EditorState.IndestructibleObj;
            }

            else if (Widgets.WD_Forest.visible)
            {   //from ind set 1 to ind set 2
                HideWidgets();
                Widgets.WD_BoatA.visible = true;
                Widgets.WD_BoatB.visible = true;
                Widgets.WD_Coliseum.visible = true;
                Widgets.WD_Dev1.visible = true;
                Widgets.WD_Dev2.visible = true;
                Widgets.WD_Dev3.visible = true;
                Widgets.WD_Dev4.visible = true;
                widgetDisplaySet_Btn.compText.text = "ind objs 2";
                Widgets.ObjectTools.editorState = WidgetObjectTools.EditorState.IndestructibleObj;
            }
            else if(Widgets.WD_BoatA.visible)
            {   //from ind set 2 to actors
                HideWidgets();
                Widgets.WA_Forest.visible = true;
                Widgets.WA_Mountain.visible = true;
                Widgets.WA_Swamp.visible = true;
                Widgets.WA_Lava.visible = true;
                Widgets.WA_Cloud.visible = true;
                Widgets.WA_Thievs.visible = true;
                Widgets.WA_Shadow.visible = true;
                widgetDisplaySet_Btn.compText.text = "actors";
                //set obj tools editor state to actors
                Widgets.ObjectTools.editorState = WidgetObjectTools.EditorState.Actor;
            }
            else if(Widgets.WA_Forest.visible)
            {   //just reset to entry point for this codebranch
                ResetWidgets();
            }






            /*

            #region Iterate Dungeon Textures too

            //disgusting hack: as we iterate thru widgets,
            //iterate thru dungeon textures (should be in own button)
            
            //set the dungeon ID based on the visible widget object
            if (Widgets.WO_Forest.visible)
            { LevelSet.dungeon.ID = LevelID.Forest_Dungeon; }
            else if (Widgets.WO_Mountain.visible)
            { LevelSet.dungeon.ID = LevelID.Mountain_Dungeon; }
            else if (Widgets.WO_Swamp.visible)
            { LevelSet.dungeon.ID = LevelID.Swamp_Dungeon; }
            else //set to non-dungeon id so default texture is displayed
            { LevelSet.dungeon.ID = LevelID.SkullIsland_ShadowKing; }
            
            //actually update the dungeon texture and floors and objs
            Functions_Texture.UpdateDungeonTexture();
            Functions_Texture.SetFloorTextures();
            for (int i = 0; i < Pool.intObjCount; i++)
            { Functions_InteractiveObjs.SetType(Pool.intObjPool[i], Pool.intObjPool[i].type); }

            //in the future, we will need to update the dungeon widget objects texture
            Functions_Texture.SetWOTexture(Widgets.WO_Dungeon);

            //hide obj tools selection box offscreen
            //because it's likely positioned over something different now
            Widgets.ObjectTools.selectionBoxObj.position.X = 2048;

            #endregion

            */
        }







        public void CheckIntObjList(WidgetIntObject WO)
        {
            if (WO.visible & WO.window.interior.rec.Contains(Input.cursorPos))
            { Widgets.ObjectTools.CheckObjList(WO.objList); }
        }

        public void CheckIndObjList(WidgetIndObject WD)
        {
            if (WD.visible & WD.window.interior.rec.Contains(Input.cursorPos))
            { Widgets.ObjectTools.CheckObjList(WD.objList); }
        }

        public void CheckActList(WidgetActor WA)
        {
            if (WA.visible & WA.window.interior.rec.Contains(Input.cursorPos))
            { Widgets.ObjectTools.CheckActorsList(WA.actors); }
        }

    }
}