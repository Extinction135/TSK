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
        ScreenRec overlay = new ScreenRec();

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
            overlay.alpha = 1.0f;
            overlay.fadeInSpeed = 0.04f;
            overlay.fadeOutSpeed = 0.04f;

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

            //right side of top map
            MapLocation colliseumRight = new MapLocation(false, new Vector2(365, 175));
            locations.Add(colliseumRight);
            MapLocation centerTown = new MapLocation(false, new Vector2(339, 141));
            locations.Add(centerTown);
            MapLocation gate = new MapLocation(false, new Vector2(421, 160));
            locations.Add(gate);
            MapLocation castle = new MapLocation(false, new Vector2(442, 135));
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
            MapLocation forestDungeon = new MapLocation(true, new Vector2(265, 125));
            locations.Add(forestDungeon);

            //town south of cave entrance
            MapLocation leftTown3 = new MapLocation(false, new Vector2(182, 179));
            locations.Add(leftTown3);
            MapLocation caveDungeon = new MapLocation(true, new Vector2(166, 150));
            locations.Add(caveDungeon);

            //top left town and town south of it
            MapLocation leftTown2 = new MapLocation(true, new Vector2(126, 182));
            locations.Add(leftTown2);
            MapLocation leftTown1 = new MapLocation(false, new Vector2(115, 219));
            locations.Add(leftTown1);

            //bottom left building (church)
            MapLocation leftTownChurch = new MapLocation(false, new Vector2(174, 255));
            locations.Add(leftTownChurch);
            MapLocation leftIslandConnector = new MapLocation(false, new Vector2(175, 296));
            locations.Add(leftIslandConnector);
            MapLocation swampDungeon = new MapLocation(true, new Vector2(117, 275));
            locations.Add(swampDungeon);

            //bottom right part of map
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

            //The Farm
            MapLocation centerIsland = new MapLocation(true, new Vector2(344, 293));
            locations.Add(centerIsland);

            #endregion


            #region Set maplocation's neighbors


            //dev neighbors - this may remain
            ship.neighborDown = centerIsland;
            centerIsland.neighborUp = ship;

            //standard neighbors
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
            leftIslandConnector.neighborLeft = swampDungeon;
            swampDungeon.neighborRight = leftIslandConnector;

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


            #region SETUP ACCESSIBLE LOCATIONS

            //requires that the 'true' boolean is passed into the MapLocation
            //constructor for the location created above. else, ignored.

            //1. Setup accessible level's ID
            colliseum.ID = LevelID.Colliseum;
            forestDungeon.ID = LevelID.Forest_Entrance;
            centerIsland.ID = LevelID.TheFarm;
            leftTown2.ID = LevelID.LeftTown2;
            caveDungeon.ID = LevelID.Mountain_Entrance;
            swampDungeon.ID = LevelID.Swamp_Entrance;


            //2. Setup current location based on level id
            // -> because hero may of exited a dungeon

            if (PlayerData.current.lastLocation == LevelID.TheFarm)
            { currentLocation = centerIsland; }
            else if (PlayerData.current.lastLocation == LevelID.LeftTown2)
            { currentLocation = leftTown2; }

            else if(PlayerData.current.lastLocation == LevelID.Colliseum
                || PlayerData.current.lastLocation == LevelID.ColliseumPit)
            { currentLocation = colliseum; }

            else if (PlayerData.current.lastLocation == LevelID.Forest_Entrance
                || PlayerData.current.lastLocation == LevelID.Forest_Dungeon)
            { currentLocation = forestDungeon; }

            else if (PlayerData.current.lastLocation == LevelID.Mountain_Entrance
                || PlayerData.current.lastLocation == LevelID.Mountain_Dungeon)
            { currentLocation = caveDungeon; }

            else if (PlayerData.current.lastLocation == LevelID.Swamp_Entrance
                || PlayerData.current.lastLocation == LevelID.Swamp_Dungeon)
            { currentLocation = swampDungeon; }



            else //default to colliseum if unknown
            { currentLocation = colliseum; }
            //set target to current (no initial target)
            targetLocation = currentLocation;

            #endregion



            //setup hero overworld actor
            hero = Pool.hero;
            hero.feetFX.visible = false;

            //teleport hero to current location
            Functions_Movement.Teleport(hero.compMove,
                currentLocation.compSprite.position.X,
                currentLocation.compSprite.position.Y - 8);
            Functions_Component.Align(hero);

            //prevents drown sprite from appearing, if hero died in water
            hero.underwater = false; hero.swimming = false;
            Functions_Actor.Update(hero);

            //play the title music
            Functions_Music.PlayMusic(Music.Title);
            //prevent kick drum from playing during overworld map
            if (Pool.hero.health < 3) { Pool.hero.health = 3; } 
            //grab the hero's current loadout
            Functions_Hero.SetLoadout();
            //reset the pool
            Functions_Pool.Reset();


            #region Add Animated Particles / Sprites to Map

            //create castle flags
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 451 + 8, 97 + 6);
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 468 + 8, 106 + 6);
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 485 + 8, 98 + 6);
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 464 + 8, 82 + 6);
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 474 + 8, 79 + 6);
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 465 + 8, 71 + 6);
            //create additional flags
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 334 + 8, 97 + 6); //old town
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 320 + 8, 113 + 6); //old town
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 357 + 8, 99 + 6); //old town
            Functions_Particle.Spawn(ObjType.Particle_Map_Flag, 305 + 8, 147 + 6); //colliseum

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
            Functions_Particle.Spawn(ObjType.Particle_Map_Campfire, 505, 257); //tent town
            Functions_Particle.Spawn(ObjType.Particle_Map_Campfire, 299, 297); //center island
            
            
            #endregion


            //set the player's sprite based on the hero.actorType
            if (PlayerData.current.actorType == ActorType.Blob)
            { hero.compSprite.texture = Assets.blobSheet; }
            else if(PlayerData.current.actorType == ActorType.Hero)
            { hero.compSprite.texture = Assets.heroSheet; }

            //autosave the current game, etc..
            Functions_Backend.SaveGame(GameFile.AutoSave);
            Assets.Play(Assets.sfxMapOpen);
            Assets.colorScheme.background = Assets.colorScheme.bkg_lightWorld;
        }

        public override void HandleInput(GameTime GameTime)
        {
            if (displayState == DisplayState.Opened)
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
                            //spawn a dash puff at the hero's feet
                            Functions_Particle.Spawn(ObjType.Particle_RisingSmoke,
                                hero.compSprite.position.X,
                                hero.compSprite.position.Y + 4);
                        }
                        else//if hero dies, he appears on map sitting
                        {   //pressing a direction will make him stand back up
                            hero.state = ActorState.Idle;
                            //hero.direction = Direction.Down;

                            //face hero in the direction of the input
                            hero.direction = cardinal;
                        }
                        
                    }
                    //check to see if player wants to load a level
                    if(Functions_Input.IsNewButtonPress(Buttons.A))
                    {   //upon A button press, check to see if current location is a level
                        if (currentLocation.isLevel) //if so, close the scroll
                        {
                            //save currentLocation into player data
                            PlayerData.current.lastLocation = currentLocation.ID;
                            //animate link into reward state
                            hero.state = ActorState.Reward;
                            hero.direction = Direction.Down;
                            displayState = DisplayState.Closing;
                            //force an animation update
                            Functions_Animation.Animate(hero.compAnim, hero.compSprite);
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
            //always animate the hero
            Functions_Actor.SetAnimationGroup(hero);
            Functions_Actor.SetAnimationDirection(hero);
            Functions_Animation.Animate(hero.compAnim, hero.compSprite);

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
                        //spawn attention particle at hero's feet
                        Functions_Particle.Spawn(ObjType.Particle_Attention,
                            hero.compSprite.position.X,
                            hero.compSprite.position.Y + 6);
                        //play arrival sound fx
                        Assets.Play(Assets.sfxMapLocation);

                        if(Flags.PrintOutput)
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


                //update particle list - particle animate and move (waves)
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
            }
            else if (displayState == DisplayState.Closing)
            {   //fade overlay in
                overlay.fadeState = FadeState.FadeIn;
                Functions_ScreenRec.Fade(overlay);
                if (overlay.fadeState == FadeState.FadeComplete)
                { displayState = DisplayState.Closed; }
            }
            else if (displayState == DisplayState.Closed)
            {   //set the level id based on the current location
                Level.ID = currentLocation.ID;
                //load the level, building the room(s)
                ScreenManager.ExitAndLoad(new ScreenLevel());
            }
        }

        public override void Draw(GameTime GameTime)
        {
            ScreenManager.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            
            Functions_Draw.Draw(map);
            Functions_Pool.Draw();
            for (i = 0; i < locations.Count; i++)
            { Functions_Draw.Draw(locations[i].compSprite); }
            Functions_Draw.Draw(hero.compSprite);
            Functions_Draw.Draw(overlay);

            ScreenManager.spriteBatch.End();
        }

    }
}
