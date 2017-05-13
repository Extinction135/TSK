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
    public static class MenuWidgetInfo
    {

        public static MenuWindow window;
        public static MenuItem infoItem;

        public static ComponentAmountDisplay goldDisplay;

        public static ComponentText description;
        public static MenuRectangle divider1;



        static MenuWidgetInfo()
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

        public static void Reset(Point Position)
        {   //align this widgets component to Position + Size
            window.ResetAndMoveWindow(Position,
                new Point(16 * 6 + 8, 16 * 5 + 8), 
                "Info Window");
            description.position.X = Position.X + 8;
            description.position.Y = Position.Y + 16 * 3;
            //reset the infoItem to unknown, align it
            MenuItemFunctions.SetMenuItemData(MenuItemType.Unknown, infoItem);
            infoItem.compSprite.position.X = Position.X + 16 * 3 + 4;
            infoItem.compSprite.position.Y = Position.Y + 16 * 2;
            //reset and align the divider line
            divider1.openDelay = window.headerLine.openDelay;
            divider1.position.X = Position.X + 8;
            divider1.position.Y = Position.Y + 16 * 3;
            divider1.size.X = window.size.X - 16;
            divider1.size.Y = 1;
            divider1.Reset();
            //align the goldAmount display to the infoItem
            Functions_Component.Move(goldDisplay, infoItem);
        }



        public static void Display(MenuItem MenuItem)
        {   //set the widget's components based on the MenuItem's fields
            infoItem.compAnim.currentAnimation = MenuItem.compAnim.currentAnimation;
            infoItem.compSprite.rotation = MenuItem.compSprite.rotation;
            window.title.text = MenuItem.name;
            description.text = MenuItem.description;
            //if we are displaying the inventory gold menu item, update goldDisplay
            if (MenuItem.type == MenuItemType.InventoryGold)
            {   //make goldDisplay visible or non-visible
                goldDisplay.amount.text = MenuWidgetLoadout.goldDisplay.amount.text;
                goldDisplay.visible = true;
            }
            else { goldDisplay.amount.text = ""; goldDisplay.visible = false; }
        }



        public static void Update()
        {
            window.Update();
            divider1.Update();
            Functions_Animation.Animate(infoItem.compAnim, infoItem.compSprite);
        }

        public static void Draw()
        {
            Functions_Draw.Draw(window);
            Functions_Draw.Draw(divider1);
            if(window.interior.displayState == DisplayState.Opened)
            {
                Functions_Draw.Draw(infoItem.compSprite);
                Functions_Draw.Draw(description);
                if (goldDisplay.visible) { Functions_Draw.Draw(goldDisplay); }
            }
        }

    }
}