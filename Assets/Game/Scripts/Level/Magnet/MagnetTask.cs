using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Level
{
    public class MagnetTask
    {
        private readonly Transform _source;
        private readonly Transform _target;

        private readonly float _minDistance;
        private readonly float _power;

        private readonly UniTaskCompletionSource _completionSource;
        private readonly CancellationToken _token;

        public MagnetTask(Transform source, Transform target, float power, float minDistance,
            UniTaskCompletionSource completionSource)
        {
            _source = source;
            _target = target;

            _minDistance = minDistance;
            _power = power;

            _token = _source.GetCancellationTokenOnDestroy();
            _completionSource = completionSource;
            
            Completed = false;
        }

        public bool Completed { get; private set; }

        public void Execute(float deltaTime)
        {
            if (_token.IsCancellationRequested)
            {
                _completionSource.TrySetCanceled(_token);
                Completed = true;
                return;
            }

            Vector3 sourcePosition = _source.position;
            Vector3 targetPosition = _target.position;

            _source.position = Vector3.Lerp(sourcePosition, targetPosition, _power * deltaTime);

            float sqrDistance = (sourcePosition - targetPosition).sqrMagnitude;
            if (sqrDistance > _minDistance)
                return;

            Completed = true;
            _completionSource.TrySetResult();
        }
    }
}