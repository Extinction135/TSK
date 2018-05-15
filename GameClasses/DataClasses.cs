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
    //Global Classes
    
    public static class Flags
    {   // **********************************************************************************************************
        public static Boolean Release = false; //puts game in release mode, overwrites other flags
        // **********************************************************************************************************
        public static float Version = 0.71f; //the version of the game
        public static BootRoutine bootRoutine = BootRoutine.Editor; //boot to game or editor?
        //game flags
        public static Boolean EnableTopMenu = true; //enables the top debug menu (draw + input)
        public static Boolean DrawUDT = false; //draw the UpdateDrawTotal timing text?
        public static Boolean DrawDebugInfo = false; //draws the bottom debug info
        public static Boolean DrawCollisions = false; //draw/hide collision rec components
        public static Boolean DrawInput = false; //draw the input display
        public static Boolean DrawWatermark = false; //top right link (for capturing purposes)

        public static Boolean Paused = false; //controlled by topMenu 'play/pause' button
        public static Boolean PlayMusic = false; //turns music on/off
        public static Boolean PlaySoundFX = true; //turns soundfx on/off
        public static Boolean SpawnMobs = true; //toggles the spawning of lesser enemies (not bosses)
        public static Boolean ProcessAI = true; //apply AI input to enemies / actors
        public static Boolean CameraTracksHero = false; //camera tracks hero or centers to dungeon room
        public static Boolean ShowEnemySpawns = false; //create & draw enemySpawn gameObjects?
        public static Boolean PrintOutput = true; //print output to the debugger
        public static Boolean ShowDialogs = true; //turn dialogs on/off

        public static Boolean HardMode = false; //makes the game harder
        
        //cheats
        public static Boolean Invincibility = true; //does hero ignore damage?
        public static Boolean InfiniteMagic = true; //does hero ignore magic costs?
        public static Boolean InfiniteGold = true; //does hero ignore item/vendor costs?
        public static Boolean InfiniteArrows = true; //does hero ignore arrow cost?
        public static Boolean InfiniteBombs = true; //does hero ignore bomb cost?
        public static Boolean MapCheat = true; //sets dungeon.map true when dungeon is built
        public static Boolean KeyCheat = true; //sets dungeon.key true when dungeon is built
        public static Boolean UnlockAll = true; //unlocks all items for hero

        static Flags()
        {

            #region Set RELEASE Mode

            if (Release)
            {
                bootRoutine = BootRoutine.Game;
                //set flags for release mode
                EnableTopMenu = false;
                DrawDebugInfo = false;
                DrawCollisions = false;
                DrawInput = false;
                Paused = false;
                PlayMusic = true;
                PlaySoundFX = true;
                SpawnMobs = true;
                ProcessAI = true;
                DrawUDT = false;
                CameraTracksHero = false;
                ShowEnemySpawns = false;
                PrintOutput = false;
                ShowDialogs = true;
                DrawWatermark = false;
                //cheats
                Invincibility = false;
                InfiniteMagic = false;
                InfiniteGold = false;
                InfiniteArrows = false;
                InfiniteBombs = false;
                MapCheat = false;
                KeyCheat = false;
                UnlockAll = false;
            }

            #endregion

            else
            {
                if (bootRoutine == BootRoutine.Editor)
                {
                    //set dev mode cheats
                    Invincibility = true; //hero cannot die in editor
                    InfiniteMagic = true; //hero has infinite magic
                    InfiniteGold = true; //hero has infinite gold
                    InfiniteArrows = true; //hero has infinite arrows
                    InfiniteBombs = true; //hero has infinite bombs
                    MapCheat = true;
                    KeyCheat = true;
                    UnlockAll = true;

                    //handle editor cheats
                    EnableTopMenu = true; //necessary
                    ShowEnemySpawns = true; //necessary for editing
                }
            }
        }
    }

    public static class World
    {
        public static float frictionAttack = 0.1f; //actor in attacking state
        public static float frictionUse = 0.5f; //actor in use state

        public static float friction = 0.75f; //standard friction

        public static float frictionAir = 0.9f; //some slowdown
        public static float frictionIce = 0.99f; //no slowdown
    }



    public static class Camera2D
    {
        public static GraphicsDevice graphics = ScreenManager.game.GraphicsDevice;
        public static Boolean tracks = false; //does camera move or teleport?
        public static Vector2 currentPosition = Vector2.Zero;
        public static Vector2 targetPosition = Vector2.Zero;
        public static float speed = 0.1f; //how fast the camera moves
        public static Vector2 distance;

        public static float targetZoom = 1.0f;
        public static float currentZoom = 1.0f;
        public static float zoomSpeed = 0.05f;

        public static Matrix view = Matrix.Identity;
        public static Matrix matRotation = Matrix.CreateRotationZ(0.0f);
        public static Matrix matZoom;
        public static Vector3 translateCenter;
        public static Vector3 translateBody;
    }

    public static class PlayerData
    {   //current points to game1/game2/game3, or loads autoSave data
        public static SaveData current = new SaveData();
        public static SaveData game1 = new SaveData();
        public static SaveData game2 = new SaveData();
        public static SaveData game3 = new SaveData();
    }

    public static class DungeonRecord
    {
        public static int dungeonID = 0;
        public static Boolean beatDungeon = false;
        public static Stopwatch timer = new Stopwatch();
        public static int enemyCount = 0;
        public static int totalDamage = 0;

        public static void Reset()
        {
            dungeonID = 0;
            beatDungeon = false;
            timer = new Stopwatch();
            enemyCount = 0;
            totalDamage = 0;
            timer.Reset();
        }
    }

    public static class Pool
    {
        //actor pool handles all actors in the room including hero
        public static int actorCount;           //total count of actors in pool
        public static List<Actor> actorPool;    //the actual list of actors
        public static int actorIndex;           //used to iterate thru the pool
        public static int actorCounter = 0;

        //obj pool handles room objects, from dungeon & main sheet
        public static int roomObjCount;
        public static List<GameObject> roomObjPool;
        public static int roomObjIndex;
        public static int roomObjCounter = 0;

        //particle pool - main sheet only
        public static int particleCount;
        public static List<GameObject> particlePool;
        public static int particleIndex;
        public static int particleCounter = 0;

        //projectile pool - main sheet only
        public static int projectileCount;
        public static List<Projectile> projectilePool;
        public static int projectileIndex;
        public static int projectileCounter = 0;

        //pickup pool - main sheet only
        public static int pickupCount;
        public static List<GameObject> pickupPool;
        public static int pickupIndex;
        public static int pickupCounter = 0;

        //floor pool - dungeon sheet only
        public static int floorCount;
        public static List<ComponentSprite> floorPool;
        public static int floorIndex;
        public static int floorCounter = 0;

        public static int activeActor = 1; //tracks the current actor being handled by AI

        public static Actor hero; //points to actorPool[0]
        public static GameObject herosPet; //points to roomObj[0]

        public static int collisionsCount = 0; //tracks collisions per frame
        public static int interactionsCount = 0; //tracks interactions per frame

        public static void Initialize()
        {
            //set pool sizes
            actorCount = 30;
            floorCount = 500;
            roomObjCount = 500;
            particleCount = 200;
            projectileCount = 40;
            pickupCount = 20;

            //actor pool
            actorPool = new List<Actor>();
            for (actorCounter = 0; actorCounter < actorCount; actorCounter++)
            {
                actorPool.Add(new Actor());
                Functions_Actor.SetType(actorPool[actorCounter], ActorType.Hero);
                Functions_Movement.Teleport(actorPool[actorCounter].compMove, -100, -100);
                actorPool[actorCounter].active = false;
            }
            actorIndex = 1;

            //room obj pool
            roomObjPool = new List<GameObject>();
            for (roomObjCounter = 0; roomObjCounter < roomObjCount; roomObjCounter++)
            { roomObjPool.Add(new GameObject()); }
            roomObjIndex = 1;

            //particle pool
            particlePool = new List<GameObject>();
            for (particleCounter = 0; particleCounter < particleCount; particleCounter++)
            { particlePool.Add(new GameObject()); }
            particleIndex = 0;

            //projectile pool
            projectilePool = new List<Projectile>();
            for (projectileCounter = 0; projectileCounter < projectileCount; projectileCounter++)
            { projectilePool.Add(new Projectile()); }
            projectileIndex = 0;

            //pickup pool
            pickupPool = new List<GameObject>();
            for (pickupCounter = 0; pickupCounter < pickupCount; pickupCounter++)
            { pickupPool.Add(new GameObject()); }
            pickupIndex = 0;

            //floor pool
            floorPool = new List<ComponentSprite>();
            for (floorCounter = 0; floorCounter < floorCount; floorCounter++)
            {
                floorPool.Add(new ComponentSprite(Assets.forestLevelSheet,
                    new Vector2(0, 0), 
                    new Byte4(15, 0, 0, 0), 
                    new Point(16, 16)));
            }
            floorIndex = 0;

            //reset all pools
            Functions_Pool.Reset();
            //create easy to remember reference for hero & pet
            hero = actorPool[0];
            herosPet = roomObjPool[0];
            Functions_Actor.SetType(hero, ActorType.Hero);
            Functions_GameObject.SetType(herosPet, ObjType.Pet_Dog);
        }

    }

    public static class Timing
    {
        public static Stopwatch stopWatch = new Stopwatch();
        public static Stopwatch total = new Stopwatch();
        public static TimeSpan updateTime = new TimeSpan();
        public static TimeSpan drawTime = new TimeSpan();
        public static TimeSpan totalTime = new TimeSpan();
        public static void Reset() { stopWatch.Reset(); stopWatch.Start(); }
    }

    public static class Input
    {
        public static KeyboardState currentKeyboardState = new KeyboardState();
        public static KeyboardState lastKeyboardState = new KeyboardState();

        public static MouseState currentMouseState = new MouseState();
        public static MouseState lastMouseState = new MouseState();

        public static Point cursorPos = new Point(0, 0);
        public static ComponentCollision cursorColl = new ComponentCollision();

        public static GamePadState currentGamePadState = new GamePadState();
        public static GamePadState lastGamePadState = new GamePadState();

        //the amount of joystick movement classified as noise
        public static float deadzone = 0.10f; 
        public static Direction gamePadDirection = Direction.None;
        public static Direction lastGamePadDirection = Direction.None;

        static Input()
        {
            cursorColl.rec.Width = 4;
            cursorColl.rec.Height = 4;
            cursorColl.blocking = false;
        }
    }

    public static class WaterMark
    {
        public static MenuRectangle bkg;
        public static ComponentText textComp;

        static WaterMark()
        {
            bkg = new MenuRectangle(
                new Point(495, 20), 
                new Point(120-2, 9), 
                Assets.colorScheme.windowBkg);
            textComp = new ComponentText(Assets.font,
                "github.com/mrgrak/dungeonrun",
                new Vector2(bkg.position.X + 2, bkg.position.Y - 3),
                Assets.colorScheme.textLight);
        }
    }

    public static class InputDisplay
    {
        public static List<ComponentSprite> directionalBkg;
        public static List<ComponentSprite> buttonBkg;

        public static List<ComponentSprite> directions;
        public static List<ComponentSprite> buttons;

        static InputDisplay()
        {
            //bkgs
            directionalBkg = new List<ComponentSprite>();
            buttonBkg = new List<ComponentSprite>();
            for (int i = 0; i < 4; i++)
            {
                directionalBkg.Add(new ComponentSprite(Assets.uiItemsSheet,
                    new Vector2(0, 0),
                    AnimationFrames.Ui_QuadBkg[0],
                    new Point(16, 16)));
                buttonBkg.Add(new ComponentSprite(Assets.uiItemsSheet,
                    new Vector2(0, 0),
                    AnimationFrames.Ui_QuadBkg[0],
                    new Point(16, 16)));
            }
            Functions_Component.MoveQuadBkg(directionalBkg, 16 * 2 - 6, 16 * 2 - 6);
            Functions_Component.MoveQuadBkg(buttonBkg, 16 * 4 - 6, 16 * 2 - 6);


            #region Directions

            directions = new List<ComponentSprite>();
            for (int i = 0; i < 4; i++)
            {
                directions.Add(new ComponentSprite(Assets.uiItemsSheet,
                    new Vector2(0, 0),
                    AnimationFrames.Input_Arrow_Unselected[0],
                    new Point(8, 8)));
            }

            //up - position and rotate the sprites
            directions[0].position.X = 16 * 2 + 1;
            directions[0].position.Y = 16 * 2 - 6;
            directions[0].rotation = Rotation.Clockwise270;
            //right
            directions[1].position.X = directions[0].position.X + 7 + 2;
            directions[1].position.Y = directions[0].position.Y + 7 - 0;
            //down
            directions[2].position.X = directions[0].position.X + 0 + 2;
            directions[2].position.Y = directions[0].position.Y + 14 + 2;
            directions[2].rotation = Rotation.Clockwise90;
            //left
            directions[3].position.X = directions[0].position.X - 7;
            directions[3].position.Y = directions[0].position.Y + 7 + 2;
            directions[3].rotation = Rotation.Clockwise180;

            #endregion


            #region Buttons

            buttons = new List<ComponentSprite>();
            for (int i = 0; i < 5; i++)
            {
                buttons.Add(new ComponentSprite(Assets.uiItemsSheet,
                    new Vector2(0, 0),
                    AnimationFrames.Input_ButtonA_Unselected[0],
                    new Point(8, 8)));
            }

            //setup A button
            buttons[0].position.X = 16 * 4 + 2;
            buttons[0].position.Y = 16 * 2 + 7;
            buttons[0].currentFrame = AnimationFrames.Input_ButtonA_Unselected[0];

            //setup X button
            buttons[1].position.X = buttons[0].position.X - 6;
            buttons[1].position.Y = buttons[0].position.Y - 6;
            buttons[1].currentFrame = AnimationFrames.Input_ButtonX_Unselected[0];

            //setup Y button
            buttons[2].position.X = buttons[0].position.X - 0;
            buttons[2].position.Y = buttons[0].position.Y - 12;
            buttons[2].currentFrame = AnimationFrames.Input_ButtonY_Unselected[0];

            //setup B button
            buttons[3].position.X = buttons[0].position.X + 6;
            buttons[3].position.Y = buttons[0].position.Y - 6;
            buttons[3].currentFrame = AnimationFrames.Input_ButtonB_Unselected[0];

            //setup start button
            buttons[4].position.X = 16 * 2 + 2;
            buttons[4].position.Y = 16 * 2 + 1;
            buttons[4].currentFrame = AnimationFrames.Input_ButtonStart_Unselected[0];

            #endregion

        }

        public static void ReadController()
        {

            #region Directions

            //reset all to unselected
            directions[0].currentFrame = AnimationFrames.Input_Arrow_Unselected[0];
            directions[1].currentFrame = AnimationFrames.Input_Arrow_Unselected[0];
            directions[2].currentFrame = AnimationFrames.Input_Arrow_Unselected[0];
            directions[3].currentFrame = AnimationFrames.Input_Arrow_Unselected[0];

            //directions = up, right, down, left

            //handle cardinals
            if (Input.gamePadDirection == Direction.Up)
            { directions[0].currentFrame = AnimationFrames.Input_Arrow_Selected[0]; }
            else if (Input.gamePadDirection == Direction.Right)
            { directions[1].currentFrame = AnimationFrames.Input_Arrow_Selected[0]; }
            else if (Input.gamePadDirection == Direction.Down)
            { directions[2].currentFrame = AnimationFrames.Input_Arrow_Selected[0]; }
            else if (Input.gamePadDirection == Direction.Left)
            { directions[3].currentFrame = AnimationFrames.Input_Arrow_Selected[0]; }

            //handle dialgonals
            else if (Input.gamePadDirection == Direction.UpRight)
            {
                directions[0].currentFrame = AnimationFrames.Input_Arrow_Selected[0];
                directions[1].currentFrame = AnimationFrames.Input_Arrow_Selected[0];
            }
            else if (Input.gamePadDirection == Direction.UpLeft)
            {
                directions[0].currentFrame = AnimationFrames.Input_Arrow_Selected[0];
                directions[3].currentFrame = AnimationFrames.Input_Arrow_Selected[0];
            }

            else if (Input.gamePadDirection == Direction.DownRight)
            {
                directions[2].currentFrame = AnimationFrames.Input_Arrow_Selected[0];
                directions[1].currentFrame = AnimationFrames.Input_Arrow_Selected[0];
            }
            else if (Input.gamePadDirection == Direction.DownLeft)
            {
                directions[2].currentFrame = AnimationFrames.Input_Arrow_Selected[0];
                directions[3].currentFrame = AnimationFrames.Input_Arrow_Selected[0];
            }

            #endregion


            #region Buttons

            //reset all to unselected
            buttons[0].currentFrame = AnimationFrames.Input_ButtonA_Unselected[0];
            buttons[1].currentFrame = AnimationFrames.Input_ButtonX_Unselected[0];
            buttons[2].currentFrame = AnimationFrames.Input_ButtonY_Unselected[0];
            buttons[3].currentFrame = AnimationFrames.Input_ButtonB_Unselected[0];
            buttons[4].currentFrame = AnimationFrames.Input_ButtonStart_Unselected[0];

            //buttons = A, X, Y, B, Start
            if(Functions_Input.IsNewButtonPress(Buttons.A))
            { buttons[0].currentFrame = AnimationFrames.Input_ButtonA_Selected[0]; }
            if (Functions_Input.IsNewButtonPress(Buttons.X))
            { buttons[1].currentFrame = AnimationFrames.Input_ButtonX_Selected[0]; }
            if (Functions_Input.IsNewButtonPress(Buttons.Y))
            { buttons[2].currentFrame = AnimationFrames.Input_ButtonY_Selected[0]; }
            if (Functions_Input.IsNewButtonPress(Buttons.B))
            { buttons[3].currentFrame = AnimationFrames.Input_ButtonB_Selected[0]; }
            if (Functions_Input.IsNewButtonPress(Buttons.Start))
            { buttons[4].currentFrame = AnimationFrames.Input_ButtonStart_Selected[0]; }

            #endregion

        }
    }

    public static class WorldUI
    {
        public static int i;

        public static List<ComponentSprite> hearts;
        public static int lastHeartCount = 3;
        public static int currentHeartCount = 3;
        public static byte maxHearts = 0;

        public static List<ComponentSprite> meterPieces;

        public static List<ComponentSprite> weaponBkg;
        public static List<ComponentSprite> itemBkg;
        public static MenuItem currentWeapon;
        public static MenuItem currentItem;
        public static MenuItemType heroWeapon;
        public static MenuItemType heroItem;
        public static ComponentAmountDisplay itemAmount; //item ammo

        public static ComponentText frametime;
        public static ComponentText autosaveText;

        public static int autosaveCounter = 100;
        public static string autosaving = "autosaving...";

        static WorldUI()
        {
            int xPos = 50;
            int yPos = 50;


            #region Hearts ui

            //create the heart sprites
            hearts = new List<ComponentSprite>();
            for (i = 0; i < 9; i++)
            {
                hearts.Add(new ComponentSprite(
                    Assets.uiItemsSheet,
                    new Vector2(0, 0), 
                    new Byte4(3, 1, 0, 0),
                    new Point(16, 16)));
            }
            //move the hearts
            for (i = 0; i < 9; i++)
            {
                hearts[i].position.X = xPos + (10 * i) + (16 * 2) + 8;
                hearts[i].position.Y = yPos + 8;
            }

            #endregion


            #region Magic ui

            //create the meter sprites
            meterPieces = new List<ComponentSprite>();
            for (i = 0; i < 11; i++)
            {
                meterPieces.Add(new ComponentSprite(
                    Assets.uiItemsSheet,
                    new Vector2(0, 0), 
                    new Byte4(5*2, 0, 0, 0),
                    new Point(8, 16)));
            }
            //move the magic meter sprites
            for (i = 0; i < 11; i++)
            {
                meterPieces[i].position.X = xPos + (8 * i) + (16 * 2) + 8;
                meterPieces[i].position.Y = yPos + 8 + 16;
            }
            //set the head and tail meter frames
            meterPieces[0].currentFrame.X = 4*2;
            meterPieces[10].currentFrame.X = 4*2;
            meterPieces[10].flipHorizontally = true;
            //fix the 1 pixel offset
            meterPieces[10].position.X -= 1;

            #endregion


            #region Weapon and Item ui

            //create the weapon and item background sprites
            weaponBkg = new List<ComponentSprite>();
            itemBkg = new List<ComponentSprite>();
            for (i = 0; i < 4; i++)
            {
                weaponBkg.Add(new ComponentSprite(Assets.uiItemsSheet,
                    new Vector2(0, 0),
                    AnimationFrames.Ui_QuadBkg[0],
                    new Point(16, 16)));
                itemBkg.Add(new ComponentSprite(Assets.uiItemsSheet,
                    new Vector2(0, 0),
                    AnimationFrames.Ui_QuadBkg[0],
                    new Point(16, 16)));
            }

            //create the current weapon & item menuItems
            currentWeapon = new MenuItem();
            currentItem = new MenuItem();
            itemAmount = new ComponentAmountDisplay(99, -100, -100);

            //get the hero's current weapon and item
            heroWeapon = Pool.hero.weapon;
            heroItem = Pool.hero.item;
            Functions_MenuItem.SetType(heroWeapon, currentWeapon);
            Functions_MenuItem.SetType(heroItem, currentItem);

            //move the weapon bkg & sprite & amount display
            Functions_Component.MoveQuadBkg(weaponBkg, xPos + 8, yPos + 8);
            currentWeapon.compSprite.position.X = xPos + 16;
            currentWeapon.compSprite.position.Y = yPos + 16;

            //move the item bkg & sprite & amount display
            Functions_Component.MoveQuadBkg(itemBkg, xPos + 16 * 8 + 8, yPos + 8);
            currentItem.compSprite.position.X = xPos + 16 * 8 + 16;
            currentItem.compSprite.position.Y = yPos + 16;
            Functions_Component.Align(itemAmount, currentItem.compSprite);

            #endregion


            //create the frametime & autosave text components
            frametime = new ComponentText(Assets.font, "test",
                new Vector2(0, 0), Assets.colorScheme.textLight);
            autosaveText = new ComponentText(Assets.font, "autosaving",
                new Vector2(0, 0), Assets.colorScheme.textLight);

            //place frametime & autosave texts
            frametime.position.X = 16+4+2;
            frametime.position.Y = 32+10+6;
            autosaveText.position.X = 54;
            autosaveText.position.Y = 81;
        }
    }

    public static class DebugInfo
    {
        public static Rectangle background;
        public static List<ComponentText> textFields;
        public static int counter = 0;
        public static int size = 0;

        public static ComponentText timingText;
        public static ComponentText actorText;
        public static ComponentText moveText;
        public static ComponentText poolText;
        public static ComponentText creationText;
        public static ComponentText recordText;
        public static ComponentText musicText;
        //public static ComponentText saveDataText;

        public static long roomTime = 0;
        public static long dungeonTime = 0;
        public static byte framesTotal = 30; //how many frames to average over
        public static byte frameCounter = 0; //increments thru frames 0-framesTotal
        public static long updateTicks; //update tick times are added to this
        public static long drawTicks; //draw tick times are added to this
        public static long updateAvg; //stores the average update ticks
        public static long drawAvg; //stores the average draw ticks

        static DebugInfo()
        {
            textFields = new List<ComponentText>();

            background = new Rectangle(0, 322 - 8, 640, 50);
            int yPos = background.Y - 2;

            timingText = new ComponentText(Assets.font, "",
                new Vector2(2, yPos + 00), Assets.colorScheme.textLight);
            textFields.Add(timingText);

            actorText = new ComponentText(Assets.font, "",
                new Vector2(16 * 3, yPos + 00), Assets.colorScheme.textLight);
            textFields.Add(actorText);

            moveText = new ComponentText(Assets.font, "",
                new Vector2(16 * 7, yPos + 00), Assets.colorScheme.textLight);
            textFields.Add(moveText);

            poolText = new ComponentText(Assets.font, "",
                new Vector2(16 * 12, yPos + 00), Assets.colorScheme.textLight);
            textFields.Add(poolText);

            creationText = new ComponentText(Assets.font, "",
                new Vector2(16 * 17, yPos + 00), Assets.colorScheme.textLight);
            textFields.Add(creationText);

            recordText = new ComponentText(Assets.font, "",
                new Vector2(16 * 21, yPos + 00), Assets.colorScheme.textLight);
            textFields.Add(recordText);

            musicText = new ComponentText(Assets.font, "",
                new Vector2(16 * 26 - 8, yPos + 00), Assets.colorScheme.textLight);
            textFields.Add(musicText);

            //saveDataText = new ComponentText(Assets.font, "",
            //    new Vector2(16 * 30, yPos + 00), Assets.colorScheme.textLight);
            //textFields.Add(saveDataText);
        }
    }

    public static class TopDebugMenu
    {
        public static ComponentSprite cursor;
        public static ObjToolState objToolState;

        public static WidgetDisplaySet display;
        public static Boolean displaySharedObjsWidget = false;

        public static Rectangle rec; //background rec
        public static List<ComponentButton> buttons;
        public static int counter;
        static TopDebugMenu()
        {
            cursor = new ComponentSprite(Assets.uiItemsSheet,
                new Vector2(0, 0),new Byte4(10, 2, 0, 0),new Point(16, 16));

            rec = new Rectangle(0, 0, 640, 13);
            buttons = new List<ComponentButton>();
            buttons.Add(new ComponentButton(
                "f1 draw recs", new Point(2, 2)));
            buttons.Add(new ComponentButton(
                "f2 draw info", new Point(buttons[0].rec.X + buttons[0].rec.Width + 2, 2)));
            buttons.Add(new ComponentButton(
                "f3 hide widgets", new Point(buttons[1].rec.X + buttons[1].rec.Width + 2, 2)));
            buttons.Add(new ComponentButton(
                "f4 -", new Point(buttons[2].rec.X + buttons[2].rec.Width + 2, 2)));
            buttons.Add(new ComponentButton(
                "f5 pause", new Point(buttons[3].rec.X + buttons[3].rec.Width + 2, 2)));

            buttons.Add(new ComponentButton(
                "f6 dung objs", new Point(buttons[4].rec.X + buttons[4].rec.Width + 2, 2)));
            buttons.Add(new ComponentButton(
                "f7 wrld objs", new Point(buttons[5].rec.X + buttons[5].rec.Width + 2, 2)));
            buttons.Add(new ComponentButton(
                "f8 shared objs", new Point(buttons[6].rec.X + buttons[6].rec.Width + 2, 2)));

            buttons.Add(new ComponentButton(
                "room editor", new Point(640 - 104, 2)));
            buttons.Add(new ComponentButton(
                "level editor", new Point(buttons[8].rec.X + buttons[8].rec.Width + 2, 2)));

            display = WidgetDisplaySet.World; //will be overwritten by editor screen
        }
    }





    
    public static class Level
    {
        public static List<Room> rooms = new List<Room>();
        public static List<Door> doors = new List<Door>();
        public static LevelID ID = LevelID.Colliseum;
        public static Boolean bigKey = false;
        public static Boolean map = false;
        public static Boolean lightWorld = true;
        public static Boolean isField = true;
    }

    public class Room
    {
        public Rectangle rec = new Rectangle(0, 0, 0, 0);
        public Boolean visited = false;
        public Byte2 size = new Byte2(0, 0); //in 16 pixel tiles
        public Point center = new Point(0, 0);
        public RoomID roomID;
        public Vector2 spawnPos; //where hero can spawn in room (last door passed thru/exit)
        public PuzzleType puzzleType = PuzzleType.None; //most rooms aren't puzzles
        public int dataIndex; //if dungeon room, what roomData index is used to populate?

        public Room(Point Pos, RoomID ID)
        {
            roomID = ID;
            Functions_Room.SetType(this, ID);
            Functions_Room.MoveRoom(this, Pos.X, Pos.Y);
            //center spawnpos to room
            spawnPos = new Vector2(
                Pos.X + rec.Width / 2, //centered horizontally
                (Pos.Y + rec.Height / 2) + (rec.Height / 2) - 128 - 64 //at bottom
            );
            dataIndex = 0; //0 means no index assigned
        }
    }

    public class Door
    {
        public Rectangle rec = new Rectangle(0, 0, 16, 16);
        public Boolean visited = false;
        public Door(Point Pos) { rec.X = Pos.X; rec.Y = Pos.Y; }
        public DoorType type = DoorType.Open;
    }



    
    


    public class RoomXmlData
    {
        public RoomID type = RoomID.Row;
        public List<ObjXmlData> objs = new List<ObjXmlData>();
    }

    public class ObjXmlData
    {   //placed relative to room's XY pos
        public ObjType type = ObjType.Dungeon_WallStraight;
        public Direction direction = Direction.Down;
        public float posX = 0;
        public float posY = 0;
    }








    //Instanced Classes

    public class Byte2
    {
        public byte X;
        public byte Y;
        public Byte2(byte x, byte y)
        {
            X = x; Y = y;
        }
    }

    public class Byte4
    {   //used for animation
        public byte X; //x frame
        public byte Y; //y frame
        public byte flipHori; //>0 = flip horizontally
        public byte flags; //represents various states (unused)
        public Byte4(byte x, byte y, byte Flip, byte Flags)
        {
            X = x; Y = y;
            flipHori = Flip;
            flags = Flags;
        }
    }

    public class AnimationGroup
    {   //represents an animation with Down, Up, Left, Right states
        public List<Byte4> down;
        public List<Byte4> up;
        public List<Byte4> right;
        public List<Byte4> left;
    }

    public class ActorAnimationList
    {
        public AnimationGroup idle;
        public AnimationGroup move;
        public AnimationGroup idleCarry;
        public AnimationGroup moveCarry;

        public AnimationGroup interact;
        public AnimationGroup dash;
        public AnimationGroup attack;

        public AnimationGroup hit;
        public AnimationGroup death;
        public AnimationGroup reward;
        public AnimationGroup pickupThrow;

        public AnimationGroup grab; //idle
        public AnimationGroup push; //move

        public AnimationGroup swim_idle;
        public AnimationGroup swim_move;
    }

    public class ColorScheme
    {
        //game colors

        //points to a bkg_color based on the level loaded
        public Color background = new Color(0, 0, 0, 255); //refs one below
        public Color bkg_lightWorld = new Color(150, 150, 150, 255);
        public Color bkg_darkWorld = new Color(75, 75, 75, 255);
        public Color bkg_dungeon = new Color(0, 0, 0, 255);

        //covers the screen, fading in/out (usually black)
        public Color overlay = new Color(0, 0, 0, 255);

        public Color windowBkg = new Color(0, 0, 0);
        public Color windowBorder = new Color(210, 210, 210);
        public Color windowInset = new Color(130, 130, 130);
        public Color windowInterior = new Color(156, 156, 156);

        public Color textLight = new Color(255, 255, 255);
        public Color textDark = new Color(0, 0, 0);

        public Color mapNotVisited = new Color(130, 130, 130);
        public Color mapVisited = new Color(70, 70, 70);
        public Color mapBlinker = new Color(255, 255, 255);

        //editor colors
        public Color debugBkg = new Color(20, 20, 20, 255);

        public Color collision = new Color(100, 0, 0, 50);
        public Color interaction = new Color(0, 100, 0, 50);
        public Color roomRec = new Color(0, 0, 100, 50);

        public Color buttonUp = new Color(44, 44, 44);
        public Color buttonOver = new Color(66, 66, 66);
        public Color buttonDown = new Color(100, 100, 100);
    }

    public class Scroll
    {
        public List<ComponentSprite> leftScroll = new List<ComponentSprite>();
        public List<ComponentSprite> rightScroll = new List<ComponentSprite>();
        public List<ComponentSprite> scrollBkg = new List<ComponentSprite>();
        public Point size; //width and height of scroll (in tiles/sprites, not pixels)
        public int i;

        public ComponentText title;
        public Rectangle headerline;
        public DisplayState displayState = DisplayState.Opening;

        public Vector2 startPos;
        public Vector2 endPos;
        public int animSpeed = 8;

        public Scroll(Vector2 StartPos, int Width, int Height)
        {
            startPos = StartPos;
            size.X = Width; size.Y = Height;
            //create the left, right rolled paper edges + center sprites
            Vector2 columnPos = new Vector2();
            columnPos.X = startPos.X;
            columnPos.Y = startPos.Y;
            //create left scroll
            Functions_Scroll.CreateColumn(false, false, 
                size.Y, columnPos, leftScroll);
            //create right scroll
            Functions_Scroll.CreateColumn(false, true, 
                size.Y, columnPos + new Vector2(32, 0), rightScroll);
            //create scroll's background columns, for width of scroll
            for (i = 0; i < size.X; i++)
            {
                columnPos += new Vector2(16, 0);
                Functions_Scroll.CreateColumn(true, false, size.Y, columnPos, scrollBkg);
            }
            //set the endPos
            endPos = new Vector2(startPos.X + ((size.X + 1) * 16), startPos.Y);
            //create title + header line
            title = new ComponentText(Assets.font, "title",
                startPos + new Vector2(20, 6), 
                Assets.colorScheme.textDark);
            headerline = new Rectangle( //Assets.colorScheme.windowInset
                new Point((int)title.position.X + 0, (int)title.position.Y + 14),
                new Point(16 * size.X - 23, 1));
        }
    }


    


    public class Dialog
    {
        public ObjType speaker; //who is speaking
        public String title;
        public String text;
        
        public SoundEffectInstance sfx;
        public Boolean fadeBackgroundIn;
        public Boolean fadeForegroundIn;
        public Boolean exitToOverworld;

        public Dialog(
            ObjType Speaker, 
            String Title, 
            String Text, 
            SoundEffectInstance Sfx, 
            Boolean FadeBkg, Boolean FadeFrg, 
            Boolean ExitToOverworld)
        {
            speaker = Speaker;
            title = Title;
            text = Text;
            sfx = Sfx;
            fadeBackgroundIn = FadeBkg;
            fadeForegroundIn = FadeFrg;
            exitToOverworld = ExitToOverworld;
        }
    }
    


    


    //GameData Classes

    public class Actor
    {
        public ActorType type; //the type of actor this is
        public ActorAI aiType = ActorAI.Basic; //what type of AI this actor gets
        public Boolean enemy = false; //defaults to ally
        public ActorState state; //what actor is doing this frame
        public ActorState inputState; //what input wants actor to do this frame

        public Boolean stateLocked; //can actor change state? actor waits for state to unlock
        public byte lockTotal = 0; //how many frames the actor statelocks for, based on state
        public byte lockCounter = 0; //counts from 0 to lockTotal, then flips stateLocked false

        public ActorAnimationList animList = AnimationFrames.Hero_Animations;
        public AnimationGroup animGroup;
        public Direction direction; //direction actor is facing
        public Boolean active; //does actor input/update/draw?

        //type specific fields, set by Functions_Actor.SetType()
        public float dashSpeed;
        public float walkSpeed;

        //the components that actor requires to function
        public ComponentSprite compSprite;
        public ComponentAnimation compAnim = new ComponentAnimation();
        public ComponentMovement compMove = new ComponentMovement();
        public ComponentCollision compCollision = new ComponentCollision();
        public ComponentInput compInput = new ComponentInput(); //actor specific
        public ComponentSoundFX sfx = new ComponentSoundFX();

        //health points
        public byte health;

        //loadout
        public MenuItemType weapon = MenuItemType.Unknown;
        public MenuItemType item = MenuItemType.Unknown;
        public MenuItemType armor = MenuItemType.Unknown;
        public MenuItemType equipment = MenuItemType.Unknown;

        //actor specific sfx
        public SoundEffectInstance sfxDash;

        public int chaseRadius;
        public int attackRadius;

        //actor fx sprites
        public ComponentSprite feetFX;
        public ComponentAnimation feetAnim;

        //fields used in pickup / carry / throw
        public Boolean carrying = false; //is actor carrying an obj?
        public GameObject heldObj = null; //obj actor might be carrying

        //fields used in grab / push / pull
        public Boolean grabbing = false;
        public GameObject grabbedObj = null;

        public Boolean swimming = false; //is actor in water?
        public Boolean createSplash = false; //handles splash entering water



        public Actor()
        {
            compSprite = new ComponentSprite(Assets.heroSheet, 
                new Vector2(0, 0), new Byte4(0, 0, 0, 0), new Point(16, 16));
            Functions_Actor.SetType(this, ActorType.Hero); //defaults to hero actor
            Functions_Movement.Teleport(compMove, 
                compSprite.position.X, compSprite.position.Y);
            //setup actor fx sprites
            feetAnim = new ComponentAnimation();
            feetAnim.currentAnimation = AnimationFrames.ActorFX_GrassyFeet;
            feetFX = new ComponentSprite(
                Assets.forestLevelSheet,
                new Vector2(100, 100),
                feetAnim.currentAnimation[0], 
                new Point(16, 8));
        }
    }

    public class GameObject
    {
        public ComponentSprite compSprite;
        public ComponentAnimation compAnim = new ComponentAnimation();
        public ComponentMovement compMove = new ComponentMovement();
        public ComponentCollision compCollision = new ComponentCollision();
        public ComponentSoundFX sfx = new ComponentSoundFX();

        public ObjGroup group = ObjGroup.Object;
        public ObjType type = ObjType.Dungeon_WallStraight;
        public Direction direction = Direction.Down; //direction obj/sprite is facing

        public Boolean active = true; //does object draw, update?
        public Boolean getsAI = false; //does object get passed to Functions_AI.HandleObj()?
        public Boolean canBeSaved = false; //can this obj be saved to RoomXMLData?
        public Byte lifetime; //how many frames this object exists for, 0 = forever/ignore
        public Byte lifeCounter; //counts up to lifetime value
        public int interactiveFrame = 0; //only for objs
        //0 = always interactive
        //>0 = just that one # frame of interaction

        public GameObject()
        {   //initialize to default value - data is changed later
            compSprite = new ComponentSprite(Assets.forestLevelSheet, 
                new Vector2(50, 50), new Byte4(0, 0, 0, 0), new Point(16, 16));
            Functions_GameObject.SetType(this, type);
        }
    }

    public class Projectile : GameObject
    {
        public ComponentMovement caster; //actor/obj who created projectile
        public Projectile()
        {   //default to an arrow
            type = ObjType.ProjectileArrow;
        }
    }

    public class GameDisplayData
    {   //this is used by LoadSaveNewScreen to display a game's data visually
        public MenuItem menuItem = new MenuItem();
        public ComponentSprite hero = new ComponentSprite(
            Assets.heroSheet, new Vector2(-100, 1000),
            new Byte4(0, 0, 0, 0), new Point(16, 16));
        public ComponentText timeDateText = new ComponentText(
            Assets.font, "time:/ndate:", new Vector2(-100, 1000),
            Assets.colorScheme.textDark);
        public MenuItem lastStoryItem = new MenuItem();
    }



    //SaveData Classes

    public class SaveData
    {   //data that will be saved/loaded from game session to session
        public DateTime dateTime = DateTime.Now;
        public int hours = 0;
        public int mins = 0;
        public int secs = 0;

        public LevelID lastLocation = LevelID.Colliseum;

        public int enemiesKilled = 0;
        public int damageTaken = 0;

        public ActorType actorType = ActorType.Hero;
        public int gold = 99;
        public byte heartsTotal = 3; //sets max health

        public byte magicCurrent = 3;
        public byte magicMax = 9;

        public byte bombsCurrent = 3;
        public byte bombsMax = 99;

        public byte arrowsCurrent = 10; //testing
        public byte arrowsMax = 99;


        #region Items

        //the hero's last selected / current item
        public MenuItemType currentItem = MenuItemType.Unknown;
        public int lastItemSelected = 0; //index of Widgets.Inventory.menuItems[?]

        //non-magical items
        public Boolean itemBoomerang = false;
        public Boolean itemBow = false;

        //bottles
        public MenuItemType bottleA = MenuItemType.BottleEmpty;
        public MenuItemType bottleB = MenuItemType.BottleEmpty;
        public MenuItemType bottleC = MenuItemType.BottleEmpty;

        //magical items
        public Boolean magicFireball = false;

        #endregion


        #region Weapon

        public byte currentWeapon = 0;
        //0=sword, 2=net
        public Boolean weaponNet = false;

        #endregion


        #region Armor

        public byte currentArmor = 0; //0=tunic, 1=cape
        public Boolean armorCape = false;

        #endregion


        #region Equipment

        public byte currentEquipment = 0;
        //0=ring, 1=pearl, 2=necklace, 3=glove, 4=pin
        public Boolean equipmentRing = true;
        public Boolean equipmentPearl = false;
        public Boolean equipmentNecklace = false;
        public Boolean equipmentGlove = false;
        public Boolean equipmentPin = false;

        #endregion


        //player's pet
        public MenuItemType petType = MenuItemType.Unknown;
    }

 

    //Map Classes

    public class MapLocation
    {
        public ComponentSprite compSprite;
        public Boolean isLevel = false; //is level or connector location?
        public LevelID ID = LevelID.Road; //roads cannot be visited

        //cardinal neighbors this location links with
        public MapLocation neighborUp;
        public MapLocation neighborRight;
        public MapLocation neighborDown;
        public MapLocation neighborLeft;
        public MapLocation(Boolean IsLevel, Vector2 Position)
        {
            isLevel = IsLevel;
            compSprite = new ComponentSprite(Assets.entitiesSheet,
                Position, new Byte4(11, 0, 0, 0), new Point(16, 8));
            //determine if location should use small or large sprite
            if (IsLevel) { compSprite.currentFrame.Y = 1; }
            neighborUp = this; neighborDown = this;
            neighborLeft = this; neighborRight = this;
        }
    }



    //UI Classes

    public class MenuItem
    {
        public ComponentSprite compSprite;
        public ComponentAnimation compAnim = new ComponentAnimation();
        public MenuItemType type;
        public String name = "";
        public String description = "";
        public Byte price = 0;
        //the cardinal neighbors this menuItem links with
        public MenuItem neighborUp;
        public MenuItem neighborDown;
        public MenuItem neighborLeft;
        public MenuItem neighborRight;
        public int id; //menuItems go on a list

        public MenuItem()
        {   //default to ? sprite, hidden offscreen
            compSprite = new ComponentSprite(Assets.uiItemsSheet,
                new Vector2(-100, 1000),
                new Byte4(11, 1, 0, 0),
                new Point(16, 16));
            Functions_MenuItem.SetType(MenuItemType.Unknown, this);
            neighborUp = this; neighborDown = this;
            neighborLeft = this; neighborRight = this;
            id = 0;
        }
    }

    public class MenuRectangle
    {
        public DisplayState displayState;
        public int speedOpen = 5;
        public int speedClose = 5;
        public int animationCounter = 0;    //counts up to delay value
        public int openDelay = 0;           //how many updates are ignored before open animation occurs
        public Rectangle rec = new Rectangle(0, 0, 0, 0);
        public Point position;
        public Point size;
        public Color color;

        public MenuRectangle(Point Position, Point Size, Color Color)
        {
            position = Position; size = Size; color = Color;
            Functions_MenuRectangle.Reset(this);
        }
    }

    public class MenuWindow
    {
        public Point size;
        public ComponentText title;
        public MenuRectangle background;
        public MenuRectangle border;
        public MenuRectangle inset;
        public MenuRectangle interior;
        public List<MenuRectangle> lines;

        public MenuWindow(Point Position, Point Size, String Title)
        {
            size = Size;
            title = new ComponentText(Assets.font, "", new Vector2(0, 0), Assets.colorScheme.textDark);
            //create the window components
            background = new MenuRectangle(
                new Point(0, 0), new Point(0, 0), Assets.colorScheme.windowBkg);
            border = new MenuRectangle(
                new Point(0, 0), new Point(0, 0), Assets.colorScheme.windowBorder);
            inset = new MenuRectangle(
                new Point(0, 0), new Point(0, 0), Assets.colorScheme.windowInset);
            interior = new MenuRectangle(
                new Point(0, 0), new Point(0, 0), Assets.colorScheme.windowInterior);
            lines = new List<MenuRectangle>();
            lines.Add(new MenuRectangle(
                new Point(0, 0), new Point(0, 0), Assets.colorScheme.windowInset)); //header
            lines.Add(new MenuRectangle(
                new Point(0, 0), new Point(0, 0), Assets.colorScheme.windowInset)); //footer
            //set the openDelay to cascade in all the components
            background.openDelay = 0;
            border.openDelay = 2;
            inset.openDelay = 2;
            interior.openDelay = 8;
            lines[0].openDelay = 12; //all lines will use this openDelay value
            //align all the window components
            Functions_MenuWindow.ResetAndMove(this, Position.X, Position.Y, Size, Title);
        }
    }

    public class ScreenRec
    {
        public Rectangle rec = new Rectangle(0, 0, 640, 360);
        public float alpha = 0.0f;
        public float maxAlpha = 1.0f;
        public float fadeInSpeed = 0.05f;
        public float fadeOutSpeed = 0.05f;
        public FadeState fadeState = FadeState.FadeIn;
        public Boolean fade = true;
    }



    //Data Components

    public class ComponentCollision
    {   //allows an object or actor to collide with other objects or actors
        public Rectangle rec = new Rectangle(0, 0, 16, 16); //used in collision checking
        public int offsetX = 0; //offsets rec from sprite.position
        public int offsetY = 0; //offsets rec from sprite.position
        public Boolean blocking = true; //impassable or interactive
    }

    public class ComponentMovement
    {   //allows an object or actor to move, with speed and friction
        public Vector2 position = new Vector2(0, 0); //current position of actor/object
        public Vector2 newPosition = new Vector2(0, 0); //projected position
        public Direction direction = Direction.Down; //the direction actor/obj is moving
        public Vector2 magnitude = new Vector2(0, 0); //how much actor/obj moves each frames
        public Boolean moving = false; //if magnitude isn't 0, this should be true
        public float speed = 0.0f; //controls magnitude
        public float friction = 0.75f; //reduces magnitude each frame
        public Boolean moveable = false; //can be moved by conveyorbelts, if on ground
        public Boolean grounded = true; //on ground or in air?
    }

    public class ComponentAnimation
    {   //allows an object or actor to animate through a sequence of frames on a timer
        public List<Byte4> currentAnimation; //a list of byte4 representing frames of an animation
        public byte index = 0; //where in the currentAnimation list the animation is (animation index)
        public byte speed = 10; //how many frames elapse b4 animation updates (limits animation speed)
        public byte timer = 0; //how many frames elapsed since last animation update (counts frames)
        public Boolean loop = true; //should currentAnimation loop?
    }

    public class ComponentInput
    {   //changes the actor's state + direction, which then sets animation list
        //InputHelper is mapped to this component, AI sets this component's values directly
        public Direction direction = Direction.None;
        public Boolean attack = false;
        public Boolean use = false;
        public Boolean dash = false;
        public Boolean interact = false;
    }

    public class ComponentSprite
    {   //displays a visual representation for an object or actor, or anything else
        public Texture2D texture;
        public Vector2 position;
        public Byte4 currentFrame;
        public Point cellSize;

        public SpriteEffects spriteEffect; //flip vertically, flip horizontally, none
        public Boolean flipHorizontally;
        public Boolean visible;
        public Vector2 origin;
        public Rectangle drawRec;

        public Color drawColor;
        public float alpha;
        public float scale;
        public int zOffset;
        public float zDepth;
        public Rotation rotation;
        public float rotationValue;

        public ComponentSprite(Texture2D Texture, Vector2 Position, Byte4 CurrentFrame, Point CellSize)
        {
            texture = Texture;
            position = Position;
            currentFrame = CurrentFrame;
            cellSize = CellSize;
            spriteEffect = SpriteEffects.None;
            flipHorizontally = false;
            visible = true;
            Functions_Component.CenterOrigin(this);
            drawRec = new Rectangle((int)Position.X, (int)Position.Y, CellSize.X, CellSize.Y);
            drawColor = new Color(255, 255, 255);
            alpha = 1.0f;
            scale = 1.0f;
            zOffset = 0;
            Functions_Component.SetZdepth(this);
            rotation = Rotation.None;
            rotationValue = 0.0f;
        }
    }

    public class ComponentSoundFX
    {   //actors / objs / projectiles can both be hit and killed

        //act/obj is hit, pro hits
        public SoundEffectInstance hit = Assets.sfxEnemyHit;
        //act/obj/pro/pick dies
        public SoundEffectInstance kill = Assets.sfxEnemyKill; 
    }   






    //UI Components

    public class ComponentText
    {
        public SpriteFont font;
        public String text;             //the string of text to draw
        public Vector2 position;        //the position of the text to draw
        public Color color;             //the color of the text to draw
        public float alpha = 1.0f;      //the opacity of the text
        public float rotation = 0.0f;
        public float scale = 1.0f;
        public float zDepth = 0.001f;   //the layer to draw the text to

        public ComponentText(SpriteFont Font, String Text, Vector2 Position, Color Color)
        {
            position = Position;
            text = Text;
            color = Color;
            font = Font;
        }
    }

    public class ComponentButton
    {
        public Rectangle rec;
        public ComponentText compText;
        public int textWidth;
        public Boolean selected;
        public Color currentColor; //points to a button color in color scheme

        public ComponentButton(String Text, Point Position)
        {
            rec = new Rectangle(Position, new Point(0, 9));
            compText = new ComponentText(Assets.font, 
                Text, new Vector2(0, 0), Assets.colorScheme.textLight);
            selected = false;
            currentColor = Assets.colorScheme.buttonUp;
            Functions_Component.CenterText(this);
        }
    }

    public class ComponentAmountDisplay
    {   //displays a 2 digit amount against a black background
        public ComponentText amount;
        public Rectangle bkg;
        public Boolean visible;
        public ComponentAmountDisplay(int Amount, int X, int Y)
        {
            amount = new ComponentText(Assets.font, "" + Amount,
                new Vector2(X, Y), Assets.colorScheme.textLight);
            bkg = new Rectangle(new Point(X, Y), new Point(9, 7));
            visible = true;
        }
    }

}