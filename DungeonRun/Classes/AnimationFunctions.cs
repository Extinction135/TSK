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
        {   //perform bounds checking for index
            if (Anim.index >= Anim.currentAnimation.Count)
            {
                if (Anim.loop) { Anim.index = 0; } //loop animation, or not
                else { Anim.index = (byte)(Anim.currentAnimation.Count-1); }
            }
            //update sprite's current frame to index value
            Sprite.currentFrame = Anim.currentAnimation[Anim.index];
            Anim.timer++;
            if (Anim.timer >= Anim.speed)
            {
                Anim.timer = 0; //reset animation timer
                Anim.index++; //increment index to next frame
            }
            //animate the scale back to 1.0f
            if (Sprite.scale > 1.0f) { Sprite.scale -= 0.01f; }
            else if (Sprite.scale < 1.0f) { Sprite.scale = 1.0f; }
        }

    }
}