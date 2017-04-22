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
    public static class InteractionFunctions
    {
        public static byte damage;
        public static float force;

        public static ComponentCollision interactionRec = new ComponentCollision();



        public static void ClearHeroInteractionRec()
        {   //move the interaction rec offscreen
            interactionRec.rec.X = -1000;
            interactionRec.rec.Y = -1000;
        }

        public static void SetHeroInteractionRec()
        {
            //set the interaction rec to the hero's position
            interactionRec.rec.X = (int)Pool.hero.compSprite.position.X - 4;
            interactionRec.rec.Y = (int)Pool.hero.compSprite.position.Y - 4;
            //offset the rec based on the direction hero is facing
            if (Pool.hero.direction == Direction.Up)
            {
                interactionRec.rec.Width = 8; interactionRec.rec.Height = 4;
                interactionRec.rec.Y -= 1;
            }
            else if (Pool.hero.direction == Direction.Down)
            {
                interactionRec.rec.Width = 8; interactionRec.rec.Height = 4;
                interactionRec.rec.Y += 14;
            }
            else if (
                Pool.hero.direction == Direction.Left || 
                Pool.hero.direction == Direction.UpLeft || 
                Pool.hero.direction == Direction.DownLeft) 
            {
                interactionRec.rec.Width = 4; interactionRec.rec.Height = 8;
                interactionRec.rec.Y += 4; interactionRec.rec.X -= 7;
            }
            else if (
                Pool.hero.direction == Direction.Right || 
                Pool.hero.direction == Direction.UpRight || 
                Pool.hero.direction == Direction.DownRight)
            {
                interactionRec.rec.Width = 4; interactionRec.rec.Height = 8;
                interactionRec.rec.Y += 4; interactionRec.rec.X += 11;
            }
        }



        public static void Handle(Actor Actor, GameObject Obj)
        {
            //the Obj is non-blocking
            //particle Objs never interact with actors or reach this function
            //objectGroups are checked in order of most commonly interacted with


            #region Projectiles

            if(Obj.objGroup == GameObject.ObjGroup.Projectile)
            {
                //all damage inducing projectiles
                damage = 0; //reset the damage value
                force = 0.0f; //reset the force amount (how much actor is pushed)


                if (Obj.type == GameObject.Type.ProjectileSword)
                {
                    damage = 1;
                    force = 8.0f;
                    //create a hit particle
                    //swords always complete their animation
                }
                //else if projectile is fireball
                //deal more damage
                //create explosion particle
                //Obj.lifeCounter = Obj.lifetime; //end the projectiles life

                //only damage/hit/push actors not in the hit state
                if (Actor.state != Actor.State.Hit)
                {   //deal damage to the actor
                    //but prevent the damage byte from underflowing the Actor.health byte
                    if (damage > Actor.health) { Actor.health = 0; }
                    else { Actor.health -= damage; }

                    //if projectile damaged hero, track the damage dealt
                    if (Actor == Pool.hero) { DungeonRecord.totalDamage += damage; }

                    //set actor into hit state, push actor the projectile's direction
                    ActorFunctions.SetHitState(Actor);
                    MovementFunctions.Push(Actor.compMove, Obj.direction, force);
                }


            }

            #endregion


            #region Items

            else if (Obj.objGroup == GameObject.ObjGroup.Item)
            {
                if(Actor == Pool.hero) //only the hero can pickup hearts or rupees
                {
                    //hearts + rupees
                    if (Obj.type == GameObject.Type.ItemHeart) { Actor.health++; }
                    else if (Obj.type == GameObject.Type.ItemRupee) { PlayerData.saveData.gold++; }

                    Assets.sfxReward.Play(); //play the item pickup soundFX
                    Obj.lifetime = 1; Obj.lifeCounter = 2; //end the items life
                    
                }
            }

            #endregion


            #region Liftable objects

            else if (Obj.objGroup == GameObject.ObjGroup.Liftable)
            {
                //pot skulls
            }

            #endregion


            #region Draggable objects

            else if (Obj.objGroup == GameObject.ObjGroup.Draggable)
            {
                //BlockDraggable
            }

            #endregion


            #region Chests

            else if (Obj.objGroup == GameObject.ObjGroup.Chest)
            {
                //only HERO can open chests, and he must do so via the InteractionRec (A Button Press)
                if (Actor == Pool.hero && Actor.state == Actor.State.Interact)
                {
                    //reward the hero with the chests contents
                    if (Obj.type == GameObject.Type.ChestGold)
                    {
                        GameObjectFunctions.SpawnParticle(
                            GameObject.Type.ParticleReward50Gold,
                            Actor.compSprite.position + new Vector2(0, -14));
                        Assets.sfxReward.Play();
                        PlayerData.saveData.gold += 50; //give the hero 50 gold
                    }
                    else if (Obj.type == GameObject.Type.ChestKey)
                    {
                        GameObjectFunctions.SpawnParticle(
                            GameObject.Type.ParticleRewardKey,
                            Actor.compSprite.position + new Vector2(0, -14));
                        Assets.sfxKeyPickup.Play();
                        DungeonFunctions.dungeon.bigKey = true;
                    }
                    else if (Obj.type == GameObject.Type.ChestMap)
                    {
                        GameObjectFunctions.SpawnParticle(
                            GameObject.Type.ParticleRewardMap,
                            Actor.compSprite.position + new Vector2(0, -14));
                        Assets.sfxReward.Play();
                        //set map boolean true, just like bigKey boolean
                    }

                    Assets.sfxChestOpen.Play();
                    GameObjectFunctions.SetType(Obj, GameObject.Type.ChestEmpty);
                    //set actor into reward state
                    Actor.state = Actor.State.Reward;
                    //play an explosion particle to show the chest was opened
                    GameObjectFunctions.SpawnParticle(GameObject.Type.ParticleExplosion, Obj.compSprite.position);
                }
            }

            #endregion


            #region Doors

            else if (Obj.objGroup == GameObject.ObjGroup.Door)
            {
                if (Obj.type == GameObject.Type.DoorBoss)
                {   //only hero can open boss door, and must have dungeon key
                    if (DungeonFunctions.dungeon.bigKey && Actor == Pool.hero)
                    {
                        GameObjectFunctions.SetType(Obj, GameObject.Type.DoorOpen);
                        Assets.sfxDoorOpen.Play();
                        GameObjectFunctions.SpawnParticle(GameObject.Type.ParticleExplosion, Obj.compSprite.position);
                    }
                }
                else if (Obj.type == GameObject.Type.DoorTrap)
                {   //trap doors push ALL actors
                    MovementFunctions.Push(Actor.compMove, Obj.direction, 1.0f);
                    GameObjectFunctions.SpawnParticle(GameObject.Type.ParticleDashPuff, Actor.compSprite.position);
                }
            }

            #endregion


            #region Other Interactive Objects

            else if(Obj.objGroup == GameObject.ObjGroup.Object)
            {
                //nothing yet, but objs like pits would be handled here
                //block spikes, lever, floor spikes, switch, bridge, flamethrower,
                //torch unlit, torch lit, conveyor belt, 
            }

            #endregion


        }



        public static void Handle(GameObject Projectile, GameObject Obj)
        {
            //Obj could be a projectile!
            //no blocking checks have been done yet

            //if a projectile collides with something, the projectile may get destroyed (end of lifetime)
            //Obj.lifeCounter = Obj.lifetime; //end the projectiles life
            //create an explosion effect here
        }

    }
}