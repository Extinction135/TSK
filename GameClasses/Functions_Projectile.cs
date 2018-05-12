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
    public static class Functions_Projectile
    {
        static Vector2 offset = new Vector2();
        static Projectile pro;
        static Boolean pushLines;
        static Vector2 pushOffset = new Vector2();



        
        public static void Spawn(ObjType Type, float X, float Y)
        {
            //this method is used for projectiles that dont have casters, like groundfires
            //get a projectile to spawn
            pro = Functions_Pool.GetProjectile();
            pro.caster = Pool.hero.compMove; //caster defaults to hero
            
            pro.direction = Direction.None;
            pro.compMove.direction = pro.direction;

            //teleport the object to the caster's location
            Functions_Movement.Teleport(pro.compMove, X, Y);
            pro.compMove.moving = true;
            Functions_GameObject.SetType(pro, Type);
            HandleBehavior(pro);
        }
        




        public static void Spawn(ObjType Type, ComponentMovement Caster, Direction Dir)
        {
            pushLines = false; //assume this pro doesn't have push lines
            pushOffset.X = 0; pushOffset.Y = 0;

            //Dir is usually the actor's / object's facing direction
            //create projectile of TYPE using CASTER, projectile gets DIRECTION
            //the caster is simpified into a moveComp, becase caster could be actor or obj

            //basically, we're setting the caster here and the projectile's initial direction from caster
            //then we call Functions_Ai.HandleObj() to properly place / handle the projectile for initial spawn
            //then for each frame, Functions_Ai.HandleObj() is called and handles ALL projectile behavior

            //check for exit states
            if (Type == ObjType.ProjectileBoomerang)
            {   //only 1 boomerang allowed in play at once
                if (Functions_Hero.boomerangInPlay) { return; }
                else { Functions_Hero.boomerangInPlay = true; }
            }

            //get a projectile to spawn
            pro = Functions_Pool.GetProjectile();
            //set the projectile's caster reference
            pro.caster = Caster;
            //determine the direction the projectile should inherit
            if (Type == ObjType.ProjectileBomb
                || Type == ObjType.ProjectileBoomerang)
            { } //do nothing, we want to be able to throw these projectiles diagonally
            else
            {   //set the projectiles direction to a cardinal one
                Dir = Functions_Direction.GetCardinalDirection(Dir);
            }
            pro.direction = Dir;
            pro.compMove.direction = Dir;

            //teleport the object to the caster's location
            Functions_Movement.Teleport(pro.compMove,
                Caster.position.X, Caster.position.Y);

            //assume this projectile is moving
            pro.compMove.moving = true;
            //finalize it: setType, rotation & align
            Functions_GameObject.SetType(pro, Type);


            #region Set Initial Projectile Behaviors + SoundFX

            if (Type == ObjType.ProjectileArrow)
            {
                //initially place the arrow outside of the caster
                if (Dir == Direction.Down)
                {
                    Functions_Movement.Teleport(pro.compMove,
                        Caster.newPosition.X + 0,
                        Caster.newPosition.Y + 16);
                }
                else if (Dir == Direction.Up)
                {
                    Functions_Movement.Teleport(pro.compMove,
                        Caster.newPosition.X + 0,
                        Caster.newPosition.Y - 14);
                }
                else if (Dir == Direction.Right)
                {
                    Functions_Movement.Teleport(pro.compMove,
                        Caster.newPosition.X + 16,
                        Caster.newPosition.Y + 1);
                }
                else if (Dir == Direction.Left)
                {
                    Functions_Movement.Teleport(pro.compMove,
                        Caster.newPosition.X - 16,
                        Caster.newPosition.Y + 1);
                }
                Functions_Component.Align(pro); //align the arrows comps
                Functions_Movement.Push(pro.compMove, Dir, 6.0f);
                Assets.Play(Assets.sfxArrowShoot);
                pushLines = true;
            }
            else if (Type == ObjType.ProjectileFireball)
            {
                //initially place the arrow outside of the caster
                if (Dir == Direction.Down)
                {
                    Functions_Movement.Teleport(pro.compMove,
                        Caster.newPosition.X + 0,
                        Caster.newPosition.Y + 16);
                }
                else if (Dir == Direction.Up)
                {
                    Functions_Movement.Teleport(pro.compMove,
                        Caster.newPosition.X + 0,
                        Caster.newPosition.Y - 14);
                }
                else if (Dir == Direction.Right)
                {
                    Functions_Movement.Teleport(pro.compMove,
                        Caster.newPosition.X + 16,
                        Caster.newPosition.Y + 1);
                }
                else if (Dir == Direction.Left)
                {
                    Functions_Movement.Teleport(pro.compMove,
                        Caster.newPosition.X - 16,
                        Caster.newPosition.Y + 1);
                }
                Functions_Component.Align(pro); //align the arrows comps
                Functions_Movement.Push(pro.compMove, Dir, 5.0f);
                Assets.Play(Assets.sfxFireballCast);
                pushLines = true;
            }
            else if (Type == ObjType.ProjectileBoomerang)
            {
                Functions_Hero.boomerangInPlay = true;

                //limit diagonal movement in favor of cardinal movement
                //push lines will appear on cardinal direction slides

                //cardinal push full
                if (Dir == Direction.Down || Dir == Direction.Up
                    || Dir == Direction.Left || Dir == Direction.Right)
                { Functions_Movement.Push(pro.compMove, Dir, 5.0f); }
                else//diagonal push half
                { Functions_Movement.Push(pro.compMove, Dir, 2.5f); }
                pushLines = true;
            }
            else if (Type == ObjType.ProjectileBomb)
            {
                Assets.Play(Assets.sfxBombDrop);

                //limit diagonal movement in favor of cardinal movement
                //push lines will appear on cardinal direction slides

                //cardinal push full
                if (Dir == Direction.Down || Dir == Direction.Up
                    || Dir == Direction.Left || Dir == Direction.Right)
                { Functions_Movement.Push(pro.compMove, Dir, 8.0f); }
                else//diagonal push half
                { Functions_Movement.Push(pro.compMove, Dir, 4.0f); }
                pushLines = true;
            }
            else if (Type == ObjType.ProjectileExplodingBarrel)
            {
                Functions_Movement.Push(pro.compMove, Dir, 6.0f);
                Assets.Play(Assets.sfxEnemyHit);
            }
            else if (Type == ObjType.ProjectileExplosion)
            {
                Assets.Play(Assets.sfxExplosion);
                //place smoke puff above explosion
                Functions_Particle.Spawn(
                    ObjType.Particle_ImpactDust,
                    pro.compSprite.position.X + 4,
                    pro.compSprite.position.Y - 8);
            }
            else if (Type == ObjType.ProjectileNet)
            {
                Assets.Play(Assets.sfxNet);
            }
            else if (Type == ObjType.ProjectileSword)
            {
                Assets.Play(Assets.sfxSwordSwipe);
            }

            #endregion


            HandleBehavior(pro);


            #region Add Decorative 'push' lines

            if (pushLines)
            {
                if (Dir == Direction.Down) { pushOffset.X = 4; pushOffset.Y = +6; }
                else if (Dir == Direction.Up) { pushOffset.X = -4; pushOffset.Y = -7; }
                else if (Dir == Direction.Right) { pushOffset.X = +5; pushOffset.Y = -2; }
                else if (Dir == Direction.Left) { pushOffset.X = -5; pushOffset.Y = 6; }
                //place push lines 'after' projectile
                //a diagonal direction will hide the push line
                Functions_Particle.Spawn(
                    ObjType.Particle_Push,
                    Caster.position.X + pushOffset.X,
                    Caster.position.Y + pushOffset.Y, Dir);
            }

            #endregion

        }

        public static void Update(Projectile Pro)
        {   //projectiles do have lifetimes
            Pro.lifeCounter++;
            HandleBehavior(Pro);
            if (Pro.lifeCounter >= Pro.lifetime) { Kill(Pro); }
        }

        public static void Kill(GameObject Obj)
        {
            //contains death events for projectiles

            if (Obj.type == ObjType.ProjectileArrow)
            {
                Functions_Particle.Spawn(
                    ObjType.Particle_Attention,
                    Obj.compSprite.position.X + 0,
                    Obj.compSprite.position.Y + 0);
            }
            else if (Obj.type == ObjType.ProjectileBomb)
            {   //create explosion projectile
                Spawn(ObjType.ProjectileExplosion,
                    Obj.compMove, Direction.None);
            }
            else if (Obj.type == ObjType.ProjectileFireball)
            {   //create explosion & ground fire
                Spawn(ObjType.ProjectileExplosion,
                    Obj.compMove, Direction.None);
            }
            //sword - no death event
            //rock debris - no death event
            else if (Obj.type == ObjType.ProjectileExplodingBarrel)
            {
                //create explosion projectile
                Spawn(ObjType.ProjectileExplosion,
                    Obj.compMove, Direction.None);
                //create loot
                Functions_Loot.SpawnLoot(Obj.compSprite.position);
                //leave some fire behind
                Functions_Projectile.Spawn(
                    ObjType.ProjectileGroundFire,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y);
            }

            //all objects are released upon death
            if (Obj.sfx.kill != null) { Assets.Play(Obj.sfx.kill); }
            Functions_Pool.Release(Obj);
        }

        public static void HandleBehavior(Projectile Pro)
        {
            //the following paths handle the per frame events, or behaviors, of a projectile
            //for example, tracking a sword to it's caster so the caster can slide and attack


            #region Sword & Net

            if (Pro.type == ObjType.ProjectileSword
                || Pro.type == ObjType.ProjectileNet)
            {   //track the projectile to it's caster
                //set offset to make projectile appear in actors hand, based on direction
                if (Pro.direction == Direction.Down) { offset.X = -1; offset.Y = +16; }
                else if (Pro.direction == Direction.Up) { offset.X = +1; offset.Y = -14; }
                else if (Pro.direction == Direction.Right) { offset.X = +15; offset.Y = 0; }
                else if (Pro.direction == Direction.Left) { offset.X = -15; offset.Y = 0; }
                //apply the offset
                Pro.compMove.newPosition.X = Pro.caster.newPosition.X + offset.X;
                Pro.compMove.newPosition.Y = Pro.caster.newPosition.Y + offset.Y;
            }

            #endregion


            #region Boomerang

            else if (Pro.type == ObjType.ProjectileBoomerang)
            {   //boomerang travels in thrown direction until this age, then returns to hero

                #region Behavior From Hero

                if (Pro.lifeCounter > 20)
                {
                    Pro.lifeCounter = 210; //lock pro here

                    //get distance to hero
                    Vector2 distance = Pool.hero.compMove.position - Pro.compMove.position;
                    float speed = 0.14f;

                    //alter boomerang's magnitude to move towards hero's position, per axis
                    if (distance.X > 0) { Pro.compMove.magnitude.X += speed; }
                    else { Pro.compMove.magnitude.X -= speed; }
                    if (distance.Y > 0) { Pro.compMove.magnitude.Y += speed; }
                    else { Pro.compMove.magnitude.Y -= speed; }

                    //boomerang has returned to hero
                    if (Pro.compCollision.rec.Intersects(Pool.hero.compCollision.rec))
                    {
                        Functions_Pool.Release(Pro);
                        Functions_Hero.boomerangInPlay = false;
                        //Assets.Play(Assets.sfxArrowHit);
                    }
                }

                #endregion


                #region Behavior To Hero

                else
                {
                    //nothing really, the boomerang just travels in a straight cardinal direction
                    //until it hits the above lifeCounter check, and then switches to return behavior
                }

                #endregion


                #region Behavior Each Frame

                //rotate boomerang - this is waaaay too fast
                //this is something that should be handled in a spritesheet
                if (Pro.compSprite.rotation == Rotation.None)
                { Pro.compSprite.rotation = Rotation.Clockwise90; }
                else if (Pro.compSprite.rotation == Rotation.Clockwise90)
                { Pro.compSprite.rotation = Rotation.Clockwise180; }
                else if (Pro.compSprite.rotation == Rotation.Clockwise180)
                { Pro.compSprite.rotation = Rotation.Clockwise270; }
                else if (Pro.compSprite.rotation == Rotation.Clockwise270)
                { Pro.compSprite.rotation = Rotation.None; }

                //check if the projectile overlaps any pickups, collect them if so
                Functions_Pickup.CheckOverlap(Pro);
                //play the spinning sound fx each frame
                Assets.Play(Assets.sfxBoomerangFlying);

                #endregion

            }

            #endregion


            #region Bow 

            else if (Pro.type == ObjType.ProjectileBow)
            {   //track the projectile to it's caster
                //set offset to make projectile appear in actors hand, based on direction
                if (Pro.direction == Direction.Down) { offset.X = 0; offset.Y = +8; }
                else if (Pro.direction == Direction.Up) { offset.X = 0; offset.Y = -8; }
                else if (Pro.direction == Direction.Right) { offset.X = +8; offset.Y = 1; }
                else if (Pro.direction == Direction.Left) { offset.X = -8; offset.Y = 1; }
                //apply the offset
                Pro.compMove.newPosition.X = Pro.caster.newPosition.X + offset.X;
                Pro.compMove.newPosition.Y = Pro.caster.newPosition.Y + offset.Y;
            }

            #endregion


            #region Ground Fire

            if (Pro.type == ObjType.ProjectileGroundFire)
            {
                if (Functions_Random.Int(0, 101) > 86)
                {   //often place randomly offset rising smoke
                    Functions_Particle.Spawn(ObjType.Particle_RisingSmoke,
                        Pro.compSprite.position.X + 5 + Functions_Random.Int(-4, 4),
                        Pro.compSprite.position.Y + 1 + Functions_Random.Int(-8, 2));
                }
                //on the fire's interactive frame, spread per axis (round spread - no diagonals)
                if(Pro.lifeCounter == Pro.compInt.interactiveFrame)
                {   //spread horizontally
                    Pro.compCollision.offsetX = -14; Pro.compCollision.rec.Width = 28;
                    Pro.compCollision.offsetY = -4; Pro.compCollision.rec.Height = 8;
                }
                else if(Pro.lifeCounter == Pro.compInt.interactiveFrame + 1)
                {   //spread vertically
                    Pro.compCollision.offsetX = -4; Pro.compCollision.rec.Width = 8;
                    Pro.compCollision.offsetY = -14; Pro.compCollision.rec.Height = 28;
                }
                else
                {   //set collision rec back to normal
                    Pro.compCollision.offsetX = -4; Pro.compCollision.offsetY = -4;
                    Pro.compCollision.rec.Width = 8; Pro.compCollision.rec.Height = 8;
                }
            }

            #endregion


            //teleport the projectile to it's new position
            Functions_Movement.Teleport(Pro.compMove,
                Pro.compMove.newPosition.X,
                Pro.compMove.newPosition.Y);
            //align all the components
            Functions_Component.Align(Pro.compMove, Pro.compSprite, Pro.compCollision);


            #region Ideas

            /*
            
            debrisRock: should be a particle
            projectilePot: hero will be picking up more than pots, obsolete soon
            shadowSm: will be obsolete soon with addition of shadow system
            *keep in mind that altering the ObjType enum values will affect the master GameObjAnimList

            planned:
            push wand: shoots a wave of light that pushes enemies/objs a distance
            boost magic: temporarily increases the damage with your sword to 2, at the cost of some magic
	            *4 beams of light quickly come together on your sword, as the hero holds it up, any nearby enemies are knocked back
	            *boost lasts 255 frames, which is the duration of the projectile that floats around the hero's feet while boost is active

            lttp:
            boomerang: travels from caster position, returns to caster position (tracking) with/out item, pushes actors it collides with
            hookshot: travels from caster position, collides with obj or actor or nothing
	            obj: either latches and pulls caster, or latches and pulls obj, or clinks and returns
	            actor: either latches and pulls actor (sm), or stuns and returns (med), or clinks and returns (large)
	            nothing: hookshot reaches max lifetime and clinks, then dissappears (no waiting about for it to return)
            bombos magic: randomly places 20 explosions around caster, but never ON caster, caster is stationary during cast
            cane of byrna: creates 4 balls of light around caster, which track with an offset, and kill anything they touch, but use magic
            cane of somaria: creates a block of light, which can be pushed / pulled, hold down switches, interact with belts, etc..
            ether magic: lightning strikes every enemy on screen, caster is stationary during
            hammer: used to smash down posts to progress into hidden / secret areas of the game (non-critical paths), tracks to caster
            ice rod: ice magic in game, shoots a ball of ice that turns sm enemies into blocks of ice, 
	            *the blocks of ice are generic, not enemy specific, and have a vague shadow inside them
	            *ice blocks can be picked up and thrown, and will spawn normal loot when destroyed normally
	            *however, smashing an enemy in a block of ice will always spawn a large magic pot
            magic cape: turns hero invisible & flying while in use, with shadow only, uses magic constantly
            magic mirror: returns hero to the start of the current dungeon or overworld
            quake magic: screen shakes, all grounded enemies on screen take 4 damage, caster waits during cast
            shovel: used for digging outside in grass or dirt, low chance to spawn loot, tracks to caster like sword

            moar?:
            remote detonated mines - must equip two items: mines + detonator
                mines are dropped with Y button
                detonator blows up ALL dropped mines with X button
                *this way the player can juggle / strategize more, with faster reaction time
                * *plus, no switching between items to blow stuff up

            */

            #endregion


        }


    }
}