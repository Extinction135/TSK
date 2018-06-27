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
    public static class Widgets
    {   //this is the global instance that contains all widget instances

        //game widgets
        public static WidgetDialog Dialog = new WidgetDialog();
        public static WidgetForSale ForSale = new WidgetForSale();
        public static WidgetInfo Info = new WidgetInfo();
        public static WidgetInventory Inventory = new WidgetInventory();
        public static WidgetLoadout Loadout = new WidgetLoadout();
        public static WidgetOptions Options = new WidgetOptions();
        public static WidgetQuestItems QuestItems = new WidgetQuestItems();

        //editor widgets
        public static WidgetObjectTools ObjectTools = new WidgetObjectTools();
        public static WidgetRoomTools RoomTools = new WidgetRoomTools();

        //shared object widgets
        public static WidgetObjects_Environment WO_Environment = new WidgetObjects_Environment();
        public static WidgetObjects_Dungeon WO_Dungeon = new WidgetObjects_Dungeon();
        public static WidgetObjects_Building WO_Building = new WidgetObjects_Building();

        //forest / standard object widgets
        public static WidgetObjects_Forest1 WO_Forest1 = new WidgetObjects_Forest1();
        public static WidgetObjects_Forest2 WO_Forest2 = new WidgetObjects_Forest2();

        //colliseum object widgets
        public static WidgetObjects_Building_Colliseum WO_Building_Colliseum = new WidgetObjects_Building_Colliseum();
    }

    public abstract class Widget
    {   //the base class for all widgets
        public int i;
        public MenuWindow window;
        public virtual void Reset(int X, int Y) { }
        public virtual void Update() { }
        public virtual void Draw() { }
    }
}