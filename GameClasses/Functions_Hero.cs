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

        //fields used in grab / push / pull
        public static Boolean grabbing = false;
        public static GameObject grabbedObj = null;




        //fields used in pickup / carry / throw
        public static Boolean carrying = false; //is hero holding pro above head?
        //allows for parallel picking & throwing due to seperation of projectiles
        public static Projectile carriedObj = null; //copy of obj picked up
        public static Projectile thrownObj = null; //copy of carried obj thrown


        




        static Functions_Hero()
        {
            interactionPoint = new Point(0, 0);
            heroRec = new Rectangle(0, 0, 16, 16);

        }





        
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
        {
            //starting Xpos + half width
            LevelSet.spawnPos_Field.X = Functions_Level.buildPosition.X + 40 * 16;
            //starting Ypos + field height - spawn offset
            LevelSet.spawnPos_Field.Y = Functions_Level.buildPosition.Y + 16 * 46 - 16 * 4;
        }
        
        public static void CheckRoomCollision()
        {
            //only process this method if the level screen is completely open
            if (Screens.Level.displayState != DisplayState.Opened) { return; }
            //otherwise it processes while level is closing causing bugs

            if (LevelSet.currentLevel.isField)
            {   
                //Handle hero leaving Field level
                if(heroRec.Intersects(LevelSet.currentLevel.currentRoom.rec) == false)
                {   
                    //if we're clipping, in a dev mode, don't exit the field level
                    if(Flags.Clipping & Flags.bootRoutine != BootRoutine.Game)
                    {
                        return; //allows for more editor control
                    }
                    else
                    {
                        //if we're clipping, and in game mode, this evaluates (making testing faster)
                        //if we're not clipping, this evaluates (game functions normally)


                        #region Based on Field, return to Overworld or other Field level

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

                        #endregion


                        //stop hero's movement upon exiting level
                        Functions_Movement.StopMovement(Pool.hero.compMove);
                    }
                }
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
                            if (carrying)
                            {   //silently destroy anything hero is carrying
                                carrying = false;
                                Functions_Pool.Release(carriedObj);
                            }

                            //transition between rooms, build room, set new room as visited
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
                if (grabbedObj.compCollision.rec.Contains(interactionPoint))
                {
                    //push grabbed obj in hero's move direction
                    Functions_Movement.Push(
                        grabbedObj.compMove,
                        Pool.hero.compMove.direction,
                        0.08f);
                    //spam play drag sfx here
                    Assets.Play(Assets.sfxDragObj);
                }
                else
                {   //release the grabbed object
                    grabbing = false;
                    grabbedObj = null;
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
        



        static MenuItemType itemSwap;
        public static void HandleDeath()
        {   //near the last frame of hero's death, create attention particles
            if (Pool.hero.compAnim.index == Pool.hero.compAnim.currentAnimation.Count - 2)
            {   //this event happens when hero falls to ground
                //goto next anim frame, this event is only processed once
                Pool.hero.compAnim.index++;
                //spawn particle to grab the player's attention
                Functions_Particle.Spawn(
                        ParticleType.Attention,
                        Pool.hero.compSprite.position.X,
                        Pool.hero.compSprite.position.Y);

                //store the hero's currently equipped item
                itemSwap = Pool.hero.item;
                //check to see if hero can save himself from death
                if (Functions_Bottle.HeroDeathCheck())
                {   //restore hero's last item (since we item swapped to self-rez)
                    Pool.hero.item = itemSwap;
                }
                else//false, hero cannot self-rez and dies
                { Functions_Level.CloseLevel(ExitAction.Summary); }
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
                Functions_Particle.Spawn(ParticleType.Attention,
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
            carrying = false; //hero can't enter a room carrying room obj
            boomerangInPlay = false; //boomerang could of been lost in prev room
            SpawnPet();
        }

        public static void SpawnPet()
        {   //spawn the hero's dog
            if (PlayerData.petType != MenuItemType.Unknown)
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

        public static void ExitDungeon()
        {
            if (Screens.Level.displayState == DisplayState.Opened)
            {   //clear any dungeon data upon exit
                if (LevelSet.dungeon.ID == LevelID.Forest_Dungeon)
                { PlayerData.ForestRecord.Clear(); }
                else if (LevelSet.dungeon.ID == LevelID.Mountain_Dungeon)
                { PlayerData.MountainRecord.Clear(); }
                else if (LevelSet.dungeon.ID == LevelID.Swamp_Dungeon)
                { PlayerData.SwampRecord.Clear(); }

                Assets.Play(Assets.sfxDoorOpen);
                //return hero to last field level
                Functions_Level.CloseLevel(ExitAction.Field);
            }
        }

        public static void CheckAchievements(Achievements Achievement)
        {
            if (Achievement == Achievements.WallJumps)
            {   //check wall jumps
                if (PlayerData.recorded_wallJumps == 10)
                {
                    Screens.Dialog.SetDialog(AssetsDialog.Achievement_WallJumps_10);
                    ScreenManager.AddScreen(Screens.Dialog);
                }
                else if (PlayerData.recorded_wallJumps == 100)
                {
                    Screens.Dialog.SetDialog(AssetsDialog.Achievement_WallJumps_100);
                    ScreenManager.AddScreen(Screens.Dialog);
                }
            }
        }


        

        public static void SetRewardState(ParticleType rewardType)
        {
            Pool.hero.state = ActorState.Reward;
            Pool.hero.stateLocked = true;
            Pool.hero.lockCounter = 0;
            Pool.hero.lockTotal = 30; //med pause
            Functions_Movement.StopMovement(Pool.hero.compMove);
            //spawn reward particle over hero's head
            Functions_Particle.Spawn(
                rewardType,
                Pool.hero.compSprite.position.X,
                Pool.hero.compSprite.position.Y - 14,
                Direction.Down);
        }







        //roomObj interaction

        public static void Pickup(GameObject Obj)
        {
            //decorate pickup
            Functions_Particle.Spawn(
                ParticleType.Attention,
                Obj.compSprite.position.X,
                Obj.compSprite.position.Y);

            //handle pickup effects
            if (Obj.type == ObjType.Wor_Bush)
            {   //spawn a stump obj at bush location
                Functions_GameObject.Spawn(
                    ObjType.Wor_Bush_Stump,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y,
                    Direction.Down);
            }

            //handle picking up roomObj as projectile dupe/fake
            carrying = true;

            //dupe roomObj values as projectile - inherit texture, animFrame
            carriedObj = Functions_Pool.GetProjectile();
            carriedObj.compSprite.texture = Obj.compSprite.texture;
            carriedObj.compAnim.currentAnimation = Obj.compAnim.currentAnimation;
            //set anim to first frame
            carriedObj.compSprite.currentFrame = carriedObj.compAnim.currentAnimation[0];


            //translate rat into the down animation for ease of use in code
            if (Obj.type == ObjType.Wor_Enemy_Rat)
            { carriedObj.compAnim.currentAnimation = AnimationFrames.Wor_Enemy_Rat_Down; }


            //setup rest of values as carriedObj
            carriedObj.type = ProjectileType.CarriedObject; //unqiue projectile
            carriedObj.compSprite.zOffset = 32; //sort over hero's head
            carriedObj.lifetime = 255; //in frames
            carriedObj.lifeCounter = 0; //new pro
            carriedObj.compAnim.index = 0; //reset to 1st frame
            carriedObj.caster = Pool.hero; //set caster to hero
            carriedObj.compCollision.blocking = false;
            carriedObj.direction = Direction.Down;

            //release roomObj (make inactive)
            Obj.active = false;

            //put actor into pickup state
            Pool.hero.state = ActorState.Pickup;
            Pool.hero.stateLocked = true;
            Pool.hero.lockTotal = 10;
            Functions_Actor.SetAnimationGroup(Pool.hero);
            Assets.Play(Assets.sfxActorLand); //temp sfx
        }

        public static void Throw()
        {
            if (carrying)
            {
                //put hero into throw state
                carrying = false;
                Pool.hero.state = ActorState.Throw;
                Pool.hero.stateLocked = true;
                Pool.hero.lockTotal = 10;
                Functions_Actor.SetAnimationGroup(Pool.hero);

                //resolve hero.direction to cardinal - no diagonal throwing
                Pool.hero.direction =
                    Functions_Direction.GetCardinalDirection_LeftRight(Pool.hero.direction);


                #region Setup hero's thrown projectile object

                thrownObj = Functions_Pool.GetProjectile();
                thrownObj.type = ProjectileType.ThrownObject;
                thrownObj.caster = Pool.hero;
                //inherit carriedPro's textures and animFrames
                thrownObj.compSprite.texture = carriedObj.compSprite.texture;
                thrownObj.compAnim.currentAnimation = carriedObj.compAnim.currentAnimation;
                //set anim to first frame
                thrownObj.compSprite.currentFrame = carriedObj.compAnim.currentAnimation[0];
                Functions_Pool.Release(carriedObj); //we are done with carried obj

                //complete thrown projectile setup
                thrownObj.compSprite.zOffset = 32; //sort over hero's head
                thrownObj.direction = Direction.Down;
                thrownObj.compSprite.rotation = Rotation.None;
                thrownObj.lifetime = 20; //in frames
                thrownObj.lifeCounter = 0; //new pro

                thrownObj.compMove.direction = Pool.hero.direction;
                thrownObj.compMove.grounded = false; //obj is airborne
                thrownObj.compMove.friction = 0.984f; //some air friction

                thrownObj.compCollision.offsetX = -6; thrownObj.compCollision.offsetY = -8;
                thrownObj.compCollision.rec.Width = 12; thrownObj.compCollision.rec.Height = 12;

                #endregion


                #region Place thrown projectile outside of hero's hitbox

                //this is tailored exactly to hero's hitbox
                if (Pool.hero.direction == Direction.Down)
                {   //too far and we throw thru thin horizontal hitboxes
                    Functions_Movement.Teleport(thrownObj.compMove,
                        Pool.hero.compCollision.rec.Center.X,
                        Pool.hero.compCollision.rec.Y + Pool.hero.compCollision.rec.Height + 8);
                }
                else if (Pool.hero.direction == Direction.Up)
                {   //too far and we throw thru thin horizontal hitboxes
                    Functions_Movement.Teleport(thrownObj.compMove,
                        Pool.hero.compCollision.rec.Center.X,
                        Pool.hero.compCollision.rec.Y - 10);
                }
                else if (Pool.hero.direction == Direction.Right)
                {   //too far and we throw thru thin vertical hitboxes
                    Functions_Movement.Teleport(thrownObj.compMove,
                        Pool.hero.compCollision.rec.X + Pool.hero.compCollision.rec.Width + 7,
                        Pool.hero.compCollision.rec.Center.Y - 4);
                }
                else if (Pool.hero.direction == Direction.Left)
                {   //too far and we throw thru thin vertical hitboxes
                    Functions_Movement.Teleport(thrownObj.compMove,
                        Pool.hero.compCollision.rec.X - 7,
                        Pool.hero.compCollision.rec.Center.Y - 4);
                }

                #endregion


                //push thrown obj projectile
                Functions_Component.Align(thrownObj); //align components
                Functions_Movement.Push(thrownObj.compMove, Pool.hero.direction, 5.0f);
                thrownObj.compMove.moving = true; //moves

                //never check thrownPro vs hero.hitBox - no collisions to worry about
                Assets.Play(Assets.sfxActorFall); //play throw sfx
                Functions_Particle.SpawnPushFX(Pool.hero.compMove, Pool.hero.direction);
            }
        }

        public static void Grab(GameObject Obj)
        {
            grabbing = true;
            grabbedObj = Obj;
        }






        //hero's special interact() method based on interaction point
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
                    SetRewardState(ParticleType.RewardKey);
                   //alter player data
                    LevelSet.currentLevel.bigKey = true;
                    //setup dialog
                    Screens.Dialog.SetDialog(AssetsDialog.HeroGotKey);
                    ScreenManager.AddScreen(Screens.Dialog);
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

                if (Obj.type == ObjType.NPC_Story)
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
                    PlayerData.bombsCurrent += 10;
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
                    //for now, brandy offers no useful advice
                    Screens.Dialog.SetDialog(AssetsDialog.Brandy_Default);

                    //this advice is based on the current room.id,
                    //and only overworld field levels have dialogs

                    if (LevelSet.currentLevel.currentRoom.roomID == RoomID.DeathMountain_MainEntrance)
                    { Screens.Dialog.SetDialog(AssetsDialog.Brandy_MountainEntrance); }

                    else if (LevelSet.currentLevel.currentRoom.roomID == RoomID.ForestIsland_MainEntrance)
                    { Screens.Dialog.SetDialog(AssetsDialog.Brandy_ForestEntrance); }

                    //this is a hack for 0.77 - to be fixed in future commits
                    else if (LevelSet.currentLevel.currentRoom.roomID == RoomID.SkullIsland_ShadowKing)
                    { Screens.Dialog.SetDialog(AssetsDialog.Brandy_SwampEntrance); }

                    //kick the dialog to the player
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
                        ParticleType.Attention,
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
                        ParticleType.Attention,
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


            #region House Doors

            else if (Obj.type == ObjType.Wor_Build_Door_Shut)
            {
                Functions_GameObject_World.OpenHouseDoor(Obj);
            }

            #endregion


            #region Climbing Footholds

            else if (Obj.type == ObjType.Wor_MountainWall_Foothold
                || Obj.type == ObjType.Wor_MountainWall_Ladder)
            {   //only link and blob have anim frames for climbing
                if (Pool.hero.type == ActorType.Hero || Pool.hero.type == ActorType.Blob)
                {   //hero isnt in climbing state yet, but will be at end of this routine
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
            }

            #endregion




            //dev/editor/cheat interactions

            #region Dungeon Exit Obj

            else if (Obj.type == ObjType.Dungeon_Exit)
            {   //movement has already been stopped, hero is interacting
                ExitDungeon();
            }

            #endregion





            if (Pool.hero.swimming) { return; }

            //Objects that can only be interacted with from Land, as link or blob
            if (Pool.hero.type == ActorType.Hero || Pool.hero.type == ActorType.Blob)
            {

                #region Carry-able Objects

                if (Obj.group == ObjGroup.Enemy)
                {   //deny link ability to pickup some enemies
                    if (Obj.type == ObjType.Wor_SeekerExploder) { return; }
                    Pickup(Obj);
                }

                if (Obj.type == ObjType.Dungeon_Pot
                    || Obj.type == ObjType.Wor_Pot
                    || Obj.type == ObjType.Wor_Bush)
                {
                    Pickup(Obj);
                }

                #endregion


                #region Push-able Objects

                //if an obj is moveable, hero should be able to push it, right?
                if (Obj.compMove.moveable & Obj.compMove.grounded)
                {
                    //some objects cannot be pushed
                    if (Obj.type == ObjType.Dungeon_Pot
                        || Obj.type == ObjType.Wor_Pot
                        || Obj.type == ObjType.Dungeon_ChestKey)
                    { return; }
                    //all other objects can be pushed
                    Grab(Obj);
                }

                #endregion

            }
        }









        public static void Update()
        {
            //match hero's rec to hero's sprite
            heroRec.X = (int)Pool.hero.compSprite.position.X - 8;
            heroRec.Y = (int)Pool.hero.compSprite.position.Y - 8;

            //check the heroRec's collisions with Level rooms
            CheckRoomCollision();


            #region Handle hero vs pickup interactions

            for (i = 0; i < Pool.pickupCount; i++)
            {
                if (Pool.pickupPool[i].active)
                {   
                    if (Pool.hero.compCollision.rec.Intersects(Pool.pickupPool[i].compCollision.rec))
                    {
                        Functions_Pickup.HandleEffect(Pool.pickupPool[i]);
                    }
                }
            }

            #endregion


            #region Hero vs Roofs

            //if the hero is under a roof, then hide all roofs
            if (underRoof)
            { Functions_GameObject_World.HideRoofs(); }

            //editor connection here - this can become a menu option
            else if(Flags.IgnoreRoofTiles)
            { Functions_GameObject_World.HideRoofs(); }

            //else game should display all roofs
            else { Functions_GameObject_World.ShowRoofs(); }

            #endregion


            #region Hero sorting in water/land

            //set hero's zoffset based on state
            if (Pool.hero.underwater)
            { Pool.hero.compSprite.zOffset = -30; } //sort under vines
            //sort normally
            else { Pool.hero.compSprite.zOffset = 0; }

            #endregion


            #region Hero carrying obj

            if (carrying)
            {   //hide the carried obj projectile from initial drawing
                carriedObj.compSprite.visible = false;
                //we will draw it later, after hero sprite, locked on link's head
            }

            #endregion



            //resets

            #region Hide Reward Sprite if Hero is not-statelocked

            if(Pool.hero.stateLocked == false)
            {   //if hero has unlocked, then hide reward sprite
                //rewardSprite.visible = false;
            }

            #endregion


        }







        public static void Draw()
        {


            #region Align and Draw Carried Object Projectile

            if(carrying)
            {   //align carried obj to hero's head after hero has been drawn
                //this is based on post-collision hitbox pos, and ensures carried obj
                //doesnt lag behind hero, despite speed and magnitude changes

                if (Pool.hero.swimming)
                {
                    if (Pool.hero.underwater)
                    {   //place heldObj above head, underwater
                        Functions_Movement.Teleport(
                            carriedObj.compMove,
                            Pool.hero.compCollision.rec.Center.X,
                            Pool.hero.compCollision.rec.Center.Y - 5);
                    }
                    else
                    {   //place heldObj above head, swimming
                        Functions_Movement.Teleport(
                            carriedObj.compMove,
                            Pool.hero.compCollision.rec.Center.X,
                            Pool.hero.compCollision.rec.Center.Y - 8);
                    }

                }
                else
                {   //place heldObj above head, on land
                    Functions_Movement.Teleport(
                        carriedObj.compMove,
                        Pool.hero.compCollision.rec.Center.X,
                        Pool.hero.compCollision.rec.Center.Y - 13);
                }

                //align pro components
                Functions_Component.Align(
                    carriedObj.compMove, 
                    carriedObj.compSprite, 
                    carriedObj.compCollision);

                //enable draw, then draw carried obj
                carriedObj.compSprite.visible = true;
                Functions_Draw.Draw(carriedObj);
            }

            #endregion

            


            //we could handle future pet related drawing here too

        }

    }
}