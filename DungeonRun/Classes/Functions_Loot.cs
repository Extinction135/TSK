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
    public static class Functions_Loot
    {

        static int dropRate;
        static int lootType;

        public static void SpawnLoot(Vector2 Pos)
        {
            dropRate = 60; //default 40% chance to drop loot
            //if hero has ring equipped, increase the loot drop rate to 70%
            if (Pool.hero.equipment == MenuItemType.EquipmentRing) { dropRate = 30; }
            //create loot if random value is greater than dropRate
            if (Functions_Random.Int(0, 100) > dropRate)
            {
                //randomly choose a type of loot to spawn
                lootType = Functions_Random.Int(0, 100);

                //common loot drops
                if (lootType < 33) //33%
                { Functions_Projectiles.SpawnProjectile(ObjType.PickupHeart, Pos.X, Pos.Y, Direction.Down); }
                else if (lootType < 66) //33%
                { Functions_Projectiles.SpawnProjectile(ObjType.PickupMagic, Pos.X, Pos.Y, Direction.Down); }

                //uncommon loot drops
                else if (lootType < 91) //25%
                { Functions_Projectiles.SpawnProjectile(ObjType.PickupRupee, Pos.X, Pos.Y, Direction.Down); }

                //rare loot drops
                else if (lootType < 100) //9%
                { Functions_Projectiles.SpawnProjectile(ObjType.PickupBomb, Pos.X, Pos.Y, Direction.Down); }
            }
        }

    }
}