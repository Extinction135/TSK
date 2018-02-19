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
            Actor.compCollision.active = true;
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

        public static void BottleActor(Actor Actor)
        {   //can we bottle this actor?
            if (Actor.type == ActorType.Boss || Actor.type == ActorType.Hero)
            {   //pop cant bottle dialog
                if (Flags.ShowDialogs)
                { ScreenManager.AddScreen(new ScreenDialog(Functions_Dialog.BottleCant)); }
                return;
            }
            else
            {   //determine what type of actor we're attempting to bottle
                byte value = 5; //defaults to fairy value
                if (Actor.type == ActorType.Fairy) { value = 5; }
                else if (Actor.type == ActorType.Blob) { value = 6; }
                //determine if hero has an empty bottle to put this actor into
                Boolean captured = false;
                if (Functions_Bottle.FillEmptyBottle(value)) { captured = true; }
                //if hero put actor into empty bottle..
                if (captured)
                {   //alert player that hero successfully bottled the actor
                    if (Flags.ShowDialogs)
                    { ScreenManager.AddScreen(new ScreenDialog(Functions_Dialog.BottleSuccess)); }
                    SetDeathState(Actor); //kill bottled actor
                }
                else
                {   //alert player that hero has no empty bottles (all bottles are full)
                    if (Flags.ShowDialogs)
                    { ScreenManager.AddScreen(new ScreenDialog(Functions_Dialog.BottleFull)); }
                }
            }
        }



        public static void SetHitState(Actor Actor)
        {   //bail if actor is already dead (dont hit dead actors)
            if (Actor.state == ActorState.Dead) { return; }
            //lock actor into hit state, play actor hit soundfx
            Actor.state = ActorState.Hit;
            Actor.stateLocked = true;
            Actor.lockCounter = 0;
            Actor.lockTotal = 15;
            Assets.Play(Actor.sfxHit);

            if (Actor == Pool.hero)
            {   //hero should drop a gold piece
                if (!Flags.InfiniteGold) 
                {
                    if (PlayerData.current.gold > 0) //if hero has any gold
                    {   //drop a gold piece upon getting hit
                        Functions_Entity.SpawnEntity(ObjType.PickupRupee, Actor);
                        PlayerData.current.gold--;
                    }
                }
                //if hero is carrying, throw pot obj
                if(Functions_Hero.carrying)
                {
                    Functions_Hero.ThrowPot();
                }
            }
        }

        public static void SetDeathState(Actor Actor)
        {
            Actor.state = ActorState.Dead;
            Actor.stateLocked = true;
            Actor.lockCounter = 0;
            Actor.lockTotal = 255;
            Assets.Play(Actor.sfxDeath); //play actor death sound fx
            if (Actor.type == ActorType.Blob || Actor.type == ActorType.Boss)
            { DungeonRecord.enemyCount++; } //track enemy deaths

            //make dead actor's corpse passable
            Actor.compCollision.blocking = false;

            //Enemy Specific Death Effects
            if (Actor.type == ActorType.Blob)
            {
                Actor.compSprite.zOffset = -16; //sort to floor
                Functions_Entity.SpawnEntity(ObjType.ParticleExplosion, Actor);
                Actor.compCollision.rec.X = -1000; //hide actor collisionRec
                Functions_Loot.SpawnLoot(Actor.compSprite.position);
            }
            else if (Actor.type == ActorType.Boss)
            {
                PlayerData.current.crystal1 = true; //flip crystal1
                DungeonRecord.beatDungeon = true; //player has beat the dungeon
                Functions_Level.CloseLevel(ExitAction.Summary);
                Actor.compSprite.zOffset = -16; //sort to floor
                Actor.compCollision.rec.X = -1000; //hide actor collisionRec
                Assets.Play(Assets.sfxExplosionsMultiple); //play explosions
            }
            else if(Actor.type == ActorType.Fairy)
            {
                Functions_Entity.SpawnEntity(ObjType.ParticleExplosion, Actor);
                Functions_Pool.Release(Actor);
            }
        }

        public static void SetRewardState(Actor Actor)
        {   //reward state for hero is set using this method
            Actor.state = ActorState.Reward;
            Actor.stateLocked = true;
            Actor.lockCounter = 0;
            Actor.lockTotal = 40;
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



        public static void SetType(Actor Actor, ActorType Type)
        {
            ResetActor(Actor);
            Actor.type = Type;


            #region Actor Specific Fields

            if (Type == ActorType.Hero)
            {
                Actor.enemy = false;
                Actor.compSprite.texture = Assets.heroSheet;
                //do not update/change the hero's weapon/item/armor/equipment
                Actor.walkSpeed = 0.35f;
                Actor.dashSpeed = 0.90f;
                //set actor sound effects
                Actor.sfxDash = Assets.sfxHeroDash;
                Actor.sfxHit = Assets.sfxHeroHit;
                Actor.sfxDeath = Assets.sfxHeroKill;
            }
            else if (Type == ActorType.Blob)
            {
                Actor.aiType = ActorAI.Basic;
                Actor.enemy = true;
                Actor.compSprite.texture = Assets.blobSheet;
                Actor.health = 1;
                ResetActorLoadout(Actor);
                Actor.weapon = MenuItemType.WeaponSword;
                Actor.walkSpeed = 0.05f;
                Actor.dashSpeed = 0.30f;
                //set actor sound effects
                Actor.sfxDash = Assets.sfxBlobDash;
                Actor.sfxHit = Assets.sfxEnemyHit;
                Actor.sfxDeath = Assets.sfxEnemyKill;
            }
            else if (Type == ActorType.Boss)
            {
                Actor.aiType = ActorAI.Random;
                Actor.enemy = true;
                Actor.compSprite.texture = Assets.bossSheet;
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
                Actor.sfxHit = Assets.sfxBossHit;
                Actor.sfxDeath = Assets.sfxBossHitDeath;
            }
            else if (Type == ActorType.Fairy)
            {   //non-combatant actor
                Actor.aiType = ActorAI.Random;
                Actor.enemy = false;
                Actor.compSprite.texture = Assets.mainSheet;
                Actor.health = 1;
                ResetActorLoadout(Actor);
                Actor.walkSpeed = 0.25f;
                Actor.dashSpeed = 0.5f;
                //set actor sound effects
                Actor.sfxDash = null;
                Actor.sfxHit = null;
                Actor.sfxDeath = Assets.sfxHeartPickup;
                Actor.compMove.grounded = false; //actor flys in air
            }
            else if (Type == ActorType.Pet)
            {   //non-combatant actor
                Actor.aiType = ActorAI.Basic;
                Actor.enemy = false;
                Actor.compSprite.texture = Assets.mainSheet;
                Actor.health = 200;
                ResetActorLoadout(Actor);
                Actor.walkSpeed = 0.25f;
                Actor.dashSpeed = 0.5f;
                //clear actor sound effects
                Actor.sfxDash = null;
                Actor.sfxHit = null;
                Actor.sfxDeath = null;
                Actor.chaseRadius = 16 * 10; //large chase radius
            }

            #endregion


            Functions_ActorAnimationList.SetAnimationGroup(Actor);
            Functions_ActorAnimationList.SetAnimationDirection(Actor);
            Functions_Component.UpdateCellSize(Actor.compSprite);
            Functions_Component.CenterOrigin(Actor.compSprite);
        }

        public static void Update(Actor Actor)
        {
            //get the input for this frame, set actor.direction
            Functions_Input.SetInputState(Actor.compInput, Actor);


            #region Actor is not Statelocked

            if (!Actor.stateLocked)
            {
                //set actor moving/facing direction
                if (Actor.compInput.direction != Direction.None)
                { Actor.direction = Actor.compInput.direction; }
                Actor.state = Actor.inputState; //sync state to input state
                Actor.lockCounter = 0; //reset lock counter in case actor statelocks
                Actor.lockTotal = 0; //reset lock total
                Actor.compMove.speed = Actor.walkSpeed; //default to walk speed

                //Handle States
                if (Actor == Pool.hero)
                {   //handles all hero's states
                    Functions_Hero.HandleState(Pool.hero);
                } 
                else
                {   //all actors other than hero are processed as follows
                    if (Actor.state == ActorState.Interact)
                    {
                        //non-hero actor's can't interact with roomObjs right now
                    }
                    else if (Actor.state == ActorState.Dash)
                    {
                        Actor.lockTotal = 10;
                        Actor.stateLocked = true;
                        Actor.compMove.speed = Actor.dashSpeed;
                        Functions_Entity.SpawnEntity(ObjType.ParticleDashPuff, Actor);
                        Assets.Play(Actor.sfxDash);
                    }
                    else if (Actor.state == ActorState.Attack)
                    {
                        Actor.stateLocked = true;
                        Functions_Movement.StopMovement(Actor.compMove);
                        Functions_Item.UseItem(Actor.weapon, Actor);
                    }
                    else if (Actor.state == ActorState.Use)
                    {
                        Actor.stateLocked = true;
                        Functions_Movement.StopMovement(Actor.compMove);
                        Functions_Item.UseItem(Actor.item, Actor);
                    }
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
                            Functions_Entity.SpawnEntity(
                                ObjType.ParticleExplosion,
                                Actor.compSprite.position.X + Functions_Random.Int(-16, 16),
                                Actor.compSprite.position.Y + Functions_Random.Int(-16, 16),
                                Direction.None);
                        }
                    }
                }
            }

            #endregion


            //set actor animation and direction
            Functions_ActorAnimationList.SetAnimationGroup(Actor);
            Functions_ActorAnimationList.SetAnimationDirection(Actor);

            //alter actor's speed based on loadout
            //chest armor reduces movement
            if (Actor.armor == MenuItemType.ArmorChest) { Actor.compMove.speed *= 0.88f; }
            //cape armor increases movement
            else if (Actor.armor == MenuItemType.ArmorCape) { Actor.compMove.speed *= 1.06f; }
        }

    }
}