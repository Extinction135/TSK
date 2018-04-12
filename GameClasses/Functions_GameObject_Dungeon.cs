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
    public static class Functions_GameObject_Dungeon
    {
        static int i;

        public static void ActivateLeverObjects()
        {
            Assets.Play(Assets.sfxSwitch);
            for (i = 0; i < Pool.roomObjCount; i++)
            {   //loop thru all active roomObjects
                if (Pool.roomObjPool[i].active)
                {   //sync all lever objects
                    if (Pool.roomObjPool[i].type == ObjType.Dungeon_LeverOff)
                    { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.Dungeon_LeverOn); }
                    else if (Pool.roomObjPool[i].type == ObjType.Dungeon_LeverOn)
                    { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.Dungeon_LeverOff); }
                    //find any spikeFloor objects in the room, toggle them
                    else if (Pool.roomObjPool[i].type == ObjType.Dungeon_SpikesFloorOn)
                    { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.Dungeon_SpikesFloorOff); }
                    else if (Pool.roomObjPool[i].type == ObjType.Dungeon_SpikesFloorOff)
                    { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.Dungeon_SpikesFloorOn); }
                    //locate and toggle conveyor belt objects
                    else if (Pool.roomObjPool[i].type == ObjType.Dungeon_ConveyorBeltOn)
                    { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.Dungeon_ConveyorBeltOff); }
                    else if (Pool.roomObjPool[i].type == ObjType.Dungeon_ConveyorBeltOff)
                    { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.Dungeon_ConveyorBeltOn); }
                }
            }
        }

        public static void ActivateSwitchObject(GameObject Switch)
        {   //convert switch off, play switch soundFx
            Functions_GameObject.SetType(Switch, ObjType.Dungeon_SwitchOff);
            //grab the player's attention
            Functions_Particle.Spawn(
                ObjType.Particle_Attention,
                Switch.compSprite.position.X,
                Switch.compSprite.position.Y);
            OpenTrapDoors(); //open all trap doors
        }
        
        public static void BounceOffBumper(ComponentMovement compMove, GameObject Bumper)
        {
            //handle the bumper animation
            Bumper.compSprite.scale = 1.5f;
            Assets.Play(Assets.sfxBounce);
            Functions_Particle.Spawn(
                ObjType.Particle_Attention,
                Bumper.compSprite.position.X,
                Bumper.compSprite.position.Y);
            //bounce opposite direction
            compMove.direction = Functions_Direction.GetOppositeDirection(compMove.direction);
            //if the direction is none, then get a direction between bumper and collider
            if (compMove.direction == Direction.None)
            {
                compMove.direction = Functions_Direction.GetOppositeCardinal(
                    compMove.position, Bumper.compSprite.position);
            }
            //push collider in direction
            Functions_Movement.Push(compMove, compMove.direction, 6.0f);
        }

        public static void BounceSpikeBlock(GameObject SpikeBlock)
        {   //spikeBlock must be moving
            if (Math.Abs(SpikeBlock.compMove.magnitude.X) > 0 || Math.Abs(SpikeBlock.compMove.magnitude.Y) > 0)
            {   //spawn a hit particle along spikeBlock's colliding edge
                Functions_Particle.Spawn(ObjType.Particle_Sparkle, SpikeBlock);
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
        }
        
        public static void CloseDoors()
        {
            Assets.Play(Assets.sfxSwitch);
            for (i = 0; i < Pool.roomObjCount; i++)
            {   //loop thru all active roomObjects
                if (Pool.roomObjPool[i].active)
                {   //convert trap doors to open doors
                    if (Pool.roomObjPool[i].type == ObjType.Dungeon_DoorOpen)
                    {   //all open doors inside room become trap doors (push hero + close)
                        Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.Dungeon_DoorTrap);
                    }
                }
            }
        }

        public static void CollapseDungeonDoor(GameObject Door, GameObject Projectile)
        {   //blow up door, change to doorOpen
            Functions_RoomObject.DestroyObject(Door, false, false);
            Functions_GameObject.SetType(Door, ObjType.Dungeon_DoorOpen);
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

        public static void ConveyorBeltPush(ComponentMovement compMove, GameObject belt)
        {   //based on belt's direction, push moveComp by amount
            Functions_Movement.Push(compMove, belt.direction, 0.15f);
        }
        
        public static Boolean CountTorches()
        {   //count to see if there are more than 3 lit torches in the current room
            int torchCount = 0;
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active)
                {   //if there is an active switch in the room
                    if (Pool.roomObjPool[i].type == ObjType.Dungeon_TorchLit)
                    { torchCount++; } //count all the lit torches
                }
            }
            //check for more than 3 torches
            if (torchCount > 3) { return true; } else { return false; }
        }

        public static void DestroyBarrel(GameObject Barrel)
        {   //create a projectile exploding barrel at Barrel's exact location
            Functions_Projectile.Spawn(
                ObjType.ProjectileExplodingBarrel,
                Barrel.compMove,
                Barrel.compMove.direction); //pushed based on this direction
            Functions_Pool.Release(Barrel);
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
        
        public static void FlipSwitchBlocks(GameObject SwitchBtn)
        {
            Assets.Play(Assets.sfxSwitch);
            Functions_Particle.Spawn(
                ObjType.Particle_Attention,
                SwitchBtn.compSprite.position.X,
                SwitchBtn.compSprite.position.Y);
            for (i = 0; i < Pool.roomObjCount; i++)
            {   //loop thru all active roomObjects
                if (Pool.roomObjPool[i].active)
                {   //flip blocks up or down
                    if (Pool.roomObjPool[i].type == ObjType.Dungeon_SwitchBlockDown)
                    { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.Dungeon_SwitchBlockUp); }
                    else if (Pool.roomObjPool[i].type == ObjType.Dungeon_SwitchBlockUp)
                    { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.Dungeon_SwitchBlockDown); }
                    //display particle fx at block location
                    if (Pool.roomObjPool[i].type == ObjType.Dungeon_SwitchBlockDown
                        || Pool.roomObjPool[i].type == ObjType.Dungeon_SwitchBlockUp)
                    {
                        Functions_Particle.Spawn(
                            ObjType.Particle_Attention,
                            Pool.roomObjPool[i].compSprite.position.X,
                            Pool.roomObjPool[i].compSprite.position.Y);
                    }
                }
            }
        }

        public static void LightTorch(GameObject UnlitTorch)
        {   //light the unlit torch
            Functions_GameObject.SetType(UnlitTorch, ObjType.Dungeon_TorchLit);
            Functions_Particle.Spawn(
                ObjType.Particle_FireGround,
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
        
        public static void OpenTrapDoors()
        {
            Assets.Play(Assets.sfxSwitch);
            for (i = 0; i < Pool.roomObjCount; i++)
            {   //loop thru all active roomObjects
                if (Pool.roomObjPool[i].active)
                {   //convert trap doors to open doors
                    if (Pool.roomObjPool[i].type == ObjType.Dungeon_DoorTrap)
                    {   //display an attention particle where the conversion happened
                        Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.Dungeon_DoorOpen);
                        Functions_Particle.Spawn(
                            ObjType.Particle_Attention,
                            Pool.roomObjPool[i].compSprite.position.X,
                            Pool.roomObjPool[i].compSprite.position.Y);
                    }
                }
            }
        }
        
        public static void PlayPitFx(GameObject Pit)
        {   //play splash particle effect
            Assets.Play(Assets.sfxSplash);
            Functions_Particle.Spawn(
                ObjType.Particle_Splash,
                Pit.compSprite.position.X,
                Pit.compSprite.position.Y - 4);
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
        
        public static void UseFairy(GameObject Fairy)
        {
            Pool.hero.health = PlayerData.current.heartsTotal; //effect
            Assets.Play(Assets.sfxHeartPickup); //sfx

            if (Fairy != null) //kill fairy
            {
                Functions_Particle.Spawn(
                    ObjType.Particle_Attention,
                    Fairy.compSprite.position.X,
                    Fairy.compSprite.position.Y);
                Functions_Pool.Release(Fairy);
            }
        }

    }
}