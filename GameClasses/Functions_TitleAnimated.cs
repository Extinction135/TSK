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
    public static class Functions_TitleAnimated
    {

        public static void SetText(TitleAnimated Title, TitleText Text)
        {
            if (Text == TitleText.Dungeon)
            {
                Title.compSprite.cellSize.X = 16 * 13;
                Title.compSprite.currentFrame.X = 0;
                Title.compSprite.currentFrame.Y = 0;
            }
            else if (Text == TitleText.Complete)
            {
                Title.compSprite.cellSize.X = 16 * 13;
                Title.compSprite.currentFrame.X = 0;
                Title.compSprite.currentFrame.Y = 1;
            }
            else if (Text == TitleText.You)
            {
                Title.compSprite.cellSize.X = 16 * 8;
                Title.compSprite.currentFrame.X = 0;
                Title.compSprite.currentFrame.Y = 2;
            }
            else if (Text == TitleText.Died)
            {
                Title.compSprite.cellSize.X = 16 * 8;
                Title.compSprite.currentFrame.X = 1;
                Title.compSprite.currentFrame.Y = 2;
            }
            else if (Text == TitleText.Run)
            {
                Title.compSprite.cellSize.X = 16 * 8;
                Title.compSprite.currentFrame.X = 0;
                Title.compSprite.currentFrame.Y = 3;
            }
            Functions_Component.UpdateCellSize(Title.compSprite);
        }

        public static void Reset(TitleAnimated Title)
        {
            Title.compSprite.position = Title.startPos;
            Title.displayState = DisplayState.Opening;
        }

        static void Move(TitleAnimated Title, Vector2 TargetPos)
        {
            if (Title.compSprite.position.X < TargetPos.X)
            {   //move sprite right to target
                Title.compSprite.position.X += (TargetPos.X - Title.compSprite.position.X) / Title.animSpeed;
                Title.compSprite.position.X += 1; //always move at least 1 pixel
            }
            if (Title.compSprite.position.X > TargetPos.X)
            {   //move sprite left to target
                Title.compSprite.position.X -= (Title.compSprite.position.X - TargetPos.X) / Title.animSpeed;
                Title.compSprite.position.X -= 1; //always move at least 1 pixel
            }
            if (Math.Abs(Title.compSprite.position.X - TargetPos.X) < 1)
            { Title.compSprite.position.X = TargetPos.X; } //if close, clip to target
        }

        public static void AnimateMovement(TitleAnimated Title)
        {
            if (Title.displayState == DisplayState.Opening)
            {   //move sprite towards endPos
                Move(Title, Title.endPos);
                if (Title.compSprite.position.X == Title.endPos.X)
                { Title.displayState = DisplayState.Opened; }
            }
            else if(Title.displayState == DisplayState.Closing)
            {   //move sprite towards startPos
                Move(Title, Title.startPos);
                if (Title.compSprite.position.X == Title.startPos.X) 
                { Title.displayState = DisplayState.Closed; }
            }
        }
        
    }
}