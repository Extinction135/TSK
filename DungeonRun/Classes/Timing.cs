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
    public static class Timing
    {
        public static Stopwatch stopWatch = new Stopwatch();
        public static Stopwatch total = new Stopwatch();
        public static TimeSpan updateTime;
        public static TimeSpan drawTime;
        public static TimeSpan totalTime;

        public static void Initialize()
        {
            stopWatch = new Stopwatch();
            total = new Stopwatch();
            updateTime = new TimeSpan();
            drawTime = new TimeSpan();
            totalTime = new TimeSpan();
        }

        public static void Reset() { stopWatch.Reset(); stopWatch.Start(); }

    }
}