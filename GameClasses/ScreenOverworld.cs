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
    public class ScreenOverworld : Screen
    {
        ScreenRec background = new ScreenRec();
        ScreenRec overlay = new ScreenRec();
        public Scroll scroll;
        public static ComponentSprite map;
        public List<MapLocation> locations;
        public int i;

        public Actor hero;
        Direction cardinal;
        MapLocation currentLocation; //where hero is currently
        MapLocation targetLocation; //where hero is going to

        Vector2 distance; //used in map movement routine
        float speed = 0.1f; //how fast hero moves between locations
        List<Vector2> waveSpawnPositions; //where wave particles can spawn
        Vector2 wavePos; //points to one of the vector2s in waveSpawnPositions list



        public ScreenOverworld() { this.name = "OverworldScreen"; }

        public override void LoadContent()
        {
            background.alpha = 1.0f;
            overlay.alpha = 1.0f;
            overlay.fadeInSpeed = 0.03f; //fade in slow

            scroll = new Scroll(new Vector2(16 * 3 - 8, 16 * 2 + 4), 34, 19);
            scroll.displayState = DisplayState.Opening;
            Assets.Play(Assets.sfxMapOpen);

            map = new ComponentSprite(Assets.overworldSheet,
                new Vector2(640/2, 360/2), new Byte4(0, 0, 0, 0), new Point(1280, 720));
            map.position.X = map.cellSize.X / 2;
            map.position.Y = map.cellSize.Y / 2;


            #region Create locations

            locations = new List<MapLocation>();

            MapLocation colliseum = new MapLocation(true, new Vector2(306, 195));
            locations.Add(colliseum);
            MapLocation ship = new MapLocation(false, new Vector2(281, 234));
            locations.Add(ship);
            MapLocation colliseumRight = new MapLocation(false, new Vector2(365, 175));
            locations.Add(colliseumRight);
            MapLocation centerTown = new MapLocation(false, new Vector2(339, 141));
            locations.Add(centerTown);
            MapLocation gate = new MapLocation(false, new Vector2(421, 160));
            locations.Add(gate);
            MapLocation castle = new MapLocation(true, new Vector2(442, 135));
            locations.Add(castle);
            MapLocation leftCastleTown = new MapLocation(false, new Vector2(431, 111));
            locations.Add(leftCastleTown);
            MapLocation rightCastleTown = new MapLocation(false, new Vector2(482, 137));
            locations.Add(rightCastleTown);
            MapLocation wallConnector = new MapLocation(false, new Vector2(465, 187));
            locations.Add(wallConnector);
            MapLocation rightTown = new MapLocation(false, new Vector2(514, 189));
            locations.Add(rightTown);
            
            MapLocation colliseumLeft = new MapLocation(false, new Vector2(258, 178));
            locations.Add(colliseumLeft);
            MapLocation forestDungeon = new MapLocation(false, new Vector2(265, 125));
            locations.Add(forestDungeon);
            MapLocation caveDungeon = new MapLocation(false, new Vector2(166, 150));
            locations.Add(caveDungeon);
            MapLocation leftTown3 = new MapLocation(false, new Vector2(182, 179));
            locations.Add(leftTown3);
            MapLocation leftTown2 = new MapLocation(false, new Vector2(126, 182));
            locations.Add(leftTown2);
            MapLocation leftTown1 = new MapLocation(false, new Vector2(115, 219));
            locations.Add(leftTown1);
            MapLocation leftTownChurch = new MapLocation(false, new Vector2(174, 255));
            locations.Add(leftTownChurch);
            MapLocation leftIslandConnector = new MapLocation(false, new Vector2(175, 296));
            locations.Add(leftIslandConnector);
            MapLocation castleRuins = new MapLocation(false, new Vector2(117, 275));
            locations.Add(castleRuins);

            MapLocation tentTown = new MapLocation(false, new Vector2(483, 256));
            locations.Add(tentTown);
            MapLocation tentTownLeft = new MapLocation(false, new Vector2(447, 228));
            locations.Add(tentTownLeft);
            MapLocation cathedral = new MapLocation(false, new Vector2(396, 245));
            locations.Add(cathedral);
            MapLocation rightIslandConnector = new MapLocation(false, new Vector2(430, 300));
            locations.Add(rightIslandConnector);
            MapLocation monumentConnector = new MapLocation(false, new Vector2(485, 308));
            locations.Add(monumentConnector);
            MapLocation monument = new MapLocation(false, new Vector2(543, 298));
            locations.Add(monument);
            MapLocation centerIsland = new MapLocation(false, new Vector2(344, 293));
            locations.Add(centerIsland);

            #endregion


            //set level types
            castle.levelType = LevelType.Castle;
            colliseum.levelType = LevelType.Shop;


            #region Set maplocation's neighbors

            rightCastleTown.neighborLeft = castle;
            castle.neighborRight = rightCastleTown;
            leftCastleTown.neighborRight = castle;
            leftCastleTown.neighborDown = castle;
            castle.neighborLeft = leftCastleTown;
            castle.neighborUp = leftCastleTown;
            
            castle.neighborDown = gate;
            gate.neighborUp = castle;

            gate.neighborLeft = colliseumRight;
            gate.neighborRight = wallConnector;
            wallConnector.neighborLeft = gate;
            wallConnector.neighborRight = rightTown;
            rightTown.neighborLeft = wallConnector;

            colliseumRight.neighborRight = gate;
            colliseumRight.neighborLeft = colliseum;
            colliseumRight.neighborUp = centerTown;
            centerTown.neighborDown = colliseumRight;

            colliseum.neighborRight = colliseumRight;
            colliseum.neighborLeft = colliseumLeft;
            colliseum.neighborDown = ship;
            ship.neighborUp = colliseum;

            colliseumLeft.neighborRight = colliseum;
            colliseumLeft.neighborUp = forestDungeon;
            colliseumLeft.neighborLeft = leftTown3;
            forestDungeon.neighborDown = colliseumLeft;

            leftTown3.neighborRight = colliseumLeft;
            leftTown3.neighborUp = caveDungeon;
            leftTown3.neighborLeft = leftTown2;
            caveDungeon.neighborDown = leftTown3;

            leftTown2.neighborRight = leftTown3;
            leftTown2.neighborDown = leftTown1;
            leftTown1.neighborUp = leftTown2;
            leftTown1.neighborDown = leftTownChurch;
            leftTown1.neighborRight = leftTownChurch;
            leftTownChurch.neighborUp = leftTown1;
            leftTownChurch.neighborLeft = leftTown1;
            leftTownChurch.neighborDown = leftIslandConnector;
            leftIslandConnector.neighborUp = leftTownChurch;
            leftIslandConnector.neighborLeft = castleRuins;
            castleRuins.neighborRight = leftIslandConnector;

            rightTown.neighborDown = tentTown;
            tentTown.neighborUp = rightTown;
            tentTown.neighborLeft = tentTownLeft;
            tentTown.neighborDown = rightIslandConnector;

            tentTownLeft.neighborRight = tentTown;
            tentTownLeft.neighborLeft = cathedral;
            cathedral.neighborRight = tentTownLeft;

            rightIslandConnector.neighborUp = tentTown;
            rightIslandConnector.neighborRight = monumentConnector;
            rightIslandConnector.neighborLeft = centerIsland;
            centerIsland.neighborRight = rightIslandConnector;

            monumentConnector.neighborLeft = rightIslandConnector;
            monumentConnector.neighborRight = monument;
            monument.neighborLeft = monumentConnector;

            #endregion


            #region Set Starting Location

            //figure out the starting location based on previousLevel instance
            for (i = 0; i < locations.Count; i++)
            {
                if (locations[i].levelType == Level.type)
                { currentLocation = locations[i]; }
            }
            //set target to current (no initial target)
            targetLocation = currentLocation;

            hero = new Actor();
            //set hero at the current location
            Functions_Movement.Teleport(hero.compMove,
                currentLocation.compSprite.position.X,
                currentLocation.compSprite.position.Y - 8);
            Functions_Component.Align(hero.compMove, hero.compSprite, hero.compCollision);

            #endregion


            //play the title music
            Functions_Music.PlayMusic(Music.Title);
            //fill hero's health up to max - prevents drum track from playing
            Pool.hero.health = PlayerData.current.heartsTotal;
            //grab the hero's current loadout
            Functions_Hero.SetLoadout();
            //reset the pool
            Functions_Pool.Reset();


            #region Add Animated Particles / Sprites to Map

            //create castle flags
            Functions_Entity.SpawnEntity(ObjType.ParticleMapFlag, 451 + 8, 97 + 6, Direction.None);
            Functions_Entity.SpawnEntity(ObjType.ParticleMapFlag, 468 + 8, 106 + 6, Direction.None);
            Functions_Entity.SpawnEntity(ObjType.ParticleMapFlag, 485 + 8, 98 + 6, Direction.None);
            Functions_Entity.SpawnEntity(ObjType.ParticleMapFlag, 464 + 8, 82 + 6, Direction.None);
            Functions_Entity.SpawnEntity(ObjType.ParticleMapFlag, 474 + 8, 79 + 6, Direction.None);
            Functions_Entity.SpawnEntity(ObjType.ParticleMapFlag, 465 + 8, 71 + 6, Direction.None);
            //create additional flags
            Functions_Entity.SpawnEntity(ObjType.ParticleMapFlag, 334 + 8, 97 + 6, Direction.None); //old town
            Functions_Entity.SpawnEntity(ObjType.ParticleMapFlag, 320 + 8, 113 + 6, Direction.None); //old town
            Functions_Entity.SpawnEntity(ObjType.ParticleMapFlag, 357 + 8, 99 + 6, Direction.None); //old town
            Functions_Entity.SpawnEntity(ObjType.ParticleMapFlag, 305 + 8, 147 + 6, Direction.None); //colliseum

            //create a list of positions where to place waves
            waveSpawnPositions = new List<Vector2>();
            waveSpawnPositions.Add(new Vector2(334 + 8, 238 + 6));
            waveSpawnPositions.Add(new Vector2(207 + 8, 226 + 6));
            waveSpawnPositions.Add(new Vector2(260 + 8, 254 + 6));
            waveSpawnPositions.Add(new Vector2(360 + 8, 270 + 6));
            waveSpawnPositions.Add(new Vector2(407 + 8, 277 + 6));
            waveSpawnPositions.Add(new Vector2(237 + 8, 293 + 6));
            waveSpawnPositions.Add(new Vector2(089 + 8, 305 + 6));
            waveSpawnPositions.Add(new Vector2(557 + 8, 259 + 6));
            waveSpawnPositions.Add(new Vector2(555 + 8, 125 + 6));
            waveSpawnPositions.Add(new Vector2(506 + 8, 090 + 6));
            waveSpawnPositions.Add(new Vector2(120 + 8, 090 + 6));
            waveSpawnPositions.Add(new Vector2(094 + 8, 121 + 6));

            //create map campfires
            Functions_Entity.SpawnEntity(ObjType.ParticleMapCampfire, 505, 257, Direction.None); //tent town
            Functions_Entity.SpawnEntity(ObjType.ParticleMapCampfire, 299, 297, Direction.None); //center island
            Functions_Entity.SpawnEntity(ObjType.ParticleMapCampfire, 109, 266, Direction.None); //left island / castle ruins
            
            #endregion


            //set the player's sprite based on the hero.actorType
            if (PlayerData.current.actorType == ActorType.Blob)
            { hero.compSprite.texture = Assets.blobSheet; }
            else if(PlayerData.current.actorType == ActorType.Hero)
            { hero.compSprite.texture = Assets.heroSheet; }

            //autosave the current game
            Functions_Backend.SaveGame(GameFile.AutoSave);
        }

        public override void HandleInput(GameTime GameTime)
        {
            if (scroll.displayState == DisplayState.Opened)
            {   //only allow input if the screen has opened completely..
                if(hero.compMove.position == hero.compMove.newPosition)
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
                            hero.state = ActorState.Move;
                            hero.direction = cardinal;
                            Assets.Play(Assets.sfxMapWalking);
                            Functions_Entity.SpawnEntity(ObjType.ParticleDashPuff,
                                hero.compSprite.position.X,
                                hero.compSprite.position.Y + 4, //at feet
                                Direction.None);
                        }
                    }
                    //check to see if player wants to load a level
                    if(Functions_Input.IsNewButtonPress(Buttons.A))
                    {   //upon A button press, check to see if current location is a level
                        if (currentLocation.isLevel) //if so, close the scroll
                        {
                            hero.state = ActorState.Reward;
                            hero.direction = Direction.Down;
                            scroll.displayState = DisplayState.Closing;
                            Assets.Play(Assets.sfxTextDone);
                            Assets.Play(Assets.sfxWindowClose);
                        }
                    }
                    else if (Functions_Input.IsNewButtonPress(Buttons.Start))
                    { ScreenManager.AddScreen(new ScreenInventory()); }
                }
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
                hero.compMove.newPosition.X = targetLocation.compSprite.position.X;
                hero.compMove.newPosition.Y = targetLocation.compSprite.position.Y - 8;

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
                    {   //set hero's animation to idle down
                        hero.state = ActorState.Idle;
                        hero.direction = Direction.Down;
                        currentLocation = targetLocation;
                        Assets.Play(Assets.sfxTextLetter);
                        Functions_Entity.SpawnEntity(ObjType.ParticleAttention,
                            hero.compSprite.position.X,
                            hero.compSprite.position.Y + 6, //at feet
                            Direction.None);
                    }
                }

                #endregion


                Functions_ActorAnimationList.SetAnimationGroup(hero);
                Functions_ActorAnimationList.SetAnimationDirection(hero);
                Functions_Animation.Animate(hero.compAnim, hero.compSprite);
                scroll.title.text = "Overworld Map - " + currentLocation.levelType;


                #region Wave Generation Routine
                
                if(Functions_Random.Int(0, 100) > 80)
                {   //randomly create a wave particle at a wave spawn location with random offset
                    wavePos = waveSpawnPositions[Functions_Random.Int(0, waveSpawnPositions.Count)];
                    Functions_Entity.SpawnEntity(ObjType.ParticleMapWave,
                        wavePos.X + Functions_Random.Int(-12, 12),
                        wavePos.Y + Functions_Random.Int(-12, 12), 
                        Direction.None);
                }

                #endregion


                Functions_Pool.UpdateGameObjList(Pool.entityPool, false);
            }
            else if (scroll.displayState == DisplayState.Closing)
            {   //fade overlay in
                overlay.fadeState = FadeState.FadeIn;
                Functions_ScreenRec.Fade(overlay);
                Functions_Scroll.AnimateClosed(scroll);
            }
            else if (scroll.displayState == DisplayState.Closed)
            {   //set the type of level dungeon screen is about to build
                Level.type = currentLocation.levelType;
                //load the dungeon screen, building the level
                ScreenManager.ExitAndLoad(new ScreenLevel());
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
                Functions_Pool.Draw();
                for (i = 0; i < locations.Count; i++)
                { Functions_Draw.Draw(locations[i].compSprite); }
                Functions_Draw.Draw(hero.compSprite);
            }
            Functions_Draw.Draw(overlay);
            ScreenManager.spriteBatch.End();
        }

    }
}
