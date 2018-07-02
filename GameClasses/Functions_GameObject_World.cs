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

        public static void BurnTree(GameObject Tree)
        {   //switch to burned tree
            Functions_GameObject.SetType(Tree, ObjType.Wor_Tree_Burning);

            //place an initial fire at bottom of tree
            Functions_Particle.Spawn(ObjType.Particle_Fire,
                Tree.compSprite.position.X,
                Tree.compSprite.position.Y + 16);
        }

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

        public static void OpenFencedGate(GameObject Gate)
        {   //pop attention particle
            Functions_Particle.Spawn(
                ObjType.Particle_Attention,
                Gate.compSprite.position.X,
                Gate.compSprite.position.Y);
            //remove the gate
            Functions_Pool.Release(Gate);
            //play an unlocking sound effect
            Assets.Play(Assets.sfxDoorOpen); //could be better
        }

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


        public static void ReadSign(GameObject Sign)
        {
            //based on level.id, and sometimes roomID, signs point to diff dialogs
            if (Level.ID == LevelID.Forest_Entrance)
            {
                ScreenManager.AddScreen(new ScreenDialog(AssetsDialog.Signpost_ForestEntrance));
            }
            else if(Level.ID == LevelID.LeftTown2)
            {
                ScreenManager.AddScreen(new ScreenDialog(AssetsDialog.Signpost_LeftTown2));
            }
            else if (Level.ID == LevelID.TheFarm)
            {
                ScreenManager.AddScreen(new ScreenDialog(AssetsDialog.Signpost_TheFarm));
            }
            else if (Level.ID == LevelID.Mountain_Entrance)
            {
                ScreenManager.AddScreen(new ScreenDialog(AssetsDialog.Signpost_MountainEntrance));
            }


            else
            {   //bare minimum, we pop a blank standard dialog
                ScreenManager.AddScreen(new ScreenDialog(AssetsDialog.Signpost_Standard));
            }
        }
    }
}