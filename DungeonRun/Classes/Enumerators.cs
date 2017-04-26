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

    public enum DisplayState { Opening, Opened, Closing, Closed }

    public enum Music { DungeonA, DungeonB, Boss, Overworld, Shop, Title, None }

    public enum FadeState { FadeIn, FadeComplete, FadeOut, Silent }

    public enum RoomType { Normal, Key, Boss, Hub, Exit, Secret }

    public enum ActorType { Hero, Blob, Boss, }

    public enum ActorState { Idle, Move, Dash, Interact, Attack, Use, Hit, Dead, Reward }

    public enum ObjGroup { Wall, Door, Chest, Object, Liftable, Draggable, Item, Projectile, Particle }

    public enum ObjType
    {
        //Room Objects
        Exit, ExitPillarLeft, ExitPillarRight, ExitLightFX,
        DoorOpen, DoorBombable, DoorBombed, DoorBoss, DoorTrap, DoorShut, DoorFake,
        WallStraight, WallStraightCracked, WallInteriorCorner, WallExteriorCorner, WallPillar, WallDecoration,
        PitTop, PitBottom, PitTrapReady, PitTrapOpening,
        BossStatue, BossDecal, Pillar, WallTorch, DebrisFloor,
        //Interactive Objects
        ChestGold, ChestKey, ChestMap, ChestEmpty,
        BlockDraggable, BlockDark, BlockLight, BlockSpikes,
        Lever, PotSkull, SpikesFloor, Bumper, Flamethrower, Switch, Bridge,
        SwitchBlockBtn, SwitchBlockDown, SwitchBlockUp,
        TorchUnlit, TorchLit, ConveyorBelt,
        //Items - picked up by hero
        ItemRupee, ItemHeart,
        //Projectiles
        ProjectileSword,
        //Particles
        ParticleDashPuff, ParticleExplosion, ParticleSmokePuff, ParticleHitSparkle,
        ParticleReward50Gold, ParticleRewardKey, ParticleRewardMap,
    }

    public enum MenuItemType
    {
        Unknown,
        InventoryGold, InventoryHeartPieces, InventoryMap, InventoryKey,
        StatsHealth, StatsMagic, StatsAgility, Stats4,
        OptionsSaveGame, OptionsLoadGame, OptionsVideoCtrls, OptionsInputCtrls, OptionsAudioCtrls,

        ItemBoomerang, ItemBomb, 
        BottleEmpty, BottleHealth, //BottleSpecial, BottleFairy,
        MagicFireball,
        WeaponSword, WeaponBow, //WeaponStaff, WeaponAxe, WeaponNet,
        ArmorCloth, //ArmorChest, ArmorCape, ArmorRobe, Armor5,
        EquipmentRing, //EquipmentPearl, EquipmentNecklace, EquipmentGlove, EquipmentPin,
    }

    //this kinda duplicates the MenuItemType enum, and we could that instead...
    public enum Weapon { Sword }

}