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
            //handle casting actor's state
            //Actor.stateLocked = true;
            //Functions_Movement.StopMovement(Actor.compMove);
            //Actor.lockTotal = 20;




            #region Hero Specific Items

            if (Actor == Pool.hero)
            {


                //check to see if we can bail, using empty bottle type check
                if (Type == MenuItemType.BottleEmpty)
                {   //bail, with error sound
                    Assets.Play(Assets.sfxError);
                    return;
                }

                //check bottles A
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







                /*

                else if (PlayerData.current.currentItem == HerosCurrentItem.BottleB) 
                {
                    Functions_Bottle.UseBottle(PlayerData.current.bottleB);
                    PlayerData.current.bottleB = BottleContent.Empty;
                }

                else if (PlayerData.current.currentItem == HerosCurrentItem.BottleC)
                {
                    Functions_Bottle.UseBottle(PlayerData.current.bottleC);
                    PlayerData.current.bottleC = BottleContent.Empty;
                }

                */




            }

            #endregion


            //all actors can use items, magic, and weapons


            #region Items

            if (Type == MenuItemType.ItemBomb)
            {
                if (Actor == Pool.hero & !CheckBombs()) //check if hero has enough
                { Assets.Play(Assets.sfxError); Actor.lockTotal = 0; return; }
                //actor spawns a bomb
                Functions_Projectile.Spawn(ObjType.ProjectileBomb, Actor);
                Functions_Actor.SetItemUseState(Actor);
            }

            #endregion


            #region Magic

            else if (Type == MenuItemType.MagicFireball)
            {
                if (Actor == Pool.hero & !CheckMagic(1)) //check if hero has enough
                { Assets.Play(Assets.sfxError); Actor.lockTotal = 0; return; }
                //actor shoots a fireball
                Functions_Projectile.Spawn(ObjType.ProjectileFireball, Actor);
                Functions_Actor.SetItemUseState(Actor);
            }

            #endregion


            #region Weapons

            else if (Type == MenuItemType.WeaponSword)
            {
                Functions_Projectile.Spawn(ObjType.ProjectileSword, Actor);
                Functions_Actor.SetItemUseState(Actor);
            }
            else if (Type == MenuItemType.WeaponBow)
            {
                if (Actor == Pool.hero & !CheckArrows()) //check if hero has enough
                { Assets.Play(Assets.sfxError); Actor.lockTotal = 0; return; }
                //actor shoots an arrow
                Functions_Projectile.Spawn(ObjType.ProjectileArrow, Actor);
                Functions_Projectile.Spawn(ObjType.ParticleBow, Actor);
                Functions_Actor.SetItemUseState(Actor);
            }
            else if (Type == MenuItemType.WeaponNet)
            {
                Functions_Projectile.Spawn(ObjType.ProjectileNet, Actor);
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