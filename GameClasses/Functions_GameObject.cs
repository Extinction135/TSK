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
        static int i;



        public static GameObject Spawn(ObjType Type, float X, float Y, Direction Direction)
        {   //spawns obj at the X, Y location, with direction
            GameObject obj = Functions_Pool.GetRoomObj();
            obj.direction = Direction;
            obj.compMove.direction = Direction;
            Functions_Movement.Teleport(obj.compMove, X, Y);
            SetType(obj, Type);
            return obj;
        }




        //power level 1 obj destruction

        public static void HandleCommon(GameObject RoomObj, Direction HitDirection)
        {
            //roomObj is blocking, interacted with arrow, explosion, sword/shovel, thrown bush/pot, etc..
            //hitDirection is used to push some objects in the direction they were hit

            #region World Objects

            if (RoomObj.type == ObjType.Wor_Pot)
            {
                RoomObj.compMove.direction = HitDirection;
                Kill(RoomObj, true, true);
            }
            else if(RoomObj.type == ObjType.Wor_Bush)
            {
                RoomObj.compMove.direction = HitDirection; 
                Functions_GameObject_World.DestroyBush(RoomObj);
            }
            else if (RoomObj.type == ObjType.Wor_Build_Door_Shut)
            {
                Functions_GameObject_World.OpenBuildingDoor(RoomObj);
            }

            //burned posts
            else if(
                RoomObj.type == ObjType.Wor_PostBurned_Corner_Left ||
                RoomObj.type == ObjType.Wor_PostBurned_Corner_Right ||
                RoomObj.type == ObjType.Wor_PostBurned_Horizontal ||
                RoomObj.type == ObjType.Wor_PostBurned_Vertical_Left ||
                RoomObj.type == ObjType.Wor_PostBurned_Vertical_Right
                )
            {
                Kill(RoomObj, true, true);
            }
            //boat barrels 
            else if(RoomObj.type == ObjType.Wor_Boat_Barrel)
            {
                Kill(RoomObj, true, true);
            }

            #endregion


            #region World Enemies

            else if (RoomObj.type == ObjType.Wor_Enemy_Turtle
                || RoomObj.type == ObjType.Wor_Enemy_Crab
                || RoomObj.type == ObjType.Wor_Enemy_Rat)
            {
                Functions_Particle.Spawn(ObjType.Particle_Attention, RoomObj);
                Kill(RoomObj, true, false);
            }
            else if(RoomObj.type == ObjType.Wor_SeekerExploder)
            {   //inherit inertia from hit
                RoomObj.compMove.direction = HitDirection;
                //become an explosion
                SetType(RoomObj, ObjType.ExplodingObject);
                Functions_Movement.Push(RoomObj.compMove, RoomObj.compMove.direction, 6.0f);
            }

            #endregion


            #region Dungeon Objects

            else if (RoomObj.type == ObjType.Dungeon_Pot)
            {
                RoomObj.compMove.direction = HitDirection;
                Kill(RoomObj, true, true);
            }
            else if (RoomObj.type == ObjType.Dungeon_Barrel)
            {
                RoomObj.compMove.direction = HitDirection;
                Functions_GameObject_Dungeon.HitBarrel(RoomObj);
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

            #endregion

        }
        

        //power level 2 obj destruction

        public static void BlowUp(GameObject Obj, GameObject Pro)
        {
            //note: only explosion and lightning bolt projectiles call this method
            //they are the only power level 2 projectiles


            #region Dungeon Objs - special cases

            if (Obj.type == ObjType.Dungeon_DoorBombable)
            {   //collapse doors
                Functions_GameObject_Dungeon.CollapseDungeonDoor(Obj, Pro);
            }
            else if (Obj.type == ObjType.Dungeon_WallStraight)
            {   //'crack' normal walls
                Functions_GameObject.SetType(Obj,
                    ObjType.Dungeon_WallStraightCracked);
                Functions_Particle.Spawn(ObjType.Particle_Blast,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y);
                Assets.Play(Assets.sfxShatter);
            }
            else if (Obj.type == ObjType.Dungeon_TorchUnlit)
            {   //light torches on fire
                Functions_GameObject_Dungeon.LightTorch(Obj);
            }

            #endregion
            

            #region World Objs - special cases

            else if (Obj.type == ObjType.Wor_Bush)
            {   //destroy the bush
                Functions_GameObject_World.DestroyBush(Obj);
                //set a ground fire ON the stump sprite
                Functions_Projectile.Spawn(
                    ObjType.ProjectileGroundFire,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y - 4,
                    Direction.None);
            }
            else if (Obj.type == ObjType.Wor_Tree || Obj.type == ObjType.Wor_Tree_Burning)
            {   //blow up tree, showing leaf explosion
                Functions_GameObject_World.BlowUpTree(Obj, true);
            }
            else if (Obj.type == ObjType.Wor_Tree_Burnt)
            {   //blow up tree, no leaf explosion
                Functions_GameObject_World.BlowUpTree(Obj, false);
            }

            else if (
                //posts + burned posts
                Obj.type == ObjType.Wor_PostBurned_Corner_Left
                || Obj.type == ObjType.Wor_PostBurned_Corner_Right
                || Obj.type == ObjType.Wor_PostBurned_Horizontal
                || Obj.type == ObjType.Wor_PostBurned_Vertical_Left
                || Obj.type == ObjType.Wor_PostBurned_Vertical_Right
                || Obj.type == ObjType.Wor_Post_Corner_Left
                || Obj.type == ObjType.Wor_Post_Corner_Right
                || Obj.type == ObjType.Wor_Post_Horizontal
                || Obj.type == ObjType.Wor_Post_Vertical_Left
                || Obj.type == ObjType.Wor_Post_Vertical_Right
                )
            {
                Functions_GameObject_World.BlowUpPost(Obj);
            }



            #endregion


            #region Objs - General cases

            else if (

                //dungeon objs

                //limited set for now
                Obj.type == ObjType.Dungeon_Statue
                || Obj.type == ObjType.Dungeon_Signpost

                //world objs

                //building objs
                || Obj.type == ObjType.Wor_Build_Wall_FrontA
                || Obj.type == ObjType.Wor_Build_Wall_FrontB
                || Obj.type == ObjType.Wor_Build_Wall_Back
                || Obj.type == ObjType.Wor_Build_Wall_Side_Left
                || Obj.type == ObjType.Wor_Build_Wall_Side_Right
                || Obj.type == ObjType.Wor_Build_Door_Shut
                || Obj.type == ObjType.Wor_Build_Door_Open
                //building interior objs
                || Obj.type == ObjType.Wor_Bookcase
                || Obj.type == ObjType.Wor_Shelf
                || Obj.type == ObjType.Wor_Stove
                || Obj.type == ObjType.Wor_Sink
                || Obj.type == ObjType.Wor_TableSingle
                || Obj.type == ObjType.Wor_TableDoubleLeft
                || Obj.type == ObjType.Wor_TableDoubleRight
                || Obj.type == ObjType.Wor_Chair
                || Obj.type == ObjType.Wor_Bed
                )
            {
                Kill(Obj, true, true);
            }

            #endregion


            else
            {   //trigger common obj interactions too
                HandleCommon(Obj, //get direction towards roomObj from pro/explosion
                    Functions_Direction.GetOppositeCardinal(
                        Obj.compSprite.position,
                        Pro.compSprite.position)
                );
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
                BecomeDebris(Obj);
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
            Obj.type = ObjType.Wor_Bush; //reset the type
            Obj.direction = Direction.Down;
            Obj.active = true; //assume this object should draw / animate
            Obj.getsAI = false; //most objects do not get any AI input
            Obj.canBeSaved = false; //most objects cannot be saved as XML data
            Obj.lifetime = 0; //assume obj exists forever (not projectile)
            Obj.lifeCounter = 0; //reset counter
            Obj.interactiveFrame = 0;

            //reset the sprite component
            Obj.compSprite.drawRec.Width = 16 * 1; //assume cell size is 16x16 (most are)
            Obj.compSprite.drawRec.Height = 16 * 1;
            Obj.compSprite.zOffset = 0;
            Obj.compSprite.flipHorizontally = false;
            Obj.compSprite.rotation = Rotation.None;
            Obj.compSprite.scale = 1.0f;
            Obj.compSprite.texture = Assets.CommonObjsSheet;
            Obj.compSprite.visible = true;

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
            else if (
                //roomObjs
                Obj.type == ObjType.Dungeon_PitTrap
                || Obj.type == ObjType.Dungeon_PitTeethBottom
                || Obj.type == ObjType.Dungeon_PitTeethTop
                //pros
                || Obj.type == ObjType.ProjectileBomb
                || Obj.type == ObjType.ProjectilePot
                || Obj.type == ObjType.ProjectilePotSkull
                || Obj.type == ObjType.ProjectileBush
                || Obj.type == ObjType.ProjectileBoomerang
                || Obj.type == ObjType.ProjectileBat
                )
            {   //some objects only face Direction.Down
                Obj.direction = Direction.Down;
            }

            else if(Obj.type == ObjType.Dungeon_FloorBlood
                || Obj.type == ObjType.Dungeon_PitTrap)
            {   //some objects are randomly flipped horizontally
                Obj.compSprite.flipHorizontally = true;
            }

            //set sprite's rotation based on direction & flipHorizontally boolean
            Functions_Component.SetSpriteRotation(Obj.compSprite, Obj.direction);

            //some objects override the sprite rotation set above
            if (Obj.type == ObjType.ProjectileLightningBolt)
            {   //align lightning bolts vertically or horizontally
                if (Obj.direction == Direction.Left || Obj.direction == Direction.Right)
                { Obj.compSprite.rotation = Rotation.Clockwise90; }
                else { Obj.compSprite.rotation = Rotation.None; }
            }
        }

        public static void Update(GameObject Obj)
        {   //only roomObjs are passed into this method, some get AI (or behaviors)
            //roomObjs don't have lifetimes, they last the life of the room
            if (Obj.getsAI) { Functions_Ai.HandleObj(Obj); }
        }

        public static void BecomeDebris(GameObject Obj)
        {
            //become permanent debris w/ 16x16 collisions
            SetType(Obj, ObjType.Wor_Debris);
            Obj.compCollision.rec.Width = 16;
            Obj.compCollision.rec.Height = 16;
            Obj.compCollision.rec.X = (int)Obj.compSprite.position.X - 8;
            Obj.compCollision.rec.Y = (int)Obj.compSprite.position.Y - 8;

            //check to see if this debris can be placed here, if not - release
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active & Pool.roomObjPool[i] != Obj)
                {
                    //check for overlap / interaction
                    if (Pool.roomObjPool[i].compCollision.rec.Intersects(Obj.compCollision.rec))
                    {
                        //blocking objs take visual priority over debris
                        if (Pool.roomObjPool[i].compCollision.blocking)
                        { Functions_Pool.Release(Obj); }
                        if (
                            //there can only be one debris obj at a tile at a time
                            Pool.roomObjPool[i].type == ObjType.Wor_Debris
                            //these objs take visual priority over debris
                            || Pool.roomObjPool[i].type == ObjType.Wor_Flowers
                            || Pool.roomObjPool[i].type == ObjType.Wor_Grass_Tall
                            || Pool.roomObjPool[i].type == ObjType.Wor_Bush_Stump
                            || Pool.roomObjPool[i].type == ObjType.Wor_Tree_Stump
                        )
                        { Functions_Pool.Release(Obj); }
                    }
                }
            }
        }




        
        public static void SetType(GameObject Obj, ObjType Type)
        {   //Obj.direction should be set prior to this method running - important

            //set the type and texture first
            Obj.type = Type;




            //this obj is either on common objs sheet, or dungeon sheet
            //certain obj enemies are on the enemies sheet too
            //or is entity (pro, part, etc..)




            //these objs inherit their texture sheets from prev obj state
            if (Obj.type == ObjType.ExplodingObject) { }
            //default to the common objs sheet - obj def will overwrite this if needed
            else { Obj.compSprite.texture = Assets.CommonObjsSheet; }
            
            //below in type checks, objs/particles/projectiles/pickups 
            //switch their textures to whatever sheet they need






            #region Unknown Obj

            if (Type == ObjType.Unknown)
            {   
                ResetObject(Obj);
                Obj.type = ObjType.Unknown;
                Obj.compCollision.blocking = false;
                Obj.compSprite.texture = Assets.uiItemsSheet;
                Obj.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Unknown;
                Obj.compSprite.zOffset = -64; //sort below everything else
            }

            #endregion


            //Dungeon Objects


            #region Exits

            else if (Type == ObjType.Dungeon_ExitPillarLeft ||
               Type == ObjType.Dungeon_ExitPillarRight)
            {
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                Obj.compSprite.drawRec.Height = 16 * 3; //nonstandard size
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
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                Obj.compSprite.drawRec.Height = 16 * 3; //nonstandard size
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.group = ObjGroup.Door;

                Obj.compCollision.rec.Height = 8;
                Obj.compCollision.offsetY = 32;

                Obj.compCollision.blocking = false;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_Exit;
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }
            else if (Type == ObjType.Dungeon_ExitLight)
            {
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                Obj.compSprite.drawRec.Height = 16 * 2; //nonstandard size
                Obj.compCollision.offsetY = 0;
                Obj.compSprite.zOffset = 256; //sort above everything
                Obj.compCollision.blocking = false;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_ExitLight;
            }

            #endregion


            #region Doors  

            else if (Type == ObjType.Dungeon_DoorOpen)
            {
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                Obj.compSprite.zOffset = +32; //sort very high (over / in front of hero)
                Obj.group = ObjGroup.Door;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_DoorOpen;
            }
            else if(Type == ObjType.Dungeon_DoorTrap)
            {
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                Obj.compCollision.blocking = false;
                Obj.compSprite.zOffset = -32; //sort very low (behind hero)
                Obj.group = ObjGroup.Door;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_DoorShut;
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }
            else if (Type == ObjType.Dungeon_DoorBombable)
            {
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                Obj.compSprite.zOffset = -32; //sort very low (behind hero)
                Obj.group = ObjGroup.Door;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_WallStraightCracked;
                Obj.sfx.hit = Assets.sfxTapHollow; //sounds hollow
            }
            else if(Type == ObjType.Dungeon_DoorBoss)
            {
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                Obj.compSprite.zOffset = -32; //sort very low (behind hero)
                Obj.group = ObjGroup.Door;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_DoorBoss;
                Obj.sfx.hit = Assets.sfxTapMetallic;
                Obj.sfx.kill = Assets.sfxShatter;
            }
            else if (Type == ObjType.Dungeon_DoorShut)
            {
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                Obj.compSprite.zOffset = -32; //sort very low (behind hero)
                Obj.group = ObjGroup.Door;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_DoorShut;
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }
            else if (Type == ObjType.Dungeon_DoorFake)
            {
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
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
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                Obj.compSprite.zOffset = -32; //sort very low (behind hero)
                Obj.group = ObjGroup.Wall;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_WallStraight;
            }
            else if(Type == ObjType.Dungeon_WallStraightCracked)
            {
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                Obj.compSprite.zOffset = -32; //sort very low (behind hero)
                Obj.group = ObjGroup.Wall;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_WallStraightCracked;
            }
            else if (Type == ObjType.Dungeon_WallInteriorCorner)
            {
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                Obj.compSprite.zOffset = -32; //sort very low (behind hero)
                Obj.group = ObjGroup.Wall;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_WallInteriorCorner;
            }
            else if (Type == ObjType.Dungeon_WallExteriorCorner)
            {
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                Obj.compSprite.zOffset = -32; //sort very low (behind hero)
                Obj.group = ObjGroup.Wall;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_WallExteriorCorner;
            }
            else if (Type == ObjType.Dungeon_WallPillar)
            {
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                Obj.compSprite.zOffset = -32; //sort very low (behind hero)
                Obj.group = ObjGroup.Wall;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_WallPillar;
            }
            else if (Type == ObjType.Dungeon_WallStatue)
            {
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                Obj.compSprite.zOffset = -16; //sort low, but over walls
                Obj.group = ObjGroup.Wall;
                Obj.getsAI = true; //obj gets AI
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_WallStatue;
            }
            else if (Type == ObjType.Dungeon_WallTorch)
            {
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                Obj.compSprite.zOffset = -16; //sort low, but over walls
                Obj.group = ObjGroup.Wall;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_WallTorch;
            }

            #endregion


            #region Boss Decal + Floor Blood

            else if (Type == ObjType.Dungeon_FloorDecal)
            {
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is unique dungeonObj
                Obj.compSprite.zOffset = -32; //sort low, but over floor
                Obj.compCollision.blocking = false;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_FloorDecal;
            }
            else if(Type == ObjType.Dungeon_FloorBlood)
            {
                //Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is common dungeonObj
                //collision rec is smaller so more debris is left when room is cleanedUp()
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 10;
                Obj.compSprite.zOffset = -32; //sort low, but over floor
                Obj.compCollision.blocking = false;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_FloorBlood;
            }

            #endregion


            #region Pits

            else if (Type == ObjType.Dungeon_Pit)
            {
                //Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is common dungeonObj
                //this pit interacts with actor
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
                //Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is common dungeonObj
                Obj.compCollision.blocking = false;
                Obj.compSprite.zOffset = -64; //sort to floor
                Obj.canBeSaved = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_PitBridge;
            }
            else if (Type == ObjType.Dungeon_PitTeethTop)
            {
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                Obj.compSprite.zOffset = -60; //sort above pits & pit bubbles
                Obj.compCollision.rec.Width = 16; Obj.compCollision.rec.Height = 8;
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_PitTeethTop;
            }
            else if(Type == ObjType.Dungeon_PitTeethBottom)
            {
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                Obj.compSprite.zOffset = -60; //sort above pits & pit bubbles
                Obj.compCollision.rec.Width = 16; Obj.compCollision.rec.Height = 8;
                Obj.compCollision.offsetY = 4;
                Obj.compCollision.rec.Height = 4;
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_PitTeethBottom;
            }
            else if (Type == ObjType.Dungeon_PitTrap)
            {   //modeled as 'floor crack' sprite
                //Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is common dungeonObj
                //this becomes a pit upon collision with hero
                Obj.compCollision.offsetX = -12; Obj.compCollision.offsetY = -12;
                Obj.compCollision.rec.Width = 24; Obj.compCollision.rec.Height = 24;
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_FloorCracked;
            }

            #endregion


            #region BossStatue & Skull Pillar

            else if (Type == ObjType.Dungeon_Statue)
            {
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                Obj.compCollision.rec.Height = 8;
                Obj.compCollision.offsetY = -1;
                Obj.canBeSaved = true;
                Obj.compMove.moveable = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_Statue;
                Obj.sfx.hit = Assets.sfxTapHollow;
                Obj.sfx.kill = Assets.sfxShatter;
            }
            else if (Type == ObjType.Dungeon_SkullPillar)
            {
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                Obj.compSprite.drawRec.Height = 16 * 2; //nonstandard size
                Obj.compSprite.zOffset = +10;
                Obj.compCollision.offsetY = +8;
                Obj.compMove.moveable = false;
                Obj.canBeSaved = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_SkullPillar;
                Obj.sfx.hit = Assets.sfxTapMetallic;
                Obj.sfx.kill = Assets.sfxShatter;
            }

            #endregion


            #region Chest + Map

            else if (Type == ObjType.Dungeon_Chest || 
                Type == ObjType.Dungeon_ChestKey)
            {
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
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
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -3;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 11;
                Obj.compSprite.zOffset = -7;
                Obj.canBeSaved = true;
                //Obj.group = ObjGroup.Chest; //not really a chest, just obj
                Obj.compMove.moveable = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_ChestOpened;
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }
            else if(Type == ObjType.Dungeon_Map)
            {
                //Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is common dungeonObj
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_Map;
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 10;
                Obj.compSprite.zOffset = 0;
                //hero simply touches map to collect it
                Obj.compCollision.blocking = false; 
            }

            #endregion


            #region Blocks

            else if (Type == ObjType.Dungeon_BlockDark || Type == ObjType.Dungeon_BlockLight)
            {
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                Obj.compSprite.zOffset = -7;
                Obj.canBeSaved = true;
                if (Type == ObjType.Dungeon_BlockLight)
                {   //lighter blocks are moveable
                    Obj.compMove.moveable = true;
                    Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_BlockLight;
                }
                else { Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_BlockDark; }
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }
            else if (Type == ObjType.Dungeon_BlockSpike)
            {
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
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
                //Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is common dungeonObj
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = 2;
                Obj.compCollision.rec.Width = 12; Obj.compCollision.rec.Height = 5;
                Obj.compSprite.zOffset = -3;
                Obj.canBeSaved = true;
                if (Type == ObjType.Dungeon_LeverOn)
                { Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_LeverOn; }
                else { Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_LeverOff; }
            }
            else if (Type == ObjType.Dungeon_SpikesFloorOn || Type == ObjType.Dungeon_SpikesFloorOff)
            {
                //Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is common dungeonObj
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
                //Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is common dungeonObj
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
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
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
                //Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is common dungeonObj
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
                //Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is common dungeonObj
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_Bumper;
            }
            else if (Type == ObjType.Dungeon_Flamethrower)
            {
                //Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is common dungeonObj
                Obj.compSprite.zOffset = -30; //sort slightly above floor
                Obj.compCollision.offsetX = -4; Obj.compCollision.offsetY = -4;
                Obj.compCollision.rec.Width = 8; Obj.compCollision.rec.Height = 8;
                Obj.compCollision.blocking = false;
                Obj.getsAI = true; //obj gets AI
                Obj.canBeSaved = true;
                Obj.compMove.moveable = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_Flamethrower;
            }
            else if (Type == ObjType.Dungeon_IceTile)
            {
                //Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is common dungeonObj
                Obj.compCollision.offsetX = -6; Obj.compCollision.offsetY = -6;
                Obj.compCollision.rec.Width = 12; Obj.compCollision.rec.Height = 12;
                Obj.compSprite.zOffset = -30; //sort a little above floor
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_IceTile;
            }

            #endregion


            #region Floor Switches

            else if (Type == ObjType.Dungeon_Switch || Type == ObjType.Dungeon_SwitchDown
                || Type == ObjType.Dungeon_SwitchDownPerm)
            {
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                Obj.compCollision.offsetX = -4; Obj.compCollision.offsetY = -3;
                Obj.compCollision.rec.Width = 8; Obj.compCollision.rec.Height = 8;
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
                Obj.compSprite.zOffset = -32; //sort to floor
                //Obj.compMove.moveable = true;
                if (Type == ObjType.Dungeon_Switch)
                { Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_FloorSwitchUp; }
                else { Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_FloorSwitchDown; }
                //this makes the switch work
                Obj.getsAI = true;
                if (Type == ObjType.Dungeon_SwitchDownPerm) { Obj.getsAI = false; }
            }

            #endregion


            #region Switch Blocks & Button

            else if (Type == ObjType.Dungeon_SwitchBlockBtn)
            {
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
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
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_SwitchBlockDown;
            }
            else if (Type == ObjType.Dungeon_SwitchBlockUp)
            {
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
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
                //Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is common dungeonObj
                Obj.compCollision.offsetX = -8; Obj.compCollision.offsetY = -4;
                Obj.compCollision.rec.Width = 16; Obj.compCollision.rec.Height = 12;
                Obj.compSprite.zOffset = -7;
                Obj.canBeSaved = true;
                Obj.compMove.moveable = true;
                if (Type == ObjType.Dungeon_TorchUnlit)
                {
                    Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_TorchUnlit;
                    Obj.getsAI = true; //unlit torches check neighbors for fire
                    Obj.interactiveFrame = 10; //only does this ever 10 frames
                }
                else
                {
                    Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_TorchLit;
                }
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }

            #endregion


            #region Living RoomObjects - Fairy

            else if (Type == ObjType.Dungeon_Fairy)
            {
                //Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is common dungeonObj
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

            else if (Type == ObjType.Dungeon_SpawnMob)
            {
                //Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is common dungeonObj
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

            else if (Type == ObjType.Wor_Grass_2
                || Type == ObjType.Wor_Grass_Cut || Type == ObjType.Wor_Grass_Tall
                || Type == ObjType.Wor_Flowers)
            {
                Obj.compSprite.zOffset = -32;
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
                Obj.compAnim.loop = true;

                //set animation frame
                if (Type == ObjType.Wor_Grass_Cut)
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

                Obj.getsAI = true; //stumps grow to become bushes if irrigated
                //how quickly stumps check for nearby filled ditches
                Obj.interactiveFrame = 254; //last possible frame
            }
            else if (Type == ObjType.Wor_Tree 
                || Type == ObjType.Wor_Tree_Burning 
                || Type == ObjType.Wor_Tree_Burnt)
            {
                Obj.compSprite.drawRec.Height = 16 * 2; //nonstandard size
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
                Obj.compSprite.drawRec.Height = 16 * 2; //nonstandard size
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
                //reset cell size
                Obj.compSprite.drawRec.Width = 16 * 1;
                Obj.compSprite.drawRec.Height = 16 * 1;

                //reset hitbox
                Obj.compCollision.rec.Width = 16;
                Obj.compCollision.rec.Height = 16;
                Obj.compCollision.offsetX = -8;
                Obj.compCollision.offsetY = -8;

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


            #region Pot

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

            else if (Type == ObjType.Wor_Bookcase 
                || Type == ObjType.Wor_Shelf
                || Type == ObjType.Wor_Stove
                || Type == ObjType.Wor_Sink
                || Type == ObjType.Wor_Chair)
            {
                Obj.compCollision.offsetY = 0; Obj.compCollision.rec.Height = 8;
                Obj.compSprite.zOffset = -7;
                Obj.canBeSaved = true;
                Obj.compMove.moveable = true;

                if (Type == ObjType.Wor_Bookcase)
                { Obj.compAnim.currentAnimation = AnimationFrames.World_Bookcase; }
                else if(Type == ObjType.Wor_Shelf)
                { Obj.compAnim.currentAnimation = AnimationFrames.World_Shelf; }

                else if (Type == ObjType.Wor_Stove)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Stove; }
                else if (Type == ObjType.Wor_Sink)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Sink; }
                else if (Type == ObjType.Wor_Chair)
                {
                    Obj.compAnim.currentAnimation = AnimationFrames.Wor_Chair;
                    Obj.compCollision.offsetX = -5; Obj.compCollision.rec.Width = 10;
                    Obj.compCollision.offsetY = -5; Obj.compCollision.rec.Height = 10;
                }

                Obj.sfx.hit = Assets.sfxTapMetallic;
                Obj.sfx.kill = Assets.sfxShatter;
            }
            else if (Type == ObjType.Wor_TableSingle )
            {
                Obj.canBeSaved = true;
                Obj.compMove.moveable = true;
                Obj.sfx.hit = Assets.sfxTapMetallic;
                Obj.sfx.kill = Assets.sfxShatter;
                Obj.compSprite.zOffset = -7;
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -6;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 12;
                Obj.compAnim.currentAnimation = AnimationFrames.World_TableSingle;
            }

            else if(Type == ObjType.Wor_TableDoubleLeft
                || Type == ObjType.Wor_TableDoubleRight)
            {
                Obj.canBeSaved = true;
                //Obj.compMove.moveable = true;
                Obj.sfx.hit = Assets.sfxTapMetallic;
                Obj.sfx.kill = Assets.sfxShatter;
                Obj.compSprite.zOffset = -7;
                
                if (Type == ObjType.Wor_TableDoubleLeft)
                {
                    Obj.compAnim.currentAnimation = AnimationFrames.World_TableDoubleLeft;
                    Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -6;
                    Obj.compCollision.rec.Width = 14 + 16; Obj.compCollision.rec.Height = 12;
                }
                else
                {
                    Obj.compAnim.currentAnimation = AnimationFrames.World_TableDoubleRight;
                    Obj.compCollision.offsetX = -7-16; Obj.compCollision.offsetY = -6;
                    Obj.compCollision.rec.Width = 14+16; Obj.compCollision.rec.Height = 12;
                }
            }

            else if(Type == ObjType.Wor_Bed)
            {
                Obj.compSprite.drawRec.Height = 16*2; //non standard cellsize
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Bed;
                Obj.compCollision.offsetX = -8; Obj.compCollision.offsetY = 0;
                Obj.compCollision.rec.Width = 16; Obj.compCollision.rec.Height = 3 + 16;
                Obj.compSprite.zOffset = 0;
                Obj.canBeSaved = true;
                Obj.sfx.hit = null;
                Obj.sfx.kill = null;
            }

            #endregion


            #region Vendors

            //Vendors
            else if (Type == ObjType.Vendor_NPC_Items || Type == ObjType.Vendor_NPC_Potions ||
                Type == ObjType.Vendor_NPC_Magic || Type == ObjType.Vendor_NPC_Weapons ||
                Type == ObjType.Vendor_NPC_Armor || Type == ObjType.Vendor_NPC_Equipment
                || Type == ObjType.Vendor_NPC_Pets)
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
            }
            else if(Type == ObjType.Vendor_Colliseum_Mob || Type == ObjType.Vendor_NPC_EnemyItems)
            {
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -3;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 11;
                Obj.compSprite.zOffset = 0;
                Obj.compAnim.speed = 20; //slow animation
                Obj.group = ObjGroup.Vendor;
                Obj.canBeSaved = true;

                if (Type == ObjType.Vendor_Colliseum_Mob)
                { Obj.compAnim.currentAnimation = AnimationFrames.Vendor_Colliseum_Mob; }
                else if (Type == ObjType.Vendor_NPC_EnemyItems)
                { Obj.compAnim.currentAnimation = AnimationFrames.Vendor_EnemyItems; }
            }

            #endregion


            #region NPCs

            //story NPC
            else if (Type == ObjType.NPC_Story)
            {
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -3;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 11;
                Obj.compSprite.zOffset = 0;
                Obj.compAnim.speed = 20; //slow animation
                Obj.group = ObjGroup.NPC;
                Obj.canBeSaved = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Vendor_Story;
            }
            
            //farmer NPC
            else if(Type == ObjType.NPC_Farmer 
                || Type == ObjType.NPC_Farmer_Reward 
                || Type == ObjType.NPC_Farmer_EndDialog)
            {
                //only end state of farmer is 'dumb'
                if (Type == ObjType.NPC_Farmer_EndDialog) { Obj.getsAI = false; }
                else { Obj.getsAI = true; } //initial + reward states get AI

                if (Type == ObjType.NPC_Farmer) //initial farmer checks often for
                { Obj.interactiveFrame = 30; } //completion condition, while reward
                else { Obj.interactiveFrame = 60; } //farmer gently spams exclamation point
                
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -3;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 11;
                Obj.compSprite.zOffset = 0;
                Obj.compAnim.speed = 20; //slow animation
                Obj.group = ObjGroup.NPC;
                Obj.canBeSaved = true;
                
                if (Type == ObjType.NPC_Farmer_Reward) //only reward has unique anim
                { Obj.compAnim.currentAnimation = AnimationFrames.NPC_Farmer_HandsUp; }
                else  { Obj.compAnim.currentAnimation = AnimationFrames.NPC_Farmer; }
            }

            //colliseum judge
            else if (Type == ObjType.Judge_Colliseum)
            {
                Obj.compAnim.currentAnimation = AnimationFrames.Vendor_Colliseum_Mob;
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -3;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 11;
                Obj.compSprite.zOffset = 0;
                Obj.compAnim.speed = 20; //slow animation
                Obj.group = ObjGroup.NPC;
                Obj.canBeSaved = false;

                //judge handles challenge completions
                Obj.getsAI = true;
                Obj.interactiveFrame = 60;
            }


            #endregion


            #region Water Objects

            else if (Type == ObjType.Wor_Water)
            {   //use less pool objs by making water tiles 2x2
                Obj.compSprite.zOffset = -128;
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Water;
                //double size
                Obj.compSprite.drawRec.Width = 16 * 2; 
                Obj.compSprite.drawRec.Height = 16 * 2; 
                Obj.compCollision.rec.Width = 16 * 2;
                Obj.compCollision.rec.Height = 16 * 2;
                Obj.compCollision.offsetX = -8;
                Obj.compCollision.offsetY = -8;
            }

            else if (Type == ObjType.Wor_Coastline_Straight
                || Type == ObjType.Wor_Coastline_Corner_Exterior
                || Type == ObjType.Wor_Coastline_Corner_Interior)
            {
                Obj.compSprite.zOffset = -40;
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
                Obj.compAnim.loop = true;

                if (Type == ObjType.Wor_Coastline_Straight)
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


            #region Ditches

            else if (
                Type == ObjType.Wor_Ditch_META

                || Type == ObjType.Wor_Ditch_Empty_Single
                || Type == ObjType.Wor_Ditch_Empty_4UP
                || Type == ObjType.Wor_Ditch_Empty_Vertical
                || Type == ObjType.Wor_Ditch_Empty_Horizontal

                || Type == ObjType.Wor_Ditch_Empty_Corner_North
                || Type == ObjType.Wor_Ditch_Empty_Corner_South
                || Type == ObjType.Wor_Ditch_Empty_3UP_North
                || Type == ObjType.Wor_Ditch_Empty_3UP_South

                || Type == ObjType.Wor_Ditch_Empty_3UP_Horizontal
                || Type == ObjType.Wor_Ditch_Empty_Endcap_South
                || Type == ObjType.Wor_Ditch_Empty_Endcap_Horizontal
                || Type == ObjType.Wor_Ditch_Empty_Endcap_North
                )
            {   //^ well that's efficient

                //ditches use the getsAI boolean for two purposes:
                //1. to model the empty and filled states of ditches
                //2. to spread water from filled to empty ditches via Ai.HandleObj()
                //so the empty state of a ditch is getsAI = false
                Obj.getsAI = false;

                //this sets how often filled ditches spread to empty ditches
                Obj.interactiveFrame = 15;
                //Obj.interactiveFrame += Functions_Random.Int(-10, 10); //vary spread times

                Obj.lifeCounter = 0; //used by AI.handleObj to count to interactive frame
                Obj.lifetime = 255; //not used in this case, but set to max value for safety

                Obj.compCollision.rec.Width = 16;
                Obj.compCollision.rec.Height = 16;
                Obj.compCollision.offsetX = -8;
                Obj.compCollision.offsetY = -8;

                Obj.canBeSaved = true;
                Obj.direction = Direction.Down;
                Obj.compCollision.blocking = false;
                Obj.compSprite.zOffset = -64; //sort under everything

                Obj.group = ObjGroup.Ditch;

                if (Type == ObjType.Wor_Ditch_META)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Empty_Single; }
                
                //empty ditch animFrames
                else if (Type == ObjType.Wor_Ditch_Empty_Single)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Empty_Single; }
                else if (Type == ObjType.Wor_Ditch_Empty_4UP)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Empty_4UP; }
                else if (Type == ObjType.Wor_Ditch_Empty_Vertical)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Empty_Vertical; }
                else if (Type == ObjType.Wor_Ditch_Empty_Horizontal)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Empty_Horizontal; }

                else if (Type == ObjType.Wor_Ditch_Empty_Corner_North)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Empty_Corner_North; }
                else if (Type == ObjType.Wor_Ditch_Empty_Corner_South)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Empty_Corner_South; }
                else if (Type == ObjType.Wor_Ditch_Empty_3UP_North)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Empty_3UP_North; }
                else if (Type == ObjType.Wor_Ditch_Empty_3UP_South)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Empty_3UP_South; }

                else if (Type == ObjType.Wor_Ditch_Empty_3UP_Horizontal)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Empty_3UP_Horizontal; }
                else if (Type == ObjType.Wor_Ditch_Empty_Endcap_South)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Empty_Endcap_South; }
                else if (Type == ObjType.Wor_Ditch_Empty_Endcap_Horizontal)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Empty_Endcap_Horizontal; }
                else if (Type == ObjType.Wor_Ditch_Empty_Endcap_North)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Empty_Endcap_North; }
            }

            #endregion


            #region Building WALLS & DOORS

            else if (Type == ObjType.Wor_Build_Wall_FrontA 
                || Type == ObjType.Wor_Build_Wall_FrontB
                || Type == ObjType.Wor_Build_Wall_Back)
            {
                Obj.canBeSaved = true;
                Obj.sfx.hit = Assets.sfxTapMetallic;
                Obj.compSprite.zOffset = 0; //sort over sidewalls
                //note - these hitboxes are custom for a reason
                Obj.compCollision.rec.Width = 16; Obj.compCollision.offsetX = -8;
                Obj.compCollision.rec.Height = 4; Obj.compCollision.offsetY = +4;
                //set anim frame based on type
                if (Type == ObjType.Wor_Build_Wall_FrontA)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Build_Wall_FrontA; }
                else if (Type == ObjType.Wor_Build_Wall_FrontB)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Build_Wall_FrontB; }
                else { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Build_Wall_Back; }
            }
            else if(Type == ObjType.Wor_Build_Wall_Side_Left)
            {
                Obj.canBeSaved = true;
                Obj.sfx.hit = Assets.sfxTapMetallic;
                Obj.compSprite.zOffset = 1;
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Build_Wall_Side_Left;
                //note - these hitboxes are custom for a reason
                Obj.compCollision.rec.Width = 3; Obj.compCollision.offsetX = -8;
                Obj.compCollision.rec.Height = 16; Obj.compCollision.offsetY = -8;
            }
            else if(Type == ObjType.Wor_Build_Wall_Side_Right)
            {
                Obj.canBeSaved = true;
                Obj.sfx.hit = Assets.sfxTapMetallic;
                Obj.compSprite.zOffset = 1;
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Build_Wall_Side_Right;
                //note - these hitboxes are custom for a reason
                Obj.compCollision.rec.Width = 3; Obj.compCollision.offsetX = +5;
                Obj.compCollision.rec.Height = 16; Obj.compCollision.offsetY = -8;
            }
            else if (Type == ObjType.Wor_Build_Door_Shut)
            {
                Obj.canBeSaved = true;
                Obj.compSprite.zOffset = 0;
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Build_Door_Shut;
                //note - these hitboxes are custom for a reason
                Obj.compCollision.rec.Width = 16; Obj.compCollision.offsetX = -8;
                Obj.compCollision.rec.Height = 4; Obj.compCollision.offsetY = +4;
            }
            else if (Type == ObjType.Wor_Build_Door_Open)
            {
                Obj.canBeSaved = true;
                Obj.compSprite.zOffset = 0;
                Obj.compCollision.blocking = false;
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Build_Door_Open;
            }

            #endregion


            #region Roofs

            else if (Type == ObjType.Wor_Build_Roof_Bottom
                || Type == ObjType.Wor_Build_Roof_Top)
            {
                Obj.canBeSaved = true;
                Obj.compSprite.zOffset = 48; //sort over most other things
                Obj.compCollision.blocking = false;

                if (Type == ObjType.Wor_Build_Roof_Bottom)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Build_Roof_Bottom; }
                else { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Build_Roof_Top; }
            }
            else if (Type == ObjType.Wor_Build_Roof_Chimney)
            {
                Obj.canBeSaved = true;
                Obj.compSprite.zOffset = 48; //sort over most other things
                Obj.compCollision.blocking = false;
                Obj.getsAI = true; //create smoke
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Build_Roof_Chimney;
            }
            else if (Type == ObjType.Wor_Build_Roof_Collapsing)
            {
                //this obj inherits the attributes of the previous roof obj it once was
                //this means we dont modify the attributes at all

                Obj.getsAI = true; //but collapsing roofs always get AI
                Obj.interactiveFrame = 20; //base speed of collapse spread
                Obj.interactiveFrame += Functions_Random.Int(-10, 10); //vary spreadtime
                Obj.lifeCounter = 0; //reset counter
            }

            #endregion


            #region Posts

            else if (
                Type == ObjType.Wor_Post_Vertical_Left || Type == ObjType.Wor_Post_Corner_Left ||
                Type == ObjType.Wor_PostBurned_Vertical_Left || Type == ObjType.Wor_PostBurned_Corner_Left
                )
            {
                Obj.canBeSaved = true;
                Obj.sfx.hit = Assets.sfxTapMetallic;
                Obj.compSprite.zOffset = 0;
                //hitbox 
                Obj.compCollision.rec.Width = 8; Obj.compCollision.offsetX = 0;
                Obj.compCollision.rec.Height = 16; Obj.compCollision.offsetY = -8;
                //animframes
                if (Type == ObjType.Wor_Post_Vertical_Left)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Post_Vertical_Left; }
                else if (Type == ObjType.Wor_Post_Corner_Left)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Post_Corner_Left; }
                else if (Type == ObjType.Wor_PostBurned_Vertical_Left)
                {   //burned posts have different hitboxes
                    Obj.compAnim.currentAnimation = AnimationFrames.Wor_PostBurned_Corner_Left;
                    Obj.compCollision.rec.Height = 4; Obj.compCollision.offsetY = 3;
                }
                else if (Type == ObjType.Wor_PostBurned_Corner_Left)
                {
                    Obj.compAnim.currentAnimation = AnimationFrames.Wor_PostBurned_Corner_Left;
                    Obj.compCollision.rec.Height = 4; Obj.compCollision.offsetY = 3;
                }
            }

            else if (
                Type == ObjType.Wor_Post_Vertical_Right || Type == ObjType.Wor_Post_Corner_Right ||
                Type == ObjType.Wor_PostBurned_Vertical_Right || Type == ObjType.Wor_PostBurned_Corner_Right
                )
            {
                Obj.canBeSaved = true;
                Obj.sfx.hit = Assets.sfxTapMetallic;
                Obj.compSprite.zOffset = 0;
                //hitbox 
                Obj.compCollision.rec.Width = 8; Obj.compCollision.offsetX = -8;
                Obj.compCollision.rec.Height = 16; Obj.compCollision.offsetY = -8;
                //animframes
                if (Type == ObjType.Wor_Post_Vertical_Right)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Post_Vertical_Right; }
                else if(Type == ObjType.Wor_Post_Corner_Right)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Post_Corner_Right; }
                else if (Type == ObjType.Wor_PostBurned_Vertical_Right)
                {   //burned posts have different hitboxes
                    Obj.compAnim.currentAnimation = AnimationFrames.Wor_PostBurned_Vertical_Right;
                    Obj.compCollision.rec.Height = 4; Obj.compCollision.offsetY = 3;
                }
                else if (Type == ObjType.Wor_PostBurned_Corner_Right)
                {
                    Obj.compAnim.currentAnimation = AnimationFrames.Wor_PostBurned_Corner_Right;
                    Obj.compCollision.rec.Height = 4; Obj.compCollision.offsetY = 3;
                }
            }

            else if (Type == ObjType.Wor_Post_Horizontal || Type == ObjType.Wor_PostBurned_Horizontal)
            {
                Obj.canBeSaved = true;
                Obj.sfx.hit = Assets.sfxTapMetallic;
                Obj.compSprite.zOffset = 0;
                //hitbox - same for burned and non burned
                Obj.compCollision.rec.Width = 16; Obj.compCollision.offsetX = -8;
                Obj.compCollision.rec.Height = 8; Obj.compCollision.offsetY = 0;
                //animFrame
                if (Type == ObjType.Wor_Post_Horizontal)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Post_Horizontal; }
                else { Obj.compAnim.currentAnimation = AnimationFrames.Wor_PostBurned_Horizontal; }
            }


            #endregion


            #region Signpost

            else if (Type == ObjType.Dungeon_Signpost) //not dungeon specific, lol
            {
                Obj.compCollision.offsetY = +4; Obj.compCollision.rec.Height = 4;
                Obj.compSprite.zOffset = +4;
                Obj.compAnim.currentAnimation = AnimationFrames.Signpost;
                Obj.sfx.hit = Assets.sfxTapMetallic;
                Obj.canBeSaved = true;
            }

            #endregion


            #region Dirt and Dirt Transition Tiles

            else if(
                Type == ObjType.Wor_Dirt || 
                Type == ObjType.Wor_DirtToGrass_Straight ||
                Type == ObjType.Wor_DirtToGrass_Corner_Exterior || 
                Type == ObjType.Wor_DirtToGrass_Corner_Interior
                )
            {   //sort above water, below everything else
                Obj.compSprite.zOffset = -40; 
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = false;

                //set anim frame, based on type some anim frames are random
                if(Type == ObjType.Wor_Dirt)
                {
                    if (Functions_Random.Int(0, 101) > 50)
                    { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Dirt_A; }
                    else { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Dirt_B; }
                }
                else if(Type == ObjType.Wor_DirtToGrass_Straight)
                {
                    if (Functions_Random.Int(0, 101) > 50)
                    { Obj.compAnim.currentAnimation = AnimationFrames.Wor_DirtToGrass_Straight_A; }
                    else { Obj.compAnim.currentAnimation = AnimationFrames.Wor_DirtToGrass_Straight_B; }
                }
                else if(Type == ObjType.Wor_DirtToGrass_Corner_Exterior)
                {
                    Obj.compAnim.currentAnimation = AnimationFrames.Wor_DirtToGrass_Corner_Exterior;
                }
                else if (Type == ObjType.Wor_DirtToGrass_Corner_Interior)
                {
                    Obj.compAnim.currentAnimation = AnimationFrames.Wor_DirtToGrass_Corner_Interior;
                }
            }

            #endregion


            #region Medium and Big Trees

            
            else if (Type == ObjType.Wor_Tree_Med)
            {   
                Obj.compSprite.zOffset = 8;
                Obj.compCollision.blocking = true;
                Obj.canBeSaved = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Tree_Med;
                //double size
                Obj.compSprite.drawRec.Width = 16 * 2;
                Obj.compSprite.drawRec.Height = 16 * 2;
                Obj.compCollision.rec.Width = 16 * 2 - 6;
                Obj.compCollision.rec.Height = 14;
                Obj.compCollision.offsetX = -8 + 3;
                Obj.compCollision.offsetY = +8;
            }
            else if (Type == ObjType.Wor_Tree_Med_Stump)
            {
                Obj.compSprite.zOffset = 8;
                Obj.compCollision.blocking = true;
                Obj.canBeSaved = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Tree_Med_Stump;
                //double size
                Obj.compSprite.drawRec.Width = 16 * 2;
                Obj.compSprite.drawRec.Height = 16 * 2;
                Obj.compCollision.rec.Width = 16 * 2 - 6;
                Obj.compCollision.rec.Height = 14;
                Obj.compCollision.offsetX = -8 + 3;
                Obj.compCollision.offsetY = +8;
            }
            else if (Type == ObjType.Wor_Tree_Big)
            {
                Obj.compSprite.zOffset = 16 * 2 + 4;
                Obj.compCollision.blocking = true;
                Obj.canBeSaved = true;
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Tree_Big;
                //double size
                Obj.compSprite.drawRec.Width = 16 * 4;
                Obj.compSprite.drawRec.Height = 16 * 5;
                //hitbox is custom
                Obj.compCollision.rec.Width = 16 * 4 - 8;
                Obj.compCollision.rec.Height = 16 * 2 + 10;
                Obj.compCollision.offsetX = -8 + 4;
                Obj.compCollision.offsetY = 16 * 2 - 5;
            }

            #endregion


            #region Big Shadow Cover

            else if (Type == ObjType.Wor_Shadow_Big)
            {   //big decoration
                Obj.canBeSaved = true;
                Obj.compSprite.zOffset = 16 * 10; //sort over everything ominously
                Obj.compMove.moveable = false;
                //setup hitbox
                Obj.compCollision.blocking = true; //hmm..
                Obj.compCollision.offsetX = -8; Obj.compCollision.rec.Width = 16*3;
                Obj.compCollision.offsetY = -8; Obj.compCollision.rec.Height = 16*4;
                //setup animFrame
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Shadow_Big;
                Obj.compSprite.drawRec.Width = 16 * 3; //nonstandard size
                Obj.compSprite.drawRec.Height = 16 * 4; //nonstandard size
            }


            #endregion


            #region Blocking Water Objects (water rocks)

            else if (Type == ObjType.Wor_Water_RockSm ||
                Type == ObjType.Wor_Water_RockMed)
            {   
                Obj.canBeSaved = true;
                Obj.compSprite.zOffset = -7; //sort under hero
                //setup hitbox
                Obj.compCollision.blocking = true;
                Obj.compCollision.offsetX = -5; Obj.compCollision.rec.Width = 10;
                Obj.compCollision.offsetY = -5; Obj.compCollision.rec.Height = 10;
                //setup animFrame
                if (Type == ObjType.Wor_Water_RockSm)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Water_RockSm; }
                else
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Water_RockMed; }
            }

            //underwater object
            else if(Type == ObjType.Wor_Water_RockUnderwater)
            {
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Water_RockUnderwater;
                Obj.canBeSaved = true;
                Obj.compSprite.zOffset = -7; //sort under hero
                Obj.compCollision.blocking = false; //just decoration
            }

            #endregion








            //Object Enemies


            #region Enemies

            //these enemies are roomObj enemies, simple ai, 1 hit mobs
            //standard, miniboss, and boss enemies are ACTORS and handled
            //via Ai Functions.

            else if (Type == ObjType.Wor_Enemy_Turtle
                || Type == ObjType.Wor_Enemy_Crab)
            {
                Obj.group = ObjGroup.Enemy;
                Obj.getsAI = true; //roomObj enemy
                Obj.canBeSaved = true;
                Obj.compSprite.zOffset = 0;
                Obj.compMove.moveable = true;
                //setup hitbox
                Obj.compCollision.blocking = true;
                Obj.compCollision.offsetX = -6; Obj.compCollision.rec.Width = 12;
                Obj.compCollision.offsetY = -3; Obj.compCollision.rec.Height = 8;
                //setup sfx
                Obj.sfx.hit = Assets.sfxEnemyHit;
                Obj.sfx.kill = Assets.sfxEnemyKill;
                //setup animFrame
                if (Type == ObjType.Wor_Enemy_Turtle)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Enemy_Turtle; }
                else if(Type == ObjType.Wor_Enemy_Crab)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Enemy_Crab; }
            }

            else if(Type == ObjType.Wor_Enemy_Rat)
            {
                Obj.group = ObjGroup.Enemy;
                Obj.getsAI = true; //roomObj enemy
                Obj.canBeSaved = true;
                Obj.compSprite.zOffset = 0;
                Obj.compMove.moveable = true;
                //setup hitbox
                Obj.compCollision.blocking = true;
                Obj.compCollision.offsetX = -5; Obj.compCollision.rec.Width = 10;
                Obj.compCollision.offsetY = -5; Obj.compCollision.rec.Height = 10;
                //setup sfx
                Obj.sfx.hit = Assets.sfxEnemyHit;
                Obj.sfx.kill = Assets.sfxEnemyKill;
                //setup animFrame - defaults to down
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Enemy_Rat_Down;
            }


            //unique enemies - their texture is specific
            else if (Type == ObjType.Wor_SeekerExploder)
            {
                Obj.group = ObjGroup.Enemy;
                Obj.canBeSaved = true;
                Obj.compSprite.zOffset = 0;
                Obj.compMove.moveable = true;
                //setup hitbox
                Obj.compCollision.blocking = false; //allow overlap
                Obj.compCollision.offsetX = -5; Obj.compCollision.rec.Width = 10;
                Obj.compCollision.offsetY = -5; Obj.compCollision.rec.Height = 10;
                //setup sfx
                Obj.sfx.hit = Assets.sfxEnemyHit;
                Obj.sfx.kill = Assets.sfxEnemyKill;
                //setup animFrame
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_SeekerExploder_Idle;
                Obj.getsAI = true; //seek to hero and explode
            }



            #endregion








            //Colliseum Objects

            #region Entrance Objects

            else if (Type == ObjType.Wor_Entrance_Colliseum)
            {
                Obj.compSprite.drawRec.Width = 16 * 3; //nonstandard size
                Obj.compSprite.drawRec.Height = 16 * 4; //nonstandard size
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Entrance_Coliseum_SkullIsland;
                //set collision rec to full size
                Obj.compCollision.rec.Width = 16 * 3; Obj.compCollision.offsetX = -8;
                Obj.compCollision.rec.Height = 16 * 4; Obj.compCollision.offsetY = -8;
                //sort save and block
                Obj.compSprite.zOffset = +16 * 3 - 2;
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = true;
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }

            #endregion


            #region Gates

            else if (Type == ObjType.Wor_Colliseum_Gate_Center)
            {
                Obj.compSprite.drawRec.Width = 16 * 1; //nonstandard size
                Obj.compSprite.drawRec.Height = 16 * 4; //nonstandard size
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Gate_Center;
                //non-blocking - made for easy grabbing
                Obj.compCollision.rec.Width = 16*1; Obj.compCollision.offsetX = -8;
                Obj.compCollision.rec.Height = 16*2; Obj.compCollision.offsetY = -4;
                //sort save and block
                Obj.compSprite.zOffset = +16 * 4;
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = false;
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }
            else if (Type == ObjType.Wor_Colliseum_Gate_Pillar_Left
                || Type == ObjType.Wor_Colliseum_Gate_Pillar_Right)
            {
                Obj.compSprite.drawRec.Width = 16 * 2; //nonstandard size
                Obj.compSprite.drawRec.Height = 16 * 4; //nonstandard size
                if (Type == ObjType.Wor_Colliseum_Gate_Pillar_Left)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Gate_Pillar_Left; }
                else { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Gate_Pillar_Right; }
                //rec base
                Obj.compCollision.rec.Width = 16 * 2; Obj.compCollision.offsetX = -8;
                Obj.compCollision.rec.Height = 16 * 2 - 4; Obj.compCollision.offsetY = +22;
                //sort save and block
                Obj.compSprite.zOffset = 16 + 8;
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = true;
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }

            #endregion


            #region Stairs + Handrails

            else if (
                Type == ObjType.Wor_Colliseum_Stairs_Handrail_Top
                || Type == ObjType.Wor_Colliseum_Stairs_Handrail_Middle
                || Type == ObjType.Wor_Colliseum_Stairs_Handrail_Bottom
                )
            {
                //Obj.compCollision.offsetY = 0; Obj.compCollision.rec.Height = 8;
                Obj.compSprite.zOffset = 0;
                Obj.canBeSaved = true;

                if (Type == ObjType.Wor_Colliseum_Stairs_Handrail_Top)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Stairs_Handrail_Top; }
                else if (Type == ObjType.Wor_Colliseum_Stairs_Handrail_Middle)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Stairs_Handrail_Middle; }
                else if (Type == ObjType.Wor_Colliseum_Stairs_Handrail_Bottom)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Stairs_Handrail_Bottom; }

                Obj.sfx.hit = Assets.sfxTapMetallic;
                Obj.sfx.kill = null;
            }
            else if (
                Type == ObjType.Wor_Colliseum_Stairs_Left
                || Type == ObjType.Wor_Colliseum_Stairs_Middle
                || Type == ObjType.Wor_Colliseum_Stairs_Right
                )
            {
                Obj.compSprite.zOffset = -16;
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = false;

                if (Type == ObjType.Wor_Colliseum_Stairs_Left)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Stairs_Left; }
                else if (Type == ObjType.Wor_Colliseum_Stairs_Middle)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Stairs_Middle; }
                else if (Type == ObjType.Wor_Colliseum_Stairs_Right)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Stairs_Right; }

                Obj.sfx.hit = null;
                Obj.sfx.kill = null;
            }

            #endregion


            #region Pillar

            else if (
                Type == ObjType.Wor_Colliseum_Pillar_Top
                || Type == ObjType.Wor_Colliseum_Pillar_Middle
                || Type == ObjType.Wor_Colliseum_Pillar_Bottom
                )
            {
                Obj.compSprite.zOffset = 0;
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = true;

                if (Type == ObjType.Wor_Colliseum_Pillar_Top)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Pillar_Top; }
                else if (Type == ObjType.Wor_Colliseum_Pillar_Middle)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Pillar_Middle; }
                else if (Type == ObjType.Wor_Colliseum_Pillar_Bottom)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Pillar_Bottom; }

                Obj.sfx.hit = Assets.sfxTapMetallic;
                Obj.sfx.kill = null;
            }

            #endregion


            #region Bricks

            else if (
                Type == ObjType.Wor_Colliseum_Bricks_Left
                || Type == ObjType.Wor_Colliseum_Bricks_Middle1
                || Type == ObjType.Wor_Colliseum_Bricks_Middle2
                || Type == ObjType.Wor_Colliseum_Bricks_Right
                )
            {
                Obj.compSprite.drawRec.Width = 16 * 1; //nonstandard size
                Obj.compSprite.drawRec.Height = 16 * 2; //nonstandard size
                Obj.compCollision.rec.Width = 16; Obj.compCollision.offsetX = -8;
                Obj.compCollision.rec.Height = 16 * 2; Obj.compCollision.offsetY = -8;

                Obj.canBeSaved = true;

                if (Type == ObjType.Wor_Colliseum_Bricks_Left)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Bricks_Left; }
                else if (Type == ObjType.Wor_Colliseum_Bricks_Middle1)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Bricks_Middle1; }
                else if (Type == ObjType.Wor_Colliseum_Bricks_Middle2)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Bricks_Middle2; }
                else if (Type == ObjType.Wor_Colliseum_Bricks_Right)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Bricks_Right; }

                Obj.sfx.hit = Assets.sfxTapMetallic;
                Obj.sfx.kill = null;

                //we need to model these walls after the top, mid, bottom approach
                //developed for mountain

                //Obj.group = ObjGroup.Wall_Climbable;
                Obj.compCollision.blocking = true; //false for climbing
                Obj.compSprite.zOffset = -18; //sorts under footholds
            }

            #endregion


            #region Floors

            //Wor_Colliseum_Outdoors_Floor

            else if (
                Type == ObjType.Wor_Colliseum_Outdoors_Floor
                //|| Type == ObjType.Wor_Colliseum_Bricks_Middle1
                )
            {
                Obj.compSprite.zOffset = -64; //floortile
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = false;

                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Outdoors_Floor;
                Obj.sfx.hit = null;
                Obj.sfx.kill = null;
            }

            #endregion


            #region Spectators


            else if (Type == ObjType.Wor_Colliseum_Spectator)
            {
                Obj.compSprite.drawRec.Width = 16 * 4; //nonstandard size
                Obj.compSprite.drawRec.Height = 16 * 1; 
                Obj.compCollision.rec.Width = 16 * 4; Obj.compCollision.offsetX = - 8;
                Obj.compCollision.rec.Height = 16 * 1; Obj.compCollision.offsetY = -8;
                
                Obj.compSprite.zOffset = 64; //sort over others like a roof
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = false;
                Obj.sfx.hit = null;
                Obj.sfx.kill = null;
                Obj.getsAI = true;

                //animation speed should be varied around 10, biased slow
                Obj.compAnim.speed = (byte)(10 + Functions_Random.Int(-3, 5));

                //randomly flip sprite horizontally
                if (Functions_Random.Int(0, 100) < 50)
                { Obj.compSprite.flipHorizontally = true; }
                else { Obj.compSprite.flipHorizontally = false; }

                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Spectator;
            }


            #endregion









            //Forest Objects

            #region Entrance Objs

            else if (Type == ObjType.Wor_Entrance_ForestDungeon)
            {
                Obj.compSprite.drawRec.Width = 16 * 3; //nonstandard size
                Obj.compSprite.drawRec.Height = 16 * 4; //nonstandard size
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Entrance_ForestDungeon;
                //set collision rec to full size
                Obj.compCollision.rec.Width = 16 * 3; Obj.compCollision.offsetX = -8;
                Obj.compCollision.rec.Height = 16 * 4; Obj.compCollision.offsetY = -8;
                //sort save and block
                Obj.compSprite.zOffset = +16 * 3 - 2;
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = true;
                Obj.sfx.hit = null;
            }


            else if (Type == ObjType.Wor_SkullToothInWater_Left ||
                Type == ObjType.Wor_SkullToothInWater_Right)
            {
                Obj.compSprite.drawRec.Width = 16 * 3; //nonstandard size
                Obj.compSprite.drawRec.Height = 16 * 4; //nonstandard size
                //sort save and block
                Obj.compSprite.zOffset = +16 * 3 - 2; //based on hero sorting
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = true;
                Obj.sfx.hit = Assets.sfxTapMetallic;
                Obj.compCollision.rec.Height = 16 * 4 - 4; Obj.compCollision.offsetY = -8;
                Obj.compCollision.rec.Width = 16 * 3 - 4;

                if (Type == ObjType.Wor_SkullToothInWater_Left)
                {   //left
                    Obj.compAnim.currentAnimation = AnimationFrames.Wor_SkullToothInWater_Left;
                    Obj.compCollision.offsetX = -4;
                }
                else
                {   //right
                    Obj.compAnim.currentAnimation = AnimationFrames.Wor_SkullToothInWater_Right;
                    Obj.compCollision.offsetX = -8;
                }
            }
            else if (Type == ObjType.Wor_SkullToothInWater_EndCap_Left ||
                Type == ObjType.Wor_SkullToothInWater_EndCap_Right)
            {
                Obj.compSprite.drawRec.Width = 16 * 3; //nonstandard size
                Obj.compSprite.drawRec.Height = 16 * 4; //nonstandard size
                //sort save and block
                Obj.compSprite.zOffset = +16 * 20; //top most
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = true;
                Obj.compCollision.rec.Height = 16 * 4; Obj.compCollision.offsetY = -8;
                Obj.compCollision.rec.Width = 16 * 3; Obj.compCollision.offsetX = -8;
                Obj.compCollision.rec.Width = 16 * 3 - 4;

                if (Type == ObjType.Wor_SkullToothInWater_EndCap_Left)
                {
                    Obj.compAnim.currentAnimation = AnimationFrames.Wor_SkullToothInWater_EndCap_Left;
                    Obj.compCollision.offsetX = -4;
                }
                else
                {
                    Obj.compAnim.currentAnimation = AnimationFrames.Wor_SkullToothInWater_EndCap_Right;
                    Obj.compCollision.offsetX = -8;
                }
            }




            else if (Type == ObjType.Wor_SkullToothInWater_Arch_Left ||
                Type == ObjType.Wor_SkullToothInWater_Arch_Right)
            {
                Obj.compSprite.drawRec.Width = 16 * 4; //nonstandard size
                Obj.compSprite.drawRec.Height = 16 * 1; //nonstandard size
                //sort save and block
                Obj.compSprite.zOffset = +16 * 20; //top most
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = false; //allows hidden caves
                Obj.compCollision.rec.Height = 16 * 1; Obj.compCollision.offsetY = -8;
                Obj.compCollision.rec.Width = 16 * 4; Obj.compCollision.offsetX = -8;

                if (Type == ObjType.Wor_SkullToothInWater_Arch_Left)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_SkullToothInWater_Arch_Left; }
                else
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_SkullToothInWater_Arch_Right; }
            }

            else if (Type == ObjType.Wor_SkullToothInWater_Center)
            {
                Obj.compSprite.drawRec.Width = 16 * 4; //nonstandard size
                Obj.compSprite.drawRec.Height = 16 * 3; //nonstandard size
                //sort save and block
                Obj.compSprite.zOffset = +16 * 9; //under arch extensions
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = false; //allows hidden caves
                Obj.compCollision.rec.Height = 16 * 3; Obj.compCollision.offsetY = -8;
                Obj.compCollision.rec.Width = 16 * 4; Obj.compCollision.offsetX = -8;
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_SkullToothInWater_Center;
            }

            
            else if (Type == ObjType.Wor_SkullToothInWater_Arch_Extension)
            {   //sort save and block
                Obj.compSprite.zOffset = +16 * 10; //under most other teeth objs
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = false; //allows hidden caves
                Obj.compCollision.rec.Height = 16 * 1; Obj.compCollision.offsetY = -8;
                Obj.compCollision.rec.Width = 16 * 1; Obj.compCollision.offsetX = -8;
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_SkullToothInWater_Arch_Extension;
            }




            #endregion







            //Mountain Objects

            #region Entrance Objs

            else if (Type == ObjType.Wor_Entrance_MountainDungeon)
            {
                Obj.compSprite.drawRec.Width = 16 * 2; //nonstandard size
                Obj.compSprite.drawRec.Height = 16 * 4; //nonstandard size
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Entrance_MountainDungeon;

                //set collision rec near bottom of entrance
                Obj.compCollision.rec.Width = 16 * 2; Obj.compCollision.offsetX = -8;
                Obj.compCollision.rec.Height = 16 * 2; Obj.compCollision.offsetY = +8 + 16;
                //sort save and block
                Obj.compSprite.zOffset = +16 * 3 - 2;
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = true;
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }

            #endregion


            #region Walls

            else if (Type == ObjType.Wor_MountainWall_Top)
            {   //nonstandard size
                Obj.compSprite.drawRec.Width = 16 * 4; Obj.compSprite.drawRec.Height = 16 * 1;
                Obj.compCollision.rec.Width = 16 * 4; Obj.compCollision.offsetX = -8;
                Obj.compCollision.rec.Height = 16 * 1; Obj.compCollision.offsetY = -8;
                
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_MountainWall_Top;
                Obj.compSprite.zOffset = -18; //sorts under footholds
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = false;
                //Obj.group = ObjGroup.Wall_Climbable; //do not do this!
            }
            else if (Type == ObjType.Wor_MountainWall_Mid)
            {   //nonstandard size
                Obj.compSprite.drawRec.Width = 16 * 4; Obj.compSprite.drawRec.Height = 16 * 2;
                Obj.compCollision.rec.Width = 16 * 4; Obj.compCollision.offsetX = -8;
                Obj.compCollision.rec.Height = 16 * 2; Obj.compCollision.offsetY = -8;
                
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_MountainWall_Mid;
                Obj.compSprite.zOffset = -18; //sorts under footholds
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = false;
                Obj.group = ObjGroup.Wall_Climbable;
            }
            else if (Type == ObjType.Wor_MountainWall_Bottom)
            {   //nonstandard size
                Obj.compSprite.drawRec.Width = 16 * 4; Obj.compSprite.drawRec.Height = 16 * 1;
                Obj.compCollision.rec.Width = 16 * 4; Obj.compCollision.offsetX = -8;
                Obj.compCollision.rec.Height = 16 * 1; Obj.compCollision.offsetY = -8;
                
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_MountainWall_Bottom;
                Obj.compSprite.zOffset = -18; //sorts under footholds
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = false;
                Obj.group = ObjGroup.Wall_Climbable;
            }

            #endregion


            #region Footholds

            else if (Type == ObjType.Wor_MountainWall_Foothold
                || Type == ObjType.Wor_MountainWall_Ladder
                || Type == ObjType.Wor_MountainWall_Ladder_Trap)
            {   
                Obj.compSprite.drawRec.Width = 16 * 1; Obj.compSprite.drawRec.Height = 16 * 1;
                Obj.compCollision.rec.Width = 20; Obj.compCollision.offsetX = -10;
                Obj.compCollision.rec.Height = 20; Obj.compCollision.offsetY = -10;

                if (Type == ObjType.Wor_MountainWall_Foothold)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_MountainWall_Foothold; }
                else if (Type == ObjType.Wor_MountainWall_Ladder)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_MountainWall_Ladder; }
                else { Obj.compAnim.currentAnimation = AnimationFrames.Wor_MountainWall_Ladder_Trap; }
                
                Obj.compSprite.zOffset = -16; //sorts under hero
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = false;
                Obj.group = ObjGroup.Wall_Climbable;

                //randomly flip sprite horizontally
                if (Functions_Random.Int(0, 100) < 50)
                { Obj.compSprite.flipHorizontally = true; }
                else { Obj.compSprite.flipHorizontally = false; }
            }

            #endregion


            #region Alcoves

            else if (Type == ObjType.Wor_MountainWall_Alcove_Left
                || Type == ObjType.Wor_MountainWall_Alcove_Right)
            {   //nonstandard size
                Obj.compSprite.drawRec.Width = 16*2; Obj.compSprite.drawRec.Height = 16 * 2;
                Obj.compCollision.rec.Width = 12; Obj.compCollision.rec.Height = 16 * 2;

                if (Type == ObjType.Wor_MountainWall_Alcove_Left)
                {   //set collision rec left
                    Obj.compAnim.currentAnimation = AnimationFrames.Wor_MountainWall_Alcove_Left;
                    Obj.compCollision.offsetX = -8; Obj.compCollision.offsetY = -8;
                }
                else
                {   //set collision rec right
                    Obj.compAnim.currentAnimation = AnimationFrames.Wor_MountainWall_Alcove_Right;
                    Obj.compCollision.offsetX = +12; Obj.compCollision.offsetY = -8;
                }
                
                Obj.compSprite.zOffset = -18; //sorts under footholds
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = true;
                Obj.group = ObjGroup.Object;
            }

            #endregion


            #region Caves

            else if (Type == ObjType.Wor_MountainWall_Cave_Bare
                || Type == ObjType.Wor_MountainWall_Cave_Covered)
            {   //nonstandard size
                Obj.compSprite.drawRec.Width = 16 * 2; Obj.compSprite.drawRec.Height = 16 * 2;
                Obj.compCollision.rec.Width = 16 * 2; Obj.compCollision.rec.Height = 16 * 2;
                Obj.compCollision.offsetX = -8; Obj.compCollision.offsetY = -8;

                if (Type == ObjType.Wor_MountainWall_Cave_Bare)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_MountainWall_Cave_Bare; }
                else
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_MountainWall_Cave_Covered; }

                Obj.compSprite.zOffset = 0; //sorts over footholds
                Obj.canBeSaved = true;
                Obj.group = ObjGroup.Object;
                Obj.compMove.moveable = false;  //nah
                Obj.compCollision.blocking = true;
            }

            #endregion






            //Swamp Objects

            #region Entrance Objs

            else if (Type == ObjType.Wor_Entrance_SwampDungeon)
            {
                Obj.compSprite.drawRec.Width = 16 * 3; //nonstandard size
                Obj.compSprite.drawRec.Height = 16 * 4; //nonstandard size
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Entrance_SwampDungeon;
                //set collision rec near bottom of entrance
                Obj.compCollision.offsetX = -8; Obj.compCollision.rec.Width = 16 * 3;
                //let link overlap the shadow bottom a little bit..
                Obj.compCollision.offsetY = -8; Obj.compCollision.rec.Height = 16 * 4 - 4; //..by -4
                //sort save and block
                Obj.compSprite.zOffset = +16 * 3 - 2;
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = true;
                Obj.sfx.hit = null; //in water
            }

            #endregion


            #region LillyPads, Plants, Vines

            else if (Type == ObjType.Wor_Swamp_LillyPad)
            {   //nonstandard size
                Obj.compSprite.drawRec.Width = 16 * 2; Obj.compSprite.drawRec.Height = 16 * 2;
                Obj.compCollision.rec.Width = 16 * 2; Obj.compCollision.offsetX = -8;
                Obj.compCollision.rec.Height = 16 * 2; Obj.compCollision.offsetY = -8;
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Swamp_LillyPad;
                Obj.compSprite.zOffset = -39; //sorts just above water (-40)
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = false;
            }
            else if (Type == ObjType.Wor_Swamp_BigPlant)
            {   //nonstandard size
                Obj.compSprite.drawRec.Width = 16 * 2; Obj.compSprite.drawRec.Height = 16 * 2;
                Obj.compCollision.rec.Width = 16 * 2; Obj.compCollision.offsetX = -8;
                Obj.compCollision.rec.Height = 16 * 2; Obj.compCollision.offsetY = -8;
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Swamp_BigPlant;
                Obj.compSprite.zOffset = 6; //has height
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = true;
            }

            else if (Type == ObjType.Wor_Swamp_Vine)
            {   //nonstandard size
                Obj.compSprite.drawRec.Width = 16 * 2; Obj.compSprite.drawRec.Height = 16 * 1;
                Obj.compCollision.rec.Width = 16 * 2; Obj.compCollision.offsetX = -8;
                Obj.compCollision.rec.Height = 12; Obj.compCollision.offsetY = -6;
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Swamp_Vine;
                Obj.compSprite.zOffset = 0;
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = false;
            }

            else if (Type == ObjType.Wor_Swamp_Bulb)
            {
                Obj.compCollision.rec.Width = 6; Obj.compCollision.offsetX = -3;
                Obj.compCollision.rec.Height = 4; Obj.compCollision.offsetY = 2;
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Swamp_Bulb;
                Obj.compSprite.zOffset = 1;
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = true;
            }
            else if (Type == ObjType.Wor_Swamp_SmPlant)
            {   
                Obj.compCollision.rec.Width = 10; Obj.compCollision.offsetX = -5;
                Obj.compCollision.rec.Height = 4; Obj.compCollision.offsetY = 2;
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Swamp_SmPlant;
                Obj.compSprite.zOffset = 1;
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = true;
            }

            #endregion





            //Boat Objects

            #region Front

            else if (Type == ObjType.Wor_Boat_Front)
            {   //nonstandard size
                Obj.compSprite.drawRec.Width = 16 * 2; Obj.compSprite.drawRec.Height = 16 * 3;
                Obj.compCollision.rec.Width = 16 * 2; Obj.compCollision.offsetX = -8;
                Obj.compCollision.rec.Height = 4; Obj.compCollision.offsetY = 32-8;
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Front;

                Obj.compSprite.zOffset = 20; //has height
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = true;
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }
            else if (Type == ObjType.Wor_Boat_Front_Left
                || Type == ObjType.Wor_Boat_Front_Right)
            {   //nonstandard size
                Obj.compSprite.drawRec.Width = 16 * 2; Obj.compSprite.drawRec.Height = 16 * 3;
                Obj.compCollision.rec.Width = 16 * 2; Obj.compCollision.offsetX = -8;
                Obj.compCollision.rec.Height = 16; Obj.compCollision.offsetY = -8+32;

                if (Type == ObjType.Wor_Boat_Front_Right)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Front_Right; }
                else { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Front_Left; }

                Obj.compSprite.zOffset = 20; //has height
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = true;
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }

            else if (Type == ObjType.Wor_Boat_Front_ConnectorLeft
                || Type == ObjType.Wor_Boat_Front_ConnectorRight)
            {   //nonstandard size
                Obj.compSprite.drawRec.Width = 16 * 2; Obj.compSprite.drawRec.Height = 16 * 1;
                Obj.compCollision.rec.Width = 8; 
                Obj.compCollision.rec.Height = 16; Obj.compCollision.offsetY = -8;

                if (Type == ObjType.Wor_Boat_Front_ConnectorRight)
                {
                    Obj.compCollision.offsetX = +16;
                    Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Front_ConnectorRight;
                }
                else
                {
                    Obj.compCollision.offsetX = -8;
                    Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Front_ConnectorLeft;
                }

                Obj.compSprite.zOffset = -32; //sort above water
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = true;
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }

            #endregion


            #region Bannisters + Stairs

            else if (Type == ObjType.Wor_Boat_Bannister_Left
                || Type == ObjType.Wor_Boat_Bannister_Right)
            {   //nonstandard size
                Obj.compSprite.drawRec.Width = 16 * 2; Obj.compSprite.drawRec.Height = 16 * 1;
                Obj.compCollision.rec.Width = 8;
                Obj.compCollision.rec.Height = 16; Obj.compCollision.offsetY = -8;

                if (Type == ObjType.Wor_Boat_Bannister_Right)
                {
                    Obj.compCollision.offsetX = +16;
                    Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Bannister_Right;
                }
                else
                {
                    Obj.compCollision.offsetX = -8;
                    Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Bannister_Left;
                }
                Obj.compSprite.zOffset = -32; //sort above water
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = true;
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }

            else if (Type == ObjType.Wor_Boat_Stairs_Top_Left
                || Type == ObjType.Wor_Boat_Stairs_Top_Right)
            {   //nonstandard size
                Obj.compSprite.drawRec.Width = 16 * 2; Obj.compSprite.drawRec.Height = 16 * 2;
                Obj.compCollision.rec.Width = 8;
                Obj.compCollision.rec.Height = 32; Obj.compCollision.offsetY = -8;

                if (Type == ObjType.Wor_Boat_Stairs_Top_Right)
                {
                    Obj.compCollision.offsetX = +16;
                    Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Stairs_Top_Right;
                }
                else
                {
                    Obj.compCollision.offsetX = -8;
                    Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Stairs_Top_Left;
                }
                Obj.compSprite.zOffset = -32; //sort above water
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = true;
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }

            else if (Type == ObjType.Wor_Boat_Stairs_Left
                || Type == ObjType.Wor_Boat_Stairs_Right)
            {   //nonstandard size
                Obj.compSprite.drawRec.Width = 16 * 2; Obj.compSprite.drawRec.Height = 16 * 1;
                Obj.compCollision.rec.Width = 16 * 2; Obj.compCollision.offsetX = -8;
                Obj.compCollision.rec.Height = 16; Obj.compCollision.offsetY = -8;

                if (Type == ObjType.Wor_Boat_Stairs_Right)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Stairs_Right; }
                else
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Stairs_Left; }

                Obj.compSprite.zOffset = -32; //sort above water
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = false;
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }

            else if (Type == ObjType.Wor_Boat_Stairs_Bottom_Left
                || Type == ObjType.Wor_Boat_Stairs_Bottom_Right)
            {   //nonstandard size
                Obj.compSprite.drawRec.Width = 16 * 2; Obj.compSprite.drawRec.Height = 16 * 1;
                Obj.compCollision.rec.Width = 8;
                Obj.compCollision.rec.Height = 16; Obj.compCollision.offsetY = -8;

                if (Type == ObjType.Wor_Boat_Stairs_Bottom_Right)
                {
                    Obj.compCollision.offsetX = +16;
                    Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Stairs_Bottom_Right;
                }
                else
                {
                    Obj.compCollision.offsetX = -8;
                    Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Stairs_Bottom_Left;
                }
                Obj.compSprite.zOffset = -32; //sort above water
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = true;
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }

            #endregion


            #region Back

            else if (Type == ObjType.Wor_Boat_Back_Left
                || Type == ObjType.Wor_Boat_Back_Right)
            {   //nonstandard size
                Obj.compSprite.drawRec.Width = 16 * 1; Obj.compSprite.drawRec.Height = 16 * 4;
                Obj.compCollision.rec.Width = 16; Obj.compCollision.offsetX = -8;
                Obj.compCollision.rec.Height = 16 * 3; Obj.compCollision.offsetY = -8;

                if (Type == ObjType.Wor_Boat_Back_Right)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Back_Right; }
                else
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Back_Left; }

                Obj.compSprite.zOffset = -32; //sort above water
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = true;
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }

            else if (Type == ObjType.Wor_Boat_Back_Left_Connector
                || Type == ObjType.Wor_Boat_Back_Right_Connector)
            {   //nonstandard size
                Obj.compSprite.drawRec.Width = 16 * 1; Obj.compSprite.drawRec.Height = 16 * 4;
                Obj.compCollision.rec.Width = 16; Obj.compCollision.offsetX = -8;
                Obj.compCollision.rec.Height = 16 * 3 + 8; Obj.compCollision.offsetY = -8;

                if (Type == ObjType.Wor_Boat_Back_Right_Connector)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Back_Right_Connector; }
                else
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Back_Left_Connector; }

                Obj.compSprite.zOffset = -32; //sort above water
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = true;
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }

            else if (Type == ObjType.Wor_Boat_Back_Center)
            {   //nonstandard size
                Obj.compSprite.drawRec.Width = 16 * 2; Obj.compSprite.drawRec.Height = 16 * 4;
                Obj.compCollision.rec.Width = 16 * 2; Obj.compCollision.offsetX = -8;
                Obj.compCollision.rec.Height = 16 * 2 - 4; Obj.compCollision.offsetY = 16 + 2 + 2;

                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Back_Center;

                Obj.compSprite.zOffset = -24; //sort above water
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = true;
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }

            #endregion


            #region Other Boat Objects

            else if (Type == ObjType.Wor_Boat_Floor)
            {   
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Floor;
                Obj.compSprite.zOffset = -40; //sort above water
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = false;
            }

            else if (Type == ObjType.Wor_Boat_Barrel)
            {
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Barrel;
                Obj.compCollision.rec.Width = 12; Obj.compCollision.offsetX = -6;
                Obj.compCollision.rec.Height = 10; Obj.compCollision.offsetY = -5+3;

                Obj.compSprite.zOffset = 0; //sort above water
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = true;
                Obj.sfx.hit = Assets.sfxEnemyHit;
                Obj.sfx.kill = Assets.sfxShatter;
            }

            else if (Type == ObjType.Wor_Boat_Engine)
            {   //nonstandard size
                Obj.compSprite.drawRec.Width = 16 * 6; Obj.compSprite.drawRec.Height = 16 * 5;
                Obj.compCollision.rec.Width = 16 * 3; Obj.compCollision.offsetX = 16;
                Obj.compCollision.rec.Height = 16 * 4 + 8; Obj.compCollision.offsetY = 0;

                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Engine;
                Obj.compSprite.zOffset = 43; //has height
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = true;
                Obj.sfx.hit = Assets.sfxTapMetallic;
            }

            else if (Type == ObjType.Wor_Boat_Stairs_Cover)
            {
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Stairs_Cover;
                Obj.compCollision.rec.Width = 16; Obj.compCollision.offsetX = -8;
                Obj.compCollision.rec.Height = 16; Obj.compCollision.offsetY = -8;

                Obj.compSprite.zOffset = -24; //sort below hero
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = false;
            }

            #endregion


            #region Boat Captain Brandy

            else if (Type == ObjType.Wor_Boat_Captain_Brandy)
            {
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Captain_Brandy;
                Obj.compCollision.rec.Width = 16; Obj.compCollision.offsetX = -8;
                Obj.compCollision.rec.Height = 16; Obj.compCollision.offsetY = -8;

                Obj.compSprite.zOffset = 0; //sorts normally
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = true;

                Obj.group = ObjGroup.NPC;
                Obj.getsAI = true;
                Obj.interactiveFrame = 60;
            }

            #endregion


            #region Boat Bridge, Pier

            else if (Type == ObjType.Wor_Boat_Bridge_Top)
            {   //nonstandard size
                Obj.compSprite.drawRec.Width = 16 * 2; Obj.compSprite.drawRec.Height = 16 * 1;
                Obj.compCollision.rec.Width = 16 * 2; Obj.compCollision.offsetX = -8;
                Obj.compCollision.rec.Height = 2; Obj.compCollision.offsetY = -8-2;
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Bridge_Top;
                Obj.compSprite.zOffset = -16;
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = true;
            }
            else if (Type == ObjType.Wor_Boat_Bridge_Bottom)
            {   //nonstandard size
                Obj.compSprite.drawRec.Width = 16 * 2; Obj.compSprite.drawRec.Height = 16 * 1;
                Obj.compCollision.rec.Width = 16 * 2; Obj.compCollision.offsetX = -8;
                Obj.compCollision.rec.Height = 14; Obj.compCollision.offsetY = -8;
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Bridge_Bottom;
                Obj.compSprite.zOffset = -16;
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = true;
            }
            else if (
                Type == ObjType.Wor_Boat_Pier_TopLeft
                || Type == ObjType.Wor_Boat_Pier_TopMiddle
                || Type == ObjType.Wor_Boat_Pier_TopRight

                || Type == ObjType.Wor_Boat_Pier_Left
                || Type == ObjType.Wor_Boat_Pier_Middle
                || Type == ObjType.Wor_Boat_Pier_Right

                || Type == ObjType.Wor_Boat_Pier_BottomMiddle
                || Type == ObjType.Wor_Boat_Pier_BottomLeft
                || Type == ObjType.Wor_Boat_Pier_BottomRight
                )
            {
                Obj.compCollision.rec.Width = 16; Obj.compCollision.offsetX = -8;
                Obj.compCollision.rec.Height = 16; Obj.compCollision.offsetY = -8;
                Obj.compSprite.zOffset = -15;
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = false;

                if (Type == ObjType.Wor_Boat_Pier_TopLeft)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Pier_TopLeft; }
                else if (Type == ObjType.Wor_Boat_Pier_TopMiddle)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Pier_TopMiddle; }
                else if (Type == ObjType.Wor_Boat_Pier_TopRight)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Pier_TopRight; }

                else if (Type == ObjType.Wor_Boat_Pier_Left)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Pier_Left; }
                else if (Type == ObjType.Wor_Boat_Pier_Middle)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Pier_Middle; }
                else if (Type == ObjType.Wor_Boat_Pier_Right)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Pier_Right; }

                else if (Type == ObjType.Wor_Boat_Pier_BottomMiddle)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Pier_BottomMiddle; }
                else if (Type == ObjType.Wor_Boat_Pier_BottomLeft)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Pier_BottomLeft; }
                else if (Type == ObjType.Wor_Boat_Pier_BottomRight)
                { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Pier_BottomRight; }
            }

            #endregion


            #region CoastLine

            //coastline sprites only exist on boat sheet for now
            else if (Type == ObjType.Wor_Boat_Coastline)
            {   //nonstandard size
                Obj.compSprite.drawRec.Width = 16 * 1; Obj.compSprite.drawRec.Height = 16 * 2;
                Obj.compCollision.rec.Width = 16 * 1; Obj.compCollision.offsetX = -8;
                Obj.compCollision.rec.Height = 16 * 2; Obj.compCollision.offsetY = -8;
                Obj.compAnim.currentAnimation = AnimationFrames.Wor_Coastline_Long;
                Obj.canBeSaved = true;
                Obj.compCollision.blocking = false;
                Obj.compSprite.zOffset = -128; //same as water tiles
            }


            #endregion





            #region Very Special Objects

            else if (Type == ObjType.ExplodingObject)
            {
                //a hit barrel ends up here
                //a hit seekerExploder ends up here
                //this object inherits from many diff objs
                //but simply waits a few frames, then explodes
                //(after being pushed a little)

                //prep previous obj for explosion
                Obj.getsAI = true;
                Obj.lifetime = 30; //in frames
                Obj.lifeCounter = 0; //reset
            }

            #endregion









            //Entities


            #region Pickups

            else if (
                Type == ObjType.Pickup_Rupee || Type == ObjType.Pickup_Heart ||
                Type == ObjType.Pickup_Magic || Type == ObjType.Pickup_Arrow ||
                Type == ObjType.Pickup_Bomb)
            {
                Obj.compSprite.drawRec.Width = 8; //non standard cellsize
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
                Obj.compSprite.zOffset = 0;
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 10;
                Obj.group = ObjGroup.Projectile;

                Obj.interactiveFrame = 20; //frame boomerang returns to hero
                Obj.lifetime = 255;  //must be greater than 0, but is kept at 200

                Obj.compMove.friction = 0.96f; //some air friction
                Obj.compAnim.speed = 3; //very fast, in frames
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
                Obj.compMove.friction = World.frictionIce;
                Obj.compAnim.speed = 5; //in frames
                Obj.compMove.moveable = true;
                Obj.compMove.grounded = false; //obj is airborne
                Obj.compAnim.currentAnimation = AnimationFrames.Projectile_Fireball;
                Obj.compSprite.texture = Assets.entitiesSheet;
            }

            else if (Type == ObjType.ProjectileLightningBolt)
            {
                Obj.compSprite.zOffset = 16;
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -7;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 14;
                Obj.group = ObjGroup.Projectile;
                Obj.lifetime = 25; //in frames
                Obj.compMove.friction = World.frictionIce;
                Obj.compAnim.speed = 1; //in frames
                Obj.compMove.grounded = false; //obj is airborne
                Obj.compAnim.currentAnimation = AnimationFrames.Projectile_Bolt;
                Obj.compSprite.texture = Assets.entitiesSheet;
            }

            else if (Type == ObjType.ProjectileBombos)
            {
                Obj.compSprite.zOffset = 32;
                Obj.compCollision.offsetX = -1; Obj.compCollision.offsetY = -1;
                Obj.compCollision.rec.Width = 3; Obj.compCollision.rec.Height = 3;
                Obj.group = ObjGroup.Projectile;
                Obj.lifetime = 255; //in frames
                Obj.compMove.grounded = false; //obj is airborne
                Obj.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Magic_Bombos;
                Obj.compSprite.texture = Assets.uiItemsSheet; //reuse bombos ui sprite
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


            #region Projectiles - Thrown

            else if (Type == ObjType.ProjectileBush
                || Type == ObjType.ProjectilePot
                || Type == ObjType.ProjectilePotSkull)
            {
                //bushes and pots exist on commonObjs sheet,
                //but skull pot exists on the dungeon sheet
                if (Type == ObjType.ProjectilePotSkull)
                { Obj.compSprite.texture = Assets.Dungeon_CurrentSheet; }

                //thrown projectile attributes
                Obj.group = ObjGroup.Projectile;
                Obj.compSprite.zOffset = 32;
                Obj.lifetime = 20; //in frames
                Obj.compMove.grounded = false; //obj is airborne
                Obj.compMove.friction = 0.984f; //some air friction
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


            #region Projectiles - Enemy Related

            else if (Type == ObjType.ProjectileBite)
            {
                Obj.compSprite.zOffset = 16;

                Obj.compCollision.rec.Width = 8;
                Obj.compCollision.rec.Height = 8;

                //set collision rec based on direction
                if (Obj.direction == Direction.Up || Obj.direction == Direction.Down)
                {Obj.compCollision.rec.Width = 16; }
                else //left or right
                { Obj.compCollision.rec.Height = 16; }

                Obj.group = ObjGroup.Projectile;
                Obj.lifetime = 2; //in frames
                Obj.compAnim.speed = 2; //in frames
                Obj.compAnim.loop = false;
                Obj.compMove.moveable = false;
                Obj.compMove.grounded = false; //obj is airborne
                Obj.compSprite.texture = Assets.entitiesSheet; //null / doesn't matter cause..
                Obj.compSprite.visible = false; //..this projectile isnt drawn
                Obj.compAnim.currentAnimation = AnimationFrames.Projectile_Sword; //null too
            }

            else if (Type == ObjType.ProjectileBat)
            {
                Obj.compSprite.zOffset = 16;

                Obj.compCollision.offsetX = -4; Obj.compCollision.offsetY = -4-4;
                Obj.compCollision.rec.Width = 8; Obj.compCollision.rec.Height = 8;

                Obj.group = ObjGroup.Projectile;
                Obj.lifetime = 200; //in frames
                Obj.compAnim.speed = 10; //in frames
                Obj.compMove.friction = 1.0f; //no air friction
                Obj.compMove.moveable = true;
                Obj.compMove.grounded = false; //obj is airborne
                Obj.compAnim.currentAnimation = AnimationFrames.Projectile_Bat;
                Obj.compSprite.texture = Assets.EnemySheet;
                Obj.sfx.kill = Assets.sfxRatSqueak;
                Obj.sfx.hit = null;
            }

            #endregion


            //Particles


            #region Particles - Dungeon Specific

            //these particle's sprites live on a dungeon sheet,
            //whichever dungeon sheet is the current one

            else if (Type == ObjType.Particle_PitBubble)
            {
                Obj.compSprite.drawRec.Width = 8; Obj.compSprite.drawRec.Height = 8; //nonstandard size
                Obj.compSprite.zOffset = -63; //sort over pits, under pit teeth
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 10 * 4 * 6; //speed * anim frames * loops
                Obj.compAnim.speed = 10; //in frames
                Obj.compAnim.currentAnimation = AnimationFrames.Particle_PitBubble;
                //not on the entities sheet, on dungeon sheet
                Obj.compSprite.texture = Assets.Dungeon_CurrentSheet;
            }

            #endregion


            #region Particles - Small

            else if (Type == ObjType.Particle_RisingSmoke)
            {
                Obj.compSprite.drawRec.Width = 8; Obj.compSprite.drawRec.Height = 8; //nonstandard size
                Obj.compSprite.zOffset = 64;
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
                Obj.compSprite.drawRec.Width = 8; Obj.compSprite.drawRec.Height = 8; //nonstandard size
                Obj.compSprite.zOffset = 32;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 24; //in frames
                Obj.compAnim.speed = 5; //in frames
                Obj.compAnim.loop = false;
                Obj.compAnim.currentAnimation = AnimationFrames.Particle_ImpactDust;
                Obj.compSprite.texture = Assets.entitiesSheet;
            }
            else if (Type == ObjType.Particle_Sparkle)
            {
                Obj.compSprite.drawRec.Width = 8; Obj.compSprite.drawRec.Height = 8; //nonstandard size
                Obj.compSprite.zOffset = 16;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 24; //in frames
                Obj.compAnim.speed = 6; //in frames
                Obj.compAnim.currentAnimation = AnimationFrames.Particle_Sparkle;
                Obj.compSprite.texture = Assets.entitiesSheet;
            }
            else if (Type == ObjType.Particle_Push)
            {
                Obj.compSprite.drawRec.Width = 8; Obj.compSprite.drawRec.Height = 8; //nonstandard size
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
                Obj.compSprite.drawRec.Width = 8; Obj.compSprite.drawRec.Height = 8; //nonstandard size
                Obj.compSprite.zOffset = 16;
                Obj.group = ObjGroup.Particle;
                Obj.compAnim.loop = false;
                Obj.lifetime = 15; //in frames
                Obj.compAnim.speed = 6; //in frames
                //setup animation frame properly
                if (Type == ObjType.Particle_Debris)
                { Obj.compAnim.currentAnimation = AnimationFrames.Particle_Debris; }
                else { Obj.compAnim.currentAnimation = AnimationFrames.Particle_Leaf; }
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
            else if(Type == ObjType.Particle_ExclamationBubble)
            {
                Obj.compSprite.zOffset = 1024;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 45; //in frames
                Obj.compAnim.speed = 5; //in frames
                Obj.compAnim.loop = false;
                Obj.compAnim.currentAnimation = AnimationFrames.Particle_ExclamationBubble;
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
                    Obj.compSprite.texture = Assets.Dungeon_CurrentSheet;
                }
                else if (Type == ObjType.Particle_RewardMap)
                {   //this obj is on the dungeon sheet
                    Obj.compAnim.currentAnimation = AnimationFrames.Dungeon_Map;
                    Obj.compSprite.texture = Assets.Dungeon_CurrentSheet;
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
                Obj.compSprite.drawRec.Width = 8; Obj.compSprite.drawRec.Height = 4; //nonstandard size
                Obj.group = ObjGroup.Particle;
                Obj.compAnim.speed = 10; //in frames
                Obj.compAnim.currentAnimation = AnimationFrames.Particle_Map_Flag;
                Obj.compSprite.texture = Assets.entitiesSheet;
                Obj.lifetime = 0; //lives forever
            }
            else if (Type == ObjType.Particle_Map_Wave)
            {
                Obj.compSprite.drawRec.Width = 8; Obj.compSprite.drawRec.Height = 4; //nonstandard size
                Obj.group = ObjGroup.Particle;
                Obj.compAnim.speed = 15; //in frames
                Obj.lifetime = (byte)(Obj.compAnim.speed * 4); //there are 4 frames of animation
                Obj.compAnim.loop = false;
                Obj.compAnim.currentAnimation = AnimationFrames.Particle_Map_Wave;
                Obj.compSprite.texture = Assets.entitiesSheet;
            }
            else if (Type == ObjType.Particle_Map_Campfire)
            {
                Obj.compSprite.drawRec.Width = 8;
                Obj.compSprite.drawRec.Height = 8; //nonstandard size
                Obj.group = ObjGroup.Particle;
                Obj.compAnim.speed = 6; //in frames
                Obj.compAnim.currentAnimation = AnimationFrames.Particle_Map_Campfire;
                Obj.compSprite.texture = Assets.entitiesSheet;
                Obj.lifetime = 0; //lives forever
            }

            #endregion


            //Pets


            #region Pets

            else if (Type == ObjType.Pet_Dog)
            {   //smaller than normal 16x16 objs
                Obj.compCollision.offsetX = -4; Obj.compCollision.rec.Width = 8;
                Obj.compCollision.offsetY = -4; Obj.compCollision.rec.Height = 8;
                Obj.compSprite.zOffset = -8; //this pet is lower to the ground

                Obj.lifetime = 0; //stay around forever
                Obj.compMove.moveable = true; //obj is moveable by belts
                Obj.getsAI = true; //obj gets ai too (track to hero, set anim frames)
                Obj.compCollision.blocking = false; //dont block
                Obj.compSprite.texture = Assets.petsSheet;
                Obj.canBeSaved = true;
                //set initial animation frame
                Obj.compAnim.currentAnimation = AnimationFrames.Pet_Dog_Idle;
            }
            else if(Type == ObjType.Pet_Chicken)
            {   //smaller than normal 16x16 objs
                Obj.compCollision.offsetX = -4; Obj.compCollision.rec.Width = 8;
                Obj.compCollision.offsetY = -4; Obj.compCollision.rec.Height = 8;
                Obj.compSprite.zOffset = -8; //this pet is lower to the ground

                Obj.lifetime = 0; //stay around forever
                Obj.compMove.moveable = true; //obj is moveable by belts
                Obj.getsAI = true; //obj gets ai too (track to hero, set anim frames)

                Obj.compCollision.blocking = false; //dont block
                Obj.canBeSaved = true;
                //set initial animation frame
                Obj.compAnim.currentAnimation = AnimationFrames.Pet_Chicken_Idle;
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
            else if(Obj.group == ObjGroup.Enemy)
            {   //all enemies exist on the enemy sheet
                Obj.compSprite.texture = Assets.EnemySheet;
            }

            #endregion


            SetRotation(Obj);
            Obj.compSprite.currentFrame = Obj.compAnim.currentAnimation[0]; //goto 1st anim frame
            Functions_Component.Align(Obj.compMove, Obj.compSprite, Obj.compCollision);
        }

    }
}