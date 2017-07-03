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
    public class WidgetDialog : Widget
    {
        public GameObject speaker;
        public ComponentText dialog;
        public String dialogString;
        public int charCount;
        public Boolean dialogDisplayed = false;


        public WidgetDialog()
        {
            window = new MenuWindow(new Point(-100, -100), new Point(100, 100), "");
            speaker = new GameObject(Assets.mainSheet);
            Functions_GameObject.SetType(speaker, ObjType.VendorStory);
            dialog = new ComponentText(Assets.font, "", new Vector2(0, 0), Assets.colorScheme.textDark);
        }

        public override void Reset(int X, int Y)
        {   //align this widgets component to Position + Size
            Functions_MenuWindow.ResetAndMove(window, X, Y,
                new Point(16 * 22, 16 * 4), "Hello!");
            speaker.compMove.newPosition.X = X + 16 * 1;
            speaker.compMove.newPosition.Y = Y + 16 * 2;
            dialog.position.X = X + 16 * 2;
            dialog.position.Y = Y + 16 * 1 + 4;
        }

        public override void Update()
        {
            Functions_MenuWindow.Update(window);
            Functions_Animation.Animate(speaker.compAnim, speaker.compSprite);

            if (window.interior.displayState == DisplayState.Opened)
            {
                charCount = dialogString.Count();
                if (charCount > 0)
                {   //strip off the first character, add it to dialog being drawn
                    dialog.text += dialogString[0].ToString();
                    dialogString = dialogString.Remove(0, 1);
                    //determine what text sound effect should play based on character count
                    if (charCount == 1)
                    { Assets.Play(Assets.sfxTextDone); dialogDisplayed = true; }
                    else { Assets.Play(Assets.sfxTextLetter); }
                }
            }
        }

        public override void Draw()
        {
            Functions_Draw.Draw(window);
            if (window.interior.displayState == DisplayState.Opened)
            {
                Functions_Draw.Draw(speaker.compSprite);
                Functions_Draw.Draw(dialog);
            }
        }


        
        public void DisplayDialog(ObjType Type, String Dialog)
        {   //set the menuItem's type and sprite frame
            Functions_GameObject.SetType(speaker, Type);
            //capture the dialog string, clear the dialog being drawn
            dialogString = Dialog;
            dialog.text = "";
            dialogDisplayed = false;
        }

    }
}