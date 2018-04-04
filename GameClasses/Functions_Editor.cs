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
    public static class Functions_Editor
    {



        public static void SetEditorFlags()
        {   //editor specific flags
            Flags.EnableTopMenu = true; //necessary

            //don't modify the draw debug bottom display
            //Flags.DrawDebugInfo = false; //initial display, can be changed

            Flags.Invincibility = true; //hero cannot die in editor
            Flags.InfiniteMagic = true; //hero has infinite magic
            Flags.InfiniteGold = true; //hero has infinite gold
            Flags.InfiniteArrows = true; //hero has infinite arrows
            Flags.InfiniteBombs = true; //hero has infinite bombs
            Flags.CameraTracksHero = false; //center to room
            Flags.ShowEnemySpawns = true; //necessary for editing
            //Flags.ProcessAI = false; //turn off ai for testing
        }

        public static void SetEditorLoadout()
        {   //unlock most/all items
            PlayerData.current = new SaveData();
            PlayerData.current.heartsTotal = 9;
            Pool.hero.health = 3;
            PlayerData.current.bombsCurrent = 99;
            PlayerData.current.arrowsCurrent = 99;
            //set items
            PlayerData.current.bottleA = MenuItemType.BottleHealth;
            PlayerData.current.bottleB = MenuItemType.BottleMagic;
            PlayerData.current.bottleC = MenuItemType.BottleFairy;
            PlayerData.current.magicFireball = true;
            //set weapons
            PlayerData.current.weaponBow = true;
            PlayerData.current.weaponNet = true;
            //set armor
            PlayerData.current.armorCape = true;
            //set equipment
            PlayerData.current.equipmentRing = true;
        }



    }
}