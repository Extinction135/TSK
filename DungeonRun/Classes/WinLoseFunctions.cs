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
        public static Boolean won;
        public static void Check(DungeonScreen DungeonScreen)
        {
            //check lost state
            if (Pool.hero.state == Actor.State.Dead)
            {
                DungeonScreen.gameState = DungeonScreen.GameState.Lost;
                DungeonScreen.screenState = DungeonScreen.ScreenState.FadeIn;
            }

            //check won state
            won = true;
            for (Pool.counter = 1; Pool.counter < Pool.actorCount; Pool.counter++)
            {   //skip the hero, thus counter starts with 1 instead of 0
                //check to see if any active enemies are alive
                if (Pool.actorPool[Pool.counter].active)
                {   //if the actor is visible/drawable & not dead, then game hasn't been won
                    if (Pool.actorPool[Pool.counter].state != Actor.State.Dead)
                    { won = false; }
                }
            }
            if (won)
            {
                DungeonScreen.gameState = DungeonScreen.GameState.Won;
                DungeonScreen.screenState = DungeonScreen.ScreenState.FadeIn;
            }

            //later this will simply be a check to see if the dungeon boss is dead
        }

    }
}