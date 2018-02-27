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
        static Boolean collisionX;
        static Boolean collisionY;
        static Boolean collisionXY;


        public static Boolean CheckRoomObjs(ComponentCollision CompColl)
        { 
            for (i = 0; i < Pool.roomObjCount; i++)
            {   //target must be active AND blocking
                if (Pool.roomObjPool[i].active && Pool.roomObjPool[i].compCollision.blocking) 
                {   //count, check for overlap, return true on FIRST collision
                    Pool.collisionsCount++;
                    if (CompColl.rec.Intersects(Pool.roomObjPool[i].compCollision.rec))
                    { if (CompColl != Pool.roomObjPool[i].compCollision) { return true; } }
                }
            }
            return false;
        }

        public static Boolean CheckActors(ComponentCollision CompColl)
        {
            for (i = 0; i < Pool.actorCount; i++)
            {   //target must be active AND blocking
                if (Pool.actorPool[i].active && Pool.actorPool[i].compCollision.blocking)
                {   //count, check for overlap, return true on FIRST collision
                    Pool.collisionsCount++;
                    if (CompColl.rec.Intersects(Pool.actorPool[i].compCollision.rec))
                    { if (CompColl != Pool.actorPool[i].compCollision) { return true; } }
                }
            }
            return false;
        }

        public static void CheckCollisions(
            ComponentMovement CompMove, ComponentCollision CompColl,
            Boolean checkRoomObjs, Boolean checkActors)
        {
            collisionXY = false; //project on both X and Y axis
            CompColl.rec.X = (int)CompMove.newPosition.X + CompColl.offsetX;
            CompColl.rec.Y = (int)CompMove.newPosition.Y + CompColl.offsetY;
            //check blocking collisions based on parameters
            if (checkRoomObjs) { if (CheckRoomObjs(CompColl)) { collisionXY = true; } }
            if (checkActors) { if (CheckActors(CompColl)) { collisionXY = true; } }
            //unproject on both X and Y axis
            CompColl.rec.X = (int)CompMove.position.X + CompColl.offsetX;
            CompColl.rec.Y = (int)CompMove.position.Y + CompColl.offsetY;

            if (collisionXY) //diagonal collision, determine which axis
            {
                collisionX = false; collisionY = false;
                //project, check collisions, unproject - X axis
                CompColl.rec.X = (int)CompMove.newPosition.X + CompColl.offsetX;
                if (checkRoomObjs) { if (CheckRoomObjs(CompColl)) { collisionX = true; } }
                if (checkActors) { if (CheckActors(CompColl)) { collisionX = true; } }
                CompColl.rec.X = (int)CompMove.position.X + CompColl.offsetX;

                //project, check collisions, unproject - Y axis
                CompColl.rec.Y = (int)CompMove.newPosition.Y + CompColl.offsetY;
                if (checkRoomObjs) { if (CheckRoomObjs(CompColl)) { collisionY = true; } }
                if (checkActors) { if (CheckActors(CompColl)) { collisionY = true; } }
                CompColl.rec.Y = (int)CompMove.position.Y + CompColl.offsetY;

                //if there was a collision, the new position reverts to the old position
                if (collisionX) { CompMove.newPosition.X = CompMove.position.X; }
                if (collisionY) { CompMove.newPosition.Y = CompMove.position.Y; }
                //we know there was a X&Y collision, but per axis check doesn't catch it, revert to old pos
                if (!collisionX & !collisionY) { CompMove.newPosition = CompMove.position; }
            }

            //the current position becomes the new position
            CompMove.position.X = CompMove.newPosition.X;
            CompMove.position.Y = CompMove.newPosition.Y;
        }

    }
}