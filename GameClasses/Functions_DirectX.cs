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
using System.Windows.Forms;

namespace DungeonRun
{
    public static class Functions_Backend
    {
        //note - this is purely for editor purposes, not game purposes

        static string localFolder = AppDomain.CurrentDomain.BaseDirectory;
        //static string localFolder = Environment.CurrentDirectory; //same address

        static string repoDir = @"C:\Users\Gx000000\Desktop\REPOs\TheShadowKing\TSK";
        static XmlSerializer serializer = new XmlSerializer(typeof(RoomXmlData));

        public static string GetRam() //get the ram footprint in mb
        { return "" + (Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024); }



        //this was created to write overworld islandsets (lists of levels/rooms)
        public static void writeXMLtoCS(List<RoomXmlData> Levels, IslandID islandID)
        {
            Debug.WriteLine("writing " + islandID + " XML to CS... wait...");


            #region Build C# output string to write

            StringBuilder csOutput = new StringBuilder();
            //string csOutput = "";
            csOutput.Append("using System.Collections.Generic;\n");
            csOutput.Append("\n");
            csOutput.Append("namespace DungeonRun\n");
            csOutput.Append("{\n");

            csOutput.Append("\tpublic static class " + islandID + "\n");
            csOutput.Append("\t{\n");

            //field defs
            csOutput.Append("\t\t//levels specific to this island\n");
            for (int i = 0; i < Levels.Count(); i++)
            {
                csOutput.Append("\t\tpublic static RoomXmlData " + Levels[i].type + " = new RoomXmlData();\n");
            }
            csOutput.Append("\n");

            //populate fields
            csOutput.Append("\t\t//level data\n");
            csOutput.Append("\t\tstatic " + islandID + "()\n");
            csOutput.Append("\t\t{\n");

            for (int i = 0; i < Levels.Count(); i++)
            {
                Debug.WriteLine("writing level " + i + " of " + (Levels.Count - 1));

                csOutput.Append("\n");
                csOutput.Append("\t\t\t#region " + Levels[i].type + "\n\n");

                csOutput.Append("\t\t\t" + Levels[i].type + ".type = RoomID." + Levels[i].type + ";\n"); //lol

                //wind
                csOutput.Append("\t\t\t" + Levels[i].type + ".windDirection = Direction." + Levels[i].windDirection + ";\n");
                csOutput.Append("\t\t\t" + Levels[i].type + ".windFrequency = " + Levels[i].windFrequency + ";\n");
                csOutput.Append("\t\t\t" + Levels[i].type + ".windIntensity = " + Levels[i].windIntensity + ";\n");

                //built into constructor?
                //csOutput += "\t\t\t" + Levels[i].type + ".inds = new List<IndObjXmlData>();\n";
                //csOutput += "\t\t\t" + Levels[i].type + ".ints = new List<IntObjXmlData>();\n";

                for (int g = 0; g < Levels[i].inds.Count(); g++)
                {
                    csOutput.Append("\t\t\t{");

                    csOutput.Append("IndObjXmlData obj = new IndObjXmlData(); ");

                    csOutput.Append("obj.type = IndestructibleType." + Levels[i].inds[g].type + "; ");
                    csOutput.Append("obj.direction = Direction." + Levels[i].inds[g].direction + "; ");
                    csOutput.Append("obj.posX = " + Levels[i].inds[g].posX + "; ");
                    csOutput.Append("obj.posY = " + Levels[i].inds[g].posY + "; ");

                    csOutput.Append("" + Levels[i].type + ".inds.Add(obj);");

                    csOutput.Append("}\n");
                }

                for (int g = 0; g < Levels[i].ints.Count(); g++)
                {
                    csOutput.Append("\t\t\t{");

                    csOutput.Append("IntObjXmlData obj = new IntObjXmlData(); ");

                    csOutput.Append("obj.type = InteractiveType." + Levels[i].ints[g].type + "; ");
                    csOutput.Append("obj.direction = Direction." + Levels[i].ints[g].direction + "; ");
                    csOutput.Append("obj.posX = " + Levels[i].ints[g].posX + "; ");
                    csOutput.Append("obj.posY = " + Levels[i].ints[g].posY + "; ");

                    csOutput.Append("" + Levels[i].type + ".ints.Add(obj);");

                    csOutput.Append("}\n");
                }

                csOutput.Append("\t\t\t#endregion\n");
                csOutput.Append("\n");
            }
            csOutput.Append("\t\t}\n");
            csOutput.Append("\t}\n");
            csOutput.Append("}");

            #endregion


            string islandAddress = repoDir + @"\GameClasses\" + islandID + @".cs";
            File.WriteAllText(islandAddress, csOutput.ToString());
        }

