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




        public WidgetRoomBuilder()
        {
            window = new MenuWindow(
                new Point(8, 16 * 4), 
                new Point(16 * 6, 16 * 14 + 8),
                "Room Builder");

            objList = new List<GameObject>();

            //populate the objList
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

        public override void Update()
        {
            window.Update();
        }

        public override void Draw()
        {
            Functions_Draw.Draw(window);

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
            }
        }

    }
}