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
            if (PlayerData.current.bottleA == MenuItemType.BottleFairy
                || PlayerData.current.bottleB == MenuItemType.BottleFairy
                || PlayerData.current.bottleC == MenuItemType.BottleFairy)
            {
                Functions_Item.UseItem(MenuItemType.BottleFairy, Pool.hero);
                return true;
            } else { return false; }
        }

        public static void EmptyBottle(MenuItemType Type)
        {   //set hero into reward state, clear item, play bottle sfx
            Functions_Actor.SetRewardState(Pool.hero);
            Pool.hero.item = MenuItemType.Unknown;
            Assets.Play(Assets.sfxBeatDungeon);
            Functions_Particle.Spawn(ObjType.Particle_Attention, Pool.hero);

            //hero used current hero.item, which was a bottle, play soundfx
            //find the first menuItemType of used bottle in bottle inventory
            //set that inventory bottle to empty
            if (PlayerData.current.bottleA == Type)
            { PlayerData.current.bottleA = MenuItemType.BottleEmpty; }
            else if (PlayerData.current.bottleB == Type)
            { PlayerData.current.bottleB = MenuItemType.BottleEmpty; }
            else if (PlayerData.current.bottleC == Type)
            { PlayerData.current.bottleC = MenuItemType.BottleEmpty; }
        }

        public static Boolean FillBottle(MenuItemType Type)
        {   //fill bottle A B or C with passed Type value
            if (PlayerData.current.bottleA == MenuItemType.BottleEmpty)
            { PlayerData.current.bottleA = Type; return true; }
            else if (PlayerData.current.bottleB == MenuItemType.BottleEmpty)
            { PlayerData.current.bottleB = Type; return true; }
            else if (PlayerData.current.bottleC == MenuItemType.BottleEmpty)
            { PlayerData.current.bottleC = Type; return true; }
            else { return false; }
        }

        public static void Bottle(GameObject Obj)
        {   //we can bottle fairys
            if(Obj.type == ObjType.Dungeon_Fairy)
            {
                if (FillBottle(MenuItemType.BottleFairy))
                {   //player has bottled fairy
                    //remove fairy from room
                    Functions_Particle.Spawn(
                        ObjType.Particle_Attention,
                        Obj.compSprite.position.X,
                        Obj.compSprite.position.Y);
                    Functions_Pool.Release(Obj);
                    ScreenManager.AddScreen(new ScreenDialog(AssetsDialog.BottleSuccess));
                    //set the hero into an idle state (else hero lingers in use state without net)
                    Functions_Actor.SetIdleState(Pool.hero);
                    return;
                }
                else
                {   //player has no empty bottles
                    ScreenManager.AddScreen(new ScreenDialog(AssetsDialog.BottleFull));
                    return;
                }
            }
            //can't bottle this, pop cant bottle dialog
            ScreenManager.AddScreen(new ScreenDialog(AssetsDialog.BottleCant));
        }

        public static void Bottle(Actor Actor)
        {
            if(Actor.type == ActorType.Blob)
            {   //we can bottle blobs
                if(FillBottle(MenuItemType.BottleBlob))
                {   //player has bottled actor
                    //remove actor from room
                    Functions_Particle.Spawn(
                        ObjType.Particle_Attention,
                        Actor.compSprite.position.X,
                        Actor.compSprite.position.Y);
                    Functions_Pool.Release(Actor);
                    ScreenManager.AddScreen(new ScreenDialog(AssetsDialog.BottleSuccess));
                    //set the hero into an idle state (else hero lingers in use state without net)
                    Functions_Actor.SetIdleState(Pool.hero);
                    return;
                }
                else
                {   //player has no empty bottles
                    ScreenManager.AddScreen(new ScreenDialog(AssetsDialog.BottleFull));
                    return;
                }
            }
            //can't bottle this, pop cant bottle dialog
            ScreenManager.AddScreen(new ScreenDialog(AssetsDialog.BottleCant));
        }

    }
}