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
    public static class AiFunctions
    {








        public static Direction directionToHero;

        public static void Think(Actor Actor)
        {
            //reset the actor's input component
            Input.ResetInputData(Actor.compInput);
            //reset the direction to hero
            directionToHero = Direction.None;



            //get the x and y distances between the actor and hero
            int xDistance = (int)Math.Abs(Pool.hero.compSprite.position.X - Actor.compSprite.position.X);
            int yDistance = (int)Math.Abs(Pool.hero.compSprite.position.Y - Actor.compSprite.position.Y);

            //determine the axis hero is closest on
            if (xDistance < yDistance)
            {   //hero is closer on xAxis, actor should move on yAxis
                if (Pool.hero.compSprite.position.Y > Actor.compSprite.position.Y)
                { directionToHero = Direction.Down; }
                else { directionToHero = Direction.Up; }
            }
            else
            {   //hero is closer on yAxis, actor should move on xAxis
                if (Pool.hero.compSprite.position.X > Actor.compSprite.position.X)
                { directionToHero = Direction.Right; }
                else { directionToHero = Direction.Left; }
            }
            


            //move randomly or move towards the hero
            if (directionToHero == Direction.None)
            {   //choose a random direction to move in
                Actor.compInput.direction = (Direction)GetRandom.Int(0, 8);
            }
            else
            {   //move towards the hero
                Actor.compInput.direction = directionToHero;
            }


            

            //determine if actor can see hero (so actor can chase)



            //determine if actor is close enough to hero to attack
            int attackArea = 14;
            if (yDistance < attackArea && xDistance < attackArea)
            {   //actor is close enough to hero to attack
                if (GetRandom.Int(0, 100) > 50) //randomly attack
                { Actor.compInput.attack = true; }
            }

            if(!Actor.compInput.attack)
            {   //if the actor isn't attacking, then randomly dash
                if (GetRandom.Int(0, 100) > 90)
                { Actor.compInput.dash = true; }
            }



            if (Pool.hero.state == Actor.State.Dead)
            {   //if hero is dead, reset AI input, randomly move + dash
                Input.ResetInputData(Actor.compInput);
                Actor.compInput.direction = (Direction)GetRandom.Int(0, 8);
                if (GetRandom.Int(0, 100) > 90) { Actor.compInput.dash = true; }
            }






            





            //choose an action to take

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


            //idea: AI could move only diagonally, that way they would slide around any blocking actors/objs
            //this would also make them harder to hit with projectiles :)


        }

    }
}