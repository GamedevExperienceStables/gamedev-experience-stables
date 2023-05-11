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

        [SerializeField]
        private float additiveAvoidance = 0.25f;

        [UsedImplicitly]
        public float StopDistance => stopDistance;

        private MovementController _movement;
        private NavMeshAgent _agent;

        private Vector3 _movementDirection;

        public bool IsCompleted
        {
            get
            {
                if (!_agent.isOnNavMesh) 
                    return true;
                
                return !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance;
            }
        }

        private void Awake()
        {
            _movement = GetComponent<MovementController>();
            _agent = GetComponent<NavMeshAgent>();
            _agent.enabled = false;
        }

        private void Start()
        {
            // prevent updating NavMeshAgent itself
            _agent.updatePosition = false;
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
            
            _agent.stoppingDistance = stopDistance;
            _agent.radius = _movement.CapsuleRadius + additiveAvoidance;
            _agent.height = _movement.CapsuleHeight;
        }

        public void Tick()
        {
            _agent.speed = _movement.Speed;
            _agent.nextPosition = transform.position;
            _agent.velocity = _movement.Velocity;

            _movementDirection = _agent.desiredVelocity.normalized;
            
            _movement.UpdateInputs(_movementDirection, _movementDirection);
            
            #if UNITY_EDITOR
                Debug.DrawLine(transform.position, _agent.steeringTarget, Color.red);
            #endif
        }

        public void Stop()
        {
            if (_agent.isOnNavMesh)
                _agent.ResetPath();

            _agent.enabled = false;

            _movementDirection = Vector3.zero;

            _movement.UpdateInputs(_movementDirection, _movementDirection);
        }

        public void LookTo(Vector3 direction) 
            => _movement.UpdateInputs(_movementDirection, direction);

        public bool SetDestination(Vector3 target)
        {
            _agent.enabled = true;
            return _agent.SetDestination(target);
        }

        public bool IsReached(Vector3 target)
            => transform.position.AlmostEquals(target, stopDistance);
    }
}