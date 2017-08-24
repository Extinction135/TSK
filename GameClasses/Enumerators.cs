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



    public enum DisplayState { Opening, Opened, Closing, Closed }

    public enum FadeState { FadeIn, FadeComplete, FadeOut, Silent }

    public enum ObjToolState { MoveObj, RotateObj, AddObj, DeleteObj }

    public enum LoadSaveNewState { Load, Save, New }

    public enum GameFile { AutoSave, Game1, Game2, Game3 }

    public enum TitleText { Dungeon, Complete, You, Died, Run }

    public enum ExitAction { Title, Summary, Overworld, QuitGame, ExitScreen }



    public enum LevelType { Road, Castle, Shop }

    public enum Music { DungeonA, DungeonB, DungeonC, Boss, Title, None }

    public enum RoomType { Exit, Hub, Boss, Key, Shop, Column, Row, Square, Secret }

    public enum DoorType { Open, Boss, Bombable, Bombed }

    

    public enum ActorType { Hero, Blob, Boss, }

    public enum ActorState { Idle, Move, Dash, Interact, Attack, Use, Hit, Dead, Reward }

    public enum ObjGroup { Wall, Door, Chest, Object, Liftable, Draggable, Pickup, Projectile, Particle, Vendor, EnemySpawn }

    public enum ObjType
    {
        //ROOM OBJECTS
        //Non-Editor Room Objects (procedurally added)
        Exit, ExitPillarLeft, ExitPillarRight, ExitLightFX,
        DoorOpen, DoorBombable, DoorBombed, DoorBoss, DoorTrap, DoorShut, DoorFake,
        WallStraight, WallStraightCracked, WallInteriorCorner, WallExteriorCorner, WallPillar, WallStatue, WallTorch,
        BossDecal, DebrisFloor,
        //Editor Room Objects (hand-placed)
        PitAnimated, PitTop, PitBottom, PitTrapReady, PitTrapOpening,
        BossStatue, Pillar,
        ChestKey, ChestMap, ChestEmpty,
        BlockDraggable, BlockDark, BlockLight,
        //lever + lever activated objects
        LeverOn, LeverOff,
        SpikesFloorOn, SpikesFloorOff,
        ConveyorBeltOn, ConveyorBeltOff,
        //
        PotSkull, Bumper, Flamethrower, Switch, Bridge,
        SwitchBlockBtn, SwitchBlockDown, SwitchBlockUp,
        TorchUnlit, TorchLit, 
        //Enemy Spawn Objects (hand-placed)
        SpawnEnemy1, SpawnEnemy2, SpawnEnemy3, //4 and 5 to be added later
        //Vendor & Story Objects
        VendorItems, VendorPotions, VendorMagic, VendorWeapons, VendorArmor, VendorEquipment,
        VendorAdvertisement,
        VendorStory,

        //ENTITIES
        //Pickups - collide with hero
        PickupRupee, PickupHeart, PickupMagic, PickupArrow, PickupBomb, 
        //Projectiles - collide with actors
        ProjectileSword, ProjectileFireball, ProjectileBomb, ProjectileExplosion, ProjectileArrow,
        ProjectileSpikeBlock,
        //Particles - small
        ParticleDashPuff, ParticleSmokePuff, ParticleHitSparkle,
        //Particles - map
        ParticleMapFlag, ParticleMapWave, ParticleMapCampfire,
        //

        //Particles - normal size
        ParticleExplosion, ParticleAttention, ParticleFire, ParticleFairy,
        ParticleBow,
        ParticleBottleEmpty, ParticleBottleHealth, ParticleBottleMagic, ParticleBottleFairy,
        //Particles - rewards
        ParticleRewardKey, ParticleRewardMap,
    }

    public enum MenuItemType
    {
        Unknown,
        InventoryGold, InventoryMap, InventoryKey,
        StatsHealth, StatsMagic, StatsAgility, Stats4,
        CrystalFilled, CrystalEmpty,

        OptionsContinue, OptionsNewGame, OptionsLoadGame, OptionsQuitGame,
        OptionsVideoCtrls, OptionsInputCtrls, OptionsAudioCtrls, OptionsGameCtrls,
        OptionsSaveGame, OptionsHelpInfo,

        ItemHeart, ItemBomb, ItemBomb3Pack, ItemArrowPack,
        ItemBoomerang,

        BottleEmpty, BottleHealth, BottleMagic, BottleFairy,

        MagicFireball,
        WeaponSword, WeaponBow, WeaponStaff, //WeaponAxe, //WeaponNet,
        ArmorCloth, ArmorChest, ArmorCape, ArmorRobe, //Armor5,
        EquipmentRing, EquipmentPearl, EquipmentNecklace, EquipmentGlove, EquipmentPin,
    }

    public enum Dialog
    {
        Default, GameSaved, GameLoaded, GameAutoSaved,
        GameCreated, GameNotFound, GameLoadFailed,
        //Intro, Dungeon1, Dungeon2 //etc...
        DoesNotHaveKey,
        HeroGotKey, HeroGotMap
    }
}