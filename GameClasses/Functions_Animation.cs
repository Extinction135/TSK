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
    public static class Functions_Animation
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
        }

        public static void ScaleSpriteUp(ComponentSprite Sprite)
        {   //scale sprite up to 1.0, lock to 1.0
            if (Sprite.scale < 1.0f)
            {
                Sprite.scale += 0.07f;
                if (Sprite.scale > 1.0f) { Sprite.scale = 1.0f; }
            }
        }

        public static void ScaleSpriteDown(ComponentSprite Sprite)
        {   //scale sprite down to 1.0, lock to 1.0
            if (Sprite.scale > 1.0f)
            {
                Sprite.scale -= 0.07f;
                if (Sprite.scale < 1.0f) { Sprite.scale = 1.0f; }
            }
        }


        //functions for types of animation movement
        //move sprite right to target, easeIn
        //Title.compSprite.position.X += (TargetPos.X - Title.compSprite.position.X) / Title.animSpeed;
        //Title.compSprite.position.X += 1; //always move at least 1 pixel
    }
}