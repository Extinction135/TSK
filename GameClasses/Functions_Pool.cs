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

        public static IndestructibleObject GetIndObj()
        {
            for (Pool.indObjCounter = 0; Pool.indObjCounter < Pool.indObjCount; Pool.indObjCounter++)
            {   //found an inactive obj to return
                if (Pool.indObjPool[Pool.indObjCounter].active == false)
                {   //reset obj to default state, hide offscreen, return it
                    Functions_IndestructibleObjs.Reset(Pool.indObjPool[Pool.indObjCounter]);
                    Pool.indObjPool[Pool.indObjCounter].compSprite.position.X = -1000;
                    return Pool.indObjPool[Pool.indObjCounter];
                }
            }
            return Pool.indObjPool[Pool.indObjCounter - 1]; //ran out of indObjs
        }

        public static InteractiveObject GetIntObj()
        {
            for (Pool.intObjCounter = 0; Pool.intObjCounter < Pool.intObjCount; Pool.intObjCounter++)
            {   //found an inactive obj to return
                if (Pool.intObjPool[Pool.intObjCounter].active == false)
                {   //reset obj to default state, hide offscreen, return it
                    Functions_InteractiveObjs.Reset(Pool.intObjPool[Pool.intObjCounter]);
                    Pool.intObjPool[Pool.intObjCounter].compSprite.position.X = -1000;
                    return Pool.intObjPool[Pool.intObjCounter];
                }
            }
            return Pool.intObjPool[Pool.intObjCounter - 1]; //ran out of indObjs
        }

        public static Actor GetActor()
        {
            Pool.actorIndex++;
            //reset index to 1, skipping hero in actor pool
            if (Pool.actorIndex == Pool.actorCount) { Pool.actorIndex = 1; }
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

        public static Particle GetParticle()
        {   
            for (Pool.particleCounter = 0; Pool.particleCounter < Pool.particleCount; Pool.particleCounter++)
            {
                Pool.particleIndex++;
                if (Pool.particleIndex >= Pool.particleCount) { Pool.particleIndex = 0; }
                if (Pool.particlePool[Pool.particleIndex].active == false)
                {   //found an inactive to return
                    //reset to default state, hide offscreen, return it
                    Functions_Particle.Reset(Pool.particlePool[Pool.particleIndex]);
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
                    Functions_Projectile.Reset(Pool.projectilePool[Pool.projectileIndex]);

                    Pool.projectilePool[Pool.projectileIndex].compMove.newPosition.X = -1000;
                    Pool.projectilePool[Pool.projectileIndex].compSprite.scale = 1.0f;
                    //clear caster ref
                    Pool.projectilePool[Pool.projectileIndex].caster = null;
                    return Pool.projectilePool[Pool.projectileIndex];
                }
            }
            return Pool.projectilePool[0]; //ran out
        }

        public static Pickup GetPickup()
        {
            for (Pool.pickupCounter = 0; Pool.pickupCounter < Pool.pickupCount; Pool.pickupCounter++)
            {
                Pool.pickupIndex++;
                if (Pool.pickupIndex >= Pool.pickupCount) { Pool.pickupIndex = 0; }
                if (Pool.pickupPool[Pool.pickupIndex].active == false)
                {   //found an inactive to return
                    //reset to default state, hide offscreen, return it
                    Functions_Pickup.Reset(Pool.pickupPool[Pool.pickupIndex]);
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
            { Pool.floorIndex = 0; } //ran out of floors to return
            Pool.floorPool[Pool.floorIndex].visible = true;
            return Pool.floorPool[Pool.floorIndex];
        }








        public static void Reset()
        {
            ResetActorPool();
            ResetIndObjPool();
            ResetIntObjPool();
            ResetParticlePool();
            ResetProjectilePool();
            ResetPickupPool();
            ResetFloorPool();
            ResetLinePool();
        }

        public static void ResetIndObjPool()
        {
            for (Pool.indObjCounter = 0; Pool.indObjCounter < Pool.indObjCount; Pool.indObjCounter++)
            { Release(Pool.indObjPool[Pool.indObjCounter]); }
        }

        public static void ResetIntObjPool()
        {
            for (Pool.intObjCounter = 0; Pool.intObjCounter < Pool.intObjCount; Pool.intObjCounter++)
            { Release(Pool.intObjPool[Pool.intObjCounter]); }
        }

        public static void ResetActorPool()
        {   //skip resetting the hero & pet
            for (Pool.actorCounter = 1; Pool.actorCounter < Pool.actorCount; Pool.actorCounter++)
            { Release(Pool.actorPool[Pool.actorCounter]); }
            Pool.actorIndex = 1;
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
                Pool.floorPool[Pool.floorCounter].zDepth = World.floorLayer;
            }
            Pool.floorIndex = 0; //reset total count
        }

        public static void ResetLinePool()
        {
            for(Pool.lineCounter = 0; Pool.lineCounter < Pool.lineCount; Pool.lineCounter++)
            {   //hide offscreen
                Pool.linePool[Pool.lineCounter].visible = false;
                Pool.linePool[Pool.lineCounter].length = 0;
                Pool.linePool[Pool.lineCounter].endPosX = 0;
                Pool.linePool[Pool.lineCounter].endPosY = 0;
                Pool.linePool[Pool.lineCounter].startPosX = 0;
                Pool.linePool[Pool.lineCounter].startPosY = 0;
            }
        }






        public static void ResetActorPoolInput()
        {
            for (Pool.actorCounter = 0; Pool.actorCounter < Pool.actorCount; Pool.actorCounter++)
            { Functions_Input.ResetInputData(Pool.actorPool[Pool.actorCounter].compInput); }
        }






        public static void Release(IndestructibleObject Obj)
        {
            Obj.active = false;
        }

        public static void Release(InteractiveObject Obj)
        {
            Obj.active = false;
        }

        public static void Release(Actor Actor)
        {
            Actor.active = false;
        }

        public static void Release(Projectile Pro)
        {
            Pro.active = false;
            Pro.lifetime = 0;
        }

        public static void Release(Particle Par)
        {
            Par.active = false;
            Par.lifetime = 0;
        }

        public static void Release(Pickup Pick)
        {
            Pick.active = false;
            Pick.lifetime = 0;
        }










        static int p;
        public static void AlignIndObjs()
        {   //align sprite + collision comps to move comp of all active objs
            for (p = 0; p < Pool.indObjCount; p++)
            {
                if (Pool.indObjPool[p].active)
                {   //align collision to sprite component
                    Functions_Component.Align(
                        Pool.indObjPool[p].compSprite,
                        Pool.indObjPool[p].compCollision);
                    //set the current animation frame, check the animation counter
                    Functions_Animation.Animate(Pool.indObjPool[p].compAnim,
                        Pool.indObjPool[i].compSprite);
                    //set the rotation for the obj's sprite
                    Functions_IndestructibleObjs.SetRotation(Pool.indObjPool[p]);
                }
            }
        }

        public static void AlignIntObjs()
        {   //align sprite + collision comps to move comp of all active objs
            for (p = 0; p < Pool.intObjCount; p++)
            {
                if (Pool.intObjPool[p].active)
                {   //align the sprite and collision components to the move component
                    Functions_Component.Align(
                        Pool.intObjPool[p].compMove,
                        Pool.intObjPool[p].compSprite,
                        Pool.intObjPool[p].compCollision);
                    //set the current animation frame, check the animation counter
                    Functions_Animation.Animate(Pool.intObjPool[p].compAnim,
                        Pool.intObjPool[i].compSprite);
                    //set the rotation for the obj's sprite
                    Functions_InteractiveObjs.SetRotation(Pool.intObjPool[p]);
                }
            }
        }













        public static void Update()
        {
            Pool.collisionsCount = 0;
            Pool.interactionsCount = 0;
            //reset hero - assume hero is not under roof
            Functions_Hero.underRoof = false;


            





            //OLD(): check interactions(act v obj, act v proj, obj v obj, obj v proj)

            //interactions new: 
            //acts vs ints, acts vs pros
            //ints vs ints
            //pros vs ints
            //hero vs pickups
            //hero intPoint vs ints & inds

            //collisions new
            //acts vs inds
            //ints vs inds
            //pros vs inds
            //acts vs hero
            //hero vs acts







            //Phase 1 - Get Input, Update, Animate, & Check Interactions

            #region Actors

            for (i = 0; i < Pool.actorCount; i++)
            {
                if (Pool.actorPool[i].active)
                {
                    //map actor input state to actor input component
                    Functions_Input.SetInputState(Pool.actorPool[i].compInput, Pool.actorPool[i]);

                    //here we are essentially resetting the actor for interactions below
                    //set actors that are in the air to world air friction
                    if (Pool.actorPool[i].compMove.grounded == false)
                    { Pool.actorPool[i].compMove.friction = World.frictionAir; }
                    //actors not in the air get set the world ground friction
                    else { Pool.actorPool[i].compMove.friction = World.friction; }

                    Pool.actorPool[i].feetFX.visible = false; //reset feetFX

                    //underwater enemies are always swimming
                    if (Pool.actorPool[i].underwaterEnemy)
                    {
                        Pool.actorPool[i].swimming = true;
                        Pool.actorPool[i].compMove.friction = World.frictionWater;
                    }
                    //else reset swimming boolean
                    else
                    {
                        Pool.actorPool[i].swimming = false;
                        Functions_Actor.Breathe(Pool.actorPool[i]);
                    }

                    //if actor is falling, assume they should land this frame
                    if (Pool.actorPool[i].state == ActorState.Falling)
                    { Pool.actorPool[i].state = ActorState.Landed; }


                    if (Pool.actorPool[i] == Pool.hero & Flags.Clipping) { } //do nothing
                    else
                    {
                        //then finally handle any interactions the actor has
                        Functions_Interaction.CheckObj_Actor(Pool.actorPool[i]);
                        Functions_Interaction.CheckProjectile_Actor(Pool.actorPool[i]);
                    }

                    Functions_Actor.Update(Pool.actorPool[i]);
                    Functions_Animation.Animate(Pool.actorPool[i].compAnim, Pool.actorPool[i].compSprite);
                    Functions_Animation.ScaleSpriteDown(Pool.actorPool[i].compSprite);

                    if (Pool.actorPool[i].swimming == false)
                    {   //if actor exited water, reset splash and underwater flags
                        Pool.actorPool[i].createSplash = false;
                        Pool.actorPool[i].underwater = false;
                    }
                }
            }

            #endregion


            #region Interactive Objects

            for (i = 0; i < Pool.intObjCount; i++)
            {
                if (Pool.intObjPool[i].active)
                {
                    //set objects that are in the air to world air friction
                    if (Pool.intObjPool[i].compMove.grounded == false)
                    { Pool.intObjPool[i].compMove.friction = World.frictionAir; }
                    //objects not in the air get set the world ground friction
                    else { Pool.intObjPool[i].compMove.friction = World.friction; }

                    //reset water booleans for objects
                    Pool.intObjPool[i].inWater = false;
                    Pool.intObjPool[i].underWater = false;

                    //handle roomObjs that remove themselves upon overlap
                    if (Pool.intObjPool[i].selfCleans)
                    {   //clean yo self, then exit cleaning branch
                        Functions_InteractiveObjs.SelfClean(Pool.intObjPool[i]);
                    }

                    //check interactions for certain roomObjs
                    if (
                        //if roomObj is moving, check interactions - BIG optimization
                        Pool.intObjPool[i].compMove.moving
                        //nonblocking objs that dont move, but interact
                        || Pool.intObjPool[i].type == InteractiveType.ConveyorBeltOn
                        || Pool.intObjPool[i].type == InteractiveType.IceTile
                        //these roomObjs always get interaction checked
                        || Pool.intObjPool[i].type == InteractiveType.Fairy
                        || Pool.intObjPool[i].type == InteractiveType.Pet_Dog

                        /*EXITS ARE INDESTRUCTIBLE OBJS NOT INTERACTIVES!
                        //exits remove anything they touch
                        || Pool.intObjPool[i].type == InteractiveType.Dungeon_Exit
                        || Pool.intObjPool[i].type == InteractiveType.Dungeon_ExitPillarLeft
                        || Pool.intObjPool[i].type == InteractiveType.Dungeon_ExitPillarRight
                        */


                        )
                    {   //Debug.WriteLine("pool update roomobj type: " + Pool.roomObjPool[i].type);
                        Functions_Interaction.CheckObj_Obj(Pool.intObjPool[i]);
                    }

                    //update, animate, scale
                    Functions_InteractiveObjs.Update(Pool.intObjPool[i]);
                    Functions_Animation.Animate(Pool.intObjPool[i].compAnim, Pool.intObjPool[i].compSprite);
                    Functions_Animation.ScaleSpriteDown(Pool.intObjPool[i].compSprite);
                }
            }

            #endregion


            #region Particles

            for (i = 0; i < Pool.particleCount; i++)
            {
                if (Pool.particlePool[i].active)
                {
                    Functions_Particle.Update(Pool.particlePool[i]);
                    Functions_Animation.Animate(Pool.particlePool[i].compAnim, Pool.particlePool[i].compSprite);
                    Functions_Animation.ScaleSpriteDown(Pool.particlePool[i].compSprite);
                }
            }

            #endregion


            #region Projectiles

            for (i = 0; i < Pool.projectileCount; i++)
            {
                if (Pool.projectilePool[i].active)
                {   
                    Functions_Projectile.Update(Pool.projectilePool[i]);
                    Functions_Animation.Animate(Pool.projectilePool[i].compAnim, Pool.projectilePool[i].compSprite);
                    Functions_Animation.ScaleSpriteDown(Pool.projectilePool[i].compSprite);
                    //interaction check projectiles (this may kill them, so we do it last)
                    Functions_Interaction.CheckProjectile_Obj(Pool.projectilePool[i]);
                }
            }

            #endregion


            #region Pickups

            for (i = 0; i < Pool.pickupCount; i++)
            {
                if (Pool.pickupPool[i].active)
                {
                    Functions_Pickup.Update(Pool.pickupPool[i]);
                    Functions_Animation.Animate(Pool.pickupPool[i].compAnim, Pool.pickupPool[i].compSprite);
                    Functions_Animation.ScaleSpriteDown(Pool.pickupPool[i].compSprite);
                }
            }

            #endregion






            









            #region Phase 2 - Project Movement

            //project movement for actors, objects, particles, projectiles, pickups
            //indestructible objs CANT move

            //actors
            for (i = 0; i < Pool.actorCount; i++)
            {
                if (Pool.actorPool[i].active)
                { Functions_Movement.ProjectMovement(Pool.actorPool[i].compMove); }
            }

            //interactive objs
            for (i = 0; i < Pool.intObjCount; i++)
            {
                if (Pool.intObjPool[i].active)
                { Functions_Movement.ProjectMovement(Pool.intObjPool[i].compMove); }
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

            #endregion


            #region Phase 3 - Check Collisions

            //this is also done last to ensure no overlaps
            //if there was a collision, the act/obj returns
            //to their original position (compMove.pos)

            //actors
            for (i = 0; i < Pool.actorCount; i++)
            {
                if (Pool.actorPool[i].active)
                {
                    if(Pool.actorPool[i] == Pool.hero)
                    {
                        if (Flags.Clipping) //if clipping, there are no collisions
                        {
                            Functions_Collision.CheckCollisions(
                                Pool.actorPool[i].compMove,
                                Pool.actorPool[i].compCollision,
                                false, false, false, false);
                        }
                        else
                        {
                            //hero checks v inds/ints + enemies
                            Functions_Collision.CheckCollisions(
                                Pool.actorPool[i].compMove,
                                Pool.actorPool[i].compCollision,
                                true, true, true, false);
                        }
                    }
                    else
                    {   //enemies check v inds/ints + hero
                        Functions_Collision.CheckCollisions(
                            Pool.actorPool[i].compMove,
                            Pool.actorPool[i].compCollision,
                            true, true, false, true);
                    }
                }
            }

            //interactive objects
            for (i = 0; i < Pool.intObjCount; i++)
            {  
                if(Pool.intObjPool[i].active) //obj must be active
                {
                    //roomObj must be blocking to be collision checked
                    //it makes no sense to collision check non-blocking objs
                    if (Pool.intObjPool[i].compCollision.blocking)
                    {   
                        //obj must be moving as well, otherwise we assume
                        //a non-moving obj is at rest and isn't overlapping
                        //any other roomObjs, either because it was placed
                        //by hand, or because it was moving, got interacted()
                        //with, and came to rest. this is an optimization.
                        if (Pool.intObjPool[i].compMove.moving)
                        {
                            Functions_Collision.CheckCollisions(
                                Pool.intObjPool[i].compMove,
                                Pool.intObjPool[i].compCollision,
                                true, true, true, true);
                        }
                    }
                    else
                    {   //roomObj isn't blocking, but may be moving
                        //in this case, we dont check collisions,
                        //but we still need to update the obj.move.position
                        if (Pool.intObjPool[i].compMove.moving)
                        {   //set the position equal to the newPosition, which was set using ProjectMovement()
                            Pool.intObjPool[i].compMove.position.X = Pool.intObjPool[i].compMove.newPosition.X;
                            Pool.intObjPool[i].compMove.position.Y = Pool.intObjPool[i].compMove.newPosition.Y;
                        }
                    }
                }
            }


            //check projectiles against the indestructibles list
            for(i = 0; i < Pool.projectileCount; i++)
            {
                if (Pool.projectilePool[i].active)
                {   //projectiles that collide with indestructibles stop movement/dont overlap
                    if(Pool.projectilePool[i].compMove.moving)
                    {
                        Functions_Collision.CheckCollisions(
                                Pool.projectilePool[i].compMove,
                                Pool.projectilePool[i].compCollision,
                                true, false, false, false);
                    }
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

            //interactive objs
            for (i = 0; i < Pool.intObjCount; i++)
            {
                if (Pool.intObjPool[i].active)
                { Functions_Component.Align(Pool.intObjPool[i]); }
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

            #endregion


            //handle hero related updates (room checking, shadow match, pickup interactions)
            Functions_Hero.Update();

            //update lines
            for (Pool.lineCounter = 0; Pool.lineCounter < Pool.lineCount; Pool.lineCounter++)
            { Functions_Line.UpdateLine(Pool.linePool[Pool.lineCounter]); }
        }
        
        public static void Draw()
        {
            //floor pool
            for (Pool.floorCounter = 0; Pool.floorCounter < Pool.floorCount; Pool.floorCounter++)
            { Functions_Draw.Draw(Pool.floorPool[Pool.floorCounter]); }
            //line pool
            for (Pool.lineCounter = 0; Pool.lineCounter < Pool.lineCount; Pool.lineCounter++)
            { Functions_Draw.Draw(Pool.linePool[Pool.lineCounter]); }


            //no longer drawing the gameObj pool
            for (Pool.indObjCounter = 0; Pool.indObjCounter < Pool.indObjCount; Pool.indObjCounter++)
            { Functions_Draw.Draw(Pool.indObjPool[Pool.indObjCounter]); }
            for (Pool.intObjCounter = 0; Pool.intObjCounter < Pool.intObjCount; Pool.intObjCounter++)
            { Functions_Draw.Draw(Pool.intObjPool[Pool.intObjCounter]); }


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
            
            //handle hero specific drawing last
            Functions_Hero.Draw();
        }




    }
}