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