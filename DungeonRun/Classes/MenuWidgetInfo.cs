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
        public static ComponentText goldAmount;
        public static Rectangle goldBkg;
        public static ComponentText description;
        public static MenuRectangle divider1;



        static MenuWidgetInfo()
        {
            window = new MenuWindow(new Point(-100, -100), 
                new Point(100, 100), "Info Window");
            infoItem = new MenuItem();
            description = new ComponentText(Assets.font, 
                "default description \ntext here...", new Vector2(-100, -100), 
                Assets.colorScheme.textDark);
            divider1 = new MenuRectangle(new Point(-100, -100), 
                new Point(0, 0), Assets.colorScheme.windowInset);
            //create the gold amount text
            goldAmount = new ComponentText(Assets.font, "99",
                new Vector2(0, 0), Assets.colorScheme.textLight);
            goldBkg = new Rectangle(0, 0, 9, 7);
        }

        public static void Reset(Point Position, Point Size)
        {   //align this widgets component to Position + Size
            window.ResetAndMoveWindow(Position, Size, "Info Window");
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
            divider1.size.X = Size.X - 16;
            divider1.size.Y = 1;
            divider1.Reset();
            //align the goldAmount to the infoItem
            goldAmount.position.X = infoItem.compSprite.position.X - 1;
            goldAmount.position.Y = infoItem.compSprite.position.Y - 4;
            goldBkg.X = (int)goldAmount.position.X - 1;
            goldBkg.Y = (int)goldAmount.position.Y + 4;
        }

        public static void Display(MenuItem MenuItem)
        {   //set the widget's components based on the MenuItem's fields
            infoItem.compAnim.currentAnimation = MenuItem.compAnim.currentAnimation;
            infoItem.compSprite.rotation = MenuItem.compSprite.rotation;
            window.title.text = MenuItem.name;
            description.text = MenuItem.description;
            //if we are displaying the inventory gold menu item, also display the goldAmount + goldBkg
            if (MenuItem.type == MenuItemType.InventoryGold)
            { goldAmount.text = MenuWidgetLoadout.goldAmount.text; goldBkg.Width = 9; }
            else { goldAmount.text = ""; goldBkg.Width = 0; }
        }

        public static void Update()
        {
            window.Update();
            divider1.Update();
            AnimationFunctions.Animate(infoItem.compAnim, infoItem.compSprite);
        }

        public static void Draw()
        {
            DrawFunctions.Draw(window);
            DrawFunctions.Draw(divider1);
            if(window.interior.displayState == DisplayState.Opened)
            {
                DrawFunctions.Draw(infoItem.compSprite);
                DrawFunctions.Draw(description);
                ScreenManager.spriteBatch.Draw(
                    Assets.dummyTexture, goldBkg,
                    Assets.colorScheme.textDark);
                DrawFunctions.Draw(goldAmount);
            }
        }

    }
}