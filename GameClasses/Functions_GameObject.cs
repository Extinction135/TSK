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
    public static class Functions_GameObject
    {

        public static GameObject Spawn(ObjType Type, float X, float Y, Direction Direction)
        {   //spawns obj at the X, Y location, with direction
            GameObject obj = Functions_Pool.GetRoomObj();
            obj.direction = Direction;
            obj.compMove.direction = Direction;
            Functions_Movement.Teleport(obj.compMove, X, Y);
            SetType(obj, Type);
            return obj;
        }

        public static void HandleCommon(GameObject RoomObj, Direction HitDirection)
        {
            //hitDirection is used to push some objects in the direction they were hit
            
            if (RoomObj.type == ObjType.Dungeon_Pot)
            {
                RoomObj.compMove.direction = HitDirection;
                Kill(RoomObj, true, true);
            }
            else if (RoomObj.type == ObjType.Wor_Pot)
            {
                RoomObj.compMove.direction = HitDirection;
                Kill(RoomObj, true, true);
            }
            else if(RoomObj.type == ObjType.Wor_Bush)
            {
                RoomObj.compMove.direction = HitDirection; 
                Functions_GameObject_World.DestroyBush(RoomObj);
            }
            else if (RoomObj.type == ObjType.Dungeon_Barrel)
            {
                RoomObj.compMove.direction = HitDirection; 
                Functions_GameObject_Dungeon.DestroyBarrel(RoomObj);
            }
            else if (RoomObj.type == ObjType.Dungeon_SwitchBlockBtn)
            {
                Functions_GameObject_Dungeon.FlipSwitchBlocks(RoomObj);
            }
            else if (RoomObj.type == ObjType.Dungeon_LeverOff
                || RoomObj.type == ObjType.Dungeon_LeverOn)
            {
                Functions_GameObject_Dungeon.ActivateLeverObjects();
            }
        }
        
        public static void Kill(GameObject Obj, Boolean spawnLoot, Boolean becomeDebris)
        {   //pop an attention particle
            Functions_Particle.Spawn(
                ObjType.Particle_Attention,
                Obj.compSprite.position.X,
                Obj.compSprite.position.Y);

            //pop loot & soundfx
            if (spawnLoot) { Functions_Loot.SpawnLoot(Obj.compSprite.position); }
            if (Obj.sfx.kill != null) { Assets.Play(Obj.sfx.kill); }

            if (becomeDebris) //should obj become debris or get released?
            {   //if obj becomes debris, explode debris
                Functions_Particle.Spawn_Explosion(
                    ObjType.Particle_Debris,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y, 
                    true);
                //become debris
                SetType(Obj, ObjType.Wor_Debris);
            }
            else { Functions_Pool.Release(Obj); }
        }

        public static void AlignRoomObjs()
        {   //align sprite + collision comps to move comp of all active objs
            for (Pool.roomObjCounter = 0; Pool.roomObjCounter < Pool.roomObjCount; Pool.roomObjCounter++)
            {
                if (Pool.roomObjPool[Pool.roomObjCounter].active)
                {   //align the sprite and collision components to the move component
                    Functions_Component.Align(
                        Pool.roomObjPool[Pool.roomObjCounter].compMove,
                        Pool.roomObjPool[Pool.roomObjCounter].compSprite,
                        Pool.roomObjPool[Pool.roomObjCounter].compCollision);
                    //set the current animation frame, check the animation counter
                    Functions_Animation.Animate(Pool.roomObjPool[Pool.roomObjCounter].compAnim,
                        Pool.roomObjPool[Pool.roomObjCounter].compSprite);
                    //set the rotation for the obj's sprite
                    SetRotation(Pool.roomObjPool[Pool.roomObjCounter]);
                }
            }
        }

        public static void ResetObject(GameObject Obj)
        {
            //reset the obj
            Obj.group = ObjGroup.Object; //assume object is a generic object
            Obj.type = ObjType.Dungeon_WallStraight; //reset the type
            Obj.direction = Direction.Down;

            Obj.active = true; //assume this object should draw / animate
            Obj.getsAI = false; //most objects do not get any AI input
            Obj.canBeSaved = false; //most objects cannot be saved as XML data
            Obj.lifetime = 0; //assume obj exists forever (not projectile)
            Obj.lifeCounter = 0; //reset counter
            Obj.interactiveFrame = 0;

            //reset the sprite component
            Obj.compSprite.cellSize.X = 16 * 1; //assume cell size is 16x16 (most are)
            Obj.compSprite.cellSize.Y = 16 * 1;
            Obj.compSprite.zOffset = 0;
            Obj.compSprite.flipHorizontally = false;
            Obj.compSprite.rotation = Rotation.None;
            Obj.compSprite.scale = 1.0f;
            //reset the animation component
            Obj.compAnim.speed = 10; //set obj's animation speed to default value
            Obj.compAnim.loop = true; //assume obj's animation loops
            Obj.compAnim.index = 0; //reset the current animation index/frame
            Obj.compAnim.timer = 0; //reset the elapsed frames
            //reset the collision component
            Obj.compCollision.blocking = true; //assume the object is blocking (most are)
            Obj.compCollision.rec.Width = 16; //assume collisionRec is 16x16
            Obj.compCollision.rec.Height = 16; //(most are)
            Obj.compCollision.offsetX = -8; //assume collisionRec offset is -8x-8
            Obj.compCollision.offsetY = -8; //(most are)
            //reset the move component
            Obj.compMove.magnitude.X = 0; //discard any previous magnitude
            Obj.compMove.magnitude.Y = 0; //
            Obj.compMove.speed = 0.0f; //assume this object doesn't move
            Obj.compMove.friction = 0.75f; //normal friction
            Obj.compMove.moveable = false; //most objects cant be moved
            Obj.compMove.grounded = true; //most objects exist on the ground
            //reset the sfx component
            Obj.sfx.hit = null;
            Obj.sfx.kill = null;
            
        }

        public static void SetRotation(GameObject Obj)
        {   
            
            //we could split this out into pro/pick/part SetRotations()
            //but there isn't enough complexity to warrant that split, yet

            
            //handle object/projectile specific cases
            if (Obj.type == ObjType.ProjectileSword 
                || Obj.type == ObjType.ProjectileNet
                || Obj.type == ObjType.ProjectileShovel)
            {   //some projectiles flip based on their direction
                if (Obj.direction == Direction.Down || Obj.direction == Direction.Left)
                { Obj.compSprite.flipHorizontally = true; }
            }
            else if (Obj.type == ObjType.ProjectileBomb
                || Obj.type == ObjType.ProjectileExplodingBarrel
                || Obj.type == ObjType.ProjectilePot
                || Obj.type == ObjType.ProjectilePotSkull
                || Obj.type == ObjType.ProjectileBush)
            {   //some objects only face Direction.Down
                Obj.direction = Direction.Down;
            }
            else if(Obj.type == ObjType.Dungeon_PitTrap)
            {   //some objects are randomly rotated
                Obj.direction = Functions_Direction.GetRandomCardinal();
            }
            else if(Obj.type == ObjType.Dungeon_FloorBlood)
            {   //some objects are randomly flipped horizontally
                Obj.compSprite.flipHorizontally = true;
            }
            //set sprite's rotation based on direction & flipHorizontally boolean
            Functions_Component.SetSpriteRotation(Obj.compSprite, Obj.direction);
        }

        public static void Update(GameObject Obj)
        {   //only roomObjs are passed into this method, some get AI (or behaviors)
            //roomObjs don't have lifetimes, they last the life of the room
            if (Obj.getsAI) { Functions_Ai.HandleObj(Obj); }
        }

        
        
        public static void SetType(GameObject Obj, ObjType Type)
        {   //Obj.direction should be set prior to this method running - important
            Obj.type = Type;


            #region Assign Level Sheet based on Level.Type Check

            Obj.compSprite.texture = Assets.forestLevelSheet;
            //below in type checks, objs/particles/projectiles/pickups 
            //switch their textures to whatever sheet they need

            #endregion



            if (Type == ObjType.Unknown)
            {   
                ResetObject(Obj);
                Obj.type = ObjType.Unknown;
                Obj.compCollision.blocking = false;
                Obj.compSprite.texture = Assets.uiItemsSheet;
                Obj.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Unknown;
                Obj.compSprite.zOffset = -64; //sort below everything else
            }


            //Dungeon Objects

            #region Exits

            else if (Type == ObjType.Dungeon_ExitPillarLeft ||
               Type == ObjType.Dungeon_ExitPillarRight)
            {
                Obj.compSprite.cellSize.Y = 16 * 3; //nonstandard size
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.group = ObjGroup.Door;
                Obj.compCollision.rec.Height = 32 - 5;
                Obj.compCollision.offsetY = 14;
                if (Type == ObjType.Dungeon_ExitPillarLeft)
                { Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_ExitPillarLeft; }
                else { Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_ExitPillarRight; }
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }
            else if (Type == ObjType.Dungeon_Exit)
            {
                Obj.compSprite.cellSize.Y = 16 * 3; //nonstandard size
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.group = ObjGroup.Door;
                Obj.compCollision.rec.Height = 4;
                Obj.compCollision.offsetY = 32 + 4;
                Obj.compCollision.blocking = false;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_Exit;
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }
            else if (Type == ObjType.Dungeon_ExitLight)
            {
                Obj.compSprite.cellSize.Y = 16 * 2; //nonstandard size
                Obj.compCollision.offsetY = 0;
                Obj.compSprite.zOffset = 256; //sort above everything
                Obj.compCollision.blocking = false;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_ExitLight;
            }

            #endregion


            #region Doors  

            else if (Type == ObjType.Dungeon_DoorOpen)
            {
                Obj.compSprite.zOffset = +32; //sort very high (over / in front of hero)
                Obj.group = ObjGroup.Door;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_DoorOpen;
            }
            else if(Type == ObjType.Dungeon_DoorTrap)
            {
                Obj.compCollision.blocking = false;
                Obj.compSprite.zOffset = -32; //sort very low (behind hero)
                Obj.group = ObjGroup.Door;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_DoorShut;
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }
            else if (Type == ObjType.Dungeon_DoorBombable)
            {
                Obj.compSprite.zOffset = -32; //sort very low (behind hero)
                Obj.group = ObjGroup.Door;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_WallStraightCracked;
                Obj.sfx.hit = Assets.sfxTapHollow; //sounds hollow
            }
            else if(Type == ObjType.Dungeon_DoorBoss)
            {
                Obj.compSprite.zOffset = -32; //sort very low (behind hero)
                Obj.group = ObjGroup.Door;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_DoorBoss;
                Obj.sfx.hit = Assets.sfxTapMetallic;
                Obj.sfx.kill = Assets.sfxShatter;
            }
            else if (Type == ObjType.Dungeon_DoorShut)
            {
                Obj.compSprite.zOffset = -32; //sort very low (behind hero)
                Obj.group = ObjGroup.Door;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_DoorShut;
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }
            else if (Type == ObjType.Dungeon_DoorFake)
            {
                Obj.compSprite.zOffset = -32; //sort very low (behind hero)
                Obj.group = ObjGroup.Door;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_WallStraightCracked;
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }

            #endregion


            #region Walls

            //wall soundfx is set in a group check at end of method

            else if (Type == ObjType.Dungeon_WallStraight)
            {
                Obj.compSprite.zOffset = -32; //sort very low (behind hero)
                Obj.group = ObjGroup.Wall;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_WallStraight;
            }
            else if(Type == ObjType.Dungeon_WallStraightCracked)
            {
                Obj.compSprite.zOffset = -32; //sort very low (behind hero)
                Obj.group = ObjGroup.Wall;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_WallStraightCracked;
            }
            else if (Type == ObjType.Dungeon_WallInteriorCorner)
            {
                Obj.compSprite.zOffset = -32; //sort very low (behind hero)
                Obj.group = ObjGroup.Wall;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_WallInteriorCorner;
                
            }
            else if (Type == ObjType.Dungeon_WallExteriorCorner)
            {
                Obj.compSprite.zOffset = -32; //sort very low (behind hero)
                Obj.group = ObjGroup.Wall;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_WallExteriorCorner;
            }
            else if (Type == ObjType.Dungeon_WallPillar)
            {
                Obj.compSprite.zOffset = -32; //sort very low (behind hero)
                Obj.group = ObjGroup.Wall;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_WallPillar;
            }
            else if (Type == ObjType.Dungeon_WallStatue)
            {
                Obj.compSprite.zOffset = -16; //sort low, but over walls
                Obj.group = ObjGroup.Wall;
                Obj.getsAI = true; //obj gets AI
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_WallStatue;
            }
            else if (Type == ObjType.Dungeon_WallTorch)
            {
                Obj.compSprite.zOffset = -16; //sort low, but over walls
                Obj.group = ObjGroup.Wall;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_WallTorch;
            }

            #endregion


            #region Floor Objects

            else if (Type == ObjType.Dungeon_FloorDecal)
            {   
                Obj.compSprite.zOffset = -32; //sort low, but over floor
                Obj.compCollision.blocking = false;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_FloorDecal;
            }
            else if(Type == ObjType.Dungeon_FloorBlood)
            {   //collision rec is smaller so more debris is left when room is cleanedUp()
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 10;
                Obj.compSprite.zOffset = -32; //sort low, but over floor
                Obj.compCollision.blocking = false;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_FloorBlood;
            }

            #endregion


            #region Pits

            else if (Type == ObjType.Dungeon_Pit)
            {   //this pit interacts with actor
                Obj.compSprite.zOffset = -64; //sort under pit teeth
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 10;
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
                Obj.getsAI = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_Pit;
            }
            else if(Type == ObjType.Dungeon_PitBridge)
            {
                Obj.compCollision.blocking = false;
                Obj.compSprite.zOffset = -64; //sort to floor
                Obj.canBeSaved = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_PitBridge;
            }
            else if (Type == ObjType.Dungeon_PitTeethTop)
            {
                Obj.compSprite.zOffset = -60; //sort above pits & pit bubbles
                Obj.compCollision.rec.Width = 16; Obj.compCollision.rec.Height = 8;
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_PitTeethTop;
            }
            else if(Type == ObjType.Dungeon_PitTeethBottom)
            {
                Obj.compSprite.zOffset = -60; //sort above pits & pit bubbles
                Obj.compCollision.rec.Width = 16; Obj.compCollision.rec.Height = 8;
                Obj.compCollision.offsetY = 4;
                Obj.compCollision.rec.Height = 4;
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_PitTeethBottom;
            }
            else if (Type == ObjType.Dungeon_PitTrap)
            {   //this becomes a pit upon collision with hero
                Obj.compCollision.offsetX = -12; Obj.compCollision.offsetY = -12;
                Obj.compCollision.rec.Width = 24; Obj.compCollision.rec.Height = 24;
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_FloorCracked;
            }

            #endregion


            #region BossStatue

            else if (Type == ObjType.Dungeon_Statue)
            {
                Obj.compCollision.rec.Height = 8;
                Obj.compCollision.offsetY = -1;
                Obj.canBeSaved = true;
                Obj.compMove.moveable = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_Statue;
                Obj.sfx.hit = Assets.sfxTapHollow;
                Obj.sfx.kill = Assets.sfxShatter;
            }

            #endregion


            #region Chests

            else if (Type == ObjType.Dungeon_Chest || 
                Type == ObjType.Dungeon_ChestKey || 
                Type == ObjType.Dungeon_ChestMap)
            {
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -3;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 11;
                Obj.compSprite.zOffset = -7;
                Obj.canBeSaved = true;
                Obj.group = ObjGroup.Chest;
                Obj.compMove.moveable = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_ChestClosed;
                Obj.sfx.hit = Assets.sfxTapHollow;
            }
            else if (Type == ObjType.Dungeon_ChestEmpty)
            {
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -3;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 11;
                Obj.compSprite.zOffset = -7;
                Obj.canBeSaved = true;
                //Obj.group = ObjGroup.Chest; //not really a chest, just obj
                Obj.compMove.moveable = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_ChestOpened;
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }

            #endregion


            #region Blocks

            else if (Type == ObjType.Dungeon_BlockDark || Type == ObjType.Dungeon_BlockLight)
            {
                Obj.compSprite.zOffset = -7;
                Obj.canBeSaved = true;
                if (Type == ObjType.Dungeon_BlockLight)
                {   //lighter blocks are moveable by belts
                    Obj.compMove.moveable = true;
                    Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_BlockLight;
                }
                else { Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_BlockDark; }
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }
            else if (Type == ObjType.Dungeon_BlockSpike)
            {
                Obj.compSprite.zOffset = -7;
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -7;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 14;
                //set block's moving direction, based on facing direction
                Obj.compMove.direction = Obj.direction;
                Obj.canBeSaved = true;
                Obj.compMove.moveable = true;
                Obj.compMove.grounded = false; //in air
                Obj.compCollision.blocking = false;
                Obj.compMove.speed = 0.225f; //spike blocks move med
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_BlockSpike;
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }

            #endregion


            #region Lever and Lever Objects

            else if (Type == ObjType.Dungeon_LeverOn || Type == ObjType.Dungeon_LeverOff)
            {
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = 2;
                Obj.compCollision.rec.Width = 12; Obj.compCollision.rec.Height = 5;
                Obj.compSprite.zOffset = -3;
                Obj.canBeSaved = true;
                Obj.compMove.moveable = true;
                if (Type == ObjType.Dungeon_LeverOn)
                { Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_LeverOn; }
                else { Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_LeverOff; }
            }
            else if (Type == ObjType.Dungeon_SpikesFloorOn || Type == ObjType.Dungeon_SpikesFloorOff)
            {
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 10;
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
                if (Type == ObjType.Dungeon_SpikesFloorOn)
                { Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_FloorSpikesOn; }
                else { Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_FloorSpikesOff; }
            }
            else if (Type == ObjType.Dungeon_ConveyorBeltOn || Type == ObjType.Dungeon_ConveyorBeltOff)
            {
                Obj.compSprite.zOffset = -31; //sort just above floor
                Obj.compAnim.speed = 10; //in frames
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
                if (Type == ObjType.Dungeon_ConveyorBeltOn)
                { Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_ConveyorBeltOn; }
                else { Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_ConveyorBeltOff; }
            }

            #endregion


            #region Pot, Barrel, Bumper, Flamethrower, IceTile

            else if (Type == ObjType.Dungeon_Pot)
            {
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -3;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 11;
                Obj.compSprite.zOffset = -7;
                Obj.canBeSaved = true;
                Obj.compMove.moveable = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_Pot;
                Obj.sfx.hit = Assets.sfxEnemyHit;
                Obj.sfx.kill = Assets.sfxShatter;
            }
            else if (Type == ObjType.Dungeon_Barrel)
            {
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 13;
                Obj.compSprite.zOffset = -7;
                Obj.canBeSaved = true;
                Obj.compMove.moveable = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_Barrel;
                Obj.sfx.hit = Assets.sfxEnemyHit;
            }
            else if (Type == ObjType.Dungeon_Bumper)
            {
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_Bumper;
            }
            else if (Type == ObjType.Dungeon_Flamethrower)
            {
                Obj.compSprite.zOffset = -30; //sort slightly above floor
                Obj.compCollision.blocking = true;
                Obj.getsAI = true; //obj gets AI
                Obj.canBeSaved = true;
                Obj.compMove.moveable = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_Flamethrower;
            }
            else if (Type == ObjType.Dungeon_IceTile)
            {
                Obj.compCollision.offsetX = -6; Obj.compCollision.offsetY = -6;
                Obj.compCollision.rec.Width = 12; Obj.compCollision.rec.Height = 12;
                Obj.compSprite.zOffset = -30; //sort a little above floor
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_IceTile;
            }

            #endregion


            #region Floor Switches

            else if (Type == ObjType.Dungeon_Switch || Type == ObjType.Dungeon_SwitchDown)
            {
                Obj.compCollision.offsetX = -4; Obj.compCollision.offsetY = -3;
                Obj.compCollision.rec.Width = 8; Obj.compCollision.rec.Height = 8;
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.compMove.moveable = true;
                if (Type == ObjType.Dungeon_Switch)
                { Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_FloorSwitchUp; }
                else { Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_FloorSwitchDown; }
                //this makes the switch work
                Obj.getsAI = true;
            }

            #endregion


            #region Switch Blocks & Button

            else if (Type == ObjType.Dungeon_SwitchBlockBtn)
            {
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -4;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 12;
                Obj.compSprite.zOffset = -7;
                Obj.canBeSaved = true;
                Obj.compMove.moveable = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_SwitchBlockBtn;
                Obj.sfx.hit = Assets.sfxEnemyHit;
            }
            else if (Type == ObjType.Dungeon_SwitchBlockDown)
            {
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_SwitchBlockDown;
            }
            else if (Type == ObjType.Dungeon_SwitchBlockUp)
            {
                Obj.compCollision.offsetX = -8; Obj.compCollision.offsetY = -8;
                Obj.compCollision.rec.Width = 16; Obj.compCollision.rec.Height = 16;
                Obj.compSprite.zOffset = -7; //sort normally
                Obj.compCollision.blocking = true;
                Obj.canBeSaved = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_SwitchBlockUp;
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }

            #endregion


            #region Torches (lit and unlit)

            else if (Type == ObjType.Dungeon_TorchUnlit || Type == ObjType.Dungeon_TorchLit)
            {
                Obj.compCollision.offsetX = -8; Obj.compCollision.offsetY = -4;
                Obj.compCollision.rec.Width = 16; Obj.compCollision.rec.Height = 12;
                Obj.compSprite.zOffset = -7;
                Obj.canBeSaved = true;
                Obj.compMove.moveable = true;
                if (Type == ObjType.Dungeon_TorchUnlit)
                { Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_TorchUnlit; }
                else { Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_TorchLit; }
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }

            #endregion


            #region Living RoomObjects - Fairy

            else if (Type == ObjType.Dungeon_Fairy)
            {
                Obj.compSprite.zOffset = 8; //sort to air
                Obj.lifetime = 0; //stay around forever
                Obj.compAnim.speed = 6; //in frames
                Obj.compMove.moveable = true;
                Obj.compCollision.blocking = false;
                Obj.getsAI = true;
                Obj.compMove.grounded = false; //obj is flying
                Obj.compMove.friction = World.frictionAir;
                Obj.canBeSaved = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_Fairy;
                Obj.sfx.hit = Assets.sfxEnemyHit;
            }

            #endregion


            #region Enemy Spawn Objects

            else if (Type == ObjType.Dungeon_SpawnMob || Type == ObjType.Dungeon_SpawnMiniBoss)
            {
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.compCollision.blocking = false;
                Obj.group = ObjGroup.EnemySpawn;
                Obj.canBeSaved = true;
                if (Type == ObjType.Dungeon_SpawnMob)
                { Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_SpawnMob; }
                else { Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_SpawnMiniBoss; }
            }

            #endregion



            //World Objects


            #region Grass Objects

            else if (Type == ObjType.Wor_Grass_1 || Type == ObjType.Wor_Grass_2
                || Type == ObjType.Wor_Grass_Cut || Type == ObjType.Wor_Grass_Tall
                || Type == ObjType.Wor_Flowers)
            {
                Obj.compSprite.zOffset = -32;
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
                Obj.compAnim.loop = true;

                //set animation frame
                if (Type == ObjType.Wor_Grass_1 || Type == ObjType.Wor_Grass_Cut)
                {
                    Obj.compAnim.currentAnimation = AnimationFrames.World_Grass_Short;
                }
                else if (Type == ObjType.Wor_Grass_2)
                {
                    Obj.compAnim.currentAnimation = AnimationFrames.World_Grass_Minimum;
                }
                else if (Type == ObjType.Wor_Grass_Tall)
                {
                    Obj.compAnim.currentAnimation = AnimationFrames.World_Grass_Tall;
                    Obj.sfx.kill = Assets.sfxBushCut; //only tall grass can get killed()
                }
                else if (Type == ObjType.Wor_Flowers)
                {
                    Obj.compAnim.currentAnimation = AnimationFrames.World_Flowers;
                    //randomly set the starting frame for flowers, so their animations dont sync up
                    Obj.compAnim.index = (byte)Functions_Random.Int(0, Obj.compAnim.currentAnimation.Count);
                }

                //randomly flip sprite horizontally
                if (Functions_Random.Int(0, 100) < 50)
                { Obj.compSprite.flipHorizontally = true; }
                else { Obj.compSprite.flipHorizontally = false; }
            }

            #endregion


            #region Foilage

            else if (Type == ObjType.Wor_Bush)
            {
                Obj.canBeSaved = true;
                Obj.compSprite.zOffset = -2;
                Obj.compCollision.blocking = true;
                Obj.compAnim.currentAnimation = AnimationFrames.World_Bush;
                Obj.compCollision.offsetX = -6; Obj.compCollision.offsetY = -2;
                Obj.compCollision.rec.Width = 12; Obj.compCollision.rec.Height = 8;
                Obj.sfx.kill = Assets.sfxBushCut;
                Obj.sfx.hit = Assets.sfxEnemyHit;
            }
            else if (Type == ObjType.Wor_Bush_Stump)
            {
                Obj.canBeSaved = true;
                Obj.compSprite.zOffset = -24;
                Obj.compCollision.blocking = false;
                Obj.compAnim.currentAnimation = AnimationFrames.World_BushStump;
                Obj.compCollision.offsetX = -2; Obj.compCollision.offsetY = -2;
                Obj.compCollision.rec.Width = 5; Obj.compCollision.rec.Height = 5;
            }
            else if (Type == ObjType.Wor_Tree 
                || Type == ObjType.Wor_Tree_Burning 
                || Type == ObjType.Wor_Tree_Burnt)
            {
                Obj.compSprite.cellSize.Y = 16 * 2; //nonstandard size
                Obj.canBeSaved = true;
                Obj.compSprite.zOffset = +10;
                Obj.compCollision.blocking = true;
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = 15;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 8;
                Obj.sfx.hit = Assets.sfxEnemyHit;
                Obj.sfx.kill = Assets.sfxBushCut;
                //set correct animFrame based on type
                if (Type == ObjType.Wor_Tree)
                { Obj.compAnim.currentAnimation = AnimationFrames.World_Tree; }
                else if (Type == ObjType.Wor_Tree_Burning)
                {   //same as tree, just covered in fire objs
                    Obj.compAnim.currentAnimation = AnimationFrames.World_Tree;
                    Obj.lifeCounter = 0;
                    Obj.lifetime = 150; //"burn" for this long
                    Obj.getsAI = true; //will spawn fire on tree
                }
                else //burning tree becomes burnt version eventually
                { Obj.compAnim.currentAnimation = AnimationFrames.World_TreeBurnt; }
            }
            else if (Type == ObjType.Wor_Tree_Stump)
            {
                Obj.compSprite.cellSize.Y = 16 * 2; //nonstandard size
                Obj.canBeSaved = true;
                Obj.compSprite.zOffset = +10;
                Obj.compCollision.blocking = false;
                Obj.compAnim.currentAnimation = AnimationFrames.World_TreeStump;
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = 15;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 8;
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }

            #endregion


            #region Debris

            else if (Type == ObjType.Wor_Debris)
            {
                Obj.canBeSaved = true;
                Obj.compSprite.zOffset = -24;
                Obj.compCollision.blocking = false;
                Obj.compMove.moveable = true;
                //randomly choose animFrame
                if (Functions_Random.Int(0, 100) < 50)
                { Obj.compAnim.currentAnimation = AnimationFrames.World_Debris1; }
                else { Obj.compAnim.currentAnimation = AnimationFrames.World_Debris2; }
                //randomly flip sprite horizontally
                if (Functions_Random.Int(0, 100) < 50)
                { Obj.compSprite.flipHorizontally = true; }
                else { Obj.compSprite.flipHorizontally = false; }
            }

            #endregion


            #region Dungeon Entrances

            else if (Type == ObjType.Wor_Entrance_ForestDungeon)
            {   
                Obj.compSprite.cellSize.X = 16 * 3; //nonstandard size
                Obj.compSprite.cellSize.Y = 16 * 4; //nonstandard size
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Entrance_ForestDungeon;
                //set collision rec near bottom of entrance
                Obj.compCollision.rec.Width = 16*3-4; Obj.compCollision.offsetX = -6;
                Obj.compCollision.rec.Height = 16*3; Obj.compCollision.offsetY = +8;
                //sort save and block
                Obj.compSprite.zOffset = +16*3 - 2;
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = true;
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }

            #endregion


            #region Objects

            else if (Type == ObjType.Wor_Pot) //same as Dungeon_Pot
            {
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -3;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 11;
                Obj.compSprite.zOffset = -7;
                Obj.canBeSaved = true;
                Obj.compMove.moveable = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Pot;
                Obj.sfx.hit = Assets.sfxEnemyHit;
                Obj.sfx.kill = Assets.sfxShatter;
            }

            #endregion


            #region Interior Building Objects

            else if (Type == ObjType.Wor_Bookcase || Type == ObjType.Wor_Shelf)
            {
                Obj.compCollision.offsetY = 0; Obj.compCollision.rec.Height = 8;
                Obj.compSprite.zOffset = -7;
                Obj.canBeSaved = true;
                if (Type == ObjType.Wor_Bookcase)
                { Obj.compAnim.currentAnimation = AnimationFrames.World_Bookcase; }
                else { Obj.compAnim.currentAnimation = AnimationFrames.World_Shelf; }
                Obj.sfx.hit = Assets.sfxTapMetallic;
                Obj.sfx.kill = Assets.sfxShatter;
            }
            else if (Type == ObjType.Wor_TableStone || Type == ObjType.Wor_TableWood)
            {
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -6;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 12;
                Obj.compSprite.zOffset = -7;
                Obj.canBeSaved = true;

                if (Type == ObjType.Wor_TableStone)
                {
                    Obj.compAnim.currentAnimation = AnimationFrames.World_TableStone;
                    Obj.sfx.hit = Assets.sfxTapMetallic;
                    Obj.sfx.kill = Assets.sfxShatter;
                }
                else
                {   //wood table
                    Obj.compAnim.currentAnimation = AnimationFrames.World_TableWood;
                    Obj.sfx.hit = Assets.sfxEnemyHit;
                    Obj.sfx.kill = Assets.sfxShatter;
                }
            }

            //Wor_TableWood

            #endregion


            #region Vendors

            //Vendors
            else if (Type == ObjType.Vendor_NPC_Items || Type == ObjType.Vendor_NPC_Potions ||
                Type == ObjType.Vendor_NPC_Magic || Type == ObjType.Vendor_NPC_Weapons ||
                Type == ObjType.Vendor_NPC_Armor || Type == ObjType.Vendor_NPC_Equipment
                || Type == ObjType.Vendor_NPC_Pets || Type == ObjType.Vendor_NPC_Story)
            {
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -3;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 11;
                Obj.compSprite.zOffset = 0;
                Obj.compAnim.speed = 20; //slow animation
                Obj.group = ObjGroup.Vendor;
                Obj.canBeSaved = true;

                if (Type == ObjType.Vendor_NPC_Items) { Obj.compAnim.currentAnimation = AnimationFrames.Vendor_Items; }
                else if (Type == ObjType.Vendor_NPC_Potions) { Obj.compAnim.currentAnimation = AnimationFrames.Vendor_Potions; }
                else if (Type == ObjType.Vendor_NPC_Magic) { Obj.compAnim.currentAnimation = AnimationFrames.Vendor_Magic; }
                else if (Type == ObjType.Vendor_NPC_Weapons) { Obj.compAnim.currentAnimation = AnimationFrames.Vendor_Weapons; }
                else if (Type == ObjType.Vendor_NPC_Armor) { Obj.compAnim.currentAnimation = AnimationFrames.Vendor_Armor; }
                else if (Type == ObjType.Vendor_NPC_Equipment) { Obj.compAnim.currentAnimation = AnimationFrames.Vendor_Equipment; }
                else if (Type == ObjType.Vendor_NPC_Pets) { Obj.compAnim.currentAnimation = AnimationFrames.Vendor_Pets; }
                else if (Type == ObjType.Vendor_NPC_Story) { Obj.compAnim.currentAnimation = AnimationFrames.Vendor_Story; }
            }

            #endregion


            #region Water Objects


            else if (Type == ObjType.Wor_Water
                || Type == ObjType.Wor_Coastline_Straight
                || Type == ObjType.Wor_Coastline_Corner_Exterior
                || Type == ObjType.Wor_Coastline_Corner_Interior)
            {
                Obj.compSprite.zOffset = -40;
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
                Obj.compAnim.loop = true; //no anim, but we should for coastline in the future

                if (Type == ObjType.Wor_Water)
                {
                    Obj.compAnim.currentAnimation = AnimationFrames.Wor_Water;
                }
                else if (Type == ObjType.Wor_Coastline_Straight)
                {
                    if (Functions_Random.Int(0, 100) > 50) //randomly choose coastline
                    { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Coastline_Straight_A; }
                    else { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Coastline_Straight_B; }
                }
                else if (Type == ObjType.Wor_Coastline_Corner_Exterior)
                {
                    Obj.compAnim.currentAnimation = AnimationFrames.Wor_Coastline_Corner_Exterior;
                }
                else if (Type == ObjType.Wor_Coastline_Corner_Interior)
                {
                    Obj.compAnim.currentAnimation = AnimationFrames.Wor_Coastline_Corner_Interior;
                }
            }
            

            #endregion



            //Entities

            #region Pickups

            else if (
                Type == ObjType.Pickup_Rupee || Type == ObjType.Pickup_Heart ||
                Type == ObjType.Pickup_Magic || Type == ObjType.Pickup_Arrow ||
                Type == ObjType.Pickup_Bomb)
            {
                Obj.compSprite.cellSize.X = 8; //non standard cellsize
                Obj.compCollision.offsetX = -8; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 8; Obj.compCollision.rec.Height = 10;
                Obj.group = ObjGroup.Pickup;
                Obj.lifetime = 255; //in frames
                Obj.compAnim.speed = 6; //in frames
                Obj.compMove.moveable = true;
                Obj.compSprite.texture = Assets.entitiesSheet; //all use entity sheet
                //set the animation frame
                if (Type == ObjType.Pickup_Rupee) { Obj.compAnim.currentAnimation = AnimationFrames.Pickup_Rupee; }
                else if (Type == ObjType.Pickup_Heart) { Obj.compAnim.currentAnimation = AnimationFrames.Pickup_Heart; }
                else if (Type == ObjType.Pickup_Magic) { Obj.compAnim.currentAnimation = AnimationFrames.Pickup_Magic; }
                else if (Type == ObjType.Pickup_Arrow) { Obj.compAnim.currentAnimation = AnimationFrames.Pickup_Arrow; }
                else if (Type == ObjType.Pickup_Bomb) { Obj.compAnim.currentAnimation = AnimationFrames.Pickup_Bomb; }
            }

            #endregion



            //Projectiles

            #region Projectiles - Items

            else if (Type == ObjType.ProjectileBomb)
            {
                Obj.compSprite.zOffset = -4; //sort to floor
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 10;
                Obj.group = ObjGroup.Projectile;
                Obj.lifetime = 100; //in frames
                Obj.compAnim.speed = 7; //in frames
                Obj.compMove.moveable = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Projectile_Bomb;
                Obj.compSprite.texture = Assets.entitiesSheet;
            }
            else if (Type == ObjType.ProjectileBoomerang)
            {
                Obj.compSprite.zOffset = 16;
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 10;
                Obj.group = ObjGroup.Projectile;
                Obj.lifetime = 255; //in frames
                Obj.compMove.friction = 0.96f; //some air friction
                Obj.compAnim.speed = 5; //in frames
                Obj.compMove.moveable = true;
                Obj.compMove.grounded = false; //obj is airborne
                Obj.compAnim.currentAnimation = AnimationFrames.Projectile_Boomerang;
                Obj.compSprite.texture = Assets.entitiesSheet;
            }

            #endregion


            #region Projectiles - Magic

            else if (Type == ObjType.ProjectileFireball)
            {
                Obj.compSprite.zOffset = 16;
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 10;
                Obj.group = ObjGroup.Projectile;
                Obj.lifetime = 50; //in frames
                Obj.compMove.friction = 0.984f; //some air friction
                Obj.compAnim.speed = 5; //in frames
                Obj.compMove.moveable = true;
                Obj.compMove.grounded = false; //obj is airborne
                Obj.compAnim.currentAnimation = AnimationFrames.Projectile_Fireball;
                Obj.compSprite.texture = Assets.entitiesSheet;
            }

            #endregion


            #region Projectiles - Weapons

            else if (Type == ObjType.ProjectileSword
                || Type == ObjType.ProjectileShovel)
            {
                Obj.compSprite.zOffset = 16;
                //set collision rec based on direction
                if (Obj.direction == Direction.Up)
                {
                    Obj.compCollision.offsetX = -4; Obj.compCollision.offsetY = -4;
                    Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 15;
                }
                else if (Obj.direction == Direction.Down)
                {
                    Obj.compCollision.offsetX = -4; Obj.compCollision.offsetY = -5;
                    Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 10;
                }
                else if (Obj.direction == Direction.Left)
                {
                    Obj.compCollision.offsetX = -4; Obj.compCollision.offsetY = -3;
                    Obj.compCollision.rec.Width = 11; Obj.compCollision.rec.Height = 10;
                }
                else //right
                {
                    Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -3;
                    Obj.compCollision.rec.Width = 11; Obj.compCollision.rec.Height = 10;
                }
                Obj.group = ObjGroup.Projectile;
                Obj.lifetime = 18; //in frames
                Obj.compAnim.speed = 2; //in frames
                Obj.compAnim.loop = false;
                Obj.compMove.moveable = true;
                Obj.compMove.grounded = false; //obj is airborne
                Obj.compSprite.texture = Assets.entitiesSheet;

                //set the animFrames based on type
                if (Type == ObjType.ProjectileSword)
                { Obj.compAnim.currentAnimation = AnimationFrames.Projectile_Sword; }
                else { Obj.compAnim.currentAnimation = AnimationFrames.Projectile_Shovel; }
            }
            else if (Type == ObjType.ProjectileArrow)
            {
                Obj.compSprite.zOffset = 16;
                //set collision rec based on direction
                if (Obj.direction == Direction.Up || Obj.direction == Direction.Down)
                {
                    Obj.compCollision.offsetX = -2; Obj.compCollision.offsetY = -6;
                    Obj.compCollision.rec.Width = 4; Obj.compCollision.rec.Height = 12;
                }
                else //left or right
                {
                    Obj.compCollision.offsetX = -6; Obj.compCollision.offsetY = -2;
                    Obj.compCollision.rec.Width = 12; Obj.compCollision.rec.Height = 4;
                }
                Obj.group = ObjGroup.Projectile;
                Obj.lifetime = 200; //in frames
                Obj.compAnim.speed = 5; //in frames
                Obj.compMove.friction = 1.0f; //no air friction
                Obj.compMove.moveable = true;
                Obj.compMove.grounded = false; //obj is airborne
                Obj.compAnim.currentAnimation = AnimationFrames.Projectile_Arrow;
                Obj.compSprite.texture = Assets.entitiesSheet;
                Obj.sfx.kill = Assets.sfxArrowHit;
                Obj.sfx.hit = Assets.sfxArrowHit;
            }
            else if (Type == ObjType.ProjectileNet)
            {
                Obj.compSprite.zOffset = 16;
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 10;
                Obj.group = ObjGroup.Projectile;
                Obj.lifetime = 18; //in frames
                Obj.compAnim.speed = 2; //in frames
                Obj.compAnim.loop = false;
                Obj.compMove.moveable = true;
                Obj.compMove.grounded = false; //obj is airborne
                Obj.compAnim.currentAnimation = AnimationFrames.Projectile_Net;
                Obj.compSprite.texture = Assets.entitiesSheet;
                Obj.sfx.hit = null; Obj.sfx.kill = null;
            }
            else if (Type == ObjType.ProjectileBow)
            {
                Obj.compSprite.zOffset = 32;
                Obj.group = ObjGroup.Projectile;
                Obj.lifetime = 15; //in frames
                Obj.compAnim.speed = 10; //in frames
                Obj.compAnim.loop = false;
                Obj.compMove.grounded = false; //obj is airborne
                Obj.compAnim.currentAnimation = AnimationFrames.Projectile_Bow;
                Obj.compSprite.texture = Assets.entitiesSheet;
                Obj.sfx.hit = null; Obj.sfx.kill = null;
            }

            #endregion


            #region Projectiles - World

            else if (Type == ObjType.ProjectileExplosion)
            {
                Obj.compSprite.zOffset = 16;
                Obj.compCollision.offsetX = -12; Obj.compCollision.offsetY = -13;
                Obj.compCollision.rec.Width = 24; Obj.compCollision.rec.Height = 26;
                Obj.group = ObjGroup.Projectile;
                Obj.lifetime = 24; //in frames
                Obj.compAnim.speed = 5; //in frames
                Obj.compAnim.loop = false;
                Obj.compMove.grounded = false; //obj is airborne
                Obj.compAnim.currentAnimation = AnimationFrames.Projectile_Explosion;
                Obj.compSprite.texture = Assets.entitiesSheet;
                Obj.sfx.kill = Assets.sfxExplosion;
                Obj.sfx.hit = Assets.sfxExplosion;
            }
            else if (Type == ObjType.ProjectileGroundFire)
            {
                Obj.compSprite.zOffset = 6;
                Obj.group = ObjGroup.Projectile;
                Obj.lifetime = 100; //in frames
                Obj.compAnim.speed = 7; //in frames
                Obj.compAnim.currentAnimation = AnimationFrames.Projectile_FireGround;
                Obj.compSprite.texture = Assets.entitiesSheet;
                //this controls how quick fire spreads:
                Obj.interactiveFrame = 60; //early in life = quick spread
                Obj.interactiveFrame += Functions_Random.Int(-15, 15); 
                //add a random -/+ offset to stagger the spread
            }

            #endregion


            #region Projectiles - World - Thrown

            else if (Type == ObjType.ProjectileBush
                || Type == ObjType.ProjectilePot
                || Type == ObjType.ProjectilePotSkull)
            {
                Obj.group = ObjGroup.Projectile;
                Obj.compSprite.zOffset = 32;

                Obj.lifetime = 20; //in frames
                Obj.compMove.grounded = false; //obj is airborne
                Obj.compMove.friction = 0.984f; //some air friction
                Obj.compSprite.texture = Assets.forestLevelSheet;

                //refine this hitBox later
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 10;

                //set animFrame based on type
                if (Type == ObjType.ProjectileBush)
                {
                    Obj.compAnim.currentAnimation = AnimationFrames.World_Bush;
                    Obj.sfx.kill = Assets.sfxBushCut;
                    Obj.sfx.hit = Assets.sfxEnemyHit;
                }
                else if (Type == ObjType.ProjectilePot)
                {
                    Obj.compAnim.currentAnimation = AnimationFrames.Wor_Pot;
                    Obj.sfx.hit = Assets.sfxEnemyHit;
                    Obj.sfx.kill = Assets.sfxShatter;
                }
                else
                {   //skull pot is default
                    Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_Pot;
                    Obj.sfx.hit = Assets.sfxEnemyHit;
                    Obj.sfx.kill = Assets.sfxShatter;
                }
            }

            #endregion


            #region Projectiles - Dungeon Specific

            else if (Type == ObjType.ProjectileExplodingBarrel)
            {   //this should match the Barrel GameObj
                Obj.compSprite.zOffset = -7;
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 10;
                Obj.group = ObjGroup.Projectile;
                Obj.lifetime = 40; //in frames
                Obj.compAnim.speed = 7;
                Obj.compMove.moveable = true;
                Obj.compMove.grounded = true;

                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_BarrelExploding;
                Obj.compSprite.texture = Assets.forestLevelSheet;
                Obj.sfx.hit = Assets.sfxEnemyHit;
            }

            #endregion



            





            //Particles

            #region Particles - Dungeon Specific

            //these particle's sprites live on a dungeon sheet,
            //whichever dungeon sheet is the current one

            else if (Type == ObjType.Particle_PitBubble)
            {
                Obj.compSprite.cellSize.X = 8; Obj.compSprite.cellSize.Y = 8; //nonstandard size
                Obj.compSprite.zOffset = -63; //sort over pits, under pit teeth
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 10 * 4 * 6; //speed * anim frames * loops
                Obj.compAnim.speed = 10; //in frames
                Obj.compAnim.currentAnimation = AnimationFrames.Particle_PitBubble;
                //not on the entities sheet, on dungeon sheet
                Obj.compSprite.texture = Assets.forestLevelSheet;
            }

            #endregion


            #region Particles - Small

            else if (Type == ObjType.Particle_RisingSmoke)
            {
                Obj.compSprite.cellSize.X = 8; Obj.compSprite.cellSize.Y = 8; //nonstandard size
                Obj.compSprite.zOffset = 16;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 24; //in frames
                Obj.compAnim.speed = 6; //in frames
                Obj.compAnim.loop = false;
                Obj.compAnim.currentAnimation = AnimationFrames.Particle_RisingSmoke;
                Obj.compSprite.texture = Assets.entitiesSheet;
                //randomly flip the smoke sprite horizontally for variation
                if (Functions_Random.Int(0, 101) > 50)
                { Obj.compSprite.flipHorizontally = true; }
                else { Obj.compSprite.flipHorizontally = false; }
            }
            else if (Type == ObjType.Particle_ImpactDust)
            {
                Obj.compSprite.cellSize.X = 8; Obj.compSprite.cellSize.Y = 8; //nonstandard size
                Obj.compSprite.zOffset = 16;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 24; //in frames
                Obj.compAnim.speed = 5; //in frames
                Obj.compAnim.loop = false;
                Obj.compAnim.currentAnimation = AnimationFrames.Particle_ImpactDust;
                Obj.compSprite.texture = Assets.entitiesSheet;
            }
            else if (Type == ObjType.Particle_Sparkle)
            {
                Obj.compSprite.cellSize.X = 8; Obj.compSprite.cellSize.Y = 8; //nonstandard size
                Obj.compSprite.zOffset = 16;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 24; //in frames
                Obj.compAnim.speed = 6; //in frames
                Obj.compAnim.currentAnimation = AnimationFrames.Particle_Sparkle;
                Obj.compSprite.texture = Assets.entitiesSheet;
            }
            else if (Type == ObjType.Particle_Push)
            {
                Obj.compSprite.cellSize.X = 8; Obj.compSprite.cellSize.Y = 8; //nonstandard size
                Obj.compSprite.zOffset = 32;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 6*3; //in frames
                Obj.compAnim.speed = 6; //in frames
                Obj.compAnim.currentAnimation = AnimationFrames.Particle_Push;
                Obj.compSprite.texture = Assets.entitiesSheet;

                //set the sprites rotation based on direction
                if (Obj.direction == Direction.Down)
                { Obj.compSprite.rotation = Rotation.None; }
                else if (Obj.direction == Direction.Left)
                { Obj.compSprite.rotation = Rotation.Clockwise90; }
                else if (Obj.direction == Direction.Up)
                { Obj.compSprite.rotation = Rotation.Clockwise180; }
                else if (Obj.direction == Direction.Right)
                { Obj.compSprite.rotation = Rotation.Clockwise270; }
                else //push particle can't be in diagonal state, hide it
                { Obj.compSprite.visible = false; }
            }
            else if (Type == ObjType.Particle_Leaf || Type == ObjType.Particle_Debris)
            {
                Obj.compSprite.cellSize.X = 8; Obj.compSprite.cellSize.Y = 8; //nonstandard size
                Obj.compSprite.zOffset = 16;
                Obj.group = ObjGroup.Particle;
                Obj.compAnim.loop = false;
                Obj.lifetime = 15; //in frames
                Obj.compAnim.speed = 6; //in frames
                //setup animation frame properly
                if (Type == ObjType.Particle_Debris)
                { Obj.compAnim.currentAnimation = AnimationFrames.Particle_Debris; }
                else { Obj.compAnim.currentAnimation = AnimationFrames.Particle_Leaf; }
                //not on the entities sheet, on dungeon sheet
                Obj.compSprite.texture = Assets.forestLevelSheet;
            }




            #endregion


            #region Particles - Normal

            else if (Type == ObjType.Particle_Attention)
            {
                Obj.compSprite.zOffset = 1024;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 24; //in frames
                Obj.compAnim.speed = 6; //in frames
                Obj.compAnim.loop = false;
                Obj.compAnim.currentAnimation = AnimationFrames.Particle_Attention;
                Obj.compSprite.texture = Assets.entitiesSheet;
            }
            else if (Type == ObjType.Particle_Splash)
            {
                Obj.compSprite.zOffset = 1024;
                Obj.group = ObjGroup.Particle;
                Obj.compAnim.speed = 7; //in frames
                Obj.lifetime = 7 * 5; //speed * animFrames
                Obj.compAnim.loop = false;
                Obj.compAnim.currentAnimation = AnimationFrames.Particle_Splash;
                Obj.compSprite.texture = Assets.entitiesSheet;
            }
            else if (Type == ObjType.Particle_Blast)
            {
                Obj.compSprite.zOffset = 64;
                Obj.group = ObjGroup.Particle;
                Obj.compAnim.speed = 6; //in frames
                Obj.lifetime = 6*4; //very short
                Obj.compAnim.loop = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Particle_Blast;
                Obj.compSprite.texture = Assets.entitiesSheet;
            }
            else if (Type == ObjType.Particle_Fire)
            {   //the non-interactive version of projectile ground fire
                Obj.compSprite.zOffset = 32;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 100; //in frames
                Obj.compAnim.speed = 7; //in frames
                Obj.compAnim.currentAnimation = AnimationFrames.Projectile_FireGround;
                Obj.compSprite.texture = Assets.entitiesSheet;
            }

            else if (Type == ObjType.Particle_WaterKick)
            {
                Obj.compSprite.zOffset = 0;
                Obj.group = ObjGroup.Particle;
                Obj.compAnim.speed = 7; //in frames
                Obj.lifetime = 7 * 4 + 5; //speed * animTotal + holdFrame
                Obj.compMove.friction = World.frictionWater;
                Obj.compAnim.currentAnimation = AnimationFrames.Particle_WaterKick;
                Obj.compSprite.texture = Assets.entitiesSheet;
                Obj.compAnim.loop = false;
            }

            //Particles - Rewards & Bottles
            else if (
                Type == ObjType.Particle_RewardKey ||
                Type == ObjType.Particle_RewardMap ||
                Type == ObjType.Particle_BottleEmpty ||
                Type == ObjType.Particle_BottleHealth ||
                Type == ObjType.Particle_BottleMagic ||
                Type == ObjType.Particle_BottleCombo ||
                Type == ObjType.Particle_BottleFairy ||
                Type == ObjType.Particle_BottleBlob)
            {
                Obj.compSprite.zOffset = 32;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 40; //in frames
                Obj.compMove.moveable = false;
                //default assume this obj isn't a dungeon key or map
                Obj.compSprite.texture = Assets.uiItemsSheet;
                //set anim frames
                if (Type == ObjType.Particle_RewardKey)
                {   //this obj is on the dungeon sheet
                    Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_BossKey;
                    Obj.compSprite.texture = Assets.forestLevelSheet;
                }
                else if (Type == ObjType.Particle_RewardMap)
                {   //this obj is on the dungeon sheet
                    Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_Map;
                    Obj.compSprite.texture = Assets.forestLevelSheet;
                }
                else if (Type == ObjType.Particle_BottleEmpty)
                { Obj.compAnim.currentAnimation = AnimationFrames.Bottle_Empty; }
                else if (Type == ObjType.Particle_BottleHealth)
                { Obj.compAnim.currentAnimation = AnimationFrames.Bottle_Health; }
                else if (Type == ObjType.Particle_BottleMagic)
                { Obj.compAnim.currentAnimation = AnimationFrames.Bottle_Magic; }
                else if (Type == ObjType.Particle_BottleCombo)
                { Obj.compAnim.currentAnimation = AnimationFrames.Bottle_Combo; }
                else if (Type == ObjType.Particle_BottleFairy)
                { Obj.compAnim.currentAnimation = AnimationFrames.Bottle_Fairy; }
                else if (Type == ObjType.Particle_BottleBlob)
                { Obj.compAnim.currentAnimation = AnimationFrames.Bottle_Blob; }
            }

            #endregion


            #region Particles - Overworld / Map

            //these particles only exist on the overworld map
            else if (Type == ObjType.Particle_Map_Flag)
            {
                Obj.compSprite.cellSize.X = 8; Obj.compSprite.cellSize.Y = 4; //nonstandard size
                Obj.group = ObjGroup.Particle;
                Obj.compAnim.speed = 10; //in frames
                Obj.compAnim.currentAnimation = AnimationFrames.Particle_Map_Flag;
                Obj.compSprite.texture = Assets.entitiesSheet;
                Obj.lifetime = 0; //lives forever
            }
            else if (Type == ObjType.Particle_Map_Wave)
            {
                Obj.compSprite.cellSize.X = 8; Obj.compSprite.cellSize.Y = 4; //nonstandard size
                Obj.group = ObjGroup.Particle;
                Obj.compAnim.speed = 15; //in frames
                Obj.lifetime = (byte)(Obj.compAnim.speed * 4); //there are 4 frames of animation
                Obj.compAnim.loop = false;
                Obj.compAnim.currentAnimation = AnimationFrames.Particle_Map_Wave;
                Obj.compSprite.texture = Assets.entitiesSheet;
            }
            else if (Type == ObjType.Particle_Map_Campfire)
            {
                Obj.compSprite.cellSize.X = 8; Obj.compSprite.cellSize.Y = 8; //nonstandard size
                Obj.group = ObjGroup.Particle;
                Obj.compAnim.speed = 6; //in frames
                Obj.compAnim.currentAnimation = AnimationFrames.Particle_Map_Campfire;
                Obj.compSprite.texture = Assets.entitiesSheet;
                Obj.lifetime = 0; //lives forever
            }

            #endregion



            //Pets

            #region Pets

            else if (Type == ObjType.Pet_Chicken || Type == ObjType.Pet_Dog)
            {   //smaller than normal 16x16 objs
                Obj.compCollision.offsetX = -4; Obj.compCollision.rec.Width = 8;
                Obj.compCollision.offsetY = -4; Obj.compCollision.rec.Height = 8;
                Obj.compSprite.zOffset = -8; //pets are lower to the ground too

                Obj.lifetime = 0; //stay around forever
                Obj.compMove.moveable = true; //obj is moveable by belts
                Obj.getsAI = true; //obj gets ai too (track to hero, set anim frames)
                //Obj.compCollision.blocking = false; //pets block!
                Obj.compSprite.texture = Assets.petsSheet;
                Obj.canBeSaved = true;
                //set initial animation frames for pets
                if (Type == ObjType.Pet_Dog)
                { Obj.compAnim.currentAnimation = AnimationFrames.Pet_Dog_Idle; }
                else if (Type == ObjType.Pet_Chicken)
                { Obj.compAnim.currentAnimation = AnimationFrames.Pet_Chicken_Idle; }
            }

            #endregion


            //Dialog Hero Speaker

            else if (Type == ObjType.Hero_Idle)
            {
                Obj.compAnim.currentAnimation = AnimationFrames.Hero_Animations.idle.down;
                //determine which texture to use when representing hero speaking to himself
                if (Pool.hero.type == ActorType.Blob)
                { Obj.compSprite.texture = Assets.blobSheet; }
                else { Obj.compSprite.texture = Assets.heroSheet; }
            }






            #region Handle Obj Group properties

            if (Obj.group == ObjGroup.Pickup ||
                Obj.group == ObjGroup.Particle || 
                Obj.group == ObjGroup.Projectile)
            {   //entities never block
                Obj.compCollision.blocking = false;
            } 

            else if (Obj.group == ObjGroup.Wall)
            {   //all wall objs have same sfx
                Obj.sfx.hit = Assets.sfxTapMetallic;
                Obj.sfx.kill = null; //cant kill wall
            }

            #endregion




            SetRotation(Obj);
            Functions_Component.UpdateCellSize(Obj.compSprite);
            Obj.compSprite.currentFrame = Obj.compAnim.currentAnimation[0]; //goto 1st anim frame
            Functions_Component.Align(Obj.compMove, Obj.compSprite, Obj.compCollision);
        }

    }
}