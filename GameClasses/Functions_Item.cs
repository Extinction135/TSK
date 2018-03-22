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

        public static void UseWeapon(MenuItemType Type, Actor Actor)
        {
            if (Type == MenuItemType.WeaponSword)
            {
                Functions_Projectile.Spawn(ObjType.ProjectileSword, Actor.compMove, Actor.direction);
                Functions_Actor.SetItemUseState(Actor);
            }
            else if (Type == MenuItemType.WeaponBow)
            {
                if (Actor == Pool.hero & !CheckArrows()) //check if hero has enough
                { Assets.Play(Assets.sfxError); Actor.lockTotal = 0; return; }
                //actor shoots an arrow
                Functions_Projectile.Spawn(ObjType.ProjectileArrow, Actor.compMove, Actor.direction);
                //actoro displays a bow
                Functions_Particle.Spawn(ObjType.ParticleBow, Actor);
                Functions_Actor.SetItemUseState(Actor);
            }
            else if (Type == MenuItemType.WeaponNet)
            {
                Functions_Projectile.Spawn(ObjType.ProjectileNet, Actor.compMove, Actor.direction);
                Functions_Actor.SetItemUseState(Actor);
            }
            else
            {   //the weapon is unknown and cannot be used by the actor
                Functions_Actor.SetIdleState(Actor);
            }
        }

        public static void UseItem(MenuItemType Type, Actor Actor)
        {   //first, check to see if we can bail from this method
            //if the item being used is unknown, silently return actor to idle
            if (Type == MenuItemType.Unknown)
            {
                Actor.state = ActorState.Idle;
                return;
            }
            //if the item being used is an empty bottle, play an error sound
            else if (Type == MenuItemType.BottleEmpty)
            {   //fail, with an error sound
                Actor.state = ActorState.Idle;
                Assets.Play(Assets.sfxError);
                return;
            }











            /*

            #region Bottles

            //it is assumed that only the player/hero can use bottles,
            //so the effects of bottles only target the hero

            if (Actor == Pool.hero)
            {
                if (Type == MenuItemType.BottleBlob
                    || Type == MenuItemType.BottleCombo
                    || Type == MenuItemType.BottleFairy
                    || Type == MenuItemType.BottleHealth
                    || Type == MenuItemType.BottleMagic)
                {
                    if (Functions_Bottle.UseBottle(Type))
                    { }
                }
            }



            if (Actor == Pool.hero)
            {   //check bottle A
                if (PlayerData.current.currentItem == HerosCurrentItem.BottleA)
                {
                    if (Functions_Bottle.UseBottle(PlayerData.current.bottleA))
                    {   //reward hero, empty bottle & clear hero's item
                        Functions_Actor.SetRewardState(Pool.hero);
                        PlayerData.current.bottleA = BottleContent.Empty;
                        Pool.hero.item = MenuItemType.Unknown;
                        Functions_Particle.Spawn(ObjType.ParticleAttention, Pool.hero);
                    }
                }
                //check bottle B
                else if (PlayerData.current.currentItem == HerosCurrentItem.BottleB)
                {
                    if (Functions_Bottle.UseBottle(PlayerData.current.bottleB))
                    {   //reward hero, empty bottle & clear hero's item
                        Functions_Actor.SetRewardState(Pool.hero);
                        PlayerData.current.bottleB = BottleContent.Empty;
                        Pool.hero.item = MenuItemType.Unknown;
                        Functions_Particle.Spawn(ObjType.ParticleAttention, Pool.hero);
                    }
                }
                //check bottle C
                else if (PlayerData.current.currentItem == HerosCurrentItem.BottleC)
                {
                    if (Functions_Bottle.UseBottle(PlayerData.current.bottleC))
                    {   //reward hero, empty bottle & clear hero's item
                        Functions_Actor.SetRewardState(Pool.hero);
                        PlayerData.current.bottleC = BottleContent.Empty;
                        Pool.hero.item = MenuItemType.Unknown;
                        Functions_Particle.Spawn(ObjType.ParticleAttention, Pool.hero);
                    }
                }
            }

            #endregion

            */



            //all actors can use items & magic


            #region Items

            if (Type == MenuItemType.ItemBomb)
            {
                if (Actor == Pool.hero & !CheckBombs()) //check if hero has enough
                { Assets.Play(Assets.sfxError); Actor.lockTotal = 0; return; }
                //actor spawns a bomb
                Functions_Projectile.Spawn(ObjType.ProjectileBomb, Actor.compMove, Actor.direction);
                Functions_Actor.SetItemUseState(Actor);
            }

            else if (Type == MenuItemType.ItemBoomerang)
            {   
                if (Functions_Hero.boomerangInPlay == false)
                {   //throw a boomerang, if there is no boomerang in play
                    Functions_Projectile.Spawn(ObjType.ProjectileBoomerang, Actor.compMove, Actor.direction);
                    Functions_Actor.SetItemUseState(Actor);
                    Assets.Play(Assets.sfxArrowShoot);
                }
            }

            #endregion


            #region Magic

            else if (Type == MenuItemType.MagicFireball)
            {
                if (Actor == Pool.hero & !CheckMagic(1)) //check if hero has enough
                { Assets.Play(Assets.sfxError); Actor.lockTotal = 0; return; }
                //actor shoots a fireball
                Functions_Projectile.Spawn(ObjType.ProjectileFireball, Actor.compMove, Actor.direction);
                Functions_Actor.SetItemUseState(Actor);
            }

            #endregion


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