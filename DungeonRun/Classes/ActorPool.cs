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
    public class ActorPool
    {
        //manages the pool of actors
        public int poolSize = 60; //60 actors, 60 fps = 1 actor gets AI input each frame
        public List<Actor> pool;
        public int counter;
        public int index;
        public int activeActor = 1; //skip the 0th actor, that's HERO

        public Actor hero;

        public ActorPool(DungeonScreen Screen)
        {
            pool = new List<Actor>();
            for (counter = 0; counter < poolSize; counter++)
            {
                pool.Add(new Actor(Screen));
                ActorFunctions.SetType(pool[counter], Actor.Type.Hero);
                ActorFunctions.Teleport(pool[counter], 0, 0);
            }
            index = 1; Reset();
            hero = pool[0]; //the hero exists at index 0 in the actorPool
        }

        public void Reset()
        {
            for (counter = 0; counter < poolSize; counter++)
            {
                pool[counter].active = false;
                pool[counter].compCollision.rec.X = -1000;
            }
        }
        public Actor GetActor()
        {
            Actor actor = pool[index];
            actor.active = true;
            index++;
            if(index > poolSize) { index = 1; } //skip 0th actor (HERO)
            return actor;
        }


        public void Move(DungeonScreen DungeonScreen)
        {
            for (counter = 0; counter < poolSize; counter++)
            {
                if (pool[counter].active)
                { CollisionFunctions.Move(pool[counter], DungeonScreen); }
            }
        }


        public void UpdateActiveActor(Actor Actor)
        {
            InputFunctions.ResetInputData(Actor.compInput);
            AiManager.Think(Actor.compInput); //pass active actor to AiManager
            activeActor++;
            if (activeActor >= poolSize) { activeActor = 1; } //skip 0th actor (HERO)
        }

        public void Update()
        {
            UpdateActiveActor(pool[activeActor]);
            UpdateActiveActor(pool[activeActor]);

            for (counter = 0; counter < poolSize; counter++)
            {
                if (pool[counter].active)
                { ActorFunctions.Update(pool[counter]); }
            }
        }
        public void Draw(ScreenManager ScreenManager)
        {
            for (counter = 0; counter < poolSize; counter++)
            {
                if (pool[counter].active)
                { ActorFunctions.Draw(pool[counter], ScreenManager); }
            }
        }
    }
}