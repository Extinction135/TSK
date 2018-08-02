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

    public class WidgetActor
    {

        static int i;
        public MenuWindow window;
        public List<Actor> actors;
        public Boolean visible = false;

        public void Update()
        {
            Functions_MenuWindow.Update(window);
            if (window.interior.displayState == DisplayState.Opened)
            {
                //animate all the enemies
                for (i = 0; i < actors.Count; i++)
                {
                    if (actors[i] != null)
                    { Functions_Animation.Animate(actors[i].compAnim, actors[i].compSprite); }
                }
            }
        }

        public void Draw()
        {
            Functions_Draw.Draw(window);
            if (window.interior.displayState == DisplayState.Opened)
            {
                //draw all the enemies
                for (i = 0; i < actors.Count; i++)
                { if (actors[i] != null) { Functions_Draw.Draw(actors[i].compSprite); } }

                if (Flags.DrawCollisions) //draw roomObjEnemies collision recs
                {
                    for (i = 0; i < actors.Count; i++)
                    { if (actors[i] != null) { Functions_Draw.Draw(actors[i].compCollision); } }
                }
            }
        }

    }

    



    public class WidgetActors_Forest : WidgetActor
    {
        public WidgetActors_Forest()
        {
            //create and set the position of the window frame
            window = new MenuWindow(
                new Point(16 * 23, 16 * 2), //position
                new Point(16 * 5, 16 * 15), //size
                "Forest actors"); //title

            actors = new List<Actor>();
            //create actor, set its type, place actor, add to actors list

            //boss
            Actor boss = new Actor();
            boss.direction = Direction.Down;
            Functions_Actor.SetType(boss, ActorType.Boss_BigEye);
            Functions_Movement.Teleport(boss.compMove,
                window.interior.rec.X + 13+8,
                window.interior.rec.Y + 29+8);
            Functions_Component.Align(boss);
            boss.state = ActorState.Idle;
            actors.Add(boss);

            //miniboss
            Actor miniboss = new Actor();
            miniboss.direction = Direction.Down;
            Functions_Actor.SetType(miniboss, ActorType.MiniBoss_BlackEye);
            Functions_Movement.Teleport(miniboss.compMove,
                window.interior.rec.X + 13 + 8 + 16 * 2,
                window.interior.rec.Y + 29 + 8);
            Functions_Component.Align(miniboss);
            miniboss.state = ActorState.Idle;
            actors.Add(miniboss);

            //standard
            Actor standard = new Actor();
            standard.direction = Direction.Down;
            Functions_Actor.SetType(standard, ActorType.Standard_AngryEye);
            Functions_Movement.Teleport(standard.compMove,
                window.interior.rec.X + 13 + 8 + 16 * 2,
                window.interior.rec.Y + 29 + 8 + 16 * 2);
            Functions_Component.Align(standard);
            standard.state = ActorState.Idle;
            actors.Add(standard);
        }
    }

    public class WidgetActors_Mountain : WidgetActor
    {
        public WidgetActors_Mountain()
        {
            //create and set the position of the window frame
            window = new MenuWindow(
                new Point(16 * 23, 16 * 2), //position
                new Point(16 * 5, 16 * 15), //size
                "Mountain actors"); //title

            actors = new List<Actor>();
            //create actor, set its type, place actor, add to actors list

            
            //boss
            Actor boss = new Actor();
            boss.direction = Direction.Down;
            Functions_Actor.SetType(boss, ActorType.Boss_BigBat);
            Functions_Movement.Teleport(boss.compMove,
                window.interior.rec.X + 13 + 8,
                window.interior.rec.Y + 29 + 8);
            Functions_Component.Align(boss);
            boss.state = ActorState.Idle;
            actors.Add(boss);

            //miniboss
            Actor miniboss = new Actor();
            miniboss.direction = Direction.Down;
            Functions_Actor.SetType(miniboss, ActorType.MiniBoss_Spider_Armored);
            Functions_Movement.Teleport(miniboss.compMove,
                window.interior.rec.X + 13 + 8 + 16 * 2,
                window.interior.rec.Y + 29 + 8);
            Functions_Component.Align(miniboss);
            miniboss.state = ActorState.Idle;
            actors.Add(miniboss);

            //standard
            Actor standard = new Actor();
            standard.direction = Direction.Down;
            Functions_Actor.SetType(standard, ActorType.Standard_BeefyBat);
            Functions_Movement.Teleport(standard.compMove,
                window.interior.rec.X + 13 + 8 + 16 * 2,
                window.interior.rec.Y + 29 + 8 + 16 * 2);
            Functions_Component.Align(standard);
            standard.state = ActorState.Idle;
            actors.Add(standard);
        }
    }

    public class WidgetActors_Swamp : WidgetActor
    {
        public WidgetActors_Swamp()
        {
            //create and set the position of the window frame
            window = new MenuWindow(
                new Point(16 * 23, 16 * 2), //position
                new Point(16 * 5, 16 * 15), //size
                "Swamp actors"); //title

            actors = new List<Actor>();
            //create actor, set its type, place actor, add to actors list


            //boss
            Actor boss = new Actor();
            boss.direction = Direction.Down;
            Functions_Actor.SetType(boss, ActorType.Boss_OctoHead);
            Functions_Movement.Teleport(boss.compMove,
                window.interior.rec.X + 13 + 8,
                window.interior.rec.Y + 29 + 8);
            Functions_Component.Align(boss);
            boss.state = ActorState.Idle;
            actors.Add(boss);

            //miniboss
            Actor miniboss = new Actor();
            miniboss.direction = Direction.Down;
            Functions_Actor.SetType(miniboss, ActorType.MiniBoss_OctoMouth);
            Functions_Movement.Teleport(miniboss.compMove,
                window.interior.rec.X + 13 + 8 + 16 * 2,
                window.interior.rec.Y + 29 + 8);
            Functions_Component.Align(miniboss);
            miniboss.state = ActorState.Idle;
            actors.Add(miniboss);

            //standard
            Actor standard = new Actor();
            standard.direction = Direction.Down;
            Functions_Actor.SetType(standard, ActorType.Standard_BeefyBat);
            Functions_Movement.Teleport(standard.compMove,
                window.interior.rec.X + 13 + 8 + 16 * 2,
                window.interior.rec.Y + 29 + 8 + 16 * 2);
            Functions_Component.Align(standard);
            standard.state = ActorState.Idle;
            actors.Add(standard);

            //special - tentacle
            Actor special = new Actor();
            special.direction = Direction.Down;
            Functions_Actor.SetType(special, ActorType.Special_Tentacle);
            Functions_Movement.Teleport(special.compMove,
                window.interior.rec.X + 13 + 8,
                window.interior.rec.Y + 29 + 8 + 16 * 4);
            Functions_Component.Align(special);
            special.state = ActorState.Idle;
            actors.Add(special);
        }
    }




    public class WidgetActors_Town : WidgetActor
    {
        public WidgetActors_Town()
        {
            //create and set the position of the window frame
            window = new MenuWindow(
                new Point(16 * 23, 16 * 2), //position
                new Point(16 * 5, 16 * 15), //size
                "Town actors"); //title

            actors = new List<Actor>();
            //create actor, set its type, place actor, add to actors list

            /*
            //boss
            Actor boss = new Actor();
            boss.direction = Direction.Down;
            Functions_Actor.SetType(boss, ActorType.Boss_BigEye);
            Functions_Movement.Teleport(boss.compMove,
                window.interior.rec.X + 13 + 8,
                window.interior.rec.Y + 29 + 8);
            Functions_Component.Align(boss);
            boss.state = ActorState.Idle;
            actors.Add(boss);

            
            //miniboss
            Actor miniboss = new Actor();
            miniboss.direction = Direction.Down;
            Functions_Actor.SetType(miniboss, ActorType.MiniBoss_BlackEye);
            Functions_Movement.Teleport(miniboss.compMove,
                window.interior.rec.X + 13 + 8 + 16 * 2,
                window.interior.rec.Y + 29 + 8);
            Functions_Component.Align(miniboss);
            miniboss.state = ActorState.Idle;
            actors.Add(miniboss);
            */

            //standard
            Actor standard = new Actor();
            standard.direction = Direction.Down;
            Functions_Actor.SetType(standard, ActorType.Blob);
            Functions_Movement.Teleport(standard.compMove,
                window.interior.rec.X + 13 + 8 + 16 * 2,
                window.interior.rec.Y + 29 + 8 + 16 * 2);
            Functions_Component.Align(standard);
            standard.state = ActorState.Idle;
            actors.Add(standard);
        }
    }






}