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
    public static class Functions_RoomObject
    {
        static int i;
        public static GameObject objRef;

        public static GameObject SpawnRoomObj(ObjType Type, float X, float Y, Direction Direction)
        {   //spawns RoomObject at the X, Y location, with direction
            GameObject obj = Functions_Pool.GetRoomObj();
            //set direction
            obj.direction = Direction;
            obj.compMove.direction = Direction;
            //teleport the projectile to the proper location
            Functions_Movement.Teleport(obj.compMove, X, Y);
            //set the type, rotation, cellsize, & alignment
            Functions_GameObject.SetType(obj, Type);
            return obj;
        }

        public static void ActivateLeverObjects()
        {
            Assets.Play(Assets.sfxSwitch);
            for (i = 0; i < Pool.roomObjCount; i++)
            {   //loop thru all active roomObjects
                if (Pool.roomObjPool[i].active)
                {   //sync all lever objects
                    if (Pool.roomObjPool[i].type == ObjType.LeverOff)
                    { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.LeverOn); }
                    else if (Pool.roomObjPool[i].type == ObjType.LeverOn)
                    { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.LeverOff); }
                    //find any spikeFloor objects in the room, toggle them
                    else if (Pool.roomObjPool[i].type == ObjType.SpikesFloorOn)
                    { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.SpikesFloorOff); }
                    else if (Pool.roomObjPool[i].type == ObjType.SpikesFloorOff)
                    { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.SpikesFloorOn); }
                    //locate and toggle conveyor belt objects
                    else if (Pool.roomObjPool[i].type == ObjType.ConveyorBeltOn)
                    { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.ConveyorBeltOff); }
                    else if (Pool.roomObjPool[i].type == ObjType.ConveyorBeltOff)
                    { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.ConveyorBeltOn); }
                }
            }
        }

        public static void FlipSwitchBlocks(Vector2 BtnPos)
        {
            Assets.Play(Assets.sfxSwitch);
            Functions_Entity.SpawnEntity(ObjType.ParticleAttention, BtnPos.X, BtnPos.Y, Direction.None);
            for (i = 0; i < Pool.roomObjCount; i++)
            {   //loop thru all active roomObjects
                if (Pool.roomObjPool[i].active)
                {   //flip blocks up or down
                    if (Pool.roomObjPool[i].type == ObjType.SwitchBlockDown)
                    { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.SwitchBlockUp); }
                    else if (Pool.roomObjPool[i].type == ObjType.SwitchBlockUp)
                    { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.SwitchBlockDown); }
                    //display particle fx at block location
                    if (Pool.roomObjPool[i].type == ObjType.SwitchBlockDown
                        || Pool.roomObjPool[i].type == ObjType.SwitchBlockUp)
                    {   
                        Functions_Entity.SpawnEntity(ObjType.ParticleAttention,
                            Pool.roomObjPool[i].compSprite.position.X,
                            Pool.roomObjPool[i].compSprite.position.Y,
                            Direction.Down);
                    }
                }
            }
        }

        public static void OpenTrapDoors()
        {
            Assets.Play(Assets.sfxSwitch);
            for (i = 0; i < Pool.roomObjCount; i++)
            {   //loop thru all active roomObjects
                if (Pool.roomObjPool[i].active)
                {   //convert trap doors to open doors
                    if (Pool.roomObjPool[i].type == ObjType.DoorTrap)
                    {   //display an attention particle where the conversion happened
                        Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.DoorOpen);
                        Functions_Entity.SpawnEntity(ObjType.ParticleAttention,
                            Pool.roomObjPool[i].compSprite.position.X,
                            Pool.roomObjPool[i].compSprite.position.Y,
                            Direction.Down);
                    }
                }
            }
        }

        public static void CloseDoors()
        {
            Assets.Play(Assets.sfxSwitch);
            for (i = 0; i < Pool.roomObjCount; i++)
            {   //loop thru all active roomObjects
                if (Pool.roomObjPool[i].active)
                {   //convert trap doors to open doors
                    if (Pool.roomObjPool[i].type == ObjType.DoorOpen)
                    {   //all open doors inside room become trap doors (push hero + close)
                        Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.DoorTrap);
                    }
                }
            }
        }





        //decorates a door on left/right or top/bottom
        static Vector2 posA = new Vector2();
        static Vector2 posB = new Vector2();
        public static void DecorateDoor(GameObject Door, ObjType Type)
        {
            if (Door.direction == Direction.Up || Door.direction == Direction.Down)
            {   //build left/right decorations if Door.direction is Up or Down
                posA.X = Door.compSprite.position.X - 16;
                posA.Y = Door.compSprite.position.Y;
                posB.X = Door.compSprite.position.X + 16;
                posB.Y = Door.compSprite.position.Y;
            }
            else
            {   //build top/bottom decorations if Door.direction is Left or Right
                posA.X = Door.compSprite.position.X;
                posA.Y = Door.compSprite.position.Y - 16;
                posB.X = Door.compSprite.position.X;
                posB.Y = Door.compSprite.position.Y + 16;
            }
            //build wall decorationA torch/pillar/decoration
            SpawnRoomObj(Type, posA.X, posA.Y, Door.direction);
            //build wall decorationB torch/pillar/decoration
            SpawnRoomObj(Type, posB.X, posB.Y, Door.direction);
        }

        public static void CreateVendor(ObjType VendorType, Vector2 Position)
        {
            //place vendor
            SpawnRoomObj(VendorType, Position.X, Position.Y, Direction.Down);
            //place stone table
            SpawnRoomObj(ObjType.TableStone, Position.X + 16, Position.Y, Direction.Down);
            //spawn vendor advertisement
            objRef = SpawnRoomObj(ObjType.VendorAdvertisement,
                Position.X + 16, Position.Y - 6, Direction.Down);
            objRef.compAnim.currentAnimation = new List<Byte4>();


            #region Display the vendor's wares for sale

            if (VendorType == ObjType.VendorItems)
            {   //add all the items from the main sheet
                objRef.compAnim.currentAnimation.Add(new Byte4(5, 5, 0, 0)); //heart
                objRef.compAnim.currentAnimation.Add(new Byte4(5, 6, 0, 0)); //bomb
                objRef.compAnim.currentAnimation.Add(new Byte4(5, 7, 0, 0)); //bombs
                objRef.compAnim.currentAnimation.Add(new Byte4(5, 8, 0, 0)); //arrows
            }
            else if (VendorType == ObjType.VendorPotions)
            {   //add all the potions from the main sheet
                objRef.compAnim.currentAnimation.Add(new Byte4(6, 6, 0, 0)); //health
                objRef.compAnim.currentAnimation.Add(new Byte4(6, 7, 0, 0)); //magic
                objRef.compAnim.currentAnimation.Add(new Byte4(6, 8, 0, 0)); //combo
            }
            else if (VendorType == ObjType.VendorMagic)
            {   //add all the magic medallions from the main sheet
                objRef.compAnim.currentAnimation.Add(new Byte4(7, 5, 0, 0)); //fireball
            }
            else if (VendorType == ObjType.VendorWeapons)
            {   //add all the weapons from the main sheet
                //objRef.compAnim.currentAnimation.Add(new Byte4(8, 5, 0, 0)); //sword
                objRef.compAnim.currentAnimation.Add(new Byte4(8, 6, 0, 0)); //bow
                objRef.compAnim.currentAnimation.Add(new Byte4(8, 7, 0, 0)); //net
                //objRef.compAnim.currentAnimation.Add(new Byte4(8, 8, 0, 0)); //axe
                //objRef.compAnim.currentAnimation.Add(new Byte4(8, 9, 0, 0)); //net
            }
            else if (VendorType == ObjType.VendorArmor)
            {   //add all the armor from the main sheet
                objRef.compAnim.currentAnimation.Add(new Byte4(9, 5, 0, 0)); //shirt
                objRef.compAnim.currentAnimation.Add(new Byte4(9, 6, 0, 0)); //plate mail
                objRef.compAnim.currentAnimation.Add(new Byte4(9, 7, 0, 0)); //cape
                objRef.compAnim.currentAnimation.Add(new Byte4(9, 8, 0, 0)); //robe
            }
            else if (VendorType == ObjType.VendorEquipment)
            {   //add all the equipment from the main sheet
                objRef.compAnim.currentAnimation.Add(new Byte4(10, 5, 0, 0)); //ring
                //objRef.compAnim.currentAnimation.Add(new Byte4(10, 6, 0, 0)); //pearl
                //objRef.compAnim.currentAnimation.Add(new Byte4(10, 7, 0, 0)); //necklace
                //objRef.compAnim.currentAnimation.Add(new Byte4(10, 8, 0, 0)); //glove
                //objRef.compAnim.currentAnimation.Add(new Byte4(10, 9, 0, 0)); //pin
            }
            else if (VendorType == ObjType.VendorPets)
            {   //add all the pets from the main sheet
                objRef.compAnim.currentAnimation.Add(new Byte4(11, 5, 0, 0)); //stinky dog
                objRef.compAnim.currentAnimation.Add(new Byte4(11, 6, 0, 0)); //chicken
            }

            #endregion

        }

        public static void DestroyObject(GameObject RoomObj, Boolean releaseObj, Boolean spawnLoot)
        {   //grab players attention, spawn rock debris, play shatter sound
            Functions_Entity.SpawnEntity(ObjType.ParticleAttention,
                RoomObj.compSprite.position.X,
                RoomObj.compSprite.position.Y,
                Direction.Down);
            Functions_Entity.ScatterRockDebris(RoomObj.compSprite.position, true);
            Functions_Entity.ScatterRockDebris(RoomObj.compSprite.position, true);
            Functions_Entity.ScatterRockDebris(RoomObj.compSprite.position, true);
            Assets.Play(Assets.sfxShatter);
            //handle parameter values
            if (spawnLoot) { Functions_Loot.SpawnLoot(RoomObj.compSprite.position); }
            if (releaseObj) { Functions_Pool.Release(RoomObj); }
        }

        public static void CollapseDungeonDoor(GameObject Door, GameObject Projectile)
        {   //blow up door, change to doorOpen
            DestroyObject(Door, false, false);
            Functions_GameObject.SetType(Door, ObjType.DoorOpen);
            //update the dungeon.doors list, change colliding door to bombed
            for (int i = 0; i < Level.doors.Count; i++)
            {   //if this explosion collides with any dungeon.door that is of type.bombable
                if (Level.doors[i].type == DoorType.Bombable)
                {   //change this door type to type.bombed
                    if (Projectile.compCollision.rec.Intersects(Level.doors[i].rec))
                    { Level.doors[i].type = DoorType.Open; }
                }
            }
        }

    }
}