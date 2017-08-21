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

        public ComponentSprite cursorSprite;
        public ComponentSprite addDeleteSprite;
        public Point worldPos;

        public ComponentSprite selectionBoxObj; //highlites the currently selected obj
        public ComponentSprite selectionBoxTool; //highlites the currently selected obj
        public GameObject activeObj; //points to a RoomObj on the obj list
        public GameObject activeTool; //points to a ToolObj on the obj list

        public List<GameObject> objList; //a list of objects user can select
        public int objListTotal;
        //0 - 35 room objs, 36 - 40 enemy objs, 41 - 43 - tool objs (move, add, delete)
        public GameObject moveObj;
        public GameObject addObj;
        public GameObject deleteObj;

        public ObjToolState objToolState; //move, add, or delete obj
        public GameObject grabbedObj; //obj that is picked up/dragged/dropped in level



        public WidgetObjectTools()
        {

            #region Create Window and Divider lines

            window = new MenuWindow(new Point(0, 0), 
                new Point(16 * 6, 16 * 15), "Object Tools");
            window.lines.Add(new MenuRectangle(new Point(0, 0), new Point(0, 0), Assets.colorScheme.windowInset));
            window.lines.Add(new MenuRectangle(new Point(0, 0), new Point(0, 0), Assets.colorScheme.windowInset));

            #endregion


            //create the cursor sprite
            cursorSprite = new ComponentSprite(Assets.mainSheet,
                new Vector2(0, 0), new Byte4(14, 13, 0, 0), new Point(16, 16));
            addDeleteSprite = new ComponentSprite(Assets.mainSheet,
                new Vector2(0, 0), new Byte4(15, 15, 0, 0), new Point(16, 16));


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
                    //Functions_GameObject.ResetObject(obj);
                    //set the new position value for the move component
                    obj.compMove.newPosition.X = 16 + 8 + (16 * j);
                    obj.compMove.newPosition.Y = 16 * 5 + (16 * i);
                    //Functions_GameObject.SetType(obj, ObjType.WallStraight);


                    #region Set the objects properly

                    if (i == 0) //first row
                    {
                        if (j == 0) { Functions_GameObject.SetType(obj, ObjType.BossStatue); }
                        else if (j == 1) { Functions_GameObject.SetType(obj, ObjType.Pillar); }
                        else if (j == 2) { Functions_GameObject.SetType(obj, ObjType.BlockDark); }
                        else if (j == 3) { Functions_GameObject.SetType(obj, ObjType.BlockLight); }
                        else if (j == 4) { Functions_GameObject.SetType(obj, ObjType.BlockDraggable); }
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
                        //else if (j == 3) { Functions_GameObject.SetType(obj, ObjType.PitTop); }
                        //else if (j == 4) { Functions_GameObject.SetType(obj, ObjType.FloorBombable); }
                    }
                    else if (i == 4) //fifth row
                    {
                        if (j == 0) { Functions_GameObject.SetType(obj, ObjType.BlockSpikes); }
                        else if (j == 1) { Functions_GameObject.SetType(obj, ObjType.BlockSpikes); }
                        else if (j == 2) { Functions_GameObject.SetType(obj, ObjType.Flamethrower); }
                        else if (j == 3) { Functions_GameObject.SetType(obj, ObjType.Bumper); }
                        //else if (j == 4) { Functions_GameObject.SetType(obj, ObjType.FloorBombable); }
                    }
                    else if (i == 5) //sixth row
                    {
                        if (j == 0) { Functions_GameObject.SetType(obj, ObjType.PitAnimated); }
                        else if (j == 1) { Functions_GameObject.SetType(obj, ObjType.PitTop); }
                        else if (j == 2) { Functions_GameObject.SetType(obj, ObjType.PitBottom); }
                        else if (j == 3) { Functions_GameObject.SetType(obj, ObjType.PitTrapReady); }
                        else if (j == 4) { Functions_GameObject.SetType(obj, ObjType.Bridge); }
                    }
                    else if (i == 6) //seventh row
                    {
                        if (j == 0) { Functions_GameObject.SetType(obj, ObjType.Switch); }
                        else if (j == 1) { Functions_GameObject.SetType(obj, ObjType.ChestKey); }
                        else if (j == 2) { Functions_GameObject.SetType(obj, ObjType.ChestEmpty); }
                        else if (j == 3) { Functions_GameObject.SetType(obj, ObjType.TorchUnlit); }
                        else if (j == 4) { Functions_GameObject.SetType(obj, ObjType.TorchLit); }
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
            //set sprite position
            moveObj.compSprite.position.X = 16 * 1 + 8;
            moveObj.compSprite.position.Y = 16 * 14;
            Functions_Component.SetZdepth(moveObj.compSprite);
            //set sprites frame
            moveObj.compSprite.currentFrame.X = 14;
            moveObj.compSprite.currentFrame.Y = 13;
            //set collision rec
            moveObj.compCollision.rec.X = 16 * 1 + 8 - 8;
            moveObj.compCollision.rec.Y = 16 * 14 - 8;
            //add object to list
            objList.Add(moveObj);

            //add icon - index 41
            addObj = new GameObject();
            Functions_GameObject.ResetObject(addObj);
            addObj.compSprite.texture = Assets.mainSheet;
            //set sprite position
            addObj.compSprite.position.X = 16 * 3 + 8;
            addObj.compSprite.position.Y = 16 * 14;
            Functions_Component.SetZdepth(addObj.compSprite);
            //set sprites frame
            addObj.compSprite.currentFrame.X = 14;
            addObj.compSprite.currentFrame.Y = 15;
            //set collision rec
            addObj.compCollision.rec.X = 16 * 3 + 8 - 8;
            addObj.compCollision.rec.Y = 16 * 14 - 8;
            //add object to list
            objList.Add(addObj);

            //minus icon - index 42
            deleteObj = new GameObject();
            Functions_GameObject.ResetObject(deleteObj);
            deleteObj.compSprite.texture = Assets.mainSheet;
            //set sprite position
            deleteObj.compSprite.position.X = 16 * 5 + 8;
            deleteObj.compSprite.position.Y = 16 * 14;
            Functions_Component.SetZdepth(deleteObj.compSprite);
            //set sprites frame
            deleteObj.compSprite.currentFrame.X = 15;
            deleteObj.compSprite.currentFrame.Y = 15;
            //set collision rec
            deleteObj.compCollision.rec.X = 16 * 5 + 8 - 8;
            deleteObj.compCollision.rec.Y = 16 * 14 - 8;
            //add object to list
            objList.Add(deleteObj);

            #endregion


            objListTotal = objList.Count();

            //initialize the widget
            SetActiveObj(0); //initialize active obj to 0 obj
            SetActiveTool(moveObj); //initialize to move tool
            objToolState = ObjToolState.MoveObj; //initialize to move state
            grabbedObj = null;
        }

        public override void Reset(int X, int Y)
        {   //reset the room builder widget's window
            window.lines[2].position.Y = Y + (16 * 3); //top divider (below current obj display)
            window.lines[3].position.Y = Y + (16 * 12); //bottom divider (above tools)
            Functions_MenuWindow.ResetAndMove(window, X, Y, window.size, window.title.text);

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


            #region Move Grab, Add, and Delete Icons

            offset = 16 * 9;

            moveObj.compSprite.position.X = X + 16 * 1;
            moveObj.compSprite.position.Y = Y + offset + yPos;
            moveObj.compCollision.rec.X = X + 16 * 1 - 8;
            moveObj.compCollision.rec.Y = Y + +offset + yPos - 8;
            
            addObj.compSprite.position.X = X + 16 * 3;
            addObj.compSprite.position.Y = Y + +offset + yPos;
            addObj.compCollision.rec.X = X + 16 * 3 - 8;
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
            //Functions_Debug.HandleDebugMenuInput();


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

            addDeleteSprite.position.X = Input.cursorPos.X + 12;
            addDeleteSprite.position.Y = Input.cursorPos.Y - 0;

            #endregion


            if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
            {   //if mouse is contained within widget
                if (window.interior.rec.Contains(Input.cursorPos))
                {

                    #region Handle Obj / Tool Selection

                    //does any obj on the widget's objList contain the mouse position?
                    for (i = 0; i < objListTotal; i++)
                    {   //if there is a collision, set the active object to the object clicked on
                        if (objList[i].compCollision.rec.Contains(Input.cursorPos))
                        {
                            //handle collision with room obj
                            if (i < 40) { SetActiveObj(i); }
                            //handle collision with tool obj
                            else if (objList[i] == moveObj)
                            {
                                SetActiveTool(moveObj);
                                objToolState = ObjToolState.MoveObj;
                            }
                            else if (objList[i] == addObj)
                            {
                                SetActiveTool(addObj);
                                objToolState = ObjToolState.AddObj;
                                addDeleteSprite.currentFrame.X = 14;
                            }
                            else if (objList[i] == deleteObj)
                            {
                                SetActiveTool(deleteObj);
                                objToolState = ObjToolState.DeleteObj;
                                addDeleteSprite.currentFrame.X = 15;
                            }
                        }
                    }

                    #endregion

                }
                //if mouse worldPos is contained in room, allow add/delete selected object
                else if (Functions_Level.currentRoom.rec.Contains(worldPos))
                {

                    #region Handle Add Object State

                    if (objToolState == ObjToolState.AddObj)
                    {   //place currently selected obj in room, aligned to 16px grid
                        GameObject objRef = Functions_Pool.GetRoomObj();

                        objRef.compMove.newPosition = AlignToGrid(worldPos.X, worldPos.Y);
                        Functions_Movement.Teleport(objRef.compMove,
                            objRef.compMove.newPosition.X, objRef.compMove.newPosition.Y);

                        Functions_GameObject.SetType(objRef, activeObj.type);
                        Functions_Component.Align(objRef.compMove, objRef.compSprite, objRef.compCollision);
                        Functions_Animation.Animate(objRef.compAnim, objRef.compSprite);
                    }

                    #endregion


                    #region Handle Delete Object State

                    else if (objToolState == ObjToolState.DeleteObj)
                    {   //check collisions between worldPos and roomObjs, release any colliding obj
                        for (Pool.roomObjCounter = 0; Pool.roomObjCounter < Pool.roomObjCount; Pool.roomObjCounter++)
                        {
                            if (Pool.roomObjPool[Pool.roomObjCounter].active)
                            {
                                if (Pool.roomObjPool[Pool.roomObjCounter].compCollision.rec.Contains(worldPos))
                                { Functions_Pool.Release(Pool.roomObjPool[Pool.roomObjCounter]); }
                            }
                        }
                    }

                    #endregion

                }

                //objects CAN be moved outside of room

                #region Handle Grab (Move) Object State

                if (objToolState == ObjToolState.MoveObj)
                {   //check collisions between worldPos and roomObjs, grab any colliding obj
                    for (Pool.roomObjCounter = 0; Pool.roomObjCounter < Pool.roomObjCount; Pool.roomObjCounter++)
                    {
                        if (Pool.roomObjPool[Pool.roomObjCounter].active)
                        {
                            if (Pool.roomObjPool[Pool.roomObjCounter].compCollision.rec.Contains(worldPos))
                            { grabbedObj = Pool.roomObjPool[Pool.roomObjCounter]; }
                        }
                    }
                }

                #endregion

            }


            #region Handle Dragging of Grabbed Obj

            if (Functions_Input.IsMouseButtonDown(MouseButtons.LeftButton))
            {   //if we have a grabbedObj, match it to cursorPos if LMB is down
                if (objToolState == ObjToolState.MoveObj)
                {   //match grabbed Obj pos to worldPos, aligned to 16px grid
                    if (grabbedObj != null)
                    {
                        grabbedObj.compMove.newPosition = AlignToGrid(worldPos.X, worldPos.Y);
                        Functions_Movement.Teleport(grabbedObj.compMove,
                            grabbedObj.compMove.newPosition.X, grabbedObj.compMove.newPosition.Y);
                        Functions_Component.Align(grabbedObj.compMove,
                            grabbedObj.compSprite, grabbedObj.compCollision);
                    }
                }
            }

            #endregion


            #region Handle Release Grabbed Obj

            if (Functions_Input.IsNewMouseButtonRelease(MouseButtons.LeftButton))
            { grabbedObj = null; }

            #endregion

        }

        public override void Update()
        {
            Functions_MenuWindow.Update(window);
            if (window.interior.displayState == DisplayState.Opened)
            {
                UpdateSelectionBox(selectionBoxObj);
                UpdateSelectionBox(selectionBoxTool);
                selectionBoxObj.position = activeObj.compSprite.position;
                selectionBoxTool.position = activeTool.compSprite.position;
            }
        }

        public override void Draw()
        {
            Functions_Draw.Draw(window);
            if (window.interior.displayState == DisplayState.Opened)
            {
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
            if (objToolState != ObjToolState.MoveObj) { Functions_Draw.Draw(addDeleteSprite); }
            Functions_Draw.Draw(cursorSprite);
        }



        public void SetActiveObj(int index)
        {
            activeObj = objList[index];
            selectionBoxObj.scale = 2.0f;
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

    }
}