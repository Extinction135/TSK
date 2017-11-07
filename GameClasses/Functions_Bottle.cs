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
    public static class Functions_Bottle
    {

        public static void SetMenuItemType(MenuItem menuItem, Byte bottleValue)
        {   //set passed menuItem.type based on bottle value
            if (bottleValue == 1) { Functions_MenuItem.SetType(MenuItemType.BottleEmpty, menuItem); }
            else if (bottleValue == 2) { Functions_MenuItem.SetType(MenuItemType.BottleHealth, menuItem); }
            else if (bottleValue == 3) { Functions_MenuItem.SetType(MenuItemType.BottleMagic, menuItem); }
            else if (bottleValue == 4) { Functions_MenuItem.SetType(MenuItemType.BottleCombo, menuItem); }
            else if (bottleValue == 5) { Functions_MenuItem.SetType(MenuItemType.BottleFairy, menuItem); }
            else if (bottleValue == 6) { Functions_MenuItem.SetType(MenuItemType.BottleBlob, menuItem); }
            else { Functions_MenuItem.SetType(MenuItemType.Unknown, menuItem); } //defaults to unknown
        }

        public static void LoadBottle(byte bottleValue)
        {   //set hero's item based on the bottleValue
            if (bottleValue == 1) { Pool.hero.item = MenuItemType.BottleEmpty; }
            else if (bottleValue == 2) { Pool.hero.item = MenuItemType.BottleHealth; }
            else if (bottleValue == 3) { Pool.hero.item = MenuItemType.BottleMagic; }
            else if (bottleValue == 4) { Pool.hero.item = MenuItemType.BottleCombo; }
            else if (bottleValue == 5) { Pool.hero.item = MenuItemType.BottleFairy; }
            else if (bottleValue == 6) { Pool.hero.item = MenuItemType.BottleBlob; }
            else { Pool.hero.item = MenuItemType.Unknown; } //defaults to unknown
        }

        public static void UseBottle(Byte bottleID, Byte bottleValue)
        {   //only hero can use bottles
            if (bottleValue == 1)
            {   //use empty bottle
                Functions_Entity.SpawnEntity(ObjType.ParticleBottleEmpty, Pool.hero);
                Assets.Play(Assets.sfxError);
            }
            else if (bottleValue == 2)
            {   //use health potion
                Pool.hero.health = PlayerData.current.heartsTotal;
                Functions_Entity.SpawnEntity(ObjType.ParticleBottleHealth, Pool.hero);
                Assets.Play(Assets.sfxBeatDungeon);
            }
            else if (bottleValue == 3)
            {   //use magic potion
                PlayerData.current.magicCurrent = PlayerData.current.magicTotal;
                Functions_Entity.SpawnEntity(ObjType.ParticleBottleMagic, Pool.hero);
                Assets.Play(Assets.sfxBeatDungeon);
            }
            else if (bottleValue == 4)
            {   //use combo potion
                Pool.hero.health = PlayerData.current.heartsTotal;
                PlayerData.current.magicCurrent = PlayerData.current.magicTotal;
                Functions_Entity.SpawnEntity(ObjType.ParticleBottleCombo, Pool.hero);
                Assets.Play(Assets.sfxBeatDungeon);
            }
            else if (bottleValue == 5)
            {   //use fairy in a bottle
                Pool.hero.health = PlayerData.current.heartsTotal;
                Functions_Entity.SpawnEntity(ObjType.ParticleBottleFairy, Pool.hero);
                Assets.Play(Assets.sfxBeatDungeon);
            }
            else if (bottleValue == 6)
            {   //display the bottled blob over hero's head
                Functions_Entity.SpawnEntity(ObjType.ParticleBottleBlob, Pool.hero);
                Assets.Play(Assets.sfxBeatDungeon);
                //use blob in a bottle (transform hero into blob and vice versa)
                if (Pool.hero.type == ActorType.Hero)
                { Functions_Actor.SetType(Pool.hero, ActorType.Blob); }
                else { Functions_Actor.SetType(Pool.hero, ActorType.Hero); }
                //reload hero's loadout from player.current data
                Functions_Hero.SetLoadout();
                Pool.hero.health = PlayerData.current.heartsTotal; //refill the hero's health
                PlayerData.current.actorType = Pool.hero.type; //save the hero's actorType
            }

            //empty bottle based on bottleID
            if (bottleID == 1) { PlayerData.current.bottleA = 1; }
            else if (bottleID == 2) { PlayerData.current.bottleB = 1; }
            else if (bottleID == 3) { PlayerData.current.bottleC = 1; }
            //set hero into reward state, grab player's attention
            Functions_Actor.SetRewardState(Pool.hero);
            Functions_Entity.SpawnEntity(ObjType.ParticleAttention, Pool.hero);
            //hero has used his item (which was a filled bottle)
            Pool.hero.item = MenuItemType.Unknown;
        }

        public static Boolean CheckBottleUponDeath(Byte bottleID, Byte bottleValue)
        {   //check for health, combo, or fairy values
            if (bottleValue == 2 || bottleValue == 4 || bottleValue == 5)
            { UseBottle(bottleID, bottleValue); return true; }
            return false; //bottle cannot be used to heal/self-rez
        }

        public static Boolean FillEmptyBottle(byte fillValue)
        {   //find a bottle with a value of 1 (empty), fill it
            if (PlayerData.current.bottleA == 1)
            { PlayerData.current.bottleA = fillValue; return true; }
            else if (PlayerData.current.bottleB == 1)
            { PlayerData.current.bottleB = fillValue; return true; }
            else if (PlayerData.current.bottleC == 1)
            { PlayerData.current.bottleC = fillValue; return true; }
            else { return false; } //did not find an empty bottle
        }


    }
}