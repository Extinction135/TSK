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
    public class ScreenRoomBuilder : Screen
    {

        public static MenuWindow window;



        public ScreenRoomBuilder() { this.name = "Room Builder Screen"; }

        public override void LoadContent()
        {
            window = new MenuWindow(
                new Point(8, 16 * 4),
                new Point(16 * 6, 16 * 14 + 8), 
                "Room Builder");
        }

        public override void HandleInput(GameTime GameTime)
        {
            Functions_Debug.HandleDebugMenuInput();
        }

        public override void Update(GameTime GameTime)
        {
            window.Update();
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            Functions_Draw.Draw(window);

            Functions_Draw.DrawDebugMenu();

            ScreenManager.spriteBatch.End();
        }

    }
}