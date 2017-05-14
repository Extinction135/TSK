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
    public class ScreenDialog : Screen
    {

        public GameObject speaker;
        public String dialog;


        public ScreenDialog(GameObject Obj)
        {
            this.name = "Dialog Screen";
            speaker = Obj;
        }

        public override void LoadContent()
        {
            displayState = DisplayState.Opening;
            WidgetDialog.Reset(new Point(16 * 9, 16 * 12));



            //here is where we could check to see where in the story the hero is
            //then set the dialog screen accordingly
            dialog = "i'm the game's guide. later on, i'll tell the hero the story.\n";
            dialog += "and i'll comment on his progress beating dungeons.";
            //if (speaker.type == ObjType.VendorStory) { }
            //we'd need to track what dungeons that hero has beaten
            //this way the story vendor could comment on the hero's progress



            //display the dialog
            WidgetDialog.DisplayDialog(speaker.type, dialog);
            //play the opening soundFX
            Assets.Play(Assets.sfxInventoryOpen);
        }

        public override void HandleInput(GameTime GameTime)
        {   //exit this screen upon start or b button press
            if (Functions_Input.IsNewButtonPress(Buttons.Start) ||
                Functions_Input.IsNewButtonPress(Buttons.B) ||
                Functions_Input.IsNewButtonPress(Buttons.A))
            {
                Assets.Play(Assets.sfxInventoryClose);
                ScreenManager.RemoveScreen(this);
            }
        }

        public override void Update(GameTime GameTime)
        {
            WidgetDialog.Update();
            Functions_Dungeon.dungeonScreen.Update(GameTime);
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            WidgetDialog.Draw();
            ScreenManager.spriteBatch.End();
        }

    }
}