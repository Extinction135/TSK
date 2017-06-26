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
using Windows.System;

using System.IO;
using Windows.Storage;
using System.Xml.Serialization;


namespace DungeonRun
{
    public static class Functions_Backend
    {
        static StorageFolder localFolder = ApplicationData.Current.LocalFolder;


        public static string GetRam()
        {   //get the ram footprint in mb
			return "" + (MemoryManager.AppMemoryUsage / 1024 / 1024);
        }

        public static async void LoadRoomData()
        {
            //Debug.WriteLine("local folder: " + ApplicationData.Current.LocalFolder.Path);
            //Debug.WriteLine("install folder: " + Windows.ApplicationModel.Package.Current.InstalledLocation.Path);
            //string roomDataPath = Windows.ApplicationModel.Package.Current.InstalledLocation.Path + @"/RoomData/";

            StorageFolder appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFolder assets = await appInstalledFolder.GetFolderAsync("RoomData");
            var files = await assets.GetFilesAsync();
            Debug.WriteLine("filename: " + files[0].Path);

            string text = await Windows.Storage.FileIO.ReadTextAsync(files[0]);
            Debug.WriteLine("text file contents: " + text);
        }

        public static async void LoadPlayerData()
        {
            /*
            //pick the playerData.xml file to load
            var loadPicker = new Windows.Storage.Pickers.FileOpenPicker();
            loadPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            loadPicker.FileTypeFilter.Add(".xml");
            StorageFile loadFile = await loadPicker.PickSingleFileAsync();
            */

            //get the PlayerData folder path
            //StorageFolder appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            //StorageFolder assets = await appInstalledFolder.GetFolderAsync("PlayerData");

            /*
            //load the 1st playerData.xml file from PlayerData folder
            var files = await assets.GetFilesAsync();
            StorageFile loadFile = files[0];
            Debug.WriteLine("loaded playerData: " + files[0].Path);
            */

            //get the autoSave.xml file
            //StorageFile loadFile = await assets.GetFileAsync("autoSave.xml");
            //Debug.WriteLine("loaded playerData: " + loadFile.Path);

            //get the local folder
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            Debug.WriteLine("loading: " + localFolder.Path + @"\autoSave.xml");
            try
            {   //load the autoSave.xml file (but it may not exist, if this is first run of game)
                StorageFile loadFile = await localFolder.GetFileAsync("autoSave.xml");
                //load the file into PlayerData.saveData
                if (loadFile != null)
                {
                    var serializer = new XmlSerializer(typeof(SaveData));
                    Stream stream = await loadFile.OpenStreamForReadAsync();
                    using (stream)
                    { PlayerData.saveData = (SaveData)serializer.Deserialize(stream); }
                    //Functions_Debug.Inspect(PlayerData.saveData);
                }
            }
            catch { }
        }






        public static async void SaveGame(GameFile Type)
        {
            string filename = "autoSave.xml"; //defaults to autoSave
            if (Type == GameFile.Game1) { filename = "game1.xml"; }
            else if (Type == GameFile.Game2) { filename = "game2.xml"; }
            else if (Type == GameFile.Game3) { filename = "game3.xml"; }
            Debug.WriteLine("saving: " + localFolder.Path + @"\" + filename);
            StorageFile saveFile = await localFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            SavePlayerData(saveFile);
        }

        public static async void SavePlayerData(StorageFile saveFile)
        {   //save the playerData, to saveFile address
            var serializer = new XmlSerializer(typeof(SaveData));
            Stream stream = await saveFile.OpenStreamForWriteAsync();
            using (stream)
            { serializer.Serialize(stream, PlayerData.saveData); }
        }
















        public static void SavePlayerData_OLD()
        {
            //Debug.WriteLine("saving: " + localFolder.Path + @"\autoSave.xml");
            //create the autoSave.xml file
            //StorageFile saveFile = await localFolder.CreateFileAsync("autoSave.xml", CreationCollisionOption.ReplaceExisting);

            /*
            //pick the path and name to save the file to
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            savePicker.FileTypeChoices.Add("PlayerDataSave", new List<string>() { ".xml" });
            savePicker.SuggestedFileName = "PlayerDataSave";
            StorageFile saveFile = await savePicker.PickSaveFileAsync();
            */

            /*
            //get the PlayerData folder path
            StorageFolder appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFolder assets = await appInstalledFolder.GetFolderAsync("PlayerData");

            //get all the files in the PlayerData folder
            //var files = await assets.GetFilesAsync();
            //StorageFile saveFile = files[0];
            //Debug.WriteLine("saved playerData: " + files[0].Path);

            //create the autoSave.xml file
            StorageFile saveFile = await assets.CreateFileAsync("autoSave.xml", CreationCollisionOption.ReplaceExisting);
            */
        }




    }
}