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
    public static class Functions_Debug
    {
        public static string output;
        static int i;

        public static void Inspect()
        {   //check if any object or actor collide with cursor collision component, pass to Inspect()
            //check mouse click position for any room objects
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active)
                {
                    if (Input.cursorColl.rec.Intersects(Pool.roomObjPool[i].compCollision.rec))
                    { Inspect(Pool.roomObjPool[i]); }
                }
            }
            //check mouse click position for any projectiles
            for (i = 0; i < Pool.projectileCount; i++)
            {
                if (Pool.projectilePool[i].active)
                {
                    if (Input.cursorColl.rec.Intersects(Pool.projectilePool[i].compCollision.rec))
                    { Inspect(Pool.projectilePool[i]); }
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

        public static void Inspect(ComponentInteraction Interaction)
        {
            output = "Component Interaction\n";
            output += "\tactive:" + Interaction.active;
            Debug.WriteLine(output);
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
            output += "\tmoving:" + Move.moving;
            output += "\tmagnitudeX:" + Move.magnitude.X;
            output += "\tmagnitudeY:" + Move.magnitude.Y;
            output += "\tspeed:" + Move.speed;
            output += "\tfriction:" + Move.friction;
            output += "\n";
            output += "\tmoveable:" + Move.moveable;
            output += "\tgrounded:" + Move.grounded;
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
            output += "\tvisible:" + Sprite.visible;
            output += "\torigin:" + Sprite.origin.X + ", " + Sprite.origin.Y;
            output += "\n";
            output += "\tspriteEffect:" + Sprite.spriteEffect;
            output += "\tflipHorizontally:" + Sprite.flipHorizontally;
            output += "\trotation:" + Sprite.rotation;
            output += "\trotation value:" + Sprite.rotationValue;
            Debug.WriteLine(output);
        }







        public static void InspectBottle(Byte value)
        {
            output += "\n  bottle contents: ";
            if (value == 0) { output += "locked"; }
            else if (value == 1) { output += "empty"; }
            else if (value == 2) { output += "health"; }
            else if (value == 3) { output += "magic"; }
            else if (value == 4) { output += "fairy"; }
        }

        public static void Inspect(SaveData SaveData)
        {
            output = "\n\n---- SAVE DATA DUMP ----";
            output += "\n  gold: " + PlayerData.current.gold;
            output += "\n  hearts: " + PlayerData.current.heartsTotal;
            output += "\n  magic: " + PlayerData.current.magicCurrent + " / " + PlayerData.current.magicTotal;
            output += "\n  bombs: " + PlayerData.current.bombsCurrent + " / " + PlayerData.current.bombsMax;
            output += "\n  arrows: " + PlayerData.current.arrowsCurrent + " / " + PlayerData.current.arrowsMax;

            output += "\n  -- Items --"; ;
            output += "\n  has boomerang: " + PlayerData.current.itemBoomerang;

            output += "\n  -- Bottles --"; ;
            output += "bottleA: " + PlayerData.current.bottleA;
            output += "bottleB: " + PlayerData.current.bottleB;
            output += "bottleC: " + PlayerData.current.bottleC;


            output += "\n  -- Magic Items --"; ;
            output += "\n  has magicFireball: " + PlayerData.current.magicFireball;

            output += "\n  -- Weapons --"; ;
            output += "\n  has Bow: " + PlayerData.current.weaponBow;
            output += "\n  has Net: " + PlayerData.current.weaponNet;

            output += "\n  -- Armor --"; ;
            output += "\n  has armorCape: " + PlayerData.current.armorCape;

            output += "\n  -- Equipment --"; ;
            output += "\n  has equipmentRing: " + PlayerData.current.equipmentRing;

            Debug.WriteLine(output);
        }

        public static void InspectRoomData()
        {
            String output = "";
            output += "" + Assets.roomDataBoss.Count + "boss..";
            output += "" + Assets.roomDataColumn.Count + "clmn..";
            output += "" + Assets.roomDataHub.Count + "hub..";
            output += "" + Assets.roomDataKey.Count + "key..";
            output += "" + Assets.roomDataRow.Count + "row..";
            output += "" + Assets.roomDataSquare.Count + "sqr..";
            output += "total: " +
                (Assets.roomDataBoss.Count + Assets.roomDataColumn.Count +
                Assets.roomDataHub.Count + Assets.roomDataKey.Count +
                Assets.roomDataRow.Count + Assets.roomDataSquare.Count);
            Debug.WriteLine(output);
        }


        public static void HandleTopMenuInput()
        {
            
            #region F1 - Toggle Collision Rec Drawing

            if (Functions_Input.IsNewKeyPress(Keys.F1))
            {   //toggle draw collision boolean
                if (Flags.DrawCollisions) { Flags.DrawCollisions = false; }
                else { Flags.DrawCollisions = true; }
            }

            #endregion


            #region F2 - Max Gold

            if (Functions_Input.IsNewKeyPress(Keys.F2))
            {   //set the player's gold to 99
                PlayerData.current.gold = 99;
                Assets.Play(Assets.sfxGoldPickup);
            }

            #endregion


            #region F3 - Dump SaveData to Output

            if (Functions_Input.IsNewKeyPress(Keys.F3))
            {   //dump savedata
                Inspect(PlayerData.current);
            }

            #endregion


            #region F4 - Toggle Drawing of InfoPanel

            if (Functions_Input.IsNewKeyPress(Keys.F4))
            {  
                if (Flags.DrawDebugInfo) { Flags.DrawDebugInfo = false; }
                else { Flags.DrawDebugInfo = true; }
            }

            #endregion


            #region F5 - Toggle Paused flag

            if (Functions_Input.IsNewKeyPress(Keys.F5))
            {
                if (Flags.Paused) { Flags.Paused = false; }
                else { Flags.Paused = true; }
            }

            #endregion


            #region F6 - Damage all active enemies

            if (Functions_Input.IsNewKeyPress(Keys.F6))
            {
                for (i = 1; i < Pool.actorCount; i++) //skip actorPool[0] (hero)
                {
                    if (Pool.actorPool[i].active) //deal 1 point of damage to all active actors
                    { Functions_Battle.Damage(Pool.actorPool[i], 1, 0.0f, Direction.Down); }
                }
            }

            #endregion


            #region F7 - Toggle Map Cheat & Map

            if (Functions_Input.IsNewKeyPress(Keys.F7))
            {
                if (Flags.MapCheat) //turn off mapCheat, take away map
                { Flags.MapCheat = false; Level.map = false; }
                else //turn on mapCheat, give hero map
                { Flags.MapCheat = true; Level.map = true; }
            }

            #endregion


            #region F8 - Iterate Hero's ActorType

            if (Functions_Input.IsNewKeyPress(Keys.F8))
            {
                if (Pool.hero.type == ActorType.Hero)
                { Functions_Actor.SetType(Pool.hero, ActorType.Blob); }
                else if (Pool.hero.type == ActorType.Blob)
                { Functions_Actor.SetType(Pool.hero, ActorType.Hero); }
                //default back to hero for all other states
                else { Functions_Actor.SetType(Pool.hero, ActorType.Hero); }
            }

            #endregion



            #region Set button colors based on what they represent

            if (Flags.DrawCollisions)
            { DebugMenu.buttons[0].currentColor = Assets.colorScheme.buttonDown; }
            else { DebugMenu.buttons[0].currentColor = Assets.colorScheme.buttonUp; }

            if (Flags.DrawDebugInfo)
            { DebugMenu.buttons[3].currentColor = Assets.colorScheme.buttonDown; }
            else { DebugMenu.buttons[3].currentColor = Assets.colorScheme.buttonUp; }

            if (Flags.Paused)
            { DebugMenu.buttons[4].currentColor = Assets.colorScheme.buttonDown; }
            else { DebugMenu.buttons[4].currentColor = Assets.colorScheme.buttonUp; }

            if (Flags.MapCheat)
            { DebugMenu.buttons[6].currentColor = Assets.colorScheme.buttonDown; }
            else { DebugMenu.buttons[6].currentColor = Assets.colorScheme.buttonUp; }

            #endregion



            //if user ctrl+LMB clicks, dump info on clicked actor/obj
            if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
            {   //user must hold down ctrl button to call Inspect()
                if (Functions_Input.IsKeyDown(Keys.LeftControl)) { Inspect(); }
            }
            //dump the states for every active actor if Enter key is pressed
            if (Functions_Input.IsNewKeyPress(Keys.Enter))
            {
                for (Pool.actorCounter = 0; Pool.actorCounter < Pool.actorCount; Pool.actorCounter++)
                {
                    if (Pool.actorPool[Pool.actorCounter].active)
                    { Inspect(Pool.actorPool[Pool.actorCounter]); }
                }
            }
        }

    }
}