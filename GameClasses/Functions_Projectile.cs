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
        //Dir is usually the actor's / object's facing direction
        public static void Spawn(ObjType Type, ComponentMovement Caster, Direction Dir)
        {
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
            Projectile pro = Functions_Pool.GetProjectile();
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

            //give some projectiles an initial push
            if (Type == ObjType.ProjectileArrow) { Functions_Movement.Push(pro.compMove, Dir, 6.0f); }
            else if (Type == ObjType.ProjectileFireball) { Functions_Movement.Push(pro.compMove, Dir, 5.0f); }

            else if (Type == ObjType.ProjectileBoomerang)
            { Functions_Movement.Push(pro.compMove, Dir, 5.0f); Functions_Hero.boomerangInPlay = true; }

            else if (Type == ObjType.ProjectileBomb) { Functions_Movement.Push(pro.compMove, Dir, 5.0f); }
            else if (Type == ObjType.ProjectileExplodingBarrel) { Functions_Movement.Push(pro.compMove, Dir, 6.0f); }
            //assume this projectile is moving
            pro.compMove.moving = true;


            #region Handle Spawn Events not handled by BirthEvent

            if (Type == ObjType.ProjectileNet)
            {   //it's done this way cause the net may create 
                //a dialog screen on the next frame, which would
                //prevent the net's birthEvent from trigerring,
                //because that happens on the net's 3rd frame alive
                Assets.Play(Assets.sfxNet);
            }

            #endregion


            //teleport the object to the caster's location
            Functions_Movement.Teleport(pro.compMove,
                Caster.position.X,
                Caster.position.Y
            );

            //handle spawn frame behavior
            pro.type = Type;
            Update(pro); 

            //finalize it: setType, rotation & align
            Functions_GameObject.SetType(pro, Type); 
        }

        static Vector2 offset = new Vector2();
        public static void Update(Projectile Pro)
        {
            //this method handles the behaviors of a projectile, by modifying it's newPosition
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


            #region Arrow & Fireballs

            else if(Pro.type == ObjType.ProjectileFireball
                || Pro.type == ObjType.ProjectileArrow)
            {   //prevent caster from overlapping with projectile
                //step 1: set minimum safe distance from caster (offset)
                if (Pro.direction == Direction.Down) { offset.X = 0; offset.Y = +16; }
                else if (Pro.direction == Direction.Up) { offset.X = 0; offset.Y = -16; }
                else if (Pro.direction == Direction.Right) { offset.X = +15; offset.Y = +2; }
                else if (Pro.direction == Direction.Left) { offset.X = -15; offset.Y = +2; }
                //step 2: apply offset to prevent projectile overlapping with caster, using direction
                if (Pro.direction == Direction.Down)
                {   //apply Y offset
                    if (Pro.compMove.newPosition.Y < Pro.caster.newPosition.Y + offset.Y)
                    { Pro.compMove.newPosition.Y = Pro.caster.newPosition.Y + offset.Y; }
                }
                else if(Pro.direction == Direction.Up)
                {   //apply Y offset
                    if (Pro.compMove.newPosition.Y > Pro.caster.newPosition.Y + offset.Y)
                    { Pro.compMove.newPosition.Y = Pro.caster.newPosition.Y + offset.Y; }
                }
                else if(Pro.direction == Direction.Left)
                {   //apply X offset
                    if (Pro.compMove.newPosition.X > Pro.caster.newPosition.X + offset.X)
                    { Pro.compMove.newPosition.X = Pro.caster.newPosition.X + offset.X; }
                }
                else if (Pro.direction == Direction.Right)
                {   //apply X offset
                    if (Pro.compMove.newPosition.X < Pro.caster.newPosition.X + offset.X)
                    { Pro.compMove.newPosition.X = Pro.caster.newPosition.X + offset.X; }
                }
            }

            #endregion


            #region Boomerang

            else if(Pro.type == ObjType.ProjectileBoomerang)
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
                        Functions_GameObject.Kill(Pro);
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





            #region Ideas
            /*

            based on the behavior of the current projectiles..
            fireball magic: spawns relative to caster position with direction, travels in direction, explodes
            arrow: same as fireball, doesn't explode, travels faster
            bomb: spawns relative to caster, slides for a moment before stopping
            sword: spawns relative to caster, tracks to caster until animation completes
            net: same as sword
            explosion: usually relative to caster (as an effect), stationary
            explodingBarrel: caster is barrel, slides in a direction away from barrel, explodes


            spikeBlock: this is a projectile? why?
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



            ideas:
            remote detonated mines - must equip two items: mines + detonator
                mines are dropped with Y button
                detonator blows up ALL dropped mines with X button
                *this way the player can juggle / strategize more, with faster reaction time
                * *plus, no switching between items to blow stuff up


            */
            #endregion




            //teleport the projectile to it's new position
            Functions_Movement.Teleport(Pro.compMove,
                Pro.compMove.newPosition.X,
                Pro.compMove.newPosition.Y);
        }

    }
}