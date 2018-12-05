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

            

            //position and set the menuitems + their neighbors
            Functions_MenuItem.PlaceMenuItems(menuItems,
                window.interior.rec.X + 8 + 5,
                window.interior.rec.Y + 16 + 8 + 5,
                (byte)rows, 16 * 3 + 8, 24);


            //setup available spells

            #region  Wind Spells - has left neighbors to last column

            labels[0].text = "gust\nwind";
            //Functions_MenuItem.SetType(MenuItemType.Explosive_Single, menuItems[0]);
            menuItems[0].neighborLeft = menuItems[4]; //connect left

            labels[5].text = "shield\nwind";
            //Functions_MenuItem.SetType(MenuItemType.Explosive_Line, menuItems[5]);
            menuItems[5].neighborLeft = menuItems[9]; //connect left

            //labels[10].text = "";
            //Functions_MenuItem.SetType(MenuItemType.Spells_Explosive_Bombos, menuItems[10]);
            menuItems[10].neighborLeft = menuItems[14]; //connect left

            //labels[15].text = "";
            //Functions_MenuItem.SetType(MenuItemType.Spells_Explosive_Bombos, menuItems[15]);
            menuItems[15].neighborLeft = menuItems[19]; //connect left

            //labels[20].text = "";
            //Functions_MenuItem.SetType(MenuItemType.Spells_Explosive_Bombos, menuItems[20]);
            menuItems[20].neighborLeft = menuItems[24]; //connect left

            //labels[25].text = "";
            //Functions_MenuItem.SetType(MenuItemType.Spells_Explosive_Bombos, menuItems[25]);
            menuItems[25].neighborLeft = menuItems[29]; //connect left

            //labels[30].text = "";
            //Functions_MenuItem.SetType(MenuItemType.Spells_Explosive_Bombos, menuItems[30]);
            menuItems[30].neighborLeft = menuItems[34]; //connect left

            //labels[35].text = "";
            //Functions_MenuItem.SetType(MenuItemType.Spells_Explosive_Bombos, menuItems[35]);
            menuItems[35].neighborLeft = menuItems[39]; //connect left

            #endregion


            #region Explode/Fire Spells

            labels[1].text = "fire\nwalk";
            Functions_MenuItem.SetType(MenuItemType.Fire_Walk, menuItems[1]);

            labels[6].text = "single\nexplode";
            Functions_MenuItem.SetType(MenuItemType.Explosive_Single, menuItems[6]);

            labels[11].text = "chain\nexplode";
            Functions_MenuItem.SetType(MenuItemType.Explosive_Line, menuItems[11]);

            #endregion


            #region Ice Spells

            labels[2].text = "ice\nwalk";
            Functions_MenuItem.SetType(MenuItemType.Spells_Ice_FreezeWalk, menuItems[2]);

            labels[7].text = "freeze\nground";
            Functions_MenuItem.SetType(MenuItemType.Spells_Ice_FreezeGround, menuItems[7]);

            #endregion


            #region Electrical Spells

            labels[3].text = "ether\nspell";
            Functions_MenuItem.SetType(MenuItemType.Spells_Lightning_Ether, menuItems[3]);

            #endregion


            #region Summon Spells - has right neighbors to explosive column

            labels[4].text = "call\nbats";
            Functions_MenuItem.SetType(MenuItemType.Spells_Summon_Bat_Explosion, menuItems[4]);
            menuItems[4].neighborRight = menuItems[0]; //connect right

            //labels[9].text = "ether\nspell";
            //Functions_MenuItem.SetType(MenuItemType.Spells_Lightning_Ether, menuItems[9]);
            menuItems[9].neighborRight = menuItems[5]; //connect right

            //labels[14].text = "ether\nspell";
            //Functions_MenuItem.SetType(MenuItemType.Spells_Lightning_Ether, menuItems[14]);
            menuItems[14].neighborRight = menuItems[10]; //connect right

            //labels[19].text = "ether\nspell";
            //Functions_MenuItem.SetType(MenuItemType.Spells_Lightning_Ether, menuItems[19]);
            menuItems[19].neighborRight = menuItems[15]; //connect right

            //labels[24].text = "ether\nspell";
            //Functions_MenuItem.SetType(MenuItemType.Spells_Lightning_Ether, menuItems[24]);
            menuItems[24].neighborRight = menuItems[20]; //connect right

            //labels[29].text = "ether\nspell";
            //Functions_MenuItem.SetType(MenuItemType.Spells_Lightning_Ether, menuItems[29]);
            menuItems[29].neighborRight = menuItems[25]; //connect right

            //labels[34].text = "ether\nspell";
            //Functions_MenuItem.SetType(MenuItemType.Spells_Lightning_Ether, menuItems[34]);
            menuItems[34].neighborRight = menuItems[30]; //connect right

            //labels[39].text = "ether\nspell";
            //Functions_MenuItem.SetType(MenuItemType.Spells_Lightning_Ether, menuItems[39]);
            menuItems[39].neighborRight = menuItems[35]; //connect right

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

            //play the opening soundFX
            Assets.Play(Assets.sfxWindowOpen);
            //setup info widget
            Widgets.Info.Reset(16 * 26 + 0, 16 * 4);
            Widgets.Info.Display(currentlySelected);
        }

        public override void HandleInput(GameTime GameTime)
        {   //prevent all input until the screen has opened
            if (displayState != DisplayState.Opened) { return; }
            base.HandleInput(GameTime);


            //Handle A Button Input
            if(Input.Player1.A & Input.Player1.A_Prev == false)
            {
                //scale up any known menuItem and play the selection sound
                if (currentlySelected.type != MenuItemType.Unknown)
                { currentlySelected.compSprite.scale = 2.0f; }
                Assets.Play(Assets.sfxMenuItem);


                #region Set Hero's Current Spell (map MenuItemType to SpellType)

                //wind spells


                //explosive/fire spells
                if (currentlySelected.type == MenuItemType.Fire_Walk)
                { PlayerData.currentSpell = SpellType.Fire_Walk; }
                else if(currentlySelected.type == MenuItemType.Explosive_Single)
                { PlayerData.currentSpell = SpellType.Explosive_Single; }
                else if (currentlySelected.type == MenuItemType.Explosive_Line)
                { PlayerData.currentSpell = SpellType.Explosive_Line; }


                //ice spells
                else if (currentlySelected.type == MenuItemType.Spells_Ice_FreezeWalk)
                { PlayerData.currentSpell = SpellType.Ice_FreezeWalk; }
                else if (currentlySelected.type == MenuItemType.Spells_Ice_FreezeGround)
                { PlayerData.currentSpell = SpellType.Ice_FreezeGround; }


                //electric spells
                else if (currentlySelected.type == MenuItemType.Spells_Lightning_Ether)
                { PlayerData.currentSpell = SpellType.Lightning_Ether; }


                //summons spells
                else if (currentlySelected.type == MenuItemType.Spells_Summon_Bat_Explosion)
                { PlayerData.currentSpell = SpellType.Summon_Bat_Explosion; }

                #endregion


                //we will always have a menuItem selected
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
            Widgets.Info.Update();


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
            {   //overlay has faded in 100%, ok to remove screen
                //spellbook is inv's selected obj, update info widget to show it
                Widgets.Info.Display(Screens.Inventory.currentlySelected);
                //remove the screen
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