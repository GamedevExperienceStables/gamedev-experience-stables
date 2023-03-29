using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Utils
{
    public class GameplayPrefabFactory
    {
        private readonly IObjectResolver _resolver;

        [Inject]
        public GameplayPrefabFactory(IObjectResolver resolver)
            => _resolver = resolver;

        public T Create<T>(T prefab) where T : MonoBehaviour
            => _resolver.Instantiate(prefab);
    }
}