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
    public static class GameObjectFunctions
    {

        static Vector2 offset = new Vector2(0, 0);



        public static void ResetObject(GameObject Obj)
        {
            Obj.compSprite.cellSize.x = 16 * 1; //assume cell size is 16x16 (most are)
            Obj.compSprite.cellSize.y = 16 * 1;
            Obj.compSprite.zOffset = 0;

            Obj.group = ObjGroup.Object; //assume object is a generic object
            Obj.lifetime = 0; //assume obj exists forever (not projectile)
            Obj.lifeCounter = 0; //reset counter
            Obj.active = true; //assume this object should draw / animate

            Obj.compAnim.speed = 10; //set obj's animation speed to default value
            Obj.compAnim.loop = true; //assume obj's animation loops
            Obj.compAnim.index = 0; //reset the current animation index/frame

            Obj.compCollision.active = false; //assume this object doesn't move, shouldnt check itself for collisions vs objs
            Obj.compCollision.blocking = true; //assume the object is blocking (most are)
            Obj.compCollision.rec.Width = 16; //assume collisionRec is 16x16
            Obj.compCollision.rec.Height = 16; //(most are)
            Obj.compCollision.offsetX = -8; //assume collisionRec offset is -8x-8
            Obj.compCollision.offsetY = -8; //(most are)

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
        }

        public static void SetParticleFields(GameObject Obj)
        {   //all particles do not block, and don't use collision recs
            Obj.compCollision.blocking = false;
            Obj.compCollision.offsetX = 0; Obj.compCollision.offsetY = 0;
            Obj.compCollision.rec.Width = 0; Obj.compCollision.rec.Height = 0;
        }

        public static void SpawnLoot(Vector2 Pos)
        {   //either spawn a rupee or a heart item
            if (GetRandom.Int(0, 100) > 50)
            { GameObjectFunctions.SpawnProjectile(ObjType.ItemRupee, Pos, Direction.Down); }
            else
            { GameObjectFunctions.SpawnProjectile(ObjType.ItemHeart, Pos, Direction.Down); }
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

        public static void SpawnProjectile(ObjType Type, Vector2 Pos, Direction Direction)
        {   //a projectile always has a direction, so it inherit's actor's direction
            GameObject projectile = PoolFunctions.GetProjectile();
            projectile.type = Type;

            //convert projectile's directions to cardinal
            projectile.direction = MovementFunctions.ConvertDiagonalDirection(Direction);
            projectile.compMove.direction = MovementFunctions.ConvertDiagonalDirection(Direction);

            AlignProjectile(projectile, Pos);
            SetType(projectile, projectile.type);
        }

        public static void SpawnParticle(ObjType Type, Vector2 Pos)
        {   //a particle doesn't have a direction, so it doesn't need actor's direction
            GameObject particle = PoolFunctions.GetProjectile();
            particle.type = Type;
            particle.direction = Direction.Down;
            particle.compMove.direction = Direction.None; //particles dont move
            AlignParticle(particle, Pos);
            SetType(particle, particle.type);
        }

        public static void AlignProjectile(GameObject Projectile, Vector2 Pos)
        {
            offset.X = 0; offset.Y = 0;

            if (Projectile.compMove.speed > 0.0f)
            {   //moving projectiles have the same offsets
                if (Projectile.direction == Direction.Down) { offset.Y = 16; }
                else if (Projectile.direction == Direction.Up) { offset.Y = -16; }
                else if (Projectile.direction == Direction.Right) { offset.X = 16; }
                else if (Projectile.direction == Direction.Left) { offset.X = -16; }
                Projectile.compSprite.flipHorizontally = false;
            }
            else
            {   //stationary (weapon) projectiles have the same offsets
                if (Projectile.direction == Direction.Down)
                { offset.X = -1; offset.Y = 15; Projectile.compSprite.flipHorizontally = true; }
                else if (Projectile.direction == Direction.Up)
                { offset.X = 1; offset.Y = -12; Projectile.compSprite.flipHorizontally = false; }
                else if (Projectile.direction == Direction.Right)
                { offset.X = 14; offset.Y = 0; Projectile.compSprite.flipHorizontally = false; }
                else if (Projectile.direction == Direction.Left)
                { offset.X = -14; offset.Y = 0; Projectile.compSprite.flipHorizontally = true; }
            }

            //teleport the projectile to the position with the offset
            MovementFunctions.Teleport(Projectile.compMove, Pos.X + offset.X, Pos.Y + offset.Y);
        }

        public static void AlignParticle(GameObject Particle, Vector2 Pos)
        {
            offset.X = 0; offset.Y = 0;
            Particle.compSprite.rotation = Rotation.None;
            Particle.compSprite.flipHorizontally = false;
            //center horizontally, place near actor's feet
            if (Particle.type == ObjType.ParticleDashPuff) { offset.X = 4; offset.Y = 8; }
            //teleport the projectile to the position with the offset
            MovementFunctions.Teleport(Particle.compMove, Pos.X + offset.X, Pos.Y + offset.Y);
        }







        public static void SetType(GameObject Obj, ObjType Type)
        {
            Obj.type = Type;
            GameObjectAnimListManager.SetAnimationList(Obj); //set obj animation list based on type
            ResetObject(Obj); //set obj fields to most common values
            SetRotation(Obj); //set the obj's sprite rotation


            #region Room Objects

            if (Type == ObjType.Exit)
            {
                Obj.compSprite.cellSize.y = 16 * 3; //nonstandard size
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.group = ObjGroup.Door;
                Obj.compCollision.rec.Height = 2;
                Obj.compCollision.offsetY = 32 + 6;
                Obj.compCollision.blocking = false;
            }
            else if (Type == ObjType.ExitPillarLeft || 
                Type == ObjType.ExitPillarRight)
            {
                Obj.compSprite.cellSize.y = 16 * 3; //nonstandard size
                Obj.compCollision.rec.Height = 32 - 5;
                Obj.compCollision.offsetY = 14;
            }
            else if (Type == ObjType.ExitLightFX)
            {
                Obj.compSprite.cellSize.y = 16 * 2; //nonstandard size
                Obj.compCollision.offsetY = 0;
                Obj.compSprite.zOffset = 256; //sort above everything
            }
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
                Obj.group = ObjGroup.Door;
            }
            else if (Type == ObjType.WallStraight || Type == ObjType.WallStraightCracked ||
                Type == ObjType.WallInteriorCorner || Type == ObjType.WallExteriorCorner ||
                Type == ObjType.WallPillar || Type == ObjType.WallDecoration)
            {
                Obj.group = ObjGroup.Wall;
            }
            else if (Type == ObjType.PitTop)
            {
                //pits dont collide with actors
                Obj.compSprite.zOffset = -32; //sort to floor
            }
            else if (Type == ObjType.PitBottom)
            {
                //instead we'll use an animated liquid obj for collision checking
                //pits just sit ontop of this object as decoration
                Obj.compSprite.zOffset = -32; //sort to floor
            }
            else if (
                Type == ObjType.PitTrapReady || Type == ObjType.PitTrapOpening)
            {
                Obj.compCollision.offsetX = -6;
                Obj.compCollision.offsetY = -6;
                Obj.compSprite.zOffset = -32; //sort to floor
            }
            else if (Type == ObjType.BossStatue)
            {
                Obj.group = ObjGroup.Draggable;
                Obj.compCollision.rec.Height = 8;
                Obj.compCollision.offsetY = -1;
            }
            else if (Type == ObjType.BossDecal)
            {
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.compCollision.blocking = false;
            }
            else if (Type == ObjType.Pillar)
            {
                Obj.compSprite.zOffset = 2;
            }
            else if (Type == ObjType.WallTorch)
            {
                Obj.compCollision.blocking = false;
                Obj.compSprite.zOffset = 2;
            }
            else if (Type == ObjType.DebrisFloor)
            {
                Obj.compCollision.blocking = false;
                Obj.compSprite.zOffset = -32;
            }

            #endregion


            #region Interactive Objects

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
            else if (Type == ObjType.BlockDraggable)
            {
                Obj.compCollision.rec.Height = 12;
                Obj.compCollision.offsetY = -4;
                Obj.compSprite.zOffset = -7;
                Obj.group = ObjGroup.Draggable;
            }
            else if (Type == ObjType.BlockDark || Type == ObjType.BlockLight)
            {
                Obj.compSprite.zOffset = -7;
            }
            else if (Type == ObjType.BlockSpikes)
            {
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -7;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 14;
                Obj.compSprite.zOffset = -7;
                Obj.compCollision.blocking = false;
            }
            else if (Type == ObjType.Lever)
            {
                Obj.compCollision.offsetX = -6; Obj.compCollision.offsetY = 1;
                Obj.compCollision.rec.Width = 12; Obj.compCollision.rec.Height = 3;
                Obj.compSprite.zOffset = -7;
            }
            else if (Type == ObjType.PotSkull)
            {
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -4;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 12;
                Obj.compSprite.zOffset = -7;
                Obj.group = ObjGroup.Liftable;
            }
            else if (
                Type == ObjType.SpikesFloor || Type == ObjType.Flamethrower ||
                Type == ObjType.Switch || Type == ObjType.Bridge)
            {
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.compCollision.blocking = false;
            }
            else if (Type == ObjType.Bumper)
            {
                Obj.compCollision.blocking = false;
            }
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
            else if (Type == ObjType.TorchUnlit || Type == ObjType.TorchLit)
            {
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -4;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 12;
                Obj.compSprite.zOffset = -7;
            }
            else if (Type == ObjType.ConveyorBelt)
            {
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.compCollision.blocking = false;
                //directions are slightly different for this obj
                if (Obj.direction == Direction.Right) { Obj.compSprite.rotation = Rotation.Clockwise270; }
                else if (Obj.direction == Direction.Left) { Obj.compSprite.rotation = Rotation.Clockwise90; }
            }

            #endregion


            #region Items

            else if (Type == ObjType.ItemRupee)
            {
                Obj.compSprite.cellSize.x = 8; //non standard cellsize
                Obj.compCollision.offsetX = -4; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 8; Obj.compCollision.rec.Height = 10;
                Obj.compCollision.blocking = false;
                Obj.group = ObjGroup.Item;

                Obj.lifetime = 255; //in frames
                Obj.compAnim.speed = 6; //in frames
                Obj.compAnim.loop = true;
            }
            else if (Type == ObjType.ItemHeart)
            {
                Obj.compSprite.cellSize.x = 8; //non standard cellsize
                Obj.compCollision.offsetX = -4; Obj.compCollision.offsetY = -3;
                Obj.compCollision.rec.Width = 8; Obj.compCollision.rec.Height = 7;
                Obj.compCollision.blocking = false;
                Obj.group = ObjGroup.Item;

                Obj.lifetime = 255; //in frames
                Obj.compAnim.speed = 6; //in frames
                Obj.compAnim.loop = true;
            }

            #endregion


            #region Projectiles

            else if (Type == ObjType.ProjectileSword)
            {
                Obj.compSprite.zOffset = 16;
                //stationary weapons share similar sprite dimensions
                SetWeaponCollisions(Obj); //collision recs + offsets are similar
                Obj.compCollision.blocking = false;
                Obj.group = ObjGroup.Projectile;
                Obj.lifetime = 18; //in frames
                Obj.compAnim.speed = 2; //in frames
                Obj.compAnim.loop = false;
            }
            else if (Type == ObjType.ProjectileFireball)
            {
                Obj.compSprite.zOffset = 16;
                //set collision rec + offset
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 10;
                Obj.compCollision.blocking = false;
                Obj.group = ObjGroup.Projectile;
                Obj.lifetime = 200; //in frames
                Obj.compAnim.speed = 5; //in frames
                Obj.compAnim.loop = true;
                Obj.compMove.speed = 1.5f; //fireballs move
            }

            #endregion


            #region Particles

            else if (Type == ObjType.ParticleDashPuff)
            {
                Obj.compSprite.cellSize.x = 8; Obj.compSprite.cellSize.y = 8; //nonstandard size
                Obj.compSprite.zOffset = -8;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 24; //in frames
                Obj.compAnim.speed = 6; //in frames
                Obj.compAnim.loop = false;
            }
            else if (Type == ObjType.ParticleExplosion)
            {
                Obj.compSprite.zOffset = 16;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 24; //in frames
                Obj.compAnim.speed = 6; //in frames
                Obj.compAnim.loop = false;
            }
            else if (Type == ObjType.ParticleSmokePuff)
            {
                Obj.compSprite.cellSize.x = 8; Obj.compSprite.cellSize.y = 8; //nonstandard size
                Obj.compSprite.zOffset = 16;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 24; //in frames
                Obj.compAnim.speed = 6; //in frames
                Obj.compAnim.loop = false;
            }
            else if (Type == ObjType.ParticleHitSparkle)
            {
                Obj.compSprite.cellSize.x = 8; Obj.compSprite.cellSize.y = 8; //nonstandard size
                Obj.compSprite.zOffset = 16;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 24; //in frames
                Obj.compAnim.speed = 6; //in frames
                Obj.compAnim.loop = true;
            }
            else if (Type == ObjType.ParticleFire)
            {
                Obj.compSprite.zOffset = 16;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 100; //in frames
                Obj.compAnim.speed = 7; //in frames
                Obj.compAnim.loop = true;
            }
            else if (Type == ObjType.ParticleReward50Gold ||
                Type == ObjType.ParticleRewardKey ||
                Type == ObjType.ParticleRewardMap ||
                Type == ObjType.ParticleRewardHeartPiece ||
                Type == ObjType.ParticleRewardHeartFull)
            {
                Obj.compSprite.zOffset = 32;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 50; //in frames
                Obj.compAnim.speed = 5; //in frames
                Obj.compAnim.loop = true;
            }

            #endregion


            //particles do not rotate like other gameObjects
            if (Obj.group == ObjGroup.Particle) { SetParticleFields(Obj); }

            ComponentFunctions.SetZdepth(Obj.compSprite);
            ComponentFunctions.UpdateCellSize(Obj.compSprite);
            ComponentFunctions.Align(Obj.compMove, Obj.compSprite, Obj.compCollision);
        }



        public static void Update(GameObject Obj)
        {
            if(Obj.lifetime > 0) //if the obj has a lifetime, count it
            {  
                Obj.lifeCounter++; //increment the life counter of the gameobject
                //if the life counter reaches the total, release this object back to the pool
                if (Obj.lifeCounter >= Obj.lifetime)
                {

                    #region Handle Object specific death states

                    if (Obj.type == ObjType.ProjectileFireball)
                    {
                        SpawnParticle(ObjType.ParticleExplosion, Obj.compSprite.position);
                        SpawnParticle(ObjType.ParticleFire, Obj.compSprite.position);
                        Assets.sfxFireballDeath.Play();
                    }

                    #endregion


                    PoolFunctions.Release(Obj);
                }
            }
        }

        public static void Draw(GameObject Obj)
        {
            DrawFunctions.Draw(Obj.compSprite);
            if (Flags.DrawCollisions)
            { DrawFunctions.Draw(Obj.compCollision); }  
        }
    }
}