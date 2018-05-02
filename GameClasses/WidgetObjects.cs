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

        public static void HideObj(GameObject Obj)
        {   //hide this widget object offscreen
            Functions_Movement.Teleport(Obj.compMove, 2048, 2048);
            Functions_Component.Align(Obj);
            Obj.compCollision.rec.X = 2048;
            Obj.compCollision.rec.Y = 2048;
            Obj.compCollision.rec.Width = 1;
            Obj.compCollision.rec.Height = 1;
        }

    }



    public class WidgetObject : Widget
    {
        public List<GameObject> objList;

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


    //on the left

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
            Functions_GameObject.SetType(objList[3], ObjType.Dungeon_Fairy);

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
            Functions_GameObject.SetType(objList[18], ObjType.Dungeon_Chest); 
            Functions_GameObject.SetType(objList[19], ObjType.Dungeon_ChestEmpty);

            //row 6
            Functions_GameObject.SetType(objList[20], ObjType.Dungeon_TorchUnlit);
            Functions_GameObject.SetType(objList[21], ObjType.Dungeon_TorchLit);
            Functions_GameObject.SetType(objList[22], ObjType.Unknown);
            Functions_GameObject.SetType(objList[23], ObjType.Dungeon_WallTorch);

            //row 7
            Functions_GameObject.SetType(objList[24], ObjType.Dungeon_SpikesFloorOn);
            Functions_GameObject.SetType(objList[25], ObjType.Dungeon_SpikesFloorOff);
            Functions_GameObject.SetType(objList[26], ObjType.Dungeon_ConveyorBeltOn);
            Functions_GameObject.SetType(objList[27], ObjType.Dungeon_ConveyorBeltOff);

            //row 8
            Functions_GameObject.SetType(objList[28], ObjType.Dungeon_PitTrap);
            Functions_GameObject.SetType(objList[29], ObjType.Dungeon_SwitchBlockUp);
            Functions_GameObject.SetType(objList[30], ObjType.Dungeon_SwitchBlockDown);
            Functions_GameObject.SetType(objList[31], ObjType.Dungeon_LeverOff);

            //row 9
            Functions_GameObject.SetType(objList[32], ObjType.Unknown);
            Functions_GameObject.SetType(objList[33], ObjType.Unknown);
            Functions_GameObject.SetType(objList[34], ObjType.Unknown);
            Functions_GameObject.SetType(objList[35], ObjType.Unknown);

            //row 10
            Functions_GameObject.SetType(objList[36], ObjType.Unknown);
            Functions_GameObject.SetType(objList[37], ObjType.Unknown);
            Functions_GameObject.SetType(objList[38], ObjType.Dungeon_WallInteriorCorner);
            Functions_GameObject.SetType(objList[39], ObjType.Dungeon_WallExteriorCorner);

            //row 11
            Functions_GameObject.SetType(objList[40], ObjType.Dungeon_WallStraight);
            Functions_GameObject.SetType(objList[41], ObjType.Dungeon_WallTorch);
            Functions_GameObject.SetType(objList[42], ObjType.Dungeon_WallPillar);
            Functions_GameObject.SetType(objList[43], ObjType.Dungeon_WallStatue);

            //row 12
            Functions_GameObject.SetType(objList[44], ObjType.Dungeon_DoorTrap);
            Functions_GameObject.SetType(objList[45], ObjType.Dungeon_DoorShut);
            Functions_GameObject.SetType(objList[46], ObjType.Dungeon_DoorFake);
            Functions_GameObject.SetType(objList[47], ObjType.Dungeon_DoorBombable);

            //position the objs relative to the window frame
            Functions_Widget.PositionObjs(this);
        }
    }

    public class WidgetObjects_Environment : WidgetObject
    {
        public WidgetObjects_Environment()
        {   //create and set the position of the window frame
            window = new MenuWindow(
                new Point(16 * 1, 16 * 2), //position
                new Point(16 * 5, 16 * 15), //size
                "Environment Objs"); //title

            objList = new List<GameObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new GameObject()); }

            //row 1
            Functions_GameObject.SetType(objList[0], ObjType.Wor_Grass_Tall);
            Functions_GameObject.SetType(objList[1], ObjType.Wor_Grass_Cut);
            Functions_GameObject.SetType(objList[2], ObjType.Wor_Grass_1);
            Functions_GameObject.SetType(objList[3], ObjType.Wor_Grass_2);

            //row 2
            Functions_GameObject.SetType(objList[4], ObjType.Wor_Bush);
            Functions_GameObject.SetType(objList[5], ObjType.Wor_Bush_Stump);
            Functions_GameObject.SetType(objList[6], ObjType.Wor_Pot);
            Functions_GameObject.SetType(objList[7], ObjType.Wor_Flowers);

            //row 3
            Functions_GameObject.SetType(objList[8], ObjType.Wor_Tree);
            Functions_GameObject.SetType(objList[9], ObjType.Wor_Tree_Stump);
            Functions_GameObject.SetType(objList[10], ObjType.Unknown);
            Functions_GameObject.SetType(objList[11], ObjType.Unknown);
            
            //row 4
            Functions_GameObject.SetType(objList[12], ObjType.Unknown);
            Functions_GameObject.SetType(objList[13], ObjType.Unknown);
            Functions_GameObject.SetType(objList[14], ObjType.Unknown);
            Functions_GameObject.SetType(objList[15], ObjType.Unknown);

            //row 5
            Functions_GameObject.SetType(objList[16], ObjType.Unknown);
            Functions_GameObject.SetType(objList[17], ObjType.Unknown);
            Functions_GameObject.SetType(objList[18], ObjType.Unknown);
            Functions_GameObject.SetType(objList[19], ObjType.Unknown);

            //row 6
            Functions_GameObject.SetType(objList[20], ObjType.Unknown);
            Functions_GameObject.SetType(objList[21], ObjType.Unknown);
            Functions_GameObject.SetType(objList[22], ObjType.Unknown);
            Functions_GameObject.SetType(objList[23], ObjType.Unknown);

            //row 7
            Functions_GameObject.SetType(objList[24], ObjType.Unknown);
            Functions_GameObject.SetType(objList[25], ObjType.Unknown);
            Functions_GameObject.SetType(objList[26], ObjType.Unknown);
            Functions_GameObject.SetType(objList[27], ObjType.Unknown);

            //row 8
            Functions_GameObject.SetType(objList[28], ObjType.Unknown);
            Functions_GameObject.SetType(objList[29], ObjType.Unknown);
            Functions_GameObject.SetType(objList[30], ObjType.Unknown);
            Functions_GameObject.SetType(objList[31], ObjType.Unknown);

            //row 9
            Functions_GameObject.SetType(objList[32], ObjType.Unknown);
            Functions_GameObject.SetType(objList[33], ObjType.Unknown);
            Functions_GameObject.SetType(objList[34], ObjType.Unknown);
            Functions_GameObject.SetType(objList[35], ObjType.Unknown);

            //row 10
            Functions_GameObject.SetType(objList[36], ObjType.Unknown);
            Functions_GameObject.SetType(objList[37], ObjType.Unknown);
            Functions_GameObject.SetType(objList[38], ObjType.Unknown);
            Functions_GameObject.SetType(objList[39], ObjType.Unknown);

            //row 11
            Functions_GameObject.SetType(objList[40], ObjType.Unknown);
            Functions_GameObject.SetType(objList[41], ObjType.Unknown);
            Functions_GameObject.SetType(objList[42], ObjType.Unknown);
            Functions_GameObject.SetType(objList[43], ObjType.Unknown);

            //row 12
            Functions_GameObject.SetType(objList[44], ObjType.Unknown);
            Functions_GameObject.SetType(objList[45], ObjType.Unknown);
            Functions_GameObject.SetType(objList[46], ObjType.Unknown);
            Functions_GameObject.SetType(objList[47], ObjType.Unknown);

            //position the objs relative to the window frame
            Functions_Widget.PositionObjs(this);

            //manually set tree recs
            objList[8].compCollision.rec.Height = 16 * 2;
            objList[9].compCollision.rec.Height = 16 * 2;

            //hide objs that trees overlap
            Functions_Widget.HideObj(objList[12]);
            Functions_Widget.HideObj(objList[13]);
        }
    }



    //on the right

    public class WidgetObjects_Building : WidgetObject
    {
        public WidgetObjects_Building()
        {   //create and set the position of the window frame
            window = new MenuWindow(
                new Point(16 * 34, 16 * 2), //position
                new Point(16 * 5, 16 * 15), //size
                "Building Objs"); //title

            objList = new List<GameObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new GameObject()); }

            //row 1
            Functions_GameObject.SetType(objList[0], ObjType.Unknown);
            Functions_GameObject.SetType(objList[1], ObjType.Unknown);
            Functions_GameObject.SetType(objList[2], ObjType.Unknown);
            Functions_GameObject.SetType(objList[3], ObjType.Unknown);

            //row 2
            Functions_GameObject.SetType(objList[4], ObjType.Unknown);
            Functions_GameObject.SetType(objList[5], ObjType.Unknown);
            Functions_GameObject.SetType(objList[6], ObjType.Unknown);
            Functions_GameObject.SetType(objList[7], ObjType.Unknown);

            //row 3
            Functions_GameObject.SetType(objList[8], ObjType.Unknown);
            Functions_GameObject.SetType(objList[9], ObjType.Unknown);
            Functions_GameObject.SetType(objList[10], ObjType.Unknown);
            Functions_GameObject.SetType(objList[11], ObjType.Unknown);

            //row 4
            Functions_GameObject.SetType(objList[12], ObjType.Unknown);
            Functions_GameObject.SetType(objList[13], ObjType.Unknown);
            Functions_GameObject.SetType(objList[14], ObjType.Unknown);
            Functions_GameObject.SetType(objList[15], ObjType.Unknown);

            //row 5
            Functions_GameObject.SetType(objList[16], ObjType.Unknown);
            Functions_GameObject.SetType(objList[17], ObjType.Unknown);
            Functions_GameObject.SetType(objList[18], ObjType.Unknown);
            Functions_GameObject.SetType(objList[19], ObjType.Unknown);

            //row 6
            Functions_GameObject.SetType(objList[20], ObjType.Unknown);
            Functions_GameObject.SetType(objList[21], ObjType.Unknown);
            Functions_GameObject.SetType(objList[22], ObjType.Unknown);
            Functions_GameObject.SetType(objList[23], ObjType.Unknown);

            //row 7
            Functions_GameObject.SetType(objList[24], ObjType.Unknown);
            Functions_GameObject.SetType(objList[25], ObjType.Unknown);
            Functions_GameObject.SetType(objList[26], ObjType.Unknown);
            Functions_GameObject.SetType(objList[27], ObjType.Unknown);

            //row 8
            Functions_GameObject.SetType(objList[28], ObjType.Unknown);
            Functions_GameObject.SetType(objList[29], ObjType.Unknown);
            Functions_GameObject.SetType(objList[30], ObjType.Unknown);
            Functions_GameObject.SetType(objList[31], ObjType.Unknown);

            //row 9
            Functions_GameObject.SetType(objList[32], ObjType.Unknown);
            Functions_GameObject.SetType(objList[33], ObjType.Unknown);
            Functions_GameObject.SetType(objList[34], ObjType.Unknown);
            Functions_GameObject.SetType(objList[35], ObjType.Unknown);

            //row 10
            Functions_GameObject.SetType(objList[36], ObjType.Unknown);
            Functions_GameObject.SetType(objList[37], ObjType.Unknown);
            Functions_GameObject.SetType(objList[38], ObjType.Unknown);
            Functions_GameObject.SetType(objList[39], ObjType.Unknown);

            //row 11
            Functions_GameObject.SetType(objList[40], ObjType.Unknown);
            Functions_GameObject.SetType(objList[41], ObjType.Unknown);
            Functions_GameObject.SetType(objList[42], ObjType.Unknown);
            Functions_GameObject.SetType(objList[43], ObjType.Unknown);

            //row 12
            Functions_GameObject.SetType(objList[44], ObjType.Wor_Bookcase);
            Functions_GameObject.SetType(objList[45], ObjType.Wor_Shelf);
            Functions_GameObject.SetType(objList[46], ObjType.Wor_TableStone);
            Functions_GameObject.SetType(objList[47], ObjType.Unknown);

            //position the objs relative to the window frame
            Functions_Widget.PositionObjs(this);
        }
    }

    public class WidgetObjects_Enemy : WidgetObject
    {
        public WidgetObjects_Enemy()
        {   //create and set the position of the window frame
            window = new MenuWindow(
                new Point(16 * 34, 16 * 2), //position
                new Point(16 * 5, 16 * 15), //size
                "Enemy Tools"); //title

            objList = new List<GameObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new GameObject()); }

            //row 1
            Functions_GameObject.SetType(objList[0], ObjType.Dungeon_SpawnMob);
            Functions_GameObject.SetType(objList[1], ObjType.Dungeon_SpawnMob);
            Functions_GameObject.SetType(objList[2], ObjType.Dungeon_SpawnMob);
            Functions_GameObject.SetType(objList[3], ObjType.Dungeon_SpawnMob);

            //row 2
            Functions_GameObject.SetType(objList[4], ObjType.Dungeon_SpawnMiniBoss);
            Functions_GameObject.SetType(objList[5], ObjType.Dungeon_SpawnMiniBoss);
            Functions_GameObject.SetType(objList[6], ObjType.Dungeon_SpawnMiniBoss);
            Functions_GameObject.SetType(objList[7], ObjType.Dungeon_SpawnMiniBoss);

            //row 3
            Functions_GameObject.SetType(objList[8], ObjType.Unknown);
            Functions_GameObject.SetType(objList[9], ObjType.Unknown);
            Functions_GameObject.SetType(objList[10], ObjType.Unknown);
            Functions_GameObject.SetType(objList[11], ObjType.Unknown);

            //row 4
            Functions_GameObject.SetType(objList[12], ObjType.Unknown);
            Functions_GameObject.SetType(objList[13], ObjType.Unknown);
            Functions_GameObject.SetType(objList[14], ObjType.Unknown);
            Functions_GameObject.SetType(objList[15], ObjType.Unknown);

            //row 5
            Functions_GameObject.SetType(objList[16], ObjType.Unknown);
            Functions_GameObject.SetType(objList[17], ObjType.Unknown);
            Functions_GameObject.SetType(objList[18], ObjType.Unknown);
            Functions_GameObject.SetType(objList[19], ObjType.Unknown);

            //row 6
            Functions_GameObject.SetType(objList[20], ObjType.Unknown);
            Functions_GameObject.SetType(objList[21], ObjType.Unknown);
            Functions_GameObject.SetType(objList[22], ObjType.Unknown);
            Functions_GameObject.SetType(objList[23], ObjType.Unknown);

            //row 7
            Functions_GameObject.SetType(objList[24], ObjType.Unknown);
            Functions_GameObject.SetType(objList[25], ObjType.Unknown);
            Functions_GameObject.SetType(objList[26], ObjType.Unknown);
            Functions_GameObject.SetType(objList[27], ObjType.Unknown);

            //row 8
            Functions_GameObject.SetType(objList[28], ObjType.Unknown);
            Functions_GameObject.SetType(objList[29], ObjType.Unknown);
            Functions_GameObject.SetType(objList[30], ObjType.Unknown);
            Functions_GameObject.SetType(objList[31], ObjType.Unknown);

            //row 9
            Functions_GameObject.SetType(objList[32], ObjType.Unknown);
            Functions_GameObject.SetType(objList[33], ObjType.Unknown);
            Functions_GameObject.SetType(objList[34], ObjType.Unknown);
            Functions_GameObject.SetType(objList[35], ObjType.Unknown);

            //row 10
            Functions_GameObject.SetType(objList[36], ObjType.Unknown);
            Functions_GameObject.SetType(objList[37], ObjType.Unknown);
            Functions_GameObject.SetType(objList[38], ObjType.Unknown);
            Functions_GameObject.SetType(objList[39], ObjType.Unknown);

            //row 11
            Functions_GameObject.SetType(objList[40], ObjType.Unknown);
            Functions_GameObject.SetType(objList[41], ObjType.Unknown);
            Functions_GameObject.SetType(objList[42], ObjType.Unknown);
            Functions_GameObject.SetType(objList[43], ObjType.Unknown);

            //row 12
            Functions_GameObject.SetType(objList[44], ObjType.Unknown);
            Functions_GameObject.SetType(objList[45], ObjType.Unknown);
            Functions_GameObject.SetType(objList[46], ObjType.Unknown);
            Functions_GameObject.SetType(objList[47], ObjType.Unknown);

            //position the objs relative to the window frame
            Functions_Widget.PositionObjs(this);
        }
    }

    //on the left, but a little right

    public class WidgetObjects_Shared : WidgetObject
    {
        public WidgetObjects_Shared()
        {   //create and set the position of the window frame
            window = new MenuWindow(
                new Point(16 * 7, 16 * 2), //position
                new Point(16 * 5, 16 * 15), //size
                "shared + npcs"); //title

            objList = new List<GameObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new GameObject()); }

            //row 1

            //this is a 3x4 * 16 sized sprite/obj
            Functions_GameObject.SetType(objList[0], ObjType.Wor_Entrance_ForestDungeon);
            Functions_GameObject.SetType(objList[1], ObjType.Unknown);
            Functions_GameObject.SetType(objList[2], ObjType.Unknown);
            Functions_GameObject.SetType(objList[3], ObjType.Dungeon_Fairy);

            //row 2
            Functions_GameObject.SetType(objList[4], ObjType.Unknown);
            Functions_GameObject.SetType(objList[5], ObjType.Unknown);
            Functions_GameObject.SetType(objList[6], ObjType.Unknown);
            Functions_GameObject.SetType(objList[7], ObjType.Dungeon_Fairy);

            //row 3
            Functions_GameObject.SetType(objList[8], ObjType.Unknown);
            Functions_GameObject.SetType(objList[9], ObjType.Unknown);
            Functions_GameObject.SetType(objList[10], ObjType.Unknown);
            Functions_GameObject.SetType(objList[11], ObjType.Dungeon_Fairy);

            //row 4
            Functions_GameObject.SetType(objList[12], ObjType.Unknown);
            Functions_GameObject.SetType(objList[13], ObjType.Unknown);
            Functions_GameObject.SetType(objList[14], ObjType.Unknown);
            Functions_GameObject.SetType(objList[15], ObjType.Dungeon_Statue);

            //row 5
            Functions_GameObject.SetType(objList[16], ObjType.Unknown);
            Functions_GameObject.SetType(objList[17], ObjType.Unknown);
            Functions_GameObject.SetType(objList[18], ObjType.Unknown);
            Functions_GameObject.SetType(objList[19], ObjType.Unknown);

            //row 6
            Functions_GameObject.SetType(objList[20], ObjType.Unknown);
            Functions_GameObject.SetType(objList[21], ObjType.Unknown);
            Functions_GameObject.SetType(objList[22], ObjType.Unknown);
            Functions_GameObject.SetType(objList[23], ObjType.Unknown);

            //row 7
            Functions_GameObject.SetType(objList[24], ObjType.Unknown);
            Functions_GameObject.SetType(objList[25], ObjType.Unknown);
            Functions_GameObject.SetType(objList[26], ObjType.Unknown);
            Functions_GameObject.SetType(objList[27], ObjType.Unknown);

            //row 8
            Functions_GameObject.SetType(objList[28], ObjType.Unknown);
            Functions_GameObject.SetType(objList[29], ObjType.Unknown);
            Functions_GameObject.SetType(objList[30], ObjType.Unknown);
            Functions_GameObject.SetType(objList[31], ObjType.Unknown);

            //row 9
            Functions_GameObject.SetType(objList[32], ObjType.Unknown);
            Functions_GameObject.SetType(objList[33], ObjType.Unknown);
            Functions_GameObject.SetType(objList[34], ObjType.Unknown);
            Functions_GameObject.SetType(objList[35], ObjType.Unknown);

            //row 10
            Functions_GameObject.SetType(objList[36], ObjType.Pet_Dog);
            Functions_GameObject.SetType(objList[37], ObjType.Pet_Chicken);
            Functions_GameObject.SetType(objList[38], ObjType.Unknown);
            Functions_GameObject.SetType(objList[39], ObjType.Unknown);

            //row 11
            Functions_GameObject.SetType(objList[40], ObjType.Vendor_NPC_Armor);
            Functions_GameObject.SetType(objList[41], ObjType.Vendor_NPC_Equipment);
            Functions_GameObject.SetType(objList[42], ObjType.Vendor_NPC_Items);
            Functions_GameObject.SetType(objList[43], ObjType.Vendor_NPC_Magic);

            //row 12
            Functions_GameObject.SetType(objList[44], ObjType.Vendor_NPC_Pets);
            Functions_GameObject.SetType(objList[45], ObjType.Vendor_NPC_Potions);
            Functions_GameObject.SetType(objList[46], ObjType.Vendor_NPC_Story);
            Functions_GameObject.SetType(objList[47], ObjType.Vendor_NPC_Weapons);

            //position the objs relative to the window frame
            Functions_Widget.PositionObjs(this);

            //manually set the dungeon entrances collision rec
            objList[0].compCollision.rec.Width = 16 * 3;
            objList[0].compCollision.rec.Height = 16 * 4;

            //hide the dungeon objs covered by dungeon entrance
            Functions_Widget.HideObj(objList[1]);
            Functions_Widget.HideObj(objList[2]);

            Functions_Widget.HideObj(objList[4]);
            Functions_Widget.HideObj(objList[5]);
            Functions_Widget.HideObj(objList[6]);

            Functions_Widget.HideObj(objList[8]);
            Functions_Widget.HideObj(objList[9]);
            Functions_Widget.HideObj(objList[10]);

            Functions_Widget.HideObj(objList[12]);
            Functions_Widget.HideObj(objList[13]);
            Functions_Widget.HideObj(objList[14]);
        }
    }



}