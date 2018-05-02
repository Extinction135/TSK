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

                #region Fairy

                if (Obj.type == ObjType.Dungeon_Fairy)
                {
                    Functions_GameObject_Dungeon.UseFairy(Obj);
                }

                #endregion


                #region PitTrap

                else if (Obj.type == ObjType.Dungeon_PitTrap)
                {   //if hero collides with a PitTrapReady, it starts to open
                    Functions_GameObject.SetType(Obj, ObjType.Dungeon_Pit);
                    Obj.lifetime = 100; //signals that this is a new Trap
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
                }

                #endregion


                #region FloorSwitch

                else if (Obj.type == ObjType.Dungeon_Switch)
                { Functions_GameObject_Dungeon.ActivateSwitchObject(Obj); }

                #endregion

            }

            //these objects interact with ALL ACTORS


            #region Projectiles

            if (Obj.group == ObjGroup.Projectile)
            {
                //check exit conditions
                //projectiles shouldn't interact with dead actor's corpses
                if (Actor.state == ActorState.Dead) { return; }
                //some projectiles dont interact with actors in any way at all
                if (Obj.type == ObjType.ProjectileBomb
                    || Obj.type == ObjType.ProjectileExplodingBarrel
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


            //other objects
            else if (Obj.group == ObjGroup.Object)
            {

                #region FloorSpikes

                if (Obj.type == ObjType.Dungeon_SpikesFloorOn)
                {   //damage push actors (on ground) away from spikes
                    if (Actor.compMove.grounded) { Functions_Battle.Damage(Actor, Obj); }
                }

                #endregion


                #region SpikeBlocks

                else if (Obj.type == ObjType.Dungeon_BlockSpike)
                {   //damage push actors (on ground or in air) away from spikes
                    Functions_Battle.Damage(Actor, Obj);
                }

                #endregion


                #region ConveyorBelts

                else if (Obj.type == ObjType.Dungeon_ConveyorBeltOn)
                {   //belt move actors (on ground)
                    if (Actor.compMove.grounded)
                    {   //halt actor movement based on certain states
                        if (Actor.state == ActorState.Reward)
                        { Functions_Movement.StopMovement(Actor.compMove); }
                        else
                        { Functions_GameObject_Dungeon.ConveyorBeltPush(Actor.compMove, Obj); }
                    }
                }

                #endregion


                #region Bumpers

                else if (Obj.type == ObjType.Dungeon_Bumper)
                {   //limit bumper to bouncing only if it's returned to 100% scale
                    if (Obj.compSprite.scale == 1.0f)
                    { Functions_GameObject_Dungeon.BounceOffBumper(Actor.compMove, Obj); }
                }

                #endregion


                #region Pits

                else if (Obj.type == ObjType.Dungeon_Pit)
                {   //actors (on ground) fall into pits
                    if (Actor.compMove.grounded)
                    {
                        if(Obj.lifetime == 100)
                        {   //Pit was created THIS frame, earlier from an interaction with hero
                            //so for this frame, ignore this interaction with what is likely the hero
                            Obj.lifetime = 0;
                            return;
                        }


                        #region Drop any Obj Hero is carrying, hide Hero's shadow

                        else if (Actor == Pool.hero)
                        {   //check to see if hero should drop carryingObj
                            //if (Functions_Hero.carrying) { Functions_Hero.DropCarryingObj(); }
                            //hide hero's shadow upon pit collision
                            Functions_Hero.heroShadow.visible = false;
                        }

                        #endregion


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
                                Functions_Hero.heroShadow.visible = true;
                                //direct player's attention to hero's respawn pos
                                Functions_Particle.Spawn(
                                    ObjType.Particle_Attention,
                                    Functions_Level.currentRoom.spawnPos.X,
                                    Functions_Level.currentRoom.spawnPos.Y);
                            }
                            else
                            {   //handle enemy pit death (no loot, insta-death)
                                Assets.Play(Actor.sfxDeath); //play actor death sfx
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


                #region SwitchBlock UP

                else if (Obj.type == ObjType.Dungeon_SwitchBlockUp)
                {   //if actor is colliding with up block, convert up to down
                    Functions_GameObject.SetType(Obj, ObjType.Dungeon_SwitchBlockDown);
                }   //this is done because block just popped up and would block actor

                #endregion

                //bridge doesn't really do anything, it just doesn't cause actor to fall into a pit
            }
        }

        public static void InteractRoomObj(GameObject RoomObj, GameObject Object)
        {
            //show me the interaction types
            //Debug.WriteLine("" + RoomObj.type + " vs " + Object.type +
            //    " \t ts:" + ScreenManager.gameTime.TotalGameTime.Milliseconds);
            Pool.interactionsCount++; //count interaction


            #region Projectile v Blocking RoomObj Interactions

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
                    {   //explosions alter some roomObjects
                        if (Object.lifeCounter == 1) //perform these interactions only once
                        {   
                            if (RoomObj.type == ObjType.Dungeon_DoorBombable)
                            {   //explosions collapse doors
                                Functions_GameObject_Dungeon.CollapseDungeonDoor(RoomObj, Object);
                            }
                            else if (RoomObj.type == ObjType.Dungeon_Statue)
                            {   //explosions destroy statues
                                Functions_GameObject.DestroyObject(RoomObj, true, true);
                            }
                            else if(RoomObj.type == ObjType.Dungeon_WallStraight)
                            {   //explosions 'crack' normal walls
                                Functions_GameObject.SetType(RoomObj, 
                                    ObjType.Dungeon_WallStraightCracked);
                                Functions_Particle.Spawn(ObjType.Particle_Blast,
                                    RoomObj.compSprite.position.X,
                                    RoomObj.compSprite.position.Y);
                                Assets.Play(Assets.sfxShatter);
                                //drop debris particles
                            }
                            else if (RoomObj.type == ObjType.Dungeon_TorchUnlit)
                            {   //explosions light torches on fire
                                Functions_GameObject_Dungeon.LightTorch(RoomObj);
                            }

                            else if(RoomObj.type == ObjType.Wor_Bush)
                            {   //set a ground fire ON the stump sprite
                                Functions_Particle.Spawn(
                                    ObjType.Particle_FireGround,
                                    RoomObj.compSprite.position.X,
                                    RoomObj.compSprite.position.Y - 3);
                            }

                            else if(RoomObj.type == ObjType.Wor_Tree)
                            {
                                Functions_GameObject_World.DestroyTree(RoomObj);
                            }

                            //explosions also trigger common obj interactions
                            Functions_GameObject.HandleCommon(RoomObj,
                                //get the direction towards the roomObj from the explosion
                                Functions_Direction.GetOppositeCardinal(
                                    RoomObj.compSprite.position,
                                    Object.compSprite.position)
                            ); //this direction should be explosion pos vs. roomObj pos

                            //leave a 'burn mark' particle, with life 255
                            //this is just a darker spot on the ground that looks like a blast mark
                        }
                    }

                    #endregion


                    #region Fireball

                    else if (Object.type == ObjType.ProjectileFireball)
                    {   //fireball hit something, play bonk sound as audio confirmation
                        Assets.Play(Assets.sfxFireballDeath);
                        Functions_Projectile.Kill(Object); //fireball becomes explosion upon death
                    }

                    #endregion


                    #region Sword

                    else if (Object.type == ObjType.ProjectileSword)
                    {   //sword swipe causes soundfx to blocking objects
                        if (Object.lifeCounter == 1) //these events happen at start of sword swing
                        {

                            if (RoomObj.group == ObjGroup.Door)
                            {   //there are two soundfx that can play for a door
                                //hollow or metallic
                                if (RoomObj.type == ObjType.Dungeon_DoorBombable)
                                {
                                    Assets.Play(Assets.sfxTapHollow); //hollow
                                    Functions_Particle.Spawn(ObjType.Particle_Sparkle, Object);
                                } 
                                else if (RoomObj.type == ObjType.Dungeon_DoorOpen)
                                {
                                    //do nothing, there is no door
                                }
                                else
                                {   //this is a blocking, nondestructible door
                                    Assets.Play(Assets.sfxTapMetallic);
                                    Functions_Particle.Spawn(ObjType.Particle_Sparkle, Object);
                                }
                            }
                            else if(RoomObj.group == ObjGroup.Wall)
                            {   
                                //all sword hits on walls are metallic for now
                                Assets.Play(Assets.sfxTapMetallic);
                                Functions_Particle.Spawn(ObjType.Particle_Sparkle, Object);
                            }

                            //swords alter some objects
                            if (RoomObj.type == ObjType.Wor_Bush)
                            {   
                                Assets.Play(Assets.sfxEnemyHit);
                            }
                            else
                            {
                                //dont play any soundfx
                                //or, play metallic tap
                                Assets.Play(Assets.sfxTapMetallic);
                            }
                        }
                        else if (Object.lifeCounter == 4)
                        {   //these interactions happen 'mid swing'
                            //swords trigger common obj interactions
                            Functions_GameObject.HandleCommon(RoomObj, Object.compMove.direction);
                        }
                    }

                    #endregion


                    #region ExplodingBarrel

                    else if (Object.type == ObjType.ProjectileExplodingBarrel)
                    {   //stop barrels from moving thru blocking objects
                        Functions_Movement.StopMovement(Object.compMove);
                    }

                    #endregion


                    #region Boomerang

                    else if (Object.type == ObjType.ProjectileBoomerang)
                    {
                        //handle common interactions
                        Functions_GameObject.HandleCommon(RoomObj,
                            Functions_Movement.GetMovingDirection(Object.compMove));
                        //return the boomerang
                        Object.lifeCounter = 200; //return to caster
                        Functions_Movement.StopMovement(Object.compMove);
                        Functions_Movement.Push(Object.compMove, 
                            Functions_Direction.GetOppositeCardinal(
                                Object.compSprite.position, 
                                RoomObj.compSprite.position), 3.0f);
                        //pop a sparkle particle
                        Functions_Particle.Spawn(ObjType.Particle_Sparkle,
                            Object.compSprite.position.X + 4,
                            Object.compSprite.position.Y + 4);
                        Assets.Play(Assets.sfxTapMetallic);
                    }

                    #endregion


                    return; //projectile interactions complete
                }

                //there are no blocking obj vs obj interactions
                //an interaction is an overlap not handled by collision system
                //two blocking objs could never overlap or interact
            }

            #endregion


            //Handle Object vs NonBlocking RoomObj


            //object.type checks

            #region Pet / Animals

            if (Object.type == ObjType.Pet_Chicken || Object.type == ObjType.Pet_Dog)
            {   //trigger switches, bounce off bumpers
                if (RoomObj.type == ObjType.Dungeon_Switch)
                { Functions_GameObject_Dungeon.ActivateSwitchObject(RoomObj); }
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



            //roomObj.type checks

            #region FloorSpikes

            if (RoomObj.type == ObjType.Dungeon_SpikesFloorOn)
            {   
                if (Object.compMove.grounded)
                {   
                    if(Object.type == ObjType.Dungeon_Statue)
                    {   //destroy boss statues and pop loot
                        Functions_GameObject.DestroyObject(Object, true, true);
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
                { Functions_GameObject.Kill(Object); }
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
            {
                /*
                //check to see if we should ground any thrown pot projectile
                if (Object.type == ObjType.ProjectilePot)
                {   //if this is the last frame of the projectile pot, ground it
                    if (Object.lifeCounter > Object.lifetime - 5) //dont let it die
                    { Object.compMove.grounded = true; Object.lifeCounter = 3; }
                }
                */

                //drag any object into the pit
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
                {   //cut the grass
                    Functions_GameObject_World.CutTallGrass(RoomObj);
                    //leave some ground fire 
                    Functions_Particle.Spawn(
                        ObjType.Particle_FireGround,
                        RoomObj.compSprite.position.X,
                        RoomObj.compSprite.position.Y);
                }
                else if (Object.type == ObjType.ProjectileSword)
                {   //cut the grass
                    Functions_GameObject_World.CutTallGrass(RoomObj);
                }
            }

            #endregion


        }

    }
}