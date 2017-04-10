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
            for (int i = 0; i < DungeonScreen.pool.objCount; i++)
            {
                if (DungeonScreen.pool.objPool[i].active)
                {   //return object if active and collides with source collision rec
                    if (Coll.rec.Intersects(DungeonScreen.pool.objPool[i].compCollision.rec))
                    {   
                        //do not return the source object (it collides with itself)
                        if (Coll != DungeonScreen.pool.objPool[i].compCollision)
                        { return DungeonScreen.pool.objPool[i]; }

                    }
                }
            }
            return null; //there was no collision
        }

        public static Actor CheckActorCollisions(ComponentCollision Coll, DungeonScreen DungeonScreen)
        {   
            for (int i = 0; i < DungeonScreen.pool.actorCount; i++)
            {
                if (DungeonScreen.pool.actorPool[i].active)
                {   //return actor if active and collides with source collision rec
                    if (Coll.rec.Intersects(DungeonScreen.pool.actorPool[i].compCollision.rec))
                    {   
                        //do not return the source object (it collides with itself)
                        if (Coll != DungeonScreen.pool.actorPool[i].compCollision)
                        { return DungeonScreen.pool.actorPool[i]; }

                    }
                }
            }
            return null; //there was no collision
        }









        static Boolean collisionX;
        static Boolean collisionY;
        public static void CheckCollisions(ComponentMovement Move, ComponentCollision Coll, DungeonScreen DungeonScreen)
        {
            //if the collision component isn't active, bail from collision checking
            if (!Coll.active) { return; }

            GameObject objCollision;
            Actor actorCollision;


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





            /*
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
            */



        }






    }
}