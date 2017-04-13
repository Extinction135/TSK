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
    public static class InteractionFunctions
    {

        public static void Handle(Actor Actor, GameObject Obj)
        {
            //Obj is non-blocking
        }

        public static void Handle(GameObject Projectile, Actor Actor)
        {
            //no blocking checks have been done yet, actors always block tho

            //if a projectile collides with something, the projectile may get destroyed (end of lifetime)
            //Obj.lifeCounter = Obj.lifetime; //end the projectiles life
            //create an explosion effect here

            //every projectile pushes an actor it collides with
            MovementFunctions.Push(Actor.compMove, Projectile.direction, 1.0f);
            //we can modify the push amount based on the projectile type
            //some projectiles could push actors harder or softer

            if (Projectile.type == GameObject.Type.ProjectileSword)
            {

            }
        }

        public static void Handle(GameObject Projectile, GameObject Obj)
        {
            //Obj could be a projectile!
            //no blocking checks have been done yet

            //if a projectile collides with something, the projectile may get destroyed (end of lifetime)
            //Obj.lifeCounter = Obj.lifetime; //end the projectiles life
            //create an explosion effect here
        }

    }
}