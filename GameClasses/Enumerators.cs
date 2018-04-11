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
    public enum Direction { Up, UpRight, Right, DownRight, Down, DownLeft, Left, UpLeft, None }

    public enum Rotation { None, Clockwise90, Clockwise180, Clockwise270 }

    public enum MouseButtons { LeftButton, RightButton }

    public enum BootRoutine { Game, RoomBuilder }

    public enum WidgetDisplaySet { World, Dungeon, None }



    public enum DisplayState { Opening, Opened, Closing, Closed }

    public enum FadeState { FadeIn, FadeComplete, FadeOut, Silent }

    public enum ObjToolState { MoveObj, RotateObj, AddObj, DeleteObj }

    public enum LoadSaveNewState { Load, Save, New }

    public enum GameFile { AutoSave, Game1, Game2, Game3 }

    public enum ExitAction { Title, Summary, ExitDungeon, Overworld, QuitGame, ExitScreen }



    public enum LevelType { Road, Castle, Shop }

    public enum Music { DungeonA, DungeonB, DungeonC, Boss, Title, None }

    public enum RoomType { Exit, Hub, Boss, Key, Shop, Column, Row, Square, Secret }

    public enum DoorType { Open, Boss, Bombable }

    public enum PuzzleType { None, Switch, Torches }



    public enum ActorType { Hero, Blob, Boss }

    public enum ActorState
    {
        Idle, Move, Dash, //movement
        Interact, Attack, Use, //action
        Hit, Dead, Reward, //consequence
        Pickup, Throw //actions++
    }

    public enum ActorAI { Random, Basic }



    public enum ObjGroup
    {
        Wall, Door, Chest, Object, Vendor, EnemySpawn, //roomObjs
        Pickup, Projectile, Particle //entities
    } 

    public enum ObjType
    {

        #region Dungeon Objects

        //PROCEDURALLY PLACED
        Dungeon_ExitPillarLeft, //ExitPillarLeft
        Dungeon_ExitPillarRight, //ExitPillarRight
        Dungeon_Exit, //Exit
        Dungeon_ExitLight, //ExitLightFX

        Dungeon_DoorOpen, //DoorOpen,
        Dungeon_DoorBombable, //DoorBombable,
        Dungeon_DoorBoss, //DoorBoss,
        Dungeon_DoorTrap, //DoorTrap,
        Dungeon_DoorShut, //DoorShut,
        Dungeon_DoorFake, //DoorFake,

        Dungeon_WallStraight,
        Dungeon_WallStraightCracked, 
        Dungeon_WallInteriorCorner, 
        Dungeon_WallExteriorCorner,
        Dungeon_WallPillar,
        Dungeon_WallStatue,
        Dungeon_WallTorch,

        Dungeon_FloorDecal, //BossDecal
        Dungeon_FloorBlood, //FloorDebrisBlood


        //HAND PLACED
        Dungeon_Pit, //PitAnimated,
        Dungeon_PitBridge, //Bridge,
        Dungeon_PitTeethTop, //PitTop,
        Dungeon_PitTeethBottom, //PitBottom,
        Dungeon_PitTrap, //PitTrap

        Dungeon_Statue, //BossStatue,


        Dungeon_Chest, //placeable in editor, becomes key or map chest
        Dungeon_ChestKey, //not placeable, procedurally set
        Dungeon_ChestMap, //not placeable, procedurally set
        Dungeon_ChestEmpty, //just empty


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
        Dungeon_SwitchOff,
        Dungeon_SwitchBlockBtn,
        Dungeon_SwitchBlockDown,
        Dungeon_SwitchBlockUp,
        Dungeon_TorchUnlit,
        Dungeon_TorchLit,

        //'Living' RoomObjs
        Dungeon_Fairy,

        //Actor Spawn Objects (hand-placed)
        Dungeon_SpawnMob, //SpawnEnemy1
        Dungeon_SpawnMiniBoss, //SpawnEnemy2

        #endregion


        #region World Objects

        //World Objects
        World_Bookcase, //Bookcase1
        World_Shelf, //Bookcase2
        World_TableStone,

        #endregion


        #region Vendors

        //Vendor NPCs
        Vendor_NPC_Items,
        Vendor_NPC_Potions,
        Vendor_NPC_Magic,
        Vendor_NPC_Weapons,
        Vendor_NPC_Armor,
        Vendor_NPC_Equipment,
        Vendor_NPC_Pets,
        Vendor_NPC_Story,

        #endregion


        #region Pickups, Projectiles, Particles

        //Pickups - collide with hero
        Pickup_Rupee,
        Pickup_Heart,
        Pickup_Magic,
        Pickup_Arrow,
        Pickup_Bomb,

        //Projectiles - items
        ProjectileBomb,
        ProjectileFireball,
        ProjectileBoomerang,

        //Projectiles - weapons
        ProjectileSword,
        ProjectileArrow,
        ProjectileNet,
        ProjectileBow,
        //Projectiles - object
        ProjectileExplosion,
        //ProjectileSpikeBlock, //this should probably be a roomObj
        ProjectileExplodingBarrel, //acceptable

        //Particles - small
        Particle_RisingSmoke, //ParticleDashPuff,
        Particle_ImpactDust, //ParticleSmokePuff,
        Particle_Sparkle, //ParticleHitSparkle,
        ParticlePitAnimation, 
        //Particles - map
        Particle_Map_Flag,
        Particle_Map_Wave,
        Particle_Map_Campfire,
        //Particles - normal size
        Particle_Explosion,
        Particle_Attention,
        Particle_FireGround, //Particle_Fire,
        Particle_Splash,
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

    }

    public enum MenuItemType
    {
        Unknown,

        //options
        OptionsContinue,

        OptionsNewGame,
        OptionsLoadGame,
        OptionsQuitGame,
        OptionsSaveGame,

        OptionsVideoCtrls,
        OptionsInputCtrls,
        OptionsAudioCtrls,
        OptionsGameCtrls,

        OptionsCheatMenu,



        //cheats
        CheatsInfiniteHP,
        CheatsInfiniteGold,
        CheatsInfiniteMagic,
        CheatsInfiniteArrows,
        CheatsInfiniteBombs,

        CheatsMap,
        CheatsKey,

        CheatsUnlockAll,


        //loadout menuItems
        InventoryGold,
        InventoryMap,
        InventoryKey,

        

        //vendor menuItems
        ItemHeart,
        ItemBomb3Pack,
        ItemArrowPack,

        //inventory menuItems
        ItemBoomerang,
        ItemBomb,

        BottleEmpty,
        BottleHealth,
        BottleMagic,
        BottleCombo,
        BottleFairy,
        BottleBlob,
        
        MagicFireball,

        WeaponSword,
        WeaponBow,
        WeaponNet, 

        ArmorCloth,
        ArmorCape,

        EquipmentRing,
        EquipmentPearl,
        EquipmentNecklace,
        EquipmentGlove,
        EquipmentPin,

        PetStinkyDog,
        PetChicken
    }

}