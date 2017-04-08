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
    public static class CollisionFunctions
    {

        public static void Move(Actor Actor, DungeonScreen DungeonScreen)
        {
            ProjectMovement(Actor.compMove);
            CheckCollisions(Actor.compMove, Actor.compCollision, DungeonScreen);
            AlignComponents(Actor.compMove, Actor.compSprite, Actor.compCollision);
        }

        public static void Move(GameObject Obj, DungeonScreen DungeonScreen)
        {
            ProjectMovement(Obj.compMove);
            CheckCollisions(Obj.compMove, Obj.compCollision, DungeonScreen);
            AlignComponents(Obj.compMove, Obj.compSprite, Obj.compCollision);
        }
        



        public static void ProjectMovement(ComponentMovement Move)
        {
            //calculate magnitude
            if (Move.direction == Direction.Down)
            { Move.magnitude.Y += Move.speed; }
            else if (Move.direction == Direction.Left)
            { Move.magnitude.X -= Move.speed; }
            else if (Move.direction == Direction.Right)
            { Move.magnitude.X += Move.speed; }
            else if (Move.direction == Direction.Up)
            { Move.magnitude.Y -= Move.speed; }
            else if (Move.direction == Direction.DownLeft)
            { Move.magnitude.Y += Move.speed * 0.75f; Move.magnitude.X -= Move.speed * 0.75f; }
            else if (Move.direction == Direction.DownRight)
            { Move.magnitude.Y += Move.speed * 0.75f; Move.magnitude.X += Move.speed * 0.75f; }
            else if (Move.direction == Direction.UpLeft)
            { Move.magnitude.Y -= Move.speed * 0.75f; Move.magnitude.X -= Move.speed * 0.75f; }
            else if (Move.direction == Direction.UpRight)
            { Move.magnitude.Y -= Move.speed * 0.75f; Move.magnitude.X += Move.speed * 0.75f; }
            //apply friction to magnitude, clip magnitude to 0 when it gets very small
            Move.magnitude = Move.magnitude * Move.friction;
            if (Math.Abs(Move.magnitude.X) < 0.01f) { Move.magnitude.X = 0; }
            if (Math.Abs(Move.magnitude.Y) < 0.01f) { Move.magnitude.Y = 0; }
            //project newPosition based on current position + magnitude
            Move.newPosition.X = Move.position.X;
            Move.newPosition.Y = Move.position.Y;
            Move.newPosition += Move.magnitude;
        }




        static Boolean collisionX;
        static Boolean collisionY;
        public static void CheckCollisions(ComponentMovement Move, ComponentCollision Coll, DungeonScreen DungeonScreen)
        {
            //if the collision component isn't active, bail from collision checking
            if (!Coll.active) { return; }

            //this is to prevent the obj/actor from colliding with itself
            //the cheapest/fastest way to do this is to check a boolean
            Coll.blocking = false; 


            #region Project collisionRec along X axis, check collisions

            collisionX = false;
            Coll.rec.X = (int)Move.newPosition.X + Coll.offsetX; //project collisionRec on X axis
            for (int i = 0; i < DungeonScreen.pool.objCount; i++)
            {   //check collisions against objects
                if (DungeonScreen.pool.objPool[i].active)
                {   //bail if obj isn't active
                    if (DungeonScreen.pool.objPool[i].compCollision.blocking)
                    {   //bail if obj isn't blocking
                        if (Coll.rec.Intersects(DungeonScreen.pool.objPool[i].compCollision.rec))
                        { collisionX = true; }
                    }
                }
            }
            for (int i = 0; i < DungeonScreen.pool.actorCount; i++)
            {   //check collisions against actors
                if (DungeonScreen.pool.actorPool[i].active)
                {   //bail if actor isn't active
                    if (DungeonScreen.pool.actorPool[i].compCollision.blocking)
                    {   //bail if actor isn't blocking
                        if (Coll.rec.Intersects(DungeonScreen.pool.actorPool[i].compCollision.rec))
                        { collisionX = true; }
                    }
                }
            }
            //check against projectile collisions
            Coll.rec.X = (int)Move.position.X + Coll.offsetX; //unproject collisionRec on X axis

            #endregion


            #region Project collisionRec along Y axis, check collisions

            collisionY = false;
            Coll.rec.Y = (int)Move.newPosition.Y + Coll.offsetY;
            for (int i = 0; i < DungeonScreen.pool.objCount; i++)
            {   //check collisions against objects
                if (DungeonScreen.pool.objPool[i].active)
                {   //bail if obj isn't active
                    if (DungeonScreen.pool.objPool[i].compCollision.blocking)
                    {   //bail if obj isn't blocking
                        if (Coll.rec.Intersects(DungeonScreen.pool.objPool[i].compCollision.rec))
                        { collisionY = true; }
                    }
                }
            }
            for (int i = 0; i < DungeonScreen.pool.actorCount; i++)
            {   //check collisions against actors
                if (DungeonScreen.pool.actorPool[i].active)
                {   //bail if actor isn't active
                    if (DungeonScreen.pool.actorPool[i].compCollision.blocking)
                    {   //bail if actor isn't blocking
                        if (Coll.rec.Intersects(DungeonScreen.pool.actorPool[i].compCollision.rec))
                        { collisionY = true; }
                    }
                }
            }
            //check against projectile collisions
            Coll.rec.Y = (int)Move.position.Y + Coll.offsetY; //unproject collisionRec on Y axis

            #endregion


            //if there was a collision, the new position reverts to the old position
            if (collisionX) { Move.newPosition.X = Move.position.X; }
            if (collisionY) { Move.newPosition.Y = Move.position.Y; }
            //the current position becomes the new position
            Move.position.X = Move.newPosition.X;
            Move.position.Y = Move.newPosition.Y;

            Coll.blocking = true; //turn this object's blocking back on
        }



        public static void AlignComponents(ComponentMovement Move, ComponentSprite Sprite, ComponentCollision Coll)
        {   //aligns the Collision.rec and Sprite to the Move.position
            Sprite.position.X = (int)Move.newPosition.X;
            Sprite.position.Y = (int)Move.newPosition.Y;
            Sprite.SetZdepth();
            Coll.rec.X = (int)Move.newPosition.X + Coll.offsetX;
            Coll.rec.Y = (int)Move.newPosition.Y + Coll.offsetY;
        }
    }
}