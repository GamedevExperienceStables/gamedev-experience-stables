using System.IO;
using Cysharp.Threading.Tasks;

namespace Game.Persistence
{
    public interface IDataSerializer
    {
        UniTask SerializeAsync<T>(T data, TextWriter writer);
        UniTask<T> DeserializeAsync<T>(TextReader reader);
    }
}