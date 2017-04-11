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
    public static class Pool
    {
        public static int actorCount;
        public static List<Actor> actorPool;
        public static int actorIndex;

        public static int objCount;
        public static List<GameObject> objPool;
        public static int objIndex;

        public static int projectileCount;
        public static List<GameObject> projectilePool;
        public static int projectileIndex;

        public static int floorCount;
        public static List<ComponentSprite> floorPool;
        public static int floorIndex;

        public static int counter;
        public static int activeActor = 1; //skip the 0th actor, that's HERO
        public static Actor hero;

        public static void Initialize(DungeonScreen Screen)
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
                actorPool.Add(new Actor());
                ActorFunctions.SetType(actorPool[counter], Actor.Type.Hero);
                MovementFunctions.Teleport(actorPool[counter].compMove, 0, 0);
            }
            actorIndex = 1;
            
            //obj pool
            objPool = new List<GameObject>();
            for (counter = 0; counter < objCount; counter++)
            { objPool.Add(new GameObject(Assets.dungeonSheet)); }
            objIndex = 0;

            //projectile pool
            projectilePool = new List<GameObject>();
            for (counter = 0; counter < projectileCount; counter++)
            { projectilePool.Add(new GameObject(Assets.particleSheet)); }
            projectileIndex = 0;

            //floor pool
            floorPool = new List<ComponentSprite>();
            for (counter = 0; counter < floorCount; counter++)
            {
                floorPool.Add(new ComponentSprite(Assets.dungeonSheet,
                    new Vector2(0, 0), new Byte4(6, 0, 0, 0), new Byte2(16, 16)));
            }
            floorIndex = 0;

            //reset all the pools
            PoolFunctions.Reset();

            //create an easy to remember reference to the player/hero actor
            hero = actorPool[0];
        }
    }
}