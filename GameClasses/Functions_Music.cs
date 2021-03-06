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
    public static class Functions_Music
    {

        public static FadeState fadeState = FadeState.Silent;
        public static float fadeInSpeed = 0.04f;
        public static float fadeOutSpeed = 0.04f;
        public static float maxVolume = 1.0f;

        //store reference to current music playing
        public static SoundEffectInstance currentMusic = Assets.musicDungeonA; 
        public static Music trackToLoad = Music.None;


        public static void PlayMusic(Music Music)
        {
            trackToLoad = Music; //get the track to play
            //begin fading out the current music, upon silence music will change
            if (fadeState != FadeState.FadeOut) { fadeState = FadeState.FadeOut; }
        }

        public static void Update()
        {
            if (Flags.PlayMusic)
            {

                #region Handle FadeIn FadeOut States

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
                else if (fadeState == FadeState.FadeComplete) { }
                else if (fadeState == FadeState.FadeOut)
                {
                    //music volume CANNOT be negative, else program error
                    if (currentMusic.Volume - fadeOutSpeed <= 0.0f)
                    { currentMusic.Volume = 0.0f;}
                    else { currentMusic.Volume -= fadeOutSpeed; }
                    //check to see if music has reached 0
                    if (currentMusic.Volume == 0.0f)
                    { fadeState = FadeState.Silent; }
                }

                #endregion


                #region Handle Silent (switch music tracks) State

                else if (fadeState == FadeState.Silent)
                {
                    //if there is no track to load, do nothing and wait
                    if (trackToLoad == Music.None) { }
                    else
                    {   
                        //determine the track to load based on the enum
                        if (trackToLoad == Music.DungeonA)
                        { currentMusic = Assets.musicDungeonA; Assets.musicDrums.Play(); }
                        else if (trackToLoad == Music.DungeonB)
                        { currentMusic = Assets.musicDungeonB; Assets.musicDrums.Play(); }
                        else if (trackToLoad == Music.DungeonC)
                        { currentMusic = Assets.musicDungeonC; Assets.musicDrums.Play(); }
                        //
                        else if (trackToLoad == Music.LightWorld)
                        { currentMusic = Assets.musicLightWorld; Assets.musicDrums.Play(); }
                        else if (trackToLoad == Music.CrowdFighting)
                        { currentMusic = Assets.musicCrowdFighting; Assets.musicDrums.Play(); }
                        else if (trackToLoad == Music.CrowdWaiting)
                        { currentMusic = Assets.musicCrowdWaiting; Assets.musicDrums.Play(); }
                        //title should never play drums
                        else if (trackToLoad == Music.Title)
                        { currentMusic = Assets.musicTitle; Assets.musicDrums.Stop(); }
                        else if (trackToLoad == Music.Boss){ currentMusic = Assets.musicBoss; } 




                        //stop the other music tracks from playing
                        if (currentMusic != Assets.musicDungeonA) { Assets.musicDungeonA.Stop(); }
                        if (currentMusic != Assets.musicDungeonB) { Assets.musicDungeonB.Stop(); }
                        if (currentMusic != Assets.musicDungeonC) { Assets.musicDungeonC.Stop(); }
                        //
                        if (currentMusic != Assets.musicLightWorld) { Assets.musicLightWorld.Stop(); }
                        if (currentMusic != Assets.musicCrowdFighting) { Assets.musicCrowdFighting.Stop(); }
                        if (currentMusic != Assets.musicCrowdWaiting) { Assets.musicCrowdWaiting.Stop(); }
                        //
                        if (currentMusic != Assets.musicTitle) { Assets.musicTitle.Stop(); }
                        if (currentMusic != Assets.musicBoss) { Assets.musicBoss.Stop(); }




                        //play the music
                        currentMusic.Play();
                        currentMusic.Volume = 0.0f;
                        currentMusic.IsLooped = true;
                        //fade music + drums back in
                        fadeState = FadeState.FadeIn;
                    }
                }

                #endregion


                #region Handle Drum Track Volume - based on hero's health

                if (Pool.hero.health < 3)
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

                #endregion

            }
            else
            {   //fade music out
                if (fadeState == FadeState.Silent) { }
                else
                {   //music volume CANNOT be negative, else program error
                    if (currentMusic.Volume - fadeOutSpeed <= 0.0f)
                    { currentMusic.Volume = 0.0f; }
                    else { currentMusic.Volume -= fadeOutSpeed; }
                    //check to see if music has reached 0
                    if (currentMusic.Volume == 0.0f)
                    { fadeState = FadeState.Silent; }
                }
            }
        }

    }
}