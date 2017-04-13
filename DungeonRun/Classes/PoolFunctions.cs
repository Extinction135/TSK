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

        public static Actor GetActor()
        {
            Pool.actorIndex++;
            if (Pool.actorIndex == Pool.actorCount) { Pool.actorIndex = 1; } //skip 0th actor (HERO)
            Pool.actorPool[Pool.actorIndex].active = true;
            Pool.actorsUsed++;
            return Pool.actorPool[Pool.actorIndex];
        }

        public static GameObject GetObj()
        {
            Pool.objIndex++;
            if (Pool.objIndex == Pool.objCount) { Pool.objIndex = 0; }
            Pool.objPool[Pool.objIndex].active = true;
            Pool.objsUsed++;
            return Pool.objPool[Pool.objIndex];
        }

        public static GameObject GetProjectile()
        {
            Pool.projectileIndex++;
            if (Pool.projectileIndex >= Pool.projectileCount) { Pool.projectileIndex = 0; }
            Pool.projectilePool[Pool.projectileIndex].active = true;
            Pool.projectilesUsed++;
            return Pool.projectilePool[Pool.projectileIndex];
        }

        public static ComponentSprite GetFloor()
        {
            Pool.floorIndex++;
            if (Pool.floorIndex == Pool.floorCount) { Pool.floorIndex = 0; }
            Pool.floorPool[Pool.floorIndex].visible = true;
            return Pool.floorPool[Pool.floorIndex];
        }



        public static void Reset()
        {
            ResetActorPool();
            ResetObjPool();
            ResetProjectilePool();
            ResetFloorPool();
        }

        public static void ResetActorPool()
        {
            for (Pool.counter = 0; Pool.counter < Pool.actorCount; Pool.counter++)
            { Release(Pool.actorPool[Pool.counter]); }
            Pool.actorsUsed = 0;
        }

        public static void ResetObjPool()
        {
            for (Pool.counter = 0; Pool.counter < Pool.objCount; Pool.counter++)
            { Release(Pool.objPool[Pool.counter]); }
            Pool.objsUsed = 0;
        }

        public static void ResetProjectilePool()
        {
            for (Pool.counter = 0; Pool.counter < Pool.projectileCount; Pool.counter++)
            { Release(Pool.projectilePool[Pool.counter]); }
            Pool.projectilesUsed = 0;
        }

        public static void ResetFloorPool()
        {
            for (Pool.counter = 0; Pool.counter < Pool.floorCount; Pool.counter++)
            { Pool.floorPool[Pool.counter].visible = false; }
        }



        public static void Release(Actor Actor)
        {
            Actor.active = false;
            Actor.compCollision.active = false;
            Pool.actorsUsed--;
        }

        public static void Release(GameObject Obj)
        {
            Obj.active = false;
            Obj.compCollision.active = false;
            Obj.lifetime = 0;
            if (Obj.objGroup == GameObject.ObjGroup.Projectile ||
                Obj.objGroup == GameObject.ObjGroup.Particle)
            { Pool.projectilesUsed--; }
            else { Pool.objsUsed--; }
        }



        public static void Move(DungeonScreen DungeonScreen)
        {   //actor pool
            for (Pool.counter = 0; Pool.counter < Pool.actorCount; Pool.counter++)
            {
                if (Pool.actorPool[Pool.counter].active)
                { MovementFunctions.Move(Pool.actorPool[Pool.counter], DungeonScreen); }
            }
            //obj pool
            for (Pool.counter = 0; Pool.counter < Pool.objCount; Pool.counter++)
            {
                if (Pool.objPool[Pool.counter].active)
                { MovementFunctions.Move(Pool.objPool[Pool.counter], DungeonScreen); }
            }
            //projectile pool
            for (Pool.counter = 0; Pool.counter < Pool.projectileCount; Pool.counter++)
            {
                if (Pool.projectilePool[Pool.counter].active)
                { MovementFunctions.Move(Pool.projectilePool[Pool.counter], DungeonScreen); }
            }
        }



        public static void UpdateActiveActor(Actor Actor)
        {
            Input.ResetInputData(Actor.compInput);
            AiFunctions.Think(Actor.compInput); //pass active actor to AiManager
            Pool.activeActor++;
            if (Pool.activeActor >= Pool.actorCount) { Pool.activeActor = 1; } //skip 0th actor (HERO)
        }

        public static void Update()
        {
            //actor pool
            UpdateActiveActor(Pool.actorPool[Pool.activeActor]);
            UpdateActiveActor(Pool.actorPool[Pool.activeActor]);
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
                {
                    //we don't update the obj pool, because no objs move right now
                    AnimationFunctions.Animate(Pool.objPool[Pool.counter].compAnim, 
                        Pool.objPool[Pool.counter].compSprite);
                }
            }

            //projectile pool
            for (Pool.counter = 0; Pool.counter < Pool.projectileCount; Pool.counter++)
            {
                if (Pool.projectilePool[Pool.counter].active)
                {
                    GameObjectFunctions.Update(Pool.projectilePool[Pool.counter]);
                    AnimationFunctions.Animate(Pool.projectilePool[Pool.counter].compAnim, 
                        Pool.projectilePool[Pool.counter].compSprite);
                }
            }
        }

        public static void Draw()
        {
            //floor pool
            for (Pool.counter = 0; Pool.counter < Pool.floorCount; Pool.counter++)
            {
                if (Pool.floorPool[Pool.counter].visible)
                { DrawFunctions.Draw(Pool.floorPool[Pool.counter]); }
            }

            //obj pool
            for (Pool.counter = 0; Pool.counter < Pool.objCount; Pool.counter++)
            {
                if (Pool.objPool[Pool.counter].active)
                { GameObjectFunctions.Draw(Pool.objPool[Pool.counter]); }
            }

            //projectile pool
            for (Pool.counter = 0; Pool.counter < Pool.projectileCount; Pool.counter++)
            {
                if (Pool.projectilePool[Pool.counter].active)
                { GameObjectFunctions.Draw(Pool.projectilePool[Pool.counter]); }
            }

            //actor pool
            for (Pool.counter = 0; Pool.counter < Pool.actorCount; Pool.counter++)
            {
                if (Pool.actorPool[Pool.counter].active)
                { ActorFunctions.Draw(Pool.actorPool[Pool.counter]); }
            }

            //match hero shadow to hero position + offset, then draw it
            Pool.heroShadow.position.X = Pool.hero.compSprite.position.X;
            Pool.heroShadow.position.Y = Pool.hero.compSprite.position.Y + 5;
            ComponentFunctions.SetZdepth(Pool.heroShadow);
            DrawFunctions.Draw(Pool.heroShadow);
        }

    }
}