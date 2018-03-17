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

        public static Boolean HeroDeathCheck()
        {   //see if hero can save himself from death using ANY bottles contents
            if (PlayerData.current.bottleA == BottleContent.Fairy)
            {
                UseBottle(PlayerData.current.bottleA);
                PlayerData.current.bottleA = BottleContent.Empty;
                Pool.hero.item = MenuItemType.BottleEmpty;
                Functions_Actor.SetRewardState(Pool.hero);
                return true;
            }
            else if (PlayerData.current.bottleB == BottleContent.Fairy)
            {
                UseBottle(PlayerData.current.bottleB);
                PlayerData.current.bottleB = BottleContent.Empty;
                Pool.hero.item = MenuItemType.BottleEmpty;
                Functions_Actor.SetRewardState(Pool.hero);
                return true;
            }
            else if (PlayerData.current.bottleC == BottleContent.Fairy)
            {
                UseBottle(PlayerData.current.bottleC);
                PlayerData.current.bottleC = BottleContent.Empty;
                Pool.hero.item = MenuItemType.BottleEmpty;
                Functions_Actor.SetRewardState(Pool.hero);
                return true;
            }
            return false; //else he cannot, return false
        }

        public static void SetMenuItemType(MenuItem menuItem, BottleContent content)
        {   //set passed menuItem.type based on bottle value
            if (content == BottleContent.Blob)
            { Functions_MenuItem.SetType(MenuItemType.BottleBlob, menuItem); }

            else if (content == BottleContent.Combo)
            { Functions_MenuItem.SetType(MenuItemType.BottleCombo, menuItem); }

            else if (content == BottleContent.Empty)
            { Functions_MenuItem.SetType(MenuItemType.BottleEmpty, menuItem); }

            else if (content == BottleContent.Fairy)
            { Functions_MenuItem.SetType(MenuItemType.BottleFairy, menuItem); }

            else if (content == BottleContent.Health)
            { Functions_MenuItem.SetType(MenuItemType.BottleHealth, menuItem); }

            else if (content == BottleContent.Magic)
            { Functions_MenuItem.SetType(MenuItemType.BottleMagic, menuItem); }

            //in an unhandled case, set the bottle to be empty
            else { Functions_MenuItem.SetType(MenuItemType.BottleEmpty, menuItem); }
        }

        public static void LoadBottle(BottleContent content)
        {   //set hero's item based on the bottleValue
            if (content == BottleContent.Blob) { Pool.hero.item = MenuItemType.BottleBlob; }
            else if (content == BottleContent.Combo) { Pool.hero.item = MenuItemType.BottleCombo; }
            else if (content == BottleContent.Empty) { Pool.hero.item = MenuItemType.BottleEmpty; }
            else if (content == BottleContent.Fairy) { Pool.hero.item = MenuItemType.BottleFairy; }
            else if (content == BottleContent.Health) { Pool.hero.item = MenuItemType.BottleHealth; }
            else if (content == BottleContent.Magic) { Pool.hero.item = MenuItemType.BottleMagic; }
            else { Pool.hero.item = MenuItemType.Unknown; } //defaults to unknown
        }

        public static Boolean UseBottle(BottleContent content)
        {   //only hero can use bottles, empty bottles are not handled here
            if (content == BottleContent.Health)
            {   //use health potion
                Pool.hero.health = PlayerData.current.heartsTotal;
                Functions_Particle.Spawn(ObjType.ParticleBottleHealth, Pool.hero);
                Assets.Play(Assets.sfxBeatDungeon);
                return true;
            }
            else if (content == BottleContent.Magic)
            {   //use magic potion
                PlayerData.current.magicCurrent = PlayerData.current.magicTotal;
                Functions_Particle.Spawn(ObjType.ParticleBottleMagic, Pool.hero);
                Assets.Play(Assets.sfxBeatDungeon);
                return true;
            }
            else if (content == BottleContent.Combo)
            {   //use combo potion
                Pool.hero.health = PlayerData.current.heartsTotal;
                PlayerData.current.magicCurrent = PlayerData.current.magicTotal;
                Functions_Particle.Spawn(ObjType.ParticleBottleCombo, Pool.hero);
                Assets.Play(Assets.sfxBeatDungeon);
                return true;
            }
            else if (content == BottleContent.Fairy)
            {   //use fairy in a bottle
                Pool.hero.health = PlayerData.current.heartsTotal;
                Functions_Particle.Spawn(ObjType.ParticleBottleFairy, Pool.hero);
                Assets.Play(Assets.sfxBeatDungeon);
                return true;
            }
            else if (content == BottleContent.Blob)
            {   //display the bottled blob over hero's head
                Functions_Particle.Spawn(ObjType.ParticleBottleBlob, Pool.hero);
                Assets.Play(Assets.sfxBeatDungeon);
                //use blob in a bottle (transform hero into blob and vice versa)
                if (Pool.hero.type == ActorType.Hero)
                { Functions_Actor.SetType(Pool.hero, ActorType.Blob); }
                else { Functions_Actor.SetType(Pool.hero, ActorType.Hero); }
                //reload hero's loadout from player.current data
                Functions_Hero.SetLoadout();
                Pool.hero.health = PlayerData.current.heartsTotal; //refill the hero's health
                PlayerData.current.actorType = Pool.hero.type; //save the hero's actorType
                return true;
            }
            else { return false; } //the bottle couldn't be used
        }

        public static Boolean FillEmptyBottle(BottleContent content)
        {   //find an empty bottle and fill it
            if (PlayerData.current.bottleA == BottleContent.Empty)
            { PlayerData.current.bottleA = content; return true; }

            else if (PlayerData.current.bottleB == BottleContent.Empty)
            { PlayerData.current.bottleB = content; return true; }

            else if (PlayerData.current.bottleC == BottleContent.Empty)
            { PlayerData.current.bottleC = content; return true; }

            else { return false; } //no empty bottles
        }

        public static void Bottle(GameObject Obj)
        {   //we can bottle fairys
            if(Obj.type == ObjType.Fairy)
            {
                if (FillEmptyBottle(BottleContent.Fairy))
                {   //player has bottled fairy
                    //remove fairy from room
                    Functions_Particle.Spawn(
                        ObjType.ParticleAttention,
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y);
                    Functions_Pool.Release(Obj);
                    if (Flags.ShowDialogs) //pop success dialog
                    { ScreenManager.AddScreen(new ScreenDialog(Functions_Dialog.BottleSuccess)); }

                    //set the hero into an idle state (else hero lingers in use state without net)
                    Functions_Actor.SetIdleState(Pool.hero);
                    return;
                }
                else
                {   //player has no empty bottles
                    if (Flags.ShowDialogs)
                    { ScreenManager.AddScreen(new ScreenDialog(Functions_Dialog.BottleFull)); }
                    return;
                }
            }
            //can't bottle this, pop cant bottle dialog
            if (Flags.ShowDialogs)
            { ScreenManager.AddScreen(new ScreenDialog(Functions_Dialog.BottleCant)); }
        }

        public static void Bottle(Actor Actor)
        {
            if(Actor.type == ActorType.Blob)
            {   //we can bottle blobs
                if(FillEmptyBottle(BottleContent.Blob))
                {   //player has bottled actor
                    //remove actor from room
                    Functions_Particle.Spawn(
                        ObjType.ParticleAttention,
                        Actor.compSprite.position.X,
                        Actor.compSprite.position.Y);
                    Functions_Pool.Release(Actor);
                    if (Flags.ShowDialogs) //pop success dialog
                    { ScreenManager.AddScreen(new ScreenDialog(Functions_Dialog.BottleSuccess)); }

                    //set the hero into an idle state (else hero lingers in use state without net)
                    Functions_Actor.SetIdleState(Pool.hero);
                    return;
                }
                else
                {   //player has no empty bottles
                    if (Flags.ShowDialogs)
                    { ScreenManager.AddScreen(new ScreenDialog(Functions_Dialog.BottleFull)); }
                    return;
                }
            }
            //can't bottle this, pop cant bottle dialog
            if (Flags.ShowDialogs)
            { ScreenManager.AddScreen(new ScreenDialog(Functions_Dialog.BottleCant)); }
        }

    }
}