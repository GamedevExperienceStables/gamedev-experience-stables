using System;
using Game.Actors;
using Game.TimeManagement;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace Game.Weapons
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private float raycastRadius = 0.1f;

        [FormerlySerializedAs("deathFeedback")]
        [SerializeField]
        private GameObject deathFeedbackPrefab;

        private Vector3 _velocity;

        private float _timer2;
        private TimerUpdatable _timer;

        private readonly RaycastHit[] _hits = new RaycastHit[5];
        private IActorController _owner;

        private IProjectileSettings _settings;
        private ProjectileBehaviour _behaviour;
        private TimerPool _timers;

        public event Action<Projectile> Completed;
        
        private void FixedUpdate()
        {
            float time = Time.fixedDeltaTime;

            UpdateVelocity();
            UpdatePosition(time);
            UpdateRotation();

            if (DetectCollisions(out CollisionData collision))
                DestroyProjectile(collision);
        }

        [Inject]
        public void Construct(ProjectileBehaviour behaviour, TimerPool timers)
        {
            _behaviour = behaviour;

            _timers = timers;
            _timer = _timers.GetTimer();
        }

        private void OnDisable() 
            => _timer?.Stop();

        private void OnDestroy() 
            => _timers?.ReleaseTimer(_timer);

        public void Init(IProjectileSettings settings)
        {
            _settings = settings;
            _timer.Init(TimeSpan.FromSeconds(_settings.LifeTime.Duration), OnLifetimeEnd);
        }

        public void Fire(Transform spawnPoint, Vector3 targetPosition)
        {
            Vector3 startPosition = spawnPoint.position;
            Quaternion startRotation = spawnPoint.rotation;
            
            if (targetPosition != Vector3.zero)
            {
                float coord1 = targetPosition.x - startPosition.x;
                float coord2 = targetPosition.z - startPosition.z;
                float angle = Mathf.Atan2(coord1, coord2) * Mathf.Rad2Deg;
                startRotation = Quaternion.Euler(new Vector3(0, angle, 0));
            }

            Transform t = transform;
            t.SetPositionAndRotation(startPosition, startRotation);

            _velocity = _settings.Trajectory.GetInitialDirection(t) * _settings.Speed;

            _timer.Start();
            Show();
        }

        private void UpdateRotation()
            => transform.forward = _velocity;

        private void UpdatePosition(float deltaTime)
            => transform.Translate(_velocity * deltaTime, Space.World);

        private void UpdateVelocity()
            => _velocity = _settings.Trajectory.CalculateVelocity(_velocity);

        private void OnLifetimeEnd()
        {
            switch (_settings.LifeTime.OnEndBehaviour)
            {
                case ProjectileLifetime.EndBehaviour.Execute:
                {
                    Transform t = transform;
                    var collision = new CollisionData
                    {
                        target = t,
                        hitPoint = t.position,
                        hitNormal = t.up
                    };
                    DestroyProjectile(collision);
                    break;
                }

                default:
                {
                    Complete();
                    break;
                }
            }
        }

        private void PlayDeathFeedback(Vector3 position, Vector3 normal)
        {
            if (deathFeedbackPrefab)
                Instantiate(deathFeedbackPrefab, position, Quaternion.LookRotation(normal));
        }


        private bool DetectCollisions(out CollisionData collision)
        {
            Transform projectileTransform = transform;
            Vector3 projectilePosition = projectileTransform.position;
            var ray = new Ray(projectilePosition, projectileTransform.forward);
            int count = Physics.SphereCastNonAlloc(ray, raycastRadius, _hits,
                raycastRadius, _settings.CollisionLayers, QueryTriggerInteraction.Ignore);
            if (count <= 0)
            {
                collision = default;
                return false;
            }

            RaycastHit raycastHit = _hits[0];
            Transform collisionTransform = raycastHit.transform;
            Vector3 normal = (projectilePosition - collisionTransform.position).normalized;
            collision = new CollisionData
            {
                target = collisionTransform,
                hitPoint = projectilePosition,
                hitNormal = normal
            };
            return true;
        }

        private void DestroyProjectile(CollisionData collision)
        {
            foreach (DamageDefinition damage in _settings.Damages)
                _behaviour.Execute(transform, damage, collision);

            PlayDeathFeedback(collision.hitPoint, collision.hitNormal);
            Complete();
        }

        private void Complete()
        {
            Completed?.Invoke(this);
            Hide();
        }

        private void Show()
            => gameObject.SetActive(true);

        private void Hide()
            => gameObject.SetActive(false);

        private void OnDrawGizmosSelected()
            => Gizmos.DrawSphere(transform.position, raycastRadius);
    }
}