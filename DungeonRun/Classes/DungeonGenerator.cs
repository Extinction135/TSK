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
    public static class DungeonGenerator
    {
        public static void CreateRoom(DungeonScreen DungeonScreen)
        {
            //set the width and height of room
            byte width = 20;
            byte height = 10;
            //reset the pools
            DungeonScreen.floorPool.Reset();
            DungeonScreen.objPool.Reset();
            DungeonScreen.actorPool.Reset();
            //reset the pool counters
            DungeonScreen.floorPool.counter = 0;
            DungeonScreen.objPool.counter = 0;
            DungeonScreen.actorPool.counter = 0;

            //build a test room
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    //place the floors
                    ComponentSprite floor = DungeonScreen.floorPool.GetFloor();
                    //ComponentSprite floor = DungeonScreen.floorPool.pool[DungeonScreen.floorPool.counter];
                    floor.position.X = i * 16;
                    floor.position.Y = j * 16;
                    //floor.visible = true;
                    //DungeonScreen.floorPool.counter++;


                    //place the walls

                    #region Top Row

                    if (j == 0)
                    {   //top row
                        GameObject obj = DungeonScreen.objPool.GetObj();
                        obj.compSprite.position.X = i * 16;
                        obj.compSprite.position.Y = 0 * 16 - 16;
                        obj.direction = Direction.Down;
                        GameObjectFunctions.SetType(obj, GameObject.Type.WallStraight);

                        if (i == 0)
                        {   //topleft corner
                            GameObject corner = DungeonScreen.objPool.GetObj();
                            corner.compSprite.position.X = -16;
                            corner.compSprite.position.Y = -16;
                            corner.direction = Direction.Down;
                            GameObjectFunctions.SetType(corner, GameObject.Type.WallInteriorCorner);
                        }
                        else if (i == width-1)
                        {   //topright corner
                            GameObject corner = DungeonScreen.objPool.GetObj();
                            corner.compSprite.position.X = width * 16;
                            corner.compSprite.position.Y = -16;
                            corner.direction = Direction.Right;
                            GameObjectFunctions.SetType(corner, GameObject.Type.WallInteriorCorner);
                        }
                    }

                    #endregion

                    #region Bottom Row

                    else if (j == height-1)
                    {   //bottom row
                        GameObject obj = DungeonScreen.objPool.GetObj();
                        obj.compSprite.position.X = i * 16;
                        obj.compSprite.position.Y = height * 16;
                        obj.direction = Direction.Up;
                        GameObjectFunctions.SetType(obj, GameObject.Type.WallStraight);

                        if (i == 0)
                        {   //topleft corner
                            GameObject corner = DungeonScreen.objPool.GetObj();
                            corner.compSprite.position.X = -16;
                            corner.compSprite.position.Y = height * 16;
                            corner.direction = Direction.Left;
                            GameObjectFunctions.SetType(corner, GameObject.Type.WallInteriorCorner);
                        }
                        else if (i == width - 1)
                        {   //topright corner
                            GameObject corner = DungeonScreen.objPool.GetObj();
                            corner.compSprite.position.X = width * 16;
                            corner.compSprite.position.Y = height * 16;
                            corner.direction = Direction.Up;
                            GameObjectFunctions.SetType(corner, GameObject.Type.WallInteriorCorner);
                        }
                    }

                    #endregion

                    #region Left & Right Columns

                    if (i == 0)
                    {   //left side
                        GameObject obj = DungeonScreen.objPool.GetObj();
                        obj.compSprite.position.X = i * 16 - 16;
                        obj.compSprite.position.Y = j * 16;
                        obj.direction = Direction.Left;
                        GameObjectFunctions.SetType(obj, GameObject.Type.WallStraight);
                        DungeonScreen.objPool.counter++;
                    }
                    else if (i == width-1)
                    {   //right side
                        GameObject obj = DungeonScreen.objPool.GetObj();
                        obj.compSprite.position.X = i * 16 + 16;
                        obj.compSprite.position.Y = j * 16;
                        obj.direction = Direction.Right;
                        GameObjectFunctions.SetType(obj, GameObject.Type.WallStraight);
                        DungeonScreen.objPool.counter++;
                    }

                    #endregion


                }
            }

            //set the room enemy count
            byte enemyCount = 10;
            //place enemies within the room
            for (int i = 0; i < enemyCount; i++)
            {
                //Actor actor = DungeonScreen.actorPool.pool[DungeonScreen.actorPool.counter];
                Actor actor = DungeonScreen.actorPool.GetActor();
                ActorFunctions.SetType(actor, Actor.Type.Blob);
                ActorFunctions.Teleport(actor, Global.Random.Next(20, 180), Global.Random.Next(20, 80));
            }

            //center hero to room
            DungeonScreen.hero.compMove.position.X = 150;
            DungeonScreen.hero.compMove.position.Y = 100;
        }





    }
}
