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
    public static class ProjectileFunctions
    {



        //a projectile always has a direction, so it inherit's actor's direction
        public static void SpawnProjectile(GameObject.Type Type, Actor Actor)
        {
            GameObject projectile = PoolFunctions.GetProjectile();
            projectile.type = Type;
            projectile.direction = Actor.direction;
            AlignProjectile(projectile, Actor.compSprite.position);
            GameObjectFunctions.SetType(projectile, projectile.type);
        }

        //a particle doesn't have a direction, so it doesn't need actor's direction
        public static void SpawnParticle(GameObject.Type Type, Vector2 Pos)
        {
            GameObject particle = PoolFunctions.GetProjectile();
            particle.type = Type;
            particle.compSprite.rotation = Rotation.None;
            particle.compSprite.flipHorizontally = false;
            particle.direction = Direction.Down;
            AlignParticle(particle, Pos);
            GameObjectFunctions.SetType(particle, particle.type);
        }



        




        static Vector2 offset = new Vector2(0, 0);
        public static void AlignProjectile(GameObject Projectile, Vector2 Pos)
        {
            offset.X = 0; offset.Y = 0;
            GameObjectFunctions.ConvertDiagonalDirections(Projectile);
            //place the sword based on it's inherited direction
            if (Projectile.type == GameObject.Type.ProjectileSword)
            {
                if (Projectile.direction == Direction.Down)
                { offset.X = -1; offset.Y = 15; Projectile.compSprite.flipHorizontally = true; }
                else if (Projectile.direction == Direction.Up)
                { offset.X = 1; offset.Y = -12; Projectile.compSprite.flipHorizontally = false; }
                else if (Projectile.direction == Direction.Right)
                { offset.X = 14; offset.Y = 0; Projectile.compSprite.flipHorizontally = false; }
                else if (Projectile.direction == Direction.Left)
                { offset.X = -14; offset.Y = 0; Projectile.compSprite.flipHorizontally = true; }
            }
            //teleport the projectile to the position with the offset
            MovementFunctions.Teleport(Projectile.compMove, Pos.X + offset.X, Pos.Y + offset.Y);
        }

        public static void AlignParticle(GameObject Particle, Vector2 Pos)
        {
            offset.X = 0; offset.Y = 0;
            GameObjectFunctions.ConvertDiagonalDirections(Particle);

            //center horizontally, place near actor's feet
            if (Particle.type == GameObject.Type.ParticleDashPuff) { offset.X = 4; offset.Y = 8; }

            //teleport the projectile to the position with the offset
            MovementFunctions.Teleport(Particle.compMove, Pos.X + offset.X, Pos.Y + offset.Y);
        }





    }
}