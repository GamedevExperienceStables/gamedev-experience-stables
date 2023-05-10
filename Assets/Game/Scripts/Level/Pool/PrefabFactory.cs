using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Level
{
    public class PrefabFactory
    {
        private readonly IObjectResolver _resolver;

        [Inject]
        public PrefabFactory(IObjectResolver resolver)
            => _resolver = resolver;

        public T Create<T>(T prefab) where T : MonoBehaviour
            => _resolver.Instantiate(prefab);
        
        public GameObject Create(GameObject prefab) 
            => _resolver.Instantiate(prefab);
        
        public GameObject Create(GameObject prefab, Transform parent)
            => _resolver.Instantiate(prefab, parent);
    }
}