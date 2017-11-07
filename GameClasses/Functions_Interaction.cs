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
        public static GameObject objRef;



        public static void InteractActor(Actor Actor, GameObject Obj)
        {   //Obj can be Entity or RoomObj

            #region Handle Hero Specific Objs

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
                    Obj.lifetime = 1; Obj.lifeCounter = 2; //end the items life
                    return;
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
                            //is hero exiting a dungeon?
                            if (Level.type == LevelType.Castle)
                            { Functions_Level.CloseLevel(ExitAction.ExitDungeon); }
                            else //return to the overworld screen
                            { Functions_Level.CloseLevel(ExitAction.Overworld); }
                            Assets.Play(Assets.sfxDoorOpen);
                        }
                        //stop movement, prevents overlap with exit
                        Functions_Movement.StopMovement(Pool.hero.compMove);
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
                    return;
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
                    Functions_RoomObject.SpawnRoomObj(ObjType.PitTop,
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y, 
                        Direction.Down);
                    Functions_RoomObject.SpawnRoomObj(ObjType.PitBottom,
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y,
                        Direction.Down);
                    return; //bail from interaction check
                }

                //switch
                //upon hero collision with switch, switch turns on, resulting in whatever event it's tied to

                #endregion

            }

            #endregion


            #region Handle Actor vs Obj

            {   //these objects interact with ALL ACTORS

                #region Projectiles

                if (Obj.group == ObjGroup.Projectile)
                {   //debris rocks dont interact with actors in any way at all
                    if (Obj.type == ObjType.ProjectileDebrisRock) { return; }
                    //check for collision between net and actor
                    else if (Obj.type == ObjType.ProjectileNet)
                    {   //make sure actor isn't in hit/dead state
                        if (Actor.state == ActorState.Dead || Actor.state == ActorState.Hit) { return; }
                        Obj.lifeCounter = Obj.lifetime; //kill projectile
                        Obj.compCollision.rec.X = -1000; //hide hitBox (prevents multiple actor collisions)
                        Functions_Actor.BottleActor(Actor); //try to bottle actor
                    }

                    //all actors take damage from projectiles (fairys take 0 damage)
                    Functions_Battle.Damage(Actor, Obj); //sets actor into hit/death
                    if (Obj.type == ObjType.ProjectileSword)
                    {
                        if (Obj.lifeCounter == 1)
                        {   //if sword projectile is brand new, spawn hit particle
                            Functions_Entity.SpawnEntity(ObjType.ParticleHitSparkle, Obj);
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
                    {   //damage push actors (on ground) away from spikes
                        if (Actor.compMove.grounded) { Functions_Battle.Damage(Actor, Obj); }
                    }
                    else if (Obj.type == ObjType.ConveyorBeltOn)
                    {   //belt move actors (on ground) the same way we move objects
                        if (Actor.compMove.grounded)
                        {   //halt actor movement based on certain states
                            if (Actor.state == ActorState.Attack
                                || Actor.state == ActorState.Reward
                                || Actor.state == ActorState.Use)
                            { Functions_Movement.StopMovement(Actor.compMove); }
                            else { ConveyorBeltPush(Actor.compMove, Obj); }
                        }
                    }
                    else if (Obj.type == ObjType.Bumper)
                    {
                        BounceOffBumper(Actor.compMove, Obj);
                    }
                    else if (Obj.type == ObjType.PitAnimated)
                    {   //actors (on ground) fall into pits
                        if (Actor.compMove.grounded)
                        {

                            #region Prevent Hero's Pet from falling into pit

                            if(Actor == Pool.herosPet)
                            {   //get the opposite direction between pet's center and pit's center
                                Actor.compMove.direction = Functions_Direction.GetOppositeDirection(
                                    Actor.compSprite.position, Obj.compSprite.position);
                                //push pet in direction
                                Functions_Movement.Push(Actor.compMove, Actor.compMove.direction, 1.0f);
                                return;
                            }

                            #endregion


                            #region Continuous collision (each frame)

                            //gradually pull actor into pit's center, manually update the actor's position
                            Actor.compMove.magnitude = (Obj.compSprite.position - Actor.compSprite.position) * 0.25f;
                            //force actor to move into pit (through any blocking collisions)
                            Actor.compMove.position += Actor.compMove.magnitude;
                            Actor.compMove.newPosition = Actor.compMove.position;
                            Functions_Component.Align(Actor.compMove, Actor.compSprite, Actor.compCollision);

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
                                    { Assets.Play(Assets.sfxActorFall); }
                                    //continue falling state, scaling actor down
                                    Actor.compSprite.scale -= 0.03f;
                                }
                            }
                            //hide hero's shadow upon pit collision
                            if (Actor == Pool.hero) { Pool.heroShadow.visible = false; }

                            #endregion


                            #region End State of actor -> pit collision

                            if (Actor.compSprite.scale < 0.0f)
                            {   //actor has reached 0% scale, has fallen into pit completely
                                PlayPitFx(Obj); //fall sfx, splash fx + sfx
                                if (Actor == Pool.hero)
                                {   //send hero back to last door he passed thru
                                    //Assets.Play(Actor.sfxHit); //play hero's hit sfx
                                    Assets.Play(Assets.sfxActorLand); //play actor land sfx
                                    Functions_Hero.SpawnInCurrentRoom();
                                    Pool.heroShadow.visible = true; //unhide hero's shadow
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
                    }
                    else if (Obj.type == ObjType.IceTile)
                    {   //set the actor's friction to ice
                        Actor.compMove.friction = Actor.frictionIce;
                        //clip magnitude's maximum values for ice
                        if (Actor.compMove.magnitude.X > 1) { Actor.compMove.magnitude.X = 1; }
                        else if (Actor.compMove.magnitude.X < -1) { Actor.compMove.magnitude.X = -1; }
                        if (Actor.compMove.magnitude.Y > 1) { Actor.compMove.magnitude.Y = 1; }
                        else if (Actor.compMove.magnitude.Y < -1) { Actor.compMove.magnitude.Y = -1; }
                    }
                    //bridge doesn't really do anything, it just doesn't cause actor to fall into a pit
                }

                #endregion

            }

            #endregion

        }

        public static void InteractObject(GameObject Entity, GameObject RoomObj)
        {
            //Handle Blocking Interactions (only with projectiles)
            if (RoomObj.compCollision.blocking)
            {

                #region Arrow

                if (Entity.type == ObjType.ProjectileArrow)
                {
                    if (RoomObj.type == ObjType.SwitchBlockBtn)
                    { Functions_RoomObject.FlipSwitchBlocks(RoomObj.compSprite.position); }
                    //arrows die upon blocking collision
                    Functions_GameObject.Kill(Entity);
                }

                #endregion


                #region Bomb

                else if (Entity.type == ObjType.ProjectileBomb)
                {   //stop bombs from moving thru blocking objects
                    Functions_Movement.StopMovement(Entity.compMove);
                }

                #endregion


                #region Explosion

                else if (Entity.type == ObjType.ProjectileExplosion)
                {   //explosions alter certain roomObjects
                    if (Entity.lifeCounter == 1)
                    {   //check these collisions only once
                        if (RoomObj.type == ObjType.DoorBombable)
                        { Functions_RoomObject.CollapseDungeonDoor(RoomObj, Entity); }
                        else if (RoomObj.type == ObjType.BossStatue || RoomObj.type == ObjType.Pot)
                        { Functions_RoomObject.DestroyObject(RoomObj, true, true); }
                        else if (RoomObj.type == ObjType.SwitchBlockBtn)
                        { Functions_RoomObject.FlipSwitchBlocks(RoomObj.compSprite.position); }
                    }
                }

                #endregion


                #region Fireball

                else if (Entity.type == ObjType.ProjectileFireball)
                {   //fireballs alter certain roomObjects
                    if (RoomObj.type == ObjType.DoorBombable)
                    { Functions_RoomObject.CollapseDungeonDoor(RoomObj, Entity); }
                    else if (RoomObj.type == ObjType.BossStatue || RoomObj.type == ObjType.Pot)
                    { Functions_RoomObject.DestroyObject(RoomObj, true, true); }
                    else if (RoomObj.type == ObjType.SwitchBlockBtn)
                    { Functions_RoomObject.FlipSwitchBlocks(RoomObj.compSprite.position); }
                    //fireballs die upon blocking collision
                    Functions_GameObject.Kill(Entity);
                }

                #endregion


                #region SpikeBlock

                else if (Entity.type == ObjType.ProjectileSpikeBlock)
                {
                    BounceSpikeBlock(Entity);
                    if (RoomObj.type == ObjType.SwitchBlockBtn)
                    { Functions_RoomObject.FlipSwitchBlocks(RoomObj.compSprite.position); }
                }

                #endregion


                #region Sword

                else if (Entity.type == ObjType.ProjectileSword)
                {   //sword swipe causes soundfx to blocking objects
                    if (Entity.lifeCounter == 1) //these events happen at start of sword swing
                    {   //if sword projectile is brand new, play collision sfx
                        if (RoomObj.type == ObjType.DoorBombable)
                        { Assets.Play(Assets.sfxTapHollow); } //play hollow
                        else { Assets.Play(Assets.sfxTapMetallic); }
                        //spawn a hit sparkle particle on sword
                        Functions_Entity.SpawnEntity(ObjType.ParticleHitSparkle, Entity);
                    }
                    else if(Entity.lifeCounter == 2)
                    {   //check these collisions only once, not on first frame
                        if (RoomObj.type == ObjType.Pot)
                        { Functions_RoomObject.DestroyObject(RoomObj, true, true); }
                        else if (RoomObj.type == ObjType.SwitchBlockBtn)
                        { Functions_RoomObject.FlipSwitchBlocks(RoomObj.compSprite.position); }
                    }
                }

                #endregion


                #region Rock Debris

                else if (Entity.type == ObjType.ProjectileDebrisRock)
                {   //revert to previous position (treat as a blocking interaction)
                    Entity.compMove.newPosition.X = Entity.compMove.position.X;
                    Entity.compMove.newPosition.Y = Entity.compMove.position.Y;
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
                    if (Entity.type == ObjType.ProjectileSword
                        || Entity.type == ObjType.ProjectileNet
                        || Entity.type == ObjType.ProjectileDebrisRock)
                    { return; }

                    //stop projectile movement, bounce it
                    Entity.compMove.magnitude.X = 0;
                    Entity.compMove.magnitude.Y = 0;
                    BounceOffBumper(Entity.compMove, RoomObj);
                    //move projectile out of collision with the bumper post-bounce
                    Functions_Movement.ProjectMovement(Entity.compMove);
                    Functions_Component.Align(Entity.compMove, Entity.compSprite, Entity.compCollision);
                    //rotate bounced projectiles (ActiveObj could be a pickup)
                    if(Entity.group == ObjGroup.Projectile)
                    {
                        Entity.direction = Entity.compMove.direction;
                        Functions_GameObject.SetRotation(Entity);
                    } 
                }

                #endregion


                #region ConveyorBelt

                else if(RoomObj.type == ObjType.ConveyorBeltOn)
                {   //if Projectile is moveable and on ground, move it
                    if (Entity.compMove.moveable)
                    {
                        if (Entity.compMove.grounded)
                        { ConveyorBeltPush(Entity.compMove, RoomObj); }
                    }
                }

                #endregion


                #region Pits

                else if(RoomObj.type == ObjType.PitAnimated)
                {
                    if(Entity.compMove.grounded)
                    {   //pull projectile into pit's center, project movement, align projectile to new pos
                        Entity.compMove.magnitude = (RoomObj.compSprite.position - Entity.compSprite.position) * 0.25f;
                        //if obj is near to pit center, begin/continue falling state
                        if (Math.Abs(Entity.compSprite.position.X - RoomObj.compSprite.position.X) < 2)
                        {
                            if (Math.Abs(Entity.compSprite.position.Y - RoomObj.compSprite.position.Y) < 2)
                            {
                                if (Entity.compSprite.scale == 1.0f) //begin falling state
                                { Assets.Play(Assets.sfxActorFall); }
                                //continue falling state, scaling object down
                                Entity.compSprite.scale -= 0.03f;
                            }
                        }
                        //when a projectile hits 0 scale, simply release it
                        if (Entity.compSprite.scale < 0.0f)
                        {
                            Functions_Pool.Release(Entity);
                            PlayPitFx(RoomObj); //fall sfx, splash fx + sfx
                        }
                    }
                }

                #endregion


                #region Trap Door

                else if (RoomObj.type == ObjType.DoorTrap)
                {   //trap doors push actors in the door's facing direction, into the room
                    //projectiles should not move thru this door - bounce or be destroyed
                    //revert to previous position (treat as a blocking interaction)
                    Entity.compMove.newPosition.X = Entity.compMove.position.X;
                    Entity.compMove.newPosition.Y = Entity.compMove.position.Y;
                    if (Entity.type == ObjType.ProjectileSpikeBlock)
                    { BounceSpikeBlock(Entity); }
                    //kill all other projectiles
                    else if(Entity.type == ObjType.ProjectileFireball
                        || Entity.type == ObjType.ProjectileArrow
                        || Entity.type == ObjType.ProjectileBomb)
                    { Functions_GameObject.Kill(Entity); }
                }

                #endregion

            }


            /*
            //can check entity vs roomObj interactions like dis
            if (RoomObj.type == ObjType.SwitchBlockBtn)
            {   //timestamp any entity collision with the roomObj
                Debug.WriteLine("" + 
                    RoomObj.type + " vs " + Entity.type + 
                    " \t ts:" + ScreenManager.gameTime.TotalGameTime.Milliseconds);
            }
            */
        }



        public static void BounceOffBumper(ComponentMovement compMove, GameObject Bumper)
        {   //bounce opposite direction
            compMove.direction = Functions_Direction.GetOppositeDirection(compMove.direction);
            //if the direction is none, then get a direction between bumper and collider
            if (compMove.direction == Direction.None)
            { compMove.direction = Functions_Direction.GetOppositeDirection(compMove.position, Bumper.compSprite.position); }
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

        public static void PlayPitFx(GameObject Pit)
        {   //play splash particle effect
            Functions_Entity.SpawnEntity(ObjType.ParticleSplash,
                Pit.compSprite.position.X ,
                Pit.compSprite.position.Y - 4,
                Direction.None);
        }

    }
}