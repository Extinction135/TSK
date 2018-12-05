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






        //interactive object widgets
        public static WidgetObjects_Environment WO_Environment = new WidgetObjects_Environment();
        public static WidgetObjects_Water WO_Water = new WidgetObjects_Water();
        public static WidgetObjects_House WO_House = new WidgetObjects_House();
        public static WidgetObjects_NPCS WO_NPCS = new WidgetObjects_NPCS();
        public static WidgetObjects_Mountain WO_Mtn = new WidgetObjects_Mountain();
        public static WidgetObjects_Dev WO_Dev = new WidgetObjects_Dev();
        public static WidgetObjects_Dungeon WO_Dungeon = new WidgetObjects_Dungeon();

        //indestructible object widgets
        public static WidgetIndestructibleObjs_BoatA WD_BoatA = new WidgetIndestructibleObjs_BoatA();
        public static WidgetIndestructibleObjs_BoatB WD_BoatB = new WidgetIndestructibleObjs_BoatB();
        public static WidgetIndestructibleObjs_Forest WD_Forest = new WidgetIndestructibleObjs_Forest();
        public static WidgetIndestructibleObjs_Mountain WD_Mountain = new WidgetIndestructibleObjs_Mountain();
        public static WidgetIndestructibleObjs_Swamp WD_Swamp = new WidgetIndestructibleObjs_Swamp();
        public static WidgetIndestructibleObjs_Coliseum WD_Coliseum = new WidgetIndestructibleObjs_Coliseum();

        //actor widgets
        public static WidgetActors_Forest WA_Forest = new WidgetActors_Forest();
        public static WidgetActors_Mountain WA_Mountain = new WidgetActors_Mountain();
        public static WidgetActors_Swamp WA_Swamp = new WidgetActors_Swamp();
        public static WidgetActors_Lava WA_Lava = new WidgetActors_Lava();
        public static WidgetActors_Cloud WA_Cloud = new WidgetActors_Cloud();
        public static WidgetActors_ThievesDen WA_Thievs = new WidgetActors_ThievesDen();
        public static WidgetActors_Shadow WA_Shadow = new WidgetActors_Shadow();
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