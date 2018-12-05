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
            {
                if (Widgets.WO_Environment.visible)
                {
                    CheckObjList(Widgets.WO_Environment);
                    CheckObjList(Widgets.WO_Water);
                    CheckObjList(Widgets.WO_House);
                    CheckObjList(Widgets.WO_NPCS);
                    CheckObjList(Widgets.WO_Mtn);
                    CheckObjList(Widgets.WO_Dev);
                    CheckObjList(Widgets.WO_Dungeon);
                }

                if (Widgets.WA_Forest.visible)
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
            if (Widgets.WO_Environment.visible)
            {
                Widgets.WO_Environment.Update(); Widgets.WO_Environment.Draw();
                Widgets.WO_Water.Update(); Widgets.WO_Water.Draw();
                Widgets.WO_House.Update(); Widgets.WO_House.Draw();
                Widgets.WO_NPCS.Update(); Widgets.WO_NPCS.Draw();
                Widgets.WO_Mtn.Update(); Widgets.WO_Mtn.Draw();
                Widgets.WO_Dev.Update(); Widgets.WO_Dev.Draw();
                Widgets.WO_Dungeon.Update(); Widgets.WO_Dungeon.Draw();
            }

            //update/draw actor widgets
            if (Widgets.WA_Forest.visible)
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
            //set interactive object widgets to be visible
            Widgets.WO_Environment.visible = true;
            Widgets.WO_Water.visible = true;
            Widgets.WO_House.visible = true;
            Widgets.WO_NPCS.visible = true;
            Widgets.WO_Mtn.visible = true;
            Widgets.WO_Dev.visible = true;
            Widgets.WO_Dungeon.visible = true;
        }

        public void HideWidgets()
        {   //set all widgets visible to false
            Widgets.WO_Environment.visible = false;
            Widgets.WO_Water.visible = false;
            Widgets.WO_House.visible = false;
            Widgets.WO_NPCS.visible = false;
            Widgets.WO_Mtn.visible = false;
            Widgets.WO_Dev.visible = false;
            Widgets.WO_Dungeon.visible = false;
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

            if(Widgets.WO_Environment.visible)
            {   //from int objs to actors
                HideWidgets();
                //show actor widgets
                Widgets.WA_Forest.visible = true;
                Widgets.WA_Mountain.visible = true;
                Widgets.WA_Swamp.visible = true;
                Widgets.WA_Lava.visible = true;
                Widgets.WA_Cloud.visible = true;
                Widgets.WA_Thievs.visible = true;
                Widgets.WA_Shadow.visible = true;
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







        public void CheckObjList(WidgetObject WO)
        {
            if (WO.visible & WO.window.interior.rec.Contains(Input.cursorPos))
            { Widgets.ObjectTools.CheckObjList(WO.objList); }
        }

        public void CheckActList(WidgetActor WA)
        {
            if (WA.visible & WA.window.interior.rec.Contains(Input.cursorPos))
            { Widgets.ObjectTools.CheckActorsList(WA.actors); }
        }

    }
}