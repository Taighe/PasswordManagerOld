using PasswordManager.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace PasswordManager.Utilities
{
    public static class ImportExportUtil
    {
        public const string FILE_NAME = "pmDB.json";
        public async static Task<IEnumerable<PasswordItem>> ImportAsync(string path)
        {
            IEnumerable<PasswordItem> items = null;
            using (TextReader reader = TextReader.Synchronized(new StreamReader(path)))
            {
                string json = await reader.ReadToEndAsync();
                items = JsonConvert.DeserializeObject<IEnumerable<PasswordItem>>(json);
            }

            return items;
        }
        public async static Task<string> ExportAsync(IEnumerable<PasswordItem> items)
        {
            string json = JsonConvert.SerializeObject(items);
            string pathToSave = Path.Combine(FileSystem.CacheDirectory, FILE_NAME);
            using (TextWriter writer = TextWriter.Synchronized(new StreamWriter(pathToSave)))
            {
                await writer.WriteAsync(json);
            }

            return pathToSave;
        }
    }
}
