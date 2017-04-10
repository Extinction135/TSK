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
    public static class DebugFunctions
    {

        public static void Inspect(Actor Actor)
        {
            Debug.WriteLine("---- Object ----");
            Debug.WriteLine("\tobjGroup: " + Actor.type);
            Debug.WriteLine("\tstate: " + Actor.state);
            Debug.WriteLine("\tinputState: " + Actor.inputState);

            Debug.WriteLine("\tstateLocked: " + Actor.stateLocked);
            Debug.WriteLine("\tlockTotal: " + Actor.lockTotal);
            Debug.WriteLine("\tlockCounter: " + Actor.lockCounter);

            Debug.WriteLine("\tanimList: " + Actor.animList);
            Debug.WriteLine("\tanimGroup: " + Actor.animGroup);
            Debug.WriteLine("\tdirection: " + Actor.direction);
            Debug.WriteLine("\tactive: " + Actor.active);

            Debug.WriteLine("\tdashSpeed: " + Actor.dashSpeed);
            Debug.WriteLine("\twalkSpeed: " + Actor.walkSpeed);

            //dump component info
            Inspect(Actor.compCollision);
            Inspect(Actor.compMove);
            Inspect(Actor.compAnim);
            Inspect(Actor.compInput);
            Inspect(Actor.compSprite);
        }

        public static void Inspect(GameObject Obj)
        {
            Debug.WriteLine("---- Object ----");
            Debug.WriteLine("\tobjGroup: " + Obj.objGroup);
            Debug.WriteLine("\ttype: " + Obj.type);
            Debug.WriteLine("\tdirection: " + Obj.direction);
            Debug.WriteLine("\tactive: " + Obj.active);
            Debug.WriteLine("\tlifetime: " + Obj.lifetime);
            Debug.WriteLine("\tlifeCounter: " + Obj.lifeCounter);

            //dump component info
            Inspect(Obj.compCollision);
            Inspect(Obj.compMove);
            Inspect(Obj.compAnim);
            Inspect(Obj.compSprite);
        }

        public static void Inspect(ComponentCollision Coll)
        {
            Debug.WriteLine("Component: Collision");
            Debug.WriteLine("\trecX: " + Coll.rec.X);
            Debug.WriteLine("\trecY: " + Coll.rec.Y);
            Debug.WriteLine("\trecWidth: " + Coll.rec.Width);
            Debug.WriteLine("\trecHeight: " + Coll.rec.Height);
            Debug.WriteLine("\toffsetX: " + Coll.offsetX);
            Debug.WriteLine("\toffsetY: " + Coll.offsetY);
            Debug.WriteLine("\tblocking: " + Coll.blocking);
            Debug.WriteLine("\tactive: " + Coll.active);
        }

        public static void Inspect(ComponentMovement Move)
        {
            Debug.WriteLine("Component: Movement");
            Debug.WriteLine("\tposX: " + Move.position.X);
            Debug.WriteLine("\tposY: " + Move.position.Y);
            Debug.WriteLine("\tnewPosX: " + Move.newPosition.X);
            Debug.WriteLine("\tnewPosY: " + Move.newPosition.Y);
            Debug.WriteLine("\tdirection: " + Move.direction);
            Debug.WriteLine("\tmagnitude: " + Move.magnitude);
            Debug.WriteLine("\tspeed: " + Move.speed);
            Debug.WriteLine("\tfriction: " + Move.friction);
        }

        public static void Inspect(ComponentAnimation Anim)
        {
            Debug.WriteLine("Component: Animation");
            Debug.WriteLine("\tanim count: " + Anim.currentAnimation.Count);
            Debug.WriteLine("\tindex: " + Anim.index);
            Debug.WriteLine("\tspeed: " + Anim.speed);
            Debug.WriteLine("\ttimer: " + Anim.timer);
            Debug.WriteLine("\tloop: " + Anim.loop);
        }

        public static void Inspect(ComponentInput Input)
        {
            Debug.WriteLine("Component: Input");
            Debug.WriteLine("\tdirection: " + Input.direction);
            Debug.WriteLine("\tattack: " + Input.attack);
            Debug.WriteLine("\tuse: " + Input.use);
            Debug.WriteLine("\tdash: " + Input.dash);
            Debug.WriteLine("\tinteract: " + Input.interact);
        }

        public static void Inspect(ComponentSprite Sprite)
        {
            Debug.WriteLine("Component: Sprite");
            Debug.WriteLine("\ttexture: " + Sprite.texture.Name);
            Debug.WriteLine("\tposX: " + Sprite.position.X);
            Debug.WriteLine("\tposY: " + Sprite.position.Y);

            Debug.WriteLine("\tcurrentFrame: " + Sprite.currentFrame.x + ", " + Sprite.currentFrame.y + ", " + Sprite.currentFrame.flipHori);
            Debug.WriteLine("\tcellSize: " + Sprite.cellSize.x + ", " + Sprite.cellSize.y);
            Debug.WriteLine("\tspriteEffect: " + Sprite.spriteEffect);
            Debug.WriteLine("\tflipHorizontally: " + Sprite.flipHorizontally);
            Debug.WriteLine("\tvisible: " + Sprite.visible);
            Debug.WriteLine("\torigin: " + Sprite.origin.X + ", " + Sprite.origin.Y);
        }

    }
}