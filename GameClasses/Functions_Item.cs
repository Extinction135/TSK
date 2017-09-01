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
        {   //all actors can use items, magic, and weapons

            #region Items

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


            #region Magic

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


            #region Weapons

            else if (Type == MenuItemType.WeaponSword)
            {
                Functions_Entity.SpawnEntity(ObjType.ProjectileSword, Actor);
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
            else if (Type == MenuItemType.WeaponNet)
            {
                Functions_Entity.SpawnEntity(ObjType.ProjectileNet, Actor);
                Actor.lockTotal = 15;
            }

            #endregion

        }

        public static void UseBottle(Byte bottleID, Byte bottleValue)
        {   //only hero can use bottles
            if(bottleValue == 1)
            {   //use empty bottle
                Functions_Entity.SpawnEntity(ObjType.ParticleBottleEmpty, Pool.hero);
            }
            else if (bottleValue == 2) 
            {   //use health potion
                Pool.hero.health = Pool.hero.maxHealth;
                Functions_Entity.SpawnEntity(ObjType.ParticleBottleHealth, Pool.hero);
            }
            else if (bottleValue == 3)
            {   //use magic potion
                PlayerData.current.magicCurrent = PlayerData.current.magicTotal;
                Functions_Entity.SpawnEntity(ObjType.ParticleBottleMagic, Pool.hero);
            }
            else if (bottleValue == 4)
            {   //use fairy in a bottle
                Pool.hero.health = Pool.hero.maxHealth;
                PlayerData.current.magicCurrent = PlayerData.current.magicTotal;
                Functions_Entity.SpawnEntity(ObjType.ParticleBottleFairy, Pool.hero);
            }

            //empty bottle based on bottleID
            if (bottleID == 1) { PlayerData.current.bottleA = 1; }
            else if (bottleID == 2) { PlayerData.current.bottleB = 1; }
            else if (bottleID == 3) { PlayerData.current.bottleC = 1; }

            //set hero into reward state
            Functions_Entity.SpawnEntity(ObjType.ParticleAttention, Pool.hero);
            Pool.hero.state = ActorState.Reward;
            Pool.hero.lockTotal = 40;

            //if hero's item is a bottle, set it to be empty
            if (Pool.hero.item == MenuItemType.BottleHealth
                || Pool.hero.item == MenuItemType.BottleMagic
                || Pool.hero.item == MenuItemType.BottleFairy)
            { Pool.hero.item = MenuItemType.BottleEmpty; }
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