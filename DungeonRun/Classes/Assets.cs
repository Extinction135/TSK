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

        //fonts
        public static SpriteFont font;
        public static SpriteFont medFont;

        //the color scheme for the game
        public static ColorScheme colorScheme;

        //actor sheets
        public static Texture2D heroSheet;
        public static Texture2D blobSheet;

        //world/ui sheets
        public static Texture2D dungeonSheet;
        public static Texture2D particleSheet;
        public static Texture2D uiSheet;
        public static Texture2D bigTextSheet;

        //soundfx
        static SoundEffect musicTrack;
        public static SoundEffectInstance music;





        public static void Load(GraphicsDevice GraphicsDevice, ContentManager ContentManager)
        {
            content = ContentManager;
            dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });

            //fonts
            font = content.Load<SpriteFont>(@"pixelFont");
            medFont = content.Load<SpriteFont>(@"mediumFont");

            //actor textures
            heroSheet = content.Load<Texture2D>(@"HeroSheet");
            blobSheet = content.Load<Texture2D>(@"BlobSheet");

            //game textures
            dungeonSheet = content.Load<Texture2D>(@"DungeonSheet");
            particleSheet = content.Load<Texture2D>(@"ParticlesProjectilesSheet");
            uiSheet = content.Load<Texture2D>(@"UISheet");
            bigTextSheet = content.Load<Texture2D>(@"BigTextSheet");

            //soundfx
            
            musicTrack = content.Load<SoundEffect>(@"MusicDungeonA");
            music = musicTrack.CreateInstance();
            music.IsLooped = true;
            music.Play();
            
        }


        





        public static void SetDefaultColorScheme()
        {
            //setup default color scheme
            colorScheme = new ColorScheme();

            colorScheme.background = new Color(100, 100, 100, 255);
            colorScheme.overlay = new Color(0, 0, 0, 255);

            colorScheme.textSmall = new Color(255, 255, 255, 255);
            colorScheme.windowBkg = new Color(0, 0, 0, 200);
            colorScheme.collisionActor = new Color(100, 0, 0, 0);
            colorScheme.collisionObj = new Color(100, 0, 0, 0);

            colorScheme.buttonUp = new Color(44, 44, 44, 0);
            colorScheme.buttonOver = new Color(66, 66, 66, 0);
            colorScheme.buttonDown = new Color(100, 100, 100, 0);
        }

    }

    public struct ColorScheme
    {
        public Color background;
        public Color overlay;

        public Color textSmall;
        public Color windowBkg;
        public Color collisionActor;
        public Color collisionObj;

        public Color buttonUp;
        public Color buttonOver;
        public Color buttonDown;
    }
}