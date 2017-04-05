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
    public static class GameObjectFunctions
    {
        public static void SetType(GameObject Obj, GameObject.Type Type)
        {
            Obj.type = Type;
            //all gameobjects exist on the dungeon sheet, so the texture never needs to be changed

            //sprites are created facing Down, but we will need to set the spite rotation based on direction
            Obj.compSprite.rotation = Rotation.None; //reset sprite rotation to default DOWN
            if (Obj.direction == Direction.Up) { Obj.compSprite.rotation = Rotation.Clockwise180; }
            else if (Obj.direction == Direction.Right) { Obj.compSprite.rotation = Rotation.Clockwise90; }
            else if (Obj.direction == Direction.Left) { Obj.compSprite.rotation = Rotation.Clockwise270; }

            //update the object's current animation based on it's type
            GameObjectAnimListManager.SetAnimationList(Obj);

            #region Set Default Values (Assumptions)

            //assume cell size is 16x16 (most are)
            Obj.compSprite.cellSize.x = 16 * 1;
            Obj.compSprite.cellSize.y = 16 * 1;
            Obj.compCollision.blocking = true; //assume the object is blocking (most are)
            Obj.objGroup = GameObject.ObjGroup.Object; //assume object is a generic object
            Obj.compCollision.rec.Width = 16; //assume collisionRec is 16x16
            Obj.compCollision.rec.Height = 16; //(most are)
            Obj.compCollision.offsetX = -8; //assume collisionRec offset is -8x-8
            Obj.compCollision.offsetY = -8; //(most are)
            Obj.compSprite.zOffset = 0;

            #endregion

            #region Room Objects

            if (Type == GameObject.Type.Exit)
            {
                Obj.compSprite.cellSize.y = 16 * 3; //nonstandard size
                Obj.objGroup = GameObject.ObjGroup.Door;
                Obj.compCollision.rec.Height = 2;
                Obj.compCollision.offsetY = 32 + 6;
                Obj.compCollision.blocking = false;
                //sorts to floor layer (or has very positive zDepth)
            }
            else if (
                Type == GameObject.Type.ExitPillarLeft || 
                Type == GameObject.Type.ExitPillarRight)
            {
                Obj.compSprite.cellSize.y = 16 * 3; //nonstandard size
                Obj.compCollision.rec.Height = 32 - 5;
                Obj.compCollision.offsetY = 14;
            }
            else if (Type == GameObject.Type.ExitLightFX)
            {
                Obj.compSprite.cellSize.y = 16 * 2; //nonstandard size
                Obj.compCollision.offsetY = 0;
                //sorts to roof layer (or has very negative zDepth)
            }


            else if (
                Type == GameObject.Type.DoorOpen ||
                Type == GameObject.Type.DoorBombed ||
                Type == GameObject.Type.DoorTrap)
            {
                Obj.compCollision.blocking = false;
                if (Obj.direction == Direction.Down)
                { Obj.compSprite.zOffset = 4; } else { Obj.compSprite.zOffset = 16; }
                Obj.objGroup = GameObject.ObjGroup.Door;
            }
            else if (
                Type == GameObject.Type.DoorBombable ||
                Type == GameObject.Type.DoorBoss ||
                Type == GameObject.Type.DoorShut ||
                Type == GameObject.Type.DoorFake)
            {
                Obj.objGroup = GameObject.ObjGroup.Door;
            }


            else if (
                Type == GameObject.Type.WallStraight ||
                Type == GameObject.Type.WallStraightCracked ||
                Type == GameObject.Type.WallInteriorCorner ||
                Type == GameObject.Type.WallExteriorCorner ||
                Type == GameObject.Type.WallPillar ||
                Type == GameObject.Type.WallDecoration)
            {
                Obj.objGroup = GameObject.ObjGroup.Wall;
            }


            else if (Type == GameObject.Type.PitTop)
            {
                //pits dont collide with actors
            }
            else if (Type == GameObject.Type.PitBottom)
            {
                //instead we'll use an animated liquid obj for collision checking
                //pits just sit ontop of this object as decoration
            }
            else if (
                Type == GameObject.Type.PitTrapReady || Type == GameObject.Type.PitTrapOpening)
            {
                Obj.compCollision.offsetX = -6;
                Obj.compCollision.offsetY = -6;
            }


            else if (Type == GameObject.Type.BossStatue)
            {
                Obj.objGroup = GameObject.ObjGroup.Draggable;
                Obj.compCollision.rec.Height = 8;
                Obj.compCollision.offsetY = -1;
            }
            else if (Type == GameObject.Type.BossDecal)
            {
                Obj.compSprite.zOffset = -32;
                Obj.compCollision.blocking = false;
            }
            else if (Type == GameObject.Type.Pillar)
            {
                Obj.compSprite.zOffset = 2;
            }
            else if (Type == GameObject.Type.WallTorch)
            {
                Obj.compCollision.blocking = false;
            }

            #endregion

            #region Interactive Objects

            else if (Type == GameObject.Type.Chest || Type == GameObject.Type.ChestEmpty)
            {
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -3;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 11;
                Obj.compSprite.zOffset = -7;
            }


            else if (Type == GameObject.Type.BlockDraggable)
            {
                Obj.compCollision.rec.Height = 12;
                Obj.compCollision.offsetY = -4;
                Obj.compSprite.zOffset = -7;
                Obj.objGroup = GameObject.ObjGroup.Draggable;
            }
            else if (Type == GameObject.Type.BlockDark || Type == GameObject.Type.BlockLight)
            {
                Obj.compSprite.zOffset = -7;
            }
            else if (Type == GameObject.Type.BlockSpikes)
            {
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -7;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 14;
                Obj.compSprite.zOffset = -7;
                Obj.compCollision.blocking = false;
            }

            else if (Type == GameObject.Type.Lever)
            {
                Obj.compCollision.offsetX = -6; Obj.compCollision.offsetY = 1;
                Obj.compCollision.rec.Width = 12; Obj.compCollision.rec.Height = 3;
                Obj.compSprite.zOffset = -7;
            }
            else if (Type == GameObject.Type.PotSkull)
            {
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -4;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 12;
                Obj.compSprite.zOffset = -7;
                Obj.objGroup = GameObject.ObjGroup.Liftable;
            }


            else if (
                Type == GameObject.Type.SpikesFloor ||
                Type == GameObject.Type.Flamethrower ||
                Type == GameObject.Type.Switch ||
                Type == GameObject.Type.Bridge)
            {
                Obj.compSprite.zOffset = -32;
                Obj.compCollision.blocking = false;
            }
            else if (Type == GameObject.Type.Bumper)
            {
                Obj.compCollision.blocking = false;
            }


            else if (Type == GameObject.Type.SwitchBlockBtn)
            {
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -4;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 12;
                Obj.compSprite.zOffset = -7;
            }
            else if (Type == GameObject.Type.SwitchBlockDown)
            {
                Obj.compSprite.zOffset = -16;
                Obj.compCollision.blocking = false;
            }
            else if (Type == GameObject.Type.SwitchBlockUp)
            {
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -7;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 14;
                Obj.compSprite.zOffset = -7;
            }


            else if (Type == GameObject.Type.TorchUnlit || Type == GameObject.Type.TorchLit)
            {
                Obj.compCollision.offsetX = -7; Obj.compCollision.offsetY = -4;
                Obj.compCollision.rec.Width = 14; Obj.compCollision.rec.Height = 12;
                Obj.compSprite.zOffset = -7;
            }
            else if (Type == GameObject.Type.ConveyorBelt)
            {
                Obj.compSprite.zOffset = -32;
                Obj.compCollision.blocking = false;
                //directions are slightly different for this obj
                if (Obj.direction == Direction.Right) { Obj.compSprite.rotation = Rotation.Clockwise270; }
                else if (Obj.direction == Direction.Left) { Obj.compSprite.rotation = Rotation.Clockwise90; }
            }

            #endregion

            #region Items

            else if (Type == GameObject.Type.ItemRupee)
            {
                Obj.compSprite.cellSize.x = 8; //non standard cellsize
                Obj.compCollision.offsetX = -4; Obj.compCollision.offsetY = -5;
                Obj.compCollision.rec.Width = 8; Obj.compCollision.rec.Height = 10;
                Obj.compCollision.blocking = false;
                Obj.objGroup = GameObject.ObjGroup.Item;
            }
            else if (Type == GameObject.Type.ItemHeart)
            {
                Obj.compSprite.cellSize.x = 8; //non standard cellsize
                Obj.compCollision.offsetX = -4; Obj.compCollision.offsetY = -3;
                Obj.compCollision.rec.Width = 8; Obj.compCollision.rec.Height = 7;
                Obj.compCollision.blocking = false;
                Obj.objGroup = GameObject.ObjGroup.Item;
            }
            else if (Type == GameObject.Type.ItemMap || Type == GameObject.Type.ItemBigKey)
            {
                Obj.compCollision.offsetX = -5; Obj.compCollision.offsetY = -4;
                Obj.compCollision.rec.Width = 10; Obj.compCollision.rec.Height = 12;
                Obj.compSprite.zOffset = -7;
                Obj.compCollision.blocking = false;
                Obj.objGroup = GameObject.ObjGroup.Item;
            }
            else if (Type == GameObject.Type.ItemHeartPiece || Type == GameObject.Type.ItemGold50)
            {
                Obj.compCollision.blocking = false;
                Obj.objGroup = GameObject.ObjGroup.Reward;
            }

            #endregion

            CollisionFunctions.PlaceCollisionToSprite(Obj.compCollision, Obj.compSprite);
        }


        


        public static void Draw(GameObject Obj, ScreenManager ScreenManager)
        {
            DrawFunctions.Draw(Obj.compSprite, ScreenManager);
            DrawFunctions.Draw(Obj.compCollision, ScreenManager);
        }
    }
}