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



    public static class Challenges
    {
        static int i;
        public static List<ActorType> Blobs;

        public static List<ActorType> Mini_Blackeyes;
        public static List<ActorType> Mini_Spiders;

        public static List<ActorType> Bosses_BigEye;
        public static List<ActorType> Bosses_BigBat;

        public static List<ActorType> Bosses_Kraken;

        static Challenges()
        {
            Blobs = new List<ActorType>();
            for (i = 0; i < 5; i++)
            {
                Blobs.Add(ActorType.Blob);
                Blobs.Add(ActorType.Standard_AngryEye);
                Blobs.Add(ActorType.Standard_BeefyBat);
            }

            //minis
            Mini_Blackeyes = new List<ActorType>();
            for (i = 0; i < 2; i++)
            { Mini_Blackeyes.Add(ActorType.MiniBoss_BlackEye); }

            Mini_Spiders = new List<ActorType>();
            for (i = 0; i < 2; i++)
            { Mini_Spiders.Add(ActorType.MiniBoss_Spider_Armored); }

            //boss
            Bosses_BigEye = new List<ActorType>();
            for (i = 0; i < 1; i++)
            { Bosses_BigEye.Add(ActorType.Boss_BigEye); }

            Bosses_BigBat = new List<ActorType>();
            for (i = 0; i < 1; i++)
            { Bosses_BigBat.Add(ActorType.Boss_BigBat); }

            Bosses_Kraken = new List<ActorType>();
            for (i = 0; i < 1; i++)
            { Bosses_Kraken.Add(ActorType.Boss_OctoHead); }
            for (i = 0; i < 4; i++)
            { Bosses_Kraken.Add(ActorType.Special_Tentacle); }
        }
    }



    public static class Functions_Colliseum
    {
        //contains pit spawning routines for light and dark colliseums
        static int i;
        public static InteractiveObject objRef;
        public static Actor actorRef;
        public static List<ActorType> currentChallenge;



        public static void BeginChallenge(List<ActorType> Challenge)
        {
            //rebuild the colliseum pit, fading in from black
            Functions_Level.BuildLevel(LevelSet.currentLevel.ID);

            //this method assums the current level + room is colliseum pit
            currentChallenge = Challenge;

            //remove all vendors (and any other decoration)
            for (i = 0; i < Pool.intObjCount; i++)
            {
                if(Pool.intObjPool[i].active)
                {   //remove all vendors
                    if (Pool.intObjPool[i].group == InteractiveGroup.Vendor)
                    { Functions_Pool.Release(Pool.intObjPool[i]); }
                    //remove any additional decorations here, by checking type
                }
            }

            //loop over ChallengeSet.actors, creating them centered near back wall
            for (i = 0; i < Challenge.Count; i++)
            {
                actorRef = Functions_Pool.GetActor();
                //place middle top
                Functions_Movement.Teleport(
                    actorRef.compMove,

                    //X
                    LevelSet.currentLevel.currentRoom.rec.X //start with the room's x pos
                    + LevelSet.currentLevel.currentRoom.rec.Width / 2 //get the center X
                    + 16 * Functions_Random.Int(-5, 5) //add random offset
                    + 8, //add offset to exactly center to room, because ocd
                         //Y
                    LevelSet.currentLevel.currentRoom.rec.Y + 16 * 24 //near center / back wall
                    + 16 * Functions_Random.Int(-4, 4) //add random offset
                    
                    ); 
                actorRef.direction = Direction.Down;
                Functions_Actor.SetType(actorRef, Challenge[i]);
            }

            //close off the pit exit, so challenge set enemies can't escape
            //creating an unwinnable and unfair gamestate
            for (i = 0; i < 10; i++)
            {
                objRef = Functions_Pool.GetIntObj();
                //block off the colliseum entrance
                Functions_Movement.Teleport(
                    objRef.compMove,

                    LevelSet.currentLevel.currentRoom.rec.X //start with the room's x pos
                    + LevelSet.currentLevel.currentRoom.rec.Width / 2 //get the center X
                    - 16 * 5 //move 5 tiles left
                    + 16 * i //from there, place objs in a horizontal line
                    + 8, //with a small alignment offset

                    //set Ypos
                    LevelSet.currentLevel.currentRoom.rec.Y //start with Y pos
                    + LevelSet.currentLevel.currentRoom.rec.Height //add height
                    - 16 - 8); //apply final offset

                objRef.direction = Direction.Down;
                Functions_InteractiveObjs.SetType(objRef, InteractiveType.Barrel);
            }

            //respawn hero inside of pit - this line is actually not needed,
            //because spawn in current room is called from level.buildLevel()..
            Functions_Hero.SpawnInCurrentRoom();

            //create the judge obj
            objRef = Functions_Pool.GetIntObj();

            //place judge along backwall
            Functions_Movement.Teleport(
                objRef.compMove,
                LevelSet.currentLevel.currentRoom.rec.X //start with the room's x pos
                + LevelSet.currentLevel.currentRoom.rec.Width / 2 //get the center X
                + 8, //align to gameworld grid
                LevelSet.currentLevel.currentRoom.rec.Y + 16 * 14 - 8 - 4); //against back wall
            objRef.direction = Direction.Down;
            Functions_InteractiveObjs.SetType(objRef, InteractiveType.Judge_Colliseum);

            //change the crowd's atmosphere to be more aggressive
            Functions_Music.PlayMusic(Music.CrowdFighting);
        }

        


    }
}