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
    public static class Functions_Interaction
    {

        public static ComponentCollision interactionRec = new ComponentCollision();

        public static void ClearHeroInteractionRec()
        {   //move the interaction rec offscreen
            interactionRec.rec.X = -1000;
            interactionRec.rec.Y = -1000;
        }

        public static void SetHeroInteractionRec()
        {
            //set the interaction rec to the hero's position
            interactionRec.rec.X = (int)Pool.hero.compSprite.position.X - 4;
            interactionRec.rec.Y = (int)Pool.hero.compSprite.position.Y - 4;
            //offset the rec based on the direction hero is facing
            if (Pool.hero.direction == Direction.Up)
            {
                interactionRec.rec.Width = 8; interactionRec.rec.Height = 4;
                interactionRec.rec.Y -= 1;
            }
            else if (Pool.hero.direction == Direction.Down)
            {
                interactionRec.rec.Width = 8; interactionRec.rec.Height = 4;
                interactionRec.rec.Y += 14;
            }
            else if (
                Pool.hero.direction == Direction.Left || 
                Pool.hero.direction == Direction.UpLeft || 
                Pool.hero.direction == Direction.DownLeft) 
            {
                interactionRec.rec.Width = 4; interactionRec.rec.Height = 8;
                interactionRec.rec.Y += 4; interactionRec.rec.X -= 7;
            }
            else if (
                Pool.hero.direction == Direction.Right || 
                Pool.hero.direction == Direction.UpRight || 
                Pool.hero.direction == Direction.DownRight)
            {
                interactionRec.rec.Width = 4; interactionRec.rec.Height = 8;
                interactionRec.rec.Y += 4; interactionRec.rec.X += 11;
            }
        }



        public static void InteractHero(GameObject Obj)
        {   //this is the hero's interactionRec colliding with Obj
            //we know this is hero, and hero is in ActorState.Interact
            
            #region Liftable objects

            if (Obj.group == ObjGroup.Liftable)
            {
                //pickup object over hero's head
            }

            #endregion


            #region Draggable objects

            else if (Obj.group == ObjGroup.Draggable)
            {
                //BlockDraggable
                //begin dragging object
            }

            #endregion


            #region Chests

            else if (Obj.group == ObjGroup.Chest)
            {

                #region Reward the hero with chest contents

                if (Obj.type == ObjType.ChestKey)
                {
                    Functions_Entity.SpawnEntity(ObjType.ParticleRewardKey, Pool.hero);
                    Assets.Play(Assets.sfxKeyPickup);
                    Level.bigKey = true;
                    if (Flags.ShowDialogs)
                    { ScreenManager.AddScreen(new ScreenDialog(Dialog.HeroGotKey)); }
                }
                else if (Obj.type == ObjType.ChestMap)
                {
                    Functions_Entity.SpawnEntity(ObjType.ParticleRewardMap, Pool.hero);
                    Assets.Play(Assets.sfxReward);
                    Level.map = true;
                    if (Flags.ShowDialogs)
                    { ScreenManager.AddScreen(new ScreenDialog(Dialog.HeroGotMap)); }
                }

                #endregion


                if (Obj.type != ObjType.ChestEmpty)
                {   //if the chest is not empty, play the reward animation
                    Assets.Play(Assets.sfxChestOpen);
                    Functions_GameObject.SetType(Obj, ObjType.ChestEmpty);
                    Pool.hero.state = ActorState.Reward; //set actor into reward state
                    Pool.hero.lockTotal = 40; //lock for a prolonged time
                    Functions_Entity.SpawnEntity( //show the chest was opened
                        ObjType.ParticleAttention,
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y,
                        Direction.None);
                }
            }

            #endregion


            #region Vendors

            else if (Obj.group == ObjGroup.Vendor)
            {   //some vendors do not sell items, so check vendor types
                if (Obj.type == ObjType.VendorStory) //for now this is default dialog
                {   //figure out what part of the story the hero is at, pass this dialog
                    ScreenManager.AddScreen(new ScreenDialog(Dialog.Default));
                }
                //check to make sure the obj isn't a vendor advertisement
                else if (Obj.type != ObjType.VendorAdvertisement)
                { ScreenManager.AddScreen(new ScreenVendor(Obj)); }
                //vendor ad objects are ignored
            }

            #endregion


            #region Boss Door

            else if (Obj.type == ObjType.DoorBoss)
            {   //if hero doesn't have the bigKey, throw a dialog screen telling player this
                if (Level.bigKey == false)
                {
                    if (Flags.ShowDialogs)
                    { ScreenManager.AddScreen(new ScreenDialog(Dialog.DoesNotHaveKey)); }
                }
            }

            #endregion


            #region Misc Interactive Dungeon Objects

            else if (Obj.type == ObjType.TorchUnlit)
            {   //light the unlit torch
                Functions_GameObject.SetType(Obj, ObjType.TorchLit);
                Functions_Entity.SpawnEntity(ObjType.ParticleFire,
                    Obj.compSprite.position.X + 0,
                    Obj.compSprite.position.Y - 7,
                    Direction.None);
                Assets.Play(Assets.sfxLightFire);
            }
            else if (Obj.type == ObjType.LeverOff || Obj.type == ObjType.LeverOn)
            {   //activate all lever objects (including lever), call attention to change
                Functions_Room.ActivateLeverObjects();
                Functions_Entity.SpawnEntity( 
                        ObjType.ParticleAttention,
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y,
                        Direction.None);
            }

            #endregion

        }

        public static void InteractActor(Actor Actor, GameObject Obj)
        {   //the Obj is non-blocking
            //particle Objs never interact with actors or reach this function
            //objectGroups are checked in order of most commonly interacted with

            if(Actor == Pool.hero) //certain objects only interact with hero
            {

                #region Pickups

                if (Obj.group == ObjGroup.Pickup)
                {   //only the hero can pickup hearts or rupees
                    if (Obj.type == ObjType.PickupHeart)
                    { Actor.health++; Assets.Play(Assets.sfxHeartPickup); }
                    else if (Obj.type == ObjType.PickupRupee)
                    { PlayerData.current.gold++; Assets.Play(Assets.sfxGoldPickup); }
                    else if (Obj.type == ObjType.PickupMagic)
                    { PlayerData.current.magicCurrent++; Assets.Play(Assets.sfxHeartPickup); }
                    else if (Obj.type == ObjType.PickupArrow)
                    { PlayerData.current.arrowsCurrent++; Assets.Play(Assets.sfxHeartPickup); }
                    else if (Obj.type == ObjType.PickupBomb)
                    { PlayerData.current.bombsCurrent++; Assets.Play(Assets.sfxHeartPickup); }
                    //end the items life
                    Obj.lifetime = 1; Obj.lifeCounter = 2;
                }

                #endregion


                #region Doors

                else if (Obj.group == ObjGroup.Door)
                {
                    //exit pillars & bombable doors have no interactions whatsoever
                    if (Obj.type == ObjType.ExitPillarLeft || Obj.type == ObjType.ExitPillarRight) { return; }
                    if (Obj.type == ObjType.DoorBombable) { return; }


                    #region Check Collisions with Door Types

                    if (Obj.type == ObjType.DoorBoss)
                    {   //hero must have dungeon key to open boss door
                        if (Level.bigKey)
                        {
                            Functions_GameObject.SetType(Obj, ObjType.DoorOpen);
                            Assets.Play(Assets.sfxDoorOpen);
                            Functions_Entity.SpawnEntity(
                                ObjType.ParticleAttention,
                                Obj.compSprite.position.X,
                                Obj.compSprite.position.Y,
                                Direction.None);
                        }
                    }
                    else if (Obj.type == ObjType.Exit)
                    {   //only hero can exit dungeon
                        if (Functions_Level.levelScreen.displayState == DisplayState.Opened)
                        {   //if dungeon screen is open, close it, perform interaction ONCE
                            DungeonRecord.beatDungeon = false;
                            Functions_Level.levelScreen.exitAction = ExitAction.Overworld;
                            Functions_Level.levelScreen.displayState = DisplayState.Closing;
                            //stop movement, prevents overlap with exit
                            Functions_Movement.StopMovement(Pool.hero.compMove);
                            Assets.Play(Assets.sfxDoorOpen);
                        }
                    }
                    else if (Obj.type == ObjType.DoorTrap)
                    {   //trap doors push ALL actors
                        Functions_Movement.Push(Actor.compMove, Obj.direction, 1.0f);
                        Functions_Entity.SpawnEntity(
                            ObjType.ParticleDashPuff,
                            Obj.compSprite.position.X,
                            Obj.compSprite.position.Y,
                            Direction.None);
                    }

                    #endregion


                    if (Obj.type == ObjType.DoorBoss) { return; } //boss doors do not center hero
                    //center Hero to Door, while still allowing him to pass thru
                    if (Obj.direction == Direction.Up || Obj.direction == Direction.Down)
                    {   //gradually center hero to door
                        Actor.compMove.magnitude.X = (Obj.compSprite.position.X - Actor.compMove.position.X) * 0.11f;
                        //if hero is close to center of door, snap/lock hero to center of door
                        if (Math.Abs(Actor.compSprite.position.X - Obj.compSprite.position.X) < 2)
                        { Actor.compMove.newPosition.X = Obj.compSprite.position.X; }
                    }
                    else
                    {   //gradually center hero to door
                        Actor.compMove.magnitude.Y = (Obj.compSprite.position.Y - Actor.compMove.position.Y) * 0.11f;
                        //if hero is close to center of door, snap/lock hero to center of door
                        if (Math.Abs(Actor.compSprite.position.Y - Obj.compSprite.position.Y) < 2)
                        { Actor.compMove.newPosition.Y = Obj.compSprite.position.Y; }
                    }
                }

                #endregion


                //switch
                //upon hero collision with switch, switch turns on, resulting in whatever event it's tied to
            }


            #region These objects interact with ALL actors

            if (Obj.group == ObjGroup.Projectile)
            {
                Functions_Battle.Damage(Actor, Obj);
            }
            else if (Obj.group == ObjGroup.Object) 
            {
                if (Obj.type == ObjType.BlockSpikes || Obj.type == ObjType.SpikesFloorOn)
                {   //damage push actor away from spikes
                    Functions_Battle.Damage(Actor, Obj);
                }
                else if (Obj.type == ObjType.ConveyorBeltOn)
                {   //belt move actors the same way we move objects
                    ConveyorBeltPush(Actor.compMove, Obj);
                }
                else if (Obj.type == ObjType.Bumper)
                {   //bounce actor off of bumper
                    Functions_Movement.Push(Actor.compMove,
                        Functions_Direction.GetRelativeDirection(
                            Obj.compSprite.position, 
                            Actor.compSprite.position), 
                            10.0f);
                    AnimateBumper(Obj);
                }
                else if (Obj.type == ObjType.PitAnimated)
                {   //push actor away from pit with a dash particle
                    Functions_Movement.Push(Actor.compMove,
                        Functions_Direction.GetRelativeDirection(
                            Obj.compSprite.position, 
                            Actor.compSprite.position),
                            1.0f);
                    Functions_Entity.SpawnEntity(
                        ObjType.ParticleDashPuff,
                        Actor.compSprite.position.X,
                        Actor.compSprite.position.Y,
                        Direction.None);
                }

                //bridge doesn't really do anything, it just doesn't cause actor to fall into a pit

                //ice tile, amplifies the actor's magnitude value in it's current direction
                //or we could modify the friction applied to the magnitude, however we choose to do it
            }

            #endregion

        }



        public static void InteractObject(GameObject ObjA, GameObject ObjB)
        {
            //ObjA is a RoomObj or Entity (projectile)
            //no blocking checks have been done yet
            //obj.interacts = projectiles, block spikes, conveyor belt
            //ObjA is the active checking object


            #region Entity Projectile Interactions

            if (ObjA.group == ObjGroup.Projectile)
            {   //handle projectile interactions with blocking gameobjects
                if (ObjB.compCollision.blocking)
                {
                    if (ObjA.type == ObjType.ProjectileExplosion)
                    {   //explosions alter certain gameobjects
                        if (ObjB.type == ObjType.DoorBombable)
                        {   //collapse the room.door
                            Functions_GameObject.SetType(ObjB, ObjType.DoorBombed);
                            CollapseDungeonDoor(ObjA); //collapse the dungeon.door
                            Assets.Play(Assets.sfxShatter);
                        }
                    }
                    else if(ObjA.type == ObjType.ProjectileBomb)
                    {   //stop bombs from moving thru blocking objects
                        Functions_Movement.StopMovement(ObjA.compMove);
                    }
                    else
                    {   //most projectiles die upon collision
                        KillProjectileUponCollision(ObjA);
                    }
                }
            }

            #endregion


            #region Non-Entity RoomObj Interactions

            else
            {   //handle non-projectile interactions (checked by type)
                if(ObjA.type == ObjType.ConveyorBeltOn)
                {   //belt move objects the same way we move actors
                    if (ObjB.compMove.moveable)
                    {   //if objB is moveable and on ground, move it
                        if (ObjB.compMove.grounded)
                        { ConveyorBeltPush(ObjB.compMove, ObjA); }
                    }
                }
                else if(ObjA.type == ObjType.BlockSpikes)
                {
                    if (ObjB.compCollision.blocking) { BounceSpikeBlock(ObjA); }
                    else if (ObjB.group == ObjGroup.Projectile) { KillProjectileUponCollision(ObjB); }
                    else if (ObjB.type == ObjType.BlockSpikes) { } //ignore spikeblock collisions
                }
                else if(ObjA.type == ObjType.Bumper)
                {
                    if (ObjB.group == ObjGroup.Projectile) { BounceOffBumper(ObjB, ObjA); }
                    else if (ObjB.type == ObjType.BlockSpikes) { BounceOffBumper(ObjB, ObjA); }
                    else if (ObjB.type == ObjType.Bumper)
                    {   //if ObjB (bumper) is moving, then bounce it, else ignore
                        if (Math.Abs(ObjB.compMove.magnitude.X) > 0 || Math.Abs(ObjB.compMove.magnitude.Y) > 0)
                        { BounceOffBumper(ObjB, ObjA); }
                        
                    }
                }

                //other obj types here
            }

            #endregion

        }



        static void CollapseDungeonDoor(GameObject Obj)
        {   //some gameobjects can collapse bombable dungeon.doors
            if (Obj.type == ObjType.ProjectileExplosion)
            {
                for(int i = 0; i < Level.doors.Count; i++)
                {   //if this explosion collides with any dungeon.door that is of type.bombable
                    if (Level.doors[i].type == DoorType.Bombable)
                    {   //change this door type to type.bombed
                        if (Obj.compCollision.rec.Intersects(Level.doors[i].rec))
                        { Level.doors[i].type = DoorType.Bombed; }
                    }
                }
            }
        }

        public static void KillProjectileUponCollision(GameObject Projectile)
        {   //these projectiles die upon a collision with another object
            if (Projectile.type == ObjType.ProjectileFireball
                || Projectile.type == ObjType.ProjectileArrow
                || Projectile.type == ObjType.ProjectileBomb)
            { Projectile.lifeCounter = Projectile.lifetime; }
        }

        public static void BounceSpikeBlock(GameObject SpikeBlock)
        {   //flip the block's direction to the opposite direction
            SpikeBlock.compMove.direction = Functions_Direction.GetOppositeDirection(SpikeBlock.compMove.direction);
            SpikeBlock.compMove.magnitude.X = 0;
            SpikeBlock.compMove.magnitude.Y = 0;
            //push the block in it's new direction, out of this collision
            Functions_Movement.Push(SpikeBlock.compMove, SpikeBlock.compMove.direction, 4.0f);
            Assets.Play(Assets.sfxMetallicTap); //play the 'clink' sound effect                               
            Functions_Entity.SpawnEntity( //show that the object has been hit
                ObjType.ParticleHitSparkle,
                SpikeBlock.compSprite.position.X + 4,
                SpikeBlock.compSprite.position.Y + 4,
                Direction.None);
        }

        public static void BounceOffBumper(GameObject Obj, GameObject Bumper)
        {   //some objects cannot be bounced
            if(Obj.type == ObjType.ProjectileExplosion) { return; }
            AnimateBumper(Bumper);
            //set object's direction to be opposite direction, update the obj's rotation
            Obj.compMove.direction = Functions_Direction.GetOppositeDirection(Obj.compMove.direction);
            Obj.direction = Obj.compMove.direction;
            Functions_GameObject.SetRotation(Obj);
            //stop obj's movement (not obj's speed tho), then push
            Obj.compMove.magnitude.X = 0;
            Obj.compMove.magnitude.Y = 0;
            Functions_Movement.Push(Obj.compMove, Obj.compMove.direction, 10.0f);
        }

        public static void AnimateBumper(GameObject Bumper)
        {   //only play the bounce sound effect if the bumper hasn't been hit this frame
            if (Bumper.compSprite.scale < 1.4f) { Assets.Play(Assets.sfxBounce); }
            Bumper.compSprite.scale = 1.5f; //scale bumper up
            Functions_Entity.SpawnEntity(
                ObjType.ParticleAttention,
                Bumper.compSprite.position.X,
                Bumper.compSprite.position.Y,
                Direction.None);
        }

        public static void ConveyorBeltPush(ComponentMovement compMove, GameObject belt)
        {   //based on belt's direction, push moveComp by amount
            Functions_Movement.Push(compMove, belt.direction, 0.1f);
            compMove.direction = belt.direction;
        }


    }
}