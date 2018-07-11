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
    public enum Direction
    {
        Up, UpRight,
        Right, DownRight,
        Down, DownLeft,
        Left, UpLeft,

        None
    }

    public enum Rotation { None, Clockwise90, Clockwise180, Clockwise270 }

    public enum MouseButtons { LeftButton, RightButton }

    public enum BootRoutine
    {
        Game,   //loads title screen
        Editor_Room,  //loads editor in room mode
        Editor_Level,  //loads editor in level mode
    }

    public enum WidgetDisplaySet { World, Dungeon, None }



    public enum DisplayState { Opening, Opened, Closing, Closed }

    public enum FadeState { FadeIn, FadeComplete, FadeOut, Silent }

    public enum ObjToolState { MoveObj, RotateObj, AddObj, DeleteObj }

    public enum LoadSaveNewState { Load, Save, New }

    public enum GameFile { AutoSave, Game1, Game2, Game3 }

    public enum ExitAction
    {
        Title,

        //hero either dies/beats dungeon or exits
        Summary, //died/beat boss, show summary
        ExitDungeon, //left dungeon, goto overworld screen

        Overworld, //goto overworld screen
        Level, //build a level (dungeon or overworld)

        QuitGame,
        ExitScreen
    }



    public enum Achievements
    {
        WallJumps,
    }




    public enum LevelID
    {
        //overworld levels (fields)
        Colliseum,
        ColliseumPit,
        TheFarm, 
        LeftTown2,

        //fields, with dungeon entrances
        Forest_Entrance, 
        Mountain_Entrance,

        //dungeons, series of rooms
        Forest_Dungeon, 
        Mountain_Dungeon,
        
        //locations that cannot be visited
        Road, //used for connecting levels on map

        //dev (hidden) unreachable locations from game
        DEV_Room, //single room with walls/doors + rules
        DEV_Field, //single room without walls/doors
    }

    public enum RoomID
    {
        //these DUNGEON rooms have procedural objs added to them
        Exit, Hub, Boss, Key,

        //these DUNGEON rooms are mostly non-procedural and handmade
        Column, Row, Square, Secret,

        //these OVERWORLD rooms are non-procedural (handmade)
        Colliseum,
        ColliseumPit,

        ForestEntrance,
        MountainEntrance,

        TheFarm,
        LeftTown2,


        //these DEV rooms are used for testing
        DEV_Field, //represents any 'outdoor' / overworld level

        //dungeon rooms that are built empty, for adding objs (development)
        DEV_Exit, DEV_Hub, DEV_Boss, DEV_Key,
        DEV_Column, DEV_Row, DEV_Square
    }


    


    public enum Music
    {
        DungeonA,
        DungeonB,
        DungeonC,
        Boss,

        Title,
        LightWorld,

        CrowdWaiting,
        CrowdFighting,

        None
    }

    

    public enum DoorType { Open, Boss, Bombable }

    public enum PuzzleType
    {   //in order of importance to the codebase
        None,       //no room puzzle
        Switch,     //an obj must be placed onto switch
        Torches,    //all torches must be lit
    }



    public enum ActorType
    {
        //standard actors (heros)
        Hero, //main
        Blob, //2ndary, playable

        //dungeon standard actors
        Standard_AngryEye, //forest
        Standard_BeefyBat, //mountain

        //minibosses
        MiniBoss_BlackEye,
        MiniBoss_Spider_Armored,
        MiniBoss_Spider_Unarmored,

        //bosses
        Boss_BigEye,
        Boss_BigBat,
    }

    public enum ActorState
    {
        Idle, Move, Dash, //movements
        Interact, Attack, Use, //actions
        Hit, Dead, Reward, //consequences
        Pickup, Throw, //actions

        Falling, Landed,
        Climbing, 
    }

    public enum ActorAI
    {
        Random,

        Basic, //aka 'standard'

        Miniboss_Blackeye,

        Boss_BigEye,
        Boss_BigBat
    }



    public enum ObjGroup
    {   //roomObjs
        Wall,
        Door,
        Chest,
        Object,

        Vendor,
        NPC,

        Enemy,
        EnemySpawn,
        Ditch,

        Wall_Climbable, //for wall climbing objects

        //entities
        Pickup,
        Projectile,
        Particle 
    } 

    public enum ObjType
    {

        Unknown, //unknown ? box, non-entity


        #region Dungeon Objects

        //PROCEDURALLY PLACED
        Dungeon_ExitPillarLeft,
        Dungeon_ExitPillarRight,
        Dungeon_Exit, 
        Dungeon_ExitLight, 

        Dungeon_DoorOpen,
        Dungeon_DoorBombable, 
        Dungeon_DoorBoss,
        Dungeon_DoorTrap,
        Dungeon_DoorShut, 
        Dungeon_DoorFake, 

        Dungeon_WallStraight,
        Dungeon_WallStraightCracked, 
        Dungeon_WallInteriorCorner, 
        Dungeon_WallExteriorCorner,
        Dungeon_WallPillar,
        Dungeon_WallStatue,
        Dungeon_WallTorch,

        Dungeon_FloorDecal,
        Dungeon_FloorBlood, 

        Dungeon_Map, //unlocks dungeon map

        //HAND PLACED
        Dungeon_Pit, 
        Dungeon_PitBridge,
        Dungeon_PitTeethTop,
        Dungeon_PitTeethBottom,
        Dungeon_PitTrap, 

        Dungeon_Statue, 
        Dungeon_SkullPillar,

        Dungeon_Chest, //placeable in editor, becomes key chest
        Dungeon_ChestKey, //not placeable, procedurally set
        Dungeon_ChestEmpty, //just empty

        Dungeon_Signpost,

        Dungeon_BlockDark,
        Dungeon_BlockLight,
        Dungeon_BlockSpike,

        //lever + lever activated objects
        Dungeon_LeverOn,
        Dungeon_LeverOff,
        Dungeon_SpikesFloorOn,
        Dungeon_SpikesFloorOff,
        Dungeon_ConveyorBeltOn,
        Dungeon_ConveyorBeltOff,

        //Room Objects
        Dungeon_Pot,
        Dungeon_Barrel,
        Dungeon_Bumper,
        Dungeon_Flamethrower,
        Dungeon_IceTile,

        Dungeon_Switch,
        Dungeon_SwitchDown,

        Dungeon_SwitchBlockBtn,
        Dungeon_SwitchBlockDown,
        Dungeon_SwitchBlockUp,
        Dungeon_TorchUnlit,
        Dungeon_TorchLit,

        //'Living' RoomObjs
        Dungeon_Fairy,

        //Actor Spawn Objects (hand-placed)
        Dungeon_SpawnMob, //spawn a standard enemy, based on dungeon type

        #endregion


        #region World Objects

        //interior building objects
        Wor_Bookcase, 
        Wor_Shelf, 
        Wor_TableSingle,
        Wor_TableDoubleLeft,
        Wor_TableDoubleRight,

        Wor_Stove,
        Wor_Sink,
        Wor_Chair,

        Wor_Bed,

        //grass objects
        Wor_Grass_Tall,
        Wor_Grass_Cut,
        Wor_Grass_2,
        Wor_Flowers,

        //foilage
        Wor_Bush,
        Wor_Bush_Stump,

        Wor_Tree,
        Wor_Tree_Stump,
        Wor_Tree_Burning,
        Wor_Tree_Burnt,

        //debris
        Wor_Debris,


        //objects
        Wor_Pot,

        //water objs
        Wor_Water,
        Wor_Coastline_Straight,
        Wor_Coastline_Corner_Exterior,
        Wor_Coastline_Corner_Interior,

        //ditch objs
        Wor_Ditch_META, //becomes one of the ditch objs below upon placement

        Wor_Ditch_Empty_Single,
        Wor_Ditch_Empty_4UP,
        Wor_Ditch_Empty_Vertical,
        Wor_Ditch_Empty_Horizontal,

        Wor_Ditch_Empty_Corner_North,
        Wor_Ditch_Empty_Corner_South,
        Wor_Ditch_Empty_3UP_North,
        Wor_Ditch_Empty_3UP_South,

        Wor_Ditch_Empty_3UP_Horizontal,
        Wor_Ditch_Empty_Endcap_South,
        Wor_Ditch_Empty_Endcap_Horizontal,
        Wor_Ditch_Empty_Endcap_North,

        //fence and gate
        Wor_Fence_Horizontal,
        Wor_Fence_Vertical_Left,
        Wor_Fence_Vertical_Right,
        Wor_Fence_Gate,

        //building objs
        Wor_Build_Wall_FrontA,
        Wor_Build_Wall_FrontB,
        Wor_Build_Wall_Back,
        Wor_Build_Wall_Side_Left,
        Wor_Build_Wall_Side_Right,

        Wor_Build_Door_Shut,
        Wor_Build_Door_Open,

        //roofs
        Wor_Build_Roof_Top,
        Wor_Build_Roof_Bottom,
        Wor_Build_Roof_Chimney,
        Wor_Build_Roof_Collapsing, //meta obj for collapsing roofs


        #endregion




        #region Colliseum Objects

        Wor_Entrance_Colliseum,

        //colliseum objects
        Wor_Colliseum_Gate_Center,
        Wor_Colliseum_Gate_Pillar_Left,
        Wor_Colliseum_Gate_Pillar_Right,

        Wor_Colliseum_Pillar_Top,
        Wor_Colliseum_Pillar_Middle,
        Wor_Colliseum_Pillar_Bottom,

        Wor_Colliseum_Stairs_Handrail_Top,
        Wor_Colliseum_Stairs_Handrail_Middle,
        Wor_Colliseum_Stairs_Handrail_Bottom,

        Wor_Colliseum_Stairs_Left,
        Wor_Colliseum_Stairs_Middle,
        Wor_Colliseum_Stairs_Right,

        Wor_Colliseum_Bricks_Left,
        Wor_Colliseum_Bricks_Middle1,
        Wor_Colliseum_Bricks_Middle2,
        Wor_Colliseum_Bricks_Right,

        Wor_Colliseum_Outdoors_Floor,
        Wor_Colliseum_Spectator,

        #endregion


        #region Forest Objects

        Wor_Entrance_ForestDungeon,

        #endregion


        #region Mountain Objects

        Wor_Entrance_MountainDungeon,

        Wor_MountainWall_Top,
        Wor_MountainWall_Mid,
        Wor_MountainWall_Bottom,

        Wor_MountainWall_Foothold,
        Wor_MountainWall_Ladder,
        Wor_MountainWall_Ladder_Trap,

        #endregion





        #region RoomObj Enemies

        Wor_Enemy_Turtle,
        Wor_Enemy_Crab,
        Wor_Enemy_Rat,

        Wor_SeekerExploder, //seeks hero

        #endregion



        #region Vendors & NPCs

        //Vendor NPCs
        Vendor_NPC_Items,
        Vendor_NPC_EnemyItems,

        Vendor_NPC_Potions,
        Vendor_NPC_Magic,
        Vendor_NPC_Weapons,
        Vendor_NPC_Armor,
        Vendor_NPC_Equipment,

        Vendor_NPC_Pets,
        
        NPC_Story,

        NPC_Farmer,
        NPC_Farmer_Reward,
        NPC_Farmer_EndDialog,

        Vendor_Colliseum_Mob,
        Judge_Colliseum,

        #endregion


        #region Pickups

        //Pickups - collide with hero
        Pickup_Rupee,
        Pickup_Heart,
        Pickup_Magic,
        Pickup_Arrow,
        Pickup_Bomb,

        #endregion


        #region Projectiles

        //Projectiles - items
        ProjectileBomb,
        ProjectileFireball,
        ProjectileLightningBolt,
        ProjectileBoomerang,

        //Projectiles - weapons
        ProjectileSword,
        ProjectileArrow,
        ProjectileNet,
        ProjectileBow,
        ProjectileShovel,

        //Projectiles - object
        ProjectileExplosion,
        ProjectileGroundFire,

        //Projectiles - thrown objs
        ProjectileBush,
        ProjectilePot,
        ProjectilePotSkull,

        //Projectiles - casted magic
        ProjectileBombos,

        //Projectiles - enemy related
        ProjectileBite,
        ProjectileBat,


        




        #endregion


        #region Particles

        //Particles - small
        Particle_RisingSmoke, 
        Particle_ImpactDust, 
        Particle_Sparkle, 

        //Particles - small, obj specific
        Particle_PitBubble,
        Particle_Leaf,
        Particle_Debris,

        //Particles - map
        Particle_Map_Flag,
        Particle_Map_Wave,
        Particle_Map_Campfire,

        //Particles - normal size
        Particle_Attention,
        Particle_Splash,
        Particle_Blast,
        Particle_Push,
        Particle_Fire,
        Particle_WaterKick,
        Particle_ExclamationBubble,

        //Particle_Debris, //doesn't exist right now
        Particle_BottleEmpty,
        Particle_BottleHealth,
        Particle_BottleMagic,
        Particle_BottleCombo,
        Particle_BottleFairy,
        Particle_BottleBlob,

        //Particles - rewards
        Particle_RewardKey,
        Particle_RewardMap,

        #endregion


        #region Pets

        Pet_None,
        Pet_Dog,
        Pet_Chicken,

        #endregion


        #region Misc (Dialog)

        Hero_Idle,

        #endregion




        //very special objects
        ExplodingObject, //creates explosion

    }

    public enum MenuItemType
    {
        Unknown,


        #region Option relate menuItems

        OptionsNewGame,
        OptionsLoadGame,
        OptionsQuitGame,

        OptionsSaveGame,
        OptionsOptionsMenu,
        OptionsCheatMenu,

        Options_DrawInput,
        Options_TrackCamera,
        Options_Watermark,
        Options_HardMode,
        Options_DrawBuildTimes,
        Options_PlayMusic,
        Options_DrawDebug,
        Options_DrawHitBoxes,

        #endregion


        #region Cheats

        CheatsInfiniteHP,
        CheatsInfiniteGold,
        CheatsInfiniteMagic,
        CheatsInfiniteArrows,
        CheatsInfiniteBombs,
        CheatsMap,
        CheatsKey,
        CheatsUnlockAll,
        CheatsClipping,
        CheatsInfiniteFairies,


        #endregion


        //loadout menuItems
        InventoryGold,
        InventoryMap,
        InventoryKey,

        //vendor menuItems
        ItemHeart,
        ItemBomb3Pack,
        ItemArrowPack,


        #region Inventory menuItems

        ItemBoomerang,
        ItemBomb,
        ItemBow,

        BottleEmpty,
        BottleHealth,
        BottleMagic,
        BottleCombo,
        BottleFairy,
        BottleBlob,
        
        MagicFireball,
        MagicBombos,
        MagicBolt,
        MagicBat,

        WeaponSword,
        WeaponNet, 
        WeaponShovel,
        WeaponFang,

        ArmorCloth,
        ArmorCape,

        EquipmentRing,
        EquipmentPearl,
        EquipmentNecklace,
        EquipmentGlove,
        EquipmentPin,

        #endregion


        //pet menuItems
        PetDog_Gray,

        #region Colliseum Challenges

        Challenge_Blobs,

        Challenge_Mini_BlackEyes,
        Challenge_Mini_Spiders,

        Challenge_Bosses_BigEye,
        Challenge_Bosses_BigBat,

        #endregion

    }

}