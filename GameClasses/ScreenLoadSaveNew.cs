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






        public void SetState(LoadSaveNewState State)
        {
            screenState = State;

            playerStationary = new ComponentAnimation();
            playerStationary.currentAnimation = new List<Byte4> { new Byte4(0, 0, 0, 0) };

            playerWalking = new ComponentAnimation();
            playerWalking.currentAnimation = new List<Byte4> { new Byte4(1, 0, 0, 0), new Byte4(1, 0, 1, 0) };
        }




        public ScreenLoadSaveNew()
        {
            this.name = "ScreenLoadSaveNew";
            //defaults to new
            screenState = LoadSaveNewState.New;

            //create the selectionBox
            selectionBox = new ComponentSprite(Assets.uiItemsSheet,
                new Vector2(0, 0),
                AnimationFrames.Ui_SelectionBox[0],
                new Point(16, 16));
            //create the arrow
            arrow = new ComponentSprite(Assets.uiItemsSheet,
                new Vector2(1000, 0),
                new Byte4(14, 1, 0, 0),
                new Point(16, 16));
            arrow.rotation = Rotation.Clockwise90;
            //create action text
            actionText = new ComponentText(Assets.font, "",
                new Vector2(0, 0), ColorScheme.textDark);


            #region Create & set window and dividers

            window = new MenuWindow(
                new Point(16 * 13 + 8 + 4, 16 * 6),
                new Point(16 * 12 + 8, 16 * 11), "Default ");
            for (i = 0; i < 5; i++)
            {
                window.lines.Add(new MenuRectangle(new Point(0, 0),
                    new Point(0, 0), ColorScheme.windowInset));
            }
            window.lines[2].position.Y = window.background.position.Y + 16 * 2;
            window.lines[3].position.Y = window.background.position.Y + 16 * 4;
            window.lines[4].position.Y = window.background.position.Y + 16 * 5;
            window.lines[5].position.Y = window.background.position.Y + 16 * 7;
            window.lines[6].position.Y = window.background.position.Y + 16 * 8;

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
            texts.Add(new ComponentText(Assets.font, "Game 1",
                new Vector2(window.background.rec.X + 8, window.background.rec.Y + 16 * 1 + 1),
                ColorScheme.textDark));
            texts.Add(new ComponentText(Assets.font, "Game 2",
                new Vector2(window.background.rec.X + 8, window.background.rec.Y + 16 * 4 + 1),
                ColorScheme.textDark));
            texts.Add(new ComponentText(Assets.font, "Game 3",
                new Vector2(window.background.rec.X + 8, window.background.rec.Y + 16 * 7 + 1),
                ColorScheme.textDark));

            #endregion


            #region Setup Game Player Sprites + Animations

            game1.hero.position = game1.menuItem.compSprite.position;
            game2.hero.position = game2.menuItem.compSprite.position;
            game3.hero.position = game3.menuItem.compSprite.position;

            #endregion


            #region Place Game Texts

            game1.timeDateText.position = game1.menuItem.compSprite.position + new Vector2(16, -12);
            game2.timeDateText.position = game2.menuItem.compSprite.position + new Vector2(16, -12);
            game3.timeDateText.position = game3.menuItem.compSprite.position + new Vector2(16, -12);

            #endregion


            #region Setup Quest Item 

            //place last collected quest item for all 3 games
            //setup the last collected quest item
            game1.lastStoryItem.compSprite.position.X = game1.menuItem.compSprite.position.X + 16 * 9 - 3;
            game1.lastStoryItem.compSprite.position.Y = game1.menuItem.compSprite.position.Y;

            game2.lastStoryItem.compSprite.position.X = game2.menuItem.compSprite.position.X + 16 * 9 - 3;
            game2.lastStoryItem.compSprite.position.Y = game2.menuItem.compSprite.position.Y;

            game3.lastStoryItem.compSprite.position.X = game3.menuItem.compSprite.position.X + 16 * 9 - 3;
            game3.lastStoryItem.compSprite.position.Y = game3.menuItem.compSprite.position.Y;

            #endregion


        }

        public override void Open()
        {
            background.alpha = 0.0f;
            background.fadeInSpeed = 0.03f;
            background.maxAlpha = 0.8f;

            Functions_MenuWindow.ResetAndMove(window,
                16 * 13 + 8 + 4, 16 * 6,
                new Point(16 * 12 + 8, 16 * 11), "Default");

            //reset game names
            texts[0].text = "Game 1";
            texts[1].text = "Game 2";
            texts[2].text = "Game 3";

            //Determine any beaten games - alters game's title
            CheckForBeatenGame(PlayerData.game1, texts[0]);
            CheckForBeatenGame(PlayerData.game2, texts[1]);
            CheckForBeatenGame(PlayerData.game3, texts[2]);

            //populate each game display
            PopulateDisplay(PlayerData.game1, game1);
            PopulateDisplay(PlayerData.game2, game2);
            PopulateDisplay(PlayerData.game3, game3);


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

        public override void Close()
        {
            //assume we loaded or created or saved gamedata
            //these operations set PlayerData.current to game1, 2, or 3.
            //we need to sync this current data to the hero.
            
            //set the hero actor's type - he can be a blob
            Functions_Actor.SetType(Pool.hero, PlayerData.current.actorType);
            //load hero's items, weapons, equipment from current gamedata
            Functions_Hero.SetLoadout();
            //load his last location too
            LevelSet.field.ID = PlayerData.current.lastLocation;
        }

        public override void HandleInput(GameTime GameTime)
        {
            if (displayState == DisplayState.Opened)
            {

                #region Exit Screen

                if(Input.Player1.B & Input.Player1.B_Prev == false)
                {   //upon B button press, exit this screen
                    //close window, play and show closing
                    Assets.Play(Assets.sfxWindowClose);
                    displayState = DisplayState.Closing;
                    Functions_MenuWindow.Close(window);
                }

                #endregion


                #region Handle load/save/new

                //only allow input if the screen has opened completely
                else if (
                    (Input.Player1.A & Input.Player1.A_Prev == false)
                    ||
                    (Input.Player1.Start & Input.Player1.Start_Prev == false)
                    )
                {

                    #region Load

                    if (screenState == LoadSaveNewState.Load)
                    {   //load playerData
                        if (currentlySelected == game1.menuItem)
                        { Functions_Backend.LoadGame(GameFile.Game1, true); }
                        else if (currentlySelected == game2.menuItem)
                        { Functions_Backend.LoadGame(GameFile.Game2, true); }
                        else if (currentlySelected == game3.menuItem)
                        { Functions_Backend.LoadGame(GameFile.Game3, true); }
                    }

                    #endregion



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
                        {
                            PopulateDisplay(PlayerData.current, game1);
                            Functions_Backend.SaveGame(GameFile.Game1);
                        }
                        else if (currentlySelected == game2.menuItem)
                        {
                            PopulateDisplay(PlayerData.current, game2);
                            Functions_Backend.SaveGame(GameFile.Game2);
                        }
                        else if (currentlySelected == game3.menuItem)
                        {
                            PopulateDisplay(PlayerData.current, game3);
                            Functions_Backend.SaveGame(GameFile.Game3);
                        }
                        //create dialog screen, let player know file has been created or saved
                        if (screenState == LoadSaveNewState.New)
                        {
                            Screens.Dialog.SetDialog(AssetsDialog.GameCreated);
                            ScreenManager.AddScreen(Screens.Dialog);
                        }
                        else
                        {
                            Screens.Dialog.SetDialog(AssetsDialog.GameSaved);
                            ScreenManager.AddScreen(Screens.Dialog);
                        }
                    }
                    //save current game to autoSave file (sets autosave)
                    Functions_Backend.SaveGame(GameFile.AutoSave);
                    Assets.Play(Assets.sfxSelectFile);
                }

                #endregion


                #region Move between MenuItems

                //get the previouslySelected menuItem
                previouslySelected = currentlySelected;

                //prevent rapid scrolling
                if (Input.Player1.direction != Input.Player1.direction_Prev)
                {
                    //this is a new direction, allow movement between menuItems
                    if (Input.Player1.direction == Direction.Right)
                    { currentlySelected = currentlySelected.neighborRight; }
                    else if (Input.Player1.direction == Direction.Left)
                    { currentlySelected = currentlySelected.neighborLeft; }
                    else if (Input.Player1.direction == Direction.Down)
                    { currentlySelected = currentlySelected.neighborDown; }
                    else if (Input.Player1.direction == Direction.Up)
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




        public void PopulateDisplay(SaveData saveData, GameDisplayData displayData)
        {
            //set time of game
            //displayData.timeDateText.text = "time: " + saveData.timeSpan.ToString(@"hh\:mm\:ss");
            //ok, let's try that again, .Net
            displayData.timeDateText.text = "time: ";
            displayData.timeDateText.text += @"" + saveData.hours + ":" + saveData.mins + ":" + saveData.secs;

            //set date of game
            //displayData.timeDateText.text += "\ndate: " + saveData.dateTime.ToString("yyyy.m.d");
            //let's try that again, .Net
            displayData.timeDateText.text += "\ndate: ";
            displayData.timeDateText.text += "" + saveData.dateTime.Year;
            displayData.timeDateText.text += "." + saveData.dateTime.Month;
            displayData.timeDateText.text += "." + saveData.dateTime.Day;

            //set the hero texture based on saveData.actorType
            if (saveData.actorType == ActorType.Hero)
            { displayData.hero.texture = Assets.heroSheet; }
            else if (saveData.actorType == ActorType.Blob)
            { displayData.hero.texture = Assets.blobSheet; }

            
        }




        public void CheckForBeatenGame(SaveData saveData, ComponentText gameTitle)
        {   //note if player has beaten the game so far
            if (
                saveData.story_forestDungeon &
                saveData.story_mountainDungeon &
                saveData.story_swampDungeon
                )
            {   //all dungeons have been beaten - note this
                gameTitle.text += @" - beaten! ";
            }
        }








    }
}