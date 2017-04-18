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

        static int i;




        static Boolean collision = false;
        public static Boolean CheckObjPoolCollisions(Actor Actor)
        {
            collision = false;
            for (i = 0; i < Pool.objCount; i++)
            {
                if (Pool.objPool[i].active)
                {
                    if (Actor.compCollision.rec.Intersects(Pool.objPool[i].compCollision.rec))
                    {
                        InteractionFunctions.Handle(Actor, Pool.objPool[i]);
                        if (Pool.objPool[i].compCollision.blocking) { collision = true; }
                    }
                }
            }
            return collision; //no collision
        }












        public static GameObject CheckObjPoolCollisions(ComponentCollision Coll)
        {
            for (i = 0; i < Pool.objCount; i++)
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

        public static Actor CheckActorPoolCollisions(ComponentCollision Coll)
        {   
            for (i = 0; i < Pool.actorCount; i++)
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

        public static GameObject CheckProjectilePoolCollisions(ComponentCollision Coll)
        {
            for (i = 0; i < Pool.projectileCount; i++)
            {
                if (Pool.projectilePool[i].active)
                {   //return actor if active and collides with source collision rec
                    if (Coll.rec.Intersects(Pool.projectilePool[i].compCollision.rec))
                    {   //do not return the source actor (it collides with itself)
                        if (Coll != Pool.projectilePool[i].compCollision)
                        { return Pool.projectilePool[i]; }
                    }
                }
            }
            return null; //no collision
        }




        static Boolean collisionX;
        static Boolean collisionY;
        static GameObject objCollision;
        static Actor actorCollision;
        static GameObject projectileCollision;

        public static void PrepForCollisionChecking()
        {
            collisionX = false;
            collisionY = false;
            objCollision = null;
            actorCollision = null;
            projectileCollision = null;
        }

        public static void CheckCollisions(Actor Actor, DungeonScreen DungeonScreen)
        {
            PrepForCollisionChecking();
            //project collisionRec on X axis
            Actor.compCollision.rec.X = (int)Actor.compMove.newPosition.X + Actor.compCollision.offsetX;
            //get actor, object, projectile collisions
            objCollision = CheckObjPoolCollisions(Actor.compCollision);
            actorCollision = CheckActorPoolCollisions(Actor.compCollision);
            
            //handle collisions
            if (objCollision != null && objCollision.compCollision.blocking) { collisionX = true; } 
            if (actorCollision != null) { collisionX = true; } 
            //unproject collisionRec on X axis
            Actor.compCollision.rec.X = (int)Actor.compMove.position.X + Actor.compCollision.offsetX;
            
            //project collisionRec on Y axis
            Actor.compCollision.rec.Y = (int)Actor.compMove.newPosition.Y + Actor.compCollision.offsetY;
            //get actor, object, projectile collisions
            objCollision = CheckObjPoolCollisions(Actor.compCollision);
            actorCollision = CheckActorPoolCollisions(Actor.compCollision);

            //handle collisions
            if (objCollision != null && objCollision.compCollision.blocking) { collisionY = true; }
            if (actorCollision != null) { collisionY = true; }
            //unproject collisionRec on Y axis
            Actor.compCollision.rec.Y = (int)Actor.compMove.position.Y + Actor.compCollision.offsetY;
            
            //handle collision effects (interaction)
            if (objCollision != null && !objCollision.compCollision.blocking)
            { InteractionFunctions.Handle(Actor, objCollision); }

            //resolve movement
            //if there was a collision, the new position reverts to the old position
            if (collisionX) { Actor.compMove.newPosition.X = Actor.compMove.position.X; }
            if (collisionY) { Actor.compMove.newPosition.Y = Actor.compMove.position.Y; }
            //the current position becomes the new position
            Actor.compMove.position.X = Actor.compMove.newPosition.X;
            Actor.compMove.position.Y = Actor.compMove.newPosition.Y;
        }

        public static void CheckCollisions(GameObject Projectile, DungeonScreen DungeonScreen)
        {
            //do not check collisions for particles
            if (Projectile.objGroup == GameObject.ObjGroup.Particle) { return; }

            PrepForCollisionChecking();
            //check projectile against gameObjs & other projectiles (blocking, might change state)
            Projectile.compCollision.rec.X = (int)Projectile.compMove.newPosition.X + Projectile.compCollision.offsetX;
            Projectile.compCollision.rec.Y = (int)Projectile.compMove.newPosition.Y + Projectile.compCollision.offsetY;

            //get actor, object, projectile collisions
            actorCollision = CheckActorPoolCollisions(Projectile.compCollision);
            objCollision = CheckObjPoolCollisions(Projectile.compCollision);
            projectileCollision = CheckProjectilePoolCollisions(Projectile.compCollision);

            //handle collision effects
            if (actorCollision != null) { InteractionFunctions.Handle(Projectile, actorCollision); }
            if (objCollision != null) { InteractionFunctions.Handle(Projectile, objCollision); }
            if (projectileCollision != null) { InteractionFunctions.Handle(Projectile, projectileCollision); }

            //the current position becomes the new position
            Projectile.compMove.position.X = Projectile.compMove.newPosition.X;
            Projectile.compMove.position.Y = Projectile.compMove.newPosition.Y;
        }

    }
}