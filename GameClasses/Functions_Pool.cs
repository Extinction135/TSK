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
        static int listCount = 0;


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
                Functions_Actor.ResetActor(Pool.actorPool[Pool.actorIndex]);
                return Pool.actorPool[Pool.actorIndex];
            }
            return Pool.actorPool[Pool.actorCount-1]; //ran out of actors
        }
        
        public static GameObject GetRoomObj()
        {   
            for (Pool.roomObjCounter = 0; Pool.roomObjCounter < Pool.roomObjCount; Pool.roomObjCounter++)
            {   //found an inactive obj to return
                if (Pool.roomObjPool[Pool.roomObjCounter].active == false)
                {   //reset obj to default state, hide offscreen, return it
                    Functions_GameObject.ResetObject(Pool.roomObjPool[Pool.roomObjCounter]);
                    Pool.roomObjPool[Pool.roomObjCounter].compMove.newPosition.X = -1000;
                    return Pool.roomObjPool[Pool.roomObjCounter];
                }
            }
            return Pool.roomObjPool[Pool.roomObjCount-1]; //ran out of roomObjs
        }

        public static GameObject GetEntity()
        {   //this is called throughout gameplay, and index loops
            for (Pool.entityCounter = 0; Pool.entityCounter < Pool.entityCount; Pool.entityCounter++)
            {   
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



        public static void Update()
        {
            Pool.collisionsCount = 0;
            UpdateGameObjList(Pool.roomObjPool, true);
            UpdateGameObjList(Pool.entityPool, false);
            UpdateActors();
            Functions_Hero.Update();
        }



        public static void UpdateActors()
        {
            for (i = 0; i < Pool.actorCount; i++)
            {
                if (Pool.actorPool[i].active)
                {

                    #region Update, Animate, Scale Actor

                    Functions_Actor.Update(Pool.actorPool[i]);
                    Functions_Animation.Animate(Pool.actorPool[i].compAnim, Pool.actorPool[i].compSprite);
                    Functions_Animation.ScaleSpriteDown(Pool.actorPool[i].compSprite);

                    #endregion


                    #region Check Collisions, Resolve Movement

                    //we want to reject as many actors as possible from these processess, due to their expense
                    //currently, we are accepting all actors that are active
                    //so we can beat up enemy corpses, and they'll move around, slide, etc..
                    //this is nice, but expensive (for now it's ok)

                    Functions_Movement.ProjectMovement(Pool.actorPool[i].compMove);
                    //based on actor, call collision checking with control booleans
                    if (Pool.actorPool[i] == Pool.hero)
                    {
                        Functions_Collision.CheckCollisions(
                            Pool.actorPool[i].compMove,
                            Pool.actorPool[i].compCollision,
                            true, true);
                    }
                    else //all other enemies/actors & pet
                    {
                        Functions_Collision.CheckCollisions(
                            Pool.actorPool[i].compMove,
                            Pool.actorPool[i].compCollision,
                            true, false);
                    }
                    //any obj that moved needs to have their components aligned
                    Functions_Component.Align(Pool.actorPool[i]);

                    #endregion


                    //set actor's friction to normal (interactions with ice will change it later)
                    Pool.actorPool[i].compMove.friction = Pool.actorPool[i].friction;

                    #region Check Interactions, Resolve Movement

                    //handle interactions, align components post-interaction
                    Functions_Interaction.CheckInteractions(Pool.actorPool[i], true, true);
                    Functions_Component.Align(Pool.actorPool[i]);

                    #endregion

                }
            }
        }

        public static void UpdateGameObjList(List<GameObject> ObjList, Boolean isRoomObj)
        {
            listCount = ObjList.Count();
            for (i = 0; i < listCount; i++)
            {
                if (ObjList[i].active)
                {

                    #region Update, Animate, Scale Object

                    Functions_GameObject.Update(ObjList[i]);
                    Functions_Animation.Animate(ObjList[i].compAnim, ObjList[i].compSprite);
                    Functions_Animation.ScaleSpriteDown(ObjList[i].compSprite);

                    #endregion
                    

                    #region Check Collisions, Resolve Movement
                    
                    if(ObjList[i].compMove.moving)
                    {   //project and resolve legal movement
                        Functions_Movement.ProjectMovement(ObjList[i].compMove);

                        if (isRoomObj)
                        {   //check roomObj vs roomObj blocking collisions
                            Functions_Collision.CheckCollisions(
                                ObjList[i].compMove,
                                ObjList[i].compCollision,
                                true, false);
                        }
                        else
                        {   //dont check entity vs roomObj collisions, just move entity
                            ObjList[i].compMove.position.X = ObjList[i].compMove.newPosition.X;
                            ObjList[i].compMove.position.Y = ObjList[i].compMove.newPosition.Y;
                        }
                        
                        //any obj that moved needs their components aligned
                        Functions_Component.Align(ObjList[i]);
                    }

                    #endregion


                    #region Check Interactions, Resolve Movement

                    if (isRoomObj) 
                    {   //aany moving roomObj gets interaction checks
                        //and specific roomObjs ALWAYS get interaction checks
                        if (
                            ObjList[i].compMove.moving
                            || ObjList[i].type == ObjType.ConveyorBeltOn)
                        {   //handle interactions, align components post-interaction
                            Functions_Interaction.CheckInteractions(ObjList[i]);
                            Functions_Component.Align(ObjList[i]);
                        }
                    }
                    else
                    {   //this is an entity
                        //only interaction check entity projectiles
                        if (ObjList[i].group == ObjGroup.Projectile) 
                        {
                            //the above projectile check is SUPER SLOPPY
                            //there are projectiles that will slip thru, like debris
                            //these shouldn't be checked at all
                            //however, its done this way cause some projectiles dont move
                            //and it's a cheaper int check than numerous obj.type checks

                            //handle interactions, align components post-interaction
                            Functions_Interaction.CheckInteractions(ObjList[i]);
                            Functions_Component.Align(ObjList[i]);
                        }
                    }

                    //this completes all roomObj / entity interactions

                    #endregion

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
            //hero's shadow
            Functions_Draw.Draw(Functions_Hero.heroShadow);
        }

    }
}