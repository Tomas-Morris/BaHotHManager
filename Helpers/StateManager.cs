using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaHotHManager.ViewModels;
using Windows.ApplicationModel;
using Windows.Storage;
using System.IO;
using Newtonsoft.Json;
using Windows.UI.Xaml.Controls;

namespace BaHotHManager.Helpers
{
    class StateManager
    {
        static readonly string currentPageKey = "CurrentPage";
        static readonly string currentPageViewModelKey = "CurrentPageViewModel";

        internal static async Task SuspendAsync(Type pageType, GameplayViewModel viewModel, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            await WriteObjectAsync(pageType, currentPageKey);
            await WriteObjectAsync(viewModel, currentPageViewModelKey);

            deferral.Complete();
        }

        private static async Task WriteObjectAsync(object o, string objectName)
        {
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(objectName, CreationCollisionOption.ReplaceExisting);
            await WriteObjectAsync(o, file);
        }
        private static async Task WriteObjectAsync(object o, StorageFile file)
        {
            using (var writer = await file.OpenStreamForWriteAsync())
            {
                await await Task.Factory.StartNew(async () =>
                {
                    var objectBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(o));
                    await writer.WriteAsync(objectBytes, 0, objectBytes.Length);
                    await writer.FlushAsync();
                });
            }
        }

        private static async Task<T> ReadObjectAsync<T>(StorageFile file)
        {
            using(var reader = await file.OpenStreamForReadAsync())
            {
                return await ReadObjectAsync<T>(reader);
            }
        }
        private static async Task<T> ReadObjectAsync<T>(string objectName)
        {
            using (var reader = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(objectName))
            {
                return await ReadObjectAsync<T>(reader);
            }
        }
        private static async Task<T> ReadObjectAsync<T>(Stream reader)
        {
            return await await Task.Factory.StartNew(async () =>
            {
                var objectBytes = new byte[reader.Length];
                await reader.ReadAsync(objectBytes, 0, (int)reader.Length);
                await reader.FlushAsync();
                return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(objectBytes));
            });
        }

        internal static async Task SaveAsync(GameplayViewModel viewModel)
        {
            var gameSaveFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("SaveGames", CreationCollisionOption.OpenIfExists);
            var gameSaveFile = await gameSaveFolder.CreateFileAsync(viewModel.Id.ToString(), CreationCollisionOption.ReplaceExisting);
            await WriteObjectAsync(viewModel, gameSaveFile);
        }

        internal static async Task<IEnumerable<GameplayViewModel>> LoadSavesAsync()
        {
            var gameSaveFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("SaveGames", CreationCollisionOption.OpenIfExists);
            var gameSaves = new List<GameplayViewModel>();
            foreach(var gameSaveFile in await gameSaveFolder.GetFilesAsync())
            {
                gameSaves.Add(await ReadObjectAsync<GameplayViewModel>(gameSaveFile));
            }
            return gameSaves;
        }

        internal static async Task ReviveAsync(Frame rootFrame)
        {
            var pageType = await ReadObjectAsync<Type>(currentPageKey);
            var pageViewModel = await ReadObjectAsync<GameplayViewModel>(currentPageViewModelKey);

            if (pageType != null && pageViewModel != null)
                rootFrame.Navigate(pageType, pageViewModel);
        }
    }
}
