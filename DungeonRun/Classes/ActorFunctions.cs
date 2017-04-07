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
    public static class ActorFunctions
    {
        //all the functions that operate on an actor class
        //input


        public static void SetType(Actor Actor, Actor.Type Type)
        {   //set the type, direction, state, and active boolean
            Actor.type = Type;
            Actor.compMove.direction = Direction.None;
            Actor.direction = Direction.Down;
            Actor.state = Actor.State.Idle;
            Actor.active = true;
            SetCollisionRec(Actor);
            //set actor animations lists, group, direction
            ActorAnimationListManager.SetAnimationList(Actor);
            ActorAnimationListManager.SetAnimationGroup(Actor);
            ActorAnimationListManager.SetAnimationDirection(Actor);

            //set actor soundFX

            //we may need to bring the actor back to life (based on their type)
        }

        public static void SetCollisionRec(Actor Actor)
        {
            //set the collisionRec parameters based on the Type
            Actor.compCollision.rec.Width = 12;
            Actor.compCollision.rec.Height = 8;
            Actor.compCollision.offsetX = 0;
            Actor.compCollision.offsetY = 4;
        }

        public static void Teleport(Actor Actor, float X, float Y)
        {
            Actor.compMove.position.X = X;
            Actor.compMove.position.Y = Y;
        }

        public static void Update(Actor Actor)
        {
            //get the input for this frame, set actor.direction
            Actor.compInput.SetInputState(); 
            //if actor can change state, sync state to inputState
            if (!Actor.stateLocked) { Actor.state = Actor.inputState; } 

            //update + play animations
            ActorAnimationListManager.SetAnimationGroup(Actor);
            ActorAnimationListManager.SetAnimationDirection(Actor);
            AnimationFunctions.Animate(Actor.compAnim, Actor.compSprite);
        }

        public static void Draw(Actor Actor, ScreenManager ScreenManager)
        {
            DrawFunctions.Draw(Actor.compSprite, ScreenManager);
            DrawFunctions.Draw(Actor.compCollision, ScreenManager);
        }
    }
}