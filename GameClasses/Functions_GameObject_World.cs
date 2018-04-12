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
        {
            //convert tallgrass to cut grass
            Functions_GameObject.SetType(TallGrass, ObjType.Wor_Grass_Cut);

            //pop an attention particle on grass pos
            Functions_Particle.Spawn(ObjType.Particle_Attention,
                TallGrass.compSprite.position.X,
                TallGrass.compSprite.position.Y);

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
        {
            //Bush.compMove.direction is the hit direction
            //create bush leaves, push them in hit direction

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

    }
}