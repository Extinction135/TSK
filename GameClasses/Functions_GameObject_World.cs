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


        //these functions refer to objects that are normally not part of a dungeon






        //cuts

        public static void CutTallGrass(GameObject TallGrass)
        {   //pop an attention particle on grass pos
            Functions_Particle.Spawn(ObjType.Particle_Attention,
                TallGrass.compSprite.position.X,
                TallGrass.compSprite.position.Y);
            //convert tallgrass to cut grass + sfx
            Functions_GameObject.SetType(TallGrass, ObjType.Wor_Grass_Cut);
            Assets.Play(Assets.sfxBushCut);
            //rarely spawn loot
            if (Functions_Random.Int(0, 101) > 90) //cut that grass boi
            { Functions_Loot.SpawnLoot(TallGrass.compSprite.position); }
        }





        //objects transforming from burning/fire effects

        public static void BurnTree(GameObject Tree)
        {   //switch to burned tree
            Functions_GameObject.SetType(Tree, ObjType.Wor_Tree_Burning);

            //place an initial fire at bottom of tree
            Functions_Particle.Spawn(ObjType.Particle_Fire,
                Tree.compSprite.position.X,
                Tree.compSprite.position.Y + 16);
        }

        public static void BurnPost(GameObject Post)
        {
            //switch to burned post
            if (Post.type == ObjType.Wor_Post_Vertical_Right)
            { Functions_GameObject.SetType(Post, ObjType.Wor_PostBurned_Vertical_Right); }
            else if (Post.type == ObjType.Wor_Post_Corner_Right)
            { Functions_GameObject.SetType(Post, ObjType.Wor_PostBurned_Corner_Right); }
            else if (Post.type == ObjType.Wor_Post_Horizontal)
            { Functions_GameObject.SetType(Post, ObjType.Wor_PostBurned_Horizontal); }
            else if (Post.type == ObjType.Wor_Post_Corner_Left)
            { Functions_GameObject.SetType(Post, ObjType.Wor_PostBurned_Corner_Left); }
            else if (Post.type == ObjType.Wor_Post_Vertical_Left)
            { Functions_GameObject.SetType(Post, ObjType.Wor_PostBurned_Vertical_Left); }
        }



        //objects transforming from explosions

        public static void BlowUpTree(GameObject Tree, Boolean popLeaves)
        {
            Assets.Play(Assets.sfxShatter);
            //switch to tree stump
            Functions_GameObject.SetType(Tree, ObjType.Wor_Tree_Stump);
            //rarely spawn loot
            if (Functions_Random.Int(0, 101) > 80)
            { Functions_Loot.SpawnLoot(Tree.compSprite.position); }

            if (popLeaves)
            {   //pop the bushy top part
                Functions_Particle.Spawn(
                    ObjType.Particle_Attention,
                    Tree.compSprite.position.X,
                    Tree.compSprite.position.Y - 2);
                //pop leaves in circular decorative pattern for tree top
                Functions_Particle.Spawn_Explosion(
                    ObjType.Particle_Leaf,
                    Tree.compSprite.position.X + 2,
                    Tree.compSprite.position.Y - 4, true);
            }
            else
            {   //pop debris
                Functions_Particle.Spawn_Explosion(
                    ObjType.Particle_Debris,
                    Tree.compSprite.position.X + 2,
                    Tree.compSprite.position.Y + 4, true);
            }
        }

        public static void BlowUpPost(GameObject Post)
        {
            //posts spawn groundfires upon explosion
            Functions_Projectile.Spawn(
                ObjType.ProjectileGroundFire,
                Post.compSprite.position.X,
                Post.compSprite.position.Y - 3,
                Direction.None);
            Functions_GameObject.Kill(Post, true, true);
        }



        //unique object transformations

        public static void DestroyBush(GameObject Bush)
        {   //pop leaf explosion
            Functions_Particle.Spawn_Explosion(
                ObjType.Particle_Leaf,
                Bush.compSprite.position.X,
                Bush.compSprite.position.Y);
            //covert bush to stump, play sfx
            Functions_GameObject.SetType(Bush, ObjType.Wor_Bush_Stump);
            Assets.Play(Assets.sfxBushCut);
            //pop an attention particle
            Functions_Particle.Spawn(ObjType.Particle_Attention,
                Bush.compSprite.position.X, Bush.compSprite.position.Y);
            //rarely spawn loot
            Functions_Loot.SpawnLoot(Bush.compSprite.position, 20);
        }










        //world doors (not dungeon doors)

        public static void OpenBuildingDoor(GameObject Door)
        {   //pop attention particle
            Functions_Particle.Spawn(
                ObjType.Particle_Attention,
                Door.compSprite.position.X,
                Door.compSprite.position.Y);
            //switch to open door
            Functions_GameObject.SetType(Door, ObjType.Wor_Build_Door_Open);
            //play an unlocking sound effect
            Assets.Play(Assets.sfxDoorOpen); //could be better
        }






        //roofs

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






        //signs

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


            //dungeon signposts
            else if (LevelSet.currentLevel.currentRoom.roomID == RoomID.Exit)
            {
                Screens.Dialog.SetDialog(AssetsDialog.Signpost_ExitRoom);
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


       


    }
}