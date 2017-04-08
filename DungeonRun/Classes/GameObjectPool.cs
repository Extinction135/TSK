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