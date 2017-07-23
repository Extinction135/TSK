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
    public static class Functions_Interaction
    {

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

        public static void Interact(Actor Actor, GameObject Obj)
        {
            //the Obj is non-blocking
            //particle Objs never interact with actors or reach this function
            //objectGroups are checked in order of most commonly interacted with

            //this function handles hero collisions AND hero interactionRec collisions

            //most projectiles deal damage to actors upon a collision/interaction
            if (Obj.group == ObjGroup.Projectile) { Functions_Battle.Damage(Actor, Obj); }


            #region Pickups

            else if (Obj.group == ObjGroup.Pickup)
            {
                if (Actor == Pool.hero) //only the hero can pickup hearts or rupees
                {
                    if (Obj.type == ObjType.PickupHeart)
                    { Actor.health++; Assets.Play(Assets.sfxHeartPickup); }
                    else if (Obj.type == ObjType.PickupRupee)
                    { PlayerData.current.gold++; Assets.Play(Assets.sfxGoldPickup); }
                    else if (Obj.type == ObjType.PickupMagic)
                    { PlayerData.current.magicCurrent++; Assets.Play(Assets.sfxHeartPickup); }
                    else if (Obj.type == ObjType.PickupArrow)
                    { PlayerData.current.arrowsCurrent++; Assets.Play(Assets.sfxHeartPickup); }
                    else if (Obj.type == ObjType.PickupBomb)
                    { PlayerData.current.bombsCurrent++; Assets.Play(Assets.sfxHeartPickup); }
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

                    #region Reward the hero with chest contents

                    if (Obj.type == ObjType.ChestGold)
                    {
                        Functions_Entity.SpawnEntity(ObjType.ParticleRewardGold, Actor);
                        Assets.Play(Assets.sfxReward);
                        PlayerData.current.gold += 20;
                    }
                    else if (Obj.type == ObjType.ChestKey)
                    {
                        Functions_Entity.SpawnEntity(ObjType.ParticleRewardKey, Actor);
                        Assets.Play(Assets.sfxKeyPickup);
                        Functions_Dungeon.dungeon.bigKey = true;
                        ScreenManager.AddScreen(new ScreenDialog(Dialog.HeroGotKey));
                    }
                    else if (Obj.type == ObjType.ChestMap)
                    {
                        Functions_Entity.SpawnEntity(ObjType.ParticleRewardMap, Actor);
                        Assets.Play(Assets.sfxReward);
                        Functions_Dungeon.dungeon.map = true;
                    }
                    else if (Obj.type == ObjType.ChestHeartPiece)
                    {
                        if (WorldUI.pieceCounter == 3) //if this completes a heart, display the full heart reward
                        { Functions_Entity.SpawnEntity(ObjType.ParticleRewardHeartFull, Actor); }
                        else //this does not complete a heart, display the heart piece reward
                        { Functions_Entity.SpawnEntity(ObjType.ParticleRewardHeartPiece, Actor); }
                        Assets.Play(Assets.sfxReward);
                        PlayerData.current.heartPieces++;
                    }

                    #endregion


                    if (Obj.type != ObjType.ChestEmpty)
                    {   //if the chest is not empty, play the reward animation
                        Assets.Play(Assets.sfxChestOpen);
                        Functions_GameObject.SetType(Obj, ObjType.ChestEmpty);
                        Actor.state = ActorState.Reward; //set actor into reward state
                        Actor.lockTotal = 40; //lock for a prolonged time
                        //play an explosion particle to show the chest was opened
                        Functions_Entity.SpawnEntity(
                            ObjType.ParticleAttention,
                            Obj.compSprite.position.X,
                            Obj.compSprite.position.Y,
                            Direction.None);
                    }
                }
            }

            #endregion


            #region Vendors

            else if (Obj.group == ObjGroup.Vendor)
            {   //only HERO can open chests, and he must do so via the InteractionRec (A Button Press)
                if (Actor == Pool.hero && Actor.state == ActorState.Interact)
                { ScreenManager.AddScreen(new ScreenVendor(Obj)); }
                //story vendor interaction occurs in 'Other Interactive Objects' section below
            }

            #endregion


            #region Doors

            else if (Obj.group == ObjGroup.Door)
            {
                //exit pillars have no interactions whatsoever
                if (Obj.type == ObjType.ExitPillarLeft || Obj.type == ObjType.ExitPillarRight) { return; }


                #region Check Collisions with Door Types

                if (Obj.type == ObjType.DoorBoss)
                {
                    if (Actor == Pool.hero)
                    {   //only hero can open boss door, and must have dungeon key
                        if (Functions_Dungeon.dungeon.bigKey)
                        {
                            Functions_GameObject.SetType(Obj, ObjType.DoorOpen);
                            Assets.Play(Assets.sfxDoorOpen);
                            Functions_Entity.SpawnEntity(
                                ObjType.ParticleAttention,
                                Obj.compSprite.position.X,
                                Obj.compSprite.position.Y,
                                Direction.None);
                        }
                        else if (Actor.state == ActorState.Interact)
                        {   //throw a dialog screen explaining hero does not have big key
                            ScreenManager.AddScreen(new ScreenDialog(Dialog.DoesNotHaveKey));
                        }
                    }
                }
                else if (Obj.type == ObjType.Exit && Actor == Pool.hero)
                {   //only hero can exit dungeon
                    if (Functions_Dungeon.dungeonScreen.displayState == DisplayState.Opened)
                    {   //if dungeon screen is open, close it, perform interaction ONCE
                        DungeonRecord.beatDungeon = false;
                        Functions_Dungeon.dungeonScreen.exitAction = ExitAction.Overworld;
                        Functions_Dungeon.dungeonScreen.displayState = DisplayState.Closing;
                        //stop movement, prevents overlap with exit
                        Functions_Movement.StopMovement(Pool.hero.compMove);
                        Assets.Play(Assets.sfxDoorOpen);
                    }
                }
                else if (Obj.type == ObjType.DoorTrap)
                {   //trap doors push ALL actors
                    Functions_Movement.Push(Actor.compMove, Obj.direction, 1.0f);
                    Functions_Entity.SpawnEntity(
                        ObjType.ParticleDashPuff,
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y,
                        Direction.None);
                }

                #endregion


                #region Center Hero to Door, while allowing him to pass thru

                if (Actor == Pool.hero)
                {   //based on the door's direction, pull/center hero
                    if (Obj.direction == Direction.Up || Obj.direction == Direction.Down)
                    {   //gradually center hero to door
                        Actor.compMove.magnitude.X = (Obj.compSprite.position.X - Actor.compMove.position.X) * 0.11f;
                        //if hero is close to center of door, snap/lock hero to center of door
                        if (Math.Abs(Actor.compSprite.position.X - Obj.compSprite.position.X) < 2)
                        { Actor.compMove.newPosition.X = Obj.compSprite.position.X; }
                    }
                    else
                    {   //gradually center hero to door
                        Actor.compMove.magnitude.Y = (Obj.compSprite.position.Y - Actor.compMove.position.Y) * 0.11f;
                        //if hero is close to center of door, snap/lock hero to center of door
                        if (Math.Abs(Actor.compSprite.position.Y - Obj.compSprite.position.Y) < 2)
                        { Actor.compMove.newPosition.Y = Obj.compSprite.position.Y; }
                    }
                }

                #endregion

            }

            #endregion



            else if (Obj.group == ObjGroup.Object)
            {

                #region Story / Guide Object

                if (Obj.type == ObjType.VendorStory)
                {   //only HERO can interact with story vender
                    if (Actor == Pool.hero && Actor.state == ActorState.Interact)
                    {   //pass the story vendor to the dialog screen
                        //figure out what part of the story the hero is at, pass this dialog
                        ScreenManager.AddScreen(new ScreenDialog(Dialog.Default));
                    }
                }

                #endregion


                #region Misc Interactive Dungeon Objects

                else if (Obj.type == ObjType.BlockSpikes) { Functions_Battle.Damage(Actor, Obj); }
                else if (Obj.type == ObjType.ConveyorBelt)
                {   //push actor in belt's direction
                    Functions_Movement.Push(Actor.compMove, Obj.direction, 0.1f);
                }
                else if (Obj.type == ObjType.Bumper)
                {
                    Functions_Movement.Push(Actor.compMove,
                        Functions_Direction.GetRelativeDirection(Obj, Actor),
                        10.0f);
                    //actors can collide with bumper twice per frame, due to per axis collision checks
                    BounceBumper(Obj);

                }
                //lever, floor spikes, switch, bridge, flamethrower,
                //torch unlit, torch lit

                #endregion

            }
        }

        public static void Interact(GameObject ObjA, GameObject ObjB)
        {
            //Obj could be a projectile!
            //no blocking checks have been done yet

            #region Handle blocking object interactions

            if (ObjB.compCollision.blocking) //is the colliding object blocking?
            {
                if (ObjA.type == ObjType.ProjectileFireball ||
                    ObjA.type == ObjType.ProjectileArrow)
                { ObjA.lifeCounter = ObjA.lifetime; } //kill projectile
                else if (ObjA.type == ObjType.BlockSpikes) { BounceSpikeBlock(ObjA); }
            }

            #endregion


            #region Handle non-blocking object interactions

            else if (ObjB.type == ObjType.BlockSpikes)
            {
                if (ObjA.type == ObjType.BlockSpikes) { BounceSpikeBlock(ObjA); BounceSpikeBlock(ObjB); }
            }
            else if(ObjB.type == ObjType.Bumper)
            {
                if (ObjA.type == ObjType.BlockSpikes) { BounceSpikeBlock(ObjA); BounceBumper(ObjB); }
            }

            #endregion

        }

        public static void BounceSpikeBlock(GameObject SpikeBlock)
        {   //flip the block's direction to the opposite direction
            SpikeBlock.compMove.direction = Functions_Direction.GetOppositeDirection(SpikeBlock.compMove.direction);
            //push the block in it's new direction, out of this collision
            Functions_Movement.Push(SpikeBlock.compMove, SpikeBlock.compMove.direction, 5.0f);
            Assets.Play(Assets.sfxMetallicTap); //play the 'clink' sound effect                               
            Functions_Entity.SpawnEntity( //show that the object has been hit
                ObjType.ParticleHitSparkle,
                SpikeBlock.compSprite.position.X,
                SpikeBlock.compSprite.position.Y,
                Direction.None);
        }

        public static void BounceBumper(GameObject Bumper)
        {   //only play the bounce sound effect if the bumper hasn't been hit this frame
            if (Bumper.compSprite.scale < 1.5f) { Assets.Play(Assets.sfxBounce); }
            Bumper.compSprite.scale = 1.5f; //scale bumper up
            Functions_Entity.SpawnEntity(
                ObjType.ParticleAttention,
                Bumper.compSprite.position.X, 
                Bumper.compSprite.position.Y, 
                Direction.None);
        }

    }
}