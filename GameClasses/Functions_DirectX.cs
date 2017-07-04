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
            //Debug.WriteLine("save/load file: " + localFolder.Path + @"\" + filename);
            Debug.WriteLine("save/load file: " + localFolder + filename);
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
            Dialog dialogType = Dialog.Default;
            try
            {
                //StorageFile file = await localFolder.GetFileAsync(filename); //UWP


                #region Load the file into proper saveData instance

                try
                {   //load gameFile into saveData parameter
                    //if (file != null) //UWP
                    {
                        var serializer = new XmlSerializer(typeof(SaveData));
                        //Stream stream = await file.OpenStreamForReadAsync(); //UWP
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
                        { dialogType = Dialog.GameAutoSaved; }
                        else { dialogType = Dialog.GameLoaded; }
                    }
                }

                #endregion


                #region Handle file loading failure

                catch
                {   //create dialog screen alerting user there was problem loading file
                    //Debug.WriteLine("problem loading");
                    //overwrite any corrupt autosave data
                    SaveGame(GameFile.AutoSave);
                    //overwrite any corrupt game file with current game data
                    if (Type == GameFile.Game1) { SaveGame(GameFile.Game1); }
                    else if (Type == GameFile.Game2) { SaveGame(GameFile.Game2); }
                    else if (Type == GameFile.Game3) { SaveGame(GameFile.Game3); }
                    //notify player of this event
                    dialogType = Dialog.GameLoadFailed;
                }

                #endregion

            }
            catch //file does not exist, cannot be loaded, save the current data to file address
            {
                //Debug.WriteLine("file does not exist");
                SaveGame(Type); dialogType = Dialog.GameNotFound;
            }
            //if loaded data is current game, notify player of loading via dialog screen
            if (loadAsCurrentGame) { ScreenManager.AddScreen(new ScreenDialog(dialogType)); }
            //Functions_Debug.Inspect(PlayerData.saveData);
            
        }

        public static void LoadRoomData()
        {
            //Debug.WriteLine("local folder: " + ApplicationData.Current.LocalFolder.Path);
            //Debug.WriteLine("install folder: " + Windows.ApplicationModel.Package.Current.InstalledLocation.Path);
            //string roomDataPath = Windows.ApplicationModel.Package.Current.InstalledLocation.Path + @"/RoomData/";

            /*
            StorageFolder appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFolder assets = await appInstalledFolder.GetFolderAsync("RoomData");
            var files = await assets.GetFilesAsync();
            Debug.WriteLine("filename: " + files[0].Path);

            string text = await Windows.Storage.FileIO.ReadTextAsync(files[0]);
            Debug.WriteLine("text file contents: " + text);
            */
        }
    }
}