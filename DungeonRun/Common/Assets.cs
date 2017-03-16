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


        public Texture2D actorsSheet; //all actors exist on the actor's sheet
        //public Texture2D objectsSheet; //all gameObjs exist on the object's sheet
        //public Texture2D uiSheet; //all ui sprites exist on the ui sheet


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

            //soundfx
        }
    }
}