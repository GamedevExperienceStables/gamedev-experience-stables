using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Game.Persistence
{
    public class LocalPersistence : IPersistence
    {
        private readonly IDataSerializer _serializer;
        private const string DIRECTORY = "Saves";

        [Inject]
        public LocalPersistence(IDataSerializer serializer)
            => _serializer = serializer;

        public bool Exists(string filename)
            => File.Exists(GetOrCreateFilePath(filename));

        public async UniTask SerializeAsync<T>(T data, string filename)
        {
            string filepath = GetOrCreateFilePath(filename);

            await using var sw = new StreamWriter(filepath, false);
            await _serializer.SerializeAsync(data, sw);
        }

        public async UniTask<T> DeserializeAsync<T>(string filename)
        {
            string filepath = GetOrCreateFilePath(filename);

            if (!File.Exists(filepath))
                throw new FileNotFoundException($"There is no file at the path \"{filepath}\".");

            using StreamReader sr = File.OpenText(filepath);
            var loadedData = await _serializer.DeserializeAsync<T>(sr);
            return loadedData;
        }

        public bool Delete(string filename)
        {
            string filepath = GetOrCreateFilePath(filename);

            if (!File.Exists(filepath))
                return false;

            File.Delete(filepath);
            return true;
        }

        private static string GetOrCreateFilePath(string filename)
        {
            string filePath = Path.Combine(Application.persistentDataPath, DIRECTORY, filename);

            string dirPath = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dirPath))
                Directory.CreateDirectory(dirPath);

            return filePath;
        }
    }
}