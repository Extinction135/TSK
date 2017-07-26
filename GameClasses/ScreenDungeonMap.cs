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
    public class ScreenDungeonMap : Screen
    {
        ScreenRec background = new ScreenRec();
        public static MenuWindow window;
        public Rectangle dungeonBkg;
        //public List<Rectangle> rooms;
        public int i;



        public ScreenDungeonMap() { this.name = "DungeonMapScreen"; }

        public override void LoadContent()
        {
            background.alpha = 0.0f;
            background.maxAlpha = 0.5f;
            
            window = new MenuWindow(
                new Point( //place map underneath the worldUI hud
                    (int)WorldUI.currentWeapon.compSprite.position.X,
                    (int)WorldUI.currentWeapon.compSprite.position.Y + 22),
                new Point(16 * 8, 16 * 9 + 8), "Dungeon Map");
            dungeonBkg = new Rectangle(
                window.background.position.X + 8,
                window.background.position.Y + 20,
                16 * 7, 16 * 7);

            //open the screen
            displayState = DisplayState.Opening;
            Assets.Play(Assets.sfxMapOpen);
        }

        public override void HandleInput(GameTime GameTime)
        {
            if (displayState == DisplayState.Opened)
            {   //only allow input if the screen has opened completely
                if (Functions_Input.IsNewButtonPress(Buttons.Start) ||
                    Functions_Input.IsNewButtonPress(Buttons.Back) ||
                    Functions_Input.IsNewButtonPress(Buttons.B))
                {
                    Assets.Play(Assets.sfxInventoryClose); //play closing sfx
                    Functions_MenuWindow.Close(window);
                    displayState = DisplayState.Closing; //begin closing the screen
                }
            }
        }

        public override void Update(GameTime GameTime)
        {

            #region Handle Screen State

            if (displayState == DisplayState.Opening)
            {
                //fade background in
                background.fadeState = FadeState.FadeIn;
                Functions_ScreenRec.Fade(background);
                if (background.fadeState == FadeState.FadeComplete)
                { displayState = DisplayState.Opened; }
            }
            else if (displayState == DisplayState.Closing)
            {
                //fade background out
                background.fadeState = FadeState.FadeOut;
                Functions_ScreenRec.Fade(background);
                if (background.fadeState == FadeState.FadeComplete)
                { displayState = DisplayState.Closed; }
            }
            else if (displayState == DisplayState.Closed)
            { ScreenManager.RemoveScreen(this); }

            #endregion


            Functions_MenuWindow.Update(window);
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            Functions_Draw.Draw(background);
            Functions_Draw.Draw(window);
            if (window.interior.displayState == DisplayState.Opened)
            {   //draw dungeon background
                ScreenManager.spriteBatch.Draw(
                    Assets.dummyTexture, dungeonBkg,
                    Assets.colorScheme.windowInset);

                //draw room recs

            }
            ScreenManager.spriteBatch.End();
        }
    }
}
