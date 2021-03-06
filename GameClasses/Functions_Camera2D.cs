﻿using System;
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

        public static Point ConvertScreenToWorld(int x, int y)
        {   //get the camera position minus half width/height of render surface
            //this location is the world position of top left screen position
            //add the x,y to this location
            point.X = x + (int)Camera2D.currentPosition.X - 640 / 2;
            point.Y = y + (int)Camera2D.currentPosition.Y - 360 / 2;
            //this final value is the world position of the screen position
            return point;
        }

        public static Point ConvertWorldToScreen(int x, int y)
        {   //subtract world position of top left screen position from x,y
            point.X = x - ((int)Camera2D.currentPosition.X - 640 / 2);
            point.Y = y - ((int)Camera2D.currentPosition.Y - 360 / 2);
            //this final value is the screen position of the world position
            return point;
        }

        public static void SetView()
        {
            //adapt the camera's center to the renderSurface.size
            Camera2D.translateCenter.X = ScreenManager.renderSurface.Width / 2;
            Camera2D.translateCenter.Y = ScreenManager.renderSurface.Height / 2;
            Camera2D.translateCenter.Z = 0;

            Camera2D.translateBody.X = -Camera2D.currentPosition.X;
            Camera2D.translateBody.Y = -Camera2D.currentPosition.Y;
            Camera2D.translateBody.Z = 0;

            //allows camera to properly zoom
            Camera2D.matZoom = Matrix.CreateScale(Camera2D.currentZoom, Camera2D.currentZoom, 1); 
            Camera2D.view = Matrix.CreateTranslation(Camera2D.translateBody) *
                    Camera2D.matRotation * Camera2D.matZoom *
                    Matrix.CreateTranslation(Camera2D.translateCenter);
        }

        public static void Update()
        {
            //discard sub-pixel values from position
            Camera2D.targetPosition.X = (int)Camera2D.targetPosition.X;
            Camera2D.targetPosition.Y = (int)Camera2D.targetPosition.Y;

            if (Camera2D.tracks)
            {   //camera gradually moves to target
                //get distance between current and target
                Camera2D.distance = Camera2D.targetPosition - Camera2D.currentPosition;
                //check to see if camera is close enough to snap, or should move
                if (Math.Abs(Camera2D.distance.X) < 1)
                { Camera2D.currentPosition.X = Camera2D.targetPosition.X; }
                else //camera should move
                { Camera2D.currentPosition.X += Camera2D.distance.X * Camera2D.speed; }
                if (Math.Abs(Camera2D.distance.Y) < 1)
                { Camera2D.currentPosition.Y = Camera2D.targetPosition.Y; }
                else //camera should move
                { Camera2D.currentPosition.Y += Camera2D.distance.Y * Camera2D.speed; }
            }
            else //instantly follows target
            {
                Camera2D.currentPosition = Camera2D.targetPosition;
            }

            //discard sub-pixel values from position
            Camera2D.currentPosition.X = (int)Camera2D.currentPosition.X;
            Camera2D.currentPosition.Y = (int)Camera2D.currentPosition.Y;

            //Debug.WriteLine("cam target:" + Camera2D.targetPosition.X + ", " + Camera2D.targetPosition.Y);
            //Debug.WriteLine("cam curr:" + Camera2D.currentPosition.X + ", " + Camera2D.currentPosition.Y);
            //Debug.WriteLine("cam distance:" + Camera2D.distance.X + ", " + Camera2D.distance.Y);

            if (Camera2D.currentZoom != Camera2D.targetZoom)
            {   //gradually match the zoom
                if (Camera2D.currentZoom > Camera2D.targetZoom) { Camera2D.currentZoom -= Camera2D.zoomSpeed; } //zoom out
                if (Camera2D.currentZoom < Camera2D.targetZoom) { Camera2D.currentZoom += Camera2D.zoomSpeed; } //zoom in
                if (Math.Abs((Camera2D.currentZoom - Camera2D.targetZoom)) < 0.05f)
                { Camera2D.currentZoom = Camera2D.targetZoom; } //limit zoom
            }

            SetView();
        }

    }
}