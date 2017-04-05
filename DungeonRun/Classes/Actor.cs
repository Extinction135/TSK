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

        public ComponentSprite compSprite;
        public ComponentAnimation compAnim;
        public ComponentInput compInput;
        public ComponentMovement compMove;
        public ComponentCollision compCollision;

        public ActorAnimationList animList; 
        public AnimationGroup animGroup;
        public Direction direction; //direction actor is facing
        public Boolean active; //does actor input/update/draw?

        //actor requires a reference to the various textures/sounds it may use - all the possible textures
        public Actor(DungeonScreen DungeonScreen)
        {
            screen = DungeonScreen;

            //create the actor components
            compSprite = new ComponentSprite(screen.screenManager.spriteBatch, 
                screen.assets.actorsSheet, new Vector2(0, 0), new Byte4(0, 0, 0, 0), new Byte2(16, 16));
            compAnim = new ComponentAnimation(compSprite);
            compInput = new ComponentInput(this);
            compMove = new ComponentMovement();
            compCollision = new ComponentCollision(10, 10, 16, 16, true);

            //initialize the actor to type hero
            type = Type.Hero; //init to hero
            active = true;
            ActorFunctions.SetType(this, Type.Hero);
            ActorFunctions.Teleport(this, compSprite.position.X, compSprite.position.Y);
        }
    }
}