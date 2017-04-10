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

        public static void Spawn(GameObject.Type Type, Actor Actor)
        {
            GameObject projectile = PoolFunctions.GetProjectile(Actor.screen.pool);
            projectile.direction = Actor.direction;
            projectile.type = Type;
            AlignProjectile(projectile, Actor);
            GameObjectFunctions.SetType(projectile, projectile.type);
        }

        static Vector2 ProjectileOffset = new Vector2(0, 0);
        public static void AlignProjectile(GameObject Projectile, Actor Actor)
        {
            ProjectileOffset.X = 0; ProjectileOffset.Y = 0;

            //convert projectile's diagonal direction to a cardinal direction
            if (Projectile.direction == Direction.UpRight)          { Projectile.direction = Direction.Right; }
            else if (Projectile.direction == Direction.DownRight)   { Projectile.direction = Direction.Right; }
            else if (Projectile.direction == Direction.UpLeft)      { Projectile.direction = Direction.Left; }
            else if (Projectile.direction == Direction.DownLeft)    { Projectile.direction = Direction.Left; }

            //different types of projectiles have different offsets
            if (Projectile.type == GameObject.Type.ProjectileSword)
            {
                if (Projectile.direction == Direction.Down)
                { ProjectileOffset.X = 0; ProjectileOffset.Y = 14; Projectile.compSprite.flipHorizontally = true; }
                else if (Projectile.direction == Direction.Up)
                { ProjectileOffset.X = 0; ProjectileOffset.Y = -14; Projectile.compSprite.flipHorizontally = false; }
                else if (Projectile.direction == Direction.Right)
                { ProjectileOffset.X = 14; ProjectileOffset.Y = 0; Projectile.compSprite.flipHorizontally = false; }
                else if (Projectile.direction == Direction.Left)
                { ProjectileOffset.X = -14; ProjectileOffset.Y = 0; Projectile.compSprite.flipHorizontally = true; }
            }

            //teleport the projectile to the actor's position with the offset applied
            MovementFunctions.Teleport(Projectile.compMove,
                Actor.compSprite.position.X + ProjectileOffset.X,
                Actor.compSprite.position.Y + ProjectileOffset.Y);
        }

    }
}