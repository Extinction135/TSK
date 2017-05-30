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

        public GameObject activeObj; //points to an obj in the obj list
        public List<GameObject> objList; //a list of room objects user can select
        public ComponentSprite selectionBox;

        public MenuRectangle divider1;
        public MenuRectangle divider2;



        public WidgetRoomBuilder()
        {
            window = new MenuWindow(
                new Point(8, 16 * 4), 
                new Point(16 * 6, 16 * 14 + 8),
                "Room Builder");

            selectionBox = new ComponentSprite(
                Assets.mainSheet, new Vector2(-100, 5000),
                new Byte4(15, 7, 0, 0), new Point(16, 16));

            divider1 = new MenuRectangle(new Point(0, 0), 
                new Point(0, 0), Assets.colorScheme.windowInset);

            divider2 = new MenuRectangle(new Point(0, 0), 
                new Point(0, 0), Assets.colorScheme.windowInset);

            //create & populate the objList
            objList = new List<GameObject>();
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


                    #region Set the object properly

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
            //set active object to first obj on objList
            SetActiveObj(0);

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
        }

        public override void Update()
        {
            window.Update();
            divider1.Update();
            divider2.Update();

            if (window.interior.displayState == DisplayState.Opened)
            {
                //pulse the selectionBox alpha
                if (selectionBox.alpha >= 1.0f) { selectionBox.alpha = 0.1f; }
                else { selectionBox.alpha += 0.025f; }

                //scale the selectionBox down to 1.0
                if (selectionBox.scale > 1.0f) { selectionBox.scale -= 0.07f; }
                else { selectionBox.scale = 1.0f; }
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
                for (i = 0; i < 5 * 7; i++)
                { Functions_Draw.Draw(objList[i].compSprite); }

                if (Flags.DrawCollisions)
                {   //draw objlist's collision recs
                    for (i = 0; i < 5 * 7; i++)
                    { Functions_Draw.Draw(objList[i].compCollision); }
                }

                Functions_Draw.Draw(selectionBox);
            }
        }



        public void SetActiveObj(int index)
        {
            activeObj = objList[index];
            selectionBox.scale = 2.0f;
            selectionBox.position = activeObj.compSprite.position;
        }

    }
}