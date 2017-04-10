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
    public static class Camera2D
    {
        public static GraphicsDevice graphics;
        public static ScreenManager screenManager;

        public static float speed = 5f; //how fast the camera moves
        public static int deadzoneX = 1;
        public static int deadzoneY = 1;
        public static Matrix view;
        public static float targetZoom = 1.0f;
        public static float zoomSpeed = 0.05f;
        public static Vector2 currentPosition;
        public static Vector2 targetPosition;

        static Matrix matRotation = Matrix.CreateRotationZ(0.0f);
        static Matrix matZoom;
        static Vector3 translateCenter;
        static Vector3 translateBody;
        static float currentZoom = 1.0f;
        static Vector2 distance;
        static Boolean followX = true;
        static Boolean followY = true;

        public static Matrix projection;
        static Vector3 T;
        static Point t;

        public static Point ConvertScreenToWorld(int x, int y)
        {   //converts screen position to world position
            projection = Matrix.CreateOrthographicOffCenter(0f, graphics.Viewport.Width, graphics.Viewport.Height, 0f, 0f, 1f);
            T.X = x; T.Y = y; T.Z = 0;
            T = graphics.Viewport.Unproject(T, projection, view, Matrix.Identity);
            t.X = (int)T.X; t.Y = (int)T.Y; return t;
        }

        public static Point ConvertWorldToScreen(int x, int y)
        {   //converts world position to screen position
            projection = Matrix.CreateOrthographicOffCenter(0f, graphics.Viewport.Width, graphics.Viewport.Height, 0f, 0f, 1f);
            T.X = x; T.Y = y; T.Z = 0;
            T = graphics.Viewport.Project(T, projection, view, Matrix.Identity);
            t.X = (int)T.X; t.Y = (int)T.Y; return t;
        }

        public static void SetView()
        {
            //adapt the camera's center to the renderSurface.size
            translateCenter.X = screenManager.renderSurface.Width / 2;
            translateCenter.Y = screenManager.renderSurface.Height / 2;

            translateBody.X = -currentPosition.X;
            translateBody.Y = -currentPosition.Y;

            matZoom = Matrix.CreateScale(currentZoom, currentZoom, 1); //allows camera to properly zoom
            view = Matrix.CreateTranslation(translateBody) *
                    matRotation *
                    matZoom *
                    Matrix.CreateTranslation(translateCenter);
        }

        public static void Initialize(ScreenManager ScreenManager)
        {
            screenManager = ScreenManager;
            graphics = screenManager.game.GraphicsDevice;

            view = Matrix.Identity;
            translateCenter.Z = 0; //these two values dont change on a 2D camera
            translateBody.Z = 0;
            currentPosition = Vector2.Zero; //initially the camera is at 0,0
            targetPosition = Vector2.Zero;
            targetZoom = 1.0f;
        }

        public static void Update(GameTime GameTime)
        {
            //discard sub-pixel values from position
            targetPosition.X = (int)targetPosition.X;
            targetPosition.Y = (int)targetPosition.Y;

            //Lazy Camera Presets
            /*
            //the default preset (fast)
            speed = 5f;
            deadzoneX = 1;
            deadzoneY = 1;

            //a responsive preset
            speed = 5f;
            deadzoneX = 50;
            deadzoneY = 50;

            //a slow preset
            speed = 3f;
            deadzoneX = 100;
            deadzoneY = 70;
            */

            /*
            //a responsive preset
            speed = 5f;
            deadzoneX = 50;
            deadzoneY = 50;

            //LAZY MATCHED CAMERA - waits for hero to move outside of deadzone before following
            distance = targetPosition - currentPosition; //get distance between current and target
            //check to see if camera is close enough to snap positions
            if (Math.Abs(distance.X) < 1) { currentPosition.X = targetPosition.X; followX = false; }
            if (Math.Abs(distance.Y) < 1) { currentPosition.Y = targetPosition.Y; followY = false; }
            //determine if we should track the hero, per axis (deadzone)
            if (Math.Abs(distance.X) > deadzoneX) { followX = true; }
            if (Math.Abs(distance.Y) > deadzoneY) { followY = true; }
            //if we are following, update current position based on distance and speed
            if (followX) { currentPosition.X += distance.X * speed * (float)GameTime.ElapsedGameTime.TotalSeconds; }
            if (followY) { currentPosition.Y += distance.Y * speed * (float)GameTime.ElapsedGameTime.TotalSeconds; }
            */

            //FAST MATCHED CAMERA - instantly follows hero
            currentPosition = targetPosition;

            //discard sub-pixel values from position
            currentPosition.X = (int)currentPosition.X;
            currentPosition.Y = (int)currentPosition.Y;

            if (currentZoom != targetZoom)
            {   //gradually match the zoom
                if (currentZoom > targetZoom) { currentZoom -= zoomSpeed; } //zoom out
                if (currentZoom < targetZoom) { currentZoom += zoomSpeed; } //zoom in
                if (Math.Abs((currentZoom - targetZoom)) < 0.05f) { currentZoom = targetZoom; } //limit zoom
            }
            SetView();
        }
    }
}