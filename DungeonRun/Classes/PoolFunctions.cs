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
            //if the target actor is dead, set it to be inactive
            if(Pool.actorPool[Pool.actorIndex].state == ActorState.Dead)
            { Release(Pool.actorPool[Pool.actorIndex]); }
            //only return inactive actors (dead actors became inactive above)
            if (!Pool.actorPool[Pool.actorIndex].active)
            {
                Pool.actorPool[Pool.actorIndex].active = true;
                Pool.actorsUsed++;
                return Pool.actorPool[Pool.actorIndex];
            }
            return null;
        }

        public static GameObject GetObj()
        {
            Pool.objIndex++;
            if (Pool.objIndex == Pool.objCount) { Pool.objIndex = 0; }


            //only return inactive objects
            if(!Pool.objPool[Pool.objIndex].active)
            {
                Pool.objPool[Pool.objIndex].active = true;
                Pool.objsUsed++;
                return Pool.objPool[Pool.objIndex];
            }
            return null; //this could cause a crash in the future
        }

        public static GameObject GetProjectile()
        {
            Pool.projectileIndex++;
            if (Pool.projectileIndex >= Pool.projectileCount) { Pool.projectileIndex = 0; }


            Pool.projectilePool[Pool.projectileIndex].active = true;
            //tell the projectile to hide offscreen, intially
            Pool.projectilePool[Pool.projectileIndex].compMove.newPosition.X = -1000;
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
        {   //we skip resetting the hero
            for (Pool.counter = 1; Pool.counter < Pool.actorCount; Pool.counter++)
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

        public static void ResetActorPoolInput()
        {
            for (Pool.counter = 0; Pool.counter < Pool.actorCount; Pool.counter++)
            { Input.ResetInputData(Pool.actorPool[Pool.counter].compInput); }
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
            if (Obj.group == ObjGroup.Projectile ||
                Obj.group == ObjGroup.Particle ||
                Obj.group == ObjGroup.Item)
            { Pool.projectilesUsed--; }
            else { Pool.objsUsed--; }
        }



        public static void UpdateObjectPool()
        {   //align sprite + collision comps to move comp of all active objs
            for (Pool.counter = 0; Pool.counter < Pool.objCount; Pool.counter++)
            {
                if (Pool.objPool[Pool.counter].active)
                {   //align the sprite and collision components to the move component
                    ComponentFunctions.Align(
                        Pool.objPool[Pool.counter].compMove, 
                        Pool.objPool[Pool.counter].compSprite, 
                        Pool.objPool[Pool.counter].compCollision);
                    //set the current animation frame, check the animation counter
                    AnimationFunctions.Animate(Pool.objPool[Pool.counter].compAnim,
                        Pool.objPool[Pool.counter].compSprite);
                }
            }
        }

        public static void Move()
        {
            for (Pool.counter = 0; Pool.counter < Pool.actorCount; Pool.counter++)
            {   //move actors in actor pool that are active and alive
                if (
                    Pool.actorPool[Pool.counter].active &&
                    Pool.actorPool[Pool.counter].state != ActorState.Dead
                    )
                { MovementFunctions.Move(Pool.actorPool[Pool.counter]); }
            }
            for (Pool.counter = 0; Pool.counter < Pool.projectileCount; Pool.counter++)
            {   //move projectiles in projectile pool that are active
                if (Pool.projectilePool[Pool.counter].active)
                { MovementFunctions.Move(Pool.projectilePool[Pool.counter]); }
            }
            for (Pool.counter = 0; Pool.counter < Pool.objCount; Pool.counter++)
            {   //if this object is active, and it isn't blocking, then move it
                if (Pool.objPool[Pool.counter].active)
                {   //only non-blocking objects get moved, collision checked, and interaction handled
                    if (!Pool.objPool[Pool.counter].compCollision.blocking)
                    { MovementFunctions.Move(Pool.objPool[Pool.counter]); }
                }
            }
        }



        public static void Update()
        {
            //actor pool
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
                {   //update only counts the object's lifetime, & no obj's have a lifetime
                    //GameObjectFunctions.Update(Pool.objPool[Pool.counter]);
                    AnimationFunctions.Animate(Pool.objPool[Pool.counter].compAnim, 
                        Pool.objPool[Pool.counter].compSprite);
                }
            }
            //projectile pool
            for (Pool.counter = 0; Pool.counter < Pool.projectileCount; Pool.counter++)
            {
                if (Pool.projectilePool[Pool.counter].active)
                {   //all projectiles have a lifetime, so pass them to update
                    GameObjectFunctions.Update(Pool.projectilePool[Pool.counter]);
                    AnimationFunctions.Animate(Pool.projectilePool[Pool.counter].compAnim, 
                        Pool.projectilePool[Pool.counter].compSprite);
                }
            }
            Move();
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