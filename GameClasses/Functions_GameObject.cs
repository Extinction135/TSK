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
            Obj.canBeSaved = false; //most objects cannot be saved as XML data
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
            Obj.compAnim.timer = 0; //reset the elapsed frames
            //reset the collision component
            Obj.compCollision.blocking = true; //assume the object is blocking (most are)
            Obj.compCollision.rec.Width = 16; //assume collisionRec is 16x16
            Obj.compCollision.rec.Height = 16; //(most are)
            Obj.compCollision.offsetX = -8; //assume collisionRec offset is -8x-8
            Obj.compCollision.offsetY = -8; //(most are)
            //reset the interaction component
            Obj.compInt.active = true;
            //reset the move component
            Obj.compMove.magnitude.X = 0; //discard any previous magnitude
            Obj.compMove.magnitude.Y = 0; //
            Obj.compMove.speed = 0.0f; //assume this object doesn't move
            Obj.compMove.friction = 0.75f; //normal friction
            Obj.compMove.moveable = false; //most objects cant be moved
            Obj.compMove.grounded = true; //most objects exist on the ground
        }

        public static void SetRotation(GameObject Obj)
        {   //rotate all objects + projectiles normally for all cardinal directions
            Functions_Component.SetSpriteRotation(Obj.compSprite, Obj.direction);

            //handle object/projectile specific cases
            if (Obj.type == ObjType.ProjectileSword || Obj.type == ObjType.ProjectileNet)
            {   //some projectiles flip based on their direction
                if (Obj.direction == Direction.Down || Obj.direction == Direction.Left)
                { Obj.compSprite.flipHorizontally = true; }
            }
            else if (Obj.type == ObjType.ProjectileBomb
                || Obj.type == ObjType.BlockSpike
                || Obj.type == ObjType.ProjectileExplodingBarrel)
            {   //some objects only face Direction.Down
                Obj.compSprite.rotation = Rotation.None;
            }
            else if(Obj.type == ObjType.PitTrap)
            {   //some objects are randomly rotated
                Obj.direction = Functions_Direction.GetRandomCardinal();
            }
            else if(Obj.type == ObjType.FloorDebrisBlood)
            {   //some objects are randomly flipped horizontally
                Obj.compSprite.flipHorizontally = true;
            }
        }

        public static void Update(GameObject Obj)
        {
            if (Obj.lifetime > 0) //if the obj has a lifetime, count it
            {
                Obj.lifeCounter++; //increment the life counter of the gameobject
                //handle the obj's birth event
                if (Obj.lifeCounter == 2) { HandleBirthEvent(Obj); }
                //handle the obj's death event
                if (Obj.lifeCounter >= Obj.lifetime) { HandleDeathEvent(Obj); }
                //reset fairy obj's life (keep them around forever)
                if (Obj.type == ObjType.Fairy) { Obj.lifeCounter = 100; }
            }
            //certain objects get AI input
            if (Obj.getsAI) { Functions_Ai.HandleObj(Obj); }
        }

        public static void Kill(GameObject Obj)
        {   //the roomObj/entity 'dies' and is then released
            HandleDeathEvent(Obj);
            Functions_Pool.Release(Obj);
        }

        public static void HandleBirthEvent(GameObject Obj)
        {

            #region Projectiles

            if (Obj.type == ObjType.ProjectileArrow)
            {
                Assets.Play(Assets.sfxArrowShoot);
            }
            else if (Obj.type == ObjType.ProjectileBomb)
            {
                Assets.Play(Assets.sfxBombDrop);
                //bomb is initially sliding upon birth
                Functions_Particle.Spawn(
                    ObjType.ParticleDashPuff,
                    Obj.compSprite.position.X + 0,
                    Obj.compSprite.position.Y + 0);
            }
            else if (Obj.type == ObjType.ProjectileExplosion)
            {
                Assets.Play(Assets.sfxExplosion);
                //place smoke puff above explosion
                Functions_Particle.Spawn(
                    ObjType.ParticleSmokePuff,
                    Obj.compSprite.position.X + 4,
                    Obj.compSprite.position.Y - 8);
            }
            else if (Obj.type == ObjType.ProjectileFireball)
            {
                Assets.Play(Assets.sfxFireballCast);
                //place smoke puff centered to fireball
                Functions_Particle.Spawn(
                    ObjType.ParticleSmokePuff,
                    Obj.compSprite.position.X + 4,
                    Obj.compSprite.position.Y + 4);
            }
            else if (Obj.type == ObjType.ProjectileSword)
            {
                Assets.Play(Assets.sfxSwordSwipe);
            }
            else if (Obj.type == ObjType.ProjectileExplodingBarrel)
            {
                Assets.Play(Assets.sfxEnemyHit);
            }

            #endregion


            #region Particles

            else if (Obj.type == ObjType.ParticleRewardMap)
            { Assets.Play(Assets.sfxReward); }
            else if (Obj.type == ObjType.ParticleRewardKey)
            { Assets.Play(Assets.sfxKeyPickup); }
            else if (Obj.type == ObjType.ParticleSplash)
            { Assets.Play(Assets.sfxSplash); }

            #endregion

        }

        public static void HandleDeathEvent(GameObject Obj)
        {
            //all item pickups are handled the same
            if (Obj.group == ObjGroup.Pickup)
            {   //when an item pickup dies, display an attention particle
                Functions_Particle.Spawn(
                    ObjType.ParticleAttention,
                    Obj.compSprite.position.X - 4,
                    Obj.compSprite.position.Y - 2);
            }


            #region Projectiles

            else if (Obj.type == ObjType.ProjectileArrow)
            {
                Functions_Particle.Spawn(
                    ObjType.ParticleAttention,
                    Obj.compSprite.position.X + 0,
                    Obj.compSprite.position.Y + 0);
                Assets.Play(Assets.sfxArrowHit);
            }
            else if (Obj.type == ObjType.ProjectileBomb)
            {   //create explosion projectile
                Functions_Projectile.Spawn(
                    ObjType.ProjectileExplosion,
                    Obj.compMove, Direction.None);
            }
            else if (Obj.type == ObjType.ProjectileFireball)
            {   //explosion & ground fire
                Functions_Particle.Spawn(
                    ObjType.ParticleExplosion,
                    Obj.compSprite.position.X + 0,
                    Obj.compSprite.position.Y + 0);
                Functions_Particle.Spawn(
                    ObjType.ParticleFire,
                    Obj.compSprite.position.X + 0,
                    Obj.compSprite.position.Y + 0);
                Assets.Play(Assets.sfxFireballDeath);
            }
            //sword - no death event
            //rock debris - no death event
            else if (Obj.type == ObjType.ProjectileExplodingBarrel)
            {
                //create explosion projectile
                Functions_Projectile.Spawn(ObjType.ProjectileExplosion,
                    Obj.compMove, Direction.None);
                //create loot
                Functions_Loot.SpawnLoot(Obj.compSprite.position);
                //leave some fire behind
                Functions_Particle.Spawn(
                    ObjType.ParticleFire,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y);
                //throw some rocks around as decoration
                //Functions_Particle.ScatterRockDebris(Obj.compSprite.position, true);
                //Functions_Particle.ScatterRockDebris(Obj.compSprite.position, true);
                //Functions_Particle.ScatterRockDebris(Obj.compSprite.position, true);
            }

            #endregion


            //all objects are released upon death
            Functions_Pool.Release(Obj);
        }

        public static void SetType(GameObject Obj, ObjType Type)
        {   //Obj.direction should be set prior to this method running
            Obj.type = Type;


            #region Assign Dungeon Sheet as Default Texture

            if (Level.type == LevelType.Castle)
            { Obj.compSprite.texture = Assets.cursedCastleSheet; }
            //expand this to include all dungeon textures...
            else if (Level.type == LevelType.Shop)
            { Obj.compSprite.texture = Assets.shopSheet; }

            //in type check, we assign mainSheet for objs that require it

            #endregion


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

            else if (Type == ObjType.DoorOpen)
            {
                Obj.compSprite.zOffset = +32; //sort very high (over / in front of hero)
                Obj.group = ObjGroup.Door;
            }
            else if(Type == ObjType.DoorTrap)
            {
                Obj.compCollision.blocking = false;
                Obj.compSprite.zOffset = -32; //sort very low (behind hero)
                Obj.group = ObjGroup.Door;
            }
            else if (Type == ObjType.DoorBombable || Type == ObjType.DoorBoss || 
                Type == ObjType.DoorShut || Type == ObjType.DoorFake)
            {
                Obj.compSprite.zOffset = -32; //sort very low (behind hero)
                Obj.group = ObjGroup.Door;
            }
            else if (Type == ObjType.WallStraight || 
                Type == ObjType.WallStraightCracked ||
                Type == ObjType.WallInteriorCorner || 
                Type == ObjType.WallExteriorCorner ||
                Type == ObjType.WallPillar)
            {
                Obj.compSprite.zOffset = -32; //sort very low (behind hero)
                Obj.group = ObjGroup.Wall;
            }
            else if (Type == ObjType.WallStatue)
            {
                Obj.compSprite.zOffset = -16; //sort low, but over walls
                Obj.group = ObjGroup.Wall;
                Obj.getsAI = true; //obj gets AI
            }
            else if (Type == ObjType.WallTorch)
            {
                Obj.compSprite.zOffset = -16; //sort low, but over walls
                Obj.group = ObjGroup.Wall;
            }

            #endregion


            #region Floor Objects

            else if (Type == ObjType.BossDecal)
            {   
                Obj.compSprite.zOffset = -32; //sort low, but over floor
                Obj.compCollision.blocking = false;
            }
            else if(Type == ObjType.FloorDebrisBlood)
            {   //collision rec is smaller so more debris is left when room is cleanedUp()
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 10;
                Obj.compSprite.zOffset = -32; //sort low, but over floor
                Obj.compCollision.blocking = false;
            }

            #endregion


            //Dungeon Room Objects

            #region Pits

            else if (Type == ObjType.PitAnimated)
            {   //this pit interacts with actor
                Obj.compSprite.zOffset = -64; //sort under pit teeth
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 10;
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
                Obj.getsAI = true;
            }
            else if (Type == ObjType.PitTop || Type == ObjType.PitBottom)
            {
                Obj.compSprite.zOffset = -60; //sort above pits & pit bubbles
                Obj.compCollision.rec.Width = 16; Obj.compCollision.rec.Height = 8;
                if (Type == ObjType.PitBottom)
                {
                    Obj.compCollision.offsetY = 4;
                    Obj.compCollision.rec.Height = 4;
                }
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
            }
            else if (Type == ObjType.PitTrap)
            {   //this becomes a pit upon collision with hero
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compCollision.offsetX = -12; Obj.compCollision.offsetY = -12;
                Obj.compCollision.rec.Width = 24; Obj.compCollision.rec.Height = 24;
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
            }

            #endregion


            #region Chests

            else if (Type == ObjType.ChestKey || Type == ObjType.ChestMap || Type == ObjType.ChestEmpty)
            {
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -3;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 11;
                Obj.compSprite.zOffset = -7;
                Obj.canBeSaved = true;
                Obj.group = ObjGroup.Chest;
                Obj.compMove.moveable = true;
            }

            #endregion


            #region Blocks

            else if (Type == ObjType.BlockDark || Type == ObjType.BlockLight)
            {
                Obj.compSprite.zOffset = -7;
                Obj.canBeSaved = true;
                //lighter blocks are moveable by belts
                if (Type == ObjType.BlockLight) { Obj.compMove.moveable = true; }
            }
            else if (Type == ObjType.BlockSpike)
            {
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compSprite.zOffset = -7;
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -7;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 14;
                //set block's moving direction, based on facing direction
                Obj.compMove.direction = Obj.direction;
                Obj.canBeSaved = true;
                Obj.compMove.moveable = true;
                Obj.compMove.grounded = false; //in air
                Obj.compCollision.blocking = false;
                Obj.compMove.speed = 0.3f; //spike blocks move med
            }

            #endregion


            #region Switch Blocks & Button

            else if (Type == ObjType.SwitchBlockBtn)
            {
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -4;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 12;
                Obj.compSprite.zOffset = -7;
                Obj.canBeSaved = true;
                Obj.compMove.moveable = true;
            }
            else if (Type == ObjType.SwitchBlockDown)
            {
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
            }
            else if (Type == ObjType.SwitchBlockUp)
            {
                Obj.compCollision.offsetX = -8; Obj.compCollision.offsetY = -8;
                Obj.compCollision.rec.Width = 16; Obj.compCollision.rec.Height = 16;
                Obj.compSprite.zOffset = -7; //sort normally
                Obj.compCollision.blocking = true;
                Obj.canBeSaved = true;
            }

            #endregion


            #region Torches

            else if (Type == ObjType.TorchUnlit || Type == ObjType.TorchLit)
            {
                Obj.compCollision.offsetX = -8; Obj.compCollision.offsetY = -4;
                Obj.compCollision.rec.Width = 16; Obj.compCollision.rec.Height = 12;
                Obj.compSprite.zOffset = -7;
                Obj.canBeSaved = true;
                Obj.compMove.moveable = true;
            }

            #endregion


            #region Fairy

            else if (Type == ObjType.Fairy)
            {
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compSprite.zOffset = 8; //sort to air
                Obj.lifetime = 255; //in frames
                Obj.compAnim.speed = 6; //in frames
                Obj.compMove.moveable = true;
                Obj.compCollision.blocking = false;
                Obj.getsAI = true;
                Obj.compMove.grounded = false; //obj is flying
                Obj.compMove.friction = World.frictionAir;
                Obj.canBeSaved = true;
            }

            #endregion


            #region Other Dungeon Objects

            else if (Type == ObjType.Pot)
            {
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -4;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 12;
                Obj.compSprite.zOffset = -7;
                Obj.canBeSaved = true;
                Obj.compMove.moveable = true;
            }
            else if (Type == ObjType.Barrel)
            {
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 13;
                Obj.compSprite.zOffset = -7;
                Obj.canBeSaved = true;
                Obj.compMove.moveable = true;
            }
            else if (Type == ObjType.BossStatue)
            {
                Obj.compCollision.rec.Height = 8;
                Obj.compCollision.offsetY = -1;
                Obj.canBeSaved = true;
                Obj.compMove.moveable = true;
            }
            else if (Type == ObjType.Pillar)
            {
                Obj.compSprite.zOffset = 2;
                Obj.compCollision.rec.Width = 10;
                Obj.compCollision.offsetX = -5;
                Obj.canBeSaved = true;
            }
            else if (Type == ObjType.Flamethrower)
            {
                Obj.compSprite.zOffset = -30; //sort slightly above floor
                Obj.compCollision.blocking = true;
                Obj.getsAI = true; //obj gets AI
                Obj.canBeSaved = true;
                Obj.compMove.moveable = true;
            }
            else if (Type == ObjType.IceTile)
            {
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compCollision.offsetX = -6; Obj.compCollision.offsetY = -6;
                Obj.compCollision.rec.Width = 12; Obj.compCollision.rec.Height = 12;
                Obj.compSprite.zOffset = -30; //sort a little above floor
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
            }
            else if (Type == ObjType.Bumper)
            {
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
            }
            else if (Type == ObjType.ConveyorBeltOn || Type == ObjType.ConveyorBeltOff)
            {
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.compAnim.speed = 10; //in frames
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
            }
            else if (Type == ObjType.SpikesFloorOn || Type == ObjType.SpikesFloorOff)
            {
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 10;
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
            }
            else if (Type == ObjType.Switch || Type == ObjType.SwitchOff)
            {
                Obj.compCollision.offsetX = -4; Obj.compCollision.offsetY = -3;
                Obj.compCollision.rec.Width = 8; Obj.compCollision.rec.Height = 8;
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
                Obj.compSprite.zOffset = -32; //sort to floor
                //if (Type == ObjType.Switch) { Obj.compSprite.zOffset = 0; } //sort normally
                Obj.compMove.moveable = true;
            }
            else if (Type == ObjType.Bridge)
            {
                Obj.compSprite.zOffset = -62; //sort to floor
                Obj.compCollision.blocking = false;
                Obj.canBeSaved = true;
            }
            else if (Type == ObjType.LeverOn || Type == ObjType.LeverOff)
            {
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = 2;
                Obj.compCollision.rec.Width = 12; Obj.compCollision.rec.Height = 5;
                Obj.compSprite.zOffset = -3;
                Obj.canBeSaved = true;
                Obj.compMove.moveable = true;
            }

            #endregion


            #region Enemy Spawn Objects

            else if (Type == ObjType.SpawnEnemy1 || Type == ObjType.SpawnEnemy2 || Type == ObjType.SpawnEnemy3)
            {
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compSprite.zOffset = -32; //sort to floor
                Obj.compCollision.blocking = false;
                Obj.group = ObjGroup.EnemySpawn;
                Obj.canBeSaved = true;
            }

            #endregion


            //Non-Dungeon Room Objects

            #region Shop Objects

            else if (Type == ObjType.Bookcase1 || Type == ObjType.Bookcase2)
            {
                Obj.compCollision.offsetY = 0; Obj.compCollision.rec.Height = 8;
                Obj.compSprite.zOffset = -7;
                Obj.canBeSaved = true;
            }
            else if (Type == ObjType.TableStone)
            {
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -7;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 14;
                Obj.compSprite.zOffset = -7;
                Obj.canBeSaved = true;
            }
            //Vendors
            else if (Type == ObjType.VendorItems || Type == ObjType.VendorPotions ||
                Type == ObjType.VendorMagic || Type == ObjType.VendorWeapons ||
                Type == ObjType.VendorArmor || Type == ObjType.VendorEquipment
                || Type == ObjType.VendorPets || Type == ObjType.VendorStory)
            {
                Obj.compSprite.texture = Assets.shopSheet;
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -3;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 11;
                Obj.compSprite.zOffset = 0;
                Obj.compAnim.speed = 20; //slow animation
                Obj.group = ObjGroup.Vendor;
            }
            else if (Type == ObjType.VendorAdvertisement)
            {
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compCollision.blocking = false;
                Obj.compSprite.zOffset = 32;
                Obj.compAnim.speed = 100; //very slow animation
                Obj.group = ObjGroup.Vendor;
            }

            #endregion


            //Entities

            #region Pickups

            else if (Type == ObjType.PickupRupee || Type == ObjType.PickupHeart ||
                Type == ObjType.PickupMagic || Type == ObjType.PickupArrow ||
                Type == ObjType.PickupBomb)
            {
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compSprite.cellSize.X = 8; //non standard cellsize
                Obj.compCollision.offsetX = -8; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 8; Obj.compCollision.rec.Height = 10;
                Obj.group = ObjGroup.Pickup;
                Obj.lifetime = 255; //in frames
                Obj.compAnim.speed = 6; //in frames
                Obj.compMove.moveable = true;
            }

            #endregion


            #region Projectiles

            //items
            else if (Type == ObjType.ProjectileBomb)
            {
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compSprite.zOffset = -4; //sort to floor
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 10;
                Obj.group = ObjGroup.Projectile;
                Obj.lifetime = 100; //in frames
                Obj.compAnim.speed = 7; //in frames
                Obj.compMove.moveable = true;
            }
            else if (Type == ObjType.ProjectileBoomerang)
            {
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compSprite.zOffset = 16;
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 10;
                Obj.group = ObjGroup.Projectile;
                Obj.lifetime = 255; //in frames
                Obj.compMove.friction = 0.96f; //some air friction
                Obj.compAnim.speed = 5; //in frames
                Obj.compMove.moveable = true;
                Obj.compMove.grounded = false; //obj is airborne
            }
            
            //magic
            else if (Type == ObjType.ProjectileFireball)
            {
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compSprite.zOffset = 16;
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 10;
                Obj.group = ObjGroup.Projectile;
                Obj.lifetime = 50; //in frames
                Obj.compMove.friction = 0.984f; //some air friction
                Obj.compAnim.speed = 5; //in frames
                Obj.compMove.moveable = true;
                Obj.compMove.grounded = false; //obj is airborne
            }

            //weapons
            else if (Type == ObjType.ProjectileSword)
            {
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compSprite.zOffset = 16;
                //set collision rec based on direction
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
                Obj.group = ObjGroup.Projectile;
                Obj.lifetime = 18; //in frames
                Obj.compAnim.speed = 2; //in frames
                Obj.compAnim.loop = false;
                Obj.compMove.moveable = true;
                Obj.compMove.grounded = false; //obj is airborne
            }
            else if (Type == ObjType.ProjectileArrow)
            {
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compSprite.zOffset = 16;
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
                Obj.group = ObjGroup.Projectile;
                Obj.lifetime = 200; //in frames
                Obj.compAnim.speed = 5; //in frames
                Obj.compMove.friction = 1.0f; //no air friction
                Obj.compMove.moveable = true;
                Obj.compMove.grounded = false; //obj is airborne
            }
            else if (Type == ObjType.ProjectileNet)
            {
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compSprite.zOffset = 16;
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 10;
                Obj.group = ObjGroup.Projectile;
                Obj.lifetime = 18; //in frames
                Obj.compAnim.speed = 2; //in frames
                Obj.compAnim.loop = false;
                Obj.compMove.moveable = true;
                Obj.compMove.grounded = false; //obj is airborne
            }
            else if (Type == ObjType.ProjectileBow)
            {
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compSprite.zOffset = 32;
                Obj.group = ObjGroup.Projectile;
                Obj.lifetime = 15; //in frames
                Obj.compAnim.speed = 10; //in frames
                Obj.compAnim.loop = false;
                Obj.compMove.grounded = false; //obj is airborne
            }

            //other
            else if (Type == ObjType.ProjectileExplosion)
            {
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compSprite.zOffset = 16;
                Obj.compCollision.offsetX = -12; Obj.compCollision.offsetY = -13;
                Obj.compCollision.rec.Width = 24; Obj.compCollision.rec.Height = 26;
                Obj.group = ObjGroup.Projectile;
                Obj.lifetime = 24; //in frames
                Obj.compAnim.speed = 5; //in frames
                Obj.compAnim.loop = false;
                Obj.compMove.grounded = false; //obj is airborne
            }
            else if (Type == ObjType.ProjectileExplodingBarrel)
            {   //this should match the Barrel GameObj
                Obj.compSprite.zOffset = -7;
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 10;
                Obj.group = ObjGroup.Projectile;
                Obj.lifetime = 40; //in frames
                Obj.compAnim.speed = 7;
                Obj.compMove.moveable = true;
                Obj.compMove.grounded = true;
            }

            #endregion


            #region Particles

            //Particles - small
            else if (Type == ObjType.ParticleDashPuff)
            {
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compSprite.cellSize.X = 8; Obj.compSprite.cellSize.Y = 8; //nonstandard size
                Obj.compSprite.zOffset = -8;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 24; //in frames
                Obj.compAnim.speed = 6; //in frames
                Obj.compAnim.loop = false;
            }
            else if (Type == ObjType.ParticleSmokePuff)
            {
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compSprite.cellSize.X = 8; Obj.compSprite.cellSize.Y = 8; //nonstandard size
                Obj.compSprite.zOffset = 16;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 24; //in frames
                Obj.compAnim.speed = 5; //in frames
                Obj.compAnim.loop = false;
            }
            else if (Type == ObjType.ParticleHitSparkle)
            {
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compSprite.cellSize.X = 8; Obj.compSprite.cellSize.Y = 8; //nonstandard size
                Obj.compSprite.zOffset = 16;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 24; //in frames
                Obj.compAnim.speed = 6; //in frames
            }
            else if (Type == ObjType.ParticlePitAnimation)
            {
                Obj.compSprite.cellSize.X = 8; Obj.compSprite.cellSize.Y = 8; //nonstandard size
                Obj.compSprite.zOffset = -63; //sort over pits, under pit teeth
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 10*4*6; //speed * anim frames * loops
                Obj.compAnim.speed = 10; //in frames
            }



            //Particles - Map
            else if (Type == ObjType.ParticleMapFlag)
            {
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compSprite.cellSize.X = 8; Obj.compSprite.cellSize.Y = 4; //nonstandard size
                Obj.group = ObjGroup.Particle;
                Obj.compAnim.speed = 10; //in frames
            }
            else if (Type == ObjType.ParticleMapWave)
            {
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compSprite.cellSize.X = 8; Obj.compSprite.cellSize.Y = 4; //nonstandard size
                Obj.group = ObjGroup.Particle;
                Obj.compAnim.speed = 15; //in frames
                Obj.lifetime = (byte)(Obj.compAnim.speed * 4); //there are 4 frames of animation
                Obj.compAnim.loop = false;
            }
            else if (Type == ObjType.ParticleMapCampfire)
            {
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compSprite.cellSize.X = 8; Obj.compSprite.cellSize.Y = 8; //nonstandard size
                Obj.group = ObjGroup.Particle;
                Obj.compAnim.speed = 6; //in frames
            }


            
            //Particles - normal size
            else if (Type == ObjType.ParticleExplosion)
            {
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compSprite.zOffset = 16;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 24; //in frames
                Obj.compAnim.speed = 5; //in frames
                Obj.compAnim.loop = false;
            }
            else if (Type == ObjType.ParticleAttention)
            {
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compSprite.zOffset = 1024;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 24; //in frames
                Obj.compAnim.speed = 6; //in frames
                Obj.compAnim.loop = false;
            }
            else if (Type == ObjType.ParticleFire)
            {
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compSprite.zOffset = -8; //to ground
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 100; //in frames
                Obj.compAnim.speed = 7; //in frames
            }
            else if(Type == ObjType.ParticleSplash)
            {
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compSprite.zOffset = 1024;
                Obj.group = ObjGroup.Particle;
                Obj.compAnim.speed = 10; //in frames
                Obj.lifetime = 10 * 5; //speed * animFrames
                Obj.compAnim.loop = false;
            }
            else if (Type == ObjType.ParticleDebris)
            {
                Obj.compSprite.zOffset = -24; //sort low, but over floor
                Obj.compSprite.cellSize.X = 8;
                Obj.compSprite.cellSize.Y = 8; //nonstandard size
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 15;
                Obj.compMove.grounded = false; //in air
                Obj.compAnim.loop = false;
            }

            
            //Particles - Rewards & Bottles
            else if (
                Type == ObjType.ParticleRewardKey ||
                Type == ObjType.ParticleRewardMap ||
                Type == ObjType.ParticleBottleEmpty ||
                Type == ObjType.ParticleBottleHealth ||
                Type == ObjType.ParticleBottleMagic ||
                Type == ObjType.ParticleBottleCombo ||
                Type == ObjType.ParticleBottleFairy ||
                Type == ObjType.ParticleBottleBlob)
            {
                Obj.compSprite.texture = Assets.mainSheet;
                Obj.compSprite.zOffset = 32;
                Obj.group = ObjGroup.Particle;
                Obj.lifetime = 40; //in frames
                Obj.compMove.moveable = false;
            }

            #endregion


            //Handle Obj Group properties
            if (Obj.group == ObjGroup.Particle || Obj.group == ObjGroup.Projectile || Obj.group == ObjGroup.Pickup)
            { Obj.compCollision.blocking = false; } //these entities never block

            SetRotation(Obj);
            Functions_GameObjectAnimList.SetAnimationList(Obj); //set obj animation list based on type
            Functions_Component.UpdateCellSize(Obj.compSprite);
            Obj.compSprite.currentFrame = Obj.compAnim.currentAnimation[0]; //goto 1st anim frame
            Functions_Component.Align(Obj.compMove, Obj.compSprite, Obj.compCollision);
        }

    }
}