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
    public class Actor
    {

        public enum Type
        {
            Hero,
            Blob,
        }

        public enum State
        {
            Idle,
            Move,
            
            Attack, //attacks with weapon
            Use, //uses item
            Dash, //moves quickly
            Interact, //picks up, throws, opens a gameobject

            Hit,
            Dead,
        }











        public DungeonScreen screen;

        public Type type; //the type of actor this is

        public State state; //what actor is doing this frame
        public State inputState; //what input wants actor to do this frame
        public Boolean stateLocked; //can actor change state? else actor must wait for state to unlock

        public SpriteComponent spriteComp;
        public AnimationComponent animComp;
        public ActorAnimationList animList; 
        public AnimationGroup animGroup;

        public Direction direction; //direction actor is facing
        public InputComponent inputComp;
        public MovementComponent moveComp;
        //collisionComp
        //

        












        //actor requires a reference to the various textures/sounds it may use - all the possible textures
        public Actor(DungeonScreen DungeonScreen)
        {
            screen = DungeonScreen;

            //create the actor components
            spriteComp = new SpriteComponent(screen.screenManager.spriteBatch, 
                screen.assets.actorsSheet, new Vector2(0, 0), new Byte4(0, 0, 0, 0), new Byte2(16, 16));
            animComp = new AnimationComponent(spriteComp);
            inputComp = new InputComponent(this);
            moveComp = new MovementComponent();

            //initialize the actor to type hero
            type = Type.Hero; //init to hero
            SetType(type, spriteComp.position.X, spriteComp.position.Y);
        }

        public void Update()
        {
            inputComp.SetInputState(); //get the input for this frame, set actor.direction
            if (!stateLocked) { state = inputState; } //if actor can change state, sync state to inputState

            //update animations
            AnimationListManager.SetAnimationGroup(this);
            AnimationListManager.SetAnimationDirection(this);




            animComp.Animate(); //play the actor's animation

            //project movement
            moveComp.ProjectMovement();
            moveComp.position = moveComp.newPosition; //update to new position (no collision checking for now)

            //update sprite position
            spriteComp.position = moveComp.position;
            spriteComp.SetZdepth();
        }

        public void Draw()
        {
            spriteComp.Draw();
            //draw actor weapon
        }



        

        //this function needs to consider the current state of the actor
        //we may need to bring an actor back to life, for example
        public void SetType(Actor.Type Type, float X, float Y)
        {
            type = Type;
            moveComp.position.X = X;
            moveComp.position.Y = Y;
            moveComp.direction = Direction.None;
            direction = Direction.Down;

            //set actor animations lists, group, direction
            AnimationListManager.SetAnimationList(this);
            AnimationListManager.SetAnimationGroup(this);
            AnimationListManager.SetAnimationDirection(this);

            //set actor soundFX
        }

        

        






       
    }
}