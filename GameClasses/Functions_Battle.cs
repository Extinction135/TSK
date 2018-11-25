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
        



        public static void Damage(Actor Actor, Projectile Pro)
        {
            //bail from method if actor is underwater
            if (Actor.underwater) { return; }

            //0. Reset damage fields
            direction = Direction.None;
            damage = 0;
            force = 0.0f;


            //Projectiles


            #region Power level 0 projectiles

            if (Pro.type == ProjectileType.ProjectileBomb)
            {   //bombs don't push or hurt actors
                return;
            }
            else if (Pro.type == ProjectileType.ProjectileFireball)
            {   //fireballs die creating explosions
                Pro.lifeCounter = Pro.lifetime;
                return;
            }
            else if (Pro.type == ProjectileType.ProjectileBoomerang)
            {   //boomerangs deal 0 damage, push 10, flip to return state
                damage = 0; force = 10.0f; direction = Pro.compMove.direction;
                Pro.lifeCounter = 200; //return to caster
            }
            else if (Pro.type == ProjectileType.ProjectileNet)
            {   //net deals 0 damage, push 6
                damage = 0; force = 6.0f; direction = Pro.direction;
            }

            #endregion


            #region Power level 1 projectiles

            else if (Pro.type == ProjectileType.ProjectileArrow)
            {   //arrows deal 1 damage, push 4, and die
                damage = 1; force = 4.0f; direction = Pro.compMove.direction;
                Pro.lifeCounter = Pro.lifetime;
            }
            else if (Pro.type == ProjectileType.ProjectileSword)
            {   //swords deal 1 damage, push 6
                damage = 1; force = 6.0f; direction = Pro.direction;
            }
            else if (Pro.type == ProjectileType.ProjectileShovel)
            {   //matches swords damage, less push
                damage = 1; force = 3.0f; direction = Pro.direction;
            }

            else if (Pro.type == ProjectileType.ProjectileBush
                || Pro.type == ProjectileType.ProjectilePot
                || Pro.type == ProjectileType.ProjectilePotSkull)
            {   //thrown objs deal 1 damage, push 4
                damage = 1; force = 4.0f; direction = Pro.compMove.direction;
            }
            else if (Pro.type == ProjectileType.ProjectileBite)
            {   //bite deals 1 damage, push 5
                damage = 1; force = 5.0f; direction = Pro.direction;
            }
            else if (Pro.type == ProjectileType.ProjectileBat)
            {
                //bats deal 1 damage, push 4, and die
                damage = 1; force = 4.0f; direction = Pro.compMove.direction;
                Pro.lifeCounter = Pro.lifetime;
            }

            #endregion


            #region Power level 2 projectiles

            else if (Pro.type == ProjectileType.ProjectileExplosion)
            {   //explosions deal 2 damage, push 10
                damage = 2; force = 10.0f;
                //push actor in their opposite direction
                direction = Functions_Direction.GetOppositeDirection(Actor.direction);
            }
            else if (Pro.type == ProjectileType.ProjectileLightningBolt)
            {   //match explosions attributes
                damage = 2; force = 10.0f;
                //push actor in their opposite direction
                direction = Functions_Direction.GetOppositeDirection(Actor.direction);
            }

            //hammers might be op, i dunno yet
            else if (Pro.type == ProjectileType.ProjectileHammer)
            {   //match explosions attributes
                damage = 2; force = 10.0f;
                //push actor in their opposite direction
                direction = Functions_Direction.GetOppositeDirection(Actor.direction);
            }

            #endregion



            //2. ENEMY INVINCIBILITY - check actor v pro/obj status effects 

            //this could also include blob type
            //this does not include link type


            #region Standard Enemy - INVINCIBILITIES

            if (Actor.type == ActorType.Standard_BeefyBat)
            {   //prevent friendly fire between enemies attacking in groups
                if (Pro.type == ProjectileType.ProjectileBite) { return; } //bail
            }
            else if (Actor.type == ActorType.Standard_AngryEye)
            {
                //
            }

            #endregion


            #region MiniBosses - INVINCIBILITIES

            //armored spider resists all projectiles except a few
            else if (Actor.type == ActorType.MiniBoss_Spider_Armored)
            {   //these projectiles ACTUALLY damage armored spider
                if (
                    Pro.type == ProjectileType.ProjectileExplosion
                    || Pro.type == ProjectileType.ProjectileLightningBolt
                    || Pro.type == ProjectileType.ProjectileShovel
                    || Pro.type == ProjectileType.ProjectileHammer
                    )
                { }
                else
                {   //all others create 'clink' sfx
                    damage = 0; //deal no damage
                    force = 3.0f; //& push spider very little
                }
            }
            else if (Actor.type == ActorType.MiniBoss_Spider_Unarmored)
            {   //immune to bite projectiles
                if (Pro.type == ProjectileType.ProjectileBite) { return; }
            }   //fast moving, can overlap it's own bite pro

            #endregion


            #region Bosses - INVINCIBILITIES

            else if (Actor.type == ActorType.Boss_BigEye)
            {   //immune to bite projectiles
                if (Pro.type == ProjectileType.ProjectileBite) { return; }
            }   //he moves fast and can overlap his bite pro, self-harming

            else if (Actor.type == ActorType.Boss_BigBat)
            {   //bat projectiles cant deal damage to the bat boss
                //(he spawns them, and it would be cheap)
                if (Pro.type == ProjectileType.ProjectileBat)
                { Pro.lifeCounter = 0; return; } //keep bat alive, bail
                //immune to bite projectiles
                if (Pro.type == ProjectileType.ProjectileBite) { return; }
            }   //same reason as big eye

            else if (Actor.type == ActorType.Boss_OctoHead)
            {   //prevent boomerang from spawning tentacle accidentally
                if (Pro.type == ProjectileType.ProjectileBoomerang)
                {   //stop all boomerang movement, bounce off object
                    Functions_Movement.StopMovement(Pro.compMove);
                    Functions_Movement.Push(Pro.compMove,
                        Functions_Direction.GetOppositeCardinal(
                            Pro.compSprite.position,
                            Actor.compSprite.position), 4.0f);
                    Functions_Particle.Spawn(ObjType.Particle_ImpactDust, Pro);
                    Assets.Play(Assets.sfxActorLand);
                    Pro.lifeCounter = 200;
                    return;
                }   //set into return mode, bail
            }

            #endregion


            #region Special Enemies (tentacle)

            else if (Actor.type == ActorType.Special_Tentacle)
            {   //prevent friendly fire between enemies attacking in groups
                if (Pro.type == ProjectileType.ProjectileBite) { return; } //bail
            }

            #endregion






            //3. pass completed damage & force
            Damage(Actor, damage, force, direction);
        }





        public static void Damage(Actor Actor, GameObject Obj)
        {
            //bail from method if actor is underwater
            if (Actor.underwater) { return; }


            //based on the obj type, deal damage to the actor, push in a direction

            //0. Reset damage fields
            direction = Direction.None;
            damage = 0;
            force = 0.0f;


            //1. setup objects (set damage, force, direction)


            #region Objects

            //dungeon objs are always power level 1 or 0 
            if(Obj.type == ObjType.Dungeon_SpikesFloorOn)
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


            //2. there are currently no obj vs enemy invicibilities



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
                    Actor.breathCounter = 0;
                    Functions_Actor.CreateSplash(Actor);
                }

            }
        }

    }
}