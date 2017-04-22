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

            Obj.objGroup = GameObject.ObjGroup.Object; //assume object is a generic object
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

        public static void ConvertDiagonalDirections(GameObject Obj)
        {   //converts objs's diagonal direction to a cardinal direction
            if (Obj.direction == Direction.UpRight) { Obj.direction = Direction.Right; }
            else if (Obj.direction == Direction.DownRight) { Obj.direction = Direction.Right; }
            else if (Obj.direction == Direction.UpLeft) { Obj.direction = Direction.Left; }
            else if (Obj.direction == Direction.DownLeft) { Obj.direction = Direction.Left; }
        }


        
        public static void SpawnProjectile(GameObject.Type Type, Vector2 Pos, Direction Direction)
        {   //a projectile always has a direction, so it inherit's actor's direction
            GameObject projectile = PoolFunctions.GetProjectile();
            projectile.type = Type;
            projectile.direction = Direction;
            AlignProjectile(projectile, Pos);
            GameObjectFunctions.SetType(projectile, projectile.type);
        }

        public static void SpawnParticle(GameObject.Type Type, Vector2 Pos)
        {   //a particle doesn't have a direction, so it doesn't need actor's direction
            GameObject particle = PoolFunctions.GetProjectile();
            particle.type = Type;
            particle.compSprite.rotation = Rotation.None;
            particle.compSprite.flipHorizontally = false;
            particle.direction = Direction.Down;
            AlignParticle(particle, Pos);
            GameObjectFunctions.SetType(particle, particle.type);
        }

        public static void SpawnLoot(Vector2 Pos)
        {   //either spawn a rupee or a heart item
            if (GetRandom.Int(0, 100) > 50)
            { GameObjectFunctions.SpawnProjectile(GameObject.Type.ItemRupee, Pos, Direction.Down); }
            else
            { GameObjectFunctions.SpawnProjectile(GameObject.Type.ItemHeart, Pos, Direction.Down); }
        }



        public static void AlignProjectile(GameObject Projectile, Vector2 Pos)
        {
            offset.X = 0; offset.Y = 0;
            GameObjectFunctions.ConvertDiagonalDirections(Projectile);
            //place the sword based on it's inherited direction
            if (Projectile.type == GameObject.Type.ProjectileSword)
            {
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
            GameObjectFunctions.ConvertDiagonalDirections(Particle);

            //center horizontally, place near actor's feet
            if (Particle.type == GameObject.Type.ParticleDashPuff) { offset.X = 4; offset.Y = 8; }

            //teleport the projectile to the position with the offset
            MovementFunctions.Teleport(Particle.compMove, Pos.X + offset.X, Pos.Y + offset.Y);
        }



        public static void SetType(GameObject Obj, GameObject.Type Type)
        {
            Obj.type = Type;
            GameObjectAnimListManager.SetAnimationList(Obj); //set obj animation list based on type
            ResetObject(Obj); //set obj fields to most common values
            SetRotation(Obj); //set the obj's sprite rotation


            #region Room Objects

            if (Type == GameObject.Type.Exit)
            {
                Obj.compSprite.cellSize.y = 16 * 3; //nonstandard size
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.objGroup = GameObject.ObjGroup.Door;
                Obj.compCollision.rec.Height = 2;
                Obj.compCollision.offsetY = 32 + 6;
                Obj.compCollision.blocking = false;
            }
            else if (
                Type == GameObject.Type.ExitPillarLeft || 
                Type == GameObject.Type.ExitPillarRight)
            {
                Obj.compSprite.cellSize.y = 16 * 3; //nonstandard size
                Obj.compCollision.rec.Height = 32 - 5;
                Obj.compCollision.offsetY = 14;
            }
            else if (Type == GameObject.Type.ExitLightFX)
            {
                Obj.compSprite.cellSize.y = 16 * 2; //nonstandard size
                Obj.compCollision.offsetY = 0;
                Obj.compSprite.zOffset = 256; //sort above everything
            }
            else if (
                Type == GameObject.Type.DoorOpen ||
                Type == GameObject.Type.DoorBombed ||
                Type == GameObject.Type.DoorTrap)
            {
                Obj.compCollision.blocking = false;
                if (Obj.direction == Direction.Down)
                { Obj.compSprite.zOffset = 4; } else { Obj.compSprite.zOffset = 16; }
                Obj.objGroup = GameObject.ObjGroup.Door;
            }
            else if (
                Type == GameObject.Type.DoorBombable ||
                Type == GameObject.Type.DoorBoss ||
                Type == GameObject.Type.DoorShut ||
                Type == GameObject.Type.DoorFake)
            {
                Obj.objGroup = GameObject.ObjGroup.Door;
            }
            else if (
                Type == GameObject.Type.WallStraight ||
                Type == GameObject.Type.WallStraightCracked ||
                Type == GameObject.Type.WallInteriorCorner ||
                Type == GameObject.Type.WallExteriorCorner ||
                Type == GameObject.Type.WallPillar ||
                Type == GameObject.Type.WallDecoration)
            {
                Obj.objGroup = GameObject.ObjGroup.Wall;
            }
            else if (Type == GameObject.Type.PitTop)
            {
                //pits dont collide with actors
                Obj.compSprite.zOffset = -32; //sort to floor
            }
            else if (Type == GameObject.Type.PitBottom)
            {
                //instead we'll use an animated liquid obj for collision checking
                //pits just sit ontop of this object as decoration
                Obj.compSprite.zOffset = -32; //sort to floor
            }
            else if (
                Type == GameObject.Type.PitTrapReady || Type == GameObject.Type.PitTrapOpening)
            {
                Obj.compCollision.offsetX = -6;
                Obj.compCollision.offsetY = -6;
                Obj.compSprite.zOffset = -32; //sort to floor
            }
            else if (Type == GameObject.Type.BossStatue)
            {
                Obj.objGroup = GameObject.ObjGroup.Draggable;
                Obj.compCollision.rec.Height = 8;
                Obj.compCollision.offsetY = -1;
            }
            else if (Type == GameObject.Type.BossDecal)
            {
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.compCollision.blocking = false;
            }
            else if (Type == GameObject.Type.Pillar)
            {
                Obj.compSprite.zOffset = 2;
            }
            else if (Type == GameObject.Type.WallTorch)
            {
                Obj.compCollision.blocking = false;
                Obj.compSprite.zOffset = 2;
            }
            else if (Type == GameObject.Type.DebrisFloor)
            {
                Obj.compCollision.blocking = false;
                Obj.compSprite.zOffset = -32;
            }

            #endregion


            #region Interactive Objects

            else if (Type == GameObject.Type.ChestGold ||
                Type == GameObject.Type.ChestKey ||
                Type == GameObject.Type.ChestMap ||
                Type == GameObject.Type.ChestEmpty)
            {
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -3;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 11;
                Obj.compSprite.zOffset = -7;
                //unopened chests get the objGroup of Chest
                if (Type != GameObject.Type.ChestEmpty)
                { Obj.objGroup = GameObject.ObjGroup.Chest; }
            }
            else if (Type == GameObject.Type.BlockDraggable)
            {
                Obj.compCollision.rec.Height = 12;
                Obj.compCollision.offsetY = -4;
                Obj.compSprite.zOffset = -7;
                Obj.objGroup = GameObject.ObjGroup.Draggable;
            }
            else if (Type == GameObject.Type.BlockDark || Type == GameObject.Type.BlockLight)
            {
                Obj.compSprite.zOffset = -7;
            }
            else if (Type == GameObject.Type.BlockSpikes)
            {
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -7;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 14;
                Obj.compSprite.zOffset = -7;
                Obj.compCollision.blocking = false;
            }
            else if (Type == GameObject.Type.Lever)
            {
                Obj.compCollision.offsetX = -6; Obj.compCollision.offsetY = 1;
                Obj.compCollision.rec.Width = 12; Obj.compCollision.rec.Height = 3;
                Obj.compSprite.zOffset = -7;
            }
            else if (Type == GameObject.Type.PotSkull)
            {
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -4;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 12;
                Obj.compSprite.zOffset = -7;
                Obj.objGroup = GameObject.ObjGroup.Liftable;
            }
            else if (
                Type == GameObject.Type.SpikesFloor ||
                Type == GameObject.Type.Flamethrower ||
                Type == GameObject.Type.Switch ||
                Type == GameObject.Type.Bridge)
            {
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.compCollision.blocking = false;
            }
            else if (Type == GameObject.Type.Bumper)
            {
                Obj.compCollision.blocking = false;
            }
            else if (Type == GameObject.Type.SwitchBlockBtn)
            {
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -4;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 12;
                Obj.compSprite.zOffset = -7;
            }
            else if (Type == GameObject.Type.SwitchBlockDown)
            {
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.compCollision.blocking = false;
            }
            else if (Type == GameObject.Type.SwitchBlockUp)
            {
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -7;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 14;
                Obj.compSprite.zOffset = -7;
            }
            else if (Type == GameObject.Type.TorchUnlit || Type == GameObject.Type.TorchLit)
            {
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -4;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 12;
                Obj.compSprite.zOffset = -7;
            }
            else if (Type == GameObject.Type.ConveyorBelt)
            {
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.compCollision.blocking = false;
                //directions are slightly different for this obj
                if (Obj.direction == Direction.Right) { Obj.compSprite.rotation = Rotation.Clockwise270; }
                else if (Obj.direction == Direction.Left) { Obj.compSprite.rotation = Rotation.Clockwise90; }
            }

            #endregion


            #region Items

            else if (Type == GameObject.Type.ItemRupee)
            {
                Obj.compSprite.cellSize.x = 8; //non standard cellsize
                Obj.compCollision.offsetX = -4; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 8; Obj.compCollision.rec.Height = 10;
                Obj.compCollision.blocking = false;
                Obj.objGroup = GameObject.ObjGroup.Item;

                Obj.lifetime = 255; //in frames
                Obj.compAnim.speed = 6; //in frames
                Obj.compAnim.loop = true;
            }
            else if (Type == GameObject.Type.ItemHeart)
            {
                Obj.compSprite.cellSize.x = 8; //non standard cellsize
                Obj.compCollision.offsetX = -4; Obj.compCollision.offsetY = -3;
                Obj.compCollision.rec.Width = 8; Obj.compCollision.rec.Height = 7;
                Obj.compCollision.blocking = false;
                Obj.objGroup = GameObject.ObjGroup.Item;

                Obj.lifetime = 255; //in frames
                Obj.compAnim.speed = 6; //in frames
                Obj.compAnim.loop = true;
            }

            #endregion


            #region Projectiles

            else if (Type == GameObject.Type.ProjectileSword)
            {
                Obj.compSprite.zOffset = 16;
                //stationary weapons share similar sprite dimensions
                SetWeaponCollisions(Obj); //collision recs + offsets are similar
                Obj.compCollision.blocking = false;
                Obj.objGroup = GameObject.ObjGroup.Projectile;
                Obj.lifetime = 18; //in frames
                Obj.compAnim.speed = 2; //in frames
                Obj.compAnim.loop = false;
            }

            #endregion


            #region Particles

            else if (Type == GameObject.Type.ParticleDashPuff)
            {
                Obj.compSprite.cellSize.x = 8; Obj.compSprite.cellSize.y = 8; //nonstandard size
                Obj.compSprite.zOffset = -8;
                Obj.objGroup = GameObject.ObjGroup.Particle;
                Obj.lifetime = 24; //in frames
                Obj.compAnim.speed = 6; //in frames
                Obj.compAnim.loop = false;
            }
            else if (Type == GameObject.Type.ParticleExplosion)
            {
                Obj.compSprite.zOffset = 16;
                Obj.objGroup = GameObject.ObjGroup.Particle;
                Obj.lifetime = 24; //in frames
                Obj.compAnim.speed = 6; //in frames
                Obj.compAnim.loop = false;
            }
            else if (Type == GameObject.Type.ParticleSmokePuff)
            {
                Obj.compSprite.cellSize.x = 8; Obj.compSprite.cellSize.y = 8; //nonstandard size
                Obj.compSprite.zOffset = 16;
                Obj.objGroup = GameObject.ObjGroup.Particle;
                Obj.lifetime = 24; //in frames
                Obj.compAnim.speed = 6; //in frames
                Obj.compAnim.loop = false;
            }
            else if (Type == GameObject.Type.ParticleHitSparkle)
            {
                Obj.compSprite.cellSize.x = 8; Obj.compSprite.cellSize.y = 8; //nonstandard size
                Obj.compSprite.zOffset = 16;
                Obj.objGroup = GameObject.ObjGroup.Particle;
                Obj.lifetime = 24; //in frames
                Obj.compAnim.speed = 6; //in frames
                Obj.compAnim.loop = true;
            }
            else if (Type == GameObject.Type.ParticleReward50Gold ||
                Type == GameObject.Type.ParticleRewardKey ||
                Type == GameObject.Type.ParticleRewardMap)
            {
                Obj.compSprite.zOffset = 32;
                Obj.objGroup = GameObject.ObjGroup.Particle;
                Obj.lifetime = 50; //in frames
                Obj.compAnim.speed = 5; //in frames
                Obj.compAnim.loop = true;
            }

            #endregion


            //particles do not rotate like other gameObjects
            if (Obj.objGroup == GameObject.ObjGroup.Particle) { SetParticleFields(Obj); }

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
                if (Obj.lifeCounter >= Obj.lifetime) { PoolFunctions.Release(Obj); }
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