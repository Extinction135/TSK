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
            {   //arrows deal 1 damage, push 4, and die
                damage = 1; force = 4.0f; direction = Obj.direction;
                Obj.lifeCounter = Obj.lifetime;
                //check to see if damage should be modified
                if (Actor.armor == MenuItemType.ArmorChest)
                { damage = 0; Assets.Play(Assets.sfxTapMetallic); }
            }
            else if (Obj.type == ObjType.ProjectileBomb)
            {   //bombs don't push or hurt actors
                return;
            }
            else if (Obj.type == ObjType.ProjectileFireball)
            {   //fireballs deal 2 damage, push 10, and die
                damage = 2; force = 10.0f; direction = Obj.direction;
                Obj.lifeCounter = Obj.lifetime;
                if (Actor.armor == MenuItemType.ArmorCloth) { damage = 1; }
            }
            else if (Obj.type == ObjType.ProjectileExplosion)
            {   //explosions deal 2 damage, push 10
                damage = 2; force = 10.0f; direction = Obj.direction;
                //check to see if damage should be modified
                if (Actor.armor == MenuItemType.ArmorChest) { damage = 1; }
            }
            else if (Obj.type == ObjType.ProjectileSpikeBlock)
            {   //hard push actor away from spikes
                damage = 1; force = 10.0f;
                direction = Functions_Direction.GetRelativeDirection(Obj.compSprite.position, Actor.compSprite.position);
            }
            else if (Obj.type == ObjType.ProjectileSword)
            {   //swords deal 1 damage, push 6
                damage = 1; force = 6.0f; direction = Obj.direction;
            }

            #endregion


            #region Objects

            else if(Obj.type == ObjType.SpikesFloorOn)
            {   //medium push actor in the actor's moving direction
                damage = 1; force = 5.0f;
                //direction = Actor.direction;
                direction = Functions_Direction.GetRelativeDirection(Obj.compSprite.position, Actor.compSprite.position);
            }

            #endregion


            //if damage was prevented, then do not damage/push the actor
            if (damage > 0) { Damage(Actor, damage, force, direction); }
        }

        public static void Damage(Actor Actor, byte Damage, float Force, Direction Direction)
        {   //only damage/hit/push actors not in the hit state
            if (Actor.state != ActorState.Hit)
            {   //set actor into hit state, push actor the projectile's direction
                Functions_Movement.Push(Actor.compMove, Direction, Force); //sets magnitude only
                Actor.direction = Direction; //actor's facing direction becomes direction pushed
                Functions_Actor.SetHitState(Actor);

                //check invincibility boolean
                if (Actor == Pool.hero) { if (Flags.Invincibility) { return; } }
                //prevent damage byte from underflowing the Actor.health byte
                if (Damage > Actor.health) { Actor.health = 0; }
                else { Actor.health -= Damage; }
                //if projectile damaged hero, track the damage dealt
                if (Actor == Pool.hero) { DungeonRecord.totalDamage += Damage; }
            }
        }

    }
}