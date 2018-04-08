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
        public static List<Byte4> Dungeon_DoorTrap = new List<Byte4> { new Byte4(11, 1, 0, 0) };
        public static List<Byte4> Dungeon_DoorBoss = new List<Byte4> { new Byte4(11, 2, 0, 0) };
        public static List<Byte4> Dungeon_DoorBombable = new List<Byte4> { new Byte4(12, 2, 0, 0) };
        public static List<Byte4> Dungeon_DoorFake = new List<Byte4> { new Byte4(12, 2, 0, 0) };

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
        public static List<Byte4> Dungeon_Flamethrower = new List<Byte4> { new Byte4(12, 5, 0, 0) };
        //
        public static List<Byte4> Dungeon_FloorCracked = new List<Byte4> { new Byte4(15, 5, 0, 0) };






        public static List<Byte4> Dungeon_Pot = new List<Byte4> { new Byte4(8, 6, 0, 0) };
        public static List<Byte4> Dungeon_ChestClosed = new List<Byte4> { new Byte4(9, 6, 0, 0) };
        public static List<Byte4> Dungeon_ChestOpened = new List<Byte4> { new Byte4(10, 6, 0, 0) };
        //





        public static List<Byte4> Dungeon_Pit = new List<Byte4> { new Byte4(8, 7, 0, 0) };
        public static List<Byte4> Dungeon_PitBridge = new List<Byte4> { new Byte4(9, 7, 0, 0) };
        public static List<Byte4> Dungeon_PitBubble = new List<Byte4>
        {   //these frames are 8x8
            new Byte4(10*2, 7*2, 0, 0),
            new Byte4(10*2+1, 7*2, 0, 0),
            new Byte4(10*2, 7*2+1, 0, 0),
            new Byte4(10*2+1, 7*2+1, 0, 0)
        };
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
        public static List<Byte4> Dungeon_DebrisSmall = new List<Byte4> { new Byte4(15*2, 9*2, 0, 0) }; //modified later


        #endregion


        #region World Objects

        public static List<Byte4> Dungeon_Entrance = new List<Byte4> { new Byte4(0, 0, 0, 0) };
        public static List<Byte4> World_Tree = new List<Byte4> { new Byte4(0, 0, 0, 0) };
        public static List<Byte4> World_TreeStump = new List<Byte4> { new Byte4(0, 0, 0, 0) };
        public static List<Byte4> World_Bush = new List<Byte4> { new Byte4(0, 0, 0, 0) };
        public static List<Byte4> World_BushStump = new List<Byte4> { new Byte4(0, 0, 0, 0) };

        public static List<Byte4> World_Pit = new List<Byte4> { new Byte4(0, 0, 0, 0) };
        public static List<Byte4> World_PitStairs = new List<Byte4> { new Byte4(0, 0, 0, 0) };

        public static List<Byte4> World_TableStone = new List<Byte4> { new Byte4(5, 6, 0, 0) };

        public static List<Byte4> World_Bookcase = new List<Byte4> { new Byte4(4, 7, 0, 0) };
        public static List<Byte4> World_Shelf = new List<Byte4> { new Byte4(5, 7, 0, 0) };


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
        public static List<Byte4> Particle_Explosion = new List<Byte4>
        {
            new Byte4(0, 1, 0, 0), new Byte4(1, 1, 0, 0),
            new Byte4(2, 1, 0, 0), new Byte4(3, 1, 0, 0)
        };

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
        public static List<Byte4> Particle_FireGround = new List<Byte4>
        {
            new Byte4(5, 2, 0, 0), new Byte4(6, 2, 0, 0),
            new Byte4(7, 2, 0, 0)
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

        public static List<Byte4> Vendor_Items = new List<Byte4> { new Byte4(0, 9, 0, 0) };
        public static List<Byte4> Vendor_Potions = new List<Byte4> { new Byte4(1, 9, 0, 0) };
        public static List<Byte4> Vendor_Magic = new List<Byte4> { new Byte4(2, 9, 0, 0) };
        public static List<Byte4> Vendor_Weapons = new List<Byte4> { new Byte4(3, 9, 0, 0) };

        public static List<Byte4> Vendor_Armor = new List<Byte4> { new Byte4(4, 9, 0, 0) };
        public static List<Byte4> Vendor_Equipment = new List<Byte4> { new Byte4(5, 9, 0, 0) };
        public static List<Byte4> Vendor_Pets = new List<Byte4> { new Byte4(6, 9, 0, 0) };
        public static List<Byte4> Vendor_Story = new List<Byte4> { new Byte4(7, 9, 0, 0) };


        public static List<Byte4> Vendor_Ad_Items = new List<Byte4>
        {
            new Byte4(0, 5, 0, 0), new Byte4(0, 6, 0, 0),
            new Byte4(0, 7, 0, 0), new Byte4(0, 8, 0, 0)
        };

        public static List<Byte4> Vendor_Ad_Potions = new List<Byte4>
        {
            new Byte4(1, 6, 0, 0), new Byte4(1, 7, 0, 0),
            new Byte4(1, 8, 0, 0)
        };

        public static List<Byte4> Vendor_Ad_Magic = new List<Byte4>
        {
            new Byte4(2, 5, 0, 0), new Byte4(2, 6, 0, 0),
            new Byte4(2, 7, 0, 0), new Byte4(2, 8, 0, 0)
        };

        public static List<Byte4> Vendor_Ad_Weapons = new List<Byte4>
        {
            new Byte4(3, 5, 0, 0), new Byte4(3, 6, 0, 0),
            new Byte4(3, 7, 0, 0)
        };

        public static List<Byte4> Vendor_Ad_Armor = new List<Byte4>
        {
            new Byte4(4, 5, 0, 0), new Byte4(4, 6, 0, 0),
            new Byte4(4, 7, 0, 0), new Byte4(4, 8, 0, 0)
        };

        public static List<Byte4> Vendor_Ad_Equipment = new List<Byte4>
        {
            new Byte4(5, 5, 0, 0), new Byte4(5, 6, 0, 0),
            new Byte4(5, 7, 0, 0), new Byte4(5, 8, 0, 0),
            new Byte4(5, 9, 0, 0)
        };

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

        //game ui
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

        //this will become per cheat theme'd in the future
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







    }
}