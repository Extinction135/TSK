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

        public static GameObject CheckObjPoolCollisions(ComponentCollision Coll, DungeonScreen DungeonScreen)
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

        public static Actor CheckActorPoolCollisions(ComponentCollision Coll, DungeonScreen DungeonScreen)
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

        public static GameObject CheckProjectilePoolCollisions(ComponentCollision Coll, DungeonScreen DungeonScreen)
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
            objCollision = CheckObjPoolCollisions(Actor.compCollision, DungeonScreen);
            actorCollision = CheckActorPoolCollisions(Actor.compCollision, DungeonScreen);
            
            //handle collisions
            if (objCollision != null && objCollision.compCollision.blocking) { collisionX = true; } 
            if (actorCollision != null && actorCollision.compCollision.blocking) { collisionX = true; } 
            //unproject collisionRec on X axis
            Actor.compCollision.rec.X = (int)Actor.compMove.position.X + Actor.compCollision.offsetX;
            
            //project collisionRec on Y axis
            Actor.compCollision.rec.Y = (int)Actor.compMove.newPosition.Y + Actor.compCollision.offsetY;
            //get actor, object, projectile collisions
            objCollision = CheckObjPoolCollisions(Actor.compCollision, DungeonScreen);
            actorCollision = CheckActorPoolCollisions(Actor.compCollision, DungeonScreen);

            //handle collisions
            if (objCollision != null && objCollision.compCollision.blocking) { collisionY = true; }
            if (actorCollision != null && actorCollision.compCollision.blocking) { collisionY = true; }
            //unproject collisionRec on Y axis
            Actor.compCollision.rec.Y = (int)Actor.compMove.position.Y + Actor.compCollision.offsetY;
            
            //handle collision effects
            if (objCollision != null && !objCollision.compCollision.blocking)
            { }//pass this to another function that determines effect (Actor, Obj)
            if (actorCollision != null && !actorCollision.compCollision.blocking)
            { }//pass this to another function that determines effect (Actor, Actor)

            //resolve movement
            //if there was a collision, the new position reverts to the old position
            if (collisionX) { Actor.compMove.newPosition.X = Actor.compMove.position.X; }
            if (collisionY) { Actor.compMove.newPosition.Y = Actor.compMove.position.Y; }
            //the current position becomes the new position
            Actor.compMove.position.X = Actor.compMove.newPosition.X;
            Actor.compMove.position.Y = Actor.compMove.newPosition.Y;
        }

        public static void CheckCollisions(GameObject Obj, DungeonScreen DungeonScreen)
        {
            PrepForCollisionChecking();
            //check projectile against gameObjs & other projectiles (blocking, might change state)
            Obj.compCollision.rec.X = (int)Obj.compMove.newPosition.X + Obj.compCollision.offsetX;
            Obj.compCollision.rec.Y = (int)Obj.compMove.newPosition.Y + Obj.compCollision.offsetY;

            //get actor, object, projectile collisions
            actorCollision = CheckActorPoolCollisions(Obj.compCollision, DungeonScreen);
            objCollision = CheckObjPoolCollisions(Obj.compCollision, DungeonScreen);
            projectileCollision = CheckProjectilePoolCollisions(Obj.compCollision, DungeonScreen);

            //handle collision effects
            if (actorCollision != null)
            { }//pass this to another function that determines effect (Projectile, Actor)
            if (objCollision != null)
            { }//pass this to another function that determines effect (Projectile, Obj)
            if (projectileCollision != null)
            { }//pass this to another function that determines effect (Projectile, Projectile)

            //if a projectile collides with something, the projectile may get destroyed (end of lifetime)
            //Obj.lifeCounter = Obj.lifetime; //end the projectiles life
            //create an explosion effect here
            //additional projectile death effects here

            //the current position becomes the new position
            Obj.compMove.position.X = Obj.compMove.newPosition.X;
            Obj.compMove.position.Y = Obj.compMove.newPosition.Y;
        }

    }
}