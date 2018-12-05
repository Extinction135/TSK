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
    public static class Functions_IndestructibleObjs
    {

        public static void Reset(IndestructibleObject IndObj)
        {
            //reset obj
            IndObj.group = IndestructibleGroup.Object;
            IndObj.type = IndestructibleType.Dungeon_BlockDark;
            IndObj.direction = Direction.Down;
            IndObj.active = true;
            IndObj.interacts = false; 
            IndObj.counter = 0;
            IndObj.selfCleans = false;

            //reset the sprite component
            IndObj.compSprite.drawRec.Width = 16 * 1; //assume cell size is 16x16 (most are)
            IndObj.compSprite.drawRec.Height = 16 * 1;
            IndObj.compSprite.zOffset = 0;
            IndObj.compSprite.flipHorizontally = false;
            IndObj.compSprite.rotation = Rotation.None;
            IndObj.compSprite.scale = 1.0f;
            IndObj.compSprite.texture = Assets.CommonObjsSheet;
            IndObj.compSprite.visible = true;

            //reset the animation component
            IndObj.compAnim.speed = 10; //set obj's animation speed to default value
            IndObj.compAnim.loop = true; //assume obj's animation loops
            IndObj.compAnim.index = 0; //reset the current animation index/frame
            IndObj.compAnim.timer = 0; //reset the elapsed frames

            //reset the collision component
            IndObj.compCollision.blocking = true; //assume the object is blocking (most are)
            IndObj.compCollision.rec.Width = 16; //assume collisionRec is 16x16
            IndObj.compCollision.rec.Height = 16; //(most are)
            IndObj.compCollision.offsetX = -8; //assume collisionRec offset is -8x-8
            IndObj.compCollision.offsetY = -8; //(most are)
        }

        static IndestructibleObject indObjRef;
        public static IndestructibleObject Spawn(IndestructibleType Type, int X, int Y, Direction Dir)
        {   //spawns obj at the X, Y location, with direction
            indObjRef = Functions_Pool.GetIndObj();
            indObjRef.direction = Dir;

            indObjRef.compSprite.position.X = X;
            indObjRef.compSprite.position.Y = Y;
            SetType(indObjRef, Type);
            return indObjRef;
        }

        public static void Kill(IndestructibleObject IndObj)
        {
            Functions_Pool.Release(IndObj);
        }









        public static void Update(IndestructibleObject IndObj)
        {
            if(IndObj.interacts)
            {
                //perform indestructible obj interactions here



                //coliseum objects

                #region Colliseum Spectators

                if (IndObj.type == IndestructibleType.Coliseum_Shadow_Spectator)
                {   //randomly create an exclamation particle
                    if (Functions_Random.Int(0, 1000) > 995)
                    {
                        //set a random number to determine which spectator speaks
                        IndObj.interactiveFrame = Functions_Random.Int(0, 100);
                        //spawn on which spectator?
                        if (IndObj.interactiveFrame < 25)
                        {
                            //#1
                            Functions_Particle.Spawn(
                                ParticleType.ExclamationBubble,
                                IndObj.compSprite.position.X - 3,
                                IndObj.compSprite.position.Y - 16);
                        }
                        else if (IndObj.interactiveFrame < 50)
                        {
                            //#2
                            Functions_Particle.Spawn(
                                ParticleType.ExclamationBubble,
                                IndObj.compSprite.position.X - 3 + 16 * 1,
                                IndObj.compSprite.position.Y - 16);
                        }
                        else if (IndObj.interactiveFrame < 75)
                        {
                            //#3
                            Functions_Particle.Spawn(
                                ParticleType.ExclamationBubble,
                                IndObj.compSprite.position.X - 3 + 16 * 2,
                                IndObj.compSprite.position.Y - 16);
                        }
                        else
                        {
                            //#4
                            Functions_Particle.Spawn(
                                ParticleType.ExclamationBubble,
                                IndObj.compSprite.position.X - 3 + 16 * 3,
                                IndObj.compSprite.position.Y - 16);
                        }
                    }
                }

                #endregion



            }
        }




        public static void SetRotation(IndestructibleObject IndObj)
        {
            //set sprite's rotation based on direction & flipHorizontally boolean
            Functions_Component.SetSpriteRotation(IndObj.compSprite, IndObj.direction);
        }




        public static void SelfClean(IndestructibleObject IndObj)
        {
            //walls + doors self clean if they overlap exits
            //ref interactive objs's selfClean() method for wall vs exit routines
        }








        public static void SetType(IndestructibleObject IndObj, IndestructibleType Type)
        {   //Obj.direction should be set prior to this method running
            IndObj.type = Type;
            IndObj.compSprite.texture = Assets.CommonObjsSheet;
            //obj is either on common objs sheet, or dungeon sheet
            IndObj.compCollision.blocking = true; //always blocks too



            #region Unknown Obj

            if (Type == IndestructibleType.Unknown)
            {
                Reset(IndObj);
                IndObj.type = IndestructibleType.Unknown;
                IndObj.compSprite.texture = Assets.uiItemsSheet;
                IndObj.compAnim.currentAnimation = AnimationFrames.Ui_MenuItem_Unknown;
                IndObj.compSprite.zOffset = -64; //sort below everything else
            }

            #endregion




            //dungeon only objs

            #region Exits

            else if (Type == IndestructibleType.Dungeon_ExitPillarLeft ||
               Type == IndestructibleType.Dungeon_ExitPillarRight)
            {
                IndObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IndObj.compSprite.drawRec.Height = 16 * 3; //nonstandard size
                IndObj.compSprite.zOffset = -32; //sort to floor
                IndObj.group = IndestructibleGroup.Exit;
                IndObj.compCollision.rec.Height = 32 - 5;
                IndObj.compCollision.offsetY = 14;
                if (Type == IndestructibleType.Dungeon_ExitPillarLeft)
                { IndObj.compAnim.currentAnimation = AnimationFrames.Dungeon_ExitPillarLeft; }
                else { IndObj.compAnim.currentAnimation = AnimationFrames.Dungeon_ExitPillarRight; }
                //these objs clean themselves up in interactions, set this state
                IndObj.selfCleans = true;
            }
            else if (Type == IndestructibleType.Dungeon_Exit)
            {
                IndObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IndObj.compSprite.drawRec.Height = 16 * 3; //nonstandard size
                IndObj.compSprite.zOffset = -32; //sort to floor
                IndObj.group = IndestructibleGroup.Exit;
                IndObj.compCollision.rec.Height = 8;
                IndObj.compCollision.offsetY = 32;
                IndObj.compAnim.currentAnimation = AnimationFrames.Dungeon_Exit;
                //these objs clean themselves up in interactions, set this state
                IndObj.selfCleans = true;
            }
            else if (Type == IndestructibleType.Dungeon_ExitLight)
            {
                IndObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IndObj.compSprite.drawRec.Height = 16 * 2; //nonstandard size
                IndObj.compCollision.offsetY = 0;
                IndObj.compSprite.zOffset = 256; //sort above everything
                IndObj.compAnim.currentAnimation = AnimationFrames.Dungeon_ExitLight;
            }

            #endregion


            #region Blocks

            else if (Type == IndestructibleType.Dungeon_BlockDark)
            {
                IndObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IndObj.compSprite.zOffset = -7;
                IndObj.compAnim.currentAnimation = AnimationFrames.Dungeon_BlockDark; 
            }

            #endregion


            #region Statue & Pillar

            else if (Type == IndestructibleType.Dungeon_SkullPillar)
            {
                IndObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IndObj.compSprite.drawRec.Height = 16 * 2; //nonstandard size
                IndObj.compSprite.zOffset = +10;
                IndObj.compCollision.offsetY = +8;
                IndObj.compAnim.currentAnimation = AnimationFrames.Dungeon_SkullPillar;
            }

            #endregion


            


            







            //world/dungeon objs

            #region Trees

            else if (Type == IndestructibleType.Tree_Med)
            {
                IndObj.compSprite.zOffset = 8;
                IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Tree_Med;
                //double size
                IndObj.compSprite.drawRec.Width = 16 * 2;
                IndObj.compSprite.drawRec.Height = 16 * 2;
                IndObj.compCollision.rec.Width = 16 * 2 - 6;
                IndObj.compCollision.rec.Height = 14;
                IndObj.compCollision.offsetX = -8 + 3;
                IndObj.compCollision.offsetY = +8;
            }
            else if (Type == IndestructibleType.Tree_Med_Stump)
            {
                IndObj.compSprite.zOffset = 8;
                IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Tree_Med_Stump;
                //double size
                IndObj.compSprite.drawRec.Width = 16 * 2;
                IndObj.compSprite.drawRec.Height = 16 * 2;
                IndObj.compCollision.rec.Width = 16 * 2 - 6;
                IndObj.compCollision.rec.Height = 14;
                IndObj.compCollision.offsetX = -8 + 3;
                IndObj.compCollision.offsetY = +8;
            }
            else if (Type == IndestructibleType.Tree_Big)
            {
                IndObj.compSprite.zOffset = 16 * 2 + 4;
                IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Tree_Big;
                //double size
                IndObj.compSprite.drawRec.Width = 16 * 4;
                IndObj.compSprite.drawRec.Height = 16 * 5;
                //hitbox is custom
                IndObj.compCollision.rec.Width = 16 * 4 - 8;
                IndObj.compCollision.rec.Height = 16 * 2 + 10;
                IndObj.compCollision.offsetX = -8 + 4;
                IndObj.compCollision.offsetY = 16 * 2 - 5;
            }

            #endregion


            #region Big Shadow Cover

            else if (Type == IndestructibleType.Shadow_Big)
            {   //big decoration
                IndObj.compSprite.zOffset = 16 * 10; //sort over everything ominously
                //setup hitbox
                IndObj.compCollision.offsetX = -8; IndObj.compCollision.rec.Width = 16 * 3;
                IndObj.compCollision.offsetY = -8; IndObj.compCollision.rec.Height = 16 * 4;
                //setup animFrame
                IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Shadow_Big;
                IndObj.compSprite.drawRec.Width = 16 * 3; //nonstandard size
                IndObj.compSprite.drawRec.Height = 16 * 4; //nonstandard size
            }

            #endregion


            #region Water Objs

            else if (
                Type == IndestructibleType.Water_RockSm 
                || Type == IndestructibleType.Water_RockMed
                )
            {
                IndObj.compSprite.zOffset = -7; //sort under hero
                //setup hitbox
                IndObj.compCollision.offsetX = -5; IndObj.compCollision.rec.Width = 10;
                IndObj.compCollision.offsetY = -5; IndObj.compCollision.rec.Height = 10;
                //setup animFrame
                if (Type == IndestructibleType.Water_RockSm)
                { IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Water_RockSm; }
                else
                { IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Water_RockMed; }
            }

            else if (Type == IndestructibleType.Water_BigPlant)
            {   //nonstandard size
                IndObj.compSprite.drawRec.Width = 16 * 2; IndObj.compSprite.drawRec.Height = 16 * 2;
                IndObj.compCollision.rec.Width = 16 * 2; IndObj.compCollision.offsetX = -8;
                IndObj.compCollision.rec.Height = 16 * 2; IndObj.compCollision.offsetY = -8;
                IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Swamp_BigPlant;
                IndObj.compSprite.zOffset = 6; //has height
            }
            else if (Type == IndestructibleType.Water_Bulb)
            {
                IndObj.compCollision.rec.Width = 6; IndObj.compCollision.offsetX = -3;
                IndObj.compCollision.rec.Height = 4; IndObj.compCollision.offsetY = 2;
                IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Swamp_Bulb;
                IndObj.compSprite.zOffset = 1;
            }
            else if (Type == IndestructibleType.Water_SmPlant)
            {
                IndObj.compCollision.rec.Width = 10; IndObj.compCollision.offsetX = -5;
                IndObj.compCollision.rec.Height = 4; IndObj.compCollision.offsetY = 2;
                IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Swamp_SmPlant;
                IndObj.compSprite.zOffset = 1;
            }

            #endregion


            #region Entrance Objects

            else if (Type == IndestructibleType.Coliseum_Shadow_Entrance)
            {
                IndObj.compSprite.drawRec.Width = 16 * 3; //nonstandard size
                IndObj.compSprite.drawRec.Height = 16 * 4; //nonstandard size
                IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Entrance_Coliseum_SkullIsland;
                //set collision rec to full size
                IndObj.compCollision.rec.Width = 16 * 3; IndObj.compCollision.offsetX = -8;
                IndObj.compCollision.rec.Height = 16 * 4; IndObj.compCollision.offsetY = -8;
                //sort save and block
                IndObj.compSprite.zOffset = +16 * 3 - 2;
            }
            else if (Type == IndestructibleType.ForestDungeon_Entrance)
            {
                IndObj.compSprite.drawRec.Width = 16 * 3; //nonstandard size
                IndObj.compSprite.drawRec.Height = 16 * 4; //nonstandard size
                IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Entrance_ForestDungeon;
                //set collision rec to full size
                IndObj.compCollision.rec.Width = 16 * 3; IndObj.compCollision.offsetX = -8;
                IndObj.compCollision.rec.Height = 16 * 4; IndObj.compCollision.offsetY = -8;
                //sort save and block
                IndObj.compSprite.zOffset = +16 * 3 - 2;
            }
            else if (Type == IndestructibleType.MountainDungeon_Entrance)
            {
                IndObj.compSprite.drawRec.Width = 16 * 2; //nonstandard size
                IndObj.compSprite.drawRec.Height = 16 * 4; //nonstandard size
                IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Entrance_MountainDungeon;

                //set collision rec near bottom of entrance
                IndObj.compCollision.rec.Width = 16 * 2; IndObj.compCollision.offsetX = -8;
                IndObj.compCollision.rec.Height = 16 * 2; IndObj.compCollision.offsetY = +8 + 16;
                //sort save and block
                IndObj.compSprite.zOffset = +16 * 3 - 2;
            }

            else if (Type == IndestructibleType.SwampDungeon_Entrance)
            {
                IndObj.compSprite.drawRec.Width = 16 * 3; //nonstandard size
                IndObj.compSprite.drawRec.Height = 16 * 4; //nonstandard size
                IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Entrance_SwampDungeon;
                //set collision rec near bottom of entrance
                IndObj.compCollision.offsetX = -8; IndObj.compCollision.rec.Width = 16 * 3;
                //let link overlap the shadow bottom a little bit..
                IndObj.compCollision.offsetY = -8; IndObj.compCollision.rec.Height = 16 * 4 - 4; //..by -4
                //sort save and block
                IndObj.compSprite.zOffset = +16 * 3 - 2;
            }

            #endregion






            //Boat Objs

            #region Front

            else if (Type == IndestructibleType.Boat_Front)
            {   //nonstandard size
                IndObj.compSprite.drawRec.Width = 16 * 2; IndObj.compSprite.drawRec.Height = 16 * 3;
                IndObj.compCollision.rec.Width = 16 * 2; IndObj.compCollision.offsetX = -8;
                IndObj.compCollision.rec.Height = 4; IndObj.compCollision.offsetY = 32 - 8;
                IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Front;

                IndObj.compSprite.zOffset = 20; //has height
            }
            else if (Type == IndestructibleType.Boat_Front_Left
                || Type == IndestructibleType.Boat_Front_Right)
            {   //nonstandard size
                IndObj.compSprite.drawRec.Width = 16 * 2; IndObj.compSprite.drawRec.Height = 16 * 3;
                IndObj.compCollision.rec.Width = 16 * 2; IndObj.compCollision.offsetX = -8;
                IndObj.compCollision.rec.Height = 16; IndObj.compCollision.offsetY = -8 + 32;

                if (Type == IndestructibleType.Boat_Front_Right)
                { IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Front_Right; }
                else { IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Front_Left; }

                IndObj.compSprite.zOffset = 20; //has height
            }

            else if (Type == IndestructibleType.Boat_Front_ConnectorLeft
                || Type == IndestructibleType.Boat_Front_ConnectorRight)
            {   //nonstandard size
                IndObj.compSprite.drawRec.Width = 16 * 2; IndObj.compSprite.drawRec.Height = 16 * 1;
                IndObj.compCollision.rec.Width = 8;
                IndObj.compCollision.rec.Height = 16; IndObj.compCollision.offsetY = -8;

                if (Type == IndestructibleType.Boat_Front_ConnectorRight)
                {
                    IndObj.compCollision.offsetX = +16;
                    IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Front_ConnectorRight;
                }
                else
                {
                    IndObj.compCollision.offsetX = -8;
                    IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Front_ConnectorLeft;
                }

                IndObj.compSprite.zOffset = -32; //sort above water
            }

            #endregion


            #region Bannisters + Stairs

            else if (Type == IndestructibleType.Boat_Bannister_Left
                || Type == IndestructibleType.Boat_Bannister_Right)
            {   //nonstandard size
                IndObj.compSprite.drawRec.Width = 16 * 2; IndObj.compSprite.drawRec.Height = 16 * 1;
                IndObj.compCollision.rec.Width = 8;
                IndObj.compCollision.rec.Height = 16; IndObj.compCollision.offsetY = -8;

                if (Type == IndestructibleType.Boat_Bannister_Right)
                {
                    IndObj.compCollision.offsetX = +16;
                    IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Bannister_Right;
                }
                else
                {
                    IndObj.compCollision.offsetX = -8;
                    IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Bannister_Left;
                }
                IndObj.compSprite.zOffset = -32; //sort above water
            }

            else if (Type == IndestructibleType.Boat_Stairs_Top_Left
                || Type == IndestructibleType.Boat_Stairs_Top_Right)
            {   //nonstandard size
                IndObj.compSprite.drawRec.Width = 16 * 2; IndObj.compSprite.drawRec.Height = 16 * 2;
                IndObj.compCollision.rec.Width = 8;
                IndObj.compCollision.rec.Height = 32; IndObj.compCollision.offsetY = -8;

                if (Type == IndestructibleType.Boat_Stairs_Top_Right)
                {
                    IndObj.compCollision.offsetX = +16;
                    IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Stairs_Top_Right;
                }
                else
                {
                    IndObj.compCollision.offsetX = -8;
                    IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Stairs_Top_Left;
                }
                IndObj.compSprite.zOffset = -32; //sort above water
            }

            else if (Type == IndestructibleType.Boat_Stairs_Bottom_Left
                || Type == IndestructibleType.Boat_Stairs_Bottom_Right)
            {   //nonstandard size
                IndObj.compSprite.drawRec.Width = 16 * 2; IndObj.compSprite.drawRec.Height = 16 * 1;
                IndObj.compCollision.rec.Width = 8;
                IndObj.compCollision.rec.Height = 16; IndObj.compCollision.offsetY = -8;

                if (Type == IndestructibleType.Boat_Stairs_Bottom_Right)
                {
                    IndObj.compCollision.offsetX = +16;
                    IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Stairs_Bottom_Right;
                }
                else
                {
                    IndObj.compCollision.offsetX = -8;
                    IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Stairs_Bottom_Left;
                }
                IndObj.compSprite.zOffset = -32; //sort above water
            }

            #endregion


            #region Back

            else if (Type == IndestructibleType.Boat_Back_Left
                || Type == IndestructibleType.Boat_Back_Right)
            {   //nonstandard size
                IndObj.compSprite.drawRec.Width = 16 * 1; IndObj.compSprite.drawRec.Height = 16 * 4;
                IndObj.compCollision.rec.Width = 16; IndObj.compCollision.offsetX = -8;
                IndObj.compCollision.rec.Height = 16 * 3; IndObj.compCollision.offsetY = -8;

                if (Type == IndestructibleType.Boat_Back_Right)
                { IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Back_Right; }
                else
                { IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Back_Left; }

                IndObj.compSprite.zOffset = -32; //sort above water
            }

            else if (Type == IndestructibleType.Boat_Back_Left_Connector
                || Type == IndestructibleType.Boat_Back_Right_Connector)
            {   //nonstandard size
                IndObj.compSprite.drawRec.Width = 16 * 1; IndObj.compSprite.drawRec.Height = 16 * 4;
                IndObj.compCollision.rec.Width = 16; IndObj.compCollision.offsetX = -8;
                IndObj.compCollision.rec.Height = 16 * 3 + 8; IndObj.compCollision.offsetY = -8;

                if (Type == IndestructibleType.Boat_Back_Right_Connector)
                { IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Back_Right_Connector; }
                else
                { IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Back_Left_Connector; }

                IndObj.compSprite.zOffset = -32; //sort above water
            }

            else if (Type == IndestructibleType.Boat_Back_Center)
            {   //nonstandard size
                IndObj.compSprite.drawRec.Width = 16 * 2; IndObj.compSprite.drawRec.Height = 16 * 4;
                IndObj.compCollision.rec.Width = 16 * 2; IndObj.compCollision.offsetX = -8;
                IndObj.compCollision.rec.Height = 16 * 2 - 4; IndObj.compCollision.offsetY = 16 + 2 + 2;

                IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Back_Center;

                IndObj.compSprite.zOffset = -24; //sort above water
            }

            #endregion


            #region Boat Engine

            else if (Type == IndestructibleType.Boat_Engine)
            {   //nonstandard size
                IndObj.compSprite.drawRec.Width = 16 * 6; IndObj.compSprite.drawRec.Height = 16 * 5;
                IndObj.compCollision.rec.Width = 16 * 3; IndObj.compCollision.offsetX = 16;
                IndObj.compCollision.rec.Height = 16 * 4 + 8; IndObj.compCollision.offsetY = 0;

                IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Boat_Engine;
                IndObj.compSprite.zOffset = 43; //has height
            }

            #endregion






            //Mountain Objs

            #region Mountain Caves

            else if (Type == IndestructibleType.MountainWall_Cave_Bare
                || Type == IndestructibleType.MountainWall_Cave_Covered)
            {   //nonstandard size
                IndObj.compSprite.drawRec.Width = 16 * 2; IndObj.compSprite.drawRec.Height = 16 * 2;
                IndObj.compCollision.rec.Width = 16 * 2; IndObj.compCollision.rec.Height = 16 * 2;
                IndObj.compCollision.offsetX = -8; IndObj.compCollision.offsetY = -8;

                if (Type == IndestructibleType.MountainWall_Cave_Bare)
                { IndObj.compAnim.currentAnimation = AnimationFrames.Wor_MountainWall_Cave_Bare; }
                else
                { IndObj.compAnim.currentAnimation = AnimationFrames.Wor_MountainWall_Cave_Covered; }

                IndObj.compSprite.zOffset = 0; //sorts over footholds
                IndObj.group = IndestructibleGroup.Object;
            }

            #endregion


            #region Mountain Alcoves

            else if (Type == IndestructibleType.MountainWall_Alcove_Left
                || Type == IndestructibleType.MountainWall_Alcove_Right)
            {   //nonstandard size
                IndObj.compSprite.drawRec.Width = 16 * 2; IndObj.compSprite.drawRec.Height = 16 * 2;
                IndObj.compCollision.rec.Width = 12; IndObj.compCollision.rec.Height = 16 * 2;

                if (Type == IndestructibleType.MountainWall_Alcove_Left)
                {   //set collision rec left
                    IndObj.compAnim.currentAnimation = AnimationFrames.Wor_MountainWall_Alcove_Left;
                    IndObj.compCollision.offsetX = -8; IndObj.compCollision.offsetY = -8;
                }
                else
                {   //set collision rec right
                    IndObj.compAnim.currentAnimation = AnimationFrames.Wor_MountainWall_Alcove_Right;
                    IndObj.compCollision.offsetX = +12; IndObj.compCollision.offsetY = -8;
                }

                IndObj.compSprite.zOffset = -18; //sorts under footholds
                IndObj.group = IndestructibleGroup.Object;
            }

            #endregion






            //Forest Dungeon Entrance Objs

            #region Big Skull Teeth in Water

            else if (Type == IndestructibleType.ForestDungeon_SkullToothInWater_Left ||
                Type == IndestructibleType.ForestDungeon_SkullToothInWater_Right)
            {
                IndObj.compSprite.drawRec.Width = 16 * 3; //nonstandard size
                IndObj.compSprite.drawRec.Height = 16 * 4; //nonstandard size
                //sort save and block
                IndObj.compSprite.zOffset = +16 * 3 - 2; //based on hero sorting
                IndObj.compCollision.rec.Height = 16 * 4 - 4; IndObj.compCollision.offsetY = -8;
                IndObj.compCollision.rec.Width = 16 * 3 - 4;

                if (Type == IndestructibleType.ForestDungeon_SkullToothInWater_Left)
                {   //left
                    IndObj.compAnim.currentAnimation = AnimationFrames.Wor_SkullToothInWater_Left;
                    IndObj.compCollision.offsetX = -4;
                }
                else
                {   //right
                    IndObj.compAnim.currentAnimation = AnimationFrames.Wor_SkullToothInWater_Right;
                    IndObj.compCollision.offsetX = -8;
                }
            }
            else if (Type == IndestructibleType.ForestDungeon_SkullToothInWater_EndCap_Left ||
                Type == IndestructibleType.ForestDungeon_SkullToothInWater_EndCap_Right)
            {
                IndObj.compSprite.drawRec.Width = 16 * 3; //nonstandard size
                IndObj.compSprite.drawRec.Height = 16 * 4; //nonstandard size
                //sort save and block
                IndObj.compSprite.zOffset = +16 * 20; //top most
                IndObj.compCollision.rec.Height = 16 * 4; IndObj.compCollision.offsetY = -8;
                IndObj.compCollision.rec.Width = 16 * 3; IndObj.compCollision.offsetX = -8;
                IndObj.compCollision.rec.Width = 16 * 3 - 4;

                if (Type == IndestructibleType.ForestDungeon_SkullToothInWater_EndCap_Left)
                {
                    IndObj.compAnim.currentAnimation = AnimationFrames.Wor_SkullToothInWater_EndCap_Left;
                    IndObj.compCollision.offsetX = -4;
                }
                else
                {
                    IndObj.compAnim.currentAnimation = AnimationFrames.Wor_SkullToothInWater_EndCap_Right;
                    IndObj.compCollision.offsetX = -8;
                }
            }

            else if (Type == IndestructibleType.ForestDungeon_SkullToothInWater_Arch_Left ||
                Type == IndestructibleType.ForestDungeon_SkullToothInWater_Arch_Right)
            {
                IndObj.compSprite.drawRec.Width = 16 * 4; //nonstandard size
                IndObj.compSprite.drawRec.Height = 16 * 1; //nonstandard size
                //sort save and block
                IndObj.compSprite.zOffset = +16 * 20; //top most
                IndObj.compCollision.rec.Height = 16 * 1; IndObj.compCollision.offsetY = -8;
                IndObj.compCollision.rec.Width = 16 * 4; IndObj.compCollision.offsetX = -8;

                if (Type == IndestructibleType.ForestDungeon_SkullToothInWater_Arch_Left)
                { IndObj.compAnim.currentAnimation = AnimationFrames.Wor_SkullToothInWater_Arch_Left; }
                else
                { IndObj.compAnim.currentAnimation = AnimationFrames.Wor_SkullToothInWater_Arch_Right; }
            }

            else if (Type == IndestructibleType.ForestDungeon_SkullToothInWater_Center)
            {
                IndObj.compSprite.drawRec.Width = 16 * 4; //nonstandard size
                IndObj.compSprite.drawRec.Height = 16 * 3; //nonstandard size
                //sort save and block
                IndObj.compSprite.zOffset = +16 * 9; //under arch extensions
                IndObj.compCollision.rec.Height = 16 * 3; IndObj.compCollision.offsetY = -8;
                IndObj.compCollision.rec.Width = 16 * 4; IndObj.compCollision.offsetX = -8;
                IndObj.compAnim.currentAnimation = AnimationFrames.Wor_SkullToothInWater_Center;
            }

            else if (Type == IndestructibleType.ForestDungeon_SkullToothInWater_Arch_Extension)
            {   //sort save and block
                IndObj.compSprite.zOffset = +16 * 10; //under most other teeth objs
                IndObj.compCollision.rec.Height = 16 * 1; IndObj.compCollision.offsetY = -8;
                IndObj.compCollision.rec.Width = 16 * 1; IndObj.compCollision.offsetX = -8;
                IndObj.compAnim.currentAnimation = AnimationFrames.Wor_SkullToothInWater_Arch_Extension;
            }

            #endregion







            //Coliseum Objs

            #region Handrails

            else if (
                Type == IndestructibleType.Coliseum_Shadow_Stairs_Handrail_Top
                || Type == IndestructibleType.Coliseum_Shadow_Stairs_Handrail_Middle
                || Type == IndestructibleType.Coliseum_Shadow_Stairs_Handrail_Bottom
                )
            {
                //Obj.compCollision.offsetY = 0; Obj.compCollision.rec.Height = 8;
                IndObj.compSprite.zOffset = 0;
                if (Type == IndestructibleType.Coliseum_Shadow_Stairs_Handrail_Top)
                { IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Stairs_Handrail_Top; }
                else if (Type == IndestructibleType.Coliseum_Shadow_Stairs_Handrail_Middle)
                { IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Stairs_Handrail_Middle; }
                else if (Type == IndestructibleType.Coliseum_Shadow_Stairs_Handrail_Bottom)
                { IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Stairs_Handrail_Bottom; }
            }

            #endregion


            #region Pillar

            else if (
                Type == IndestructibleType.Coliseum_Shadow_Pillar_Top
                || Type == IndestructibleType.Coliseum_Shadow_Pillar_Middle
                || Type == IndestructibleType.Coliseum_Shadow_Pillar_Bottom
                )
            {
                IndObj.compSprite.zOffset = 0;
                if (Type == IndestructibleType.Coliseum_Shadow_Pillar_Top)
                { IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Pillar_Top; }
                else if (Type == IndestructibleType.Coliseum_Shadow_Pillar_Middle)
                { IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Pillar_Middle; }
                else if (Type == IndestructibleType.Coliseum_Shadow_Pillar_Bottom)
                { IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Pillar_Bottom; }
            }

            #endregion


            #region Bricks

            else if (
                Type == IndestructibleType.Coliseum_Shadow_Bricks_Left
                || Type == IndestructibleType.Coliseum_Shadow_Bricks_Middle1
                || Type == IndestructibleType.Coliseum_Shadow_Bricks_Middle2
                || Type == IndestructibleType.Coliseum_Shadow_Bricks_Right
                )
            {
                IndObj.compSprite.drawRec.Width = 16 * 1; //nonstandard size
                IndObj.compSprite.drawRec.Height = 16 * 2; //nonstandard size
                IndObj.compCollision.rec.Width = 16; IndObj.compCollision.offsetX = -8;
                IndObj.compCollision.rec.Height = 16 * 2; IndObj.compCollision.offsetY = -8;

                if (Type == IndestructibleType.Coliseum_Shadow_Bricks_Left)
                { IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Bricks_Left; }
                else if (Type == IndestructibleType.Coliseum_Shadow_Bricks_Middle1)
                { IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Bricks_Middle1; }
                else if (Type == IndestructibleType.Coliseum_Shadow_Bricks_Middle2)
                { IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Bricks_Middle2; }
                else if (Type == IndestructibleType.Coliseum_Shadow_Bricks_Right)
                { IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Bricks_Right; }

                IndObj.compSprite.zOffset = -18;
            }

            #endregion


            #region Spectators

            else if (Type == IndestructibleType.Coliseum_Shadow_Spectator)
            {
                IndObj.compSprite.drawRec.Width = 16 * 4; //nonstandard size
                IndObj.compSprite.drawRec.Height = 16 * 1;
                IndObj.compCollision.rec.Width = 16 * 4; IndObj.compCollision.offsetX = -8;
                IndObj.compCollision.rec.Height = 16 * 1; IndObj.compCollision.offsetY = -8;

                IndObj.compSprite.zOffset = 64; //sort over others like a roof
                IndObj.interacts = true;

                //animation speed should be varied around 10, biased slow
                IndObj.compAnim.speed = (byte)(10 + Functions_Random.Int(-3, 5));

                //randomly flip sprite horizontally
                if (Functions_Random.Int(0, 100) < 50)
                { IndObj.compSprite.flipHorizontally = true; }
                else { IndObj.compSprite.flipHorizontally = false; }

                IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Colliseum_Spectator;
            }

            #endregion








            //all indestructible objs have metallic tap sfx
            //IndObj.sfx.hit = Assets.sfxTapMetallic;

            //finalize rotation, animation, and component alignment
            SetRotation(IndObj);
            IndObj.compSprite.currentFrame = IndObj.compAnim.currentAnimation[0]; //goto 1st anim frame
            Functions_Component.Align(IndObj.compSprite, IndObj.compCollision);
        }
    }
}