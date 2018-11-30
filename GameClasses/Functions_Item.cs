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
            //Debug.WriteLine("use itm: " + Actor.type + ", type: " + Type);


            #region Weapons

            if (Type == MenuItemType.WeaponSword)
            {
                Functions_Projectile.Spawn(ProjectileType.Sword, Actor, Actor.direction);
                Functions_Actor.SetItemUseState(Actor);
            }
            else if (Type == MenuItemType.WeaponNet)
            {
                Functions_Projectile.Spawn(ProjectileType.Net, Actor, Actor.direction);
                Functions_Actor.SetItemUseState(Actor);
            }
            else if (Type == MenuItemType.WeaponShovel)
            {
                Functions_Projectile.Spawn(ProjectileType.Shovel, Actor, Actor.direction);
                Functions_Actor.SetItemUseState(Actor);
            }
            else if (Type == MenuItemType.WeaponHammer)
            {
                Functions_Projectile.Spawn(ProjectileType.Hammer, Actor, Actor.direction);
                Functions_Actor.SetItemUseState(Actor);
            }
            else if (Type == MenuItemType.WeaponFang)
            {
                Functions_Projectile.Spawn(ProjectileType.Bite, Actor, Actor.direction);
                Functions_Actor.SetItemUseState(Actor);
            }

            #endregion


            #region Items

            else if (Type == MenuItemType.ItemBomb)
            {
                if (Actor == Pool.hero & !CheckBombs()) //check if hero has enough
                { Assets.Play(Assets.sfxError); Actor.lockTotal = 0; return; }
                //actor spawns a bomb
                Functions_Projectile.Spawn(ProjectileType.Bomb, Actor, Actor.direction);
                Functions_Actor.SetItemUseState(Actor);
            }
            else if (Type == MenuItemType.ItemBoomerang)
            {
                if (Functions_Hero.boomerangInPlay == false)
                {   //throw a boomerang, if there is no boomerang in play
                    Functions_Projectile.Spawn(ProjectileType.Boomerang, Actor, Actor.direction);
                    Functions_Actor.SetItemUseState(Actor);
                    Assets.Play(Assets.sfxArrowShoot);
                }
            }
            else if (Type == MenuItemType.ItemBow)
            {
                if (Actor == Pool.hero & !CheckArrows()) //check if hero has enough
                { Assets.Play(Assets.sfxError); Actor.lockTotal = 0; return; }
                //actor shoots an arrow
                Functions_Projectile.Spawn(ProjectileType.Arrow, Actor, Actor.direction);
                //actor displays a bow
                Functions_Projectile.Spawn(ProjectileType.Bow, Actor, Actor.direction);
                Functions_Actor.SetItemUseState(Actor);
            }

            else if (Type == MenuItemType.ItemFirerod)
            {
                if (Actor == Pool.hero & !CheckMagic(1)) //check if hero has enough
                { Assets.Play(Assets.sfxError); Actor.lockTotal = 0; return; }
                //actor shoots a fireball
                Functions_Projectile.Spawn(ProjectileType.Fireball, Actor, Actor.direction);
                //actor displays a rod
                Functions_Projectile.Spawn(ProjectileType.Firerod, Actor, Actor.direction);
                Functions_Actor.SetItemUseState(Actor);
            }
            else if (Type == MenuItemType.ItemIcerod)
            {
                if (Actor == Pool.hero & !CheckMagic(1)) //check if hero has enough
                { Assets.Play(Assets.sfxError); Actor.lockTotal = 0; return; }
                //actor shoots a fireball
                Functions_Projectile.Spawn(ProjectileType.Iceball, Actor, Actor.direction);
                //actor displays a rod
                Functions_Projectile.Spawn(ProjectileType.Icerod, Actor, Actor.direction);
                Functions_Actor.SetItemUseState(Actor);
            }

            #endregion


            #region HERO only Magic

            else if (Type == MenuItemType.MagicBombos)
            {   //only hero can cast this magic
                if (Actor == Pool.hero)
                {
                    if (!CheckMagic(5))
                    {   //failed to cast
                        Assets.Play(Assets.sfxError);
                        Actor.lockTotal = 0;
                        return;
                    }
                    else
                    {   //casted successfully
                        Functions_Projectile.Cast_Bombos();
                        Functions_Hero.SetRewardState(ParticleType.RewardMagicBombos);
                    }
                }
            }
            else if (Type == MenuItemType.MagicEther)
            {   //only hero can cast this magic
                if(Actor == Pool.hero)
                {
                    if (!CheckMagic(5))
                    {   //failed to cast
                        Assets.Play(Assets.sfxError);
                        Actor.lockTotal = 0;
                        return;
                    }
                    else
                    {   //casted successfully
                        Functions_Projectile.Cast_Ether();
                        Functions_Hero.SetRewardState(ParticleType.RewardMagicEther);
                    }
                }
            }

            #endregion


            #region Enemy/Actor Magic

            else if (Type == MenuItemType.MagicBat)
            {
                //create bat projectile, shorten casting time to allow for spamming
                Functions_Projectile.Spawn(ProjectileType.Bat, Actor, Actor.direction);
                Functions_Actor.SetItemUseState(Actor);
                Actor.lockTotal = 4;
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
                Functions_Hero.SetRewardState(ParticleType.RewardBottle);
                Functions_Bottle.EmptyBottle(Type);
            }
            else if (Type == MenuItemType.BottleMagic)
            {   //use magic potion
                PlayerData.current.magicCurrent = PlayerData.current.magicMax;
                Functions_Hero.SetRewardState(ParticleType.RewardBottle);
                Functions_Bottle.EmptyBottle(Type);
            }
            else if (Type == MenuItemType.BottleCombo)
            {   //use combo potion
                Pool.hero.health = PlayerData.current.heartsTotal;
                PlayerData.current.magicCurrent = PlayerData.current.magicMax;
                Functions_Hero.SetRewardState(ParticleType.RewardBottle);
                Functions_Bottle.EmptyBottle(Type);
            }
            else if (Type == MenuItemType.BottleFairy)
            {   //use fairy in a bottle
                Pool.hero.health = PlayerData.current.heartsTotal;
                Functions_Hero.SetRewardState(ParticleType.RewardBottle);
                Functions_Bottle.EmptyBottle(Type);
            }
            else if (Type == MenuItemType.BottleBlob)
            {
                Functions_Bottle.EmptyBottle(Type);
                //display the bottled blob over hero's head
                Functions_Hero.SetRewardState(ParticleType.RewardBottle);

                //use blob in a bottle (transform hero into blob and vice versa)
                if (Pool.hero.type == ActorType.Hero)
                { Functions_Actor.Transform(Pool.hero, ActorType.Blob); }
                else
                { Functions_Actor.Transform(Pool.hero, ActorType.Hero); }
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