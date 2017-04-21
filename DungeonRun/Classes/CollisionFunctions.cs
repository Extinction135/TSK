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
    public static class CollisionFunctions
    {

        static int i;
        static Boolean collision = false;
        static Boolean collisionX;
        static Boolean collisionY;



        public static void CheckDungeonRoomCollisions()
        {
            for (i = 0; i < DungeonFunctions.dungeon.rooms.Count; i++)
            {   //if the current room is not the room we are checking against, then continue
                if (DungeonFunctions.currentRoom.id != DungeonFunctions.dungeon.rooms[i].id)
                {   //if hero collides with this room rec, set it to be currentRoom, build the room
                    if (Pool.hero.compCollision.rec.Intersects(DungeonFunctions.dungeon.rooms[i].collision.rec))
                    {
                        DungeonFunctions.currentRoom = DungeonFunctions.dungeon.rooms[i];
                        DungeonFunctions.BuildRoom(DungeonFunctions.dungeon.rooms[i]);
                        //if hero just entered the boss room, play the boss intro theme
                        if (DungeonFunctions.currentRoom.type == RoomType.Boss)
                        { Assets.sfxBossIntro.Play(); }
                    }
                }
            }
        }



        public static void CheckInteractionRecCollisions()
        {
            InteractionFunctions.SetHeroInteractionRec();

            //checks to see if the interactionRec collides with any gameObjects
            //passes these collisions to a Handle() function

            //move the interaction rec offscreen
        }





        public static Boolean CheckObjPoolCollisions(Actor Actor)
        {
            collision = false; //assume no collision
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
            return collision;
        }

        public static Boolean CheckActorPoolCollisions(Actor Actor)
        {
            collision = false; //assume no collision
            for (i = 0; i < Pool.actorCount; i++)
            {
                if (Pool.actorPool[i].active)
                {   //ignore actor collisions with the boss actor
                    if (Actor.compCollision.rec.Intersects(Pool.actorPool[i].compCollision.rec))
                    { if (Actor != Pool.actorPool[i]) { collision = true; } }

                    //if this was a collision between a blob and the boss, ignore it
                    if (Actor.type == Actor.Type.Blob && Pool.actorPool[i].type == Actor.Type.Boss)
                    { collision = false; }
                    //if this was a collision between the boss and a blob, ignore it
                    else if (Actor.type == Actor.Type.Boss && Pool.actorPool[i].type == Actor.Type.Blob)
                    { collision = false; }
                }
            }
            return collision; 
        }



        public static void CheckObjPoolCollisions(GameObject Projectile)
        {
            for (i = 0; i < Pool.objCount; i++)
            {
                if (Pool.objPool[i].active)
                {
                    if (Projectile.compCollision.rec.Intersects(Pool.objPool[i].compCollision.rec))
                    { InteractionFunctions.Handle(Projectile, Pool.objPool[i]); }
                }
            }
        }

        public static void CheckActorPoolCollisions(GameObject Projectile)
        {
            for (i = 0; i < Pool.actorCount; i++)
            {
                if (Pool.actorPool[i].active)
                {
                    if (Projectile.compCollision.rec.Intersects(Pool.actorPool[i].compCollision.rec))
                    { InteractionFunctions.Handle(Projectile, Pool.actorPool[i]); }
                }
            }
        }

        public static void CheckProjectilePoolCollisions(GameObject Projectile)
        {
            for (i = 0; i < Pool.projectileCount; i++)
            {
                if (Pool.projectilePool[i].active)
                {
                    if (Projectile.compCollision.rec.Intersects(Pool.projectilePool[i].compCollision.rec))
                    {   //projectiles shouldn't be allowed to collide with themselves
                        if (Projectile != Pool.projectilePool[i])
                        { InteractionFunctions.Handle(Projectile, Pool.projectilePool[i]); }
                    }
                }
            }
        }


        
        public static void CheckCollisions(Actor Actor)
        {
            collisionX = false; collisionY = false;

            //project collisionRec on X axis
            Actor.compCollision.rec.X = (int)Actor.compMove.newPosition.X + Actor.compCollision.offsetX;
            //check object and actor collisions/interactions
            if (CheckObjPoolCollisions(Actor)) { collisionX = true; }
            if (CheckActorPoolCollisions(Actor)) { collisionX = true; }
            //unproject collisionRec on X axis
            Actor.compCollision.rec.X = (int)Actor.compMove.position.X + Actor.compCollision.offsetX;
            
            //project collisionRec on Y axis
            Actor.compCollision.rec.Y = (int)Actor.compMove.newPosition.Y + Actor.compCollision.offsetY;
            //get actor, object, projectile collisions
            //check object and actor collisions/interactions
            if (CheckObjPoolCollisions(Actor)) { collisionY = true; }
            if (CheckActorPoolCollisions(Actor)) { collisionY = true; }
            //unproject collisionRec on Y axis
            Actor.compCollision.rec.Y = (int)Actor.compMove.position.Y + Actor.compCollision.offsetY;

            //if there was a collision, the new position reverts to the old position
            if (collisionX) { Actor.compMove.newPosition.X = Actor.compMove.position.X; }
            if (collisionY) { Actor.compMove.newPosition.Y = Actor.compMove.position.Y; }
            //the current position becomes the new position
            Actor.compMove.position.X = Actor.compMove.newPosition.X;
            Actor.compMove.position.Y = Actor.compMove.newPosition.Y;
        }

        public static void CheckCollisions(GameObject Projectile)
        {
            //do not check collisions for particles
            if (Projectile.objGroup == GameObject.ObjGroup.Particle) { return; }

            collisionX = false; collisionY = false;

            //project collisionRec on X & Y axis
            Projectile.compCollision.rec.X = (int)Projectile.compMove.newPosition.X + Projectile.compCollision.offsetX;
            Projectile.compCollision.rec.Y = (int)Projectile.compMove.newPosition.Y + Projectile.compCollision.offsetY;

            //handle actor, object, projectile collisions/interactions
            CheckObjPoolCollisions(Projectile);
            CheckActorPoolCollisions(Projectile);
            CheckProjectilePoolCollisions(Projectile);

            //the current position becomes the new position
            Projectile.compMove.position.X = Projectile.compMove.newPosition.X;
            Projectile.compMove.position.Y = Projectile.compMove.newPosition.Y;
        }

    }
}