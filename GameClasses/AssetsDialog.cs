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
    public static class AssetsDialog
    {
        //create instances for all the dialog in the game

        public static List<Dialog> Guide = new List<Dialog>
        {
            new Dialog(ObjType.NPC_Story, "Hi!",
                "I don't have dialog yet.\n" + 
                "Cool, huh?", 
                Assets.sfxTextLetter, false, false, false),
        };


        #region Farmer NPC Dialogs

        public static List<Dialog> Farmer_Setup = new List<Dialog>
        {   
            new Dialog(ObjType.NPC_Farmer, "Help!",
                "My bushes are too dry! Without water, they won't grow..\n" +
                "If only I had a shovel, I'd finish these ditches..",
                Assets.sfxTextLetter, false, false, false),
        };

        public static List<Dialog> Farmer_Reward = new List<Dialog>
        {
            new Dialog(ObjType.NPC_Farmer, "Thank you!",
                "You did it! You got some of my bushes to grow.\n" +
                "I can eat again! I don't have much, but take 10 bombs as a reward..",
                Assets.sfxTextLetter, false, false, false),
        };

        public static List<Dialog> Farmer_EndDialog = new List<Dialog>
        {
            new Dialog(ObjType.NPC_Farmer, "Thanks again!",
                "Thanks again for helping me with my crops.\n" +
                "I'm sure I'll need more help again, as I'm old and tired.",
                Assets.sfxTextLetter, false, false, false),
        };

        #endregion





        #region Utility dialogs

        public static List<Dialog> GameSaved = new List<Dialog>
        {   //returns to inventory screen upon close
            new Dialog(ObjType.Hero_Idle, "Success!",
                "I saved the current game.",
                Assets.sfxGoldPickup, true, false, false)
        };
        public static List<Dialog> GameLoaded = new List<Dialog>
        {   //returns to previous screen (inventory or title) upon close
            new Dialog(ObjType.Hero_Idle, "Ready!",
                "I loaded the selected game file.",
                Assets.sfxChestOpen, true, true, true)
        };
        public static List<Dialog> GameAutoSaved = new List<Dialog>
        {   //goes to overworld screen upon close
            new Dialog(ObjType.Hero_Idle, "Complete!",
                "I  successfully loaded the last autosaved game.",
                Assets.sfxChestOpen, true, true, true)
        };
        public static List<Dialog> GameCreated = new List<Dialog>
        {   //goes to overworld screen upon close
            new Dialog(ObjType.Hero_Idle, "Welcome...",
                "I created a new game for you.",
                Assets.sfxChestOpen, true, true, true)
        };
        public static List<Dialog> GameNotFound = new List<Dialog>
        {   //goes to overworld screen upon close
            new Dialog(ObjType.Hero_Idle, "Uh Oh!",
                "the selected game file was not found. I have saved your current game to the\n" +
                "selected game slot instead.",
                Assets.sfxError, true, true, true)
        };
        public static List<Dialog> GameLoadFailed = new List<Dialog>
        {   //goes to overworld screen upon close
            new Dialog(ObjType.Hero_Idle, "Oh No!",
                "Oh no! I'm terribly sorry, but there was a problem loading this game file...\n" +
                "The data is corrupted... I've overwritten the file with your current game.",
                Assets.sfxError, true, false, true)
        };

        #endregion


        #region Dungeon/Level Entrance Dialogs

        //dont fade in the foreground on these dialogs, because
        //they exit the level, which fades the foreground in
        //if we set foreground fade parameter to true (in Dialog()),
        //then a 'double-fade' happens, and it looks really bad

        public static List<Dialog> Enter_ForestDungeon = new List<Dialog>
        {
            new Dialog(ObjType.Hero_Idle, "Hmm..",
                "this hollow log looks slippery and dangerous..\n" +
                "A) enter carefully    B) i'm just not into logs..",
                Assets.sfxTextLetter, false, false, false)
        };

        public static List<Dialog> Enter_Colliseum = new List<Dialog>
        {
            new Dialog(ObjType.Hero_Idle, "Hmm..",
                "i can hear a crowd inside, along with the sounds of battle..\n" +
                "A) enter the colliseum    B) i've got other things to do..",
                Assets.sfxTextLetter, false, false, false)
        };

        public static List<Dialog> Enter_MountainDungeon = new List<Dialog>
        {
            new Dialog(ObjType.Hero_Idle, "Hmm..",
                "this dark cave smells terrible. there's probably nothing inside..\n" +
                "A) enter anyway    B) i don't like dark, musty caves..",
                Assets.sfxTextLetter, false, false, false)
        };

        #endregion


        #region Signpost Dialogs

        public static List<Dialog> Signpost_Standard = new List<Dialog>
        {
            new Dialog(ObjType.Hero_Idle, "Hmm..",
                "There doesn't seem to be anything written down..\n" +
                "It's just empty.",
                Assets.sfxTextLetter, false, false, false)
        };

        public static List<Dialog> Signpost_ForestEntrance = new List<Dialog>
        {
            new Dialog(ObjType.Hero_Idle, "Hyrule Forest",
                "'Please Respect our community - no logging!'\n" +
                "..I guess they really care about their wood.",
                Assets.sfxTextLetter, false, false, false)
        };

        public static List<Dialog> Signpost_LeftTown2 = new List<Dialog>
        {
            new Dialog(ObjType.Hero_Idle, "Hmm..",
                "it says 'welcome to left town'.\n" +
                "that's an odd name for a town.",
                Assets.sfxTextLetter, false, false, false)
        };

        public static List<Dialog> Signpost_TheFarm = new List<Dialog>
        {
            new Dialog(ObjType.Hero_Idle, "The Farm",
                "welcome to the farm.\n" +
                "...",
                Assets.sfxTextLetter, false, false, false)
        };

        public static List<Dialog> Signpost_MountainEntrance = new List<Dialog>
        {
            new Dialog(ObjType.Hero_Idle, "The Mountain",
                "Climb the mountain using A to grab onto a foothold.\n" +
                "Press B to jump off the wall. Press A to grab again, while falling.",
                Assets.sfxTextLetter, false, false, false)
        };

        #endregion






        #region Colliseum Dialogs

        public static List<Dialog> Colliseum_Judge = new List<Dialog>
        {
            new Dialog(ObjType.Judge_Colliseum, "...",
                "Complete the challenge to claim your reward!\n" +
                "Kill or be killed..",
                Assets.sfxTextLetter, false, false, false),
        };

        public static List<Dialog> Colliseum_Challenge_Complete = new List<Dialog>
        {
            new Dialog(ObjType.Judge_Colliseum, "Congratulations!",
                "You have completed the challenge, and have been rewarded.\n" +
                "We look forward to seeing your skills again soon...",
                Assets.sfxTextLetter, false, false, false),
        };

        #endregion






        #region Dungeon dialogs

        public static List<Dialog> DoesNotHaveKey = new List<Dialog>
        {
            new Dialog(ObjType.Hero_Idle, "Hmm..",
                "This door is locked.",
                Assets.sfxError, false, false, false)
        };
        public static List<Dialog> HeroGotKey = new List<Dialog>
        {
            new Dialog(ObjType.Hero_Idle, "Hey!",
                "I found a key.",
                Assets.sfxReward, false, false, false)
        };
        public static List<Dialog> HeroGotMap = new List<Dialog>
        {
            new Dialog(ObjType.Hero_Idle, "Hey!",
                "I found a map!\n" + 
                "I can look at it by selecting it from Inventory.\n",
                Assets.sfxReward, false, false, false)
        };

        #endregion

        
        #region Bottle dialogs

        public static List<Dialog> BottleCant = new List<Dialog>
        {
            new Dialog(ObjType.Hero_Idle, "Gotcha!",
                "I caught a creature in my net!\n" +
                "Unfortunately, I can't fit this creature in a bottle.",
                Assets.sfxError, false, false, false)
        };
        public static List<Dialog> BottleFull = new List<Dialog>
        {
            new Dialog(ObjType.Hero_Idle, "Gotcha!",
                "I caught a creature in my net!\n" +
                "Unfortunately, I don't have any empty bottles to put this creature in.",
                Assets.sfxError, false, false, false)
        };
        public static List<Dialog> BottleSuccess = new List<Dialog>
        {
            new Dialog(ObjType.Hero_Idle, "Gotcha!",
                "I caught a creature in my net!\n" +
                "I put the creature in an empty bottle.",
                Assets.sfxReward, false, false, false)
        };

        #endregion


        #region Editor dialogs

        public static List<Dialog> CantAddChests = new List<Dialog>
        {
            new Dialog(ObjType.Hero_Idle, "Nope!",
                "You cant add more chests to this room!",
                Assets.sfxError, true, false, false)
        };
        public static List<Dialog> CantAddMoreSwitches = new List<Dialog>
        {
            new Dialog(ObjType.Hero_Idle, "Nope!",
                "You cant add more than 1 switch to a room!",
                Assets.sfxError, true, false, false)
        };

        #endregion




        #region Achievement Dialogs

        public static List<Dialog> Achievement_WallJumps_10 = new List<Dialog>
        {   //returns to inventory screen upon close
            new Dialog(ObjType.Hero_Idle, "Nice!",
                "That's 10 successful wall jumps."
                + "\nI'm getting pretty good at climbing..",
                Assets.sfxKeyPickup, false, false, false)
        };
        public static List<Dialog> Achievement_WallJumps_100 = new List<Dialog>
        {   //returns to inventory screen upon close
            new Dialog(ObjType.Hero_Idle, "Mastery!",
                "That's 100 successful wall jumps."
                + "\nI've mastered climbing..",
                Assets.sfxKeyPickup, false, false, false)
        };

        #endregion


    }
}