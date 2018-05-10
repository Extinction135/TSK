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
            new Dialog(ObjType.Vendor_NPC_Story, "Hello!",
                "Press the A button to interact with objects,\n" + 
                "like signposts, torches, and doors.", 
                Assets.sfxTextLetter, false, false, false),
        };

        
        #region Utility dialogs

        public static List<Dialog> GameSaved = new List<Dialog>
        {   //returns to inventory screen upon close
            new Dialog(ObjType.Vendor_NPC_Story, "Success!",
                "I have successfully saved the current game.",
                Assets.sfxGoldPickup, true, false, false)
        };
        public static List<Dialog> GameLoaded = new List<Dialog>
        {   //returns to previous screen (inventory or title) upon close
            new Dialog(ObjType.Vendor_NPC_Story, "Ready!",
                "I have loaded the selected game file.",
                Assets.sfxChestOpen, true, true, true)
        };
        public static List<Dialog> GameAutoSaved = new List<Dialog>
        {   //goes to overworld screen upon close
            new Dialog(ObjType.Vendor_NPC_Story, "Complete!",
                "I've successfully loaded your last autosaved game.",
                Assets.sfxChestOpen, true, true, true)
        };
        public static List<Dialog> GameCreated = new List<Dialog>
        {   //goes to overworld screen upon close
            new Dialog(ObjType.Vendor_NPC_Story, "Welcome...",
                "I have created a new game for you.",
                Assets.sfxChestOpen, true, true, true)
        };
        public static List<Dialog> GameNotFound = new List<Dialog>
        {   //goes to overworld screen upon close
            new Dialog(ObjType.Vendor_NPC_Story, "UhOh!",
                "the selected game file was not found. I have saved your current game to the\n" +
                "selected game slot instead.",
                Assets.sfxError, true, true, true)
        };
        public static List<Dialog> GameLoadFailed = new List<Dialog>
        {   //goes to overworld screen upon close
            new Dialog(ObjType.Vendor_NPC_Story, "OhNo!",
                "Oh no! I'm terribly sorry, but there was a problem loading this game file...\n" +
                "The data is corrupted... I've overwritten the file with your current game.",
                Assets.sfxError, true, false, true)
        };

        #endregion




        #region Dungeon Entrance Dialogs

        public static List<Dialog> Enter_ForestDungeon = new List<Dialog>
        {
            new Dialog(ObjType.Hero_Idle, "Hmm..",
                "Should I enter this dungeon?\n" +
                "A) yes    B) no",
                Assets.sfxTextLetter, false, false, false)
        };

        #endregion




        #region Dungeon dialogs

        public static List<Dialog> DoesNotHaveKey = new List<Dialog>
        {
            new Dialog(ObjType.Vendor_NPC_Story, "Uh...!",
                "This door is locked. You'll need a key to open it.",
                Assets.sfxError, false, false, false)
        };
        public static List<Dialog> HeroGotKey = new List<Dialog>
        {
            new Dialog(ObjType.Vendor_NPC_Story, "Hey!",
                "You found the big key. It can open all doors.",
                Assets.sfxReward, false, false, false)
        };
        public static List<Dialog> HeroGotMap = new List<Dialog>
        {
            new Dialog(ObjType.Vendor_NPC_Story, "Hey!",
                "You found the map!\n" + 
                "You can look at it by selecting it from the Inventory screen.\n",
                Assets.sfxReward, false, false, false)
        };

        #endregion

        
        #region Bottle dialogs

        public static List<Dialog> BottleCant = new List<Dialog>
        {
            new Dialog(ObjType.Vendor_NPC_Story, "Gotcha!",
                "I caught a creature in the net!\n" +
                "Unfortunately, I can't fit this creature in a bottle.",
                Assets.sfxError, false, false, false)
        };
        public static List<Dialog> BottleFull = new List<Dialog>
        {
            new Dialog(ObjType.Vendor_NPC_Story, "Gotcha!",
                "I caught a creature in the net!\n" +
                "Unfortunately, I don't have any empty bottles to put this creature in.",
                Assets.sfxError, false, false, false)
        };
        public static List<Dialog> BottleSuccess = new List<Dialog>
        {
            new Dialog(ObjType.Vendor_NPC_Story, "Gotcha!",
                "I caught a creature in the net!\n" +
                "I put the creature in an empty bottle.",
                Assets.sfxReward, false, false, false)
        };

        #endregion


        #region Editor dialogs

        public static List<Dialog> CantAddChests = new List<Dialog>
        {
            new Dialog(ObjType.Vendor_NPC_Story, "Nope!",
                "You cannot add more chests to this room!",
                Assets.sfxError, true, false, false)
        };
        public static List<Dialog> CantAddMoreSwitches = new List<Dialog>
        {
            new Dialog(ObjType.Vendor_NPC_Story, "Nope!",
                "You cannot add more than 1 switch to a room!",
                Assets.sfxError, true, false, false)
        };

        #endregion

    }
}