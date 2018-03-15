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
        {   //keep in mind this is called every frame
            if (Obj.type == ObjType.Flamethrower)
            {   
                if (Functions_Random.Int(0, 500) > 497) //aggressively shoots
                { Functions_Projectile.Spawn(ObjType.ProjectileFireball, Obj); }
            }
            else if (Obj.type == ObjType.WallStatue)
            {
                if (Functions_Random.Int(0, 2000) > 1998) //rarely shoots
                { Functions_Projectile.Spawn(ObjType.ProjectileArrow, Obj); }
            }
            else if(Obj.type == ObjType.PitAnimated)
            {
                if (Functions_Random.Int(0, 2000) > 1997) //occasionally bubbles
                { Functions_Particle.Spawn(ObjType.ParticlePitAnimation, Obj); }
            }
        }

    }
}