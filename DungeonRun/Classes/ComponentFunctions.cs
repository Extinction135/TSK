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
    public static class ComponentFunctions
    {

        public static void Align(ComponentMovement Move, ComponentSprite Sprite, ComponentCollision Coll)
        {   //aligns the collision component and sprite component to the move component's position
            Sprite.position.X = (int)Move.newPosition.X;
            Sprite.position.Y = (int)Move.newPosition.Y;
            SetZdepth(Sprite);
            Coll.rec.X = (int)Move.newPosition.X + Coll.offsetX;
            Coll.rec.Y = (int)Move.newPosition.Y + Coll.offsetY;
        }

        public static void CenterOrigin(ComponentSprite Sprite)
        {
            Sprite.origin.X = Sprite.cellSize.X * 0.5f;
            Sprite.origin.Y = Sprite.cellSize.Y * 0.5f;
        }

        public static void SetZdepth(ComponentSprite Sprite)
        {
            Sprite.zDepth = 0.999990f - (Sprite.position.Y + Sprite.zOffset) * 0.000001f;
        }

        public static void UpdateCellSize(ComponentSprite Sprite)
        {
            Sprite.drawRec.Width = Sprite.cellSize.X;
            Sprite.drawRec.Height = Sprite.cellSize.Y;
        }

        public static void CenterText(ComponentButton Button)
        {   //measure width of the button's text
            Button.textWidth = (int)Assets.font.MeasureString(Button.compText.text).X;
            //resize button to fit around the text
            Button.rec.Width = Button.textWidth + 6;
            //center text to button, prevent half pixel offsets
            Button.compText.position.X = (int)(Button.rec.Location.X + Button.rec.Width / 2) - (Button.textWidth / 2);
            //center text vertically
            Button.compText.position.Y = Button.rec.Location.Y - 3;
        }

        static int textWidth;
        public static void CenterText(ComponentText Text, SpriteFont Font, int X)
        {   //center the text to the X and Y position passed in, prevent half pixel offsets
            textWidth = (int)Font.MeasureString(Text.text).X;
            Text.position.X = (int)X - (textWidth / 2);
        }

    }
}