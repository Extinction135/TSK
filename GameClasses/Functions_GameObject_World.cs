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
    public static class Functions_GameObject_World
    {
        //static int i;



        public static void CutTallGrass(GameObject TallGrass)
        {   //pop an attention particle on grass pos
            Functions_Particle.Spawn(ObjType.Particle_Attention,
                TallGrass.compSprite.position.X,
                TallGrass.compSprite.position.Y);
            //convert tallgrass to cut grass + sfx
            Functions_GameObject.SetType(TallGrass, ObjType.Wor_Grass_Cut);
            Assets.Play(Assets.sfxBushCut);
            //rarely spawn loot
            if (Functions_Random.Int(0, 101) > 90) //cut that grass boi
            { Functions_Loot.SpawnLoot(TallGrass.compSprite.position); }
        }

        public static void CreateVendor(ObjType VendorType, Vector2 Position)
        {
            //place vendor
            Functions_GameObject.Spawn(VendorType, 
                Position.X, Position.Y, Direction.Down);
            //place stone table
            Functions_GameObject.Spawn(ObjType.Wor_TableStone, 
                Position.X + 16, Position.Y, Direction.Down);
            //we could spawn other stuff around the vendor too (thematically appropriate)
        }

        public static void DestroyBush(GameObject Bush)
        {   //pop leaf explosion
            Functions_Particle.Spawn_Explosion(
                ObjType.Particle_Leaf,
                Bush.compSprite.position.X,
                Bush.compSprite.position.Y);
            //covert bush to stump, play sfx
            Functions_GameObject.SetType(Bush, ObjType.Wor_Bush_Stump);
            Assets.Play(Assets.sfxBushCut);
            //pop an attention particle
            Functions_Particle.Spawn(ObjType.Particle_Attention,
                Bush.compSprite.position.X, Bush.compSprite.position.Y);
            //rarely spawn loot
            if (Functions_Random.Int(0,101) > 90)
            { Functions_Loot.SpawnLoot(Bush.compSprite.position); }
        }




        public static void BurnTree(GameObject Tree)
        {







            /*
            //pop the bushy top part
            Functions_Particle.Spawn(
                ObjType.Particle_Attention,
                Tree.compSprite.position.X,
                Tree.compSprite.position.Y - 2);

            //pop leaf explosion
            Functions_Particle.Spawn_Explosion(
                ObjType.Particle_Leaf,
                Tree.compSprite.position.X + 2,
                Tree.compSprite.position.Y - 4);
            */

            //switch to burned tree
            Functions_GameObject.SetType(Tree, ObjType.Wor_Tree_Burnt);

            //set ground fires all over the tree
            Functions_Projectile.Spawn(
                ObjType.ProjectileGroundFire,
                Tree.compSprite.position.X,
                Tree.compSprite.position.Y + 5);
            Functions_Projectile.Spawn(
                ObjType.ProjectileGroundFire,
                Tree.compSprite.position.X,
                Tree.compSprite.position.Y -2);

        }

        public static void BlowUpTree(GameObject Tree)
        {
            Assets.Play(Assets.sfxShatter);
            //pop the bushy top part
            Functions_Particle.Spawn(
                ObjType.Particle_Attention,
                Tree.compSprite.position.X,
                Tree.compSprite.position.Y - 2);
            //pop leaves in circular decorative pattern for tree top
            Functions_Particle.Spawn_Explosion(
                ObjType.Particle_Leaf,
                Tree.compSprite.position.X + 2,
                Tree.compSprite.position.Y - 4, true);
            //switch to tree stump
            Functions_GameObject.SetType(Tree, ObjType.Wor_Tree_Stump);
            //rarely spawn loot
            if (Functions_Random.Int(0, 101) > 90)
            { Functions_Loot.SpawnLoot(Tree.compSprite.position); }

            /*
            //set a ground fire ON the stump sprite
            Functions_Projectile.Spawn(
                ObjType.ProjectileGroundFire,
                Tree.compSprite.position.X,
                Tree.compSprite.position.Y + 5);
            */
        }

    }
}