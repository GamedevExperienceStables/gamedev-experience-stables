using System;
using UnityEngine;

namespace Game.Level
{
    public class SpawnPoolTarget : MonoBehaviour
    {
        private Action<SpawnPoolTarget> _onDisable;
        public int OriginId { get; private set; }

        public void Init(int originId, Action<SpawnPoolTarget> actionOnDisable)
        {
            _onDisable = actionOnDisable;
            
            OriginId = originId;
        }

        public void Activate(Vector3 position, Quaternion rotation)
        {
            gameObject.transform.SetPositionAndRotation(position, rotation);
            gameObject.SetActive(true);
        }

        public void OnDisable() 
            => _onDisable?.Invoke(this);
    }
}