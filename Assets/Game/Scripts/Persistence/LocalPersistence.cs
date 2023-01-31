using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Game.Persistence
{
    public class LocalPersistence : IPersistence
    {
        private readonly IDataSerializer _serializer;

        [Inject]
        public LocalPersistence(IDataSerializer serializer)
            => _serializer = serializer;

        public bool Exists(string filename)
            => File.Exists(GetFilePath(filename));

        public async UniTask SerializeAsync<T>(T data, string filename)
        {
            string filepath = GetFilePath(filename);

            await using var sw = new StreamWriter(filepath, false);
            await _serializer.SerializeAsync(data, sw);
        }

        public async UniTask<T> DeserializeAsync<T>(string filename)
        {
            string filepath = GetFilePath(filename);

            if (!File.Exists(filepath))
                throw new FileNotFoundException($"There is no file at the path \"{filepath}\".");

            using StreamReader sr = File.OpenText(filepath);
            var loadedData = await _serializer.DeserializeAsync<T>(sr);
            return loadedData;
        }

        public bool Delete(string filename)
        {
            string filepath = GetFilePath(filename);

            if (!File.Exists(filepath))
                return false;

            File.Delete(filepath);
            return true;
        }

        private static string GetFilePath(string filename)
            => Path.Combine(Application.persistentDataPath, filename);
    }
}