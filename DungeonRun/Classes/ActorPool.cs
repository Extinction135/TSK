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
        
        
        

        
        




        public void Draw(ScreenManager ScreenManager)
        {
            
        }





    }
}