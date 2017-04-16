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
    public static class MusicFunctions
    {

        public enum FadeState { FadeIn, FadeComplete, FadeOut, Silent }
        public static FadeState fadeState = FadeState.Silent;
        public static float fadeSpeed = 0.005f;
        public static float maxVolume = 1.0f;

        public static void LoadMusic()
        {
            if (fadeState == FadeState.Silent)
            {   //if music is silent, then we can switch tracks
                //dispose old music instance + track
                if (Assets.musicIns != null) { Assets.musicIns.Dispose(); }
                if (Assets.musicTrack != null) { Assets.musicTrack.Dispose(); }
                //load the new music track + instance
                Assets.musicTrack = Assets.content.Load<SoundEffect>(@"MusicDungeonA");
                Assets.musicIns = Assets.musicTrack.CreateInstance();
                //loop instance and play it
                Assets.musicIns.IsLooped = true;
                Assets.musicIns.Volume = 0.0f;
                Assets.musicIns.Play();
                fadeState = FadeState.FadeIn;
            }
            else
            {
                //if the music isn't silent, fade the music out
                fadeState = FadeState.FadeOut;
            }
        }

        public static void Update()
        {
            //fade in/out
            if (fadeState == FadeState.FadeIn)
            {
                //music volume CANNOT exceed 1.0f, else program error
                if (Assets.musicIns.Volume + fadeSpeed >= maxVolume)
                { Assets.musicIns.Volume = maxVolume; }
                else { Assets.musicIns.Volume += fadeSpeed; }
                //check to see if music has reached maxVolume
                if (Assets.musicIns.Volume == maxVolume)
                { fadeState = FadeState.FadeComplete; }
            }
            else if (fadeState == FadeState.FadeComplete) { }
            else if (fadeState == FadeState.FadeOut)
            {
                //music volume CANNOT be negative, else program error
                if (Assets.musicIns.Volume - fadeSpeed <= 0.0f)
                { Assets.musicIns.Volume = 0.0f; }
                else { Assets.musicIns.Volume -= fadeSpeed; }
                //check to see if music has reached maxVolume
                if (Assets.musicIns.Volume == 0.0f)
                { fadeState = FadeState.Silent; }
            }
        }

    }
}