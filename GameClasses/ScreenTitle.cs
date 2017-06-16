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
    public class ScreenTitle : Screen
    {
        ScreenRec overlay = new ScreenRec();
        ComponentSprite background;
        static MenuWindow window;

        TitleAnimated leftTitle;
        TitleAnimated rightTitle;

        List<MenuItem> menuItems;

        /*
        MenuItem contiue;
        MenuItem newGame;
        MenuItem loadGame;
        MenuItem quitGame;
        MenuItem audioCtrls;
        MenuItem inputCtrls;
        MenuItem videoCtrls;
        MenuItem gameCtrls;

        MenuItem currentlySelected;
        MenuItem previouslySelected;
        ComponentSprite selectionBox;
        int i = 0;
        */



        public ScreenTitle() { this.name = "TitleScreen"; }

        public override void LoadContent()
        {
            overlay.alpha = 6.0f;

            background = new ComponentSprite(Assets.titleBkgSheet,
                new Vector2(640/2, 360/2), new Byte4(0, 0, 0, 0), new Point(640, 360));

            window = new MenuWindow(new Point(16 * 14, 16 * 15),
                new Point(16 * 12, 16 * 6), "Main Menu");

            float yPos = 200;
            leftTitle = new TitleAnimated(
                new Vector2(-150, yPos),
                new Vector2(200-35, yPos),
                TitleText.Dungeon, 8);
            rightTitle = new TitleAnimated(
                new Vector2(640+25, yPos),
                new Vector2(320+35, yPos),
                TitleText.Run, 8);

            //leftTitle.compSprite.alpha = 1.0f;
            //rightTitle.compSprite.alpha = 1.0f;

            /*
            //create the selectionBox
            selectionBox = new ComponentSprite(Assets.mainSheet,
                new Vector2(0, 0), new Byte4(15, 7, 0, 0),
                new Point(16, 16));
            */
            //create the menuItems
            menuItems = new List<MenuItem>();


            //open the screen
            displayState = DisplayState.Opening;
            //play the title music
            Functions_Music.PlayMusic(Music.Title);
        }

        public override void HandleInput(GameTime GameTime)
        {
            if (displayState == DisplayState.Opened)
            {   //only allow input if the screen has opened completely
                if (Functions_Input.IsNewButtonPress(Buttons.Start) ||
                    Functions_Input.IsNewButtonPress(Buttons.A))
                {
                    displayState = DisplayState.Closing;
                    //play the summary exit sound effect immediately
                    Assets.Play(Assets.sfxMenuItem);
                }
            }
        }

        public override void Update(GameTime GameTime)
        {

            #region Handle Screen State

            if (displayState == DisplayState.Opening)
            {   //fade overlay out
                overlay.alpha -= overlay.fadeInSpeed;
                if (overlay.alpha <= 0.0f)
                {
                    overlay.alpha = 0.0f;
                    displayState = DisplayState.Opened;
                    //Assets.Play(Assets.sfxMapOpen);
                }
            }
            else if (displayState == DisplayState.Opened)
            {   //animate titles in
                Functions_TitleAnimated.AnimateMovement(leftTitle);
                Functions_TitleAnimated.AnimateMovement(rightTitle);
                //fade titles in
                if (leftTitle.compSprite.alpha < 1.0f)
                { leftTitle.compSprite.alpha += 0.05f; }
                if (rightTitle.compSprite.alpha < 1.0f)
                { rightTitle.compSprite.alpha += 0.05f; }
                //open the window
                Functions_MenuWindow.Update(window);
            }
            else if (displayState == DisplayState.Closing)
            {   //fade overlay in
                overlay.alpha += overlay.fadeInSpeed;
                if (overlay.alpha >= 1.0f)
                {
                    overlay.alpha = 1.0f;
                    displayState = DisplayState.Closed;
                }
            }
            else if (displayState == DisplayState.Closed)
            {
                //goto the proper screen based on user input
                //only continue & quit game get here
                ScreenManager.ExitAndLoad(new ScreenTitle());
            }

            #endregion


            if (displayState != DisplayState.Opening)
            {   
                /*
                //if screen is opened, closing, or closed, pulse the selectionBox alpha
                if (selectionBox.alpha >= 1.0f) { selectionBox.alpha = 0.1f; }
                else { selectionBox.alpha += 0.025f; }
                //match the position of the selectionBox to the currently selected menuItem
                selectionBox.position = currentlySelected.compSprite.position;
                //scale the selectionBox down to 1.0
                if (selectionBox.scale > 1.0f) { selectionBox.scale -= 0.07f; }
                else { selectionBox.scale = 1.0f; }
                */
            }
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            Functions_Draw.Draw(background);
            Functions_Draw.Draw(leftTitle.compSprite);
            Functions_Draw.Draw(rightTitle.compSprite);
            Functions_Draw.Draw(window);

            if (window.interior.displayState == DisplayState.Opened)
            {
                //Functions_Draw.Draw(selectionBox);
            }

            //draw overlay last
            ScreenManager.spriteBatch.Draw(Assets.dummyTexture,
                overlay.rec, Assets.colorScheme.overlay * overlay.alpha);

            ScreenManager.spriteBatch.End();
        }

    }
}