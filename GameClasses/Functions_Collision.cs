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



        public static void CheckLevelRoomCollisions()
        {
            for (i = 0; i < Level.rooms.Count; i++)
            {   //if the current room is not the room we are checking against, then continue
                if (Functions_Dungeon.currentRoom != Level.rooms[i])
                {   //if hero collides with this room rec, set it to be currentRoom, build the room
                    if (Pool.hero.compCollision.rec.Intersects(Level.rooms[i].rec))
                    {
                        Functions_Dungeon.currentRoom = Level.rooms[i];
                        Level.rooms[i].visited = true;
                        Functions_Room.BuildRoom(Level.rooms[i]);
                        Functions_Room.FinishRoom(Level.rooms[i]);
                        if (Functions_Dungeon.currentRoom.type == RoomType.Boss)
                        {   //if hero just entered the boss room, play the boss intro & music
                            Assets.Play(Assets.sfxBossIntro);
                            Functions_Music.PlayMusic(Music.Boss);
                        }
                    }
                }
            }
            //check hero collision against Level.doors too
            for (i = 0; i < Level.doors.Count; i++)
            {
                if (Pool.hero.compCollision.rec.Intersects(Level.doors[i].rec))
                { Level.doors[i].visited = true; } //hero has visited this door
            }
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
                if (Pool.roomObjPool[i].active)
                {
                    if (Actor.compCollision.rec.Intersects(Pool.roomObjPool[i].compCollision.rec))
                    {
                        Functions_Interaction.InteractActor(Actor, Pool.roomObjPool[i]);
                        if (Pool.roomObjPool[i].compCollision.blocking) { collision = true; }
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
                {   //only check collisions with hero
                    if (Actor.type == ActorType.Hero || Pool.actorPool[i].type == ActorType.Hero)
                    {   //check for overlap, actors cannot collide with themselves
                        if (Actor.compCollision.rec.Intersects(Pool.actorPool[i].compCollision.rec))
                        { if (Actor != Pool.actorPool[i]) { collision = true; } }
                    }

                    /* THE OLD WAY
                    //actors always block/collide with each other, but not with themselves
                    if (Actor.compCollision.rec.Intersects(Pool.actorPool[i].compCollision.rec))
                    { if (Actor != Pool.actorPool[i]) { collision = true; } }
                    //if this was a collision between a blob and the boss, ignore it
                    if (Actor.type == ActorType.Blob && Pool.actorPool[i].type == ActorType.Boss)
                    { collision = false; }
                    //if this was a collision between the boss and a blob, ignore it
                    else if (Actor.type == ActorType.Boss && Pool.actorPool[i].type == ActorType.Blob)
                    { collision = false; }
                    */
                }
            }
            return collision; 
        }



        public static void CheckObjPoolCollisions(GameObject Obj)
        {   //Obj could be a projectile/particle from the entity pool
            if (Obj.interacts == false) { return; } //Obj must be interactive
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active)
                {   //roomObj must be active
                    if (Obj.compCollision.rec.Intersects(Pool.roomObjPool[i].compCollision.rec))
                    {   //roomObjs cant collide with themselves
                        if (Obj != Pool.roomObjPool[i])
                        { Functions_Interaction.InteractObject(Obj, Pool.roomObjPool[i]); }
                    }
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
                    { Functions_Interaction.InteractActor(Pool.actorPool[i], Projectile); }
                }
            }
        }

        public static void CheckEntityPoolCollisions(GameObject Projectile)
        {   //Projectile is from from the entity pool
            if (Projectile.interacts == false) { return; } //Projectile must be interactive
            for (i = 0; i < Pool.entityCount; i++)
            {
                if (Pool.entityPool[i].active)
                {   //entityObj must be active & Projectile must be interactive
                    if (Projectile.compCollision.rec.Intersects(Pool.entityPool[i].compCollision.rec))
                    {   //projectiles cant collide with themselves
                        if (Projectile != Pool.entityPool[i])
                        { Functions_Interaction.InteractObject(Projectile, Pool.entityPool[i]); }
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

        public static void CheckCollisions(GameObject Obj)
        {
            //do not check collisions for particles
            if (Obj.group == ObjGroup.Particle) { return; }

            collisionX = false; collisionY = false;

            //project collisionRec on X & Y axis
            Obj.compCollision.rec.X = (int)Obj.compMove.newPosition.X + Obj.compCollision.offsetX;
            Obj.compCollision.rec.Y = (int)Obj.compMove.newPosition.Y + Obj.compCollision.offsetY;

            //handle actor, object, projectile collisions/interactions
            CheckObjPoolCollisions(Obj);
            CheckActorPoolCollisions(Obj);
            CheckEntityPoolCollisions(Obj);

            //the current position becomes the new position
            Obj.compMove.position.X = Obj.compMove.newPosition.X;
            Obj.compMove.position.Y = Obj.compMove.newPosition.Y;
        }

    }
}