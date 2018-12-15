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
        Editor_Map,  //loads editor in overworld map mode
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

        GameCreated, //from title screen
        Overworld, //goto overworld screen

        Field, //build a field level
        Dungeon, //build a dungeon level

        QuitGame,
        ExitScreen
    }



    public enum Achievements
    {
        WallJumps,
    }










    public enum IslandID
    {
        #region IslandID

        LevelData_SkullIsland,
        LevelData_ForestIsland,
        LevelData_ThievesHideout,
        LevelData_DeathMountain,
        LevelData_HauntedSwamps,
        LevelData_CloudIsland,
        LevelData_LavaIsland

        //expand this to handle the smaller map islands too

        #endregion
    }
    public enum LevelID
    {
        #region LevelID

        //overworld levels (fields) - overworld accessible

        //skull island
        SkullIsland_Colliseum,
        SkullIsland_ColliseumPit,
        SkullIsland_Town,
        SkullIsland_ShadowKing,

        //death mountain
        DeathMountain_MainEntrance,

        //forest island
        ForestIsland_MainEntrance,

        //lava island
        LavaIsland_MainEntrance,

        //cloud island
        CloudIsland_MainEntrance,

        //swamp island
        SwampIsland_MainEntrance,

        //thieves den island
        ThievesDen_GateEntrance,






        //locations that cannot be visited
        Road, //used for connecting levels on map

        //dungeons, series of rooms - not overworld accessible
        Forest_Dungeon,
        Mountain_Dungeon,
        Swamp_Dungeon,

        //dev (hidden) unreachable locations from game
        DEV_Field, //single room without walls/doors

        #endregion    }
    }
    public enum RoomID
    {
        #region RoomID

        //these DUNGEON rooms have procedural objs added to them
        Exit,
        Key,

        //these DUNGEON rooms are mostly non-procedural and handmade
        Column,
        Row,
        Square,
        Secret,

        //these dungeon rooms are specific to dungeon and handmade
        ForestIsland_BossRoom,
        DeathMountain_BossRoom,
        SwampIsland_BossRoom,

        ForestIsland_HubRoom,
        DeathMountain_HubRoom,
        SwampIsland_HubRoom,





        //these OVERWORLD rooms are non-procedural (handmade)

        //skull island
        SkullIsland_Colliseum,
        SkullIsland_ColliseumPit,
        SkullIsland_Town,
        SkullIsland_ShadowKing,

        //death mountain
        DeathMountain_MainEntrance,

        //forest island
        ForestIsland_MainEntrance,

        //lava island
        LavaIsland_MainEntrance,

        //cloud island
        CloudIsland_MainEntrance,

        //swamp island
        SwampIsland_MainEntrance,

        //thieves den island
        ThievesDen_GateEntrance,





        //these DEV rooms are used for testing
        DEV_Field, //represents any 'outdoor' / overworld level

        //dungeon rooms that are built empty, for adding objs (development)
        DEV_Exit, DEV_Hub, DEV_Boss, DEV_Key,
        DEV_Column, DEV_Row, DEV_Square

        #endregion
    }





    public enum DoorType { Open, Boss, Bombable }

    public enum PuzzleType
    {   //in order of importance to the codebase
        None,       //no room puzzle
        Switch,     //an obj must be placed onto switch
        Torches,    //all torches must be lit
    }





    #region Music

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

    #endregion









    #region ActorType

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
        MiniBoss_OctoMouth,

        //bosses
        Boss_BigEye,
        Boss_BigBat,
        Boss_OctoHead,

        //special actors
        Special_Tentacle,
    }

    #endregion


    #region Actor AI

    public enum ActorAI
    {
        Random,

        Basic, //aka 'standard'

        Standard_BeefyBat, //special standards

        Miniboss_Blackeye,
        MiniBoss_OctoMouth,

        Boss_BigEye,
        Boss_BigBat,
        Boss_OctoHead,

        Special_Tentacle
    }

    #endregion


    #region Actor State

    public enum ActorState
    {
        Idle, Move, Dash, //movements
        Interact, Attack, Use, //actions
        Hit, Dead, Reward, //consequences
        Pickup, Throw, //actions

        Falling, Landed,
        Climbing,
    }

    #endregion









    




    public enum IndestructibleGroup
    {
        Object,
        Exit,
    }

    public enum IndestructibleType
    {
        Unknown,


        #region Dungeon Objects

        Dungeon_ExitPillarLeft,
        Dungeon_ExitPillarRight,
        Dungeon_Exit,
        Dungeon_ExitLight,

        Dungeon_SkullPillar,
        Dungeon_BlockDark,

        #endregion


        #region World Objects

        //med & big trees
        Tree_Med,
        Tree_Med_Stump,
        Tree_Big,

        //big shadow cover
        Shadow_Big,

        #endregion


        #region Coliseum Objects

        //shadow coliseum
        Coliseum_Shadow_Entrance,

        Coliseum_Shadow_Pillar_Top,
        Coliseum_Shadow_Pillar_Middle,
        Coliseum_Shadow_Pillar_Bottom,

        Coliseum_Shadow_Stairs_Handrail_Top,
        Coliseum_Shadow_Stairs_Handrail_Middle,
        Coliseum_Shadow_Stairs_Handrail_Bottom,

        Coliseum_Shadow_Bricks_Left, //not added to editor yet
        Coliseum_Shadow_Bricks_Middle1, //not added to editor yet
        Coliseum_Shadow_Bricks_Middle2, //not added to editor yet
        Coliseum_Shadow_Bricks_Right, //not added to editor yet

        Coliseum_Shadow_Spectator,

        #endregion


        #region Forest Objects

        ForestDungeon_Entrance, //big shadow between teeth in water

        //objects that build the skull teeth
        ForestDungeon_SkullToothInWater_Left,
        ForestDungeon_SkullToothInWater_Right,

        ForestDungeon_SkullToothInWater_Arch_Left,
        ForestDungeon_SkullToothInWater_Arch_Right,
        ForestDungeon_SkullToothInWater_Arch_Extension,

        ForestDungeon_SkullToothInWater_Center,

        ForestDungeon_SkullToothInWater_EndCap_Left,
        ForestDungeon_SkullToothInWater_EndCap_Right,

        #endregion


        #region Mountain Objects
        
        MountainDungeon_Entrance, //batcave

        MountainWall_Alcove_Left,
        MountainWall_Alcove_Right,

        MountainWall_Cave_Bare,
        MountainWall_Cave_Covered,

        #endregion


        #region Swamp Objects

        SwampDungeon_Entrance,

        #endregion


        #region Boat Objects

        Boat_Front,
        Boat_Front_Left,
        Boat_Front_Right,

        Boat_Front_ConnectorLeft,
        Boat_Front_ConnectorRight,

        Boat_Bannister_Left,
        Boat_Bannister_Right,

        Boat_Stairs_Top_Left,
        Boat_Stairs_Top_Right,

        Boat_Stairs_Bottom_Left,
        Boat_Stairs_Bottom_Right,

        Boat_Back_Left,
        Boat_Back_Left_Connector,
        Boat_Back_Center,
        Boat_Back_Right_Connector,
        Boat_Back_Right,

        Boat_Engine,
        
        #endregion


    }





    public enum InteractiveGroup
    {
        Object,

        Wall_Dungeon,
        Door_Dungeon,

        Vendor,
        NPC,
        Enemy,
        EnemySpawn,
        Ditch,
        Wall_Climbable,
    }

    public enum InteractiveType
    {
        Unknown,


        #region Dungeon Objs

        Dungeon_WallStraight,
        Dungeon_WallStraightCracked,
        Dungeon_WallInteriorCorner,
        Dungeon_WallExteriorCorner,
        Dungeon_WallPillar,
        Dungeon_WallStatue,
        Dungeon_WallTorch,
        Dungeon_DoorFake, //is just wall

        Dungeon_DoorOpen,
        Dungeon_DoorBombable,
        Dungeon_DoorBoss,
        Dungeon_DoorTrap,
        Dungeon_DoorShut,

        Dungeon_Pot, //skull
        Dungeon_Map, //unlocks dungeon map
        Dungeon_Statue,
        Dungeon_FloorDecal, //floor icon for boss

        Dungeon_BlockLight,
        Dungeon_BlockSpike,
        Dungeon_SpikesFloorOn,
        Dungeon_SpikesFloorOff,
        
        Dungeon_Switch, //the up switch, becomes down
        Dungeon_SwitchDown, //down requires obj or actor on it
        Dungeon_SwitchDownPerm, //special, bypasses switch puzzle

        Dungeon_SwitchBlockBtn,
        Dungeon_SwitchBlockDown,
        Dungeon_SwitchBlockUp,

        Dungeon_SpawnMob, //spawn a standard enemy, based on dungeon type
        

        #endregion


        #region World Objects

        //common floor objs
        Debris,
        FloorStain, //procedurally added
        FloorBlood, //created by just-dead actors/objs
        FloorSkeleton, //^ randomly becomes skeleton anim 1 or 2

        //common environment objs
        Grass_Tall,
        Grass_Cut,
        Grass_2,
        Flowers,
        Grass_Burned,
        Bush,
        Bush_Stump,
        Tree,
        Tree_Stump,
        Tree_Burning,
        Tree_Burnt,
        
        //common objs
        Pot,
        TorchUnlit,
        TorchLit,
        Fairy,
        Barrel,
        Bumper,
        Flamethrower,
        ConveyorBeltOn,
        ConveyorBeltOff,
        LeverOn,
        LeverOff,
        Chest, //
        ChestKey, //not placeable, procedurally set
        ChestEmpty, //just empty
        Signpost,

        //dirt + transition objs
        Dirt_Main,
        Dirt_ToGrass_Straight,
        Dirt_ToGrass_Corner_Exterior,
        Dirt_ToGrass_Corner_Interior,

        IceTile,

        #endregion


        #region Lava Pits

        Lava_Pit,
        Lava_PitBridge,
        Lava_PitTeethTop,
        Lava_PitTeethBottom,
        Lava_PitTrap,

        #endregion


        #region Ditch Objects

        Ditch_META, //becomes one of the ditch objs below upon placement

        Ditch_Empty_Single,
        Ditch_Empty_4UP,
        Ditch_Empty_Vertical,
        Ditch_Empty_Horizontal,

        Ditch_Empty_Corner_North,
        Ditch_Empty_Corner_South,
        Ditch_Empty_3UP_North,
        Ditch_Empty_3UP_South,

        Ditch_Empty_3UP_Horizontal,
        Ditch_Empty_Endcap_South,
        Ditch_Empty_Endcap_Horizontal,
        Ditch_Empty_Endcap_North,

        #endregion


        #region Posts

        Post_VerticalRight,
        Post_CornerRight,
        Post_Horizontal,
        Post_CornerLeft,
        Post_VerticalLeft,

        PostBurned_VerticalRight,
        PostBurned_CornerRight,
        PostBurned_Horizontal,
        PostBurned_CornerLeft,
        PostBurned_VerticalLeft,

        //posts that only hammers can destroy
        Post_HammerPost_Up,
        Post_HammerPost_Down,

        #endregion







        #region Water Objects

        //water objs
        Water_1x1,
        Water_2x2,
        Water_3x3,
        Water_RockUnderwater,
        Water_LillyPad,
        Water_Vine,
        //blocking water objects
        Water_RockSm,
        Water_RockMed,
        Water_BigPlant,
        Water_Bulb,
        Water_SmPlant,
        //coastlines
        Coastline_Straight,
        Coastline_Corner_Exterior,
        Coastline_Corner_Interior,
        Coastline_1x2_Animated,

        #endregion


        #region House Objects

        //interior
        House_Bookcase,
        House_Shelf,
        House_TableSingle,
        House_TableDoubleLeft,
        House_TableDoubleRight,
        House_Stove,
        House_Sink,
        House_Chair,
        House_Bed,
        //exterior objs
        House_Wall_FrontA,
        House_Wall_FrontB,
        House_Wall_Back,
        House_Wall_Side_Left,
        House_Wall_Side_Right,
        House_Door_Shut,
        House_Door_Open,
        //roofs
        House_Roof_Top,
        House_Roof_Bottom,
        House_Roof_Chimney,
        House_Roof_Collapsing, //meta obj for collapsing roofs

        #endregion






        #region Vendors & NPCs

        //Vendor NPCs
        Vendor_Items,
        Vendor_EnemyItems,

        Vendor_Potions,
        Vendor_Magic,
        Vendor_Weapons,
        Vendor_Armor,
        Vendor_Equipment,

        Vendor_Pets,

        NPC_Story,

        NPC_Farmer,
        NPC_Farmer_Reward,
        NPC_Farmer_EndDialog,

        Vendor_Colliseum_Mob,
        Judge_Colliseum,

        #endregion






        #region Colliseum Objects

        //shadow coliseum
        Coliseum_Shadow_Stairs_Left,
        Coliseum_Shadow_Stairs_Middle,
        Coliseum_Shadow_Stairs_Right,
        Coliseum_Shadow_Outdoors_Floor,
        Coliseum_Shadow_Gate_Center,
        Coliseum_Shadow_Gate_Pillar_Left,
        Coliseum_Shadow_Gate_Pillar_Right,

        #endregion


        #region Forest Objects

        //none

        #endregion


        #region Mountain Objects

        MountainWall_Top,
        MountainWall_Mid,
        MountainWall_Bottom,

        MountainWall_Foothold,
        MountainWall_Ladder,
        MountainWall_Ladder_Trap,

        #endregion


        #region Swamp Objects

        //none

        #endregion


        #region Boat Objects

        Boat_Captain_Brandy,

        Boat_Stairs_Left,
        Boat_Stairs_Right,

        Boat_Floor,
        Boat_Floor_Burned,

        Boat_Barrel,

        Boat_Stairs_Cover,

        Boat_Bridge_Top,
        Boat_Bridge_Bottom, 

        Boat_Pier_TopLeft,
        Boat_Pier_TopMiddle,
        Boat_Pier_TopRight,

        Boat_Pier_Left,
        Boat_Pier_Middle,
        Boat_Pier_Right,

        Boat_Pier_BottomLeft,
        Boat_Pier_BottomMiddle,
        Boat_Pier_BottomRight,

        #endregion







        #region Enemies/Pets/Living Objs

        Enemy_Turtle,
        Enemy_Crab,
        Enemy_Rat,

        Enemy_SeekerExploder, //seeks hero

        Pet_None,
        Pet_Dog,
        Pet_Chicken,

        #endregion


        #region Unique Objs

        ExplodingObject, //creates explosion

        #endregion






        #region Dialog Objs

        //consider just using a sprite + animation component to
        DialogObj_Hero_Idle, //model this better in dialog, just copy sprite+anim

        #endregion


    }







    







    #region ProjectileType 

    public enum ProjectileType
    {
        //Projectiles - items
        Bomb,
        Boomerang,

        Firerod,
        Fireball,

        Icerod,
        Iceball,
        Iceblock,
        IceblockCracking,

        //Projectiles - weapons
        Sword,
        Arrow,
        Net,
        Bow,
        Shovel,
        Hammer,
        Wand,

        //Projectiles - objects
        Explosion,
        GroundFire,
        
        //Emitters - create projectiles/particles per frame
        Emitter_Explosion,
        Emitter_GroundFire,
        Emitter_IceTile,

        //Projectiles - enemy related
        Bite,
        Bat,

        //Carried and Thrown Projectiles
        CarriedObject,
        ThrownObject,
    }

    #endregion


    







    #region Pickup + Particle Types

    public enum PickupType
    {
        Rupee,
        Heart,
        Magic,
        Arrow,
        Bomb,
    }

    public enum ParticleType
    {
        //8x8 - small generic
        RisingSmoke,
        ImpactDust,

        Sparkle,
        SparkleBlue,


        //8x8 small, obj specific
        PitBubble,
        //8x8 explosion specific
        LeafGreen,
        DebrisBrown,
        BloodRed,
        SlimeGreen,

        //8x8 map
        Map_Flag,
        Map_Wave,
        Map_Campfire,

        //16x16 size
        Attention,
        Splash,
        Blast,
        Push,
        Fire,
        WaterKick,
        ExclamationBubble,
        LightningBolt,


        //Particles - rewards
        RewardKey,
        RewardMap,
        RewardBottle,

        RewardMagicBombos,
        RewardMagicEther,
    }

    #endregion


    #region Magic Spells

    public enum SpellType
    {
        None,

        //wind
        Wind_Gust,
        Wind_Calm,
        Wind_Fury,
        Wind_Dir,
        Wind_Stop,

        //explosive/fire
        Fire,
        Fire_Walk,
        Explosive_Single,
        Explosive_Line,
        Explosive_Bombos,

        //ice
        Ice_FreezeWalk,
        Ice_FreezeGround,

        //electrical
        Lightning_Ether,

        //summons
        Summon_Bat_Projectile,
        Summon_Bat_Explosion,
    }

    #endregion


    #region MenuItemType

    public enum MenuItemType
    {
        Unknown,


        #region Options relate menuItems

        OptionsNewGame,
        OptionsSandBox,
        OptionsQuitGame,

        OptionsCheatMenu,
        OptionsOptionsMenu,

        OptionsAudioCtrls,
        OptionsInputCtrls,
        OptionsVideoCtrls,

        Options_DrawInput,
        Options_TrackCamera,
        Options_Watermark,
        Options_HardMode,
        Options_DrawBuildTimes,
        Options_PlayMusic,
        Options_DrawDebug,
        Options_DrawHitBoxes,
        Options_Gore,

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
        CheatsAutoSolvePuzzles,

        #endregion


        #region Spells

        //wind
        Wind_Gust,
        Wind_Calm,
        Wind_Fury,
        Wind_Dir,
        Wind_Stop,

        //explosive
        Fire,
        Fire_Walk,
        Explosive_Single,
        Explosive_Line,


        //ice
        Spells_Ice_FreezeWalk,
        Spells_Ice_FreezeGround,


        //electrical
        Spells_Lightning_Ether,


        //summons
        Spells_Summon_Bat_Explosion,


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
        ItemFirerod,
        ItemIcerod,
        ItemMagicMirror,
        ItemSpellbook,

        BottleEmpty,
        BottleHealth,
        BottleMagic,
        BottleCombo,
        BottleFairy,
        BottleBlob,
        
        MagicBombos,
        MagicEther,

        WeaponSword,
        WeaponNet,
        WeaponShovel,
        WeaponHammer,
        WeaponWand,

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

    #endregion




}