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
            {   //randomly choose a type of loot to spawn
                lootType = Functions_Random.Int(0, 1001);
                //rare
                if (lootType < 5) //0.5% chance
                {   //spawn a fairy roomObj
                    GameObject Fairy = Functions_Pool.GetRoomObj();
                    Functions_GameObject.SetType(Fairy, ObjType.Dungeon_Fairy);
                    Functions_Movement.Teleport(Fairy.compMove, Pos.X, Pos.Y);
                }
                //uncommon
                else if (lootType < 100) //10%
                {
                    Functions_Pickup.Spawn(ObjType.Pickup_Bomb, Pos.X, Pos.Y);
                }
                else if (lootType < 200) //10%
                {
                    Functions_Pickup.Spawn(ObjType.Pickup_Arrow, Pos.X, Pos.Y);
                }
                else if(lootType < 300) //10%
                {
                    Functions_Pickup.Spawn(ObjType.Pickup_Magic, Pos.X, Pos.Y);
                }
                //common
                else if(lootType < 650)
                {
                    Functions_Pickup.Spawn(ObjType.Pickup_Heart, Pos.X, Pos.Y);
                }
                else
                {
                    Functions_Pickup.Spawn(ObjType.Pickup_Rupee, Pos.X, Pos.Y);
                }
            }
        }

    }
}