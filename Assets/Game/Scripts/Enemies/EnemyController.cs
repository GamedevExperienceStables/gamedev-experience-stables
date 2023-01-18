using Game.Actors;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemies
{
    [RequireComponent(typeof(MovementController))]
    public class EnemyController : ActorController
    {

        [SerializeField]
        private float updatePathInterval = 0.2f;

        [SerializeField]
        private float stopDistance = 1f;

        private MovementController _movement;
        private Transform _targetTransform;
        private bool _hasTarget;

        private Vector3 _movementDirection;
        private Vector3 _lookDirection;

        private float _elapsedToCalculatePath;

        private NavMeshPath _path;
        private int _pathIndex;

        private NavMeshHit _hit;

        private bool IsPathIndexValid => _pathIndex >= _path.corners.Length;

        private void Awake()
        {
            _movement = GetComponent<MovementController>();
            _path = new NavMeshPath();
        }

        private void Update()
        {
            if (!_hasTarget)
            {
                return;
            }

            _elapsedToCalculatePath += Time.deltaTime;
            if (_elapsedToCalculatePath >= updatePathInterval)
            {
                _elapsedToCalculatePath = 0;
                CalculatePath();
            }

            UpdateDirection();
            Move();

#if UNITY_EDITOR
            DrawDebugNavigationPath();
#endif
        }

        public void SetTarget(Transform target)
        {
            _targetTransform = target;
            _hasTarget = true;
        }

        public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            _movement.SetPositionAndRotation(position, rotation);
        }

        private void UpdateDirection()
        {
            _movementDirection = Vector3.zero;
            _lookDirection = Vector3.zero;

            if (IsPathIndexValid)
            {
                return;
            }

            float sqrDistance = (transform.position - _path.corners[_pathIndex]).sqrMagnitude;
            float sqrStopDistance = stopDistance * stopDistance;
            float sqrRadius = _movement.CapsuleRadius * _movement.CapsuleRadius;
            if (sqrDistance <= sqrRadius + sqrStopDistance)
            {
                _pathIndex++;
                if (_pathIndex >= _path.corners.Length)
                {
                    return;
                }
            }

            _movementDirection = (_path.corners[_pathIndex] - transform.position).normalized;
            _lookDirection = _movementDirection;
        }


        private void Move()
        {
            _movement.UpdateInputs(_movementDirection, _lookDirection);
        }

        private void CalculatePath()
        {
            Vector3 targetPosition = GetValidTargetPosition();
            NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, _path);
            
            ResetNextPathTarget();
        }

        private Vector3 GetValidTargetPosition()
        {
            Vector3 targetPosition = _targetTransform.position;
            if (NavMesh.SamplePosition(targetPosition, out _hit, 5f, NavMesh.AllAreas))
            {
                targetPosition = _hit.position;
            }

            return targetPosition;
        }

        private void DrawDebugNavigationPath()
        {
            Debug.DrawLine(transform.position, _hit.position, Color.red);
            for (int i = 0; i < _path.corners.Length - 1; i++)
            {
                Debug.DrawLine(_path.corners[i], _path.corners[i + 1], Color.blue);
            }
        }

        private void ResetNextPathTarget()
        {
            _pathIndex = 0;
        }
    }
}