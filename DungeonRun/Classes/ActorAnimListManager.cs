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
    public static class ActorAnimationListManager
    {
        //the animationList used by all actors
        public static ActorAnimationList actorAnims;

        public static void SetAnimationGroup(Actor Actor)
        {
            if (Actor.state == ActorState.Idle) { Actor.animGroup = Actor.animList.idle; }
            else if (Actor.state == ActorState.Move) { Actor.animGroup = Actor.animList.move; }
            else if (Actor.state == ActorState.Dash) { Actor.animGroup = Actor.animList.dash; }
            else if (Actor.state == ActorState.Interact) { Actor.animGroup = Actor.animList.interact; }
            //
            else if (Actor.state == ActorState.Attack) { Actor.animGroup = Actor.animList.attack; }
            else if (Actor.state == ActorState.Use) { Actor.animGroup = Actor.animList.use; }
            else if (Actor.state == ActorState.Hit) { Actor.animGroup = Actor.animList.hit; }
            else if (Actor.state == ActorState.Dead) { Actor.animGroup = Actor.animList.dead; }
            //
            else if (Actor.state == ActorState.Reward) { Actor.animGroup = Actor.animList.reward; }
        }

        public static void SetAnimationDirection(Actor Actor)
        {
            //set cardinal directions
            if (Actor.direction == Direction.Down) { Actor.compAnim.currentAnimation = Actor.animGroup.down; }
            else if (Actor.direction == Direction.Up) { Actor.compAnim.currentAnimation = Actor.animGroup.up; }
            else if (Actor.direction == Direction.Right) { Actor.compAnim.currentAnimation = Actor.animGroup.right; }
            else if (Actor.direction == Direction.Left) { Actor.compAnim.currentAnimation = Actor.animGroup.left; }
            //set diagonal directions
            else if (Actor.direction == Direction.DownRight) { Actor.compAnim.currentAnimation = Actor.animGroup.right; }
            else if (Actor.direction == Direction.DownLeft) { Actor.compAnim.currentAnimation = Actor.animGroup.left; }
            else if (Actor.direction == Direction.UpRight) { Actor.compAnim.currentAnimation = Actor.animGroup.right; }
            else if (Actor.direction == Direction.UpLeft) { Actor.compAnim.currentAnimation = Actor.animGroup.left; }
        }

        static ActorAnimationListManager()
        {
            //create/populate Actor's AnimationList
            actorAnims = new ActorAnimationList();

            //idle, move, dash, & interact lists
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

            actorAnims.interact = new AnimationGroup();
            actorAnims.interact.down = new List<Byte4>  { new Byte4(4, 0, 0, 0) };
            actorAnims.interact.up = new List<Byte4>    { new Byte4(4, 1, 0, 0) };
            actorAnims.interact.right = new List<Byte4> { new Byte4(4, 2, 0, 0) };
            actorAnims.interact.left = new List<Byte4>  { new Byte4(4, 2, 1, 0) };

            //attack, use, hit, & dead lists
            actorAnims.attack = new AnimationGroup();
            actorAnims.attack.down = new List<Byte4>     { new Byte4(3, 0, 0, 0) };
            actorAnims.attack.up = new List<Byte4>       { new Byte4(3, 1, 0, 0) };
            actorAnims.attack.right = new List<Byte4>    { new Byte4(3, 2, 0, 0) };
            actorAnims.attack.left = new List<Byte4>     { new Byte4(3, 2, 1, 0) };

            //use anim just points to attack anim
            actorAnims.use = actorAnims.attack;

            actorAnims.hit = new AnimationGroup();
            actorAnims.hit.down = new List<Byte4>   { new Byte4(0, 3, 0, 0) };
            actorAnims.hit.up = actorAnims.hit.down;
            actorAnims.hit.right = actorAnims.hit.down;
            actorAnims.hit.left = actorAnims.hit.down;

            actorAnims.dead = new AnimationGroup();
            actorAnims.dead.down = new List<Byte4>  { new Byte4(1, 3, 0, 0) };
            actorAnims.dead.up = actorAnims.dead.down;
            actorAnims.dead.right = actorAnims.dead.down;
            actorAnims.dead.left = actorAnims.dead.down;

            //reward list
            actorAnims.reward = new AnimationGroup();
            actorAnims.reward.down = new List<Byte4> { new Byte4(3, 3, 0, 0) };
            actorAnims.reward.up = actorAnims.reward.down;
            actorAnims.reward.right = actorAnims.reward.down;
            actorAnims.reward.left = actorAnims.reward.down;

        }

    }
}