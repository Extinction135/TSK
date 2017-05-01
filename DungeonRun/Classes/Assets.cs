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
        public static List<SoundEffectInstance> sfxSwordSwipe;
        public static List<SoundEffectInstance> sfxEnemyHit;
        public static List<SoundEffectInstance> sfxHeroHit;
        public static List<SoundEffectInstance> sfxEnemyKill;
        //
        public static List<SoundEffectInstance> sfxHeroKill;
        public static List<SoundEffectInstance> sfxBeatDungeon;
        public static List<SoundEffectInstance> sfxDoorOpen;
        public static List<SoundEffectInstance> sfxKeyPickup;
        public static List<SoundEffectInstance> sfxBossIntro;
        //
        public static List<SoundEffectInstance> sfxChestOpen;
        public static List<SoundEffectInstance> sfxReward;
        public static List<SoundEffectInstance> sfxTextLetter;
        public static List<SoundEffectInstance> sfxTextDone;
        public static List<SoundEffectInstance> sfxExitSummary;
        //
        public static List<SoundEffectInstance> sfxHeartPickup;
        public static List<SoundEffectInstance> sfxGoldPickup;
        public static List<SoundEffectInstance> sfxMenuItem;
        public static List<SoundEffectInstance> sfxInventoryOpen;

        

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
            sfxSwordSwipe = new List<SoundEffectInstance>();
            SoundEffect swordSwipeSrc = content.Load<SoundEffect>(@"SoundSwordSwipe");
            sfxEnemyHit = new List<SoundEffectInstance>();
            SoundEffect enemyHitSrc = content.Load<SoundEffect>(@"SoundEnemyHit");
            sfxHeroHit = new List<SoundEffectInstance>();
            SoundEffect heroHitSrc = content.Load<SoundEffect>(@"SoundHeroHit");
            sfxEnemyKill = new List<SoundEffectInstance>();
            SoundEffect enemyKillSrc = content.Load<SoundEffect>(@"SoundEnemyKill");

            //
            sfxHeroKill = new List<SoundEffectInstance>();
            SoundEffect heroKillSrc = content.Load<SoundEffect>(@"SoundHeroKill");
            sfxBeatDungeon = new List<SoundEffectInstance>();
            SoundEffect beatDuneonSrc = content.Load<SoundEffect>(@"SoundBeatDungeon");
            sfxDoorOpen = new List<SoundEffectInstance>();
            SoundEffect doorOpenSrc = content.Load<SoundEffect>(@"SoundDoorOpen");
            sfxKeyPickup = new List<SoundEffectInstance>();
            SoundEffect keyPickupSrc = content.Load<SoundEffect>(@"SoundKeyPickup");
            sfxBossIntro = new List<SoundEffectInstance>();
            SoundEffect bossIntroSrc = content.Load<SoundEffect>(@"SoundBossIntro");
            //
            sfxChestOpen = new List<SoundEffectInstance>();
            SoundEffect chestOpenSrc = content.Load<SoundEffect>(@"SoundChestOpen");
            sfxReward = new List<SoundEffectInstance>();
            SoundEffect rewardSrc = content.Load<SoundEffect>(@"SoundReward");
            sfxTextLetter = new List<SoundEffectInstance>();
            SoundEffect textLetterSrc = content.Load<SoundEffect>(@"SoundTextLetter");
            sfxTextDone = new List<SoundEffectInstance>();
            SoundEffect textDoneSrc = content.Load<SoundEffect>(@"SoundTextDone");
            sfxExitSummary = new List<SoundEffectInstance>();
            SoundEffect exitSummarySrc = content.Load<SoundEffect>(@"SoundExitSummary");
            //
            sfxHeartPickup = new List<SoundEffectInstance>();
            SoundEffect heartPickupSrc = content.Load<SoundEffect>(@"SoundHeartPickup");
            sfxGoldPickup = new List<SoundEffectInstance>();
            SoundEffect goldPickupSrc = content.Load<SoundEffect>(@"SoundGoldPickup");
            sfxMenuItem = new List<SoundEffectInstance>();
            SoundEffect menuItemSrc = content.Load<SoundEffect>(@"SoundSelectMenuItem");
            sfxInventoryOpen = new List<SoundEffectInstance>();
            SoundEffect inventoryOpenSrc = content.Load<SoundEffect>(@"SoundInventoryOpen");
            

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
                sfxSwordSwipe.Add(swordSwipeSrc.CreateInstance());
                sfxEnemyHit.Add(enemyHitSrc.CreateInstance());
                sfxHeroHit.Add(heroHitSrc.CreateInstance());
                sfxEnemyKill.Add(enemyKillSrc.CreateInstance());
                //
                sfxHeroKill.Add(heroKillSrc.CreateInstance());
                sfxBeatDungeon.Add(beatDuneonSrc.CreateInstance());
                sfxDoorOpen.Add(doorOpenSrc.CreateInstance());
                sfxKeyPickup.Add(keyPickupSrc.CreateInstance());
                sfxBossIntro.Add(bossIntroSrc.CreateInstance());
                //
                sfxChestOpen.Add(chestOpenSrc.CreateInstance());
                sfxReward.Add(rewardSrc.CreateInstance());
                sfxTextLetter.Add(textLetterSrc.CreateInstance());
                sfxTextDone.Add(textDoneSrc.CreateInstance());
                sfxExitSummary.Add(exitSummarySrc.CreateInstance());
                //
                sfxHeartPickup.Add(heartPickupSrc.CreateInstance());
                sfxGoldPickup.Add(goldPickupSrc.CreateInstance());
                sfxMenuItem.Add(menuItemSrc.CreateInstance());
                sfxInventoryOpen.Add(inventoryOpenSrc.CreateInstance());

                //
                sfxExplosion.Add(sfxExplosionSrc.CreateInstance());
            }
        }

    }
}