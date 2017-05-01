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
    public static class Assets
    {
        public static ContentManager content;
        public static Texture2D dummyTexture;
        static int listSize = 5;
        static int i;

        //fonts
        public static SpriteFont font;
        public static SpriteFont medFont;

        //the color scheme for the game
        public static ColorScheme colorScheme;

        //actor sheets
        public static Texture2D heroSheet;
        public static Texture2D blobSheet;
        public static Texture2D bossSheet;

        //world/ui sheets
        public static Texture2D mainSheet;
        public static Texture2D bigTextSheet;
        public static Texture2D dungeonSheet;


        #region Music

        static SoundEffect musicDungeonASrc;
        public static SoundEffectInstance musicDungeonA;

        static SoundEffect musicDrumsSrc;
        public static SoundEffectInstance musicDrums;

        static SoundEffect musicOverworldSrc;
        public static SoundEffectInstance musicOverworld;

        //static SoundEffect musicShopSrc;
        //public static SoundEffectInstance musicShop;

        #endregion


        #region Soundfx

        //monogame defaults to 1024 max sound effect instances in sound effect instance pool
        //the number of lists * listSize SHOULD be less than 1024

        public static List<SoundEffectInstance> sfxDash;

        static SoundEffect sfxSwordSwipeSrc;
        public static SoundEffectInstance sfxSwordSwipe;

        static SoundEffect sfxEnemyHitSrc;
        public static SoundEffectInstance sfxEnemyHit;

        static SoundEffect sfxHeroHitSrc;
        public static SoundEffectInstance sfxHeroHit;

        static SoundEffect sfxEnemyKillSrc;
        public static SoundEffectInstance sfxEnemyKill;

        static SoundEffect sfxHeroKillSrc;
        public static SoundEffectInstance sfxHeroKill;

        static SoundEffect sfxBeatDungeonSrc;
        public static SoundEffectInstance sfxBeatDungeon;

        static SoundEffect sfxDoorOpenSrc;
        public static SoundEffectInstance sfxDoorOpen;

        static SoundEffect sfxKeyPickupSrc;
        public static SoundEffectInstance sfxKeyPickup;

        static SoundEffect sfxBossIntroSrc;
        public static SoundEffectInstance sfxBossIntro;

        static SoundEffect sfxChestOpenSrc;
        public static SoundEffectInstance sfxChestOpen;

        static SoundEffect sfxRewardSrc;
        public static SoundEffectInstance sfxReward;

        static SoundEffect sfxTextLetterSrc;
        public static SoundEffectInstance sfxTextLetter;

        static SoundEffect sfxTextDoneSrc;
        public static SoundEffectInstance sfxTextDone;

        static SoundEffect sfxExitSummarySrc;
        public static SoundEffectInstance sfxExitSummary;

        static SoundEffect sfxHeartPickupSrc;
        public static SoundEffectInstance sfxHeartPickup;

        static SoundEffect sfxGoldPickupSrc;
        public static SoundEffectInstance sfxGoldPickup;

        static SoundEffect sfxSelectMenuItemSrc;
        public static SoundEffectInstance sfxSelectMenuItem;

        static SoundEffect sfxInventoryOpenSrc;
        public static SoundEffectInstance sfxInventoryOpen;

        static SoundEffect sfxInventoryCloseSrc;
        public static SoundEffectInstance sfxInventoryClose;

        static SoundEffect sfxBossHitSrc;
        public static SoundEffectInstance sfxBossHit;

        public static List<SoundEffectInstance> sfxExplosion;

        static SoundEffect sfxFireballCastSrc;
        public static SoundEffectInstance sfxFireballCast;

        static SoundEffect sfxFireballDeathSrc;
        public static SoundEffectInstance sfxFireballDeath;

        static SoundEffect sfxMetallicTapSrc;
        public static SoundEffectInstance sfxMetallicTap;

        static SoundEffect sfxBounceSrc;
        public static SoundEffectInstance sfxBounce;

        #endregion



        public static void Play(List<SoundEffectInstance> List)
        {
            for (i = 0; i < listSize; i++)
            {   //find a sfx instance not playing, play it, exit loop
                if (List[i].State == SoundState.Stopped)
                { List[i].Play(); i = listSize; }
                else { List[i].Stop(); }
            }
        }



        public static void Load(GraphicsDevice GraphicsDevice, ContentManager ContentManager)
        {
            content = ContentManager;
            colorScheme = new ColorScheme("default");
            dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });

            //fonts
            font = content.Load<SpriteFont>(@"pixelFont");
            medFont = content.Load<SpriteFont>(@"mediumFont");

            //actor textures
            heroSheet = content.Load<Texture2D>(@"HeroSheet");
            blobSheet = content.Load<Texture2D>(@"BlobSheet");
            bossSheet = content.Load<Texture2D>(@"BossSheet");

            //game textures
            mainSheet = content.Load<Texture2D>(@"MainSheet");
            bigTextSheet = content.Load<Texture2D>(@"BigTextSheet");
            dungeonSheet = content.Load<Texture2D>(@"DungeonSheet");


            #region Music

            musicDungeonASrc = content.Load<SoundEffect>(@"MusicDungeonA");
            musicDungeonA = musicDungeonASrc.CreateInstance();
            musicDungeonA.IsLooped = true;

            musicDrumsSrc = content.Load<SoundEffect>(@"MusicLowHealthDrums");
            musicDrums = musicDrumsSrc.CreateInstance();
            musicDrums.IsLooped = true;

            musicOverworldSrc = content.Load<SoundEffect>(@"MusicOverworld");
            musicOverworld = musicOverworldSrc.CreateInstance();
            musicOverworld.IsLooped = true;

            //musicShopSrc = content.Load<SoundEffect>(@"MusicShop");
            //musicShop = musicShopSrc.CreateInstance();
            //musicShop.IsLooped = true;

            #endregion


            #region Soundfx

            sfxDash = new List<SoundEffectInstance>();
            SoundEffect dashSrc = content.Load<SoundEffect>(@"SoundDash");

            sfxSwordSwipeSrc = content.Load<SoundEffect>(@"SoundSwordSwipe");
            sfxSwordSwipe = sfxSwordSwipeSrc.CreateInstance();

            sfxEnemyHitSrc = content.Load<SoundEffect>(@"SoundEnemyHit");
            sfxEnemyHit = sfxEnemyHitSrc.CreateInstance();

            sfxHeroHitSrc = content.Load<SoundEffect>(@"SoundHeroHit");
            sfxHeroHit = sfxHeroHitSrc.CreateInstance();

            sfxEnemyKillSrc = content.Load<SoundEffect>(@"SoundEnemyKill");
            sfxEnemyKill = sfxEnemyKillSrc.CreateInstance();

            sfxHeroKillSrc = content.Load<SoundEffect>(@"SoundHeroKill");
            sfxHeroKill = sfxHeroKillSrc.CreateInstance();

            sfxBeatDungeonSrc = content.Load<SoundEffect>(@"SoundBeatDungeon");
            sfxBeatDungeon = sfxBeatDungeonSrc.CreateInstance();

            sfxDoorOpenSrc = content.Load<SoundEffect>(@"SoundDoorOpen");
            sfxDoorOpen = sfxDoorOpenSrc.CreateInstance();

            sfxKeyPickupSrc = content.Load<SoundEffect>(@"SoundKeyPickup");
            sfxKeyPickup = sfxKeyPickupSrc.CreateInstance();

            sfxBossIntroSrc = content.Load<SoundEffect>(@"SoundBossIntro");
            sfxBossIntro = sfxBossIntroSrc.CreateInstance();

            sfxChestOpenSrc = content.Load<SoundEffect>(@"SoundChestOpen");
            sfxChestOpen = sfxChestOpenSrc.CreateInstance();

            sfxRewardSrc = content.Load<SoundEffect>(@"SoundReward");
            sfxReward = sfxRewardSrc.CreateInstance();

            sfxTextLetterSrc = content.Load<SoundEffect>(@"SoundTextLetter");
            sfxTextLetter = sfxTextLetterSrc.CreateInstance();

            sfxTextDoneSrc = content.Load<SoundEffect>(@"SoundTextDone");
            sfxTextDone = sfxTextDoneSrc.CreateInstance();

            sfxExitSummarySrc = content.Load<SoundEffect>(@"SoundExitSummary");
            sfxExitSummary = sfxExitSummarySrc.CreateInstance();

            sfxHeartPickupSrc = content.Load<SoundEffect>(@"SoundHeartPickup");
            sfxHeartPickup = sfxHeartPickupSrc.CreateInstance();

            sfxGoldPickupSrc = content.Load<SoundEffect>(@"SoundGoldPickup");
            sfxGoldPickup = sfxGoldPickupSrc.CreateInstance();

            sfxSelectMenuItemSrc = content.Load<SoundEffect>(@"SoundSelectMenuItem");
            sfxSelectMenuItem = sfxSelectMenuItemSrc.CreateInstance();

            sfxInventoryOpenSrc = content.Load<SoundEffect>(@"SoundInventoryOpen");
            sfxInventoryOpen = sfxInventoryOpenSrc.CreateInstance();

            sfxInventoryCloseSrc = content.Load<SoundEffect>(@"SoundInventoryClose");
            sfxInventoryClose = sfxInventoryCloseSrc.CreateInstance();

            sfxBossHitSrc = content.Load<SoundEffect>(@"SoundBossHit");
            sfxBossHit = sfxBossHitSrc.CreateInstance();

            sfxExplosion = new List<SoundEffectInstance>();
            SoundEffect sfxExplosionSrc = content.Load<SoundEffect>(@"SoundExplosion");

            sfxFireballCastSrc = content.Load<SoundEffect>(@"SoundFireballCast");
            sfxFireballCast = sfxFireballCastSrc.CreateInstance();

            sfxFireballDeathSrc = content.Load<SoundEffect>(@"SoundFireballDeath");
            sfxFireballDeath = sfxFireballDeathSrc.CreateInstance();

            sfxMetallicTapSrc = content.Load<SoundEffect>(@"SoundMetallicTap");
            sfxMetallicTap = sfxMetallicTapSrc.CreateInstance();

            sfxBounceSrc = content.Load<SoundEffect>(@"SoundBounce");
            sfxBounce = sfxBounceSrc.CreateInstance();

            #endregion


            //populate the instance lists
            for (i = 0; i < listSize; i++)
            {
                sfxDash.Add(dashSrc.CreateInstance());

                sfxExplosion.Add(sfxExplosionSrc.CreateInstance());
            }
        }

    }
}