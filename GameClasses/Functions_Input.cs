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



        //input component methods 

        public static void ResetInputData(ComponentInput CompInput)
        {
            CompInput.direction = Direction.None;
            CompInput.attack = false;
            CompInput.use = false;
            CompInput.dash = false;
            CompInput.interact = false;
        }

        public static void FuzzInput(ComponentInput CompInput)
        {
            //the idea is to randomly change the input components values
            //to test game systems to see if they break. useful when new
            //systems are added. we could fuzz controller input, but thats
            //more front-facing. this fuzzes the back-end input systems,
            //those based around the input component class.

            CompInput.direction = Functions_Direction.GetRandomDirection();

            ran = Functions_Random.Int(0, 100);
            if (ran > 50)
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



        //keyboard related methods

        public static bool IsNewKeyPress(Keys key)
        { return (Input.currentKeyboardState.IsKeyDown(key) && Input.lastKeyboardState.IsKeyUp(key)); }

        public static bool IsKeyDown(Keys key)
        { return (Input.currentKeyboardState.IsKeyDown(key)); }

        public static bool IsNewKeyRelease(Keys key)
        { return (Input.lastKeyboardState.IsKeyDown(key) && Input.currentKeyboardState.IsKeyUp(key)); }

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



        //gamepad related methods

        public static bool IsButtonDown(GamePadState PadIns, Buttons button)
        {
            return (PadIns.IsButtonDown(button));
        }

        public static bool IsButtonUp(GamePadState PadIns, Buttons button)
        {
            return (PadIns.IsButtonUp(button));
        }

        public static void DumpGamePadState(GamePadState PadIns)
        {
            Debug.WriteLine("calling screen: " + ScreenManager.screens.Last().name);
            //direction info
            Debug.WriteLine("joystick X: " + PadIns.ThumbSticks.Left.X);
            Debug.WriteLine("joystick Y: " + PadIns.ThumbSticks.Left.Y);
            Debug.WriteLine("dpad direction: " + PadIns.DPad.ToString());
            //button info
            Debug.WriteLine("start button: " + PadIns.Buttons.Start.ToString());
            Debug.WriteLine("a button: " + PadIns.Buttons.A.ToString());
            Debug.WriteLine("b button: " + PadIns.Buttons.B.ToString());
            Debug.WriteLine("x button: " + PadIns.Buttons.X.ToString());
            Debug.WriteLine("y button: " + PadIns.Buttons.Y.ToString());
        }



        //gameInput related methods

        public static void ResetGameInput(GameInput InputIns)
        {   //directions
            InputIns.direction = Direction.None;
            //buttons
            InputIns.A = false;
            InputIns.X = false;
            InputIns.Y = false;
            InputIns.B = false;
            //start
            InputIns.Start = false;
        }

        public static void MapGamePadToInput(GamePadState PadIns, GameInput InputIns)
        {
            //map gamepad left joystick to gamePadDirection
            if (PadIns.ThumbSticks.Left.X > Input.deadzone &
                PadIns.ThumbSticks.Left.Y > Input.deadzone)
            { InputIns.direction = Direction.UpRight; }
            else if (PadIns.ThumbSticks.Left.X < -Input.deadzone &
                PadIns.ThumbSticks.Left.Y > Input.deadzone)
            { InputIns.direction = Direction.UpLeft; }

            else if (PadIns.ThumbSticks.Left.X > Input.deadzone &
                PadIns.ThumbSticks.Left.Y < -Input.deadzone)
            { InputIns.direction = Direction.DownRight; }
            else if (PadIns.ThumbSticks.Left.X < -Input.deadzone &
                PadIns.ThumbSticks.Left.Y < -Input.deadzone)
            { InputIns.direction = Direction.DownLeft; }

            else if (PadIns.ThumbSticks.Left.X > Input.deadzone)
            { InputIns.direction = Direction.Right; }
            else if (PadIns.ThumbSticks.Left.X < -Input.deadzone)
            { InputIns.direction = Direction.Left; }
            else if (PadIns.ThumbSticks.Left.Y > Input.deadzone)
            { InputIns.direction = Direction.Up; }
            else if (PadIns.ThumbSticks.Left.Y < -Input.deadzone)
            { InputIns.direction = Direction.Down; }

            //map gamepad Dpad to gamePadDirection - simple and accurate
            if (PadIns.IsButtonDown(Buttons.DPadRight) &
                PadIns.IsButtonDown(Buttons.DPadUp))
            { InputIns.direction = Direction.UpRight; }
            else if (PadIns.IsButtonDown(Buttons.DPadLeft) &
                PadIns.IsButtonDown(Buttons.DPadUp))
            { InputIns.direction = Direction.UpLeft; }
            else if (PadIns.IsButtonDown(Buttons.DPadLeft) &
                PadIns.IsButtonDown(Buttons.DPadDown))
            { InputIns.direction = Direction.DownLeft; }
            else if (PadIns.IsButtonDown(Buttons.DPadRight) &
                PadIns.IsButtonDown(Buttons.DPadDown))
            { InputIns.direction = Direction.DownRight; }
            else if (PadIns.IsButtonDown(Buttons.DPadRight))
            { InputIns.direction = Direction.Right; }
            else if (PadIns.IsButtonDown(Buttons.DPadLeft))
            { InputIns.direction = Direction.Left; }
            else if (PadIns.IsButtonDown(Buttons.DPadUp))
            { InputIns.direction = Direction.Up; }
            else if (PadIns.IsButtonDown(Buttons.DPadDown))
            { InputIns.direction = Direction.Down; }

            //map attack, use item, and interact button presses
            if (IsButtonDown(PadIns, Buttons.X)) { Input.Player1.X = true; }
            else if (IsButtonDown(PadIns, Buttons.Y)) { Input.Player1.Y = true; }
            else if (IsButtonDown(PadIns, Buttons.A)) { Input.Player1.A = true; }
            else if (IsButtonDown(PadIns, Buttons.B)) { Input.Player1.B = true; }

            //map start button input
            if (IsButtonDown(PadIns, Buttons.Start)) { Input.Player1.Start = true; }
        }

        public static void MapKeyboardToInput(KeyboardState KeyIns, GameInput InputIns)
        {
            //direction: WASD as expected

            //up
            if (IsKeyDown(Keys.W) & IsKeyDown(Keys.D)) //up right
            { InputIns.direction = Direction.UpRight; }
            else if (IsKeyDown(Keys.W) & IsKeyDown(Keys.A)) //up left
            { InputIns.direction = Direction.UpLeft; }
            else if (IsKeyDown(Keys.W))
            { InputIns.direction = Direction.Up; }
            //down
            else if (IsKeyDown(Keys.S) & IsKeyDown(Keys.D)) //down right
            { InputIns.direction = Direction.DownRight; }
            else if (IsKeyDown(Keys.S) & IsKeyDown(Keys.A)) //down left
            { InputIns.direction = Direction.DownLeft; }
            else if (IsKeyDown(Keys.S))
            { InputIns.direction = Direction.Down; }
            //horizontal left and right
            else if (IsKeyDown(Keys.D))
            { InputIns.direction = Direction.Right; }
            else if (IsKeyDown(Keys.A))
            { InputIns.direction = Direction.Left; }

            //button mapping: JILK to XYBA - A:K, B:L, X:J, Y:I
            if (IsKeyDown(Keys.J)) { InputIns.X = true; }
            else if (IsKeyDown(Keys.I)) { InputIns.Y = true; }
            else if (IsKeyDown(Keys.K)) { InputIns.A = true; }
            else if (IsKeyDown(Keys.L)) { InputIns.B = true; }

            //start button
            if (IsKeyDown(Keys.Enter)) { InputIns.Start = true; }
        }

        public static void MapGameInputToInputComponent(GameInput InputIns, ComponentInput CompInput)
        {   //reset actor input component
            ResetInputData(CompInput);
            //map direction
            CompInput.direction = InputIns.direction;

            /*
            //map buttons - method 1 : allows spamming of all buttons
            CompInput.interact = InputIns.A;
            CompInput.attack = InputIns.X;
            CompInput.use = InputIns.Y;
            CompInput.dash = InputIns.B;
            */

            //map buttons - method 2 : only dash is allowed to be spammed
            if (InputIns.A & !InputIns.A_Prev) { CompInput.interact = true; }
            if (InputIns.X & !InputIns.X_Prev) { CompInput.attack = true; }
            if (InputIns.Y & !InputIns.Y_Prev) { CompInput.use = true; }
            CompInput.dash = InputIns.B;
        }








        //this is called every frame, prior to screens calling HandleInput()
        public static void Update()
        {

            #region Set Cursor Position

            //the render surface is 640x360, the window size is 1280x720
            //divide the mouse position in half to get render surface pos
            Input.cursorPos.X = Input.currentMouseState.X / 2;
            Input.cursorPos.Y = Input.currentMouseState.Y / 2;
            //set cursor collision comp pos to world pos of the cursor's render surface pos
            Input.cursorColl.rec.Location =
                Functions_Camera2D.ConvertScreenToWorld(Input.cursorPos.X, Input.cursorPos.Y);

            #endregion


            #region  Setup Player1 Input - gamepad 1 and keyboard

            //capture previous program inputs
            Input.lastKeyboardState = Input.currentKeyboardState;
            Input.lastMouseState = Input.currentMouseState;
            //capture previous game inputs
            Input.Player1.direction_Prev = Input.Player1.direction;
            Input.Player1.A_Prev = Input.Player1.A;
            Input.Player1.B_Prev = Input.Player1.B;
            Input.Player1.X_Prev = Input.Player1.X;
            Input.Player1.Y_Prev = Input.Player1.Y;
            Input.Player1.Start_Prev = Input.Player1.Start;
            //clear player1 game inputs data
            ResetGameInput(Input.Player1);
            //get program inputs
            Input.currentGamePadState_1 = GamePad.GetState(PlayerIndex.One);
            Input.currentKeyboardState = Keyboard.GetState();
            Input.currentMouseState = Mouse.GetState();
            //map gamepad state to gameInput
            MapGamePadToInput(Input.currentGamePadState_1, Input.Player1);
            //map keyboard state to gameInput
            MapKeyboardToInput(Input.currentKeyboardState, Input.Player1);

            #endregion


            #region  Setup Player2 Input - gamepad 2 only

            //capture previous game inputs
            Input.Player2.direction_Prev = Input.Player2.direction;
            Input.Player2.A_Prev = Input.Player2.A;
            Input.Player2.B_Prev = Input.Player2.B;
            Input.Player2.X_Prev = Input.Player2.X;
            Input.Player2.Y_Prev = Input.Player2.Y;
            Input.Player2.Start_Prev = Input.Player2.Start;
            //clear player2 game inputs data
            ResetGameInput(Input.Player2);
            //get program inputs
            Input.currentGamePadState_2 = GamePad.GetState(PlayerIndex.Two);
            //map gamepad state to gameInput
            MapGamePadToInput(Input.currentGamePadState_2, Input.Player2);

            #endregion

        }
    }
}