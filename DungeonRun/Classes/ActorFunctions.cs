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

        public static void SetType(Actor Actor, Actor.Type Type)
        {   //set the type, direction, state, and active boolean
            Actor.type = Type;
            Actor.compMove.direction = Direction.None;
            Actor.direction = Direction.Down;
            Actor.state = Actor.State.Idle;
            Actor.active = true;
            Actor.compCollision.active = true;
            SetCollisionRec(Actor);
            //set actor animations lists, group, direction
            ActorAnimationListManager.SetAnimationList(Actor);
            ActorAnimationListManager.SetAnimationGroup(Actor);
            ActorAnimationListManager.SetAnimationDirection(Actor);

            //set type specific fields
            if (Type == Actor.Type.Hero)
            {
                Actor.walkSpeed = 0.30f;
                Actor.dashSpeed = 0.80f;
                //set actor soundFX
            }
            else if (Type == Actor.Type.Blob)
            {
                Actor.walkSpeed = 0.05f;
                Actor.dashSpeed = 0.30f;
                //set actor soundFX
            }

        }

        public static void SetCollisionRec(Actor Actor)
        {
            //set the collisionRec parameters based on the Type
            Actor.compCollision.rec.Width = 12;
            Actor.compCollision.rec.Height = 8;
            Actor.compCollision.offsetX = -6;
            Actor.compCollision.offsetY = 0;
        }

        public static void Update(Actor Actor)
        {
            //get the input for this frame, set actor.direction
            Input.SetInputState(Actor.compInput, Actor);

            //if actor can change state, sync state to inputState
            if (!Actor.stateLocked)
            {
                //set actor moving/facing direction
                if (Actor.compInput.direction != Direction.None)
                { Actor.direction = Actor.compInput.direction; }

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
                    ComponentFunctions.StopMovement(Actor.compMove);

                    //create weapon projectile here
                    ProjectileFunctions.Spawn(GameObject.Type.ProjectileSword, Actor);
                }
                else if (Actor.state == Actor.State.Use)
                {
                    Actor.lockTotal = 25;
                    Actor.stateLocked = true;
                    ComponentFunctions.StopMovement(Actor.compMove);
                    //create item projectile here
                }
            }
            else
            {   //actor is statelocked
                Actor.lockCounter++; //increment the lock counter
                if (Actor.lockCounter > Actor.lockTotal) //check against lock total
                {
                    Actor.stateLocked = false; //unlock actor
                    Input.ResetInputData(Actor.compInput); //reset input component
                } 
            }

            //set actor animation and direction
            ActorAnimationListManager.SetAnimationGroup(Actor);
            ActorAnimationListManager.SetAnimationDirection(Actor);
        }

        public static void Draw(Actor Actor)
        {
            DrawFunctions.Draw(Actor.compSprite);
            if (Flags.DrawCollisions) { DrawFunctions.Draw(Actor.compCollision); }
        }
    }
}