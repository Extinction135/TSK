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

    public enum ExitAction { Title, Summary, ExitDungeon, Overworld, QuitGame, ExitScreen }



    public enum LevelType { Road, Castle, Shop }

    public enum Music { DungeonA, DungeonB, DungeonC, Boss, Title, None }

    public enum RoomType { Exit, Hub, Boss, Key, Shop, Column, Row, Square, Secret }

    public enum DoorType { Open, Boss, Bombable }

    public enum PuzzleType { None, Switch, Torches }



    public enum ActorType { Hero, Blob, Boss, Pet }

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
        //Non-Editor Room Objects (procedurally added)
        Exit, ExitPillarLeft, ExitPillarRight, ExitLightFX,
        DoorOpen, DoorBombable, DoorBoss, DoorTrap, DoorShut, DoorFake,
        WallStraight, WallStraightCracked, WallInteriorCorner, WallExteriorCorner, WallPillar, WallStatue, WallTorch,
        BossDecal, FloorDebrisBlood,
        
        
        
        //Editor Room Objects (hand-placed)
        PitAnimated, Bridge, PitTop, PitBottom, PitTrap, 
        BossStatue, Pillar,
        ChestKey, ChestMap, ChestEmpty,
        BlockDark, BlockLight,
        //lever + lever activated objects
        LeverOn, LeverOff,
        SpikesFloorOn, SpikesFloorOff,
        ConveyorBeltOn, ConveyorBeltOff,
        //Room Objects
        Pot, Barrel,
        Bumper, Flamethrower, IceTile,
        Switch, SwitchOff,
        SwitchBlockBtn, SwitchBlockDown, SwitchBlockUp,
        TorchUnlit, TorchLit, 
        //Unique RoomObjs
        Fairy,
        //Actor Spawn Objects (hand-placed)
        SpawnEnemy1, SpawnEnemy2, SpawnEnemy3, 
        //Shop Object
        Bookcase1, Bookcase2, TableStone,
        VendorAdvertisement,
        //Vendors
        VendorItems, VendorPotions, VendorMagic, VendorWeapons,
        VendorArmor, VendorEquipment, VendorPets, VendorStory,



        //Pickups - collide with hero
        PickupRupee, PickupHeart, PickupMagic, PickupArrow, PickupBomb,
        


        //Projectiles - items
        ProjectileBomb, ProjectileFireball,
        //Projectiles - weapons
        ProjectileSword, ProjectileArrow, ProjectileNet,
        //Projectiles - object
        ProjectileExplosion,
        ProjectileSpikeBlock, ProjectileDebrisRock,
        ProjectilePot, ProjectileExplodingBarrel,
        ProjectileShadowSm,
        


        //Particles - small
        ParticleDashPuff, ParticleSmokePuff, ParticleHitSparkle, ParticlePitAnimation, 
        //Particles - map
        ParticleMapFlag, ParticleMapWave, ParticleMapCampfire,
        //Particles - normal size
        ParticleExplosion, ParticleAttention, ParticleFire, ParticleSplash,
        ParticleBow,
        ParticleBottleEmpty, ParticleBottleHealth, ParticleBottleMagic, ParticleBottleCombo,
        ParticleBottleFairy, ParticleBottleBlob,
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
        
        BottleEmpty, BottleHealth, BottleMagic, BottleCombo,
        BottleFairy, BottleBlob,
        
        MagicFireball,
        WeaponSword, WeaponBow, WeaponNet, //WeaponStaff, //WeaponAxe
        ArmorCloth, ArmorChest, ArmorCape, ArmorRobe, //Armor5,
        EquipmentRing, EquipmentPearl, EquipmentNecklace, EquipmentGlove, EquipmentPin,
        PetStinkyDog, PetChicken
    }

    public enum SpeakerType
    {
        Guide, Hero, Blob,
        VendorItems, VendorPotions, VendorMagic, VendorWeapons,
        VendorArmor, VendorEquipment, VendorPets, 
    }

    public enum BottleContent
    {
        NotOwned, Empty, 
        Health, Magic, Combo, Fairy,
        Blob 
    }

}