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
    public static class WinLoseFunctions
    {

        public static void Check()
        {
            //check lost state
            if (Pool.hero.state == Actor.State.Dead)
            { ScreenManager.AddScreen(new SummaryScreen(false)); }

            //check won state
            Boolean won = true;
            for (Pool.counter = 0; Pool.counter < Pool.actorCount; Pool.counter++)
            {   //check to see if any active actors are alive
                if (Pool.actorPool[Pool.counter].active)
                {   //if the actor is visible/drawable & not dead, then game hasn't been won
                    if (Pool.actorPool[Pool.counter].state != Actor.State.Dead)
                    { won = false; }
                }
            }
            if (won) { ScreenManager.AddScreen(new SummaryScreen(true)); }

            //later this will simply be a check to see if the dungeon boss is dead
        }

    }
}