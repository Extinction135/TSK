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
    public static class Functions_Scroll
    {
        static int i;



        public static void CreateColumn(Boolean Bkg, Boolean Flip, int Height, Vector2 Pos, List<ComponentSprite> List)
        {   //can create a scroll or background column
            byte xFrame = 6;
            if (Bkg) { xFrame = 14; }
            for (int i = 0; i < Height; i++)
            {   //create a blank sprite
                ComponentSprite sprite = new ComponentSprite(
                    Assets.mainSheet, new Vector2(0, 0),
                    new Byte4(0, 0, 0, 0), new Point(32, 16));
                if (Bkg) { sprite.cellSize.X = 16; }
                Functions_Component.UpdateCellSize(sprite);
                sprite.position.X = Pos.X; //set blank's pos
                sprite.position.Y = Pos.Y + (16 * i);
                //set sprite's frame
                if (i == 0) { sprite.currentFrame = new Byte4(xFrame, 0, 0, 0); } //head
                else if (i == Height - 1) { sprite.currentFrame = new Byte4(xFrame, 2, 0, 0); } //tail
                else { sprite.currentFrame = new Byte4(xFrame, 1, 0, 0); } //mid section
                sprite.flipHorizontally = Flip;
                List.Add(sprite); //add mod blank to list
            }
        }
        
        public static void AnimateOpen(Scroll Scroll)
        {   //animate the right pillar to the right
            if (Scroll.rightScroll[0].position.X < Scroll.endPos.X)
            {   //animate the rightScroll right
                for (i = 0; i < Scroll.rightScroll.Count; i++)
                {
                    Scroll.rightScroll[i].position.X += (Scroll.endPos.X - Scroll.rightScroll[i].position.X) / Scroll.animSpeed;
                    Scroll.rightScroll[i].position.X += 1;
                    if (Scroll.rightScroll[i].position.X > Scroll.endPos.X)
                    { Scroll.rightScroll[i].position.X = Scroll.endPos.X; }
                }
            }
            else { Scroll.displayState = DisplayState.Opened; }
        }

        public static void AnimateClosed(Scroll Scroll)
        {   //animate the left pillar to the right
            if (Scroll.leftScroll[0].position.X < Scroll.endPos.X - 32)
            {   //animate the rightScroll right
                for (i = 0; i < Scroll.leftScroll.Count; i++)
                {
                    Scroll.leftScroll[i].position.X += (Scroll.endPos.X - Scroll.leftScroll[i].position.X) / Scroll.animSpeed;
                    Scroll.leftScroll[i].position.X += 1;
                    if (Scroll.leftScroll[i].position.X > Scroll.endPos.X - 32)
                    { Scroll.leftScroll[i].position.X = Scroll.endPos.X - 32; }
                }
            }
            else { Scroll.displayState = DisplayState.Closed; }
        }

        public static void Draw(Scroll Scroll)
        {   //based on display state, draw bkg, title, headline
            if (Scroll.displayState == DisplayState.Opening)
            {
                for (i = 0; i < Scroll.scrollBkg.Count; i++)
                {   //if bkg sprite is left of animating right scroll, draw
                    if (Scroll.scrollBkg[i].position.X < Scroll.rightScroll[0].position.X)
                    { Functions_Draw.Draw(Scroll.scrollBkg[i]); }
                }
            }
            else if (Scroll.displayState == DisplayState.Closing)
            {
                for (i = 0; i < Scroll.scrollBkg.Count; i++)
                {   //if bkg sprite is right of animating left scroll, draw
                    if (Scroll.scrollBkg[i].position.X > Scroll.leftScroll[0].position.X)
                    { Functions_Draw.Draw(Scroll.scrollBkg[i]); }
                }
            }
            else if (Scroll.displayState == DisplayState.Opened)
            {   // if screen is open draw bkg, title, headerline
                for (i = 0; i < Scroll.scrollBkg.Count; i++)
                { Functions_Draw.Draw(Scroll.scrollBkg[i]); }
                Functions_Draw.Draw(Scroll.title);
                ScreenManager.spriteBatch.Draw(
                    Assets.dummyTexture, Scroll.headerline,
                    Assets.colorScheme.windowInset);
            }
            //always draw scroll pillars / ends
            for (i = 0; i < Scroll.leftScroll.Count; i++)
            {   //these lists will always have same count
                Functions_Draw.Draw(Scroll.leftScroll[i]);
                Functions_Draw.Draw(Scroll.rightScroll[i]);
            }
        }

    }
}