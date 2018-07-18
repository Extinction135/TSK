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
    public class ScreenOptions : Screen
    {
        int i;
        MenuWindow window;
        ScreenRec background = new ScreenRec();

        //the list of menuItems and texts for cheats
        public List<ComponentText> labels;
        public List<MenuItem> menuItems;

        //these point to a menuItem
        public MenuItem currentlySelected;
        public MenuItem previouslySelected;
        //simply visually tracks which menuItem is selected
        public ComponentSprite selectionBox;

     

        public ScreenOptions()
        {
            this.name = "Options Screen";

            #region Create Window, Labels, MenuItems

            //create the bkg window
            window = new MenuWindow(
                new Point(16 * 8 - 8, 16 * 4),
                new Point(16 * 18, 16 * 14 + 8),
                "Options");

            int columns = 8;
            int rows = 5;

            //create menuitem labels
            labels = new List<ComponentText>();
            for (i = 0; i < columns * rows; i++)
            {
                labels.Add(new ComponentText(Assets.font,
                  "...\n...", new Vector2(-100, -100),
                  Assets.colorScheme.textDark));
            }

            //create menuitems
            menuItems = new List<MenuItem>();
            for (i = 0; i < columns * rows; i++) { menuItems.Add(new MenuItem()); }

            #endregion


            #region Set MenuItems & Labels

            //position and set the neighbors
            Functions_MenuItem.PlaceMenuItems(menuItems,
                window.interior.rec.X + 8 + 5,
                window.interior.rec.Y + 16 + 8 + 5,
                (byte)rows, 16 * 3 + 8, 24);

            //this is how to set the menuItems, once they're ready for options screen
            labels[0].text = "draw\ninput";
            Functions_MenuItem.SetType(MenuItemType.Options_DrawInput, menuItems[0]);

            labels[1].text = "trak\ncam";
            Functions_MenuItem.SetType(MenuItemType.Options_TrackCamera, menuItems[1]);

            labels[2].text = "watr\nmark";
            Functions_MenuItem.SetType(MenuItemType.Options_Watermark, menuItems[2]);

            labels[3].text = "hard\nmode";
            Functions_MenuItem.SetType(MenuItemType.Options_HardMode, menuItems[3]);

            labels[4].text = "draw\ntime";
            Functions_MenuItem.SetType(MenuItemType.Options_DrawBuildTimes, menuItems[4]);

            labels[5].text = "play\nmusc";
            Functions_MenuItem.SetType(MenuItemType.Options_PlayMusic, menuItems[5]);

            labels[6].text = "draw\ndbug";
            Functions_MenuItem.SetType(MenuItemType.Options_DrawDebug, menuItems[6]);

            labels[7].text = "hit\nboxs";
            Functions_MenuItem.SetType(MenuItemType.Options_DrawHitBoxes, menuItems[7]);

            #endregion


            #region Finish Up, prep for screen opening

            //set labels relative to menuItems
            for (i = 0; i < labels.Count; i++)
            {
                labels[i].position.X = menuItems[i].compSprite.position.X + 12;
                labels[i].position.Y = menuItems[i].compSprite.position.Y - 12;
            }

            //set the cheat menuItem sprites based on their relative booleans
            SetCheatMenuItems();

            //create the selectionBox
            selectionBox = new ComponentSprite(Assets.uiItemsSheet,
                new Vector2(0, 0),
                AnimationFrames.Ui_SelectionBox[0],
                new Point(16, 16));

            #endregion

        }

        public override void Open()
        {
            //setup the screen
            background.alpha = 0.0f;
            background.fadeInSpeed = 0.03f;
            background.fadeOutSpeed = 0.07f;
            background.maxAlpha = 0.7f;
            displayState = DisplayState.Opening;

            //set the currently selected menuItem to the first inventory menuItem
            currentlySelected = menuItems[0];
            previouslySelected = menuItems[0];
            Widgets.Info.Display(currentlySelected);

            //play the opening soundFX
            Assets.Play(Assets.sfxWindowOpen);
        }

        public override void HandleInput(GameTime GameTime)
        {   //prevent all input until the screen has opened
            if (displayState != DisplayState.Opened) { return; }
            base.HandleInput(GameTime);


            //Handle A Button Input
            if (Functions_Input.IsNewButtonPress(Buttons.A))
            {

                #region Handle Effects

                if (currentlySelected.type == MenuItemType.Options_DrawInput)
                {
                    if (Flags.DrawInput) { Flags.DrawInput = false; }
                    else { Flags.DrawInput = true; }
                }
                else if (currentlySelected.type == MenuItemType.Options_TrackCamera)
                {
                    if (Flags.CameraTracksHero) { Flags.CameraTracksHero = false; }
                    else { Flags.CameraTracksHero = true; }
                }
                else if (currentlySelected.type == MenuItemType.Options_Watermark)
                {
                    if (Flags.DrawWatermark) { Flags.DrawWatermark = false; }
                    else { Flags.DrawWatermark = true; }
                }
                else if (currentlySelected.type == MenuItemType.Options_HardMode)
                {
                    if (Flags.HardMode) { Flags.HardMode = false; }
                    else { Flags.HardMode = true; }
                }
                else if (currentlySelected.type == MenuItemType.Options_DrawBuildTimes)
                {
                    if (Flags.DrawUItime) { Flags.DrawUItime = false; }
                    else { Flags.DrawUItime = true; }
                }
                else if (currentlySelected.type == MenuItemType.Options_PlayMusic)
                {
                    if (Flags.PlayMusic) { Flags.PlayMusic = false; }
                    else { Flags.PlayMusic = true; }
                }
                else if (currentlySelected.type == MenuItemType.Options_DrawDebug)
                {
                    if (Flags.EnableDebugInfo) { Flags.EnableDebugInfo = false; }
                    else { Flags.EnableDebugInfo = true; }
                }
                else if (currentlySelected.type == MenuItemType.Options_DrawHitBoxes)
                {
                    if (Flags.DrawCollisions) { Flags.DrawCollisions = false; }
                    else { Flags.DrawCollisions = true; }
                }

                #endregion


                //we will always have a menuItem selected
                Assets.Play(Assets.sfxMenuItem);
                SetCheatMenuItems();

                if (Flags.PrintOutput)
                { Debug.WriteLine("menuItem pressed: " + currentlySelected.type); }
            }


            #region Exit Screen Input

            //exit this screen upon start / b button press
            else if (Functions_Input.IsNewButtonPress(Buttons.Start)
                || Functions_Input.IsNewButtonPress(Buttons.B))
            {
                Assets.Play(Assets.sfxWindowClose);
                displayState = DisplayState.Closing;
                Functions_MenuWindow.Close(window);
            }

            #endregion


            #region Move Between MenuItems Input

            //get the previouslySelected menuItem
            previouslySelected = currentlySelected;
            //check to see if the gamePad direction is a new direction - prevents rapid scrolling
            if (Input.gamePadDirection != Input.lastGamePadDirection)
            {
                //this is a new direction, allow movement between menuItems
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
                    Widgets.Info.Display(currentlySelected);
                    Assets.Play(Assets.sfxTextLetter);
                    previouslySelected.compSprite.scale = 1.0f;
                    selectionBox.scale = 2.0f;
                }
            }

            #endregion


        }

        public override void Update(GameTime GameTime)
        {
            base.Update(GameTime);

            //update the window, and set screen's display state relative
            Functions_MenuWindow.Update(window);


            #region Handle Display State

            if (displayState == DisplayState.Opening)
            {   //fade background in
                background.fadeState = FadeState.FadeIn;
                Functions_ScreenRec.Fade(background);
                if (background.fadeState == FadeState.FadeComplete)
                {
                    displayState = DisplayState.Opened;
                    Assets.Play(Assets.sfxTextLetter);
                }
                selectionBox.scale = 2.0f;
            }
            else if (displayState == DisplayState.Closing)
            {   //fade background out
                background.fadeState = FadeState.FadeOut;
                Functions_ScreenRec.Fade(background);
                if (background.fadeState == FadeState.FadeComplete)
                { displayState = DisplayState.Closed; }
            }
            else if (displayState == DisplayState.Closed)
            {   //overlay has faded in 100%
                ScreenManager.RemoveScreen(this);
            }

            #endregion


            #region Handle SelectionBox

            //pulse the selectionBox alpha
            if (selectionBox.alpha >= 1.0f) { selectionBox.alpha = 0.1f; }
            else { selectionBox.alpha += 0.025f; }
            //match the position of the selectionBox to the currently selected menuItem
            selectionBox.position = currentlySelected.compSprite.position;
            //scale the selectionBox down to 1.0
            if (selectionBox.scale > 1.0f) { selectionBox.scale -= 0.07f; }
            else { selectionBox.scale = 1.0f; }

            #endregion

        }

        public override void Draw(GameTime GameTime)
        {
            base.Draw(GameTime);
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            //draw stuff
            Functions_Draw.Draw(background);

            Functions_Draw.Draw(window);
            //only draw the selection box if the screen has opened completely
            if (displayState == DisplayState.Opened)
            {
                for (i = 0; i < menuItems.Count; i++)
                { Functions_Draw.Draw(menuItems[i].compSprite); }
                for (i = 0; i < labels.Count; i++)
                { Functions_Draw.Draw(labels[i]); }

                Functions_Draw.Draw(selectionBox);
            }
            Widgets.Info.Draw();
            ScreenManager.spriteBatch.End();
        }



        void SetCheatMenuItems() //this sets the X next to the option
        {   //reset all menuItems to unknown state
            for (i = 0; i < menuItems.Count; i++)
            { menuItems[i].compSprite.currentFrame = AnimationFrames.Ui_MenuItem_CheatOff[0]; }

            //set menuItem based on boolean
            if (Flags.DrawInput)
            { menuItems[0].compSprite.currentFrame = AnimationFrames.Ui_MenuItem_CheatOn[0]; }

            if (Flags.CameraTracksHero)
            { menuItems[1].compSprite.currentFrame = AnimationFrames.Ui_MenuItem_CheatOn[0]; }

            if (Flags.DrawWatermark)
            { menuItems[2].compSprite.currentFrame = AnimationFrames.Ui_MenuItem_CheatOn[0]; }

            if (Flags.HardMode)
            { menuItems[3].compSprite.currentFrame = AnimationFrames.Ui_MenuItem_CheatOn[0]; }

            if (Flags.DrawUItime)
            { menuItems[4].compSprite.currentFrame = AnimationFrames.Ui_MenuItem_CheatOn[0]; }

            if (Flags.PlayMusic)
            { menuItems[5].compSprite.currentFrame = AnimationFrames.Ui_MenuItem_CheatOn[0]; }

            if (Flags.EnableDebugInfo)
            { menuItems[6].compSprite.currentFrame = AnimationFrames.Ui_MenuItem_CheatOn[0]; }

            if (Flags.DrawCollisions)
            { menuItems[7].compSprite.currentFrame = AnimationFrames.Ui_MenuItem_CheatOn[0]; }

            //expand this to include additional options
        }

    }
}