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
            Functions_GameObject.SetType(speaker, ObjType.VendorStory);
            dialog = new ComponentText(Assets.font, "", new Vector2(0, 0), Assets.colorScheme.textDark);
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


        
        public void DisplayDialog(SpeakerType SpeakerType, String Title, String Dialog)
        {   //set the menuItem's type and sprite frame
            Functions_GameObject.ResetObject(speaker);

            if (SpeakerType == SpeakerType.Guide)
            {
                speaker.compSprite.texture = Assets.shopSheet;
                speaker.compAnim.currentAnimation = new List<Byte4> { new Byte4(7, 7, 0, 0) };
            }
            else if (SpeakerType == SpeakerType.Hero)
            {
                speaker.compSprite.texture = Assets.heroSheet;
                speaker.compAnim.currentAnimation = new List<Byte4> { new Byte4(0, 0, 0, 0) };
            }
            else if (SpeakerType == SpeakerType.Blob)
            {
                speaker.compSprite.texture = Assets.blobSheet;
                speaker.compAnim.currentAnimation = new List<Byte4> { new Byte4(0, 0, 0, 0) };
            }
            else
            {   //they are a vendor
                speaker.compSprite.texture = Assets.shopSheet;
                speaker.compAnim.currentAnimation = new List<Byte4> { new Byte4(0, 7, 0, 0) };
                //set the X frame based on the speaker type
                if (SpeakerType == SpeakerType.VendorItems) { speaker.compAnim.currentAnimation[0].X = 0;  }
                else if (SpeakerType == SpeakerType.VendorPotions) { speaker.compAnim.currentAnimation[0].X = 1;  }
                else if (SpeakerType == SpeakerType.VendorMagic) { speaker.compAnim.currentAnimation[0].X = 2; }
                else if (SpeakerType == SpeakerType.VendorWeapons) { speaker.compAnim.currentAnimation[0].X = 3; }
                else if (SpeakerType == SpeakerType.VendorArmor) { speaker.compAnim.currentAnimation[0].X = 4; }
                else if (SpeakerType == SpeakerType.VendorEquipment) { speaker.compAnim.currentAnimation[0].X = 5; }
                else if (SpeakerType == SpeakerType.VendorPets) { speaker.compAnim.currentAnimation[0].X = 6; }
            }

            Functions_Animation.Animate(speaker.compAnim, speaker.compSprite);
            //capture the dialog string, clear the dialog being drawn
            dialogString = Dialog;
            dialog.text = "";
            dialogDisplayed = false;
            window.title.text = Title;
        }

    }
}