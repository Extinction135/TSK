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
        static Boolean collision = false;
        public static ComponentCollision interactionRec = new ComponentCollision();



        public static void CheckRoomCollision()
        {

            #region Handle Hero transferring between Level.Rooms

            for (i = 0; i < Level.rooms.Count; i++)
            {   //if the current room is not the room we are checking against, then continue
                if (Functions_Level.currentRoom != Level.rooms[i])
                {   //if heroRec collides with room rec, set it as currentRoom, build room
                    if (Pool.heroRec.Intersects(Level.rooms[i].rec))
                    {
                        Functions_Level.currentRoom = Level.rooms[i];
                        Level.rooms[i].visited = true;
                        Functions_Room.BuildRoom(Level.rooms[i]);
                        Functions_Room.FinishRoom(Level.rooms[i]);
                        if (Functions_Level.currentRoom.type == RoomType.Boss)
                        {   //if hero just entered the boss room, play the boss intro & music
                            Assets.Play(Assets.sfxBossIntro);
                            Functions_Music.PlayMusic(Music.Boss);
                        }
                    }
                }
            }

            #endregion


            #region Track Doors that Hero has visited

            for (i = 0; i < Level.doors.Count; i++)
            {   //check heroRec collision against Level.doors
                if (Pool.heroRec.Intersects(Level.doors[i].rec))
                {   //track doors hero has visited
                    Level.doors[i].visited = true;
                    if (Level.doors[i].type == DoorType.Open)
                    {   //set the current room's spawnPos to the last open door hero collided with
                        Functions_Level.currentRoom.spawnPos.X = Level.doors[i].rec.X + 8;
                        Functions_Level.currentRoom.spawnPos.Y = Level.doors[i].rec.Y + 8;
                    }
                }
            }

            #endregion


            #region Open/Close Doors for Hero

            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active) //roomObj must be active
                {
                    if (Pool.roomObjPool[i].type == ObjType.DoorOpen)
                    {   //set open/bombed doors to blocking or non-blocking
                        Pool.roomObjPool[i].compCollision.blocking = true; //set door blocking

                        //compare hero to door positions, unblock door if hero is close enough
                        if (Math.Abs(Pool.hero.compSprite.position.X - Pool.roomObjPool[i].compSprite.position.X) < 18)
                        {   //compare hero to door sprite positions, unblock door if hero is close enough
                            if (Math.Abs(Pool.hero.compSprite.position.Y - Pool.roomObjPool[i].compSprite.position.Y) < 18)
                            { Pool.roomObjPool[i].compCollision.blocking = false; }
                        }
                        //do this for hero's pet as well
                        if (Math.Abs(Pool.herosPet.compSprite.position.X - Pool.roomObjPool[i].compSprite.position.X) < 18)
                        {
                            if (Math.Abs(Pool.herosPet.compSprite.position.Y - Pool.roomObjPool[i].compSprite.position.Y) < 18)
                            { Pool.roomObjPool[i].compCollision.blocking = false; }
                        }
                    }
                }
            }

            #endregion

        }



        public static void ClearInteractionRec()
        {   //move the interaction rec offscreen
            interactionRec.rec.X = -1000;
            interactionRec.rec.Y = -1000;
        }

        public static void SetInteractionRec()
        {
            //set the interaction rec to the hero's position
            interactionRec.rec.X = (int)Pool.hero.compSprite.position.X - 4;
            interactionRec.rec.Y = (int)Pool.hero.compSprite.position.Y - 4;
            //offset the rec based on the direction hero is facing
            if (Pool.hero.direction == Direction.Up)
            {
                interactionRec.rec.Width = 8; interactionRec.rec.Height = 4;
                interactionRec.rec.Y -= 1;
            }
            else if (Pool.hero.direction == Direction.Down)
            {
                interactionRec.rec.Width = 8; interactionRec.rec.Height = 4;
                interactionRec.rec.Y += 14;
            }
            else if (
                Pool.hero.direction == Direction.Left ||
                Pool.hero.direction == Direction.UpLeft ||
                Pool.hero.direction == Direction.DownLeft)
            {
                interactionRec.rec.Width = 4; interactionRec.rec.Height = 8;
                interactionRec.rec.Y += 4; interactionRec.rec.X -= 7;
            }
            else if (
                Pool.hero.direction == Direction.Right ||
                Pool.hero.direction == Direction.UpRight ||
                Pool.hero.direction == Direction.DownRight)
            {
                interactionRec.rec.Width = 4; interactionRec.rec.Height = 8;
                interactionRec.rec.Y += 4; interactionRec.rec.X += 11;
            }
        }

        public static Boolean CheckInteractionRecCollisions()
        {   //set the interaction rec to the hero's position + direction
            SetInteractionRec();
            collision = false;
            //check to see if the interactionRec collides with any gameObjects
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active)
                {
                    if (interactionRec.rec.Intersects(Pool.roomObjPool[i].compCollision.rec))
                    {
                        Functions_Movement.StopMovement(Pool.hero.compMove);
                        Pool.hero.stateLocked = true;
                        Pool.hero.lockTotal = 10; //required to show the pickup animation
                        collision = true;
                        //handle the hero interaction, may overwrites hero.lockTotal
                        InteractRecWith(Pool.roomObjPool[i]);
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

            #region Chests

            if (Obj.group == ObjGroup.Chest)
            {

                #region Reward the hero with chest contents

                if (Obj.type == ObjType.ChestKey)
                {
                    Functions_Entity.SpawnEntity(ObjType.ParticleRewardKey, Pool.hero);
                    Level.bigKey = true;
                    if (Flags.ShowDialogs)
                    { ScreenManager.AddScreen(new ScreenDialog(Functions_Dialog.HeroGotKey)); }
                }
                else if (Obj.type == ObjType.ChestMap)
                {
                    Functions_Entity.SpawnEntity(ObjType.ParticleRewardMap, Pool.hero);
                    Level.map = true;
                    if (Flags.ShowDialogs)
                    { ScreenManager.AddScreen(new ScreenDialog(Functions_Dialog.HeroGotMap)); }
                }

                #endregion


                if (Obj.type != ObjType.ChestEmpty)
                {   //if the chest is not empty, play the reward animation
                    Assets.Play(Assets.sfxChestOpen);
                    Functions_GameObject.SetType(Obj, ObjType.ChestEmpty);
                    Functions_Entity.SpawnEntity( //show the chest was opened
                        ObjType.ParticleAttention,
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y,
                        Direction.None);
                    Functions_Actor.SetRewardState(Pool.hero);
                }
            }

            #endregion


            #region Vendors

            else if (Obj.group == ObjGroup.Vendor)
            {   //some vendors do not sell items, so check vendor types
                if (Obj.type == ObjType.VendorStory) //for now this is default dialog
                {   //figure out what part of the story the hero is at, pass this dialog
                    ScreenManager.AddScreen(new ScreenDialog(Functions_Dialog.Guide));
                }
                //check to make sure the obj isn't a vendor advertisement
                else if (Obj.type != ObjType.VendorAdvertisement)
                { ScreenManager.AddScreen(new ScreenVendor(Obj)); }
                //vendor ad objects are ignored
            }

            #endregion


            #region Boss Door

            else if (Obj.type == ObjType.DoorBoss)
            {
                if (Level.bigKey)
                {   //hero must have dungeon key to open boss door
                    Functions_GameObject.SetType(Obj, ObjType.DoorOpen);
                    Assets.Play(Assets.sfxDoorOpen);
                    Functions_Entity.SpawnEntity(
                        ObjType.ParticleAttention,
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y,
                        Direction.None);
                }
                else
                {   //if hero doesn't have the bigKey, throw a dialog screen telling player this
                    if (Flags.ShowDialogs)
                    { ScreenManager.AddScreen(new ScreenDialog(Functions_Dialog.DoesNotHaveKey)); }
                }
            }

            #endregion


            #region Dungeon Objects

            else if (Obj.type == ObjType.Pot)
            {
                //put hero into pickup state
                Pool.hero.state = ActorState.Pickup;
                Pool.hero.stateLocked = true;
                Pool.hero.lockTotal = 10;

                Pool.hero.carrying = true; //set carrying state
                Pool.hero.carryingObj = Obj; //set obj ref
                Obj.compSprite.zOffset = +256; //sort above hero
                Obj.compCollision.blocking = false; //prevent hero/obj collisions
                //Functions_Movement.StopMovement(Pool.hero.compMove);
            }

            else if (Obj.type == ObjType.TorchUnlit)
            {   //light the unlit torch
                Functions_GameObject.SetType(Obj, ObjType.TorchLit);
                Functions_Entity.SpawnEntity(ObjType.ParticleFire,
                    Obj.compSprite.position.X + 0,
                    Obj.compSprite.position.Y - 7,
                    Direction.None);
                Assets.Play(Assets.sfxLightFire);

                //check to see if lighting this torch can solve the room's puzzle
                if(Functions_Level.currentRoom.puzzleType == PuzzleType.Torches)
                {   //if the current room's puzzle type is Torches, check to see how many have been lit
                    if(Functions_RoomObject.CountTorches())
                    {   //enough torches have been lit to unlock this room / solve puzzle
                        Assets.Play(Assets.sfxReward); //this should be secret sfx later
                        //open all the trap doors in the room
                        Functions_RoomObject.OpenTrapDoors();
                    }
                }
            }
            else if (Obj.type == ObjType.LeverOff || Obj.type == ObjType.LeverOn)
            {   //activate all lever objects (including lever), call attention to change
                Functions_RoomObject.ActivateLeverObjects();
                Functions_Entity.SpawnEntity(
                        ObjType.ParticleAttention,
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y,
                        Direction.None);
            }
            else if (Obj.type == ObjType.SwitchBlockBtn)
            {
                Functions_RoomObject.FlipSwitchBlocks(Obj.compSprite.position);
            }

            #endregion

        }



        public static void CollideWith(Actor Actor)
        {   //this is an Actor bumping into/overlapping with hero
            if (Actor.type == ActorType.Fairy)
            {   //kill fairy, fill hero's health to max
                Functions_Actor.SetDeathState(Actor);
                Pool.hero.health = PlayerData.current.heartsTotal;
            }
        }

        public static void Interact(GameObject Obj)
        {

            #region Pickups

            if (Obj.group == ObjGroup.Pickup)
            {   //only the hero can pickup hearts or rupees
                if (Obj.type == ObjType.PickupHeart)
                { Pool.hero.health++; Assets.Play(Assets.sfxHeartPickup); }
                else if (Obj.type == ObjType.PickupRupee)
                { PlayerData.current.gold++; Assets.Play(Assets.sfxGoldPickup); }
                else if (Obj.type == ObjType.PickupMagic)
                { PlayerData.current.magicCurrent++; Assets.Play(Assets.sfxHeartPickup); }
                else if (Obj.type == ObjType.PickupArrow)
                { PlayerData.current.arrowsCurrent++; Assets.Play(Assets.sfxHeartPickup); }
                else if (Obj.type == ObjType.PickupBomb)
                { PlayerData.current.bombsCurrent++; Assets.Play(Assets.sfxHeartPickup); }
                Obj.lifetime = 1; Obj.lifeCounter = 2; //end the items life
                return;
            }

            #endregion


            #region Doors

            else if (Obj.group == ObjGroup.Door)
            {   //handle hero interaction with exit door
                if (Obj.type == ObjType.Exit)
                {
                    if (Functions_Level.levelScreen.displayState == DisplayState.Opened)
                    {   //if dungeon screen is open, close it, perform interaction ONCE
                        DungeonRecord.beatDungeon = false;
                        //is hero exiting a dungeon?
                        if (Level.type == LevelType.Castle)
                        { Functions_Level.CloseLevel(ExitAction.ExitDungeon); }
                        else //return to the overworld screen
                        { Functions_Level.CloseLevel(ExitAction.Overworld); }
                        Assets.Play(Assets.sfxDoorOpen);
                    }
                    //stop movement, prevents overlap with exit
                    Functions_Movement.StopMovement(Pool.hero.compMove);
                }
                //center Hero to Door, while still allowing him to pass thru
                if (Obj.direction == Direction.Up || Obj.direction == Direction.Down)
                {   //gradually center hero to door
                    Pool.hero.compMove.magnitude.X = (Obj.compSprite.position.X - Pool.hero.compMove.position.X) * 0.11f;
                    //if hero is close to center of door, snap/lock hero to center of door
                    if (Math.Abs(Pool.hero.compSprite.position.X - Obj.compSprite.position.X) < 2)
                    { Pool.hero.compMove.newPosition.X = Obj.compSprite.position.X; }
                }
                else
                {   //gradually center hero to door
                    Pool.hero.compMove.magnitude.Y = (Obj.compSprite.position.Y - Pool.hero.compMove.position.Y) * 0.11f;
                    //if hero is close to center of door, snap/lock hero to center of door
                    if (Math.Abs(Pool.hero.compSprite.position.Y - Obj.compSprite.position.Y) < 2)
                    { Pool.hero.compMove.newPosition.Y = Obj.compSprite.position.Y; }
                }
                return;
            }

            #endregion


            #region PitTrap

            if (Obj.type == ObjType.PitTrap)
            {   //if hero collides with a PitTrapReady, it starts to open
                Functions_GameObject.SetType(Obj, ObjType.PitAnimated);
                Assets.Play(Assets.sfxShatter); //play collapse sound
                                                //draw attention to the collapsed floor
                Functions_Entity.SpawnEntity(ObjType.ParticleAttention,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y,
                    Direction.Down);
                Functions_Entity.SpawnEntity(ObjType.ParticleSmokePuff,
                    Obj.compSprite.position.X + 4,
                    Obj.compSprite.position.Y - 8,
                    Direction.Down);
                //create pit teeth over new pit obj
                Functions_RoomObject.SpawnRoomObj(ObjType.PitTop,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y,
                    Direction.Down);
                Functions_RoomObject.SpawnRoomObj(ObjType.PitBottom,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y,
                    Direction.Down);
                return; //bail from interaction check
            }

            #endregion


            #region SwitchBlock UP

            else if (Obj.type == ObjType.SwitchBlockUp)
            {   //if hero isnt moving and is colliding with up block, convert up to down
                if (Pool.hero.compMove.newPosition == Pool.hero.compMove.position)
                { Functions_GameObject.SetType(Obj, ObjType.SwitchBlockDown); }
            }

            #endregion


            #region FloorSwitch

            else if (Obj.type == ObjType.Switch)
            {   //convert switch off, play switch soundFx
                Functions_GameObject.SetType(Obj, ObjType.SwitchOff);
                //grab the player's attention
                Functions_Entity.SpawnEntity(ObjType.ParticleAttention,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y,
                    Direction.Down);
                //open all the trap doors in the room
                Functions_RoomObject.OpenTrapDoors();
            }

            #endregion

        }



        public static void SetLoadout()
        {   //set the hero's loadout based on playerdata.current
            Pool.hero.weapon = MenuItemType.Unknown;
            Pool.hero.item = MenuItemType.Unknown;
            Pool.hero.armor = MenuItemType.Unknown;
            Pool.hero.equipment = MenuItemType.Unknown;

            //sanitize playerdata.current to within expected values
            if (PlayerData.current.currentItem > 9) { PlayerData.current.currentItem = 0; }
            if (PlayerData.current.currentWeapon > 4) { PlayerData.current.currentWeapon = 0; }
            if (PlayerData.current.currentArmor > 4) { PlayerData.current.currentArmor = 0; }
            if (PlayerData.current.currentEquipment > 4) { PlayerData.current.currentEquipment = 0; }

            //set hero's item
            if (PlayerData.current.currentItem == 0)
            { Pool.hero.item = MenuItemType.ItemBomb; }
            else if (PlayerData.current.currentItem == 1)
            { Pool.hero.item = MenuItemType.ItemBoomerang; }
            //set item based on bottle contents
            else if (PlayerData.current.currentItem == 2)
            { Functions_Bottle.LoadBottle(PlayerData.current.bottleA); }
            else if (PlayerData.current.currentItem == 3)
            { Functions_Bottle.LoadBottle(PlayerData.current.bottleB); }
            else if (PlayerData.current.currentItem == 4)
            { Functions_Bottle.LoadBottle(PlayerData.current.bottleC); }

            //magic items
            else if (PlayerData.current.currentItem == 5)
            { Pool.hero.item = MenuItemType.MagicFireball; }
            else if (PlayerData.current.currentItem == 6)
            { Pool.hero.item = MenuItemType.Unknown; }
            else if (PlayerData.current.currentItem == 7)
            { Pool.hero.item = MenuItemType.Unknown; }
            else if (PlayerData.current.currentItem == 8)
            { Pool.hero.item = MenuItemType.Unknown; }
            else if (PlayerData.current.currentItem == 9)
            { Pool.hero.item = MenuItemType.Unknown; }

            //set hero's weapon
            if (PlayerData.current.currentWeapon == 0)
            { Pool.hero.weapon = MenuItemType.WeaponSword; }
            else if (PlayerData.current.currentWeapon == 1)
            { Pool.hero.weapon = MenuItemType.WeaponBow; }
            else if (PlayerData.current.currentWeapon == 2)
            { Pool.hero.weapon = MenuItemType.WeaponNet; }
            else if (PlayerData.current.currentWeapon == 3)
            { Pool.hero.weapon = MenuItemType.Unknown; }
            else if (PlayerData.current.currentWeapon == 4)
            { Pool.hero.weapon = MenuItemType.Unknown; }

            //set hero's armor
            if (PlayerData.current.currentArmor == 0)
            { Pool.hero.armor = MenuItemType.ArmorCloth; }
            else if (PlayerData.current.currentArmor == 1)
            { Pool.hero.armor = MenuItemType.ArmorChest; }
            else if (PlayerData.current.currentArmor == 2)
            { Pool.hero.armor = MenuItemType.ArmorCape; }
            else if (PlayerData.current.currentArmor == 3)
            { Pool.hero.armor = MenuItemType.ArmorRobe; }
            else if (PlayerData.current.currentArmor == 4)
            { Pool.hero.armor = MenuItemType.Unknown; }

            //set hero's equipment
            if (PlayerData.current.currentEquipment == 0)
            { Pool.hero.equipment = MenuItemType.EquipmentRing; }
            else if (PlayerData.current.currentEquipment == 1)
            { Pool.hero.equipment = MenuItemType.EquipmentPearl; }
            else if (PlayerData.current.currentEquipment == 2)
            { Pool.hero.equipment = MenuItemType.EquipmentNecklace; }
            else if (PlayerData.current.currentEquipment == 3)
            { Pool.hero.equipment = MenuItemType.EquipmentGlove; }
            else if (PlayerData.current.currentEquipment == 4)
            { Pool.hero.equipment = MenuItemType.EquipmentPin; }
        }

        public static void SetPet()
        {   //set the hero's pet to be active or inactive
            Pool.herosPet.active = PlayerData.current.hasPet;
            Functions_ActorAnimationList.SetPetAnimList();

            //set the pet's dash sound
            if (PlayerData.current.petType == MenuItemType.PetChicken)
            { Pool.herosPet.sfxDash = Assets.sfxPetChicken; }
            else if (PlayerData.current.petType == MenuItemType.PetStinkyDog)
            { Pool.herosPet.sfxDash = Assets.sfxPetDog; }
        }

        public static void HandleDeath()
        {   //near the last frame of hero's death, create attention particles
            if (Pool.hero.compAnim.index == Pool.hero.compAnim.currentAnimation.Count - 2)
            {   //this event happens when hero falls to ground
                //goto next anim frame, this event is only processed once
                Pool.hero.compAnim.index++;
                //spawn particle to grab the player's attention
                Functions_Entity.SpawnEntity(
                        ObjType.ParticleAttention,
                        Pool.hero.compSprite.position.X,
                        Pool.hero.compSprite.position.Y,
                        Direction.None);
                //check to see if hero can use any bottle to heal/self-rez
                if (Functions_Bottle.CheckBottleUponDeath(1, PlayerData.current.bottleA)) { }
                else if (Functions_Bottle.CheckBottleUponDeath(2, PlayerData.current.bottleB)) { }
                else if (Functions_Bottle.CheckBottleUponDeath(3, PlayerData.current.bottleC)) { }
                else
                {   //player has died, failed the dungeon
                    DungeonRecord.beatDungeon = false;
                    Functions_Level.CloseLevel(ExitAction.Summary);
                }
            }
        }

        public static void SpawnInCurrentRoom()
        {   //teleport hero to currentRoom's spawn position
            Functions_Movement.Teleport(Pool.hero.compMove,
                Functions_Level.currentRoom.spawnPos.X,
                Functions_Level.currentRoom.spawnPos.Y);
            Functions_Movement.StopMovement(Pool.hero.compMove);
            Pool.hero.compSprite.scale = 1.0f; //rescale hero to 100%
            Pool.hero.state = ActorState.Idle;
            Pool.hero.stateLocked = false;
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
            TeleportPet(); //teleport pet to hero's position
        }

        public static void TeleportPet()
        {
            if (PlayerData.current.hasPet == false)
            {   //hide pet off screen
                Functions_Movement.Teleport(Pool.herosPet.compMove, -100, -100);
                return;
            }
            //else, teleport pet to hero's position
            Functions_Movement.Teleport(Pool.herosPet.compMove,
                Pool.hero.compMove.newPosition.X,
                Pool.hero.compMove.newPosition.Y);
            Functions_Movement.StopMovement(Pool.herosPet.compMove);
            Pool.herosPet.compSprite.scale = 1.0f; //rescale hero to 100%
        }

    }
}