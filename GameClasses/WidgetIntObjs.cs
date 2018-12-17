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

    public static class Functions_WidgetIntObjs
    {
        static int i = 0;
        static int row = 0;
        static int counter = 0;

        public static void PositionObjs(WidgetIntObject WO)
        {
            counter = 0;
            for (row = 0; row < 12; row++)
            {   //12 rows, with 4 objs per row
                for(i = 0; i < 4; i++)
                {
                    //obj could be null
                    if(WO.objList[counter] != null)
                    {   //move gameObj relative to interior window rec
                        Functions_Movement.Teleport(
                            WO.objList[counter].compMove,
                            WO.window.interior.rec.X + 13 + (i * 16),
                            WO.window.interior.rec.Y + 29 + (row * 16));
                        Functions_Component.Align(WO.objList[counter]);

                        //manually set the collisionRecs (they need to be 16x16, default aligned for editor use)
                        WO.objList[counter].compCollision.rec.X = (int)WO.objList[counter].compSprite.position.X - 8;
                        WO.objList[counter].compCollision.rec.Y = (int)WO.objList[counter].compSprite.position.Y - 8;
                        WO.objList[counter].compCollision.rec.Width = 16;
                        WO.objList[counter].compCollision.rec.Height = 16;
                    }

                    counter++;
                }
            }
        }

        public static void HideObj(InteractiveObject Obj)
        {   //hide this widget object offscreen
            Functions_Movement.Teleport(Obj.compMove, 2048, 2048);
            Functions_Component.Align(Obj);
            Obj.compCollision.rec.X = 2048;
            Obj.compCollision.rec.Y = 2048;
            Obj.compCollision.rec.Width = 1;
            Obj.compCollision.rec.Height = 1;
        }

    }



    public class WidgetIntObject : Widget
    {
        public List<InteractiveObject> objList;
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







    //set 1 - island related objs

    public class WidgetObjects_Forest : WidgetIntObject
    {
        public WidgetObjects_Forest()
        {   //1st
            window = new MenuWindow(
                new Point(16 * 1, 16 * 2),
                new Point(16 * 5, 16 * 15), //size
                "Forest"); //title

            objList = new List<InteractiveObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new InteractiveObject()); }

            //row 1
            Functions_InteractiveObjs.SetType(objList[0], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[1], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[2], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[3], InteractiveType.Unknown);

            //row 2
            Functions_InteractiveObjs.SetType(objList[4], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[5], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[6], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[7], InteractiveType.Unknown);

            //row 3
            Functions_InteractiveObjs.SetType(objList[8], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[9], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[10], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[11], InteractiveType.Unknown);

            //row 4
            Functions_InteractiveObjs.SetType(objList[12], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[13], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[14], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[15], InteractiveType.Unknown);

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
            Functions_WidgetIntObjs.PositionObjs(this);

            //Functions_WidgetIntObjs.HideObj(objList[1]);

            //reset update all zdepths
            for (i = 0; i < 4 * 12; i++)
            {
                objList[i].compSprite.zOffset = 0;
                Functions_Component.SetZdepth(objList[i].compSprite);
            }
        }
    }

    public class WidgetObjects_Mountain : WidgetIntObject
    {
        public WidgetObjects_Mountain()
        {   //2nd
            window = new MenuWindow(
                new Point(16 * 6 + 8, 16 * 2),
                new Point(16 * 5, 16 * 15), //size
                "Mountain"); //title

            objList = new List<InteractiveObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new InteractiveObject()); }

            //row 1
            Functions_InteractiveObjs.SetType(objList[0], InteractiveType.MountainWall_Top);
            Functions_InteractiveObjs.SetType(objList[1], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[2], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[3], InteractiveType.Unknown);//

            //row 2
            Functions_InteractiveObjs.SetType(objList[4], InteractiveType.MountainWall_Mid);
            Functions_InteractiveObjs.SetType(objList[5], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[6], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[7], InteractiveType.Unknown);//

            //row 3
            Functions_InteractiveObjs.SetType(objList[8], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[9], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[10], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[11], InteractiveType.Unknown);//

            //row 4
            Functions_InteractiveObjs.SetType(objList[12], InteractiveType.MountainWall_Bottom);
            Functions_InteractiveObjs.SetType(objList[13], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[14], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[15], InteractiveType.Unknown);//

            //row 5
            Functions_InteractiveObjs.SetType(objList[16], InteractiveType.MountainWall_Foothold);
            Functions_InteractiveObjs.SetType(objList[17], InteractiveType.MountainWall_Ladder);
            Functions_InteractiveObjs.SetType(objList[18], InteractiveType.MountainWall_Ladder_Trap);
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
            Functions_WidgetIntObjs.PositionObjs(this);

            Functions_WidgetIntObjs.HideObj(objList[1]);
            Functions_WidgetIntObjs.HideObj(objList[2]);
            Functions_WidgetIntObjs.HideObj(objList[3]);

            Functions_WidgetIntObjs.HideObj(objList[5]);
            Functions_WidgetIntObjs.HideObj(objList[6]);
            Functions_WidgetIntObjs.HideObj(objList[7]);

            Functions_WidgetIntObjs.HideObj(objList[8]);
            Functions_WidgetIntObjs.HideObj(objList[9]);
            Functions_WidgetIntObjs.HideObj(objList[10]);
            Functions_WidgetIntObjs.HideObj(objList[11]);

            Functions_WidgetIntObjs.HideObj(objList[13]);
            Functions_WidgetIntObjs.HideObj(objList[14]);
            Functions_WidgetIntObjs.HideObj(objList[15]);

            //reset update all zdepths
            for (i = 0; i < 4 * 12; i++)
            {
                objList[i].compSprite.zOffset = 0;
                Functions_Component.SetZdepth(objList[i].compSprite);
            }
        }
    }

    public class WidgetObjects_Swamp : WidgetIntObject
    {
        public WidgetObjects_Swamp()
        {   //3rd
            window = new MenuWindow(
                new Point(16 * 12, 16 * 2),
                new Point(16 * 5, 16 * 15), //size
                "Swamp"); //title

            objList = new List<InteractiveObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new InteractiveObject()); }

            //row 1
            Functions_InteractiveObjs.SetType(objList[0], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[1], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[2], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[3], InteractiveType.Unknown);

            //row 2
            Functions_InteractiveObjs.SetType(objList[4], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[5], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[6], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[7], InteractiveType.Unknown);

            //row 3
            Functions_InteractiveObjs.SetType(objList[8], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[9], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[10], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[11], InteractiveType.Unknown);

            //row 4
            Functions_InteractiveObjs.SetType(objList[12], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[13], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[14], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[15], InteractiveType.Unknown);

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
            Functions_WidgetIntObjs.PositionObjs(this);

            //Functions_WidgetIntObjs.HideObj(objList[1]);

            //reset update all zdepths
            for (i = 0; i < 4 * 12; i++)
            {
                objList[i].compSprite.zOffset = 0;
                Functions_Component.SetZdepth(objList[i].compSprite);
            }
        }
    }

    public class WidgetObjects_Lava : WidgetIntObject
    {
        public WidgetObjects_Lava()
        {   //4th left
            window = new MenuWindow(
                new Point(16 * 17+8, 16 * 2),
                new Point(16 * 5, 16 * 15), //size
                "Lava"); //title

            objList = new List<InteractiveObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new InteractiveObject()); }

            //row 1
            Functions_InteractiveObjs.SetType(objList[0], InteractiveType.Lava_Pit);
            Functions_InteractiveObjs.SetType(objList[1], InteractiveType.Lava_PitBridge);
            Functions_InteractiveObjs.SetType(objList[2], InteractiveType.Lava_PitTeethBottom);
            Functions_InteractiveObjs.SetType(objList[3], InteractiveType.Lava_PitTeethTop);

            //row 2
            Functions_InteractiveObjs.SetType(objList[4], InteractiveType.Lava_PitTrap);
            Functions_InteractiveObjs.SetType(objList[5], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[6], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[7], InteractiveType.Unknown);

            //row 3
            Functions_InteractiveObjs.SetType(objList[8], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[9], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[10], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[11], InteractiveType.Unknown);

            //row 4
            Functions_InteractiveObjs.SetType(objList[12], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[13], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[14], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[15], InteractiveType.Unknown);

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
            Functions_WidgetIntObjs.PositionObjs(this);

            //Functions_WidgetIntObjs.HideObj(objList[1]);

            //reset update all zdepths
            for (i = 0; i < 4 * 12; i++)
            {
                objList[i].compSprite.zOffset = 0;
                Functions_Component.SetZdepth(objList[i].compSprite);
            }
        }
    }

    public class WidgetObjects_Cloud : WidgetIntObject
    {
        public WidgetObjects_Cloud()
        {   //5th
            window = new MenuWindow(
                new Point(16 * 23, 16 * 2),
                new Point(16 * 5, 16 * 15), //size
                "Cloud"); //title

            objList = new List<InteractiveObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new InteractiveObject()); }

            //row 1
            Functions_InteractiveObjs.SetType(objList[0], InteractiveType.IceTile);
            Functions_InteractiveObjs.SetType(objList[1], InteractiveType.IceTile);
            Functions_InteractiveObjs.SetType(objList[2], InteractiveType.IceTile);
            Functions_InteractiveObjs.SetType(objList[3], InteractiveType.IceTile);

            //row 2
            Functions_InteractiveObjs.SetType(objList[4], InteractiveType.IceTile);
            Functions_InteractiveObjs.SetType(objList[5], InteractiveType.IceTile);
            Functions_InteractiveObjs.SetType(objList[6], InteractiveType.IceTile);
            Functions_InteractiveObjs.SetType(objList[7], InteractiveType.IceTile);

            //row 3
            Functions_InteractiveObjs.SetType(objList[8], InteractiveType.IceTile);
            Functions_InteractiveObjs.SetType(objList[9], InteractiveType.IceTile);
            Functions_InteractiveObjs.SetType(objList[10], InteractiveType.IceTile);
            Functions_InteractiveObjs.SetType(objList[11], InteractiveType.IceTile);

            //row 4
            Functions_InteractiveObjs.SetType(objList[12], InteractiveType.IceTile);
            Functions_InteractiveObjs.SetType(objList[13], InteractiveType.IceTile);
            Functions_InteractiveObjs.SetType(objList[14], InteractiveType.IceTile);
            Functions_InteractiveObjs.SetType(objList[15], InteractiveType.IceTile);

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
            Functions_WidgetIntObjs.PositionObjs(this);

            //Functions_WidgetIntObjs.HideObj(objList[1]);

            //reset update all zdepths
            for (i = 0; i < 4 * 12; i++)
            {
                objList[i].compSprite.zOffset = 0;
                Functions_Component.SetZdepth(objList[i].compSprite);
            }
        }
    }

    public class WidgetObjects_ThievesDen : WidgetIntObject
    {
        public WidgetObjects_ThievesDen()
        {   //6th
            window = new MenuWindow(
                new Point(16 * 28 + 8, 16 * 2),
                new Point(16 * 5, 16 * 15), //size
                "ThievesDen"); //title

            objList = new List<InteractiveObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new InteractiveObject()); }

            //row 1
            Functions_InteractiveObjs.SetType(objList[0], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[1], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[2], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[3], InteractiveType.Unknown);

            //row 2
            Functions_InteractiveObjs.SetType(objList[4], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[5], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[6], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[7], InteractiveType.Unknown);

            //row 3
            Functions_InteractiveObjs.SetType(objList[8], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[9], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[10], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[11], InteractiveType.Unknown);

            //row 4
            Functions_InteractiveObjs.SetType(objList[12], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[13], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[14], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[15], InteractiveType.Unknown);

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
            Functions_WidgetIntObjs.PositionObjs(this);

            //Functions_WidgetIntObjs.HideObj(objList[1]);
            

            //reset update all zdepths
            for (i = 0; i < 4 * 12; i++)
            {
                objList[i].compSprite.zOffset = 0;
                Functions_Component.SetZdepth(objList[i].compSprite);
            }
        }
    }

    public class WidgetObjects_Shadow : WidgetIntObject
    {
        public WidgetObjects_Shadow()
        {   //7th
            window = new MenuWindow(
                new Point(16 * 34, 16 * 2),
                new Point(16 * 5, 16 * 15), //size
                "Shadow"); //title

            objList = new List<InteractiveObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new InteractiveObject()); }

            //row 1
            Functions_InteractiveObjs.SetType(objList[0], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[1], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[2], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[3], InteractiveType.Unknown);

            //row 2
            Functions_InteractiveObjs.SetType(objList[4], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[5], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[6], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[7], InteractiveType.Unknown);

            //row 3
            Functions_InteractiveObjs.SetType(objList[8], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[9], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[10], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[11], InteractiveType.Unknown);

            //row 4
            Functions_InteractiveObjs.SetType(objList[12], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[13], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[14], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[15], InteractiveType.Unknown);

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
            Functions_WidgetIntObjs.PositionObjs(this);

            //Functions_WidgetIntObjs.HideObj(objList[1]);
            

            //reset update all zdepths
            for (i = 0; i < 4 * 12; i++)
            {
                objList[i].compSprite.zOffset = 0;
                Functions_Component.SetZdepth(objList[i].compSprite);
            }
        }
    }















    //set 2 - general world related objs

    public class WidgetObjects_Environment : WidgetIntObject
    {
        public WidgetObjects_Environment()
        {   //1st left
            window = new MenuWindow(
                new Point(16 * 1, 16 * 2), //position
                new Point(16 * 5, 16 * 15), //size
                "World/Env Objs"); //title

            objList = new List<InteractiveObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new InteractiveObject()); }

            //row 1
            Functions_InteractiveObjs.SetType(objList[0], InteractiveType.Grass_Tall);
            Functions_InteractiveObjs.SetType(objList[1], InteractiveType.Grass_Cut);
            Functions_InteractiveObjs.SetType(objList[2], InteractiveType.Grass_2);
            Functions_InteractiveObjs.SetType(objList[3], InteractiveType.Flowers);

            //row 2
            Functions_InteractiveObjs.SetType(objList[4], InteractiveType.Bush);
            Functions_InteractiveObjs.SetType(objList[5], InteractiveType.Bush_Stump);
            Functions_InteractiveObjs.SetType(objList[6], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[7], InteractiveType.Unknown);

            //row 3
            Functions_InteractiveObjs.SetType(objList[8], InteractiveType.Tree);
            Functions_InteractiveObjs.SetType(objList[9], InteractiveType.Tree_Burning);
            Functions_InteractiveObjs.SetType(objList[10], InteractiveType.Tree_Burnt);
            Functions_InteractiveObjs.SetType(objList[11], InteractiveType.Tree_Stump);

            //row 4
            Functions_InteractiveObjs.SetType(objList[12], InteractiveType.Unknown); //covered
            Functions_InteractiveObjs.SetType(objList[13], InteractiveType.Unknown); //covered
            Functions_InteractiveObjs.SetType(objList[14], InteractiveType.Unknown); //covered
            Functions_InteractiveObjs.SetType(objList[15], InteractiveType.Unknown); //covered

            //row 5
            Functions_InteractiveObjs.SetType(objList[16], InteractiveType.Dirt_Main);
            Functions_InteractiveObjs.SetType(objList[17], InteractiveType.Dirt_ToGrass_Corner_Exterior); 
            Functions_InteractiveObjs.SetType(objList[18], InteractiveType.Dirt_ToGrass_Corner_Interior);
            Functions_InteractiveObjs.SetType(objList[19], InteractiveType.Dirt_ToGrass_Straight);

            //row 6
            Functions_InteractiveObjs.SetType(objList[20], InteractiveType.Unknown); 
            Functions_InteractiveObjs.SetType(objList[21], InteractiveType.Unknown); 
            Functions_InteractiveObjs.SetType(objList[22], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[23], InteractiveType.Unknown);

            //row 7
            Functions_InteractiveObjs.SetType(objList[24], InteractiveType.Post_VerticalLeft);
            Functions_InteractiveObjs.SetType(objList[25], InteractiveType.Debris);
            Functions_InteractiveObjs.SetType(objList[26], InteractiveType.Post_VerticalRight);
            Functions_InteractiveObjs.SetType(objList[27], InteractiveType.Post_HammerPost_Down); //why?

            //row 8
            Functions_InteractiveObjs.SetType(objList[28], InteractiveType.Post_CornerLeft);
            Functions_InteractiveObjs.SetType(objList[29], InteractiveType.Post_Horizontal);
            Functions_InteractiveObjs.SetType(objList[30], InteractiveType.Post_CornerRight);
            Functions_InteractiveObjs.SetType(objList[31], InteractiveType.Post_HammerPost_Up);

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
            Functions_InteractiveObjs.SetType(objList[40], InteractiveType.Signpost);
            Functions_InteractiveObjs.SetType(objList[41], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[42], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[43], InteractiveType.Unknown);

            //row 12
            Functions_InteractiveObjs.SetType(objList[44], InteractiveType.Boat_Barrel);
            Functions_InteractiveObjs.SetType(objList[45], InteractiveType.Barrel);
            Functions_InteractiveObjs.SetType(objList[46], InteractiveType.Pot);
            Functions_InteractiveObjs.SetType(objList[47], InteractiveType.Dungeon_Pot);

            //position the objs relative to the window frame
            Functions_WidgetIntObjs.PositionObjs(this);

            //manually set tree recs
            objList[8].compCollision.rec.Height = 16 * 2;
            objList[9].compCollision.rec.Height = 16 * 2;

            //hide objs that trees overlap
            Functions_WidgetIntObjs.HideObj(objList[12]);
            Functions_WidgetIntObjs.HideObj(objList[13]);
            Functions_WidgetIntObjs.HideObj(objList[14]);
            Functions_WidgetIntObjs.HideObj(objList[15]);

            //reset update all zdepths
            for (i = 0; i < 4 * 12; i++)
            {
                objList[i].compSprite.zOffset = 0;
                Functions_Component.SetZdepth(objList[i].compSprite);
            }
        }
    }

    public class WidgetObjects_Water : WidgetIntObject
    {
        public WidgetObjects_Water()
        {   //2nd left
            window = new MenuWindow(
                new Point(16 * 6 + 8, 16 * 2), //position
                new Point(16 * 5, 16 * 15), //size
                "Water Objs"); //title

            objList = new List<InteractiveObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new InteractiveObject()); }

            //row 1
            Functions_InteractiveObjs.SetType(objList[0], InteractiveType.Water_2x2);
            Functions_InteractiveObjs.SetType(objList[1], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[2], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[3], InteractiveType.Coastline_Corner_Exterior);

            //row 2
            Functions_InteractiveObjs.SetType(objList[4], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[5], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[6], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[7], InteractiveType.Coastline_Corner_Interior);

            //row 3
            Functions_InteractiveObjs.SetType(objList[8], InteractiveType.Water_RockUnderwater);
            Functions_InteractiveObjs.SetType(objList[9], InteractiveType.Water_LillyPad);
            Functions_InteractiveObjs.SetType(objList[10], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[11], InteractiveType.Coastline_Straight);

            //row 4
            Functions_InteractiveObjs.SetType(objList[12], InteractiveType.Water_RockSm);
            Functions_InteractiveObjs.SetType(objList[13], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[14], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[15], InteractiveType.Water_RockMed);

            //row 5
            Functions_InteractiveObjs.SetType(objList[16], InteractiveType.Water_Vine);
            Functions_InteractiveObjs.SetType(objList[17], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[18], InteractiveType.Water_BigPlant);
            Functions_InteractiveObjs.SetType(objList[19], InteractiveType.Unknown);//

            //row 6
            Functions_InteractiveObjs.SetType(objList[20], InteractiveType.Water_Bulb);
            Functions_InteractiveObjs.SetType(objList[21], InteractiveType.Water_SmPlant);
            Functions_InteractiveObjs.SetType(objList[22], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[23], InteractiveType.Unknown);//

            //row 7
            Functions_InteractiveObjs.SetType(objList[24], InteractiveType.Water_3x3);
            Functions_InteractiveObjs.SetType(objList[25], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[26], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[27], InteractiveType.Water_1x1);

            //row 8
            Functions_InteractiveObjs.SetType(objList[28], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[29], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[30], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[31], InteractiveType.Coastline_1x2_Animated);

            //row 9
            Functions_InteractiveObjs.SetType(objList[32], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[33], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[34], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[35], InteractiveType.Unknown);//

            //row 10
            Functions_InteractiveObjs.SetType(objList[36], InteractiveType.Boat_Stairs_Left);
            Functions_InteractiveObjs.SetType(objList[37], InteractiveType.Unknown);//
            Functions_InteractiveObjs.SetType(objList[38], InteractiveType.Boat_Stairs_Right);
            Functions_InteractiveObjs.SetType(objList[39], InteractiveType.Unknown);//

            //row 11
            Functions_InteractiveObjs.SetType(objList[40], InteractiveType.Boat_Anchor);
            Functions_InteractiveObjs.SetType(objList[41], InteractiveType.Water_LillyPad_Mini);
            Functions_InteractiveObjs.SetType(objList[42], InteractiveType.Boat_Bridge_Top);
            Functions_InteractiveObjs.SetType(objList[43], InteractiveType.Unknown);//

            //row 12
            Functions_InteractiveObjs.SetType(objList[44], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[45], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[46], InteractiveType.Boat_Bridge_Bottom);
            Functions_InteractiveObjs.SetType(objList[47], InteractiveType.Unknown);//

            //position the objs relative to the window frame
            Functions_WidgetIntObjs.PositionObjs(this);

            //set recs
            objList[0].compCollision.rec.Height = objList[0].compCollision.rec.Width = 16 * 2;
            objList[9].compCollision.rec.Height = objList[9].compCollision.rec.Width = 16 * 2;
            objList[18].compCollision.rec.Height = objList[18].compCollision.rec.Width = 16 * 2;
            objList[24].compCollision.rec.Height = objList[24].compCollision.rec.Width = 16 * 3;

            objList[31].compCollision.rec.Height = 16 * 2;
            objList[36].compCollision.rec.Width = 16 * 2;
            objList[38].compCollision.rec.Width = 16 * 2;
            objList[42].compCollision.rec.Width = 16 * 2;
            objList[47].compCollision.rec.Width = 16 * 2;

            Functions_WidgetIntObjs.HideObj(objList[1]);
            Functions_WidgetIntObjs.HideObj(objList[4]);
            Functions_WidgetIntObjs.HideObj(objList[5]);

            Functions_WidgetIntObjs.HideObj(objList[10]);
            Functions_WidgetIntObjs.HideObj(objList[13]);
            Functions_WidgetIntObjs.HideObj(objList[14]);

            Functions_WidgetIntObjs.HideObj(objList[17]);

            Functions_WidgetIntObjs.HideObj(objList[19]);
            Functions_WidgetIntObjs.HideObj(objList[22]);
            Functions_WidgetIntObjs.HideObj(objList[23]);
            
            Functions_WidgetIntObjs.HideObj(objList[25]);
            Functions_WidgetIntObjs.HideObj(objList[26]);

            Functions_WidgetIntObjs.HideObj(objList[28]);
            Functions_WidgetIntObjs.HideObj(objList[29]);

            Functions_WidgetIntObjs.HideObj(objList[30]);
            Functions_WidgetIntObjs.HideObj(objList[32]);
            Functions_WidgetIntObjs.HideObj(objList[33]);
            Functions_WidgetIntObjs.HideObj(objList[34]);
            Functions_WidgetIntObjs.HideObj(objList[35]);
            Functions_WidgetIntObjs.HideObj(objList[37]);
            Functions_WidgetIntObjs.HideObj(objList[39]);

            Functions_WidgetIntObjs.HideObj(objList[43]);
            Functions_WidgetIntObjs.HideObj(objList[47]);


            //reset update all zdepths
            for (i = 0; i < 4 * 12; i++)
            {
                objList[i].compSprite.zOffset = 0;
                Functions_Component.SetZdepth(objList[i].compSprite);
            }
        }
    }

    public class WidgetObjects_House : WidgetIntObject
    {
        public WidgetObjects_House()
        {   //3rd left
            window = new MenuWindow(
                new Point(16 * 12, 16 * 2),
                new Point(16 * 5, 16 * 15), //size
                "House Objects"); //title

            objList = new List<InteractiveObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new InteractiveObject()); }

            //row 1
            Functions_InteractiveObjs.SetType(objList[0], InteractiveType.House_Wall_Side_Left);
            Functions_InteractiveObjs.SetType(objList[1], InteractiveType.House_Wall_Back);
            Functions_InteractiveObjs.SetType(objList[2], InteractiveType.House_Door_Open);
            Functions_InteractiveObjs.SetType(objList[3], InteractiveType.House_Wall_Side_Right);

            //row 2
            Functions_InteractiveObjs.SetType(objList[4], InteractiveType.House_Wall_FrontA);
            Functions_InteractiveObjs.SetType(objList[5], InteractiveType.House_Wall_FrontB);
            Functions_InteractiveObjs.SetType(objList[6], InteractiveType.House_Door_Shut);
            Functions_InteractiveObjs.SetType(objList[7], InteractiveType.House_Bed);

            //row 3
            Functions_InteractiveObjs.SetType(objList[8], InteractiveType.House_Roof_Top);
            Functions_InteractiveObjs.SetType(objList[9], InteractiveType.House_Roof_Chimney);
            Functions_InteractiveObjs.SetType(objList[10], InteractiveType.House_Roof_Bottom);
            Functions_InteractiveObjs.SetType(objList[11], InteractiveType.Unknown); //coverd

            //row 4
            Functions_InteractiveObjs.SetType(objList[12], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[13], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[14], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[15], InteractiveType.Unknown);

            //row 5
            Functions_InteractiveObjs.SetType(objList[16], InteractiveType.Boat_Floor);
            Functions_InteractiveObjs.SetType(objList[17], InteractiveType.Coliseum_Shadow_Stairs_Left);
            Functions_InteractiveObjs.SetType(objList[18], InteractiveType.Coliseum_Shadow_Stairs_Middle);
            Functions_InteractiveObjs.SetType(objList[19], InteractiveType.Coliseum_Shadow_Stairs_Right);

            //row 6
            Functions_InteractiveObjs.SetType(objList[20], InteractiveType.Boat_Floor_Burned);
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
            Functions_InteractiveObjs.SetType(objList[36], InteractiveType.House_TableSingle);
            Functions_InteractiveObjs.SetType(objList[37], InteractiveType.House_TableDoubleLeft);
            Functions_InteractiveObjs.SetType(objList[38], InteractiveType.House_TableDoubleRight);
            Functions_InteractiveObjs.SetType(objList[39], InteractiveType.Unknown);

            //row 11
            Functions_InteractiveObjs.SetType(objList[40], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[41], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[42], InteractiveType.House_Chair);
            Functions_InteractiveObjs.SetType(objList[43], InteractiveType.Unknown);

            //row 12
            Functions_InteractiveObjs.SetType(objList[44], InteractiveType.House_Bookcase);
            Functions_InteractiveObjs.SetType(objList[45], InteractiveType.House_Shelf);
            Functions_InteractiveObjs.SetType(objList[46], InteractiveType.House_Stove);
            Functions_InteractiveObjs.SetType(objList[47], InteractiveType.House_Sink);

            //position the objs relative to the window frame
            Functions_WidgetIntObjs.PositionObjs(this);

            Functions_WidgetIntObjs.HideObj(objList[11]);

            //reset update all zdepths
            for (i = 0; i < 4 * 12; i++)
            {
                objList[i].compSprite.zOffset = 0;
                Functions_Component.SetZdepth(objList[i].compSprite);
            }
        }
    }

    public class WidgetObjects_NPCS : WidgetIntObject
    {
        public WidgetObjects_NPCS()
        {   //4th left
            window = new MenuWindow(
                new Point(16 * 17 + 8, 16 * 2),
                new Point(16 * 5, 16 * 15), //size
                "NPC Objects"); //title

            objList = new List<InteractiveObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new InteractiveObject()); }

            //row 1
            Functions_InteractiveObjs.SetType(objList[0], InteractiveType.NPC_Story);
            Functions_InteractiveObjs.SetType(objList[1], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[2], InteractiveType.NPC_Farmer);
            Functions_InteractiveObjs.SetType(objList[3], InteractiveType.NPC_Farmer_Reward);

            //row 2
            Functions_InteractiveObjs.SetType(objList[4], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[5], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[6], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[7], InteractiveType.Unknown);

            //row 3
            Functions_InteractiveObjs.SetType(objList[8], InteractiveType.Vendor_Armor);
            Functions_InteractiveObjs.SetType(objList[9], InteractiveType.Vendor_Equipment);
            Functions_InteractiveObjs.SetType(objList[10], InteractiveType.Vendor_Items);
            Functions_InteractiveObjs.SetType(objList[11], InteractiveType.Vendor_Magic);

            //row 4
            Functions_InteractiveObjs.SetType(objList[12], InteractiveType.Vendor_Potions);
            Functions_InteractiveObjs.SetType(objList[13], InteractiveType.Vendor_Weapons);
            Functions_InteractiveObjs.SetType(objList[14], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[15], InteractiveType.Vendor_Pets);

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
            Functions_InteractiveObjs.SetType(objList[28], InteractiveType.Vendor_Colliseum_Mob);
            Functions_InteractiveObjs.SetType(objList[29], InteractiveType.Vendor_EnemyItems);
            Functions_InteractiveObjs.SetType(objList[30], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[31], InteractiveType.Unknown);

            //row 9
            Functions_InteractiveObjs.SetType(objList[32], InteractiveType.Pet_Dog);
            Functions_InteractiveObjs.SetType(objList[33], InteractiveType.Pet_Chicken);
            Functions_InteractiveObjs.SetType(objList[34], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[35], InteractiveType.Unknown);

            //row 10
            Functions_InteractiveObjs.SetType(objList[36], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[37], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[38], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[39], InteractiveType.Unknown);

            //row 11
            Functions_InteractiveObjs.SetType(objList[40], InteractiveType.Enemy_Turtle);
            Functions_InteractiveObjs.SetType(objList[41], InteractiveType.Enemy_Crab);
            Functions_InteractiveObjs.SetType(objList[42], InteractiveType.Enemy_Rat);
            Functions_InteractiveObjs.SetType(objList[43], InteractiveType.Unknown);

            //row 12
            Functions_InteractiveObjs.SetType(objList[44], InteractiveType.Enemy_SeekerExploder);
            Functions_InteractiveObjs.SetType(objList[45], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[46], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[47], InteractiveType.Unknown);

            //position the objs relative to the window frame
            Functions_WidgetIntObjs.PositionObjs(this);

            //Functions_WidgetObject.HideObj(objList[5]);

            //reset update all zdepths
            for (i = 0; i < 4 * 12; i++)
            {
                objList[i].compSprite.zOffset = 0;
                Functions_Component.SetZdepth(objList[i].compSprite);
            }
        }
    }

    public class WidgetObjects_Dev1 : WidgetIntObject
    {
        public WidgetObjects_Dev1()
        {   //5th
            window = new MenuWindow(
                new Point(16 * 23, 16 * 2),
                new Point(16 * 5, 16 * 15), //size
                "Dev1"); //title

            objList = new List<InteractiveObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new InteractiveObject()); }

            //row 1
            Functions_InteractiveObjs.SetType(objList[0], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[1], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[2], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[3], InteractiveType.Unknown);

            //row 2
            Functions_InteractiveObjs.SetType(objList[4], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[5], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[6], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[7], InteractiveType.Unknown);

            //row 3
            Functions_InteractiveObjs.SetType(objList[8], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[9], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[10], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[11], InteractiveType.Unknown);

            //row 4
            Functions_InteractiveObjs.SetType(objList[12], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[13], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[14], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[15], InteractiveType.Unknown);

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

            //row 12 - cheat and put these beyond widget bounds cause they big
            Functions_InteractiveObjs.SetType(objList[44], InteractiveType.Coliseum_Shadow_Gate_Pillar_Left);
            Functions_InteractiveObjs.SetType(objList[45], InteractiveType.Unknown);//covered by left pillar
            Functions_InteractiveObjs.SetType(objList[46], InteractiveType.Coliseum_Shadow_Gate_Center);
            Functions_InteractiveObjs.SetType(objList[47], InteractiveType.Coliseum_Shadow_Gate_Pillar_Right);//


            //position the objs relative to the window frame
            Functions_WidgetIntObjs.PositionObjs(this);

            //hide covered objs
            Functions_WidgetIntObjs.HideObj(objList[45]);

            //reset update all zdepths
            for (i = 0; i < 4 * 12; i++)
            {
                objList[i].compSprite.zOffset = 0;
                Functions_Component.SetZdepth(objList[i].compSprite);
            }
        }
    }

    public class WidgetObjects_Dev2 : WidgetIntObject
    {
        public WidgetObjects_Dev2()
        {   //5th
            window = new MenuWindow(
                new Point(16 * 28 + 8, 16 * 2),
                new Point(16 * 5, 16 * 15), //size
                "Dev2"); //title

            objList = new List<InteractiveObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new InteractiveObject()); }

            //row 1
            Functions_InteractiveObjs.SetType(objList[0], InteractiveType.Boat_Pier_TopLeft);
            Functions_InteractiveObjs.SetType(objList[1], InteractiveType.Boat_Pier_TopMiddle);
            Functions_InteractiveObjs.SetType(objList[2], InteractiveType.Boat_Pier_TopRight);
            Functions_InteractiveObjs.SetType(objList[3], InteractiveType.Unknown);

            //row 2
            Functions_InteractiveObjs.SetType(objList[4], InteractiveType.Boat_Pier_Left);
            Functions_InteractiveObjs.SetType(objList[5], InteractiveType.Boat_Pier_Middle);
            Functions_InteractiveObjs.SetType(objList[6], InteractiveType.Boat_Pier_Right);
            Functions_InteractiveObjs.SetType(objList[7], InteractiveType.Unknown);

            //row 3
            Functions_InteractiveObjs.SetType(objList[8], InteractiveType.Boat_Pier_BottomLeft);
            Functions_InteractiveObjs.SetType(objList[9], InteractiveType.Boat_Pier_BottomMiddle);
            Functions_InteractiveObjs.SetType(objList[10], InteractiveType.Boat_Pier_BottomRight);
            Functions_InteractiveObjs.SetType(objList[11], InteractiveType.Unknown);

            //row 4
            Functions_InteractiveObjs.SetType(objList[12], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[13], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[14], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[15], InteractiveType.Unknown);

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

            //row 12 - cheat and put these beyond widget bounds cause they big
            Functions_InteractiveObjs.SetType(objList[44], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[45], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[46], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[47], InteractiveType.Unknown);


            //position the objs relative to the window frame
            Functions_WidgetIntObjs.PositionObjs(this);

            //hide covered objs
            //Functions_WidgetIntObjs.HideObj(objList[45]);

            //reset update all zdepths
            for (i = 0; i < 4 * 12; i++)
            {
                objList[i].compSprite.zOffset = 0;
                Functions_Component.SetZdepth(objList[i].compSprite);
            }
        }
    }

    public class WidgetObjects_Dungeon : WidgetIntObject
    {   //NOTE: we can update dungeon widget object's texture to see diff dungeons
        public WidgetObjects_Dungeon()
        {   //far right
            window = new MenuWindow(
                new Point(16 * 34, 16 * 2), //position
                new Point(16 * 5, 16 * 15), //size
                "Dungeon Objs"); //title

            objList = new List<InteractiveObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new InteractiveObject()); }

            //row 1
            Functions_InteractiveObjs.SetType(objList[0], InteractiveType.Dungeon_Statue);
            Functions_InteractiveObjs.SetType(objList[1], InteractiveType.Dungeon_BlockLight);
            Functions_InteractiveObjs.SetType(objList[2], InteractiveType.Dungeon_BlockSpike);
            Functions_InteractiveObjs.SetType(objList[3], InteractiveType.Dungeon_Switch);

            //row 2
            Functions_InteractiveObjs.SetType(objList[4], InteractiveType.Dungeon_SwitchBlockBtn);
            Functions_InteractiveObjs.SetType(objList[5], InteractiveType.Dungeon_SwitchBlockDown);
            Functions_InteractiveObjs.SetType(objList[6], InteractiveType.Dungeon_SwitchBlockUp);
            Functions_InteractiveObjs.SetType(objList[7], InteractiveType.Dungeon_SwitchDownPerm);

            //row 3
            Functions_InteractiveObjs.SetType(objList[8], InteractiveType.Chest);
            Functions_InteractiveObjs.SetType(objList[9], InteractiveType.ChestEmpty);
            Functions_InteractiveObjs.SetType(objList[10], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[11], InteractiveType.Unknown);

            //row 4
            Functions_InteractiveObjs.SetType(objList[12], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[13], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[14], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[15], InteractiveType.Unknown);

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
            Functions_InteractiveObjs.SetType(objList[28], InteractiveType.Dungeon_Map);
            Functions_InteractiveObjs.SetType(objList[29], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[30], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[31], InteractiveType.LeverOff);

            //row 9
            Functions_InteractiveObjs.SetType(objList[32], InteractiveType.Dungeon_SpikesFloorOn);
            Functions_InteractiveObjs.SetType(objList[33], InteractiveType.Dungeon_SpikesFloorOff);
            Functions_InteractiveObjs.SetType(objList[34], InteractiveType.ConveyorBeltOn);
            Functions_InteractiveObjs.SetType(objList[35], InteractiveType.ConveyorBeltOff);

            //row 10
            Functions_InteractiveObjs.SetType(objList[36], InteractiveType.Flamethrower); 
            Functions_InteractiveObjs.SetType(objList[37], InteractiveType.TorchUnlit);
            Functions_InteractiveObjs.SetType(objList[38], InteractiveType.TorchLit);
            Functions_InteractiveObjs.SetType(objList[39], InteractiveType.Bumper);

            //row 11
            Functions_InteractiveObjs.SetType(objList[40], InteractiveType.Fairy);
            Functions_InteractiveObjs.SetType(objList[41], InteractiveType.FloorBlood);
            Functions_InteractiveObjs.SetType(objList[42], InteractiveType.FloorSkeleton);
            Functions_InteractiveObjs.SetType(objList[43], InteractiveType.Unknown);

            //row 12
            Functions_InteractiveObjs.SetType(objList[44], InteractiveType.Dungeon_SpawnMob);
            Functions_InteractiveObjs.SetType(objList[45], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[46], InteractiveType.Unknown);
            Functions_InteractiveObjs.SetType(objList[47], InteractiveType.Unknown);

            //position the objs relative to the window frame
            Functions_WidgetIntObjs.PositionObjs(this);
            

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