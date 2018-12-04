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
    public class ScreenSpellBook : Screen
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

     

        public ScreenSpellBook()
        {
            this.name = "SpellBook Screen";


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
                  ColorScheme.textDark));
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



            //Explosive Spells
            labels[0].text = "bombos\nspell";
            Functions_MenuItem.SetType(MenuItemType.Spells_Explosive_Bombos, menuItems[0]);
            labels[5].text = "bombos\nspell";
            Functions_MenuItem.SetType(MenuItemType.Spells_Explosive_Bombos, menuItems[5]);



            //Fire Spells
            labels[1].text = "fire\nspells";
            //Functions_MenuItem.SetType(MenuItemType.Options_TrackCamera, menuItems[1]);



            //Ice Spells
            labels[2].text = "ice\nspells";
            //Functions_MenuItem.SetType(MenuItemType.Options_Watermark, menuItems[2]);



            //Wind Spells
            labels[3].text = "wind\nspells";
            //Functions_MenuItem.SetType(MenuItemType.Options_HardMode, menuItems[3]);



            //Electric Spells
            labels[4].text = "ether\nspell";
            Functions_MenuItem.SetType(MenuItemType.Spells_Lightning_Ether, menuItems[4]);
            labels[9].text = "ether\nspell";
            Functions_MenuItem.SetType(MenuItemType.Spells_Lightning_Ether, menuItems[9]);






            #endregion


            #region Finish Up, prep for screen opening

            //set labels relative to menuItems
            for (i = 0; i < labels.Count; i++)
            {
                labels[i].position.X = menuItems[i].compSprite.position.X + 12;
                labels[i].position.Y = menuItems[i].compSprite.position.Y - 12;
            }

            //set the cheat menuItem sprites based on their relative booleans
            UpdateMenuItems();

            //create the selectionBox
            selectionBox = new ComponentSprite(Assets.uiItemsSheet,
                new Vector2(0, 0),
                AnimationFrames.Ui_SelectionBox[0],
                new Point(16, 16));

            #endregion

        }

        public override void Open()
        {
            Functions_MenuWindow.ResetAndMove(window,
                16 * 8 - 8, 16 * 4,
                new Point(16 * 18, 16 * 14 + 8), 
                "Known Spells");

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
            if(Input.Player1.A & Input.Player1.A_Prev == false)
            {

                #region Set Hero's Current Spell

                //explosive spells
                if (currentlySelected.type == MenuItemType.Spells_Explosive_Bombos)
                {
                    PlayerData.currentSpell = SpellType.Explosive_Bombos;
                }

                //electric spells
                else if (currentlySelected.type == MenuItemType.Spells_Lightning_Ether)
                {
                    PlayerData.currentSpell = SpellType.Lightning_Ether;
                }

                #endregion


                //we will always have a menuItem selected
                Assets.Play(Assets.sfxMenuItem);
                UpdateMenuItems();

                if (Flags.PrintOutput)
                { Debug.WriteLine("menuItem pressed: " + currentlySelected.type); }
            }




            #region Exit Screen Input

            //exit this screen upon start / b button press
            if (
                (Input.Player1.Start & Input.Player1.Start_Prev == false)
                ||
                (Input.Player1.B & Input.Player1.B_Prev == false)
                )
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





            else if (displayState == DisplayState.Opened)
            {   //main screen opened state
                for (i = 0; i < menuItems.Count; i++)
                {   //scale all menuitems down each frame
                    Functions_Animation.ScaleSpriteDown(menuItems[i].compSprite);

                    if(currentlySelected == menuItems[i])
                    {   //animate the currently selected spell
                        Functions_Animation.Animate(currentlySelected.compAnim, currentlySelected.compSprite);
                    }
                    else //reset all other spells to 1st anim frame
                    { menuItems[i].compSprite.currentFrame = menuItems[i].compAnim.currentAnimation[0]; }
                }
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



        void UpdateMenuItems()
        {   //literally animate the menuitems please
            for (i = 0; i < menuItems.Count; i++)
            { Functions_Animation.ScaleSpriteDown(menuItems[i].compSprite); }
        }

    }
}