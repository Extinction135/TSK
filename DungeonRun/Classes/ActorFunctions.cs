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
            Actor.compCollision.active = true;
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
            Actor.compCollision.offsetX = -6;
            Actor.compCollision.offsetY = 0;
        }

        public static void Teleport(Actor Actor, float X, float Y)
        {
            Actor.compMove.position.X = X;
            Actor.compMove.position.Y = Y;
        }




        public static Vector2 ProjectileOffset = new Vector2(0, 0);
        public static void AlignProjectile(GameObject Projectile, Actor Actor)
        {
            ProjectileOffset.X = 0; ProjectileOffset.Y = 0;

            //convert projectile's diagonal direction to a cardinal direction
            if (Projectile.direction == Direction.UpRight) { Projectile.direction = Direction.Right; }
            else if (Projectile.direction == Direction.DownRight) { Projectile.direction = Direction.Right; }
            else if (Projectile.direction == Direction.UpLeft) { Projectile.direction = Direction.Left; }
            else if (Projectile.direction == Direction.DownLeft) { Projectile.direction = Direction.Left; }

            //aligns the projectile to the actor
            if (Actor.direction == Direction.Down) { ProjectileOffset.X = 0; ProjectileOffset.Y = 8; }
            else if (Actor.direction == Direction.Up) { ProjectileOffset.X = 0; ProjectileOffset.Y = -8; }
            else if (Actor.direction == Direction.Right) { ProjectileOffset.X = 8; ProjectileOffset.Y = 0; }
            else if (Actor.direction == Direction.Left) { ProjectileOffset.X = -8; ProjectileOffset.Y = 0; }

            //different types of projectiles have different offsets

            //teleport the projectile to the actor's position with the offset applied
            GameObjectFunctions.Teleport(Projectile,
                Actor.compSprite.position.X + ProjectileOffset.X,
                Actor.compSprite.position.Y + ProjectileOffset.Y);
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
                    GameObject projectile = PoolFunctions.GetProjectile(Actor.screen.pool);
                    projectile.direction = Actor.direction;
                    GameObjectFunctions.SetType(projectile, GameObject.Type.ProjectileSword);
                    AlignProjectile(projectile, Actor);
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

            //set actor animation and direction
            ActorAnimationListManager.SetAnimationGroup(Actor);
            ActorAnimationListManager.SetAnimationDirection(Actor);
        }

        public static void Draw(Actor Actor, ScreenManager ScreenManager)
        {
            DrawFunctions.Draw(Actor.compSprite, ScreenManager);
            if (ScreenManager.game.drawCollisionRecs)
            { DrawFunctions.Draw(Actor.compCollision, ScreenManager); }
        }
    }
}