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
    public class InputHelper
    {
        public KeyboardState currentKeyboardState;
        public KeyboardState lastKeyboardState;

        public MouseState currentMouseState;
        public MouseState lastMouseState;

        public Point lastCursorPosition;
        public Point cursorPosition; //used to hold vector2 position as integer

        public GamePadState currentGamePadState;
        public GamePadState lastGamePadState;
        public float deadzone = 0.10f; //the amount of joystick movement classified as noise
        public Direction gamePadDirection;


        public InputHelper()
        {
            currentKeyboardState = new KeyboardState();
            currentMouseState = new MouseState();
            lastKeyboardState = new KeyboardState();
            lastMouseState = new MouseState();
            cursorPosition = new Point(0, 0);
            lastCursorPosition = new Point(0, 0);
            currentGamePadState = new GamePadState();
            lastGamePadState = new GamePadState();
            gamePadDirection = Direction.None;
        }



        public void Update(GameTime gameTime)
        {
            lastKeyboardState = currentKeyboardState;
            lastMouseState = currentMouseState;
            lastGamePadState = currentGamePadState;

            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            //track cursor position
            lastCursorPosition = cursorPosition;
            cursorPosition.X = (int)currentMouseState.X; //convert cursor position to int
            cursorPosition.Y = (int)currentMouseState.Y; //we don't need cursor position as a float

            //reset game pad direction
            gamePadDirection = Direction.None;

            //map gamepad left joystick to gamePadDirection
            if (currentGamePadState.ThumbSticks.Left.X > deadzone & currentGamePadState.ThumbSticks.Left.Y > deadzone)
            { gamePadDirection = Direction.UpRight; }
            else if (currentGamePadState.ThumbSticks.Left.X < -deadzone & currentGamePadState.ThumbSticks.Left.Y > deadzone)
            { gamePadDirection = Direction.UpLeft; }
            else if (currentGamePadState.ThumbSticks.Left.X > deadzone & currentGamePadState.ThumbSticks.Left.Y < -deadzone)
            { gamePadDirection = Direction.DownRight; }
            else if (currentGamePadState.ThumbSticks.Left.X < -deadzone & currentGamePadState.ThumbSticks.Left.Y < -deadzone)
            { gamePadDirection = Direction.DownLeft; }
            else if (currentGamePadState.ThumbSticks.Left.X > deadzone) { gamePadDirection = Direction.Right; }
            else if (currentGamePadState.ThumbSticks.Left.X < -deadzone) { gamePadDirection = Direction.Left; }
            else if (currentGamePadState.ThumbSticks.Left.Y > deadzone) { gamePadDirection = Direction.Up; }
            else if (currentGamePadState.ThumbSticks.Left.Y < -deadzone) { gamePadDirection = Direction.Down; }

            //map gamepad Dpad to gamePadDirection
            if (currentGamePadState.IsButtonDown(Buttons.DPadRight) & currentGamePadState.IsButtonDown(Buttons.DPadUp))
            { gamePadDirection = Direction.UpRight; }
            else if (currentGamePadState.IsButtonDown(Buttons.DPadLeft) & currentGamePadState.IsButtonDown(Buttons.DPadUp))
            { gamePadDirection = Direction.UpLeft; }
            else if (currentGamePadState.IsButtonDown(Buttons.DPadLeft) & currentGamePadState.IsButtonDown(Buttons.DPadDown))
            { gamePadDirection = Direction.DownLeft; }
            else if (currentGamePadState.IsButtonDown(Buttons.DPadRight) & currentGamePadState.IsButtonDown(Buttons.DPadDown))
            { gamePadDirection = Direction.DownRight; }
            else if (currentGamePadState.IsButtonDown(Buttons.DPadRight)) { gamePadDirection = Direction.Right; }
            else if (currentGamePadState.IsButtonDown(Buttons.DPadLeft)) { gamePadDirection = Direction.Left; }
            else if (currentGamePadState.IsButtonDown(Buttons.DPadUp)) { gamePadDirection = Direction.Up; }
            else if (currentGamePadState.IsButtonDown(Buttons.DPadDown)) { gamePadDirection = Direction.Down; }
        }



        //check for keyboard key presses and releases
        public bool IsNewKeyPress(Keys key) { return (currentKeyboardState.IsKeyDown(key) && lastKeyboardState.IsKeyUp(key)); }
        public bool IsKeyDown(Keys key) { return (currentKeyboardState.IsKeyDown(key)); }
        public bool IsNewKeyRelease(Keys key) { return (lastKeyboardState.IsKeyDown(key) && currentKeyboardState.IsKeyUp(key)); }

        //check for gamepad button presses and releases
        public bool IsNewButtonPress(Buttons button) { return (currentGamePadState.IsButtonDown(button) && lastGamePadState.IsButtonUp(button)); }
        public bool IsNewButtonRelease(Buttons button) { return (lastGamePadState.IsButtonDown(button) && currentGamePadState.IsButtonUp(button)); }

        //check to see if a controller button is down or up
        public Boolean IsButtonDown(Buttons button) { return (currentGamePadState.IsButtonDown(button)); }
        public Boolean IsButtonUp(Buttons button) { return (currentGamePadState.IsButtonUp(button)); }

        //Check to see the mouse button was pressed
        public bool IsNewMouseButtonPress(MouseButtons button)
        {
            if (button == MouseButtons.LeftButton)
            { return (currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released); }
            else if (button == MouseButtons.RightButton)
            { return (currentMouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Released); }
            else { return false; }
        }

        //Check to see the mouse button was released
        public bool IsNewMouseButtonRelease(MouseButtons button)
        {
            if (button == MouseButtons.LeftButton)
            { return (lastMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released); }
            else if (button == MouseButtons.RightButton)
            { return (lastMouseState.RightButton == ButtonState.Pressed && currentMouseState.RightButton == ButtonState.Released); }
            else { return false; }
        }

        //Check to see if the mouse button is being held down
        public Boolean IsMouseButtonDown(MouseButtons button)
        {
            if (button == MouseButtons.LeftButton)
            { return (currentMouseState.LeftButton == ButtonState.Pressed); }
            else if (button == MouseButtons.RightButton)
            { return (currentMouseState.RightButton == ButtonState.Pressed); }
            else { return false; }
        }
    }
}