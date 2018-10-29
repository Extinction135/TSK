﻿using System;
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
    public static class Functions_Texture
    {
        static int i;

        //called just after a dungeon is built
        public static void UpdateDungeonTexture()
        {
            //default dungeon texture to base sheet
            Assets.Dungeon_CurrentSheet = Assets.Dungeon_DefaultSheet;
            //based on the dungeon level loaded, set the dungeon texture
            if (LevelSet.dungeon.ID == LevelID.Forest_Dungeon)
            { Assets.Dungeon_CurrentSheet = Assets.Dungeon_ForestSheet; }
            else if (LevelSet.dungeon.ID == LevelID.Mountain_Dungeon)
            { Assets.Dungeon_CurrentSheet = Assets.Dungeon_MountainSheet; }
            else if (LevelSet.dungeon.ID == LevelID.Swamp_Dungeon)
            { Assets.Dungeon_CurrentSheet = Assets.Dungeon_SwampSheet; }
        }

        //called just after a dungeon is built
        public static void SetFloorTextures()
        {
            //update all floor texture references
            for (Pool.floorCounter = 0; Pool.floorCounter < Pool.floorCount; Pool.floorCounter++)
            { Pool.floorPool[Pool.floorCounter].texture = Assets.Dungeon_CurrentSheet; }
        }




        //this method should be used to update the dungeon objects widget later
        public static void SetWOTexture(WidgetObject WO)
        {   //12 rows, with 4 objs per row
            for (i = 0; i < 12 * 4; i++)
            {
                Functions_GameObject.SetType(WO.objList[i], WO.objList[i].type);
                //make hitboxes easy to select, because this is a gameobj acting as a ui menu item
                WO.objList[i].compCollision.rec.Width = 16;
                WO.objList[i].compCollision.rec.Height = 16;
                WO.objList[i].compCollision.rec.X = (int)WO.objList[i].compSprite.position.X - 8;
                WO.objList[i].compCollision.rec.Y = (int)WO.objList[i].compSprite.position.Y - 8;
            }
        }





    }
}