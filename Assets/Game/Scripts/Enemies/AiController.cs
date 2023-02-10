using Game.Actors;
using UnityEngine;

namespace Game.Enemies
{
    public class AiController : MonoBehaviour, IActorInputController
    {
        private NavigationController _navigation;

        private Transform _target;
        private bool _hasTarget;
        private bool _inputBlocked;

        private void Awake()
            => _navigation = GetComponent<NavigationController>();

        public void SetTarget(Transform target)
        {
            _target = target;
            _hasTarget = true;
        }

        public void Update()
        {
            if (!_hasTarget)
                return;

            if (_navigation.IsValidPath())
                _navigation.MoveToPosition(_target.position);
        }

        public void BlockInput(bool isBlocked) 
            => _inputBlocked = isBlocked;
    }
}