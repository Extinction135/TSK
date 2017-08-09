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
    public class ScreenOverworldNew : Screen
    {
        ScreenRec background = new ScreenRec();
        ScreenRec overlay = new ScreenRec();
        public Scroll scroll;
        public static ComponentSprite map;
        public List<MapLocation> locations;
        public int i;

        public Actor hero;
        Direction cardinal;
        MapLocation currentLocation;
        Vector2 distance; //used in map movement routine
        float speed = 0.1f; //how fast hero moves between locations


        public ScreenOverworldNew() { this.name = "OverworldScreen"; }

        public override void LoadContent()
        {
            background.alpha = 1.0f;
            overlay.alpha = 1.0f;
            overlay.fadeInSpeed = 0.03f; //fade in slow

            scroll = new Scroll(new Vector2(16 * 3 - 8, 16 * 2 + 4), 34, 19);
            //open the scroll
            scroll.displayState = DisplayState.Opening;
            Assets.Play(Assets.sfxMapOpen);

            map = new ComponentSprite(Assets.overworldSheet,
                new Vector2(640/2, 360/2), new Byte4(0, 0, 0, 0), new Point(1280, 720));
            map.position.X = map.cellSize.X / 2;
            map.position.Y = map.cellSize.Y / 2;





            #region Create locations

            MapLocation castle = new MapLocation(true, new Vector2(442, 135));
            MapLocation gate = new MapLocation(true, new Vector2(421, 160));
            MapLocation colliseum = new MapLocation(true, new Vector2(306, 195));
            MapLocation colliseumRight = new MapLocation(false, new Vector2(365, 175));

            #endregion


            #region Set mapLocation's names

            castle.name = "Castle";
            gate.name = "Gate";
            colliseum.name = "Colliseum";
            colliseumRight.name = "Road";

            #endregion


            #region Set maplocation's neighbors

            castle.neighborDown = gate;
            castle.neighborLeft = gate;

            gate.neighborUp = castle;
            gate.neighborLeft = colliseumRight;

            colliseumRight.neighborRight = gate;
            colliseumRight.neighborLeft = colliseum;

            colliseum.neighborRight = colliseumRight;

            #endregion


            #region Add locations to locations list

            locations = new List<MapLocation>();
            locations.Add(castle);
            locations.Add(gate);
            locations.Add(colliseum);
            locations.Add(colliseumRight);

            #endregion





            currentLocation = colliseum;
            UpdateTitle();

            hero = new Actor();
            //set hero at the current location
            Functions_Movement.Teleport(hero.compMove,
                currentLocation.compSprite.position.X,
                currentLocation.compSprite.position.Y - 8);
            Functions_Component.Align(hero.compMove, hero.compSprite, hero.compCollision);

            // * in the future, we should place hero at the location of the level he left





            //play the title music
            Functions_Music.PlayMusic(Music.Title);
            //fill hero's health up to max - prevents drum track from playing
            Pool.hero.health = Pool.hero.maxHealth;
        }

        public override void HandleInput(GameTime GameTime)
        {
            if (scroll.displayState == DisplayState.Opened)
            {   //only allow input if the screen has opened completely


                #region Change Locations based on Input

                //hero can only move to a new location, if hero is at a location
                if(hero.compMove.position == hero.compMove.newPosition)
                {   //check to see if the gamePad direction is a new direction
                    if (Input.gamePadDirection != Input.lastGamePadDirection)
                    {   //get the cardinal direction of the gamepad input
                        cardinal = Functions_Direction.GetCardinalDirection(Input.gamePadDirection);
                        //set the currentLocation based on cardinal direction
                        if (cardinal == Direction.Up)
                        { currentLocation = currentLocation.neighborUp; }
                        if (cardinal == Direction.Right)
                        { currentLocation = currentLocation.neighborRight; }
                        if (cardinal == Direction.Down)
                        { currentLocation = currentLocation.neighborDown; }
                        if (cardinal == Direction.Left)
                        { currentLocation = currentLocation.neighborLeft; }
                        //change hero's animation to moving, inherit cardinal direction
                        hero.state = ActorState.Move;
                        hero.direction = cardinal;
                    }
                }

                #endregion


            }
        }

        public override void Update(GameTime GameTime)
        {
            if (scroll.displayState == DisplayState.Opening)
            {   //fade overlay out
                overlay.fadeState = FadeState.FadeOut;
                Functions_ScreenRec.Fade(overlay);
                Functions_Scroll.AnimateOpen(scroll);
            }
            else if(scroll.displayState == DisplayState.Opened)
            {

                #region Map Movement Routine

                //get hero's newPosition
                hero.compMove.newPosition.X = currentLocation.compSprite.position.X;
                hero.compMove.newPosition.Y = currentLocation.compSprite.position.Y - 8;

                //get distance between hero and location
                distance = hero.compMove.newPosition - hero.compMove.position;

                //check to see if hero is close enough to snap positions
                if (Math.Abs(distance.X) < 1)
                { hero.compMove.position.X = hero.compMove.newPosition.X; }
                if (Math.Abs(distance.Y) < 1)
                { hero.compMove.position.Y = hero.compMove.newPosition.Y; }
                //move hero towards location
                if (hero.compMove.position.X != hero.compMove.newPosition.X)
                { hero.compMove.position.X += distance.X * speed; }
                if (hero.compMove.position.Y != hero.compMove.newPosition.Y)
                { hero.compMove.position.Y += distance.Y * speed; }

                //align hero's sprite to current move position
                hero.compSprite.position.X = (int)hero.compMove.position.X;
                hero.compSprite.position.Y = (int)hero.compMove.position.Y;

                //check to see if hero just arrived at a location
                if (hero.state == ActorState.Move)
                {   //if hero just reached destination.. (moving + position match)
                    if (hero.compMove.position == hero.compMove.newPosition)
                    {   //set hero's animation to idle down, update map title
                        hero.state = ActorState.Idle;
                        hero.direction = Direction.Down;
                        UpdateTitle();
                    }
                }

                #endregion


                Functions_ActorAnimationList.SetAnimationGroup(hero);
                Functions_ActorAnimationList.SetAnimationDirection(hero);
                Functions_Animation.Animate(hero.compAnim, hero.compSprite);
            }
            else if (scroll.displayState == DisplayState.Closing)
            {   //fade overlay in
                overlay.fadeState = FadeState.FadeIn;
                Functions_ScreenRec.Fade(overlay);
                Functions_Scroll.AnimateClosed(scroll);
            }
            else if (scroll.displayState == DisplayState.Closed)
            {   
                /*
                //set the type of dungeon we are about to build/load
                if (currentlySelected == Shop)
                { Functions_Dungeon.dungeonType = DungeonType.Shop; }
                else { Functions_Dungeon.dungeonType = DungeonType.CursedCastle; }
                //dungeon is built by dungeon screen
                ScreenManager.ExitAndLoad(new ScreenDungeon());
                */

            }
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            Functions_Draw.Draw(background);
            Functions_Scroll.Draw(scroll);
            if (scroll.displayState == DisplayState.Opened)
            {
                Functions_Draw.Draw(map);
                for(i = 0; i < locations.Count; i++)
                { Functions_Draw.Draw(locations[i].compSprite); }
                Functions_Draw.Draw(hero.compSprite);
            }
            Functions_Draw.Draw(overlay);
            ScreenManager.spriteBatch.End();
        }



        public void UpdateTitle()
        { scroll.title.text = "Overworld Map - " + currentLocation.name; }

    }
}
