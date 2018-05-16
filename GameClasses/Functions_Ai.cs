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

                if (Actor.type == ActorType.Boss)
                {   //randomly spawn a blob mob at boss location
                    if (Functions_Random.Int(0, 100) > 50)
                    { Functions_Actor.SpawnActor(ActorType.Blob, actorPos); }
                }
            }

            #endregion


            #region Basic AI
            
            else if(Actor.aiType == ActorAI.Basic)
            {   //by default, choose a random direction to move in & randomly dash
                Actor.compInput.direction = (Direction)Functions_Random.Int(0, 8);
                if (Functions_Random.Int(0, 100) > 90) { Actor.compInput.dash = true; }

                //if the hero's underwater (hidden), just bail from rest of method
                if (Pool.hero.underwater) { return; }
                
                //if the hero's alive, determine if actor should chase/attack hero
                if (Pool.hero.state != ActorState.Dead)
                {   //if the actor is an enemy, and the hero is on other team (ally)
                    if (Actor.enemy & Pool.hero.enemy == false)
                    {   
                        ChaseHero(); //this method will chase hero using diagonal movement only
                        //determine if actor is close enough to attack hero
                        if (yDistance < Actor.attackRadius && xDistance < Actor.attackRadius)
                        {   //actor is close enough to hero to attack
                            if (Functions_Random.Int(0, 100) > 50) //randomly proceed
                            {   //set the cardinal direction towards hero, attack, cancel any dash
                                Actor.compInput.direction = Functions_Direction.GetCardinalDirectionToHero(actorPos); // HERE
                                Actor.compInput.attack = true;
                                Actor.compInput.dash = false;
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
            }

            #endregion


            


            //slightly more advanced AI

            //if wounded, try to heal
            //else...

            //if offensive...
            //if very close, attack/use/dash hero
            //if nearby, dash towards hero
            //if in visibility range, move towards hero
            //else, wander around

            //if defensive...
            //if very close or nearby, move away from hero
            //if in visibility range, ranged attack hero
            //else, wander around
        }

        public static void ChaseHero()
        {   //actor is close enough to chase hero, set actor's direction to hero
            if (yDistance < Actor.chaseRadius && xDistance < Actor.chaseRadius)
            { Actor.compInput.direction = Functions_Direction.GetDiagonalToHero(actorPos); }
        }

        public static void HandleObj(GameObject Obj)
        {   //keep in mind this method is called every frame


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
                        Pool.actorPool[i].compCollision.rec.Intersects(Obj.compCollision.rec))
                    { overlap = true; }
                }

                //loop over all active block roomObjs
                for (i = 0; i < Pool.roomObjCount; i++)
                {   //only blocking objs can activate switches
                    if (Pool.roomObjPool[i].active & Pool.roomObjPool[i].compCollision.blocking &
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