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


    public static class Functions_Overworld
    {
        public static void OpenMap()
        {
            //based on the last level that hero was on, load proper overworld
            if(Flags.PrintOutput)
            {
                Debug.WriteLine(
                    "Functions_Overworld.OpenMap(): current level id: " + 
                    LevelSet.currentLevel.ID);
            }

            //all levels point to shadowking overworld
            ScreenManager.ExitAndLoad(Screens.Overworld_ShadowKing);
        }
    }


    //there can be multiple overworld screens, this is the base class they inherit from
    public class ScreenOverworld : Screen
    {
        public ScreenRec overlay = new ScreenRec();
        //default map to shadowking texture
        public static ComponentSprite map;
        public List<MapLocation> locations;
        public int i;
        
        public Direction cardinal;
        public MapLocation currentLocation; //where hero is currently
        public MapLocation targetLocation; //where hero is going to

        public Vector2 distance; //used in map movement routine
        public float speed = 0.1f; //how fast hero moves between locations
        public List<Vector2> waveSpawnPositions; //where wave particles can spawn
        public Vector2 wavePos; //points to one of the vector2s in waveSpawnPositions list


        public override void Open()
        {
            overlay.alpha = 1.0f;
            overlay.fadeInSpeed = 0.04f;
            overlay.fadeOutSpeed = 0.04f;
            //open the screen
            Assets.Play(Assets.sfxMapOpen);
            displayState = DisplayState.Opening;
        }

        public override void HandleInput(GameTime GameTime)
        {

            #region GamePlay Input

            if (displayState == DisplayState.Opened)
            {   //only allow input if the screen has opened completely..
                if (Pool.hero.compMove.position == Pool.hero.compMove.newPosition)
                {   //only allow player input if hero currently occupies a map location
                    if (Input.gamePadDirection != Input.lastGamePadDirection)
                    {   //get the cardinal direction of new gamepad input
                        cardinal = Functions_Direction.GetCardinalDirection(Input.gamePadDirection);
                        //set the currentLocation based on cardinal direction
                        if (cardinal == Direction.Up)
                        { targetLocation = currentLocation.neighborUp; }
                        if (cardinal == Direction.Right)
                        { targetLocation = currentLocation.neighborRight; }
                        if (cardinal == Direction.Down)
                        { targetLocation = currentLocation.neighborDown; }
                        if (cardinal == Direction.Left)
                        { targetLocation = currentLocation.neighborLeft; }
                        //mapLocation neighbors point to self mapLocation by default
                        if (currentLocation != targetLocation)
                        {   //if mapLocation doesn't point to itself, then hero can move to target location
                            //change hero's animation to moving, inherit cardinal direction
                            Pool.hero.state = ActorState.Move;
                            Pool.hero.direction = cardinal;
                            Assets.Play(Assets.sfxMapWalking);
                            //spawn a dash puff at the hero's feet
                            Functions_Particle.Spawn(ObjType.Particle_RisingSmoke,
                                Pool.hero.compSprite.position.X,
                                Pool.hero.compSprite.position.Y + 4);
                        }
                        else//if hero dies, he appears on map sitting
                        {   //pressing a direction will make him stand back up
                            Pool.hero.state = ActorState.Idle;
                            //hero.direction = Direction.Down;
                            //face hero in the direction of the input
                            Pool.hero.direction = cardinal;
                        }
                    }
                    //check to see if player wants to load a level
                    if (Functions_Input.IsNewButtonPress(Buttons.A))
                    {   //upon A button press, check to see if current location is a level
                        if (currentLocation.isLevel) //if so, close the scroll
                        {
                            //setup field level hero is about to enter
                            LevelSet.field.ID = currentLocation.ID;
                            LevelSet.currentLevel = LevelSet.field;
                            //save currentLocation into player data
                            PlayerData.current.lastLocation = currentLocation.ID;

                            //animate link into reward state
                            Pool.hero.state = ActorState.Reward;
                            Pool.hero.direction = Direction.Down;
                            displayState = DisplayState.Closing;
                            //force an animation update
                            Functions_Animation.Animate(Pool.hero.compAnim, Pool.hero.compSprite);
                            Assets.Play(Assets.sfxTextDone);
                            Assets.Play(Assets.sfxWindowClose);

                            //setup hero's spawnPos for field level
                            Functions_Hero.ResetFieldSpawnPos();
                        }
                    }
                    else if (Functions_Input.IsNewButtonPress(Buttons.Start))
                    { ScreenManager.AddScreen(Screens.Inventory); }
                }
            }

            #endregion


            #region Editor Input

            if(Flags.bootRoutine == BootRoutine.Editor_Map)
            {
                //here we can click, grab, drag, release and get pos info for locations
            }

            #endregion

        }

        public override void Update(GameTime GameTime)
        {
            //always animate the hero
            Functions_Actor.SetAnimationGroup(Pool.hero);
            Functions_Actor.SetAnimationDirection(Pool.hero);
            Functions_Animation.Animate(Pool.hero.compAnim, Pool.hero.compSprite);
            //update hero's zdepth
            Functions_Component.SetZdepth(Pool.hero.compSprite);

            //always track the hero
            //(later locations will have an offset to pull attention when necessary)
            Camera2D.targetPosition.X = Pool.hero.compSprite.position.X;
            Camera2D.targetPosition.Y = Pool.hero.compSprite.position.Y;
            Functions_Camera2D.Update();

            //we used to display the location 'name' like this
            //scroll.title.text = "Overworld Map - " + currentLocation.ID;

            if (displayState == DisplayState.Opening)
            {   //fade overlay out
                overlay.fadeState = FadeState.FadeOut;
                Functions_ScreenRec.Fade(overlay);
                if (overlay.fadeState == FadeState.FadeComplete)
                { displayState = DisplayState.Opened; }
            }
            else if (displayState == DisplayState.Opened)
            {

                #region Map Movement Routine

                //get hero's newPosition
                Pool.hero.compMove.newPosition.X = targetLocation.compSprite.position.X;
                Pool.hero.compMove.newPosition.Y = targetLocation.compSprite.position.Y - 8;

                //get distance between hero and location
                distance = Pool.hero.compMove.newPosition - Pool.hero.compMove.position;

                //check to see if hero is close enough to snap positions
                if (Math.Abs(distance.X) < 1)
                { Pool.hero.compMove.position.X = Pool.hero.compMove.newPosition.X; }
                if (Math.Abs(distance.Y) < 1)
                { Pool.hero.compMove.position.Y = Pool.hero.compMove.newPosition.Y; }

                //move hero towards location
                if (Pool.hero.compMove.position.X != Pool.hero.compMove.newPosition.X)
                { Pool.hero.compMove.position.X += distance.X * speed; }
                if (Pool.hero.compMove.position.Y != Pool.hero.compMove.newPosition.Y)
                { Pool.hero.compMove.position.Y += distance.Y * speed; }

                //align hero's sprite to current move position
                Pool.hero.compSprite.position.X = (int)Pool.hero.compMove.position.X;
                Pool.hero.compSprite.position.Y = (int)Pool.hero.compMove.position.Y;

                //check to see if hero just arrived at a location
                if (Pool.hero.state == ActorState.Move)
                {   //if hero just reached destination.. (moving + position match)
                    if (Pool.hero.compMove.position == Pool.hero.compMove.newPosition)
                    {   //set hero's animation to idle down
                        Pool.hero.state = ActorState.Idle;
                        Pool.hero.direction = Direction.Down;
                        currentLocation = targetLocation;
                        //spawn attention particle at hero's feet
                        Functions_Particle.Spawn(ObjType.Particle_Attention,
                            Pool.hero.compSprite.position.X,
                            Pool.hero.compSprite.position.Y + 6);
                        //play arrival sound fx
                        Assets.Play(Assets.sfxMapLocation);

                        if (Flags.PrintOutput)
                        { Debug.WriteLine("location: " + currentLocation.ID); }
                    }
                }

                #endregion


                #region Wave Generation Routine

                if (Functions_Random.Int(0, 100) > 80)
                {   //randomly create a wave particle at a wave spawn location with random offset
                    wavePos = waveSpawnPositions[Functions_Random.Int(0, waveSpawnPositions.Count)];
                    Functions_Particle.Spawn(ObjType.Particle_Map_Wave,
                        wavePos.X + Functions_Random.Int(-12, 12),
                        wavePos.Y + Functions_Random.Int(-12, 12));
                }

                #endregion


                #region Update particle list - particle animate and move (waves)

                for (i = 0; i < Pool.particleCount; i++)
                {
                    if (Pool.particlePool[i].active)
                    {
                        Functions_GameObject.Update(Pool.particlePool[i]);
                        Functions_Animation.Animate(Pool.particlePool[i].compAnim, Pool.particlePool[i].compSprite);
                        Functions_Animation.ScaleSpriteDown(Pool.particlePool[i].compSprite);
                        //any particle that moves needs to have their position set, then aligned
                        Pool.particlePool[i].compMove.position.X = Pool.particlePool[i].compMove.newPosition.X;
                        Pool.particlePool[i].compMove.position.Y = Pool.particlePool[i].compMove.newPosition.Y;
                        Functions_Component.Align(Pool.particlePool[i]);
                        //we also need to be counting the life of the particles
                        Functions_Particle.Update(Pool.particlePool[i]);
                    }
                }

                #endregion

            }
            else if (displayState == DisplayState.Closing)
            {   //fade overlay in
                overlay.fadeState = FadeState.FadeIn;
                Functions_ScreenRec.Fade(overlay);
                if (overlay.fadeState == FadeState.FadeComplete)
                { displayState = DisplayState.Closed; }
            }
            else if (displayState == DisplayState.Closed)
            {
                //set the level id based on the current location, load into field
                LevelSet.field.ID = currentLocation.ID;
                LevelSet.currentLevel = LevelSet.field;
                //load the level, building the room(s)
                ScreenManager.ExitAndLoad(Screens.Level);
            }
        }

        public override void Draw(GameTime GameTime)
        {

            #region Draw from Camera View

            ScreenManager.spriteBatch.Begin(
                        SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend,
                        SamplerState.PointClamp,
                        null, null, null, Camera2D.view);

            Functions_Draw.Draw(map);
            for (i = 0; i < locations.Count; i++) { Functions_Draw.Draw(locations[i].compSprite); }
            Functions_Pool.Draw();
            Functions_Draw.Draw(Pool.hero.compSprite);

            ScreenManager.spriteBatch.End();

            #endregion


            #region Draw UI

            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            Functions_Draw.Draw(overlay);

            ScreenManager.spriteBatch.End();

            #endregion

        }

    }


    //there is only one overworld implemented right now
    public class Overworld_ShadowKing : ScreenOverworld
    {

        public Overworld_ShadowKing()
        {
            this.name = "shadowking";


            #region Create locations

            locations = new List<MapLocation>();

            //skull island
            MapLocation shadowKing = new MapLocation(true, new Vector2(1038, 487));
            shadowKing.ID = LevelID.Colliseum;
            locations.Add(shadowKing);

            MapLocation shadowKing_south = new MapLocation(false, new Vector2(1043, 536));
            locations.Add(shadowKing_south);

            #endregion
            


            #region Setup location neighbors

            shadowKing.neighborDown = shadowKing_south;
            shadowKing_south.neighborUp = shadowKing;

            #endregion



            #region Setup location levels

            //set currentlocation based on last loaded level
            if (LevelSet.field.ID == LevelID.Colliseum)
            { currentLocation = shadowKing; }

            //default to shadowking if last level is unknown
            else { currentLocation = shadowKing; }

            //set target to current (no initial target)
            targetLocation = currentLocation;

            #endregion



            #region Setup location zDepths

            for(i = 0; i < locations.Count; i++)
            {
                locations[i].compSprite.zOffset = -32;
                Functions_Component.SetZdepth(locations[i].compSprite);
            }

            #endregion



            #region Setup wave spawn positions

            //create a list of positions where to place waves
            waveSpawnPositions = new List<Vector2>();
            waveSpawnPositions.Add(new Vector2(1087, 483));

            #endregion
            

        }

        public override void Open()
        {
            //setup map texture
            map = new ComponentSprite(
                 Assets.overworld_image,
                 new Vector2(2000/2, 900/2),
                 new Byte4(0, 0, 0, 0),
                 new Point(2000, 900));

            Functions_Pool.Reset();

            //teleport hero to current location
            Functions_Movement.Teleport(Pool.hero.compMove,
                currentLocation.compSprite.position.X,
                currentLocation.compSprite.position.Y - 8);
            Functions_Component.Align(Pool.hero);
            //prevents drown sprite from appearing, if hero died in water
            Pool.hero.underwater = false;
            Pool.hero.swimming = false;
            Pool.hero.feetFX.visible = false;
            //prevent kick drum from playing during overworld map
            if (Pool.hero.health < 3) { Pool.hero.health = 3; }
            Functions_Actor.Update(Pool.hero);


            base.Open();

       
            /*
            //spawn map campfires
            Functions_Particle.Spawn(ObjType.Particle_Map_Campfire, 505, 257); //tent town
            Functions_Particle.Spawn(ObjType.Particle_Map_Campfire, 299, 297); //center island

            //spawn castle flags
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 451 + 8, 97 + 6);
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 468 + 8, 106 + 6);
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 485 + 8, 98 + 6);
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 464 + 8, 82 + 6);
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 474 + 8, 79 + 6);
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 465 + 8, 71 + 6);

            //spawn additional flags
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 334 + 8, 97 + 6); //old town
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 320 + 8, 113 + 6); //old town
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 357 + 8, 99 + 6); //old town
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 305 + 8, 147 + 6); //colliseum
            */

            Functions_Music.PlayMusic(Music.Title); //play overworld music
            Assets.colorScheme.background = Assets.colorScheme.bkg_lightWorld;
        }

    }
    

}