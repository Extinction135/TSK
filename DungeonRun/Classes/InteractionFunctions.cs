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
    public static class InteractionFunctions
    {
        public static byte damage;
        public static float force;



        public static void Handle(Actor Actor, GameObject Obj)
        {
            //Obj is non-blocking
            if(Actor == Pool.hero)
            {
                if (Obj.type == GameObject.Type.DoorBoss)
                {
                    if (DungeonFunctions.dungeon.bigKey)
                    {
                        GameObjectFunctions.SetType(Obj, GameObject.Type.DoorOpen);
                    }
                }
            }
        }




        public static void Handle(GameObject Projectile, Actor Actor)
        {
            //no blocking checks have been done yet, actors always block tho
            damage = 0; //reset the damage value
            force = 0.0f; //reset the force amount (how much actor is pushed)


            #region Determine Damage and Force based on Projectile Type

            if (Projectile.type == GameObject.Type.ProjectileSword)
            {
                damage = 1;
                force = 8.0f;
                //create a hit particle
                //swords always complete their animation
            }
            //else if projectile is fireball
            //deal more damage
            //create explosion particle
            //Obj.lifeCounter = Obj.lifetime; //end the projectiles life

            #endregion

            
            //only damage/hit/push actors not in the hit state
            if (Actor.state != Actor.State.Hit)
            {   //deal damage to the actor
                //but prevent the damage byte from underflowing the Actor.health byte
                if (damage > Actor.health) { Actor.health = 0; }
                else { Actor.health -= damage; }

                //if projectile damaged hero, track the damage dealt
                if (Actor == Pool.hero) { DungeonRecord.totalDamage += damage; }

                //set actor into hit state, push actor the projectile's direction
                ActorFunctions.SetHitState(Actor);
                MovementFunctions.Push(Actor.compMove, Projectile.direction, force);
            }
        }

        public static void Handle(GameObject Projectile, GameObject Obj)
        {
            //Obj could be a projectile!
            //no blocking checks have been done yet

            //if a projectile collides with something, the projectile may get destroyed (end of lifetime)
            //Obj.lifeCounter = Obj.lifetime; //end the projectiles life
            //create an explosion effect here
        }

    }
}