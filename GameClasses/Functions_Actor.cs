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

        public static void SetHitState(Actor Actor)
        {   //bail if actor is already dead (dont hit dead actors)
            if (Actor.state == ActorState.Dead) { return; }
            //else lock actor into hit state
            Actor.state = ActorState.Hit;
            Actor.stateLocked = true;
            Actor.lockCounter = 0;
            Actor.lockTotal = 15;
            //display the hit effect particle
            Functions_Entity.SpawnEntity(ObjType.ParticleHitSparkle, Actor);

            //play hero/enemy hit sound effect
            if (Actor == Pool.hero)
            {
                Assets.Play(Assets.sfxHeroHit);
                if (!Flags.InfiniteGold) //continue if infiniteGold is false
                {
                    if (PlayerData.current.gold > 0) //if hero has any gold
                    {   //drop a gold piece upon getting hit
                        Functions_Entity.SpawnEntity(ObjType.PickupRupee, Actor);
                        PlayerData.current.gold--;
                    }
                }
            }
            else { Assets.Play(Assets.sfxEnemyHit); }
            //if the actor hit was the boss, also play the boss hit sound
            if (Actor.type == ActorType.Boss) { Assets.Play(Assets.sfxBossHit); }
        }

        public static void SetDeathState(Actor Actor)
        {
            Actor.state = ActorState.Dead;
            Actor.stateLocked = true;
            Actor.lockCounter = 0;
            Actor.lockTotal = 255;

            if (Actor == Pool.hero)
            {   //play hero death sfx
                Assets.Play(Assets.sfxHeroKill);
                //track hero deaths here
            }   //track enemy deaths
            else { DungeonRecord.enemyCount++; }


            #region Enemy Specific Death Effects

            if (Actor.type == ActorType.Blob)
            {
                Actor.compSprite.zOffset = -16; //sort to floor
                Functions_Entity.SpawnEntity(ObjType.ParticleExplosion, Actor);
                Actor.compCollision.rec.X = -1000; //hide actor collisionRec
                Functions_Loot.SpawnLoot(Actor.compSprite.position);
                Assets.Play(Assets.sfxEnemyKill); //death sfx
            }
            else if (Actor.type == ActorType.Boss)
            {
                PlayerData.current.crystal1 = true; //flip crystal1
                DungeonRecord.beatDungeon = true; //player has beat the dungeon
                Functions_Dungeon.dungeonScreen.exitAction = ExitAction.Summary;
                Functions_Dungeon.dungeonScreen.displayState = DisplayState.Closing;
                Actor.compSprite.zOffset = -16; //sort to floor
                Actor.compCollision.rec.X = -1000; //hide actor collisionRec
                Assets.Play(Assets.sfxEnemyKill); //death sfx

                //boss explosions are handled in Update()
                //explosions are called perpetually across frames
                //this makes the explosions appear to be sequential
                //this is closer to the original LttP implementation
            }

            #endregion


            //sort actor for last time
            Functions_Component.SetZdepth(Actor.compSprite);
        }

        public static void SetCollisionRec(Actor Actor)
        {
            //set the collisionRec parameters based on the Type
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
            Actor.type = Type;
            //bring actor back to life
            Actor.stateLocked = false;
            Actor.active = true;
            Actor.lockCounter = 0;
            Actor.lockTotal = 0;
            //reset actor's state and direction
            Actor.state = ActorState.Idle;
            Actor.direction = Direction.Down;
            Actor.compMove.direction = Direction.None;
            //reset actor's collisions
            Actor.compCollision.active = true;
            Actor.compCollision.blocking = true; //actors always block
            SetCollisionRec(Actor);
            //reset actor's sprite zDepth
            Actor.compSprite.zOffset = 0;
            //assume standard actor
            Actor.compSprite.cellSize.X = 16;
            Actor.compSprite.cellSize.Y = 16;
            

            #region Actor Specific Fields

            if (Type == ActorType.Hero)
            {
                Actor.compSprite.texture = Assets.heroSheet;
                Actor.maxHealth = 14;
                //do not update/change the hero's weapon/item/armor/equipment
                Actor.walkSpeed = 0.35f;
                Actor.dashSpeed = 0.90f;
            }
            else if (Type == ActorType.Blob)
            {
                Actor.compSprite.texture = Assets.blobSheet;
                Actor.maxHealth = 1;
                ResetActorLoadout(Actor);
                Actor.weapon = MenuItemType.WeaponSword;

                Actor.walkSpeed = 0.05f;
                Actor.dashSpeed = 0.30f;
            }
            else if (Type == ActorType.Boss)
            {
                Actor.compSprite.texture = Assets.bossSheet;
                Actor.maxHealth = 10;
                ResetActorLoadout(Actor);

                Actor.walkSpeed = 0.50f;
                Actor.dashSpeed = 1.00f;

                //this actor is a boss (double size)
                Actor.compSprite.cellSize.X = 32;
                Actor.compSprite.cellSize.Y = 32;
                //the boss actor has a lower sorting point that normal actors
                Actor.compSprite.zOffset = 12;
            }

            #endregion


            //set actor's health
            Actor.health = Actor.maxHealth;

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

                //check states
                if (Actor.state == ActorState.Interact)
                {   //if there is an object to interact with, interact with it
                    if (Functions_Collision.CheckInteractionRecCollisions()) {}
                    else { Actor.state = ActorState.Idle; } //no interaction
                }
                else if (Actor.state == ActorState.Dash)
                {
                    Actor.lockTotal = 10;
                    Actor.stateLocked = true;
                    Actor.compMove.speed = Actor.dashSpeed;
                    Functions_Entity.SpawnEntity(ObjType.ParticleDashPuff, Actor);
                    if (Actor == Pool.hero) { Assets.Play(Assets.sfxDash); }
                }
                else if (Actor.state == ActorState.Attack)
                {
                    Actor.stateLocked = true;
                    Functions_Movement.StopMovement(Actor.compMove);
                    Functions_Item.UseItem(Actor.weapon, Actor);
                    if (Actor == Pool.hero) { WorldUI.currentWeapon.compSprite.scale = 2.0f; }
                }
                else if (Actor.state == ActorState.Use)
                {   
                    if (Actor.item != MenuItemType.Unknown)
                    { 
                        Actor.stateLocked = true;
                        Functions_Movement.StopMovement(Actor.compMove);
                        Functions_Item.UseItem(Actor.item, Actor);
                        if (Actor == Pool.hero) { WorldUI.currentItem.compSprite.scale = 2.0f; }
                    }
                    else { Actor.state = ActorState.Idle; } //no item to use
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
                //lock actor into the death state
                if (Actor.state == ActorState.Dead)
                {
                    Actor.lockCounter = 0; //lock actor into dead state
                    Actor.health = 0; //lock actor's health at 0


                    #region Boss Death Effects

                    if (Actor.type == ActorType.Boss)
                    {   //dead bosses perpetually explode
                        if(Functions_Random.Int(0,100) > 75) //randomly create explosions
                        {   //randomly place explosion around boss
                            Functions_Entity.SpawnEntity(
                                ObjType.ParticleExplosion,
                                Actor.compSprite.position.X + Functions_Random.Int(-16, 16),
                                Actor.compSprite.position.Y + Functions_Random.Int(-16, 16),
                                Direction.None);
                            //play corresponding explosion sound effect too
                            Assets.Play(Assets.sfxExplosion);
                        }
                    }

                    #endregion


                    #region Hero Death Effects

                    else if(Actor.type == ActorType.Hero)
                    {   //near the last frame of hero's death, create attention particles
                        if (Actor.compAnim.index == Actor.compAnim.currentAnimation.Count-2)
                        {   //this event happens when hero falls to ground
                            //goto next anim frame, this event is only processed once
                            Actor.compAnim.index++; 
                            //spawn particle to grab the player's attention
                            Functions_Entity.SpawnEntity(
                                    ObjType.ParticleAttention,
                                    Actor.compSprite.position.X,
                                    Actor.compSprite.position.Y,
                                    Direction.None);
                            //check to see if hero can use fairy bottle to selfrez
                            if (PlayerData.current.bottleFairy)
                            { Functions_Item.UseItem(MenuItemType.BottleFairy, Actor); }
                            else
                            {   //player has died, failed the dungeon
                                DungeonRecord.beatDungeon = false;
                                Functions_Dungeon.dungeonScreen.exitAction = ExitAction.Summary;
                                Functions_Dungeon.dungeonScreen.displayState = DisplayState.Closing;
                            }
                        }
                    }

                    #endregion


                }
            }

            #endregion


            //set actor animation and direction
            Functions_ActorAnimationList.SetAnimationGroup(Actor);
            Functions_ActorAnimationList.SetAnimationDirection(Actor);
        }

        public static void Draw(Actor Actor)
        {
            Functions_Draw.Draw(Actor.compSprite);
            if (Flags.DrawCollisions) { Functions_Draw.Draw(Actor.compCollision); }
        }

    }
}