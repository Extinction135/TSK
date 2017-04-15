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
    public static class DungeonRecord
    {

        public static int dungeonID;
        public static Boolean beatDungeon;

        public static Stopwatch timer;
        public static int enemyCount;
        public static int totalDamage;

        public static void Reset()
        {
            dungeonID = 0;
            beatDungeon = false;

            timer = new Stopwatch();
            timer.Reset();
            enemyCount = 0;
            totalDamage = 0;
        }

    }
}