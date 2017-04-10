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
    public class Pool
    {
        public int actorCount;
        public List<Actor> actorPool;
        public int actorIndex;

        public int objCount;
        public List<GameObject> objPool;
        public int objIndex;

        public int projectileCount;
        public List<GameObject> projectilePool;
        public int projectileIndex;

        public int floorCount;
        public List<ComponentSprite> floorPool;
        public int floorIndex;

        public int counter;
        public int activeActor = 1; //skip the 0th actor, that's HERO
        public Actor hero;

        public Pool(DungeonScreen Screen)
        {
            //set the pool sizes
            actorCount = 60;
            objCount = 100;
            projectileCount = 5;
            floorCount = 500;

            //actor pool
            actorPool = new List<Actor>();
            for (counter = 0; counter < actorCount; counter++)
            {
                actorPool.Add(new Actor(Screen));
                ActorFunctions.SetType(actorPool[counter], Actor.Type.Hero);
                MovementFunctions.Teleport(actorPool[counter].compMove, 0, 0);
            }
            actorIndex = 1;
            
            //obj pool
            objPool = new List<GameObject>();
            for (counter = 0; counter < objCount; counter++)
            { objPool.Add(new GameObject(Screen.assets.dungeonSheet)); }
            objIndex = 0;

            //projectile pool
            projectilePool = new List<GameObject>();
            for (counter = 0; counter < projectileCount; counter++)
            { projectilePool.Add(new GameObject(Screen.assets.particleSheet)); }
            projectileIndex = 0;

            //floor pool
            floorPool = new List<ComponentSprite>();
            for (counter = 0; counter < floorCount; counter++)
            {
                floorPool.Add(new ComponentSprite(Screen.assets.dungeonSheet,
                    new Vector2(0, 0), new Byte4(6, 0, 0, 0), new Byte2(16, 16)));
            }
            floorIndex = 0;

            //reset all the pools
            PoolFunctions.Reset(this);

            //create an easy to remember reference to the player/hero actor
            hero = actorPool[0];
        }
    }
}