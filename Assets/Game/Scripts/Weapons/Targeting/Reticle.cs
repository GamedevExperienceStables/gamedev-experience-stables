using System;
using UnityEngine;

namespace Game.Weapons
{
    public class Reticle : MonoBehaviour
    {
        [SerializeField]
        private float smoothTime = 0.01f;

        private bool _initialized;
        private Vector3 _targetPosition;
        private Vector3 _velocity;

        private Action _onUpdate;
    
        private void Update()
        {
            if (!_initialized)
                return;

            _onUpdate.Invoke();

            // transform.position = Vector3.SmoothDamp(transform.position, _targetPosition, ref _velocity, smoothTime);
            transform.position = _targetPosition;
        }

        public void Init(Action callback)
        {
            _onUpdate = callback;
            _initialized = true;
        }

        public void Show() 
            => gameObject.SetActive(true);

        public void Hide() 
            => gameObject.SetActive(false);

        public void SetPosition(Vector3 position)
            => _targetPosition = position;
    }
}