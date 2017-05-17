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

        //the foreground black rectangle, overlays and hides game content
        Rectangle background;
        public float bkgAlpha = 0.0f;
        public float maxAlpha = 0.7f;
        float fadeInSpeed = 0.03f;
        float fadeOutSpeed = 0.1f;

        //these point to a menuItem that is part of a widget
        public MenuItem currentlySelected;
        public MenuItem previouslySelected;

        //simply visually tracks which menuItem is selected
        public ComponentSprite selectionBox;



        public ScreenInventory() { this.name = "InventoryScreen"; }

        public override void LoadContent()
        {
            displayState = DisplayState.Opening;

            Widgets.Loadout.Reset(16 * 9, 16 * 4);
            Widgets.Stats.Reset(16 * 9, 16 * 10);
            Widgets.Inventory.Reset(16 * 16, 16 * 4);
            Widgets.Info.Reset(16 * 24 + 8, 16 * 4);
            Widgets.Options.Reset(16 * 24 + 8, 16 * 10);


            #region Connect loadout widget's menuItems to stats widget's menuItems

            Widgets.Loadout.menuItems[4].neighborDown = Widgets.Stats.menuItems[0];
            Widgets.Loadout.menuItems[5].neighborDown = Widgets.Stats.menuItems[0];
            Widgets.Loadout.menuItems[6].neighborDown = Widgets.Stats.menuItems[0];
            Widgets.Loadout.menuItems[7].neighborDown = Widgets.Stats.menuItems[0];
            Widgets.Stats.menuItems[0].neighborUp = Widgets.Loadout.menuItems[4];

            #endregion


            #region Connect loadout widget's menuItems to inventory widget's menuItems

            Widgets.Loadout.menuItems[3].neighborRight = Widgets.Inventory.menuItems[0];
            Widgets.Inventory.menuItems[0].neighborLeft = Widgets.Loadout.menuItems[3];

            Widgets.Loadout.menuItems[7].neighborRight = Widgets.Inventory.menuItems[5];
            Widgets.Inventory.menuItems[5].neighborLeft = Widgets.Loadout.menuItems[7];

            #endregion


            #region Connect stat widget's menuItems to inventory widget's menuItems

            Widgets.Stats.menuItems[0].neighborRight = Widgets.Inventory.menuItems[10];
            Widgets.Inventory.menuItems[10].neighborLeft = Widgets.Stats.menuItems[0];

            Widgets.Stats.menuItems[1].neighborRight = Widgets.Inventory.menuItems[15];
            Widgets.Inventory.menuItems[15].neighborLeft = Widgets.Stats.menuItems[1];

            Widgets.Stats.menuItems[2].neighborRight = Widgets.Inventory.menuItems[15];

            Widgets.Stats.menuItems[3].neighborRight = Widgets.Inventory.menuItems[20];
            Widgets.Inventory.menuItems[20].neighborLeft = Widgets.Stats.menuItems[3];

            #endregion


            #region Connect options widget's menuItems to inventory widget's menuItems

            Widgets.Options.menuItems[0].neighborLeft = Widgets.Inventory.menuItems[14];
            Widgets.Inventory.menuItems[14].neighborRight = Widgets.Options.menuItems[0];

            Widgets.Options.menuItems[2].neighborLeft = Widgets.Inventory.menuItems[19];
            Widgets.Inventory.menuItems[19].neighborRight = Widgets.Options.menuItems[2];

            Widgets.Options.menuItems[4].neighborLeft = Widgets.Inventory.menuItems[19];

            Widgets.Options.menuItems[6].neighborLeft = Widgets.Inventory.menuItems[24];
            Widgets.Inventory.menuItems[24].neighborRight = Widgets.Options.menuItems[6];

            #endregion


            //set the currently selected menuItem to the first inventory menuItem
            currentlySelected = Widgets.Inventory.menuItems[0];
            previouslySelected = Widgets.Inventory.menuItems[0];
            Widgets.Info.Display(currentlySelected);

            //create the selectionBox
            selectionBox = new ComponentSprite(Assets.mainSheet, 
                new Vector2(0, 0), new Byte4(15, 7, 0, 0), 
                new Point(16, 16));

            //create the background rec
            background = new Rectangle(0, 0, 640, 360);
            //play the opening soundFX
            Assets.Play(Assets.sfxInventoryOpen);
        }



        public void SetLoadout()
        {

            #region Items

            //check if currently selected is an item, set hero.item
            if (currentlySelected.type == MenuItemType.ItemBomb ||
                currentlySelected.type == MenuItemType.BottleEmpty ||
                currentlySelected.type == MenuItemType.BottleHealth ||
                currentlySelected.type == MenuItemType.BottleMagic ||
                currentlySelected.type == MenuItemType.BottleFairy ||
                currentlySelected.type == MenuItemType.MagicFireball)
            { Pool.hero.item = currentlySelected.type; }

            #endregion


            #region Weapons

            //check if currently selected is a weapon, set hero.weapon
            if (currentlySelected.type == MenuItemType.WeaponSword ||
                currentlySelected.type == MenuItemType.WeaponBow)
            { Pool.hero.weapon = currentlySelected.type; }

            #endregion


            #region Armor

            //check if currently selected is armor, set hero.armor
            if (currentlySelected.type == MenuItemType.ArmorCloth ||
                currentlySelected.type == MenuItemType.ArmorChest ||
                currentlySelected.type == MenuItemType.ArmorCape ||
                currentlySelected.type == MenuItemType.ArmorRobe)
            { Pool.hero.armor = currentlySelected.type; }

            #endregion


            #region Equipment

            //check if currently selected is equipment, set hero.equipment
            if (currentlySelected.type == MenuItemType.EquipmentRing)
            { Pool.hero.equipment = currentlySelected.type; }

            #endregion


            //update the LoadoutWidget to show equipped items
            Widgets.Loadout.UpdateLoadout();
        }



        public override void HandleInput(GameTime GameTime)
        {
            //exit this screen upon start or b button press
            
            if (Functions_Input.IsNewButtonPress(Buttons.Start) ||
                Functions_Input.IsNewButtonPress(Buttons.B))
            {
                Assets.Play(Assets.sfxInventoryClose);
                //ScreenManager.RemoveScreen(this);
                displayState = DisplayState.Closing;
            }

            else if(Functions_Input.IsNewButtonPress(Buttons.A))
            {
                if (currentlySelected.type != MenuItemType.Unknown)
                { currentlySelected.compSprite.scale = 2.0f; }
                Assets.Play(Assets.sfxMenuItem);
                SetLoadout();
            }

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

        }

        public override void Update(GameTime GameTime)
        {
            //fade background in
            if (displayState == DisplayState.Opening)
            {   
                bkgAlpha += fadeInSpeed;
                selectionBox.scale = 2.0f;
                if (bkgAlpha >= maxAlpha)
                {
                    bkgAlpha = maxAlpha;
                    displayState = DisplayState.Opened;
                    Assets.Play(Assets.sfxTextLetter);
                }
            }
            //fade background out
            else if (displayState == DisplayState.Closing)
            {
                bkgAlpha -= fadeOutSpeed;
                if (bkgAlpha <= 0.0f)
                {
                    bkgAlpha = 0.0f;
                    ScreenManager.RemoveScreen(this);
                }
            }

            Widgets.Loadout.Update();
            Widgets.Stats.Update();
            Widgets.Info.Update();
            Widgets.Inventory.Update();
            Widgets.Options.Update();

            //pulse the selectionBox alpha
            if (selectionBox.alpha >= 1.0f) { selectionBox.alpha = 0.1f; }
            else { selectionBox.alpha += 0.025f; }
            //match the position of the selectionBox to the currently selected menuItem
            selectionBox.position = currentlySelected.compSprite.position;
            
            //scale the selectionBox down to 1.0
            if (selectionBox.scale > 1.0f) { selectionBox.scale -= 0.07f; }
            else { selectionBox.scale = 1.0f; }

            //animate the currently selected menuItem - this scales it back down to 1.0
            if (currentlySelected.type != MenuItemType.InventoryGold) //inventory gold animates already
            { Functions_Animation.Animate(currentlySelected.compAnim, currentlySelected.compSprite); }
            //this prevents inventory gold from getting animated twice per frame
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            ScreenManager.spriteBatch.Draw(Assets.dummyTexture, background, Assets.colorScheme.overlay * bkgAlpha);

            Widgets.Loadout.Draw();
            Widgets.Stats.Draw();
            Widgets.Info.Draw();
            Widgets.Inventory.Draw();
            Widgets.Options.Draw();
            //only draw the selection box if the screen has opened completely
            if (displayState == DisplayState.Opened)
            { Functions_Draw.Draw(selectionBox); }
            
            ScreenManager.spriteBatch.End();
        }

    }
}