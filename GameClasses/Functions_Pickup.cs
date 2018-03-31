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
    public static class Functions_Pickup
    {
        static Vector2 posRef = new Vector2();
        static Direction direction;

        //spawn relative to actor
        public static void Spawn(ObjType Type, Actor Actor)
        {
            //entities are spawned relative to actor, based on passed objType
            //set position reference to Actor's sprite position
            posRef.X = Actor.compSprite.position.X;
            posRef.Y = Actor.compSprite.position.Y;
            //get the actor's facing direction as cardinal direction
            direction = Functions_Direction.GetCardinalDirection(Actor.direction);

            if (Type == ObjType.PickupRupee)
            {   //place dropped rupee away from hero, cardinal = direction actor was pushed
                if (direction == Direction.Down) { posRef.X += 4; posRef.Y -= 12; } //place above
                else if (direction == Direction.Up) { posRef.X += 4; posRef.Y += 15; } //place below
                else if (direction == Direction.Right) { posRef.X -= 14; posRef.Y += 4; } //place left
                else if (direction == Direction.Left) { posRef.X += 14; posRef.Y += 4; } //place right
            }

            Spawn(Type, posRef.X, posRef.Y);
        }

        public static void Spawn(ObjType Type, float X, float Y)
        {   //get a pickup to spawn
            GameObject obj = Functions_Pool.GetPickup();
            obj.compMove.moving = true;
            //set direction to down
            obj.direction = Direction.Down;
            obj.compMove.direction = Direction.Down;
            //teleport the object to the proper location
            Functions_Movement.Teleport(obj.compMove, X, Y);
            //set the type, rotation, cellsize, & alignment
            Functions_GameObject.SetType(obj, Type);
            Functions_Component.Align(obj); //align upon birth
            //Debug.WriteLine("entity made: " + Type + " - location: " + X + ", " + Y);
        }



        public static void Update(GameObject Obj)
        {   //pickups do have lifetimes
            Obj.lifeCounter++;
            if (Obj.lifeCounter >= Obj.lifetime) { Kill(Obj); }
        }

        public static void Kill(GameObject Obj)
        {
            //when an item pickup dies, display an attention particle
            Functions_Particle.Spawn(
                ObjType.ParticleAttention,
                Obj.compSprite.position.X - 4,
                Obj.compSprite.position.Y - 2);
            //all objects are released upon death
            Functions_Pool.Release(Obj);
        }







        public static void CheckOverlap(GameObject Obj)
        {   //check to see if OBJ overlaps any OBJ on the pickups list, return pickup OBJ
            for (int i = 0; i < Pool.pickupCount; i++)
            {
                if(Pool.pickupPool[i].active)
                {
                    if (Obj.compCollision.rec.Intersects(Pool.pickupPool[i].compCollision.rec))
                    {   //this function assumes that the overlapping gameObj collects the pickup
                        HandleEffect(Pool.pickupPool[i]);
                    }
                }
            }
        }

        public static void HandleEffect(GameObject Pickup)
        {
            if (Pickup.type == ObjType.PickupHeart)
            {
                Pool.hero.health++;
                Assets.Play(Assets.sfxHeartPickup);
            }
            else if (Pickup.type == ObjType.PickupRupee)
            {
                PlayerData.current.gold++;
                Assets.Play(Assets.sfxGoldPickup);
            }
            else if (Pickup.type == ObjType.PickupMagic)
            {
                PlayerData.current.magicCurrent++;
                Assets.Play(Assets.sfxHeartPickup);
            }
            else if (Pickup.type == ObjType.PickupArrow)
            {
                PlayerData.current.arrowsCurrent++;
                Assets.Play(Assets.sfxHeartPickup);
            }
            else if (Pickup.type == ObjType.PickupBomb)
            {
                PlayerData.current.bombsCurrent++;
                Assets.Play(Assets.sfxHeartPickup);
            }

            //end the pickups life
            Pickup.lifeCounter = 2;
            Pickup.lifetime = 1;
        }


    }
}