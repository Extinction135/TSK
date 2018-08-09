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

        //unique object widgets
        public static WidgetObjects_Town WO_Town = new WidgetObjects_Town();
        public static WidgetObjects_Colliseum WO_Colliseum = new WidgetObjects_Colliseum();
        public static WidgetObjects_Boat_Front WO_Boat_Front = new WidgetObjects_Boat_Front();
        public static WidgetObjects_Boat_Back WO_Boat_Back = new WidgetObjects_Boat_Back();

        public static WidgetObjects_Forest WO_Forest = new WidgetObjects_Forest();
        public static WidgetObjects_Mountain WO_Mountain = new WidgetObjects_Mountain();
        public static WidgetObjects_Swamp WO_Swamp = new WidgetObjects_Swamp();

        public static WidgetObjects_DEV WO_DEV = new WidgetObjects_DEV();

        //actor widgets
        public static WidgetActors_Forest WE_Forest = new WidgetActors_Forest();
        public static WidgetActors_Mountain WE_Mountain = new WidgetActors_Mountain();
        public static WidgetActors_Swamp WE_Swamp = new WidgetActors_Swamp();
        public static WidgetActors_Town WE_Town = new WidgetActors_Town();
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