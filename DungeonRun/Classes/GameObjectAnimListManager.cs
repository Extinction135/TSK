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
    public static class GameObjectAnimListManager
    {
        




        static List<Byte4> Exit;


        static GameObjectAnimListManager()
        {
            //define animation lists for each gameObject.type
            Exit = new List<Byte4> { new Byte4(1, 0, 0, 0) };



        }




        //sets an object's current animation based on the object type
        public static void SetAnimationList(GameObject Obj)
        {
            if (Obj.type == GameObject.Type.Exit) { Obj.compAnim.currentAnimation = Exit; }

            //Obj.compAnim.currentAnimation = ObjsAnimationSet[Obj.type as int]
            //convert the enum string to an int value, then use the value as index in a list of byte4 animations
            //so we dont have to check the type at all
        }
    }
}