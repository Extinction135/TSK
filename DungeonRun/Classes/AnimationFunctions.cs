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
    public static class AnimationFunctions
    {
        public static void Animate(ComponentAnimation Anim, ComponentSprite Sprite)
        {
            //perform bounds checking for index
            if (Anim.index >= Anim.currentAnimation.Count) { Anim.index = 0; }
            //update sprite's current frame to index value
            Sprite.currentFrame = Anim.currentAnimation[Anim.index];
            Anim.timer++;
            if (Anim.timer >= Anim.speed)
            {
                Anim.timer = 0; //reset animation timer
                Anim.index++; //increment index to next frame
            }
        }
    }
}