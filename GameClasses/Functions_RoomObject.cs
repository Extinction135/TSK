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
    public static class Functions_RoomObject
    {
        static int i;
        public static GameObject objRef;

        public static GameObject SpawnRoomObj(ObjType Type, float X, float Y, Direction Direction)
        {   //spawns RoomObject at the X, Y location, with direction
            GameObject obj = Functions_Pool.GetRoomObj();
            //set direction
            obj.direction = Direction;
            obj.compMove.direction = Direction;
            //teleport the projectile to the proper location
            Functions_Movement.Teleport(obj.compMove, X, Y);
            //set the type, rotation, cellsize, & alignment
            Functions_GameObject.SetType(obj, Type);
            return obj;
        }

        public static void ActivateLeverObjects()
        {
            Assets.Play(Assets.sfxSwitch);
            for (i = 0; i < Pool.roomObjCount; i++)
            {   //loop thru all active roomObjects
                if (Pool.roomObjPool[i].active)
                {   //sync all lever objects
                    if (Pool.roomObjPool[i].type == ObjType.LeverOff)
                    { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.LeverOn); }
                    else if (Pool.roomObjPool[i].type == ObjType.LeverOn)
                    { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.LeverOff); }
                    //find any spikeFloor objects in the room, toggle them
                    else if (Pool.roomObjPool[i].type == ObjType.SpikesFloorOn)
                    { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.SpikesFloorOff); }
                    else if (Pool.roomObjPool[i].type == ObjType.SpikesFloorOff)
                    { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.SpikesFloorOn); }
                    //locate and toggle conveyor belt objects
                    else if (Pool.roomObjPool[i].type == ObjType.ConveyorBeltOn)
                    { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.ConveyorBeltOff); }
                    else if (Pool.roomObjPool[i].type == ObjType.ConveyorBeltOff)
                    { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.ConveyorBeltOn); }
                }
            }
        }

        public static void ActivateSwitchObject(GameObject Obj)
        {   //convert switch off, play switch soundFx
            Functions_GameObject.SetType(Obj, ObjType.SwitchOff);
            //grab the player's attention
            Functions_Particle.Spawn(
                ObjType.ParticleAttention,
                Obj.compSprite.position.X,
                Obj.compSprite.position.Y);
            OpenTrapDoors(); //open all trap doors
        }

        public static void FlipSwitchBlocks(GameObject SwitchBtn)
        {
            Assets.Play(Assets.sfxSwitch);
            Functions_Particle.Spawn(
                ObjType.ParticleAttention,
                SwitchBtn.compSprite.position.X,
                SwitchBtn.compSprite.position.Y);
            for (i = 0; i < Pool.roomObjCount; i++)
            {   //loop thru all active roomObjects
                if (Pool.roomObjPool[i].active)
                {   //flip blocks up or down
                    if (Pool.roomObjPool[i].type == ObjType.SwitchBlockDown)
                    { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.SwitchBlockUp); }
                    else if (Pool.roomObjPool[i].type == ObjType.SwitchBlockUp)
                    { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.SwitchBlockDown); }
                    //display particle fx at block location
                    if (Pool.roomObjPool[i].type == ObjType.SwitchBlockDown
                        || Pool.roomObjPool[i].type == ObjType.SwitchBlockUp)
                    {
                        Functions_Particle.Spawn(
                            ObjType.ParticleAttention,
                            Pool.roomObjPool[i].compSprite.position.X,
                            Pool.roomObjPool[i].compSprite.position.Y);
                    }
                }
            }
        }

        public static Boolean CountTorches()
        {   //count to see if there are more than 3 lit torches in the current room
            int torchCount = 0;
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active)
                {   //if there is an active switch in the room
                    if (Pool.roomObjPool[i].type == ObjType.TorchLit)
                    { torchCount++; } //count all the lit torches
                }
            }
            //check for more than 3 torches
            if (torchCount > 3) { return true; } else { return false; }
        }

        public static void OpenTrapDoors()
        {
            Assets.Play(Assets.sfxSwitch);
            for (i = 0; i < Pool.roomObjCount; i++)
            {   //loop thru all active roomObjects
                if (Pool.roomObjPool[i].active)
                {   //convert trap doors to open doors
                    if (Pool.roomObjPool[i].type == ObjType.DoorTrap)
                    {   //display an attention particle where the conversion happened
                        Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.DoorOpen);
                        Functions_Particle.Spawn(
                            ObjType.ParticleAttention,
                            Pool.roomObjPool[i].compSprite.position.X,
                            Pool.roomObjPool[i].compSprite.position.Y);
                    }
                }
            }
        }

        public static void CloseDoors()
        {
            Assets.Play(Assets.sfxSwitch);
            for (i = 0; i < Pool.roomObjCount; i++)
            {   //loop thru all active roomObjects
                if (Pool.roomObjPool[i].active)
                {   //convert trap doors to open doors
                    if (Pool.roomObjPool[i].type == ObjType.DoorOpen)
                    {   //all open doors inside room become trap doors (push hero + close)
                        Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.DoorTrap);
                    }
                }
            }
        }

        public static void LightTorch(GameObject UnlitTorch)
        {   //light the unlit torch
            Functions_GameObject.SetType(UnlitTorch, ObjType.TorchLit);
            Functions_Particle.Spawn(
                ObjType.ParticleFire,
                UnlitTorch.compSprite.position.X + 0,
                UnlitTorch.compSprite.position.Y - 7);
            Assets.Play(Assets.sfxLightFire);

            //check to see if lighting this torch can solve the room's puzzle
            if (Functions_Level.currentRoom.puzzleType == PuzzleType.Torches)
            {   //if the current room's puzzle type is Torches, check to see how many have been lit
                if (CountTorches())
                {   //enough torches have been lit to unlock this room / solve puzzle
                    Assets.Play(Assets.sfxReward); //should be secret sfx!!!
                    OpenTrapDoors(); //open all the trap doors in the room
                }
            }
        }

        public static void CreateVendor(ObjType VendorType, Vector2 Position)
        {
            //place vendor
            SpawnRoomObj(VendorType, Position.X, Position.Y, Direction.Down);
            //place stone table
            SpawnRoomObj(ObjType.TableStone, Position.X + 16, Position.Y, Direction.Down);
            //spawn vendor advertisement
            objRef = SpawnRoomObj(ObjType.VendorAdvertisement,
                Position.X + 16, Position.Y - 6, Direction.Down);
            objRef.compAnim.currentAnimation = new List<Byte4>();


            #region Display the vendor's wares for sale

            if (VendorType == ObjType.VendorItems)
            {   //add all the items from the main sheet
                objRef.compAnim.currentAnimation.Add(new Byte4(5, 5, 0, 0)); //heart
                objRef.compAnim.currentAnimation.Add(new Byte4(5, 6, 0, 0)); //bomb
                objRef.compAnim.currentAnimation.Add(new Byte4(5, 7, 0, 0)); //bombs
                objRef.compAnim.currentAnimation.Add(new Byte4(5, 8, 0, 0)); //arrows
            }
            else if (VendorType == ObjType.VendorPotions)
            {   //add all the potions from the main sheet
                objRef.compAnim.currentAnimation.Add(new Byte4(6, 6, 0, 0)); //health
                objRef.compAnim.currentAnimation.Add(new Byte4(6, 7, 0, 0)); //magic
                objRef.compAnim.currentAnimation.Add(new Byte4(6, 8, 0, 0)); //combo
            }
            else if (VendorType == ObjType.VendorMagic)
            {   //add all the magic medallions from the main sheet
                objRef.compAnim.currentAnimation.Add(new Byte4(7, 5, 0, 0)); //fireball
            }
            else if (VendorType == ObjType.VendorWeapons)
            {   //add all the weapons from the main sheet
                //objRef.compAnim.currentAnimation.Add(new Byte4(8, 5, 0, 0)); //sword
                objRef.compAnim.currentAnimation.Add(new Byte4(8, 6, 0, 0)); //bow
                objRef.compAnim.currentAnimation.Add(new Byte4(8, 7, 0, 0)); //net
                //objRef.compAnim.currentAnimation.Add(new Byte4(8, 8, 0, 0)); //axe
                //objRef.compAnim.currentAnimation.Add(new Byte4(8, 9, 0, 0)); //net
            }
            else if (VendorType == ObjType.VendorArmor)
            {   //add all the armor from the main sheet
                objRef.compAnim.currentAnimation.Add(new Byte4(9, 5, 0, 0)); //shirt
                objRef.compAnim.currentAnimation.Add(new Byte4(9, 6, 0, 0)); //plate mail
                objRef.compAnim.currentAnimation.Add(new Byte4(9, 7, 0, 0)); //cape
                objRef.compAnim.currentAnimation.Add(new Byte4(9, 8, 0, 0)); //robe
            }
            else if (VendorType == ObjType.VendorEquipment)
            {   //add all the equipment from the main sheet
                objRef.compAnim.currentAnimation.Add(new Byte4(10, 5, 0, 0)); //ring
                //objRef.compAnim.currentAnimation.Add(new Byte4(10, 6, 0, 0)); //pearl
                //objRef.compAnim.currentAnimation.Add(new Byte4(10, 7, 0, 0)); //necklace
                //objRef.compAnim.currentAnimation.Add(new Byte4(10, 8, 0, 0)); //glove
                //objRef.compAnim.currentAnimation.Add(new Byte4(10, 9, 0, 0)); //pin
            }
            else if (VendorType == ObjType.VendorPets)
            {   //add all the pets from the main sheet
                objRef.compAnim.currentAnimation.Add(new Byte4(11, 5, 0, 0)); //stinky dog
                objRef.compAnim.currentAnimation.Add(new Byte4(11, 6, 0, 0)); //chicken
            }

            #endregion

        }

        public static void CollapseDungeonDoor(GameObject Door, GameObject Projectile)
        {   //blow up door, change to doorOpen
            DestroyObject(Door, false, false);
            Functions_GameObject.SetType(Door, ObjType.DoorOpen);
            //update the dungeon.doors list, change colliding door to bombed
            for (int i = 0; i < Level.doors.Count; i++)
            {   //if this explosion collides with any dungeon.door that is of type.bombable
                if (Level.doors[i].type == DoorType.Bombable)
                {   //change this door type to type.bombed
                    if (Projectile.compCollision.rec.Intersects(Level.doors[i].rec))
                    { Level.doors[i].type = DoorType.Open; }
                }
            }
        }

        public static void DestroyBarrel(GameObject Barrel)
        {   //create a projectile exploding barrel at Barrel's exact location
            Functions_Projectile.Spawn(
                ObjType.ProjectileExplodingBarrel,
                Barrel.compMove,
                Barrel.compMove.direction); //pushed based on this direction
            Functions_Pool.Release(Barrel);
        }

        public static void DestroyObject(GameObject RoomObj, Boolean releaseObj, Boolean spawnLoot)
        {   //grab players attention, spawn rock debris, play shatter sound
            Functions_Particle.Spawn(
                ObjType.ParticleAttention,
                RoomObj.compSprite.position.X,
                RoomObj.compSprite.position.Y);
            Functions_Particle.ScatterDebris(RoomObj.compSprite.position);
            Functions_Particle.ScatterDebris(RoomObj.compSprite.position);
            Functions_Particle.ScatterDebris(RoomObj.compSprite.position);
            Assets.Play(Assets.sfxShatter);
            //handle parameter values
            if (spawnLoot) { Functions_Loot.SpawnLoot(RoomObj.compSprite.position); }
            if (releaseObj) { Functions_Pool.Release(RoomObj); }
        }

        public static void HandleCommon(GameObject RoomObj, Direction HitDirection)
        {   //this handles the most common room objs
            //hitDirection is used to push some objects in the direction they were hit
            if (RoomObj.type == ObjType.Pot)
            {
                DestroyObject(RoomObj, true, true);
            }
            else if (RoomObj.type == ObjType.Barrel)
            {
                RoomObj.compMove.direction = HitDirection; //pass hitDirection
                DestroyBarrel(RoomObj);
            }
            else if(RoomObj.type == ObjType.SwitchBlockBtn)
            {
                FlipSwitchBlocks(RoomObj);
            }
        }

        public static void SlideOnIce(ComponentMovement compMove)
        {   //set the component's friction to ice (slides)
            compMove.friction = World.frictionIce;
            //clip magnitude's maximum values for ice (reduces max speed)
            if (compMove.magnitude.X > 1) { compMove.magnitude.X = 1; }
            else if (compMove.magnitude.X < -1) { compMove.magnitude.X = -1; }
            if (compMove.magnitude.Y > 1) { compMove.magnitude.Y = 1; }
            else if (compMove.magnitude.Y < -1) { compMove.magnitude.Y = -1; }
        }

        public static void DragIntoPit(GameObject Object, GameObject Pit)
        {   //obj must be grounded
            if (Object.compMove.grounded)
            {
                //check to see if obj started falling, or has been falling
                if (Object.compSprite.scale == 1.0f) //begin falling state
                {   //dont play falling sound if entity is thrown pot (falling sound was just played)
                    //if (ObjA.type != ObjType.ProjectilePot) { Assets.Play(Assets.sfxActorFall); }
                    Assets.Play(Assets.sfxActorFall); //this should be objFall sfx

                    //we should use a throw sound fx that is diff than falling sound
                }

                //scale obj down if it's colliding with a pit
                Object.compSprite.scale -= 0.03f;

                //slightly pull obj towards pit's center
                Object.compMove.magnitude = (Pit.compSprite.position - Object.compSprite.position) * 0.25f;

                //when obj drops below a threshold scale, release it
                if (Object.compSprite.scale < 0.8f)
                {
                    Functions_Pool.Release(Object);
                    PlayPitFx(Pit);
                }
            }
        }

        public static void PlayPitFx(GameObject Pit)
        {   //play splash particle effect
            Assets.Play(Assets.sfxSplash);
            Functions_Particle.Spawn(
                ObjType.ParticleSplash,
                Pit.compSprite.position.X,
                Pit.compSprite.position.Y - 4);
        }

        public static void BounceOffBumper(ComponentMovement compMove, GameObject Bumper)
        {   //bounce opposite direction
            compMove.direction = Functions_Direction.GetOppositeDirection(compMove.direction);
            //if the direction is none, then get a direction between bumper and collider
            if (compMove.direction == Direction.None)
            { compMove.direction = Functions_Direction.GetOppositeCardinal(compMove.position, Bumper.compSprite.position); }
            //push collider in direction
            Functions_Movement.Push(compMove, compMove.direction, 8.0f);
            //handle the bumper animation
            Bumper.compSprite.scale = 1.5f;
            Assets.Play(Assets.sfxBounce);
            Functions_Particle.Spawn(
                ObjType.ParticleAttention,
                Bumper.compSprite.position.X,
                Bumper.compSprite.position.Y);
        }

        public static void ConveyorBeltPush(ComponentMovement compMove, GameObject belt)
        {   //based on belt's direction, push moveComp by amount
            Functions_Movement.Push(compMove, belt.direction, 0.15f);
        }

        public static void BounceSpikeBlock(GameObject SpikeBlock)
        {   //spawn a hit particle along spikeBlock's colliding edge
            Functions_Particle.Spawn(ObjType.ParticleHitSparkle, SpikeBlock);
            Assets.Play(Assets.sfxTapMetallic); //play the 'clink' sound effect
            //flip the block's direction to the opposite direction
            SpikeBlock.compMove.direction = Functions_Direction.GetOppositeDirection(SpikeBlock.compMove.direction);
            SpikeBlock.compMove.magnitude.X = 0;
            SpikeBlock.compMove.magnitude.Y = 0;
            //push the block in it's new direction, out of this collision
            Functions_Movement.Push(SpikeBlock.compMove, SpikeBlock.compMove.direction, 2.0f);
            //force move spikeblock to it's new position, ignoring collisions
            SpikeBlock.compMove.position += SpikeBlock.compMove.magnitude;
            SpikeBlock.compMove.newPosition = SpikeBlock.compMove.position;
            Functions_Component.Align(SpikeBlock.compMove, SpikeBlock.compSprite, SpikeBlock.compCollision);
        }

        public static void UseFairy(GameObject Fairy)
        {
            Pool.hero.health = PlayerData.current.heartsTotal; //effect
            Assets.Play(Assets.sfxHeartPickup); //sfx

            if (Fairy != null) //end fairys life
            { Fairy.lifeCounter = 2; Fairy.lifetime = 1; }
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
                    Functions_GameObject.SetRotation(Pool.roomObjPool[Pool.roomObjCounter]);
                }
            }
        }


        //decorates a door on left/right or top/bottom
        static Vector2 posA = new Vector2();
        static Vector2 posB = new Vector2();
        public static void DecorateDoor(GameObject Door, ObjType Type)
        {
            if (Door.direction == Direction.Up || Door.direction == Direction.Down)
            {   //build left/right decorations if Door.direction is Up or Down
                posA.X = Door.compSprite.position.X - 16;
                posA.Y = Door.compSprite.position.Y;
                posB.X = Door.compSprite.position.X + 16;
                posB.Y = Door.compSprite.position.Y;
            }
            else
            {   //build top/bottom decorations if Door.direction is Left or Right
                posA.X = Door.compSprite.position.X;
                posA.Y = Door.compSprite.position.Y - 16;
                posB.X = Door.compSprite.position.X;
                posB.Y = Door.compSprite.position.Y + 16;
            }
            //build wall decorationA torch/pillar/decoration
            SpawnRoomObj(Type, posA.X, posA.Y, Door.direction);
            //build wall decorationB torch/pillar/decoration
            SpawnRoomObj(Type, posB.X, posB.Y, Door.direction);
        }

    }
}