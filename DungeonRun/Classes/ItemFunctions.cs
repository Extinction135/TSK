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
    public static class ItemFunctions
    {

        public static void UseItem(MenuItemType Type, Actor Actor)
        {

            #region Items

            if (Type == MenuItemType.ItemBomb)
            {

                //create bomb projectile at actor's location
                //place a distance away from actor in direction
                //this should be handled in alignment routines in ObjFunctions

                if (Actor == Pool.hero)
                {   //hero has cast a fireball
                    if (PlayerData.saveData.bombsCurrent > 0)
                    {
                        PlayerData.saveData.bombsCurrent--;
                        GameObjectFunctions.SpawnProjectile(ObjType.ProjectileBomb, Actor);
                        Assets.Play(Assets.sfxFireballCast);
                        Actor.lockTotal = 15;
                    }
                    else { Assets.Play(Assets.sfxError); }
                }
                else
                {   //an enemy has cast a fireball
                    GameObjectFunctions.SpawnProjectile(ObjType.ProjectileBomb, Actor);
                    Assets.Play(Assets.sfxFireballCast);
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
                PlayerData.saveData.magicCurrent = PlayerData.saveData.magicMax;
                PlayerData.saveData.bottleMagic = false;
                UseBottle(Type, Actor);
            }
            else if (Type == MenuItemType.BottleFairy)
            {   //refill the actor's health and magic
                Actor.health = Actor.maxHealth;
                PlayerData.saveData.magicCurrent = PlayerData.saveData.magicMax;
                GameObjectFunctions.SpawnProjectile(ObjType.ParticleFairy, Actor);
                PlayerData.saveData.bottleFairy = false;
                UseBottle(Type, Actor);
            }

            #endregion


            #region Magic - hero and enemies can use magic

            else if (Type == MenuItemType.MagicFireball)
            {
                if(Actor == Pool.hero)
                {   //hero has cast a fireball
                    if (PlayerData.saveData.magicCurrent > 0)
                    {
                        PlayerData.saveData.magicCurrent--;
                        GameObjectFunctions.SpawnProjectile(ObjType.ProjectileFireball, Actor);
                        Assets.Play(Assets.sfxFireballCast);
                        Actor.lockTotal = 15;
                    }
                    else { Assets.Play(Assets.sfxError); }
                }
                else
                {   //an enemy has cast a fireball
                    GameObjectFunctions.SpawnProjectile(ObjType.ProjectileFireball, Actor);
                    Assets.Play(Assets.sfxFireballCast);
                    Actor.lockTotal = 15;
                }
            }

            #endregion


            #region Weapons - hero and enemies can use weapons

            else if (Type == MenuItemType.WeaponSword)
            {
                GameObjectFunctions.SpawnProjectile(ObjType.ProjectileSword, Actor);
                Assets.Play(Assets.sfxSwordSwipe);
                Actor.lockTotal = 15;
            }

            #endregion

        }

        static void UseBottle(MenuItemType Type, Actor Actor)
        {
            GameObjectFunctions.SpawnProjectile(ObjType.ParticleAttention, Actor);
            if (Actor.item == Type) { Actor.item = MenuItemType.BottleEmpty; }
            Assets.Play(Assets.sfxHeartPickup); //need a refill sound effect
            Actor.state = ActorState.Reward;
            Actor.lockTotal = 30;
        }

    }
}