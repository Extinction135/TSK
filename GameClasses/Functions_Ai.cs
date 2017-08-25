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


            #region Boss AI

            if (Actor.type == ActorType.Boss)
            {
                //randomly move in a direction + dash
                Actor.compInput.direction = (Direction)Functions_Random.Int(0, 8);
                if (Functions_Random.Int(0, 100) > 80) { Actor.compInput.dash = true; }
                //randomly spawn a blob mob at boss location
                if (Functions_Random.Int(0, 100) > 50)
                {
                    Actor actorRef = Functions_Pool.GetActor();
                    if(actorRef != null)
                    {   //actorRef can never be an actor already active in room
                        Functions_Actor.SetType(actorRef, ActorType.Blob);
                        Functions_Movement.Teleport(actorRef.compMove, actorPos.X, actorPos.Y);
                    }
                }
            }

            #endregion


            #region Blob AI

            else //blob
            {
                //determine if actor is close enough to hero to chase
                int chaseRadius = 16 * 6;
                if (yDistance < chaseRadius && xDistance < chaseRadius)
                {   //actor is close enough to hero to chase, move towards the hero
                    Actor.compInput.direction = Functions_Direction.GetDirectionToHero(actorPos);
                }
                else
                {   //choose a random direction to move in
                    Actor.compInput.direction = (Direction)Functions_Random.Int(0, 8);
                }

                //determine if actor is close enough to hero to attack
                int attackRadius = 14;
                if (yDistance < attackRadius && xDistance < attackRadius)
                {   //actor is close enough to hero to attack
                    if (Functions_Random.Int(0, 100) > 50) //randomly attack
                    { Actor.compInput.attack = true; }
                }

                //determine if the actor can dash
                if (!Actor.compInput.attack)
                {   //if the actor isn't attacking, then randomly dash
                    if (Functions_Random.Int(0, 100) > 90)
                    { Actor.compInput.dash = true; }
                }

                //handle the state where the hero is dead
                if (Pool.hero.state == ActorState.Dead)
                {   //reset AI input, randomly move + dash
                    Functions_Input.ResetInputData(Actor.compInput);
                    Actor.compInput.direction = (Direction)Functions_Random.Int(0, 8);
                    if (Functions_Random.Int(0, 100) > 90) { Actor.compInput.dash = true; }
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

        public static void HandleObj(GameObject Obj)
        {   //keep in mind this is called every frame
            if (Obj.type == ObjType.Flamethrower)
            {   
                if (Functions_Random.Int(0, 500) > 497) //aggressively shoots
                { Functions_Entity.SpawnEntity(ObjType.ProjectileFireball, Obj); }
            }
            else if (Obj.type == ObjType.WallStatue)
            {
                if (Functions_Random.Int(0, 2000) > 1998) //rarely shoots
                { Functions_Entity.SpawnEntity(ObjType.ProjectileArrow, Obj); }
            }
            else if(Obj.type == ObjType.PitAnimated)
            {
                if (Functions_Random.Int(0, 500) > 495) //occasionally bubbles
                { Functions_Entity.SpawnEntity(ObjType.ParticleBubble, Obj); }
            }
        }

    }
}