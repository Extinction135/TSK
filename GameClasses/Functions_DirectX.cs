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
    public static class Functions_Backend
    {
        public static string GetRam()
        {   //get the ram footprint in mb
			return "" + (Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024);
        }
		
		public static async void LoadRoomData()
        {
            //NEEDS TO BE IMPLEMENTED, BASED ON FUNCTIONS_UWP
        }
		
		public static async void SavePlayerData()
        {
			//NEEDS TO BE IMPLEMENTED, BASED ON FUNCTIONS_UWP
			//open the file, serialize the data to XML, and always close the stream
            //FileStream stream = File.Open(FileAddress, FileMode.OpenOrCreate);
            //serialize PlayerData to XML file
            //XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
            //serializer.Serialize(stream, saveData);
        }

        public static async void LoadPlayerData()
        {
			//NEEDS TO BE IMPLEMENTED, BASED ON FUNCTIONS_UWP
			//deserialize the XML data to PlayerData
            //using (FileStream stream = new FileStream(FileAddress, FileMode.Open))
            //{
            //    XmlSerializer serializer = new XmlSerializer(typeof(PlayerData));
            //    saveData = (SaveData)serializer.Deserialize(stream);
            //}
        }
    }
}