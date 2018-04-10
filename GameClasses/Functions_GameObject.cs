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

        public static void ResetObject(GameObject Obj)
        {
            //reset the obj
            Obj.direction = Direction.Down;
            Obj.type = ObjType.Dungeon_WallStraight; //reset the type
            Obj.group = ObjGroup.Object; //assume object is a generic object
            Obj.lifetime = 0; //assume obj exists forever (not projectile)
            Obj.lifeCounter = 0; //reset counter
            Obj.active = true; //assume this object should draw / animate
            Obj.getsAI = false; //most objects do not get any AI input
            Obj.canBeSaved = false; //most objects cannot be saved as XML data
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
            //reset the interaction component
            Obj.compInt.active = true;
            //reset the move component
            Obj.compMove.magnitude.X = 0; //discard any previous magnitude
            Obj.compMove.magnitude.Y = 0; //
            Obj.compMove.speed = 0.0f; //assume this object doesn't move
            Obj.compMove.friction = 0.75f; //normal friction
            Obj.compMove.moveable = false; //most objects cant be moved
            Obj.compMove.grounded = true; //most objects exist on the ground
        }

        public static void SetRotation(GameObject Obj)
        {   
            
            //we could split this out into pro/pick/part SetRotations()
            //but there isn't enough complexity to warrant that split, yet

            
            //handle object/projectile specific cases
            if (Obj.type == ObjType.ProjectileSword || Obj.type == ObjType.ProjectileNet)
            {   //some projectiles flip based on their direction
                if (Obj.direction == Direction.Down || Obj.direction == Direction.Left)
                { Obj.compSprite.flipHorizontally = true; }
            }
            else if (Obj.type == ObjType.ProjectileBomb
                || Obj.type == ObjType.ProjectileExplodingBarrel)
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

        public static void Kill(GameObject Obj)
        {   //first, it's very rare that we actually Kill() a roomObj
            //but, here we can handle any death events for the roomObj, using a type check
            //if (Obj.type == ObjType.Barrel) { } //for example
            Functions_Pool.Release(Obj);
        }

        
        public static void SetType(GameObject Obj, ObjType Type)
        {   //Obj.direction should be set prior to this method running - important
            Obj.type = Type;
            

            #region Assign Level Sheet based on Level.Type Check

            if (Level.type == LevelType.Castle)
            { Obj.compSprite.texture = Assets.forestLevelSheet; }
            //expand this to include all dungeon textures...
            //else if (Level.type == LevelType.Shop)
            //{ Obj.compSprite.texture = Assets.forestLevelSheet; }

            //below in type checks, particles/projectiles/pickups switch their texture to Entity Sheet
            //vendor advertisements set their texture to the UiItems Sheet

            #endregion



            //Dungeon Objects

            #region Exits

            if (Type == ObjType.Dungeon_ExitPillarLeft ||
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
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_DoorTrap;
            }
            else if (Type == ObjType.Dungeon_DoorBombable)
            {
                Obj.compSprite.zOffset = -32; //sort very low (behind hero)
                Obj.group = ObjGroup.Door;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_DoorTrap;
            }
            else if(Type == ObjType.Dungeon_DoorBoss)
            {
                Obj.compSprite.zOffset = -32; //sort very low (behind hero)
                Obj.group = ObjGroup.Door;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_DoorBoss;
            }
            else if (Type == ObjType.Dungeon_DoorShut)
            {
                Obj.compSprite.zOffset = -32; //sort very low (behind hero)
                Obj.group = ObjGroup.Door;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_DoorShut;
            }
            else if (Type == ObjType.Dungeon_DoorFake)
            {
                Obj.compSprite.zOffset = -32; //sort very low (behind hero)
                Obj.group = ObjGroup.Door;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_DoorFake;
            }

            #endregion


            #region Walls

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
                Obj.compSprite.zOffset = -32; //sort to floor
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
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -4;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 12;
                Obj.compSprite.zOffset = -7;
                Obj.canBeSaved = true;
                Obj.compMove.moveable = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_Pot;
            }
            else if (Type == ObjType.Dungeon_Barrel)
            {
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 13;
                Obj.compSprite.zOffset = -7;
                Obj.canBeSaved = true;
                Obj.compMove.moveable = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_Barrel;
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

            else if (Type == ObjType.Dungeon_Switch || Type == ObjType.Dungeon_SwitchOff)
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

            #region Shop Objects

            else if (Type == ObjType.World_Bookcase || Type == ObjType.World_Shelf)
            {
                Obj.compCollision.offsetY = 0; Obj.compCollision.rec.Height = 8;
                Obj.compSprite.zOffset = -7;
                Obj.canBeSaved = true;
                if (Type == ObjType.World_Bookcase)
                { Obj.compAnim.currentAnimation = AnimationFrames.World_Bookcase; }
                else { Obj.compAnim.currentAnimation = AnimationFrames.World_Shelf; }
            }
            else if (Type == ObjType.World_TableStone)
            {
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -7;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 14;
                Obj.compSprite.zOffset = -7;
                Obj.canBeSaved = true;
                Obj.compAnim.currentAnimation = AnimationFrames.World_TableStone;
            }

            #endregion


            #region Vendors + Vendor Advertisements

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

                if (Type == ObjType.Vendor_NPC_Items) { Obj.compAnim.currentAnimation = AnimationFrames.Vendor_Items; }
                else if (Type == ObjType.Vendor_NPC_Potions) { Obj.compAnim.currentAnimation = AnimationFrames.Vendor_Potions; }
                else if (Type == ObjType.Vendor_NPC_Magic) { Obj.compAnim.currentAnimation = AnimationFrames.Vendor_Magic; }
                else if (Type == ObjType.Vendor_NPC_Weapons) { Obj.compAnim.currentAnimation = AnimationFrames.Vendor_Weapons; }
                else if (Type == ObjType.Vendor_NPC_Armor) { Obj.compAnim.currentAnimation = AnimationFrames.Vendor_Armor; }
                else if (Type == ObjType.Vendor_NPC_Equipment) { Obj.compAnim.currentAnimation = AnimationFrames.Vendor_Equipment; }
                else if (Type == ObjType.Vendor_NPC_Pets) { Obj.compAnim.currentAnimation = AnimationFrames.Vendor_Pets; }
                else if (Type == ObjType.Vendor_NPC_Story) { Obj.compAnim.currentAnimation = AnimationFrames.Vendor_Story; }
            }

            //advertisements are on the UiItems sheet
            else if (Type == ObjType.Vendor_Ad_Armor || Type == ObjType.Vendor_Ad_Equipment
                || Type == ObjType.Vendor_Ad_Items || Type == ObjType.Vendor_Ad_Magics
                || Type == ObjType.Vendor_Ad_Pets || Type == ObjType.Vendor_Ad_Potions
                || Type == ObjType.Vendor_Ad_Story || Type == ObjType.Vendor_Ad_Weapons)
            {
                Obj.compSprite.texture = Assets.uiItemsSheet;
                Obj.compCollision.blocking = false;
                Obj.compSprite.zOffset = 32;
                Obj.compAnim.speed = 100; //very slow animation
                //set animation frames
                if (Type == ObjType.Vendor_Ad_Armor) { Obj.compAnim.currentAnimation = AnimationFrames.Vendor_Ad_Armor; }
                else if (Type == ObjType.Vendor_Ad_Equipment) { Obj.compAnim.currentAnimation = AnimationFrames.Vendor_Ad_Armor; }
                else if (Type == ObjType.Vendor_Ad_Items) { Obj.compAnim.currentAnimation = AnimationFrames.Vendor_Ad_Armor; }
                else if (Type == ObjType.Vendor_Ad_Magics) { Obj.compAnim.currentAnimation = AnimationFrames.Vendor_Ad_Armor; }
                else if (Type == ObjType.Vendor_Ad_Pets) { Obj.compAnim.currentAnimation = AnimationFrames.Vendor_Ad_Armor; }
                else if (Type == ObjType.Vendor_Ad_Potions) { Obj.compAnim.currentAnimation = AnimationFrames.Vendor_Ad_Armor; }
                else if (Type == ObjType.Vendor_Ad_Story) { Obj.compAnim.currentAnimation = AnimationFrames.Vendor_Ad_Armor; }
                else if (Type == ObjType.Vendor_Ad_Weapons) { Obj.compAnim.currentAnimation = AnimationFrames.Vendor_Ad_Armor; }
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

            else if (Type == ObjType.ProjectileSword)
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
                Obj.compAnim.currentAnimation = AnimationFrames.Projectile_Sword;
                Obj.compSprite.texture = Assets.entitiesSheet;
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
            }

            #endregion



            //Particles

            #region Particles - Dungeon Specific

            //these particle's sprites live on a dungeon sheet,
            //whichever dungeon sheet is the current one

            else if (Type == ObjType.ParticlePitAnimation)
            {
                Obj.compSprite.cellSize.X = 8; Obj.compSprite.cellSize.Y = 8; //nonstandard size
                Obj.compSprite.zOffset = -63; //sort over pits, under pit teeth
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 10 * 4 * 6; //speed * anim frames * loops
                Obj.compAnim.speed = 10; //in frames
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_PitBubble;
                //not on the entities sheet, on dungeon sheet
                Obj.compSprite.texture = Assets.forestLevelSheet;
            }

            #endregion


            #region Particles - Small

            else if (Type == ObjType.Particle_RisingSmoke)
            {
                Obj.compSprite.cellSize.X = 8; Obj.compSprite.cellSize.Y = 8; //nonstandard size
                Obj.compSprite.zOffset = -8;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 24; //in frames
                Obj.compAnim.speed = 6; //in frames
                Obj.compAnim.loop = false;
                Obj.compAnim.currentAnimation = AnimationFrames.Particle_RisingSmoke;
                Obj.compSprite.texture = Assets.entitiesSheet;
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

            #endregion


            #region Particles - Normal

            else if (Type == ObjType.Particle_Explosion)
            {
                Obj.compSprite.zOffset = 16;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 24; //in frames
                Obj.compAnim.speed = 5; //in frames
                Obj.compAnim.loop = false;
                Obj.compAnim.currentAnimation = AnimationFrames.Particle_Explosion;
                Obj.compSprite.texture = Assets.entitiesSheet;
            }
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
            else if (Type == ObjType.Particle_FireGround)
            {
                Obj.compSprite.zOffset = -8; //to ground
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 100; //in frames
                Obj.compAnim.speed = 7; //in frames
                Obj.compAnim.currentAnimation = AnimationFrames.Particle_FireGround;
                Obj.compSprite.texture = Assets.entitiesSheet;
            }
            else if (Type == ObjType.Particle_Splash)
            {
                Obj.compSprite.zOffset = 1024;
                Obj.group = ObjGroup.Particle;
                Obj.compAnim.speed = 10; //in frames
                Obj.lifetime = 10 * 5; //speed * animFrames
                Obj.compAnim.loop = false;
                Obj.compAnim.currentAnimation = AnimationFrames.Particle_Splash;
                Obj.compSprite.texture = Assets.entitiesSheet;
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
                Obj.compSprite.texture = Assets.entitiesSheet;
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
                //Obj.compCollision.blocking = false; //pets don't block

                //set initial animation frames for pets
                if (Type == ObjType.Pet_Dog)
                { Obj.compAnim.currentAnimation = AnimationFrames.Pet_Dog_Idle; }
                else if (Type == ObjType.Pet_Chicken)
                { Obj.compAnim.currentAnimation = AnimationFrames.Pet_Chicken_Idle; }

                Obj.compSprite.texture = Assets.petsSheet;
            }

            #endregion






            //Handle Obj Group properties
            if (Obj.group == ObjGroup.Pickup ||
                Obj.group == ObjGroup.Particle || 
                Obj.group == ObjGroup.Projectile)
            { Obj.compCollision.blocking = false; } //entities never block

            SetRotation(Obj);
            Functions_Component.UpdateCellSize(Obj.compSprite);
            Obj.compSprite.currentFrame = Obj.compAnim.currentAnimation[0]; //goto 1st anim frame
            Functions_Component.Align(Obj.compMove, Obj.compSprite, Obj.compCollision);
        }


    }
}