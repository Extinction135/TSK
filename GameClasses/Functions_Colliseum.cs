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



    public static class ChallengeSets
    {
        static int i;
        public static ChallengeSet Blobs;
        public static ChallengeSet Minibosses;

        static ChallengeSets()
        {
            Blobs = new ChallengeSet();
            for (i=0; i<25; i++)
            { Blobs.actors.Add(ActorType.Blob); }

            Minibosses = new ChallengeSet();
            for (i = 0; i < 3; i++)
            { Minibosses.actors.Add(ActorType.MiniBoss_BlackEye); }

            /*
            //add some testing actors
            Blobs.actors.Add(ActorType.MiniBoss_BlackEye);
            Blobs.actors.Add(ActorType.Blob);
            Blobs.actors.Add(ActorType.Boss_BigEye_Mob);
            //add some testing enemies
            Blobs.roomObjs.Add(ObjType.Wor_Enemy_Rat);
            Blobs.roomObjs.Add(ObjType.Wor_Enemy_Crab);
            Blobs.roomObjs.Add(ObjType.Wor_Enemy_Turtle);
            Blobs.roomObjs.Add(ObjType.Wor_Enemy_Rat);
            */
        }
    }



    public static class Functions_Colliseum
    {
        //contains pit spawning routines for light and dark colliseums
        static int i;
        public static GameObject objRef;
        public static Actor actorRef;
        public static ChallengeSet currentChallenge;



        public static void BeginChallenge(ChallengeSet ChallengeSet)
        {
            //this method assums the current level + room is colliseum pit
            //note this method works for both light and dark world colliseums :p
            currentChallenge = ChallengeSet;

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

            //loop over ChallengeSet.roomObjs, creating them near back wall
            for (i = 0; i < ChallengeSet.roomObjs.Count; i++)
            {
                objRef = Functions_Pool.GetRoomObj();
                //place along backwall, left to right
                Functions_Movement.Teleport(
                    objRef.compMove,

                    Functions_Level.currentRoom.rec.X //start with the room's x pos
                    + Functions_Level.currentRoom.rec.Width / 2 //get the center X
                    - 16 * 5 //move 5 tiles left
                    + 16 * i, //from there, place objs in a horizontal line
                    
                    Functions_Level.currentRoom.rec.Y + 16 * 20); //place in front of actors
                objRef.direction = Direction.Down;
                Functions_GameObject.SetType(objRef, ChallengeSet.roomObjs[i]);
            }

            //loop over ChallengeSet.actors, creating them near back wall
            for (i = 0; i < ChallengeSet.actors.Count; i++)
            {
                actorRef = Functions_Pool.GetActor();
                //place middle top
                Functions_Movement.Teleport(
                    actorRef.compMove,

                    Functions_Level.currentRoom.rec.X //start with the room's x pos
                    + Functions_Level.currentRoom.rec.Width / 2 //get the center X
                    - 16 * 5 //move 5 tiles left
                    + 16 * i, //from there, place actors in a horizontal line

                    Functions_Level.currentRoom.rec.Y + 16 * 18); //place behind enemies
                actorRef.direction = Direction.Down;
                Functions_Actor.SetType(actorRef, ChallengeSet.actors[i]);
            }

            //close off the pit exit, so challenge set enemies can't escape
            //creating an unwinnable and unfair gamestate
            for (i = 0; i < 10; i++)
            {
                objRef = Functions_Pool.GetRoomObj();
                //block off the colliseum entrance
                Functions_Movement.Teleport(
                    objRef.compMove,

                    Functions_Level.currentRoom.rec.X //start with the room's x pos
                    + Functions_Level.currentRoom.rec.Width / 2 //get the center X
                    - 16 * 5 //move 5 tiles left
                    + 16 * i //from there, place objs in a horizontal line
                    + 8, //with a small alignment offset

                    Functions_Level.currentRoom.spawnPos.Y + 16 * 2 - 8);
                objRef.direction = Direction.Down;
                Functions_GameObject.SetType(objRef, ObjType.Wor_Colliseum_Pillar_Top);
            }

            //respawn hero inside of pit
            Functions_Hero.SpawnInCurrentRoom();

            //create the judge obj
            objRef = Functions_Pool.GetRoomObj();

            //place along backwall
            Functions_Movement.Teleport(
                objRef.compMove,
                Functions_Level.currentRoom.rec.X //start with the room's x pos
                + Functions_Level.currentRoom.rec.Width / 2 //get the center X
                + 8, //align to gameworld grid
                Functions_Level.currentRoom.rec.Y + 16 * 14 - 8 - 4); //place against back wall
            objRef.direction = Direction.Down;
            Functions_GameObject.SetType(objRef, ObjType.Judge_Colliseum);
        }

        


    }
}