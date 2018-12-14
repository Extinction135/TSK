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
    public static class Functions_Particle
    {
        static Vector2 posRef = new Vector2();
        static Vector2 pushOffset = new Vector2();




        //resets the particle to a default state
        public static void Reset(Particle Part)
        {
            //reset the Part
            Part.type = ParticleType.Attention; //reset the type
            Part.direction = Direction.Down;
            Part.active = true; //assume this object should draw / animate
            Part.lifetime = 0; //assume obj exists forever (not projectile)
            Part.lifeCounter = 0; //reset counter

            //reset the sprite component
            Part.compSprite.drawRec.Width = 16 * 1; //assume cell size is 16x16 (most are)
            Part.compSprite.drawRec.Height = 16 * 1;
            Part.compSprite.zOffset = 0;
            Part.compSprite.flipHorizontally = false;
            Part.compSprite.rotation = Rotation.None;
            Part.compSprite.scale = 1.0f;
            Part.compSprite.texture = Assets.CommonObjsSheet;
            Part.compSprite.visible = true;

            //reset the animation component
            Part.compAnim.speed = 10; //set obj's animation speed to default value
            Part.compAnim.loop = true; //assume obj's animation loops
            Part.compAnim.index = 0; //reset the current animation index/frame
            Part.compAnim.timer = 0; //reset the elapsed frames

            //reset the move component
            Part.compMove.magnitude.X = 0; //discard any previous magnitude
            Part.compMove.magnitude.Y = 0; //
            Part.compMove.speed = 0.0f; //assume this object doesn't move
            Part.compMove.friction = 0.75f; //normal friction
            Part.compMove.moveable = true; //
            Part.compMove.grounded = true; //most objects exist on the ground
        }

        //spawns a particle of Type on Projectile
        public static void Spawn(ParticleType Type, Projectile Pro)
        {
            //set position reference to sprite position
            posRef.X = Pro.compSprite.position.X;
            posRef.Y = Pro.compSprite.position.Y;


            #region Sword & Net

            if (Pro.type == ProjectileType.Sword || Pro.type == ProjectileType.Net)
            {
                if (Pro.direction == Direction.Up) { posRef.X += 8; posRef.Y -= 0; }
                else if (Pro.direction == Direction.Right) { posRef.X += 8; posRef.Y += 8; }
                else if (Pro.direction == Direction.Down) { posRef.X += 8; posRef.Y += 8; }
                else if (Pro.direction == Direction.Left) { posRef.X += 2; posRef.Y += 8; }
            }

            #endregion


            Spawn(Type, posRef.X, posRef.Y);
        }

        //spawns a particle of Type on RoomObject
        public static void Spawn(ParticleType Type, InteractiveObject Object)
        {
            //set position reference to sprite position
            posRef.X = Object.compSprite.position.X;
            posRef.Y = Object.compSprite.position.Y;


            #region Pit

            if (Object.type == InteractiveType.Lava_Pit)
            {   //randomly offset where the bubble particle is placed
                posRef.X += 4; posRef.Y += 4; //because bubble is 8x8 size
                posRef.X += Functions_Random.Int(-3, 4);
                posRef.Y += Functions_Random.Int(-3, 4);
            }

            #endregion


            #region SpikeBlock

            else if (Object.type == InteractiveType.Dungeon_BlockSpike)
            {
                posRef.X += 4; posRef.Y += 4;
                //place particle along colliding edge
                if (Object.compMove.direction == Direction.Up) { posRef.Y -= 6; }
                else if (Object.compMove.direction == Direction.Down) { posRef.Y += 6; }
                else if (Object.compMove.direction == Direction.Right) { posRef.X += 6; }
                else if (Object.compMove.direction == Direction.Left) { posRef.X -= 6; }
            }

            #endregion


            Spawn(Type, posRef.X, posRef.Y);
        }

        //spawns a particle of Type on Actor
        public static void Spawn(ParticleType Type, Actor Actor)
        {
            posRef.X = Actor.compSprite.position.X;
            posRef.Y = Actor.compSprite.position.Y;

            if (Type == ParticleType.RisingSmoke)
            {   //center horizontally, place near actor's feet
                posRef.X += 4; posRef.Y += 8;
            }

            Spawn(Type, posRef.X, posRef.Y);
        }

        //spawns a particle of Type at XY (other spawns() feed into this one)
        public static void Spawn(ParticleType Type, float X, float Y, Direction Dir = Direction.Down)
        {   //get a particle to spawn
            Particle part = Functions_Pool.GetParticle();
            part.compMove.moving = true;

            //set particles direction to passed direction
            part.direction = Dir;
            part.compMove.direction = Dir;

            //properly rotate water kick particles
            if (Type == ParticleType.WaterKick)
            {
                part.direction = Functions_Direction.GetOppositeDirection(Dir);
            }

            //teleport the object to the proper location
            Functions_Movement.Teleport(part.compMove, X, Y);
            //set the type, rotation, cellsize, & alignment
            SetType(part, Type);
            Functions_Component.Align(part); //align upon birth
                                            
  
            #region Handle Particle Birth Events

            //sfx for reward particles
            if (Type == ParticleType.RewardMap)
            {
                Assets.Play(Assets.sfxReward);
            }
            else if (Type == ParticleType.RewardKey)
            {
                Assets.Play(Assets.sfxKeyPickup);
            }

            //misc
            else if (Type == ParticleType.Splash)
            {
                Assets.Play(Assets.sfxSplash);
            }
            else if (Type == ParticleType.Push)
            {   //trails behind item used in cardinal direction
                Functions_Movement.Push(part.compMove, part.direction, 4.0f);
                //down is no rotation
                part.compSprite.rotation = Rotation.None;
                //set up, left, right
                if (Dir == Direction.Up)
                { part.compSprite.rotation = Rotation.Clockwise180; }
                else if (Dir == Direction.Left)
                { part.compSprite.rotation = Rotation.Clockwise90; }
                else if (Dir == Direction.Right)
                { part.compSprite.rotation = Rotation.Clockwise270; }
            }
            else if (Type == ParticleType.WaterKick)
            {
                Functions_Movement.Push(part.compMove, part.compMove.direction, 1.0f);
            }

            //these are individual pieces of an explosion that is circular or random
            else if (Type == ParticleType.LeafGreen || Type == ParticleType.DebrisBrown
                || Type == ParticleType.BloodRed || Type == ParticleType.SlimeGreen)
            {   
                Functions_Movement.Push(part.compMove, part.direction, 4.0f);
            }

            #endregion


            //Debug.WriteLine("particle made: " + Type + " - location: " + X + ", " + Y);
        }





        
        //per-frame logic
        public static void Update(Particle Obj)
        {
            if (Obj.lifetime == 0) { } //some particles live forever
            else
            {   //these particles 'die' after a lifetime
                Obj.lifeCounter++;
                if (Obj.lifeCounter >= Obj.lifetime) { Kill(Obj); }
            }
        }

        //death events
        public static void Kill(Particle Obj)
        {   //contains death events for particles
            Obj.lifeCounter = Obj.lifetime;
            //all objects are released upon death
            Functions_Pool.Release(Obj);
        }












        //misc methods

        public static void Spawn_Explosion(ParticleType Type, float X, float Y, Boolean circular = false)
        {  
            if(circular == false)
            {   //spawn 8 particles in random directions
                Spawn(Type, X, Y, Functions_Direction.GetRandomDirection());
                Spawn(Type, X, Y, Functions_Direction.GetRandomDirection());
                Spawn(Type, X, Y, Functions_Direction.GetRandomDirection());
                Spawn(Type, X, Y, Functions_Direction.GetRandomDirection());
                Spawn(Type, X, Y, Functions_Direction.GetRandomDirection());
                Spawn(Type, X, Y, Functions_Direction.GetRandomDirection());
                Spawn(Type, X, Y, Functions_Direction.GetRandomDirection());
                Spawn(Type, X, Y, Functions_Direction.GetRandomDirection());
            }
            else
            {   //spawn a circular burst of particles
                Spawn(Type, X, Y, Direction.Up);
                Spawn(Type, X, Y, Direction.UpRight);
                Spawn(Type, X, Y, Direction.Right);
                Spawn(Type, X, Y, Direction.DownRight);
                Spawn(Type, X, Y, Direction.Down);
                Spawn(Type, X, Y, Direction.DownLeft);
                Spawn(Type, X, Y, Direction.Left);
                Spawn(Type, X, Y, Direction.UpLeft);
            }
        }

        public static void SpawnPushFX(ComponentMovement Caster, Direction Dir)
        {
            pushOffset.X = 0; pushOffset.Y = 0; //reset offset
            if (Dir == Direction.Down) { pushOffset.X = 4; pushOffset.Y = +6; }
            else if (Dir == Direction.Up) { pushOffset.X = -4; pushOffset.Y = -7; }
            else if (Dir == Direction.Right) { pushOffset.X = +5; pushOffset.Y = -2; }
            else if (Dir == Direction.Left) { pushOffset.X = -5; pushOffset.Y = 6; }
            //place push lines 'after' projectile
            //a diagonal direction will hide the push line
            Spawn(ParticleType.Push,
                Caster.position.X + pushOffset.X,
                Caster.position.Y + pushOffset.Y, Dir);
        }








        //maps type to object values/state
        public static void SetType(Particle Part, ParticleType Type)
        {
            Part.type = Type;



            //Particles

            #region Dungeon Specific

            //these particle's sprites live on a dungeon sheet,
            //whichever dungeon sheet is the current one

            if (Type == ParticleType.PitBubble)
            {
                Part.compSprite.drawRec.Width = 8; Part.compSprite.drawRec.Height = 8; //nonstandard size
                Part.compSprite.zOffset = -63; //sort over pits, under pit teeth
                Part.lifetime = 10 * 4 * 6; //speed * anim frames * loops
                Part.compAnim.speed = 10; //in frames
                Part.compAnim.currentAnimation = AnimationFrames.Particle_PitBubble;
                //not on the entities sheet, on dungeon sheet
                Part.compSprite.texture = Assets.Dungeon_CurrentSheet;
            }

            #endregion


            #region Small - 8x8

            else if (Type == ParticleType.RisingSmoke)
            {
                Part.compSprite.drawRec.Width = 8; Part.compSprite.drawRec.Height = 8; //nonstandard size
                Part.compSprite.zOffset = 64;
                Part.lifetime = 24; //in frames
                Part.compAnim.speed = 6; //in frames
                Part.compAnim.loop = false;
                Part.compAnim.currentAnimation = AnimationFrames.Particle_RisingSmoke;
                Part.compSprite.texture = Assets.entitiesSheet;
                //randomly flip the smoke sprite horizontally for variation
                if (Functions_Random.Int(0, 101) > 50)
                { Part.compSprite.flipHorizontally = true; }
                else { Part.compSprite.flipHorizontally = false; }
            }
            else if (Type == ParticleType.ImpactDust)
            {
                Part.compSprite.drawRec.Width = 8; Part.compSprite.drawRec.Height = 8; //nonstandard size
                Part.compSprite.zOffset = 32;
                Part.lifetime = 24; //in frames
                Part.compAnim.speed = 5; //in frames
                Part.compAnim.loop = false;
                Part.compAnim.currentAnimation = AnimationFrames.Particle_ImpactDust;
                Part.compSprite.texture = Assets.entitiesSheet;
            }

            else if (Type == ParticleType.Sparkle)
            {
                Part.compSprite.drawRec.Width = 8; Part.compSprite.drawRec.Height = 8; //nonstandard size
                Part.compSprite.zOffset = 16;
                Part.lifetime = 24; //in frames
                Part.compAnim.speed = 6; //in frames
                Part.compAnim.currentAnimation = AnimationFrames.Particle_Sparkle;
                Part.compSprite.texture = Assets.entitiesSheet;
            }
            else if (Type == ParticleType.SparkleBlue)
            {
                Part.compSprite.drawRec.Width = 8; Part.compSprite.drawRec.Height = 8; //nonstandard size
                Part.compSprite.zOffset = 16;
                Part.lifetime = 24; //in frames
                Part.compAnim.speed = 6; //in frames
                Part.compAnim.currentAnimation = AnimationFrames.Particle_SparkleBlue;
                Part.compSprite.texture = Assets.entitiesSheet;
            }

            else if (Type == ParticleType.Push)
            {
                Part.compSprite.drawRec.Width = 8; Part.compSprite.drawRec.Height = 8; //nonstandard size
                Part.compSprite.zOffset = 32;
                Part.lifetime = 6 * 3; //in frames
                Part.compAnim.speed = 6; //in frames
                Part.compAnim.currentAnimation = AnimationFrames.Particle_Push;
                Part.compSprite.texture = Assets.entitiesSheet;

                //set the sprites rotation based on direction
                if (Part.direction == Direction.Down)
                { Part.compSprite.rotation = Rotation.None; }
                else if (Part.direction == Direction.Left)
                { Part.compSprite.rotation = Rotation.Clockwise90; }
                else if (Part.direction == Direction.Up)
                { Part.compSprite.rotation = Rotation.Clockwise180; }
                else if (Part.direction == Direction.Right)
                { Part.compSprite.rotation = Rotation.Clockwise270; }
                else //push particle can't be in diagonal state, hide it
                { Part.compSprite.visible = false; }
            }

            #endregion


            #region Explosive Debris - 8x8

            else if (
                Type == ParticleType.LeafGreen || Type == ParticleType.DebrisBrown
                || Type == ParticleType.BloodRed || Type == ParticleType.SlimeGreen
                )
            {
                Part.compSprite.drawRec.Width = 8; Part.compSprite.drawRec.Height = 8; //nonstandard size
                Part.compSprite.zOffset = 16;
                Part.compAnim.loop = false;
                Part.lifetime = 15; //in frames
                Part.compAnim.speed = 6; //in frames
                //setup animation frame properly
                if (Type == ParticleType.LeafGreen)
                { Part.compAnim.currentAnimation = AnimationFrames.Particle_LeafGreen; }
                else if (Type == ParticleType.DebrisBrown)
                { Part.compAnim.currentAnimation = AnimationFrames.Particle_DebrisBrown; }
                else if (Type == ParticleType.BloodRed)
                { Part.compAnim.currentAnimation = AnimationFrames.Particle_BloodRed; }
                else { Part.compAnim.currentAnimation = AnimationFrames.Particle_SlimeGreen; }

                //these debris sprites are on the common sheet
                Part.compSprite.texture = Assets.CommonObjsSheet;
            }

            #endregion


            #region Normal - 16x16

            else if (Type == ParticleType.Attention)
            {
                Part.compSprite.zOffset = 1024;
                Part.lifetime = 24; //in frames
                Part.compAnim.speed = 6; //in frames
                Part.compAnim.loop = false;
                Part.compAnim.currentAnimation = AnimationFrames.Particle_Attention;
                Part.compSprite.texture = Assets.entitiesSheet;
            }
            else if (Type == ParticleType.ExclamationBubble)
            {
                Part.compSprite.zOffset = 1024;
                Part.lifetime = 45; //in frames
                Part.compAnim.speed = 5; //in frames
                Part.compAnim.loop = false;
                Part.compAnim.currentAnimation = AnimationFrames.Particle_ExclamationBubble;
                Part.compSprite.texture = Assets.entitiesSheet;
            }
            else if (Type == ParticleType.Splash)
            {
                Part.compSprite.zOffset = 16;
                Part.compAnim.speed = 7; //in frames
                Part.lifetime = 7 * 5; //speed * animFrames
                Part.compAnim.loop = false;
                Part.compAnim.currentAnimation = AnimationFrames.Particle_Splash;
                Part.compSprite.texture = Assets.entitiesSheet;
            }
            else if (Type == ParticleType.Blast)
            {
                Part.compSprite.zOffset = 64;
                Part.compAnim.speed = 6; //in frames
                Part.lifetime = 6 * 4; //very short
                Part.compAnim.loop = true;
                Part.compAnim.currentAnimation = AnimationFrames.Particle_Blast;
                Part.compSprite.texture = Assets.entitiesSheet;
            }
            else if (Type == ParticleType.Fire)
            {   //very short and fast, non-interactive ground fire
                Part.compSprite.zOffset = 64;
                Part.lifetime = 20; //in frames
                Part.compAnim.speed = 6; //in frames
                Part.compAnim.currentAnimation = AnimationFrames.Projectile_FireGround;
                Part.compSprite.texture = Assets.entitiesSheet;
            }

            else if (Type == ParticleType.WaterKick)
            {
                Part.compSprite.zOffset = 0;
                Part.compAnim.speed = 7; //in frames
                Part.lifetime = 7 * 4 + 5; //speed * animTotal + holdFrame
                Part.compMove.friction = World.frictionWater;
                Part.compAnim.currentAnimation = AnimationFrames.Particle_WaterKick;
                Part.compSprite.texture = Assets.entitiesSheet;
                Part.compAnim.loop = false;
            }

            //Particles - Rewards & Bottles & Magics
            else if (
                Type == ParticleType.RewardKey
                || Type == ParticleType.RewardMap
                || Type == ParticleType.RewardBottle
                //magics
                || Type == ParticleType.RewardMagicBombos
                || Type == ParticleType.RewardMagicEther
                )
            {
                Part.compSprite.zOffset = 32;
                Part.lifetime = 30; //in frames
                Part.compMove.moveable = false;
                //common rewards
                if (Type == ParticleType.RewardKey)
                {  
                    Part.compAnim.currentAnimation = AnimationFrames.Dungeon_BossKey;
                    Part.compSprite.texture = Assets.CommonObjsSheet;
                }
                else if (Type == ParticleType.RewardMap)
                {   
                    Part.compAnim.currentAnimation = AnimationFrames.Dungeon_Map;
                    Part.compSprite.texture = Assets.CommonObjsSheet;
                }
                else if (Type == ParticleType.RewardBottle)
                {   //assume just an empty bottle
                    Part.compAnim.currentAnimation = AnimationFrames.Bottle_Empty;
                    //this is only for hero, and is based on what hero's item is
                    if (Pool.hero.item == MenuItemType.BottleBlob)
                    { Part.compAnim.currentAnimation = AnimationFrames.Bottle_Blob; }
                    else if (Pool.hero.item == MenuItemType.BottleCombo)
                    { Part.compAnim.currentAnimation = AnimationFrames.Bottle_Combo; }
                    else if (Pool.hero.item == MenuItemType.BottleEmpty)
                    { Part.compAnim.currentAnimation = AnimationFrames.Bottle_Empty; }
                    else if (Pool.hero.item == MenuItemType.BottleFairy)
                    { Part.compAnim.currentAnimation = AnimationFrames.Bottle_Fairy; }
                    else if (Pool.hero.item == MenuItemType.BottleHealth)
                    { Part.compAnim.currentAnimation = AnimationFrames.Bottle_Health; }
                    else if (Pool.hero.item == MenuItemType.BottleMagic)
                    { Part.compAnim.currentAnimation = AnimationFrames.Bottle_Magic; }
                    //bottles all exist on the ui sheet
                    Part.compSprite.texture = Assets.uiItemsSheet;
                }
                //magics
                else if (Type == ParticleType.RewardMagicBombos)
                {   
                    Part.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Magic_Bombos;
                    Part.compSprite.texture = Assets.uiItemsSheet;
                }
                else if (Type == ParticleType.RewardMagicEther)
                {
                    Part.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Magic_Ether;
                    Part.compSprite.texture = Assets.uiItemsSheet;
                }
            }

            #endregion


            #region Irregular sized

            else if (Type == ParticleType.LightningBolt)
            {
                Part.compSprite.zOffset = 1024;
                Part.lifetime = 30; //in frames
                Part.compAnim.speed = 1; //in frames
                Part.compAnim.loop = true;
                Part.compAnim.currentAnimation = AnimationFrames.Particle_LightningBolt;
                Part.compSprite.texture = Assets.entitiesSheet;
            }

            #endregion








            #region Overworld / Map - misc

            //these particles only exist on the overworld map
            else if (Type == ParticleType.Map_Flag)
            {
                Part.compSprite.drawRec.Width = 8; Part.compSprite.drawRec.Height = 4; //nonstandard size
                Part.compAnim.speed = 10; //in frames
                Part.compAnim.currentAnimation = AnimationFrames.Particle_Map_Flag;
                Part.compSprite.texture = Assets.entitiesSheet;
                Part.lifetime = 0; //lives forever
            }
            else if (Type == ParticleType.Map_Wave)
            {
                Part.compSprite.drawRec.Width = 8; Part.compSprite.drawRec.Height = 4; //nonstandard size
                Part.compAnim.speed = 15; //in frames
                Part.lifetime = (byte)(Part.compAnim.speed * 4); //there are 4 frames of animation
                Part.compAnim.loop = false;
                Part.compAnim.currentAnimation = AnimationFrames.Particle_Map_Wave;
                Part.compSprite.texture = Assets.entitiesSheet;
            }
            else if (Type == ParticleType.Map_Campfire)
            {
                Part.compSprite.drawRec.Width = 8;
                Part.compSprite.drawRec.Height = 8; //nonstandard size
                Part.compAnim.speed = 6; //in frames
                Part.compAnim.currentAnimation = AnimationFrames.Particle_Map_Campfire;
                Part.compSprite.texture = Assets.entitiesSheet;
                Part.lifetime = 0; //lives forever
            }

            #endregion



            //based on type, we can flip horizontally/rotate
            Part.compSprite.flipHorizontally = false;
            Part.compSprite.rotation = Rotation.None;

            //set animframe to 0
            Part.compSprite.currentFrame = Part.compAnim.currentAnimation[0];
            //align particles sprite and movecomp
            Part.compMove.position.X = (int)Part.compMove.newPosition.X;
            Part.compMove.position.Y = (int)Part.compMove.newPosition.Y;
            Functions_Component.SetZdepth(Part.compSprite);
        }

    }
}