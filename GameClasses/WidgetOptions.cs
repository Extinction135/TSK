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
    public class WidgetOptions : Widget
    {
        public List<ComponentText> labels;
        public List<MenuItem> menuItems;



        public WidgetOptions()
        {
            window = new MenuWindow(new Point(-100, -100),
                new Point(100, 100), "Options");

            //create menuitem labels
            labels = new List<ComponentText>();
            for (i = 0; i < 8; i++)
            { labels.Add(new ComponentText(Assets.font, 
                "test\ntest", new Vector2(-100, -100), 
                Assets.colorScheme.textDark));
            }

            //create menuitems
            menuItems = new List<MenuItem>();
            for (i = 0; i < 8; i++) { menuItems.Add(new MenuItem()); }
        }

        public override void Reset(int X, int Y)
        {   //align this widgets component to Position + Size
            Functions_MenuWindow.ResetAndMove(window, X, Y, 
                new Point(16 * 6 + 8, 16 * 8 + 8), "Options");


            #region Place the menuItems

            menuItems[0].compSprite.position.X = window.background.position.X + 16 * 1;
            menuItems[0].compSprite.position.Y = window.background.position.Y + 16 * 2;
            menuItems[1].compSprite.position.X = menuItems[0].compSprite.position.X + 16 * 3;
            menuItems[1].compSprite.position.Y = menuItems[0].compSprite.position.Y;

            menuItems[2].compSprite.position.X = menuItems[0].compSprite.position.X;
            menuItems[2].compSprite.position.Y = menuItems[0].compSprite.position.Y + 24;
            menuItems[3].compSprite.position.X = menuItems[2].compSprite.position.X + 16 * 3;
            menuItems[3].compSprite.position.Y = menuItems[2].compSprite.position.Y;

            menuItems[4].compSprite.position.X = menuItems[2].compSprite.position.X;
            menuItems[4].compSprite.position.Y = menuItems[2].compSprite.position.Y + 24;
            menuItems[5].compSprite.position.X = menuItems[4].compSprite.position.X + 16 * 3;
            menuItems[5].compSprite.position.Y = menuItems[4].compSprite.position.Y;

            menuItems[6].compSprite.position.X = menuItems[4].compSprite.position.X;
            menuItems[6].compSprite.position.Y = menuItems[4].compSprite.position.Y + 24;
            menuItems[7].compSprite.position.X = menuItems[6].compSprite.position.X + 16 * 3;
            menuItems[7].compSprite.position.Y = menuItems[6].compSprite.position.Y;

            #endregion


            #region Set menuItem data

            labels[0].text = "save\ngame";
            Functions_MenuItem.SetType(MenuItemType.OptionsSaveGame, menuItems[0]);

            labels[1].text = "cheat\nmenu";
            Functions_MenuItem.SetType(MenuItemType.OptionsCheatMenu, menuItems[1]);

            labels[2].text = "";
            Functions_MenuItem.SetType(MenuItemType.Unknown, menuItems[2]);

            labels[3].text = "";
            Functions_MenuItem.SetType(MenuItemType.Unknown, menuItems[3]);

            labels[4].text = "";
            Functions_MenuItem.SetType(MenuItemType.Unknown, menuItems[4]);

            labels[5].text = "optns\nmenu";
            Functions_MenuItem.SetType(MenuItemType.OptionsOptionsMenu, menuItems[5]);

            labels[6].text = "load\ngame";
            Functions_MenuItem.SetType(MenuItemType.OptionsLoadGame, menuItems[6]);

            labels[7].text = "quit\ngame";
            Functions_MenuItem.SetType(MenuItemType.OptionsQuitGame, menuItems[7]);

            for (i = 0; i < 8; i++)
            {   //position labels relative to menuItems
                labels[i].position.X = menuItems[i].compSprite.position.X + 12;
                labels[i].position.Y = menuItems[i].compSprite.position.Y - 12;
            }

            #endregion


            //set the menuItem's neighbors
            Functions_MenuItem.SetNeighbors(menuItems, 2);
            //reset scale of menuItems to 1.0
            for (i = 0; i < 8; i++) { menuItems[i].compSprite.scale = 1.0f; }
        }

        public override void Update()
        {
            Functions_MenuWindow.Update(window);
        }

        public override void Draw()
        {
            Functions_Draw.Draw(window);
            if (window.interior.displayState == DisplayState.Opened)
            {
                for (i = 0; i < menuItems.Count; i++)
                { Functions_Draw.Draw(menuItems[i].compSprite); }
                for (i = 0; i < labels.Count; i++)
                { Functions_Draw.Draw(labels[i]); }
            }
        }

    }
}