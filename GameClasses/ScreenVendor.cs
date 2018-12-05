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
        public InteractiveType vendorRef;

        public String welcomeDialog;



        public void SetVendor(InteractiveType vendor)
        {
            vendorRef = vendor;
        }





        public ScreenVendor()
        {
            this.name = "Vendor Screen";
            //needs a default value
            vendorRef = InteractiveType.Vendor_Items;

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
            displayState = DisplayState.Opening;

            Widgets.Loadout.Reset(16 * 9, 16 * 6);
            Widgets.ForSale.Reset(16 * 16, 16 * 6);
            Widgets.Info.Reset(16 * 24 + 8, 16 * 6);
            Widgets.Dialog.Reset(16 * 9, 16 * 12);


            #region Set the welcome dialog based on the vendor type

            //set a default welcome dialog
            welcomeDialog = "i've got many useful goods for sale, adventurer!";

            if (vendorRef == InteractiveType.Vendor_Armor)
            {
                Widgets.ForSale.window.title.text = "Tailor";
                welcomeDialog = "A fine set of cloth? A sturdy set of armor?\n";
                welcomeDialog += "A light and silent shawl? I'm quite busy with my designs..";
            }
            else if (vendorRef == InteractiveType.Vendor_Equipment)
            {
                Widgets.ForSale.window.title.text = "Jewler";
                welcomeDialog = "I'm working on expanding my selection..\n";
                welcomeDialog += "but you should equip the ring if you haven't already.";
            }
            else if (vendorRef == InteractiveType.Vendor_Items)
            {
                Widgets.ForSale.window.title.text = "Item Dealer";
                welcomeDialog = "What I've got, you won't find anywhere else.\n";
                welcomeDialog += "So pay my prices or kick dust. HaHaHa!";
            }
            else if (vendorRef == InteractiveType.Vendor_Magic)
            {
                Widgets.ForSale.window.title.text = "Lonely Kid";
                welcomeDialog = "I found these while diving in the sea. Not sure what they do..\n";
                welcomeDialog += "But I know they're worth something.. so don't play me cheap, mister!";
            }
            else if (vendorRef == InteractiveType.Vendor_Potions)
            {
                Widgets.ForSale.window.title.text = "Old Woman";
                welcomeDialog = "I carefully craft all my potions from local, non-gmo plants\n";
                welcomeDialog += "grown in my garden right here. Guaranteed no side effects!";
            }
            else if (vendorRef == InteractiveType.Vendor_Weapons)
            {
                Widgets.ForSale.window.title.text = "Blacksmith";
                welcomeDialog = "All I need is my forge and my bed. I keep it simple.\n";
                welcomeDialog += "You'll need everything I make, if you want to beat him..";
            }
            else if (vendorRef == InteractiveType.Vendor_Pets)
            {
                Widgets.ForSale.window.title.text = "For Adoption";
                welcomeDialog = "Please adopt a pet, mister! They all need good homes..\n";
                welcomeDialog += "My dad says I can't keep all these puppies..";
            }
            else if (vendorRef == InteractiveType.Vendor_EnemyItems)
            {
                Widgets.ForSale.window.title.text = "Secret Dungeon Vendor";
                welcomeDialog = "don't tell mr.grak I let you play with these secret items..\n";
                welcomeDialog += "he'd kick me from the game.. or worse.. turn me into a chicken.";
            }
            



            else if (vendorRef == InteractiveType.Vendor_Colliseum_Mob)
            {
                welcomeDialog = "I have a few colliseum challenges for sale..\n";
                welcomeDialog += "Win gold and prizes.. or... die.";
                Widgets.ForSale.window.title.text = "Challenges - Mobs";
            }


            #endregion


            //display the welcome dialog
            Widgets.Dialog.DisplayDialog(vendorRef, "Welcome!", welcomeDialog);
            //display the items for sale
            Widgets.ForSale.SetItemsForSale(vendorRef);
            //set the currently selected menuItem to the first inventory menuItem
            currentlySelected = Widgets.ForSale.menuItems[0];
            previouslySelected = Widgets.ForSale.menuItems[0];
            Widgets.Info.Display(currentlySelected);
            //play the opening soundFX
            Assets.Play(Assets.sfxWindowOpen);
        }

        public override void HandleInput(GameTime GameTime)
        {
            

            #region Exit Screen Input

            //exit this screen upon start / b button press
            if (
                (Input.Player1.Start & Input.Player1.Start_Prev == false)
                ||
                (Input.Player1.B & Input.Player1.B_Prev == false)
                )
            {
                Assets.Play(Assets.sfxWindowClose);
                CloseVendorScreen();
            }

            #endregion


            #region Purchase Item with A

            if (Input.Player1.A & Input.Player1.A_Prev == false)
            {
                if (currentlySelected.type != MenuItemType.Unknown)
                { currentlySelected.compSprite.scale = 2.0f; }
                PurchaseItem(currentlySelected);
                Assets.Play(Assets.sfxMenuItem);
            }

            #endregion


            #region Move between Vendor Items

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


        public void CloseVendorScreen()
        {
            Functions_MenuWindow.Close(Widgets.Loadout.window);
            Functions_MenuWindow.Close(Widgets.ForSale.window);
            Functions_MenuWindow.Close(Widgets.Info.window);
            Functions_MenuWindow.Close(Widgets.Dialog.window);
            displayState = DisplayState.Closing;
        }



        public Boolean CheckGold(byte cost)
        {   //if infinite gold is enabled, allow
            if (Flags.InfiniteGold) { return true; }
            //if hero has enough gold to buy, allow
            if (PlayerData.gold >= cost) { return true; }
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
                    if (PlayerData.heartsTotal < 9)
                    {   //increment saveData's total hearts
                        PlayerData.heartsTotal++;
                        Functions_WorldUI.Update(); //show the newly purchased heart
                        Pool.hero.health = PlayerData.heartsTotal; //refill hearts
                        CompleteSale(Item);
                    }
                    else { DialogMaxHearts(); }
                }
                else if (Item.type == MenuItemType.ItemBomb || Item.type == MenuItemType.ItemBomb3Pack)
                {   //check to see if hero is full on bombs
                    if (PlayerData.bombsCurrent < PlayerData.bombsMax)
                    {   //check to see how many bombs hero is purchasing
                        if (Item.type == MenuItemType.ItemBomb)
                        { PlayerData.bombsCurrent++; }
                        else { PlayerData.bombsCurrent += 3; }
                        CompleteSale(Item);
                    }
                    else { DialogCarryingMaxAmount(); }
                }
                //bow and arrows
                else if (Item.type == MenuItemType.ItemBow)
                {
                    if (!PlayerData.itemBow)
                    {
                        PlayerData.itemBow = true;
                        CompleteSale(Item);
                    }
                    else { DialogAlreadyPurchased(); }
                }
                else if (Item.type == MenuItemType.ItemArrowPack)
                {   //check to see if hero has a bow weapon
                    if (PlayerData.itemBow)
                    {   //check to see if hero is full on arrows
                        if (PlayerData.arrowsCurrent < PlayerData.arrowsMax)
                        {   //increment the arrows, complete the sale
                            PlayerData.arrowsCurrent += 20;
                            CompleteSale(Item);
                        }
                        else { DialogCarryingMaxAmount(); }
                    }
                    else { DialogNeedsBow(); }
                }



                else if (Item.type == MenuItemType.ItemFirerod)
                {
                    if (!PlayerData.itemFirerod)
                    {
                        PlayerData.itemFirerod = true;
                        CompleteSale(Item);
                    }
                    else { DialogAlreadyPurchased(); }
                }
                else if (Item.type == MenuItemType.ItemIcerod)
                {
                    if (!PlayerData.itemIcerod)
                    {
                        PlayerData.itemIcerod = true;
                        CompleteSale(Item);
                    }
                    else { DialogAlreadyPurchased(); }
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

                



                #region Weapons

                else if (Item.type == MenuItemType.WeaponNet)
                {
                    if (!PlayerData.weaponNet)
                    {
                        PlayerData.weaponNet = true;
                        CompleteSale(Item);
                    }
                    else { DialogAlreadyPurchased(); }
                }
                else if (Item.type == MenuItemType.WeaponShovel)
                {
                    if (!PlayerData.weaponShovel)
                    {
                        PlayerData.weaponShovel = true;
                        CompleteSale(Item);
                    }
                    else { DialogAlreadyPurchased(); }
                }
                else if (Item.type == MenuItemType.WeaponHammer)
                {
                    if (!PlayerData.weaponHammer)
                    {
                        PlayerData.weaponHammer = true;
                        CompleteSale(Item);
                    }
                    else { DialogAlreadyPurchased(); }
                }
                else if (Item.type == MenuItemType.WeaponWand)
                {
                    if (!PlayerData.weaponHammer)
                    {
                        PlayerData.weaponHammer = true;
                        CompleteSale(Item);
                    }
                    else { DialogAlreadyPurchased(); }
                }


                else if (Item.type == MenuItemType.WeaponFang)
                {
                    //set into player's enemyWeapon slot
                    PlayerData.enemyWeapon = MenuItemType.WeaponFang;
                    CompleteSale(Item);
                }

                #endregion


                #region Armor

                else if (Item.type == MenuItemType.ArmorCape)
                {
                    if (!PlayerData.armorCape)
                    {
                        PlayerData.armorCape = true;
                        CompleteSale(Item);
                    }
                    else { DialogAlreadyPurchased(); }
                }

                #endregion


                #region Equipment

                else if (Item.type == MenuItemType.EquipmentRing)
                {
                    if (!PlayerData.equipmentRing)
                    {
                        PlayerData.equipmentRing = true;
                        CompleteSale(Item);
                    }
                    else { DialogAlreadyPurchased(); }
                }

                #endregion


                #region Pets

                else if (Item.type == MenuItemType.PetDog_Gray)
                { CompleteAdoption(Item); }

                #endregion


                #region Challenges

                //standards
                else if (Item.type == MenuItemType.Challenge_Blobs)
                {
                    CompleteSale(Item);
                    CloseVendorScreen();
                    Functions_Colliseum.BeginChallenge(Challenges.Blobs);
                }

                //minis
                else if (Item.type == MenuItemType.Challenge_Mini_BlackEyes)
                {
                    CompleteSale(Item);
                    CloseVendorScreen();
                    Functions_Colliseum.BeginChallenge(Challenges.Mini_Blackeyes);
                }
                else if (Item.type == MenuItemType.Challenge_Mini_Spiders)
                {
                    CompleteSale(Item);
                    CloseVendorScreen();
                    Functions_Colliseum.BeginChallenge(Challenges.Mini_Spiders);
                }

                //bosses
                else if (Item.type == MenuItemType.Challenge_Bosses_BigEye)
                {
                    CompleteSale(Item);
                    CloseVendorScreen();
                    Functions_Colliseum.BeginChallenge(Challenges.Bosses_BigEye);
                }
                else if (Item.type == MenuItemType.Challenge_Bosses_BigBat)
                {
                    CompleteSale(Item);
                    CloseVendorScreen();
                    Functions_Colliseum.BeginChallenge(Challenges.Bosses_BigBat);
                }


                #endregion
                


            } //else, hero doesn't have enough gold to purchase the item
            else { DialogNotEnoughGold(); }
        }

        public void CompleteSale(MenuItem Item)
        {   //if infinite gold is disabled, deduct item cost
            if (!Flags.InfiniteGold) { PlayerData.gold -= Item.price; }
            //update various widgets affected by this purchase
            Widgets.ForSale.SetItemsForSale(vendorRef);
            Widgets.Info.Display(currentlySelected);
            Widgets.Loadout.UpdateLoadout();
            //display the purchase message & play purchase sfx
            DialogPurchaseThankyou();
        }

        public void CompleteAdoption(MenuItem Item)
        {   //set pet type, spawn it, update the loadout
            PlayerData.petType = Item.type;
            Functions_Hero.SpawnPet();
            Widgets.Loadout.UpdateLoadout();
            //display adoption dialog text
            Widgets.Dialog.DisplayDialog(vendorRef, "thanks!",
                "thank you for adopting this loveable pet.");
            Assets.Play(Assets.sfxBeatDungeon);
        }







        //vendor specific dialogs

        public void DialogNotEnoughGold()
        {
            Widgets.Dialog.DisplayDialog(vendorRef, "Sorry...",
                "you don't have enough gold to purchase this item.");
            Assets.Play(Assets.sfxError);
        }

        public void DialogPurchaseThankyou()
        {
            Widgets.Dialog.DisplayDialog(vendorRef, "Thanks!",
                "thank you for your purchase!");
            //Assets.Play(Assets.sfxBeatDungeon);
            Assets.Play(Assets.sfxGoldSpam);
        }

        public void DialogCarryingMaxAmount()
        {
            Widgets.Dialog.DisplayDialog(vendorRef, "Sorry...",
                "you are carrying the maximum amount of this item.");
            Assets.Play(Assets.sfxError);
        }

        public void DialogBottlesFull()
        {
            Widgets.Dialog.DisplayDialog(vendorRef, "Sorry...",
                "you do not have an empty bottle available to fill.");
            Assets.Play(Assets.sfxError);
        }

        public void DialogAlreadyPurchased()
        {
            Widgets.Dialog.DisplayDialog(vendorRef, "Sorry...",
                "you have already purchased this item.");
            Assets.Play(Assets.sfxError);
        }

        public void DialogMaxHearts()
        {
            Widgets.Dialog.DisplayDialog(vendorRef, "Sorry...",
                "you have reached the maximum amount of hearts.");
            Assets.Play(Assets.sfxError);
        }

        public void DialogNeedsBow()
        {
            Widgets.Dialog.DisplayDialog(vendorRef, "Sorry...",
                "you need a bow before you can shoot these arrows.");
            Assets.Play(Assets.sfxError);
        }

    }
}