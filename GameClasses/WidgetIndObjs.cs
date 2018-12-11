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

    public static class Functions_WidgetIndObjs
    {
        static int i = 0;
        static int row = 0;
        static int counter = 0;

        public static void PositionObjs(WidgetIndObject WD)
        {
            counter = 0;
            for (row = 0; row < 12; row++)
            {   //12 rows, with 4 objs per row
                for (i = 0; i < 4; i++)
                {
                    //obj could be null
                    if (WD.objList[counter] != null)
                    {   //move gameObj relative to interior window rec

                        WD.objList[counter].compSprite.position.X = WD.window.interior.rec.X + 13 + (i * 16) + (0);
                        WD.objList[counter].compSprite.position.Y = WD.window.interior.rec.Y + 29 + (row * 16) + (0);

                        //manually set the collisionRecs (they need to be 16x16, default aligned for editor use)
                        WD.objList[counter].compCollision.rec.X = (int)WD.objList[counter].compSprite.position.X - 8;
                        WD.objList[counter].compCollision.rec.Y = (int)WD.objList[counter].compSprite.position.Y - 8;
                        WD.objList[counter].compCollision.rec.Width = 16;
                        WD.objList[counter].compCollision.rec.Height = 16;
                    }

                    counter++;
                }
            }
        }

        public static void HideObj(IndestructibleObject Obj)
        {   //hide this widget object offscreen
            Obj.compSprite.position.X = 2048;
            Obj.compCollision.rec.X = 2048;
            Obj.compCollision.rec.Width = 1;
        }
    }



    public class WidgetIndObject : Widget
    {
        public List<IndestructibleObject> objList;
        public Boolean visible = false;

        public override void Reset(int X, int Y)
        {
            Functions_MenuWindow.ResetAndMove(window, X, Y, window.size, window.title.text);
        }

        public override void Update()
        {
            Functions_MenuWindow.Update(window);
            if (window.interior.displayState == DisplayState.Opened)
            {   //animate all the game objects
                for (i = 0; i < objList.Count; i++)
                {
                    if (objList[i] != null)
                    { Functions_Animation.Animate(objList[i].compAnim, objList[i].compSprite); }
                }
            }
        }

        public override void Draw()
        {
            Functions_Draw.Draw(window);
            if (window.interior.displayState == DisplayState.Opened)
            {
                for (i = 0; i < objList.Count; i++)
                { if (objList[i] != null) { Functions_Draw.Draw(objList[i].compSprite); } }
                if (Flags.DrawCollisions) //draw objlist's collision recs
                {
                    for (i = 0; i < objList.Count; i++)
                    { if (objList[i] != null) { Functions_Draw.Draw(objList[i].compCollision); } }
                }
            }
        }

    }















    //interactive objects display set

    public class WidgetIndestructibleObjs_BoatA : WidgetIndObject
    {
        public WidgetIndestructibleObjs_BoatA()
        {   //1st left
            window = new MenuWindow(
                new Point(16 * 1, 16 * 2), //position
                new Point(16 * 5, 16 * 15), //size
                "Boat A"); //title

            objList = new List<IndestructibleObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new IndestructibleObject()); }

            //row 1
            Functions_IndestructibleObjs.SetType(objList[0], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[1], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[2], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[3], IndestructibleType.Dungeon_BlockDark);

            //row 2
            Functions_IndestructibleObjs.SetType(objList[4], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[5], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[6], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[7], IndestructibleType.Dungeon_BlockDark);

            //row 3
            Functions_IndestructibleObjs.SetType(objList[8], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[9], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[10], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[11], IndestructibleType.Dungeon_BlockDark);

            //row 4
            Functions_IndestructibleObjs.SetType(objList[12], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[13], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[14], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[15], IndestructibleType.Dungeon_BlockDark);

            //row 5
            Functions_IndestructibleObjs.SetType(objList[16], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[17], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[18], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[19], IndestructibleType.Dungeon_BlockDark);

            //row 6
            Functions_IndestructibleObjs.SetType(objList[20], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[21], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[22], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[23], IndestructibleType.Dungeon_BlockDark);

            //row 7
            Functions_IndestructibleObjs.SetType(objList[24], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[25], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[26], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[27], IndestructibleType.Dungeon_BlockDark);

            //row 8
            Functions_IndestructibleObjs.SetType(objList[28], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[29], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[30], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[31], IndestructibleType.Dungeon_BlockDark);

            //row 9
            Functions_IndestructibleObjs.SetType(objList[32], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[33], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[34], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[35], IndestructibleType.Dungeon_BlockDark);

            //row 10
            Functions_IndestructibleObjs.SetType(objList[36], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[37], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[38], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[39], IndestructibleType.Dungeon_BlockDark);

            //row 11
            Functions_IndestructibleObjs.SetType(objList[40], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[41], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[42], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[43], IndestructibleType.Dungeon_BlockDark);

            //row 12
            Functions_IndestructibleObjs.SetType(objList[44], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[45], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[46], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[47], IndestructibleType.Dungeon_BlockDark);

            //position the objs relative to the window frame
            Functions_WidgetIndObjs.PositionObjs(this);

            //manually set recs
            //objList[8].compCollision.rec.Height = 16 * 2;
            //objList[9].compCollision.rec.Height = 16 * 2;

            //hide objs that trees overlap
            Functions_WidgetIndObjs.HideObj(objList[12]);

            //reset update all zdepths
            for (i = 0; i < 4 * 12; i++)
            {
                objList[i].compSprite.zOffset = 0;
                Functions_Component.SetZdepth(objList[i].compSprite);
            }
        }
    }


    public class WidgetIndestructibleObjs_BoatB : WidgetIndObject
    {
        public WidgetIndestructibleObjs_BoatB()
        {   //1st left
            window = new MenuWindow(
                new Point(16 * 6 + 8, 16 * 2), //position
                new Point(16 * 5, 16 * 15), //size
                "Boat B"); //title

            objList = new List<IndestructibleObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new IndestructibleObject()); }

            //row 1
            Functions_IndestructibleObjs.SetType(objList[0], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[1], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[2], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[3], IndestructibleType.Dungeon_BlockDark);

            //row 2
            Functions_IndestructibleObjs.SetType(objList[4], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[5], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[6], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[7], IndestructibleType.Dungeon_BlockDark);

            //row 3
            Functions_IndestructibleObjs.SetType(objList[8], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[9], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[10], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[11], IndestructibleType.Dungeon_BlockDark);

            //row 4
            Functions_IndestructibleObjs.SetType(objList[12], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[13], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[14], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[15], IndestructibleType.Dungeon_BlockDark);

            //row 5
            Functions_IndestructibleObjs.SetType(objList[16], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[17], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[18], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[19], IndestructibleType.Dungeon_BlockDark);

            //row 6
            Functions_IndestructibleObjs.SetType(objList[20], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[21], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[22], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[23], IndestructibleType.Dungeon_BlockDark);

            //row 7
            Functions_IndestructibleObjs.SetType(objList[24], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[25], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[26], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[27], IndestructibleType.Dungeon_BlockDark);

            //row 8
            Functions_IndestructibleObjs.SetType(objList[28], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[29], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[30], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[31], IndestructibleType.Dungeon_BlockDark);

            //row 9
            Functions_IndestructibleObjs.SetType(objList[32], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[33], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[34], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[35], IndestructibleType.Dungeon_BlockDark);

            //row 10
            Functions_IndestructibleObjs.SetType(objList[36], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[37], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[38], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[39], IndestructibleType.Dungeon_BlockDark);

            //row 11
            Functions_IndestructibleObjs.SetType(objList[40], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[41], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[42], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[43], IndestructibleType.Dungeon_BlockDark);

            //row 12
            Functions_IndestructibleObjs.SetType(objList[44], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[45], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[46], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[47], IndestructibleType.Dungeon_BlockDark);

            //position the objs relative to the window frame
            Functions_WidgetIndObjs.PositionObjs(this);

            //manually set recs
            //objList[8].compCollision.rec.Height = 16 * 2;
            //objList[9].compCollision.rec.Height = 16 * 2;

            //hide objs that trees overlap
            //Functions_WidgetIndObjs.HideObj(objList[12]);

            //reset update all zdepths
            for (i = 0; i < 4 * 12; i++)
            {
                objList[i].compSprite.zOffset = 0;
                Functions_Component.SetZdepth(objList[i].compSprite);
            }
        }
    }


    public class WidgetIndestructibleObjs_Forest : WidgetIndObject
    {
        public WidgetIndestructibleObjs_Forest()
        {   //1st left
            window = new MenuWindow(
                new Point(16 * 12, 16 * 2), //position
                new Point(16 * 5, 16 * 15), //size
                "Forest"); //title

            objList = new List<IndestructibleObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new IndestructibleObject()); }

            //row 1
            Functions_IndestructibleObjs.SetType(objList[0], IndestructibleType.ForestDungeon_Entrance);
            Functions_IndestructibleObjs.SetType(objList[1], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[2], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[3], IndestructibleType.Dungeon_BlockDark);

            //row 2
            Functions_IndestructibleObjs.SetType(objList[4], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[5], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[6], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[7], IndestructibleType.Dungeon_BlockDark);

            //row 3
            Functions_IndestructibleObjs.SetType(objList[8], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[9], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[10], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[11], IndestructibleType.Dungeon_BlockDark);

            //row 4
            Functions_IndestructibleObjs.SetType(objList[12], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[13], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[14], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[15], IndestructibleType.Dungeon_BlockDark);

            //row 5
            Functions_IndestructibleObjs.SetType(objList[16], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[17], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[18], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[19], IndestructibleType.Dungeon_BlockDark);

            //row 6
            Functions_IndestructibleObjs.SetType(objList[20], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[21], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[22], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[23], IndestructibleType.Dungeon_BlockDark);

            //row 7
            Functions_IndestructibleObjs.SetType(objList[24], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[25], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[26], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[27], IndestructibleType.Dungeon_BlockDark);

            //row 8
            Functions_IndestructibleObjs.SetType(objList[28], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[29], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[30], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[31], IndestructibleType.Dungeon_BlockDark);

            //row 9
            Functions_IndestructibleObjs.SetType(objList[32], IndestructibleType.Tree_Big);
            Functions_IndestructibleObjs.SetType(objList[33], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[34], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[35], IndestructibleType.Dungeon_BlockDark);//

            //row 10
            Functions_IndestructibleObjs.SetType(objList[36], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[37], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[38], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[39], IndestructibleType.Dungeon_BlockDark);//

            //row 11
            Functions_IndestructibleObjs.SetType(objList[40], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[41], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[42], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[43], IndestructibleType.Dungeon_BlockDark);//

            //row 12
            Functions_IndestructibleObjs.SetType(objList[44], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[45], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[46], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[47], IndestructibleType.Dungeon_BlockDark);//

            //position the objs relative to the window frame
            Functions_WidgetIndObjs.PositionObjs(this);

            //manually set recs
            objList[0].compCollision.rec.Width = 16 * 3;
            objList[0].compCollision.rec.Height = 16 * 4;

            //hide objs that trees overlap
            Functions_WidgetIndObjs.HideObj(objList[1]);
            Functions_WidgetIndObjs.HideObj(objList[2]);

            Functions_WidgetIndObjs.HideObj(objList[4]);
            Functions_WidgetIndObjs.HideObj(objList[5]);
            Functions_WidgetIndObjs.HideObj(objList[6]);

            Functions_WidgetIndObjs.HideObj(objList[8]);
            Functions_WidgetIndObjs.HideObj(objList[9]);
            Functions_WidgetIndObjs.HideObj(objList[10]);

            Functions_WidgetIndObjs.HideObj(objList[12]);
            Functions_WidgetIndObjs.HideObj(objList[13]);
            Functions_WidgetIndObjs.HideObj(objList[14]);



            //manually set big tree rec
            objList[32].compCollision.rec.Width = 16 * 4;
            objList[32].compCollision.rec.Height = 16 * 5;
            //big tree hides these objs
            Functions_WidgetIndObjs.HideObj(objList[33]);
            Functions_WidgetIndObjs.HideObj(objList[34]);
            Functions_WidgetIndObjs.HideObj(objList[35]);
            Functions_WidgetIndObjs.HideObj(objList[36]);
            Functions_WidgetIndObjs.HideObj(objList[37]);
            Functions_WidgetIndObjs.HideObj(objList[38]);
            Functions_WidgetIndObjs.HideObj(objList[39]);
            Functions_WidgetIndObjs.HideObj(objList[40]);
            Functions_WidgetIndObjs.HideObj(objList[41]);
            Functions_WidgetIndObjs.HideObj(objList[42]);
            Functions_WidgetIndObjs.HideObj(objList[43]);
            Functions_WidgetIndObjs.HideObj(objList[44]);
            Functions_WidgetIndObjs.HideObj(objList[45]);
            Functions_WidgetIndObjs.HideObj(objList[46]);
            Functions_WidgetIndObjs.HideObj(objList[47]);




            //reset update all zdepths
            for (i = 0; i < 4 * 12; i++)
            {
                objList[i].compSprite.zOffset = 0;
                Functions_Component.SetZdepth(objList[i].compSprite);
            }
        }
    }


    public class WidgetIndestructibleObjs_Mountain : WidgetIndObject
    {
        public WidgetIndestructibleObjs_Mountain()
        {   //1st left
            window = new MenuWindow(
                new Point(16 * 17 + 8, 16 * 2), //position
                new Point(16 * 5, 16 * 15), //size
                "Mountain"); //title

            objList = new List<IndestructibleObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new IndestructibleObject()); }

            //row 1
            Functions_IndestructibleObjs.SetType(objList[0], IndestructibleType.MountainDungeon_Entrance);
            Functions_IndestructibleObjs.SetType(objList[1], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[2], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[3], IndestructibleType.Dungeon_BlockDark);

            //row 2
            Functions_IndestructibleObjs.SetType(objList[4], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[5], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[6], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[7], IndestructibleType.Dungeon_BlockDark);

            //row 3
            Functions_IndestructibleObjs.SetType(objList[8], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[9], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[10], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[11], IndestructibleType.Dungeon_BlockDark);

            //row 4
            Functions_IndestructibleObjs.SetType(objList[12], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[13], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[14], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[15], IndestructibleType.Dungeon_BlockDark);

            //row 5
            Functions_IndestructibleObjs.SetType(objList[16], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[17], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[18], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[19], IndestructibleType.Dungeon_BlockDark);

            //row 6
            Functions_IndestructibleObjs.SetType(objList[20], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[21], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[22], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[23], IndestructibleType.Dungeon_BlockDark);

            //row 7
            Functions_IndestructibleObjs.SetType(objList[24], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[25], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[26], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[27], IndestructibleType.Dungeon_BlockDark);

            //row 8
            Functions_IndestructibleObjs.SetType(objList[28], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[29], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[30], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[31], IndestructibleType.Dungeon_BlockDark);

            //row 9
            Functions_IndestructibleObjs.SetType(objList[32], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[33], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[34], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[35], IndestructibleType.Dungeon_BlockDark);

            //row 10
            Functions_IndestructibleObjs.SetType(objList[36], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[37], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[38], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[39], IndestructibleType.Dungeon_BlockDark);

            //row 11
            Functions_IndestructibleObjs.SetType(objList[40], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[41], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[42], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[43], IndestructibleType.Dungeon_BlockDark);

            //row 12
            Functions_IndestructibleObjs.SetType(objList[44], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[45], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[46], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[47], IndestructibleType.Dungeon_BlockDark);

            //position the objs relative to the window frame
            Functions_WidgetIndObjs.PositionObjs(this);

            //manually set recs
            objList[0].compCollision.rec.Width = 16 * 2;
            objList[0].compCollision.rec.Height = 16 * 4;

            //hide objs that entrance overlaps
            Functions_WidgetIndObjs.HideObj(objList[1]);

            Functions_WidgetIndObjs.HideObj(objList[4]);
            Functions_WidgetIndObjs.HideObj(objList[5]);

            Functions_WidgetIndObjs.HideObj(objList[8]);
            Functions_WidgetIndObjs.HideObj(objList[9]);

            Functions_WidgetIndObjs.HideObj(objList[12]);
            Functions_WidgetIndObjs.HideObj(objList[13]);



            //reset update all zdepths
            for (i = 0; i < 4 * 12; i++)
            {
                objList[i].compSprite.zOffset = 0;
                Functions_Component.SetZdepth(objList[i].compSprite);
            }
        }
    }


    public class WidgetIndestructibleObjs_Swamp : WidgetIndObject
    {
        public WidgetIndestructibleObjs_Swamp()
        {   //1st left
            window = new MenuWindow(
                new Point(16 * 23, 16 * 2), //position
                new Point(16 * 5, 16 * 15), //size
                "Swamp"); //title

            objList = new List<IndestructibleObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new IndestructibleObject()); }

            //row 1
            Functions_IndestructibleObjs.SetType(objList[0], IndestructibleType.SwampDungeon_Entrance);
            Functions_IndestructibleObjs.SetType(objList[1], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[2], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[3], IndestructibleType.Dungeon_BlockDark);

            //row 2
            Functions_IndestructibleObjs.SetType(objList[4], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[5], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[6], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[7], IndestructibleType.Dungeon_BlockDark);

            //row 3
            Functions_IndestructibleObjs.SetType(objList[8], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[9], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[10], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[11], IndestructibleType.Dungeon_BlockDark);

            //row 4
            Functions_IndestructibleObjs.SetType(objList[12], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[13], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[14], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[15], IndestructibleType.Dungeon_BlockDark);

            //row 5
            Functions_IndestructibleObjs.SetType(objList[16], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[17], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[18], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[19], IndestructibleType.Dungeon_BlockDark);

            //row 6
            Functions_IndestructibleObjs.SetType(objList[20], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[21], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[22], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[23], IndestructibleType.Dungeon_BlockDark);

            //row 7
            Functions_IndestructibleObjs.SetType(objList[24], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[25], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[26], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[27], IndestructibleType.Dungeon_BlockDark);

            //row 8
            Functions_IndestructibleObjs.SetType(objList[28], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[29], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[30], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[31], IndestructibleType.Dungeon_BlockDark);

            //row 9
            Functions_IndestructibleObjs.SetType(objList[32], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[33], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[34], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[35], IndestructibleType.Dungeon_BlockDark);

            //row 10
            Functions_IndestructibleObjs.SetType(objList[36], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[37], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[38], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[39], IndestructibleType.Dungeon_BlockDark);

            //row 11
            Functions_IndestructibleObjs.SetType(objList[40], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[41], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[42], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[43], IndestructibleType.Dungeon_BlockDark);

            //row 12
            Functions_IndestructibleObjs.SetType(objList[44], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[45], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[46], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[47], IndestructibleType.Dungeon_BlockDark);

            //position the objs relative to the window frame
            Functions_WidgetIndObjs.PositionObjs(this);

            //manually set recs
            objList[0].compCollision.rec.Width = 16 * 3;
            objList[0].compCollision.rec.Height = 16 * 4;

            //hide objs that overlap
            Functions_WidgetIndObjs.HideObj(objList[1]);
            Functions_WidgetIndObjs.HideObj(objList[2]);

            Functions_WidgetIndObjs.HideObj(objList[4]);
            Functions_WidgetIndObjs.HideObj(objList[5]);
            Functions_WidgetIndObjs.HideObj(objList[6]);

            Functions_WidgetIndObjs.HideObj(objList[8]);
            Functions_WidgetIndObjs.HideObj(objList[9]);
            Functions_WidgetIndObjs.HideObj(objList[10]);

            Functions_WidgetIndObjs.HideObj(objList[12]);
            Functions_WidgetIndObjs.HideObj(objList[13]);
            Functions_WidgetIndObjs.HideObj(objList[14]);

            //reset update all zdepths
            for (i = 0; i < 4 * 12; i++)
            {
                objList[i].compSprite.zOffset = 0;
                Functions_Component.SetZdepth(objList[i].compSprite);
            }
        }
    }


    public class WidgetIndestructibleObjs_Coliseum : WidgetIndObject
    {
        public WidgetIndestructibleObjs_Coliseum()
        {   //1st left
            window = new MenuWindow(
                new Point(16 * 28 + 8, 16 * 2), //position
                new Point(16 * 5, 16 * 15), //size
                "Coliseum"); //title

            objList = new List<IndestructibleObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new IndestructibleObject()); }

            //row 1
            Functions_IndestructibleObjs.SetType(objList[0], IndestructibleType.Coliseum_Shadow_Entrance);
            Functions_IndestructibleObjs.SetType(objList[1], IndestructibleType.Dungeon_BlockDark);//ent
            Functions_IndestructibleObjs.SetType(objList[2], IndestructibleType.Dungeon_BlockDark);//ent
            Functions_IndestructibleObjs.SetType(objList[3], IndestructibleType.Coliseum_Shadow_Pillar_Top);

            //row 2
            Functions_IndestructibleObjs.SetType(objList[4], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[5], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[6], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[7], IndestructibleType.Coliseum_Shadow_Pillar_Middle);

            //row 3
            Functions_IndestructibleObjs.SetType(objList[8], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[9], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[10], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[11], IndestructibleType.Coliseum_Shadow_Pillar_Bottom);

            //row 4
            Functions_IndestructibleObjs.SetType(objList[12], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[13], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[14], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[15], IndestructibleType.Coliseum_Shadow_Stairs_Handrail_Top);

            //row 5
            Functions_IndestructibleObjs.SetType(objList[16], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[17], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[18], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[19], IndestructibleType.Coliseum_Shadow_Stairs_Handrail_Middle);

            //row 6
            Functions_IndestructibleObjs.SetType(objList[20], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[21], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[22], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[23], IndestructibleType.Coliseum_Shadow_Stairs_Handrail_Bottom);

            //row 7
            Functions_IndestructibleObjs.SetType(objList[24], IndestructibleType.Coliseum_Shadow_Spectator);
            Functions_IndestructibleObjs.SetType(objList[25], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[26], IndestructibleType.Dungeon_BlockDark);//
            Functions_IndestructibleObjs.SetType(objList[27], IndestructibleType.Dungeon_BlockDark);//

            //row 8
            Functions_IndestructibleObjs.SetType(objList[28], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[29], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[30], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[31], IndestructibleType.Dungeon_BlockDark);

            //row 9
            Functions_IndestructibleObjs.SetType(objList[32], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[33], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[34], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[35], IndestructibleType.Dungeon_BlockDark);

            //row 10
            Functions_IndestructibleObjs.SetType(objList[36], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[37], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[38], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[39], IndestructibleType.Dungeon_BlockDark);

            //row 11
            Functions_IndestructibleObjs.SetType(objList[40], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[41], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[42], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[43], IndestructibleType.Dungeon_BlockDark);

            //row 12
            Functions_IndestructibleObjs.SetType(objList[44], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[45], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[46], IndestructibleType.Dungeon_BlockDark);
            Functions_IndestructibleObjs.SetType(objList[47], IndestructibleType.Dungeon_BlockDark);

            //position the objs relative to the window frame
            Functions_WidgetIndObjs.PositionObjs(this);

            //set recs for entrance
            objList[0].compCollision.rec.Width = 16 * 3;
            objList[0].compCollision.rec.Height = 16 * 4;
            //spectators
            objList[24].compCollision.rec.Width = 16 * 4;




            //hide objs that trees overlap
            Functions_WidgetIndObjs.HideObj(objList[1]);
            Functions_WidgetIndObjs.HideObj(objList[2]);

            Functions_WidgetIndObjs.HideObj(objList[4]);
            Functions_WidgetIndObjs.HideObj(objList[5]);
            Functions_WidgetIndObjs.HideObj(objList[6]);

            Functions_WidgetIndObjs.HideObj(objList[8]);
            Functions_WidgetIndObjs.HideObj(objList[9]);
            Functions_WidgetIndObjs.HideObj(objList[10]);

            Functions_WidgetIndObjs.HideObj(objList[12]);
            Functions_WidgetIndObjs.HideObj(objList[13]);
            Functions_WidgetIndObjs.HideObj(objList[14]);

            Functions_WidgetIndObjs.HideObj(objList[25]);
            Functions_WidgetIndObjs.HideObj(objList[26]);
            Functions_WidgetIndObjs.HideObj(objList[27]);

            //reset update all zdepths
            for (i = 0; i < 4 * 12; i++)
            {
                objList[i].compSprite.zOffset = 0;
                Functions_Component.SetZdepth(objList[i].compSprite);
            }
        }
    }












    /*





    

    public class WidgetObjects_Colliseum : WidgetObject
    {
        public WidgetObjects_Colliseum()
        {   //create and set the position of the window frame
            window = new MenuWindow(
                new Point(16 * 12, 16 * 2), //position
                new Point(16 * 5, 16 * 15), //size
                "Colliseum Objs"); //title

            objList = new List<InteractiveObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new InteractiveObject()); }

            //row 1
            //Functions_InteractiveObjs.SetType(objList[0], InteractiveType.Wor_Entrance_Colliseum);
            Functions_InteractiveObjs.SetType(objList[1], InteractiveType.Unknown);//ent
            Functions_InteractiveObjs.SetType(objList[2], InteractiveType.Unknown);//ent
            //Functions_InteractiveObjs.SetType(objList[3], InteractiveType.Wor_Colliseum_Pillar_Top);

            //row 2
            Functions_InteractiveObjs.SetType(objList[4], InteractiveType.Unknown);//ent
            Functions_InteractiveObjs.SetType(objList[5], InteractiveType.Unknown);//ent
            Functions_InteractiveObjs.SetType(objList[6], InteractiveType.Unknown);//ent
            //Functions_InteractiveObjs.SetType(objList[7], InteractiveType.Wor_Colliseum_Pillar_Middle);

            //row 3
            Functions_InteractiveObjs.SetType(objList[8], InteractiveType.Unknown);//ent
            Functions_InteractiveObjs.SetType(objList[9], InteractiveType.Unknown);//ent
            Functions_InteractiveObjs.SetType(objList[10], InteractiveType.Unknown);//ent
            //Functions_InteractiveObjs.SetType(objList[11], InteractiveType.Wor_Colliseum_Pillar_Bottom);

            //row 4
            Functions_InteractiveObjs.SetType(objList[12], InteractiveType.Unknown);//ent
            Functions_InteractiveObjs.SetType(objList[13], InteractiveType.Unknown);//ent
            Functions_InteractiveObjs.SetType(objList[14], InteractiveType.Unknown);//ent
            //Functions_InteractiveObjs.SetType(objList[15], InteractiveType.Wor_Colliseum_Outdoors_Floor);

            //row 5
            //Functions_InteractiveObjs.SetType(objList[16], InteractiveType.Wor_Colliseum_Bricks_Left);
            //Functions_InteractiveObjs.SetType(objList[17], InteractiveType.Wor_Colliseum_Bricks_Middle1);
            //Functions_InteractiveObjs.SetType(objList[18], InteractiveType.Wor_Colliseum_Bricks_Middle2);
            //Functions_InteractiveObjs.SetType(objList[19], InteractiveType.Wor_Colliseum_Bricks_Right);

            //row 6
            Functions_InteractiveObjs.SetType(objList[20], InteractiveType.Unknown);//brix
            Functions_InteractiveObjs.SetType(objList[21], InteractiveType.Unknown);//brix
            Functions_InteractiveObjs.SetType(objList[22], InteractiveType.Unknown);//brix
            Functions_InteractiveObjs.SetType(objList[23], InteractiveType.Unknown);//brix




            //row 7
            //Functions_InteractiveObjs.SetType(objList[24], InteractiveType.Wor_Colliseum_Spectator);
            Functions_InteractiveObjs.SetType(objList[25], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[26], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[27], InteractiveType.Unknown);

            //row 8
            Functions_InteractiveObjs.SetType(objList[28], InteractiveType.Vendor_Colliseum_Mob);
            Functions_InteractiveObjs.SetType(objList[29], InteractiveType.Vendor_EnemyItems);
            Functions_InteractiveObjs.SetType(objList[30], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[31], InteractiveType.Unknown);

            //row 9
            //Functions_InteractiveObjs.SetType(objList[32], InteractiveType.Wor_Colliseum_Stairs_Handrail_Top);
            Functions_InteractiveObjs.SetType(objList[33], InteractiveType.Coliseum_Shadow_Stairs_Left);
            Functions_InteractiveObjs.SetType(objList[34], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[35], InteractiveType.Unknown);

            //row 10
            //Functions_InteractiveObjs.SetType(objList[36], InteractiveType.Wor_Colliseum_Stairs_Handrail_Middle);
            Functions_InteractiveObjs.SetType(objList[37], InteractiveType.Coliseum_Shadow_Stairs_Middle);
            Functions_InteractiveObjs.SetType(objList[38], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[39], InteractiveType.Unknown);

            //row 11
            //Functions_InteractiveObjs.SetType(objList[40], InteractiveType.Wor_Colliseum_Stairs_Handrail_Bottom);
            Functions_InteractiveObjs.SetType(objList[41], InteractiveType.Coliseum_Shadow_Stairs_Right);
            Functions_InteractiveObjs.SetType(objList[42], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[43], InteractiveType.Unknown);

            //row 12 - cheat and put these beyond widget bounds cause they big
            Functions_InteractiveObjs.SetType(objList[44], InteractiveType.Coliseum_Shadow_Gate_Pillar_Left);
            Functions_InteractiveObjs.SetType(objList[45], InteractiveType.Unknown);//covered by left pillar
            Functions_InteractiveObjs.SetType(objList[46], InteractiveType.Coliseum_Shadow_Gate_Center);
            Functions_InteractiveObjs.SetType(objList[47], InteractiveType.Coliseum_Shadow_Gate_Pillar_Right);//

            //position the objs relative to the window frame
            Functions_WidgetObject.PositionObjs(this);



            //manually set the dungeon entrances collision rec
            objList[0].compCollision.rec.Width = 16 * 3;
            objList[0].compCollision.rec.Height = 16 * 4;




            //hide the dungeon objs covered by dungeon entrance
            Functions_WidgetObject.HideObj(objList[1]);
            Functions_WidgetObject.HideObj(objList[2]);
            Functions_WidgetObject.HideObj(objList[4]);
            Functions_WidgetObject.HideObj(objList[5]);
            Functions_WidgetObject.HideObj(objList[6]);
            Functions_WidgetObject.HideObj(objList[8]);
            Functions_WidgetObject.HideObj(objList[9]);
            Functions_WidgetObject.HideObj(objList[10]);
            Functions_WidgetObject.HideObj(objList[12]);
            Functions_WidgetObject.HideObj(objList[13]);
            Functions_WidgetObject.HideObj(objList[14]);

            //hide objs covered by brix
            Functions_WidgetObject.HideObj(objList[20]);
            Functions_WidgetObject.HideObj(objList[21]);
            Functions_WidgetObject.HideObj(objList[22]);
            Functions_WidgetObject.HideObj(objList[23]);

            //hide objs covered by gates
            Functions_WidgetObject.HideObj(objList[45]);




        }
    }

    public class WidgetObjects_Boat_Front : WidgetObject
    {
        public WidgetObjects_Boat_Front()
        {   //create and set the position of the window frame
            window = new MenuWindow(
                new Point(16 * 12, 16 * 2), //position
                new Point(16 * 5, 16 * 15), //size
                "Boat Front Objs"); //title

            objList = new List<InteractiveObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new InteractiveObject()); }

            //row 1
            //Functions_InteractiveObjs.SetType(objList[0], InteractiveType.Boat_Front_Left); //ind
            Functions_InteractiveObjs.SetType(objList[1], InteractiveType.Unknown);//
            //Functions_InteractiveObjs.SetType(objList[2], InteractiveType.Boat_Front_Right);//ind
            Functions_InteractiveObjs.SetType(objList[3], InteractiveType.Unknown);//

            //row 2
            Functions_InteractiveObjs.SetType(objList[4], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[5], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[6], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[7], InteractiveType.Unknown);//

            //row 3
            Functions_InteractiveObjs.SetType(objList[8], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[9], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[10], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[11], InteractiveType.Unknown);//

            //row 4
            //Functions_InteractiveObjs.SetType(objList[12], InteractiveType.Wor_Boat_Front_ConnectorLeft);
            Functions_InteractiveObjs.SetType(objList[13], InteractiveType.Unknown);//
            //Functions_InteractiveObjs.SetType(objList[14], InteractiveType.Wor_Boat_Front_ConnectorRight);
            Functions_InteractiveObjs.SetType(objList[15], InteractiveType.Unknown);//

            //row 5
            //Functions_InteractiveObjs.SetType(objList[16], InteractiveType.Wor_Boat_Bannister_Left);
            Functions_InteractiveObjs.SetType(objList[17], InteractiveType.Unknown);//
            //Functions_InteractiveObjs.SetType(objList[18], InteractiveType.Wor_Boat_Bannister_Right);
            Functions_InteractiveObjs.SetType(objList[19], InteractiveType.Unknown);//

            //row 6
            //Functions_InteractiveObjs.SetType(objList[20], InteractiveType.Wor_Boat_Stairs_Top_Left);
            Functions_InteractiveObjs.SetType(objList[21], InteractiveType.Unknown);//
            //Functions_InteractiveObjs.SetType(objList[22], InteractiveType.Wor_Boat_Stairs_Top_Right);
            Functions_InteractiveObjs.SetType(objList[23], InteractiveType.Unknown);//

            //row 7
            Functions_InteractiveObjs.SetType(objList[24], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[25], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[26], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[27], InteractiveType.Unknown);//

            //row 8
            Functions_InteractiveObjs.SetType(objList[28], InteractiveType.Boat_Stairs_Left);
            Functions_InteractiveObjs.SetType(objList[29], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[30], InteractiveType.Boat_Stairs_Right);
            Functions_InteractiveObjs.SetType(objList[31], InteractiveType.Unknown);//

            //row 9
            //Functions_InteractiveObjs.SetType(objList[32], InteractiveType.Wor_Boat_Stairs_Bottom_Left);
            Functions_InteractiveObjs.SetType(objList[33], InteractiveType.Unknown);//
            //Functions_InteractiveObjs.SetType(objList[34], InteractiveType.Wor_Boat_Stairs_Bottom_Right);
            Functions_InteractiveObjs.SetType(objList[35], InteractiveType.Unknown);//

            //row 10
            Functions_InteractiveObjs.SetType(objList[36], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[37], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[38], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[39], InteractiveType.Unknown);

            //row 11
            Functions_InteractiveObjs.SetType(objList[40], InteractiveType.Boat_Floor);
            Functions_InteractiveObjs.SetType(objList[41], InteractiveType.Boat_Barrel);
            Functions_InteractiveObjs.SetType(objList[42], InteractiveType.Boat_Stairs_Cover);
            Functions_InteractiveObjs.SetType(objList[43], InteractiveType.Boat_Captain_Brandy);

            //row 12 - cheat and put these beyond widget bounds cause they big
            Functions_InteractiveObjs.SetType(objList[44], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[45], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[46], InteractiveType.Unknown);
            //Functions_InteractiveObjs.SetType(objList[47], InteractiveType.Wor_Boat_Front);

            //position the objs relative to the window frame
            Functions_WidgetObject.PositionObjs(this);



            //manually set collision recs for some objs
            objList[0].compCollision.rec.Width = 16 * 2;
            objList[0].compCollision.rec.Height = 16 * 3;
            objList[2].compCollision.rec.Width = 16 * 2;
            objList[2].compCollision.rec.Height = 16 * 3;

            //connectors
            objList[12].compCollision.rec.Width = 16 * 2;
            objList[14].compCollision.rec.Width = 16 * 2;
            //bannisters
            objList[16].compCollision.rec.Width = 16 * 2;
            objList[18].compCollision.rec.Width = 16 * 2;
            //top stairs
            objList[20].compCollision.rec.Width = 16 * 2;
            objList[20].compCollision.rec.Height = 16 * 2;
            objList[22].compCollision.rec.Width = 16 * 2;
            objList[22].compCollision.rec.Height = 16 * 2;
            //stairs
            objList[28].compCollision.rec.Width = 16 * 2;
            objList[30].compCollision.rec.Width = 16 * 2;
            //bottom stairs
            objList[32].compCollision.rec.Width = 16 * 2;
            objList[34].compCollision.rec.Width = 16 * 2;


            //hide the dungeon objs covered by other objs
            Functions_WidgetObject.HideObj(objList[1]);
            Functions_WidgetObject.HideObj(objList[3]);
            Functions_WidgetObject.HideObj(objList[4]);
            Functions_WidgetObject.HideObj(objList[5]);
            Functions_WidgetObject.HideObj(objList[6]);
            Functions_WidgetObject.HideObj(objList[7]);
            Functions_WidgetObject.HideObj(objList[8]);
            Functions_WidgetObject.HideObj(objList[9]);
            Functions_WidgetObject.HideObj(objList[10]);
            Functions_WidgetObject.HideObj(objList[11]);
            Functions_WidgetObject.HideObj(objList[13]);
            Functions_WidgetObject.HideObj(objList[15]);
            Functions_WidgetObject.HideObj(objList[17]);
            Functions_WidgetObject.HideObj(objList[19]);
            Functions_WidgetObject.HideObj(objList[21]);
            Functions_WidgetObject.HideObj(objList[23]);
            Functions_WidgetObject.HideObj(objList[24]);
            Functions_WidgetObject.HideObj(objList[25]);
            Functions_WidgetObject.HideObj(objList[26]);
            Functions_WidgetObject.HideObj(objList[27]);
            Functions_WidgetObject.HideObj(objList[29]);
            Functions_WidgetObject.HideObj(objList[31]);
            Functions_WidgetObject.HideObj(objList[33]);
            Functions_WidgetObject.HideObj(objList[35]);


            // - we could simply hide all unknown objects?
        }
    }

    public class WidgetObjects_Boat_Back : WidgetObject
    {
        public WidgetObjects_Boat_Back()
        {   //create and set the position of the window frame
            window = new MenuWindow(
                new Point(16 * 12 + 16 * 6, 16 * 2), //position
                new Point(16 * 5, 16 * 15), //size
                "Boat Back Objs"); //title

            objList = new List<InteractiveObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new InteractiveObject()); }

            //row 1
            //Functions_InteractiveObjs.SetType(objList[0], InteractiveType.Wor_Boat_Back_Left);
            //Functions_InteractiveObjs.SetType(objList[1], InteractiveType.Wor_Boat_Back_Left_Connector);
            //Functions_InteractiveObjs.SetType(objList[2], InteractiveType.Wor_Boat_Back_Right_Connector);
            //Functions_InteractiveObjs.SetType(objList[3], InteractiveType.Wor_Boat_Back_Right);

            //row 2
            Functions_InteractiveObjs.SetType(objList[4], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[5], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[6], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[7], InteractiveType.Unknown);//

            //row 3
            Functions_InteractiveObjs.SetType(objList[8], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[9], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[10], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[11], InteractiveType.Unknown);//

            //row 4
            Functions_InteractiveObjs.SetType(objList[12], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[13], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[14], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[15], InteractiveType.Unknown);//

            //row 5
            Functions_InteractiveObjs.SetType(objList[16], InteractiveType.Boat_Bridge_Top);
            Functions_InteractiveObjs.SetType(objList[17], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[18], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[19], InteractiveType.Unknown);

            //row 6
            Functions_InteractiveObjs.SetType(objList[20], InteractiveType.Boat_Bridge_Bottom);
            Functions_InteractiveObjs.SetType(objList[21], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[22], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[23], InteractiveType.Unknown);

            //row 7
            Functions_InteractiveObjs.SetType(objList[24], InteractiveType.Boat_Pier_TopLeft);
            Functions_InteractiveObjs.SetType(objList[25], InteractiveType.Boat_Pier_TopMiddle);
            Functions_InteractiveObjs.SetType(objList[26], InteractiveType.Boat_Pier_TopRight);
            Functions_InteractiveObjs.SetType(objList[27], InteractiveType.Coastline_1x2_Animated);

            //row 8
            Functions_InteractiveObjs.SetType(objList[28], InteractiveType.Boat_Pier_Left);
            Functions_InteractiveObjs.SetType(objList[29], InteractiveType.Boat_Pier_Middle);
            Functions_InteractiveObjs.SetType(objList[30], InteractiveType.Boat_Pier_Right);
            Functions_InteractiveObjs.SetType(objList[31], InteractiveType.Unknown);//

            //row 9
            Functions_InteractiveObjs.SetType(objList[32], InteractiveType.Boat_Pier_BottomLeft);
            Functions_InteractiveObjs.SetType(objList[33], InteractiveType.Boat_Pier_BottomMiddle);
            Functions_InteractiveObjs.SetType(objList[34], InteractiveType.Boat_Pier_BottomRight);
            Functions_InteractiveObjs.SetType(objList[35], InteractiveType.Unknown);

            //row 10
            Functions_InteractiveObjs.SetType(objList[36], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[37], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[38], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[39], InteractiveType.Unknown);

            //row 11
            Functions_InteractiveObjs.SetType(objList[40], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[41], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[42], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[43], InteractiveType.Unknown);

            //row 12 - cheat and put these beyond widget bounds cause they big
            //Functions_InteractiveObjs.SetType(objList[44], InteractiveType.Wor_Boat_Back_Center);
            Functions_InteractiveObjs.SetType(objList[45], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[46], InteractiveType.Unknown);
            //Functions_InteractiveObjs.SetType(objList[47], InteractiveType.Wor_Boat_Engine); //engine

            //position the objs relative to the window frame
            Functions_WidgetObject.PositionObjs(this);


            //set collision rec
            objList[0].compCollision.rec.Height = 16 * 4;
            objList[1].compCollision.rec.Height = 16 * 4;
            objList[2].compCollision.rec.Height = 16 * 4;
            objList[3].compCollision.rec.Height = 16 * 4;

            objList[16].compCollision.rec.Width = 16 * 2;
            objList[20].compCollision.rec.Width = 16 * 2;

            objList[27].compCollision.rec.Height = 16 * 2;

            //hide covered objs
            Functions_WidgetObject.HideObj(objList[4]);
            Functions_WidgetObject.HideObj(objList[5]);
            Functions_WidgetObject.HideObj(objList[6]);
            Functions_WidgetObject.HideObj(objList[7]);
            Functions_WidgetObject.HideObj(objList[8]);
            Functions_WidgetObject.HideObj(objList[9]);
            Functions_WidgetObject.HideObj(objList[10]);
            Functions_WidgetObject.HideObj(objList[11]);
            Functions_WidgetObject.HideObj(objList[12]);
            Functions_WidgetObject.HideObj(objList[13]);
            Functions_WidgetObject.HideObj(objList[14]);
            Functions_WidgetObject.HideObj(objList[15]);

            Functions_WidgetObject.HideObj(objList[17]);
            Functions_WidgetObject.HideObj(objList[21]);
            Functions_WidgetObject.HideObj(objList[31]);


        }
    }









    //island specific widget objects

    public class WidgetObjects_Forest : WidgetObject
    {
        public WidgetObjects_Forest()
        {   //create and set the position of the window frame
            window = new MenuWindow(
                new Point(16 * 12, 16 * 2), //position
                new Point(16 * 5, 16 * 15), //size
                "Forest Objects"); //title

            objList = new List<InteractiveObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new InteractiveObject()); }

            //row 1

            //this is a 3x4 * 16 sized sprite/obj
            //Functions_InteractiveObjs.SetType(objList[0], InteractiveType.Wor_Entrance_ForestDungeon);
            Functions_InteractiveObjs.SetType(objList[1], InteractiveType.Unknown);//
            //Functions_InteractiveObjs.SetType(objList[2], InteractiveType.Wor_SkullToothInWater_Arch_Extension);
            //Functions_InteractiveObjs.SetType(objList[3], InteractiveType.Wor_SkullToothInWater_Arch_Left);

            //row 2
            Functions_InteractiveObjs.SetType(objList[4], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[5], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[6], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[7], InteractiveType.Unknown);

            //row 3
            Functions_InteractiveObjs.SetType(objList[8], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[9], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[10], InteractiveType.Unknown);//
            //Functions_InteractiveObjs.SetType(objList[11], InteractiveType.Wor_SkullToothInWater_Arch_Right);

            //row 4
            Functions_InteractiveObjs.SetType(objList[12], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[13], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[14], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[15], InteractiveType.Unknown);

            //row 5
            //Functions_InteractiveObjs.SetType(objList[16], InteractiveType.Wor_SkullToothInWater_Left);
            Functions_InteractiveObjs.SetType(objList[17], InteractiveType.Unknown);
            //Functions_InteractiveObjs.SetType(objList[18], InteractiveType.Wor_SkullToothInWater_Right);
            Functions_InteractiveObjs.SetType(objList[19], InteractiveType.Unknown);

            //row 6
            Functions_InteractiveObjs.SetType(objList[20], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[21], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[22], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[23], InteractiveType.Unknown);

            //row 7
            Functions_InteractiveObjs.SetType(objList[24], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[25], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[26], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[27], InteractiveType.Unknown);

            //row 8
            Functions_InteractiveObjs.SetType(objList[28], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[29], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[30], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[31], InteractiveType.Unknown);

            //row 9
            //Functions_InteractiveObjs.SetType(objList[32], InteractiveType.Wor_SkullToothInWater_EndCap_Left);
            Functions_InteractiveObjs.SetType(objList[33], InteractiveType.Unknown);
            //Functions_InteractiveObjs.SetType(objList[34], InteractiveType.Wor_SkullToothInWater_EndCap_Right);
            Functions_InteractiveObjs.SetType(objList[35], InteractiveType.Unknown);

            //row 10
            Functions_InteractiveObjs.SetType(objList[36], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[37], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[38], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[39], InteractiveType.Unknown);

            //row 11
            Functions_InteractiveObjs.SetType(objList[40], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[41], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[42], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[43], InteractiveType.Unknown);

            //row 12
            //Functions_InteractiveObjs.SetType(objList[44], InteractiveType.Wor_SkullToothInWater_Center);
            Functions_InteractiveObjs.SetType(objList[45], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[46], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[47], InteractiveType.Unknown);

            //position the objs relative to the window frame
            Functions_WidgetObject.PositionObjs(this);

            //manually set the dungeon entrances collision rec
            objList[0].compCollision.rec.Width = 16 * 3;
            objList[0].compCollision.rec.Height = 16 * 4;

            //hide the dungeon objs covered by dungeon entrance
            Functions_WidgetObject.HideObj(objList[1]);
            //Functions_WidgetObject.HideObj(objList[2]);

            Functions_WidgetObject.HideObj(objList[4]);
            Functions_WidgetObject.HideObj(objList[5]);
            Functions_WidgetObject.HideObj(objList[6]);

            Functions_WidgetObject.HideObj(objList[8]);
            Functions_WidgetObject.HideObj(objList[9]);
            Functions_WidgetObject.HideObj(objList[10]);

            Functions_WidgetObject.HideObj(objList[12]);
            Functions_WidgetObject.HideObj(objList[13]);
            Functions_WidgetObject.HideObj(objList[14]);

            //left tooth in water
            //Functions_WidgetObject.HideObj(objList[17]);
            Functions_WidgetObject.HideObj(objList[20]);
            //Functions_WidgetObject.HideObj(objList[21]);
            Functions_WidgetObject.HideObj(objList[22]);
            //Functions_WidgetObject.HideObj(objList[23]);
            Functions_WidgetObject.HideObj(objList[24]);
            //Functions_WidgetObject.HideObj(objList[25]);
            Functions_WidgetObject.HideObj(objList[26]);
            //Functions_WidgetObject.HideObj(objList[27]);
            Functions_WidgetObject.HideObj(objList[28]);
            //Functions_WidgetObject.HideObj(objList[29]);
            Functions_WidgetObject.HideObj(objList[30]);
            //Functions_WidgetObject.HideObj(objList[31]);

            //Functions_WidgetObject.HideObj(objList[33]);
            Functions_WidgetObject.HideObj(objList[36]);
            //Functions_WidgetObject.HideObj(objList[37]);
            Functions_WidgetObject.HideObj(objList[38]);
            Functions_WidgetObject.HideObj(objList[40]);
            //Functions_WidgetObject.HideObj(objList[41]);
            Functions_WidgetObject.HideObj(objList[42]);
            //Functions_WidgetObject.HideObj(objList[44]);
            //Functions_WidgetObject.HideObj(objList[45]);
        }
    }

    public class WidgetObjects_Mountain : WidgetObject
    {
        public WidgetObjects_Mountain()
        {   //create and set the position of the window frame
            window = new MenuWindow(
                new Point(16 * 12, 16 * 2), //position
                new Point(16 * 5, 16 * 15), //size
                "Mountain Objects"); //title

            objList = new List<InteractiveObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new InteractiveObject()); }

            //row 1

            //this is a 3x4 * 16 sized sprite/obj
            //Functions_InteractiveObjs.SetType(objList[0], InteractiveType.Wor_Entrance_MountainDungeon);
            Functions_InteractiveObjs.SetType(objList[1], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[2], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[3], InteractiveType.Dungeon_Statue);

            //row 2
            Functions_InteractiveObjs.SetType(objList[4], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[5], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[6], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[7], InteractiveType.Enemy_SeekerExploder);

            //row 3
            Functions_InteractiveObjs.SetType(objList[8], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[9], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[10], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[11], InteractiveType.Dungeon_Statue);

            //row 4
            Functions_InteractiveObjs.SetType(objList[12], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[13], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[14], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[15], InteractiveType.Dungeon_Statue);

            //row 5
            Functions_InteractiveObjs.SetType(objList[16], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[17], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[18], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[19], InteractiveType.Unknown);

            //row 6
            Functions_InteractiveObjs.SetType(objList[20], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[21], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[22], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[23], InteractiveType.Unknown);

            //row 7
            Functions_InteractiveObjs.SetType(objList[24], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[25], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[26], InteractiveType.Unknown);
            //Functions_InteractiveObjs.SetType(objList[27], InteractiveType.Wor_MountainWall_Cave_Covered);

            //row 8
            Functions_InteractiveObjs.SetType(objList[28], InteractiveType.Unknown);
            //Functions_InteractiveObjs.SetType(objList[29], InteractiveType.Wor_MountainWall_Cave_Bare);
            Functions_InteractiveObjs.SetType(objList[30], InteractiveType.Unknown); //cov
            Functions_InteractiveObjs.SetType(objList[31], InteractiveType.Unknown); //cov

            //row 9
            Functions_InteractiveObjs.SetType(objList[32], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[33], InteractiveType.Unknown); //cov
            Functions_InteractiveObjs.SetType(objList[34], InteractiveType.Unknown); //cov
            Functions_InteractiveObjs.SetType(objList[35], InteractiveType.MountainWall_Top); //wall top

            //row 10
            Functions_InteractiveObjs.SetType(objList[36], InteractiveType.MountainWall_Ladder_Trap);
            //Functions_InteractiveObjs.SetType(objList[37], InteractiveType.Wor_MountainWall_Alcove_Right);
            Functions_InteractiveObjs.SetType(objList[38], InteractiveType.Unknown); //covered
            Functions_InteractiveObjs.SetType(objList[39], InteractiveType.MountainWall_Mid); //wall mid

            //row 11
            Functions_InteractiveObjs.SetType(objList[40], InteractiveType.MountainWall_Ladder);
            Functions_InteractiveObjs.SetType(objList[41], InteractiveType.Unknown); //cov
            Functions_InteractiveObjs.SetType(objList[42], InteractiveType.Unknown); //cov
            Functions_InteractiveObjs.SetType(objList[43], InteractiveType.Unknown); //covered

            //row 12
            Functions_InteractiveObjs.SetType(objList[44], InteractiveType.MountainWall_Foothold);
            //Functions_InteractiveObjs.SetType(objList[45], InteractiveType.Wor_MountainWall_Alcove_Left);
            Functions_InteractiveObjs.SetType(objList[46], InteractiveType.Unknown); //covered
            Functions_InteractiveObjs.SetType(objList[47], InteractiveType.MountainWall_Bottom); //wall bottom

            //position the objs relative to the window frame
            Functions_WidgetObject.PositionObjs(this);

            //manually set the dungeon entrances collision rec
            objList[0].compCollision.rec.Width = 16 * 3;
            objList[0].compCollision.rec.Height = 16 * 4;

            //hide the dungeon objs covered by dungeon entrance
            Functions_WidgetObject.HideObj(objList[1]);
            Functions_WidgetObject.HideObj(objList[2]);

            Functions_WidgetObject.HideObj(objList[4]);
            Functions_WidgetObject.HideObj(objList[5]);
            Functions_WidgetObject.HideObj(objList[6]);

            Functions_WidgetObject.HideObj(objList[8]);
            Functions_WidgetObject.HideObj(objList[9]);
            Functions_WidgetObject.HideObj(objList[10]);

            Functions_WidgetObject.HideObj(objList[12]);
            Functions_WidgetObject.HideObj(objList[13]);
            Functions_WidgetObject.HideObj(objList[14]);

            Functions_WidgetObject.HideObj(objList[30]);
            Functions_WidgetObject.HideObj(objList[31]);

            Functions_WidgetObject.HideObj(objList[33]);
            Functions_WidgetObject.HideObj(objList[34]);

            Functions_WidgetObject.HideObj(objList[38]);

            Functions_WidgetObject.HideObj(objList[41]);
            Functions_WidgetObject.HideObj(objList[42]);
            Functions_WidgetObject.HideObj(objList[43]);

            Functions_WidgetObject.HideObj(objList[46]);
        }
    }

    public class WidgetObjects_Swamp : WidgetObject
    {
        public WidgetObjects_Swamp()
        {   //create and set the position of the window frame
            window = new MenuWindow(
                new Point(16 * 12, 16 * 2), //position
                new Point(16 * 5, 16 * 15), //size
                "Swamp Objects"); //title

            objList = new List<InteractiveObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new InteractiveObject()); }

            //row 1

            //this is a 3x4 * 16 sized sprite/obj
            //Functions_InteractiveObjs.SetType(objList[0], InteractiveType.Wor_Entrance_SwampDungeon);
            Functions_InteractiveObjs.SetType(objList[1], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[2], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[3], InteractiveType.Dungeon_Statue);

            //row 2
            Functions_InteractiveObjs.SetType(objList[4], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[5], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[6], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[7], InteractiveType.Enemy_SeekerExploder);

            //row 3
            Functions_InteractiveObjs.SetType(objList[8], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[9], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[10], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[11], InteractiveType.Dungeon_Statue);

            //row 4
            Functions_InteractiveObjs.SetType(objList[12], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[13], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[14], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[15], InteractiveType.Dungeon_Statue);

            //row 5
            //Functions_InteractiveObjs.SetType(objList[16], InteractiveType.Wor_Swamp_SmPlant);
            //Functions_InteractiveObjs.SetType(objList[17], InteractiveType.Wor_Swamp_BigPlant);
            Functions_InteractiveObjs.SetType(objList[18], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[19], InteractiveType.Water_LillyPad);

            //row 6
            //Functions_InteractiveObjs.SetType(objList[20], InteractiveType.Wor_Swamp_Bulb);
            Functions_InteractiveObjs.SetType(objList[21], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[22], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[23], InteractiveType.Unknown);//

            //row 7
            Functions_InteractiveObjs.SetType(objList[24], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[25], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[26], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[27], InteractiveType.Water_Vine);

            //row 8
            Functions_InteractiveObjs.SetType(objList[28], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[29], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[30], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[31], InteractiveType.Unknown);

            //row 9
            Functions_InteractiveObjs.SetType(objList[32], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[33], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[34], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[35], InteractiveType.Unknown);

            //row 10
            Functions_InteractiveObjs.SetType(objList[36], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[37], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[38], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[39], InteractiveType.Unknown); 

            //row 11
            Functions_InteractiveObjs.SetType(objList[40], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[41], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[42], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[43], InteractiveType.Unknown);

            //row 12
            Functions_InteractiveObjs.SetType(objList[44], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[45], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[46], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[47], InteractiveType.Unknown); 

            //position the objs relative to the window frame
            Functions_WidgetObject.PositionObjs(this);

            //manually set the dungeon entrances collision rec
            objList[0].compCollision.rec.Width = 16 * 3;
            objList[0].compCollision.rec.Height = 16 * 4;

            //hide the dungeon objs covered by dungeon entrance
            Functions_WidgetObject.HideObj(objList[1]);
            Functions_WidgetObject.HideObj(objList[2]);

            Functions_WidgetObject.HideObj(objList[4]);
            Functions_WidgetObject.HideObj(objList[5]);
            Functions_WidgetObject.HideObj(objList[6]);

            Functions_WidgetObject.HideObj(objList[8]);
            Functions_WidgetObject.HideObj(objList[9]);
            Functions_WidgetObject.HideObj(objList[10]);

            Functions_WidgetObject.HideObj(objList[12]);
            Functions_WidgetObject.HideObj(objList[13]);
            Functions_WidgetObject.HideObj(objList[14]);

            Functions_WidgetObject.HideObj(objList[18]);
            Functions_WidgetObject.HideObj(objList[21]);
            Functions_WidgetObject.HideObj(objList[22]);
            Functions_WidgetObject.HideObj(objList[23]);


        }
    }





    //a widget to throw random objects on for testing

    public class WidgetObjects_DEV : WidgetObject
    {
        public WidgetObjects_DEV()
        {   //create and set the position of the window frame
            window = new MenuWindow(
                new Point(16 * 34, 16 * 2), //position
                new Point(16 * 5, 16 * 15), //size
                "DEV Objs"); //title

            objList = new List<InteractiveObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new InteractiveObject()); }

            //row 1
            Functions_InteractiveObjs.SetType(objList[0], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[1], InteractiveType.Unknown);
            //Functions_InteractiveObjs.SetType(objList[2], InteractiveType.Wor_Shadow_Big);
            Functions_InteractiveObjs.SetType(objList[3], InteractiveType.Unknown);//

            //row 2
            Functions_InteractiveObjs.SetType(objList[4], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[5], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[6], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[7], InteractiveType.Unknown);//

            //row 3
            Functions_InteractiveObjs.SetType(objList[8], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[9], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[10], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[11], InteractiveType.Unknown);//

            //row 4
            Functions_InteractiveObjs.SetType(objList[12], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[13], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[14], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[15], InteractiveType.Unknown);//

            //row 5
            Functions_InteractiveObjs.SetType(objList[16], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[17], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[18], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[19], InteractiveType.Unknown);

            //row 6
            Functions_InteractiveObjs.SetType(objList[20], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[21], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[22], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[23], InteractiveType.Unknown);

            //row 7
            Functions_InteractiveObjs.SetType(objList[24], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[25], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[26], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[27], InteractiveType.Unknown);

            //row 8
            Functions_InteractiveObjs.SetType(objList[28], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[29], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[30], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[31], InteractiveType.Unknown);

            //row 9
            Functions_InteractiveObjs.SetType(objList[32], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[33], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[34], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[35], InteractiveType.Unknown);

            //row 10
            Functions_InteractiveObjs.SetType(objList[36], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[37], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[38], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[39], InteractiveType.Unknown);

            //row 11
            Functions_InteractiveObjs.SetType(objList[40], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[41], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[42], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[43], InteractiveType.Unknown);

            //row 12
            Functions_InteractiveObjs.SetType(objList[44], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[45], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[46], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[47], InteractiveType.Unknown);

            //position the objs relative to the window frame
            Functions_WidgetObject.PositionObjs(this);

            //hide objs covered by big shadow
            Functions_WidgetObject.HideObj(objList[3]);
            Functions_WidgetObject.HideObj(objList[6]);
            Functions_WidgetObject.HideObj(objList[7]);
            Functions_WidgetObject.HideObj(objList[10]);
            Functions_WidgetObject.HideObj(objList[11]);
            Functions_WidgetObject.HideObj(objList[14]);
            Functions_WidgetObject.HideObj(objList[15]);

        }
    }





    */





}