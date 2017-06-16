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
    public class WidgetInfo : Widget
    {
        public MenuItem infoItem;
        public ComponentAmountDisplay goldDisplay;
        public ComponentText description;
        public MenuRectangle divider1;



        public WidgetInfo()
        {
            window = new MenuWindow(new Point(-100, -100), new Point(100, 100), "");
            infoItem = new MenuItem();
            description = new ComponentText(Assets.font, 
                "default description \ntext here...", new Vector2(-100, -100), 
                Assets.colorScheme.textDark);
            divider1 = new MenuRectangle(new Point(-100, -100), 
                new Point(0, 0), Assets.colorScheme.windowInset);
            //create gold amount display
            goldDisplay = new ComponentAmountDisplay(0, 0, 0);
        }

        public override void Reset(int X, int Y)
        {   //align this widgets component to Position + Size
            Functions_MenuWindow.ResetAndMove(window, X, Y,
                new Point(16 * 6 + 8, 16 * 5 + 8), 
                "Info Window");
            description.position.X = X + 8;
            description.position.Y = Y + 16 * 3;
            //reset the infoItem to unknown, align it
            Functions_MenuItem.SetMenuItemData(MenuItemType.Unknown, infoItem);
            infoItem.compSprite.position.X = X + 16 * 3 + 4;
            infoItem.compSprite.position.Y = Y + 16 * 2;
            //reset and align the divider line
            divider1.openDelay = window.headerLine.openDelay;
            divider1.position.X = X + 8;
            divider1.position.Y = Y + 16 * 3;
            divider1.size.X = window.size.X - 16;
            divider1.size.Y = 1;
            Functions_MenuRectangle.Reset(divider1);
            //align the goldAmount display to the infoItem
            Functions_Component.Move(goldDisplay, infoItem);
        }

        public override void Update()
        {
            Functions_MenuWindow.Update(window);
            Functions_MenuRectangle.Update(divider1);
            Functions_Animation.Animate(infoItem.compAnim, infoItem.compSprite);
        }

        public override void Draw()
        {
            Functions_Draw.Draw(window);
            Functions_Draw.Draw(divider1);
            if (window.interior.displayState == DisplayState.Opened)
            {
                Functions_Draw.Draw(infoItem.compSprite);
                Functions_Draw.Draw(description);
                if (goldDisplay.visible) { Functions_Draw.Draw(goldDisplay); }
            }
        }



        public void Display(MenuItem MenuItem)
        {   //set the widget's components based on the MenuItem's fields
            infoItem.compAnim.currentAnimation = MenuItem.compAnim.currentAnimation;
            infoItem.compSprite.rotation = MenuItem.compSprite.rotation;
            window.title.text = MenuItem.name;
            description.text = MenuItem.description;
            //if we are displaying the inventory gold menu item, update goldDisplay
            if (MenuItem.type == MenuItemType.InventoryGold)
            {   //make goldDisplay visible or non-visible
                goldDisplay.amount.text = Widgets.Loadout.goldDisplay.amount.text;
                goldDisplay.visible = true;
            }
            else { goldDisplay.amount.text = ""; goldDisplay.visible = false; }
        }

    }
}