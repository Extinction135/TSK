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
                //perform indestructible obj behaviors here



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
            //currently, no ind objs clean themselves
            IndObj.selfCleans = false; //exit routine
        }






        public static void HandleProCollision(IndestructibleObject IndObj, Projectile Pro)
        {

            #region Arrow

            if (Pro.type == ProjectileType.Arrow)
            {
                if (Pro.compAnim.currentAnimation == AnimationFrames.Projectile_Arrow)
                {   //flip animFrame to hit, rendering arrow unable to hit other actors/objs
                    Functions_Movement.StopMovement(Pro.compMove);
                    Functions_Projectile.SetArrowHitState(Pro); //change anim frame to hit arrow
                }
            }

            #endregion


            #region Boomerang

            else if (Pro.type == ProjectileType.Boomerang)
            {
                if (Pro.lifeCounter < 200)
                {   //set boomerang into return mode only once
                    Pro.lifeCounter = 200;
                    Functions_Particle.Spawn(
                        ParticleType.Attention,
                        Pro.compSprite.position.X + 4,
                        Pro.compSprite.position.Y + 4);
                }
                Functions_Projectile.BoomerangBounce(Pro,
                    IndObj.compSprite.position);
                Assets.Play(Assets.sfxTapMetallic); //always metallic
            }

            #endregion


            #region Bomb

            else if (Pro.type == ProjectileType.Bomb)
            {
                Functions_Movement.StopMovement(Pro.compMove);
            }

            #endregion


            else
            {   //all other (moving) projectiles are killed
                Functions_Projectile.Kill(Pro);
            }
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
            }
            else if (Type == IndestructibleType.Dungeon_Exit)
            {
                IndObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IndObj.compSprite.drawRec.Height = 16 * 3; //nonstandard size
                IndObj.compSprite.zOffset = -32; //sort to floor
                IndObj.group = IndestructibleGroup.Exit;
                IndObj.compCollision.rec.Height = 11;
                IndObj.compCollision.offsetY = 30;
                IndObj.compAnim.currentAnimation = AnimationFrames.Dungeon_Exit;
            }
            else if (Type == IndestructibleType.Dungeon_ExitLight)
            {
                IndObj.compSprite.texture = Assets.Dungeon_CurrentSheet; //is dungeonObj
                IndObj.compSprite.drawRec.Height = 16 * 2; //nonstandard size
                IndObj.compCollision.offsetY = 0;
                IndObj.compSprite.zOffset = 256; //sort above everything
                IndObj.compAnim.currentAnimation = AnimationFrames.Dungeon_ExitLight;

                IndObj.compCollision.rec.Height = 11;
                IndObj.compCollision.offsetY = 30;
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

            
            #region Dungeon and Coliseum Entrance Objects

            else if (Type == IndestructibleType.Coliseum_Shadow_Entrance)
            {
                IndObj.compSprite.drawRec.Width = 16 * 3; //nonstandard size
                IndObj.compSprite.drawRec.Height = 16 * 4; //nonstandard size
                IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Entrance_Coliseum_SkullIsland;
                IndObj.compCollision.rec.Width = 16 * 3; IndObj.compCollision.offsetX = -8;
                IndObj.compCollision.rec.Height = 16 * 4; IndObj.compCollision.offsetY = -8;
                IndObj.compSprite.zOffset = +16 * 3 - 2;
            }
            else if (Type == IndestructibleType.ForestDungeon_Entrance)
            {
                IndObj.compSprite.drawRec.Width = 16 * 3; //nonstandard size
                IndObj.compSprite.drawRec.Height = 16 * 4; //nonstandard size
                IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Entrance_ForestDungeon;
                IndObj.compCollision.rec.Width = 16 * 3; IndObj.compCollision.offsetX = -8;
                IndObj.compCollision.rec.Height = 16 * 4; IndObj.compCollision.offsetY = -8;
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
                IndObj.compSprite.zOffset = +16 * 3 - 2;
            }

            else if (Type == IndestructibleType.SwampDungeon_Entrance)
            {
                IndObj.compSprite.drawRec.Width = 16 * 3; //nonstandard size
                IndObj.compSprite.drawRec.Height = 16 * 4; //nonstandard size
                IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Entrance_ForestDungeon;
                IndObj.compCollision.offsetX = -8; IndObj.compCollision.rec.Width = 16 * 3;
                IndObj.compCollision.offsetY = -8; IndObj.compCollision.rec.Height = 16 * 4 - 4; //..by -4
                IndObj.compSprite.zOffset = +16 * 3 - 2;
            }





            else if (Type == IndestructibleType.ThievesDungeon_Entrance)
            {
                IndObj.compSprite.drawRec.Width = 16 * 3; //nonstandard size
                IndObj.compSprite.drawRec.Height = 16 * 4; //nonstandard size
                IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Entrance_ForestDungeon;
                IndObj.compCollision.rec.Width = 16 * 3; IndObj.compCollision.offsetX = -8;
                IndObj.compCollision.rec.Height = 16 * 4; IndObj.compCollision.offsetY = -8;
                IndObj.compSprite.zOffset = +16 * 3 - 2;
            }
            else if (Type == IndestructibleType.LavaDungeon_Entrance)
            {
                IndObj.compSprite.drawRec.Width = 16 * 3; //nonstandard size
                IndObj.compSprite.drawRec.Height = 16 * 4; //nonstandard size
                IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Entrance_ForestDungeon;
                IndObj.compCollision.rec.Width = 16 * 3; IndObj.compCollision.offsetX = -8;
                IndObj.compCollision.rec.Height = 16 * 4; IndObj.compCollision.offsetY = -8;
                IndObj.compSprite.zOffset = +16 * 3 - 2;
            }
            else if (Type == IndestructibleType.CloudDungeon_Entrance)
            {
                IndObj.compSprite.drawRec.Width = 16 * 3; //nonstandard size
                IndObj.compSprite.drawRec.Height = 16 * 4; //nonstandard size
                IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Entrance_ForestDungeon;
                IndObj.compCollision.rec.Width = 16 * 3; IndObj.compCollision.offsetX = -8;
                IndObj.compCollision.rec.Height = 16 * 4; IndObj.compCollision.offsetY = -8;
                IndObj.compSprite.zOffset = +16 * 3 - 2;
            }
            else if (Type == IndestructibleType.SkullDungeon_Entrance)
            {
                IndObj.compSprite.drawRec.Width = 16 * 3; //nonstandard size
                IndObj.compSprite.drawRec.Height = 16 * 4; //nonstandard size
                IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Entrance_ForestDungeon;
                IndObj.compCollision.rec.Width = 16 * 3; IndObj.compCollision.offsetX = -8;
                IndObj.compCollision.rec.Height = 16 * 4; IndObj.compCollision.offsetY = -8;
                IndObj.compSprite.zOffset = +16 * 3 - 2;
            }

            #endregion







            //big shadow extension
            else if (Type == IndestructibleType.Wor_ShadowEntrance_Extension)
            {
                IndObj.compSprite.drawRec.Width = 16 * 3; //nonstandard size
                IndObj.compSprite.drawRec.Height = 16 * 4; //nonstandard size
                IndObj.compAnim.currentAnimation = AnimationFrames.Wor_Shadow_Big;
                IndObj.compCollision.rec.Width = 16 * 3; IndObj.compCollision.offsetX = -8;
                IndObj.compCollision.rec.Height = 16 * 4; IndObj.compCollision.offsetY = -8;
                IndObj.compSprite.zOffset = +16 * 3 - 2;
            }








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