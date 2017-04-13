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
                force = 1.0f;
                //create a hit particle
                //swords always complete their animation
            }
            //else if projectile is fireball
            //deal more damage
            //create explosion particle
            //Obj.lifeCounter = Obj.lifetime; //end the projectiles life

            #endregion


            //every projectile pushes an actor it collides with
            MovementFunctions.Push(Actor.compMove, Projectile.direction, force);
            //only deal damage to actors that are not in a hit state
            if (Actor.state != Actor.State.Hit) { Actor.health -= damage; }
            //set the actor into a hit state
            ActorFunctions.SetHitState(Actor);
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