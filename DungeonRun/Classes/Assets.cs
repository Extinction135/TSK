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
    public class Assets
    {
        ContentManager content;

        public Texture2D dummyTexture;
        public SpriteFont font;

        //the sprite sheets used in the game
        public Texture2D actorsSheet;
        public Texture2D dungeonSheet;
        public Texture2D particleSheet;
        public Texture2D uiSheet;
        public Texture2D weaponSheet;

        //soundfx

        public Assets(ContentManager ContentManager) { content = ContentManager; }
        public void Load(GraphicsDevice GraphicsDevice)
        {
            dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });

            //fonts
            font = content.Load<SpriteFont>(@"pixelFont");

            //textures
            actorsSheet = content.Load<Texture2D>(@"ActorSheet");
            dungeonSheet = content.Load<Texture2D>(@"DungeonSheet");
            particleSheet = content.Load<Texture2D>(@"ParticlesProjectilesSheet");
            uiSheet = content.Load<Texture2D>(@"UISheet");
            weaponSheet = content.Load<Texture2D>(@"WeaponsSheet");

            //soundfx
        }
    }
}