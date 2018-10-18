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
    public static class Functions_Hero
    {
        static int i;

        public static Point interactionPoint;
        public static Rectangle heroRec; //16x16 px rec that matches hero's sprite
        public static Boolean boomerangInPlay = false; //only 1 boomerang on screen at once
        public static Boolean underRoof = false; //is hero under a roofObj?





        
        

        public static void SetFieldSpawnPos(GameObject Obj)
        {   //this assumes obj is a 2/3x4 dugneon entrance obj!

            //match obj X
            LevelSet.spawnPos_Field.X = Obj.compSprite.position.X; 
            //setup spawnPos X based on obj.type
            if (Obj.type == ObjType.Wor_Entrance_MountainDungeon) //this is modeled as a 2x4 obj
            {
                LevelSet.spawnPos_Field.X += 8;
            }
            else
            {   //all other entrances are 3x4 objs
                LevelSet.spawnPos_Field.X += 16;
            }

            //setup Y spawnPos
            LevelSet.spawnPos_Field.Y = Obj.compSprite.position.Y + 16 * 3 + 10;
            //^ start with obj.Y, add vertical south offset (place hero in front of obj)
        }

        public static void ResetFieldSpawnPos()
        {   //set to center of room, plus south offset
            LevelSet.spawnPos_Field.X = LevelSet.field.currentRoom.center.X;
            LevelSet.spawnPos_Field.Y = LevelSet.field.currentRoom.rec.Y;
            //spawn hero at bottom of level
            LevelSet.spawnPos_Field.Y += LevelSet.field.currentRoom.rec.Height - 16 * 4;
        }
        














        static Functions_Hero()
        {
            //interactionRec = new ComponentCollision();
            interactionPoint = new Point(0, 0);
            heroRec = new Rectangle(0, 0, 16, 16);
        }

        public static void CheckRoomCollision()
        {
            //only process this method if the level screen is completely open
            if (Screens.Level.displayState != DisplayState.Opened) { return; }
            //otherwise it processes while level is closing causing bugs

            if (LevelSet.currentLevel.isField)
            {   //hero is in level

                if (Flags.Clipping) //if player has enabled clipping
                { return; } //prevent exit of field level (for editing)


                #region Handle hero transferring to overworld screen or field

                if(heroRec.Intersects(LevelSet.currentLevel.currentRoom.rec) == false)
                {
                    //some fields return to other fields
                    if (LevelSet.currentLevel.ID == LevelID.SkullIsland_ColliseumPit)
                    {   //return to colliseum exterior level
                        LevelSet.field.ID = LevelID.SkullIsland_Colliseum;
                        Functions_Level.CloseLevel(ExitAction.Field);
                    }
                    //all other fields return to overworld screen
                    else
                    {
                        Functions_Level.CloseLevel(ExitAction.Overworld);
                    }

                    //stop hero's movement
                    Functions_Movement.StopMovement(Pool.hero.compMove);
                }

                #endregion

            }
            else
            {
                //hero is in dungeon

                #region Handle Hero transferring between Level.Rooms

                for (i = 0; i < LevelSet.currentLevel.rooms.Count; i++)
                {   //if the current room is not the room we are checking against, then continue
                    if (LevelSet.currentLevel.currentRoom != LevelSet.currentLevel.rooms[i])
                    {   //if heroRec collides with room rec, set it as currentRoom, build room
                        if (heroRec.Intersects(LevelSet.currentLevel.rooms[i].rec))
                        {

                            if (Pool.hero.carrying) //destroy anything hero is carrying
                            {
                                Pool.hero.carrying = false;
                                Functions_GameObject.HandleCommon(Pool.hero.heldObj, Direction.None);
                                Functions_Pool.Release(Pool.hero.heldObj);
                            }


                            //transitions between rooms, build
                            LevelSet.currentLevel.currentRoom = LevelSet.currentLevel.rooms[i];
                            Functions_Room.BuildRoom(LevelSet.currentLevel.rooms[i]);
                            LevelSet.currentLevel.rooms[i].visited = true;
                        }
                    }
                }

                #endregion


                #region Track Doors that Hero has visited

                for (i = 0; i < LevelSet.currentLevel.doors.Count; i++)
                {   //check heroRec collision against Level.doors
                    if (heroRec.Intersects(LevelSet.currentLevel.doors[i].rec))
                    {   //track doors hero has visited
                        LevelSet.currentLevel.doors[i].visited = true;
                        if (LevelSet.currentLevel.doors[i].type == DoorType.Open)
                        {   //set hero's spawnPos to this open door's position
                            LevelSet.spawnPos_Dungeon.X = LevelSet.currentLevel.doors[i].rec.X + 8;
                            LevelSet.spawnPos_Dungeon.Y = LevelSet.currentLevel.doors[i].rec.Y + 8;
                        }
                    }
                }

                #endregion


                #region Open/Close Doors for Hero

                for (i = 0; i < Pool.roomObjCount; i++)
                {
                    if (Pool.roomObjPool[i].active) //roomObj must be active
                    {
                        if (Pool.roomObjPool[i].type == ObjType.Dungeon_DoorOpen)
                        {   //set open/bombed doors to blocking or non-blocking
                            Pool.roomObjPool[i].compCollision.blocking = true; //set door blocking
                            //compare hero to door positions, unblock door if hero is close enough
                            if (Math.Abs(Pool.hero.compSprite.position.X - Pool.roomObjPool[i].compSprite.position.X) < 18)
                            {   //compare hero to door sprite positions, unblock door if hero is close enough
                                if (Math.Abs(Pool.hero.compSprite.position.Y - Pool.roomObjPool[i].compSprite.position.Y) < 18)
                                { Pool.roomObjPool[i].compCollision.blocking = false; }
                            }
                        }
                    }
                }

                #endregion

            }
        }

        public static void ClearInteractionRec()
        {   //move the interaction point offscreen
            interactionPoint.X = -1000;
            interactionPoint.Y = -1000;
        }

        public static void SetInteractionRec()
        {
            //center interaction point to hero's collision rec
            interactionPoint.X = Pool.hero.compCollision.rec.X;
            interactionPoint.Y = Pool.hero.compCollision.rec.Y;
            interactionPoint.X += Pool.hero.compCollision.rec.Width / 2;
            interactionPoint.Y += Pool.hero.compCollision.rec.Height / 2;

            //offset the intPoint based on the direction hero is facing
            if (Pool.hero.direction == Direction.Up)
            {
                interactionPoint.Y -= 8;
            }
            else if (Pool.hero.direction == Direction.Down)
            {
                interactionPoint.Y += 8;
            }
            else if (
                Pool.hero.direction == Direction.Left ||
                Pool.hero.direction == Direction.UpLeft ||
                Pool.hero.direction == Direction.DownLeft)
            {
                interactionPoint.X -= 8;
            }
            else if (
                Pool.hero.direction == Direction.Right ||
                Pool.hero.direction == Direction.UpRight ||
                Pool.hero.direction == Direction.DownRight)
            {
                interactionPoint.X += 8;
            }
        }

        public static void PushGrabbedObj()
        {
            if (Pool.hero.state == ActorState.Move)
            {
                //place interaction rec, check that it still touches grabbedObj
                SetInteractionRec();

                //else, hero has moved too far away to push/pull obj
                if (Pool.hero.grabbedObj.compCollision.rec.Contains(interactionPoint))
                {
                    //push grabbed obj in hero's move direction
                    Functions_Movement.Push(
                        Pool.hero.grabbedObj.compMove,
                        Pool.hero.compMove.direction,
                        0.08f);
                    //spam play drag sfx here
                    Assets.Play(Assets.sfxDragObj);
                }
                else
                {   //release the grabbed object
                    Pool.hero.grabbing = false;
                    Pool.hero.grabbedObj = null;
                }

                ClearInteractionRec();

                //the grabbedObj shoudn't be null rn
                //unless it's been destroyed while dragged
                //that could cause a big problem, lol
                //if (Actor.grabbedObj != null) { }
            }
        }

        static Boolean collision = false;
        public static Boolean CheckInteractionRec()
        {   
            //note this method happens once, on the frame player presses A button
            SetInteractionRec();
            collision = false;
            //check to see if the interactionRec collides with any gameObjects
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active)
                {
                    if (Pool.roomObjPool[i].compCollision.rec.Contains(interactionPoint))
                    {
                        if (Pool.roomObjPool[i].type == ObjType.Wor_Water
                            || Pool.roomObjPool[i].type == ObjType.Wor_MountainWall_Mid
                            || Pool.roomObjPool[i].type == ObjType.Wor_MountainWall_Bottom)
                        { } //ignore these objects for hero rec interaction
                        else
                        {   //all other objects are tested for interaction
                            Functions_Movement.StopMovement(Pool.hero.compMove);
                            Pool.hero.stateLocked = true;
                            Pool.hero.lockTotal = 10; //required to show the pickup animation
                            collision = true;
                            //handle the hero interaction, may overwrite hero.lockTotal
                            InteractRecWith(Pool.roomObjPool[i]);
                            //we could bail here if we wanted only 1 interaction per frame
                            //but we allow overlapping obj interactions cause we cray'
                        }
                    }
                }
            }
            //move the interaction rec offscreen
            ClearInteractionRec();
            return collision;
        }

        public static void InteractRecWith(GameObject Obj)
        {   //this is the hero's interactionRec colliding with Obj
            //we know this is hero, and hero is in ActorState.Interact
            //Objects that can be interacted with from Land & Water


            //obj.group checks


            #region Chests

            if (Obj.group == ObjGroup.Chest)
            {
                //Reward the hero with chest contents
                if (Obj.type == ObjType.Dungeon_ChestKey)
                {
                    Functions_Particle.Spawn(ObjType.Particle_RewardKey, Pool.hero);
                    LevelSet.currentLevel.bigKey = true;
                    //setup dialog
                    Screens.Dialog.SetDialog(AssetsDialog.HeroGotKey);
                    ScreenManager.AddScreen(Screens.Dialog);
                }
                if (Obj.type != ObjType.Dungeon_ChestEmpty)
                {   //if the chest is not empty, play the reward animation
                    Assets.Play(Assets.sfxChestOpen);
                    Functions_GameObject.SetType(Obj, ObjType.Dungeon_ChestEmpty);
                    Functions_Particle.Spawn( //show the chest was opened
                        ObjType.Particle_Attention,
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y);
                    Functions_Actor.SetRewardState(Pool.hero);
                }
            }

            #endregion


            //Vendors
            else if (Obj.group == ObjGroup.Vendor)
            {
                Screens.Vendor.SetVendor(Obj.type);
                ScreenManager.AddScreen(Screens.Vendor);
            }


            //NPCs
            else if (Obj.group == ObjGroup.NPC)
            {   //based on obj.type, select dialog


                #region Story/Guide

                if(Obj.type == ObjType.NPC_Story)
                {   //figure out what part of the story the hero is at, pass this dialog
                    Screens.Dialog.SetDialog(AssetsDialog.Guide);
                    ScreenManager.AddScreen(Screens.Dialog);
                }

                #endregion


                #region Farmer

                else if (Obj.type == ObjType.NPC_Farmer)
                {
                    Screens.Dialog.SetDialog(AssetsDialog.Farmer_Setup);
                    ScreenManager.AddScreen(Screens.Dialog);
                }
                else if (Obj.type == ObjType.NPC_Farmer_Reward)
                {   //convert farmer to end state
                    Functions_GameObject.SetType(Obj, ObjType.NPC_Farmer_EndDialog);
                    //reward player
                    PlayerData.current.bombsCurrent += 10;
                    //play reward sfx
                    Assets.Play(Assets.sfxReward);
                    //display reward dialog
                    Screens.Dialog.SetDialog(AssetsDialog.Farmer_Reward);
                    ScreenManager.AddScreen(Screens.Dialog);
                }
                else if (Obj.type == ObjType.NPC_Farmer_EndDialog)
                {
                    Screens.Dialog.SetDialog(AssetsDialog.Farmer_EndDialog);
                    ScreenManager.AddScreen(Screens.Dialog);
                }

                #endregion


                #region Colliseum Judge

                else if (Obj.type == ObjType.Judge_Colliseum)
                {
                    Screens.Dialog.SetDialog(AssetsDialog.Colliseum_Judge);
                    ScreenManager.AddScreen(Screens.Dialog);
                }

                #endregion


                #region Brandy Ships Captain

                else if (Obj.type == ObjType.Wor_Boat_Captain_Brandy)
                {
                    Screens.Dialog.SetDialog(AssetsDialog.Guide); //fix this later
                    ScreenManager.AddScreen(Screens.Dialog);
                }

                #endregion

            }



            //obj.type checks

            //dungeon objects

            #region Torches, Levers, Switches

            if (Obj.type == ObjType.Dungeon_TorchUnlit)
            {   //light any unlit torch  //git lit *
                Functions_GameObject_Dungeon.LightTorch(Obj);
            }
            else if (Obj.type == ObjType.Dungeon_TorchLit)
            {   //unlight any lit torch
                Functions_GameObject_Dungeon.UnlightTorch(Obj);
            }
            else if (Obj.type == ObjType.Dungeon_LeverOff || Obj.type == ObjType.Dungeon_LeverOn)
            {   //activate all lever objects (including lever), call attention to change
                Functions_GameObject_Dungeon.ActivateLeverObjects();
                Functions_Particle.Spawn(
                        ObjType.Particle_Attention,
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y);
            }
            else if (Obj.type == ObjType.Dungeon_SwitchBlockBtn)
            {
                Functions_GameObject_Dungeon.FlipSwitchBlocks(Obj);
            }

            #endregion


            #region Boss Door

            else if (Obj.type == ObjType.Dungeon_DoorBoss)
            {
                if (LevelSet.currentLevel.bigKey)
                {   //hero must have dungeon key to open boss door
                    Functions_GameObject.SetType(Obj, ObjType.Dungeon_DoorOpen);
                    Assets.Play(Assets.sfxDoorOpen);
                    Functions_Particle.Spawn(
                        ObjType.Particle_Attention,
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y);
                }
                else
                {   //if hero doesn't have the bigKey, throw a dialog screen telling player this
                    Screens.Dialog.SetDialog(AssetsDialog.DoesNotHaveKey);
                    ScreenManager.AddScreen(Screens.Dialog);
                }
            }

            #endregion



            //world objects

            #region Signposts

            else if (Obj.type == ObjType.Dungeon_Signpost)
            {
                if (LevelSet.currentLevel.currentRoom.roomID == RoomID.Exit)
                {   
                    //setup signpost title
                    AssetsDialog.Signpost_ExitRoom[0].title = "...";
                    //populate signpost text with level data
                    AssetsDialog.Signpost_ExitRoom[0].text = 
                        "Dungeon: " + LevelSet.currentLevel.ID + ". " +
                        "Size: " + LevelSet.currentLevel.rooms.Count + " rooms.\n" +
                        "Head North 3 Rooms to find the map. Good luck!";
                }

                //read signpost from global dialogs
                Functions_GameObject_World.ReadSign(Obj);
            }

            #endregion


            #region World / Level Entrances

            else if (Obj.type == ObjType.Wor_Entrance_ForestDungeon)
            {   //give player choice to enter
                Screens.Dialog.SetDialog(AssetsDialog.Enter_ForestDungeon);
                ScreenManager.AddScreen(Screens.Dialog);
                SetFieldSpawnPos(Obj);
            }
            else if (Obj.type == ObjType.Wor_Entrance_MountainDungeon)
            {   //give player choice to enter
                Screens.Dialog.SetDialog(AssetsDialog.Enter_MountainDungeon);
                ScreenManager.AddScreen(Screens.Dialog);
                SetFieldSpawnPos(Obj);
            }
            else if (Obj.type == ObjType.Wor_Entrance_SwampDungeon)
            {   //give player choice to enter
                Screens.Dialog.SetDialog(AssetsDialog.Enter_SwampDungeon);
                ScreenManager.AddScreen(Screens.Dialog);
                SetFieldSpawnPos(Obj);
            }



            else if (Obj.type == ObjType.Wor_Entrance_Colliseum)
            {   //give player choice to enter
                Screens.Dialog.SetDialog(AssetsDialog.Enter_Colliseum);
                ScreenManager.AddScreen(Screens.Dialog);
                SetFieldSpawnPos(Obj);
            }

            #endregion


            #region Gate and House Doors

            else if (Obj.type == ObjType.Wor_Fence_Gate)
            {
                Functions_GameObject_World.OpenFencedGate(Obj);
            }
            else if (Obj.type == ObjType.Wor_Build_Door_Shut)
            {
                Functions_GameObject_World.OpenBuildingDoor(Obj);
            }

            #endregion


            #region Climbing Footholds

            else if (Obj.type == ObjType.Wor_MountainWall_Foothold
                || Obj.type == ObjType.Wor_MountainWall_Ladder)
            {   
                //hero isnt in climbing state yet, but will be at end of this routine
                //so this is the initial 'attach' to the wall from idle or falling
                if (Pool.hero.state != ActorState.Climbing)
                {   //help hero 'onto' the wall/foothold
                    Functions_Movement.Teleport(Pool.hero.compMove,
                        Pool.hero.compSprite.position.X, //keep X
                        Obj.compSprite.position.Y + 6); //align on Y
                    Functions_Component.Align(Pool.hero);
                    //this was done to ensure heros' center sprite pos
                    //overlaps with the foothold/ladder obj
                    //which is required to keep actor in climbing state each frame
                }

                Pool.hero.state = ActorState.Climbing;
                Pool.hero.stateLocked = true;
            }

            #endregion




            if (Pool.hero.swimming) { return; }

            //Objects that can only be interacted with from Land

            #region Carry-able Objects

            if(Obj.group == ObjGroup.Enemy)
            {   //deny link ability to pickup some enemies
                if (Obj.type == ObjType.Wor_SeekerExploder) { return; }
                Functions_Actor.Pickup(Obj, Pool.hero);
            }

            if (Obj.type == ObjType.Dungeon_Pot
                || Obj.type == ObjType.Wor_Pot
                || Obj.type == ObjType.Wor_Bush)
            {
                Functions_Actor.Pickup(Obj, Pool.hero);
            }

            #endregion


            #region Push-able Objects

            //if an obj is moveable, hero should be able to push it, right?
            if(Obj.compMove.moveable & Obj.compMove.grounded)
            {
                //some objects cannot be pushed
                if (Obj.type == ObjType.Dungeon_Pot
                    || Obj.type == ObjType.Wor_Pot
                    || Obj.type == ObjType.Dungeon_ChestKey)
                { return; }
                //all other objects can be pushed
                Functions_Actor.Grab(Obj, Pool.hero);
            }

            #endregion


        }

        public static void HandleDeath()
        {   //near the last frame of hero's death, create attention particles
            if (Pool.hero.compAnim.index == Pool.hero.compAnim.currentAnimation.Count - 2)
            {   //this event happens when hero falls to ground
                //goto next anim frame, this event is only processed once
                Pool.hero.compAnim.index++;
                //spawn particle to grab the player's attention
                Functions_Particle.Spawn(
                        ObjType.Particle_Attention,
                        Pool.hero.compSprite.position.X,
                        Pool.hero.compSprite.position.Y);
                //check to see if hero can save himself from death
                if(Functions_Bottle.HeroDeathCheck()) { } //true, hero self-rezs
                else
                {   //false, hero cannot self-rez and dies
                    DungeonRecord.beatDungeon = false;
                    Functions_Level.CloseLevel(ExitAction.Summary);
                }
            }
        }

        public static void SpawnInCurrentRoom()
        {   
            //teleport hero to level's spawn position (field or dungeon)
            if(LevelSet.currentLevel == LevelSet.field)
            {
                if (LevelSet.currentLevel.ID == LevelID.SkullIsland_ColliseumPit)
                {   //ALWAYS spawn hero in south position, but don't use spawnPos ref
                    Functions_Movement.Teleport(
                        Pool.hero.compMove,
                        LevelSet.field.currentRoom.center.X,
                        LevelSet.field.currentRoom.rec.Y
                        + LevelSet.field.currentRoom.rec.Height - 16 * 4);
                }
                else
                {   //place hero based on field spawnPos ref
                    Functions_Movement.Teleport(Pool.hero.compMove,
                        LevelSet.spawnPos_Field.X, LevelSet.spawnPos_Field.Y);
                }
            }
            else
            {
                Functions_Movement.Teleport(Pool.hero.compMove, 
                    LevelSet.spawnPos_Dungeon.X, LevelSet.spawnPos_Dungeon.Y);

                //pop an attention particle to draw player's attention
                Functions_Particle.Spawn(ObjType.Particle_Attention,
                    LevelSet.spawnPos_Dungeon.X, LevelSet.spawnPos_Dungeon.Y);
            }

            //kill movement, reset hero to proper starting state
            Functions_Movement.StopMovement(Pool.hero.compMove);
            ResetHero();

            
        }

        public static void ResetHero()
        {
            Pool.hero.compSprite.scale = 1.0f; //rescale hero to 100%
            Pool.hero.stateLocked = false;
            Pool.hero.state = ActorState.Idle;
            Pool.hero.carrying = false; //hero can't enter a room carrying room obj
            boomerangInPlay = false; //boomerang could of been lost in prev room
            SpawnPet();
        }

        public static void Update()
        {
            //match hero's rec to hero's sprite
            heroRec.X = (int)Pool.hero.compSprite.position.X - 8;
            heroRec.Y = (int)Pool.hero.compSprite.position.Y - 8;
            //match hero's shadow to hero's sprite
            //heroShadow.position.X = Pool.hero.compSprite.position.X;
            //heroShadow.position.Y = Pool.hero.compSprite.position.Y + 5;
            //Functions_Component.SetZdepth(heroShadow);

            //check the heroRec's collisions with Level rooms
            CheckRoomCollision();


            #region Handle hero vs pickup interactions

            for (i = 0; i < Pool.pickupCount; i++)
            {
                if (Pool.pickupPool[i].active)
                {   
                    if (Pool.hero.compCollision.rec.Intersects(Pool.pickupPool[i].compCollision.rec))
                    { Functions_Interaction.InteractActor(Pool.hero, Pool.pickupPool[i]); }
                }
            }

            #endregion


            //if the hero is under a roof, then hide all roofs
            if (underRoof)
            { Functions_GameObject_World.HideRoofs(); }
            //else game should display all roofs
            else { Functions_GameObject_World.ShowRoofs(); }

            //set hero's zoffset based on state
            if (Pool.hero.underwater)
            { Pool.hero.compSprite.zOffset = -30; } //sort under vines
            //sort normally
            else { Pool.hero.compSprite.zOffset = 0; }
        }

        public static void SetLoadout()
        {   //set the hero's loadout based on playerdata.current
            Pool.hero.item = PlayerData.current.currentItem;
            Pool.hero.weapon = PlayerData.current.currentWeapon;
            Pool.hero.armor = PlayerData.current.currentArmor;
            Pool.hero.equipment = PlayerData.current.currentEquipment;
            UnlockAll();
        }

        public static void SpawnPet()
        {   //spawn the hero's dog
            if (PlayerData.current.petType != MenuItemType.Unknown)
            {   
                Functions_GameObject.SetType(Pool.herosPet, ObjType.Pet_Dog);
                Pool.herosPet.compAnim.currentAnimation = AnimationFrames.Pet_Dog_Idle;

                Pool.herosPet.compCollision.blocking = false; //pet doesn't block
                Pool.herosPet.active = true; //pet is active
                Functions_Movement.Teleport(
                    Pool.herosPet.compMove,
                    Pool.hero.compMove.position.X,
                    Pool.hero.compMove.position.Y);
                Functions_Component.Align(Pool.herosPet);
            }
        }

        public static void UnlockAll()
        {   
            //if the cheat is not enabled, bail from method
            if (Flags.UnlockAll == false)
            {   //reset player's data to fresh instance
                PlayerData.current = new SaveData();
                return;
            }

            //this method unlocks all available items, weapons, equipment, armor

            //max hearts and magic
            PlayerData.current.heartsTotal = 9;
            Pool.hero.health = 9;
            PlayerData.current.magicMax = 9;
            PlayerData.current.magicCurrent = 9;
            //max arrows and bombs
            PlayerData.current.bombsCurrent = 99;
            PlayerData.current.arrowsCurrent = 99;
            //set bottle contents
            PlayerData.current.bottleA = MenuItemType.BottleHealth;
            PlayerData.current.bottleB = MenuItemType.BottleMagic;
            PlayerData.current.bottleC = MenuItemType.BottleFairy;
            //set items
            PlayerData.current.itemBoomerang = true;
            PlayerData.current.itemBow = true;
            //set magic
            PlayerData.current.magicFireball = true;
            PlayerData.current.magicBombos = true;
            PlayerData.current.magicBolt = true;
            //set weapons
            PlayerData.current.weaponNet = true;
            PlayerData.current.weaponShovel = true;
            //set armor
            PlayerData.current.armorCape = true;
            //set equipment
            PlayerData.current.equipmentRing = true;

            //we could set the pet here too, but we wont for now
            //PlayerData.current.petType = MenuItemType.Unknown;
            //PlayerData.current.petType = MenuItemType.PetStinkyDog;

            //setup testing enemy weapon/item
            PlayerData.current.enemyItem = MenuItemType.MagicBat;
            PlayerData.current.enemyWeapon = MenuItemType.WeaponFang;
        }





        public static void CheckAchievements(Achievements Achievement)
        {
            if(Achievement == Achievements.WallJumps)
            {   //check wall jumps
                if (PlayerData.current.recorded_wallJumps == 10)
                {
                    Screens.Dialog.SetDialog(AssetsDialog.Achievement_WallJumps_10);
                    ScreenManager.AddScreen(Screens.Dialog);
                }
                else if(PlayerData.current.recorded_wallJumps == 100)
                {
                    Screens.Dialog.SetDialog(AssetsDialog.Achievement_WallJumps_100);
                    ScreenManager.AddScreen(Screens.Dialog);
                }
            }
        }





    }
}