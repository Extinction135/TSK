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
    public static class Functions_Camera2D
    {

        public static Point point; //used in conversion functions below

        public static Point ConvertScreenToWorld(Camera2D Cam, int x, int y)
        {   //get the camera position minus half width/height of render surface
            //this location is the world position of top left screen position
            //add the x,y to this location
            point.X = x + (int)Cam.currentPosition.X - 640 / 2;
            point.Y = y + (int)Cam.currentPosition.Y - 360 / 2;
            //this final value is the world position of the screen position
            return point;
        }

        public static Point ConvertWorldToScreen(Camera2D Cam, int x, int y)
        {   //subtract world position of top left screen position from x,y
            point.X = x - (int)Cam.currentPosition.X - 640 / 2;
            point.Y = y - (int)Cam.currentPosition.Y - 360 / 2;
            //this final value is the screen position of the world position
            return point;
        }

        public static void SetView(Camera2D Cam)
        {
            //adapt the camera's center to the renderSurface.size
            Cam.translateCenter.X = ScreenManager.renderSurface.Width / 2;
            Cam.translateCenter.Y = ScreenManager.renderSurface.Height / 2;

            Cam.translateBody.X = -Cam.currentPosition.X;
            Cam.translateBody.Y = -Cam.currentPosition.Y;

            Cam.matZoom = Matrix.CreateScale(Cam.currentZoom, Cam.currentZoom, 1); //allows camera to properly zoom
            Cam.view = Matrix.CreateTranslation(Cam.translateBody) *
                    Cam.matRotation *
                    Cam.matZoom *
                    Matrix.CreateTranslation(Cam.translateCenter);
        }

        public static void Update(Camera2D Cam, GameTime GameTime)
        {
            //discard sub-pixel values from position
            Cam.targetPosition.X = (int)Cam.targetPosition.X;
            Cam.targetPosition.Y = (int)Cam.targetPosition.Y;

            if (Cam.lazyMovement)
            {   //LAZY MATCHED CAMERA - waits for hero to move outside of deadzone before following
                Cam.distance = Cam.targetPosition - Cam.currentPosition; //get distance between current and target

                //check to see if camera is close enough to snap positions
                if (Math.Abs(Cam.distance.X) < 1)
                { Cam.currentPosition.X = Cam.targetPosition.X; Cam.followX = false; }
                if (Math.Abs(Cam.distance.Y) < 1)
                { Cam.currentPosition.Y = Cam.targetPosition.Y; Cam.followY = false; }

                //determine if we should track the hero, per axis (deadzone)
                if (Math.Abs(Cam.distance.X) > Cam.deadzoneX) { Cam.followX = true; }
                if (Math.Abs(Cam.distance.Y) > Cam.deadzoneY) { Cam.followY = true; }

                //if we are following, update current position based on distance and speed
                if (Cam.followX)
                { Cam.currentPosition.X += Cam.distance.X * Cam.speed * (float)GameTime.ElapsedGameTime.TotalSeconds; }
                if (Cam.followY)
                { Cam.currentPosition.Y += Cam.distance.Y * Cam.speed * (float)GameTime.ElapsedGameTime.TotalSeconds; }
            }
            else //FAST MATCHED CAMERA - instantly follows hero
            { Cam.currentPosition = Cam.targetPosition; }

            //discard sub-pixel values from position
            Cam.currentPosition.X = (int)Cam.currentPosition.X;
            Cam.currentPosition.Y = (int)Cam.currentPosition.Y;

            if (Cam.currentZoom != Cam.targetZoom)
            {   //gradually match the zoom
                if (Cam.currentZoom > Cam.targetZoom) { Cam.currentZoom -= Cam.zoomSpeed; } //zoom out
                if (Cam.currentZoom < Cam.targetZoom) { Cam.currentZoom += Cam.zoomSpeed; } //zoom in
                if (Math.Abs((Cam.currentZoom - Cam.targetZoom)) < 0.05f)
                { Cam.currentZoom = Cam.targetZoom; } //limit zoom
            }
            SetView(Cam);
        }

    }
}