using Game.Actors;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemies
{
    [RequireComponent(typeof(MovementController))]
    public class NavigationController : MonoBehaviour
    {
        [SerializeField]
        private float updatePathInterval = 0.2f;

        [SerializeField]
        private float stopDistance = 1f;

        public float StopDistance => stopDistance;

        private MovementController _movement;

        private Vector3 _movementDirection;
        private Vector3 _lookDirection;

        private float _elapsedToCalculatePath;

        private NavMeshPath _path;
        private int _pathIndex;

        private NavMeshHit _hit;

        private bool IsPathIndexValid => _pathIndex < _path.corners.Length;

        private void Awake()
        {
            _movement = GetComponent<MovementController>();
            _path = new NavMeshPath();
        }

        public void MoveToPosition(Vector3 position)
        {
            _elapsedToCalculatePath += Time.deltaTime;
            if (_elapsedToCalculatePath >= updatePathInterval)
            {
                _elapsedToCalculatePath = 0;
                CalculatePath(position);
            }

            UpdateDirection();
            UpdateMovement();

#if UNITY_EDITOR
            DrawDebugNavigationPath();
#endif
        }

        public void Stop()
        {
            _path.ClearCorners();
            _pathIndex = 0;
            
            _movementDirection = Vector3.zero;
            _lookDirection = Vector3.zero;

            UpdateMovement();
        }

        public void LookTo(Vector3 direction)
        {
            _lookDirection = direction;

            UpdateMovement();
        }

        private void UpdateDirection()
        {
            _movementDirection = Vector3.zero;
            _lookDirection = Vector3.zero;

            if (!IsPathIndexValid)
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


        private void UpdateMovement()
        {
            _movement.UpdateInputs(_movementDirection, _lookDirection);
        }

        private void CalculatePath(Vector3 position)
        {
            Vector3 targetPosition = GetValidTargetPosition(position);
            NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, _path);
            
            ResetNextPathTarget();
        }

        public Vector3 GetValidTargetPosition(Vector3 position)
        {
            if (NavMesh.SamplePosition(position, out _hit, 5f, NavMesh.AllAreas))
            {
                position = _hit.position;
            }

            return position;
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

        public bool IsValidPath() 
            => true;
    }
}