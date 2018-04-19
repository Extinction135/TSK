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
            Boolean autoSave = false;
            List<Dialog> dialog = Functions_Dialog.Guide;

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
                                autoSave = true;
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
                        if (autoSave) //let player know file has been loaded
                        { dialog = Functions_Dialog.GameAutoSaved; }
                        else { dialog = Functions_Dialog.GameLoaded; }
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
                    dialog = Functions_Dialog.GameLoadFailed;
                }

                #endregion

            }
            catch (Exception ex) //file does not exist, cannot be loaded, save the current data to file address
            {
                if (Flags.PrintOutput) { Debug.WriteLine("file does not exist. error: " + ex.Message); }
                SaveGame(Type); dialog = Functions_Dialog.GameNotFound;
            }
            //if loaded data is current game, notify player of loading via dialog screen
            if (loadAsCurrentGame) { ScreenManager.AddScreen(new ScreenDialog(dialog)); }
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

    }
}