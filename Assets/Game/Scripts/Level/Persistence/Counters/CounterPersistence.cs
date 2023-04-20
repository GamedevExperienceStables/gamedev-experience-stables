using CleverCrow.Fluid.UniqueIds;
using UnityEngine;

namespace Game.Level
{
    [RequireComponent(typeof(UniqueId))]
    public class CounterPersistence : MonoBehaviour, ILocationCounter
    {
        private UniqueId _uid;
        private ICounterObject _counter;

        public bool IsDirty => _counter.IsDirty;
        
        public string Id => _uid.Id;

        public int RemainingCount => _counter.RemainingCount;

        private void Awake()
        {
            _uid = GetComponent<UniqueId>();
            _counter = GetComponent<ICounterObject>();
        }

        public void SetCount(int count)
            => _counter.SetCount(count);
    }
}