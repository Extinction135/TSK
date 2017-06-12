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

        public static async void SavePlayerData()
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            savePicker.FileTypeChoices.Add("PlayerDataSave", new List<string>() { ".xml" });
            savePicker.SuggestedFileName = "PlayerDataSave";
            StorageFile newfile = await savePicker.PickSaveFileAsync();

            var serializer = new XmlSerializer(typeof(SaveData));
            Stream stream = await newfile.OpenStreamForWriteAsync();
            using (stream)
            { serializer.Serialize(stream, PlayerData.saveData); }
        }

        public static async void LoadPlayerData()
        {
            var loadPicker = new Windows.Storage.Pickers.FileOpenPicker();
            loadPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            loadPicker.FileTypeFilter.Add(".xml");

            StorageFile loadFile = await loadPicker.PickSingleFileAsync();
            if (loadFile != null)
            {
                var serializer = new XmlSerializer(typeof(SaveData));
                Stream stream = await loadFile.OpenStreamForReadAsync();
                using (stream)
                { PlayerData.saveData = (SaveData)serializer.Deserialize(stream); }
                Functions_Debug.Inspect(PlayerData.saveData);
            }
        }

    }
}