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
    public static class Functions_Projectiles
    {
        static Direction cardinal; //a cardinal direction used by SpawnProjectile()
        static Vector2 offset = new Vector2(0, 0);



        public static void SpawnProjectile(ObjType Type, Actor Actor)
        {   //wraps the SpawnProjectile() method below
            //applies projectile offset relative to Actor based on Type
            offset.X = 0; offset.Y = 0;
            cardinal = Functions_Direction.GetCardinalDirection(Actor.direction);

            //center horizontally, place near actor's feet
            if (Type == ObjType.ParticleDashPuff) { offset.X = 4; offset.Y = 8; }
            //center horizontally, place near actor's body
            else if (Type == ObjType.ParticleSmokePuff) { offset.X = 4; offset.Y = 4; }


            #region Pickups

            else if (Type == ObjType.PickupRupee)
            {   //place the dropped rupee away from hero, cardinal = pushed direction
                if (cardinal == Direction.Down) { offset.X = 4; offset.Y = -12; }
                else if (cardinal == Direction.Up) { offset.X = 4; offset.Y = 15; }
                else if (cardinal == Direction.Right) { offset.X = -14; offset.Y = 4; }
                else if (cardinal == Direction.Left) { offset.X = 14; offset.Y = 4; }
            }

            #endregion


            #region Projectiles

            //place fireballs relative to direction actor is facing
            else if (Type == ObjType.ProjectileFireball ||
                Type == ObjType.ProjectileBomb || Type == ObjType.ProjectileArrow)
            {
                if (cardinal == Direction.Down) { offset.Y = 14; }
                else if (cardinal == Direction.Up) { offset.Y = -9; }
                else if (cardinal == Direction.Right) { offset.X = 11; offset.Y = 2; }
                else if (cardinal == Direction.Left) { offset.X = -11; offset.Y = 2; }
            }
            //place swords relative to direction actor is facing
            else if (Type == ObjType.ProjectileSword)
            {

                if (cardinal == Direction.Down) { offset.X = -1; offset.Y = 15; }
                else if (cardinal == Direction.Up) { offset.X = 1; offset.Y = -12; }
                else if (cardinal == Direction.Right) { offset.X = 14; }
                else if (cardinal == Direction.Left) { offset.X = -14; }

                //can't we put the SetWeaponCollisions() routines here?
                //we know the direction the obj/actor is facing
                //then we could set the arrow's collision rec here as well
                //and get rid of SetWeaponCollisions() in Functions_GameObject
            }

            #endregion


            #region Reward Particles

            //place reward particles above actor's head
            else if (Type == ObjType.ParticleRewardGold ||
                Type == ObjType.ParticleRewardKey ||
                Type == ObjType.ParticleRewardMap ||
                Type == ObjType.ParticleRewardHeartFull ||
                Type == ObjType.ParticleRewardHeartPiece ||
                Type == ObjType.ParticleFairy)
            { offset.Y = -14; }

            #endregion


            //call the real SpawnProjectile method
            SpawnProjectile(Type,
                Actor.compSprite.position.X + offset.X,
                Actor.compSprite.position.Y + offset.Y,
                cardinal);
        }

        public static void SpawnProjectile(ObjType Type, float X, float Y, Direction Direction)
        {
            GameObject obj = Functions_Pool.GetProjectile();
            //default this projectile/particle to Down direction
            obj.direction = Direction.Down;
            obj.compMove.direction = Direction.Down;

            //convert projectile's directions to cardinal
            if (Type == ObjType.ProjectileFireball || Type == ObjType.ProjectileSword ||
                Type == ObjType.ProjectileArrow)
            {
                obj.direction = Functions_Direction.GetCardinalDirection(Direction);
                obj.compMove.direction = Functions_Direction.GetCardinalDirection(Direction);
            }





            //  ***** this can be moved into the above SpawnProjectile() method
            //set stationary weapon's collision recs, now that they have proper direction
            if (Type == ObjType.ProjectileSword) { Functions_GameObject.SetWeaponCollisions(obj); }







            //teleport the projectile to the proper location
            Functions_Movement.Teleport(obj.compMove, X, Y);
            //set the type, rotation, cellsize, & alignment as last step
            Functions_GameObject.SetType(obj, Type);
        }

        public static void HandleBirthEvent(GameObject Obj)
        {   //this targets projectiles/particles only
            if (Obj.type == ObjType.ProjectileBomb)
            {
                SpawnProjectile(ObjType.ParticleAttention,
                    Obj.compSprite.position.X + 0,
                    Obj.compSprite.position.Y + 0,
                    Direction.None);
                Assets.Play(Assets.sfxBombDrop);
            }
            else if (Obj.type == ObjType.ProjectileFireball)
            {   //place smoke puff centered to fireball
                SpawnProjectile(ObjType.ParticleSmokePuff,
                    Obj.compSprite.position.X + 4,
                    Obj.compSprite.position.Y + 4,
                    Direction.None);
                Assets.Play(Assets.sfxFireballCast);
            }
            else if (Obj.type == ObjType.ProjectileExplosion)
            {   //place smoke puff above explosion
                SpawnProjectile(ObjType.ParticleSmokePuff,
                    Obj.compSprite.position.X + 4,
                    Obj.compSprite.position.Y - 8,
                    Direction.None);
                Assets.Play(Assets.sfxExplosion);
            }
        }

        public static void HandleDeathEvent(GameObject Obj)
        {   //this targets projectiles/particles only
            if (Obj.type == ObjType.ProjectileBomb)
            {   //create explosion projectile
                SpawnProjectile(ObjType.ProjectileExplosion,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y,
                    Direction.None);
            }
            else if (Obj.type == ObjType.ProjectileFireball)
            {
                SpawnProjectile(ObjType.ParticleExplosion,
                    Obj.compSprite.position.X + 0,
                    Obj.compSprite.position.Y + 0,
                    Direction.None);
                SpawnProjectile(ObjType.ParticleFire,
                    Obj.compSprite.position.X + 0,
                    Obj.compSprite.position.Y + 0,
                    Direction.None);
                Assets.Play(Assets.sfxFireballDeath);
            }
            else if (Obj.type == ObjType.ProjectileArrow)
            {
                SpawnProjectile(ObjType.ParticleAttention,
                    Obj.compSprite.position.X + 0,
                    Obj.compSprite.position.Y + 0,
                    Direction.None);
                Assets.Play(Assets.sfxMetallicTap);
                //spawn an arrow pickup item (so hero can reuse arrows, if he's quick)
                Functions_Projectiles.SpawnProjectile(ObjType.PickupArrow,
                    Obj.compSprite.position.X + 4,
                    Obj.compSprite.position.Y - 2, 
                    Direction.Down);
            }
        }

    }
}