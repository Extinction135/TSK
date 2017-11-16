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
                if (PlayerData.current.currentItem == 2) //bottleA
                { Functions_Bottle.UseBottle(1, PlayerData.current.bottleA); }
                else if (PlayerData.current.currentItem == 3) //bottleB
                { Functions_Bottle.UseBottle(2, PlayerData.current.bottleB); }
                else if (PlayerData.current.currentItem == 4) //bottleC
                { Functions_Bottle.UseBottle(3, PlayerData.current.bottleC); }
            }

            #endregion

            //all actors can use items, magic, and weapons

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