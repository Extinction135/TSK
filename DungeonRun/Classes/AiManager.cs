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
    public static class AiManager
    {

        public static void Think(ComponentInput Input)
        {
            InputFunctions.ResetInputData(Input);

            //choose a random direction to move in
            Input.direction = (Direction)Global.Random.Next(0, 8);

            //randomly dash
            if (Global.Random.Next(0, 100) > 80) { Input.dash = true; }




            //choose an action to take
            /*
            if (Input.IsNewButtonPress(Buttons.X)) { attack = true; }
            else if (Input.IsNewButtonPress(Buttons.Y)) { use = true; }
            else if (Input.IsNewButtonPress(Buttons.B)) { dash = true; }
            else if (Input.IsNewButtonPress(Buttons.A)) { interact = true; }
            */

            //(we'll need to have a reference to the hero AND actor)
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