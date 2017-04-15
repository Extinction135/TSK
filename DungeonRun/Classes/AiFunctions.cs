﻿using System;
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
    public static class AiFunctions
    {

        public static Direction directionToHero;
        public static Vector2 actorPos;
        public static Vector2 heroPos;
        static Actor Actor;
        public static void SetActorInput()
        {
            //target the active actor from the actor's pool
            Actor = Pool.actorPool[Pool.activeActor];



            #region AI Routines

            //reset the direction to hero
            directionToHero = Direction.None;

            //collect the actor and hero sprite positions
            actorPos = Actor.compSprite.position;
            heroPos = Pool.hero.compSprite.position;

            //get the x and y distances between the actor and hero
            int xDistance = (int)Math.Abs(heroPos.X - actorPos.X);
            int yDistance = (int)Math.Abs(heroPos.Y - actorPos.Y);

            //determine the axis hero is closest on
            if (xDistance < yDistance)
            {   //hero is closer on xAxis, actor should move on yAxis
                if (heroPos.Y > actorPos.Y)
                { directionToHero = Direction.Down; }
                else { directionToHero = Direction.Up; }
            }
            else
            {   //hero is closer on yAxis, actor should move on xAxis
                if (heroPos.X > actorPos.X)
                { directionToHero = Direction.Right; }
                else { directionToHero = Direction.Left; }
            }

            //determine if actor is close enough to hero to chase
            int chaseRadius = 16 * 6;
            if (yDistance < chaseRadius && xDistance < chaseRadius)
            {   //actor is close enough to hero to chase, move towards the hero
                Actor.compInput.direction = directionToHero;
            }
            else
            {   //choose a random direction to move in
                Actor.compInput.direction = (Direction)GetRandom.Int(0, 8);
            }

            //determine if actor is close enough to hero to attack
            int attackRadius = 14;
            if (yDistance < attackRadius && xDistance < attackRadius)
            {   //actor is close enough to hero to attack
                if (GetRandom.Int(0, 100) > 50) //randomly attack
                { Actor.compInput.attack = true; }
            }

            //determine if the actor can dash
            if(!Actor.compInput.attack)
            {   //if the actor isn't attacking, then randomly dash
                if (GetRandom.Int(0, 100) > 90)
                { Actor.compInput.dash = true; }
            }

            //handle the state where the hero is dead
            if (Pool.hero.state == Actor.State.Dead)
            {   //reset AI input, randomly move + dash
                Input.ResetInputData(Actor.compInput);
                Actor.compInput.direction = (Direction)GetRandom.Int(0, 8);
                if (GetRandom.Int(0, 100) > 90) { Actor.compInput.dash = true; }
            }

            #endregion



            //slightly more advanced AI

            //if wounded, try to heal
            //else...

            //if offensive...
            //if very close, attack/use/dash hero
            //if nearby, dash towards hero
            //if in visibility range, move towards hero
            //else, wander around

            //if defensive...
            //if very close or nearby, move away from hero
            //if in visibility range, ranged attack hero
            //else, wander around



            //increment the active actor
            Pool.activeActor++;
            if (Pool.activeActor >= Pool.actorCount) { Pool.activeActor = 1; } //skip 0th actor (HERO)
        }

    }
}