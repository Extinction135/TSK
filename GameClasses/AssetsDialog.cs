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
            new Dialog(InteractiveType.NPC_Story, "Hello!",
                "I'm the guide. There's 3 dungeons, and some items to play around with.\n" +
                "Not much of a story or main quest though, so just have fun for now..", 
                Assets.sfxTextLetter, false, false),
        };




        #region Brandy Dialogs

        public static List<Dialog> Brandy_Default = new List<Dialog>
        {
            new Dialog(InteractiveType.Boat_Captain_Brandy, "Hmm..",
                "I'm not familiar with this place..\n" +
                "Must be new..",
                Assets.sfxTextLetter, false, false),
        };

        public static List<Dialog> Brandy_MountainEntrance = new List<Dialog>
        {
            new Dialog(InteractiveType.Boat_Captain_Brandy, "I see something up there..",
                "..there's a cave up that mountain wall.. you can reach it if\n" +
                "you climb for a bit. be careful.",
                Assets.sfxTextLetter, false, false),
        };
        public static List<Dialog> Brandy_ForestEntrance = new List<Dialog>
        {
            new Dialog(InteractiveType.Boat_Captain_Brandy, "Hmm..",
                "It seems like there might be something between those giant\n" +
                "teeth over there in the water.. a cave of some sort..",
                Assets.sfxTextLetter, false, false),
        };
        public static List<Dialog> Brandy_SwampEntrance = new List<Dialog>
        {
            new Dialog(InteractiveType.Boat_Captain_Brandy, "Psst..",
                "You can dash while swimming by pressing or holding the B button.\n" +
                "You can also dive underwater for awhile by pressing the X button.",
                Assets.sfxTextLetter, false, false),
        };



        #endregion





        










        #region Farmer NPC Dialogs

        public static List<Dialog> Farmer_Setup = new List<Dialog>
        {   
            new Dialog(InteractiveType.NPC_Farmer, "Help!",
                "My bushes are too dry! Without water, they won't grow..\n" +
                "If only I had a shovel, I'd finish these ditches..",
                Assets.sfxTextLetter, false, false),
        };

        public static List<Dialog> Farmer_Reward = new List<Dialog>
        {
            new Dialog(InteractiveType.NPC_Farmer, "Thank you!",
                "You did it! You got some of my bushes to grow.\n" +
                "I can eat again! I don't have much, but take 10 bombs as a reward..",
                Assets.sfxTextLetter, false, false),
        };

        public static List<Dialog> Farmer_EndDialog = new List<Dialog>
        {
            new Dialog(InteractiveType.NPC_Farmer, "Thanks again!",
                "Thanks again for helping me with my crops.\n" +
                "I'm sure I'll need more help again, as I'm old and tired.",
                Assets.sfxTextLetter, false, false),
        };

        #endregion





        #region Game Option / Utility dialogs

        public static List<Dialog> GameCreated = new List<Dialog>
        {   //goes to overworld screen upon close
            new Dialog(InteractiveType.DialogObj_Hero_Idle, "Welcome...",
                "I created a new game for you.",
                Assets.sfxChestOpen, true, true)
        };

        public static List<Dialog> AreYouSure = new List<Dialog>
        {   //goes to overworld screen upon close
            new Dialog(InteractiveType.DialogObj_Hero_Idle, "Hey!",
                "If you quit, all your progress will be lost..\nAre You Sure?    A) Yes    B) No",
                Assets.sfxChestOpen, true, true)
        };

        #endregion


        #region Dungeon/Level Entrance Dialogs


        public static List<Dialog> Enter_Colliseum = new List<Dialog>
        {
            new Dialog(InteractiveType.DialogObj_Hero_Idle, "Hmm..",
                "i can hear a crowd inside, along with the sounds of battle..\n" +
                "A) enter the colliseum    B) i've got other things to do..",
                Assets.sfxTextLetter, false, false)
        };




        //dungeon entrance dialogs
        public static List<Dialog> Enter_ForestDungeon = new List<Dialog>
        {
            new Dialog(InteractiveType.DialogObj_Hero_Idle, "Hmm..",
                "forest dungeon entrance\n" +
                "A) enter    B) leave",
                Assets.sfxTextLetter, false, false)
        };
        public static List<Dialog> Enter_MountainDungeon = new List<Dialog>
        {
            new Dialog(InteractiveType.DialogObj_Hero_Idle, "Hmm..",
                "mountain dungeon entrance\n" +
                "A) enter    B) leave",
                Assets.sfxTextLetter, false, false)
        };
        public static List<Dialog> Enter_SwampDungeon = new List<Dialog>
        {
            new Dialog(InteractiveType.DialogObj_Hero_Idle, "Hmm..",
                "swamp dungeon entrance..\n" +
                "A) enter    B) leave",
                Assets.sfxTextLetter, false, false)
        };
        public static List<Dialog> Enter_ThievesDungeon = new List<Dialog>
        {
            new Dialog(InteractiveType.DialogObj_Hero_Idle, "Hmm..",
                "thieves dungeon entrance\n" +
                "A) enter    B) leave",
                Assets.sfxTextLetter, false, false)
        };
        public static List<Dialog> Enter_LavaDungeon = new List<Dialog>
        {
            new Dialog(InteractiveType.DialogObj_Hero_Idle, "Hmm..",
                "lava dungeon entrance\n" +
                "A) enter    B) leave",
                Assets.sfxTextLetter, false, false)
        };
        public static List<Dialog> Enter_CloudDungeon = new List<Dialog>
        {
            new Dialog(InteractiveType.DialogObj_Hero_Idle, "Hmm..",
                "cloud dungeon entrance\n" +
                "A) enter    B) leave",
                Assets.sfxTextLetter, false, false)
        };
        public static List<Dialog> Enter_SkullDungeon = new List<Dialog>
        {
            new Dialog(InteractiveType.DialogObj_Hero_Idle, "Hmm..",
                "shadow dungeon entrance\n" +
                "A) enter    B) leave",
                Assets.sfxTextLetter, false, false)
        };







        #endregion










        #region Signpost Dialogs


        public static List<Dialog> Signpost_Standard = new List<Dialog>
        {
            new Dialog(InteractiveType.DialogObj_Hero_Idle, "Hmm..",
                "There doesn't seem to be anything written down..\n" +
                "It's just empty.",
                Assets.sfxTextLetter, false, false)
        };


        //field level signposts
        public static List<Dialog> Signpost_ForestEntrance = new List<Dialog>
        {
            new Dialog(InteractiveType.DialogObj_Hero_Idle, "The Forest Dungeon",
                "A small, easy dungeon.\n" +
                "Have fun!",
                Assets.sfxTextLetter, false, false)
        };
        public static List<Dialog> Signpost_MountainEntrance = new List<Dialog>
        {
            new Dialog(InteractiveType.DialogObj_Hero_Idle, "The Mountain Dungeon",
                "Climb using A to grab onto a foothold.\n" +
                "Climb up the mountain to the cave. Med difficulty.",
                Assets.sfxTextLetter, false, false)
        };
        public static List<Dialog> Signpost_SwampEntrance = new List<Dialog>
        {
            new Dialog(InteractiveType.DialogObj_Hero_Idle, "The Swamp Dungeon",
                "Caution: End game dungeon. Enter the Shadow above to begin.\n" +
                "Do not touch the lever to left ..You were warned.",
                Assets.sfxTextLetter, false, false)
        };
        public static List<Dialog> Signpost_ShadowTown = new List<Dialog>
        {
            new Dialog(InteractiveType.DialogObj_Hero_Idle, "Shadow Town",
                "all vendors located here for your convienence..\n" +
                "...",
                Assets.sfxTextLetter, false, false)
        };
        public static List<Dialog> Signpost_ShadowColliseum = new List<Dialog>
        {
            new Dialog(InteractiveType.DialogObj_Hero_Idle, "Shadow Coliseum",
                "The Colliseum is officially open.\n" +
                "Win gold for defeating enemies, or die shamefully.",
                Assets.sfxTextLetter, false, false)
        };





        
        //dungeon signposts
        public static List<Dialog> Signpost_ExitRoom = new List<Dialog>
        {
            new Dialog(InteractiveType.DialogObj_Hero_Idle, "this gets overwritten",
                "overwritten by heros interaction\n" +
                "with signpost, so current data is fetched.",
                Assets.sfxTextLetter, false, false)
        };

        public static List<Dialog> Signpost_SecretVendor = new List<Dialog>
        {
            new Dialog(InteractiveType.DialogObj_Hero_Idle, "Attention!",
                "If you see anyone giving away secret items, please report them.\n" +
                "-MGMT (I promise not to turn them into a chicken, like last time).",
                Assets.sfxTextLetter, false, false)
        };




        #endregion













        #region Colliseum Dialogs

        public static List<Dialog> Colliseum_Judge = new List<Dialog>
        {
            new Dialog(InteractiveType.Judge_Colliseum, "...",
                "Complete the challenge to claim your reward!\n" +
                "Kill or be killed..",
                Assets.sfxTextLetter, false, false),
        };

        public static List<Dialog> Colliseum_Challenge_Complete = new List<Dialog>
        {
            new Dialog(InteractiveType.Judge_Colliseum, "Congratulations!",
                "You have completed the challenge, and have been rewarded..\n" +
                "The Shadow King was entertained by your victory.",
                Assets.sfxTextLetter, false, false),
        };

        public static List<Dialog> Colliseum_Challenge_Failure = new List<Dialog>
        {
            new Dialog(InteractiveType.Judge_Colliseum, "Amusing!",
                "You died shamefully in the shadow coliseum..\n" +
                "The Shadow King was entertained by your defeat.",
                Assets.sfxTextLetter, false, false),
        };

        #endregion






        #region Dungeon dialogs

        public static List<Dialog> DoesNotHaveKey = new List<Dialog>
        {
            new Dialog(InteractiveType.DialogObj_Hero_Idle, "Hmm..",
                "This door is locked.",
                Assets.sfxError, false, false)
        };
        public static List<Dialog> HeroGotKey = new List<Dialog>
        {
            new Dialog(InteractiveType.DialogObj_Hero_Idle, "Hey!",
                "I found a key.",
                Assets.sfxReward, false, false)
        };
        public static List<Dialog> HeroGotMap = new List<Dialog>
        {
            new Dialog(InteractiveType.DialogObj_Hero_Idle, "Hey!",
                "I found a map!\n" + 
                "I can look at it by selecting it from Inventory.\n",
                Assets.sfxReward, false, false)
        };

        #endregion

        
        #region Bottle dialogs

        public static List<Dialog> BottleCant = new List<Dialog>
        {
            new Dialog(InteractiveType.DialogObj_Hero_Idle, "Gotcha!",
                "I caught a creature in my net!\n" +
                "Unfortunately, I can't fit this creature in a bottle.",
                Assets.sfxError, false, false)
        };
        public static List<Dialog> BottleFull = new List<Dialog>
        {
            new Dialog(InteractiveType.DialogObj_Hero_Idle, "Gotcha!",
                "I caught a creature in my net!\n" +
                "Unfortunately, I don't have any empty bottles to put this creature in.",
                Assets.sfxError, false, false)
        };
        public static List<Dialog> BottleSuccess = new List<Dialog>
        {
            new Dialog(InteractiveType.DialogObj_Hero_Idle, "Gotcha!",
                "I caught a creature in my net!\n" +
                "I put the creature in an empty bottle.",
                Assets.sfxReward, false, false)
        };

        #endregion


        #region Editor dialogs

        public static List<Dialog> CantAddChests = new List<Dialog>
        {
            new Dialog(InteractiveType.DialogObj_Hero_Idle, "Nope!",
                "You cant add more chests to this room!",
                Assets.sfxError, true, false)
        };
        public static List<Dialog> CantAddMoreSwitches = new List<Dialog>
        {
            new Dialog(InteractiveType.DialogObj_Hero_Idle, "Nope!",
                "You cant add more than 1 switch to a room!",
                Assets.sfxError, true, false)
        };

        public static List<Dialog> Signpost_CantRead = new List<Dialog>
        {
            new Dialog(InteractiveType.DialogObj_Hero_Idle, "error",
                "this room has an improper id, or you're in the \n" +
                "editor, so every room is a dev room.",
                Assets.sfxTextLetter, false, false)
        };

        #endregion




        #region Achievement Dialogs

        public static List<Dialog> Achievement_WallJumps_10 = new List<Dialog>
        {   //returns to inventory screen upon close
            new Dialog(InteractiveType.DialogObj_Hero_Idle, "Nice!",
                "That's 10 successful wall jumps."
                + "\nI'm getting pretty good at climbing..",
                Assets.sfxKeyPickup, false, false)
        };
        public static List<Dialog> Achievement_WallJumps_100 = new List<Dialog>
        {   //returns to inventory screen upon close
            new Dialog(InteractiveType.DialogObj_Hero_Idle, "Mastery!",
                "That's 100 successful wall jumps."
                + "\nI've mastered climbing..",
                Assets.sfxKeyPickup, false, false)
        };

        #endregion


    }
}