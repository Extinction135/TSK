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



        public static void Reset(Pickup Pick)
        {
            //reset pickup
            Pick.type = PickupType.Arrow; //reset the type
            Pick.direction = Direction.Down;
            Pick.active = true; //assume this object should draw / animate
            Pick.lifetime = 0; //assume obj exists forever (not projectile)
            Pick.lifeCounter = 0; //reset counter

            //reset the sprite component
            Pick.compSprite.drawRec.Width = 16 * 1; //assume cell size is 16x16 (most are)
            Pick.compSprite.drawRec.Height = 16 * 1;
            Pick.compSprite.zOffset = 0;
            Pick.compSprite.flipHorizontally = false;
            Pick.compSprite.rotation = Rotation.None;
            Pick.compSprite.scale = 1.0f;
            Pick.compSprite.texture = Assets.CommonObjsSheet;
            Pick.compSprite.visible = true;
            Pick.compSprite.position.X = -1000;
            Pick.compSprite.position.Y = -1000;

            //reset the animation component
            Pick.compAnim.speed = 10; //set obj's animation speed to default value
            Pick.compAnim.loop = true; //assume obj's animation loops
            Pick.compAnim.index = 0; //reset the current animation index/frame
            Pick.compAnim.timer = 0; //reset the elapsed frames

            //reset the collision component
            Pick.compCollision.blocking = true; //assume the object is blocking (most are)
            Pick.compCollision.rec.Width = 16; //assume collisionRec is 16x16
            Pick.compCollision.rec.Height = 16; //(most are)
            Pick.compCollision.offsetX = -8; //assume collisionRec offset is -8x-8
            Pick.compCollision.offsetY = -8; //(most are)
        }


        //spawn relative to actor
        public static void Spawn(PickupType Type, Actor Actor)
        {
            //entities are spawned relative to actor, based on passed objType
            //set position reference to Actor's sprite position
            posRef.X = Actor.compSprite.position.X;
            posRef.Y = Actor.compSprite.position.Y;
            //get the actor's facing direction as cardinal direction
            direction = Functions_Direction.GetCardinalDirection_LeftRight(Actor.direction);

            if (Type == PickupType.Rupee)
            {   //place dropped rupee away from hero, cardinal = direction actor was pushed
                if (direction == Direction.Down) { posRef.X += 4; posRef.Y -= 12; } //place above
                else if (direction == Direction.Up) { posRef.X += 4; posRef.Y += 15; } //place below
                else if (direction == Direction.Right) { posRef.X -= 14; posRef.Y += 4; } //place left
                else if (direction == Direction.Left) { posRef.X += 14; posRef.Y += 4; } //place right
            }

            Spawn(Type, posRef.X, posRef.Y);
        }

        public static void Spawn(PickupType Type, float X, float Y)
        {   //get a pickup to spawn
            Pickup obj = Functions_Pool.GetPickup();
            //set direction to down
            obj.direction = Direction.Down;

            //teleport pickup sprite and hitbox to the proper location
            obj.compSprite.position.X = (int)X;
            obj.compSprite.position.Y = (int)Y;
            obj.compCollision.rec.X = (int)X + obj.compCollision.offsetX;
            obj.compCollision.rec.Y = (int)Y + obj.compCollision.offsetY;

            //set the type then zdepth
            SetType(obj, Type);
            Functions_Component.SetZdepth(obj.compSprite);
        }





        public static void Update(Pickup Pickup)
        {   //pickups do have lifetimes
            Pickup.lifeCounter++;
            if (Pickup.lifeCounter >= Pickup.lifetime) { Kill(Pickup); }
        }

        public static void Kill(Pickup Pickup)
        {   //when an item pickup dies, display an attention particle
            Functions_Particle.Spawn(
                ParticleType.Attention,
                Pickup.compSprite.position.X - 4,
                Pickup.compSprite.position.Y - 2);
            //all objects are released upon death
            Functions_Pool.Release(Pickup);
        }







        public static void CheckOverlap(Projectile Pro)
        {   //check to see if PRO overlaps any PICKUP, handle effect on hero
            for (int i = 0; i < Pool.pickupCount; i++)
            {
                if(Pool.pickupPool[i].active)
                {
                    if (Pro.compCollision.rec.Intersects(Pool.pickupPool[i].compCollision.rec))
                    {   //this function assumes that the overlapping gameObj collects the pickup
                        HandleEffect(Pool.pickupPool[i]);
                    }
                }
            }
        }

        



        public static void HandleEffect(Pickup Pickup)
        {
            if (Pickup.type == PickupType.Heart)
            {
                Pool.hero.health++;
                Assets.Play(Assets.sfxHeartPickup);
            }
            else if (Pickup.type == PickupType.Rupee)
            {
                PlayerData.current.gold++;
                Assets.Play(Assets.sfxGoldPickup);
            }
            else if (Pickup.type == PickupType.Magic)
            {
                PlayerData.current.magicCurrent++;
                Assets.Play(Assets.sfxHeartPickup);
            }
            else if (Pickup.type == PickupType.Arrow)
            {
                PlayerData.current.arrowsCurrent++;
                Assets.Play(Assets.sfxHeartPickup);
            }
            else if (Pickup.type == PickupType.Bomb)
            {
                PlayerData.current.bombsCurrent++;
                Assets.Play(Assets.sfxHeartPickup);
            }

            Kill(Pickup);
        }




        public static void SetType(Pickup Pick, PickupType Type)
        {
            Pick.compSprite.drawRec.Width = 8; //non standard cellsize
            Pick.compCollision.offsetX = -8; Pick.compCollision.offsetY = -5;
            Pick.compCollision.rec.Width = 8; Pick.compCollision.rec.Height = 10;
            Pick.lifetime = 255; //in frames
            Pick.compAnim.speed = 6; //in frames
            Pick.compSprite.texture = Assets.entitiesSheet; //all use entity sheet
                                                            //set the animation frame
            if (Type == PickupType.Rupee)
            { Pick.compAnim.currentAnimation = AnimationFrames.Pickup_Rupee; }
            else if (Type == PickupType.Heart)
            { Pick.compAnim.currentAnimation = AnimationFrames.Pickup_Heart; }
            else if (Type == PickupType.Magic)
            { Pick.compAnim.currentAnimation = AnimationFrames.Pickup_Magic; }
            else if (Type == PickupType.Arrow)
            { Pick.compAnim.currentAnimation = AnimationFrames.Pickup_Arrow; }
            else if (Type == PickupType.Bomb)
            { Pick.compAnim.currentAnimation = AnimationFrames.Pickup_Bomb; }

            //pickups never block
            Pick.compCollision.blocking = false;
            //based on type, we can flip horizontally/rotate
            Pick.compSprite.flipHorizontally = false;
            Pick.compSprite.rotation = Rotation.None;
            //set animframe to 0
            Pick.compSprite.currentFrame = Pick.compAnim.currentAnimation[0];
        }





    }
}