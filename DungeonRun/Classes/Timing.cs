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
        public static TimeSpan updateTime = new TimeSpan();
        public static TimeSpan drawTime = new TimeSpan();
        public static TimeSpan totalTime = new TimeSpan();
        public static void Reset() { stopWatch.Reset(); stopWatch.Start(); }

    }
}