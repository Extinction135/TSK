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
    public static class InputFunctions
    {

        public static void ResetInputData(ComponentInput Input)
        {
            Input.direction = Direction.None;
            Input.attack = false;
            Input.use = false;
            Input.dash = false;
            Input.interact = false;
        }

        public static void SetActorInputState(ComponentInput Input, Actor Actor)
        {
            Actor.inputState = Actor.State.Idle; //reset inputState
            Actor.compMove.direction = Input.direction; //set move direction
            if (Input.direction != Direction.None)
            {
                Actor.inputState = Actor.State.Move; //actor must be moving
                Actor.direction = Input.direction; //set actor moving/facing direction
            }
            //determine + set button inputs
            if (Input.attack) { Actor.inputState = Actor.State.Attack; }
            else if (Input.use) { Actor.inputState = Actor.State.Use; }
            else if (Input.dash) { Actor.inputState = Actor.State.Dash; }
            else if (Input.interact) { Actor.inputState = Actor.State.Interact; }
        }

        public static void MapPlayerInput(InputHelper Helper, ComponentInput Component)
        {   //maps input helper state to input component state
            //AI sets input component state directly, without using a controller/input helper abstraction
            ResetInputData(Component); //reset the component
            Component.direction = Helper.gamePadDirection;
            if (Helper.IsNewButtonPress(Buttons.X)) { Component.attack = true; }
            else if (Helper.IsNewButtonPress(Buttons.Y)) { Component.use = true; }
            else if (Helper.IsNewButtonPress(Buttons.B)) { Component.dash = true; }
            else if (Helper.IsNewButtonPress(Buttons.A)) { Component.interact = true; }
        }
    }
}