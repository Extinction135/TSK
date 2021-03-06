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









        //this writes a list of roomXml data to a .cs file in the main GameClasses directory
        public static void writeXMLtoCS(List<RoomXmlData> Levels, IslandID islandID, Boolean levelData)
        {
            Debug.WriteLine("writing " + islandID + " XML to CS... wait...");
            StringBuilder csOutput = new StringBuilder();


            if(levelData)
            {

                #region Write data in level format (save wind)

                csOutput.Append("using System.Collections.Generic;\n");
                csOutput.Append("\n");
                csOutput.Append("namespace DungeonRun\n");
                csOutput.Append("{\n");

                csOutput.Append("\tpublic static class " + islandID + "\n");
                csOutput.Append("\t{\n");

                //field defs
                csOutput.Append("\t\t//levelData specific to this island\n");
                for (int i = 0; i < Levels.Count(); i++)
                {
                    csOutput.Append("\t\tpublic static RoomXmlData " + Levels[i].type + " = new RoomXmlData();\n");
                }
                csOutput.Append("\n");

                //populate fields
                csOutput.Append("\t\t//data is added here\n");
                csOutput.Append("\t\tstatic " + islandID + "()\n");
                csOutput.Append("\t\t{\n");

                for (int i = 0; i < Levels.Count(); i++)
                {
                    Debug.WriteLine("writing level " + i + " of " + (Levels.Count - 1));

                    csOutput.Append("\n");
                    csOutput.Append("\t\t\t#region " + Levels[i].type + "\n\n");

                    csOutput.Append("\t\t\t" + Levels[i].type + ".type = RoomID." + Levels[i].type + ";\n"); //lol
                    //save wind
                    csOutput.Append("\t\t\t" + Levels[i].type + ".windDirection = Direction." + Levels[i].windDirection + ";\n");
                    csOutput.Append("\t\t\t" + Levels[i].type + ".windFrequency = " + Levels[i].windFrequency + ";\n");
                    csOutput.Append("\t\t\t" + Levels[i].type + ".windIntensity = " + Levels[i].windIntensity + ";\n");

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

            }

            else
            {

                #region Write data in room format (reset wind)

                csOutput.Append("using System.Collections.Generic;\n");
                csOutput.Append("\n");
                csOutput.Append("namespace DungeonRun\n");
                csOutput.Append("{\n");

                csOutput.Append("\tpublic static class " + islandID + "\n");
                csOutput.Append("\t{\n");

                //define the data list
                csOutput.Append("\t\t//data is added to this list\n");
                csOutput.Append("\t\tpublic static List<RoomXmlData> Data = new List<RoomXmlData>();\n");
                csOutput.Append("\t\t//data uses this reference\n");
                csOutput.Append("\t\tpublic static RoomXmlData dataRef;\n");

                //populate fields
                csOutput.Append("\t\t//data is added in the constructor below\n");
                csOutput.Append("\t\tstatic " + islandID + "()\n");
                csOutput.Append("\t\t{\n");

                for (int i = 0; i < Levels.Count(); i++)
                {
                    Debug.WriteLine("writing level " + i + " of " + (Levels.Count - 1));

                    csOutput.Append("\n");
                    csOutput.Append("\t\t\t#region " + Levels[i].type + "\n\n");
                    
                    csOutput.Append("\t\t\tdataRef = new RoomXmlData();\n");
                    csOutput.Append("\t\t\tdataRef.type = RoomID." + Levels[i].type + ";\n");
                    //this is no direction/intensity wind field values
                    csOutput.Append("\t\t\tdataRef.windDirection = Direction.None;\n");
                    csOutput.Append("\t\t\tdataRef.windFrequency = 2;\n");
                    csOutput.Append("\t\t\tdataRef.windIntensity = 0;\n");

                    for (int g = 0; g < Levels[i].inds.Count(); g++)
                    {
                        csOutput.Append("\t\t\t{");

                        csOutput.Append("IndObjXmlData obj = new IndObjXmlData(); ");

                        csOutput.Append("obj.type = IndestructibleType." + Levels[i].inds[g].type + "; ");
                        csOutput.Append("obj.direction = Direction." + Levels[i].inds[g].direction + "; ");
                        csOutput.Append("obj.posX = " + Levels[i].inds[g].posX + "; ");
                        csOutput.Append("obj.posY = " + Levels[i].inds[g].posY + "; ");

                        csOutput.Append("dataRef.inds.Add(obj);");

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

                        csOutput.Append("dataRef.ints.Add(obj);");

                        csOutput.Append("}\n");
                    }
                    csOutput.Append("\t\t\t//add the ref to the data list\n");
                    csOutput.Append("\t\t\tData.Add(dataRef);\n");

                    csOutput.Append("\t\t\t#endregion\n");
                    csOutput.Append("\n");
                }
                csOutput.Append("\t\t}\n");
                csOutput.Append("\t}\n");
                csOutput.Append("}");


                #endregion

            }


            string islandAddress = repoDir + @"\GameClasses\" + islandID + @".cs";
            File.WriteAllText(islandAddress, csOutput.ToString());
        }

        //this collects all xml data, sorts it to lists, then writes it using above method
        public static void ConvertXMLtoCS()
        {
            Debug.WriteLine("Beginning XML to CS conversion... wait...");
            string[] filePaths = Directory.GetFiles(repoDir + @"\RoomData\RoomData", "*.xml");

            //level data lists
            List<RoomXmlData> SkullIsland_LevelData = new List<RoomXmlData>();
            List<RoomXmlData> ForestIsland_Data = new List<RoomXmlData>();
            List<RoomXmlData> ThievesHideout_Data = new List<RoomXmlData>();
            List<RoomXmlData> DeathMountain_Data = new List<RoomXmlData>();
            List<RoomXmlData> HauntedSwamps_Data = new List<RoomXmlData>();
            List<RoomXmlData> CloudIsland_Data = new List<RoomXmlData>();
            List<RoomXmlData> LavaIsland_Data = new List<RoomXmlData>();

            //dungeon room data lists
            List<RoomXmlData> ForestIsland_RoomData_Column = new List<RoomXmlData>();
            List<RoomXmlData> ForestIsland_RoomData_Row = new List<RoomXmlData>();
            List<RoomXmlData> ForestIsland_RoomData_Square = new List<RoomXmlData>();
            List<RoomXmlData> ForestIsland_RoomData_Key = new List<RoomXmlData>();
            List<RoomXmlData> ForestIsland_RoomData_ExitBossHub = new List<RoomXmlData>();

            List<RoomXmlData> DeathMountain_RoomData_Column = new List<RoomXmlData>();
            List<RoomXmlData> DeathMountain_RoomData_Row = new List<RoomXmlData>();
            List<RoomXmlData> DeathMountain_RoomData_Square = new List<RoomXmlData>();
            List<RoomXmlData> DeathMountain_RoomData_Key = new List<RoomXmlData>();
            List<RoomXmlData> DeathMountain_RoomData_ExitBossHub = new List<RoomXmlData>();

            List<RoomXmlData> HauntedSwamps_RoomData_Column = new List<RoomXmlData>();
            List<RoomXmlData> HauntedSwamps_RoomData_Row = new List<RoomXmlData>();
            List<RoomXmlData> HauntedSwamps_RoomData_Square = new List<RoomXmlData>();
            List<RoomXmlData> HauntedSwamps_RoomData_Key = new List<RoomXmlData>();
            List<RoomXmlData> HauntedSwamps_RoomData_ExitBossHub = new List<RoomXmlData>();

            List<RoomXmlData> ThievesHideout_RoomData_Column = new List<RoomXmlData>();
            List<RoomXmlData> ThievesHideout_RoomData_Row = new List<RoomXmlData>();
            List<RoomXmlData> ThievesHideout_RoomData_Square = new List<RoomXmlData>();
            List<RoomXmlData> ThievesHideout_RoomData_Key = new List<RoomXmlData>();
            List<RoomXmlData> ThievesHideout_RoomData_ExitBossHub = new List<RoomXmlData>();

            List<RoomXmlData> LavaIsland_RoomData_Column = new List<RoomXmlData>();
            List<RoomXmlData> LavaIsland_RoomData_Row = new List<RoomXmlData>();
            List<RoomXmlData> LavaIsland_RoomData_Square = new List<RoomXmlData>();
            List<RoomXmlData> LavaIsland_RoomData_Key = new List<RoomXmlData>();
            List<RoomXmlData> LavaIsland_RoomData_ExitBossHub = new List<RoomXmlData>();

            List<RoomXmlData> CloudIsland_RoomData_Column = new List<RoomXmlData>();
            List<RoomXmlData> CloudIsland_RoomData_Row = new List<RoomXmlData>();
            List<RoomXmlData> CloudIsland_RoomData_Square = new List<RoomXmlData>();
            List<RoomXmlData> CloudIsland_RoomData_Key = new List<RoomXmlData>();
            List<RoomXmlData> CloudIsland_RoomData_ExitBossHub = new List<RoomXmlData>();

            List<RoomXmlData> SkullIsland_RoomData_Column = new List<RoomXmlData>();
            List<RoomXmlData> SkullIsland_RoomData_Row = new List<RoomXmlData>();
            List<RoomXmlData> SkullIsland_RoomData_Square = new List<RoomXmlData>();
            List<RoomXmlData> SkullIsland_RoomData_Key = new List<RoomXmlData>();
            List<RoomXmlData> SkullIsland_RoomData_ExitBossHub = new List<RoomXmlData>();







            #region Get all the data from roomData folder

            for (int i = 0; i < filePaths.Count(); i++)
            {
                RoomXmlData RoomData = new RoomXmlData();
                FileStream stream = new FileStream(filePaths[i], FileMode.Open);
                using (stream)
                {
                    RoomData = (RoomXmlData)serializer.Deserialize(stream);
                    //place roomData onto level or room lists based on type/id


                    #region LevelData Sorting

                    //skull island
                    if (
                        RoomData.type == RoomID.SkullIsland_Colliseum ||
                        RoomData.type == RoomID.SkullIsland_ColliseumPit ||
                        RoomData.type == RoomID.SkullIsland_ShadowKing ||
                        RoomData.type == RoomID.SkullIsland_Town
                        )
                    {
                        SkullIsland_LevelData.Add(RoomData);
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

                    #endregion


                    #region Dungeon RoomData Sorting

                    //forest set
                    else if (RoomData.type == RoomID.ForestIsland_ColumnRoom)
                    { ForestIsland_RoomData_Column.Add(RoomData); }
                    else if (RoomData.type == RoomID.ForestIsland_RowRoom)
                    { ForestIsland_RoomData_Row.Add(RoomData); }
                    else if (RoomData.type == RoomID.ForestIsland_SquareRoom)
                    { ForestIsland_RoomData_Square.Add(RoomData); }
                    else if (RoomData.type == RoomID.ForestIsland_KeyRoom)
                    { ForestIsland_RoomData_Key.Add(RoomData); }
                    else if (
                        RoomData.type == RoomID.ForestIsland_BossRoom
                        || RoomData.type == RoomID.ForestIsland_HubRoom
                        || RoomData.type == RoomID.ForestIsland_ExitRoom
                        )
                    {
                        ForestIsland_RoomData_ExitBossHub.Add(RoomData);
                    }

                    //death mountain set
                    else if (RoomData.type == RoomID.DeathMountain_ColumnRoom)
                    { DeathMountain_RoomData_Column.Add(RoomData); }
                    else if (RoomData.type == RoomID.DeathMountain_RowRoom)
                    { DeathMountain_RoomData_Row.Add(RoomData); }
                    else if (RoomData.type == RoomID.DeathMountain_SquareRoom)
                    { DeathMountain_RoomData_Square.Add(RoomData); }
                    else if (RoomData.type == RoomID.DeathMountain_KeyRoom)
                    { DeathMountain_RoomData_Key.Add(RoomData); }
                    else if (
                        RoomData.type == RoomID.DeathMountain_BossRoom
                        || RoomData.type == RoomID.DeathMountain_HubRoom
                        || RoomData.type == RoomID.DeathMountain_ExitRoom
                        )
                    {
                        DeathMountain_RoomData_ExitBossHub.Add(RoomData);
                    }

                    //haunted swamps set
                    else if (RoomData.type == RoomID.HauntedSwamps_ColumnRoom)
                    { HauntedSwamps_RoomData_Column.Add(RoomData); }
                    else if (RoomData.type == RoomID.HauntedSwamps_RowRoom)
                    { HauntedSwamps_RoomData_Row.Add(RoomData); }
                    else if (RoomData.type == RoomID.HauntedSwamps_SquareRoom)
                    { HauntedSwamps_RoomData_Square.Add(RoomData); }
                    else if (RoomData.type == RoomID.HauntedSwamps_KeyRoom)
                    { HauntedSwamps_RoomData_Key.Add(RoomData); }
                    else if (
                        RoomData.type == RoomID.HauntedSwamps_BossRoom
                        || RoomData.type == RoomID.HauntedSwamps_HubRoom
                        || RoomData.type == RoomID.HauntedSwamps_ExitRoom
                        )
                    {
                        HauntedSwamps_RoomData_ExitBossHub.Add(RoomData);
                    }

                    //thieves hideout set
                    else if (RoomData.type == RoomID.ThievesHideout_ColumnRoom)
                    { ThievesHideout_RoomData_Column.Add(RoomData); }
                    else if (RoomData.type == RoomID.ThievesHideout_RowRoom)
                    { ThievesHideout_RoomData_Row.Add(RoomData); }
                    else if (RoomData.type == RoomID.ThievesHideout_SquareRoom)
                    { ThievesHideout_RoomData_Square.Add(RoomData); }
                    else if (RoomData.type == RoomID.ThievesHideout_KeyRoom)
                    { ThievesHideout_RoomData_Key.Add(RoomData); }
                    else if (
                        RoomData.type == RoomID.ThievesHideout_BossRoom
                        || RoomData.type == RoomID.ThievesHideout_HubRoom
                        || RoomData.type == RoomID.ThievesHideout_ExitRoom
                        )
                    {
                        ThievesHideout_RoomData_ExitBossHub.Add(RoomData);
                    }

                    //lava island set
                    else if (RoomData.type == RoomID.LavaIsland_ColumnRoom)
                    { LavaIsland_RoomData_Column.Add(RoomData); }
                    else if (RoomData.type == RoomID.LavaIsland_RowRoom)
                    { LavaIsland_RoomData_Row.Add(RoomData); }
                    else if (RoomData.type == RoomID.LavaIsland_SquareRoom)
                    { LavaIsland_RoomData_Square.Add(RoomData); }
                    else if (RoomData.type == RoomID.LavaIsland_KeyRoom)
                    { LavaIsland_RoomData_Key.Add(RoomData); }
                    else if (
                        RoomData.type == RoomID.LavaIsland_BossRoom
                        || RoomData.type == RoomID.LavaIsland_HubRoom
                        || RoomData.type == RoomID.LavaIsland_ExitRoom
                        )
                    {
                        LavaIsland_RoomData_ExitBossHub.Add(RoomData);
                    }

                    //cloud island set
                    else if (RoomData.type == RoomID.CloudIsland_ColumnRoom)
                    { CloudIsland_RoomData_Column.Add(RoomData); }
                    else if (RoomData.type == RoomID.CloudIsland_RowRoom)
                    { CloudIsland_RoomData_Row.Add(RoomData); }
                    else if (RoomData.type == RoomID.CloudIsland_SquareRoom)
                    { CloudIsland_RoomData_Square.Add(RoomData); }
                    else if (RoomData.type == RoomID.CloudIsland_KeyRoom)
                    { CloudIsland_RoomData_Key.Add(RoomData); }
                    else if (
                        RoomData.type == RoomID.CloudIsland_BossRoom
                        || RoomData.type == RoomID.CloudIsland_HubRoom
                        || RoomData.type == RoomID.CloudIsland_ExitRoom
                        )
                    {
                        CloudIsland_RoomData_ExitBossHub.Add(RoomData);
                    }

                    //skull island set
                    else if (RoomData.type == RoomID.SkullIsland_ColumnRoom)
                    { SkullIsland_RoomData_Column.Add(RoomData); }
                    else if (RoomData.type == RoomID.SkullIsland_RowRoom)
                    { SkullIsland_RoomData_Row.Add(RoomData); }
                    else if (RoomData.type == RoomID.SkullIsland_SquareRoom)
                    { SkullIsland_RoomData_Square.Add(RoomData); }
                    else if (RoomData.type == RoomID.SkullIsland_KeyRoom)
                    { SkullIsland_RoomData_Key.Add(RoomData); }
                    else if (
                        RoomData.type == RoomID.SkullIsland_BossRoom
                        || RoomData.type == RoomID.SkullIsland_HubRoom
                        || RoomData.type == RoomID.SkullIsland_ExitRoom
                        )
                    {
                        SkullIsland_RoomData_ExitBossHub.Add(RoomData);
                    }

                    #endregion


                }
            }

            #endregion






            //write level data (save wind)
            writeXMLtoCS(SkullIsland_LevelData, IslandID.LevelData_SkullIsland, true);
            writeXMLtoCS(ForestIsland_Data, IslandID.LevelData_ForestIsland, true);
            writeXMLtoCS(ThievesHideout_Data, IslandID.LevelData_ThievesHideout, true);
            writeXMLtoCS(DeathMountain_Data, IslandID.LevelData_DeathMountain, true);
            writeXMLtoCS(HauntedSwamps_Data, IslandID.LevelData_HauntedSwamps, true);
            writeXMLtoCS(CloudIsland_Data, IslandID.LevelData_CloudIsland, true);
            writeXMLtoCS(LavaIsland_Data, IslandID.LevelData_LavaIsland, true);

            //write room data (reset wind)
            writeXMLtoCS(ForestIsland_RoomData_Column, IslandID.RoomData_ForestIsland_Columns, false);
            writeXMLtoCS(ForestIsland_RoomData_Row, IslandID.RoomData_ForestIsland_Row, false);
            writeXMLtoCS(ForestIsland_RoomData_Square, IslandID.RoomData_ForestIsland_Square, false);
            writeXMLtoCS(ForestIsland_RoomData_Key, IslandID.RoomData_ForestIsland_Key, false);
            writeXMLtoCS(ForestIsland_RoomData_ExitBossHub, IslandID.RoomData_ForestIsland_ExitBossHub, false);

            writeXMLtoCS(DeathMountain_RoomData_Column, IslandID.RoomData_DeathMountain_Columns, false);
            writeXMLtoCS(DeathMountain_RoomData_Row, IslandID.RoomData_DeathMountain_Row, false);
            writeXMLtoCS(DeathMountain_RoomData_Square, IslandID.RoomData_DeathMountain_Square, false);
            writeXMLtoCS(DeathMountain_RoomData_Key, IslandID.RoomData_DeathMountain_Key, false);
            writeXMLtoCS(DeathMountain_RoomData_ExitBossHub, IslandID.RoomData_DeathMountain_ExitBossHub, false);

            writeXMLtoCS(HauntedSwamps_RoomData_Column, IslandID.RoomData_HauntedSwamps_Columns, false);
            writeXMLtoCS(HauntedSwamps_RoomData_Row, IslandID.RoomData_HauntedSwamps_Row, false);
            writeXMLtoCS(HauntedSwamps_RoomData_Square, IslandID.RoomData_HauntedSwamps_Square, false);
            writeXMLtoCS(HauntedSwamps_RoomData_Key, IslandID.RoomData_HauntedSwamps_Key, false);
            writeXMLtoCS(HauntedSwamps_RoomData_ExitBossHub, IslandID.RoomData_HauntedSwamps_ExitBossHub, false);

            writeXMLtoCS(ThievesHideout_RoomData_Column, IslandID.RoomData_ThievesHideout_Columns, false);
            writeXMLtoCS(ThievesHideout_RoomData_Row, IslandID.RoomData_ThievesHideout_Row, false);
            writeXMLtoCS(ThievesHideout_RoomData_Square, IslandID.RoomData_ThievesHideout_Square, false);
            writeXMLtoCS(ThievesHideout_RoomData_Key, IslandID.RoomData_ThievesHideout_Key, false);
            writeXMLtoCS(ThievesHideout_RoomData_ExitBossHub, IslandID.RoomData_ThievesHideout_ExitBossHub, false);

            writeXMLtoCS(LavaIsland_RoomData_Column, IslandID.RoomData_LavaIsland_Columns, false);
            writeXMLtoCS(LavaIsland_RoomData_Row, IslandID.RoomData_LavaIsland_Row, false);
            writeXMLtoCS(LavaIsland_RoomData_Square, IslandID.RoomData_LavaIsland_Square, false);
            writeXMLtoCS(LavaIsland_RoomData_Key, IslandID.RoomData_LavaIsland_Key, false);
            writeXMLtoCS(LavaIsland_RoomData_ExitBossHub, IslandID.RoomData_LavaIsland_ExitBossHub, false);

            writeXMLtoCS(CloudIsland_RoomData_Column, IslandID.RoomData_CloudIsland_Columns, false);
            writeXMLtoCS(CloudIsland_RoomData_Row, IslandID.RoomData_CloudIsland_Row, false);
            writeXMLtoCS(CloudIsland_RoomData_Square, IslandID.RoomData_CloudIsland_Square, false);
            writeXMLtoCS(CloudIsland_RoomData_Key, IslandID.RoomData_CloudIsland_Key, false);
            writeXMLtoCS(CloudIsland_RoomData_ExitBossHub, IslandID.RoomData_CloudIsland_ExitBossHub, false);

            writeXMLtoCS(SkullIsland_RoomData_Column, IslandID.RoomData_SkullIsland_Columns, false);
            writeXMLtoCS(SkullIsland_RoomData_Row, IslandID.RoomData_SkullIsland_Row, false);
            writeXMLtoCS(SkullIsland_RoomData_Square, IslandID.RoomData_SkullIsland_Square, false);
            writeXMLtoCS(SkullIsland_RoomData_Key, IslandID.RoomData_SkullIsland_Key, false);
            writeXMLtoCS(SkullIsland_RoomData_ExitBossHub, IslandID.RoomData_SkullIsland_ExitBossHub, false);

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