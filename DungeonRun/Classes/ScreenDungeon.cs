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

        public Camera2D camera;



        public ScreenDungeon() { this.name = "DungeonScreen"; }

        public override void LoadContent()
        {
            overlay = new Rectangle(0, 0, 640, 360);
            camera = new Camera2D();

            Pool.Initialize();
            Functions_Dungeon.Initialize(this);
            Functions_Dungeon.BuildDungeon(DungeonType.Shop);
            //ActorFunctions.SetType(Pool.hero, Actor.Type.Blob);

            //open the screen
            displayState = DisplayState.Opening;
        }

        public override void HandleInput(GameTime GameTime)
        {
            if (displayState == DisplayState.Opened)
            {   //input maps only when screen is open

                #region Handle AI Input

                if (Flags.ProcessAI)
                {   //process AI for up to 3 actors per frame
                    Functions_Ai.SetActorInput();
                    Functions_Ai.SetActorInput();
                    Functions_Ai.SetActorInput();
                }

                #endregion


                #region Handle Player Input

                //map player input to hero
                Functions_Input.MapPlayerInput(Pool.hero.compInput);
                //open the inventory screen if player presses start button
                if (Functions_Input.IsNewButtonPress(Buttons.Start))
                { ScreenManager.AddScreen(new ScreenInventory()); }

                #endregion


                #region Handle Debug Keyboard / Mouse Input

                if (Flags.Debug)
                {   //if the game is in debug mode, dump info on clicked actor/obj
                    if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
                    { Functions_Debug.Inspect(); }
                    //toggle the paused boolean
                    if (Functions_Input.IsNewKeyPress(Keys.Space))
                    { if (Flags.Paused) { Flags.Paused = false; } else { Flags.Paused = true; } }
                    Functions_Debug.HandleDebugMenuInput();
                }

                #endregion

            }
            else
            {   //screen is not opened, prevent all input mapping
                Functions_Pool.ResetActorPoolInput(); //clear enemy input
                Functions_Input.ResetInputData(Pool.hero.compInput); //clear hero input
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
                    Functions_Pool.Update();
                    Functions_Collision.CheckDungeonRoomCollisions();
                    Functions_WorldUI.Update();
                    //track camera to hero
                    camera.targetPosition = Pool.hero.compSprite.position;
                    Functions_Camera2D.Update(camera, GameTime);
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
                        camera.view
                        );
            Functions_Pool.Draw();
            if (Flags.DrawCollisions)
            {
                Functions_Draw.Draw(Input.cursorColl);
                Functions_Draw.Draw(Functions_Dungeon.dungeon);
                Functions_Draw.Draw(Functions_Interaction.interactionRec);
            }
            ScreenManager.spriteBatch.End();

            //draw UI, debug info + debug menu, without camera view
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            Functions_WorldUI.Draw();
            if (Flags.Debug) { Functions_Draw.DrawDebugInfo(); Functions_Draw.DrawDebugMenu(); }
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