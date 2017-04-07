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
    public struct AnimationGroup
    {   //represents an animation with Down, Up, Left, Right states
        public List<Byte4> down;
        public List<Byte4> up;
        public List<Byte4> right;
        public List<Byte4> left;
    }


    public struct ActorAnimationList
    {
        public AnimationGroup idle;
        public AnimationGroup move;
        public AnimationGroup dash;

        //attack
        //use
        //interact

        //hit
        //death
        //etc...
    }












    public static class ActorAnimationListManager
    {
        //sets an actor's animationList based on the actor.type
        public static void SetAnimationList(Actor ActorRef)
        {
            if (ActorRef.type == Actor.Type.Hero) { ActorRef.animList = heroAnims; }
            else if (ActorRef.type == Actor.Type.Blob) { ActorRef.animList = blobAnims; }
        }

        public static void SetAnimationGroup(Actor ActorRef)
        {
            if (ActorRef.state == Actor.State.Idle) { ActorRef.animGroup = ActorRef.animList.idle; }
            else if (ActorRef.state == Actor.State.Move) { ActorRef.animGroup = ActorRef.animList.move; }
            else if (ActorRef.state == Actor.State.Dash) { ActorRef.animGroup = ActorRef.animList.dash; }
        }

        public static void SetAnimationDirection(Actor ActorRef)
        {
            //set cardinal directions
            if (ActorRef.direction == Direction.Down) { ActorRef.compAnim.currentAnimation = ActorRef.animGroup.down; }
            else if (ActorRef.direction == Direction.Up) { ActorRef.compAnim.currentAnimation = ActorRef.animGroup.up; }
            else if (ActorRef.direction == Direction.Right) { ActorRef.compAnim.currentAnimation = ActorRef.animGroup.right; }
            else if (ActorRef.direction == Direction.Left) { ActorRef.compAnim.currentAnimation = ActorRef.animGroup.left; }
            //set diagonal directions
            else if (ActorRef.direction == Direction.DownRight) { ActorRef.compAnim.currentAnimation = ActorRef.animGroup.right; }
            else if (ActorRef.direction == Direction.DownLeft) { ActorRef.compAnim.currentAnimation = ActorRef.animGroup.left; }
            else if (ActorRef.direction == Direction.UpRight) { ActorRef.compAnim.currentAnimation = ActorRef.animGroup.right; }
            else if (ActorRef.direction == Direction.UpLeft) { ActorRef.compAnim.currentAnimation = ActorRef.animGroup.left; }
        }


















        //the animationLists for all actors
        static ActorAnimationList heroAnims;
        static ActorAnimationList blobAnims;


        //populates the animation lists
        static ActorAnimationListManager()
        {

            #region Create Hero's Animation List

            heroAnims = new ActorAnimationList();

            heroAnims.idle = new AnimationGroup();
            heroAnims.idle.down = new List<Byte4>   { new Byte4(0, 0, 0, 0) };
            heroAnims.idle.up = new List<Byte4>     { new Byte4(0, 1, 0, 0) };
            heroAnims.idle.right = new List<Byte4>  { new Byte4(0, 2, 0, 0) };
            heroAnims.idle.left = new List<Byte4>   { new Byte4(0, 2, 1, 0) };

            heroAnims.move = new AnimationGroup();
            heroAnims.move.down = new List<Byte4>   { new Byte4(1, 0, 0, 0), new Byte4(1, 0, 1, 0) };
            heroAnims.move.up = new List<Byte4>     { new Byte4(1, 1, 0, 0), new Byte4(1, 1, 1, 0) };
            heroAnims.move.right = new List<Byte4>  { new Byte4(0, 2, 0, 0), new Byte4(1, 2, 0, 0) };
            heroAnims.move.left = new List<Byte4>   { new Byte4(0, 2, 1, 0), new Byte4(1, 2, 1, 0) };

            heroAnims.dash = new AnimationGroup();
            heroAnims.dash.down = new List<Byte4>   { new Byte4(2, 0, 0, 0) };
            heroAnims.dash.up = new List<Byte4>     { new Byte4(2, 1, 0, 0) };
            heroAnims.dash.right = new List<Byte4>  { new Byte4(2, 2, 0, 0) };
            heroAnims.dash.left = new List<Byte4>   { new Byte4(2, 2, 1, 0) };

            #endregion


            #region Create Blobs's Animation List

            blobAnims = new ActorAnimationList();

            blobAnims.idle = new AnimationGroup();
            blobAnims.idle.down = new List<Byte4>   { new Byte4(0, 3, 0, 0), new Byte4(1, 3, 0, 0) };
            blobAnims.idle.up = new List<Byte4>     { new Byte4(0, 4, 0, 0), new Byte4(1, 4, 0, 0) };
            blobAnims.idle.right = new List<Byte4>  { new Byte4(0, 5, 0, 0), new Byte4(1, 5, 0, 0) };
            blobAnims.idle.left = new List<Byte4>   { new Byte4(0, 5, 1, 0), new Byte4(1, 5, 1, 0) };

            blobAnims.move = new AnimationGroup();
            blobAnims.move.down = new List<Byte4>   { new Byte4(0, 3, 0, 0), new Byte4(1, 3, 0, 0), new Byte4(2, 3, 0, 0) };
            blobAnims.move.up = new List<Byte4>     { new Byte4(0, 4, 0, 0), new Byte4(1, 4, 0, 0), new Byte4(2, 4, 0, 0) };
            blobAnims.move.right = new List<Byte4>  { new Byte4(0, 5, 0, 0), new Byte4(1, 5, 0, 0), new Byte4(2, 5, 0, 0) };
            blobAnims.move.left = new List<Byte4>   { new Byte4(0, 5, 1, 0), new Byte4(1, 5, 1, 0), new Byte4(2, 5, 1, 0) };

            blobAnims.dash = new AnimationGroup();
            blobAnims.dash.down = new List<Byte4>   { new Byte4(2, 3, 0, 0) };
            blobAnims.dash.up = new List<Byte4>     { new Byte4(2, 4, 0, 0) };
            blobAnims.dash.right = new List<Byte4>  { new Byte4(2, 5, 0, 0) };
            blobAnims.dash.left = new List<Byte4>   { new Byte4(2, 5, 1, 0) };

            #endregion


        }
    }
}