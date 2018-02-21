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

        public static void CheckInteractions(Actor Actor, Boolean checkEntities, Boolean checkRoomObjs)
        {
            if (checkEntities)
            {   //loop thru entity list, check overlaps, pass to Interact()
                for (i = 0; i < Pool.entityCount; i++)
                {
                    if (Actor.compCollision.rec.Intersects(Pool.entityPool[i].compCollision.rec))
                    { InteractActor(Actor, Pool.entityPool[i]); }
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

        public static void CheckInteractions(GameObject roomObj)
        {
            //loop thru roomObj list, check overlaps, pass to Interact()
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active)
                {
                    if (roomObj.compCollision.rec.Intersects(Pool.roomObjPool[i].compCollision.rec))
                    {   //perform self-check to prevent self overlap interaction
                        if (roomObj != Pool.roomObjPool[i]) { InteractRoomObj(Pool.roomObjPool[i], roomObj); }
                    }
                }
            }
            //loop thru entity list, check overlaps, pass to Interact()
            for (i = 0; i < Pool.entityCount; i++)
            {
                if (Pool.entityPool[i].active)
                {
                    if (roomObj.compCollision.rec.Intersects(Pool.entityPool[i].compCollision.rec))
                    { InteractEntities(Pool.entityPool[i], roomObj); }
                }
            }

            /*
            Debug.WriteLine("" + 
                Pool.entityPool[i].type + " vs " + roomObj.type + 
                " \t ts:" + ScreenManager.gameTime.TotalGameTime.Milliseconds);
            */
        }

        public static void InteractActor(Actor Actor, GameObject Obj)
        {   //Obj can be Entity or RoomObj, check for hero state first

            if (!Obj.active) { return; } //inactive objects are denied interaction

            //Hero Specific Interactions
            if (Actor == Pool.hero)
            {

                #region Pickups

                if (Obj.group == ObjGroup.Pickup)
                {   //only the hero can pickup hearts or rupees
                    if (Obj.type == ObjType.PickupHeart)
                    { Pool.hero.health++; Assets.Play(Assets.sfxHeartPickup); }
                    else if (Obj.type == ObjType.PickupRupee)
                    { PlayerData.current.gold++; Assets.Play(Assets.sfxGoldPickup); }
                    else if (Obj.type == ObjType.PickupMagic)
                    { PlayerData.current.magicCurrent++; Assets.Play(Assets.sfxHeartPickup); }
                    else if (Obj.type == ObjType.PickupArrow)
                    { PlayerData.current.arrowsCurrent++; Assets.Play(Assets.sfxHeartPickup); }
                    else if (Obj.type == ObjType.PickupBomb)
                    { PlayerData.current.bombsCurrent++; Assets.Play(Assets.sfxHeartPickup); }
                    Obj.lifetime = 1; Obj.lifeCounter = 2; //end the items life
                    return;
                }

                #endregion


                #region Doors

                else if (Obj.group == ObjGroup.Door)
                {   //handle hero interaction with exit door
                    if (Obj.type == ObjType.Exit)
                    {
                        if (Functions_Level.levelScreen.displayState == DisplayState.Opened)
                        {   //if dungeon screen is open, close it, perform interaction ONCE
                            DungeonRecord.beatDungeon = false;
                            //is hero exiting a dungeon?
                            if (Level.type == LevelType.Castle)
                            { Functions_Level.CloseLevel(ExitAction.ExitDungeon); }
                            else //return to the overworld screen
                            { Functions_Level.CloseLevel(ExitAction.Overworld); }
                            Assets.Play(Assets.sfxDoorOpen);
                        }
                        //stop movement, prevents overlap with exit
                        Functions_Movement.StopMovement(Pool.hero.compMove);
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


                #region PitTrap

                if (Obj.type == ObjType.PitTrap)
                {   //if hero collides with a PitTrapReady, it starts to open
                    Functions_GameObject.SetType(Obj, ObjType.PitAnimated);
                    Assets.Play(Assets.sfxShatter); //play collapse sound
                    Functions_Entity.SpawnEntity(ObjType.ParticleAttention,
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y,
                        Direction.Down);
                    Functions_Entity.SpawnEntity(ObjType.ParticleSmokePuff,
                        Obj.compSprite.position.X + 4,
                        Obj.compSprite.position.Y - 8,
                        Direction.Down);
                    //create pit teeth over new pit obj
                    Functions_RoomObject.SpawnRoomObj(ObjType.PitTop,
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y,
                        Direction.Down);
                    Functions_RoomObject.SpawnRoomObj(ObjType.PitBottom,
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y,
                        Direction.Down);
                }

                #endregion


                #region SwitchBlock UP

                else if (Obj.type == ObjType.SwitchBlockUp)
                {   //if hero isnt moving and is colliding with up block, convert up to down
                    Functions_GameObject.SetType(Obj, ObjType.SwitchBlockDown);
                }

                #endregion


                #region FloorSwitch

                else if (Obj.type == ObjType.Switch)
                {   //convert switch off, play switch soundFx
                    Functions_GameObject.SetType(Obj, ObjType.SwitchOff);
                    //grab the player's attention
                    Functions_Entity.SpawnEntity(ObjType.ParticleAttention,
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y,
                        Direction.Down);
                    //open all the trap doors in the room
                    Functions_RoomObject.OpenTrapDoors();
                }

                #endregion

            }

            //these objects interact with ALL ACTORS

            #region Projectiles

            if (Obj.group == ObjGroup.Projectile)
            {   //some projectiles dont interact with actors in any way at all
                if (Obj.type == ObjType.ProjectileBomb
                    || Obj.type == ObjType.ProjectileDebrisRock
                    || Obj.type == ObjType.ProjectileExplodingBarrel
                    || Obj.type == ObjType.ProjectileShadowSm
                    )
                { return; }
                //check for collision between net and actor
                else if (Obj.type == ObjType.ProjectileNet)
                {   //make sure actor isn't in hit/dead state
                    if (Actor.state == ActorState.Dead || Actor.state == ActorState.Hit) { return; }
                    Obj.lifeCounter = Obj.lifetime; //kill projectile
                    Obj.compCollision.rec.X = -1000; //hide hitBox (prevents multiple actor collisions)
                    Functions_Actor.BottleActor(Actor); //try to bottle actor
                }
                //if sword projectile is brand new, spawn hit particle
                else if (Obj.type == ObjType.ProjectileSword)
                {
                    if (Obj.lifeCounter == 1)
                    { Functions_Entity.SpawnEntity(ObjType.ParticleHitSparkle, Obj); }
                }
                //all actors take damage from projectiles that reach this path
                Functions_Battle.Damage(Actor, Obj); //sets actor into hit state!
            }

            #endregion


            #region Doors

            else if (Obj.group == ObjGroup.Door)
            {
                if (Obj.type == ObjType.DoorTrap)
                {   //trap doors push ALL actors
                    Functions_Movement.Push(Actor.compMove, Obj.direction, 1.0f);
                    Functions_Entity.SpawnEntity(
                        ObjType.ParticleDashPuff,
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y,
                        Direction.None);
                }
            }

            #endregion


            #region Objects

            else if (Obj.group == ObjGroup.Object)
            {
                if (Obj.type == ObjType.SpikesFloorOn)
                {   //damage push actors (on ground) away from spikes
                    if (Actor.compMove.grounded) { Functions_Battle.Damage(Actor, Obj); }
                }
                else if (Obj.type == ObjType.ConveyorBeltOn)
                {   //belt move actors (on ground)
                    if (Actor.compMove.grounded)
                    {   //halt actor movement based on certain states
                        if (Actor.state == ActorState.Attack
                            || Actor.state == ActorState.Reward
                            || Actor.state == ActorState.Use)
                        { Functions_Movement.StopMovement(Actor.compMove); }
                        else { Functions_RoomObject.ConveyorBeltPush(Actor.compMove, Obj); }
                    }
                }
                else if (Obj.type == ObjType.Bumper)
                {
                    Functions_RoomObject.BounceOffBumper(Actor.compMove, Obj);
                }
                else if (Obj.type == ObjType.PitAnimated)
                {   //actors (on ground) fall into pits
                    if (Actor.compMove.grounded)
                    {

                        #region Prevent Hero's Pet from falling into pit

                        if (Actor == Pool.herosPet)
                        {   //get the opposite direction between pet's center and pit's center
                            Actor.compMove.direction = Functions_Direction.GetOppositeDiagonal(
                                Actor.compSprite.position, Obj.compSprite.position);
                            //push pet in direction
                            Functions_Movement.Push(Actor.compMove, Actor.compMove.direction, 1.0f);
                            return;
                        }

                        #endregion


                        #region Drop any Obj Hero is carrying, hide Hero's shadow

                        if (Actor == Pool.hero)
                        {   //check to see if hero should drop carryingObj
                            if (Functions_Hero.carrying) { Functions_Hero.DropCarryingObj(Actor); }
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
                            Functions_RoomObject.PlayPitFx(Obj);
                            if (Actor == Pool.hero)
                            {   //send hero back to last door he passed thru
                                Assets.Play(Assets.sfxActorLand); //play actor land sfx
                                Functions_Hero.SpawnInCurrentRoom();
                                Functions_Hero.heroShadow.visible = true; 
                                //direct player's attention to hero's respawn pos
                                Functions_Entity.SpawnEntity(
                                    ObjType.ParticleAttention,
                                    Functions_Level.currentRoom.spawnPos.X,
                                    Functions_Level.currentRoom.spawnPos.Y,
                                    Direction.None);
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
                else if (Obj.type == ObjType.IceTile)
                {   //set the actor's friction to ice
                    Actor.compMove.friction = Actor.frictionIce;
                    //clip magnitude's maximum values for ice
                    if (Actor.compMove.magnitude.X > 1) { Actor.compMove.magnitude.X = 1; }
                    else if (Actor.compMove.magnitude.X < -1) { Actor.compMove.magnitude.X = -1; }
                    if (Actor.compMove.magnitude.Y > 1) { Actor.compMove.magnitude.Y = 1; }
                    else if (Actor.compMove.magnitude.Y < -1) { Actor.compMove.magnitude.Y = -1; }
                }
                    
                //bridge doesn't really do anything, it just doesn't cause actor to fall into a pit
            }

            #endregion
                
        }







        public static void InteractEntities(GameObject Entity, GameObject RoomObj)
        {


            #region Blocking RoomObjects

            if (RoomObj.compCollision.blocking)
            {

                #region Arrow

                if (Entity.type == ObjType.ProjectileArrow)
                {   //arrows trigger common obj interactions
                    Functions_RoomObject.HandleCommon(RoomObj, Entity.compMove.direction);
                    //arrows die upon blocking collision
                    Functions_GameObject.Kill(Entity);
                }

                #endregion


                #region Bomb

                else if (Entity.type == ObjType.ProjectileBomb)
                {   //stop bombs from moving thru blocking objects
                    Functions_Movement.StopMovement(Entity.compMove);
                }

                #endregion


                #region Explosion

                else if (Entity.type == ObjType.ProjectileExplosion)
                {   //explosions alter certain roomObjects
                    if (Entity.lifeCounter == 1)
                    {   //check these collisions only once
                        if (RoomObj.type == ObjType.DoorBombable)
                        { Functions_RoomObject.CollapseDungeonDoor(RoomObj, Entity); }
                        else if (RoomObj.type == ObjType.BossStatue)
                        { Functions_RoomObject.DestroyObject(RoomObj, true, true); }
                        //explosions also trigger common obj interactions
                        Functions_RoomObject.HandleCommon(RoomObj,
                            //get the direction towards the roomObj from the explosion
                            Functions_Direction.GetOppositeCardinal(
                                RoomObj.compSprite.position,
                                Entity.compSprite.position)
                            ); //this direction should be explosion pos vs. roomObj pos
                    }
                }

                #endregion


                #region Fireball

                else if (Entity.type == ObjType.ProjectileFireball)
                {   //fireballs alter certain roomObjects
                    if (RoomObj.type == ObjType.DoorBombable)
                    { Functions_RoomObject.CollapseDungeonDoor(RoomObj, Entity); }
                    else if (RoomObj.type == ObjType.BossStatue)
                    { Functions_RoomObject.DestroyObject(RoomObj, true, true); }
                    else if (RoomObj.type == ObjType.TorchUnlit)
                    { Functions_RoomObject.LightTorch(RoomObj); }
                    //fireballs trigger common obj interactions
                    Functions_RoomObject.HandleCommon(RoomObj, Entity.compMove.direction);
                    //fireballs die upon blocking collision
                    Functions_GameObject.Kill(Entity);
                }

                #endregion


                #region SpikeBlock

                else if (Entity.type == ObjType.ProjectileSpikeBlock)
                {
                    Functions_RoomObject.BounceSpikeBlock(Entity);
                    //spikeblocks trigger common obj interactions
                    Functions_RoomObject.HandleCommon(RoomObj, Entity.compMove.direction);
                }

                #endregion


                #region Sword

                else if (Entity.type == ObjType.ProjectileSword)
                {   //sword swipe causes soundfx to blocking objects
                    if (Entity.lifeCounter == 1) //these events happen at start of sword swing
                    {   //if sword projectile is brand new, play collision sfx
                        if (RoomObj.type == ObjType.DoorBombable)
                        { Assets.Play(Assets.sfxTapHollow); } //play hollow
                        else { Assets.Play(Assets.sfxTapMetallic); }
                        //spawn a hit sparkle particle on sword
                        Functions_Entity.SpawnEntity(ObjType.ParticleHitSparkle, Entity);
                    }
                    else if (Entity.lifeCounter == 4)
                    {   //these interactions happen 'mid swing'
                        //swords trigger common obj interactions
                        Functions_RoomObject.HandleCommon(RoomObj, Entity.compMove.direction);
                    }
                }

                #endregion


                #region Rock Debris

                else if (Entity.type == ObjType.ProjectileDebrisRock)
                {   //revert to previous position (treat as a blocking interaction)
                    //Entity.compMove.newPosition.X = Entity.compMove.position.X;
                    //Entity.compMove.newPosition.Y = Entity.compMove.position.Y;
                }

                #endregion


                #region Thrown / Dropped Pot (ProjectilePot & Pot ObjB)

                else if (Entity.type == ObjType.ProjectilePot || Entity.type == ObjType.Pot)
                {   //destroy the Pot object
                    Functions_RoomObject.DestroyObject(Entity, true, true);
                    //thrown / dropped pots trigger common obj interactions
                    Functions_RoomObject.HandleCommon(RoomObj, Entity.compMove.direction);
                }

                #endregion


                #region ExplodingBarrel

                else if (Entity.type == ObjType.ProjectileExplodingBarrel)
                {   //stop barrels from moving thru blocking objects
                    Functions_Movement.StopMovement(Entity.compMove);
                }

                #endregion

            }

            #endregion


            else //these are interactions with non-blocking roomObjs
            {

                #region Conveyor Belt
                
                if (RoomObj.type == ObjType.ConveyorBeltOn)
                {
                    //entities do not interact with conveyor belts
                    //but specific ones could, if needed, right here
                }

                #endregion


                #region Trap Door

                else if (RoomObj.type == ObjType.DoorTrap)
                {   //prevent obj from passing thru door
                    Functions_Movement.RevertPosition(Entity.compMove);
                    //kill specific entities
                    if (Entity.type == ObjType.ProjectileFireball
                        || Entity.type == ObjType.ProjectileArrow)
                    { Functions_GameObject.Kill(Entity); }
                }

                #endregion


                #region Bumper

                else if(RoomObj.type == ObjType.Bumper)
                {   //some projectiles cannot be bounced off bumper
                    if (Entity.type == ObjType.ProjectileDebrisRock
                        || Entity.type == ObjType.ProjectileExplosion
                        || Entity.type == ObjType.ProjectileNet
                        || Entity.type == ObjType.ProjectileShadowSm
                        || Entity.type == ObjType.ProjectileSword
                        )
                    { return; }
                    //the rest of the projectiles are bounced
                    Functions_RoomObject.BounceOffBumper(Entity.compMove, RoomObj);
                    //rotate the bounced projectiles
                    Entity.direction = Entity.compMove.direction;
                    Functions_GameObject.SetRotation(Entity);
                }
                #endregion


                #region Pits

                else if (RoomObj.type == ObjType.PitAnimated)
                {   //check to see if we should ground any thrown pot projectile
                    if (Entity.type == ObjType.ProjectilePot)
                    {   //if this is the last frame of the projectile pot, ground it
                        if (Entity.lifeCounter > Entity.lifetime - 5) //dont let it die
                        { Entity.compMove.grounded = true; Entity.lifeCounter = 3; }
                    }
                    //drag the obj into the pit
                    Functions_RoomObject.DragIntoPit(Entity, RoomObj);
                }

                #endregion


            }
        }

        public static void InteractRoomObj(GameObject PassiveRoomObj, GameObject RoomObj)
        {

            #region ConveyorBelt

            if (RoomObj.type == ObjType.ConveyorBeltOn)
            {   //if obj is moveable and on ground, move it
                if (PassiveRoomObj.compMove.moveable && PassiveRoomObj.compMove.grounded)
                { Functions_RoomObject.ConveyorBeltPush(PassiveRoomObj.compMove, RoomObj); }
            }

            #endregion


            #region Trap Door

            if (RoomObj.type == ObjType.DoorTrap)
            {   //prevent obj from passing thru door
                Functions_Movement.RevertPosition(PassiveRoomObj.compMove);
            }

            #endregion


            #region Bumper

            else if (RoomObj.type == ObjType.Bumper)
            { Functions_RoomObject.BounceOffBumper(PassiveRoomObj.compMove, RoomObj); }

            #endregion


            #region Pits

            else if (RoomObj.type == ObjType.PitAnimated)
            { Functions_RoomObject.DragIntoPit(PassiveRoomObj, RoomObj); }

            #endregion


        }
    }
}