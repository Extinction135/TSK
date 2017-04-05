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




        //input functions

        //collision functions

        //movement functions

        //draw functions



        //set the actor's type




        //to move an actor/obj: 
        //1. projectMovement via move component
        //2. update the collisionRec and check collisions (move or block)
        //3. place the sprite at the collision recs position

        //this requires collision, movement, and sprite components



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
            //we may need to bring the actor back to life (based on their type)
        }



        public static void Teleport(Actor Actor, float X, float Y)
        {
            Actor.compMove.position.X = X;
            Actor.compMove.position.Y = Y;
        }




        //match the actor's collrec to it's sprite, centering it and applying the collrec offset



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

            //project movement via move component
            Actor.compMove.ProjectMovement();

            //update the collision component's rectangle

            //check for collisions between collRec + gameObjPool

            //match collisionRec to projected position
            Actor.compCollision.rec.X = (int)Actor.compMove.newPosition.X;
            Actor.compCollision.rec.Y = (int)Actor.compMove.newPosition.Y;
            //update to new position (no collision checking for now)
            Actor.compMove.position = Actor.compMove.newPosition;

            //update sprite position
            Actor.compSprite.position = Actor.compMove.position;
            Actor.compSprite.SetZdepth();
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