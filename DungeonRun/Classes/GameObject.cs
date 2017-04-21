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
    public class GameObject
    {
        public enum ObjGroup
        {
            Wall,       //usually blocks hero from passing
            Door,       //might block/change type upon collision
            Chest,      //object that hero can 'open' for a reward

            Object,     //standard object, might collide or interact with hero
            Liftable,   //can the hero pick this object up, carry it, and throw it?
            Draggable,  //can the hero push, pull, or drag this object?

            Item,       //picked up off ground, held above hero's head
            Projectile, //a colliding object (moving or stationary)
            Particle    //doesn't collide, direction always faces down, used as a visual cue
                        //the rewards from chests are particles
            //Inspectable,//hero can 'read' or 'inspect' with this obj
        }

        public enum Type
        {

            #region Room Objects

            Exit,
            ExitPillarLeft,
            ExitPillarRight,
            ExitLightFX,    //overlaid on top of DoorExit, looks like light shines ontop of hero

            DoorOpen,
            DoorBombable,   //a wall that looks like it's cracked
            DoorBombed,
            DoorBoss,
            DoorTrap,       //an open door waiting for hero to stop colliding with it - becomes shut door
            DoorShut,       //a closed door sprite
            DoorFake,       //a wall tile that looks like a shut door, not actually connected to other rooms

            WallStraight,
            WallStraightCracked,
            WallInteriorCorner,
            WallExteriorCorner,
            WallPillar,
            WallDecoration,

            PitTop,         //top teeth
            PitBottom,      //bottom teeth
            PitTrapReady,   //a cracked floor sprite, ready for hero to step on it
            PitTrapOpening, //a cracked floor sprite, waiting for hero to step off of it - becomes a pitTopBottom

            BossStatue,
            BossDecal,      //the graphic in front of the boss door
            Pillar,         //pillars placed around doors/walls
            WallTorch,      //torches placed around doors/walls

            #endregion


            #region Interactive Objects

            ChestGold,
            ChestKey,
            ChestMap,
            ChestEmpty,

            BlockDraggable, //blocking, mobile
            BlockDark,      //blocking, immobile
            BlockLight,     //blocking, immobile
            BlockSpikes,

            Lever,          //used to cue room events
            PotSkull,       //hero can pick and throw pot skulls

            SpikesFloor,    //animated spikes popping in/out of floor
            Bumper,         //pushes hero upon collision
            Flamethrower,   //shoots fireballs towards the hero
            Switch,         //used to cue room events
            Bridge,         //made to fit between pit objects, allows passage over pits

            SwitchBlockBtn, //toggles switch blocks up/down
            SwitchBlockDown,//nonblocking
            SwitchBlockUp,  //blocking

            TorchUnlit,
            TorchLit,
            ConveyorBelt,   //moves hero upon collision

            #endregion


            #region Items

            //items can be picked up by Hero
            ItemRupee,      //increases gold +1
            ItemHeart,      //increases current HP +1

            #endregion


            #region Projectiles

            ProjectileSword,

            #endregion


            #region Particles

            ParticleDashPuff,
            ParticleExplosion,
            ParticleSmokePuff,
            ParticleHitSparkle,

            ParticleReward50Gold,

            #endregion

        }

        public ObjGroup objGroup;
        public Type type;

        public ComponentSprite compSprite;
        public ComponentCollision compCollision;
        public ComponentAnimation compAnim;
        public ComponentMovement compMove;

        public Direction direction;
        public Boolean active; //does this object draw, update?

        public Byte lifetime;   //how many frames this object exists for, 0 = forever/ignore
        public Byte lifeCounter;//counts up to lifetime value

        public GameObject(Texture2D DungeonSheet)
        {   //initialize to default value - this data is changed in Update()
            objGroup = ObjGroup.Object;
            type = Type.WallStraight;
            compSprite = new ComponentSprite(DungeonSheet, new Vector2(50, 50), new Byte4(0, 0, 0, 0), new Byte2(16, 16));
            compCollision = new ComponentCollision();
            compAnim = new ComponentAnimation();
            compMove = new ComponentMovement();
            direction = Direction.Down;
            active = true;
            GameObjectFunctions.SetType(this, type);
        }
    }
}