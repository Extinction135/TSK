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
    public static class Functions_Alignment
    {
        static Direction cardinal;
        public static int offsetX;
        public static int offsetY;



        public static void SetOffsets(GameObject Obj, ObjType Type)
        {
            offsetX = 0; offsetY = 0; //reset offsets
            cardinal = Functions_Direction.GetCardinalDirection(Obj.direction);

            if (Type == ObjType.ProjectileArrow)
            {   //place projectile outside of obj's hitbox
                if (cardinal == Direction.Down) { offsetY = 16; }
                else if (cardinal == Direction.Up) { offsetY = -16; }
                else if (cardinal == Direction.Right) { offsetX = 16; }
                else if (cardinal == Direction.Left) { offsetX = -16; }
            }
        }

        public static void SetOffsets(Actor Actor, ObjType Type)
        {
            offsetX = 0; offsetY = 0; //reset offsets
            cardinal = Functions_Direction.GetCardinalDirection(Actor.direction);


            #region Particles

            //center horizontally, place near actor's feet
            if (Type == ObjType.ParticleDashPuff) { offsetX = 4; offsetY = 8; }

            else if (Type == ObjType.ParticleBow)
            {   //place bow particle in the actor's hands
                if (cardinal == Direction.Down) { offsetY = 6; }
                else if (cardinal == Direction.Up) { offsetY = -6; }
                else if (cardinal == Direction.Right) { offsetX = 6; }
                else if (cardinal == Direction.Left) { offsetX = -6; }
            }

            #endregion


            #region Pickups

            else if (Type == ObjType.PickupRupee)
            {   //place the dropped rupee away from hero, cardinal = pushed direction
                if (cardinal == Direction.Down) { offsetX = 4; offsetY = -12; }
                else if (cardinal == Direction.Up) { offsetX = 4; offsetY = 15; }
                else if (cardinal == Direction.Right) { offsetX = -14; offsetY = 4; }
                else if (cardinal == Direction.Left) { offsetX = 14; offsetY = 4; }
            }

            #endregion


            #region Projectiles

            else if (
                Type == ObjType.ProjectileFireball ||
                Type == ObjType.ProjectileBomb ||
                Type == ObjType.ProjectileArrow)
            {   //place projectile outside of actor's hitbox
                if (cardinal == Direction.Down) { offsetY = 14; }
                else if (cardinal == Direction.Up) { offsetY = -9; }
                else if (cardinal == Direction.Right) { offsetX = 11; offsetY = 2; }
                else if (cardinal == Direction.Left) { offsetX = -11; offsetY = 2; }
            }
            else if (
                Type == ObjType.ProjectileSword)
            {   //place projectile outside of actor's hitbox, in actor's hand
                if (cardinal == Direction.Down) { offsetX = -1; offsetY = 15; }
                else if (cardinal == Direction.Up) { offsetX = 1; offsetY = -12; }
                else if (cardinal == Direction.Right) { offsetX = 14; }
                else if (cardinal == Direction.Left) { offsetX = -14; }
            }

            #endregion


            #region Reward Particles

            //place reward particles above actor's head
            else if (Type == ObjType.ParticleRewardGold ||
                Type == ObjType.ParticleRewardKey ||
                Type == ObjType.ParticleRewardMap ||
                Type == ObjType.ParticleRewardHeartFull ||
                Type == ObjType.ParticleRewardHeartPiece ||
                Type == ObjType.ParticleFairy)
            { offsetY = -14; }

            #endregion


        }

    }
}