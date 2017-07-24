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
    public static class Functions_Pool
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
                return Pool.actorPool[Pool.actorIndex];
            }
            return null;
        }
        
        public static GameObject GetRoomObj()
        {   //only class that calls GetObj() is DungeonFunctions, during room building
            Pool.roomObjIndex++;
            if (Pool.roomObjIndex == Pool.roomObjCount) { Pool.roomObjIndex = 0; }
            //reset obj to default state, hide offscreen
            Functions_GameObject.ResetObject(Pool.roomObjPool[Pool.roomObjIndex]);
            Pool.roomObjPool[Pool.roomObjIndex].compMove.newPosition.X = -1000;
            return Pool.roomObjPool[Pool.roomObjIndex];
        }

        public static GameObject GetEntity()
        {   //this is called throughout room gameplay, and will loop/reset
            Pool.entityIndex++;
            if (Pool.entityIndex >= Pool.entityCount) { Pool.entityIndex = 0; }
            //reset projectile to default state, hide offscreen
            Functions_GameObject.ResetObject(Pool.entityPool[Pool.entityIndex]);
            Pool.entityPool[Pool.entityIndex].compMove.newPosition.X = -1000;
            return Pool.entityPool[Pool.entityIndex];
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
            ResetRoomObjPool();
            ResetEntityPool();
            ResetFloorPool();
        }

        public static void ResetActorPool()
        {   //we skip resetting the hero
            for (Pool.counter = 1; Pool.counter < Pool.actorCount; Pool.counter++)
            { Release(Pool.actorPool[Pool.counter]); }
            Pool.actorIndex = 1;
        }

        public static void ResetRoomObjPool()
        {
            for (Pool.counter = 0; Pool.counter < Pool.roomObjCount; Pool.counter++)
            { Release(Pool.roomObjPool[Pool.counter]); }
            Pool.roomObjIndex = 0;
        }

        public static void ResetEntityPool()
        {
            for (Pool.counter = 0; Pool.counter < Pool.entityCount; Pool.counter++)
            { Release(Pool.entityPool[Pool.counter]); }
            Pool.entityIndex = 0;
        }

        public static void ResetFloorPool()
        {
            for (Pool.counter = 0; Pool.counter < Pool.floorCount; Pool.counter++)
            { Pool.floorPool[Pool.counter].visible = false; }
        }

        public static void ResetActorPoolInput()
        {
            for (Pool.counter = 0; Pool.counter < Pool.actorCount; Pool.counter++)
            { Functions_Input.ResetInputData(Pool.actorPool[Pool.counter].compInput); }
        }

        public static void Release(Actor Actor)
        {
            Actor.active = false;
            Actor.compCollision.active = false;
        }

        public static void Release(GameObject Obj)
        {
            Obj.active = false;
            Obj.compCollision.active = false;
            Obj.lifetime = 0;
        }


        
        public static void SetFloorTexture(Texture2D Texture)
        {   //set the floor pool texture
            for (Pool.counter = 0; Pool.counter < Pool.floorCount; Pool.counter++)
            { Pool.floorPool[Pool.counter].texture = Texture; }
        }

        public static void UpdateRoomObjPool()
        {   //align sprite + collision comps to move comp of all active objs
            for (Pool.counter = 0; Pool.counter < Pool.roomObjCount; Pool.counter++)
            {
                if (Pool.roomObjPool[Pool.counter].active)
                {   //align the sprite and collision components to the move component
                    Functions_Component.Align(
                        Pool.roomObjPool[Pool.counter].compMove, 
                        Pool.roomObjPool[Pool.counter].compSprite, 
                        Pool.roomObjPool[Pool.counter].compCollision);
                    //set the current animation frame, check the animation counter
                    Functions_Animation.Animate(Pool.roomObjPool[Pool.counter].compAnim,
                        Pool.roomObjPool[Pool.counter].compSprite);
                    //set the rotation for the obj's sprite
                    Functions_GameObject.SetRotation(Pool.roomObjPool[Pool.counter]);
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
                { Functions_Movement.Move(Pool.actorPool[Pool.counter]); }
            }
            for (Pool.counter = 0; Pool.counter < Pool.entityCount; Pool.counter++)
            {   //move projectiles in projectile pool that are active
                if (Pool.entityPool[Pool.counter].active)
                { Functions_Movement.Move(Pool.entityPool[Pool.counter]); }
            }
            for (Pool.counter = 0; Pool.counter < Pool.roomObjCount; Pool.counter++)
            {   //if this object is active, and it isn't blocking, then move it
                if (Pool.roomObjPool[Pool.counter].active)
                {   //only non-blocking objects get moved, collision checked, and interaction handled
                    if (!Pool.roomObjPool[Pool.counter].compCollision.blocking)
                    { Functions_Movement.Move(Pool.roomObjPool[Pool.counter]); }
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
                    Functions_Actor.Update(Pool.actorPool[Pool.counter]);
                    Functions_Animation.Animate(Pool.actorPool[Pool.counter].compAnim, 
                        Pool.actorPool[Pool.counter].compSprite);
                }
            }
            //room obj pool
            for (Pool.counter = 0; Pool.counter < Pool.roomObjCount; Pool.counter++)
            {
                if (Pool.roomObjPool[Pool.counter].active)
                {
                    Functions_GameObject.Update(Pool.roomObjPool[Pool.counter]);
                    Functions_Animation.Animate(Pool.roomObjPool[Pool.counter].compAnim, 
                        Pool.roomObjPool[Pool.counter].compSprite);
                }
            }
            //projectile pool
            for (Pool.counter = 0; Pool.counter < Pool.entityCount; Pool.counter++)
            {
                if (Pool.entityPool[Pool.counter].active)
                {  
                    Functions_GameObject.Update(Pool.entityPool[Pool.counter]);
                    Functions_Animation.Animate(Pool.entityPool[Pool.counter].compAnim, 
                        Pool.entityPool[Pool.counter].compSprite);
                }
            }
            Move();
        }

        public static void Draw()
        {
            //floor pool
            for (Pool.counter = 0; Pool.counter < Pool.floorCount; Pool.counter++)
            { Functions_Draw.Draw(Pool.floorPool[Pool.counter]); }

            //obj pool
            for (Pool.counter = 0; Pool.counter < Pool.roomObjCount; Pool.counter++)
            { Functions_Draw.Draw(Pool.roomObjPool[Pool.counter]); }

            //projectile pool
            for (Pool.counter = 0; Pool.counter < Pool.entityCount; Pool.counter++)
            { Functions_Draw.Draw(Pool.entityPool[Pool.counter]); }

            //actor pool
            for (Pool.counter = 0; Pool.counter < Pool.actorCount; Pool.counter++)
            { Functions_Draw.Draw(Pool.actorPool[Pool.counter]); }

            //match hero shadow to hero position + offset, then draw it
            Pool.heroShadow.position.X = Pool.hero.compSprite.position.X;
            Pool.heroShadow.position.Y = Pool.hero.compSprite.position.Y + 5;
            Functions_Component.SetZdepth(Pool.heroShadow);
            Functions_Draw.Draw(Pool.heroShadow);
        }

    }
}