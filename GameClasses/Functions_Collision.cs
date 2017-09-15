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



        public static void CheckRoomCollision()
        {

            #region Handle Hero transferring between Level.Rooms

            for (i = 0; i < Level.rooms.Count; i++)
            {   //if the current room is not the room we are checking against, then continue
                if (Functions_Level.currentRoom != Level.rooms[i])
                {   //if heroRec collides with room rec, set it as currentRoom, build room
                    if (Pool.heroRec.Intersects(Level.rooms[i].rec))
                    {
                        Functions_Level.currentRoom = Level.rooms[i];
                        Level.rooms[i].visited = true;
                        Functions_Room.BuildRoom(Level.rooms[i]);
                        Functions_Room.FinishRoom(Level.rooms[i]);
                        if (Functions_Level.currentRoom.type == RoomType.Boss)
                        {   //if hero just entered the boss room, play the boss intro & music
                            Assets.Play(Assets.sfxBossIntro);
                            Functions_Music.PlayMusic(Music.Boss);
                        }
                    }
                }
            }

            #endregion


            #region Track Doors that Hero has visited

            for (i = 0; i < Level.doors.Count; i++)
            {   //check heroRec collision against Level.doors
                if (Pool.heroRec.Intersects(Level.doors[i].rec))
                {   //track doors hero has visited
                    Level.doors[i].visited = true; 
                    if(Level.doors[i].type == DoorType.Open)
                    {   //set the current room's spawnPos to the last open door hero collided with
                        Functions_Level.currentRoom.spawnPos.X = Level.doors[i].rec.X + 8;
                        Functions_Level.currentRoom.spawnPos.Y = Level.doors[i].rec.Y + 8;
                    }
                } 
            }

            #endregion


            #region Open/Close Doors for Hero

            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active) //roomObj must be active
                {
                    if(Pool.roomObjPool[i].type == ObjType.DoorOpen)
                    {   //set open/bombed doors to blocking or non-blocking
                        Pool.roomObjPool[i].compCollision.blocking = true; //set door blocking

                        //compare hero to door positions, unblock door if hero is close enough
                        if (Math.Abs(Pool.hero.compSprite.position.X - Pool.roomObjPool[i].compSprite.position.X) < 18)
                        {   //compare hero to door sprite positions, unblock door if hero is close enough
                            if (Math.Abs(Pool.hero.compSprite.position.Y - Pool.roomObjPool[i].compSprite.position.Y) < 18)
                            { Pool.roomObjPool[i].compCollision.blocking = false; }
                        }
                        //do this for hero's pet as well
                        if (Math.Abs(Pool.herosPet.compSprite.position.X - Pool.roomObjPool[i].compSprite.position.X) < 18)
                        {
                            if (Math.Abs(Pool.herosPet.compSprite.position.Y - Pool.roomObjPool[i].compSprite.position.Y) < 18)
                            { Pool.roomObjPool[i].compCollision.blocking = false; }
                        }
                    }
                }
            }

            #endregion

        }

        public static Boolean CheckInteractionRecCollisions()
        {   //set the interaction rec to the hero's position + direction
            Functions_Interaction.SetHeroInteractionRec();
            collision = false;
            //check to see if the interactionRec collides with any gameObjects
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active)
                {
                    if (Functions_Interaction.interactionRec.rec.Intersects(Pool.roomObjPool[i].compCollision.rec))
                    {
                        Functions_Movement.StopMovement(Pool.hero.compMove);
                        Pool.hero.stateLocked = true;
                        Pool.hero.lockTotal = 10; //required to show the pickup animation
                        collision = true;
                        //handle the hero interaction, may overwrites hero.lockTotal
                        Functions_Interaction.InteractHero(Pool.roomObjPool[i]);
                    }
                }
            }
            //move the interaction rec offscreen
            Functions_Interaction.ClearHeroInteractionRec();
            return collision;
        }

        

        public static Boolean CheckObjPoolCollisions(Actor Actor)
        {
            collision = false; //assume no collision
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active) //roomObj must be active
                {   //check for overlap
                    if (Actor.compCollision.rec.Intersects(Pool.roomObjPool[i].compCollision.rec))
                    {   //blocking collision returns true, non-blocking collision causes Interaction()
                        if (Pool.roomObjPool[i].compCollision.blocking) { collision = true; }
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
                            {   //handle any actor vs actor interaction, return collision
                                if (Actor == Pool.hero) { Functions_Interaction.InteractHero(Pool.actorPool[i]); }
                                collision = true;
                            }
                        }
                        Pool.collisionsCount++;
                    }
                }
            }
            return collision; 
        }

        public static Boolean CheckObjPoolCollisions(GameObject Entity)
        {
            collision = false; //assume no collision
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active) //roomObj must be active
                {   //check for overlap
                    if (Entity.compCollision.rec.Intersects(Pool.roomObjPool[i].compCollision.rec))
                    {   //roomObjs cant collide with themselves
                        if (Entity != Pool.roomObjPool[i])
                        {   //handle interaction, check to see if collision is blocking
                            Functions_Interaction.InteractObject(Entity, Pool.roomObjPool[i]);
                            if (Pool.roomObjPool[i].compCollision.blocking) { collision = true; }
                        }
                    }
                    Pool.collisionsCount++;
                }
            }
            return collision;
        }

        public static Boolean CheckActorPoolCollisions(GameObject Entity)
        {
            collision = false; //assume no collision
            for (i = 0; i < Pool.actorCount; i++)
            {   //skip checking projectiles against hero's pet
                if (Pool.actorPool[i] == Pool.herosPet) { } //no checking
                else if (Pool.actorPool[i].active) //actor must be active
                {   //check for overlap
                    if (Entity.compCollision.rec.Intersects(Pool.actorPool[i].compCollision.rec))
                    {   //handle interaction
                        Functions_Interaction.InteractActor(Pool.actorPool[i], Entity);
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

    }
}