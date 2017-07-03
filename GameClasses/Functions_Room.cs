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
    public static class Functions_Room
    {

        public static Stopwatch stopWatch = new Stopwatch();
        public static TimeSpan time;
        public static int i;
        public static int j;
        public static ComponentSprite floorRef;
        public static GameObject objRef;
        public static Actor actorRef;
        public static Point pos;



        public static void BuildRoom(Room Room)
        {
            stopWatch.Reset(); stopWatch.Start();

            //reset the pools + counter
            Functions_Pool.Reset();
            Pool.counter = 0;
            //shorten the room's position reference
            pos = Room.collision.rec.Location;


            #region Build the room

            for (i = 0; i < Room.size.X; i++)
            {
                for (j = 0; j < Room.size.Y; j++)
                {
                    //place the floors
                    floorRef = Functions_Pool.GetFloor();
                    floorRef.position.X = i * 16 + pos.X + 8;
                    floorRef.position.Y = j * 16 + pos.Y + 8;


                    #region Top Row Walls

                    if (j == 0)
                    {
                        //top row
                        objRef = Functions_Pool.GetRoomObj();
                        Functions_Movement.Teleport(objRef.compMove,
                            i * 16 + pos.X + 8,
                            0 * 16 - 16 + pos.Y + 8);
                        objRef.direction = Direction.Down;
                        Functions_GameObject.SetType(objRef, ObjType.WallStraight);

                        if (i == 0)
                        {   //topleft corner
                            objRef = Functions_Pool.GetRoomObj();
                            Functions_Movement.Teleport(objRef.compMove,
                                -16 + pos.X + 8,
                                -16 + pos.Y + 8);
                            objRef.direction = Direction.Down;
                            Functions_GameObject.SetType(objRef, ObjType.WallInteriorCorner);
                        }
                        else if (i == Room.size.X - 1)
                        {   //topright corner
                            objRef = Functions_Pool.GetRoomObj();
                            Functions_Movement.Teleport(objRef.compMove,
                                Room.size.X * 16 + pos.X + 8,
                                -16 + pos.Y + 8);
                            objRef.direction = Direction.Left;
                            Functions_GameObject.SetType(objRef, ObjType.WallInteriorCorner);
                        }
                    }

                    #endregion


                    #region Bottom Row Walls

                    else if (j == Room.size.Y - 1)
                    {
                        //bottom row
                        objRef = Functions_Pool.GetRoomObj();
                        Functions_Movement.Teleport(objRef.compMove,
                            i * 16 + pos.X + 8,
                            Room.size.Y * 16 + pos.Y + 8);
                        objRef.direction = Direction.Up;
                        Functions_GameObject.SetType(objRef, ObjType.WallStraight);

                        if (i == 0)
                        {   //bottom left corner
                            objRef = Functions_Pool.GetRoomObj();
                            Functions_Movement.Teleport(objRef.compMove,
                                -16 + pos.X + 8,
                                Room.size.Y * 16 + pos.Y + 8);
                            objRef.direction = Direction.Right;
                            Functions_GameObject.SetType(objRef, ObjType.WallInteriorCorner);
                        }
                        else if (i == Room.size.X - 1)
                        {   //bottom right corner
                            objRef = Functions_Pool.GetRoomObj();
                            Functions_Movement.Teleport(objRef.compMove,
                                Room.size.X * 16 + pos.X + 8,
                                Room.size.Y * 16 + pos.Y + 8);
                            objRef.direction = Direction.Up;
                            Functions_GameObject.SetType(objRef, ObjType.WallInteriorCorner);
                        }
                    }

                    #endregion


                    #region Left & Right Column Walls

                    if (i == 0)
                    {   //left side
                        objRef = Functions_Pool.GetRoomObj();
                        Functions_Movement.Teleport(objRef.compMove,
                            i * 16 - 16 + pos.X + 8,
                            j * 16 + pos.Y + 8);
                        objRef.direction = Direction.Right;
                        Functions_GameObject.SetType(objRef, ObjType.WallStraight);
                    }
                    else if (i == Room.size.X - 1)
                    {   //right side
                        objRef = Functions_Pool.GetRoomObj();
                        Functions_Movement.Teleport(objRef.compMove,
                            i * 16 + 16 + pos.X + 8,
                            j * 16 + pos.Y + 8);
                        objRef.direction = Direction.Left;
                        Functions_GameObject.SetType(objRef, ObjType.WallStraight);
                    }

                    #endregion

                }
            }

            #endregion


            SetDoors(Room); //set the room's doors
            SetFloors(Room); //set the floortile frames based on room.type

            //pass the room to the appropriate method for completion
            if (Room.type == RoomType.Exit) { FinishExitRoom(Room); }
            else if (Room.type == RoomType.Hub) { FinishHubRoom(Room); }
            else if (Room.type == RoomType.Boss) { FinishBossRoom(Room); }
            else if (Room.type == RoomType.Shop) { FinishShopRoom(Room); }

            CleanupRoom(Room); //remove overlapping objs
            
            //update the object pool, since we teleported objects around
            Functions_Pool.UpdateRoomObjPool();
            Assets.Play(Assets.sfxDoorOpen); //play the door sound

            stopWatch.Stop(); time = stopWatch.Elapsed;
            DebugInfo.roomTime = time.Ticks;
        }



        public static void MoveRoom(Room Room, int X, int Y)
        {
            Room.collision.rec.X = X;
            Room.collision.rec.Y = Y;
            Room.center.X = X + (Room.size.X / 2) * 16;
            Room.center.Y = Y + (Room.size.Y / 2) * 16;
        }

        public static void SetFloors(Room Room)
        {
            for (i = 0; i < Pool.floorCount; i++)
            {   //set all the floor sprite's current frame based on the room.type
                Pool.floorPool[i].currentFrame.X = 6;
                if (Room.type == RoomType.Shop) { Pool.floorPool[i].currentFrame.Y = 0; }
                else if (Room.type == RoomType.Exit) { Pool.floorPool[i].currentFrame.Y = 0; }
                else if (Room.type == RoomType.Boss) { Pool.floorPool[i].currentFrame.Y = 1; }
                else { Pool.floorPool[i].currentFrame.Y = 0; }
            }
        }

        public static void SpawnEnemies(Room Room)
        {
            if (Flags.SpawnMobs)
            {
                //place enemies within the room
                for (i = 0; i < 10; i++)
                {
                    actorRef = Functions_Pool.GetActor();
                    //we SHOULD be checking to see if actorRef is null..
                    //but because we reset the pool earlier in this function,
                    //and the room's enemy count will never be larger than the total actors
                    //we'll never get a null result from GetActor() right here
                    Functions_Actor.SetType(actorRef, ActorType.Blob);
                    //get a random value between the min/max size of room
                    int randomX = Functions_Random.Int(-Room.size.X + 2, Room.size.X - 2);
                    int randomY = Functions_Random.Int(-Room.size.Y + 2, Room.size.Y - 2);
                    //divide random value in half
                    randomX = randomX / 2;
                    randomY = randomY / 2;
                    //ensure this value isn't 0
                    if (randomX == 0) { randomX = 1; }
                    if (randomY == 0) { randomY = 1; }
                    //teleport actor to center of room, apply random offset
                    Functions_Movement.Teleport(actorRef.compMove,
                        Room.center.X + 16 * randomX + 8,
                        Room.center.Y + 16 * randomY + 8);
                }
            }
        }

        public static void CleanupRoom(Room Room)
        {
            Boolean checkObjB; //remove certain overlapping objects
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                checkObjB = false; //check to see if ObjA can delete ObjB
                if (Pool.roomObjPool[i].active) //if ObjA is active..
                {   //certain objects delete other objects, if they collide
                    if (Pool.roomObjPool[i].group == ObjGroup.Door 
                        || Pool.roomObjPool[i].type == ObjType.WallStatue
                        || Pool.roomObjPool[i].type == ObjType.WallPillar
                        )
                    { checkObjB = true; }
                }

                if (checkObjB) //ObjA can delete ObjB
                {
                    for (j = 0; j < Pool.roomObjCount; j++)
                    {   //if ObjB is active, and not the same object (dont compare objects with themselves)
                        if (Pool.roomObjPool[j].active && Pool.roomObjPool[i] != Pool.roomObjPool[j])
                        {   //check that the obj is a wall
                            if (Pool.roomObjPool[j].group == ObjGroup.Wall)
                            {   //walls that intersect ObjA get released (deleted)
                                if (Pool.roomObjPool[i].compCollision.rec.Intersects(Pool.roomObjPool[j].compCollision.rec))
                                { Functions_Pool.Release(Pool.roomObjPool[j]); }
                            }
                        }
                    }
                }
            }
        }

        public static void SetDoors(Room Room)
        {
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active)
                {   
                    if (Pool.roomObjPool[i].group == ObjGroup.Wall)
                    {   //check to see if wall collides with any doorPos point from dungeon
                        for (int g = 0; g < Functions_Dungeon.dungeon.doorLocations.Count; g++)
                        {
                            if(Pool.roomObjPool[i].compCollision.rec.Contains(Functions_Dungeon.dungeon.doorLocations[g]))
                            {
                                //if current room is boss room, then door is trap door
                                if (Room.type == RoomType.Boss)
                                {
                                    Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.DoorTrap);
                                    DecorateDoor(Pool.roomObjPool[i], ObjType.WallPillar);
                                }
                                //if room is hub, and the doorLocation is 0 (boss door), then door is boss door
                                else if (Room.type == RoomType.Hub && g == 0)
                                {
                                    Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.DoorBoss);
                                    DecorateDoor(Pool.roomObjPool[i], ObjType.WallPillar);

                                    //build the boss welcome mat (left)
                                    objRef = Functions_Pool.GetRoomObj();
                                    Functions_Movement.Teleport(objRef.compMove,
                                        Pool.roomObjPool[i].compSprite.position.X - 8,
                                        Pool.roomObjPool[i].compSprite.position.Y + 16);
                                    objRef.direction = Direction.Down;
                                    Functions_GameObject.SetType(objRef, ObjType.BossDecal);
                                    //build the boss welcome mat (right)
                                    objRef = Functions_Pool.GetRoomObj();
                                    Functions_Movement.Teleport(objRef.compMove,
                                        Pool.roomObjPool[i].compSprite.position.X + 8,
                                        Pool.roomObjPool[i].compSprite.position.Y + 16);
                                    objRef.direction = Direction.Down;
                                    objRef.compSprite.flipHorizontally = true;
                                    Functions_GameObject.SetType(objRef, ObjType.BossDecal);
                                }
                                //all other rooms simply have open doors
                                else
                                {
                                    Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.DoorOpen);
                                    DecorateDoor(Pool.roomObjPool[i], ObjType.WallTorch);
                                }

                                //sort the object that became a door
                                Functions_Component.SetZdepth(Pool.roomObjPool[i].compSprite);

                                //place a floor tile underneath this door
                                floorRef = Functions_Pool.GetFloor();
                                floorRef.position = Pool.roomObjPool[i].compSprite.position;
                            }
                        }
                    }
                }
            }
        }

        public static void CreateVendor(ObjType VendorType, Vector2 Position)
        {
            //place vendor
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                Position.X, Position.Y);
            Functions_GameObject.SetType(objRef, VendorType);

            //place stone table
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                Position.X + 16, Position.Y);
            Functions_GameObject.SetType(objRef, ObjType.SwitchBlockUp);

            //place vendor advertisement
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                Position.X + 16, Position.Y - 6);
            Functions_GameObject.SetType(objRef, ObjType.VendorAdvertisement);
            objRef.compAnim.currentAnimation = new List<Byte4>();


            #region Display the vendor's wares for sale

            if (VendorType == ObjType.VendorItems)
            {   //add all the items from the main sheet
                //objRef.compAnim.currentAnimation.Add(new Byte4(5, 5, 0, 0)); //boomerang
                objRef.compAnim.currentAnimation.Add(new Byte4(5, 6, 0, 0)); //bomb
                objRef.compAnim.currentAnimation.Add(new Byte4(5, 7, 0, 0)); //bombs
                objRef.compAnim.currentAnimation.Add(new Byte4(5, 8, 0, 0)); //arrows
            }
            else if (VendorType == ObjType.VendorPotions)
            {   //add all the potions from the main sheet
                //objRef.compAnim.currentAnimation.Add(new Byte4(6, 5, 0, 0)); //empty bottle
                objRef.compAnim.currentAnimation.Add(new Byte4(6, 6, 0, 0)); //health
                objRef.compAnim.currentAnimation.Add(new Byte4(6, 7, 0, 0)); //magic
                //objRef.compAnim.currentAnimation.Add(new Byte4(6, 8, 0, 0)); //mix
                objRef.compAnim.currentAnimation.Add(new Byte4(6, 9, 0, 0)); //fairy
            }
            else if (VendorType == ObjType.VendorMagic)
            {   //add all the magic medallions from the main sheet
                //objRef.compAnim.currentAnimation.Add(new Byte4(7, 5, 0, 0)); //portal
                objRef.compAnim.currentAnimation.Add(new Byte4(7, 6, 0, 0)); //fireball
                //objRef.compAnim.currentAnimation.Add(new Byte4(7, 7, 0, 0)); //lightning
                //objRef.compAnim.currentAnimation.Add(new Byte4(7, 8, 0, 0)); //quake
                //objRef.compAnim.currentAnimation.Add(new Byte4(7, 9, 0, 0)); //summon
            }
            else if (VendorType == ObjType.VendorWeapons)
            {   //add all the weapons from the main sheet
                //objRef.compAnim.currentAnimation.Add(new Byte4(8, 5, 0, 0)); //sword
                objRef.compAnim.currentAnimation.Add(new Byte4(8, 6, 0, 0)); //bow
                //objRef.compAnim.currentAnimation.Add(new Byte4(8, 7, 0, 0)); //staff
                //objRef.compAnim.currentAnimation.Add(new Byte4(8, 8, 0, 0)); //axe
                //objRef.compAnim.currentAnimation.Add(new Byte4(8, 9, 0, 0)); //net
            }
            else if (VendorType == ObjType.VendorArmor)
            {   //add all the armor from the main sheet
                objRef.compAnim.currentAnimation.Add(new Byte4(9, 5, 0, 0)); //shirt
                objRef.compAnim.currentAnimation.Add(new Byte4(9, 6, 0, 0)); //plate mail
                objRef.compAnim.currentAnimation.Add(new Byte4(9, 7, 0, 0)); //cape
                objRef.compAnim.currentAnimation.Add(new Byte4(9, 8, 0, 0)); //robe
                //objRef.compAnim.currentAnimation.Add(new Byte4(9, 9, 0, 0)); //robe2
            }
            else if (VendorType == ObjType.VendorEquipment)
            {   //add all the equipment from the main sheet
                objRef.compAnim.currentAnimation.Add(new Byte4(10, 5, 0, 0)); //ring
                objRef.compAnim.currentAnimation.Add(new Byte4(10, 6, 0, 0)); //pearl
                objRef.compAnim.currentAnimation.Add(new Byte4(10, 7, 0, 0)); //necklace
                objRef.compAnim.currentAnimation.Add(new Byte4(10, 8, 0, 0)); //glove
                objRef.compAnim.currentAnimation.Add(new Byte4(10, 9, 0, 0)); //pin
            }

            #endregion


        }

        public static void PlaceExit(Room Room)
        {
            //create the exit
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                (Room.size.X / 2) * 16 + pos.X + 8,
                Room.size.Y * 16 + pos.Y + 8 - 16 * 2);
            Functions_GameObject.SetType(objRef, ObjType.Exit);

            //place the exit light fx over exit obj
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                (Room.size.X / 2) * 16 + pos.X + 8,
                Room.size.Y * 16 + pos.Y + 8 - 16 * 1);
            Functions_GameObject.SetType(objRef, ObjType.ExitLightFX);

            //create exit pillars
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                (Room.size.X / 2) * 16 + pos.X + 8 - 16,
                Room.size.Y * 16 + pos.Y + 8 - 16 * 2);
            Functions_GameObject.SetType(objRef, ObjType.ExitPillarLeft);
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                (Room.size.X / 2) * 16 + pos.X + 8 + 16,
                Room.size.Y * 16 + pos.Y + 8 - 16 * 2);
            Functions_GameObject.SetType(objRef, ObjType.ExitPillarRight);
        }

        public static void DecorateDoor(GameObject Door, ObjType Type)
        {   //build left wall torch/pillar/decoration
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                Door.compSprite.position.X - 16,
                Door.compSprite.position.Y);
            objRef.direction = Door.direction;
            Functions_GameObject.SetType(objRef, Type);
            //build right wall torch/pillar/decoration
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                Door.compSprite.position.X + 16,
                Door.compSprite.position.Y);
            objRef.direction = Door.direction;
            Functions_GameObject.SetType(objRef, Type);
        }



        public static void FinishExitRoom(Room Room)
        {
            PlaceExit(Room);


            //place decorative statues
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                3 * 16 + pos.X + 8, 2 * 16 + pos.Y + 8);
            Functions_GameObject.SetType(objRef, ObjType.BossStatue);
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                7 * 16 + pos.X + 8, 2 * 16 + pos.Y + 8);
            Functions_GameObject.SetType(objRef, ObjType.BossStatue);

            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                3 * 16 + pos.X + 8, 4 * 16 + pos.Y + 8);
            Functions_GameObject.SetType(objRef, ObjType.BossStatue);
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                7 * 16 + pos.X + 8, 4 * 16 + pos.Y + 8);
            Functions_GameObject.SetType(objRef, ObjType.BossStatue);

            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                3 * 16 + pos.X + 8, 6 * 16 + pos.Y + 8);
            Functions_GameObject.SetType(objRef, ObjType.BossStatue);
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                7 * 16 + pos.X + 8, 6 * 16 + pos.Y + 8);
            Functions_GameObject.SetType(objRef, ObjType.BossStatue);
        }

        public static void FinishHubRoom(Room Room)
        {

            #region Create the Testing Chests

            //place chest gameObj in bottom right corner
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                (Room.size.X - 1) * 16 + pos.X + 8,
                1 * 16 + pos.Y + 8);
            objRef.direction = Direction.Down;
            Functions_GameObject.SetType(objRef, ObjType.ChestGold);

            //create a big key chest
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                (Room.size.X - 1) * 16 + pos.X + 8,
                3 * 16 + pos.Y + 8);
            objRef.direction = Direction.Down;
            Functions_GameObject.SetType(objRef, ObjType.ChestKey);

            //create a map chest
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                (Room.size.X - 1) * 16 + pos.X + 8,
                5 * 16 + pos.Y + 8);
            objRef.direction = Direction.Down;
            Functions_GameObject.SetType(objRef, ObjType.ChestMap);

            //create a heart piece chest
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                (Room.size.X - 1) * 16 + pos.X + 8,
                7 * 16 + pos.Y + 8);
            objRef.direction = Direction.Down;
            Functions_GameObject.SetType(objRef, ObjType.ChestHeartPiece);

            #endregion


            #region Place Test Objects

            /*
            //place skeleton pots along left wall
            for (i = 0; i < Room.size.Y; i++)
            {
                objRef = Functions_Pool.GetRoomObj();
                Functions_Movement.Teleport(objRef.compMove,
                    0 * 16 + pos.X + 8,
                    i * 16 + pos.Y + 8);
                objRef.direction = Direction.Down;
                Functions_GameObject.SetType(objRef, ObjType.PotSkull);
            }
            */

            //Create testing spike blocks
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                7 * 16 + pos.X + 8,
                3 * 16 + pos.Y + 8);
            Functions_GameObject.SetType(objRef, ObjType.BlockSpikes);

            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                7 * 16 + pos.X + 8,
                7 * 16 + pos.Y + 8);
            objRef.compMove.direction = Direction.Right;
            Functions_GameObject.SetType(objRef, ObjType.BlockSpikes);

            //place conveyor belt
            for (i = 0; i < Room.size.Y; i++)
            {
                objRef = Functions_Pool.GetRoomObj();
                Functions_Movement.Teleport(objRef.compMove,
                    15 * 16 + pos.X + 8,
                    i * 16 + pos.Y + 8);
                objRef.direction = Direction.Down;
                Functions_GameObject.SetType(objRef, ObjType.ConveyorBelt);
            }

            //place bumper
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                13 * 16 + pos.X + 8,
                3 * 16 + pos.Y + 8);
            Functions_GameObject.SetType(objRef, ObjType.Bumper);

            //place flamethrower
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                13 * 16 + pos.X + 8,
                5 * 16 + pos.Y + 8);
            Functions_GameObject.SetType(objRef, ObjType.Flamethrower);

            #endregion


            #region Create Wall Statues

            //place test wall statue object - top wall
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                8 * 16 + pos.X + 8,
                0 * 16 - 16 + pos.Y + 8);
            objRef.direction = Direction.Down;
            Functions_GameObject.SetType(objRef, ObjType.WallStatue);

            //place test wall statue object - left wall
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                -1 * 16 + pos.X + 8,
                5 * 16 - 16 + pos.Y + 8);
            objRef.direction = Direction.Right;
            Functions_GameObject.SetType(objRef, ObjType.WallStatue);

            //place test wall statue object - right wall
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                (Room.size.X + 0) * 16 + pos.X + 8,
                7 * 16 - 16 + pos.Y + 8);
            objRef.direction = Direction.Left;
            Functions_GameObject.SetType(objRef, ObjType.WallStatue);

            //place test wall statue object - bottom wall
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                5 * 16 + pos.X + 8,
                (Room.size.Y + 1) * 16 - 16 + pos.Y + 8);
            objRef.direction = Direction.Up;
            Functions_GameObject.SetType(objRef, ObjType.WallStatue);

            #endregion


            SpawnEnemies(Room);
        }

        public static void FinishBossRoom(Room Room)
        {
            //randomly place debris around room
            for (i = 0; i < 30; i++)
            {
                objRef = Functions_Pool.GetRoomObj();
                Functions_Movement.Teleport(objRef.compMove,
                    Functions_Random.Int(0, Room.size.X) * 16 + pos.X + 8,
                    Functions_Random.Int(0, Room.size.Y) * 16 + pos.Y + 8);
                objRef.direction = Direction.Down;
                Functions_GameObject.SetType(objRef, ObjType.DebrisFloor);
            }

            //spawn a boss actor
            actorRef = Functions_Pool.GetActor();
            Functions_Actor.SetType(actorRef, ActorType.Boss);
            //teleport boss to center of room
            Functions_Movement.Teleport(actorRef.compMove,
                Room.center.X + 8,
                Room.center.Y + 8);
        }

        public static void FinishShopRoom(Room Room)
        {
            PlaceExit(Room);


            #region Place some test shop objects

            //bookcase
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                5 * 16 + pos.X + 8,
                0 * 16 + pos.Y + 0);
            Functions_GameObject.SetType(objRef, ObjType.BlockDark);
            //drawers
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                7 * 16 + pos.X + 8,
                0 * 16 + pos.Y + 0);
            Functions_GameObject.SetType(objRef, ObjType.BlockLight);

            #endregion


            //create all the vendors
            CreateVendor(ObjType.VendorItems, new Vector2(1 * 16 + pos.X + 8, 4 * 16 + pos.Y + 8));
            CreateVendor(ObjType.VendorPotions, new Vector2(4 * 16 + pos.X + 8, 4 * 16 + pos.Y + 8));
            CreateVendor(ObjType.VendorMagic, new Vector2(7 * 16 + pos.X + 8, 4 * 16 + pos.Y + 8));
            CreateVendor(ObjType.VendorWeapons, new Vector2(10 * 16 + pos.X + 8, 4 * 16 + pos.Y + 8));
            CreateVendor(ObjType.VendorArmor, new Vector2(13 * 16 + pos.X + 8, 4 * 16 + pos.Y + 8));
            CreateVendor(ObjType.VendorEquipment, new Vector2(16 * 16 + pos.X + 8, 4 * 16 + pos.Y + 8));

            //create story vendor
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                7 * 16 + pos.X + 8,
                8 * 16 + pos.Y + 0);
            Functions_GameObject.SetType(objRef, ObjType.VendorStory);
        }

    }
}