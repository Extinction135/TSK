﻿using System;
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
                        {   //handle interaction, check to see if collision is blocking
                            Functions_Interaction.InteractObject(Object, Pool.roomObjPool[i]);
                            if (Pool.roomObjPool[i].compCollision.blocking) { collision = true; }
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

        public static void CheckCollisions(GameObject Entity)
        {   //single projection collision check (X&Y axis)
            //project X&Y
            Entity.compCollision.rec.X = (int)Entity.compMove.newPosition.X + Entity.compCollision.offsetX;
            Entity.compCollision.rec.Y = (int)Entity.compMove.newPosition.Y + Entity.compCollision.offsetY;
            //check actor & object collisions/interactions
            if (CheckActorPoolCollisions(Entity)) { }
            if (CheckObjPoolCollisions(Entity)) { }
            //unproject X&Y
            Entity.compCollision.rec.X = (int)Entity.compMove.position.X + Entity.compCollision.offsetX;
            Entity.compCollision.rec.Y = (int)Entity.compMove.position.Y + Entity.compCollision.offsetY;
            //the current position becomes the new position
            Entity.compMove.position.X = Entity.compMove.newPosition.X;
            Entity.compMove.position.Y = Entity.compMove.newPosition.Y;
        }

        public static void CheckRoomObjCollisions(GameObject RoomObj)
        {
            collisionX = false; collisionY = false;

            //project collisionRec on X axis
            RoomObj.compCollision.rec.X = (int)RoomObj.compMove.newPosition.X + RoomObj.compCollision.offsetX;
            //check object collisions/interactions
            if (CheckObjPoolCollisions(RoomObj)) { collisionX = true; }
            //unproject collisionRec on X axis
            RoomObj.compCollision.rec.X = (int)RoomObj.compMove.position.X + RoomObj.compCollision.offsetX;

            //project collisionRec on Y axis
            RoomObj.compCollision.rec.Y = (int)RoomObj.compMove.newPosition.Y + RoomObj.compCollision.offsetY;
            //check object collisions/interactions
            if (CheckObjPoolCollisions(RoomObj)) { collisionY = true; }
            //unproject collisionRec on Y axis
            RoomObj.compCollision.rec.Y = (int)RoomObj.compMove.position.Y + RoomObj.compCollision.offsetY;

            //the current position becomes the new position
            RoomObj.compMove.position.X = RoomObj.compMove.newPosition.X;
            RoomObj.compMove.position.Y = RoomObj.compMove.newPosition.Y;
        }
        
    }
}