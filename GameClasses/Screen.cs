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
        public virtual void Open() { }
        public virtual void Close() { }
        public virtual void HandleInput(GameTime gameTime) { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime) { }
    }


    public static class Screens
    {
        //acts as global access point for all screens in game
        public static ScreenCheats Cheats = new ScreenCheats();
        public static ScreenDialog Dialog = new ScreenDialog();
        public static ScreenEditor Editor = new ScreenEditor();
        public static ScreenEditorMenu EditorMenu = new ScreenEditorMenu();

        //we can have multiple overworld screens in the future
        public static Overworld_ShadowKing Overworld_ShadowKing = new Overworld_ShadowKing();

        public static ScreenLevel Level = new ScreenLevel();
        public static ScreenLevelMap LevelMap = new ScreenLevelMap();
        public static ScreenInventory Inventory = new ScreenInventory();
        public static ScreenVendor Vendor = new ScreenVendor();

        public static ScreenLoadSaveNew LoadSaveNew = new ScreenLoadSaveNew();
        public static ScreenOptions Options = new ScreenOptions();
        public static ScreenSummary Summary = new ScreenSummary();
        public static ScreenTitle Title = new ScreenTitle();
    }


}