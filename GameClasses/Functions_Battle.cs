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
    public static class Functions_Battle
    {
        static Direction direction;
        static byte damage;
        static float force;
        


        public static void Damage(Actor Actor, GameObject Obj)
        {
            //bail from method if actor is underwater
            if (Actor.underwater) { return; }


            //based on the obj type, deal damage to the actor, push in a direction

            //0. Reset damage fields
            direction = Direction.None;
            damage = 0;
            force = 0.0f;


            //1. setup projectiles & objects (set damage, force, direction)

            #region Projectiles


            //power level 0 projectiles
            if (Obj.type == ObjType.ProjectileBomb)
            {   //bombs don't push or hurt actors
                return;
            }
            else if (Obj.type == ObjType.ProjectileFireball)
            {   //fireballs die creating explosions
                Obj.lifeCounter = Obj.lifetime;
                return;
            }
            else if (Obj.type == ObjType.ProjectileBoomerang)
            {   //boomerangs deal 0 damage, push 10, flip to return state
                damage = 0; force = 10.0f; direction = Obj.compMove.direction;
                Obj.lifeCounter = 200; //return to caster
            }
            else if (Obj.type == ObjType.ProjectileNet)
            {   //net deals 0 damage, push 6
                damage = 0; force = 6.0f; direction = Obj.direction;
            }


            //power level 1 projectiles
            else if(Obj.type == ObjType.ProjectileArrow)
            {   //arrows deal 1 damage, push 4, and die
                damage = 1; force = 4.0f; direction = Obj.compMove.direction;
                Obj.lifeCounter = Obj.lifetime;
            }
            else if (Obj.type == ObjType.ProjectileSword)
            {   //swords deal 1 damage, push 6
                damage = 1; force = 6.0f; direction = Obj.direction;
            }
            else if (Obj.type == ObjType.ProjectileShovel)
            {   //matches swords damage, less push
                damage = 1; force = 3.0f; direction = Obj.direction;
            }
            else if (Obj.type == ObjType.ProjectileBush
                || Obj.type == ObjType.ProjectilePot
                || Obj.type == ObjType.ProjectilePotSkull)
            {   //thrown objs deal 1 damage, push 4
                damage = 1; force = 4.0f; direction = Obj.compMove.direction;
            }
            else if (Obj.type == ObjType.ProjectileBite)
            {   //bite deals 1 damage, push 5
                damage = 1; force = 5.0f; direction = Obj.direction;
            }
            else if (Obj.type == ObjType.ProjectileBat)
            {
                //bats deal 1 damage, push 4, and die
                damage = 1; force = 4.0f; direction = Obj.compMove.direction;
                Obj.lifeCounter = Obj.lifetime;
            }


            //power level 2 projectiles
            else if (Obj.type == ObjType.ProjectileExplosion)
            {   //explosions deal 2 damage, push 10
                damage = 2; force = 10.0f;
                //push actor in their opposite direction
                direction = Functions_Direction.GetOppositeDirection(Actor.direction);
            }
            else if (Obj.type == ObjType.ProjectileLightningBolt)
            {   //match explosions attributes
                damage = 2; force = 10.0f;
                //push actor in their opposite direction
                direction = Functions_Direction.GetOppositeDirection(Actor.direction);
            }

            #endregion


            #region Objects

            //dungeon objs are always power level 1 or 0 
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


            //2. ENEMY INVINCIBILITY - check actor v pro/obj status effects 

            #region Standard Enemy - INVINCIBILITIES

            if (Actor.type == ActorType.Standard_BeefyBat)
            {   //prevent friendly fire between bats attacking in groups
                if (Obj.type == ObjType.ProjectileBite) { return; } //bail
            }

            #endregion


            #region MiniBosses - INVINCIBILITIES

            else if (Actor.type == ActorType.MiniBoss_Spider_Armored)
            {
                //armored spider resists all projectiles except a few
                if (Obj.group == ObjGroup.Projectile)
                {
                    //these projectiles ACTUALLY damage armored spider
                    if (
                        Obj.type == ObjType.ProjectileExplosion
                        || Obj.type == ObjType.ProjectileLightningBolt
                        || Obj.type == ObjType.ProjectileShovel
                        )
                    { }
                    else
                    {   //all others create 'clink' sfx
                        damage = 0; //deal no damage
                        force = 3.0f; //& push spider very little
                    }
                }
                //all other damaging objects are ignored
                else { return; }
            }

            #endregion


            #region Bosses - INVINCIBILITIES

            else if (Actor.type == ActorType.Boss_BigBat)
            {
                //bat projectiles cant deal damage to the bat boss
                //(he spawns them, and it would be cheap)
                if (Obj.type == ObjType.ProjectileBat)
                { Obj.lifeCounter = 0; return; } //keep bat alive, bail
            }

            #endregion


            //3. pass completed damage & force
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

                //put underwater enemies into their underwater state (so player can't spam attack them)
                if (Actor.underwaterEnemy)
                {
                    Actor.underwater = true;
                    Functions_Particle.Spawn(
                        ObjType.Particle_Splash,
                        Actor.compSprite.position.X,
                        Actor.compSprite.position.Y);
                }

            }
        }

    }
}