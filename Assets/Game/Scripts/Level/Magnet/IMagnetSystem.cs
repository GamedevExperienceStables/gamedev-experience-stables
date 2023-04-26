using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Level
{
    public interface IMagnetSystem
    {
        UniTask StartPullAsync(Transform source, Transform target);
    }
}