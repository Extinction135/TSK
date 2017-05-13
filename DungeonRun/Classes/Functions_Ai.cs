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

        public static Direction directionToHero;
        public static Vector2 actorPos;
        public static Vector2 heroPos;
        static Actor Actor;



        public static void SetActorInput()
        {
            //increment the active actor
            Pool.activeActor++;
            if (Pool.activeActor >= Pool.actorCount) { Pool.activeActor = 1; } //skip 0th actor (HERO)
            //target the active actor from the actor's pool
            Actor = Pool.actorPool[Pool.activeActor];
            //if this actor isn't active, don't pass AI to it
            if (Actor.active == false) { return; }

            //reset the target actor's input
            Input.ResetInputData(Actor.compInput);
            //reset the direction to hero
            directionToHero = Direction.None;
            //collect the actor and hero sprite positions
            actorPos = Actor.compSprite.position;
            heroPos = Pool.hero.compSprite.position;
            //get the x and y distances between the actor and hero
            int xDistance = (int)Math.Abs(heroPos.X - actorPos.X);
            int yDistance = (int)Math.Abs(heroPos.Y - actorPos.Y);


            #region Boss AI

            if (Actor.type == ActorType.Boss)
            {
                //randomly move in a direction + dash
                Actor.compInput.direction = (Direction)GetRandom.Int(0, 8);
                if (GetRandom.Int(0, 100) > 80) { Actor.compInput.dash = true; }
                //randomly spawn a blob mob at boss location
                if (GetRandom.Int(0, 100) > 50)
                {
                    Actor actorRef = PoolFunctions.GetActor();
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
                //determine the axis hero is closest on
                if (xDistance < yDistance)
                {   //hero is closer on xAxis, actor should move on yAxis
                    if (heroPos.Y > actorPos.Y)
                    { directionToHero = Direction.Down; }
                    else { directionToHero = Direction.Up; }
                }
                else
                {   //hero is closer on yAxis, actor should move on xAxis
                    if (heroPos.X > actorPos.X)
                    { directionToHero = Direction.Right; }
                    else { directionToHero = Direction.Left; }
                }

                //determine if actor is close enough to hero to chase
                int chaseRadius = 16 * 6;
                if (yDistance < chaseRadius && xDistance < chaseRadius)
                {   //actor is close enough to hero to chase, move towards the hero
                    Actor.compInput.direction = directionToHero;
                }
                else
                {   //choose a random direction to move in
                    Actor.compInput.direction = (Direction)GetRandom.Int(0, 8);
                }

                //determine if actor is close enough to hero to attack
                int attackRadius = 14;
                if (yDistance < attackRadius && xDistance < attackRadius)
                {   //actor is close enough to hero to attack
                    if (GetRandom.Int(0, 100) > 50) //randomly attack
                    { Actor.compInput.attack = true; }
                }

                //determine if the actor can dash
                if (!Actor.compInput.attack)
                {   //if the actor isn't attacking, then randomly dash
                    if (GetRandom.Int(0, 100) > 90)
                    { Actor.compInput.dash = true; }
                }

                //handle the state where the hero is dead
                if (Pool.hero.state == ActorState.Dead)
                {   //reset AI input, randomly move + dash
                    Input.ResetInputData(Actor.compInput);
                    Actor.compInput.direction = (Direction)GetRandom.Int(0, 8);
                    if (GetRandom.Int(0, 100) > 90) { Actor.compInput.dash = true; }
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

    }
}