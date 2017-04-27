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
    public static class PlayerData
    {   
        //'wraps' saveData and provides global access to this instance
        public static SaveData saveData;
        static PlayerData()
        {
            saveData = new SaveData();
            saveData.gold = 0;
            saveData.heartPieces = 4 * 3; //player starts with 3 hearts

            saveData.heartPieces += 4;
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
}