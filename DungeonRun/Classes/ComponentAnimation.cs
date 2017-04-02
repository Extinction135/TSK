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
    public class ComponentAnimation
    {
        ComponentSprite spriteComp; //this component changes the state of it's sprite component ref
        public List<Byte4> currentAnimation; //a list of byte4 representing frames of an animation
        public byte index = 0; //where in the currentAnimation list the animation is (animation index)

        public byte speed = 10; //how many frames should elapse before animation is updated (limits animation speed)
        public byte timer = 0; //how many frames have elapsed since last animation update (counts frames) @ 60fps


        public ComponentAnimation(ComponentSprite SpriteComp) { spriteComp = SpriteComp; }
        

        public void Animate()
        {
            //perform bounds checking for index
            if (index >= currentAnimation.Count) { index = 0; }

            //update sprite's current frame to index value
            spriteComp.currentFrame = currentAnimation[index]; 

            timer++;
            if (timer >= speed)
            {
                timer = 0; //reset animation timer
                index++; //increment index to next frame
            }
        }
    }
}