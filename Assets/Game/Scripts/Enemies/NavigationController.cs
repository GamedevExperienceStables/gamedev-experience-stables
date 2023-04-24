using Game.Actors;
using Game.Utils;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemies
{
    [RequireComponent(typeof(MovementController))]
    public class NavigationController : MonoBehaviour
    {
        [SerializeField]
        private float stopDistance = 1f;

        [UsedImplicitly]
        public float StopDistance => stopDistance;

        private MovementController _movement;
        private NavMeshAgent _agent;

        private Vector3 _movementDirection;

        public bool IsCompleted => !_agent.pathPending && IsReached(_agent.pathEndPosition);

        private void Awake()
        {
            _movement = GetComponent<MovementController>();
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            // prevent updating NavMeshAgent itself
            _agent.updatePosition = false;
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
            
            _agent.stoppingDistance = stopDistance;
            _agent.radius = _movement.CapsuleRadius;
            _agent.height = _movement.CapsuleHeight;
            _agent.avoidancePriority = Random.Range(50, 90);
        }

        public void Tick()
        {
            _agent.speed = _movement.Speed;
            _agent.nextPosition = transform.position;
            _agent.velocity = _movement.Velocity;

            _movementDirection = _agent.desiredVelocity.normalized;
            
            _movement.UpdateInputs(_movementDirection, _movementDirection);
        }

        public void Stop()
        {
            if (_agent.isOnNavMesh)
                _agent.ResetPath();
            
            _movementDirection = Vector3.zero;

            _movement.UpdateInputs(_movementDirection, _movementDirection);
        }

        public void LookTo(Vector3 direction) 
            => _movement.UpdateInputs(_movementDirection, direction);

        public bool SetDestination(Vector3 target)
            => _agent.SetDestination(target);

        public bool IsReached(Vector3 target)
            => transform.position.AlmostEquals(target, stopDistance);
    }
}