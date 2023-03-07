using System;
using Game.Actors;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Weapons
{
    public class Projectile : MonoBehaviour
    {
        private struct CollisionData
        {
            public Transform target;
            public Vector3 hitPoint;
            public Vector3 hitNormal;
        }

        [SerializeField]
        private float raycastRadius = 0.1f;

        [FormerlySerializedAs("deathFeedback")]
        [SerializeField]
        private GameObject deathFeedbackPrefab;

        private Vector3 _velocity;

        private float _timer;

        private readonly RaycastHit[] _hits = new RaycastHit[5];
        private IActorController _owner;
        
        private IProjectileSettings _settings;

        public event Action<Projectile> Completed;

        private void Update() 
            => UpdateLifeTimer();

        private void FixedUpdate()
        {
            float time = Time.fixedDeltaTime;
            
            UpdateVelocity();
            UpdatePosition(time);
            UpdateRotation();
            
            if (DetectCollisions(out CollisionData collision))
                DestroyProjectile(collision);
        }

        public void Init(IProjectileSettings settings) 
            => _settings = settings;

        public void Fire(Transform spawnPoint)
        {
            _timer = _settings.LifeTime.Duration;

            Transform t = transform;
            t.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);

            _velocity = _settings.Trajectory.GetInitialDirection(t) * _settings.Speed;

            Show();
        }

        private void UpdateRotation()
            => transform.forward = _velocity;
        
        private void UpdatePosition(float deltaTime) 
            => transform.Translate(_velocity * deltaTime, Space.World);

        private void UpdateVelocity() 
            => _velocity = _settings.Trajectory.CalculateVelocity(_velocity);

        private void UpdateLifeTimer()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
                OnLifetimeEnd();
        }

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
                damage.TryDealDamage(transform, collision.target, collision.hitPoint);

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