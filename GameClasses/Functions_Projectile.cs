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





        //these methods handle update and death of projectiles, birth events are in spawn()
        public static void Update(Projectile Pro)
        {   //projectiles have lifetimes
            Pro.lifeCounter++;
            HandleBehavior(Pro);
            if (Pro.lifeCounter >= Pro.lifetime) { Kill(Pro); }
        }

        public static void Kill(GameObject Obj)
        {
            //contains death events for projectiles


            if (Obj.type == ObjType.ProjectileArrow
                || Obj.type == ObjType.ProjectileBat)
            {
                Functions_Particle.Spawn(
                    ObjType.Particle_Attention,
                    Obj.compSprite.position.X + 0,
                    Obj.compSprite.position.Y + 0);
            }

            else if (Obj.type == ObjType.ProjectileBomb)
            {
                //create explosion projectile + ground fire
                Spawn(ObjType.ProjectileExplosion, Obj.compMove.position.X, Obj.compMove.position.Y, Direction.None);
                //create groundfire
                Spawn(ObjType.ProjectileGroundFire, Obj.compMove.position.X, Obj.compMove.position.Y, Direction.None);
            }
            else if (Obj.type == ObjType.ProjectileFireball)
            {   //create explosion
                Spawn(ObjType.ProjectileExplosion, Obj.compMove.position.X, Obj.compMove.position.Y, Direction.None);
                //create groundfire
                Spawn(ObjType.ProjectileGroundFire, Obj.compMove.position.X, Obj.compMove.position.Y, Direction.None);
            }



            #region Thrown Objs

            else if (Obj.type == ObjType.ProjectileBush)
            {
                Functions_Loot.SpawnLoot(Obj.compSprite.position);
                //pop leaves
                Functions_Particle.Spawn_Explosion(
                    ObjType.Particle_Leaf,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y,
                    true);
            }
            else if (Obj.type == ObjType.ProjectilePotSkull
                || Obj.type == ObjType.ProjectilePot)
            {
                Functions_Loot.SpawnLoot(Obj.compSprite.position);
                //pop debris
                Functions_Particle.Spawn_Explosion(
                    ObjType.Particle_Debris,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y,
                    true);
            }

            #endregion


            //all objects are released upon death
            if (Obj.sfx.kill != null) { Assets.Play(Obj.sfx.kill); }
            Functions_Pool.Release(Obj);
        }






        //this method is used for projectiles that dont have casters, like groundfires
        public static void Spawn(ObjType Type, float X, float Y, Direction Dir)
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
            Functions_GameObject.SetType(pro, Type);
            Functions_Component.Align(pro);


            #region Lightning Bolt

            //a bolt is spawned by other bolts, hence no caster
            if (Type == ObjType.ProjectileLightningBolt)
            {   //push bolt a little bit, play sfx
                Functions_Movement.Push(pro.compMove, Dir, 2.0f);
                Assets.Play(Assets.sfxShock);
            }

            #endregion


            #region Projectile Bomb

            //what room obj or event spawns a bomb?
            else if (Type == ObjType.ProjectileBomb)
            {
                Assets.Play(Assets.sfxBombDrop);
            }

            #endregion


            #region Fireball

            //flamethrowers spawn fireballs, etc..
            else if (Type == ObjType.ProjectileFireball)
            {
                Functions_Movement.Push(pro.compMove, Dir, 4.0f);
                Assets.Play(Assets.sfxFireballCast);
                pushLines = true;
            }

            #endregion


            HandleBehavior(pro);
        }
 

        //this method is used (more), and requires a caster for the projectile
        public static void Spawn(ObjType Type, Actor Caster, Direction Dir)
        {   //Dir is usually the actor's facing/input direction
            pushLines = false; //reset pushlines (only cardinal dirs)


            #region Boomerang Spawn - only one on screen at a time

            if (Type == ObjType.ProjectileBoomerang)
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
            if (Type == ObjType.ProjectileBomb
                || Type == ObjType.ProjectileBoomerang
                || Type == ObjType.ProjectileBat)
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
            Functions_GameObject.SetType(pro, Type);




            //these pros must be placed outside of their caster's hitbox initially
            //we just apply an initial force, and the obj's speed moves it until death

            #region Arrows and Fireballs

            //fireball may need a specific offset different from arrows, because they are larger

            if (Type == ObjType.ProjectileArrow || Type == ObjType.ProjectileFireball)
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

                if (Type == ObjType.ProjectileArrow)
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

            else if (Type == ObjType.ProjectileBat)
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

            else if (Type == ObjType.ProjectileBush
                || Type == ObjType.ProjectilePotSkull
                || Type == ObjType.ProjectilePot)
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

            else if (Type == ObjType.ProjectileBoomerang)
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

            #endregion



            
            //these projectiles track to their caster per frame, in HandleBehavior()

            #region Net

            else if (Type == ObjType.ProjectileNet)
            {
                Assets.Play(Assets.sfxNet);
            }

            #endregion


            #region Sword

            else if (Type == ObjType.ProjectileSword)
            {
                Assets.Play(Assets.sfxSwordSwipe);
            }

            #endregion


            #region Shovel

            else if (Type == ObjType.ProjectileShovel)
            {
                Assets.Play(Assets.sfxActorLand); //generic use sound
            }

            #endregion


            #region Hammer

            else if (Type == ObjType.ProjectileHammer)
            {
                Assets.Play(Assets.sfxActorLand); //generic use sound
            }

            #endregion


            #region Bite Projectile

            else if (Type == ObjType.ProjectileBite)
            {
                Assets.Play(Assets.sfxEnemyTaunt);
                pushLines = true;
            }

            #endregion



            //these projectiles dont track or care about their caster

            #region Explosions

            else if (Type == ObjType.ProjectileExplosion)
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

            else if (Type == ObjType.ProjectileBombos)
            {   //play casting soundfx
                Assets.Play(Assets.sfxCastBombos);
            }

            #endregion


            

            HandleBehavior(pro);
            if (pushLines) { Functions_Particle.SpawnPushFX(Caster.compMove, Dir); }
        }



        



        

        //this method handles the projectile's behavior each frame
        public static void HandleBehavior(Projectile Pro)
        {
            //the following paths handle the per frame events, or behaviors, of a projectile
            //for example, tracking a sword to it's caster or whatevs..



            //track to caster each frame

            #region Bite/Fang

            //fang/bite simply tracks outside of the casters hitbox, centered
            if (Pro.type == ObjType.ProjectileBite)
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


            #region Sword, Net, Shovel, Hammer

            //sword, net, shovel all track to the HERO'S HAND, based on direction
            else if (Pro.type == ObjType.ProjectileSword
                || Pro.type == ObjType.ProjectileNet
                || Pro.type == ObjType.ProjectileShovel
                || Pro.type == ObjType.ProjectileHammer
                )
            {   
                if (Pro.direction == Direction.Down)
                {   //place inhand below casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.X - 1+8-2,
                        Pro.caster.compCollision.rec.Y + Pro.caster.compCollision.rec.Height + 8);
                }
                else if (Pro.direction == Direction.Up)
                {   //place inhand above casters hb
                    Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.X + 1+8-2,
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

            else if (Pro.type == ObjType.ProjectileBow)
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





            //fire and forget projectiles

            #region Boomerang

            else if (Pro.type == ObjType.ProjectileBoomerang)
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

            else if (Pro.type == ObjType.ProjectileFireball)
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
                                        ObjType.ProjectileGroundFire,
                                        Pool.roomObjPool[i].compSprite.position.X,
                                        Pool.roomObjPool[i].compSprite.position.Y - 3,
                                        Direction.None);
                                }
                                //fireballs can cascade spread across bushes
                                else if (Pool.roomObjPool[i].type == ObjType.Wor_Bush)
                                {   //spread the fire 
                                    Functions_Projectile.Spawn(
                                        ObjType.ProjectileGroundFire,
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
                                        ObjType.ProjectileGroundFire,
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

            else if (Pro.type == ObjType.ProjectileBombos)
            {   //casted magic tracks over caster's head
                Functions_Movement.Teleport(Pro.compMove,
                        Pro.caster.compCollision.rec.X + 8,
                        Pro.caster.compCollision.rec.Y - 12);

                if (Functions_Random.Int(0, 100) > 40)
                {   //create a bomb each frame, for a wide area around caster
                    Spawn(ObjType.ProjectileBomb,
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

            else if (Pro.type == ObjType.ProjectileGroundFire)
            {
                if (Functions_Random.Int(0, 101) > 86)
                {   //often place randomly offset rising smoke
                    Functions_Particle.Spawn(ObjType.Particle_RisingSmoke,
                        Pro.compSprite.position.X + 5 + Functions_Random.Int(-4, 4),
                        Pro.compSprite.position.Y + 1 + Functions_Random.Int(-8, 2));
                }
                //expand groundfires hitbox to cause interactions with room objs
                if(Pro.lifeCounter == Pro.interactiveFrame)
                {   //spread horizontally on interaction frame
                    Pro.compCollision.offsetX = -17; Pro.compCollision.rec.Width = 34;
                    Pro.compCollision.offsetY = -4; Pro.compCollision.rec.Height = 8;
                }
                else if(Pro.lifeCounter == Pro.interactiveFrame + 1)
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



        


        
        //lightning is chain of bolts, each with offset, moving same
        public static void Cast_Bolt(Actor Caster)
        {
            //create a series of bolts in the facing direction of caster

            //resolve caster's direction to a cardinal one
            Caster.direction = Functions_Direction.GetCardinalDirection_LeftRight(Caster.direction);
            //setup offsets based on resolved caster's direction
            if(Caster.direction == Direction.Up) { offset.X = 0; offset.Y = -16; }
            else if (Caster.direction == Direction.Right) { offset.X = +16; offset.Y = 0; }
            else if (Caster.direction == Direction.Down) { offset.X = 0; offset.Y = +16; }
            else if (Caster.direction == Direction.Left) { offset.X = -16; offset.Y = 0; }

            //using the offset, loop create explosions, multiplying offset
            for(i = 1; i < 8; i++)
            {
                Spawn(ObjType.ProjectileLightningBolt,
                    Caster.compSprite.position.X + offset.X * i,
                    Caster.compSprite.position.Y + offset.Y * i,
                    Caster.direction);
            }
        }


        






    }
}