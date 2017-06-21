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
    public class ScreenLoadSaveNew : Screen
    {
        ScreenRec background = new ScreenRec();
        static MenuWindow window;
        LoadSaveNewState screenState;
        ExitAction exitAction;
        int i;

        public List<MenuRectangle> dividers;
        public List<ComponentText> texts;

        MenuItem game1MenuItem;
        MenuItem game2MenuItem;
        MenuItem game3MenuItem;
        //these point to a menuItem
        MenuItem currentlySelected;
        MenuItem previouslySelected;
        //simply visually tracks which menuItem is selected
        ComponentSprite selectionBox;
        ComponentSprite arrow;
        ComponentText actionText;



        public ScreenLoadSaveNew(LoadSaveNewState State)
        {
            screenState = State;
            this.name = "ScreenLoadSaveNew";
        }

        public override void LoadContent()
        {
            background.alpha = 0.0f;
            background.fadeInSpeed = 0.03f;
            window = new MenuWindow(new Point(16 * 13 + 8 + 4, 16 * 6),
                new Point(16 * 12 + 8, 16 * 11), "Default ");


            #region Setup Game1-3 MenuItems

            game1MenuItem = new MenuItem();
            game2MenuItem = new MenuItem();
            game3MenuItem = new MenuItem();
            //set menuItem sprites
            Functions_MenuItem.SetMenuItemData(MenuItemType.Unknown, game1MenuItem);
            Functions_MenuItem.SetMenuItemData(MenuItemType.Unknown, game2MenuItem);
            Functions_MenuItem.SetMenuItemData(MenuItemType.Unknown, game3MenuItem);
            //set X positions
            game1MenuItem.compSprite.position.X = window.background.rec.X + 35 + 8;
            game2MenuItem.compSprite.position.X = game1MenuItem.compSprite.position.X;
            game3MenuItem.compSprite.position.X = game1MenuItem.compSprite.position.X;
            //set Y positions
            game1MenuItem.compSprite.position.Y = window.background.rec.Y + 16 * 3;
            game2MenuItem.compSprite.position.Y = window.background.rec.Y + 16 * 6;
            game3MenuItem.compSprite.position.Y = window.background.rec.Y + 16 * 9;
            //set neighbors
            game1MenuItem.neighborDown = game2MenuItem;
            game2MenuItem.neighborDown = game3MenuItem;
            game3MenuItem.neighborUp = game2MenuItem;
            game2MenuItem.neighborUp = game1MenuItem;

            #endregion


            #region Setup dividers

            dividers = new List<MenuRectangle>();
            //game1 header and footer
            dividers.Add(new MenuRectangle(
                new Point(window.background.rec.X + 8, window.background.rec.Y + 16 * 2),
                new Point(window.size.X - 16, 1), 
                Assets.colorScheme.windowInset));
            dividers.Add(new MenuRectangle(
                new Point(window.background.rec.X + 8, window.background.rec.Y + 16 * 4),
                new Point(window.size.X - 16, 1),
                Assets.colorScheme.windowInset));
            //game2 header and footer
            dividers.Add(new MenuRectangle(
                new Point(window.background.rec.X + 8, window.background.rec.Y + 16 * 5),
                new Point(window.size.X - 16, 1),
                Assets.colorScheme.windowInset));
            dividers.Add(new MenuRectangle(
                new Point(window.background.rec.X + 8, window.background.rec.Y + 16 * 7),
                new Point(window.size.X - 16, 1),
                Assets.colorScheme.windowInset));
            //game3 header
            dividers.Add(new MenuRectangle(
                new Point(window.background.rec.X + 8, window.background.rec.Y + 16 * 8),
                new Point(window.size.X - 16, 1),
                Assets.colorScheme.windowInset));

            #endregion


            #region Setup Game Title Texts

            texts = new List<ComponentText>();
            texts.Add(new ComponentText(Assets.font, "Game 1 - test",
                new Vector2(window.background.rec.X + 8, window.background.rec.Y + 16 * 1 + 1),
                Assets.colorScheme.textDark));
            texts.Add(new ComponentText(Assets.font, "Game 2 - test",
                new Vector2(window.background.rec.X + 8, window.background.rec.Y + 16 * 4 + 1),
                Assets.colorScheme.textDark));
            texts.Add(new ComponentText(Assets.font, "Game 3 - test",
                new Vector2(window.background.rec.X + 8, window.background.rec.Y + 16 * 7 + 1),
                Assets.colorScheme.textDark));

            #endregion

            
            //set the currently selected menuItem to the first inventory menuItem
            currentlySelected = game1MenuItem;
            previouslySelected = game1MenuItem;
            //create the selectionBox
            selectionBox = new ComponentSprite(Assets.mainSheet,
                new Vector2(0, 0), new Byte4(15, 7, 0, 0),
                new Point(16, 16));
            //create the arrow
            arrow = new ComponentSprite(Assets.mainSheet,
                new Vector2(0, 0), new Byte4(15, 8, 0, 0),
                new Point(16, 16));
            arrow.rotation = Rotation.Clockwise90;
            //create action text
            actionText = new ComponentText(Assets.font, "",
                new Vector2(0, 0), Assets.colorScheme.textDark);


            #region Modify components based on screenState

            if (screenState == LoadSaveNewState.Load)
            {
                window.title.text = "Load ";
                actionText.text = "Load";
            }
            else if (screenState == LoadSaveNewState.New)
            {
                window.title.text = "New ";
                actionText.text = "New";
            }
            else if (screenState == LoadSaveNewState.Save)
            {
                window.title.text = "Save ";
                actionText.text = "Save";
            }
            window.title.text += "Game";

            #endregion


            //open the screen
            displayState = DisplayState.Opening;
        }

        public override void HandleInput(GameTime GameTime)
        {
            if (displayState == DisplayState.Opened)
            {   //exit this screen upon start or b button press
                if (Functions_Input.IsNewButtonPress(Buttons.B))
                {
                    Assets.Play(Assets.sfxInventoryClose);
                    displayState = DisplayState.Closing;
                    exitAction = ExitAction.ExitScreen;
                }

                /*
                //only allow input if the screen has opened completely
                if (Functions_Input.IsNewButtonPress(Buttons.Start) ||
                    Functions_Input.IsNewButtonPress(Buttons.A))
                {
                    currentlySelected.compSprite.scale = 2.0f;

                    //handle load/save/new

                    //handle soundEffect
                    if (currentlySelected.type == MenuItemType.OptionsQuitGame)
                    { Assets.Play(Assets.sfxInventoryClose); }
                    else { Assets.Play(Assets.sfxMenuItem); }
                }
                */

                //get the previouslySelected menuItem
                previouslySelected = currentlySelected;
                //check to see if the gamePad direction is a new direction - prevents rapid scrolling
                if (Input.gamePadDirection != Input.lastGamePadDirection)
                {   //this is a new direction, allow movement between menuItems
                    if (Input.gamePadDirection == Direction.Right)
                    { currentlySelected = currentlySelected.neighborRight; }
                    else if (Input.gamePadDirection == Direction.Left)
                    { currentlySelected = currentlySelected.neighborLeft; }
                    else if (Input.gamePadDirection == Direction.Down)
                    { currentlySelected = currentlySelected.neighborDown; }
                    else if (Input.gamePadDirection == Direction.Up)
                    { currentlySelected = currentlySelected.neighborUp; }
                    //check to see if we changed menuItems
                    if (previouslySelected != currentlySelected)
                    {
                        Assets.Play(Assets.sfxTextLetter);
                        previouslySelected.compSprite.scale = 1.0f;
                        selectionBox.scale = 2.0f;
                    }
                }
            }
        }

        public override void Update(GameTime GameTime)
        {

            #region Handle Screen State

            if (displayState == DisplayState.Opening)
            {   //fade bkg in
                background.alpha += background.fadeInSpeed;
                if (background.alpha >= 0.8f)
                {
                    background.alpha = 0.8f;
                    displayState = DisplayState.Opened;
                }
            }
            else if (displayState == DisplayState.Closing)
            {   //fade bkg out
                background.alpha -= background.fadeOutSpeed;
                if (background.alpha <= 0.0f)
                {
                    background.alpha = 0.0f;
                    displayState = DisplayState.Closed;
                }
            }
            else if (displayState == DisplayState.Closed)
            {
                if (exitAction == ExitAction.ExitScreen)
                {
                    //Functions_Backend.LoadPlayerData(); //load autoSave data
                    //ScreenManager.ExitAndLoad(new ScreenOverworld());
                    ScreenManager.RemoveScreen(this);
                }
            }

            #endregion


            //open the window + dividers
            Functions_MenuWindow.Update(window);
            for (i = 0; i < dividers.Count; i++)
            { Functions_MenuRectangle.Update(dividers[i]); }

            if (displayState != DisplayState.Opening)
            {   //pulse the selectionBox alpha
                if (selectionBox.alpha >= 1.0f) { selectionBox.alpha = 0.1f; }
                else { selectionBox.alpha += 0.025f; }
                //match the position of the selectionBox to the currently selected menuItem
                selectionBox.position = currentlySelected.compSprite.position;
                //place arrow relative to selectionBox
                arrow.position.X = selectionBox.position.X - 20 - 8;
                arrow.position.Y = selectionBox.position.Y + 3;
                //place action text relative to arrow
                actionText.position.X = arrow.position.X - 6;
                actionText.position.Y = arrow.position.Y - 16;
                //scale the selectionBox down to 1.0
                if (selectionBox.scale > 1.0f) { selectionBox.scale -= 0.07f; }
                else { selectionBox.scale = 1.0f; }
                //animate the currently selected menuItem - this scales it back down to 1.0
                Functions_Animation.Animate(currentlySelected.compAnim, currentlySelected.compSprite);
            }
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            ScreenManager.spriteBatch.Draw(Assets.dummyTexture,
                background.rec, Assets.colorScheme.overlay * background.alpha);

            //only draw screen contents if screen is opening or opened
            if (displayState == DisplayState.Opening || displayState == DisplayState.Opened)
            {
                Functions_Draw.Draw(window);
                for (i = 0; i < dividers.Count; i++) { Functions_Draw.Draw(dividers[i]); }
                if (window.interior.displayState == DisplayState.Opened)
                {
                    Functions_Draw.Draw(game1MenuItem.compSprite);
                    Functions_Draw.Draw(game2MenuItem.compSprite);
                    Functions_Draw.Draw(game3MenuItem.compSprite);
                    for (i = 0; i < texts.Count; i++) { Functions_Draw.Draw(texts[i]); }
                    Functions_Draw.Draw(selectionBox);
                    Functions_Draw.Draw(arrow);
                    Functions_Draw.Draw(actionText);
                }
            }

            ScreenManager.spriteBatch.End();
        }

    }
}