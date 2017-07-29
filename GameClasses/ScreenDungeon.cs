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
        public ScreenRec overlay = new ScreenRec();
        public ExitAction exitAction;



        public ScreenDungeon() { this.name = "DungeonScreen"; }

        public override void LoadContent()
        {
            overlay.alpha = 1.0f;
            overlay.fadeOutSpeed = 0.025f;

            Functions_Dungeon.Initialize(this);
            Functions_Dungeon.BuildDungeon();
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
                //open inventory screen if player presses start button
                if (Functions_Input.IsNewButtonPress(Buttons.Start))
                { ScreenManager.AddScreen(new ScreenInventory()); }

                //open map widget if player presses back button
                else if (Functions_Input.IsNewButtonPress(Buttons.Back))
                {
                    if (Widgets.Map.window.interior.displayState == DisplayState.Closed)
                    {
                        Widgets.Map.Reset(0, 0);
                        Assets.Play(Assets.sfxMapOpen);
                    }
                    else if(Widgets.Map.window.interior.displayState == DisplayState.Opened)
                    {
                        Functions_MenuWindow.Close(Widgets.Map.window);
                        Assets.Play(Assets.sfxInventoryClose);
                    }
                }

                #endregion


                if (Flags.EnableTopMenu) { Functions_Debug.HandleTopMenuInput(); }
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
                {   //fade overlay out
                    overlay.fadeState = FadeState.FadeOut;
                    Functions_ScreenRec.Fade(overlay);
                    if (overlay.fadeState == FadeState.FadeComplete)
                    { displayState = DisplayState.Opened; }
                }
                else if (displayState == DisplayState.Opened)
                {   //set overlay alpha to a negative value
                    overlay.alpha = -1.5f; //delays closing state a bit
                }   //delay gives player time to understand what's happening
                else if (displayState == DisplayState.Closing) //fade overlay to 1.0
                {
                    if (overlay.alpha == -1.5f) //just began fading in overlay
                    {   //events that happen when hero exits dungeon/game
                        Functions_WorldUI.DisplayAutosave();
                        Functions_MenuWindow.Close(Widgets.Map.window);
                    }
                    //set the fadeInSpeed & overlay alpha based on the exitAction
                    if (exitAction == ExitAction.Overworld)
                    {   //exits fade in immediately, and much faster
                        overlay.fadeInSpeed = 0.05f;
                        if (overlay.alpha < 0.0f) { overlay.alpha = 0.0f; }
                    }
                    else { overlay.fadeInSpeed = 0.015f; } //victory/defeat fades in much slower
                    //fade overlay in
                    overlay.fadeState = FadeState.FadeIn;
                    Functions_ScreenRec.Fade(overlay);
                    if (overlay.fadeState == FadeState.FadeComplete)
                    { displayState = DisplayState.Closed; }
                }
                else if (displayState == DisplayState.Closed)
                {
                    DungeonRecord.timer.Stop();
                    //save the player's current game progress
                    Functions_Backend.SaveGame(GameFile.AutoSave);
                    //handle exit action
                    if (exitAction == ExitAction.Summary)
                    { ScreenManager.ExitAndLoad(new ScreenSummary()); }
                    else if(exitAction == ExitAction.Overworld)
                    { ScreenManager.ExitAndLoad(new ScreenOverworld()); }
                    else if(exitAction == ExitAction.Title)
                    { ScreenManager.ExitAndLoad(new ScreenTitle()); }
                }

                #endregion


                //if the screen is closed, don't play any sounds or do any work
                if (displayState != DisplayState.Closed)
                {   //update and move actors, objects, projectiles, and camera
                    Functions_Pool.Update();
                    Functions_Collision.CheckDungeonRoomCollisions();
                    Functions_WorldUI.Update();
                    Widgets.Map.Update();
                    if (Flags.CameraTracksHero) //track camera to hero
                    {
                        Camera2D.tracks = false; //teleport follow hero
                        Camera2D.targetPosition = Pool.hero.compSprite.position;
                    }
                    else
                    {   //center camera to current room
                        Camera2D.targetPosition.X = Functions_Dungeon.currentRoom.center.X;
                        Camera2D.targetPosition.Y = Functions_Dungeon.currentRoom.center.Y + 16;
                        Camera2D.tracks = true; //move follow room to room
                    }
                    Functions_Camera2D.Update(GameTime);
                }
            }

            Timing.stopWatch.Stop();
            Timing.updateTime = Timing.stopWatch.Elapsed;
        }

        public override void Draw(GameTime GameTime)
        {
            Timing.Reset();


            #region Draw gameworld from camera's view

            ScreenManager.spriteBatch.Begin(
                        SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend,
                        SamplerState.PointClamp,
                        null, null, null, Camera2D.view);
            Functions_Pool.Draw();
            if (Flags.DrawCollisions)
            {
                Functions_Draw.Draw(Input.cursorColl);
                Functions_Draw.Draw(Functions_Dungeon.dungeon);
                Functions_Draw.Draw(Functions_Interaction.interactionRec);
            }
            ScreenManager.spriteBatch.End();

            #endregion


            #region Draw UI, debug info + debug menu, & overlay

            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            Functions_WorldUI.Draw();
            Widgets.Map.Draw();
            //draw debug menu + info
            if (Flags.EnableTopMenu) { Functions_Draw.DrawDebugMenu(); }
            if (Flags.DrawDebugInfo) { Functions_Draw.DrawDebugInfo(); }
            //draw the overlay rec last
            Functions_Draw.Draw(overlay);
            ScreenManager.spriteBatch.End();

            #endregion


            Timing.stopWatch.Stop();
            Timing.drawTime = Timing.stopWatch.Elapsed;
            Timing.totalTime = Timing.updateTime + Timing.drawTime;
        }

    }
}