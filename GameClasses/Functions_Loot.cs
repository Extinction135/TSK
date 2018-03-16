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
            dropRate = 40; //default 40% chance to drop loot
            //if hero has ring equipped, increase the loot drop rate to 70%
            if (Pool.hero.equipment == MenuItemType.EquipmentRing) { dropRate = 70; }
            //create loot if random value is less than dropRate
            if (Functions_Random.Int(0, 100) < dropRate)
            {
                //randomly choose a type of loot to spawn
                lootType = Functions_Random.Int(0, 101);

                //common loot drops
                if (lootType < 25) //25%
                { Functions_Pickup.Spawn(ObjType.PickupHeart, Pos.X, Pos.Y); }
                else if (lootType < 50) //25%
                { Functions_Pickup.Spawn(ObjType.PickupMagic, Pos.X, Pos.Y); }
                else if (lootType < 75) //25%
                { Functions_Pickup.Spawn(ObjType.PickupRupee, Pos.X, Pos.Y); }

                //uncommon loot drops
                else if (lootType < 87) //12%
                { Functions_Pickup.Spawn(ObjType.PickupBomb, Pos.X, Pos.Y); }
                else if (lootType < 97) //12%
                { Functions_Pickup.Spawn(ObjType.PickupArrow, Pos.X, Pos.Y); }

                //rare loot drops
                else if (lootType < 101) //1%
                {   //spawn a fairy roomObj
                    GameObject Fairy = Functions_Pool.GetRoomObj();
                    Functions_GameObject.SetType(Fairy, ObjType.Fairy);
                    Functions_Movement.Teleport(Fairy.compMove, Pos.X, Pos.Y);
                }
            }
        }

    }
}