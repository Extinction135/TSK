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

    public struct Byte2
    {
        public byte X;
        public byte Y;
        public Byte2(byte x, byte y)
        {
            X = x; Y = y;
        }
    }

    public struct Byte4
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

    public struct AnimationGroup
    {   //represents an animation with Down, Up, Left, Right states
        public List<Byte4> down;
        public List<Byte4> up;
        public List<Byte4> right;
        public List<Byte4> left;
    }

    public struct ActorAnimationList
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

    public struct Room
    {
        public ComponentCollision collision;
        public Byte2 size;
        public Point center;
        public RoomType type;
        public byte enemyCount;
        public int id;
        public Room(Point Pos, Byte2 Size, RoomType Type, byte EnemyCount, int ID)
        {
            collision = new ComponentCollision();
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

    public struct Dungeon
    {
        public List<Room> rooms;
        public String name;
        public Boolean bigKey;
        public Boolean map;
        public DungeonType type;
        public Dungeon(String Name)
        {   //initially, the map and key have not been found
            rooms = new List<Room>();
            name = Name;
            bigKey = false;
            map = false;
            type = DungeonType.CursedCastle;
        }
    }

    public struct SaveData
    {   //data that will be saved/loaded from game session to session
        public String name;
        public int gold;
        public byte heartPieces; //sets max health

        public byte magicCurrent; //current magic amount
        public byte magicMax; //max magic amount

        public byte bombsCurrent;
        public byte bombsMax;

        public Boolean itemBoomerang;
        //itemBomb

        public Boolean bottle1;
        public Boolean bottle2;
        public Boolean bottle3;
        public Boolean bottleHealth;
        public Boolean bottleMagic;
        public Boolean bottleFairy;

        public Boolean magicFireball;
        //portal

        public Boolean weaponBow;
        //bow
        //axe

        public Boolean armorPlatemail;
        //platemail
        //cape
        //robe

        public Boolean equipmentRing;
        //pearl
        //necklace
        //glove
        //pin

        public SaveData(String Name)
        {
            name = Name;
            gold = 99;
            heartPieces = 4 * 3; //player starts with 3 hearts

            magicCurrent = 3;
            magicMax = 3;

            bombsCurrent = 3;
            bombsMax = 99;

            //all items default to false
            itemBoomerang = false;

            bottle1 = false;
            bottle2 = false;
            bottle3 = false;
            bottleHealth = false;
            bottleMagic = false;
            bottleFairy = false;

            magicFireball = false;
            weaponBow = false;
            armorPlatemail = false;
            equipmentRing = false;
        }

    }

    public struct ColorScheme
    {
        public String name;
        public Color background;
        public Color overlay;
        public Color debugBkg;

        public Color collision;
        public Color interaction;

        public Color buttonUp;
        public Color buttonOver;
        public Color buttonDown;

        public Color windowBkg;
        public Color windowBorder;
        public Color windowInset;
        public Color windowInterior;

        public Color textLight;
        public Color textDark;

        public ColorScheme(String Name)
        {
            name = Name;
            background = new Color(100, 100, 100, 255);
            overlay = new Color(0, 0, 0, 255);
            debugBkg = new Color(0, 0, 0, 200);

            collision = new Color(100, 0, 0, 50);
            interaction = new Color(0, 100, 0, 50);

            buttonUp = new Color(44, 44, 44);
            buttonOver = new Color(66, 66, 66);
            buttonDown = new Color(100, 100, 100);

            windowBkg = new Color(0, 0, 0);
            windowBorder = new Color(210, 210, 210);
            windowInset = new Color(130, 130, 130);
            windowInterior = new Color(156, 156, 156);

            textLight = new Color(255, 255, 255);
            textDark = new Color(0, 0, 0);
        }
    }



    //Classes (instanced)

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
        public ComponentAnimation compAnim;
        public ComponentInput compInput;
        public ComponentMovement compMove;
        public ComponentCollision compCollision;

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
            //create the actor components
            compSprite = new ComponentSprite(Assets.heroSheet, new Vector2(0, 0), new Byte4(0, 0, 0, 0), new Point(16, 16));
            compAnim = new ComponentAnimation();
            compInput = new ComponentInput();
            compMove = new ComponentMovement();
            compCollision = new ComponentCollision();
            //set the actor type to hero, teleport to position
            Functions_Actor.SetType(this, ActorType.Hero);
            Functions_Movement.Teleport(this.compMove, compSprite.position.X, compSprite.position.Y);
        }
    }

    public class GameObject
    {
        public ObjGroup group;
        public ObjType type;

        public ComponentSprite compSprite;
        public ComponentCollision compCollision;
        public ComponentAnimation compAnim;
        public ComponentMovement compMove;

        public Direction direction;
        public Boolean active; //does this object draw, update?

        public Byte lifetime;   //how many frames this object exists for, 0 = forever/ignore
        public Byte lifeCounter;//counts up to lifetime value

        public GameObject(Texture2D Texture)
        {   //initialize to default value - this data is changed in Update()
            group = ObjGroup.Object;
            type = ObjType.WallStraight;
            compSprite = new ComponentSprite(Texture, new Vector2(50, 50), new Byte4(0, 0, 0, 0), new Point(16, 16));
            compCollision = new ComponentCollision();
            compAnim = new ComponentAnimation();
            compMove = new ComponentMovement();
            direction = Direction.Down;
            active = true;
            Functions_GameObject.SetType(this, type);
        }
    }

    public class MenuItem
    {
        public ComponentSprite compSprite;
        public ComponentAnimation compAnim;
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
            compAnim = new ComponentAnimation();
            Functions_MenuItem.SetMenuItemData(MenuItemType.Unknown, this);
            neighborUp = this;
            neighborDown = this;
            neighborLeft = this;
            neighborRight = this;
        }
    }

    public class Camera2D
    {
        public GraphicsDevice graphics;
        public Boolean lazyMovement = false;
        public float speed = 5f; //how fast the camera moves
        public int deadzoneX = 50;
        public int deadzoneY = 50;

        public Matrix view;
        public float targetZoom = 1.0f;
        public float zoomSpeed = 0.05f;
        public Vector2 currentPosition;
        public Vector2 targetPosition;

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
            view = Matrix.Identity;
            translateCenter.Z = 0; //these two values dont change on a 2D camera
            translateBody.Z = 0;
            currentPosition = Vector2.Zero; //initially the camera is at 0,0
            targetPosition = Vector2.Zero;
            targetZoom = 1.0f;
        }
    }



    //Classes (global)

    public static class PlayerData
    {
        //'wraps' saveData and provides global access to this instance
        public static SaveData saveData;
        static PlayerData()
        {
            saveData = new SaveData("NewGame");
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



    //Components (instanced)

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