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
        //ExitAction exitAction;
        int i;
        //contains all text components used
        public List<ComponentText> texts;
        //animations used in game data display
        ComponentAnimation playerStationary;
        ComponentAnimation playerWalking;
        //create the game display data
        GameDisplayData game1 = new GameDisplayData();
        GameDisplayData game2 = new GameDisplayData();
        GameDisplayData game3 = new GameDisplayData();
        //these point to a GameDisplayData.menuItem above
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
            background.maxAlpha = 0.8f;


            #region Create & set window and dividers

            window = new MenuWindow(
                new Point(16 * 13 + 8 + 4, 16 * 6),
                new Point(16 * 12 + 8, 16 * 11), "Default ");
            for (i = 0; i < 5; i++)
            {
                window.lines.Add(new MenuRectangle(new Point(0, 0),
                    new Point(0, 0), Assets.colorScheme.windowInset));
            }
            window.lines[2].position.Y = window.background.position.Y + 16 * 2;
            window.lines[3].position.Y = window.background.position.Y + 16 * 4;
            window.lines[4].position.Y = window.background.position.Y + 16 * 5;
            window.lines[5].position.Y = window.background.position.Y + 16 * 7;
            window.lines[6].position.Y = window.background.position.Y + 16 * 8;
            Functions_MenuWindow.ResetAndMove(window,
                16 * 13 + 8 + 4, 16 * 6,
                new Point(16 * 12 + 8, 16 * 11), "Default");

            #endregion


            #region Setup Games 1-3 MenuItems

            //set X positions
            game1.menuItem.compSprite.position.X = window.background.rec.X + 35 + 8;
            game2.menuItem.compSprite.position.X = game1.menuItem.compSprite.position.X;
            game3.menuItem.compSprite.position.X = game1.menuItem.compSprite.position.X;
            //set Y positions
            game1.menuItem.compSprite.position.Y = window.background.rec.Y + 16 * 3;
            game2.menuItem.compSprite.position.Y = window.background.rec.Y + 16 * 6;
            game3.menuItem.compSprite.position.Y = window.background.rec.Y + 16 * 9;
            //set neighbors
            game1.menuItem.neighborDown = game2.menuItem;
            game2.menuItem.neighborDown = game3.menuItem;
            game3.menuItem.neighborUp = game2.menuItem;
            game2.menuItem.neighborUp = game1.menuItem;

            #endregion


            #region Setup Game Title Texts

            texts = new List<ComponentText>();
            texts.Add(new ComponentText(Assets.font, "Game 1 - " + PlayerData.game1.name,
                new Vector2(window.background.rec.X + 8, window.background.rec.Y + 16 * 1 + 1),
                Assets.colorScheme.textDark));
            texts.Add(new ComponentText(Assets.font, "Game 2 - " + PlayerData.game2.name,
                new Vector2(window.background.rec.X + 8, window.background.rec.Y + 16 * 4 + 1),
                Assets.colorScheme.textDark));
            texts.Add(new ComponentText(Assets.font, "Game 3 - " + PlayerData.game3.name,
                new Vector2(window.background.rec.X + 8, window.background.rec.Y + 16 * 7 + 1),
                Assets.colorScheme.textDark));

            #endregion


            #region Setup Game Player Sprites + Animations

            game1.hero.position = game1.menuItem.compSprite.position;
            game2.hero.position = game2.menuItem.compSprite.position;
            game3.hero.position = game3.menuItem.compSprite.position;

            playerStationary = new ComponentAnimation();
            playerStationary.currentAnimation = new List<Byte4> { new Byte4(0, 0, 0, 0) };

            playerWalking = new ComponentAnimation();
            playerWalking.currentAnimation = new List<Byte4> { new Byte4(1, 0, 0, 0), new Byte4(1, 0, 1, 0) };

            #endregion


            #region Setup Game Texts

            game1.timeDateText.text = "time: " + PlayerData.game1.time;
            game1.timeDateText.text += "\ndate: " + PlayerData.game1.date;
            game1.timeDateText.position = game1.menuItem.compSprite.position + new Vector2(16, -12);

            game2.timeDateText.text = "time: " + PlayerData.game2.time;
            game2.timeDateText.text += "\ndate: " + PlayerData.game2.date;
            game2.timeDateText.position = game2.menuItem.compSprite.position + new Vector2(16, -12);

            game3.timeDateText.text = "time: " + PlayerData.game3.time;
            game3.timeDateText.text += "\ndate: " + PlayerData.game3.date;
            game3.timeDateText.position = game3.menuItem.compSprite.position + new Vector2(16, -12);

            #endregion


            #region Setup Games 1-3 Crystals
            
            for (i = 0; i < 6; i++)
            {
                game1.crystals.Add(new MenuItem());
                game2.crystals.Add(new MenuItem());
                game3.crystals.Add(new MenuItem());
                Functions_MenuItem.SetMenuItemData(MenuItemType.CrystalEmpty, game1.crystals[i]);
                Functions_MenuItem.SetMenuItemData(MenuItemType.CrystalEmpty, game2.crystals[i]);
                Functions_MenuItem.SetMenuItemData(MenuItemType.CrystalEmpty, game3.crystals[i]);
            }
            PlaceCrystals(game1.crystals, new Vector2(window.background.rec.X + 16 * 8 + 4, window.background.rec.Y + 16 * 3));
            PlaceCrystals(game2.crystals, new Vector2(window.background.rec.X + 16 * 8 + 4, window.background.rec.Y + 16 * 6));
            PlaceCrystals(game3.crystals, new Vector2(window.background.rec.X + 16 * 8 + 4, window.background.rec.Y + 16 * 9));
            SetCrystals(PlayerData.game1, game1.crystals);
            SetCrystals(PlayerData.game2, game2.crystals);
            SetCrystals(PlayerData.game3, game3.crystals);

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
            currentlySelected = game1.menuItem;
            previouslySelected = game1.menuItem;
            //open the screen
            displayState = DisplayState.Opening;
            Assets.Play(Assets.sfxWindowOpen);
        }

        public override void UnloadContent()
        {   //silently reload the latest game files
            Functions_Backend.LoadGame(GameFile.Game1, false);
            Functions_Backend.LoadGame(GameFile.Game2, false);
            Functions_Backend.LoadGame(GameFile.Game3, false);
        }

        public override void HandleInput(GameTime GameTime)
        {
            if (displayState == DisplayState.Opened)
            {   //exit this screen upon start or b button press
                if (Functions_Input.IsNewButtonPress(Buttons.B))
                {
                    Assets.Play(Assets.sfxWindowClose);
                    displayState = DisplayState.Closing;
                    Functions_MenuWindow.Close(window);
                }
                

                #region Handle load/save/new

                //only allow input if the screen has opened completely
                else if (
                    Functions_Input.IsNewButtonPress(Buttons.Start) ||
                    Functions_Input.IsNewButtonPress(Buttons.A))
                {
                    if (screenState == LoadSaveNewState.Load)
                    {   //load playerData
                        if (currentlySelected == game1.menuItem)
                        { Functions_Backend.LoadGame(GameFile.Game1, true); }
                        else if (currentlySelected == game2.menuItem)
                        { Functions_Backend.LoadGame(GameFile.Game2, true); }
                        else if (currentlySelected == game3.menuItem)
                        { Functions_Backend.LoadGame(GameFile.Game3, true); }
                    }
                    else//screenState == Save or New
                    {   //reset saveDatas, if screen is in 'new game' state
                        if (screenState == LoadSaveNewState.New)
                        {
                            PlayerData.current = new SaveData();
                            if (currentlySelected == game1.menuItem)
                            { PlayerData.game1 = new SaveData(); }
                            else if (currentlySelected == game2.menuItem)
                            { PlayerData.game2 = new SaveData(); }
                            else if (currentlySelected == game3.menuItem)
                            { PlayerData.game3 = new SaveData(); }
                        }
                        //save playerData
                        if (currentlySelected == game1.menuItem)
                        { Functions_Backend.SaveGame(GameFile.Game1); }
                        else if (currentlySelected == game2.menuItem)
                        { Functions_Backend.SaveGame(GameFile.Game2); }
                        else if (currentlySelected == game3.menuItem)
                        { Functions_Backend.SaveGame(GameFile.Game3); }
                        //create dialog screen, let player know file has been created or saved
                        if (screenState == LoadSaveNewState.New)
                        { ScreenManager.AddScreen(new ScreenDialog(Dialog.GameCreated)); }
                        else { ScreenManager.AddScreen(new ScreenDialog(Dialog.GameSaved)); }
                        UpdateCurrentDisplay();
                    }
                    //save current game to autoSave file (sets autosave)
                    Functions_Backend.SaveGame(GameFile.AutoSave);
                    Assets.Play(Assets.sfxSelectFile);
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
            {   //fade background in
                background.fadeState = FadeState.FadeIn;
                Functions_ScreenRec.Fade(background);
                if (background.fadeState == FadeState.FadeComplete)
                { displayState = DisplayState.Opened; }
            }
            else if (displayState == DisplayState.Closing)
            {   //fade background out
                background.fadeState = FadeState.FadeOut;
                Functions_ScreenRec.Fade(background);
                if (background.fadeState == FadeState.FadeComplete)
                { displayState = DisplayState.Closed; }
            }
            else if (displayState == DisplayState.Closed)
            {
                ScreenManager.RemoveScreen(this);
            }

            #endregion


            //open the window + dividers
            Functions_MenuWindow.Update(window);

            if (displayState != DisplayState.Opening)
            {   //pulse the selectionBox alpha
                if (selectionBox.alpha >= 1.0f) { selectionBox.alpha = 0.1f; }
                else { selectionBox.alpha += 0.025f; }
                //match the position of the selectionBox to the currently selected menuItem
                selectionBox.position = currentlySelected.compSprite.position;
                //scale the selectionBox down to 1.0
                if (selectionBox.scale > 1.0f) { selectionBox.scale -= 0.07f; }
                else { selectionBox.scale = 1.0f; }
                //place action text relative to selection box
                actionText.position.X = selectionBox.position.X - 34;
                actionText.position.Y = selectionBox.position.Y - 13;
                //animate the arrow relative to action text
                if (arrow.position.X > actionText.position.X + 10)
                { arrow.position.X = actionText.position.X + 6; }
                else { arrow.position.X += 0.1f; }
                arrow.position.Y = actionText.position.Y + 16;


                #region Set the animation list for each game player sprite

                if (currentlySelected == game1.menuItem)
                { Functions_Animation.Animate(playerWalking, game1.hero); }
                else { Functions_Animation.Animate(playerStationary, game1.hero); }

                if (currentlySelected == game2.menuItem)
                { Functions_Animation.Animate(playerWalking, game2.hero); }
                else { Functions_Animation.Animate(playerStationary, game2.hero); }

                if (currentlySelected == game3.menuItem)
                { Functions_Animation.Animate(playerWalking, game3.hero); }
                else { Functions_Animation.Animate(playerStationary, game3.hero); }

                #endregion


            }
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            Functions_Draw.Draw(background);
            Functions_Draw.Draw(window);
            if (window.interior.displayState == DisplayState.Opened)
            {
                for (i = 0; i < texts.Count; i++) { Functions_Draw.Draw(texts[i]); }
                Functions_Draw.Draw(game1);
                Functions_Draw.Draw(game2);
                Functions_Draw.Draw(game3);
                Functions_Draw.Draw(selectionBox);
                Functions_Draw.Draw(arrow);
                Functions_Draw.Draw(actionText);
            }
            ScreenManager.spriteBatch.End();
        }



        public void UpdateCurrentDisplay()
        {
            if (currentlySelected == game1.menuItem)
            {
                texts[0].text = "Game 1 - " + PlayerData.current.name;
                game1.timeDateText.text = "time: " + PlayerData.current.time;
                game1.timeDateText.text += "\ndate: " + PlayerData.current.date;
                SetCrystals(PlayerData.current, game1.crystals);
            }
            else if (currentlySelected == game2.menuItem)
            {
                texts[1].text = "Game 2 - " + PlayerData.current.name;
                game2.timeDateText.text = "time: " + PlayerData.current.time;
                game2.timeDateText.text += "\ndate: " + PlayerData.current.date;
                SetCrystals(PlayerData.current, game2.crystals);
            }
            else if (currentlySelected == game3.menuItem)
            {
                texts[2].text = "Game 3 - " + PlayerData.current.name;
                game3.timeDateText.text = "time: " + PlayerData.current.time;
                game3.timeDateText.text += "\ndate: " + PlayerData.current.date;
                SetCrystals(PlayerData.current, game3.crystals);
            }
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

        public void SetCrystals(SaveData SaveData, List<MenuItem> Crystals)
        {
            if (SaveData.crystal1) { Functions_MenuItem.SetMenuItemData(MenuItemType.CrystalFilled, Crystals[0]); }
            else { Functions_MenuItem.SetMenuItemData(MenuItemType.CrystalEmpty, Crystals[0]); }

            if (SaveData.crystal2) { Functions_MenuItem.SetMenuItemData(MenuItemType.CrystalFilled, Crystals[1]); }
            else { Functions_MenuItem.SetMenuItemData(MenuItemType.CrystalEmpty, Crystals[1]); }

            if (SaveData.crystal3) { Functions_MenuItem.SetMenuItemData(MenuItemType.CrystalFilled, Crystals[2]); }
            else { Functions_MenuItem.SetMenuItemData(MenuItemType.CrystalEmpty, Crystals[2]); }

            if (SaveData.crystal4) { Functions_MenuItem.SetMenuItemData(MenuItemType.CrystalFilled, Crystals[3]); }
            else { Functions_MenuItem.SetMenuItemData(MenuItemType.CrystalEmpty, Crystals[3]); }

            if (SaveData.crystal5) { Functions_MenuItem.SetMenuItemData(MenuItemType.CrystalFilled, Crystals[4]); }
            else { Functions_MenuItem.SetMenuItemData(MenuItemType.CrystalEmpty, Crystals[4]); }

            if (SaveData.crystal6) { Functions_MenuItem.SetMenuItemData(MenuItemType.CrystalFilled, Crystals[5]); }
            else { Functions_MenuItem.SetMenuItemData(MenuItemType.CrystalEmpty, Crystals[5]); }
        }

    }
}