using Cysharp.Threading.Tasks;

namespace Game.Persistence
{
    public interface IPersistence
    {
        bool Exists(string filename);
        UniTask SerializeAsync<T>(T data, string filename);
        UniTask<T> DeserializeAsync<T>(string filename);
        bool Delete(string filename);
    }
}