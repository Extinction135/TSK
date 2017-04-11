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

        public static GameObject CheckObjCollisions(ComponentCollision Coll, DungeonScreen DungeonScreen)
        {
            for (int i = 0; i < Pool.objCount; i++)
            {
                if (Pool.objPool[i].active)
                {   //return object if active and collides with source collision rec
                    if (Coll.rec.Intersects(Pool.objPool[i].compCollision.rec))
                    {   //do not return the source object (it collides with itself)
                        if (Coll != Pool.objPool[i].compCollision)
                        { return Pool.objPool[i]; }
                    }
                }
            }
            return null; //no collision
        }

        public static Actor CheckActorCollisions(ComponentCollision Coll, DungeonScreen DungeonScreen)
        {   
            for (int i = 0; i < Pool.actorCount; i++)
            {
                if (Pool.actorPool[i].active)
                {   //return actor if active and collides with source collision rec
                    if (Coll.rec.Intersects(Pool.actorPool[i].compCollision.rec))
                    {   //do not return the source actor (it collides with itself)
                        if (Coll != Pool.actorPool[i].compCollision)
                        { return Pool.actorPool[i]; }
                    }
                }
            }
            return null; //no collision
        }

        static Boolean collisionX;
        static Boolean collisionY;
        static GameObject objCollision;
        static Actor actorCollision;
        public static void CheckCollisions(ComponentMovement Move, ComponentCollision Coll, DungeonScreen DungeonScreen)
        {
            //if the collision component isn't active, bail from collision checking
            if (!Coll.active) { return; }
            objCollision = null;
            actorCollision = null;


            #region Handle Collisions on X axis

            //project collisionRec on X axis, get collisions
            Coll.rec.X = (int)Move.newPosition.X + Coll.offsetX; 
            objCollision = CheckObjCollisions(Coll, DungeonScreen);
            actorCollision = CheckActorCollisions(Coll, DungeonScreen);
            //handle collisions
            collisionX = false;
            if (objCollision != null)
            {
                if (objCollision.compCollision.blocking) { collisionX = true; }
                else { } //pass this to another function that determines effect
            }
            if (actorCollision != null)
            {
                if (actorCollision.compCollision.blocking) { collisionX = true; }
                else { } //pass this to another function that determines effect
            }
            //unproject collisionRec on X axis
            Coll.rec.X = (int)Move.position.X + Coll.offsetX;

            #endregion


            #region Handle Collisions on Y axis

            //project collisionRec on X axis, get collisions
            Coll.rec.Y = (int)Move.newPosition.Y + Coll.offsetY;
            objCollision = CheckObjCollisions(Coll, DungeonScreen);
            actorCollision = CheckActorCollisions(Coll, DungeonScreen);
            //handle collisions
            collisionY = false;
            if (objCollision != null)
            {
                if (objCollision.compCollision.blocking) { collisionY = true; }
                else { } //pass this to another function that determines effect
            }
            if (actorCollision != null)
            {
                if (actorCollision.compCollision.blocking) { collisionY = true; }
                else { } //pass this to another function that determines effect
            }
            //unproject collisionRec on Y axis
            Coll.rec.Y = (int)Move.position.Y + Coll.offsetY;

            #endregion


            //resolve movement
            //if there was a collision, the new position reverts to the old position
            if (collisionX) { Move.newPosition.X = Move.position.X; }
            if (collisionY) { Move.newPosition.Y = Move.position.Y; }
            //the current position becomes the new position
            Move.position.X = Move.newPosition.X;
            Move.position.Y = Move.newPosition.Y;
        }

    }
}