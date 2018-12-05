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
            widgetDisplaySet_Btn.compText.text = "forest";
            Widgets.WO_Forest.visible = true;
            Widgets.WE_Forest.visible = true;

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
                if (Widgets.WO_Environment.visible) { CheckObjList(Widgets.WO_Environment); }
                if (Widgets.WO_Dungeon.visible) { CheckObjList(Widgets.WO_Dungeon); }

                //unique WOs
                if (Widgets.WO_Town.visible) { CheckObjList(Widgets.WO_Town); }
                if (Widgets.WO_Colliseum.visible) { CheckObjList(Widgets.WO_Colliseum); }
                if (Widgets.WO_Boat_Front.visible) { CheckObjList(Widgets.WO_Boat_Front); }
                if (Widgets.WO_Boat_Back.visible) { CheckObjList(Widgets.WO_Boat_Back); }

                //
                if (Widgets.WO_Forest.visible) { CheckObjList(Widgets.WO_Forest); }
                if (Widgets.WO_Mountain.visible) { CheckObjList(Widgets.WO_Mountain); }
                if (Widgets.WO_Swamp.visible) { CheckObjList(Widgets.WO_Swamp); }
                

                //dev WO
                if (Widgets.WO_DEV.visible) { CheckObjList(Widgets.WO_DEV); }


                //actor widgets
                if (Widgets.WE_Forest.visible) { CheckActList(Widgets.WE_Forest); }
                if (Widgets.WE_Mountain.visible) { CheckActList(Widgets.WE_Mountain); }
                if (Widgets.WE_Swamp.visible) { CheckActList(Widgets.WE_Swamp); }
                //
                if (Widgets.WE_Town.visible) { CheckActList(Widgets.WE_Town); }

            }

            #endregion


            //mouse button click input
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
            if(Functions_Input.IsNewKeyPress(Keys.F1))
            { IterateWidgetSet(); }


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
            if (Widgets.WO_Swamp.visible) { Widgets.WO_Swamp.Update(); Widgets.WO_Swamp.Draw(); }

            if (Widgets.WO_Town.visible) { Widgets.WO_Town.Update(); Widgets.WO_Town.Draw(); }
            if (Widgets.WO_Colliseum.visible) { Widgets.WO_Colliseum.Update(); Widgets.WO_Colliseum.Draw(); }
            if (Widgets.WO_Boat_Front.visible) { Widgets.WO_Boat_Front.Update(); Widgets.WO_Boat_Front.Draw(); }
            if (Widgets.WO_Boat_Back.visible) { Widgets.WO_Boat_Back.Update(); Widgets.WO_Boat_Back.Draw(); }

            if (Widgets.WO_DEV.visible) { Widgets.WO_DEV.Update(); Widgets.WO_DEV.Draw(); }


            //update and draw actor widgets that are visible
            if (Widgets.WE_Forest.visible) { Widgets.WE_Forest.Update(); Widgets.WE_Forest.Draw(); }
            if (Widgets.WE_Mountain.visible) { Widgets.WE_Mountain.Update(); Widgets.WE_Mountain.Draw(); }
            if (Widgets.WE_Swamp.visible) { Widgets.WE_Swamp.Update(); Widgets.WE_Swamp.Draw(); }
            if (Widgets.WE_Town.visible) { Widgets.WE_Town.Update(); Widgets.WE_Town.Draw(); }


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
            widgetDisplaySet_Btn.compText.text = "nothing";

            //set shared widgets to be visible
            Widgets.WO_Environment.visible = true;
            Widgets.WO_Dungeon.visible = true;

            //set all other widgets not visible
            Widgets.WO_Forest.visible = false;
            Widgets.WO_Mountain.visible = false;
            Widgets.WO_Swamp.visible = false;
            Widgets.WO_Town.visible = false;
            Widgets.WO_Colliseum.visible = false;
            Widgets.WO_Boat_Front.visible = false;
            Widgets.WO_Boat_Back.visible = false;

            //set actor widgets not visible
            Widgets.WE_Forest.visible = false;
            Widgets.WE_Mountain.visible = false;
            Widgets.WE_Swamp.visible = false;
            Widgets.WE_Town.visible = false;

            //set dev widget to always be visible
            Widgets.WO_DEV.visible = true;
        }



        public void IterateWidgetSet()
        {


            #region Iterate thru widget objs + widget enemy objs

            if (Widgets.WO_Town.visible)
            {   //TOWN
                ResetWidgets(); //goto forest
                widgetDisplaySet_Btn.compText.text = "forest";
                Widgets.WO_Forest.visible = true;
                Widgets.WE_Forest.visible = true;
            }
            else if (Widgets.WO_Forest.visible)
            {   //FOREST
                ResetWidgets(); //goto mountain
                widgetDisplaySet_Btn.compText.text = "mountain";
                Widgets.WO_Mountain.visible = true;
                Widgets.WE_Mountain.visible = true;
            }
            else if (Widgets.WO_Mountain.visible)
            {   //MOUNTAIN
                ResetWidgets(); //goto swamp
                widgetDisplaySet_Btn.compText.text = "swamp";
                Widgets.WO_Swamp.visible = true;
                Widgets.WE_Swamp.visible = true;
            }
            else if (Widgets.WO_Swamp.visible)
            {   //SWAMP
                ResetWidgets(); //goto coliseum
                widgetDisplaySet_Btn.compText.text = "coliseum";
                Widgets.WO_Colliseum.visible = true;
                //Widgets.WE_Colliseum.visible = true; //not yet
            }
            else if (Widgets.WO_Colliseum.visible)
            {   //COLISEUM
                ResetWidgets(); //goto boat
                widgetDisplaySet_Btn.compText.text = "boat";
                Widgets.WO_Boat_Front.visible = true;
                Widgets.WO_Boat_Back.visible = true;
            }
            else if (Widgets.WO_Boat_Front.visible)
            {   //BOAT
                ResetWidgets(); //goto Town
                widgetDisplaySet_Btn.compText.text = "town";
                Widgets.WO_Town.visible = true;
                Widgets.WE_Town.visible = true;
            }

            #endregion



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


        }



    }
}