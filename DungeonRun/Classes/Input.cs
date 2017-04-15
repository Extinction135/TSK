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
    public static class Input
    {
        public static KeyboardState currentKeyboardState = new KeyboardState();
        public static KeyboardState lastKeyboardState = new KeyboardState();

        public static MouseState currentMouseState = new MouseState();
        public static MouseState lastMouseState = new MouseState();

        public static Point cursorPos = new Point(0,0);
        public static ComponentCollision cursorColl = new ComponentCollision();

        public static GamePadState currentGamePadState = new GamePadState();
        public static GamePadState lastGamePadState = new GamePadState();

        public static float deadzone = 0.10f; //the amount of joystick movement classified as noise
        public static Direction gamePadDirection = Direction.None;



        public static void Initialize()
        {
            cursorColl.rec.Width = 4;
            cursorColl.rec.Height = 4;
            cursorColl.blocking = false;
            cursorColl.active = true;
        }

        public static void Update(GameTime gameTime)
        {
            lastKeyboardState = currentKeyboardState;
            lastMouseState = currentMouseState;
            lastGamePadState = currentGamePadState;

            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            //the render surface is 640x360, the window size is 1280x720
            //divide the mouse position in half to get render surface pos
            cursorPos.X = currentMouseState.X / 2;
            cursorPos.Y = currentMouseState.Y / 2;
            //set cursor collision comp pos to world pos of the cursor's render surface pos
            cursorColl.rec.Location = Camera2D.ConvertScreenToWorld(cursorPos.X, cursorPos.Y);

            //reset game pad direction
            gamePadDirection = Direction.None;

            //map gamepad left joystick to gamePadDirection
            if (currentGamePadState.ThumbSticks.Left.X > deadzone &
                currentGamePadState.ThumbSticks.Left.Y > deadzone)
            { gamePadDirection = Direction.UpRight; }
            else if (currentGamePadState.ThumbSticks.Left.X < -deadzone &
                currentGamePadState.ThumbSticks.Left.Y > deadzone)
            { gamePadDirection = Direction.UpLeft; }
            else if (currentGamePadState.ThumbSticks.Left.X > deadzone &
                currentGamePadState.ThumbSticks.Left.Y < -deadzone)
            { gamePadDirection = Direction.DownRight; }
            else if (currentGamePadState.ThumbSticks.Left.X < -deadzone &
                currentGamePadState.ThumbSticks.Left.Y < -deadzone)
            { gamePadDirection = Direction.DownLeft; }
            else if (currentGamePadState.ThumbSticks.Left.X > deadzone)
            { gamePadDirection = Direction.Right; }
            else if (currentGamePadState.ThumbSticks.Left.X < -deadzone)
            { gamePadDirection = Direction.Left; }
            else if (currentGamePadState.ThumbSticks.Left.Y > deadzone)
            { gamePadDirection = Direction.Up; }
            else if (currentGamePadState.ThumbSticks.Left.Y < -deadzone)
            { gamePadDirection = Direction.Down; }

            //map gamepad Dpad to gamePadDirection
            if (currentGamePadState.IsButtonDown(Buttons.DPadRight) &
                currentGamePadState.IsButtonDown(Buttons.DPadUp))
            { gamePadDirection = Direction.UpRight; }
            else if (currentGamePadState.IsButtonDown(Buttons.DPadLeft) &
                currentGamePadState.IsButtonDown(Buttons.DPadUp))
            { gamePadDirection = Direction.UpLeft; }
            else if (currentGamePadState.IsButtonDown(Buttons.DPadLeft) &
                currentGamePadState.IsButtonDown(Buttons.DPadDown))
            { gamePadDirection = Direction.DownLeft; }
            else if (currentGamePadState.IsButtonDown(Buttons.DPadRight) &
                currentGamePadState.IsButtonDown(Buttons.DPadDown))
            { gamePadDirection = Direction.DownRight; }
            else if (currentGamePadState.IsButtonDown(Buttons.DPadRight))
            { gamePadDirection = Direction.Right; }
            else if (currentGamePadState.IsButtonDown(Buttons.DPadLeft))
            { gamePadDirection = Direction.Left; }
            else if (currentGamePadState.IsButtonDown(Buttons.DPadUp))
            { gamePadDirection = Direction.Up; }
            else if (currentGamePadState.IsButtonDown(Buttons.DPadDown))
            { gamePadDirection = Direction.Down; }
        }



        public static bool IsNewKeyPress(Keys key)
        { return (currentKeyboardState.IsKeyDown(key) && lastKeyboardState.IsKeyUp(key)); }

        public static bool IsKeyDown(Keys key)
        { return (currentKeyboardState.IsKeyDown(key)); }

        public static bool IsNewKeyRelease(Keys key)
        { return (lastKeyboardState.IsKeyDown(key) && currentKeyboardState.IsKeyUp(key)); }
        
        public static bool IsNewButtonPress(Buttons button)
        { return (currentGamePadState.IsButtonDown(button) && lastGamePadState.IsButtonUp(button)); }

        public static bool IsNewButtonRelease(Buttons button)
        { return (lastGamePadState.IsButtonDown(button) && currentGamePadState.IsButtonUp(button)); }
        
        public static bool IsButtonDown(Buttons button)
        { return (currentGamePadState.IsButtonDown(button)); }

        public static bool IsButtonUp(Buttons button)
        { return (currentGamePadState.IsButtonUp(button)); }

        public static bool IsNewMouseButtonPress(MouseButtons button)
        {
            if (button == MouseButtons.LeftButton)
            { return (currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released); }
            else if (button == MouseButtons.RightButton)
            { return (currentMouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Released); }
            else { return false; }
        }

        public static bool IsNewMouseButtonRelease(MouseButtons button)
        {
            if (button == MouseButtons.LeftButton)
            { return (lastMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released); }
            else if (button == MouseButtons.RightButton)
            { return (lastMouseState.RightButton == ButtonState.Pressed && currentMouseState.RightButton == ButtonState.Released); }
            else { return false; }
        }

        public static bool IsMouseButtonDown(MouseButtons button)
        {
            if (button == MouseButtons.LeftButton)
            { return (currentMouseState.LeftButton == ButtonState.Pressed); }
            else if (button == MouseButtons.RightButton)
            { return (currentMouseState.RightButton == ButtonState.Pressed); }
            else { return false; }
        }

        public static void ResetInputData(ComponentInput CompInput)
        {
            CompInput.direction = Direction.None;
            CompInput.attack = false;
            CompInput.use = false;
            CompInput.dash = false;
            CompInput.interact = false;
        }

        public static void MapPlayerInput(ComponentInput CompInput)
        {   //maps input helper state to input component state
            //AI sets input component state directly, without using a controller/input helper abstraction
            CompInput.direction = gamePadDirection;
            if (IsNewButtonPress(Buttons.X)) { CompInput.attack = true; }
            else if (IsNewButtonPress(Buttons.Y)) { CompInput.use = true; }
            else if (IsNewButtonPress(Buttons.B)) { CompInput.dash = true; }
            else if (IsNewButtonPress(Buttons.A)) { CompInput.interact = true; }
        }

        public static void SetInputState(ComponentInput CompInput, Actor Actor)
        {
            Actor.inputState = Actor.State.Idle; //reset inputState
            Actor.compMove.direction = CompInput.direction; //set move direction
            if (CompInput.direction != Direction.None)
            {   //if there is directional input, then the actor is moving
                Actor.inputState = Actor.State.Move;
                //the actor can only dash while moving
                if (CompInput.dash) { Actor.inputState = Actor.State.Dash; }
            }
            //determine + set button inputs
            if (CompInput.attack) { Actor.inputState = Actor.State.Attack; }
            else if (CompInput.use) { Actor.inputState = Actor.State.Use; }
            else if (CompInput.interact) { Actor.inputState = Actor.State.Interact; }
        }

    }
}