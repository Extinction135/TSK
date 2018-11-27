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
        static string localFolder = AppDomain.CurrentDomain.BaseDirectory;
        //static string localFolder = Environment.CurrentDirectory; //same address
        static string filename;

        static string repoDir = @"C:\Users\Gx000000\Desktop\REPOs\TheShadowKing\TSK";




        //this was created to write overworld islandsets (lists of levels/rooms)
        public static void writeXMLtoCS(List<RoomXmlData> Levels, IslandID islandID)
        {
            Debug.WriteLine("writing " + islandID + " XML to CS... wait...");


            #region Build C# output string to write

            string csOutput = "";
            csOutput += "using System.Collections.Generic;\n";
            csOutput += "\n";
            csOutput += "namespace DungeonRun\n";
            csOutput += "{\n";

            csOutput += "\tpublic static class " + islandID + "\n";
            csOutput += "\t{\n";

            //field defs
            csOutput += "\t\t//levels specific to this island\n";
            for (int i = 0; i < Levels.Count(); i++)
            {
                csOutput += "\t\tpublic static RoomXmlData " + Levels[i].type + " = new RoomXmlData();\n";
            }
            csOutput += "\n";

            //populate fields
            csOutput += "\t\t//level data\n";
            csOutput += "\t\tstatic " + islandID + "()\n";
            csOutput += "\t\t{\n";

            for (int i = 0; i < Levels.Count(); i++)
            {
                Debug.WriteLine("writing level " + i + " of " + (Levels.Count - 1));

                csOutput += "\n";
                csOutput += "\t\t\t#region " + Levels[i].type + "\n\n";

                csOutput += "\t\t\t" + Levels[i].type + ".type = RoomID." + Levels[i].type + ";\n"; //lol
                csOutput += "\t\t\t" + Levels[i].type + ".objs = new List<ObjXmlData>();\n";

                for (int g = 0; g < Levels[i].objs.Count(); g++)
                {
                    csOutput += "\t\t\t{";

                    csOutput += "ObjXmlData obj = new ObjXmlData(); ";

                    csOutput += "obj.type = ObjType." + Levels[i].objs[g].type + "; ";
                    csOutput += "obj.direction = Direction." + Levels[i].objs[g].direction + "; ";
                    csOutput += "obj.posX = " + Levels[i].objs[g].posX + "; ";
                    csOutput += "obj.posY = " + Levels[i].objs[g].posY + "; ";

                    csOutput += "" + Levels[i].type + ".objs.Add(obj);";

                    csOutput += "}\n";
                }

                csOutput += "\t\t\t#endregion\n";
                csOutput += "\n";
            }
            csOutput += "\t\t}\n";
            csOutput += "\t}\n";
            csOutput += "}";

            #endregion


            string islandAddress = repoDir + @"\GameClasses\" + islandID + @".cs";
            File.WriteAllText(islandAddress, csOutput);
        }


        //this writes BOTH overworld level data and room data
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

                string csOutput = "";
                csOutput += "using System.Collections.Generic;\n";
                csOutput += "\n";
                csOutput += "namespace DungeonRun\n";
                csOutput += "{\n";

                csOutput += "\tpublic static class RoomData\n";
                csOutput += "\t{\n";

                //create fields
                csOutput += "\t\t//roomData is sorted to lists, based on type\n";
                csOutput += "\t\tpublic static List<RoomXmlData> bossRooms = new List<RoomXmlData>();\n";
                csOutput += "\t\tpublic static List<RoomXmlData> columnRooms = new List<RoomXmlData>();\n";
                csOutput += "\t\tpublic static List<RoomXmlData> exitRooms = new List<RoomXmlData>();\n";
                csOutput += "\t\tpublic static List<RoomXmlData> hubRooms = new List<RoomXmlData>();\n";
                csOutput += "\t\tpublic static List<RoomXmlData> keyRooms = new List<RoomXmlData>();\n";
                csOutput += "\t\tpublic static List<RoomXmlData> rowRooms = new List<RoomXmlData>();\n";
                csOutput += "\t\tpublic static List<RoomXmlData> secretRooms = new List<RoomXmlData>();\n";
                csOutput += "\t\tpublic static List<RoomXmlData> squareRooms = new List<RoomXmlData>();\n";
                csOutput += "\n";

                //populate fields
                csOutput += "\t\tstatic RoomData()\n";
                csOutput += "\t\t{\n";

                for (int i = 0; i < roomData.Count(); i++)
                {
                    Debug.WriteLine("writing ROOM " + i + " of " + (roomData.Count-1));

                    csOutput += "\n";
                    csOutput += "\t\t\t#region Room - " + roomData[i].type + "\n\n";
                    csOutput += "\t\t\t{\n";

                    csOutput += "\t\t\t\tRoomXmlData room = new RoomXmlData();\n";
                    csOutput += "\t\t\t\t"; //transfer (preserve) RoomID - set this by hand in XML
                    csOutput += "room.type = RoomID." + roomData[i].type + ";\n";

                    csOutput += "\t\t\t\troom.objs = new List<ObjXmlData>();\n";
                    for (int g = 0; g < roomData[i].objs.Count(); g++)
                    {
                        csOutput += "\t\t\t\t{";
                        csOutput += "ObjXmlData Obj = new ObjXmlData(); ";
                        csOutput += "Obj.type = ObjType." + roomData[i].objs[g].type + "; ";
                        csOutput += "Obj.direction = Direction." + roomData[i].objs[g].direction + "; ";
                        csOutput += "Obj.posX = " + roomData[i].objs[g].posX + "; ";
                        csOutput += "Obj.posY = " + roomData[i].objs[g].posY + "; ";
                        csOutput += "room.objs.Add(Obj);";
                        csOutput += "}\n";
                    }

                    if (
                        roomData[i].type == RoomID.ForestIsland_BossRoom ||
                        roomData[i].type == RoomID.DeathMountain_BossRoom ||
                        roomData[i].type == RoomID.SwampIsland_BossRoom
                        )
                    { csOutput += "\t\t\t\t bossRooms.Add(room);\n"; }
                    else if (
                        roomData[i].type == RoomID.ForestIsland_HubRoom ||
                        roomData[i].type == RoomID.DeathMountain_HubRoom ||
                        roomData[i].type == RoomID.SwampIsland_HubRoom
                        )
                    { csOutput += "\t\t\t\t hubRooms.Add(room);\n"; }

                    else if (roomData[i].type == RoomID.Column)
                    { csOutput += "\t\t\t\t columnRooms.Add(room);\n"; }
                    else if (roomData[i].type == RoomID.Exit)
                    { csOutput += "\t\t\t\t exitRooms.Add(room);\n"; }
                    else if (roomData[i].type == RoomID.Key)
                    { csOutput += "\t\t\t\t keyRooms.Add(room);\n"; }
                    else if (roomData[i].type == RoomID.Row)
                    { csOutput += "\t\t\t\t rowRooms.Add(room);\n"; }
                    else if (roomData[i].type == RoomID.Square)
                    { csOutput += "\t\t\t\t squareRooms.Add(room);\n"; }

                    csOutput += "\t\t\t}\n";
                    csOutput += "\t\t\t#endregion\n\n";
                }

                csOutput += "\t\t}\n";
                csOutput += "\t}\n";
                csOutput += "}";

                string roomAddress = @"C:\Users\Gx000000\Desktop\REPOs\DungeonRun\DungeonRun\GameClasses\AssetsRoomData.cs";
                //string csFile = @"C:\Users\Gx000000\Desktop\AssetsRoomData.cs";
                //File.Create(roomAddress).Dispose();
                File.WriteAllText(roomAddress, csOutput);

                #endregion

            }

            Debug.WriteLine("Xml to CS conversion done.");
        }


		
        public static string GetRam()
        {   //get the ram footprint in mb
			return "" + (Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024);
        }

        public static void SetFilename(GameFile Type)
        {
            filename = "autoSave.xml"; //defaults to autoSave
            if (Type == GameFile.Game1) { filename = "game1.xml"; }
            else if (Type == GameFile.Game2) { filename = "game2.xml"; }
            else if (Type == GameFile.Game3) { filename = "game3.xml"; }
            if (Flags.PrintOutput)
            { Debug.WriteLine("folder: " + localFolder + filename); }
        }

        public static void SaveGame(GameFile Type)
        {
            SetFilename(Type);
            //overwrite savefile
            FileStream stream = File.Open(localFolder + filename, FileMode.Create);
            var serializer = new XmlSerializer(typeof(SaveData));
            using (stream) //save the playerData, to saveFile address
            { serializer.Serialize(stream, PlayerData.current); }
        }

        public static void LoadGame(GameFile Type, Boolean loadAsCurrentGame)
        {
            SetFilename(Type);
            //Boolean autoSave = false;
            List<Dialog> dialog = AssetsDialog.Guide;

            try
            {

                #region Load the file into proper saveData instance

                try
                {   //load gameFile into saveData parameter
                    {
                        var serializer = new XmlSerializer(typeof(SaveData));
                        FileStream stream = new FileStream(localFolder + filename, FileMode.Open);

                        using (stream)
                        {
                            if (Type == GameFile.AutoSave)
                            {
                                PlayerData.current = (SaveData)serializer.Deserialize(stream);
                                //autoSave = true;
                            }
                            else if (Type == GameFile.Game1)
                            {
                                PlayerData.game1 = (SaveData)serializer.Deserialize(stream);
                                if (loadAsCurrentGame) { PlayerData.current = PlayerData.game1; }
                            }
                            else if (Type == GameFile.Game2)
                            {
                                PlayerData.game2 = (SaveData)serializer.Deserialize(stream);
                                if (loadAsCurrentGame) { PlayerData.current = PlayerData.game2; }
                            }
                            else if (Type == GameFile.Game3)
                            {
                                PlayerData.game3 = (SaveData)serializer.Deserialize(stream);
                                if (loadAsCurrentGame) { PlayerData.current = PlayerData.game3; }
                            }
                        }
                        //if (autoSave) //let player know file has been loaded
                        //{ dialog = AssetsDialog.GameAutoSaved; }
                        //else { dialog = AssetsDialog.GameLoaded; }
                        dialog = AssetsDialog.GameLoaded;
                    }
                }

                #endregion


                #region Handle file loading failure

                catch (Exception ex)
                {   //create dialog screen alerting user there was problem loading file
                    if (Flags.PrintOutput) { Debug.WriteLine("load game file error: " + ex.Message); }
                    //overwrite any corrupt autosave data
                    SaveGame(GameFile.AutoSave);
                    //overwrite any corrupt game file with current game data
                    if (Type == GameFile.Game1) { SaveGame(GameFile.Game1); }
                    else if (Type == GameFile.Game2) { SaveGame(GameFile.Game2); }
                    else if (Type == GameFile.Game3) { SaveGame(GameFile.Game3); }
                    //notify player of this event
                    dialog = AssetsDialog.GameLoadFailed;
                }

                #endregion

            }
            catch (Exception ex) //file does not exist, cannot be loaded, save the current data to file address
            {
                if (Flags.PrintOutput) { Debug.WriteLine("file does not exist. error: " + ex.Message); }
                SaveGame(Type); dialog = AssetsDialog.GameNotFound;
            }
            //if loaded data is current game, notify player of loading via dialog screen
            if (loadAsCurrentGame)
            {
                Screens.Dialog.SetDialog(dialog);
                ScreenManager.AddScreen(Screens.Dialog);
            }
            //Functions_Debug.Inspect(PlayerData.saveData);
        }



        static XmlSerializer serializer = new XmlSerializer(typeof(RoomXmlData));

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



        /*
        public static void LoadAllRoomData()
        {
            List<String> roomDataFiles = Directory.GetFiles(
                localFolder + "\\RoomData", "*.xml", 
                SearchOption.AllDirectories
                ).ToList();

            if (Flags.PrintOutput) { Debug.WriteLine("loading room data - total:" + roomDataFiles.Count); }
            for(int i = 0; i < roomDataFiles.Count; i++)
            {
                //if (Flags.PrintOutput) { Debug.WriteLine("filepath: " + roomDataFiles[i]); }
                RoomXmlData RoomData = new RoomXmlData();
                FileStream stream = new FileStream(roomDataFiles[i], FileMode.Open);
                using (stream)
                { RoomData = (RoomXmlData)serializer.Deserialize(stream); }

                //place the loaded roomData into the correct Assets list
                if (RoomData.type == RoomID.Boss) { Assets.roomDataBoss.Add(RoomData); }
                else if (RoomData.type == RoomID.Column) { Assets.roomDataColumn.Add(RoomData); }
                else if (RoomData.type == RoomID.Hub) { Assets.roomDataHub.Add(RoomData); }
                else if (RoomData.type == RoomID.Key) { Assets.roomDataKey.Add(RoomData); }
                else if (RoomData.type == RoomID.Row) { Assets.roomDataRow.Add(RoomData); }
                else if (RoomData.type == RoomID.Square) { Assets.roomDataSquare.Add(RoomData); }
                //if roomData isn't dungeon, then it defaults to overworld level list
                else { Assets.overworldLevels.Add(RoomData); };
            }
            if (Flags.PrintOutput) { Functions_Debug.InspectRoomData(); }
        }
        */





    }
}