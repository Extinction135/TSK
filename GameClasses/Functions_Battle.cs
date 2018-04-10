﻿using System;
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
    public static class Functions_Battle
    {
        static Direction direction;
        static byte damage;
        static float force;
        


        public static void Damage(Actor Actor, GameObject Obj)
        {   //based on the obj type, deal damage to the actor, push in a direction
            //reset the damage fields
            direction = Direction.None;
            damage = 0;
            force = 0.0f;


            #region Projectiles

            if (Obj.type == ObjType.ProjectileArrow)
            {   //arrows deal 2 damage, push 4, and die
                damage = 2; force = 4.0f; direction = Obj.direction;
                Obj.lifeCounter = Obj.lifetime;
            }
            else if (Obj.type == ObjType.ProjectileBomb)
            {   //bombs don't push or hurt actors
                return;
            }
            else if (Obj.type == ObjType.ProjectileFireball)
            {   //fireballs deal 3 damage, push 10, and die
                damage = 3; force = 10.0f; direction = Obj.direction;
                Obj.lifeCounter = Obj.lifetime;
            }
            else if (Obj.type == ObjType.ProjectileBoomerang)
            {   //boomerangs deal 0 damage, push 10, flip to return state
                damage = 0; force = 10.0f; direction = Obj.direction;
                Obj.lifeCounter = 200; //return to caster
            }
            else if (Obj.type == ObjType.ProjectileExplosion)
            {   //explosions deal 3 damage, push 10
                damage = 3; force = 10.0f;
                //push actor in their opposite direction
                direction = Functions_Direction.GetOppositeDirection(Actor.direction);
            }
            else if (Obj.type == ObjType.ProjectileSword)
            {   //swords deal 1 damage, push 6
                damage = 1; force = 6.0f; direction = Obj.direction;
            }
            else if(Obj.type == ObjType.ProjectileNet)
            {   //net deals 0 damage, push 6
                damage = 0; force = 6.0f; direction = Obj.direction;
            }
            else if (Obj.type == ObjType.ProjectileBow)
            {   //bows don't push or hurt actors
                return;
            }

            #endregion


            #region Objects

            else if(Obj.type == ObjType.Dungeon_SpikesFloorOn)
            {   //med push actors away from spikes
                damage = 1; force = 7.5f;
                direction = Functions_Direction.GetOppositeCardinal(
                    Actor.compSprite.position, Obj.compSprite.position);
            }
            else if (Obj.type == ObjType.Dungeon_BlockSpike)
            {   //med push actor away from spikes
                damage = 1; force = 7.5f;
                direction = Functions_Direction.GetOppositeDiagonal(
                    Actor.compSprite.position, Obj.compSprite.position);
            }

            #endregion


            //if damage was prevented, then do not damage/push the actor
            Damage(Actor, damage, force, direction);
        }

        public static void Damage(Actor Actor, byte Damage, float Force, Direction Direction)
        {   //only damage/hit/push actors not in the hit state
            if (Actor.state != ActorState.Hit)
            {   //set actor into hit state, push actor the projectile's direction
                Functions_Movement.Push(Actor.compMove, Direction, Force); //sets magnitude only
                Actor.direction = Direction; //actor's facing direction becomes direction pushed
                Functions_Actor.SetHitState(Actor);

                //check invincibility boolean
                if (Actor == Pool.hero & Flags.Invincibility) { return; }
                //prevent damage byte from underflowing the Actor.health byte
                if (Damage > Actor.health) { Actor.health = 0; }
                else { Actor.health -= Damage; }
                //if projectile damaged hero, track the damage dealt
                if (Actor == Pool.hero) { DungeonRecord.totalDamage += Damage; }
            }
        }

    }
}