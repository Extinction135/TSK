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

        public static void CheckObj_Obj(GameObject Obj)
        {   //this is an obj against the objects list
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active)
                {
                    if (Obj.compCollision.rec.Intersects(Pool.roomObjPool[i].compCollision.rec))
                    {   //perform self-check to prevent self overlap interaction
                        if (Obj != Pool.roomObjPool[i])
                        {
                            Pool.interactionsCount++;
                            Interact_ObjectObject(Pool.roomObjPool[i], Obj);
                        }
                    }
                }
            }
        }

        public static void CheckObj_Actor(Actor Actor)
        {   //this is an actor against the object list, actor is active
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if(Pool.roomObjPool[i].active)
                {
                    if (Actor.compCollision.rec.Intersects(Pool.roomObjPool[i].compCollision.rec))
                    {
                        Pool.interactionsCount++;
                        Interact_ObjectActor(Pool.roomObjPool[i], Actor);
                    }
                }
            }
        }

        //Check Against Projectiles List

        public static void CheckProjectile_Obj(Projectile Pro)
        {   //this is a projectile against the objects list
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active)
                {
                    if (Pro.compCollision.rec.Intersects(Pool.roomObjPool[i].compCollision.rec))
                    {   //interaction between pro and obj
                        Pool.interactionsCount++;
                        Interact_ProjectileRoomObj(Pro, Pool.roomObjPool[i]);
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
                    if (Actor.compCollision.rec.Intersects(Pool.projectilePool[i].compCollision.rec))
                    {   //interaction between pro and actor
                        Pool.interactionsCount++;
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



            //Handle Initial Hit State - pro can't have hitObj/actor
            if(Pro.hitObj == null & Pro.hitActor == null)
            {   //projectile hits actor, setting hitActor reference
                Pro.hitActor = Actor;

                //set initial hit offset
                Pro.hitOffsetX = Pro.compMove.position.X - Actor.compMove.position.X;
                Pro.hitOffsetY = Pro.compMove.position.Y - Actor.compMove.position.Y;

                

                //Handle projectile hit behaviors


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
                }

                #endregion


                #region Fireball

                if (Pro.type == ProjectileType.Fireball)
                {
                    Functions_Projectile.Kill(Pro);
                }

                #endregion


                #region Iceball

                if (Pro.type == ProjectileType.Iceball)
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


                //actors take damage from projectiles that get here..
                Functions_Battle.Damage(Actor, Pro); //sets actor into hit state
            }
        }

        public static void Interact_ProjectileRoomObj(Projectile Pro, GameObject RoomObj)
        {
            //ignored carried objects for all interactions
            if (Pro.type == ProjectileType.CarriedObject) { return; }


            
            //this is a hack to make non-blocking objs block for easy blocking/destruction

            #region Setup Special RoomObjs prior to Interaction

            //seekers dont block, but we want projectiles to kill them
            if (RoomObj.type == ObjType.Wor_SeekerExploder)
            { RoomObj.compCollision.blocking = true; }
            //so we temporarily turn on their blocking, then check, then turn off

            //mountain walls dont block, but we'll to pretend they do for this check
            if (RoomObj.group == ObjGroup.Wall_Climbable)
            { RoomObj.compCollision.blocking = true; }

            #endregion







            #region Blocking RoomObj vs Projectile

            if (RoomObj.compCollision.blocking)
            {

                #region Arrow

                if (Pro.type == ProjectileType.Arrow)
                {   
                    if(Pro.hitObj == null & Pro.hitActor == null) //set initial hitObj
                    {   //arrows push hit obj
                        RoomObj.compMove.direction = Pro.compMove.direction;
                        //cut enemies and roomObjs
                        if (RoomObj.group == ObjGroup.Enemy)
                        { Functions_GameObject_World.CutRoomEnemy(RoomObj); }
                        else { Functions_GameObject_World.Cut(RoomObj); }
                        //stop arrows movement
                        Functions_Movement.StopMovement(Pro.compMove);
                        //stick into hitObj for remainder of life
                        Pro.hitObj = RoomObj;
                        //set the initial hitoffset - move arrow closer to obj's center
                        Pro.hitOffsetX = Pro.compMove.position.X - RoomObj.compMove.position.X;
                        Pro.hitOffsetY = Pro.compMove.position.Y - RoomObj.compMove.position.Y;
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
                    RoomObj.compMove.direction = Pro.compMove.direction;
                    //cut enemies and roomObjs
                    if (RoomObj.group == ObjGroup.Enemy)
                    { Functions_GameObject_World.CutRoomEnemy(RoomObj); }
                    else { Functions_GameObject_World.Cut(RoomObj); }
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


                #region Explosions

                else if (Pro.type == ProjectileType.Explosion)
                {
                    if (Pro.lifeCounter == 2) //perform these interactions only once
                    {   //explosions call power level 2 destruction routines
                        RoomObj.compMove.direction = Pro.compMove.direction;

                        if (RoomObj.group == ObjGroup.Enemy)
                        { Functions_GameObject_World.CutRoomEnemy(RoomObj); }
                        else //blow up the roomObj
                        { Functions_GameObject_World.Explode(RoomObj); }
                    }
                }

                #endregion


                #region Fireball

                else if (Pro.type == ProjectileType.Fireball)
                {   //fireball becomes explosion upon collision/death
                    Functions_Projectile.Kill(Pro);
                }

                #endregion


                #region Iceball

                else if (Pro.type == ProjectileType.Iceball)
                {   
                    //we're using arrow here to model iceball tracking
                    if (Pro.hitObj == null & Pro.hitActor == null) //set initial hitObj
                    {   //arrows push hit obj
                        RoomObj.compMove.direction = Pro.compMove.direction;
                        //cut enemies and roomObjs
                        if (RoomObj.group == ObjGroup.Enemy)
                        { Functions_GameObject_World.CutRoomEnemy(RoomObj); }
                        else { Functions_GameObject_World.Cut(RoomObj); }
                        //stop arrows movement
                        Functions_Movement.StopMovement(Pro.compMove);
                        //stick into hitObj for remainder of life
                        Pro.hitObj = RoomObj;
                        //set initial hitoffset
                        Pro.hitOffsetX = Pro.compMove.position.X - RoomObj.compMove.position.X;
                        Pro.hitOffsetY = Pro.compMove.position.Y - RoomObj.compMove.position.Y;
                        //Debug.WriteLine("XY offset: " + Pro.hitOffsetX + ", " + Pro.hitOffsetY);
                        //Functions_Projectile.SetArrowHitState(Pro);

                        //lets watch the iceball track around
                        Pro.lifetime = 250;
                        Pro.lifeCounter = 0;
                    }
                }

                #endregion


                #region Sword & Shovel & Bite

                else if (Pro.type == ProjectileType.Sword
                    || Pro.type == ProjectileType.Shovel)
                {   //swords and shovels cause soundfx to blocking objects
                    if (Pro.lifeCounter == 2) //these events happen only at start
                    {   //bail if pro is hitting open door, else sparkle + hit sfx
                        if (RoomObj.type == ObjType.Dungeon_DoorOpen) { return; }
                        //center sparkle to sword/shovel
                        Functions_Particle.Spawn(ParticleType.Sparkle,
                            Pro.compSprite.position.X + 4,
                            Pro.compSprite.position.Y + 4);
                        Assets.Play(RoomObj.sfx.hit);
                    }
                    else if (Pro.lifeCounter == 4) //mid-swing
                    {   //cut enemies and roomObjs
                        if (RoomObj.group == ObjGroup.Enemy)
                        { Functions_GameObject_World.CutRoomEnemy(RoomObj); }
                        else { Functions_GameObject_World.Cut(RoomObj); }
                    }
                }

                #endregion


                #region Fang / Bite / Invs Enemy Attack Projectile

                else if (Pro.type == ProjectileType.Bite)
                {   //cut enemies and roomObjs
                    if (RoomObj.group == ObjGroup.Enemy)
                    { Functions_GameObject_World.CutRoomEnemy(RoomObj); }
                    else { Functions_GameObject_World.Cut(RoomObj); }
                    //end projectile's life
                    Pro.lifeCounter = Pro.lifetime;
                }

                #endregion


                #region Boomerang

                else if (Pro.type == ProjectileType.Boomerang)
                {
                    //cue specific sfx
                    if (RoomObj.group == ObjGroup.Wall)
                    { Assets.Play(Assets.sfxTapMetallic); }
                    else if (RoomObj.group == ObjGroup.Door)
                    { Assets.Play(Assets.sfxTapHollow); }
                    else if (RoomObj.type == ObjType.Dungeon_DoorBombable)
                    { Assets.Play(Assets.sfxTapHollow); }

                    //kill room enemies or bounce off objs
                    if (RoomObj.group == ObjGroup.Enemy)
                    { Functions_GameObject_World.CutRoomEnemy(RoomObj); }
                    else { Functions_GameObject_World.Bounce(RoomObj); }

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
                            RoomObj.compSprite.position), 4.0f);
                }

                #endregion
                

                #region GroundFires

                else if (Pro.type == ProjectileType.GroundFire)
                {   //blocking objs only here, btw

                    //ground fires damage and kill roomEnemies
                    if (RoomObj.group == ObjGroup.Enemy)
                    { Functions_GameObject_World.CutRoomEnemy(RoomObj); }


                    //ground fires spread to some room objs
                    else if (
                        //burn bushes
                        RoomObj.type == ObjType.Wor_Bush
                        //burn posts
                        || RoomObj.type == ObjType.Wor_Post_Corner_Left 
                        || RoomObj.type == ObjType.Wor_Post_Corner_Right 
                        || RoomObj.type == ObjType.Wor_Post_Horizontal 
                        || RoomObj.type == ObjType.Wor_Post_Vertical_Left 
                        || RoomObj.type == ObjType.Wor_Post_Vertical_Right
                        //burn trees
                        || RoomObj.type == ObjType.Wor_Tree
                        //light torches
                        || RoomObj.type == ObjType.Dungeon_TorchUnlit
                        )
                    {
                        if (Pro.compCollision.rec.Contains(
                                RoomObj.compSprite.position.X,
                                RoomObj.compSprite.position.Y))
                        {   //check against sprite center pos
                            Functions_GameObject_World.Burn(RoomObj);
                        }   //this prevents immediate fire spread across verticals
                    }

                    //groundfires light explosive barrels too
                    else if (RoomObj.type == ObjType.Dungeon_Barrel)
                    {   //dont push the barrel in any direction
                        RoomObj.compMove.direction = Direction.None;
                        Functions_GameObject_Dungeon.HitBarrel(RoomObj);
                    }
                }

                #endregion


                #region Thrown Objects

                else if (Pro.type == ProjectileType.ThrownObject)
                {   
                    RoomObj.compMove.direction = Pro.compMove.direction;
                    //cut enemies and roomObjs
                    if (RoomObj.group == ObjGroup.Enemy)
                    { Functions_GameObject_World.CutRoomEnemy(RoomObj); }
                    else { Functions_GameObject_World.Cut(RoomObj); }
                    //thrown objs die upon blocking collision
                    Functions_Projectile.Kill(Pro);
                }

                #endregion


                #region Hammer

                else if (Pro.type == ProjectileType.Hammer)
                {   //time this interaction to the hammer hitting the ground
                    if (Pro.compAnim.index == 3)
                    {   //hammers are only pro that alters hammer up posts
                        if (RoomObj.type == ObjType.Wor_Post_Hammer_Up)
                        {
                            Assets.Play(RoomObj.sfx.kill);
                            Functions_GameObject.SetType(RoomObj, ObjType.Wor_Post_Hammer_Down);
                        }
                        else
                        {   
                            if (RoomObj.group == ObjGroup.Enemy)
                            { Functions_GameObject_World.CutRoomEnemy(RoomObj); }
                            else //destroy roomObj
                            { Functions_GameObject_World.Destroy(RoomObj); }
                        }
                    }
                }

                #endregion

            }

            #endregion


            #region Non-blocking RoomObj vs Projectile

            else
            {
                //group  checks first
                
                if (RoomObj.group == ObjGroup.Wall_Climbable)
                {   //kill projectiles for now
                    Functions_Projectile.Kill(Pro);
                }




                //type check next

                #region Roofs - collapse from explosions

                if (
                    RoomObj.type == ObjType.Wor_Build_Roof_Bottom
                    || RoomObj.type == ObjType.Wor_Build_Roof_Top
                    || RoomObj.type == ObjType.Wor_Build_Roof_Chimney
                    )
                {
                    if (Pro.type == ProjectileType.Explosion)
                    {   //begin cascading roof collapse
                        Functions_GameObject_World.CollapseRoof(RoomObj);
                    }
                }

                #endregion


                #region Trap Doors bounce or kill pros

                else if (RoomObj.type == ObjType.Dungeon_DoorTrap)
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

                else if (RoomObj.type == ObjType.Dungeon_Bumper)
                {   //limit bumper to bouncing only if it's returned to 100% scale
                    if (RoomObj.compSprite.scale != 1.0f) { return; }

                    //specific projectiles cannot be bounced off bumper
                    if (Pro.type == ProjectileType.Explosion
                        || Pro.type == ProjectileType.Net
                        || Pro.type == ProjectileType.Sword
                        )
                    { return; }

                    //one of the two objects must be moving,
                    //in order to trigger a bounce interaction
                    if (!RoomObj.compMove.moving & !Pro.compMove.moving)
                    { return; }
                    //all other objects are bounced
                    Functions_GameObject_Dungeon.BounceOffBumper(Pro.compMove, RoomObj);

                    //rotate bounced projectiles
                    Pro.direction = Pro.compMove.direction;
                    Functions_Projectile.SetRotation(Pro);
                }

                #endregion


                #region Fairy can be captured by nets, collected by boomerangs, etc..

                else if (RoomObj.type == ObjType.Dungeon_Fairy)
                {
                    if (Pro.type == ProjectileType.Net)
                    {
                        if (Pro.lifeCounter < Pro.lifetime) //net is still young
                        {
                            //a net has overlapped a fairy (and only a hero can create a net)
                            //attempt to bottle the fairy into one of hero's bottles
                            Functions_Bottle.Bottle(RoomObj);
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
                        Functions_GameObject_Dungeon.UseFairy(RoomObj);
                    }
                }

                #endregion


                #region Tall Grass

                else if (RoomObj.type == ObjType.Wor_Grass_Tall)
                {
                    if (Pro.type == ProjectileType.Sword)
                    {   //cutting
                        Functions_GameObject_World.Destroy(RoomObj);
                    }
                    else if (Pro.type == ProjectileType.GroundFire)
                    {   //burning
                        if (Pro.compCollision.rec.Contains(
                                RoomObj.compSprite.position.X,
                                RoomObj.compSprite.position.Y))
                        {   //check against sprite center pos
                            Functions_GameObject_World.Burn(RoomObj);
                        }   //this prevents immediate fire spread across verticals
                    }
                }

                #endregion


                #region Destroy Open House Doors

                else if(RoomObj.type == ObjType.Wor_Build_Door_Open)
                {
                    if (
                        Pro.type == ProjectileType.Explosion ||
                        Pro.type == ProjectileType.Hammer
                        )
                    { Functions_GameObject.Kill(RoomObj, false, true); }
                }

                #endregion


            }

            #endregion








            //then we just unblock them (return to prev state)

            #region Reset Special Objects post Interaction

            //projectile checks against these objects complete,
            //return them to their non-blocking state
            if (RoomObj.type == ObjType.Wor_SeekerExploder)
            { RoomObj.compCollision.blocking = false; }

            if (RoomObj.group == ObjGroup.Wall_Climbable)
            { RoomObj.compCollision.blocking = false; }

            #endregion

        }













        //object interactions

        public static void Interact_ObjectActor(GameObject RoomObj, Actor Actor)
        {


            #region Hero Specific RoomObj Interactions

            if (Actor == Pool.hero)
            {
                //group checks

                #region Doors

                if (RoomObj.group == ObjGroup.Door)
                {   //handle hero interaction with exit door
                    if (RoomObj.type == ObjType.Dungeon_Exit)
                    {   //stop movement, prevents overlap with exit
                        Functions_Movement.StopMovement(Pool.hero.compMove);
                        Functions_Hero.ExitDungeon();
                        return;
                    }

                    //center Hero to Door horiontally or vertically
                    if (RoomObj.direction == Direction.Up || RoomObj.direction == Direction.Down)
                    {   //gradually center hero to door
                        Pool.hero.compMove.magnitude.X = (RoomObj.compSprite.position.X - Pool.hero.compMove.position.X) * 0.11f;
                        //if hero is close to center of door, snap/lock hero to center of door
                        if (Math.Abs(Pool.hero.compSprite.position.X - RoomObj.compSprite.position.X) < 2)
                        { Pool.hero.compMove.newPosition.X = RoomObj.compSprite.position.X; }
                    }
                    else
                    {   //gradually center hero to door
                        Pool.hero.compMove.magnitude.Y = (RoomObj.compSprite.position.Y - Pool.hero.compMove.position.Y) * 0.11f;
                        //if hero is close to center of door, snap/lock hero to center of door
                        if (Math.Abs(Pool.hero.compSprite.position.Y - RoomObj.compSprite.position.Y) < 2)
                        { Pool.hero.compMove.newPosition.Y = RoomObj.compSprite.position.Y; }
                    }

                }

                #endregion


                //type checks

                #region Map

                if (RoomObj.type == ObjType.Dungeon_Map)
                {
                    if (LevelSet.currentLevel.map == false) //make sure hero doesn't already have map
                    {
                        Functions_Pool.Release(RoomObj); //hero collects map obj
                        RoomObj.compSprite.visible = false; //hide map obj
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

                else if (RoomObj.type == ObjType.Dungeon_Fairy)
                {
                    Functions_GameObject_Dungeon.UseFairy(RoomObj);
                    return;
                }

                #endregion


                #region Roofs

                else if (
                    RoomObj.type == ObjType.Wor_Build_Roof_Bottom
                    || RoomObj.type == ObjType.Wor_Build_Roof_Chimney
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

            if (RoomObj.group == ObjGroup.Door)
            {
                if (RoomObj.type == ObjType.Dungeon_DoorTrap)
                {   //trap doors push ALL actors
                    Functions_Movement.Push(Actor.compMove, RoomObj.direction, 1.0f);
                }
            }

            #endregion


            #region Ditches

            else if (RoomObj.group == ObjGroup.Ditch)
            {   //if the actor is flying, ignore interaction with ditch
                if (Actor.compMove.grounded == false) { return; }
                //if ditch getsAI, then ditch is in it's filled state
                if (RoomObj.getsAI)
                {   //display animated ripple at actor's feet
                    Functions_Actor.DisplayWetFeet(Actor);
                }
            }

            #endregion


            #region Climbable Walls

            else if (RoomObj.group == ObjGroup.Wall_Climbable)
            {

                #region Mid Walls - THESE CAUSING FALLING TO HAPPEN

                if (RoomObj.type == ObjType.Wor_MountainWall_Mid)
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

                else if (RoomObj.type == ObjType.Wor_MountainWall_Bottom)
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

                else if (RoomObj.type == ObjType.Wor_MountainWall_Ladder_Trap)
                {
                    //creates attention particle and removes itself
                    Functions_Particle.Spawn(ParticleType.Attention,
                        RoomObj.compSprite.position.X, RoomObj.compSprite.position.Y, Direction.Down);
                    Functions_Pool.Release(RoomObj);
                    Assets.Play(Assets.sfxShatter);
                }

                #endregion

            }

            #endregion



            //Objects
            else if (RoomObj.group == ObjGroup.Object)
            {
                //Phase 1 - objects that interact with flying and grounded actors

                #region SpikeBlocks

                if (RoomObj.type == ObjType.Dungeon_BlockSpike)
                {   //damage push actors (on ground or in air) away from spikes
                    Functions_Battle.Damage(Actor, RoomObj);
                }

                #endregion


                #region Bumpers

                else if (RoomObj.type == ObjType.Dungeon_Bumper)
                {   //limit bumper to bouncing only if it's returned to 100% scale
                    if (RoomObj.compSprite.scale == 1.0f)
                    { Functions_GameObject_Dungeon.BounceOffBumper(Actor.compMove, RoomObj); }
                }

                #endregion


                #region SwitchBlock UP

                else if (RoomObj.type == ObjType.Dungeon_SwitchBlockUp)
                {   //if actor is colliding with up block, convert up to down
                    Functions_GameObject.SetType(RoomObj, ObjType.Dungeon_SwitchBlockDown);
                }   //this is done because block just popped up and would block actor

                #endregion


                //Phase 2 - all flying actors bail from method - they're done
                if (Actor.compMove.grounded == false) { return; }



                //Phase 3 - objects that interact with grounded actors only (all remaining)

                #region FloorSpikes

                if (RoomObj.type == ObjType.Dungeon_SpikesFloorOn)
                {   //damage push actors (on ground) away from spikes
                    Functions_Battle.Damage(Actor, RoomObj);
                }

                #endregion


                #region ConveyorBelts

                else if (RoomObj.type == ObjType.Dungeon_ConveyorBeltOn)
                {
                    //halt actor movement based on certain states
                    if (Actor.state == ActorState.Reward)
                    { Functions_Movement.StopMovement(Actor.compMove); }
                    else
                    { Functions_GameObject_Dungeon.ConveyorBeltPush(Actor.compMove, RoomObj); }

                }

                #endregion


                #region Pits

                else if (RoomObj.type == ObjType.Dungeon_Pit)
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
                    Actor.compMove.magnitude = (RoomObj.compSprite.position - Actor.compSprite.position) * 0.25f;

                    //if actor is near to pit center, begin/continue falling state
                    if (Math.Abs(Actor.compSprite.position.X - RoomObj.compSprite.position.X) < 2)
                    {
                        if (Math.Abs(Actor.compSprite.position.Y - RoomObj.compSprite.position.Y) < 2)
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
                        Functions_GameObject_Dungeon.PlayPitFx(RoomObj);
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


                #region Ice

                else if (RoomObj.type == ObjType.Dungeon_IceTile)
                {
                    Functions_GameObject_Dungeon.SlideOnIce(Actor.compMove);
                }

                #endregion


                #region Tall grass

                else if (RoomObj.type == ObjType.Wor_Grass_Tall)
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

                else if (RoomObj.type == ObjType.Wor_Water)
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


                #region Swamp Vines

                else if (RoomObj.type == ObjType.Wor_Swamp_Vine)
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
                                RoomObj.compCollision.rec.Center),
                                1.0f);
                        return;
                    }
                }

                #endregion


                #region Coastlines

                else if (RoomObj.type == ObjType.Wor_Coastline_Corner_Exterior
                    || RoomObj.type == ObjType.Wor_Coastline_Corner_Interior
                    || RoomObj.type == ObjType.Wor_Coastline_Straight
                    || RoomObj.type == ObjType.Wor_Boat_Coastline)
                {   //display animated ripple at actor's feet
                    Functions_Actor.DisplayWetFeet(Actor);
                }

                #endregion


                #region PitTrap

                else if (RoomObj.type == ObjType.Dungeon_PitTrap)
                {   //if hero collides with a PitTrapReady, it starts to open
                    Functions_GameObject.SetType(RoomObj, ObjType.Dungeon_Pit);
                    Assets.Play(Assets.sfxShatter); //play collapse sound
                    Functions_Particle.Spawn(ParticleType.Attention,
                        RoomObj.compSprite.position.X,
                        RoomObj.compSprite.position.Y);
                    Functions_Particle.Spawn(ParticleType.ImpactDust,
                        RoomObj.compSprite.position.X + 4,
                        RoomObj.compSprite.position.Y - 8);
                    //Functions_Particle.ScatterDebris(Obj.compSprite.position);
                    //create pit teeth over new pit obj
                    Functions_GameObject.Spawn(ObjType.Dungeon_PitTeethTop,
                        RoomObj.compSprite.position.X,
                        RoomObj.compSprite.position.Y,
                        Direction.Down);
                    Functions_GameObject.Spawn(ObjType.Dungeon_PitTeethBottom,
                        RoomObj.compSprite.position.X,
                        RoomObj.compSprite.position.Y,
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

        public static void Interact_ObjectObject(GameObject RoomObj, GameObject Object)
        {
            //special case OBJECTS (pets and conveyor belts)


            #region Pet / Animals

            if (Object.type == ObjType.Pet_Dog)
            {
                //dogs can swim
                if (RoomObj.type == ObjType.Wor_Water) { Object.inWater = true; }

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


            #region Dungeon Walls (self cleaning)

            else if (
                Object.type == ObjType.Dungeon_WallStraight
                || Object.type == ObjType.Dungeon_WallStraightCracked
                )
            {   //we use life counter to handle roomObj state/lifetime
                if (Object.lifeCounter == 1) //just spawned, do cleanup
                {   //if a wall overlaps a copy of itself, remove wall
                    if (RoomObj.type == Object.type)
                    { Functions_Pool.Release(RoomObj); }
                    //walls cannot overlap these objects
                    if (
                        //exits
                        RoomObj.type == ObjType.Dungeon_Exit
                        || RoomObj.type == ObjType.Dungeon_ExitPillarLeft
                        || RoomObj.type == ObjType.Dungeon_ExitPillarRight
                        //other wall objs
                        || RoomObj.type == ObjType.Dungeon_WallPillar
                        || RoomObj.type == ObjType.Dungeon_WallStatue
                        || RoomObj.type == ObjType.Dungeon_WallTorch
                        || RoomObj.type == ObjType.Dungeon_WallExteriorCorner
                        || RoomObj.type == ObjType.Dungeon_WallInteriorCorner
                        )
                    { Functions_Pool.Release(Object); }
                    Object.lifeCounter = 0; //bail from branch
                }
            }

            #endregion


            #region Dungeon Exits

            else if (
                Object.type == ObjType.Dungeon_Exit
                || Object.type == ObjType.Dungeon_ExitPillarLeft
                || Object.type == ObjType.Dungeon_ExitPillarRight
                )
            {
                if (Object.lifeCounter == 1) //just spawned
                {   //objs that touch exits are removed, except other exits
                    if (
                        RoomObj.type == ObjType.Dungeon_Exit
                        || RoomObj.type == ObjType.Dungeon_ExitPillarLeft
                        || RoomObj.type == ObjType.Dungeon_ExitPillarRight
                        )
                    { } //do nothing, else release
                    else { Functions_Pool.Release(RoomObj); }

                    Object.lifeCounter = 0;
                }
            }

            #endregion









            //Blocking ROOMOBJECT.Type Checks

            if (RoomObj.compCollision.blocking)
            {

                #region ExplodingObject

                if (Object.type == ObjType.ExplodingObject)
                {   //stop exploding object from moving thru blocking objects
                    Functions_Movement.StopMovement(Object.compMove);
                }

                #endregion


                #region SeekerExploders

                else if (Object.type == ObjType.Wor_SeekerExploder)
                {
                    Object.lifeCounter = Object.lifetime; //explode this frame
                    Functions_GameObject_World.Bounce(Object);
                }

                #endregion


                #region SpikeBlock

                else if (Object.type == ObjType.Dungeon_BlockSpike)
                {   //bounce spike blocks off blocking roomObjs
                    Functions_GameObject_Dungeon.BounceSpikeBlock(Object); //rev direction
                    //spikeblocks trigger common obj interactions
                    RoomObj.compMove.direction = Object.compMove.direction;
                    Functions_GameObject_World.Destroy(RoomObj); //destroys most things
                }

                #endregion

            }







            //Non-blocking ROOMOBJECT.Type Checks
            else
            {
                
                #region Water (objects that 'fall' into water - draggable objs)

                if (RoomObj.type == ObjType.Wor_Water)
                {
                    //object groups not removed by water
                    if (Object.group == ObjGroup.Wall || Object.group == ObjGroup.Door
                        || Object.group == ObjGroup.Vendor || Object.group == ObjGroup.NPC
                        || Object.group == ObjGroup.EnemySpawn || Object.group == ObjGroup.Ditch
                        || Object.group == ObjGroup.Wall_Climbable)
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

                else if (RoomObj.group == ObjGroup.Wall_Climbable)
                {
                    //prevent certain wall objs from pulling any objects down the wall
                    if (RoomObj.type == ObjType.Wor_MountainWall_Foothold
                        || RoomObj.type == ObjType.Wor_MountainWall_Ladder
                        || RoomObj.type == ObjType.Wor_MountainWall_Ladder_Trap)
                    { return; }

                    //if hero is climbing/landed with the object as pet, ignore push
                    if (Object == Pool.herosPet)
                    {
                        if (Pool.hero.state == ActorState.Climbing || Pool.hero.state == ActorState.Landed)
                        { return; }
                    }

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

                else if (RoomObj.type == ObjType.Dungeon_DoorTrap)
                {   //prevent obj from passing thru door
                    Functions_Movement.RevertPosition(Object.compMove);

                    //actually, we should be pushing the object the same way we push an actor
                }

                #endregion


                #region Bumper

                else if (RoomObj.type == ObjType.Dungeon_Bumper)
                {   //limit bumper to bouncing only if it's returned to 100% scale
                    if (RoomObj.compSprite.scale != 1.0f) { return; }

                    //one of the two objects must be moving,
                    //in order to trigger a bounce interaction
                    if (!RoomObj.compMove.moving & !Object.compMove.moving)
                    { return; }
                    //all other objects are bounced
                    Functions_GameObject_Dungeon.BounceOffBumper(Object.compMove, RoomObj);
                }

                #endregion








                //floor objects

                #region FloorSpikes

                if (RoomObj.type == ObjType.Dungeon_SpikesFloorOn)
                {
                    if (Object.compMove.grounded)
                    {
                        Object.compMove.direction =
                            Functions_Direction.GetOppositeCardinal(
                                    Object.compMove.position,
                                    RoomObj.compMove.position);
                        Functions_GameObject_World.Destroy(Object);
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

                else if (RoomObj.type == ObjType.Dungeon_IceTile)
                {   //only objects on the ground can slide on ice
                    if (Object.compMove.grounded)
                    {
                        Functions_GameObject_Dungeon.SlideOnIce(Object.compMove);
                    }
                }

                #endregion


                #region Floor Decorations (self-cleaning) - debris, stains blood, skeletons

                //if a floor decoration overlaps an obj, prolly remove it
                else if (
                    RoomObj.type == ObjType.Wor_Debris
                    || RoomObj.type == ObjType.Dungeon_FloorStain
                    || RoomObj.type == ObjType.Dungeon_FloorBlood
                    || RoomObj.type == ObjType.Dungeon_FloorSkeleton
                    )
                {   //we use life counter to handle roomObj state/lifetime
                    if(RoomObj.lifeCounter == 1) //just spawned, do cleanup
                    {

                        #region Clean Yo'Self

                        //if decor overlaps a copy of itself, remove decor
                        if (RoomObj.type == Object.type)
                        { Functions_Pool.Release(RoomObj); }

                        //these objs remove decor too
                        if (
                            //world objs that cant be overlapped
                            Object.type == ObjType.Wor_Flowers
                            || Object.type == ObjType.Wor_Grass_Tall
                            || Object.type == ObjType.Wor_Bush_Stump
                            || Object.type == ObjType.Wor_Tree_Stump


                            //dungeon objs that cant be overlapped
                            || Object.type == ObjType.Dungeon_Pit
                            || Object.type == ObjType.Dungeon_PitBridge
                            || Object.type == ObjType.Dungeon_PitTrap

                            //unique objs that cant be overlapped
                            || Object.type == ObjType.Wor_Boat_Stairs_Cover
                            )
                        {
                            Functions_Pool.Release(RoomObj);
                        }

                        //some decor cant overlap other decor, like debris cant overlap skeletons for example

                        //we sprinkle stains in new dungeon rooms, and they could overlap roomObjs
                        if (RoomObj.type == ObjType.Dungeon_FloorStain & Object.compCollision.blocking)
                        { Functions_Pool.Release(RoomObj); }



                        #endregion


                        RoomObj.lifeCounter = 0; //bail from branch
                    }
                }

                #endregion



                
            }
        }









    }
}