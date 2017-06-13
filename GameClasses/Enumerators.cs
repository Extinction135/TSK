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

    public enum Direction { Up, UpRight, Right, DownRight, Down, DownLeft, Left, UpLeft, None }

    public enum Rotation { None, Clockwise90, Clockwise180, Clockwise270 }

    public enum MouseButtons { LeftButton, RightButton }

    public enum DisplayState { Opening, Opened, Closing, Closed }

    public enum ExitAction { None, Summary, Overworld }

    public enum Music { DungeonA, DungeonB, DungeonC, Boss, Title, None }

    public enum FadeState { FadeIn, FadeComplete, FadeOut, Silent }

    public enum RoomType { Exit, Hub, Boss, Shop, Dev } //Normal, Key, Hub, Secret

    public enum DungeonType { CursedCastle, Shop }

    public enum ActorType { Hero, Blob, Boss, }

    public enum ActorState { Idle, Move, Dash, Interact, Attack, Use, Hit, Dead, Reward }

    public enum ObjGroup { Wall, Door, Chest, Object, Liftable, Draggable, Pickup, Projectile, Particle, Vendor }

    public enum ObjType
    {
        //Room Objects
        Exit, ExitPillarLeft, ExitPillarRight, ExitLightFX,
        DoorOpen, DoorBombable, DoorBombed, DoorBoss, DoorTrap, DoorShut, DoorFake,
        WallStraight, WallStraightCracked, WallInteriorCorner, WallExteriorCorner, WallPillar, WallStatue,
        PitTop, PitBottom, PitTrapReady, PitTrapOpening,
        BossStatue, BossDecal, Pillar, WallTorch, DebrisFloor,

        //Interactive Objects - dungeon
        ChestGold, ChestKey, ChestMap, ChestHeartPiece, ChestEmpty,
        BlockDraggable, BlockDark, BlockLight, BlockSpikes,
        Lever, PotSkull, SpikesFloor, Bumper, Flamethrower, Switch, Bridge,
        SwitchBlockBtn, SwitchBlockDown, SwitchBlockUp,
        TorchUnlit, TorchLit, ConveyorBelt,

        //Interactive Objects - shop
        VendorItems, VendorPotions, VendorMagic, VendorWeapons, VendorArmor, VendorEquipment, VendorStory,
        VendorAdvertisement,

        //Pickups - collide with hero
        PickupRupee, PickupHeart, PickupMagic, PickupArrow, PickupBomb, 

        //Projectiles - collide with actors
        ProjectileSword, ProjectileFireball, ProjectileBomb, ProjectileExplosion, ProjectileArrow,

        //Particles - small
        ParticleDashPuff, ParticleSmokePuff, ParticleHitSparkle,
        //Particles - normal size
        ParticleExplosion, ParticleAttention, ParticleFire, ParticleFairy,
        ParticleBow,

        ParticleHealthPotion, ParticleMagicPotion, ParticleFairyBottle,

        //Particles - rewards
        ParticleRewardGold, ParticleRewardKey, ParticleRewardMap,
        ParticleRewardHeartPiece, ParticleRewardHeartFull,

    }

    public enum MenuItemType
    {
        Unknown,
        InventoryGold, InventoryHeartPieces, InventoryMap, InventoryKey,
        StatsHealth, StatsMagic, StatsAgility, Stats4,
        OptionsSaveGame, OptionsLoadGame, OptionsVideoCtrls, OptionsInputCtrls, OptionsAudioCtrls,

        ItemBoomerang, ItemBomb, ItemBomb3Pack, ItemArrowPack,

        BottleEmpty, BottleHealth, BottleMagic, BottleFairy,

        MagicFireball,
        WeaponSword, WeaponBow, WeaponStaff, //WeaponAxe, //WeaponNet,
        ArmorCloth, ArmorChest, ArmorCape, ArmorRobe, //Armor5,
        EquipmentRing, EquipmentPearl, EquipmentNecklace, EquipmentGlove, EquipmentPin,
    }

}