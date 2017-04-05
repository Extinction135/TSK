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
    public class GameObjectPool
    {
        //manages the pool of gameobjects
        public int poolSize = 100;
        public List<GameObject> pool;
        public int counter;
        public int index;

        public GameObjectPool(DungeonScreen Screen)
        {
            pool = new List<GameObject>();
            for (counter = 0; counter < poolSize; counter++)
            { pool.Add(new GameObject(Screen.screenManager.spriteBatch, Screen.assets.dungeonSheet)); }
            index = 0; Reset();
        }


        public void Reset()
        {
            for (counter = 0; counter < poolSize; counter++)
            {
                pool[counter].active = false;
                pool[counter].compCollision.rec.X = -1000;
            }
        }
        public GameObject GetObj()
        {
            GameObject obj = pool[index];
            obj.active = true;
            index++;
            if (index > poolSize) { index = 0; }
            return obj;
        }


        public void Update()
        {
            for (counter = 0; counter < poolSize; counter++)
            {
                if (pool[counter].active)
                { pool[counter].compAnim.Animate(); }
            }
        }
        public void Draw(ScreenManager ScreenManager)
        {
            for (counter = 0; counter < poolSize; counter++)
            {
                if (pool[counter].active)
                { GameObjectFunctions.Draw(pool[counter], ScreenManager); }
            }
        }
    }
}