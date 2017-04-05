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



        static DungeonGenerator()
        { }



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
                        obj.Update();

                        if (i == 0)
                        {   //topleft corner
                            GameObject corner = DungeonScreen.objPool.GetObj();
                            corner.compSprite.position.X = -16;
                            corner.compSprite.position.Y = -16;
                            corner.type = GameObject.Type.WallInteriorCorner;
                            corner.direction = Direction.Down;
                            corner.Update();
                        }
                        else if (i == width-1)
                        {   //topright corner
                            GameObject corner = DungeonScreen.objPool.GetObj();
                            corner.compSprite.position.X = width * 16;
                            corner.compSprite.position.Y = -16;
                            corner.type = GameObject.Type.WallInteriorCorner;
                            corner.direction = Direction.Right;
                            corner.Update();
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
                        obj.Update();

                        if (i == 0)
                        {   //topleft corner
                            GameObject corner = DungeonScreen.objPool.GetObj();
                            corner.compSprite.position.X = -16;
                            corner.compSprite.position.Y = height * 16;
                            corner.type = GameObject.Type.WallInteriorCorner;
                            corner.direction = Direction.Left;
                            corner.Update();
                        }
                        else if (i == width - 1)
                        {   //topright corner
                            GameObject corner = DungeonScreen.objPool.GetObj();
                            corner.compSprite.position.X = width * 16;
                            corner.compSprite.position.Y = height * 16;
                            corner.type = GameObject.Type.WallInteriorCorner;
                            corner.direction = Direction.Up;
                            corner.Update();
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
                        obj.Update();
                        DungeonScreen.objPool.counter++;
                    }
                    else if (i == width-1)
                    {   //right side
                        GameObject obj = DungeonScreen.objPool.GetObj();
                        obj.compSprite.position.X = i * 16 + 16;
                        obj.compSprite.position.Y = j * 16;
                        obj.direction = Direction.Right;
                        obj.Update();
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
                actor.SetType(Actor.Type.Blob, 100, 100);
            }


            //center hero to room
            DungeonScreen.hero.compMove.position.X = 150;
            DungeonScreen.hero.compMove.position.Y = 100;
        }





    }
}
