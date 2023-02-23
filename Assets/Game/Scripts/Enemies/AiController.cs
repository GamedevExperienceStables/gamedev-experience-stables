using Game.Actors;
using UnityEngine;

namespace Game.Enemies
{
    public class AiController : MonoBehaviour, IActorInputController
    {
        private Transform _target;
        
        private bool _inputBlocked;

        public Transform Target => _target;

        public void SetTarget(Transform target)
            => _target = target;

        public void BlockInput(bool isBlocked) 
            => _inputBlocked = isBlocked;
    }
}