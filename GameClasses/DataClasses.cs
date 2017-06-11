﻿using System;
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
using System.IO;
using System.Xml.Serialization;

namespace DungeonRun
{
    //Global Classes

    public static class Flags
    {   // **********************************************************************************************************
        public static Boolean Release = false; //puts game in release mode, overwrites other flags
        // **********************************************************************************************************

        public static Boolean Debug = true; //draw/enable debugging info/menu/dev input
        public static Boolean DrawCollisions = false; //draw/hide collision rec components
        public static Boolean Paused = false; //this shouldn't be changed here, it's controlled by user in debug mode
        public static Boolean PlayMusic = false; //turns music on/off (but not soundFX)
        public static Boolean SpawnMobs = true; //toggles the spawning of lesser enemies (not bosses)
        public static Boolean ProcessAI = true; //apply AI input to enemies / actors

        static Flags()
        {
            if (Release)
            {   //set flags for release mode
                Debug = false;
                DrawCollisions = false;
                Paused = false;
                PlayMusic = true;
                SpawnMobs = true;
                ProcessAI = true;
            }
        }
    }

    public static class Camera2D
    {
        public static GraphicsDevice graphics = ScreenManager.game.GraphicsDevice;
        public static Boolean lazyMovement = false;
        public static float speed = 5f; //how fast the camera moves
        public static int deadzoneX = 50;
        public static int deadzoneY = 50;

        public static Matrix view = Matrix.Identity;
        public static float targetZoom = 1.0f;
        public static float zoomSpeed = 0.05f;
        public static Vector2 currentPosition = Vector2.Zero;
        public static Vector2 targetPosition = Vector2.Zero;

        public static Matrix matRotation = Matrix.CreateRotationZ(0.0f);
        public static Matrix matZoom;
        public static Vector3 translateCenter;
        public static Vector3 translateBody;
        public static float currentZoom = 1.0f;
        public static Vector2 distance;
        public static Boolean followX = true;
        public static Boolean followY = true;
    }

    public static class PlayerData
    {
        //'wraps' saveData and provides global access to this instance
        public static SaveData saveData;
        static PlayerData()
        {
            saveData = new SaveData();
        }
        public static void Save(string FileAddress)
        {
            //open the file, serialize the data to XML, and always close the stream
            FileStream stream = File.Open(FileAddress, FileMode.OpenOrCreate);
            //serialize PlayerData to XML file
            XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
            serializer.Serialize(stream, saveData);
        }
        public static void Load(string FileAddress)
        {
            //deserialize the XML data to PlayerData
            using (FileStream stream = new FileStream(FileAddress, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(PlayerData));
                saveData = (SaveData)serializer.Deserialize(stream);
            }
        }
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

        //obj pool handles room objects, from dungeon & main sheet
        public static int roomObjCount;
        public static List<GameObject> roomObjPool;
        public static int roomObjIndex;

        //entity pool handles projectiles/particles/pickups, only from main sheet
        public static int entityCount;
        public static List<GameObject> entityPool;
        public static int entityIndex;

        public static int floorCount;
        public static List<ComponentSprite> floorPool;
        public static int floorIndex;

        public static int counter;
        public static int activeActor = 1; //tracks the current actor being handled by AI
        public static Actor hero;
        public static ComponentSprite heroShadow;

