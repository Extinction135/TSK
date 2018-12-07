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
    public class WidgetObjectTools : Widget
    {
        //handles the selection, grabbing, dragging, releasing, adding, and deleting of objs + actors
        int j;


        Boolean objMode = true; //start widget in object mode (rather than actor mode)
        public ActorType selectedActor;

        public InteractiveObject moveObj;
        public InteractiveObject rotateObj;
        public InteractiveObject addObj;
        public InteractiveObject deleteObj;

        //move these into Input class or Functions_Input
        public Point worldPos; //used to translate screen to world position
        public Point screenPos; //used to translate world to screen position

        public InteractiveObject currentObjRef;
        public ComponentText currentObjDirectionText;

        public ComponentSprite selectionBoxObj; //highlites the currently selected obj
        public ComponentSprite selectionBoxTool; //highlites the currently selected tool

        public InteractiveObject activeObj; //points to Obj on objList OR on roomObj/entity list
        public InteractiveObject grabbedObj; //obj/entity that is picked up/dragged/dropped in room
        public InteractiveObject activeTool; //points to a ToolObj on the obj list

        Boolean ignoreObj = false; //helper for modeling editor ignore state












        public enum EditorState { InteractiveObj, IndestructibleObj, Actor }
        public EditorState editorState = EditorState.InteractiveObj;








        public WidgetObjectTools()
        {
            window = new MenuWindow(
                new Point(0, 0),
                new Point(16 * 8, 16 * 4),
                "Object Tools");


            #region Add Toolbar objs

            //hand (move) 
            moveObj = new InteractiveObject();
            Functions_InteractiveObjs.Reset(moveObj);
            moveObj.compSprite.texture = Assets.uiItemsSheet;
            Functions_Movement.Teleport(moveObj.compMove, 16 * 1, 16);
            Functions_Component.Align(moveObj);
            moveObj.compAnim.currentAnimation = AnimationFrames.Ui_Hand_Open;
            Functions_Animation.Animate(moveObj.compAnim, moveObj.compSprite);

            //rotateObj 
            rotateObj = new InteractiveObject();
            Functions_InteractiveObjs.Reset(rotateObj);
            rotateObj.compSprite.texture = Assets.uiItemsSheet;
            Functions_Movement.Teleport(rotateObj.compMove, 16 * 2, 16);
            Functions_Component.Align(rotateObj);
            rotateObj.compAnim.currentAnimation = AnimationFrames.Ui_Rotate;
            Functions_Animation.Animate(rotateObj.compAnim, rotateObj.compSprite);

            //add icon
            addObj = new InteractiveObject();
            Functions_InteractiveObjs.Reset(addObj);
            addObj.compSprite.texture = Assets.uiItemsSheet;
            Functions_Movement.Teleport(addObj.compMove, 16 * 3, 16);
            Functions_Component.Align(addObj);
            addObj.compAnim.currentAnimation = AnimationFrames.Ui_Add;
            Functions_Animation.Animate(addObj.compAnim, addObj.compSprite);

            //minus icon
            deleteObj = new InteractiveObject();
            Functions_InteractiveObjs.Reset(deleteObj);
            deleteObj.compSprite.texture = Assets.uiItemsSheet;
            Functions_Movement.Teleport(deleteObj.compMove, 16 * 4, 16);
            Functions_Component.Align(deleteObj);
            deleteObj.compAnim.currentAnimation = AnimationFrames.Ui_Delete;
            Functions_Animation.Animate(deleteObj.compAnim, deleteObj.compSprite);

            #endregion


            //create current obj components
            currentObjRef = new InteractiveObject();
            currentObjDirectionText = new ComponentText(
                Assets.font, "",
                new Vector2(0, 0),
                ColorScheme.textDark);

            selectionBoxObj = new ComponentSprite(
                Assets.uiItemsSheet,
                new Vector2(-100, 5000),
                AnimationFrames.Ui_SelectionBox[0],
                new Point(16, 16));
            selectionBoxTool = new ComponentSprite(
                Assets.uiItemsSheet,
                new Vector2(-100, 5000),
                AnimationFrames.Ui_SelectionBox[0],
                new Point(16, 16));

            //initialize the widget 
            SetActiveTool(moveObj);
            TopDebugMenu.objToolState = ObjToolState.MoveObj;
            grabbedObj = null;
            Reset(16 * 1, 16 * 17 + 8); //bottom left
        }

        public override void Reset(int X, int Y)
        {  
            Functions_MenuWindow.ResetAndMove(window, X, Y, window.size, window.title.text);
            //move tools
            Functions_Movement.Teleport(moveObj.compMove, X + 16 * 1, Y + 16 * 2);
            Functions_Movement.Teleport(rotateObj.compMove, X + 16 * 2, Y + 16 * 2);
            Functions_Movement.Teleport(addObj.compMove, X + 16 * 3, Y + 16 * 2);
            Functions_Movement.Teleport(deleteObj.compMove, X + 16 * 4, Y + 16 * 2);
            //align tools
            Functions_Component.Align(moveObj);
            Functions_Component.Align(rotateObj);
            Functions_Component.Align(addObj);
            Functions_Component.Align(deleteObj);

            Functions_Movement.Teleport(currentObjRef.compMove, X + 16 * 5, Y + 16 * 2);
            Functions_Component.Align(currentObjRef);
            //place direction text in footer of window
            currentObjDirectionText.position.X = window.interior.rec.X + 5;
            currentObjDirectionText.position.Y = window.interior.rec.Y + 45;
        }





        public Boolean HandleInput_Widget()
        {
            if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
            {
                if (TopDebugMenu.hideAll == false)
                {   //Set Active Tool (Move, Rotate, Add, Delete)
                    if (window.interior.rec.Contains(Input.cursorPos))
                    {
                        //check cursorPos contains with individual tool objs
                        if (moveObj.compCollision.rec.Contains(Input.cursorPos))
                        {
                            SetActiveTool(moveObj);
                            TopDebugMenu.objToolState = ObjToolState.MoveObj;
                        }
                        else if (rotateObj.compCollision.rec.Contains(Input.cursorPos))
                        {
                            SetActiveTool(rotateObj);
                            TopDebugMenu.objToolState = ObjToolState.RotateObj;
                        }
                        else if (addObj.compCollision.rec.Contains(Input.cursorPos))
                        {
                            SetActiveTool(addObj);
                            TopDebugMenu.objToolState = ObjToolState.AddObj;
                        }
                        else if (deleteObj.compCollision.rec.Contains(Input.cursorPos))
                        {
                            SetActiveTool(deleteObj);
                            TopDebugMenu.objToolState = ObjToolState.DeleteObj;
                        }
                        return true; //click was on widget
                    }
                }
            }
            return false;
        }

        public void HandleInput_World()
        {
            //convert cursor Pos to world pos
            worldPos = Functions_Camera2D.ConvertScreenToWorld(Input.cursorPos.X, Input.cursorPos.Y);

            if (objMode == false) { HandleInput_ActorMode(); return; }


            //mouse button states for Obj mode

            #region Handle Left Button CLICK

            if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
            {

                #region Handle Adding an Object To Room

                if (TopDebugMenu.objToolState == ObjToolState.AddObj)
                {

                    #region Check to see if we can add this type of Obj to this type of Room

                    /*
                    if (currentObjRef.type == InteractiveType.Dungeon_Chest)
                    {
                        //we convert the 'safe' chest into a key or hub chest here
                        if (LevelSet.currentLevel.currentRoom.roomID == RoomID.Key ||
                            LevelSet.currentLevel.currentRoom.roomID == RoomID.DEV_Key)
                        {   //convert to key chest
                            currentObjRef.type = InteractiveType.Dungeon_ChestKey;

                        }
                        else
                        {   //tell user we cant add a chest to this type of room
                            Screens.Dialog.SetDialog(AssetsDialog.CantAddChests);
                            ScreenManager.AddScreen(Screens.Dialog);
                            return; //dont add chest
                        }

                        //we cannot have more than one chest in a room
                        for (j = 0; j < Pool.intObjCount; j++)
                        {   //check all roomObjs for an active chest
                            if (Pool.intObjPool[j].active & Pool.intObjPool[j].group == InteractiveGroup.Chest)
                            {
                                Screens.Dialog.SetDialog(AssetsDialog.CantAddChests);
                                ScreenManager.AddScreen(Screens.Dialog);
                                return; //dont add chest
                            }
                        }
                    }
                    */



                    if (currentObjRef.type == InteractiveType.Dungeon_Switch)
                    {   //we cannot have more than one switch in a room
                        for (j = 0; j < Pool.intObjCount; j++)
                        {   //check all roomObjs for an active chest
                            if (Pool.intObjPool[j].active && Pool.intObjPool[j].type == InteractiveType.Dungeon_Switch)
                            {
                                Screens.Dialog.SetDialog(AssetsDialog.CantAddMoreSwitches);
                                ScreenManager.AddScreen(Screens.Dialog);
                                return; //dont add switch
                            }
                        }
                    }

                    #endregion


                    //we can only add roomObjects to the room, no particles/projectiles
                    InteractiveObject objRef;
                    objRef = Functions_Pool.GetIntObj();


                    //place currently selected obj in room, aligned to 16px grid
                    objRef.compMove.newPosition = Functions_Movement.AlignToGrid(worldPos.X, worldPos.Y);
                    Functions_Movement.Teleport(objRef.compMove,
                        objRef.compMove.newPosition.X, objRef.compMove.newPosition.Y);
                    //set obj direction + type from stored values
                    objRef.direction = currentObjRef.direction;
                    objRef.compMove.direction = currentObjRef.direction;
                    Functions_InteractiveObjs.SetType(objRef, currentObjRef.type);
                    //set animation frame
                    Functions_Animation.Animate(objRef.compAnim, objRef.compSprite);


                    #region Convert chest objs to empty chests after placement

                    if (
                        currentObjRef.type == InteractiveType.Chest
                        || currentObjRef.type == InteractiveType.ChestKey
                        )
                    {
                        Functions_InteractiveObjs.SetType(currentObjRef, InteractiveType.ChestEmpty);
                        //set the tool to be empty chest
                        activeObj = Widgets.WO_Dungeon.objList[19];
                    }

                    #endregion


                }

                #endregion


                #region Handle Rotating an Object in Room

                else if (TopDebugMenu.objToolState == ObjToolState.RotateObj)
                {
                    if (GrabRoomObject()) { RotateActiveObj(); }
                }

                #endregion

                
                #region Handle Grab/Move RoomObject State

                if (TopDebugMenu.objToolState == ObjToolState.MoveObj) { GrabRoomObject(); }

                //we handle grabbing roomObjs last because these roomObjs could be
                //anywhere on screen, including underneath a widget. by checking and
                //bailing from this method earlier (using widgets), we ensure that
                //the user has to be clicking on an object not covered by a widget.

                //we also want to allow the user the ability to temp store objects
                //outside of the room level, so we don't put this check into the
                //currentRoom collision check above. if we did, user couldn't grab
                //objs outside of the room's rec, but COULD drop them, which is dumb.

                #endregion

            }

            #endregion


            #region Handle Left Button DOWN (dragging)

            if (Functions_Input.IsMouseButtonDown(MouseButtons.LeftButton))
            {

                #region Handle Dragging an Object in Room

                if (TopDebugMenu.objToolState == ObjToolState.MoveObj)
                {   //if we are in Move state,
                    if (grabbedObj != null)
                    {   //match grabbed Obj pos to worldPos, aligned to 16px grid
                        grabbedObj.compMove.newPosition = Functions_Movement.AlignToGrid(worldPos.X, worldPos.Y);
                        Functions_Movement.Teleport(grabbedObj.compMove,
                            grabbedObj.compMove.newPosition.X, grabbedObj.compMove.newPosition.Y);
                        Functions_Component.Align(grabbedObj.compMove,
                            grabbedObj.compSprite, grabbedObj.compCollision);
                        //update selectionBox position (convert world pos to screen pos)
                        screenPos = Functions_Camera2D.ConvertWorldToScreen(
                            (int)activeObj.compSprite.position.X,
                            (int)activeObj.compSprite.position.Y);
                        selectionBoxObj.position.X = screenPos.X;
                        selectionBoxObj.position.Y = screenPos.Y;
                    }
                }

                #endregion


                #region Handle Deleting Objects in Room

                else if (TopDebugMenu.objToolState == ObjToolState.DeleteObj)
                {   //check collisions between cursor worldPos and obj, release() any colliding objs
                    
                    
                    
                    //delete roomObjs
                    for (Pool.intObjCounter = 0; Pool.intObjCounter < Pool.intObjCount; Pool.intObjCounter++)
                    {
                        if (Pool.intObjPool[Pool.intObjCounter].active)
                        {
                            if (Pool.intObjPool[Pool.intObjCounter].compCollision.rec.Contains(worldPos))
                            {

                                ignoreObj = false;


                                #region Editor Based Selection Cases

                                //check for specific conditions, like ignoring water tiles
                                if (Flags.IgnoreWaterTiles & Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Water_2x2)
                                { ignoreObj = true; } //ignore this object

                                //ignoring roof tiles for deletion
                                if (Flags.IgnoreRoofTiles)
                                {
                                    if(
                                        Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.House_Roof_Bottom ||
                                        Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.House_Roof_Chimney ||
                                        Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.House_Roof_Top
                                        )
                                    { ignoreObj = true; } //ignore this object
                                }

                                //boat tiles
                                if (Flags.IgnoreBoatTiles)
                                {
                                    if (
                                        //all the boat objs, in groups of 5 - should we model this as an obj.group value?

                                        //these are all indestructible objects
                                        /*
                                        Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Back_Center ||
                                        Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Back_Left ||
                                        Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Back_Left_Connector ||
                                        Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Back_Right ||
                                        Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Back_Right_Connector ||

                                        Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Bannister_Left ||
                                        Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Bannister_Right ||
                                        Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Bridge_Bottom ||
                                        Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Bridge_Top ||
                                        Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Engine ||

                                        Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Front ||
                                        Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Front_ConnectorLeft ||
                                        Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Front_ConnectorRight ||
                                        Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Front_Left ||
                                        Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Front_Right ||

                                        Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Stairs_Bottom_Left ||
                                        Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Stairs_Bottom_Right ||

                                        Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Stairs_Cover ||
                                        Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Stairs_Left ||
                                        Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Stairs_Right ||
                                        */


                                        Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Boat_Stairs_Left ||
                                        Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Boat_Stairs_Right ||

                                        //this one is special, because we use this obj as interior house floor
                                        Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Boat_Floor
                                        //ignore boat button also ignores floors = easy to move furniture around
                                        )
                                    { ignoreObj = true; } //ignore this object
                                }








                                #endregion



                                if (ignoreObj == false)
                                {
                                    //if we aren't ignoring the object, then release it
                                    Functions_Pool.Release(Pool.intObjPool[Pool.intObjCounter]);
                                }
                            }
                        }
                    }


                    //delete entityObjs
                    for (Pool.projectileCounter = 0; Pool.projectileCounter < Pool.projectileCount; Pool.projectileCounter++)
                    {
                        if (Pool.projectilePool[Pool.projectileCounter].active)
                        {
                            if (Pool.projectilePool[Pool.projectileCounter].compCollision.rec.Contains(worldPos))
                            { Functions_Pool.Release(Pool.projectilePool[Pool.projectileCounter]); }
                        }
                    }


                }

                #endregion

            }

            #endregion


            #region Handle Left Button RELEASE

            if (Functions_Input.IsNewMouseButtonRelease(MouseButtons.LeftButton))
            {
                grabbedObj = null; //release grabbed obj
            }

            #endregion

        }

        public void HandleInput_ActorMode_FUK()
        {
            if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
            {
                //add actor to room
                if (TopDebugMenu.objToolState == ObjToolState.AddObj)
                {
                    Actor actorRef = Functions_Pool.GetActor();

                    //place currently selected obj in room, aligned to 16px grid
                    actorRef.compMove.newPosition = Functions_Movement.AlignToGrid(worldPos.X, worldPos.Y);
                    Functions_Movement.Teleport(
                        actorRef.compMove,
                        actorRef.compMove.newPosition.X, 
                        actorRef.compMove.newPosition.Y);
                    //set type and initial state
                    Functions_Actor.SetType(actorRef, selectedActor);
                    actorRef.state = ActorState.Idle;
                }
            }
        }

















        public override void Update()
        {
            Functions_MenuWindow.Update(window);
            if (window.interior.displayState == DisplayState.Opened)
            {
                UpdateSelectionBox(selectionBoxObj);
                UpdateSelectionBox(selectionBoxTool);
                selectionBoxTool.position = activeTool.compSprite.position;
                Functions_Animation.Animate(currentObjRef.compAnim, currentObjRef.compSprite);
            }
        }

        public override void Draw()
        {
            Functions_Draw.Draw(window);
            if (window.interior.displayState == DisplayState.Opened)
            {
                Functions_Draw.Draw(moveObj);
                Functions_Draw.Draw(rotateObj);
                Functions_Draw.Draw(addObj);
                Functions_Draw.Draw(deleteObj);
                Functions_Draw.Draw(currentObjRef.compSprite);
                Functions_Draw.Draw(currentObjDirectionText);
                Functions_Draw.Draw(selectionBoxObj);
                Functions_Draw.Draw(selectionBoxTool);
            }
        }






        public void SetActiveTool(InteractiveObject Tool)
        {
            activeTool = Tool;
            selectionBoxTool.scale = 2.0f;
        }

        public void UpdateSelectionBox(ComponentSprite SelectionBox)
        {   //pulse the selectionBox alpha
            if (SelectionBox.alpha >= 1.0f) { SelectionBox.alpha = 0.1f; }
            else { SelectionBox.alpha += 0.025f; }
            //scale the selectionBox down to 1.0
            if (SelectionBox.scale > 1.0f) { SelectionBox.scale -= 0.07f; }
            else { SelectionBox.scale = 1.0f; }
        }

        public void GetActiveObjInfo()
        {   //reset objRef, match currentObjRef to activeObj
            Functions_InteractiveObjs.Reset(currentObjRef);
            currentObjRef.direction = activeObj.direction; //store direction value
            currentObjRef.compSprite.rotationValue = activeObj.compSprite.rotationValue;
            Functions_InteractiveObjs.SetType(currentObjRef, activeObj.type);
            //update the currentObj text displays
            window.title.text = "" + currentObjRef.type;
            currentObjDirectionText.text = "dir: " + currentObjRef.direction;
        }






        
        public Boolean GrabRoomObject()
        {   //grab roomObjs
            for (Pool.intObjCounter = 0; Pool.intObjCounter < Pool.intObjCount; Pool.intObjCounter++)
            {   //loop thru roomObj pool, checking collisions with cursor's worldPos
                if (Pool.intObjPool[Pool.intObjCounter].active)
                {   //check collisions between worldPos and obj, grab any colliding obj
                    if (Pool.intObjPool[Pool.intObjCounter].compCollision.rec.Contains(worldPos))
                    {
                        ignoreObj = false;


                        #region Editor Based Selection Cases

                        //check for specific conditions, like ignoring water tiles
                        if (Flags.IgnoreWaterTiles)
                        {
                            if(
                                Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Water_2x2 ||
                                Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Coastline_1x2_Animated
                                )
                            { ignoreObj = true; } //ignore this object
                        }

                        //ignoring roof tiles for selection
                        if (Flags.IgnoreRoofTiles)
                        {
                            if (
                                Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.House_Roof_Bottom ||
                                Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.House_Roof_Chimney ||
                                Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.House_Roof_Top
                                )
                            { ignoreObj = true; } //ignore this object
                        }

                        //boat tiles
                        if (Flags.IgnoreBoatTiles)
                        {
                            if (

                                /*
                                //all the boat objs, in groups of 5 - should we model this as an obj.group value?
                                Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Back_Center ||
                                Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Back_Left ||
                                Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Back_Left_Connector ||
                                Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Back_Right ||
                                Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Back_Right_Connector ||

                                Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Bannister_Left ||
                                Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Bannister_Right ||
                                Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Bridge_Bottom ||
                                Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Bridge_Top ||
                                Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Engine ||
                                
                                Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Front ||
                                Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Front_ConnectorLeft ||
                                Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Front_ConnectorRight ||
                                Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Front_Left ||
                                Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Front_Right ||

                                Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Stairs_Bottom_Left ||
                                Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Stairs_Bottom_Right ||
                                Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Stairs_Top_Left ||
                                Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Wor_Boat_Stairs_Top_Right ||
                                */

                                Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Boat_Stairs_Cover ||
                                Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Boat_Stairs_Left ||
                                Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Boat_Stairs_Right ||

                                

                                //this one is special, because we use this obj as interior house floor
                                Pool.intObjPool[Pool.intObjCounter].type == InteractiveType.Boat_Floor
                                //ignore boat button also ignores floors = easy to move furniture around
                                )
                            { ignoreObj = true; } //ignore this object
                        }

                        #endregion


                        //editor can ignore certain object types to make editing easier
                        if (ignoreObj == false)
                        {
                            SelectObject(Pool.intObjPool[Pool.intObjCounter]);
                            return true;
                        }
                        else { } //continue onto the next object
                    }
                }
            }
            return false; //no collision with roomObj
        }



        public void SelectObject(InteractiveObject Obj)
        {
            grabbedObj = Obj;
            activeObj = Obj;
            GetActiveObjInfo();
            selectionBoxObj.position = Obj.compSprite.position;
            selectionBoxObj.scale = 2.0f;
            window.title.text = "Obj: " + Obj.type;
            currentObjDirectionText.text = "dir: " + Obj.direction;
        }




        public void RotateActiveObj()
        {   
            //set activeObj's obj.direction based on type
            if (activeObj.type == InteractiveType.Lava_PitBridge)
            {   //flip between horizontal and vertical directions
                if (activeObj.direction == Direction.Up || activeObj.direction == Direction.Down)
                { activeObj.direction = Direction.Left; }
                else { activeObj.direction = Direction.Down; }
            }


            //these are objects that we allow rotation upon
            else if (activeObj.type == InteractiveType.ConveyorBeltOn
                || activeObj.type == InteractiveType.ConveyorBeltOff
                || activeObj.type == InteractiveType.Dungeon_BlockSpike)
            {   //flip thru cardinal directions
                activeObj.direction = Functions_Direction.GetCardinalDirection_LeftRight(activeObj.direction);
                if (activeObj.direction == Direction.Up) { activeObj.direction = Direction.Left; }
                else if (activeObj.direction == Direction.Left) { activeObj.direction = Direction.Down; }
                else if (activeObj.direction == Direction.Down) { activeObj.direction = Direction.Right; }
                else { activeObj.direction = Direction.Up; }

                //set object's move component direction based on type
                if (activeObj.type == InteractiveType.Dungeon_BlockSpike)
                { activeObj.compMove.direction = activeObj.direction; }
            }

            //rotate all other objects thru clockwise rotation
            else
            {
                //flip thru cardinal directions
                activeObj.direction = Functions_Direction.GetCardinalDirection_LeftRight(activeObj.direction);
                if (activeObj.direction == Direction.Up) { activeObj.direction = Direction.Left; }
                else if (activeObj.direction == Direction.Left) { activeObj.direction = Direction.Down; }
                else if (activeObj.direction == Direction.Down) { activeObj.direction = Direction.Right; }
                else { activeObj.direction = Direction.Up; }
            }


            //set the rotation of the sprite based on obj.direction                                             
            Functions_InteractiveObjs.SetRotation(activeObj);
            GetActiveObjInfo();
        }





        public void CheckObjList(List<InteractiveObject> objList)
        {   //does any obj on the widget's objList contain the mouse position?
            for (int i = 0; i < objList.Count; i++)
            {   //if there is a collision, set the active object to the object clicked on
                if (objList[i].compCollision.rec.Contains(Input.cursorPos))
                {
                    //set activeObj, update selection box scale + position
                    activeObj = objList[i];
                    selectionBoxObj.position = activeObj.compSprite.position;
                    selectionBoxObj.scale = 2.0f;
                    GetActiveObjInfo();
                    if (TopDebugMenu.objToolState == ObjToolState.RotateObj) { RotateActiveObj(); }

                    //set a state to tell objtools to create obj
                    objMode = true;
                }
            }
        }

        public void CheckObjList(List<IndestructibleObject> objList)
        {   //does any obj on the widget's objList contain the mouse position?
            for (int i = 0; i < objList.Count; i++)
            {   //if there is a collision, set the active object to the object clicked on
                if (objList[i].compCollision.rec.Contains(Input.cursorPos))
                {
                    //set activeObj, update selection box scale + position

                    /*
                    //we cant set the indestructible object to the active obj
                    //simply because the active obj is an interactive obj, not
                    //an indestructible one
                    activeObj = objList[i];

                    selectionBoxObj.position = activeObj.compSprite.position;
                    selectionBoxObj.scale = 2.0f;
                    GetActiveObjInfo();
                    if (TopDebugMenu.objToolState == ObjToolState.RotateObj) { RotateActiveObj(); }

                    //set a state to tell objtools to create obj
                    objMode = true;

                    */

                    Debug.WriteLine("indObj type selected: " + objList[i].type);

                }
            }
        }

        public void CheckActorsList(List<Actor> actList)
        {
            for (int i = 0; i < actList.Count; i++)
            {
                if (actList[i].compCollision.rec.Contains(Input.cursorPos))
                {
                    //copy actor's sprite and animation into current obj
                    currentObjRef.compAnim.currentAnimation = actList[i].compAnim.currentAnimation;
                    currentObjRef.compSprite.texture = actList[i].compSprite.texture;
                    currentObjRef.compSprite.drawRec.Width = actList[i].compSprite.drawRec.Width;
                    currentObjRef.compSprite.drawRec.Height = actList[i].compSprite.drawRec.Height;
                    currentObjRef.compSprite.currentFrame = actList[i].compSprite.currentFrame;
                    currentObjRef.compAnim.index = 0;

                    //select actor
                    selectionBoxObj.position = actList[i].compSprite.position;
                    selectionBoxObj.scale = 2.0f;
                    //display info about actor
                    window.title.text = "" + actList[i].type;
                    currentObjDirectionText.text = "!!!! actor selected !!!!";
                    //store actor type (in this widget)
                    selectedActor = actList[i].type;
                    //set a state to tell objtools to create actor
                    objMode = false;
                }
            }
        }









    }
}