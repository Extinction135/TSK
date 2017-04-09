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
    public class ComponentCollision
    {   //allows an object or actor to collide with other objects or actors
        public Rectangle rec = new Rectangle(100, 100, 16, 16); //used in collision checking
        public int offsetX = 0; //offsets rec from sprite.position
        public int offsetY = 0; //offsets rec from sprite.position
        public Boolean blocking = true; //is this actor/obj impassable
        public Boolean active = false; //check to see if this component collides with other components?
    }

    public class ComponentMovement
    {   //allows an object or actor to move, with speed and friction
        public Vector2 position = new Vector2(100, 100); //current position of actor/object
        public Vector2 newPosition = new Vector2(100, 100); //projected position
        public Direction direction = Direction.Down; //the direction actor/obj is moving
        public Vector2 magnitude = new Vector2(0, 0); //how much actor/obj moves each frames
        public float speed = 0.0f; //controls magnitude
        public float friction = 0.75f; //reduces magnitude each frame
        public void StopMovement() { magnitude.X = 0; magnitude.Y = 0; speed = 0; }
    }

    public class ComponentAnimation
    {   //allows an object or actor to animate through a sequence of frames on a timer
        public List<Byte4> currentAnimation; //a list of byte4 representing frames of an animation
        public byte index = 0; //where in the currentAnimation list the animation is (animation index)
        public byte speed = 10; //how many frames should elapse before animation is updated (limits animation speed)
        public byte timer = 0; //how many frames have elapsed since last animation update (counts frames) @ 60fps
        public Boolean loop = true; //should currentAnimation loop?
    }

    public class ComponentInput
    {   //changes the actor's state + direction, which then sets animation list
        //InputHelper is mapped to this component, AI sets this component's values directly
        public Direction direction = Direction.None;
        public Boolean attack = false;
        public Boolean use = false;
        public Boolean dash = false;
        public Boolean interact = false;
    }

    public class ComponentSprite
    {   //displays a visual representation for an object or actor, or anything else
        public Texture2D texture;
        public Vector2 position;
        public Byte4 currentFrame;
        public Byte2 cellSize;

        public SpriteEffects spriteEffect; //flip vertically, flip horizontally, none
        public Boolean flipHorizontally;

        public Boolean visible;
        public Vector2 origin;
        public Rectangle drawRec;

        public Color drawColor;
        public float alpha;
        public float scale;
        public int zOffset;

        public float zDepth;
        public Rotation rotation;
        public float rotationValue;

        public ComponentSprite(Texture2D Texture, Vector2 Position, Byte4 CurrentFrame, Byte2 CellSize)
        {
            texture = Texture;
            position = Position;
            currentFrame = CurrentFrame;
            cellSize = CellSize;

            spriteEffect = SpriteEffects.None;
            flipHorizontally = false;
            visible = true;
            CenterOrigin();
            drawRec = new Rectangle((int)Position.X, (int)Position.Y, (int)CellSize.x, (int)CellSize.y);

            drawColor = new Color(255, 255, 255);
            alpha = 1.0f;
            scale = 1.0f;
            zOffset = 0;

            SetZdepth();
            rotation = Rotation.None;
            rotationValue = 0.0f;
        }

        public void CenterOrigin() { origin.X = cellSize.x * 0.5f; origin.Y = cellSize.y * 0.5f; }

        public void SetZdepth() { zDepth = 0.999990f - ((position.Y + zOffset) * 0.000001f); }

        public void UpdateCellSize() { drawRec.Width = cellSize.x; drawRec.Height = cellSize.y; }
    }

}