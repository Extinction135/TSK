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
    public class ScreenVendor : Screen
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
        public GameObject vendorType;
        public String welcomeDialog;


        public ScreenVendor(GameObject Obj)
        {
            this.name = "Vendor Screen";
            vendorType = Obj;
        }

        public override void LoadContent()
        {
            displayState = DisplayState.Opening;

            MenuWidgetLoadout.Reset(new Point(16 * 9, 16 * 6));
            MenuWidgetForSale.Reset(new Point(16 * 16, 16 * 6));
            MenuWidgetInfo.Reset(new Point(16 * 24 + 8, 16 * 6));
            MenuWidgetDialog.Reset(new Point(16 * 9, 16 * 12));

            //set a default welcome dialog
            welcomeDialog = "i've got many useful goods for sale, adventurer!";
            //set the welcome dialog based on the vendor type
            if (vendorType.type == ObjType.VendorArmor)
            { welcomeDialog = "I have a fine selection of armor for sale."; }
            else if (vendorType.type == ObjType.VendorEquipment)
            { welcomeDialog = "I have a fine selection of equipment for sale."; }
            else if (vendorType.type == ObjType.VendorItems)
            { welcomeDialog = "I have a fine selection of items for sale."; }
            else if (vendorType.type == ObjType.VendorMagic)
            { welcomeDialog = "I have a fine selection of magic items for sale."; }
            else if (vendorType.type == ObjType.VendorPotions)
            { welcomeDialog = "I have a fine selection of potions for sale."; }
            else if (vendorType.type == ObjType.VendorWeapons)
            { welcomeDialog = "I have a fine selection of weapons for sale."; }
            //display the welcome dialog
            MenuWidgetDialog.DisplayDialog(vendorType.type, welcomeDialog);

            //display the items for sale
            MenuWidgetForSale.SetItemsForSale(vendorType.type);
            //set the currently selected menuItem to the first inventory menuItem
            currentlySelected = MenuWidgetForSale.menuItems[0];
            previouslySelected = MenuWidgetForSale.menuItems[0];
            MenuWidgetInfo.Display(currentlySelected);

            //create the selectionBox
            selectionBox = new ComponentSprite(Assets.mainSheet,
                new Vector2(0, 0), new Byte4(15, 6, 0, 0),
                new Point(16, 16));

            //create the background rec
            background = new Rectangle(0, 0, 640, 360);
            //play the opening soundFX
            Assets.Play(Assets.sfxInventoryOpen);
        }



        public void PurchaseItem(MenuItem Item)
        {
            //the player cannot purchase an unknown item
            if (Item.type == MenuItemType.Unknown) { return; }

            //the player cannot purchase an item twice
            if (Item.type == MenuItemType.MagicFireball && PlayerData.saveData.magicFireball) { return; }
            else if (Item.type == MenuItemType.EquipmentRing && PlayerData.saveData.equipmentRing) { return; }

            //see if hero has enough gold to purchase this item
            if (PlayerData.saveData.gold >= Item.price)
            {
                //deduct cost, play purchase sound
                PlayerData.saveData.gold -= Item.price;
                //flip the corresponding saveData boolean true, auto-equip the purchased item
                if (Item.type == MenuItemType.MagicFireball)
                {
                    PlayerData.saveData.magicFireball = true;
                    Pool.hero.item = MenuItemType.MagicFireball;
                }
                else if (Item.type == MenuItemType.EquipmentRing)
                {
                    PlayerData.saveData.equipmentRing = true;
                    Pool.hero.equipment = MenuItemType.EquipmentRing;
                }

                //update the vendor's forSale items
                MenuWidgetForSale.SetItemsForSale(vendorType.type);
                MenuWidgetInfo.Display(currentlySelected);
                //update the loadout
                MenuWidgetLoadout.UpdateLoadout();
                //update the dialog widget
                MenuWidgetDialog.DisplayDialog(vendorType.type, 
                    "thanks for your purchase!");
                //play purchase complete sound effect
                Assets.Play(Assets.sfxBeatDungeon);
            }

            else//else, hero doesn't have enough gold to purcahse the item
            {   //notify player of this state, via dialog widget and error sound effect
                MenuWidgetDialog.DisplayDialog(vendorType.type, 
                    "you don't have enough gold to purchase that item.");
                Assets.Play(Assets.sfxError);
            }
        }



        public override void HandleInput(GameTime GameTime)
        {
            //exit this screen upon start or b button press

            if (Input.IsNewButtonPress(Buttons.Start) ||
                Input.IsNewButtonPress(Buttons.B))
            {
                Assets.Play(Assets.sfxInventoryClose);
                //ScreenManager.RemoveScreen(this);
                displayState = DisplayState.Closing;
            }

            else if (Input.IsNewButtonPress(Buttons.A))
            {
                if (currentlySelected.type != MenuItemType.Unknown)
                { currentlySelected.compSprite.scale = 2.0f; }
                PurchaseItem(currentlySelected);
                Assets.Play(Assets.sfxMenuItem);
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
                    MenuWidgetInfo.Display(currentlySelected);
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


            
            MenuWidgetLoadout.Update();
            MenuWidgetForSale.Update();
            MenuWidgetInfo.Update();
            MenuWidgetDialog.Update();

            //pulse the selectionBox alpha
            if (selectionBox.alpha >= 1.0f) { selectionBox.alpha = 0.1f; }
            else { selectionBox.alpha += 0.025f; }
            //match the position of the selectionBox to the currently selected menuItem
            selectionBox.position = currentlySelected.compSprite.position;
            AnimationFunctions.Animate(currentlySelected.compAnim, currentlySelected.compSprite);
            //scale the selectionBox down to 1.0
            if (selectionBox.scale > 1.0f) { selectionBox.scale -= 0.07f; }
            else { selectionBox.scale = 1.0f; }
            
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            ScreenManager.spriteBatch.Draw(Assets.dummyTexture, background, Assets.colorScheme.overlay * bkgAlpha);

            MenuWidgetLoadout.Draw();
            MenuWidgetForSale.Draw();
            MenuWidgetInfo.Draw();
            MenuWidgetDialog.Draw();

            //only draw the selection box if the screen has opened completely
            if (displayState == DisplayState.Opened)
            { DrawFunctions.Draw(selectionBox); }
            ScreenManager.spriteBatch.End();
        }

    }
}