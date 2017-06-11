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
    public static class Functions_Entity
    {

        public static void SpawnEntity(ObjType Type, Actor Actor)
        {   //set the offsets for projectile's spawn, based on actor type and direction
            Functions_Alignment.SetOffsets(Actor, Type);
            //pass the calculated offsets to the SpawnEntity() method
            SpawnEntity(Type,
                Actor.compSprite.position.X + Functions_Alignment.offsetX,
                Actor.compSprite.position.Y + Functions_Alignment.offsetY,
                Actor.direction);
        }

        public static void SpawnEntity(ObjType Type, float X, float Y, Direction Direction)
        {   //this method spawns a projectile at the X, Y location, with a cardinal direction
            GameObject obj = Functions_Pool.GetEntity();
            //default this projectile/particle to Down direction
            obj.direction = Direction.Down;
            obj.compMove.direction = Direction.Down;

            //certain projectiles/particles get a cardinal direction, others dont
            if (Type == ObjType.ProjectileFireball || 
                Type == ObjType.ProjectileSword ||
                Type == ObjType.ProjectileArrow ||
                Type == ObjType.ParticleBow)
            {
                obj.direction = Functions_Direction.GetCardinalDirection(Direction);
                obj.compMove.direction = Functions_Direction.GetCardinalDirection(Direction);
            }

            //teleport the projectile to the proper location
            Functions_Movement.Teleport(obj.compMove, X, Y);
            //set the type, rotation, cellsize, & alignment as last step
            Functions_GameObject.SetType(obj, Type);
        }

        public static void HandleBirthEvent(GameObject Obj)
        {   //Obj is from the Entity pool


            #region Projectiles

            if (Obj.type == ObjType.ProjectileBomb)
            {
                SpawnEntity(ObjType.ParticleAttention,
                    Obj.compSprite.position.X + 0,
                    Obj.compSprite.position.Y + 0,
                    Direction.None);
                Assets.Play(Assets.sfxBombDrop);
            }
            else if (Obj.type == ObjType.ProjectileFireball)
            {   //place smoke puff centered to fireball
                SpawnEntity(ObjType.ParticleSmokePuff,
                    Obj.compSprite.position.X + 4,
                    Obj.compSprite.position.Y + 4,
                    Direction.None);
                Assets.Play(Assets.sfxFireballCast);
            }
            else if (Obj.type == ObjType.ProjectileArrow)
            {
                Assets.Play(Assets.sfxArrowShoot);
            }
            else if (Obj.type == ObjType.ProjectileExplosion)
            {   //place smoke puff above explosion
                SpawnEntity(ObjType.ParticleSmokePuff,
                    Obj.compSprite.position.X + 4,
                    Obj.compSprite.position.Y - 8,
                    Direction.None);
                Assets.Play(Assets.sfxExplosion);
            }

            #endregion


        }

        public static void HandleDeathEvent(GameObject Obj)
        {   //Obj is from the Entity pool

            if (Obj.group == ObjGroup.Pickup)
            {   //when an item pickup dies, display an attention particle
                SpawnEntity(ObjType.ParticleAttention,
                    Obj.compSprite.position.X + 0,
                    Obj.compSprite.position.Y + 0,
                    Direction.None);
            }


            #region Projectiles

            else if (Obj.type == ObjType.ProjectileBomb)
            {   //create explosion projectile
                SpawnEntity(ObjType.ProjectileExplosion,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y,
                    Direction.None);
            }
            else if (Obj.type == ObjType.ProjectileFireball)
            {
                SpawnEntity(ObjType.ParticleExplosion,
                    Obj.compSprite.position.X + 0,
                    Obj.compSprite.position.Y + 0,
                    Direction.None);
                SpawnEntity(ObjType.ParticleFire,
                    Obj.compSprite.position.X + 0,
                    Obj.compSprite.position.Y + 0,
                    Direction.None);
                Assets.Play(Assets.sfxFireballDeath);
            }
            else if (Obj.type == ObjType.ProjectileArrow)
            {
                SpawnEntity(ObjType.ParticleAttention,
                    Obj.compSprite.position.X + 0,
                    Obj.compSprite.position.Y + 0,
                    Direction.None);
                Assets.Play(Assets.sfxArrowHit);
                //spawn an arrow pickup item (so hero can reuse arrows, if he's quick)
                /*
                SpawnEntity(ObjType.PickupArrow,
                    Obj.compSprite.position.X + 4,
                    Obj.compSprite.position.Y - 2, 
                    Direction.Down);
                */
            }

            #endregion


        }

    }
}