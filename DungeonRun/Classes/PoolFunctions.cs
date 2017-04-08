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
    public static class PoolFunctions
    {
        public static Actor GetActor(Pool Pool)
        {
            Actor actor = Pool.actorPool[Pool.actorIndex];
            ActorFunctions.SetType(actor, actor.type);
            Pool.actorIndex++;
            if (Pool.actorIndex > Pool.actorCount) { Pool.actorIndex = 1; } //skip 0th actor (HERO)
            return actor;
        }

        public static GameObject GetObj(Pool Pool)
        {
            GameObject obj = Pool.objPool[Pool.objIndex];
            GameObjectFunctions.SetType(obj, obj.type);
            Pool.objIndex++;
            if (Pool.objIndex > Pool.objCount) { Pool.objIndex = 0; }
            return obj;
        }

        public static GameObject GetProjectile(Pool Pool)
        {
            GameObject projectile = Pool.projectilePool[Pool.projectileIndex];
            GameObjectFunctions.SetType(projectile, projectile.type);
            Pool.projectileIndex++;
            if (Pool.projectileIndex > Pool.projectileCount) { Pool.projectileIndex = 0; }
            return projectile;
        }

        public static ComponentSprite GetFloor(Pool Pool)
        {
            ComponentSprite floor = Pool.floorPool[Pool.floorIndex];
            floor.visible = true;
            Pool.floorIndex++;
            if (Pool.floorIndex > Pool.floorCount) { Pool.floorIndex = 0; }
            return floor;
        }



        public static void Reset(Pool Pool)
        {
            ResetActorPool(Pool);
            ResetObjPool(Pool);
            ResetProjectilePool(Pool);
            ResetFloorPool(Pool);
        }

        public static void ResetActorPool(Pool Pool)
        {
            for (Pool.counter = 0; Pool.counter < Pool.actorCount; Pool.counter++)
            { Release(Pool.actorPool[Pool.counter]); }
        }

        public static void ResetObjPool(Pool Pool)
        {
            for (Pool.counter = 0; Pool.counter < Pool.objCount; Pool.counter++)
            { Release(Pool.objPool[Pool.counter]); }
        }

        public static void ResetProjectilePool(Pool Pool)
        {
            for (Pool.counter = 0; Pool.counter < Pool.projectileCount; Pool.counter++)
            { Release(Pool.projectilePool[Pool.counter]); }
        }

        public static void ResetFloorPool(Pool Pool)
        {
            for (Pool.counter = 0; Pool.counter < Pool.floorCount; Pool.counter++)
            { Pool.floorPool[Pool.counter].visible = false; }
        }



        public static void Release(Actor Actor)
        {
            Actor.active = false;
            Actor.compCollision.active = false;
            Actor.compCollision.rec.X = -1000;
        }

        public static void Release(GameObject Obj)
        {
            Obj.active = false;
            Obj.compCollision.active = false;
            Obj.compCollision.rec.X = -1000;
        }



        public static void Move(DungeonScreen DungeonScreen, Pool Pool)
        {   //actor pool
            for (Pool.counter = 0; Pool.counter < Pool.actorCount; Pool.counter++)
            {
                if (Pool.actorPool[Pool.counter].active)
                { CollisionFunctions.Move(Pool.actorPool[Pool.counter], DungeonScreen); }
            }
            //obj pool
            for (Pool.counter = 0; Pool.counter < Pool.objCount; Pool.counter++)
            {
                if (Pool.objPool[Pool.counter].active)
                { CollisionFunctions.Move(Pool.objPool[Pool.counter], DungeonScreen); }
            }
            //projectile pool
            for (Pool.counter = 0; Pool.counter < Pool.projectileCount; Pool.counter++)
            {
                if (Pool.projectilePool[Pool.counter].active)
                { CollisionFunctions.Move(Pool.projectilePool[Pool.counter], DungeonScreen); }
            }
        }



        public static void UpdateActiveActor(Actor Actor, Pool Pool)
        {
            InputFunctions.ResetInputData(Actor.compInput);
            AiManager.Think(Actor.compInput); //pass active actor to AiManager
            Pool.activeActor++;
            if (Pool.activeActor >= Pool.actorCount) { Pool.activeActor = 1; } //skip 0th actor (HERO)
        }

        public static void Update(Pool Pool)
        {
            //actor pool
            UpdateActiveActor(Pool.actorPool[Pool.activeActor], Pool);
            UpdateActiveActor(Pool.actorPool[Pool.activeActor], Pool);
            for (Pool.counter = 0; Pool.counter < Pool.actorCount; Pool.counter++)
            {
                if (Pool.actorPool[Pool.counter].active)
                {
                    ActorFunctions.Update(Pool.actorPool[Pool.counter]);
                    AnimationFunctions.Animate(Pool.actorPool[Pool.counter].compAnim, 
                        Pool.actorPool[Pool.counter].compSprite);
                }
            }

            //obj pool
            for (Pool.counter = 0; Pool.counter < Pool.objCount; Pool.counter++)
            {
                if (Pool.objPool[Pool.counter].active)
                { AnimationFunctions.Animate(Pool.objPool[Pool.counter].compAnim, 
                    Pool.objPool[Pool.counter].compSprite); }
            }

            //projectile pool
            for (Pool.counter = 0; Pool.counter < Pool.projectileCount; Pool.counter++)
            {
                if (Pool.projectilePool[Pool.counter].active)
                { AnimationFunctions.Animate(Pool.projectilePool[Pool.counter].compAnim, 
                    Pool.projectilePool[Pool.counter].compSprite); }
            }
        }

        public static void Draw(ScreenManager ScreenManager, Pool Pool)
        {
            //floor pool
            for (Pool.counter = 0; Pool.counter < Pool.floorCount; Pool.counter++)
            {
                if (Pool.floorPool[Pool.counter].visible)
                { DrawFunctions.Draw(Pool.floorPool[Pool.counter], ScreenManager); }
            }

            //obj pool
            for (Pool.counter = 0; Pool.counter < Pool.objCount; Pool.counter++)
            {
                if (Pool.objPool[Pool.counter].active)
                { GameObjectFunctions.Draw(Pool.objPool[Pool.counter], ScreenManager); }
            }

            //projectile pool
            for (Pool.counter = 0; Pool.counter < Pool.projectileCount; Pool.counter++)
            {
                if (Pool.projectilePool[Pool.counter].active)
                { GameObjectFunctions.Draw(Pool.projectilePool[Pool.counter], ScreenManager); }
            }

            //actor pool
            for (Pool.counter = 0; Pool.counter < Pool.actorCount; Pool.counter++)
            {
                if (Pool.actorPool[Pool.counter].active)
                { ActorFunctions.Draw(Pool.actorPool[Pool.counter], ScreenManager); }
            }
        }
    }
}