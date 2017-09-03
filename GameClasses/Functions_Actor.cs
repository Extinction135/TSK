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
    public static class Functions_Actor
    {

        public static void SpawnActor(ActorType Type, Vector2 Pos)
        {
            SpawnActor(Type, Pos.X, Pos.Y);
        }

        public static void SpawnActor(ActorType Type, float X, float Y)
        {   //grab an actor, place at X, Y position
            Actor actor = Functions_Pool.GetActor();
            if (actor != null)
            {
                SetType(actor, Type);
                Functions_Movement.Teleport(actor.compMove, X, Y);
            }
        }

        public static void BottleActor(Actor Actor)
        {   //can we bottle this actor?
            if (Actor.type == ActorType.Boss || Actor.type == ActorType.Hero)
            {   //pop cant bottle dialog
                if (Flags.ShowDialogs)
                { ScreenManager.AddScreen(new ScreenDialog(Dialog.BottleCant)); }
                return;
            }
            else
            {   //determine what type of actor we're attempting to bottle
                byte value = 5; //defaults to fairy value
                if (Actor.type == ActorType.Fairy) { value = 5; }
                else if (Actor.type == ActorType.Blob) { value = 6; }
                //determine if hero has an empty bottle to put this actor into
                Boolean captured = false;
                if (Functions_Bottle.FillEmptyBottle(value)) { captured = true; }
                //if hero put actor into empty bottle..
                if (captured)
                {   //alert player that hero successfully bottled the actor
                    if (Flags.ShowDialogs)
                    { ScreenManager.AddScreen(new ScreenDialog(Dialog.BottleSuccess)); }
                    SetDeathState(Actor); //kill bottled actor
                }
                else
                {   //alert player that hero has no empty bottles (all bottles are full)
                    if (Flags.ShowDialogs)
                    { ScreenManager.AddScreen(new ScreenDialog(Dialog.BottleFull)); }
                }
            }
        }



        public static void SetHitState(Actor Actor)
        {   //bail if actor is already dead (dont hit dead actors)
            if (Actor.state == ActorState.Dead) { return; }
            //else lock actor into hit state, play actor hit soundfx
            Actor.state = ActorState.Hit;
            Actor.stateLocked = true;
            Actor.lockCounter = 0;
            Actor.lockTotal = 15;
            Assets.Play(Actor.sfxHit);

            if (Actor == Pool.hero)
            {
                if (!Flags.InfiniteGold) //continue if infiniteGold is false
                {
                    if (PlayerData.current.gold > 0) //if hero has any gold
                    {   //drop a gold piece upon getting hit
                        Functions_Entity.SpawnEntity(ObjType.PickupRupee, Actor);
                        PlayerData.current.gold--;
                    }
                }
            }
        }

        public static void SetDeathState(Actor Actor)
        {
            Actor.state = ActorState.Dead;
            Actor.stateLocked = true;
            Actor.lockCounter = 0;
            Actor.lockTotal = 255;
            Assets.Play(Actor.sfxDeath); //play actor death sound fx
            if (Actor.type == ActorType.Blob || Actor.type == ActorType.Boss)
            { DungeonRecord.enemyCount++; } //track enemy deaths


            #region Enemy Specific Death Effects

            if (Actor.type == ActorType.Blob)
            {
                Actor.compSprite.zOffset = -16; //sort to floor
                Functions_Entity.SpawnEntity(ObjType.ParticleExplosion, Actor);
                Actor.compCollision.rec.X = -1000; //hide actor collisionRec
                Functions_Loot.SpawnLoot(Actor.compSprite.position);
            }
            else if (Actor.type == ActorType.Boss)
            {
                PlayerData.current.crystal1 = true; //flip crystal1
                DungeonRecord.beatDungeon = true; //player has beat the dungeon
                Functions_Level.levelScreen.exitAction = ExitAction.Summary;
                Functions_Level.levelScreen.displayState = DisplayState.Closing;
                Actor.compSprite.zOffset = -16; //sort to floor
                Actor.compCollision.rec.X = -1000; //hide actor collisionRec
                Assets.Play(Assets.sfxExplosionsMultiple); //play explosions
            }
            else if(Actor.type == ActorType.Fairy)
            {
                Functions_Entity.SpawnEntity(ObjType.ParticleExplosion, Actor);
                Functions_Pool.Release(Actor);
            }

            #endregion


            //sort actor for last time
            Functions_Component.SetZdepth(Actor.compSprite);
        }

        public static void SetRewardState(Actor Actor)
        {   //reward state for hero is set using this method
            Actor.state = ActorState.Reward;
            Actor.stateLocked = true;
            Actor.lockCounter = 0;
            Actor.lockTotal = 40;
        }



        public static void SetCollisionRec(Actor Actor)
        {
            //set the collisionRec parameters based on the Type
            if (Actor.type == ActorType.Boss)
            {
                Actor.compCollision.rec.Width = 24;
                Actor.compCollision.rec.Height = 16;
                Actor.compCollision.offsetX = -12;
                Actor.compCollision.offsetY = -2;
            }
            else
            {
                Actor.compCollision.rec.Width = 12;
                Actor.compCollision.rec.Height = 8;
                Actor.compCollision.offsetX = -6;
                Actor.compCollision.offsetY = 0;
            }
        }

        public static void ResetActorLoadout(Actor Actor)
        {
            Actor.weapon = MenuItemType.Unknown;
            Actor.item = MenuItemType.Unknown;
            Actor.armor = MenuItemType.Unknown;
            Actor.equipment = MenuItemType.Unknown;
        }

        public static void SetHeroLoadout()
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


  
        public static void SetType(Actor Actor, ActorType Type)
        {
            Actor.type = Type;
            //bring actor back to life
            Actor.stateLocked = false;
            Actor.active = true;
            Actor.lockCounter = 0;
            Actor.lockTotal = 0;
            //reset actor's state and direction
            Actor.state = ActorState.Idle;
            Actor.direction = Direction.Down;
            Actor.compMove.direction = Direction.None;
            Actor.compMove.grounded = true; //most actors move on ground
            //reset actor's collisions
            Actor.compCollision.active = true;
            Actor.compCollision.blocking = true;
            SetCollisionRec(Actor);
            //reset actor's sprite zDepth
            Actor.compSprite.zOffset = 0;
            //assume standard actor
            Actor.compSprite.cellSize.X = 16;
            Actor.compSprite.cellSize.Y = 16;
            

            #region Actor Specific Fields

            if (Type == ActorType.Hero)
            {
                Actor.enemy = false;
                Actor.compSprite.texture = Assets.heroSheet;
                //do not update/change the hero's weapon/item/armor/equipment
                Actor.walkSpeed = 0.35f;
                Actor.dashSpeed = 0.90f;
                //set actor sound effects
                Actor.sfxDash = Assets.sfxHeroDash;
                Actor.sfxHit = Assets.sfxHeroHit;
                Actor.sfxDeath = Assets.sfxHeroKill;
            }
            else if (Type == ActorType.Blob)
            {
                Actor.aiType = ActorAI.Basic;
                Actor.enemy = true;
                Actor.compSprite.texture = Assets.blobSheet;
                Actor.health = 1;
                ResetActorLoadout(Actor);
                Actor.weapon = MenuItemType.WeaponSword;
                Actor.walkSpeed = 0.05f;
                Actor.dashSpeed = 0.30f;
                //set actor sound effects
                Actor.sfxDash = Assets.sfxBlobDash;
                Actor.sfxHit = Assets.sfxEnemyHit;
                Actor.sfxDeath = Assets.sfxEnemyKill;
            }
            else if (Type == ActorType.Boss)
            {
                Actor.aiType = ActorAI.Random;
                Actor.enemy = true;
                Actor.compSprite.texture = Assets.bossSheet;
                Actor.health = 10;
                ResetActorLoadout(Actor);
                Actor.walkSpeed = 0.50f;
                Actor.dashSpeed = 1.00f;

                //this actor is a boss (double size)
                Actor.compSprite.cellSize.X = 32;
                Actor.compSprite.cellSize.Y = 32;
                //the boss actor has a lower sorting point that normal actors
                Actor.compSprite.zOffset = 12;

                //set actor sound effects
                Actor.sfxDash = Assets.sfxBlobDash;
                Actor.sfxHit = Assets.sfxBossHit;
                Actor.sfxDeath = Assets.sfxBossHitDeath;
            }
            else if (Type == ActorType.Fairy)
            {   //non-combatant actor
                Actor.aiType = ActorAI.Random;
                Actor.enemy = false;
                Actor.compSprite.texture = Assets.mainSheet;
                Actor.health = 1;
                ResetActorLoadout(Actor);
                Actor.walkSpeed = 0.25f;
                Actor.dashSpeed = 0.5f;
                //set actor sound effects
                Actor.sfxDash = null;
                Actor.sfxHit = null;
                Actor.sfxDeath = Assets.sfxHeartPickup;
                Actor.compMove.grounded = false; //actor flys in air
            }
            else if (Type == ActorType.Doggo)
            {   //non-combatant actor
                Actor.aiType = ActorAI.Basic;
                Actor.enemy = false;
                Actor.compSprite.texture = Assets.mainSheet;
                Actor.health = 200;
                ResetActorLoadout(Actor);
                Actor.walkSpeed = 0.25f;
                Actor.dashSpeed = 0.5f;
                //set actor sound effects
                Actor.sfxDash = null; //bark sound
                Actor.sfxHit = null; //bark sound
                Actor.sfxDeath = null;
            }

            #endregion


            Functions_ActorAnimationList.SetAnimationGroup(Actor);
            Functions_ActorAnimationList.SetAnimationDirection(Actor);
            Functions_Component.UpdateCellSize(Actor.compSprite);
            Functions_Component.CenterOrigin(Actor.compSprite);
        }

        public static void Update(Actor Actor)
        {
            //get the input for this frame, set actor.direction
            Functions_Input.SetInputState(Actor.compInput, Actor);


            #region Actor is not Statelocked

            if (!Actor.stateLocked)
            {   
                //set actor moving/facing direction
                if (Actor.compInput.direction != Direction.None)
                { Actor.direction = Actor.compInput.direction; }

                Actor.state = Actor.inputState; //sync state to input state
                Actor.lockCounter = 0; //reset lock counter in case actor statelocks
                Actor.lockTotal = 0; //reset lock total
                Actor.compMove.speed = Actor.walkSpeed; //default to walk speed

                //check states
                if (Actor.state == ActorState.Interact)
                {   //if there is an object to interact with, interact with it
                    if (Functions_Collision.CheckInteractionRecCollisions()) {}
                    else { Actor.state = ActorState.Idle; } //no interaction
                }
                else if (Actor.state == ActorState.Dash)
                {
                    Actor.lockTotal = 10;
                    Actor.stateLocked = true;
                    Actor.compMove.speed = Actor.dashSpeed;
                    Functions_Entity.SpawnEntity(ObjType.ParticleDashPuff, Actor);
                    Assets.Play(Actor.sfxDash);
                }
                else if (Actor.state == ActorState.Attack)
                {
                    Actor.stateLocked = true;
                    Functions_Movement.StopMovement(Actor.compMove);
                    Functions_Item.UseItem(Actor.weapon, Actor);
                    if (Actor == Pool.hero) { WorldUI.currentWeapon.compSprite.scale = 2.0f; }
                }
                else if (Actor.state == ActorState.Use)
                {   
                    if (Actor.item != MenuItemType.Unknown)
                    { 
                        Actor.stateLocked = true;
                        Functions_Movement.StopMovement(Actor.compMove);
                        
                        //check hero specific cases
                        if (Actor == Pool.hero)
                        {   //bottles are handled seperately
                            if (PlayerData.current.currentItem == 2) //bottleA
                            { Functions_Bottle.UseBottle(1, PlayerData.current.bottleA); }
                            else if (PlayerData.current.currentItem == 3) //bottleB
                            { Functions_Bottle.UseBottle(2, PlayerData.current.bottleB); }
                            else if (PlayerData.current.currentItem == 4) //bottleC
                            { Functions_Bottle.UseBottle(3, PlayerData.current.bottleC); }
                            //item is not a bottle
                            else { Functions_Item.UseItem(Actor.item, Actor); }
                            //scale up worldUI item sprite
                            WorldUI.currentItem.compSprite.scale = 2.0f;
                        }
                        //useItem() is generalized for actors as well
                        else { Functions_Item.UseItem(Actor.item, Actor); }
                    }
                    else { Actor.state = ActorState.Idle; } //no item to use
                }
            }

            #endregion


            #region Actor is Statelocked

            else
            {
                Actor.lockCounter++; //increment lock counter
                if (Actor.lockCounter > Actor.lockTotal) //check against lock total
                {
                    Actor.stateLocked = false; //unlock actor
                    Functions_Input.ResetInputData(Actor.compInput); //reset input component
                    //check to see if the actor is dead
                    if (Actor.health <= 0) { SetDeathState(Actor); }
                }
                if (Actor.state == ActorState.Dead)
                {   //check death state
                    Actor.lockCounter = 0; //lock actor into dead state
                    Actor.health = 0; //lock actor's health at 0


                    #region Hero Death Effects

                    if (Actor == Pool.hero)
                    {   //near the last frame of hero's death, create attention particles
                        if (Actor.compAnim.index == Actor.compAnim.currentAnimation.Count - 2)
                        {   //this event happens when hero falls to ground
                            //goto next anim frame, this event is only processed once
                            Actor.compAnim.index++;
                            //spawn particle to grab the player's attention
                            Functions_Entity.SpawnEntity(
                                    ObjType.ParticleAttention,
                                    Actor.compSprite.position.X,
                                    Actor.compSprite.position.Y,
                                    Direction.None);

                            //check to see if hero can use any bottle to heal/self-rez
                            if (Functions_Bottle.CheckBottleUponDeath(1, PlayerData.current.bottleA)) { }
                            else if (Functions_Bottle.CheckBottleUponDeath(2, PlayerData.current.bottleB)) { }
                            else if (Functions_Bottle.CheckBottleUponDeath(3, PlayerData.current.bottleC)) { }
                            else
                            {   //player has died, failed the dungeon
                                DungeonRecord.beatDungeon = false;
                                Functions_Level.levelScreen.exitAction = ExitAction.Summary;
                                Functions_Level.levelScreen.displayState = DisplayState.Closing;
                            }
                        }
                    }

                    #endregion


                    #region Boss Death Effects

                    else if (Actor.type == ActorType.Boss)
                    {   //dead bosses perpetually explode
                        if(Functions_Random.Int(0,100) > 75) //randomly create explosions
                        {   //randomly place explosions around boss
                            Functions_Entity.SpawnEntity(
                                ObjType.ParticleExplosion,
                                Actor.compSprite.position.X + Functions_Random.Int(-16, 16),
                                Actor.compSprite.position.Y + Functions_Random.Int(-16, 16),
                                Direction.None);
                        }
                    }

                    #endregion

                }
            }

            #endregion


            //set actor animation and direction
            Functions_ActorAnimationList.SetAnimationGroup(Actor);
            Functions_ActorAnimationList.SetAnimationDirection(Actor);

            //alter actor's speed based on loadout
            //chest armor reduces movement
            if (Actor.armor == MenuItemType.ArmorChest) { Actor.compMove.speed *= 0.88f; }
            //cape armor increases movement
            else if (Actor.armor == MenuItemType.ArmorCape) { Actor.compMove.speed *= 1.06f; }
        }

    }
}