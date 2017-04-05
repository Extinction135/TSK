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
            //reset the pool counters
            DungeonScreen.floorPool.counter = 0;
            DungeonScreen.objPool.counter = 0;

            //build a test room
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    //place the floors
                    ComponentSprite floor = DungeonScreen.floorPool.pool[DungeonScreen.floorPool.counter];
                    floor.position.X = i * 16;
                    floor.position.Y = j * 16;
                    floor.visible = true;
                    DungeonScreen.floorPool.counter++;


                    //place the walls
                    GameObject obj = DungeonScreen.objPool.pool[DungeonScreen.objPool.counter];
                    obj.active = true;
                    if (j == 0)
                    {   //top row
                        obj.compSprite.position.X = i * 16;
                        obj.compSprite.position.Y = 0 * 16 - 16;
                        obj.direction = Direction.Down;
                        obj.Update();
                        DungeonScreen.objPool.counter++;
                    }
                    else if (j == height-1)
                    {   //bottom row
                        obj.compSprite.position.X = i * 16;
                        obj.compSprite.position.Y = height * 16;
                        obj.direction = Direction.Up;
                        obj.Update();
                        DungeonScreen.objPool.counter++;
                    }
                    if (i == 0)
                    {   //left side
                        obj.compSprite.position.X = i * 16;
                        obj.compSprite.position.Y = j * 16;
                        obj.direction = Direction.Left;
                        obj.Update();
                        DungeonScreen.objPool.counter++;
                    }
                    else if (i == width-1)
                    {   //right side
                        obj.compSprite.position.X = i * 16;
                        obj.compSprite.position.Y = j * 16;
                        obj.direction = Direction.Right;
                        obj.Update();
                        DungeonScreen.objPool.counter++;
                    }



                }
            }


            //build top + bottom row of walls
            for (int i = 0; i < width; i++)
            {
                

                
            }
            




            //place enemies within the room

            //center hero to room






        }





    }
}
