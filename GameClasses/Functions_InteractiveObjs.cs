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
    public static class Functions_InteractiveObjs
    {
        //some counters, prolly


        public static void Reset() { }

        public static void Spawn(int X, int Y) { }

        public static void Update()
        {
            //this is where we actually process all 
            //the crazy interactive ai stuff, instead of in ai_funcs
        }

        public static void Kill() { }






        //various interaction methods

        public static void BecomeDebris() { }

        public static void BecomeGore(Boolean dropSkeleton) { }

        public static void SelfClean() { }














        public static void SetType(InteractiveObject IntObj, InteractiveType Type)
        {





        }
    }
}