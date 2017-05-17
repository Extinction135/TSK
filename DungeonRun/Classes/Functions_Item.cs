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
                    if (PlayerData.saveData.bombsCurrent > 0)
                    {
                        PlayerData.saveData.bombsCurrent--;
                        Functions_Entity.SpawnEntity(ObjType.ProjectileBomb, Actor);
                        Actor.lockTotal = 15;
                        //if hero used the last bomb, set the item to be unknown/none
                        if (PlayerData.saveData.bombsCurrent == 0)
                        { Actor.item = MenuItemType.Unknown; }
                    }
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
                PlayerData.saveData.bottleHealth = false;
                UseBottle(Type, Actor);
            }
            else if (Type == MenuItemType.BottleMagic)
            {   //refill the actor's magic
                PlayerData.saveData.magicCurrent = PlayerData.saveData.magicTotal;
                PlayerData.saveData.bottleMagic = false;
                UseBottle(Type, Actor);
            }
            else if (Type == MenuItemType.BottleFairy)
            {   //refill the actor's health and magic
                Actor.health = Actor.maxHealth;
                PlayerData.saveData.magicCurrent = PlayerData.saveData.magicTotal;
                Functions_Entity.SpawnEntity(ObjType.ParticleFairy, Actor);
                PlayerData.saveData.bottleFairy = false;
                UseBottle(Type, Actor);
            }

            #endregion


            #region Magic - hero and enemies can use magic

            else if (Type == MenuItemType.MagicFireball)
            {
                if(Actor == Pool.hero)
                {   
                    if (PlayerData.saveData.magicCurrent > 0)
                    {
                        PlayerData.saveData.magicCurrent--;
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
                    if (PlayerData.saveData.arrowsCurrent > 0)
                    {
                        PlayerData.saveData.arrowsCurrent--;
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
            Assets.Play(Assets.sfxHeartPickup); //need a refill sound effect
            Actor.state = ActorState.Reward;
            Actor.lockTotal = 30;
        }

    }
}