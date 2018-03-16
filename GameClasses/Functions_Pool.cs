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
            if (Pool.actorPool[Pool.actorIndex].state == ActorState.Dead)
            { Release(Pool.actorPool[Pool.actorIndex]); }
            //only return inactive actors (dead actors became inactive above)
            if (!Pool.actorPool[Pool.actorIndex].active)
            {
                Functions_Actor.ResetActor(Pool.actorPool[Pool.actorIndex]);
                return Pool.actorPool[Pool.actorIndex];
            }
            return Pool.actorPool[Pool.actorCount - 1]; //ran out of actors
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
            return Pool.roomObjPool[Pool.roomObjCount - 1]; //ran out of roomObjs
        }

        public static GameObject GetParticle()
        {   
            for (Pool.particleCounter = 0; Pool.particleCounter < Pool.particleCount; Pool.particleCounter++)
            {
                Pool.particleIndex++;
                if (Pool.particleIndex >= Pool.particleCount) { Pool.particleIndex = 0; }
                if (Pool.particlePool[Pool.particleIndex].active == false)
                {   //found an inactive to return
                    //reset to default state, hide offscreen, return it
                    Functions_GameObject.ResetObject(Pool.particlePool[Pool.particleIndex]);
                    Pool.particlePool[Pool.particleIndex].compMove.newPosition.X = -1000;
                    Pool.particlePool[Pool.particleIndex].compSprite.scale = 1.0f;
                    return Pool.particlePool[Pool.particleIndex];
                }
            }
            return Pool.particlePool[0]; //ran out
        }

        public static Projectile GetProjectile()
        {
            for (Pool.projectileCounter = 0; Pool.projectileCounter < Pool.projectileCount; Pool.projectileCounter++)
            {
                Pool.projectileIndex++;
                if (Pool.projectileIndex >= Pool.projectileCount) { Pool.projectileIndex = 0; }
                if (Pool.projectilePool[Pool.projectileIndex].active == false)
                {   //found an inactive to return
                    //reset to default state, hide offscreen, return it
                    Functions_GameObject.ResetObject(Pool.projectilePool[Pool.projectileIndex]);
                    Pool.projectilePool[Pool.projectileIndex].compMove.newPosition.X = -1000;
                    Pool.projectilePool[Pool.projectileIndex].compSprite.scale = 1.0f;
                    return Pool.projectilePool[Pool.projectileIndex];
                }
            }
            return Pool.projectilePool[0]; //ran out
        }

        public static GameObject GetPickup()
        {
            for (Pool.pickupCounter = 0; Pool.pickupCounter < Pool.pickupCount; Pool.pickupCounter++)
            {
                Pool.pickupIndex++;
                if (Pool.pickupIndex >= Pool.pickupCount) { Pool.pickupIndex = 0; }
                if (Pool.pickupPool[Pool.pickupIndex].active == false)
                {   //found an inactive to return
                    //reset to default state, hide offscreen, return it
                    Functions_GameObject.ResetObject(Pool.pickupPool[Pool.pickupIndex]);
                    Pool.pickupPool[Pool.pickupIndex].compMove.newPosition.X = -1000;
                    Pool.pickupPool[Pool.pickupIndex].compSprite.scale = 1.0f;
                    return Pool.pickupPool[Pool.pickupIndex];
                }
            }
            return Pool.pickupPool[0]; //ran out
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
            ResetParticlePool();
            ResetProjectilePool();
            ResetPickupPool();
            ResetFloorPool();
        }

        public static void ResetActorPool()
        {   //skip resetting the hero & pet
            for (Pool.actorCounter = 2; Pool.actorCounter < Pool.actorCount; Pool.actorCounter++)
            { Release(Pool.actorPool[Pool.actorCounter]); }
            Pool.actorIndex = 2;
        }

        public static void ResetRoomObjPool()
        {
            for (Pool.roomObjCounter = 0; Pool.roomObjCounter < Pool.roomObjCount; Pool.roomObjCounter++)
            { Release(Pool.roomObjPool[Pool.roomObjCounter]); }
        }

        public static void ResetParticlePool()
        {
            for (Pool.particleCounter = 0; Pool.particleCounter < Pool.particleCount; Pool.particleCounter++)
            { Release(Pool.particlePool[Pool.particleCounter]); }
        }

        public static void ResetProjectilePool()
        {
            for (Pool.projectileCounter = 0; Pool.projectileCounter < Pool.projectileCount; Pool.projectileCounter++)
            { Release(Pool.projectilePool[Pool.projectileCounter]); }
        }

        public static void ResetPickupPool()
        {
            for (Pool.pickupCounter = 0; Pool.pickupCounter < Pool.pickupCount; Pool.pickupCounter++)
            { Release(Pool.pickupPool[Pool.pickupCounter]); }
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
            Pool.interactionsCount = 0;

            //the following phases affect actors, room objects, and projectiles all at once

            #region Phase 1 - Get Input, Update, Animate, & Check Interactions

            //check interactions(act v obj, act v proj, obj v obj, obj v proj)

            //get input & interactions for actors
            for (i = 0; i < Pool.actorCount; i++)
            {
                if (Pool.actorPool[i].active)
                {

                    Functions_Input.SetInputState(Pool.actorPool[i].compInput, Pool.actorPool[i]);

                    Functions_Actor.Update(Pool.actorPool[i]);
                    Functions_Animation.Animate(Pool.actorPool[i].compAnim, Pool.actorPool[i].compSprite);
                    Functions_Animation.ScaleSpriteDown(Pool.actorPool[i].compSprite);

                    //reset the actor's friction for this frame
                    Pool.actorPool[i].compMove.friction = World.friction;

                    //here we could reject dead actor interactions like this
                    //if (Pool.actorPool[i].state != ActorState.Dead)
                    {
                        Functions_Interaction.CheckInteractions(Pool.actorPool[i], true, true);
                        //but it's more fun to see their corpses being moved around by belts
                    }
                }
            }

            //roomObjs
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active)
                {
                    Functions_GameObject.Update(Pool.roomObjPool[i]);
                    Functions_Animation.Animate(Pool.roomObjPool[i].compAnim, Pool.roomObjPool[i].compSprite);
                    Functions_Animation.ScaleSpriteDown(Pool.roomObjPool[i].compSprite);

                    //set objects that are in the air to world air friction
                    if (Pool.roomObjPool[i].compMove.grounded == false)
                    { Pool.roomObjPool[i].compMove.friction = World.frictionAir; }
                    //objects not in the air get set the world ground friction
                    else { Pool.roomObjPool[i].compMove.friction = World.friction; }

                    //any moving roomObj gets interaction checks
                    //and specific roomObjs ALWAYS get interaction checks
                    if (Pool.roomObjPool[i].compMove.moving
                        || Pool.roomObjPool[i].type == ObjType.ConveyorBeltOn)
                    { Functions_Interaction.CheckInteractions(Pool.roomObjPool[i]); }
                }
            }

            //particles
            for (i = 0; i < Pool.particleCount; i++)
            {
                if (Pool.particlePool[i].active)
                {
                    Functions_GameObject.Update(Pool.particlePool[i]);
                    Functions_Animation.Animate(Pool.particlePool[i].compAnim, Pool.particlePool[i].compSprite);
                    Functions_Animation.ScaleSpriteDown(Pool.particlePool[i].compSprite);
                }
            }

            //projectiles
            for (i = 0; i < Pool.projectileCount; i++)
            {
                if (Pool.projectilePool[i].active)
                {
                    Functions_GameObject.Update(Pool.projectilePool[i]);
                    Functions_Animation.Animate(Pool.projectilePool[i].compAnim, Pool.projectilePool[i].compSprite);
                    Functions_Animation.ScaleSpriteDown(Pool.projectilePool[i].compSprite);
                    //interaction check projectiles
                    Functions_Interaction.CheckInteractions(Pool.projectilePool[i]);
                }
            }

            //pickups
            for (i = 0; i < Pool.pickupCount; i++)
            {
                if (Pool.pickupPool[i].active)
                {
                    Functions_GameObject.Update(Pool.pickupPool[i]);
                    Functions_Animation.Animate(Pool.pickupPool[i].compAnim, Pool.pickupPool[i].compSprite);
                    Functions_Animation.ScaleSpriteDown(Pool.pickupPool[i].compSprite);
                    //interaction check pickups
                    Functions_Interaction.CheckInteractions(Pool.pickupPool[i]);
                }
            }

            #endregion


            #region Phase 2 - Project Movement

            //project movement for actors, objects, particles, projectiles, pickups

            //actors
            for (i = 0; i < Pool.actorCount; i++)
            {
                if (Pool.actorPool[i].active)
                { Functions_Movement.ProjectMovement(Pool.actorPool[i].compMove); }
            }

            //roomObjs
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active)
                { Functions_Movement.ProjectMovement(Pool.roomObjPool[i].compMove); }
            }

            //particles
            for (i = 0; i < Pool.particleCount; i++)
            {
                if (Pool.particlePool[i].active)
                { Functions_Movement.ProjectMovement(Pool.particlePool[i].compMove); }
            }

            //projectiles
            for (i = 0; i < Pool.projectileCount; i++)
            {
                if (Pool.projectilePool[i].active)
                { Functions_Movement.ProjectMovement(Pool.projectilePool[i].compMove); }
            }

            //pickups
            for (i = 0; i < Pool.pickupCount; i++)
            {
                if (Pool.pickupPool[i].active)
                { Functions_Movement.ProjectMovement(Pool.pickupPool[i].compMove); }
            }

            #endregion


            #region Phase 3 - Check Collisions

            //check collisions (act v act, act v obj)
            //this is also done last to ensure no overlaps
            //if there was a collision, the act/obj returns
            //to their original position (compMove.pos)

            //actors
            for (i = 0; i < Pool.actorCount; i++)
            {
                if (Pool.actorPool[i].active)
                {
                    if(Pool.actorPool[i] == Pool.hero)
                    {   //hero checks v roomObjs + enemies
                        Functions_Collision.CheckCollisions(
                            Pool.actorPool[i].compMove,
                            Pool.actorPool[i].compCollision,
                            true, true, false);
                    }
                    else if (Pool.actorPool[i] == Pool.herosPet)
                    {   //pet checks v roomObjs only (no actors)
                        Functions_Collision.CheckCollisions(
                            Pool.actorPool[i].compMove,
                            Pool.actorPool[i].compCollision,
                            true, false, false);
                    }
                    else
                    {   //enemies check v roomObjs + hero
                        Functions_Collision.CheckCollisions(
                            Pool.actorPool[i].compMove,
                            Pool.actorPool[i].compCollision,
                            true, false, true);
                    }
                }
            }

            //roomObjs
            for (i = 0; i < Pool.roomObjCount; i++)
            {   
                //only active, moving roomObjs are collision checked
                //an object not moving is assumed to be non-overlapping
                if (Pool.roomObjPool[i].active & Pool.roomObjPool[i].compMove.moving)
                {
                    Functions_Collision.CheckCollisions(
                                Pool.roomObjPool[i].compMove,
                                Pool.roomObjPool[i].compCollision,
                                true, true, true);
                }
            }

            #endregion

            
            #region Phase 4 - Resolution, Align() Components

            //actors
            for (i = 0; i < Pool.actorCount; i++)
            {
                if (Pool.actorPool[i].active)
                { Functions_Component.Align(Pool.actorPool[i]); }
            }

            //roomObjs
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active)
                { Functions_Component.Align(Pool.roomObjPool[i]); }
            }

            //particles
            for (i = 0; i < Pool.particleCount; i++)
            {
                if (Pool.particlePool[i].active)
                {   //set position to the new position (projected pos)
                    Pool.particlePool[i].compMove.position.X = Pool.particlePool[i].compMove.newPosition.X;
                    Pool.particlePool[i].compMove.position.Y = Pool.particlePool[i].compMove.newPosition.Y;
                    Functions_Component.Align(Pool.particlePool[i]);
                }
            }

            //projectiles
            for (i = 0; i < Pool.projectileCount; i++)
            {
                if (Pool.projectilePool[i].active)
                {   //set position to the new position (projected pos)
                    Pool.projectilePool[i].compMove.position.X = Pool.projectilePool[i].compMove.newPosition.X;
                    Pool.projectilePool[i].compMove.position.Y = Pool.projectilePool[i].compMove.newPosition.Y;
                    Functions_Component.Align(Pool.projectilePool[i]);
                }
            }

            //pickups
            for (i = 0; i < Pool.pickupCount; i++)
            {
                if (Pool.pickupPool[i].active)
                {   //set position to the new position (projected pos)
                    Pool.pickupPool[i].compMove.position.X = Pool.pickupPool[i].compMove.newPosition.X;
                    Pool.pickupPool[i].compMove.position.Y = Pool.pickupPool[i].compMove.newPosition.Y;
                    Functions_Component.Align(Pool.pickupPool[i]);
                }
            }

            #endregion


            //handle hero related updates (room checking, shadow match, pickup interactions)
            Functions_Hero.Update();
        }
        
        public static void Draw()
        {
            //floor pool
            for (Pool.floorCounter = 0; Pool.floorCounter < Pool.floorCount; Pool.floorCounter++)
            { Functions_Draw.Draw(Pool.floorPool[Pool.floorCounter]); }
            //roomObj pool
            for (Pool.roomObjCounter = 0; Pool.roomObjCounter < Pool.roomObjCount; Pool.roomObjCounter++)
            { Functions_Draw.Draw(Pool.roomObjPool[Pool.roomObjCounter]); }
            //particles
            for (Pool.particleCounter = 0; Pool.particleCounter < Pool.particleCount; Pool.particleCounter++)
            { Functions_Draw.Draw(Pool.particlePool[Pool.particleCounter]); }
            //projectiles
            for (Pool.projectileCounter = 0; Pool.projectileCounter < Pool.projectileCount; Pool.projectileCounter++)
            { Functions_Draw.Draw(Pool.projectilePool[Pool.projectileCounter]); }
            //pickups
            for (Pool.pickupCounter = 0; Pool.pickupCounter < Pool.pickupCount; Pool.pickupCounter++)
            { Functions_Draw.Draw(Pool.pickupPool[Pool.pickupCounter]); }
            //actor pool
            for (Pool.actorCounter = 0; Pool.actorCounter < Pool.actorCount; Pool.actorCounter++)
            { Functions_Draw.Draw(Pool.actorPool[Pool.actorCounter]); }
            //hero's shadow
            Functions_Draw.Draw(Functions_Hero.heroShadow);
        }

    }
}