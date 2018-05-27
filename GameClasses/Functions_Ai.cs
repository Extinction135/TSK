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
            //get actor sprite position
            actorPos = Actor.compSprite.position;
            //get the x & y distances between actor and hero
            xDistance = (int)Math.Abs(Pool.hero.compSprite.position.X - actorPos.X);
            yDistance = (int)Math.Abs(Pool.hero.compSprite.position.Y - actorPos.Y);


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
            {   //by default, choose a random direction to move in & randomly dash
                Actor.compInput.direction = (Direction)Functions_Random.Int(0, 8);
                if (Functions_Random.Int(0, 100) > 90) { Actor.compInput.dash = true; }

                //if the hero's underwater (hidden), just bail from rest of method
                if (Pool.hero.underwater) { return; }
                //if hero's dead, bail
                if (Pool.hero.state == ActorState.Dead) { return; }

                //if the actor is an enemy, and the hero is on other team (ally)
                if (Actor.enemy & Pool.hero.enemy == false)
                {   
                    ChaseHero(); //this method will chase hero using diagonal movement only
                    //determine if actor is close enough to attack hero
                    if (yDistance < Actor.attackRadius && xDistance < Actor.attackRadius)
                    {   //actor is close enough to hero to attack
                        if (Functions_Random.Int(0, 100) > 50) //randomly proceed
                        {
                            //enemies vary their attacks based on type

                            #region Enemies that attack with weapons / items

                            if(Actor.type == ActorType.Blob)
                            {
                                //set the cardinal direction towards hero, attack, cancel any dash
                                Actor.compInput.direction = 
                                    Functions_Direction.GetCardinalDirectionToHero(actorPos);
                                Actor.compInput.attack = true;
                                Actor.compInput.dash = false;
                            }

                            #endregion


                            #region Enemies that explode when near target

                            else if(Actor.type == ActorType.Boss_BigEye_Mob)
                            {
                                Functions_Actor.SetDeathState(Actor);
                            }

                            #endregion

                        }
                    }
                }
                else if(Actor.enemy == false)
                {   //if actor is an ally, then chase the hero
                    ChaseHero();
                    //determine if actor is close enough to stop chasing hero
                    if (yDistance < Actor.attackRadius && xDistance < Actor.attackRadius)
                    {   
                        Actor.compInput.dash = false;
                        Actor.compInput.direction = Direction.None;
                    }
                }
                
            }

            #endregion


            //cooler ai

            #region Miniboss AI

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
                    {   //shoot fireballs at hero
                        Functions_Projectile.Spawn(ObjType.ProjectileFireball,
                            Actor.compMove,
                            Functions_Direction.GetCardinalDirectionToHero(Actor.compSprite.position));
                        Actor.compInput.attack = true;
                    }
                }
                else //actor is at low health
                {
                    //move very fast
                    Actor.walkSpeed = 0.20f;
                    Actor.dashSpeed = 0.50f;

                    //smoke (each frame) as a sign of nearing defeat
                    Functions_Particle.Spawn(
                        ObjType.Particle_ImpactDust,
                        Actor.compSprite.position.X + 6 + Functions_Random.Int(-8, 8),
                        Actor.compSprite.position.Y - 10 + Functions_Random.Int(-5, 5)
                    );

                    if (Functions_Random.Int(0, 100) > 75)
                    {   //shoot fireballs at hero more often
                        Functions_Projectile.Spawn(ObjType.ProjectileFireball,
                            Actor.compMove,
                            Functions_Direction.GetCardinalDirectionToHero(Actor.compSprite.position));
                        Actor.compInput.attack = true;
                    }

                    //rarely taunt the player
                    if (Functions_Random.Int(0, 100) > 95)
                    {
                        Assets.Play(Assets.sfxEnemyTaunt);
                        Functions_Particle.Spawn(
                            ObjType.Particle_Push,
                            Actor.compSprite.position.X,
                            Actor.compSprite.position.Y,
                            Direction.Down);
                    }
                }
            }

            #endregion


            #region Boss AI

            else if (Actor.aiType == ActorAI.Boss_BigEye)
            {
                //by default, choose a random direction to move in & randomly dash
                Actor.compInput.direction = (Direction)Functions_Random.Int(0, 8);
                if (Functions_Random.Int(0, 100) > 70) { Actor.compInput.dash = true; }


                //3 Phase - based on health, change how actor behaves
                if(Actor.health > 20) 
                {
                    //move slowly
                    Actor.walkSpeed = 0.05f;
                    Actor.dashSpeed = 0.30f;

                    if (Functions_Random.Int(0, 100) > 90)
                    {   //rarely spawn mob
                        Functions_Actor.SpawnActor(ActorType.Boss_BigEye_Mob, actorPos);
                        Actor.compInput.attack = true;
                    }
                }
                else if(Actor.health > 10) 
                {
                    //move faster
                    Actor.walkSpeed = 0.25f;
                    Actor.dashSpeed = 0.30f;

                    if (Functions_Random.Int(0, 100) > 70)
                    {   //regularly spawn mob
                        Functions_Actor.SpawnActor(ActorType.Boss_BigEye_Mob, actorPos);
                        Actor.compInput.attack = true;
                    }
                }
                else //actor is below 10 health
                {
                    //dash faster too
                    Actor.walkSpeed = 0.25f;
                    Actor.dashSpeed = 0.80f;
                    
                    if (Functions_Random.Int(0, 100) > 40)
                    {   //often spawn mob
                        Functions_Actor.SpawnActor(ActorType.Boss_BigEye_Mob, actorPos);
                        Actor.compInput.attack = true;
                    }

                    //smoke (each frame) as a sign of nearing defeat
                    Functions_Particle.Spawn(
                        ObjType.Particle_ImpactDust,
                        Actor.compSprite.position.X + 6 + Functions_Random.Int(-8, 8),
                        Actor.compSprite.position.Y - 10 + Functions_Random.Int(-5, 5)
                    );

                    //rarely taunt the player
                    if (Functions_Random.Int(0, 100) > 95)
                    {
                        Assets.Play(Assets.sfxEnemyTaunt);
                        Functions_Particle.Spawn(
                            ObjType.Particle_Push,
                            Actor.compSprite.position.X, 
                            Actor.compSprite.position.Y, 
                            Direction.Down);
                    }
                }
            }

            #endregion

        }

        public static void ChaseHero()
        {   //actor is close enough to chase hero, set actor's direction to hero
            if (yDistance < Actor.chaseRadius && xDistance < Actor.chaseRadius)
            { Actor.compInput.direction = Functions_Direction.GetDiagonalToHero(actorPos); }
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



            //Obj.type checks

            #region Dungeon Objects

            if (Obj.type == ObjType.Dungeon_Flamethrower)
            {
                if (Functions_Random.Int(0, 500) > 497) //aggressively shoots
                {   //shoot fireball towards hero along a cardinal direction
                    Functions_Projectile.Spawn(
                        ObjType.ProjectileFireball, Obj.compMove,
                        Functions_Direction.GetCardinalDirectionToHero(Obj.compSprite.position));
                }
            }
            else if (Obj.type == ObjType.Dungeon_WallStatue)
            {
                if (Functions_Random.Int(0, 2000) > 1998) //rarely shoots
                { Functions_Projectile.Spawn(ObjType.ProjectileArrow, Obj.compMove, Obj.direction); }
            }
            else if (Obj.type == ObjType.Dungeon_Pit)
            {
                if (Functions_Random.Int(0, 2000) > 1997) //occasionally bubbles
                { Functions_Particle.Spawn(ObjType.Particle_PitBubble, Obj); }
            }

            else if(Obj.type == ObjType.Dungeon_Switch || Obj.type == ObjType.Dungeon_SwitchDown)
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
                        ObjType.Particle_Attention,
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
                        ObjType.Particle_Attention,
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y);
                    Assets.Play(Assets.sfxSwitch);
                }
            }

            #endregion


            #region World Objects
            

            //bush stump obj growing back into a bush
            else if(Obj.type == ObjType.Wor_Bush_Stump)
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

                    //bail from this check if stump touches hero
                    //we dont want to regrow a bush ontop of hero, locking him
                    if(Pool.hero.compCollision.rec.Intersects(Obj.compCollision.rec))
                    {   //reset hitBox, bail from method
                        Functions_GameObject.SetType(Obj, ObjType.Wor_Bush_Stump);
                        return;
                    }

                    //loop over all active roomObjs, looking at filled ditches
                    for (i = 0; i < Pool.roomObjCount; i++)
                    {
                        if (Pool.roomObjPool[i].active &
                            Pool.roomObjPool[i].group == ObjGroup.Ditch &
                            Pool.roomObjPool[i].getsAI == true) //filled
                        {
                            //check collisions
                            if (Pool.roomObjPool[i].compCollision.rec.Intersects(Obj.compCollision.rec))
                            {   //regrow into bush, with a pop
                                Functions_Particle.Spawn(
                                    ObjType.Particle_Attention,
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





            else if (Obj.type == ObjType.Wor_Tree_Burning)
            {   //check to see if tree should still burn
                if (Obj.lifeCounter < Obj.lifetime)
                {
                    Obj.lifeCounter++;
                    if (Obj.lifeCounter < 75) //place fires only at beginning
                    {
                        if (Functions_Random.Int(0, 100) > 93)
                        {   //often spawn fires on the bushy top
                            Functions_Particle.Spawn(ObjType.Particle_Fire,
                                Obj.compSprite.position.X + Functions_Random.Int(-6, 6),
                                Obj.compSprite.position.Y + Functions_Random.Int(-8, 4));
                        }
                        if (Functions_Random.Int(0, 100) > 93)
                        {   //less often spawn fires along the tree trunk
                            Functions_Particle.Spawn(ObjType.Particle_Fire,
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
                        ObjType.Particle_Attention,
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y - 2);
                    //pop leaves in circular decorative pattern for tree top
                    Functions_Particle.Spawn_Explosion(
                        ObjType.Particle_Leaf,
                        Obj.compSprite.position.X + 2,
                        Obj.compSprite.position.Y - 4, true);
                    //switch to burned tree
                    Functions_GameObject.ResetObject(Obj);
                    Functions_GameObject.SetType(Obj, ObjType.Wor_Tree_Burnt);
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
                    if(!Functions_Level.currentRoom.rec.Contains(Obj.compSprite.position))
                    {
                        Functions_Particle.Spawn(
                            ObjType.Particle_Attention,
                            Obj.compSprite.position.X + 0,
                            Obj.compSprite.position.Y + 0);
                        Functions_Pool.Release(Obj);
                    }
                }
            }

            #endregion


            #region Pet - Dog

            else if (Obj.type == ObjType.Pet_Dog)
            {
                if (Functions_Random.Int(0, 101) > 50)
                {
                    //track to the hero, within radius - get distance to hero
                    xDistance = (int)Math.Abs(Pool.hero.compSprite.position.X - Obj.compSprite.position.X);
                    yDistance = (int)Math.Abs(Pool.hero.compSprite.position.Y - Obj.compSprite.position.Y);

                    //check if pet can see hero
                    if (yDistance < 64 & xDistance < 64)
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
                    else//pet cannot see hero..
                    {   //randomly push the pet in a direction
                        if (Functions_Random.Int(0, 101) > 80)
                        {
                            Functions_Movement.Push(Obj.compMove,
                                Functions_Direction.GetRandomDirection(), 1.0f);
                        }
                    }

                    //set the facing direction based on X magnitude
                    if (Obj.compMove.magnitude.X < 0) //moving left
                    { Obj.compSprite.flipHorizontally = true; }
                    else { Obj.compSprite.flipHorizontally = false; } //moving right                                              

                    //set moving or idle anim frame
                    if (Math.Abs(Obj.compMove.magnitude.X) > 0 || Math.Abs(Obj.compMove.magnitude.Y) > 0)
                    { Obj.compAnim.currentAnimation = AnimationFrames.Pet_Dog_Move; }
                    else { Obj.compAnim.currentAnimation = AnimationFrames.Pet_Dog_Idle; }

                    //play the pet's sound fx occasionally
                    if (Functions_Random.Int(0, 101) > 99) { Assets.Play(Assets.sfxPetDog); }
                }
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

            
        }

    }
}