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
    public class ComponentInput
    {
        Actor actor;
        //InputData that an actor needs to know about
        public Direction direction;
        public Boolean attack;
        public Boolean use;
        public Boolean dash;
        public Boolean interact;

        public ComponentInput(Actor Actor) { actor = Actor; ResetInputData(); }

        public void ResetInputData()
        {
            direction = Direction.None;
            attack = false; use = false; dash = false; interact = false;
        }

        public void SetInputState()
        {
            actor.inputState = Actor.State.Idle; //reset inputState

            actor.compMove.direction = direction; //set move direction
            if (direction != Direction.None)
            {
                actor.inputState = Actor.State.Move; //actor must be moving
                actor.direction = direction; //set actor moving/facing direction
            }

            //determine + set button inputs
            if (attack) { actor.inputState = Actor.State.Attack; }
            else if (use) { actor.inputState = Actor.State.Use; }
            else if (dash) { actor.inputState = Actor.State.Dash; }
            else if (interact) { actor.inputState = Actor.State.Interact; }
        }

        public void HandlePlayerInput(InputHelper Input)
        {
            ResetInputData();

            //map InputHelper to InputData
            direction = Input.gamePadDirection;
            if (Input.IsNewButtonPress(Buttons.X)) { attack = true; }
            else if (Input.IsNewButtonPress(Buttons.Y)) { use = true; }
            else if (Input.IsNewButtonPress(Buttons.B)) { dash = true; }
            else if(Input.IsNewButtonPress(Buttons.A)) { interact = true; }
        }

        //AI sets input data directly, then mapInput runs
    }
}