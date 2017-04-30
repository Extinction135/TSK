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

        public static void StopMovement(ComponentMovement Move)
        {
            Move.magnitude.X = 0;
            Move.magnitude.Y = 0;
            Move.speed = 0;
        }

        public static void CenterOrigin(ComponentSprite Sprite)
        {
            Sprite.origin.X = Sprite.cellSize.x * 0.5f;
            Sprite.origin.Y = Sprite.cellSize.y * 0.5f;
        }

        public static void SetZdepth(ComponentSprite Sprite)
        {
            Sprite.zDepth = 0.999990f - (Sprite.position.Y + Sprite.zOffset) * 0.000001f;
        }

        public static void UpdateCellSize(ComponentSprite Sprite)
        {
            Sprite.drawRec.Width = Sprite.cellSize.x;
            Sprite.drawRec.Height = Sprite.cellSize.y;
        }

        public static void CenterText(ComponentButton Button)
        {   //measure width of the button's text
            Button.textWidth = (int)Assets.font.MeasureString(Button.compText.text).X;
            //resize button to fit around the text
            Button.rec.Width = Button.textWidth + 6;
            //center text to button
            Button.compText.position.X = (Button.rec.Location.X + Button.rec.Width / 2) - (Button.textWidth / 2);
            //if the textWidth is odd, it blurs, so add half pixel to keep it sharp
            if (Button.textWidth % 2 != 0) { Button.compText.position.X += 0.5f; }
            //center text vertically
            Button.compText.position.Y = Button.rec.Location.Y - 3;
        }

        public static Direction GetOppositeDirection(Direction Direction)
        {
            if (Direction == Direction.Down) { return Direction.Up; }
            else if (Direction == Direction.Up) { return Direction.Down; }
            else if (Direction == Direction.Left) { return Direction.Right; }
            else if (Direction == Direction.UpLeft) { return Direction.DownRight; }
            else if (Direction == Direction.DownLeft) { return Direction.UpRight; }
            else if (Direction == Direction.Right) { return Direction.Left; }
            else if (Direction == Direction.DownRight) { return Direction.UpLeft; }
            else if (Direction == Direction.UpRight) { return Direction.DownLeft; }
            return Direction.None;
        }

    }
}