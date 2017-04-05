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
    public class FloorPool
    {
        //manages the pool of floor sprites
        public int poolSize = 200;
        public List<ComponentSprite> pool;
        public int counter;


        public FloorPool(DungeonScreen Screen)
        {
            pool = new List<ComponentSprite>();
            for (counter = 0; counter < poolSize; counter++)
            {
                //pool.Add(new GameObject(Screen.screenManager.spriteBatch, Screen.assets.dungeonSheet));
                pool.Add(new ComponentSprite(
                    Screen.screenManager.spriteBatch, Screen.assets.dungeonSheet, 
                    new Vector2(0, 0), new Byte4(6, 0, 0, 0), new Byte2(16, 16)));
            }
        }

        public void Reset()
        {
            for (counter = 0; counter < poolSize; counter++)
            { pool[counter].visible = false; }
        }

        public void Draw()
        {
            for (counter = 0; counter < poolSize; counter++)
            { if (pool[counter].visible) { pool[counter].Draw(); } }
        }
    }
}