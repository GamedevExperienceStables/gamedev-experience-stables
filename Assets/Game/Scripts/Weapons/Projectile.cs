using System;
using System.Collections.Generic;
using Game.Actors;
using Game.Utils;
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
        private float maxSpeed = 200f;

        [SerializeField]
        private float acceleration;

        [SerializeField]
        private float lifeTime = 3f;

        [SerializeField]
        private float raycastMaxDistance = 0.4f;

        [SerializeField]
        private float raycastRadius = 0.1f;

        [SerializeField]
        private LayerMask collisionLayerMask;

        [FormerlySerializedAs("deathFeedback")]
        [SerializeField]
        private GameObject deathFeedbackPrefab;

        private float _currentSpeed;

        private Vector3 _velocity;

        private float _timer;

        private readonly RaycastHit[] _hits = new RaycastHit[5];
        private IActorController _owner;

        private ProjectileLifetime _lifeTime;
        private IList<DamageDefinition> _damages;
        
        public event Action<Projectile> Completed;

        private void Update()
        {
            UpdateLifeTimer();
        }

        private void FixedUpdate()
        {
            CalculateSpeed();
            Move();
            
            if (DetectCollisions(out CollisionData collision))
                DestroyProjectile(collision);
        }

        private bool DetectCollisions(out CollisionData collision)
        {
            Transform projectileTransform = transform;
            Vector3 projectilePosition = projectileTransform.position;
            var ray = new Ray(projectilePosition, projectileTransform.forward);
            int count = Physics.SphereCastNonAlloc(ray, raycastRadius, _hits,
                raycastMaxDistance, collisionLayerMask, QueryTriggerInteraction.Ignore);
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
            bool damaged = false;

            foreach (DamageDefinition damage in _damages)
                damaged |= damage.TryDealDamage(transform, collision.target, collision.hitPoint);

            if (damaged)
                Complete();
            else
                DestroyOnCollision(collision.hitPoint, collision.hitNormal);
        }

        private void DestroyOnCollision(Vector3 position, Vector3 normal)
        {
            PlayDeathFeedback(position, normal);
            Complete();
        }

        public void Init(Transform startPoint, IActorController owner)
        {
            _owner = owner;
            transform.SetPositionAndRotation(startPoint.position, startPoint.rotation);

            _timer = lifeTime;
            _currentSpeed = acceleration > 0 ? 0 : maxSpeed;
        }

        public void Init(LayerMask collisionLayers, float speed, ProjectileLifetime lifeTime,
            IList<DamageDefinition> damages)
        {
            collisionLayerMask = collisionLayers;
            maxSpeed = speed;
            _lifeTime = lifeTime;
            _damages = damages;
        }

        public void Fire(Transform spawnPoint)
        {
            transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);

            _timer = _lifeTime.Duration;
            _currentSpeed = acceleration > 0 ? 0 : maxSpeed;

            Show();
        }

        private void Complete()
        {
            Completed?.Invoke(this);
            Hide();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Move()
        {
            transform.Translate(_velocity, Space.World);
        }

        private void CalculateSpeed()
        {
            _velocity = transform.forward * (_currentSpeed * Time.fixedDeltaTime);
            if (!MathExtensions.AlmostEquals(_currentSpeed, maxSpeed))
            {
                _currentSpeed += acceleration * Time.fixedDeltaTime;
            }
        }

        private void UpdateLifeTimer()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
                OnLifetimeEnd();
        }

        private void OnLifetimeEnd()
        {
            switch (_lifeTime.OnEndBehaviour)
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

        private void OnDrawGizmosSelected()
            => Gizmos.DrawSphere(transform.position, raycastRadius);
    }
}