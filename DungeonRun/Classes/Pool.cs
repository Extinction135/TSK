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

        //actor pool handles all actors in the room including hero
        public static int actorCount;           //total count of actors in pool
        public static List<Actor> actorPool;    //the actual list of actors
        public static int actorIndex;           //used to iterate thru the pool

        //obj pool handles gameobjects that don't move, & may(not) interact with actors
        public static int objCount;
        public static List<GameObject> objPool;
        public static int objIndex;

        //projectile pool handles projectiles/particles that move or are stationary
        public static int projectileCount;
        public static List<GameObject> projectilePool;
        public static int projectileIndex;

        public static int floorCount;
        public static List<ComponentSprite> floorPool;
        public static int floorIndex;

        public static int counter;
        public static int activeActor = 1; //tracks the current actor being handled by AI
        public static Actor hero;
        public static ComponentSprite heroShadow;
        
        public static void Initialize()
        {
            //set the pool sizes
            actorCount = 30;
            objCount = 150;
            projectileCount = 50;
            floorCount = 500;

            //actor pool
            actorPool = new List<Actor>();
            for (counter = 0; counter < actorCount; counter++)
            {
                actorPool.Add(new Actor());
                ActorFunctions.SetType(actorPool[counter], ActorType.Hero);
                MovementFunctions.Teleport(actorPool[counter].compMove, 0, 0);
            }
            actorIndex = 1;

            //obj pool
            objPool = new List<GameObject>();
            for (counter = 0; counter < objCount; counter++)
            { objPool.Add(new GameObject(Assets.shopSheet)); }
            objIndex = 0;

            //projectile pool
            projectilePool = new List<GameObject>();
            for (counter = 0; counter < projectileCount; counter++)
            { projectilePool.Add(new GameObject(Assets.mainSheet)); }
            projectileIndex = 0;

            //floor pool
            floorPool = new List<ComponentSprite>();
            for (counter = 0; counter < floorCount; counter++)
            {
                floorPool.Add(new ComponentSprite(Assets.shopSheet,
                    new Vector2(0, 0), new Byte4(6, 0, 0, 0), new Point(16, 16)));
            }
            floorIndex = 0;

            //reset all the pools
            PoolFunctions.Reset();

            //create an easy to remember reference to the player/hero actor
            hero = actorPool[0];
            heroShadow = new ComponentSprite(Assets.mainSheet, new Vector2(0, 0), new Byte4(0, 1, 0, 0), new Point(16, 8));
            heroShadow.zOffset = -16;
        }

    }
}