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
    public class ScreenInventory : Screen
    {
        ScreenRec background = new ScreenRec();
        ScreenRec overlay = new ScreenRec();
        //these point to a menuItem that is part of a widget
        public MenuItem currentlySelected;
        public MenuItem previouslySelected;
        //simply visually tracks which menuItem is selected
        public ComponentSprite selectionBox;
        public ExitAction exitAction = ExitAction.ExitScreen;



        public ScreenInventory() { this.name = "InventoryScreen"; }

        public override void LoadContent()
        {
            background.alpha = 0.0f;
            background.fadeInSpeed = 0.03f;
            background.fadeOutSpeed = 0.07f;
            background.maxAlpha = 0.7f;
            overlay.fadeInSpeed = 0.025f; //fade in slowly
            displayState = DisplayState.Opening;

            Widgets.Loadout.Reset(16 * 8 - 8, 16 * 4);
            Widgets.QuestItems.Reset(16 * 8 - 8, 16 * 10);
            Widgets.Inventory.Reset(16 * 15 - 8, 16 * 4);
            Widgets.Info.Reset(16 * 26 + 0, 16 * 4);
            Widgets.Options.Reset(16 * 26 + 0, 16 * 10);


            #region Connect loadout widget's menuItems to stats widget's menuItems

            Widgets.Loadout.menuItems[4].neighborDown = Widgets.QuestItems.menuItems[0];
            Widgets.Loadout.menuItems[5].neighborDown = Widgets.QuestItems.menuItems[1];
            Widgets.Loadout.menuItems[6].neighborDown = Widgets.QuestItems.menuItems[2];
            Widgets.Loadout.menuItems[7].neighborDown = Widgets.QuestItems.menuItems[3];

            Widgets.QuestItems.menuItems[0].neighborUp = Widgets.Loadout.menuItems[4];
            Widgets.QuestItems.menuItems[1].neighborUp = Widgets.Loadout.menuItems[5];
            Widgets.QuestItems.menuItems[2].neighborUp = Widgets.Loadout.menuItems[6];
            Widgets.QuestItems.menuItems[3].neighborUp = Widgets.Loadout.menuItems[7];

            #endregion


            #region Connect loadout widget's menuItems to inventory widget's menuItems

            Widgets.Loadout.menuItems[3].neighborRight = Widgets.Inventory.menuItems[0];
            Widgets.Inventory.menuItems[0].neighborLeft = Widgets.Loadout.menuItems[3];

            Widgets.Loadout.menuItems[7].neighborRight = Widgets.Inventory.menuItems[7 * 1];
            Widgets.Inventory.menuItems[7 * 1].neighborLeft = Widgets.Loadout.menuItems[7];

            #endregion


            #region Connect questItems widget's menuItems to inventory widget's menuItems

            Widgets.Inventory.menuItems[7 * 3].neighborLeft = Widgets.QuestItems.menuItems[3];
            Widgets.QuestItems.menuItems[3].neighborRight = Widgets.Inventory.menuItems[7 * 3];

            Widgets.Inventory.menuItems[7 * 4].neighborLeft = Widgets.QuestItems.menuItems[11];
            Widgets.QuestItems.menuItems[11].neighborRight = Widgets.Inventory.menuItems[7 * 4];

            Widgets.Inventory.menuItems[7 * 5].neighborLeft = Widgets.QuestItems.menuItems[15];
            Widgets.QuestItems.menuItems[15].neighborRight = Widgets.Inventory.menuItems[7 * 5];

            #endregion


            #region Connect options widget's menuItems to inventory widget's menuItems
            
            Widgets.Options.menuItems[0].neighborLeft = Widgets.Inventory.menuItems[7 * 3 + 6];
            Widgets.Inventory.menuItems[7 * 3 + 6].neighborRight = Widgets.Options.menuItems[0];

            Widgets.Options.menuItems[4].neighborLeft = Widgets.Inventory.menuItems[7 * 4 + 6];
            Widgets.Inventory.menuItems[7 * 4 + 6].neighborRight = Widgets.Options.menuItems[4];

            Widgets.Options.menuItems[6].neighborLeft = Widgets.Inventory.menuItems[7 * 5 + 6];
            Widgets.Inventory.menuItems[7 * 5 + 6].neighborRight = Widgets.Options.menuItems[6];

            #endregion


            //set the currently selected menuItem to the first inventory menuItem
            currentlySelected = Widgets.Inventory.menuItems[0];
            previouslySelected = Widgets.Inventory.menuItems[0];
            Widgets.Info.Display(currentlySelected);
            //create the selectionBox
            selectionBox = new ComponentSprite(Assets.uiItemsSheet, 
                new Vector2(0, 0), 
                AnimationFrames.Ui_SelectionBox[0], 
                new Point(16, 16));
            //play the opening soundFX
            Assets.Play(Assets.sfxWindowOpen);
        }

        public override void HandleInput(GameTime GameTime)
        {   //select a menuItem with button A press
            if (Functions_Input.IsNewButtonPress(Buttons.A))
            {
                //scale up any known menuItem and play the selection sound
                if (currentlySelected.type != MenuItemType.Unknown)
                { currentlySelected.compSprite.scale = 2.0f; }
                Assets.Play(Assets.sfxMenuItem);

                //store the ID of the selected menuItem (index of inventory widget.menuItems)
                PlayerData.current.lastItemSelected = currentlySelected.id;


                #region Handle the Options MenuItems

                if (currentlySelected.type == MenuItemType.OptionsQuitGame)
                {   //close dungeon screen (autosaves), goto title screen
                    displayState = DisplayState.Closing;
                    exitAction = ExitAction.Title;
                    Assets.Play(Assets.sfxQuit);
                }
                else if (currentlySelected.type == MenuItemType.OptionsCheatMenu)
                {
                    ScreenManager.AddScreen(new ScreenCheats());
                }
                else if (currentlySelected.type == MenuItemType.OptionsSaveGame)
                {
                    ScreenManager.AddScreen(new ScreenLoadSaveNew(LoadSaveNewState.Save));
                }
                else if (currentlySelected.type == MenuItemType.OptionsLoadGame)
                {
                    ScreenManager.AddScreen(new ScreenLoadSaveNew(LoadSaveNewState.Load));
                }

                #endregion


                else
                {   //Handle Items, weapons, armor, and equipment


                    #region Items

                    if( //items
                        currentlySelected.type == MenuItemType.ItemBomb
                        || currentlySelected.type == MenuItemType.ItemBoomerang
                        //bottles
                        || currentlySelected.type == MenuItemType.BottleEmpty
                        || currentlySelected.type == MenuItemType.BottleBlob
                        || currentlySelected.type == MenuItemType.BottleCombo
                        || currentlySelected.type == MenuItemType.BottleFairy
                        || currentlySelected.type == MenuItemType.BottleHealth
                        || currentlySelected.type == MenuItemType.BottleMagic
                        //magics
                        || currentlySelected.type == MenuItemType.MagicFireball
                        )
                    {
                        PlayerData.current.currentItem = currentlySelected.type;
                        Pool.hero.item = currentlySelected.type;
                    }

                    #endregion


                    #region Weapons

                    else if (currentlySelected.type == MenuItemType.WeaponSword) //sword
                    { PlayerData.current.currentWeapon = 0; Pool.hero.weapon = currentlySelected.type; }
                    else if (currentlySelected.type == MenuItemType.WeaponBow) //bow
                    { PlayerData.current.currentWeapon = 1; Pool.hero.weapon = currentlySelected.type; }
                    else if (currentlySelected.type == MenuItemType.WeaponNet) //net
                    { PlayerData.current.currentWeapon = 2; Pool.hero.weapon = currentlySelected.type; }

                    #endregion


                    #region Armor

                    else if (currentlySelected.type == MenuItemType.ArmorCloth) //tunic
                    { PlayerData.current.currentArmor = 0; Pool.hero.armor = currentlySelected.type; }
                    else if (currentlySelected.type == MenuItemType.ArmorCape) //cape
                    { PlayerData.current.currentArmor = 2; Pool.hero.armor = currentlySelected.type; }
                    

                    #endregion


                    #region Equipment

                    else if (currentlySelected.type == MenuItemType.EquipmentRing) //ring
                    { PlayerData.current.currentEquipment = 0; Pool.hero.equipment = currentlySelected.type; }

                    #endregion


                    //update the LoadoutWidget to show equipped items
                    Widgets.Loadout.UpdateLoadout();
                }
            }


            #region Exit Screen Input

            //exit this screen upon start / b button / right shoulder button press
            else if (Functions_Input.IsNewButtonPress(Buttons.Start) 
                || Functions_Input.IsNewButtonPress(Buttons.B)
                || Functions_Input.IsNewButtonPress(Buttons.RightShoulder))
            {
                Assets.Play(Assets.sfxWindowClose);
                displayState = DisplayState.Closing;
                Functions_MenuWindow.Close(Widgets.Loadout.window);
                Functions_MenuWindow.Close(Widgets.QuestItems.window);
                Functions_MenuWindow.Close(Widgets.Info.window);
                Functions_MenuWindow.Close(Widgets.Inventory.window);
                Functions_MenuWindow.Close(Widgets.Options.window);
                exitAction = ExitAction.ExitScreen;
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
            {
                if (exitAction == ExitAction.ExitScreen)
                {   //fade background out
                    background.fadeState = FadeState.FadeOut;
                    Functions_ScreenRec.Fade(background);
                    if (background.fadeState == FadeState.FadeComplete)
                    { displayState = DisplayState.Closed; }
                }
                else if (exitAction == ExitAction.Title)
                {   //fade overlay in
                    overlay.fadeState = FadeState.FadeIn;
                    Functions_ScreenRec.Fade(overlay);
                    if (overlay.fadeState == FadeState.FadeComplete)
                    { displayState = DisplayState.Closed; }
                }
            }
            else if (displayState == DisplayState.Closed)
            {
                if (exitAction == ExitAction.ExitScreen)
                {   //overlay has faded in 100%
                    ScreenManager.RemoveScreen(this);
                }
                else if (exitAction == ExitAction.Title)
                {   //bkg has faded out
                    ScreenManager.ExitAndLoad(new ScreenTitle());
                }
            }

            #endregion


            Widgets.Loadout.Update();
            Widgets.QuestItems.Update();
            Widgets.Info.Update();
            Widgets.Inventory.Update();
            Widgets.Options.Update();


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


            //animate the currently selected menuItem - this scales it back down to 1.0
            if (currentlySelected.type != MenuItemType.InventoryGold) //inventory gold animates already
            {
                Functions_Animation.Animate(currentlySelected.compAnim, currentlySelected.compSprite);
                Functions_Animation.ScaleSpriteDown(currentlySelected.compSprite);
            }
            //this prevents inventory gold from getting animated twice per frame
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            Functions_Draw.Draw(background);
            Widgets.Loadout.Draw();
            Widgets.QuestItems.Draw();
            Widgets.Info.Draw();
            Widgets.Inventory.Draw();
            Widgets.Options.Draw();
            //only draw the selection box if the screen has opened completely
            if (displayState == DisplayState.Opened) { Functions_Draw.Draw(selectionBox); }
            Functions_Draw.Draw(overlay);
            ScreenManager.spriteBatch.End();
        }

    }
}