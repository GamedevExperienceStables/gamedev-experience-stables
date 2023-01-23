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

        public UniTask SerializeAsync<T>(T data, string filename)
        {
            string filepath = GetFilePath(filename);

            using var sw = new StreamWriter(filepath, false);
            return _serializer.SerializeAsync(data, sw);
        }

        public UniTask<T> DeserializeAsync<T>(string filename)
        {
            string filepath = GetFilePath(filename);

            if (!File.Exists(filepath))
                throw new FileNotFoundException($"There is no file at the path \"{filepath}\".");

            using StreamReader sr = File.OpenText(filepath);
            return _serializer.DeserializeAsync<T>(sr);
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