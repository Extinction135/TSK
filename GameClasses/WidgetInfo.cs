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
                ColorScheme.textDark);
            divider1 = new MenuRectangle(new Point(-100, -100), 
                new Point(0, 0), ColorScheme.windowInset);
            window.lines.Add(divider1);
            //create gold amount display
            goldDisplay = new ComponentAmountDisplay(0, 0, 0);
        }

        public override void Reset(int X, int Y)
        {   //reset additional divider lines
            divider1.position.Y = Y + 16 * 3;
            //align this widgets component to Position + Size
            Functions_MenuWindow.ResetAndMove(window, X, Y,
                new Point(16 * 6 + 8, 16 * 5 + 8), 
                "Info Window");
            //reset description
            description.position.X = X + 8;
            description.position.Y = Y + 16 * 3;
            //reset the infoItem to unknown, align it
            Functions_MenuItem.SetType(MenuItemType.Unknown, infoItem);
            infoItem.compSprite.position.X = X + 16 * 3 + 4;
            infoItem.compSprite.position.Y = Y + 16 * 2;
            //align the goldAmount display to the infoItem
            Functions_Component.Align(goldDisplay, infoItem.compSprite);
        }

        public override void Update()
        {
            Functions_MenuWindow.Update(window);
            Functions_Animation.Animate(infoItem.compAnim, infoItem.compSprite);
            Functions_Animation.ScaleSpriteDown(infoItem.compSprite); //does this need to be scaled?
        }

        public override void Draw()
        {
            Functions_Draw.Draw(window);
            if (window.interior.displayState == DisplayState.Opened)
            {
                Functions_Draw.Draw(infoItem.compSprite);
                Functions_Draw.Draw(description);
                if (goldDisplay.visible) { Functions_Draw.Draw(goldDisplay); }
            }
        }



        public void Display(MenuItem MenuItem)
        {   //match the infoItem to the passed MenuItem
            infoItem.compAnim.currentAnimation = MenuItem.compAnim.currentAnimation;
            infoItem.compSprite.texture = MenuItem.compSprite.texture;

            infoItem.compSprite.drawRec.Width = MenuItem.compSprite.drawRec.Width;
            infoItem.compSprite.drawRec.Height = MenuItem.compSprite.drawRec.Height;

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