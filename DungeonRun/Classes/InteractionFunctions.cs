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
        static Vector2 offset = new Vector2(0, 0);

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
            offset.X = 0; offset.Y = 0;


            #region Projectiles

            if (Obj.group == ObjGroup.Projectile)
            {
                //all damage inducing projectiles
                damage = 0; //reset the damage value
                force = 0.0f; //reset the force amount (how much actor is pushed)

                if (Obj.type == ObjType.ProjectileSword)
                {   //swords always complete their animation
                    damage = 1; force = 8.0f;
                }
                else if (Obj.type == ObjType.ProjectileFireball)
                {   //fireballs destruct upon collision/interaction
                    Obj.lifeCounter = Obj.lifetime;
                    damage = 1; force = 8.0f;
                }

                BattleFunctions.Damage(Actor, damage, force, Obj.direction);
            }

            #endregion


            #region Items

            else if (Obj.group == ObjGroup.Item)
            {
                if(Actor == Pool.hero) //only the hero can pickup hearts or rupees
                {
                    if (Obj.type == ObjType.ItemHeart)
                    { Actor.health++; Assets.sfxHeartPickup.Play(); }
                    else if (Obj.type == ObjType.ItemRupee)
                    { PlayerData.saveData.gold++; Assets.sfxGoldPickup.Play(); }
                    //end the items life
                    Obj.lifetime = 1; Obj.lifeCounter = 2; 
                }
            }

            #endregion


            #region Liftable objects

            else if (Obj.group == ObjGroup.Liftable)
            {
                //pot skulls
            }

            #endregion


            #region Draggable objects

            else if (Obj.group == ObjGroup.Draggable)
            {
                //BlockDraggable
            }

            #endregion


            #region Chests

            else if (Obj.group == ObjGroup.Chest)
            {   //only HERO can open chests, and he must do so via the InteractionRec (A Button Press)
                if (Actor == Pool.hero && Actor.state == ActorState.Interact)
                {
                    offset.Y = -14; //place reward above hero's head
                    //reward the hero with the chests contents
                    if (Obj.type == ObjType.ChestGold)
                    {
                        GameObjectFunctions.SpawnProjectile(
                            ObjType.ParticleReward50Gold,
                            Actor.compSprite.position + offset, 
                            Direction.None);
                        Assets.sfxReward.Play();
                        PlayerData.saveData.gold += 50; //give the hero 50 gold
                    }
                    else if (Obj.type == ObjType.ChestKey)
                    {
                        GameObjectFunctions.SpawnProjectile(
                            ObjType.ParticleRewardKey,
                            Actor.compSprite.position + offset,
                            Direction.None);
                        Assets.sfxKeyPickup.Play();
                        DungeonFunctions.dungeon.bigKey = true;
                    }
                    else if (Obj.type == ObjType.ChestMap)
                    {
                        GameObjectFunctions.SpawnProjectile(
                            ObjType.ParticleRewardMap,
                            Actor.compSprite.position + offset,
                            Direction.None);
                        Assets.sfxReward.Play();
                        DungeonFunctions.dungeon.map = true;
                    }
                    else if (Obj.type == ObjType.ChestHeartPiece)
                    {
                        if (WorldUI.pieceCounter == 3)
                        {   //if this completes a heart, display the full heart reward
                            GameObjectFunctions.SpawnProjectile(
                                ObjType.ParticleRewardHeartFull,
                                Actor.compSprite.position + offset,
                                Direction.None);
                        }
                        else
                        {   //this does not complete a heart, display the heart piece reward
                            GameObjectFunctions.SpawnProjectile(
                                ObjType.ParticleRewardHeartPiece,
                                Actor.compSprite.position + offset,
                                Direction.None);
                        }
                        Assets.sfxReward.Play();
                        PlayerData.saveData.heartPieces++;
                    }

                    Assets.sfxChestOpen.Play();
                    GameObjectFunctions.SetType(Obj, ObjType.ChestEmpty);
                    //set actor into reward state
                    Actor.state = ActorState.Reward;
                    //play an explosion particle to show the chest was opened
                    GameObjectFunctions.SpawnProjectile(
                        ObjType.ParticleAttention,
                        Obj.compSprite.position,
                        Direction.None);
                }
            }

            #endregion


            #region Doors

            else if (Obj.group == ObjGroup.Door)
            {
                if (Obj.type == ObjType.DoorBoss)
                {   //only hero can open boss door, and must have dungeon key
                    if (DungeonFunctions.dungeon.bigKey && Actor == Pool.hero)
                    {
                        GameObjectFunctions.SetType(Obj, ObjType.DoorOpen);
                        Assets.sfxDoorOpen.Play();
                        GameObjectFunctions.SpawnProjectile(
                            ObjType.ParticleAttention, 
                            Obj.compSprite.position,
                            Direction.None);
                    }
                }
                else if (Obj.type == ObjType.DoorTrap)
                {   //trap doors push ALL actors
                    MovementFunctions.Push(Actor.compMove, Obj.direction, 1.0f);
                    GameObjectFunctions.SpawnProjectile(
                        ObjType.ParticleDashPuff, 
                        Actor.compSprite.position,
                        Direction.None);
                }
            }

            #endregion


            #region Other Interactive Objects

            else if(Obj.group == ObjGroup.Object)
            {
                if (Obj.type == ObjType.BlockSpikes)
                {   //damage actor and push in actor's opposite direction
                    BattleFunctions.Damage(Actor, 1, 10.0f,
                        ComponentFunctions.GetOppositeDirection(Actor.direction));
                }
                else if (Obj.type == ObjType.ConveyorBelt)
                {   //push actor in belt's direction
                    MovementFunctions.Push(Actor.compMove, Obj.direction, 0.1f);
                }
                else if (Obj.type == ObjType.Bumper)
                {   
                    //push actor in opposite direction
                    MovementFunctions.Push(Actor.compMove,
                        ComponentFunctions.GetOppositeDirection(Actor.direction),
                        10.0f);
                    //scale the bumper up
                    Obj.compSprite.scale = 1.25f;
                    //play the bounce sound fx
                    Assets.sfxBounce.Play(); 
                    GameObjectFunctions.SpawnProjectile(
                        ObjType.ParticleDashPuff,
                        Actor.compSprite.position,
                        Direction.None);
                }

                //block spikes, lever, floor spikes, switch, bridge, flamethrower,
                //torch unlit, torch lit, 

            }

            #endregion


        }

        public static void Handle(GameObject ObjA, GameObject ObjB)
        {
            //Obj could be a projectile!
            //Projectile could = Obj!, if comparing Projectile to Obj in ProjectilePool
            //no blocking checks have been done yet

            if(ObjB.compCollision.blocking) //is the colliding object blocking?
            {
                if (ObjA.type == ObjType.ProjectileFireball)
                {
                    ObjA.lifeCounter = ObjA.lifetime; //kill fireball
                }

                else if (ObjA.type == ObjType.BlockSpikes)
                {   //flip the object's direction to the opposite direction
                    ObjA.compMove.direction = ComponentFunctions.GetOppositeDirection(ObjA.compMove.direction);
                    //push the object in it's new direction, out of this collision
                    MovementFunctions.Push(ObjA.compMove, ObjA.compMove.direction, 5.0f);
                    Assets.sfxMetallicTap.Play(); //play the 'clink' sound effect
                    //show that the object has been hit
                    GameObjectFunctions.SpawnProjectile(
                        ObjType.ParticleHitSparkle, 
                        ObjA.compSprite.position, 
                        Direction.None);
                }
            }
        }

    }
}