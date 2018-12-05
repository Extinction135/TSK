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

        public static void Align(Actor Actor) { Align(Actor.compMove, Actor.compSprite, Actor.compCollision); }
        public static void Align(Projectile Pro) { Align(Pro.compMove, Pro.compSprite, Pro.compCollision); }
        public static void Align(Particle Part)
        {
            //particles dont have hitboxes
            Part.compSprite.position.X = (int)Part.compMove.newPosition.X;
            Part.compSprite.position.Y = (int)Part.compMove.newPosition.Y;
            SetZdepth(Part.compSprite);
        }
        public static void Align(IndestructibleObject Obj) { Align(Obj.compSprite, Obj.compCollision); }
        public static void Align(InteractiveObject Obj) { Align(Obj.compMove, Obj.compSprite, Obj.compCollision); }

        public static void Align(ComponentMovement Move, ComponentSprite Sprite, ComponentCollision Coll)
        {   //aligns the collision component and sprite component to the move component's newPosition
            Sprite.position.X = (int)Move.newPosition.X;
            Sprite.position.Y = (int)Move.newPosition.Y;
            SetZdepth(Sprite);
            Coll.rec.X = (int)Move.newPosition.X + Coll.offsetX;
            Coll.rec.Y = (int)Move.newPosition.Y + Coll.offsetY;
        }

        public static void Align(ComponentSprite Sprite, ComponentCollision Coll)
        {   //aligns the collision component to sprite component
            SetZdepth(Sprite);
            Coll.rec.X = (int)Sprite.position.X + Coll.offsetX;
            Coll.rec.Y = (int)Sprite.position.Y + Coll.offsetY;
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
            Sprite.origin.X = Sprite.drawRec.Width * 0.5f;
            Sprite.origin.Y = Sprite.drawRec.Height * 0.5f;
        }

        public static void SetZdepth(ComponentSprite Sprite)
        {
            Sprite.zDepth = 0.999990f - (Sprite.position.Y + Sprite.zOffset) * 0.000001f;
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

        public static void SetSpriteRotation(ComponentSprite Sprite, Direction Direction)
        {   //this assumes the sprite was designed facing down in the spritesheet
            if (Direction == Direction.Up) { Sprite.rotation = Rotation.Clockwise180; }
            else if (Direction == Direction.Right) { Sprite.rotation = Rotation.Clockwise270; }
            else if (Direction == Direction.Left) { Sprite.rotation = Rotation.Clockwise90; }
            else { Sprite.rotation = Rotation.None; }
        }

        public static void MoveQuadBkg(List<ComponentSprite> bkgList, int Xpos, int Ypos)
        {
            bkgList[0].position.X = Xpos;
            bkgList[0].position.Y = Ypos;

            bkgList[1].position.X = Xpos + 16;
            bkgList[1].position.Y = Ypos;
            bkgList[1].flipHorizontally = true;

            bkgList[2].position.X = Xpos;
            bkgList[2].position.Y = Ypos + 16;
            bkgList[2].flipHorizontally = true;

            bkgList[3].position.X = Xpos + 16;
            bkgList[3].position.Y = Ypos + 16;

            bkgList[2].rotation = Rotation.Clockwise180;
            bkgList[3].rotation = Rotation.Clockwise180;
        }

    }
}