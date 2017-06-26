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

        ComponentSprite game1Player;
        ComponentSprite game2Player;
        ComponentSprite game3Player;
        ComponentAnimation playerStationary;
        ComponentAnimation playerWalking;

        ComponentText game1Text;
        ComponentText game2Text;
        ComponentText game3Text;

        List<MenuItem> game1Crystals;
        List<MenuItem> game2Crystals;
        List<MenuItem> game3Crystals;



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


            #region Setup Games 1-3 MenuItems

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


            #region Setup Game Player Sprites + Animations

            game1Player = new ComponentSprite(Assets.heroSheet,
                game1MenuItem.compSprite.position, 
                new Byte4(0, 0, 0, 0), new Point(16, 16));
            game2Player = new ComponentSprite(Assets.heroSheet,
                game2MenuItem.compSprite.position,
                new Byte4(0, 0, 0, 0), new Point(16, 16));
            game3Player = new ComponentSprite(Assets.heroSheet,
                game3MenuItem.compSprite.position,
                new Byte4(0, 0, 0, 0), new Point(16, 16));

            playerStationary = new ComponentAnimation();
            playerStationary.currentAnimation = new List<Byte4> { new Byte4(0, 0, 0, 0) };

            playerWalking = new ComponentAnimation();
            playerWalking.currentAnimation = new List<Byte4> { new Byte4(1, 0, 0, 0), new Byte4(1, 0, 1, 0) };

            #endregion


            #region Setup Game Texts

            game1Text = new ComponentText(Assets.font, "time: 12:34:56 \ndate: 01.23.4567",
                game1MenuItem.compSprite.position + new Vector2(16, -12), 
                Assets.colorScheme.textDark);
            game2Text = new ComponentText(Assets.font, "time: 12:34:56 \ndate: 01.23.4567",
                game2MenuItem.compSprite.position + new Vector2(16, -12),
                Assets.colorScheme.textDark);
            game3Text = new ComponentText(Assets.font, "time: - \ndate: -",
                game3MenuItem.compSprite.position + new Vector2(16, -12),
                Assets.colorScheme.textDark);

            #endregion


            #region Setup Games 1-3 Crystals

            game1Crystals = new List<MenuItem>();
            game2Crystals = new List<MenuItem>();
            game3Crystals = new List<MenuItem>();
            for (i = 0; i < 6; i++)
            {
                game1Crystals.Add(new MenuItem());
                game2Crystals.Add(new MenuItem());
                game3Crystals.Add(new MenuItem());
                Functions_MenuItem.SetMenuItemData(MenuItemType.CrystalEmpty, game1Crystals[i]);
                Functions_MenuItem.SetMenuItemData(MenuItemType.CrystalEmpty, game2Crystals[i]);
                Functions_MenuItem.SetMenuItemData(MenuItemType.CrystalEmpty, game3Crystals[i]);
            }

            PlaceCrystals(game1Crystals, new Vector2(window.background.rec.X + 16 * 8 + 4, window.background.rec.Y + 16 * 3));
            PlaceCrystals(game2Crystals, new Vector2(window.background.rec.X + 16 * 8 + 4, window.background.rec.Y + 16 * 6));
            PlaceCrystals(game3Crystals, new Vector2(window.background.rec.X + 16 * 8 + 4, window.background.rec.Y + 16 * 9));

            #endregion


            //create the selectionBox
            selectionBox = new ComponentSprite(Assets.mainSheet,
                new Vector2(0, 0), new Byte4(15, 7, 0, 0),
                new Point(16, 16));
            //create the arrow
            arrow = new ComponentSprite(Assets.mainSheet,
                new Vector2(1000, 0), new Byte4(15, 8, 0, 0),
                new Point(16, 16));
            arrow.rotation = Rotation.Clockwise90;
            //create action text
            actionText = new ComponentText(Assets.font, "",
                new Vector2(0, 0), Assets.colorScheme.textDark);


            #region Modify components based on screenState

            if (screenState == LoadSaveNewState.Load)
            { window.title.text = "Load "; actionText.text = "Load"; }
            else if (screenState == LoadSaveNewState.New)
            { window.title.text = "New "; actionText.text = "New"; }
            else if (screenState == LoadSaveNewState.Save)
            { window.title.text = "Save "; actionText.text = "Save"; }
            window.title.text += "Game";

            #endregion


            //set the currently selected menuItem to the first inventory menuItem
            currentlySelected = game1MenuItem;
            previouslySelected = game1MenuItem;
            //open the screen
            displayState = DisplayState.Opening;
        }



        public void PlaceCrystals(List<MenuItem> Crystals, Vector2 Pos)
        {
            Crystals[0].compSprite.position = Pos;
            Crystals[1].compSprite.position = Crystals[0].compSprite.position + new Vector2(11, 0);
            Crystals[2].compSprite.position = Crystals[1].compSprite.position + new Vector2(11, 0);
            Crystals[3].compSprite.position = Crystals[2].compSprite.position + new Vector2(11, 0);
            Crystals[4].compSprite.position = Crystals[3].compSprite.position + new Vector2(11, 0);
            Crystals[5].compSprite.position = Crystals[4].compSprite.position + new Vector2(11, 0);
        }



        public override void HandleInput(GameTime GameTime)
        {
            if (displayState == DisplayState.Opened)
            {

                #region Exit this screen upon start or b button press

                if (Functions_Input.IsNewButtonPress(Buttons.B))
                {
                    Assets.Play(Assets.sfxInventoryClose);
                    displayState = DisplayState.Closing;
                    exitAction = ExitAction.ExitScreen;
                }

                #endregion


                #region Handle load/save/new

                //only allow input if the screen has opened completely
                else if (
                    Functions_Input.IsNewButtonPress(Buttons.Start) ||
                    Functions_Input.IsNewButtonPress(Buttons.A))
                {
                    if (screenState == LoadSaveNewState.Load)
                    {
                        //load
                    }
                    else //screenState == Save or New
                    {
                        if (screenState == LoadSaveNewState.New)
                        { } //reset playerData
                        //save playerData
                        if (currentlySelected == game1MenuItem)
                        { Functions_Backend.SaveGame(GameFile.Game1); }
                        else if (currentlySelected == game2MenuItem)
                        { Functions_Backend.SaveGame(GameFile.Game2); }
                        else if (currentlySelected == game3MenuItem)
                        { Functions_Backend.SaveGame(GameFile.Game3); }
                    }
                    //handle soundEffect
                    if (currentlySelected.type == MenuItemType.OptionsQuitGame)
                    { Assets.Play(Assets.sfxInventoryClose); }
                    else { Assets.Play(Assets.sfxMenuItem); }
                    //scale up currently selected
                    currentlySelected.compSprite.scale = 2.0f;
                }

                #endregion


                #region Move between MenuItems

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

                #endregion

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
                //scale the selectionBox down to 1.0
                if (selectionBox.scale > 1.0f) { selectionBox.scale -= 0.07f; }
                else { selectionBox.scale = 1.0f; }
                //animate the currently selected menuItem - this scales it back down to 1.0
                Functions_Animation.Animate(currentlySelected.compAnim, currentlySelected.compSprite);
                //place action text relative to selection box
                actionText.position.X = selectionBox.position.X - 34;
                actionText.position.Y = selectionBox.position.Y - 13;
                //animate the arrow relative to action text
                if (arrow.position.X > actionText.position.X + 10)
                { arrow.position.X = actionText.position.X + 6; }
                else { arrow.position.X += 0.1f; }
                arrow.position.Y = actionText.position.Y + 16;


                #region Set the animation list for each game player sprite

                if (currentlySelected == game1MenuItem)
                { Functions_Animation.Animate(playerWalking, game1Player); }
                else { Functions_Animation.Animate(playerStationary, game1Player); }

                if (currentlySelected == game2MenuItem)
                { Functions_Animation.Animate(playerWalking, game2Player); }
                else { Functions_Animation.Animate(playerStationary, game2Player); }

                if (currentlySelected == game3MenuItem)
                { Functions_Animation.Animate(playerWalking, game3Player); }
                else { Functions_Animation.Animate(playerStationary, game3Player); }

                #endregion


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

                    Functions_Draw.Draw(game1Player);
                    Functions_Draw.Draw(game2Player);
                    Functions_Draw.Draw(game3Player);

                    Functions_Draw.Draw(game1Text);
                    Functions_Draw.Draw(game2Text);
                    Functions_Draw.Draw(game3Text);

                    for (i = 0; i < texts.Count; i++) { Functions_Draw.Draw(texts[i]); }
                    Functions_Draw.Draw(selectionBox);
                    Functions_Draw.Draw(arrow);
                    Functions_Draw.Draw(actionText);

                    for (i = 0; i < game1Crystals.Count; i++)
                    {
                        Functions_Draw.Draw(game1Crystals[i].compSprite);
                        Functions_Draw.Draw(game2Crystals[i].compSprite);
                        Functions_Draw.Draw(game3Crystals[i].compSprite);
                    }
                }
            }

            ScreenManager.spriteBatch.End();
        }

    }
}