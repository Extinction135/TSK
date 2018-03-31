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
    public static class Functions_GameObjectAnimList
    {
        //a list of byte4 lists that describe all game object animations
        static List<List<Byte4>> GameObjAnims;
        static int index;



        static Functions_GameObjectAnimList()
        {
            GameObjAnims = new List<List<Byte4>>
            {

                #region Non-Editor Room Objects

                //Exit
                new List<Byte4> { new Byte4(1, 0, 0, 0) },
                //ExitPillarLeft
                new List<Byte4> { new Byte4(0, 0, 0, 0) },
                //ExitPillarRight
                new List<Byte4> { new Byte4(0, 0, 1, 0) },
                //ExitLightFX
                new List<Byte4> { new Byte4(2, 0, 0, 0) },
                
                //DoorOpen
                new List<Byte4> { new Byte4(3, 0, 0, 0) },
                //DoorBombable
                new List<Byte4> { new Byte4(4, 2, 0, 0) },
                //DoorBoss
                new List<Byte4> { new Byte4(3, 2, 0, 0) },
                //DoorTrap
                new List<Byte4> { new Byte4(3, 1, 0, 0) },
                //DoorShut
                new List<Byte4> { new Byte4(3, 1, 0, 0) },
                //DoorFake
                new List<Byte4> { new Byte4(3, 1, 0, 0) },

                //WallStraight
                new List<Byte4> { new Byte4(4, 0, 0, 0) },
                //WallStraightCracked
                new List<Byte4> { new Byte4(4, 2, 0, 0) },
                //WallInteriorCorner
                new List<Byte4> { new Byte4(5, 0, 0, 0) },
                //WallExteriorCorner
                new List<Byte4> { new Byte4(5, 2, 0, 0) },
                //WallPillar
                new List<Byte4> { new Byte4(4, 1, 0, 0) },
                //WallStatue
                new List<Byte4> { new Byte4(5, 1, 0, 0) },
                //WallTorch
                new List<Byte4> { new Byte4(7, 0, 0, 0), new Byte4(7, 1, 0, 0), new Byte4(7, 2, 0, 0) },

                //BossDecal
                new List<Byte4> { new Byte4(1, 3, 0, 0) },
                //FloorDebrisBlood
                new List<Byte4> { new Byte4(6, 7, 0, 0) },

                #endregion


                #region Editor Room Objects

                //PitAnimated
                new List<Byte4> { new Byte4(1, 7, 0, 0) },
                //Bridge
                new List<Byte4> { new Byte4(2, 7, 0, 0) },
                //PitTop
                new List<Byte4> { new Byte4(3, 7, 0, 0) },
                //PitBottom
                new List<Byte4> { new Byte4(4, 7, 0, 0) },
                //PitTrap
                new List<Byte4> { new Byte4(7, 15, 0, 0) },

                //BossStatue
                new List<Byte4> { new Byte4(0, 3, 0, 0) },
                //Pillar
                new List<Byte4> { new Byte4(2, 3, 0, 0) },

                //ChestKey,
                new List<Byte4> { new Byte4(1, 6, 0, 0) },
                //ChestMap,
                new List<Byte4> { new Byte4(1, 6, 0, 0) },
                //ChestEmpty
                new List<Byte4> { new Byte4(2, 6, 0, 0) },

                //BlockDark
                new List<Byte4> { new Byte4(0, 4, 0, 0) },
                //BlockLight
                new List<Byte4> { new Byte4(1, 4, 0, 0) },
                //BlockSpike
                new List<Byte4> { new Byte4(0, 10, 0, 0) },

                //LeverOn
                new List<Byte4> { new Byte4(5, 14, 0, 0) },
                //LeverOff
                new List<Byte4> { new Byte4(5, 14, 1, 0) },
                //SpikesFloorOn
                new List<Byte4> { new Byte4(6, 14, 0, 0), new Byte4(7, 14, 0, 0) },
                //SpikesFloorOff
                new List<Byte4> { new Byte4(7, 14, 0, 0), new Byte4(7, 14, 0, 0) },
                //ConveyorBeltOn
                new List<Byte4> { new Byte4(5, 15, 0, 0), new Byte4(6, 15, 0, 0) },
                //ConveyorBeltOff
                new List<Byte4> { new Byte4(5, 15, 0, 0), new Byte4(5, 15, 0, 0) },

                //Pot
                new List<Byte4> { new Byte4(0, 6, 0, 0) },
                //Barrel
                new List<Byte4> { new Byte4(0, 5, 0, 0) },
                //Bumper
                new List<Byte4> { new Byte4(4, 5, 0, 0) },
                //Flamethrower
                new List<Byte4> { new Byte4(5, 5, 0, 0) },
                //IceTile
                new List<Byte4> { new Byte4(8, 15, 0, 0) },
                
                //Switch
                new List<Byte4> { new Byte4(3, 6, 0, 0) },
                //SwitchOff
                new List<Byte4> { new Byte4(4, 6, 0, 0) },

                //SwitchBlockBtn
                new List<Byte4> { new Byte4(3, 4, 0, 0) },
                //SwitchBlockDown
                new List<Byte4> { new Byte4(4, 4, 0, 0) },
                //SwitchBlockUp
                new List<Byte4> { new Byte4(5, 4, 0, 0) },
                //TorchUnlit
                new List<Byte4> { new Byte4(7, 3, 0, 0) },
                //TorchLit
                new List<Byte4> { new Byte4(7, 4, 0, 0), new Byte4(7, 5, 0, 0), new Byte4(7, 6, 0, 0) },
                
                //Fairy
                new List<Byte4> { new Byte4(12, 3, 0, 0), new Byte4(13, 3, 0, 0) },

                //Actor Spawn Objects
                //SpawnEnemy1
                new List<Byte4> { new Byte4(0, 15, 0, 0) },
                //SpawnEnemy2
                new List<Byte4> { new Byte4(1, 15, 0, 0) },

                #endregion


                #region Shop Objects

                //Bookcase1
                new List<Byte4> { new Byte4(0, 4, 0, 0) },
                //Bookcase2
                new List<Byte4> { new Byte4(1, 4, 0, 0) },
                //TableStone
                new List<Byte4> { new Byte4(5, 4, 0, 0) },
                //VendorAdvertisement
                new List<Byte4> { new Byte4(5, 5, 0, 0) },

                #endregion


                #region Vendors

                //VendorItems
                new List<Byte4> { new Byte4(0, 7, 0, 0), new Byte4(0, 7, 1, 0) },
                //VendorPotions
                new List<Byte4> { new Byte4(1, 7, 0, 0), new Byte4(1, 7, 1, 0) },
                //VendorMagic
                new List<Byte4> { new Byte4(2, 7, 0, 0), new Byte4(2, 7, 1, 0) },
                //VendorWeapons
                new List<Byte4> { new Byte4(3, 7, 0, 0), new Byte4(3, 7, 1, 0) },
                //VendorArmor
                new List<Byte4> { new Byte4(4, 7, 0, 0), new Byte4(4, 7, 1, 0) },
                //VendorEquipment
                new List<Byte4> { new Byte4(5, 7, 0, 0), new Byte4(5, 7, 1, 0) },
                //VendorPets
                new List<Byte4> { new Byte4(6, 7, 0, 0), new Byte4(6, 7, 1, 0) },
                //VendorStory
                new List<Byte4> { new Byte4(7, 7, 0, 0), new Byte4(7, 7, 1, 0) },

                #endregion


                #region Pickups

                //PickupRupee
                new List<Byte4> { new Byte4(20, 0, 0, 0), new Byte4(21, 0, 0, 0), new Byte4(22, 0, 0, 0), new Byte4(23, 0, 0, 0) },
                //PickupHeart
                new List<Byte4> { new Byte4(20, 1, 0, 0), new Byte4(21, 1, 0, 0), new Byte4(22, 1, 0, 0), new Byte4(23, 1, 0, 0) },
                //PickupMagic
                new List<Byte4> { new Byte4(20, 2, 0, 0), new Byte4(21, 2, 0, 0), new Byte4(22, 2, 0, 0), new Byte4(23, 2, 0, 0) },
                //PickupArrow
                new List<Byte4> { new Byte4(20, 3, 0, 0), new Byte4(21, 3, 0, 0), new Byte4(22, 3, 0, 0), new Byte4(23, 3, 0, 0) },
                //PickupBomb
                new List<Byte4> { new Byte4(20, 4, 0, 0), new Byte4(21, 4, 0, 0), new Byte4(22, 4, 0, 0), new Byte4(23, 4, 0, 0) },

                #endregion


                #region Projectiles

                //items
                //ProjectileBomb
                new List<Byte4> { new Byte4(4, 11, 0, 0), new Byte4(4, 12, 0, 0) },
                //ProjectileFireball
                new List<Byte4> { new Byte4(4, 9, 0, 0), new Byte4(4, 9, 1, 0) },
                //ProjectileBoomerang
                new List<Byte4> { new Byte4(4, 5, 0, 0), new Byte4(4, 5, 0, 0) },

                //weapons
                //ProjectileSword
                new List<Byte4> { new Byte4(0, 5, 0, 0), new Byte4(1, 5, 0, 0), new Byte4(2, 5, 0, 0), new Byte4(3, 5, 0, 0) },
                //ProjectileArrow
                new List<Byte4> { new Byte4(4, 7, 0, 0) },
                //ProjectileNet
                new List<Byte4> { new Byte4(0, 6, 0, 0), new Byte4(1, 6, 0, 0), new Byte4(2, 6, 0, 0), new Byte4(3, 6, 0, 0) },
                //ProjectileBow
                new List<Byte4> { new Byte4(4, 6, 0, 0) },

                //objects
                //ProjectileExplosion
                new List<Byte4> { new Byte4(5, 2, 0, 0), new Byte4(5, 2, 0, 0), new Byte4(6, 2, 0, 0), new Byte4(7, 2, 0, 0) },
                //Projectile ExplodingBarrel
                new List<Byte4> { new Byte4(0, 5, 0, 0), new Byte4(1, 5, 0, 0) },
                

                #endregion


                #region Particles

                //Small
                //ParticleSmokeSmall
                new List<Byte4> { new Byte4(2, 0, 0, 0), new Byte4(3, 0, 0, 0), new Byte4(4, 0, 0, 0), new Byte4(5, 0, 0, 0) },
                //ParticleSmokePuff
                new List<Byte4> { new Byte4(5, 1, 0, 0), new Byte4(5, 1, 0, 0), new Byte4(6, 1, 0, 0), new Byte4(7, 1, 0, 0) },
                //ParticleHitSparkle
                new List<Byte4> { new Byte4(8, 1, 0, 0), new Byte4(9, 1, 0, 0) },
                //ParticlePitAnimation
                new List<Byte4> { new Byte4(0, 14, 0, 0), new Byte4(1, 14, 0, 0), new Byte4(0, 15, 0, 0), new Byte4(1, 15, 0, 0) },

                //ParticleMapFlag
                new List<Byte4> { new Byte4(16, 0, 0, 0), new Byte4(17, 0, 0, 0), new Byte4(18, 0, 0, 0), new Byte4(19, 0, 0, 0) },
                //ParticleMapWave
                new List<Byte4> { new Byte4(16, 1, 0, 0), new Byte4(17, 1, 0, 0), new Byte4(18, 1, 0, 0), new Byte4(19, 1, 0, 0) },
                //ParticleMapCampfire
                new List<Byte4> { new Byte4(16, 1, 0, 0), new Byte4(17, 1, 0, 0), new Byte4(18, 1, 0, 0), new Byte4(19, 1, 0, 0) },

                //NormalSize
                //ParticleExplosion
                new List<Byte4> { new Byte4(5, 2, 0, 0), new Byte4(5, 2, 0, 0), new Byte4(6, 2, 0, 0), new Byte4(7, 2, 0, 0) },

                //ParticleAttention
                new List<Byte4> { new Byte4(6, 2, 0, 0), new Byte4(7, 2, 0, 0) },
                //new List<Byte4> { new Byte4(5, 1, 0, 0), new Byte4(6, 1, 0, 0), new Byte4(7, 1, 0, 0) },


                //ParticleFire
                new List<Byte4> { new Byte4(5, 3, 0, 0), new Byte4(6, 3, 0, 0), new Byte4(7, 3, 0, 0) },
                //ParticleSplash
                new List<Byte4> {
                    new Byte4(0, 2, 0, 0), new Byte4(1, 2, 0, 0),
                    new Byte4(2, 2, 0, 0), new Byte4(3, 2, 0, 0),
                    new Byte4(4, 2, 0, 0)
                },

                //ParticleDebris
                new List<Byte4> { new Byte4(15, 15, 0, 0) },

                //ParticleBottleEmpty
                new List<Byte4> { new Byte4(6, 5, 0, 0) },
                //ParticleBottleHealth
                new List<Byte4> { new Byte4(6, 6, 0, 0) },
                //ParticleBottleMagic
                new List<Byte4> { new Byte4(6, 7, 0, 0) },
                //ParticleBottleCombo
                new List<Byte4> { new Byte4(6, 8, 0, 0) },
                //ParticleBottleFairy
                new List<Byte4> { new Byte4(6, 9, 0, 0) },
                //ParticleBottleBlob
                new List<Byte4> { new Byte4(6, 10, 0, 0) },

                //Rewards
                //ParticleRewardKey,
                new List<Byte4> { new Byte4(13, 4, 0, 0) },
                //ParticleRewardMap,
                new List<Byte4> { new Byte4(12, 4, 0, 0) },

                #endregion

            };
        }

        public static void SetAnimationList(GameObject Obj)
        {   //we could do this by checking the obj.type, but that doesn't scale well
            //instead we extract the enum type value and set the animation using that int
            //this requires that the GameObjAnims list match the ObjType enum defs exactly
            index = (int)Obj.type; //get the type int value
            Obj.compAnim.currentAnimation = GameObjAnims[index]; //set animation based on index
        }

    }
}