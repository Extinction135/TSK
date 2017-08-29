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
        int j;
        int counter;
        public Point worldPos; //used to translate screen to world position
        public Point screenPos; //used to translate world to screen position
        public ObjToolState objToolState;

        public ComponentSprite cursorSprite;
        public ComponentSprite toolTipSprite;

        public GameObject currentObjRef;
        public ComponentText currentObjTypeText;
        public ComponentText currentObjDirectionText;

        public ComponentSprite selectionBoxObj; //highlites the currently selected obj
        public ComponentSprite selectionBoxTool; //highlites the currently selected obj
        public GameObject activeObj; //points to Obj on obj list OR on roomObj/entity list
        public GameObject grabbedObj; //obj/entity that is picked up/dragged/dropped in room
        public GameObject activeTool; //points to a ToolObj on the obj list

        public List<GameObject> objList; //a list of objects user can select
        public int objListTotal;
        //0 - 35 room objs, 36 - 40 enemy objs, 41+ tool objs
        public GameObject moveObj;
        public GameObject rotateObj;
        public GameObject addObj;
        public GameObject deleteObj;



        public WidgetObjectTools()
        {

            #region Create Window and Divider lines

            window = new MenuWindow(new Point(0, 0),
                new Point(16 * 6, 16 * 15), "Object Tools");
            window.lines.Add(new MenuRectangle(new Point(0, 0), new Point(0, 0), Assets.colorScheme.windowInset));
            window.lines.Add(new MenuRectangle(new Point(0, 0), new Point(0, 0), Assets.colorScheme.windowInset));

            #endregion


            //create cursor sprites
            cursorSprite = new ComponentSprite(Assets.mainSheet,
                new Vector2(0, 0), new Byte4(14, 13, 0, 0), new Point(16, 16));
            toolTipSprite = new ComponentSprite(Assets.mainSheet,
                new Vector2(0, 0), new Byte4(15, 15, 0, 0), new Point(16, 16));

            //create current obj components
            currentObjRef = new GameObject();
            currentObjTypeText = new ComponentText(Assets.font, "", new Vector2(0, 0), Assets.colorScheme.textDark);
            currentObjDirectionText = new ComponentText(Assets.font, "", new Vector2(0, 0), Assets.colorScheme.textDark);


            #region Create the SelectionBoxes

            selectionBoxObj = new ComponentSprite(
                Assets.mainSheet, new Vector2(-100, 5000),
                new Byte4(15, 7, 0, 0), new Point(16, 16));
            selectionBoxTool = new ComponentSprite(
                Assets.mainSheet, new Vector2(-100, 5000),
                new Byte4(15, 7, 0, 0), new Point(16, 16));

            #endregion


            objList = new List<GameObject>();


            #region Add RoomObjs to ObjList

            for (i = 0; i < 7; i++) //row
            {
                for (j = 0; j < 5; j++) //column
                {
                    GameObject obj = new GameObject();
                    //set the new position value for the move component
                    obj.compMove.newPosition.X = 16 + 8 + (16 * j);
                    obj.compMove.newPosition.Y = 16 * 5 + (16 * i);


                    #region Set the objects properly

                    if (i == 0) //first row
                    {
                        if (j == 0) { Functions_GameObject.SetType(obj, ObjType.BossStatue); }
                        else if (j == 1) { Functions_GameObject.SetType(obj, ObjType.Pillar); }
                        else if (j == 2) { Functions_GameObject.SetType(obj, ObjType.BlockDark); }
                        else if (j == 3) { Functions_GameObject.SetType(obj, ObjType.BlockLight); }
                        //else if (j == 4) { Functions_GameObject.SetType(obj, ObjType.BossStatue); }
                    }
                    else if (i == 1) //second row
                    {
                        if (j == 0) { Functions_GameObject.SetType(obj, ObjType.PotSkull); }
                        //else if (j == 1) { Functions_GameObject.SetType(obj, ObjType.SwitchBlockDown); }
                        //else if (j == 2) { Functions_GameObject.SetType(obj, ObjType.SwitchBlockUp); }
                        //else if (j == 3) { Functions_GameObject.SetType(obj, ObjType.TorchUnlit); }
                        //else if (j == 4) { Functions_GameObject.SetType(obj, ObjType.TorchLit); }
                    }
                    else if (i == 2) //third row
                    {
                        if (j == 0) { Functions_GameObject.SetType(obj, ObjType.LeverOff); }
                        else if (j == 1) { Functions_GameObject.SetType(obj, ObjType.ConveyorBeltOn); }
                        else if (j == 2) { Functions_GameObject.SetType(obj, ObjType.SpikesFloorOn); }
                        //else if (j == 3) { Functions_GameObject.SetType(obj, ObjType.SpikesFloor); }
                        //else if (j == 4) { Functions_GameObject.SetType(obj, ObjType.Lever); }
                    }
                    else if (i == 3) //fourth row
                    {
                        if (j == 0) { Functions_GameObject.SetType(obj, ObjType.SwitchBlockBtn); }
                        else if (j == 1) { Functions_GameObject.SetType(obj, ObjType.SwitchBlockDown); }
                        else if (j == 2) { Functions_GameObject.SetType(obj, ObjType.SwitchBlockUp); }
                        else if (j == 3) { Functions_GameObject.SetType(obj, ObjType.Flamethrower); }
                        else if (j == 4) { Functions_GameObject.SetType(obj, ObjType.Bumper); }
                    }
                    else if (i == 4) //fifth row
                    {
                        if (j == 0) { Functions_GameObject.SetType(obj, ObjType.Switch); }
                        else if (j == 1) { Functions_GameObject.SetType(obj, ObjType.ChestKey); }
                        else if (j == 2) { Functions_GameObject.SetType(obj, ObjType.ChestMap); }
                        else if (j == 3) { Functions_GameObject.SetType(obj, ObjType.TorchUnlit); }
                        else if (j == 4) { Functions_GameObject.SetType(obj, ObjType.TorchLit); }
                    }
                    else if (i == 5) //sixth row - Pits and Pit Accessories
                    {
                        if (j == 0) { Functions_GameObject.SetType(obj, ObjType.PitTrap); }
                        else if (j == 1) { Functions_GameObject.SetType(obj, ObjType.PitAnimated); }
                        else if (j == 2) { Functions_GameObject.SetType(obj, ObjType.Bridge); }
                        else if (j == 3) { Functions_GameObject.SetType(obj, ObjType.PitTop); }
                        else if (j == 4) { Functions_GameObject.SetType(obj, ObjType.PitBottom); }
                    }
                    else if (i == 6) //seventh row - Projectiles
                    {
                        if (j == 0) { Functions_GameObject.SetType(obj, ObjType.ProjectileSpikeBlock); }
                        //else if (j == 1) { Functions_GameObject.SetType(obj, ObjType.ProjectileSpikeBlock); }
                        //else if (j == 2) { Functions_GameObject.SetType(obj, ObjType.ProjectileSpikeBlock); }
                        //else if (j == 3) { Functions_GameObject.SetType(obj, ObjType.ProjectileSpikeBlock); }
                        //else if (j == 4) { Functions_GameObject.SetType(obj, ObjType.ProjectileSpikeBlock); }
                    }

                    #endregion


                    //update the obj's sprite comp to display proper frame
                    Functions_Animation.Animate(obj.compAnim, obj.compSprite);
                    //manually set & align the obj's collision rec
                    obj.compCollision.rec.Width = 16;
                    obj.compCollision.rec.Height = 16;
                    obj.compCollision.rec.X = 16 + 8 + (16 * j) - 8;
                    obj.compCollision.rec.Y = 16 * 5 + (16 * i) - 8;
                    //add the object to the list
                    objList.Add(obj);
                }
            }

            #endregion


            #region Add Enemy objs to ObjList

            for (i = 1; i < 6; i++)
            {
                GameObject enemySpawn = new GameObject();
                enemySpawn.compMove.newPosition.X = 16 * i + 8;
                enemySpawn.compMove.newPosition.Y = 16 * 12;

                if (i == 1)
                { Functions_GameObject.SetType(enemySpawn, ObjType.SpawnEnemy1); }
                else if (i == 2)
                { Functions_GameObject.SetType(enemySpawn, ObjType.SpawnEnemy2); }
                else if (i == 3)
                { Functions_GameObject.SetType(enemySpawn, ObjType.SpawnEnemy3); }
                else if (i == 4)
                { Functions_GameObject.SetType(enemySpawn, ObjType.SpawnEnemy2); }
                else if (i == 5)
                { Functions_GameObject.SetType(enemySpawn, ObjType.SpawnEnemy1); }

                Functions_Animation.Animate(enemySpawn.compAnim, enemySpawn.compSprite);
                objList.Add(enemySpawn); //index 35-39
            }

            #endregion


            #region Add Toolbar objs to ObjList

            //move icon - index 40
            moveObj = new GameObject();
            Functions_GameObject.ResetObject(moveObj);
            moveObj.compSprite.texture = Assets.mainSheet;
            //set sprite position, frame, collision rec
            moveObj.compSprite.position.X = 16 * 1 + 8;
            moveObj.compSprite.position.Y = 16 * 14;
            Functions_Component.SetZdepth(moveObj.compSprite);
            moveObj.compCollision.rec.X = 16 * 1 + 8 - 8;
            moveObj.compCollision.rec.Y = 16 * 14 - 8;
            moveObj.compAnim.currentAnimation = new List<Byte4> { new Byte4(14, 13, 0, 0) };
            Functions_Animation.Animate(moveObj.compAnim, moveObj.compSprite);
            objList.Add(moveObj); //add object to list

            //rotateObj
            rotateObj = new GameObject();
            Functions_GameObject.ResetObject(rotateObj);
            rotateObj.compSprite.texture = Assets.mainSheet;
            //set sprite position, frame, collision rec
            rotateObj.compSprite.position.X = 16 * 3 + 8;
            rotateObj.compSprite.position.Y = 16 * 14;
            Functions_Component.SetZdepth(rotateObj.compSprite);
            rotateObj.compCollision.rec.X = 16 * 3 + 8 - 8;
            rotateObj.compCollision.rec.Y = 16 * 14 - 8;
            rotateObj.compAnim.currentAnimation = new List<Byte4> { new Byte4(13, 15, 0, 0) };
            Functions_Animation.Animate(rotateObj.compAnim, rotateObj.compSprite);
            objList.Add(rotateObj); //add object to list

            //add icon
            addObj = new GameObject();
            Functions_GameObject.ResetObject(addObj);
            addObj.compSprite.texture = Assets.mainSheet;
            //set sprite position, frame, collision rec
            addObj.compSprite.position.X = 16 * 4 + 8;
            addObj.compSprite.position.Y = 16 * 14;
            Functions_Component.SetZdepth(addObj.compSprite);
            addObj.compCollision.rec.X = 16 * 4 + 8 - 8;
            addObj.compCollision.rec.Y = 16 * 14 - 8;
            addObj.compAnim.currentAnimation = new List<Byte4> { new Byte4(14, 15, 0, 0) };
            Functions_Animation.Animate(addObj.compAnim, addObj.compSprite);
            objList.Add(addObj); //add object to list

            //minus icon
            deleteObj = new GameObject();
            Functions_GameObject.ResetObject(deleteObj);
            deleteObj.compSprite.texture = Assets.mainSheet;
            //set sprite position, frame, collision rec
            deleteObj.compSprite.position.X = 16 * 5 + 8;
            deleteObj.compSprite.position.Y = 16 * 14;
            Functions_Component.SetZdepth(deleteObj.compSprite);
            deleteObj.compCollision.rec.X = 16 * 5 + 8 - 8;
            deleteObj.compCollision.rec.Y = 16 * 14 - 8;
            deleteObj.compAnim.currentAnimation = new List<Byte4> { new Byte4(15, 15, 0, 0) };
            Functions_Animation.Animate(deleteObj.compAnim, deleteObj.compSprite);
            objList.Add(deleteObj); //add object to list

            #endregion


            objListTotal = objList.Count();

            //initialize the widget
            activeObj = objList[0]; 
            SetActiveTool(moveObj);
            objToolState = ObjToolState.MoveObj;
            grabbedObj = null;
            GetActiveObjInfo();
        }

        public override void Reset(int X, int Y)
        {   //reset the room builder widget's window
            window.lines[2].position.Y = Y + (16 * 3); //top divider (below current obj display)
            window.lines[3].position.Y = Y + (16 * 12); //bottom divider (above tools)
            Functions_MenuWindow.ResetAndMove(window, X, Y, window.size, window.title.text);


            #region Set current object components

            Functions_Movement.Teleport(currentObjRef.compMove, X + 16, Y + 16 * 2);
            Functions_Component.Align(currentObjRef.compMove, currentObjRef.compSprite, currentObjRef.compCollision);

            currentObjTypeText.position.X = currentObjRef.compMove.position.X + 14;
            currentObjTypeText.position.Y = currentObjRef.compMove.position.Y - 12;

            currentObjDirectionText.position.X = currentObjRef.compMove.position.X + 14;
            currentObjDirectionText.position.Y = currentObjRef.compMove.position.Y - 2;

            #endregion


            int yPos = 16 * 4; //controls Y position of roomObjs, enemy spawn objs
            int offset; //controls position of enemy spawn objs, tool icons


            #region Move room objs

            counter = 0;
            for (i = 0; i < 7; i++) //row
            {
                for (j = 0; j < 5; j++) //column
                {
                    objList[counter].compSprite.position.X = X + 16 + 0 + (16 * j);
                    objList[counter].compSprite.position.Y = Y + yPos + (16 * i);
                    objList[counter].compCollision.rec.X = X + 16 + 0 + (16 * j) - 8;
                    objList[counter].compCollision.rec.Y = Y + yPos + (16 * i) - 8;
                    counter++;
                }
            }

            #endregion


            #region Move enemy spawn objs

            offset = 16 * 7;

            for (j = 0; j < 5; j++)
            {
                objList[counter].compSprite.position.X = X + 16 + 0 + (16 * j);
                objList[counter].compSprite.position.Y = Y + yPos + offset;
                objList[counter].compCollision.rec.X = X + 16 + 0 + (16 * j) - 8;
                objList[counter].compCollision.rec.Y = Y + yPos + offset - 8;
                counter++;
            }

            #endregion


            #region Move Grab, Rotate, Add, and Delete Icons

            offset = 16 * 9;

            moveObj.compSprite.position.X = X + 16 * 1;
            moveObj.compSprite.position.Y = Y + offset + yPos;
            moveObj.compCollision.rec.X = X + 16 * 1 - 8;
            moveObj.compCollision.rec.Y = Y + +offset + yPos - 8;

            rotateObj.compSprite.position.X = X + 16 * 3;
            rotateObj.compSprite.position.Y = Y + offset + yPos;
            rotateObj.compCollision.rec.X = X + 16 * 3 - 8;
            rotateObj.compCollision.rec.Y = Y + +offset + yPos - 8;

            addObj.compSprite.position.X = X + 16 * 4;
            addObj.compSprite.position.Y = Y + +offset + yPos;
            addObj.compCollision.rec.X = X + 16 * 4 - 8;
            addObj.compCollision.rec.Y = Y + +offset + yPos - 8;

            deleteObj.compSprite.position.X = X + 16 * 5;
            deleteObj.compSprite.position.Y = Y + +offset + yPos;
            deleteObj.compCollision.rec.X = X + 16 * 5 - 8;
            deleteObj.compCollision.rec.Y = Y + +offset + yPos - 8;

            #endregion


        }

        public void HandleInput()
        {
            //convert cursor Pos to world pos
            worldPos = Functions_Camera2D.ConvertScreenToWorld(Input.cursorPos.X, Input.cursorPos.Y);


            #region Set Mouse Cursor Sprite

            cursorSprite.currentFrame.Y = 14; ; //default to pointer
            if (objToolState == ObjToolState.MoveObj) //check/set move state
            { cursorSprite.currentFrame.Y = 13; }
            if (Functions_Input.IsMouseButtonDown(MouseButtons.LeftButton))
            { cursorSprite.currentFrame.X = 15; } //set clicked frame
            else { cursorSprite.currentFrame.X = 14; }

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


            if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
            {

                #region Handle Grab/Move RoomObject State

                if (objToolState == ObjToolState.MoveObj)
                { GrabRoomObject(); }

                #endregion


                if (window.interior.rec.Contains(Input.cursorPos))
                {   //if mouse is contained within widget, select active obj


                    #region Handle Obj / Tool Selection

                    //does any obj on the widget's objList contain the mouse position?
                    for (i = 0; i < objListTotal; i++)
                    {   //if there is a collision, set the active object to the object clicked on
                        if (objList[i].compCollision.rec.Contains(Input.cursorPos))
                        {
                            //handle collision with objList
                            if (i < 40)
                            {   //set activeObj, update selection box scale + position
                                activeObj = objList[i];
                                selectionBoxObj.position = activeObj.compSprite.position;
                                selectionBoxObj.scale = 2.0f;
                                GetActiveObjInfo();
                                //if we are in rotate mode, rotate the activeObj
                                if (objToolState == ObjToolState.RotateObj) { RotateActiveObj(); }
                            }
                            //handle collision with tool obj
                            else if (objList[i] == moveObj)
                            {
                                SetActiveTool(moveObj);
                                objToolState = ObjToolState.MoveObj;
                            }
                            else if (objList[i] == rotateObj)
                            {
                                SetActiveTool(rotateObj);
                                objToolState = ObjToolState.RotateObj;
                                toolTipSprite.currentFrame.X = 13;
                            }
                            else if (objList[i] == addObj)
                            {
                                SetActiveTool(addObj);
                                objToolState = ObjToolState.AddObj;
                                toolTipSprite.currentFrame.X = 14;
                            }
                            else if (objList[i] == deleteObj)
                            {
                                SetActiveTool(deleteObj);
                                objToolState = ObjToolState.DeleteObj;
                                toolTipSprite.currentFrame.X = 15;
                            }
                        }
                    }

                    #endregion

                }
                else if (Functions_Level.currentRoom.rec.Contains(worldPos))
                {   //if mouse worldPos is within room, allow adding of active object


                    #region Handle Adding an Object To Room

                    if (objToolState == ObjToolState.AddObj)
                    {


                        #region Check to see if we can add this type of Obj to this type of Room

                        if (currentObjRef.type == ObjType.ChestKey)
                        {
                            if (Functions_Level.currentRoom.type != RoomType.Key)
                            { ScreenManager.AddScreen(new ScreenDialog(Dialog.CantAddKeyChest)); }
                            return; //dont add chest
                        }
                        else if (currentObjRef.type == ObjType.ChestMap)
                        {
                            if (Functions_Level.currentRoom.type != RoomType.Hub)
                            { ScreenManager.AddScreen(new ScreenDialog(Dialog.CantAddMapChest)); }
                            return; //dont add chest
                        }

                        //we cannot have more than one chest in a room

                        #endregion


                        GameObject objRef;
                        //get an object from the entity pool or roomObj pool
                        if (currentObjRef.group == ObjGroup.Projectile)
                        { objRef = Functions_Pool.GetEntity(); }
                        else { objRef = Functions_Pool.GetRoomObj(); }
                        
                        //place currently selected obj in room, aligned to 16px grid
                        objRef.compMove.newPosition = AlignToGrid(worldPos.X, worldPos.Y);
                        Functions_Movement.Teleport(objRef.compMove,
                            objRef.compMove.newPosition.X, objRef.compMove.newPosition.Y);
                        //set obj direction + type from stored values
                        objRef.direction = currentObjRef.direction;
                        objRef.compMove.direction = currentObjRef.direction;
                        Functions_GameObject.SetType(objRef, currentObjRef.type);

                        //align & set animation frame
                        //Functions_Component.Align(objRef.compMove, objRef.compSprite, objRef.compCollision);
                        Functions_Animation.Animate(objRef.compAnim, objRef.compSprite);
                    }

                    #endregion


                    #region Handle Rotating an Object in Room

                    else if (objToolState == ObjToolState.RotateObj)
                    {
                        if (GrabRoomObject()) { RotateActiveObj(); }
                    }

                    #endregion

                }
            }
            if (Functions_Input.IsMouseButtonDown(MouseButtons.LeftButton))
            {

                #region Handle Dragging an Object in Room

                if (objToolState == ObjToolState.MoveObj)
                {   //if we are in Move state,
                    if (grabbedObj != null)
                    {   //match grabbed Obj pos to worldPos, aligned to 16px grid
                        grabbedObj.compMove.newPosition = AlignToGrid(worldPos.X, worldPos.Y);
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
                    for (Pool.entityCounter = 0; Pool.entityCounter < Pool.entityCount; Pool.entityCounter++)
                    {
                        if (Pool.entityPool[Pool.entityCounter].active)
                        {
                            if (Pool.entityPool[Pool.entityCounter].compCollision.rec.Contains(worldPos))
                            { Functions_Pool.Release(Pool.entityPool[Pool.entityCounter]); }
                        }
                    }
                }

                #endregion

            }
            if (Functions_Input.IsNewMouseButtonRelease(MouseButtons.LeftButton))
            {

                #region Handle Release Grabbed Obj

                grabbedObj = null;

                #endregion

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
                Functions_Draw.Draw(currentObjRef.compSprite);
                Functions_Draw.Draw(currentObjTypeText);
                Functions_Draw.Draw(currentObjDirectionText);

                for (i = 0; i < objListTotal; i++) //draw objlist's sprites
                { Functions_Draw.Draw(objList[i].compSprite); }

                if (Flags.DrawCollisions)
                {   //draw objlist's collision recs
                    for (i = 0; i < objListTotal; i++)
                    { Functions_Draw.Draw(objList[i].compCollision); }
                }
                Functions_Draw.Draw(selectionBoxObj);
                Functions_Draw.Draw(selectionBoxTool);
            }
            if (objToolState != ObjToolState.MoveObj) { Functions_Draw.Draw(toolTipSprite); }
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

        public Vector2 AlignToGrid(int X, int Y)
        {
            return new Vector2(16 * (X / 16) + 8, 16 * (Y / 16) + 8);
        }

        public void GetActiveObjInfo()
        {   //reset objRef, match currentObjRef to activeObj
            Functions_GameObject.ResetObject(currentObjRef);
            currentObjRef.direction = activeObj.direction; //store direction value
            currentObjRef.compSprite.rotationValue = activeObj.compSprite.rotationValue;
            Functions_GameObject.SetType(currentObjRef, activeObj.type);
            //update the currentObj text displays
            currentObjTypeText.text = "" + currentObjRef.type;
            currentObjDirectionText.text = "dir: " + currentObjRef.direction;
        }

        public Boolean GrabRoomObject()
        {
            //grab entities
            for (Pool.entityCounter = 0; Pool.entityCounter < Pool.entityCount; Pool.entityCounter++)
            {   //loop thru entity pool, checking collisions with cursor's worldPos
                if (Pool.entityPool[Pool.entityCounter].active)
                {   //check collisions between worldPos and obj, grab any colliding projectile
                    if(Pool.entityPool[Pool.entityCounter].group == ObjGroup.Projectile)
                    {
                        if (Pool.entityPool[Pool.entityCounter].compCollision.rec.Contains(worldPos))
                        {   //set both grabbedObj and activeObj
                            grabbedObj = Pool.entityPool[Pool.entityCounter];
                            activeObj = Pool.entityPool[Pool.entityCounter];
                            GetActiveObjInfo();
                            selectionBoxObj.scale = 2.0f;
                            return true;
                        }
                    }
                }
            }
            //grab roomObjs
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
            //there was no collision with a roomObj or entity
            return false;
        }

        public void RotateActiveObj()
        {   //set activeObj's obj.direction based on type
            if (activeObj.type == ObjType.Bridge)
            {   //flip between horizontal and vertical directions
                if (activeObj.direction == Direction.Up || activeObj.direction == Direction.Down)
                { activeObj.direction = Direction.Left; }
                else { activeObj.direction = Direction.Down; }
            }
            else if (activeObj.type == ObjType.ConveyorBeltOn
                || activeObj.type == ObjType.ConveyorBeltOff
                || activeObj.type == ObjType.ProjectileSpikeBlock)
            {   //flip thru cardinal directions
                activeObj.direction = Functions_Direction.GetCardinalDirection(activeObj.direction);
                if (activeObj.direction == Direction.Up) { activeObj.direction = Direction.Left; }
                else if (activeObj.direction == Direction.Left) { activeObj.direction = Direction.Down; }
                else if (activeObj.direction == Direction.Down) { activeObj.direction = Direction.Right; }
                else { activeObj.direction = Direction.Up; }
            }

            //set object's move component direction based on type
            if (activeObj.type == ObjType.ProjectileSpikeBlock)
            { activeObj.compMove.direction = activeObj.direction; }

            //set the rotation of the sprite based on obj.direction                                              
            Functions_GameObject.SetRotation(activeObj);
            GetActiveObjInfo();
        }

    }
}