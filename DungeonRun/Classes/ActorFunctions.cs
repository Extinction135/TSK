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
    public static class ActorFunctions
    {

        public static void SetHitState(Actor Actor)
        {
            Actor.state = Actor.State.Hit;
            Actor.stateLocked = true;
            Actor.lockCounter = 0;
            Actor.lockTotal = 15;

            if (Actor == Pool.hero) { Assets.sfxHeroHit.Play(); }
            else { Assets.sfxEnemyHit.Play(); }

            //display the hit effect particle
            GameObjectFunctions.SpawnParticle(GameObject.Type.ParticleHitSparkle, Actor.compSprite.position);
        }

        public static void SetDeathState(Actor Actor)
        {
            Actor.state = Actor.State.Dead;
            Actor.stateLocked = true;
            Actor.lockCounter = 0;
            Actor.lockTotal = 255;


            #region Hero Specific Death Effects

            if (Actor == Pool.hero)
            {
                //player has died, failed the dungeon
                DungeonRecord.beatDungeon = false;
                DungeonFunctions.dungeonScreen.screenState = DungeonScreen.ScreenState.FadeIn;
                //we could track hero deaths here
                Assets.sfxHeroKill.Play();
            }
            else
            {
                DungeonRecord.enemyCount++; //track non-hero actor deaths
                Assets.sfxEnemyKill.Play(); //play enemy kill soundFX
            }

            #endregion


            #region Enemy Specific Death Effects

            if (Actor.type == Actor.Type.Blob)
            {
                Actor.compSprite.zOffset = -16; //sort to floor
                GameObjectFunctions.SpawnParticle(GameObject.Type.ParticleExplosion, Actor.compSprite.position);
                Actor.compCollision.rec.X = -1000; //hide actor collisionRec
                //call spawn loot function, passing actor
            }
            else if (Actor.type == Actor.Type.Boss)
            {
                //player has beat the dungeon
                DungeonRecord.beatDungeon = true;
                DungeonFunctions.dungeonScreen.screenState = DungeonScreen.ScreenState.FadeIn;
                Actor.compSprite.zOffset = -16; //sort to floor
                Actor.compCollision.rec.X = -1000; //hide actor collisionRec
                //create boss explosion
                GameObjectFunctions.SpawnParticle(GameObject.Type.ParticleExplosion, Actor.compSprite.position);
                //create a series of explosions around boss
                GameObjectFunctions.SpawnParticle(GameObject.Type.ParticleExplosion,
                    Actor.compSprite.position + new Vector2(10, 10));
                GameObjectFunctions.SpawnParticle(GameObject.Type.ParticleExplosion,
                    Actor.compSprite.position + new Vector2(10, -10));
                GameObjectFunctions.SpawnParticle(GameObject.Type.ParticleExplosion,
                    Actor.compSprite.position + new Vector2(-10, 10));
                GameObjectFunctions.SpawnParticle(GameObject.Type.ParticleExplosion,
                    Actor.compSprite.position + new Vector2(-10, -10));
            }

            #endregion


            //sort actor for last time
            ComponentFunctions.SetZdepth(Actor.compSprite);
        }



        public static void SetCollisionRec(Actor Actor)
        {
            //set the collisionRec parameters based on the Type
            if (Actor.type == Actor.Type.Boss)
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

        public static void UseWeapon(Actor Actor)
        {
            if (Actor.weapon == Weapon.Sword) { GameObjectFunctions.SpawnProjectile(GameObject.Type.ProjectileSword, Actor); }

            //scale up the current weapon in world ui
            if (Actor == Pool.hero) { WorldUI.currentWeapon.scale = 1.4f; }
        }



        public static void SetType(Actor Actor, Actor.Type Type)
        {
            Actor.type = Type;
            //bring actor back to life
            Actor.stateLocked = false;
            Actor.active = true;
            Actor.lockCounter = 0;
            Actor.lockTotal = 0;
            //reset actor's state and direction
            Actor.state = Actor.State.Idle;
            Actor.direction = Direction.Down;
            Actor.compMove.direction = Direction.None;
            //reset actor's collisions
            Actor.compCollision.active = true;
            Actor.compCollision.blocking = true; //actors always block
            SetCollisionRec(Actor);
            //reset actor's sprite zDepth
            Actor.compSprite.zOffset = 0;
            //set actor animations group, direction
            ActorAnimationListManager.SetAnimationGroup(Actor);
            ActorAnimationListManager.SetAnimationDirection(Actor);


            #region Actor Specific Fields

            if (Type == Actor.Type.Hero)
            {
                Actor.compSprite.texture = Assets.heroSheet;
                Actor.health = 3;
                Actor.weapon = Weapon.Sword;
                Actor.walkSpeed = 0.30f;
                Actor.dashSpeed = 0.80f;
                //set actor soundFX

                //standard actor
                Actor.compSprite.cellSize.x = 16;
                Actor.compSprite.cellSize.y = 16;
                ComponentFunctions.UpdateCellSize(Actor.compSprite);
                ComponentFunctions.CenterOrigin(Actor.compSprite);

                //set the actor's animation list, actor could be a boss
                //Actor.animList = ActorAnimationListManager.actorAnims;
            }
            else if (Type == Actor.Type.Blob)
            {
                Actor.compSprite.texture = Assets.blobSheet;
                Actor.health = 1;
                Actor.weapon = Weapon.Sword;
                Actor.walkSpeed = 0.05f;
                Actor.dashSpeed = 0.30f;
                //set the actor soundFX

                //standard actor
                Actor.compSprite.cellSize.x = 16;
                Actor.compSprite.cellSize.y = 16;
                ComponentFunctions.UpdateCellSize(Actor.compSprite);
                ComponentFunctions.CenterOrigin(Actor.compSprite);

                //set the actor's animation list, actor could be a boss
                //Actor.animList = ActorAnimationListManager.actorAnims;
            }
            else if (Type == Actor.Type.Boss)
            {
                Actor.compSprite.texture = Assets.bossSheet;
                Actor.health = 10;
                Actor.weapon = Weapon.Sword;
                Actor.walkSpeed = 0.50f;
                Actor.dashSpeed = 1.00f;
                //set the actor soundFX

                //this actor is a boss (double size)
                Actor.compSprite.cellSize.x = 32;
                Actor.compSprite.cellSize.y = 32;
                ComponentFunctions.UpdateCellSize(Actor.compSprite);
                ComponentFunctions.CenterOrigin(Actor.compSprite);

                //set the actor's animation list, actor could be a boss
                //Actor.animList = ActorAnimationListManager.actorAnims;

                //the boss actor has a lower sorting point that normal actors
                Actor.compSprite.zOffset = 12;
            }

            #endregion


            Actor.animList = ActorAnimationListManager.actorAnims;
        }



        public static void Update(Actor Actor)
        {
            //get the input for this frame, set actor.direction
            Input.SetInputState(Actor.compInput, Actor);


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
                if (Actor.state == Actor.State.Interact)
                {
                    Actor.lockTotal = 10;
                    Actor.stateLocked = true;
                    ComponentFunctions.StopMovement(Actor.compMove);
                    CollisionFunctions.CheckInteractionRecCollisions();
                }
                else if (Actor.state == Actor.State.Dash)
                {
                    Actor.lockTotal = 10;
                    Actor.stateLocked = true;
                    Actor.compMove.speed = Actor.dashSpeed;
                    //create a dash particle 
                    GameObjectFunctions.SpawnParticle(GameObject.Type.ParticleDashPuff, Actor.compSprite.position);
                    if (Actor == Pool.hero) { Assets.sfxDash.Play(); }
                }
                else if (Actor.state == Actor.State.Attack)
                {
                    Actor.lockTotal = 15;
                    Actor.stateLocked = true;
                    ComponentFunctions.StopMovement(Actor.compMove);
                    UseWeapon(Actor);

                    Assets.sfxSwordSwipe.Play();
                    //if (Actor == Pool.hero) { Assets.swordSwipe.Play(); }
                }
                else if (Actor.state == Actor.State.Use)
                {
                    Actor.lockTotal = 25;
                    Actor.stateLocked = true;
                    ComponentFunctions.StopMovement(Actor.compMove);
                    //call useItem() - creates a projectile just like useWeapon()
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
                    Input.ResetInputData(Actor.compInput); //reset input component
                    //check to see if the actor is dead
                    if (Actor.health <= 0) { SetDeathState(Actor); }
                }
                //lock actor into the death state
                if (Actor.state == Actor.State.Dead) { Actor.lockCounter = 0; }
            }

            #endregion


            //set actor animation and direction
            ActorAnimationListManager.SetAnimationGroup(Actor);
            ActorAnimationListManager.SetAnimationDirection(Actor);
        }

        public static void Draw(Actor Actor)
        {
            DrawFunctions.Draw(Actor.compSprite);
            if (Flags.DrawCollisions) { DrawFunctions.Draw(Actor.compCollision); }
        }
    }
}