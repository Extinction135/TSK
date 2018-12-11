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
    public static class Functions_Wind
    {
        static int i;
        static WindObject windRef;
        static Point spawnPt;






        //resets the projectile to a default state
        public static void Reset(WindObject Wind)
        {   //reset object to GUST
            Wind.type = WindType.Gust; //reset the type
            Wind.direction = Direction.Down;
            Wind.active = true; //assume this object should draw / animate
            Wind.lifeTotal = 255; //live for this long
            Wind.lifeCounter = 0;

            //reset the sprite component
            Wind.compSprite.drawRec.Width = 16 * 1; //assume cell size is 16x16 (most are)
            Wind.compSprite.drawRec.Height = 16 * 1;
            Wind.compSprite.zOffset = 1024; //wind is globally over everything
            Wind.compSprite.flipHorizontally = false;
            Wind.compSprite.rotation = Rotation.None;
            Wind.compSprite.scale = 1.0f;
            Wind.compSprite.texture = Assets.entitiesSheet;
            Wind.compSprite.visible = true;

            //reset the animation component
            Wind.compAnim.speed = 10; //set obj's animation speed to default value
            Wind.compAnim.loop = true; //assume obj's animation loops
            Wind.compAnim.index = 0; //reset the current animation index/frame
            Wind.compAnim.timer = 0; //reset the elapsed frames
            Wind.compAnim.currentAnimation = AnimationFrames.Wind_Gust;

            //reset the collision component
            Wind.compCollision.blocking = false;
            Wind.compCollision.rec.Width = 16; //assume collisionRec is 16x16
            Wind.compCollision.rec.Height = 16; //(most are)
            Wind.compCollision.offsetX = -8; //assume collisionRec offset is -8x-8
            Wind.compCollision.offsetY = -8; //(most are)

            //reset the move component
            Wind.compMove.magnitude.X = 0; //discard any previous magnitude
            Wind.compMove.magnitude.Y = 0; //
            Wind.compMove.speed = 0.03f; //wind moves
            Wind.compMove.friction = 0.99f; //no friction
            Wind.compMove.moveable = false; //unused
            Wind.compMove.grounded = false; //unused
        }



        public static void SetRotation(WindObject Wind)
        {   //rotate wind sprite to moving direction
            if (Wind.compMove.direction == Direction.Down)
            { Wind.compSprite.rotation = Rotation.None; }
            else if (Wind.compMove.direction == Direction.Left)
            { Wind.compSprite.rotation = Rotation.Clockwise90; }
            else if (Wind.compMove.direction == Direction.Up)
            { Wind.compSprite.rotation = Rotation.Clockwise180; }
            else if (Wind.compMove.direction == Direction.Right)
            { Wind.compSprite.rotation = Rotation.Clockwise270; }
            //update the sprites rotation
            Functions_Component.SetSpriteRotation(Wind.compSprite, Wind.direction);
        }



        static WindObject spawnRef;
        public static void Spawn(WindType Type, float X, float Y, Direction Dir)
        {
            spawnRef = Functions_Pool.GetWind();

            spawnRef.direction = Dir;
            spawnRef.compMove.direction = Dir;
            //put wind into position
            Functions_Movement.Teleport(spawnRef.compMove, X, Y);
            Functions_Component.Align(
                spawnRef.compMove,
                spawnRef.compSprite,
                spawnRef.compCollision);

            //define wind characteristics
            spawnRef.lifeTotal = 60; //1 second for gusts
            spawnRef.lifeCounter = 0;
            spawnRef.compMove.speed = 0.03f; //wind moves
            spawnRef.compMove.friction = 0.99f; //no friction
            Functions_Movement.Push(spawnRef.compMove, Dir, 7.0f);

            //prep sprite for display
            SetRotation(spawnRef);
            spawnRef.compCollision.blocking = false;
            spawnRef.compSprite.currentFrame = spawnRef.compAnim.currentAnimation[0]; //goto 1st anim frame
            Assets.Play(Assets.sfxArrowShoot); //to be replaced
        }
        






        //per-frame logic, after pool's interactions(), before projectMovement()
        public static void Update()
        {   
            //reset the wind interaction pool counters
            Pool.windInts_ThisFrame = 0;
            Pool.windInts_Possible = 0;


            #region Spawn Level Wind

            //spawn wind based on level values
            if(Functions_Random.Int(0, 101) < LevelSet.currentLevel.currentRoom.windFrequency)
            {
                //ensure level has a real wind direction (randomly set direction cardinal if direction = none)
                if (LevelSet.currentLevel.currentRoom.windDirection == Direction.None)
                { LevelSet.currentLevel.currentRoom.windDirection = Functions_Direction.GetRandomCardinal(); }
                //resolve any diagonal wind directions to cardinals, left/right preferred due to window ratio
                LevelSet.currentLevel.currentRoom.windDirection = 
                    Functions_Direction.GetCardinalDirection_LeftRight(LevelSet.currentLevel.currentRoom.windDirection);

                //grab a random screen position
                spawnPt.X = Functions_Random.Int(0, 640);
                spawnPt.Y = Functions_Random.Int(0, 360);
                //bias this position against the wind direction (so we dont spawn wind that goes off screen next frame)
                if(LevelSet.currentLevel.currentRoom.windDirection == Direction.Down)
                { spawnPt.Y -= 100; } //spawn more above, moving down
                else if (LevelSet.currentLevel.currentRoom.windDirection == Direction.Up)
                { spawnPt.Y += 100; } //spawn more below, moving up
                else if (LevelSet.currentLevel.currentRoom.windDirection == Direction.Left)
                { spawnPt.X += 100; } //spawn more right, moving left
                else if (LevelSet.currentLevel.currentRoom.windDirection == Direction.Right)
                { spawnPt.X -= 100; } //spawn more left, moving right

                //project spawnpoint down into gameworld & spawn
                spawnPt = Functions_Camera2D.ConvertScreenToWorld(spawnPt.X, spawnPt.Y);
                Spawn(WindType.Gust, spawnPt.X, spawnPt.Y,
                    LevelSet.currentLevel.currentRoom.windDirection);
            }

            #endregion


            #region Proces Wind Object Lifetime, Animations, and Interactions with Ints/Acts/Objs

            //loop over the pool.wind list, find active wind objs
            for (i = 0; i < Pool.windObjCount; i++)
            {
                if (Pool.windObjPool[i].active)
                {   //shorten ref
                    windRef = Pool.windObjPool[i]; 


                    //Death conditions

                    #region Count Lifetimes of Wind Objs

                    windRef.lifeCounter++;
                    if(windRef.lifeCounter > windRef.lifeTotal)
                    {
                        Kill(windRef);
                    }

                    #endregion


                    #region DIE against Indestructibles

                    for (Pool.indObjCounter = 0; Pool.indObjCounter < Pool.indObjCount; Pool.indObjCounter++)
                    {   //active check
                        if (Pool.indObjPool[Pool.indObjCounter].active)
                        {   //overlap check
                            Pool.windInts_Possible++; //possible interaction up next
                            if (windRef.compCollision.rec.Intersects(
                                Pool.indObjPool[Pool.indObjCounter].compCollision.rec))
                            {   //wind dies upon ind collisions
                                Pool.windInts_ThisFrame++; //count it
                                Kill(windRef);
                            }
                        }
                    }

                    #endregion



                    //if wind obj is inside of current level, then we move it and animate it
                    if (LevelSet.currentLevel.currentRoom.rec.Contains(windRef.compSprite.position))
                    {

                        #region Move and Animate Wind

                        //animate wind
                        Functions_Animation.Animate(windRef.compAnim, windRef.compSprite);
                        Functions_Animation.ScaleSpriteDown(windRef.compSprite);
                        //move wind
                        Functions_Movement.ProjectMovement(windRef.compMove);
                        //set position to the new position (projected pos)
                        windRef.compMove.position.X = windRef.compMove.newPosition.X;
                        windRef.compMove.position.Y = windRef.compMove.newPosition.Y;
                        //align wind components
                        Functions_Component.Align(
                            windRef.compMove,
                            windRef.compSprite,
                            windRef.compCollision);

                        #endregion


                        #region PUSH Interactive Objs

                        for (Pool.intObjCounter = 0; Pool.intObjCounter < Pool.intObjCount; Pool.intObjCounter++)
                        {   //active check
                            if (Pool.intObjPool[Pool.intObjCounter].active)
                            {   //overlap check
                                Pool.windInts_Possible++; //possible interaction up next
                                if (windRef.compCollision.rec.Intersects(
                                    Pool.intObjPool[Pool.intObjCounter].compCollision.rec))
                                {   //wind pushes this obj
                                    Pool.windInts_ThisFrame++; //count it
                                    Pool.intObjPool[Pool.intObjCounter].compMove.direction = windRef.compMove.direction;
                                    Push(Pool.intObjPool[Pool.intObjCounter]);
                                }
                            }
                        }

                        #endregion


                        #region PUSH Actors

                        for (Pool.actorCounter = 0; Pool.actorCounter < Pool.actorCount; Pool.actorCounter++)
                        {   //active check
                            if (Pool.actorPool[Pool.actorCounter].active)
                            {   //overlap check
                                Pool.windInts_Possible++; //possible interaction up next
                                if (windRef.compCollision.rec.Intersects(
                                    Pool.actorPool[Pool.actorCounter].compCollision.rec))
                                {   //wind pushes this actor
                                    Pool.windInts_ThisFrame++; //count it
                                    Pool.actorPool[Pool.actorCounter].compMove.direction = windRef.compMove.direction;
                                    Push(Pool.actorPool[Pool.actorCounter]);
                                }
                            }
                        }

                        #endregion


                        #region PUSH Projectiles

                        for (Pool.projectileCounter = 0; Pool.projectileCounter < Pool.projectileCount; Pool.projectileCounter++)
                        {   //active check
                            if (Pool.projectilePool[Pool.projectileCounter].active)
                            {   //overlap check
                                Pool.windInts_Possible++; //possible interaction up next
                                if (windRef.compCollision.rec.Intersects(
                                    Pool.projectilePool[Pool.projectileCounter].compCollision.rec))
                                {   //wind pushes this object
                                    Pool.windInts_ThisFrame++; //count it
                                    Pool.projectilePool[Pool.projectileCounter].compMove.direction = windRef.compMove.direction;
                                    Push(Pool.projectilePool[Pool.projectileCounter]);
                                }
                            }
                        }

                        #endregion

                    }
                    else
                    {   //wind obj is outside of current room rec, and is released
                        Kill(windRef);
                    }
                }
            }

            #endregion


        }



        //death events
        public static void Kill(WindObject Wind)
        {   //pop attention particle
            Functions_Particle.Spawn(
                ParticleType.Attention,
                Wind.compSprite.position.X,
                Wind.compSprite.position.Y);
            Functions_Pool.Release(Wind);
        }


        


        public static void Push(InteractiveObject Obj)
        {
            if(Obj.compMove.moveable)
            {   //wind is hitting moveable obj, push obj
                Functions_Movement.Push(Obj.compMove, Obj.compMove.direction, 1.0f);
            }
        }

        public static void Push(Actor Act)
        {
            //wind is overlapping actor, handle per actor pushes with gust
            Functions_Movement.Push(Act.compMove, Act.compMove.direction, 1.0f);
        }

        public static void Push(Projectile Pro)
        {
            if (Pro.compMove.moveable)
            {   //wind is overlapping pro, handle per pro pushes with gust
                Functions_Movement.Push(Pro.compMove, Pro.compMove.direction, 1.0f);
            }
        }
       
    }
}