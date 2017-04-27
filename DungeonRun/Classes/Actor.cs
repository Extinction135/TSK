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

        public ActorType type; //the type of actor this is
        public ActorState state; //what actor is doing this frame
        public ActorState inputState; //what input wants actor to do this frame

        public Boolean stateLocked; //can actor change state? else actor must wait for state to unlock
        public byte lockTotal = 0; //how many frames the actor statelocks for, based on state
        public byte lockCounter = 0; //counts from 0 to lockTotal, then flips stateLocked false

        public ActorAnimationList animList;
        public AnimationGroup animGroup;
        public Direction direction; //direction actor is facing
        public Boolean active; //does actor input/update/draw?

        //type specific fields, changed by ActorFunctions.SetType()
        public float dashSpeed = 0.75f; 
        public float walkSpeed = 0.25f;

        //the components that actor requires to function
        public ComponentSprite compSprite;
        public ComponentAnimation compAnim;
        public ComponentInput compInput;
        public ComponentMovement compMove;
        public ComponentCollision compCollision;

        //battle fields
        public byte health;
        public Weapon weapon;
        public Item item;

        //actor requires a reference to the various textures/sounds it may use - all the possible textures
        public Actor()
        {
            //create the actor components
            compSprite = new ComponentSprite(Assets.heroSheet, new Vector2(0, 0), new Byte4(0, 0, 0, 0), new Byte2(16, 16));
            compAnim = new ComponentAnimation();
            compInput = new ComponentInput();
            compMove = new ComponentMovement();
            compCollision = new ComponentCollision();
            //set the actor type to hero, teleport to position
            ActorFunctions.SetType(this, ActorType.Hero);
            MovementFunctions.Teleport(this.compMove, compSprite.position.X, compSprite.position.Y);
        }
    }
}