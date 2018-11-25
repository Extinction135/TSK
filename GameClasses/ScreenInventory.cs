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




        //public ExitAction exitAction = ExitAction.ExitScreen;



        public ScreenInventory()
        {
            this.name = "InventoryScreen";

            //create the selectionBox
            selectionBox = new ComponentSprite(Assets.uiItemsSheet,
                new Vector2(0, 0),
                AnimationFrames.Ui_SelectionBox[0],
                new Point(16, 16));
        }

        public override void Open()
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
            
            //play the opening soundFX
            Assets.Play(Assets.sfxWindowOpen);
        }

        public override void HandleInput(GameTime GameTime)
        {   //select a menuItem with new A button press
            if (Input.Player1.A & Input.Player1.A_Prev == false)
            {
                //scale up any known menuItem and play the selection sound
                if (currentlySelected.type != MenuItemType.Unknown)
                { currentlySelected.compSprite.scale = 2.0f; }
                Assets.Play(Assets.sfxMenuItem);

                //store the ID of the selected menuItem (index of inventory widget.menuItems)
                PlayerData.current.lastItemSelected = currentlySelected.id;


                #region Handle Opening the Dungeon Map

                if (currentlySelected.type == MenuItemType.InventoryMap)
                {   //create a new map screen
                    ScreenManager.AddScreen(Screens.LevelMap);
                }

                #endregion


                #region Handle the Options MenuItems

                else if (currentlySelected.type == MenuItemType.OptionsQuitGame)
                {
                    //pop savePls dialog
                    Screens.Dialog.SetDialog(AssetsDialog.GameSavePls);
                    ScreenManager.AddScreen(Screens.Dialog);
                }
                else if (currentlySelected.type == MenuItemType.OptionsCheatMenu)
                {
                    ScreenManager.AddScreen(Screens.Cheats);
                }
                else if (currentlySelected.type == MenuItemType.OptionsOptionsMenu)
                {
                    ScreenManager.AddScreen(Screens.Options);
                }
                else if (currentlySelected.type == MenuItemType.OptionsSaveGame)
                {
                    Screens.LoadSaveNew.SetState(LoadSaveNewState.Save);
                    ScreenManager.AddScreen(Screens.LoadSaveNew);
                }
                else if (currentlySelected.type == MenuItemType.OptionsLoadGame)
                {
                    Screens.LoadSaveNew.SetState(LoadSaveNewState.Load);
                    ScreenManager.AddScreen(Screens.LoadSaveNew);
                }

                #endregion


                else
                {   //Handle Items, weapons, armor, and equipment


                    #region Items

                    if( //items
                        currentlySelected.type == MenuItemType.ItemBomb
                        || currentlySelected.type == MenuItemType.ItemBoomerang
                        || currentlySelected.type == MenuItemType.ItemBow
                        //bottles
                        || currentlySelected.type == MenuItemType.BottleEmpty
                        || currentlySelected.type == MenuItemType.BottleBlob
                        || currentlySelected.type == MenuItemType.BottleCombo
                        || currentlySelected.type == MenuItemType.BottleFairy
                        || currentlySelected.type == MenuItemType.BottleHealth
                        || currentlySelected.type == MenuItemType.BottleMagic
                        //magics
                        || currentlySelected.type == MenuItemType.MagicFireball
                        || currentlySelected.type == MenuItemType.MagicBombos
                        || currentlySelected.type == MenuItemType.MagicBolt
                        || currentlySelected.type == MenuItemType.MagicBat
                        )
                    {
                        PlayerData.current.currentItem = currentlySelected.type;
                        Pool.hero.item = currentlySelected.type;
                    }

                    #endregion


                    #region Weapons

                    else if (
                        currentlySelected.type == MenuItemType.WeaponSword
                        || currentlySelected.type == MenuItemType.WeaponNet
                        || currentlySelected.type == MenuItemType.WeaponShovel
                        || currentlySelected.type == MenuItemType.WeaponHammer
                        || currentlySelected.type == MenuItemType.WeaponFang
                        ) 
                    {
                        PlayerData.current.currentWeapon = currentlySelected.type;
                        Pool.hero.weapon = currentlySelected.type;
                    }

                    #endregion


                    #region Armor

                    else if (
                        currentlySelected.type == MenuItemType.ArmorCloth
                        || currentlySelected.type == MenuItemType.ArmorCape
                        )
                    {
                        PlayerData.current.currentArmor = currentlySelected.type;
                        Pool.hero.armor = currentlySelected.type;
                    }

                    #endregion


                    #region Equipment

                    else if (
                        currentlySelected.type == MenuItemType.EquipmentRing
                        //|| currentlySelected.type == MenuItemType.ArmorCape
                        )
                    {
                        PlayerData.current.currentEquipment = currentlySelected.type;
                        Pool.hero.equipment = currentlySelected.type;
                    }

                    #endregion


                    //update the LoadoutWidget to show equipped items
                    Widgets.Loadout.UpdateLoadout();
                }
            }


            #region Exit Screen Input

            //exit this screen upon start / b button
            if (
                (Input.Player1.Start & Input.Player1.Start_Prev == false)
                ||
                (Input.Player1.B & Input.Player1.B_Prev == false)
                )
            {
                Assets.Play(Assets.sfxWindowClose);
                displayState = DisplayState.Closing;
                Functions_MenuWindow.Close(Widgets.Loadout.window);
                Functions_MenuWindow.Close(Widgets.QuestItems.window);
                Functions_MenuWindow.Close(Widgets.Info.window);
                Functions_MenuWindow.Close(Widgets.Inventory.window);
                Functions_MenuWindow.Close(Widgets.Options.window);
            }

            #endregion


            #region Move Between MenuItems Input

            //get the previouslySelected menuItem
            previouslySelected = currentlySelected;
            // prevents rapid scrolling
            if(Input.Player1.direction != Input.Player1.direction_Prev)
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
                //fade background out
                background.fadeState = FadeState.FadeOut;
                Functions_ScreenRec.Fade(background);
                if (background.fadeState == FadeState.FadeComplete)
                { displayState = DisplayState.Closed; }
            }
            else if (displayState == DisplayState.Closed)
            {
                //overlay has faded in 100%
                ScreenManager.RemoveScreen(this);
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