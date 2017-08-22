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



        public static void CheckLevelRoomCollisions()
        {
            for (i = 0; i < Level.rooms.Count; i++)
            {   //if the current room is not the room we are checking against, then continue
                if (Functions_Level.currentRoom != Level.rooms[i])
                {   //if hero collides with this room rec, set it to be currentRoom, build the room
                    if (Pool.hero.compCollision.rec.Intersects(Level.rooms[i].rec))
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
                if (Pool.roomObjPool[i].active) //roomObj must be active
                {   //check for overlap
                    if (Actor.compCollision.rec.Intersects(Pool.roomObjPool[i].compCollision.rec))
                    {   //handle interaction, check to see if collision is blocking
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
                if (Pool.actorPool[i].active) //actor must be active
                {   //only check collisions with hero
                    if (Actor.type == ActorType.Hero || Pool.actorPool[i].type == ActorType.Hero)
                    {   //check for overlap
                        if (Actor.compCollision.rec.Intersects(Pool.actorPool[i].compCollision.rec))
                        {   //actors cannot collide with themselves
                            if (Actor != Pool.actorPool[i]) { collision = true; }
                        }
                    }
                }
            }
            return collision; 
        }

        public static Boolean CheckObjPoolCollisions(GameObject Obj)
        {
            collision = false; //assume no collision
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active) //roomObj must be active
                {   //check for overlap
                    if (Obj.compCollision.rec.Intersects(Pool.roomObjPool[i].compCollision.rec))
                    {   //roomObjs cant collide with themselves
                        if (Obj != Pool.roomObjPool[i])
                        {   //handle interaction, check to see if collision is blocking
                            Functions_Interaction.InteractObject(Obj, Pool.roomObjPool[i]);
                            if (Pool.roomObjPool[i].compCollision.blocking) { collision = true; }
                        }
                    }
                }
            }
            return collision;
        }

        public static Boolean CheckActorPoolCollisions(GameObject Obj)
        {
            collision = false; //assume no collision
            for (i = 0; i < Pool.actorCount; i++)
            {
                if (Pool.actorPool[i].active) //actor must be active
                {   //check for overlap
                    if (Obj.compCollision.rec.Intersects(Pool.actorPool[i].compCollision.rec))
                    {   //handle interaction, actors are always blocking
                        Functions_Interaction.InteractActor(Pool.actorPool[i], Obj);
                        collision = true;
                    }
                }
            }
            return collision;
        }





        /*
        public static Boolean CheckEntityPoolCollisions(GameObject Obj)
        {
            collision = false; //assume no collision
            for (i = 0; i < Pool.entityCount; i++)
            {
                if (Pool.entityPool[i].active) //entityObj must be active
                {   //check for overlap
                    if (Obj.compCollision.rec.Intersects(Pool.entityPool[i].compCollision.rec))
                    {   //projectiles cant collide with themselves
                        if (Obj != Pool.entityPool[i])
                        {   //handle interaction, check to see if collision is blocking
                            Functions_Interaction.InteractObject(Obj, Pool.entityPool[i]);
                            if (Pool.entityPool[i].compCollision.blocking) { collision = true; }
                        }
                    }
                }
            }
            return collision;
        }
        */





        

        public static void CheckCollisions(Actor Actor)
        {   //most frames, an actor isn't colliding with anything
            collisionX = false; collisionY = false; //check per axis
            collisionXY = false; //and diagonal check

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

        public static void CheckCollisions(GameObject Obj)
        {
            collisionX = false; collisionY = false; //check per axis

            //project X
            Obj.compCollision.rec.X = (int)Obj.compMove.newPosition.X + Obj.compCollision.offsetX;
            //check actor, object, entity collisions/interactions
            if (CheckActorPoolCollisions(Obj)) { collisionX = true; }
            if (CheckObjPoolCollisions(Obj)) { collisionX = true; }
            //if (CheckEntityPoolCollisions(Obj)) { collisionX = true; }
            //unproject X
            Obj.compCollision.rec.X = (int)Obj.compMove.position.X + Obj.compCollision.offsetX;

            //project Y
            Obj.compCollision.rec.Y = (int)Obj.compMove.newPosition.Y + Obj.compCollision.offsetY;
            //check actor, object, entity collisions/interactions
            if (CheckActorPoolCollisions(Obj)) { collisionY = true; }
            if (CheckObjPoolCollisions(Obj)) { collisionY = true; }
            //if (CheckEntityPoolCollisions(Obj)) { collisionY = true; }
            //unproject Y
            Obj.compCollision.rec.Y = (int)Obj.compMove.position.Y + Obj.compCollision.offsetY;

            //if there was a collision, revert to previous position, per axis
            if (collisionX) { Obj.compMove.newPosition.X = Obj.compMove.position.X; }
            if (collisionY) { Obj.compMove.newPosition.Y = Obj.compMove.position.Y; }

            //the current position becomes the new position
            Obj.compMove.position.X = Obj.compMove.newPosition.X;
            Obj.compMove.position.Y = Obj.compMove.newPosition.Y;
        }

    }
}