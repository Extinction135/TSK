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
                            //release all the lines being used
                            Functions_Pool.ResetLinePool();
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
            {   //print the position of the cursor to aid in placing locations
                if (Functions_Input.IsNewMouseButtonPress(MouseButtons.LeftButton))
                {
                    if(Flags.PrintOutput)
                    {
                        Debug.WriteLine("map pos:" + 
                            Functions_Camera2D.ConvertScreenToWorld(Input.cursorPos.X, Input.cursorPos.Y) 
                            + "");
                    }
                }
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

                        //gimme some feedback
                        if (Flags.PrintOutput)
                        { Debug.WriteLine("location name: " + currentLocation.name + ", ID:" + currentLocation.ID); }

                        //setup the new location's connection lines
                        SetupLinesToNeighbors();
                    }
                }

                #endregion


                #region Wave Generation Routine

                if (Functions_Random.Int(0, 100) > 94) //5% chance to spawn wave
                {   //randomly create a wave particle at a wave spawn location with random offset
                    wavePos = waveSpawnPositions[Functions_Random.Int(0, waveSpawnPositions.Count)];
                    Functions_Particle.Spawn(ObjType.Particle_Map_Wave,
                        wavePos.X + Functions_Random.Int(-6, 6),
                        wavePos.Y + Functions_Random.Int(-6, 6));
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
                //setup level id based on current location, load into field
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





        public void SetupLinesToNeighbors()
        {
            //reset all the lines prior to setting up new ones
            Functions_Pool.ResetLinePool();

            //now lets add all the neighbors as lines
            Functions_Line.AddLine( //neighbor right
                (int)currentLocation.compSprite.position.X,
                (int)currentLocation.compSprite.position.Y,
                (int)currentLocation.neighborRight.compSprite.position.X,
                (int)currentLocation.neighborRight.compSprite.position.Y
            );
            Functions_Line.AddLine( //neighbor left
                (int)currentLocation.compSprite.position.X,
                (int)currentLocation.compSprite.position.Y,
                (int)currentLocation.neighborLeft.compSprite.position.X,
                (int)currentLocation.neighborLeft.compSprite.position.Y
            );
            Functions_Line.AddLine( //neighbor up
                (int)currentLocation.compSprite.position.X,
                (int)currentLocation.compSprite.position.Y,
                (int)currentLocation.neighborUp.compSprite.position.X,
                (int)currentLocation.neighborUp.compSprite.position.Y
            );
            Functions_Line.AddLine( //neighbor down
                (int)currentLocation.compSprite.position.X,
                (int)currentLocation.compSprite.position.Y,
                (int)currentLocation.neighborDown.compSprite.position.X,
                (int)currentLocation.neighborDown.compSprite.position.Y
            );

            //prep lines for display
            for (i = 0; i < Pool.lineCount; i++)
            {
                //sets up angles, length, etc..
                Functions_Line.UpdateLine(Pool.linePool[i]);
                //force lines to layer under hero, over map
                Pool.linePool[i].zDepth = 0.9999f;
            }
            
        }



    }

    
    //there is only one overworld implemented right now
    public class Overworld_ShadowKing : ScreenOverworld
    {

        
        //skullisland
        static MapLocation SkullIsland_ShadowDungeon;
        static MapLocation SkullIsland_Coliseum;
        static MapLocation SkullIsland_Town;

        //death mtn
        static MapLocation DeathMountain_MainEntrance;

        //forest island
        static MapLocation ForestIsland_MainEntrance;

        //lava island
        static MapLocation LavaIsland_MainEntrance;

        //cloud island
        static MapLocation CloudIsland_MainEntrance;

        //swamp island
        static MapLocation SwampIsland_MainEntrance_OLD;

        //thieves den island
        static MapLocation ThievesDen_GateEntrance;

        //misc
        static MapLocation Sea_GiantHole;




        public Overworld_ShadowKing()
        {
            this.name = "shadowking";

            //setup map texture
            map = new ComponentSprite(
                 Assets.overworld_image,
                 new Vector2(2000 / 2, 900 / 2),
                 new Byte4(0, 0, 0, 0),
                 new Point(2000, 900));
            map.zOffset = -450; //set many layers behind link & locations
            Functions_Component.SetZdepth(map);





            //create locations master list
            locations = new List<MapLocation>();
            //create island locations + levels



            #region Misc locations

            //bottom right of map, giant hole in the sea
            Sea_GiantHole = new MapLocation(false, new Vector2(1786, 766), "Sea_GiantHole");
            Sea_GiantHole.ID = LevelID.SkullIsland_ShadowKing;
            locations.Add(Sea_GiantHole);

            #endregion





            #region Skull Island

            SkullIsland_ShadowDungeon = new MapLocation(true, new Vector2(1038, 487), "SkullIsland_ShadowDungeon");
            SkullIsland_ShadowDungeon.ID = LevelID.SkullIsland_ShadowKing;
            locations.Add(SkullIsland_ShadowDungeon);

            //bottom rounded bowl shaped row of locations
            MapLocation SkullIsland_BottomRow_1 = new MapLocation(false, new Vector2(794, 476), "SkullIsland_BottomRow_1");
            locations.Add(SkullIsland_BottomRow_1);
            MapLocation SkullIsland_BottomRow_2 = new MapLocation(false, new Vector2(829, 501), "SkullIsland_BottomRow_2");
            locations.Add(SkullIsland_BottomRow_2);
            MapLocation SkullIsland_BottomRow_3 = new MapLocation(false, new Vector2(890, 521), "SkullIsland_BottomRow_3");
            locations.Add(SkullIsland_BottomRow_3);
            MapLocation SkullIsland_BottomRow_4 = new MapLocation(false, new Vector2(965, 534), "SkullIsland_BottomRow_4");
            locations.Add(SkullIsland_BottomRow_4);
            MapLocation SkullIsland_BottomRow_5 = new MapLocation(false, new Vector2(1043, 533), "SkullIsland_BottomRow_5");
            locations.Add(SkullIsland_BottomRow_5);

            MapLocation SkullIsland_BottomRow_6 = new MapLocation(false, new Vector2(1115, 527), "SkullIsland_BottomRow_6");
            locations.Add(SkullIsland_BottomRow_6);
            MapLocation SkullIsland_BottomRow_7 = new MapLocation(false, new Vector2(1163, 508), "SkullIsland_BottomRow_7");
            locations.Add(SkullIsland_BottomRow_7);
            MapLocation SkullIsland_BottomRow_8 = new MapLocation(false, new Vector2(1216, 471), "SkullIsland_BottomRow_8");
            locations.Add(SkullIsland_BottomRow_8);

            //top crown shaped row of location
            MapLocation SkullIsland_TopCrown_1 = new MapLocation(false, new Vector2(791, 426), "SkullIsland_TopCrown_1");
            locations.Add(SkullIsland_TopCrown_1);
            MapLocation SkullIsland_TopCrown_2 = new MapLocation(false, new Vector2(822, 377), "SkullIsland_TopCrown_2");
            locations.Add(SkullIsland_TopCrown_2);
            MapLocation SkullIsland_TopCrown_3 = new MapLocation(false, new Vector2(896, 345), "SkullIsland_TopCrown_3");
            locations.Add(SkullIsland_TopCrown_3);
            MapLocation SkullIsland_TopCrown_4 = new MapLocation(false, new Vector2(888, 282), "SkullIsland_TopCrown_4");
            locations.Add(SkullIsland_TopCrown_4);
            MapLocation SkullIsland_TopCrown_5 = new MapLocation(false, new Vector2(955, 277), "SkullIsland_TopCrown_5");
            locations.Add(SkullIsland_TopCrown_5);

            MapLocation SkullIsland_TopCrown_6 = new MapLocation(false, new Vector2(1008, 237), "SkullIsland_TopCrown_6");
            locations.Add(SkullIsland_TopCrown_6);
            MapLocation SkullIsland_TopCrown_7 = new MapLocation(false, new Vector2(1060, 279), "SkullIsland_TopCrown_7");
            locations.Add(SkullIsland_TopCrown_7);
            MapLocation SkullIsland_TopCrown_8 = new MapLocation(false, new Vector2(1131, 286), "SkullIsland_TopCrown_8");
            locations.Add(SkullIsland_TopCrown_8);
            MapLocation SkullIsland_TopCrown_9 = new MapLocation(false, new Vector2(1112, 347), "SkullIsland_TopCrown_9");
            locations.Add(SkullIsland_TopCrown_9);
            MapLocation SkullIsland_TopCrown_10 = new MapLocation(false, new Vector2(1187, 377), "SkullIsland_TopCrown_10");
            locations.Add(SkullIsland_TopCrown_10);

            MapLocation SkullIsland_TopCrown_11 = new MapLocation(false, new Vector2(1224, 421), "SkullIsland_TopCrown_11");
            locations.Add(SkullIsland_TopCrown_11);

            //path to coliseum
            SkullIsland_Coliseum = new MapLocation(true, new Vector2(858, 445), "SkullIsland_Coliseum");
            SkullIsland_Coliseum.ID = LevelID.SkullIsland_Colliseum;
            locations.Add(SkullIsland_Coliseum);
            MapLocation SkullIsland_Coliseum_South_1 = new MapLocation(false, new Vector2(869, 474), "SkullIsland_Coliseum_South_1");
            locations.Add(SkullIsland_Coliseum_South_1);
            MapLocation SkullIsland_Coliseum_South_2 = new MapLocation(false, new Vector2(926, 499), "SkullIsland_Coliseum_South_2");
            locations.Add(SkullIsland_Coliseum_South_2);

            //path to town
            SkullIsland_Town = new MapLocation(true, new Vector2(1158, 436), "SkullIsland_Town");
            SkullIsland_Town.ID = LevelID.SkullIsland_Town;
            locations.Add(SkullIsland_Town);
            MapLocation SkullIsland_Town_South_1 = new MapLocation(false, new Vector2(1162, 469), "SkullIsland_Town_South_1");
            locations.Add(SkullIsland_Town_South_1);
            MapLocation SkullIsland_Town_South_2 = new MapLocation(false, new Vector2(1131, 489), "SkullIsland_Town_South_2");
            locations.Add(SkullIsland_Town_South_2);




            //Setup location neighbors

            #region Level path connections to bottom row

            //shadow king
            SkullIsland_ShadowDungeon.neighborDown = SkullIsland_BottomRow_5;
            SkullIsland_BottomRow_5.neighborUp = SkullIsland_ShadowDungeon;

            //town path (up)
            SkullIsland_BottomRow_7.neighborUp = SkullIsland_Town_South_2;
            SkullIsland_Town_South_2.neighborUp = SkullIsland_Town_South_1;
            SkullIsland_Town_South_1.neighborUp = SkullIsland_Town;
            //(down)
            SkullIsland_Town.neighborDown = SkullIsland_Town_South_1;
            SkullIsland_Town_South_1.neighborDown = SkullIsland_Town_South_2;
            SkullIsland_Town_South_2.neighborDown = SkullIsland_BottomRow_7;
            //(polish)
            SkullIsland_Town_South_2.neighborRight = SkullIsland_Town_South_1;

            //coliseum path (up)
            SkullIsland_BottomRow_3.neighborUp = SkullIsland_Coliseum_South_2;
            SkullIsland_Coliseum_South_2.neighborUp = SkullIsland_Coliseum_South_1;
            SkullIsland_Coliseum_South_1.neighborUp = SkullIsland_Coliseum;
            //(down)
            SkullIsland_Coliseum.neighborDown = SkullIsland_Coliseum_South_1;
            SkullIsland_Coliseum_South_1.neighborDown = SkullIsland_Coliseum_South_2;
            SkullIsland_Coliseum_South_2.neighborDown = SkullIsland_BottomRow_3;
            //(polish)
            SkullIsland_Coliseum_South_2.neighborLeft = SkullIsland_Coliseum_South_1;
            SkullIsland_Coliseum_South_1.neighborLeft = SkullIsland_Coliseum;
            SkullIsland_Coliseum.neighborRight = SkullIsland_Coliseum_South_1;
            SkullIsland_Coliseum_South_1.neighborRight = SkullIsland_Coliseum_South_2;

            #endregion


            #region Bottom row connections 

            //right
            SkullIsland_BottomRow_1.neighborRight = SkullIsland_BottomRow_2;
            SkullIsland_BottomRow_2.neighborRight = SkullIsland_BottomRow_3;
            SkullIsland_BottomRow_3.neighborRight = SkullIsland_BottomRow_4;
            SkullIsland_BottomRow_4.neighborRight = SkullIsland_BottomRow_5;
            SkullIsland_BottomRow_5.neighborRight = SkullIsland_BottomRow_6;
            SkullIsland_BottomRow_6.neighborRight = SkullIsland_BottomRow_7;
            SkullIsland_BottomRow_7.neighborRight = SkullIsland_BottomRow_8;
            //left
            SkullIsland_BottomRow_8.neighborLeft = SkullIsland_BottomRow_7;
            SkullIsland_BottomRow_7.neighborLeft = SkullIsland_BottomRow_6;
            SkullIsland_BottomRow_6.neighborLeft = SkullIsland_BottomRow_5;
            SkullIsland_BottomRow_5.neighborLeft = SkullIsland_BottomRow_4;
            SkullIsland_BottomRow_4.neighborLeft = SkullIsland_BottomRow_3;
            SkullIsland_BottomRow_3.neighborLeft = SkullIsland_BottomRow_2;
            SkullIsland_BottomRow_2.neighborLeft = SkullIsland_BottomRow_1;

            #endregion


            #region Bottom row connections to crown

            SkullIsland_TopCrown_1.neighborDown = SkullIsland_BottomRow_1;
            SkullIsland_BottomRow_1.neighborUp = SkullIsland_TopCrown_1;

            SkullIsland_TopCrown_11.neighborDown = SkullIsland_BottomRow_8;
            SkullIsland_BottomRow_8.neighborUp = SkullIsland_TopCrown_11;

            #endregion


            #region Top crown connections - tricky!

            SkullIsland_TopCrown_1.neighborUp = SkullIsland_TopCrown_2;
            SkullIsland_TopCrown_1.neighborRight = SkullIsland_TopCrown_2;
            SkullIsland_TopCrown_2.neighborDown = SkullIsland_TopCrown_1;

            SkullIsland_TopCrown_2.neighborRight = SkullIsland_TopCrown_3;
            SkullIsland_TopCrown_3.neighborLeft = SkullIsland_TopCrown_2;

            SkullIsland_TopCrown_3.neighborUp = SkullIsland_TopCrown_4;
            SkullIsland_TopCrown_4.neighborDown = SkullIsland_TopCrown_3;

            SkullIsland_TopCrown_4.neighborRight = SkullIsland_TopCrown_5;
            SkullIsland_TopCrown_5.neighborLeft = SkullIsland_TopCrown_4;

            //crown top = 6
            SkullIsland_TopCrown_5.neighborUp = SkullIsland_TopCrown_6;
            SkullIsland_TopCrown_5.neighborRight = SkullIsland_TopCrown_6;
            SkullIsland_TopCrown_6.neighborLeft = SkullIsland_TopCrown_5;
            SkullIsland_TopCrown_6.neighborRight = SkullIsland_TopCrown_7;
            SkullIsland_TopCrown_7.neighborLeft = SkullIsland_TopCrown_6;
            SkullIsland_TopCrown_7.neighborUp = SkullIsland_TopCrown_6;

            //
            SkullIsland_TopCrown_7.neighborRight = SkullIsland_TopCrown_8;
            SkullIsland_TopCrown_8.neighborLeft = SkullIsland_TopCrown_7;

            SkullIsland_TopCrown_8.neighborDown = SkullIsland_TopCrown_9;
            SkullIsland_TopCrown_9.neighborUp = SkullIsland_TopCrown_8;

            SkullIsland_TopCrown_9.neighborRight = SkullIsland_TopCrown_10;
            SkullIsland_TopCrown_9.neighborDown = SkullIsland_TopCrown_10;
            SkullIsland_TopCrown_10.neighborLeft = SkullIsland_TopCrown_9;

            SkullIsland_TopCrown_10.neighborDown = SkullIsland_TopCrown_11;
            SkullIsland_TopCrown_11.neighborUp = SkullIsland_TopCrown_11.neighborLeft = SkullIsland_TopCrown_10;

            #endregion


            #endregion


            #region Death Mountain

            DeathMountain_MainEntrance = new MapLocation(true, new Vector2(1676, 503), "DeathMountain_MainEntrance");
            DeathMountain_MainEntrance.ID = LevelID.DeathMountain_MainEntrance;
            locations.Add(DeathMountain_MainEntrance);

            MapLocation DeathMountain_EntConnection = new MapLocation(false, new Vector2(1620, 503), "DeathMountain_EntConnection");
            locations.Add(DeathMountain_EntConnection);

            //main center path connected to skull island triway
            MapLocation DeathMountain_MainPath_1 = new MapLocation(false, new Vector2(1280, 450), "DeathMountain_MainPath_1");
            locations.Add(DeathMountain_MainPath_1);
            MapLocation DeathMountain_MainPath_2 = new MapLocation(false, new Vector2(1342, 475), "DeathMountain_MainPath_2");
            locations.Add(DeathMountain_MainPath_2);
            MapLocation DeathMountain_MainPath_3 = new MapLocation(false, new Vector2(1408, 472), "DeathMountain_MainPath_3");
            locations.Add(DeathMountain_MainPath_3);
            MapLocation DeathMountain_MainPath_4 = new MapLocation(false, new Vector2(1475, 484), "DeathMountain_MainPath_4");
            locations.Add(DeathMountain_MainPath_4);
            MapLocation DeathMountain_MainPath_5 = new MapLocation(false, new Vector2(1519, 517), "DeathMountain_MainPath_5");
            locations.Add(DeathMountain_MainPath_5);

            MapLocation DeathMountain_MainPath_6 = new MapLocation(false, new Vector2(1582, 528), "DeathMountain_MainPath_6");
            locations.Add(DeathMountain_MainPath_6);
            MapLocation DeathMountain_MainPath_7 = new MapLocation(false, new Vector2(1630, 549), "DeathMountain_MainPath_7");
            locations.Add(DeathMountain_MainPath_7);
            MapLocation DeathMountain_MainPath_8 = new MapLocation(false, new Vector2(1685, 542), "DeathMountain_MainPath_8");
            locations.Add(DeathMountain_MainPath_8);
            MapLocation DeathMountain_MainPath_9 = new MapLocation(false, new Vector2(1727, 532), "DeathMountain_MainPath_9");
            locations.Add(DeathMountain_MainPath_9);
            MapLocation DeathMountain_MainPath_10 = new MapLocation(false, new Vector2(1772, 544), "DeathMountain_MainPath_10");
            locations.Add(DeathMountain_MainPath_10);

            MapLocation DeathMountain_MainPath_11 = new MapLocation(false, new Vector2(1816, 520), "DeathMountain_MainPath_11");
            locations.Add(DeathMountain_MainPath_11);
            MapLocation DeathMountain_MainPath_12 = new MapLocation(false, new Vector2(1801, 477), "DeathMountain_MainPath_12");
            locations.Add(DeathMountain_MainPath_12);
            MapLocation DeathMountain_MainPath_13 = new MapLocation(false, new Vector2(1846, 451), "DeathMountain_MainPath_13");
            locations.Add(DeathMountain_MainPath_13);
            MapLocation DeathMountain_MainPath_14 = new MapLocation(false, new Vector2(1905, 454), "DeathMountain_MainPath_14");
            locations.Add(DeathMountain_MainPath_14);
            MapLocation DeathMountain_MainPath_15 = new MapLocation(false, new Vector2(1936, 489), "DeathMountain_MainPath_15");
            locations.Add(DeathMountain_MainPath_15);

            MapLocation DeathMountain_MainPath_16 = new MapLocation(false, new Vector2(1920, 522), "DeathMountain_MainPath_16");
            locations.Add(DeathMountain_MainPath_16);
            MapLocation DeathMountain_MainPath_17 = new MapLocation(false, new Vector2(1945, 554), "DeathMountain_MainPath_17");
            locations.Add(DeathMountain_MainPath_17);
            MapLocation DeathMountain_MainPath_18 = new MapLocation(false, new Vector2(1905, 568), "DeathMountain_MainPath_18");
            locations.Add(DeathMountain_MainPath_18);
            MapLocation DeathMountain_MainPath_19 = new MapLocation(false, new Vector2(1866, 552), "DeathMountain_MainPath_19");
            locations.Add(DeathMountain_MainPath_19);





            //Setup location neighbors

            #region Connection to Skull Island

            SkullIsland_BottomRow_8.neighborRight = DeathMountain_MainPath_1;
            SkullIsland_TopCrown_11.neighborRight = DeathMountain_MainPath_1;
            DeathMountain_MainPath_1.neighborLeft = SkullIsland_BottomRow_8;
            DeathMountain_MainPath_1.neighborUp = SkullIsland_TopCrown_11;

            #endregion


            #region Main Path L/R

            //path right
            DeathMountain_MainPath_1.neighborRight = DeathMountain_MainPath_2;
            DeathMountain_MainPath_2.neighborRight = DeathMountain_MainPath_3;
            DeathMountain_MainPath_3.neighborRight = DeathMountain_MainPath_4;
            DeathMountain_MainPath_4.neighborRight = DeathMountain_MainPath_5;

            DeathMountain_MainPath_5.neighborRight = DeathMountain_MainPath_6;
            DeathMountain_MainPath_6.neighborRight = DeathMountain_MainPath_7;
            DeathMountain_MainPath_6.neighborDown = DeathMountain_MainPath_7;
            DeathMountain_MainPath_7.neighborRight = DeathMountain_MainPath_8;
            DeathMountain_MainPath_8.neighborRight = DeathMountain_MainPath_9;
            DeathMountain_MainPath_9.neighborRight = DeathMountain_MainPath_10;

            DeathMountain_MainPath_10.neighborRight = DeathMountain_MainPath_11;
            DeathMountain_MainPath_11.neighborUp = DeathMountain_MainPath_12;
            DeathMountain_MainPath_12.neighborUp = DeathMountain_MainPath_13;
            DeathMountain_MainPath_12.neighborRight = DeathMountain_MainPath_13;
            DeathMountain_MainPath_13.neighborRight = DeathMountain_MainPath_14;
            DeathMountain_MainPath_14.neighborRight = DeathMountain_MainPath_15;
            DeathMountain_MainPath_14.neighborDown = DeathMountain_MainPath_15;
            DeathMountain_MainPath_15.neighborDown = DeathMountain_MainPath_16;

            DeathMountain_MainPath_16.neighborDown = DeathMountain_MainPath_17;
            DeathMountain_MainPath_17.neighborLeft = DeathMountain_MainPath_18;
            DeathMountain_MainPath_18.neighborLeft = DeathMountain_MainPath_19;


            //path left
            DeathMountain_MainPath_2.neighborLeft = DeathMountain_MainPath_1;
            DeathMountain_MainPath_3.neighborLeft = DeathMountain_MainPath_2;
            DeathMountain_MainPath_4.neighborLeft = DeathMountain_MainPath_3;
            DeathMountain_MainPath_5.neighborLeft = DeathMountain_MainPath_4;

            DeathMountain_MainPath_6.neighborLeft = DeathMountain_MainPath_5;
            DeathMountain_MainPath_7.neighborLeft = DeathMountain_MainPath_6;
            DeathMountain_MainPath_8.neighborLeft = DeathMountain_MainPath_7;
            DeathMountain_MainPath_9.neighborLeft = DeathMountain_MainPath_8;
            DeathMountain_MainPath_10.neighborLeft = DeathMountain_MainPath_9;

            DeathMountain_MainPath_11.neighborLeft = DeathMountain_MainPath_10;
            DeathMountain_MainPath_11.neighborDown = DeathMountain_MainPath_10;
            DeathMountain_MainPath_12.neighborDown = DeathMountain_MainPath_11;
            DeathMountain_MainPath_13.neighborDown = DeathMountain_MainPath_12;
            DeathMountain_MainPath_13.neighborLeft = DeathMountain_MainPath_12;
            DeathMountain_MainPath_14.neighborLeft = DeathMountain_MainPath_13;
            DeathMountain_MainPath_15.neighborUp = DeathMountain_MainPath_14;
            DeathMountain_MainPath_15.neighborLeft = DeathMountain_MainPath_14;

            DeathMountain_MainPath_16.neighborRight = DeathMountain_MainPath_15;
            DeathMountain_MainPath_16.neighborUp = DeathMountain_MainPath_15;
            DeathMountain_MainPath_17.neighborUp = DeathMountain_MainPath_16;
            DeathMountain_MainPath_18.neighborRight = DeathMountain_MainPath_17;
            DeathMountain_MainPath_19.neighborRight = DeathMountain_MainPath_18;

            #endregion


            #region Entrance Connection Path

            DeathMountain_MainPath_6.neighborUp = DeathMountain_EntConnection;
            DeathMountain_EntConnection.neighborLeft = DeathMountain_MainPath_6;
            DeathMountain_EntConnection.neighborDown = DeathMountain_MainPath_6;

            DeathMountain_EntConnection.neighborRight = DeathMountain_MainEntrance;
            DeathMountain_MainEntrance.neighborLeft = DeathMountain_EntConnection;
            DeathMountain_MainEntrance.neighborDown = DeathMountain_MainPath_8;

            DeathMountain_MainPath_8.neighborUp = DeathMountain_MainEntrance;

            #endregion



            #endregion


            #region Forest Island

            ForestIsland_MainEntrance = new MapLocation(true, new Vector2(554, 831), "ForestIsland_MainEntrance");
            ForestIsland_MainEntrance.ID = LevelID.ForestIsland_MainEntrance;
            locations.Add(ForestIsland_MainEntrance);

            //main path from skull island to dungeon entrance, around to left temple too
            MapLocation ForestIsland_MainPath_1 = new MapLocation(false, new Vector2(849, 550), "ForestIsland_MainPath_1");
            locations.Add(ForestIsland_MainPath_1);
            MapLocation ForestIsland_MainPath_2 = new MapLocation(false, new Vector2(876, 596), "ForestIsland_MainPath_2");
            locations.Add(ForestIsland_MainPath_2);
            MapLocation ForestIsland_MainPath_3 = new MapLocation(false, new Vector2(907, 620), "ForestIsland_MainPath_3");
            locations.Add(ForestIsland_MainPath_3);
            MapLocation ForestIsland_MainPath_4 = new MapLocation(false, new Vector2(877, 650), "ForestIsland_MainPath_4");
            locations.Add(ForestIsland_MainPath_4);
            MapLocation ForestIsland_MainPath_5 = new MapLocation(false, new Vector2(825, 670), "ForestIsland_MainPath_5");
            locations.Add(ForestIsland_MainPath_5);

            MapLocation ForestIsland_MainPath_6 = new MapLocation(false, new Vector2(809, 703), "ForestIsland_MainPath_6");
            locations.Add(ForestIsland_MainPath_6);
            MapLocation ForestIsland_MainPath_7 = new MapLocation(false, new Vector2(824, 731), "ForestIsland_MainPath_7");
            locations.Add(ForestIsland_MainPath_7);
            MapLocation ForestIsland_MainPath_8 = new MapLocation(false, new Vector2(885, 746), "ForestIsland_MainPath_8");
            locations.Add(ForestIsland_MainPath_8);
            MapLocation ForestIsland_MainPath_9 = new MapLocation(false, new Vector2(897, 795), "ForestIsland_MainPath_9");
            locations.Add(ForestIsland_MainPath_9);
            MapLocation ForestIsland_MainPath_10 = new MapLocation(false, new Vector2(850, 817), "ForestIsland_MainPath_10");
            locations.Add(ForestIsland_MainPath_10);

            MapLocation ForestIsland_MainPath_11 = new MapLocation(false, new Vector2(797, 834), "ForestIsland_MainPath_11");
            locations.Add(ForestIsland_MainPath_11);
            MapLocation ForestIsland_MainPath_12 = new MapLocation(false, new Vector2(748, 824), "ForestIsland_MainPath_12");
            locations.Add(ForestIsland_MainPath_12);
            MapLocation ForestIsland_MainPath_13 = new MapLocation(false, new Vector2(715, 798), "ForestIsland_MainPath_13");
            locations.Add(ForestIsland_MainPath_13);
            MapLocation ForestIsland_MainPath_14 = new MapLocation(false, new Vector2(673, 808), "ForestIsland_MainPath_14");
            locations.Add(ForestIsland_MainPath_14);
            MapLocation ForestIsland_MainPath_15 = new MapLocation(false, new Vector2(666, 832), "ForestIsland_MainPath_15");
            locations.Add(ForestIsland_MainPath_15);

            MapLocation ForestIsland_MainPath_16 = new MapLocation(false, new Vector2(611, 842), "ForestIsland_MainPath_16");
            locations.Add(ForestIsland_MainPath_16);
            //17 is the skull/dungeon entrance
            //MapLocation ForestIsland_MainPath_17 = new MapLocation(false, new Vector2(554, 831), "ForestIsland_MainPath_17");
            //locations.Add(ForestIsland_MainPath_17);
            MapLocation ForestIsland_MainPath_18 = new MapLocation(false, new Vector2(521, 816), "ForestIsland_MainPath_18");
            locations.Add(ForestIsland_MainPath_18);
            MapLocation ForestIsland_MainPath_19 = new MapLocation(false, new Vector2(491, 793), "ForestIsland_MainPath_19");
            locations.Add(ForestIsland_MainPath_19);
            MapLocation ForestIsland_MainPath_20 = new MapLocation(false, new Vector2(456, 780), "ForestIsland_MainPath_20");
            locations.Add(ForestIsland_MainPath_20);

            MapLocation ForestIsland_MainPath_21 = new MapLocation(false, new Vector2(411, 779), "ForestIsland_MainPath_21");
            locations.Add(ForestIsland_MainPath_21);
            MapLocation ForestIsland_MainPath_22 = new MapLocation(false, new Vector2(385, 722), "ForestIsland_MainPath_22");
            locations.Add(ForestIsland_MainPath_22);
            MapLocation ForestIsland_MainPath_23 = new MapLocation(false, new Vector2(403, 674), "ForestIsland_MainPath_23");
            locations.Add(ForestIsland_MainPath_23);
            MapLocation ForestIsland_MainPath_24 = new MapLocation(false, new Vector2(361, 658), "ForestIsland_MainPath_24");
            locations.Add(ForestIsland_MainPath_24);
            MapLocation ForestIsland_MainPath_25 = new MapLocation(false, new Vector2(374, 622), "ForestIsland_MainPath_25");
            locations.Add(ForestIsland_MainPath_25);

            MapLocation ForestIsland_MainPath_26 = new MapLocation(false, new Vector2(324, 592), "ForestIsland_MainPath_26");
            locations.Add(ForestIsland_MainPath_26);
            MapLocation ForestIsland_MainPath_27 = new MapLocation(false, new Vector2(279, 553), "ForestIsland_MainPath_27");
            locations.Add(ForestIsland_MainPath_27); 


            //setup location neighbors

            #region Connection to Skull Island

            SkullIsland_BottomRow_3.neighborDown = ForestIsland_MainPath_1;
            ForestIsland_MainPath_1.neighborUp = SkullIsland_BottomRow_3;

            #endregion


            #region Main Path Connections

            //(down)
            ForestIsland_MainPath_1.neighborDown = ForestIsland_MainPath_2;
            ForestIsland_MainPath_2.neighborDown = ForestIsland_MainPath_3;
            ForestIsland_MainPath_3.neighborDown = ForestIsland_MainPath_4;
            ForestIsland_MainPath_4.neighborLeft = ForestIsland_MainPath_5;

            ForestIsland_MainPath_5.neighborDown = ForestIsland_MainPath_6;
            ForestIsland_MainPath_6.neighborDown = ForestIsland_MainPath_7;
            ForestIsland_MainPath_7.neighborRight = ForestIsland_MainPath_8;
            ForestIsland_MainPath_8.neighborDown = ForestIsland_MainPath_9;
            ForestIsland_MainPath_9.neighborLeft = ForestIsland_MainPath_10;

            ForestIsland_MainPath_10.neighborLeft = ForestIsland_MainPath_11;
            ForestIsland_MainPath_11.neighborLeft = ForestIsland_MainPath_12;
            ForestIsland_MainPath_12.neighborLeft = ForestIsland_MainPath_13;
            ForestIsland_MainPath_13.neighborLeft = ForestIsland_MainPath_14;
            ForestIsland_MainPath_14.neighborLeft = ForestIsland_MainPath_14.neighborDown = ForestIsland_MainPath_15;

            ForestIsland_MainPath_15.neighborLeft = ForestIsland_MainPath_16;
            ForestIsland_MainPath_16.neighborLeft = ForestIsland_MainEntrance;
            ForestIsland_MainEntrance.neighborLeft = ForestIsland_MainPath_18;
            ForestIsland_MainPath_18.neighborLeft = ForestIsland_MainPath_19;
            ForestIsland_MainPath_19.neighborLeft = ForestIsland_MainPath_20;

            ForestIsland_MainPath_20.neighborLeft = ForestIsland_MainPath_21;
            ForestIsland_MainPath_21.neighborUp = ForestIsland_MainPath_22;
            ForestIsland_MainPath_22.neighborUp = ForestIsland_MainPath_23;
            ForestIsland_MainPath_23.neighborUp = ForestIsland_MainPath_23.neighborLeft = ForestIsland_MainPath_24;
            ForestIsland_MainPath_24.neighborUp = ForestIsland_MainPath_25;

            ForestIsland_MainPath_25.neighborLeft = ForestIsland_MainPath_25.neighborUp = ForestIsland_MainPath_26;
            ForestIsland_MainPath_26.neighborLeft = ForestIsland_MainPath_26.neighborUp = ForestIsland_MainPath_27;

            //(up)
            ForestIsland_MainPath_2.neighborUp = ForestIsland_MainPath_1;
            ForestIsland_MainPath_3.neighborUp = ForestIsland_MainPath_2;
            ForestIsland_MainPath_4.neighborUp = ForestIsland_MainPath_3;
            ForestIsland_MainPath_5.neighborRight = ForestIsland_MainPath_4;

            ForestIsland_MainPath_6.neighborUp = ForestIsland_MainPath_5;
            ForestIsland_MainPath_7.neighborUp = ForestIsland_MainPath_6;
            ForestIsland_MainPath_8.neighborLeft = ForestIsland_MainPath_7;
            ForestIsland_MainPath_9.neighborUp = ForestIsland_MainPath_8;
            ForestIsland_MainPath_10.neighborRight = ForestIsland_MainPath_9;

            ForestIsland_MainPath_11.neighborRight = ForestIsland_MainPath_10;
            ForestIsland_MainPath_12.neighborRight = ForestIsland_MainPath_11;
            ForestIsland_MainPath_13.neighborRight = ForestIsland_MainPath_12;
            ForestIsland_MainPath_14.neighborRight = ForestIsland_MainPath_13;
            ForestIsland_MainPath_15.neighborRight = ForestIsland_MainPath_15.neighborUp = ForestIsland_MainPath_14;

            ForestIsland_MainPath_16.neighborRight = ForestIsland_MainPath_15;
            ForestIsland_MainEntrance.neighborRight = ForestIsland_MainPath_16;
            ForestIsland_MainPath_18.neighborRight = ForestIsland_MainEntrance;
            ForestIsland_MainPath_19.neighborRight = ForestIsland_MainPath_18;
            ForestIsland_MainPath_20.neighborRight = ForestIsland_MainPath_19;

            ForestIsland_MainPath_21.neighborRight = ForestIsland_MainPath_20;
            ForestIsland_MainPath_22.neighborDown = ForestIsland_MainPath_21;
            ForestIsland_MainPath_23.neighborDown = ForestIsland_MainPath_22;
            ForestIsland_MainPath_24.neighborDown = ForestIsland_MainPath_24.neighborRight = ForestIsland_MainPath_23;
            ForestIsland_MainPath_25.neighborDown = ForestIsland_MainPath_24;

            ForestIsland_MainPath_26.neighborDown = ForestIsland_MainPath_26.neighborRight = ForestIsland_MainPath_25;
            ForestIsland_MainPath_27.neighborDown = ForestIsland_MainPath_27.neighborRight = ForestIsland_MainPath_26;

            #endregion


            #region Additonal Connections



            #endregion


            #endregion


            #region Lava Island

            LavaIsland_MainEntrance = new MapLocation(true, new Vector2(372, 447), "LavaIsland_MainEntrance");
            LavaIsland_MainEntrance.ID = LevelID.LavaIsland_MainEntrance;
            locations.Add(LavaIsland_MainEntrance);

            //main path from skull island to dungeon entrance, past to castle
            MapLocation LavaIsland_MainPath_1 = new MapLocation(false, new Vector2(743, 484), "LavaIsland_MainPath_1");
            locations.Add(LavaIsland_MainPath_1);
            MapLocation LavaIsland_MainPath_2 = new MapLocation(false, new Vector2(740, 520), "LavaIsland_MainPath_2");
            locations.Add(LavaIsland_MainPath_2);
            MapLocation LavaIsland_MainPath_3 = new MapLocation(false, new Vector2(698, 548), "LavaIsland_MainPath_3");
            locations.Add(LavaIsland_MainPath_3);
            MapLocation LavaIsland_MainPath_4 = new MapLocation(false, new Vector2(637, 551), "LavaIsland_MainPath_4");
            locations.Add(LavaIsland_MainPath_4);
            MapLocation LavaIsland_MainPath_5 = new MapLocation(false, new Vector2(600, 542), "LavaIsland_MainPath_5");
            locations.Add(LavaIsland_MainPath_5);

            MapLocation LavaIsland_MainPath_6 = new MapLocation(false, new Vector2(577, 516), "LavaIsland_MainPath_6");
            locations.Add(LavaIsland_MainPath_6);
            MapLocation LavaIsland_MainPath_7 = new MapLocation(false, new Vector2(533, 514), "LavaIsland_MainPath_7");
            locations.Add(LavaIsland_MainPath_7);
            MapLocation LavaIsland_MainPath_8 = new MapLocation(false, new Vector2(521, 478), "LavaIsland_MainPath_8");
            locations.Add(LavaIsland_MainPath_8);
            MapLocation LavaIsland_MainPath_9 = new MapLocation(false, new Vector2(457, 475), "LavaIsland_MainPath_9");
            locations.Add(LavaIsland_MainPath_9);
            MapLocation LavaIsland_MainPath_10 = new MapLocation(false, new Vector2(429, 443), "LavaIsland_MainPath_10");
            locations.Add(LavaIsland_MainPath_10);

            MapLocation LavaIsland_MainPath_11 = new MapLocation(false, new Vector2(358, 481), "LavaIsland_MainPath_11");
            locations.Add(LavaIsland_MainPath_11);
            MapLocation LavaIsland_MainPath_12 = new MapLocation(false, new Vector2(296, 500), "LavaIsland_MainPath_12");
            locations.Add(LavaIsland_MainPath_12);
            MapLocation LavaIsland_MainPath_13 = new MapLocation(false, new Vector2(244, 470), "LavaIsland_MainPath_13");
            locations.Add(LavaIsland_MainPath_13);
            MapLocation LavaIsland_MainPath_14 = new MapLocation(false, new Vector2(178, 465), "LavaIsland_MainPath_14");
            locations.Add(LavaIsland_MainPath_14);
            MapLocation LavaIsland_MainPath_15 = new MapLocation(false, new Vector2(133, 484), "LavaIsland_MainPath_15");
            locations.Add(LavaIsland_MainPath_15);

            MapLocation LavaIsland_MainPath_16 = new MapLocation(false, new Vector2(77, 474), "LavaIsland_MainPath_16");
            locations.Add(LavaIsland_MainPath_16);
            MapLocation LavaIsland_MainPath_17 = new MapLocation(false, new Vector2(55, 440), "LavaIsland_MainPath_17");
            locations.Add(LavaIsland_MainPath_17);


            //setup location neighbors

            #region Connection to Skull Island

            SkullIsland_BottomRow_1.neighborLeft = LavaIsland_MainPath_1;
            LavaIsland_MainPath_1.neighborRight = SkullIsland_BottomRow_1;

            #endregion


            #region Main Path Connections

            //(left)
            LavaIsland_MainPath_1.neighborDown = LavaIsland_MainPath_2;
            LavaIsland_MainPath_2.neighborLeft = LavaIsland_MainPath_3;
            LavaIsland_MainPath_3.neighborLeft = LavaIsland_MainPath_4;
            LavaIsland_MainPath_4.neighborLeft = LavaIsland_MainPath_5;
            LavaIsland_MainPath_5.neighborLeft = LavaIsland_MainPath_6;
            LavaIsland_MainPath_6.neighborLeft = LavaIsland_MainPath_7;
            LavaIsland_MainPath_7.neighborUp = LavaIsland_MainPath_8;
            LavaIsland_MainPath_8.neighborLeft = LavaIsland_MainPath_9;
            LavaIsland_MainPath_9.neighborUp = LavaIsland_MainPath_9.neighborLeft = LavaIsland_MainPath_10;
            LavaIsland_MainPath_10.neighborLeft = LavaIsland_MainEntrance;

            LavaIsland_MainEntrance.neighborLeft = LavaIsland_MainEntrance.neighborDown = LavaIsland_MainPath_11;
            LavaIsland_MainPath_11.neighborLeft = LavaIsland_MainPath_12;
            LavaIsland_MainPath_12.neighborLeft = LavaIsland_MainPath_13;
            LavaIsland_MainPath_13.neighborLeft = LavaIsland_MainPath_14;
            LavaIsland_MainPath_14.neighborLeft = LavaIsland_MainPath_15;

            LavaIsland_MainPath_15.neighborLeft = LavaIsland_MainPath_16;
            LavaIsland_MainPath_16.neighborLeft = LavaIsland_MainPath_17;


            //(right)
            LavaIsland_MainPath_2.neighborUp = LavaIsland_MainPath_1;
            LavaIsland_MainPath_3.neighborRight = LavaIsland_MainPath_2;
            LavaIsland_MainPath_4.neighborRight = LavaIsland_MainPath_3;
            LavaIsland_MainPath_5.neighborRight = LavaIsland_MainPath_4;
            LavaIsland_MainPath_6.neighborRight = LavaIsland_MainPath_5;

            LavaIsland_MainPath_7.neighborRight = LavaIsland_MainPath_6;
            LavaIsland_MainPath_8.neighborDown = LavaIsland_MainPath_7;
            LavaIsland_MainPath_9.neighborRight = LavaIsland_MainPath_8;
            LavaIsland_MainPath_10.neighborDown = LavaIsland_MainPath_10.neighborRight = LavaIsland_MainPath_9;
            LavaIsland_MainEntrance.neighborRight = LavaIsland_MainPath_10;

            LavaIsland_MainPath_11.neighborRight = LavaIsland_MainPath_11.neighborUp = LavaIsland_MainEntrance;
            LavaIsland_MainPath_12.neighborRight = LavaIsland_MainPath_11;
            LavaIsland_MainPath_13.neighborRight = LavaIsland_MainPath_12;
            LavaIsland_MainPath_14.neighborRight = LavaIsland_MainPath_13;
            LavaIsland_MainPath_15.neighborRight = LavaIsland_MainPath_14;

            LavaIsland_MainPath_16.neighborRight = LavaIsland_MainPath_15;
            LavaIsland_MainPath_17.neighborRight = LavaIsland_MainPath_16;

            #endregion


            #region Additonal Connections

            //connection to forest path
            ForestIsland_MainPath_27.neighborUp = LavaIsland_MainPath_12;
            LavaIsland_MainPath_12.neighborDown = ForestIsland_MainPath_27;

            #endregion


            #endregion


            #region Cloud Island

            CloudIsland_MainEntrance = new MapLocation(true, new Vector2(678, 264), "CloudIsland_MainEntrance");
            CloudIsland_MainEntrance.ID = LevelID.CloudIsland_MainEntrance;
            locations.Add(CloudIsland_MainEntrance);

            //main path - starting and extending up and left from skull island crown 2, (left side)
            MapLocation CloudIsland_MainPath_1 = new MapLocation(false, new Vector2(760, 348), "CloudIsland_MainPath_1");
            locations.Add(CloudIsland_MainPath_1);
            MapLocation CloudIsland_MainPath_2 = new MapLocation(false, new Vector2(742, 302), "CloudIsland_MainPath_2");
            locations.Add(CloudIsland_MainPath_2);
            //this is dungeon entrance
            //MapLocation CloudIsland_MainPath_3 = new MapLocation(false, new Vector2(678, 264), "CloudIsland_MainPath_3");
            //locations.Add(CloudIsland_MainPath_3);
            MapLocation CloudIsland_MainPath_4 = new MapLocation(false, new Vector2(651, 310), "CloudIsland_MainPath_4");
            locations.Add(CloudIsland_MainPath_4);
            MapLocation CloudIsland_MainPath_5 = new MapLocation(false, new Vector2(567, 324), "CloudIsland_MainPath_5");
            locations.Add(CloudIsland_MainPath_5);

            MapLocation CloudIsland_MainPath_6 = new MapLocation(false, new Vector2(507, 298), "CloudIsland_MainPath_6");
            locations.Add(CloudIsland_MainPath_6);
            MapLocation CloudIsland_MainPath_7 = new MapLocation(false, new Vector2(460, 277), "CloudIsland_MainPath_7");
            locations.Add(CloudIsland_MainPath_7);
            MapLocation CloudIsland_MainPath_8 = new MapLocation(false, new Vector2(410, 258), "CloudIsland_MainPath_8");
            locations.Add(CloudIsland_MainPath_8);
            MapLocation CloudIsland_MainPath_9 = new MapLocation(false, new Vector2(404, 212), "CloudIsland_MainPath_9");
            locations.Add(CloudIsland_MainPath_9);
            MapLocation CloudIsland_MainPath_10 = new MapLocation(false, new Vector2(422, 170), "CloudIsland_MainPath_10");
            locations.Add(CloudIsland_MainPath_10);





            //setup location neighbors

            #region Connection to Skull Island

            SkullIsland_TopCrown_2.neighborLeft = CloudIsland_MainPath_1;
            CloudIsland_MainPath_1.neighborRight = SkullIsland_TopCrown_2;

            #endregion


            #region Main Path Connections

            //left up from skull island
            CloudIsland_MainPath_1.neighborLeft = CloudIsland_MainPath_1.neighborUp = CloudIsland_MainPath_2;
            CloudIsland_MainPath_2.neighborLeft = CloudIsland_MainPath_2.neighborUp = CloudIsland_MainEntrance;
            CloudIsland_MainEntrance.neighborLeft = CloudIsland_MainPath_4;
            CloudIsland_MainPath_4.neighborLeft = CloudIsland_MainPath_5;

            CloudIsland_MainPath_5.neighborLeft = CloudIsland_MainPath_5.neighborUp = CloudIsland_MainPath_6;
            CloudIsland_MainPath_6.neighborUp = CloudIsland_MainPath_7;
            CloudIsland_MainPath_7.neighborLeft = CloudIsland_MainPath_7.neighborUp = CloudIsland_MainPath_8;
            CloudIsland_MainPath_8.neighborUp = CloudIsland_MainPath_9;
            CloudIsland_MainPath_9.neighborUp = CloudIsland_MainPath_10;

            //return connections
            CloudIsland_MainPath_2.neighborRight = CloudIsland_MainPath_2.neighborDown = CloudIsland_MainPath_1;
            CloudIsland_MainEntrance.neighborRight = CloudIsland_MainPath_2;
            CloudIsland_MainPath_4.neighborRight = CloudIsland_MainPath_4.neighborUp = CloudIsland_MainEntrance;
            CloudIsland_MainPath_5.neighborRight = CloudIsland_MainPath_4;

            CloudIsland_MainPath_6.neighborRight = CloudIsland_MainPath_5;
            CloudIsland_MainPath_7.neighborRight = CloudIsland_MainPath_6;
            CloudIsland_MainPath_8.neighborRight = CloudIsland_MainPath_7;
            CloudIsland_MainPath_9.neighborDown = CloudIsland_MainPath_8;
            CloudIsland_MainPath_10.neighborDown = CloudIsland_MainPath_9;



            #endregion


            #region Additonal Connections



            #endregion


            #endregion


            #region Swamp Island

            //in the future, this will become the swamp main entrance, but it's empty for now
            SwampIsland_MainEntrance_OLD = new MapLocation(false, new Vector2(1319, 237), "SwampIsland_MainEntrance");
            SwampIsland_MainEntrance_OLD.ID = LevelID.SwampIsland_MainEntrance;
            locations.Add(SwampIsland_MainEntrance_OLD);

            //main path - starting and extending up and left from skull island crown 2, (left side)
            MapLocation SwampIsland_MainPath_1 = new MapLocation(false, new Vector2(1086, 222), "SwampIsland_MainPath_1");
            locations.Add(SwampIsland_MainPath_1);
            MapLocation SwampIsland_MainPath_2 = new MapLocation(false, new Vector2(1103, 239), "SwampIsland_MainPath_2");
            locations.Add(SwampIsland_MainPath_2);
            MapLocation SwampIsland_MainPath_3 = new MapLocation(false, new Vector2(1139, 248), "SwampIsland_MainPath_3");
            locations.Add(SwampIsland_MainPath_3);
            MapLocation SwampIsland_MainPath_4 = new MapLocation(false, new Vector2(1190, 228), "SwampIsland_MainPath_4");
            locations.Add(SwampIsland_MainPath_4);
            MapLocation SwampIsland_MainPath_5 = new MapLocation(false, new Vector2(1248, 245), "SwampIsland_MainPath_5");
            locations.Add(SwampIsland_MainPath_5);

            MapLocation SwampIsland_Main_Connection = new MapLocation(false, new Vector2(1204, 276), "SwampIsland_Main_Connection");
            locations.Add(SwampIsland_Main_Connection);

            MapLocation SwampIsland_MainPath_6 = new MapLocation(false, new Vector2(1317, 259), "SwampIsland_MainPath_6");
            locations.Add(SwampIsland_MainPath_6);
            MapLocation SwampIsland_MainPath_7 = new MapLocation(false, new Vector2(1402, 275), "SwampIsland_MainPath_7");
            locations.Add(SwampIsland_MainPath_7);
            MapLocation SwampIsland_MainPath_8 = new MapLocation(false, new Vector2(1407, 319), "SwampIsland_MainPath_8");
            locations.Add(SwampIsland_MainPath_8);
            MapLocation SwampIsland_MainPath_9 = new MapLocation(false, new Vector2(1469, 332), "SwampIsland_MainPath_9");
            locations.Add(SwampIsland_MainPath_9);
            MapLocation SwampIsland_MainPath_10 = new MapLocation(false, new Vector2(1539, 321), "SwampIsland_MainPath_10");
            locations.Add(SwampIsland_MainPath_10);

            MapLocation SwampIsland_MainPath_11 = new MapLocation(false, new Vector2(1596, 300), "SwampIsland_MainPath_11");
            locations.Add(SwampIsland_MainPath_11);
            MapLocation SwampIsland_MainPath_12 = new MapLocation(false, new Vector2(1626, 271), "SwampIsland_MainPath_12");
            locations.Add(SwampIsland_MainPath_12);
            MapLocation SwampIsland_MainPath_13 = new MapLocation(false, new Vector2(1653, 237), "SwampIsland_MainPath_13");
            locations.Add(SwampIsland_MainPath_13);
            MapLocation SwampIsland_MainPath_14 = new MapLocation(false, new Vector2(1713, 231), "SwampIsland_MainPath_14");
            locations.Add(SwampIsland_MainPath_14);
            MapLocation SwampIsland_MainPath_15 = new MapLocation(false, new Vector2(1755, 258), "SwampIsland_MainPath_15");
            locations.Add(SwampIsland_MainPath_15);

            MapLocation SwampIsland_MainPath_16 = new MapLocation(false, new Vector2(1816, 247), "SwampIsland_MainPath_16");
            locations.Add(SwampIsland_MainPath_16);
            MapLocation SwampIsland_MainPath_17 = new MapLocation(false, new Vector2(1834, 284), "SwampIsland_MainPath_17");
            locations.Add(SwampIsland_MainPath_17);
            MapLocation SwampIsland_MainPath_18 = new MapLocation(false, new Vector2(1895, 280), "SwampIsland_MainPath_18");
            locations.Add(SwampIsland_MainPath_18);
            MapLocation SwampIsland_MainPath_19 = new MapLocation(false, new Vector2(1949, 325), "SwampIsland_MainPath_19");
            locations.Add(SwampIsland_MainPath_19);


            //setup location neighbors

            #region Connection to Skull Island

            //this is done thru the islands MAIN CONNECTION instance
            SwampIsland_Main_Connection.neighborLeft = SkullIsland_TopCrown_8;
            SkullIsland_TopCrown_8.neighborRight = SwampIsland_Main_Connection;

            #endregion


            #region Main Path Connections

            //left to right, from mental hospital graveyard thing
            SwampIsland_MainPath_1.neighborRight = SwampIsland_MainPath_2;
            SwampIsland_MainPath_2.neighborRight = SwampIsland_MainPath_3;
            SwampIsland_MainPath_3.neighborRight = SwampIsland_MainPath_4;
            SwampIsland_MainPath_4.neighborRight = SwampIsland_MainPath_5;

            SwampIsland_MainPath_5.neighborDown = SwampIsland_Main_Connection;
            SwampIsland_MainPath_5.neighborRight = SwampIsland_MainPath_6;
            SwampIsland_MainPath_6.neighborUp = SwampIsland_MainEntrance_OLD;
            SwampIsland_MainPath_6.neighborRight = SwampIsland_MainPath_7;
            SwampIsland_MainPath_7.neighborDown = SwampIsland_MainPath_8;
            SwampIsland_MainPath_8.neighborRight = SwampIsland_MainPath_9;
            SwampIsland_MainPath_9.neighborRight = SwampIsland_MainPath_10;

            SwampIsland_MainPath_10.neighborRight = SwampIsland_MainPath_10.neighborUp = SwampIsland_MainPath_11;
            SwampIsland_MainPath_11.neighborRight = SwampIsland_MainPath_11.neighborUp = SwampIsland_MainPath_12;
            SwampIsland_MainPath_12.neighborRight = SwampIsland_MainPath_12.neighborUp = SwampIsland_MainPath_13;
            SwampIsland_MainPath_13.neighborRight = SwampIsland_MainPath_14;
            SwampIsland_MainPath_14.neighborDown = SwampIsland_MainPath_14.neighborRight = SwampIsland_MainPath_15;

            SwampIsland_MainPath_15.neighborRight = SwampIsland_MainPath_15.neighborDown = SwampIsland_MainPath_16;
            SwampIsland_MainPath_16.neighborRight = SwampIsland_MainPath_16.neighborDown = SwampIsland_MainPath_17;
            SwampIsland_MainPath_17.neighborRight = SwampIsland_MainPath_18;
            SwampIsland_MainPath_18.neighborRight = SwampIsland_MainPath_18.neighborDown = SwampIsland_MainPath_19;



            //return connections
            SwampIsland_MainPath_2.neighborLeft = SwampIsland_MainPath_1;
            SwampIsland_MainPath_3.neighborLeft = SwampIsland_MainPath_2;
            SwampIsland_MainPath_4.neighborLeft = SwampIsland_MainPath_3;

            SwampIsland_MainPath_5.neighborLeft = SwampIsland_MainPath_4;
            SwampIsland_Main_Connection.neighborUp = SwampIsland_Main_Connection.neighborRight = SwampIsland_MainPath_5;
            SwampIsland_MainPath_6.neighborLeft = SwampIsland_MainPath_5;
            SwampIsland_MainEntrance_OLD.neighborDown = SwampIsland_MainPath_6;
            SwampIsland_MainPath_7.neighborLeft = SwampIsland_MainPath_6;
            SwampIsland_MainPath_8.neighborUp = SwampIsland_MainPath_7;
            SwampIsland_MainPath_9.neighborLeft = SwampIsland_MainPath_8;
            SwampIsland_MainPath_10.neighborLeft = SwampIsland_MainPath_9;

            SwampIsland_MainPath_11.neighborDown = SwampIsland_MainPath_11.neighborLeft = SwampIsland_MainPath_10;
            SwampIsland_MainPath_12.neighborDown = SwampIsland_MainPath_11;
            SwampIsland_MainPath_13.neighborDown = SwampIsland_MainPath_12;
            SwampIsland_MainPath_14.neighborLeft = SwampIsland_MainPath_13;
            SwampIsland_MainPath_15.neighborLeft = SwampIsland_MainPath_14;

            SwampIsland_MainPath_16.neighborLeft = SwampIsland_MainPath_16.neighborUp = SwampIsland_MainPath_15;
            SwampIsland_MainPath_17.neighborLeft = SwampIsland_MainPath_16;
            SwampIsland_MainPath_18.neighborLeft = SwampIsland_MainPath_17;
            SwampIsland_MainPath_19.neighborUp = SwampIsland_MainPath_19.neighborLeft = SwampIsland_MainPath_18;


            #endregion


            #region Additonal Connections



            #endregion





            #endregion


            #region Theives Den - Bottom Path

            ThievesDen_GateEntrance = new MapLocation(true, new Vector2(1277, 789), "ThievesDen_GateEntrance");
            ThievesDen_GateEntrance.ID = LevelID.ThievesDen_GateEntrance;
            locations.Add(ThievesDen_GateEntrance);

            //main path - starting going right from forest main path at sm plateau island
            MapLocation ThievesDen_ExtBottomPath_1 = new MapLocation(false, new Vector2(956, 800), "ThievesDen_ExtBottomPath_1");
            locations.Add(ThievesDen_ExtBottomPath_1);
            MapLocation ThievesDen_ExtBottomPath_2 = new MapLocation(false, new Vector2(1019, 793), "ThievesDen_ExtBottomPath_2");
            locations.Add(ThievesDen_ExtBottomPath_2);
            MapLocation ThievesDen_ExtBottomPath_3 = new MapLocation(false, new Vector2(1061, 768), "ThievesDen_ExtBottomPath_3");
            locations.Add(ThievesDen_ExtBottomPath_3);
            MapLocation ThievesDen_ExtBottomPath_4 = new MapLocation(false, new Vector2(1119, 764), "ThievesDen_ExtBottomPath_4");
            locations.Add(ThievesDen_ExtBottomPath_4);
            MapLocation ThievesDen_ExtBottomPath_5 = new MapLocation(false, new Vector2(1156, 808), "ThievesDen_ExtBottomPath_5");
            locations.Add(ThievesDen_ExtBottomPath_5);

            MapLocation ThievesDen_ExtBottomPath_6 = new MapLocation(false, new Vector2(1231, 821), "ThievesDen_ExtBottomPath_6");
            locations.Add(ThievesDen_ExtBottomPath_6);
            MapLocation ThievesDen_ExtBottomPath_7 = new MapLocation(false, new Vector2(1283, 811), "ThievesDen_ExtBottomPath_7");
            locations.Add(ThievesDen_ExtBottomPath_7);
            MapLocation ThievesDen_ExtBottomPath_8 = new MapLocation(false, new Vector2(1309, 821), "ThievesDen_ExtBottomPath_8");
            locations.Add(ThievesDen_ExtBottomPath_8);
            MapLocation ThievesDen_ExtBottomPath_9 = new MapLocation(false, new Vector2(1347, 822), "ThievesDen_ExtBottomPath_9");
            locations.Add(ThievesDen_ExtBottomPath_9);
            MapLocation ThievesDen_ExtBottomPath_10 = new MapLocation(false, new Vector2(1371, 802), "ThievesDen_ExtBottomPath_10");
            locations.Add(ThievesDen_ExtBottomPath_10);

            MapLocation ThievesDen_ExtBottomPath_11 = new MapLocation(false, new Vector2(1414, 798), "ThievesDen_ExtBottomPath_11");
            locations.Add(ThievesDen_ExtBottomPath_11);
            MapLocation ThievesDen_ExtBottomPath_12 = new MapLocation(false, new Vector2(1454, 801), "ThievesDen_ExtBottomPath_12");
            locations.Add(ThievesDen_ExtBottomPath_12);
            MapLocation ThievesDen_ExtBottomPath_13 = new MapLocation(false, new Vector2(1487, 811), "ThievesDen_ExtBottomPath_13");
            locations.Add(ThievesDen_ExtBottomPath_13);
            MapLocation ThievesDen_ExtBottomPath_14 = new MapLocation(false, new Vector2(1524, 804), "ThievesDen_ExtBottomPath_14");
            locations.Add(ThievesDen_ExtBottomPath_14);
            MapLocation ThievesDen_ExtBottomPath_15 = new MapLocation(false, new Vector2(1560, 778), "ThievesDen_ExtBottomPath_15");
            locations.Add(ThievesDen_ExtBottomPath_15);

            MapLocation ThievesDen_ExtBottomPath_16 = new MapLocation(false, new Vector2(1607, 777), "ThievesDen_ExtBottomPath_16");
            locations.Add(ThievesDen_ExtBottomPath_16);
            MapLocation ThievesDen_ExtBottomPath_17 = new MapLocation(false, new Vector2(1642, 784), "ThievesDen_ExtBottomPath_17");
            locations.Add(ThievesDen_ExtBottomPath_17);
            MapLocation ThievesDen_ExtBottomPath_18 = new MapLocation(false, new Vector2(1663, 801), "ThievesDen_ExtBottomPath_18");
            locations.Add(ThievesDen_ExtBottomPath_18);
            MapLocation ThievesDen_ExtBottomPath_19 = new MapLocation(false, new Vector2(1696, 795), "ThievesDen_ExtBottomPath_19");
            locations.Add(ThievesDen_ExtBottomPath_19);
            MapLocation ThievesDen_ExtBottomPath_20 = new MapLocation(false, new Vector2(1734, 795), "ThievesDen_ExtBottomPath_20");
            locations.Add(ThievesDen_ExtBottomPath_20);



            //setup location neighbors

            #region Connection to Skull Island

            ForestIsland_MainPath_9.neighborRight = ThievesDen_ExtBottomPath_1;
            ThievesDen_ExtBottomPath_1.neighborLeft = ForestIsland_MainPath_9;

            #endregion


            #region Main Path Connections

            //right
            ThievesDen_ExtBottomPath_1.neighborRight = ThievesDen_ExtBottomPath_2;
            ThievesDen_ExtBottomPath_2.neighborRight = ThievesDen_ExtBottomPath_3;
            ThievesDen_ExtBottomPath_3.neighborRight = ThievesDen_ExtBottomPath_4;
            ThievesDen_ExtBottomPath_4.neighborRight = ThievesDen_ExtBottomPath_5;

            ThievesDen_ExtBottomPath_5.neighborRight = ThievesDen_ExtBottomPath_6;
            ThievesDen_ExtBottomPath_6.neighborUp = ThievesDen_GateEntrance;
            ThievesDen_ExtBottomPath_6.neighborRight = ThievesDen_ExtBottomPath_7;
            ThievesDen_ExtBottomPath_7.neighborRight = ThievesDen_ExtBottomPath_8;
            ThievesDen_ExtBottomPath_8.neighborRight = ThievesDen_ExtBottomPath_9;
            ThievesDen_ExtBottomPath_9.neighborRight = ThievesDen_ExtBottomPath_10;

            ThievesDen_ExtBottomPath_10.neighborRight = ThievesDen_ExtBottomPath_11;
            ThievesDen_ExtBottomPath_11.neighborRight = ThievesDen_ExtBottomPath_12;
            ThievesDen_ExtBottomPath_12.neighborRight = ThievesDen_ExtBottomPath_13;
            ThievesDen_ExtBottomPath_13.neighborRight = ThievesDen_ExtBottomPath_14;
            ThievesDen_ExtBottomPath_14.neighborRight = ThievesDen_ExtBottomPath_15;

            ThievesDen_ExtBottomPath_15.neighborRight = ThievesDen_ExtBottomPath_16;
            ThievesDen_ExtBottomPath_16.neighborRight = ThievesDen_ExtBottomPath_17;
            ThievesDen_ExtBottomPath_17.neighborRight = ThievesDen_ExtBottomPath_18;
            ThievesDen_ExtBottomPath_18.neighborRight = ThievesDen_ExtBottomPath_19;
            ThievesDen_ExtBottomPath_19.neighborRight = ThievesDen_ExtBottomPath_20;


            //left
            ThievesDen_ExtBottomPath_2.neighborLeft = ThievesDen_ExtBottomPath_1;
            ThievesDen_ExtBottomPath_3.neighborLeft = ThievesDen_ExtBottomPath_2;
            ThievesDen_ExtBottomPath_4.neighborLeft = ThievesDen_ExtBottomPath_3;
            ThievesDen_ExtBottomPath_5.neighborLeft = ThievesDen_ExtBottomPath_4;

            ThievesDen_ExtBottomPath_6.neighborLeft = ThievesDen_ExtBottomPath_5;
            ThievesDen_GateEntrance.neighborLeft = ThievesDen_ExtBottomPath_6;
            ThievesDen_GateEntrance.neighborRight = ThievesDen_GateEntrance.neighborDown = ThievesDen_ExtBottomPath_7;
            ThievesDen_ExtBottomPath_7.neighborLeft = ThievesDen_ExtBottomPath_6;
            ThievesDen_ExtBottomPath_7.neighborUp = ThievesDen_GateEntrance;
            ThievesDen_ExtBottomPath_8.neighborLeft = ThievesDen_ExtBottomPath_7;
            ThievesDen_ExtBottomPath_9.neighborLeft = ThievesDen_ExtBottomPath_8;
            ThievesDen_ExtBottomPath_10.neighborLeft = ThievesDen_ExtBottomPath_9;

            ThievesDen_ExtBottomPath_11.neighborLeft = ThievesDen_ExtBottomPath_10;
            ThievesDen_ExtBottomPath_12.neighborLeft = ThievesDen_ExtBottomPath_11;
            ThievesDen_ExtBottomPath_13.neighborLeft = ThievesDen_ExtBottomPath_12;
            ThievesDen_ExtBottomPath_14.neighborLeft = ThievesDen_ExtBottomPath_13;
            ThievesDen_ExtBottomPath_15.neighborLeft = ThievesDen_ExtBottomPath_14;

            ThievesDen_ExtBottomPath_16.neighborLeft = ThievesDen_ExtBottomPath_15;
            ThievesDen_ExtBottomPath_17.neighborLeft = ThievesDen_ExtBottomPath_16;
            ThievesDen_ExtBottomPath_18.neighborLeft = ThievesDen_ExtBottomPath_17;
            ThievesDen_ExtBottomPath_19.neighborLeft = ThievesDen_ExtBottomPath_18;
            ThievesDen_ExtBottomPath_20.neighborLeft = ThievesDen_ExtBottomPath_19;

            #endregion


            #region Additonal Connections




            #endregion





            #endregion


            #region Thieves Den - Left Path

            //main path - starting from skul bottom row6 down to thieves den bottom path 3
            MapLocation ThievesDen_ExtLeftPath_1 = new MapLocation(false, new Vector2(1124, 560), "ThievesDen_ExtLeftPath_1");
            locations.Add(ThievesDen_ExtLeftPath_1);
            MapLocation ThievesDen_ExtLeftPath_2 = new MapLocation(false, new Vector2(1155, 577), "ThievesDen_ExtLeftPath_2");
            locations.Add(ThievesDen_ExtLeftPath_2);
            MapLocation ThievesDen_ExtLeftPath_3 = new MapLocation(false, new Vector2(1116, 606), "ThievesDen_ExtLeftPath_3");
            locations.Add(ThievesDen_ExtLeftPath_3);
            MapLocation ThievesDen_ExtLeftPath_4 = new MapLocation(false, new Vector2(1114, 637), "ThievesDen_ExtLeftPath_4");
            locations.Add(ThievesDen_ExtLeftPath_4);
            MapLocation ThievesDen_ExtLeftPath_5 = new MapLocation(false, new Vector2(1077, 652), "ThievesDen_ExtLeftPath_5");
            locations.Add(ThievesDen_ExtLeftPath_5);

            MapLocation ThievesDen_ExtLeftPath_6 = new MapLocation(false, new Vector2(1057, 677), "ThievesDen_ExtLeftPath_6");
            locations.Add(ThievesDen_ExtLeftPath_6);
            MapLocation ThievesDen_ExtLeftPath_7 = new MapLocation(false, new Vector2(1075, 716), "ThievesDen_ExtLeftPath_7");
            locations.Add(ThievesDen_ExtLeftPath_7);


            #region Neighbors

            //skull island connection
            ThievesDen_ExtLeftPath_1.neighborUp = SkullIsland_BottomRow_6;
            SkullIsland_BottomRow_6.neighborDown = ThievesDen_ExtLeftPath_1;

            //down
            ThievesDen_ExtLeftPath_1.neighborDown = ThievesDen_ExtLeftPath_1.neighborRight = ThievesDen_ExtLeftPath_2;
            ThievesDen_ExtLeftPath_2.neighborDown = ThievesDen_ExtLeftPath_2.neighborLeft = ThievesDen_ExtLeftPath_3;
            ThievesDen_ExtLeftPath_3.neighborDown = ThievesDen_ExtLeftPath_4;
            ThievesDen_ExtLeftPath_4.neighborDown = ThievesDen_ExtLeftPath_4.neighborLeft = ThievesDen_ExtLeftPath_5;
            ThievesDen_ExtLeftPath_5.neighborDown = ThievesDen_ExtLeftPath_5.neighborLeft = ThievesDen_ExtLeftPath_6;
            ThievesDen_ExtLeftPath_6.neighborDown = ThievesDen_ExtLeftPath_7;
            ThievesDen_ExtLeftPath_7.neighborDown = ThievesDen_ExtBottomPath_3;

            //up
            ThievesDen_ExtLeftPath_2.neighborUp = ThievesDen_ExtLeftPath_2.neighborLeft = ThievesDen_ExtLeftPath_1;
            ThievesDen_ExtLeftPath_3.neighborUp = ThievesDen_ExtLeftPath_3.neighborRight = ThievesDen_ExtLeftPath_2;
            ThievesDen_ExtLeftPath_4.neighborUp = ThievesDen_ExtLeftPath_3;
            ThievesDen_ExtLeftPath_5.neighborUp = ThievesDen_ExtLeftPath_5.neighborRight = ThievesDen_ExtLeftPath_4;
            ThievesDen_ExtLeftPath_6.neighborUp = ThievesDen_ExtLeftPath_6.neighborRight = ThievesDen_ExtLeftPath_5;
            ThievesDen_ExtLeftPath_7.neighborUp = ThievesDen_ExtLeftPath_6;
            ThievesDen_ExtBottomPath_3.neighborUp = ThievesDen_ExtLeftPath_7;

            #endregion



            #endregion


            #region Thieves Den - Right Path

            //main path - starting from death mth main path down to thieves den bottom path
            MapLocation ThievesDen_ExtRightPath_1 = new MapLocation(false, new Vector2(1606, 581), "ThievesDen_ExtRightPath_1");
            locations.Add(ThievesDen_ExtRightPath_1);
            MapLocation ThievesDen_ExtRightPath_2 = new MapLocation(false, new Vector2(1656, 609), "ThievesDen_ExtRightPath_2");
            locations.Add(ThievesDen_ExtRightPath_2);
            MapLocation ThievesDen_ExtRightPath_3 = new MapLocation(false, new Vector2(1610, 647), "ThievesDen_ExtRightPath_3");
            locations.Add(ThievesDen_ExtRightPath_3);
            MapLocation ThievesDen_ExtRightPath_4 = new MapLocation(false, new Vector2(1634, 675), "ThievesDen_ExtRightPath_4");
            locations.Add(ThievesDen_ExtRightPath_4);
            MapLocation ThievesDen_ExtRightPath_5 = new MapLocation(false, new Vector2(1643, 709), "ThievesDen_ExtRightPath_5");
            locations.Add(ThievesDen_ExtRightPath_5);

            MapLocation ThievesDen_ExtRightPath_6 = new MapLocation(false, new Vector2(1696, 720), "ThievesDen_ExtRightPath_6");
            locations.Add(ThievesDen_ExtRightPath_6);
            MapLocation ThievesDen_ExtRightPath_7 = new MapLocation(false, new Vector2(1747, 718), "ThievesDen_ExtRightPath_7");
            locations.Add(ThievesDen_ExtRightPath_7);
            MapLocation ThievesDen_ExtRightPath_8 = new MapLocation(false, new Vector2(1790, 699), "ThievesDen_ExtRightPath_8");
            locations.Add(ThievesDen_ExtRightPath_8);
            MapLocation ThievesDen_ExtRightPath_9 = new MapLocation(false, new Vector2(1849, 699), "ThievesDen_ExtRightPath_9");
            locations.Add(ThievesDen_ExtRightPath_9);
            MapLocation ThievesDen_ExtRightPath_10 = new MapLocation(false, new Vector2(1888, 720), "ThievesDen_ExtRightPath_10");
            locations.Add(ThievesDen_ExtRightPath_10);

            MapLocation ThievesDen_ExtRightPath_11 = new MapLocation(false, new Vector2(1897, 755), "ThievesDen_ExtRightPath_11");
            locations.Add(ThievesDen_ExtRightPath_11);
            MapLocation ThievesDen_ExtRightPath_12 = new MapLocation(false, new Vector2(1884, 794), "ThievesDen_ExtRightPath_12");
            locations.Add(ThievesDen_ExtRightPath_12);
            MapLocation ThievesDen_ExtRightPath_13 = new MapLocation(false, new Vector2(1850, 806), "ThievesDen_ExtRightPath_13");
            locations.Add(ThievesDen_ExtRightPath_13);
            MapLocation ThievesDen_ExtRightPath_14 = new MapLocation(false, new Vector2(1834, 779), "ThievesDen_ExtRightPath_14");
            locations.Add(ThievesDen_ExtRightPath_14);




            #region Neighbors

            //top connection to death mtn
            DeathMountain_MainPath_7.neighborDown = ThievesDen_ExtRightPath_1;
            ThievesDen_ExtRightPath_1.neighborUp = DeathMountain_MainPath_7;

            //down
            ThievesDen_ExtRightPath_1.neighborDown = ThievesDen_ExtRightPath_2;
            ThievesDen_ExtRightPath_2.neighborDown = ThievesDen_ExtRightPath_3;
            ThievesDen_ExtRightPath_3.neighborDown = ThievesDen_ExtRightPath_4;
            ThievesDen_ExtRightPath_4.neighborDown = ThievesDen_ExtRightPath_5;

            ThievesDen_ExtRightPath_5.neighborRight = ThievesDen_ExtRightPath_6;
            ThievesDen_ExtRightPath_6.neighborRight = ThievesDen_ExtRightPath_7;
            ThievesDen_ExtRightPath_7.neighborRight = ThievesDen_ExtRightPath_8;
            ThievesDen_ExtRightPath_8.neighborRight = ThievesDen_ExtRightPath_9;
            ThievesDen_ExtRightPath_9.neighborRight = ThievesDen_ExtRightPath_10;

            ThievesDen_ExtRightPath_10.neighborDown = ThievesDen_ExtRightPath_11;
            ThievesDen_ExtRightPath_11.neighborDown = ThievesDen_ExtRightPath_12;
            ThievesDen_ExtRightPath_12.neighborLeft = ThievesDen_ExtRightPath_13;
            ThievesDen_ExtRightPath_13.neighborLeft = ThievesDen_ExtRightPath_13.neighborUp = ThievesDen_ExtRightPath_14;


            //up
            ThievesDen_ExtRightPath_2.neighborUp = ThievesDen_ExtRightPath_1;
            ThievesDen_ExtRightPath_3.neighborUp = ThievesDen_ExtRightPath_2;
            ThievesDen_ExtRightPath_4.neighborUp = ThievesDen_ExtRightPath_3;
            ThievesDen_ExtRightPath_5.neighborUp = ThievesDen_ExtRightPath_4;

            ThievesDen_ExtRightPath_6.neighborUp = ThievesDen_ExtRightPath_5;
            ThievesDen_ExtRightPath_7.neighborUp = ThievesDen_ExtRightPath_6;
            ThievesDen_ExtRightPath_8.neighborUp = ThievesDen_ExtRightPath_7;
            ThievesDen_ExtRightPath_9.neighborUp = ThievesDen_ExtRightPath_8;
            ThievesDen_ExtRightPath_10.neighborUp = ThievesDen_ExtRightPath_9;

            ThievesDen_ExtRightPath_11.neighborUp = ThievesDen_ExtRightPath_10;
            ThievesDen_ExtRightPath_12.neighborUp = ThievesDen_ExtRightPath_12.neighborRight = ThievesDen_ExtRightPath_11;
            ThievesDen_ExtRightPath_13.neighborRight = ThievesDen_ExtRightPath_12;
            ThievesDen_ExtRightPath_14.neighborRight = ThievesDen_ExtRightPath_14.neighborDown = ThievesDen_ExtRightPath_13;


            #endregion



            #endregion


            #region Giant Sea Hole

            //connects right path to giant sea hole via south connection
            MapLocation SeaHole_Path_1 = new MapLocation(false, new Vector2(1735, 743), "SeaHole_Path_1");
            locations.Add(SeaHole_Path_1);
            MapLocation SeaHole_Path_2 = new MapLocation(false, new Vector2(1749, 762), "SeaHole_Path_2");
            locations.Add(SeaHole_Path_2);


            #region Neighbors

            ThievesDen_ExtRightPath_7.neighborDown = SeaHole_Path_1;
            SeaHole_Path_1.neighborUp = ThievesDen_ExtRightPath_7;

            SeaHole_Path_1.neighborDown = SeaHole_Path_1.neighborRight = SeaHole_Path_2;
            SeaHole_Path_2.neighborUp = SeaHole_Path_2.neighborLeft = SeaHole_Path_1;

            SeaHole_Path_2.neighborRight = Sea_GiantHole;
            Sea_GiantHole.neighborLeft = SeaHole_Path_2;

            Sea_GiantHole.neighborDown = ThievesDen_ExtBottomPath_20;
            ThievesDen_ExtBottomPath_20.neighborUp = Sea_GiantHole;
            ThievesDen_ExtBottomPath_20.neighborRight = Sea_GiantHole;

            Sea_GiantHole.neighborRight = ThievesDen_ExtRightPath_14;
            ThievesDen_ExtRightPath_14.neighborLeft = Sea_GiantHole;

            #endregion


            #endregion




            #region Skull Island SubPath

            MapLocation SkullIsland_SubPath_1 = new MapLocation(false, new Vector2(952, 567), "SkullIsland_SubPath_1");
            locations.Add(SkullIsland_SubPath_1);
            MapLocation SkullIsland_SubPath_2 = new MapLocation(false, new Vector2(991, 591), "SkullIsland_SubPath_2");
            locations.Add(SkullIsland_SubPath_2);

            //neighbors
            SkullIsland_SubPath_1.neighborUp = SkullIsland_BottomRow_4;
            SkullIsland_BottomRow_4.neighborDown = SkullIsland_SubPath_1;

            SkullIsland_SubPath_1.neighborDown = SkullIsland_SubPath_2;
            SkullIsland_SubPath_2.neighborUp = SkullIsland_SubPath_1;

            #endregion


            #region Path into Crest (middle plateau) Island, from castle

            MapLocation CrestIsland_Path_1 = new MapLocation(false, new Vector2(880, 692), "CrestIsland_Path_1");
            locations.Add(CrestIsland_Path_1);
            MapLocation CrestIsland_Path_2 = new MapLocation(false, new Vector2(928, 707), "CrestIsland_Path_2");
            locations.Add(CrestIsland_Path_2);
            MapLocation CrestIsland_Path_3 = new MapLocation(false, new Vector2(975, 692), "CrestIsland_Path_3");
            locations.Add(CrestIsland_Path_3);
            MapLocation CrestIsland_Path_4 = new MapLocation(false, new Vector2(988, 720), "CrestIsland_Path_4");
            locations.Add(CrestIsland_Path_4);

            //neighbors
            CrestIsland_Path_1.neighborUp = ForestIsland_MainPath_4;
            ForestIsland_MainPath_4.neighborDown = CrestIsland_Path_1;

            CrestIsland_Path_1.neighborRight = CrestIsland_Path_2;
            CrestIsland_Path_2.neighborLeft = CrestIsland_Path_1;

            CrestIsland_Path_2.neighborUp = CrestIsland_Path_2.neighborRight = CrestIsland_Path_3;
            CrestIsland_Path_3.neighborLeft = CrestIsland_Path_2;

            CrestIsland_Path_3.neighborDown = CrestIsland_Path_4;
            CrestIsland_Path_4.neighborUp = CrestIsland_Path_3;

            #endregion


            #region Fisherman's Path, down left of marsh/forest castle

            MapLocation Fishermans_Path_0 = new MapLocation(false, new Vector2(782, 549), "Fishermans_Path_0");
            locations.Add(Fishermans_Path_0);
            MapLocation Fishermans_Path_1 = new MapLocation(false, new Vector2(751, 586), "Fishermans_Path_1");
            locations.Add(Fishermans_Path_1);
            MapLocation Fishermans_Path_2 = new MapLocation(false, new Vector2(719, 621), "Fishermans_Path_2");
            locations.Add(Fishermans_Path_2);
            MapLocation Fishermans_Path_3 = new MapLocation(false, new Vector2(725, 660), "Fishermans_Path_3");
            locations.Add(Fishermans_Path_3);

            MapLocation Fishermans_House = new MapLocation(false, new Vector2(682, 629), "Fishermans_House");
            locations.Add(Fishermans_House);

            MapLocation Fishermans_Path_4 = new MapLocation(false, new Vector2(752, 693), "Fishermans_Path_4");
            locations.Add(Fishermans_Path_4);
            MapLocation Fishermans_Path_5 = new MapLocation(false, new Vector2(730, 724), "Fishermans_Path_5");
            locations.Add(Fishermans_Path_5);


            //neighbors
            Fishermans_Path_0.neighborLeft = LavaIsland_MainPath_2;
            LavaIsland_MainPath_2.neighborRight = Fishermans_Path_0;

            Fishermans_Path_0.neighborRight = ForestIsland_MainPath_1;
            ForestIsland_MainPath_1.neighborLeft = Fishermans_Path_0;

            Fishermans_Path_0.neighborDown = Fishermans_Path_1;
            Fishermans_Path_1.neighborUp = Fishermans_Path_0;

            Fishermans_Path_1.neighborDown = Fishermans_Path_2;
            Fishermans_Path_2.neighborUp = Fishermans_Path_1;

            Fishermans_Path_2.neighborLeft = Fishermans_House;
            Fishermans_House.neighborRight = Fishermans_Path_2;

            Fishermans_Path_2.neighborDown = Fishermans_Path_3;
            Fishermans_Path_3.neighborUp = Fishermans_Path_2;

            Fishermans_Path_3.neighborDown = Fishermans_Path_4;
            Fishermans_Path_4.neighborUp = Fishermans_Path_3;

            Fishermans_Path_4.neighborDown = Fishermans_Path_5;
            Fishermans_Path_5.neighborUp = Fishermans_Path_4;

            Fishermans_Path_4.neighborRight = ForestIsland_MainPath_6;
            ForestIsland_MainPath_6.neighborLeft = Fishermans_Path_4;

            #endregion


            #region Forest Castle

            MapLocation ForestCastle = new MapLocation(false, new Vector2(789, 659), "ForestCastle");
            locations.Add(ForestCastle);

            //neighbors
            ForestCastle.neighborRight = ForestIsland_MainPath_5;
            ForestIsland_MainPath_5.neighborLeft = ForestCastle;
            

            #endregion


            #region Forest Path Connecting Right to Thieves Den Path on Left

            //shortcut path from right of forest castle right to td watchtower
            MapLocation ForestIsland_To_ThievesDenLeft_1 = new MapLocation(false, new Vector2(928, 660), "ForestIsland_To_ThievesDenLeft_1");
            locations.Add(ForestIsland_To_ThievesDenLeft_1);
            MapLocation ForestIsland_To_ThievesDenLeft_2 = new MapLocation(false, new Vector2(976, 658), "ForestIsland_To_ThievesDenLeft_2");
            locations.Add(ForestIsland_To_ThievesDenLeft_2);
            MapLocation ForestIsland_To_ThievesDenLeft_3 = new MapLocation(false, new Vector2(1021, 665), "ForestIsland_To_ThievesDenLeft_3");
            locations.Add(ForestIsland_To_ThievesDenLeft_3);

            //neighbors right
            ForestIsland_MainPath_4.neighborRight = ForestIsland_To_ThievesDenLeft_1;
            ForestIsland_To_ThievesDenLeft_1.neighborRight = ForestIsland_To_ThievesDenLeft_2;
            ForestIsland_To_ThievesDenLeft_2.neighborRight = ForestIsland_To_ThievesDenLeft_3;
            ForestIsland_To_ThievesDenLeft_3.neighborRight = ThievesDen_ExtLeftPath_6;

            //neighbors left
            ThievesDen_ExtLeftPath_6.neighborLeft = ForestIsland_To_ThievesDenLeft_3;
            ForestIsland_To_ThievesDenLeft_3.neighborLeft = ForestIsland_To_ThievesDenLeft_2;
            ForestIsland_To_ThievesDenLeft_2.neighborLeft = ForestIsland_To_ThievesDenLeft_1;
            ForestIsland_To_ThievesDenLeft_1.neighborLeft = ForestIsland_MainPath_4;

            #endregion








            #region Cloud Temple

            MapLocation CloudTemple = new MapLocation(false, new Vector2(767, 400), "CloudTemple");
            locations.Add(CloudTemple);

            //neighbors
            CloudTemple.neighborRight = SkullIsland_TopCrown_1;
            SkullIsland_TopCrown_1.neighborLeft = CloudTemple;


            #endregion


            #region Lava Cave 

            MapLocation Lava_Cave_1 = new MapLocation(false, new Vector2(694, 474), "Lava_Cave_1");
            locations.Add(Lava_Cave_1);

            //neighbors
            Lava_Cave_1.neighborRight = LavaIsland_MainPath_1;
            LavaIsland_MainPath_1.neighborLeft = Lava_Cave_1;


            #endregion



            //prep overworld for interaction

            #region Setup location zDepths

            for (i = 0; i < locations.Count; i++)
            { Functions_Component.SetZdepth(locations[i].compSprite); }

            #endregion

            
            
        }

        public override void Open()
        {
            
            

            #region What location should the game place Link upon opening?

            //set currentlocation based on last loaded level
            Debug.WriteLine("field id to load: " + LevelSet.field.ID);


            //skull island
            if (LevelSet.field.ID == LevelID.SkullIsland_ShadowKing)
            { currentLocation = SkullIsland_ShadowDungeon; }
            else if (LevelSet.field.ID == LevelID.SkullIsland_Colliseum
                || LevelSet.field.ID == LevelID.SkullIsland_ColliseumPit)
            { currentLocation = SkullIsland_Coliseum; }
            else if (LevelSet.field.ID == LevelID.SkullIsland_Town)
            { currentLocation = SkullIsland_Town; }

            //death mountain
            else if (LevelSet.field.ID == LevelID.DeathMountain_MainEntrance)
            { currentLocation = DeathMountain_MainEntrance; }

            //forest island
            else if (LevelSet.field.ID == LevelID.ForestIsland_MainEntrance)
            { currentLocation = ForestIsland_MainEntrance; }

            //lava island
            else if (LevelSet.field.ID == LevelID.LavaIsland_MainEntrance)
            { currentLocation = LavaIsland_MainEntrance; }

            //cloud island
            else if (LevelSet.field.ID == LevelID.CloudIsland_MainEntrance)
            { currentLocation = CloudIsland_MainEntrance; }

            //swamp island
            else if (LevelSet.field.ID == LevelID.SwampIsland_MainEntrance)
            { currentLocation = SkullIsland_ShadowDungeon; }

            //thieves den
            else if (LevelSet.field.ID == LevelID.ThievesDen_GateEntrance)
            { currentLocation = ThievesDen_GateEntrance; }




            //default to shadowking if last level is unknown
            else { currentLocation = SkullIsland_ShadowDungeon; }
            //set target to current (no initial target)
            targetLocation = currentLocation;

            #endregion



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

            //teleport camera to hero's position - no fly in
            Camera2D.targetPosition.X = Pool.hero.compSprite.position.X;
            Camera2D.targetPosition.Y = Pool.hero.compSprite.position.Y;
            Camera2D.currentPosition = Camera2D.targetPosition;
            Functions_Camera2D.SetView();

            base.Open();
            Functions_Pool.Reset();

            //clear out all wave spawn positions, we will set them below
            waveSpawnPositions = new List<Vector2>();


            #region Skull island decorations

            //various decorative flags
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 920, 459); //left house in water

            //coliseum flags - left to right (15)

            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 847, 410); //1
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 849, 415); //2
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 854, 418); //3
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 859, 421); //4
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 865, 423); //5
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 873, 425); //6
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 882, 426); //7 - door top left
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 893, 426); //8 - door top right
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 903, 425); //9
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 911, 423); //10
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 917, 421); //11
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 922, 419); //12
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 926, 416); //13
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 925, 410); //14
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 920, 406); //15





            //skull king campfires
            Functions_Particle.Spawn(ObjType.Particle_Map_Campfire, 1018, 490); //left
            Functions_Particle.Spawn(ObjType.Particle_Map_Campfire, 1062, 482); //right

            //wave spawn locations
            waveSpawnPositions.Add(new Vector2(1090, 483)); //in front right skullking
            waveSpawnPositions.Add(new Vector2(1095, 458)); //in front right skullking
            waveSpawnPositions.Add(new Vector2(963, 482)); //in front left skullking
            waveSpawnPositions.Add(new Vector2(1010, 290)); //above center skull king
            waveSpawnPositions.Add(new Vector2(937, 355)); //above left skull king
            waveSpawnPositions.Add(new Vector2(1083, 336)); //above left skull king

            #endregion


            //other wave spawn locations
            waveSpawnPositions.Add(new Vector2(786, 553)); //entering fishermans path
            waveSpawnPositions.Add(new Vector2(741, 447)); //near intro lava cave
            waveSpawnPositions.Add(new Vector2(1272, 481)); //heading towards death mountain



            Functions_Music.PlayMusic(Music.Title); //play overworld music
            ColorScheme.background = ColorScheme.bkg_overworld;

            base.SetupLinesToNeighbors(); //draw the starting neighbor lines
        }

    }
    

}