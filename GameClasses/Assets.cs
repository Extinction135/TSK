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
        //all the fonts, sprite sheets, music, and soundfx the program loads

        public static ContentManager content;
        public static Texture2D dummyTexture;

        //fonts
        public static SpriteFont font;
        public static SpriteFont medFont;

        //actor sheets
        public static Texture2D heroSheet;
        public static Texture2D blobSheet;
        //pets sheet
        public static Texture2D petsSheet;

        //ui/screen sheets
        public static Texture2D entitiesSheet;
        public static Texture2D uiItemsSheet;


        //line texture
        public static Texture2D lineTexture;



        //level sheets
        public static Texture2D CommonObjsSheet;
        public static Texture2D EnemySheet;

        //dungeon sheets
        public static Texture2D Dungeon_CurrentSheet; //points to one below
        public static Texture2D Dungeon_DefaultSheet; //also blob dungeon
        public static Texture2D Dungeon_ForestSheet;
        public static Texture2D Dungeon_MountainSheet;
        public static Texture2D Dungeon_SwampSheet;
        public static Texture2D Dungeon_CloudSheet;
        public static Texture2D Dungeon_LavaSheet;
        public static Texture2D Dungeon_ThievesSheet;
        public static Texture2D Dungeon_ShadowSheet;





        //bkg sprite textures
        public static Texture2D BkgSp_Shadow_Coliseum;
















        //overworld sheets
        public static Texture2D overworld_image;


        //misc
        public static Texture2D bigTextSheet;
        public static Texture2D titleBkgSheet;



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

        static SoundEffect musicLightWorldSrc;
        public static SoundEffectInstance musicLightWorld;




        //colliseum soundfx
        static SoundEffect musicCrowdWaitingSrc;
        public static SoundEffectInstance musicCrowdWaiting;

        static SoundEffect musicCrowdFightingSrc;
        public static SoundEffectInstance musicCrowdFighting;


        #endregion


        #region Soundfx Instances

        public static SoundEffectInstance sfxHeroDash;
        public static SoundEffectInstance sfxSwordSwipe;
        public static SoundEffectInstance sfxEnemyHit;
        public static SoundEffectInstance sfxHeroHit;
        public static SoundEffectInstance sfxEnemyKill;

        public static SoundEffectInstance sfxHeroKill;
        public static SoundEffectInstance sfxBeatDungeon;
        public static SoundEffectInstance sfxDoorOpen;
        public static SoundEffectInstance sfxKeyPickup;
        public static SoundEffectInstance sfxBossIntro;

        public static SoundEffectInstance sfxChestOpen;
        public static SoundEffectInstance sfxReward;
        public static SoundEffectInstance sfxTextLetter;
        public static SoundEffectInstance sfxTextDone;
        public static SoundEffectInstance sfxExitSummary;

        public static SoundEffectInstance sfxHeartPickup;
        public static SoundEffectInstance sfxGoldPickup;
        public static SoundEffectInstance sfxMenuItem;
        public static SoundEffectInstance sfxWindowOpen;
        public static SoundEffectInstance sfxWindowClose;

        public static SoundEffectInstance sfxBossHit;
        public static SoundEffectInstance sfxExplosion;
        public static SoundEffectInstance sfxFireballCast;
        public static SoundEffectInstance sfxFireballDeath;
        public static SoundEffectInstance sfxTapMetallic;

        public static SoundEffectInstance sfxBounce;
        public static SoundEffectInstance sfxMapOpen;
        public static SoundEffectInstance sfxError;
        public static SoundEffectInstance sfxBombDrop;
        public static SoundEffectInstance sfxArrowShoot;

        public static SoundEffectInstance sfxArrowHit;
        public static SoundEffectInstance sfxLightFire;
        public static SoundEffectInstance sfxSwitch;
        public static SoundEffectInstance sfxShatter;
        public static SoundEffectInstance sfxBossHitDeath;

        public static SoundEffectInstance sfxExplosionsMultiple;
        public static SoundEffectInstance sfxQuit;
        public static SoundEffectInstance sfxBlobDash;
        public static SoundEffectInstance sfxSelectFile;
        public static SoundEffectInstance sfxMapWalking;

        public static SoundEffectInstance sfxActorFall;
        public static SoundEffectInstance sfxActorLand;
        public static SoundEffectInstance sfxTapHollow;
        public static SoundEffectInstance sfxSplash;
        public static SoundEffectInstance sfxPetChicken;

        public static SoundEffectInstance sfxPetDog;
        public static SoundEffectInstance sfxNet;
        public static SoundEffectInstance sfxBoomerangFlying;
        public static SoundEffectInstance sfxBushCut;
        public static SoundEffectInstance sfxGrassWalk;

        public static SoundEffectInstance sfxEnterDungeon;
        public static SoundEffectInstance sfxMapLocation;
        public static SoundEffectInstance sfxDragObj;
        public static SoundEffectInstance sfxWaterWalk;
        public static SoundEffectInstance sfxWaterSwim;

        public static SoundEffectInstance sfxEnemyTaunt;
        public static SoundEffectInstance sfxRatSqueak;
        public static SoundEffectInstance sfxShock;
        public static SoundEffectInstance sfxCastBolt;
        public static SoundEffectInstance sfxCastBombos;

        public static SoundEffectInstance sfxGoldSpam;
        public static SoundEffectInstance sfxHammer;
        public static SoundEffectInstance sfxHammerPost;
        public static SoundEffectInstance sfxIcerod;
        public static SoundEffectInstance sfxWarp;

        #endregion



        public static void Play(SoundEffectInstance Ins)
        {
            if (Flags.PlaySoundFX)
            {
                if (Ins != null)
                {
                    Ins.IsLooped = false;
                    Ins.Volume = 1.0f;
                    Ins.Play();
                }
            }
        }

        public static void Load(GraphicsDevice GraphicsDevice, ContentManager ContentManager)
        {
            content = ContentManager;
            dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });

            //fonts
            font = content.Load<SpriteFont>(@"pixelFont");
            medFont = content.Load<SpriteFont>(@"mediumFont");
            titleBkgSheet = content.Load<Texture2D>(@"TitleBkg");

            //textures used by drawn lines
            lineTexture = content.Load<Texture2D>(@"LineTexture");



            //overworld textures
            overworld_image = content.Load<Texture2D>(@"Overworld_Image");

            //load all sprite textures
            bigTextSheet = content.Load<Texture2D>(@"Sprites_BigText");
            entitiesSheet = content.Load<Texture2D>(@"Sprites_Entities");

            blobSheet = content.Load<Texture2D>(@"Sprites_Hero_Blob");
            heroSheet = content.Load<Texture2D>(@"Sprites_Hero_Link");


            //new sheets
            CommonObjsSheet = content.Load<Texture2D>(@"Sprites_CommonObjs");
            EnemySheet = content.Load<Texture2D>(@"Sprites_Enemies");

            Dungeon_DefaultSheet = content.Load<Texture2D>(@"Sprites_Dungeon_Default");
            Dungeon_ForestSheet = content.Load<Texture2D>(@"Sprites_Dungeon_Forest");
            Dungeon_MountainSheet = content.Load<Texture2D>(@"Sprites_Dungeon_Mountain");
            Dungeon_SwampSheet = content.Load<Texture2D>(@"Sprites_Dungeon_Swamp");
            Dungeon_CloudSheet = content.Load<Texture2D>(@"Sprites_Dungeon_Cloud");
            Dungeon_LavaSheet = content.Load<Texture2D>(@"Sprites_Dungeon_Lava");
            Dungeon_ThievesSheet = content.Load<Texture2D>(@"Sprites_Dungeon_ThievesDen");
            Dungeon_ShadowSheet = content.Load<Texture2D>(@"Sprites_Dungeon_Shadow");

            //points to default sheet upon creation
            Dungeon_CurrentSheet = Dungeon_DefaultSheet;

            petsSheet = content.Load<Texture2D>(@"Sprites_Pets");
            uiItemsSheet = content.Load<Texture2D>(@"Sprites_Ui");





            //load the bkg sprite textures
            BkgSp_Shadow_Coliseum = content.Load<Texture2D>(@"BkgSprite_Shadow_Coliseum");















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

            musicLightWorldSrc = content.Load<SoundEffect>(@"MusicLightWorld");
            musicLightWorld = musicLightWorldSrc.CreateInstance();
            musicLightWorld.IsLooped = true;



            musicCrowdWaitingSrc = content.Load<SoundEffect>(@"MusicCrowdWaiting");
            musicCrowdWaiting = musicCrowdWaitingSrc.CreateInstance();
            musicCrowdWaiting.IsLooped = true;

            musicCrowdFightingSrc = content.Load<SoundEffect>(@"MusicCrowdFighting");
            musicCrowdFighting = musicCrowdFightingSrc.CreateInstance();
            musicCrowdFighting.IsLooped = true;




            #endregion


            #region Create the soundfx instance lists + sources

            SoundEffect heroDashSrc = content.Load<SoundEffect>(@"SoundHeroDash");
            sfxHeroDash = heroDashSrc.CreateInstance();
            SoundEffect swordSwipeSrc = content.Load<SoundEffect>(@"SoundSwordSwipe");
            sfxSwordSwipe = swordSwipeSrc.CreateInstance();
            SoundEffect enemyHitSrc = content.Load<SoundEffect>(@"SoundEnemyHit");
            sfxEnemyHit = enemyHitSrc.CreateInstance();
            SoundEffect heroHitSrc = content.Load<SoundEffect>(@"SoundHeroHit");
            sfxHeroHit = heroHitSrc.CreateInstance();
            SoundEffect enemyKillSrc = content.Load<SoundEffect>(@"SoundEnemyKill");
            sfxEnemyKill = enemyKillSrc.CreateInstance();
            //
            SoundEffect heroKillSrc = content.Load<SoundEffect>(@"SoundHeroKill");
            sfxHeroKill = heroKillSrc.CreateInstance();
            SoundEffect beatDuneonSrc = content.Load<SoundEffect>(@"SoundBeatDungeon");
            sfxBeatDungeon = beatDuneonSrc.CreateInstance();
            SoundEffect doorOpenSrc = content.Load<SoundEffect>(@"SoundDoorOpen");
            sfxDoorOpen = doorOpenSrc.CreateInstance();
            SoundEffect keyPickupSrc = content.Load<SoundEffect>(@"SoundKeyPickup");
            sfxKeyPickup = keyPickupSrc.CreateInstance();
            SoundEffect bossIntroSrc = content.Load<SoundEffect>(@"SoundBossIntro");
            sfxBossIntro = bossIntroSrc.CreateInstance();
            //
            SoundEffect chestOpenSrc = content.Load<SoundEffect>(@"SoundChestOpen");
            sfxChestOpen = chestOpenSrc.CreateInstance();
            SoundEffect rewardSrc = content.Load<SoundEffect>(@"SoundReward");
            sfxReward = rewardSrc.CreateInstance();
            SoundEffect textLetterSrc = content.Load<SoundEffect>(@"SoundTextLetter");
            sfxTextLetter = textLetterSrc.CreateInstance();
            SoundEffect textDoneSrc = content.Load<SoundEffect>(@"SoundTextDone");
            sfxTextDone = textDoneSrc.CreateInstance();
            SoundEffect exitSummarySrc = content.Load<SoundEffect>(@"SoundExitSummary");
            sfxExitSummary = exitSummarySrc.CreateInstance();
            //
            SoundEffect heartPickupSrc = content.Load<SoundEffect>(@"SoundHeartPickup");
            sfxHeartPickup = heartPickupSrc.CreateInstance();
            SoundEffect goldPickupSrc = content.Load<SoundEffect>(@"SoundGoldPickup");
            sfxGoldPickup = goldPickupSrc.CreateInstance();
            SoundEffect menuItemSrc = content.Load<SoundEffect>(@"SoundSelectMenuItem");
            sfxMenuItem = menuItemSrc.CreateInstance();
            SoundEffect windowOpenSrc = content.Load<SoundEffect>(@"SoundWindowOpen");
            sfxWindowOpen = windowOpenSrc.CreateInstance();
            SoundEffect windowCloseSrc = content.Load<SoundEffect>(@"SoundWindowClose");
            sfxWindowClose = windowCloseSrc.CreateInstance();
            //
            SoundEffect bossHitSrc = content.Load<SoundEffect>(@"SoundBossHit");
            sfxBossHit = bossHitSrc.CreateInstance();
            SoundEffect sfxExplosionSrc = content.Load<SoundEffect>(@"SoundExplosion");
            sfxExplosion = sfxExplosionSrc.CreateInstance();
            SoundEffect fireballCastSrc = content.Load<SoundEffect>(@"SoundFireballCast");
            sfxFireballCast = fireballCastSrc.CreateInstance();
            SoundEffect fireballDeathSrc = content.Load<SoundEffect>(@"SoundFireballDeath");
            sfxFireballDeath = fireballDeathSrc.CreateInstance();
            SoundEffect tapMetallicSrc = content.Load<SoundEffect>(@"SoundTapMetallic");
            sfxTapMetallic = tapMetallicSrc.CreateInstance();
            //
            SoundEffect bounceSrc = content.Load<SoundEffect>(@"SoundBounce");
            sfxBounce = bounceSrc.CreateInstance();
            SoundEffect mapOpenSrc = content.Load<SoundEffect>(@"SoundMapOpen");
            sfxMapOpen = mapOpenSrc.CreateInstance();
            SoundEffect errorSrc = content.Load<SoundEffect>(@"SoundError");
            sfxError = errorSrc.CreateInstance();
            SoundEffect bombDropSrc = content.Load<SoundEffect>(@"SoundBombDrop");
            sfxBombDrop = bombDropSrc.CreateInstance();
            SoundEffect arrowShootSrc = content.Load<SoundEffect>(@"SoundArrowShoot");
            sfxArrowShoot = arrowShootSrc.CreateInstance();
            //
            SoundEffect arrowHitSrc = content.Load<SoundEffect>(@"SoundArrowHit");
            sfxArrowHit = arrowHitSrc.CreateInstance();
            SoundEffect lightFireSrc = content.Load<SoundEffect>(@"SoundLightFire");
            sfxLightFire = lightFireSrc.CreateInstance();
            SoundEffect switchSrc = content.Load<SoundEffect>(@"SoundSwitch");
            sfxSwitch = switchSrc.CreateInstance();
            SoundEffect shatterSrc = content.Load<SoundEffect>(@"SoundShatter");
            sfxShatter = shatterSrc.CreateInstance();
            SoundEffect bossHitDeathSrc = content.Load<SoundEffect>(@"SoundBossHitDeath");
            sfxBossHitDeath = bossHitDeathSrc.CreateInstance();
            //
            SoundEffect explosionsMultipleSrc = content.Load<SoundEffect>(@"SoundExplosionsMultiple");
            sfxExplosionsMultiple = explosionsMultipleSrc.CreateInstance();
            SoundEffect quitSrc = content.Load<SoundEffect>(@"SoundQuit");
            sfxQuit = quitSrc.CreateInstance();
            SoundEffect blobDashSrc = content.Load<SoundEffect>(@"SoundBlobDash");
            sfxBlobDash = blobDashSrc.CreateInstance();
            SoundEffect selectFileSrc = content.Load<SoundEffect>(@"SoundSelectFile");
            sfxSelectFile = selectFileSrc.CreateInstance();
            SoundEffect mapWalkingSrc = content.Load<SoundEffect>(@"SoundMapWalking");
            sfxMapWalking = mapWalkingSrc.CreateInstance();
            //
            SoundEffect actorFallSrc = content.Load<SoundEffect>(@"SoundActorFall");
            sfxActorFall = actorFallSrc.CreateInstance();
            SoundEffect actorLandSrc = content.Load<SoundEffect>(@"SoundActorLand");
            sfxActorLand = actorLandSrc.CreateInstance();
            SoundEffect tapHollowSrc = content.Load<SoundEffect>(@"SoundTapHollow");
            sfxTapHollow = tapHollowSrc.CreateInstance();
            SoundEffect splashSrc = content.Load<SoundEffect>(@"SoundSplash");
            sfxSplash = splashSrc.CreateInstance();
            SoundEffect chickenSrc = content.Load<SoundEffect>(@"SoundPetChicken");
            sfxPetChicken = chickenSrc.CreateInstance();
            //
            SoundEffect dogSrc = content.Load<SoundEffect>(@"SoundPetDog");
            sfxPetDog = dogSrc.CreateInstance();
            SoundEffect netSrc = content.Load<SoundEffect>(@"SoundNet");
            sfxNet = netSrc.CreateInstance();
            SoundEffect boomerangFlyingSrc = content.Load<SoundEffect>(@"SoundBoomerangFlying");
            sfxBoomerangFlying = boomerangFlyingSrc.CreateInstance();
            SoundEffect bushCutSrc = content.Load<SoundEffect>(@"SoundBushCut");
            sfxBushCut = bushCutSrc.CreateInstance();
            SoundEffect grassWalkSrc = content.Load<SoundEffect>(@"SoundGrassWalk");
            sfxGrassWalk = grassWalkSrc.CreateInstance();
            //
            SoundEffect enterDungSrc = content.Load<SoundEffect>(@"SoundEnterDungeon");
            sfxEnterDungeon = enterDungSrc.CreateInstance();
            SoundEffect mapLocationSrc = content.Load<SoundEffect>(@"SoundMapLocation");
            sfxMapLocation = mapLocationSrc.CreateInstance();
            SoundEffect dragObjSrc = content.Load<SoundEffect>(@"SoundDragObject");
            sfxDragObj = dragObjSrc.CreateInstance();
            SoundEffect waterWalkSrc = content.Load<SoundEffect>(@"SoundWaterWalk");
            sfxWaterWalk = waterWalkSrc.CreateInstance();
            SoundEffect waterSwimSrc = content.Load<SoundEffect>(@"SoundWaterSwim");
            sfxWaterSwim = waterSwimSrc.CreateInstance();
            //
            SoundEffect enemyTauntSrc = content.Load<SoundEffect>(@"SoundEnemyTaunt");
            sfxEnemyTaunt = enemyTauntSrc.CreateInstance();
            SoundEffect ratSqueakSrc = content.Load<SoundEffect>(@"SoundRatSqueak");
            sfxRatSqueak = ratSqueakSrc.CreateInstance();
            SoundEffect shockSrc = content.Load<SoundEffect>(@"SoundShock");
            sfxShock = shockSrc.CreateInstance();
            SoundEffect castBoltSrc = content.Load<SoundEffect>(@"SoundCastBolt");
            sfxCastBolt = castBoltSrc.CreateInstance();
            SoundEffect castBombosSrc = content.Load<SoundEffect>(@"SoundCastBombos");
            sfxCastBombos = castBombosSrc.CreateInstance();
            //
            SoundEffect goldSpamSrc = content.Load<SoundEffect>(@"SoundGoldSpam");
            sfxGoldSpam = goldSpamSrc.CreateInstance();
            SoundEffect hammerSrc = content.Load<SoundEffect>(@"SoundHammer");
            sfxHammer = hammerSrc.CreateInstance();
            SoundEffect hammerPostSrc = content.Load<SoundEffect>(@"SoundHammerPost");
            sfxHammerPost = hammerPostSrc.CreateInstance();
            SoundEffect icerodSrc = content.Load<SoundEffect>(@"SoundIcerod");
            sfxIcerod = icerodSrc.CreateInstance();
            SoundEffect warpSrc = content.Load<SoundEffect>(@"SoundWarp");
            sfxWarp = warpSrc.CreateInstance();
            //



            //


            #endregion


        }

    }
}