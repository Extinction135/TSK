﻿using System;
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

        

    }
}