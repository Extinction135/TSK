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
        static Projectile pro;
        static Boolean pushLines;
        static int i;




        //resets the projectile to a default state
        public static void Reset(Projectile Pro)
        {   //reset projectile
            Pro.type = ProjectileType.Arrow; //reset the type
            Pro.direction = Direction.Down;
            Pro.active = true; //assume this object should draw / animate
            Pro.lifetime = 0; //assume obj exists forever (not projectile)
            Pro.lifeCounter = 0; //reset counter
            
            //clear hit actor/obj references + offsets
            Pro.hitActor = null;
            Pro.hitObj = null;
            Pro.hitOffsetX = 0;
            Pro.hitOffsetY = 0;

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
            SetType(pro, Type);
            Functions_Component.Align(pro);




            #region Projectile Bomb

            //what room obj or event spawns a bomb?
            if (Type == ProjectileType.Bomb)
            {
                Assets.Play(Assets.sfxBombDrop);
            }

            #endregion


            #region Fireball

            //flamethrowers spawn fireballs, etc..
            else if (Type == ProjectileType.Fireball)
            {
                Functions_Movement.Push(pro.compMove, Dir, 4.0f);
                Assets.Play(Assets.sfxFireballCast);
                pushLines = true;
            }

            #endregion


            Update(pro);
        }

        //this method is used (more), and requires a caster for the projectile
        public static void Spawn(ProjectileType Type, Actor Caster, Direction Dir)
        {   //Dir is usually the actor's facing/input direction
            pushLines = false; //reset pushlines (only cardinal dirs)


            #region Boomerang Spawn - only one on screen at a time

            if (Type == ProjectileType.Boomerang)
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
            if (Type == ProjectileType.Bomb
                || Type == ProjectileType.Boomerang
                || Type == ProjectileType.Bat)
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
            SetType(pro, Type); //may later pro.direction!




            //these pros must be placed outside of their caster's hitbox initially
            //we just apply an initial force, and the obj's speed moves it until death


            #region Arrow, Fireball, Iceball

            //fireball may need a specific offset different from arrows, because they are larger

            if (
                Type == ProjectileType.Arrow || 
                Type == ProjectileType.Fireball || 
                Type == ProjectileType.Iceball
                )
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
                
                if (Type == ProjectileType.Arrow)
                {
                    Functions_Movement.Push(pro.compMove, Dir, 6.0f);
                    Assets.Play(Assets.sfxArrowShoot);
                    pushLines = true;
                }
                else if(Type == ProjectileType.Fireball)
                {   //no push lines for fireball cast - it comes from a rod
                    Functions_Movement.Push(pro.compMove, Dir, 4.0f);
                    Assets.Play(Assets.sfxFireballCast);
                }
                else if (Type == ProjectileType.Iceball)
                {   //no push lines for iceball cast - it comes from a rod
                    Functions_Movement.Push(pro.compMove, Dir, 4.0f);
                    Assets.Play(Assets.sfxIcerod);
                }
            }


            #endregion


            #region Bat projectile

            else if (Type == ProjectileType.Bat)
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


            #region Iceblock Cracking

            else if (Type == ProjectileType.IceblockCracking)
            {   //play initial crack sfx
                Assets.Play(Assets.sfxShatter);
            }

            #endregion



            //projectiles that have no collision effect upon caster

            #region Boomerang

            else if (Type == ProjectileType.Boomerang)
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

            else if (Type == ProjectileType.Bomb)
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

            else if (Type == ProjectileType.Net)
            {
                Assets.Play(Assets.sfxNet);
            }

            #endregion


            #region Sword

            else if (Type == ProjectileType.Sword)
            {
                Assets.Play(Assets.sfxSwordSwipe);
            }

            #endregion


            #region Shovel

            else if (Type == ProjectileType.Shovel)
            {
                Assets.Play(Assets.sfxActorLand); //generic use sound
            }

            #endregion


            #region Hammer

            else if (Type == ProjectileType.Hammer)
            {
                Assets.Play(Assets.sfxHammer);

                //hammer down animFrame has already been set by SetType()
                //set other directional animFrames
                if (Dir == Direction.Up)
                { pro.compAnim.currentAnimation = AnimationFrames.Projectile_Hammer_Up; }

                else if (Dir == Direction.Right)
                { pro.compAnim.currentAnimation = AnimationFrames.Projectile_Hammer_Right; }

                else if (Dir == Direction.Left)
                { pro.compAnim.currentAnimation = AnimationFrames.Projectile_Hammer_Left; }
            }

            #endregion


            #region Firerod

            else if (Type == ProjectileType.Firerod)
            {
                //Assets.Play(Assets.sfxActorLand); //generic use sound
            }

            #endregion


            #region Icerod

            else if (Type == ProjectileType.Icerod)
            {
                //Assets.Play(Assets.sfxActorLand); //generic use sound
            }

            #endregion


            #region Bite Projectile

            else if (Type == ProjectileType.Bite)
            {
                Assets.Play(Assets.sfxEnemyTaunt);
                pushLines = true;
            }

            #endregion


            #region Wand

            else if (Type == ProjectileType.Wand)
            {
                Assets.Play(Assets.sfxArrowShoot); //this could be magically better
            }

            #endregion

            //these pros track to their hit obj/actor per frame






            //these projectiles dont track or care about their caster

            #region Explosions

            else if (Type == ProjectileType.Explosion)
            {
                Assets.Play(Assets.sfxExplosion);
                //place smoke puff above explosion
                Functions_Particle.Spawn(
                    ParticleType.ImpactDust,
                    pro.compSprite.position.X + 4,
                    pro.compSprite.position.Y - 8);
            }

            #endregion

            




            Update(pro);
            if (pushLines) { Functions_Particle.SpawnPushFX(Caster.compMove, Dir); }
        }







        //per-frame logic
        public static void Update(Projectile Pro)
        {   
            Pro.lifeCounter++;
            if (Pro.lifeCounter >= Pro.lifetime) { Kill(Pro); }


            //handles per frame events, or behaviors, of a projectile
            //for example, tracking a sword to it's caster's hand



            //track to caster each frame

            #region Bite/Fang

            //fang/bite simply tracks outside of the casters hitbox, centered
            if (Pro.type == ProjectileType.Bite)
            {
                if (Pro.compMove.direction == Direction.Down)
                {   //place centered below casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.Center.X,
                        Pro.caster.compCollision.rec.Y + Pro.caster.compCollision.rec.Height + 6);
                }
                else if (Pro.compMove.direction == Direction.Up)
                {   //place centered above casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.Center.X,
                        Pro.caster.compCollision.rec.Y - 8);
                }
                else if (Pro.compMove.direction == Direction.Right)
                {   //place centered right side of casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.X + Pro.caster.compCollision.rec.Width + 8,
                        Pro.caster.compCollision.rec.Center.Y);
                }
                else if (Pro.compMove.direction == Direction.Left)
                {   //place centered left side of casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.X - 8,
                        Pro.caster.compCollision.rec.Center.Y);
                }
            }

            #endregion


            #region Sword, Net, Shovel, Fire/Ice Rods, Wand

            //sword, net, shovel all track to the HERO'S HAND, based on direction
            else if (Pro.type == ProjectileType.Sword
                || Pro.type == ProjectileType.Net
                || Pro.type == ProjectileType.Shovel
                || Pro.type == ProjectileType.Firerod
                || Pro.type == ProjectileType.Icerod
                || Pro.type == ProjectileType.Wand
                )
            {
                if (Pro.compMove.direction == Direction.Down)
                {   //place inhand below casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.X - 1 + 8 - 2,
                        Pro.caster.compCollision.rec.Y + Pro.caster.compCollision.rec.Height + 8);
                }
                else if (Pro.compMove.direction == Direction.Up)
                {   //place inhand above casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.X + 1 + 8 - 2,
                        Pro.caster.compCollision.rec.Y - 14);
                }
                else if (Pro.compMove.direction == Direction.Right)
                {   //place inhand right side of casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.X + Pro.caster.compCollision.rec.Width + 9,
                        Pro.caster.compCollision.rec.Y + 0);
                }
                else if (Pro.compMove.direction == Direction.Left)
                {   //place inhand left side of casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.X - 9,
                        Pro.caster.compCollision.rec.Y + 0);
                }
            }

            #endregion


            #region Bow 

            else if (Pro.type == ProjectileType.Bow)
            {
                //bows appear to be in links/blobs hands (other actors dont matter)
                //move projectile outside of caster's hitbox based on direction
                if (Pro.compMove.direction == Direction.Down)
                {   //place centered below casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.Center.X,
                        Pro.caster.compCollision.rec.Y + Pro.caster.compCollision.rec.Height + 1);
                }
                else if (Pro.compMove.direction == Direction.Up)
                {   //place centered above casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.Center.X,
                        Pro.caster.compCollision.rec.Y - 7);
                }
                else if (Pro.compMove.direction == Direction.Right)
                {   //place centered right side of casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.X + Pro.caster.compCollision.rec.Width + 1,
                        Pro.caster.compCollision.rec.Center.Y);
                }
                else if (Pro.compMove.direction == Direction.Left)
                {   //place centered left side of casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.X - 1,
                        Pro.caster.compCollision.rec.Center.Y);
                }
            }

            #endregion


            #region Hammer

            else if (Pro.type == ProjectileType.Hammer)
            {   //track hammer to the hand of the caster
                if (Pro.compMove.direction == Direction.Down)
                {   //place inhand below casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.X + 12,
                        Pro.caster.compCollision.rec.Y + Pro.caster.compCollision.rec.Height + 7);
                }
                else if (Pro.compMove.direction == Direction.Up)
                {   //place inhand above casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.X + 12,
                        Pro.caster.compCollision.rec.Y - 13);
                }
                else if (Pro.compMove.direction == Direction.Right)
                {   //place inhand right side of casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.X + Pro.caster.compCollision.rec.Width + 6,
                        Pro.caster.compCollision.rec.Y + 1);

                }
                else if (Pro.compMove.direction == Direction.Left)
                {   //place inhand left side of casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.X - 6,
                        Pro.caster.compCollision.rec.Y + 1);

                }


                if (Pro.lifeCounter == 12)
                {   //create random impact fx post hammer down frame
                    if (Pro.compMove.direction == Direction.Right)
                    {
                        Functions_Particle.Spawn(ParticleType.Blast,
                                Pro.compSprite.position.X + 4,
                                Pro.compSprite.position.Y + 3);
                    }
                    else if (Pro.compMove.direction == Direction.Left)
                    {
                        Functions_Particle.Spawn(ParticleType.Blast,
                                Pro.compSprite.position.X - 2,
                                Pro.compSprite.position.Y + 3);
                    }
                    else
                    {   //up/down
                        Functions_Particle.Spawn(ParticleType.Blast,
                                Pro.compSprite.position.X - 2,
                                Pro.compSprite.position.Y + 0);
                    }
                }
            }

            #endregion


            #region Carried Object Projectile (hero specific)

            else if (Pro.type == ProjectileType.CarriedObject)
            {
                //the carried obj is only used by hero, so it lives in func_hero class
                //it's aligned to hero's head just prior to drawing it, so it doesn't
                //lag behind and always draws ontop of hero.. it's behavior here is
                //simply to keep it alive forever - over hero's head - until player
                //throws() it or is forced to drop it by their actions (like pit fall)

                Pro.lifeCounter = 10; 
                Pro.active = true;
                //just lives forever, until thrown()
            }

            #endregion






            //projectiles that track to hit actors/objects

            #region Arrow

            if (Pro.type == ProjectileType.Arrow)
            {   //3 branches: one for no hit obj/act, hit act, hit obj
                if (Pro.hitActor != null)
                {   //track to hitActor with proper offset
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.hitActor.compMove.position.X + Pro.hitOffsetX,
                        Pro.hitActor.compMove.position.Y + Pro.hitOffsetY);
                    //if hitactor dies/goes invis, kill arrow
                    if (Pro.hitActor.compSprite.visible == false)
                    { Kill(Pro); }
                }
                else if (Pro.hitObj != null)
                {   //track to hitObj with proper offset
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.hitObj.compMove.position.X + Pro.hitOffsetX,
                        Pro.hitObj.compMove.position.Y + Pro.hitOffsetY);
                }
                //else { } //nothing, continue flying through air
            }

            #endregion






            //fire and forget projectiles

            #region Boomerang

            else if (Pro.type == ProjectileType.Boomerang)
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

                if (Functions_Random.Int(0, 101) > 95)
                {   //often place randomly offset sparkles
                    Functions_Particle.Spawn(ParticleType.Sparkle,
                        Pro.compSprite.position.X + Functions_Random.Int(-2, 2),
                        Pro.compSprite.position.Y + Functions_Random.Int(-2, 2));
                }

                #endregion

            }

            #endregion


            #region Fireballs

            else if (Pro.type == ProjectileType.Fireball)
            {
                //Debug.WriteLine("processing fireball behavior");
                if (Functions_Random.Int(0, 101) > 50)
                {   //often place randomly offset rising smoke
                    Functions_Particle.Spawn(ParticleType.RisingSmoke,
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
                                //this allow for cascade spreading, where a single fireball
                                //touches several of these objects in sequence as it travels
                                //this creates a cascade effect, handled here - keep this in mind
                                Pool.roomObjPool[i].compMove.direction = Pro.compMove.direction;
                                Functions_GameObject_World.Burn(Pool.roomObjPool[i]); //burn
                                Functions_GameObject_World.Bounce(Pool.roomObjPool[i]); //weak damage while in air
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





            #region Iceballs

            else if (Pro.type == ProjectileType.Iceball)
            {   //Debug.WriteLine("processing iceball behavior");
                //ball is flying, sparkle
                if (Functions_Random.Int(0, 101) > 75)
                {
                    Functions_Particle.Spawn(ParticleType.SparkleBlue,
                        Pro.compSprite.position.X + 4 + Functions_Random.Int(-4, 4),
                        Pro.compSprite.position.Y + 4 + Functions_Random.Int(-4, 4));
                }
            }

            #endregion


            #region Iceblocks

            else if(Pro.type == ProjectileType.Iceblock)
            {   //slow actor / objects friction, so it cant move
                if (Pro.hitActor != null)
                {   //slow actor
                    Pro.hitActor.compMove.friction = World.friction_Max; //max friction
                    //center track to hit actor
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.hitActor.compMove.position.X + 0,
                        Pro.hitActor.compMove.position.Y + 0);
                    //if hitactor dies/goes invis, kill crack block
                    if (Pro.hitActor.compSprite.visible == false) { Kill(Pro); }
                }
                else if (Pro.hitObj != null)
                {   //slow obj
                    Pro.hitObj.compMove.friction = World.friction_Max; //max friction
                    //center track to hit obj
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.hitObj.compMove.position.X + 0,
                        Pro.hitObj.compMove.position.Y + 0);
                    //if hitactor dies/goes invis, kill crack block
                    if (Pro.hitObj.compSprite.visible == false) { Kill(Pro); }
                }
                else
                {
                    //if iceblock has no target to slow, turn into cracking
                    if (Pro.type == ProjectileType.Iceblock) { Kill(Pro); }
                }
            }
            else if(Pro.type == ProjectileType.IceblockCracking)
            {   //slow actor / objects friction, so it cant move
                if (Pro.hitActor != null)
                {   //slow actor
                    Pro.hitActor.compMove.friction = World.friction_Max; //max friction
                    //center track to hit actor
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.hitActor.compMove.position.X + 0,
                        Pro.hitActor.compMove.position.Y + 0);
                }
                else if (Pro.hitObj != null)
                {   //slow obj
                    Pro.hitObj.compMove.friction = World.friction_Max; //max friction
                    //center track to hit obj
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.hitObj.compMove.position.X + 0,
                        Pro.hitObj.compMove.position.Y + 0);
                }
            }

            #endregion








            //world projectiles

            #region Ground Fire

            else if (Pro.type == ProjectileType.GroundFire)
            {
                if (Functions_Random.Int(0, 101) > 86)
                {   //often place randomly offset rising smoke
                    Functions_Particle.Spawn(ParticleType.RisingSmoke,
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
                else if (Pro.lifeCounter == Pro.interactiveFrame + 2)
                {   //set collision rec back to normal on 3rd frame
                    Pro.compCollision.offsetX = -4; Pro.compCollision.offsetY = -4;
                    Pro.compCollision.rec.Width = 8; Pro.compCollision.rec.Height = 8;
                }
            }

            #endregion










            //align all the components
            Functions_Component.Align(Pro.compMove, Pro.compSprite, Pro.compCollision);
        }

        //death events
        public static void Kill(Projectile Pro)
        {


            #region Arrow

            if (Pro.type == ProjectileType.Arrow)
            {
                //nothing, just disappear
            }

            #endregion


            #region Bat

            else if (Pro.type == ProjectileType.Bat)
            {
                Functions_Particle.Spawn(
                    ParticleType.Attention,
                    Pro.compSprite.position.X + 0,
                    Pro.compSprite.position.Y + 0);
                Assets.Play(Assets.sfxRatSqueak);
            }

            #endregion


            #region Bomb

            else if (Pro.type == ProjectileType.Bomb)
            {
                //create explosion projectile + ground fire
                Spawn(ProjectileType.Explosion, Pro.compMove.position.X, Pro.compMove.position.Y, Direction.None);
                //create groundfire
                Spawn(ProjectileType.GroundFire, Pro.compMove.position.X, Pro.compMove.position.Y, Direction.None);
            }

            #endregion


            #region Fireball

            else if (Pro.type == ProjectileType.Fireball)
            {   //create explosion
                Spawn(ProjectileType.Explosion, Pro.compMove.position.X, Pro.compMove.position.Y, Direction.None);
                //create groundfire
                Spawn(ProjectileType.GroundFire, Pro.compMove.position.X, Pro.compMove.position.Y, Direction.None);
            }

            #endregion


            #region Iceball

            else if (Pro.type == ProjectileType.Iceball)
            {   //create ice block projectile
                Spawn(ProjectileType.Iceblock, Pro.compMove.position.X, Pro.compMove.position.Y, Direction.None);
            }

            #endregion


            #region Iceblock

            else if (Pro.type == ProjectileType.Iceblock)
            {   //become cracking ice block
                SetType(Pro, ProjectileType.IceblockCracking); //maintain hit actor/obj refs while cracking
                return; //prevent release() + sfx
            }
            else if (Pro.type == ProjectileType.IceblockCracking)
            {   //casts freeze ground upon cracking, with attention particle
                Functions_Particle.Spawn(ParticleType.Attention, Pro.compMove.position.X, Pro.compMove.position.Y);
                Cast_FreezeGround(Pro.compSprite.position);
            }

            #endregion


            #region Thrown Objs

            else if (Pro.type == ProjectileType.ThrownObject)
            {
                Functions_Loot.SpawnLoot(Pro.compSprite.position);


                #region Explode as Bush

                if(
                    Pro.compAnim.currentAnimation == AnimationFrames.World_Bush
                    )
                {   //pop leaf explosion for bushes
                    Functions_Particle.Spawn_Explosion(
                        ParticleType.LeafGreen,
                        Pro.compSprite.position.X +4,
                        Pro.compSprite.position.Y +4,
                        true);
                    Assets.Play(Assets.sfxBushCut);
                }

                #endregion


                #region Explode as Thrown Enemy

                else if(
                    Pro.compAnim.currentAnimation == AnimationFrames.Wor_Enemy_Crab
                    || Pro.compAnim.currentAnimation == AnimationFrames.Wor_Enemy_Turtle
                    || Pro.compAnim.currentAnimation == AnimationFrames.Wor_Enemy_Rat_Down
                    )
                {   //create floor blood, blood explosion, maybe skeleton
                    Functions_GameObject_Dungeon.DecorateEnemyDeath(Pro.compSprite);
                }   //we release pro at end of method

                #endregion


                #region Explode as Anything Else (pot, skull, etc..)

                else
                {   //pop rock debris for all other thrown objs
                    Functions_Particle.Spawn_Explosion(
                        ParticleType.DebrisBrown,
                        Pro.compSprite.position.X+4,
                        Pro.compSprite.position.Y+4,
                        true);
                    Assets.Play(Assets.sfxShatter);
                }

                #endregion

            }

            #endregion





            //all objects are released upon death
            if (Pro.sfx.kill != null) { Assets.Play(Pro.sfx.kill); }
            Functions_Pool.Release(Pro);
        }









        //special hero-only magic casting methods

        static int castCounter = 0;
        static int totalCount = 0;
        static int counterB = 0;
        public static List<Vector2> castPositions;

        public static void Cast_Ether()
        {
            //for each active enemy actor and roomObj
            //create a lightning bolt particle - up from their position
            //do this many times, so we have bolts going up from all enemies
            //then create explosions at enemy locations

            castPositions = new List<Vector2>();

            //collect enemy actor positions
            for(i = 0; i < Pool.actorCount; i++)
            {
                if (Pool.actorPool[i].active)
                {
                    if(Pool.actorPool[i].enemy)
                    { castPositions.Add(Pool.actorPool[i].compSprite.position); }
                }
            }
            //collect enemy object positions
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active)
                {
                    if (Pool.roomObjPool[i].group == ObjGroup.Enemy)
                    { castPositions.Add(Pool.roomObjPool[i].compSprite.position); }
                }
            }

            //loop all collected positions, creating explosions and bolts
            totalCount = castPositions.Count;
            //spawn bolts + explosions at all positions
            for (castCounter = 0; castCounter < totalCount; castCounter++)
            {   //explosions to damage actors/objs
                Spawn(ProjectileType.Explosion,
                    castPositions[castCounter].X,
                    castPositions[castCounter].Y,
                    Direction.None);
                for (counterB = 1; counterB < 5; counterB++)
                {   //place bolts centered, going up from position, for decor
                    Functions_Particle.Spawn(ParticleType.LightningBolt,
                        castPositions[castCounter].X,
                        (castPositions[castCounter].Y + 4) - (16 * counterB),
                        Direction.Down);
                }
            }
            Assets.Play(Assets.sfxShock); //play sfx
        }


        static int bombosSpread = 22;
        public static void Cast_Bombos()
        {
            //create explosions around hero
            //-this could look alot better, if we used 5 point or 7 point circle
            //instead of square 3x3 setup


            #region Cardinals

            //up
            Spawn(ProjectileType.Explosion,
                Pool.hero.compMove.position.X,
                Pool.hero.compMove.position.Y - bombosSpread,
                Direction.Down);
            //down
            Spawn(ProjectileType.Explosion,
                Pool.hero.compMove.position.X,
                Pool.hero.compMove.position.Y + bombosSpread,
                Direction.Down);
            //left
            Spawn(ProjectileType.Explosion,
                Pool.hero.compMove.position.X - bombosSpread,
                Pool.hero.compMove.position.Y,
                Direction.Down);
            //down
            Spawn(ProjectileType.Explosion,
                Pool.hero.compMove.position.X + bombosSpread,
                Pool.hero.compMove.position.Y,
                Direction.Down);

            #endregion


            #region Diagonals

            //up R/L
            Spawn(ProjectileType.Explosion,
                Pool.hero.compMove.position.X - bombosSpread,
                Pool.hero.compMove.position.Y - bombosSpread,
                Direction.Down);
            Spawn(ProjectileType.Explosion,
                Pool.hero.compMove.position.X + bombosSpread,
                Pool.hero.compMove.position.Y - bombosSpread,
                Direction.Down);

            //down R/L
            Spawn(ProjectileType.Explosion,
                Pool.hero.compMove.position.X - bombosSpread,
                Pool.hero.compMove.position.Y + bombosSpread,
                Direction.Down);
            Spawn(ProjectileType.Explosion,
                Pool.hero.compMove.position.X + bombosSpread,
                Pool.hero.compMove.position.Y + bombosSpread,
                Direction.Down);

            #endregion


            Assets.Play(Assets.sfxExplosionsMultiple); //play sfx
        }



        


        static int iceCounter = 0;
        static GameObject iceTile;
        static Vector2 icePosRef = new Vector2();
        public static void Cast_FreezeGround(Vector2 pos)
        {   //align pos to grid
            pos = Functions_Movement.AlignToGrid((int)pos.X, (int)pos.Y);
            //place icetiles at pos, and NSEW + DIAG
            for (iceCounter = 0; iceCounter < 9; iceCounter++)
            {   //top
                if (iceCounter == 0) //top left
                { icePosRef.X = pos.X - 16; icePosRef.Y = pos.Y - 16; }
                else if (iceCounter == 1) //top mid
                { icePosRef.X = pos.X + 0; icePosRef.Y = pos.Y - 16; }
                else if (iceCounter == 2) //top rite
                { icePosRef.X = pos.X + 16; icePosRef.Y = pos.Y - 16; }
                //mid
                else if (iceCounter == 3) //left
                { icePosRef.X = pos.X - 16; icePosRef.Y = pos.Y + 0; }
                else if (iceCounter == 4) //mid
                { icePosRef.X = pos.X + 0; icePosRef.Y = pos.Y + 0; }
                else if (iceCounter == 5) //rite
                { icePosRef.X = pos.X + 16; icePosRef.Y = pos.Y + 0; }
                //bottom
                else if (iceCounter == 6) //bot left
                { icePosRef.X = pos.X - 16; icePosRef.Y = pos.Y + 16; }
                else if (iceCounter == 7) //bot mid
                { icePosRef.X = pos.X + 0; icePosRef.Y = pos.Y + 16; }
                else if (iceCounter == 8) //bot rite
                { icePosRef.X = pos.X + 16; icePosRef.Y = pos.Y + 16; }

                //place ice tile at offset
                iceTile = Functions_Pool.GetRoomObj();
                Functions_GameObject.SetType(iceTile, ObjType.Dungeon_IceTile);
                Functions_Movement.Teleport(iceTile.compMove, icePosRef.X, icePosRef.Y);
                Functions_Component.Align(iceTile);
                //note ice tile birth with attention particle
                Functions_Particle.Spawn(ParticleType.Attention,
                    icePosRef.X, icePosRef.Y);
            }
        }










        /*
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
                Spawn(ProjectileType.LightningBolt,
                    Caster.compSprite.position.X + offset.X * i,
                    Caster.compSprite.position.Y + offset.Y * i,
                    Caster.direction);
            }
        }
        */





        public static void SetRotation(Projectile Pro)
        {
            //some pros rotate to appear in casters hand
            if (
                Pro.type == ProjectileType.Sword
                || Pro.type == ProjectileType.Net
                || Pro.type == ProjectileType.Shovel
                || Pro.type == ProjectileType.Firerod
                || Pro.type == ProjectileType.Icerod
                || Pro.type == ProjectileType.Wand
                )
            {   //some projectiles flip based on their direction
                if (Pro.direction == Direction.Down || Pro.direction == Direction.Left)
                { Pro.compSprite.flipHorizontally = true; }
            }

            //some pros only face Direction.Down, with no rotation
            else if (
                Pro.type == ProjectileType.Bomb
                || Pro.type == ProjectileType.Boomerang
                || Pro.type == ProjectileType.Bat
                || Pro.type == ProjectileType.Hammer
                //ice blocks too
                || Pro.type == ProjectileType.Iceblock
                || Pro.type == ProjectileType.IceblockCracking
                )
            {   
                Pro.direction = Direction.Down;
                Pro.compSprite.rotation = Rotation.None;
            }



            /*
            //other pros rotate to form chains (HOOKOSHOT!!!)
            else if (
                Pro.type == ProjectileType.LightningBolt
                )
            {   //align lightning bolts vertically or horizontally
                if (Pro.direction == Direction.Left || Pro.direction == Direction.Right)
                { Pro.compSprite.rotation = Rotation.Clockwise90; }
                else { Pro.compSprite.rotation = Rotation.None; }
            }
            */


            //finally, set sprite's rotation based on direction & flipHorizontally boolean
            Functions_Component.SetSpriteRotation(Pro.compSprite, Pro.direction);
        }

        public static void SetArrowHitState(Projectile Arrow)
        {   //limit arrow life, swamp to hit sprite, change zdepth offset
            Arrow.lifeCounter = 0; //reset lifecounter
            Arrow.lifetime = 60*3; //3 seconds
            Arrow.compAnim.currentAnimation = AnimationFrames.Projectile_ArrowHit;
            Arrow.compSprite.zOffset = 64; //sort over most things
            //pop an attention particle to signify state change to player
            Functions_Particle.Spawn(
                ParticleType.Attention,
                Arrow.compSprite.position.X + 0,
                Arrow.compSprite.position.Y + 0);
        }







        
        //maps type to object values/state
        public static void SetType(Projectile Pro, ProjectileType Type)
        {
            Pro.type = Type;
            Pro.compSprite.texture = Assets.entitiesSheet;






            
            //Projectiles

            #region Bomb, Boomerang

            if (Type == ProjectileType.Bomb)
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
            else if (Type == ProjectileType.Boomerang)
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


            #region Fireball

            else if (Type == ProjectileType.Fireball)
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

            #endregion





            #region Iceball

            else if (Type == ProjectileType.Iceball)
            {
                Pro.compSprite.zOffset = 64;
                Pro.compCollision.offsetX = -5; Pro.compCollision.offsetY = -5;
                Pro.compCollision.rec.Width = 10; Pro.compCollision.rec.Height = 10;
                Pro.lifeCounter = 0;
                Pro.lifetime = 50; //in frames
                Pro.compMove.friction = World.frictionIce;
                Pro.compAnim.speed = 2; //in frames
                Pro.compMove.moveable = true;
                Pro.compMove.grounded = false; //obj is airborne
                Pro.compAnim.currentAnimation = AnimationFrames.Projectile_Iceball;
                Pro.compSprite.texture = Assets.entitiesSheet;
            }

            #endregion


            #region Iceblock + Cracking State

            else if (Type == ProjectileType.Iceblock)
            {
                Pro.compSprite.zOffset = 8;
                Pro.compCollision.offsetX = -5; Pro.compCollision.offsetY = -5;
                Pro.compCollision.rec.Width = 10; Pro.compCollision.rec.Height = 10;
                Pro.lifeCounter = 0;
                Pro.lifetime = 250; //in frames
                Pro.compAnim.speed = 5; //in frames
                Pro.compMove.moveable = false;
                Pro.compMove.grounded = false; //obj is airborne
                Pro.compAnim.currentAnimation = AnimationFrames.Projectile_Iceblock;
                Pro.compMove.friction = World.friction_Max; //very heavy
            }
            else if (Type == ProjectileType.IceblockCracking)
            {
                Pro.compSprite.zOffset = 32;
                Pro.compCollision.offsetX = -5; Pro.compCollision.offsetY = -5;
                Pro.compCollision.rec.Width = 10; Pro.compCollision.rec.Height = 10;
                Pro.lifeCounter = 0;
                Pro.lifetime = 45; //in frames
                Pro.compAnim.speed = 4; //in frames
                Pro.compMove.moveable = false;
                Pro.compMove.grounded = false; //obj is airborne
                Pro.compAnim.currentAnimation = AnimationFrames.Projectile_IceblockCracking;
                Pro.compMove.friction = World.friction_Max; //very heavy
            }

            #endregion









            //Projectiles - Weapons

            #region Sword

            else if (Type == ProjectileType.Sword)
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

            else if (Type == ProjectileType.Shovel)
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

            else if (Type == ProjectileType.Hammer)
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
                
                Pro.lifetime = 17; //in frames
                Pro.compAnim.speed = 2; //in frames
                Pro.compAnim.loop = false;
                Pro.compMove.moveable = true;
                Pro.compMove.grounded = false; //is flying, cant fall into pit
                Pro.compSprite.texture = Assets.entitiesSheet;
                Pro.compAnim.currentAnimation = AnimationFrames.Projectile_Hammer_Down;
            }

            #endregion


            #region Arrow

            else if (Type == ProjectileType.Arrow)
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

            #endregion


            #region Bow

            else if (Type == ProjectileType.Bow)
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

            else if (Type == ProjectileType.Net)
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


            #region Fire/Ice Rods

            else if (Type == ProjectileType.Firerod || Type == ProjectileType.Icerod)
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
                Pro.compAnim.speed = 3; //in frames
                Pro.compAnim.loop = false;
                Pro.compMove.moveable = true;
                Pro.compMove.grounded = false; //is flying, cant fall into pit
                Pro.compSprite.texture = Assets.entitiesSheet;
                if (Type == ProjectileType.Firerod)
                { Pro.compAnim.currentAnimation = AnimationFrames.Projectile_FireRod; }
                else { Pro.compAnim.currentAnimation = AnimationFrames.Projectile_IceRod; }
                
            }

            #endregion


            #region Explosions and Ground Fires

            else if (Type == ProjectileType.Explosion)
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
            else if (Type == ProjectileType.GroundFire)
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


            #region Fang/Bite and Bat

            else if (Type == ProjectileType.Bite)
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

            else if (Type == ProjectileType.Bat)
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


            #region Wand

            else if (Type == ProjectileType.Wand)
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
                Pro.compAnim.currentAnimation = AnimationFrames.Projectile_Wand;
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