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

            Widgets.Loadout.Reset(16 * 9, 16 * 6);
            Widgets.ForSale.Reset(16 * 16, 16 * 6);
            Widgets.Info.Reset(16 * 24 + 8, 16 * 6);
            Widgets.Dialog.Reset(16 * 9, 16 * 12);

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
            Widgets.Dialog.DisplayDialog(vendorType.type, welcomeDialog);
            //display the items for sale
            Widgets.ForSale.SetItemsForSale(vendorType.type);
            //set the currently selected menuItem to the first inventory menuItem
            currentlySelected = Widgets.ForSale.menuItems[0];
            previouslySelected = Widgets.ForSale.menuItems[0];
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




        public void PurchaseItem(MenuItem Item)
        {
            //the player cannot purchase an unknown item
            if (Item.type == MenuItemType.Unknown) { return; }

            //see if hero has enough gold to purchase this item
            if (PlayerData.saveData.gold >= Item.price)
            {


                #region Items

                if (Item.type == MenuItemType.ItemBomb || Item.type == MenuItemType.ItemBombs)
                {   //check to see if hero is full on bombs
                    if (PlayerData.saveData.bombsCurrent < PlayerData.saveData.bombsMax)
                    {   //check to see how many bombs hero is purchasing
                        if (Item.type == MenuItemType.ItemBomb)
                        { PlayerData.saveData.bombsCurrent++; }
                        else { PlayerData.saveData.bombsCurrent += 3; }
                        //auto-equip the bombs purchased
                        Pool.hero.item = MenuItemType.ItemBomb;
                        CompleteSale(Item);
                    }
                    else { DialogCarryingMaxAmount(); }
                }
                else if (Item.type == MenuItemType.ItemArrows20)
                {   //check to see if hero is full on arrows
                    if (PlayerData.saveData.arrowsCurrent < PlayerData.saveData.arrowsMax)
                    {   //increment the arrows, complete the sale
                        PlayerData.saveData.arrowsCurrent += 20;
                        CompleteSale(Item);
                    }
                    else { DialogCarryingMaxAmount(); }
                }

                #endregion


                #region Bottles

                else if (Item.type == MenuItemType.BottleHealth)
                {
                    if (!PlayerData.saveData.bottleHealth)
                    {
                        PlayerData.saveData.bottle1 = true;
                        PlayerData.saveData.bottleHealth = true;
                        Pool.hero.item = Item.type;
                        CompleteSale(Item);
                    }
                    else { DialogCarryingMaxAmount(); }
                }
                else if (Item.type == MenuItemType.BottleMagic)
                {
                    if (!PlayerData.saveData.bottleMagic)
                    {
                        PlayerData.saveData.bottle2 = true;
                        PlayerData.saveData.bottleMagic = true;
                        Pool.hero.item = Item.type;
                        CompleteSale(Item);
                    }
                    else { DialogCarryingMaxAmount(); }
                }
                else if (Item.type == MenuItemType.BottleFairy)
                {
                    if (!PlayerData.saveData.bottleFairy)
                    {
                        PlayerData.saveData.bottle3 = true;
                        PlayerData.saveData.bottleFairy = true;
                        Pool.hero.item = Item.type;
                        CompleteSale(Item);
                    }
                    else { DialogCarryingMaxAmount(); }
                }

                #endregion


                #region Magic Medallions

                else if (Item.type == MenuItemType.MagicFireball)
                {
                    PlayerData.saveData.magicFireball = true;
                    Pool.hero.item = Item.type;
                    CompleteSale(Item);
                }

                #endregion


                #region Weapons

                else if (Item.type == MenuItemType.WeaponBow)
                {
                    if (!PlayerData.saveData.weaponBow)
                    {
                        PlayerData.saveData.weaponBow = true;
                        Pool.hero.weapon = Item.type;
                        CompleteSale(Item);
                    }
                    else { DialogAlreadyPurchased(); }
                }

                #endregion


                #region Equipment

                else if (Item.type == MenuItemType.EquipmentRing)
                {
                    if (!PlayerData.saveData.equipmentRing)
                    {
                        PlayerData.saveData.equipmentRing = true;
                        Pool.hero.equipment = Item.type;
                        CompleteSale(Item);
                    }
                    else { DialogAlreadyPurchased(); }
                }

                #endregion


            } //else, hero doesn't have enough gold to purchase the item
            else { DialogNotEnoughGold(); }
        }

        public void CompleteSale(MenuItem Item)
        {
            PlayerData.saveData.gold -= Item.price; //deduct cost
            //update various widgets affected by this purchase
            Widgets.ForSale.SetItemsForSale(vendorType.type);
            Widgets.Info.Display(currentlySelected);
            Widgets.Loadout.UpdateLoadout();
            //display the purchase message & play purchase sfx
            DialogPurchaseThankyou();
        }

        public void DialogNotEnoughGold()
        {
            Widgets.Dialog.DisplayDialog(vendorType.type,
                    "you don't have enough gold to purchase this item.");
            Assets.Play(Assets.sfxError);
        }

        public void DialogPurchaseThankyou()
        {
            Widgets.Dialog.DisplayDialog(vendorType.type,
                    "thankyou for your purchase!");
            Assets.Play(Assets.sfxBeatDungeon);
        }

        public void DialogCarryingMaxAmount()
        {
            Widgets.Dialog.DisplayDialog(vendorType.type,
                    "you are carrying the maximum amount of this item.");
            Assets.Play(Assets.sfxError);
        }

        public void DialogAlreadyPurchased()
        {
            Widgets.Dialog.DisplayDialog(vendorType.type,
                    "you have already purchased this item.");
            Assets.Play(Assets.sfxError);
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

            else if (Functions_Input.IsNewButtonPress(Buttons.A))
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
            Widgets.ForSale.Update();
            Widgets.Info.Update();
            Widgets.Dialog.Update();

            //pulse the selectionBox alpha
            if (selectionBox.alpha >= 1.0f) { selectionBox.alpha = 0.1f; }
            else { selectionBox.alpha += 0.025f; }
            //match the position of the selectionBox to the currently selected menuItem
            selectionBox.position = currentlySelected.compSprite.position;
            Functions_Animation.Animate(currentlySelected.compAnim, currentlySelected.compSprite);
            //scale the selectionBox down to 1.0
            if (selectionBox.scale > 1.0f) { selectionBox.scale -= 0.07f; }
            else { selectionBox.scale = 1.0f; }
            
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            ScreenManager.spriteBatch.Draw(Assets.dummyTexture, background, Assets.colorScheme.overlay * bkgAlpha);

            Widgets.Loadout.Draw();
            Widgets.ForSale.Draw();
            Widgets.Info.Draw();
            Widgets.Dialog.Draw();

            //only draw the selection box if the screen has opened completely
            if (displayState == DisplayState.Opened)
            { Functions_Draw.Draw(selectionBox); }
            ScreenManager.spriteBatch.End();
        }

    }
}