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
        ScreenRec background = new ScreenRec();
        //these point to a menuItem that is part of a widget
        public MenuItem currentlySelected;
        public MenuItem previouslySelected;
        //simply visually tracks which menuItem is selected
        public ComponentSprite selectionBox;
        public SpeakerType vendorType;

        public String welcomeDialog;



        public ScreenVendor(GameObject Obj)
        {
            this.name = "Vendor Screen";
            if (Obj.type == ObjType.VendorArmor) { vendorType = SpeakerType.VendorArmor; }
            else if (Obj.type == ObjType.VendorEquipment) { vendorType = SpeakerType.VendorEquipment; }
            else if (Obj.type == ObjType.VendorItems) { vendorType = SpeakerType.VendorItems; }
            else if (Obj.type == ObjType.VendorMagic) { vendorType = SpeakerType.VendorMagic; }
            else if (Obj.type == ObjType.VendorPets) { vendorType = SpeakerType.VendorPets; }
            else if (Obj.type == ObjType.VendorPotions) { vendorType = SpeakerType.VendorPotions; }
            else if (Obj.type == ObjType.VendorWeapons) { vendorType = SpeakerType.VendorWeapons; }
        }

        public override void LoadContent()
        {
            background.alpha = 0.0f;
            background.fadeInSpeed = 0.03f;
            background.fadeOutSpeed = 0.07f;
            background.maxAlpha = 0.7f;
            displayState = DisplayState.Opening;

            Widgets.Loadout.Reset(16 * 9, 16 * 6);
            Widgets.ForSale.Reset(16 * 16, 16 * 6);
            Widgets.Info.Reset(16 * 24 + 8, 16 * 6);
            Widgets.Dialog.Reset(16 * 9, 16 * 12);


            #region Set the welcome dialog based on the vendor type

            //set a default welcome dialog
            welcomeDialog = "i've got many useful goods for sale, adventurer!";

            if (vendorType == SpeakerType.VendorArmor)
            { welcomeDialog = "I have a strong selection of armor for sale."; }
            else if (vendorType == SpeakerType.VendorEquipment)
            { welcomeDialog = "I have a useful selection of equipment for sale."; }
            else if (vendorType == SpeakerType.VendorItems)
            { welcomeDialog = "I have an interesting selection of items for sale."; }
            else if (vendorType == SpeakerType.VendorMagic)
            { welcomeDialog = "I have a mysterious selection of magic items for sale."; }
            else if (vendorType == SpeakerType.VendorPotions)
            { welcomeDialog = "I have a fine selection of potions for sale."; }
            else if (vendorType == SpeakerType.VendorWeapons)
            { welcomeDialog = "I have a wide selection of weapons for sale."; }
            else if (vendorType == SpeakerType.VendorPets)
            { welcomeDialog = "I have a happy selection of pets for adoption."; }

            #endregion


            //display the welcome dialog
            Widgets.Dialog.DisplayDialog(vendorType, "Welcome!", welcomeDialog);
            //display the items for sale
            Widgets.ForSale.SetItemsForSale(vendorType);
            //set the currently selected menuItem to the first inventory menuItem
            currentlySelected = Widgets.ForSale.menuItems[0];
            previouslySelected = Widgets.ForSale.menuItems[0];
            Widgets.Info.Display(currentlySelected);
            //create the selectionBox
            selectionBox = new ComponentSprite(Assets.mainSheet,
                new Vector2(0, 0), new Byte4(15, 7, 0, 0), new Point(16, 16));
            //play the opening soundFX
            Assets.Play(Assets.sfxWindowOpen);
        }

        public override void HandleInput(GameTime GameTime)
        {
            //exit this screen upon start or b button press

            if (Functions_Input.IsNewButtonPress(Buttons.Start) ||
                Functions_Input.IsNewButtonPress(Buttons.B))
            {
                Assets.Play(Assets.sfxWindowClose);
                Functions_MenuWindow.Close(Widgets.Loadout.window);
                Functions_MenuWindow.Close(Widgets.ForSale.window);
                Functions_MenuWindow.Close(Widgets.Info.window);
                Functions_MenuWindow.Close(Widgets.Dialog.window);
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

            #region Handle Screen State

            if (displayState == DisplayState.Opening)
            {
                selectionBox.scale = 2.0f;
                //fade background in
                background.fadeState = FadeState.FadeIn;
                Functions_ScreenRec.Fade(background);
                if (background.fadeState == FadeState.FadeComplete)
                {
                    displayState = DisplayState.Opened;
                    Assets.Play(Assets.sfxTextLetter);
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
            { ScreenManager.RemoveScreen(this); }

            #endregion


            Widgets.Loadout.Update();
            Widgets.ForSale.Update();
            Widgets.Info.Update();
            Widgets.Dialog.Update();

            //pulse the selectionBox alpha
            if (selectionBox.alpha >= 1.0f) { selectionBox.alpha = 0.1f; }
            else { selectionBox.alpha += 0.025f; }
            //match the position of the selectionBox to the currently selected menuItem
            selectionBox.position = currentlySelected.compSprite.position;
            //scale the selectionBox down to 1.0
            if (selectionBox.scale > 1.0f) { selectionBox.scale -= 0.07f; }
            else { selectionBox.scale = 1.0f; }
            //animate and scale the currentlySelected menuItem
            Functions_Animation.Animate(currentlySelected.compAnim, currentlySelected.compSprite);
            Functions_Animation.ScaleSpriteDown(currentlySelected.compSprite);
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            Functions_Draw.Draw(background);
            Widgets.Loadout.Draw();
            Widgets.ForSale.Draw();
            Widgets.Info.Draw();
            Widgets.Dialog.Draw();
            //only draw the selection box if the screen has opened completely
            if (displayState == DisplayState.Opened)
            { Functions_Draw.Draw(selectionBox); }
            ScreenManager.spriteBatch.End();
        }



        public Boolean CheckGold(byte cost)
        {   //if infinite gold is enabled, allow
            if (Flags.InfiniteGold) { return true; }
            //if hero has enough gold to buy, allow
            if (PlayerData.current.gold >= cost) { return true; }
            return false; //else disallow
        }

        public void PurchaseItem(MenuItem Item)
        {   //the player cannot purchase an unknown item
            if (Item.type == MenuItemType.Unknown) { return; }
            //see if hero has enough gold to purchase this item
            if (CheckGold(Item.price))
            {

                #region Items

                if (Item.type == MenuItemType.ItemHeart)
                {   //check that hero is not at max hearts value (9)
                    if (PlayerData.current.heartsTotal < 9)
                    {   //increment saveData's total hearts
                        PlayerData.current.heartsTotal++;
                        Functions_WorldUI.Update(); //show the newly purchased heart
                        Pool.hero.health = PlayerData.current.heartsTotal; //refill hearts
                        CompleteSale(Item);
                    }
                    else { DialogMaxHearts(); }
                }
                else if (Item.type == MenuItemType.ItemBomb || Item.type == MenuItemType.ItemBomb3Pack)
                {   //check to see if hero is full on bombs
                    if (PlayerData.current.bombsCurrent < PlayerData.current.bombsMax)
                    {   //check to see how many bombs hero is purchasing
                        if (Item.type == MenuItemType.ItemBomb)
                        { PlayerData.current.bombsCurrent++; }
                        else { PlayerData.current.bombsCurrent += 3; }
                        Pool.hero.item = MenuItemType.ItemBomb;
                        CompleteSale(Item);
                    }
                    else { DialogCarryingMaxAmount(); }
                }
                else if (Item.type == MenuItemType.ItemArrowPack)
                {   //check to see if hero has a bow weapon
                    if (PlayerData.current.weaponBow)
                    {   //check to see if hero is full on arrows
                        if (PlayerData.current.arrowsCurrent < PlayerData.current.arrowsMax)
                        {   //increment the arrows, complete the sale
                            PlayerData.current.arrowsCurrent += 20;
                            CompleteSale(Item);
                        }
                        else { DialogCarryingMaxAmount(); }
                    }
                    else { DialogNeedsBow(); }
                }

                #endregion


                #region Bottles

                //we dont set the player's item upon purchase of a bottle
                else if (Item.type == MenuItemType.BottleHealth)
                {   //if empty bottle was filled, complete the sale, else alert player
                    if (Functions_Bottle.FillBottle(MenuItemType.BottleHealth))
                    { CompleteSale(Item); } else { DialogBottlesFull(); } 
                }
                else if (Item.type == MenuItemType.BottleMagic)
                {   //if empty bottle was filled, complete the sale, else alert player
                    if (Functions_Bottle.FillBottle(MenuItemType.BottleMagic))
                    { CompleteSale(Item); } else { DialogBottlesFull(); }
                }
                else if (Item.type == MenuItemType.BottleCombo)
                {   //if empty bottle was filled, complete the sale, else alert player
                    if (Functions_Bottle.FillBottle(MenuItemType.BottleCombo))
                    { CompleteSale(Item); } else { DialogBottlesFull(); }
                }

                #endregion


                #region Magic Medallions

                else if (Item.type == MenuItemType.MagicFireball)
                {
                    if (!PlayerData.current.magicFireball)
                    {
                        PlayerData.current.magicFireball = true;
                        Pool.hero.item = Item.type;
                        CompleteSale(Item);
                    }
                    else { DialogAlreadyPurchased(); }
                }

                #endregion


                #region Weapons

                else if (Item.type == MenuItemType.WeaponBow)
                {
                    if (!PlayerData.current.weaponBow)
                    {
                        PlayerData.current.weaponBow = true;
                        Pool.hero.weapon = Item.type;
                        CompleteSale(Item);
                    }
                    else { DialogAlreadyPurchased(); }
                }
                else if (Item.type == MenuItemType.WeaponNet)
                {
                    if (!PlayerData.current.weaponNet)
                    {
                        PlayerData.current.weaponNet = true;
                        Pool.hero.weapon = Item.type;
                        CompleteSale(Item);
                    }
                    else { DialogAlreadyPurchased(); }
                }

                #endregion


                #region Armor

                else if (Item.type == MenuItemType.ArmorCape)
                {
                    if (!PlayerData.current.armorCape)
                    {
                        PlayerData.current.armorCape = true;
                        Pool.hero.armor = Item.type;
                        CompleteSale(Item);
                    }
                    else { DialogAlreadyPurchased(); }
                }

                #endregion


                #region Equipment

                else if (Item.type == MenuItemType.EquipmentRing)
                {
                    if (!PlayerData.current.equipmentRing)
                    {
                        PlayerData.current.equipmentRing = true;
                        Pool.hero.equipment = Item.type;
                        CompleteSale(Item);
                    }
                    else { DialogAlreadyPurchased(); }
                }

                #endregion


                #region Pets

                else if (Item.type == MenuItemType.PetStinkyDog
                    || Item.type == MenuItemType.PetChicken)
                { CompleteAdoption(Item); }

                #endregion


            } //else, hero doesn't have enough gold to purchase the item
            else { DialogNotEnoughGold(); }
        }

        public void CompleteSale(MenuItem Item)
        {   //if infinite gold is disabled, deduct item cost
            if (!Flags.InfiniteGold) { PlayerData.current.gold -= Item.price; }
            //update various widgets affected by this purchase
            Widgets.ForSale.SetItemsForSale(vendorType);
            Widgets.Info.Display(currentlySelected);
            Widgets.Loadout.UpdateLoadout();
            //display the purchase message & play purchase sfx
            DialogPurchaseThankyou();
        }

        public void CompleteAdoption(MenuItem Item)
        {   //set pet type, update hero's pet type
            PlayerData.current.hasPet = true;
            PlayerData.current.petType = Item.type;
            Functions_Hero.SetPet();
            //move pet to hero, update hero's loadout
            Functions_Hero.TeleportPet();
            Widgets.Loadout.UpdateLoadout();
            //display adoption dialog text
            Widgets.Dialog.DisplayDialog(vendorType, "thanks!",
                "thank you for adopting this loveable pet.");
            Assets.Play(Assets.sfxBeatDungeon);
        }

        public void DialogNotEnoughGold()
        {
            Widgets.Dialog.DisplayDialog(vendorType, "Sorry...",
                "you don't have enough gold to purchase this item.");
            Assets.Play(Assets.sfxError);
        }

        public void DialogPurchaseThankyou()
        {
            Widgets.Dialog.DisplayDialog(vendorType, "Thanks!",
                "thankyou for your purchase!");
            Assets.Play(Assets.sfxBeatDungeon);
        }

        public void DialogCarryingMaxAmount()
        {
            Widgets.Dialog.DisplayDialog(vendorType, "Sorry...",
                "you are carrying the maximum amount of this item.");
            Assets.Play(Assets.sfxError);
        }

        public void DialogBottlesFull()
        {
            Widgets.Dialog.DisplayDialog(vendorType, "Sorry...",
                "you do not have an empty bottle available to fill.");
            Assets.Play(Assets.sfxError);
        }

        public void DialogAlreadyPurchased()
        {
            Widgets.Dialog.DisplayDialog(vendorType, "Sorry...",
                "you have already purchased this item.");
            Assets.Play(Assets.sfxError);
        }

        public void DialogMaxHearts()
        {
            Widgets.Dialog.DisplayDialog(vendorType, "Sorry...",
                "you have reached the maximum amount of hearts.");
            Assets.Play(Assets.sfxError);
        }

        public void DialogNeedsBow()
        {
            Widgets.Dialog.DisplayDialog(vendorType, "Sorry...",
                "you need a bow before you can shoot these arrows.");
            Assets.Play(Assets.sfxError);
        }

    }
}