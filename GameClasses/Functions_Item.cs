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

            #region Hero Specific Items

            if (Actor == Pool.hero)
            {   //bottles
                if (PlayerData.current.currentItem == HerosCurrentItem.BottleA)
                { Functions_Bottle.UseBottle(PlayerData.current.bottleA); }

                else if (PlayerData.current.currentItem == HerosCurrentItem.BottleB) 
                { Functions_Bottle.UseBottle(PlayerData.current.bottleB); }

                else if (PlayerData.current.currentItem == HerosCurrentItem.BottleC)
                { Functions_Bottle.UseBottle(PlayerData.current.bottleC); }
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
            }

            #endregion


            #region Magic

            else if (Type == MenuItemType.MagicFireball)
            {
                if (Actor == Pool.hero & !CheckMagic(1)) //check if hero has enough
                { Assets.Play(Assets.sfxError); Actor.lockTotal = 0; return; }
                //actor shoots a fireball
                Functions_Projectile.Spawn(ObjType.ProjectileFireball, Actor);
            }

            #endregion


            #region Weapons

            else if (Type == MenuItemType.WeaponSword)
            {
                Functions_Projectile.Spawn(ObjType.ProjectileSword, Actor);
                Actor.lockTotal = 15;
            }
            else if (Type == MenuItemType.WeaponBow)
            {
                if (Actor == Pool.hero & !CheckArrows()) //check if hero has enough
                { Assets.Play(Assets.sfxError); Actor.lockTotal = 0; return; }
                //actor shoots an arrow
                Functions_Projectile.Spawn(ObjType.ProjectileArrow, Actor);
                Functions_Projectile.Spawn(ObjType.ParticleBow, Actor);
            }
            else if (Type == MenuItemType.WeaponNet)
            {
                Functions_Projectile.Spawn(ObjType.ProjectileNet, Actor);
                Actor.lockTotal = 15;
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