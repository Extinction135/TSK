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
    public abstract class Screen
    {  
        public DisplayState displayState;
        public Screen() { }
        public string name = "New";
        public virtual void LoadContent() { }
        public virtual void UnloadContent() { }
        public virtual void HandleInput(GameTime gameTime) { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime) { }
    }


    public static class Screens
    {
        //acts as global access point for all screens in game


    }
}