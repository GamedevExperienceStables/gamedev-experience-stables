using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Inventory
{
    public class MagnetTask
    {
        private readonly Transform _source;
        private readonly Transform _target;
        
        private readonly float _minDistance;
        private readonly float _power;
        
        private readonly UniTaskCompletionSource _completionSource;

        public MagnetTask(Transform source, Transform target, float power, float minDistance,
            UniTaskCompletionSource completionSource)
        {
            _source = source;
            _target = target;
            
            _minDistance = minDistance;
            _power = power;
            
            _completionSource = completionSource;
        }

        public bool Completed { get; private set; }

        public void Execute(float deltaTime)
        {
            Vector3 sourcePosition = _source.position;
            Vector3 targetPosition = _target.position;

            _source.position = Vector3.Lerp(sourcePosition, targetPosition, _power * deltaTime);

            float sqrDistance = (sourcePosition - targetPosition).sqrMagnitude;
            if (sqrDistance <= _minDistance)
            {
                Debug.Log("Completed!");
                Completed = true;

                _completionSource.TrySetResult();
            }
        }
    }
}