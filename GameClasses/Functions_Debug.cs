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
            int total = 0;
            String output = "";
            output += "" + RoomData.bossRooms.Count + "boss.."; total += RoomData.bossRooms.Count;
            output += "" + RoomData.columnRooms.Count + "clmn.."; total += RoomData.columnRooms.Count;
            output += "" + RoomData.hubRooms.Count + "hub.."; total += RoomData.hubRooms.Count;
            output += "" + RoomData.keyRooms.Count + "key.."; total += RoomData.keyRooms.Count;
            output += "" + RoomData.rowRooms.Count + "row.."; total += RoomData.rowRooms.Count;
            output += "" + RoomData.squareRooms.Count + "sqr.."; total += RoomData.squareRooms.Count;
            output += "total: " + total;
            Debug.WriteLine(output);
        }



        public static void Draw()
        {

            #region Draw Frame Times and Ram useage

            //average over framesTotal
            DebugInfo.timingText.text = "u: " + DebugInfo.updateAvg;
            DebugInfo.timingText.text += "\nd: " + DebugInfo.drawAvg;
            DebugInfo.timingText.text += "\nt: " + Timing.totalTime.Milliseconds + " ms";
            DebugInfo.timingText.text += "\n" + ScreenManager.gameTime.TotalGameTime.ToString(@"hh\:mm\:ss");
            DebugInfo.timingText.text += "\n" + Functions_Backend.GetRam() + " mb";

            #endregion


            #region Actor + Movement Components

            DebugInfo.actorText.text = "actor: hero";
            DebugInfo.actorText.text += "\ninp: " + Pool.hero.inputState;
            DebugInfo.actorText.text += "\ncur: " + Pool.hero.state;
            DebugInfo.actorText.text += "\nlck: " + Pool.hero.stateLocked;
            DebugInfo.actorText.text += "\ndir: " + Pool.hero.direction;

            DebugInfo.moveText.text = "pos x:" + Pool.hero.compSprite.position.X + ", y:" + Pool.hero.compSprite.position.Y;
            DebugInfo.moveText.text += "\nspd:" + Pool.hero.compMove.speed + "  fric:" + Pool.hero.compMove.friction;
            DebugInfo.moveText.text += "\nmag x:" + Pool.hero.compMove.magnitude.X;
            DebugInfo.moveText.text += "\nmag y:" + Pool.hero.compMove.magnitude.Y;
            DebugInfo.moveText.text += "\ndir: " + Pool.hero.compMove.direction;

            #endregion


            #region Pool, Creation Time, and Record Components

            DebugInfo.poolText.text = "floors: " + Pool.floorIndex + "/" + Pool.floorCount;

            DebugInfo.poolText.text += "\nobjs: " + Pool.roomObjIndex + "/" + Pool.roomObjCount;
            DebugInfo.poolText.text += "\nactrs: " + Pool.actorIndex + "/" + Pool.actorCount;
            DebugInfo.poolText.text += "\nprojs: " + Pool.projectileIndex + "/" + Pool.projectileCount;

            DebugInfo.creationText.text = "timers";
            DebugInfo.creationText.text += "\nroom: " + DebugInfo.roomTime;
            DebugInfo.creationText.text += "\ndung: " + DebugInfo.dungeonTime;

            DebugInfo.recordText.text = "record";
            DebugInfo.recordText.text += "\ntime: " + DungeonRecord.timer.Elapsed.ToString(@"hh\:mm\:ss");
            DebugInfo.recordText.text += "\nenemies: " + DungeonRecord.enemyCount;
            DebugInfo.recordText.text += "\ndamage: " + DungeonRecord.totalDamage;

            #endregion


            #region Music + SaveData Components

            DebugInfo.musicText.text = "music: " + Functions_Music.trackToLoad;
            DebugInfo.musicText.text += "\n" + Functions_Music.currentMusic.State + ": " + Functions_Music.currentMusic.Volume;
            DebugInfo.musicText.text += "\n" + Assets.musicDrums.State + ": " + Assets.musicDrums.Volume;

            //saveDataText.text = "save data";
            //saveDataText.text += "\ngold: " + PlayerData.saveData.gold;

            #endregion


            ScreenManager.spriteBatch.Draw(Assets.dummyTexture, DebugInfo.background, Assets.colorScheme.debugBkg);
            DebugInfo.size = DebugInfo.textFields.Count();
            for (DebugInfo.counter = 0; DebugInfo.counter < DebugInfo.size; DebugInfo.counter++)
            { Functions_Draw.Draw(DebugInfo.textFields[DebugInfo.counter]); }
        }



    }
}