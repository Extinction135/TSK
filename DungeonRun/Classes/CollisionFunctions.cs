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
    public static class CollisionFunctions
    {





        /*
        public static void Move(ComponentMovement Move, ComponentCollision Coll, ComponentSprite Sprite, DungeonScreen DungeonScreen)
        {
            ProjectMovement(Move);
            CheckCollisions(Move, Coll, DungeonScreen);
            PlaceSpriteToCollision(Coll, Sprite);
        }
        */

        public static void Move(Actor Actor, DungeonScreen DungeonScreen)
        {
            ProjectMovement(Actor.compMove);
            CheckCollisions(Actor.compMove, Actor.compCollision, DungeonScreen);
            PlaceSpriteToCollision(Actor.compCollision, Actor.compSprite);
        }

        /*
        public static void Move(GameObject Obj, DungeonScreen DungeonScreen)
        {
            ProjectMovement(Obj.compMove);
            CheckCollisions(Obj.compMove, Obj.compCollision);
            PlaceSpriteToCollision(Obj.compCollision, Obj.compSprite);
        }
        */



        public static void ProjectMovement(ComponentMovement Move)
        {
            //calculate magnitude
            if (Move.direction == Direction.Down)
            { Move.magnitude.Y += Move.speed; }
            else if (Move.direction == Direction.Left)
            { Move.magnitude.X -= Move.speed; }
            else if (Move.direction == Direction.Right)
            { Move.magnitude.X += Move.speed; }
            else if (Move.direction == Direction.Up)
            { Move.magnitude.Y -= Move.speed; }
            else if (Move.direction == Direction.DownLeft)
            { Move.magnitude.Y += Move.speed * 0.75f; Move.magnitude.X -= Move.speed * 0.75f; }
            else if (Move.direction == Direction.DownRight)
            { Move.magnitude.Y += Move.speed * 0.75f; Move.magnitude.X += Move.speed * 0.75f; }
            else if (Move.direction == Direction.UpLeft)
            { Move.magnitude.Y -= Move.speed * 0.75f; Move.magnitude.X -= Move.speed * 0.75f; }
            else if (Move.direction == Direction.UpRight)
            { Move.magnitude.Y -= Move.speed * 0.75f; Move.magnitude.X += Move.speed * 0.75f; }
            //apply friction to magnitude, clip magnitude to 0 when it gets very small
            Move.magnitude = Move.magnitude * Move.friction;
            if (Math.Abs(Move.magnitude.X) < 0.01f) { Move.magnitude.X = 0; }
            if (Math.Abs(Move.magnitude.Y) < 0.01f) { Move.magnitude.Y = 0; }
            //project newPosition based on current position + magnitude
            Move.newPosition.X = Move.position.X;
            Move.newPosition.Y = Move.position.Y;
            Move.newPosition += Move.magnitude;
        }





        static Boolean collisionX;
        static Boolean collisionY;
        public static void CheckCollisions(ComponentMovement Move, ComponentCollision Coll, DungeonScreen DungeonScreen)
        {
            Coll.blocking = false; //this is to prevent the object from colliding with itself

            #region Project collisionRec along X axis

            collisionX = false;
            Coll.rec.X = (int)Move.newPosition.X; //project collisionRec along X axis
            for (int i = 0; i < DungeonScreen.objPool.poolSize; i++)
            {   //check collisions against objects
                if (Coll.rec.Intersects(DungeonScreen.objPool.pool[i].compCollision.rec))
                { collisionX = true; }
            }
            for (int i = 0; i < DungeonScreen.actorPool.poolSize; i++)
            {   //check collisions against actors
                if (Coll.rec.Intersects(DungeonScreen.actorPool.pool[i].compCollision.rec) & DungeonScreen.actorPool.pool[i].compCollision.blocking)
                { collisionX = true; }
            }
            if (collisionX) { Move.newPosition.X = Move.position.X; }
            Coll.rec.X = (int)Move.newPosition.X;
            Move.position.X = Move.newPosition.X;

            #endregion

            #region Project collisionRec along Y axis

            collisionY = false;
            Coll.rec.Y = (int)Move.newPosition.Y; //
            for (int i = 0; i < DungeonScreen.objPool.poolSize; i++)
            {   //check collisions against objects
                if (Coll.rec.Intersects(DungeonScreen.objPool.pool[i].compCollision.rec))
                { collisionY = true; }
            }
            for (int i = 0; i < DungeonScreen.actorPool.poolSize; i++)
            {   //check collisions against actors
                if (Coll.rec.Intersects(DungeonScreen.actorPool.pool[i].compCollision.rec) & DungeonScreen.actorPool.pool[i].compCollision.blocking)
                { collisionY = true; }
            }
            if (collisionY) { Move.newPosition.Y = Move.position.Y; }
            Coll.rec.Y = (int)Move.newPosition.Y;
            Move.position.Y = Move.newPosition.Y;

            #endregion

            Coll.blocking = true; //turn this object's blocking back on
        }








        public static void PlaceSpriteToCollision(ComponentCollision Coll, ComponentSprite Sprite)
        {   //set sprite.pos to collisionRec.pos
            Sprite.position.X = (int)(Coll.rec.X + Coll.rec.Width / 2 - Coll.offsetX);
            Sprite.position.Y = (int)(Coll.rec.Y + Coll.rec.Height / 2 - Coll.offsetY);
            Sprite.SetZdepth();
        }

        public static void PlaceCollisionToSprite(ComponentCollision Coll, ComponentSprite Sprite)
        {   //set collisionRec.pos to sprite.pos
            Coll.rec.X = (int)Sprite.position.X - Sprite.cellSize.x / 2;// + Coll.offsetX;
            Coll.rec.Y = (int)Sprite.position.Y - Sprite.cellSize.y / 2;// + Coll.offsetY;
        }
    }
}