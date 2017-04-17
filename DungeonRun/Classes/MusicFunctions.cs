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
        public static float fadeInSpeed = 0.003f;
        public static float fadeOutSpeed = 0.01f;
        public static float maxVolume = 1.0f;

        public static void LoadMusic(Music Music)
        {

            if (fadeState == FadeState.FadeOut)
            {
                Assets.musicIns.Volume = 0.0f;
                fadeState = FadeState.Silent;
            }
            if (fadeState == FadeState.Silent)
            {
                /*
                ////// CRASHES PROGRAM randomly
                // i suspect that disposing of the sound file is causing an issue in the UWP or Monogame framework
                // this issue can't be reliably reproduced, and happens randomly
                // sometimes it happens during the course of the game, sometimes it happens when user closes the program
                // the debugger can't catch the exception, because it's a win32 unhandled exception without an error code
                // or rather the error code i'm getting is random each time, so it has no searchable value

                //stop and dispose of the music track + instance
                if (Assets.musicIns != null)
                {
                    Assets.musicIns.Stop();
                    Assets.musicIns.Dispose();
                    Assets.musicIns = null;
                }
                if (Assets.musicTrack != null)
                {
                    Assets.musicTrack.Dispose();
                    Assets.musicTrack = null;
                }
                //load the new music track, based on passed enum
                if (Music == Music.DungeonA) { Assets.musicTrack = Assets.content.Load<SoundEffect>(@"MusicDungeonA"); }
                //additional codepaths...
                */



                //reload the music track, recreate the instance
                if (Assets.musicTrack == null)
                { Assets.musicTrack = Assets.content.Load<SoundEffect>(@"MusicDungeonA"); }
                if (Assets.musicIns == null)
                { Assets.musicIns = Assets.musicTrack.CreateInstance(); }

                //if we have a music instance, loop play it, fade it in
                if (Assets.musicIns != null)
                {
                    Assets.musicIns.IsLooped = true;
                    Assets.musicIns.Volume = 0.0f;
                    Assets.musicIns.Play();
                    fadeState = FadeState.FadeIn;
                }
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
                if (Assets.musicIns.Volume + fadeInSpeed >= maxVolume)
                { Assets.musicIns.Volume = maxVolume; }
                else { Assets.musicIns.Volume += fadeInSpeed; }
                //check to see if music has reached maxVolume
                if (Assets.musicIns.Volume == maxVolume)
                { fadeState = FadeState.FadeComplete; }
            }
            else if (fadeState == FadeState.FadeComplete) { }
            else if (fadeState == FadeState.FadeOut)
            {
                //music volume CANNOT be negative, else program error
                if (Assets.musicIns.Volume - fadeOutSpeed <= 0.0f)
                { Assets.musicIns.Volume = 0.0f; }
                else { Assets.musicIns.Volume -= fadeOutSpeed; }
                //check to see if music has reached maxVolume
                if (Assets.musicIns.Volume == 0.0f)
                { fadeState = FadeState.Silent; }
            }
        }

    }
}