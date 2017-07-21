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
using Windows.Storage; //UWP
using Windows.System; //UWP

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

        public static void SetFilename(GameFile Type)
        {
            filename = "autoSave.xml"; //defaults to autoSave
            if (Type == GameFile.Game1) { filename = "game1.xml"; }
            else if (Type == GameFile.Game2) { filename = "game2.xml"; }
            else if (Type == GameFile.Game3) { filename = "game3.xml"; }
            if (Flags.PrintOutput)
            { Debug.WriteLine("folder: " + localFolder + filename); }
        }

        public static async void SaveGame(GameFile Type)
        {
            SetFilename(Type);
            //overwrite savefile
            StorageFile file = await localFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            var serializer = new XmlSerializer(typeof(SaveData));
            Stream stream = await file.OpenStreamForWriteAsync();
            using (stream) //save the playerData, to saveFile address
            { serializer.Serialize(stream, PlayerData.current); }
        }

        public static async void LoadGame(GameFile Type, Boolean loadAsCurrentGame)
        {
            SetFilename(Type);
            Boolean autoSave = false;
            Dialog dialogType = Dialog.Default;
            try
            {
                StorageFile file = await localFolder.GetFileAsync(filename);


                #region Load the file into proper saveData instance

                try
                {   //load gameFile into saveData parameter
                    if (file != null)
                    {
                        var serializer = new XmlSerializer(typeof(SaveData));
                        Stream stream = await file.OpenStreamForReadAsync();
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
                        { dialogType = Dialog.GameAutoSaved; } else { dialogType = Dialog.GameLoaded; }
                    }
                }

                #endregion


                #region Handle file loading failure

                catch
                {   //create dialog screen alerting user there was problem loading file
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
                if (Flags.PrintOutput) { Debug.WriteLine("file does not exist"); }
                SaveGame(Type); dialogType = Dialog.GameNotFound;
            }
            //if loaded data is current game, notify player of loading via dialog screen
            if (loadAsCurrentGame) { ScreenManager.AddScreen(new ScreenDialog(dialogType)); }
            //Functions_Debug.Inspect(PlayerData.saveData);
        }



        static XmlSerializer serializer = new XmlSerializer(typeof(RoomXmlData));

        public static async void SaveRoomData(RoomXmlData RoomData)
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            savePicker.FileTypeChoices.Add("RoomData", new List<string>() { ".xml" });
            savePicker.SuggestedFileName = "RoomData";
            StorageFile saveFile = await savePicker.PickSaveFileAsync();

            if (saveFile != null) //the file was created when user presses 'save' button
            {   //clear the saveFile of previous data
                await FileIO.WriteTextAsync(saveFile, "");
                //write the saveData to the empty saveFile
                Stream stream = await saveFile.OpenStreamForWriteAsync();
                using (stream) { serializer.Serialize(stream, RoomData); }
            }
        }

        public static async void SelectRoomFile(ScreenRoomBuilder RBScreen)
        {   //select a room xml file to load into RoomXmlData
            var loadPicker = new Windows.Storage.Pickers.FileOpenPicker();
            loadPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            loadPicker.FileTypeFilter.Add(".xml");
            StorageFile loadFile = await loadPicker.PickSingleFileAsync();
            if(loadFile != null)
            {   //if user selected a xml file, continue
                RBScreen.roomData = new RoomXmlData();
                Stream stream = await loadFile.OpenStreamForReadAsync();
                using (stream)
                { RBScreen.roomData = (RoomXmlData)serializer.Deserialize(stream); }
                RBScreen.BuildRoomData(RBScreen.roomData);
            }
        }

        public static async void LoadAllRoomData()
        {
            StorageFolder appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFolder RoomDataFolder = await appInstalledFolder.GetFolderAsync("RoomData");
            var roomDataFiles = await RoomDataFolder.GetFilesAsync();

            if (Flags.PrintOutput) { Debug.WriteLine("loading room data..."); }
            for (int i = 0; i < roomDataFiles.Count; i++)
            {
                if (Flags.PrintOutput) { Debug.WriteLine("filepath: " + roomDataFiles[i].Path); }
                RoomXmlData RoomData = new RoomXmlData();
                Stream stream = await roomDataFiles[i].OpenStreamForReadAsync();
                using (stream)
                { RoomData = (RoomXmlData)serializer.Deserialize(stream); }
                //place the loaded roomData into the correct Assets list
                if (RoomData.type == RoomType.Boss) { Assets.roomDataBoss.Add(RoomData); }
                else if (RoomData.type == RoomType.Column) { Assets.roomDataColumn.Add(RoomData); }
                else if (RoomData.type == RoomType.Hub) { Assets.roomDataHub.Add(RoomData); }
                else if (RoomData.type == RoomType.Key) { Assets.roomDataKey.Add(RoomData); }
                else if (RoomData.type == RoomType.Row) { Assets.roomDataRow.Add(RoomData); }
                else if (RoomData.type == RoomType.Square) { Assets.roomDataSquare.Add(RoomData); }
            }
            if (Flags.PrintOutput) { Debug.WriteLine("load complete! total: " + roomDataFiles.Count); }
        }

    }
}