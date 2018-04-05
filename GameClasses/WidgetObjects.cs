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

    public static class Functions_Widget
    {
        //need a function to set the objList items relative to the window frame
        //they will always sort to these positions, and we need a null check
        static int i = 0;
        static int row = 0;
        static int counter = 0;

        public static void PositionObjs(WidgetObject WO)
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
                    }

                    counter++;
                }
            }
        }

    }


    public class WidgetObject : Widget
    {
        public List<GameObject> objList;

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


    public class WidgetObjects_Dungeon : WidgetObject
    {
        public WidgetObjects_Dungeon()
        {   //create and set the position of the window frame
            window = new MenuWindow(
                new Point(16 * 1, 16 * 2), //position
                new Point(16 * 5, 16 * 15), //size
                "Dungeon Objs"); //title

            objList = new List<GameObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new GameObject()); }

            //row 1
            Functions_GameObject.SetType(objList[0], ObjType.Dungeon_BlockDark);
            Functions_GameObject.SetType(objList[1], ObjType.Dungeon_BlockLight);
            Functions_GameObject.SetType(objList[2], ObjType.Dungeon_BlockSpike);
            Functions_GameObject.SetType(objList[3], ObjType.Dungeon_SwitchBlockDown);

            //row 2
            Functions_GameObject.SetType(objList[4], ObjType.Dungeon_SwitchBlockBtn);
            Functions_GameObject.SetType(objList[5], ObjType.Dungeon_SwitchBlockDown);
            Functions_GameObject.SetType(objList[6], ObjType.Dungeon_SwitchBlockUp);
            Functions_GameObject.SetType(objList[7], ObjType.Dungeon_IceTile);

            //row 3
            Functions_GameObject.SetType(objList[8], ObjType.Dungeon_Pit);
            Functions_GameObject.SetType(objList[9], ObjType.Dungeon_PitBridge);
            Functions_GameObject.SetType(objList[10], ObjType.Dungeon_PitTeethTop);
            Functions_GameObject.SetType(objList[11], ObjType.Dungeon_PitTeethBottom);

            //row 4
            Functions_GameObject.SetType(objList[12], ObjType.Dungeon_Bumper);
            Functions_GameObject.SetType(objList[13], ObjType.Dungeon_Switch);
            Functions_GameObject.SetType(objList[14], ObjType.Dungeon_SwitchOff);
            Functions_GameObject.SetType(objList[15], ObjType.Dungeon_Flamethrower);

            //row 5
            Functions_GameObject.SetType(objList[16], ObjType.Dungeon_Barrel);
            Functions_GameObject.SetType(objList[17], ObjType.Dungeon_Pot);
            //algo decides if chest if key or map
            Functions_GameObject.SetType(objList[18], ObjType.Dungeon_ChestKey); 
            //^^^ should just be a chest obj
            Functions_GameObject.SetType(objList[19], ObjType.Dungeon_ChestEmpty);

            //row 6
            Functions_GameObject.SetType(objList[20], ObjType.Dungeon_TorchUnlit);
            Functions_GameObject.SetType(objList[21], ObjType.Dungeon_TorchLit);
            Functions_GameObject.SetType(objList[22], ObjType.Dungeon_SwitchBlockDown);
            Functions_GameObject.SetType(objList[23], ObjType.Dungeon_WallTorch);

            //row 7
            Functions_GameObject.SetType(objList[24], ObjType.Dungeon_SpikesFloorOn);
            Functions_GameObject.SetType(objList[25], ObjType.Dungeon_SpikesFloorOff);
            Functions_GameObject.SetType(objList[26], ObjType.Dungeon_ConveyorBeltOn);
            Functions_GameObject.SetType(objList[27], ObjType.Dungeon_ConveyorBeltOff);

            //row 8
            Functions_GameObject.SetType(objList[28], ObjType.Dungeon_PitTrap);
            Functions_GameObject.SetType(objList[29], ObjType.Dungeon_SwitchBlockDown);
            Functions_GameObject.SetType(objList[30], ObjType.Dungeon_SwitchBlockDown);
            Functions_GameObject.SetType(objList[31], ObjType.Dungeon_LeverOff);

            //row 9
            Functions_GameObject.SetType(objList[32], ObjType.Dungeon_SwitchBlockDown);
            Functions_GameObject.SetType(objList[33], ObjType.Dungeon_SwitchBlockDown);
            Functions_GameObject.SetType(objList[34], ObjType.Dungeon_SwitchBlockDown);
            Functions_GameObject.SetType(objList[35], ObjType.Dungeon_SwitchBlockDown);

            //row 10
            Functions_GameObject.SetType(objList[36], ObjType.Dungeon_SwitchBlockDown);
            Functions_GameObject.SetType(objList[37], ObjType.Dungeon_SwitchBlockDown);
            Functions_GameObject.SetType(objList[38], ObjType.Dungeon_SwitchBlockDown);
            Functions_GameObject.SetType(objList[39], ObjType.Dungeon_SwitchBlockDown);

            //row 11
            Functions_GameObject.SetType(objList[40], ObjType.Dungeon_SwitchBlockDown);
            Functions_GameObject.SetType(objList[41], ObjType.Dungeon_SwitchBlockDown);
            Functions_GameObject.SetType(objList[42], ObjType.Dungeon_SwitchBlockDown);
            Functions_GameObject.SetType(objList[43], ObjType.Dungeon_SwitchBlockDown);

            //row 12
            Functions_GameObject.SetType(objList[44], ObjType.Dungeon_SwitchBlockDown);
            Functions_GameObject.SetType(objList[45], ObjType.Dungeon_SwitchBlockDown);
            Functions_GameObject.SetType(objList[46], ObjType.Dungeon_SwitchBlockDown);
            Functions_GameObject.SetType(objList[47], ObjType.Dungeon_SwitchBlockDown);

            //position the objs relative to the window frame
            Functions_Widget.PositionObjs(this);
        }
    }


}