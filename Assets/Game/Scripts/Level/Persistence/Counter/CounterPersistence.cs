using CleverCrow.Fluid.UniqueIds;
using UnityEngine;

namespace Game.Level
{
    [RequireComponent(typeof(UniqueId))]
    public class CounterPersistence : MonoBehaviour, ILocationPersistenceInt
    {
        private UniqueId _uid;
        private ICounterObject _counter;

        public bool IsDirty => _counter.IsDirty;

        public string Id => _uid.Id;

        public int Value
        {
            get => _counter.Count;
            set
            {
                _counter.SetCount(value);
                _counter.IsDirty = true;
            }
        }

        private void Awake()
        {
            _uid = GetComponent<UniqueId>();
            _counter = GetComponent<ICounterObject>();
        }
    }
}