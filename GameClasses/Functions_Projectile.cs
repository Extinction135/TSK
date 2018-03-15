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
    public static class Functions_Projectile
    {
        static Vector2 posRef = new Vector2();
        static Direction direction;

        //spawn relative to object
        public static void Spawn(ObjType Type, GameObject Object)
        {
            //set position reference to sprite position
            posRef.X = Object.compSprite.position.X;
            posRef.Y = Object.compSprite.position.Y;
            //direction is set based on obj.type


            #region Sword / Net

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

            else if (Object.type == ObjType.Flamethrower)
            {   //shoots fireball (or whatever) at their position, facing towards the hero
                direction = Functions_Direction.GetCardinalDirectionToHero(Object.compSprite.position);
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


            Spawn(Type, posRef.X, posRef.Y, direction);
        }

        //spawn relative to actor
        public static void Spawn(ObjType Type, Actor Actor)
        {
            //entities are spawned relative to actor, based on passed objType
            //set position reference to Actor's sprite position
            posRef.X = Actor.compSprite.position.X;
            posRef.Y = Actor.compSprite.position.Y;
            //get the actor's facing direction as cardinal direction
            direction = Functions_Direction.GetCardinalDirection(Actor.direction);


            #region Pickups

            if (Type == ObjType.PickupRupee)
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
                if (direction == Direction.Down) { posRef.Y += 16; }
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
            else if (Type == ObjType.ProjectilePot)
            {   //make direction opposite if actor is hit (only applies to hero)
                if (Actor == Pool.hero & Actor.state == ActorState.Hit)
                {   //this causes the carrying pot to be thrown in the correct direction
                    direction = Functions_Direction.GetOppositeDirection(direction);
                }
                //place projectile outside of actor's hitbox, above actors head
                if (direction == Direction.Down) { posRef.Y += 15; }
                else if (direction == Direction.Up) { posRef.Y -= 12; }
                else if (direction == Direction.Right) { posRef.X += 14; posRef.Y -= 8; }
                else if (direction == Direction.Left) { posRef.X -= 14; posRef.Y -= 8; }
            }

            #endregion


            Spawn(Type, posRef.X, posRef.Y, direction);
        }

        //spawn relative to position
        public static void Spawn(ObjType Type, float X, float Y, Direction Direction)
        {
            //get a projectile to spawn
            Projectile obj = Functions_Pool.GetProjectile();
            obj.compMove.moving = true;




            #region Set Object's Direction

            //certain projectiles/particles get a cardinal direction, others dont
            if (Type == ObjType.ProjectileFireball ||
                Type == ObjType.ProjectileSword ||
                Type == ObjType.ProjectileNet ||
                Type == ObjType.ProjectileArrow ||
                Type == ObjType.ProjectileBomb ||
                Type == ObjType.ParticleBow ||
                Type == ObjType.ProjectileDebrisRock ||
                Type == ObjType.ProjectilePot ||
                Type == ObjType.ProjectileExplodingBarrel)
            {
                obj.direction = Direction;
                obj.compMove.direction = Direction;
            }
            else
            {   //ALL other objects default to down
                obj.direction = Direction.Down;
                obj.compMove.direction = Direction.Down;
            }

            #endregion


            //teleport the object to the proper location
            Functions_Movement.Teleport(obj.compMove, X, Y);
            //set the type, rotation, cellsize, & alignment
            Functions_GameObject.SetType(obj, Type);

            //handle specific projectile characteristics

            #region  Set arrow collision rec based on direction + Push them

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
                Functions_Movement.Push(obj.compMove, obj.compMove.direction, 6.0f);
            }

            #endregion


            #region Push Fireballs (they are flying thru the air)

            //bombs are pushed, and slide into a resting position
            else if (Type == ObjType.ProjectileFireball)
            { Functions_Movement.Push(obj.compMove, obj.compMove.direction, 5.0f); }

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


            #region Push Bombs (sliding)

            //bombs are pushed, and slide into a resting position
            else if (Type == ObjType.ProjectileBomb)
            { Functions_Movement.Push(obj.compMove, obj.compMove.direction, 5.0f); }

            #endregion


            #region Push Pot Projectiles (being thrown)

            else if (Type == ObjType.ProjectilePot)
            {   //spawn a shadow for the Pot Projectile
                GameObject shadow = Functions_Pool.GetParticle();
                //teleport shadow to objects location, then to ground
                Functions_Movement.Teleport(shadow.compMove, X, Y + 16);
                Functions_GameObject.SetType(shadow, ObjType.ProjectileShadowSm);
                Functions_Component.Align(shadow);
                //shadow inherits some of pot's attributes so they will fly in sync
                shadow.compMove.friction = obj.compMove.friction;
                //push pot and shadow identically
                Functions_Movement.Push(obj.compMove, obj.compMove.direction, 4.0f);
                Functions_Movement.Push(shadow.compMove, obj.compMove.direction, 4.0f);
            }

            #endregion


            #region Give ExplodingBarrels an Initial Push (slide them)

            else if (Type == ObjType.ProjectileExplodingBarrel)
            { Functions_Movement.Push(obj.compMove, obj.compMove.direction, 6.0f); }

            #endregion


            #region Modify RockDebris Projectiles Animation Frame + Slide them

            //some projectiles get their current frame randomly assigned (for variation)
            else if (Type == ObjType.ProjectileDebrisRock)
            {   //is assigned 15,15 - randomize down to 14,14
                List<Byte4> rockFrame = new List<Byte4> { new Byte4(15, 15, 0, 0) };
                if (Functions_Random.Int(0, 100) > 50) { rockFrame[0].X = 14; }
                if (Functions_Random.Int(0, 100) > 50) { rockFrame[0].Y = 14; }
                obj.compAnim.currentAnimation = rockFrame;
                //push rock debris in their direction
                Functions_Movement.Push(obj.compMove, obj.compMove.direction, 5.0f);
            }

            #endregion


            Functions_Component.Align(obj); //align the entity upon birth
            obj.compCollision.blocking = false; //entities interact, never block

            //Debug.WriteLine("entity made: " + Type + " - location: " + X + ", " + Y);
        }







    }
}