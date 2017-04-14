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
    public class DungeonScreen : Screen
    {
        public DungeonScreen() { this.name = "DungeonScreen"; }

        public override void LoadContent()
        {
            Pool.Initialize(this);
            DungeonGenerator.BuildRoom();
            //ActorFunctions.SetType(Pool.hero, Actor.Type.Blob);
        }

        public override void HandleInput(GameTime GameTime)
        {
            Input.MapPlayerInput(Pool.hero.compInput);

            if (Flags.Debug)
            {   //if the game is in debug mode, dump info on clicked actor/obj
                if (Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
                { DebugFunctions.Inspect(this); }
                //toggle the paused boolean
                if (Input.IsNewKeyPress(Keys.Space))
                {
                    if (Flags.Paused) { Flags.Paused = false; }
                    else { Flags.Paused = true; }
                }
                DebugMenu.HandleInput();
            }
        }

        public override void Update(GameTime GameTime)
        {
            Timing.Reset();

            if(!Flags.Paused) //update and move actors, objects, and projectiles
            { PoolFunctions.Update(this); }
            WorldUI.Update();

            //track camera to hero
            Camera2D.targetPosition = Pool.hero.compSprite.position;
            Camera2D.Update(GameTime);

            Timing.stopWatch.Stop();
            Timing.updateTime = Timing.stopWatch.Elapsed;
        }

        public override void Draw(GameTime GameTime)
        {
            Timing.Reset();

            //draw gameworld
            ScreenManager.spriteBatch.Begin(
                        SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend,
                        SamplerState.PointClamp,
                        null,
                        null,
                        null,
                        Camera2D.view
                        );
            PoolFunctions.Draw();
            if (Flags.DrawCollisions) { DrawFunctions.Draw(Input.cursorColl); }
            ScreenManager.spriteBatch.End();

            //draw UI, debug info + debug menu
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            WorldUI.Draw();
            if (Flags.Debug) { DebugInfo.Draw(); DebugMenu.Draw(); }
            ScreenManager.spriteBatch.End();

            Timing.stopWatch.Stop();
            Timing.drawTime = Timing.stopWatch.Elapsed;
            Timing.totalTime = Timing.updateTime + Timing.drawTime;
        }
    }
}