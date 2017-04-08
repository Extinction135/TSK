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



        public static void SetInputState(ComponentInput Input, Actor Actor)
        {
            Actor.inputState = Actor.State.Idle; //reset inputState
            Actor.compMove.direction = Input.direction; //set move direction
            if (Input.direction != Direction.None)
            {
                Actor.inputState = Actor.State.Move; //actor must be moving
                Actor.direction = Input.direction; //set actor moving/facing direction
            }
            //determine + set button inputs
            if (Input.attack) { Actor.inputState = Actor.State.Attack; }
            else if (Input.use) { Actor.inputState = Actor.State.Use; }
            else if (Input.dash) { Actor.inputState = Actor.State.Dash; }
            else if (Input.interact) { Actor.inputState = Actor.State.Interact; }
        }

        public static void Update(Actor Actor)
        {
            //get the input for this frame, set actor.direction
            ActorFunctions.SetInputState(Actor.compInput, Actor);

            //if actor can change state, sync state to inputState
            if (!Actor.stateLocked)
            {
                Actor.state = Actor.inputState; //pass the input state
                Actor.lockCounter = 0; //reset lock counter in case actor statelocks
                Actor.lockTotal = 0; //reset lock total
                Actor.compMove.speed = Actor.walkSpeed; //default to walk speed

                //based on state, lock and begin count
                if (Actor.state == Actor.State.Dash)
                {
                    Actor.lockTotal = 10;
                    Actor.stateLocked = true;
                    Actor.compMove.speed = Actor.dashSpeed;
                    //create a dash effect here
                }
                else if (Actor.state == Actor.State.Attack)
                {
                    Actor.lockTotal = 15;
                    Actor.stateLocked = true;
                    Actor.compMove.speed = 0.0f;

                    //create weapon projectile here
                    GameObject projectile = Actor.screen.projectilePool.GetObj();
                    GameObjectFunctions.SetType(projectile, GameObject.Type.ProjectileSword);
                    GameObjectFunctions.Teleport(projectile, Actor.compCollision.rec.X, Actor.compCollision.rec.Y);
                }
                else if (Actor.state == Actor.State.Use)
                {
                    Actor.lockTotal = 25;
                    Actor.stateLocked = true;
                    Actor.compMove.speed = 0.0f;
                    //create item projectile here
                }
            }
            else
            {   //actor is statelocked
                Actor.lockCounter++; //increment the lock counter
                if (Actor.lockCounter > Actor.lockTotal) //check against lock total
                {
                    Actor.stateLocked = false; //unlock actor
                    InputFunctions.ResetInputData(Actor.compInput); //reset input component
                } 
            }

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