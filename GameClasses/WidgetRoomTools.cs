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
    public class WidgetRoomTools : Widget
    {
        int j;
        public int total;
        int counter;


        public List<ComponentButton> buttons; //save, new, load buttons
        public ComponentButton saveBtn;
        public ComponentButton loadBtn;

        public ComponentButton newRoomBtn;
        public ComponentButton roomTypeBtn;
        public RoomType roomType;

        public ComponentButton reloadRoomBtn;


        public WidgetRoomTools()
        {

            #region Create Window and Divider lines

            window = new MenuWindow(
                new Point(8, 16 * 3), 
                new Point(16 * 6, 16 * 16 + 8),
                "Room Builder");
            window.lines.Add(new MenuRectangle(new Point(0, 0), new Point(0, 0), Assets.colorScheme.windowInset));
            window.lines.Add(new MenuRectangle(new Point(0, 0), new Point(0, 0), Assets.colorScheme.windowInset));

            #endregion


            #region Create Save/Play/Load New/Type/Reload Buttons

            buttons = new List<ComponentButton>();
            saveBtn = new ComponentButton("save", new Point(0, 0));
            loadBtn = new ComponentButton("load", new Point(0, 0));
            buttons.Add(saveBtn);
            buttons.Add(loadBtn);

            newRoomBtn = new ComponentButton("new room", new Point(0, 0));
            roomTypeBtn = new ComponentButton("column", new Point(0, 0));
            reloadRoomBtn = new ComponentButton("reload", new Point(0, 0));
            buttons.Add(newRoomBtn);
            buttons.Add(roomTypeBtn);
            buttons.Add(reloadRoomBtn);

            roomType = RoomType.Column;

            #endregion

        }

        public override void Reset(int X, int Y)
        {   //reset the room builder widget's window
            window.lines[2].position.Y = Y + (16 * 10);
            window.lines[3].position.Y = Y + (16 * 12);
            Functions_MenuWindow.ResetAndMove(window, X, Y, window.size, window.title.text);


            #region Move Save/New/Load/etc Buttons

            saveBtn.rec.X = X + 16 * 1 - 8;
            saveBtn.rec.Y = Y + 16 * 12 + 8;
            Functions_Component.CenterText(saveBtn);

            loadBtn.rec.X = X + 16 * 4 + 10 - 8;
            loadBtn.rec.Y = Y + 16 * 12 + 8;
            Functions_Component.CenterText(loadBtn);

            newRoomBtn.rec.X = X + 16 * 1 - 8;
            newRoomBtn.rec.Y = Y + 16 * 13 + 8;
            Functions_Component.CenterText(newRoomBtn);

            roomTypeBtn.rec.X = X + 16 * 4 - 8 - 1;
            roomTypeBtn.rec.Y = Y + 16 * 13 + 8;
            Functions_Component.CenterText(roomTypeBtn);

            reloadRoomBtn.rec.X = X + 16 * 1 - 8;
            reloadRoomBtn.rec.Y = Y + 16 * 14 + 8;
            Functions_Component.CenterText(reloadRoomBtn);

            #endregion

        }

        public override void Update()
        {
            Functions_MenuWindow.Update(window);
            if (window.interior.displayState == DisplayState.Opened)
            {
            }
        }

        public override void Draw()
        {
            Functions_Draw.Draw(window);
            if (window.interior.displayState == DisplayState.Opened)
            {
                for (i = 0; i < buttons.Count; i++) //draw all the buttons
                { Functions_Draw.Draw(buttons[i]); }
            }
        }


        
    }
}