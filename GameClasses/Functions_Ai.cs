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
    public static class Functions_Ai
    {
        static Actor Actor;
        static Vector2 actorPos;
        static int xDistance;
        static int yDistance;


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
                        Functions_InteractiveObjs.Spawn(InteractiveType.Enemy_SeekerExploder, 
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
                        Functions_InteractiveObjs.Spawn(InteractiveType.Enemy_SeekerExploder,
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
                    {
                        Actor.compInput.use = true; //for anim purposes only, has no item
                        Functions_Projectile.Spawn(ProjectileType.Bat, Actor, Actor.compMove.direction);
                    }
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
                        Functions_MagicSpells.Cast_Bat_Explosion(Actor);
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

    }
}