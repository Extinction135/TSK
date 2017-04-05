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
        public int activeActor = 0;
        

        public ActorPool(DungeonScreen Screen)
        {
            pool = new List<Actor>();
            for (counter = 0; counter < poolSize; counter++)
            {
                pool.Add(new Actor(Screen));
                ActorFunctions.SetType(pool[counter], Actor.Type.Blob);
                ActorFunctions.Teleport(pool[counter], Global.Random.Next(0, 300), Global.Random.Next(0, 200));
            }
            index = 0;
        }

        public void Reset()
        {
            for (counter = 0; counter < poolSize; counter++)
            { pool[counter].active = false; }
        }
        public Actor GetActor()
        {
            Actor actor = pool[index];
            actor.active = true;
            index++;
            if(index > poolSize) { index = 0; }
            return actor;
        }

        public void Update()
        {
            //calculate AI input for X actors per frame
            pool[activeActor].compInput.ResetInputData(); //clear input data for active actor
            AiManager.Think(pool[activeActor].compInput); //pass active actor to AiManager
            activeActor++;
            if(activeActor >= poolSize) { activeActor = 0; }

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