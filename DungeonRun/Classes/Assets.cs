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
        public static Texture2D dungeonSheet;
        public static Texture2D particleSheet;
        public static Texture2D uiSheet;
        public static Texture2D bigTextSheet;


        #region Music

        static SoundEffect musicDungeonASrc;
        public static SoundEffectInstance musicDungeonA;

        static SoundEffect musicDrumsSrc;
        public static SoundEffectInstance musicDrums;

        static SoundEffect musicOverworldSrc;
        public static SoundEffectInstance musicOverworld;

        static SoundEffect musicShopSrc;
        public static SoundEffectInstance musicShop;

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
            dungeonSheet = content.Load<Texture2D>(@"DungeonSheet");
            particleSheet = content.Load<Texture2D>(@"ParticlesProjectilesSheet");
            uiSheet = content.Load<Texture2D>(@"UISheet");
            bigTextSheet = content.Load<Texture2D>(@"BigTextSheet");


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

            musicShopSrc = content.Load<SoundEffect>(@"MusicShop");
            musicShop = musicShopSrc.CreateInstance();
            musicShop.IsLooped = true;

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

            #endregion


        }

        public static void SetDefaultColorScheme()
        {
            //setup default color scheme
            colorScheme = new ColorScheme();

            colorScheme.background = new Color(100, 100, 100, 255);
            colorScheme.overlay = new Color(0, 0, 0, 255);

            colorScheme.textSmall = new Color(255, 255, 255, 255);
            colorScheme.windowBkg = new Color(0, 0, 0, 200);
            colorScheme.collision = new Color(100, 0, 0, 0);

            colorScheme.buttonUp = new Color(44, 44, 44, 0);
            colorScheme.buttonOver = new Color(66, 66, 66, 0);
            colorScheme.buttonDown = new Color(100, 100, 100, 0);
        }

    }

    public struct ColorScheme
    {
        public Color background;
        public Color overlay;

        public Color textSmall;
        public Color windowBkg;
        public Color collision;

        public Color buttonUp;
        public Color buttonOver;
        public Color buttonDown;
    }
}