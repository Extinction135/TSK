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
    public static class Functions_GameObject_World
    {
        static int i;





        





        //level 0.5 caused by: boomerang
        public static void Bounce(GameObject Obj)
        {   //Obj.compMove.direction needs to be set by collider



            //Kill RoomObj Enemies
            if (Obj.group == ObjGroup.Enemy)
            {

                #region Seeker Exploders

                if (Obj.type == ObjType.Wor_SeekerExploder)
                {   //Obj.compMove.direction should be set by colliding pro prior
                    Functions_GameObject.SetType(Obj, ObjType.ExplodingObject); //explode
                    Functions_Movement.Push(Obj.compMove, Obj.compMove.direction, 6.0f);
                    Assets.Play(Assets.sfxActorLand);
                }

                #endregion


                //All Other RoomObj Enemies
                else
                {   //Obj.compMove.direction should be set by colliding pro prior
                    Functions_Particle.Spawn(ParticleType.Attention, Obj);
                    Functions_GameObject.Kill(Obj, true, false);
                    Assets.Play(Assets.sfxActorLand);
                }
            }







            #region Bush

            if (Obj.type == ObjType.Wor_Bush)
            {
                //pop leaf explosion
                Functions_Particle.Spawn_Explosion(
                    ParticleType.LeafGreen,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y);
                //covert bush to stump, play sfx
                Functions_GameObject.SetType(Obj, ObjType.Wor_Bush_Stump);
                Assets.Play(Assets.sfxBushCut);
                //pop an attention particle
                Functions_Particle.Spawn(ParticleType.Attention,
                    Obj.compSprite.position.X, Obj.compSprite.position.Y);
                //rarely spawn loot
                Functions_Loot.SpawnLoot(Obj.compSprite.position, 20);
            }

            #endregion


            #region House Doors

            else if (Obj.type == ObjType.Wor_Build_Door_Shut)
            {   
                OpenHouseDoor(Obj);
            }

            #endregion


            #region Levers

            else if (Obj.type == ObjType.Dungeon_LeverOff
                || Obj.type == ObjType.Dungeon_LeverOn)
            {
                Functions_GameObject_Dungeon.ActivateLeverObjects();
            }

            #endregion


            #region Explosive Dungeon Barrels

            else if (Obj.type == ObjType.Dungeon_Barrel)
            {   //Obj.compMove.direction should be set by colliding pro prior
                Functions_GameObject_Dungeon.HitBarrel(Obj);
            }

            #endregion


            #region Switch Block Globe/Buttons

            else if (Obj.type == ObjType.Dungeon_SwitchBlockBtn)
            {   //Obj.compMove.direction should be set by colliding pro prior
                Functions_GameObject_Dungeon.FlipSwitchBlocks(Obj);
            }

            #endregion


            else
            {   //audible note something hit obj
                //Assets.Play(Assets.sfxActorLand);
            }
        }

        //level 1 caused by: sword, shovel, arrow, bat, bite/fang, thrown objs
        public static void Cut(GameObject Obj)
        {   //Obj.compMove.direction needs to be set by collider


            #region Grass

            if (Obj.type == ObjType.Wor_Grass_Tall)
            {
                //pop an attention particle on grass pos
                Functions_Particle.Spawn(ParticleType.Attention,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y);
                //convert tallgrass to cut grass + sfx
                Functions_GameObject.SetType(Obj, ObjType.Wor_Grass_Cut);
                Assets.Play(Assets.sfxBushCut);
                //rarely spawn loot
                if (Functions_Random.Int(0, 101) > 90) //cut that grass boi
                { Functions_Loot.SpawnLoot(Obj.compSprite.position); }
            }

            #endregion


            #region Pots, Dungeon pot (skull), Boat Barrels

            else if (
                Obj.type == ObjType.Wor_Pot
                || Obj.type == ObjType.Dungeon_Pot
                || Obj.type == ObjType.Wor_Boat_Barrel
                )
            {   //pop debris explosion
                Functions_Particle.Spawn_Explosion(
                    ParticleType.DebrisBrown,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y);
                //pop an attention particle
                Functions_Particle.Spawn(ParticleType.Attention,
                    Obj.compSprite.position.X, Obj.compSprite.position.Y);
                Functions_GameObject.Kill(Obj, true, true); //become loot & debris
                Assets.Play(Assets.sfxShatter);
            }

            #endregion


            #region Burned Posts

            else if (
                Obj.type == ObjType.Wor_PostBurned_Corner_Left
                || Obj.type == ObjType.Wor_PostBurned_Corner_Right
                || Obj.type == ObjType.Wor_PostBurned_Horizontal
                || Obj.type == ObjType.Wor_PostBurned_Vertical_Left
                || Obj.type == ObjType.Wor_PostBurned_Vertical_Right
                )
            { Functions_GameObject.Kill(Obj, true, true); }

            #endregion

            
            Bounce(Obj); //call all lower levels of destruction on obj
        }

        //level 2 caused by: hammer, spikeblock, floorspikes
        public static void Destroy(GameObject Obj)
        {   //Obj.compMove.direction needs to be set by collider


            #region Posts

            if (
                Obj.type == ObjType.Wor_Post_Corner_Left
                || Obj.type == ObjType.Wor_Post_Corner_Right
                || Obj.type == ObjType.Wor_Post_Horizontal
                || Obj.type == ObjType.Wor_Post_Vertical_Left
                || Obj.type == ObjType.Wor_Post_Vertical_Right
                )
            { Functions_GameObject.Kill(Obj, true, true); }

            #endregion


            #region Collapse Bombable Dungeon Doors

            else if (Obj.type == ObjType.Dungeon_DoorBombable)
            {   //blow up door, change to doorOpen
                Functions_Particle.Spawn(
                    ParticleType.Attention,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y);
                Assets.Play(Assets.sfxShatter);
                Functions_GameObject.SetType(Obj, ObjType.Dungeon_DoorOpen);
                //hide the sprite switch with a blast particle
                Functions_Particle.Spawn(ParticleType.Blast,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y);
                //update the dungeon data that we collapsed this door
                Functions_GameObject_Dungeon.SetDungeonDoor(Obj);
            }

            #endregion


            #region Crack Normal Dungeon Walls

            else if (Obj.type == ObjType.Dungeon_WallStraight)
            {   //'crack' normal walls
                Functions_GameObject.SetType(Obj,
                    ObjType.Dungeon_WallStraightCracked);
                Functions_Particle.Spawn(ParticleType.Blast,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y);
                Assets.Play(Assets.sfxShatter);
            }

            #endregion





            else if (

                //dungeon objs

                //limited set for now
                Obj.type == ObjType.Dungeon_Statue
                || Obj.type == ObjType.Dungeon_Signpost

                //world objs

                //building objs
                || Obj.type == ObjType.Wor_Build_Wall_FrontA
                || Obj.type == ObjType.Wor_Build_Wall_FrontB
                || Obj.type == ObjType.Wor_Build_Wall_Back
                || Obj.type == ObjType.Wor_Build_Wall_Side_Left
                || Obj.type == ObjType.Wor_Build_Wall_Side_Right
                || Obj.type == ObjType.Wor_Build_Door_Shut
                || Obj.type == ObjType.Wor_Build_Door_Open
                //building interior objs
                || Obj.type == ObjType.Wor_Bookcase
                || Obj.type == ObjType.Wor_Shelf
                || Obj.type == ObjType.Wor_Stove
                || Obj.type == ObjType.Wor_Sink
                || Obj.type == ObjType.Wor_TableSingle
                || Obj.type == ObjType.Wor_TableDoubleLeft
                || Obj.type == ObjType.Wor_TableDoubleRight
                || Obj.type == ObjType.Wor_Chair
                || Obj.type == ObjType.Wor_Bed
                )
            {
                Functions_GameObject.Kill(Obj, true, true);
            }









            else
            {   //call all lower levels of destruction on obj
                Cut(Obj);
            }
        }

        //level 3 caused by: explosions, bolts
        public static void Explode(GameObject Obj)
        {
            
            #region Bush

            if (Obj.type == ObjType.Wor_Bush) { Burn(Obj); }

            #endregion


            #region Trees - unburnt and burnt

            else if (Obj.type == ObjType.Wor_Tree
                || Obj.type == ObjType.Wor_Tree_Burnt)
            {
                Assets.Play(Assets.sfxShatter);
                //switch to tree stump
                Functions_GameObject.SetType(Obj, ObjType.Wor_Tree_Stump);
                //rarely spawn loot
                if (Functions_Random.Int(0, 101) > 80)
                { Functions_Loot.SpawnLoot(Obj.compSprite.position); }
                //pop the bushy/not bushy top part
                Functions_Particle.Spawn(
                    ParticleType.Attention,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y - 2);

                if (Obj.type == ObjType.Wor_Tree)
                {   //pop leaves in circular pattern toward top
                    Functions_Particle.Spawn_Explosion(
                        ParticleType.LeafGreen,
                        Obj.compSprite.position.X + 2,
                        Obj.compSprite.position.Y - 4, true);
                }
                else
                {   //pop debris in random dir from trunk
                    Functions_Particle.Spawn_Explosion(
                        ParticleType.LeafGreen,
                        Obj.compSprite.position.X + 2,
                        Obj.compSprite.position.Y + 0, false);
                }
                //spawn groundfire upon explosion
                Functions_Projectile.Spawn(
                    ProjectileType.GroundFire,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y - 3,
                    Direction.None);
            }

            #endregion


            #region Posts

            else if (//burned and unburned
                Obj.type == ObjType.Wor_Post_Vertical_Right
                || Obj.type == ObjType.Wor_Post_Corner_Right
                || Obj.type == ObjType.Wor_Post_Horizontal
                || Obj.type == ObjType.Wor_Post_Corner_Left
                || Obj.type == ObjType.Wor_Post_Vertical_Left

                || Obj.type == ObjType.Wor_PostBurned_Corner_Left
                || Obj.type == ObjType.Wor_PostBurned_Corner_Right
                || Obj.type == ObjType.Wor_PostBurned_Horizontal
                || Obj.type == ObjType.Wor_PostBurned_Vertical_Left
                || Obj.type == ObjType.Wor_PostBurned_Vertical_Right
                )
            {   //spawn groundfires upon explosion
                Functions_Projectile.Spawn(
                    ProjectileType.GroundFire,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y - 3,
                    Direction.None);
                Destroy(Obj);
            }

            #endregion

            
            else
            {   //call all lower levels of destruction on obj
                Destroy(Obj);
            }
        }








        //BURNING STATUS caused by: ground fire, fireball
        public static void Burn(GameObject Obj)
        {

            #region Grass

            if (Obj.type == ObjType.Wor_Grass_Tall)
            {   //spread the fire 
                Functions_Projectile.Spawn(
                    ProjectileType.GroundFire,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y - 3,
                    Direction.None);
                Destroy(Obj); //destroy grass as normal
            }

            #endregion


            #region Bush

            else if (Obj.type == ObjType.Wor_Bush)
            {   //spread the fire 
                Functions_Projectile.Spawn(
                    ProjectileType.GroundFire,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y - 3,
                    Direction.None);
                Assets.Play(Assets.sfxLightFire);
                Destroy(Obj); //destroy bush as normal
            }

            #endregion


            #region Posts

            else if (
                Obj.type == ObjType.Wor_Post_Vertical_Right
                || Obj.type == ObjType.Wor_Post_Corner_Right
                || Obj.type == ObjType.Wor_Post_Horizontal
                || Obj.type == ObjType.Wor_Post_Corner_Left
                || Obj.type == ObjType.Wor_Post_Vertical_Left
                )
            {   //switch to burned post
                if (Obj.type == ObjType.Wor_Post_Vertical_Right)
                { Functions_GameObject.SetType(Obj, ObjType.Wor_PostBurned_Vertical_Right); }
                else if (Obj.type == ObjType.Wor_Post_Corner_Right)
                { Functions_GameObject.SetType(Obj, ObjType.Wor_PostBurned_Corner_Right); }
                else if (Obj.type == ObjType.Wor_Post_Horizontal)
                { Functions_GameObject.SetType(Obj, ObjType.Wor_PostBurned_Horizontal); }
                else if (Obj.type == ObjType.Wor_Post_Corner_Left)
                { Functions_GameObject.SetType(Obj, ObjType.Wor_PostBurned_Corner_Left); }
                else if (Obj.type == ObjType.Wor_Post_Vertical_Left)
                { Functions_GameObject.SetType(Obj, ObjType.Wor_PostBurned_Vertical_Left); }

                //spread the fire 
                Functions_Projectile.Spawn(
                    ProjectileType.GroundFire,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y - 3,
                    Direction.None);
                Assets.Play(Assets.sfxLightFire);
            }

            #endregion


            #region Tree

            else if (Obj.type == ObjType.Wor_Tree)
            {   //switch to burned tree
                Functions_GameObject.SetType(Obj, ObjType.Wor_Tree_Burning);
                //place an initial fire at bottom of tree
                Functions_Particle.Spawn(ParticleType.Fire,
                    Obj.compSprite.position.X,
                    Obj.compSprite.position.Y + 16);
            }

            #endregion


            #region Floor Torch

            else if (Obj.type == ObjType.Dungeon_TorchUnlit)
            {
                Functions_GameObject_Dungeon.LightTorch(Obj);
            }

            #endregion


        }

        //FROZEN STATUS caused by: iceball
        public static void Freeze(GameObject Obj)
        {

        }










        //house roof methods

        public static void HideRoofs()
        {   //for over active roomObjs, hide any roof obj found
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active)
                {
                    if (Pool.roomObjPool[i].type == ObjType.Wor_Build_Roof_Bottom
                        || Pool.roomObjPool[i].type == ObjType.Wor_Build_Roof_Top
                        || Pool.roomObjPool[i].type == ObjType.Wor_Build_Roof_Chimney)
                    {
                        //instantly hide
                        //Pool.roomObjPool[i].compSprite.visible = false;
                        //fade hide
                        if (Pool.roomObjPool[i].compSprite.alpha > 0f)
                        { Pool.roomObjPool[i].compSprite.alpha -= 0.05f; }
                        if (Pool.roomObjPool[i].compSprite.alpha < 0f)
                        { Pool.roomObjPool[i].compSprite.alpha = 0f; }
                    }
                }
            }
        }

        public static void ShowRoofs()
        {   //for over active roomObjs, show any roof obj found
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active)
                {
                    if (Pool.roomObjPool[i].type == ObjType.Wor_Build_Roof_Bottom
                        || Pool.roomObjPool[i].type == ObjType.Wor_Build_Roof_Top
                        || Pool.roomObjPool[i].type == ObjType.Wor_Build_Roof_Chimney)
                    {
                        //instantly show
                        //Pool.roomObjPool[i].compSprite.visible = true;
                        //fade show
                        if (Pool.roomObjPool[i].compSprite.alpha < 1f)
                        { Pool.roomObjPool[i].compSprite.alpha += 0.05f; }
                        if (Pool.roomObjPool[i].compSprite.alpha > 1f)
                        { Pool.roomObjPool[i].compSprite.alpha = 1f; }
                    }
                }
            }
        }

        public static void CollapseRoof(GameObject Roof)
        {
            Assets.Play(Assets.sfxShatter); //play shatter sfx
            //turn roof into it's collapsing version
            Functions_GameObject.SetType(Roof, ObjType.Wor_Build_Roof_Collapsing);
        }

        



        //hero interaction rec methods

        public static void ReadSign(GameObject Sign)
        {
            //based on current roomid, signs point to diff dialogs
            //everything is based on currentRoom's ID, never levelID


            //field signposts
            if(LevelSet.currentLevel.currentRoom.roomID == RoomID.SkullIsland_Town)
            {
                Screens.Dialog.SetDialog(AssetsDialog.Signpost_ShadowTown);
                ScreenManager.AddScreen(Screens.Dialog);
            }
            else if (LevelSet.currentLevel.currentRoom.roomID == RoomID.SkullIsland_Colliseum)
            {
                Screens.Dialog.SetDialog(AssetsDialog.Signpost_ShadowColliseum);
                ScreenManager.AddScreen(Screens.Dialog);
            }

            else if (LevelSet.currentLevel.currentRoom.roomID == RoomID.ForestIsland_MainEntrance)
            {
                Screens.Dialog.SetDialog(AssetsDialog.Signpost_ForestEntrance);
                ScreenManager.AddScreen(Screens.Dialog);
            }
            else if (LevelSet.currentLevel.currentRoom.roomID == RoomID.DeathMountain_MainEntrance)
            {
                Screens.Dialog.SetDialog(AssetsDialog.Signpost_MountainEntrance);
                ScreenManager.AddScreen(Screens.Dialog);
            }



            //this is a temp hack for 0.77 release and will change in future commits
            else if (LevelSet.currentLevel.currentRoom.roomID == RoomID.SkullIsland_ShadowKing)
            {
                Screens.Dialog.SetDialog(AssetsDialog.Signpost_SwampEntrance);
                ScreenManager.AddScreen(Screens.Dialog);
            }







            //dungeon signposts
            else if (LevelSet.currentLevel.currentRoom.roomID == RoomID.Exit)
            {
                Screens.Dialog.SetDialog(AssetsDialog.Signpost_ExitRoom);
                ScreenManager.AddScreen(Screens.Dialog);
            }


            //hack for 0.77 - secret vendor exists in column room, find him, set signpost dialog
            else if (LevelSet.currentLevel.currentRoom.roomID == RoomID.Column)
            {
                //default dialog
                Screens.Dialog.SetDialog(AssetsDialog.Signpost_Standard);

                //loop over all roomObjs to find secret vendor obj
                for (i = 0; i < Pool.roomObjCount; i++)
                {
                    if(Pool.roomObjPool[i].active)
                    {
                        if(Pool.roomObjPool[i].type == ObjType.Vendor_NPC_EnemyItems)
                        {   //set the secret vendor dialog
                            Screens.Dialog.SetDialog(AssetsDialog.Signpost_SecretVendor);
                        }
                    }
                }

                //either add default dialog or secret vendor dialog
                ScreenManager.AddScreen(Screens.Dialog);
            }









            //editor dialogs
            else if(LevelSet.currentLevel.currentRoom.roomID == RoomID.DEV_Field)
            {
                Screens.Dialog.SetDialog(AssetsDialog.Signpost_CantRead);
                ScreenManager.AddScreen(Screens.Dialog);
            }
            else if(LevelSet.currentLevel.currentRoom.roomID == RoomID.DEV_Exit)
            {
                Screens.Dialog.SetDialog(AssetsDialog.Signpost_CantRead);
                ScreenManager.AddScreen(Screens.Dialog);
            }



            else
            {   //bare minimum, we pop a blank standard dialog
                Screens.Dialog.SetDialog(AssetsDialog.Signpost_Standard);
                ScreenManager.AddScreen(Screens.Dialog);
            }
        }

        public static void OpenHouseDoor(GameObject Obj)
        {
            //pop attention particle
            Functions_Particle.Spawn(
                ParticleType.Attention,
                Obj.compSprite.position.X,
                Obj.compSprite.position.Y);
            //switch to open door
            Functions_GameObject.SetType(Obj, ObjType.Wor_Build_Door_Open);
            //play an unlocking sound effect
            Assets.Play(Assets.sfxDoorOpen); //could be better
        }





    }
}