        //this writes all levelData OR all roomData
        public static void ConvertXMLtoCS()
        {
            Debug.WriteLine("Beginning XML to CS conversion... wait...");


            List<RoomXmlData> SkullIsland_Data = new List<RoomXmlData>();
            List<RoomXmlData> ForestIsland_Data = new List<RoomXmlData>();
            List<RoomXmlData> ThievesHideout_Data = new List<RoomXmlData>();
            List<RoomXmlData> DeathMountain_Data = new List<RoomXmlData>();
            List<RoomXmlData> HauntedSwamps_Data = new List<RoomXmlData>();
            List<RoomXmlData> CloudIsland_Data = new List<RoomXmlData>();
            List<RoomXmlData> LavaIsland_Data = new List<RoomXmlData>();




            //this will become roomdata per island/dungeon/theme later on
            List<RoomXmlData> roomData = new List<RoomXmlData>();
            string[] filePaths = Directory.GetFiles(repoDir + @"\RoomData\RoomData", "*.xml");


            #region Get all the data from roomData folder

            for (int i = 0; i < filePaths.Count(); i++)
            {
                RoomXmlData RoomData = new RoomXmlData();
                FileStream stream = new FileStream(filePaths[i], FileMode.Open);
                using (stream)
                {
                    RoomData = (RoomXmlData)serializer.Deserialize(stream);
                    //place roomData onto level or room list based on type



                    //dungeon room data
                    if (
                       RoomData.type == RoomID.Column ||
                       RoomData.type == RoomID.Exit ||
                       RoomData.type == RoomID.Key ||
                       RoomData.type == RoomID.Row ||
                       RoomData.type == RoomID.Square ||

                       RoomData.type == RoomID.ForestIsland_BossRoom ||
                       RoomData.type == RoomID.DeathMountain_BossRoom ||
                       RoomData.type == RoomID.SwampIsland_BossRoom ||

                       RoomData.type == RoomID.ForestIsland_HubRoom ||
                       RoomData.type == RoomID.DeathMountain_HubRoom ||
                       RoomData.type == RoomID.SwampIsland_HubRoom
                       )
                    {
                        roomData.Add(RoomData);
                    }



                    //skull island
                    else if(RoomData.type == RoomID.SkullIsland_Colliseum ||
                        RoomData.type == RoomID.SkullIsland_ColliseumPit ||
                        RoomData.type == RoomID.SkullIsland_ShadowKing ||
                        RoomData.type == RoomID.SkullIsland_Town
                        )
                    {
                        SkullIsland_Data.Add(RoomData);
                    }

                    //death mountain
                    else if (RoomData.type == RoomID.DeathMountain_MainEntrance
                        )
                    {
                        DeathMountain_Data.Add(RoomData);
                    }

                    //forest island
                    else if (RoomData.type == RoomID.ForestIsland_MainEntrance
                        )
                    {
                        ForestIsland_Data.Add(RoomData);
                    }

                    //lava island
                    else if (RoomData.type == RoomID.LavaIsland_MainEntrance
                        )
                    {
                        LavaIsland_Data.Add(RoomData);
                    }

                    //cloud island
                    else if (RoomData.type == RoomID.CloudIsland_MainEntrance
                        )
                    {
                        CloudIsland_Data.Add(RoomData);
                    }

                    //swamp island
                    else if (RoomData.type == RoomID.SwampIsland_MainEntrance
                        )
                    {
                        HauntedSwamps_Data.Add(RoomData);
                    }

                    //thieves den
                    else if (RoomData.type == RoomID.ThievesDen_GateEntrance
                        )
                    {
                        ThievesHideout_Data.Add(RoomData);
                    }

                    

                    //plateau island?
                    





                }
            }

            #endregion



            if (Flags.bootRoutine == BootRoutine.Editor_Level)
            {

                #region Write Island (Level) Data

                writeXMLtoCS(SkullIsland_Data, IslandID.LevelData_SkullIsland);
                writeXMLtoCS(ForestIsland_Data, IslandID.LevelData_ForestIsland);
                writeXMLtoCS(ThievesHideout_Data, IslandID.LevelData_ThievesHideout);
                writeXMLtoCS(DeathMountain_Data, IslandID.LevelData_DeathMountain);
                writeXMLtoCS(HauntedSwamps_Data, IslandID.LevelData_HauntedSwamps);
                writeXMLtoCS(CloudIsland_Data, IslandID.LevelData_CloudIsland);
                writeXMLtoCS(LavaIsland_Data, IslandID.LevelData_LavaIsland);

                #endregion

            }
            else
            {

                #region Write Room Data

                StringBuilder csOutput = new StringBuilder();
                csOutput.Append("using System.Collections.Generic;\n");
                csOutput.Append("\n");
                csOutput.Append("namespace DungeonRun\n");
                csOutput.Append("{\n");

                csOutput.Append("\tpublic static class RoomData\n");
                csOutput.Append("\t{\n");

                //create fields
                csOutput.Append("\t\t//roomData is sorted to lists, based on type\n");
                csOutput.Append("\t\tpublic static List<RoomXmlData> bossRooms = new List<RoomXmlData>();\n");
                csOutput.Append("\t\tpublic static List<RoomXmlData> columnRooms = new List<RoomXmlData>();\n");
                csOutput.Append("\t\tpublic static List<RoomXmlData> exitRooms = new List<RoomXmlData>();\n");
                csOutput.Append("\t\tpublic static List<RoomXmlData> hubRooms = new List<RoomXmlData>();\n");
                csOutput.Append("\t\tpublic static List<RoomXmlData> keyRooms = new List<RoomXmlData>();\n");
                csOutput.Append("\t\tpublic static List<RoomXmlData> rowRooms = new List<RoomXmlData>();\n");
                csOutput.Append("\t\tpublic static List<RoomXmlData> secretRooms = new List<RoomXmlData>();\n");
                csOutput.Append("\t\tpublic static List<RoomXmlData> squareRooms = new List<RoomXmlData>();\n");
                csOutput.Append("\n");

                //populate fields
                csOutput.Append("\t\tstatic RoomData()\n");
                csOutput.Append("\t\t{\n");

                for (int i = 0; i < roomData.Count(); i++)
                {
                    Debug.WriteLine("writing " + roomData[i].type + " " + i + " of " + (roomData.Count-1));

                    csOutput.Append("\n");
                    csOutput.Append("\t\t\t#region Room - " + roomData[i].type + "\n\n");
                    csOutput.Append("\t\t\t{\n");

                    csOutput.Append("\t\t\t\tRoomXmlData room = new RoomXmlData();\n");
                    csOutput.Append("\t\t\t\t"); //transfer (preserve) RoomID - set this by hand in XML
                    csOutput.Append("room.type = RoomID." + roomData[i].type + ";\n");

                    //csOutput += "\t\t\t\troom.inds = new List<IndObjXmlData>();\n";
                    //csOutput += "\t\t\t\troom.ints = new List<IntObjXmlData>();\n";

                    for (int g = 0; g < roomData[i].inds.Count(); g++)
                    {
                        csOutput.Append("\t\t\t\t{");
                        csOutput.Append("IndObjXmlData Obj = new IndObjXmlData(); ");
                        csOutput.Append("Obj.type = IndestructibleType." + roomData[i].inds[g].type + "; ");
                        csOutput.Append("Obj.direction = Direction." + roomData[i].inds[g].direction + "; ");
                        csOutput.Append("Obj.posX = " + roomData[i].inds[g].posX + "; ");
                        csOutput.Append("Obj.posY = " + roomData[i].inds[g].posY + "; ");
                        csOutput.Append("room.inds.Add(Obj);");
                        csOutput.Append("}\n");
                    }

                    for (int g = 0; g < roomData[i].ints.Count(); g++)
                    {
                        csOutput.Append("\t\t\t\t{");
                        csOutput.Append("IntObjXmlData Obj = new IntObjXmlData(); ");
                        csOutput.Append("Obj.type = InteractiveType." + roomData[i].ints[g].type + "; ");
                        csOutput.Append("Obj.direction = Direction." + roomData[i].ints[g].direction + "; ");
                        csOutput.Append("Obj.posX = " + roomData[i].ints[g].posX + "; ");
                        csOutput.Append("Obj.posY = " + roomData[i].ints[g].posY + "; ");
                        csOutput.Append("room.ints.Add(Obj);");
                        csOutput.Append("}\n");
                    }




                    if (
                        roomData[i].type == RoomID.ForestIsland_BossRoom ||
                        roomData[i].type == RoomID.DeathMountain_BossRoom ||
                        roomData[i].type == RoomID.SwampIsland_BossRoom
                        )
                    { csOutput.Append("\t\t\t\tbossRooms.Add(room);\n"); }
                    else if (
                        roomData[i].type == RoomID.ForestIsland_HubRoom ||
                        roomData[i].type == RoomID.DeathMountain_HubRoom ||
                        roomData[i].type == RoomID.SwampIsland_HubRoom
                        )
                    { csOutput.Append("\t\t\t\thubRooms.Add(room);\n"); }

                    else if (roomData[i].type == RoomID.Column)
                    { csOutput.Append("\t\t\t\tcolumnRooms.Add(room);\n"); }
                    else if (roomData[i].type == RoomID.Exit)
                    { csOutput.Append("\t\t\t\texitRooms.Add(room);\n"); }
                    else if (roomData[i].type == RoomID.Key)
                    { csOutput.Append("\t\t\t\tkeyRooms.Add(room);\n"); }
                    else if (roomData[i].type == RoomID.Row)
                    { csOutput.Append("\t\t\t\trowRooms.Add(room);\n"); }
                    else if (roomData[i].type == RoomID.Square)
                    { csOutput.Append("\t\t\t\tsquareRooms.Add(room);\n"); }

                    csOutput.Append("\t\t\t}\n");
                    csOutput.Append("\t\t\t#endregion\n\n");
                }

                csOutput.Append("\t\t}\n");
                csOutput.Append("\t}\n");
                csOutput.Append("}");

                string roomAddress = repoDir + @"\GameClasses\AssetsRoomData.cs";
                //File.Create(roomAddress).Dispose();
                File.WriteAllText(roomAddress, csOutput.ToString());

                #endregion


            }

            Debug.WriteLine("Xml to CS conversion done.");
        }

