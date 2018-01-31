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
    public static class Functions_Collision
    {
        static int i;
        static Boolean collision = false;
        static Boolean collisionX;
        static Boolean collisionY;
        static Boolean collisionXY;

        public static Boolean CheckObjPoolCollisions(Actor Actor)
        {
            collision = false; //assume no collision
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active) //roomObj must be active
                {   //check for overlap
                    if (Actor.compCollision.rec.Intersects(Pool.roomObjPool[i].compCollision.rec))
                    {   //check if obj is blocking
                        if (Pool.roomObjPool[i].compCollision.blocking)
                        {
                            collision = true; //blocking collisions return true
                            if (Actor == Pool.hero) //hero interacts with all objects
                            { Functions_Interaction.InteractActor(Actor, Pool.roomObjPool[i]); }
                        } //actors interact with non-blocking objects
                        else { Functions_Interaction.InteractActor(Actor, Pool.roomObjPool[i]); }
                    }
                    Pool.collisionsCount++;
                }
            }
            return collision;
        }

        public static Boolean CheckActorPoolCollisions(Actor Actor)
        {
            collision = false; //assume no collision
            for (i = 0; i < Pool.actorCount; i++)
            {
                if (Pool.actorPool[i].active) //actor must be active
                {   //actors cannot collide with themselves
                    if (Actor != Pool.actorPool[i])
                    {   //remove doggo from actor vs actor collision checking
                        if(Actor.type == ActorType.Pet || Pool.actorPool[i].type == ActorType.Pet) { }
                        //only check collisions between actors & hero
                        else if (Actor == Pool.hero || Pool.actorPool[i] == Pool.hero)
                        {   //check for overlap
                            if (Actor.compCollision.rec.Intersects(Pool.actorPool[i].compCollision.rec))
                            {   //handle any hero vs actor collision
                                if (Actor == Pool.hero) { Functions_Hero.CollideWith(Pool.actorPool[i]); }
                                collision = true;
                            }
                        }
                        Pool.collisionsCount++;
                    }
                }
            }
            return collision; 
        }

        public static Boolean CheckObjPoolCollisions(GameObject Object)
        {
            collision = false; //assume no collision
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active) //roomObj must be active
                {   //check for overlap
                    if (Object.compCollision.rec.Intersects(Pool.roomObjPool[i].compCollision.rec))
                    {   //roomObjs cant collide with themselves
                        if (Object != Pool.roomObjPool[i])
                        {
                            if (Pool.roomObjPool[i].compCollision.blocking) { collision = true; }
                            Functions_Interaction.InteractObject(Object, Pool.roomObjPool[i]);
                        }
                    }
                    Pool.collisionsCount++;
                }
            }
            return collision;
        }

        public static Boolean CheckActorPoolCollisions(GameObject Obj)
        {
            collision = false; //assume no collision
            for (i = 0; i < Pool.actorCount; i++)
            {   //skip checking projectiles/objs against hero's pet
                if (Pool.actorPool[i] == Pool.herosPet) { } //no checking
                else if (Pool.actorPool[i].active) //actor must be active
                {   //check for overlap
                    if (Obj.compCollision.rec.Intersects(Pool.actorPool[i].compCollision.rec))
                    {   //handle interaction
                        Functions_Interaction.InteractActor(Pool.actorPool[i], Obj);
                        collision = true;
                    }
                    Pool.collisionsCount++;
                }
            }
            return collision;
        }

        public static void CheckCollisions(Actor Actor)
        {   //most frames, an actor isn't colliding with anything, so do diagonal check first
            //triple projection collision check (X&Y, X, Y axis)
            collisionX = false; collisionY = false; collisionXY = false; //horizontal, vertical, and diagonal check

            //project on both X and Y axis
            Actor.compCollision.rec.X = (int)Actor.compMove.newPosition.X + Actor.compCollision.offsetX;
            Actor.compCollision.rec.Y = (int)Actor.compMove.newPosition.Y + Actor.compCollision.offsetY;
            //check object and actor collisions/interactions
            if (CheckObjPoolCollisions(Actor)) { collisionXY = true; }
            if (CheckActorPoolCollisions(Actor)) { collisionXY = true; }
            //unproject on both X and Y axis
            Actor.compCollision.rec.X = (int)Actor.compMove.position.X + Actor.compCollision.offsetX;
            Actor.compCollision.rec.Y = (int)Actor.compMove.position.Y + Actor.compCollision.offsetY;
            
            if(collisionXY) //actor has collided with obj/actor, on atleast one axis, determine which axis
            {   
                //project collisionRec on X axis
                Actor.compCollision.rec.X = (int)Actor.compMove.newPosition.X + Actor.compCollision.offsetX;
                //check object and actor collisions/interactions
                if (CheckObjPoolCollisions(Actor)) { collisionX = true; }
                if (CheckActorPoolCollisions(Actor)) { collisionX = true; }
                //unproject collisionRec on X axis
                Actor.compCollision.rec.X = (int)Actor.compMove.position.X + Actor.compCollision.offsetX;

                //project collisionRec on Y axis
                Actor.compCollision.rec.Y = (int)Actor.compMove.newPosition.Y + Actor.compCollision.offsetY;
                //check object and actor collisions/interactions
                if (CheckObjPoolCollisions(Actor)) { collisionY = true; }
                if (CheckActorPoolCollisions(Actor)) { collisionY = true; }
                //unproject collisionRec on Y axis
                Actor.compCollision.rec.Y = (int)Actor.compMove.position.Y + Actor.compCollision.offsetY;

                //if there was a collision, the new position reverts to the old position
                if (collisionX) { Actor.compMove.newPosition.X = Actor.compMove.position.X; }
                if (collisionY) { Actor.compMove.newPosition.Y = Actor.compMove.position.Y; }
                //we know there was a X&Y collision, but per axis check doesn't catch it, revert to old pos
                if (!collisionX & !collisionY) { Actor.compMove.newPosition = Actor.compMove.position; }
            }

            //the current position becomes the new position
            Actor.compMove.position.X = Actor.compMove.newPosition.X;
            Actor.compMove.position.Y = Actor.compMove.newPosition.Y;
        }

        public static void CheckCollisions(GameObject gameObject)
        {
            collisionX = false; collisionY = false; collisionXY = false;


            //this is done on one axis right now, but it should be done PER AXIS in the near future
            //project
            gameObject.compCollision.rec.X = (int)gameObject.compMove.newPosition.X + gameObject.compCollision.offsetX;
            gameObject.compCollision.rec.Y = (int)gameObject.compMove.newPosition.Y + gameObject.compCollision.offsetY;
            //check collisions
            if (CheckObjPoolCollisions(gameObject)) { collisionXY = true; }
            if (CheckActorPoolCollisions(gameObject)) { collisionXY = true; }
            //unproject
            gameObject.compCollision.rec.X = (int)gameObject.compMove.position.X + gameObject.compCollision.offsetX;
            gameObject.compCollision.rec.Y = (int)gameObject.compMove.position.Y + gameObject.compCollision.offsetY;

            //if there was a collision, the new position reverts to the old position
            if (collisionXY) { gameObject.compMove.newPosition = gameObject.compMove.position; }


            /*
             
            
            1axis
            project the object
            check collisions with non-blocking objs - INTERACT() - (may push)
            check collisions with blocking objs - INTERACT() - (any overlap will reset position)
            check collisions with actor pool
            unproject, resolve position

            2axis
            project XY, check blocking collisions with roomObjs + actors (compColl.blocking), 
            if any block happened
                unproject XY 
                check per axis, to allow sliding..
                project X, check blocking collisions, flip booleanX?, unproject X
                project Y, check blocking collisions, flip booleanY?, unproject Y
            resolve position / movement based on booleans
            NOW check non-blocking obj collision + interaction
                if an interaction happens that alters position, we'll need to resolve movement again

            2axis faster
            when we project the XY, store a list of compMove references to all overlapping objs/actors
            then when we determine per axis, we can just check against the overlapping compMove list,
            instead of the entire list of objs/actors, in order to resolve a blocking movement.
            the first time thru the actors/objs, we can pull out only the data we actually need.
            this should reduce the runtime of the algorithm by a little bit.
            
            
            
            
            //project collisionRec on X axis
            gameObject.compCollision.rec.X = (int)gameObject.compMove.newPosition.X + gameObject.compCollision.offsetX;
            //check object collisions/interactions
            // WE SHOULDNT BE DOING THIS TWICE!
            if (CheckObjPoolCollisions(gameObject)) { collisionX = true; }
            if (CheckActorPoolCollisions(gameObject)) { collisionX = true; }
            //unproject collisionRec on X axis
            gameObject.compCollision.rec.X = (int)gameObject.compMove.position.X + gameObject.compCollision.offsetX;

            //project collisionRec on Y axis
            gameObject.compCollision.rec.Y = (int)gameObject.compMove.newPosition.Y + gameObject.compCollision.offsetY;
            //check object collisions/interactions
            // WE SHOULDNT BE DOING THIS TWICE!
            if (CheckObjPoolCollisions(gameObject)) { collisionY = true; }
            if (CheckActorPoolCollisions(gameObject)) { collisionY = true; }
            //unproject collisionRec on Y axis
            gameObject.compCollision.rec.Y = (int)gameObject.compMove.position.Y + gameObject.compCollision.offsetY;

            //if there was a collision, the new position reverts to the old position
            if (collisionX) { gameObject.compMove.newPosition.X = gameObject.compMove.position.X; }
            if (collisionY) { gameObject.compMove.newPosition.Y = gameObject.compMove.position.Y; }
            */



            //the current position becomes the new position
            gameObject.compMove.position.X = gameObject.compMove.newPosition.X;
            gameObject.compMove.position.Y = gameObject.compMove.newPosition.Y;
        }
        
    }
}