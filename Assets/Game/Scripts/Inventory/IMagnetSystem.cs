using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Inventory
{
    public interface IMagnetSystem
    {
        UniTask StartPullAsync(Transform source, Transform target);
    }
}