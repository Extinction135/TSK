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

        static Vector2 posRef = new Vector2();
        static Direction direction;

        //spawn relative to actor
        public static void Spawn(ObjType Type, Actor Actor)
        {
            //entities are spawned relative to actor, based on passed objType
            //set position reference to Actor's sprite position
            posRef.X = Actor.compSprite.position.X;
            posRef.Y = Actor.compSprite.position.Y;
            //get the actor's facing direction as cardinal direction
            direction = Functions_Direction.GetCardinalDirection(Actor.direction);


            #region Pickups

            if (Type == ObjType.PickupRupee)
            {   //place dropped rupee away from hero, cardinal = direction actor was pushed
                if (direction == Direction.Down) { posRef.X += 4; posRef.Y -= 12; } //place above
                else if (direction == Direction.Up) { posRef.X += 4; posRef.Y += 15; } //place below
                else if (direction == Direction.Right) { posRef.X -= 14; posRef.Y += 4; } //place left
                else if (direction == Direction.Left) { posRef.X += 14; posRef.Y += 4; } //place right
            }

            #endregion


            Spawn(Type, posRef.X, posRef.Y);
        }

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