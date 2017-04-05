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
            //set actor animations lists, group, direction
            ActorAnimationListManager.SetAnimationList(Actor);
            ActorAnimationListManager.SetAnimationGroup(Actor);
            ActorAnimationListManager.SetAnimationDirection(Actor);
            
            //set actor soundFX

            //set the collisionRec parameters based on the Type
            Actor.compCollision.rec.Width = 16;
            Actor.compCollision.rec.Height = 16;
            Actor.compCollision.offsetX = 0;
            Actor.compCollision.offsetY = 0;

            //we may need to bring the actor back to life (based on their type)
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

            //update animations
            ActorAnimationListManager.SetAnimationGroup(Actor);
            ActorAnimationListManager.SetAnimationDirection(Actor);

            //play the actor's animation
            Actor.compAnim.Animate();

            //move actor based on direction & check collisions
            CollisionFunctions.Move(Actor.compMove, Actor.compCollision, Actor.compSprite);
        }

        public static void Draw(Actor Actor, ScreenManager ScreenManager)
        {
            Actor.compSprite.Draw();
            //draw actor weapon
            //draw collision rec
            ScreenManager.spriteBatch.Draw(
                ScreenManager.game.assets.dummyTexture,
                Actor.compCollision.rec,
                ScreenManager.game.colorScheme.collisionActor);
        }
    }
}