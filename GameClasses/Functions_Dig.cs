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
    public static class Functions_Dig
    {
        //the neighbor list
        static List<Boolean> nei = new List<bool>
        {   //N     //R     //S     //L
            false, false, false, false
        };
        static Boolean filledNeighbor = false;

        static int d;
        static int n;

        static int i;
        static List<GameObject> ditchesToUpdate;

        static ComponentSprite floorSprite;


        public static void FillDitch(GameObject Ditch)
        {
            Ditch.getsAI = true; //ditch is in filled state

            //convert empty to filled
            if (Ditch.compAnim.currentAnimation == AnimationFrames.Wor_Ditch_Empty_3UP_Horizontal)
            { Ditch.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Filled_3UP_Horizontal; }
            else if (Ditch.compAnim.currentAnimation == AnimationFrames.Wor_Ditch_Empty_3UP_North)
            { Ditch.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Filled_3UP_North; }
            else if (Ditch.compAnim.currentAnimation == AnimationFrames.Wor_Ditch_Empty_3UP_South)
            { Ditch.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Filled_3UP_South; }
            else if (Ditch.compAnim.currentAnimation == AnimationFrames.Wor_Ditch_Empty_4UP)
            { Ditch.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Filled_4UP; }

            else if (Ditch.compAnim.currentAnimation == AnimationFrames.Wor_Ditch_Empty_Corner_North)
            { Ditch.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Filled_Corner_North; }
            else if (Ditch.compAnim.currentAnimation == AnimationFrames.Wor_Ditch_Empty_Corner_South)
            { Ditch.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Filled_Corner_South; }
            else if (Ditch.compAnim.currentAnimation == AnimationFrames.Wor_Ditch_Empty_Endcap_Horizontal)
            { Ditch.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Filled_Endcap_Horizontal; }
            else if (Ditch.compAnim.currentAnimation == AnimationFrames.Wor_Ditch_Empty_Endcap_North)
            { Ditch.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Filled_Endcap_North; }

            else if (Ditch.compAnim.currentAnimation == AnimationFrames.Wor_Ditch_Empty_Endcap_South)
            { Ditch.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Filled_Endcap_South; }
            else if (Ditch.compAnim.currentAnimation == AnimationFrames.Wor_Ditch_Empty_Horizontal)
            { Ditch.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Filled_Horizontal; }
            else if (Ditch.compAnim.currentAnimation == AnimationFrames.Wor_Ditch_Empty_Single)
            { Ditch.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Filled_Single; }
            else if (Ditch.compAnim.currentAnimation == AnimationFrames.Wor_Ditch_Empty_Vertical)
            { Ditch.compAnim.currentAnimation = AnimationFrames.Wor_Ditch_Filled_Vertical; }

            Functions_Particle.Spawn(ObjType.Particle_Splash,
                Ditch.compSprite.position.X,
                Ditch.compSprite.position.Y);
        }
 

        public static void SetDitch(GameObject Ditch)
        {
            //reset neighbors
            nei[0] = false; nei[1] = false; nei[2] = false; nei[3] = false;
            //reset ditch meta
            Functions_GameObject.SetType(Ditch, ObjType.Wor_Ditch_META);
            Ditch.active = false; //set inactive for check below
            filledNeighbor = false; //assume no ditch neighbors are filled


            #region Set Neighbors

            for (n = 0; n < Pool.roomObjCount; n++)
            {
                if (Pool.roomObjPool[n].active & Pool.roomObjPool[n].group == ObjGroup.Ditch)
                {
                    //expand horizontally
                    Ditch.compCollision.rec.Width = 22;
                    Ditch.compCollision.rec.X -= 4;
                    //set left/right neighbors
                    if (Ditch.compCollision.rec.Intersects(Pool.roomObjPool[n].compCollision.rec))
                    {
                        //either left or right of new ditch
                        if (Pool.roomObjPool[n].compSprite.position.X < Ditch.compSprite.position.X)
                        { nei[3] = true; } //set L
                        else if (Pool.roomObjPool[n].compSprite.position.X == Ditch.compSprite.position.X)
                        { } //nothing
                        else { nei[1] = true; } //set R

                        //check neighbor for filled state
                        if (Pool.roomObjPool[n].getsAI) { filledNeighbor = true; }
                    }
                    //retract
                    Ditch.compCollision.rec.Width = 16;
                    Ditch.compCollision.rec.X += 4;

                    //expand vertically
                    Ditch.compCollision.rec.Height = 22;
                    Ditch.compCollision.rec.Y -= 4;
                    //set top/bottom neighbors
                    if (Ditch.compCollision.rec.Intersects(Pool.roomObjPool[n].compCollision.rec))
                    {   //either above or below new ditch
                        if (Pool.roomObjPool[n].compSprite.position.Y < Ditch.compSprite.position.Y)
                        { nei[0] = true; } //set N
                        else if (Pool.roomObjPool[n].compSprite.position.Y == Ditch.compSprite.position.Y)
                        { } //nothing
                        else { nei[2] = true; } //set S

                        //check neighbor for filled state
                        if (Pool.roomObjPool[n].getsAI) { filledNeighbor = true; }
                    }
                    //retract
                    Ditch.compCollision.rec.Height = 16;
                    Ditch.compCollision.rec.Y += 4;
                }
            }

            #endregion


            Ditch.active = true; //make active
            //do comparisons on neighbors to determine what type of ditch to make

            //no connections - all false - single hole
            if (nei[0] == false & nei[1] == false & nei[2] == false & nei[3] == false)
            {
                Functions_GameObject.SetType(Ditch, ObjType.Wor_Ditch_Empty_Single);
                Ditch.compSprite.flipHorizontally = false;
            }


            #region 1 connection - endCaps

            //north
            else if (nei[0] == true & nei[1] == false & nei[2] == false & nei[3] == false)
            {
                Functions_GameObject.SetType(Ditch, ObjType.Wor_Ditch_Empty_Endcap_South);
                Ditch.compSprite.flipHorizontally = false;
            }
            //right
            else if (nei[0] == false & nei[1] == true & nei[2] == false & nei[3] == false)
            {
                Functions_GameObject.SetType(Ditch, ObjType.Wor_Ditch_Empty_Endcap_Horizontal);
                Ditch.compSprite.flipHorizontally = true;
            }
            //south
            else if (nei[0] == false & nei[1] == false & nei[2] == true & nei[3] == false)
            {
                Functions_GameObject.SetType(Ditch, ObjType.Wor_Ditch_Empty_Endcap_North);
                Ditch.compSprite.flipHorizontally = false;
            }
            //left
            else if (nei[0] == false & nei[1] == false & nei[2] == false & nei[3] == true)
            {
                Functions_GameObject.SetType(Ditch, ObjType.Wor_Ditch_Empty_Endcap_Horizontal);
                Ditch.compSprite.flipHorizontally = false;
            }

            #endregion

            
            #region 2 connections

            //up down
            else if (nei[0] == true & nei[1] == false & nei[2] == true & nei[3] == false)
            {
                Functions_GameObject.SetType(Ditch, ObjType.Wor_Ditch_Empty_Vertical);
                Ditch.compSprite.flipHorizontally = false;
            }
            //left right
            else if (nei[0] == false & nei[1] == true & nei[2] == false & nei[3] == true)
            {
                Functions_GameObject.SetType(Ditch, ObjType.Wor_Ditch_Empty_Horizontal);
                Ditch.compSprite.flipHorizontally = false;
            }

            //2 connections - corners
            //up right
            else if (nei[0] == true & nei[1] == true & nei[2] == false & nei[3] == false)
            {
                Functions_GameObject.SetType(Ditch, ObjType.Wor_Ditch_Empty_Corner_South);
                Ditch.compSprite.flipHorizontally = true;
            }
            //up left
            else if (nei[0] == true & nei[1] == false & nei[2] == false & nei[3] == true)
            {
                Functions_GameObject.SetType(Ditch, ObjType.Wor_Ditch_Empty_Corner_South);
                Ditch.compSprite.flipHorizontally = false;
            }
            //down right
            else if (nei[0] == false & nei[1] == true & nei[2] == true & nei[3] == false)
            {
                Functions_GameObject.SetType(Ditch, ObjType.Wor_Ditch_Empty_Corner_North);
                Ditch.compSprite.flipHorizontally = false;
            }
            //down left
            else if (nei[0] == false & nei[1] == false & nei[2] == true & nei[3] == true)
            {
                Functions_GameObject.SetType(Ditch, ObjType.Wor_Ditch_Empty_Corner_North);
                Ditch.compSprite.flipHorizontally = true;
            }

            #endregion


            #region 3 connections

            //up down left
            else if (nei[0] == true & nei[1] == true & nei[2] == true & nei[3] == false)
            {
                Functions_GameObject.SetType(Ditch, ObjType.Wor_Ditch_Empty_3UP_Horizontal);
                Ditch.compSprite.flipHorizontally = false;
            }
            //up down right
            else if (nei[0] == true & nei[1] == false & nei[2] == true & nei[3] == true)
            {
                Functions_GameObject.SetType(Ditch, ObjType.Wor_Ditch_Empty_3UP_Horizontal);
                Ditch.compSprite.flipHorizontally = true;
            }
            //left right up
            else if (nei[0] == true & nei[1] == true & nei[2] == false & nei[3] == true)
            {
                Functions_GameObject.SetType(Ditch, ObjType.Wor_Ditch_Empty_3UP_South);
                Ditch.compSprite.flipHorizontally = false;
            }
            //left right down
            else if (nei[0] == false & nei[1] == true & nei[2] == true & nei[3] == true)
            {
                Functions_GameObject.SetType(Ditch, ObjType.Wor_Ditch_Empty_3UP_North);
                Ditch.compSprite.flipHorizontally = false;
            }

            #endregion


            //4 connections - all true - 4up
            else if (nei[0] == true & nei[1] == true & nei[2] == true & nei[3] == true)
            {
                Functions_GameObject.SetType(Ditch, ObjType.Wor_Ditch_Empty_4UP);
                Ditch.compSprite.flipHorizontally = false;
            }

            //if the ditch touched any filled neighbor, then fill this ditch with water
            if (filledNeighbor) { FillDitch(Ditch); }
        }


        public static void Dig()
        {
            //set hero's interaction rec
            Functions_Hero.SetInteractionRec();

            //place obj aligned to 16x16 grid
            GameObject objRef;
            objRef = Functions_Pool.GetRoomObj();
            //setup move position based on interaction point, aligned to 16px grid
            objRef.compMove.newPosition = Functions_Movement.AlignToGrid(
                Functions_Hero.interactionPoint.X,
                Functions_Hero.interactionPoint.Y);
            Functions_Hero.ClearInteractionRec(); //done with intRec

            //move obj to aligned position
            Functions_Movement.Teleport(
                objRef.compMove,
                objRef.compMove.newPosition.X,
                objRef.compMove.newPosition.Y);
            //switch to meta ditch obj
            objRef.compMove.direction = Direction.Down;
            Functions_GameObject.SetType(objRef, ObjType.Wor_Ditch_META);

            //for purposes of the following checks, objRef.active is set to false
            objRef.active = false;
            //we'll use .visible to act as a temp flag to cue release()
            objRef.compSprite.visible = true;


            #region Check to see if ditch can be dug at this location

            for (d = 0; d < Pool.roomObjCount; d++)
            {
                if (Pool.roomObjPool[d].active & //check overlap for active roomObjs
                    Pool.roomObjPool[d].compCollision.rec.Intersects(objRef.compCollision.rec))
                {
                    //if ditch touches any of these objs, release ditch & bail
                    if (Pool.roomObjPool[d].compCollision.blocking)
                    {
                        objRef.compSprite.visible = false;
                    }
                    //group checks
                    else if (Pool.roomObjPool[d].group == ObjGroup.Wall
                        || Pool.roomObjPool[d].group == ObjGroup.Door
                        )
                    {
                        objRef.compSprite.visible = false;
                    }
                    //type checks
                    else if (
                        //world objs
                        Pool.roomObjPool[d].type == ObjType.Wor_Entrance_ForestDungeon
                        || Pool.roomObjPool[d].type == ObjType.Wor_Bookcase

                        //water objs
                        || Pool.roomObjPool[d].type == ObjType.Wor_Water
                        //digging into coastlines is fine and necessary
                        //|| Pool.roomObjPool[d].type == ObjType.Wor_Coastline_Corner_Exterior
                        //|| Pool.roomObjPool[d].type == ObjType.Wor_Coastline_Corner_Interior
                        //|| Pool.roomObjPool[d].type == ObjType.Wor_Coastline_Straight

                        //dungeon objs
                        || Pool.roomObjPool[d].type == ObjType.Dungeon_Pit
                        || Pool.roomObjPool[d].type == ObjType.Dungeon_PitBridge
                        || Pool.roomObjPool[d].type == ObjType.Dungeon_PitTeethBottom
                        || Pool.roomObjPool[d].type == ObjType.Dungeon_PitTeethTop
                        || Pool.roomObjPool[d].type == ObjType.Dungeon_PitTrap
                        )
                    {
                        objRef.compSprite.visible = false;
                    }
                }
            }

            #endregion


            //if we released the obj, bail from method
            if (objRef.compSprite.visible == false) { return; }
            else { objRef.compSprite.visible = true; } //done with flag


            #region Good to dig, remove any overlapping objects, spawn loot

            for (d = 0; d < Pool.roomObjCount; d++)
            {   //if this ditch contains the center of any active object's hitBox, release obj
                if (Pool.roomObjPool[d].active &
                    objRef.compCollision.rec.Contains(Pool.roomObjPool[d].compCollision.rec.Location))
                {
                    //spawn loot with negative rate - why?
                    //if hero has ring equipped, his loot chances increase 25 points
                    //which means he could literally spam dig loot out of the ground
                    //we set this to -20 to curb the effects of this state..
                    //but hero can *still* dig loot right out of the ground if he wants to
                    Functions_Loot.SpawnLoot(Pool.roomObjPool[d].compSprite.position, -20);
                    //release any obj that makes it here
                    Functions_Pool.Release(Pool.roomObjPool[d]);
                }
            }

            #endregion


            //determine what type of ditch this should be
            Functions_Component.Align(objRef);
            SetDitch(objRef);
            

            


            //now we need to update the surrounding ditch objects,
            //because it's likely that the new ditch alters them
            //also, we need to check if new ditch touches a water tile and fill it


            #region Update Ditch Neighbors

            //expand to 4x4
            objRef.compCollision.rec.Width = 16*2 + 4;
            objRef.compCollision.rec.Height = 16*2 + 4;
            objRef.compCollision.offsetX = -20;
            objRef.compCollision.offsetY = -20;
            Functions_Component.Align(objRef);

            ditchesToUpdate = new List<GameObject>(); //clear the list
            objRef.active = false; //remove objRef from check below
            for (d = 0; d < Pool.roomObjCount; d++)
            {   
                if (Pool.roomObjPool[d].active & 
                    objRef.compCollision.rec.Intersects(Pool.roomObjPool[d].compCollision.rec))
                {
                    //check to see if the new ditch touches a water tile
                    //in which case we need to fill the ditch with water
                    if (Pool.roomObjPool[d].type == ObjType.Wor_Water)
                    {   //ditch becomes filled version
                        FillDitch(objRef);

                        //we need to blend this ditch tile with the nearby water tile
                        //so we get a floor sprite and place it at this location
                        floorSprite = Functions_Pool.GetFloor();
                        //set the texture and animation frame to water tile
                        floorSprite.texture = Assets.forestLevelSheet;
                        floorSprite.currentFrame = AnimationFrames.Wor_Water[0];
                        //place the water tile
                        floorSprite.position.X = objRef.compSprite.position.X;
                        floorSprite.position.Y = objRef.compSprite.position.Y;
                        //set zDepth above other floor tiles
                        floorSprite.zDepth = World.waterLayer;
                    }
                    else if(Pool.roomObjPool[d].group == ObjGroup.Ditch)
                    {   //if ditch touches any other ditches, update them
                        ditchesToUpdate.Add(Pool.roomObjPool[d]);
                    }
                }
            }
            objRef.active = true;

            //reset ditch hitBox
            objRef.compCollision.rec.Width = 16;
            objRef.compCollision.rec.Height = 16;
            objRef.compCollision.offsetX = -8;
            objRef.compCollision.offsetY = -8;
            Functions_Component.Align(objRef);
            //set sprite frame prior to this loop's draw call
            Functions_Animation.Animate(objRef.compAnim, objRef.compSprite);

            //update the surrounding ditches
            for (i = 0; i < ditchesToUpdate.Count; i++)
            { SetDitch(ditchesToUpdate[i]); }

            //push a frame of animation
            for (i = 0; i < ditchesToUpdate.Count; i++)
            { Functions_Animation.Animate(ditchesToUpdate[i].compAnim, ditchesToUpdate[i].compSprite); }

            #endregion



            //create some 'digging' debris
            Functions_Particle.Spawn_Explosion(ObjType.Particle_Debris,
                objRef.compSprite.position.X, objRef.compSprite.position.Y, false);
        }


    }
}