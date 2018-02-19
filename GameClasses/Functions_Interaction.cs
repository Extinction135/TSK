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

        public static void CheckInteractions(GameObject gameObj, Boolean checkEntities, Boolean checkRoomObjs)
        {
            if (checkEntities)
            {   //loop thru entity list, check overlaps, pass to Interact()
                for (i = 0; i < Pool.entityCount; i++)
                {
                    if(gameObj.compCollision.rec.Intersects(Pool.entityPool[i].compCollision.rec))
                    {   //perform self-check to prevent self overlap interaction
                        if (gameObj != Pool.entityPool[i]) { InteractObject(gameObj, Pool.entityPool[i]); }
                    }
                }
            }
            if(checkRoomObjs)
            {   //loop thru entity list, check overlaps, pass to Interact()
                for (i = 0; i < Pool.roomObjCount; i++)
                {
                    if (gameObj.compCollision.rec.Intersects(Pool.roomObjPool[i].compCollision.rec))
                    {   //perform self-check to prevent self overlap interaction
                        if (gameObj != Pool.roomObjPool[i]) { InteractObject(gameObj, Pool.roomObjPool[i]); }
                    }
                }
            }
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








        public static void InteractObject(GameObject ObjA, GameObject ObjB)
        {
            //Debug.WriteLine("" + ObjA.type + " vs " + ObjB.type + " \t ts:" + ScreenManager.gameTime.TotalGameTime.Milliseconds);
            if (!ObjA.active || !ObjB.active) { return; }





            //this is very confusing, because the method can be called with 
            //projectiles OR roomObjs as ObjA OR ObjB
            //this needs to be simplified to be clearer

            //split this method into InteractEntities() & InteractRoomObj()
            //then in the above method CheckInteractions(), call the proper method



            //Handle Blocking Interactions (ObjA vs ObjB)
            //it's done this way because projectile vs. blocking obj is simple to evaluate
            if (ObjB.compCollision.blocking)
            {

                #region Arrow

                if (ObjA.type == ObjType.ProjectileArrow)
                {   //arrows trigger common obj interactions
                    Functions_RoomObject.HandleCommon(ObjB, ObjA.compMove.direction);
                    //arrows die upon blocking collision
                    Functions_GameObject.Kill(ObjA);
                }

                #endregion


                #region Bomb

                else if (ObjA.type == ObjType.ProjectileBomb)
                {   //stop bombs from moving thru blocking objects
                    Functions_Movement.StopMovement(ObjA.compMove);
                }

                #endregion


                #region Explosion

                else if (ObjA.type == ObjType.ProjectileExplosion)
                {   //explosions alter certain roomObjects
                    if (ObjA.lifeCounter == 1)
                    {   //check these collisions only once
                        if (ObjB.type == ObjType.DoorBombable)
                        { Functions_RoomObject.CollapseDungeonDoor(ObjB, ObjA); }
                        else if (ObjB.type == ObjType.BossStatue)
                        { Functions_RoomObject.DestroyObject(ObjB, true, true); }
                        //explosions also trigger common obj interactions
                        Functions_RoomObject.HandleCommon(ObjB,
                            //get the direction towards the roomObj from the explosion
                            Functions_Direction.GetOppositeCardinal(
                                ObjB.compSprite.position,
                                ObjA.compSprite.position)
                            ); //this direction should be explosion pos vs. roomObj pos
                    }
                }

                #endregion


                #region Fireball

                else if (ObjA.type == ObjType.ProjectileFireball)
                {   //fireballs alter certain roomObjects
                    if (ObjB.type == ObjType.DoorBombable)
                    { Functions_RoomObject.CollapseDungeonDoor(ObjB, ObjA); }
                    else if (ObjB.type == ObjType.BossStatue)
                    { Functions_RoomObject.DestroyObject(ObjB, true, true); }
                    else if (ObjB.type == ObjType.TorchUnlit)
                    { Functions_RoomObject.LightTorch(ObjB); }
                    //fireballs trigger common obj interactions
                    Functions_RoomObject.HandleCommon(ObjB, ObjA.compMove.direction);
                    //fireballs die upon blocking collision
                    Functions_GameObject.Kill(ObjA);
                }

                #endregion


                #region SpikeBlock

                else if (ObjA.type == ObjType.ProjectileSpikeBlock)
                {
                    Functions_RoomObject.BounceSpikeBlock(ObjA);
                    //spikeblocks trigger common obj interactions
                    Functions_RoomObject.HandleCommon(ObjB, ObjA.compMove.direction);
                }

                #endregion


                #region Sword

                else if (ObjA.type == ObjType.ProjectileSword)
                {   //sword swipe causes soundfx to blocking objects
                    if (ObjA.lifeCounter == 1) //these events happen at start of sword swing
                    {   //if sword projectile is brand new, play collision sfx
                        if (ObjB.type == ObjType.DoorBombable)
                        { Assets.Play(Assets.sfxTapHollow); } //play hollow
                        else { Assets.Play(Assets.sfxTapMetallic); }
                        //spawn a hit sparkle particle on sword
                        Functions_Entity.SpawnEntity(ObjType.ParticleHitSparkle, ObjA);
                    }
                    else if(ObjA.lifeCounter == 4)
                    {   //these interactions happen 'mid swing'
                        //swords trigger common obj interactions
                        Functions_RoomObject.HandleCommon(ObjB, ObjA.compMove.direction);
                    }
                }

                #endregion


                #region Rock Debris

                else if (ObjA.type == ObjType.ProjectileDebrisRock)
                {   //revert to previous position (treat as a blocking interaction)
                    ObjA.compMove.newPosition.X = ObjA.compMove.position.X;
                    ObjA.compMove.newPosition.Y = ObjA.compMove.position.Y;
                }

                #endregion


                #region Thrown / Dropped Pot (ProjectilePot & Pot ObjB)

                else if (ObjA.type == ObjType.ProjectilePot || ObjA.type == ObjType.Pot)
                {   //destroy the Pot object
                    Functions_RoomObject.DestroyObject(ObjA, true, true);
                    //thrown / dropped pots trigger common obj interactions
                    Functions_RoomObject.HandleCommon(ObjB, ObjA.compMove.direction);
                }

                #endregion


                #region ExplodingBarrel

                else if (ObjA.type == ObjType.ProjectileExplodingBarrel)
                {   //stop barrels from moving thru blocking objects
                    Functions_Movement.StopMovement(ObjA.compMove);
                }

                #endregion

            }




            

            #region ConveyorBelt

            if (ObjA.type == ObjType.ConveyorBeltOn)
            {   //if obj is moveable and on ground, move it
                if (ObjB.compMove.moveable && ObjB.compMove.grounded)
                { Functions_RoomObject.ConveyorBeltPush(ObjB.compMove, ObjA); }
            }

            #endregion


            #region Trap Door

            else if (ObjA.type == ObjType.DoorTrap)
            {   //objects should not move thru this door - they should bounce or be destroyed
                //revert to previous position (treat as a blocking interaction)
                ObjB.compMove.newPosition.X = ObjB.compMove.position.X;
                ObjB.compMove.newPosition.Y = ObjB.compMove.position.Y;

                if (ObjB.type == ObjType.ProjectileSpikeBlock)
                { Functions_RoomObject.BounceSpikeBlock(ObjB); }
                //kill all other projectiles
                else if (ObjB.type == ObjType.ProjectileFireball
                    || ObjB.type == ObjType.ProjectileArrow
                    || ObjB.type == ObjType.ProjectileBomb)
                { Functions_GameObject.Kill(ObjB); }
            }

            #endregion


            #region Bumper

            else if (ObjB.type == ObjType.Bumper)
            {   //some projectiles cannot be bounced off bumper
                if (ObjA.type == ObjType.ProjectileDebrisRock
                    || ObjA.type == ObjType.ProjectileExplosion
                    || ObjA.type == ObjType.ProjectileNet
                    || ObjA.type == ObjType.ProjectileShadowSm
                    || ObjA.type == ObjType.ProjectileSword
                    )
                { return; }
                Functions_RoomObject.BounceOffBumper(ObjA.compMove, ObjB);
                if (ObjA.group == ObjGroup.Projectile)
                {   //rotate bounced projectiles
                    ObjA.direction = ObjA.compMove.direction;
                    Functions_GameObject.SetRotation(ObjA);
                }
            }

            #endregion


            #region Pits

            else if (ObjB.type == ObjType.PitAnimated)
            {
                //check to see if we should ground the thrown pot projectile
                if (ObjA.type == ObjType.ProjectilePot)
                {   //if this is the last frame of the projectile pot, ground it
                    if (ObjA.lifeCounter > ObjA.lifetime - 5) //dont let it die
                    { ObjA.compMove.grounded = true; ObjA.lifeCounter = 3; }
                }

                //check to see if this pit can pull in a grounded object
                if (ObjA.compMove.grounded)
                {  //slightly pull projectile towards pit's center
                    ObjA.compMove.magnitude = (ObjB.compSprite.position - ObjA.compSprite.position) * 0.25f;

                    //check to see if this object started falling, or has been falling
                    if (ObjA.compSprite.scale == 1.0f) //begin falling state
                    {   //dont play falling sound if entity is thrown pot (falling sound was just played)
                        if (ObjA.type != ObjType.ProjectilePot) { Assets.Play(Assets.sfxActorFall); }
                    }

                    //scale object down if it's colliding with a pit
                    ObjA.compSprite.scale -= 0.03f;

                    //when a projectile drops below a threshold scale, release it
                    if (ObjA.compSprite.scale < 0.8f)
                    {
                        Functions_Pool.Release(ObjA);
                        Functions_RoomObject.PlayPitFx(ObjB);
                    }
                }
            }

            #endregion

            










            /*
            //can check interactions like dis
            if (ObjB.type == ObjType.ConveyorBeltOn)
            {   //timestamp any entity collision with the roomObj
                Debug.WriteLine("" + 
                    ObjB.type + " vs " + ObjA.type + 
                    " \t ts:" + ScreenManager.gameTime.TotalGameTime.Milliseconds);
            }
            */
        }

    }
}