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
        static string filename;



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



        public static void SetFilename(GameFile Type)
        {
            filename = "autoSave.xml"; //defaults to autoSave
            if (Type == GameFile.Game1) { filename = "game1.xml"; }
            else if (Type == GameFile.Game2) { filename = "game2.xml"; }
            else if (Type == GameFile.Game3) { filename = "game3.xml"; }
        }

        public static async void SaveGame(GameFile Type)
        {
            SetFilename(Type);
            Debug.WriteLine("saving: " + localFolder.Path + @"\" + filename);
            StorageFile file = await localFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            SavePlayerData(file);
        }

        public static async void LoadGame(GameFile Type)
        {
            SetFilename(Type);
            Debug.WriteLine("loading: " + localFolder.Path + @"\" + filename);
            try
            {
                StorageFile file = await localFolder.GetFileAsync(filename);
                LoadPlayerData(file);
            }
            catch
            {   //file does not exist, cannot be loaded
                SaveGame(Type); //so save the current data to file address
                ScreenManager.AddScreen(new ScreenDialog(Dialog.GameNotFound));
            }
        }
  


        public static async void SavePlayerData(StorageFile gameFile)
        {   //save the playerData, to saveFile address
            var serializer = new XmlSerializer(typeof(SaveData));
            Stream stream = await gameFile.OpenStreamForWriteAsync();
            using (stream)
            { serializer.Serialize(stream, PlayerData.saveData); }
        }

        public static async void LoadPlayerData(StorageFile gameFile)
        {
            try
            {   //load gameFile into PlayerData.saveData
                if (gameFile != null)
                {
                    var serializer = new XmlSerializer(typeof(SaveData));
                    Stream stream = await gameFile.OpenStreamForReadAsync();
                    using (stream)
                    { PlayerData.saveData = (SaveData)serializer.Deserialize(stream); }
                    //create dialog screen, let player know file has been loaded
                    ScreenManager.AddScreen(new ScreenDialog(Dialog.GameLoaded));
                }
            }
            catch
            {   //create a dialog screen alerting user there was a problem loading the saved game file
                ScreenManager.AddScreen(new ScreenDialog(Dialog.GameLoadFailed));
                //Debug.WriteLine("loading: " + localFolder.Path + @"\" + filename);
            }
            finally
            {
                //inspect the loaded saveData
                //Functions_Debug.Inspect(PlayerData.saveData);
            }
        }



        /*
        //pick the path and name to save the file to
        var savePicker = new Windows.Storage.Pickers.FileSavePicker();
        savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
        savePicker.FileTypeChoices.Add("PlayerDataSave", new List<string>() { ".xml" });
        savePicker.SuggestedFileName = "PlayerDataSave";
        StorageFile saveFile = await savePicker.PickSaveFileAsync();
        */

        /*
        //pick the playerData.xml file to load
        var loadPicker = new Windows.Storage.Pickers.FileOpenPicker();
        loadPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
        loadPicker.FileTypeFilter.Add(".xml");
        StorageFile loadFile = await loadPicker.PickSingleFileAsync();
        */

        /*
        //get the PlayerData folder path
        StorageFolder appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
        StorageFolder assets = await appInstalledFolder.GetFolderAsync("PlayerData");

        //get all the files in the PlayerData folder
        //var files = await assets.GetFilesAsync();
        //StorageFile saveFile = files[0];
        //Debug.WriteLine("saved playerData: " + files[0].Path);
        */

        /*
        //load the 1st playerData.xml file from PlayerData folder
        var files = await assets.GetFilesAsync();
        StorageFile loadFile = files[0];
        Debug.WriteLine("loaded playerData: " + files[0].Path);
        */
    }
}