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

        public int counter;

        public int activeActor = 1; //skip the 0th actor, that's HERO
        public Actor hero;


        public Pool(DungeonScreen Screen)
        {
            //set the pool sizes
            actorCount = 100;
            objCount = 100;
            projectileCount = 100;

            //actor pool
            actorPool = new List<Actor>();
            for (counter = 0; counter < actorCount; counter++)
            {
                actorPool.Add(new Actor(Screen));
                ActorFunctions.SetType(actorPool[counter], Actor.Type.Hero);
                ActorFunctions.Teleport(actorPool[counter], 0, 0);
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

            //reset all the pools
            Reset();

            //create an easy to remember reference to the player/hero actor
            hero = actorPool[0];
        }








        public Actor GetActor()
        {
            Actor actor = actorPool[actorIndex];
            actor.active = true;
            actorIndex++;
            if (actorIndex > actorCount) { actorIndex = 1; } //skip 0th actor (HERO)
            return actor;
        }

        public GameObject GetObj()
        {
            GameObject obj = objPool[objIndex];
            obj.active = true;
            objIndex++;
            if (objIndex > objCount) { objIndex = 0; }
            return obj;
        }

        public GameObject GetProjectile()
        {
            GameObject projectile = projectilePool[projectileIndex];
            projectile.active = true;
            projectileIndex++;
            if (projectileIndex > projectileCount) { projectileIndex = 0; }
            return projectile;
        }







        public void Reset()
        {
            ResetActorPool();
            ResetObjPool();
            ResetProjectilePool();
        }

        public void ResetActorPool()
        {
            for (counter = 0; counter < actorCount; counter++)
            {
                actorPool[counter].active = false;
                actorPool[counter].compCollision.rec.X = -1000;
            }
        }

        public void ResetObjPool()
        {
            for (counter = 0; counter < objCount; counter++)
            {
                objPool[counter].active = false;
                objPool[counter].compCollision.rec.X = -1000;
            }
        }

        public void ResetProjectilePool()
        {
            for (counter = 0; counter < projectileCount; counter++)
            {
                projectilePool[counter].active = false;
                projectilePool[counter].compCollision.rec.X = -1000;
            }
        }







        public void Move(DungeonScreen DungeonScreen)
        {
            for (counter = 0; counter < actorCount; counter++)
            { if (actorPool[counter].active) { CollisionFunctions.Move(actorPool[counter], DungeonScreen); } }

            for (counter = 0; counter < objCount; counter++)
            { if (objPool[counter].active) { CollisionFunctions.Move(objPool[counter], DungeonScreen); } }

            for (counter = 0; counter < projectileCount; counter++)
            { if (projectilePool[counter].active) { CollisionFunctions.Move(projectilePool[counter], DungeonScreen); } }
        }




        //handle the drawing of each pool





    }
}