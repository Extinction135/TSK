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

        //the sprite sheets used in the game
        public static Texture2D actorsSheet;
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

            //textures
            actorsSheet = content.Load<Texture2D>(@"ActorSheet");
            dungeonSheet = content.Load<Texture2D>(@"DungeonSheet");
            particleSheet = content.Load<Texture2D>(@"ParticlesProjectilesSheet");
            uiSheet = content.Load<Texture2D>(@"UISheet");

            //soundfx
        }
    }
}