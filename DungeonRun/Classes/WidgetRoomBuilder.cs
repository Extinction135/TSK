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
    public class WidgetRoomBuilder : Widget
    {

        public MenuRectangle divider1;
        public MenuRectangle divider2;

        public ComponentSprite selectionBoxObj; //highlites the currently selected obj
        public ComponentSprite selectionBoxTool; //highlites the currently selected obj

        public GameObject activeObj; //points to a RoomObj on the obj list
        public GameObject activeTool; //points to a ToolObj on the obj list

        public List<GameObject> objList; //a list of objects user can select
        public int total;

        public ComponentButton saveBtn;
        public ComponentButton newBtn;
        public ComponentButton loadBtn;


        public WidgetRoomBuilder()
        {

            #region Create Window and Additional Divider lines

            window = new MenuWindow(
                new Point(8, 16 * 4), 
                new Point(16 * 6, 16 * 14 + 8),
                "Room Builder");
            divider1 = new MenuRectangle(new Point(0, 0), 
                new Point(0, 0), Assets.colorScheme.windowInset);
            divider2 = new MenuRectangle(new Point(0, 0), 
                new Point(0, 0), Assets.colorScheme.windowInset);

            #endregion


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
                for (int j = 0; j < 5; j++) //column
                {
                    GameObject obj = new GameObject(Assets.cursedCastleSheet);
                    Functions_GameObject.ResetObject(obj);
                    //set the new position value for the move component
                    obj.compMove.newPosition.X = 16 + 8 + (16 * j);
                    obj.compMove.newPosition.Y = 16 * 6 + (16 * i);
                    Functions_GameObject.SetType(obj, ObjType.WallStraight);


                    #region Set the objects properly

                    if (i == 0) //first row
                    {
                        if (j == 0) { Functions_GameObject.SetType(obj, ObjType.WallPillar); }
                        else if (j == 1) { Functions_GameObject.SetType(obj, ObjType.WallStatue); }
                        else if (j == 2) { Functions_GameObject.SetType(obj, ObjType.BossStatue); }
                        //else if (j == 3) { Functions_GameObject.SetType(obj, ObjType.PitTop); }
                        //else if (j == 4) { Functions_GameObject.SetType(obj, ObjType.FloorBombable); }
                    }
                    else if (i == 1) //second row
                    {
                        //if (j == 0) { Functions_GameObject.SetType(obj, ObjType.WallPillar); }
                        //else if (j == 1) { Functions_GameObject.SetType(obj, ObjType.WallStatue); }
                        //else if (j == 2) { Functions_GameObject.SetType(obj, ObjType.BossStatue); }
                        //else if (j == 3) { Functions_GameObject.SetType(obj, ObjType.PitTop); }
                        //else if (j == 4) { Functions_GameObject.SetType(obj, ObjType.FloorBombable); }
                    }
                    else if (i == 2) //third row
                    {
                        if (j == 0) { Functions_GameObject.SetType(obj, ObjType.BlockSpikes); }
                        else if (j == 1) { Functions_GameObject.SetType(obj, ObjType.Bumper); }
                        else if (j == 2) { Functions_GameObject.SetType(obj, ObjType.Flamethrower); }
                        //else if (j == 3) { Functions_GameObject.SetType(obj, ObjType.PitTop); }
                        //else if (j == 4) { Functions_GameObject.SetType(obj, ObjType.FloorBombable); }
                    }
                    else if (i == 3) //fourth row
                    {
                        //if (j == 0) { Functions_GameObject.SetType(obj, ObjType.WallPillar); }
                        //else if (j == 1) { Functions_GameObject.SetType(obj, ObjType.WallStatue); }
                        if (j == 2) { Functions_GameObject.SetType(obj, ObjType.ConveyorBelt); }
                        //else if (j == 3) { Functions_GameObject.SetType(obj, ObjType.PitTop); }
                        //else if (j == 4) { Functions_GameObject.SetType(obj, ObjType.FloorBombable); }
                    }
                    else if (i == 4) //fifth row
                    {
                        //if (j == 0) { Functions_GameObject.SetType(obj, ObjType.WallPillar); }
                        //else if (j == 1) { Functions_GameObject.SetType(obj, ObjType.WallStatue); }
                        //else if (j == 2) { Functions_GameObject.SetType(obj, ObjType.BossStatue); }
                        //else if (j == 3) { Functions_GameObject.SetType(obj, ObjType.PitTop); }
                        //else if (j == 4) { Functions_GameObject.SetType(obj, ObjType.FloorBombable); }
                    }
                    else if (i == 5) //sixth row
                    {
                        //if (j == 0) { Functions_GameObject.SetType(obj, ObjType.WallPillar); }
                        //else if (j == 1) { Functions_GameObject.SetType(obj, ObjType.WallStatue); }
                        //else if (j == 2) { Functions_GameObject.SetType(obj, ObjType.BossStatue); }
                        //else if (j == 3) { Functions_GameObject.SetType(obj, ObjType.PitTop); }
                        //else if (j == 4) { Functions_GameObject.SetType(obj, ObjType.FloorBombable); }
                    }
                    else if (i == 6) //seventh row
                    {
                        if (j == 0) { Functions_GameObject.SetType(obj, ObjType.PotSkull); }
                        else if (j == 1) { Functions_GameObject.SetType(obj, ObjType.ChestGold); }
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
                    obj.compCollision.rec.Y = 16 * 6 + (16 * i) - 8;
                    //add the object to the list
                    objList.Add(obj); 
                }
            }

            #endregion


            #region Add Enemy objs to ObjList

            //enemy1 - index 35
            GameObject enemy1 = new GameObject(Assets.blobSheet);
            Functions_GameObject.ResetObject(enemy1);
            enemy1.compSprite.position.X = 16 * 1 + 8;
            enemy1.compSprite.position.Y = 16 * 13;
            enemy1.compCollision.rec.X = 16 * 1;
            enemy1.compCollision.rec.Y = 16 * 13 - 8;
            objList.Add(enemy1);

            //enemy2 - index 36
            GameObject enemy2 = new GameObject(Assets.blobSheet);
            Functions_GameObject.ResetObject(enemy2);
            enemy2.compSprite.position.X = 16 * 2 + 8;
            enemy2.compSprite.position.Y = 16 * 13;
            enemy2.compCollision.rec.X = 16 * 2;
            enemy2.compCollision.rec.Y = 16 * 13 - 8;
            objList.Add(enemy2);

            //enemy3 - index 37
            GameObject enemy3 = new GameObject(Assets.blobSheet);
            Functions_GameObject.ResetObject(enemy3);
            enemy3.compSprite.position.X = 16 * 3 + 8;
            enemy3.compSprite.position.Y = 16 * 13;
            enemy3.compCollision.rec.X = 16 * 3;
            enemy3.compCollision.rec.Y = 16 * 13 - 8;
            objList.Add(enemy3);

            //enemy4 - index 38
            GameObject enemy4 = new GameObject(Assets.blobSheet);
            Functions_GameObject.ResetObject(enemy4);
            enemy4.compSprite.position.X = 16 * 4 + 8;
            enemy4.compSprite.position.Y = 16 * 13;
            enemy4.compCollision.rec.X = 16 * 4;
            enemy4.compCollision.rec.Y = 16 * 13 - 8;
            objList.Add(enemy4);

            //enemy4 - index 39
            GameObject enemy5 = new GameObject(Assets.blobSheet);
            Functions_GameObject.ResetObject(enemy5);
            enemy5.compSprite.position.X = 16 * 5 + 8;
            enemy5.compSprite.position.Y = 16 * 13;
            enemy5.compCollision.rec.X = 16 * 5;
            enemy5.compCollision.rec.Y = 16 * 13 - 8;
            objList.Add(enemy5);

            #endregion


            #region Add Toolbar objs to ObjList

            //move icon - index 40
            GameObject moveObj = new GameObject(Assets.mainSheet);
            Functions_GameObject.ResetObject(moveObj);
            //set sprite position
            moveObj.compSprite.position.X = 16 * 1 + 8;
            moveObj.compSprite.position.Y = 16 * 15;
            //set sprites frame
            moveObj.compSprite.currentFrame.X = 14;
            moveObj.compSprite.currentFrame.Y = 13;
            //set collision rec
            moveObj.compCollision.rec.X = 16 * 1 + 8 - 8;
            moveObj.compCollision.rec.Y = 16 * 15 - 8;
            //add object to list
            objList.Add(moveObj);

            //add icon - index 41
            GameObject addObj = new GameObject(Assets.mainSheet);
            Functions_GameObject.ResetObject(addObj);
            //set sprite position
            addObj.compSprite.position.X = 16 * 3 + 8;
            addObj.compSprite.position.Y = 16 * 15;
            //set sprites frame
            addObj.compSprite.currentFrame.X = 14;
            addObj.compSprite.currentFrame.Y = 15;
            //set collision rec
            addObj.compCollision.rec.X = 16 * 3 + 8 - 8;
            addObj.compCollision.rec.Y = 16 * 15 - 8;
            //add object to list
            objList.Add(addObj);

            //minus icon - index 42
            GameObject minusObj = new GameObject(Assets.mainSheet);
            Functions_GameObject.ResetObject(minusObj);
            //set sprite position
            minusObj.compSprite.position.X = 16 * 5 + 8;
            minusObj.compSprite.position.Y = 16 * 15;
            //set sprites frame
            minusObj.compSprite.currentFrame.X = 15;
            minusObj.compSprite.currentFrame.Y = 15;
            //set collision rec
            minusObj.compCollision.rec.X = 16 * 5 + 8 - 8;
            minusObj.compCollision.rec.Y = 16 * 15 - 8;
            //add object to list
            objList.Add(minusObj);

            #endregion


            total = objList.Count();

            saveBtn = new ComponentButton("save", new Point(16 * 1, 16 * 16 + 8));
            newBtn = new ComponentButton("new", new Point(16 * 2 + 13, 16 * 16 + 8));
            loadBtn = new ComponentButton("load", new Point(16 * 4 + 10, 16 * 16 + 8));
        }

        public override void Reset(int X, int Y)
        {
            //reset the room builder widget's window
            window.background.Reset();
            window.border.Reset();
            window.inset.Reset();
            window.interior.Reset();
            window.headerLine.Reset();
            window.footerLine.Reset();
            
            //reset the divider lines
            divider1.position.X = 16;
            divider1.position.Y = 16 * 14;
            divider1.size.X = 16 * 5;
            divider1.size.Y = 1;
            divider1.Reset();
            divider1.openDelay = 12;

            divider2.position.X = 16;
            divider2.position.Y = 16 * 16;
            divider2.size.X = 16 * 5;
            divider2.size.Y = 1;
            divider2.Reset();
            divider2.openDelay = 12;

            //set active object to first obj on objList
            SetActiveObj(0);
            //set active tool to move tool
            SetActiveTool(40);
        }

        public override void Update()
        {
            window.Update();
            divider1.Update();
            divider2.Update();

            if (window.interior.displayState == DisplayState.Opened)
            {
                UpdateSelectionBox(selectionBoxObj);
                UpdateSelectionBox(selectionBoxTool);
            }
        }

        public override void Draw()
        {
            Functions_Draw.Draw(window);
            Functions_Draw.Draw(divider1);
            Functions_Draw.Draw(divider2);

            if (window.interior.displayState == DisplayState.Opened)
            {
                //draw objlist's sprites
                for (i = 0; i < total; i++)
                { Functions_Draw.Draw(objList[i].compSprite); }

                if (Flags.DrawCollisions)
                {   //draw objlist's collision recs
                    for (i = 0; i < total; i++)
                    { Functions_Draw.Draw(objList[i].compCollision); }
                }

                Functions_Draw.Draw(selectionBoxObj);
                Functions_Draw.Draw(selectionBoxTool);

                Functions_Draw.Draw(saveBtn);
                Functions_Draw.Draw(newBtn);
                Functions_Draw.Draw(loadBtn);
            }
        }



        public void SetActiveObj(int index)
        {
            activeObj = objList[index];
            selectionBoxObj.scale = 2.0f;
            selectionBoxObj.position = activeObj.compSprite.position;
        }

        public void SetActiveTool(int index)
        {
            activeTool = objList[index];
            selectionBoxTool.scale = 2.0f;
            selectionBoxTool.position = activeTool.compSprite.position;
        }

        public void UpdateSelectionBox(ComponentSprite SelectionBox)
        {
            //pulse the selectionBox alpha
            if (SelectionBox.alpha >= 1.0f) { SelectionBox.alpha = 0.1f; }
            else { SelectionBox.alpha += 0.025f; }
            //scale the selectionBox down to 1.0
            if (SelectionBox.scale > 1.0f) { SelectionBox.scale -= 0.07f; }
            else { SelectionBox.scale = 1.0f; }
        }

    }
}