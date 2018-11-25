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
        static int i;





        //dev methods / to remove

        public static void Cast_Bolt(Actor Caster)
        {
            //create a series of bolts in the facing direction of caster

            //resolve caster's direction to a cardinal one
            Caster.direction = Functions_Direction.GetCardinalDirection_LeftRight(Caster.direction);
            //setup offsets based on resolved caster's direction
            if (Caster.direction == Direction.Up) { offset.X = 0; offset.Y = -16; }
            else if (Caster.direction == Direction.Right) { offset.X = +16; offset.Y = 0; }
            else if (Caster.direction == Direction.Down) { offset.X = 0; offset.Y = +16; }
            else if (Caster.direction == Direction.Left) { offset.X = -16; offset.Y = 0; }

            //using the offset, loop create explosions, multiplying offset
            for (i = 1; i < 8; i++)
            {
                Spawn(ProjectileType.ProjectileLightningBolt,
                    Caster.compSprite.position.X + offset.X * i,
                    Caster.compSprite.position.Y + offset.Y * i,
                    Caster.direction);
            }
        }








        public static void Reset(Projectile Pro)
        {   //reset projectile
            Pro.type = ProjectileType.ProjectileArrow; //reset the type
            Pro.direction = Direction.Down;
            Pro.active = true; //assume this object should draw / animate
            Pro.lifetime = 0; //assume obj exists forever (not projectile)
            Pro.lifeCounter = 0; //reset counter

            //reset the sprite component
            Pro.compSprite.drawRec.Width = 16 * 1; //assume cell size is 16x16 (most are)
            Pro.compSprite.drawRec.Height = 16 * 1;
            Pro.compSprite.zOffset = 0;
            Pro.compSprite.flipHorizontally = false;
            Pro.compSprite.rotation = Rotation.None;
            Pro.compSprite.scale = 1.0f;
            Pro.compSprite.texture = Assets.CommonObjsSheet;
            Pro.compSprite.visible = true;

            //reset the animation component
            Pro.compAnim.speed = 10; //set obj's animation speed to default value
            Pro.compAnim.loop = true; //assume obj's animation loops
            Pro.compAnim.index = 0; //reset the current animation index/frame
            Pro.compAnim.timer = 0; //reset the elapsed frames

            //reset the collision component
            Pro.compCollision.blocking = true; //assume the object is blocking (most are)
            Pro.compCollision.rec.Width = 16; //assume collisionRec is 16x16
            Pro.compCollision.rec.Height = 16; //(most are)
            Pro.compCollision.offsetX = -8; //assume collisionRec offset is -8x-8
            Pro.compCollision.offsetY = -8; //(most are)

            //reset the move component
            Pro.compMove.magnitude.X = 0; //discard any previous magnitude
            Pro.compMove.magnitude.Y = 0; //
            Pro.compMove.speed = 0.0f; //assume this object doesn't move
            Pro.compMove.friction = 0.75f; //normal friction
            Pro.compMove.moveable = false; //most objects cant be moved
            Pro.compMove.grounded = true; //most objects exist on the ground

            //reset the sfx component
            Pro.sfx.hit = null;
            Pro.sfx.kill = null;
        }


        public static void SetRotation(Projectile Pro)
        {
            //some pros rotate to appear in casters hand
            if (
                Pro.type == ProjectileType.ProjectileSword
                || Pro.type == ProjectileType.ProjectileNet
                || Pro.type == ProjectileType.ProjectileShovel
                )
            {   //some projectiles flip based on their direction
                if (Pro.direction == Direction.Down || Pro.direction == Direction.Left)
                { Pro.compSprite.flipHorizontally = true; }
            }

            //some pros only face Direction.Down
            else if (
                Pro.type == ProjectileType.ProjectileBomb
                || Pro.type == ProjectileType.ProjectilePot
                || Pro.type == ProjectileType.ProjectilePotSkull
                || Pro.type == ProjectileType.ProjectileBush
                || Pro.type == ProjectileType.ProjectileBoomerang
                || Pro.type == ProjectileType.ProjectileBat
                )
            {   
                Pro.direction = Direction.Down;
                Pro.compSprite.rotation = Rotation.None;
            }

            //other pros rotate to form chains
            else if (
                Pro.type == ProjectileType.ProjectileLightningBolt
                )
            {   //align lightning bolts vertically or horizontally
                if (Pro.direction == Direction.Left || Pro.direction == Direction.Right)
                { Pro.compSprite.rotation = Rotation.Clockwise90; }
                else { Pro.compSprite.rotation = Rotation.None; }
            }

            //other pros have no rotation, but do have direction
            else if (
                Pro.type == ProjectileType.ProjectileHammer
                )
            {
                Pro.compSprite.rotation = Rotation.None;
            }


            //finally, set sprite's rotation based on direction & flipHorizontally boolean
            Functions_Component.SetSpriteRotation(Pro.compSprite, Pro.direction);
        }








        

        //this method is used for projectiles that dont have casters, like groundfires
        public static void Spawn(ProjectileType Type, float X, float Y, Direction Dir)
        {   //also, roomObjs like flamethrower shoot fireballs, and caster can't be a roomObj
            //get a projectile to spawn
            pro = Functions_Pool.GetProjectile();

            //caster defaults to hero, which is really not a good idea
            pro.caster = Pool.hero;

            //some projectiles rely on their direction being none,
            //otherwise they move in the set direction upon spawn
            pro.direction = Dir;
            pro.compMove.direction = pro.direction;

            //put projectile into play
            Functions_Movement.Teleport(pro.compMove, X, Y);
            pro.compMove.moving = true;
            Functions_Projectile.SetType(pro, Type);
            Functions_Component.Align(pro);


            #region Lightning Bolt

            //a bolt is spawned by other bolts, hence no caster
            if (Type == ProjectileType.ProjectileLightningBolt)
            {   //push bolt a little bit, play sfx
                Functions_Movement.Push(pro.compMove, Dir, 2.0f);
                Assets.Play(Assets.sfxShock);
            }

            #endregion


            #region Projectile Bomb

            //what room obj or event spawns a bomb?
            else if (Type == ProjectileType.ProjectileBomb)
            {
                Assets.Play(Assets.sfxBombDrop);
            }

            #endregion


            #region Fireball

            //flamethrowers spawn fireballs, etc..
            else if (Type == ProjectileType.ProjectileFireball)
            {
                Functions_Movement.Push(pro.compMove, Dir, 4.0f);
                Assets.Play(Assets.sfxFireballCast);
                pushLines = true;
            }

            #endregion


            HandleBehavior(pro);
        }
 

        //this method is used (more), and requires a caster for the projectile
        public static void Spawn(ProjectileType Type, Actor Caster, Direction Dir)
        {   //Dir is usually the actor's facing/input direction
            pushLines = false; //reset pushlines (only cardinal dirs)


            #region Boomerang Spawn - only one on screen at a time

            if (Type == ProjectileType.ProjectileBoomerang)
            {   //only 1 boomerang allowed in play at once
                if (Functions_Hero.boomerangInPlay) { return; }
                else { Functions_Hero.boomerangInPlay = true; }
            }

            #endregion


            //get a projectile to spawn
            pro = Functions_Pool.GetProjectile();
            //set the projectile's caster reference
            pro.caster = Caster;


            #region Spawn Projectile Diagonally or Only Cardinal?

            //determine the direction the projectile should inherit
            if (Type == ProjectileType.ProjectileBomb
                || Type == ProjectileType.ProjectileBoomerang
                || Type == ProjectileType.ProjectileBat)
            { } //do nothing, we want to be able to throw these projectiles diagonally
            else
            {
                //set the projectiles direction to a cardinal one
                Dir = Functions_Direction.GetCardinalDirection_LeftRight(Dir);
            }

            #endregion


            pro.compMove.direction = Dir;
            pro.direction = Dir;



            
            //default: teleport projectile to caster's HITBOX center
            Functions_Movement.Teleport(pro.compMove,
                Caster.compCollision.rec.Center.X,
                Caster.compCollision.rec.Center.Y);
            //certain projectiles will teleport themselves more

            //assume this projectile doesn't move
            pro.compMove.moving = false;
            //finalize it: setType, rotation & align
            SetType(pro, Type);




            //these pros must be placed outside of their caster's hitbox initially
            //we just apply an initial force, and the obj's speed moves it until death

            #region Arrows and Fireballs

            //fireball may need a specific offset different from arrows, because they are larger

            if (Type == ProjectileType.ProjectileArrow || Type == ProjectileType.ProjectileFireball)
            {   //move projectile outside of caster's hitbox based on direction
                if (Dir == Direction.Down)
                {   //place centered below casters hb
                    Functions_Movement.Teleport(pro.compMove,
                        Caster.compCollision.rec.Center.X,
                        Caster.compCollision.rec.Y + Caster.compCollision.rec.Height + 16);
                }
                else if (Dir == Direction.Up)
                {   //place centered above casters hb
                    Functions_Movement.Teleport(pro.compMove,
                        Caster.compCollision.rec.Center.X,
                        Caster.compCollision.rec.Y - 14);
                }
                else if (Dir == Direction.Right)
                {   //place centered right side of casters hb
                    Functions_Movement.Teleport(pro.compMove,
                        Caster.compCollision.rec.X + Caster.compCollision.rec.Width + 16,
                        Caster.compCollision.rec.Center.Y + 0);
                }
                else if (Dir == Direction.Left)
                {   //place centered left side of casters hb
                    Functions_Movement.Teleport(pro.compMove,
                        Caster.compCollision.rec.X - 16,
                        Caster.compCollision.rec.Center.Y + 0);
                }

                Functions_Component.Align(pro); //align components
                pro.compMove.moving = true; //moves
                pushLines = true;

                if (Type == ProjectileType.ProjectileArrow)
                {
                    Functions_Movement.Push(pro.compMove, Dir, 6.0f);
                    Assets.Play(Assets.sfxArrowShoot);
                }
                else
                {
                    Functions_Movement.Push(pro.compMove, Dir, 4.0f);
                    Assets.Play(Assets.sfxFireballCast);
                }
            }

            #endregion


            #region Bat projectile

            else if (Type == ProjectileType.ProjectileBat)
            {
                //move projectile outside of caster's hitbox based on direction
                if (Dir == Direction.Down)
                {   //place centered below casters hb
                    Functions_Movement.Teleport(pro.compMove,
                        Caster.compCollision.rec.Center.X,
                        Caster.compCollision.rec.Y + Caster.compCollision.rec.Height + 16);
                }
                else if (Dir == Direction.Up)
                {   //place centered above casters hb
                    Functions_Movement.Teleport(pro.compMove,
                        Caster.compCollision.rec.Center.X,
                        Caster.compCollision.rec.Y - 14);
                }
                else if (Dir == Direction.Right)
                {   //place centered right side of casters hb
                    Functions_Movement.Teleport(pro.compMove,
                        Caster.compCollision.rec.X + Caster.compCollision.rec.Width + 16,
                        Caster.compCollision.rec.Center.Y);
                }
                else if (Dir == Direction.Left)
                {   //place centered left side of casters hb
                    Functions_Movement.Teleport(pro.compMove,
                        Caster.compCollision.rec.X - 16,
                        Caster.compCollision.rec.Center.Y);
                }
                
                //diagonals
                else if (Dir == Direction.UpRight)
                {
                    Functions_Movement.Teleport(pro.compMove,
                        Caster.compCollision.rec.X + Caster.compCollision.rec.Width + 16,
                        Caster.compCollision.rec.Y - 14);
                }
                else if (Dir == Direction.UpLeft)
                {
                    Functions_Movement.Teleport(pro.compMove,
                        Caster.compCollision.rec.X - 16,
                        Caster.compCollision.rec.Y - 14);
                }
                else if (Dir == Direction.DownRight)
                {
                    Functions_Movement.Teleport(pro.compMove,
                        Caster.compCollision.rec.X + Caster.compCollision.rec.Width + 16,
                        Caster.compCollision.rec.Y + Caster.compCollision.rec.Height + 16);
                }
                else if (Dir == Direction.DownLeft)
                {
                    Functions_Movement.Teleport(pro.compMove,
                        Caster.compCollision.rec.X - 16,
                        Caster.compCollision.rec.Y + Caster.compCollision.rec.Height + 16);
                }

                Functions_Component.Align(pro); //align components
                Assets.Play(Assets.sfxRatSqueak);
                pushLines = true;

                //push bat in initial direction, plus random direction
                Functions_Movement.Push(pro.compMove, Dir, 1.5f);
                Functions_Movement.Push(pro.compMove, Functions_Direction.GetRandomDirection(), 0.5f);
            }

            #endregion


            #region Thrown Objects (Bush, Pot, Skull Pot, Small Enemies)

            else if (Type == ProjectileType.ProjectileBush
                || Type == ProjectileType.ProjectilePotSkull
                || Type == ProjectileType.ProjectilePot)
            {
                //move projectile outside of caster's hitbox based on direction
                if (Dir == Direction.Down)
                {   //place centered below casters hb
                    Functions_Movement.Teleport(pro.compMove,
                        Caster.compCollision.rec.Center.X,
                        Caster.compCollision.rec.Y + Caster.compCollision.rec.Height + 16);
                }
                else if (Dir == Direction.Up)
                {   //place centered above casters hb
                    Functions_Movement.Teleport(pro.compMove,
                        Caster.compCollision.rec.Center.X,
                        Caster.compCollision.rec.Y - 14);
                }
                else if (Dir == Direction.Right)
                {   //place centered right side of casters hb
                    Functions_Movement.Teleport(pro.compMove,
                        Caster.compCollision.rec.X + Caster.compCollision.rec.Width + 16,
                        Caster.compCollision.rec.Center.Y);
                }
                else if (Dir == Direction.Left)
                {   //place centered left side of casters hb
                    Functions_Movement.Teleport(pro.compMove,
                        Caster.compCollision.rec.X - 16,
                        Caster.compCollision.rec.Center.Y);
                }

                Functions_Component.Align(pro); //align the pros comps
                Functions_Movement.Push(pro.compMove, Dir, 5.0f);
                //pushLines = true; //this is handled in Throw()
            }

            #endregion




            //projectiles that have no collision effect upon caster

            #region Boomerang

            else if (Type == ProjectileType.ProjectileBoomerang)
            {
                Functions_Hero.boomerangInPlay = true;

                //limit diagonal movement in favor of cardinal movement
                //push lines will appear on cardinal direction slides

                //cardinal push full
                if (Dir == Direction.Down || Dir == Direction.Up
                    || Dir == Direction.Left || Dir == Direction.Right)
                { Functions_Movement.Push(pro.compMove, Dir, 6.0f); }
                else//diagonal push half
                { Functions_Movement.Push(pro.compMove, Dir, 3.0f); }
                pushLines = true;

                //pro.direction = Direction.Down; //boomerangs always face down
            }

            #endregion


            #region Bombs

            else if (Type == ProjectileType.ProjectileBomb)
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

            #endregion



            
            //these projectiles track to their caster per frame, in HandleBehavior()

            #region Net

            else if (Type == ProjectileType.ProjectileNet)
            {
                Assets.Play(Assets.sfxNet);
            }

            #endregion


            #region Sword

            else if (Type == ProjectileType.ProjectileSword)
            {
                Assets.Play(Assets.sfxSwordSwipe);
            }

            #endregion


            #region Shovel

            else if (Type == ProjectileType.ProjectileShovel)
            {
                Assets.Play(Assets.sfxActorLand); //generic use sound
            }

            #endregion


            #region Hammer

            else if (Type == ProjectileType.ProjectileHammer)
            {
                Assets.Play(Assets.sfxActorLand); //generic use sound
                //set anim frame based on direction
                if (pro.direction == Direction.Down)
                {
                    pro.compAnim.currentAnimation = AnimationFrames.Projectile_Hammer_Down;
                }
                else if (pro.direction == Direction.Up)
                {
                    pro.compAnim.currentAnimation = AnimationFrames.Projectile_Hammer_Up;
                }
                else if (pro.direction == Direction.Right)
                {   
                    pro.compAnim.currentAnimation = AnimationFrames.Projectile_Hammer_Right;
                }
                else if (pro.direction == Direction.Left)
                {
                    pro.compAnim.currentAnimation = AnimationFrames.Projectile_Hammer_Left;
                }
            }

            #endregion


            #region Bite Projectile

            else if (Type == ProjectileType.ProjectileBite)
            {
                Assets.Play(Assets.sfxEnemyTaunt);
                pushLines = true;
            }

            #endregion



            //these projectiles dont track or care about their caster

            #region Explosions

            else if (Type == ProjectileType.ProjectileExplosion)
            {
                Assets.Play(Assets.sfxExplosion);
                //place smoke puff above explosion
                Functions_Particle.Spawn(
                    ObjType.Particle_ImpactDust,
                    pro.compSprite.position.X + 4,
                    pro.compSprite.position.Y - 8);
            }

            #endregion


            #region Bombos

            else if (Type == ProjectileType.ProjectileBombos)
            {   //play casting soundfx
                Assets.Play(Assets.sfxCastBombos);
            }

            #endregion


            

            HandleBehavior(pro);
            if (pushLines) { Functions_Particle.SpawnPushFX(Caster.compMove, Dir); }
        }











        public static void Update(Projectile Pro)
        {   //projectiles have lifetimes
            Pro.lifeCounter++;
            HandleBehavior(Pro);
            if (Pro.lifeCounter >= Pro.lifetime) { Kill(Pro); }
        }


        //this method handles the projectile's behavior each frame
        public static void HandleBehavior(Projectile Pro)
        {
            //the following paths handle the per frame events, or behaviors, of a projectile
            //for example, tracking a sword to it's caster or whatevs..



            //track to caster each frame

            #region Bite/Fang

            //fang/bite simply tracks outside of the casters hitbox, centered
            if (Pro.type == ProjectileType.ProjectileBite)
            {
                if (Pro.direction == Direction.Down)
                {   //place centered below casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.Center.X,
                        Pro.caster.compCollision.rec.Y + Pro.caster.compCollision.rec.Height + 6);
                }
                else if (Pro.direction == Direction.Up)
                {   //place centered above casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.Center.X,
                        Pro.caster.compCollision.rec.Y - 8);
                }
                else if (Pro.direction == Direction.Right)
                {   //place centered right side of casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.X + Pro.caster.compCollision.rec.Width + 8,
                        Pro.caster.compCollision.rec.Center.Y);
                }
                else if (Pro.direction == Direction.Left)
                {   //place centered left side of casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.X - 8,
                        Pro.caster.compCollision.rec.Center.Y);
                }
            }

            #endregion


            #region Sword, Net, Shovel

            //sword, net, shovel all track to the HERO'S HAND, based on direction
            else if (Pro.type == ProjectileType.ProjectileSword
                || Pro.type == ProjectileType.ProjectileNet
                || Pro.type == ProjectileType.ProjectileShovel
                )
            {
                if (Pro.direction == Direction.Down)
                {   //place inhand below casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.X - 1 + 8 - 2,
                        Pro.caster.compCollision.rec.Y + Pro.caster.compCollision.rec.Height + 8);
                }
                else if (Pro.direction == Direction.Up)
                {   //place inhand above casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.X + 1 + 8 - 2,
                        Pro.caster.compCollision.rec.Y - 14);
                }
                else if (Pro.direction == Direction.Right)
                {   //place inhand right side of casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.X + Pro.caster.compCollision.rec.Width + 9,
                        Pro.caster.compCollision.rec.Y + 0);
                }
                else if (Pro.direction == Direction.Left)
                {   //place inhand left side of casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.X - 9,
                        Pro.caster.compCollision.rec.Y + 0);
                }
            }

            #endregion


            #region Bow 

            else if (Pro.type == ProjectileType.ProjectileBow)
            {
                //bows appear to be in links/blobs hands (other actors dont matter)
                //move projectile outside of caster's hitbox based on direction
                if (Pro.direction == Direction.Down)
                {   //place centered below casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.Center.X,
                        Pro.caster.compCollision.rec.Y + Pro.caster.compCollision.rec.Height + 1);
                }
                else if (Pro.direction == Direction.Up)
                {   //place centered above casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.Center.X,
                        Pro.caster.compCollision.rec.Y - 7);
                }
                else if (Pro.direction == Direction.Right)
                {   //place centered right side of casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.X + Pro.caster.compCollision.rec.Width + 1,
                        Pro.caster.compCollision.rec.Center.Y);
                }
                else if (Pro.direction == Direction.Left)
                {   //place centered left side of casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.X - 1,
                        Pro.caster.compCollision.rec.Center.Y);
                }
            }

            #endregion


            #region Hammer

            //we need to set hammer's animFrames here, based on direction
            else if (Pro.type == ProjectileType.ProjectileHammer)
            {
                if (Pro.direction == Direction.Down)
                {   //place inhand below casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.X + 12,
                        Pro.caster.compCollision.rec.Y + Pro.caster.compCollision.rec.Height + 7);
                }
                else if (Pro.direction == Direction.Up)
                {   //place inhand above casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.X + 12,
                        Pro.caster.compCollision.rec.Y - 13);
                }
                else if (Pro.direction == Direction.Right)
                {   //place inhand right side of casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.X + Pro.caster.compCollision.rec.Width + 6,
                        Pro.caster.compCollision.rec.Y + 1);

                }
                else if (Pro.direction == Direction.Left)
                {   //place inhand left side of casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.X - 6,
                        Pro.caster.compCollision.rec.Y + 1);

                }


                if (Pro.compAnim.index == 3)
                {   //create random impact fx (right is special case)
                    if (Pro.direction == Direction.Right)
                    {
                        Functions_Particle.Spawn(ObjType.Particle_ImpactDust,
                                Pro.compSprite.position.X + 8 + Functions_Random.Int(-4, 4),
                                Pro.compSprite.position.Y + 8 + Functions_Random.Int(-4, 4));
                    }
                    else
                    {
                        Functions_Particle.Spawn(ObjType.Particle_ImpactDust,
                                Pro.compSprite.position.X + 2 + Functions_Random.Int(-4, 4),
                                Pro.compSprite.position.Y + 8 + Functions_Random.Int(-4, 4));
                    }
                }
            }

            #endregion






            //fire and forget projectiles

            #region Boomerang

            else if (Pro.type == ProjectileType.ProjectileBoomerang)
            {   //boomerang travels in thrown direction until this age, then returns to hero


                #region Behavior From Hero

                if (Pro.lifeCounter > Pro.interactiveFrame)
                {
                    Pro.lifeCounter = 200; //keep pro alive

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

                //check if the projectile overlaps any pickups, collect them if so
                Functions_Pickup.CheckOverlap(Pro);
                //play the spinning sound fx each frame
                Assets.Play(Assets.sfxBoomerangFlying);

                #endregion

            }

            #endregion


            #region Fireballs

            else if (Pro.type == ProjectileType.ProjectileFireball)
            {
                //Debug.WriteLine("processing fireball behavior");
                if (Functions_Random.Int(0, 101) > 50)
                {   //often place randomly offset rising smoke
                    Functions_Particle.Spawn(ObjType.Particle_RisingSmoke,
                        Pro.compSprite.position.X + 5 + Functions_Random.Int(-4, 4),
                        Pro.compSprite.position.Y + 1 + Functions_Random.Int(-8, 2));
                }

                //expand fireballs hitbox to cause interactions with SOME room objs or actors
                if (Pro.lifeCounter == 10 ||
                    Pro.lifeCounter == 20 ||
                    Pro.lifeCounter == 30 ||
                    Pro.lifeCounter == 40 ||
                    Pro.lifeCounter == 50) //only lives to 50
                {   //expand every 10 frames
                    //this allows fireballs to light passing objects on fire / alter them

                    //do a big expansion of the hitbox
                    Pro.compCollision.offsetX = -25; Pro.compCollision.rec.Width = 50;
                    Pro.compCollision.offsetY = -25; Pro.compCollision.rec.Height = 50;
                    //update the hitbox position
                    Pro.compCollision.rec.X = (int)Pro.compMove.position.X + Pro.compCollision.offsetX;
                    Pro.compCollision.rec.Y = (int)Pro.compMove.position.Y + Pro.compCollision.offsetY;

                    //loop roomObjs, checking for specific roomObjs
                    for (i = 0; i < Pool.roomObjCount; i++)
                    {   //check for active objects, then overlap
                        if (Pool.roomObjPool[i].active)
                        {
                            if (Pro.compCollision.rec.Contains(Pool.roomObjPool[i].compCollision.rec))
                            {

                                #region Fireballs Passing By a RoomObject Effects

                                //first, this allow for cascade spreading, where a single fireball
                                //touches several of these objects in sequence as it travels
                                //this creates a cascade effect, handled here - keep this in mind

                                //fireballs can cascade spread across unlit torches
                                if (Pool.roomObjPool[i].type == ObjType.Dungeon_TorchUnlit)
                                {
                                    Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.Dungeon_TorchLit);
                                    Assets.Play(Assets.sfxLightFire);
                                }
                                //fireballs can cascade spread across tall grass
                                else if (Pool.roomObjPool[i].type == ObjType.Wor_Grass_Tall)
                                {   //'burn' the grass
                                    Functions_GameObject_World.CutTallGrass(Pool.roomObjPool[i]);
                                    //spread the fire 
                                    Functions_Projectile.Spawn(
                                        ProjectileType.ProjectileGroundFire,
                                        Pool.roomObjPool[i].compSprite.position.X,
                                        Pool.roomObjPool[i].compSprite.position.Y - 3,
                                        Direction.None);
                                }
                                //fireballs can cascade spread across bushes
                                else if (Pool.roomObjPool[i].type == ObjType.Wor_Bush)
                                {   //spread the fire 
                                    Functions_Projectile.Spawn(
                                        ProjectileType.ProjectileGroundFire,
                                        Pool.roomObjPool[i].compSprite.position.X,
                                        Pool.roomObjPool[i].compSprite.position.Y - 3,
                                        Direction.None);
                                    //destroy the bush
                                    Functions_GameObject_World.DestroyBush(Pool.roomObjPool[i]);
                                    Assets.Play(Assets.sfxLightFire);
                                }

                                //not across trees tho - those take longer to burn

                                //fireballs can cascade spread across barrels too
                                else if (Pool.roomObjPool[i].type == ObjType.Dungeon_Barrel)
                                {
                                    //inherit the direction of the fireball
                                    Pool.roomObjPool[i].compMove.direction = Pro.compMove.direction;
                                    Functions_GameObject_Dungeon.HitBarrel(Pool.roomObjPool[i]);
                                }

                                //fireballs burn wooden posts
                                else if (Pool.roomObjPool[i].type == ObjType.Wor_Post_Corner_Left ||
                                    Pool.roomObjPool[i].type == ObjType.Wor_Post_Corner_Right ||
                                    Pool.roomObjPool[i].type == ObjType.Wor_Post_Horizontal ||
                                    Pool.roomObjPool[i].type == ObjType.Wor_Post_Vertical_Left ||
                                    Pool.roomObjPool[i].type == ObjType.Wor_Post_Vertical_Right)
                                {
                                    //spread the fire 
                                    Functions_Projectile.Spawn(
                                        ProjectileType.ProjectileGroundFire,
                                        Pool.roomObjPool[i].compSprite.position.X,
                                        Pool.roomObjPool[i].compSprite.position.Y - 3,
                                        Direction.None);
                                    //burn the post
                                    Functions_GameObject_World.BurnPost(Pool.roomObjPool[i]);
                                    Assets.Play(Assets.sfxLightFire);
                                }

                                #endregion

                            }
                        }
                    }

                    //we reset hitbox like this, but this means debug wont draw this larger rec
                    //set collision rec to normal
                    Pro.compCollision.offsetX = -5; Pro.compCollision.offsetY = -5;
                    Pro.compCollision.rec.Width = 10; Pro.compCollision.rec.Height = 10;
                    //update the hitbox position again
                    Pro.compCollision.rec.X = (int)Pro.compMove.position.X + Pro.compCollision.offsetX;
                    Pro.compCollision.rec.Y = (int)Pro.compMove.position.Y + Pro.compCollision.offsetY;
                }
            }

            #endregion






            //magic - unique behaviors

            #region Bombos 

            else if (Pro.type == ProjectileType.ProjectileBombos)
            {   //casted magic tracks over caster's head
                Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.X + 8,
                        Pro.caster.compCollision.rec.Y - 12);

                if (Functions_Random.Int(0, 100) > 40)
                {   //create a bomb each frame, for a wide area around caster
                    Spawn(ProjectileType.ProjectileBomb,
                        Camera2D.currentPosition.X + Functions_Random.Int(-16 * 22, 16 * 22),
                        Camera2D.currentPosition.Y + Functions_Random.Int(-16 * 12, 16 * 12),
                        Direction.None);
                }
                //when bombos reaches a certain age, play multiple explosions sfx
                //this plays right around the time bombs should start exploding
                //and again when the first sfx ends, keeping bombs sounds going on
                //this helps to increase the overall awesomeness of bombos
                if (Pro.lifeCounter == 60 || Pro.lifeCounter == 160)
                { Assets.Play(Assets.sfxExplosionsMultiple); }
            }

            #endregion






            //misc

            #region Ground Fire

            else if (Pro.type == ProjectileType.ProjectileGroundFire)
            {
                if (Functions_Random.Int(0, 101) > 86)
                {   //often place randomly offset rising smoke
                    Functions_Particle.Spawn(ObjType.Particle_RisingSmoke,
                        Pro.compSprite.position.X + 5 + Functions_Random.Int(-4, 4),
                        Pro.compSprite.position.Y + 1 + Functions_Random.Int(-8, 2));
                }
                //expand groundfires hitbox to cause interactions with room objs
                if (Pro.lifeCounter == Pro.interactiveFrame)
                {   //spread horizontally on interaction frame
                    Pro.compCollision.offsetX = -17; Pro.compCollision.rec.Width = 34;
                    Pro.compCollision.offsetY = -4; Pro.compCollision.rec.Height = 8;
                }
                else if (Pro.lifeCounter == Pro.interactiveFrame + 1)
                {   //spread vertically on next frame
                    Pro.compCollision.offsetX = -4; Pro.compCollision.rec.Width = 8;
                    //xtra south for south bushes/objs with lower hitboxes (further away)
                    Pro.compCollision.offsetY = -17; Pro.compCollision.rec.Height = 38;
                }
                else
                {   //set collision rec back to normal on 3rd frame
                    Pro.compCollision.offsetX = -4; Pro.compCollision.offsetY = -4;
                    Pro.compCollision.rec.Width = 8; Pro.compCollision.rec.Height = 8;
                }
            }

            #endregion






            //align all the components
            Functions_Component.Align(Pro.compMove, Pro.compSprite, Pro.compCollision);
        }

        
        public static void Kill(Projectile Pro)
        {
            //contains death events for projectiles


            if (Pro.type == ProjectileType.ProjectileArrow
                || Pro.type == ProjectileType.ProjectileBat)
            {
                Functions_Particle.Spawn(
                    ObjType.Particle_Attention,
                    Pro.compSprite.position.X + 0,
                    Pro.compSprite.position.Y + 0);
            }

            else if (Pro.type == ProjectileType.ProjectileBomb)
            {
                //create explosion projectile + ground fire
                Spawn(ProjectileType.ProjectileExplosion, Pro.compMove.position.X, Pro.compMove.position.Y, Direction.None);
                //create groundfire
                Spawn(ProjectileType.ProjectileGroundFire, Pro.compMove.position.X, Pro.compMove.position.Y, Direction.None);
            }
            else if (Pro.type == ProjectileType.ProjectileFireball)
            {   //create explosion
                Spawn(ProjectileType.ProjectileExplosion, Pro.compMove.position.X, Pro.compMove.position.Y, Direction.None);
                //create groundfire
                Spawn(ProjectileType.ProjectileGroundFire, Pro.compMove.position.X, Pro.compMove.position.Y, Direction.None);
            }



            #region Thrown Objs

            else if (Pro.type == ProjectileType.ProjectileBush)
            {
                Functions_Loot.SpawnLoot(Pro.compSprite.position);
                //pop leaves
                Functions_Particle.Spawn_Explosion(
                    ObjType.Particle_Leaf,
                    Pro.compSprite.position.X,
                    Pro.compSprite.position.Y,
                    true);
            }
            else if (Pro.type == ProjectileType.ProjectilePotSkull
                || Pro.type == ProjectileType.ProjectilePot)
            {
                Functions_Loot.SpawnLoot(Pro.compSprite.position);
                //pop debris
                Functions_Particle.Spawn_Explosion(
                    ObjType.Particle_Debris,
                    Pro.compSprite.position.X,
                    Pro.compSprite.position.Y,
                    true);
            }

            #endregion


            //all objects are released upon death
            if (Pro.sfx.kill != null) { Assets.Play(Pro.sfx.kill); }
            Functions_Pool.Release(Pro);
        }








        public static void SetType(Projectile Pro, ProjectileType Type)
        {
            Pro.type = Type;
            Pro.compSprite.texture = Assets.entitiesSheet;






            
            //Projectiles


            #region Projectiles - Items

            if (Type == ProjectileType.ProjectileBomb)
            {
                Pro.compSprite.zOffset = -4; //sort to floor
                Pro.compCollision.offsetX = -5; Pro.compCollision.offsetY = -5;
                Pro.compCollision.rec.Width = 10; Pro.compCollision.rec.Height = 10;
                Pro.lifetime = 100; //in frames
                Pro.compAnim.speed = 7; //in frames
                Pro.compMove.moveable = true;
                Pro.compAnim.currentAnimation = AnimationFrames.Projectile_Bomb;
                Pro.compSprite.texture = Assets.entitiesSheet;
            }
            else if (Type == ProjectileType.ProjectileBoomerang)
            {
                Pro.compSprite.zOffset = 0;
                Pro.compCollision.offsetX = -5; Pro.compCollision.offsetY = -5;
                Pro.compCollision.rec.Width = 10; Pro.compCollision.rec.Height = 10;

                Pro.interactiveFrame = 20; //frame boomerang returns to hero
                Pro.lifetime = 255;  //must be greater than 0, but is kept at 200

                Pro.compMove.friction = 0.96f; //some air friction
                Pro.compAnim.speed = 3; //very fast, in frames
                Pro.compMove.moveable = true;
                Pro.compMove.grounded = false; //obj is airborne
                Pro.compAnim.currentAnimation = AnimationFrames.Projectile_Boomerang;
                Pro.compSprite.texture = Assets.entitiesSheet;
            }

            #endregion


            #region Projectiles - Magic

            else if (Type == ProjectileType.ProjectileFireball)
            {
                Pro.compSprite.zOffset = 16;
                Pro.compCollision.offsetX = -5; Pro.compCollision.offsetY = -5;
                Pro.compCollision.rec.Width = 10; Pro.compCollision.rec.Height = 10;
                Pro.lifetime = 50; //in frames
                Pro.compMove.friction = World.frictionIce;
                Pro.compAnim.speed = 5; //in frames
                Pro.compMove.moveable = true;
                Pro.compMove.grounded = false; //obj is airborne
                Pro.compAnim.currentAnimation = AnimationFrames.Projectile_Fireball;
                Pro.compSprite.texture = Assets.entitiesSheet;
            }

            else if (Type == ProjectileType.ProjectileLightningBolt)
            {
                Pro.compSprite.zOffset = 16;
                Pro.compCollision.offsetX = -7; Pro.compCollision.offsetY = -7;
                Pro.compCollision.rec.Width = 14; Pro.compCollision.rec.Height = 14;
                Pro.lifetime = 25; //in frames
                Pro.compMove.friction = World.frictionIce;
                Pro.compAnim.speed = 1; //in frames
                Pro.compMove.grounded = false; //obj is airborne
                Pro.compAnim.currentAnimation = AnimationFrames.Projectile_Bolt;
                Pro.compSprite.texture = Assets.entitiesSheet;
            }

            else if (Type == ProjectileType.ProjectileBombos)
            {
                Pro.compSprite.zOffset = 32;
                Pro.compCollision.offsetX = -1; Pro.compCollision.offsetY = -1;
                Pro.compCollision.rec.Width = 3; Pro.compCollision.rec.Height = 3;
                Pro.lifetime = 255; //in frames
                Pro.compMove.grounded = false; //obj is airborne
                Pro.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Magic_Bombos;
                Pro.compSprite.texture = Assets.uiItemsSheet; //reuse bombos ui sprite
            }

            #endregion




            //Projectiles - Weapons

            #region Sword

            else if (Type == ProjectileType.ProjectileSword)
            {
                Pro.compSprite.zOffset = 16;
                //set collision rec based on direction
                if (Pro.direction == Direction.Up)
                {
                    Pro.compCollision.offsetX = -4; Pro.compCollision.offsetY = -4;
                    Pro.compCollision.rec.Width = 10; Pro.compCollision.rec.Height = 15;
                }
                else if (Pro.direction == Direction.Down)
                {
                    Pro.compCollision.offsetX = -4; Pro.compCollision.offsetY = -5;
                    Pro.compCollision.rec.Width = 10; Pro.compCollision.rec.Height = 10;
                }
                else if (Pro.direction == Direction.Left)
                {
                    Pro.compCollision.offsetX = -4; Pro.compCollision.offsetY = -3;
                    Pro.compCollision.rec.Width = 11; Pro.compCollision.rec.Height = 10;
                }
                else //right
                {
                    Pro.compCollision.offsetX = -7; Pro.compCollision.offsetY = -3;
                    Pro.compCollision.rec.Width = 11; Pro.compCollision.rec.Height = 10;
                }
                
                Pro.lifetime = 18; //in frames
                Pro.compAnim.speed = 2; //in frames
                Pro.compAnim.loop = false;
                Pro.compMove.moveable = true;
                Pro.compMove.grounded = false; //is flying, cant fall into pit
                Pro.compSprite.texture = Assets.entitiesSheet;
                Pro.compAnim.currentAnimation = AnimationFrames.Projectile_Sword;
            }

            #endregion


            #region Shovel

            else if (Type == ProjectileType.ProjectileShovel)
            {
                Pro.compSprite.zOffset = 16;
                //set collision rec based on direction
                if (Pro.direction == Direction.Up)
                {
                    Pro.compCollision.offsetX = -1; Pro.compCollision.offsetY = -4;
                    Pro.compCollision.rec.Width = 10; Pro.compCollision.rec.Height = 15;
                }
                else if (Pro.direction == Direction.Down)
                {
                    Pro.compCollision.offsetX = -1; Pro.compCollision.offsetY = -4;
                    Pro.compCollision.rec.Width = 10; Pro.compCollision.rec.Height = 10;
                }
                else if (Pro.direction == Direction.Left)
                {
                    Pro.compCollision.offsetX = -4; Pro.compCollision.offsetY = -1;
                    Pro.compCollision.rec.Width = 11; Pro.compCollision.rec.Height = 10;
                }
                else //right
                {
                    Pro.compCollision.offsetX = -7; Pro.compCollision.offsetY = -1;
                    Pro.compCollision.rec.Width = 11; Pro.compCollision.rec.Height = 10;
                }
                
                Pro.lifetime = 18; //in frames
                Pro.compAnim.speed = 2; //in frames
                Pro.compAnim.loop = false;
                Pro.compMove.moveable = true;
                Pro.compMove.grounded = false; //is flying, cant fall into pit
                Pro.compSprite.texture = Assets.entitiesSheet;
                Pro.compAnim.currentAnimation = AnimationFrames.Projectile_Shovel;
            }

            #endregion


            #region Hammer

            else if (Type == ProjectileType.ProjectileHammer)
            {
                Pro.compSprite.zOffset = 6;
                Pro.compCollision.rec.Width = 10;
                Pro.compCollision.rec.Height = 10;

                //set collision rec offsets based on direction
                if (Pro.direction == Direction.Up)
                {
                    Pro.compCollision.offsetX = -7; Pro.compCollision.offsetY = -4;
                }
                else if (Pro.direction == Direction.Down)
                {
                    Pro.compCollision.offsetX = -8; Pro.compCollision.offsetY = -5;
                }
                else if (Pro.direction == Direction.Left)
                {
                    Pro.compCollision.offsetX = -4 - 3; Pro.compCollision.offsetY = -3;
                }
                else //right
                {
                    Pro.compCollision.offsetX = -7 + 3; Pro.compCollision.offsetY = -3;
                }
                
                Pro.lifetime = 16; //in frames
                Pro.compAnim.speed = 2; //in frames
                Pro.compAnim.loop = false;
                Pro.compMove.moveable = true;
                Pro.compMove.grounded = false; //is flying, cant fall into pit
                Pro.compSprite.texture = Assets.entitiesSheet;
                Pro.compAnim.currentAnimation = AnimationFrames.Projectile_Hammer_Down;
            }

            #endregion


            #region Arrow and Bow

            else if (Type == ProjectileType.ProjectileArrow)
            {
                Pro.compSprite.zOffset = 16;
                //set collision rec based on direction
                if (Pro.direction == Direction.Up || Pro.direction == Direction.Down)
                {
                    Pro.compCollision.offsetX = -2; Pro.compCollision.offsetY = -6;
                    Pro.compCollision.rec.Width = 4; Pro.compCollision.rec.Height = 12;
                }
                else //left or right
                {
                    Pro.compCollision.offsetX = -6; Pro.compCollision.offsetY = -2;
                    Pro.compCollision.rec.Width = 12; Pro.compCollision.rec.Height = 4;
                }
                Pro.lifetime = 200; //in frames
                Pro.compAnim.speed = 5; //in frames
                Pro.compMove.friction = 1.0f; //no air friction
                Pro.compMove.moveable = true;
                Pro.compMove.grounded = false; //obj is airborne
                Pro.compAnim.currentAnimation = AnimationFrames.Projectile_Arrow;
                Pro.compSprite.texture = Assets.entitiesSheet;
                Pro.sfx.kill = Assets.sfxArrowHit;
                Pro.sfx.hit = Assets.sfxArrowHit;
            }
            else if (Type == ProjectileType.ProjectileBow)
            {
                Pro.compSprite.zOffset = 0;
                Pro.lifetime = 15; //in frames
                Pro.compAnim.speed = 10; //in frames
                Pro.compAnim.loop = false;
                Pro.compMove.grounded = false; //obj is airborne
                Pro.compAnim.currentAnimation = AnimationFrames.Projectile_Bow;
                Pro.compSprite.texture = Assets.entitiesSheet;
                Pro.sfx.hit = null; Pro.sfx.kill = null;
            }


            #endregion


            #region Net

            else if (Type == ProjectileType.ProjectileNet)
            {
                Pro.compSprite.zOffset = 16;
                Pro.compCollision.offsetX = -5; Pro.compCollision.offsetY = -5;
                Pro.compCollision.rec.Width = 10; Pro.compCollision.rec.Height = 10;
                Pro.lifetime = 18; //in frames
                Pro.compAnim.speed = 2; //in frames
                Pro.compAnim.loop = false;
                Pro.compMove.moveable = true;
                Pro.compMove.grounded = false; //obj is airborne
                Pro.compAnim.currentAnimation = AnimationFrames.Projectile_Net;
                Pro.compSprite.texture = Assets.entitiesSheet;
                Pro.sfx.hit = null; Pro.sfx.kill = null;
            }

            #endregion







            #region Projectiles - World

            else if (Type == ProjectileType.ProjectileExplosion)
            {
                Pro.compSprite.zOffset = 16;
                Pro.compCollision.offsetX = -12; Pro.compCollision.offsetY = -13;
                Pro.compCollision.rec.Width = 24; Pro.compCollision.rec.Height = 26;
                Pro.lifetime = 24; //in frames
                Pro.compAnim.speed = 5; //in frames
                Pro.compAnim.loop = false;
                Pro.compMove.grounded = false; //obj is airborne
                Pro.compAnim.currentAnimation = AnimationFrames.Projectile_Explosion;
                Pro.compSprite.texture = Assets.entitiesSheet;
                Pro.sfx.kill = Assets.sfxExplosion;
                Pro.sfx.hit = Assets.sfxExplosion;
            }
            else if (Type == ProjectileType.ProjectileGroundFire)
            {
                Pro.compSprite.zOffset = 6;
                Pro.lifetime = 100; //in frames
                Pro.compAnim.speed = 7; //in frames
                Pro.compAnim.currentAnimation = AnimationFrames.Projectile_FireGround;
                Pro.compSprite.texture = Assets.entitiesSheet;
                //this controls how quick fire spreads:
                Pro.interactiveFrame = 60; //early in life = quick spread
                Pro.interactiveFrame += Functions_Random.Int(-15, 15);
                //add a random -/+ offset to stagger the spread
            }

            #endregion


            #region Projectiles - Thrown

            else if (Type == ProjectileType.ProjectileBush
                || Type == ProjectileType.ProjectilePot
                || Type == ProjectileType.ProjectilePotSkull)
            {
                //bushes and pots exist on commonObjs sheet,
                //but skull pot exists on the dungeon sheet
                if (Type == ProjectileType.ProjectilePotSkull)
                { Pro.compSprite.texture = Assets.Dungeon_CurrentSheet; }

                //thrown projectile attributes
                Pro.compSprite.zOffset = 32;
                Pro.lifetime = 20; //in frames
                Pro.compMove.grounded = false; //obj is airborne
                Pro.compMove.friction = 0.984f; //some air friction
                //refine this hitBox later
                Pro.compCollision.offsetX = -5; Pro.compCollision.offsetY = -5;
                Pro.compCollision.rec.Width = 10; Pro.compCollision.rec.Height = 10;

                //set animFrame based on type
                if (Type == ProjectileType.ProjectileBush)
                {
                    Pro.compAnim.currentAnimation = AnimationFrames.World_Bush;
                    Pro.sfx.kill = Assets.sfxBushCut;
                    Pro.sfx.hit = Assets.sfxEnemyHit;
                }
                else if (Type == ProjectileType.ProjectilePot)
                {
                    Pro.compAnim.currentAnimation = AnimationFrames.Wor_Pot;
                    Pro.sfx.hit = Assets.sfxEnemyHit;
                    Pro.sfx.kill = Assets.sfxShatter;
                }
                else
                {   //skull pot is default
                    Pro.compAnim.currentAnimation = AnimationFrames.Dungeon_Pot;
                    Pro.sfx.hit = Assets.sfxEnemyHit;
                    Pro.sfx.kill = Assets.sfxShatter;
                }
            }

            #endregion


            #region Projectiles - Enemy Related

            else if (Type == ProjectileType.ProjectileBite)
            {
                Pro.compSprite.zOffset = 16;

                Pro.compCollision.offsetX = -4;
                Pro.compCollision.offsetY = -4;
                Pro.compCollision.rec.Width = 8;
                Pro.compCollision.rec.Height = 8;
                
                Pro.lifetime = 15; //in frames
                Pro.compAnim.speed = 1; //in frames
                Pro.compAnim.loop = false;
                Pro.compMove.moveable = true;
                Pro.compMove.grounded = false; //obj is airborne
                Pro.compSprite.texture = Assets.entitiesSheet; //null / doesn't matter cause..
                Pro.compSprite.visible = false; //..this projectile isnt drawn
                Pro.compAnim.currentAnimation = AnimationFrames.Projectile_Sword; //null too
            }

            else if (Type == ProjectileType.ProjectileBat)
            {
                Pro.compSprite.zOffset = 16;

                Pro.compCollision.offsetX = -4; Pro.compCollision.offsetY = -8;
                Pro.compCollision.rec.Width = 8; Pro.compCollision.rec.Height = 8;
                
                Pro.lifetime = 200; //in frames
                Pro.compAnim.speed = 10; //in frames
                Pro.compMove.friction = 1.0f; //no air friction
                Pro.compMove.moveable = true;
                Pro.compMove.grounded = false; //obj is airborne
                Pro.compAnim.currentAnimation = AnimationFrames.Projectile_Bat;
                Pro.compSprite.texture = Assets.EnemySheet;
                Pro.sfx.kill = Assets.sfxRatSqueak;
                Pro.sfx.hit = null;
            }

            #endregion




            //projectiles never block
            Pro.compCollision.blocking = false;
            //set rotations, animation, align all components
            SetRotation(Pro);
            Pro.compSprite.currentFrame = Pro.compAnim.currentAnimation[0]; //goto 1st anim frame
            Functions_Component.Align(Pro.compMove, Pro.compSprite, Pro.compCollision);
        }

    }
}