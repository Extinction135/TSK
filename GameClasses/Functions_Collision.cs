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






        //check against indestructible objects
        public static Boolean CheckIndestructibles(ComponentCollision CompColl)
        {
            for (i = 0; i < Pool.indObjCount; i++)
            {   //target must be active AND blocking
                if (Pool.indObjPool[i].active && Pool.indObjPool[i].compCollision.blocking)
                {   //check for overlap, return true on FIRST collision
                    Pool.collisions_Possible++; //possible collision up next
                    if (CompColl.rec.Intersects(Pool.indObjPool[i].compCollision.rec))
                    {   //count this as a completed collision check
                        if (CompColl != Pool.indObjPool[i].compCollision)
                        { Pool.collisions_ThisFrame++; return true; }
                    }
                }
            }
            return false;
        }

        






        //check against interactive objects
        public static Boolean CheckInteractives(ComponentCollision CompColl)
        {
            for (i = 0; i < Pool.intObjCount; i++)
            {   //target must be active AND blocking
                if (Pool.intObjPool[i].active && Pool.intObjPool[i].compCollision.blocking)
                {   //check for overlap, return true on FIRST collision
                    Pool.collisions_Possible++; //possible collision up next
                    if (CompColl.rec.Intersects(Pool.intObjPool[i].compCollision.rec))
                    {
                        if (CompColl != Pool.intObjPool[i].compCollision)
                        { Pool.collisions_ThisFrame++; return true; }
                    }
                }
            }
            return false;
        }


















        public static Boolean CheckEnemies(ComponentCollision CompColl)
        {   //we skip hero (0) and hero's pet (1)
            for (i = 2; i < Pool.actorCount; i++)
            {   //target must be active AND blocking
                if (Pool.actorPool[i].active && Pool.actorPool[i].compCollision.blocking)
                {   //count, check for overlap, return true on FIRST collision
                    Pool.collisions_Possible++; //possible collision up next
                    if (CompColl.rec.Intersects(Pool.actorPool[i].compCollision.rec))
                    { if (CompColl != Pool.actorPool[i].compCollision)
                        { Pool.collisions_ThisFrame++; return true; }
                    }
                }
            }
            return false;
        }

        public static Boolean CheckHero(ComponentCollision CompColl)
        {
            Pool.collisions_Possible++; //possible collision up next
            if (CompColl.rec.Intersects(Pool.hero.compCollision.rec))
            { Pool.collisions_ThisFrame++; return true; }
            else { return false; }
        }











        public static void CheckCollisions(
            ComponentMovement CompMove, ComponentCollision CompColl,
            Boolean checkIndestructibles,
            Boolean checkInteractives,
            Boolean checkEnemies, 
            Boolean checkHero)
        {
            collisionXY = false; //project on both X and Y axis
            CompColl.rec.X = (int)CompMove.newPosition.X + CompColl.offsetX;
            CompColl.rec.Y = (int)CompMove.newPosition.Y + CompColl.offsetY;
            //check blocking collisions based on parameters
            if (checkIndestructibles) { if (CheckIndestructibles(CompColl)) { collisionXY = true; } }
            if (checkInteractives) { if (CheckInteractives(CompColl)) { collisionXY = true; } }
            if (checkEnemies) { if (CheckEnemies(CompColl)) { collisionXY = true; } }
            if (checkHero) { if (CheckHero(CompColl)) { collisionXY = true; } }
            //unproject on both X and Y axis
            CompColl.rec.X = (int)CompMove.position.X + CompColl.offsetX;
            CompColl.rec.Y = (int)CompMove.position.Y + CompColl.offsetY;

            if (collisionXY) //diagonal collision, determine which axis
            {
                collisionX = false; collisionY = false;
                //project, check collisions, unproject - X axis
                CompColl.rec.X = (int)CompMove.newPosition.X + CompColl.offsetX;
                if (checkIndestructibles) { if (CheckIndestructibles(CompColl)) { collisionX = true; } }
                if (checkInteractives) { if (CheckInteractives(CompColl)) { collisionX = true; } }
                if (checkEnemies) { if (CheckEnemies(CompColl)) { collisionX = true; } }
                if (checkHero) { if (CheckHero(CompColl)) { collisionX = true; } }
                CompColl.rec.X = (int)CompMove.position.X + CompColl.offsetX;

                //project, check collisions, unproject - Y axis
                CompColl.rec.Y = (int)CompMove.newPosition.Y + CompColl.offsetY;
                if (checkIndestructibles) { if (CheckIndestructibles(CompColl)) { collisionY = true; } }
                if (checkInteractives) { if (CheckInteractives(CompColl)) { collisionY = true; } }
                if (checkEnemies) { if (CheckEnemies(CompColl)) { collisionY = true; } }
                if (checkHero) { if (CheckHero(CompColl)) { collisionY = true; } }
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