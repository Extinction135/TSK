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
    public static class Assets
    {
        static ContentManager content;

        public static Texture2D dummyTexture;
        public static SpriteFont font;

        //the color scheme for the game
        public static ColorScheme colorScheme;

        //the sprite sheets used in the game
        public static Texture2D heroSheet;
        public static Texture2D blobSheet;


        public static Texture2D dungeonSheet;
        public static Texture2D particleSheet;
        public static Texture2D uiSheet;

        //soundfx

        public static void Load(GraphicsDevice GraphicsDevice, ContentManager ContentManager)
        {
            content = ContentManager;
            dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });

            //fonts
            font = content.Load<SpriteFont>(@"pixelFont");

            //actor textures
            heroSheet = content.Load<Texture2D>(@"HeroSheet");
            blobSheet = content.Load<Texture2D>(@"BlobSheet");

            //game textures
            dungeonSheet = content.Load<Texture2D>(@"DungeonSheet");
            particleSheet = content.Load<Texture2D>(@"ParticlesProjectilesSheet");
            uiSheet = content.Load<Texture2D>(@"UISheet");

            //soundfx
        }


        public static void SetDefaultColorScheme()
        {
            //setup default color scheme
            colorScheme = new ColorScheme();

            colorScheme.background = new Color(100, 100, 100, 255);
            colorScheme.textSmall = new Color(255, 255, 255, 255);
            colorScheme.windowBkg = new Color(0, 0, 0, 200);
            colorScheme.collisionActor = new Color(100, 0, 0, 0);
            colorScheme.collisionObj = new Color(100, 0, 0, 0);

            colorScheme.buttonUp = new Color(44, 44, 44, 0);
            colorScheme.buttonOver = new Color(66, 66, 66, 0);
            colorScheme.buttonDown = new Color(100, 100, 100, 0);
        }
    }
}