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


        
        //used to iterate over the screen
        static Point Total_ScreenSize = new Point(640 / 16, 360 / 16); //x total, y total


        //bombos fields
        static Boolean Casting_Bombos = false; //master flag
        static Point Bombos_ActiveCounter = new Point(0, 6); //counter, total (processes bombos routine)
        static int Bombos_lifetime = 40; //entire screen of destruction
        static int Bombos_counter = 0; //counts up to lifetime
        static Point Bombos_spawnPos = new Point(); //used to place explosions on screen
        static int Bombos_ExpCounter = 0; //explosion counter



        




        public static void Update()
        {
            //allows magic system to cast across frames



            #region Bombos

            if(Casting_Bombos) //incremently place explosions left to right across room
            {
                Bombos_ActiveCounter.X++; //increment active counter
                if (Bombos_ActiveCounter.X >= Bombos_ActiveCounter.Y)
                {   //reset counter, process bombos
                    Bombos_ActiveCounter.X = 0;
                    //place explosions vertically in screen space, project to world space,
                    //check overlaps with hero (prevent spawns), then spawn exps.
                    //this works for both fields and dungeons and isn't too complicated
                    //plus it limits explosions produced by bombos to be onscreen
                    for (Bombos_ExpCounter = 0; Bombos_ExpCounter < Total_ScreenSize.Y + 1; Bombos_ExpCounter++)
                    {   //sequentially cover screen
                        Bombos_spawnPos.X = Bombos_counter * 16; //row id
                        Bombos_spawnPos.Y = Bombos_ExpCounter * 16; //column id
                        //project from screen to world
                        Bombos_spawnPos = Functions_Camera2D.ConvertScreenToWorld(Bombos_spawnPos.X, Bombos_spawnPos.Y);
                        //prevent explosions spawn ontop of hero (per frame)
                        if (
                            (Math.Abs(Pool.hero.compSprite.position.X - Bombos_spawnPos.X) < 28)
                            & //per axis control over explosion checking pls
                            (Math.Abs(Pool.hero.compSprite.position.Y - Bombos_spawnPos.Y) < 28)
                        )
                        { } //do nothing
                        else
                        {   //it's ok to spawn explosion at this position
                            Functions_Projectile.Spawn(ProjectileType.Explosion,
                                Bombos_spawnPos.X, Bombos_spawnPos.Y, Direction.Down);
                        }
                    }
                    //increment counter, flipping control bool when complete
                    Bombos_counter++;
                    if (Bombos_counter >= Bombos_lifetime) { Casting_Bombos = false; }
                }

            }

            #endregion





        }

        public static void StopAll()
        {
            Casting_Bombos = false;
        }














        public static void Cast(SpellType Spell, Actor Caster)
        {
            //resolve caster direction to NSEW cardinal - no casting diagonally
            Caster.direction = Functions_Direction.GetCardinalDirection_LeftRight(Caster.direction);

            //based on Spell value, we call Cast() methods below
            if (Spell == SpellType.None)
            {   //simply tell the player no spell was cast
                //Assets.Play(Assets.sfxError); //silently fail
            }




            //wind spells



            //explosive/fire spells

            #region Fire

            else if (Spell == SpellType.Fire)
            {
                Assets.Play(Assets.sfxLightFire);
                //as close as possible on the offsets
                if (Caster.direction == Direction.Up)
                {
                    Functions_Projectile.Spawn(
                        ProjectileType.GroundFire,
                        Caster.compCollision.rec.Center.X,
                        Caster.compCollision.rec.Center.Y - 16,
                        Direction.Down);
                }
                else if (Caster.direction == Direction.Down)
                {
                    Functions_Projectile.Spawn(
                        ProjectileType.GroundFire,
                        Caster.compCollision.rec.Center.X,
                        Caster.compCollision.rec.Center.Y + 16,
                        Direction.Down);
                }
                else if (Caster.direction == Direction.Right)
                {
                    Functions_Projectile.Spawn(
                        ProjectileType.GroundFire,
                        Caster.compCollision.rec.Center.X + 16,
                        Caster.compCollision.rec.Center.Y,
                        Direction.Down);
                }
                else if (Caster.direction == Direction.Left)
                {
                    Functions_Projectile.Spawn(
                        ProjectileType.GroundFire,
                        Caster.compCollision.rec.Center.X - 16,
                        Caster.compCollision.rec.Center.Y,
                        Direction.Down);
                }
            }

            #endregion


            #region Firewalk

            else if(Spell == SpellType.Fire_Walk)
            {
                Assets.Play(Assets.sfxLightFire);
                Functions_Projectile.Spawn(ProjectileType.Emitter_GroundFire, Caster, Direction.Down);
            }

            #endregion


            #region Single Explosion

            else if (Spell == SpellType.Explosive_Single)
            {   //as close as possible on the offsets
                if(Caster.direction == Direction.Up)
                {
                    Functions_Projectile.Spawn(
                        ProjectileType.Explosion,
                        Caster.compCollision.rec.Center.X,
                        Caster.compCollision.rec.Center.Y - 20,
                        Direction.Up);
                }
                else if (Caster.direction == Direction.Down)
                {
                    Functions_Projectile.Spawn(
                        ProjectileType.Explosion,
                        Caster.compCollision.rec.Center.X,
                        Caster.compCollision.rec.Center.Y + 20,
                        Direction.Down);
                }
                else if (Caster.direction == Direction.Right)
                {
                    Functions_Projectile.Spawn(
                        ProjectileType.Explosion,
                        Caster.compCollision.rec.Center.X + 20,
                        Caster.compCollision.rec.Center.Y,
                        Direction.Right);
                }
                else if (Caster.direction == Direction.Left)
                {
                    Functions_Projectile.Spawn(
                        ProjectileType.Explosion,
                        Caster.compCollision.rec.Center.X - 20,
                        Caster.compCollision.rec.Center.Y,
                        Direction.Left);
                }
            }

            #endregion


            #region Chain Explosions

            else if (Spell == SpellType.Explosive_Line)
            {
                if (Caster.direction == Direction.Up)
                {
                    Functions_Projectile.Spawn(
                        ProjectileType.Emitter_Explosion,
                        Caster.compCollision.rec.Center.X,
                        Caster.compCollision.rec.Center.Y - 20,
                        Direction.Up);
                }
                else if (Caster.direction == Direction.Down)
                {
                    Functions_Projectile.Spawn(
                        ProjectileType.Emitter_Explosion,
                        Caster.compCollision.rec.Center.X,
                        Caster.compCollision.rec.Center.Y + 20,
                        Direction.Down);
                }
                else if (Caster.direction == Direction.Right)
                {
                    Functions_Projectile.Spawn(
                        ProjectileType.Emitter_Explosion,
                        Caster.compCollision.rec.Center.X + 20,
                        Caster.compCollision.rec.Center.Y,
                        Direction.Right);
                }
                else if (Caster.direction == Direction.Left)
                {
                    Functions_Projectile.Spawn(
                        ProjectileType.Emitter_Explosion,
                        Caster.compCollision.rec.Center.X - 20,
                        Caster.compCollision.rec.Center.Y,
                        Direction.Left);
                }
            }

            #endregion


            #region Bombos

            else if (Spell == SpellType.Explosive_Bombos)
            {   //reset counters, flip flag
                Bombos_counter = 0;
                //Bombos_spread = 0;
                Bombos_ActiveCounter.X = 0; //start process this frame
                Casting_Bombos = true; //flip flag
                Assets.Play(Assets.sfxExplosionsMultiple); //play sfx
            }

            #endregion





            //ice spells

            #region Ice Walk

            else if (Spell == SpellType.Ice_FreezeWalk)
            {
                Assets.Play(Assets.sfxIcerod);
                Functions_Projectile.Spawn(ProjectileType.Emitter_IceTile, Caster, Direction.Down);
            }

            #endregion


            #region Freeze Ground

            else if (Spell == SpellType.Ice_FreezeGround)
            {   //cast and play sfx
                Cast_FreezeGround(Caster.compSprite.position);
                Assets.Play(Assets.sfxIcerod);
            }

            #endregion





            //electrical spells

            #region Ether

            else if (Spell == SpellType.Lightning_Ether)
            {
                Cast_Ether();
            }

            #endregion











            //summon spells

            #region Summons

            else if(Spell == SpellType.Summon_Bat_Projectile)
            {   //shoot bat from caster's direction
                Functions_Projectile.Spawn(ProjectileType.Bat, Caster, Caster.direction);
            }
            else if(Spell == SpellType.Summon_Bat_Explosion)
            {
                Cast_Bat_Explosion(Caster);
            }

            #endregion

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
            for (i = 0; i < Pool.intObjCount; i++)
            {
                if (Pool.intObjPool[i].active)
                {
                    if (Pool.intObjPool[i].group == InteractiveGroup.Enemy)
                    { castPositions.Add(Pool.intObjPool[i].compSprite.position); }
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

        











        //any actor cast spells
        static int iceCounter = 0;
        static InteractiveObject iceTile;
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
                iceTile = Functions_Pool.GetIntObj();
                Functions_InteractiveObjs.SetType(iceTile, InteractiveType.IceTile);
                Functions_Movement.Teleport(iceTile.compMove, icePosRef.X, icePosRef.Y);
                Functions_Component.Align(iceTile);
                //note ice tile birth with attention particle
                Functions_Particle.Spawn(ParticleType.Attention,
                    icePosRef.X, icePosRef.Y);
            }
        }




        

        public static void Cast_Bat_Explosion(Actor Caster)
        {   //spawn bats all around, and away from caster
            Functions_Projectile.Spawn(ProjectileType.Bat, Caster, Direction.Down);
            Functions_Projectile.Spawn(ProjectileType.Bat, Caster, Direction.DownRight);
            Functions_Projectile.Spawn(ProjectileType.Bat, Caster, Direction.Right);
            Functions_Projectile.Spawn(ProjectileType.Bat, Caster, Direction.UpRight);
            Functions_Projectile.Spawn(ProjectileType.Bat, Caster, Direction.Up);
            Functions_Projectile.Spawn(ProjectileType.Bat, Caster, Direction.UpLeft);
            Functions_Projectile.Spawn(ProjectileType.Bat, Caster, Direction.Left);
            Functions_Projectile.Spawn(ProjectileType.Bat, Caster, Direction.DownLeft);
        }




        






        /*
        static int bombosSpread = 22;
        static void Cast_Bombos()
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


            
            Assets.Play(Assets.sfxExplosion); //play sfx
        }
        */





    }
}