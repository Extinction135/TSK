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

                            //if(Actor.type == ActorType.Blob)
                            {
                                //set the cardinal direction towards hero, attack, cancel any dash
                                Actor.compInput.direction = 
                                    Functions_Direction.GetCardinalDirectionToHero(actorPos);
                                Actor.compInput.attack = true;
                                Actor.compInput.dash = false;
                            }

                            //other enemy types would choose maybe to use an item, etc..

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
                        Functions_Projectile.Spawn(ObjType.ProjectileArrow,
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
                        Functions_Projectile.Spawn(ObjType.ProjectileArrow,
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


                //2 Phase - based on health, change how actor behaves
                if(Actor.health > 10) 
                {
                    //move faster
                    Actor.walkSpeed = 0.25f;
                    Actor.dashSpeed = 0.30f;

                    if (Functions_Random.Int(0, 100) > 70)
                    {   //often spawn seeker
                        Functions_GameObject.Spawn(ObjType.Wor_SeekerExploder, 
                            actorPos.X, actorPos.Y, Direction.Down);
                        Actor.compInput.attack = true;
                    }
                }
                else //actor is below 10 health
                {
                    //dash faster too
                    Actor.walkSpeed = 0.25f;
                    Actor.dashSpeed = 0.80f;
                    
                    if (Functions_Random.Int(0, 100) > 40)
                    {   //spam spawn seeker
                        Functions_GameObject.Spawn(ObjType.Wor_SeekerExploder,
                            actorPos.X, actorPos.Y, Direction.Down);
                        Actor.compInput.attack = true;
                    }

                    //smoke (each frame) as a sign of nearing defeat/rage
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


            #region Handle RoomObj Enemies - behavior

            else if (Obj.group == ObjGroup.Enemy)
            {

                //if an enemy has gone beyond the bounds of a roomRec, release without loot
                if (!Level.currentRoom.rec.Contains(Obj.compSprite.position))
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


                #region Seeker Exploders

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
                    }
                    else
                    {   //randomly set direction seeker moves
                        if (Functions_Random.Int(0, 100) > 95)
                        { Obj.compMove.direction = Functions_Direction.GetRandomDirection(); }
                        Obj.direction = Obj.compMove.direction;
                        //seeker moves with less energy
                        Functions_Movement.Push(Obj.compMove, Obj.compMove.direction, 0.1f);
                    }

                    if (yDistance < 15 & xDistance < 15) //is seeker close enough to explode?
                    {   //instantly explode
                        Functions_Projectile.Spawn(ObjType.ProjectileExplosion,
                            Obj.compSprite.position.X,
                            Obj.compSprite.position.Y);
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

            #endregion


            #region Switches

            else if (Obj.type == ObjType.Dungeon_Switch || Obj.type == ObjType.Dungeon_SwitchDown)
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


            #region Fairy

            else if (Obj.type == ObjType.Dungeon_Fairy)
            {
                if (Functions_Random.Int(0, 101) > 93) //float around
                {   //randomly push fairy a direction
                    Functions_Movement.Push(Obj.compMove,
                        Functions_Direction.GetRandomDirection(), 1.0f);
                    //check that the fairy overlaps the current room rec,
                    //otherwise the fairy has strayed too far and must be killed
                    if (!Level.currentRoom.rec.Contains(Obj.compSprite.position))
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
                        ObjType.Particle_Attention, 
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y);
                    Functions_Particle.Spawn_Explosion(
                        ObjType.Particle_Debris,
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
                            ObjType.Particle_RisingSmoke,
                            Obj.compSprite.position.X + 4 + Functions_Random.Int(-3, 3),
                            Obj.compSprite.position.Y - 2 + Functions_Random.Int(-5, 3));
                    }
                }
            }

            #endregion


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
                        ObjType.Particle_ExclamationBubble,
                        Obj.compSprite.position.X - 3,
                        Obj.compSprite.position.Y - 16);
                    }
                    else if(Obj.interactiveFrame < 50)
                    {
                        //#2
                        Functions_Particle.Spawn(
                        ObjType.Particle_ExclamationBubble,
                        Obj.compSprite.position.X - 3 + 16 * 1,
                        Obj.compSprite.position.Y - 16);
                    }
                    else if (Obj.interactiveFrame < 75)
                    {
                        //#3
                        Functions_Particle.Spawn(
                        ObjType.Particle_ExclamationBubble,
                        Obj.compSprite.position.X - 3 + 16 * 2,
                        Obj.compSprite.position.Y - 16);
                    }
                    else 
                    {
                        //#4
                        Functions_Particle.Spawn(
                        ObjType.Particle_ExclamationBubble,
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

                    if (Functions_Colliseum.currentChallenge == Challenges.Blobs)
                    {   //reward hero with gold
                        PlayerData.current.gold += 25;
                    }
                    else if(Functions_Colliseum.currentChallenge == Challenges.Minibosses)
                    {   //reward hero with gold
                        PlayerData.current.gold += 99;
                    }
                    else if (Functions_Colliseum.currentChallenge == Challenges.Bosses)
                    {   //reward hero with gold
                        PlayerData.current.gold += 99;
                    }

                    #endregion


                    // <<< exit condition : completed >>>
                    //exit level to pit level
                    Level.ID = LevelID.ColliseumPit;
                    Functions_Level.CloseLevel(ExitAction.Level);

                    //pop a new dialog screen telling player they completed challenge
                    ScreenManager.AddScreen(new ScreenDialog(AssetsDialog.Colliseum_Challenge_Complete));
                    Assets.Play(Assets.sfxGoldSpam); //audibly cue player they were rewarded
                    Assets.Play(Assets.sfxKeyPickup); //oh no, too spicy!
                }
            }


            #endregion




            //pets

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

                    //play the pet's sound fx occasionally
                    if (Functions_Random.Int(0, 101) > 99) { Assets.Play(Assets.sfxPetDog); }

                    //set animFrame, based on movement and inWater boolean
                    //if dogs in water, set to water animation, else set to moving anim, else idle anim
                    if (Obj.inWater) { Obj.compAnim.currentAnimation = AnimationFrames.Pet_Dog_InWater; }
                    else if (Math.Abs(Obj.compMove.magnitude.X) > 0 || Math.Abs(Obj.compMove.magnitude.Y) > 0)
                    { Obj.compAnim.currentAnimation = AnimationFrames.Pet_Dog_Move; }
                    else { Obj.compAnim.currentAnimation = AnimationFrames.Pet_Dog_Idle; }
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

            


            //very special objects

            #region ExplodingObject

            else if (Obj.type == ObjType.ExplodingObject)
            {
                Obj.lifeCounter++;
                if(Obj.lifeCounter > Obj.lifetime)
                {
                    Functions_Projectile.Spawn(
                        ObjType.ProjectileExplosion,
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y);
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
                        ObjType.Particle_ExclamationBubble,
                        Obj.compSprite.position.X - 3,
                        Obj.compSprite.position.Y - 16);
                }
            }

            #endregion



        }

    }
}