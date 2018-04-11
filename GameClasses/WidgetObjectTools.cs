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
        //this widget handles the selection, grabbing, dragging,
        //releasing, adding, and deleting of roomObjects in an editor (Level or Room)
        
        int j;
        public ComponentSprite cursorSprite;
        public GameObject moveObj;
        public GameObject rotateObj;
        public GameObject addObj;
        public GameObject deleteObj;

        public Point worldPos; //used to translate screen to world position
        public Point screenPos; //used to translate world to screen position
        public ObjToolState objToolState;

        public ComponentSprite toolTipSprite;

        public GameObject currentObjRef;
        public ComponentText currentObjDirectionText;

        public ComponentSprite selectionBoxObj; //highlites the currently selected obj
        public ComponentSprite selectionBoxTool; //highlites the currently selected obj
        public GameObject activeObj; //points to Obj on objList OR on roomObj/entity list
        public GameObject grabbedObj; //obj/entity that is picked up/dragged/dropped in room
        public GameObject activeTool; //points to a ToolObj on the obj list



        public WidgetObjectTools()
        {
            window = new MenuWindow(
                new Point(0, 0),
                new Point(16 * 6, 16 * 4),
                "Object Tools");
            //create cursor sprites
            cursorSprite = new ComponentSprite(
                Assets.uiItemsSheet,
                new Vector2(0, 0),
                new Byte4(10, 2, 0, 0),
                new Point(16, 16));
            toolTipSprite = new ComponentSprite(
                Assets.uiItemsSheet,
                new Vector2(0, 0),
                new Byte4(10, 4, 0, 0),
                new Point(16, 16));


            #region Add Toolbar objs

            //hand (move) 
            moveObj = new GameObject();
            Functions_GameObject.ResetObject(moveObj);
            moveObj.compSprite.texture = Assets.uiItemsSheet;
            Functions_Movement.Teleport(moveObj.compMove, 16 * 1, 16);
            Functions_Component.Align(moveObj);
            moveObj.compAnim.currentAnimation = AnimationFrames.Ui_Hand_Open;
            Functions_Animation.Animate(moveObj.compAnim, moveObj.compSprite);

            //rotateObj 
            rotateObj = new GameObject();
            Functions_GameObject.ResetObject(rotateObj);
            rotateObj.compSprite.texture = Assets.uiItemsSheet;
            Functions_Movement.Teleport(rotateObj.compMove, 16 * 2, 16);
            Functions_Component.Align(rotateObj);
            rotateObj.compAnim.currentAnimation = AnimationFrames.Ui_Rotate;
            Functions_Animation.Animate(rotateObj.compAnim, rotateObj.compSprite);

            //add icon
            addObj = new GameObject();
            Functions_GameObject.ResetObject(addObj);
            addObj.compSprite.texture = Assets.uiItemsSheet;
            Functions_Movement.Teleport(addObj.compMove, 16 * 3, 16);
            Functions_Component.Align(addObj);
            addObj.compAnim.currentAnimation = AnimationFrames.Ui_Add;
            Functions_Animation.Animate(addObj.compAnim, addObj.compSprite);

            //minus icon
            deleteObj = new GameObject();
            Functions_GameObject.ResetObject(deleteObj);
            deleteObj.compSprite.texture = Assets.uiItemsSheet;
            Functions_Movement.Teleport(deleteObj.compMove, 16 * 4, 16);
            Functions_Component.Align(deleteObj);
            deleteObj.compAnim.currentAnimation = AnimationFrames.Ui_Delete;
            Functions_Animation.Animate(deleteObj.compAnim, deleteObj.compSprite);

            #endregion


            //create current obj components
            currentObjRef = new GameObject();
            currentObjDirectionText = new ComponentText(
                Assets.font, "",
                new Vector2(0, 0),
                Assets.colorScheme.textDark);

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
            objToolState = ObjToolState.MoveObj;
            grabbedObj = null;
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


            #region Set current object components

            Functions_Movement.Teleport(currentObjRef.compMove, X + 16 * 5, Y + 16 * 2);
            Functions_Component.Align(currentObjRef);
            //place direction text in footer of window
            currentObjDirectionText.position.X = window.interior.rec.X + 5;
            currentObjDirectionText.position.Y = window.interior.rec.Y + 45;

            #endregion

        }

        public void HandleInput()
        {
            //convert cursor Pos to world pos
            worldPos = Functions_Camera2D.ConvertScreenToWorld(Input.cursorPos.X, Input.cursorPos.Y);


            #region Set Mouse Cursor Sprite

            if (objToolState == ObjToolState.MoveObj) //check/set move state
            {   //if moving, show open hand cursor
                cursorSprite.currentFrame = AnimationFrames.Ui_Hand_Open[0];
                //if moving, and dragging, show grab cursor
                if (Functions_Input.IsMouseButtonDown(MouseButtons.LeftButton))
                { cursorSprite.currentFrame = AnimationFrames.Ui_Hand_Grab[0]; }
            }
            else
            {   //default to pointer
                cursorSprite.currentFrame = AnimationFrames.Ui_Hand_Point[0];
                //if clicking/dragging, show pointer press cursor
                if (Functions_Input.IsMouseButtonDown(MouseButtons.LeftButton))
                { cursorSprite.currentFrame = AnimationFrames.Ui_Hand_Press[0]; }
            }

            #endregion


            #region Match position of cursor sprite to cursor

            cursorSprite.position.X = Input.cursorPos.X;
            cursorSprite.position.Y = Input.cursorPos.Y;
            if (objToolState != ObjToolState.MoveObj)
            {   //apply offset for pointer sprite
                cursorSprite.position.X += 3;
                cursorSprite.position.Y += 6;
            }

            toolTipSprite.position.X = Input.cursorPos.X + 12;
            toolTipSprite.position.Y = Input.cursorPos.Y - 0;

            #endregion


            #region Set toolTip's animation frame based on objToolState

            if (objToolState == ObjToolState.AddObj)
            { toolTipSprite.currentFrame = AnimationFrames.Ui_Add[0]; }
            else if (objToolState == ObjToolState.DeleteObj)
            { toolTipSprite.currentFrame = AnimationFrames.Ui_Delete[0]; }
            else { toolTipSprite.currentFrame = AnimationFrames.Ui_Rotate[0]; }

            #endregion


            //mouse button states


            #region Handle Left Button CLICK

            if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
            {
                if (window.interior.rec.Contains(Input.cursorPos))
                {

                    #region Set Active Tool (Move, Rotate, Add, Delete)

                    //check cursorPos contains with individual tool objs
                    if (moveObj.compCollision.rec.Contains(Input.cursorPos))
                    { SetActiveTool(moveObj); objToolState = ObjToolState.MoveObj; }

                    else if (rotateObj.compCollision.rec.Contains(Input.cursorPos))
                    { SetActiveTool(rotateObj); objToolState = ObjToolState.RotateObj; }

                    else if (addObj.compCollision.rec.Contains(Input.cursorPos))
                    { SetActiveTool(addObj); objToolState = ObjToolState.AddObj; }

                    else if (deleteObj.compCollision.rec.Contains(Input.cursorPos))
                    { SetActiveTool(deleteObj); objToolState = ObjToolState.DeleteObj; }

                    #endregion

                    return; //prevent grabbing of obj underneath widget/window
                }
                else if (Functions_Level.currentRoom.rec.Contains(worldPos))
                {   //if mouse worldPos is within room, allow adding of active object

                    #region Handle Adding an Object To Room

                    if (objToolState == ObjToolState.AddObj)
                    {


                        #region Check to see if we can add this type of Obj to this type of Room

                        if (currentObjRef.type == ObjType.Dungeon_Chest)
                        {   
                            //we convert the 'safe' chest into a key or hub chest here
                            if (Functions_Level.currentRoom.type == RoomType.Key)
                            {   //convert to key chest
                                currentObjRef.type = ObjType.Dungeon_ChestKey;

                            }
                            else if (Functions_Level.currentRoom.type == RoomType.Hub)
                            {   //convert to map chest
                                currentObjRef.type = ObjType.Dungeon_ChestMap;
                            }
                            else
                            {   //tell user we cant add a chest to this type of room
                                ScreenManager.AddScreen(new ScreenDialog(Functions_Dialog.CantAddChests));
                                return; //dont add chest
                            }

                            //we cannot have more than one chest in a room
                            for (j = 0; j < Pool.roomObjCount; j++)
                            {   //check all roomObjs for an active chest
                                if (Pool.roomObjPool[j].active & Pool.roomObjPool[j].group == ObjGroup.Chest)
                                {
                                    ScreenManager.AddScreen(new ScreenDialog(Functions_Dialog.CantAddChests));
                                    return; //dont add chest
                                }
                            }
                        }
                        
                        else if (currentObjRef.type == ObjType.Dungeon_Switch)
                        {   //we cannot have more than one switch in a room
                            for (j = 0; j < Pool.roomObjCount; j++)
                            {   //check all roomObjs for an active chest
                                if (Pool.roomObjPool[j].active && Pool.roomObjPool[j].type == ObjType.Dungeon_Switch)
                                {
                                    ScreenManager.AddScreen(new ScreenDialog(Functions_Dialog.CantAddMoreSwitches));
                                    return; //dont add switch
                                }
                            }
                        }

                        #endregion


                        GameObject objRef;
                        //get an object from the entity pool or roomObj pool
                        if (currentObjRef.group == ObjGroup.Projectile)
                        { objRef = Functions_Pool.GetProjectile(); }
                        else if (currentObjRef.group == ObjGroup.Pickup)
                        { objRef = Functions_Pool.GetPickup(); }
                        else { objRef = Functions_Pool.GetRoomObj(); }

                        //place currently selected obj in room, aligned to 16px grid
                        objRef.compMove.newPosition = Functions_Movement.AlignToGrid(worldPos.X, worldPos.Y);
                        Functions_Movement.Teleport(objRef.compMove,
                            objRef.compMove.newPosition.X, objRef.compMove.newPosition.Y);
                        //set obj direction + type from stored values
                        objRef.direction = currentObjRef.direction;
                        objRef.compMove.direction = currentObjRef.direction;
                        Functions_GameObject.SetType(objRef, currentObjRef.type);
                        //set animation frame
                        Functions_Animation.Animate(objRef.compAnim, objRef.compSprite);


                        #region Convert chest objs to empty chests after placement

                        if (currentObjRef.group == ObjGroup.Chest)
                        {
                            Functions_GameObject.SetType(currentObjRef, ObjType.Dungeon_ChestEmpty);
                            window.title.text = "Obj: " + currentObjRef.type;
                            currentObjDirectionText.text = "dir: " + currentObjRef.direction;
                            //set the tool to be empty chest
                            activeObj = Widgets.WidgetObjects_Dungeon.objList[19];
                            selectionBoxObj.position = activeObj.compSprite.position;
                            selectionBoxObj.scale = 2.0f;
                            GetActiveObjInfo();
                        }

                        #endregion


                    }

                    #endregion


                    #region Handle Rotating an Object in Room

                    else if (objToolState == ObjToolState.RotateObj)
                    {
                        if (GrabRoomObject()) { RotateActiveObj(); }
                    }

                    #endregion

                    //dont return; here, continue to objToolState check below on purpose
                }


                #region Handle Grab/Move RoomObject State

                if (objToolState == ObjToolState.MoveObj) { GrabRoomObject(); }

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

                if (objToolState == ObjToolState.MoveObj)
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

                else if (objToolState == ObjToolState.DeleteObj)
                {   //check collisions between cursor worldPos and obj, release() any colliding objs
                    //delete roomObjs
                    for (Pool.roomObjCounter = 0; Pool.roomObjCounter < Pool.roomObjCount; Pool.roomObjCounter++)
                    {
                        if (Pool.roomObjPool[Pool.roomObjCounter].active)
                        {
                            if (Pool.roomObjPool[Pool.roomObjCounter].compCollision.rec.Contains(worldPos))
                            { Functions_Pool.Release(Pool.roomObjPool[Pool.roomObjCounter]); }
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


            if (!Flags.HideEditorWidgets)
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
                if (objToolState != ObjToolState.MoveObj) { Functions_Draw.Draw(toolTipSprite); }
            }


            //draw + track cursor regardless

            
            Functions_Draw.Draw(cursorSprite); 
        }



        public void SetActiveTool(GameObject Tool)
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
            Functions_GameObject.ResetObject(currentObjRef);
            currentObjRef.direction = activeObj.direction; //store direction value
            currentObjRef.compSprite.rotationValue = activeObj.compSprite.rotationValue;
            Functions_GameObject.SetType(currentObjRef, activeObj.type);
            //update the currentObj text displays
            window.title.text = "" + currentObjRef.type;
            currentObjDirectionText.text = "dir: " + currentObjRef.direction;
        }

        public Boolean GrabRoomObject()
        {   //grab roomObjs
            for (Pool.roomObjCounter = 0; Pool.roomObjCounter < Pool.roomObjCount; Pool.roomObjCounter++)
            {   //loop thru roomObj pool, checking collisions with cursor's worldPos
                if (Pool.roomObjPool[Pool.roomObjCounter].active)
                {   //check collisions between worldPos and obj, grab any colliding obj
                    if (Pool.roomObjPool[Pool.roomObjCounter].compCollision.rec.Contains(worldPos))
                    {   //set both grabbedObj and activeObj
                        grabbedObj = Pool.roomObjPool[Pool.roomObjCounter];
                        activeObj = Pool.roomObjPool[Pool.roomObjCounter];
                        GetActiveObjInfo();
                        selectionBoxObj.scale = 2.0f;
                        return true;
                    }
                }
            }
            return false; //no collision with roomObj
        }

        public void RotateActiveObj()
        {   //set activeObj's obj.direction based on type
            if (activeObj.type == ObjType.Dungeon_PitBridge)
            {   //flip between horizontal and vertical directions
                if (activeObj.direction == Direction.Up || activeObj.direction == Direction.Down)
                { activeObj.direction = Direction.Left; }
                else { activeObj.direction = Direction.Down; }
            }
            else if (activeObj.type == ObjType.Dungeon_ConveyorBeltOn
                || activeObj.type == ObjType.Dungeon_ConveyorBeltOff
                || activeObj.type == ObjType.Dungeon_BlockSpike)
            {   //flip thru cardinal directions
                activeObj.direction = Functions_Direction.GetCardinalDirection(activeObj.direction);
                if (activeObj.direction == Direction.Up) { activeObj.direction = Direction.Left; }
                else if (activeObj.direction == Direction.Left) { activeObj.direction = Direction.Down; }
                else if (activeObj.direction == Direction.Down) { activeObj.direction = Direction.Right; }
                else { activeObj.direction = Direction.Up; }
            }

            //set object's move component direction based on type
            if (activeObj.type == ObjType.Dungeon_BlockSpike)
            { activeObj.compMove.direction = activeObj.direction; }

            //set the rotation of the sprite based on obj.direction                                             
            Functions_GameObject.SetRotation(activeObj);
            GetActiveObjInfo();
        }

        public void CheckObjList(List<GameObject> objList)
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
                    if (objToolState == ObjToolState.RotateObj) { RotateActiveObj(); }

                }
            }
        }

    }
}