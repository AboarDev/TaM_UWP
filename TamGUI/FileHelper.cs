using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamGui
{
    class FileHelper<T>
    {
        const string FileExtention = ".json";
        static Windows.Storage.StorageFolder storageFolder;
        private readonly string FilePath;
        static FileHelper()
        {
            storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        }

        public FileHelper(string path)
        {
            FilePath = $"{path}{FileExtention}";
        }

        public async void MakeFile (T t)
        {
            string toAppend = JsonConvert.SerializeObject(t);
            Windows.Storage.StorageFile file = await storageFolder.CreateFileAsync(FilePath,
            Windows.Storage.CreationCollisionOption.OpenIfExists);
            await Windows.Storage.FileIO.WriteTextAsync(file, toAppend);
        }

        public async Task<string> LoadFile ()
        {
            Windows.Storage.StorageFile file = await storageFolder.GetFileAsync(FilePath);
            string toParse = await Windows.Storage.FileIO.ReadTextAsync(file);
            return toParse;
        }

        public async Task<T> ParseFile(string toParse)
        {
            T alt = JsonConvert.DeserializeObject<T>(toParse);
            await Task.Delay(0);
            return alt;
        }
    }
}
