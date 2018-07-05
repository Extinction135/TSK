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
    public static class Functions_Actor
    {
        static ObjType throwType;
        static int i;


        public static void ResetActor(Actor Actor)
        {   //reset actor to default living state
            Actor.stateLocked = false;
            Actor.active = true;
            Actor.lockCounter = 0;
            Actor.lockTotal = 0;
            //reset actor's state and direction
            Actor.state = ActorState.Idle;
            Actor.direction = Direction.Down;
            Actor.compMove.direction = Direction.None;
            Actor.compMove.grounded = true; //most actors move on ground
            //reset actor's collisions
            Actor.compCollision.blocking = true;
            //reset actor's sprite 
            Actor.compSprite.zOffset = 0;
            Actor.compSprite.scale = 1.0f;
            //assume standard actor
            Actor.compSprite.cellSize.X = 16;
            Actor.compSprite.cellSize.Y = 16;
            //setup hitbox for standard 16x16 actor
            Actor.compCollision.rec.Width = 12;
            Actor.compCollision.rec.Height = 8;
            Actor.compCollision.offsetX = -6;
            Actor.compCollision.offsetY = 0;
            //assume standard search/attack radius
            Actor.chaseRadius = 16 * 5;
            Actor.attackRadius = 14;
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

        public static void SetHitState(Actor Actor)
        {   //bail if actor is already dead (dont hit dead actors)
            if (Actor.state == ActorState.Dead) { return; }
            //lock actor into hit state, play actor hit soundfx
            Actor.state = ActorState.Hit;
            Actor.stateLocked = true;
            Actor.lockCounter = 0;
            Actor.lockTotal = 15;
            Assets.Play(Actor.sfx.hit);

            //throw any held object
            if (Actor.carrying) { Throw(Actor); }

            if (Actor == Pool.hero)
            {   //hero should drop a gold piece
                if (!Flags.InfiniteGold) 
                {
                    if (PlayerData.current.gold > 0) //if hero has any gold
                    {   //drop a gold piece upon getting hit
                        Functions_Pickup.Spawn(ObjType.Pickup_Rupee, Actor);
                        PlayerData.current.gold--;
                    }
                }
            }
        }

        public static void SetDeathState(Actor Actor)
        {
            //prior to hero dying, he must return to being link/hero
            if (Actor == Pool.hero) { Transform(Pool.hero, ActorType.Hero); }
            //link is only actor with correct death animation for gameover
            
            //and switching hero back also makes it easy to track enemy deaths
            else { DungeonRecord.enemyCount++; }

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
            

            #region Bosses

            else if (Actor.type == ActorType.Boss_BigEye
                || Actor.type == ActorType.Boss_BigBat)
            {   //decorate this death as special / explosive
                Functions_Particle.Spawn_Explosion(ObjType.Particle_Debris,
                    Actor.compSprite.position.X, Actor.compSprite.position.Y, true);
                
                //End Dungeon
                if (Level.currentRoom.roomID == RoomID.Boss)
                {   //boss must die in boss room to end dungeon
                    DungeonRecord.beatDungeon = true; //player beat dungeon
                    Functions_Level.CloseLevel(ExitAction.Summary);

                    //flip dungeon booleans
                    if (Actor.type == ActorType.Boss_BigEye)
                    { PlayerData.current.story_forestDungeon = true; }
                    else if (Actor.type == ActorType.Boss_BigBat)
                    { PlayerData.current.story_mountainDungeon = true; }
                }
            }

            #endregion


            #region Minibosses

            else if(Actor.type == ActorType.MiniBoss_BlackEye)
            {   //decorate this death as special / explosive
                Functions_Particle.Spawn_Explosion(ObjType.Particle_Debris,
                    Actor.compSprite.position.X, Actor.compSprite.position.Y, true);
                Actor.compAnim.speed = 15; //slow down death animation to taste
                //this actor becomes debris on floor, sort to floor.
                Actor.compSprite.zOffset = -8;
                Functions_Component.SetZdepth(Actor.compSprite);
                //try to drop map
                Functions_GameObject_Dungeon.DropMap(
                    Actor.compSprite.position.X, 
                    Actor.compSprite.position.Y);
            }

            else if (Actor.type == ActorType.MiniBoss_Spider_Armored)
            {   
                //decorate this death as special / explosive
                Functions_Particle.Spawn_Explosion(ObjType.Particle_Debris,
                    Actor.compSprite.position.X, Actor.compSprite.position.Y, true);
                //armored spider becomes unarmored version
                SetType(Actor, ActorType.MiniBoss_Spider_Unarmored);
                //audibly alert player battle isn't over
                Assets.Play(Assets.sfxBossHit);
            }

            else if (Actor.type == ActorType.MiniBoss_Spider_Unarmored)
            {   
                //decorate this death as special / explosive
                Functions_Particle.Spawn_Explosion(ObjType.Particle_Debris,
                    Actor.compSprite.position.X, Actor.compSprite.position.Y, true);
                Actor.compAnim.speed = 10; //slow down death animation to taste
                //this actor becomes debris on floor, sort to floor.
                Actor.compSprite.zOffset = -8;
                Functions_Component.SetZdepth(Actor.compSprite);
                //try to drop map
                Functions_GameObject_Dungeon.DropMap(
                    Actor.compSprite.position.X,
                    Actor.compSprite.position.Y);
            }

            #endregion


            else
            {   //this is all other actor types: blob, angry eye, etc..
                Functions_Particle.Spawn(ObjType.Particle_Blast, Actor);
                Functions_Loot.SpawnLoot(Actor.compSprite.position); //loot!
                Functions_Pool.Release(Actor);
            }



            //this is all actors except hero, who bails from method early
            Actor.compCollision.blocking = false; //make dead actor's corpse passable
        }

        public static void SetRewardState(Actor Actor)
        {
            Actor.state = ActorState.Reward;
            Actor.stateLocked = true;
            Actor.lockCounter = 0;
            Actor.lockTotal = 30; //med pause
            Functions_Movement.StopMovement(Actor.compMove);
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

        public static void SetCollisionRec(Actor Actor)
        {

            //set the collisionRec parameters based on the Type
            /*
            if (Actor.type == ActorType.Boss_BigEye)
            {
                Actor.compCollision.rec.Width = 24;
                Actor.compCollision.rec.Height = 16;
                Actor.compCollision.offsetX = -12;
                Actor.compCollision.offsetY = 0;
            }

            
            //this doesn't work, because pro.spawn() doesn't
            //take into account the actor's hitBox when creating a fireball
            //so this will cause self-interaction upon spawn, leading to death.
            else if(Actor.type == ActorType.MiniBoss_BlackEye)
            {
                Actor.compCollision.rec.Width = 16;
                Actor.compCollision.rec.Height = 16;
                Actor.compCollision.offsetX = -8;
                Actor.compCollision.offsetY = 0;
            }
            */

            
            {   //hero/blob/etc actors have same hitBox (for 16x16 sprite)
                Actor.compCollision.rec.Width = 12;
                Actor.compCollision.rec.Height = 8;
                Actor.compCollision.offsetX = -6;
                Actor.compCollision.offsetY = 0;
            }
        }

        public static void ResetActorLoadout(Actor Actor)
        {
            Actor.weapon = MenuItemType.Unknown;
            Actor.item = MenuItemType.Unknown;
            Actor.armor = MenuItemType.Unknown;
            Actor.equipment = MenuItemType.Unknown;
        }

        public static void SetAnimationGroup(Actor Actor)
        {
            //assume default animation speed and looping
            Actor.compAnim.speed = 10;
            Actor.compAnim.loop = true;

            //movement

            #region Idle

            if (Actor.state == ActorState.Idle)
            {
                if(Actor.swimming)
                {
                    if (Actor.underwater)
                    { Actor.animGroup = Actor.animList.underwater_idle; }
                    else { Actor.animGroup = Actor.animList.swim_idle; }
                }
                else if (Actor.carrying)
                { Actor.animGroup = Actor.animList.idleCarry; }
                else if (Actor.grabbing)
                { Actor.animGroup = Actor.animList.grab; }
                else { Actor.animGroup = Actor.animList.idle; }
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
                else if (Actor.carrying)
                { Actor.animGroup = Actor.animList.moveCarry; }
                else if (Actor.grabbing)
                { Actor.animGroup = Actor.animList.push; }
                else { Actor.animGroup = Actor.animList.move; }
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
                { }
                else if (Actor.carrying)
                { }
                else { Actor.animGroup = Actor.animList.attack; }
            }

            #endregion


            #region Use

            else if (Actor.state == ActorState.Use)
            {
                if (Actor.swimming)
                { }
                else if (Actor.carrying)
                { }
                else { Actor.animGroup = Actor.animList.attack; }
            }

            #endregion


            #region Pickup and Throw

            else if (Actor.state == ActorState.Pickup)
            {
                Actor.animGroup = Actor.animList.pickupThrow; 
            }
            else if (Actor.state == ActorState.Throw)
            {
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
                if(Actor == Pool.hero)
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
                if(Actor.swimming)
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
                if (Actor.breathCounter > 60 * 4) //4 seconds
                {
                    Actor.underwater = false;
                    Functions_Particle.Spawn(ObjType.Particle_Splash, Actor);
                    Actor.breathCounter = 0;
                }
            }
        }

        public static void Pickup(GameObject Obj, Actor Act)
        {
            //decorate pickup
            Functions_Particle.Spawn(
                ObjType.Particle_Attention,
                Obj.compSprite.position.X,
                Obj.compSprite.position.Y);

            //handle pickup effects
            if (Obj.type == ObjType.Wor_Bush)
            {   //spawn a stump obj at bush location
                Functions_GameObject.Spawn(
                    ObjType.Wor_Bush_Stump,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y,
                    Direction.Down);
            }

            //handle picking up obj
            Act.carrying = true;
            Act.heldObj = Obj;
            //'hide' roomObject offscreen - temporary, necessary
            Functions_Movement.Teleport(Obj.compMove, 4096, 4096);
            //put actor into pickup state
            Act.state = ActorState.Pickup;
            Act.stateLocked = true;
            Act.lockTotal = 10;
            SetAnimationGroup(Act);
            Act.heldObj.compSprite.zOffset = +16; //sort above actor
            Act.heldObj.compCollision.blocking = false; //prevent act/obj overlaps
            Assets.Play(Assets.sfxActorLand); //temp sfx
        }

        public static void Throw(Actor Act)
        {   //put actor into throw state
            Act.carrying = false;
            Act.state = ActorState.Throw;
            Act.stateLocked = true;
            Act.lockTotal = 10;
            SetAnimationGroup(Act);


            //check if we're throwing an enemy or object
            if(Act.heldObj.type == ObjType.Wor_Enemy_Turtle
                || Act.heldObj.type == ObjType.Wor_Enemy_Crab)
            {
                //resolve act.direction to cardinal
                Act.direction = Functions_Direction.GetCardinalDirection(Act.direction);
                //place enemy outside of hero's hitbox
                if (Act.direction == Direction.Left)
                {
                    Functions_Movement.Teleport(Act.heldObj.compMove,
                        Act.compSprite.position.X - 8,
                        Act.compSprite.position.Y);
                }
                else if (Act.direction == Direction.Up)
                {
                    Functions_Movement.Teleport(Act.heldObj.compMove,
                        Act.compSprite.position.X,
                        Act.compSprite.position.Y - 8);
                }
                else if (Act.direction == Direction.Right)
                {
                    Functions_Movement.Teleport(Act.heldObj.compMove,
                        Act.compSprite.position.X + 8,
                        Act.compSprite.position.Y);
                }
                else if (Act.direction == Direction.Down)
                {
                    Functions_Movement.Teleport(Act.heldObj.compMove,
                        Act.compSprite.position.X,
                        Act.compSprite.position.Y + 8);
                }
                //strongly push enemy in actor's facing direction
                Functions_Movement.Push(
                    Act.heldObj.compMove,
                    Act.direction, 10.0f);
                //reset hitbox, sorting offsets, etc...
                Functions_GameObject.SetType(Act.heldObj, Act.heldObj.type);
            }
            else
            {   //create a thrown version of heldObj
                //assume we're throwing a bush
                throwType = ObjType.ProjectileBush;
                //check for pots
                if (Act.heldObj.type == ObjType.Dungeon_Pot)
                { throwType = ObjType.ProjectilePotSkull; }
                else if (Act.heldObj.type == ObjType.Wor_Pot)
                { throwType = ObjType.ProjectilePot; }
                //spawn the thrown projectile obj
                Functions_Projectile.Spawn(throwType, Act.compMove, Act.direction);
                Functions_Pool.Release(Act.heldObj);
            }
            
            Act.heldObj = null; //release reference to roomObj
            Assets.Play(Assets.sfxActorFall); //play throw sfx
            Functions_Particle.SpawnPushFX(Act.compMove, Act.direction);
        }

        public static void Grab(GameObject Obj, Actor Act)
        {
            Act.grabbing = true;
            Act.grabbedObj = Obj;
        }

        public static void Transform(Actor Actor, ActorType Type)
        {   //transforms one actor into another
            SetType(Actor, Type);

            if (Actor == Pool.hero)
            {   //store actor type
                PlayerData.current.actorType = Actor.type;
            }

            Functions_Particle.Spawn(ObjType.Particle_Attention, Actor);
        }



        public static void SetType(Actor Actor, ActorType Type)
        {
            ResetActor(Actor);
            Actor.type = Type;
            SetCollisionRec(Actor);


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
                Actor.health = 3;
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
                Actor.compSprite.texture = Assets.forestLevelSheet;
                Actor.animList = AnimationFrames.Standard_AngryEye_Animations;
                Actor.health = 3;
                ResetActorLoadout(Actor);
                Actor.weapon = MenuItemType.WeaponFang;
                Actor.walkSpeed = 0.05f;
                Actor.dashSpeed = 0.30f;

                //this actor is a 2x1 enemy
                Actor.compSprite.cellSize.X = 16 * 1;
                Actor.compSprite.cellSize.Y = 16 * 2;
                Actor.compCollision.offsetY = -4;

                //actor is floating in air
                Actor.compSprite.zOffset = 16;

                //set actor sound effects
                Actor.sfxDash = null; //silent dash
                Actor.sfx.hit = Assets.sfxEnemyHit;
                Actor.sfx.kill = Assets.sfxEnemyKill;
            }

            #endregion


            #region Standard - BeefyBat

            else if (Type == ActorType.Standard_BeefyBat)
            {
                Actor.aiType = ActorAI.Basic;
                Actor.compMove.grounded = false; //is flying

                Actor.enemy = true;
                Actor.compSprite.texture = Assets.mountainLevelSheet;
                Actor.animList = AnimationFrames.Standard_BeefyBat_Animations;
                Actor.health = 2;
                ResetActorLoadout(Actor);
                Actor.weapon = MenuItemType.WeaponFang;
                Actor.walkSpeed = 0.05f;
                Actor.dashSpeed = 0.30f;

                //bias hitbox north
                Actor.compCollision.offsetY = -2; 
                //actor is flying, sort over
                Actor.compSprite.zOffset = 16;

                //set actor sound effects
                Actor.sfxDash = null; //silent dash
                Actor.sfx.hit = Assets.sfxEnemyHit;
                Actor.sfx.kill = Assets.sfxEnemyKill;
            }

            #endregion








            //Bosses

            #region BigEye

            else if (Type == ActorType.Boss_BigEye)
            {
                Actor.aiType = ActorAI.Boss_BigEye;
                Actor.compMove.grounded = false; //is flying

                Actor.enemy = true;
                Actor.compSprite.texture = Assets.forestLevelSheet;
                Actor.animList = AnimationFrames.Boss_BigEye_Animations;
                Actor.health = 30;

                //this boss creates seeker exploders based on it's hp
                ResetActorLoadout(Actor);
                Actor.weapon = MenuItemType.WeaponFang; //and can bite

                //walk and dash speeds are set in Functions_Ai
                //because they change based on this actor's health

                //this actor is a 2x3 boss 
                Actor.compSprite.cellSize.X = 16 * 2;
                Actor.compSprite.cellSize.Y = 16 * 3;
                //actor is floating in air
                Actor.compSprite.zOffset = 16;
                //set actor sound effects
                Actor.sfxDash = null; //silent dash
                Actor.sfx.hit = Assets.sfxBossHit;
                Actor.sfx.kill = Assets.sfxBossHitDeath;

                //boss parameters
                Actor.attackRadius = 20;
                Actor.chaseRadius = 16 * 20;
            }

            #endregion


            #region BigBat

            else if (Type == ActorType.Boss_BigBat)
            {
                Actor.aiType = ActorAI.Boss_BigBat;
                Actor.compMove.grounded = false; //is flying

                Actor.enemy = true;
                Actor.compSprite.texture = Assets.mountainLevelSheet;
                Actor.animList = AnimationFrames.Boss_BigBat_Animations;
                Actor.health = 20;

                //this boss spam casts bat projectiles as main attack
                ResetActorLoadout(Actor);
                Actor.item = MenuItemType.MagicBat;
                Actor.weapon = MenuItemType.WeaponFang; //and can bite

                //walk and dash speeds are set in Functions_Ai
                //because they change based on this actor's health

                //this actor is a 2x3 boss 
                Actor.compSprite.cellSize.X = 16 * 2;
                Actor.compSprite.cellSize.Y = 16 * 3;

                //actor is floating in air
                Actor.compSprite.zOffset = 32;
                //set actor sound effects
                Actor.sfxDash = null; //silent dash
                Actor.sfx.hit = Assets.sfxBossHit;
                Actor.sfx.kill = Assets.sfxBossHitDeath;

                //boss parameters
                Actor.attackRadius = 20;
                Actor.chaseRadius = 16 * 20;
            }

            #endregion







            //Minibosses

            #region Blackeye

            else if (Type == ActorType.MiniBoss_BlackEye)
            {
                Actor.aiType = ActorAI.Miniboss_Blackeye;
                Actor.compMove.grounded = false; //is flying

                Actor.enemy = true;
                Actor.compSprite.texture = Assets.forestLevelSheet;
                Actor.animList = AnimationFrames.MiniBoss_BlackEye_Animations;
                Actor.health = 15;
                ResetActorLoadout(Actor);

                //walk and dash speeds are set in Functions_Ai
                //because they change based on this actor's health

                //this actor is a 2x2 miniboss
                Actor.compSprite.cellSize.X = 16 * 2;
                Actor.compSprite.cellSize.Y = 16 * 2;

                Actor.compCollision.offsetY = -4;
                //actor is floating in air
                Actor.compSprite.zOffset = 16;
                //set actor sound effects
                Actor.sfxDash = null; //silent dash
                Actor.sfx.hit = Assets.sfxBossHit;
                Actor.sfx.kill = Assets.sfxShatter; //sounds good
            }

            #endregion


            #region Spider - Armored

            else if (Type == ActorType.MiniBoss_Spider_Armored)
            {
                Actor.aiType = ActorAI.Basic;

                Actor.enemy = true;
                Actor.compSprite.texture = Assets.mountainLevelSheet;
                Actor.animList = AnimationFrames.MiniBoss_SpiderArmored_Animations;
                Actor.health = 1; //simply takes 1 damage, then becomes unarmored

                ResetActorLoadout(Actor);
                Actor.weapon = MenuItemType.WeaponFang;

                //moves slowly
                Actor.walkSpeed = 0.05f;
                Actor.dashSpeed = 0.30f;

                //this actor is a 3x1 miniboss
                Actor.compSprite.cellSize.X = 16 * 3;
                Actor.compSprite.cellSize.Y = 16 * 1;
                Actor.compCollision.offsetY = 0;
                Actor.compSprite.zOffset = 0;

                //set actor sound effects
                Actor.sfxDash = Assets.sfxEnemyTaunt;
                Actor.sfx.hit = Assets.sfxTapMetallic;
                Actor.sfx.kill = Assets.sfxShatter;
            }

            #endregion


            #region Spider - UN-armored

            else if (Type == ActorType.MiniBoss_Spider_Unarmored)
            {
                Actor.aiType = ActorAI.Basic;
                Actor.compCollision.blocking = true; //actor was 'dead'

                Actor.enemy = true;
                Actor.compSprite.texture = Assets.mountainLevelSheet;
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
                Actor.compSprite.cellSize.X = 16 * 3;
                Actor.compSprite.cellSize.Y = 16 * 1;
                Actor.compCollision.offsetY = 0;
                Actor.compSprite.zOffset = 16;

                //set actor sound effects
                Actor.sfxDash = Assets.sfxEnemyTaunt; 
                Actor.sfx.hit = Assets.sfxBossHit;
                Actor.sfx.kill = Assets.sfxBossHitDeath; 
            }


            #endregion






            SetAnimationGroup(Actor);
            SetAnimationDirection(Actor);
            Functions_Component.UpdateCellSize(Actor.compSprite);
            Functions_Component.CenterOrigin(Actor.compSprite);
        }

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
                

                if (Actor.swimming)
                {   //SWIM STATES
                    Actor.compMove.speed = Actor.swimSpeed;
                    if (Actor.state == ActorState.Interact)
                    {   //swim interact
                        if (Actor == Pool.hero) //only hero can interact with room objs
                        { Functions_Hero.CheckInteractionRec(); }
                    }
                    else if (Actor.state == ActorState.Dash)
                    {

                        #region  Swim dash (but can't dash underwater)

                        if (Actor.underwater == false)
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
                                //place water kick fx behind actor
                                Functions_Particle.Spawn(
                                    ObjType.Particle_WaterKick,
                                    Actor.compSprite.position.X,
                                    Actor.compSprite.position.Y,
                                    Functions_Direction.GetOppositeDirection(Actor.direction));
                            }
                            else
                            {   //diagonal swim dash - half power push
                                Functions_Movement.Push(
                                    Actor.compMove,
                                    Actor.compInput.direction, 1.0f);
                                Assets.Play(Assets.sfxWaterSwim);
                            }
                        }
                        else
                        {   //return actor to normal move state, they can't dash underwater
                            Actor.state = ActorState.Move; //nah breh
                        }

                        #endregion

                    }
                    else if (Actor.state == ActorState.Attack)
                    {

                        #region Swim Dive (toggled, only for Hero)

                        if(Actor == Pool.hero)
                        {   //only hero has sweet diving action rn
                            if(Functions_Input.IsNewButtonPress(Buttons.X))
                            {   //toggle dive state only on new button press
                                if (Actor.underwater == false) //setup new dive
                                { Actor.underwater = true; Actor.breathCounter = 0; }
                                else { Actor.underwater = false; }
                                //make a splash
                                Functions_Particle.Spawn(
                                    ObjType.Particle_Splash,
                                    Actor.compSprite.position.X,
                                    Actor.compSprite.position.Y);
                            }
                        }

                        #endregion

                    }
                    else if (Actor.state == ActorState.Use)
                    {
                        //nothing
                        //but in future we can use magic items, etc.. in water
                    }
                }
                else if(Actor.carrying)
                {   //this is on land

                    #region Carrying state

                    //cant attack or use
                    //interact throws held obj

                    if (Actor.state == ActorState.Interact)
                    {
                        if(Actor.heldObj != null)
                        {
                            Functions_Movement.StopMovement(Actor.compMove);
                            Throw(Actor);
                        }
                    }
                    else if (Actor.state == ActorState.Dash)
                    {
                        //nothing
                        Actor.lockTotal = 10;
                        Actor.stateLocked = true;
                        Actor.compMove.speed = Actor.dashSpeed;
                        Functions_Particle.Spawn(ObjType.Particle_RisingSmoke, Actor);
                        Assets.Play(Actor.sfxDash);
                    }
                    else if (Actor.state == ActorState.Attack)
                    {
                        //nothing
                    }
                    else if (Actor.state == ActorState.Use)
                    {
                        //nothing
                    }

                    #endregion

                }
                else if(Actor.grabbing)
                {   //this is on land

                    #region Grabbing State

                    if (Actor == Pool.hero)
                    {   //make sure A button is down and hero is in grab state
                        if (Functions_Input.IsButtonDown(Buttons.A))
                        {   //alter hero's friction - slow
                            Actor.compMove.friction = World.frictionUse;
                            //push obj hero has grabbed
                            Functions_Hero.PushGrabbedObj();
                        }
                        else
                        {   //release the grabbed object
                            Actor.grabbing = false;
                            Actor.grabbedObj = null;
                        }
                    }

                    #endregion

                }
                else
                {   //this is on land

                    #region Non-carrying state

                    //all actors other than hero are processed as follows
                    if (Actor.state == ActorState.Interact)
                    {
                        if (Actor == Pool.hero) //only hero can interact with room objs
                        { Functions_Hero.CheckInteractionRec(); }
                    }
                    else if (Actor.state == ActorState.Dash)
                    {
                        Actor.lockTotal = 10;
                        Actor.stateLocked = true;
                        Actor.compMove.speed = Actor.dashSpeed;
                        Functions_Particle.Spawn(ObjType.Particle_RisingSmoke, Actor);
                        Assets.Play(Actor.sfxDash);
                    }
                    else if (Actor.state == ActorState.Attack)
                    {
                        if (Actor == Pool.hero)
                        {   //scale up worldUI weapon 
                            WorldUI.currentWeapon.compSprite.scale = 2.0f;
                            //hero has special skills based on equipped weapons
                            if (Pool.hero.weapon == MenuItemType.WeaponShovel) { Functions_Dig.Dig(); }
                        }
                        Functions_Item.UseItem(Actor.weapon, Actor);
                    }
                    else if (Actor.state == ActorState.Use)
                    {
                        if(Actor == Pool.hero)
                        {   //scale up the worldUI item
                            WorldUI.currentItem.compSprite.scale = 2.0f;
                            //hero has special skills based on equipped items
                            //if (Pool.hero.weapon == MenuItemType.WeaponShovel) { Functions_Hero.Dig(); }
                        }
                        Functions_Item.UseItem(Actor.item, Actor);
                    }

                    #endregion

                }
            }

            #endregion


            #region Actor is Statelocked

            else
            {
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
                        if (Functions_Input.IsNewButtonPress(Buttons.A))
                        {

                            #region Resolve input direction to left or right

                            if (Input.gamePadDirection == Direction.Up 
                                || Input.gamePadDirection == Direction.Down)
                            { Input.gamePadDirection = Direction.None; }

                            else if (Input.gamePadDirection == Direction.UpLeft
                                || Input.gamePadDirection == Direction.DownLeft)
                            { Input.gamePadDirection = Direction.Left; }

                            else if (Input.gamePadDirection == Direction.UpRight
                                || Input.gamePadDirection == Direction.DownRight)
                            { Input.gamePadDirection = Direction.Right; }

                            #endregion



                            //set hero.direction based on controller input *right now*
                            Pool.hero.direction = Input.gamePadDirection;
                            //this direction will be used in check() below
                            //to set where interaction point is placed

                            //however, we dont want any up direction
                            

                            //check to see if hero can grab a foothold/ladder
                            Functions_Hero.CheckInteractionRec();

                            //if hero grabbed, he is now in climbing state
                            if (Actor.state != ActorState.Climbing)
                            {   //hero did not grab a foothold/ladder, continues fall south
                                //reset actor direction down
                                Actor.direction = Direction.Down;
                            }
                            else
                            {
                                //hero successfully grabbed, from a fall
                                //that's preeetty crispy, so we should celebrate a little

                                //place an attention particle above the hero's head as recognition
                                Functions_Particle.Spawn(
                                    ObjType.Particle_Attention,
                                    Actor.compSprite.position.X, 
                                    Actor.compSprite.position.Y - 4, 
                                    Direction.Down);
                                //play a special soundfx
                                Assets.Play(Assets.sfxActorLand);
                                //track successful wall jumps
                                PlayerData.current.recorded_wallJumps++;
                                //check wall jump achievements
                                Functions_Hero.CheckAchievements(Achievements.WallJumps);
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
                        Functions_Particle.Spawn(ObjType.Particle_Attention, Actor);
                        Assets.Play(Assets.sfxActorLand);
                        Functions_Movement.StopMovement(Actor.compMove);
                    }
                }
                else if (Actor.state == ActorState.Climbing)
                {
                    //allow directional input
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

                    Actor.compMove.speed = Actor.swimSpeed; //move slowly


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
                    
                    for (i = 0; i < Pool.roomObjCount; i++)
                    {   //find an active foothold obj
                        if (Pool.roomObjPool[i].active)
                        {
                            //prove actor is nearby foothold to maintain grip
                            if (Pool.roomObjPool[i].type == ObjType.Wor_MountainWall_Foothold ||
                                Pool.roomObjPool[i].type == ObjType.Wor_MountainWall_Ladder)
                            {
                                //if foothold does not contain center point of actor, actor loses grip and falls
                                if (Pool.roomObjPool[i].compCollision.rec.Contains(Actor.compSprite.position))
                                {   //Actor.state = ActorState.Climbing;
                                    Actor.lockTotal = 1;
                                }
                            }
                            
                            //actors touching top wall return to idle from climbing
                            else if(Pool.roomObjPool[i].type == ObjType.Wor_MountainWall_Top)
                            {
                                if (Pool.roomObjPool[i].compCollision.rec.Contains(Actor.compSprite.position))
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


                    //allow player to drop from climb using B button
                    if (Actor == Pool.hero)
                    {
                        if (Functions_Input.IsNewButtonPress(Buttons.B))
                        {
                            Actor.state = ActorState.Falling;
                            //push actor north to fake a jump
                            Functions_Movement.Push(Pool.hero.compMove, Direction.Up, 10.0f);
                        }
                    }
                    
                    //we manually set the animation frames, bail from rest of method
                    return;
                }
                
            }

            #endregion


            //set actor animation and direction
            SetAnimationGroup(Actor);
            SetAnimationDirection(Actor);
        }

    }
}