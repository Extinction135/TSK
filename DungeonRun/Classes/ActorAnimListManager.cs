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

        public AnimationGroup attack;
        public AnimationGroup use;
        public AnimationGroup hit;
        public AnimationGroup dead;

        //interact
        //pickup, hold, carry, drag, etc...
    }

    public static class ActorAnimationListManager
    {
        //the animationList used by all actors
        public static ActorAnimationList actorAnims;

        public static void SetAnimationGroup(Actor ActorRef)
        {
            if (ActorRef.state == Actor.State.Idle) { ActorRef.animGroup = ActorRef.animList.idle; }
            else if (ActorRef.state == Actor.State.Move) { ActorRef.animGroup = ActorRef.animList.move; }
            else if (ActorRef.state == Actor.State.Dash) { ActorRef.animGroup = ActorRef.animList.dash; }
            //
            else if (ActorRef.state == Actor.State.Attack) { ActorRef.animGroup = ActorRef.animList.attack; }
            else if (ActorRef.state == Actor.State.Use) { ActorRef.animGroup = ActorRef.animList.use; }
            else if (ActorRef.state == Actor.State.Hit) { ActorRef.animGroup = ActorRef.animList.hit; }
            else if (ActorRef.state == Actor.State.Dead) { ActorRef.animGroup = ActorRef.animList.dead; }
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

        static ActorAnimationListManager()
        {
            //create/populate Actor's AnimationList
            actorAnims = new ActorAnimationList();

            actorAnims.idle = new AnimationGroup();
            actorAnims.idle.down = new List<Byte4>   { new Byte4(0, 0, 0, 0) };
            actorAnims.idle.up = new List<Byte4>     { new Byte4(0, 1, 0, 0) };
            actorAnims.idle.right = new List<Byte4>  { new Byte4(0, 2, 0, 0) };
            actorAnims.idle.left = new List<Byte4>   { new Byte4(0, 2, 1, 0) };

            actorAnims.move = new AnimationGroup();
            actorAnims.move.down = new List<Byte4>   { new Byte4(1, 0, 0, 0), new Byte4(1, 0, 1, 0) };
            actorAnims.move.up = new List<Byte4>     { new Byte4(1, 1, 0, 0), new Byte4(1, 1, 1, 0) };
            actorAnims.move.right = new List<Byte4>  { new Byte4(0, 2, 0, 0), new Byte4(1, 2, 0, 0) };
            actorAnims.move.left = new List<Byte4>   { new Byte4(0, 2, 1, 0), new Byte4(1, 2, 1, 0) };

            actorAnims.dash = new AnimationGroup();
            actorAnims.dash.down = new List<Byte4>   { new Byte4(2, 0, 0, 0) };
            actorAnims.dash.up = new List<Byte4>     { new Byte4(2, 1, 0, 0) };
            actorAnims.dash.right = new List<Byte4>  { new Byte4(2, 2, 0, 0) };
            actorAnims.dash.left = new List<Byte4>   { new Byte4(2, 2, 1, 0) };

            actorAnims.attack = new AnimationGroup();
            actorAnims.attack.down = new List<Byte4>     { new Byte4(3, 0, 0, 0) };
            actorAnims.attack.up = new List<Byte4>       { new Byte4(3, 1, 0, 0) };
            actorAnims.attack.right = new List<Byte4>    { new Byte4(3, 2, 0, 0) };
            actorAnims.attack.left = new List<Byte4>     { new Byte4(3, 2, 1, 0) };

            actorAnims.use = new AnimationGroup();
            actorAnims.use.down = new List<Byte4>    { new Byte4(4, 1, 0, 0) };
            actorAnims.use.up = actorAnims.use.down;
            actorAnims.use.right = actorAnims.use.down;
            actorAnims.use.left = actorAnims.use.down;

            actorAnims.hit = new AnimationGroup();
            actorAnims.hit.down = new List<Byte4> { new Byte4(4, 0, 0, 0) };
            actorAnims.hit.up = actorAnims.hit.down;
            actorAnims.hit.right = actorAnims.hit.down;
            actorAnims.hit.left = actorAnims.hit.down;

            actorAnims.dead = new AnimationGroup();
            actorAnims.dead.down = new List<Byte4> { new Byte4(4, 2, 0, 0) };
            actorAnims.dead.up = actorAnims.dead.down;
            actorAnims.dead.right = actorAnims.dead.down;
            actorAnims.dead.left = actorAnims.dead.down;
        }
    }
}