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

        public static Boolean lazyMovement = false;
        public static float speed = 5f; //how fast the camera moves
        public static int deadzoneX = 50;
        public static int deadzoneY = 50;

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
        static Point point; //used in conversion functions below

        static Camera2D()
        {
            graphics = ScreenManager.game.GraphicsDevice;
            view = Matrix.Identity;
            translateCenter.Z = 0; //these two values dont change on a 2D camera
            translateBody.Z = 0;
            currentPosition = Vector2.Zero; //initially the camera is at 0,0
            targetPosition = Vector2.Zero;
            targetZoom = 1.0f;
        }

        public static Point ConvertScreenToWorld(int x, int y)
        {
            //get the camera position minus half width/height of render surface
            //this location is the world position of top left screen position
            //add the x,y to this location
            point.X = x + (int)currentPosition.X - 640 / 2;
            point.Y = y + (int)currentPosition.Y - 360 / 2;
            //this final value is the world position of the screen position
            return point;
        }

        public static Point ConvertWorldToScreen(int x, int y)
        {
            //subtract world position of top left screen position from x,y
            point.X = x - (int)currentPosition.X - 640 / 2;
            point.Y = y - (int)currentPosition.Y - 360 / 2;
            //this final value is the screen position of the world position
            return point;
        }

        public static void SetView()
        {
            //adapt the camera's center to the renderSurface.size
            translateCenter.X = ScreenManager.renderSurface.Width / 2;
            translateCenter.Y = ScreenManager.renderSurface.Height / 2;

            translateBody.X = -currentPosition.X;
            translateBody.Y = -currentPosition.Y;

            matZoom = Matrix.CreateScale(currentZoom, currentZoom, 1); //allows camera to properly zoom
            view = Matrix.CreateTranslation(translateBody) *
                    matRotation *
                    matZoom *
                    Matrix.CreateTranslation(translateCenter);
        }

        

        public static void Update(GameTime GameTime)
        {
            //discard sub-pixel values from position
            targetPosition.X = (int)targetPosition.X;
            targetPosition.Y = (int)targetPosition.Y;

            if(lazyMovement)
            {   //LAZY MATCHED CAMERA - waits for hero to move outside of deadzone before following
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
            }
            else //FAST MATCHED CAMERA - instantly follows hero
            { currentPosition = targetPosition; }

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