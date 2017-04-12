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
    public static class WorldUI
    {

        public static List<ComponentSprite> hearts;
        public static List<ComponentSprite> weaponBkg;
        public static List<ComponentSprite> itemBkg;
        public static ComponentSprite currentWeapon;
        public static ComponentSprite currentItem;
        public static ComponentText frametime;
        public static int counter;

        public static void CreateRow(int Xpos, int Ypos)
        {
            for (counter = 0; counter < 7; counter++)
            {
                hearts.Add(new ComponentSprite(Assets.uiSheet,
                    new Vector2(Xpos + 11 * counter, Ypos),
                    new Byte4(0, 0, 0, 0), new Byte2(16, 16)));
            }
        }

        public static List<ComponentSprite> CreateBkg(int Xpos, int Ypos)
        {
            List<ComponentSprite> background = new List<ComponentSprite>();
            background.Add(new ComponentSprite(Assets.uiSheet, new Vector2(Xpos, Ypos),
                new Byte4(4, 0, 0, 0), new Byte2(16, 16)));
            background.Add(new ComponentSprite(Assets.uiSheet, new Vector2(Xpos + 16, Ypos),
                new Byte4(4, 0, 1, 0), new Byte2(16, 16)));
            background.Add(new ComponentSprite(Assets.uiSheet, new Vector2(Xpos, Ypos + 16),
                new Byte4(4, 0, 1, 0), new Byte2(16, 16)));
            background.Add(new ComponentSprite(Assets.uiSheet, new Vector2(Xpos + 16, Ypos + 16),
                new Byte4(4, 0, 0, 0), new Byte2(16, 16)));
            background[2].rotation = Rotation.Clockwise180;
            background[3].rotation = Rotation.Clockwise180;
            return background;
        }

        public static void Initialize()
        {
            Point UIpos = new Point(255, 32); //center aligned
            UIpos.X = 32; //left aligned
            hearts = new List<ComponentSprite>();
            
            weaponBkg = CreateBkg(UIpos.X, UIpos.Y);
            CreateRow(UIpos.X + 30, UIpos.Y + 01);
            CreateRow(UIpos.X + 30, UIpos.Y + 14);
            itemBkg = CreateBkg(UIpos.X + 110, UIpos.Y);

            //create the current weapon sprite
            currentWeapon = new ComponentSprite(Assets.uiSheet,
                new Vector2(UIpos.X + 8, UIpos.Y + 8),
                new Byte4(0, 5, 0, 0), new Byte2(16, 16));

            //create the current item sprite
            currentItem = new ComponentSprite(Assets.uiSheet, 
                new Vector2(UIpos.X + 8 + 110, UIpos.Y + 8),
                new Byte4(0, 4, 0, 0), new Byte2(16, 16));

            //create the frametime text component
            frametime = new ComponentText(Assets.font, "test", new Vector2(UIpos.X + 136, UIpos.Y - 8), Assets.colorScheme.textSmall);
        }


        public static void Update()
        {
            //set the hearts based on the player's HP value
        }

        public static void Draw()
        {
            for (counter = 0; counter < hearts.Count; counter++)
            {
                //check to see if sprite is visible
                DrawFunctions.Draw(hearts[counter]);
            }
            for (counter = 0; counter < itemBkg.Count; counter++)
            {
                DrawFunctions.Draw(weaponBkg[counter]);
                DrawFunctions.Draw(itemBkg[counter]);
            }
            DrawFunctions.Draw(currentWeapon);
            DrawFunctions.Draw(currentItem);
            if(!Flags.Release)
            {
                frametime.text = Timing.updateTime.Milliseconds + " ms\n";
                frametime.text += Timing.drawTime.Milliseconds + " ms\n";
                frametime.text += Timing.totalTime.Milliseconds + " ms";
                DrawFunctions.Draw(frametime);
            }
        }

    }
}