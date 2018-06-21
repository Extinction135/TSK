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
        public static int i;


        public static void CheckInteractions(Actor Actor, Boolean checkProjectiles, Boolean checkRoomObjs)
        {
            //if actor is hero, and clipping is enabled, then no interactions happen
            if(Actor == Pool.hero & Flags.Clipping) { return; }

            if (checkProjectiles)
            {   //loop thru projectile list, check overlaps, pass to Interact()
                for (i = 0; i < Pool.projectileCount; i++)
                {
                    if (Actor.compCollision.rec.Intersects(Pool.projectilePool[i].compCollision.rec))
                    { InteractActor(Actor, Pool.projectilePool[i]); }
                }
            }
            if (checkRoomObjs)
            {   //loop thru entity list, check overlaps, pass to Interact()
                for (i = 0; i < Pool.roomObjCount; i++)
                {
                    if (Actor.compCollision.rec.Intersects(Pool.roomObjPool[i].compCollision.rec))
                    { InteractActor(Actor, Pool.roomObjPool[i]); }
                }
            }
        }

        public static void CheckInteractions(GameObject Object)
        {   //this is ANY Object against RoomObj list
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active)
                {
                    if (Object.compCollision.rec.Intersects(Pool.roomObjPool[i].compCollision.rec))
                    {   //perform self-check to prevent self overlap interaction
                        if (Object != Pool.roomObjPool[i]) { InteractRoomObj(Pool.roomObjPool[i], Object); }
                    }
                }
            }
        }

        public static void InteractActor(Actor Actor, GameObject Obj)
        {   //Obj can be Entity or RoomObj, check for hero state first
            //ensure the object is active - have we done this in the calling code?
            if (!Obj.active) { return; } //inactive objects are denied interaction
            Pool.interactionsCount++; //count interaction

            //Hero Specific Interactions
            if (Actor == Pool.hero)
            {
                //group checks

                #region Pickups

                if (Obj.group == ObjGroup.Pickup)
                {
                    Functions_Pickup.HandleEffect(Obj);
                    return;
                }

                #endregion


                #region Doors

                else if (Obj.group == ObjGroup.Door)
                {   //handle hero interaction with exit door
                    if (Obj.type == ObjType.Dungeon_Exit)
                    {   //stop movement, prevents overlap with exit
                        Functions_Movement.StopMovement(Pool.hero.compMove);
                        if (Functions_Level.levelScreen.displayState == DisplayState.Opened)
                        {   
                            DungeonRecord.Reset();
                            Assets.Play(Assets.sfxDoorOpen);
                            Functions_Level.CloseLevel(ExitAction.ExitDungeon);
                        }
                        return; //dont center or move hero anymore
                    }
                    //center Hero to Door horiontally or vertically
                    if (Obj.direction == Direction.Up || Obj.direction == Direction.Down)
                    {   //gradually center hero to door
                        Pool.hero.compMove.magnitude.X = (Obj.compSprite.position.X - Pool.hero.compMove.position.X) * 0.11f;
                        //if hero is close to center of door, snap/lock hero to center of door
                        if (Math.Abs(Pool.hero.compSprite.position.X - Obj.compSprite.position.X) < 2)
                        { Pool.hero.compMove.newPosition.X = Obj.compSprite.position.X; }
                    }
                    else
                    {   //gradually center hero to door
                        Pool.hero.compMove.magnitude.Y = (Obj.compSprite.position.Y - Pool.hero.compMove.position.Y) * 0.11f;
                        //if hero is close to center of door, snap/lock hero to center of door
                        if (Math.Abs(Pool.hero.compSprite.position.Y - Obj.compSprite.position.Y) < 2)
                        { Pool.hero.compMove.newPosition.Y = Obj.compSprite.position.Y; }
                    }
                }

                #endregion


                //type checks

                #region Map

                if (Obj.type == ObjType.Dungeon_Map)
                {
                    if (Level.map == false) //make sure hero doesn't already have map
                    {
                        Functions_Pool.Release(Obj); //hero collects map obj
                        Level.map = true; //flip map true
                        Functions_Actor.SetRewardState(Pool.hero);
                        Functions_Actor.SetAnimationGroup(Pool.hero);
                        Functions_Actor.SetAnimationDirection(Pool.hero); 
                        Functions_Particle.Spawn(ObjType.Particle_RewardMap, Pool.hero);
                        Assets.Play(Assets.sfxReward); //play reward / boss defeat sfx

                        if (Flags.ShowDialogs)
                        { ScreenManager.AddScreen(new ScreenDialog(AssetsDialog.HeroGotMap)); }
                    }
                    return;
                }

                #endregion


                #region Fairy

                else if (Obj.type == ObjType.Dungeon_Fairy)
                {
                    Functions_GameObject_Dungeon.UseFairy(Obj);
                    return;
                }

                #endregion


                #region Roofs

                else if (Obj.type == ObjType.Wor_Build_Roof_Bottom
                    || Obj.type == ObjType.Wor_Build_Roof_Top
                    || Obj.type == ObjType.Wor_Build_Roof_Chimney)
                {   //if hero touches a roof, hide all the roofs
                    //Functions_GameObject_World.HideRoofs();
                    Functions_Hero.underRoof = true;
                    return;
                }

                #endregion


            }

            //these objects interact with ALL ACTORS
            //note that some objs, like floor switches, are handled by Functions_Ai.HandleObj()


            #region Projectiles

            if (Obj.group == ObjGroup.Projectile)
            {
                //check exit conditions
                //projectiles shouldn't interact with dead actor's corpses
                if (Actor.state == ActorState.Dead) { return; }
                //some projectiles dont interact with actors in any way at all
                if (Obj.type == ObjType.ProjectileBomb
                    || Obj.type == ObjType.ProjectileGroundFire
                    || Obj.type == ObjType.ProjectileBow
                    )
                { return; }
                //check for boomerang interaction with hero
                else if(Obj.type == ObjType.ProjectileBoomerang & Actor == Pool.hero)
                { return; }

                //check specific projectile interactions

                //check for collision between net and actor
                else if (Obj.type == ObjType.ProjectileNet)
                {   //make sure actor isn't in hit/dead state
                    if (Actor.state == ActorState.Dead || Actor.state == ActorState.Hit) { return; }
                    Obj.lifeCounter = Obj.lifetime; //kill projectile
                    Obj.compCollision.rec.X = -1000; //hide hitBox (prevents multiple actor collisions)
                    Functions_Bottle.Bottle(Actor); //try to bottle the actor
                    Functions_Pool.Release(Obj); //release the net
                }

                //if sword projectile is brand new, spawn hit particle
                else if (Obj.type == ObjType.ProjectileSword)
                {
                    if (Obj.lifeCounter == 1)
                    { Functions_Particle.Spawn(ObjType.Particle_Sparkle, Obj); }
                }

                else if(Obj.type == ObjType.ProjectileBush
                    || Obj.type == ObjType.ProjectilePot 
                    || Obj.type == ObjType.ProjectilePotSkull)
                {
                    Obj.lifeCounter = Obj.lifetime; //kill projectile next frame
                }

                //all actors take damage from these projectiles
                Functions_Battle.Damage(Actor, Obj); //sets actor into hit state
            }

            #endregion


            #region Doors

            else if (Obj.group == ObjGroup.Door)
            {
                if (Obj.type == ObjType.Dungeon_DoorTrap)
                {   //trap doors push ALL actors
                    Functions_Movement.Push(Actor.compMove, Obj.direction, 1.0f);
                }
            }

            #endregion


            #region Ditches

            else if (Obj.group == ObjGroup.Ditch)
            {   //if the actor is flying, ignore interaction with ditch
                if (Actor.compMove.grounded == false) { return; }
                //if ditch getsAI, then ditch is in it's filled state
                if (Obj.getsAI)
                {   //display animated ripple at actor's feet
                    Functions_Actor.DisplayWetFeet(Actor);
                }
            }

            #endregion


            //Objects
            else if (Obj.group == ObjGroup.Object)
            {
                //Phase 1 - objects that interact with flying and grounded actors

                #region SpikeBlocks

                if (Obj.type == ObjType.Dungeon_BlockSpike)
                {   //damage push actors (on ground or in air) away from spikes
                    Functions_Battle.Damage(Actor, Obj);
                }

                #endregion


                #region Bumpers

                else if (Obj.type == ObjType.Dungeon_Bumper)
                {   //limit bumper to bouncing only if it's returned to 100% scale
                    if (Obj.compSprite.scale == 1.0f)
                    { Functions_GameObject_Dungeon.BounceOffBumper(Actor.compMove, Obj); }
                }

                #endregion


                #region SwitchBlock UP

                else if (Obj.type == ObjType.Dungeon_SwitchBlockUp)
                {   //if actor is colliding with up block, convert up to down
                    Functions_GameObject.SetType(Obj, ObjType.Dungeon_SwitchBlockDown);
                }   //this is done because block just popped up and would block actor

                #endregion


                //Phase 2 - all flying actors bail from method - they're done
                if (Actor.compMove.grounded == false) { return; }


                //Phase 3 - objects that interact with grounded actors only (all remaining)

                #region FloorSpikes

                if (Obj.type == ObjType.Dungeon_SpikesFloorOn)
                {   //damage push actors (on ground) away from spikes
                    Functions_Battle.Damage(Actor, Obj);
                }

                #endregion


                #region ConveyorBelts

                else if (Obj.type == ObjType.Dungeon_ConveyorBeltOn)
                {
                    //halt actor movement based on certain states
                    if (Actor.state == ActorState.Reward)
                    { Functions_Movement.StopMovement(Actor.compMove); }
                    else
                    { Functions_GameObject_Dungeon.ConveyorBeltPush(Actor.compMove, Obj); }
                    
                }

                #endregion


                #region Pits

                else if (Obj.type == ObjType.Dungeon_Pit)
                {   //actors (on ground) fall into pits
                    //if an actor wasn't hit into the pit, push the actor away
                    if (Actor != Pool.hero & Actor.state != ActorState.Hit)
                    {
                        //push actor away from pit
                        Functions_Movement.Push(Actor.compMove,
                            Functions_Direction.GetOppositeCardinal(
                                Actor.compMove.position,
                                Obj.compSprite.position),
                            3.0f);

                    }
                    else//pull actor into pit
                    {   //this is hero, or any enemy in hit state
                        if (Actor.carrying) //toss whatever actor might be carrying
                        { Functions_Actor.Throw(Actor); }

                        //set actor's state
                        //dead actors simply stay dead as they fall into a pit
                        //else actors are set into a hit state as they fall
                        // **we actually only need to do this once, instead of every frame**
                        if (Actor.state != ActorState.Dead) { Actor.state = ActorState.Hit; }
                        Actor.stateLocked = true;
                        Actor.lockCounter = 0;
                        Actor.lockTotal = 45;
                        Actor.compMove.speed = 0.0f;


                        #region Continuous collision (each frame) - ALL ACTORS

                        //gradually pull actor into pit's center, manually update the actor's position
                        Actor.compMove.magnitude = (Obj.compSprite.position - Actor.compSprite.position) * 0.25f;

                        //if actor is near to pit center, begin/continue falling state
                        if (Math.Abs(Actor.compSprite.position.X - Obj.compSprite.position.X) < 2)
                        {
                            if (Math.Abs(Actor.compSprite.position.Y - Obj.compSprite.position.Y) < 2)
                            {   //begin actor falling state
                                if (Actor.compSprite.scale == 1.0f) { Assets.Play(Assets.sfxActorFall); }
                                //continue falling state, scaling actor down
                                Actor.compSprite.scale -= 0.03f;
                            }
                        }

                        #endregion


                        #region End State of actor -> pit collision - ALL ACTORS

                        if (Actor.compSprite.scale < 0.8f)
                        {   //actor has reached scale, fallen into pit completely
                            Functions_GameObject_Dungeon.PlayPitFx(Obj);
                            if (Actor == Pool.hero)
                            {   //send hero back to last door he passed thru
                                Assets.Play(Assets.sfxActorLand); //play actor land sfx
                                Functions_Hero.SpawnInCurrentRoom();
                                //direct player's attention to hero's respawn pos
                                Functions_Particle.Spawn(
                                    ObjType.Particle_Attention,
                                    Functions_Level.currentRoom.spawnPos.X,
                                    Functions_Level.currentRoom.spawnPos.Y);
                            }
                            else
                            {   //handle enemy pit death (no loot, insta-death)
                                Assets.Play(Actor.sfx.kill); //play actor death sfx
                                Functions_Pool.Release(Actor); //release this actor back to pool
                            }
                        }

                        #endregion

                    }
                    
                }

                #endregion


                #region Ice

                else if (Obj.type == ObjType.Dungeon_IceTile)
                {
                    Functions_GameObject_Dungeon.SlideOnIce(Actor.compMove);
                }

                #endregion


                #region Tall grass

                else if (Obj.type == ObjType.Wor_Grass_Tall)
                {   //unhide + place grassy feet at actor's feet
                    Actor.feetFX.visible = true;
                    Actor.feetAnim.currentAnimation = AnimationFrames.ActorFX_GrassyFeet;
                    if (Actor == Pool.hero)
                    {   //actor must be hero, and moving
                        if (Actor.state == ActorState.Move)
                        { Assets.Play(Assets.sfxGrassWalk); }
                    }
                }

                #endregion


                #region Water

                else if(Obj.type == ObjType.Wor_Water)
                {
                    if(Actor.createSplash == false)
                    {   //actor is transitioning into water this frame
                        Functions_Particle.Spawn(ObjType.Particle_Splash,
                            Actor.compSprite.position.X,
                            Actor.compSprite.position.Y);
                        //"1 splash only" - Marko Ramius
                        Actor.createSplash = true;
                        //kill the actor's momentum, adding weight to splash
                        Functions_Movement.StopMovement(Actor.compMove);
                    }

                    //note water interaction comes before coastline interaction
                    Actor.swimming = true; 
                    Actor.compMove.friction = World.frictionWater;
                }

                #endregion


                #region Coastlines

                else if (Obj.type == ObjType.Wor_Coastline_Corner_Exterior
                    || Obj.type == ObjType.Wor_Coastline_Corner_Interior
                    || Obj.type == ObjType.Wor_Coastline_Straight)
                {   //display animated ripple at actor's feet
                    Functions_Actor.DisplayWetFeet(Actor);
                }

                #endregion


                #region PitTrap

                else if (Obj.type == ObjType.Dungeon_PitTrap)
                {   //if hero collides with a PitTrapReady, it starts to open
                    Functions_GameObject.SetType(Obj, ObjType.Dungeon_Pit);
                    Assets.Play(Assets.sfxShatter); //play collapse sound
                    Functions_Particle.Spawn(ObjType.Particle_Attention,
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y);
                    Functions_Particle.Spawn(ObjType.Particle_ImpactDust,
                        Obj.compSprite.position.X + 4,
                        Obj.compSprite.position.Y - 8);
                    //Functions_Particle.ScatterDebris(Obj.compSprite.position);
                    //create pit teeth over new pit obj
                    Functions_GameObject.Spawn(ObjType.Dungeon_PitTeethTop,
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y,
                        Direction.Down);
                    Functions_GameObject.Spawn(ObjType.Dungeon_PitTeethBottom,
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y,
                        Direction.Down);
                    return; //required
                    //otherwise code below will evaluate this obj as a pitTrap
                    //and will lock hero into being pulled into pit, no recourse
                }

                #endregion


                //bridge just doesn't cause actor to fall into pit
            }
        }

        public static void InteractRoomObj(GameObject RoomObj, GameObject Object)
        {
            //show me the interaction types
            //Debug.WriteLine("" + RoomObj.type + " vs " + Object.type +
            //    " \t ts:" + ScreenManager.gameTime.TotalGameTime.Milliseconds);
            Pool.interactionsCount++; //count interaction



            
            #region Setup Special RoomObjs prior to Interaction

            //seekers dont block, but we want projectiles to kill them
            if (RoomObj.type == ObjType.Wor_SeekerExploder)
            { RoomObj.compCollision.blocking = true; }
            //so we temporarily turn on their blocking, then check, then turn off

            #endregion







            // *** Projectile vs Blocking RoomObj Interactions *** \\

            if (RoomObj.compCollision.blocking)
            {   //Handle Projectile vs Blocking RoomObj 
                if (Object.group == ObjGroup.Projectile)
                {

                    #region Arrow

                    if (Object.type == ObjType.ProjectileArrow)
                    {   //arrows trigger common obj interactions
                        Functions_GameObject.HandleCommon(RoomObj, Object.compMove.direction);
                        //arrows die upon blocking collision
                        Functions_Projectile.Kill(Object);
                    }

                    #endregion


                    #region Bomb

                    else if (Object.type == ObjType.ProjectileBomb)
                    {   //stop bombs from moving thru blocking objects
                        Functions_Movement.StopMovement(Object.compMove);
                    }

                    #endregion


                    #region Explosions

                    else if (Object.type == ObjType.ProjectileExplosion)
                    {   
                        if (Object.lifeCounter == 1) //perform these interactions only once
                        {   //explosions call power level 2 destruction routines
                            Functions_GameObject.BlowUp(RoomObj, Object);
                        }
                    }

                    #endregion


                    #region Fireball

                    else if (Object.type == ObjType.ProjectileFireball)
                    {   //fireball becomes explosion upon death
                        Functions_Projectile.Kill(Object);
                    }

                    #endregion


                    #region Sword & Shovel

                    else if (Object.type == ObjType.ProjectileSword
                        || Object.type == ObjType.ProjectileShovel)
                    {   
                        //swords and shovels cause soundfx to blocking objects
                        if (Object.lifeCounter == 1) //these events happen only at start
                        {   //bail if pro is hitting open door, else sparkle + hit sfx
                            if (RoomObj.type == ObjType.Dungeon_DoorOpen) { return; }
                            //center sparkle to hit obj
                            Functions_Particle.Spawn(ObjType.Particle_Sparkle, 
                                RoomObj.compSprite.position.X + 4,
                                RoomObj.compSprite.position.Y + 4);
                            Assets.Play(RoomObj.sfx.hit);
                        }
                        else if (Object.lifeCounter == 4)
                        {   //these interactions happen mid-swing
                            Functions_GameObject.HandleCommon(RoomObj, Object.compMove.direction);
                        }
                    }

                    #endregion


                    #region ExplodingObject

                    else if (Object.type == ObjType.ExplodingObject)
                    {   //stop exploding object from moving thru blocking objects
                        Functions_Movement.StopMovement(Object.compMove);
                    }

                    #endregion


                    #region Boomerang

                    else if (Object.type == ObjType.ProjectileBoomerang)
                    {

                        //kill roomObj enemies, just like a sword would
                        if (RoomObj.group == ObjGroup.Enemy)
                        {
                            Functions_GameObject.HandleCommon(RoomObj, Object.compMove.direction);
                        }


                        #region Activate a limited set of RoomObjs

                        //activate levers
                        if (RoomObj.type == ObjType.Dungeon_LeverOff
                            || RoomObj.type == ObjType.Dungeon_LeverOn)
                        { Functions_GameObject_Dungeon.ActivateLeverObjects(); }
                        //activate explosive barrels
                        else if (RoomObj.type == ObjType.Dungeon_Barrel)
                        {
                            RoomObj.compMove.direction = Object.compMove.direction;
                            Functions_GameObject_Dungeon.HitBarrel(RoomObj);
                        }
                        //activate switch block buttons
                        else if (RoomObj.type == ObjType.Dungeon_SwitchBlockBtn)
                        {
                            Functions_GameObject_Dungeon.FlipSwitchBlocks(RoomObj);
                        }
                        //destroy bushes
                        else if (RoomObj.type == ObjType.Wor_Bush)
                        {
                            Functions_GameObject.HandleCommon(RoomObj, Object.compMove.direction);
                        }

                        #endregion


                        //if this is the initial hit, set the boomerang
                        //into a return state, pop an attention particle
                        if (Object.lifeCounter < Object.interactiveFrame)
                        {   //set boomerang into return mode
                            Object.lifeCounter = 200;
                            Functions_Particle.Spawn(
                                ObjType.Particle_Attention,
                                Object.compSprite.position.X + 4,
                                Object.compSprite.position.Y + 4);
                        }

                        //stop all boomerang movement, bounce off object
                        Functions_Movement.StopMovement(Object.compMove);
                        Functions_Movement.Push(Object.compMove,
                            Functions_Direction.GetOppositeCardinal(
                                Object.compSprite.position,
                                RoomObj.compSprite.position), 4.0f);

                        //determine what type of soundfx to play
                        if (RoomObj.group == ObjGroup.Wall)
                        { Assets.Play(Assets.sfxTapMetallic); }
                        else if (RoomObj.type == ObjType.Dungeon_DoorBombable)
                        { Assets.Play(Assets.sfxTapHollow); }
                        else if (RoomObj.type == ObjType.Dungeon_DoorOpen)
                        { } //literally nothing
                        else if (RoomObj.group == ObjGroup.Door)
                        { Assets.Play(Assets.sfxTapMetallic); }
                        else //play default boomerang hit sfx
                        { Assets.Play(Assets.sfxActorLand); }

                    }

                    #endregion


                    #region GroundFires

                    else if (Object.type == ObjType.ProjectileGroundFire)
                    {   //blocking objs only here, btw

                        //groundfires can burn trees
                        if (RoomObj.type == ObjType.Wor_Tree)
                        {
                            Functions_GameObject_World.BurnTree(RoomObj);
                        }
                        //groundfires can spread across bushes
                        else if(RoomObj.type == ObjType.Wor_Bush)
                        {   //spread the fire 
                            Functions_Projectile.Spawn(
                                ObjType.ProjectileGroundFire,
                                RoomObj.compSprite.position.X,
                                RoomObj.compSprite.position.Y - 3);
                            Functions_GameObject_World.DestroyBush(RoomObj);
                            Assets.Play(Assets.sfxLightFire);
                        }
                        else if(RoomObj.type == ObjType.Dungeon_Barrel)
                        {   //dont push the barrel in any direction
                            RoomObj.compMove.direction = Direction.None;
                            Functions_GameObject_Dungeon.HitBarrel(RoomObj);
                        }
                    }

                    #endregion


                    #region Lightning Bolt

                    else if (Object.type == ObjType.ProjectileLightningBolt)
                    {   
                        if (Object.lifeCounter == 1) //perform these interactions only once
                        {   //bolts call power level 2 destruction routines
                            Functions_GameObject.BlowUp(RoomObj, Object);
                        }
                    }

                    #endregion


                    #region Thrown Objects (Bush, Pot)

                    else if (Object.type == ObjType.ProjectileBush
                        || Object.type == ObjType.ProjectilePot
                        || Object.type == ObjType.ProjectilePotSkull)
                    {
                        //handle common interactions caused by thrown objs
                        Functions_GameObject.HandleCommon(RoomObj, Object.compMove.direction);
                        //thrown objs die upon blocking collision
                        Functions_Projectile.Kill(Object);
                    }
                    
                    #endregion


                    return; //projectile interactions complete
                }

                //an interaction is an overlap not handled by collision system
                //there are no blocking obj vs obj interactions
                //two blocking objs could never overlap or interact
            }




            
            #region Reset Special Objects post Interaction

            //projectile checks against the seeker complete
            if (RoomObj.type == ObjType.Wor_SeekerExploder)
            { RoomObj.compCollision.blocking = false; }
            //return seeker to non-blocking state, allowing overlap

            #endregion

            





            // *** Handle Obj vs Obj *** \\


            //object.type checks

            #region Pet / Animals

            if (Object.type == ObjType.Pet_Dog)
            {   
                //dogs can swim
                if(RoomObj.type == ObjType.Wor_Water) { Object.inWater = true; }

                //dogs bounce
                else if (RoomObj.type == ObjType.Dungeon_Bumper)
                { Functions_GameObject_Dungeon.BounceOffBumper(Object.compMove, RoomObj); }
            }

            #endregion


            #region ConveyorBelts

            else if (Object.type == ObjType.Dungeon_ConveyorBeltOn)
            {   //if obj is moveable and on ground, move it
                if (RoomObj.compMove.moveable && RoomObj.compMove.grounded)
                { Functions_GameObject_Dungeon.ConveyorBeltPush(RoomObj.compMove, Object); }
            }

            #endregion


            #region SpikeBlock

            else if (Object.type == ObjType.Dungeon_BlockSpike)
            {   //the roomObj must be blocking for blockSpike to bounce
                if (RoomObj.compCollision.blocking)
                {   //reverse the direction of the spikeBlock
                    Functions_GameObject_Dungeon.BounceSpikeBlock(Object);
                    //spikeblocks trigger common obj interactions
                    Functions_GameObject.HandleCommon(RoomObj, Object.compMove.direction);
                }
                //here blockSpikes could trigger non-blocking roomObj interactions
                //whatever those are...
            }

            #endregion


            #region SeekerExploders

            else if (Object.type == ObjType.Wor_SeekerExploder)
            {
                if (RoomObj.compCollision.blocking)
                {   
                    Object.lifeCounter = Object.lifetime; //explode this frame
                    Functions_GameObject.HandleCommon(Object, Direction.None);
                }
            }

            #endregion







            //roomObj.type checks

            #region FloorSpikes

            if (RoomObj.type == ObjType.Dungeon_SpikesFloorOn)
            {   
                if (Object.compMove.grounded)
                {   
                    if(Object.type == ObjType.Dungeon_Statue)
                    {   //destroy boss statues and pop loot
                        Functions_GameObject.Kill(Object, true, true);
                    }
                    else
                    {   //push obj in opposite direction and destroy it
                        Functions_GameObject.HandleCommon(Object,
                            Functions_Direction.GetOppositeCardinal(
                                Object.compMove.position,
                                RoomObj.compMove.position)
                            );
                    }
                }
            }

            #endregion


            #region Trap Door

            else if (RoomObj.type == ObjType.Dungeon_DoorTrap)
            {   //prevent obj from passing thru door
                Functions_Movement.RevertPosition(Object.compMove);

                //actually, we should be pushing the object the same way we push an actor

                //kill specific projectiles / objects
                if (Object.type == ObjType.ProjectileFireball
                    || Object.type == ObjType.ProjectileArrow)
                { Functions_Pool.Release(Object); }
            }

            #endregion

            
            #region Bumper

            else if (RoomObj.type == ObjType.Dungeon_Bumper)
            {   //limit bumper to bouncing only if it's returned to 100% scale
                if (RoomObj.compSprite.scale != 1.0f) { return; }
                //specific projectiles cannot be bounced off bumper
                if (Object.type == ObjType.ProjectileExplosion
                    || Object.type == ObjType.ProjectileNet
                    || Object.type == ObjType.ProjectileSword
                    )
                { return; }
                //one of the two objects must be moving,
                //in order to trigger a bounce interaction
                if (!RoomObj.compMove.moving & !Object.compMove.moving)
                { return; }
                //all other objects are bounced
                Functions_GameObject_Dungeon.BounceOffBumper(Object.compMove, RoomObj);
                //rotate bounced projectiles
                if (Object.group == ObjGroup.Projectile)
                {
                    Object.direction = Object.compMove.direction;
                    Functions_GameObject.SetRotation(Object);
                }
            }

            #endregion

            
            #region Pits

            else if (RoomObj.type == ObjType.Dungeon_Pit)
            {   //drag any object into the pit
                Functions_GameObject_Dungeon.DragIntoPit(Object, RoomObj);
            }

            #endregion


            #region IceTiles

            else if(RoomObj.type == ObjType.Dungeon_IceTile)
            {   //only objects on the ground can slide on ice
                if (Object.compMove.grounded)
                {
                    Functions_GameObject_Dungeon.SlideOnIce(Object.compMove);
                }
            }

            #endregion


            #region Fairy

            else if (RoomObj.type == ObjType.Dungeon_Fairy)
            {
                if (Object.type == ObjType.ProjectileNet)
                {   
                    if(Object.lifeCounter < Object.lifetime) //net is still young
                    {
                        //a net has overlapped a fairy (and only a hero can create a net)
                        //attempt to bottle the fairy into one of hero's bottles
                        Functions_Bottle.Bottle(RoomObj);
                        //kill net NEXT frame, this allows it to be drawn
                        //while the dialog screen appears over the level screen
                        Object.lifeCounter = Object.lifetime;
                        //hide hitBox (prevents multiple collisions)
                        Object.compCollision.rec.X = -1000;
                        //the beginning if() prevents the net from Interacting()
                        //on the next frame, when it is dying from this interaction
                        //without the check, the net interacts twice, across two frames
                        //causing two dialog screens to pop, which feels buggy
                    }
                }
                else if (Object.type == ObjType.Dungeon_Bumper)
                { Functions_GameObject_Dungeon.BounceOffBumper(RoomObj.compMove, Object); }
            }

            #endregion


            #region Tall Grass

            else if (RoomObj.type == ObjType.Wor_Grass_Tall)
            {
                if (Object.type == ObjType.ProjectileExplosion)
                {   //pass the obj's direction into the grass (fake inertia)
                    RoomObj.compMove.direction = Object.direction;
                    Functions_GameObject_World.CutTallGrass(RoomObj);
                    //add some ground fire 
                    Functions_Projectile.Spawn(
                        ObjType.ProjectileGroundFire,
                        RoomObj.compSprite.position.X,
                        RoomObj.compSprite.position.Y - 3);
                }
                else if (Object.type == ObjType.ProjectileSword)
                {   //pass the obj's direction into the grass (fake inertia)
                    RoomObj.compMove.direction = Object.direction;
                    Functions_GameObject_World.CutTallGrass(RoomObj);
                }
                else if (Object.type == ObjType.ProjectileGroundFire)
                {   //'burn' the grass
                    Functions_GameObject_World.CutTallGrass(RoomObj);
                    //spread the fire 
                    Functions_Projectile.Spawn(
                        ObjType.ProjectileGroundFire,
                        RoomObj.compSprite.position.X,
                        RoomObj.compSprite.position.Y - 3);
                    //Assets.Play(Assets.sfxLightFire);
                }
            }

            #endregion


            #region Water (objects that 'fall' into water - draggable objs)

            else if(RoomObj.type == ObjType.Wor_Water)
            {
                //if object is not of the general object group, bail
                //otherwise projecctiles (+ others) splash into water upon contact
                if (Object.group != ObjGroup.Object) { return; }

                //if an obj is moveable, then hero can push it, then it should sink in water
                if (Object.compMove.moveable)
                {
                    //if object's hitBox is disabled, then obj shouldn't sink, ignore
                    if (Object.compCollision.blocking == false) { return; }

                    //if object's sprite center touches the water tile, sink it
                    if (RoomObj.compCollision.rec.Contains(Object.compSprite.position))
                    {
                        //release the obj, create a splash particle centered to object
                        Functions_Particle.Spawn(ObjType.Particle_Splash, Object);
                        Functions_Pool.Release(Object);
                    }
                    //otherwise the obj sinks as soon as it touches a water tile,
                    //which looks early. and bad. cause we tried it. no good.
                }
            }

            #endregion


            #region Roofs - collapse from explosions + bolts

            else if (RoomObj.type == ObjType.Wor_Build_Roof_Bottom
                || RoomObj.type == ObjType.Wor_Build_Roof_Top
                || RoomObj.type == ObjType.Wor_Build_Roof_Chimney)
            {
                if (Object.type == ObjType.ProjectileExplosion 
                    || Object.type == ObjType.ProjectileLightningBolt)
                {   //begin cascading roof collapse
                    Functions_GameObject_World.CollapseRoof(RoomObj);
                }
            }

            #endregion


            #region Debris - debris removal

            //if debris roomObj overlaps other objs, remove it
            else if (RoomObj.type == ObjType.Wor_Debris)
            {
                if(Object.compCollision.blocking)
                {   //any blocking obj takes priority over debris
                    Functions_Pool.Release(RoomObj);
                }
                //these objs take priority over debris
                if (Object.type == ObjType.Wor_Debris //there can be only one
                    || Object.type == ObjType.Wor_Flowers
                    || Object.type == ObjType.Wor_Grass_Tall
                    || Object.type == ObjType.Wor_Bush_Stump
                    || Object.type == ObjType.Wor_Tree_Stump
                    )
                {
                    Functions_Pool.Release(RoomObj);
                }
            }

            #endregion


            #region Open Doors - collapse from explosions + bolts

            else if (RoomObj.type == ObjType.Wor_Build_Door_Open)
            {
                if (Object.type == ObjType.ProjectileExplosion
                    || Object.type == ObjType.ProjectileLightningBolt)
                {   //destroy obj
                    Functions_GameObject.Kill(RoomObj, false, true);
                }
            }

            #endregion






        }

    }
}