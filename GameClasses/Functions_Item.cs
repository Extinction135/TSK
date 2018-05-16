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

            #region Weapons

            if (Type == MenuItemType.WeaponSword)
            {
                Functions_Projectile.Spawn(ObjType.ProjectileSword, Actor.compMove, Actor.direction);
                Functions_Actor.SetItemUseState(Actor);
            }
            else if (Type == MenuItemType.WeaponNet)
            {
                Functions_Projectile.Spawn(ObjType.ProjectileNet, Actor.compMove, Actor.direction);
                Functions_Actor.SetItemUseState(Actor);
            }

            #endregion


            #region Items

            else if (Type == MenuItemType.ItemBomb)
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
            else if (Type == MenuItemType.ItemBow)
            {
                if (Actor == Pool.hero & !CheckArrows()) //check if hero has enough
                { Assets.Play(Assets.sfxError); Actor.lockTotal = 0; return; }
                //actor shoots an arrow
                Functions_Projectile.Spawn(ObjType.ProjectileArrow, Actor.compMove, Actor.direction);
                //actor displays a bow
                Functions_Projectile.Spawn(ObjType.ProjectileBow, Actor.compMove, Actor.direction);
                Functions_Actor.SetItemUseState(Actor);
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


            #region Bottles

            else if (Type == MenuItemType.BottleEmpty)
            {   //goto idle, with error sfx
                Actor.state = ActorState.Idle;
                Assets.Play(Assets.sfxError);
                return;
            }
            else if (Type == MenuItemType.BottleHealth)
            {   //use health potion
                Pool.hero.health = PlayerData.current.heartsTotal;
                Functions_Particle.Spawn(ObjType.Particle_BottleHealth, Pool.hero);
                Functions_Bottle.EmptyBottle(Type);
            }
            else if (Type == MenuItemType.BottleMagic)
            {   //use magic potion
                PlayerData.current.magicCurrent = PlayerData.current.magicMax;
                Functions_Particle.Spawn(ObjType.Particle_BottleMagic, Pool.hero);
                Functions_Bottle.EmptyBottle(Type);
            }
            else if (Type == MenuItemType.BottleCombo)
            {   //use combo potion
                Pool.hero.health = PlayerData.current.heartsTotal;
                PlayerData.current.magicCurrent = PlayerData.current.magicMax;
                Functions_Particle.Spawn(ObjType.Particle_BottleCombo, Pool.hero);
                Functions_Bottle.EmptyBottle(Type);
            }
            else if (Type == MenuItemType.BottleFairy)
            {   //use fairy in a bottle
                Pool.hero.health = PlayerData.current.heartsTotal;
                Functions_Particle.Spawn(ObjType.Particle_BottleFairy, Pool.hero);
                Functions_Bottle.EmptyBottle(Type);
            }
            else if (Type == MenuItemType.BottleBlob)
            {
                Functions_Bottle.EmptyBottle(Type);
                //display the bottled blob over hero's head
                Functions_Particle.Spawn(ObjType.Particle_BottleBlob, Pool.hero);

                //use blob in a bottle (transform hero into blob and vice versa)
                if (Pool.hero.type == ActorType.Hero)
                { Functions_Actor.Transform(Pool.hero, ActorType.Blob); }
                else
                { Functions_Actor.Transform(Pool.hero, ActorType.Hero); }
            }
            
            #endregion


            else//unknown case: return actor to idle
            { 
                Functions_Actor.SetIdleState(Actor);
            }
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