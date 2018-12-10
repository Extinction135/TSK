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
        public static float terminalVelocity = 4.0f;





        //Check Against RoomObj List

        public static void CheckObj_Obj(InteractiveObject Obj)
        {   //this is an obj against the objects list
            for (i = 0; i < Pool.intObjCount; i++)
            {
                if (Pool.intObjPool[i].active)
                {
                    Pool.interactions_Possible++; //possible interaction up next
                    if (Obj.compCollision.rec.Intersects(Pool.intObjPool[i].compCollision.rec))
                    {   //perform self-check to prevent self overlap interaction
                        if (Obj != Pool.intObjPool[i])
                        {
                            Pool.interactions_ThisFrame++; //count it
                            Interact_ObjectObject(Pool.intObjPool[i], Obj);
                        }
                    }
                }
            }
        }


        public static void CheckObj_Actor(Actor Actor)
        {   //this is an actor against the interactive objects list, actor is active
            for (i = 0; i < Pool.intObjCount; i++)
            {
                if(Pool.intObjPool[i].active)
                {
                    Pool.interactions_Possible++; //possible interaction up next
                    if (Actor.compCollision.rec.Intersects(Pool.intObjPool[i].compCollision.rec))
                    {
                        Pool.interactions_ThisFrame++; //count it
                        Interact_ObjectActor(Pool.intObjPool[i], Actor);
                    }
                }
            }
        }







        //Check Against Projectiles List

        public static void CheckProjectile_Obj(Projectile Pro)
        {   //this is a projectile against the interactive objects list
            for (i = 0; i < Pool.intObjCount; i++)
            {
                if (Pool.intObjPool[i].active)
                {
                    Pool.interactions_Possible++; //possible interaction up next
                    if (Pro.compCollision.rec.Intersects(Pool.intObjPool[i].compCollision.rec))
                    {   //interaction between pro and obj
                        Pool.interactions_ThisFrame++; //count it
                        Interact_ProjectileIntObj(Pro, Pool.intObjPool[i]);
                    }
                }
            }
        }


        public static void CheckProjectile_Actor(Actor Actor)
        {   //this is an actor against the projectiles list, actor is active
            for (i = 0; i < Pool.projectileCount; i++)
            {
                if(Pool.projectilePool[i].active)
                {
                    Pool.interactions_Possible++; //possible interaction up next
                    if (Actor.compCollision.rec.Intersects(Pool.projectilePool[i].compCollision.rec))
                    {   //interaction between pro and actor
                        Pool.interactions_ThisFrame++; //count it
                        Interact_ProjectileActor(Pool.projectilePool[i], Actor);
                    }
                }
            }
        }














        //projectile interactions

        public static void Interact_ProjectileActor(Projectile Pro, Actor Actor)
        {

            #region Exit conditions

            //projectiles shouldn't interact with dead actor's corpses
            if (Actor.state == ActorState.Dead) { return; }

            //some projectiles dont interact with actors in any way at all
            else if (Pro.type == ProjectileType.Bomb
                || Pro.type == ProjectileType.GroundFire
                || Pro.type == ProjectileType.Bow
                || Pro.type == ProjectileType.Iceblock
                || Pro.type == ProjectileType.IceblockCracking
                //all emitters
                || Pro.type == ProjectileType.Emitter_Explosion
                || Pro.type == ProjectileType.Emitter_GroundFire
                || Pro.type == ProjectileType.Emitter_IceTile
                )
            { return; }
            //check for boomerang interaction with hero
            else if (Pro.type == ProjectileType.Boomerang & Actor == Pool.hero)
            { return; }
            //check for hero's carried object projectile
            else if (Pro.type == ProjectileType.CarriedObject)
            { return; }
            //ignore thrown projectile obj and hero interactions (allow overlap)
            else if (Pro.type == ProjectileType.ThrownObject & Actor == Pool.hero)
            { return; }

            #endregion



            //Handle special 'targeting' projectiles
            if(Pro.hitObj == null & Pro.hitActor == null)
            {   //projectile hits actor, setting initial hitActor ref
                Pro.hitActor = Actor;
                //set initial hit offset
                Pro.hitOffsetX = Pro.compMove.position.X - Actor.compMove.position.X;
                Pro.hitOffsetY = Pro.compMove.position.Y - Actor.compMove.position.Y;


                #region Arrow

                if (Pro.type == ProjectileType.Arrow)
                {   //improve arrow placement on moving actors (move closer to center)
                    if (Pro.compMove.direction == Direction.Up)
                    {
                        Pro.hitOffsetX = Pro.hitOffsetX / 2;
                        Pro.hitOffsetY = 6;
                    } 
                    else if (Pro.compMove.direction == Direction.Down)
                    {
                        Pro.hitOffsetX = Pro.hitOffsetX / 2;
                        Pro.hitOffsetY = -6;
                    }
                    else if (Pro.compMove.direction == Direction.Left)
                    {
                        Pro.hitOffsetX = 6;
                        Pro.hitOffsetY = Pro.hitOffsetY / 2;
                    }
                    else if (Pro.compMove.direction == Direction.Right)
                    {
                        Pro.hitOffsetX = -6;
                        Pro.hitOffsetY = Pro.hitOffsetY / 2;
                    }
                    Functions_Projectile.SetArrowHitState(Pro);
                    Functions_Battle.Damage(Actor, Pro); //arrows deal damage
                    return;
                }

                #endregion


                #region Iceballs

                else if (Pro.type == ProjectileType.Iceball)
                {   //iceball pro has to become iceblock, via setType()
                    if (Pro.hitObj == null & Pro.hitActor == null) //set initial hitObj
                    {   //stop movement
                        Functions_Movement.StopMovement(Pro.compMove);
                        //stick with hitObj for remainder of life
                        Pro.hitActor = Actor;
                        //become iceblock, with obj ref
                        Functions_Projectile.SetType(Pro, ProjectileType.Iceblock);
                        //hit actor/obj takes push from iceball
                        Functions_Movement.Push(Actor.compMove, Pro.compMove.direction, 4.0f);
                        return;
                    }
                }
                //there are no interaction routines for iceblocks vs. actors
                //because this is handled in projectile's update routine, via Pro.hitActor

                #endregion


            }
            else
            {   //projectile has hit actor, has hitActor ref, do nothing

                #region Arrow

                if (Pro.type == ProjectileType.Arrow)
                {
                    return;
                }

                #endregion


                #region Iceball

                else if (Pro.type == ProjectileType.Iceball)
                {
                    return;
                }

                #endregion

            }

             


            //Handle non-targeting projectile actor hits

            #region Fireball

            if (Pro.type == ProjectileType.Fireball)
            {
                Functions_Projectile.Kill(Pro);
            }

            #endregion


            #region Bat

            if (Pro.type == ProjectileType.Bat)
            {
                Functions_Projectile.Kill(Pro);
            }

            #endregion


            #region Net

            else if (Pro.type == ProjectileType.Net)
            {   //make sure actor isn't in hit/dead state
                if (Actor.state == ActorState.Dead || Actor.state == ActorState.Hit) { return; }
                Pro.lifeCounter = Pro.lifetime; //kill projectile
                Pro.compCollision.rec.X = -1000; //hide hitBox (prevents multiple actor collisions)
                Functions_Bottle.Bottle(Actor); //try to bottle the actor
                Functions_Pool.Release(Pro); //release the net
            }

            #endregion


            #region Hero's ThrownObject

            //kill thrown projectiles upon impact, next frame
            else if (Pro.type == ProjectileType.ThrownObject)
            {
                Pro.lifeCounter = Pro.lifetime;
            }

            #endregion


            #region Bite

            //limit bite to only the first frame of life
            else if (Pro.type == ProjectileType.Bite)
            {   //prevents fast moving caster overlap, while still hitbox drawable
                if (Pro.lifeCounter > 2) { return; }
            }

            #endregion


            #region Explosions (limit to initial blast)

            else if (Pro.type == ProjectileType.Explosion)
            {   //prevents lingering explosions, while still hitbox drawable
                if (Pro.lifeCounter > 3) { return; }
            }

            #endregion


            



            //actors take damage from projectiles that get here..
            Functions_Battle.Damage(Actor, Pro); //sets actor into hit state
        }


        public static void Interact_ProjectileIntObj(Projectile Pro, InteractiveObject IntObj)
        {
            //ignored carried objects for all interactions
            if (Pro.type == ProjectileType.CarriedObject) { return; }
            else if (Pro.type == ProjectileType.Iceblock) { return; }
            else if (Pro.type == ProjectileType.IceblockCracking) { return; }










            //this is a hack to make non-blocking objs block for easy blocking/destruction

            #region Setup Special RoomObjs prior to Interaction

            //seekers dont block, but we want projectiles to kill them
            if (IntObj.type == InteractiveType.Enemy_SeekerExploder)
            { IntObj.compCollision.blocking = true; }
            //so we temporarily turn on their blocking, then check, then turn off

            //mountain walls dont block, but we'll to pretend they do for this check
            if (IntObj.group == InteractiveGroup.Wall_Climbable)
            { IntObj.compCollision.blocking = true; }

            #endregion







            #region Blocking RoomObj vs Projectile

            if (IntObj.compCollision.blocking)
            {

                #region Arrow

                if (Pro.type == ProjectileType.Arrow)
                {   
                    if(Pro.hitObj == null & Pro.hitActor == null) //set initial hitObj
                    {   //arrows push hit obj
                        IntObj.compMove.direction = Pro.compMove.direction;
                        //cut enemies and roomObjs
                        if (IntObj.group == InteractiveGroup.Enemy)
                        { Functions_InteractiveObjs.CutRoomEnemy(IntObj); }
                        else { Functions_InteractiveObjs.Cut(IntObj); }
                        //stop arrows movement
                        Functions_Movement.StopMovement(Pro.compMove);
                        //stick into hitObj for remainder of life
                        Pro.hitObj = IntObj;
                        //set the initial hitoffset - move arrow closer to obj's center
                        Pro.hitOffsetX = Pro.compMove.position.X - IntObj.compMove.position.X;
                        Pro.hitOffsetY = Pro.compMove.position.Y - IntObj.compMove.position.Y;
                        //Debug.WriteLine("XY offset: " + Pro.hitOffsetX + ", " + Pro.hitOffsetY);
                        Functions_Projectile.SetArrowHitState(Pro);

                        /*
                        //set the initial hitoffset - only track horizontally for visual ++
                        Pro.hitOffsetX = Pro.compMove.position.X - RoomObj.compMove.position.X;
                        //make offset smaller - pull arrow towards actors center
                        Pro.hitOffsetX = Pro.hitOffsetX / 2;
                        //Pro.hitOffsetY = Pro.compMove.position.Y - Actor.compMove.position.Y;
                        Pro.hitOffsetY = 0; //no vert offset
                        */
                    }
                    //else ignore interaction after initial hit
                }

                #endregion


                #region Bat

                if (Pro.type == ProjectileType.Bat)
                {   //bats push hit obj
                    IntObj.compMove.direction = Pro.compMove.direction;
                    //cut enemies and roomObjs
                    if (IntObj.group == InteractiveGroup.Enemy)
                    { Functions_InteractiveObjs.CutRoomEnemy(IntObj); }
                    else { Functions_InteractiveObjs.Cut(IntObj); }
                    //bats die upon collision
                    Functions_Projectile.Kill(Pro);
                }

                #endregion


                #region Bomb

                else if (Pro.type == ProjectileType.Bomb)
                {   //stop bombs from moving thru blocking objects
                    Functions_Movement.StopMovement(Pro.compMove);
                }

                #endregion


                #region Fireball

                else if (Pro.type == ProjectileType.Fireball)
                {   //fireball becomes explosion upon collision/death
                    Functions_Projectile.Kill(Pro);
                    //fireballs push hit objs, but death explosion will push the objs max
                    Functions_Movement.Push(IntObj.compMove, Pro.compMove.direction, 4.0f);
                }

                #endregion


                #region Iceball

                else if (Pro.type == ProjectileType.Iceball)
                {   //iceball pro has to become iceblock, via setType()
                    if (Pro.hitObj == null & Pro.hitActor == null) //set initial hitObj
                    {   //stop movement
                        Functions_Movement.StopMovement(Pro.compMove);
                        //stick with hitObj for remainder of life
                        Pro.hitObj = IntObj;
                        //become iceblock, with obj ref
                        Functions_Projectile.SetType(Pro, ProjectileType.Iceblock);
                    }
                }
                //there are no interaction routines for iceblocks vs. objects
                //because this is handled in projectile's update routine, via Pro.hitObj

                #endregion


                #region Sword & Shovel

                else if (Pro.type == ProjectileType.Sword
                    || Pro.type == ProjectileType.Shovel)
                {   //swords and shovels cause soundfx to blocking objects
                    if (Pro.lifeCounter == 2) //these events happen only at start
                    {   //bail if pro is hitting open door, else sparkle + hit sfx
                        if (IntObj.type == InteractiveType.Dungeon_DoorOpen) { return; }
                        //center sparkle to sword/shovel
                        Functions_Particle.Spawn(ParticleType.Sparkle,
                            Pro.compSprite.position.X + 4,
                            Pro.compSprite.position.Y + 4);
                        Assets.Play(IntObj.sfx.hit);
                    }
                    else if (Pro.lifeCounter == 4) //mid-swing
                    {
                        //set hit direction
                        IntObj.compMove.direction = Pro.compMove.direction;
                        //cut enemies and roomObjs
                        if (IntObj.group == InteractiveGroup.Enemy)
                        { Functions_InteractiveObjs.CutRoomEnemy(IntObj); }
                        else { Functions_InteractiveObjs.Cut(IntObj); }
                    }
                }

                #endregion


                #region Fang / Bite / Invs Enemy Attack Projectile

                else if (Pro.type == ProjectileType.Bite)
                {
                    //set hit direction
                    IntObj.compMove.direction = Pro.compMove.direction;
                    //cut enemies and roomObjs
                    if (IntObj.group == InteractiveGroup.Enemy)
                    { Functions_InteractiveObjs.CutRoomEnemy(IntObj); }
                    else { Functions_InteractiveObjs.Cut(IntObj); }
                    //end projectile's life
                    Pro.lifeCounter = Pro.lifetime;
                }

                #endregion


                #region Boomerang

                else if (Pro.type == ProjectileType.Boomerang)
                {
                    //set hit direction
                    IntObj.compMove.direction = Pro.compMove.direction;

                    //cue specific sfx
                    if (IntObj.group == InteractiveGroup.Wall_Dungeon)
                    { Assets.Play(Assets.sfxTapMetallic); }
                    else if (IntObj.group == InteractiveGroup.Door_Dungeon)
                    { Assets.Play(Assets.sfxTapHollow); }
                    else if (IntObj.type == InteractiveType.Dungeon_DoorBombable)
                    { Assets.Play(Assets.sfxTapHollow); }

                    //kill room enemies or bounce off objs
                    if (IntObj.group == InteractiveGroup.Enemy)
                    { Functions_InteractiveObjs.CutRoomEnemy(IntObj); }
                    else { Functions_InteractiveObjs.Bounce(IntObj); }

                    //if this is the initial hit, set the boomerang
                    //into a return state, pop an attention particle
                    if (Pro.lifeCounter < Pro.interactiveFrame)
                    {   //set boomerang into return mode
                        Pro.lifeCounter = 200;
                        Functions_Particle.Spawn(
                            ParticleType.Attention,
                            Pro.compSprite.position.X + 4,
                            Pro.compSprite.position.Y + 4);
                    }

                    //stop all boomerang movement, bounce off object
                    Functions_Movement.StopMovement(Pro.compMove);
                    Functions_Movement.Push(Pro.compMove,
                        Functions_Direction.GetOppositeCardinal(
                            Pro.compSprite.position,
                            IntObj.compSprite.position), 4.0f);
                }

                #endregion
                

                #region Thrown Objects

                else if (Pro.type == ProjectileType.ThrownObject)
                {
                    IntObj.compMove.direction = Pro.compMove.direction;
                    //cut enemies and roomObjs
                    if (IntObj.group == InteractiveGroup.Enemy)
                    { Functions_InteractiveObjs.CutRoomEnemy(IntObj); }
                    else { Functions_InteractiveObjs.Cut(IntObj); }
                    //thrown objs die upon blocking collision
                    Functions_Projectile.Kill(Pro);
                }

                #endregion


                #region Hammer

                else if (Pro.type == ProjectileType.Hammer)
                {   //time this interaction to the hammer hitting the ground
                    if (Pro.compAnim.index == 3)
                    {   //hammers are only pro that alters hammer up posts
                        if (IntObj.type == InteractiveType.Post_HammerPost_Up)
                        {
                            Assets.Play(IntObj.sfx.kill);
                            Functions_InteractiveObjs.SetType(IntObj, InteractiveType.Post_HammerPost_Down);
                        }
                        else
                        {   //inherit hammer's direction for impact
                            IntObj.compMove.direction = Pro.compMove.direction;
                            if (IntObj.group == InteractiveGroup.Enemy)
                            { Functions_InteractiveObjs.CutRoomEnemy(IntObj); }
                            else //destroy roomObj
                            { Functions_InteractiveObjs.Destroy(IntObj); }
                        }
                    }
                }

                #endregion


                #region Wand

                else if (Pro.type == ProjectileType.Wand)
                {   
                    if (Pro.lifeCounter == 2) //these events happen only at start
                    {   //spawn a sparkle on wand tip at start
                        Functions_Particle.Spawn(ParticleType.Sparkle,
                            Pro.compSprite.position.X + 4,
                            Pro.compSprite.position.Y + 4);
                    }
                    else if (Pro.lifeCounter == 4) //mid-swing
                    {   //set hit direction
                        IntObj.compMove.direction = Pro.compMove.direction;
                        //wands can bounce roomObjs
                        Functions_InteractiveObjs.Bounce(IntObj);
                    }
                }

                #endregion






                #region GroundFires

                else if (Pro.type == ProjectileType.GroundFire)
                {   //blocking objs only here, btw

                    //set objs push direction away from fire center
                    IntObj.compMove.direction =
                        Functions_Direction.GetOppositeDiagonal(
                            IntObj.compSprite.position, //to
                            Pro.compSprite.position); //from

                    //ground fires damage and kill roomEnemies
                    if (IntObj.group == InteractiveGroup.Enemy)
                    { Functions_InteractiveObjs.CutRoomEnemy(IntObj); }

                    //ground fires spread to some room objs
                    else if (
                        //burn bushes
                        IntObj.type == InteractiveType.Bush
                        //burn posts
                        || IntObj.type == InteractiveType.PostBurned_CornerLeft
                        || IntObj.type == InteractiveType.PostBurned_CornerRight
                        || IntObj.type == InteractiveType.PostBurned_Horizontal
                        || IntObj.type == InteractiveType.PostBurned_VerticalLeft
                        || IntObj.type == InteractiveType.PostBurned_VerticalRight
                        //burn trees
                        || IntObj.type == InteractiveType.Tree
                        //light torches
                        || IntObj.type == InteractiveType.TorchUnlit
                        )
                    {
                        if (Pro.compCollision.rec.Contains(
                                IntObj.compSprite.position.X,
                                IntObj.compSprite.position.Y))
                        {   //check against sprite center pos
                            Functions_InteractiveObjs.Burn(IntObj);
                        }   //this prevents immediate fire spread across verticals
                    }

                    //groundfires light explosive barrels too
                    else if (IntObj.type == InteractiveType.Barrel)
                    {   //dont push the barrel in any direction
                        IntObj.compMove.direction = Direction.None;
                        Functions_InteractiveObjs.HitBarrel(IntObj);
                    }
                }

                #endregion






                #region Explosions

                else if (Pro.type == ProjectileType.Explosion)
                {
                    if (Pro.lifeCounter == 2) //perform these interactions only once
                    {   //explosions call power level 2 destruction routines

                        //set explosion direction based on pro position
                        IntObj.compMove.direction =
                            Functions_Direction.GetOppositeDiagonal(
                                IntObj.compSprite.position, //to
                                Pro.compSprite.position); //from

                        //blow up enemy
                        if (IntObj.group == InteractiveGroup.Enemy)
                        { Functions_InteractiveObjs.CutRoomEnemy(IntObj); }
                        else //blow up the roomObj
                        { Functions_InteractiveObjs.Explode(IntObj); }
                    }
                }

                #endregion


            }

            #endregion


            #region Non-blocking RoomObj vs Projectile

            else
            {
                //group checks first
                if (IntObj.group == InteractiveGroup.Wall_Climbable)
                {   //kill all projectiles for now
                    Functions_Projectile.Kill(Pro);
                }




                //type check next

                #region Roofs - collapse from explosions, hammers, etc..

                if (
                    IntObj.type == InteractiveType.House_Roof_Bottom
                    || IntObj.type == InteractiveType.House_Roof_Top
                    || IntObj.type == InteractiveType.House_Roof_Chimney
                    )
                {
                    if (
                        Pro.type == ProjectileType.Explosion
                        || Pro.type == ProjectileType.Hammer
                        || Pro.type == ProjectileType.Fireball
                        )
                    {   //begin cascading roof collapse
                        Functions_InteractiveObjs.CollapseRoof(IntObj);
                    }
                }

                #endregion


                #region Trap Doors bounce or kill pros

                else if (IntObj.type == InteractiveType.Dungeon_DoorTrap)
                {   //prevent obj from passing thru door
                    Functions_Movement.RevertPosition(Pro.compMove);

                    //actually, we should be pushing the object the same way we push an actor

                    //kill specific projectiles / objects
                    if (Pro.type == ProjectileType.Fireball
                        || Pro.type == ProjectileType.Arrow)
                    { Functions_Pool.Release(Pro); }
                    else if(Pro.type == ProjectileType.ThrownObject)
                    { Functions_Projectile.Kill(Pro); }
                }

                #endregion


                #region Bumpers bounce some pros

                else if (IntObj.type == InteractiveType.Bumper)
                {   //limit bumper to bouncing only if it's returned to 100% scale
                    if (IntObj.compSprite.scale != 1.0f) { return; }

                    //specific projectiles cannot be bounced off bumper
                    if (Pro.type == ProjectileType.Explosion
                        || Pro.type == ProjectileType.Net
                        || Pro.type == ProjectileType.Sword
                        )
                    { return; }

                    //one of the two objects must be moving,
                    //in order to trigger a bounce interaction
                    if (!IntObj.compMove.moving & !Pro.compMove.moving)
                    { return; }
                    //all other objects are bounced
                    Functions_InteractiveObjs.BounceOffBumper(Pro.compMove, IntObj);

                    //rotate bounced projectiles
                    Pro.direction = Pro.compMove.direction;
                    Functions_Projectile.SetRotation(Pro);
                }

                #endregion


                #region Fairy can be captured by nets, collected by boomerangs, etc..

                else if (IntObj.type == InteractiveType.Fairy)
                {
                    if (Pro.type == ProjectileType.Net)
                    {
                        if (Pro.lifeCounter < Pro.lifetime) //net is still young
                        {
                            //a net has overlapped a fairy (and only a hero can create a net)
                            //attempt to bottle the fairy into one of hero's bottles
                            Functions_Bottle.Bottle(IntObj);
                            //kill net NEXT frame, this allows it to be drawn
                            //while the dialog screen appears over the level screen
                            Pro.lifeCounter = Pro.lifetime;
                            //hide hitBox (prevents multiple collisions)
                            Pro.compCollision.rec.X = -1000;
                            //the beginning if() prevents the net from Interacting()
                            //on the next frame, when it is dying from this interaction
                            //without the check, the net interacts twice, across two frames
                            //causing two dialog screens to pop, which feels buggy
                        }
                    }

                    //collect fairys with boomerang, hookshot, axe, etc...
                    else if(Pro.type == ProjectileType.Boomerang)
                    {   //pickup fairy
                        Functions_InteractiveObjs.UseFairy(IntObj);
                    }
                }

                #endregion


                #region Tall Grass

                else if (IntObj.type == InteractiveType.Grass_Tall)
                {
                    if (Pro.type == ProjectileType.Sword)
                    {   //cutting
                        Functions_InteractiveObjs.Destroy(IntObj);
                    }
                    else if (Pro.type == ProjectileType.GroundFire)
                    {   //burning
                        if (Pro.compCollision.rec.Contains(
                                IntObj.compSprite.position.X,
                                IntObj.compSprite.position.Y))
                        {   //check against sprite center pos
                            Functions_InteractiveObjs.Burn(IntObj);
                        }   //this prevents immediate fire spread across verticals
                    }
                }

                #endregion


                #region Destroy Open House Doors

                else if(IntObj.type == InteractiveType.House_Door_Open)
                {
                    if (
                        Pro.type == ProjectileType.Explosion 
                        || Pro.type == ProjectileType.Hammer
                        || Pro.type == ProjectileType.Fireball
                        )
                    { Functions_InteractiveObjs.Kill(IntObj, false, true); }
                }

                #endregion


                #region Ice Tiles

                else if(IntObj.type == InteractiveType.IceTile)
                {
                    if (
                        Pro.type == ProjectileType.Fireball
                        || Pro.type == ProjectileType.GroundFire
                        )
                    {   //melt icetiles
                        Functions_InteractiveObjs.MeltIceTile(IntObj);
                    }
                }

                #endregion




            }

            #endregion








            //then we just unblock them (return to prev state)

            #region Reset Special Objects post Interaction

            //projectile checks against these objects complete,
            //return them to their non-blocking state
            if (IntObj.type == InteractiveType.Enemy_SeekerExploder)
            { IntObj.compCollision.blocking = false; }

            if (IntObj.group == InteractiveGroup.Wall_Climbable)
            { IntObj.compCollision.blocking = false; }

            #endregion

        }













        //object interactions

        public static void Interact_ObjectActor(InteractiveObject IntObj, Actor Actor)
        {


            #region Hero Specific RoomObj Interactions

            if (Actor == Pool.hero)
            {
                //group checks


                #region Doors

                if (IntObj.group == InteractiveGroup.Door_Dungeon)
                {   //center Hero to Door horiontally or vertically
                    if (IntObj.direction == Direction.Up || IntObj.direction == Direction.Down)
                    {   //gradually center hero to door
                        Pool.hero.compMove.magnitude.X = (IntObj.compSprite.position.X - Pool.hero.compMove.position.X) * 0.11f;
                        //if hero is close to center of door, snap/lock hero to center of door
                        if (Math.Abs(Pool.hero.compSprite.position.X - IntObj.compSprite.position.X) < 2)
                        { Pool.hero.compMove.newPosition.X = IntObj.compSprite.position.X; }
                    }
                    else
                    {   //gradually center hero to door
                        Pool.hero.compMove.magnitude.Y = (IntObj.compSprite.position.Y - Pool.hero.compMove.position.Y) * 0.11f;
                        //if hero is close to center of door, snap/lock hero to center of door
                        if (Math.Abs(Pool.hero.compSprite.position.Y - IntObj.compSprite.position.Y) < 2)
                        { Pool.hero.compMove.newPosition.Y = IntObj.compSprite.position.Y; }
                    }

                }

                #endregion


                //type checks


                #region Map

                if (IntObj.type == InteractiveType.Dungeon_Map)
                {
                    if (LevelSet.currentLevel.map == false) //make sure hero doesn't already have map
                    {
                        Functions_Pool.Release(IntObj); //hero collects map obj
                        IntObj.compSprite.visible = false; //hide map obj
                        LevelSet.currentLevel.map = true; //flip map true
                        Functions_Hero.SetRewardState(ParticleType.RewardMap);
                        Functions_Actor.SetAnimationGroup(Pool.hero);
                        Functions_Actor.SetAnimationDirection(Pool.hero);
                        Functions_Particle.Spawn(ParticleType.RewardMap, Pool.hero);
                        Assets.Play(Assets.sfxReward); //play reward / boss defeat sfx
                        Screens.Dialog.SetDialog(AssetsDialog.HeroGotMap);
                        ScreenManager.AddScreen(Screens.Dialog);
                    }
                    return;
                }

                #endregion


                #region Fairy

                else if (IntObj.type == InteractiveType.Fairy)
                {
                    Functions_InteractiveObjs.UseFairy(IntObj);
                    return;
                }

                #endregion


                #region Roofs

                else if (
                    IntObj.type == InteractiveType.House_Roof_Bottom
                    || IntObj.type == InteractiveType.House_Roof_Chimney
                    //RoomObj.type == ObjType.Wor_Build_Roof_Top
                    //^ no peeking into houses from rear
                    )
                {   //if hero touches a roof, set underRoofs true for this frame
                    Functions_Hero.underRoof = true;
                    return;
                }

                #endregion

            }

            #endregion




            //these objs affect all actors (enemies included)


            #region Doors

            if (IntObj.group == InteractiveGroup.Door_Dungeon)
            {
                if (IntObj.type == InteractiveType.Dungeon_DoorTrap)
                {   //trap doors push ALL actors
                    Functions_Movement.Push(Actor.compMove, IntObj.direction, 1.0f);
                }
            }

            #endregion


            #region Ditches

            else if (IntObj.group == InteractiveGroup.Ditch)
            {   //if the actor is flying, ignore interaction with ditch
                if (Actor.compMove.grounded == false) { return; }
                //if ditch getsAI, then ditch is in it's filled state
                if (IntObj.interacts)
                {   //display animated ripple at actor's feet
                    Functions_Actor.DisplayWetFeet(Actor);
                }
            }

            #endregion


            #region Climbable Walls

            else if (IntObj.group == InteractiveGroup.Wall_Climbable)
            {

                #region Mid Walls - THESE CAUSING FALLING TO HAPPEN

                if (IntObj.type == InteractiveType.MountainWall_Mid)
                {
                    //actor is climbing
                    if (Actor.state == ActorState.Climbing) { return; }

                    //actor is walking/dashing into wall from the south - push down
                    else if (Actor.state == ActorState.Move || Actor.state == ActorState.Dash)
                    {
                        //see if actor is walking north into wall
                        if (Actor.direction == Direction.Up
                            || Actor.direction == Direction.UpLeft
                            || Actor.direction == Direction.UpRight)
                        {
                            //prevent overlap
                            Functions_Movement.Push(Actor.compMove, Direction.Down,
                                Actor.compMove.speed * 1.25f);
                            return; //done with actor
                        }
                    }

                    //all other states resolve to actor falling
                    Functions_Movement.Push(Actor.compMove, Direction.Down, 1.5f);
                    Actor.state = ActorState.Falling;
                    Actor.stateLocked = true;

                    //hack in initial falling sfx, based on falling speed
                    if (Actor.compMove.magnitude.Y < terminalVelocity)
                    { Assets.Play(Assets.sfxActorFall); }
                    //^ this will very quickly reach terminalVelocity
                    //meaning the call to play fall will only happen for a few frames
                    //and the sfx can't be spammed until it's stopped playing

                    //limit how much we can push actor (terminal velocity)
                    else if (Actor.compMove.magnitude.Y > terminalVelocity)
                    { Actor.compMove.magnitude.Y = terminalVelocity; }
                }

                #endregion


                #region Bottom Walls

                else if (IntObj.type == InteractiveType.MountainWall_Bottom)
                {
                    if (Actor.state == ActorState.Climbing) { return; }

                    //only pass falling/landed actor thru wall south
                    else if (Actor.state == ActorState.Falling
                        || Actor.state == ActorState.Landed)
                    {   //lock actor into falling state
                        Actor.state = ActorState.Falling;
                        Actor.stateLocked = true;

                        //limit how much we can push actor (terminal velocity)
                        if (Actor.compMove.magnitude.Y > terminalVelocity) { return; }

                        //fall/push
                        Functions_Movement.Push(Actor.compMove, Direction.Down, 1.5f);
                    }
                    else
                    {
                        //prevent overlap
                        if (Actor.state == ActorState.Dash)
                        {
                            Functions_Movement.Push(Actor.compMove, Direction.Down,
                            Actor.compMove.speed * 1.5f); //push more
                        }
                        else
                        {
                            Functions_Movement.Push(Actor.compMove, Direction.Down,
                            Actor.compMove.speed * 1.25f); //push standard
                        }
                    }
                }

                #endregion


                #region Trap Ladders

                else if (IntObj.type == InteractiveType.MountainWall_Ladder_Trap)
                {
                    //creates attention particle and removes itself
                    Functions_Particle.Spawn(ParticleType.Attention,
                        IntObj.compSprite.position.X, IntObj.compSprite.position.Y, Direction.Down);
                    Functions_Pool.Release(IntObj);
                    Assets.Play(Assets.sfxShatter);
                }

                #endregion

            }

            #endregion



            //Objects
            else if (IntObj.group == InteractiveGroup.Object)
            {
                //Phase 1 - objects that interact with flying and grounded actors

                #region SpikeBlocks

                if (IntObj.type == InteractiveType.Dungeon_BlockSpike)
                {   //damage push actors (on ground or in air) away from spikes
                    Functions_Battle.Damage(Actor, IntObj);
                }

                #endregion


                #region Bumpers

                else if (IntObj.type == InteractiveType.Bumper)
                {   //limit bumper to bouncing only if it's returned to 100% scale
                    if (IntObj.compSprite.scale == 1.0f)
                    { Functions_InteractiveObjs.BounceOffBumper(Actor.compMove, IntObj); }
                }

                #endregion


                #region SwitchBlock UP

                else if (IntObj.type == InteractiveType.Dungeon_SwitchBlockUp)
                {   //if actor is colliding with up block, convert up to down
                    Functions_InteractiveObjs.SetType(IntObj, InteractiveType.Dungeon_SwitchBlockDown);
                }   //this is done because block just popped up and would block actor

                #endregion


                //Phase 2 - all flying actors bail from method - they're done
                if (Actor.compMove.grounded == false) { return; }



                //Phase 3 - objects that interact with grounded actors only (all remaining)

                #region FloorSpikes

                if (IntObj.type == InteractiveType.Dungeon_SpikesFloorOn)
                {   //damage push actors (on ground) away from spikes
                    Functions_Battle.Damage(Actor, IntObj);
                }

                #endregion


                #region ConveyorBelts

                else if (IntObj.type == InteractiveType.ConveyorBeltOn)
                {
                    //halt actor movement based on certain states
                    if (Actor.state == ActorState.Reward)
                    { Functions_Movement.StopMovement(Actor.compMove); }
                    else
                    { Functions_InteractiveObjs.ConveyorBeltPush(Actor.compMove, IntObj); }

                }

                #endregion


                #region Pits

                else if (IntObj.type == InteractiveType.Lava_Pit)
                {   //hero throw()s any held object
                    if (Actor == Pool.hero & Functions_Hero.carrying) 
                    { Functions_Hero.Throw(); }


                    //set ALL ACTORS state
                    //dead actors simply stay dead as they fall into a pit
                    //else actors are set into a hit state as they fall
                    if (Actor.state != ActorState.Dead) { Actor.state = ActorState.Hit; }
                    Actor.stateLocked = true;
                    Actor.lockCounter = 0;
                    Actor.lockTotal = 45;
                    Actor.compMove.speed = 0.0f;


                    #region Continuous collision (each frame) - ALL ACTORS

                    //gradually pull actor into pit's center, manually update the actor's position
                    Actor.compMove.magnitude = (IntObj.compSprite.position - Actor.compSprite.position) * 0.25f;

                    //if actor is near to pit center, begin/continue falling state
                    if (Math.Abs(Actor.compSprite.position.X - IntObj.compSprite.position.X) < 2)
                    {
                        if (Math.Abs(Actor.compSprite.position.Y - IntObj.compSprite.position.Y) < 2)
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
                        Functions_InteractiveObjs.PlayPitFx(IntObj);
                        if (Actor == Pool.hero)
                        {   //send hero back to last door he passed thru
                            Assets.Play(Assets.sfxActorLand); //play actor land sfx
                            Functions_Hero.SpawnInCurrentRoom();
                        }
                        else
                        {   //handle enemy pit death (no loot, insta-death)
                            Assets.Play(Actor.sfx.kill); //play actor death sfx
                            Functions_Pool.Release(Actor); //release this actor back to pool
                        }
                    }

                    #endregion

                }

                #endregion


                #region Ice Tiles

                else if (IntObj.type == InteractiveType.IceTile)
                {
                    Functions_InteractiveObjs.SlideOnIce(Actor.compMove);
                }

                #endregion


                #region Tall grass

                else if (IntObj.type == InteractiveType.Grass_Tall)
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

                else if (IntObj.type == InteractiveType.Water_2x2)
                {
                    //if actor is flying, don't sink or swim in water
                    if (Actor.compMove.grounded == false) { return; }

                    if (Actor.createSplash == false)
                    {   //actor is transitioning into water this frame
                        Functions_Particle.Spawn(ParticleType.Splash,
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


                #region Water Vines

                else if (IntObj.type == InteractiveType.Water_Vine)
                {

                    if (Actor.underwater)
                    {
                        //ignore interactions with vine
                        return;
                    }
                    else
                    {
                        //just prevent overlap (push opposite direction)
                        Functions_Movement.Push(
                            Actor.compMove,
                            Functions_Direction.GetOppositeCardinal(
                                Actor.compCollision.rec.Center,
                                IntObj.compCollision.rec.Center),
                                1.0f);
                        return;
                    }
                }

                #endregion


                #region Coastlines

                else if (
                    IntObj.type == InteractiveType.Coastline_Corner_Exterior
                    || IntObj.type == InteractiveType.Coastline_Corner_Interior
                    || IntObj.type == InteractiveType.Coastline_Straight
                    || IntObj.type == InteractiveType.Coastline_1x2_Animated)
                {   //display animated ripple at actor's feet
                    Functions_Actor.DisplayWetFeet(Actor);
                }

                #endregion


                #region PitTrap

                else if (IntObj.type == InteractiveType.Lava_PitTrap)
                {   //if hero collides with a PitTrapReady, it starts to open
                    Functions_InteractiveObjs.SetType(IntObj, InteractiveType.Lava_Pit);
                    Assets.Play(Assets.sfxShatter); //play collapse sound
                    Functions_Particle.Spawn(ParticleType.Attention,
                        IntObj.compSprite.position.X,
                        IntObj.compSprite.position.Y);
                    Functions_Particle.Spawn(ParticleType.ImpactDust,
                        IntObj.compSprite.position.X + 4,
                        IntObj.compSprite.position.Y - 8);
                    //Functions_Particle.ScatterDebris(Obj.compSprite.position);
                    //create pit teeth over new pit obj
                    Functions_InteractiveObjs.Spawn(InteractiveType.Lava_PitTeethTop,
                        (int)IntObj.compSprite.position.X,
                        (int)IntObj.compSprite.position.Y,
                        Direction.Down);
                    Functions_InteractiveObjs.Spawn(InteractiveType.Lava_PitTeethBottom,
                        (int)IntObj.compSprite.position.X,
                        (int)IntObj.compSprite.position.Y,
                        Direction.Down);
                    //note: this section of code must always come
                    //before dungeon_pit interaction code, otherwise
                    //the pit's interaction happens this frame
                }

                #endregion


                //bridge just doesn't cause actor to fall into pit
                //lillypad just doesn't cause actor to be put into swimming state
            }
        }


        public static void Interact_ObjectObject(InteractiveObject RoomObj, InteractiveObject Object)
        {
            //Debug.WriteLine("RoomObj: " + RoomObj.type + " + Object: " + Object.type);






            //special case OBJECTS (pets and conveyor belts)

            #region Pet / Animals

            if (Object.type == InteractiveType.Pet_Dog)
            {
                //dogs can swim
                if (RoomObj.type == InteractiveType.Water_2x2) { Object.inWater = true; }

                //dogs bounce
                else if (RoomObj.type == InteractiveType.Bumper)
                { Functions_InteractiveObjs.BounceOffBumper(Object.compMove, RoomObj); }
            }

            #endregion


            #region ConveyorBelts

            else if (Object.type == InteractiveType.ConveyorBeltOn)
            {   //if obj is moveable and on ground, move it
                if (RoomObj.compMove.moveable && RoomObj.compMove.grounded)
                { Functions_InteractiveObjs.ConveyorBeltPush(RoomObj.compMove, Object); }
            }

            #endregion







            /*
            //objs that remove other objs (vips)
            //THESE OBJS ARE INDESTRUCTIBLE
            #region Dungeon Exits

            else if (
                Object.type == ObjType.Dungeon_Exit
                || Object.type == ObjType.Dungeon_ExitPillarLeft
                || Object.type == ObjType.Dungeon_ExitPillarRight
                )
            {
                if(
                    RoomObj.group == ObjGroup.Door
                    || RoomObj.group == ObjGroup.Wall
                   )
                {   //doors & walls cannot overlap exits
                    Functions_Pool.Release(RoomObj);
                }
                else { } //do nothing
            }

            #endregion

            */

            








            //Blocking ROOMOBJECT.Type Checks

            if (RoomObj.compCollision.blocking)
            {

                #region ExplodingObject

                if (Object.type == InteractiveType.ExplodingObject)
                {   //stop exploding object from moving thru blocking objects
                    Functions_Movement.StopMovement(Object.compMove);
                }

                #endregion


                #region SeekerExploders

                else if (Object.type == InteractiveType.Enemy_SeekerExploder)
                {
                    Object.counter = Object.countTotal; //explode this frame
                    Functions_InteractiveObjs.Bounce(Object);
                }

                #endregion


                #region SpikeBlock

                else if (Object.type == InteractiveType.Dungeon_BlockSpike)
                {   //bounce spike blocks off blocking roomObjs
                    Functions_InteractiveObjs.BounceSpikeBlock(Object); //rev direction
                    //spikeblocks trigger common obj interactions
                    RoomObj.compMove.direction = Object.compMove.direction;
                    Functions_InteractiveObjs.Destroy(RoomObj); //destroys most things
                }

                #endregion


                #region IceTiles - slides objs

                else if (Object.type == InteractiveType.IceTile)
                {
                    if (RoomObj.compMove.grounded)
                    {   //only objects on the ground can slide on ice
                        Functions_InteractiveObjs.SlideOnIce(RoomObj.compMove);
                    }
                }

                #endregion

            }
            //Non-blocking ROOMOBJECT.Type Checks
            else
            {
                
                #region Water (objects that 'fall' into water - draggable objs)

                if (RoomObj.type == InteractiveType.Water_2x2)
                {
                    //object groups not removed by water
                    if (
                        //Object.group == InteractiveGroup.exit
                        Object.group == InteractiveGroup.Wall_Dungeon 
                        || Object.group == InteractiveGroup.Door_Dungeon
                        || Object.group == InteractiveGroup.Vendor 
                        || Object.group == InteractiveGroup.NPC
                        || Object.group == InteractiveGroup.EnemySpawn 
                        || Object.group == InteractiveGroup.Ditch
                        || Object.group == InteractiveGroup.Wall_Climbable
                        )
                    { return; }

                    //if object is airborne, it shouldn't sink
                    if (Object.compMove.grounded == false) { return; }

                    //if an obj is moveable, then hero can push it, then it should sink in water
                    if (Object.compMove.moveable)
                    {
                        //if object's hitBox is disabled, then obj shouldn't sink, ignore
                        if (Object.compCollision.blocking == false) { return; }

                        //if object's sprite center touches the water tile, sink it
                        if (RoomObj.compCollision.rec.Contains(Object.compSprite.position))
                        {
                            //release the obj, create a splash particle centered to object
                            Functions_Particle.Spawn(ParticleType.Splash, Object);
                            Functions_Pool.Release(Object);
                        }
                        //otherwise the obj sinks as soon as it touches a water tile,
                        //which looks early. and bad. cause we tried it. no good.
                    }

                    //bushes and stumps are grounded, not pushable, and dont eval() here
                }

                #endregion


                #region Climbable Walls

                else if (RoomObj.group == InteractiveGroup.Wall_Climbable)
                {
                    //prevent certain wall objs from pulling any objects down the wall
                    if (RoomObj.type == InteractiveType.MountainWall_Foothold
                        || RoomObj.type == InteractiveType.MountainWall_Ladder
                        || RoomObj.type == InteractiveType.MountainWall_Ladder_Trap)
                    { return; }

                    //if hero is climbing/landed with the object as pet, ignore push

                    //wall moves obj fast, if obj is moving slow then this is initial fall
                    if (Object.compMove.magnitude.Y < 2.0f & Object.compMove.magnitude.Y > 0.5f)
                    {   //play soundfx and directional cue on initial fall
                        Assets.Play(Assets.sfxActorFall);
                        Functions_Particle.Spawn(
                            ParticleType.Push,
                            Object.compSprite.position.X + 4,
                            Object.compSprite.position.Y - 8,
                            Direction.Down);
                    }

                    //limit how much we can push objs (terminal velocity)
                    if (Object.compMove.magnitude.Y < terminalVelocity)
                    {   //fall/push
                        Functions_Movement.Push(Object.compMove, Direction.Down, 1.5f);

                        //we could grab a shadow from the shadow pool and place it here to 
                        //fake depth and distance away from the wall / ground
                    }
                }

                #endregion


                #region Trap Door

                else if (RoomObj.type == InteractiveType.Dungeon_DoorTrap)
                {   //prevent obj from passing thru door
                    Functions_Movement.RevertPosition(Object.compMove);

                    //actually, we should be pushing the object the same way we push an actor
                }

                #endregion


                #region Bumper

                else if (RoomObj.type == InteractiveType.Bumper)
                {   //limit bumper to bouncing only if it's returned to 100% scale
                    if (RoomObj.compSprite.scale != 1.0f) { return; }

                    //one of the two objects must be moving,
                    //in order to trigger a bounce interaction
                    if (!RoomObj.compMove.moving & !Object.compMove.moving)
                    { return; }
                    //all other objects are bounced
                    Functions_InteractiveObjs.BounceOffBumper(Object.compMove, RoomObj);
                }

                #endregion


                //floor objects

                #region FloorSpikes

                if (RoomObj.type == InteractiveType.Dungeon_SpikesFloorOn)
                {
                    if (Object.compMove.grounded)
                    {
                        Object.compMove.direction =
                            Functions_Direction.GetOppositeCardinal(
                                    Object.compMove.position,
                                    RoomObj.compMove.position);
                        Functions_InteractiveObjs.Destroy(Object);
                    }
                }

                #endregion


                #region Pits

                else if (RoomObj.type == InteractiveType.Lava_Pit)
                {   //drag any object into the pit
                    Functions_InteractiveObjs.DragIntoPit(Object, RoomObj);
                }

                #endregion


            }
        }



    }
}