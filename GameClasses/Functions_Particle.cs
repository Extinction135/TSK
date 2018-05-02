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



        public static void Spawn(ObjType Type, GameObject Object)
        {
            //set position reference to sprite position
            posRef.X = Object.compSprite.position.X;
            posRef.Y = Object.compSprite.position.Y;


            #region Sword

            if (Object.type == ObjType.ProjectileSword || Object.type == ObjType.ProjectileNet)
            {   //place particle at tip, based on direction
                if (Object.direction == Direction.Up) { posRef.X += 8; posRef.Y -= 0; }
                else if (Object.direction == Direction.Right) { posRef.X += 8; posRef.Y += 8; }
                else if (Object.direction == Direction.Down) { posRef.X += 8; posRef.Y += 8; }
                else if (Object.direction == Direction.Left) { posRef.X += 2; posRef.Y += 8; }
            }

            #endregion


            #region Pit

            else if (Object.type == ObjType.Dungeon_Pit)
            {   //randomly offset where the bubble particle is placed
                posRef.X += 4; posRef.Y += 4; //because bubble is 8x8 size
                posRef.X += Functions_Random.Int(-3, 4);
                posRef.Y += Functions_Random.Int(-3, 4);
            }

            #endregion


            #region SpikeBlock

            else if (Object.type == ObjType.Dungeon_BlockSpike)
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

        public static void Spawn(ObjType Type, Actor Actor)
        {
            //spawned relative to actor, based on passed objType
            //set position reference to Actor's sprite position
            posRef.X = Actor.compSprite.position.X;
            posRef.Y = Actor.compSprite.position.Y;
            //get the actor's facing direction as cardinal direction
            //direction = Functions_Direction.GetCardinalDirection(Actor.direction);

            if (Type == ObjType.Particle_RisingSmoke)
            {   //center horizontally, place near actor's feet
                posRef.X += 4; posRef.Y += 8;
            }
            else if ( //place reward/bottle particles above actor's head
                Type == ObjType.Particle_RewardKey ||
                Type == ObjType.Particle_RewardMap ||
                Type == ObjType.Particle_BottleEmpty ||
                Type == ObjType.Particle_BottleHealth ||
                Type == ObjType.Particle_BottleMagic ||
                Type == ObjType.Particle_BottleCombo ||
                Type == ObjType.Particle_BottleFairy ||
                Type == ObjType.Particle_BottleBlob)
            { posRef.Y -= 14; }

            Spawn(Type, posRef.X, posRef.Y);
        }

        public static void Spawn(ObjType Type, float X, float Y, Direction Dir = Direction.Down)
        {   //get a particle to spawn
            GameObject obj = Functions_Pool.GetParticle();
            obj.compMove.moving = true;
            //set particles direction to passed direction
            obj.direction = Dir;
            obj.compMove.direction = Dir;
            //teleport the object to the proper location
            Functions_Movement.Teleport(obj.compMove, X, Y);
            //set the type, rotation, cellsize, & alignment
            Functions_GameObject.SetType(obj, Type);
            Functions_Component.Align(obj); //align upon birth
            //Debug.WriteLine("particle made: " + Type + " - location: " + X + ", " + Y);

            //handle soundfx for specific particles
            if (Type == ObjType.Particle_RewardMap) { Assets.Play(Assets.sfxReward); }
            else if (Type == ObjType.Particle_RewardKey) { Assets.Play(Assets.sfxKeyPickup); }
            else if (Type == ObjType.Particle_Splash) { Assets.Play(Assets.sfxSplash); }





            #region Handle Particle Birth Events

            if (Type == ObjType.Particle_FireGround)
            {
                Spawn(ObjType.Particle_RisingSmoke, X + 5, Y + 1);
            }
            else if (Type == ObjType.Particle_Push)
            {   //push the particle, usually less than whatever it's trailing
                Functions_Movement.Push(obj.compMove, obj.direction, 4.0f);
            }

            #endregion














            //we no longer have rock debris in this system, but we will in the future
            //this is a useful reference for randomizing an object's animFrame upon Spawn
            /*

            #region Modify RockDebris Particles Animation Frame + Slide them

            //some projectiles get their current frame randomly assigned (for variation)
            if (Type == ObjType.Particle_Debris)
            {   //is assigned 15,15 - randomize down to 14,14
                List<Byte4> rockFrame = new List<Byte4> { new Byte4(15, 15, 0, 0) };
                if (Functions_Random.Int(0, 100) > 50) { rockFrame[0].X = 14; }
                if (Functions_Random.Int(0, 100) > 50) { rockFrame[0].Y = 14; }
                obj.compAnim.currentAnimation = rockFrame;
                //push rock debris in a random direction
                Functions_Movement.Push(obj.compMove, 
                    Functions_Direction.GetRandomDirection(), 3.0f);
            }

            #endregion
            
            */

        }

        public static void Update(GameObject Obj)
        {
            if (Obj.lifetime == 0) { } //some particles live forever
            else
            {   //these particles 'die' after a lifetime
                Obj.lifeCounter++;
                if (Obj.lifeCounter >= Obj.lifetime) { Kill(Obj); }
            }


            #region Handle Particle Per Frame Behaviors

            if(Obj.type == ObjType.Particle_FireGround)
            {
                if (Functions_Random.Int(0, 101) > 86)
                {   //randomly place randomly offset rising smoke
                    Spawn(ObjType.Particle_RisingSmoke,
                        Obj.compSprite.position.X + 5 + Functions_Random.Int(-4, 4),
                        Obj.compSprite.position.Y + 1 + Functions_Random.Int(-8, 2));
                }
            }

            #endregion

        }

        public static void Kill(GameObject Obj)
        {
            //contains death events for particles

            //all objects are released upon death
            Functions_Pool.Release(Obj);
        }






        /*
        public static void ScatterDebris(Vector2 Pos)
        {   //add up to 4 debris particles randomly around Pos
            int spread = 5; //how far apart the debris spawns from Pos
            //always add at least one rock
            Spawn(ObjType.ParticleDebris, Pos.X, Pos.Y);
            //sometimes add another rock
            if (Functions_Random.Int(0, 100) > 20)
            {   
                Spawn(ObjType.ParticleDebris,
                    Pos.X + Functions_Random.Int(-spread, spread),
                    Pos.Y + Functions_Random.Int(-spread, spread));
            }
            //sometimes add another rock
            if (Functions_Random.Int(0, 100) > 40)
            { 
                Spawn(ObjType.ParticleDebris,
                    Pos.X + Functions_Random.Int(-spread, spread),
                    Pos.Y + Functions_Random.Int(-spread, spread));
            }
            //sometimes add another rock
            if (Functions_Random.Int(0, 100) > 60)
            {  
                Spawn(ObjType.ParticleDebris,
                    Pos.X + Functions_Random.Int(-spread, spread),
                    Pos.Y + Functions_Random.Int(-spread, spread));
            }
        }
        */





    }
}