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
    public static class Functions_Pickup
    {
        public static void Spawn(ObjType Type, float X, float Y)
        {   //get a pickup to spawn
            GameObject obj = Functions_Pool.GetPickup();
            obj.compMove.moving = true;
            //set direction to down
            obj.direction = Direction.Down;
            obj.compMove.direction = Direction.Down;
            //teleport the object to the proper location
            Functions_Movement.Teleport(obj.compMove, X, Y);
            //set the type, rotation, cellsize, & alignment
            Functions_GameObject.SetType(obj, Type);
            Functions_Component.Align(obj); //align upon birth
            //Debug.WriteLine("entity made: " + Type + " - location: " + X + ", " + Y);
        }
    }
}