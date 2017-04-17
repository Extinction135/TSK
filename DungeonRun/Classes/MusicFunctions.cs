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
            {   //check to see if we should fade in the drum track
                if(Pool.hero.health < 3)
                {
                    if (Assets.musicDrums.Volume + fadeInSpeed >= maxVolume)
                    { Assets.musicDrums.Volume = maxVolume; }
                    else { Assets.musicDrums.Volume += fadeInSpeed; }
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
                {
                    Debug.WriteLine("track to load: " + trackToLoad);

                    if (trackToLoad == Music.DungeonA) { currentMusic = Assets.musicDungeonA; }
                    //additional track ref setting here

                    //sync the music track with the drum track
                    currentMusic.Stop(); Assets.musicDrums.Stop();
                    currentMusic.Play(); Assets.musicDrums.Play();
                    //prep for music + drums to fade back in
                    currentMusic.Volume = 0.0f;
                    Assets.musicDrums.Volume = 0.0f;
                    //loop the music track and the drum track
                    currentMusic.IsLooped = true;
                    Assets.musicDrums.IsLooped = true;
                    //fade music + drums back in
                    fadeState = FadeState.FadeIn;
                    //drums will only fade back in if hero's health is low
                }
            }
        }

    }
}