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
        static Vector2 posRef = new Vector2();
        static Direction direction;

        public static void SpawnEntity(ObjType Type, GameObject Object)
        {   //entities are spawned relative to Object, based on Object.type
            //set position reference to Object's sprite position
            posRef.X = Object.compSprite.position.X;
            posRef.Y = Object.compSprite.position.Y;
            //direction is set based on Object.type


            #region Sword/Net

            //we could spawn a fireball here if we wanted to (which is how we'll handle the staff weapon)

            if (Object.type == ObjType.ProjectileSword || Object.type == ObjType.ProjectileNet)
            {   //place entity at tip of sword, based on direction
                if (Object.direction == Direction.Up) { posRef.X += 8; posRef.Y -= 0; }
                else if (Object.direction == Direction.Right) { posRef.X += 8; posRef.Y += 8; }
                else if (Object.direction == Direction.Down) { posRef.X += 8; posRef.Y += 8; }
                else if (Object.direction == Direction.Left) { posRef.X += 2; posRef.Y += 8; }
                direction = Object.direction;
            }

            #endregion


            #region FlameThrower

            else if(Object.type == ObjType.Flamethrower)
            {   //shoots fireball (or whatever) at their position, facing towards the hero
                direction = Functions_Direction.GetDirectionToHero(Object.compSprite.position);
            }

            #endregion


            #region Wall Statue

            else if (Object.type == ObjType.WallStatue)
            {   //shoots arrow (or whatever) in it's facing direction, outside of obj's hitbox
                if (Object.direction == Direction.Down) { posRef.Y += 16; }
                else if (Object.direction == Direction.Up) { posRef.Y -= 16; }
                else if (Object.direction == Direction.Right) { posRef.X += 16; }
                else if (Object.direction == Direction.Left) { posRef.X -= 16; }
                direction = Object.direction;
            }

            #endregion


            #region Pit

            else if(Object.type == ObjType.PitAnimated)
            {   //randomly offset where the bubble particle is placed
                posRef.X += 4; posRef.Y += 4; //because bubble is 8x8 size
                posRef.X += Functions_Random.Int(-3, 4);
                posRef.Y += Functions_Random.Int(-3, 4);
                direction = Direction.None;
            }

            #endregion


            #region SpikeBlock

            else if(Object.type == ObjType.ProjectileSpikeBlock)
            {   //spikeblocks create hit particles upon their colliding (bounced) edge
                if (Object.compMove.direction == Direction.Down) { posRef.X += 4; posRef.Y += 10; }
                else if (Object.compMove.direction == Direction.Up) { posRef.X += 4; posRef.Y -= 4; }
                else if (Object.compMove.direction == Direction.Right) { posRef.X += 8; posRef.Y += 4; }
                else if (Object.compMove.direction == Direction.Left) { posRef.X -= 4; posRef.Y += 4; }
                direction = Object.compMove.direction;
            }

            #endregion


            SpawnEntity(Type, posRef.X, posRef.Y, direction);
        }

        public static void SpawnEntity(ObjType Type, Actor Actor)
        {   //entities are spawned relative to actor, based on passed objType
            //set position reference to Actor's sprite position
            posRef.X = Actor.compSprite.position.X;
            posRef.Y = Actor.compSprite.position.Y;
            //get the actor's facing direction as cardinal direction
            direction = Functions_Direction.GetCardinalDirection(Actor.direction);


            #region Particles

            if (Type == ObjType.ParticleDashPuff)
            {   //center horizontally, place near actor's feet
                posRef.X += 4; posRef.Y += 8;
            }
            else if (Type == ObjType.ParticleBow)
            {   //place bow particle in the actor's hands
                if (direction == Direction.Down) { posRef.Y += 6; }
                else if (direction == Direction.Up) { posRef.Y -= 6; }
                else if (direction == Direction.Right) { posRef.X += 6; }
                else if (direction == Direction.Left) { posRef.X -= 6; }
            }

            #endregion


            #region Pickups

            else if (Type == ObjType.PickupRupee)
            {   //place dropped rupee away from hero, cardinal = direction actor was pushed
                if (direction == Direction.Down) { posRef.X += 4; posRef.Y -= 12; } //place above
                else if (direction == Direction.Up) { posRef.X += 4; posRef.Y += 15; } //place below
                else if (direction == Direction.Right) { posRef.X -= 14; posRef.Y += 4; } //place left
                else if (direction == Direction.Left) { posRef.X += 14; posRef.Y += 4; } //place right
            }

            #endregion


            #region Projectiles

            else if (Type == ObjType.ProjectileArrow)
            {   //place projectile outside of actor's hitbox
                if (direction == Direction.Down) { posRef.Y += 14; }
                else if (direction == Direction.Up) { posRef.Y -= 9; }
                else if (direction == Direction.Right) { posRef.X += 13; posRef.Y += 2; }
                else if (direction == Direction.Left) { posRef.X -= 13; posRef.Y += 2; }
            }
            else if (Type == ObjType.ProjectileBomb)
            {   //bombs are placed closer to the actor
                if (direction == Direction.Down) { posRef.Y += 6; }
                else if (direction == Direction.Up) { posRef.Y += 0; }
                else if (direction == Direction.Right) { posRef.X += 4; posRef.Y += 2; }
                else if (direction == Direction.Left) { posRef.X -= 4; posRef.Y += 2; }
            }
            else if (Type == ObjType.ProjectileFireball)
            {   //place projectile outside of actor's hitbox
                if (direction == Direction.Down) { posRef.Y += 13; }
                else if (direction == Direction.Up) { posRef.Y -= 9; }
                else if (direction == Direction.Right) { posRef.X += 11; posRef.Y += 2; }
                else if (direction == Direction.Left) { posRef.X -= 11; posRef.Y += 2; }
            }
            else if (Type == ObjType.ProjectileSword || Type == ObjType.ProjectileNet)
            {   //place projectile outside of actor's hitbox, in actor's hand
                if (direction == Direction.Down) { posRef.X -= 1; posRef.Y += 15; }
                else if (direction == Direction.Up) { posRef.X += 1; posRef.Y -= 12; }
                else if (direction == Direction.Right) { posRef.X += 14; }
                else if (direction == Direction.Left) { posRef.X -= 14; }
            }

            #endregion


            #region Reward & Bottle Particles

            else if ( //place reward/bottle particles above actor's head
                Type == ObjType.ParticleRewardKey ||
                Type == ObjType.ParticleRewardMap ||
                Type == ObjType.ParticleBottleEmpty ||
                Type == ObjType.ParticleBottleHealth ||
                Type == ObjType.ParticleBottleMagic ||
                Type == ObjType.ParticleBottleCombo ||
                Type == ObjType.ParticleBottleFairy ||
                Type == ObjType.ParticleBottleBlob)
            { posRef.Y -= 14; }

            #endregion


            SpawnEntity(Type, posRef.X, posRef.Y, direction);
        }

        public static void SpawnEntity(ObjType Type, float X, float Y, Direction Direction)
        {   //actually spawns Entity at the X, Y location, with direction
            GameObject obj = Functions_Pool.GetEntity();


            #region Set Object's Direction

            //certain projectiles/particles get a cardinal direction, others dont
            if (Type == ObjType.ProjectileFireball || 
                Type == ObjType.ProjectileSword ||
                Type == ObjType.ProjectileNet ||
                Type == ObjType.ProjectileArrow ||
                Type == ObjType.ProjectileBomb ||
                Type == ObjType.ParticleBow ||
                Type == ObjType.ProjectileDebrisRock)
            {
                obj.direction = Direction;
                obj.compMove.direction = Direction;
            }
            else
            {   //other objects default to down
                obj.direction = Direction.Down;
                obj.compMove.direction = Direction.Down;
            }

            #endregion


            //teleport the object to the proper location
            Functions_Movement.Teleport(obj.compMove, X, Y);
            //set the type, rotation, cellsize, & alignment
            Functions_GameObject.SetType(obj, Type);

            //handle specific projectile characteristics

            #region  Set arrow collision rec based on direction

            if (Type == ObjType.ProjectileArrow)
            {
                //set collision rec based on direction
                if (obj.direction == Direction.Up || obj.direction == Direction.Down)
                {
                    obj.compCollision.offsetX = -2; obj.compCollision.offsetY = -6;
                    obj.compCollision.rec.Width = 4; obj.compCollision.rec.Height = 12;
                }
                else //left or right
                {
                    obj.compCollision.offsetX = -6; obj.compCollision.offsetY = -2;
                    obj.compCollision.rec.Width = 12; obj.compCollision.rec.Height = 4;
                }
            }

            #endregion


            #region Set Sword collision rec based on direction

            else if (Type == ObjType.ProjectileSword)
            {
                if (obj.direction == Direction.Up)
                {
                    obj.compCollision.offsetX = -4; obj.compCollision.offsetY = -4;
                    obj.compCollision.rec.Width = 10; obj.compCollision.rec.Height = 15;
                }
                else if (obj.direction == Direction.Down)
                {
                    obj.compCollision.offsetX = -4; obj.compCollision.offsetY = -5;
                    obj.compCollision.rec.Width = 10; obj.compCollision.rec.Height = 10;
                }
                else if (obj.direction == Direction.Left)
                {
                    obj.compCollision.offsetX = -4; obj.compCollision.offsetY = -3;
                    obj.compCollision.rec.Width = 11; obj.compCollision.rec.Height = 10;
                }
                else //right
                {
                    obj.compCollision.offsetX = -7; obj.compCollision.offsetY = -3;
                    obj.compCollision.rec.Width = 11; obj.compCollision.rec.Height = 10;
                }
            }

            #endregion


            #region Give Bombs an Initial Push (slide them)

            //bombs are pushed, and slide into a resting position
            else if (Type == ObjType.ProjectileBomb)
            { Functions_Movement.Push(obj.compMove, obj.compMove.direction, 10.0f); }

            #endregion


            #region Modify RockDebris Projectiles Animation Frame + Slide them

            //some projectiles get their current frame randomly assigned (for variation)
            else if(Type == ObjType.ProjectileDebrisRock)
            {   //is assigned 15,15 - randomize down to 14,14
                List <Byte4> rockFrame = new List<Byte4> { new Byte4(15, 15, 0, 0) };
                if (Functions_Random.Int(0, 100) > 50) { rockFrame[0].X = 14; }
                if (Functions_Random.Int(0, 100) > 50) { rockFrame[0].Y = 14; }
                obj.compAnim.currentAnimation = rockFrame;
                //push rock debris in their direction
                Functions_Movement.Push(obj.compMove, obj.compMove.direction, 5.0f);
            }

            #endregion

        }

        public static void HandleBirthEvent(GameObject Projectile)
        {

            #region Projectiles

            if (Projectile.type == ObjType.ProjectileArrow)
            {
                Assets.Play(Assets.sfxArrowShoot);
            }
            else if (Projectile.type == ObjType.ProjectileBomb)
            {   
                Assets.Play(Assets.sfxBombDrop);
                //bomb is initially sliding upon birth
                SpawnEntity(ObjType.ParticleDashPuff,
                    Projectile.compSprite.position.X + 0,
                    Projectile.compSprite.position.Y + 0,
                    Direction.None);
            }
            else if (Projectile.type == ObjType.ProjectileExplosion)
            {   
                Assets.Play(Assets.sfxExplosion);
                //place smoke puff above explosion
                SpawnEntity(ObjType.ParticleSmokePuff,
                    Projectile.compSprite.position.X + 4,
                    Projectile.compSprite.position.Y - 8,
                    Direction.None);
            }
            else if (Projectile.type == ObjType.ProjectileFireball)
            {   
                Assets.Play(Assets.sfxFireballCast);
                //place smoke puff centered to fireball
                SpawnEntity(ObjType.ParticleSmokePuff,
                    Projectile.compSprite.position.X + 4,
                    Projectile.compSprite.position.Y + 4,
                    Direction.None);
            }
            else if (Projectile.type == ObjType.ProjectileSword)
            {   
                Assets.Play(Assets.sfxSwordSwipe);
            }
            else if (Projectile.type == ObjType.ProjectileNet)
            {
                Assets.Play(Assets.sfxSwordSwipe); //play net swipe sound effect
            }

            #endregion


            #region Particles

            //shouldn't we be handling map/key particle soundfx here?


            else if(Projectile.type == ObjType.ParticleSplash)
            {
                Assets.Play(Assets.sfxSplash);
            }

            #endregion

        }

        public static void HandleDeathEvent(GameObject Obj)
        {
            if (Obj.group == ObjGroup.Pickup)
            {   //when an item pickup dies, display an attention particle
                SpawnEntity(ObjType.ParticleAttention,
                    Obj.compSprite.position.X + 0,
                    Obj.compSprite.position.Y + 0,
                    Direction.None);
            }


            #region Projectiles

            else if (Obj.type == ObjType.ProjectileArrow)
            {
                SpawnEntity(ObjType.ParticleAttention,
                    Obj.compSprite.position.X + 0,
                    Obj.compSprite.position.Y + 0,
                    Direction.None);
                Assets.Play(Assets.sfxArrowHit);
            }
            else if (Obj.type == ObjType.ProjectileBomb)
            {   //create explosion projectile
                SpawnEntity(ObjType.ProjectileExplosion,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y,
                    Direction.None);
            }
            //explosion
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
            //sword
            //rock debris

            #endregion

        }



        public static void ScatterRockDebris(Vector2 Pos, Boolean Push)
        {   //add up to 4 rocks randomly around the passed Pos value, with option to push them
            Direction pushDir = Direction.None;
            int spread = 6;
            if (Push) { pushDir = Functions_Direction.GetRandomCardinal(); }
            Functions_Entity.SpawnEntity(
                ObjType.ProjectileDebrisRock,
                Pos.X, Pos.Y, pushDir);
            if (Functions_Random.Int(0, 100) > 20)
            {   //sometimes  add another rock
                if (Push) { pushDir = Functions_Direction.GetRandomCardinal(); }
                Functions_Entity.SpawnEntity(
                    ObjType.ProjectileDebrisRock,
                    Pos.X + Functions_Random.Int(-spread, spread),
                    Pos.Y + Functions_Random.Int(-spread, spread), pushDir);
            }
            if (Functions_Random.Int(0, 100) > 40)
            {   //sometimes add another rock
                if (Push) { pushDir = Functions_Direction.GetRandomCardinal(); }
                Functions_Entity.SpawnEntity(
                    ObjType.ProjectileDebrisRock,
                    Pos.X + Functions_Random.Int(-spread, spread),
                    Pos.Y + Functions_Random.Int(-spread, spread), pushDir);
            }
            if (Functions_Random.Int(0, 100) > 60)
            {   //sometimes add another rock
                if (Push) { pushDir = Functions_Direction.GetRandomCardinal(); }
                Functions_Entity.SpawnEntity(
                    ObjType.ProjectileDebrisRock,
                    Pos.X + Functions_Random.Int(-spread, spread),
                    Pos.Y + Functions_Random.Int(-spread, spread), pushDir);
            }
        }

    }
}