        public static void Initialize()
        {
            //set the pool sizes
            actorCount = 30;
            roomObjCount = 150;
            entityCount = 100;
            floorCount = 500;

            //actor pool
            actorPool = new List<Actor>();
            for (counter = 0; counter < actorCount; counter++)
            {
                actorPool.Add(new Actor());
                Functions_Actor.SetType(actorPool[counter], ActorType.Hero);
                Functions_Movement.Teleport(actorPool[counter].compMove, 0, 0);
            }
            actorIndex = 1;

            //room obj pool
            roomObjPool = new List<GameObject>();
            for (counter = 0; counter < roomObjCount; counter++)
            { roomObjPool.Add(new GameObject(Assets.shopSheet)); }
            roomObjIndex = 0;

            //entity pool
            entityPool = new List<GameObject>();
            for (counter = 0; counter < entityCount; counter++)
            { entityPool.Add(new GameObject(Assets.mainSheet)); }
            entityIndex = 0;

            //floor pool
            floorPool = new List<ComponentSprite>();
            for (counter = 0; counter < floorCount; counter++)
            {
                floorPool.Add(new ComponentSprite(Assets.shopSheet,
                    new Vector2(0, 0), new Byte4(6, 0, 0, 0), new Point(16, 16)));
            }
            floorIndex = 0;

            //reset all the pools
            Functions_Pool.Reset();

            //create an easy to remember reference to the player/hero actor
            hero = actorPool[0];
            //set the hero's initial loadout
            //we shouldn't really be doing this here, it's buried in the pool class which makes no sense
            //it'd be better to set the hero's loadOut when gameData is loaded
            //and by default we should load gameData set to the new game values
            hero.weapon = MenuItemType.WeaponSword;
            hero.item = MenuItemType.Unknown;
            hero.armor = MenuItemType.ArmorCloth;
            hero.equipment = MenuItemType.Unknown;

            //create the hero's shadow
            heroShadow = new ComponentSprite(Assets.mainSheet, new Vector2(0, 0), new Byte4(0, 1, 0, 0), new Point(16, 8));
            heroShadow.zOffset = -16;
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

        public static float deadzone = 0.10f; //the amount of joystick movement classified as noise
        public static Direction gamePadDirection = Direction.None;
        public static Direction lastGamePadDirection = Direction.None;

        static Input()
        {
            cursorColl.rec.Width = 4;
            cursorColl.rec.Height = 4;
            cursorColl.blocking = false;
            cursorColl.active = true;
        }
    }

    public static class WorldUI
    {
        public static int i;

        public static List<ComponentSprite> hearts;
        public static int lastHeartCount = 3;
        public static int currentHeartCount = 3;
        public static byte maxHearts = 0;
        public static int pieceCounter = 0;

        public static List<ComponentSprite> meterPieces;

        public static List<ComponentSprite> weaponBkg;
        public static List<ComponentSprite> itemBkg;
        public static MenuItem currentWeapon;
        public static MenuItem currentItem;
        public static MenuItemType heroWeapon;
        public static MenuItemType heroItem;

        public static ComponentText frametime;

