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
    public static class Functions_Actor
    {
        
        static int i;
        static int k;



        public static void ResetActor(Actor Actor)
        {   //reset actor to default living hero state
            Actor.type = ActorType.Hero;
            Actor.stateLocked = false;
            Actor.active = true;
            Actor.lockCounter = 0;
            Actor.lockTotal = 0;
            //reset actor's state and direction
            Actor.state = ActorState.Idle;
            Actor.direction = Direction.Down;
            Actor.compMove.direction = Direction.None;
            Actor.compMove.grounded = true; //most actors move on ground
            Actor.compMove.moveable = true; //all actors can move
            //reset actor's collisions
            Actor.compCollision.blocking = true;
            //reset actor's sprite 
            Actor.compSprite.zOffset = 0;
            Actor.compSprite.scale = 1.0f;
            //assume standard actor
            Actor.compSprite.drawRec.Width = 16;
            Actor.compSprite.drawRec.Height = 16;
            //setup hitbox for standard 16x16 actor
            Actor.compCollision.rec.Width = 12;
            Actor.compCollision.rec.Height = 8;
            Actor.compCollision.offsetX = -6;
            Actor.compCollision.offsetY = 0;
            //assume standard search/attack radius
            Actor.chaseRadius = 16 * 5;
            Actor.attackRadius = 14;
            //reset to above ground enemy (most are)
            Actor.underwaterEnemy = false;
        }

        public static void SpawnActor(ActorType Type, Vector2 Pos)
        {
            SpawnActor(Type, Pos.X, Pos.Y);
        }

        public static void SpawnActor(ActorType Type, float X, float Y)
        {   //grab an actor, place at X, Y position
            Actor actor = Functions_Pool.GetActor();
            SetType(actor, Type);
            Functions_Movement.Teleport(actor.compMove, X, Y);
        }

        public static void ResetActorLoadout(Actor Actor)
        {
            Actor.weapon = MenuItemType.Unknown;
            Actor.item = MenuItemType.Unknown;
            Actor.armor = MenuItemType.Unknown;
            Actor.equipment = MenuItemType.Unknown;
        }






        //animation management

        public static void SetAnimationGroup(Actor Actor)
        {
            //assume default animation speed and looping
            Actor.compAnim.speed = 10;
            Actor.compAnim.loop = true;

            //movement

            #region Idle

            if (Actor.state == ActorState.Idle)
            {
                if (Actor.swimming)
                {
                    if (Actor.underwater)
                    { Actor.animGroup = Actor.animList.underwater_idle; }
                    else { Actor.animGroup = Actor.animList.swim_idle; }
                }
                else
                {
                    Actor.animGroup = Actor.animList.idle;
                }

                //hero only actions
                if(Actor == Pool.hero & !Pool.hero.swimming)
                {   //add on carrying/grabbing anims to hero
                    if (Functions_Hero.carrying)
                    { Pool.hero.animGroup = Pool.hero.animList.idleCarry; }
                    else if (Functions_Hero.grabbing)
                    { Pool.hero.animGroup = Pool.hero.animList.grab; }
                }
            }

            #endregion


            #region Move

            else if (Actor.state == ActorState.Move)
            {
                if (Actor.swimming)
                {
                    if (Actor.underwater)
                    { Actor.animGroup = Actor.animList.underwater_move; }
                    else { Actor.animGroup = Actor.animList.swim_move; }
                }
                else
                {
                    Pool.hero.animGroup = Pool.hero.animList.move;
                }

                //hero only, not swimming actions
                if (Actor == Pool.hero & !Pool.hero.swimming)
                {
                    if (Functions_Hero.carrying)
                    { Pool.hero.animGroup = Pool.hero.animList.moveCarry; }
                    else if (Functions_Hero.grabbing)
                    { Pool.hero.animGroup = Pool.hero.animList.push; }
                }
            }

            #endregion


            //actions

            #region Dash

            else if (Actor.state == ActorState.Dash)
            {
                if (Actor.swimming)
                {
                    if (Actor.underwater) { } //do nothing
                    else { Actor.animGroup = Actor.animList.swim_dash; }
                }
                else { Actor.animGroup = Actor.animList.dash; }
            }

            #endregion


            #region Interact

            else if (Actor.state == ActorState.Interact)
            {
                if (Actor.swimming)
                { }
                else { Actor.animGroup = Actor.animList.interact; }
            }

            #endregion


            #region Attack

            else if (Actor.state == ActorState.Attack)
            {
                if (Actor.swimming)
                {
                    if (Actor.underwaterEnemy)
                    { Actor.animGroup = Actor.animList.attack; }
                }
                else { Actor.animGroup = Actor.animList.attack; }

                //hero only actions
                if (Actor == Pool.hero)
                { if (Functions_Hero.carrying) { } } //hide attack while carrying
            }

            #endregion


            #region Use

            else if (Actor.state == ActorState.Use)
            {
                if (Actor.swimming)
                {
                    if (Actor.underwaterEnemy)
                    { Actor.animGroup = Actor.animList.attack; }
                }
                else { Actor.animGroup = Actor.animList.attack; }

                //hero only actions
                if (Actor == Pool.hero)
                { if (Functions_Hero.carrying) { } } //hide use while carrying
            }

            #endregion


            #region Pickup and Throw

            else if (Actor.state == ActorState.Pickup)
            {   //only hero can be set into pickup state
                Actor.animGroup = Actor.animList.pickupThrow;
            }
            else if (Actor.state == ActorState.Throw)
            {   //only hero can be set into throw state
                Actor.animGroup = Actor.animList.pickupThrow;
            }

            #endregion


            //consequences

            #region Hit & Death

            else if (Actor.state == ActorState.Hit)
            {
                if (Actor.swimming)
                { Actor.animGroup = Actor.animList.swim_hit; }
                else { Actor.animGroup = Actor.animList.hit; }
            }
            else if (Actor.state == ActorState.Dead)
            {
                Actor.compAnim.loop = false; //stop looping
                if (Actor == Pool.hero)
                {   //play the hero's death in water or on land
                    if (Actor.swimming)
                    { Actor.animGroup = Actor.animList.death_heroic_water; }
                    else { Actor.animGroup = Actor.animList.death_heroic; }
                    Actor.compAnim.speed = 6; //speed up hero's death 
                }
                else
                {   //non-hero actors disappear upon death
                    Actor.animGroup = Actor.animList.death_blank;
                }
            }

            #endregion


            #region Reward

            else if (Actor.state == ActorState.Reward)
            {
                if (Actor.swimming)
                { Actor.animGroup = Actor.animList.swim_reward; }
                else { Actor.animGroup = Actor.animList.reward; }
            }

            #endregion


            #region Falling & Landing

            else if (Actor.state == ActorState.Falling)
            {
                Actor.animGroup = Actor.animList.falling;
            }
            else if (Actor.state == ActorState.Landed)
            {
                if (Actor.swimming)
                {
                    Actor.animGroup = Actor.animList.swim_idle;
                    Actor.lockTotal = 30;
                }
                else
                {
                    Actor.animGroup = Actor.animList.landed;
                    Actor.lockTotal = 60;
                    Actor.compAnim.speed = 30; //slow down anim
                    Actor.compAnim.loop = false;
                }
            }

            #endregion

        }

        public static void SetAnimationDirection(Actor Actor)
        {
            //set cardinal directions
            if (Actor.direction == Direction.Down) { Actor.compAnim.currentAnimation = Actor.animGroup.down; }
            else if (Actor.direction == Direction.Up) { Actor.compAnim.currentAnimation = Actor.animGroup.up; }
            else if (Actor.direction == Direction.Right) { Actor.compAnim.currentAnimation = Actor.animGroup.right; }
            else if (Actor.direction == Direction.Left) { Actor.compAnim.currentAnimation = Actor.animGroup.left; }
            //set diagonal directions
            else if (Actor.direction == Direction.DownRight) { Actor.compAnim.currentAnimation = Actor.animGroup.right; }
            else if (Actor.direction == Direction.DownLeft) { Actor.compAnim.currentAnimation = Actor.animGroup.left; }
            else if (Actor.direction == Direction.UpRight) { Actor.compAnim.currentAnimation = Actor.animGroup.right; }
            else if (Actor.direction == Direction.UpLeft) { Actor.compAnim.currentAnimation = Actor.animGroup.left; }
        }







        //actor state management

        public static void SetHitState(Actor Actor)
        {   //bail if actor is already dead (dont hit dead actors)
            if (Actor.state == ActorState.Dead) { return; }
            //lock actor into hit state, play actor hit soundfx
            Actor.state = ActorState.Hit;
            Actor.stateLocked = true;
            Actor.lockCounter = 0;
            Actor.lockTotal = 15;
            Assets.Play(Actor.sfx.hit);

            if (Actor == Pool.hero)
            {
                //hero throws whatever he's carrying
                Functions_Hero.Throw();
                //hero always spawns a gold piece upon hit
                Functions_Pickup.Spawn(PickupType.Rupee, Actor);
                //but we check flags and values to decrement supply
                if (!Flags.InfiniteGold & PlayerData.gold > 0) 
                { PlayerData.gold--; }
            }
        }

        public static void SetDeathState(Actor Actor)
        {
            //prior to hero dying, he must return to being link/hero
            if (Actor == Pool.hero) { Transform(Pool.hero, ActorType.Hero); }
            //link is only actor with correct death animation for gameover
            
            //and switching hero back also makes it easy to track enemy deaths
            else
            {   //track enemy deaths per dungeon
                //if(LevelSet.dungeon.ID == LevelID.Forest_Dungeon)
                //{ PlayerData.ForestRecord.enemyCount++; }
                //else if (LevelSet.dungeon.ID == LevelID.Mountain_Dungeon)
                //{ PlayerData.MountainRecord.enemyCount++; }
                //else if (LevelSet.dungeon.ID == LevelID.Swamp_Dungeon)
                //{ PlayerData.SwampRecord.enemyCount++; }

                PlayerData.enemiesKilled++;
            }

            //lock actor into dead state
            Actor.state = ActorState.Dead;
            Actor.stateLocked = true;
            Actor.lockCounter = 0;
            Actor.lockTotal = 255;
            Assets.Play(Actor.sfx.kill); //play actor death sound fx
            

            //Actor Specific Death Effects
            if(Actor.type == ActorType.Hero)
            {
                return; //done with hero
            }
            





            //Bosses - corpses stay

            else if (Actor.type == ActorType.Boss_BigEye
                || Actor.type == ActorType.Boss_BigBat
                || Actor.type == ActorType.Boss_OctoHead)
            {

                #region Decorate Death Event

                //octohead, bigeye pops slime instead of blood

                if (Actor.type == ActorType.Boss_BigEye || Actor.type == ActorType.Boss_OctoHead)
                {   //decorate this death as slimey
                    Functions_Particle.Spawn_Explosion(ParticleType.SlimeGreen,
                        Actor.compSprite.position.X, Actor.compSprite.position.Y, true);
                }
                else
                {   //decorate this death as bloody
                    Functions_Particle.Spawn_Explosion(ParticleType.BloodRed,
                        Actor.compSprite.position.X, Actor.compSprite.position.Y, true);
                }

                #endregion


                #region Cleanup any boss mobs leftover

                if (Actor.type == ActorType.Boss_OctoHead)
                {
                    //loop over all actors, checking for active tentacles, and kill em'
                    for (k = 0; k < Pool.actorCount; k++)
                    {
                        if (Pool.actorPool[k].active & Pool.actorPool[k].type == ActorType.Special_Tentacle)
                        { SetDeathState(Pool.actorPool[k]); }
                    }
                }

                #endregion


                #region Handle Dungeon Progress / Boss Death Events

                //specific boss must die in specific boss room to end dungeon/advance progress
                if (Actor.type == ActorType.Boss_BigEye &
                    LevelSet.currentLevel.currentRoom.roomID == RoomID.ForestIsland_BossRoom)
                {
                    PlayerData.ForestRecord.timer.Stop();
                    PlayerData.story_forestDungeon = true;
                    Functions_Level.CloseLevel(ExitAction.Overworld);
                    Assets.Play(Assets.sfxBeatDungeon);
                }
                else if(Actor.type == ActorType.Boss_BigBat &
                    LevelSet.currentLevel.currentRoom.roomID == RoomID.DeathMountain_BossRoom)
                {
                    PlayerData.MountainRecord.timer.Stop();
                    PlayerData.story_mountainDungeon = true;
                    Functions_Level.CloseLevel(ExitAction.Overworld);
                    Assets.Play(Assets.sfxBeatDungeon);
                }
                else if(Actor.type == ActorType.Boss_OctoHead &
                    LevelSet.currentLevel.currentRoom.roomID == RoomID.HauntedSwamps_BossRoom)
                {
                    PlayerData.SwampRecord.timer.Stop();
                    PlayerData.story_swampDungeon = true;
                    Functions_Level.CloseLevel(ExitAction.Overworld);
                    Assets.Play(Assets.sfxBeatDungeon);
                }

                //handle additional bosses dying in dungeon boss rooms here
                //for now we just use bigBat to model all other dungeon bosses
                //this is just a catch all until we have real bosses for these dungeons
                else if(Actor.type == ActorType.Boss_BigBat)
                {
                    if(
                        LevelSet.currentLevel.currentRoom.roomID == RoomID.ThievesHideout_BossRoom ||
                        LevelSet.currentLevel.currentRoom.roomID == RoomID.LavaIsland_BossRoom ||
                        LevelSet.currentLevel.currentRoom.roomID == RoomID.CloudIsland_BossRoom ||
                        LevelSet.currentLevel.currentRoom.roomID == RoomID.SkullIsland_BossRoom
                        )
                    {
                        Functions_Level.CloseLevel(ExitAction.Overworld);
                        Assets.Play(Assets.sfxBeatDungeon);
                    }
                }

                #endregion

            }







            #region Minibosses - corpses stay

            else if(Actor.type == ActorType.MiniBoss_BlackEye)
            {   //decorate this death as special / explosive
                Functions_Particle.Spawn_Explosion(ParticleType.DebrisBrown,
                    Actor.compSprite.position.X, Actor.compSprite.position.Y, true);
                Actor.compAnim.speed = 15; //slow down death animation to taste
                //this actor becomes debris on floor, sort to floor.
                Actor.compSprite.zOffset = -8;
                Functions_Component.SetZdepth(Actor.compSprite);
                //try to drop map
                Functions_InteractiveObjs.DropMap(
                    Actor.compSprite.position.X, 
                    Actor.compSprite.position.Y);
            }
            else if (Actor.type == ActorType.MiniBoss_Spider_Armored)
            {   
                //decorate this death as special / explosive
                Functions_Particle.Spawn_Explosion(ParticleType.DebrisBrown,
                    Actor.compSprite.position.X, Actor.compSprite.position.Y, true);
                //armored spider becomes unarmored version
                SetType(Actor, ActorType.MiniBoss_Spider_Unarmored);
                //audibly alert player battle isn't over
                Assets.Play(Assets.sfxBossHit);
            }
            else if (Actor.type == ActorType.MiniBoss_Spider_Unarmored)
            {   
                //decorate this death as special / explosive
                Functions_Particle.Spawn_Explosion(ParticleType.BloodRed,
                    Actor.compSprite.position.X, Actor.compSprite.position.Y, true);
                Actor.compAnim.speed = 10; //slow down death animation to taste
                //this actor becomes debris on floor, sort to floor.
                Actor.compSprite.zOffset = -8;
                Functions_Component.SetZdepth(Actor.compSprite);
                //try to drop map
                Functions_InteractiveObjs.DropMap(
                    Actor.compSprite.position.X,
                    Actor.compSprite.position.Y);
            }
            else if (Actor.type == ActorType.MiniBoss_OctoMouth)
            {
                //decorate this death as special / explosive
                Functions_Particle.Spawn_Explosion(ParticleType.SlimeGreen,
                    Actor.compSprite.position.X, Actor.compSprite.position.Y, true);
                //Actor.compAnim.speed = 10; //slow down death animation to taste
                //this actor becomes debris in water, sort to floor.
                Actor.compSprite.zOffset = -8;
                Functions_Component.SetZdepth(Actor.compSprite);
                //try to drop map
                Functions_InteractiveObjs.DropMap(
                    Actor.compSprite.position.X,
                    Actor.compSprite.position.Y);
            }

            #endregion









            else//this is all other actor types: blob, angry eye, etc..
            {   //decorate actor's death with blast, loot, blood, skeleton
                Functions_Particle.Spawn(ParticleType.Blast, Actor);
                Functions_InteractiveObjs.DecorateEnemyDeath(Actor.compSprite, true);
                Functions_Pool.Release(Actor);
            }



            //this is all actors except hero, who bails from method early
            Actor.compCollision.blocking = false; //make dead actor's corpse passable
        }

        public static void SetItemUseState(Actor Actor)
        {   //set actor into a short paused state
            Actor.stateLocked = true;
            Actor.lockCounter = 0;
            Actor.lockTotal = 15;
        }

        public static void SetIdleState(Actor Actor)
        {
            Actor.state = ActorState.Idle;
            Actor.stateLocked = false;
            Actor.lockCounter = 0;
            Actor.lockTotal = 0;
        }

        //actor interaction helpers

        public static void Transform(Actor Actor, ActorType Type)
        {   //transforms one actor into another
            SetType(Actor, Type);

            if (Actor == Pool.hero)
            {   //store actor type
                PlayerData.actorType = Actor.type;
            }

            Functions_Particle.Spawn(ParticleType.Attention, Actor);
        }

        //actor in water helpers

        public static void DisplayWetFeet(Actor Actor)
        {
            //bail if actor is dead, most dead actors dont display anything
            if (Actor.state == ActorState.Dead) { return; }
            Actor.feetFX.visible = true;
            Actor.feetAnim.currentAnimation = AnimationFrames.ActorFX_WetFeet;
            if (Actor == Pool.hero & Actor.state == ActorState.Move)
            {   //actor must be hero, above water on land, and moving
                Assets.Play(Assets.sfxWaterWalk);
            }
        }

        public static void Breathe(Actor Actor)
        {
            if (Actor.underwater)
            {   //if actor has been underwater for awhile, force them to come up for air
                Actor.breathCounter++;
                if (Actor.breathCounter > Actor.breathTotal)
                {
                    Actor.underwater = false;
                    CreateSplash(Actor);
                    Actor.breathCounter = 0;
                }
            }
        }

        public static void CreateSplash(Actor Actor)
        {

            #region Water Enemies get extra and specific splashes

            //decorate water transition based on actor type
            if (Actor.type == ActorType.Boss_OctoHead)
            {   //dis burlyboy requires more than one splash to look good
                //front
                Functions_Particle.Spawn(
                    ParticleType.Splash,
                    Actor.compSprite.position.X - 8,
                    Actor.compSprite.position.Y + 20);
                Functions_Particle.Spawn(
                    ParticleType.Splash,
                    Actor.compSprite.position.X + 8,
                    Actor.compSprite.position.Y + 20);
                //back
                Functions_Particle.Spawn(
                    ParticleType.Splash,
                    Actor.compSprite.position.X - 8,
                    Actor.compSprite.position.Y - 2);
                Functions_Particle.Spawn(
                    ParticleType.Splash,
                    Actor.compSprite.position.X + 8,
                    Actor.compSprite.position.Y - 2);
                //and side to side
                Functions_Particle.Spawn(
                    ParticleType.Splash,
                    Actor.compSprite.position.X - 16,
                    Actor.compSprite.position.Y + 10);
                Functions_Particle.Spawn(
                    ParticleType.Splash,
                    Actor.compSprite.position.X + 16,
                    Actor.compSprite.position.Y + 10);
            }
            else if(Actor.type == ActorType.MiniBoss_OctoMouth)
            {
                Functions_Particle.Spawn(
                    ParticleType.Splash,
                    Actor.compSprite.position.X,
                    Actor.compSprite.position.Y + 2);
            }
            else if (Actor.type == ActorType.Special_Tentacle)
            {   //taller actor, offset splash towards bottom
                Functions_Particle.Spawn(
                    ParticleType.Splash,
                    Actor.compSprite.position.X,
                    Actor.compSprite.position.Y + 12);
            }

            #endregion


            else
            {   //all other actors are small enough to use just one splash
                Functions_Particle.Spawn(
                    ParticleType.Splash,
                    Actor.compSprite.position.X,
                    Actor.compSprite.position.Y - 2);
            }
        }

        static byte dashWaveOffset = 0;
        public static void CreateSwimDashWave(Actor Actor)
        {   //place water kick fx behind actor
            //this will always be at base of actor, so use offset


            #region Tall Actors in Water

            //taller actors get a larger wave offset
            if (Actor.type == ActorType.Boss_OctoHead)
            {
                dashWaveOffset = 16;
            }
            else if(Actor.type == ActorType.Special_Tentacle)
            {   
                dashWaveOffset = 12;
            }

            #endregion


            else
            {   //these are 16x16 actors
                dashWaveOffset = 2;
            }

            //create water kick particle with offset for actor
            Functions_Particle.Spawn(
                ParticleType.WaterKick,
                Actor.compSprite.position.X,
                Actor.compSprite.position.Y + dashWaveOffset,
                Functions_Direction.GetOppositeDirection(Actor.direction));
        }



        



        //a complicated mess of state and actor booleans, tread carefully

        public static void Update(Actor Actor)
        {

            #region Actor is not Statelocked

            if (Actor.stateLocked == false)
            {
                //set actor moving/facing direction
                if (Actor.compInput.direction != Direction.None)
                { Actor.direction = Actor.compInput.direction; }

                Actor.state = Actor.inputState; //sync state to input state
                Actor.lockCounter = 0; //reset lock counter in case actor statelocks
                Actor.lockTotal = 0; //reset lock total
                Actor.compMove.speed = Actor.walkSpeed; //default to walk speed


                #region Map Interact/Dash/Attack/Use

                //Swimming Interact/Dash/Attack/Use Mapping, in water
                if (Actor.swimming)
                {   
                    Actor.compMove.speed = Actor.swimSpeed;

                    #region Swim Interact

                    if (Actor.state == ActorState.Interact)
                    {   //only hero has swim interactions
                        if (Actor == Pool.hero) { Functions_Hero.CheckInteractionRec(); }
                    }

                    #endregion


                    #region Swim Dash

                    else if (Actor.state == ActorState.Dash)
                    {
                        Actor.lockTotal = 15;
                        Actor.stateLocked = true;

                        if (Actor.compInput.direction == Direction.Down
                            || Actor.compInput.direction == Direction.Up
                            || Actor.compInput.direction == Direction.Left
                            || Actor.compInput.direction == Direction.Right)
                        {   //cardinal swim dash - full power push + confirmation particle
                            Functions_Movement.Push(
                                Actor.compMove,
                                Actor.compInput.direction, 2.0f);
                            Assets.Play(Assets.sfxWaterSwim);
                            CreateSwimDashWave(Actor);
                        }
                        else
                        {   //diagonal swim dash - half power push
                            Functions_Movement.Push(
                                Actor.compMove,
                                Actor.compInput.direction, 1.0f);
                            Assets.Play(Assets.sfxWaterSwim);
                        }
                    }

                    #endregion


                    #region Swim Attack (Hero Dive)

                    else if (Actor.state == ActorState.Attack)
                    {

                        if(Actor == Pool.hero)
                        {   
                            //if hero is link or blob, then he dives like normal
                            //all other actors are flying and instead attack
                            
                            //only hero can dive on new X button press
                            if(Input.Player1.X & Input.Player1.X_Prev == false)
                            {   

                                //only link and blob have proper hero diving animations
                                if(Pool.hero.type == ActorType.Hero
                                    || Pool.hero.type == ActorType.Blob
                                    //water based actors have dive animations too
                                    || Pool.hero.type == ActorType.MiniBoss_OctoMouth
                                    || Pool.hero.type == ActorType.Boss_OctoHead
                                    || Pool.hero.type == ActorType.Special_Tentacle
                                    )
                                {
                                    //toggle dive state
                                    if (Actor.underwater == false) //setup new dive
                                    { Actor.underwater = true; Actor.breathCounter = 0; }
                                    else { Actor.underwater = false; }
                                    CreateSplash(Actor); //create right splash for actor type
                                }
                                else
                                {   //use the weapon item, assuming actor is above water (flying)
                                    Functions_Item.UseItem(Actor.weapon, Actor);
                                    WorldUI.currentWeapon.compSprite.scale = 2.0f;
                                }
                            }
                        }

                        else
                        {   //npcs can use weapons while in swimming state
                            Functions_Item.UseItem(Actor.weapon, Actor);
                        }
                    }

                    #endregion


                    #region Swim Use

                    else if (Actor.state == ActorState.Use)
                    {
                        if (Actor == Pool.hero)
                        {
                            //the hero can use items in the water, but only as certain actors
                            if(Pool.hero.type != ActorType.Hero & Pool.hero.type != ActorType.Blob)
                            {
                                //link and the blob cannot use items in the water, others can
                                Functions_Item.UseItem(Actor.item, Actor);
                                WorldUI.currentItem.compSprite.scale = 2.0f;
                            }
                        }
                        else
                        {
                            //npc actors can use items in the water
                            //this allows enemies ai to function properly
                            Functions_Item.UseItem(Actor.item, Actor);
                        }
                    }

                    #endregion

                }
                //Non-Swimming Interact/Dash/Attack/Use Mapping, on land
                else
                {   

                    #region Interact

                    if (Actor.state == ActorState.Interact)
                    {   //A button press
                        if (Actor == Pool.hero)
                        {   //if hero is carrying, throw held obj
                            if (Functions_Hero.carrying)
                            {
                                Functions_Movement.StopMovement(Pool.hero.compMove);
                                Functions_Hero.Throw();
                            }
                            else
                            {
                                Functions_Hero.CheckInteractionRec();
                            }
                        }

                        //non-hero actors have no A button/interact input mapping
                    }

                    #endregion


                    #region Dash

                    else if (Actor.state == ActorState.Dash)
                    {   //B button 
                        Actor.lockTotal = 10;
                        Actor.stateLocked = true;
                        Actor.compMove.speed = Actor.dashSpeed;
                        Functions_Particle.Spawn(ParticleType.RisingSmoke, Actor);
                        Assets.Play(Actor.sfxDash);
                    }

                    #endregion


                    #region Attack

                    else if (Actor.state == ActorState.Attack)
                    {   //X button
                        if (Actor == Pool.hero)
                        {
                            if (Functions_Hero.carrying)
                            {
                                //do nothing
                            }
                            else
                            {
                                //scale up worldUI weapon 
                                WorldUI.currentWeapon.compSprite.scale = 2.0f;
                                //hero has special skills based on equipped weapons
                                if (Pool.hero.weapon == MenuItemType.WeaponShovel) { Functions_Dig.Dig(); }
                                //hero uses weapon
                                Functions_Item.UseItem(Actor.weapon, Actor);
                            }
                        }
                        else
                        {   //nonhero's always use weapon
                            Functions_Item.UseItem(Actor.weapon, Actor);
                        }

                    }

                    #endregion


                    #region Use

                    else if (Actor.state == ActorState.Use)
                    {   //Y button
                        if (Actor == Pool.hero)
                        {
                            if (Functions_Hero.carrying)
                            {
                                //do nothing
                            }
                            else
                            {
                                //scale up the worldUI item
                                WorldUI.currentItem.compSprite.scale = 2.0f;
                                //hero uses item
                                Functions_Item.UseItem(Actor.item, Actor);
                            }
                        }
                        else
                        {   //nonhero's always use item
                            Functions_Item.UseItem(Actor.item, Actor);
                        }
                    }

                    #endregion

                }

                #endregion


                #region Append Hero PUSHING RoomObjs

                //hero in non-swimming state, handling push/grab obj
                if(Actor == Pool.hero & Functions_Hero.grabbing)
                {
                    //bail if grabbing from a swim state
                    if (Actor.swimming) { return; }
                    else
                    {   //make sure A button is down
                        if (Input.Player1.A)
                        {   //alter hero's friction - slow
                            Actor.compMove.friction = World.frictionUse;
                            //push obj hero has grabbed
                            Functions_Hero.PushGrabbedObj();
                        }
                        else
                        {   //release the grabbed object
                            Functions_Hero.grabbing = false;
                            Functions_Hero.grabbedObj = null;
                        }
                    }
                }

                #endregion

            }

            #endregion


            #region Actor is Statelocked

            else
            {
                //play the statelocked animation / count state
                Actor.lockCounter++; //increment lock counter
                if (Actor.lockCounter > Actor.lockTotal) //check against lock total
                {
                    Actor.stateLocked = false; //unlock actor
                    Functions_Input.ResetInputData(Actor.compInput); //reset input component
                    //check to see if the actor is dead
                    if (Actor.health <= 0) { SetDeathState(Actor); }
                }

                if (Actor.state == ActorState.Attack)
                {   //when an actor attacks, they slow down for the duration
                    Actor.compMove.friction = World.frictionAttack;
                }
                else if (Actor.state == ActorState.Use)
                {   //when an actor uses something, they slow down for the duration
                    Actor.compMove.friction = World.frictionUse;
                }
                else if (Actor.state == ActorState.Hit)
                {   //zero out the actor's speed
                    Actor.compMove.speed = 0.0f;
                    //this prevents actor from affecting the hit's push via directional input
                    //this also gives hits ALOT more push
                }
                else if (Actor.state == ActorState.Dead)
                {   //check death state
                    Actor.lockCounter = 0; //lock actor into state
                    Actor.health = 0; //lock actor's health at 0
                    if (Actor == Pool.hero) { Functions_Hero.HandleDeath(); }
                }
                else if (Actor.state == ActorState.Falling)
                {   //lock actor into state
                    Actor.lockCounter = 0; 
                    Actor.lockTotal = 60;
                    Actor.compMove.speed = Actor.walkSpeed;


                    #region Allow Hero to grab while falling via A button press

                    if (Actor == Pool.hero)
                    {
                        //only link and blob have proper animframes for climbing
                        if (Pool.hero.type == ActorType.Hero || Pool.hero.type == ActorType.Blob)
                        {
                            //this means player can't hold down A to auto-grab - must time it right
                            if (Input.Player1.A & Input.Player1.A_Prev == false)
                            {

                                #region Resolve input direction to left or right

                                if (Pool.hero.compInput.direction == Direction.Up
                                    || Pool.hero.compInput.direction == Direction.Down)
                                { Pool.hero.compInput.direction = Direction.None; }

                                else if (Pool.hero.compInput.direction == Direction.UpLeft
                                    || Pool.hero.compInput.direction == Direction.DownLeft)
                                { Pool.hero.compInput.direction = Direction.Left; }

                                else if (Pool.hero.compInput.direction == Direction.UpRight
                                    || Pool.hero.compInput.direction == Direction.DownRight)
                                { Pool.hero.compInput.direction = Direction.Right; }

                                #endregion


                                //set hero.direction based on hero input *right now*
                                Pool.hero.direction = Pool.hero.compInput.direction;
                                //this direction will be used in check() below
                                //to set where interaction point is placed
                                //however, we dont want any up direction

                                //check to see if hero can grab a foothold/ladder
                                Functions_Hero.CheckInteractionRec();


                                #region Hero Missed the Grab

                                //if hero grabbed, he is now in climbing state
                                if (Actor.state != ActorState.Climbing)
                                {   //hero did not grab a foothold/ladder, continues fall south
                                    //reset actor direction down
                                    Actor.direction = Direction.Down;
                                }

                                #endregion


                                #region Hero Made the Grab!

                                else
                                {
                                    //hero successfully grabbed, from a fall
                                    //that's pretty crispy, so we should celebrate a little

                                    //place a (!) above hero's head as recognition
                                    Functions_Particle.Spawn(
                                        ParticleType.ExclamationBubble,
                                        Actor.compSprite.position.X,
                                        Actor.compSprite.position.Y - 4,
                                        Direction.Down);
                                    //^ nowhere else can hero 'spawn' a (!)

                                    //play a special soundfx
                                    Assets.Play(Assets.sfxActorLand);
                                    //track successful wall jumps
                                    PlayerData.recorded_wallJumps++;
                                    //check wall jump achievements
                                    Functions_Hero.CheckAchievements(Achievements.WallJumps);
                                }

                                #endregion

                            }
                        }
                    }

                    #endregion

                }
                else if(Actor.state == ActorState.Landed)
                {   //actor has landed on ground, not touching wall obj
                    //actor inherit's counter + total from falling state above
                    if (Actor.lockCounter == 1) //on first frame of landing..
                    {   //pop attention and play landing sfx
                        Functions_Particle.Spawn(ParticleType.Attention, Actor);
                        Assets.Play(Assets.sfxActorLand);
                        Functions_Movement.StopMovement(Actor.compMove);
                    }
                }
                else if (Actor.state == ActorState.Climbing)
                {
                    if (Actor == Pool.hero)
                    {

                        #region Allow directional input

                        if (Actor.compInput.direction != Direction.None)
                        {   //set direction and moving animation
                            Actor.direction = Actor.compInput.direction;
                            Actor.compAnim.currentAnimation = AnimationFrames.Hero_Animations.climbing.down;
                            Actor.compAnim.speed = 10; //normal
                            Assets.Play(Assets.sfxMapWalking);
                        }
                        else
                        {   //set idle animation
                            Actor.compAnim.currentAnimation = AnimationFrames.Hero_Animations.climbing.down;
                            Actor.compAnim.speed = 255; //extremely slow, feels like an idle
                        }

                        #endregion


                        Actor.compMove.speed = Actor.swimSpeed; //move slowly while climbing


                        #region Set Actor State to Falling, Climbing, or Idle

                        Actor.lockCounter = 0; //lock into state

                        //borrow lockTotal to model outcome states 
                        //lockTotal = irrelevant, cuz lockcounter gets reset to 0 above
                        //if you don't understand the two lines ^above^, dont touch
                        //the code below...

                        Actor.lockTotal = 0;
                        //0 = falling
                        //1 = climbing
                        //2 = finished climbing
                    
                        for (i = 0; i < Pool.intObjCount; i++)
                        {   //find an active foothold obj
                            if (Pool.intObjPool[i].active)
                            {
                                //prove actor is nearby foothold to maintain grip
                                if (Pool.intObjPool[i].type == InteractiveType.MountainWall_Foothold ||
                                    Pool.intObjPool[i].type == InteractiveType.MountainWall_Ladder)
                                {
                                    //if foothold does not contain center point of actor, actor loses grip and falls
                                    if (Pool.intObjPool[i].compCollision.rec.Contains(Actor.compSprite.position))
                                    {   //Actor.state = ActorState.Climbing;
                                        Actor.lockTotal = 1;
                                    }
                                }
                            
                                //actors touching top wall return to idle from climbing
                                else if(Pool.intObjPool[i].type == InteractiveType.MountainWall_Top)
                                {
                                    if (Pool.intObjPool[i].compCollision.rec.Contains(Actor.compSprite.position))
                                    {   //Actor.state = ActorState.Idle;
                                        Actor.lockTotal = 2; 
                                    }
                                }

                            }
                        }

                        //based on state, set actor state
                        if (Actor.lockTotal == 0)
                        { Actor.state = ActorState.Falling; Actor.stateLocked = true; }
                        else if(Actor.lockTotal == 1)
                        { Actor.state = ActorState.Climbing; Actor.stateLocked = true; }
                        else if (Actor.lockTotal == 2)
                        { Actor.state = ActorState.Idle; Actor.stateLocked = false; }

                        //leave Actor.lockTotal alone
                        Actor.lockTotal = 100;

                        #endregion


                        #region Allow player to jump off the wall alittle bit using B button

                        if(Input.Player1.B & Input.Player1.B_Prev == false)
                        {
                            Actor.state = ActorState.Falling;
                            //push actor north to fake a jump
                            Functions_Movement.Push(Pool.hero.compMove, Direction.Up, 10.0f);
                        }

                        #endregion
                        
                        
                        //we manually set the animation frames, bail from rest of method
                        return;
                    }
                }
            }

            #endregion


            //set actor animation and direction
            SetAnimationGroup(Actor);
            SetAnimationDirection(Actor);
        }

        //maps type to object values and state

        public static void SetType(Actor Actor, ActorType Type)
        {
            ResetActor(Actor);
            Actor.type = Type;
            




            //Standard Actors

            #region Hero

            if (Type == ActorType.Hero)
            {
                Actor.enemy = false;
                Actor.compSprite.texture = Assets.heroSheet;
                Actor.animList = AnimationFrames.Hero_Animations; //actor is hero
                //do not update/change the hero's weapon/item/armor/equipment
                Actor.walkSpeed = 0.35f;
                Actor.dashSpeed = 0.90f;
                //set actor sound effects
                Actor.sfxDash = Assets.sfxHeroDash;
                Actor.sfx.hit = Assets.sfxHeroHit;
                Actor.sfx.kill = Assets.sfxHeroKill;
            }

            #endregion


            #region Blob

            else if (Type == ActorType.Blob)
            {
                Actor.aiType = ActorAI.Basic;
                Actor.enemy = true;
                Actor.compSprite.texture = Assets.blobSheet;
                Actor.animList = AnimationFrames.Hero_Animations; //actor is hero
                Actor.health = 3; //base hero hp
                ResetActorLoadout(Actor);
                Actor.weapon = MenuItemType.WeaponSword;
                Actor.walkSpeed = 0.05f;
                Actor.dashSpeed = 0.30f;
                //set actor sound effects
                Actor.sfxDash = Assets.sfxBlobDash;
                Actor.sfx.hit = Assets.sfxEnemyHit;
                Actor.sfx.kill = Assets.sfxEnemyKill;
            }

            #endregion


            #region Standard - AngryEye

            else if (Type == ActorType.Standard_AngryEye)
            {
                Actor.aiType = ActorAI.Basic;
                Actor.compMove.grounded = false; //is flying

                Actor.enemy = true;
                Actor.compSprite.texture = Assets.EnemySheet;
                Actor.animList = AnimationFrames.Standard_AngryEye_Animations;
                Actor.health = 1; //very easy
                ResetActorLoadout(Actor);
                Actor.weapon = MenuItemType.WeaponFang;
                Actor.walkSpeed = 0.05f;
                Actor.dashSpeed = 0.30f;

                //this actor is a 2x1 enemy
                Actor.compSprite.drawRec.Width = 16 * 1;
                Actor.compSprite.drawRec.Height = 16 * 2;
                //actor is floating in air
                Actor.compSprite.zOffset = 16;

                //set actor sound effects
                Actor.sfxDash = null; //silent dash
                Actor.sfx.hit = Assets.sfxEnemyHit;
                Actor.sfx.kill = Assets.sfxEnemyKill;

                //custom hitbox
                Actor.compCollision.rec.Width = 12;
                Actor.compCollision.rec.Height = 10;
                Actor.compCollision.offsetX = -6;
                Actor.compCollision.offsetY = -9;
            }

            #endregion


            #region Standard - BeefyBat

            else if (Type == ActorType.Standard_BeefyBat)
            {
                Actor.aiType = ActorAI.Standard_BeefyBat;
                Actor.chaseDiagonally = false; //sprite requires it
                Actor.compMove.grounded = false; //is flying

                Actor.enemy = true;
                Actor.compSprite.texture = Assets.EnemySheet;
                Actor.animList = AnimationFrames.Standard_BeefyBat_Animations;

                Actor.health = 2; //med
                Actor.chaseRadius = 16 * 3; //decrease chase radius

                ResetActorLoadout(Actor);
                Actor.weapon = MenuItemType.WeaponFang;
                Actor.walkSpeed = 0.05f;
                Actor.dashSpeed = 0.30f;

                //actor is flying, low
                Actor.compSprite.zOffset = 16;

                //set actor sound effects
                Actor.sfxDash = null; //silent dash
                Actor.sfx.hit = Assets.sfxEnemyHit;
                Actor.sfx.kill = Assets.sfxEnemyKill;

                //custom hitbox
                Actor.compCollision.rec.Width = 12;
                Actor.compCollision.rec.Height = 8;
                Actor.compCollision.offsetX = -6;
                Actor.compCollision.offsetY = -6;
            }

            #endregion















            //Minibosses

            #region Blackeye

            else if (Type == ActorType.MiniBoss_BlackEye)
            {
                Actor.aiType = ActorAI.Miniboss_Blackeye;
                Actor.compMove.grounded = false; //is flying

                Actor.enemy = true;
                Actor.compSprite.texture = Assets.EnemySheet;
                Actor.animList = AnimationFrames.MiniBoss_BlackEye_Animations;
                Actor.health = 15;

                //walk and dash speeds are set in Functions_Ai
                //because they change based on this actor's health

                //this actor is a 2x2 miniboss
                Actor.compSprite.drawRec.Width = 16 * 2;
                Actor.compSprite.drawRec.Height = 16 * 2;

                Actor.compCollision.offsetY = -4;
                //actor is floating in air
                Actor.compSprite.zOffset = 16;
                //set actor sound effects
                Actor.sfxDash = null; //silent dash
                Actor.sfx.hit = Assets.sfxBossHit;
                Actor.sfx.kill = Assets.sfxShatter; //sounds good

                ResetActorLoadout(Actor);
                //setup actor with useable item, so player can shoot arrows
                Actor.item = MenuItemType.ItemBow;

                //enemy with a bigger hitbox
                Actor.compCollision.rec.Width = 18;
                Actor.compCollision.rec.Height = 14;
                Actor.compCollision.offsetX = -8;
                Actor.compCollision.offsetY = -8;
            }

            #endregion


            #region Spider - Armored

            else if (Type == ActorType.MiniBoss_Spider_Armored)
            {
                Actor.aiType = ActorAI.Basic;

                Actor.enemy = true;
                Actor.compSprite.texture = Assets.EnemySheet;
                Actor.animList = AnimationFrames.MiniBoss_SpiderArmored_Animations;
                Actor.health = 1; //simply takes 1 damage, then becomes unarmored

                ResetActorLoadout(Actor);
                Actor.weapon = MenuItemType.WeaponFang;

                //moves slowly
                Actor.walkSpeed = 0.05f;
                Actor.dashSpeed = 0.30f;

                //this actor is a 3x1 miniboss
                Actor.compSprite.drawRec.Width = 16 * 3;
                Actor.compSprite.drawRec.Height = 16 * 1;
                Actor.compCollision.offsetY = 0;
                Actor.compSprite.zOffset = 0;

                //set actor sound effects
                Actor.sfxDash = Assets.sfxEnemyTaunt;
                Actor.sfx.hit = Assets.sfxTapMetallic;
                Actor.sfx.kill = Assets.sfxShatter;

                Actor.compCollision.rec.Width = 16;
                Actor.compCollision.rec.Height = 10;
                Actor.compCollision.offsetX = -8;
                Actor.compCollision.offsetY = -2;
            }

            #endregion


            #region Spider - UN-armored

            else if (Type == ActorType.MiniBoss_Spider_Unarmored)
            {
                Actor.aiType = ActorAI.Basic;
                Actor.compCollision.blocking = true; //actor was 'dead'

                Actor.enemy = true;
                Actor.compSprite.texture = Assets.EnemySheet;
                Actor.animList = AnimationFrames.MiniBoss_SpiderUnarmored_Animations;
                Actor.health = 5; //actual miniboss

                ResetActorLoadout(Actor);
                Actor.weapon = MenuItemType.WeaponFang;

                //moves extremely fast
                Actor.walkSpeed = 0.75f;
                Actor.dashSpeed = 1.5f;
                Actor.compAnim.speed = 5;

                //always 'see' hero
                Actor.chaseRadius = 16 * 30;

                //this actor is a 3x1 miniboss
                Actor.compSprite.drawRec.Width = 16 * 3;
                Actor.compSprite.drawRec.Height = 16 * 1;
                Actor.compCollision.offsetY = 0;
                Actor.compSprite.zOffset = 16;

                //set actor sound effects
                Actor.sfxDash = Assets.sfxEnemyTaunt;
                Actor.sfx.hit = Assets.sfxBossHit;
                Actor.sfx.kill = Assets.sfxBossHitDeath;

                Actor.compCollision.rec.Width = 16;
                Actor.compCollision.rec.Height = 10;
                Actor.compCollision.offsetX = -8;
                Actor.compCollision.offsetY = -2;
            }


            #endregion


            #region OctoMouth

            else if (Type == ActorType.MiniBoss_OctoMouth)
            {
                Actor.aiType = ActorAI.MiniBoss_OctoMouth;
                Actor.compMove.grounded = true;

                Actor.enemy = true;
                Actor.underwaterEnemy = true;
                Actor.compSprite.texture = Assets.EnemySheet;
                Actor.animList = AnimationFrames.MiniBoss_OctoMouth_Animations;
                Actor.health = 10;

                //walk and dash speeds are set in Functions_Ai
                //because they change based on this actor's health

                //this actor is a 2x2 miniboss
                Actor.compSprite.drawRec.Width = 16 * 2;
                Actor.compSprite.drawRec.Height = 16 * 2;

                Actor.compCollision.offsetY = -3;
                Actor.compSprite.zOffset = 2; //sort over water

                //set actor sound effects
                Actor.sfxDash = null; //silent dash
                Actor.sfx.hit = Assets.sfxBossHit;
                Actor.sfx.kill = Assets.sfxEnemyKill;

                ResetActorLoadout(Actor);
                //setup actor with useable item, so player can shoot fireballs
                Actor.item = MenuItemType.ItemFirerod;
                //custom hitbox
                Actor.compCollision.rec.Width = 16;
                Actor.compCollision.rec.Height = 12;
                Actor.compCollision.offsetX = -8;
                Actor.compCollision.offsetY = -4;
            }

            #endregion















            //Bosses

            #region BigEye

            else if (Type == ActorType.Boss_BigEye)
            {
                Actor.aiType = ActorAI.Boss_BigEye;
                Actor.compMove.grounded = false; //is flying

                Actor.enemy = true;
                Actor.compSprite.texture = Assets.EnemySheet;
                Actor.animList = AnimationFrames.Boss_BigEye_Animations;
                Actor.health = 30;

                //this boss creates seeker exploders based on it's hp
                ResetActorLoadout(Actor);
                Actor.weapon = MenuItemType.WeaponFang; //and can bite

                //walk and dash speeds are set in Functions_Ai
                //because they change based on this actor's health

                //this actor is a 2x3 boss 
                Actor.compSprite.drawRec.Width = 16 * 2;
                Actor.compSprite.drawRec.Height = 16 * 3;
                //actor is floating in air
                Actor.compSprite.zOffset = 16;
                //set actor sound effects
                Actor.sfxDash = null; //silent dash
                Actor.sfx.hit = Assets.sfxBossHit;
                Actor.sfx.kill = Assets.sfxBossHitDeath;

                //boss parameters
                Actor.attackRadius = 20;
                Actor.chaseRadius = 16 * 20;

                //custom hitbox
                Actor.compCollision.rec.Width = 24;
                Actor.compCollision.rec.Height = 16;
                Actor.compCollision.offsetX = -12;
                Actor.compCollision.offsetY = -7;
            }

            #endregion


            #region BigBat

            else if (Type == ActorType.Boss_BigBat)
            {
                Actor.aiType = ActorAI.Boss_BigBat;
                Actor.compMove.grounded = false; //is flying

                Actor.enemy = true;
                Actor.compSprite.texture = Assets.EnemySheet;
                Actor.animList = AnimationFrames.Boss_BigBat_Animations;
                Actor.health = 20;

                //this boss spam casts bat projectiles as main attack
                ResetActorLoadout(Actor);
                Actor.item = MenuItemType.Unknown;
                Actor.weapon = MenuItemType.WeaponFang; //and can bite

                //walk and dash speeds are set in Functions_Ai
                //because they change based on this actor's health

                //this actor is a 2x3 boss 
                Actor.compSprite.drawRec.Width = 16 * 2;
                Actor.compSprite.drawRec.Height = 16 * 3;

                //actor is floating in air
                Actor.compSprite.zOffset = 32;
                //set actor sound effects
                Actor.sfxDash = null; //silent dash
                Actor.sfx.hit = Assets.sfxBossHit;
                Actor.sfx.kill = Assets.sfxBossHitDeath;

                //boss parameters
                Actor.attackRadius = 20;
                Actor.chaseRadius = 16 * 20;

                //custom hitbox
                Actor.compCollision.rec.Width = 24;
                Actor.compCollision.rec.Height = 16;
                Actor.compCollision.offsetX = -12;
                Actor.compCollision.offsetY = -15;
            }

            #endregion


            #region OctoHead

            else if (Type == ActorType.Boss_OctoHead)
            {
                Actor.aiType = ActorAI.Boss_OctoHead;
                Actor.compMove.grounded = true;

                Actor.enemy = true;
                Actor.underwaterEnemy = true;
                Actor.compSprite.texture = Assets.EnemySheet;
                Actor.animList = AnimationFrames.Boss_OctoHead_Animations;
                Actor.health = 8; //each time head takes damage he spawns a tentacle actor
                ResetActorLoadout(Actor);

                //walk and dash speeds are set in Functions_Ai
                //because they change based on this actor's health

                //this actor is a 2x3 boss
                Actor.compSprite.drawRec.Width = 16 * 2;
                Actor.compSprite.drawRec.Height = 16 * 3;

                Actor.compCollision.offsetY = 8;
                Actor.compSprite.zOffset = 14;

                //set actor sound effects
                Actor.sfxDash = null; //silent dash
                Actor.sfx.hit = Assets.sfxBossHit;
                Actor.sfx.kill = Assets.sfxBossHitDeath;

                //setup collision rec (since actor doesn't spawn projectiles)
                Actor.compCollision.rec.Width = 16;
                Actor.compCollision.rec.Height = 16;
                Actor.compCollision.offsetX = -8;
                Actor.compCollision.offsetY = 2;
            }

            #endregion












            //Specials

            #region Tentacle

            else if (Type == ActorType.Special_Tentacle)
            {
                Actor.aiType = ActorAI.Special_Tentacle;
                Actor.compMove.grounded = true;

                Actor.enemy = true;
                Actor.underwaterEnemy = true;
                Actor.compSprite.texture = Assets.EnemySheet;
                Actor.animList = AnimationFrames.Special_Tentacle_Animations;
                Actor.health = 10; //not meant to be killed, but they can be

                Actor.walkSpeed = 0.15f;
                Actor.dashSpeed = 0.15f;

                //this actor is a 2x3 special
                Actor.compSprite.drawRec.Width = 16 * 2;
                Actor.compSprite.drawRec.Height = 16 * 3;
                Actor.compSprite.zOffset = 14;

                //set actor sound effects
                Actor.sfxDash = null; //silent dash
                Actor.sfx.hit = Assets.sfxEnemyHit;
                Actor.sfx.kill = Assets.sfxEnemyKill;

                //ai params
                Actor.chaseRadius = 16 * 30; //massive search space
                Actor.attackRadius = 18;

                ResetActorLoadout(Actor);
                //setup actor with useable item, so player can attack + dive
                Actor.item = MenuItemType.WeaponFang;

                Actor.compCollision.rec.Width = 14;
                Actor.compCollision.rec.Height = 10;
                Actor.compCollision.offsetX = -7;
                Actor.compCollision.offsetY = 6;
            }

            #endregion





            Functions_Component.CenterOrigin(Actor.compSprite);
        }

    }
}