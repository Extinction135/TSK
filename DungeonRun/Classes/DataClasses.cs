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
using System.IO;
using System.Xml.Serialization;

namespace DungeonRun
{
    //all the data classes the program uses

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
        public Byte2 size;
        public Point center;
        public RoomType type;
        public byte enemyCount;
        public int id;
        public Room(Point Pos, Byte2 Size, RoomType Type, byte EnemyCount, int ID)
        {
            collision.rec.X = Pos.X;
            collision.rec.Y = Pos.Y;
            collision.rec.Width = Size.X * 16;
            collision.rec.Height = Size.Y * 16;
            size = Size;
            center = new Point(Pos.X + (Size.X / 2) * 16, Pos.Y + (Size.Y / 2) * 16);
            type = Type;
            enemyCount = EnemyCount;
            id = ID;
        }
    }

    public class Dungeon
    {
        public List<Room> rooms = new List<Room>();
        public Boolean bigKey = false;
        public Boolean map = false;
        public DungeonType type = DungeonType.CursedCastle;
    }

    public class ColorScheme
    {
        public Color background = new Color(100, 100, 100, 255);
        public Color overlay = new Color(0, 0, 0, 255);
        public Color debugBkg = new Color(0, 0, 0, 200);

        public Color collision = new Color(100, 0, 0, 50);
        public Color interaction = new Color(0, 100, 0, 50);

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

        //type specific fields, changed by ActorFunctions.SetType()
        public float dashSpeed = 0.75f;
        public float walkSpeed = 0.25f;

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
        public MenuItemType weapon;
        public MenuItemType item;
        public MenuItemType armor;
        public MenuItemType equipment;

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

        public Byte lifetime;   //how many frames this object exists for, 0 = forever/ignore
        public Byte lifeCounter;//counts up to lifetime value

        public GameObject(Texture2D Texture)
        {   //initialize to default value - this data is changed in Update()
            compSprite = new ComponentSprite(Texture, new Vector2(50, 50), new Byte4(0, 0, 0, 0), new Point(16, 16));
            Functions_GameObject.SetType(this, type);
        }
    }

    public class Camera2D
    {
        public GraphicsDevice graphics;
        public Boolean lazyMovement = false;
        public float speed = 5f; //how fast the camera moves
        public int deadzoneX = 50;
        public int deadzoneY = 50;

        public Matrix view = Matrix.Identity;
        public float targetZoom = 1.0f;
        public float zoomSpeed = 0.05f;
        public Vector2 currentPosition = Vector2.Zero;
        public Vector2 targetPosition = Vector2.Zero;

        public Matrix matRotation = Matrix.CreateRotationZ(0.0f);
        public Matrix matZoom;
        public Vector3 translateCenter;
        public Vector3 translateBody;
        public float currentZoom = 1.0f;
        public Vector2 distance;
        public Boolean followX = true;
        public Boolean followY = true;

        public Camera2D()
        {
            graphics = ScreenManager.game.GraphicsDevice;
            //these two values dont change on a 2D camera
            translateCenter.Z = 0; translateBody.Z = 0;
        }
    }

    public class SaveData
    {   //data that will be saved/loaded from game session to session
        public int gold = 99;
        public byte heartPieces = 4 * 3; //sets max health

        public byte magicCurrent = 3; //current magic amount
        public byte magicMax = 3; //max magic amount

        public byte bombsCurrent = 3;
        public byte bombsMax = 99;

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
        //bow
        //axe

        public Boolean armorPlatemail = false;
        //platemail
        //cape
        //robe

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
            ResetAndMoveWindow(Position, Size, Title);
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

        public void ResetAndMoveWindow(Point Position, Point Size, String Title)
        {
            size = Size;
            //set the new title, move into position
            title.text = Title;
            title.position.X = Position.X + 8;
            title.position.Y = Position.Y + 2;


            #region Reset all the MenuRectangles, and update them to the passed Position + Size

            background.position.X = Position.X + 0;
            background.position.Y = Position.Y + 0;
            background.size.X = Size.X + 0;
            background.size.Y = Size.Y + 0;
            background.Reset();

            border.position.X = Position.X + 1;
            border.position.Y = Position.Y + 1;
            border.size.X = Size.X - 2;
            border.size.Y = Size.Y - 2;
            border.Reset();

            inset.position.X = Position.X + 2;
            inset.position.Y = Position.Y + 2;
            inset.size.X = Size.X - 4;
            inset.size.Y = Size.Y - 4;
            inset.Reset();

            interior.position.X = Position.X + 3;
            interior.position.Y = Position.Y + 3;
            interior.size.X = Size.X - 6;
            interior.size.Y = Size.Y - 6;
            interior.Reset();

            #endregion


            #region Reset the header and footer lines, update with Position + Size

            headerLine.position.X = Position.X + 8;
            headerLine.position.Y = Position.Y + 16;
            headerLine.size.X = Size.X - 16;
            headerLine.size.Y = 1;
            headerLine.Reset();

            footerLine.position.X = Position.X + 8;
            footerLine.position.Y = Position.Y + Size.Y - 16;
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





    //Global Classes

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
        public static int dungeonID;
        public static Boolean beatDungeon;

        public static Stopwatch timer;
        public static int enemyCount;
        public static int totalDamage;

        public static void Reset()
        {
            dungeonID = 0;
            beatDungeon = false;

            timer = new Stopwatch();
            timer.Reset();
            enemyCount = 0;
            totalDamage = 0;
        }
    }

    public static class Pool
    {
        //actor pool handles all actors in the room including hero
        public static int actorCount;           //total count of actors in pool
        public static List<Actor> actorPool;    //the actual list of actors
        public static int actorIndex;           //used to iterate thru the pool

        //obj pool handles gameobjects that don't move, & may(not) interact with actors
        public static int objCount;
        public static List<GameObject> objPool;
        public static int objIndex;

        //projectile pool handles projectiles/particles that move or are stationary
        public static int projectileCount;
        public static List<GameObject> projectilePool;
        public static int projectileIndex;

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
            objCount = 150;
            projectileCount = 50;
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

            //obj pool
            objPool = new List<GameObject>();
            for (counter = 0; counter < objCount; counter++)
            { objPool.Add(new GameObject(Assets.shopSheet)); }
            objIndex = 0;

            //projectile pool
            projectilePool = new List<GameObject>();
            for (counter = 0; counter < projectileCount; counter++)
            { projectilePool.Add(new GameObject(Assets.mainSheet)); }
            projectileIndex = 0;

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

}