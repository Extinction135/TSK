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
            //turn this object's blocking back on
            Coll.blocking = true; 
        }


        
        
        

    }
}