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
    public static class GameObjectAnimListManager
    {
        //a list of byte4 lists that describe all game object animations
        static List<List<Byte4>> GameObjAnims;
        static GameObjectAnimListManager()
        {
            GameObjAnims = new List<List<Byte4>>
            {

                #region Room Objects

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
                //DoorBombed
                new List<Byte4> { new Byte4(3, 2, 0, 0) },
                //DoorBoss
                new List<Byte4> { new Byte4(3, 3, 0, 0) },
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
                //WallDecoration
                new List<Byte4> { new Byte4(5, 1, 0, 0) },

                //PitTop
                new List<Byte4> { new Byte4(2, 4, 0, 0) },
                //PitBottom
                new List<Byte4> { new Byte4(2, 5, 0, 0) },
                //PitTrapReady
                new List<Byte4> { new Byte4(6, 3, 0, 0) },
                //PitTrapOpening
                new List<Byte4> { new Byte4(6, 3, 0, 0) },

                //BossStatue
                new List<Byte4> { new Byte4(0, 3, 0, 0) },
                //BossDecal
                new List<Byte4> { new Byte4(1, 3, 0, 0) },
                //Pillar
                new List<Byte4> { new Byte4(2, 3, 0, 0) },
                //WallTorch
                new List<Byte4> { new Byte4(7, 0, 0, 0), new Byte4(7, 1, 0, 0), new Byte4(7, 2, 0, 0) },

                #endregion


                #region Interactive Objects

                //ChestGold,
                new List<Byte4> { new Byte4(1, 6, 0, 0) },
                //ChestKey,
                new List<Byte4> { new Byte4(1, 6, 0, 0) },
                //ChestMap,
                new List<Byte4> { new Byte4(1, 6, 0, 0) },
                //ChestEmpty
                new List<Byte4> { new Byte4(2, 6, 0, 0) },

                //BlockDraggable
                new List<Byte4> { new Byte4(2, 4, 0, 0) },
                //BlockDark
                new List<Byte4> { new Byte4(0, 4, 0, 0) },
                //BlockLight
                new List<Byte4> { new Byte4(1, 4, 0, 0) },
                //BlockSpikes
                new List<Byte4> { new Byte4(3, 5, 0, 0) },
                //Lever
                new List<Byte4> { new Byte4(0, 5, 0, 0) },
                //PotSkull
                new List<Byte4> { new Byte4(0, 6, 0, 0) },
                //SpikesFloor
                new List<Byte4> { new Byte4(1, 5, 0, 0), new Byte4(2, 5, 0, 0) },
                //Bumper
                new List<Byte4> { new Byte4(4, 5, 0, 0) },
                //Flamethrower
                new List<Byte4> { new Byte4(5, 5, 0, 0) },
                //Switch
                new List<Byte4> { new Byte4(3, 6, 0, 0) },
                //Bridge
                new List<Byte4> { new Byte4(6, 5, 0, 0) },
                //SwitchBlockBtn
                new List<Byte4> { new Byte4(3, 4, 0, 0) },
                //SwitchBlockDown
                new List<Byte4> { new Byte4(4, 4, 0, 0) },
                //SwitchBlockUp
                new List<Byte4> { new Byte4(5, 4, 0, 0) },
                //TorchUnlit
                new List<Byte4> { new Byte4(7, 3, 0, 0) },
                //TorchLit
                new List<Byte4> { new Byte4(7, 4, 0, 0), new Byte4(7, 5, 0, 0), new Byte4(7, 6, 0, 0), new Byte4(7, 7, 0, 0) },
                //ConveyorBelt
                new List<Byte4> { new Byte4(4, 6, 0, 0), new Byte4(5, 6, 0, 0), new Byte4(6, 6, 0, 0) },

                #endregion


                #region Items

                //ItemRupee
                new List<Byte4> { new Byte4(1, 7, 0, 0), new Byte4(1, 7, 0, 0), new Byte4(2, 7, 0, 0), new Byte4(3, 7, 0, 0) },
                //ItemHeart
                new List<Byte4> { new Byte4(4, 7, 0, 0), new Byte4(5, 7, 0, 0), new Byte4(6, 7, 0, 0), new Byte4(7, 7, 0, 0) },

                #endregion


                #region Projectiles

                //ProjectileSword
                new List<Byte4> { new Byte4(0, 4, 0, 0), new Byte4(1, 4, 0, 0), new Byte4(2, 4, 0, 0), new Byte4(3, 4, 0, 0) },

                #endregion


                #region Particles

                //ParticleSmokeSmall
                new List<Byte4> { new Byte4(2, 0, 0, 0), new Byte4(3, 0, 0, 0), new Byte4(4, 0, 0, 0), new Byte4(5, 0, 0, 0) },
                //ParticleExplosion
                new List<Byte4> { new Byte4(5, 0, 0, 0), new Byte4(6, 0, 0, 0), new Byte4(7, 0, 0, 0) },
                //ParticleSmokePuff
                new List<Byte4> { new Byte4(5, 0, 0, 0), new Byte4(6, 0, 0, 0), new Byte4(7, 0, 0, 0) },
                //ParticleHitSparkle
                new List<Byte4> { new Byte4(8, 1, 0, 0), new Byte4(9, 1, 0, 0) },

                //ParticleReward50Gold
                new List<Byte4> { new Byte4(6, 2, 0, 0) },
                //ParticleRewardKey,
                new List<Byte4> { new Byte4(7, 2, 0, 0) },
                //ParticleRewardMap,
                new List<Byte4> { new Byte4(5, 2, 0, 0) },

                #endregion

            };
        }



        static int index;
        public static void SetAnimationList(GameObject Obj)
        {
            //we could do this by checking the obj.type, but that doesn't scale well
            //instead we extract the enum type value and set the animation using that int
            //this requires that the GameObjAnims list match the GameObject.type definition exactly
            index = (int)Obj.type; //get the type int value
            Obj.compAnim.currentAnimation = GameObjAnims[index]; //set animation based on index
        }
    }
}