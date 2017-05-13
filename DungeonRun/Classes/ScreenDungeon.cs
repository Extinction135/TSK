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
    public class ScreenDungeon : Screen
    {

        //the foreground black rectangle, overlays and hides screen content
        Rectangle overlay; 
        public float overlayAlpha = 1.0f;
        float fadeInSpeed; //this is set in Update()
        float fadeOutSpeed = 0.025f;
        //what happens when this screen exits?
        public ExitAction exitAction = ExitAction.None;



        public ScreenDungeon() { this.name = "DungeonScreen"; }

        public override void LoadContent()
        {
            overlay = new Rectangle(0, 0, 640, 360);

            Pool.Initialize();
            Functions_Dungeon.Initialize(this);
            Functions_Dungeon.BuildDungeon(DungeonType.Shop);
            //ActorFunctions.SetType(Pool.hero, Actor.Type.Blob);

            //open the screen
            displayState = DisplayState.Opening;
        }

        public override void HandleInput(GameTime GameTime)
        {
            //if screen is playing, allow input for player + active actor
            if (displayState == DisplayState.Opened) 
            {
                //reset the input for hero, map player input to hero
                Input.ResetInputData(Pool.hero.compInput);
                Input.MapPlayerInput(Pool.hero.compInput);
                Functions_Ai.SetActorInput(); //set AI for actor
                Functions_Ai.SetActorInput(); //set AI for actor
                Functions_Ai.SetActorInput(); //set AI for actor

                //open the inventory screen if player presses start button
                if (Input.IsNewButtonPress(Buttons.Start))
                { ScreenManager.AddScreen(new ScreenInventory()); }
            }
            else//dungeonScreen is not in playing state..
            {   //reset all input for hero + actors
                Input.ResetInputData(Pool.hero.compInput);
                PoolFunctions.ResetActorPoolInput();
            }

            if (Flags.Debug)
            {   //if the game is in debug mode, dump info on clicked actor/obj
                if (Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
                { Functions_Debug.Inspect(); }
                //toggle the paused boolean
                if (Input.IsNewKeyPress(Keys.Space))
                { if (Flags.Paused) { Flags.Paused = false; } else { Flags.Paused = true; } }
                DebugMenu.HandleInput();
            }
        }

        public override void Update(GameTime GameTime)
        {
            Timing.Reset();

            if(!Flags.Paused)
            {

                #region Handle Screen State

                if(displayState == DisplayState.Opening) //fade overlay to 0
                {
                    overlayAlpha -= fadeOutSpeed;
                    if (overlayAlpha <= 0.0f)
                    {
                        overlayAlpha = 0.0f;
                        displayState = DisplayState.Opened;
                    }
                }
                else if (displayState == DisplayState.Opened)
                {   //set overlay alpha to a negative value
                    overlayAlpha = -2.0f; //delays closing state a bit
                }   //delay gives player time to understand what's happening
                else if (displayState == DisplayState.Closing) //fade overlay to 1.0
                {
                    //set the fadeInSpeed & overlaayAlpha based on the exitAction
                    if (exitAction == ExitAction.Overworld)
                    {   //exits fade in immediately, and much faster
                        fadeInSpeed = 0.05f;
                        if (overlayAlpha < 0.0f) { overlayAlpha = 0.0f; }
                    }
                    else { fadeInSpeed = 0.015f; } //victory/defeat fades in much slower

                    overlayAlpha += fadeInSpeed;
                    if (overlayAlpha >= 1.0f)
                    {
                        overlayAlpha = 1.0f;
                        displayState = DisplayState.Closed;
                    }
                }
                else if (displayState == DisplayState.Closed)
                {
                    if (exitAction == ExitAction.Summary)
                    {
                        DungeonRecord.timer.Stop();
                        ScreenManager.AddScreen(new ScreenSummary());
                        exitAction = ExitAction.None;
                    }
                    else if(exitAction == ExitAction.Overworld)
                    {
                        DungeonRecord.timer.Stop();
                        ScreenManager.AddScreen(new ScreenOverworld());
                        exitAction = ExitAction.None;
                    }
                    else { } //wait for call to generate a new dungeon
                }

                #endregion


                //if the screen is closed, don't play any sounds or do any work
                if (displayState != DisplayState.Closed)
                {   //update and move actors, objects, projectiles, and camera
                    PoolFunctions.Update();
                    Functions_Collision.CheckDungeonRoomCollisions();
                    WorldUI.Update();
                    //track camera to hero
                    Camera2D.targetPosition = Pool.hero.compSprite.position;
                    Camera2D.Update(GameTime);
                }
            }

            Timing.stopWatch.Stop();
            Timing.updateTime = Timing.stopWatch.Elapsed;
        }

        public override void Draw(GameTime GameTime)
        {
            Timing.Reset();

            //draw gameworld from camera's view
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
            if (Flags.DrawCollisions)
            {
                Functions_Draw.Draw(Input.cursorColl);
                Functions_Draw.Draw(Functions_Dungeon.dungeon);
                Functions_Draw.Draw(Functions_Interaction.interactionRec);
            }
            ScreenManager.spriteBatch.End();

            //draw UI, debug info + debug menu
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            WorldUI.Draw();
            if (Flags.Debug) { DebugInfo.Draw(); DebugMenu.Draw(); }
            //draw the overlay rec last
            ScreenManager.spriteBatch.Draw( Assets.dummyTexture, 
                overlay, Assets.colorScheme.overlay * overlayAlpha);
            ScreenManager.spriteBatch.End();

            Timing.stopWatch.Stop();
            Timing.drawTime = Timing.stopWatch.Elapsed;
            Timing.totalTime = Timing.updateTime + Timing.drawTime;
        }

    }
}