        static WorldUI()
        {
            //create the heart sprites
            hearts = new List<ComponentSprite>();
            for (i = 0; i < 9; i++)
            {
                hearts.Add(new ComponentSprite(Assets.mainSheet,
                    new Vector2(0, 0), new Byte4(15, 2, 0, 0),
                    new Point(16, 16)));
            }

            //create the meter sprites
            meterPieces = new List<ComponentSprite>();
            for (i = 0; i < 11; i++)
            {
                meterPieces.Add(new ComponentSprite(Assets.mainSheet,
                    new Vector2(0, 0), new Byte4(31, 3, 0, 0),
                    new Point(8, 16)));
            }

            //set the head and tail meter frames
            meterPieces[0].currentFrame.X = 28;
            meterPieces[10].currentFrame.X = 28;
            meterPieces[10].flipHorizontally = true;

            //create the weapon and item background sprites
            weaponBkg = new List<ComponentSprite>();
            itemBkg = new List<ComponentSprite>();
            for (i = 0; i < 4; i++)
            {
                weaponBkg.Add(new ComponentSprite(Assets.mainSheet,
                    new Vector2(0, 0), new Byte4(15, 4, 0, 0),
                    new Point(16, 16)));
                itemBkg.Add(new ComponentSprite(Assets.mainSheet,
                    new Vector2(0, 0), new Byte4(15, 4, 0, 0),
                    new Point(16, 16)));
            }

            //create the current weapon & item menuItems
            currentWeapon = new MenuItem();
            currentItem = new MenuItem();

            //get the hero's current weapon and item
            heroWeapon = Pool.hero.weapon;
            heroItem = Pool.hero.item;
            Functions_MenuItem.SetMenuItemData(heroWeapon, currentWeapon);
            Functions_MenuItem.SetMenuItemData(heroItem, currentItem);

            //create the frametime text component
            frametime = new ComponentText(Assets.font, "test",
                new Vector2(640 - 55, 41), Assets.colorScheme.textLight);

            //move the entire worldUI
            Functions_WorldUI.Move(50, 50);
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

    public static class DebugMenu
    {
        public static Rectangle rec; //background rec
        public static List<ComponentButton> buttons;
        public static int counter;
        static DebugMenu()
        {
            rec = new Rectangle(0, 0, 640, 13);
            buttons = new List<ComponentButton>();
            buttons.Add(new ComponentButton("draw collisions", new Point(2, 2)));
            buttons.Add(new ComponentButton("build dungeon", new Point(68, 2)));
            buttons.Add(new ComponentButton("max gold", new Point(127, 2)));
            buttons.Add(new ComponentButton("dump savedata", new Point(168, 2)));
        }
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
        public AnimationGroup dash;
        public AnimationGroup interact;

        public AnimationGroup attack;
        public AnimationGroup hit;
        public AnimationGroup death;
        public AnimationGroup heroDeath;

        public AnimationGroup reward;
        //pickup, hold, carry, drag, etc...
    }

    public class Room
    {
        public ComponentCollision collision = new ComponentCollision();
        public Byte2 size = new Byte2(0, 0);
        public Point center = new Point(0, 0);
        public RoomType type;
        public int id;
        public Room(Point Pos, RoomType Type, int ID)
        {
            id = ID;
            type = Type;
            //set room size based on type
            if (type == RoomType.Exit)      { size.X = 11; size.Y = 11; }
            else if (type == RoomType.Hub)  { size.X = 20; size.Y = 10; }
            else if (type == RoomType.Boss) { size.X = 20; size.Y = 10; }
            else if (type == RoomType.Dev)  { size.X = 20; size.Y = 10; }
            else if (type == RoomType.Shop) { size.X = 20; size.Y = 10; }

            collision.rec.Width = size.X * 16;
            collision.rec.Height = size.Y * 16;
            Move(Pos.X, Pos.Y);
        }
        public void Move(int X, int Y)
        {
            collision.rec.X = X;
            collision.rec.Y = Y;
            center.X = X + (size.X / 2) * 16;
            center.Y = Y + (size.Y / 2) * 16;
        }
    }

    public class Dungeon
    {
        public List<Room> rooms = new List<Room>();
        public Boolean bigKey = false;
        public Boolean map = false;
        public DungeonType type = DungeonType.CursedCastle;
        public List<Point> doorLocations = new List<Point>();
    }

    public class ColorScheme
    {
        public Color background = new Color(100, 100, 100, 255);
        public Color overlay = new Color(0, 0, 0, 255);
        public Color debugBkg = new Color(0, 0, 0, 200);

        public Color collision = new Color(100, 0, 0, 50);
        public Color interaction = new Color(0, 100, 0, 50);
        public Color roomRec = new Color(0, 0, 100, 50);

        public Color buttonUp = new Color(44, 44, 44);
        public Color buttonOver = new Color(66, 66, 66);
        public Color buttonDown = new Color(100, 100, 100);

        public Color windowBkg = new Color(0, 0, 0);
        public Color windowBorder = new Color(210, 210, 210);
        public Color windowInset = new Color(130, 130, 130);
        public Color windowInterior = new Color(156, 156, 156);

        public Color textLight = new Color(255, 255, 255);
        public Color textDark = new Color(0, 0, 0);
    }





    //Data Classes

    public class Actor
    {
        public ActorType type; //the type of actor this is
        public ActorState state; //what actor is doing this frame
        public ActorState inputState; //what input wants actor to do this frame

        public Boolean stateLocked; //can actor change state? else actor must wait for state to unlock
        public byte lockTotal = 0; //how many frames the actor statelocks for, based on state
        public byte lockCounter = 0; //counts from 0 to lockTotal, then flips stateLocked false

        public ActorAnimationList animList = Functions_ActorAnimationList.actorAnims; //this never changes
        public AnimationGroup animGroup;
        public Direction direction; //direction actor is facing
        public Boolean active; //does actor input/update/draw?

        //type specific fields, set by Functions_Actor.SetType()
        public float dashSpeed;
        public float walkSpeed;

        //the components that actor requires to function
        public ComponentSprite compSprite;
        public ComponentAnimation compAnim = new ComponentAnimation();
        public ComponentInput compInput = new ComponentInput();
        public ComponentMovement compMove = new ComponentMovement();
        public ComponentCollision compCollision = new ComponentCollision();

        //health points
        public byte health;
        public byte maxHealth;

        //loadout
        public MenuItemType weapon = MenuItemType.Unknown;
        public MenuItemType item = MenuItemType.Unknown;
        public MenuItemType armor = MenuItemType.Unknown;
        public MenuItemType equipment = MenuItemType.Unknown;

        public Actor()
        {
            compSprite = new ComponentSprite(Assets.heroSheet, new Vector2(0, 0), new Byte4(0, 0, 0, 0), new Point(16, 16));
            Functions_Actor.SetType(this, ActorType.Hero);//default to hero actor
            Functions_Movement.Teleport(this.compMove, compSprite.position.X, compSprite.position.Y);
        }
    }

    public class GameObject
    {
        public ObjGroup group = ObjGroup.Object;
        public ObjType type = ObjType.WallStraight;

        public ComponentSprite compSprite;
        public ComponentCollision compCollision = new ComponentCollision();
        public ComponentAnimation compAnim = new ComponentAnimation();
        public ComponentMovement compMove = new ComponentMovement();

        public Direction direction = Direction.Down;
        public Boolean active = true; //does this object draw, update?
        public Boolean getsAI = false; //does this object get passed to Functions_AI.HandleObj()?

        public Byte lifetime;   //how many frames this object exists for, 0 = forever/ignore
        public Byte lifeCounter;//counts up to lifetime value

        public GameObject(Texture2D Texture)
        {   //initialize to default value - data is changed later
            compSprite = new ComponentSprite(Texture, new Vector2(50, 50), new Byte4(0, 0, 0, 0), new Point(16, 16));
            Functions_GameObject.SetType(this, type);
        }
    }

    public class SaveData
    {   //data that will be saved/loaded from game session to session
        public int gold = 99;
        public byte heartPieces = 4 * 3; //sets max health

        public byte magicCurrent = 3;
        public byte magicUnlocked = 3;
        public byte magicTotal; //magicUnlocked + any modifiers

        public byte bombsCurrent = 3;
        public byte bombsMax = 99;

        public byte arrowsCurrent = 99; //testing
        public byte arrowsMax = 99;

        public Boolean itemBoomerang = false;
        //itemBomb

        public Boolean bottle1 = false;
        public Boolean bottle2 = false;
        public Boolean bottle3 = false;
        public Boolean bottleHealth = false;
        public Boolean bottleMagic = false;
        public Boolean bottleFairy = false;

        public Boolean magicFireball = false;
        //portal

        public Boolean weaponBow = false;
        //axe

        public Boolean armorChest = false;
        public Boolean armorCape = false;
        public Boolean armorRobe = false;

        public Boolean equipmentRing = false;
        //pearl
        //necklace
        //glove
        //pin
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

        public MenuItem()
        {   //default to ? sprite, hidden offscreen
            compSprite = new ComponentSprite(Assets.mainSheet,
                new Vector2(-100, 1000),
                new Byte4(15, 5, 0, 0),
                new Point(16, 16));
            Functions_MenuItem.SetMenuItemData(MenuItemType.Unknown, this);
            neighborUp = this; neighborDown = this;
            neighborLeft = this; neighborRight = this;
        }
    }

    public class MenuRectangle
    {
        public DisplayState displayState;
        public int animationSpeed = 5;      //how quickly the UI element animates in/out
        public int animationCounter = 0;    //counts up to delay value
        public int openDelay = 0;           //how many updates are ignored before open animation occurs
        public Rectangle rec = new Rectangle(0, 0, 0, 0);
        public Point position;
        public Point size;
        public Color color;

        public MenuRectangle(Point Position, Point Size, Color Color)
        {
            position = Position; size = Size;
            color = Color; Reset();
        }

        public void Update()
        {
            if (displayState == DisplayState.Opening)
            {
                if (animationCounter < openDelay) { animationCounter += 1; }
                if (animationCounter >= openDelay)
                {   //grow right
                    rec.Height = size.Y;
                    if (rec.Width < size.X) { rec.Width += ((size.X - rec.Width) / animationSpeed) + 1; } //easeIn 
                    if (rec.Width > size.X) { rec.Width = size.X; }
                    if (rec.Width == size.X) { displayState = DisplayState.Opened; animationCounter = 0; } //open complete
                }
            }
        }

        public void Reset()
        {
            rec.Width = 0;
            rec.Height = 0;
            rec.Location = position;
            animationCounter = 0;
            displayState = DisplayState.Opening;
        }

    }

    public class MenuWindow
    {
        public Point size;
        public int animationCounter = 0;        //counts up to delay value
        public int openDelay = 0;               //how many updates are ignored before open occurs

        public MenuRectangle background;
        public MenuRectangle border;
        public MenuRectangle inset;
        public MenuRectangle interior;

        public ComponentText title;
        public MenuRectangle headerLine;
        public MenuRectangle footerLine;

        public MenuWindow(Point Position, Point Size, String Title)
        {
            size = Size;
            //create the window components
            background = new MenuRectangle(new Point(0, 0), new Point(0, 0), Assets.colorScheme.windowBkg);
            border = new MenuRectangle(new Point(0, 0), new Point(0, 0), Assets.colorScheme.windowBorder);
            inset = new MenuRectangle(new Point(0, 0), new Point(0, 0), Assets.colorScheme.windowInset);
            interior = new MenuRectangle(new Point(0, 0), new Point(0, 0), Assets.colorScheme.windowInterior);
            headerLine = new MenuRectangle(new Point(0, 0), new Point(0, 0), Assets.colorScheme.windowInset);
            footerLine = new MenuRectangle(new Point(0, 0), new Point(0, 0), Assets.colorScheme.windowInset);
            title = new ComponentText(Assets.font, "", new Vector2(0, 0), Assets.colorScheme.textDark);
            //align all the window components
            ResetAndMove(Position.X, Position.Y, Size, Title);
            //set the openDelay to cascade in all the components
            background.openDelay = 0;
            border.openDelay = 2;
            inset.openDelay = 2;
            interior.openDelay = 8;
            headerLine.openDelay = 12;
            footerLine.openDelay = 12;
        }

        public void Update()
        {   //count up to the openDelay value, then begin updating the menu rectangles
            if (animationCounter < openDelay) { animationCounter += 1; }
            if (animationCounter >= openDelay)
            {
                background.Update();
                border.Update();
                inset.Update();
                interior.Update();
                headerLine.Update();
                footerLine.Update();
            }
        }

        public void ResetAndMove(int X, int Y, Point Size, String Title)
        {
            size = Size;
            //set the new title, move into position
            title.text = Title;
            title.position.X = X + 8;
            title.position.Y = Y + 2;


            #region Reset all the MenuRectangles, and update them to the passed Position + Size

            background.position.X = X + 0;
            background.position.Y = Y + 0;
            background.size.X = Size.X + 0;
            background.size.Y = Size.Y + 0;
            background.Reset();

            border.position.X = X + 1;
            border.position.Y = Y + 1;
            border.size.X = Size.X - 2;
            border.size.Y = Size.Y - 2;
            border.Reset();

            inset.position.X = X + 2;
            inset.position.Y = Y + 2;
            inset.size.X = Size.X - 4;
            inset.size.Y = Size.Y - 4;
            inset.Reset();

            interior.position.X = X + 3;
            interior.position.Y = Y + 3;
            interior.size.X = Size.X - 6;
            interior.size.Y = Size.Y - 6;
            interior.Reset();

            #endregion


            #region Reset the header and footer lines, update with Position + Size

            headerLine.position.X = X + 8;
            headerLine.position.Y = Y + 16;
            headerLine.size.X = Size.X - 16;
            headerLine.size.Y = 1;
            headerLine.Reset();

            footerLine.position.X = X + 8;
            footerLine.position.Y = Y + Size.Y - 16;
            footerLine.size.X = Size.X - 16;
            footerLine.size.Y = 1;
            footerLine.Reset();

            #endregion

        }

    }





    //Data Components

    public class ComponentCollision
    {   //allows an object or actor to collide with other objects or actors
        public Rectangle rec = new Rectangle(100, 100, 16, 16); //used in collision checking
        public int offsetX = 0; //offsets rec from sprite.position
        public int offsetY = 0; //offsets rec from sprite.position
        public Boolean blocking = true; //impassable or interactive
        public Boolean active = false; //check to see if this component collides with other components?
    }

    public class ComponentMovement
    {   //allows an object or actor to move, with speed and friction
        public Vector2 position = new Vector2(100, 100); //current position of actor/object
        public Vector2 newPosition = new Vector2(100, 100); //projected position
        public Direction direction = Direction.Down; //the direction actor/obj is moving
        public Vector2 magnitude = new Vector2(0, 0); //how much actor/obj moves each frames
        public float speed = 0.0f; //controls magnitude
        public float friction = 0.75f; //reduces magnitude each frame
    }

    public class ComponentAnimation
    {   //allows an object or actor to animate through a sequence of frames on a timer
        public List<Byte4> currentAnimation; //a list of byte4 representing frames of an animation
        public byte index = 0; //where in the currentAnimation list the animation is (animation index)
        public byte speed = 10; //how many frames should elapse before animation is updated (limits animation speed)
        public byte timer = 0; //how many frames have elapsed since last animation update (counts frames) @ 60fps
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
            compText = new ComponentText(Assets.font, Text, new Vector2(0, 0), Assets.colorScheme.textLight);
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