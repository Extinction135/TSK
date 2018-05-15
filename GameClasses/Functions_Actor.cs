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
            SetCollisionRec(Actor);
            //reset actor's sprite 
            Actor.compSprite.zOffset = 0;
            Actor.compSprite.scale = 1.0f;
            //assume standard actor
            Actor.compSprite.cellSize.X = 16;
            Actor.compSprite.cellSize.Y = 16;
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
            Actor.state = ActorState.Dead;
            Actor.stateLocked = true;
            Actor.lockCounter = 0;
            Actor.lockTotal = 255;
            Assets.Play(Actor.sfx.kill); //play actor death sound fx

            if (Actor.type == ActorType.Blob || Actor.type == ActorType.Boss)
            { DungeonRecord.enemyCount++; } //track enemy deaths

            //make dead actor's corpse passable
            Actor.compCollision.blocking = false;

            //Enemy Specific Death Effects
            if (Actor.type == ActorType.Blob)
            {
                Actor.compSprite.zOffset = -16; //sort to floor
                Functions_Particle.Spawn(ObjType.Particle_Blast, Actor);
                Actor.compCollision.rec.X = -1000; //hide actor collisionRec
                Functions_Loot.SpawnLoot(Actor.compSprite.position);
            }
            else if (Actor.type == ActorType.Boss)
            {
                DungeonRecord.beatDungeon = true; //player has beat the dungeon
                Functions_Level.CloseLevel(ExitAction.Summary);
                Actor.compSprite.zOffset = -16; //sort to floor
                Actor.compCollision.rec.X = -1000; //hide actor collisionRec
                Assets.Play(Assets.sfxExplosionsMultiple); //play explosions
            }
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
        {   //set the collisionRec parameters based on the Type
            if (Actor.type == ActorType.Boss)
            {
                Actor.compCollision.rec.Width = 24;
                Actor.compCollision.rec.Height = 16;
                Actor.compCollision.offsetX = -12;
                Actor.compCollision.offsetY = -2;
            }
            else
            {
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
            if (Actor.state == ActorState.Idle)
            {
                if(Actor.swimming)
                { Actor.animGroup = Actor.animList.swim_idle; }
                else if (Actor.carrying)
                { Actor.animGroup = Actor.animList.idleCarry; }
                else if (Actor.grabbing)
                { Actor.animGroup = Actor.animList.grab; }
                else { Actor.animGroup = Actor.animList.idle; }
            }
            else if (Actor.state == ActorState.Move)
            {
                if (Actor.swimming)
                { Actor.animGroup = Actor.animList.swim_move; }
                else if (Actor.carrying)
                { Actor.animGroup = Actor.animList.moveCarry; }
                else if (Actor.grabbing)
                { Actor.animGroup = Actor.animList.push; }
                else { Actor.animGroup = Actor.animList.move; }
            }

            //actions
            else if (Actor.state == ActorState.Dash)
            {
                if (Actor.swimming)
                { Actor.animGroup = Actor.animList.swim_dash; }
                else { Actor.animGroup = Actor.animList.dash; }
            }
            else if (Actor.state == ActorState.Interact)
            {
                if (Actor.swimming)
                { }
                else { Actor.animGroup = Actor.animList.interact; }
            }
            else if (Actor.state == ActorState.Attack)
            {
                if (Actor.swimming)
                { }
                else { Actor.animGroup = Actor.animList.attack; }
            }
            else if (Actor.state == ActorState.Use)
            {
                if (Actor.swimming)
                { }
                else { Actor.animGroup = Actor.animList.attack; }
            }
            else if (Actor.state == ActorState.Pickup)
            {
                Actor.animGroup = Actor.animList.pickupThrow; 
            }
            else if (Actor.state == ActorState.Throw)
            {
                Actor.animGroup = Actor.animList.pickupThrow; 
            }

            //consequences
            else if (Actor.state == ActorState.Hit)
            {
                if (Actor.swimming)
                { Actor.animGroup = Actor.animList.swim_hit; }
                else { Actor.animGroup = Actor.animList.hit; }
            }
            else if (Actor.state == ActorState.Dead)
            {
                Actor.compAnim.loop = false; //stop looping

                if (Actor.swimming)
                {
                    //drown anim
                }
                else
                {
                    Actor.animGroup = Actor.animList.death;
                    //speed up hero's death
                    if (Actor.type == ActorType.Hero) { Actor.compAnim.speed = 6; }
                    else//skip spinning death animation if actor isn't link hero
                    { Actor.compAnim.index = (byte)(Actor.compAnim.currentAnimation.Count - 1); }
                }
            }
            else if (Actor.state == ActorState.Reward)
            {
                Actor.animGroup = Actor.animList.reward;
            }
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
            //'hide' roomObject far away from level/room
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


        static ObjType throwType;
        public static void Throw(Actor Act)
        {   //put actor into throw state
            Act.carrying = false;
            Act.state = ActorState.Throw;
            Act.stateLocked = true;
            Act.lockTotal = 10;
            SetAnimationGroup(Act);

            //for now, just destroy the obj
            //Functions_GameObject.HandleCommon(Act.heldObj, Act.direction);

            //create a thrown version of heldObj
            //assume we're throwing a bush
            throwType = ObjType.ProjectileBush;
            //if (Act.heldObj.type == ObjType.Wor_Bush)
            //{ throwType = ObjType.ProjectileBush; }

            //check for pot
            if (Act.heldObj.type == ObjType.Dungeon_Pot)
            { throwType = ObjType.ProjectilePotSkull; }
            else if (Act.heldObj.type == ObjType.Wor_Pot)
            { throwType = ObjType.ProjectilePot; }

            Functions_Projectile.Spawn(throwType, Act.compMove, Act.direction);
            Functions_Pool.Release(Act.heldObj);
            Act.heldObj = null; //has to be last line, we lose ref to Obj
        }


        public static void Grab(GameObject Obj, Actor Act)
        {
            Act.grabbing = true;
            Act.grabbedObj = Obj;
        }
        




        public static void SetType(Actor Actor, ActorType Type)
        {
            ResetActor(Actor);
            Actor.type = Type;


            #region Actor Specific Fields

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
            else if (Type == ActorType.Blob)
            {
                Actor.aiType = ActorAI.Basic;
                Actor.enemy = true;
                Actor.compSprite.texture = Assets.blobSheet;
                Actor.animList = AnimationFrames.Hero_Animations; //actor is hero
                Actor.health = 1;
                ResetActorLoadout(Actor);
                Actor.weapon = MenuItemType.WeaponSword;
                Actor.walkSpeed = 0.05f;
                Actor.dashSpeed = 0.30f;
                //set actor sound effects
                Actor.sfxDash = Assets.sfxBlobDash;
                Actor.sfx.hit = Assets.sfxEnemyHit;
                Actor.sfx.kill = Assets.sfxEnemyKill;
            }
            else if (Type == ActorType.Boss)
            {
                Actor.aiType = ActorAI.Random;
                Actor.enemy = true;
                Actor.compSprite.texture = Assets.bossSheet;
                Actor.animList = AnimationFrames.Boss_Blob_Animations;
                Actor.health = 10;
                ResetActorLoadout(Actor);
                Actor.walkSpeed = 0.50f;
                Actor.dashSpeed = 1.00f;

                //this actor is a boss (double size)
                Actor.compSprite.cellSize.X = 32;
                Actor.compSprite.cellSize.Y = 32;
                //the boss actor has a lower sorting point that normal actors
                Actor.compSprite.zOffset = 12;

                //set actor sound effects
                Actor.sfxDash = Assets.sfxBlobDash;
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




                if(Actor.swimming)
                {

                    Actor.compMove.speed = Actor.swimSpeed;

                    #region Swimming State

                    if (Actor.state == ActorState.Interact)
                    {

                        //the issue here is that hero could pickup / etc while swimming..
                        //do not want that

                        if (Actor == Pool.hero) //only hero can interact with room objs
                        { Functions_Hero.CheckInteractionRecCollisions(); }


                    }
                    else if (Actor.state == ActorState.Dash)
                    {
                        Actor.lockTotal = 15;
                        Actor.stateLocked = true;

                        if(Actor.compInput.direction == Direction.Down
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
                else if(Actor.carrying)
                {

                    #region Carrying state

                    //cant attack, dash, or use
                    //interact throws held obj
                    //for now, just destroy the obj in actor's hands

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
                {

                    #region Grabbing State

                    if(Actor == Pool.hero)
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
                {

                    #region Non-carrying state

                    //all actors other than hero are processed as follows
                    if (Actor.state == ActorState.Interact)
                    {
                        if (Actor == Pool.hero) //only hero can interact with room objs
                        { Functions_Hero.CheckInteractionRecCollisions(); }
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
                        Functions_Item.UseItem(Actor.weapon, Actor);
                        if (Actor == Pool.hero) //scale up worldUI weapon 
                        { WorldUI.currentWeapon.compSprite.scale = 2.0f; }
                    }
                    else if (Actor.state == ActorState.Use)
                    {
                        Functions_Item.UseItem(Actor.item, Actor);
                        if (Actor == Pool.hero) //scale up worldUI item 
                        { WorldUI.currentItem.compSprite.scale = 2.0f; }
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


                #region State Checks

                if (Actor.state == ActorState.Dead)
                {   //check death state
                    Actor.lockCounter = 0; //lock actor into dead state
                    Actor.health = 0; //lock actor's health at 0

                    //Death Effects
                    if (Actor == Pool.hero) { Functions_Hero.HandleDeath(); }
                    else if (Actor.type == ActorType.Boss)
                    {   //dead bosses perpetually explode
                        if (Functions_Random.Int(0, 100) > 75) //randomly create explosions
                        {   //randomly place explosions around boss
                            Functions_Particle.Spawn(
                                ObjType.Particle_Blast,
                                Actor.compSprite.position.X + Functions_Random.Int(-16, 16),
                                Actor.compSprite.position.Y + Functions_Random.Int(-16, 16));
                        }
                    }
                }
                else if (Actor.state == ActorState.Attack)
                {   //when an actor attacks, they slow down for the duration
                    Actor.compMove.friction = World.frictionAttack;
                }
                else if(Actor.state == ActorState.Use)
                {   //when an actor uses something, they slow down for the duration
                    Actor.compMove.friction = World.frictionUse;
                }
                else if(Actor.state == ActorState.Hit)
                {   //zero out the actor's speed
                    Actor.compMove.speed = 0.0f;
                    //this prevents actor from affecting the hit's push via directional input
                    //this also gives hits ALOT more push
                }

                #endregion

            }

            #endregion


            //set actor animation and direction
            SetAnimationGroup(Actor);
            SetAnimationDirection(Actor);
        }

    }
}