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
    {   
        // **********************************************************************************************************
        public static Boolean Release = false; //puts game in release mode, overwrites other flags
        // **********************************************************************************************************
        public static float Version = 0.78f; //the version of the game
        public static BootRoutine bootRoutine = BootRoutine.Editor_Level; //boot to game or editor?

        //dev/editor flags
        public static Boolean EnableTopMenu = true; //enables the top debug menu (draw + input)
        public static Boolean EnableDebugInfo = false; //rightside debug info
        public static Boolean SpawnMobs = true; //toggles the spawning of lesser enemies (not bosses)
        public static Boolean ShowEnemySpawns = false; //create & draw enemySpawn gameObjects?
        public static Boolean PrintOutput = true; //print output to the debugger
        public static Boolean IgnoreWaterTiles = false; //enable/disable pickup/selection/deletion of waterTiles
        public static Boolean IgnoreBoatTiles = false; //enable/disable pickup/selection/deletion of boatTiles
        public static Boolean Paused = false; //controlled by topMenu 'play/pause' button

        //level control flags
        public static Boolean ProcessAI = true; //apply AI input to enemies / actors

        //option flags
        public static Boolean DrawInput = false; //draw the input display
        public static Boolean DrawUItime = false; //draw build times next to world ui?
        public static Boolean DrawCollisions = false; //draw/hide collision rec components
        public static Boolean PlayMusic = false; //turns music on/off
        public static Boolean PlaySoundFX = true; //turns soundfx on/off
        public static Boolean CameraTracksHero = false; //camera tracks hero or centers to dungeon room
        public static Boolean IgnoreRoofTiles = false; //enable/disable pickup/selection of roofTiles
        public static Boolean HardMode = false; //forces dun.room puzzle setup
        public static Boolean DrawWatermark = true; //top right link (for capturing purposes)
        public static Boolean Gore = true; //blood, guts, skeletons upon enemy death

        //cheat flags
        public static Boolean Invincibility = true; //does hero ignore damage?
        public static Boolean InfiniteMagic = true; //does hero ignore magic costs?
        public static Boolean InfiniteGold = true; //does hero ignore item/vendor costs?
        public static Boolean InfiniteArrows = true; //does hero ignore arrow cost?
        public static Boolean InfiniteBombs = true; //does hero ignore bomb cost?
        public static Boolean MapCheat = false; //sets dungeon.map true when dungeon is built
        public static Boolean KeyCheat = false; //sets dungeon.key true when dungeon is built
        public static Boolean UnlockAll = false; //unlocks all items for hero
        public static Boolean Clipping = false; //removes hero from collision/interaction/exit routines
        public static Boolean FuzzyInput = false; //fuzz controller input each frame (for testing) 
        public static Boolean InfiniteFairies = false; //hero always has fairy in a bottle
        public static Boolean AutoSolvePuzzle = false; //solve dun.room puzzles (torches + switches)










        static Flags()
        {

            #region Set RELEASE Mode

            if (Release)
            {
                bootRoutine = BootRoutine.Game;
                //set flags for release mode
                EnableTopMenu = false;
                EnableDebugInfo = false;
                DrawUItime = false;
                DrawCollisions = false;
                DrawInput = false;
                DrawWatermark = true;

                Paused = false;
                PlayMusic = true;
                PlaySoundFX = true;
                SpawnMobs = true;
                ProcessAI = true;
                CameraTracksHero = false;
                ShowEnemySpawns = false;
                PrintOutput = false;

                HardMode = false;

                //cheats
                Invincibility = false;
                InfiniteMagic = false;
                InfiniteGold = false;
                InfiniteArrows = false;
                InfiniteBombs = false;
                MapCheat = false;
                KeyCheat = false;
                UnlockAll = false;
                Clipping = false;
                FuzzyInput = false;
                InfiniteFairies = false;
            }

            #endregion

            else
            {
                if (bootRoutine != BootRoutine.Game)
                {
                    //set dev mode cheats
                    Invincibility = true; //hero cannot die in editor
                    InfiniteMagic = true; //hero has infinite magic
                    InfiniteGold = true; //hero has infinite gold
                    InfiniteArrows = true; //hero has infinite arrows
                    InfiniteBombs = true; //hero has infinite bombs

                    //handle editor cheats
                    EnableTopMenu = true; //necessary
                    ShowEnemySpawns = true; //necessary for editing
                }
            }
        }

    }



    #region Color Scheme

    public static class ColorScheme
    {
        //points to a bkg_color based on the level loaded
        public static Color background = new Color(0, 0, 0, 255); //refs one below

        public static Color bkg_overworld = new Color(206, 199, 185, 255); //cloud gray
        public static Color bkg_level_room = new Color(88, 127, 62, 255); //grass green


        //dungeons now have their own bkg colors
        public static Color bkg_dungeon_forest = new Color(25, 53, 25, 255); //darkest green
        public static Color bkg_dungeon_mountain = new Color(44, 38, 25, 255); //darkest brown
        public static Color bkg_dungeon_swamp = new Color(88, 127, 62, 255); //slime green
        





        //covers the screen, fading in/out (usually black)
        public static Color overlay = new Color(0, 0, 0, 255);

        public static Color windowBkg = new Color(44, 38, 25); //0.0.0.
        public static Color windowBorder = new Color(236, 224, 198); //250?
        public static Color windowInset = new Color(166, 143, 104); //130
        public static Color windowInterior = new Color(188, 176, 151); //156

        public static Color textLight = new Color(255, 255, 255);
        public static Color textDark = new Color(0, 0, 0);

        public static Color mapNotVisited = new Color(118, 102, 62);
        public static Color mapVisited = new Color(69, 58, 31);
        public static Color mapBlinker = new Color(236, 224, 198);


        //editor colors - dont touch
        public static Color debugBkg = new Color(20, 20, 20, 255); 

        public static Color collision = new Color(100, 0, 0, 50);
        public static Color interaction = new Color(0, 100, 0, 50);
        public static Color roomRec = new Color(0, 0, 100, 50);

        public static Color buttonUp = new Color(44, 44, 44);
        public static Color buttonOver = new Color(66, 66, 66);
        public static Color buttonDown = new Color(100, 100, 100);
    }

    #endregion





    #region Save data, level data, room data, door data


    public class SaveData
    {   //data that will be saved/loaded from game session to session
        public DateTime dateTime = DateTime.Now;
        public int hours = 0;
        public int mins = 0;
        public int secs = 0;

        public LevelID lastLocation = LevelID.SkullIsland_Colliseum;


        public ActorType actorType = ActorType.Hero;
        public int gold = 99;
        public byte heartsTotal = 3; //sets max health

        public byte magicCurrent = 3;
        public byte magicMax = 9;

        public byte bombsCurrent = 3;
        public byte bombsMax = 99;

        public byte arrowsCurrent = 10; //testing
        public byte arrowsMax = 99;



        //fun random tracking data
        public int enemiesKilled = 0;
        public int damageTaken = 0;
        public int recorded_wallJumps = 0;


        //player's pet
        public MenuItemType petType = MenuItemType.Unknown;




        //the hero's last selected / current item
        public MenuItemType currentItem = MenuItemType.ItemBoomerang;
        public int lastItemSelected = 0; //index of Widgets.Inventory.menuItems[?]

        public Boolean itemBoomerang = true;
        public Boolean itemBow = false;
        public Boolean itemFirerod = false;

        public Boolean magicBombos = false;
        public Boolean magicBolt = false;

        //bottles - hero just always has 3 bottles
        public MenuItemType bottleA = MenuItemType.BottleEmpty;
        public MenuItemType bottleB = MenuItemType.BottleEmpty;
        public MenuItemType bottleC = MenuItemType.BottleEmpty;

        //Weapon
        public MenuItemType currentWeapon = MenuItemType.WeaponSword;
        //public Boolean weaponSword = false; //not used rn
        public Boolean weaponNet = false;
        public Boolean weaponShovel = false;
        public Boolean weaponHammer = false;

        //Armor
        public MenuItemType currentArmor = MenuItemType.ArmorCloth;
        //public Boolean armorCloth = false; //not used rn
        public Boolean armorCape = false; //cosmetic only rn, no effect

        //Equipment
        public MenuItemType currentEquipment = MenuItemType.Unknown;
        public Boolean equipmentRing = true;
        public Boolean equipmentPearl = false;
        public Boolean equipmentNecklace = false;
        public Boolean equipmentGlove = false;
        public Boolean equipmentPin = false;

        //setup default enemy items
        public MenuItemType enemyItem = MenuItemType.Unknown;
        public MenuItemType enemyWeapon = MenuItemType.Unknown;


        //story booleans
        public Boolean story_forestDungeon = false;
        public Boolean story_mountainDungeon = false;
        public Boolean story_swampDungeon = false;

    }



    public static class LevelSet
    {
        public static Level field;
        public static Level dungeon;
        //current level points to field or dungeon
        public static Level currentLevel;

        //used to spawn hero in field or dungeon rooms
        public static Vector2 spawnPos_Field = new Vector2();
        //^ canBe: centered + south, or based on entrance roomObj
        public static Vector2 spawnPos_Dungeon = new Vector2();
        //^ entirely based on connecting doors and exit door in dungeon


        static LevelSet()
        {
            field = new Level();
            dungeon = new Level();
            currentLevel = field;
        }
    }

    public class Level
    {
        public LevelID ID = LevelID.SkullIsland_Colliseum;
        public List<Room> rooms = new List<Room>();
        public Room currentRoom; //points to one in list above
        public List<Door> doors = new List<Door>();

        public Boolean bigKey = false;
        public Boolean map = false;
        public Boolean isField = true;
        public int dungeonTrack = 0; //what music plays
    }




    public class Room
    {
        public Rectangle rec = new Rectangle(0, 0, 0, 0);
        public Boolean visited = false;
        public Byte2 size = new Byte2(0, 0); //in 16 pixel tiles
        public Point center = new Point(0, 0);
        public RoomID roomID;
        public LevelID levelID = LevelID.Forest_Dungeon;
        public PuzzleType puzzleType = PuzzleType.None; //most rooms aren't puzzles
        public int dataIndex; //if dungeon room, what roomData index is used to populate?

        public Room(Point Pos, RoomID ID)
        {
            roomID = ID;
            Functions_Room.SetType(this, ID);
            Functions_Room.MoveRoom(this, Pos.X, Pos.Y);
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



    #endregion



    #region Globals

    public static class World
    {
        public static float frictionAttack = 0.1f; //actor in attacking state
        public static float frictionUse = 0.5f; //actor in use state

        public static float friction = 0.75f; //standard friction

        public static float frictionWater = 0.85f; //some slowdown
        public static float frictionAir = 0.9f; //some slowdown
        public static float frictionIce = 0.99f; //no slowdown


        //floors are placed on the last layer, all others draw above this
        public static float floorLayer = 0.999990f;
        public static float waterLayer = 0.999989f;

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

        public static void Clear()
        {
            dungeonID = 0;
            beatDungeon = false;
            enemyCount = 0;
            totalDamage = 0;
            timer.Reset();
        }
    }

    

    //used to contain the global inputs for player1, 2
    public static class Input
    {
        //there can only be 1 keyboard input at a time
        public static KeyboardState currentKeyboardState = new KeyboardState();
        public static KeyboardState lastKeyboardState = new KeyboardState();
        public static MouseState currentMouseState = new MouseState();
        public static MouseState lastMouseState = new MouseState();


        //but, there can be multiple gamepad inputs at a time


        #region Player 1 input instances

        public static GameInput Player1 = new GameInput();
        public static GamePadState currentGamePadState_1 = new GamePadState();
        public static Direction gamePadDirection_1 = Direction.None;

        #endregion


        #region Player 2 input instances

        //player 2 input instances
        public static GameInput Player2 = new GameInput();
        public static GamePadState currentGamePadState_2 = new GamePadState();
        public static Direction gamePadDirection_2 = Direction.None;

        #endregion


        //the amount of joystick movement classified as noise
        public static float deadzone = 0.20f;
        //used to visually display a hand icon as cursor in editor
        public static Point cursorPos = new Point(0, 0);
        public static ComponentCollision cursorColl = new ComponentCollision();

        static Input()
        {
            cursorColl.rec.Width = 4;
            cursorColl.rec.Height = 4;
            cursorColl.blocking = false;
        }
    }





    public static class WaterMark
    {
        public static DebugDisplay display;

        static WaterMark()
        {   //remodel debugDisplay to be a single line
            display = new DebugDisplay(495+31, 20);
            display.bkg.size.X = 118-31;
            display.bkg.size.Y = 9;

            display.textComp.text = "github.com/mrgrak/tsk";
            display.textComp.position.X = display.bkg.position.X + 2;
            display.textComp.position.Y = display.bkg.position.Y - 3;

            Functions_MenuRectangle.Reset(display.bkg);
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

        public static void ReadInput()
        {

            #region Directions

            //reset all to unselected
            directions[0].currentFrame = AnimationFrames.Input_Arrow_Unselected[0];
            directions[1].currentFrame = AnimationFrames.Input_Arrow_Unselected[0];
            directions[2].currentFrame = AnimationFrames.Input_Arrow_Unselected[0];
            directions[3].currentFrame = AnimationFrames.Input_Arrow_Unselected[0];

            //directions = up, right, down, left

            //handle cardinals
            if (Input.Player1.direction == Direction.Up)
            { directions[0].currentFrame = AnimationFrames.Input_Arrow_Selected[0]; }
            else if (Input.Player1.direction == Direction.Right)
            { directions[1].currentFrame = AnimationFrames.Input_Arrow_Selected[0]; }
            else if (Input.Player1.direction == Direction.Down)
            { directions[2].currentFrame = AnimationFrames.Input_Arrow_Selected[0]; }
            else if (Input.Player1.direction == Direction.Left)
            { directions[3].currentFrame = AnimationFrames.Input_Arrow_Selected[0]; }

            //handle dialgonals
            else if (Input.Player1.direction == Direction.UpRight)
            {
                directions[0].currentFrame = AnimationFrames.Input_Arrow_Selected[0];
                directions[1].currentFrame = AnimationFrames.Input_Arrow_Selected[0];
            }
            else if (Input.Player1.direction == Direction.UpLeft)
            {
                directions[0].currentFrame = AnimationFrames.Input_Arrow_Selected[0];
                directions[3].currentFrame = AnimationFrames.Input_Arrow_Selected[0];
            }

            else if (Input.Player1.direction == Direction.DownRight)
            {
                directions[2].currentFrame = AnimationFrames.Input_Arrow_Selected[0];
                directions[1].currentFrame = AnimationFrames.Input_Arrow_Selected[0];
            }
            else if (Input.Player1.direction == Direction.DownLeft)
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
            if(Input.Player1.A || Input.Player1.A_Prev)
            { buttons[0].currentFrame = AnimationFrames.Input_ButtonA_Selected[0]; }

            if (Input.Player1.X || Input.Player1.X_Prev)
            { buttons[1].currentFrame = AnimationFrames.Input_ButtonX_Selected[0]; }

            if (Input.Player1.Y || Input.Player1.Y_Prev)
            { buttons[2].currentFrame = AnimationFrames.Input_ButtonY_Selected[0]; }

            if (Input.Player1.B || Input.Player1.B_Prev)
            { buttons[3].currentFrame = AnimationFrames.Input_ButtonB_Selected[0]; }

            if (Input.Player1.Start || Input.Player1.Start_Prev)
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
                new Vector2(0, 0), ColorScheme.textLight);
            autosaveText = new ComponentText(Assets.font, "autosaving",
                new Vector2(0, 0), ColorScheme.textLight);

            //place frametime & autosave texts
            frametime.position.X = 16+4+2;
            frametime.position.Y = 32+10+6;
            autosaveText.position.X = 54;
            autosaveText.position.Y = 81;
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

    public static class TopDebugMenu
    {
        public static ComponentSprite cursor;
        public static ComponentSprite toolTipSprite;

        public static ObjToolState objToolState;
        public static Boolean hideAll = false;

        public static Rectangle rec; //background rec
        public static List<ComponentButton> buttons;
        public static int counter;


        public static DebugDisplay DebugDisplay_Ram;
        public static DebugDisplay DebugDisplay_HeroState;
        public static DebugDisplay DebugDisplay_Movement;
        public static DebugDisplay DebugDisplay_PoolCounter;
        public static DebugDisplay DebugDisplay_BuildTimes;
        public static DebugDisplay DebugDisplay_Collisions;

        static TopDebugMenu()
        {
            cursor = new ComponentSprite(
                Assets.uiItemsSheet,
                new Vector2(0, 0),
                new Byte4(10, 2, 0, 0),
                new Point(16, 16));

            toolTipSprite = new ComponentSprite(
                Assets.uiItemsSheet,
                new Vector2(0, 0),
                new Byte4(10, 4, 0, 0),
                new Point(16, 16));

            rec = new Rectangle(0, 0, 640, 13);

            buttons = new List<ComponentButton>();



            buttons.Add(new ComponentButton(
                "backspace = menu", new Point(2, 2)));

            buttons.Add(new ComponentButton(
                "f1 draw recs", new Point(buttons[0].rec.X + buttons[0].rec.Width + 2, 2)));
            buttons.Add(new ComponentButton(
                "f2 draw info", new Point(buttons[1].rec.X + buttons[1].rec.Width + 2, 2)));
            buttons.Add(new ComponentButton(
                "f3 hide widgets", new Point(buttons[2].rec.X + buttons[2].rec.Width + 2, 2)));

            buttons.Add(new ComponentButton(
                "f4 compile xml", new Point(buttons[3].rec.X + buttons[3].rec.Width + 2, 2)));
            buttons.Add(new ComponentButton(
                "f5 pause", new Point(buttons[4].rec.X + buttons[4].rec.Width + 2, 2)));
            buttons.Add(new ComponentButton(
                "f6 heroType++", new Point(buttons[5].rec.X + buttons[5].rec.Width + 2, 2)));





            #region Setup Debug Displays

            int Xpos = 565;

            DebugDisplay_Ram = new DebugDisplay(Xpos, 16*3 - 16);
            DebugDisplay_HeroState = new DebugDisplay(Xpos, DebugDisplay_Ram.bkg.position.Y + 16 * 3 + 4);
            DebugDisplay_Movement = new DebugDisplay(Xpos, DebugDisplay_HeroState.bkg.position.Y + 16 * 3 + 4);
            DebugDisplay_PoolCounter = new DebugDisplay(Xpos, DebugDisplay_Movement.bkg.position.Y + 16 * 3 + 4);
            DebugDisplay_BuildTimes = new DebugDisplay(Xpos, DebugDisplay_PoolCounter.bkg.position.Y + 16 * 3 + 4);
            DebugDisplay_Collisions = new DebugDisplay(Xpos, DebugDisplay_BuildTimes.bkg.position.Y + 16 * 3 + 4);

            #endregion


        }
    }



    #endregion



    #region Instanced Classes



    //models player input, used for player 1 and 2
    public class GameInput
    {   //direction values
        public Direction direction = Direction.None;
        public Direction direction_Prev = Direction.None;

        //button booleans
        public Boolean A = false;
        public Boolean X = false;
        public Boolean Y = false;
        public Boolean B = false;

        public Boolean A_Prev = false;
        public Boolean X_Prev = false;
        public Boolean Y_Prev = false;
        public Boolean B_Prev = false;

        //start booleans
        public Boolean Start = false;
        public Boolean Start_Prev = false;
    }

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
        public AnimationGroup reward;
        public AnimationGroup pickupThrow;

        public AnimationGroup death_heroic; //spin twice, fall (on land)
        public AnimationGroup death_heroic_water; //spin twice, (drown in water)
        public AnimationGroup death_blank; //literally empty, 'disappear' from screen

        public AnimationGroup grab; //idle
        public AnimationGroup push; //move

        public AnimationGroup swim_idle;
        public AnimationGroup swim_move;
        public AnimationGroup swim_dash;
        public AnimationGroup swim_hit;
        public AnimationGroup swim_reward;

        public AnimationGroup underwater_idle;
        public AnimationGroup underwater_move;

        public AnimationGroup falling;
        public AnimationGroup landed;
        
        public AnimationGroup climbing;
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
                ColorScheme.textDark);
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

        public Dialog(
            ObjType Speaker, 
            String Title, 
            String Text, 
            SoundEffectInstance Sfx, 
            Boolean FadeBkg, Boolean FadeFrg)
        {
            speaker = Speaker;
            title = Title;
            text = Text;
            sfx = Sfx;
            fadeBackgroundIn = FadeBkg;
            fadeForegroundIn = FadeFrg;
        }
    }

    public class DebugDisplay
    {
        public MenuRectangle bkg;
        public ComponentText textComp;

        public DebugDisplay(int X, int Y)
        {
            bkg = new MenuRectangle(
                new Point(X, Y),
                new Point(16 * 3, 16 * 3),
                ColorScheme.windowBkg);
            textComp = new ComponentText(Assets.font,
                "test\ntest\ntest\ntest\ntest",
                new Vector2(bkg.position.X + 2, bkg.position.Y - 2),
                ColorScheme.textLight);
        }
    }

    #endregion







    #region The Pool

    public static class Pool
    {
        //actor pool handles all actors in the room including hero
        public static int actorCount = 30;           //total count of actors in pool
        public static List<Actor> actorPool = new List<Actor>();//the actual list of actors
        public static int actorIndex;           //used to iterate thru the pool
        public static int actorCounter = 0;

        //obj pool handles room objects, from dungeon & main sheet
        public static int roomObjCount = 3000;
        public static List<GameObject> roomObjPool = new List<GameObject>();
        public static int roomObjIndex;
        public static int roomObjCounter = 0;


        //floor pool - dungeon sheet only
        public static int floorCount = 500;
        public static List<ComponentSprite> floorPool = new List<ComponentSprite>();
        public static int floorIndex;
        public static int floorCounter = 0;

        //lines (for overworld map mostly)
        public static int lineCount = 25;
        public static List<Line> linePool = new List<Line>();
        public static int lineCounter = 0;




        //projectile pool
        public static int projectileCount = 300;
        public static List<Projectile> projectilePool = new List<Projectile>();
        public static int projectileIndex;
        public static int projectileCounter = 0;

        //pickups are modeled as projectiles - we can consolidate this into proPool above later
        public static int pickupCount = 50;
        public static List<Pickup> pickupPool = new List<Pickup>();
        public static int pickupIndex;
        public static int pickupCounter = 0;








        //particle pool
        public static int particleCount = 750;
        public static List<Particle> particlePool = new List<Particle>();
        public static int particleIndex;
        public static int particleCounter = 0;





        public static int activeActor = 1; //tracks the current actor being handled by AI
        public static Actor hero; //points to actorPool[0]
        public static GameObject herosPet; //points to roomObj[1]

        public static int collisionsCount = 0; //tracks collisions per frame
        public static int interactionsCount = 0; //tracks interactions per frame


        public static void Initialize()
        {
            //actor pool
            for (actorCounter = 0; actorCounter < actorCount; actorCounter++)
            {
                actorPool.Add(new Actor());
                Functions_Actor.SetType(actorPool[actorCounter], ActorType.Hero);
                Functions_Movement.Teleport(actorPool[actorCounter].compMove, -100, -100);
                actorPool[actorCounter].active = false;
            }
            actorIndex = 1;

            //room obj pool
            for (roomObjCounter = 0; roomObjCounter < roomObjCount; roomObjCounter++)
            { roomObjPool.Add(new GameObject()); }
            roomObjIndex = 1;

            //particle pool
            for (particleCounter = 0; particleCounter < particleCount; particleCounter++)
            { particlePool.Add(new Particle()); }
            particleIndex = 0;

            //projectile pool
            for (projectileCounter = 0; projectileCounter < projectileCount; projectileCounter++)
            { projectilePool.Add(new Projectile()); }
            projectileIndex = 0;

            //pickup pool
            for (pickupCounter = 0; pickupCounter < pickupCount; pickupCounter++)
            { pickupPool.Add(new Pickup()); }
            pickupIndex = 0;

            //floor pool
            for (floorCounter = 0; floorCounter < floorCount; floorCounter++)
            {
                floorPool.Add(new ComponentSprite(Assets.Dungeon_DefaultSheet,
                    new Vector2(0, 0),
                    new Byte4(15, 0, 0, 0),
                    new Point(16, 16)));
            }
            floorIndex = 0;

            //line pool
            for (lineCounter = 0; lineCounter < lineCount; lineCounter++)
            {
                linePool.Add(new Line()); //recycled later
            }



            //reset all pools
            Functions_Pool.Reset();
            //create easy to remember reference for hero & pet
            hero = actorPool[0];
            herosPet = roomObjPool[0];
            //setup hero and pet
            Functions_Actor.SetType(hero, ActorType.Hero);
            Functions_GameObject.SetType(herosPet, ObjType.Pet_Dog);
        }

    }

    #endregion


    #region Actor, GameObject, Projectile, Particle, Pickup

    public class Actor
    {
        public ActorType type; //the type of actor this is

        public ActorAI aiType = ActorAI.Basic; //what type of AI this actor gets
        public Boolean chaseDiagonally = true; //most actors chase diagonally to slide around blocking objs
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
        public float swimSpeed = 0.10f; //unchanged

        //components actor requires to function
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

        //fields used in swimming
        public Boolean swimming = false; //is actor in water?
        public Boolean createSplash = false; //handles splash entering/exiting water
        public Boolean underwater = false; //is actor's head underwater or above water?
        public int breathCounter = 0; //counts frames while actor is underwater, forces surface
        public int breathTotal = 60 * 4; //in frames, defaults to 4 seconds
        public Boolean underwaterEnemy = false;



        public Actor()
        {
            //setup sprite component
            compSprite = new ComponentSprite(Assets.heroSheet, 
                new Vector2(0, 0), new Byte4(0, 0, 0, 0), new Point(16, 16));

            //setup actor fx sprites
            feetAnim = new ComponentAnimation();
            feetAnim.currentAnimation = AnimationFrames.ActorFX_GrassyFeet;
            feetFX = new ComponentSprite(
                Assets.CommonObjsSheet,
                new Vector2(100, 100),
                feetAnim.currentAnimation[0],
                new Point(16, 8));

            //reset actor to running state
            Functions_Actor.ResetActor(this);

            //setup initial values
            animGroup = animList.idle;
            compAnim.currentAnimation = animGroup.down;
            direction = Direction.Down;
            state = ActorState.Idle;

            Functions_Actor.SetType(this, ActorType.Hero);
            Functions_Movement.Teleport(compMove, 
                compSprite.position.X, compSprite.position.Y);
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
        public Boolean canBeSaved = false; //can this obj be saved to RoomXMLData?
        public Byte lifetime; //how many frames this object exists for, 0 = forever/ignore
        public Byte lifeCounter; //counts up to lifetime value

        public Boolean getsAI = false; //obj goes to Functions_AI.HandleObj()
        public int interactiveFrame = 0; 
        //0 = always interactive, >0 = just that one # frame of interaction in ai.handleObj()

        public Boolean underWater = false; //is obj underwater
        public Boolean inWater = false; //is obj partially submerged in water? ex: swimming

        public GameObject()
        {   //initialize to default value - data is changed later
            compSprite = new ComponentSprite(Assets.CommonObjsSheet, 
                new Vector2(50, 50), new Byte4(0, 0, 0, 0), new Point(16, 16));
            Functions_GameObject.SetType(this, type);
        }
    }

    public class Projectile
    {
        public ComponentSprite compSprite;
        public ComponentAnimation compAnim = new ComponentAnimation();
        public ComponentMovement compMove = new ComponentMovement();
        public ComponentCollision compCollision = new ComponentCollision();
        public ComponentSoundFX sfx = new ComponentSoundFX();

        public Boolean active = true; //does object draw, update?

        public Actor caster = Pool.hero; //default caster to hero
        public ProjectileType type = ProjectileType.Arrow;
        public Direction direction = Direction.Down; //direction sprite is facing

        public Byte lifetime = 30; //how many frames this object exists for, 0 = forever/ignore
        public Byte lifeCounter = 0; //counts up to lifetime value

        public int interactiveFrame = 0;
        //0 = always interactive, >0 = just that one # frame of interaction

        public Projectile()
        {   //initialize to default value - data is changed later
            compSprite = new ComponentSprite(Assets.entitiesSheet,
                new Vector2(50, 50), new Byte4(0, 0, 0, 0), new Point(16, 16));
            Functions_Projectile.SetType(this, type);
        }
    }

    public class Particle
    {
        public ComponentSprite compSprite;
        public ComponentAnimation compAnim = new ComponentAnimation();
        public ComponentMovement compMove = new ComponentMovement();

        public Boolean active = true; //does object draw, update?
        public ParticleType type = ParticleType.Attention;
        public Direction direction = Direction.Down; //direction sprite is facing

        public Byte lifetime = 30; //how many frames this object exists for, 0 = forever/ignore
        public Byte lifeCounter = 0; //counts up to lifetime value

        public Particle()
        {   //initialize to default value - data is changed later
            compSprite = new ComponentSprite(Assets.entitiesSheet,
                new Vector2(50, 50), new Byte4(0, 0, 0, 0), new Point(16, 16));
            Functions_Particle.SetType(this, type);
        }
    }

    public class Pickup
    {
        public ComponentSprite compSprite;
        public ComponentAnimation compAnim = new ComponentAnimation();
        public ComponentCollision compCollision = new ComponentCollision();
        public ComponentSoundFX sfx = new ComponentSoundFX();

        public Boolean active = true; //does object draw, update?

        public Actor caster = Pool.hero; //default caster to hero
        public PickupType type = PickupType.Rupee;
        public Direction direction = Direction.Down; //direction sprite is facing

        public Byte lifetime = 30; //how many frames this object exists for, 0 = forever/ignore
        public Byte lifeCounter = 0; //counts up to lifetime value

        public Pickup()
        {   //initialize to default value - data is changed later
            compSprite = new ComponentSprite(Assets.entitiesSheet,
                new Vector2(50, 50), new Byte4(0, 0, 0, 0), new Point(16, 16));
            Functions_Pickup.SetType(this, type);
        }
    }


    #endregion













    #region UI Classes


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
            title = new ComponentText(Assets.font, "", new Vector2(0, 0), ColorScheme.textDark);
            //create the window components
            background = new MenuRectangle(
                new Point(0, 0), new Point(0, 0), ColorScheme.windowBkg);
            border = new MenuRectangle(
                new Point(0, 0), new Point(0, 0), ColorScheme.windowBorder);
            inset = new MenuRectangle(
                new Point(0, 0), new Point(0, 0), ColorScheme.windowInset);
            interior = new MenuRectangle(
                new Point(0, 0), new Point(0, 0), ColorScheme.windowInterior);
            lines = new List<MenuRectangle>();
            lines.Add(new MenuRectangle(
                new Point(0, 0), new Point(0, 0), ColorScheme.windowInset)); //header
            lines.Add(new MenuRectangle(
                new Point(0, 0), new Point(0, 0), ColorScheme.windowInset)); //footer
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

    public class GameDisplayData
    {   //this is used by LoadSaveNewScreen to display a game's data visually
        public MenuItem menuItem = new MenuItem();
        public ComponentSprite hero = new ComponentSprite(
            Assets.heroSheet, new Vector2(-100, 1000),
            new Byte4(0, 0, 0, 0), new Point(16, 16));
        public ComponentText timeDateText = new ComponentText(
            Assets.font, "time:/ndate:", new Vector2(-100, 1000),
            ColorScheme.textDark);
        public MenuItem lastStoryItem = new MenuItem();
    }


    #endregion



    #region COMPONENTS - collosion, movement, animation, input, sprite, soundfx


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
            spriteEffect = SpriteEffects.None;
            flipHorizontally = false;
            visible = true;
            drawRec = new Rectangle((int)Position.X, (int)Position.Y, CellSize.X, CellSize.Y);
            Functions_Component.CenterOrigin(this);
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


    #endregion



    #region UI COMPONENTS - text, button, amountDisplay

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
                Text, new Vector2(0, 0), ColorScheme.textLight);
            selected = false;
            currentColor = ColorScheme.buttonUp;
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
                new Vector2(X, Y), ColorScheme.textLight);
            bkg = new Rectangle(new Point(X, Y), new Point(9, 7));
            visible = true;
        }
    }

    #endregion



    //misc
    public class MapLocation
    {
        public ComponentSprite compSprite;
        public Boolean isLevel = false; //is level or connector location?
        public LevelID ID = LevelID.Road; //roads cannot be visited
        public string name = "default";

        //cardinal neighbors this location links with
        public MapLocation neighborUp;
        public MapLocation neighborRight;
        public MapLocation neighborDown;
        public MapLocation neighborLeft;
        public MapLocation(Boolean IsLevel, Vector2 Position, string Name)
        {
            isLevel = IsLevel;
            compSprite = new ComponentSprite(Assets.entitiesSheet,
                Position, new Byte4(11, 0, 0, 0), new Point(16, 8));
            compSprite.zOffset = -32; //always sort under hero
            //determine if location should use small or large sprite
            if (IsLevel) { compSprite.currentFrame.Y = 1; }
            neighborUp = this; neighborDown = this;
            neighborLeft = this; neighborRight = this;
            name = Name;
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

    

    public class Line
    {
        //this is a single pixel line drawn from posA to posB,
        //using a color - added to a list of lines, used in a pool
        //based approach

        public int startPosX = 0;
        public int startPosY = 0;

        public int endPosX = 0;
        public int endPosY = 0;

        public float angle = 0.0f;
        public int length = 0;
        public Boolean visible = false;
        public float zDepth = 0.0f; //modeled same as sprite components zDepth

        public Rectangle rec = new Rectangle(0, 0, 1, 1); //this is drawn

        public Texture2D texture = Assets.lineTexture; //never changes
        public Rectangle texRec = new Rectangle(0, 0, 16, 16);
        public Vector2 texOrigin = new Vector2(0, 0);
    }
}