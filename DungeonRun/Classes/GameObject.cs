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
    public class GameObject
    {

        public ObjGroup group;
        public ObjType type;

        public ComponentSprite compSprite;
        public ComponentCollision compCollision;
        public ComponentAnimation compAnim;
        public ComponentMovement compMove;

        public Direction direction;
        public Boolean active; //does this object draw, update?

        public Byte lifetime;   //how many frames this object exists for, 0 = forever/ignore
        public Byte lifeCounter;//counts up to lifetime value

        public GameObject(Texture2D Texture)
        {   //initialize to default value - this data is changed in Update()
            group = ObjGroup.Object;
            type = ObjType.WallStraight;
            compSprite = new ComponentSprite(Texture, new Vector2(50, 50), new Byte4(0, 0, 0, 0), new Point(16, 16));
            compCollision = new ComponentCollision();
            compAnim = new ComponentAnimation();
            compMove = new ComponentMovement();
            direction = Direction.Down;
            active = true;
            GameObjectFunctions.SetType(this, type);
        }

    }
}