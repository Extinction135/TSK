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
    public static class Functions_Dialog
    {
        //create instances for all the dialog in the game



        public static List<Dialog> Default = new List<Dialog>
        {
            new Dialog(ObjType.VendorStory, "Hello!",
                "i'm the guide. in a future update i'll explain the story.\n" + 
                "for now, just have fun playtesting the game.", 
                Assets.sfxHeartPickup, false, false, false),

            new Dialog(ObjType.VendorStory, "Hmmm...",
                "actually, i'm going to keep bugging you.\n" +
                "not alot, but just enough to test the new dialog system..",
                Assets.sfxGoldPickup, false, false, false),

            new Dialog(ObjType.VendorStory, "...",
                "ok. i'm done. thanks!",
                Assets.sfxReward, false, false, false)

        };



        #region Utility dialogs

        public static List<Dialog> GameSaved = new List<Dialog>
        {   //returns to inventory screen upon close
            new Dialog(ObjType.VendorStory, "Success!",
                "I have successfully saved the current game.",
                Assets.sfxGoldPickup, true, false, false)
        };
        public static List<Dialog> GameLoaded = new List<Dialog>
        {   //returns to previous screen (inventory or title) upon close
            new Dialog(ObjType.VendorStory, "Ready!",
                "I have loaded the selected game file.",
                Assets.sfxChestOpen, true, true, true)
        };
        public static List<Dialog> GameAutoSaved = new List<Dialog>
        {   //goes to overworld screen upon close
            new Dialog(ObjType.VendorStory, "Complete!",
                "I've successfully loaded your last autosaved game.",
                Assets.sfxChestOpen, true, true, true)
        };
        public static List<Dialog> GameCreated = new List<Dialog>
        {
            new Dialog(ObjType.VendorStory, "Welcome...",
                "I have created a new game for you.",
                Assets.sfxChestOpen, true, true, true)
        };
        public static List<Dialog> GameNotFound = new List<Dialog>
        {   //goes to overworld screen upon close
            new Dialog(ObjType.VendorStory, "UhOh!",
                "the selected game file was not found. I have saved your current game to the\n" +
                "selected game slot instead.",
                Assets.sfxError, true, true, true)
        };
        public static List<Dialog> GameLoadFailed = new List<Dialog>
        {
            new Dialog(ObjType.VendorStory, "OhNo!",
                "Oh no! I'm terribly sorry, but there was a problem loading this game file...\n" +
                "The data is corrupted... I've overwritten the file with your current game.",
                Assets.sfxError, true, false, true)
        };

        #endregion



        #region Dungeon dialogs

        public static List<Dialog> DoesNotHaveKey = new List<Dialog>
        {
            new Dialog(ObjType.VendorStory, "UhOh!",
                "This door is locked. You'll need a key to open it.",
                Assets.sfxError, false, false, false)
        };
        public static List<Dialog> HeroGotKey = new List<Dialog>
        {
            new Dialog(ObjType.VendorStory, "Congrats!",
                "You found the dungeon key. It can open any door.",
                Assets.sfxReward, false, false, false)
        };
        public static List<Dialog> HeroGotMap = new List<Dialog>
        {
            new Dialog(ObjType.VendorStory, "Congrats!",
                "You found the dungeon map! This map reveals the location of all rooms.\n" +
                "Press the Left Shoulder button to view this dungeon map.",
                Assets.sfxReward, false, false, false)
        };

        #endregion



        #region Bottle dialogs

        public static List<Dialog> BottleCant = new List<Dialog>
        {
            new Dialog(ObjType.VendorStory, "UhOh!",
                "You caught a creature in your net!\n" +
                "Unfortunately, you can't fit this creature in a bottle.",
                Assets.sfxError, false, false, false)
        };
        public static List<Dialog> BottleFull = new List<Dialog>
        {
            new Dialog(ObjType.VendorStory, "UhOh!",
                "You caught a creature in your net!\n" +
                "Unfortunately, You don't have any empty bottles to put this creature in.",
                Assets.sfxError, false, false, false)
        };
        public static List<Dialog> BottleSuccess = new List<Dialog>
        {
            new Dialog(ObjType.VendorStory, "Congrats!",
                "You caught a creature in your net!\n" +
                "You have successfully captured this creature in an empty bottle.",
                Assets.sfxReward, false, false, false)
        };

        #endregion



        #region Editor dialogs

        public static List<Dialog> CantAddKeyChest = new List<Dialog>
        {
            new Dialog(ObjType.VendorStory, "Nope!",
                "You cannot add a key chest to a non-key room!",
                Assets.sfxError, true, false, false)
        };
        public static List<Dialog> CantAddMapChest = new List<Dialog>
        {
            new Dialog(ObjType.VendorStory, "Nope!",
                "You cannot add a map chest to a non-hub room!",
                Assets.sfxError, true, false, false)
        };
        public static List<Dialog> CantAddMoreChests = new List<Dialog>
        {
            new Dialog(ObjType.VendorStory, "Nope!",
                "You cannot add more than 1 chest to a room!",
                Assets.sfxError, true, false, false)
        };

        #endregion

    }
}