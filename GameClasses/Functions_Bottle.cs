﻿using System;
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
            if (PlayerData.bottleA == MenuItemType.BottleFairy
                || PlayerData.bottleB == MenuItemType.BottleFairy
                || PlayerData.bottleC == MenuItemType.BottleFairy)
            {
                Functions_Item.UseItem(MenuItemType.BottleFairy, Pool.hero);
                return true;
            }
            else if(Flags.InfiniteFairies)
            {
                Functions_Item.UseItem(MenuItemType.BottleFairy, Pool.hero);
                return true;
            }
            else { return false; }
        }

        public static void EmptyBottle(MenuItemType Type)
        {   //set hero into reward state, clear item, play bottle sfx
            Functions_Hero.SetRewardState(ParticleType.RewardBottle);
            Pool.hero.item = MenuItemType.Unknown;
            Assets.Play(Assets.sfxBeatDungeon);
            Functions_Particle.Spawn(ParticleType.Attention, Pool.hero);

            //hero used current hero.item, which was a bottle, play soundfx
            //find the first menuItemType of used bottle in bottle inventory
            //set that inventory bottle to empty
            if (PlayerData.bottleA == Type)
            { PlayerData.bottleA = MenuItemType.BottleEmpty; }
            else if (PlayerData.bottleB == Type)
            { PlayerData.bottleB = MenuItemType.BottleEmpty; }
            else if (PlayerData.bottleC == Type)
            { PlayerData.bottleC = MenuItemType.BottleEmpty; }
        }

        public static Boolean FillBottle(MenuItemType Type)
        {   //fill bottle A B or C with passed Type value
            if (PlayerData.bottleA == MenuItemType.BottleEmpty)
            { PlayerData.bottleA = Type; return true; }
            else if (PlayerData.bottleB == MenuItemType.BottleEmpty)
            { PlayerData.bottleB = Type; return true; }
            else if (PlayerData.bottleC == MenuItemType.BottleEmpty)
            { PlayerData.bottleC = Type; return true; }
            else { return false; }
        }

        public static void Bottle(InteractiveObject Obj)
        {   //we can bottle fairys
            if(Obj.type == InteractiveType.Fairy)
            {
                if (FillBottle(MenuItemType.BottleFairy))
                {   //player has bottled fairy
                    //remove fairy from room
                    Functions_Particle.Spawn(
                        ParticleType.Attention,
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y);
                    Functions_Pool.Release(Obj);
                    Screens.Dialog.SetDialog(AssetsDialog.BottleSuccess);
                    ScreenManager.AddScreen(Screens.Dialog);

                    //set the hero into an idle state (else hero lingers in use state without net)
                    Functions_Actor.SetIdleState(Pool.hero);
                    return;
                }
                else
                {   //player has no empty bottles
                    Screens.Dialog.SetDialog(AssetsDialog.BottleFull);
                    ScreenManager.AddScreen(Screens.Dialog);

                    return;
                }
            }
            //can't bottle this, pop cant bottle dialog
            Screens.Dialog.SetDialog(AssetsDialog.BottleCant);
            ScreenManager.AddScreen(Screens.Dialog);
        }

        public static void Bottle(Actor Actor)
        {
            if(Actor.type == ActorType.Blob)
            {   //we can bottle blobs
                if(FillBottle(MenuItemType.BottleBlob))
                {   //player has bottled actor
                    //remove actor from room
                    Functions_Particle.Spawn(
                        ParticleType.Attention,
                        Actor.compSprite.position.X,
                        Actor.compSprite.position.Y);
                    Functions_Pool.Release(Actor);
                    Screens.Dialog.SetDialog(AssetsDialog.BottleSuccess);
                    ScreenManager.AddScreen(Screens.Dialog);

                    //set the hero into an idle state (else hero lingers in use state without net)
                    Functions_Actor.SetIdleState(Pool.hero);
                    return;
                }
                else
                {   //player has no empty bottles
                    Screens.Dialog.SetDialog(AssetsDialog.BottleFull);
                    ScreenManager.AddScreen(Screens.Dialog);
                    return;
                }
            }
            //can't bottle this, pop cant bottle dialog
            Screens.Dialog.SetDialog(AssetsDialog.BottleCant);
            ScreenManager.AddScreen(Screens.Dialog);

        }

    }
}