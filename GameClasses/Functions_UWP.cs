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
using Windows.Storage; //UWP
using Windows.System; //UWP

namespace DungeonRun
{
    public static class Functions_Backend
    {
        static StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        //all methods are empty except the ram collection
        //because all development & edting is done in directX
        //where things are not as crazy

        public static string GetRam()
        {   //get the ram footprint in mb
			return "" + (MemoryManager.AppMemoryUsage / 1024 / 1024);
        }
		
        public static void ConvertXMLtoCS() { }

        public static void SetFilename(GameFile Type) { }

        public static void SaveRoomData(RoomXmlData RoomData) { }

        public static void SelectRoomFile() { }

    }
}