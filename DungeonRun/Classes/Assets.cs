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
    public static class Assets
    {
        public static ContentManager content;
        public static Texture2D dummyTexture;

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

        static SoundEffect sfxDashSrc;
        public static SoundEffectInstance sfxDash;

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

        static SoundEffect sfxExplosionSrc;
        //multiple explosions can be playing simultaneously
        static List<SoundEffectInstance> explosionInstances;
        static int explosionsCount = 5;
        static int i;

        static SoundEffect sfxFireballCastSrc;
        public static SoundEffectInstance sfxFireballCast;

        static SoundEffect sfxFireballDeathSrc;
        public static SoundEffectInstance sfxFireballDeath;

        static SoundEffect sfxMetallicTapSrc;
        public static SoundEffectInstance sfxMetallicTap;

        static SoundEffect sfxBounceSrc;
        public static SoundEffectInstance sfxBounce;

        #endregion



        public static void Load(GraphicsDevice GraphicsDevice, ContentManager ContentManager)
        {
            content = ContentManager;
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

            sfxDashSrc = content.Load<SoundEffect>(@"SoundDash");
            sfxDash = sfxDashSrc.CreateInstance();

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

            sfxExplosionSrc = content.Load<SoundEffect>(@"SoundExplosion");
            //create the explosion instances list, populate it
            explosionInstances = new List<SoundEffectInstance>();
            for (i = 0; i < explosionsCount; i++)
            { explosionInstances.Add(sfxExplosionSrc.CreateInstance()); }

            sfxFireballCastSrc = content.Load<SoundEffect>(@"SoundFireballCast");
            sfxFireballCast = sfxFireballCastSrc.CreateInstance();

            sfxFireballDeathSrc = content.Load<SoundEffect>(@"SoundFireballDeath");
            sfxFireballDeath = sfxFireballDeathSrc.CreateInstance();

            sfxMetallicTapSrc = content.Load<SoundEffect>(@"SoundMetallicTap");
            sfxMetallicTap = sfxMetallicTapSrc.CreateInstance();

            sfxBounceSrc = content.Load<SoundEffect>(@"SoundBounce");
            sfxBounce = sfxBounceSrc.CreateInstance();

            #endregion


        }

        public static void SetDefaultColorScheme()
        {
            //setup default color scheme
            colorScheme = new ColorScheme();

            colorScheme.background = new Color(100, 100, 100, 255);
            colorScheme.overlay = new Color(0, 0, 0, 255);
            colorScheme.debugBkg = new Color(0, 0, 0, 200);
            colorScheme.collision = new Color(100, 0, 0, 50);

            colorScheme.buttonUp = new Color(44, 44, 44);
            colorScheme.buttonOver = new Color(66, 66, 66);
            colorScheme.buttonDown = new Color(100, 100, 100);

            colorScheme.windowBkg = new Color(0, 0, 0);
            colorScheme.windowBorder = new Color(210, 210, 210);
            colorScheme.windowInset = new Color(130, 130, 130);
            colorScheme.windowInterior = new Color(156, 156, 156);

            colorScheme.textLight = new Color(255, 255, 255);
            colorScheme.textDark = new Color(0, 0, 0);
        }

        public static void PlayExplosionSoundEffect()
        {   //find an explosion instances not playing, play it & bail
            for (i = 0; i < explosionsCount; i++)
            {
                if (explosionInstances[i].State == SoundState.Stopped)
                {
                    explosionInstances[i].Play();
                    i = explosionsCount; //end the loop
                }
                //stop previous instances from playing
                //this prevents the 'echo/overlap' effect
                else { explosionInstances[i].Stop(); }
                //but this causes a 'clipping' effect
            }
        }

    }

    public struct ColorScheme
    {
        public Color background;
        public Color overlay;
        public Color debugBkg;
        public Color collision;

        public Color buttonUp;
        public Color buttonOver;
        public Color buttonDown;

        public Color windowBkg;
        public Color windowBorder;
        public Color windowInset;
        public Color windowInterior;

        public Color textLight;
        public Color textDark;
    }
}