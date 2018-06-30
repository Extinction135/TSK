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
    public static class Functions_Input
    {
        static int ran;



        public static void ResetInputData(ComponentInput CompInput)
        {
            CompInput.direction = Direction.None;
            CompInput.attack = false;
            CompInput.use = false;
            CompInput.dash = false;
            CompInput.interact = false;
        }

        public static void MapPlayerInput(ComponentInput CompInput)
        {   //hero has loaded into a level, via level screen
            ResetInputData(CompInput); //reset the input component

            //map attack, use item, and interact button presses
            if (IsNewButtonPress(Buttons.X)) { CompInput.attack = true; }
            else if (IsNewButtonPress(Buttons.Y)) { CompInput.use = true; }
            else if (IsNewButtonPress(Buttons.A)) { CompInput.interact = true; }

            //method 1 : hyper light drifter style dashing
            //if (IsNewButtonPress(Buttons.B)) { CompInput.dash = true; }
            //method 2 : lttp style-dashing + hyper light drifter style dashing
            if (IsButtonDown(Buttons.B)) { CompInput.dash = true; }
            //this essentially spams dash


            #region Implement Fuzzy input

            //the idea is to randomly change the input components values
            //to test game systems to see if they break. useful when new
            //systems are added. we could fuzz controller input, but thats
            //more front-facing. this fuzzes the back-end input systems,
            //those based around the input component class.

            if(Flags.FuzzyInput)
            {
                CompInput.direction = Functions_Direction.GetRandomDirection();

                ran = Functions_Random.Int(0, 100);
                if(ran > 50)
                {
                    //method 1 - multiple button mashing
                    if (Functions_Random.Int(0, 100) > 50)
                    { CompInput.attack = true; }
                    else { CompInput.attack = false; }

                    if (Functions_Random.Int(0, 100) > 50)
                    { CompInput.use = true; }
                    else { CompInput.use = false; }

                    if (Functions_Random.Int(0, 100) > 50)
                    { CompInput.interact = true; }
                    else { CompInput.interact = false; }

                    if (Functions_Random.Int(0, 100) > 50)
                    { CompInput.dash = true; }
                    else { CompInput.dash = false; }
                }
                else
                {
                    //method 2 - single button spamming
                    CompInput.attack = false;
                    CompInput.use = false;
                    CompInput.interact = false;
                    CompInput.dash = false;

                    ran = Functions_Random.Int(0, 100);

                    if (ran < 25) { CompInput.attack = true; }
                    else if (ran < 50) { CompInput.use = true; }
                    else if (ran < 75) { CompInput.interact = true; }
                    else { CompInput.dash = true; }
                }
            }

            #endregion


            //map direction post fuzz
            CompInput.direction = Input.gamePadDirection; 
            //open inventory with start button
            if (IsNewButtonPress(Buttons.Start))
            { ScreenManager.AddScreen(new ScreenInventory()); }
        }

        public static void SetInputState(ComponentInput CompInput, Actor Actor)
        {
            Actor.inputState = ActorState.Idle; //reset inputState
            Actor.compMove.direction = CompInput.direction; //set move direction
            if (CompInput.direction != Direction.None)
            {   //if there is directional input, then the actor is moving
                Actor.inputState = ActorState.Move;
                //the actor can only dash while moving
                if (CompInput.dash) { Actor.inputState = ActorState.Dash; }
            }
            //determine + set button inputs
            if (CompInput.attack) { Actor.inputState = ActorState.Attack; }
            else if (CompInput.use) { Actor.inputState = ActorState.Use; }
            else if (CompInput.interact) { Actor.inputState = ActorState.Interact; }
        }

        public static bool IsNewKeyPress(Keys key)
        { return (Input.currentKeyboardState.IsKeyDown(key) && Input.lastKeyboardState.IsKeyUp(key)); }

        public static bool IsKeyDown(Keys key)
        { return (Input.currentKeyboardState.IsKeyDown(key)); }

        public static bool IsNewKeyRelease(Keys key)
        { return (Input.lastKeyboardState.IsKeyDown(key) && Input.currentKeyboardState.IsKeyUp(key)); }

        public static bool IsNewButtonPress(Buttons button)
        {
            return (Input.currentGamePadState.IsButtonDown(button) && Input.lastGamePadState.IsButtonUp(button));
        }

        public static bool IsNewButtonRelease(Buttons button)
        { return (Input.lastGamePadState.IsButtonDown(button) && Input.currentGamePadState.IsButtonUp(button)); }

        public static bool IsButtonDown(Buttons button)
        { return (Input.currentGamePadState.IsButtonDown(button)); }

        public static bool IsButtonUp(Buttons button)
        { return (Input.currentGamePadState.IsButtonUp(button)); }

        public static bool IsNewMouseButtonPress(MouseButtons button)
        {
            if (button == MouseButtons.LeftButton)
            {
                return (
                    Input.currentMouseState.LeftButton == ButtonState.Pressed && 
                    Input.lastMouseState.LeftButton == ButtonState.Released);
            }
            else if (button == MouseButtons.RightButton)
            {
                return (
                    Input.currentMouseState.RightButton == ButtonState.Pressed && 
                    Input.lastMouseState.RightButton == ButtonState.Released);
            }
            else { return false; }
        }

        public static bool IsNewMouseButtonRelease(MouseButtons button)
        {
            if (button == MouseButtons.LeftButton)
            {
                return (
                    Input.lastMouseState.LeftButton == ButtonState.Pressed && 
                    Input.currentMouseState.LeftButton == ButtonState.Released);
            }
            else if (button == MouseButtons.RightButton)
            {
                return (
                    Input.lastMouseState.RightButton == ButtonState.Pressed && 
                    Input.currentMouseState.RightButton == ButtonState.Released);
            }
            else { return false; }
        }

        public static bool IsMouseButtonDown(MouseButtons button)
        {
            if (button == MouseButtons.LeftButton)
            { return (Input.currentMouseState.LeftButton == ButtonState.Pressed); }
            else if (button == MouseButtons.RightButton)
            { return (Input.currentMouseState.RightButton == ButtonState.Pressed); }
            else { return false; }
        }

        public static void Update()
        {
            Input.lastKeyboardState = Input.currentKeyboardState;
            Input.lastMouseState = Input.currentMouseState;
            Input.lastGamePadState = Input.currentGamePadState;

            Input.currentKeyboardState = Keyboard.GetState();
            Input.currentMouseState = Mouse.GetState();
            Input.currentGamePadState = GamePad.GetState(PlayerIndex.One);

            //the render surface is 640x360, the window size is 1280x720
            //divide the mouse position in half to get render surface pos
            Input.cursorPos.X = Input.currentMouseState.X / 2;
            Input.cursorPos.Y = Input.currentMouseState.Y / 2;
            //set cursor collision comp pos to world pos of the cursor's render surface pos
            Input.cursorColl.rec.Location = Functions_Camera2D.ConvertScreenToWorld(Input.cursorPos.X, Input.cursorPos.Y);

            //save the last gamePadDirection
            Input.lastGamePadDirection = Input.gamePadDirection;
            //reset the current game pad direction
            Input.gamePadDirection = Direction.None;

            //map gamepad left joystick to gamePadDirection
            if (Input.currentGamePadState.ThumbSticks.Left.X > Input.deadzone &
                Input.currentGamePadState.ThumbSticks.Left.Y > Input.deadzone)
            { Input.gamePadDirection = Direction.UpRight; }
            else if (Input.currentGamePadState.ThumbSticks.Left.X < -Input.deadzone &
                Input.currentGamePadState.ThumbSticks.Left.Y > Input.deadzone)
            { Input.gamePadDirection = Direction.UpLeft; }
            else if (Input.currentGamePadState.ThumbSticks.Left.X > Input.deadzone &
                Input.currentGamePadState.ThumbSticks.Left.Y < -Input.deadzone)
            { Input.gamePadDirection = Direction.DownRight; }
            else if (Input.currentGamePadState.ThumbSticks.Left.X < -Input.deadzone &
                Input.currentGamePadState.ThumbSticks.Left.Y < -Input.deadzone)
            { Input.gamePadDirection = Direction.DownLeft; }
            else if (Input.currentGamePadState.ThumbSticks.Left.X > Input.deadzone)
            { Input.gamePadDirection = Direction.Right; }
            else if (Input.currentGamePadState.ThumbSticks.Left.X < -Input.deadzone)
            { Input.gamePadDirection = Direction.Left; }
            else if (Input.currentGamePadState.ThumbSticks.Left.Y > Input.deadzone)
            { Input.gamePadDirection = Direction.Up; }
            else if (Input.currentGamePadState.ThumbSticks.Left.Y < -Input.deadzone)
            { Input.gamePadDirection = Direction.Down; }

            //map gamepad Dpad to gamePadDirection
            if (Input.currentGamePadState.IsButtonDown(Buttons.DPadRight) &
                Input.currentGamePadState.IsButtonDown(Buttons.DPadUp))
            { Input.gamePadDirection = Direction.UpRight; }
            else if (Input.currentGamePadState.IsButtonDown(Buttons.DPadLeft) &
                Input.currentGamePadState.IsButtonDown(Buttons.DPadUp))
            { Input.gamePadDirection = Direction.UpLeft; }
            else if (Input.currentGamePadState.IsButtonDown(Buttons.DPadLeft) &
                Input.currentGamePadState.IsButtonDown(Buttons.DPadDown))
            { Input.gamePadDirection = Direction.DownLeft; }
            else if (Input.currentGamePadState.IsButtonDown(Buttons.DPadRight) &
                Input.currentGamePadState.IsButtonDown(Buttons.DPadDown))
            { Input.gamePadDirection = Direction.DownRight; }
            else if (Input.currentGamePadState.IsButtonDown(Buttons.DPadRight))
            { Input.gamePadDirection = Direction.Right; }
            else if (Input.currentGamePadState.IsButtonDown(Buttons.DPadLeft))
            { Input.gamePadDirection = Direction.Left; }
            else if (Input.currentGamePadState.IsButtonDown(Buttons.DPadUp))
            { Input.gamePadDirection = Direction.Up; }
            else if (Input.currentGamePadState.IsButtonDown(Buttons.DPadDown))
            { Input.gamePadDirection = Direction.Down; }
        }

        public static void DumpControllerState()
        {
            Debug.WriteLine("calling screen: " + ScreenManager.screens.Last().name);
            //dump info on controller
            Debug.WriteLine("back button: " + Input.currentGamePadState.Buttons.Back.ToString());
            Debug.WriteLine("start button: " + Input.currentGamePadState.Buttons.Start.ToString());
            Debug.WriteLine("a button: " + Input.currentGamePadState.Buttons.A.ToString());
            Debug.WriteLine("b button: " + Input.currentGamePadState.Buttons.B.ToString());
            Debug.WriteLine("x button: " + Input.currentGamePadState.Buttons.X.ToString());
            Debug.WriteLine("y button: " + Input.currentGamePadState.Buttons.Y.ToString());
        }

    }
}