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
        public static List<ActorType> Minibosses;
        public static List<ActorType> Bosses;

        static Challenges()
        {
            Blobs = new List<ActorType>();
            for (i=0; i<25; i++)
            { Blobs.Add(ActorType.Blob); }

            Minibosses = new List<ActorType>();
            for (i = 0; i < 5; i++)
            { Minibosses.Add(ActorType.MiniBoss_BlackEye); }

            Bosses = new List<ActorType>();
            for (i = 0; i < 3; i++)
            { Bosses.Add(ActorType.Boss_BigEye); }
        }
    }



    public static class Functions_Colliseum
    {
        //contains pit spawning routines for light and dark colliseums
        static int i;
        public static GameObject objRef;
        public static Actor actorRef;
        public static List<ActorType> currentChallenge;



        public static void BeginChallenge(List<ActorType> Challenge)
        {
            //rebuild the pit, fading in from black
            Functions_Level.BuildLevel(Level.ID);

            //this method assums the current level + room is colliseum pit
            //note this method works for both light and dark world colliseums :p
            currentChallenge = Challenge;

            //remove all vendors (and any other decoration)
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if(Pool.roomObjPool[i].active)
                {   //remove all vendors
                    if (Pool.roomObjPool[i].group == ObjGroup.Vendor)
                    { Functions_Pool.Release(Pool.roomObjPool[i]); }
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
                    Level.currentRoom.rec.X //start with the room's x pos
                    + Level.currentRoom.rec.Width / 2 //get the center X
                    + 16 * Functions_Random.Int(-5, 5) //add random offset
                    + 8, //add offset to exactly center to room, because ocd
                    //Y
                    Level.currentRoom.rec.Y + 16 * 24 //near center / back wall
                    + 16 * Functions_Random.Int(-4, 4) //add random offset
                    
                    ); 
                actorRef.direction = Direction.Down;
                Functions_Actor.SetType(actorRef, Challenge[i]);
            }

            //close off the pit exit, so challenge set enemies can't escape
            //creating an unwinnable and unfair gamestate
            for (i = 0; i < 10; i++)
            {
                objRef = Functions_Pool.GetRoomObj();
                //block off the colliseum entrance
                Functions_Movement.Teleport(
                    objRef.compMove,

                    Level.currentRoom.rec.X //start with the room's x pos
                    + Level.currentRoom.rec.Width / 2 //get the center X
                    - 16 * 5 //move 5 tiles left
                    + 16 * i //from there, place objs in a horizontal line
                    + 8, //with a small alignment offset

                    Level.currentRoom.spawnPos.Y + 16 * 2 - 8); //place behind enemies
                objRef.direction = Direction.Down;
                Functions_GameObject.SetType(objRef, ObjType.Wor_Colliseum_Pillar_Top);
            }

            //respawn hero inside of pit
            Functions_Hero.SpawnInCurrentRoom();

            //create the judge obj
            objRef = Functions_Pool.GetRoomObj();

            //place judge along backwall
            Functions_Movement.Teleport(
                objRef.compMove,
                Level.currentRoom.rec.X //start with the room's x pos
                + Level.currentRoom.rec.Width / 2 //get the center X
                + 8, //align to gameworld grid
                Level.currentRoom.rec.Y + 16 * 14 - 8 - 4); //against back wall
            objRef.direction = Direction.Down;
            Functions_GameObject.SetType(objRef, ObjType.Judge_Colliseum);

            //change the crowd's atmosphere to be more aggressive
            Functions_Music.PlayMusic(Music.CrowdFighting);
        }

        


    }
}