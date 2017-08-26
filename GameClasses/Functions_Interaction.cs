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
        public static GameObject objRef;


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
            {
                if (Level.bigKey)
                {   //hero must have dungeon key to open boss door
                    Functions_GameObject.SetType(Obj, ObjType.DoorOpen);
                    Assets.Play(Assets.sfxDoorOpen);
                    Functions_Entity.SpawnEntity(
                        ObjType.ParticleAttention,
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y,
                        Direction.None);
                }
                else
                {   //if hero doesn't have the bigKey, throw a dialog screen telling player this
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
        {   //Obj is non-blocking, can be Entity or RoomObj
            //these objects interact with HERO
            if (Actor == Pool.hero) 
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
                {   //handle hero interaction with exit door
                    if (Obj.type == ObjType.Exit)
                    {
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


                #region Objects

                if (Obj.type == ObjType.PitTrap)
                {   //if hero collides with a PitTrapReady, it starts to open
                    Functions_GameObject.SetType(Obj, ObjType.PitAnimated);
                    Assets.Play(Assets.sfxShatter); //play collapse sound
                    //draw attention to the collapsed floor
                    Functions_Entity.SpawnEntity(ObjType.ParticleAttention,
                        Obj.compSprite.position.X, 
                        Obj.compSprite.position.Y, 
                        Direction.Down);
                    Functions_Entity.SpawnEntity(ObjType.ParticleSmokePuff,
                        Obj.compSprite.position.X + 4, 
                        Obj.compSprite.position.Y - 8, 
                        Direction.Down);
                    //create pit teeth over new pit obj
                    objRef = Functions_Pool.GetRoomObj();
                    Functions_Movement.Teleport(objRef.compMove,
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y);
                    Functions_GameObject.SetType(objRef, ObjType.PitTop);

                    objRef = Functions_Pool.GetRoomObj();
                    Functions_Movement.Teleport(objRef.compMove,
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y);
                    Functions_GameObject.SetType(objRef, ObjType.PitBottom);
                    return; //bail from interaction check
                }

                //switch
                //upon hero collision with switch, switch turns on, resulting in whatever event it's tied to

                #endregion

            }
            //these objects interact with ALL ACTORS
            {

                #region Projectiles

                if (Obj.group == ObjGroup.Projectile)
                {
                    if (Obj.type == ObjType.ProjectileDebrisRock)
                    {   //if the rock isn't moving, actor just collided with it, inherit actor's magnitude
                        if (Obj.compMove.magnitude.X == 0 & Obj.compMove.magnitude.Y == 0)
                        {   //inherit actor's magnitude multiplied alot, so the rock quickly moves away
                            Obj.compMove.magnitude.X = Actor.compMove.magnitude.X * 5.0f;
                            Obj.compMove.magnitude.Y = Actor.compMove.magnitude.Y * 5.0f;
                        }
                    }
                    else
                    {
                        Functions_Battle.Damage(Actor, Obj);
                        if (Obj.type == ObjType.ProjectileSword)
                        {
                            if (Obj.lifeCounter == 1)
                            {   //if sword projectile is brand new, spawn hit particle
                                Functions_Entity.SpawnEntity(ObjType.ParticleHitSparkle, Obj);
                            }
                        }
                    }
                }

                #endregion


                #region Doors

                else if (Obj.group == ObjGroup.Door)
                {
                    if (Obj.type == ObjType.DoorTrap)
                    {   //trap doors push ALL actors
                        Functions_Movement.Push(Actor.compMove, Obj.direction, 1.0f);
                        Functions_Entity.SpawnEntity(
                            ObjType.ParticleDashPuff,
                            Obj.compSprite.position.X,
                            Obj.compSprite.position.Y,
                            Direction.None);
                    }
                }

                #endregion


                #region Objects

                else if (Obj.group == ObjGroup.Object)
                {
                    if (Obj.type == ObjType.SpikesFloorOn)
                    {   //damage push actor away from spikes
                        Functions_Battle.Damage(Actor, Obj);
                    }
                    else if (Obj.type == ObjType.ConveyorBeltOn)
                    {   //belt move actors the same way we move objects
                        ConveyorBeltPush(Actor.compMove, Obj);
                    }
                    else if (Obj.type == ObjType.Bumper)
                    {
                        BounceOffBumper(Actor.compMove, Obj);
                    }
                    else if (Obj.type == ObjType.PitAnimated)
                    {

                        #region Continuous collision (each frame)

                        //gradually pull actor into pit's center, manually update the actor's position
                        Actor.compMove.magnitude = (Obj.compSprite.position - Actor.compSprite.position) * 0.25f;
                        //force actor to move into pit (through any blocking collisions)
                        Actor.compMove.position += Actor.compMove.magnitude;
                        Actor.compMove.newPosition = Actor.compMove.position;
                        Functions_Component.Align(Actor.compMove, Actor.compSprite, Actor.compCollision);

                        //if this is the first frame that actor collides with pit, play fall sound effect
                        if (Actor.state != ActorState.Hit) { Assets.Play(Assets.sfxActorFall); }
                        //lock actor into hit state, prevent movement
                        Actor.state = ActorState.Hit;
                        Actor.stateLocked = true;
                        Actor.lockCounter = 0;
                        Actor.lockTotal = 45;
                        Actor.compMove.speed = 0.0f;

                        //if actor is near to pit center, begin/continue falling state
                        if (Math.Abs(Actor.compSprite.position.X - Obj.compSprite.position.X) < 2)
                        {
                            if (Math.Abs(Actor.compSprite.position.Y - Obj.compSprite.position.Y) < 2)
                            {
                                if (Actor.compSprite.scale == 1.0f) //begin actor falling state
                                {
                                    //play rising smoke puff to reinforce that actor has fallen into pit
                                    Functions_Entity.SpawnEntity(
                                        ObjType.ParticleSmokePuff,
                                        Obj.compSprite.position.X + 4,
                                        Obj.compSprite.position.Y - 8,
                                        Direction.None);
                                }
                                //continue falling state, scaling actor down
                                Actor.compSprite.scale -= 0.03f;
                            }
                        }

                        #endregion


                        #region End State of actor -> pit collision

                        if (Actor.compSprite.scale < 0.0f)
                        {   //actor has reached 0% scale, has fallen into pit completely
                            if (Actor == Pool.hero)
                            {   //send hero back to last door he passed thru
                                //Assets.Play(Actor.sfxHit); //play hero's hit sfx
                                Assets.Play(Assets.sfxActorLand); //play actor land sfx
                                Functions_Room.SpawnHeroInCurrentRoom();
                                //direct player's attention to hero's respawn pos
                                Functions_Entity.SpawnEntity(
                                    ObjType.ParticleAttention,
                                    Functions_Level.currentRoom.spawnPos.X,
                                    Functions_Level.currentRoom.spawnPos.Y,
                                    Direction.None);
                            }
                            else
                            {   //handle enemy pit death (no loot, insta-death)
                                Assets.Play(Actor.sfxDeath); //play actor death sfx
                                Functions_Pool.Release(Actor); //release this actor back to pool
                            }
                        }

                        #endregion

                    }

                    //bridge doesn't really do anything, it just doesn't cause actor to fall into a pit

                    //ice tile, amplifies the actor's magnitude value in it's current direction
                    //or we could modify the friction applied to the magnitude, however we choose to do it
                }

                #endregion

            }
        }

        public static void InteractObject(GameObject Projectile, GameObject RoomObj)
        {
            //Handle Blocking Interactions (only with projectiles)
            if (RoomObj.compCollision.blocking)
            {

                #region Arrow

                if (Projectile.type == ObjType.ProjectileArrow)
                {   //arrows die upon blocking collision
                    KillProjectileUponCollision(Projectile);
                }

                #endregion


                #region Bomb

                else if (Projectile.type == ObjType.ProjectileBomb)
                {   //stop bombs from moving thru blocking objects
                    Functions_Movement.StopMovement(Projectile.compMove);
                }

                #endregion


                #region Explosion

                else if (Projectile.type == ObjType.ProjectileExplosion)
                {   //explosions alter certain roomObjects
                    if (RoomObj.type == ObjType.DoorBombable)
                    { CollapseDungeonDoor(RoomObj, Projectile); }
                }

                #endregion


                #region Fireball

                else if(Projectile.type == ObjType.ProjectileFireball)
                {   //fireballs alter certain roomObjects
                    if (RoomObj.type == ObjType.DoorBombable)
                    { CollapseDungeonDoor(RoomObj, Projectile); }
                    //fireballs die upon blocking collision
                    KillProjectileUponCollision(Projectile);
                }

                #endregion


                #region SpikeBlock

                else if(Projectile.type == ObjType.ProjectileSpikeBlock)
                {
                    BounceSpikeBlock(Projectile);
                }

                #endregion


                #region Sword

                else if(Projectile.type == ObjType.ProjectileSword)
                {   //sword swipe causes soundfx to blocking objects
                    if(Projectile.lifeCounter == 1)
                    {   //if sword projectile is brand new, play collision sfx
                        if (RoomObj.type == ObjType.DoorBombable)
                        { Assets.Play(Assets.sfxTapHollow); } //play hollow
                        else { Assets.Play(Assets.sfxTapMetallic); }
                        //spawn a hit sparkle particle on sword
                        Functions_Entity.SpawnEntity(ObjType.ParticleHitSparkle, Projectile);
                    }
                }

                #endregion


                #region Rock Debris

                else if (Projectile.type == ObjType.ProjectileDebrisRock)
                {   //revert to previous position (treat as a blocking interaction)
                    Projectile.compMove.newPosition.X = Projectile.compMove.position.X;
                    Projectile.compMove.newPosition.Y = Projectile.compMove.position.Y;
                }

                #endregion


            }
            //Handle Non-Blocking Interactions (with projectiles & pickups)
            else
            {

                #region Bumper

                if (RoomObj.type == ObjType.Bumper)
                {
                    //some projectiles cannot be bounced off bumper
                    if (Projectile.type == ObjType.ProjectileSword
                        || Projectile.type == ObjType.ProjectileDebrisRock)
                    { return; }

                    //stop projectile movement, bounce it
                    Projectile.compMove.magnitude.X = 0;
                    Projectile.compMove.magnitude.Y = 0;
                    BounceOffBumper(Projectile.compMove, RoomObj);
                    //move projectile out of collision with the bumper post-bounce
                    Functions_Movement.ProjectMovement(Projectile.compMove);
                    Functions_Component.Align(Projectile.compMove, Projectile.compSprite, Projectile.compCollision);
                    //rotate bounced projectiles (ActiveObj could be a pickup)
                    if(Projectile.group == ObjGroup.Projectile)
                    {
                        Projectile.direction = Projectile.compMove.direction;
                        Functions_GameObject.SetRotation(Projectile);
                    } 
                }

                #endregion


                #region ConveyorBelt

                else if(RoomObj.type == ObjType.ConveyorBeltOn)
                {   //if Projectile is moveable and on ground, move it
                    if (Projectile.compMove.moveable)
                    {   
                        if (Projectile.compMove.grounded)
                        { ConveyorBeltPush(Projectile.compMove, RoomObj); }
                    }
                }

                #endregion


                #region Pits

                else if(RoomObj.type == ObjType.PitAnimated)
                {
                    if(Projectile.compMove.grounded)
                    {   //pull projectile into pit's center, project movement, align projectile to new pos
                        Projectile.compMove.magnitude = (RoomObj.compSprite.position - Projectile.compSprite.position) * 0.25f;
                        //if obj is near to pit center, begin/continue falling state
                        if (Math.Abs(Projectile.compSprite.position.X - RoomObj.compSprite.position.X) < 2)
                        {
                            if (Math.Abs(Projectile.compSprite.position.Y - RoomObj.compSprite.position.Y) < 2)
                            {
                                if (Projectile.compSprite.scale == 1.0f) //begin falling state
                                {
                                    Assets.Play(Assets.sfxActorFall);
                                    //play rising smoke puff to reinforce that object has fallen into pit
                                    Functions_Entity.SpawnEntity(
                                        ObjType.ParticleSmokePuff,
                                        RoomObj.compSprite.position.X + 4,
                                        RoomObj.compSprite.position.Y - 8,
                                        Direction.None);
                                }
                                //continue falling state, scaling object down
                                Projectile.compSprite.scale -= 0.03f;
                            }
                        }
                        //when a projectile hits 0 scale, simply release it
                        if (Projectile.compSprite.scale < 0.0f)
                        { Functions_Pool.Release(Projectile); }
                    }
                }

                #endregion


                #region Trap Door

                else if (RoomObj.type == ObjType.DoorTrap)
                {   //trap doors push actors in the door's facing direction, into the room
                    //projectiles should not move thru this door - bounce or be destroyed
                    //revert to previous position (treat as a blocking interaction)
                    Projectile.compMove.newPosition.X = Projectile.compMove.position.X;
                    Projectile.compMove.newPosition.Y = Projectile.compMove.position.Y;
                    if (Projectile.type == ObjType.ProjectileSpikeBlock)
                    { BounceSpikeBlock(Projectile); }
                    else { KillProjectileUponCollision(Projectile); }
                }

                #endregion

            }
        }



        static void CollapseDungeonDoor(GameObject Door, GameObject Projectile)
        {   //update the door roomObject, change to doorBombed, play soundfx
            Functions_GameObject.SetType(Door, ObjType.DoorOpen);
            Assets.Play(Assets.sfxShatter);

            //create a debris rock projectile at door position
            Functions_Entity.SpawnEntity(ObjType.ProjectileDebrisRock,
                Door.compSprite.position.X,
                Door.compSprite.position.Y,
                Direction.Down);

            //draw attention to the collapsing door
            Functions_Entity.SpawnEntity(ObjType.ParticleAttention,
                Door.compSprite.position.X, Door.compSprite.position.Y, Direction.Down);

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

        public static void KillProjectileUponCollision(GameObject Projectile)
        {   //these projectiles die upon a collision with another object
            if (Projectile.type == ObjType.ProjectileFireball
                || Projectile.type == ObjType.ProjectileArrow
                || Projectile.type == ObjType.ProjectileBomb)
            { Projectile.lifeCounter = Projectile.lifetime; }
        }

        public static void BounceOffBumper(ComponentMovement compMove, GameObject Bumper)
        {   //bounce opposite direction
            compMove.direction = Functions_Direction.GetOppositeDirection(compMove.direction);
            //if the direction is none, then get a direction between bumper and collider
            if (compMove.direction == Direction.None)
            { compMove.direction = Functions_Direction.GetRelativeDirection(Bumper.compSprite.position, compMove.position); }
            //push collider in direction
            Functions_Movement.Push(compMove, compMove.direction, 10.0f);
            //handle the bumper animation
            Bumper.compSprite.scale = 1.5f;
            Assets.Play(Assets.sfxBounce);
            Functions_Entity.SpawnEntity(
                ObjType.ParticleAttention,
                Bumper.compSprite.position.X,
                Bumper.compSprite.position.Y,
                Direction.None);
        }

        public static void ConveyorBeltPush(ComponentMovement compMove, GameObject belt)
        {   //based on belt's direction, push moveComp by amount
            Functions_Movement.Push(compMove, belt.direction, 0.1f);
        }

        public static void BounceSpikeBlock(GameObject SpikeBlock)
        {   //spawn a hit particle along spikeBlock's colliding edge
            Functions_Entity.SpawnEntity(ObjType.ParticleHitSparkle, SpikeBlock);
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
}