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
        //all the fonts, sprite sheets, music, and soundfx the program loads

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

        //world/ui/screen sheets
        public static Texture2D mainSheet;
        public static Texture2D bigTextSheet;
        public static Texture2D overworldSheet;
        public static Texture2D titleBkgSheet;

        //dungeon sheets
        public static Texture2D cursedCastleSheet;
        public static Texture2D shopSheet;

        //roomData lists - handmade room data
        public static List<RoomXmlData> roomDataHub = new List<RoomXmlData>();
        public static List<RoomXmlData> roomDataBoss = new List<RoomXmlData>();
        public static List<RoomXmlData> roomDataKey = new List<RoomXmlData>();
        public static List<RoomXmlData> roomDataColumn = new List<RoomXmlData>();
        public static List<RoomXmlData> roomDataRow = new List<RoomXmlData>();
        public static List<RoomXmlData> roomDataSquare = new List<RoomXmlData>();


        #region Music Instances

        static SoundEffect musicDungeonASrc;
        public static SoundEffectInstance musicDungeonA;

        static SoundEffect musicDungeonBSrc;
        public static SoundEffectInstance musicDungeonB;

        static SoundEffect musicDungeonCSrc;
        public static SoundEffectInstance musicDungeonC;



        static SoundEffect musicTitleSrc;
        public static SoundEffectInstance musicTitle;

        static SoundEffect musicBossSrc;
        public static SoundEffectInstance musicBoss;

        static SoundEffect musicDrumsSrc;
        public static SoundEffectInstance musicDrums;


        #endregion


        #region Soundfx Instance Lists

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
        public static List<SoundEffectInstance> sfxInventoryClose;
        //
        public static List<SoundEffectInstance> sfxBossHit;
        public static List<SoundEffectInstance> sfxExplosion;
        public static List<SoundEffectInstance> sfxFireballCast;
        public static List<SoundEffectInstance> sfxFireballDeath;
        public static List<SoundEffectInstance> sfxMetallicTap;
        //
        public static List<SoundEffectInstance> sfxBounce;
        public static List<SoundEffectInstance> sfxMapOpen;
        public static List<SoundEffectInstance> sfxError;
        public static List<SoundEffectInstance> sfxBombDrop;
        public static List<SoundEffectInstance> sfxArrowShoot;
        //
        public static List<SoundEffectInstance> sfxArrowHit;

        #endregion


        public static void Play(List<SoundEffectInstance> List)
        {
            if(Flags.PlaySoundFX)
            {
                for (i = 0; i < listSize; i++)
                {   //find a sfx instance not playing, play it, exit loop
                    if (List[i].State == SoundState.Stopped)
                    { List[i].Play(); i = listSize; }
                    //else { List[i].Stop(); } //causes clipping
                }
            }
        }

        public static void Load(GraphicsDevice GraphicsDevice, ContentManager ContentManager)
        {
            content = ContentManager;
            colorScheme = new ColorScheme();
            dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });

            //fonts
            font = content.Load<SpriteFont>(@"pixelFont");
            medFont = content.Load<SpriteFont>(@"mediumFont");

            //actor textures
            heroSheet = content.Load<Texture2D>(@"HeroSheet");
            blobSheet = content.Load<Texture2D>(@"BlobSheet");
            bossSheet = content.Load<Texture2D>(@"BossSheet");

            //world/ui/screen sheets
            mainSheet = content.Load<Texture2D>(@"MainSheet");
            bigTextSheet = content.Load<Texture2D>(@"BigTextSheet");
            overworldSheet = content.Load<Texture2D>(@"OverworldTexture");
            titleBkgSheet = content.Load<Texture2D>(@"TitleBkg");

            //dungeon textures
            cursedCastleSheet = content.Load<Texture2D>(@"DungeonSheet");
            shopSheet = content.Load<Texture2D>(@"ShopSheet");

            //load all roomData
            Functions_Backend.LoadAllRoomData();


            #region Create Music instances

            musicDungeonASrc = content.Load<SoundEffect>(@"MusicDungeonA");
            musicDungeonA = musicDungeonASrc.CreateInstance();
            musicDungeonA.IsLooped = true;

            musicDungeonBSrc = content.Load<SoundEffect>(@"MusicDungeonB");
            musicDungeonB = musicDungeonBSrc.CreateInstance();
            musicDungeonB.IsLooped = true;

            musicDungeonCSrc = content.Load<SoundEffect>(@"MusicDungeonC");
            musicDungeonC = musicDungeonCSrc.CreateInstance();
            musicDungeonC.IsLooped = true;



            musicTitleSrc = content.Load<SoundEffect>(@"MusicTitle");
            musicTitle = musicTitleSrc.CreateInstance();
            musicTitle.IsLooped = true;

            musicBossSrc = content.Load<SoundEffect>(@"MusicBoss");
            musicBoss = musicBossSrc.CreateInstance();
            musicBoss.IsLooped = true;

            musicDrumsSrc = content.Load<SoundEffect>(@"MusicLowHealthDrums");
            musicDrums = musicDrumsSrc.CreateInstance();
            musicDrums.IsLooped = true;
            musicDrums.Volume = 0.0f;

            #endregion


            #region Create the soundfx instance lists + sources

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
            sfxInventoryClose = new List<SoundEffectInstance>();
            SoundEffect inventoryCloseSrc = content.Load<SoundEffect>(@"SoundInventoryClose");
            //
            sfxBossHit = new List<SoundEffectInstance>();
            SoundEffect bossHitSrc = content.Load<SoundEffect>(@"SoundBossHit");
            sfxExplosion = new List<SoundEffectInstance>();
            SoundEffect sfxExplosionSrc = content.Load<SoundEffect>(@"SoundExplosion");
            sfxFireballCast = new List<SoundEffectInstance>();
            SoundEffect fireballCastSrc = content.Load<SoundEffect>(@"SoundFireballCast");
            sfxFireballDeath = new List<SoundEffectInstance>();
            SoundEffect fireballDeathSrc = content.Load<SoundEffect>(@"SoundFireballDeath");
            sfxMetallicTap = new List<SoundEffectInstance>();
            SoundEffect metallicTapSrc = content.Load<SoundEffect>(@"SoundMetallicTap");
            //
            sfxBounce = new List<SoundEffectInstance>();
            SoundEffect bounceSrc = content.Load<SoundEffect>(@"SoundBounce");
            sfxMapOpen = new List<SoundEffectInstance>();
            SoundEffect mapOpenSrc = content.Load<SoundEffect>(@"SoundMapOpen");
            sfxError = new List<SoundEffectInstance>();
            SoundEffect errorSrc = content.Load<SoundEffect>(@"SoundError");
            sfxBombDrop = new List<SoundEffectInstance>();
            SoundEffect bombDropSrc = content.Load<SoundEffect>(@"SoundBombDrop");
            sfxArrowShoot = new List<SoundEffectInstance>();
            SoundEffect arrowShootSrc = content.Load<SoundEffect>(@"SoundArrowShoot");
            //
            sfxArrowHit = new List<SoundEffectInstance>();
            SoundEffect arrowHitSrc = content.Load<SoundEffect>(@"SoundArrowHit");

            #endregion


            #region Populate the instance lists

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
                sfxInventoryClose.Add(inventoryCloseSrc.CreateInstance());
                //
                sfxBossHit.Add(bossHitSrc.CreateInstance());
                sfxExplosion.Add(sfxExplosionSrc.CreateInstance());
                sfxFireballCast.Add(fireballCastSrc.CreateInstance());
                sfxFireballDeath.Add(fireballDeathSrc.CreateInstance());
                sfxMetallicTap.Add(metallicTapSrc.CreateInstance());
                //
                sfxBounce.Add(bounceSrc.CreateInstance());
                sfxMapOpen.Add(mapOpenSrc.CreateInstance());
                sfxError.Add(errorSrc.CreateInstance());
                sfxBombDrop.Add(bombDropSrc.CreateInstance());
                sfxArrowShoot.Add(arrowShootSrc.CreateInstance());
                //
                sfxArrowHit.Add(arrowHitSrc.CreateInstance());
            }

            #endregion

        }

    }
}