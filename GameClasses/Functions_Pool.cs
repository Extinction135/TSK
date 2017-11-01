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
        static int i;



        public static Actor GetActor()
        {
            Pool.actorIndex++;
            //reset index to 2, skipping hero and hero's pet in actor pool
            if (Pool.actorIndex == Pool.actorCount) { Pool.actorIndex = 2; } 
            //if the target actor is dead, set it to be inactive
            if(Pool.actorPool[Pool.actorIndex].state == ActorState.Dead)
            { Release(Pool.actorPool[Pool.actorIndex]); }
            //only return inactive actors (dead actors became inactive above)
            if (!Pool.actorPool[Pool.actorIndex].active)
            {
                Pool.actorPool[Pool.actorIndex].active = true;
                Pool.actorPool[Pool.actorIndex].compSprite.scale = 1.0f;
                return Pool.actorPool[Pool.actorIndex];
            }
            return null;
        }
        
        public static GameObject GetRoomObj()
        {   //called when building/finishing room, index does not loop
            for (Pool.roomObjCounter = 0; Pool.roomObjCounter < Pool.roomObjCount; Pool.roomObjCounter++)
            {
                if (Pool.roomObjPool[Pool.roomObjCounter].active == false)
                {   //found an inactive obj to return
                    //reset obj to default state, hide offscreen, return it
                    Functions_GameObject.ResetObject(Pool.roomObjPool[Pool.roomObjCounter]);
                    Pool.roomObjPool[Pool.roomObjCounter].compMove.newPosition.X = -1000;
                    Pool.roomObjPool[Pool.roomObjCounter].compSprite.scale = 1.0f;
                    return Pool.roomObjPool[Pool.roomObjCounter];
                }
            }
            return Pool.roomObjPool[0]; //ran out of roomObjs
        }

        public static GameObject GetEntity()
        {   //this is called throughout gameplay, and index loops
            for (Pool.entityCounter = 0; Pool.entityCounter < Pool.entityCount; Pool.entityCounter++)
            {   //loop thru the entire entity pool, but use the entityIndex, NOT Pool.counter
                Pool.entityIndex++;
                if (Pool.entityIndex >= Pool.entityCount) { Pool.entityIndex = 0; }
                if (Pool.entityPool[Pool.entityIndex].active == false)
                {   //found an inactive entity to return
                    //reset entity to default state, hide offscreen, return it
                    Functions_GameObject.ResetObject(Pool.entityPool[Pool.entityIndex]);
                    Pool.entityPool[Pool.entityIndex].compMove.newPosition.X = -1000;
                    Pool.entityPool[Pool.entityIndex].compSprite.scale = 1.0f;
                    return Pool.entityPool[Pool.entityIndex];
                }
            }
            return Pool.entityPool[0]; //ran out of entities
        }

        public static ComponentSprite GetFloor()
        {   //we never release a floor sprite, and floors are returned sequentially
            Pool.floorIndex++;
            if (Pool.floorIndex == Pool.floorCount)
            { Pool.floorIndex = Pool.floorCount; } //ran out of floors to return
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
        {   //skip resetting the hero & doggo
            for (Pool.actorCounter = 2; Pool.actorCounter < Pool.actorCount; Pool.actorCounter++)
            { Release(Pool.actorPool[Pool.actorCounter]); }
            Pool.actorIndex = 2;
        }

        public static void ResetRoomObjPool()
        {
            for (Pool.roomObjCounter = 0; Pool.roomObjCounter < Pool.roomObjCount; Pool.roomObjCounter++)
            { Release(Pool.roomObjPool[Pool.roomObjCounter]); }
        }

        public static void ResetEntityPool()
        {
            for (Pool.entityCounter = 0; Pool.entityCounter < Pool.entityCount; Pool.entityCounter++)
            { Release(Pool.entityPool[Pool.entityCounter]); }
        }

        public static void ResetFloorPool()
        {
            for (Pool.floorCounter = 0; Pool.floorCounter < Pool.floorCount; Pool.floorCounter++)
            {
                Pool.floorPool[Pool.floorCounter].visible = false;
                Pool.floorPool[Pool.floorCounter].zDepth = 0.999990f; //sort to lowest level
            }
            Pool.floorIndex = 0; //reset total count
        }

        public static void ResetActorPoolInput()
        {
            for (Pool.actorCounter = 0; Pool.actorCounter < Pool.actorCount; Pool.actorCounter++)
            { Functions_Input.ResetInputData(Pool.actorPool[Pool.actorCounter].compInput); }
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


        
        public static void SetFloorTexture(LevelType Type)
        {   //set the floor pool texture based on dungeon type
            Texture2D Texture = Assets.cursedCastleSheet;

            if (Type == LevelType.Castle) { Texture = Assets.cursedCastleSheet; }
            //expand this to include all dungeon textures...
            else if (Type == LevelType.Shop) { Texture = Assets.shopSheet; }

            //set the floor texture
            for (Pool.floorCounter = 0; Pool.floorCounter < Pool.floorCount; Pool.floorCounter++)
            { Pool.floorPool[Pool.floorCounter].texture = Texture; }
        }

        public static void AlignRoomObjs()
        {   //align sprite + collision comps to move comp of all active objs
            for (Pool.roomObjCounter = 0; Pool.roomObjCounter < Pool.roomObjCount; Pool.roomObjCounter++)
            {
                if (Pool.roomObjPool[Pool.roomObjCounter].active)
                {   //align the sprite and collision components to the move component
                    Functions_Component.Align(
                        Pool.roomObjPool[Pool.roomObjCounter].compMove, 
                        Pool.roomObjPool[Pool.roomObjCounter].compSprite, 
                        Pool.roomObjPool[Pool.roomObjCounter].compCollision);
                    //set the current animation frame, check the animation counter
                    Functions_Animation.Animate(Pool.roomObjPool[Pool.roomObjCounter].compAnim,
                        Pool.roomObjPool[Pool.roomObjCounter].compSprite);
                    //set the rotation for the obj's sprite
                    Functions_GameObject.SetRotation(Pool.roomObjPool[Pool.roomObjCounter]);
                }
            }
        }
        


        public static void Update()
        {
            Pool.collisionsCount = 0;
            UpdateActors();
            UpdateEntities();
            UpdateRoomObjects();
        }

        public static void UpdateActors()
        {
            for (i = 0; i < Pool.actorCount; i++)
            {
                if (Pool.actorPool[i].active)
                {
                    Functions_Actor.Update(Pool.actorPool[i]);
                    Functions_Animation.Animate(Pool.actorPool[i].compAnim, Pool.actorPool[i].compSprite);
                    Functions_Animation.ScaleSpriteDown(Pool.actorPool[i].compSprite);

                    if (Pool.actorPool[i].state != ActorState.Dead)
                    {
                        //project movement
                        Functions_Movement.ProjectMovement(Pool.actorPool[i].compMove);
                        //set actor's friction to normal
                        Pool.actorPool[i].compMove.friction = Pool.actorPool[i].friction;
                        //collision & interaction check
                        Functions_Collision.CheckCollisions(Pool.actorPool[i]);
                        //resolve movement
                        Functions_Component.Align(
                            Pool.actorPool[i].compMove,
                            Pool.actorPool[i].compSprite,
                            Pool.actorPool[i].compCollision);
                    }
                }
            }

            //match hero's rec to hero's sprite
            Pool.heroRec.X = (int)Pool.hero.compSprite.position.X - 8;
            Pool.heroRec.Y = (int)Pool.hero.compSprite.position.Y - 8;
            //check the heroRec's collisions with Level rooms
            Functions_Collision.CheckRoomCollision();
        }

        public static void UpdateEntities()
        {
            for (i = 0; i < Pool.entityCount; i++)
            {
                if (Pool.entityPool[i].active)
                {
                    Functions_GameObject.Update(Pool.entityPool[i]);
                    Functions_Animation.Animate(Pool.entityPool[i].compAnim, Pool.entityPool[i].compSprite);
                    Functions_Animation.ScaleSpriteDown(Pool.entityPool[i].compSprite);

                    //project movement
                    if (Pool.entityPool[i].compMove.moveable)
                    { Functions_Movement.ProjectMovement(Pool.entityPool[i].compMove); }

                    //particles dont get collision checked
                    if (Pool.entityPool[i].group == ObjGroup.Particle) { }
                    else //projectiles + pickups get collision checked
                    { Functions_Collision.CheckCollisions(Pool.entityPool[i]); }

                    //resolve movement
                    Functions_Component.Align(
                        Pool.entityPool[i].compMove,
                        Pool.entityPool[i].compSprite,
                        Pool.entityPool[i].compCollision);
                }
            }
        }

        public static void UpdateRoomObjects()
        {
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active)
                {
                    Functions_GameObject.Update(Pool.roomObjPool[i]);
                    Functions_Animation.Animate(Pool.roomObjPool[i].compAnim,
                        Pool.roomObjPool[i].compSprite);
                    Functions_Animation.ScaleSpriteDown(Pool.roomObjPool[i].compSprite);
                }
            }
        }

        public static void Draw()
        {
            //floor pool
            for (Pool.floorCounter = 0; Pool.floorCounter < Pool.floorCount; Pool.floorCounter++)
            { Functions_Draw.Draw(Pool.floorPool[Pool.floorCounter]); }

            //roomObj pool
            for (Pool.roomObjCounter = 0; Pool.roomObjCounter < Pool.roomObjCount; Pool.roomObjCounter++)
            { Functions_Draw.Draw(Pool.roomObjPool[Pool.roomObjCounter]); }

            //entity pool
            for (Pool.entityCounter = 0; Pool.entityCounter < Pool.entityCount; Pool.entityCounter++)
            { Functions_Draw.Draw(Pool.entityPool[Pool.entityCounter]); }

            //actor pool
            for (Pool.actorCounter = 0; Pool.actorCounter < Pool.actorCount; Pool.actorCounter++)
            { Functions_Draw.Draw(Pool.actorPool[Pool.actorCounter]); }

            //match hero shadow to hero position + offset, then draw it
            Pool.heroShadow.position.X = Pool.hero.compSprite.position.X;
            Pool.heroShadow.position.Y = Pool.hero.compSprite.position.Y + 5;
            Functions_Component.SetZdepth(Pool.heroShadow);
            Functions_Draw.Draw(Pool.heroShadow);
        }

    }
}