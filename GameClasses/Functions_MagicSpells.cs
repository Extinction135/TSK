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
    public static class Functions_MagicSpells
    {
        //dumping ground for magic/spells
        static int i;






        public static void Cast(SpellType Spell)
        {
            //based on Spell value, we call Cast() methods below

            if(Spell == SpellType.Lightning_Ether)
            {
                Cast_Ether();
            }
            else if(Spell == SpellType.Explosive_Bombos)
            {
                Cast_Bombos();
            }
        }



        


        
        //hero cast only spells
        static int castCounter = 0;
        static int totalCount = 0;
        static int counterB = 0;
        public static List<Vector2> castPositions;

        static void Cast_Ether()
        {
            //for each active enemy actor and roomObj
            //create a lightning bolt particle - up from their position
            //do this many times, so we have bolts going up from all enemies
            //then create explosions at enemy locations

            castPositions = new List<Vector2>();

            //collect enemy actor positions
            for(i = 0; i < Pool.actorCount; i++)
            {
                if (Pool.actorPool[i].active)
                {
                    if(Pool.actorPool[i].enemy)
                    { castPositions.Add(Pool.actorPool[i].compSprite.position); }
                }
            }
            //collect enemy object positions
            for (i = 0; i < Pool.roomObjCount; i++)
            {
                if (Pool.roomObjPool[i].active)
                {
                    if (Pool.roomObjPool[i].group == ObjGroup.Enemy)
                    { castPositions.Add(Pool.roomObjPool[i].compSprite.position); }
                }
            }

            //loop all collected positions, creating explosions and bolts
            totalCount = castPositions.Count;
            //spawn bolts + explosions at all positions
            for (castCounter = 0; castCounter < totalCount; castCounter++)
            {   //explosions to damage actors/objs
                Functions_Projectile.Spawn(ProjectileType.Explosion,
                    castPositions[castCounter].X,
                    castPositions[castCounter].Y,
                    Direction.None);
                for (counterB = 1; counterB < 5; counterB++)
                {   //place bolts centered, going up from position, for decor
                    Functions_Particle.Spawn(ParticleType.LightningBolt,
                        castPositions[castCounter].X,
                        (castPositions[castCounter].Y + 4) - (16 * counterB),
                        Direction.Down);
                }
            }
            Assets.Play(Assets.sfxShock); //play sfx
        }



        static int bombosSpread = 22;
        public static void Cast_Bombos()
        {
            //create explosions around hero
            //-this could look alot better, if we used 5 point or 7 point circle
            //instead of square 3x3 setup


            #region Cardinals

            //up
            Functions_Projectile.Spawn(ProjectileType.Explosion,
                Pool.hero.compMove.position.X,
                Pool.hero.compMove.position.Y - bombosSpread,
                Direction.Down);
            //down
            Functions_Projectile.Spawn(ProjectileType.Explosion,
                Pool.hero.compMove.position.X,
                Pool.hero.compMove.position.Y + bombosSpread,
                Direction.Down);
            //left
            Functions_Projectile.Spawn(ProjectileType.Explosion,
                Pool.hero.compMove.position.X - bombosSpread,
                Pool.hero.compMove.position.Y,
                Direction.Down);
            //down
            Functions_Projectile.Spawn(ProjectileType.Explosion,
                Pool.hero.compMove.position.X + bombosSpread,
                Pool.hero.compMove.position.Y,
                Direction.Down);

            #endregion


            #region Diagonals

            //up R/L
            Functions_Projectile.Spawn(ProjectileType.Explosion,
                Pool.hero.compMove.position.X - bombosSpread,
                Pool.hero.compMove.position.Y - bombosSpread,
                Direction.Down);
            Functions_Projectile.Spawn(ProjectileType.Explosion,
                Pool.hero.compMove.position.X + bombosSpread,
                Pool.hero.compMove.position.Y - bombosSpread,
                Direction.Down);

            //down R/L
            Functions_Projectile.Spawn(ProjectileType.Explosion,
                Pool.hero.compMove.position.X - bombosSpread,
                Pool.hero.compMove.position.Y + bombosSpread,
                Direction.Down);
            Functions_Projectile.Spawn(ProjectileType.Explosion,
                Pool.hero.compMove.position.X + bombosSpread,
                Pool.hero.compMove.position.Y + bombosSpread,
                Direction.Down);

            #endregion


            Assets.Play(Assets.sfxExplosionsMultiple); //play sfx
        }







        //any actor cast spells
        static int iceCounter = 0;
        static GameObject iceTile;
        static Vector2 icePosRef = new Vector2();
        public static void Cast_FreezeGround(Vector2 pos)
        {   //align pos to grid
            pos = Functions_Movement.AlignToGrid((int)pos.X, (int)pos.Y);
            //place icetiles at pos, and NSEW + DIAG
            for (iceCounter = 0; iceCounter < 9; iceCounter++)
            {   //top
                if (iceCounter == 0) //top left
                { icePosRef.X = pos.X - 16; icePosRef.Y = pos.Y - 16; }
                else if (iceCounter == 1) //top mid
                { icePosRef.X = pos.X + 0; icePosRef.Y = pos.Y - 16; }
                else if (iceCounter == 2) //top rite
                { icePosRef.X = pos.X + 16; icePosRef.Y = pos.Y - 16; }
                //mid
                else if (iceCounter == 3) //left
                { icePosRef.X = pos.X - 16; icePosRef.Y = pos.Y + 0; }
                else if (iceCounter == 4) //mid
                { icePosRef.X = pos.X + 0; icePosRef.Y = pos.Y + 0; }
                else if (iceCounter == 5) //rite
                { icePosRef.X = pos.X + 16; icePosRef.Y = pos.Y + 0; }
                //bottom
                else if (iceCounter == 6) //bot left
                { icePosRef.X = pos.X - 16; icePosRef.Y = pos.Y + 16; }
                else if (iceCounter == 7) //bot mid
                { icePosRef.X = pos.X + 0; icePosRef.Y = pos.Y + 16; }
                else if (iceCounter == 8) //bot rite
                { icePosRef.X = pos.X + 16; icePosRef.Y = pos.Y + 16; }

                //place ice tile at offset
                iceTile = Functions_Pool.GetRoomObj();
                Functions_GameObject.SetType(iceTile, ObjType.Dungeon_IceTile);
                Functions_Movement.Teleport(iceTile.compMove, icePosRef.X, icePosRef.Y);
                Functions_Component.Align(iceTile);
                //note ice tile birth with attention particle
                Functions_Particle.Spawn(ParticleType.Attention,
                    icePosRef.X, icePosRef.Y);
            }
        }





        
    }
}