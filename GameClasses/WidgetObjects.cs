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


    //only texture changes

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
            Functions_GameObject.SetType(objList[2], ObjType.Unknown);
            Functions_GameObject.SetType(objList[3], ObjType.Wor_Grass_2);

            //row 2
            Functions_GameObject.SetType(objList[4], ObjType.Wor_Bush);
            Functions_GameObject.SetType(objList[5], ObjType.Wor_Bush_Stump);
            Functions_GameObject.SetType(objList[6], ObjType.Wor_Pot);
            Functions_GameObject.SetType(objList[7], ObjType.Wor_Flowers);

            //row 3
            Functions_GameObject.SetType(objList[8], ObjType.Wor_Tree);
            Functions_GameObject.SetType(objList[9], ObjType.Wor_Tree_Burning);
            Functions_GameObject.SetType(objList[10], ObjType.Wor_Tree_Burnt);
            Functions_GameObject.SetType(objList[11], ObjType.Wor_Tree_Stump);

            //row 4
            Functions_GameObject.SetType(objList[12], ObjType.Unknown); //covered
            Functions_GameObject.SetType(objList[13], ObjType.Unknown); //covered
            Functions_GameObject.SetType(objList[14], ObjType.Unknown); //covered
            Functions_GameObject.SetType(objList[15], ObjType.Unknown); //covered

            //row 5
            Functions_GameObject.SetType(objList[16], ObjType.Wor_Water);
            Functions_GameObject.SetType(objList[17], ObjType.Unknown);
            Functions_GameObject.SetType(objList[18], ObjType.Wor_Coastline_Corner_Exterior);
            Functions_GameObject.SetType(objList[19], ObjType.Wor_Coastline_Corner_Interior);

            //row 6
            Functions_GameObject.SetType(objList[20], ObjType.Unknown);
            Functions_GameObject.SetType(objList[21], ObjType.Unknown);
            Functions_GameObject.SetType(objList[22], ObjType.Wor_Coastline_Straight);
            Functions_GameObject.SetType(objList[23], ObjType.Unknown);

            //row 7
            Functions_GameObject.SetType(objList[24], ObjType.Wor_Post_Vertical_Left);
            Functions_GameObject.SetType(objList[25], ObjType.Wor_Debris);
            Functions_GameObject.SetType(objList[26], ObjType.Wor_Post_Vertical_Right);
            Functions_GameObject.SetType(objList[27], ObjType.Unknown);

            //row 8
            Functions_GameObject.SetType(objList[28], ObjType.Wor_Post_Corner_Left);
            Functions_GameObject.SetType(objList[29], ObjType.Wor_Post_Horizontal);
            Functions_GameObject.SetType(objList[30], ObjType.Wor_Post_Corner_Right);
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
            Functions_GameObject.SetType(objList[44], ObjType.Wor_Enemy_Turtle);
            Functions_GameObject.SetType(objList[45], ObjType.Wor_Enemy_Crab);
            Functions_GameObject.SetType(objList[46], ObjType.Wor_Enemy_Rat);
            Functions_GameObject.SetType(objList[47], ObjType.Unknown);

            //position the objs relative to the window frame
            Functions_Widget.PositionObjs(this);

            //manually set tree recs
            objList[8].compCollision.rec.Height = 16 * 2;
            objList[9].compCollision.rec.Height = 16 * 2;

            //hide objs that trees overlap
            Functions_Widget.HideObj(objList[12]);
            Functions_Widget.HideObj(objList[13]);
            Functions_Widget.HideObj(objList[14]);
            Functions_Widget.HideObj(objList[15]);

            //water tile hides these
            Functions_Widget.HideObj(objList[17]);
            Functions_Widget.HideObj(objList[20]);
            Functions_Widget.HideObj(objList[21]);
        }
    }

    public class WidgetObjects_Dungeon : WidgetObject
    {
        public WidgetObjects_Dungeon()
        {   //create and set the position of the window frame
            window = new MenuWindow(
                new Point(16 * 6 + 8, 16 * 2), //position
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
            Functions_GameObject.SetType(objList[14], ObjType.Dungeon_SwitchDown);
            Functions_GameObject.SetType(objList[15], ObjType.Dungeon_Flamethrower);

            //row 5
            Functions_GameObject.SetType(objList[16], ObjType.Dungeon_Barrel);
            Functions_GameObject.SetType(objList[17], ObjType.Dungeon_Pot);
            Functions_GameObject.SetType(objList[18], ObjType.Dungeon_Chest); 
            Functions_GameObject.SetType(objList[19], ObjType.Dungeon_ChestEmpty);

            //row 6
            Functions_GameObject.SetType(objList[20], ObjType.Dungeon_TorchUnlit);
            Functions_GameObject.SetType(objList[21], ObjType.Dungeon_TorchLit);
            Functions_GameObject.SetType(objList[22], ObjType.Dungeon_Statue);
            Functions_GameObject.SetType(objList[23], ObjType.Unknown);

            //row 7
            Functions_GameObject.SetType(objList[24], ObjType.Dungeon_SpikesFloorOn);
            Functions_GameObject.SetType(objList[25], ObjType.Dungeon_SpikesFloorOff);
            Functions_GameObject.SetType(objList[26], ObjType.Dungeon_ConveyorBeltOn);
            Functions_GameObject.SetType(objList[27], ObjType.Dungeon_ConveyorBeltOff);

            //row 8
            Functions_GameObject.SetType(objList[28], ObjType.Dungeon_PitTrap);
            Functions_GameObject.SetType(objList[29], ObjType.Unknown);
            Functions_GameObject.SetType(objList[30], ObjType.Dungeon_Signpost);
            Functions_GameObject.SetType(objList[31], ObjType.Dungeon_LeverOff);

            //row 9
            Functions_GameObject.SetType(objList[32], ObjType.Dungeon_SkullPillar);
            Functions_GameObject.SetType(objList[33], ObjType.Unknown);
            Functions_GameObject.SetType(objList[34], ObjType.Unknown);
            Functions_GameObject.SetType(objList[35], ObjType.Unknown);

            //row 10
            Functions_GameObject.SetType(objList[36], ObjType.Unknown); //hidden by skull pillar
            Functions_GameObject.SetType(objList[37], ObjType.Unknown);
            Functions_GameObject.SetType(objList[38], ObjType.Unknown);
            Functions_GameObject.SetType(objList[39], ObjType.Unknown);

            //row 11
            Functions_GameObject.SetType(objList[40], ObjType.Unknown);
            Functions_GameObject.SetType(objList[41], ObjType.Unknown);
            Functions_GameObject.SetType(objList[42], ObjType.Unknown);
            Functions_GameObject.SetType(objList[43], ObjType.Unknown);

            //row 12
            Functions_GameObject.SetType(objList[44], ObjType.Dungeon_SpawnMob);
            Functions_GameObject.SetType(objList[45], ObjType.Unknown);
            Functions_GameObject.SetType(objList[46], ObjType.Unknown);
            Functions_GameObject.SetType(objList[47], ObjType.Dungeon_Map);

            //position the objs relative to the window frame
            Functions_Widget.PositionObjs(this);

            Functions_Widget.HideObj(objList[36]);
        }
    }







    //contain objects specific to texture sheets / levels

    public class WidgetObjects_Town : WidgetObject
    {
        public WidgetObjects_Town()
        {   //create and set the position of the window frame
            window = new MenuWindow(
                new Point(16 * 12, 16 * 2),
                new Point(16 * 5, 16 * 15), //size
                "Town Objects"); //title

            objList = new List<GameObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new GameObject()); }

            //row 1
            Functions_GameObject.SetType(objList[0], ObjType.Wor_Build_Wall_Side_Left);
            Functions_GameObject.SetType(objList[1], ObjType.Wor_Build_Wall_Back);
            Functions_GameObject.SetType(objList[2], ObjType.Wor_Build_Door_Open);
            Functions_GameObject.SetType(objList[3], ObjType.Wor_Build_Wall_Side_Right);

            //row 2
            Functions_GameObject.SetType(objList[4], ObjType.Wor_Build_Wall_FrontA);
            Functions_GameObject.SetType(objList[5], ObjType.Wor_Build_Wall_FrontB);
            Functions_GameObject.SetType(objList[6], ObjType.Wor_Build_Door_Shut);
            Functions_GameObject.SetType(objList[7], ObjType.Wor_Build_Roof_Top);

            //row 3
            Functions_GameObject.SetType(objList[8], ObjType.Unknown);
            Functions_GameObject.SetType(objList[9], ObjType.Unknown);
            Functions_GameObject.SetType(objList[10], ObjType.Wor_Build_Roof_Chimney);
            Functions_GameObject.SetType(objList[11], ObjType.Wor_Build_Roof_Bottom);

            //row 4
            Functions_GameObject.SetType(objList[12], ObjType.Wor_Fence_Vertical_Left);
            Functions_GameObject.SetType(objList[13], ObjType.Wor_Fence_Horizontal);
            Functions_GameObject.SetType(objList[14], ObjType.Wor_Fence_Gate);
            Functions_GameObject.SetType(objList[15], ObjType.Wor_Fence_Vertical_Right);

            //row 5
            Functions_GameObject.SetType(objList[16], ObjType.NPC_Story);
            Functions_GameObject.SetType(objList[17], ObjType.Unknown);
            Functions_GameObject.SetType(objList[18], ObjType.NPC_Farmer);
            Functions_GameObject.SetType(objList[19], ObjType.NPC_Farmer_Reward);

            //row 6
            Functions_GameObject.SetType(objList[20], ObjType.Vendor_NPC_Armor);
            Functions_GameObject.SetType(objList[21], ObjType.Vendor_NPC_Equipment);
            Functions_GameObject.SetType(objList[22], ObjType.Vendor_NPC_Items);
            Functions_GameObject.SetType(objList[23], ObjType.Vendor_NPC_Magic);

            //row 7
            Functions_GameObject.SetType(objList[24], ObjType.Vendor_NPC_Pets);
            Functions_GameObject.SetType(objList[25], ObjType.Vendor_NPC_Potions);
            Functions_GameObject.SetType(objList[26], ObjType.Vendor_NPC_Weapons);
            Functions_GameObject.SetType(objList[27], ObjType.Unknown);

            //row 8
            Functions_GameObject.SetType(objList[28], ObjType.Unknown);
            Functions_GameObject.SetType(objList[29], ObjType.Unknown);
            Functions_GameObject.SetType(objList[30], ObjType.Unknown);
            Functions_GameObject.SetType(objList[31], ObjType.Unknown);

            //row 9
            Functions_GameObject.SetType(objList[32], ObjType.Pet_Dog);
            Functions_GameObject.SetType(objList[33], ObjType.Pet_Chicken);
            Functions_GameObject.SetType(objList[34], ObjType.Unknown);
            Functions_GameObject.SetType(objList[35], ObjType.Unknown);

            //row 10
            Functions_GameObject.SetType(objList[36], ObjType.Wor_TableSingle);
            Functions_GameObject.SetType(objList[37], ObjType.Wor_TableDoubleLeft);
            Functions_GameObject.SetType(objList[38], ObjType.Wor_TableDoubleRight);
            Functions_GameObject.SetType(objList[39], ObjType.Wor_Bed);

            //row 11
            Functions_GameObject.SetType(objList[40], ObjType.Unknown);
            Functions_GameObject.SetType(objList[41], ObjType.Unknown);
            Functions_GameObject.SetType(objList[42], ObjType.Wor_Chair);
            Functions_GameObject.SetType(objList[43], ObjType.Unknown); //hidden by bed

            //row 12
            Functions_GameObject.SetType(objList[44], ObjType.Wor_Bookcase);
            Functions_GameObject.SetType(objList[45], ObjType.Wor_Shelf);
            Functions_GameObject.SetType(objList[46], ObjType.Wor_Stove);
            Functions_GameObject.SetType(objList[47], ObjType.Wor_Sink);

            //position the objs relative to the window frame
            Functions_Widget.PositionObjs(this);

            Functions_Widget.HideObj(objList[43]);
        }
    }

    public class WidgetObjects_Colliseum : WidgetObject
    {
        public WidgetObjects_Colliseum()
        {   //create and set the position of the window frame
            window = new MenuWindow(
                new Point(16 * 12, 16 * 2), //position
                new Point(16 * 5, 16 * 15), //size
                "Colliseum Objs"); //title

            objList = new List<GameObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new GameObject()); }

            //row 1
            Functions_GameObject.SetType(objList[0], ObjType.Wor_Entrance_Colliseum);
            Functions_GameObject.SetType(objList[1], ObjType.Unknown);//ent
            Functions_GameObject.SetType(objList[2], ObjType.Unknown);//ent
            Functions_GameObject.SetType(objList[3], ObjType.Wor_Colliseum_Pillar_Top);

            //row 2
            Functions_GameObject.SetType(objList[4], ObjType.Unknown);//ent
            Functions_GameObject.SetType(objList[5], ObjType.Unknown);//ent
            Functions_GameObject.SetType(objList[6], ObjType.Unknown);//ent
            Functions_GameObject.SetType(objList[7], ObjType.Wor_Colliseum_Pillar_Middle);

            //row 3
            Functions_GameObject.SetType(objList[8], ObjType.Unknown);//ent
            Functions_GameObject.SetType(objList[9], ObjType.Unknown);//ent
            Functions_GameObject.SetType(objList[10], ObjType.Unknown);//ent
            Functions_GameObject.SetType(objList[11], ObjType.Wor_Colliseum_Pillar_Bottom);

            //row 4
            Functions_GameObject.SetType(objList[12], ObjType.Unknown);//ent
            Functions_GameObject.SetType(objList[13], ObjType.Unknown);//ent
            Functions_GameObject.SetType(objList[14], ObjType.Unknown);//ent
            Functions_GameObject.SetType(objList[15], ObjType.Wor_Colliseum_Outdoors_Floor);

            //row 5
            Functions_GameObject.SetType(objList[16], ObjType.Wor_Colliseum_Bricks_Left);
            Functions_GameObject.SetType(objList[17], ObjType.Wor_Colliseum_Bricks_Middle1);
            Functions_GameObject.SetType(objList[18], ObjType.Wor_Colliseum_Bricks_Middle2);
            Functions_GameObject.SetType(objList[19], ObjType.Wor_Colliseum_Bricks_Right);

            //row 6
            Functions_GameObject.SetType(objList[20], ObjType.Unknown);//brix
            Functions_GameObject.SetType(objList[21], ObjType.Unknown);//brix
            Functions_GameObject.SetType(objList[22], ObjType.Unknown);//brix
            Functions_GameObject.SetType(objList[23], ObjType.Unknown);//brix




            //row 7
            Functions_GameObject.SetType(objList[24], ObjType.Wor_Colliseum_Spectator);
            Functions_GameObject.SetType(objList[25], ObjType.Unknown);
            Functions_GameObject.SetType(objList[26], ObjType.Unknown);
            Functions_GameObject.SetType(objList[27], ObjType.Unknown);

            //row 8
            Functions_GameObject.SetType(objList[28], ObjType.Vendor_Colliseum_Mob);
            Functions_GameObject.SetType(objList[29], ObjType.Vendor_NPC_EnemyItems);
            Functions_GameObject.SetType(objList[30], ObjType.Unknown);
            Functions_GameObject.SetType(objList[31], ObjType.Unknown);

            //row 9
            Functions_GameObject.SetType(objList[32], ObjType.Wor_Colliseum_Stairs_Handrail_Top);
            Functions_GameObject.SetType(objList[33], ObjType.Wor_Colliseum_Stairs_Left);
            Functions_GameObject.SetType(objList[34], ObjType.Unknown);
            Functions_GameObject.SetType(objList[35], ObjType.Unknown);

            //row 10
            Functions_GameObject.SetType(objList[36], ObjType.Wor_Colliseum_Stairs_Handrail_Middle);
            Functions_GameObject.SetType(objList[37], ObjType.Wor_Colliseum_Stairs_Middle);
            Functions_GameObject.SetType(objList[38], ObjType.Unknown);
            Functions_GameObject.SetType(objList[39], ObjType.Unknown);

            //row 11
            Functions_GameObject.SetType(objList[40], ObjType.Wor_Colliseum_Stairs_Handrail_Bottom);
            Functions_GameObject.SetType(objList[41], ObjType.Wor_Colliseum_Stairs_Right);
            Functions_GameObject.SetType(objList[42], ObjType.Unknown);
            Functions_GameObject.SetType(objList[43], ObjType.Unknown);

            //row 12 - cheat and put these beyond widget bounds cause they big
            Functions_GameObject.SetType(objList[44], ObjType.Wor_Colliseum_Gate_Pillar_Left);
            Functions_GameObject.SetType(objList[45], ObjType.Unknown);//covered by left pillar
            Functions_GameObject.SetType(objList[46], ObjType.Wor_Colliseum_Gate_Center);
            Functions_GameObject.SetType(objList[47], ObjType.Wor_Colliseum_Gate_Pillar_Right);//

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

            //hide objs covered by brix
            Functions_Widget.HideObj(objList[20]);
            Functions_Widget.HideObj(objList[21]);
            Functions_Widget.HideObj(objList[22]);
            Functions_Widget.HideObj(objList[23]);

            //hide objs covered by gates
            Functions_Widget.HideObj(objList[45]);




        }
    }






















    public class WidgetObjects_Boat : WidgetObject
    {
        public WidgetObjects_Boat()
        {   //create and set the position of the window frame
            window = new MenuWindow(
                new Point(16 * 12, 16 * 2), //position
                new Point(16 * 5, 16 * 15), //size
                "Boat Objs"); //title

            objList = new List<GameObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new GameObject()); }

            //row 1
            Functions_GameObject.SetType(objList[0], ObjType.Wor_Boat_Front_Left);
            Functions_GameObject.SetType(objList[1], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[2], ObjType.Wor_Boat_Front_Right);//
            Functions_GameObject.SetType(objList[3], ObjType.Unknown);//

            //row 2
            Functions_GameObject.SetType(objList[4], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[5], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[6], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[7], ObjType.Unknown);//

            //row 3
            Functions_GameObject.SetType(objList[8], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[9], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[10], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[11], ObjType.Unknown);//

            //row 4
            Functions_GameObject.SetType(objList[12], ObjType.Wor_Boat_Front_ConnectorLeft);
            Functions_GameObject.SetType(objList[13], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[14], ObjType.Wor_Boat_Front_ConnectorRight);
            Functions_GameObject.SetType(objList[15], ObjType.Unknown);//

            //row 5
            Functions_GameObject.SetType(objList[16], ObjType.Wor_Boat_Bannister_Left);
            Functions_GameObject.SetType(objList[17], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[18], ObjType.Wor_Boat_Bannister_Right);
            Functions_GameObject.SetType(objList[19], ObjType.Unknown);//

            //row 6
            Functions_GameObject.SetType(objList[20], ObjType.Wor_Boat_Stairs_Top_Left);
            Functions_GameObject.SetType(objList[21], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[22], ObjType.Wor_Boat_Stairs_Top_Right);
            Functions_GameObject.SetType(objList[23], ObjType.Unknown);//

            //row 7
            Functions_GameObject.SetType(objList[24], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[25], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[26], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[27], ObjType.Unknown);//

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

            //row 12 - cheat and put these beyond widget bounds cause they big
            Functions_GameObject.SetType(objList[44], ObjType.Wor_Boat_Front);
            Functions_GameObject.SetType(objList[45], ObjType.Unknown);
            Functions_GameObject.SetType(objList[46], ObjType.Unknown);
            Functions_GameObject.SetType(objList[47], ObjType.Wor_Boat_Front); //engine

            //position the objs relative to the window frame
            Functions_Widget.PositionObjs(this);



            //manually set collision recs for some objs
            objList[0].compCollision.rec.Width = 16 * 2;
            objList[0].compCollision.rec.Height = 16 * 3;

            objList[2].compCollision.rec.Width = 16 * 2;
            objList[2].compCollision.rec.Height = 16 * 3;

            objList[14].compCollision.rec.Width = 16 * 2;
            objList[14].compCollision.rec.Height = 16 * 3;

            //connectors
            objList[12].compCollision.rec.Width = 16 * 2;
            objList[14].compCollision.rec.Width = 16 * 2;
            //bannisters
            objList[16].compCollision.rec.Width = 16 * 2;
            objList[18].compCollision.rec.Width = 16 * 2;
            //stairs
            objList[20].compCollision.rec.Width = 16 * 2;
            objList[20].compCollision.rec.Height = 16 * 2;
            objList[22].compCollision.rec.Width = 16 * 2;
            objList[22].compCollision.rec.Height = 16 * 2;




            //hide the dungeon objs covered by other objs
            Functions_Widget.HideObj(objList[1]);
            Functions_Widget.HideObj(objList[3]);
            Functions_Widget.HideObj(objList[4]);
            Functions_Widget.HideObj(objList[5]);
            Functions_Widget.HideObj(objList[6]);
            Functions_Widget.HideObj(objList[7]);
            Functions_Widget.HideObj(objList[8]);
            Functions_Widget.HideObj(objList[9]);
            Functions_Widget.HideObj(objList[10]);
            Functions_Widget.HideObj(objList[11]);
            Functions_Widget.HideObj(objList[13]);
            Functions_Widget.HideObj(objList[15]);
            Functions_Widget.HideObj(objList[17]);
            Functions_Widget.HideObj(objList[19]);
            Functions_Widget.HideObj(objList[21]);
            Functions_Widget.HideObj(objList[23]);
            Functions_Widget.HideObj(objList[24]);
            Functions_Widget.HideObj(objList[25]);
            Functions_Widget.HideObj(objList[26]);
            Functions_Widget.HideObj(objList[27]);




            // - we could simply hide all unknown objects?
        }
    }






    public class WidgetObjects_Forest : WidgetObject
    {
        public WidgetObjects_Forest()
        {   //create and set the position of the window frame
            window = new MenuWindow(
                new Point(16 * 12, 16 * 2), //position
                new Point(16 * 5, 16 * 15), //size
                "Forest Objects"); //title

            objList = new List<GameObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new GameObject()); }

            //row 1

            //this is a 3x4 * 16 sized sprite/obj
            Functions_GameObject.SetType(objList[0], ObjType.Wor_Entrance_ForestDungeon);
            Functions_GameObject.SetType(objList[1], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[2], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[3], ObjType.Wor_SeekerExploder);

            //row 2
            Functions_GameObject.SetType(objList[4], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[5], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[6], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[7], ObjType.Wor_SeekerExploder);

            //row 3
            Functions_GameObject.SetType(objList[8], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[9], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[10], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[11], ObjType.Wor_SeekerExploder);

            //row 4
            Functions_GameObject.SetType(objList[12], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[13], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[14], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[15], ObjType.Wor_SeekerExploder);

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

    public class WidgetObjects_Mountain : WidgetObject
    {
        public WidgetObjects_Mountain()
        {   //create and set the position of the window frame
            window = new MenuWindow(
                new Point(16 * 12, 16 * 2), //position
                new Point(16 * 5, 16 * 15), //size
                "Mountain Objects"); //title

            objList = new List<GameObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new GameObject()); }

            //row 1

            //this is a 3x4 * 16 sized sprite/obj
            Functions_GameObject.SetType(objList[0], ObjType.Wor_Entrance_MountainDungeon);
            Functions_GameObject.SetType(objList[1], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[2], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[3], ObjType.Dungeon_Statue);

            //row 2
            Functions_GameObject.SetType(objList[4], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[5], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[6], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[7], ObjType.Wor_SeekerExploder);

            //row 3
            Functions_GameObject.SetType(objList[8], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[9], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[10], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[11], ObjType.Dungeon_Statue);

            //row 4
            Functions_GameObject.SetType(objList[12], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[13], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[14], ObjType.Unknown);//
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
            Functions_GameObject.SetType(objList[27], ObjType.Wor_MountainWall_Cave_Covered);

            //row 8
            Functions_GameObject.SetType(objList[28], ObjType.Unknown);
            Functions_GameObject.SetType(objList[29], ObjType.Wor_MountainWall_Cave_Bare);
            Functions_GameObject.SetType(objList[30], ObjType.Unknown); //cov
            Functions_GameObject.SetType(objList[31], ObjType.Unknown); //cov

            //row 9
            Functions_GameObject.SetType(objList[32], ObjType.Unknown);
            Functions_GameObject.SetType(objList[33], ObjType.Unknown); //cov
            Functions_GameObject.SetType(objList[34], ObjType.Unknown); //cov
            Functions_GameObject.SetType(objList[35], ObjType.Wor_MountainWall_Top); //wall top

            //row 10
            Functions_GameObject.SetType(objList[36], ObjType.Wor_MountainWall_Ladder_Trap);
            Functions_GameObject.SetType(objList[37], ObjType.Wor_MountainWall_Alcove_Right);
            Functions_GameObject.SetType(objList[38], ObjType.Unknown); //covered
            Functions_GameObject.SetType(objList[39], ObjType.Wor_MountainWall_Mid); //wall mid

            //row 11
            Functions_GameObject.SetType(objList[40], ObjType.Wor_MountainWall_Ladder);
            Functions_GameObject.SetType(objList[41], ObjType.Unknown); //cov
            Functions_GameObject.SetType(objList[42], ObjType.Unknown); //cov
            Functions_GameObject.SetType(objList[43], ObjType.Unknown); //covered

            //row 12
            Functions_GameObject.SetType(objList[44], ObjType.Wor_MountainWall_Foothold);
            Functions_GameObject.SetType(objList[45], ObjType.Wor_MountainWall_Alcove_Left);
            Functions_GameObject.SetType(objList[46], ObjType.Unknown); //covered
            Functions_GameObject.SetType(objList[47], ObjType.Wor_MountainWall_Bottom); //wall bottom

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

            Functions_Widget.HideObj(objList[30]);
            Functions_Widget.HideObj(objList[31]);

            Functions_Widget.HideObj(objList[33]);
            Functions_Widget.HideObj(objList[34]);

            Functions_Widget.HideObj(objList[38]);

            Functions_Widget.HideObj(objList[41]);
            Functions_Widget.HideObj(objList[42]);
            Functions_Widget.HideObj(objList[43]);

            Functions_Widget.HideObj(objList[46]);
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

            objList = new List<GameObject>();
            //4 per row, 12 rows total
            for (i = 0; i < 4 * 12; i++) { objList.Add(new GameObject()); }

            //row 1

            //this is a 3x4 * 16 sized sprite/obj
            Functions_GameObject.SetType(objList[0], ObjType.Wor_Entrance_SwampDungeon);
            Functions_GameObject.SetType(objList[1], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[2], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[3], ObjType.Dungeon_Statue);

            //row 2
            Functions_GameObject.SetType(objList[4], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[5], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[6], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[7], ObjType.Wor_SeekerExploder);

            //row 3
            Functions_GameObject.SetType(objList[8], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[9], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[10], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[11], ObjType.Dungeon_Statue);

            //row 4
            Functions_GameObject.SetType(objList[12], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[13], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[14], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[15], ObjType.Dungeon_Statue);

            //row 5
            Functions_GameObject.SetType(objList[16], ObjType.Unknown);
            Functions_GameObject.SetType(objList[17], ObjType.Wor_Swamp_BigPlant);
            Functions_GameObject.SetType(objList[18], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[19], ObjType.Wor_Swamp_LillyPad);

            //row 6
            Functions_GameObject.SetType(objList[20], ObjType.Unknown);
            Functions_GameObject.SetType(objList[21], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[22], ObjType.Unknown);//
            Functions_GameObject.SetType(objList[23], ObjType.Unknown);//

            //row 7
            Functions_GameObject.SetType(objList[24], ObjType.Unknown);
            Functions_GameObject.SetType(objList[25], ObjType.Unknown);
            Functions_GameObject.SetType(objList[26], ObjType.Unknown);
            Functions_GameObject.SetType(objList[27], ObjType.Wor_Swamp_Vine);

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

            Functions_Widget.HideObj(objList[18]);
            Functions_Widget.HideObj(objList[21]);
            Functions_Widget.HideObj(objList[22]);
            Functions_Widget.HideObj(objList[23]);


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
            Functions_GameObject.SetType(objList[44], ObjType.Unknown);
            Functions_GameObject.SetType(objList[45], ObjType.Unknown);
            Functions_GameObject.SetType(objList[46], ObjType.Unknown);
            Functions_GameObject.SetType(objList[47], ObjType.Unknown);

            //position the objs relative to the window frame
            Functions_Widget.PositionObjs(this);
            
        }
    }



}