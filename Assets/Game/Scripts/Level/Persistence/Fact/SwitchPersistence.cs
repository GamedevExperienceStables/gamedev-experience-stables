using CleverCrow.Fluid.UniqueIds;
using UnityEngine;

namespace Game.Level
{
    [RequireComponent(typeof(UniqueId))]
    public class SwitchPersistence : MonoBehaviour, ILocationPersistenceInt
    {
        private UniqueId _uid;
        private ISwitchObject _switch;

        public string Id => _uid.Id;
        
        public bool IsDirty => _switch.IsDirty;
        
        public int Value
        {
            get => _switch.IsActive ? 1 : 0;
            set
            {
                _switch.IsActive = value == 1;
                _switch.IsDirty = true;
            }
        }

        private void Awake()
        {
            _uid = GetComponent<UniqueId>();
            _switch = GetComponent<ISwitchObject>();
        }
    }
}