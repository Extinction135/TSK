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
        //represents the actor's pool, obj's pool, and projectile's pool

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
            actorCount = 60;
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
            ActorFunctions.SetType(actor, actor.type);
            actorIndex++;
            if (actorIndex > actorCount) { actorIndex = 1; } //skip 0th actor (HERO)
            return actor;
        }

        public GameObject GetObj()
        {
            GameObject obj = objPool[objIndex];
            GameObjectFunctions.SetType(obj, obj.type);
            objIndex++;
            if (objIndex > objCount) { objIndex = 0; }
            return obj;
        }

        public GameObject GetProjectile()
        {
            GameObject projectile = projectilePool[projectileIndex];
            GameObjectFunctions.SetType(projectile, projectile.type);
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
                actorPool[counter].compCollision.active = false;
                actorPool[counter].compCollision.rec.X = -1000;
            }
        }

        public void ResetObjPool()
        {
            for (counter = 0; counter < objCount; counter++)
            {
                objPool[counter].active = false;
                objPool[counter].compCollision.active = false;
                objPool[counter].compCollision.rec.X = -1000;
            }
        }

        public void ResetProjectilePool()
        {
            for (counter = 0; counter < projectileCount; counter++)
            {
                projectilePool[counter].active = false;
                projectilePool[counter].compCollision.active = false;
                projectilePool[counter].compCollision.rec.X = -1000;
            }
        }





        public void Move(DungeonScreen DungeonScreen)
        {   //actor pool
            for (counter = 0; counter < actorCount; counter++)
            { if (actorPool[counter].active) { CollisionFunctions.Move(actorPool[counter], DungeonScreen); } }
            //obj pool
            for (counter = 0; counter < objCount; counter++)
            { if (objPool[counter].active) { CollisionFunctions.Move(objPool[counter], DungeonScreen); } }
            //projectile pool
            for (counter = 0; counter < projectileCount; counter++)
            { if (projectilePool[counter].active) { CollisionFunctions.Move(projectilePool[counter], DungeonScreen); } }
        }




        public void UpdateActiveActor(Actor Actor)
        {
            InputFunctions.ResetInputData(Actor.compInput);
            AiManager.Think(Actor.compInput); //pass active actor to AiManager
            activeActor++;
            if (activeActor >= actorCount) { activeActor = 1; } //skip 0th actor (HERO)
        }

        public void Update()
        {
            //actor pool
            UpdateActiveActor(actorPool[activeActor]);
            UpdateActiveActor(actorPool[activeActor]);
            for (counter = 0; counter < actorCount; counter++)
            {
                if (actorPool[counter].active)
                {
                    ActorFunctions.Update(actorPool[counter]);
                    AnimationFunctions.Animate(actorPool[counter].compAnim, actorPool[counter].compSprite);
                }
            }

            //obj pool
            for (counter = 0; counter < objCount; counter++)
            {
                if (objPool[counter].active)
                { AnimationFunctions.Animate(objPool[counter].compAnim, objPool[counter].compSprite); }
            }

            //projectile pool
            for (counter = 0; counter < projectileCount; counter++)
            {
                if (projectilePool[counter].active)
                { AnimationFunctions.Animate(projectilePool[counter].compAnim, projectilePool[counter].compSprite); }
            }
        }




        public void Draw(ScreenManager ScreenManager)
        {
            //actor pool
            for (counter = 0; counter < actorCount; counter++)
            {
                if (actorPool[counter].active)
                { ActorFunctions.Draw(actorPool[counter], ScreenManager); }
            }

            //obj pool
            for (counter = 0; counter < objCount; counter++)
            {
                if (objPool[counter].active)
                { GameObjectFunctions.Draw(objPool[counter], ScreenManager); }
            }

            //projectile pool
            for (counter = 0; counter < projectileCount; counter++)
            {
                if (projectilePool[counter].active)
                { GameObjectFunctions.Draw(projectilePool[counter], ScreenManager); }
            }
        }
    }
}