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
    public static class AnimationFrames
    {
        //defines all animation frames for gameObjects, projectiles, particles, & pickups
        //and also floor animation frames
        //and in the future we'll define actor animation frames here too

        public static List<Byte4> Dungeon_FloorNormal = new List<Byte4> { new Byte4(15, 0, 0, 0) };
        public static List<Byte4> Dungeon_FloorSpecial = new List<Byte4> { new Byte4(15, 1, 0, 0) };
        public static List<Byte4> Dungeon_FloorBoss = new List<Byte4> { new Byte4(15, 2, 0, 0) };


        #region Dungeon Objects

        public static List<Byte4> Dungeon_ExitPillarLeft = new List<Byte4> { new Byte4(8, 0, 0, 0) };
        public static List<Byte4> Dungeon_ExitPillarRight = new List<Byte4> { new Byte4(8, 0, 1, 0) };
        public static List<Byte4> Dungeon_Exit = new List<Byte4> { new Byte4(9, 0, 0, 0) };
        public static List<Byte4> Dungeon_ExitLight = new List<Byte4> { new Byte4(10, 0, 0, 0) };

        public static List<Byte4> Dungeon_DoorOpen = new List<Byte4> { new Byte4(11, 0, 0, 0) };
        public static List<Byte4> Dungeon_DoorShut = new List<Byte4> { new Byte4(11, 1, 0, 0) };
        public static List<Byte4> Dungeon_DoorBoss = new List<Byte4> { new Byte4(11, 2, 0, 0) };

        public static List<Byte4> Dungeon_WallStraight = new List<Byte4> { new Byte4(12, 0, 0, 0) };
        public static List<Byte4> Dungeon_WallPillar = new List<Byte4> { new Byte4(12, 1, 0, 0) };
        public static List<Byte4> Dungeon_WallStraightCracked = new List<Byte4> { new Byte4(12, 2, 0, 0) };

        public static List<Byte4> Dungeon_WallInteriorCorner = new List<Byte4> { new Byte4(13, 0, 0, 0) };
        public static List<Byte4> Dungeon_WallStatue = new List<Byte4> { new Byte4(13, 1, 0, 0) };
        public static List<Byte4> Dungeon_WallExteriorCorner = new List<Byte4> { new Byte4(13, 2, 0, 0) };

        public static List<Byte4> Dungeon_WallTorch = new List<Byte4>
        { new Byte4(14, 0, 0, 0), new Byte4(14, 1, 0, 0), new Byte4(14, 2, 0, 0) };

        


        public static List<Byte4> Dungeon_Statue = new List<Byte4> { new Byte4(8, 3, 0, 0) };
        public static List<Byte4> Dungeon_SkullPillar = new List<Byte4> { new Byte4(10, 5, 0, 0) };

        public static List<Byte4> Dungeon_Fairy = new List<Byte4> { new Byte4(9, 3, 0, 0), new Byte4(10, 3, 0, 0) };
        public static List<Byte4> Dungeon_BossKey = new List<Byte4> { new Byte4(12, 3, 0, 0) };
        public static List<Byte4> Dungeon_Map = new List<Byte4> { new Byte4(11, 3, 0, 0) };
        //
        public static List<Byte4> Dungeon_IceTile = new List<Byte4> { new Byte4(14, 3, 0, 0) };
        public static List<Byte4> Dungeon_FloorDecal = new List<Byte4> { new Byte4(15, 3, 0, 0) };







        public static List<Byte4> Dungeon_BlockDark = new List<Byte4> { new Byte4(8, 4, 0, 0) };
        public static List<Byte4> Dungeon_BlockLight = new List<Byte4> { new Byte4(9, 4, 0, 0) };
        public static List<Byte4> Dungeon_BlockSpike = new List<Byte4> { new Byte4(10, 4, 0, 0) };
        public static List<Byte4> Dungeon_SwitchBlockBtn = new List<Byte4> { new Byte4(11, 4, 0, 0) };
        public static List<Byte4> Dungeon_SwitchBlockDown = new List<Byte4> { new Byte4(12, 4, 0, 0) };
        public static List<Byte4> Dungeon_SwitchBlockUp = new List<Byte4> { new Byte4(13, 4, 0, 0) };
        //
        public static List<Byte4> Dungeon_FloorBlood = new List<Byte4> { new Byte4(15, 4, 0, 0) };






        public static List<Byte4> Dungeon_Barrel = new List<Byte4> { new Byte4(8, 5, 0, 0) };
        public static List<Byte4> Dungeon_BarrelExploding = new List<Byte4> { new Byte4(8, 5, 0, 0), new Byte4(9, 5, 0, 0) };
        public static List<Byte4> Dungeon_Bumper = new List<Byte4> { new Byte4(10, 5, 0, 0) };
        public static List<Byte4> Dungeon_FloorSwitchUp = new List<Byte4> { new Byte4(11, 5, 0, 0) };
        public static List<Byte4> Dungeon_FloorSwitchDown = new List<Byte4> { new Byte4(12, 5, 0, 0) };
        public static List<Byte4> Dungeon_Flamethrower = new List<Byte4> { new Byte4(13, 5, 0, 0) };
        //
        public static List<Byte4> Dungeon_FloorCracked = new List<Byte4> { new Byte4(15, 5, 0, 0) };






        public static List<Byte4> Dungeon_Pot = new List<Byte4> { new Byte4(8, 6, 0, 0) };
        public static List<Byte4> Dungeon_ChestClosed = new List<Byte4> { new Byte4(9, 6, 0, 0) };
        public static List<Byte4> Dungeon_ChestOpened = new List<Byte4> { new Byte4(10, 6, 0, 0) };
        public static List<Byte4> Dungeon_Signpost = new List<Byte4> { new Byte4(6, 6, 0, 0) };


        public static List<Byte4> Dungeon_Pit = new List<Byte4> { new Byte4(8, 7, 0, 0) };
        public static List<Byte4> Dungeon_PitBridge = new List<Byte4> { new Byte4(9, 7, 0, 0) };
        public static List<Byte4> Dungeon_PitTeethTop = new List<Byte4> { new Byte4(11, 7, 0, 0) }; //16x8
        public static List<Byte4> Dungeon_PitTeethBottom = new List<Byte4> { new Byte4(12, 7, 0, 0) }; //16x8
        //







        public static List<Byte4> Dungeon_TorchUnlit = new List<Byte4> { new Byte4(8, 8, 0, 0) };
        public static List<Byte4> Dungeon_TorchLit = new List<Byte4>
        {
            new Byte4(9, 8, 0, 0),
            new Byte4(10, 8, 0, 0),
            new Byte4(11, 8, 0, 0)
        };
        //




        public static List<Byte4> Dungeon_LeverOn = new List<Byte4> { new Byte4(8, 9, 0, 0) };
        public static List<Byte4> Dungeon_LeverOff = new List<Byte4> { new Byte4(8, 9, 1, 0) };

        public static List<Byte4> Dungeon_FloorSpikesOn = new List<Byte4> { new Byte4(9, 9, 0, 0), new Byte4(10, 9, 0, 0) };
        public static List<Byte4> Dungeon_FloorSpikesOff = new List<Byte4> { new Byte4(10, 9, 0, 0) };

        public static List<Byte4> Dungeon_ConveyorBeltOn = new List<Byte4> { new Byte4(11, 9, 0, 0), new Byte4(12, 9, 0, 0) };
        public static List<Byte4> Dungeon_ConveyorBeltOff = new List<Byte4> { new Byte4(12, 9, 0, 0) };

        public static List<Byte4> Dungeon_Bed = new List<Byte4> { new Byte4(8, 6, 0, 0) };







        public static List<Byte4> Dungeon_SpawnMob = new List<Byte4> { new Byte4(13, 9, 0, 0) };
        public static List<Byte4> Dungeon_SpawnMiniBoss = new List<Byte4> { new Byte4(14, 9, 0, 0) };
        


        #endregion


        #region World Objects

        public static List<Byte4> World_Bush = new List<Byte4> { new Byte4(3, 5, 0, 0) };
        public static List<Byte4> World_BushStump = new List<Byte4> { new Byte4(4, 5, 0, 0) };

        public static List<Byte4> World_Tree = new List<Byte4> { new Byte4(0, 2, 0, 0) };
        public static List<Byte4> World_TreeStump = new List<Byte4> { new Byte4(1, 2, 0, 0) };
        public static List<Byte4> World_TreeBurnt = new List<Byte4> { new Byte4(2, 2, 0, 0) };

        //foilage
        public static List<Byte4> World_Grass_Tall = new List<Byte4> { new Byte4(3, 0, 0, 0) };
        public static List<Byte4> World_Grass_Short = new List<Byte4> { new Byte4(3, 1, 0, 0) };
        public static List<Byte4> World_Grass_Minimum = new List<Byte4> { new Byte4(4, 1, 0, 0) };

        public static List<Byte4> World_Flowers = new List<Byte4>
        { new Byte4(4, 4, 0, 0), new Byte4(5, 4, 0, 0), new Byte4(6, 4, 0, 0), new Byte4(7, 4, 0, 0) };

        //debris
        public static List<Byte4> World_Debris1 = new List<Byte4> { new Byte4(6, 5, 0, 0) };
        public static List<Byte4> World_Debris2 = new List<Byte4> { new Byte4(7, 5, 0, 0) };

        //interior building objs
        public static List<Byte4> World_Bookcase = new List<Byte4> { new Byte4(0, 15, 0, 0) };
        public static List<Byte4> World_Shelf = new List<Byte4> { new Byte4(1, 15, 0, 0) };
        public static List<Byte4> World_TableSingle = new List<Byte4> { new Byte4(2, 15, 0, 0) };
        public static List<Byte4> World_TableDoubleLeft = new List<Byte4> { new Byte4(3, 15, 0, 0) };
        public static List<Byte4> World_TableDoubleRight = new List<Byte4> { new Byte4(3, 15, 1, 0) };

        public static List<Byte4> Wor_Stove = new List<Byte4> { new Byte4(5, 15, 0, 0) };
        public static List<Byte4> Wor_Sink = new List<Byte4> { new Byte4(4, 15, 0, 0) };
        public static List<Byte4> Wor_Chair = new List<Byte4> { new Byte4(0, 14, 0, 0) };

        public static List<Byte4> Wor_Bed = new List<Byte4> { new Byte4(8, 5, 0, 0) };
        public static List<Byte4> Wor_BedDirty = new List<Byte4> { new Byte4(9, 5, 0, 0) };


        

        //objects
        public static List<Byte4> Wor_Pot = new List<Byte4> { new Byte4(7, 6, 0, 0) };

        //water tiles
        public static List<Byte4> Wor_Water = new List<Byte4> { new Byte4(3, 1, 0, 0) };
        public static List<Byte4> Wor_Coastline_Straight_A = new List<Byte4> { new Byte4(4, 2, 0, 0) };
        public static List<Byte4> Wor_Coastline_Straight_B = new List<Byte4> { new Byte4(4, 3, 0, 0) };
        public static List<Byte4> Wor_Coastline_Corner_Exterior = new List<Byte4> { new Byte4(5, 2, 0, 0) };
        public static List<Byte4> Wor_Coastline_Corner_Interior = new List<Byte4> { new Byte4(5, 3, 0, 0) };

        //ditches
        public static List<Byte4> Wor_Ditch_Empty_Single = new List<Byte4> { new Byte4(0, 8, 0, 0) };
        public static List<Byte4> Wor_Ditch_Empty_4UP = new List<Byte4> { new Byte4(0, 9, 0, 0) };
        public static List<Byte4> Wor_Ditch_Empty_Vertical = new List<Byte4> { new Byte4(1, 8, 0, 0) };
        public static List<Byte4> Wor_Ditch_Empty_Horizontal = new List<Byte4> { new Byte4(1, 9, 0, 0) };

        public static List<Byte4> Wor_Ditch_Empty_Corner_North = new List<Byte4> { new Byte4(2, 8, 0, 0) };
        public static List<Byte4> Wor_Ditch_Empty_Corner_South = new List<Byte4> { new Byte4(2, 9, 0, 0) };
        public static List<Byte4> Wor_Ditch_Empty_3UP_North = new List<Byte4> { new Byte4(3, 8, 0, 0) };
        public static List<Byte4> Wor_Ditch_Empty_3UP_South = new List<Byte4> { new Byte4(3, 9, 0, 0) };

        public static List<Byte4> Wor_Ditch_Empty_3UP_Horizontal = new List<Byte4> { new Byte4(4, 8, 0, 0) };
        public static List<Byte4> Wor_Ditch_Empty_Endcap_South = new List<Byte4> { new Byte4(4, 9, 0, 0) };
        public static List<Byte4> Wor_Ditch_Empty_Endcap_Horizontal = new List<Byte4> { new Byte4(5, 8, 0, 0) };
        public static List<Byte4> Wor_Ditch_Empty_Endcap_North = new List<Byte4> { new Byte4(5, 9, 0, 0) };

        //filled
        public static List<Byte4> Wor_Ditch_Filled_Single = new List<Byte4> { new Byte4(0, 10, 0, 0) };
        public static List<Byte4> Wor_Ditch_Filled_4UP = new List<Byte4> { new Byte4(0, 11, 0, 0) };
        public static List<Byte4> Wor_Ditch_Filled_Vertical = new List<Byte4> { new Byte4(1, 10, 0, 0) };
        public static List<Byte4> Wor_Ditch_Filled_Horizontal = new List<Byte4> { new Byte4(1, 11, 0, 0) };

        public static List<Byte4> Wor_Ditch_Filled_Corner_North = new List<Byte4> { new Byte4(2, 10, 0, 0) };
        public static List<Byte4> Wor_Ditch_Filled_Corner_South = new List<Byte4> { new Byte4(2, 11, 0, 0) };
        public static List<Byte4> Wor_Ditch_Filled_3UP_North = new List<Byte4> { new Byte4(3, 10, 0, 0) };
        public static List<Byte4> Wor_Ditch_Filled_3UP_South = new List<Byte4> { new Byte4(3, 11, 0, 0) };

        public static List<Byte4> Wor_Ditch_Filled_3UP_Horizontal = new List<Byte4> { new Byte4(4, 10, 0, 0) };
        public static List<Byte4> Wor_Ditch_Filled_Endcap_South = new List<Byte4> { new Byte4(4, 11, 0, 0) };
        public static List<Byte4> Wor_Ditch_Filled_Endcap_Horizontal = new List<Byte4> { new Byte4(5, 10, 0, 0) };
        public static List<Byte4> Wor_Ditch_Filled_Endcap_North = new List<Byte4> { new Byte4(5, 11, 0, 0) };





        //fence and gate
        public static List<Byte4> Wor_Fence_Horizontal = new List<Byte4> { new Byte4(0, 13, 0, 0) }; 
        public static List<Byte4> Wor_Fence_Vertical_Left = new List<Byte4> { new Byte4(0, 12, 0, 0) };
        public static List<Byte4> Wor_Fence_Vertical_Right = new List<Byte4> { new Byte4(0, 12, 1, 0) };
        public static List<Byte4> Wor_Fence_Gate = new List<Byte4> { new Byte4(1, 13, 0, 0) };

        //building objs
        public static List<Byte4> Wor_Build_Wall_FrontA = new List<Byte4> { new Byte4(8, 14, 0, 0) };
        public static List<Byte4> Wor_Build_Wall_FrontB = new List<Byte4> { new Byte4(9, 14, 0, 0) };
        public static List<Byte4> Wor_Build_Wall_Back = new List<Byte4> { new Byte4(8, 12, 0, 0) };

        public static List<Byte4> Wor_Build_Wall_Side_Left = new List<Byte4> { new Byte4(8, 13, 0, 0) };
        public static List<Byte4> Wor_Build_Wall_Side_Right = new List<Byte4> { new Byte4(8, 13, 1, 0) };

        public static List<Byte4> Wor_Build_Door_Shut = new List<Byte4> { new Byte4(8, 15, 0, 0) };
        public static List<Byte4> Wor_Build_Door_Open = new List<Byte4> { new Byte4(9, 15, 0, 0) };

        //roofs
        public static List<Byte4> Wor_Build_Roof_Top = new List<Byte4> { new Byte4(9, 12, 0, 0) };
        public static List<Byte4> Wor_Build_Roof_Bottom = new List<Byte4> { new Byte4(9, 13, 0, 0) };
        public static List<Byte4> Wor_Build_Roof_Chimney = new List<Byte4> { new Byte4(10, 12, 0, 0) };


        //posts
        public static List<Byte4> Wor_Post_Vertical_Left = new List<Byte4> { new Byte4(5, 6, 0, 0) };
        public static List<Byte4> Wor_Post_Corner_Left = new List<Byte4> { new Byte4(5, 7, 0, 0) };
        public static List<Byte4> Wor_Post_Vertical_Right = new List<Byte4> { new Byte4(5, 6, 1, 0) };
        public static List<Byte4> Wor_Post_Corner_Right = new List<Byte4> { new Byte4(5, 7, 1, 0) };

        public static List<Byte4> Wor_Post_Horizontal = new List<Byte4> { new Byte4(6, 7, 0, 0) };


        #endregion




        //Generic Dungeon/Level Entrance Object, 3x4 entrance object at 0,0
        public static List<Byte4> Wor_Entrance_Dungeon = new List<Byte4> { new Byte4(0, 0, 0, 0) };





        #region Colliseum Objects

        //

        public static List<Byte4> Wor_Colliseum_Gate_Center = new List<Byte4> { new Byte4(2, 3, 0, 0) };
        public static List<Byte4> Wor_Colliseum_Gate_Pillar_Left = new List<Byte4> { new Byte4(0, 3, 0, 0) };
        public static List<Byte4> Wor_Colliseum_Gate_Pillar_Right = new List<Byte4> { new Byte4(0, 3, 1, 0) };

        public static List<Byte4> Wor_Colliseum_Pillar_Top = new List<Byte4> { new Byte4(3, 12, 0, 0) };
        public static List<Byte4> Wor_Colliseum_Pillar_Middle = new List<Byte4> { new Byte4(3, 13, 0, 0) };
        public static List<Byte4> Wor_Colliseum_Pillar_Bottom = new List<Byte4> { new Byte4(3, 14, 0, 0) };

        public static List<Byte4> Wor_Colliseum_Stairs_Handrail_Top = new List<Byte4> { new Byte4(4, 12, 0, 0) };
        public static List<Byte4> Wor_Colliseum_Stairs_Handrail_Middle = new List<Byte4> { new Byte4(4, 13, 0, 0) };
        public static List<Byte4> Wor_Colliseum_Stairs_Handrail_Bottom = new List<Byte4> { new Byte4(4, 14, 0, 0) };

        public static List<Byte4> Wor_Colliseum_Stairs_Left = new List<Byte4> { new Byte4(5, 12, 0, 0) };
        public static List<Byte4> Wor_Colliseum_Stairs_Middle = new List<Byte4> { new Byte4(5, 13, 0, 0) };
        public static List<Byte4> Wor_Colliseum_Stairs_Right = new List<Byte4> { new Byte4(5, 12, 1, 0) };

        public static List<Byte4> Wor_Colliseum_Bricks_Left = new List<Byte4> { new Byte4(6, 6, 0, 0) };
        public static List<Byte4> Wor_Colliseum_Bricks_Middle1 = new List<Byte4> { new Byte4(7, 6, 0, 0) };
        public static List<Byte4> Wor_Colliseum_Bricks_Middle2 = new List<Byte4> { new Byte4(8, 6, 0, 0) };
        public static List<Byte4> Wor_Colliseum_Bricks_Right = new List<Byte4> { new Byte4(9, 6, 0, 0) };

        public static List<Byte4> Wor_Colliseum_Outdoors_Floor = new List<Byte4> { new Byte4(3, 15, 0, 0) };

        public static List<Byte4> Wor_Colliseum_Spectator = new List<Byte4>
        { new Byte4(4, 12, 0, 0), new Byte4(4, 13, 0, 0) };

        #endregion


        #region Forest Objects

        //

        #endregion


        #region Mountain Objects

        //

        public static List<Byte4> Wor_MountainWall_Mid = new List<Byte4> { new Byte4(0, 6, 0, 0) };
        public static List<Byte4> Wor_MountainWall_Bottom = new List<Byte4> { new Byte4(0, 14, 0, 0) };
        public static List<Byte4> Wor_MountainWall_Top = new List<Byte4> { new Byte4(0, 15, 0, 0) };

        public static List<Byte4> Wor_MountainWall_Foothold = new List<Byte4> { new Byte4(15, 12, 0, 0) };
        public static List<Byte4> Wor_MountainWall_Ladder = new List<Byte4> { new Byte4(15, 13, 0, 0) };
        public static List<Byte4> Wor_MountainWall_Ladder_Trap = new List<Byte4> { new Byte4(15, 14, 0, 0) };

        public static List<Byte4> Wor_MountainWall_Alcove_Left = new List<Byte4> { new Byte4(2, 6, 0, 0) };
        public static List<Byte4> Wor_MountainWall_Alcove_Right = new List<Byte4> { new Byte4(2, 6, 1, 0) };

        public static List<Byte4> Wor_MountainWall_Cave_Bare = new List<Byte4> { new Byte4(3, 6, 0, 0) };
        public static List<Byte4> Wor_MountainWall_Cave_Covered = new List<Byte4> { new Byte4(3, 7, 0, 0) };

        #endregion


        #region Swamp Objects

        public static List<Byte4> Wor_Swamp_LillyPad = new List<Byte4> { new Byte4(3, 6, 0, 0) };
        public static List<Byte4> Wor_Swamp_BigPlant = new List<Byte4> { new Byte4(2, 6, 0, 0) };
        public static List<Byte4> Wor_Swamp_Vine = new List<Byte4> { new Byte4(2, 14, 0, 0) };

        #endregion


        #region Boat Objects

        public static List<Byte4> Wor_Boat_Front_Left = new List<Byte4> { new Byte4(0, 0, 0, 0) };
        public static List<Byte4> Wor_Boat_Front = new List<Byte4> { new Byte4(1, 0, 0, 0) };
        public static List<Byte4> Wor_Boat_Front_Right = new List<Byte4> { new Byte4(2, 0, 0, 0) };

        public static List<Byte4> Wor_Boat_Front_ConnectorLeft = new List<Byte4> { new Byte4(0, 3, 0, 0) };
        public static List<Byte4> Wor_Boat_Front_ConnectorRight = new List<Byte4> { new Byte4(2, 3, 0, 0) };

        public static List<Byte4> Wor_Boat_Bannister_Left = new List<Byte4> { new Byte4(0, 5, 0, 0) };
        public static List<Byte4> Wor_Boat_Bannister_Right = new List<Byte4> { new Byte4(2, 5, 0, 0) };

        #endregion


        #region World Enemies



        //global enemies (can appear on any level)

        public static List<Byte4> Wor_Enemy_Crab = new List<Byte4>
        { new Byte4(16, 10, 0, 0), new Byte4(17, 10, 0, 0) };

        public static List<Byte4> Wor_Enemy_Turtle = new List<Byte4>
        { new Byte4(16, 11, 0, 0), new Byte4(17, 11, 0, 0) };

        //rat enemy has directional animFrames
        public static List<Byte4> Wor_Enemy_Rat_Up = new List<Byte4>
        { new Byte4(19, 9, 0, 0), new Byte4(19, 9, 1, 0) };
        public static List<Byte4> Wor_Enemy_Rat_Right = new List<Byte4>
        { new Byte4(17, 9, 0, 0), new Byte4(18, 9, 0, 0) };
        public static List<Byte4> Wor_Enemy_Rat_Down = new List<Byte4>
        { new Byte4(16, 9, 0, 0), new Byte4(16, 9, 1, 0) };
        public static List<Byte4> Wor_Enemy_Rat_Left = new List<Byte4>
        { new Byte4(17, 9, 1, 0), new Byte4(18, 9, 1, 0) };




        //unique enemies (only appears on specific level / colliseum)
        public static List<Byte4> Wor_SeekerExploder_Idle = new List<Byte4>
        { new Byte4(16, 3, 0, 0), new Byte4(17, 3, 0, 0) };
        public static List<Byte4> Wor_SeekerExploder_Chase = new List<Byte4>
        { new Byte4(18, 3, 0, 0), new Byte4(19, 3, 0, 0) };


        #endregion







        #region Pickups

        public static List<Byte4> Pickup_Rupee = new List<Byte4>
        {
            new Byte4(14*2, 0, 0, 0), new Byte4(14*2+1, 0, 0, 0),
            new Byte4(15*2, 0, 0, 0), new Byte4(15*2+1, 0, 0, 0)
        };
        public static List<Byte4> Pickup_Heart = new List<Byte4>
        {
            new Byte4(14*2, 1, 0, 0), new Byte4(14*2+1, 1, 0, 0),
            new Byte4(15*2, 1, 0, 0), new Byte4(15*2+1, 1, 0, 0)
        };
        public static List<Byte4> Pickup_Magic = new List<Byte4>
        {
            new Byte4(14*2, 2, 0, 0), new Byte4(14*2+1, 2, 0, 0),
            new Byte4(15*2, 2, 0, 0), new Byte4(15*2+1, 2, 0, 0)
        };
        public static List<Byte4> Pickup_Arrow = new List<Byte4>
        {
            new Byte4(14*2, 3, 0, 0), new Byte4(14*2+1, 3, 0, 0),
            new Byte4(15*2, 3, 0, 0), new Byte4(15*2+1, 3, 0, 0)
        };
        public static List<Byte4> Pickup_Bomb = new List<Byte4>
        {
            new Byte4(14*2, 4, 0, 0), new Byte4(14*2+1, 4, 0, 0),
            new Byte4(15*2, 4, 0, 0), new Byte4(15*2+1, 4, 0, 0)
        };

        #endregion


        #region Projectiles

        public static List<Byte4> Projectile_Sword = new List<Byte4>
        {
            new Byte4(0, 7, 0, 0), new Byte4(1, 7, 0, 0),
            new Byte4(2, 7, 0, 0), new Byte4(3, 7, 0, 0)
        };
        public static List<Byte4> Projectile_Net = new List<Byte4>
        {
            new Byte4(0, 8, 0, 0), new Byte4(1, 8, 0, 0),
            new Byte4(2, 8, 0, 0), new Byte4(3, 8, 0, 0)
        };
        public static List<Byte4> Projectile_Shovel = new List<Byte4>
        {
            new Byte4(0, 9, 0, 0), new Byte4(1, 9, 0, 0),
            new Byte4(2, 9, 0, 0), new Byte4(3, 9, 0, 0)
        };

        public static List<Byte4> Projectile_Boomerang = new List<Byte4>
        {
            new Byte4(5, 10, 0, 0), new Byte4(6, 10, 0, 0),
            new Byte4(7, 10, 0, 0), new Byte4(8, 10, 0, 0),
            new Byte4(9, 10, 0, 0), new Byte4(10, 10, 0, 0),
            new Byte4(11, 10, 0, 0), new Byte4(12, 10, 0, 0)
        };

        public static List<Byte4> Projectile_Bow = new List<Byte4> { new Byte4(6, 7, 0, 0) };
        public static List<Byte4> Projectile_Arrow = new List<Byte4> { new Byte4(7, 7, 0, 0) };
        public static List<Byte4> Projectile_Bomb = new List<Byte4> { new Byte4(8, 7, 0, 0), new Byte4(9, 7, 0, 0) };
        public static List<Byte4> Projectile_Explosion = new List<Byte4>
        { new Byte4(10, 7, 0, 0), new Byte4(11, 7, 0, 0), new Byte4(12, 7, 0, 0), };

        public static List<Byte4> Projectile_Fireball = new List<Byte4> { new Byte4(5, 8, 0, 0) };

        public static List<Byte4> Projectile_Bolt = new List<Byte4>
        { new Byte4(6, 8, 0, 0),new Byte4(6, 8, 1, 0) };

        public static List<Byte4> Projectile_FireGround = new List<Byte4>
        {
            new Byte4(5, 2, 0, 0), new Byte4(6, 2, 0, 0),
            new Byte4(7, 2, 0, 0)
        };

        public static List<Byte4> Projectile_Bat = new List<Byte4>
        { new Byte4(31, 1, 0, 0), new Byte4(31, 2, 0, 0) };

        #endregion


        #region Particles

        //8x8 particles - small
        public static List<Byte4> Particle_RisingSmoke = new List<Byte4>
        {
            new Byte4(0, 0, 0, 0), new Byte4(1, 0, 0, 0),
            new Byte4(2, 0, 0, 0), new Byte4(3, 0, 0, 0)
        };
        public static List<Byte4> Particle_ImpactDust = new List<Byte4>
        {
            new Byte4(4, 0, 0, 0), new Byte4(5, 0, 0, 0),
            new Byte4(6, 0, 0, 0), new Byte4(7, 0, 0, 0)
        };
        public static List<Byte4> Particle_Sparkle = new List<Byte4>
        {
            new Byte4(8, 0, 0, 0), new Byte4(9, 0, 0, 0)
        };
        public static List<Byte4> Particle_Push = new List<Byte4>
        {
            new Byte4(2*2, 1, 0, 0), new Byte4(2*2+1, 1, 0, 0)
        };
        public static List<Byte4> Particle_WaterKick = new List<Byte4>
        {
            new Byte4(0, 3, 0, 0), new Byte4(0, 4, 0, 0),
            new Byte4(0, 5, 0, 0), new Byte4(0, 6, 0, 0)
        };


        public static List<Byte4> Particle_PitBubble = new List<Byte4>
        {   //these frames are 8x8
            new Byte4(10*2, 7*2, 0, 0),
            new Byte4(10*2+1, 7*2, 0, 0),
            new Byte4(10*2, 7*2+1, 0, 0),
            new Byte4(10*2+1, 7*2+1, 0, 0)
        };



        public static List<Byte4> Particle_Leaf = new List<Byte4>
        {
            new Byte4(10, 10, 0, 0), new Byte4(11, 10, 0, 0)
        };
        public static List<Byte4> Particle_Debris = new List<Byte4>
        {
            new Byte4(10, 11, 0, 0), new Byte4(11, 11, 0, 0)
        };





        //8x4 - tiny
        public static List<Byte4> Particle_Map_Flag = new List<Byte4>
        {
            new Byte4(16, 0, 0, 0), new Byte4(17, 0, 0, 0),
            new Byte4(18, 0, 0, 0), new Byte4(19, 0, 0, 0)
        };
        public static List<Byte4> Particle_Map_Wave = new List<Byte4>
        {
            new Byte4(16, 1, 0, 0), new Byte4(17, 1, 0, 0),
            new Byte4(18, 1, 0, 0), new Byte4(19, 1, 0, 0)
        };

        //8x8 - small
        public static List<Byte4> Particle_Map_Campfire = new List<Byte4>
        {
            new Byte4(16, 1, 0, 0), new Byte4(17, 1, 0, 0),
            new Byte4(18, 1, 0, 0), new Byte4(19, 1, 0, 0)
        };

        //16x8
        public static List<Byte4> Particle_ShadowMed = new List<Byte4> { new Byte4(10, 0, 0, 0) };
        public static List<Byte4> Particle_ShadowSmall = new List<Byte4> { new Byte4(10, 0, 0, 0) };
        public static List<Byte4> Particle_MapConnector = new List<Byte4> { new Byte4(11, 0, 0, 0) };
        public static List<Byte4> Particle_MapLevel = new List<Byte4> { new Byte4(11, 1, 0, 0) };

        //16x16 - normal
        public static List<Byte4> Particle_Attention = new List<Byte4>
        {
            new Byte4(2, 1, 0, 0), new Byte4(3, 1, 0, 0)
        };

        public static List<Byte4> Particle_ExclamationBubble = new List<Byte4> { new Byte4(4, 1, 0, 0) };

        public static List<Byte4> Particle_Hit = new List<Byte4>
        {
            new Byte4(5, 1, 0, 0), new Byte4(6, 1, 0, 0),
            new Byte4(7, 1, 0, 0)
        };

        public static List<Byte4> Particle_Splash = new List<Byte4>
        {
            new Byte4(0, 2, 0, 0), new Byte4(1, 2, 0, 0),
            new Byte4(2, 2, 0, 0), new Byte4(3, 2, 0, 0),
            new Byte4(4, 2, 0, 0)
        };

        public static List<Byte4> Particle_Blast = new List<Byte4>
        {
            new Byte4(0, 1, 0, 0), new Byte4(0, 1, 1, 0)
        };

        #endregion


        #region Bottles

        public static List<Byte4> Bottle_Empty = new List<Byte4> { new Byte4(1, 5, 0, 0) };
        public static List<Byte4> Bottle_Health = new List<Byte4> { new Byte4(1, 6, 0, 0) };
        public static List<Byte4> Bottle_Magic = new List<Byte4> { new Byte4(1, 7, 0, 0) };
        public static List<Byte4> Bottle_Combo = new List<Byte4> { new Byte4(1, 8, 0, 0) };
        public static List<Byte4> Bottle_Fairy = new List<Byte4> { new Byte4(1, 9, 0, 0) };
        public static List<Byte4> Bottle_Blob = new List<Byte4> { new Byte4(1, 10, 0, 0) };

        #endregion


        #region Vendors & NPCs

        public static List<Byte4> Vendor_Items = new List<Byte4> { new Byte4(16, 14, 0, 0), new Byte4(16, 14, 1, 0) };
        public static List<Byte4> Vendor_EnemyItems = new List<Byte4> { new Byte4(16, 14, 0, 0), new Byte4(16, 14, 1, 0) };

        public static List<Byte4> Vendor_Potions = new List<Byte4> { new Byte4(17, 14, 0, 0), new Byte4(17, 14, 1, 0) };
        public static List<Byte4> Vendor_Magic = new List<Byte4> { new Byte4(18, 14, 0, 0), new Byte4(18, 14, 1, 0) };
        public static List<Byte4> Vendor_Weapons = new List<Byte4> { new Byte4(19, 14, 0, 0), new Byte4(19, 14, 1, 0) };
        public static List<Byte4> Vendor_Armor = new List<Byte4> { new Byte4(20, 14, 0, 0), new Byte4(20, 14, 1, 0) };
        public static List<Byte4> Vendor_Equipment = new List<Byte4> { new Byte4(21, 14, 0, 0), new Byte4(21, 14, 1, 0) };
        public static List<Byte4> Vendor_Pets = new List<Byte4> { new Byte4(22, 14, 0, 0), new Byte4(22, 14, 1, 0) };

        public static List<Byte4> Vendor_Story = new List<Byte4> { new Byte4(23, 14, 0, 0), new Byte4(23, 14, 1, 0) };

        public static List<Byte4> NPC_Farmer = new List<Byte4> { new Byte4(24, 14, 0, 0), new Byte4(24, 14, 1, 0) };
        public static List<Byte4> NPC_Farmer_HandsUp = new List<Byte4> { new Byte4(24, 14, 0, 0), new Byte4(25, 14, 0, 0) };

        public static List<Byte4> Vendor_Colliseum_Mob = new List<Byte4> { new Byte4(16, 15, 0, 0), new Byte4(16, 15, 1, 0) };

        #endregion


        #region Pets

        public static List<Byte4> Pet_Dog_Idle = new List<Byte4> { new Byte4(0, 1, 0, 0), new Byte4(1, 1, 0, 0) };
        public static List<Byte4> Pet_Dog_Move = new List<Byte4> { new Byte4(1, 1, 0, 0), new Byte4(2, 1, 0, 0) };
        public static List<Byte4> Pet_Dog_InWater = new List<Byte4> { new Byte4(3, 1, 0, 0), new Byte4(4, 1, 0, 0) };
        public static List<Byte4> Pet_Dog_Climbing = new List<Byte4> { new Byte4(5, 1, 0, 0) };

        public static List<Byte4> Pet_Chicken_Idle = new List<Byte4>
        { new Byte4(16, 12, 0, 0), new Byte4(17, 12, 0, 0) };
        public static List<Byte4> Pet_Chicken_Move = new List<Byte4>
        { new Byte4(17, 12, 0, 0), new Byte4(18, 12, 0, 0) };

        #endregion


        #region Ui Items

        public static List<Byte4> Ui_SelectionBox = new List<Byte4> { new Byte4(10, 1, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Unknown = new List<Byte4> { new Byte4(11, 1, 0, 0) };

        //editor ui
        public static List<Byte4> Ui_Add = new List<Byte4> { new Byte4(10, 4, 0, 0) };
        public static List<Byte4> Ui_Delete = new List<Byte4> { new Byte4(11, 4, 0, 0) };
        public static List<Byte4> Ui_Rotate = new List<Byte4> { new Byte4(12, 4, 0, 0) };

        public static List<Byte4> Ui_Hand_Open = new List<Byte4> { new Byte4(10, 2, 0, 0) };
        public static List<Byte4> Ui_Hand_Grab = new List<Byte4> { new Byte4(11, 2, 0, 0) };
        public static List<Byte4> Ui_Hand_Point = new List<Byte4> { new Byte4(10, 3, 0, 0) };
        public static List<Byte4> Ui_Hand_Press = new List<Byte4> { new Byte4(11, 3, 0, 0) };

        //input display
        public static List<Byte4> Input_Arrow_Selected = new List<Byte4> { new Byte4(7 * 2, 0, 0, 0) };
        public static List<Byte4> Input_Arrow_Unselected = new List<Byte4> { new Byte4(8 * 2 + 1, 0, 0, 0) };

        public static List<Byte4> Input_ButtonA_Selected = new List<Byte4> { new Byte4(6 * 2, 0, 0, 0) };
        public static List<Byte4> Input_ButtonA_Unselected = new List<Byte4> { new Byte4(7 * 2 + 1, 0, 0, 0) };

        public static List<Byte4> Input_ButtonB_Selected = new List<Byte4> { new Byte4(6 * 2 + 1, 0, 0, 0) };
        public static List<Byte4> Input_ButtonB_Unselected = new List<Byte4> { new Byte4(8 * 2, 0, 0, 0) };

        public static List<Byte4> Input_ButtonX_Selected = new List<Byte4> { new Byte4(6 * 2, 1, 0, 0) };
        public static List<Byte4> Input_ButtonX_Unselected = new List<Byte4> { new Byte4(7 * 2 + 1, 1, 0, 0) };

        public static List<Byte4> Input_ButtonY_Selected = new List<Byte4> { new Byte4(6 * 2 + 1, 1, 0, 0) };
        public static List<Byte4> Input_ButtonY_Unselected = new List<Byte4> { new Byte4(8 * 2, 1, 0, 0) };

        public static List<Byte4> Input_ButtonStart_Selected = new List<Byte4> { new Byte4(7 * 2, 1, 0, 0) };
        public static List<Byte4> Input_ButtonStart_Unselected = new List<Byte4> { new Byte4(8 * 2 + 1, 1, 0, 0) };




        //game ui
        public static List<Byte4> Ui_QuadBkg = new List<Byte4> { new Byte4(4, 1, 0, 0) };

        public static List<Byte4> Ui_MenuItem_InventoryGold = new List<Byte4>
        {
            new Byte4(10*2, 0, 0, 0), new Byte4(10*2+1, 0, 0, 0),
            new Byte4(11*2, 0, 0, 0), new Byte4(11*2+1, 0, 0, 0)
        };

        public static List<Byte4> Ui_MenuItem_ArrowToDisk = new List<Byte4> { new Byte4(14, 0, 0, 0) };
        public static List<Byte4> Ui_MenuItem_ArrowUp = new List<Byte4> { new Byte4(14, 1, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Monitor = new List<Byte4> { new Byte4(15, 1, 0, 0) };
        public static List<Byte4> Ui_MenuItem_QuestionMark = new List<Byte4> { new Byte4(14, 2, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Controller = new List<Byte4> { new Byte4(15, 2, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Joystick = new List<Byte4> { new Byte4(14, 3, 0, 0) };
        public static List<Byte4> Ui_MenuItem_SpeakerVolume = new List<Byte4> { new Byte4(15, 3, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Cross = new List<Byte4> { new Byte4(14, 4, 0, 0) };
        public static List<Byte4> Ui_MenuItem_ArrowRight = new List<Byte4> { new Byte4(15, 4, 0, 0) };

        //cheat ui
        public static List<Byte4> Ui_MenuItem_CheatOn = new List<Byte4> { new Byte4(14, 4, 0, 0) };
        public static List<Byte4> Ui_MenuItem_CheatOff = new List<Byte4> { new Byte4(11, 1, 0, 0) };



        //inventory
        public static List<Byte4> Ui_MenuItem_ItemHeart = new List<Byte4> { new Byte4(0, 5, 0, 0) };
        public static List<Byte4> Ui_MenuItem_ItemBomb = new List<Byte4> { new Byte4(0, 6, 0, 0) };
        public static List<Byte4> Ui_MenuItem_ItemBomb3Pack = new List<Byte4> { new Byte4(0, 7, 0, 0) };
        public static List<Byte4> Ui_MenuItem_ItemArrowPack = new List<Byte4> { new Byte4(0, 8, 0, 0) };
        public static List<Byte4> Ui_MenuItem_ItemBoomerang = new List<Byte4> { new Byte4(0, 9, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Item_Bow = new List<Byte4> { new Byte4(3, 6, 0, 0) };

        public static List<Byte4> Ui_MenuItem_Bottle_Empty = new List<Byte4> { new Byte4(1, 5, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Bottle_Health = new List<Byte4> { new Byte4(1, 6, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Bottle_Magic = new List<Byte4> { new Byte4(1, 7, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Bottle_Combo = new List<Byte4> { new Byte4(1, 8, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Bottle_Fairy = new List<Byte4> { new Byte4(1, 9, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Bottle_Blob = new List<Byte4> { new Byte4(1, 10, 0, 0) };

        public static List<Byte4> Ui_MenuItem_Magic_Fireball = new List<Byte4> { new Byte4(2, 5, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Magic_Bombos = new List<Byte4> { new Byte4(2, 6, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Magic_Bolt = new List<Byte4> { new Byte4(2, 7, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Magic_Bat = new List<Byte4> { new Byte4(2, 8, 0, 0) };


        public static List<Byte4> Ui_MenuItem_Weapon_Sword = new List<Byte4> { new Byte4(3, 5, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Weapon_Net = new List<Byte4> { new Byte4(3, 7, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Weapon_Shovel = new List<Byte4> { new Byte4(3, 8, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Weapon_Fang = new List<Byte4> { new Byte4(3, 9, 0, 0) };

        public static List<Byte4> Ui_MenuItem_Armor_Cloth = new List<Byte4> { new Byte4(4, 5, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Armor_Cape = new List<Byte4> { new Byte4(4, 6, 0, 0) };

        public static List<Byte4> Ui_MenuItem_Eq_Ring = new List<Byte4> { new Byte4(5, 5, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Eq_Pearl = new List<Byte4> { new Byte4(5, 6, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Eq_Necklace = new List<Byte4> { new Byte4(5, 7, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Eq_Glove = new List<Byte4> { new Byte4(5, 8, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Eq_Pin = new List<Byte4> { new Byte4(5, 9, 0, 0) };


        //challenges
        public static List<Byte4> Ui_MenuItem_Challenge = new List<Byte4> { new Byte4(15, 15, 0, 0) };

        #endregion




        //actor fx anim frames
        public static List<Byte4> ActorFX_GrassyFeet = new List<Byte4>
        { new Byte4(4, 1, 0, 0), new Byte4(4, 1, 1, 0) };
        public static List<Byte4> ActorFX_WetFeet = new List<Byte4> //16x8
        { new Byte4(3, 6, 0, 0), new Byte4(3, 7, 0, 0) };


        //actor animation frames
        public static ActorAnimationList Hero_Animations; //blob uses this too

        public static ActorAnimationList Standard_AngryEye_Animations;
        public static ActorAnimationList Standard_BeefyBat_Animations;

        public static ActorAnimationList MiniBoss_BlackEye_Animations;
        public static ActorAnimationList MiniBoss_SpiderArmored_Animations;
        public static ActorAnimationList MiniBoss_SpiderUnarmored_Animations;
        public static ActorAnimationList MiniBoss_OctoMouth_Animations;

        public static ActorAnimationList Boss_BigEye_Animations;
        public static ActorAnimationList Boss_BigBat_Animations;
        public static ActorAnimationList Boss_OctoHead_Animations;

        public static ActorAnimationList Special_Tentacle_Animations;




        


        static AnimationFrames()
        {

            //standard actors

            #region Hero Animations - Link and Blob

            Hero_Animations = new ActorAnimationList();

            //movement
            Hero_Animations.idle = new AnimationGroup();
            Hero_Animations.idle.down = new List<Byte4> { new Byte4(0, 0, 0, 0) };
            Hero_Animations.idle.up = new List<Byte4> { new Byte4(0, 1, 0, 0) };
            Hero_Animations.idle.right = new List<Byte4> { new Byte4(0, 2, 0, 0) };
            Hero_Animations.idle.left = new List<Byte4> { new Byte4(0, 2, 1, 0) };

            Hero_Animations.move = new AnimationGroup();
            Hero_Animations.move.down = new List<Byte4> { new Byte4(1, 0, 0, 0), new Byte4(1, 0, 1, 0) };
            Hero_Animations.move.up = new List<Byte4> { new Byte4(1, 1, 0, 0), new Byte4(1, 1, 1, 0) };
            Hero_Animations.move.right = new List<Byte4> { new Byte4(0, 2, 0, 0), new Byte4(1, 2, 0, 0) };
            Hero_Animations.move.left = new List<Byte4> { new Byte4(0, 2, 1, 0), new Byte4(1, 2, 1, 0) };

            Hero_Animations.idleCarry = new AnimationGroup();
            Hero_Animations.idleCarry.down = new List<Byte4> { new Byte4(5, 0, 0, 0) };
            Hero_Animations.idleCarry.up = new List<Byte4> { new Byte4(5, 1, 0, 0) };
            Hero_Animations.idleCarry.right = new List<Byte4> { new Byte4(5, 2, 0, 0) };
            Hero_Animations.idleCarry.left = new List<Byte4> { new Byte4(5, 2, 1, 0) };

            Hero_Animations.moveCarry = new AnimationGroup();
            Hero_Animations.moveCarry.down = new List<Byte4> { new Byte4(6, 0, 0, 0), new Byte4(6, 0, 1, 0) };
            Hero_Animations.moveCarry.up = new List<Byte4> { new Byte4(6, 1, 0, 0), new Byte4(6, 1, 1, 0) };
            Hero_Animations.moveCarry.right = new List<Byte4> { new Byte4(5, 2, 0, 0), new Byte4(6, 2, 0, 0) };
            Hero_Animations.moveCarry.left = new List<Byte4> { new Byte4(5, 2, 1, 0), new Byte4(6, 2, 1, 0) };

            //actions
            Hero_Animations.dash = new AnimationGroup();
            Hero_Animations.dash.down = new List<Byte4> { new Byte4(2, 0, 0, 0) };
            Hero_Animations.dash.up = new List<Byte4> { new Byte4(2, 1, 0, 0) };
            Hero_Animations.dash.right = new List<Byte4> { new Byte4(2, 2, 0, 0) };
            Hero_Animations.dash.left = new List<Byte4> { new Byte4(2, 2, 1, 0) };

            Hero_Animations.interact = new AnimationGroup();
            Hero_Animations.interact.down = new List<Byte4> { new Byte4(4, 0, 0, 0) };
            Hero_Animations.interact.up = new List<Byte4> { new Byte4(4, 1, 0, 0) };
            Hero_Animations.interact.right = new List<Byte4> { new Byte4(4, 2, 0, 0) };
            Hero_Animations.interact.left = new List<Byte4> { new Byte4(4, 2, 1, 0) };

            Hero_Animations.attack = new AnimationGroup();
            Hero_Animations.attack.down = new List<Byte4> { new Byte4(3, 0, 0, 0) };
            Hero_Animations.attack.up = new List<Byte4> { new Byte4(3, 1, 0, 0) };
            Hero_Animations.attack.right = new List<Byte4> { new Byte4(3, 2, 0, 0) };
            Hero_Animations.attack.left = new List<Byte4> { new Byte4(3, 2, 1, 0) };

            Hero_Animations.pickupThrow = new AnimationGroup();
            Hero_Animations.pickupThrow.down = new List<Byte4> { new Byte4(4, 0, 0, 0) };
            Hero_Animations.pickupThrow.up = new List<Byte4> { new Byte4(4, 1, 0, 0) };
            Hero_Animations.pickupThrow.right = new List<Byte4> { new Byte4(4, 2, 0, 0) };
            Hero_Animations.pickupThrow.left = new List<Byte4> { new Byte4(4, 2, 1, 0) };


            //reward list
            Hero_Animations.reward = new AnimationGroup();
            Hero_Animations.reward.down = new List<Byte4> { new Byte4(0, 3, 0, 0) };
            Hero_Animations.reward.up = Hero_Animations.reward.down;
            Hero_Animations.reward.right = Hero_Animations.reward.down;
            Hero_Animations.reward.left = Hero_Animations.reward.down;

            Hero_Animations.hit = new AnimationGroup();
            Hero_Animations.hit.down = new List<Byte4> { new Byte4(1, 3, 0, 0) };
            Hero_Animations.hit.up = Hero_Animations.hit.down;
            Hero_Animations.hit.right = Hero_Animations.hit.down;
            Hero_Animations.hit.left = Hero_Animations.hit.down;

            //heroic death on land
            Hero_Animations.death_heroic = new AnimationGroup();
            Hero_Animations.death_heroic.down = new List<Byte4>
            {
                //spin clockwise twice, then fall
                new Byte4(0, 0, 0, 0), new Byte4(0, 2, 1, 0), new Byte4(0, 1, 0, 0), new Byte4(0, 2, 0, 0),
                new Byte4(0, 0, 0, 0), new Byte4(0, 2, 1, 0), new Byte4(0, 1, 0, 0), new Byte4(0, 2, 0, 0),
                new Byte4(1, 3, 0, 0),
                new Byte4(2, 3, 0, 0) //fell, dead
            };
            Hero_Animations.death_heroic.up = Hero_Animations.death_heroic.down;
            Hero_Animations.death_heroic.right = Hero_Animations.death_heroic.down;
            Hero_Animations.death_heroic.left = Hero_Animations.death_heroic.down;


            //heroic death in water
            Hero_Animations.death_heroic_water = new AnimationGroup();
            Hero_Animations.death_heroic_water.down = new List<Byte4>
            {
                new Byte4(0, 5, 0, 0), new Byte4(0, 7, 0, 0), new Byte4(0, 6, 0, 0), new Byte4(0, 7, 1, 0), //spin
                new Byte4(0, 5, 0, 0), new Byte4(0, 7, 0, 0), new Byte4(0, 6, 0, 0), new Byte4(0, 7, 1, 0), //spin

                new Byte4(1, 8, 0, 0), new Byte4(1, 8, 0, 0), new Byte4(1, 8, 0, 0), //hold on swim hit frame
                new Byte4(4, 5, 1, 0), //final frame = still, underwater
            };
            Hero_Animations.death_heroic_water.up = Hero_Animations.death_heroic_water.down;
            Hero_Animations.death_heroic_water.right = Hero_Animations.death_heroic_water.down;
            Hero_Animations.death_heroic_water.left = Hero_Animations.death_heroic_water.down;

            //points to the blank frame reserved in the actor's sprite sheet, right of death / sitting
            Hero_Animations.death_blank = new AnimationGroup();
            Hero_Animations.death_blank.down = new List<Byte4> { new Byte4(3, 3, 0, 0) };
            Hero_Animations.death_blank.up = Hero_Animations.death_blank.down;
            Hero_Animations.death_blank.right = Hero_Animations.death_blank.down;
            Hero_Animations.death_blank.left = Hero_Animations.death_blank.down;

            //grab, push, pull
            Hero_Animations.grab = new AnimationGroup();
            Hero_Animations.grab.down = new List<Byte4> { new Byte4(7, 0, 0, 0) };
            Hero_Animations.grab.up = new List<Byte4> { new Byte4(7, 1, 0, 0) };
            Hero_Animations.grab.right = new List<Byte4> { new Byte4(7, 2, 0, 0) };
            Hero_Animations.grab.left = new List<Byte4> { new Byte4(7, 2, 1, 0) };

            Hero_Animations.push = new AnimationGroup();
            Hero_Animations.push.down = new List<Byte4> { new Byte4(7, 0, 0, 0), new Byte4(8, 0, 0, 0) };
            Hero_Animations.push.up = new List<Byte4> { new Byte4(7, 1, 0, 0), new Byte4(8, 1, 0, 0) };
            Hero_Animations.push.right = new List<Byte4> { new Byte4(7, 2, 0, 0), new Byte4(8, 2, 0, 0) };
            Hero_Animations.push.left = new List<Byte4> { new Byte4(7, 2, 1, 0), new Byte4(8, 2, 1, 0) };

            //swim idle and move
            Hero_Animations.swim_idle = new AnimationGroup();
            Hero_Animations.swim_idle.down = new List<Byte4> { new Byte4(0, 5, 0, 0), new Byte4(1, 5, 0, 0) };
            Hero_Animations.swim_idle.up = new List<Byte4> { new Byte4(0, 6, 0, 0), new Byte4(1, 6, 0, 0) };
            Hero_Animations.swim_idle.right = new List<Byte4> { new Byte4(0, 7, 0, 0), new Byte4(1, 7, 0, 0) };
            Hero_Animations.swim_idle.left = new List<Byte4> { new Byte4(0, 7, 1, 0), new Byte4(1, 7, 1, 0) };

            Hero_Animations.swim_move = new AnimationGroup();
            Hero_Animations.swim_move.down = new List<Byte4> { new Byte4(1, 5, 0, 0), new Byte4(1, 5, 1, 0) };
            Hero_Animations.swim_move.up = new List<Byte4> { new Byte4(1, 6, 0, 0), new Byte4(1, 6, 1, 0) };
            Hero_Animations.swim_move.right = new List<Byte4> { new Byte4(0, 7, 0, 0), new Byte4(1, 7, 0, 0) };
            Hero_Animations.swim_move.left = new List<Byte4> { new Byte4(0, 7, 1, 0), new Byte4(1, 7, 1, 0) };

            Hero_Animations.swim_dash = new AnimationGroup();
            Hero_Animations.swim_dash.down = new List<Byte4> { new Byte4(2, 5, 0, 0) };
            Hero_Animations.swim_dash.up = new List<Byte4> { new Byte4(2, 6, 0, 0) };
            Hero_Animations.swim_dash.right = new List<Byte4> { new Byte4(2, 7, 0, 0) };
            Hero_Animations.swim_dash.left = new List<Byte4> { new Byte4(2, 7, 1, 0) };

            Hero_Animations.swim_hit = new AnimationGroup();
            Hero_Animations.swim_hit.down = new List<Byte4> { new Byte4(1, 8, 0, 0) };
            Hero_Animations.swim_hit.up = Hero_Animations.swim_hit.down;
            Hero_Animations.swim_hit.right = Hero_Animations.swim_hit.down;
            Hero_Animations.swim_hit.left = Hero_Animations.swim_hit.down;

            Hero_Animations.swim_reward = new AnimationGroup();
            Hero_Animations.swim_reward.down = new List<Byte4> { new Byte4(0, 8, 0, 0) };
            Hero_Animations.swim_reward.up = Hero_Animations.swim_reward.down;
            Hero_Animations.swim_reward.right = Hero_Animations.swim_reward.down;
            Hero_Animations.swim_reward.left = Hero_Animations.swim_reward.down;

            //underwater
            Hero_Animations.underwater_idle = new AnimationGroup();
            Hero_Animations.underwater_idle.down = new List<Byte4> { new Byte4(4, 5, 0, 0), new Byte4(5, 5, 0, 0) };
            Hero_Animations.underwater_idle.up = new List<Byte4> { new Byte4(4, 6, 0, 0), new Byte4(5, 6, 0, 0) };
            Hero_Animations.underwater_idle.right = new List<Byte4> { new Byte4(4, 7, 0, 0), new Byte4(5, 7, 0, 0) };
            Hero_Animations.underwater_idle.left = new List<Byte4> { new Byte4(4, 7, 1, 0), new Byte4(5, 7, 1, 0) };

            Hero_Animations.underwater_move = new AnimationGroup();
            Hero_Animations.underwater_move.down = new List<Byte4>
            { new Byte4(3, 5, 0, 0), new Byte4(4, 5, 0, 0), new Byte4(5, 5, 0, 0) };
            Hero_Animations.underwater_move.up = new List<Byte4>
            { new Byte4(3, 6, 0, 0), new Byte4(4, 6, 0, 0), new Byte4(5, 6, 0, 0) };
            Hero_Animations.underwater_move.right = new List<Byte4>
            { new Byte4(3, 7, 0, 0), new Byte4(4, 7, 0, 0), new Byte4(5, 7, 0, 0) };
            Hero_Animations.underwater_move.left = new List<Byte4>
            { new Byte4(3, 7, 1, 0), new Byte4(4, 7, 1, 0), new Byte4(5, 7, 1, 0) };


            
            


            //falling
            Hero_Animations.falling = Hero_Animations.hit;

            //landed
            Hero_Animations.landed = new AnimationGroup();
            Hero_Animations.landed.down = new List<Byte4> { new Byte4(2, 3, 0, 0), new Byte4(4, 3, 0, 0) };
            Hero_Animations.landed.up = Hero_Animations.landed.down;
            Hero_Animations.landed.right = Hero_Animations.landed.down;
            Hero_Animations.landed.left = Hero_Animations.landed.down;

            

            //climbing move
            Hero_Animations.climbing = new AnimationGroup();
            Hero_Animations.climbing.down = new List<Byte4>
            {
                new Byte4(7, 1, 0, 0), new Byte4(8, 1, 0, 0),
                new Byte4(7, 1, 1, 0), new Byte4(8, 1, 1, 0),
            };
            Hero_Animations.climbing.up = Hero_Animations.climbing.down;
            Hero_Animations.climbing.right = Hero_Animations.climbing.down;
            Hero_Animations.climbing.left = Hero_Animations.climbing.down;





            #endregion


            #region AngryEye Animations

            Standard_AngryEye_Animations = new ActorAnimationList();

            //movement
            Standard_AngryEye_Animations.idle = new AnimationGroup();
            Standard_AngryEye_Animations.idle.down = new List<Byte4> { new Byte4(16, 3, 0, 0), new Byte4(16, 3, 1, 0) };
            Standard_AngryEye_Animations.idle.up = Standard_AngryEye_Animations.idle.down;
            Standard_AngryEye_Animations.idle.right = Standard_AngryEye_Animations.idle.down;
            Standard_AngryEye_Animations.idle.left = Standard_AngryEye_Animations.idle.down;

            Standard_AngryEye_Animations.move = Standard_AngryEye_Animations.idle;
            Standard_AngryEye_Animations.idleCarry = Standard_AngryEye_Animations.idle;
            Standard_AngryEye_Animations.moveCarry = Standard_AngryEye_Animations.idle;

            //actions
            Standard_AngryEye_Animations.dash = new AnimationGroup();
            Standard_AngryEye_Animations.dash.down = new List<Byte4> { new Byte4(18, 3, 0, 0) }; //menacing
            Standard_AngryEye_Animations.dash.up = Standard_AngryEye_Animations.dash.down;
            Standard_AngryEye_Animations.dash.right = Standard_AngryEye_Animations.dash.down;
            Standard_AngryEye_Animations.dash.left = Standard_AngryEye_Animations.dash.down;

            Standard_AngryEye_Animations.interact = Standard_AngryEye_Animations.dash;

            Standard_AngryEye_Animations.attack = new AnimationGroup();
            Standard_AngryEye_Animations.attack.down = new List<Byte4>
            { new Byte4(18, 3, 0, 0), new Byte4(19, 3, 0, 0) };
            Standard_AngryEye_Animations.attack.up = Standard_AngryEye_Animations.attack.down;
            Standard_AngryEye_Animations.attack.right = Standard_AngryEye_Animations.attack.down;
            Standard_AngryEye_Animations.attack.left = Standard_AngryEye_Animations.attack.down;

            Standard_AngryEye_Animations.hit = new AnimationGroup();
            Standard_AngryEye_Animations.hit.down = new List<Byte4> { new Byte4(17, 3, 0, 0) };
            Standard_AngryEye_Animations.hit.up = Standard_AngryEye_Animations.hit.down;
            Standard_AngryEye_Animations.hit.right = Standard_AngryEye_Animations.hit.down;
            Standard_AngryEye_Animations.hit.left = Standard_AngryEye_Animations.hit.down;
            Standard_AngryEye_Animations.reward = Standard_AngryEye_Animations.hit;
            Standard_AngryEye_Animations.pickupThrow = Standard_AngryEye_Animations.attack;

            Standard_AngryEye_Animations.death_heroic = Standard_AngryEye_Animations.hit;
            Standard_AngryEye_Animations.death_heroic_water = Standard_AngryEye_Animations.hit;
            Standard_AngryEye_Animations.death_blank = Standard_AngryEye_Animations.hit;
            //grab, push, pull
            Standard_AngryEye_Animations.grab = Standard_AngryEye_Animations.attack;
            Standard_AngryEye_Animations.push = Standard_AngryEye_Animations.attack;
            //swim idle and move
            Standard_AngryEye_Animations.swim_idle = Standard_AngryEye_Animations.idle;
            Standard_AngryEye_Animations.swim_move = Standard_AngryEye_Animations.idle;
            Standard_AngryEye_Animations.swim_dash = Standard_AngryEye_Animations.dash;
            Standard_AngryEye_Animations.swim_hit = Standard_AngryEye_Animations.hit;
            Standard_AngryEye_Animations.swim_reward = Standard_AngryEye_Animations.hit;
            //underwater
            Standard_AngryEye_Animations.underwater_idle = Standard_AngryEye_Animations.idle;
            Standard_AngryEye_Animations.underwater_move = Standard_AngryEye_Animations.idle;
            //falling & landed
            Standard_AngryEye_Animations.falling = Standard_AngryEye_Animations.idle;
            Standard_AngryEye_Animations.landed = Standard_AngryEye_Animations.idle;
            //climbing move
            Standard_AngryEye_Animations.climbing = Standard_AngryEye_Animations.attack;

            #endregion


            #region BeefyBat Animations

            Standard_BeefyBat_Animations = new ActorAnimationList();

            //movement
            Standard_BeefyBat_Animations.idle = new AnimationGroup();

            Standard_BeefyBat_Animations.idle.down = new List<Byte4>
            { new Byte4(16, 3, 0, 0), new Byte4(17, 3, 0, 0) };

            Standard_BeefyBat_Animations.idle.up = new List<Byte4>
            { new Byte4(16, 5, 0, 0), new Byte4(17, 5, 0, 0) };

            Standard_BeefyBat_Animations.idle.right = new List<Byte4>
            { new Byte4(16, 4, 0, 0), new Byte4(17, 4, 0, 0) };

            Standard_BeefyBat_Animations.idle.left = new List<Byte4>
            { new Byte4(16, 4, 1, 0), new Byte4(17, 4, 1, 0) };

            Standard_BeefyBat_Animations.move = Standard_BeefyBat_Animations.idle;
            Standard_BeefyBat_Animations.idleCarry = Standard_BeefyBat_Animations.idle;
            Standard_BeefyBat_Animations.moveCarry = Standard_BeefyBat_Animations.idle;
            Standard_BeefyBat_Animations.dash = Standard_BeefyBat_Animations.idle;
            Standard_BeefyBat_Animations.interact = Standard_BeefyBat_Animations.idle;
            Standard_BeefyBat_Animations.attack = Standard_BeefyBat_Animations.idle;
            Standard_BeefyBat_Animations.hit = Standard_BeefyBat_Animations.idle;
            Standard_BeefyBat_Animations.reward = Standard_BeefyBat_Animations.idle;
            Standard_BeefyBat_Animations.pickupThrow = Standard_BeefyBat_Animations.idle;
            Standard_BeefyBat_Animations.death_heroic = Standard_BeefyBat_Animations.idle;
            Standard_BeefyBat_Animations.death_heroic_water = Standard_BeefyBat_Animations.idle;
            Standard_BeefyBat_Animations.death_blank = Standard_BeefyBat_Animations.idle;
            Standard_BeefyBat_Animations.grab = Standard_BeefyBat_Animations.idle;
            Standard_BeefyBat_Animations.push = Standard_BeefyBat_Animations.idle;
            Standard_BeefyBat_Animations.swim_idle = Standard_BeefyBat_Animations.idle;
            Standard_BeefyBat_Animations.swim_move = Standard_BeefyBat_Animations.idle;
            Standard_BeefyBat_Animations.swim_dash = Standard_BeefyBat_Animations.idle;
            Standard_BeefyBat_Animations.swim_hit = Standard_BeefyBat_Animations.idle;
            Standard_BeefyBat_Animations.swim_reward = Standard_BeefyBat_Animations.idle;
            Standard_BeefyBat_Animations.underwater_idle = Standard_BeefyBat_Animations.idle;
            Standard_BeefyBat_Animations.underwater_move = Standard_BeefyBat_Animations.idle;
            Standard_BeefyBat_Animations.falling = Standard_BeefyBat_Animations.idle;
            Standard_BeefyBat_Animations.landed = Standard_BeefyBat_Animations.idle;
            Standard_BeefyBat_Animations.climbing = Standard_BeefyBat_Animations.idle;




            #endregion





            //miniboss actors

            #region Miniboss - Blackeye

            MiniBoss_BlackEye_Animations = new ActorAnimationList();

            //movement
            MiniBoss_BlackEye_Animations.idle = new AnimationGroup();
            MiniBoss_BlackEye_Animations.idle.down = new List<Byte4> { new Byte4(8, 2, 0, 0), new Byte4(9, 2, 0, 0) };
            MiniBoss_BlackEye_Animations.idle.up = MiniBoss_BlackEye_Animations.idle.down;
            MiniBoss_BlackEye_Animations.idle.right = MiniBoss_BlackEye_Animations.idle.down;
            MiniBoss_BlackEye_Animations.idle.left = MiniBoss_BlackEye_Animations.idle.down;

            MiniBoss_BlackEye_Animations.move = MiniBoss_BlackEye_Animations.idle;
            MiniBoss_BlackEye_Animations.idleCarry = MiniBoss_BlackEye_Animations.idle;
            MiniBoss_BlackEye_Animations.moveCarry = MiniBoss_BlackEye_Animations.idle;
            MiniBoss_BlackEye_Animations.interact = MiniBoss_BlackEye_Animations.idle;

            //dash is semi-shut eye
            MiniBoss_BlackEye_Animations.dash = new AnimationGroup();
            MiniBoss_BlackEye_Animations.dash.down = new List<Byte4> { new Byte4(10, 2, 0, 0) };
            MiniBoss_BlackEye_Animations.dash.up = MiniBoss_BlackEye_Animations.dash.down;
            MiniBoss_BlackEye_Animations.dash.right = MiniBoss_BlackEye_Animations.dash.down;
            MiniBoss_BlackEye_Animations.dash.left = MiniBoss_BlackEye_Animations.dash.down;

            //attack is semi-shut eye too
            MiniBoss_BlackEye_Animations.attack = MiniBoss_BlackEye_Animations.dash;

            MiniBoss_BlackEye_Animations.hit = new AnimationGroup();
            MiniBoss_BlackEye_Animations.hit.down = new List<Byte4> { new Byte4(11, 2, 0, 0) };
            MiniBoss_BlackEye_Animations.hit.up = MiniBoss_BlackEye_Animations.hit.down;
            MiniBoss_BlackEye_Animations.hit.right = MiniBoss_BlackEye_Animations.hit.down;
            MiniBoss_BlackEye_Animations.hit.left = MiniBoss_BlackEye_Animations.hit.down;

            MiniBoss_BlackEye_Animations.reward = MiniBoss_BlackEye_Animations.hit;
            MiniBoss_BlackEye_Animations.pickupThrow = MiniBoss_BlackEye_Animations.attack;

            MiniBoss_BlackEye_Animations.death_blank = new AnimationGroup();
            MiniBoss_BlackEye_Animations.death_blank.down = new List<Byte4>
            {
                new Byte4(11, 2, 0, 0), new Byte4(12, 2, 0, 0),
                new Byte4(13, 2, 0, 0), new Byte4(14, 2, 0, 0),
                new Byte4(15, 2, 0, 0) //leftover cracked debris
            };

            MiniBoss_BlackEye_Animations.death_blank.up = MiniBoss_BlackEye_Animations.death_blank.down;
            MiniBoss_BlackEye_Animations.death_blank.right = MiniBoss_BlackEye_Animations.death_blank.down;
            MiniBoss_BlackEye_Animations.death_blank.left = MiniBoss_BlackEye_Animations.death_blank.down;

            MiniBoss_BlackEye_Animations.death_heroic = MiniBoss_BlackEye_Animations.death_blank;
            MiniBoss_BlackEye_Animations.death_heroic_water = MiniBoss_BlackEye_Animations.death_blank;

            MiniBoss_BlackEye_Animations.grab = MiniBoss_BlackEye_Animations.idle;
            MiniBoss_BlackEye_Animations.push = MiniBoss_BlackEye_Animations.idle;
            MiniBoss_BlackEye_Animations.swim_idle = MiniBoss_BlackEye_Animations.idle;
            MiniBoss_BlackEye_Animations.swim_move = MiniBoss_BlackEye_Animations.idle;
            MiniBoss_BlackEye_Animations.swim_dash = MiniBoss_BlackEye_Animations.idle;
            MiniBoss_BlackEye_Animations.swim_hit = MiniBoss_BlackEye_Animations.idle;
            MiniBoss_BlackEye_Animations.swim_reward = MiniBoss_BlackEye_Animations.idle;
            MiniBoss_BlackEye_Animations.underwater_idle = MiniBoss_BlackEye_Animations.idle;
            MiniBoss_BlackEye_Animations.underwater_move = MiniBoss_BlackEye_Animations.idle;
            MiniBoss_BlackEye_Animations.falling = MiniBoss_BlackEye_Animations.idle;
            MiniBoss_BlackEye_Animations.landed = MiniBoss_BlackEye_Animations.idle;
            MiniBoss_BlackEye_Animations.climbing = MiniBoss_BlackEye_Animations.idle;

            #endregion


            #region Miniboss - Spider (Armored)

            MiniBoss_SpiderArmored_Animations = new ActorAnimationList();

            //movement
            MiniBoss_SpiderArmored_Animations.idle = new AnimationGroup();
            MiniBoss_SpiderArmored_Animations.idle.down = new List<Byte4>
            { new Byte4(6, 3, 0, 0), new Byte4(6, 3, 1, 0) };
            MiniBoss_SpiderArmored_Animations.idle.up = MiniBoss_SpiderArmored_Animations.idle.down;
            MiniBoss_SpiderArmored_Animations.idle.right = MiniBoss_SpiderArmored_Animations.idle.down;
            MiniBoss_SpiderArmored_Animations.idle.left = MiniBoss_SpiderArmored_Animations.idle.down;

            MiniBoss_SpiderArmored_Animations.move = MiniBoss_SpiderArmored_Animations.idle;
            MiniBoss_SpiderArmored_Animations.idleCarry = MiniBoss_SpiderArmored_Animations.idle;
            MiniBoss_SpiderArmored_Animations.moveCarry = MiniBoss_SpiderArmored_Animations.idle;
            MiniBoss_SpiderArmored_Animations.interact = MiniBoss_SpiderArmored_Animations.idle;

            //dash is semi-shut eye
            MiniBoss_SpiderArmored_Animations.dash = new AnimationGroup();
            MiniBoss_SpiderArmored_Animations.dash.down = new List<Byte4>
            { new Byte4(6, 4, 0, 0), new Byte4(6, 4, 1, 0) };
            MiniBoss_SpiderArmored_Animations.dash.up = MiniBoss_SpiderArmored_Animations.dash.down;
            MiniBoss_SpiderArmored_Animations.dash.right = MiniBoss_SpiderArmored_Animations.dash.down;
            MiniBoss_SpiderArmored_Animations.dash.left = MiniBoss_SpiderArmored_Animations.dash.down;

            MiniBoss_SpiderArmored_Animations.attack = MiniBoss_SpiderArmored_Animations.dash;
            MiniBoss_SpiderArmored_Animations.hit = MiniBoss_SpiderArmored_Animations.dash;
            MiniBoss_SpiderArmored_Animations.reward = MiniBoss_SpiderArmored_Animations.dash;
            MiniBoss_SpiderArmored_Animations.pickupThrow = MiniBoss_SpiderArmored_Animations.dash;
            MiniBoss_SpiderArmored_Animations.death_blank = MiniBoss_SpiderArmored_Animations.dash;
            MiniBoss_SpiderArmored_Animations.death_heroic = MiniBoss_SpiderArmored_Animations.dash;
            MiniBoss_SpiderArmored_Animations.death_heroic_water = MiniBoss_SpiderArmored_Animations.dash;

            MiniBoss_SpiderArmored_Animations.grab = MiniBoss_SpiderArmored_Animations.idle;
            MiniBoss_SpiderArmored_Animations.push = MiniBoss_SpiderArmored_Animations.idle;
            MiniBoss_SpiderArmored_Animations.swim_idle = MiniBoss_SpiderArmored_Animations.idle;
            MiniBoss_SpiderArmored_Animations.swim_move = MiniBoss_SpiderArmored_Animations.idle;
            MiniBoss_SpiderArmored_Animations.swim_dash = MiniBoss_SpiderArmored_Animations.idle;
            MiniBoss_SpiderArmored_Animations.swim_hit = MiniBoss_SpiderArmored_Animations.idle;
            MiniBoss_SpiderArmored_Animations.swim_reward = MiniBoss_SpiderArmored_Animations.idle;
            MiniBoss_SpiderArmored_Animations.underwater_idle = MiniBoss_SpiderArmored_Animations.idle;
            MiniBoss_SpiderArmored_Animations.underwater_move = MiniBoss_SpiderArmored_Animations.idle;
            MiniBoss_SpiderArmored_Animations.falling = MiniBoss_SpiderArmored_Animations.idle;
            MiniBoss_SpiderArmored_Animations.landed = MiniBoss_SpiderArmored_Animations.idle;
            MiniBoss_SpiderArmored_Animations.climbing = MiniBoss_SpiderArmored_Animations.idle;

            #endregion


            #region Miniboss - Spider (Unarmored)

            MiniBoss_SpiderUnarmored_Animations = new ActorAnimationList();

            //movement
            MiniBoss_SpiderUnarmored_Animations.idle = new AnimationGroup();
            MiniBoss_SpiderUnarmored_Animations.idle.down = new List<Byte4>
            { new Byte4(6, 5, 0, 0), new Byte4(6, 5, 1, 0) };
            MiniBoss_SpiderUnarmored_Animations.idle.up = MiniBoss_SpiderUnarmored_Animations.idle.down;
            MiniBoss_SpiderUnarmored_Animations.idle.right = MiniBoss_SpiderUnarmored_Animations.idle.down;
            MiniBoss_SpiderUnarmored_Animations.idle.left = MiniBoss_SpiderUnarmored_Animations.idle.down;

            MiniBoss_SpiderUnarmored_Animations.move = MiniBoss_SpiderUnarmored_Animations.idle;
            MiniBoss_SpiderUnarmored_Animations.idleCarry = MiniBoss_SpiderUnarmored_Animations.idle;
            MiniBoss_SpiderUnarmored_Animations.moveCarry = MiniBoss_SpiderUnarmored_Animations.idle;
            MiniBoss_SpiderUnarmored_Animations.interact = MiniBoss_SpiderUnarmored_Animations.idle;

            //dash is semi-shut eye
            MiniBoss_SpiderUnarmored_Animations.dash = new AnimationGroup();
            MiniBoss_SpiderUnarmored_Animations.dash.down = new List<Byte4>
            { new Byte4(6, 6, 0, 0), new Byte4(6, 6, 1, 0) };
            MiniBoss_SpiderUnarmored_Animations.dash.up = MiniBoss_SpiderUnarmored_Animations.dash.down;
            MiniBoss_SpiderUnarmored_Animations.dash.right = MiniBoss_SpiderUnarmored_Animations.dash.down;
            MiniBoss_SpiderUnarmored_Animations.dash.left = MiniBoss_SpiderUnarmored_Animations.dash.down;

            MiniBoss_SpiderUnarmored_Animations.attack = MiniBoss_SpiderUnarmored_Animations.dash;

            MiniBoss_SpiderUnarmored_Animations.hit = new AnimationGroup();
            MiniBoss_SpiderUnarmored_Animations.hit.down = new List<Byte4>
            {
                new Byte4(6, 7, 0, 0), new Byte4(6, 7, 1, 0), //eyes closed wobble x3
                new Byte4(6, 7, 0, 0), new Byte4(6, 7, 1, 0),
                new Byte4(6, 7, 0, 0), new Byte4(6, 7, 1, 0),
            };
            MiniBoss_SpiderUnarmored_Animations.hit.up = MiniBoss_SpiderUnarmored_Animations.hit.down;
            MiniBoss_SpiderUnarmored_Animations.hit.right = MiniBoss_SpiderUnarmored_Animations.hit.down;
            MiniBoss_SpiderUnarmored_Animations.hit.left = MiniBoss_SpiderUnarmored_Animations.hit.down;

            MiniBoss_SpiderUnarmored_Animations.reward = MiniBoss_SpiderUnarmored_Animations.dash;
            MiniBoss_SpiderUnarmored_Animations.pickupThrow = MiniBoss_SpiderUnarmored_Animations.dash;

            MiniBoss_SpiderUnarmored_Animations.death_blank = new AnimationGroup();
            MiniBoss_SpiderUnarmored_Animations.death_blank.down = new List<Byte4>
            {
                new Byte4(6, 7, 0, 0), new Byte4(6, 7, 1, 0), //eyes closed wobble x3
                new Byte4(6, 7, 0, 0), new Byte4(6, 7, 1, 0),
                new Byte4(6, 7, 0, 0), new Byte4(6, 7, 1, 0),
                new Byte4(6, 8, 0, 0) //squished spider death
            };
            MiniBoss_SpiderUnarmored_Animations.death_blank.up = MiniBoss_SpiderUnarmored_Animations.death_blank.down;
            MiniBoss_SpiderUnarmored_Animations.death_blank.right = MiniBoss_SpiderUnarmored_Animations.death_blank.down;
            MiniBoss_SpiderUnarmored_Animations.death_blank.left = MiniBoss_SpiderUnarmored_Animations.death_blank.down;

            MiniBoss_SpiderUnarmored_Animations.death_heroic = MiniBoss_SpiderUnarmored_Animations.death_blank;
            MiniBoss_SpiderUnarmored_Animations.death_heroic_water = MiniBoss_SpiderUnarmored_Animations.death_blank;

            MiniBoss_SpiderUnarmored_Animations.grab = MiniBoss_SpiderUnarmored_Animations.idle;
            MiniBoss_SpiderUnarmored_Animations.push = MiniBoss_SpiderUnarmored_Animations.idle;
            MiniBoss_SpiderUnarmored_Animations.swim_idle = MiniBoss_SpiderUnarmored_Animations.idle;
            MiniBoss_SpiderUnarmored_Animations.swim_move = MiniBoss_SpiderUnarmored_Animations.idle;
            MiniBoss_SpiderUnarmored_Animations.swim_dash = MiniBoss_SpiderUnarmored_Animations.idle;
            MiniBoss_SpiderUnarmored_Animations.swim_hit = MiniBoss_SpiderUnarmored_Animations.idle;
            MiniBoss_SpiderUnarmored_Animations.swim_reward = MiniBoss_SpiderUnarmored_Animations.idle;
            MiniBoss_SpiderUnarmored_Animations.underwater_idle = MiniBoss_SpiderUnarmored_Animations.idle;
            MiniBoss_SpiderUnarmored_Animations.underwater_move = MiniBoss_SpiderUnarmored_Animations.idle;
            MiniBoss_SpiderUnarmored_Animations.falling = MiniBoss_SpiderUnarmored_Animations.idle;
            MiniBoss_SpiderUnarmored_Animations.landed = MiniBoss_SpiderUnarmored_Animations.idle;
            MiniBoss_SpiderUnarmored_Animations.climbing = MiniBoss_SpiderUnarmored_Animations.idle;

            #endregion



            #region Miniboss - OctoMouth

            MiniBoss_OctoMouth_Animations = new ActorAnimationList();

            //idle
            MiniBoss_OctoMouth_Animations.idle = new AnimationGroup();
            //two diagonal sprite frames become 4 cardinal directions using MAGIC
            MiniBoss_OctoMouth_Animations.idle.down = new List<Byte4> { new Byte4(11, 3, 1, 0) };
            MiniBoss_OctoMouth_Animations.idle.up = new List<Byte4> { new Byte4(9, 3, 1, 0) };
            MiniBoss_OctoMouth_Animations.idle.right = new List<Byte4> { new Byte4(9, 3, 0, 0) };
            MiniBoss_OctoMouth_Animations.idle.left = new List<Byte4> { new Byte4(11, 3, 0, 0) };

            //attack
            MiniBoss_OctoMouth_Animations.attack = new AnimationGroup();
            //same magic used for attack as well
            MiniBoss_OctoMouth_Animations.attack.down = new List<Byte4> { new Byte4(10, 3, 1, 0) };
            MiniBoss_OctoMouth_Animations.attack.up = new List<Byte4> { new Byte4(8, 3, 1, 0) };
            MiniBoss_OctoMouth_Animations.attack.right = new List<Byte4> { new Byte4(8, 3, 0, 0) };
            MiniBoss_OctoMouth_Animations.attack.left = new List<Byte4> { new Byte4(10, 3, 0, 0) };

            //underwater
            MiniBoss_OctoMouth_Animations.underwater_idle = new AnimationGroup();
            MiniBoss_OctoMouth_Animations.underwater_idle.down = 
                new List<Byte4> { new Byte4(12, 3, 0, 0), new Byte4(12, 3, 1, 0) };
            MiniBoss_OctoMouth_Animations.underwater_idle.up = MiniBoss_OctoMouth_Animations.underwater_idle.down;
            MiniBoss_OctoMouth_Animations.underwater_idle.right = MiniBoss_OctoMouth_Animations.underwater_idle.down;
            MiniBoss_OctoMouth_Animations.underwater_idle.left = MiniBoss_OctoMouth_Animations.underwater_idle.down;
            MiniBoss_OctoMouth_Animations.underwater_move = MiniBoss_OctoMouth_Animations.underwater_idle;

            //hit
            MiniBoss_OctoMouth_Animations.hit = new AnimationGroup();
            MiniBoss_OctoMouth_Animations.hit.down = MiniBoss_OctoMouth_Animations.attack.down;
            MiniBoss_OctoMouth_Animations.hit.up = MiniBoss_OctoMouth_Animations.attack.up;
            MiniBoss_OctoMouth_Animations.hit.right = MiniBoss_OctoMouth_Animations.attack.right;
            MiniBoss_OctoMouth_Animations.hit.left = MiniBoss_OctoMouth_Animations.attack.left;

            //death
            MiniBoss_OctoMouth_Animations.death_blank = MiniBoss_OctoMouth_Animations.underwater_idle;
            MiniBoss_OctoMouth_Animations.death_heroic = MiniBoss_OctoMouth_Animations.underwater_idle;
            MiniBoss_OctoMouth_Animations.death_heroic_water = MiniBoss_OctoMouth_Animations.underwater_idle;


            //unused animation states
            MiniBoss_OctoMouth_Animations.move = MiniBoss_OctoMouth_Animations.idle;
            MiniBoss_OctoMouth_Animations.idleCarry = MiniBoss_OctoMouth_Animations.idle;
            MiniBoss_OctoMouth_Animations.moveCarry = MiniBoss_OctoMouth_Animations.idle;
            MiniBoss_OctoMouth_Animations.interact = MiniBoss_OctoMouth_Animations.idle;
            MiniBoss_OctoMouth_Animations.dash = MiniBoss_OctoMouth_Animations.idle;
            MiniBoss_OctoMouth_Animations.reward = MiniBoss_OctoMouth_Animations.idle;
            MiniBoss_OctoMouth_Animations.pickupThrow = MiniBoss_OctoMouth_Animations.idle;
            MiniBoss_OctoMouth_Animations.grab = MiniBoss_OctoMouth_Animations.idle;
            MiniBoss_OctoMouth_Animations.push = MiniBoss_OctoMouth_Animations.idle;
            MiniBoss_OctoMouth_Animations.swim_idle = MiniBoss_OctoMouth_Animations.idle;
            MiniBoss_OctoMouth_Animations.swim_move = MiniBoss_OctoMouth_Animations.idle;
            MiniBoss_OctoMouth_Animations.swim_dash = MiniBoss_OctoMouth_Animations.idle;
            MiniBoss_OctoMouth_Animations.swim_hit = MiniBoss_OctoMouth_Animations.idle;
            MiniBoss_OctoMouth_Animations.swim_reward = MiniBoss_OctoMouth_Animations.idle;
            MiniBoss_OctoMouth_Animations.falling = MiniBoss_OctoMouth_Animations.idle;
            MiniBoss_OctoMouth_Animations.landed = MiniBoss_OctoMouth_Animations.idle;
            MiniBoss_OctoMouth_Animations.climbing = MiniBoss_OctoMouth_Animations.idle;

            #endregion






            //boss actors

            #region Boss Big Eye Animations

            Boss_BigEye_Animations = new ActorAnimationList();

            //movement
            Boss_BigEye_Animations.idle = new AnimationGroup();
            Boss_BigEye_Animations.idle.down = new List<Byte4> { new Byte4(8, 0, 0, 0), new Byte4(9, 0, 0, 0) };
            Boss_BigEye_Animations.idle.up = Boss_BigEye_Animations.idle.down;
            Boss_BigEye_Animations.idle.right = Boss_BigEye_Animations.idle.down;
            Boss_BigEye_Animations.idle.left = Boss_BigEye_Animations.idle.down;

            Boss_BigEye_Animations.move = Boss_BigEye_Animations.idle;
            Boss_BigEye_Animations.idleCarry = Boss_BigEye_Animations.idle;
            Boss_BigEye_Animations.moveCarry = Boss_BigEye_Animations.idle;
            Boss_BigEye_Animations.interact = Boss_BigEye_Animations.idle;
            Boss_BigEye_Animations.dash = Boss_BigEye_Animations.idle;

            Boss_BigEye_Animations.attack = new AnimationGroup();
            Boss_BigEye_Animations.attack.down = new List<Byte4>
            {
                new Byte4(10, 0, 0, 0), new Byte4(11, 0, 0, 0),
                new Byte4(10, 0, 0, 0), new Byte4(11, 0, 0, 0)
            };
            Boss_BigEye_Animations.attack.up = Boss_BigEye_Animations.attack.down;
            Boss_BigEye_Animations.attack.right = Boss_BigEye_Animations.attack.down;
            Boss_BigEye_Animations.attack.left = Boss_BigEye_Animations.attack.down;

            Boss_BigEye_Animations.hit = new AnimationGroup();
            Boss_BigEye_Animations.hit.down = new List<Byte4> { new Byte4(12, 0, 0, 0), new Byte4(12, 0, 1, 0) };
            Boss_BigEye_Animations.hit.up = Boss_BigEye_Animations.hit.down;
            Boss_BigEye_Animations.hit.right = Boss_BigEye_Animations.hit.down;
            Boss_BigEye_Animations.hit.left = Boss_BigEye_Animations.hit.down;

            Boss_BigEye_Animations.reward = Boss_BigEye_Animations.hit;
            Boss_BigEye_Animations.pickupThrow = Boss_BigEye_Animations.idle;

            Boss_BigEye_Animations.death_heroic = Boss_BigEye_Animations.death_blank;
            Boss_BigEye_Animations.death_heroic_water = Boss_BigEye_Animations.death_blank;
            Boss_BigEye_Animations.death_blank = new AnimationGroup();
            Boss_BigEye_Animations.death_blank.down = new List<Byte4>
            {   //repeating hit frames
                new Byte4(11, 0, 0, 0), new Byte4(12, 0, 0, 0),
                new Byte4(11, 0, 0, 0), new Byte4(12, 0, 0, 0),
                new Byte4(11, 0, 0, 0), new Byte4(12, 0, 0, 0),
                new Byte4(13, 0, 0, 0), new Byte4(14, 0, 0, 0),
                new Byte4(15, 0, 0, 0)
            };
            Boss_BigEye_Animations.death_blank.up = Boss_BigEye_Animations.death_blank.down;
            Boss_BigEye_Animations.death_blank.right = Boss_BigEye_Animations.death_blank.down;
            Boss_BigEye_Animations.death_blank.left = Boss_BigEye_Animations.death_blank.down;

            Boss_BigEye_Animations.grab = Boss_BigEye_Animations.idle;
            Boss_BigEye_Animations.push = Boss_BigEye_Animations.idle;
            Boss_BigEye_Animations.swim_idle = Boss_BigEye_Animations.idle;
            Boss_BigEye_Animations.swim_move = Boss_BigEye_Animations.idle;
            Boss_BigEye_Animations.swim_dash = Boss_BigEye_Animations.idle;
            Boss_BigEye_Animations.swim_hit = Boss_BigEye_Animations.idle;
            Boss_BigEye_Animations.swim_reward = Boss_BigEye_Animations.idle;
            Boss_BigEye_Animations.underwater_idle = Boss_BigEye_Animations.idle;
            Boss_BigEye_Animations.underwater_move = Boss_BigEye_Animations.idle;
            Boss_BigEye_Animations.falling = Boss_BigEye_Animations.idle;
            Boss_BigEye_Animations.landed = Boss_BigEye_Animations.idle;
            Boss_BigEye_Animations.climbing = Boss_BigEye_Animations.idle;

            #endregion


            #region Boss Big Bat Animations

            Boss_BigBat_Animations = new ActorAnimationList();

            //movement
            Boss_BigBat_Animations.idle = new AnimationGroup();
            Boss_BigBat_Animations.idle.down = new List<Byte4>
            { new Byte4(8, 0, 0, 0), new Byte4(9, 0, 0, 0), new Byte4(10, 0, 0, 0) };
            Boss_BigBat_Animations.idle.up = Boss_BigBat_Animations.idle.down;
            Boss_BigBat_Animations.idle.right = Boss_BigBat_Animations.idle.down;
            Boss_BigBat_Animations.idle.left = Boss_BigBat_Animations.idle.down;

            Boss_BigBat_Animations.move = Boss_BigBat_Animations.idle;
            Boss_BigBat_Animations.idleCarry = Boss_BigBat_Animations.idle;
            Boss_BigBat_Animations.moveCarry = Boss_BigBat_Animations.idle;
            Boss_BigBat_Animations.interact = Boss_BigBat_Animations.idle;
            Boss_BigBat_Animations.dash = Boss_BigBat_Animations.idle;

            Boss_BigBat_Animations.attack = new AnimationGroup();
            Boss_BigBat_Animations.attack.down = new List<Byte4>
            { new Byte4(11, 0, 0, 0), new Byte4(11, 0, 0, 0) };
            Boss_BigBat_Animations.attack.up = Boss_BigBat_Animations.attack.down;
            Boss_BigBat_Animations.attack.right = Boss_BigBat_Animations.attack.down;
            Boss_BigBat_Animations.attack.left = Boss_BigBat_Animations.attack.down;

            Boss_BigBat_Animations.hit = new AnimationGroup();
            Boss_BigBat_Animations.hit.down = new List<Byte4>
            { new Byte4(9, 0, 0, 0), new Byte4(9, 0, 0, 0) };
            Boss_BigBat_Animations.hit.up = Boss_BigBat_Animations.hit.down;
            Boss_BigBat_Animations.hit.right = Boss_BigBat_Animations.hit.down;
            Boss_BigBat_Animations.hit.left = Boss_BigBat_Animations.hit.down;

            Boss_BigBat_Animations.reward = Boss_BigBat_Animations.hit;
            Boss_BigBat_Animations.pickupThrow = Boss_BigBat_Animations.idle;

            Boss_BigBat_Animations.death_heroic = Boss_BigBat_Animations.death_blank;
            Boss_BigBat_Animations.death_heroic_water = Boss_BigBat_Animations.death_blank;
            Boss_BigBat_Animations.death_blank = new AnimationGroup();
            Boss_BigBat_Animations.death_blank.down = new List<Byte4>
            {   //laying on ground
                new Byte4(12, 0, 0, 0), new Byte4(12, 0, 0, 0)
            };
            Boss_BigBat_Animations.death_blank.up = Boss_BigBat_Animations.death_blank.down;
            Boss_BigBat_Animations.death_blank.right = Boss_BigBat_Animations.death_blank.down;
            Boss_BigBat_Animations.death_blank.left = Boss_BigBat_Animations.death_blank.down;

            Boss_BigBat_Animations.grab = Boss_BigBat_Animations.idle;
            Boss_BigBat_Animations.push = Boss_BigBat_Animations.idle;
            Boss_BigBat_Animations.swim_idle = Boss_BigBat_Animations.idle;
            Boss_BigBat_Animations.swim_move = Boss_BigBat_Animations.idle;
            Boss_BigBat_Animations.swim_dash = Boss_BigBat_Animations.idle;
            Boss_BigBat_Animations.swim_hit = Boss_BigBat_Animations.idle;
            Boss_BigBat_Animations.swim_reward = Boss_BigBat_Animations.idle;
            Boss_BigBat_Animations.underwater_idle = Boss_BigBat_Animations.idle;
            Boss_BigBat_Animations.underwater_move = Boss_BigBat_Animations.idle;
            Boss_BigBat_Animations.falling = Boss_BigBat_Animations.attack;
            Boss_BigBat_Animations.landed = Boss_BigBat_Animations.death_blank;
            Boss_BigBat_Animations.climbing = Boss_BigBat_Animations.idle;

            #endregion


            #region Boss OctoHead Animations

            Boss_OctoHead_Animations = new ActorAnimationList();

            //above water idle
            Boss_OctoHead_Animations.idle = new AnimationGroup();
            Boss_OctoHead_Animations.idle.down = new List<Byte4>
            {   //oscillate between 3 frames
                new Byte4(8, 0, 0, 0), new Byte4(9, 0, 0, 0),
                new Byte4(10, 0, 0, 0), new Byte4(9, 0, 0, 0)
            };
            Boss_OctoHead_Animations.idle.up = Boss_OctoHead_Animations.idle.down;
            Boss_OctoHead_Animations.idle.right = Boss_OctoHead_Animations.idle.down;
            Boss_OctoHead_Animations.idle.left = Boss_OctoHead_Animations.idle.down;
            Boss_OctoHead_Animations.move = Boss_OctoHead_Animations.idle;
            Boss_OctoHead_Animations.dash = Boss_OctoHead_Animations.idle;

            //below water idle
            Boss_OctoHead_Animations.underwater_idle = new AnimationGroup();
            Boss_OctoHead_Animations.underwater_idle.down = new List<Byte4>
            { new Byte4(11, 0, 0, 0), new Byte4(12, 0, 0, 0) };
            Boss_OctoHead_Animations.underwater_idle.up = Boss_OctoHead_Animations.underwater_idle.down;
            Boss_OctoHead_Animations.underwater_idle.right = Boss_OctoHead_Animations.underwater_idle.down;
            Boss_OctoHead_Animations.underwater_idle.left = Boss_OctoHead_Animations.underwater_idle.down;
            Boss_OctoHead_Animations.underwater_move = Boss_OctoHead_Animations.underwater_idle;
            Boss_OctoHead_Animations.swim_idle = Boss_OctoHead_Animations.idle;
            Boss_OctoHead_Animations.swim_move = Boss_OctoHead_Animations.idle;
            Boss_OctoHead_Animations.swim_dash = Boss_OctoHead_Animations.idle;
            Boss_OctoHead_Animations.swim_hit = Boss_OctoHead_Animations.idle;

            //everything else is unused
            //attack
            Boss_OctoHead_Animations.attack = Boss_OctoHead_Animations.idle;
            //hit
            Boss_OctoHead_Animations.hit = Boss_OctoHead_Animations.idle;
            //death
            Boss_OctoHead_Animations.death_blank = Boss_OctoHead_Animations.underwater_idle;
            Boss_OctoHead_Animations.death_heroic = Boss_OctoHead_Animations.underwater_idle;
            Boss_OctoHead_Animations.death_heroic_water = Boss_OctoHead_Animations.underwater_idle;
            //etc...
            Boss_OctoHead_Animations.idleCarry = Boss_OctoHead_Animations.idle;
            Boss_OctoHead_Animations.moveCarry = Boss_OctoHead_Animations.idle;
            Boss_OctoHead_Animations.interact = Boss_OctoHead_Animations.idle;
            Boss_OctoHead_Animations.reward = Boss_OctoHead_Animations.idle;
            Boss_OctoHead_Animations.pickupThrow = Boss_OctoHead_Animations.idle;
            Boss_OctoHead_Animations.grab = Boss_OctoHead_Animations.idle;
            Boss_OctoHead_Animations.push = Boss_OctoHead_Animations.idle;
            Boss_OctoHead_Animations.swim_reward = Boss_OctoHead_Animations.idle;
            Boss_OctoHead_Animations.falling = Boss_OctoHead_Animations.idle;
            Boss_OctoHead_Animations.landed = Boss_OctoHead_Animations.idle;
            Boss_OctoHead_Animations.climbing = Boss_OctoHead_Animations.idle;

            #endregion





            //special actors

            #region Tentacle
            
            Special_Tentacle_Animations = new ActorAnimationList();

            //movement - special setup
            Special_Tentacle_Animations.idle = new AnimationGroup();
            Special_Tentacle_Animations.idle.right = new List<Byte4>
            { new Byte4(8, 1, 0, 0), new Byte4(9, 1, 0, 0) };
            Special_Tentacle_Animations.idle.left = new List<Byte4>
            { new Byte4(8, 1, 1, 0), new Byte4(9, 1, 1, 0) };
            Special_Tentacle_Animations.idle.down = Special_Tentacle_Animations.idle.left;
            Special_Tentacle_Animations.idle.up = Special_Tentacle_Animations.idle.right;
            
            Special_Tentacle_Animations.move = Special_Tentacle_Animations.idle;
            Special_Tentacle_Animations.idleCarry = Special_Tentacle_Animations.idle;
            Special_Tentacle_Animations.moveCarry = Special_Tentacle_Animations.idle;
            Special_Tentacle_Animations.interact = Special_Tentacle_Animations.idle;
            
            //dash is setup specifically to look better too
            Special_Tentacle_Animations.dash = new AnimationGroup();
            Special_Tentacle_Animations.dash.down = new List<Byte4> { new Byte4(13, 1, 1, 0) };
            Special_Tentacle_Animations.dash.up = new List<Byte4> { new Byte4(13, 1, 0, 0) };
            Special_Tentacle_Animations.dash.right = new List<Byte4> { new Byte4(13, 1, 1, 0) };
            Special_Tentacle_Animations.dash.left = new List<Byte4> { new Byte4(13, 1, 0, 0) };

            //underwater states - special setup here too
            Special_Tentacle_Animations.underwater_idle = new AnimationGroup();
            //define left/right (we have sprites for these)
            Special_Tentacle_Animations.underwater_idle.right = new List<Byte4>
            { new Byte4(14, 1, 1, 0), new Byte4(15, 1, 1, 0) };
            Special_Tentacle_Animations.underwater_idle.left = new List<Byte4>
            { new Byte4(14, 1, 0, 0), new Byte4(15, 1, 0, 0) };
            //stuff up/down into left
            Special_Tentacle_Animations.underwater_idle.down = Special_Tentacle_Animations.underwater_idle.left;
            Special_Tentacle_Animations.underwater_idle.up = Special_Tentacle_Animations.underwater_idle.left;
            Special_Tentacle_Animations.underwater_move = Special_Tentacle_Animations.underwater_idle;



            //attack
            Special_Tentacle_Animations.attack = new AnimationGroup();
            //special setup here too
            Special_Tentacle_Animations.attack.left = new List<Byte4>
            {   //anticipation frames
                new Byte4(11, 1, 0, 0), new Byte4(11, 1, 0, 0),
                //attack frames
                new Byte4(10, 1, 0, 0), new Byte4(10, 1, 0, 0)
            };
            Special_Tentacle_Animations.attack.right = new List<Byte4>
            {   //anticipation frames
                new Byte4(11, 1, 0, 0), new Byte4(11, 1, 0, 0),
                //attack frames
                new Byte4(12, 1, 0, 0), new Byte4(12, 1, 0, 0)
            };

            //there aren't really used
            Special_Tentacle_Animations.attack.down = Special_Tentacle_Animations.attack.left;
            Special_Tentacle_Animations.attack.up = Special_Tentacle_Animations.attack.left;
            Special_Tentacle_Animations.hit = Special_Tentacle_Animations.idle;
            //this guy can't die
            Special_Tentacle_Animations.death_blank = Special_Tentacle_Animations.idle;
            //unused animation states
            Special_Tentacle_Animations.death_heroic = Special_Tentacle_Animations.death_blank;
            Special_Tentacle_Animations.death_heroic_water = Special_Tentacle_Animations.death_blank;
            Special_Tentacle_Animations.reward = Special_Tentacle_Animations.idle;
            Special_Tentacle_Animations.pickupThrow = Special_Tentacle_Animations.idle;
            Special_Tentacle_Animations.grab = Special_Tentacle_Animations.idle;
            Special_Tentacle_Animations.push = Special_Tentacle_Animations.idle;
            Special_Tentacle_Animations.swim_idle = Special_Tentacle_Animations.idle;
            Special_Tentacle_Animations.swim_move = Special_Tentacle_Animations.idle;
            Special_Tentacle_Animations.swim_dash = Special_Tentacle_Animations.idle;
            Special_Tentacle_Animations.swim_hit = Special_Tentacle_Animations.idle;
            Special_Tentacle_Animations.swim_reward = Special_Tentacle_Animations.idle;
            Special_Tentacle_Animations.falling = Special_Tentacle_Animations.idle;
            Special_Tentacle_Animations.landed = Special_Tentacle_Animations.idle;
            Special_Tentacle_Animations.climbing = Special_Tentacle_Animations.idle;

            #endregion

            

        }
    }
}