        //saves the current room/levels data to xml file
        public static void SaveRoomData(RoomXmlData RoomData)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = "roomData.xml";
            savefile.Filter = "RoomData XML (*.xml)|*.xml";
            //dont set initial directory, defaults to last directory used
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter stream = new StreamWriter(savefile.FileName))
                { serializer.Serialize(stream, RoomData); }
            }
        }

        //selects a room/level xml file and loads it into editor
        public static void SelectRoomFile()
        {
            Stream stream = null;
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Title = "Open RoomData XML File";
            openDialog.Filter = "RoomData XML files (*.xml)|*.xml";
            //dont set initial directory, defaults to last directory used
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((stream = openDialog.OpenFile()) != null)
                    {
                        using (stream)
                        { Widgets.RoomTools.roomData = (RoomXmlData)serializer.Deserialize(stream); }
                        //store filename in RoomTools widget
                        Widgets.RoomTools.window.title.text = openDialog.SafeFileName;
                        //build the loaded roomData
                        Widgets.RoomTools.BuildFromFile(Widgets.RoomTools.roomData);
                    }
                }
                catch (Exception ex)
                {
                    if (Flags.PrintOutput) { Debug.WriteLine("Error: " + ex.Message); }
                    MessageBox.Show("Error: Could not read file from disk. Error: " + ex.Message);
                }
            }
        }

    }
}