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
        public static string output;
        static int i;

        public static void Inspect()
        {   //check if any object or actor collide with cursor collision component, pass to Inspect()
            //check mouse click position for any objects
            for (i = 0; i < Pool.objCount; i++)
            {
                if (Pool.objPool[i].active)
                {
                    if (Input.cursorColl.rec.Intersects(Pool.objPool[i].compCollision.rec))
                    { Inspect(Pool.objPool[i]); }
                }
            }
            //check mouse click position for any actors
            for (i = 0; i < Pool.actorCount; i++)
            {
                if (Pool.actorPool[i].active)
                {
                    if (Input.cursorColl.rec.Intersects(Pool.actorPool[i].compCollision.rec))
                    { Inspect(Pool.actorPool[i]); }
                }
            }
        }

        public static void Inspect(Actor Actor)
        {
            output = "\n\n\n---- Actor (objGroup:" + Actor.type + ") ----\n";
            output += "\tstate:" + Actor.state;
            output += "\tinputState:" + Actor.inputState;
            output += "\tstateLocked:" + Actor.stateLocked;
            output += "\tlockTotal:" + Actor.lockTotal;
            output += "\tlockCounter:" + Actor.lockCounter;
            output += "\n";
            output += "\tdirection:" + Actor.direction;
            output += "\tactive:" + Actor.active;
            output += "\tdashSpeed:" + Actor.dashSpeed;
            output += "\twalkSpeed:" + Actor.walkSpeed;
            //output += "\n";
            //output += "\tanimList:" + Actor.animList;
            //output += "\tanimGroup:" + Actor.animGroup;
            Debug.WriteLine(output);

            //dump component info
            Inspect(Actor.compCollision);
            Inspect(Actor.compMove);
            Inspect(Actor.compAnim);
            Inspect(Actor.compInput);
            Inspect(Actor.compSprite);
        }

        public static void Inspect(GameObject Obj)
        {
            output = "\n\n\n---- Object (objGroup:" + Obj.group + ") (type:" + Obj.type + ") ----\n";
            output += "\tdirection:" + Obj.direction;
            output += "\tactive:" + Obj.active;
            output += "\tlifetime:" + Obj.lifetime;
            output += "\tlifeCounter:" + Obj.lifeCounter;
            Debug.WriteLine(output);

            //dump component info
            Inspect(Obj.compCollision);
            Inspect(Obj.compMove);
            Inspect(Obj.compAnim);
            Inspect(Obj.compSprite);
        }

        public static void Inspect(ComponentCollision Coll)
        {
            output = "Component Collision\n";
            output += "\trecX:" + Coll.rec.X;
            output += "\trecY:" + Coll.rec.Y;
            output += "\trecWidth:" + Coll.rec.Width;
            output += "\t\trecHeight:" + Coll.rec.Height;
            output += "\n";
            output += "\toffsetX:" + Coll.offsetX;
            output += "\toffsetY:" + Coll.offsetY;
            output += "\tblocking:" + Coll.blocking;
            output += "\tactive:" + Coll.active;
            Debug.WriteLine(output);
        }

        public static void Inspect(ComponentMovement Move)
        {
            output = "Component Movement (direction:" + Move.direction + ")\n";
            output += "\tposX:" + Move.position.X;
            output += "\tposY:" + Move.position.Y;
            output += "\tnewPosX:" + Move.newPosition.X;
            output += "\tnewPosY:" + Move.newPosition.Y;
            output += "\n";
            output += "\tmagnitudeX:" + Move.magnitude.X;
            output += "\tmagnitudeY:" + Move.magnitude.Y;
            output += "\tspeed:" + Move.speed;
            output += "\tfriction:" + Move.friction;
            Debug.WriteLine(output);
        }

        public static void Inspect(ComponentAnimation Anim)
        {
            output = "Component Animation\n";
            output += "\tanim count:" + Anim.currentAnimation.Count;
            output += "\tindex:" + Anim.index;
            output += "\tspeed:" + Anim.speed;
            output += "\ttimer:" + Anim.timer;
            output += "\tloop:" + Anim.loop;
            Debug.WriteLine(output);
        }

        public static void Inspect(ComponentInput Input)
        {
            output = "Component Input\n";
            output += "\tdirection:" + Input.direction;
            output += "\tattack:" + Input.attack;
            output += "\tuse:" + Input.use;
            output += "\tdash:" + Input.dash;
            output += "\tinteract:" + Input.interact;
            Debug.WriteLine(output);
        }

        public static void Inspect(ComponentSprite Sprite)
        {
            output = "Component Sprite (texture:" + Sprite.texture.Name + ")\n";

            output += "\tposX:" + Sprite.position.X;
            output += "\tposY:" + Sprite.position.Y;
            output += "\tcurrentFrame:" + Sprite.currentFrame.x + ", " + Sprite.currentFrame.y + ", " + Sprite.currentFrame.flipHori;
            output += "\tcellSize:" + Sprite.cellSize.x + ", " + Sprite.cellSize.y;
            output += "\n";
            output += "\tspriteEffect:" + Sprite.spriteEffect;
            output += "\tflipHorizontally:" + Sprite.flipHorizontally;
            output += "\tvisible:" + Sprite.visible;
            output += "\torigin:" + Sprite.origin.X + ", " + Sprite.origin.Y;
            Debug.WriteLine(output);
        }

    }
}