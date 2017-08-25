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
            //shorten the room's position reference
            pos = Room.rec.Location;


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
            //update all roomObjs, then remove overlapping objs
            Functions_Pool.AlignRoomObjs();
            CleanupRoom(Room); //remove overlapping objs

            stopWatch.Stop(); time = stopWatch.Elapsed;
            DebugInfo.roomTime = time.Ticks;
            if (Flags.PrintOutput)
            { Debug.WriteLine("built " + Room.type + " room in " + time.Ticks + " ticks"); }
        }

        public static void FinishRoom(Room Room)
        {   //Pass the room to the appropriate method for completion
            stopWatch.Reset(); stopWatch.Start();

            //procedurally finished rooms
            if (Room.type == RoomType.Shop) { FinishShopRoom(Room); }
            else if (Room.type == RoomType.Exit) { FinishExitRoom(Room); }
            else if (Room.type == RoomType.Secret) { FinishSecretRoom(Room); }
            //special rooms (key, hub, boss)
            else if (Room.type == RoomType.Key)
            {
                BuildRoomXmlData(Assets.roomDataKey[Room.XMLid]);
                FinishKeyRoom(Room);
                AddWallStatues(Room);
            }
            else if (Room.type == RoomType.Hub)
            {
                BuildRoomXmlData(Assets.roomDataHub[Room.XMLid]);
                FinishHubRoom(Room);
                AddWallStatues(Room);
            }
            else if (Room.type == RoomType.Boss)
            {
                BuildRoomXmlData(Assets.roomDataBoss[Room.XMLid]);
                FinishBossRoom(Room);
            }
            //standard/generic rooms (column, row, square)
            else if (Room.type == RoomType.Column)
            {
                BuildRoomXmlData(Assets.roomDataColumn[Room.XMLid]);
                AddWallStatues(Room);
            }
            else if (Room.type == RoomType.Row)
            {
                BuildRoomXmlData(Assets.roomDataRow[Room.XMLid]);
                AddWallStatues(Room);
            }
            else if (Room.type == RoomType.Square)
            {
                BuildRoomXmlData(Assets.roomDataSquare[Room.XMLid]);
                AddWallStatues(Room);
            }
            //dungeon rooms get debris + cracked walls
            if (Room.type != RoomType.Shop)
            {
                ScatterDebris(Room);
                AddCrackedWalls(Room);
            }
            //update the room objs, remove overlapping objs
            Functions_Pool.AlignRoomObjs();
            CleanupRoom(Room);
            Assets.Play(Assets.sfxDoorOpen); //play door sfx









            stopWatch.Stop(); time = stopWatch.Elapsed;
            DebugInfo.roomTime += time.Ticks; //add finish time to roomTime
            if (Flags.PrintOutput)
            { Debug.WriteLine("finished " + Room.type + " room (id:" + Room.XMLid + ") in " + time.Ticks + " ticks"); }
        }

        public static void SpawnHeroInCurrentRoom()
        {   //teleport hero to currentRoom's spawn position
            Functions_Movement.Teleport(Pool.hero.compMove, 
                Functions_Level.currentRoom.spawnPos.X,
                Functions_Level.currentRoom.spawnPos.Y);
            Functions_Movement.StopMovement(Pool.hero.compMove);
            Pool.hero.compSprite.scale = 1.0f; //rescale hero to 100%
            //set camera's target to hero or room based on flag boolean
            if (Flags.CameraTracksHero) //center camera to hero
            { Camera2D.targetPosition = Pool.hero.compMove.newPosition; }
            else
            {   //center camera to current room
                Camera2D.targetPosition.X = Functions_Level.currentRoom.center.X;
                Camera2D.targetPosition.Y = Functions_Level.currentRoom.center.Y;
            }
            //teleport camera to targetPos, update camera view
            Camera2D.currentPosition.X = Camera2D.targetPosition.X;
            Camera2D.currentPosition.Y = Camera2D.targetPosition.Y;
            Functions_Camera2D.Update();
        }


        public static void SetRoomXMLid(Room Room)
        {   //based on the room type, set the xml value between 0 and relative xmlRoomData list count
            int count = 1;
            if (Room.type == RoomType.Boss) { count = Assets.roomDataBoss.Count; }
            else if (Room.type == RoomType.Column) { count = Assets.roomDataColumn.Count; }
            else if (Room.type == RoomType.Hub) { count = Assets.roomDataHub.Count; }
            else if (Room.type == RoomType.Key) { count = Assets.roomDataKey.Count; }
            else if (Room.type == RoomType.Row) { count = Assets.roomDataRow.Count; }
            else if (Room.type == RoomType.Square) { count = Assets.roomDataSquare.Count; }
            Room.XMLid = Functions_Random.Int(0, count);
        }

        public static void SetType(Room Room, RoomType Type)
        {
            Room.type = Type;
            //set room size based on type - sizes should be odd, so doors can be centered
            if (Type == RoomType.Exit) { Room.size.X = 11; Room.size.Y = 11; }
            else if (Type == RoomType.Hub) { Room.size.X = 19; Room.size.Y = 19; }
            else if (Type == RoomType.Boss) { Room.size.X = 19; Room.size.Y = 11; }
            else if (Type == RoomType.Key) { Room.size.X = 19; Room.size.Y = 11; }
            else if (Type == RoomType.Shop) { Room.size.X = 19; Room.size.Y = 11; }
            //comon dungeon rooms
            else if (Type == RoomType.Column) { Room.size.X = 11; Room.size.Y = 19; }
            else if (Type == RoomType.Row) { Room.size.X = 19; Room.size.Y = 11; }
            else if (Type == RoomType.Square) { Room.size.X = 11; Room.size.Y = 11; }
            else if (Type == RoomType.Secret) { Room.size.X = 3; Room.size.Y = 3; }
            //set collision rec size
            Room.rec.Width = Room.size.X * 16;
            Room.rec.Height = Room.size.Y * 16;
        }

        public static void MoveRoom(Room Room, int X, int Y)
        {
            Room.rec.X = X;
            Room.rec.Y = Y;
            Room.center.X = X + (Room.size.X / 2) * 16;
            Room.center.Y = Y + (Room.size.Y / 2) * 16;
        }

        public static void SetFloors(Room Room)
        {
            for (i = 0; i < Pool.floorCount; i++)
            {   //set all the floor sprite's current frame based on the room.type
                Pool.floorPool[i].currentFrame.X = 6;
                Pool.floorPool[i].currentFrame.Y = 0; //default to normal floor
                //determine what floor sprite to display for special rooms
                if (Room.type == RoomType.Hub) { Pool.floorPool[i].currentFrame.Y = 1; }
                else if (Room.type == RoomType.Key) { Pool.floorPool[i].currentFrame.Y = 1; }
                else if (Room.type == RoomType.Boss) { Pool.floorPool[i].currentFrame.Y = 2; }
            }
        }

        public static void SetDoors(Room Room)
        {
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active)
                {
                    if (Pool.roomObjPool[i].group == ObjGroup.Wall)
                    {   //check to see if wall collides with any door from dungeon
                        for (j = 0; j < Level.doors.Count; j++)
                        {
                            if (Pool.roomObjPool[i].compCollision.rec.Contains(Level.doors[j].rec.Location))
                            {
                                //set the room's doors based on the dungeon.door.type
                                if (Level.doors[j].type == DoorType.Bombable)
                                { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.DoorBombable); }

                                else if (Level.doors[j].type == DoorType.Boss)
                                { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.DoorBoss); }

                                else //all other doorTypes are Open
                                { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.DoorOpen); }

                                //set the door decorations (bombed/bombable doors dont get decorations)
                                if (Pool.roomObjPool[i].type == ObjType.DoorBoss)
                                { DecorateDoor(Pool.roomObjPool[i], ObjType.WallPillar); }
                                else if (Pool.roomObjPool[i].type == ObjType.DoorOpen)
                                { DecorateDoor(Pool.roomObjPool[i], ObjType.WallTorch); }

                                //finally, override door types based on specific room.type
                                if (Room.type == RoomType.Boss)
                                {   //all doors inside boss room are trap doors (push hero + close)
                                    Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.DoorTrap);
                                }

                                //sort door object
                                Functions_Component.SetZdepth(Pool.roomObjPool[i].compSprite);
                                //place a floor tile underneath door
                                floorRef = Functions_Pool.GetFloor();
                                floorRef.position = Pool.roomObjPool[i].compSprite.position;
                            }
                        }
                    }
                }
            }
        }
 


        //adds/changes objs in room 

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
            Functions_GameObject.SetType(objRef, ObjType.TableStone);

            //place vendor advertisement
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                Position.X + 16, Position.Y - 6);
            Functions_GameObject.SetType(objRef, ObjType.VendorAdvertisement);
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
            //set the room.spawnPos to pos above exit door obj
            Room.spawnPos.X = (Room.size.X / 2) * 16 + Room.rec.X + 8;
            Room.spawnPos.Y = Room.rec.Y + (Room.size.Y - 1) * 16;

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

        public static void AddWallStatues(Room Room)
        {   //add wall statues along 1/3rd and 2/3rds of all walls
            int RoomThirdX = Room.rec.X + (Room.size.X / 3) * 16 + 8;
            int RoomTwoThirdsX = 16 + RoomThirdX + (Room.size.X / 3) * 16;
            int RoomThirdY = Room.rec.Y + (Room.size.Y / 3) * 16 + 8;
            int RoomTwoThirdsY = 16 + RoomThirdY + (Room.size.Y / 3) * 16;
            //we could also check against room centers
            //if (obj.compSprite.position.X == Room.collision.rec.X + room.center.X)
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active)
                {
                    if (Pool.roomObjPool[i].type == ObjType.WallStraight)
                    {
                        if (Pool.roomObjPool[i].compSprite.position.X == RoomThirdX ||
                            Pool.roomObjPool[i].compSprite.position.X == RoomTwoThirdsX)
                        { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.WallStatue); }

                        else if (Pool.roomObjPool[i].compSprite.position.Y == RoomThirdY ||
                            Pool.roomObjPool[i].compSprite.position.Y == RoomTwoThirdsY)
                        { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.WallStatue); }

                        //the wall statue inherits the proper direction from the original wall obj
                    }
                }
            }
        }

        public static void ScatterDebris(Room Room)
        {   //randomly place debris at each floor tile position
            for (i = 0; i < Pool.floorCount; i++)
            {
                if (Pool.floorPool[i].visible)
                {
                    if (Functions_Random.Int(0, 100) > 80)
                    {
                        objRef = Functions_Pool.GetRoomObj();
                        Functions_Movement.Teleport(objRef.compMove,
                            Pool.floorPool[i].position.X + Functions_Random.Int(-8, 8),
                            Pool.floorPool[i].position.Y + Functions_Random.Int(-8, 8));
                        Functions_GameObject.SetType(objRef, ObjType.DebrisFloor);
                    }
                }
            }
        }

        public static void AddCrackedWalls(Room Room)
        {   //randomly change straight walls into cracked walls
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active && Pool.roomObjPool[i].type == ObjType.WallStraight)
                {
                    if (Functions_Random.Int(0, 100) > 85)
                    { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.WallStraightCracked); }
                }
            }
        }



        //decorates a door on left/right or top/bottom

        static Vector2 decorationPosA = new Vector2();
        static Vector2 decorationPosB = new Vector2();

        public static void DecorateDoor(GameObject Door, ObjType Type)
        {
            if (Door.direction == Direction.Up || Door.direction == Direction.Down)
            {   //build left/right decorations if Door.direction is Up or Down
                decorationPosA.X = Door.compSprite.position.X - 16;
                decorationPosA.Y = Door.compSprite.position.Y;
                decorationPosB.X = Door.compSprite.position.X + 16;
                decorationPosB.Y = Door.compSprite.position.Y;
            }
            else
            {   //build top/bottom decorations if Door.direction is Left or Right
                decorationPosA.X = Door.compSprite.position.X;
                decorationPosA.Y = Door.compSprite.position.Y - 16;
                decorationPosB.X = Door.compSprite.position.X;
                decorationPosB.Y = Door.compSprite.position.Y + 16;
            }
            //build wall decorationA torch/pillar/decoration
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove, decorationPosA.X, decorationPosA.Y);
            objRef.direction = Door.direction;
            Functions_GameObject.SetType(objRef, Type);
            //build wall decorationB torch/pillar/decoration
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove, decorationPosB.X, decorationPosB.Y);
            objRef.direction = Door.direction;
            Functions_GameObject.SetType(objRef, Type);
        }



        //removes overlapping objs from room

        static GameObject objA; //object we want to keep
        static GameObject objB; //object we want to remove
        static Boolean checkOverlap;
        static Boolean removeObjB;

        public static void CleanupRoom(Room Room)
        {
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                checkOverlap = false;
                objA = Pool.roomObjPool[i];

                if (objA.active)
                {   //objA groups & types we check overlap for
                    if (objA.group == ObjGroup.Door) { checkOverlap = true; }
                    else if (objA.group == ObjGroup.Wall) { checkOverlap = true; }
                    else if (objA.group == ObjGroup.Object) { checkOverlap = true; }
                }   //we are not checking liftable/draggable obj collisions with debris

                if (checkOverlap)
                {
                    for (j = 0; j < Pool.roomObjCount; j++)
                    {
                        objB = Pool.roomObjPool[j];
                        if (objB.active && objA != objB)
                        {
                            removeObjB = false;
                            if (objB.group == ObjGroup.Wall)
                            {   //prevent walls from overlapping doors, torches, and pillars
                                if (objA.group == ObjGroup.Door) { removeObjB = true; }
                                if (objA.type == ObjType.WallTorch) { removeObjB = true; }
                                if (objA.type == ObjType.WallPillar) { removeObjB = true; }
                            }
                            else if (objB.type == ObjType.DebrisFloor)
                            {   //prevent debris from overlapping various objects
                                if (objA.group == ObjGroup.Wall) { removeObjB = true; }
                                else if (objA.group == ObjGroup.Object) { removeObjB = true; }
                            }
                            if (removeObjB)
                            {   //check that objA and objB actually overlap
                                if (objA.compCollision.rec.Intersects(objB.compCollision.rec))
                                { Functions_Pool.Release(objB); }
                            }
                        }
                    }
                }
            }

            if (Flags.DrawDebugInfo)
            {   //get the count of active roomObjs, if debug info is visible
                Pool.roomObjIndex = 0;
                for (i = 0; i < Pool.roomObjCount; i++)
                { if (Pool.roomObjPool[i].active) { Pool.roomObjIndex++; } }
            }
        }



        //rooms that are procedurally 'finished' - exit, shop, secret

        public static void FinishShopRoom(Room Room)
        {
            PlaceExit(Room);


            #region Place some test shop objects

            //bookcase
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                5 * 16 + pos.X + 8,
                0 * 16 + pos.Y + 0);
            Functions_GameObject.SetType(objRef, ObjType.Bookcase1);
            //drawers
            objRef = Functions_Pool.GetRoomObj();
            Functions_Movement.Teleport(objRef.compMove,
                7 * 16 + pos.X + 8,
                0 * 16 + pos.Y + 0);
            Functions_GameObject.SetType(objRef, ObjType.Bookcase2);

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

        public static void FinishSecretRoom(Room Room)
        {
            //what goes in a secret room? not chests
            //perhaps a secret vendor? or vendors?
        }

        public static void FinishBossRoom(Room Room)
        {
            actorRef = Functions_Pool.GetActor();
            Functions_Actor.SetType(actorRef, ActorType.Boss);
            //teleport boss to center of room
            Functions_Movement.Teleport(actorRef.compMove,
                Room.center.X + 8,
                Room.center.Y + 8);
        }

        public static void FinishKeyRoom(Room Room)
        {
            for (i = 0; i < Pool.roomObjCount; i++)
            {   //loop thru all active roomObjects
                if (Pool.roomObjPool[i].active)
                {   //find any chest objects in the key room
                    if (Pool.roomObjPool[i].group == ObjGroup.Chest)
                    {   //check the dungeon.bigKey boolean to see if this chest should be filled
                        if (Level.bigKey)
                        { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.ChestEmpty); }
                        else //if hero has found the map, this chest is empty, else it has a key
                        { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.ChestKey); }
                    }
                }
            }
        }

        public static void FinishHubRoom(Room Room)
        {
            for (i = 0; i < Pool.roomObjCount; i++)
            {   //loop thru all active roomObjects
                if (Pool.roomObjPool[i].active)
                {   //find any chest objects in the hub room
                    if (Pool.roomObjPool[i].group == ObjGroup.Chest)
                    {   //check the dungeon.map boolean to see if this chest should be filled
                        if (Level.map)
                        { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.ChestEmpty); }
                        else //if hero has found the map, this chest is empty, else it has a map
                        { Functions_GameObject.SetType(Pool.roomObjPool[i], ObjType.ChestMap); }
                    }
                    else if (Pool.roomObjPool[i].type == ObjType.DoorBoss)
                    {   //build the boss welcome mat (left)
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
                }
            }
        }



        //rooms finished using XML roomData - row, column, square, boss, key, hub

        public static void BuildRoomXmlData(RoomXmlData RoomXmlData)
        {

            #region Create room objs & enemies

            if (RoomXmlData != null && RoomXmlData.objs.Count > 0)
            {
                for (i = 0; i < RoomXmlData.objs.Count; i++)
                {   
                    //we store roomObjs & projectiles in roomXmlData
                    //based on the roomXmlData.obj.type, get an entity or a roomObj
                    if(RoomXmlData.objs[i].type == ObjType.ProjectileSpikeBlock)
                    { objRef = Functions_Pool.GetEntity(); }
                    else { objRef = Functions_Pool.GetRoomObj(); }
                    
                    //move roomObj to xmlObj's position (with room offset)
                    Functions_Movement.Teleport(objRef.compMove,
                        Functions_Level.currentRoom.rec.X + RoomXmlData.objs[i].posX,
                        Functions_Level.currentRoom.rec.Y + RoomXmlData.objs[i].posY);
                    //get obj direction
                    objRef.direction = RoomXmlData.objs[i].direction;
                    //finally, set roomObj.type to xmlObj.type
                    Functions_GameObject.SetType(objRef, RoomXmlData.objs[i].type);

                    //create enemies at enemySpawn obj locations
                    if (objRef.group == ObjGroup.EnemySpawn)
                    {
                        actorRef = Functions_Pool.GetActor();
                        if (actorRef != null)
                        {   //we should be checking what level of enemy to create
                            Functions_Actor.SetType(actorRef, ActorType.Blob);
                            Functions_Movement.Teleport(actorRef.compMove,
                                objRef.compSprite.position.X,
                                objRef.compSprite.position.Y);
                        }
                    }
                }
            }

            #endregion


            #region Check enemySpawn obj visibility

            if (Flags.ShowEnemySpawns == false)
            {   //find any spawnObj, set obj.active = false
                for (i = 0; i < Pool.roomObjCount; i++)
                {
                    if (Pool.roomObjPool[i].group == ObjGroup.EnemySpawn)
                    { Pool.roomObjPool[i].active = false; }
                }
            }

            #endregion

        }



        //methods that alter roomObjs

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



    }
}