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
    public static class Functions_Battle
    {

        public static void Damage(Actor Actor, byte Damage, float Force, Direction Direction)
        {
            //only damage/hit/push actors not in the hit state
            if (Actor.state != ActorState.Hit)
            {   //deal damage to the actor
                //but prevent the damage byte from underflowing the Actor.health byte
                if (Damage > Actor.health) { Actor.health = 0; }
                else { Actor.health -= Damage; }

                //if projectile damaged hero, track the damage dealt
                if (Actor == Pool.hero) { DungeonRecord.totalDamage += Damage; }

                //set actor into hit state, push actor the projectile's direction
                Functions_Actor.SetHitState(Actor);
                MovementFunctions.Push(Actor.compMove, Direction, Force);
            }
        }

    }
}