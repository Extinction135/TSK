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
            Inspectable,//hero can 'read' or 'inspect' with this obj
            NPC,        //obj is an NPC hero can talk with

            Object,     //standard object, might collide or interact with hero
            Liftable,   //can the hero pick this object up, carry it, and throw it?
            Draggable,  //can the hero push, pull, or drag this object?

            Item,       //picked up off ground, deleted from objects list, held above hero's head
            Consumable, //picked up off ground, deleted from objects list, not held above hero's head
            Reward,     //spawned from chest, not actually on objects list, held above hero's head
        }
        public ObjGroup objGroup;





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

            Chest,
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


            //items can be picked up by Hero
            ItemRupee,      //increases gold +1
            ItemHeart,      //increases current HP +1
            ItemHeartPiece, //increases maxHP +0.25
            ItemGold50,     //increases gold +50
            ItemMap,        //unhides the dungeon's rooms on map
            ItemBigKey,     //unlocks the boss room
        }
        public Type type;












        public ComponentSprite compSprite;
        public ComponentCollision compCollision;
        public ComponentAnimation compAnim;
        public Direction direction;
        public Boolean active; //does this object draw, update?

        public GameObject(SpriteBatch SpriteBatch, Texture2D DungeonSheet)
        {   //initialize to default value - this data is changed in Update()
            objGroup = ObjGroup.Object;
            type = Type.WallTorch;
            compSprite = new ComponentSprite(SpriteBatch, DungeonSheet, new Vector2(50, 50), new Byte4(0, 0, 0, 0), new Byte2(16, 16));
            compCollision = new ComponentCollision(0, 0, 0, 0, false);
            compAnim = new ComponentAnimation(compSprite);
            direction = Direction.Down;
            active = true;
            Update();
        }


        public void Update()
        {
            //all gameobjects exist on the dungeon sheet, so the texture never needs to be changed

            //sprites are created facing Down, but we will need to set the spite rotation based on direction
            compSprite.rotation = Rotation.None; //reset sprite rotation to default DOWN
            if (direction == Direction.Up) { compSprite.rotation = Rotation.Clockwise180; }
            else if (direction == Direction.Right) { compSprite.rotation = Rotation.Clockwise90; }
            else if (direction == Direction.Left) { compSprite.rotation = Rotation.Clockwise270; }

            //update the object's current animation based on it's type
            GameObjectAnimListManager.SetAnimationList(this);

            //assume cell size is 16x16 (most are)
            compSprite.cellSize.x = 16 * 1; compSprite.cellSize.y = 16 * 1;
            compCollision.blocking = true; //assume the object is blocking (most are)
            objGroup = ObjGroup.Object; //assume object is a generic object
            compCollision.rec.Width = 16; //assume collisionRec is 16x16
            compCollision.rec.Height = 16; //(most are)
            compCollision.offsetX = -8; //assume collisionRec offset is -8x-8
            compCollision.offsetY = -8; //(most are)
            compSprite.zOffset = 0;

            //we'll need to apply the compColl.offset value to the collisionRec
            //we'll need to do that because the object may be moving
            //that should only happen if an object moves (dragging block, skull pot, spike block)
            //we'll set the gameObj.sprite.zDepth elsewhere - not here




            #region Room Objects

            if (type == Type.Exit)
            {
                compSprite.cellSize.y = 16 * 3; //nonstandard size
                objGroup = ObjGroup.Door;
                compCollision.rec.Height = 2;
                compCollision.offsetY = 32 + 6;
                compCollision.blocking = false;
                //sorts to floor layer (or has very positive zDepth)
            }
            else if (type == Type.ExitPillarLeft || type == Type.ExitPillarRight)
            {
                compSprite.cellSize.y = 16 * 3; //nonstandard size
                compCollision.rec.Height = 32 - 5;
                compCollision.offsetY = 14;
            }
            else if (type == Type.ExitLightFX)
            {
                compSprite.cellSize.y = 16 * 2; //nonstandard size
                compCollision.offsetY = 0;
                //sorts to roof layer (or has very negative zDepth)
            }


            else if (type == Type.DoorOpen || type == Type.DoorBombed || type == Type.DoorTrap)
            {
                compCollision.blocking = false;
                if (direction == Direction.Down) { compSprite.zOffset = 4; } else { compSprite.zOffset = 16; }
                objGroup = ObjGroup.Door;
            }
            else if (type == Type.DoorBombable || type == Type.DoorBoss || type == Type.DoorShut || type == Type.DoorFake)
            {
                objGroup = ObjGroup.Door;
            }


            else if (type == Type.WallStraight || type == Type.WallStraightCracked || type == Type.WallInteriorCorner ||
                type == Type.WallExteriorCorner || type == Type.WallPillar || type == Type.WallDecoration)
            {
                objGroup = ObjGroup.Wall;
            }


            else if (type == Type.PitTop)
            {
                //pits dont collide with actors
            }
            else if (type == Type.PitBottom)
            {
                //instead we'll use an animated liquid obj for collision checking
                //pits just sit ontop of this object as decoration
            }
            else if (type == Type.PitTrapReady || type == Type.PitTrapOpening)
            {
                compCollision.offsetX = -6; compCollision.offsetY = -6;
            }


            else if (type == Type.BossStatue)
            {
                objGroup = ObjGroup.Draggable;
                compCollision.rec.Height = 8;
                compCollision.offsetY = -1;
            }
            else if (type == Type.BossDecal)
            {
                compSprite.zOffset = -32;
                compCollision.blocking = false;
            }
            else if (type == Type.Pillar)
            {
                compSprite.zOffset = 2;
            }
            else if (type == Type.WallTorch)
            {
                compCollision.blocking = false;
            }

            #endregion


            #region Interactive Objects

            else if (type == Type.Chest || type == Type.ChestEmpty)
            {
                compCollision.offsetX = -7; compCollision.offsetY = -3;
                compCollision.rec.Width = 14; compCollision.rec.Height = 11;
                compSprite.zOffset = -7;
            }


            else if (type == Type.BlockDraggable)
            {
                compCollision.rec.Height = 12;
                compCollision.offsetY = -4;
                compSprite.zOffset = -7;
                objGroup = ObjGroup.Draggable;
            }
            else if (type == Type.BlockDark || type == Type.BlockLight)
            {
                compSprite.zOffset = -7;
            }
            else if (type == Type.BlockSpikes)
            {
                compCollision.offsetX = -7; compCollision.offsetY = -7;
                compCollision.rec.Width = 14; compCollision.rec.Height = 14;
                compSprite.zOffset = -7;
                compCollision.blocking = false;
            }

            else if (type == Type.Lever)
            {
                compCollision.offsetX = -6; compCollision.offsetY = 1;
                compCollision.rec.Width = 12; compCollision.rec.Height = 3;
                compSprite.zOffset = -7;
            }
            else if (type == Type.PotSkull)
            {
                compCollision.offsetX = -5; compCollision.offsetY = -4;
                compCollision.rec.Width = 10; compCollision.rec.Height = 12;
                compSprite.zOffset = -7;
                objGroup = ObjGroup.Liftable;
            }


            else if (type == Type.SpikesFloor || type == Type.Flamethrower || type == Type.Switch || type == Type.Bridge)
            {
                compSprite.zOffset = -32;
                compCollision.blocking = false;
            }
            else if (type == Type.Bumper)
            {
                compCollision.blocking = false;
            }


            else if (type == Type.SwitchBlockBtn)
            {
                compCollision.offsetX = -5; compCollision.offsetY = -4;
                compCollision.rec.Width = 10; compCollision.rec.Height = 12;
                compSprite.zOffset = -7;
            }
            else if (type == Type.SwitchBlockDown)
            {
                compSprite.zOffset = -16;
                compCollision.blocking = false;
            }
            else if (type == Type.SwitchBlockUp)
            {
                compCollision.offsetX = -7; compCollision.offsetY = -7;
                compCollision.rec.Width = 14; compCollision.rec.Height = 14;
                compSprite.zOffset = -7;
            }


            else if (type == Type.TorchUnlit || type == Type.TorchLit)
            {
                compCollision.offsetX = -7; compCollision.offsetY = -4;
                compCollision.rec.Width = 14; compCollision.rec.Height = 12;
                compSprite.zOffset = -7;
            }
            else if (type == Type.ConveyorBelt)
            {
                compSprite.zOffset = -32;
                compCollision.blocking = false;
                //directions are slightly different for this obj
                if (direction == Direction.Right) { compSprite.rotation = Rotation.Clockwise270; }
                else if (direction == Direction.Left) { compSprite.rotation = Rotation.Clockwise90; }
            }

            #endregion


            else if (type == Type.ItemRupee)
            {
                compSprite.cellSize.x = 8; //non standard cellsize
                compCollision.offsetX = -4; compCollision.offsetY = -5;
                compCollision.rec.Width = 8; compCollision.rec.Height = 10;
                compCollision.blocking = false;
                objGroup = ObjGroup.Item;
            }
            else if (type == Type.ItemHeart)
            {
                compSprite.cellSize.x = 8; //non standard cellsize
                compCollision.offsetX = -4; compCollision.offsetY = -3;
                compCollision.rec.Width = 8; compCollision.rec.Height = 7;
                compCollision.blocking = false;
                objGroup = ObjGroup.Item;
            }
            else if (type == Type.ItemMap || type == Type.ItemBigKey)
            {
                compCollision.offsetX = -5; compCollision.offsetY = -4;
                compCollision.rec.Width = 10; compCollision.rec.Height = 12;
                compSprite.zOffset = -7;
                compCollision.blocking = false;
                objGroup = ObjGroup.Item;
            }
            else if (type == Type.ItemHeartPiece || type == Type.ItemGold50)
            {
                compCollision.blocking = false;
                objGroup = ObjGroup.Reward;
            }
        }
    }
}