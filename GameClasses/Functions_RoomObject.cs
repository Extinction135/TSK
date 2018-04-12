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
    public static class Functions_RoomObject
    {
        //static int i;

        public static GameObject SpawnRoomObj(ObjType Type, float X, float Y, Direction Direction)
        {   //spawns RoomObject at the X, Y location, with direction
            GameObject obj = Functions_Pool.GetRoomObj();
            //set direction
            obj.direction = Direction;
            obj.compMove.direction = Direction;
            //teleport the projectile to the proper location
            Functions_Movement.Teleport(obj.compMove, X, Y);
            //set the type, rotation, cellsize, & alignment
            Functions_GameObject.SetType(obj, Type);
            return obj;
        }

        public static void HandleCommon(GameObject RoomObj, Direction HitDirection)
        {   
            //hitDirection is used to push some objects in the direction they were hit

            //dungeon objects
            if (RoomObj.type == ObjType.Dungeon_Pot)
            {
                DestroyObject(RoomObj, true, true);
            }
            else if (RoomObj.type == ObjType.Dungeon_Barrel)
            {
                RoomObj.compMove.direction = HitDirection; //pass hitDirection
                Functions_GameObject_Dungeon.DestroyBarrel(RoomObj);
            }
            else if (RoomObj.type == ObjType.Dungeon_SwitchBlockBtn)
            {
                Functions_GameObject_Dungeon.FlipSwitchBlocks(RoomObj);
            }
            else if (RoomObj.type == ObjType.Dungeon_LeverOff 
                || RoomObj.type == ObjType.Dungeon_LeverOn)
            {
                Functions_GameObject_Dungeon.ActivateLeverObjects();
            }
        }

        public static void DestroyObject(GameObject RoomObj, Boolean releaseObj, Boolean spawnLoot)
        {   //grab players attention, spawn rock debris, play shatter sound
            Functions_Particle.Spawn(
                ObjType.Particle_Attention,
                RoomObj.compSprite.position.X,
                RoomObj.compSprite.position.Y);
            //Functions_Particle.ScatterDebris(RoomObj.compSprite.position);
            //Functions_Particle.ScatterDebris(RoomObj.compSprite.position);
            //Functions_Particle.ScatterDebris(RoomObj.compSprite.position);
            Assets.Play(Assets.sfxShatter);
            //handle parameter values
            if (spawnLoot) { Functions_Loot.SpawnLoot(RoomObj.compSprite.position); }
            if (releaseObj) { Functions_Pool.Release(RoomObj); }
        }

        public static void AlignRoomObjs()
        {   //align sprite + collision comps to move comp of all active objs
            for (Pool.roomObjCounter = 0; Pool.roomObjCounter < Pool.roomObjCount; Pool.roomObjCounter++)
            {
                if (Pool.roomObjPool[Pool.roomObjCounter].active)
                {   //align the sprite and collision components to the move component
                    Functions_Component.Align(
                        Pool.roomObjPool[Pool.roomObjCounter].compMove,
                        Pool.roomObjPool[Pool.roomObjCounter].compSprite,
                        Pool.roomObjPool[Pool.roomObjCounter].compCollision);
                    //set the current animation frame, check the animation counter
                    Functions_Animation.Animate(Pool.roomObjPool[Pool.roomObjCounter].compAnim,
                        Pool.roomObjPool[Pool.roomObjCounter].compSprite);
                    //set the rotation for the obj's sprite
                    Functions_GameObject.SetRotation(Pool.roomObjPool[Pool.roomObjCounter]);
                }
            }
        }

        //decorates a door on left/right or top/bottom
        static Vector2 posA = new Vector2();
        static Vector2 posB = new Vector2();
        public static void DecorateDoor(GameObject Door, ObjType Type)
        {
            if (Door.direction == Direction.Up || Door.direction == Direction.Down)
            {   //build left/right decorations if Door.direction is Up or Down
                posA.X = Door.compSprite.position.X - 16;
                posA.Y = Door.compSprite.position.Y;
                posB.X = Door.compSprite.position.X + 16;
                posB.Y = Door.compSprite.position.Y;
            }
            else
            {   //build top/bottom decorations if Door.direction is Left or Right
                posA.X = Door.compSprite.position.X;
                posA.Y = Door.compSprite.position.Y - 16;
                posB.X = Door.compSprite.position.X;
                posB.Y = Door.compSprite.position.Y + 16;
            }
            //build wall decorationA torch/pillar/decoration
            SpawnRoomObj(Type, posA.X, posA.Y, Door.direction);
            //build wall decorationB torch/pillar/decoration
            SpawnRoomObj(Type, posB.X, posB.Y, Door.direction);
        }

    }
}