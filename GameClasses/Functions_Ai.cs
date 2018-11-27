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
    public static class Functions_Ai
    {
        static Actor Actor;
        static Vector2 actorPos;
        static int xDistance;
        static int yDistance;
        static int i;
        static Boolean overlap;


        public static void SetActorInput()
        {
            Pool.activeActor++; //increment the active actor
            if (Pool.activeActor >= Pool.actorCount) { Pool.activeActor = 1; } //skip 0th actor (HERO)
            //target the active actor from the actor's pool
            Actor = Pool.actorPool[Pool.activeActor];
            //if this actor isn't active, don't pass AI to it
            if (Actor.active == false) { return; }
            //if this actor is dead, don't pass AI to it
            if (Actor.state == ActorState.Dead) { return; }
            //reset the target actor's input
            Functions_Input.ResetInputData(Actor.compInput);

            //get actor's hitBox center position
            actorPos.X = Actor.compCollision.rec.Center.X;
            actorPos.Y = Actor.compCollision.rec.Center.Y;
            //get the x & y distances between actor and hero hitbox
            xDistance = (int)Math.Abs(Pool.hero.compCollision.rec.Center.X - actorPos.X);
            yDistance = (int)Math.Abs(Pool.hero.compCollision.rec.Center.Y - actorPos.Y);







            //control actor based on actor.aiType

            #region Random AI

            if(Actor.aiType == ActorAI.Random)
            {   //randomly move in a direction + dash
                Actor.compInput.direction = (Direction)Functions_Random.Int(0, 8);
                if (Functions_Random.Int(0, 100) > 80) { Actor.compInput.dash = true; }
            }

            #endregion


            #region Basic AI
            
            else if(Actor.aiType == ActorAI.Basic)
            {   
                //by default, choose a random direction to move in & randomly dash
                Actor.compInput.direction = (Direction)Functions_Random.Int(0, 8);
                if (Functions_Random.Int(0, 100) > 90) { Actor.compInput.dash = true; }

                //if the actor is an enemy, and the hero is on other team (ally)
                if (Actor.enemy & Pool.hero.enemy == false)
                {   
                    ChaseHero();

                    //determine if actor is close enough to attack hero
                    if (yDistance < Actor.attackRadius && xDistance < Actor.attackRadius)
                    {   //actor is close enough to hero to attack
                        if (Functions_Random.Int(0, 100) > 25) //randomly proceed
                        {
                            //set the cardinal direction towards hero, attack, cancel any dash
                            Actor.compInput.direction =
                                Functions_Direction.GetCardinalDirectionToHero(actorPos);
                            Actor.compInput.attack = true;
                            Actor.compInput.dash = false;
                        }
                    }
                }
                else if(Actor.enemy == false)
                {   
                    ChaseHero(); //if actor is an ally, follow(chase) hero
                    //determine if actor is close enough to stop chasing hero
                    if (yDistance < Actor.attackRadius && xDistance < Actor.attackRadius)
                    {   
                        Actor.compInput.dash = false;
                        Actor.compInput.direction = Direction.None;
                    }
                }
            }

            #endregion


            //Standard AIs

            #region BeefyBat

            else if (Actor.aiType == ActorAI.Standard_BeefyBat)
            {
                //strategy: idles in place until hero gets close, then dash attacks him

                //reset input to none (to idle)
                Actor.compInput.direction = Direction.None;
                ChaseHero(); //if close enough, compInput gets a direction towards hero

                //if actor is close enough to see hero, attack or dash towards hero
                if(Actor.compInput.direction != Direction.None) //we got a direction from chase hero
                {   //determine if actor is close enough to attack hero too
                    if (yDistance < Actor.attackRadius && xDistance < Actor.attackRadius)
                    {   //actor is close enough to hero to attack
                        if (Functions_Random.Int(0, 100) > 10) //randomly proceed
                        {   //set cardinal direction towards hero, attack
                            Actor.compInput.direction = 
                                Functions_Direction.GetCardinalDirectionToHero(actorPos);
                            Actor.compInput.attack = true;
                        }
                    }
                    else
                    {   //if we cant attack, aggressively dash towards hero
                        Actor.compInput.dash = true;
                    }
                }
                else
                {   //we cannot 'see' hero

                    //very rarely dash in a random direction (to mix up position)
                    if (Functions_Random.Int(0, 100) > 98)
                    {   
                        Actor.compInput.direction = (Direction)Functions_Random.Int(0, 8);
                        Actor.compInput.dash = true;
                    }
                    //upon coming to a rest, return to a south facing state (hivemind)
                    if(Actor.compMove.moving == false) { Actor.direction = Direction.Down; }
                }
            }

            #endregion




            //Miniboss AIs

            #region BlackEye

            else if (Actor.aiType == ActorAI.Miniboss_Blackeye)
            {
                //by default, choose a random direction to move in & randomly dash
                Actor.compInput.direction = (Direction)Functions_Random.Int(0, 8);
                if (Functions_Random.Int(0, 100) > 80) { Actor.compInput.dash = true; }

                //2 Phase - based on health, change how actor behaves
                if (Actor.health > 5) //hardline of 5
                {
                    //move slowly
                    Actor.walkSpeed = 0.05f;
                    Actor.dashSpeed = 0.30f;

                    if (Functions_Random.Int(0, 100) > 95)
                    {
                        //face hero, then use item, set use input true (displays a bow)
                        Actor.compInput.direction = Functions_Direction.GetCardinalDirectionToHero(actorPos);
                        Actor.direction = Actor.compInput.direction;
                        Actor.compInput.use = true;
                    }
                }
                else //actor is at low health
                {
                    //move very fast
                    Actor.walkSpeed = 0.20f;
                    Actor.dashSpeed = 0.50f;
                    //double up the animation (speed up)
                    Functions_Animation.Animate(Actor.compAnim, Actor.compSprite);

                    //smoke (each frame) as a sign of nearing defeat
                    Functions_Particle.Spawn(
                        ParticleType.ImpactDust,
                        Actor.compSprite.position.X + 6 + Functions_Random.Int(-8, 8),
                        Actor.compSprite.position.Y - 10 + Functions_Random.Int(-5, 5)
                    );

                    if (Functions_Random.Int(0, 100) > 75)
                    {   //shoot arrows at hero more often
                        //face hero, then use item, set use input true (displays a bow)
                        Actor.compInput.direction = Functions_Direction.GetCardinalDirectionToHero(actorPos);
                        Actor.direction = Actor.compInput.direction;
                        Actor.compInput.use = true;
                    }

                    //rarely taunt the player
                    if (Functions_Random.Int(0, 100) > 95)
                    {
                        Assets.Play(Assets.sfxEnemyTaunt);
                    }
                }
            }

            #endregion


            #region OctoMouth

            else if (Actor.aiType == ActorAI.MiniBoss_OctoMouth)
            {
                //setup movement speeds
                if (Actor.health < 5)
                {   //move ALOT faster
                    Actor.walkSpeed = 1.20f;
                    Actor.dashSpeed = 4.00f;
                }
                else
                {
                    Actor.walkSpeed = 0.40f;
                    Actor.dashSpeed = 1.00f;
                }


                #region Underwater routine

                if (Actor.underwater)
                {
                    //randomly change directions and dash
                    if (Functions_Random.Int(0, 100) > 70)
                    {
                        if (Actor.health < 5)
                        {   //move towards hero
                            Actor.compInput.direction = 
                                Functions_Direction.GetCardinalDirectionToHero(actorPos);
                            Actor.compInput.dash = true;
                        }
                        else
                        {   //move randomly
                            Actor.compInput.direction = (Direction)Functions_Random.Int(0, 8);
                            Actor.compInput.dash = true;
                        }
                    }

                    //use breath counter to keep enemy underwater for a moment
                    Actor.breathCounter++;
                    if (Actor.breathCounter > Functions_Random.Int(15, 25)) //not in frames!
                    {
                        Actor.underwater = false;
                        Functions_Actor.CreateSplash(Actor);
                        Actor.breathCounter = 0;
                    }
                }

                #endregion


                #region Above water routine

                else
                {
                    //face the direction of the hero at all times
                    Actor.direction = Functions_Direction.GetCardinalDirectionToHero(actorPos);
                    //but don't move in any direction
                    Actor.compInput.direction = Direction.None;
                    //stop any movement
                    Functions_Movement.StopMovement(Actor.compMove);

                    //shoot fireballs at hero
                    if (Functions_Random.Int(0, 100) > 92)
                    {   
                        Actor.compInput.use = true;
                    }

                    //rarely randomly dive (give hero time to attack them)
                    if (Functions_Random.Int(0, 100) > 98)
                    {
                        Actor.underwater = true;
                        Functions_Actor.CreateSplash(Actor);
                        Actor.breathCounter = 0;
                    }

                    if (Actor.health < 5)
                    {   
                        //smoke (each frame) as a sign of nearing defeat
                        Functions_Particle.Spawn(
                            ParticleType.ImpactDust,
                            Actor.compSprite.position.X + 6 + Functions_Random.Int(-8, 8),
                            Actor.compSprite.position.Y - 10 + Functions_Random.Int(-5, 5)
                        );

                        //often taunt the player
                        if (Functions_Random.Int(0, 100) > 50)
                        { Assets.Play(Assets.sfxEnemyTaunt); }
                    }
                }

                #endregion

            }

            #endregion




            //Boss AIs

            #region BigEye

            else if (Actor.aiType == ActorAI.Boss_BigEye)
            {
                //by default, choose a random direction to move in & randomly dash
                Actor.compInput.direction = (Direction)Functions_Random.Int(0, 8);
                if (Functions_Random.Int(0, 100) > 70) { Actor.compInput.dash = true; }


                #region Phase 1

                if(Actor.health > 10) 
                {
                    //move faster
                    Actor.walkSpeed = 0.25f;
                    Actor.dashSpeed = 0.30f;

                    if (Functions_Random.Int(0, 100) > 60)
                    {   //often spawn seeker
                        Functions_GameObject.Spawn(ObjType.Wor_SeekerExploder, 
                            actorPos.X, actorPos.Y, Direction.Down);
                    }
                }

                #endregion


                #region Phase 2

                else //actor is below 10 health
                {
                    //move and dash faster
                    Actor.walkSpeed = 0.4f;
                    Actor.dashSpeed = 0.6f;
                    //double up the animation (speed up)
                    Functions_Animation.Animate(Actor.compAnim, Actor.compSprite);

                    //strategy 2 - aggresively bite hero
                    ChaseHero(); //always chases hero
                    //now boss will rely on bite attack only

                    //smoke (each frame) as a sign of nearing defeat/rage
                    Functions_Particle.Spawn(
                        ParticleType.ImpactDust,
                        Actor.compSprite.position.X + 6 + Functions_Random.Int(-8, 8),
                        Actor.compSprite.position.Y - 10 + Functions_Random.Int(-5, 5)
                    );
                    //rarely taunt the player
                    if (Functions_Random.Int(0, 100) > 95)
                    {
                        Assets.Play(Assets.sfxEnemyTaunt);
                    }

                    //occasionally spawn a seeker
                    if (Functions_Random.Int(0, 100) > 90)
                    {   
                        Functions_GameObject.Spawn(ObjType.Wor_SeekerExploder,
                            actorPos.X, actorPos.Y, Direction.Down);
                    }
                }

                #endregion


                Check_Bite();
            }

            #endregion


            #region BigBat

            else if (Actor.aiType == ActorAI.Boss_BigBat)
            {
                //by default, choose a random direction to move in & randomly dash
                Actor.compInput.direction = (Direction)Functions_Random.Int(0, 8);
                if (Functions_Random.Int(0, 100) > 70) { Actor.compInput.dash = true; }


                #region Phase 1

                if (Actor.health > 10)
                {
                    //initial speed
                    Actor.walkSpeed = 0.25f;
                    Actor.dashSpeed = 0.30f;

                    //rarely use bat magic
                    if (Functions_Random.Int(0, 100) > 85)
                    { Actor.compInput.use = true; }
                }

                #endregion


                #region Phase 2

                else if (Actor.health > 5)
                {
                    //dash fast
                    Actor.walkSpeed = 0.25f;
                    Actor.dashSpeed = 0.80f;
                    //double up the animation (speed up)
                    Functions_Animation.Animate(Actor.compAnim, Actor.compSprite);

                    ChaseHero(); //boss becomes aggressive
                    //boss switches to bite attack

                    //rarely taunt the player
                    if (Functions_Random.Int(0, 100) > 95)
                    { Assets.Play(Assets.sfxEnemyTaunt); }
                }

                #endregion


                #region Phase 3

                else
                {
                    //move towards center of room
                    Actor.compInput.direction = 
                        Functions_Direction.GetDiagonalToCenterOfRoom(Actor.compSprite.position);

                    //limit special attack
                    if(Functions_Random.Int(0, 100) > 50)
                    {   //Super Special Attack : spam spawn bats in all directions
                        Functions_Projectile.Spawn(ProjectileType.Bat, Actor, Direction.Down);
                        Functions_Projectile.Spawn(ProjectileType.Bat, Actor, Direction.DownRight);
                        Functions_Projectile.Spawn(ProjectileType.Bat, Actor, Direction.Right);
                        Functions_Projectile.Spawn(ProjectileType.Bat, Actor, Direction.UpRight);
                        Functions_Projectile.Spawn(ProjectileType.Bat, Actor, Direction.Up);
                        Functions_Projectile.Spawn(ProjectileType.Bat, Actor, Direction.UpLeft);
                        Functions_Projectile.Spawn(ProjectileType.Bat, Actor, Direction.Left);
                        Functions_Projectile.Spawn(ProjectileType.Bat, Actor, Direction.DownLeft);
                    }

                    //always taunt the player
                    Assets.Play(Assets.sfxEnemyTaunt);

                    //smoke (each frame) as a sign of nearing defeat
                    Functions_Particle.Spawn(
                        ParticleType.ImpactDust,
                        Actor.compSprite.position.X + 6 + Functions_Random.Int(-8, 8),
                        Actor.compSprite.position.Y - 10 + Functions_Random.Int(-5, 5)
                    );

                    //set some states false
                    Actor.compInput.use = false;
                    Actor.compInput.dash = false;
                    Actor.compInput.attack = false;
                    return; //dont worry about bite / special attack below
                }

                #endregion


                Check_Bite();


                #region Special Attack - Bat Swarm

                //bat boss special attack:
                //create lots of bats each time boss uses magic
                if (Actor.compInput.use)
                {
                    //spawn 2 bats in the direction boss is moving
                    Functions_Projectile.Spawn(ProjectileType.Bat, Actor, 
                        Actor.compInput.direction);
                    Functions_Projectile.Spawn(ProjectileType.Bat, Actor,
                        Actor.compInput.direction);

                    //spawn 2 bats towards hero, diagonally
                    Functions_Projectile.Spawn(ProjectileType.Bat, Actor,
                        Functions_Direction.GetDiagonalToHero(Actor.compSprite.position));
                    Functions_Projectile.Spawn(ProjectileType.Bat, Actor,
                        Functions_Direction.GetDiagonalToHero(Actor.compSprite.position));

                    //spawn 2 bats towards center of room
                    Functions_Projectile.Spawn(ProjectileType.Bat, Actor,
                        Functions_Direction.GetDiagonalToCenterOfRoom(Actor.compSprite.position));
                    Functions_Projectile.Spawn(ProjectileType.Bat, Actor,
                        Functions_Direction.GetDiagonalToCenterOfRoom(Actor.compSprite.position));
                }

                #endregion

            }

            #endregion


            #region OctoHead / Kraken

            else if (Actor.aiType == ActorAI.Boss_OctoHead)
            {
                //setup movement speeds
                if (Actor.health < 5)
                {   //move ALOT faster
                    Actor.walkSpeed = 1.00f;
                    Actor.dashSpeed = 4.00f;
                }
                else
                {
                    Actor.walkSpeed = 0.50f;
                    Actor.dashSpeed = 1.00f;
                }


                #region Underwater routine

                if (Actor.underwater)
                {
                    //Debug.WriteLine("count v counter: " + Actor.breathCounter + " / " + Actor.breathTotal);

                    //randomly dash
                    if (Functions_Random.Int(0, 100) > 80) { Actor.compInput.dash = true; }

                    //use breath counter to keep enemy underwater for a moment
                    Actor.breathCounter++;
                    if (Actor.breathCounter > Functions_Random.Int(15, 25)) //not in frames!
                    {
                        Actor.underwater = false;
                        Functions_Actor.CreateSplash(Actor);
                        Actor.breathCounter = 0;

                        //spawn a tentacle on way back to surface (since boss took damage)
                        Functions_Actor.SpawnActor(ActorType.Special_Tentacle, Actor.compSprite.position);
                    }
                }

                #endregion


                #region Above water routine

                else
                {
                    Actor.direction = Direction.Down;
                    //but don't move in any direction
                    Actor.compInput.direction = Direction.None;

                    //boss waits for something to hit him to dive
                    if (Actor.health < 5)
                    {
                        //often taunt the player
                        if (Functions_Random.Int(0, 100) > 50)
                        { Assets.Play(Assets.sfxEnemyTaunt); }
                    }
                }

                #endregion


                //randomly move in a direction
                if (Functions_Random.Int(0, 100) > 70)
                { Actor.compInput.direction = (Direction)Functions_Random.Int(0, 8); }

                //handle phase 2 low on health decorations
                if (Actor.health < 5)
                {   //smoke (each frame) as a sign of nearing defeat
                    Functions_Particle.Spawn(
                        ParticleType.ImpactDust,
                        Actor.compSprite.position.X + 6 + Functions_Random.Int(-8, 8),
                        Actor.compSprite.position.Y - 0 + Functions_Random.Int(-5, 5)
                    );
                }

            }

            #endregion




            //Special AIs

            #region Tentacle
            
            else if (Actor.aiType == ActorAI.Special_Tentacle)
            {

                #region Underwater routine

                if (Actor.underwater)
                {
                    //randomly move in a direction
                    if (Functions_Random.Int(0, 100) > 80)
                    { Actor.compInput.direction = (Direction)Functions_Random.Int(0, 8); }

                    //always dash while underwater
                    Actor.compInput.dash = true;

                    //use breath counter to keep enemy underwater for a moment
                    Actor.breathCounter++;
                    if (Actor.breathCounter > Functions_Random.Int(30, 60)) //not in frames!
                    {
                        Actor.underwater = false;
                        Functions_Actor.CreateSplash(Actor);
                        Actor.breathCounter = 0;
                    }
                }

                #endregion


                #region Above water routine

                else
                {
                    //slowly chase hero (effect: many tentacles converge slowly around hero)
                    ChaseHero();
                    Actor.compInput.dash = false;

                    //always face the direction of the hero (hivemind)
                    Actor.compInput.direction = Functions_Direction.GetCardinalDirectionToHero(actorPos);
                    Actor.direction = Actor.compInput.direction;

                    if (yDistance < Actor.attackRadius & xDistance < Actor.attackRadius)
                    {   //actor is close enough to hero to attack, do so
                        Actor.compInput.use = true;
                    }
                    //note: tentacle doesn't dive on it's own, HAS to be hit by projectile
                }

                #endregion

            }

            #endregion





        }

        public static void ChaseHero()
        {
            //if the hero's underwater (hidden), just bail from rest of method
            if (Pool.hero.underwater) { return; }
            //if hero's dead, bail
            if (Pool.hero.state == ActorState.Dead) { return; }

            //actor is close enough to chase hero, set actor's direction to hero
            if (yDistance < Actor.chaseRadius && xDistance < Actor.chaseRadius)
            {
                if(Actor.chaseDiagonally)
                {   //chasing diagonally lets actors slide around blocking objs, appearing smart
                    Actor.compInput.direction = Functions_Direction.GetDiagonalToHero(actorPos);
                }
                else
                {   //chasing cardinally lets actors easily get stuck on blocking objs, appearing dumb
                    Actor.compInput.direction = Functions_Direction.GetCardinalDirectionToHero(actorPos);
                }   //some actor sprites require cardinal movement, tho
            }
        }

        public static void Check_Bite()
        {   //determine if actor is close enough to attack hero
            if (yDistance < Actor.attackRadius & xDistance < Actor.attackRadius)
            {   //actor is close enough to hero to attack
                //set the cardinal direction towards hero, attack, cancel other inputs
                Actor.compInput.direction = Functions_Direction.GetCardinalDirectionToHero(actorPos);
                Actor.compInput.attack = true;
                Actor.compInput.dash = false;
                Actor.compInput.use = false;
            }
        }





        
        public static void HandleObj(GameObject Obj)
        {   //keep in mind this method is called every frame



            //Obj.group checks

            #region Spreading water thru ditches

            if(Obj.group == ObjGroup.Ditch)
            {
                //this ditch is 'filled' (hasAI), so it spreads to nearby unfilled ditches
                Obj.lifeCounter++; //this isn't being used on roomObjs, so we steal it
                if (Obj.lifeCounter == Obj.interactiveFrame)
                {   //reset timer
                    Obj.lifeCounter = 0; //only 'spread' water to empty ditches on their interactive frame

                    //loop over all active roomObjs, //locating an unfilled ditches
                    for (i = 0; i < Pool.roomObjCount; i++)
                    {   
                        if (Pool.roomObjPool[i].active &
                            Pool.roomObjPool[i].group == ObjGroup.Ditch &
                            Pool.roomObjPool[i].getsAI == false) //unfilled
                        {
                            //expand horizontally
                            Obj.compCollision.rec.Width = 22;
                            Obj.compCollision.rec.X -= 4;
                            //check collisions
                            if (Pool.roomObjPool[i].compCollision.rec.Intersects(Obj.compCollision.rec))
                            {   //fill empty ditch obj (will create splash)
                                Functions_Dig.FillDitch(Pool.roomObjPool[i]);
                            }
                            //contract
                            Obj.compCollision.rec.Width = 16;
                            Obj.compCollision.rec.X += 4;

                            //expand vertically
                            Obj.compCollision.rec.Height = 22;
                            Obj.compCollision.rec.Y -= 4;
                            //check collisions
                            if (Pool.roomObjPool[i].compCollision.rec.Intersects(Obj.compCollision.rec))
                            {   //fill empty ditch obj (will create splash)
                                Functions_Dig.FillDitch(Pool.roomObjPool[i]);
                            }
                            //retract
                            Obj.compCollision.rec.Height = 16;
                            Obj.compCollision.rec.Y += 4;
                        }
                    }
                }
            }

            #endregion


            #region Handle RoomObj Enemies - behavior

            else if (Obj.group == ObjGroup.Enemy)
            {

                //if an enemy has gone beyond the bounds of a roomRec, release without loot
                if (!LevelSet.currentLevel.currentRoom.rec.Contains(Obj.compSprite.position))
                {
                    Functions_Pool.Release(Obj);
                }


                #region Enemy - Turtles & Crabs

                if (Obj.type == ObjType.Wor_Enemy_Turtle
                    || Obj.type == ObjType.Wor_Enemy_Crab)
                {   //rarely gently push in a direction
                    if (Functions_Random.Int(0, 1001) > 900)
                    {
                        Functions_Movement.Push(Obj.compMove,
                            Functions_Direction.GetRandomDirection(), 0.5f);
                    }
                }

                #endregion


                #region Enemy - Rats

                else if (Obj.type == ObjType.Wor_Enemy_Rat)
                {
                    //very rarely play rat soundfx
                    if (Functions_Random.Int(0, 1001) > 999)
                    { Assets.Play(Assets.sfxRatSqueak); }

                    //rarely choose a random cardinal direction
                    if (Functions_Random.Int(0, 1001) > 990)
                    {
                        Obj.direction = Functions_Direction.GetRandomCardinal();
                        //set animation frame based on direction
                        if (Obj.direction == Direction.Up)
                        { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Enemy_Rat_Up; }
                        else if (Obj.direction == Direction.Right)
                        { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Enemy_Rat_Right; }
                        else if (Obj.direction == Direction.Down)
                        { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Enemy_Rat_Down; }
                        else if (Obj.direction == Direction.Left)
                        { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Enemy_Rat_Left; }
                    }

                    //often push in current direction
                    if (Functions_Random.Int(0, 1001) > 700)
                    { Functions_Movement.Push(Obj.compMove, Obj.direction, 1.0f); }
                }

                #endregion


                #region Enemy - Seeker Exploders

                else if (Obj.type == ObjType.Wor_SeekerExploder)
                {
                    //get the x & y distances between actor and hero
                    xDistance = (int)Math.Abs(Pool.hero.compSprite.position.X - Obj.compSprite.position.X);
                    yDistance = (int)Math.Abs(Pool.hero.compSprite.position.Y - Obj.compSprite.position.Y);

                    if (yDistance < 16 * 10 & xDistance < 16 * 10) //can seeker see hero?
                    {   //set direction towards hero
                        Obj.compMove.direction = Functions_Direction.GetDiagonalToHero(Obj.compSprite.position);
                        Obj.direction = Obj.compMove.direction;
                        //seeker moves with high energy
                        Functions_Movement.Push(Obj.compMove, Obj.compMove.direction, 0.25f);
                        Obj.compAnim.currentAnimation = AnimationFrames.Wor_SeekerExploder_Chase;
                    }
                    else
                    {   //randomly set direction seeker moves
                        if (Functions_Random.Int(0, 100) > 95)
                        { Obj.compMove.direction = Functions_Direction.GetRandomDirection(); }
                        Obj.direction = Obj.compMove.direction;
                        //seeker moves with less energy
                        Functions_Movement.Push(Obj.compMove, Obj.compMove.direction, 0.1f);
                        Obj.compAnim.currentAnimation = AnimationFrames.Wor_SeekerExploder_Idle;
                    }

                    if (yDistance < 15 & xDistance < 15) //is seeker close enough to explode?
                    {   //instantly explode
                        Functions_Projectile.Spawn(
                            ProjectileType.Explosion,
                            Obj.compSprite.position.X,
                            Obj.compSprite.position.Y,
                            Direction.None);
                        Functions_Pool.Release(Obj);
                    }
                }

                #endregion


            }

            #endregion






            //Obj.type checks

            #region Dungeon Objects

            if (Obj.type == ObjType.Dungeon_Flamethrower)
            {
                if (Functions_Random.Int(0, 500) > 497) //aggressively shoots
                {   //shoot fireball towards hero along a cardinal direction
                    Functions_Projectile.Spawn(
                        ProjectileType.Fireball, 
                        Obj.compMove.position.X,
                        Obj.compMove.position.Y,
                        Functions_Direction.GetCardinalDirectionToHero(Obj.compSprite.position));
                }
            }


            else if (Obj.type == ObjType.Dungeon_WallStatue)
            {
                if (Functions_Random.Int(0, 2000) > 1998) //rarely shoots
                {   //lol, this is wrong and spawns arrow ontop of statue
                    Functions_Projectile.Spawn(
                        ProjectileType.Arrow, 
                        Obj.compMove.position.X,
                        Obj.compMove.position.Y, 
                        Obj.direction);
                }   //this should have an offset applied based on it's' direction
            }

            else if (Obj.type == ObjType.Dungeon_Pit)
            {
                if (Functions_Random.Int(0, 2000) > 1997) //occasionally bubbles
                { Functions_Particle.Spawn(ParticleType.PitBubble, Obj); }
            }

            #endregion


            #region Switches

            else if (Obj.type == ObjType.Dungeon_Switch || Obj.type == ObjType.Dungeon_SwitchDown)
            {   
                //only if a level is a switch puzzle type do we enable switches
                if(LevelSet.currentLevel.currentRoom.puzzleType == PuzzleType.Switch)
                {
                    overlap = false; //assume no hit

                    //loop over all active actors
                    for (i = 0; i < Pool.actorCount; i++)
                    {   //allow dead actor corpses to activate switches
                        if (Pool.actorPool[i].active &
                            Pool.actorPool[i].compCollision.blocking &
                            Pool.actorPool[i].compCollision.rec.Intersects(Obj.compCollision.rec))
                        { overlap = true; }
                    }

                    //loop over all active blocking roomObjs
                    for (i = 0; i < Pool.roomObjCount; i++)
                    {   //only blocking objs can activate switches
                        if (Pool.roomObjPool[i].active &
                            Pool.roomObjPool[i].compCollision.blocking &
                            Pool.roomObjPool[i].compCollision.rec.Intersects(Obj.compCollision.rec))
                        { overlap = true; }
                    }

                    //if any actors/objs overlap switch, openTrap doors
                    if (overlap)
                    {
                        Functions_GameObject_Dungeon.OpenTrapDoors();
                        //bail if we already did this
                        if (Obj.type == ObjType.Dungeon_SwitchDown) { return; }
                        Functions_GameObject.SetType(Obj, ObjType.Dungeon_SwitchDown);
                        Functions_Particle.Spawn(
                            ParticleType.Attention,
                            Obj.compSprite.position.X,
                            Obj.compSprite.position.Y);
                        Assets.Play(Assets.sfxSwitch);
                    }
                    //else close all open doors to trap doors
                    else
                    {
                        Functions_GameObject_Dungeon.CloseTrapDoors();
                        //bail if we already did this
                        if (Obj.type == ObjType.Dungeon_Switch) { return; }
                        Functions_GameObject.SetType(Obj, ObjType.Dungeon_Switch);
                        Functions_Particle.Spawn(
                            ParticleType.Attention,
                            Obj.compSprite.position.X,
                            Obj.compSprite.position.Y);
                        Assets.Play(Assets.sfxSwitch);
                    }
                }
            }


            #endregion


            #region Fairy

            else if (Obj.type == ObjType.Dungeon_Fairy)
            {
                if (Functions_Random.Int(0, 101) > 93) //float around
                {   //randomly push fairy a direction
                    Functions_Movement.Push(Obj.compMove,
                        Functions_Direction.GetRandomDirection(), 1.0f);
                    //check that the fairy overlaps the current room rec,
                    //otherwise the fairy has strayed too far and must be killed
                    if (!LevelSet.currentLevel.currentRoom.rec.Contains(Obj.compSprite.position))
                    {
                        Functions_Particle.Spawn(
                            ParticleType.Attention,
                            Obj.compSprite.position.X + 0,
                            Obj.compSprite.position.Y + 0);
                        Functions_Pool.Release(Obj);
                    }
                }
            }

            #endregion







            //world objects

            #region Bush Stump

            //bush stump obj growing back into a bush
            else if (Obj.type == ObjType.Wor_Bush_Stump)
            {
                Obj.lifeCounter++; //this isn't being used on roomObjs, so we steal it
                if (Obj.lifeCounter == Obj.interactiveFrame)
                {   //reset timer
                    Obj.lifeCounter = 0;

                    //dramatically expand stump's hitBox
                    Obj.compCollision.rec.Width = 32;
                    Obj.compCollision.rec.Height = 32;
                    Obj.compCollision.rec.X = (int)Obj.compSprite.position.X - 16;
                    Obj.compCollision.rec.Y = (int)Obj.compSprite.position.Y - 16;


                    #region Prevent Growth ONTO active Actors in room

                    //stop regrowing if bush touches ANY actor (else will grow-lock actors)
                    for (i = 0; i < Pool.actorCount; i++)
                    {   //note: this wont grow bushes over dead actors, but we could add a death check
                        if (Pool.actorPool[i].active) 
                        {
                            if(Pool.actorPool[i].compCollision.rec.Intersects(Obj.compCollision.rec))
                            {   //reset hitBox, bail from method
                                Functions_GameObject.SetType(Obj, ObjType.Wor_Bush_Stump);
                                return;
                            }
                        }
                    }

                    #endregion


                    //loop over all active roomObjs, looking at filled ditches + water tiles
                    for (i = 0; i < Pool.roomObjCount; i++)
                    {
                        if (
                            //filled ditch
                            (Pool.roomObjPool[i].active &
                            Pool.roomObjPool[i].group == ObjGroup.Ditch &
                            Pool.roomObjPool[i].getsAI == true)
                            ||
                            //water tile
                            (Pool.roomObjPool[i].active &
                            Pool.roomObjPool[i].type == ObjType.Wor_Water)
                            )
                        {
                            //check collisions
                            if (Pool.roomObjPool[i].compCollision.rec.Intersects(Obj.compCollision.rec))
                            {   //regrow into bush, with a pop
                                Functions_Particle.Spawn(
                                    ParticleType.Attention,
                                    Obj.compSprite.position.X,
                                    Obj.compSprite.position.Y);
                                Functions_GameObject.SetType(Obj, ObjType.Wor_Bush);
                                Assets.Play(Assets.sfxGrassWalk); //sounds kinda grow-y
                                return; //this only needs to happen once
                            }
                        }
                    }

                    //if the stump has reached this code, it never reached
                    //a filled ditch, so it should remain a stump - BUT,
                    //we need to reset it's hitBox. safe way of doing this:
                    Functions_GameObject.SetType(Obj, ObjType.Wor_Bush_Stump);
                }
            }

            #endregion


            #region Tree - Burning

            else if (Obj.type == ObjType.Wor_Tree_Burning)
            {   //check to see if tree should still burn
                if (Obj.lifeCounter < Obj.lifetime)
                {
                    Obj.lifeCounter++;
                    if (Obj.lifeCounter < 75) //place fires only at beginning
                    {
                        if (Functions_Random.Int(0, 100) > 93)
                        {   //often spawn fires on the bushy top
                            Functions_Particle.Spawn(ParticleType.Fire,
                                Obj.compSprite.position.X + Functions_Random.Int(-6, 6),
                                Obj.compSprite.position.Y + Functions_Random.Int(-8, 4));
                        }
                        if (Functions_Random.Int(0, 100) > 93)
                        {   //less often spawn fires along the tree trunk
                            Functions_Particle.Spawn(ParticleType.Fire,
                                Obj.compSprite.position.X + 0,
                                Obj.compSprite.position.Y + Functions_Random.Int(4, 16));
                        }
                    }
                }
                //stop 'burning' phase of tree, remove from AI calculations
                else
                {
                    Assets.Play(Assets.sfxActorLand); //decent popping sound
                    //pop the bushy top part
                    Functions_Particle.Spawn(
                        ParticleType.Attention,
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y - 2);
                    //pop leaves in circular decorative pattern for tree top
                    Functions_Particle.Spawn_Explosion(
                        ParticleType.LeafGreen,
                        Obj.compSprite.position.X + 2,
                        Obj.compSprite.position.Y - 4, true);
                    //switch to burned tree
                    Functions_GameObject.Reset(Obj);
                    Functions_GameObject.SetType(Obj, ObjType.Wor_Tree_Burnt);
                }
            }

            #endregion


            #region Collapsing Roofs

            else if (Obj.type == ObjType.Wor_Build_Roof_Collapsing)
            {
                Obj.lifeCounter++;
                if (Obj.lifeCounter == Obj.interactiveFrame)
                {
                    //Obj.lifeCounter = 0; //no need to reset, this only happens once
                    //expand to check surrounding tiles
                    Obj.compCollision.rec.Width = 32;
                    Obj.compCollision.rec.Height = 32;
                    Obj.compCollision.rec.X = (int)Obj.compSprite.position.X - 16;
                    Obj.compCollision.rec.Y = (int)Obj.compSprite.position.Y - 16;

                    //loop over all active roomObjs, collapse any nearby roofs
                    for (i = 0; i < Pool.roomObjCount; i++)
                    {
                        if (Pool.roomObjPool[i].active)
                        {
                            if (Pool.roomObjPool[i].type == ObjType.Wor_Build_Roof_Bottom
                                || Pool.roomObjPool[i].type == ObjType.Wor_Build_Roof_Top
                                || Pool.roomObjPool[i].type == ObjType.Wor_Build_Roof_Chimney)
                            {   //check for overlap / interaction
                                if (Pool.roomObjPool[i].compCollision.rec.Intersects(Obj.compCollision.rec))
                                { Functions_GameObject_World.CollapseRoof(Pool.roomObjPool[i]); }
                            }
                        }
                    }

                    //pop attention and debris
                    Functions_Particle.Spawn(
                        ParticleType.Attention, 
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y);
                    Functions_Particle.Spawn_Explosion(
                        ParticleType.DebrisBrown,
                        Obj.compSprite.position.X, 
                        Obj.compSprite.position.Y, false);
                    
                    //turn this roof obj into debris
                    Functions_GameObject.BecomeDebris(Obj);
                }
            }

            #endregion


            #region Chimney - smoke

            else if (Obj.type == ObjType.Wor_Build_Roof_Chimney)
            {   //chimney is a roof
                if(Obj.compSprite.alpha == 1.0f) //chimney is visible
                {   //roofs switch between visible and non-visible via alpha
                    if (Functions_Random.Int(0, 100) > 90)
                    {   //often spawn rising smoke particles from obj
                        Functions_Particle.Spawn(
                            ParticleType.RisingSmoke,
                            Obj.compSprite.position.X + 4 + Functions_Random.Int(-3, 3),
                            Obj.compSprite.position.Y - 2 + Functions_Random.Int(-5, 3));
                    }
                }
            }

            #endregion





            //coliseum objects

            #region Colliseum Spectators

            else if (Obj.type == ObjType.Wor_Colliseum_Spectator)
            {   //randomly create an exclamation particle
                if (Functions_Random.Int(0, 1000) > 995)
                {
                    //set a random number to determine which spectator speaks
                    Obj.interactiveFrame = Functions_Random.Int(0, 100);
                    //spawn on which spectator?
                    if (Obj.interactiveFrame < 25)
                    {
                        //#1
                        Functions_Particle.Spawn(
                            ParticleType.ExclamationBubble,
                            Obj.compSprite.position.X - 3,
                            Obj.compSprite.position.Y - 16);
                    }
                    else if(Obj.interactiveFrame < 50)
                    {
                        //#2
                        Functions_Particle.Spawn(
                            ParticleType.ExclamationBubble,
                            Obj.compSprite.position.X - 3 + 16 * 1,
                            Obj.compSprite.position.Y - 16);
                    }
                    else if (Obj.interactiveFrame < 75)
                    {
                        //#3
                        Functions_Particle.Spawn(
                            ParticleType.ExclamationBubble,
                            Obj.compSprite.position.X - 3 + 16 * 2,
                            Obj.compSprite.position.Y - 16);
                    }
                    else 
                    {
                        //#4
                        Functions_Particle.Spawn(
                            ParticleType.ExclamationBubble,
                            Obj.compSprite.position.X - 3 + 16 * 3,
                            Obj.compSprite.position.Y - 16);
                    }
                }
            }

            #endregion


            #region Colliseum Judge

            else if (Obj.type == ObjType.Judge_Colliseum)
            {
                //periodically check to see if all active actors are dead
                //if true, then hero has completed active challenge

                Obj.lifeCounter++;
                if (Obj.lifeCounter == Obj.interactiveFrame)
                {   //reset timer
                    Obj.lifeCounter = 0;


                    #region Check hero death + enemy death

                    //check to see if hero has died or killed all enemies
                    for(i = 0; i <  Pool.actorCount; i++)
                    {
                        if(Pool.actorPool[i].active)
                        {   
                            if(Pool.actorPool[i] == Pool.hero)
                            {   //check to see if the actor has died
                                if(Pool.hero.state == ActorState.Dead)
                                {
                                    // <<< exit condition : failed >>>


                                    #region Pop Failure Dialog

                                    //initially, current dialog wont be challenge failure, so only pops once
                                    if(Screens.Dialog.dialogs != AssetsDialog.Colliseum_Challenge_Failure)
                                    {   //this prevents spamming of failure dialog, which is annoying
                                        Screens.Dialog.SetDialog(AssetsDialog.Colliseum_Challenge_Failure);
                                        ScreenManager.AddScreen(Screens.Dialog);
                                    }

                                    #endregion


                                    //check to see if hero is at end of death animation,
                                    //this gives the player time to visually process hero 
                                    //spinning, falling to ground, screen fading in black

                                    if (Pool.hero.compAnim.index == Pool.hero.compAnim.currentAnimation.Count)
                                    {   //kicked out of colliseum to overworld screen
                                        Functions_Level.CloseLevel(ExitAction.Summary);
                                        //change the judge to another obj to prevent dialog spam
                                        Functions_GameObject.SetType(Obj, ObjType.Vendor_Colliseum_Mob);
                                        return; //challenge is not complete
                                    }
                                }
                            }
                            else
                            {
                                //find any living actor, fail check
                                if (Pool.actorPool[i].state != ActorState.Dead)
                                { return; } //challenge is not complete
                            }
                        }
                    }

                    #endregion


                    //if code gets here, then hero is only living actor
                    //this means the challenge has been completed


                    #region Reward the player based on current ChallengeSet

                    //standard
                    if (Functions_Colliseum.currentChallenge == Challenges.Blobs)
                    {   //reward hero with gold
                        PlayerData.current.gold += 25;
                    }

                    //minis
                    else if(Functions_Colliseum.currentChallenge == Challenges.Mini_Blackeyes
                        || Functions_Colliseum.currentChallenge == Challenges.Mini_Spiders)
                    {   //reward hero with gold
                        PlayerData.current.gold += 99;
                    }

                    //bosses
                    else if (Functions_Colliseum.currentChallenge == Challenges.Bosses_BigEye
                        || Functions_Colliseum.currentChallenge == Challenges.Bosses_BigBat
                        || Functions_Colliseum.currentChallenge == Challenges.Bosses_Kraken)
                    {   //reward hero with gold
                        PlayerData.current.gold += 99;
                    }

                    #endregion


                    // <<< exit condition : completed >>>
                    //exit level to pit level
                    LevelSet.currentLevel.ID = LevelID.SkullIsland_ColliseumPit;
                    Functions_Level.CloseLevel(ExitAction.Field);

                    //pop a new dialog screen telling player they completed challenge
                    Screens.Dialog.SetDialog(AssetsDialog.Colliseum_Challenge_Complete);
                    ScreenManager.AddScreen(Screens.Dialog);


                    Assets.Play(Assets.sfxGoldSpam); //audibly cue player they were rewarded
                    Assets.Play(Assets.sfxKeyPickup); //oh no, too spicy!
                }
            }


            #endregion

            



            //pets

            #region Pet - Dog

            else if (Obj.type == ObjType.Pet_Dog)
            {

                //pet models 'state' based on the hero's state
                if(Pool.hero.state == ActorState.Climbing)
                {
                    //place pet at hero's location, slightly lower than hero
                    //this simulates hero climbing with pet in his backpack
                    Functions_Movement.Teleport(Obj.compMove,
                        Pool.hero.compSprite.position.X,
                        Pool.hero.compSprite.position.Y + 0);
                    Functions_Component.Align(Obj);

                    Obj.compSprite.zOffset = 16; //sort pet over hero
                    Obj.inWater = false; //pet isn't in water while on wall
                }
                else if(Pool.hero.state == ActorState.Landed)
                {   //prevent pet from falling down wall when hero 'lands' at top
                    Functions_Movement.Teleport(Obj.compMove,
                        Pool.hero.compSprite.position.X,
                        //move north until no longer overlapping
                        Pool.hero.compSprite.position.Y -= 1); 
                    Functions_Component.Align(Obj);
                    //sort pet under hero (on ground)
                    Obj.compSprite.zOffset = -8;
                }
                else
                {
                    //pet is free to roam around the game world (not in hero's backpack)

                    //track to the hero, within radius - get distance to hero
                    xDistance = (int)Math.Abs(Pool.hero.compSprite.position.X - Obj.compSprite.position.X);
                    yDistance = (int)Math.Abs(Pool.hero.compSprite.position.Y - Obj.compSprite.position.Y);

                    //check if pet can see hero
                    if (yDistance < 16 * 5 & xDistance < 16 * 5)
                    {   //if distance is less than rest radius, rest 
                        if (yDistance < 24 & xDistance < 24)
                        { } //do nothing, pet is close enough to hero to rest
                        else
                        {   //move diagonally towards hero
                            Functions_Movement.Push(Obj.compMove,
                                Functions_Direction.GetDiagonalToHero(Obj.compSprite.position),
                                0.4f);
                        }
                    }
                    //pet cannot see hero..
                    else
                    {   //randomly push the pet in a direction
                        if (Functions_Random.Int(0, 101) > 80)
                        {
                            Functions_Movement.Push(Obj.compMove,
                                Functions_Direction.GetRandomDirection(), 1.0f);
                        }
                    }
                }

                //set the facing direction based on X magnitude
                if (Obj.compMove.magnitude.X < 0) //moving left
                { Obj.compSprite.flipHorizontally = true; }
                else { Obj.compSprite.flipHorizontally = false; } //moving right 


                //set the animation frame, based on a number of factors

                //climbing
                if (Pool.hero.state == ActorState.Climbing)
                { Obj.compAnim.currentAnimation = AnimationFrames.Pet_Dog_Climbing; }
                //swimming
                else if (Obj.inWater)
                { Obj.compAnim.currentAnimation = AnimationFrames.Pet_Dog_InWater; }
                //moving
                else if (Math.Abs(Obj.compMove.magnitude.X) > 0 || Math.Abs(Obj.compMove.magnitude.Y) > 0)
                { Obj.compAnim.currentAnimation = AnimationFrames.Pet_Dog_Move; }
                //not moving - idle
                else { Obj.compAnim.currentAnimation = AnimationFrames.Pet_Dog_Idle; }

                //play the pet's sound fx occasionally
                if (Functions_Random.Int(0, 1001) > 995) { Assets.Play(Assets.sfxPetDog); }
            }

            #endregion


            #region Pet - Chicken

            else if(Obj.type == ObjType.Pet_Chicken)
            {
                //randomly push the pet in a direction
                if (Functions_Random.Int(0, 101) > 98)
                {
                    Functions_Movement.Push(Obj.compMove,
                        Functions_Direction.GetRandomDirection(), 
                        2.0f);
                }

                //set the facing direction based on X magnitude
                if (Obj.compMove.magnitude.X < 0) //moving left
                { Obj.compSprite.flipHorizontally = true; }
                else { Obj.compSprite.flipHorizontally = false; } //moving right

                //set moving or idle anim frame
                if (Math.Abs(Obj.compMove.magnitude.X) > 0 || Math.Abs(Obj.compMove.magnitude.Y) > 0)
                { Obj.compAnim.currentAnimation = AnimationFrames.Pet_Chicken_Move; }
                else { Obj.compAnim.currentAnimation = AnimationFrames.Pet_Chicken_Idle; }

                //rarely play the pet's sound fx 
                if (Functions_Random.Int(0, 101) > 99) { Assets.Play(Assets.sfxPetChicken); }
            }

            #endregion

            


            //very special objects

            #region ExplodingObject

            else if (Obj.type == ObjType.ExplodingObject)
            {
                Obj.lifeCounter++;
                if(Obj.lifeCounter > Obj.lifetime)
                {
                    Functions_Projectile.Spawn(
                        ProjectileType.Explosion,
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y,
                        Direction.None);
                    Functions_GameObject.Kill(Obj, true, false);
                }
            }

            #endregion




            //npcs + sidequests

            #region NPC - Farmer

            else if (Obj.type == ObjType.NPC_Farmer)
            {
                //SETUP state
                //periodically expand hitbox to check for nearby bushes
                //if a bush is nearby, obj becomes farmer reward obj
                Obj.lifeCounter++;
                if (Obj.lifeCounter == Obj.interactiveFrame)
                {   //reset timer
                    Obj.lifeCounter = 0;
                    //expand to check surrounding tiles
                    Obj.compCollision.rec.Width = 32;
                    Obj.compCollision.rec.Height = 32;
                    Obj.compCollision.rec.X = (int)Obj.compSprite.position.X - 16;
                    Obj.compCollision.rec.Y = (int)Obj.compSprite.position.Y - 16;
                    //loop over all active roomObjs, locate any nearby bush
                    for (i = 0; i < Pool.roomObjCount; i++)
                    {
                        if (Pool.roomObjPool[i].active &
                            Pool.roomObjPool[i].type == ObjType.Wor_Bush)
                        {   //if farmer touches neighboring bush, convert farmer to reward state
                            if (Pool.roomObjPool[i].compCollision.rec.Intersects(Obj.compCollision.rec))
                            {
                                Assets.Play(Assets.sfxKeyPickup); //audibly cue player
                                Functions_GameObject.SetType(Obj, ObjType.NPC_Farmer_Reward);
                                return; //bail, no need to continue
                            }
                        }
                    }
                    //if this section of code is reached, no bush was found, reset hitbox/obj
                    Functions_GameObject.SetType(Obj, ObjType.NPC_Farmer);
                }
            }
            else if(Obj.type == ObjType.NPC_Farmer_Reward)
            {
                //REWARD state
                //periodically create an exclamation particle
                Obj.lifeCounter++;
                if (Obj.lifeCounter == Obj.interactiveFrame)
                {   //reset timer
                    Obj.lifeCounter = 0;
                    Functions_Particle.Spawn(
                        ParticleType.ExclamationBubble,
                        Obj.compSprite.position.X - 3,
                        Obj.compSprite.position.Y - 16);
                }
            }

            #endregion


            #region Brandy, Ship's Captain

            else if (Obj.type == ObjType.Wor_Boat_Captain_Brandy)
            {   //she only transitions into a special animation if she's standing idle
                if (Obj.compAnim.currentAnimation == AnimationFrames.Wor_Boat_Captain_Brandy)
                {   //randomly play hands up or blink animation
                    if (Functions_Random.Int(0, 1001) > 990)
                    {
                        Obj.compAnim.index = 0;
                        if (Functions_Random.Int(0, 101) > 50)
                        { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Captain_Brandy_Blink; }
                        else
                        { Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Captain_Brandy_HandsUp; }
                    }
                }
                else if(Obj.compAnim.index == Obj.compAnim.currentAnimation.Count)
                {   //once we hit the end of the special animation, return to idle
                    Obj.compAnim.index = 0;
                    Obj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Captain_Brandy;
                }
            }

            #endregion





        }

    }
}