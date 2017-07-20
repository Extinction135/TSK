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
    public static class Functions_GameObject
    {

        public static void ResetObject(GameObject Obj)
        {
            //reset the obj
            Obj.direction = Direction.Down;
            Obj.type = ObjType.WallStraight; //reset the type
            Obj.group = ObjGroup.Object; //assume object is a generic object
            Obj.lifetime = 0; //assume obj exists forever (not projectile)
            Obj.lifeCounter = 0; //reset counter
            Obj.active = true; //assume this object should draw / animate
            Obj.getsAI = false; //most objects do not get any AI input
            //reset the sprite component
            Obj.compSprite.cellSize.X = 16 * 1; //assume cell size is 16x16 (most are)
            Obj.compSprite.cellSize.Y = 16 * 1;
            Obj.compSprite.zOffset = 0;
            Obj.compSprite.flipHorizontally = false;
            Obj.compSprite.rotation = Rotation.None;
            Obj.compSprite.scale = 1.0f;
            //reset the animation component
            Obj.compAnim.speed = 10; //set obj's animation speed to default value
            Obj.compAnim.loop = true; //assume obj's animation loops
            Obj.compAnim.index = 0; //reset the current animation index/frame
            //reset the collision component
            Obj.compCollision.active = false; //assume this object doesn't move, shouldnt check itself for collisions vs objs
            Obj.compCollision.blocking = true; //assume the object is blocking (most are)
            Obj.compCollision.rec.Width = 16; //assume collisionRec is 16x16
            Obj.compCollision.rec.Height = 16; //(most are)
            Obj.compCollision.offsetX = -8; //assume collisionRec offset is -8x-8
            Obj.compCollision.offsetY = -8; //(most are)
            //reset the move component
            Obj.compMove.magnitude.X = 0; //discard any previous magnitude
            Obj.compMove.magnitude.Y = 0; //
            Obj.compMove.speed = 0.0f; //assume this object doesn't move
        }

        public static void SetRotation(GameObject Obj)
        {   //sprites are created facing Down, but we will need to set the spite rotation based on direction
            Obj.compSprite.rotation = Rotation.None; //reset sprite rotation to default DOWN
            if (Obj.direction == Direction.Up) { Obj.compSprite.rotation = Rotation.Clockwise180; }
            else if (Obj.direction == Direction.Right) { Obj.compSprite.rotation = Rotation.Clockwise270; }
            else if (Obj.direction == Direction.Left) { Obj.compSprite.rotation = Rotation.Clockwise90; }
            //some objects flip based on their direction
            if (Obj.type == ObjType.ProjectileSword)
            {
                if (Obj.direction == Direction.Down || Obj.direction == Direction.Left)
                { Obj.compSprite.flipHorizontally = true; }
            }
        }

        public static void SetWeaponCollisions(GameObject Obj)
        {   //set the weapons's collision rec + offsets to the sprite's dimensions
            //these values are based off the sword sprite's dimentions
            if (Obj.direction == Direction.Up)
            {
                Obj.compCollision.offsetX = -4; Obj.compCollision.offsetY = -4;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 15;
            }
            else if (Obj.direction == Direction.Down)
            {
                Obj.compCollision.offsetX = -4; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 10;
            }
            else if (Obj.direction == Direction.Left)
            {
                Obj.compCollision.offsetX = -4; Obj.compCollision.offsetY = -3;
                Obj.compCollision.rec.Width = 11; Obj.compCollision.rec.Height = 10;
            }
            else //right
            {
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -3;
                Obj.compCollision.rec.Width = 11; Obj.compCollision.rec.Height = 10;
            }
        }



        public static void SetType(GameObject Obj, ObjType Type)
        {   //Obj.direction should be set prior to this method running
            Obj.type = Type;


            //Non-Editor Room Objects

            #region Exits

            if (Type == ObjType.Exit)
            {
                Obj.compSprite.cellSize.Y = 16 * 3; //nonstandard size
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.group = ObjGroup.Door;
                Obj.compCollision.rec.Height = 4;
                Obj.compCollision.offsetY = 32 + 4;
                Obj.compCollision.blocking = false;
            }
            else if (Type == ObjType.ExitPillarLeft || 
                Type == ObjType.ExitPillarRight)
            {
                Obj.compSprite.cellSize.Y = 16 * 3; //nonstandard size
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.group = ObjGroup.Door;
                Obj.compCollision.rec.Height = 32 - 5;
                Obj.compCollision.offsetY = 14;
            }
            else if (Type == ObjType.ExitLightFX)
            {
                Obj.compSprite.cellSize.Y = 16 * 2; //nonstandard size
                Obj.compCollision.offsetY = 0;
                Obj.compSprite.zOffset = 256; //sort above everything
                Obj.compCollision.blocking = false;
            }

            #endregion


            #region Doors and Walls

            else if (Type == ObjType.DoorOpen ||
                Type == ObjType.DoorBombed ||
                Type == ObjType.DoorTrap)
            {
                Obj.compCollision.blocking = false;
                if (Obj.direction == Direction.Down)
                { Obj.compSprite.zOffset = 4; } else { Obj.compSprite.zOffset = 16; }
                Obj.group = ObjGroup.Door;
            }
            else if (Type == ObjType.DoorBombable || Type == ObjType.DoorBoss ||
                Type == ObjType.DoorShut || Type == ObjType.DoorFake)
            {
                if (Obj.direction == Direction.Down)
                { Obj.compSprite.zOffset = 4; }
                else { Obj.compSprite.zOffset = 16; }
                Obj.group = ObjGroup.Door;
            }
            else if (Type == ObjType.WallStraight || 
                Type == ObjType.WallStraightCracked ||
                Type == ObjType.WallInteriorCorner || 
                Type == ObjType.WallExteriorCorner ||
                Type == ObjType.WallPillar)
            {
                if (Obj.direction == Direction.Down) { Obj.compSprite.zOffset = -32; }
                else if (Obj.direction == Direction.Up) { Obj.compSprite.zOffset = 16; }
                else { Obj.compSprite.zOffset = 8; }
                Obj.group = ObjGroup.Wall;
            }
            else if (Type == ObjType.WallStatue)
            {
                Obj.compSprite.zOffset = 24;
                if (Obj.direction == Direction.Down) { Obj.compSprite.zOffset = -16; }
                Obj.group = ObjGroup.Wall;
                Obj.getsAI = true; //obj gets AI
            }
            else if (Type == ObjType.WallTorch)
            {
                Obj.compCollision.blocking = false;
                Obj.compSprite.zOffset = 24;
                if (Obj.direction == Direction.Down) { Obj.compSprite.zOffset = -16; }
            }

            #endregion


            #region Floor Objects

            else if (Type == ObjType.BossDecal)
            {
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.compCollision.blocking = false;
            }
            else if (Type == ObjType.DebrisFloor)
            {
                Obj.compCollision.blocking = false;
                Obj.compSprite.zOffset = -32; //sort to floor
            }

            #endregion


            //Editor Room Objects

            #region Pits

            else if (Type == ObjType.PitAnimated)
            {
                //this pit interacts with actor
                Obj.compSprite.zOffset = -40; //sort under pit decorations
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 10;
                Obj.compCollision.blocking = false;
            }
            else if (Type == ObjType.PitTop || Type == ObjType.PitBottom)
            {   //this is pit decoration
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.compCollision.rec.Width = 16; Obj.compCollision.rec.Height = 8;
                if (Type == ObjType.PitBottom) { Obj.compCollision.offsetY = 4; }
                Obj.compCollision.blocking = false;
            }
            else if (
                Type == ObjType.PitTrapReady || Type == ObjType.PitTrapOpening)
            {   //this becomes a pit
                Obj.compCollision.offsetX = -6;
                Obj.compCollision.offsetY = -6;
                Obj.compSprite.zOffset = -32; //sort to floor
            }

            #endregion


            #region Chests

            else if (Type == ObjType.ChestGold || Type == ObjType.ChestKey ||
                Type == ObjType.ChestMap || Type == ObjType.ChestHeartPiece ||
                Type == ObjType.ChestEmpty)
            {
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -3;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 11;
                Obj.compSprite.zOffset = -7;
                //unopened chests get the objGroup of Chest
                if (Type != ObjType.ChestEmpty)
                { Obj.group = ObjGroup.Chest; }
            }

            #endregion


            #region Blocks

            else if (Type == ObjType.BlockDark || Type == ObjType.BlockLight)
            {
                Obj.compSprite.zOffset = -7;
            }
            else if (Type == ObjType.BlockDraggable)
            {
                Obj.compCollision.rec.Height = 12;
                Obj.compCollision.offsetY = -4;
                Obj.compSprite.zOffset = -7;
                Obj.group = ObjGroup.Draggable;
            }
            else if (Type == ObjType.BlockSpikes)
            {
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -7;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 14;
                Obj.compSprite.zOffset = -7;
                Obj.compCollision.blocking = false;
                Obj.compMove.speed = 0.75f; //spike blocks move
            }

            #endregion


            #region Liftables / Throwables - Skull

            else if (Type == ObjType.PotSkull)
            {
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -4;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 12;
                Obj.compSprite.zOffset = -7;
                Obj.group = ObjGroup.Liftable;
            }

            #endregion


            #region Switch Blocks & Button

            else if (Type == ObjType.SwitchBlockBtn)
            {
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -4;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 12;
                Obj.compSprite.zOffset = -7;
            }
            else if (Type == ObjType.SwitchBlockDown)
            {
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.compCollision.blocking = false;
            }
            else if (Type == ObjType.SwitchBlockUp)
            {
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -7;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 14;
                Obj.compSprite.zOffset = -7;
            }

            #endregion


            #region Torches

            else if (Type == ObjType.TorchUnlit || Type == ObjType.TorchLit)
            {
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -4;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 12;
                Obj.compSprite.zOffset = -7;
            }

            #endregion


            #region Other Dungeon Objects

            else if (Type == ObjType.BossStatue)
            {
                Obj.group = ObjGroup.Draggable;
                Obj.compCollision.rec.Height = 8;
                Obj.compCollision.offsetY = -1;
            }
            else if (Type == ObjType.Pillar)
            {
                Obj.compSprite.zOffset = 2;
                Obj.compCollision.rec.Width = 10;
                Obj.compCollision.offsetX = -5;
            }
            else if (Type == ObjType.Flamethrower)
            {
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.compCollision.blocking = false;
                Obj.getsAI = true; //obj gets AI
            }
            else if (Type == ObjType.Bumper)
            {
                Obj.compCollision.blocking = false;
            }
            else if (Type == ObjType.ConveyorBelt)
            {
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.compCollision.blocking = false;
                //directions are slightly different for this obj
                if (Obj.direction == Direction.Right) { Obj.compSprite.rotation = Rotation.Clockwise270; }
                else if (Obj.direction == Direction.Left) { Obj.compSprite.rotation = Rotation.Clockwise90; }
            }
            else if (
                Type == ObjType.SpikesFloor ||
                Type == ObjType.Switch ||
                Type == ObjType.Bridge)
            {
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.compCollision.blocking = false;
            }
            else if (Type == ObjType.Lever)
            {
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = 2;
                Obj.compCollision.rec.Width = 12; Obj.compCollision.rec.Height = 5;
                Obj.compSprite.zOffset = -7;
            }

            #endregion


            #region Enemy Spawn Objects

            else if (Type == ObjType.SpawnEnemy1 || Type == ObjType.SpawnEnemy2 || Type == ObjType.SpawnEnemy3)
            {
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.compCollision.blocking = false;
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.group = ObjGroup.EnemySpawn;
            }

            #endregion


            //Additional Game Objects

            #region Vendor & Story Objects

            else if (Type == ObjType.VendorItems || Type == ObjType.VendorPotions ||
                Type == ObjType.VendorMagic || Type == ObjType.VendorWeapons ||
                Type == ObjType.VendorArmor || Type == ObjType.VendorEquipment)
            {   
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -3;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 11;
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compSprite.zOffset = -7;
                Obj.group = ObjGroup.Vendor;
                Obj.compAnim.speed = 20; //slow animation
            }
            else if (Type == ObjType.VendorStory)
            {
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -3;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 11;
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compSprite.zOffset = 0;
                Obj.compAnim.speed = 20; //slow animation
            }
            else if (Type == ObjType.VendorAdvertisement)
            {
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compCollision.blocking = false;
                Obj.compSprite.zOffset = 32;
                Obj.compAnim.speed = 100; //very slow animation
            }

            #endregion


            #region Pickups

            else if (Type == ObjType.PickupRupee || Type == ObjType.PickupHeart ||
                Type == ObjType.PickupMagic || Type == ObjType.PickupArrow ||
                Type == ObjType.PickupBomb)
            {
                Obj.compSprite.cellSize.X = 8; //non standard cellsize
                Obj.compCollision.offsetX = -8; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 8; Obj.compCollision.rec.Height = 10;
                Obj.compCollision.blocking = false;
                Obj.group = ObjGroup.Pickup;
                Obj.lifetime = 255; //in frames
                Obj.compAnim.speed = 6; //in frames
            }

            #endregion


            #region Projectiles

            else if (Type == ObjType.ProjectileSword)
            {
                Obj.compSprite.zOffset = 16;
                Obj.compCollision.blocking = false;
                Obj.group = ObjGroup.Projectile;
                Obj.lifetime = 18; //in frames
                Obj.compAnim.speed = 2; //in frames
                Obj.compAnim.loop = false;
                SetWeaponCollisions(Obj); //set collisions based on direction
            }
            else if (Type == ObjType.ProjectileFireball)
            {
                Obj.compSprite.zOffset = 16;
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 10;
                Obj.compCollision.blocking = false;
                Obj.group = ObjGroup.Projectile;
                Obj.lifetime = 200; //in frames
                Obj.compAnim.speed = 5; //in frames
                Obj.compMove.speed = 1.5f; //fireballs move slow
            }
            else if (Type == ObjType.ProjectileBomb)
            {
                Obj.compSprite.zOffset = -4; //sort to floor
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 10;
                Obj.compCollision.blocking = false;
                Obj.group = ObjGroup.Projectile;
                Obj.lifetime = 100; //in frames
                Obj.compAnim.speed = 7; //in frames
            }
            else if (Type == ObjType.ProjectileExplosion)
            {
                Obj.compSprite.zOffset = 16;
                Obj.compCollision.offsetX = -12; Obj.compCollision.offsetY = -12;
                Obj.compCollision.rec.Width = 24; Obj.compCollision.rec.Height = 24;
                Obj.compCollision.blocking = false;
                Obj.group = ObjGroup.Projectile;
                Obj.lifetime = 24; //in frames
                Obj.compAnim.speed = 5; //in frames
                Obj.compAnim.loop = false;
            }
            else if (Type == ObjType.ProjectileArrow)
            {
                Obj.compSprite.zOffset = 16;
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 10;
                Obj.compCollision.blocking = false;
                Obj.group = ObjGroup.Projectile;
                Obj.lifetime = 200; //in frames
                Obj.compAnim.speed = 5; //in frames
                Obj.compMove.speed = 2.25f; //arrow move fast
                //set collision rec based on direction
                if (Obj.direction == Direction.Up || Obj.direction == Direction.Down)
                {
                    Obj.compCollision.offsetX = -2; Obj.compCollision.offsetY = -6;
                    Obj.compCollision.rec.Width = 4; Obj.compCollision.rec.Height = 12;
                }
                else //left or right
                {
                    Obj.compCollision.offsetX = -6; Obj.compCollision.offsetY = -2;
                    Obj.compCollision.rec.Width = 12; Obj.compCollision.rec.Height = 4;
                }
            }

            #endregion


            #region Particles

            //Particles - small
            else if (Type == ObjType.ParticleDashPuff)
            {
                Obj.compSprite.cellSize.X = 8; Obj.compSprite.cellSize.Y = 8; //nonstandard size
                Obj.compSprite.zOffset = -8;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 24; //in frames
                Obj.compAnim.speed = 6; //in frames
                Obj.compAnim.loop = false;
            }
            else if (Type == ObjType.ParticleSmokePuff)
            {
                Obj.compSprite.cellSize.X = 8; Obj.compSprite.cellSize.Y = 8; //nonstandard size
                Obj.compSprite.zOffset = 16;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 24; //in frames
                Obj.compAnim.speed = 5; //in frames
                Obj.compAnim.loop = false;
            }
            else if (Type == ObjType.ParticleHitSparkle)
            {
                Obj.compSprite.cellSize.X = 8; Obj.compSprite.cellSize.Y = 8; //nonstandard size
                Obj.compSprite.zOffset = 16;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 24; //in frames
                Obj.compAnim.speed = 6; //in frames
            }

            //Particles - normal size
            else if (Type == ObjType.ParticleExplosion)
            {
                Obj.compSprite.zOffset = 16;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 24; //in frames
                Obj.compAnim.speed = 5; //in frames
                Obj.compAnim.loop = false;
            }
            else if (Type == ObjType.ParticleAttention)
            {
                Obj.compSprite.zOffset = 32;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 24; //in frames
                Obj.compAnim.speed = 6; //in frames
                Obj.compAnim.loop = false;
            }
            else if (Type == ObjType.ParticleFire)
            {
                Obj.compSprite.zOffset = 12;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 100; //in frames
                Obj.compAnim.speed = 7; //in frames
            }
            else if (Type == ObjType.ParticleFairy)
            {
                Obj.compSprite.zOffset = 32;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 50; //in frames
                Obj.compAnim.speed = 10; //in frames
            }
            else if (Type == ObjType.ParticleBow)
            {
                Obj.compSprite.zOffset = 32;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 15; //in frames
                Obj.compAnim.speed = 10; //in frames
                Obj.compAnim.loop = false;
            }

            //Particles - Rewards & Bottles
            else if (Type == ObjType.ParticleRewardGold ||
                Type == ObjType.ParticleRewardKey ||
                Type == ObjType.ParticleRewardMap ||
                Type == ObjType.ParticleRewardHeartPiece ||
                Type == ObjType.ParticleRewardHeartFull ||
                Type == ObjType.ParticleHealthPotion ||
                Type == ObjType.ParticleMagicPotion ||
                Type == ObjType.ParticleFairyBottle)
            {
                Obj.compSprite.zOffset = 32;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 40; //in frames
            }

            #endregion


            //particles do not block upon collision
            if (Obj.group == ObjGroup.Particle) { Obj.compCollision.blocking = false; }
            SetRotation(Obj);
            Functions_GameObjectAnimList.SetAnimationList(Obj); //set obj animation list based on type
            Functions_Component.UpdateCellSize(Obj.compSprite);
            Functions_Component.Align(Obj.compMove, Obj.compSprite, Obj.compCollision);
        }



        public static void Update(GameObject Obj)
        {
            if(Obj.lifetime > 0) //if the obj has a lifetime, count it
            {  
                Obj.lifeCounter++; //increment the life counter of the gameobject
                //handle the object's birth & death events
                if (Obj.lifeCounter == 2) { Functions_Entity.HandleBirthEvent(Obj); }
                if (Obj.lifeCounter >= Obj.lifetime)
                {   //any dead object is released
                    Functions_Entity.HandleDeathEvent(Obj);
                    Functions_Pool.Release(Obj);
                }
            }
            //certain objects get AI input
            if(Obj.getsAI) { Functions_Ai.HandleObj(Obj); }
        }

    }
}