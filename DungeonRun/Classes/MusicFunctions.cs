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

        public static FadeState fadeState = FadeState.Silent;
        public static float fadeInSpeed = 0.01f;
        public static float fadeOutSpeed = 0.01f;
        public static float maxVolume = 1.0f;

        //store reference to current music playing
        public static SoundEffectInstance currentMusic = Assets.musicDungeonA; 
        public static Music trackToLoad = Music.None;

        public static void Update()
        {

            #region Fade in/out, play

            if (fadeState == FadeState.FadeIn)
            {
                //music volume CANNOT exceed 1.0f, else program error
                if (currentMusic.Volume + fadeInSpeed >= maxVolume)
                { currentMusic.Volume = maxVolume; }
                else { currentMusic.Volume += fadeInSpeed; }
                //check to see if music has reached maxVolume
                if (currentMusic.Volume == maxVolume)
                { fadeState = FadeState.FadeComplete; }
            }
            else if (fadeState == FadeState.FadeComplete)
            {   //check the hero's health to see if we should fade drum track
                if(Pool.hero.health < 3)
                {   //fade the drum track in
                    if (Assets.musicDrums.Volume + fadeInSpeed >= maxVolume)
                    { Assets.musicDrums.Volume = maxVolume; }
                    else { Assets.musicDrums.Volume += fadeInSpeed; }
                }
                else
                {   //fade the drum track out
                    if (Assets.musicDrums.Volume - fadeOutSpeed <= 0.0f)
                    { Assets.musicDrums.Volume = 0.0f; }
                    else { Assets.musicDrums.Volume -= fadeOutSpeed; }
                }
            }
            else if (fadeState == FadeState.FadeOut)
            {
                //music volume CANNOT be negative, else program error
                if (currentMusic.Volume - fadeOutSpeed <= 0.0f)
                { currentMusic.Volume = 0.0f;}
                else { currentMusic.Volume -= fadeOutSpeed; }
                //match drum volume to current music volume
                Assets.musicDrums.Volume = currentMusic.Volume;
                //check to see if music has reached 0
                if (currentMusic.Volume == 0.0f)
                { fadeState = FadeState.Silent; }
            }

            #endregion


            else if (fadeState == FadeState.Silent)
            {
                //if there is no track to load, do nothing and wait
                if (trackToLoad == Music.None) { }
                else
                {   //determine the track to load based on the enum
                    if (trackToLoad == Music.DungeonA) { currentMusic = Assets.musicDungeonA; }
                    else if (trackToLoad == Music.Overworld) { currentMusic = Assets.musicOverworld; }
                    //else if (trackToLoad == Music.Shop) { currentMusic = Assets.musicShop; }

                    //stop the other music tracks from playing, to reduce CPU load
                    if (currentMusic != Assets.musicDungeonA) { Assets.musicDungeonA.Stop(); }
                    else if (currentMusic != Assets.musicOverworld) { Assets.musicOverworld.Stop(); }
                    //else if (currentMusic != Assets.musicShop) { Assets.musicShop.Stop(); }

                    //prep for music + drums to fade back in
                    currentMusic.Volume = 0.0f;
                    Assets.musicDrums.Volume = 0.0f;
                    //loop the music track and the drum track
                    currentMusic.IsLooped = true;
                    Assets.musicDrums.IsLooped = true;
                    //start playing the music and drum tracks
                    currentMusic.Play(); Assets.musicDrums.Play();
                    //fade music + drums back in
                    fadeState = FadeState.FadeIn;
                }
            }
        }

    }
}