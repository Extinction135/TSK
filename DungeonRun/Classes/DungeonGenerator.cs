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
            //the room should have an overall position
            int positionX = 16 * 10;
            int positionY = 16 * 10;

            //set the width and height of room
            byte width = 20;
            byte height = 10;
            //reset the pools + counter
            PoolFunctions.Reset();
            Pool.counter = 0;

            //build a test room
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    //place the floors
                    ComponentSprite floor = PoolFunctions.GetFloor();
                    floor.position.X = i * 16 + positionX;
                    floor.position.Y = j * 16 + positionY;
                    

                    #region Top Row Walls

                    if (j == 0)
                    {   
                        //top row
                        GameObject obj = PoolFunctions.GetObj();
                        MovementFunctions.Teleport(obj.compMove, i * 16 + positionX, 0 * 16 - 16 + positionY);
                        obj.direction = Direction.Down;
                        GameObjectFunctions.SetType(obj, GameObject.Type.WallStraight);
                        if (i == 0)
                        {   //topleft corner
                            GameObject corner = PoolFunctions.GetObj();
                            MovementFunctions.Teleport(corner.compMove, -16 + positionX, -16 + positionY);
                            corner.direction = Direction.Down;
                            GameObjectFunctions.SetType(corner, GameObject.Type.WallInteriorCorner);
                        }
                        else if (i == width-1)
                        {   //topright corner
                            GameObject corner = PoolFunctions.GetObj();
                            MovementFunctions.Teleport(corner.compMove, width * 16 + positionX, -16 + positionY);
                            corner.direction = Direction.Left;
                            GameObjectFunctions.SetType(corner, GameObject.Type.WallInteriorCorner);
                        }
                    }

                    #endregion


                    #region Bottom Row Walls

                    else if (j == height-1)
                    {   //bottom row
                        GameObject obj = PoolFunctions.GetObj();
                        MovementFunctions.Teleport(obj.compMove, i * 16 + positionX, height * 16 + positionY);
                        obj.direction = Direction.Up;
                        GameObjectFunctions.SetType(obj, GameObject.Type.WallStraight);
                        if (i == 0)
                        {   //bottom left corner
                            GameObject corner = PoolFunctions.GetObj();
                            MovementFunctions.Teleport(corner.compMove, -16 + positionX, height * 16 + positionY);
                            corner.direction = Direction.Right;
                            GameObjectFunctions.SetType(corner, GameObject.Type.WallInteriorCorner);
                        }
                        else if (i == width - 1)
                        {   //bottom right corner
                            GameObject corner = PoolFunctions.GetObj();
                            MovementFunctions.Teleport(corner.compMove, width * 16 + positionX, height * 16 + positionY);
                            corner.direction = Direction.Up;
                            GameObjectFunctions.SetType(corner, GameObject.Type.WallInteriorCorner);
                        }
                    }

                    #endregion


                    #region Left & Right Column Walls

                    if (i == 0)
                    {   //left side
                        GameObject obj = PoolFunctions.GetObj();
                        MovementFunctions.Teleport(obj.compMove, i * 16 - 16 + positionX, j * 16 + positionY);
                        obj.direction = Direction.Right;
                        GameObjectFunctions.SetType(obj, GameObject.Type.WallStraight);
                    }
                    else if (i == width-1)
                    {   //right side
                        GameObject obj = PoolFunctions.GetObj();
                        MovementFunctions.Teleport(obj.compMove, i * 16 + 16 + positionX, j * 16 + positionY);
                        obj.direction = Direction.Left;
                        GameObjectFunctions.SetType(obj, GameObject.Type.WallStraight);
                    }

                    #endregion
                    
                }
            }

            //set the room enemy count
            byte enemyCount = 10;
            //place enemies within the room
            for (int i = 0; i < enemyCount; i++)
            {
                Actor actor = PoolFunctions.GetActor();
                ActorFunctions.SetType(actor, Actor.Type.Blob);
                MovementFunctions.Teleport(actor.compMove,
                    GetRandom.Int(20, 180) + positionX,
                    GetRandom.Int(20, 80) + positionY);
            }

            //center hero to room
            ActorFunctions.SetType(Pool.hero, Actor.Type.Hero);
            MovementFunctions.Teleport(Pool.hero.compMove, 
                150 + positionX, 100 + positionY);
        }





    }
}