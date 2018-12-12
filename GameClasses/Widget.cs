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






        //interactive object widgets - set 1
        public static WidgetObjects_Forest WO_Forest = new WidgetObjects_Forest();
        public static WidgetObjects_Mountain WO_Mountain = new WidgetObjects_Mountain();
        public static WidgetObjects_Swamp WO_Swamp = new WidgetObjects_Swamp();
        public static WidgetObjects_Lava WO_Lava = new WidgetObjects_Lava();
        public static WidgetObjects_Cloud WO_Cloud = new WidgetObjects_Cloud();
        public static WidgetObjects_ThievesDen WO_Den= new WidgetObjects_ThievesDen();
        public static WidgetObjects_Shadow WO_Shadow = new WidgetObjects_Shadow();
        //interactive object widgets - set 2
        public static WidgetObjects_Environment WO_Environment = new WidgetObjects_Environment();
        public static WidgetObjects_Water WO_Water = new WidgetObjects_Water();
        public static WidgetObjects_House WO_House = new WidgetObjects_House();
        public static WidgetObjects_NPCS WO_NPCS = new WidgetObjects_NPCS();
        public static WidgetObjects_Dev1 WO_Dev1 = new WidgetObjects_Dev1();
        public static WidgetObjects_Dev2 WO_Dev2 = new WidgetObjects_Dev2();
        public static WidgetObjects_Dungeon WO_Dungeon = new WidgetObjects_Dungeon();

        //indestructible object widgets - set 1
        public static WidgetIndestructibleObjs_Forest WD_Forest = new WidgetIndestructibleObjs_Forest();
        public static WidgetIndestructibleObjs_Mountain WD_Mountain = new WidgetIndestructibleObjs_Mountain();
        public static WidgetIndestructibleObjs_Swamp WD_Swamp = new WidgetIndestructibleObjs_Swamp();
        public static WidgetIndestructibleObjs_Lava WD_Lava = new WidgetIndestructibleObjs_Lava();
        public static WidgetIndestructibleObjs_Cloud WD_Cloud = new WidgetIndestructibleObjs_Cloud();
        public static WidgetIndestructibleObjs_ThievesDen WD_Den = new WidgetIndestructibleObjs_ThievesDen();
        public static WidgetIndestructibleObjs_Shadow WD_Shadow = new WidgetIndestructibleObjs_Shadow();
        //indestructible object widgets - set 2
        public static WidgetIndestructibleObjs_BoatA WD_BoatA = new WidgetIndestructibleObjs_BoatA();
        public static WidgetIndestructibleObjs_BoatB WD_BoatB = new WidgetIndestructibleObjs_BoatB();
        public static WidgetIndestructibleObjs_Coliseum WD_Coliseum = new WidgetIndestructibleObjs_Coliseum();
        public static WidgetIndestructibleObjs_Dev1 WD_Dev1 = new WidgetIndestructibleObjs_Dev1();
        public static WidgetIndestructibleObjs_Dev2 WD_Dev2 = new WidgetIndestructibleObjs_Dev2();
        public static WidgetIndestructibleObjs_Dev3 WD_Dev3 = new WidgetIndestructibleObjs_Dev3();
        public static WidgetIndestructibleObjs_Dev4 WD_Dev4 = new WidgetIndestructibleObjs_Dev4();

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