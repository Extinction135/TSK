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
    public static class Functions_ScreenRec
    {
        public static void Fade(ScreenRec Rec)
        {   //fadeIn or Out based on Rec.fadeState
            if (Rec.fadeState == FadeState.FadeIn)
            {
                Rec.alpha += Rec.fadeInSpeed;
                if (Rec.alpha >= Rec.maxAlpha)
                {
                    Rec.alpha = Rec.maxAlpha;
                    Rec.fadeState = FadeState.FadeComplete;
                }
            }
            else if(Rec.fadeState == FadeState.FadeOut)
            {
                Rec.alpha -= Rec.fadeOutSpeed;
                if (Rec.alpha <= 0.0f)
                {
                    Rec.alpha = 0.0f;
                    Rec.fadeState = FadeState.FadeComplete;
                }
            }
        }
    }
}