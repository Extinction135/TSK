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

        public static void SetHitState(Actor Actor)
        {
            Actor.state = Actor.State.Hit;
            Actor.stateLocked = true;
            Actor.lockCounter = 0;
            Actor.lockTotal = 15;
            
            //create a dash(smoke) particle here
            //ProjectileFunctions.Spawn(GameObject.Type.ParticleDashPuff, Actor);
        }



        public static void SetType(Actor Actor, Actor.Type Type)
        {   //set the type, direction, state, and active boolean
            Actor.type = Type;
            Actor.compMove.direction = Direction.None;
            Actor.direction = Direction.Down;
            Actor.state = Actor.State.Idle;
            Actor.active = true;
            Actor.compCollision.active = true;
            Actor.compCollision.blocking = true; //actors always block
            SetCollisionRec(Actor);
            //set actor animations group, direction
            ActorAnimationListManager.SetAnimationGroup(Actor);
            ActorAnimationListManager.SetAnimationDirection(Actor);

            //set type specific fields
            if (Type == Actor.Type.Hero)
            {
                Actor.compSprite.texture = Assets.heroSheet;
                Actor.health = 3;
                Actor.walkSpeed = 0.30f;
                Actor.dashSpeed = 0.80f;
                //set actor soundFX
            }
            else if (Type == Actor.Type.Blob)
            {
                Actor.compSprite.texture = Assets.blobSheet;
                Actor.health = 1;
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


            #region Actor is not Statelocked

            if (!Actor.stateLocked)
            {   
                //set actor moving/facing direction
                if (Actor.compInput.direction != Direction.None)
                { Actor.direction = Actor.compInput.direction; }

                Actor.state = Actor.inputState; //sync state to input state
                Actor.lockCounter = 0; //reset lock counter in case actor statelocks
                Actor.lockTotal = 0; //reset lock total
                Actor.compMove.speed = Actor.walkSpeed; //default to walk speed

                //check states
                if (Actor.state == Actor.State.Dash)
                {
                    Actor.lockTotal = 10;
                    Actor.stateLocked = true;
                    Actor.compMove.speed = Actor.dashSpeed;

                    //create a dash(smoke) particle here
                    ProjectileFunctions.Spawn(GameObject.Type.ParticleDashPuff, Actor);
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

            #endregion


            #region Actor is Statelocked

            else
            {
                Actor.lockCounter++; //increment lock counter
                if (Actor.lockCounter > Actor.lockTotal) //check against lock total
                {
                    Actor.stateLocked = false; //unlock actor
                    Input.ResetInputData(Actor.compInput); //reset input component
                    if (Actor.health <= 0) //check to see if the actor is dead
                    { Actor.state = Actor.State.Dead; }
                }
                if (Actor.state == Actor.State.Dead)
                {   //manage the death state
                    Actor.lockCounter = 0;
                    Actor.lockTotal = 255;
                    Actor.stateLocked = true;
                    Actor.compCollision.rec.X = -1000; 
                }
            }

            #endregion


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