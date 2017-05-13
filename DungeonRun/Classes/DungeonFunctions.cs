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
    public static class DungeonFunctions
    {

        public static ScreenDungeon dungeonScreen;
        public static Dungeon dungeon;
        public static Room currentRoom; //points to a room on the dungeon's room list

        public static Stopwatch stopWatch = new Stopwatch();
        public static TimeSpan time;
        public static int i;
        public static int j;
        public static int g;
        public static int musicCounter = 0;
        public static ComponentSprite floorRef;
        public static GameObject objRef;
        public static Actor actorRef;
        public static Point pos;



        public static void Initialize(ScreenDungeon DungeonScreen) { dungeonScreen = DungeonScreen; }

        public static void BuildDungeon(DungeonType Type)
        {
            //create a new dungeon
            dungeon = new Dungeon(""+ Type);
            dungeon.type = Type;

            if (Type == DungeonType.Shop)
            {   //set the objPool texture
                PoolFunctions.SetDungeonTexture(Assets.shopSheet);
                //create the shop room
                dungeon.rooms.Add(new Room(new Point(16 * 10, 16 * 21), new Byte2(20, 10), RoomType.Shop, 10, 0));

                //keep the title music playing
                MusicFunctions.PlayMusic(Music.Title);
            }
            else
            {   //set the objPool texture
                PoolFunctions.SetDungeonTexture(Assets.cursedCastleSheet);
                //populate the dungeon with rooms
                dungeon.rooms.Add(new Room(new Point(16 * 10, 16 * 21), new Byte2(20, 10), RoomType.Exit, 10, 0));
                dungeon.rooms.Add(new Room(new Point(16 * 10, 16 * 10), new Byte2(20, 10), RoomType.Boss, 10, 1));

                //cycle thru the dungeon tracks
                if (musicCounter == 0) { MusicFunctions.PlayMusic(Music.DungeonA); }
                else if (musicCounter == 1) { MusicFunctions.PlayMusic(Music.DungeonB); }
                else if (musicCounter == 2) { MusicFunctions.PlayMusic(Music.DungeonC); }
                //increment and reset the counter
                musicCounter++;
                if (musicCounter > 2) { musicCounter = 0; }
            }

            //build the first room in the dungeon (the spawn room)
            BuildRoom(dungeon.rooms[0]);
            currentRoom = dungeon.rooms[0];

            //reset the dungeon screen's dungeon record, passing dungeonID
            DungeonRecord.Reset();
            DungeonRecord.dungeonID = 0; //ID = 0 for now
            DungeonRecord.timer.Start(); //start the record timer

            //fade the dungeon screen out from black, revealing the new level
            dungeonScreen.overlayAlpha = 1.0f;
            dungeonScreen.displayState = DisplayState.Opening;
        }

        public static void BuildRoom(Room Room)
        {
            stopWatch.Reset(); stopWatch.Start();

            //reset the pools + counter
            PoolFunctions.Reset();
            Pool.counter = 0;
            //shorten the room's position reference
            pos = Room.collision.rec.Location;


            #region Build the room

            for (i = 0; i < Room.size.X; i++)
            {
                for (j = 0; j < Room.size.Y; j++)
                {
                    //place the floors
                    floorRef = PoolFunctions.GetFloor();
                    floorRef.position.X = i * 16 + pos.X + 8;
                    floorRef.position.Y = j * 16 + pos.Y + 8;

                    if (Room.type == RoomType.Shop) { floorRef.currentFrame.Y = 0; }
                    else if (Room.type == RoomType.Exit) { floorRef.currentFrame.Y = 0; }
                    else if (Room.type == RoomType.Boss) { floorRef.currentFrame.Y = 1; }


                    #region Top Row Walls

                    if (j == 0)
                    {
                        //top row
                        objRef = PoolFunctions.GetObj();
                        MovementFunctions.Teleport(objRef.compMove,
                            i * 16 + pos.X + 8,
                            0 * 16 - 16 + pos.Y + 8);
                        objRef.direction = Direction.Down;
                        GameObjectFunctions.SetType(objRef, ObjType.WallStraight);

                        if (i == 0)
                        {   //topleft corner
                            objRef = PoolFunctions.GetObj();
                            MovementFunctions.Teleport(objRef.compMove, 
                                -16 + pos.X + 8, 
                                -16 + pos.Y + 8);
                            objRef.direction = Direction.Down;
                            GameObjectFunctions.SetType(objRef, ObjType.WallInteriorCorner);
                        }
                        else if (i == Room.size.X - 1)
                        {   //topright corner
                            objRef = PoolFunctions.GetObj();
                            MovementFunctions.Teleport(objRef.compMove,
                                Room.size.X * 16 + pos.X + 8, 
                                -16 + pos.Y + 8);
                            objRef.direction = Direction.Left;
                            GameObjectFunctions.SetType(objRef, ObjType.WallInteriorCorner);
                        }
                    }

                    #endregion


                    #region Bottom Row Walls

                    else if (j == Room.size.Y - 1)
                    {
                        //bottom row
                        objRef = PoolFunctions.GetObj();
                        MovementFunctions.Teleport(objRef.compMove,
                            i * 16 + pos.X + 8,
                            Room.size.Y * 16 + pos.Y + 8);
                        objRef.direction = Direction.Up;
                        GameObjectFunctions.SetType(objRef, ObjType.WallStraight);

                        if (i == 0)
                        {   //bottom left corner
                            objRef = PoolFunctions.GetObj();
                            MovementFunctions.Teleport(objRef.compMove, 
                                -16 + pos.X + 8,
                                Room.size.Y * 16 + pos.Y + 8);
                            objRef.direction = Direction.Right;
                            GameObjectFunctions.SetType(objRef, ObjType.WallInteriorCorner);
                        }
                        else if (i == Room.size.X - 1)
                        {   //bottom right corner
                            objRef = PoolFunctions.GetObj();
                            MovementFunctions.Teleport(objRef.compMove,
                                Room.size.X * 16 + pos.X + 8,
                                Room.size.Y * 16 + pos.Y + 8);
                            objRef.direction = Direction.Up;
                            GameObjectFunctions.SetType(objRef, ObjType.WallInteriorCorner);
                        }
                    }

                    #endregion


                    #region Left & Right Column Walls

                    if (i == 0)
                    {   //left side
                        objRef = PoolFunctions.GetObj();
                        MovementFunctions.Teleport(objRef.compMove, 
                            i * 16 - 16 + pos.X + 8, 
                            j * 16 + pos.Y + 8);
                        objRef.direction = Direction.Right;
                        GameObjectFunctions.SetType(objRef, ObjType.WallStraight);
                    }
                    else if (i == Room.size.X - 1)
                    {   //right side
                        objRef = PoolFunctions.GetObj();
                        MovementFunctions.Teleport(objRef.compMove, 
                            i * 16 + 16 + pos.X + 8, 
                            j * 16 + pos.Y + 8);
                        objRef.direction = Direction.Left;
                        GameObjectFunctions.SetType(objRef, ObjType.WallStraight);
                    }

                    #endregion
                    
                }
            }

            #endregion


            FinishRoom(Room); //add type specific room objs
            CleanupRoom(Room); //remove overlapping objs
            //update the object pool, since we teleported objects around
            PoolFunctions.UpdateObjectPool();

            stopWatch.Stop(); time = stopWatch.Elapsed;
            DebugInfo.roomTime = time.Ticks;
        }

        public static void FinishRoom(Room Room)
        {

            #region Exit Room

            if (Room.type == RoomType.Exit)
            {

                #region Create the Exit, place Hero at Exit

                //create the exit
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    (Room.size.X / 2) * 16 + pos.X + 8,
                    Room.size.Y * 16 + pos.Y + 8 - 16 * 2);
                GameObjectFunctions.SetType(objRef, ObjType.Exit);

                //place hero at exit door
                Functions_Actor.SetType(Pool.hero, ActorType.Hero);
                MovementFunctions.Teleport(Pool.hero.compMove,
                    objRef.compSprite.position.X,
                    objRef.compSprite.position.Y + 8);
                MovementFunctions.StopMovement(Pool.hero.compMove);
                Pool.hero.direction = Direction.Up; //face hero up

                //place the exit light fx over exit obj
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    (Room.size.X / 2) * 16 + pos.X + 8,
                    Room.size.Y * 16 + pos.Y + 8 - 16 * 1);
                GameObjectFunctions.SetType(objRef, ObjType.ExitLightFX);

                //create exit pillars
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    (Room.size.X / 2) * 16 + pos.X + 8 - 16,
                    Room.size.Y * 16 + pos.Y + 8 - 16 * 2);
                GameObjectFunctions.SetType(objRef, ObjType.ExitPillarLeft);
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    (Room.size.X / 2) * 16 + pos.X + 8 + 16,
                    Room.size.Y * 16 + pos.Y + 8 - 16 * 2);
                GameObjectFunctions.SetType(objRef, ObjType.ExitPillarRight);

                #endregion


                #region Create the BossDoor, Decals, and Door Decorations

                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    5 * 16 + pos.X + 8,
                    0 * 16 - 16 + pos.Y + 8);
                objRef.direction = Direction.Down;
                GameObjectFunctions.SetType(objRef, ObjType.DoorBoss);
                //build left wall torch
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    (5 - 1) * 16 + pos.X + 8,
                    0 * 16 - 16 + pos.Y + 8);
                objRef.direction = Direction.Down;
                GameObjectFunctions.SetType(objRef, ObjType.WallTorch);
                //build right wall torch
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    (5 + 1) * 16 + pos.X + 8,
                    0 * 16 - 16 + pos.Y + 8);
                objRef.direction = Direction.Down;
                GameObjectFunctions.SetType(objRef, ObjType.WallTorch);

                //build the boss welcome mat (left)
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    5 * 16 + pos.X + 0,
                    1 * 16 - 16 + pos.Y + 8);
                objRef.direction = Direction.Down;
                GameObjectFunctions.SetType(objRef, ObjType.BossDecal);
                //build the boss welcome mat (right)
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    6 * 16 + pos.X + 0,
                    1 * 16 - 16 + pos.Y + 8);
                objRef.direction = Direction.Down;
                objRef.compSprite.flipHorizontally = true;
                GameObjectFunctions.SetType(objRef, ObjType.BossDecal);
                

                //place skeleton pots along left wall
                for (i = 0; i < Room.size.Y; i++)
                {
                    objRef = PoolFunctions.GetObj();
                    MovementFunctions.Teleport(objRef.compMove,
                        0 * 16 + pos.X + 8,
                        i * 16 + pos.Y + 8);
                    objRef.direction = Direction.Down;
                    GameObjectFunctions.SetType(objRef, ObjType.PotSkull);
                }

                #endregion


                #region Create the Testing Chests

                //place chest gameObj in bottom right corner
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    (Room.size.X - 1) * 16 + pos.X + 8,
                    1 * 16 + pos.Y + 8);
                objRef.direction = Direction.Down;
                GameObjectFunctions.SetType(objRef, ObjType.ChestGold);

                //create a big key chest
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    (Room.size.X - 1) * 16 + pos.X + 8,
                    3 * 16 + pos.Y + 8);
                objRef.direction = Direction.Down;
                GameObjectFunctions.SetType(objRef, ObjType.ChestKey);

                //create a map chest
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    (Room.size.X - 1) * 16 + pos.X + 8,
                    5 * 16 + pos.Y + 8);
                objRef.direction = Direction.Down;
                GameObjectFunctions.SetType(objRef, ObjType.ChestMap);

                //create a heart piece chest
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    (Room.size.X - 1) * 16 + pos.X + 8,
                    7 * 16 + pos.Y + 8);
                objRef.direction = Direction.Down;
                GameObjectFunctions.SetType(objRef, ObjType.ChestHeartPiece);

                #endregion


                //Create testing spike blocks
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    7 * 16 + pos.X + 8,
                    3 * 16 + pos.Y + 8);
                GameObjectFunctions.SetType(objRef, ObjType.BlockSpikes);

                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    7 * 16 + pos.X + 8,
                    7 * 16 + pos.Y + 8);
                objRef.compMove.direction = Direction.Right;
                GameObjectFunctions.SetType(objRef, ObjType.BlockSpikes);
                

                //place test conveyor belt
                for (i = 0; i < Room.size.Y; i++)
                {
                    objRef = PoolFunctions.GetObj();
                    MovementFunctions.Teleport(objRef.compMove,
                        15 * 16 + pos.X + 8,
                        i * 16 + pos.Y + 8);
                    objRef.direction = Direction.Down;
                    GameObjectFunctions.SetType(objRef, ObjType.ConveyorBelt);
                }

                //place a test bumper
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    13 * 16 + pos.X + 8,
                    3 * 16 + pos.Y + 8);
                GameObjectFunctions.SetType(objRef, ObjType.Bumper);

                //spawn enemies inside of this room
                SpawnEnemies(Room);
            }

            #endregion


            #region Boss Room

            else if (Room.type == RoomType.Boss)
            {

                #region Create Trap Door + Door Decorations

                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    5 * 16 + pos.X + 8,
                    Room.size.Y * 16 + pos.Y + 8);
                objRef.direction = Direction.Up;
                GameObjectFunctions.SetType(objRef, ObjType.DoorTrap);
                //build left wall torch
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    (5 - 1) * 16 + pos.X + 8,
                    Room.size.Y * 16 + pos.Y + 8);
                objRef.direction = Direction.Up;
                GameObjectFunctions.SetType(objRef, ObjType.WallTorch);
                //build right wall torch
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    (5 + 1) * 16 + pos.X + 8,
                    Room.size.Y * 16 + pos.Y + 8);
                objRef.direction = Direction.Up;
                GameObjectFunctions.SetType(objRef, ObjType.WallTorch);

                #endregion


                //spawn a boss actor
                actorRef = PoolFunctions.GetActor();
                Functions_Actor.SetType(actorRef, ActorType.Boss);
                //teleport boss to center of room
                MovementFunctions.Teleport(actorRef.compMove,
                    Room.center.X + 8,
                    Room.center.Y + 8);

                //randomly place debris around room
                for (i = 0; i < 30; i++)
                {
                    objRef = PoolFunctions.GetObj();
                    MovementFunctions.Teleport(objRef.compMove,
                        GetRandom.Int(0, Room.size.X) * 16 + pos.X + 8,
                        GetRandom.Int(0, Room.size.Y) * 16 + pos.Y + 8);
                    objRef.direction = Direction.Down;
                    GameObjectFunctions.SetType(objRef, ObjType.DebrisFloor);
                }

                //dont spawn any mobs in this room
            }

            #endregion


            #region Create Shop Room

            else if (Room.type == RoomType.Shop)
            {


                #region Create the Exit, place Hero at Exit

                //create the exit
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    (Room.size.X / 2) * 16 + pos.X + 8,
                    Room.size.Y * 16 + pos.Y + 8 - 16 * 2);
                GameObjectFunctions.SetType(objRef, ObjType.Exit);

                //place hero at exit door
                Functions_Actor.SetType(Pool.hero, ActorType.Hero);
                Pool.hero.state = ActorState.Idle;
                MovementFunctions.Teleport(Pool.hero.compMove,
                    objRef.compSprite.position.X,
                    objRef.compSprite.position.Y + 8);
                MovementFunctions.StopMovement(Pool.hero.compMove);
                Pool.hero.direction = Direction.Up; //face hero up

                //place the exit light fx over exit obj
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    (Room.size.X / 2) * 16 + pos.X + 8,
                    Room.size.Y * 16 + pos.Y + 8 - 16 * 1);
                GameObjectFunctions.SetType(objRef, ObjType.ExitLightFX);

                //create exit pillars
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    (Room.size.X / 2) * 16 + pos.X + 8 - 16,
                    Room.size.Y * 16 + pos.Y + 8 - 16 * 2);
                GameObjectFunctions.SetType(objRef, ObjType.ExitPillarLeft);
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    (Room.size.X / 2) * 16 + pos.X + 8 + 16,
                    Room.size.Y * 16 + pos.Y + 8 - 16 * 2);
                GameObjectFunctions.SetType(objRef, ObjType.ExitPillarRight);

                #endregion



                //place some test shop objects

                //bookcase
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    5 * 16 + pos.X + 8,
                    0 * 16 + pos.Y + 0);
                GameObjectFunctions.SetType(objRef, ObjType.BlockDark);

                //drawers
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    7 * 16 + pos.X + 8,
                    0 * 16 + pos.Y + 0);
                GameObjectFunctions.SetType(objRef, ObjType.BlockLight);



                //create all the vendors
                CreateVendor(ObjType.VendorItems, new Vector2(1 * 16 + pos.X + 8, 4 * 16 + pos.Y + 8));
                CreateVendor(ObjType.VendorPotions, new Vector2(4 * 16 + pos.X + 8, 4 * 16 + pos.Y + 8));
                CreateVendor(ObjType.VendorMagic, new Vector2(7 * 16 + pos.X + 8, 4 * 16 + pos.Y + 8));
                CreateVendor(ObjType.VendorWeapons, new Vector2(10 * 16 + pos.X + 8, 4 * 16 + pos.Y + 8));
                CreateVendor(ObjType.VendorArmor, new Vector2(13 * 16 + pos.X + 8, 4 * 16 + pos.Y + 8));
                CreateVendor(ObjType.VendorEquipment, new Vector2(16 * 16 + pos.X + 8, 4 * 16 + pos.Y + 8));

                //create story vendor
                objRef = PoolFunctions.GetObj();
                MovementFunctions.Teleport(objRef.compMove,
                    7 * 16 + pos.X + 8,
                    8 * 16 + pos.Y + 0);
                GameObjectFunctions.SetType(objRef, ObjType.VendorStory);

            }

            #endregion


        }

        public static void SpawnEnemies(Room Room)
        {
            if (Flags.SpawnMobs)
            {
                //place enemies within the room
                for (i = 0; i < Room.enemyCount; i++)
                {
                    actorRef = PoolFunctions.GetActor();
                    //we SHOULD be checking to see if actorRef is null..
                    //but because we reset the pool earlier in this function,
                    //and the room's enemy count will never be larger than the total actors
                    //we'll never get a null result from GetActor() right here
                    Functions_Actor.SetType(actorRef, ActorType.Blob);
                    //get a random value between the min/max size of room
                    int randomX = GetRandom.Int(-Room.size.X + 2, Room.size.X - 2);
                    int randomY = GetRandom.Int(-Room.size.Y + 2, Room.size.Y - 2);
                    //divide random value in half
                    randomX = randomX / 2;
                    randomY = randomY / 2;
                    //ensure this value isn't 0
                    if (randomX == 0) { randomX = 1; }
                    if (randomY == 0) { randomY = 1; }
                    //teleport actor to center of room, apply random offset
                    MovementFunctions.Teleport(actorRef.compMove,
                        Room.center.X + 16 * randomX + 8,
                        Room.center.Y + 16 * randomY + 8);
                }
            }
        }

        public static void CleanupRoom(Room Room)
        {
            //remove any walls that overlap doors
            for (i = 0; i < Pool.objCount; i++)
            {
                if (Pool.objPool[i].group == ObjGroup.Door && Pool.objPool[i].active)
                {
                    for (j = 0; j < Pool.objCount; j++)
                    {
                        if (Pool.objPool[j].group == ObjGroup.Wall && Pool.objPool[j].active)
                        {
                            if (Pool.objPool[i].compCollision.rec.Intersects(Pool.objPool[j].compCollision.rec))
                            {
                                PoolFunctions.Release(Pool.objPool[j]);
                            }
                        }
                    }
                }
            }
        }




        public static void CreateVendor(ObjType VendorType, Vector2 Position)
        {
            //place vendor
            objRef = PoolFunctions.GetObj();
            MovementFunctions.Teleport(objRef.compMove,
                Position.X, Position.Y);
            GameObjectFunctions.SetType(objRef, VendorType);

            //place stone table
            objRef = PoolFunctions.GetObj();
            MovementFunctions.Teleport(objRef.compMove,
                Position.X + 16, Position.Y);
            GameObjectFunctions.SetType(objRef, ObjType.SwitchBlockUp);
            
            //place vendor advertisement
            objRef = PoolFunctions.GetObj();
            MovementFunctions.Teleport(objRef.compMove,
                Position.X + 16, Position.Y - 6);
            GameObjectFunctions.SetType(objRef, ObjType.VendorAdvertisement);
            objRef.compAnim.currentAnimation = new List<Byte4>();


            #region Display the vendor's wares for sale

            if (VendorType == ObjType.VendorItems)
            {   //add all the items from the main sheet
                objRef.compAnim.currentAnimation.Add(new Byte4(5, 5, 0, 0)); //boomerang
                objRef.compAnim.currentAnimation.Add(new Byte4(5, 6, 0, 0)); //bomb
                objRef.compAnim.currentAnimation.Add(new Byte4(5, 7, 0, 0)); //bombs
            }
            else if (VendorType == ObjType.VendorPotions)
            {   //add all the potions from the main sheet
                //objRef.compAnim.currentAnimation.Add(new Byte4(6, 5, 0, 0)); //empty bottle
                objRef.compAnim.currentAnimation.Add(new Byte4(6, 6, 0, 0)); //health
                //objRef.compAnim.currentAnimation.Add(new Byte4(6, 7, 0, 0)); //magic
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
                objRef.compAnim.currentAnimation.Add(new Byte4(8, 5, 0, 0)); //sword
                objRef.compAnim.currentAnimation.Add(new Byte4(8, 6, 0, 0)); //bow
                objRef.compAnim.currentAnimation.Add(new Byte4(8, 7, 0, 0)); //staff
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





    }
}