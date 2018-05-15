﻿using System;
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
        //





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

        public static List<Byte4> Dungeon_SpawnMob = new List<Byte4> { new Byte4(13, 9, 0, 0) };
        public static List<Byte4> Dungeon_SpawnMiniBoss = new List<Byte4> { new Byte4(14, 9, 0, 0) };

        //public static List<Byte4> Dungeon_DebrisSmall = new List<Byte4> { new Byte4(15*2, 9*2, 0, 0) }; //modified later


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
        public static List<Byte4> World_TableWood = new List<Byte4> { new Byte4(2, 15, 0, 0) };
        public static List<Byte4> World_TableStone = new List<Byte4> { new Byte4(3, 15, 0, 0) };

        //dungeon entrances
        public static List<Byte4> Wor_Entrance_ForestDungeon = new List<Byte4> { new Byte4(0, 0, 0, 0) };

        //objects
        public static List<Byte4> Wor_Pot = new List<Byte4> { new Byte4(7, 6, 0, 0) };

        //water tiles
        public static List<Byte4> Wor_Water = new List<Byte4> { new Byte4(3, 2, 0, 0) };
        public static List<Byte4> Wor_Coastline_Straight_A = new List<Byte4> { new Byte4(4, 2, 0, 0) };
        public static List<Byte4> Wor_Coastline_Straight_B = new List<Byte4> { new Byte4(4, 3, 0, 0) };
        public static List<Byte4> Wor_Coastline_Corner_Exterior = new List<Byte4> { new Byte4(5, 2, 0, 0) };
        public static List<Byte4> Wor_Coastline_Corner_Interior = new List<Byte4> { new Byte4(5, 3, 0, 0) };



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

        public static List<Byte4> Projectile_Boomerang = new List<Byte4> { new Byte4(5, 7, 0, 0) };
        public static List<Byte4> Projectile_Bow = new List<Byte4> { new Byte4(6, 7, 0, 0) };
        public static List<Byte4> Projectile_Arrow = new List<Byte4> { new Byte4(7, 7, 0, 0) };
        public static List<Byte4> Projectile_Bomb = new List<Byte4> { new Byte4(8, 7, 0, 0), new Byte4(9, 7, 0, 0) };
        public static List<Byte4> Projectile_Explosion = new List<Byte4>
        { new Byte4(10, 7, 0, 0), new Byte4(11, 7, 0, 0), new Byte4(12, 7, 0, 0), };

        public static List<Byte4> Projectile_Fireball = new List<Byte4> { new Byte4(5, 8, 0, 0) };

        public static List<Byte4> Projectile_FireGround = new List<Byte4>
        {
            new Byte4(5, 2, 0, 0), new Byte4(6, 2, 0, 0),
            new Byte4(7, 2, 0, 0)
        };

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


        #region Vendors

        public static List<Byte4> Vendor_Items = new List<Byte4> { new Byte4(0, 14, 0, 0), new Byte4(0, 14, 1, 0) };
        public static List<Byte4> Vendor_Potions = new List<Byte4> { new Byte4(1, 14, 0, 0), new Byte4(1, 14, 1, 0) };
        public static List<Byte4> Vendor_Magic = new List<Byte4> { new Byte4(2, 14, 0, 0), new Byte4(2, 14, 1, 0) };
        public static List<Byte4> Vendor_Weapons = new List<Byte4> { new Byte4(3, 14, 0, 0), new Byte4(3, 14, 1, 0) };

        public static List<Byte4> Vendor_Armor = new List<Byte4> { new Byte4(4, 14, 0, 0), new Byte4(4, 14, 1, 0) };
        public static List<Byte4> Vendor_Equipment = new List<Byte4> { new Byte4(5, 14, 0, 0), new Byte4(5, 14, 1, 0) };
        public static List<Byte4> Vendor_Pets = new List<Byte4> { new Byte4(6, 14, 0, 0), new Byte4(6, 14, 1, 0) };
        public static List<Byte4> Vendor_Story = new List<Byte4> { new Byte4(7, 14, 0, 0), new Byte4(7, 14, 1, 0) };

        #endregion


        #region Pets

        public static List<Byte4> Pet_None_Idle = new List<Byte4> { new Byte4(0, 0, 0, 0), new Byte4(1, 0, 0, 0) };

        public static List<Byte4> Pet_Dog_Idle = new List<Byte4> { new Byte4(0, 1, 0, 0), new Byte4(1, 1, 0, 0) };
        public static List<Byte4> Pet_Dog_Move = new List<Byte4> { new Byte4(1, 1, 0, 0), new Byte4(2, 1, 0, 0) };

        public static List<Byte4> Pet_Chicken_Idle = new List<Byte4> { new Byte4(0, 2, 0, 0), new Byte4(1, 2, 0, 0) };
        public static List<Byte4> Pet_Chicken_Move = new List<Byte4> { new Byte4(1, 2, 0, 0), new Byte4(2, 2, 0, 0) };

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

        public static List<Byte4> Ui_MenuItem_Bottle_Empty = new List<Byte4> { new Byte4(1, 5, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Bottle_Health = new List<Byte4> { new Byte4(1, 6, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Bottle_Magic = new List<Byte4> { new Byte4(1, 7, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Bottle_Combo = new List<Byte4> { new Byte4(1, 8, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Bottle_Fairy = new List<Byte4> { new Byte4(1, 9, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Bottle_Blob = new List<Byte4> { new Byte4(1, 10, 0, 0) };

        public static List<Byte4> Ui_MenuItem_Magic_Fireball = new List<Byte4> { new Byte4(2, 5, 0, 0) };

        public static List<Byte4> Ui_MenuItem_Weapon_Sword = new List<Byte4> { new Byte4(3, 5, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Weapon_Bow = new List<Byte4> { new Byte4(3, 6, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Weapon_Net = new List<Byte4> { new Byte4(3, 7, 0, 0) };

        public static List<Byte4> Ui_MenuItem_Armor_Cloth = new List<Byte4> { new Byte4(4, 5, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Armor_Cape = new List<Byte4> { new Byte4(4, 6, 0, 0) };

        public static List<Byte4> Ui_MenuItem_Eq_Ring = new List<Byte4> { new Byte4(5, 5, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Eq_Pearl = new List<Byte4> { new Byte4(5, 6, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Eq_Necklace = new List<Byte4> { new Byte4(5, 7, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Eq_Glove = new List<Byte4> { new Byte4(5, 8, 0, 0) };
        public static List<Byte4> Ui_MenuItem_Eq_Pin = new List<Byte4> { new Byte4(5, 9, 0, 0) };

        #endregion


        


        //actor animation frames
        public static ActorAnimationList Hero_Animations;
        public static ActorAnimationList Boss_Blob_Animations;

        //actor fx anim frames
        public static List<Byte4> ActorFX_GrassyFeet = new List<Byte4>
        { new Byte4(4, 1, 0, 0), new Byte4(4, 1, 1, 0) };
        public static List<Byte4> ActorFX_WetFeet = new List<Byte4> //16x8
        { new Byte4(3, 6, 0, 0), new Byte4(3, 7, 0, 0) };


        static AnimationFrames()
        {


            #region Hero Animations

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

            Hero_Animations.death = new AnimationGroup();
            Hero_Animations.death.down = new List<Byte4>
            {
                //spin clockwise twice, then fall
                new Byte4(0, 0, 0, 0), new Byte4(0, 2, 1, 0), new Byte4(0, 1, 0, 0), new Byte4(0, 2, 0, 0),
                new Byte4(0, 0, 0, 0), new Byte4(0, 2, 1, 0), new Byte4(0, 1, 0, 0), new Byte4(0, 2, 0, 0),
                new Byte4(1, 3, 0, 0),
                new Byte4(2, 3, 0, 0) //fell, dead
            };
            Hero_Animations.death.up = Hero_Animations.death.down;
            Hero_Animations.death.right = Hero_Animations.death.down;
            Hero_Animations.death.left = Hero_Animations.death.down;

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

            #endregion


            #region Boss Blob Animations

            Boss_Blob_Animations = new ActorAnimationList();

            //movement
            Boss_Blob_Animations.idle = new AnimationGroup();
            Boss_Blob_Animations.idle.down = new List<Byte4> { new Byte4(0, 0, 0, 0) };
            Boss_Blob_Animations.idle.up = new List<Byte4> { new Byte4(0, 1, 0, 0) };
            Boss_Blob_Animations.idle.right = new List<Byte4> { new Byte4(0, 2, 0, 0) };
            Boss_Blob_Animations.idle.left = new List<Byte4> { new Byte4(0, 2, 1, 0) };

            Boss_Blob_Animations.move = new AnimationGroup();
            Boss_Blob_Animations.move.down = new List<Byte4> { new Byte4(1, 0, 0, 0), new Byte4(1, 0, 1, 0) };
            Boss_Blob_Animations.move.up = new List<Byte4> { new Byte4(1, 1, 0, 0), new Byte4(1, 1, 1, 0) };
            Boss_Blob_Animations.move.right = new List<Byte4> { new Byte4(0, 2, 0, 0), new Byte4(1, 2, 0, 0) };
            Boss_Blob_Animations.move.left = new List<Byte4> { new Byte4(0, 2, 1, 0), new Byte4(1, 2, 1, 0) };

            //actions
            Boss_Blob_Animations.dash = new AnimationGroup();
            Boss_Blob_Animations.dash.down = new List<Byte4> { new Byte4(2, 0, 0, 0) };
            Boss_Blob_Animations.dash.up = new List<Byte4> { new Byte4(2, 1, 0, 0) };
            Boss_Blob_Animations.dash.right = new List<Byte4> { new Byte4(2, 2, 0, 0) };
            Boss_Blob_Animations.dash.left = new List<Byte4> { new Byte4(2, 2, 1, 0) };

            Boss_Blob_Animations.attack = new AnimationGroup();
            Boss_Blob_Animations.attack.down = new List<Byte4> { new Byte4(3, 0, 0, 0) };
            Boss_Blob_Animations.attack.up = new List<Byte4> { new Byte4(3, 1, 0, 0) };
            Boss_Blob_Animations.attack.right = new List<Byte4> { new Byte4(3, 2, 0, 0) };
            Boss_Blob_Animations.attack.left = new List<Byte4> { new Byte4(3, 2, 1, 0) };


            //reward (cast in this case) list
            Boss_Blob_Animations.reward = new AnimationGroup();
            Boss_Blob_Animations.reward.down = new List<Byte4> { new Byte4(0, 3, 0, 0) };
            Boss_Blob_Animations.reward.up = Hero_Animations.reward.down;
            Boss_Blob_Animations.reward.right = Hero_Animations.reward.down;
            Boss_Blob_Animations.reward.left = Hero_Animations.reward.down;

            Boss_Blob_Animations.hit = new AnimationGroup();
            Boss_Blob_Animations.hit.down = new List<Byte4> { new Byte4(1, 3, 0, 0) };
            Boss_Blob_Animations.hit.up = Hero_Animations.hit.down;
            Boss_Blob_Animations.hit.right = Hero_Animations.hit.down;
            Boss_Blob_Animations.hit.left = Hero_Animations.hit.down;

            Boss_Blob_Animations.death = new AnimationGroup();
            Boss_Blob_Animations.death.down = new List<Byte4> { new Byte4(2, 3, 0, 0) };
            Boss_Blob_Animations.death.up = Hero_Animations.death.down;
            Boss_Blob_Animations.death.right = Hero_Animations.death.down;
            Boss_Blob_Animations.death.left = Hero_Animations.death.down;

            #endregion


        }
    }
}