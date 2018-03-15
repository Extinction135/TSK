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
        static Direction direction;


        //spawn relative to object
        public static void Spawn(ObjType Type, GameObject Object)
        {
            //set position reference to sprite position
            posRef.X = Object.compSprite.position.X;
            posRef.Y = Object.compSprite.position.Y;
            //direction is set based on obj.type


            #region Sword/Net

            //we could spawn a fireball here if we wanted to (which is how we'll handle the staff weapon)

            if (Object.type == ObjType.ProjectileSword || Object.type == ObjType.ProjectileNet)
            {   //place entity at tip of sword, based on direction
                if (Object.direction == Direction.Up) { posRef.X += 8; posRef.Y -= 0; }
                else if (Object.direction == Direction.Right) { posRef.X += 8; posRef.Y += 8; }
                else if (Object.direction == Direction.Down) { posRef.X += 8; posRef.Y += 8; }
                else if (Object.direction == Direction.Left) { posRef.X += 2; posRef.Y += 8; }
                direction = Object.direction;
            }

            #endregion


            #region Pit

            else if (Object.type == ObjType.PitAnimated)
            {   //randomly offset where the bubble particle is placed
                posRef.X += 4; posRef.Y += 4; //because bubble is 8x8 size
                posRef.X += Functions_Random.Int(-3, 4);
                posRef.Y += Functions_Random.Int(-3, 4);
                direction = Direction.None;
            }

            #endregion


            #region SpikeBlock

            else if (Object.type == ObjType.ProjectileSpikeBlock)
            {   //spikeblocks create hit particles upon their colliding (bounced) edge
                if (Object.compMove.direction == Direction.Down) { posRef.X += 4; posRef.Y += 10; }
                else if (Object.compMove.direction == Direction.Up) { posRef.X += 4; posRef.Y -= 4; }
                else if (Object.compMove.direction == Direction.Right) { posRef.X += 8; posRef.Y += 4; }
                else if (Object.compMove.direction == Direction.Left) { posRef.X -= 4; posRef.Y += 4; }
                direction = Object.compMove.direction;
            }

            #endregion


            Spawn(Type, posRef.X, posRef.Y, direction);
        }

        //spawn relative to actor
        public static void Spawn(ObjType Type, Actor Actor)
        {
            //spawned relative to actor, based on passed objType
            //set position reference to Actor's sprite position
            posRef.X = Actor.compSprite.position.X;
            posRef.Y = Actor.compSprite.position.Y;
            //get the actor's facing direction as cardinal direction
            direction = Functions_Direction.GetCardinalDirection(Actor.direction);

            if (Type == ObjType.ParticleDashPuff)
            {   //center horizontally, place near actor's feet
                posRef.X += 4; posRef.Y += 8;
            }
            else if (Type == ObjType.ParticleBow)
            {   //place bow particle in the actor's hands
                if (direction == Direction.Down) { posRef.Y += 6; }
                else if (direction == Direction.Up) { posRef.Y -= 6; }
                else if (direction == Direction.Right) { posRef.X += 6; }
                else if (direction == Direction.Left) { posRef.X -= 6; }
            }
            else if ( //place reward/bottle particles above actor's head
                Type == ObjType.ParticleRewardKey ||
                Type == ObjType.ParticleRewardMap ||
                Type == ObjType.ParticleBottleEmpty ||
                Type == ObjType.ParticleBottleHealth ||
                Type == ObjType.ParticleBottleMagic ||
                Type == ObjType.ParticleBottleCombo ||
                Type == ObjType.ParticleBottleFairy ||
                Type == ObjType.ParticleBottleBlob)
            { posRef.Y -= 14; }

            Spawn(Type, posRef.X, posRef.Y, direction);
        }

        //spawn relative to position
        public static void Spawn(ObjType Type, float X, float Y, Direction Direction)
        {   //get a particle to spawn
            GameObject obj = Functions_Pool.GetParticle();
            obj.compMove.moving = true;
            //set particles direction to down
            obj.direction = Direction.Down;
            obj.compMove.direction = Direction.Down;
            //teleport the object to the proper location
            Functions_Movement.Teleport(obj.compMove, X, Y);
            //set the type, rotation, cellsize, & alignment
            Functions_GameObject.SetType(obj, Type);
            Functions_Component.Align(obj); //align upon birth
            //Debug.WriteLine("entity made: " + Type + " - location: " + X + ", " + Y);
        }







        public static void ScatterRockDebris(Vector2 Pos, Boolean Push)
        {   //add up to 4 rocks randomly around the passed Pos value, with option to push them
            Direction pushDir = Direction.None;
            int spread = 6;
            if (Push) { pushDir = Functions_Direction.GetRandomCardinal(); }
            //always add at least one rock
            Spawn(ObjType.ProjectileDebrisRock, Pos.X, Pos.Y, pushDir);
            //sometimes add another rock
            if (Functions_Random.Int(0, 100) > 20)
            {   
                if (Push) { pushDir = Functions_Direction.GetRandomCardinal(); }
                Spawn(ObjType.ProjectileDebrisRock,
                    Pos.X + Functions_Random.Int(-spread, spread),
                    Pos.Y + Functions_Random.Int(-spread, spread), pushDir);
            }
            //sometimes add another rock
            if (Functions_Random.Int(0, 100) > 40)
            { 
                if (Push) { pushDir = Functions_Direction.GetRandomCardinal(); }
                Spawn(ObjType.ProjectileDebrisRock,
                    Pos.X + Functions_Random.Int(-spread, spread),
                    Pos.Y + Functions_Random.Int(-spread, spread), pushDir);
            }
            //sometimes add another rock
            if (Functions_Random.Int(0, 100) > 60)
            {  
                if (Push) { pushDir = Functions_Direction.GetRandomCardinal(); }
                Spawn(ObjType.ProjectileDebrisRock,
                    Pos.X + Functions_Random.Int(-spread, spread),
                    Pos.Y + Functions_Random.Int(-spread, spread), pushDir);
            }
        }


    }
}