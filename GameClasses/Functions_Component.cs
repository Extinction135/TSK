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
    public static class Functions_Component
    {
        static int textWidth;



        public static void Align(ComponentMovement Move, ComponentSprite Sprite, ComponentCollision Coll)
        {   //aligns the collision component and sprite component to the move component's position
            Sprite.position.X = (int)Move.newPosition.X;
            Sprite.position.Y = (int)Move.newPosition.Y;
            SetZdepth(Sprite);
            Coll.rec.X = (int)Move.newPosition.X + Coll.offsetX;
            Coll.rec.Y = (int)Move.newPosition.Y + Coll.offsetY;
        }

        public static void Align(ComponentAmountDisplay Display, ComponentSprite Sprite)
        {   //align display amount to sprite
            Display.amount.position.X = Sprite.position.X - 1;
            Display.amount.position.Y = Sprite.position.Y - 4;
            Display.bkg.X = (int)Display.amount.position.X - 1;
            Display.bkg.Y = (int)Display.amount.position.Y + 4;
        }

        public static void UpdateAmount(ComponentAmountDisplay Display, int Value)
        {   //clip Value to 99
            if (Value > 99) { Value = 99; }
            //prefix a 0 if Value is less than 10
            if (Value < 10) { Display.amount.text = "0" + Value; }
            else { Display.amount.text = "" + Value; }
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

        public static void CenterText(ComponentText Text, SpriteFont Font, int X)
        {   //center the text to the X and Y position passed in, prevent half pixel offsets
            textWidth = (int)Font.MeasureString(Text.text).X;
            Text.position.X = (int)X - (textWidth / 2);
        }



        public static void CreateScrollColumn(Boolean Bkg, Boolean Flip, int Height, Vector2 Pos, List<ComponentSprite> List)
        {
            byte xFrame = 6;
            if (Bkg) { xFrame = 14; }
            for (int i = 0; i < Height; i++)
            {   //create a blank sprite
                ComponentSprite sprite = new ComponentSprite(
                    Assets.mainSheet, new Vector2(0, 0),
                    new Byte4(0, 0, 0, 0), new Point(32, 16));
                if (Bkg) { sprite.cellSize.X = 16; }
                UpdateCellSize(sprite);
                sprite.position.X = Pos.X; //set blank's pos
                sprite.position.Y = Pos.Y + (16 * i);
                //set sprite's frame
                if (i == 0) { sprite.currentFrame = new Byte4(xFrame, 0, 0, 0); } //head
                else if (i == Height - 1) { sprite.currentFrame = new Byte4(xFrame, 2, 0, 0); } //tail
                else { sprite.currentFrame = new Byte4(xFrame, 1, 0, 0); } //mid section
                sprite.flipHorizontally = Flip;
                List.Add(sprite); //add mod blank to list
            }
        }

    }
}