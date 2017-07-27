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
    public static class Functions_Item
    {

        public static void UseItem(MenuItemType Type, Actor Actor)
        {

            #region Items - hero and enemies can use Items

            if (Type == MenuItemType.ItemBomb)
            {
                if (Actor == Pool.hero)
                {   
                    if (CheckBombs())
                    {
                        Functions_Entity.SpawnEntity(ObjType.ProjectileBomb, Actor);
                        Actor.lockTotal = 15;
                    }
                    else { Assets.Play(Assets.sfxError); }
                }
                else
                {
                    Functions_Entity.SpawnEntity(ObjType.ProjectileBomb, Actor);
                    Actor.lockTotal = 15;
                }
            }

            #endregion


            #region Bottles - only hero can use bottles

            else if (Type == MenuItemType.BottleEmpty)
            {
                Actor.state = ActorState.Idle;
                Actor.stateLocked = false;
                Actor.lockTotal = 0;
                Assets.Play(Assets.sfxError);
            }
            else if (Type == MenuItemType.BottleHealth)
            {   //refill actor's health, draw attention, soundfx, update boolean
                Actor.health = Actor.maxHealth;
                Functions_Entity.SpawnEntity(ObjType.ParticleHealthPotion, Actor);
                PlayerData.current.bottleHealth = false;
                UseBottle(Type, Actor);
            }
            else if (Type == MenuItemType.BottleMagic)
            {   //refill the actor's magic
                PlayerData.current.magicCurrent = PlayerData.current.magicTotal;
                Functions_Entity.SpawnEntity(ObjType.ParticleMagicPotion, Actor);
                PlayerData.current.bottleMagic = false;
                UseBottle(Type, Actor);
            }
            else if (Type == MenuItemType.BottleFairy)
            {   //refill the actor's health and magic
                Actor.health = Actor.maxHealth;
                PlayerData.current.magicCurrent = PlayerData.current.magicTotal;
                Functions_Entity.SpawnEntity(ObjType.ParticleFairyBottle, Actor);
                PlayerData.current.bottleFairy = false;
                UseBottle(Type, Actor);
            }

            #endregion


            #region Magic - hero and enemies can use magic

            else if (Type == MenuItemType.MagicFireball)
            {
                if(Actor == Pool.hero)
                {
                    if (CheckMagic(1)) //the cost of fireball is 1
                    {
                        Functions_Entity.SpawnEntity(ObjType.ProjectileFireball, Actor);
                        Actor.lockTotal = 15;
                    }
                    else { Assets.Play(Assets.sfxError); }
                }
                else
                {
                    Functions_Entity.SpawnEntity(ObjType.ProjectileFireball, Actor);
                    Actor.lockTotal = 15;
                }
            }

            #endregion


            #region Weapons - hero and enemies can use weapons

            else if (Type == MenuItemType.WeaponSword)
            {
                Functions_Entity.SpawnEntity(ObjType.ProjectileSword, Actor);
                Assets.Play(Assets.sfxSwordSwipe);
                Actor.lockTotal = 15;
            }
            else if (Type == MenuItemType.WeaponBow)
            {
                if (Actor == Pool.hero)
                {
                    if (CheckArrows())
                    {
                        Functions_Entity.SpawnEntity(ObjType.ProjectileArrow, Actor);
                        Functions_Entity.SpawnEntity(ObjType.ParticleBow, Actor);
                        Actor.lockTotal = 15;
                    }
                    else { Assets.Play(Assets.sfxError); }
                }
                else
                {
                    Functions_Entity.SpawnEntity(ObjType.ProjectileArrow, Actor);
                    Functions_Entity.SpawnEntity(ObjType.ParticleBow, Actor);
                    Actor.lockTotal = 15;
                }
            }

            #endregion

        }

        static void UseBottle(MenuItemType Type, Actor Actor)
        {
            Functions_Entity.SpawnEntity(ObjType.ParticleAttention, Actor);
            if (Actor.item == Type) { Actor.item = MenuItemType.BottleEmpty; }
            Assets.Play(Assets.sfxBeatDungeon); //need a refill sound effect
            Actor.state = ActorState.Reward;
            Actor.lockTotal = 40;
        }



        static Boolean CheckMagic(int castingCost)
        {   //if infinite magic is enabled, allow
            if (Flags.InfiniteMagic) { return true; }
            //if hero has enough magic to cast, allow
            if (PlayerData.current.magicCurrent >= castingCost)
            {
                PlayerData.current.magicCurrent -= (byte)castingCost;
                return true;
            }
            return false; //else disallow
        }

        static Boolean CheckArrows()
        {   //if infinite arrows is enabled, allow
            if (Flags.InfiniteArrows) { return true; }
            //if hero has enough arrows to shoot, allow
            if (PlayerData.current.arrowsCurrent > 0)
            { PlayerData.current.arrowsCurrent--; return true; }
            return false;
        }

        static Boolean CheckBombs()
        {   //if infinite arrows is enabled, allow
            if (Flags.InfiniteBombs) { return true; }
            //if hero has enough arrows to shoot, allow
            if (PlayerData.current.bombsCurrent > 0)
            { PlayerData.current.bombsCurrent--; return true; }
            return false;
        }

    }
}