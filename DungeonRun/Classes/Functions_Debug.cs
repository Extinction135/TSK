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
    public static class Functions_Debug
    {
        public static string output;
        static int i;

        public static void Inspect()
        {   //check if any object or actor collide with cursor collision component, pass to Inspect()
            //check mouse click position for any objects
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active)
                {
                    if (Input.cursorColl.rec.Intersects(Pool.roomObjPool[i].compCollision.rec))
                    { Inspect(Pool.roomObjPool[i]); }
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
            output += "\tcurrentFrame:" + Sprite.currentFrame.X + ", " + Sprite.currentFrame.Y + ", " + Sprite.currentFrame.flipHori;
            output += "\tcellSize:" + Sprite.cellSize.X + ", " + Sprite.cellSize.Y;
            output += "\n";
            output += "\tspriteEffect:" + Sprite.spriteEffect;
            output += "\tflipHorizontally:" + Sprite.flipHorizontally;
            output += "\tvisible:" + Sprite.visible;
            output += "\torigin:" + Sprite.origin.X + ", " + Sprite.origin.Y;
            Debug.WriteLine(output);
        }

        public static void Inspect(SaveData SaveData)
        {
            output = "\n\n---- SAVE DATA DUMP ----";
            output += "\n  gold: " + PlayerData.saveData.gold;
            output += "\n  heart pieces: " + PlayerData.saveData.heartPieces;
            output += "\n  magic: " + PlayerData.saveData.magicCurrent + " / " + PlayerData.saveData.magicTotal;
            output += "\n  bombs: " + PlayerData.saveData.bombsCurrent + " / " + PlayerData.saveData.bombsMax;
            output += "\n  arrows: " + PlayerData.saveData.arrowsCurrent + " / " + PlayerData.saveData.arrowsMax;

            output += "\n  -- Items --"; ;
            output += "\n  has boomerang: " + PlayerData.saveData.itemBoomerang;

            output += "\n  has bottle1: " + PlayerData.saveData.bottle1;
            output += "\n  has bottle2: " + PlayerData.saveData.bottle2;
            output += "\n  has bottle3: " + PlayerData.saveData.bottle3;

            output += "\n  has bottleHealth: " + PlayerData.saveData.bottleHealth;
            output += "\n  has bottleMagic: " + PlayerData.saveData.bottleMagic;
            output += "\n  has bottleFairy: " + PlayerData.saveData.bottleFairy;

            output += "\n  has magicFireball: " + PlayerData.saveData.magicFireball;

            output += "\n  has weaponBow: " + PlayerData.saveData.weaponBow;

            output += "\n  has armorChest: " + PlayerData.saveData.armorChest;
            output += "\n  has armorCape: " + PlayerData.saveData.armorCape;
            output += "\n  has armorRobe: " + PlayerData.saveData.armorRobe;

            output += "\n  has equipmentRing: " + PlayerData.saveData.equipmentRing;

            Debug.WriteLine(output);
        }



        public static void HandleDebugMenuInput()
        {
            //dump the states for every active actor if Enter key is pressed
            if (Functions_Input.IsNewKeyPress(Keys.Enter))
            {
                for (Pool.counter = 0; Pool.counter < Pool.actorCount; Pool.counter++)
                {
                    if (Pool.actorPool[Pool.counter].active)
                    { Inspect(Pool.actorPool[Pool.counter]); }
                }
            }
            //check each button for user mouse button input (click + hover states)
            for (DebugMenu.counter = 0; DebugMenu.counter < DebugMenu.buttons.Count; DebugMenu.counter++)
            {   //check each button to see if it contains the cursor
                if (DebugMenu.buttons[DebugMenu.counter].rec.Contains(Input.cursorPos))
                {   //any button containing the cursor draws with 'over' color
                    DebugMenu.buttons[DebugMenu.counter].currentColor = Assets.colorScheme.buttonOver;
                    if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
                    {   //any button clicked on becomes selected


                        #region Button Events

                        if (DebugMenu.counter == 0) //toggle draw collisions boolean
                        {
                            if (Flags.DrawCollisions) { Flags.DrawCollisions = false; }
                            else { Flags.DrawCollisions = true; }
                            //match the draw collisions boolean for the selected state
                            DebugMenu.buttons[DebugMenu.counter].selected = Flags.DrawCollisions;
                        }
                        else if (DebugMenu.counter == 1) //build the dungeon room again
                        {   //build dungeon based on the last dungeon type
                            Functions_Dungeon.BuildDungeon(Functions_Dungeon.dungeon.type);
                        }
                        else if (DebugMenu.counter == 2) //set the player's gold to 99
                        {
                            PlayerData.saveData.gold = 99;
                            Assets.Play(Assets.sfxGoldPickup);
                        }
                        else if (DebugMenu.counter == 3) //dump saveData to output
                        { Inspect(PlayerData.saveData); }

                        else if (DebugMenu.counter == 4) //room builder
                        {
                            //exit all screens
                            //create room builder screen
                        }

                        #endregion


                    }
                } //buttons not touching cursor return to button up color
                else { DebugMenu.buttons[DebugMenu.counter].currentColor = Assets.colorScheme.buttonUp; }
                //selected buttons get the button down color
                if (DebugMenu.buttons[DebugMenu.counter].selected)
                { DebugMenu.buttons[DebugMenu.counter].currentColor = Assets.colorScheme.buttonDown; }
            }
        }

    }
}