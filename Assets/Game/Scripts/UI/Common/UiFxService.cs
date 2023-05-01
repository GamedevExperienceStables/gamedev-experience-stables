using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.UI
{
    public sealed class UiFxService
    {
        private readonly Dictionary<GameObject, GameObject> _items = new();
        private readonly GameObject _container;

        public UiFxService()
        {
            _container = new GameObject("UiFx");
        }

        public void Destroy(GameObject prefab)
        {
            if (!_items.TryGetValue(prefab, out GameObject instance))
                return;

            _items.Remove(prefab);
            Object.Destroy(instance);
        }


        public void Play(GameObject prefab)
        {
            if (!_items.TryGetValue(prefab, out GameObject instance)) 
                instance = CreateInstance(prefab);

            instance.SetActive(false);
            instance.SetActive(true);
        }

        private GameObject CreateInstance(GameObject prefab)
        {
            GameObject instance = Object.Instantiate(prefab, _container.transform);
            instance.SetActive(false);

            _items.Add(prefab, instance);

            return instance;
        }
    }
}