using CleverCrow.Fluid.UniqueIds;
using UnityEngine;

namespace Game.Level
{
    public class ActivePersistence : MonoBehaviour, ILocationPersistenceFact
    {
        private enum State
        {
            Inactive = 0,
            Active = 1,
        }

        [SerializeField]
        private State targetState = State.Active;

        private UniqueId _uid;
        private ISwitchObject _switch;

        public string Id => _uid.Id;

        public bool IsConfirmed => _switch.IsActive == ConvertToBool(targetState);

        public void Confirm()
        {
            _switch.IsActive = ConvertToBool(targetState);
            _switch.IsDirty = true;
        }

        private void Awake()
        {
            _uid = GetComponent<UniqueId>();
            _switch = GetComponent<ISwitchObject>();
        }

        private static bool ConvertToBool(State state)
            => state == State.Active;
    }
}