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

        public static ComponentCollision interactionRec = new ComponentCollision();
        //public static Rectangle interactionRec = new Rectangle(0, 0, 8, 8);




        public static void SetHeroInteractionRec()
        {
            //set the interaction rec to the hero's position
            interactionRec.rec.X = (int)Pool.hero.compSprite.position.X - 4;
            interactionRec.rec.Y = (int)Pool.hero.compSprite.position.Y - 4;
            //offset the rec based on the direction hero is facing
            if (Pool.hero.direction == Direction.Up)
            {
                interactionRec.rec.Width = 8; interactionRec.rec.Height = 4;
                interactionRec.rec.Y -= 1;
            }
            else if (Pool.hero.direction == Direction.Down)
            {
                interactionRec.rec.Width = 8; interactionRec.rec.Height = 4;
                interactionRec.rec.Y += 14;
            }
            else if (
                Pool.hero.direction == Direction.Left || 
                Pool.hero.direction == Direction.UpLeft || 
                Pool.hero.direction == Direction.DownLeft) 
            {
                interactionRec.rec.Width = 4; interactionRec.rec.Height = 8;
                interactionRec.rec.Y += 4; interactionRec.rec.X -= 7;
            }
            else if (
                Pool.hero.direction == Direction.Right || 
                Pool.hero.direction == Direction.UpRight || 
                Pool.hero.direction == Direction.DownRight)
            {
                interactionRec.rec.Width = 4; interactionRec.rec.Height = 8;
                interactionRec.rec.Y += 4; interactionRec.rec.X += 11;
            }
        }





        public static void Handle(Actor Actor, GameObject Obj)
        {
            //Obj is non-blocking


            #region Handle Hero specific object interactions

            if(Actor == Pool.hero)
            {
                if (Obj.type == GameObject.Type.DoorBoss)
                {
                    if (DungeonFunctions.dungeon.bigKey)
                    {
                        GameObjectFunctions.SetType(Obj, GameObject.Type.DoorOpen);
                        Assets.sfxDoorOpen.Play();
                        GameObjectFunctions.SpawnParticle(GameObject.Type.ParticleExplosion, Obj.compSprite.position);
                    }
                }
                else if(Obj.type == GameObject.Type.DoorTrap)
                {
                    MovementFunctions.Push(Actor.compMove, Obj.direction, 1.0f);
                }
                else if(Obj.type == GameObject.Type.ItemBigKey)
                {
                    PoolFunctions.Release(Obj);
                    DungeonFunctions.dungeon.bigKey = true;
                    Assets.sfxKeyPickup.Play();
                }
            }

            #endregion


            //non-hero actors may interact with certain objects as well

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