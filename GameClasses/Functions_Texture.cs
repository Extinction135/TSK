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
    public static class Functions_Texture
    {
        static Texture2D currentTexture; //points to a texture in assets
        static int i;


        static void SetTexture()
        {   //based on level id, set the current texture
            currentTexture = Assets.forestLevelSheet; //default to forest

            if (LevelSet.currentLevel.ID == LevelID.LeftTown2) { currentTexture = Assets.townLevelSheet; }
            else if (LevelSet.currentLevel.ID == LevelID.TheFarm) { currentTexture = Assets.townLevelSheet; }

            else if (LevelSet.currentLevel.ID == LevelID.Colliseum) { currentTexture = Assets.colliseumLevelSheet; }
            else if (LevelSet.currentLevel.ID == LevelID.ColliseumPit) { currentTexture = Assets.colliseumLevelSheet; }

            else if (LevelSet.currentLevel.ID == LevelID.Forest_Entrance) { currentTexture = Assets.forestLevelSheet; }
            else if (LevelSet.currentLevel.ID == LevelID.Forest_Dungeon) { currentTexture = Assets.forestLevelSheet; }

            else if (LevelSet.currentLevel.ID == LevelID.Mountain_Entrance) { currentTexture = Assets.mountainLevelSheet; }
            else if (LevelSet.currentLevel.ID == LevelID.Mountain_Dungeon) { currentTexture = Assets.mountainLevelSheet; }

            else if (LevelSet.currentLevel.ID == LevelID.Swamp_Entrance) { currentTexture = Assets.swampLevelSheet; }
            else if (LevelSet.currentLevel.ID == LevelID.Swamp_Dungeon) { currentTexture = Assets.swampLevelSheet; }
        }


        public static void SetObjTexture(GameObject Obj)
        {
            SetTexture();
            Obj.compSprite.texture = currentTexture;
        }

        public static void SetFloorTextures()
        {
            SetTexture();
            //update all floor texture references
            for (Pool.floorCounter = 0; Pool.floorCounter < Pool.floorCount; Pool.floorCounter++)
            { Pool.floorPool[Pool.floorCounter].texture = currentTexture; }
        }

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