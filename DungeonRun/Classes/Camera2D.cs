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
    public class Camera2D
    {
        public static GraphicsDevice graphics;
        public ScreenManager screenManager;


        //the important variables
        public float speed = 5f; //how fast the camera moves
        public int deadzoneX = 1;
        public int deadzoneY = 1;



        public Matrix matRotation = Matrix.CreateRotationZ(0.0f);
        public Matrix matZoom;
        public Vector3 translateCenter;
        public Vector3 translateBody;
        public Matrix view;

        public Vector2 distance;

        public float currentZoom = 1.0f;
        public float targetZoom = 1.0f;
        public float zoomSpeed = 0.05f;

        public Boolean followX = true;
        public Boolean followY = true;
        public Vector2 currentPosition;
        public Vector2 targetPosition;




        public Matrix projection;
        Vector3 T; Point t;
        public Point ConvertScreenToWorld(int x, int y)
        {   //converts screen position to world position
            projection = Matrix.CreateOrthographicOffCenter(0f, graphics.Viewport.Width, graphics.Viewport.Height, 0f, 0f, 1f);
            T.X = x; T.Y = y; T.Z = 0;
            T = graphics.Viewport.Unproject(T, projection, view, Matrix.Identity);
            t.X = (int)T.X; t.Y = (int)T.Y; return t;
        }
        public Point ConvertWorldToScreen(int x, int y)
        {   //converts world position to screen position
            projection = Matrix.CreateOrthographicOffCenter(0f, graphics.Viewport.Width, graphics.Viewport.Height, 0f, 0f, 1f);
            T.X = x; T.Y = y; T.Z = 0;
            T = graphics.Viewport.Project(T, projection, view, Matrix.Identity);
            t.X = (int)T.X; t.Y = (int)T.Y; return t;
        }





        public void SetView()
        {
            //this only works if viewport.size matches screenManager.renderSurface.size
            //translateCenter.X = (int)graphics.Viewport.Width / 2f;
            //translateCenter.Y = (int)graphics.Viewport.Height / 2f;

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


        public Camera2D(ScreenManager ScreenManager)
        {
            screenManager = ScreenManager;
            graphics = screenManager.game.GraphicsDevice;

            view = Matrix.Identity;
            translateCenter.Z = 0; //these two values dont change on a 2D camera
            translateBody.Z = 0;
            currentPosition = Vector2.Zero; //initially the camera is at 0,0
            targetPosition = Vector2.Zero;
            SetView();
        }




        public void Update(GameTime GameTime)
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