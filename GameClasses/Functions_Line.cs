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
    public static class Functions_Line
    {
        static int i;

        public static void AddLine(int StartPosX, int StartPosY, int EndPosX, int EndPosY)
        {
            for(i = 0; i < Pool.lineCount; i++)
            {
                if (Pool.linePool[i].visible == false)
                {
                    //setup this line with startpos, end pos, bail
                    Pool.linePool[i].visible = true; //begin drawing this line
                    Pool.linePool[i].startPosX = StartPosX;
                    Pool.linePool[i].startPosY = StartPosY;
                    Pool.linePool[i].endPosX = EndPosX;
                    Pool.linePool[i].endPosY = EndPosY;
                    return;
                }
            }
        }

        public static void RemoveLine(Line Line)
        {
            Line.visible = false; //line is no longer drawn/is recycled
        }

		public static void UpdateLine(Line Line)
        {
            //set angle
            Line.angle = (float)Math.Atan2(
                (double)(Line.startPosY - Line.endPosY),
                (double)(Line.startPosX - Line.endPosX)
                );
             
            //set length
            Line.length = (int)Vector2.Distance(
                new Vector2(Line.endPosX, Line.endPosY), 
                new Vector2(Line.startPosX, Line.startPosY));

            //update the rectangle parameters based on length, angle
            Line.rec.X = Line.endPosX;
            Line.rec.Y = Line.endPosY;
            Line.rec.Width = (int)Line.length;
            Line.rec.Height = 1;

            SetZdepth(Line);
        }

        public static void SetZdepth(Line Line)
        {
            Line.zDepth = 0.999990f - Line.startPosY * 0.000001f;
        }
    }
}