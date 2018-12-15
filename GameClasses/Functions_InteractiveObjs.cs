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
    public static class Functions_InteractiveObjs
    {
        static int xDistance;
        static int yDistance;
        static Boolean overlap;





        public static void Reset(InteractiveObject IntObj)
        {
            //reset interactive obj
            IntObj.group = InteractiveGroup.Object; //assume object is a generic object
            IntObj.type = InteractiveType.Barrel; //reset the type
            IntObj.direction = Direction.Down;
            IntObj.active = true; //assume this object should draw / animate
            IntObj.canBeSaved = false; //most objects cannot be saved as XML data

            IntObj.counter = 0;
            IntObj.countTotal = 0;
            IntObj.interacts = false;
            IntObj.interactiveFrame = 0;

            IntObj.underWater = false;
            IntObj.inWater = false;
            IntObj.selfCleans = false;

            //reset the sprite component
            IntObj.compSprite.drawRec.Width = 16 * 1; //assume cell size is 16x16 (most are)
            IntObj.compSprite.drawRec.Height = 16 * 1;
            IntObj.compSprite.zOffset = 0;
            IntObj.compSprite.flipHorizontally = false;
            IntObj.compSprite.rotation = Rotation.None;
            IntObj.compSprite.scale = 1.0f;
            IntObj.compSprite.texture = Assets.CommonObjsSheet;
            IntObj.compSprite.visible = true;

            //reset the animation component
            IntObj.compAnim.speed = 10; //set obj's animation speed to default value
            IntObj.compAnim.loop = true; //assume obj's animation loops
            IntObj.compAnim.index = 0; //reset the current animation index/frame
            IntObj.compAnim.timer = 0; //reset the elapsed frames

            //reset the collision component
            IntObj.compCollision.blocking = true; //assume the object is blocking (most are)
            IntObj.compCollision.rec.Width = 16; //assume collisionRec is 16x16
            IntObj.compCollision.rec.Height = 16; //(most are)
            IntObj.compCollision.offsetX = -8; //assume collisionRec offset is -8x-8
            IntObj.compCollision.offsetY = -8; //(most are)

            //reset the move component
            IntObj.compMove.magnitude.X = 0; //discard any previous magnitude
            IntObj.compMove.magnitude.Y = 0; //
            IntObj.compMove.speed = 0.0f; //assume this object doesn't move
            IntObj.compMove.friction = 0.75f; //normal friction
            IntObj.compMove.moveable = false; //most objects cant be moved
            IntObj.compMove.grounded = true; //most objects exist on the ground

            //reset the sfx component
            IntObj.sfx.hit = null;
            IntObj.sfx.kill = null;
        }

        static InteractiveObject intObjRef;
        public static InteractiveObject Spawn(InteractiveType Type, float X, float Y, Direction Dir)
        {   //spawns obj at the X, Y location, with direction
            intObjRef = Functions_Pool.GetIntObj();
            intObjRef.direction = Dir;

            Functions_Movement.Teleport(intObjRef.compMove, X, Y);
            Functions_Component.Align(intObjRef);

            SetType(intObjRef, Type);
            return intObjRef;
        }

        public static void Kill(InteractiveObject IntObj, Boolean spawnLoot, Boolean becomeDebris)
        {
            //pop an attention particle
            Functions_Particle.Spawn(
                ParticleType.Attention,
                IntObj.compSprite.position.X,
                IntObj.compSprite.position.Y);
            if (IntObj.sfx.kill != null) { Assets.Play(IntObj.sfx.kill); }

            //based on obj type, we spawn death debris
            if (IntObj.group == InteractiveGroup.Enemy)
            {   //ignore seekers for gore decorations
                if (IntObj.type != InteractiveType.Enemy_SeekerExploder)
                {   //create floor blood, blood explosion, maybe skeleton
                    DecorateEnemyDeath(IntObj.compSprite, true);
                }
                Functions_Pool.Release(IntObj);
                return;
            }

            //decorate enemy death handles enemy loot spawns
            if (spawnLoot) { Functions_Loot.SpawnLoot(IntObj.compSprite.position); }

            //should obj become debris or get released?
            if (becomeDebris)
            {   //spawn debris explosion, become debris ground obj
                Functions_Particle.Spawn_Explosion(
                    ParticleType.DebrisBrown,
                    IntObj.compSprite.position.X,
                    IntObj.compSprite.position.Y,
                    true); //circular explosion
                BecomeDebris(IntObj);
            }
            else { Functions_Pool.Release(IntObj); }
        }








        public static void Update(InteractiveObject IntObj)
        {
            if (IntObj.interacts)
            {
                //perform interactions (obj ai) here
            }



            //old way from functions_ai.handleObj(Obj)


            //Obj.group checks

            #region Spreading water thru ditches

            if (IntObj.group == InteractiveGroup.Ditch)
            {
                //this ditch is 'filled' (hasAI), so it spreads to nearby unfilled ditches
                IntObj.counter++; //this isn't being used on roomObjs, so we steal it
                if (IntObj.counter == IntObj.interactiveFrame)
                {   //reset timer
                    IntObj.counter = 0; //only 'spread' water to empty ditches on their interactive frame

                    //loop over all active roomObjs, //locating an unfilled ditches
                    for (i = 0; i < Pool.intObjCount; i++)
                    {
                        if (Pool.intObjPool[i].active &
                            Pool.intObjPool[i].group == InteractiveGroup.Ditch &
                            Pool.intObjPool[i].interacts == false) //unfilled
                        {

                            //expand horizontally
                            IntObj.compCollision.rec.Width = 22;
                            IntObj.compCollision.rec.X -= 4;
                            //check collisions
                            if (Pool.intObjPool[i].compCollision.rec.Intersects(IntObj.compCollision.rec))
                            {   //fill empty ditch obj (will create splash)
                                Functions_Dig.FillDitch(Pool.intObjPool[i]);
                            }
                            //contract
                            IntObj.compCollision.rec.Width = 16;
                            IntObj.compCollision.rec.X += 4;

                            //expand vertically
                            IntObj.compCollision.rec.Height = 22;
                            IntObj.compCollision.rec.Y -= 4;
                            //check collisions
                            if (Pool.intObjPool[i].compCollision.rec.Intersects(IntObj.compCollision.rec))
                            {   //fill empty ditch obj (will create splash)
                                Functions_Dig.FillDitch(Pool.intObjPool[i]);
                            }
                            //retract
                            IntObj.compCollision.rec.Height = 16;
                            IntObj.compCollision.rec.Y += 4;
                        }
                    }
                }
            }

            #endregion


            #region Handle RoomObj Enemies - behavior

            else if (IntObj.group == InteractiveGroup.Enemy)
            {
                //if an enemy has gone beyond the bounds of a roomRec, release without loot
                if (!LevelSet.currentLevel.currentRoom.rec.Contains(IntObj.compSprite.position))
                {
                    Functions_Pool.Release(IntObj);
                }


                #region Enemy - Turtles & Crabs

                if (IntObj.type == InteractiveType.Enemy_Turtle
                    || IntObj.type == InteractiveType.Enemy_Crab)
                {   //rarely gently push in a direction
                    if (Functions_Random.Int(0, 1001) > 900)
                    {
                        Functions_Movement.Push(IntObj.compMove,
                            Functions_Direction.GetRandomDirection(), 0.5f);
                    }
                }

                #endregion


                #region Enemy - Rats

                else if (IntObj.type == InteractiveType.Enemy_Rat)
                {
                    //very rarely play rat soundfx
                    if (Functions_Random.Int(0, 1001) > 999)
                    { Assets.Play(Assets.sfxRatSqueak); }

                    //rarely choose a random cardinal direction
                    if (Functions_Random.Int(0, 1001) > 990)
                    {
                        IntObj.direction = Functions_Direction.GetRandomCardinal();
                        //set animation frame based on direction
                        if (IntObj.direction == Direction.Up)
                        { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Enemy_Rat_Up; }
                        else if (IntObj.direction == Direction.Right)
                        { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Enemy_Rat_Right; }
                        else if (IntObj.direction == Direction.Down)
                        { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Enemy_Rat_Down; }
                        else if (IntObj.direction == Direction.Left)
                        { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Enemy_Rat_Left; }
                    }

                    //often push in current direction
                    if (Functions_Random.Int(0, 1001) > 700)
                    { Functions_Movement.Push(IntObj.compMove, IntObj.direction, 1.0f); }
                }

                #endregion


                #region Enemy - Seeker Exploders

                else if (IntObj.type == InteractiveType.Enemy_SeekerExploder)
                {
                    //get the x & y distances between actor and hero
                    xDistance = (int)Math.Abs(Pool.hero.compSprite.position.X - IntObj.compSprite.position.X);
                    yDistance = (int)Math.Abs(Pool.hero.compSprite.position.Y - IntObj.compSprite.position.Y);

                    if (yDistance < 16 * 10 & xDistance < 16 * 10) //can seeker see hero?
                    {   //set direction towards hero
                        IntObj.compMove.direction = Functions_Direction.GetDiagonalToHero(IntObj.compSprite.position);
                        IntObj.direction = IntObj.compMove.direction;
                        //seeker moves with high energy
                        Functions_Movement.Push(IntObj.compMove, IntObj.compMove.direction, 0.25f);
                        IntObj.compAnim.currentAnimation = AnimationFrames.Wor_SeekerExploder_Chase;
                    }
                    else
                    {   //randomly set direction seeker moves
                        if (Functions_Random.Int(0, 100) > 95)
                        { IntObj.compMove.direction = Functions_Direction.GetRandomDirection(); }
                        IntObj.direction = IntObj.compMove.direction;
                        //seeker moves with less energy
                        Functions_Movement.Push(IntObj.compMove, IntObj.compMove.direction, 0.1f);
                        IntObj.compAnim.currentAnimation = AnimationFrames.Wor_SeekerExploder_Idle;
                    }

                    if (yDistance < 13 & xDistance < 13) //is seeker close enough to explode?
                    {   //instantly explode
                        Functions_Projectile.Spawn(
                            ProjectileType.Explosion,
                            IntObj.compSprite.position.X,
                            IntObj.compSprite.position.Y,
                            Direction.None);
                        Functions_Pool.Release(IntObj);
                    }
                }

                #endregion


            }

            #endregion






            //IntObj.type checks

            #region Dungeon Objects

            if (IntObj.type == InteractiveType.Flamethrower)
            {
                if (Functions_Random.Int(0, 500) > 497) //aggressively shoots
                {   //shoot fireball towards hero along a cardinal direction
                    Functions_Projectile.Spawn(
                        ProjectileType.Fireball,
                        IntObj.compMove.position.X,
                        IntObj.compMove.position.Y,
                        Functions_Direction.GetCardinalDirectionToHero(IntObj.compSprite.position));
                }
            }


            else if (IntObj.type == InteractiveType.Dungeon_WallStatue)
            {
                if (Functions_Random.Int(0, 2000) > 1998) //rarely shoots
                {   //lol, this is wrong and spawns arrow ontop of statue
                    Functions_Projectile.Spawn(
                        ProjectileType.Arrow,
                        IntObj.compMove.position.X,
                        IntObj.compMove.position.Y,
                        IntObj.direction);
                }   //this should have an offset applied based on it's' direction
            }

            else if (IntObj.type == InteractiveType.Lava_Pit)
            {
                if (Functions_Random.Int(0, 2000) > 1997) //occasionally bubbles
                { Functions_Particle.Spawn(ParticleType.PitBubble, IntObj); }
            }

            #endregion


            #region Switches

            else if (IntObj.type == InteractiveType.Dungeon_Switch || IntObj.type == InteractiveType.Dungeon_SwitchDown)
            {
                //only if a level is a switch puzzle type do we enable switches
                if (LevelSet.currentLevel.currentRoom.puzzleType == PuzzleType.Switch)
                {   //loop over all active blocking roomObjs
                    overlap = false; //assume no hit
                    for (i = 0; i < Pool.intObjCount; i++)
                    {   //only blocking objs can activate switches
                        if (Pool.intObjPool[i].active &
                            Pool.intObjPool[i].compCollision.blocking &
                            Pool.intObjPool[i].compCollision.rec.Intersects(IntObj.compCollision.rec))
                        { overlap = true; }
                    }
                    if (overlap)
                    {   //if any objs overlap switch, openTrap doors
                        Functions_Room.OpenTrapDoors();
                        //bail if we already did this
                        if (IntObj.type == InteractiveType.Dungeon_SwitchDown) { return; }
                        SetType(IntObj, InteractiveType.Dungeon_SwitchDown);
                        Functions_Particle.Spawn(
                            ParticleType.Attention,
                            IntObj.compSprite.position.X,
                            IntObj.compSprite.position.Y);
                        Assets.Play(Assets.sfxSwitch);
                    }
                    else
                    {   //else close all open doors to trap doors
                        Functions_Room.CloseTrapDoors();
                        //bail if we already did this
                        if (IntObj.type == InteractiveType.Dungeon_Switch) { return; }
                        SetType(IntObj, InteractiveType.Dungeon_Switch);
                        Functions_Particle.Spawn(
                            ParticleType.Attention,
                            IntObj.compSprite.position.X,
                            IntObj.compSprite.position.Y);
                        Assets.Play(Assets.sfxSwitch);
                    }
                }
            }


            #endregion


            #region Fairy

            else if (IntObj.type == InteractiveType.Fairy)
            {
                if (Functions_Random.Int(0, 101) > 93) //float around
                {   //randomly push fairy a direction
                    Functions_Movement.Push(IntObj.compMove,
                        Functions_Direction.GetRandomDirection(), 1.0f);
                    //check that the fairy overlaps the current room rec,
                    //otherwise the fairy has strayed too far and must be killed
                    if (!LevelSet.currentLevel.currentRoom.rec.Contains(IntObj.compSprite.position))
                    {
                        Functions_Particle.Spawn(
                            ParticleType.Attention,
                            IntObj.compSprite.position.X + 0,
                            IntObj.compSprite.position.Y + 0);
                        Functions_Pool.Release(IntObj);
                    }
                }
            }

            #endregion







            //world objects

            #region Bush Stump

            //bush stump obj growing back into a bush
            else if (IntObj.type == InteractiveType.Bush_Stump)
            {
                IntObj.counter++; //this isn't being used on roomObjs, so we steal it
                if (IntObj.counter == IntObj.interactiveFrame)
                {   //reset timer
                    IntObj.counter = 0;

                    //dramatically expand stump's hitBox
                    IntObj.compCollision.rec.Width = 32;
                    IntObj.compCollision.rec.Height = 32;
                    IntObj.compCollision.rec.X = (int)IntObj.compSprite.position.X - 16;
                    IntObj.compCollision.rec.Y = (int)IntObj.compSprite.position.Y - 16;


                    #region Prevent Growth ONTO active Actors in room

                    //stop regrowing if bush touches ANY actor (else will grow-lock actors)
                    for (i = 0; i < Pool.actorCount; i++)
                    {   //note: this wont grow bushes over dead actors, but we could add a death check
                        if (Pool.actorPool[i].active)
                        {
                            if (Pool.actorPool[i].compCollision.rec.Intersects(IntObj.compCollision.rec))
                            {   //reset hitBox, bail from method
                                SetType(IntObj, InteractiveType.Bush_Stump);
                                return;
                            }
                        }
                    }

                    #endregion


                    //loop over all active roomObjs, looking at filled ditches + water tiles
                    for (i = 0; i < Pool.intObjCount; i++)
                    {
                        if (
                            //filled ditch
                            (Pool.intObjPool[i].active &
                            Pool.intObjPool[i].group == InteractiveGroup.Ditch &
                            Pool.intObjPool[i].interacts == true)
                            ||
                            //water tile
                            (Pool.intObjPool[i].active &
                            Pool.intObjPool[i].type == InteractiveType.Water_2x2)
                            )
                        {
                            //check collisions
                            if (Pool.intObjPool[i].compCollision.rec.Intersects(IntObj.compCollision.rec))
                            {   //regrow into bush, with a pop
                                Functions_Particle.Spawn(
                                    ParticleType.Attention,
                                    IntObj.compSprite.position.X,
                                    IntObj.compSprite.position.Y);
                                SetType(IntObj, InteractiveType.Bush);
                                Assets.Play(Assets.sfxGrassWalk); //sounds kinda grow-y
                                return; //this only needs to happen once
                            }
                        }
                    }

                    //if the stump has reached this code, it never reached
                    //a filled ditch, so it should remain a stump - BUT,
                    //we need to reset it's hitBox. safe way of doing this:
                    SetType(IntObj, InteractiveType.Bush_Stump);
                }
            }

            #endregion


            #region Tree - Burning

            else if (IntObj.type == InteractiveType.Tree_Burning)
            {   //check to see if tree should still burn
                if (IntObj.counter < IntObj.countTotal)
                {
                    IntObj.counter++;
                    if (IntObj.counter < 30 * 5) //how many frames of burning?
                    {
                        if (Functions_Random.Int(0, 101) > 50)
                        {   //spawn smoke often
                            Functions_Particle.Spawn(ParticleType.RisingSmoke,
                                IntObj.compSprite.position.X + 4 + Functions_Random.Int(-9, 9),
                                IntObj.compSprite.position.Y + Functions_Random.Int(-10, 8));
                        }
                        if (Functions_Random.Int(0, 101) > 85)
                        {   //spawn fires on top of tree
                            Functions_Particle.Spawn(ParticleType.Fire,
                                IntObj.compSprite.position.X + Functions_Random.Int(-6, 6),
                                IntObj.compSprite.position.Y + Functions_Random.Int(-9, 5));

                        }
                        if (Functions_Random.Int(0, 101) > 90)
                        {   //spawn fires along tree trunk
                            Functions_Particle.Spawn(ParticleType.Fire,
                                IntObj.compSprite.position.X + 0,
                                IntObj.compSprite.position.Y + Functions_Random.Int(4, 16));
                        }
                    }
                }
                //stop 'burning' phase of tree, remove from AI calculations
                else
                {
                    Assets.Play(Assets.sfxActorLand); //decent popping sound
                    //pop the bushy top part
                    Functions_Particle.Spawn(
                        ParticleType.Attention,
                        IntObj.compSprite.position.X,
                        IntObj.compSprite.position.Y - 2);
                    //pop leaves in circular decorative pattern for tree top
                    Functions_Particle.Spawn_Explosion(
                        ParticleType.LeafGreen,
                        IntObj.compSprite.position.X + 2,
                        IntObj.compSprite.position.Y - 4, true);
                    //switch to burned tree
                    Reset(IntObj);
                    SetType(IntObj, InteractiveType.Tree_Burnt);
                }
            }
            else if (IntObj.type == InteractiveType.Tree_Burnt)
            {
                if (Functions_Random.Int(0, 201) > 199)
                {   //rarely smoke, centered to top of burned tree
                    Functions_Particle.Spawn(ParticleType.RisingSmoke,
                        IntObj.compSprite.position.X + 4 + Functions_Random.Int(-3, 3),
                        IntObj.compSprite.position.Y + 2);
                }
            }

            #endregion


            #region Collapsing Roofs

            else if (IntObj.type == InteractiveType.House_Roof_Collapsing)
            {
                IntObj.counter++;
                if (IntObj.counter == IntObj.interactiveFrame)
                {
                    //IntObj.counter = 0; //no need to reset, this only happens once
                    //expand to check surrounding tiles
                    IntObj.compCollision.rec.Width = 32;
                    IntObj.compCollision.rec.Height = 32;
                    IntObj.compCollision.rec.X = (int)IntObj.compSprite.position.X - 16;
                    IntObj.compCollision.rec.Y = (int)IntObj.compSprite.position.Y - 16;

                    //loop over all active roomObjs, collapse any nearby roofs
                    for (i = 0; i < Pool.intObjCount; i++)
                    {
                        if (Pool.intObjPool[i].active)
                        {
                            if (Pool.intObjPool[i].type == InteractiveType.House_Roof_Bottom
                                || Pool.intObjPool[i].type == InteractiveType.House_Roof_Top
                                || Pool.intObjPool[i].type == InteractiveType.House_Roof_Chimney)
                            {   //check for overlap / interaction
                                if (Pool.intObjPool[i].compCollision.rec.Intersects(IntObj.compCollision.rec))
                                { CollapseRoof(Pool.intObjPool[i]); }
                            }
                        }
                    }

                    //pop attention
                    Functions_Particle.Spawn(
                        ParticleType.Attention,
                        IntObj.compSprite.position.X,
                        IntObj.compSprite.position.Y);
                    //turn this roof obj into debris
                    BecomeDebris(IntObj);
                }
            }

            #endregion


            #region Chimney - smoke

            else if (IntObj.type == InteractiveType.House_Roof_Chimney)
            {   //chimney is a roof
                if (IntObj.compSprite.alpha == 1.0f) //chimney is visible
                {   //roofs switch between visible and non-visible via alpha
                    if (Functions_Random.Int(0, 100) > 90)
                    {   //often spawn rising smoke particles from obj
                        Functions_Particle.Spawn(
                            ParticleType.RisingSmoke,
                            IntObj.compSprite.position.X + 4 + Functions_Random.Int(-3, 3),
                            IntObj.compSprite.position.Y - 2 + Functions_Random.Int(-5, 3));
                    }
                }
            }

            #endregion













            


            #region Colliseum Judge

            else if (IntObj.type == InteractiveType.Judge_Colliseum)
            {
                //periodically check to see if all active actors are dead
                //if true, then hero has completed active challenge

                IntObj.counter++;
                if (IntObj.counter == IntObj.interactiveFrame)
                {   //reset timer
                    IntObj.counter = 0;


                    #region Check hero death + enemy death

                    //check to see if hero has died or killed all enemies
                    for (i = 0; i < Pool.actorCount; i++)
                    {
                        if (Pool.actorPool[i].active)
                        {
                            if (Pool.actorPool[i] == Pool.hero)
                            {   //check to see if the actor has died
                                if (Pool.hero.state == ActorState.Dead)
                                {
                                    // <<< exit condition : failed >>>


                                    #region Pop Failure Dialog

                                    //initially, current dialog wont be challenge failure, so only pops once
                                    if (Screens.Dialog.dialogs != AssetsDialog.Colliseum_Challenge_Failure)
                                    {   //this prevents spamming of failure dialog, which is annoying
                                        Screens.Dialog.SetDialog(AssetsDialog.Colliseum_Challenge_Failure);
                                        ScreenManager.AddScreen(Screens.Dialog);
                                    }

                                    #endregion


                                    //check to see if hero is at end of death animation,
                                    //this gives the player time to visually process hero 
                                    //spinning, falling to ground, screen fading in black

                                    if (Pool.hero.compAnim.index == Pool.hero.compAnim.currentAnimation.Count)
                                    {   //kicked out of colliseum to overworld screen
                                        Functions_Level.CloseLevel(ExitAction.Summary);
                                        //change the judge to another obj to prevent dialog spam
                                        SetType(IntObj, InteractiveType.Vendor_Colliseum_Mob);
                                        return; //challenge is not complete
                                    }
                                }
                            }
                            else
                            {
                                //find any living actor, fail check
                                if (Pool.actorPool[i].state != ActorState.Dead)
                                { return; } //challenge is not complete
                            }
                        }
                    }

                    #endregion


                    //if code gets here, then hero is only living actor
                    //this means the challenge has been completed


                    #region Reward the player based on current ChallengeSet

                    //standard
                    if (Functions_Colliseum.currentChallenge == Challenges.Blobs)
                    {   //reward hero with gold
                        PlayerData.gold += 25;
                    }

                    //minis
                    else if (Functions_Colliseum.currentChallenge == Challenges.Mini_Blackeyes
                        || Functions_Colliseum.currentChallenge == Challenges.Mini_Spiders)
                    {   //reward hero with gold
                        PlayerData.gold += 99;
                    }

                    //bosses
                    else if (Functions_Colliseum.currentChallenge == Challenges.Bosses_BigEye
                        || Functions_Colliseum.currentChallenge == Challenges.Bosses_BigBat
                        || Functions_Colliseum.currentChallenge == Challenges.Bosses_Kraken)
                    {   //reward hero with gold
                        PlayerData.gold += 99;
                    }

                    #endregion


                    // <<< exit condition : completed >>>
                    //exit level to pit level
                    LevelSet.currentLevel.ID = LevelID.SkullIsland_ColliseumPit;
                    Functions_Level.CloseLevel(ExitAction.Field);

                    //pop a new dialog screen telling player they completed challenge
                    Screens.Dialog.SetDialog(AssetsDialog.Colliseum_Challenge_Complete);
                    ScreenManager.AddScreen(Screens.Dialog);


                    Assets.Play(Assets.sfxGoldSpam); //audibly cue player they were rewarded
                    Assets.Play(Assets.sfxKeyPickup); //oh no, too spicy!
                }
            }


            #endregion





            //pets

            #region Pet - Dog

            else if (IntObj.type == InteractiveType.Pet_Dog)
            {

                //pet models 'state' based on the hero's state
                if (Pool.hero.state == ActorState.Climbing)
                {
                    //place pet at hero's location, slightly lower than hero
                    //this simulates hero climbing with pet in his backpack
                    Functions_Movement.Teleport(IntObj.compMove,
                        Pool.hero.compSprite.position.X,
                        Pool.hero.compSprite.position.Y + 0);
                    Functions_Component.Align(IntObj);

                    IntObj.compSprite.zOffset = 16; //sort pet over hero
                    IntObj.inWater = false; //pet isn't in water while on wall
                }
                else if (Pool.hero.state == ActorState.Landed)
                {   //prevent pet from falling down wall when hero 'lands' at top
                    Functions_Movement.Teleport(IntObj.compMove,
                        Pool.hero.compSprite.position.X,
                        //move north until no longer overlapping
                        Pool.hero.compSprite.position.Y -= 1);
                    Functions_Component.Align(IntObj);
                    //sort pet under hero (on ground)
                    IntObj.compSprite.zOffset = -8;
                }
                else
                {
                    //pet is free to roam around the game world (not in hero's backpack)

                    //track to the hero, within radius - get distance to hero
                    xDistance = (int)Math.Abs(Pool.hero.compSprite.position.X - IntObj.compSprite.position.X);
                    yDistance = (int)Math.Abs(Pool.hero.compSprite.position.Y - IntObj.compSprite.position.Y);

                    //check if pet can see hero
                    if (yDistance < 16 * 5 & xDistance < 16 * 5)
                    {   //if distance is less than rest radius, rest 
                        if (yDistance < 24 & xDistance < 24)
                        { } //do nothing, pet is close enough to hero to rest
                        else
                        {   //move diagonally towards hero
                            Functions_Movement.Push(IntObj.compMove,
                                Functions_Direction.GetDiagonalToHero(IntObj.compSprite.position),
                                0.4f);
                        }
                    }
                    //pet cannot see hero..
                    else
                    {   //randomly push the pet in a direction
                        if (Functions_Random.Int(0, 101) > 80)
                        {
                            Functions_Movement.Push(IntObj.compMove,
                                Functions_Direction.GetRandomDirection(), 1.0f);
                        }
                    }
                }

                //set the facing direction based on X magnitude
                if (IntObj.compMove.magnitude.X < 0) //moving left
                { IntObj.compSprite.flipHorizontally = true; }
                else { IntObj.compSprite.flipHorizontally = false; } //moving right 


                //set the animation frame, based on a number of factors

                //climbing
                if (Pool.hero.state == ActorState.Climbing)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Pet_Dog_Climbing; }
                //swimming
                else if (IntObj.inWater)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Pet_Dog_InWater; }
                //moving
                else if (Math.Abs(IntObj.compMove.magnitude.X) > 0 || Math.Abs(IntObj.compMove.magnitude.Y) > 0)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Pet_Dog_Move; }
                //not moving - idle
                else { IntObj.compAnim.currentAnimation = AnimationFrames.Pet_Dog_Idle; }

                //play the pet's sound fx occasionally
                if (Functions_Random.Int(0, 1001) > 995) { Assets.Play(Assets.sfxPetDog); }
            }

            #endregion


            #region Pet - Chicken

            else if (IntObj.type == InteractiveType.Pet_Chicken)
            {
                //randomly push the pet in a direction
                if (Functions_Random.Int(0, 101) > 98)
                {
                    Functions_Movement.Push(IntObj.compMove,
                        Functions_Direction.GetRandomDirection(),
                        2.0f);
                }

                //set the facing direction based on X magnitude
                if (IntObj.compMove.magnitude.X < 0) //moving left
                { IntObj.compSprite.flipHorizontally = true; }
                else { IntObj.compSprite.flipHorizontally = false; } //moving right

                //set moving or idle anim frame
                if (Math.Abs(IntObj.compMove.magnitude.X) > 0 || Math.Abs(IntObj.compMove.magnitude.Y) > 0)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Pet_Chicken_Move; }
                else { IntObj.compAnim.currentAnimation = AnimationFrames.Pet_Chicken_Idle; }

                //rarely play the pet's sound fx 
                if (Functions_Random.Int(0, 101) > 99) { Assets.Play(Assets.sfxPetChicken); }
            }

            #endregion




            //very special objects

            #region ExplodingObject

            else if (IntObj.type == InteractiveType.ExplodingObject)
            {
                IntObj.counter++;
                if (IntObj.counter > IntObj.countTotal)
                {
                    Functions_Projectile.Spawn(
                        ProjectileType.Explosion,
                        IntObj.compSprite.position.X,
                        IntObj.compSprite.position.Y,
                        Direction.None);
                    Kill(IntObj, true, false);
                }
            }

            #endregion







            //npcs + sidequests

            #region NPC - Farmer

            else if (IntObj.type == InteractiveType.NPC_Farmer)
            {
                //SETUP state
                //periodically expand hitbox to check for nearby bushes
                //if a bush is nearby, obj becomes farmer reward obj
                IntObj.counter++;
                if (IntObj.counter == IntObj.interactiveFrame)
                {   //reset timer
                    IntObj.counter = 0;
                    //expand to check surrounding tiles
                    IntObj.compCollision.rec.Width = 32;
                    IntObj.compCollision.rec.Height = 32;
                    IntObj.compCollision.rec.X = (int)IntObj.compSprite.position.X - 16;
                    IntObj.compCollision.rec.Y = (int)IntObj.compSprite.position.Y - 16;
                    //loop over all active roomObjs, locate any nearby bush
                    for (i = 0; i < Pool.intObjCount; i++)
                    {
                        if (Pool.intObjPool[i].active &
                            Pool.intObjPool[i].type == InteractiveType.Bush)
                        {   //if farmer touches neighboring bush, convert farmer to reward state
                            if (Pool.intObjPool[i].compCollision.rec.Intersects(IntObj.compCollision.rec))
                            {
                                Assets.Play(Assets.sfxKeyPickup); //audibly cue player
                                SetType(IntObj, InteractiveType.NPC_Farmer_Reward);
                                return; //bail, no need to continue
                            }
                        }
                    }
                    //if this section of code is reached, no bush was found, reset hitbox/obj
                    SetType(IntObj, InteractiveType.NPC_Farmer);
                }
            }
            else if (IntObj.type == InteractiveType.NPC_Farmer_Reward)
            {
                //REWARD state
                //periodically create an exclamation particle
                IntObj.counter++;
                if (IntObj.counter == IntObj.interactiveFrame)
                {   //reset timer
                    IntObj.counter = 0;
                    Functions_Particle.Spawn(
                        ParticleType.ExclamationBubble,
                        IntObj.compSprite.position.X - 3,
                        IntObj.compSprite.position.Y - 16);
                }
            }

            #endregion


            #region Brandy, Ship's Captain

            else if (IntObj.type == InteractiveType.Boat_Captain_Brandy)
            {   //she only transitions into a special animation if she's standing idle
                if (IntObj.compAnim.currentAnimation == AnimationFrames.Wor_Boat_Captain_Brandy)
                {   //randomly play hands up or blink animation
                    if (Functions_Random.Int(0, 1001) > 990)
                    {
                        IntObj.compAnim.index = 0;
                        if (Functions_Random.Int(0, 101) > 50)
                        { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Captain_Brandy_Blink; }
                        else
                        { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Captain_Brandy_HandsUp; }
                    }
                }
                else if (IntObj.compAnim.index == IntObj.compAnim.currentAnimation.Count)
                {   //once we hit the end of the special animation, return to idle
                    IntObj.compAnim.index = 0;
                    IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Captain_Brandy;
                }
            }

            #endregion


        }





        public static void SetRotation(InteractiveObject IntObj)
        {
            if (
                IntObj.type == InteractiveType.Lava_PitTrap
                || IntObj.type == InteractiveType.Lava_PitTeethBottom
                || IntObj.type == InteractiveType.Lava_PitTeethTop
                )
            {   //some objects only face Direction.Down
                IntObj.direction = Direction.Down;
                IntObj.compSprite.rotation = Rotation.None;
            }

            //room enemies only face down
            else if (IntObj.group == InteractiveGroup.Enemy)
            {
                IntObj.direction = Direction.Down;
                IntObj.compSprite.rotation = Rotation.None;
            }
            
            //set sprite's rotation based on direction & flipHorizontally boolean
            Functions_Component.SetSpriteRotation(IntObj.compSprite, IntObj.direction);
        }













        //level 0.5 caused by: boomerang
        public static void Bounce(InteractiveObject Obj)
        {   //Obj.compMove.direction needs to be set by collider


            #region Bush

            if (Obj.type == InteractiveType.Bush)
            {
                //pop leaf explosion
                Functions_Particle.Spawn_Explosion(
                    ParticleType.LeafGreen,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y);
                //covert bush to stump, play sfx
                SetType(Obj, InteractiveType.Bush_Stump);
                Assets.Play(Assets.sfxBushCut);
                //pop an attention particle
                Functions_Particle.Spawn(ParticleType.Attention,
                    Obj.compSprite.position.X, Obj.compSprite.position.Y);
                //rarely spawn loot
                Functions_Loot.SpawnLoot(Obj.compSprite.position, 20);
            }

            #endregion


            #region House Doors

            else if (Obj.type == InteractiveType.House_Door_Shut)
            {
                OpenHouseDoor(Obj);
            }

            #endregion


            #region Levers

            else if (Obj.type == InteractiveType.LeverOff
                || Obj.type == InteractiveType.LeverOn)
            {
                ActivateLeverObjects();
            }

            #endregion


            #region Explosive Barrels

            else if (Obj.type == InteractiveType.Barrel)
            {   //Obj.compMove.direction should be set by colliding pro prior
                HitBarrel(Obj);
            }

            #endregion


            #region Switch Block Globe/Buttons

            else if (Obj.type == InteractiveType.Dungeon_SwitchBlockBtn)
            {   //Obj.compMove.direction should be set by colliding pro prior
                FlipSwitchBlocks(Obj);
            }

            #endregion


            else
            {   //audible note something hit obj
                //Assets.Play(Assets.sfxActorLand);
            }
        }

        //level 1 caused by: sword, shovel, arrow, bat, bite/fang, thrown objs
        public static void Cut(InteractiveObject Obj)
        {   //Obj.compMove.direction needs to be set by collider


            #region Grass

            if (Obj.type == InteractiveType.Grass_Tall)
            {
                //pop an attention particle on grass pos
                Functions_Particle.Spawn(ParticleType.Attention,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y);
                //convert tallgrass to cut grass + sfx
                SetType(Obj, InteractiveType.Grass_Cut);
                Assets.Play(Assets.sfxBushCut);
                //rarely spawn loot
                if (Functions_Random.Int(0, 101) > 90) //cut that grass boi
                { Functions_Loot.SpawnLoot(Obj.compSprite.position); }
            }

            #endregion


            #region Pots, Dungeon pot (skull), Boat Barrels

            else if (
                Obj.type == InteractiveType.Pot
                || Obj.type == InteractiveType.Dungeon_Pot
                || Obj.type == InteractiveType.Boat_Barrel
                )
            {   //pop debris explosion
                Functions_Particle.Spawn_Explosion(
                    ParticleType.DebrisBrown,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y);
                //pop an attention particle
                Functions_Particle.Spawn(ParticleType.Attention,
                    Obj.compSprite.position.X, Obj.compSprite.position.Y);
                Kill(Obj, true, true); //become loot & debris
                Assets.Play(Assets.sfxShatter);
            }

            #endregion


            #region Burned Posts

            else if (
                Obj.type == InteractiveType.PostBurned_CornerLeft
                || Obj.type == InteractiveType.PostBurned_CornerRight
                || Obj.type == InteractiveType.PostBurned_Horizontal
                || Obj.type == InteractiveType.PostBurned_VerticalLeft
                || Obj.type == InteractiveType.PostBurned_VerticalRight
                )
            { Kill(Obj, true, true); }

            #endregion


            Bounce(Obj); //call all lower levels of destruction on obj
        }

        //level 1 caused by: sword, shovel, arrow, bat, bite, 
        public static void CutRoomEnemy(InteractiveObject Enemy)
        {

            #region Seeker Exploders

            if (Enemy.type == InteractiveType.Enemy_SeekerExploder)
            {   //Obj.compMove.direction should be set by colliding pro prior
                SetType(Enemy, InteractiveType.ExplodingObject); //explode
                Enemy.group = InteractiveGroup.Object; //remove from enemy death checks
                Functions_Movement.Push(Enemy.compMove, Enemy.compMove.direction, 6.0f);
                Assets.Play(Assets.sfxActorLand);
            }

            #endregion


            //All Other RoomObj Enemies
            else
            {   //Obj.compMove.direction should be set by colliding pro prior
                Functions_Particle.Spawn(ParticleType.Attention, Enemy);
                Kill(Enemy, true, false);
                Assets.Play(Assets.sfxActorLand);
            }
        }

        //level 2 caused by: hammer, spikeblock, floorspikes
        public static void Destroy(InteractiveObject Obj)
        {   //Obj.compMove.direction needs to be set by collider


            #region Posts

            if (
                Obj.type == InteractiveType.Post_CornerLeft
                || Obj.type == InteractiveType.Post_CornerRight
                || Obj.type == InteractiveType.Post_Horizontal
                || Obj.type == InteractiveType.Post_VerticalLeft
                || Obj.type == InteractiveType.Post_VerticalRight
                )
            { Kill(Obj, true, true); }

            #endregion


            #region Collapse Bombable Dungeon Doors

            else if (Obj.type == InteractiveType.Dungeon_DoorBombable)
            {   //blow up door, change to doorOpen
                Functions_Particle.Spawn(
                    ParticleType.Attention,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y);
                Assets.Play(Assets.sfxShatter);
                SetType(Obj, InteractiveType.Dungeon_DoorOpen);
                //hide the sprite switch with a blast particle
                Functions_Particle.Spawn(ParticleType.Blast,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y);
                //update the dungeon data that we collapsed this door
                SetDungeonDoor(Obj);
            }

            #endregion


            #region Crack Normal Dungeon Walls

            else if (Obj.type == InteractiveType.Dungeon_WallStraight)
            {   //'crack' normal walls
                SetType(Obj, InteractiveType.Dungeon_WallStraightCracked);
                Functions_Particle.Spawn(ParticleType.Blast,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y);
                Assets.Play(Assets.sfxShatter);
            }

            #endregion





            //turn some objs into loot + debris
            else if (


            #region Dungeon Objs - (Statue, Signpost, etc)

                //limited set for now
                Obj.type == InteractiveType.Dungeon_Statue
                || Obj.type == InteractiveType.Signpost

            #endregion


            #region Building Walls & Doors

                //world objs

                //building objs
                || Obj.type == InteractiveType.House_Wall_FrontA
                || Obj.type == InteractiveType.House_Wall_FrontB
                || Obj.type == InteractiveType.House_Wall_Back
                || Obj.type == InteractiveType.House_Wall_Side_Left
                || Obj.type == InteractiveType.House_Wall_Side_Right
                || Obj.type == InteractiveType.House_Door_Shut
                || Obj.type == InteractiveType.House_Door_Open

            #endregion


            #region Building interior objs

                || Obj.type == InteractiveType.House_Bookcase
                || Obj.type == InteractiveType.House_Shelf
                || Obj.type == InteractiveType.House_Stove
                || Obj.type == InteractiveType.House_Sink
                || Obj.type == InteractiveType.House_TableSingle
                || Obj.type == InteractiveType.House_TableDoubleLeft
                || Obj.type == InteractiveType.House_TableDoubleRight
                || Obj.type == InteractiveType.House_Chair
                || Obj.type == InteractiveType.House_Bed

            #endregion


                )
            { Kill(Obj, true, true); }






            else
            {   //call all lower levels of destruction on obj
                Cut(Obj);
            }
        }

        //level 3 caused by: explosions
        public static void Explode(InteractiveObject Obj)
        {


            #region Trees - unburnt and burnt

            if (Obj.type == InteractiveType.Tree
                || Obj.type == InteractiveType.Tree_Burnt)
            {
                Assets.Play(Assets.sfxShatter);
                //switch to tree stump
                SetType(Obj, InteractiveType.Tree_Stump);
                //rarely spawn loot
                if (Functions_Random.Int(0, 101) > 80)
                { Functions_Loot.SpawnLoot(Obj.compSprite.position); }
                //pop the bushy/not bushy top part
                Functions_Particle.Spawn(
                    ParticleType.Attention,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y - 2);

                if (Obj.type == InteractiveType.Tree)
                {   //pop leaves in circular pattern toward top
                    Functions_Particle.Spawn_Explosion(
                        ParticleType.LeafGreen,
                        Obj.compSprite.position.X + 2,
                        Obj.compSprite.position.Y - 4, true);
                }
                else
                {   //pop debris in random dir from trunk
                    Functions_Particle.Spawn_Explosion(
                        ParticleType.LeafGreen,
                        Obj.compSprite.position.X + 2,
                        Obj.compSprite.position.Y + 0, false);
                }
            }

            #endregion


            #region Flowers

            else if (Obj.type == InteractiveType.Flowers)
            {   //switch to least grass
                SetType(Obj, InteractiveType.Grass_2);
                //attention pop
                Functions_Particle.Spawn(
                        ParticleType.Attention,
                        Obj.compSprite.position.X + 2,
                        Obj.compSprite.position.Y - 4);
            }

            #endregion


            else
            {   //call all lower levels of destruction on obj
                Destroy(Obj);
            }
        }








        //BURNING STATUS caused by: ground fire, fireball
        public static void Burn(InteractiveObject Obj)
        {

            #region Grass

            if (
                Obj.type == InteractiveType.Grass_Tall
                || Obj.type == InteractiveType.Grass_Cut
                || Obj.type == InteractiveType.Grass_2
                || Obj.type == InteractiveType.Flowers
                )
            {   //spread the fire 
                Functions_Projectile.Spawn(
                    ProjectileType.GroundFire,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y - 3,
                    Direction.None);
                //pop an attention particle on grass pos
                Functions_Particle.Spawn(ParticleType.Attention,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y);
                //convert to burned version
                SetType(Obj, InteractiveType.Grass_Burned);
                Assets.Play(Assets.sfxLightFire);
            }

            #endregion


            #region Bush

            else if (Obj.type == InteractiveType.Bush)
            {   //spread the fire 
                Functions_Projectile.Spawn(
                    ProjectileType.GroundFire,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y - 3,
                    Direction.None);
                Assets.Play(Assets.sfxLightFire);
                Destroy(Obj); //destroy bush as normal
            }

            #endregion


            #region Posts

            else if (
                Obj.type == InteractiveType.Post_VerticalRight
                || Obj.type == InteractiveType.Post_CornerRight
                || Obj.type == InteractiveType.Post_Horizontal
                || Obj.type == InteractiveType.Post_CornerLeft
                || Obj.type == InteractiveType.Post_VerticalLeft
                )
            {   //switch to burned post
                if (Obj.type == InteractiveType.Post_VerticalRight)
                { SetType(Obj, InteractiveType.PostBurned_VerticalRight); }
                else if (Obj.type == InteractiveType.Post_CornerRight)
                { SetType(Obj, InteractiveType.PostBurned_CornerRight); }
                else if (Obj.type == InteractiveType.Post_Horizontal)
                { SetType(Obj, InteractiveType.PostBurned_Horizontal); }
                else if (Obj.type == InteractiveType.Post_CornerLeft)
                { SetType(Obj, InteractiveType.PostBurned_CornerLeft); }
                else if (Obj.type == InteractiveType.Post_VerticalLeft)
                { SetType(Obj, InteractiveType.PostBurned_VerticalLeft); }

                //spread the fire 
                Functions_Projectile.Spawn(
                    ProjectileType.GroundFire,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y - 3,
                    Direction.None);
                Assets.Play(Assets.sfxLightFire);
            }

            #endregion


            #region Tree

            else if (Obj.type == InteractiveType.Tree)
            {   //switch to burned tree
                SetType(Obj, InteractiveType.Tree_Burning);
                //place an initial fire at bottom of tree
                Functions_Particle.Spawn(ParticleType.Fire,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y + 16);
            }

            #endregion


            #region Floor Torch

            else if (Obj.type == InteractiveType.TorchUnlit)
            {
                LightTorch(Obj);
            }

            #endregion


            #region Floors - house, boat

            else if (Obj.type == InteractiveType.Boat_Floor)
            {   //spread the fire 
                Functions_Projectile.Spawn(
                    ProjectileType.GroundFire,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y - 3,
                    Direction.None);
                Assets.Play(Assets.sfxLightFire);
                //convert to burned version
                SetType(Obj, InteractiveType.Boat_Floor_Burned);
            }

            #endregion

        }








        //special methods

        public static void MeltIceTile(InteractiveObject Obj)
        {   //create rising smoke to fake evaporation
            Functions_Particle.Spawn(ParticleType.RisingSmoke,
                Obj.compSprite.position.X,
                Obj.compSprite.position.Y - 4,
                Direction.Down);
            Functions_Particle.Spawn(ParticleType.Attention,
                Obj.compSprite.position.X,
                Obj.compSprite.position.Y,
                Direction.Down);
            Functions_Pool.Release(Obj);
        }

        public static void OpenHouseDoor(InteractiveObject Obj)
        {   //pop attention particle
            Functions_Particle.Spawn(
                ParticleType.Attention,
                Obj.compSprite.position.X,
                Obj.compSprite.position.Y);
            //switch to open door
            SetType(Obj, InteractiveType.House_Door_Open);
            //play an unlocking sound effect
            Assets.Play(Assets.sfxDoorOpen); //could be better
        }







        //house roof methods

        public static void HideRoofs()
        {   //for over active roomObjs, hide any roof obj found
            for (i = 0; i < Pool.intObjCount; i++)
            {
                if (Pool.intObjPool[i].active)
                {
                    if (Pool.intObjPool[i].type == InteractiveType.House_Roof_Bottom
                        || Pool.intObjPool[i].type == InteractiveType.House_Roof_Top
                        || Pool.intObjPool[i].type == InteractiveType.House_Roof_Chimney)
                    {
                        //instantly hide
                        //Pool.roomObjPool[i].compSprite.visible = false;
                        //fade hide
                        if (Pool.intObjPool[i].compSprite.alpha > 0f)
                        { Pool.intObjPool[i].compSprite.alpha -= 0.05f; }
                        if (Pool.intObjPool[i].compSprite.alpha < 0f)
                        { Pool.intObjPool[i].compSprite.alpha = 0f; }
                    }
                }
            }
        }

        public static void ShowRoofs()
        {   //for over active roomObjs, show any roof obj found
            for (i = 0; i < Pool.intObjCount; i++)
            {
                if (Pool.intObjPool[i].active)
                {
                    if (Pool.intObjPool[i].type == InteractiveType.House_Roof_Bottom
                        || Pool.intObjPool[i].type == InteractiveType.House_Roof_Top
                        || Pool.intObjPool[i].type == InteractiveType.House_Roof_Chimney)
                    {
                        //instantly show
                        //Pool.roomObjPool[i].compSprite.visible = true;
                        //fade show
                        if (Pool.intObjPool[i].compSprite.alpha < 1f)
                        { Pool.intObjPool[i].compSprite.alpha += 0.05f; }
                        if (Pool.intObjPool[i].compSprite.alpha > 1f)
                        { Pool.intObjPool[i].compSprite.alpha = 1f; }
                    }
                }
            }
        }

        public static void CollapseRoof(InteractiveObject Roof)
        {
            Assets.Play(Assets.sfxShatter); //play shatter sfx
            //turn roof into it's collapsing version
            SetType(Roof, InteractiveType.House_Roof_Collapsing);
        }











        //various interaction methods

        public static void BecomeDebris(InteractiveObject IntObj)
        {   //become debris
            SetType(IntObj, InteractiveType.Debris);
        }

        public static void DecorateEnemyDeath(ComponentSprite compSprite, Boolean dropSkeleton)
        {   //used to kill enemies in uniform way
            Functions_Particle.Spawn_Explosion(
                ParticleType.BloodRed,
                compSprite.position.X + 4,
                compSprite.position.Y + 4,
                false); //create blood spatter
            Assets.Play(Assets.sfxEnemyKill); //call sfx
            Functions_Loot.SpawnLoot(compSprite.position); //spawn loot
            //what kind of additional decorations should drop?
            if (Flags.Gore)
            {   //drop flood blood
                Spawn(InteractiveType.FloorBlood,
                    (int)compSprite.position.X,
                    (int)compSprite.position.Y,
                    Direction.Down);
                if (dropSkeleton)
                {   //if (Functions_Random.Int(0, 101) > 70) {} 
                    //drop skeleton
                    Spawn(InteractiveType.FloorSkeleton,
                        (int)compSprite.position.X,
                        (int)compSprite.position.Y,
                        Direction.Down);
                }
            }
        }


        static int g;
        public static void SelfClean(InteractiveObject IntObj)
        {
            //loop over indestructible objs, removing int based on types
            for (g = 0; g < Pool.indObjCount; g++)
            {   //ensure roomObj is active and not self-comparing
                if (Pool.indObjPool[g].active)
                {   //ensure roomObjs are overlapping
                    if (IntObj.compCollision.rec.Intersects(Pool.indObjPool[g].compCollision.rec))
                    {   //any int obj that overlaps an exit obj gets removed
                        if (Pool.indObjPool[g].group == IndestructibleGroup.Exit)
                        { Functions_Pool.Release(IntObj); }

                        //all other int vs ind overlaps are allowed
                        else { }
                    }
                }
            }

            //loop over interactive objs, removing int based on types
            for (g = 0; g < Pool.intObjCount; g++)
            {   //ensure intObj is active and not self-checking
                if (Pool.intObjPool[g].active & Pool.intObjPool[g] != IntObj)
                {   
                    //ensure roomObjs are overlapping
                    if (IntObj.compCollision.rec.Intersects(Pool.intObjPool[g].compCollision.rec))
                    {

                        #region Dungeon Walls

                        if(
                            IntObj.type == InteractiveType.Dungeon_WallStraight
                            || IntObj.type == InteractiveType.Dungeon_WallStraightCracked
                            )
                        {   //walls remove overlap with pillars/torches/wall decor objs
                            if (
                                Pool.intObjPool[g].type == InteractiveType.Dungeon_WallTorch
                                || Pool.intObjPool[g].type == InteractiveType.Dungeon_WallPillar
                                )
                            { Functions_Pool.Release(IntObj); }
                        }

                        #endregion


                        #region Floor Decorations - debris, stain, blood, skeletons

                        //if a floor decoration overlaps an obj, prolly remove it
                        else if (
                            IntObj.type == InteractiveType.Debris
                            || IntObj.type == InteractiveType.FloorStain
                            || IntObj.type == InteractiveType.FloorBlood
                            || IntObj.type == InteractiveType.FloorSkeleton
                            )
                        {
                            //remove obj if it is outside of current room rec
                            if (!LevelSet.currentLevel.currentRoom.rec.Intersects(IntObj.compCollision.rec))
                            { Functions_Pool.Release(IntObj); }

                            //if decor overlaps a copy of itself, remove decor
                            else if (IntObj.type == Pool.intObjPool[g].type)
                            { Functions_Pool.Release(IntObj); }

                            //decor cannot overlap blocking objects, obvs
                            else if (Pool.intObjPool[g].compCollision.blocking)
                            { Functions_Pool.Release(IntObj); }

                            //cannot be placed over ditches
                            else if (Pool.intObjPool[g].group == InteractiveGroup.Ditch)
                            { Functions_Pool.Release(IntObj); }

                            //these non-blocking objs remove decor too
                            else if (
                                //world objs that cant be overlapped
                                Pool.intObjPool[g].type == InteractiveType.Flowers
                                || Pool.intObjPool[g].type == InteractiveType.Grass_Tall
                                || Pool.intObjPool[g].type == InteractiveType.Bush_Stump
                                || Pool.intObjPool[g].type == InteractiveType.Tree_Stump

                                //dungeon objs that cant be overlapped
                                || Pool.intObjPool[g].type == InteractiveType.Lava_Pit
                                || Pool.intObjPool[g].type == InteractiveType.Lava_PitBridge
                                || Pool.intObjPool[g].type == InteractiveType.Lava_PitTrap

                                //unique objs that cant be overlapped
                                || Pool.intObjPool[g].type == InteractiveType.Boat_Stairs_Cover
                                //bottom piers, cause it looks look bad
                                || Pool.intObjPool[g].type == InteractiveType.Boat_Pier_BottomLeft
                                || Pool.intObjPool[g].type == InteractiveType.Boat_Pier_BottomMiddle
                                || Pool.intObjPool[g].type == InteractiveType.Boat_Pier_BottomRight
                                )
                            { Functions_Pool.Release(IntObj); }
                        }

                        #endregion


                        #region IceTiles

                        else if (IntObj.type == InteractiveType.IceTile)
                        {
                            //remove obj if it is outside of current room rec
                            if (!LevelSet.currentLevel.currentRoom.rec.Intersects(IntObj.compCollision.rec))
                            { Functions_Pool.Release(IntObj); }

                            //remove icetile if blocking roomObj overlaps
                            else if (Pool.intObjPool[g].compCollision.blocking)
                            {   //but allow some blocking objs, like posts
                                if (
                                    //burned posts
                                    Pool.intObjPool[g].type == InteractiveType.PostBurned_CornerLeft
                                    || Pool.intObjPool[g].type == InteractiveType.PostBurned_CornerRight
                                    || Pool.intObjPool[g].type == InteractiveType.PostBurned_Horizontal
                                    || Pool.intObjPool[g].type == InteractiveType.PostBurned_VerticalLeft
                                    || Pool.intObjPool[g].type == InteractiveType.PostBurned_VerticalRight
                                    //normal posts
                                    || Pool.intObjPool[g].type == InteractiveType.Post_CornerLeft
                                    || Pool.intObjPool[g].type == InteractiveType.Post_CornerRight
                                    || Pool.intObjPool[g].type == InteractiveType.Post_HammerPost_Down
                                    || Pool.intObjPool[g].type == InteractiveType.Post_VerticalLeft
                                    || Pool.intObjPool[g].type == InteractiveType.Post_VerticalRight
                                    )
                                { } //nothing, allow icetiles to overlap these objs
                                else //all other blocking objs kill ice tiles
                                { Functions_Pool.Release(IntObj); }
                            }

                            //cannot be placed over ditches
                            else if (Pool.intObjPool[g].group == InteractiveGroup.Ditch)
                            { Functions_Pool.Release(IntObj); }

                            //these non-blocking objs stop icetile birth too
                            else if (
                                //world objs that cant be overlapped
                                Pool.intObjPool[g].type == InteractiveType.Water_2x2
                                || Pool.intObjPool[g].type == InteractiveType.Coastline_1x2_Animated
                                || Pool.intObjPool[g].type == InteractiveType.Coastline_Corner_Exterior
                                || Pool.intObjPool[g].type == InteractiveType.Coastline_Corner_Interior
                                || Pool.intObjPool[g].type == InteractiveType.Coastline_Straight
                                //dungeon objs that cant be overlapped
                                || Pool.intObjPool[g].type == InteractiveType.Lava_Pit
                                || Pool.intObjPool[g].type == InteractiveType.Lava_PitBridge
                                || Pool.intObjPool[g].type == InteractiveType.Lava_PitTrap
                                || Pool.intObjPool[g].type == InteractiveType.Dungeon_SpikesFloorOff
                                || Pool.intObjPool[g].type == InteractiveType.Dungeon_SpikesFloorOn
                                //unique objs that cant be overlapped
                                || Pool.intObjPool[g].type == InteractiveType.Boat_Stairs_Cover
                                || Pool.intObjPool[g].type == InteractiveType.Boat_Bridge_Bottom
                                || Pool.intObjPool[g].type == InteractiveType.Boat_Pier_BottomLeft
                                || Pool.intObjPool[g].type == InteractiveType.Boat_Pier_BottomMiddle
                                || Pool.intObjPool[g].type == InteractiveType.Boat_Pier_BottomRight
                                //dont overlap other icetiles, pls
                                || Pool.intObjPool[g].type == InteractiveType.IceTile
                                )
                            { Functions_Pool.Release(IntObj); }
                        }

                        #endregion

                    }
                }
            }

            IntObj.selfCleans = false; //this is only run once 
        }






        static int i;
        public static void ActivateLeverObjects()
        {
            Assets.Play(Assets.sfxSwitch);
            for (i = 0; i < Pool.intObjCount; i++)
            {   //loop thru all active roomObjects
                if (Pool.intObjPool[i].active)
                {   //sync all lever objects
                    if (Pool.intObjPool[i].type == InteractiveType.LeverOff)
                    { SetType(Pool.intObjPool[i], InteractiveType.LeverOn); }
                    else if (Pool.intObjPool[i].type == InteractiveType.LeverOn)
                    { SetType(Pool.intObjPool[i], InteractiveType.LeverOff); }
                    //find any spikeFloor objects in the room, toggle them
                    else if (Pool.intObjPool[i].type == InteractiveType.Dungeon_SpikesFloorOn)
                    { SetType(Pool.intObjPool[i], InteractiveType.Dungeon_SpikesFloorOff); }
                    else if (Pool.intObjPool[i].type == InteractiveType.Dungeon_SpikesFloorOff)
                    { SetType(Pool.intObjPool[i], InteractiveType.Dungeon_SpikesFloorOn); }
                    //locate and toggle conveyor belt objects
                    else if (Pool.intObjPool[i].type == InteractiveType.ConveyorBeltOn)
                    { SetType(Pool.intObjPool[i], InteractiveType.ConveyorBeltOff); }
                    else if (Pool.intObjPool[i].type == InteractiveType.ConveyorBeltOff)
                    { SetType(Pool.intObjPool[i], InteractiveType.ConveyorBeltOn); }
                }
            }
        }

        public static void BounceOffBumper(ComponentMovement compMove, InteractiveObject Bumper)
        {   //handle the bumper animation
            Bumper.compSprite.scale = 1.5f;
            Assets.Play(Assets.sfxBounce);
            Functions_Particle.Spawn(
                ParticleType.Attention,
                Bumper.compSprite.position.X,
                Bumper.compSprite.position.Y);
            //bounce opposite direction
            compMove.direction = Functions_Direction.GetOppositeDirection(compMove.direction);
            //if the direction is none, then get a direction between bumper and collider
            if (compMove.direction == Direction.None)
            {
                compMove.direction = Functions_Direction.GetOppositeCardinal(
                    compMove.position, Bumper.compSprite.position);
            }
            //push collider in direction
            Functions_Movement.Push(compMove, compMove.direction, 6.0f);
        }

        public static void BounceSpikeBlock(InteractiveObject SpikeBlock)
        {   //spikeBlock must be moving
            if (Math.Abs(SpikeBlock.compMove.magnitude.X) > 0 || Math.Abs(SpikeBlock.compMove.magnitude.Y) > 0)
            {   //spawn a hit particle along spikeBlock's colliding edge
                Functions_Particle.Spawn(ParticleType.Sparkle, SpikeBlock);
                Assets.Play(Assets.sfxTapMetallic); //play the 'clink' sound effect                                  
                //flip the block's direction to the opposite direction
                SpikeBlock.compMove.direction = Functions_Direction.GetOppositeDirection(SpikeBlock.compMove.direction);
                SpikeBlock.compMove.magnitude.X = 0;
                SpikeBlock.compMove.magnitude.Y = 0;
                //push the block in it's new direction, out of this collision
                Functions_Movement.Push(SpikeBlock.compMove, SpikeBlock.compMove.direction, 2.0f);
                //force move spikeblock to it's new position, ignoring collisions
                SpikeBlock.compMove.position += SpikeBlock.compMove.magnitude;
                SpikeBlock.compMove.newPosition = SpikeBlock.compMove.position;
                Functions_Component.Align(SpikeBlock.compMove, SpikeBlock.compSprite, SpikeBlock.compCollision);
            }
        }

        public static void CloseDoors()
        {
            Assets.Play(Assets.sfxSwitch);
            for (i = 0; i < Pool.intObjCount; i++)
            {   //loop thru all active roomObjects
                if (Pool.intObjPool[i].active)
                {   //convert trap doors to open doors
                    if (Pool.intObjPool[i].type == InteractiveType.Dungeon_DoorOpen)
                    {   //all open doors inside room become trap doors (push hero + close)
                        SetType(Pool.intObjPool[i], InteractiveType.Dungeon_DoorTrap);
                    }
                }
            }
        }

        public static void ConveyorBeltPush(ComponentMovement compMove, InteractiveObject belt)
        {   //based on belt's direction, push moveComp by amount
            Functions_Movement.Push(compMove, belt.direction, 0.15f);
        }

        public static void HitBarrel(InteractiveObject Barrel)
        {   //turn into exploding barrel
            Barrel.compAnim.currentAnimation = AnimationFrames.Dungeon_BarrelExploding;
            //if barrel has a hit direction, push in that direction
            if (Barrel.compMove.direction != Direction.None)
            { Functions_Movement.Push(Barrel.compMove, Barrel.compMove.direction, 6.0f); }
            //become an explosion
            SetType(Barrel, InteractiveType.ExplodingObject);
            //play sfx
            Assets.Play(Assets.sfxEnemyHit);
        }

        public static void DragIntoPit(InteractiveObject Object, InteractiveObject Pit)
        {   //obj must be grounded
            if (Object.compMove.grounded)
            {
                //check to see if obj started falling, or has been falling
                if (Object.compSprite.scale == 1.0f) //begin falling state
                {   //dont play falling sound if entity is thrown pot (falling sound was just played)
                    //if (ObjA.type != ObjType.ProjectilePot) { Assets.Play(Assets.sfxActorFall); }
                    Assets.Play(Assets.sfxActorFall); //this should be objFall sfx

                    //we should use a throw sound fx that is diff than falling sound
                }

                //scale obj down if it's colliding with a pit
                Object.compSprite.scale -= 0.03f;

                //slightly pull obj towards pit's center
                Object.compMove.magnitude = (Pit.compSprite.position - Object.compSprite.position) * 0.25f;

                //when obj drops below a threshold scale, release it
                if (Object.compSprite.scale < 0.8f)
                {
                    Functions_Pool.Release(Object);
                    PlayPitFx(Pit);
                }
            }
        }

        public static void FlipSwitchBlocks(InteractiveObject SwitchBtn)
        {
            Assets.Play(Assets.sfxSwitch);
            Functions_Particle.Spawn(
                ParticleType.Attention,
                SwitchBtn.compSprite.position.X,
                SwitchBtn.compSprite.position.Y);
            for (i = 0; i < Pool.intObjCount; i++)
            {   //loop thru all active roomObjects
                if (Pool.intObjPool[i].active)
                {   //flip blocks up or down
                    if (Pool.intObjPool[i].type == InteractiveType.Dungeon_SwitchBlockDown)
                    { SetType(Pool.intObjPool[i], InteractiveType.Dungeon_SwitchBlockUp); }
                    else if (Pool.intObjPool[i].type == InteractiveType.Dungeon_SwitchBlockUp)
                    { SetType(Pool.intObjPool[i], InteractiveType.Dungeon_SwitchBlockDown); }
                    //display particle fx at block location
                    if (Pool.intObjPool[i].type == InteractiveType.Dungeon_SwitchBlockDown
                        || Pool.intObjPool[i].type == InteractiveType.Dungeon_SwitchBlockUp)
                    {
                        Functions_Particle.Spawn(
                            ParticleType.Attention,
                            Pool.intObjPool[i].compSprite.position.X,
                            Pool.intObjPool[i].compSprite.position.Y);
                    }
                }
            }
        }

        public static void LightTorch(InteractiveObject UnlitTorch)
        {   //light the unlit torch
            SetType(UnlitTorch, InteractiveType.TorchLit);
            Functions_Particle.Spawn(
                ParticleType.Fire,
                UnlitTorch.compSprite.position.X + 0,
                UnlitTorch.compSprite.position.Y - 7,
                Direction.None);
            Assets.Play(Assets.sfxLightFire);
            Functions_Room.CheckForPuzzles(false); //may of solved room
        }

        public static void UnlightTorch(InteractiveObject LitTorch)
        {   //light the unlit torch
            SetType(LitTorch, InteractiveType.TorchUnlit);
            Functions_Particle.Spawn(
                ParticleType.Attention,
                LitTorch.compSprite.position.X + 0,
                LitTorch.compSprite.position.Y - 7);
            Assets.Play(Assets.sfxActorLand);
            Functions_Room.CheckForPuzzles(false); //may of solved room
        }

        public static void PlayPitFx(InteractiveObject Pit)
        {   //place splash 'centered' to pit
            Functions_Particle.Spawn(
                ParticleType.Splash,
                Pit.compSprite.position.X,
                Pit.compSprite.position.Y - 4);
        }

        public static void SlideOnIce(ComponentMovement compMove)
        {   //set the component's friction to ice (slides)
            compMove.friction = World.frictionIce;
            //clip magnitude's maximum values for ice (reduces max speed)
            if (compMove.magnitude.X > 1) { compMove.magnitude.X = 1; }
            else if (compMove.magnitude.X < -1) { compMove.magnitude.X = -1; }
            if (compMove.magnitude.Y > 1) { compMove.magnitude.Y = 1; }
            else if (compMove.magnitude.Y < -1) { compMove.magnitude.Y = -1; }
        }

        public static void UseFairy(InteractiveObject Fairy)
        {
            Pool.hero.health = PlayerData.heartsTotal; //effect
            Assets.Play(Assets.sfxHeartPickup); //sfx
            if (Fairy != null) //kill fairy
            {
                Functions_Particle.Spawn(
                    ParticleType.Attention,
                    Fairy.compSprite.position.X,
                    Fairy.compSprite.position.Y);
                Functions_Pool.Release(Fairy);
            }
        }

        public static void DropMap(float X, float Y)
        {   //a map drop only comes from a miniboss death in a hub room
            if (
                LevelSet.currentLevel.currentRoom.roomID == RoomID.ForestIsland_HubRoom ||
                LevelSet.currentLevel.currentRoom.roomID == RoomID.DeathMountain_HubRoom ||
                LevelSet.currentLevel.currentRoom.roomID == RoomID.SwampIsland_HubRoom
                )
            {   //a map will only spawn if hero doesn't have the map
                if (LevelSet.currentLevel.map == false)
                {
                    Spawn(InteractiveType.Dungeon_Map, (int)X, (int)Y, Direction.Down);
                }
            }
        }

        



        //maps obj to dungeon door data

        public static void SetDungeonDoor(InteractiveObject Door)
        {   //update the dungeon.doors list, change colliding door to bombed
            for (int i = 0; i < LevelSet.currentLevel.doors.Count; i++)
            {   //if this explosion collides with any dungeon.door that is of type.bombable
                if (LevelSet.currentLevel.doors[i].type == DoorType.Bombable)
                {   //change this door type to type.bombed
                    if (LevelSet.currentLevel.doors[i].rec.Contains(
                        Door.compSprite.position.X, Door.compSprite.position.Y))
                    { LevelSet.currentLevel.doors[i].type = DoorType.Open; }
                }
            }
        }











        public static void SetType(InteractiveObject IntObj, InteractiveType Type)
        {   //Obj.direction should be set prior to this method running
            IntObj.type = Type;

            //these objs inherit their texture sheets from prev obj state
            if (IntObj.type == InteractiveType.ExplodingObject) { }
            //default to the common objs sheet - obj def will overwrite this if needed
            else { IntObj.compSprite.texture = Assets.CommonObjsSheet; }
            //obj is either on common objs sheet, or dungeon sheet
            //some obj enemies maybe on enemies sheet


            #region Unknown Obj

            if (Type == InteractiveType.Unknown)
            {
                Reset(IntObj);
                IntObj.type = InteractiveType.Unknown;
                IntObj.compCollision.blocking = false;
                IntObj.compSprite.texture = Assets.uiItemsSheet;
                IntObj.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Unknown;
                IntObj.compSprite.zOffset = -64; //sort below everything else
            }

            #endregion








            //Dungeon Objs

            #region Walls

            //basic straight walls clean themselves once up post-birth 
            else if (Type == InteractiveType.Dungeon_WallStraight)
            {
                IntObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IntObj.compSprite.zOffset = -32; //sort very low (behind hero)
                IntObj.group = InteractiveGroup.Wall_Dungeon;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_WallStraight;
                //these objs clean themselves up in interactions, set this state
                IntObj.selfCleans = true;
            }
            else if (Type == InteractiveType.Dungeon_WallStraightCracked)
            {
                IntObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IntObj.compSprite.zOffset = -32; //sort very low (behind hero)
                IntObj.group = InteractiveGroup.Wall_Dungeon;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_WallStraightCracked;
                //these objs clean themselves up in interactions, set this state
                IntObj.selfCleans = true;
            }

            //these can overlap straight walls, causing straight walls to self-remove (clean)
            else if (Type == InteractiveType.Dungeon_WallInteriorCorner)
            {
                IntObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IntObj.compSprite.zOffset = -32; //sort very low (behind hero)
                IntObj.group = InteractiveGroup.Wall_Dungeon;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_WallInteriorCorner;
            }
            else if (Type == InteractiveType.Dungeon_WallExteriorCorner)
            {
                IntObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IntObj.compSprite.zOffset = -32; //sort very low (behind hero)
                IntObj.group = InteractiveGroup.Wall_Dungeon;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_WallExteriorCorner;
            }
            else if (Type == InteractiveType.Dungeon_WallPillar)
            {
                IntObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IntObj.compSprite.zOffset = -32; //sort very low (behind hero)
                IntObj.group = InteractiveGroup.Wall_Dungeon;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_WallPillar;
                IntObj.selfCleans = true;
            }
            else if (Type == InteractiveType.Dungeon_WallStatue)
            {
                IntObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IntObj.compSprite.zOffset = -16; //sort low, but over walls
                IntObj.group = InteractiveGroup.Wall_Dungeon;
                IntObj.interacts = true; //obj gets AI
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_WallStatue;
            }
            else if (Type == InteractiveType.Dungeon_WallTorch)
            {
                IntObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IntObj.compSprite.zOffset = -16; //sort low, but over walls
                IntObj.group = InteractiveGroup.Wall_Dungeon;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_WallTorch;
                IntObj.selfCleans = true;
            }

            #endregion


            #region Fake Dungeon Doors

            else if (Type == InteractiveType.Dungeon_DoorFake)
            {
                IntObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IntObj.compSprite.zOffset = -32; //sort very low (behind hero)
                IntObj.group = InteractiveGroup.Door_Dungeon;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_WallStraightCracked;
            }

            #endregion


            #region Doors  

            if (Type == InteractiveType.Dungeon_DoorOpen)
            {
                IntObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IntObj.compSprite.zOffset = +32; //sort very high (over / in front of hero)
                IntObj.group = InteractiveGroup.Door_Dungeon;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_DoorOpen;
                IntObj.selfCleans = true;
            }
            else if (Type == InteractiveType.Dungeon_DoorTrap)
            {
                IntObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IntObj.compCollision.blocking = false;
                IntObj.compSprite.zOffset = -32; //sort very low (behind hero)
                IntObj.group = InteractiveGroup.Door_Dungeon;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_DoorShut;
                IntObj.sfx.hit = Assets.sfxTapMetallic;
            }
            else if (Type == InteractiveType.Dungeon_DoorBombable)
            {
                IntObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IntObj.compSprite.zOffset = -32; //sort very low (behind hero)
                IntObj.group = InteractiveGroup.Door_Dungeon;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_WallStraightCracked;
                IntObj.sfx.hit = Assets.sfxTapHollow; //sounds hollow
            }
            else if (Type == InteractiveType.Dungeon_DoorBoss)
            {
                IntObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IntObj.compSprite.zOffset = -32; //sort very low (behind hero)
                IntObj.group = InteractiveGroup.Door_Dungeon;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_DoorBoss;
                IntObj.sfx.hit = Assets.sfxTapMetallic;
                IntObj.sfx.kill = Assets.sfxShatter;
            }
            else if (Type == InteractiveType.Dungeon_DoorShut)
            {
                IntObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IntObj.compSprite.zOffset = -32; //sort very low (behind hero)
                IntObj.group = InteractiveGroup.Door_Dungeon;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_DoorShut;
                IntObj.sfx.hit = Assets.sfxTapMetallic;
                IntObj.selfCleans = true;
            }

            #endregion


            #region Boss Floor Decal

            else if (Type == InteractiveType.Dungeon_FloorDecal)
            {
                IntObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is unique dungeonObj
                IntObj.compSprite.zOffset = -32; //sort low, but over floor
                IntObj.compCollision.blocking = false;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_FloorDecal;
            }

            #endregion


            #region Pit Teeth

            else if (Type == InteractiveType.Lava_PitTeethTop)
            {
                IntObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IntObj.compSprite.zOffset = -60; //sort above pits & pit bubbles
                IntObj.compCollision.rec.Width = 16; IntObj.compCollision.rec.Height = 8;
                IntObj.compCollision.blocking = false;
                IntObj.canBeSaved = true;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_PitTeethTop;
            }
            else if (Type == InteractiveType.Lava_PitTeethBottom)
            {
                IntObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IntObj.compSprite.zOffset = -60; //sort above pits & pit bubbles
                IntObj.compCollision.rec.Width = 16; IntObj.compCollision.rec.Height = 8;
                IntObj.compCollision.offsetY = 4;
                IntObj.compCollision.rec.Height = 4;
                IntObj.compCollision.blocking = false;
                IntObj.canBeSaved = true;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_PitTeethBottom;
            }

            #endregion


            #region Dungeon Statue

            else if (Type == InteractiveType.Dungeon_Statue)
            {
                IntObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IntObj.compCollision.rec.Height = 8;
                IntObj.compCollision.offsetY = -1;
                IntObj.canBeSaved = true;
                IntObj.compMove.moveable = true;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_Statue;
                IntObj.sfx.hit = Assets.sfxTapHollow;
                IntObj.sfx.kill = Assets.sfxShatter;
            }

            #endregion


            #region Pushable Block, Spike Block

            else if (Type == InteractiveType.Dungeon_BlockLight)
            {
                IntObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IntObj.compSprite.zOffset = -7;
                IntObj.canBeSaved = true;
                //lighter blocks are moveable
                IntObj.compMove.moveable = true;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_BlockLight;
                IntObj.sfx.hit = Assets.sfxTapMetallic;
            }
            else if (Type == InteractiveType.Dungeon_BlockSpike)
            {
                IntObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IntObj.compSprite.zOffset = -7;
                IntObj.compCollision.offsetX = -7; IntObj.compCollision.offsetY = -7;
                IntObj.compCollision.rec.Width = 14; IntObj.compCollision.rec.Height = 14;
                //set block's moving direction, based on facing direction
                IntObj.compMove.direction = IntObj.direction;
                IntObj.canBeSaved = true;
                IntObj.compMove.moveable = true;
                IntObj.compMove.grounded = false; //in air
                IntObj.compCollision.blocking = false;
                IntObj.compMove.speed = 0.225f; //spike blocks move med
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_BlockSpike;
                IntObj.sfx.hit = Assets.sfxTapMetallic;
            }

            #endregion


            #region Dungeon Pot (skull)

            else if (Type == InteractiveType.Dungeon_Pot)
            {
                IntObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IntObj.compCollision.offsetX = -5; IntObj.compCollision.offsetY = -3;
                IntObj.compCollision.rec.Width = 10; IntObj.compCollision.rec.Height = 11;
                IntObj.compSprite.zOffset = -7;
                IntObj.canBeSaved = true;
                IntObj.compMove.moveable = true;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_Pot;
                IntObj.sfx.hit = Assets.sfxEnemyHit;
                IntObj.sfx.kill = Assets.sfxShatter;
            }

            #endregion


            #region Floor Switches

            else if (Type == InteractiveType.Dungeon_Switch || Type == InteractiveType.Dungeon_SwitchDown
                || Type == InteractiveType.Dungeon_SwitchDownPerm)
            {
                IntObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IntObj.compCollision.offsetX = -4; IntObj.compCollision.offsetY = -3;
                IntObj.compCollision.rec.Width = 8; IntObj.compCollision.rec.Height = 8;
                IntObj.compCollision.blocking = false;
                IntObj.canBeSaved = true;
                IntObj.compSprite.zOffset = -32; //sort to floor
                //IntObj.compMove.moveable = true;
                if (Type == InteractiveType.Dungeon_Switch)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_FloorSwitchUp; }
                else { IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_FloorSwitchDown; }
                //this makes the switch work
                IntObj.interacts = true;
                if (Type == InteractiveType.Dungeon_SwitchDownPerm) { IntObj.interacts = false; }
            }

            #endregion


            #region Switch Blocks & Button

            else if (Type == InteractiveType.Dungeon_SwitchBlockBtn)
            {
                IntObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IntObj.compCollision.offsetX = -5; IntObj.compCollision.offsetY = -4;
                IntObj.compCollision.rec.Width = 10; IntObj.compCollision.rec.Height = 12;
                IntObj.compSprite.zOffset = -7;
                IntObj.canBeSaved = true;
                IntObj.compMove.moveable = true;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_SwitchBlockBtn;
                IntObj.sfx.hit = Assets.sfxEnemyHit;
            }
            else if (Type == InteractiveType.Dungeon_SwitchBlockDown)
            {
                IntObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IntObj.compSprite.zOffset = -32; //sort to floor
                IntObj.compCollision.blocking = false;
                IntObj.canBeSaved = true;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_SwitchBlockDown;
            }
            else if (Type == InteractiveType.Dungeon_SwitchBlockUp)
            {
                IntObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IntObj.compCollision.offsetX = -8; IntObj.compCollision.offsetY = -8;
                IntObj.compCollision.rec.Width = 16; IntObj.compCollision.rec.Height = 16;
                IntObj.compSprite.zOffset = -7; //sort normally
                IntObj.compCollision.blocking = true;
                IntObj.canBeSaved = true;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_SwitchBlockUp;
                IntObj.sfx.hit = Assets.sfxTapMetallic;
            }

            #endregion


            #region Floor Spikes

            else if (Type == InteractiveType.Dungeon_SpikesFloorOn || Type == InteractiveType.Dungeon_SpikesFloorOff)
            {
                IntObj.compCollision.offsetX = -5; IntObj.compCollision.offsetY = -5;
                IntObj.compCollision.rec.Width = 10; IntObj.compCollision.rec.Height = 10;
                IntObj.compSprite.zOffset = -32; //sort to floor
                IntObj.compCollision.blocking = false;
                IntObj.canBeSaved = true;
                if (Type == InteractiveType.Dungeon_SpikesFloorOn)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_FloorSpikesOn; }
                else { IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_FloorSpikesOff; }
            }

            #endregion

























            //Common Dungeon Objs

            #region Filled and Empty CHESTS

            else if (Type == InteractiveType.Chest ||
                Type == InteractiveType.ChestKey)
            {
                IntObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IntObj.compCollision.offsetX = -7; IntObj.compCollision.offsetY = -3;
                IntObj.compCollision.rec.Width = 14; IntObj.compCollision.rec.Height = 11;
                IntObj.compSprite.zOffset = -7;
                IntObj.canBeSaved = true;
                IntObj.compMove.moveable = true;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_ChestClosed;
                IntObj.sfx.hit = Assets.sfxTapHollow;
            }
            else if (Type == InteractiveType.ChestEmpty)
            {
                IntObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IntObj.compCollision.offsetX = -7; IntObj.compCollision.offsetY = -3;
                IntObj.compCollision.rec.Width = 14; IntObj.compCollision.rec.Height = 11;
                IntObj.compSprite.zOffset = -7;
                IntObj.canBeSaved = true;
                //IntObj.group = InteractiveGroup.Chest; //not really a chest, just obj
                IntObj.compMove.moveable = true;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_ChestOpened;
                IntObj.sfx.hit = Assets.sfxTapMetallic;
            }


            #endregion
            

            #region Torches (lit and unlit)

            else if (Type == InteractiveType.TorchUnlit || Type == InteractiveType.TorchLit)
            {
                IntObj.compCollision.offsetX = -8; IntObj.compCollision.offsetY = -4;
                IntObj.compCollision.rec.Width = 16; IntObj.compCollision.rec.Height = 12;
                IntObj.compSprite.zOffset = -7;
                IntObj.canBeSaved = true;
                IntObj.compMove.moveable = true;
                if (Type == InteractiveType.TorchUnlit)
                {
                    IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_TorchUnlit;
                    IntObj.interacts = true; //unlit torches check neighbors for fire
                    IntObj.interactiveFrame = 10; //only does this ever 10 frames
                }
                else
                {
                    IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_TorchLit;
                }
                IntObj.sfx.hit = Assets.sfxTapMetallic;
            }

            #endregion


            #region Living RoomObjects - Fairy

            else if (Type == InteractiveType.Fairy)
            {
                IntObj.compSprite.zOffset = 32; //sort to air
                IntObj.countTotal = 0; //stay around forever
                IntObj.compAnim.speed = 6; //in frames
                IntObj.compMove.moveable = true;
                IntObj.compCollision.blocking = false;
                IntObj.interacts = true;
                IntObj.compMove.grounded = false; //obj is flying
                IntObj.compMove.friction = World.frictionAir;
                IntObj.canBeSaved = true;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_Fairy;
                IntObj.sfx.hit = Assets.sfxEnemyHit;
            }

            #endregion


            #region Dungeon Map

            else if (Type == InteractiveType.Dungeon_Map)
            {
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_Map;
                IntObj.compCollision.offsetX = -5; IntObj.compCollision.offsetY = -5;
                IntObj.compCollision.rec.Width = 10; IntObj.compCollision.rec.Height = 10;
                IntObj.compSprite.zOffset = 0;
                //hero simply touches map to collect it
                IntObj.compCollision.blocking = false;
            }

            #endregion


            #region Levers and Conveyor Belts

            else if (Type == InteractiveType.LeverOn || Type == InteractiveType.LeverOff)
            {
                IntObj.compCollision.offsetX = -7; IntObj.compCollision.offsetY = 2;
                IntObj.compCollision.rec.Width = 12; IntObj.compCollision.rec.Height = 5;
                IntObj.compSprite.zOffset = -3;
                IntObj.canBeSaved = true;
                if (Type == InteractiveType.LeverOn)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_LeverOn; }
                else { IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_LeverOff; }
            }
            else if (Type == InteractiveType.ConveyorBeltOn || Type == InteractiveType.ConveyorBeltOff)
            {
                IntObj.compSprite.zOffset = -31; //sort just above floor
                IntObj.compAnim.speed = 10; //in frames
                IntObj.compCollision.blocking = false;
                IntObj.canBeSaved = true;
                if (Type == InteractiveType.ConveyorBeltOn)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_ConveyorBeltOn; }
                else { IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_ConveyorBeltOff; }
            }

            #endregion


            #region Barrel, Bumper, Flamethrower

            else if (Type == InteractiveType.Barrel)
            {
                IntObj.compCollision.offsetX = -5; IntObj.compCollision.offsetY = -5;
                IntObj.compCollision.rec.Width = 10; IntObj.compCollision.rec.Height = 13;
                IntObj.compSprite.zOffset = -7;
                IntObj.canBeSaved = true;
                IntObj.compMove.moveable = true;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_Barrel;
                IntObj.sfx.hit = Assets.sfxEnemyHit;
            }
            else if (Type == InteractiveType.Bumper)
            {
                IntObj.compCollision.blocking = false;
                IntObj.canBeSaved = true;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_Bumper;
            }
            else if (Type == InteractiveType.Flamethrower)
            {
                IntObj.compSprite.zOffset = -30; //sort slightly above floor
                IntObj.compCollision.offsetX = -4; IntObj.compCollision.offsetY = -4;
                IntObj.compCollision.rec.Width = 8; IntObj.compCollision.rec.Height = 8;
                IntObj.compCollision.blocking = false;
                IntObj.interacts = true; //obj gets AI
                IntObj.canBeSaved = true;
                IntObj.compMove.moveable = true;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_Flamethrower;
            }

            #endregion



            #region LAVA Pit, Pit Bridge, Trap Pit

            else if (Type == InteractiveType.Lava_Pit)
            {   //this pit interacts with actor
                IntObj.compSprite.zOffset = -64; //sort under pit teeth
                IntObj.compCollision.offsetX = -5; IntObj.compCollision.offsetY = -5;
                IntObj.compCollision.rec.Width = 10; IntObj.compCollision.rec.Height = 10;
                IntObj.compCollision.blocking = false;
                IntObj.canBeSaved = true;
                IntObj.interacts = true;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_Pit;
            }
            else if (Type == InteractiveType.Lava_PitBridge)
            {
                IntObj.compCollision.blocking = false;
                IntObj.compSprite.zOffset = -64; //sort to floor
                IntObj.canBeSaved = true;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_PitBridge;
            }
            else if (Type == InteractiveType.Lava_PitTrap)
            {   //this becomes a pit upon collision with hero
                IntObj.compCollision.offsetX = -12; IntObj.compCollision.offsetY = -12;
                IntObj.compCollision.rec.Width = 24; IntObj.compCollision.rec.Height = 24;
                IntObj.compSprite.zOffset = -32; //sort to floor
                IntObj.compCollision.blocking = false;
                IntObj.canBeSaved = true;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_FloorCracked;
                //randomly horizontally flip for more variation
                if (Functions_Random.Int(0, 101) > 50)
                { IntObj.compSprite.flipHorizontally = true; }
            }

            #endregion


            #region Enemy Spawn Objects

            else if (Type == InteractiveType.Dungeon_SpawnMob)
            {
                IntObj.compSprite.zOffset = -32; //sort to floor
                IntObj.compCollision.blocking = false;
                IntObj.group = InteractiveGroup.EnemySpawn;
                IntObj.canBeSaved = true;
                if (Type == InteractiveType.Dungeon_SpawnMob)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_SpawnMob; }
                else { IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_SpawnMiniBoss; }
            }

            #endregion






            #region Floor Decorations (stain, blood, skeletons, debris)

            else if (
                Type == InteractiveType.FloorStain
                || Type == InteractiveType.FloorBlood
                || Type == InteractiveType.FloorSkeleton
                )
            {   
                //collision rec is smaller so more debris is left when room is cleanedUp()
                //also allows for more blood, skeletons, etc.. upon deaths
                IntObj.compCollision.offsetX = -2; IntObj.compCollision.offsetY = -2;
                IntObj.compCollision.rec.Width = 4; IntObj.compCollision.rec.Height = 4;
                IntObj.compCollision.blocking = false;

                //reset cell size
                IntObj.compSprite.drawRec.Width = 16 * 1;
                IntObj.compSprite.drawRec.Height = 16 * 1;

                //default is floor stain
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_FloorStain;
                IntObj.compSprite.zOffset = -32; //sort over floors

                if (Type == InteractiveType.FloorBlood)
                {
                    IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_FloorBlood;
                    IntObj.compSprite.zOffset = -31; //sort over stains
                }
                else if (Type == InteractiveType.FloorSkeleton)
                {   //randomly choose skeleton anim frame to draw
                    IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_FloorSkeleton1;
                    if (Functions_Random.Int(0, 101) > 50)
                    { IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_FloorSkeleton2; }
                    IntObj.compSprite.zOffset = -30; //sort over blood
                }

                //randomly horizontally flip for more variation
                if (Functions_Random.Int(0, 101) > 50)
                { IntObj.compSprite.flipHorizontally = true; }

                IntObj.selfCleans = true; //selfclean next frame
                IntObj.canBeSaved = true;
            }
            else if (Type == InteractiveType.Debris)
            {
                //collision rec is smaller so more debris is left when room is cleanedUp()
                //also allows for more blood, skeletons, etc.. upon deaths
                IntObj.compCollision.offsetX = -2; IntObj.compCollision.offsetY = -2;
                IntObj.compCollision.rec.Width = 4; IntObj.compCollision.rec.Height = 4;
                IntObj.compCollision.blocking = false;

                //reset cell size
                IntObj.compSprite.drawRec.Width = 16 * 1;
                IntObj.compSprite.drawRec.Height = 16 * 1;
                
                //sort over blood and skeletons
                IntObj.compSprite.zOffset = -29;
                //randomly choose animFrame
                if (Functions_Random.Int(0, 100) < 50)
                { IntObj.compAnim.currentAnimation = AnimationFrames.World_Debris1; }
                else { IntObj.compAnim.currentAnimation = AnimationFrames.World_Debris2; }
                //randomly flip sprite horizontally
                if (Functions_Random.Int(0, 100) < 50)
                { IntObj.compSprite.flipHorizontally = true; }
                else { IntObj.compSprite.flipHorizontally = false; }

                IntObj.selfCleans = true; //selfclean next frame
                IntObj.canBeSaved = true;
            }

            #endregion


            #region Ice Tiles

            else if (Type == InteractiveType.IceTile)
            {
                IntObj.compCollision.offsetX = -6; IntObj.compCollision.offsetY = -6;
                IntObj.compCollision.rec.Width = 12; IntObj.compCollision.rec.Height = 12;
                IntObj.compSprite.zOffset = -28; //sort over most floor decor
                IntObj.compCollision.blocking = false;
                IntObj.canBeSaved = true;
                IntObj.compAnim.currentAnimation = AnimationFrames.Dungeon_IceTile;
                IntObj.selfCleans = true; //selfclean next frame
                IntObj.compMove.moveable = false; //cannot be moved by wind/belts ever
            }

            #endregion

















            //World Objs

            #region Grass Objects

            else if (
                Type == InteractiveType.Grass_2
                || Type == InteractiveType.Grass_Cut 
                || Type == InteractiveType.Grass_Tall
                || Type == InteractiveType.Flowers
                || Type == InteractiveType.Grass_Burned
                )
            {
                IntObj.compSprite.zOffset = -32;
                IntObj.compCollision.blocking = false;
                IntObj.canBeSaved = true;
                IntObj.compAnim.loop = true;

                //set animation frame
                if (Type == InteractiveType.Grass_Cut)
                {
                    IntObj.compAnim.currentAnimation = AnimationFrames.World_Grass_Short;
                }
                else if (Type == InteractiveType.Grass_2)
                {
                    IntObj.compAnim.currentAnimation = AnimationFrames.World_Grass_Minimum;
                }
                else if (Type == InteractiveType.Grass_Tall)
                {
                    IntObj.compAnim.currentAnimation = AnimationFrames.World_Grass_Tall;
                    IntObj.sfx.kill = Assets.sfxBushCut; //only tall grass can get killed()
                }
                else if (Type == InteractiveType.Flowers)
                {
                    IntObj.compAnim.currentAnimation = AnimationFrames.World_Flowers;
                    //randomly set the starting frame for flowers, so their animations dont sync up
                    IntObj.compAnim.index = (byte)Functions_Random.Int(0, IntObj.compAnim.currentAnimation.Count);
                }
                else if(Type == InteractiveType.Grass_Burned)
                {
                    IntObj.compAnim.currentAnimation = AnimationFrames.World_Grass_Burned;
                }

                //randomly flip sprite horizontally
                if (Functions_Random.Int(0, 100) < 50)
                { IntObj.compSprite.flipHorizontally = true; }
                else { IntObj.compSprite.flipHorizontally = false; }
            }

            #endregion


            #region Bush and Bush Stump

            else if (Type == InteractiveType.Bush)
            {
                IntObj.canBeSaved = true;
                IntObj.compSprite.zOffset = -2;
                IntObj.compCollision.blocking = true;
                IntObj.compAnim.currentAnimation = AnimationFrames.World_Bush;
                IntObj.compCollision.offsetX = -6; IntObj.compCollision.offsetY = -2;
                IntObj.compCollision.rec.Width = 12; IntObj.compCollision.rec.Height = 8;
                IntObj.sfx.kill = Assets.sfxBushCut;
                IntObj.sfx.hit = Assets.sfxEnemyHit;
            }
            else if (Type == InteractiveType.Bush_Stump)
            {
                IntObj.canBeSaved = true;
                IntObj.compSprite.zOffset = -24;
                IntObj.compCollision.blocking = false;
                IntObj.compAnim.currentAnimation = AnimationFrames.World_BushStump;
                IntObj.compCollision.offsetX = -2; IntObj.compCollision.offsetY = -2;
                IntObj.compCollision.rec.Width = 5; IntObj.compCollision.rec.Height = 5;

                IntObj.interacts = true; //stumps grow to become bushes if irrigated
                //how quickly stumps check for nearby filled ditches
                IntObj.interactiveFrame = 254; //last possible frame
            }

            #endregion


            #region Trees

            else if (Type == InteractiveType.Tree
                || Type == InteractiveType.Tree_Burning
                || Type == InteractiveType.Tree_Burnt)
            {
                IntObj.compSprite.drawRec.Height = 16 * 2; //nonstandard size
                IntObj.canBeSaved = true;
                IntObj.compSprite.zOffset = +10;
                IntObj.compCollision.blocking = true;
                IntObj.compCollision.offsetX = -5; IntObj.compCollision.offsetY = 15;
                IntObj.compCollision.rec.Width = 10; IntObj.compCollision.rec.Height = 8;
                IntObj.sfx.hit = Assets.sfxEnemyHit;
                IntObj.sfx.kill = Assets.sfxBushCut;
                //set correct animFrame based on type
                if (Type == InteractiveType.Tree)
                { IntObj.compAnim.currentAnimation = AnimationFrames.World_Tree; }
                else if (Type == InteractiveType.Tree_Burning)
                {   //same as tree, just covered in fire objs
                    IntObj.compAnim.currentAnimation = AnimationFrames.World_Tree;
                    IntObj.counter = 0;
                    IntObj.countTotal = 150; //"burn" for this long
                    IntObj.interacts = true; //will spawn fire on tree
                }
                else //burning tree becomes burnt version eventually
                {
                    IntObj.compAnim.currentAnimation = AnimationFrames.World_TreeBurnt;
                    IntObj.interacts = true; //randomly smokes
                }
            }
            else if (Type == InteractiveType.Tree_Stump)
            {
                IntObj.compSprite.drawRec.Height = 16 * 2; //nonstandard size
                IntObj.canBeSaved = true;
                IntObj.compSprite.zOffset = +10;
                IntObj.compCollision.blocking = false;
                IntObj.compAnim.currentAnimation = AnimationFrames.World_TreeStump;
                IntObj.compCollision.offsetX = -5; IntObj.compCollision.offsetY = 15;
                IntObj.compCollision.rec.Width = 10; IntObj.compCollision.rec.Height = 8;
                IntObj.sfx.hit = Assets.sfxTapMetallic;
            }

            #endregion


            #region Pot

            else if (Type == InteractiveType.Pot) //same as Dungeon_Pot
            {
                IntObj.compCollision.offsetX = -5; IntObj.compCollision.offsetY = -3;
                IntObj.compCollision.rec.Width = 10; IntObj.compCollision.rec.Height = 11;
                IntObj.compSprite.zOffset = -7;
                IntObj.canBeSaved = true;
                IntObj.compMove.moveable = true;
                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Pot;
                IntObj.sfx.hit = Assets.sfxEnemyHit;
                IntObj.sfx.kill = Assets.sfxShatter;
            }

            #endregion


            #region House Objects

            else if (Type == InteractiveType.House_Bookcase
                || Type == InteractiveType.House_Shelf
                || Type == InteractiveType.House_Stove
                || Type == InteractiveType.House_Sink
                || Type == InteractiveType.House_Chair)
            {
                IntObj.compCollision.offsetY = 0; IntObj.compCollision.rec.Height = 8;
                IntObj.compSprite.zOffset = -7;
                IntObj.canBeSaved = true;
                IntObj.compMove.moveable = true;

                if (Type == InteractiveType.House_Bookcase)
                { IntObj.compAnim.currentAnimation = AnimationFrames.World_Bookcase; }
                else if (Type == InteractiveType.House_Shelf)
                { IntObj.compAnim.currentAnimation = AnimationFrames.World_Shelf; }

                else if (Type == InteractiveType.House_Stove)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Stove; }
                else if (Type == InteractiveType.House_Sink)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Sink; }
                else if (Type == InteractiveType.House_Chair)
                {
                    IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Chair;
                    IntObj.compCollision.offsetX = -5; IntObj.compCollision.rec.Width = 10;
                    IntObj.compCollision.offsetY = -5; IntObj.compCollision.rec.Height = 10;
                }

                IntObj.sfx.hit = Assets.sfxTapMetallic;
                IntObj.sfx.kill = Assets.sfxShatter;
            }
            else if (Type == InteractiveType.House_TableSingle)
            {
                IntObj.canBeSaved = true;
                IntObj.compMove.moveable = true;
                IntObj.sfx.hit = Assets.sfxTapMetallic;
                IntObj.sfx.kill = Assets.sfxShatter;
                IntObj.compSprite.zOffset = -7;
                IntObj.compCollision.offsetX = -7; IntObj.compCollision.offsetY = -6;
                IntObj.compCollision.rec.Width = 14; IntObj.compCollision.rec.Height = 12;
                IntObj.compAnim.currentAnimation = AnimationFrames.World_TableSingle;
            }

            else if (Type == InteractiveType.House_TableDoubleLeft
                || Type == InteractiveType.House_TableDoubleRight)
            {
                IntObj.canBeSaved = true;
                //IntObj.compMove.moveable = true;
                IntObj.sfx.hit = Assets.sfxTapMetallic;
                IntObj.sfx.kill = Assets.sfxShatter;
                IntObj.compSprite.zOffset = -7;

                if (Type == InteractiveType.House_TableDoubleLeft)
                {
                    IntObj.compAnim.currentAnimation = AnimationFrames.World_TableDoubleLeft;
                    IntObj.compCollision.offsetX = -7; IntObj.compCollision.offsetY = -6;
                    IntObj.compCollision.rec.Width = 14 + 16; IntObj.compCollision.rec.Height = 12;
                }
                else
                {
                    IntObj.compAnim.currentAnimation = AnimationFrames.World_TableDoubleRight;
                    IntObj.compCollision.offsetX = -7 - 16; IntObj.compCollision.offsetY = -6;
                    IntObj.compCollision.rec.Width = 14 + 16; IntObj.compCollision.rec.Height = 12;
                }
            }

            else if (Type == InteractiveType.House_Bed)
            {
                IntObj.compSprite.drawRec.Height = 16 * 2; //non standard cellsize
                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Bed;
                IntObj.compCollision.offsetX = -8; IntObj.compCollision.offsetY = 0;
                IntObj.compCollision.rec.Width = 16; IntObj.compCollision.rec.Height = 3 + 16;
                IntObj.compSprite.zOffset = 0;
                IntObj.canBeSaved = true;
                IntObj.sfx.hit = null;
                IntObj.sfx.kill = null;
            }

            #endregion


            #region Building WALLS & DOORS

            else if (Type == InteractiveType.House_Wall_FrontA
                || Type == InteractiveType.House_Wall_FrontB
                || Type == InteractiveType.House_Wall_Back)
            {
                IntObj.canBeSaved = true;
                IntObj.sfx.hit = Assets.sfxTapMetallic;
                IntObj.compSprite.zOffset = 0; //sort over sidewalls
                //note - these hitboxes are custom for a reason
                IntObj.compCollision.rec.Width = 16; IntObj.compCollision.offsetX = -8;
                IntObj.compCollision.rec.Height = 4; IntObj.compCollision.offsetY = +4;
                //set anim frame based on type
                if (Type == InteractiveType.House_Wall_FrontA)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Build_Wall_FrontA; }
                else if (Type == InteractiveType.House_Wall_FrontB)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Build_Wall_FrontB; }
                else { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Build_Wall_Back; }
            }
            else if (Type == InteractiveType.House_Wall_Side_Left)
            {
                IntObj.canBeSaved = true;
                IntObj.sfx.hit = Assets.sfxTapMetallic;
                IntObj.compSprite.zOffset = 1;
                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Build_Wall_Side_Left;
                //note - these hitboxes are custom for a reason
                IntObj.compCollision.rec.Width = 3; IntObj.compCollision.offsetX = -8;
                IntObj.compCollision.rec.Height = 16; IntObj.compCollision.offsetY = -8;
            }
            else if (Type == InteractiveType.House_Wall_Side_Right)
            {
                IntObj.canBeSaved = true;
                IntObj.sfx.hit = Assets.sfxTapMetallic;
                IntObj.compSprite.zOffset = 1;
                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Build_Wall_Side_Right;
                //note - these hitboxes are custom for a reason
                IntObj.compCollision.rec.Width = 3; IntObj.compCollision.offsetX = +5;
                IntObj.compCollision.rec.Height = 16; IntObj.compCollision.offsetY = -8;
            }
            else if (Type == InteractiveType.House_Door_Shut)
            {
                IntObj.canBeSaved = true;
                IntObj.compSprite.zOffset = 0;
                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Build_Door_Shut;
                //note - these hitboxes are custom for a reason
                IntObj.compCollision.rec.Width = 16; IntObj.compCollision.offsetX = -8;
                IntObj.compCollision.rec.Height = 4; IntObj.compCollision.offsetY = +4;
            }
            else if (Type == InteractiveType.House_Door_Open)
            {
                IntObj.canBeSaved = true;
                IntObj.compSprite.zOffset = 0;
                IntObj.compCollision.blocking = false;
                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Build_Door_Open;
            }

            #endregion


            #region Roofs

            else if (Type == InteractiveType.House_Roof_Bottom
                || Type == InteractiveType.House_Roof_Top)
            {
                IntObj.canBeSaved = true;
                IntObj.compSprite.zOffset = 48; //sort over most other things
                IntObj.compCollision.blocking = false;

                if (Type == InteractiveType.House_Roof_Bottom)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Build_Roof_Bottom; }
                else { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Build_Roof_Top; }
            }
            else if (Type == InteractiveType.House_Roof_Chimney)
            {
                IntObj.canBeSaved = true;
                IntObj.compSprite.zOffset = 48; //sort over most other things
                IntObj.compCollision.blocking = false;
                IntObj.interacts = true; //create smoke
                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Build_Roof_Chimney;
            }
            else if (Type == InteractiveType.House_Roof_Collapsing)
            {
                //this obj inherits the attributes of the previous roof obj it once was
                //this means we dont modify the attributes at all

                IntObj.interacts = true; //but collapsing roofs always get AI
                IntObj.interactiveFrame = 20; //base speed of collapse spread
                IntObj.interactiveFrame += Functions_Random.Int(-10, 10); //vary spreadtime
                IntObj.counter = 0; //reset counter
            }

            #endregion


            #region Water Objects

            else if (Type == InteractiveType.Water_1x1)
            {
                IntObj.compSprite.zOffset = -128;
                IntObj.compCollision.blocking = false;
                IntObj.canBeSaved = true;
                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Water_1x1;
                //double size
                IntObj.compSprite.drawRec.Width = 16 * 1;
                IntObj.compSprite.drawRec.Height = 16 * 1;
                IntObj.compCollision.rec.Width = 16 * 1;
                IntObj.compCollision.rec.Height = 16 * 1;
                IntObj.compCollision.offsetX = -8;
                IntObj.compCollision.offsetY = -8;
            }
            else if (Type == InteractiveType.Water_2x2)
            {   //use less pool objs by being bigger
                IntObj.compSprite.zOffset = -128;
                IntObj.compCollision.blocking = false;
                IntObj.canBeSaved = true;
                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Water_2x2;
                //double size
                IntObj.compSprite.drawRec.Width = 16 * 2;
                IntObj.compSprite.drawRec.Height = 16 * 2;
                IntObj.compCollision.rec.Width = 16 * 2;
                IntObj.compCollision.rec.Height = 16 * 2;
                IntObj.compCollision.offsetX = -8;
                IntObj.compCollision.offsetY = -8;
            }
            else if (Type == InteractiveType.Water_3x3)
            {   //use less pool objs by being bigger
                IntObj.compSprite.zOffset = -128;
                IntObj.compCollision.blocking = false;
                IntObj.canBeSaved = true;
                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Water_3x3;
                //triple size
                IntObj.compSprite.drawRec.Width = 16 * 3;
                IntObj.compSprite.drawRec.Height = 16 * 3;
                IntObj.compCollision.rec.Width = 16 * 3;
                IntObj.compCollision.rec.Height = 16 * 3;
                IntObj.compCollision.offsetX = -8;
                IntObj.compCollision.offsetY = -8;
            }


            

            else if (
                Type == InteractiveType.Coastline_Straight
                || Type == InteractiveType.Coastline_Corner_Exterior
                || Type == InteractiveType.Coastline_Corner_Interior
                )
            {
                IntObj.compSprite.zOffset = -100;
                IntObj.compCollision.blocking = false;
                IntObj.canBeSaved = true;
                IntObj.compAnim.loop = true;

                if (Type == InteractiveType.Coastline_Straight)
                {
                    if (Functions_Random.Int(0, 100) > 50) //randomly choose coastline
                    { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Coastline_Straight_A; }
                    else { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Coastline_Straight_B; }
                }
                else if (Type == InteractiveType.Coastline_Corner_Exterior)
                {
                    IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Coastline_Corner_Exterior;
                }
                else if (Type == InteractiveType.Coastline_Corner_Interior)
                {
                    IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Coastline_Corner_Interior;
                }
            }
            else if (Type == InteractiveType.Water_RockUnderwater)
            {
                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Water_RockUnderwater;
                IntObj.canBeSaved = true;
                IntObj.compSprite.zOffset = -12; //sort under hero
                IntObj.compCollision.blocking = false; //just decoration
            }
            else if (Type == InteractiveType.Water_LillyPad)
            {   //nonstandard size
                IntObj.compSprite.drawRec.Width = 16 * 2; IntObj.compSprite.drawRec.Height = 16 * 2;
                IntObj.compCollision.rec.Width = 16 * 2; IntObj.compCollision.offsetX = -8;
                IntObj.compCollision.rec.Height = 16 * 2; IntObj.compCollision.offsetY = -8;
                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Swamp_LillyPad;
                IntObj.compSprite.zOffset = -39; //sorts just above water (-40)
                IntObj.canBeSaved = true;
                IntObj.compCollision.blocking = false;
            }
            else if (Type == InteractiveType.Water_Vine)
            {   //nonstandard size
                IntObj.compSprite.drawRec.Width = 16 * 2; IntObj.compSprite.drawRec.Height = 16 * 1;
                IntObj.compCollision.rec.Width = 16 * 2; IntObj.compCollision.offsetX = -8;
                IntObj.compCollision.rec.Height = 12; IntObj.compCollision.offsetY = -6;
                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Swamp_Vine;
                IntObj.compSprite.zOffset = 0;
                IntObj.canBeSaved = true;
                IntObj.compCollision.blocking = false;
            }


            else if (Type == InteractiveType.Coastline_1x2_Animated)
            {   //nonstandard size
                IntObj.compSprite.drawRec.Width = 16 * 1; IntObj.compSprite.drawRec.Height = 16 * 2;
                IntObj.compCollision.rec.Width = 16 * 1; IntObj.compCollision.offsetX = -8;
                IntObj.compCollision.rec.Height = 16 * 2; IntObj.compCollision.offsetY = -8;
                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Coastline_Long;
                IntObj.canBeSaved = true;
                IntObj.compCollision.blocking = false;
                IntObj.compSprite.zOffset = -64; //over water, coastline objs
            }


            else if (
                Type == InteractiveType.Water_RockSm
                || Type == InteractiveType.Water_RockMed
                )
            {
                IntObj.compSprite.zOffset = -7; //sort under hero
                //setup hitbox
                IntObj.compCollision.offsetX = -5; IntObj.compCollision.rec.Width = 10;
                IntObj.compCollision.offsetY = -5; IntObj.compCollision.rec.Height = 10;
                //setup animFrame
                if (Type == InteractiveType.Water_RockSm)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Water_RockSm; }
                else
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Water_RockMed; }
                IntObj.canBeSaved = true;
            }

            else if (Type == InteractiveType.Water_BigPlant)
            {   //nonstandard size
                IntObj.compSprite.drawRec.Width = 16 * 2; IntObj.compSprite.drawRec.Height = 16 * 2;
                IntObj.compCollision.rec.Width = 16 * 2; IntObj.compCollision.offsetX = -8;
                IntObj.compCollision.rec.Height = 16 * 2; IntObj.compCollision.offsetY = -8;
                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Swamp_BigPlant;
                IntObj.compSprite.zOffset = 6; //has height
                IntObj.canBeSaved = true;
            }
            else if (Type == InteractiveType.Water_Bulb)
            {
                IntObj.compCollision.rec.Width = 6; IntObj.compCollision.offsetX = -3;
                IntObj.compCollision.rec.Height = 4; IntObj.compCollision.offsetY = 2;
                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Swamp_Bulb;
                IntObj.compSprite.zOffset = 1;
                IntObj.canBeSaved = true;
            }
            else if (Type == InteractiveType.Water_SmPlant)
            {
                IntObj.compCollision.rec.Width = 10; IntObj.compCollision.offsetX = -5;
                IntObj.compCollision.rec.Height = 4; IntObj.compCollision.offsetY = 2;
                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Swamp_SmPlant;
                IntObj.compSprite.zOffset = 1;
                IntObj.canBeSaved = true;
            }

            #endregion


            #region Ditches

            else if (
                Type == InteractiveType.Ditch_META

                || Type == InteractiveType.Ditch_Empty_Single
                || Type == InteractiveType.Ditch_Empty_4UP
                || Type == InteractiveType.Ditch_Empty_Vertical
                || Type == InteractiveType.Ditch_Empty_Horizontal

                || Type == InteractiveType.Ditch_Empty_Corner_North
                || Type == InteractiveType.Ditch_Empty_Corner_South
                || Type == InteractiveType.Ditch_Empty_3UP_North
                || Type == InteractiveType.Ditch_Empty_3UP_South

                || Type == InteractiveType.Ditch_Empty_3UP_Horizontal
                || Type == InteractiveType.Ditch_Empty_Endcap_South
                || Type == InteractiveType.Ditch_Empty_Endcap_Horizontal
                || Type == InteractiveType.Ditch_Empty_Endcap_North
                )
            {   //^ well that's efficient

                //ditches use the interacts boolean for two purposes:
                //1. to model the empty and filled states of ditches
                //2. to spread water from filled to empty ditches via Ai.HandleObj()
                //so the empty state of a ditch is interacts = false
                IntObj.interacts = false;

                //this sets how often filled ditches spread to empty ditches
                IntObj.interactiveFrame = 15;
                //IntObj.interactiveFrame += Functions_Random.Int(-10, 10); //vary spread times

                IntObj.counter = 0; //used by AI.handleObj to count to interactive frame
                IntObj.countTotal = 255; //not used in this case, but set to max value for safety

                IntObj.compCollision.rec.Width = 16;
                IntObj.compCollision.rec.Height = 16;
                IntObj.compCollision.offsetX = -8;
                IntObj.compCollision.offsetY = -8;

                IntObj.canBeSaved = true;
                IntObj.direction = Direction.Down;
                IntObj.compCollision.blocking = false;
                IntObj.compSprite.zOffset = -64; //sort under everything

                IntObj.group = InteractiveGroup.Ditch;

                if (Type == InteractiveType.Ditch_META)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Empty_Single; }

                //empty ditch animFrames
                else if (Type == InteractiveType.Ditch_Empty_Single)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Empty_Single; }
                else if (Type == InteractiveType.Ditch_Empty_4UP)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Empty_4UP; }
                else if (Type == InteractiveType.Ditch_Empty_Vertical)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Empty_Vertical; }
                else if (Type == InteractiveType.Ditch_Empty_Horizontal)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Empty_Horizontal; }

                else if (Type == InteractiveType.Ditch_Empty_Corner_North)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Empty_Corner_North; }
                else if (Type == InteractiveType.Ditch_Empty_Corner_South)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Empty_Corner_South; }
                else if (Type == InteractiveType.Ditch_Empty_3UP_North)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Empty_3UP_North; }
                else if (Type == InteractiveType.Ditch_Empty_3UP_South)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Empty_3UP_South; }

                else if (Type == InteractiveType.Ditch_Empty_3UP_Horizontal)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Empty_3UP_Horizontal; }
                else if (Type == InteractiveType.Ditch_Empty_Endcap_South)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Empty_Endcap_South; }
                else if (Type == InteractiveType.Ditch_Empty_Endcap_Horizontal)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Empty_Endcap_Horizontal; }
                else if (Type == InteractiveType.Ditch_Empty_Endcap_North)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Empty_Endcap_North; }
            }

            #endregion


            #region Posts - unburned, burned, hammer only

            else if (
                Type == InteractiveType.Post_VerticalLeft
                || Type == InteractiveType.Post_CornerLeft
                || Type == InteractiveType.PostBurned_VerticalLeft
                || Type == InteractiveType.PostBurned_CornerLeft
                )
            {
                IntObj.canBeSaved = true;
                IntObj.sfx.hit = Assets.sfxTapMetallic;
                IntObj.compSprite.zOffset = 0;
                IntObj.compCollision.rec.Width = 8; IntObj.compCollision.offsetX = 0;
                IntObj.compCollision.rec.Height = 8; IntObj.compCollision.offsetY = 0;

                //animframes
                if (Type == InteractiveType.Post_VerticalLeft)
                {
                    IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Post_Vertical_Left;
                    IntObj.compCollision.rec.Height = 16; //fills gap
                }
                else if (Type == InteractiveType.Post_CornerLeft)
                {
                    IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Post_Corner_Left;
                }
                else if (Type == InteractiveType.PostBurned_VerticalLeft)
                {   //burned posts have different hitboxes
                    IntObj.compAnim.currentAnimation = AnimationFrames.Wor_PostBurned_Corner_Left;
                    IntObj.compCollision.rec.Height = 4; IntObj.compCollision.offsetY = 3;
                }
                else if (Type == InteractiveType.PostBurned_CornerLeft)
                {
                    IntObj.compAnim.currentAnimation = AnimationFrames.Wor_PostBurned_Corner_Left;
                    IntObj.compCollision.rec.Height = 4; IntObj.compCollision.offsetY = 3;
                }
            }

            else if (
                Type == InteractiveType.Post_VerticalRight
                || Type == InteractiveType.Post_CornerRight
                || Type == InteractiveType.PostBurned_VerticalRight
                || Type == InteractiveType.PostBurned_CornerRight
                )
            {
                IntObj.canBeSaved = true;
                IntObj.sfx.hit = Assets.sfxTapMetallic;
                IntObj.compSprite.zOffset = 0;
                IntObj.compCollision.rec.Width = 8; IntObj.compCollision.offsetX = -8;
                IntObj.compCollision.rec.Height = 8; IntObj.compCollision.offsetY = 0;

                //animframes
                if (Type == InteractiveType.Post_VerticalRight)
                {
                    IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Post_Vertical_Right;
                    IntObj.compCollision.rec.Height = 16; //fills gap
                }
                else if (Type == InteractiveType.Post_CornerRight)
                {
                    IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Post_Corner_Right;
                }
                else if (Type == InteractiveType.PostBurned_VerticalRight)
                {   //burned posts have different hitboxes
                    IntObj.compAnim.currentAnimation = AnimationFrames.Wor_PostBurned_Vertical_Right;
                    IntObj.compCollision.rec.Height = 4; IntObj.compCollision.offsetY = 3;
                }
                else if (Type == InteractiveType.PostBurned_CornerRight)
                {
                    IntObj.compAnim.currentAnimation = AnimationFrames.Wor_PostBurned_Corner_Right;
                    IntObj.compCollision.rec.Height = 4; IntObj.compCollision.offsetY = 3;
                }
            }

            else if (Type == InteractiveType.Post_Horizontal
                || Type == InteractiveType.PostBurned_Horizontal)
            {
                IntObj.canBeSaved = true;
                IntObj.sfx.hit = Assets.sfxTapMetallic;
                IntObj.compSprite.zOffset = 0;

                //hitbox - same for burned and non burned
                IntObj.compCollision.rec.Width = 16; IntObj.compCollision.offsetX = -8;
                IntObj.compCollision.rec.Height = 8; IntObj.compCollision.offsetY = 0;
                //animFrame
                if (Type == InteractiveType.Post_Horizontal)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Post_Horizontal; }
                else { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_PostBurned_Horizontal; }
            }

            else if (
                Type == InteractiveType.Post_HammerPost_Up
                || Type == InteractiveType.Post_HammerPost_Down
                )
            {
                IntObj.canBeSaved = true;

                IntObj.sfx.hit = Assets.sfxTapMetallic; //tells player cant be destroyed
                IntObj.sfx.kill = Assets.sfxHammerPost; //dat' lttp flavor

                IntObj.compCollision.rec.Width = 8; IntObj.compCollision.offsetX = -4;
                IntObj.compCollision.rec.Height = 8; IntObj.compCollision.offsetY = 0;

                //animframes
                if (Type == InteractiveType.Post_HammerPost_Up)
                {   //resetObj() sets blocking to true, so up posts block
                    IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Post_Hammer_Up;
                    IntObj.compSprite.zOffset = 0;
                }
                else
                {   //make down posts passable
                    IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Post_Hammer_Down;
                    IntObj.compCollision.blocking = false;
                    IntObj.compSprite.zOffset = -32; //same offset as blood
                }
            }

            #endregion


            #region Signpost

            else if (Type == InteractiveType.Signpost)
            {
                IntObj.compCollision.offsetY = +4; IntObj.compCollision.rec.Height = 4;
                IntObj.compSprite.zOffset = +4;
                IntObj.compAnim.currentAnimation = AnimationFrames.Signpost;
                IntObj.sfx.hit = Assets.sfxTapMetallic;
                IntObj.canBeSaved = true;
            }

            #endregion


            #region Dirt and Dirt Transition Tiles

            else if (
                Type == InteractiveType.Dirt_Main ||
                Type == InteractiveType.Dirt_ToGrass_Straight ||
                Type == InteractiveType.Dirt_ToGrass_Corner_Exterior ||
                Type == InteractiveType.Dirt_ToGrass_Corner_Interior
                )
            {   //sort above water, below everything else
                IntObj.compSprite.zOffset = -40;
                IntObj.canBeSaved = true;
                IntObj.compCollision.blocking = false;

                //set anim frame, based on type some anim frames are random
                if (Type == InteractiveType.Dirt_Main)
                {
                    if (Functions_Random.Int(0, 101) > 50)
                    { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Dirt_A; }
                    else { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Dirt_B; }
                }
                else if (Type == InteractiveType.Dirt_ToGrass_Straight)
                {
                    if (Functions_Random.Int(0, 101) > 50)
                    { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_DirtToGrass_Straight_A; }
                    else { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_DirtToGrass_Straight_B; }
                }
                else if (Type == InteractiveType.Dirt_ToGrass_Corner_Exterior)
                {
                    IntObj.compAnim.currentAnimation = AnimationFrames.Wor_DirtToGrass_Corner_Exterior;
                }
                else if (Type == InteractiveType.Dirt_ToGrass_Corner_Interior)
                {
                    IntObj.compAnim.currentAnimation = AnimationFrames.Wor_DirtToGrass_Corner_Interior;
                }
            }

            #endregion

            





            //Vendors & NPCs

            #region Vendors

            //Vendors
            else if (
                Type == InteractiveType.Vendor_Items
                || Type == InteractiveType.Vendor_Potions
                || Type == InteractiveType.Vendor_Magic
                || Type == InteractiveType.Vendor_Weapons
                || Type == InteractiveType.Vendor_Armor
                || Type == InteractiveType.Vendor_Equipment
                || Type == InteractiveType.Vendor_Pets
                )
            {
                IntObj.compCollision.offsetX = -7; IntObj.compCollision.offsetY = -3;
                IntObj.compCollision.rec.Width = 14; IntObj.compCollision.rec.Height = 11;
                IntObj.compSprite.zOffset = 0;
                IntObj.compAnim.speed = 20; //slow animation
                IntObj.group = InteractiveGroup.Vendor;
                IntObj.canBeSaved = true;

                if (Type == InteractiveType.Vendor_Items) { IntObj.compAnim.currentAnimation = AnimationFrames.Vendor_Items; }
                else if (Type == InteractiveType.Vendor_Potions) { IntObj.compAnim.currentAnimation = AnimationFrames.Vendor_Potions; }
                else if (Type == InteractiveType.Vendor_Magic) { IntObj.compAnim.currentAnimation = AnimationFrames.Vendor_Magic; }
                else if (Type == InteractiveType.Vendor_Weapons) { IntObj.compAnim.currentAnimation = AnimationFrames.Vendor_Weapons; }
                else if (Type == InteractiveType.Vendor_Armor) { IntObj.compAnim.currentAnimation = AnimationFrames.Vendor_Armor; }
                else if (Type == InteractiveType.Vendor_Equipment) { IntObj.compAnim.currentAnimation = AnimationFrames.Vendor_Equipment; }
                else if (Type == InteractiveType.Vendor_Pets) { IntObj.compAnim.currentAnimation = AnimationFrames.Vendor_Pets; }
            }
            else if (
                Type == InteractiveType.Vendor_Colliseum_Mob 
                || Type == InteractiveType.Vendor_EnemyItems
                )
            {
                IntObj.compCollision.offsetX = -7; IntObj.compCollision.offsetY = -3;
                IntObj.compCollision.rec.Width = 14; IntObj.compCollision.rec.Height = 11;
                IntObj.compSprite.zOffset = 0;
                IntObj.compAnim.speed = 20; //slow animation
                IntObj.group = InteractiveGroup.Vendor;
                IntObj.canBeSaved = true;

                if (Type == InteractiveType.Vendor_Colliseum_Mob)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Vendor_Colliseum_Mob; }
                else if (Type == InteractiveType.Vendor_EnemyItems)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Vendor_EnemyItems; }
            }

            #endregion


            #region NPCs

            //story NPC
            else if (Type == InteractiveType.NPC_Story)
            {
                IntObj.compCollision.offsetX = -7; IntObj.compCollision.offsetY = -3;
                IntObj.compCollision.rec.Width = 14; IntObj.compCollision.rec.Height = 11;
                IntObj.compSprite.zOffset = 0;
                IntObj.compAnim.speed = 20; //slow animation
                IntObj.group = InteractiveGroup.NPC;
                IntObj.canBeSaved = true;
                IntObj.compAnim.currentAnimation = AnimationFrames.Vendor_Story;
            }

            //farmer NPC
            else if (Type == InteractiveType.NPC_Farmer
                || Type == InteractiveType.NPC_Farmer_Reward
                || Type == InteractiveType.NPC_Farmer_EndDialog)
            {
                //only end state of farmer is 'dumb'
                if (Type == InteractiveType.NPC_Farmer_EndDialog) { IntObj.interacts = false; }
                else { IntObj.interacts= true; } //initial + reward states get AI

                if (Type == InteractiveType.NPC_Farmer) //initial farmer checks often for
                { IntObj.interactiveFrame = 30; } //completion condition, while reward
                else { IntObj.interactiveFrame = 60; } //farmer gently spams exclamation point

                IntObj.compCollision.offsetX = -7; IntObj.compCollision.offsetY = -3;
                IntObj.compCollision.rec.Width = 14; IntObj.compCollision.rec.Height = 11;
                IntObj.compSprite.zOffset = 0;
                IntObj.compAnim.speed = 20; //slow animation
                IntObj.group = InteractiveGroup.NPC;
                IntObj.canBeSaved = true;

                if (Type == InteractiveType.NPC_Farmer_Reward) //only reward has unique anim
                { IntObj.compAnim.currentAnimation = AnimationFrames.NPC_Farmer_HandsUp; }
                else { IntObj.compAnim.currentAnimation = AnimationFrames.NPC_Farmer; }
            }

            //colliseum judge
            else if (Type == InteractiveType.Judge_Colliseum)
            {
                IntObj.compAnim.currentAnimation = AnimationFrames.Vendor_Colliseum_Mob;
                IntObj.compCollision.offsetX = -7; IntObj.compCollision.offsetY = -3;
                IntObj.compCollision.rec.Width = 14; IntObj.compCollision.rec.Height = 11;
                IntObj.compSprite.zOffset = 0;
                IntObj.compAnim.speed = 20; //slow animation
                IntObj.group = InteractiveGroup.NPC;
                IntObj.canBeSaved = false;

                //judge handles challenge completions
                IntObj.interacts = true;
                IntObj.interactiveFrame = 60;
            }


            #endregion







            //Colliseum Objects

            #region Gates

            else if (Type == InteractiveType.Coliseum_Shadow_Gate_Center)
            {
                IntObj.compSprite.drawRec.Width = 16 * 1; //nonstandard size
                IntObj.compSprite.drawRec.Height = 16 * 4; //nonstandard size
                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Gate_Center;
                //non-blocking - made for easy grabbing
                IntObj.compCollision.rec.Width = 16 * 1; IntObj.compCollision.offsetX = -8;
                IntObj.compCollision.rec.Height = 16 * 2; IntObj.compCollision.offsetY = -4;
                //sort save and block
                IntObj.compSprite.zOffset = +16 * 4;
                IntObj.canBeSaved = true;
                IntObj.compCollision.blocking = false;
                IntObj.sfx.hit = Assets.sfxTapMetallic;
            }
            else if (Type == InteractiveType.Coliseum_Shadow_Gate_Pillar_Left
                || Type == InteractiveType.Coliseum_Shadow_Gate_Pillar_Right)
            {
                IntObj.compSprite.drawRec.Width = 16 * 2; //nonstandard size
                IntObj.compSprite.drawRec.Height = 16 * 4; //nonstandard size
                if (Type == InteractiveType.Coliseum_Shadow_Gate_Pillar_Left)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Gate_Pillar_Left; }
                else { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Gate_Pillar_Right; }
                //rec base
                IntObj.compCollision.rec.Width = 16 * 2; IntObj.compCollision.offsetX = -8;
                IntObj.compCollision.rec.Height = 16 * 2 - 4; IntObj.compCollision.offsetY = +22;
                //sort save and block
                IntObj.compSprite.zOffset = 16 + 8;
                IntObj.canBeSaved = true;
                IntObj.compCollision.blocking = true;
                IntObj.sfx.hit = Assets.sfxTapMetallic;
            }

            #endregion


            #region Coliseum Stairs

            else if (
                Type == InteractiveType.Coliseum_Shadow_Stairs_Left
                || Type == InteractiveType.Coliseum_Shadow_Stairs_Middle
                || Type == InteractiveType.Coliseum_Shadow_Stairs_Right
                )
            {
                IntObj.compSprite.zOffset = -16;
                IntObj.canBeSaved = true;
                IntObj.compCollision.blocking = false;

                if (Type == InteractiveType.Coliseum_Shadow_Stairs_Left)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Stairs_Left; }
                else if (Type == InteractiveType.Coliseum_Shadow_Stairs_Middle)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Stairs_Middle; }
                else if (Type == InteractiveType.Coliseum_Shadow_Stairs_Right)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Stairs_Right; }

                IntObj.sfx.hit = null;
                IntObj.sfx.kill = null;
            }

            #endregion


            #region Floors

            else if (
                Type == InteractiveType.Coliseum_Shadow_Outdoors_Floor
                )
            {
                IntObj.compSprite.zOffset = -64; //floortile
                IntObj.canBeSaved = true;
                IntObj.compCollision.blocking = false;

                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Outdoors_Floor;
                IntObj.sfx.hit = null;
                IntObj.sfx.kill = null;
            }

            #endregion








            //Forest Objects

            #region None Yet



            #endregion









            //Mountain Objects


            #region MountainWalls

            else if (Type == InteractiveType.MountainWall_Top)
            {   //nonstandard size
                IntObj.compSprite.drawRec.Width = 16 * 4; IntObj.compSprite.drawRec.Height = 16 * 1;
                IntObj.compCollision.rec.Width = 16 * 4; IntObj.compCollision.offsetX = -8;
                IntObj.compCollision.rec.Height = 16 * 1; IntObj.compCollision.offsetY = -8;

                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_MountainWall_Top;
                IntObj.compSprite.zOffset = -18; //sorts under footholds
                IntObj.canBeSaved = true;
                IntObj.compCollision.blocking = false;
                //IntObj.group = InteractiveGroup.Wall_Climbable; //do not do this!
            }
            else if (Type == InteractiveType.MountainWall_Mid)
            {   //nonstandard size
                IntObj.compSprite.drawRec.Width = 16 * 4; IntObj.compSprite.drawRec.Height = 16 * 2;
                IntObj.compCollision.rec.Width = 16 * 4; IntObj.compCollision.offsetX = -8;
                IntObj.compCollision.rec.Height = 16 * 2; IntObj.compCollision.offsetY = -8;

                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_MountainWall_Mid;
                IntObj.compSprite.zOffset = -18; //sorts under footholds
                IntObj.canBeSaved = true;
                IntObj.compCollision.blocking = false;
                IntObj.group = InteractiveGroup.Wall_Climbable;
            }
            else if (Type == InteractiveType.MountainWall_Bottom)
            {   //nonstandard size
                IntObj.compSprite.drawRec.Width = 16 * 4; IntObj.compSprite.drawRec.Height = 16 * 1;
                IntObj.compCollision.rec.Width = 16 * 4; IntObj.compCollision.offsetX = -8;
                IntObj.compCollision.rec.Height = 16 * 1; IntObj.compCollision.offsetY = -8;

                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_MountainWall_Bottom;
                IntObj.compSprite.zOffset = -18; //sorts under footholds
                IntObj.canBeSaved = true;
                IntObj.compCollision.blocking = false;
                IntObj.group = InteractiveGroup.Wall_Climbable;
            }

            #endregion


            #region Footholds

            else if (Type == InteractiveType.MountainWall_Foothold
                || Type == InteractiveType.MountainWall_Ladder
                || Type == InteractiveType.MountainWall_Ladder_Trap)
            {
                IntObj.compSprite.drawRec.Width = 16 * 1; IntObj.compSprite.drawRec.Height = 16 * 1;
                IntObj.compCollision.rec.Width = 20; IntObj.compCollision.offsetX = -10;
                IntObj.compCollision.rec.Height = 20; IntObj.compCollision.offsetY = -10;

                if (Type == InteractiveType.MountainWall_Foothold)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_MountainWall_Foothold; }
                else if (Type == InteractiveType.MountainWall_Ladder)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_MountainWall_Ladder; }
                else { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_MountainWall_Ladder_Trap; }

                IntObj.compSprite.zOffset = -16; //sorts under hero
                IntObj.canBeSaved = true;
                IntObj.compCollision.blocking = false;
                IntObj.group = InteractiveGroup.Wall_Climbable;

                //randomly flip sprite horizontally
                if (Functions_Random.Int(0, 100) < 50)
                { IntObj.compSprite.flipHorizontally = true; }
                else { IntObj.compSprite.flipHorizontally = false; }
            }

            #endregion











            //Swamp Objects

            #region None Yet


            #endregion











            //Boat objs

            #region Boat Stairs (off boat)

            //non-blocking stairs (int obj)
            else if (Type == InteractiveType.Boat_Stairs_Left
                || Type == InteractiveType.Boat_Stairs_Right)
            {   //nonstandard size
                IntObj.compSprite.drawRec.Width = 16 * 2; IntObj.compSprite.drawRec.Height = 16 * 1;
                IntObj.compCollision.rec.Width = 16 * 2; IntObj.compCollision.offsetX = -8;
                IntObj.compCollision.rec.Height = 16; IntObj.compCollision.offsetY = -8;

                if (Type == InteractiveType.Boat_Stairs_Right)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Stairs_Right; }
                else
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Stairs_Left; }

                IntObj.compSprite.zOffset = -32; //sort above water
                IntObj.canBeSaved = true;
                IntObj.compCollision.blocking = false;
                IntObj.sfx.hit = Assets.sfxTapMetallic;
            }

            #endregion


            #region Floor, Barrel, Stairs Into Ship, Stairs Cover

            else if (Type == InteractiveType.Boat_Floor)
            {
                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Floor;
                //-33 is minimum layer setting for a floor object
                IntObj.compSprite.zOffset = -40; //sort above water
                IntObj.canBeSaved = true;
                IntObj.compCollision.blocking = false;
            }
            else if (Type == InteractiveType.Boat_Floor_Burned)
            {
                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Floor_Burned;
                //-33 is minimum layer setting for a floor object
                IntObj.compSprite.zOffset = -40; //sort above water
                IntObj.canBeSaved = true;
                IntObj.compCollision.blocking = false;
            }

            else if (Type == InteractiveType.Boat_Barrel)
            {
                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Barrel;
                IntObj.compCollision.rec.Width = 12; IntObj.compCollision.offsetX = -6;
                IntObj.compCollision.rec.Height = 10; IntObj.compCollision.offsetY = -5 + 3;

                IntObj.compSprite.zOffset = 0; //sort above water
                IntObj.canBeSaved = true;
                IntObj.compCollision.blocking = true;
                IntObj.compMove.moveable = true;
                IntObj.sfx.hit = Assets.sfxEnemyHit;
                IntObj.sfx.kill = Assets.sfxShatter;
            }

            else if (Type == InteractiveType.Boat_Stairs_Cover)
            {
                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Stairs_Cover;
                IntObj.compCollision.rec.Width = 16; IntObj.compCollision.offsetX = -8;
                IntObj.compCollision.rec.Height = 16; IntObj.compCollision.offsetY = -8;

                IntObj.compSprite.zOffset = -24; //sort below hero
                IntObj.canBeSaved = true;
                IntObj.compCollision.blocking = false;
            }

            #endregion


            #region Captain Brandy

            else if (Type == InteractiveType.Boat_Captain_Brandy)
            {
                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Captain_Brandy;
                IntObj.compCollision.rec.Width = 16; IntObj.compCollision.offsetX = -8;
                IntObj.compCollision.rec.Height = 16; IntObj.compCollision.offsetY = -8;

                IntObj.compSprite.zOffset = 0; //sorts normally
                IntObj.canBeSaved = true;
                IntObj.compCollision.blocking = true;

                IntObj.group = InteractiveGroup.NPC;
                IntObj.interacts = true;
                IntObj.interactiveFrame = 60;
            }

            #endregion


            #region Boat Bridge, Pier

            else if (Type == InteractiveType.Boat_Bridge_Top)
            {   //nonstandard size
                IntObj.compSprite.drawRec.Width = 16 * 2; IntObj.compSprite.drawRec.Height = 16 * 1;
                IntObj.compCollision.rec.Width = 16 * 2; IntObj.compCollision.offsetX = -8;
                IntObj.compCollision.rec.Height = 2; IntObj.compCollision.offsetY = -8 - 2;
                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Bridge_Top;
                IntObj.compSprite.zOffset = -16;
                IntObj.canBeSaved = true;
                IntObj.compCollision.blocking = true;
            }
            else if (Type == InteractiveType.Boat_Bridge_Bottom)
            {   //nonstandard size
                IntObj.compSprite.drawRec.Width = 16 * 2; IntObj.compSprite.drawRec.Height = 16 * 1;
                IntObj.compCollision.rec.Width = 16 * 2; IntObj.compCollision.offsetX = -8;
                IntObj.compCollision.rec.Height = 14; IntObj.compCollision.offsetY = -8;
                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Bridge_Bottom;
                IntObj.compSprite.zOffset = -16;
                IntObj.canBeSaved = true;
                IntObj.compCollision.blocking = true;
            }
            else if (
                Type == InteractiveType.Boat_Pier_TopLeft
                || Type == InteractiveType.Boat_Pier_TopMiddle
                || Type == InteractiveType.Boat_Pier_TopRight

                || Type == InteractiveType.Boat_Pier_Left
                || Type == InteractiveType.Boat_Pier_Middle
                || Type == InteractiveType.Boat_Pier_Right

                || Type == InteractiveType.Boat_Pier_BottomLeft
                || Type == InteractiveType.Boat_Pier_BottomMiddle
                || Type == InteractiveType.Boat_Pier_BottomRight
                )
            {
                IntObj.compCollision.rec.Width = 16; IntObj.compCollision.offsetX = -8;
                IntObj.compCollision.rec.Height = 16; IntObj.compCollision.offsetY = -8;
                IntObj.compSprite.zOffset = -15;
                IntObj.canBeSaved = true;
                IntObj.compCollision.blocking = false;

                if (Type == InteractiveType.Boat_Pier_TopLeft)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Pier_TopLeft; }
                else if (Type == InteractiveType.Boat_Pier_TopMiddle)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Pier_TopMiddle; }
                else if (Type == InteractiveType.Boat_Pier_TopRight)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Pier_TopRight; }

                else if (Type == InteractiveType.Boat_Pier_Left)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Pier_Left; }
                else if (Type == InteractiveType.Boat_Pier_Middle)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Pier_Middle; }
                else if (Type == InteractiveType.Boat_Pier_Right)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Pier_Right; }
                
                else if (Type == InteractiveType.Boat_Pier_BottomLeft)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Pier_BottomLeft; }
                else if (Type == InteractiveType.Boat_Pier_BottomMiddle)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Pier_BottomMiddle; }
                else if (Type == InteractiveType.Boat_Pier_BottomRight)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Pier_BottomRight; }
            }

            #endregion

            







            //Living Objects

            #region Enemies

            //these enemies are roomObj enemies, simple ai, 1 hit mobs
            //standard, miniboss, and boss enemies are ACTORS and handled
            //via Ai Functions.

            else if (
                Type == InteractiveType.Enemy_Turtle
                || Type == InteractiveType.Enemy_Crab
                )
            {
                IntObj.group = InteractiveGroup.Enemy;
                IntObj.interacts = true; //roomObj enemy
                IntObj.canBeSaved = true;
                IntObj.compSprite.zOffset = 0;
                IntObj.compMove.moveable = true;
                //setup hitbox
                IntObj.compCollision.blocking = true;
                IntObj.compCollision.offsetX = -6; IntObj.compCollision.rec.Width = 12;
                IntObj.compCollision.offsetY = -3; IntObj.compCollision.rec.Height = 8;
                //setup sfx
                IntObj.sfx.hit = Assets.sfxEnemyHit;
                IntObj.sfx.kill = Assets.sfxEnemyKill;
                //setup animFrame
                if (Type == InteractiveType.Enemy_Turtle)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Enemy_Turtle; }
                else if (Type == InteractiveType.Enemy_Crab)
                { IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Enemy_Crab; }
            }

            else if (Type == InteractiveType.Enemy_Rat)
            {
                IntObj.group = InteractiveGroup.Enemy;
                IntObj.interacts = true; //roomObj enemy
                IntObj.canBeSaved = true;
                IntObj.compSprite.zOffset = 0;
                IntObj.compMove.moveable = true;
                //setup hitbox
                IntObj.compCollision.blocking = true;
                IntObj.compCollision.offsetX = -5; IntObj.compCollision.rec.Width = 10;
                IntObj.compCollision.offsetY = -5; IntObj.compCollision.rec.Height = 10;
                //setup sfx
                IntObj.sfx.hit = Assets.sfxEnemyHit;
                IntObj.sfx.kill = Assets.sfxEnemyKill;
                //setup animFrame - defaults to down
                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_Enemy_Rat_Down;
            }

            //unique enemies - their texture is specific
            else if (Type == InteractiveType.Enemy_SeekerExploder)
            {
                IntObj.group = InteractiveGroup.Enemy;
                IntObj.canBeSaved = true;
                IntObj.compSprite.zOffset = 0;
                IntObj.compMove.moveable = true;
                //setup hitbox
                IntObj.compCollision.blocking = false; //allow overlap
                IntObj.compCollision.offsetX = -5; IntObj.compCollision.rec.Width = 10;
                IntObj.compCollision.offsetY = -5; IntObj.compCollision.rec.Height = 10;
                //setup sfx
                IntObj.sfx.hit = Assets.sfxEnemyHit;
                IntObj.sfx.kill = Assets.sfxEnemyKill;
                //setup animFrame
                IntObj.compAnim.currentAnimation = AnimationFrames.Wor_SeekerExploder_Idle;
                IntObj.interacts = true; //seek to hero and explode
            }

            #endregion


            #region Pets

            else if (Type == InteractiveType.Pet_Dog)
            {   //smaller than normal 16x16 objs
                IntObj.compCollision.offsetX = -4; IntObj.compCollision.rec.Width = 8;
                IntObj.compCollision.offsetY = -4; IntObj.compCollision.rec.Height = 8;
                IntObj.compSprite.zOffset = -8; //this pet is lower to the ground

                IntObj.countTotal = 0; //stay around forever
                IntObj.compMove.moveable = true; //obj is moveable by belts
                IntObj.interacts = true; //obj gets ai too (track to hero, set anim frames)
                IntObj.compCollision.blocking = false; //dont block
                IntObj.compSprite.texture = Assets.petsSheet;
                IntObj.canBeSaved = true;
                //set initial animation frame
                IntObj.compAnim.currentAnimation = AnimationFrames.Pet_Dog_Idle;
            }
            else if (Type == InteractiveType.Pet_Chicken)
            {   //smaller than normal 16x16 objs
                IntObj.compCollision.offsetX = -4; IntObj.compCollision.rec.Width = 8;
                IntObj.compCollision.offsetY = -4; IntObj.compCollision.rec.Height = 8;
                IntObj.compSprite.zOffset = -8; //this pet is lower to the ground

                IntObj.countTotal = 0; //stay around forever
                IntObj.compMove.moveable = true; //obj is moveable by belts
                IntObj.interacts = true; //obj gets ai too (track to hero, set anim frames)

                IntObj.compCollision.blocking = false; //dont block
                IntObj.canBeSaved = true;
                //set initial animation frame
                IntObj.compAnim.currentAnimation = AnimationFrames.Pet_Chicken_Idle;
            }

            #endregion





            //Unique Objects

            #region Exploding Object

            else if (Type == InteractiveType.ExplodingObject)
            {
                //a hit barrel ends up here
                //a hit seekerExploder ends up here
                //this object inherits from many diff objs
                //but simply waits a few frames, then explodes
                //(after being pushed a little)

                //prep previous obj for explosion
                IntObj.interacts = true;
                IntObj.countTotal = 30; //in frames
                IntObj.counter = 0; //reset
                IntObj.group = InteractiveGroup.Object;
            }

            #endregion








            //Dialog Objects

            #region Hero

            else if (Type == InteractiveType.DialogObj_Hero_Idle)
            {
                IntObj.compAnim.currentAnimation = AnimationFrames.Hero_Animations.idle.down;
                //determine which texture to use when representing hero speaking to himself
                if (Pool.hero.type == ActorType.Blob)
                { IntObj.compSprite.texture = Assets.blobSheet; }
                else { IntObj.compSprite.texture = Assets.heroSheet; }
            }

            #endregion







            //Handle Group properties
            if (IntObj.group == InteractiveGroup.Enemy)
            {   //all enemies exist on enemy sheet
                IntObj.compSprite.texture = Assets.EnemySheet;
            }

            //finalize rotation, animation, and component alignment
            SetRotation(IntObj);
            IntObj.compSprite.currentFrame = IntObj.compAnim.currentAnimation[0]; //goto 1st anim frame
            Functions_Component.Align(IntObj.compMove, IntObj.compSprite, IntObj.compCollision);
        }
    }
}