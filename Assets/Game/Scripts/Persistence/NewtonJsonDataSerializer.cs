using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using VContainer;

namespace Game.Persistence
{
    public class NewtonJsonDataSerializer : IDataSerializer
    {
        private readonly JsonSerializer _serializer;

        [Inject]
        public NewtonJsonDataSerializer(bool prettyPrint = false)
        {
            _serializer = new JsonSerializer();

            if (prettyPrint)
                _serializer.Formatting = Formatting.Indented;
        }


        public UniTask SerializeAsync<T>(T data, TextWriter writer)
            => UniTask.Run(() => _serializer.Serialize(writer, data, typeof(T)));

        public UniTask<T> DeserializeAsync<T>(TextReader reader)
        {
            return UniTask.Run(() =>
            {
                using var textReader = new JsonTextReader(reader);
                var data = _serializer.Deserialize<T>(textReader);

                Debug.Assert(data != null, $"{nameof(data)} != null");

                return data;
            });
        }
    }
}