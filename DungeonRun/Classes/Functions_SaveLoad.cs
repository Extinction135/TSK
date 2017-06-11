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
//using System.Windows.Forms;



namespace DungeonRun
{
    public static class Functions_SaveLoad
    {


        public static void Save(Room Room)
        {
            /*
            //open a file dialog for where to save room data
            string fileaddress = @"C:\Users\Gx000000\Desktop\REPOs\DungeonRun\DungeonRun\DungeonRun\RoomData\test.xml";
            //create the file if it doesn't exist, or empty it's contents if it does exist
            File.WriteAllText(fileaddress, string.Empty);
            //open the file, serialize the data to XML, and always close the stream
            FileStream stream = File.Open(fileaddress, FileMode.OpenOrCreate);
            try
            {
                //XmlSerializer serializer = new XmlSerializer(typeof(RoomData));
                //serializer.Serialize(stream, roomData);
            }
            catch (Exception e) { }
            //finally { stream.Close(); }
            */
        }

        public static void Load(Room Room)
        { }


    }
}