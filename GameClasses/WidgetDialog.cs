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
            speaker = new GameObject();
            Functions_GameObject.SetType(speaker, ObjType.NPC_Story);
            dialog = new ComponentText(Assets.font, "", new Vector2(0, 0), ColorScheme.textDark);
        }

        public override void Reset(int X, int Y)
        {   //align this widgets component to Position + Size
            Functions_MenuWindow.ResetAndMove(window, X, Y,
                new Point(16 * 22, 16 * 4), "Hello!");
            Functions_Movement.Teleport(speaker.compMove, X + 16 * 1, Y + 16 * 2);
            Functions_Component.Align(speaker.compMove, speaker.compSprite, speaker.compCollision);
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


        
        public void DisplayDialog(ObjType SpeakerType, String Title, String Dialog)
        {   
            //reset the speaker to the passed speakerType value
            Functions_GameObject.ResetObject(speaker);
            speaker.direction = Direction.Down;
            Functions_GameObject.SetType(speaker, SpeakerType);
            Functions_Animation.Animate(speaker.compAnim, speaker.compSprite);

            //capture the dialog string, clear the dialog being drawn
            dialogString = Dialog;
            dialog.text = "";
            dialogDisplayed = false;
            window.title.text = Title;
        }

    }
}