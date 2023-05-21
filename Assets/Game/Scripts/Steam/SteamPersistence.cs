using System.IO;
using Cysharp.Threading.Tasks;
using Game.Persistence;

namespace Game.Steam
{
    public class SteamPersistence : IPersistence
    {
        private const string DIR = "Steam";

        private readonly SteamService _steam;
        private readonly LocalPersistence _localPersistence;

        public SteamPersistence(SteamService steam, IDataSerializer serializer)
        {
            _steam = steam;

            _localPersistence = new LocalPersistence(serializer);
        }

        public bool Exists(string filename)
        {
            filename = AddedSteamId(filename);
            return _localPersistence.Exists(filename);
        }

        public UniTask SerializeAsync<T>(T data, string filename)
        {
            filename = AddedSteamId(filename);
            return _localPersistence.SerializeAsync(data, filename);
        }

        public UniTask<T> DeserializeAsync<T>(string filename)
        {
            filename = AddedSteamId(filename);
            return _localPersistence.DeserializeAsync<T>(filename);
        }

        public bool Delete(string filename)
        {
            filename = AddedSteamId(filename);
            return _localPersistence.Delete(filename);
        }


        private string AddedSteamId(string filename)
            => Path.Combine(DIR, _steam.GetSteamID(), filename);
    }
}